using System;

namespace GA2.Application.Dto.Proyecto
{
    public class LicenciaUrbanismoDto
    {
        public string LUP_RESOLUCION { get; set; }
        public string LUP_CURADURIA { get; set; }
        public int LUP_DEPARTAMENTO_ID { get; set; }
        public int LUP_MUNICIPIO_ID { get; set; }
        public DateTime LUP_DATE_VIGENCIA { get; set; }
        public string LUP_TITULAR { get; set; }
        public DateTime LUP_DATE_RESOLUCION { get; set; }
        public int LUP_MISMA_LICENCI { get; set; }
        public Guid? PC_PROYECTO_ID { get; set; } = Guid.Empty; 
    }
}
