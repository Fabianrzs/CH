using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.VerificacionDerechos
{
    public class VerificacionDerechoSolicitudTarea
    {

        public Guid SLT_ID {  get; set; }
        public Guid TRA_ID {  get; set; }
        public Guid SOL_ID {  get; set; }
        public Guid USR_ID {  get; set; }

        public string OBSERVACION { get; set; }

    }
}
