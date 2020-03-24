using Sonovate.CodeTest.Interfaces;

namespace Sonovate.CodeTest.Domain
{
    public class Agency : IAgency
    {
        public string Id { get; set; }
        public IBankDetails BankDetails { get; set; }
    }
}