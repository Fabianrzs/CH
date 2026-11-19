using Dapper;
using Dapper.Transaction;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera;
//using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera;
using GA2.Domain.Entities.Cartera.Balance;
using GA2.Domain.Entities.Cartera.Cierres;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual.Cartera.Entities;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Entities.Cartera.Novedades;
using GA2.Domain.Entities.Cartera.Pagos;
using GA2.Domain.Entities.Cartera.ProductoPorCliente;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Cartera.Comprobantes;
using GA2.Infraestructure.Data.Cartera.Pagos;
using GA2.Infraestructure.Data.Cartera.ProductoPorCliente;
using GA2.Infraestructure.Data.Reportes;
using GA2.Infraestructure.Data.Reportes.Cartera;
using GA2.Infraestructure.Data.Tasa.EnumTasaSimuladorParams;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class CarteraRepository : DapperGenericRepository, ICarteraRepository
    {
        private readonly IConfiguration configuration;

        public CarteraRepository(IConfiguration configuration) : base(configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCredito"></param>
        /// <returns></returns>
        /// <Author>Sergio Ortega Fula</Author>
        /// <Date>19/10/2021</Date>
        public async Task<InformacionCredito> ActualizarCredito(InformacionCredito updateCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.IdCredito), updateCredito.CRE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.NumeroCredito), updateCredito.CRE_NRO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaDesembolso), updateCredito.CRE_FECHA_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorDesembolso), updateCredito.CRE_VALOR_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorCuota), updateCredito.CRE_VALOR_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.PlazoTotal), updateCredito.CRE_PLAZO_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.IdTipoCredito), updateCredito.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.DiaPago), updateCredito.CRE_DIA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.IdEstadoCartera), updateCredito.ESC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaEstado), updateCredito.CRE_FECHA_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.DiasMora), updateCredito.CRE_DIAS_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorVivienda), updateCredito.CRE_VALOR_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.IdTipoVivienda), updateCredito.TIV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.IdEstadoVivienda), updateCredito.TVV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.SaldoCapital), updateCredito.CRE_SALDO_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.SaldoCapitalMora), updateCredito.CRE_SALDO_CAPITAL_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.PlazoActual), updateCredito.CRE_PLAZO_ACTUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.TasaSeguroVida), updateCredito.CRE_TASA_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.TasaSeguroHogar), updateCredito.CRE_TASA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.TasaCreditoEfectivaAnual), updateCredito.CRE_TASA_CREDITO_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.TasaCreditoPer), updateCredito.CRE_TASA_CREDITO_PER);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.TasaFrech), updateCredito.CRE_TASA_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorAlivioCuota), updateCredito.CRE_VALOR_ALIVIO_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaUltimoPago), updateCredito.CRE_FECHA_ULTIMO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaProximoPago), updateCredito.CRE_FECHA_PROXIMO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.TipoAbonoExtra), updateCredito.CRE_TIPO_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorAbonoExtra), updateCredito.CRE_VALOR_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorDeudaRemanente), updateCredito.CRE_VALOR_DEUDA_REMANENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorCanonInicial), updateCredito.CRE_VALOR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ValorOpcionCompra), updateCredito.CRE_VALOR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.PorcentajeCanonInicial), updateCredito.CRE_POR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.PorcentajePorOpcionCompra), updateCredito.CRE_POR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.AcumuladoInteresMora), updateCredito.CRE_ACUMULADO_INTERES_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.AcumuladoInteresCorriente), updateCredito.CRE_ACUMULADO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.AcumuladoSeguroHogar), updateCredito.CRE_ACUMULADO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.AcumuladoSeguroVida), updateCredito.CRE_ACUMULADO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaUltimoPagoInteresMora), updateCredito.CRE_FECHA_ULTIMO_PAGO_INTERES_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaUltimoPagoInteresCorriente), updateCredito.CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaUltimoPagoSeguroHogar), updateCredito.CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaUltimoPagoSeguroVida), updateCredito.CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.AlivioFrech), updateCredito.CRE_ALIVIO_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.NumeroCuotasCanceladas), updateCredito.CRE_NUMERO_CUOTAS_CANCELADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.CreadoPor), updateCredito.CRE_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaCreacion), updateCredito.CRE_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.ModificadoPor), updateCredito.CRE_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.FechaModificacion), updateCredito.CRE_FECHA_MODIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.SaldocapitalTradicional), updateCredito.CRE_SALDO_CAPITAL_TRADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreditoParams.AcumuladoCapital), updateCredito.CRE_ACUMULADO_CAPITAl);



            return await GetAsyncFirst<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aplicacionPago"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AplicacionPago>> AplicarPago(PagoAplicarDto aplicacionPago)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION_PAGO), aplicacionPago.Fecha_Pago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_CUOTA_PAGADA), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CRE_NRO_CREDITO), aplicacionPago.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), aplicacionPago.CreadoPor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.AAP_CODIGO), aplicacionPago.Codigo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR_PAGO), aplicacionPago.ValorPago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TPA_ID), aplicacionPago.TipoActor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CUOTAS_ACUMULADAS), aplicacionPago.CuotasAcumuladas);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.NUMERO_CUOTAS), aplicacionPago.NumeroCuotas);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.APLICA_SIMULACION), aplicacionPago.AplicaSimulacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TCR_ID), aplicacionPago.Tcr_id);
            parameters.Add("@TipoRecurso", aplicacionPago.TipoRecurso);
            parameters.Add("@INCLUIR_MORA", aplicacionPago.IncluyeMora);

            return await GetAsyncList<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<AplicacionPago> AplicarPagoMora(PagoAplicarDto aplicacionPago)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CRE_NRO_CREDITO), aplicacionPago.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR_PAGO), aplicacionPago.ValorPago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION_PAGO), aplicacionPago.Fecha_Pago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_CUOTA_PAGADA), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), aplicacionPago.CreadoPor);

            parameters.Add("@CPG_ABONO_SEGURO_VIDA", aplicacionPago.SeguroVidaPendiente);
            parameters.Add("@CPG_ABONO_SEGURO_HOGAR", aplicacionPago.SeguroHogarPendiente);
            parameters.Add("@CPG_ABONO_INTERES_MORA", aplicacionPago.InteresMorePendiente);
            parameters.Add("@CPG_ABONO_INTERES_CORRIENTE", aplicacionPago.InteresCorrientePendiente);
            parameters.Add("@CPG_ABONO_CAPITAL_MORA", aplicacionPago.CapitalMorePendiente);
            parameters.Add("@CPG_ABONO_CAPITAL", aplicacionPago.CapitalPendiente);
            parameters.Add("@OBSERVACIONES", aplicacionPago.Observaciones ?? "");
            parameters.Add("@TOTAL_PAGADO_MES", aplicacionPago.TotalPagadoMes);
            parameters.Add("@TOTAL_PLAN_MES", aplicacionPago.TotalPlanMes);
            parameters.Add("@TIPORECURSO", aplicacionPago.TipoRecurso);

            return await GetAsyncFirst<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }
        public async Task<AplicacionPago> AplicarPagoLiquidacionLeasing(PagoAplicarDto aplicacionPago)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION_PAGO), aplicacionPago.Fecha_Pago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_CUOTA_PAGADA), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CRE_NRO_CREDITO), aplicacionPago.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), aplicacionPago.CreadoPor);
            //  parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.AAP_CODIGO), aplicacionPago.Codigo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR_PAGO), aplicacionPago.ValorPago);
            // parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TPA_ID), aplicacionPago.TipoActor);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CUOTAS_ACUMULADAS), aplicacionPago.CuotasAcumuladas);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.NUMERO_CUOTAS), aplicacionPago.NumeroCuotas);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.APLICA_SIMULACION), aplicacionPago.AplicaSimulacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TCR_ID), aplicacionPago.Tcr_id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TIPO_RECURSO), aplicacionPago.TipoRecurso);

            return await GetAsyncFirst<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<AplicacionPago> AplicarPagoOpcionCompra(PagoAplicarDto aplicacionPago)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION_PAGO), aplicacionPago.Fecha_Pago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_CUOTA_PAGADA), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CRE_NRO_CREDITO), aplicacionPago.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), aplicacionPago.CreadoPor);
            //  parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.AAP_CODIGO), aplicacionPago.Codigo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR_PAGO), aplicacionPago.ValorPago);
            // parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TPA_ID), aplicacionPago.TipoActor);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CUOTAS_ACUMULADAS), aplicacionPago.CuotasAcumuladas);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.NUMERO_CUOTAS), aplicacionPago.NumeroCuotas);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.APLICA_SIMULACION), aplicacionPago.AplicaSimulacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TCR_ID), aplicacionPago.Tcr_id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TIPO_RECURSO), aplicacionPago.TipoRecurso);

            return await GetAsyncFirst<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<AplicacionPago> ReversarPago(RequestReversionPagoDto reversionPagoDto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.IdAplicacionPago), reversionPagoDto.IdPago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), reversionPagoDto.CreadoPor);

            return await GetAsyncFirst<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<bool> ReversarProcesoAplicacionPagos(int idProceso)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), idProceso);
            var result = await ExecuteAsync(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return result > 0;
        }        
        public async Task<bool> ReversarPagoUnitarioDeLista(RequestReversionPagoListaDto request)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.CpgID), request.CpgId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.CreadoPor), request.CreadoPor);
            var result = await ExecuteAsync(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return result > 0;
        }


        /// <summary>
        /// Obtener Abono Extra
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        public async Task<AbonoExtra> ObtenerAbonoExtra(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAbonoExtraParams.Identificacion), identificacion);

            return await GetAsyncFirst<AbonoExtra>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <Author>Sergio Ortega Fula</Author>
        /// <Date>19/10/2021</Date>
        public async Task<InformacionCredito> ObtenerCreditoCartera(ObtenerProductoCartera obtenerProductoCartera)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.NumeroCredito), obtenerProductoCartera.Nro_Credito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.idTCR), obtenerProductoCartera.Tcr_Id);

            return await GetAsyncFirst<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<double> ObtenerCapitalNeto_AbonosExtra(string Cli_Identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CLI_IDENTIFICACION", Cli_Identificacion);
            var Resultado = await GetAsyncFirst<double>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return Resultado;
        }

        /// <summary>
        /// Obtiene parametros pagos
        /// </summary>
        /// <returns></returns>
        public async Task<ParametrosAplicacionPagos> ObtenerParametrosAplicacionPagos()
        {
            return await GetAsyncFirst<ParametrosAplicacionPagos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los productos de cada cliente
        /// </summary>
        /// <param name="productoCliente"></param>
        /// <returns></returns>
        /// <Author>Sergio Ortega Fula</Author>
        /// <Date>06/09/2021</Date>
        public async Task<IEnumerable<ProductoCliente>> ObtenerProductoCliente(int tipoIdentificacion, string identificacion, Guid? idProducto)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.identificacion), identificacion, DbType.String);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.tipoIdentificacion), tipoIdentificacion, DbType.Int32);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.idProducto), idProducto, DbType.Guid);

            var result = await GetAsyncList<ProductoCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<CreditosConPagosEnAnio>> ObtenerCreditosConPagosEnAnio(int anio, int tipoCredito)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Anio", anio);
            parameters.Add("@TipoCredito", tipoCredito);

            var result = await GetAsyncList<CreditosConPagosEnAnio>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<PagosAplicadosCreditoAnio>> ObtenerPagosAplicadosCreditoAnio(int anio, Guid creId)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Anio", anio);
            parameters.Add("@CreId", creId);

            var result = await GetAsyncList<PagosAplicadosCreditoAnio>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<ProductoCliente>> ObtenerProductosCliente(int tipoIdentificacion, string identificacion)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.identificacion), identificacion, DbType.String);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.tipoIdentificacion), tipoIdentificacion, DbType.Int32);

            var result = await GetAsyncList<ProductoCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }

        /// <summary>
        /// Crear la restitucion de inmueble de cartera
        /// </summary>
        /// <param name="productoCliente"></param>
        /// <returns></returns>
        /// <Author>Sergio Ortega Fula</Author>
        /// <Date>06/09/2021</Date>
        public async Task<int> CreateRestitucionInmuebleCartera(RestitucionInmuebleCartera request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RIC_TIPO_NOVEDAD", request.TipoNovedad);
                parameters.Add("@RIC_TIPO_DOCUMENTO", request.TipoDpcumento);
                parameters.Add("@RIC_NUMERO_DOCUMENTO", request.NumeroDocumento);
                parameters.Add("@RIC_ID_CREDITO", request.IdCredito);
                parameters.Add("@RIC_FECHA_NOVEDAD", request.FechaNovedad);
                parameters.Add("@RIC_FECHA_FINALIZACION", request.FechaFinalizacion);
                parameters.Add("@RIC_CREADO_POR", request.CreadoPor);
                parameters.Add("@RIC_FECHA_CREACION", DateTime.Now);
                parameters.Add("@RIC_CAPITAL", request.Capital);
                parameters.Add("@RIC_SEGURO_VIDA", request.SeguroVida);
                parameters.Add("@RIC_SEGURO_HOGAR", request.SeguroHogar);
                parameters.Add("@RIC_INTERESES_CORRIENTES", request.InteresCorriente);
                parameters.Add("@RIC_INTERESES_MORA", request.InteresMora);
                parameters.Add("@RIC_INTERES_REMANENTE", request.InteresRemanente);
                parameters.Add("@RIC_CAPITAL_REMANENTE", request.CapitalRemanente);
                parameters.Add("@RIC_TOTAL_DEUDA", request.TotalDeuda);
                parameters.Add("@RIC_VALOR_RESTANTE", request.ValorRestante);

                var result = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }

        }
        public async Task<IEnumerable<RestitucionInmuebleCartera>> ObtenerRestitucionInmuebleCartera(Guid idCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RIC_ID_CREDITO", idCredito);

            var result = await GetAsyncList<RestitucionInmuebleCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;

        }
        public async Task<Guid> AplicarPagoCancelacionRestitucion(PagoAplicarDto request)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CRE_NRO_CREDITO", request.NumeroCredito);
            parameters.Add("@VALOR_PAGO", request.ValorPago);
            parameters.Add("@FECHA_APLICACION_PAGO", request.Fecha_Pago);
            parameters.Add("@FECHA_CUOTA_PAGADA", request.Fecha_Pago_Cuota);
            parameters.Add("@CREADO_POR", request.CreadoPor);
            parameters.Add("@APLICA_SIMULACION", request.AplicaSimulacion);
            parameters.Add("@TCR_ID", request.Tcr_id);
            var result = await GetAsyncFirst<Guid>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<Guid> AplicarPagoCancelacionRestructuracion(PagoAplicarDto request)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CRE_NRO_CREDITO", request.NumeroCredito);
            parameters.Add("@VALOR_PAGO", request.ValorPago);
            parameters.Add("@FECHA_APLICACION_PAGO", request.Fecha_Pago);
            parameters.Add("@FECHA_CUOTA_PAGADA", request.Fecha_Pago_Cuota);
            parameters.Add("@CREADO_POR", request.CreadoPor);
            parameters.Add("@APLICA_SIMULACION", request.AplicaSimulacion);
            parameters.Add("@TCR_ID", request.Tcr_id);
            parameters.Add("@Valor_Intereses_Remanente", request.ValorInteresRemanente);
            parameters.Add("@Valor_Capital_Remanente", request.ValorCapitalRemanente);
            var result = await GetAsyncFirst<Guid>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> ActualizarEstadoCreditoRestituido(string identificacion, int tipoIdentificacion, Guid creditoid)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CLI_IDENTIFICACION", identificacion);
            parameters.Add("@CLI_TIPO_IDENTIFICACION", tipoIdentificacion);
            parameters.Add("@CRE_ID_CREDITO", creditoid);
            var result = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }



        /// <summary>
        /// Ejecutar desembolso
        /// </summary>
        /// <param name="producto"></param>
        /// <returns></returns>
        public async Task<ProductoCliente> EjecutarDesembolso(ProductoCliente producto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdCredito), producto.CRE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaDesembolso), producto.CRE_FECHA_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorDesembolso), producto.CRE_VALOR_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorCuota), producto.CRE_VALOR_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PlazoTotal), producto.CRE_PLAZO_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdTipoCredito), producto.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.DiaPago), producto.CRE_DIA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdEstadoCartera), producto.ESC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaEstadoCartera), producto.CRE_FECHA_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.DiasMora), producto.CRE_DIAS_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorVivienda), producto.CRE_VALOR_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdTipoVivienda), producto.TIV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdTipoViviendaV), producto.TVV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.SaldoCapital), producto.CRE_SALDO_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.SaldoCapitalMora), producto.CRE_SALDO_CAPITAL_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PlazoActual), producto.CRE_PLAZO_ACTUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaSeguroVida), producto.CRE_TASA_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaSeguroHogar), producto.CRE_TASA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaCreditoEA), producto.CRE_TASA_CREDITO_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaCreditoPer), producto.CRE_TASA_CREDITO_PER);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaFrech), producto.CRE_TASA_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorAlivioCuota), producto.CRE_VALOR_ALIVIO_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaUltimoPago), producto.CRE_FECHA_ULTIMO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaProximoPago), producto.CRE_FECHA_PROXIMO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TipoAbonoExtra), producto.CRE_TIPO_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorAbonoExtra), producto.CRE_VALOR_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorDeudaRemanente), producto.CRE_VALOR_DEUDA_REMANENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorCanonInicial), producto.CRE_VALOR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorOpcionCompra), producto.CRE_VALOR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PorcentajeCanonInicial), producto.CRE_POR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PorcentajeOpcionCompra), producto.CRE_POR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.AcumuladoInteresMora), producto.CRE_ACUMULADO_INTERES_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.UltimoPagoInteresCorriente), producto.CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.AcumuladoInteresCorriente), producto.CRE_ACUMULADO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.AcumuladoSeguroHogar), producto.CRE_ACUMULADO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.UltimoPagoSeguroHogar), producto.CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.AcumuladoSeguroVida), producto.CRE_ACUMULADO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.UltimoPagoSeguroVida), producto.CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.AlivioFrech), producto.CRE_ALIVIO_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.NumeroCuotasCanceladas), producto.CRE_NUMERO_CUOTAS_CANCELADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.CreadoPor), producto.CRE_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaCreacion), producto.CRE_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ModificadoPor), producto.CRE_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaModificacion), producto.CRE_FECHA_MODIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.NumeroCredito), producto.CRE_NRO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Identificacion), producto.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Score), producto.SOC_SCORE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TipoIdentificacion), producto.TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Categoria), producto.CATEGORIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Grado), producto.GRADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.DireccionInmueble), producto.CRE_DIRECCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.CiudadInmueble), producto.CIUDAD_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.NoMatricula), producto.NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Escritura), producto.ESCRITURA_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaEscritura), producto.FECHA_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Notaria), producto.NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Linderos), producto.SIJ_LINDEROS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.DescripcionInmueble), producto.DESCRIPCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaSolicitud), producto.FECHA_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaFinPagos), producto.CRE_FECHA_FINALIZA_PAGOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Edad), producto.CLI_EDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.CanonExtraordinario), producto.CRE_CANON_EXTRAORDINARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ClaseInmueble), producto.CLASE_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaCausacion), producto.CRE_FECHA_CAUSACION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.UltimoPagoInteresMora), producto.CRE_FECHA_ULTIMO_PAGO_INTERES_MORA);


            var result = await GetAsyncFirst<ProductoCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<ProductoCliente> PrepararEjecucionDesembolso(int tipoIdentificacion, string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.identificacion), identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumProductosPorClienteParameters.tipoIdentificacion), tipoIdentificacion);

            var result = await GetAsyncFirst<ProductoCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<TasaSimulador> ActualizarTasasSimulacion(TasaSimulador tasaUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.IdSimulacion), tasaUpdate.SIM_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.Descripcion), tasaUpdate.SIM_DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.TasaEA), tasaUpdate.SIM_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MinimoPlazoMeses), tasaUpdate.SIM_MINIMO_MESES_PLAZO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MaximoPlazoMeses), tasaUpdate.SIM_MAXIMO_MESES_PLAZO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.Estado), tasaUpdate.SIM_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.ModificadoPor), tasaUpdate.SIM_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.FechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MinCanonInicial), tasaUpdate.SIM_MIN_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MaxCanonInicial), tasaUpdate.SIM_MAX_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MaxOpcCompra), tasaUpdate.SIM_MAX_OPC_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.TasaNominalMensual), tasaUpdate.SIM_TASA_NOMINAL_MENSUAL);

            return await GetAsyncFirst<TasaSimulador>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TasaSimulador>> ObtenerTasasSimulador()
        {
            return await GetAsyncList<TasaSimulador>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<TasaSimulador> CrearTasasSimulacion(TasaSimulador tasaCreate)
        {
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.IdSimulacion), tasaCreate.SIM_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.Descripcion), tasaCreate.SIM_DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.TasaEA), tasaCreate.SIM_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MinimoPlazoMeses), tasaCreate.SIM_MINIMO_MESES_PLAZO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MaximoPlazoMeses), tasaCreate.SIM_MAXIMO_MESES_PLAZO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.Estado), 1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.CreadoPor), tasaCreate.SIM_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.FechaCreacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.ModificadoPor), tasaCreate.SIM_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.FechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MinCanonInicial), tasaCreate.SIM_MIN_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MaxCanonInicial), tasaCreate.SIM_MAX_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.MaxOpcCompra), tasaCreate.SIM_MAX_OPC_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.TasaNominalMensual), tasaCreate.SIM_TASA_NOMINAL_MENSUAL);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.SeguroHogar), tasaCreate.SIM_SEGURO_HOGAR);

            return await GetAsyncFirst<TasaSimulador>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<TasaSimulador> EliminarRegistroSimuladorPorId(TasaSimulador tasaDelete)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.IdSimulacion), tasaDelete.SIM_ID);

            return await GetAsyncFirst<TasaSimulador>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TasaSimulador>> ConsultarTipoActorPorId(TasaSimulador consultaExcepcion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.IdSimulacion), consultaExcepcion.SIM_ID);

            return await GetAsyncList<TasaSimulador>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoActor>> ObtenerTipoActor()
        {

            return await GetAsyncList<TipoActor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<ParametrosTasaActor> CrearTasaActor(ParametrosTasaActor actorCreate)
        {
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.IdSimulacion), tasaCreate.SIM_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.SimuladorId), actorCreate.SIM_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroId), actorCreate.PAR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.TipoId), actorCreate.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroTasaEa), actorCreate.PAR_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroCreadoPor), actorCreate.PAR_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroFechaCreacion), DateTime.Now);


            return await GetAsyncFirst<ParametrosTasaActor>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<ParametrosTasaActor> ActualizarTasaSimuladorActor(ParametrosTasaActor actorUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroId), actorUpdate.PAR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.TipoId), actorUpdate.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroTasaEa), actorUpdate.PAR_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroEstado), actorUpdate.PAR_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroModificadoPor), actorUpdate.PAR_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroFechaModificacion), DateTime.Now);


            return await GetAsyncFirst<ParametrosTasaActor>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParametrosTasaActor> EliminarParametroSimuladorActor(ParametrosTasaActor actorDelete)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActorSimuladorParams.ParametroId), actorDelete.PAR_ID);

            return await GetAsyncFirst<ParametrosTasaActor>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UnidadesEjecutoras>> ObtenerUnidadesEjecutorasPorId(UnidadesEjecutoras consultaUnidadEjecutora)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idSimulacion), consultaUnidadEjecutora.SIM_ID);

            return await GetAsyncList<UnidadesEjecutoras>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<UnidadesEjecutoras> CrearUnidadEjecutoraSimulador(UnidadesEjecutoras unidadEjecutoraCreate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idSimulacion), unidadEjecutoraCreate.SIM_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idUnidadEjecutora), unidadEjecutoraCreate.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.tasaEa), unidadEjecutoraCreate.PAR_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.creadoPor), unidadEjecutoraCreate.PAR_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.fechaCreacion), DateTime.Now);


            return await GetAsyncFirst<UnidadesEjecutoras>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UnidadesEjecutoras>> ConsultarUnidadEjecutora()
        {

            return await GetAsyncList<UnidadesEjecutoras>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<UnidadEjecutora> ConsultarUnidadEjecutoraPorId(int unidadEjecutoraID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idUnidadEjecutora), unidadEjecutoraID);


            return await GetAsyncFirst<UnidadEjecutora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<UnidadesEjecutoras> ActualizarUnidadEjecutoraSimulador(UnidadesEjecutoras unidadEjecutoraUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idParametro), unidadEjecutoraUpdate.PAR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idUnidadEjecutora), unidadEjecutoraUpdate.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.tasaEa), unidadEjecutoraUpdate.PAR_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.modificadoPor), unidadEjecutoraUpdate.PAR_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.estado), unidadEjecutoraUpdate.PAR_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.fechaModificacion), DateTime.Now);


            return await GetAsyncFirst<UnidadesEjecutoras>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<UnidadesEjecutoras> EliminarUnidadEjecutoraSimulador(UnidadesEjecutoras unidadEjecutoraDelete)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUnidadesEjecutorasParams.idParametro), unidadEjecutoraDelete.PAR_ID);

            return await GetAsyncFirst<UnidadesEjecutoras>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesCreditoYCartera> ObtenerParamGeneralesCreditoYCartera()
        {

            return await GetAsyncFirst<ParamGeneralesCreditoYCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesCreditoYCartera> ActualizarParamGeneralesCreditoYCartera(ParamGeneralesCreditoYCartera paramCreditoYCarteraUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.parametroSalarioMinimo), paramCreditoYCarteraUpdate.PRM_SMLV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeInflacion), paramCreditoYCarteraUpdate.PRM_PORC_INFLACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeViviendaVis), paramCreditoYCarteraUpdate.PRM_PORC_FINANCIA_VIS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeViviendaNoVis), paramCreditoYCarteraUpdate.PRM_PORC_FINANCIA_NO_VIS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.limiteSalariosVis), paramCreditoYCarteraUpdate.PRM_NO_SALARIO_MIN_VIS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.diasPreaprobado), paramCreditoYCarteraUpdate.PRM_DIAS_VALIDO_PREAPROB);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.puntajeMinimoCentrales), paramCreditoYCarteraUpdate.PRM_SCORE_MINIMO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeMinimoEndeudamiento), paramCreditoYCarteraUpdate.PRM_PORC_CAPACIDAD_ENDEUDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.vigenciaDiasConsultaCentrales), paramCreditoYCarteraUpdate.PRM_VIGENCIA_CONSULTA_BURO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeMinimoCanonInicial), paramCreditoYCarteraUpdate.PRM_PORC_CANON_INI_LSG);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeMaximoCanonInicial), paramCreditoYCarteraUpdate.PRM_PORC_CANON_FIN_LSG);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeMaximoOpcionCompra), paramCreditoYCarteraUpdate.PRM_PORC_OPC_COMPRA_LSG);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.maximoMesesAlivioTasa), paramCreditoYCarteraUpdate.PRM_MESES_LEY_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.numeroMesesAlivio), paramCreditoYCarteraUpdate.PRM_MESES_SUBSIDIO_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.factorAhorro), paramCreditoYCarteraUpdate.PRM_FACTOR_AHORRO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.factorCesantias), paramCreditoYCarteraUpdate.PRM_FACTOR_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.porcentajeIncrementoAnual), paramCreditoYCarteraUpdate.PRM_PORC_INCREMENTO_ANUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.tarifaUnicaAvaluos), paramCreditoYCarteraUpdate.PRM_TARIFA_UNICA_AVALUOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.tarifaEstudioTitulos), paramCreditoYCarteraUpdate.PRM_TARIFA_ESTUDIO_TITULOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.valorMinimoCuota), paramCreditoYCarteraUpdate.PRM_VALOR_MINIMO_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.tasaUsura), paramCreditoYCarteraUpdate.PRM_TASA_USURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.fechaTasaUsura), paramCreditoYCarteraUpdate.PRM_FECHA_TASA_USURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.tasaMora), paramCreditoYCarteraUpdate.PRM_TASA_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.fechaTasaMora), paramCreditoYCarteraUpdate.PRM_FECHA_TASA_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.Transunion), paramCreditoYCarteraUpdate.PRM_TRANSUNION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.factorSubsidio), paramCreditoYCarteraUpdate.PRM_PORC_INCREMENTO_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.diasAprobado), paramCreditoYCarteraUpdate.PRM_DIAS_VALIDO_APROB);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.ScoreTU_Min), paramCreditoYCarteraUpdate.PRM_SCORE_TU_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.valor_SMMLV), paramCreditoYCarteraUpdate.PRM_SMMLV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.Meses_diferir), paramCreditoYCarteraUpdate.PRM_MESES_DIFERIR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.BlobStorage), paramCreditoYCarteraUpdate.PRM_BLOBSTORAGE);
            parameters.Add("PRM_TASA_USURA_PERIODICA", paramCreditoYCarteraUpdate.PRM_TASA_USURA_PERIODICA);
            parameters.Add("PRM_VALOR_IPV_ANUAL", paramCreditoYCarteraUpdate.PRM_VALOR_IPV_ANUAL);
            parameters.Add("PRM_VALOR_IPV_ANUAL_OTRAS_CIUDADES", paramCreditoYCarteraUpdate.PRM_VALOR_IPV_ANUAL_OTRAS_CIUDADES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.RangoAscenso), paramCreditoYCarteraUpdate.PRM_RANGO_ASCENSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.CesantiasIntereses), paramCreditoYCarteraUpdate.PRM_CESANTIAS_INTERESES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.AhorroInteres), paramCreditoYCarteraUpdate.PRM_AHORRO_INTERESES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.IncluirObligacionesExternasCapacidadPago), paramCreditoYCarteraUpdate.PRM_OBLIGACIONES_EXTERNAS_CAPACIDAD_PAGO);



            return await GetAsyncFirst<ParamGeneralesCreditoYCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InformacionCredito> ObtenerCreditoCarteraPorIdentificacion(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.Identificacion), identificacion);

            return await GetAsyncFirst<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<InformacionCredito>ObtenerCreditoEnMoraCarteraPorIdentificacion(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.Identificacion), identificacion);

            return await GetAsyncFirst<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        //endpoints excepcion plazo credito
        public async Task<IEnumerable<ParamGeneralesExcepcionPlazo>> ObtenerParametrosGeneralesPlazoCredito()
        {
            return await GetAsyncList<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesExcepcionPlazo> CrearExcepcionPlazo(ParamGeneralesExcepcionPlazo plazoCreate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoAnio), plazoCreate.PLAZ_YEAR_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoMes), plazoCreate.PLAZ_MONTH_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoAnio), plazoCreate.PLAZ_YEAR_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoMes), plazoCreate.PLAZ_MONTH_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoModificadoPor), plazoCreate.PLAZ_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoFechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.IdTipoActor), plazoCreate.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.tipoCredito), plazoCreate.TCR_ID);


            return await GetAsyncFirst<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesExcepcionPlazo> ActualizarParametrosGeneralesPlazoCredito(ParamGeneralesExcepcionPlazo plazo)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoId), plazo.PLAZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoAnio), plazo.PLAZ_YEAR_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoMes), plazo.PLAZ_MONTH_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoAnio), plazo.PLAZ_YEAR_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoMes), plazo.PLAZ_MONTH_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoFechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoEstado), plazo.PLAZ_ESTADO);


            return await GetAsyncFirst<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<ParamGeneralesExcepcionPlazo> EliminarExcepcionPlazo(ParamGeneralesExcepcionPlazo plazoDelete)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoId), plazoDelete.PLAZ_ID);

            return await GetAsyncFirst<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesExcepcionPlazo> ActualizarExcepcionesPlazo(ParamGeneralesExcepcionPlazo excepcionUpdate)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoId), excepcionUpdate.PLAZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoAnio), excepcionUpdate.PLAZ_YEAR_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoMes), excepcionUpdate.PLAZ_MONTH_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoAnio), excepcionUpdate.PLAZ_YEAR_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoMes), excepcionUpdate.PLAZ_MONTH_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoFechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoEstado), excepcionUpdate.PLAZ_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.IdTipoActor), excepcionUpdate.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.idExcepcion), excepcionUpdate.ID_EXE_PLAZO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoModificadoPor), excepcionUpdate.PLAZ_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.tipoCredito), excepcionUpdate.TCR_ID);

            return await GetAsyncFirst<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ParamGeneralesExcepcionPlazo>> ObtenerExcepcionesPlazo(ParamGeneralesExcepcionPlazo getPlazo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.tipoCredito), getPlazo.TCR_ID);


            return await GetAsyncList<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ParamGeneralesExcepcionPlazo>> ObtenerExcepcionPlazoPorUnidadEjecutora(ParamGeneralesExcepcionPlazo getPlazo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.tipoCredito), getPlazo.TCR_ID);


            return await GetAsyncList<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesExcepcionPlazo> CrearExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazo plazoCreate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoAnio), plazoCreate.PLAZ_YEAR_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoMes), plazoCreate.PLAZ_MONTH_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoAnio), plazoCreate.PLAZ_YEAR_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoMes), plazoCreate.PLAZ_MONTH_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoModificadoPor), plazoCreate.PLAZ_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoFechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.unidadEjecutoraId), plazoCreate.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.tipoCredito), plazoCreate.TCR_ID);


            return await GetAsyncFirst<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesExcepcionPlazo> ActualizarExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazo plazoUpdate)

        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoId), plazoUpdate.PLAZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoAnio), plazoUpdate.PLAZ_YEAR_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMinimoMes), plazoUpdate.PLAZ_MONTH_MIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoAnio), plazoUpdate.PLAZ_YEAR_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoMaximoMes), plazoUpdate.PLAZ_MONTH_MAX);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoFechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoEstado), plazoUpdate.PLAZ_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.unidadEjecutoraId), plazoUpdate.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.idExcepcionPlazo), plazoUpdate.ID_EXC_PLAZO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.plazoModificadoPor), plazoUpdate.PLAZ_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesExcepcionPlazo.tipoCredito), plazoUpdate.TCR_ID);

            return await GetAsyncFirst<ParamGeneralesExcepcionPlazo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        //endpoints seguro general

        public async Task<IEnumerable<ParamGeneralesSeguros>> ObtenerSeguroGeneral()
        {
            return await GetAsyncList<ParamGeneralesSeguros>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        public async Task<ParamGeneralesSeguros> ActualizarSeguroGeneral(ParamGeneralesSeguros seguroUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.idSeguro), seguroUpdate.SEG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroVida), seguroUpdate.SEG_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroTodoRiesgo), seguroUpdate.SEG_TODO_RIESGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.segIva), seguroUpdate.SEG_IVA);

            return await GetAsyncFirst<ParamGeneralesSeguros>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        //endpoints excepcion por municipio

        public async Task<IEnumerable<ParamGeneralesSeguros>> ObtenerExcepSeguroPorMunicipio()
        {
            return await GetAsyncList<ParamGeneralesSeguros>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesSeguros> CrearExcepcionPorMunicipio(ParamGeneralesSeguros seguroExcepcionCreate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.departamentoId), seguroExcepcionCreate.DPP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.departamentoCodigo), seguroExcepcionCreate.DPD_CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.ciudadId), seguroExcepcionCreate.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.ciudadCodigo), seguroExcepcionCreate.DPC_CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.idSeguro), seguroExcepcionCreate.SEG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroVida), seguroExcepcionCreate.SEG_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroEstado), seguroExcepcionCreate.SEG_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroTodoRiesgo), seguroExcepcionCreate.SEG_TODO_RIESGO);

            return await GetAsyncFirst<ParamGeneralesSeguros>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesSeguros> ActualizarExcepSeguroPorMunicipio(ParamGeneralesSeguros seguroExcepcionUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.idExcepcionSeguro), seguroExcepcionUpdate.ID_EXC_SEGURO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.departamentoId), seguroExcepcionUpdate.DPP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.departamentoCodigo), seguroExcepcionUpdate.DPD_CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.ciudadId), seguroExcepcionUpdate.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.ciudadCodigo), seguroExcepcionUpdate.DPC_CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.idSeguro), seguroExcepcionUpdate.SEG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroVida), seguroExcepcionUpdate.SEG_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroEstado), seguroExcepcionUpdate.SEG_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroFechaModificacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.seguroTodoRiesgo), seguroExcepcionUpdate.SEG_TODO_RIESGO);

            return await GetAsyncFirst<ParamGeneralesSeguros>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamGeneralesSeguros> EliminarExcepcionSeguroPorMunicipio(ParamGeneralesSeguros seguroExcepcionDelete)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSeguroGeneral.idSeguro), seguroExcepcionDelete.SEG_ID);

            return await GetAsyncFirst<ParamGeneralesSeguros>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene la liquidacion general params
        /// </summary>
        /// <returns></returns>
        public async Task<DatosLiquidacion> ObtenerLiquidacionGeneral()
        {
            return await GetAsyncFirst<DatosLiquidacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

        }

        /// <summary>
        /// Actualiza params generales liquidacion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DatosLiquidacion> ActualizarLiquidacionGeneral(DatosLiquidacion request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsLiquidacion.FechaPago), request.LIQ_FECHA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsLiquidacion.FechaCorte), request.LIQ_FECHA_CORTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsLiquidacion.FechaModificacion), request.LIQ_FECHA_MODIFICACION);

            return await GetAsyncFirst<DatosLiquidacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AplicacionPago>> ObtenerAplicacionPago(Guid request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.IdCredito), request);
            return await GetAsyncList<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CuotaAdelantada>> ObtenerCuotasAdelantadas(Guid idCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerCuotasAdelantadas.IdCredito), idCredito);
            return await GetAsyncList<CuotaAdelantada>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CuotaAdelantadaFechaCierre>> AplicarCuotasAdelantadasByFechaActual(DateTime FechaAplicacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarCuotasAdelantadasByFechaActual.FechaAplicacion), FechaAplicacion);
            return await GetAsyncList<CuotaAdelantadaFechaCierre>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }


        /// <summary>
        /// GenerarLista
        /// </summary>
        /// <param name="unidadEjecutora"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PagoListaDetalle>> GenerarLista(GenerarListaParametersDto listaParameters)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGenerarListaParams.IdUnidadEjecutora), listaParameters.UnidadEjecutora);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGenerarListaParams.TipoCredito), listaParameters.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGenerarListaParams.FechaAplicacion), listaParameters.FechaAplicacion);
            return await GetAsyncList<PagoListaDetalle>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ColumnasArchivoCargueMasivo>> ObtenerColumnasArchivoCargueMasivoPorUnidadEjecutora(int unidadEjecutora)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumColumnasArchivoCargueMasivoParams.ID_UNIDAD_EJECUTORA), unidadEjecutora);
            return await GetAsyncList<ColumnasArchivoCargueMasivo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<ExcepcionTasaActor> ObtenerExcepcionesPorTipoAfiliacion(int request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumExcepcionTipoAfiliadoParams.IdTipoActor), request);
            return await GetAsyncFirst<ExcepcionTasaActor>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<ExcepcionTasaUEJ> ObtenerExcepcionesTasaUEj(int simId, int uEJ)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumExcepcioTasaUEJParam.IdSimulador), simId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumExcepcioTasaUEJParam.UEJ), uEJ);
            return await GetAsyncFirst<ExcepcionTasaUEJ>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCierreMensualCartera>> CierreMensualCartera(ParamCierreMensualCartera param)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsCierres.AnioCierre), param.AnioCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsCierres.MesCierre), param.MesCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsCierres.Actualiza), param.Actualiza);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsCierres.CreadoPor), param.CreadoPor);

            return await GetAsyncList<ReporteCierreMensualCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCierreAnualCartera>> CierreAnualCartera(RequestCierreAnualCartera param)
        {
            DynamicParameters parameters = new();
            parameters.Add(EnumParamsCierres.AnioCierre.GetDescription(), param.AnioCierre);
            parameters.Add(EnumParamsCierres.Actualiza.GetDescription(), param.Actualiza);
            parameters.Add(EnumParamsCierres.CreadoPor.GetDescription(), param.CreadoPor);

            return await GetAsyncList<ReporteCierreAnualCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteCierreMensualCartera(int anio, int mes)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreAnualParams.ANIO), anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreAnualParams.MES), mes);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> CrearTempTableAplicacionPagos()
        {
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<ResultadoAplicacionPagos> ActualizarAplicacionDePagos(PagoMasivoNominaDto request, ParametrosAplicacionPagoDto parametrosAplicacionPago)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FUERZA), request.FUERZA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.TIPO_NOMINA), request.TIPO_NOMINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CONCEPTO_COD), request.CONCEPTO_COD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CONCEPTO), request.CONCEPTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FUNCIONARIO), request.FUNCIONARIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CEDULA), request.CEDULA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CODIGO_MILITAR), request.CODIGO_MILITAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR), request.VALOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_DESDE), request.FECHA_DESDE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_HASTA), request.FECHA_HASTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION), parametrosAplicacionPago.FechaAplicacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.APLICA_PAGOS), parametrosAplicacionPago.AplicanPagos);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), parametrosAplicacionPago.CreadoPor);

            return await GetAsyncFirst<ResultadoAplicacionPagos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }



        public async Task<IEnumerable<ResultadoAplicacionPagos>> ActualizarAplicacionDePagos(ParametrosAplicacionPagoMasivoDto aplicacionPagos)
        {
            var result = new List<ResultadoAplicacionPagos>();
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            try
            {

                foreach (var idDetalle in aplicacionPagos.IdDetalles)
                {
                    DynamicParameters parametros = new DynamicParameters();
                    parametros.Add(EnumProcesoAplicacionPago.IdDetalle.GetDescription(), idDetalle);
                    parametros.Add(EnumAplicarPago.APLICA_PAGOS.GetDescription(), aplicacionPagos.AplicanPagos);
                    parametros.Add(EnumAplicarPago.CREADO_POR.GetDescription(), aplicacionPagos.CreadoPor);

                    var aplicarPagoSp = HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO);
                    var resultSp = await connection.QueryFirstAsync<ResultadoAplicacionPagos>(aplicarPagoSp, parametros, transaction, null, CommandType.StoredProcedure);
                    result.Add(resultSp);
                }

                DynamicParameters parametrosAct = new();

                parametrosAct.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), aplicacionPagos.IdProceso);
                parametrosAct.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.ModificadoPor), aplicacionPagos.CreadoPor);
                parametrosAct.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), "APROBADA");

                var actEstadoSp = HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, nameof(ActualizarEstadoProcesoAplicacionPagos));
                var actualizacionResult = await connection.QueryFirstAsync<ProcesoAplicacionPagosMasivo>(actEstadoSp, parametrosAct, transaction, null, CommandType.StoredProcedure);

                await transaction.CommitAsync();
                //await transaction.RollbackAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<NovedadCartera> CrearNovedadCartera(NovedadCartera novedad)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CRE_ID), novedad.CRE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.TNV_ID), novedad.TNV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.FECHA_NOVEDAD), novedad.FECHA_NOVEDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.FECHA_FINALIZACION), novedad.FECHA_FINALIZACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CAN_ID), novedad.CAN_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CREADO_POR), novedad.NVC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.SEGURO_HOGAR), novedad.NVC_SEGURO_HOGAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.SEGURO_VIDA), novedad.NVC_SEGURO_VIDA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CAPITAL), novedad.NVC_CAPITAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.INTERESES_CORRIENTES), novedad.NVC_INTERESES_CORRIENTES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.INTERES_REMANENTE), novedad.NVC_INTERES_REMANENTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.INTERESES_MORA), novedad.NVC_INTERESES_MORA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CAPITAL_REMANENTE), novedad.NVC_CAPITAL_REMANENTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.TOTAL_DEUDA), novedad.NVC_TOTAL_DEUDA);
            parametros.Add("@NVC_VALOR_NUEVA_TASA", novedad.NVC_VALOR_NUEVA_TASA);
            parametros.Add("@NVC_FECHA_PROXIMO_PAGO", novedad.NVC_FECHA_PROXIMO_PAGO);

            var result = await GetAsyncFirst<NovedadCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// Cancela (pone a 0) el Interés Remanente y Capital Por Pagar en una reestructuración
        /// Estos valores se capitalizan en el saldo principal del crédito
        /// </summary>
        public async Task CancelarInteresRemanenteYCapitalPorPagar(long numeroCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);

            await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "CancelarInteresRemanenteYCapitalPorPagar"),
                parametros,
                CommandType.StoredProcedure
            );
        }
        public async Task<NovedadCartera> ActualizarNovedadCartera(NovedadCartera novedad)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.ID_NOVEDAD), novedad.ID_NOVEDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.TNV_ID), novedad.TNV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.FECHA_NOVEDAD), novedad.FECHA_NOVEDAD);
            //parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CAUSAL), novedad.CAUSAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.FECHA_FINALIZACION), novedad.FECHA_FINALIZACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.ENC_ID), novedad.ENC_ID);

            return await GetAsyncFirst<NovedadCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<NovedadCartera> ObtenerNovedadCartera(Guid IdNovedad)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.ID_NOVEDAD), IdNovedad);

            return await GetAsyncFirst<NovedadCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<NovedadCartera>> ObtenerNovedadesCartera(Guid IdCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CRE_ID), IdCredito);

            return await GetAsyncList<NovedadCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<NovedadCartera>> ObtenerNovedadesCarteraPorCreditos(List<Guid> creditoIds)
        {
            if (creditoIds == null || creditoIds.Count == 0)
                return new List<NovedadCartera>();

            // 1. Crear DataTable para TVP
            var tvpCreditoIds = new DataTable();
            tvpCreditoIds.Columns.Add("Id", typeof(Guid));

            foreach (var id in creditoIds)
            {
                tvpCreditoIds.Rows.Add(id);
            }

            // 2. Agregar como Table-Valued Parameter
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@CreditoIds", tvpCreditoIds.AsTableValuedParameter("dbo.GuidListType"));

            // 3. Ejecutar SP
            return await GetAsyncList<NovedadCartera>(
                "ObtenerNovedadesCarteraBatch",
                parametros,
                CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<TiposNovedadesCartera>> ObtenerTiposNovedadesCartera()
        {
            return await GetAsyncList<TiposNovedadesCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TiposCausalesAceleracionCartera>> ObtenerTiposCausalesAceleracionCartera()
        {
            return await GetAsyncList<TiposCausalesAceleracionCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<SaldoObligacion> ObtenerSaldoObligacion(Guid IdCredito)
        {
            var parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNovedadCarteraParameters.CRE_ID), IdCredito);

            return await GetAsyncFirst<SaldoObligacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<AplicacionPago> AplicarPagoNovedadCartera(RequestPagoNovedadDto request, decimal capital, decimal interesRemanente)
        {
            var parametros = new DynamicParameters();
            parametros.Add(EnumNovedadCarteraParameters.ID_NOVEDAD.GetDescription(), request.IdNovedad);
            parametros.Add(EnumNovedadCarteraParameters.VALOR_PAGO.GetDescription(), request.ValorPago);
            parametros.Add(EnumNovedadCarteraParameters.FECHA_APLICACION.GetDescription(), request.FechaAplicacion);
            parametros.Add(EnumNovedadCarteraParameters.CREADO_POR.GetDescription(), request.CreadoPor);
            parametros.Add(EnumNovedadCarteraParameters.APLICAR_PAGO.GetDescription(), request.AplicarPago);
            parametros.Add("@CAPITAL", capital);
            parametros.Add("@INTERES_REMANENTE", interesRemanente);

            return await GetAsyncFirst<AplicacionPago>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadosNovedadCartera>> ObtenerEstadosNovedadCartera()
        {
            return await GetAsyncList<EstadosNovedadCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AlternativosAplicacionPago>> ObtenerAlternativosAplicacionPago()
        {
            return await GetAsyncList<AlternativosAplicacionPago>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PlanPago>> ObtenerPlanPago(ObtenerProductoCartera obtenerProductoCartera)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.CRE_NRO_CREDITO), obtenerProductoCartera.Nro_Credito);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.CRE_TCR_ID), obtenerProductoCartera.Tcr_Id);

            return await GetAsyncList<PlanPago>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<ResponseCreditosDesembolsadosPorPeriodo>> ObtenerCreditosDesembolsadosPorPeriodo(int anio, int mes, int  tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@ANIO",anio);
            parametros.Add("@MES", mes);
            parametros.Add("@TIPO_CREDITO", tipoCredito);

            return await GetAsyncList<ResponseCreditosDesembolsadosPorPeriodo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<int>ActualizarCausacionInteresMora(Guid creId, decimal causacionMora, int diasMora,DateTime fechaUltimo,decimal SaldoMora,decimal moraAnterior)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@CRE_ID", creId);
            parametros.Add("@CAUSACION_INTERES_MORA",causacionMora);
            parametros.Add("@DIAS_MORA", diasMora);
            parametros.Add("@CRE_FECHA_ULTIMO_PAGO_INTERES_MORA", fechaUltimo);
            parametros.Add("@CRE_SALDO_CAPITAL_MORA", SaldoMora);
            parametros.Add("@CRE_INT_MORA_MES_ANTERIOR", moraAnterior);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PlanPago>> ObtenerPlanPagosCredito(ObtenerProductoCartera obtenerProductoCartera)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.CRE_NRO_CREDITO), obtenerProductoCartera.Nro_Credito);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumParamGeneralesCreditoYCartera.CRE_TCR_ID), obtenerProductoCartera.Tcr_Id);

            return await GetAsyncList<PlanPago>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ProductoCliente> CrearProductoCredito(ProductoCliente producto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TipoIdentificacion), producto.TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Identificacion), producto.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.NumeroCredito), producto.CRE_NRO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdTipoCredito), producto.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaSolicitud), producto.FECHA_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaDesembolso), producto.CRE_FECHA_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorDesembolso), producto.CRE_VALOR_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorCuota), producto.CRE_VALOR_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PlazoTotal), producto.CRE_PLAZO_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaSeguroVida), producto.CRE_TASA_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaSeguroHogar), producto.CRE_TASA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.TasaCreditoEA), producto.CRE_TASA_CREDITO_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorVivienda), producto.CRE_VALOR_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.IdTipoViviendaV), producto.TVV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.DireccionInmueble), producto.CRE_DIRECCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.CiudadInmueble), producto.CIUDAD_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.NoMatricula), producto.NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Escritura), producto.ESCRITURA_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.FechaEscritura), producto.FECHA_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Notaria), producto.NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Linderos), producto.SIJ_LINDEROS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.DescripcionInmueble), producto.DESCRIPCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PorcentajeCanonInicial), producto.CRE_POR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.PorcentajeOpcionCompra), producto.CRE_POR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.CanonExtraordinario), producto.CRE_CANON_EXTRAORDINARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Edad), producto.CLI_EDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Categoria), producto.CATEGORIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Grado), producto.GRADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.CreadoPor), producto.CRE_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.Score), producto.SOC_SCORE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.SaldoCapitalTradicional), producto.CRE_SALDO_CAPITAL_TRADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.SaldoInicialRedicido), producto.CRE_SALDO_INICIAL_REDUCIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.ValorCuotaTradicional), producto.CRE_VALOR_CUOTA_TRADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEjecucionDesembolsoParams.UnidadEjecutora), producto.UEJ_ID);

            return await GetAsyncFirst<ProductoCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }
        /// <summary>
        /// Obtiene la aplicación de pagos en un rango de fechas de pago.Obtiene la aplicación de pagos en un rango de fechas de pago.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AplicacionPago>> ObtenerAplicacionPagoByFechaPago(CierreDiarioDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_DESDE), request.fechaPagoDesde);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_HASTA), request.fechaPagoHasta);
            return await GetAsyncList<AplicacionPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EquivalenciaCuentasContablesSeven>> ObtenerEquivalenciaCuentasContablesSevenByTipoCredito(int tcrId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.idTCR), tcrId);

            return await GetAsyncList<EquivalenciaCuentasContablesSeven>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EquivalenciaCuentasContablesSeven>> ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(Int32 codOperacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.codOperacion), codOperacion);

            return await GetAsyncList<EquivalenciaCuentasContablesSeven>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }
        //public async Task<IEnumerable<ReporteGeneralGarantiasDto>> ObtenerValorGarantia(ReporteGeneralGarantiaParametersDto request)
        //{
        //    DynamicParameters parameters = new DynamicParameters();

        //    //parameters.Add(EnumParamsReporteGeneralGarantia.Anio.GetDescription(), parametros.Anio);

        //    parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralGarantia.Anio), request.Anio);
        //    parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralGarantia.Mes), request.Mes);
        //    parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralGarantia.Identificacion), request.Identificacion);

        //    return await GetAsyncList<ReporteGeneralGarantiasDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
        //        parameters, CommandType.StoredProcedure);
        //}
        public async Task<IEnumerable<ReporteGeneralGarantias>> ObtenerValorGarantia(ReporteGeneralGarantiaParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteGeneralGarantia.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteGeneralGarantia.Mes.GetDescription(), parametros.Mes);
            parameters.Add(EnumParamsReporteGeneralGarantia.Identificacion.GetDescription(), parametros.Identificacion);

            return await GetAsyncList<ReporteGeneralGarantias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<InformacionCredito> ObtenerCreditoCarteraById(Guid idCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.IdCredito), idCredito);

            return await GetAsyncFirst<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> CrearMovContableCierreDiario(MovimientoContable request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.CRE_MOV_ID), request.CRE_MOV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.NUM_COMPROBANTE), request.NUM_COMPROBANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.FECHA_MOVIMIENTO), request.FECHA_MOVIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.CUENTA_CONTABLE_SEVEN), request.CUENTA_CONTABLE_SEVEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.NUM_CREDITO), request.NUM_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.TIPO_IDENTIFICACION), request.TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.CLI_IDENTIFICACION), request.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.CONCEPTO_APLICADO), request.CONCEPTO_APLICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.VALOR_MOVIMIENTO), request.VALOR_MOVIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.TIPO_MOVIMIENTO), request.TIPO_MOVIMIENTO);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<MovimientoContableCierreDiario>> ObtenerMovContableCierreDiarioByNumCredito(int numCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovContableParams.NUM_CREDITO), numCredito);

            return await GetAsyncList<MovimientoContableCierreDiario>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UltimoCierre>> ConsultarUltimosCierres()
        {
            return await GetAsyncList<UltimoCierre>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteAltasBajas>> GenerarReporteAltasBajas(int idUnidadEjecutora, int tipoReporte, int periodo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteAltasBajas.IdUnidadEjecutora), idUnidadEjecutora);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteAltasBajas.TipoReporte), tipoReporte);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteAltasBajas.Periodo), periodo);

            return await GetAsyncList<ReporteAltasBajas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ProcesoAplicacionPagosMasivo> InsertarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo aplicacionPagos)
        {
            var parametrosProceso = GenerarParametrosProcesoAplicacionPagos(aplicacionPagos);
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<ProcesoAplicacionPagosMasivo>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                    param: parametrosProceso,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                aplicacionPagos.PAP_ID = result.PAP_ID;

                foreach (var item in aplicacionPagos.PAP_PAGOS)
                {
                    item.PAP_ID = result.PAP_ID;

                    var parametrosDetalle = GenerarParametrosDetallePago(item);
                    var detalleResult = await connection.QueryFirstOrDefaultAsync<PagoListaDetalle>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarAplicacionPagoDetalle"),
                        param: parametrosDetalle,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure
                    );

                    detalleResult.APD_ID = detalleResult.APD_ID;
                }

                await transaction.CommitAsync();

                return aplicacionPagos;

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private DynamicParameters GenerarParametrosProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo proceso)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.FechaAplicacion), proceso.PAP_FECHA_APLICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.PAP_FECHA_PAGO), proceso.PAP_FECHA_PAGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Periodo), proceso.PAP_PERIODO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.UnidadEjecutoraId), proceso.UEJ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.TipoCreditoId), proceso.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.CreadoPor), proceso.PAP_CREADOR_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), proceso.PAP_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Descripcion), proceso.PAP_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.JsonPagos), "");

            return parametros;
        }

        public DynamicParameters GenerarParametrosDetallePago(PagoListaDetalle detalle)
        {
            DynamicParameters parametros = new();

            parametros.Add(EnumProcesoAplicacionPago.IdProceso.GetDescription(), detalle.PAP_ID);
            parametros.Add(EnumProcesoAplicacionPago.IdCredito.GetDescription(), detalle.CRE_ID);
            parametros.Add(EnumProcesoAplicacionPago.ValorSeguroHogar.GetDescription(), detalle.VALOR_SEGURO_HOGAR);
            parametros.Add(EnumProcesoAplicacionPago.ValorSeguroVida.GetDescription(), detalle.VALOR_SEGURO_VIDA);
            parametros.Add(EnumProcesoAplicacionPago.ValorInteresCorriente.GetDescription(), detalle.VALOR_INTERES_CORRIENTE);
            parametros.Add(EnumProcesoAplicacionPago.ValorCapital.GetDescription(), detalle.VALOR_CAPITAL);
            parametros.Add(EnumProcesoAplicacionPago.ValorCuota.GetDescription(), detalle.VALOR_CUOTA);

            return parametros;
        }

        public async Task<ProcesoAplicacionPagosMasivo> ActualizarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo procesoAplicacionPagosMasivo)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), procesoAplicacionPagosMasivo.PAP_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.ModificadoPor), procesoAplicacionPagosMasivo.PAP_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), procesoAplicacionPagosMasivo.PAP_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Descripcion), procesoAplicacionPagosMasivo.PAP_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.JsonPagos), "");

            return await GetAsyncFirst<ProcesoAplicacionPagosMasivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarProcesoAplicacionPagos(int idProceso)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), idProceso);
            var result = await ExecuteAsync(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<ProcesoAplicacionPagosMasivo> ActualizarEstadoProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo procesoAplicacionPagosMasivo)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), procesoAplicacionPagosMasivo.PAP_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.ModificadoPor), procesoAplicacionPagosMasivo.PAP_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), procesoAplicacionPagosMasivo.PAP_ESTADO);

            return await GetAsyncFirst<ProcesoAplicacionPagosMasivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProcesoAplicacionPagosMasivo>> ObtenerProcesoAplicacionPagos(int? idProceso = null, string estado = null)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), idProceso);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), estado);

            return await GetAsyncList<ProcesoAplicacionPagosMasivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProcesoAplicacionPagosMasivo>> ObtenerProcesoAplicacionPagosConDetalle(
            ParametrosObtenerProcesoAplicacionPagosDto request)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.IdProceso), request.IdProceso);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.UnidadEjecutoraId), request.IdUnidadEjecutora);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.TipoCreditoId), request.IdTipoCredito);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Periodo), request.Periodo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), request.Estado);

            var result = await GetAsyncList<ProcesoAplicacionPagosMasivoDetalle>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            var idProcesos = result.Select(x => x.PAP_ID).Distinct();

            var procesos = idProcesos.Select(papId =>
            {
                var proceso = result.First(x => x.PAP_ID == papId);

                return new ProcesoAplicacionPagosMasivo()
                {
                    PAP_ID = proceso.PAP_ID,
                    PAP_FECHA_APLICACION = proceso.PAP_FECHA_APLICACION,
                    PAP_FECHA_PAGO = proceso.PAP_FECHA_PAGO,
                    PAP_PERIODO = proceso.PAP_PERIODO,
                    UEJ_NOMBRE = proceso.UEJ_NOMBRE,
                    TCR_DESCRIPCION = proceso.TCR_DESCRIPCION,
                    UEJ_ID = proceso.UEJ_ID,
                    TCR_ID = proceso.TCR_ID,
                    PAP_FECHA_CREACION = proceso.PAP_FECHA_CREACION,
                    PAP_CREADOR_POR = proceso.PAP_CREADOR_POR,
                    PAP_MODIFICADO_POR = proceso.PAP_MODIFICADO_POR,
                    PAP_ESTADO = proceso.PAP_ESTADO,
                    PAP_DESCRIPCION = proceso.PAP_DESCRIPCION,
                    PAP_JSON_PAGOS = proceso.PAP_JSON_PAGOS,

                };
            });

            var procesosDetalle = procesos.GroupJoin(result, p => p.PAP_ID, d => d.PAP_ID, (proceso, detalles) =>
            {
                proceso.PAP_PAGOS = detalles.Where(x => x.APD_ID != 0)
                  .Select(detalle => new PagoListaDetalle
                  {
                      PAP_ID = detalle.PAP_ID,
                      APD_ID = detalle.APD_ID,
                      FUERZA_NOMBRE = detalle.UEJ_NOMBRE,
                      TIPO_NOMINA = detalle.TIPO_NOMINA,
                      COD_CONCEPTO = detalle.COD_CONCEPTO,
                      CONCEPTO = detalle.TCR_DESCRIPCION,
                      FUNCIONARIO = "",
                      TIPO_IDENTIFICACION = detalle.TIPO_IDENTIFICACION,
                      NRO_IDENTIFICACION = detalle.NRO_IDENTIFICACION,
                      CODIGO_MILITAR = detalle.CODIGO_MILITAR,
                      CRE_ID = detalle.CRE_ID,
                      NUMERO_CREDITO = detalle.NUMERO_CREDITO,
                      VALOR_SEGURO_HOGAR = detalle.VALOR_SEGURO_HOGAR,
                      VALOR_SEGURO_VIDA = detalle.VALOR_SEGURO_VIDA,
                      VALOR_INTERES_CORRIENTE = detalle.VALOR_INTERES_CORRIENTE,
                      VALOR_CAPITAL = detalle.VALOR_CAPITAL,
                      VALOR_CUOTA = detalle.VALOR_CUOTA,
                      FECHA_DESDE = detalle.FECHA_DESDE,
                      FECHA_HASTA = detalle.FECHA_HASTA,
                      CPG_ID = detalle.CPG_ID ?? Guid.Empty,
                  });

                return proceso;
            });

            return procesosDetalle;

        }

        public async Task<IEnumerable<ProcesoAplicacionPagosMasivo>> ObtenerProcesoAplicacionPagosPorUnidadEjecutora(int unidadEjecutoraId, int? tipoCreditoId = null,
            int? periodo = null, string estado = null)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.UnidadEjecutoraId), unidadEjecutoraId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.TipoCreditoId), tipoCreditoId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Periodo), periodo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProcesoAplicacionPago.Estado), estado);

            return await GetAsyncList<ProcesoAplicacionPagosMasivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<PlanPagoAbonoCuota> ConsultarPlanPago(long numeroCredito, Guid cpgId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@NUMERO_CREDITO", numeroCredito);
            parametros.Add("@CPG_ID", cpgId);

            return await GetAsyncFirst<PlanPagoAbonoCuota>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<PlanPagoAbonoCuota>> ConsultarPlanPagoAdelantoCuota(long numeroCredito, Guid cpgId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@NUMERO_CREDITO", numeroCredito);
            parametros.Add("@CPG_ID", cpgId);
            return await GetAsyncList<PlanPagoAbonoCuota>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> UpdatePlanPago(UpdatePlanPago updatePlanPago)
        {
            DynamicParameters parametros = new();

            parametros.Add("@NUMERO_CREDITO", updatePlanPago.numeroCredito);
            parametros.Add("@CPG_ID", updatePlanPago.cpgId);
            parametros.Add("@MCO_CONT", updatePlanPago.mcoCont);
            parametros.Add("@MCO_NUME", updatePlanPago.mcoNume);
            parametros.Add("@CC_ID", updatePlanPago.ccId);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<ComprobanteCarteraPorProcesoDTO> UpdateComprobanteContableCartera(ComprobanteCarteraUpdate comprobanteCarteraUpdate)
        {
            DynamicParameters parametros = new();
            parametros.Add("@Id_Comprobante", comprobanteCarteraUpdate.CC_ID);
            parametros.Add("@Id_ComprobanteSeven", comprobanteCarteraUpdate.CC_COMPROBANTE_SEVEN_ID);
            parametros.Add("@Tipo_operacion", comprobanteCarteraUpdate.TIPO_OPERACION);
            parametros.Add("@Id_Aprobo", comprobanteCarteraUpdate.CC_APROBADO_POR);
            parametros.Add("@EsExitoso", comprobanteCarteraUpdate.ES_EXITOSO);
            parametros.Add("@RespuestaSeven", comprobanteCarteraUpdate.RESPUESTA_SEVEN);
            parametros.Add("@Estado", comprobanteCarteraUpdate.ESTADO);

            return await GetAsyncFirst<ComprobanteCarteraPorProcesoDTO>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> UpdateDetallesComprobanteContableCartera(ComprobanteCarteraUpdate comprobanteCarteraUpdate)
        {
            DynamicParameters parametros = new();

            parametros.Add("@Id_Comprobante", comprobanteCarteraUpdate.CC_ID);
            parametros.Add("@RequestSeven", comprobanteCarteraUpdate.RESPUESTA_SEVEN);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<long> ObtenerCreditoEnMoraPeriodo(long numeroCredito, int mes, int anio )
        {
            DynamicParameters parametros = new();

            parametros.Add("@NRO_CREDITO", numeroCredito);
            parametros.Add("@MES", mes);
            parametros.Add("@ANIO", anio);

            return await GetAsyncFirst<long>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> UpdatePlanPagoAdelantoCuota(UpdatePlanPagoAdelantoCuota updatePlanPago)
        {
            DynamicParameters parametros = new();

            parametros.Add("@NUMERO_CREDITO", updatePlanPago.numeroCredito);
            parametros.Add("@MCO_CONT", updatePlanPago.mcoCont);
            parametros.Add("@MCO_NUME", updatePlanPago.mcoNume);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> UpdatePapAplicacionPagosMasivos(int idProceso, int mcoCont, int mcoNume)
        {
            DynamicParameters parametros = new();

            parametros.Add("@PAP_ID", idProceso);
            parametros.Add("@MCO_CONT", mcoCont);
            parametros.Add("@MCO_NUME", mcoNume);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarCpgDetallesPagosSeven(RequestDetallePago request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.CPG_ID), request.Cpg_id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.EMP_CODI), request.Emp_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.TOP_CODI), request.Top_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.MCO_FECH), request.Mco_fech);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.MOD_CODI), request.Mod_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.MCO_DESC), request.Mco_desc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.CUE_CODI), request.Cue_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.DMC_DESC), request.Dmc_desc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.DMC_CANT), request.Dmc_cant);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.DMC_REFE), request.Dmc_refe);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.DMC_VADB), request.Dmc_vadb);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.DMC_VACR), request.Dmc_vacr);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.TER_CODA), request.Ter_coda);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.ARB_CODC), request.Arb_codc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.ARB_CODA), request.Arb_coda);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.ARB_CODP), request.Arb_codp);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.ARB_CODS), request.Arb_cods);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.MOV_CONTABLE_SEVEN), request.Mov_Contable_Seven, DbType.String, size: -1);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.RESPUESTA_SEVEN), request.Respuesta_Seven);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.FECHA_EJECUCION), request.Fecha_Ejecucion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.CC_ID), request.Cc_Id);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> UpdateCierresCarteraRealizados(RequestCierreMensual requestCierreMensual)
        {
            DynamicParameters parametros = new();

            parametros.Add("@ID", requestCierreMensual.Id);
            parametros.Add("@MCO_CONT_LEASING", requestCierreMensual.McoContLeasing);
            parametros.Add("@MCO_NUME_LEASING", requestCierreMensual.McoNumeLeasing);
            parametros.Add("@MCO_CONT_HIPOTECARIO", requestCierreMensual.McoContHipotecario);
            parametros.Add("@MCO_NUME_HIPOTECARIO", requestCierreMensual.McoNumeHipotecario);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> ActualizarComprobanteGarantiaCartera(int ccId, int ccGarantia)
        {
            DynamicParameters parametros = new();

            parametros.Add("@CC_ID", ccId);
            parametros.Add("@CC_ID_GARANTIA", ccGarantia);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int>ObtenerComprobanteGarantiaCartera(int ccId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@CC_ID", ccId);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> ActualizarNumeroComprobanteCpgAplicacionPago(Guid creditoId ,int comprobanteId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@CRE_ID", creditoId);
            parametros.Add("@CC_ID", comprobanteId);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConsultaSaldos>> ConsultarDataCreditoReportes(ConsultaSaldosRequest param)
        {
            DynamicParameters parametros = new();

            parametros.Add("@TIPO_CREDITO", param.TipoCredito);
            parametros.Add("@NUMERO_OBLIGACION", param.NumeroObligacion);
            parametros.Add("@MES", param.PeriodoActual ? "0" : param.Mes);
            parametros.Add("@ANIO", param.PeriodoActual ? "0" : param.Anio);
            parametros.Add("@PERIODO_ACTUAL", param.PeriodoActual);

            return await GetAsyncList<ConsultaSaldos>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetMoraCredito>> GetMoraCredito(int idProceso)
        {
            DynamicParameters parametros = new();

            parametros.Add("@PAP_ID", idProceso);

            return await GetAsyncList<GetMoraCredito>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PlanPagoAbonoCuota>> ConsultarPlanPagoPorFecha(DateTime fechaAplicacion)
        {
            DynamicParameters parametros = new();

            parametros.Add("@FECHA_APLICACION", fechaAplicacion);
            return await GetAsyncList<PlanPagoAbonoCuota>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> ConsultarPapAplicacionPagosMasivosSeven(int papId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@PAP_ID", papId);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        #region Creacion Creditos Nuevos
        public async Task<bool> CrearProdutoClienteCartera(CrearProductoCliente producto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ID), producto.CRE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_DESEMBOLSO), producto.CRE_FECHA_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_DESEMBOLSO), producto.CRE_VALOR_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_CUOTA), producto.CRE_VALOR_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_PLAZO_TOTAL), producto.CRE_PLAZO_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.TCR_ID), producto.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_DIA_PAGO), producto.CRE_DIA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.ESC_ID), producto.ESC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_ESTADO), producto.CRE_FECHA_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_DIAS_MORA), producto.CRE_DIAS_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_VIVIENDA), producto.CRE_VALOR_VIVIENDA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_AVALUO_COMERCIAL), producto.CRE_VALOR_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.TIV_ID), producto.TIV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.TVV_ID), producto.TVV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_SALDO_CAPITAL), producto.CRE_SALDO_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_SALDO_CAPITAL_MORA), producto.CRE_SALDO_CAPITAL_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_PLAZO_ACTUAL), producto.CRE_PLAZO_ACTUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_TASA_SEGURO_VIDA), producto.CRE_TASA_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_TASA_SEGURO_HOGAR), producto.CRE_TASA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_TASA_CREDITO_EA), producto.CRE_TASA_CREDITO_EA/100);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_TASA_CREDITO_PER), producto.CRE_TASA_CREDITO_PER/100);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_TASA_FRECH), producto.CRE_TASA_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_ALIVIO_CUOTA), producto.CRE_VALOR_ALIVIO_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_ULTIMO_PAGO), producto.CRE_FECHA_ULTIMO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_PROXIMO_PAGO), producto.CRE_FECHA_PROXIMO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_TIPO_ABONO_EXTRA), producto.CRE_TIPO_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_ABONO_EXTRA), producto.CRE_VALOR_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_DEUDA_REMANENTE), producto.CRE_VALOR_DEUDA_REMANENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_CANON_INICIAL), producto.CRE_VALOR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_OPCION_COMPRA), producto.CRE_VALOR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_POR_CANON_INICIAL), producto.CRE_POR_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_POR_OPCION_COMPRA), producto.CRE_POR_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ACUMULADO_INTERES_MORA), producto.CRE_ACUMULADO_INTERES_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_ULTIMO_PAGO_INTERES_MORA), producto.CRE_FECHA_ULTIMO_PAGO_INTERES_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ACUMULADO_INTERES_CORRIENTE), producto.CRE_ACUMULADO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE), producto.CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ACUMULADO_SEGURO_HOGAR), producto.CRE_ACUMULADO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR), producto.CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ACUMULADO_SEGURO_VIDA), producto.CRE_ACUMULADO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA), producto.CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ALIVIO_FRECH), producto.CRE_ALIVIO_FRECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_NUMERO_CUOTAS_CANCELADAS), producto.CRE_NUMERO_CUOTAS_CANCELADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_CREADO_POR), producto.CRE_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_CREACION), producto.CRE_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_MODIFICADO_POR), producto.CRE_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_MODIFICACION), producto.CRE_FECHA_MODIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_NRO_CREDITO), producto.CRE_NRO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CLI_IDENTIFICACION), producto.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.TIPO_IDENTIFICACION), producto.TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CLI_EDAD), producto.CLI_EDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CATEGORIA), producto.CATEGORIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.GRADO), producto.GRADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_DIRECCION_INMUEBLE), producto.CRE_DIRECCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CIUDAD_INMUEBLE), producto.CIUDAD_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.NUMERO_MATRICULA), producto.NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.ESCRITURA_INMUEBLE), producto.ESCRITURA_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.FECHA_ESCRITURA), producto.FECHA_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.NOTARIA), producto.NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.SIJ_LINDEROS), producto.SIJ_LINDEROS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.DESCRIPCION_INMUEBLE), producto.DESCRIPCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.SOC_SCORE), producto.SOC_SCORE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.FECHA_SOLICITUD), producto.FECHA_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_FINALIZA_PAGOS), producto.CRE_FECHA_FINALIZA_PAGOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_CANON_EXTRAORDINARIO), producto.CRE_CANON_EXTRAORDINARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_FECHA_CAUSACION), producto.CRE_FECHA_CAUSACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_PROVISION), producto.CRE_PROVISION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_PROVISION_PORCENTAJE), producto.CRE_PROVISION_PORCENTAJE/10);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_CALIFICACION), producto.CRE_CALIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CLASE_INMUEBLE), producto.CLASE_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.UEJ_ID), producto.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.TCR_ID_ESTADO_CREDITO), producto.TCR_ID_ESTADO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_SALDO_CAPITAL_TRADICIONAL), producto.CRE_SALDO_CAPITAL_TRADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_SALDO_INICIAL_REDUCIDO), producto.CRE_SALDO_INICIAL_REDUCIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_VALOR_CUOTA_TRADICIONAL), producto.CRE_VALOR_CUOTA_TRADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_ACUMULADO_CAPITAL), producto.CRE_ACUMULADO_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CRE_PROVISION_ADICIONAL), producto.CRE_PROVISION_ADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearProductoCliente.CLC_ID), producto.CLC_ID);
            parameters.Add("@CRE_PORCENTAJE_SEGURO_VIDA", producto.CRE_PORCENTAJE_SEGURO_VIDA);
            parameters.Add("@CRE_PORCENTAJE_EXTRA_PRIMA_SEGURO_VIDA", producto.CRE_PORCENTAJE_EXTRA_PRIMA_SEGURO_VIDA);
            parameters.Add("@CRE_ASEGURADORA_SOLIDARIA", producto.CRE_ASEGURADORA_SOLIDARIA);

            var Res = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return Res;
        }
        public async Task<bool> CrearPlanPagosProductoClienteCartera(List<InsertPlanPagosProductoCartera> insertPlanPagosProductoCartera, int TipoCredito, DbConnection connection = null, DbTransaction transaction = null)
        {
            bool abrirConexion = (connection == null);
            try
            {
                // Si no se proporcionó una conexión, abrir una nueva
                if (abrirConexion)
                {
                    connection = await OpenConnectionAsync();
                    transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
                }
                var CreacionPlanPagos = HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO);
                foreach (InsertPlanPagosProductoCartera PLN in insertPlanPagosProductoCartera)
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.TCR_ID), TipoCredito);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_NUMERO_CREDITO), PLN.PLN_NUMERO_CREDITO);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_NUMERO_CUOTA), PLN.PLN_NUMERO_CUOTA);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_FECHA_PAGO), PLN.PLN_FECHA_PAGO);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_SALDO_INICIAL), PLN.PLN_SALDO_INICIAL);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_CAPITAL), PLN.PLN_CAPITAL);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_CAPITAL_POR_PAGAR), PLN.PLN_CAPITAL_POR_PAGAR);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_INTERESES), PLN.PLN_INTERESES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_INTERES_POR_PAGAR), PLN.PLN_INTERES_POR_PAGAR);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_SEGURO_VIDA), PLN.PLN_SEGURO_VIDA);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_SEGURO_HOGAR), PLN.PLN_SEGURO_HOGAR);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_A_PAGAR), PLN.PLN_A_PAGAR);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_PLAZO), PLN.PLN_PLAZO);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumPlanPagosProductoCliente.PLN_VALOR_DEUDA_REMANENTE), PLN.PLN_VALOR_DEUDA_REMANENTE);

                    await connection.QueryFirstAsync<bool>(CreacionPlanPagos, parameters, transaction, null, CommandType.StoredProcedure);
                }
                if (abrirConexion)
                {
                    await transaction.CommitAsync();
                }
                return true;
            }
            catch (Exception)
            {
                // Si la transacción fue abierta localmente, hacer rollback
                if (abrirConexion && transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                return false;
            }
            finally
            {
                // Cerrar la conexión solo si fue abierta en este método
                if (abrirConexion && connection != null)
                {
                    await connection.CloseAsync();
                }
            }
        }
        public async Task<bool> CrearCpgProductoClienteCartera(AplicacionPago aplicacionPago)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ID), aplicacionPago.CPG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CRE_ID), aplicacionPago.CRE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_FECHA_PAGO), aplicacionPago.CPG_FECHA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_FECHA_APLICACION), aplicacionPago.CPG_FECHA_APLICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_SEGURO_HOGAR), aplicacionPago.CPG_ABONO_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_SEGURO_VIDA), aplicacionPago.CPG_ABONO_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_INTERES_CORRIENTE), aplicacionPago.CPG_ABONO_INTERES_CORRIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_INTERES_MORA), aplicacionPago.CPG_ABONO_INTERES_MORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_CAPITAL), aplicacionPago.CPG_ABONO_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_VALOR_PAGO), aplicacionPago.CPG_VALOR_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.DCT_ID), aplicacionPago.DCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_CREADO_POR), aplicacionPago.CPG_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_FECHA_CREACION), aplicacionPago.CPG_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_MODIFICADO_POR), aplicacionPago.CPG_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_FECHA_MODIFICACION), aplicacionPago.CPG_FECHA_MODIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.TPP_ID), aplicacionPago.TPP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_CAPITAL_REMANENTE), aplicacionPago.CPG_ABONO_CAPITAL_REMANENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPagosProductoCliente.CPG_ABONO_INTERES_REMANENTE), aplicacionPago.CPG_ABONO_INTERES_REMANENTE);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        #endregion
        public async Task<IEnumerable<AplicacionListaModel>> ObtenerProcesoAplicacionPagosConDetalleSeven(int papId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@PAP_ID", papId);

            return await GetAsyncList<AplicacionListaModel>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<MovimientoComprobanteEntity> InsertarMovimientoComprobanteCartera(MovimientoComprobanteEntity movimientoComprobanteEntity)
        {
            var parametrosMovimientos = GenerarParametrosMovimientoComprobanteCartera(movimientoComprobanteEntity);
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
            HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
            param: parametrosMovimientos,
            transaction: transaction,
            commandType: CommandType.StoredProcedure
        );
                movimientoComprobanteEntity.MCC_ID = result.MCC_ID;
                await transaction.CommitAsync();
                return movimientoComprobanteEntity;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<ComprobanteCartera> ObtenerComprobanteCarteraByComprobanteId(int? ComprobanteId, int Transaction)
        {
            DynamicParameters parametros = new();

            parametros.Add("@COMID", ComprobanteId);
            parametros.Add("@TRANID", Transaction);

            return await GetAsyncFirst<ComprobanteCartera>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ComprobanteCarteraPorProcesoDTO>> ObtenerComprobanteCartera(ObtenerComprobantePorProcesoDto Request, int CodigoOperacion)
        {
            DynamicParameters parametros = new();

            parametros.Add("@FECHA_INICIAL", string.IsNullOrEmpty(Request.FechaInicio) ? null : Convert.ToDateTime(Request.FechaInicio).Date);
            parametros.Add("@FECHA_FINAL", string.IsNullOrEmpty(Request.FechaFinal) ? null : Convert.ToDateTime(Request.FechaFinal).Date);
            parametros.Add("@CODIGO_PPROCESO", Request.CodigoProceso);
            parametros.Add("@TIPO_OPERACION", Request.TipoOperacion);
            parametros.Add("@COMPROBANTE_ID", Request.ComprobanteId);
            parametros.Add("@DETALLE_MOVIMIENTO_ID", Request.DetalleMovimientoId);
            parametros.Add("@ESTADO", Request.Estado);
            parametros.Add("@CC_COMPROBANTE_SEVEN_ID", Request.ComprobanteSevenId);

            return await GetAsyncList<ComprobanteCarteraPorProcesoDTO>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ComprobanteCarteraPorProcesoDTO>> ObtenerComprobanteCarteraPorId(int IdComprobanteContable, string CodigoProceso)
        {
            DynamicParameters parametros = new();

            parametros.Add("@ID_COMPROBANTE", IdComprobanteContable);
            parametros.Add("@CODIGO_PROCESO", CodigoProceso);


            return await GetAsyncList<ComprobanteCarteraPorProcesoDTO>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AplicacionListaModel>> ConsultarListasPorFecha(DateTime fechaPago)
        {
            DynamicParameters parametros = new();

            parametros.Add("@FECHA_PAGO", fechaPago);

            return await GetAsyncList<AplicacionListaModel>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MovimientoComprobanteEntity>> ObtenerMovimientosComprobanteCartera(int Comprobante)
        {
            DynamicParameters parametros = new();

            parametros.Add("@CC_ID", Comprobante);

            return await GetAsyncList<MovimientoComprobanteEntity>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<MovimientoComprobantePorProcesoDto>> ObtenerMovimientosContableCartera(MovimientosContableParametersDto detalle)
        {
            DynamicParameters parametros = new();

            parametros.Add("@DETALLE_COMPROBANTE", detalle.IdComprobante);
            parametros.Add("@CANTIDAD_REGISTROS", detalle.RegistrosPorPagina);
            parametros.Add("@NUMERO_PAGINA", detalle.NumeroPagina);

            return await GetAsyncList<MovimientoComprobantePorProcesoDto>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }        
        public async Task<IEnumerable<ObtenerComprobanteTotalizadoPorCuentaDto>>ObtenerComprobanteTotalizadoPorCuenta(ObtenerComprobanteParametersDto detalle)
        {
            DynamicParameters parametros = new();

            parametros.Add("@DETALLE_COMPROBANTE", detalle.IdComprobante);

            return await GetAsyncList<ObtenerComprobanteTotalizadoPorCuentaDto>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        //public async Task<IEnumerable<ObtenerComprobanteTotalizadoPorCuentaDto>> ObtenerComprobanteTotalizadoPorCuenta(ObtenerComprobanteParametersDto detalle)
        //{
        //    DynamicParameters parametros = new();

        //    parametros.Add("@DETALLE_COMPROBANTE", detalle.IdComprobante);

        //    return await GetAsyncList<ObtenerComprobanteTotalizadoPorCuentaDto>(HelperDBParameters.BuilderFunction(
        //       HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        //}
        //public async Task<IEnumerable<ObtenerComprobanteTotalizadoPorCuentaDto>> ConsultarBalancePorFecha(ObtenerComprobanteParametersDto detalle)
        //{
        //    DynamicParameters parametros = new();

        //    parametros.Add("FechaFinal", detalle.IdComprobante);

        //    return await GetAsyncList<ObtenerComprobanteTotalizadoPorCuentaDto>(HelperDBParameters.BuilderFunction(
        //       HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        //}

        public async Task<IEnumerable<BalanceCarteraPorPeriodo>> ConsultarBalancePorFecha(ObtenerComprobantePorProcesoDto Request)
        {
            DynamicParameters parametros = new();

            parametros.Add("@FECHA_FINAL", string.IsNullOrEmpty(Request.FechaFinal) ? null : Convert.ToDateTime(Request.FechaFinal).Date);


            return await GetAsyncList<BalanceCarteraPorPeriodo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<RequestDetallePago>> ObtenerDetallesPagosSevenCartera(int comprobanteId)
        {
            DynamicParameters parametros = new();

            parametros.Add("@CC_ID", comprobanteId);

            return await GetAsyncList<RequestDetallePago>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerGarantiaActualizada>> ObtenerCreditosActualizanGarantia(requestGarantiasDto requestGarantias)
        {
            DynamicParameters parametros = new();

            parametros.Add("@ANIO", requestGarantias.Anio);
            parametros.Add("@MES", requestGarantias.Mes);
            //parametros.Add("@TCR_ID", requestGarantias.TipoCredito);

            return await GetAsyncList<ObtenerGarantiaActualizada>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ObtenerGarantiaActualizada> ObtenerUltimosAjustesGarantiaPorCedula(string cliIdentificacion)
        {
            DynamicParameters parametros = new();
            parametros.Add("@CLI_IDENTIFICACION", cliIdentificacion);

            var lista = await GetAsyncList<ObtenerGarantiaActualizada>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parametros,
                CommandType.StoredProcedure
            );

            return lista.FirstOrDefault();
        }




        private DynamicParameters GenerarParametrosComprobanteCartera(ComprobanteCartera comprobanteCartera)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.CC_FECHA_COMPROBANTE), comprobanteCartera.CC_FECHA_COMPROBANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.TCC_TIPO_COMPROBANTE_ID), comprobanteCartera.TCC_TIPO_COMPROBANTE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.DPD_DEPARTAMENTO_ID), comprobanteCartera.DPD_DEPARTAMENTO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.DPC_CIUDAD_ID), comprobanteCartera.DPC_CIUDAD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.TCC_TIPO_CONCEPTO_ID), comprobanteCartera.TCC_TIPO_CONCEPTO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.TBC_TIPO_BENEFICIARIO_ID), comprobanteCartera.TBC_TIPO_BENEFICIARIO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.TIPO_IDENTIFICACION), comprobanteCartera.TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.CLI_IDENTIFICACION), comprobanteCartera.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.TOTAL_COMPROMBANTE_CREDITO), comprobanteCartera.TOTAL_COMPROMBANTE_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.TOTAL_COMPROMBANTE_DEBITO), comprobanteCartera.TOTAL_COMPROMBANTE_DEBITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobanteCartera.CC_APROBADO), comprobanteCartera.CC_APROBADO);

            return parametros;
        }

        private DynamicParameters GenerarParametrosMovimientoComprobanteCartera(MovimientoComprobanteEntity item)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_ID), item.MCC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.CC_ID), item.CC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_CUENTA_CONTABLE), item.MCC_CUENTA_CONTABLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_DESCRIPCION_CUENTA), item.MCC_DESCRIPCION_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_CENTRO_COSTO), item.MCC_CENTRO_COSTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_DETALLE), item.MCC_DETALLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_TIPO), item.MCC_TIPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.MCC_MONTO), item.MCC_MONTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientoComprobanteCartera.TRANID), item.MCC_ID == 0 ? 1 : 2);
            return parametros;
        }

        public async Task<IEnumerable<CierreMnesualSevenModel>> ObtenerDesembolsoSevenCierreMensual(DateTime fechaCierre)
        {
            DynamicParameters parametros = new();

            parametros.Add("@FECHADESEMBOLSO", fechaCierre);

            return await GetAsyncList<CierreMnesualSevenModel>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<CierreMnesualSevenModel>> ObtenerCancelacionTotalCierreMensual(DateTime fechaCierreInicial, DateTime fechaCierreFinal)
        {
            DynamicParameters parametros = new();

            parametros.Add("@FECHADESINICIAL", fechaCierreInicial);
            parametros.Add("@FECHADESFINAL", fechaCierreFinal);

            return await GetAsyncList<CierreMnesualSevenModel>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> InsertarComprobantePagosSeven(RequestDetallePago request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.EMP_CODI), request.Emp_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.TOP_CODI), request.Top_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.MCO_FECH), request.Mco_fech);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.MOD_CODI), request.Mod_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.MCO_DESC), request.Mco_desc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.MOV_CONTABLE_SEVEN), request.Mov_Contable_Seven);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.RESPUESTA_SEVEN), request.Respuesta_Seven);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumComprobantePagoSeven.FECHA_EJECUCION), request.Fecha_Ejecucion);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> InsertarMovimientoComprobante(MovimientoComprobanteEntity request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.CC_ID), request.CC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.MCC_CUENTA_CONTABLE), request.MCC_CUENTA_CONTABLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.MCC_DESCRIPCION_CUENTA), request.MCC_DESCRIPCION_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.MCC_CENTRO_COSTO), request.MCC_CENTRO_COSTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.MCC_DETALLE), request.MCC_DETALLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.MCC_TIPO), request.MCC_TIPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumMovimientosCarteraComprobante.MCC_MONTO), request.MCC_MONTO);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }



        public async Task<ComprobanteCartera> CrearComprobanteCartera(ComprobanteCartera comprobanteCartera)
        {
            var parametrosProceso = GenerarParametrosComprobanteCartera(comprobanteCartera);
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<ComprobanteCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                    param: parametrosProceso,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                comprobanteCartera.CC_ID = result.CC_ID;
                comprobanteCartera.ESTADO_ID = result.ESTADO_ID;

                foreach (var item in comprobanteCartera.MOVCOMPROBANTE)
                {
                    item.CC_ID = result.CC_ID;
                    var parametrosMovimientos = GenerarParametrosMovimientoComprobanteCartera(item);
                    var detalleResult = await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarMovimientoComprobanteCartera"),
                        param: parametrosMovimientos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                    item.MCC_ID = detalleResult.MCC_ID;
                }

                await transaction.CommitAsync();
                return comprobanteCartera;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<int> CrearNovedadRediferido(RequestNovedadRediferido request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NRC_TIPO_CREDITO",  request.TipoCredito);
            parametros.Add("@NRC_TIPO_DOCUMENTO", request.TipoIdentificacion);
            parametros.Add("@NRC_NUMERO_DOCUMENTO", request.NumeroDocumentoCliente);
            parametros.Add("@NRC_CREDITO_ID", request.CreditoId);
            parametros.Add("@NRC_NUMERO_CREDITO", request.NumeroCredito);
            parametros.Add("@NRC_FECHA_CIERRE", request.FechaCierre);
            parametros.Add("@NRC_PLAZO_CUOTAS", request.CuotaCredito);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ResponseNovedadRediferido>> ObtenerNovedadRediferido(Guid creditoId)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NRC_CREDITO_ID", creditoId);

            return await GetAsyncList<ResponseNovedadRediferido>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> UpdateNuevaTasaEACredito(InsertPlanPagosProductoCartera request,int causal,int tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@PLN_NUMERO_CREDITO", request.PLN_NUMERO_CREDITO);
            parametros.Add("@PLN_CAPITAL", request.PLN_CAPITAL);
            parametros.Add("@PLN_INTERESES", request.PLN_INTERESES);
            parametros.Add("@PLN_SEGURO_VIDA", request.PLN_SEGURO_VIDA);
            parametros.Add("@PLN_NUMERO_CUOTA", request.PLN_NUMERO_CUOTA);
            parametros.Add("@PLN_A_PAGAR", request.PLN_A_PAGAR);
            parametros.Add("@PLN_SALDO_INICIAL", request.PLN_SALDO_INICIAL);
            parametros.Add("@PLN_SEGURO_HOGAR", request.PLN_SEGURO_HOGAR);
            parametros.Add("@ID_CAUSAL", causal);
            parametros.Add("@TIPO_CREDITO", tipoCredito);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> ActualizarPorcentajeSeguro(float seguroVida,float seguroHogar, long nroCredito, int plazo,int plazoPendiente)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@PLN_NUMERO_CREDITO", nroCredito);
            parametros.Add("@TASA_HOGAR", seguroHogar);
            parametros.Add("@TASA_VIDA", seguroVida);
            parametros.Add("@PLAZO", plazo);
            parametros.Add("@PLAZO_PENDIENTE", plazoPendiente);


            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> ActualizarTasaEACredito(Guid creditoId,decimal tasaEA,double cuota,decimal tasa, int causal)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@ID_CREDITO", creditoId);
            parametros.Add("@NUEVA_TASA_EA", tasaEA);
            parametros.Add("@CUOTA", cuota);
            parametros.Add("@TASAMIXTA", tasa);
            parametros.Add("@ID_CAUSAL", causal);


            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> InsertarAbonoHistorico(AbonoHistorico dto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@AH_NUMERO_CREDITO", dto.AH_NUMERO_CREDITO);
            parameters.Add("@AH_TIPO_CREDITO", dto.AH_TIPO_CREDITO);
            parameters.Add("@AH_CAPITAL", dto.AH_CAPITAL);
            parameters.Add("@AH_INTERES", dto.AH_INTERES);
            parameters.Add("@AH_SEGURO_VIDA", dto.AH_SEGURO_VIDA);
            parameters.Add("@AH_SEGURO_HOGAR", dto.AH_SEGURO_HOGAR);
            parameters.Add("@AH_NUMERO_CUOTA", dto.AH_NUMERO_CUOTA);
            parameters.Add("@AH_VALOR_PAGO", dto.AH_VALOR_PAGO);
            parameters.Add("@AH_SALDO", dto.AH_SALDO);
            parameters.Add("@FECHA_APLICACION", dto.AH_FECHA_CREACION);
            parameters.Add("@CREADO_POR", dto.CreadoPor);
            parameters.Add("@TIPORECURSO", dto.TipoRecurso);

            var result = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return result;
        }
        public async Task<List<AplicacionPago>> ObtenerPagoAbonoHistorico(string numeroCredito,int typeCredit)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@NRO_CREDITO", numeroCredito);
            parameters.Add("@CRE_TCR_ID", typeCredit);

            var result = await GetAsyncList<AplicacionPago>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<bool> ActualizarNumeroCuota(AbonoHistorico dto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@AH_NUMERO_CREDITO", dto.AH_NUMERO_CREDITO);
            parameters.Add("@AH_TIPO_CREDITO", dto.AH_TIPO_CREDITO);
            parameters.Add("@NRO_CUOTA", dto.AH_NUMERO_CUOTA);

            var result = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return result;
        }
        public async Task<IEnumerable<AbonoHistorico>> ObtenerAbonoHistorico(string numeroCredito, int tipoCredito)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@AH_NUMERO_CREDITO", numeroCredito);
            parameters.Add("@AH_TIPO_CREDITO", tipoCredito);

            return await GetAsyncList<AbonoHistorico>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<int> ActualizarPlanPagoMasivos(DataTable dt, int tipoCredito)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@DATA", dt.AsTableValuedParameter("dbo.TVP_PlanPagoUpdate"));
            parameters.Add("@TIPO_CREDITO", tipoCredito);

            return await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        #region Abonos_Capital
        public async Task<AplicacionPago> AplicaPagoReducirCuota(RequestPagoAbonoCapital requestPagoAbonoCapital, InformacionCredito infoCredito, PagoAplicarDto aplicacionPago)
        {
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                Guid CPGID = Guid.NewGuid();
                var paramInsertarHistoricoCreditoPago = InsertarHistoricoCreditoPago(infoCredito, CPGID);
                await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoCreditoPago"),
                    param: paramInsertarHistoricoCreditoPago,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                if (infoCredito.TCR_ID == 1)
                {
                    var paramInsertarHistoricoPlanPagos = InsertarHistoricoPlanPagos(infoCredito, CPGID);
                    await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoPlanPagosLeasing"),
                        param: paramInsertarHistoricoPlanPagos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                }
                else
                {
                    var paramInsertarHistoricoPlanPagos = InsertarHistoricoPlanPagos(infoCredito, CPGID);
                    await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoPlanPagosHipotecario"),
                        param: paramInsertarHistoricoPlanPagos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                }
                var paramActualizarCreCreditoEliminarPlanPagos = ActualizarCreCreditoEliminarPlanPagos(infoCredito, requestPagoAbonoCapital, aplicacionPago);
                await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "UpdCreCredito_DelPln"),
                    param: paramActualizarCreCreditoEliminarPlanPagos,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);

                await CrearPlanPagosProductoClienteCartera(requestPagoAbonoCapital.LstPln, infoCredito.TCR_ID, connection, transaction);

                var paramAplicaPagoReducirCuotaP = AplicaPagoReducirCuotaP(aplicacionPago, CPGID, infoCredito.CRE_ID);
                var result = await connection.QueryFirstOrDefaultAsync<AplicacionPago>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                    param: paramAplicaPagoReducirCuotaP,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<AplicacionPago> AplicaPagoReducirPlazo(RequestPagoAbonoCapital requestPagoAbonoCapital, InformacionCredito infoCredito, PagoAplicarDto aplicacionPago)
        {
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                Guid CPGID = Guid.NewGuid();
                var paramInsertarHistoricoCreditoPago = InsertarHistoricoCreditoPago(infoCredito, CPGID);
                await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoCreditoPago"),
                    param: paramInsertarHistoricoCreditoPago,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                if (infoCredito.TCR_ID == 1)
                {
                    var paramInsertarHistoricoPlanPagos = InsertarHistoricoPlanPagos(infoCredito, CPGID);
                    await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoPlanPagosLeasing"),
                        param: paramInsertarHistoricoPlanPagos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                }
                else
                {
                    var paramInsertarHistoricoPlanPagos = InsertarHistoricoPlanPagos(infoCredito, CPGID);
                    await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoPlanPagosHipotecario"),
                        param: paramInsertarHistoricoPlanPagos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                }
                var paramActualizarCreCreditoEliminarPlanPagos = ActualizarCreCreditoEliminarPlanPagos(infoCredito, requestPagoAbonoCapital, aplicacionPago);
                await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "UpdCreCredito_DelPln"),
                    param: paramActualizarCreCreditoEliminarPlanPagos,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);

                await CrearPlanPagosProductoClienteCartera(requestPagoAbonoCapital.LstPln, infoCredito.TCR_ID, connection, transaction);

                var paramAplicaPagoReducirCuotaP = AplicaPagoReducirCuotaP(aplicacionPago, CPGID, infoCredito.CRE_ID);
                var result = await connection.QueryFirstOrDefaultAsync<AplicacionPago>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                    param: paramAplicaPagoReducirCuotaP,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<AplicacionPago> AplicaPagoRediferidoPlazo(RequestPagoAbonoCapital requestPagoAbonoCapital, InformacionCredito infoCredito, PagoAplicarDto aplicacionPago)
        {
            using var connection = await OpenConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                Guid CPGID = Guid.NewGuid();
                var paramInsertarHistoricoCreditoPago = InsertarHistoricoCreditoPago(infoCredito, CPGID);
                await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoCreditoPago"),
                    param: paramInsertarHistoricoCreditoPago,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                if (infoCredito.TCR_ID == 1)
                {
                    var paramInsertarHistoricoPlanPagos = InsertarHistoricoPlanPagos(infoCredito, CPGID);
                    await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoPlanPagosLeasing"),
                        param: paramInsertarHistoricoPlanPagos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                }
                else
                {
                    var paramInsertarHistoricoPlanPagos = InsertarHistoricoPlanPagos(infoCredito, CPGID);
                    await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                        HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoPlanPagosHipotecario"),
                        param: paramInsertarHistoricoPlanPagos,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);
                }
                var paramActualizarCreCreditoEliminarPlanPagos = ActualizarCreCreditoEliminarPlanPagos(infoCredito, requestPagoAbonoCapital, aplicacionPago);
                await connection.QueryFirstOrDefaultAsync<MovimientoComprobanteEntity>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "UpdCreCredito_DelPln"),
                    param: paramActualizarCreCreditoEliminarPlanPagos,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);

                await CrearPlanPagosProductoClienteCartera(requestPagoAbonoCapital.LstPln, infoCredito.TCR_ID, connection, transaction);

                var paramAplicaPagoReducirCuotaP = AplicaPagoRediferidoCuota(aplicacionPago, CPGID, infoCredito.CRE_ID);
                var result = await connection.QueryFirstOrDefaultAsync<AplicacionPago>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                    param: paramAplicaPagoReducirCuotaP,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private DynamicParameters InsertarHistoricoCreditoPago(InformacionCredito infoCredito, Guid CPGID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarHistoricoCreditoPago.CRE_ID), infoCredito.CRE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarHistoricoCreditoPago.CPG_ID), CPGID);
            return parametros;
        }
        private DynamicParameters InsertarHistoricoPlanPagos(InformacionCredito infoCredito, Guid CPGID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarHistoricoPlanPagos.NUMERO_CREDITO), infoCredito.CRE_NRO_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarHistoricoPlanPagos.CPG_ID), CPGID);
            return parametros;
        }
        private DynamicParameters ActualizarCreCreditoEliminarPlanPagos(InformacionCredito infoCredito, RequestPagoAbonoCapital requestPagoAbonoCapital, PagoAplicarDto aplicacionPago)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.NUEVOSALDOCAPITAL), requestPagoAbonoCapital.NuevoSaldoReducido);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.NUEVOVALORCUOTAREDUCIDO), requestPagoAbonoCapital.NuevaCuotaReducida);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.NUEVOSALDOTRADICIONAL), requestPagoAbonoCapital.NuevoSaldoTradicional);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.NUEVOVALORCUOTATRADICIONAL), requestPagoAbonoCapital.NuevaCuotaTradicional);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.CREID), infoCredito.CRE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.TCRID), infoCredito.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.NROCREDITO), infoCredito.CRE_NRO_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.AAP_CODIGO), aplicacionPago.Codigo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.CRE_PLAZO_TOTAL), requestPagoAbonoCapital.PlazoTotal);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.CRE_PLAZO_ACTUAL), requestPagoAbonoCapital.PlazoActual);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.CRE_CANON_EXTRAORDINARIO), requestPagoAbonoCapital.PagoExtraOrdinario);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.CRE_FECHA_FINALIZA_PAGOS), requestPagoAbonoCapital.FechaFinalizaPagos);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizarCreCreditoEliminarPlanPagos.MODIFICADOPOR), aplicacionPago.CreadoPor);
            return parametros;
        }
        private DynamicParameters AplicaPagoReducirCuotaP(PagoAplicarDto aplicacionPago, Guid CPGID, Guid CreID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION_PAGO), aplicacionPago.Fecha_Pago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_CUOTA_PAGADA), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.IdAplicacionPago), CPGID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), aplicacionPago.CreadoPor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.AAP_ID), aplicacionPago.Codigo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR_PAGO), aplicacionPago.ValorPago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.IdCredito), CreID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.APLICA_SIMULACION), aplicacionPago.AplicaSimulacion);
            return parameters;
        }
        private DynamicParameters AplicaPagoRediferidoCuota(PagoAplicarDto aplicacionPago, Guid CPGID, Guid CreID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_APLICACION_PAGO), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.FECHA_CUOTA_PAGADA), aplicacionPago.Fecha_Pago_Cuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.IdAplicacionPago), CPGID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.CREADO_POR), aplicacionPago.CreadoPor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.AAP_ID), aplicacionPago.Codigo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.VALOR_PAGO), aplicacionPago.ValorPago);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.IdCredito), CreID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAplicarPago.APLICA_SIMULACION), aplicacionPago.AplicaSimulacion);
            return parameters;
        }


        #endregion

        // ================================================================================
        // MÉTODOS DEL REPOSITORY PARA REESTRUCTURACIÓN
        // ================================================================================

        /// <summary>
        /// Obtiene el valor de un parámetro del sistema
        /// </summary>
        public async Task<object> ObtenerParametro(string nombreParametro)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Nombre", nombreParametro);

            var resultado = await GetAsyncFirst<object>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerParametro"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Valida si el crédito tuvo mora excesiva en el período especificado
        /// </summary>
        public async Task<bool> ValidarMoraEnPeriodo(long numeroCredito, DateTime fechaInicio, DateTime fechaFin, int diasMoraMaxima)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);
            var diasAtras = (int)Math.Ceiling((fechaFin - fechaInicio).TotalDays);
            parametros.Add("@DiasAtras", diasAtras);
            parametros.Add("@DiasMoraMaxima", diasMoraMaxima);

            var resultado = await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ValidarMoraEnPeriodo"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Cuenta el número de reestructuraciones realizadas en el período especificado
        /// </summary>
        public async Task<int> ContarReestructuraciones(long numeroCredito, DateTime fechaInicio, DateTime fechaFin)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);
            var diasAtras = (int)Math.Ceiling((fechaFin - fechaInicio).TotalDays);
            parametros.Add("@DiasAtras", diasAtras);

            var resultado = await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ContarReestructuraciones"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Obtiene el total de intereses corrientes pendientes de pago
        /// </summary>
        public async Task<decimal> ObtenerInteresesCorrientesPendientes(long numeroCredito, int tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);

            var resultado = await GetAsyncFirst<decimal>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerInteresesCorrientesPendientes"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Calcula el total de intereses de mora del crédito
        /// </summary>
        public async Task<decimal> ObtenerInteresesMora(long numeroCredito, int tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);

            var resultado = await GetAsyncFirst<decimal>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerInteresesMora"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Obtiene la tasa vigente para reestructuración según el tipo de crédito
        /// </summary>
        public async Task<double> ObtenerTasaVigenteReestructuracion(int tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@TipoCredito", tipoCredito);

            var resultado = await GetAsyncFirst<double>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerTasaVigenteReestructuracion"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Registra una transacción contable (PAGO o CARGO de reestructuración)
        /// </summary>
        public async Task RegistrarTransaccion(TransaccionContable transaccion)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", transaccion.NumeroCredito);
            parametros.Add("@TipoTransaccion", transaccion.TipoTransaccion);
            parametros.Add("@Monto", transaccion.Monto);
            parametros.Add("@Fecha", transaccion.Fecha);
            parametros.Add("@Descripcion", transaccion.Descripcion);
            parametros.Add("@CreadoPor", transaccion.CreadoPor);

            await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "RegistrarTransaccion"),
                parametros,
                CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Asigna un bloqueo al crédito (código 24 para reestructuración)
        /// </summary>
        public async Task AsignarBloqueoCredito(long numeroCredito, int codigoBloqueo, string concepto, DateTime fechaAsignacion)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);
            parametros.Add("@CodigoBloqueo", codigoBloqueo);
            parametros.Add("@Concepto", concepto);
            parametros.Add("@CreadoPor", "SYSTEM");

            await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "AsignarBloqueoCredito"),
                parametros,
                CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Inserta un registro en el histórico de reestructuraciones
        /// </summary>
        public async Task InsertarHistoricoReestructuracion(HistoricoReestructuracion historico)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", historico.NumeroCredito);
            parametros.Add("@TipoCredito", historico.TipoCredito);
            parametros.Add("@FechaReestructuracion", historico.FechaReestructuracion);
            parametros.Add("@FechaSolicitud", historico.FechaSolicitud);
            parametros.Add("@PlazoAnterior", historico.PlazoAnterior);
            parametros.Add("@PlazoNuevo", historico.PlazoNuevo);
            parametros.Add("@TasaAnterior", historico.TasaAnterior);
            parametros.Add("@TasaNueva", historico.TasaNueva);
            parametros.Add("@ValorPagoRealizado", historico.ValorPagoRealizado);
            parametros.Add("@ValorPagoMinimo", historico.ValorPagoMinimo);
            parametros.Add("@InteresesCorrientes", historico.InteresesCorrientes);
            parametros.Add("@InteresesMora", historico.InteresesMora);
            parametros.Add("@SaldoCapitalAnterior", historico.SaldoCapitalAnterior);
            parametros.Add("@DiasMora", historico.DiasMoraAlReestructurar);
            parametros.Add("@CuotaAnterior", historico.CuotaAnterior);
            parametros.Add("@CuotaNueva", historico.CuotaNueva);
            parametros.Add("@Observaciones", historico.Observaciones);
            parametros.Add("@CreadoPor", historico.CreadoPor);

            await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "InsertarHistoricoReestructuracion"),
                parametros,
                CommandType.StoredProcedure
            );
        }

        

        /// <summary>
        /// Obtiene los días de mora actual del crédito
        /// </summary>
        public async Task<int> ObtenerDiasMoraActual(long numeroCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);

            var resultado = await GetAsyncFirst<int>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerDiasMoraActual"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }

        /// <summary>
        /// Obtiene los datos completos del crédito para reestructuración
        /// </summary>
        public async Task<DatosCreditoReestructuracion> ObtenerDatosCreditoReestructuracion(long numeroCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@NumeroCredito", numeroCredito);

            var resultado = await GetAsyncFirst<DatosCreditoReestructuracion>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerDatosCreditoReestructuracion"),
                parametros,
                CommandType.StoredProcedure
            );

            return resultado;
        }
        /// <summary>
        /// Registra un pago con información de mora al momento del pago
        /// Llamar cuando el cliente realiza un pago
        /// </summary>
        public async Task<bool> RegistrarPagoEnMora(
            Guid creId,
            DateTime fechaPago,
            DateTime fechaAplicacion,
            decimal monto,
            int diasMoraAlPago,
            decimal causacionMoraAlPago)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CRE_ID", creId);
                parameters.Add("@PAG_FECHA_PAGO", fechaPago);
                parameters.Add("@PAG_FECHA_APLICACION", fechaAplicacion);
                parameters.Add("@PAG_MONTO", monto);
                parameters.Add("@PAG_DIAS_MORA_AL_PAGO", diasMoraAlPago);
                parameters.Add("@PAG_CAUSACION_MORA_AL_PAGO", causacionMoraAlPago);

                var result = await GetAsyncList<bool>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "RegistrarPagoEnMora"),
                    parameters,
                    CommandType.StoredProcedure);

                return result.Any();
            }
            catch (Exception ex)
            {
                // Log error
                throw;
            }
        }

        // ═════════════════════════════════════════════════════════════════════════════════
        // 2️⃣ OBTENER PAGO DEL DÍA (para validar en causación)
        // ═════════════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Obtiene el pago registrado para un crédito en una fecha específica
        /// Llamar en CierreDiarioCartera para verificar si pagó HOY
        /// </summary>
        public async Task<PagoPagoMora> ObtenerPagoDeDia(Guid creId, DateTime fechaAplicacion)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CRE_ID", creId);
                parameters.Add("@FECHA_APLICACION", fechaAplicacion);

                var result = await GetAsyncList<PagoPagoMora>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerPagoDeDia"),
                    parameters,
                    CommandType.StoredProcedure);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log error
                return null;
            }
        }

        // ═════════════════════════════════════════════════════════════════════════════════
        // 3️⃣ OBTENER PAGOS POR RANGO DE FECHAS
        // ═════════════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Obtiene todos los pagos de un crédito en un rango de fechas
        /// Útil para auditoría y reporte
        /// </summary>
        public async Task<IEnumerable<PagoPagoMora>> ObtenerPagosPorRango(
            Guid creId,
            DateTime fechaInicial,
            DateTime fechaFinal)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CRE_ID", creId);
                parameters.Add("@FECHA_INICIAL", fechaInicial);
                parameters.Add("@FECHA_FINAL", fechaFinal);

                var result = await GetAsyncList<PagoPagoMora>(
                    HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ObtenerPagosPorRango"),
                    parameters,
                    CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                // Log error
                return null;
            }
        }
        public async Task<bool> ActualizarComprobanteCarteraEliminarAfiliado(int ccId,
            decimal totalCredito,
            decimal totalDebito,
            string movContable)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@CC_ID", ccId);
            parametros.Add("@TOTAL_COMPROMBANTE_CREDITO", totalCredito);
            parametros.Add("@TOTAL_COMPROMBANTE_DEBITO", totalDebito);
            parametros.Add("@MOV_CONTABLE_SEVEN", movContable);

            var result = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> ActualizarMoraDelCredito(
            Guid creId,
            decimal nuevoAcumuladoInteresMora,
            int nuevosDiasMora)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CRE_ID", creId);
            parameters.Add("@NUEVO_ACUMULADO_INTERES_MORA", nuevoAcumuladoInteresMora);
            parameters.Add("@NUEVOS_DIAS_MORA", nuevosDiasMora);

            var result = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return result;
        }

        public async Task ActualizarTipoRecurso(Guid idAplicacionPago, string tipoRecurso)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CPG_ID", idAplicacionPago);
            parameters.Add("@TipoRecurso", tipoRecurso);

            await ExecuteAsync(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }
    }
}
