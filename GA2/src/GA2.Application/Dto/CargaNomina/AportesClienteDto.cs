using System;

namespace GA2.Application.Dto
{
    public class AportesClienteDto
    {
        public int APC_ID { get; set; }
        public int CLI_ID { get; set; }
        public int CTG_ID { get; set; }
        public int APC_CUOTAS_ACUMULADAS { get; set; }
        public DateTime APC_FECHA_ULTIMA_COUTA { get; set; }
        public DateTime APC_FECHA_PRIMERA_CUOTA { get; set; }
        public string CTG_DESCRIPCION { get; set; }
    }
}