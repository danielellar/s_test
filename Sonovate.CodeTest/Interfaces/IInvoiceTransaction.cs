using System;

namespace Sonovate.CodeTest.Interfaces
{
    public interface IInvoiceTransaction
    {
        public decimal Gross { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceReference { get; set; }
        public string SupplierId { get; set; }
    }
}
