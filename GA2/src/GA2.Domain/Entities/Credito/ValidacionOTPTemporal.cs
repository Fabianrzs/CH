using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ValidacionOTPTemporal
    {

        public Guid VOT_ID { get; set; }
        public string VOT_CELULAR { get; set; }
        public string VOT_IDENTIFICACION { get; set; }
        public int VOT_INTENTOS { get; set; }
        public bool VOT_ESTADO { get; set; }
        public DateTime VOT_FECHA { get; set; }
        public bool BloqueoDia { get; set; }
    }
}
