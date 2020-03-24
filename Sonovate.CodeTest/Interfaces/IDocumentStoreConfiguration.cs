using System;

namespace Sonovate.CodeTest.Interfaces
{
    public interface IDocumentStoreConfiguration
    {
        public string Url { get; set; }
        public string Database { get; set; }
        
    }
}