namespace GA2.Application.Dto
{
    using System;
    public class RequestCreditoSeven
    {
        public Guid idSolicitudCredito { get; set; }
        public int tipoIdentificacion { get; set; }
        public string NumeroDocumentoCliente { get; set; }
        public int Afl_Id { get; set; }
    }

    public class RequestDesembolsoSeven
    {
        public string numeroDocumentoCliente { get; set; }
        public int tipoCredito { get; set; }
        public decimal valorDesembolso { get; set; }
        public string numeroCredito { get; set; }
        public int idComiteCredito { get; set; }
        public DateTime fechaDesembolso { get; set; }
        public int tipoOperacion { get; set; }
        public Guid User { get; set; }
        public Guid idSolicitudCredito { get; set; }

    }
    public class RequestGarantiasSeven
    {
        public string numeroDocumentoCliente { get; set; }
        public int tipoCredito { get; set; }
        public decimal valorGarantia { get; set; }
        public decimal valorGarantiaAct { get; set; }
        public string numeroCredito { get; set; }
        public DateTime fechaGarantia { get; set; }

    }
    public class RequestDetalleSeven
    {
        public string cuenta { get; set; }
        public string identificacion { get; set; }
        public int NumeroComprobante{ get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int tipoCredito { get; set; }
    }
    public class RequestValoresCDPSeven
    {
        public Guid idSolicitudCredito { get; set; }
        public string NumeroDocumentoCliente { get; set; }
    }
    public class RequestValoresFactura
    {
        public string IDENTIFICACION { get; set; }

        public string NUMERO_RADICADO { get; set; }
    }
    public class RequestObtenerComprobantesCredito
    {
        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN { get; set; }
        public string TCC_CODIGO { get; set; }
    }

    public class CpgDmcInfo
    {
        public long DmcRefe { get; set; }

        public string CpgId { get; set; }
    }
    public class RequestProvisionIndSeven
    {
        public string numeroDocumentoCliente { get; set; }
        public int tipoCredito { get; set; }
        public decimal valorPagoCapital { get; set; }
        public decimal valorPagoInteres { get; set; }
        public decimal valorPagoTotal { get; set; }
        public string numeroCredito { get; set; }
        public DateTime fechaProvision { get; set; }
        public string codigo { get; set; }


    }
}
