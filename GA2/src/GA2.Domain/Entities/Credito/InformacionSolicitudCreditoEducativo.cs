using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class InformacionSolicitudCreditoEducativo
    {
        public int TCR_ID {  get; set; }
        public string NOMBRE_ESTUDIANTE { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string DIRECCION { get; set; }
    }
}
