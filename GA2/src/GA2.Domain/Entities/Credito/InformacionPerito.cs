using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class InformacionPerito
    {
        public Guid TRS_ID { get; set; }

        public string TAC_DESCRIPCION { get; set; }

        public string EMAIL { get; set; }

        public string USUARIO_ASESOR { get; set; }

        public string ESTADO { get; set; }

        public DateTime FECHA_ASIGNACION { get; set; }

        public string NUMERO_DOCUMENTO { get; set; }
    }
}
