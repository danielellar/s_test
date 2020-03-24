using Sonovate.CodeTest.Interfaces;
using System.Collections.Generic;

namespace Sonovate.CodeTest.Domain
{
    internal class Candidate : ICandidate
    {
        public IBankDetails BankDetails { get; set; }
    }
}