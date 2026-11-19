using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class BeneficiariosAfiliaciones
    {
        public int DPT_ID { get; set; }
        public int CLI_ID { get; set; }
        public int TID_ID { get; set; }
        public string DPT_IDENTIFICACION { get; set; }
        public string DPT_NOMBRE { get; set; }
        public int DPT_PARTICIPACION { get; set; }
        public int DPA_ID { get; set; }
        public string DPA_DIRECCION { get; set; }
        public string DPA_CORREO { get; set; }
        public string DPA_TELEFONO { get; set; }
        public bool DPA_CONSERVA_ANTIGUEDAD { get; set; }
        public int DPA_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL { get; set; }
        public DateTime DPA_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PERSONAL { get; set; }
        public int DPA_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES { get; set; }
        public int DPA_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES { get; set; }
        public DateTime DPA_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES { get; set; }
        public int DPA_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS { get; set; }
        public bool DPA_DERECHO_LIQUIDACION_CUOTAS_BENEFICIARIOS { get; set; }
        public int DPA_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO { get; set; }
    }
}
