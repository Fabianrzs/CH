using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera
{
    public class ObtenerProductoCartera
    {
        public long Nro_Credito { get; set; }
        public int Tcr_Id { get; set; }

        public ObtenerProductoCartera()
        {
        }

        public ObtenerProductoCartera(long nro_Credito, int tcr_Id)
        {
            Nro_Credito = nro_Credito;
            Tcr_Id = tcr_Id;
        }
    }
}
