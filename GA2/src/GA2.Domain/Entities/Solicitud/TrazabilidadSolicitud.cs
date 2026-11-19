using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TrazabilidadSolicitud
    {
        public Guid SLT_ID { get; set; }
        public Guid TRA_ID { get; set; }
        public string TRA_NOMBRE { get; set; }
        public Guid SLT_CREATEDOPOR { get; set; }
        public string SLT_NOMBRE_USUARIO { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public string CVL_DESCRIPCION { get; set; }
        public string SLT_OBSERVACION { get; set; }
        public DateTime SLT_FECHACREACION { get; set; }
        public DateTime SLT_FECHAFIN { get; set; }
        public DateTime SLT_FECHAMODIFICACION { get; set; }
    }
}
