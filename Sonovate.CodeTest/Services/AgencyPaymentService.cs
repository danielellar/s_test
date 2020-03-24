using CsvHelper;
using Raven.Client.Documents;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sonovate.CodeTest.Services
{
    internal class AgencyPaymentService : IAgencyPaymentService
    {
        private const string NO_AGENCY_PAYMENTS_FORMAT = "No agency payments found between dates {0:dd/MM/yyyy} to {1:dd/MM/yyyy}";
        private const string BACS_RESULT_REFERENCE_DATE_FORMAT = "ddMMyyyy";
        private const string BACS_RESULT_REFERENCE_FORMAT = "SONOVATE{0}";
        private const string BACS_EXPORT_CSV_FORMAT = "{0}_BACSExport.csv";

        private readonly IDocumentStore documentStore;

        public AgencyPaymentService(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        async Task<List<IBacsResult>> IAgencyPaymentService.GetAgencyPayments(DateTime startDate, DateTime endDate)
        {
            var paymentRepository = new PaymentsRepository();
            var payments = paymentRepository.GetBetweenDates(startDate, endDate);

            if (!payments.Any())
            {
                throw new InvalidOperationException(string.Format(NO_AGENCY_PAYMENTS_FORMAT, startDate, endDate));
            }

            var agencies = await GetAgenciesForPayments(payments);

            return BuildAgencyPayments(payments, agencies);
        }

        void IAgencyPaymentService.SavePayments(IEnumerable<IBacsResult> payments, BacsExportType type)
        {
            var filename = string.Format(BACS_EXPORT_CSV_FORMAT, type);

            using var csv = new CsvWriter(new StreamWriter(new FileStream(filename, FileMode.Create)));
            csv.WriteRecords(payments);
        }

        private async Task<List<IAgency>> GetAgenciesForPayments(IList<Payment> payments)
        {
            var agencyIds = payments.Select(x => x.AgencyId).Distinct().ToList();

            using var session = documentStore.OpenAsyncSession();
            return (await session.LoadAsync<IAgency>(agencyIds)).Values.ToList();
        }

        private List<IBacsResult> BuildAgencyPayments(IEnumerable<IPayment> payments, List<IAgency> agencies)
        {
            return (from p in payments
                    let agency = agencies.FirstOrDefault(x => x.Id == p.AgencyId)
                    where agency != null && agency.BankDetails != null
                    let bank = agency.BankDetails
                    select new BacsResult
                    {
                        BankDetails = new BankDetails
                        {
                            AccountName = bank.AccountName,
                            AccountNumber = bank.AccountNumber,
                            SortCode = bank.SortCode
                        },
                        Amount = p.Balance,
                        Reference = string.Format(BACS_RESULT_REFERENCE_FORMAT, p.PaymentDate.ToString(BACS_RESULT_REFERENCE_DATE_FORMAT))
                    }).ToList<IBacsResult>();
        }
    }
}
