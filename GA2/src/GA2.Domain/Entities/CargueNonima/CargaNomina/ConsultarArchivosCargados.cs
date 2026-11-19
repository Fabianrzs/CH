
using System;

namespace GA2.Domain.Entities.CargueNonima.CargaNomina
{
    public class ConsultarArchivosCargados
    {
        public int DCT_ID { get; set; }
        public int UEJ_ID { get; set; }
        public int TAR_ID { get; set; }
        public string NAN_TIPO { get; set; }
        public string NAN_CATEGORIA { get; set; }
        public string FAN_SUMATORIA_APORTE { get; set; }
        public int NAN_PERIODOS_APORTES { get; set; }
        public string NAN_FECHA_ENVIO { get; set; }
        public string DCT_NOMBRE { get; set; }
        public long DCT_REGISTROS { get; set; }
        public long ESD_ID { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public int TotalEstadoError { get; set; }

    }
}
