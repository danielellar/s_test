using Sonovate.CodeTest.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sonovate.CodeTest.Interfaces
{
    internal interface IAgencyPaymentService
    {
        Task<List<IBacsResult>> GetAgencyPayments(DateTime startDate, DateTime endDate);
        void SavePayments(IEnumerable<IBacsResult> payments, BacsExportType type);
    }
}