using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GA2.Application.Dto
{
    public class ArchivoAplicacionPagoCSVDtoComparer : IEqualityComparer<ArchivoAplicacionPagoCSVDto>
    {
        public bool Equals(ArchivoAplicacionPagoCSVDto x, ArchivoAplicacionPagoCSVDto y)
        {
            return x.NroDocumento == y.NroDocumento;
        }

        public int GetHashCode([DisallowNull] ArchivoAplicacionPagoCSVDto obj)
        {
            return obj.NroDocumento.GetHashCode();
        }
    }
}
