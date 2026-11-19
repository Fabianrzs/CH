using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ReportesSolicitudCredito
    {
        public DateTime FECHA_SOLICITUD { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string DEPARTAMENTO_ID { get; set; }
        public string CIUDAD_ID { get; set; }
        public string PUNTO_ATENCION { get; set; }
        public string TIPO_CREDITO { get; set; }
        public string VALOR_INMUBLE { get; set; }
        public string VALOR_AVALUO { get; set; }
        public string CONVENIO_VIDA { get; set; }
        public string CONVENIO_HOGAR { get; set; }
        public string VALOR_PRESTAMO { get; set; }
        public string CANON_INICIAL { get; set; }
        public string NUMERO_PREAPROBADO { get; set; }
        public DateTime FECHA_PREAPROBADO { get; set; }
        public string RADICADO { get; set; }
        public int CONSECUTIVO { get; set; }
        public string DescripcionTarea { get; set; }
        public string CorreoAsesor { get; set; }
        public string NombreCompletoAsesor { get; set; }
        public string EstadoCredito { get; set; }

    }
}
