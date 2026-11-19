using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class InconsistenciaSolicitud
    {
        public Guid INC_ID { get; set; }
        public Guid LIN_ID_FK { get; set; }
        public Guid HER_ID_FK { get; set; }
        public Guid PTA_ID_FK { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public Guid USR_ID_FK { get; set; }
        public int GIN_ID_FK { get; set; }
        public string NVD_ID_FK { get; set; }
        public string INC_OBJETO_ESTUDIO { get; set; }
        public string INC_ANALISIS_REALIZADO { get; set; }
        public string INC_OBSERVACION { get; set; }
        public bool INC_ESTADO { get; set; }
        public Guid INC_CREATEDOPOR { get; set; }
        public Guid INC_MODIFICADOPOR { get; set; }
        public DateTime INC_FECHAMODIFICACION { get; set; }
        public string LIN_DESCRIPCION { get; set; }
        public string RadicadoSolicitud { get; set; }
        public int IdCliente { get; set; }

        public string CIN_ID_FK { get; set; }

        public Guid CAU_ID { get; set; }
    }
}
