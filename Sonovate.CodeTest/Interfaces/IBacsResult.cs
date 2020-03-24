namespace Sonovate.CodeTest.Interfaces
{
    public interface IBacsResult
    {
        IBankDetails BankDetails { get; set; }
        object Amount { get; set; }
        string Reference { get; set; }
    }
}