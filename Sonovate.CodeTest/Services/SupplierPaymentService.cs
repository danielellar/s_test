using CsvHelper;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sonovate.CodeTest.Services
{
    internal class SupplierPaymentService : ISupplierPaymentService
    {
        private const string NOT_AVAILABLE = "NOT AVAILABLE";
        private const string LOAD_CANDIDATE_ERROR = "Could not load candidate with Id {0}";

        private const string NO_SUPPLIER_TRANSACTIONS_FORMAT = "No supplier invoice transactions found between dates {0} to {1}";
        private const string BACS_RESULT_REFERENCE_DATE_FORMAT = "ddMMyyyy";
        private const string BACS_RESULT_REFERENCE_FORMAT = "SONOVATE{0}";
        private const string BACS_EXPORT_CSV_FORMAT = "{0}_BACSExport.csv";

        ISupplierBacsExport ISupplierPaymentService.GetSupplierPayments(DateTime startDate, DateTime endDate)
        {
            var invoiceTransactions = new InvoiceTransactionRepository();
            var candidateInvoiceTransactions = (IList<IInvoiceTransaction>)invoiceTransactions.GetBetweenDates(startDate, endDate);

            if (!candidateInvoiceTransactions.Any())
            {
                throw new InvalidOperationException(string.Format(NO_SUPPLIER_TRANSACTIONS_FORMAT, startDate, endDate));
            }

            var candidateBacsExport = CreateCandidateBacxExportFromSupplierPayments(candidateInvoiceTransactions);

            return candidateBacsExport;
        }

        void ISupplierPaymentService.SaveSupplierBacsExport(ISupplierBacsExport supplierBacsExport)
        {
            var fileName = string.Format(BACS_EXPORT_CSV_FORMAT, BacsExportType.Supplier);

            using (var csv = new CsvWriter(new StreamWriter(new FileStream(fileName, FileMode.Create))))
            {
                csv.WriteRecords(supplierBacsExport.SupplierPayment);
            }
        }

        private SupplierBacsExport CreateCandidateBacxExportFromSupplierPayments(IList<IInvoiceTransaction> supplierPayments)
        {
            var candidateBacsExport = new SupplierBacsExport
            {
                SupplierPayment = new List<ISupplierBacs>()
            };

            candidateBacsExport.SupplierPayment = BuildSupplierPayments(supplierPayments);

            return candidateBacsExport;
        }

        private List<ISupplierBacs> BuildSupplierPayments(IEnumerable<IInvoiceTransaction> invoiceTransactions)
        {
            var results = new List<ISupplierBacs>();

            var transactionsByCandidateAndInvoiceId = invoiceTransactions.GroupBy(transaction => new
            {
                transaction.InvoiceId,
                transaction.SupplierId
            });

            foreach (var transactionGroup in transactionsByCandidateAndInvoiceId)
            {
                var candidateRepository = new CandidateRepository();
                var candidate = candidateRepository.GetById(transactionGroup.Key.SupplierId);

                if (candidate == null)
                {
                    throw new InvalidOperationException(string.Format(LOAD_CANDIDATE_ERROR,
                        transactionGroup.Key.SupplierId));
                }

                var result = new SupplierBacs();

                var bank = candidate.BankDetails;

                result.BankDetails.AccountName = bank.AccountName;
                result.BankDetails.AccountNumber = bank.AccountNumber;
                result.BankDetails.SortCode = bank.SortCode;
                result.PaymentAmount = transactionGroup.Sum(invoiceTransaction => invoiceTransaction.Gross);
                result.InvoiceReference = string.IsNullOrEmpty(transactionGroup.First().InvoiceReference)
                    ? NOT_AVAILABLE
                    : transactionGroup.First().InvoiceReference;
                result.PaymentReference = string.Format(BACS_RESULT_REFERENCE_FORMAT,
                    transactionGroup.First().InvoiceDate.GetValueOrDefault().ToString(BACS_RESULT_REFERENCE_DATE_FORMAT));

                results.Add(result);
            }

            return results;
        }
    }
}
