using System;

namespace GA2.Application.Dto.Proyecto
{
    public class PolizaProyectoDto
    {
        public int PP_CONCEPTOPOLIZA_ID { get; set; }
        public string PP_RADICADOPOLIZA { get; set; }
        public DateTime PP_FECHAINICIO { get; set; }
        public DateTime PP_FECHAVIGENCIA { get; set; }
        public string PP_NUMEROPOLIZA { get; set; }
        public string PP_ASEGURADORA { get; set; }
        public double PP_VALORPOLIZA { get; set; }
        public int PP_PORCENTAJE { get; set; }
        public string PP_AJUSTADOR { get; set; }
        public string PP_DESCRIPCION { get; set; }
        public Guid PC_PROYECTO_ID { get; set; }
    }
}
