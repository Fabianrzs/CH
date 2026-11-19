using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class AfiliadosSeven: AfiliadoReinegroList
    {
        public string Numero_Radicado { get; set; }
        public int Id_Unidad_Ejecutora { get; set; }
        public string Unidad_Ejecutora { get; set; }
        public int NumerSolicitud { get; set; }
        public int PVD_CODD { get; set; }
        public int TipoCuenta { get; set; }
        public int CodigoBanco { get; set; }
        public int NumeroCuenta { get; set; }
        public string Direccion { get; set; }
        public int MPR_CONT { get; set; }


    }
}
