using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{

    public class ParametrosVerificacionDerechoDto
    {

        public int Id { get; set; }
        public string NumeroSolicitud { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public int TotalCuentasVerificadas { get; set; }
        public int TotalCambiosEstado { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int? Aprobado { get; set; }
        public string NumeroRadicado { get; set; }

        public int Estado { get; set; }
        public string NombreEstado { get; set; }

        public Guid SltId { get; set; }
        public Guid SltIdAprobar { get; set; }


    }

    public class ResultVerificacionDerechoDto
    {
        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public DateTime? FechaUltimoMovimientoCredito { get; set; }
        public DateTime CtaFechaPrimerAporte { get; set; }
        public int? CtaCuotas { get; set; }
        public int? CuotasPendientes { get; set; }
        public int EctIdInicial { get; set; }
        public int? EctIdFinal { get; set; }

        public string? EstadoInicial { get; set; }
        public string? EstadoFinal { get; set; }
        public string? CambiarEstado { get; set; }


        public int PeriodoAnio { get; set; }
        public int PeriodoMes { get; set; }        
        public string? Flag { get; set; }
        public string? Identificacion { get; set; }

        
        public decimal? CtaInteseresNoCausadosAportes { get; set; }
        public decimal? CtaInteseresCausadosAportes { get; set; }
        public decimal? CtaInteseresNoCausadosCesantias { get; set; }
        public decimal? CtaInteseresCausadosCesantias { get; set; }
        public decimal? CtaSaldo { get; set; }

        public decimal? SaldoAnualAportes { get; set; }
        public decimal? SaldoAnualCesantias { get; set; }
        public decimal? MovimientosMensualAportes { get; set; }
        public decimal? MovimientosMensualCesantias { get; set; }


        public decimal? SaldoAnteriorAportes { get; set; }
        public decimal? SaldoAnteriorCesantias { get; set; }
        public decimal? SaldoFinalAportes { get; set; }
        public decimal? SaldoFinalCesantias { get; set; }
        
        public int UnidadEjecutora { get; set; }
        public string? NombreUnidadEjecutora { get; set; }

        public long? NumeroSolicitud { get; set; }
        public string? TipoSolicitud { get; set; }

        public decimal InteresesAportes { get; set; }
        public decimal InteresesCesantias { get; set; }

        public decimal InteresNoAbonadosSaldoInsuficienteAportes { get; set; }
        public decimal InteresNoAbonadosSaldoInsuficienteCesantias { get; set; }

        public int TotalCuenta { get; set; }
    }

    public class InfoVerificacionDerechoDto
    {
        public int? VigenteMorosa { get; set; }
        public int? VigenteInactiva { get; set; }
        public int? VigentePrecancelada { get; set; }
        public int? VigenteCancelada { get; set; }
        public int? MorosaVigente { get; set; }
        public int? MorosaInactiva { get; set; }
        public int? MorosaPrecancelada { get; set; }
        public int? MorosaCancelada { get; set; }
        public int? InactivaVigente { get; set; }
        public int? InactivaMorosa { get; set; }
        public int? InactivaPrecancelada { get; set; }
        public int? InactivaCancelada { get; set; }
        public int? SuspendidaCancelada { get; set; }
        public int? TotalCuentasAfectadas { get; set; }

    }


    public class DatosVerificacionDerechoCuentaDto
    {

        public int CtaId { get; set; }
        public int CtaIdIntegracion { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int EctId { get; set; }
        public DateTime CtaFechaCreacion { get; set; }
        public DateTime CtaFechaPrimerAporte { get; set; }
        public int? CtaCuotas { get; set; }
        public decimal? CtaSaldo { get; set; }
        public int? CuotasEsperadas { get; set; }
        public int? CantidadMovimientoCredito { get; set; }
        public DateTime? FechaUltimoMovimientoAporte { get; set; }
        public int? UltimoAnioPeriodo { get; set; }
        public int? UltimoMesPeriodo { get; set; }
        public int? AhorrosObligarotios { get; set; }
        public string? Identificacion { get; set; }
        public string? Flag { get; set; }

        public decimal? SaldoAnualAportes{ get; set; }
        public decimal? SaldoAnualCesantias { get; set; }
        public decimal? MovimientosMensualAportes { get; set; }
        public decimal? MovimientosMensualCesantias { get; set; }
        public int UnidadEjecutora { get; set; }

        public decimal InteresesAportes { get; set; }
        public decimal InteresesCesantias { get; set; }
        public int TotalCuentas { get; set; }

    }

    public class VerificacionDerechosPeriodo
    {

        public int Anio { get; set; }
        public int Mes { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }

        public string? NumeroRadicado { get; set; }


    }

    public class ActualizarEctCuentasDto
    {

        public int ctaID { get; set; }
        public int? EctId { get; set; }         


    }

    public class VerificacionDerechosByFiltroPeriodo
    {

        public int DesdeAnio { get; set; }
        public int DesdeMes { get; set; }
        public int HastaAnio { get; set; }
        public int HastaMes { get; set; }


    }

    public class VerificacionDerechosResult
    {

        public int DesdeAnio { get; set; }
        public int DesdeMes { get; set; }
        public int HastaAnio { get; set; }
        public int HastaMes { get; set; }


    }


    public class InteresNoCausadoDto
    {
        public int Anio { get; set; }
        public int UsuarioId { get; set; }
        public int Concepto { get; set; }
        public int CtaId { get; set; }

        public int? TipoCuentaId { get; set; }
        public decimal? Saldo  { get; set; }

        public decimal? InteresesNoCausados { get; set; }
        public decimal? SaldoTotalInteresNoCausado { get; set; }


    }

    public class LiquidacionCuentasSolucionViviendaDTO
    {
        public int Mes { get; set; }
        public decimal? VP { get; set; }
        public decimal? Ipc { get; set; }
        public decimal? Pa { get; set; }
        public decimal? InteresNoCausado { get; set; }
        public decimal? InteresCausado { get; set; }
        public decimal? Vf { get; set; }


    }


    public class DatosInteresCUentaAVDto
    {

        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int? UltimoAnioPeriodo { get; set; }
        public int? UltimoMesPeriodo { get; set; }
        public string? Identificacion { get; set; }


    }

    public class VerificacionDerechosByFechaDto
    {

        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }

    }


    public class ObtenerInfoCedulaRequest
    {

        public int CuentaId { get; set; }
        public int Periodo { get; set; }

    }


}
