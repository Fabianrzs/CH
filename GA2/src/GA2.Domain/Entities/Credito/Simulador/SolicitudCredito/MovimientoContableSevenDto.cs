namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// DTO: Movimiento Contable Seven
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class MovimientoContableSevenDto
    {
        /// <summary>Código de cuenta</summary>
        public string Cue_codi { get; set; }

        /// <summary>Descripción del movimiento</summary>
        public string Dmc_desc { get; set; }

        /// <summary>Cantidad de movimientos</summary>
        public int Dmc_cant { get; set; }

        /// <summary>Referencia del afiliado/crédito</summary>
        public string Dmc_refe { get; set; }

        /// <summary>Valor débito</summary>
        public decimal Dmc_vadb { get; set; }

        /// <summary>Valor crédito</summary>
        public decimal Dmc_vacr { get; set; }

        /// <summary>Código de tercero</summary>
        public string Ter_coda { get; set; }

        /// <summary>Código A del árbol</summary>
        public string Arb_codc { get; set; }

        /// <summary>Código A del árbol</summary>
        public string Arb_coda { get; set; }

        /// <summary>Código P del árbol</summary>
        public string Arb_codp { get; set; }

        /// <summary>Código S del árbol</summary>
        public string Arb_cods { get; set; }

        /// <summary>Código M de tercero</summary>
        public string Ter_codm { get; set; }
    }
}
