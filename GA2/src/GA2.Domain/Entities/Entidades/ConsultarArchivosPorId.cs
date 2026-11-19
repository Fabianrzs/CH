using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Entidades
{
    public class ConsultarArchivosPorId
    {
        public string ENE_PAM_NOMBRE_ARCHIVO { get; set; }
        public string ENE_PAM_DESCRIPCION_ARCHIVO { get; set; }
        public int PAM_CVL_TIPO_PERSONA_ID { get; set; }
        public Guid ENE_PAM_CREATEDOPOR { get; set; }
        public DateTime ENE_PAM_FECHACREACION { get; set; }
        public Guid ENE_PAM_MODIFICADOPOR { get; set; }
        public DateTime ENE_PAM_FECHAMODIFICACION { get; set; }
       
    }
}
