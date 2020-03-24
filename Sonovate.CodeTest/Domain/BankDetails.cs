using Sonovate.CodeTest.Interfaces;

namespace Sonovate.CodeTest.Domain
{
    public class BankDetails : IBankDetails
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string SortCode { get; set; }
    }
}