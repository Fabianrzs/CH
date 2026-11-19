using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Seven
{
    public class ResponseMov
    {
        public string Cue_codi { get; set; }
        public string Dmc_desc { get; set; }
        public double Dmc_cant { get; set; }
        public double Dmc_acti { get; set; }
        public string Dmc_refe{ get; set; }
        public decimal Dmc_vadb { get; set; }
        public decimal Dmc_vacr { get; set; }
        public decimal Dmc_vaba { get; set; }
        public string Ter_coda { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_coda { get; set; }
        public string Arb_codp { get; set; }
        public string Arb_cods { get; set; }
        public string Ter_codm{ get; set; }
        public Guid Cpg_Id { get; set; }
    }

    public class AplicacionListaModel
    {
        public decimal ValorCapital { get; set; }
        public decimal ValorInteresCorriente { get; set; }
        public decimal ValorSeguroHogar { get; set; }
        public decimal ValorSeguroVida { get; set; }
        public decimal ValorCuota { get; set; }
        public string NroDocumento { get; set; }
        public string NumeroCredito { get; set; }
        public string Concepto { get; set; }
        public Guid Cpg_Id { get; set; }
        public DateTime FechaPago { get; set; }
        public string Nombre { get; set; }

        public int? UnidadEjecutora { get; set; }

        public int? AreaNegocio { get; set; }

    }

    public class CierreMnesualSevenModel
    {
        public Guid CreId { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public int PlazoTotal { get; set; }
        public int TcrId { get; set; }
        public string TipoCredito { get; set; }
        public long NumeroCredito { get; set; }
        public string Identificacion { get; set; }
        public int EscId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal SaldoCapitalRemanente { get; set; }
        public Guid CpgId { get; set; }
       
    }

}
