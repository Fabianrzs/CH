using System;

namespace GA2.Application.Dto
{
    public class CuotaAdelantadaDto
    {
        public Guid IdAplicacionPago { get; set; }
        public Guid IdCredito { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public decimal AbonoSeguroHogar { get; set; }
        public decimal AbonoSeguroVida { get; set; }
        public decimal AbonoInteresCorriente { get; set; }
        public decimal AbonoInteresMora { get; set; }
        public decimal AbonoCapital { get; set; }
        public decimal ValorPago { get; set; }
        public int IdDct { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string TipoPago { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal RemanenteUltimoPago { get; set; }
    }   
    
    
    
    public class CuotaAdelantadaFechaCierreDto
    {

        //CREC.CRE_NRO_CREDITO,
        //CREC.CLI_IDENTIFICACION,
        //CREC.TIPO_IDENTIFICACION,
        //CREC.TCR_ID

        public int tipoIdentificacion { get; set; }
        public string NumeroDocumentoCliente { get; set; }
        public long numeroCredito { get; set; }
        public int tipoCredito { get; set; }
        public Guid cpgId { get; set; }
    }    
    
    
    public class responseAdelantadaFechaCierreDto
    {

        //CREC.CRE_NRO_CREDITO,
        //CREC.CLI_IDENTIFICACION,
        //CREC.TIPO_IDENTIFICACION,
        //CREC.TCR_ID

        public long numeroCredito { get; set; }
        public string mensaje { get; set; }
        public bool integracionSeven { get; set; }
    }

}