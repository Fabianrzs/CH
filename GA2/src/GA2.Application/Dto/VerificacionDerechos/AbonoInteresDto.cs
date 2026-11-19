using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.VerificacionDerechos
{

    public class ParametrosAbonosInteresDto
    {

        public int Id { get; set; }
        public string NumeroSolicitud { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal TotalInteresesAbonados { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public DateTime? FechaAprobacion { get; set; }

        public DateTime? fechaRecepcion { get; set; }
        public float ValorIpc { get; set; }

        public float PorcentajeIpc { get; set; }

        public string ComprobanteSolucionViviendaAportes { get; set; }
        public string ComprobanteSolucionViviendaCesantias { get; set; }
        public string ComprobanteSolucionAdministracionCesantias { get; set; }
        public string ComprobanteAVAC { get; set; }
        public string ComprobanteAVAV { get; set; }
        public string ComprobanteAVC { get; set; }

        public int? Aprobado { get; set; }
        public string NumeroRadicado { get; set; }


        public int Estado { get; set; }
        public string NombreEstado { get; set; }

        public Guid SltId { get; set; }
        public Guid SltIdAprobar { get; set; }


    }


    public class DatosCuentasAdministracionCesantiasDto
    {

        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public decimal CtaSaldo { get; set; }
        public DateTime FechaUltimoMovimientosCredito { get; set; }
        public string Identificacion { get; set; }
        public decimal SaldoAnualCesantias { get; set; }
        public decimal MovimientosMensualCesantias { get; set; }
        public int UnidadEjecutora { get; set; }
        public decimal InteresesCesantias { get; set; }

    }

    public class DatosCuentasAdministracionCesantiasResultDto
    {
        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public decimal CtaSaldo { get; set; }
        public DateTime FechaUltimoMovimientosCredito { get; set; }
        public string Identificacion { get; set; }

        public int PeriodoAnio { get; set; }
        public int PeriodoMes { get; set; }

        public decimal SaldoAnualCesantias { get; set; }
        public decimal MovimientosMensualCesantias { get; set; }


        public decimal? InteseresNoCausadosCesantias { get; set; }
        public decimal? InteseresCausadosCesantias { get; set; }

        public decimal? SaldoAnteriorCesantias { get; set; }
        public decimal? SaldoFinalCesantias { get; set; }

        public int UnidadEjecutora { get; set; }

        public long? NumeroSolicitud { get; set; }
        public string? TipoSolicitud { get; set; }
        public decimal InteresesCesantias { get; set; }

        public decimal InteresNoAbonadosSaldoInsuficienteCesantias { get; set; }


    }


    public class DatosCuentasAVDto
    {

        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int EctId { get; set; }
        public DateTime CtaFechaCreacion { get; set; }
        public decimal? CtaSaldo { get; set; }

        public DateTime? AVAC { get; set; }
        public decimal? ValorAVAC { get; set; }
        public decimal? SaldoAnualAVAC { get; set; }
        public decimal? MovimientosMensualAVAC { get; set; }


        public DateTime? AVAV { get; set; }
        public decimal? ValorAVAV { get; set; }
        public decimal? SaldoAnualAVAV { get; set; }
        public decimal? MovimientosMensualAVAV { get; set; }

        public DateTime? AVC { get; set; }
        public decimal? ValorAVC { get; set; }
        public decimal? SaldoAnualAVC { get; set; }
        public decimal? MovimientosMensualAVC { get; set; }

        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }


    }



    public class DatosCuentasAVResultDto
    {
        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int PeriodoAnio { get; set; }
        public int PeriodoMes { get; set; }



        public decimal? ValorAVAC { get; set; }
        public decimal? SaldoAnualAVAC { get; set; }
        public decimal? MovimientosMensualAVAC { get; set; }
        public decimal? InteresesNoCausadosAVAC { get; set; }
        public decimal? InteresesCausadosAVAC { get; set; }
        public decimal? SaldoAnteriorAVAC { get; set; }
        public decimal? SaldoFinalAVAC { get; set; }



        public decimal? ValorAVAV { get; set; }
        public decimal? SaldoAnualAVAV { get; set; }
        public decimal? MovimientosMensualAVAV { get; set; }
        public decimal? InteresesNoCausadosAVAV { get; set; }
        public decimal? InteresesCausadosAVAV { get; set; }
        public decimal? SaldoAnteriorAVAV { get; set; }
        public decimal? SaldoFinalAVAV { get; set; }


        public decimal? ValorAVC { get; set; }
        public decimal? SaldoAnualAVC { get; set; }
        public decimal? MovimientosMensualAVC { get; set; }
        public decimal? InteresesNoCausadosAVC { get; set; }
        public decimal? InteresesCausadosAVC { get; set; }
        public decimal? SaldoAnteriorAVC { get; set; }
        public decimal? SaldoFinalAVC { get; set; }

        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

        public long? NumeroSolicitud { get; set; }
        public string? TipoSolicitud { get; set; }


    }

    public class InformacionConsolidadoBaseCalculo
    {
        public int Id { get; set; }
        public decimal? TotalBase { get; set; }
        public decimal? TotaInteresAbonadoPeriodo { get; set; }
        public decimal? TotaInteresNoAbonadoPeriodo { get; set; }
        public decimal? TotaInteresReconocidoPeriodo { get; set; }
        public decimal? TotaInteresIPCNegativo { get; set; }


    }

    public class InformacionAbonoInteresUnidadEjecutora
    {
        public int Id { get; set; }
        public decimal? TotalSolucionVivienda { get; set; }
        public decimal? TotaAdministracionCesantias { get; set; }
        public decimal? TotaAVAV { get; set; }
        public decimal? TotaAVC { get; set; }
        public decimal? TotaAVAC { get; set; }


    }

    public class CrearAbonosInteresDatos
    {

        public List<ResultVerificacionDerecho> resultVerificacionDerecho { get; set; }
        public List<DatosCuentasAVResult> datosCuentasAVResult { get; set; }
        public List<DatosCuentasAdministracionCesantiasResult> datosCuentasAdministracionCesantiasResult { get; set; }




    }




}
