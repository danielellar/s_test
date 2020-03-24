namespace Sonovate.CodeTest.Interfaces
{
    internal interface ISupplierBacs
    {
        IBankDetails BankDetails { get; set; }
        string InvoiceReference { get; set; }
        decimal PaymentAmount { get; set; }
        string PaymentReference { get; set; }
    }
}