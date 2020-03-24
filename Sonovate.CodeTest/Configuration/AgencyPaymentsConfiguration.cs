using Sonovate.CodeTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sonovate.CodeTest.Configuration
{
    public class AgencyPaymentsConfiguration : IAgencyPaymentsConfiguration
    {
        public bool Enabled { get; set; }
    }
}
