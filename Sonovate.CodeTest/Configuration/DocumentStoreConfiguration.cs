using Sonovate.CodeTest.Interfaces;
using System;

namespace Sonovate.CodeTest.Configuration
{
    public class DocumentStoreConfiguration : IDocumentStoreConfiguration
    {
        public string Url { get; set; }
        public string Database { get; set; }
        public bool EnableAgencyPayments { get; set; }
    }
}