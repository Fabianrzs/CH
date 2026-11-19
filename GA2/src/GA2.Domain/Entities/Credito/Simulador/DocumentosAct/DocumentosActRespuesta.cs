using System.ComponentModel.DataAnnotations;

namespace GA2.Domain.Entities
{
    public class DocumentosActRespuesta
    {
        [Key]
        public string TDC_ID { get; set; }
        public int ID_ACTIVIDAD_FLUJO { get; set; }
        public string ACTIVIDAD { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string OBLIGATORIO { get; set; }
        public string CARGA { get; set; }
        public string VISUALIZA { get; set; }
        public string VISUALIZACLIENTE { get; set; }
        public string DOCPREAPROBADO { get; set; }
        public int ORDEN { get; set; }
        public string RUTA_IMAGEN { get; set; }
        public int DCS_APROBACION { get; set; }
    }
}
