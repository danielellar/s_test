using System;

namespace Sonovate.CodeTest.Interfaces
{
    internal interface ISupplierPaymentService
    {
        ISupplierBacsExport GetSupplierPayments(DateTime startDate, DateTime endDate);

        void SaveSupplierBacsExport(ISupplierBacsExport supplierBacsExport);
    }
}