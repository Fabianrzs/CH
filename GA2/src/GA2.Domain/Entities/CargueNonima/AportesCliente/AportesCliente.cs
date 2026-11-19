using GA2.Transversals.Commons;
using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Entidad aportes clientes archivo nomina
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public class AportesCliente : BaseEntity
    {
        public int APC_ID { get; set; }
        public int CLI_ID { get; set; }
        public int CTG_ID { get; set; }
        public int APC_CUOTAS_ACUMULADAS { get; set; }
        public DateTime APC_FECHA_ULTIMA_CUOTA { get; set; }
        public DateTime APC_FECHA_PRIMERA_CUOTA { get; set; }
    }
}
