using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Sonovate.CodeTest.Configuration;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Interfaces;
using Sonovate.CodeTest.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sonovate.CodeTest
{
    public class BacsExportService
    {
        //TODO: Move strings to resource file
        //TODO: Add logging
        //TODO: Move dependencies to Application class and instantiate using dependency injection.
        //See new constructor.
        //TODO: Add tests that cover all paths, using DI and mocking.
        private const string APP_SETTINGS_PATH = "appsettings.json";
        private const string DOCUMENT_STORE_KEY = "documentStore";
        private const string AGENCY_PAYMENTS_KEY = "agencyPayments";

        private const string INVALID_BACSEXPORTTYPE_ERROR = "Invalid BACS Export Type.";

        private readonly IDocumentStore documentStore;
        private readonly IAgencyPaymentsConfiguration agencyPaymentsConfiguration;
        private readonly IAgencyPaymentService agencyPaymentService;
        private readonly ISupplierPaymentService supplierPaymentService;

        public BacsExportService()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
             .AddJsonFile(APP_SETTINGS_PATH)
             .Build();

            var documentStoreConfig = new DocumentStoreConfiguration();
            agencyPaymentsConfiguration = new AgencyPaymentsConfiguration();
            Configuration
                .GetSection(DOCUMENT_STORE_KEY)
                .Bind(documentStoreConfig);
            Configuration
                .GetSection(AGENCY_PAYMENTS_KEY)
                .Bind(agencyPaymentsConfiguration);

            documentStore = new DocumentStore
            {
                Urls = new[] { documentStoreConfig.Url },
                Database = documentStoreConfig.Database
            };

            documentStore.Initialize();

            agencyPaymentService = new AgencyPaymentService(documentStore);
            supplierPaymentService = new SupplierPaymentService();
        }

        public BacsExportService(IDocumentStore documentStore, IAgencyPaymentsConfiguration agencyPaymentsConfiguration)
        {
            this.agencyPaymentsConfiguration = agencyPaymentsConfiguration;
            this.documentStore = documentStore;
            agencyPaymentService = new AgencyPaymentService(documentStore);
            supplierPaymentService = new SupplierPaymentService();

            documentStore.Initialize();
        }

        public async Task ExportZip(BacsExportType bacsExportType)
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            try
            {
                List<IBacsResult> payments;
                switch (bacsExportType)
                {
                    case BacsExportType.Agency:
                        if (agencyPaymentsConfiguration.Enabled)
                        {
                            payments = await agencyPaymentService.GetAgencyPayments(startDate, endDate);
                            agencyPaymentService.SavePayments(payments, bacsExportType);
                        }

                        break;
                    case BacsExportType.Supplier:
                        var supplierBacsExport = supplierPaymentService.GetSupplierPayments(startDate, endDate);
                        supplierPaymentService.SaveSupplierBacsExport(supplierBacsExport);
                        break;
                    default:
                        throw new Exception(INVALID_BACSEXPORTTYPE_ERROR);
                }

            }
            catch (InvalidOperationException inOpEx)
            {
                throw new Exception(inOpEx.Message);
            }
        }
    }
}