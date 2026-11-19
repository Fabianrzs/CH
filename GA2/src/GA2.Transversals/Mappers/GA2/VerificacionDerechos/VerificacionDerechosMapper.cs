using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVC;

namespace GA2.Transversals.Mappers
{
    /// <summary>
    /// Mapper profile aplication Identity
    /// <author>Oscar Julian Rojas Garces</author>
    /// <date>24/02/2021</date>
    /// </summary>
    public class VerificacionDerechosMapper : Profile
    {
        public VerificacionDerechosMapper()
        {

            CreateMap<ParametrosVerificacionDerecho, ParametrosVerificacionDerechoDto>()
                 .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                 .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
                 .ForMember(x => x.Anio, map => map.MapFrom(dto => dto.ANIO))
                 .ForMember(x => x.Mes, map => map.MapFrom(dto => dto.MES))
                 .ForMember(x => x.FechaOperacion, map => map.MapFrom(dto => dto.FECHA_OPERACION))
                 .ForMember(x => x.FechaAprobacion, map => map.MapFrom(dto => dto.FECHA_APROBACION))
                 .ForMember(x => x.Anio, map => map.MapFrom(dto => dto.ANIO))
                 .ForMember(x => x.TotalCuentasVerificadas, map => map.MapFrom(dto => dto.TOTAL_CUENTAS_VERIFICADAS))
                 .ForMember(x => x.TotalCambiosEstado, map => map.MapFrom(dto => dto.TOTAL_CAMBIOS_ESTADO))
                 .ForMember(x => x.UsuarioId, map => map.MapFrom(dto => dto.USUARIO_ID))
                 .ForMember(x => x.UsuarioNombre, map => map.MapFrom(dto => dto.USUARIO_NOMBRE))
                 .ForMember(x => x.Aprobado, map => map.MapFrom(dto => dto.APROBADO))
                 .ForMember(x => x.NumeroRadicado, map => map.MapFrom(dto => dto.NUMERO_RADICADO))

                 .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
                 .ForMember(x => x.NombreEstado, map => map.MapFrom(dto => dto.NOMBRE_ESTADO))

                 .ForMember(x => x.SltId, map => map.MapFrom(dto => dto.SLT_ID))
                 .ForMember(x => x.SltIdAprobar, map => map.MapFrom(dto => dto.SLT_ID_APROBAR))

                 .ReverseMap();



            CreateMap<ResultVerificacionDerecho, ResultVerificacionDerechoDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.FechaUltimoMovimientoCredito, map => map.MapFrom(dto => dto.FECHA_ULTIMO_MOVIMIENTOS_CREDITO))
                .ForMember(x => x.CtaFechaPrimerAporte, map => map.MapFrom(dto => dto.CTA_FECHA_PRIMER_APORTE))
                .ForMember(x => x.CtaCuotas, map => map.MapFrom(dto => dto.CTA_CUOTAS))
                .ForMember(x => x.CuotasPendientes, map => map.MapFrom(dto => dto.CUOTAS_PENDIENTES))
                .ForMember(x => x.EctIdInicial, map => map.MapFrom(dto => dto.ECT_INICIAL))
                .ForMember(x => x.EctIdFinal, map => map.MapFrom(dto => dto.ECT_FINAL))
                .ForMember(x => x.CambiarEstado, map => map.MapFrom(dto => dto.CAMBIO_ESTADO))

                .ForMember(x => x.PeriodoAnio, map => map.MapFrom(dto => dto.PERIODO_ANIO))
                .ForMember(x => x.PeriodoMes, map => map.MapFrom(dto => dto.PERIODO_MES))
                .ForMember(x => x.Flag, map => map.MapFrom(dto => dto.FLAG))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))

                .ForMember(x => x.SaldoAnualAportes, map => map.MapFrom(dto => dto.SALDO_ANUAL_APORTES))
                .ForMember(x => x.SaldoAnualCesantias, map => map.MapFrom(dto => dto.SALDO_ANUAL_CESANTIAS))
                .ForMember(x => x.MovimientosMensualAportes, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_APORTES))
                .ForMember(x => x.MovimientosMensualCesantias, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_CESANTIAS))

                .ForMember(x => x.SaldoAnteriorAportes, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_APORTES))
                .ForMember(x => x.SaldoAnteriorCesantias, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_CESANTIAS))
                .ForMember(x => x.SaldoFinalAportes, map => map.MapFrom(dto => dto.SALDO_FINAL_APORTES))
                .ForMember(x => x.SaldoFinalCesantias, map => map.MapFrom(dto => dto.SALDO_FINAL_CESANTIAS))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
                .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))

                .ForMember(x => x.InteresesAportes, map => map.MapFrom(dto => dto.INTERES_APORTES))
                .ForMember(x => x.InteresesCesantias, map => map.MapFrom(dto => dto.INTERES_CESANTIAS))

                .ForMember(x => x.InteresNoAbonadosSaldoInsuficienteAportes, map => map.MapFrom(dto => dto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES))
                .ForMember(x => x.InteresNoAbonadosSaldoInsuficienteCesantias, map => map.MapFrom(dto => dto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS))

                .ReverseMap();

            CreateMap<DatosVerificacionDerechoCuenta, DatosVerificacionDerechoCuentaDto>()
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaIdIntegracion, map => map.MapFrom(dto => dto.CTA_ID_INTEGRACION))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))
                .ForMember(x => x.CtaFechaCreacion, map => map.MapFrom(dto => dto.CTA_FECHA_CREACION))
                .ForMember(x => x.CtaFechaPrimerAporte, map => map.MapFrom(dto => dto.CTA_FECHA_PRIMER_APORTE))
                .ForMember(x => x.CtaCuotas, map => map.MapFrom(dto => dto.CTA_CUOTAS))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))
                .ForMember(x => x.CuotasEsperadas, map => map.MapFrom(dto => dto.CUOTAS_ESPERADAS))
                .ForMember(x => x.CantidadMovimientoCredito, map => map.MapFrom(dto => dto.CANTIDAD_MOVIMIENTOS_CREDITO))
                .ForMember(x => x.FechaUltimoMovimientoAporte, map => map.MapFrom(dto => dto.FECHA_ULTIMO_MOVIMIENTOS_CREDITO))
                .ForMember(x => x.UltimoAnioPeriodo, map => map.MapFrom(dto => dto.ULTIMO_ANIO_PERIODO))
                .ForMember(x => x.UltimoMesPeriodo, map => map.MapFrom(dto => dto.ULTIMO_MES_PERIODO))
                .ForMember(x => x.AhorrosObligarotios, map => map.MapFrom(dto => dto.AHORROS_OBLIGATORIOS))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.Flag, map => map.MapFrom(dto => dto.FLAG))

                .ForMember(x => x.SaldoAnualAportes, map => map.MapFrom(dto => dto.SALDO_ANUAL_APORTES))
                .ForMember(x => x.SaldoAnualCesantias, map => map.MapFrom(dto => dto.SALDO_ANUAL_CESANTIAS))
                .ForMember(x => x.MovimientosMensualAportes, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_APORTES))
                .ForMember(x => x.MovimientosMensualCesantias, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_CESANTIAS))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ForMember(x => x.InteresesAportes, map => map.MapFrom(dto => dto.INTERES_APORTES))
                .ForMember(x => x.InteresesCesantias, map => map.MapFrom(dto => dto.INTERES_CESANTIAS))

                .ReverseMap();


            CreateMap<IPC, IPCDto>()
               .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
               .ForMember(x => x.Anio, map => map.MapFrom(dto => dto.ANIO))
               .ForMember(x => x.Mes, map => map.MapFrom(dto => dto.MES))
               .ForMember(x => x.ValorIPC, map => map.MapFrom(dto => dto.VALOR_IPC))
               .ForMember(x => x.FechaActualizacion, map => map.MapFrom(dto => dto.FECHA_ACTUALIZACION))
               .ForMember(x => x.PorcentajeIPC, map => map.MapFrom(dto => dto.PORCENTAJE_IPC))
               .ReverseMap();




            CreateMap<DatosCuentasAdministracionCesantias, DatosCuentasAdministracionCesantiasDto>()

                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))
                .ForMember(x => x.FechaUltimoMovimientosCredito, map => map.MapFrom(dto => dto.FECHA_ULTIMO_MOVIMIENTOS_CREDITO))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.SaldoAnualCesantias, map => map.MapFrom(dto => dto.SALDO_ANUAL_CESANTIAS))
                .ForMember(x => x.MovimientosMensualCesantias, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_CESANTIAS))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))
                .ForMember(x => x.InteresesCesantias, map => map.MapFrom(dto => dto.INTERES_CESANTIAS))

                .ReverseMap();

            CreateMap<DatosCuentasAV, DatosCuentasAVDto>()

                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))
                .ForMember(x => x.CtaFechaCreacion, map => map.MapFrom(dto => dto.CTA_FECHA_CREACION))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))

                .ForMember(x => x.AVAC, map => map.MapFrom(dto => dto.AVAC))
                .ForMember(x => x.ValorAVAC, map => map.MapFrom(dto => dto.VALOR_AVAC))
                .ForMember(x => x.SaldoAnualAVAC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAC))
                .ForMember(x => x.MovimientosMensualAVAC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAC))

                .ForMember(x => x.AVAV, map => map.MapFrom(dto => dto.AVAV))
                .ForMember(x => x.ValorAVAV, map => map.MapFrom(dto => dto.VALOR_AVAV))
                .ForMember(x => x.SaldoAnualAVAV, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAV))
                .ForMember(x => x.MovimientosMensualAVAV, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAV))

                .ForMember(x => x.AVC, map => map.MapFrom(dto => dto.AVC))
                .ForMember(x => x.ValorAVC, map => map.MapFrom(dto => dto.VALOR_AVC))
                .ForMember(x => x.SaldoAnualAVC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVC))
                .ForMember(x => x.MovimientosMensualAVC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVC))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))



                .ReverseMap();




            CreateMap<DatosCuentasAdministracionCesantiasResult, DatosCuentasAdministracionCesantiasResultDto>()

                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))

                .ForMember(x => x.FechaUltimoMovimientosCredito, map => map.MapFrom(dto => dto.FECHA_ULTIMO_MOVIMIENTOS_CREDITO))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.PeriodoAnio, map => map.MapFrom(dto => dto.PERIODO_ANIO))
                .ForMember(x => x.PeriodoMes, map => map.MapFrom(dto => dto.PERIODO_MES))

                .ForMember(x => x.SaldoAnualCesantias, map => map.MapFrom(dto => dto.SALDO_ANUAL_CESANTIAS))
                .ForMember(x => x.MovimientosMensualCesantias, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_CESANTIAS))

                .ForMember(x => x.InteseresNoCausadosCesantias, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_CESANTIAS))
                .ForMember(x => x.InteseresCausadosCesantias, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_CESANTIAS))

                .ForMember(x => x.SaldoAnteriorCesantias, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_CESANTIAS))
                .ForMember(x => x.SaldoFinalCesantias, map => map.MapFrom(dto => dto.SALDO_FINAL_CESANTIAS))


                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
                .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))

                .ForMember(x => x.InteresesCesantias, map => map.MapFrom(dto => dto.INTERES_CESANTIAS))

                .ForMember(x => x.InteresNoAbonadosSaldoInsuficienteCesantias, map => map.MapFrom(dto => dto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS))

                .ReverseMap();



            CreateMap<DatosCuentasAVResult, DatosCuentasAVResultDto>()

                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))

                .ForMember(x => x.PeriodoAnio, map => map.MapFrom(dto => dto.PERIODO_ANIO))
                .ForMember(x => x.PeriodoMes, map => map.MapFrom(dto => dto.PERIODO_MES))


                .ForMember(x => x.ValorAVAC, map => map.MapFrom(dto => dto.VALOR_AVAC))
                .ForMember(x => x.SaldoAnualAVAC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAC))
                .ForMember(x => x.MovimientosMensualAVAC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAC))
                .ForMember(x => x.InteresesNoCausadosAVAC, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_AVAC))
                .ForMember(x => x.InteresesCausadosAVAC, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_AVAC))
                .ForMember(x => x.SaldoAnteriorAVAC, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_AVAC))
                .ForMember(x => x.SaldoFinalAVAC, map => map.MapFrom(dto => dto.SALDO_FINAL_AVAC))



                .ForMember(x => x.ValorAVAV, map => map.MapFrom(dto => dto.VALOR_AVAV))
                .ForMember(x => x.SaldoAnualAVAV, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAV))
                .ForMember(x => x.MovimientosMensualAVAV, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAV))
                .ForMember(x => x.InteresesNoCausadosAVAV, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_AVAV))
                .ForMember(x => x.InteresesCausadosAVAV, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_AVAV))
                .ForMember(x => x.SaldoAnteriorAVAV, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_AVAV))
                .ForMember(x => x.SaldoFinalAVAV, map => map.MapFrom(dto => dto.SALDO_FINAL_AVAV))



                .ForMember(x => x.ValorAVC, map => map.MapFrom(dto => dto.VALOR_AVC))
                .ForMember(x => x.SaldoAnualAVC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVC))
                .ForMember(x => x.MovimientosMensualAVC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVC))
                .ForMember(x => x.InteresesNoCausadosAVC, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_AVC))
                .ForMember(x => x.InteresesCausadosAVC, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_AVC))
                .ForMember(x => x.SaldoAnteriorAVC, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_AVC))
                .ForMember(x => x.SaldoFinalAVC, map => map.MapFrom(dto => dto.SALDO_FINAL_AVC))

                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
                .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))

                .ReverseMap();


            CreateMap<ParametrosAbonoInteres, ParametrosAbonosInteresDto>()
                 .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                 .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
                 .ForMember(x => x.Anio, map => map.MapFrom(dto => dto.ANIO))
                 .ForMember(x => x.Mes, map => map.MapFrom(dto => dto.MES))
                 .ForMember(x => x.FechaAprobacion, map => map.MapFrom(dto => dto.FECHA_APROBACION))
                 .ForMember(x => x.Anio, map => map.MapFrom(dto => dto.ANIO))
                 .ForMember(x => x.TotalInteresesAbonados, map => map.MapFrom(dto => dto.TOTAL_INTERESES_ABONADOS))
                 .ForMember(x => x.UsuarioId, map => map.MapFrom(dto => dto.USUARIO_ID))
                 .ForMember(x => x.UsuarioNombre, map => map.MapFrom(dto => dto.USUARIO_NOMBRE))
                 .ForMember(x => x.Aprobado, map => map.MapFrom(dto => dto.APROBADO))

                 .ForMember(x => x.fechaRecepcion, map => map.MapFrom(dto => dto.FECHA_RECEPCION))
                 .ForMember(x => x.ValorIpc, map => map.MapFrom(dto => dto.VALOR_IPC))
                 .ForMember(x => x.PorcentajeIpc, map => map.MapFrom(dto => dto.PORCENTAJE_IPC))

                 .ForMember(x => x.ComprobanteSolucionViviendaAportes, map => map.MapFrom(dto => dto.COMPROBANTE_SOLUCION_VIVIENDA_APORTES))
                 .ForMember(x => x.ComprobanteSolucionViviendaCesantias, map => map.MapFrom(dto => dto.COMPROBANTE_SOLUCION_VIVIENDA_CESANTIAS))
                 .ForMember(x => x.ComprobanteSolucionAdministracionCesantias, map => map.MapFrom(dto => dto.COMPROBANTE_SOLUCION_ADMINISTRACION_CESANTIAS))
                 .ForMember(x => x.ComprobanteAVAC, map => map.MapFrom(dto => dto.COMPROBANTE_AVAC))
                 .ForMember(x => x.ComprobanteAVAV, map => map.MapFrom(dto => dto.COMPROBANTE_AVAV))
                 .ForMember(x => x.ComprobanteAVC, map => map.MapFrom(dto => dto.COMPROBANTE_AVC))
                 .ForMember(x => x.NumeroRadicado, map => map.MapFrom(dto => dto.NUMERO_RADICADO))

                 .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
                 .ForMember(x => x.NombreEstado, map => map.MapFrom(dto => dto.NOMBRE_ESTADO))

                 .ForMember(x => x.SltId, map => map.MapFrom(dto => dto.SLT_ID))
                 .ForMember(x => x.SltIdAprobar, map => map.MapFrom(dto => dto.SLT_ID_APROBAR))

                 .ReverseMap();

            CreateMap<SolicitudConsecutivo, SolicitudConsecutivoDto>()
               .ForMember(x => x.CnsId, map => map.MapFrom(dto => dto.CNS_ID))
               .ForMember(x => x.CnsDescripcion, map => map.MapFrom(dto => dto.CNS_DESCRIPCION))
               .ForMember(x => x.CnsUltimoNumero, map => map.MapFrom(dto => dto.CNS_ULTIMO_NUMERO))
               .ForMember(x => x.CnsEstado, map => map.MapFrom(dto => dto.CNS_ESTADO))
               .ForMember(x => x.CnsCreadoPor, map => map.MapFrom(dto => dto.CNS_CREADO_POR))
               .ForMember(x => x.CnsFechaCreacion, map => map.MapFrom(dto => dto.CNS_FECHA_CREACION))
               .ForMember(x => x.CnsActualizadoPor, map => map.MapFrom(dto => dto.CNS_ACTUALIZADO_POR))
               .ForMember(x => x.CnsFechaActualiza, map => map.MapFrom(dto => dto.CNS_FECHA_ACTUALIZA))
               .ReverseMap();



            CreateMap<ObtenerCuentasAVAC, ObtenerCuentasAVACDto>()

                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))
                .ForMember(x => x.CtaFechaCreacion, map => map.MapFrom(dto => dto.CTA_FECHA_CREACION))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))

                .ForMember(x => x.AVAC, map => map.MapFrom(dto => dto.AVAC))
                .ForMember(x => x.ValorAVAC, map => map.MapFrom(dto => dto.VALOR_AVAC))
                .ForMember(x => x.SaldoAnualAVAC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAC))
                .ForMember(x => x.InteresesAVAC, map => map.MapFrom(dto => dto.INTERESES_AVAC))
                .ForMember(x => x.MovimientosMensualAVAC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAC))

                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                

                .ReverseMap();


            CreateMap<ObtenerCuentasAVAV, ObtenerCuentasAVAVDto>()

                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))
                .ForMember(x => x.CtaFechaCreacion, map => map.MapFrom(dto => dto.CTA_FECHA_CREACION))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))

                .ForMember(x => x.AVAV, map => map.MapFrom(dto => dto.AVAV))
                .ForMember(x => x.ValorAVAV, map => map.MapFrom(dto => dto.VALOR_AVAV))
                .ForMember(x => x.SaldoAnualAVAV, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAV))
                .ForMember(x => x.InteresesAVAV, map => map.MapFrom(dto => dto.INTERESES_AVAV))
                .ForMember(x => x.MovimientosMensualAVAV, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAV))

                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ReverseMap();

            CreateMap<ObtenerCuentasAVC, ObtenerCuentasAVCDto>()

                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TctId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))
                .ForMember(x => x.CtaFechaCreacion, map => map.MapFrom(dto => dto.CTA_FECHA_CREACION))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))

                .ForMember(x => x.AVC, map => map.MapFrom(dto => dto.AVC))
                .ForMember(x => x.ValorAVC, map => map.MapFrom(dto => dto.VALOR_AVC))
                .ForMember(x => x.SaldoAnualAVC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVC))
                .ForMember(x => x.InteresesAVC, map => map.MapFrom(dto => dto.INTERESES_AVC))
                .ForMember(x => x.MovimientosMensualAVC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVC))

                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ReverseMap();

            CreateMap<ObtenerCuentasAVACResult, ObtenerCuentasAVACResultDto>()

                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
                .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))

                .ForMember(x => x.PeriodoAnio, map => map.MapFrom(dto => dto.PERIODO_ANIO))
                .ForMember(x => x.PeriodoMes, map => map.MapFrom(dto => dto.PERIODO_MES))
                .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))


                .ForMember(x => x.ValorAVAC, map => map.MapFrom(dto => dto.VALOR_AVAC))
                .ForMember(x => x.SaldoAnualAVAC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAC))
                .ForMember(x => x.MovimientosMensualAVAC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAC))
                .ForMember(x => x.InteresesAVAC, map => map.MapFrom(dto => dto.INTERES_AVAC))
                .ForMember(x => x.InteresesNoCausadoAVAC, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_AVAC))
                .ForMember(x => x.InteresesCausadoAVAC, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_AVAC))
                .ForMember(x => x.SaldoAnteriorAVAC, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_AVAC))
                .ForMember(x => x.SaldoFinalAVAC, map => map.MapFrom(dto => dto.SALDO_FINAL_AVAC))


                .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
                .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))

                .ForMember(x => x.InteresNoAbonadosSaldoInsuficienteAVAC, map => map.MapFrom(dto => dto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC))

                .ReverseMap();           


            CreateMap<ObtenerCuentasAVAVResult, ObtenerCuentasAVAVResultDto>()

               .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
               .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
               .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
               .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))

               .ForMember(x => x.PeriodoAnio, map => map.MapFrom(dto => dto.PERIODO_ANIO))
               .ForMember(x => x.PeriodoMes, map => map.MapFrom(dto => dto.PERIODO_MES))
               .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))


               .ForMember(x => x.ValorAVAV, map => map.MapFrom(dto => dto.VALOR_AVAV))
               .ForMember(x => x.SaldoAnualAVAV, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVAV))
               .ForMember(x => x.MovimientosMensualAVAV, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVAV))
               .ForMember(x => x.InteresesAVAV, map => map.MapFrom(dto => dto.INTERES_AVAV))
               .ForMember(x => x.InteresesNoCausadoAVAV, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_AVAV))
               .ForMember(x => x.InteresesCausadoAVAV, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_AVAV))
               .ForMember(x => x.SaldoAnteriorAVAV, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_AVAV))
               .ForMember(x => x.SaldoFinalAVAV, map => map.MapFrom(dto => dto.SALDO_FINAL_AVAV))


               .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
               .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
               .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

               .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
               .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))

               .ForMember(x => x.InteresNoAbonadosSaldoInsuficienteAVAV, map => map.MapFrom(dto => dto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV))

               .ReverseMap();

            CreateMap<ObtenerCuentasAVCResult, ObtenerCuentasAVCResultDto>()

               .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
               .ForMember(x => x.CtaId, map => map.MapFrom(dto => dto.CTA_ID))
               .ForMember(x => x.CtaNumero, map => map.MapFrom(dto => dto.CTA_NUMERO))
               .ForMember(x => x.EctId, map => map.MapFrom(dto => dto.ECT_ID))

               .ForMember(x => x.PeriodoAnio, map => map.MapFrom(dto => dto.PERIODO_ANIO))
               .ForMember(x => x.PeriodoMes, map => map.MapFrom(dto => dto.PERIODO_MES))
               .ForMember(x => x.CtaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))


               .ForMember(x => x.ValorAVC, map => map.MapFrom(dto => dto.VALOR_AVC))
               .ForMember(x => x.SaldoAnualAVC, map => map.MapFrom(dto => dto.SALDO_ANUAL_AVC))
               .ForMember(x => x.MovimientosMensualAVC, map => map.MapFrom(dto => dto.MOVIMIENTOS_MENSUAL_AVC))
               .ForMember(x => x.InteresesAVC, map => map.MapFrom(dto => dto.INTERES_AVC))
               .ForMember(x => x.InteresesNoCausadoAVC, map => map.MapFrom(dto => dto.INTERESES_NO_CAUSADO_AVC))
               .ForMember(x => x.InteresesCausadoAVC, map => map.MapFrom(dto => dto.INTERESES_CAUSADO_AVC))
               .ForMember(x => x.SaldoAnteriorAVC, map => map.MapFrom(dto => dto.SALDO_ANTERIOR_AVC))
               .ForMember(x => x.SaldoFinalAVC, map => map.MapFrom(dto => dto.SALDO_FINAL_AVC))


               .ForMember(x => x.CliId, map => map.MapFrom(dto => dto.CLI_ID))
               .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
               .ForMember(x => x.UnidadEjecutora, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))

               .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NUMERO_SOLICITUD))
               .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))

               .ForMember(x => x.InteresNoAbonadosSaldoInsuficienteAVC, map => map.MapFrom(dto => dto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC))

               .ReverseMap();




            CreateMap<ParamValoresIPCNotificacionAutomatica, ParamValoresIPCNotificacionAutomaticaDto>()

               .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ID))
               .ForMember(x => x.Anio, map => map.MapFrom(dto => dto.ANIO))
               .ForMember(x => x.Mes, map => map.MapFrom(dto => dto.MES))
               .ForMember(x => x.SltId, map => map.MapFrom(dto => dto.SLT_ID))

               .ReverseMap();

            CreateMap<ObtenerCuentaAVAVResponse, ObtenerCuentaAVAVResponseDto>().ReverseMap();
            CreateMap<DatosCuentasAVDto, CuentaTemp>().ReverseMap();
        }
    }
}
