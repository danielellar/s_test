using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Documents;
using Sonovate.CodeTest.Configuration;
using Sonovate.CodeTest.Domain;
using System;

namespace Sonovate.CodeTest.Tests
{
    [TestClass]
    public class BacsExportServiceTest
    {
        [TestMethod]
        public void CanExportZip()
        {
            IConfiguration configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();

            var documentStoreConfig = new DocumentStoreConfiguration();
            var agencyPaymentsConfiguration = new AgencyPaymentsConfiguration();

            configuration
                .GetSection("documentStore")
                .Bind(documentStoreConfig);
            configuration
                .GetSection("agencyPayments")
                .Bind(agencyPaymentsConfiguration);

            IDocumentStore documentStore = new DocumentStore
            {
                Urls = new[] { documentStoreConfig.Url },
                Database = documentStoreConfig.Database
            };

            new BacsExportService(documentStore, agencyPaymentsConfiguration).ExportZip(BacsExportType.Agency).Wait();
            new BacsExportService(documentStore, agencyPaymentsConfiguration).ExportZip(BacsExportType.Supplier).Wait();
        }

        [TestMethod, Ignore]
        public void CanExportSupplier()
        {
            throw new NotImplementedException();
        }

        [TestMethod, Ignore]
        public void CanExportAgency()
        {
            throw new NotImplementedException();
        }
    }
}
