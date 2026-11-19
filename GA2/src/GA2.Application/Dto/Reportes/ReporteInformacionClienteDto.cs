using System;

namespace GA2.Application.Dto
{
    public class ReporteInformacionClienteDto
    {
        public int CLI_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CLI_NOMBRES { get; set; }
        public string DRC_DIRECCION { get; set; }
        public string DTL_TELEFONO { get; set; }
        public string DCE_CORREO { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public DateTime CLI_FECHA_EXPEDICION { get; set; }
    }
}
