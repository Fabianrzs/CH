namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// DTO: Información de Afiliado con sus Movimientos
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class AfiliadoMovimientosDto
    {
        /// <summary>Referencia del afiliado/crédito</summary>
        public string Dmc_refe { get; set; }

        /// <summary>Descripción del cliente</summary>
        public string Dmc_desc { get; set; }

        /// <summary>Cantidad de movimientos</summary>
        public int CantidadMovimientos { get; set; }

        /// <summary>Total débito del afiliado</summary>
        public decimal TotalDebito { get; set; }

        /// <summary>Total crédito del afiliado</summary>
        public decimal TotalCredito { get; set; }

        /// <summary>Código de tercero</summary>
        public string Ter_coda { get; set; }

        /// <summary>Porcentaje del total del comprobante</summary>
        public decimal PorcentajeTotal { get; set; }
    }
}
