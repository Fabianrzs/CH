using System;

namespace GA2.Domain.Entities.Cartera.Cierres.CierresCausacionesRealizadas
{
    public class CierresCausacionesRealizadas
    {
        public int ID { get; set; }
        public string TIPO_CIERRE { get; set; }
        public DateTime FECHA_CIERRE { get; set; }
        public DateTime FECHA_EJECUCION { get; set; }
        public Guid USUARIO_EJECUTA_CIERRE { get; set; }
        public int TCC_ID { get; set; }
        public string TCC_NOMBRE { get; set; }
        public string TCC_DESCRIPCION { get; set; }
    }
}
