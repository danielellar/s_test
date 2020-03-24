using Sonovate.CodeTest.Interfaces;

namespace Sonovate.CodeTest.Domain
{
    public class BacsResult : IBacsResult
    {
        public IBankDetails BankDetails { get; set; }
        public object Amount { get; set; }
        public string Reference  { get; set; }
    }
}