using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class SolicitudNomina
    {
        
        public Guid SNMN_ID { get; set; }
        public int DCT_ID { get; set; }
        public int SNMN_NUMERO_SOLICITUD { get; set; }
        public int SNMN_ESTADO_SOLICITUD { get; set; }
        public Boolean SNMN_ESTADO { get; set; }
        public DateTime SNMN_FECHA_INICIAL { get; set; }
        public DateTime SNMN_FECHA_FINAL { get; set; }
        public string SNMN_OBSERVACION { get; set; }
        public Guid SNMN_AUTORIZADOPOR { get; set; }
        public Guid SNMN_PROCESADOPOR { get; set; }
        public Guid SNMN_APROBADOPOR { get; set; }
        public DateTime SNMN_FECHACREACION { get; set; }
        public DateTime SNMN_FECHAMODIFICACION { get; set; }
        public string SNMN_RADICADO { get; set; }
        public decimal SNMN_MONTO { get; set; }
        public int SNMN_PERIODO { get; set; }
        public long SNMN_REGISTROS { get; set; }
        public string SNMN_TIPO_ARCHIVO { get; set; }
        public string SNMN_NOMBRE_ARCHIVO { get; set; }
    }
}
