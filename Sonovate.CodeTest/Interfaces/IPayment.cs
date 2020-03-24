using System;

namespace Sonovate.CodeTest.Interfaces
{
    internal interface IPayment
    {
        string AgencyId { get; set; }
        decimal Balance { get; set; }
        DateTime PaymentDate { get; set; }
    }
}