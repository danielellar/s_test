using Sonovate.CodeTest.Interfaces;
using System.Collections.Generic;

namespace Sonovate.CodeTest.Domain
{
    internal class SupplierBacsExport : ISupplierBacsExport
    {
        public List<ISupplierBacs> SupplierPayment { get; set; }
    }
}