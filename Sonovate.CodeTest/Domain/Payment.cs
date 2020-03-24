using Sonovate.CodeTest.Interfaces;
using System;

namespace Sonovate.CodeTest.Domain
{
    internal class Payment : IPayment
    {
        public string AgencyId { get; set; }
        public decimal Balance { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}