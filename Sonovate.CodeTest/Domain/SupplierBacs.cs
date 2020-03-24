using Sonovate.CodeTest.Interfaces;

namespace Sonovate.CodeTest.Domain
{
    internal class SupplierBacs : ISupplierBacs
    {
        public IBankDetails BankDetails { get; set; }
        public decimal PaymentAmount { get; set; }
        public string InvoiceReference { get; set; }
        public string PaymentReference { get; set; }
    }
}