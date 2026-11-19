using Aspose.Cells.Revisions;
using AutoMapper;
using ClosedXML.Excel;
using CsvHelper;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera;
using GA2.Application.Dto.Cartera.Cierres;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Application.Dto.Cartera.Cierres.CierreAnual;
using GA2.Application.Dto.Cartera.Cierres.CierresCausacionesRealizadas;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCarteraPorPeriodoDto;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Application.Dto.Reportes;
using GA2.Application.Main;
using GA2.Domain.Core.BuaServiceSvc;
using GA2.Domain.Core.Cartera;
using GA2.Domain.Core.Connected_Services.ServiceSeven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Domain.Core.Servicios.Bua;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual.Cartera.Entities;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Entities.Cartera.CreditoCarteraProducto;
using GA2.Domain.Entities.Cartera.Novedades;
using GA2.Domain.Entities.Cartera.Pagos;
using GA2.Domain.Entities.Cartera.ProductoPorCliente;
using GA2.Domain.Entities.Cartera.Reportes;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Credito.Simulador;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.Prospeccion;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Escrituras;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Extensions;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using ServiceSeven;
using SharpCompress;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GA2.Domain.Core
{
    public class CarteraBusinessLogic : ICarteraBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ICarteraRepository _carteraRepository;
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly IReportesRequestProvider _iReportesRequestProvider;
        private readonly IBuaService _buaService;
        private readonly IReportesRepository _reportesRepository;
        private readonly IConsumoSCnMcont _wsMovContable;
        private readonly IReportesExcelInvariantCultureLogic _reportesExcel;
        private readonly ICreditoRepository _creditoRepository;
        private readonly IEscriturasRepository _escrituraRepository;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly System.Lazy<IReportesBusinessLogic> _reportesBusinessLogic;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly LocalBlogStorageConfig _config;
        private readonly System.Lazy<IProspeccionBusinessLogic> _prospeccionBusinessLogic;
        private readonly IMovimientoContableService _IMovimientoContableService;
        public CarteraBusinessLogic(IMapper mapper,
            ICarteraRepository carteraRepository,
            IConfiguration configuration,
            ICreditoBusinessLogic creditoBusinessLogic,
            IBlobStorageGenericRepository blobStorageGenericRepository,
            ICatalogosBusinessLogic catalogosBusinessLogic,
            IReportesRequestProvider iReportesRequestProvider,
            IClientRequestProvider iClientRequestProvider,
            IBuaService buaService,
            IReportesRepository reportesRepository,
            IConsumoSCnMcont wsMovContable,
            IReportesExcelInvariantCultureLogic reportesExcel,
            ICreditoRepository creditoRepository,
            IEscriturasRepository escriturasRepository,
            ICatalogosRepository catalogsRepository,
             IWebHostEnvironment webHostEnvironment,
            IOptions<BlobStorageOptions> options,
            IOptions<LocalBlogStorageConfig> config,
            IMovimientoContableService IMovimientoContableService,
            System.Lazy<IReportesBusinessLogic> reportesBusinessLogic,
            System.Lazy<IProspeccionBusinessLogic> prospeccionBusinessLogic
            )
        {
            _mapper = mapper;
            _carteraRepository = carteraRepository;
            _creditoBusinessLogic = creditoBusinessLogic;
            _catalogosBusinessLogic = catalogosBusinessLogic;
            _iClientRequestProvider = iClientRequestProvider;
            _iReportesRequestProvider = iReportesRequestProvider ?? throw new ArgumentNullException(nameof(iReportesRequestProvider));
            _buaService = buaService;
            _reportesRepository = reportesRepository;
            _wsMovContable = wsMovContable ?? throw new ArgumentNullException(nameof(wsMovContable));
            _reportesExcel = reportesExcel;
            _creditoRepository = creditoRepository;
            _escrituraRepository = escriturasRepository;
            _catalogsRepository = catalogsRepository;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _config = config.Value;
            _IMovimientoContableService = IMovimientoContableService;
            _reportesBusinessLogic = reportesBusinessLogic;
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }


        /// <summary>
        /// Actualiza Simulacion
        /// </summary>
        /// <param name="tasas"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<TasaSimuladorDto>> ActualizarTasasSimulacion(TasaSimuladorDto tasas)
        {
            var tasaUpdate = this._mapper.Map<TasaSimulador>(tasas);
            var tasa = tasas.TasaEA / 100;
            var tasaUsuara = (decimal)Math.Pow(1 + (double)tasa, 1.0 / 12.0) - 1;
            tasaUpdate.SIM_TASA_NOMINAL_MENSUAL = tasaUsuara * 100;
            var response = this._mapper.Map<TasaSimuladorDto>(await this._carteraRepository.ActualizarTasasSimulacion(tasaUpdate));
            return new Response<TasaSimuladorDto> { Data = response };
        }

        public async Task<Response<Guid>> CrearRestitucionInmueble(RestitucionInmuebleCarteraDto request)
        {
            try
            {
                var requestMap = this._mapper.Map<RestitucionInmuebleCartera>(request);
                var response = await this._carteraRepository.CreateRestitucionInmuebleCartera(requestMap);
                if (response > 0)
                {
                    //var updateStatus = await this._carteraRepository.ActualizarEstadoCreditoRestituido(
                    //    request.NumeroDocumento, request.TipoDpcumento, request.IdCredito);
                    var updateStatu = await _carteraRepository.AplicarPagoCancelacionRestitucion(new PagoAplicarDto
                    {
                        NumeroCredito = (long)request.NumeroCredito,
                        ValorPago = request.ValorPagado,
                        Fecha_Pago = request.FechaFinalizacion,
                        Fecha_Pago_Cuota = request.FechaFinalizacion,
                        CreadoPor = request.CreadoPor,
                        AplicaSimulacion = false,
                        Tcr_id = (int)request.IdTipoCredito
                    });

                    return new Response<Guid> { Data = updateStatu, IsSuccess = true };
                }
                return new Response<Guid> { Data = new Guid(), IsSuccess = false };
            }
            catch (Exception E)
            {
                return new Response<Guid> { Data = new Guid(), IsSuccess = false };
            }

        }
        public async Task<Response<IEnumerable<RestitucionInmuebleCarteraDto>>> ObtenerRestitucionInmuebleCartera(Guid idCredito)
        {
            var response = await this._carteraRepository.ObtenerRestitucionInmuebleCartera(idCredito);
            if (response.Count() > 0)
            {
                var responseMap = _mapper.Map<IEnumerable<RestitucionInmuebleCarteraDto>>(response);
                return new Response<IEnumerable<RestitucionInmuebleCarteraDto>> { Data = responseMap, IsSuccess = true };
            }
            return new Response<IEnumerable<RestitucionInmuebleCarteraDto>> { Data = null, IsSuccess = false };

        }

        /* ============================================================================
   NUEVO MÉTODO: AplicarValorComoOtrosPagos
 
   Es la misma jerarquía de pago que ya usa la rama "OTROS_PAGOS"
   (Seguro Vida -> Seguro Hogar -> Interés Mora -> Interés Corriente ->
   Capital Mora -> Capital), pero:
     1) parametrizada para recibir el valor a aplicar (valorAAplicar) en vez
        de usar siempre request.ValorPago completo -- así se puede invocar
        con solo la parte de la mora, dejando el resto para la disminución.
     2) SIN el "if (allPayments.Any())" que tenía OTROS_PAGOS -- ese guard
        hacía que la lógica solo corriera si ya existía otro pago aplicado
        ese mismo día/mes. Para este caso (primer pago del mes que además
        cubre mora) allPayments normalmente viene vacío, y con el guard
        original la mora NUNCA se habría aplicado. Aquí simplemente se
        toma como 0 lo que ya se pagó este mes cuando no hay otros pagos
        (el Sum() de una lista vacía ya da 0 de forma natural).
 
   Revisa este supuesto -- si OTROS_PAGOS pone ese guard a propósito por
   alguna otra razón de negocio que yo no conozca, dímelo y lo ajustamos.
============================================================================ */
        private async Task<AplicacionPagoDto> AplicarValorComoOtrosPagos(PagoAplicarDto request, InformacionCredito infoCredito, decimal valorAAplicar)
        {
            var aplicationPayment = await _carteraRepository.ObtenerAplicacionPago(infoCredito.CRE_ID);
            var allPayments = aplicationPayment.Where(x =>
                x.CPG_FECHA_APLICACION.Date == request.Fecha_Pago_Cuota.Date
                && x.CPG_VALOR_PAGO > 0
                && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2)).ToList();

            var requetPlanPayment = new ObtenerProductoCartera
            {
                Nro_Credito = infoCredito.CRE_NRO_CREDITO,
                Tcr_Id = infoCredito.TCR_ID
            };
            var getPlanPayment = await ObtenerPlanPagosCredito(requetPlanPayment);
            var infoPaymentDate = getPlanPayment.Data.PlanPago.Where(x => x.Fecha.Date == request.Fecha_Pago_Cuota.Date).FirstOrDefault();

            var seguroVidaPendiente = infoPaymentDate.SeguroVida - allPayments.Sum(x => x.CPG_ABONO_SEGURO_VIDA);
            var seguroHogarPendiente = infoPaymentDate.SeguroHogar - allPayments.Sum(x => x.CPG_ABONO_SEGURO_HOGAR);
            var interesMorePendiente = infoCredito.CRE_ACUMULADO_INTERES_MORA;
            var interesCorrientePendiente = (infoCredito.TCR_ID == 1
                ? infoPaymentDate.InteresReducido
                : infoPaymentDate.Interes) - allPayments.Sum(x => x.CPG_ABONO_INTERES_CORRIENTE);
            var capitalMorePendiente = Math.Max(0, infoCredito.CRE_SALDO_CAPITAL_MORA);
            var capitalPendiente = (infoCredito.TCR_ID == 1
                ? infoPaymentDate.CapitalReducido
                : infoPaymentDate.Capital) - allPayments.Sum(x => x.CPG_ABONO_CAPITAL);

            // ═══ APLICAR JERARQUÍA DE PAGO (idéntica a OTROS_PAGOS) ═══
            decimal valorRestante = valorAAplicar;
            decimal pagoSeguroVida = 0, pagoSeguroHogar = 0, pagoInteresMora = 0;
            decimal pagoInteresCorriente = 0, pagoCapitalMora = 0, pagoCapital = 0;

            if (valorRestante > 0 && seguroVidaPendiente > 0)
            {
                pagoSeguroVida = Math.Min(valorRestante, seguroVidaPendiente);
                valorRestante -= pagoSeguroVida;
            }
            if (valorRestante > 0 && seguroHogarPendiente > 0)
            {
                pagoSeguroHogar = Math.Min(valorRestante, seguroHogarPendiente);
                valorRestante -= pagoSeguroHogar;
            }
            if (valorRestante > 0 && interesMorePendiente > 0)
            {
                pagoInteresMora = Math.Min(valorRestante, interesMorePendiente);
                valorRestante -= pagoInteresMora;
            }
            if (valorRestante > 0 && interesCorrientePendiente > 0)
            {
                pagoInteresCorriente = Math.Min(valorRestante, interesCorrientePendiente);
                valorRestante -= pagoInteresCorriente;
            }
            if (valorRestante > 0 && capitalMorePendiente > 0)
            {
                pagoCapitalMora = Math.Min(valorRestante, capitalMorePendiente);
                valorRestante -= pagoCapitalMora;
            }
            if (valorRestante > 0 && capitalPendiente > 0)
            {
                pagoCapital = Math.Min(valorRestante, capitalPendiente);
            }

            decimal totalPagadoMes = allPayments.Sum(x =>
                x.CPG_ABONO_SEGURO_VIDA + x.CPG_ABONO_SEGURO_HOGAR +
                x.CPG_ABONO_INTERES_MORA + x.CPG_ABONO_INTERES_CORRIENTE +
                x.CPG_ABONO_CAPITAL
            ) + pagoSeguroVida + pagoSeguroHogar + pagoInteresMora + pagoInteresCorriente + pagoCapitalMora + pagoCapital;

            var interes = infoCredito.TCR_ID == 1 ? infoPaymentDate.InteresReducido : infoPaymentDate.Interes;
            var capital = infoCredito.TCR_ID == 1 ? infoPaymentDate.CapitalReducido : infoPaymentDate.Capital;
            decimal moraPendiente = Math.Max(0, allPayments.Sum(x => x.CPG_ABONO_INTERES_MORA));
            decimal totalPlanMes = infoPaymentDate.SeguroVida + infoPaymentDate.SeguroHogar + moraPendiente + interes + capital + interesMorePendiente;

            request.SeguroVidaPendiente = pagoSeguroVida;
            request.SeguroHogarPendiente = pagoSeguroHogar;
            request.InteresMorePendiente = pagoInteresMora;
            request.InteresCorrientePendiente = pagoInteresCorriente;
            request.CapitalMorePendiente = 0;
            request.CapitalPendiente = pagoCapitalMora + pagoCapital;
            request.NumeroCredito = infoCredito.CRE_NRO_CREDITO;
            request.TotalPagadoMes = totalPagadoMes;
            request.TotalPlanMes = totalPlanMes;

            var result = await _carteraRepository.AplicarPagoMora(request);
            var requestCuotaAnterior = new RequestCarteraSeven
            {
                AplicaSimulacion = request.AplicaSimulacion,       // TODO: confirmar nombre real
                numeroCredito = request.NumeroCredito,           // TODO: confirmar nombre real
                tipoIdentificacion = 1,   // TODO: ¿viene en request o en infoCredito?
                NumeroDocumentoCliente = request.Identificacion, // TODO
                cpgId = result.CPG_ID,
                codigo = request.Codigo,
                nCuota = request.nCuota - 1,                        // TODO: confirmar que la cuota anterior es exactamente nCuota-1
                tipoCredito = infoCredito.TCR_ID,                   // TODO: confirmar nombre
                FechaPago = result.CPG_FECHA_PAGO   // clave: fecha de LA CUOTA ANTERIOR
            };
            await CrearCuotaCartera(requestCuotaAnterior, default);
            return _mapper.Map<AplicacionPagoDto>(result);
        }
        /// <summary>
        /// Aplicar Pago
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>19/10/2021</Date>
        public async Task<Response<AplicacionPagoDto>> AplicarPago(PagoAplicarDto request)
        {
            try
            {
                ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
                {
                    Nro_Credito = request.NumeroCredito,
                    Tcr_Id = request.Tcr_id
                };
                InformacionCredito infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);
                var CapitalAbonado = await this._carteraRepository.ObtenerCapitalNeto_AbonosExtra(request.Identificacion);
                var cuotasAcumuladas = await this._iClientRequestProvider.ObtenerCuotasNumeroIdentificacion(infoCredito.CLI_IDENTIFICACION);

                if (cuotasAcumuladas.IsSuccess)
                {
                    if (cuotasAcumuladas.Data is not null)
                    {
                        request.TipoActor = cuotasAcumuladas.Data.TipoActorId;
                        request.CuotasAcumuladas = cuotasAcumuladas.Data.Cuotas;
                    }
                }
                AplicacionPagoDto response = new AplicacionPagoDto();

                decimal moraTotal = 0;
                if (request.Codigo == "LIQUIDACION_LEASING")
                {
                    response = this._mapper.Map<AplicacionPagoDto>(await _carteraRepository.AplicarPagoLiquidacionLeasing(request));
                }
                else if (request.Codigo == "OPCION_COMPRA")
                {
                    response = this._mapper.Map<AplicacionPagoDto>(await _carteraRepository.AplicarPagoOpcionCompra(request));
                }
                else if (request.Codigo == "DISMINUYE_CUOTA")
                {
                    List<InsertPlanPagosDto> LstPln = new();
                    double NuevaCuotaReducido = 0.0;
                    double NuevaCuotaTrad = 0.0;

                    int cuotaI = 0; // 0 para el mes siguiente, 1 para 2 meses después

                    if (request.aplazarDisminucionCuota)
                    {
                        cuotaI = 1; // 1 para 2 meses después
                    }
                    else
                    {
                        cuotaI = 0; // 0 para el mes siguiente
                    }


                    if (request.ValorPago > infoCredito.CRE_SALDO_CAPITAL)
                    {
                        //El valor del pago no debe ser mayor al saldo reducido adeudado.
                        return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "El valor del pago no debe ser mayor al saldo reducido adeudado." };
                    }
                    else if (infoCredito.CRE_SALDO_CAPITAL_MORA > 0)
                    {
                        //Para disminuir las cuotas se requiere no estar en mora.
                        return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "Para disminuir las cuotas se requiere no estar en mora." };
                    }
                    else
                    {
                        if (request.Tcr_id == 1)
                        {
                            //Leasing
                            ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                            if (responsePlanPagoDto != null)
                            {
                                List<ConformacionLeasing> LstConformacionLeasing = new List<ConformacionLeasing>();
                                var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                                var PlazoActual = infoCredito.CRE_PLAZO_ACTUAL;
                                var ValorSaldoActual = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == 1).FirstOrDefault().SaldoInicial - (decimal)CapitalAbonado;
                                ;
                                var SaldoNuevo = ValorSaldoActual - request.ValorPago;

                                var DeudaRemanenteNueva = responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA") && x.Cuota == PlazoCancelado).LastOrDefault().DeudaRemanente - request.ValorPago;
                                var SaldoCapitalTradPagado = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalReducido) + (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                                var SaldoCapitalTradActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);
                                var CapitalPorPagar = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                                var SaldoCapitalTradNuevo = (double)SaldoCapitalTradActual - (double)request.ValorPago - CapitalPorPagar;

                                //Para Leasing es Cre_Saldo_Tradicional
                                if ((decimal)SaldoCapitalTradActual != (infoCredito.CRE_SALDO_CAPITAL_TRADICIONAL + infoCredito.CRE_ACUMULADO_CAPITAl))
                                {
                                    //Devuelvo Alerta para verificar porque los saldos no estan igual.
                                    return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "Alerta!! El saldo no coincide, verifique los pagos/abonos realizados." };
                                }

                                #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                                for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - 1; i++)
                                {
                                    double DeudaRemanenteMes = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - 1
                                        ? (double)responsePlanPagoDto.PlanPago[i].DeudaRemanente - (double)request.ValorPago
                                        : (double)responsePlanPagoDto.PlanPago[i].DeudaRemanente;

                                    InsertPlanPagosDto insertPlanPagosDto = new()
                                    {
                                        PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                        PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                                        PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                        PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].SaldoInicial,
                                        PlnCapital = responsePlanPagoDto.PlanPago[i].CapitalReducido,
                                        PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                                        PlnInteres = responsePlanPagoDto.PlanPago[i].InteresReducido,
                                        PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                                        PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                        PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                        PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                                        PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                        PlnValorDeudaRemanente = (decimal)DeudaRemanenteMes
                                    };
                                    if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                                    {
                                        insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                                    }
                                    LstPln.Add(insertPlanPagosDto);
                                }
                                #endregion

                                for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i <= infoCredito.CRE_PLAZO_TOTAL - 1; i++)
                                {


                                    if (i == infoCredito.CRE_PLAZO_TOTAL - 1)
                                    {
                                        Console.WriteLine($"Índice fuera de rango: i = {i - 1}, Tamaño de PlanPago = {responsePlanPagoDto.PlanPago.Count}");
                                    }
                                    if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                                    {
                                        ConformacionLeasing CL = new ConformacionLeasing();
                                        CL.Mes = i;
                                        //Tradicional
                                        CL.Seguros = (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroHogar + (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroVida;
                                        CL.Canon = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros, 0);
                                        CL.Intereses = Math.Round(SaldoCapitalTradNuevo * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                        CL.AporteCapital = CL.Canon - CL.Intereses;
                                        CL.SaldoaCapital = SaldoCapitalTradNuevo - CL.AporteCapital;
                                        CL.CanonSeguro = CL.Canon + CL.Seguros;

                                        //Leasing
                                        CL.InteresReducido = Math.Round((double)SaldoNuevo * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                        CL.CanonReducido = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros;
                                        CL.InteresRemanente = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i + 1).FirstOrDefault().InteresPorPagar : 0;
                                        CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                                        CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i + 1).FirstOrDefault().CapitalPorPagar : 0;
                                        CL.SaldoaCapitalReducido = (double)SaldoNuevo - CL.AporteCapitalReducido;
                                        CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                                        CL.DeudaRemanente = (double)DeudaRemanenteNueva - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                                        CL.DeudaRemanenteAnterior = (double)LstPln[i - 1].PlnValorDeudaRemanente;

                                        LstConformacionLeasing.Add(CL);
                                        //Insertamos las nuevas cuotas
                                        InsertPlanPagosDto insertPlanPagosDto = new()
                                        {
                                            PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                            PlnCuota = i + 1,
                                            PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                            PlnSaldoIncial = (decimal)LstPln[i - 1].PlnValorDeudaRemanente,
                                            PlnCapital = (decimal)CL.AporteCapitalReducido,
                                            PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                            PlnInteres = (decimal)CL.InteresReducido,
                                            PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                            PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                            PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                            PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                            PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                            PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                                        };
                                        LstPln.Add(insertPlanPagosDto);
                                        //Luego de que quede el mes siguiente bien, calculamos la nueva cuota
                                        NuevaCuotaReducido = Math.Round(await CalcularPago(infoCredito.CRE_TASA_CREDITO_PER, infoCredito.CRE_PLAZO_TOTAL - (i + 1), -CL.SaldoaCapitalReducido, 0, 0), 0);
                                        var DifCuota = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros - NuevaCuotaReducido;
                                        NuevaCuotaTrad = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros - DifCuota, 0);
                                    }
                                    else
                                    {
                                        ConformacionLeasing CL = new ConformacionLeasing();
                                        CL.Mes = i;
                                        //Tradicional
                                        CL.Canon = (double)NuevaCuotaTrad;
                                        CL.Intereses = Math.Round(LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                        CL.AporteCapital = CL.Canon - CL.Intereses;
                                        CL.SaldoaCapital = LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital - CL.AporteCapital;
                                        CL.Seguros = (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroHogar + (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroVida;
                                        CL.CanonSeguro = CL.Canon + CL.Seguros;

                                        //Leasing
                                        CL.InteresReducido = Math.Round(LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                        CL.CanonReducido = i == infoCredito.CRE_PLAZO_TOTAL ? LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido + CL.InteresReducido : (double)NuevaCuotaReducido;
                                        CL.InteresRemanente = infoCredito.CLC_ID != 3 ? CL.Intereses - CL.InteresReducido : 0;
                                        CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                                        CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? CL.AporteCapital - CL.AporteCapitalReducido : 0;
                                        CL.SaldoaCapitalReducido = LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido - CL.AporteCapitalReducido;
                                        CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                                        CL.DeudaRemanente = LstConformacionLeasing[LstConformacionLeasing.Count - 1].DeudaRemanente - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                                        LstConformacionLeasing.Add(CL);
                                        //Insertamos las nuevas cuotas

                                        //decimal NuevoCalculoSaldoTradicional = SaldoCapitalTradicional - ValorPago;



                                        InsertPlanPagosDto insertPlanPagosDto = new()
                                        {
                                            PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                            PlnCuota = i + 1,
                                            PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                            PlnSaldoIncial = (decimal)LstPln[i - 1].PlnValorDeudaRemanente,
                                            PlnCapital = (decimal)CL.AporteCapitalReducido,
                                            PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                            PlnInteres = (decimal)CL.InteresReducido,
                                            PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                            PlnSeguroHogar = responsePlanPagoDto.PlanPago[i - 1].SeguroHogar,
                                            PlnSeguroVida = responsePlanPagoDto.PlanPago[i - 1].SeguroVida,
                                            PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                            PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                            PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                                        };
                                        LstPln.Add(insertPlanPagosDto);
                                    }
                                }
                                #region Comienzo de abono pago
                                RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                                {
                                    LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                                    NumeroCredito = request.NumeroCredito,
                                    Identificacion = request.Identificacion,
                                    Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                                    Fecha_Pago = request.Fecha_Pago,
                                    NuevaCuotaReducida = NuevaCuotaReducido,
                                    NuevaCuotaTradicional = NuevaCuotaTrad,
                                    PlazoTotal = infoCredito.CRE_PLAZO_TOTAL,
                                    PlazoActual = infoCredito.CRE_PLAZO_ACTUAL,
                                    NuevoSaldoReducido = (double)SaldoNuevo,
                                    NuevoSaldoTradicional = SaldoCapitalTradNuevo,
                                    FechaFinalizaPagos = null,
                                    PagoExtraOrdinario = 0
                                };
                                #endregion
                                response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.AplicaPagoReducirCuota(requestPagoAbonoCapital, infoCredito, request));
                            }
                        }
                        else
                        {
                            //Hipotecario
                            ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                            if (responsePlanPagoDto != null)
                            {
                                List<Hipotecario> LstConformacionHipotecario = new();
                                var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                                var PlazoActual = infoCredito.CRE_PLAZO_ACTUAL;
                                var ValorSaldoActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);
                                var SaldoNuevo = ValorSaldoActual - request.ValorPago;

                                //Para Hipotecario es Cre_Saldo_Capital
                                if (!(Math.Abs(ValorSaldoActual - infoCredito.CRE_SALDO_CAPITAL) <= 10))
                                {
                                    //Devuelvo Alerta para verificar porque los saldos no estan igual.
                                    return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "Alerta!! El saldo no coincide, verifique los pagos/abonos realizados." };
                                }

                                #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                                for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i++)
                                {
                                    InsertPlanPagosDto insertPlanPagosDto = new()
                                    {
                                        PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                        PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                                        PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                        PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].Saldo,
                                        PlnCapital = responsePlanPagoDto.PlanPago[i].Capital,
                                        PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                                        PlnInteres = responsePlanPagoDto.PlanPago[i].Interes,
                                        PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                                        PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                        PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                        PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                                        PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                        PlnValorDeudaRemanente = responsePlanPagoDto.PlanPago[i].DeudaRemanente
                                    };
                                    if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                                    {
                                        insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                                        //NuevaCuotaReducido = (decimal)Math.Round(await CalcularPago(infoCredito.CRE_TASA_CREDITO_PER, infoCredito.CRE_PLAZO_TOTAL - i, (double)SaldoNuevo, 0, 0), 0) + responsePlanPagoDto.PlanPago[i].SeguroHogar;
                                        var tasaCompuesta = infoCredito.CRE_TASA_CREDITO_PER + (infoCredito.CRE_TASA_SEGURO_VIDA / 100);
                                        NuevaCuotaReducido = -Math.Round(await CalcularPago(
                                        tasaCompuesta,
                                        infoCredito.CRE_PLAZO_TOTAL - i,
                                        (double)SaldoNuevo,
                                        0,
                                        0), 0)
                                        + (double)responsePlanPagoDto.PlanPago[i].SeguroHogar;


                                        //NuevaCuotaReducido = Math.Round(await CalcularPago(infoCredito.CRE_TASA_CREDITO_PER, infoCredito.CRE_PLAZO_TOTAL - i, (double)SaldoNuevo, 0, 0), 0) + responsePlanPagoDto.PlanPago[i].SeguroHogar;

                                    }
                                    LstPln.Add(insertPlanPagosDto);
                                }
                                #endregion

                                for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + (1 - cuotaI); i <= infoCredito.CRE_PLAZO_TOTAL - cuotaI; i++)
                                {


                                    if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + (1 - cuotaI))
                                    {
                                        Hipotecario H = new();

                                        H.Cuota = i + cuotaI;
                                        H.Seguro_Vida = Math.Round((double)SaldoNuevo * (infoCredito.CRE_TASA_SEGURO_VIDA / 100), 0);
                                        H.Seguro_Hogar = Math.Round(
                                            Math.Max((double)infoCredito.CRE_VALOR_VIVIENDA, (double)infoCredito.CRE_VALOR_AVALUO_COMERCIAL)
                                            * infoCredito.CRE_TASA_SEGURO_HOGAR / 100,
                                            0
                                        );

                                        if (cuotaI == 1)
                                        {
                                            H.Cuota_Pagadora = Math.Round((double)infoCredito.CRE_VALOR_CUOTA, 0);

                                        }
                                        else
                                        {
                                            H.Cuota_Pagadora = Math.Round(NuevaCuotaReducido, 0);

                                        }

                                        H.Intereses = Math.Round((double)SaldoNuevo * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                        H.Capital = H.Cuota_Pagadora - H.Seguro_Vida - H.Seguro_Hogar - H.Intereses;
                                        H.Saldo = (double)SaldoNuevo - H.Capital;
                                        LstConformacionHipotecario.Add(H);

                                        //Insertamos las nuevas cuotas
                                        InsertPlanPagosDto insertPlanPagosDto = new()
                                        {
                                            PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                            PlnCuota = i + cuotaI,
                                            PlnFechaPago = responsePlanPagoDto.PlanPago[i + cuotaI].Fecha,
                                            PlnCapital = (decimal)H.Capital,
                                            PlnSaldoIncial = (decimal)H.Saldo,
                                            PlnCapitalPorPagar = 0,
                                            PlnInteres = (decimal)H.Intereses,
                                            PlnInteresPorPagar = 0,
                                            PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                            PlnSeguroVida = (decimal)H.Seguro_Vida,
                                            PlnAPagar = (decimal)H.Cuota_Pagadora,
                                            PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                            PlnValorDeudaRemanente = 0
                                        };
                                        LstPln.Add(insertPlanPagosDto);
                                        //Luego de que quede el mes siguiente bien, calculamos la nueva cuota
                                        if (cuotaI == 1)
                                        {

                                            var tasaCompuesta = infoCredito.CRE_TASA_CREDITO_PER + (infoCredito.CRE_TASA_SEGURO_VIDA / 100);

                                            NuevaCuotaReducido = -Math.Round(await CalcularPago(
                                            tasaCompuesta,
                                            infoCredito.CRE_PLAZO_TOTAL - (i + 1),
                                            H.Saldo,
                                            0,
                                            0), 0)
                                            + (double)responsePlanPagoDto.PlanPago[i].SeguroHogar;

                                        }

                                    }
                                    else
                                    {

                                        Hipotecario H = new();



                                        var PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].Saldo;
                                        H.Cuota = i + cuotaI;
                                        H.Seguro_Vida = Math.Round((double)LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo * (infoCredito.CRE_TASA_SEGURO_VIDA / 100), 0);
                                        H.Seguro_Hogar = Math.Round(
                                            Math.Max((double)infoCredito.CRE_VALOR_VIVIENDA, (double)infoCredito.CRE_VALOR_AVALUO_COMERCIAL)
                                            * infoCredito.CRE_TASA_SEGURO_HOGAR / 100,
                                            0
                                        );
                                        H.Intereses = Math.Round((double)LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                        H.Cuota_Pagadora = i == infoCredito.CRE_PLAZO_TOTAL ? Math.Round(LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo + H.Intereses + H.Seguro_Hogar + H.Seguro_Vida) : Math.Round((double)NuevaCuotaReducido, 0);
                                        H.Capital = H.Cuota_Pagadora - H.Seguro_Vida - H.Seguro_Hogar - H.Intereses;
                                        H.Saldo = (double)LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo - H.Capital;
                                        LstConformacionHipotecario.Add(H);

                                        //Insertamos las nuevas cuotas
                                        InsertPlanPagosDto insertPlanPagosDto = new()
                                        {
                                            PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                            PlnCuota = i + cuotaI,
                                            PlnFechaPago = responsePlanPagoDto.PlanPago[i + cuotaI].Fecha,
                                            PlnCapital = (decimal)H.Capital,
                                            PlnSaldoIncial = (decimal)H.Saldo,
                                            PlnCapitalPorPagar = 0,
                                            PlnInteres = (decimal)H.Intereses,
                                            PlnInteresPorPagar = 0,
                                            PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                            PlnSeguroVida = (decimal)H.Seguro_Vida,
                                            PlnAPagar = (decimal)H.Cuota_Pagadora,
                                            PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                            PlnValorDeudaRemanente = 0
                                        };
                                        LstPln.Add(insertPlanPagosDto);
                                    }
                                }

                                if (cuotaI == 1)
                                {
                                    NuevaCuotaReducido = (double)infoCredito.CRE_VALOR_CUOTA;

                                }

                                #region Comienzo de Abono Capital
                                RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                                {
                                    LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                                    NumeroCredito = request.NumeroCredito,
                                    Identificacion = request.Identificacion,
                                    Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                                    Fecha_Pago = request.Fecha_Pago,
                                    NuevaCuotaReducida = NuevaCuotaReducido,
                                    NuevaCuotaTradicional = NuevaCuotaTrad,
                                    PlazoTotal = infoCredito.CRE_PLAZO_TOTAL,
                                    PlazoActual = infoCredito.CRE_PLAZO_ACTUAL,
                                    NuevoSaldoReducido = (double)SaldoNuevo,
                                    NuevoSaldoTradicional = 0,
                                    FechaFinalizaPagos = null,
                                    PagoExtraOrdinario = 0
                                };
                                #endregion
                                response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.AplicaPagoReducirCuota(requestPagoAbonoCapital, infoCredito, request));
                            }
                        }
                    }
                }
                else if (request.Codigo == "DISMINUYE_OBLIGACION")
                {
                    List<InsertPlanPagosDto> LstPln = new();
                    if (request.ValorPago > infoCredito.CRE_SALDO_CAPITAL)
                    {
                        //El valor del pago no debe ser mayor al saldo reducido adeudado.
                        return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "El valor del pago no debe ser mayor al saldo reducido adeudado." };
                    }
                    if (infoCredito.CRE_SALDO_CAPITAL_MORA > 0)
                    {
                        moraTotal = infoCredito.CRE_SALDO_CAPITAL_MORA
                            + (infoCredito.CRE_ACUMULADO_INTERES_MORA > 0 ? infoCredito.CRE_ACUMULADO_INTERES_MORA : 0);

                        if (moraTotal > 0)
                        {
                            decimal valorOriginal = request.ValorPago;
                            decimal valorAplicadoAMora = Math.Min(valorOriginal, moraTotal);
                            PagoAplicarDto requestOtrosPagos = new PagoAplicarDto
                            {
                                NumeroCredito = request.NumeroCredito,
                                ValorPago = valorAplicadoAMora, // <-- distinto del request original
                                Identificacion = request.Identificacion,
                                Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                                Fecha_Pago = request.Fecha_Pago,
                                Codigo = "OTROS_PAGOS", // <-- distinto del request original
                                nCuota = request.nCuota,
                                TipoActor = request.TipoActor,
                                CuotasAcumuladas = request.CuotasAcumuladas,
                                CreadoPor = request.CreadoPor,
                                NumeroCuotas = request.NumeroCuotas,
                                AplicaSimulacion = request.AplicaSimulacion,
                                Tcr_id = request.Tcr_id,
                                aplazarDisminucionCuota = request.aplazarDisminucionCuota,
                                ValorInteresRemanente = request.ValorInteresRemanente,
                                ValorCapitalRemanente = request.ValorCapitalRemanente,
                                IncluyeMora = request.IncluyeMora,
                                SeguroVidaPendiente = request.SeguroVidaPendiente,
                                SeguroHogarPendiente = request.SeguroHogarPendiente,
                                InteresMorePendiente = request.InteresMorePendiente,
                                InteresCorrientePendiente = request.InteresCorrientePendiente,
                                CapitalMorePendiente = request.CapitalMorePendiente,
                                CapitalPendiente = request.CapitalPendiente,
                                Observaciones = request.Observaciones,
                                TotalPagadoMes = request.TotalPagadoMes,
                                TotalPlanMes = request.TotalPlanMes,
                                TipoRecurso = request.TipoRecurso
                            };
                            response = await AplicarValorComoOtrosPagos(requestOtrosPagos, infoCredito, valorAplicadoAMora);

                            if (valorOriginal <= moraTotal)
                            {
                                // SUPUESTO A CONFIRMAR: si el pago no alcanza ni para cubrir toda la
                                // mora, se aplica todo como abono a mora (arriba) y NO se hace ninguna
                                // disminución este mes -- en vez de rechazar el pago como antes.

                            }
                            else
                            {
                                // Ya se cubrió toda la mora; el resto sigue el flujo normal de disminución.
                                request.ValorPago = valorOriginal - moraTotal;
                            }
                        }
                    }
                    if (request.Tcr_id == 1)
                    {
                        //Leasing
                        ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                        if (responsePlanPagoDto != null)
                        {
                            List<ConformacionLeasing> LstConformacionLeasing = new List<ConformacionLeasing>();
                            var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                            var PlazoActual = infoCredito.CRE_PLAZO_ACTUAL;
                            var ValorSaldoActual = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == 1).FirstOrDefault().SaldoInicial - (decimal)CapitalAbonado;
                            var SaldoNuevo = ValorSaldoActual - request.ValorPago;
                            var DeudaRemanenteNueva = responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA") && x.Cuota == PlazoCancelado).LastOrDefault().DeudaRemanente - request.ValorPago;
                            var SaldoCapitalTradPagado = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalReducido) + (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                            var SaldoCapitalTradActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0); ;
                            var SaldoCapitalTradNuevo = (double)SaldoCapitalTradActual - (double)request.ValorPago;
                            var DeudaRemanenteEvaluada = 0.0;
                            //Para Leasing es Cre_Saldo_Tradicional
                            //if ((decimal)SaldoCapitalTradActual != infoCredito.CRE_SALDO_CAPITAL_TRADICIONAL)
                            //{
                            //    //Devuelvo Alerta para verificar porque los saldos no estan igual.
                            //    return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "Alerta!! El saldo no coincide, verifique los pagos/abonos realizados." };
                            //}
                            #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                            for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i++)
                            {
                                InsertPlanPagosDto insertPlanPagosDto = new()
                                {
                                    PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                    PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                                    PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                    PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].SaldoInicial,
                                    PlnCapital = responsePlanPagoDto.PlanPago[i].CapitalReducido,
                                    PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                                    PlnInteres = responsePlanPagoDto.PlanPago[i].InteresReducido,
                                    PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                                    PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                    PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                    PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                                    PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                    PlnValorDeudaRemanente = responsePlanPagoDto.PlanPago[i].DeudaRemanente
                                };
                                LstPln.Add(insertPlanPagosDto);
                                if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                                {
                                    insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                                }
                            }
                            #endregion
                            for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1; i <= infoCredito.CRE_PLAZO_TOTAL; i++)
                            {
                                var SaldoTradEvaluado = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1 ? SaldoCapitalTradNuevo : LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital;
                                var SaldoRedEvaluado = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1 ? (double)SaldoNuevo : LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido;
                                if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1)
                                    DeudaRemanenteEvaluada = (double)DeudaRemanenteNueva;
                                ConformacionLeasing CL = new ConformacionLeasing();
                                CL.Mes = i;
                                //Tradicional
                                CL.Seguros = (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroHogar + (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroVida;
                                CL.Canon = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros, 0);
                                CL.Intereses = Math.Round(SaldoTradEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                CL.AporteCapital = CL.Canon - CL.Intereses;
                                CL.SaldoaCapital = SaldoTradEvaluado - CL.AporteCapital;
                                CL.CanonSeguro = CL.Canon + CL.Seguros;
                                //Leasing
                                CL.InteresReducido = Math.Round((double)SaldoRedEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                CL.CanonReducido = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros;
                                CL.InteresRemanente = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i).FirstOrDefault().InteresPorPagar : 0;
                                CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                                CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i).FirstOrDefault().CapitalPorPagar : 0;
                                CL.SaldoaCapitalReducido = (double)SaldoRedEvaluado - CL.AporteCapitalReducido;
                                CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                                CL.DeudaRemanente = CL.SaldoaCapitalReducido < 0 || CL.SaldoaCapital < 0 ? DeudaRemanenteEvaluada : (double)DeudaRemanenteEvaluada - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                                DeudaRemanenteEvaluada = CL.DeudaRemanente;
                                if (CL.SaldoaCapitalReducido < 0 || CL.SaldoaCapital < 0)
                                {
                                    //Tradicional
                                    CL.Intereses = Math.Round(SaldoTradEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                    CL.Canon = SaldoTradEvaluado + CL.Intereses;
                                    CL.AporteCapital = CL.Canon - CL.Intereses;
                                    CL.SaldoaCapital = SaldoTradEvaluado - CL.AporteCapital;
                                    CL.CanonSeguro = CL.Canon + CL.Seguros;
                                    //Leasing
                                    CL.InteresReducido = Math.Round((double)SaldoRedEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                    CL.CanonReducido = (double)SaldoRedEvaluado + CL.InteresReducido;
                                    CL.InteresRemanente = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i).FirstOrDefault().InteresPorPagar : 0;
                                    CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                                    CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i).FirstOrDefault().CapitalPorPagar : 0;
                                    CL.SaldoaCapitalReducido = (double)SaldoRedEvaluado - CL.AporteCapitalReducido;
                                    CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                                    CL.DeudaRemanente = (double)DeudaRemanenteEvaluada - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                                    LstConformacionLeasing.Add(CL);
                                    //Insertamos las nuevas cuotas
                                    InsertPlanPagosDto insertPlanPagosDto = new()
                                    {
                                        PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                        PlnCuota = i,
                                        PlnFechaPago = responsePlanPagoDto.PlanPago[i - 1].Fecha,
                                        PlnSaldoIncial = (decimal)CL.SaldoaCapitalReducido,
                                        PlnCapital = (decimal)CL.AporteCapitalReducido,
                                        PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                        PlnInteres = (decimal)CL.InteresReducido,
                                        PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                        PlnSeguroHogar = responsePlanPagoDto.PlanPago[i - 1].SeguroHogar,
                                        PlnSeguroVida = responsePlanPagoDto.PlanPago[i - 1].SeguroVida,
                                        PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                        PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                        PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                                    };
                                    LstPln.Add(insertPlanPagosDto);
                                    foreach (InsertPlanPagosDto item in LstPln)
                                        item.PlnPlazo = i;
                                    break;
                                }
                                else
                                {
                                    LstConformacionLeasing.Add(CL);
                                    //Insertamos las nuevas cuotas
                                    InsertPlanPagosDto insertPlanPagosDto = new()
                                    {
                                        PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                        PlnCuota = i,
                                        PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                        PlnSaldoIncial = (decimal)CL.SaldoaCapitalReducido,
                                        PlnCapital = (decimal)CL.AporteCapitalReducido,
                                        PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                        PlnInteres = (decimal)CL.InteresReducido,
                                        PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                        PlnSeguroHogar = responsePlanPagoDto.PlanPago[i - 1].SeguroHogar,
                                        PlnSeguroVida = responsePlanPagoDto.PlanPago[i - 1].SeguroVida,
                                        PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                        PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                        PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                                    };
                                    LstPln.Add(insertPlanPagosDto);
                                }
                            }
                            #region Comienzo de abono pago
                            RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                            {
                                LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                                NumeroCredito = request.NumeroCredito,
                                Identificacion = request.Identificacion,
                                Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                                Fecha_Pago = request.Fecha_Pago,
                                NuevaCuotaReducida = (double)infoCredito.CRE_VALOR_CUOTA,
                                NuevaCuotaTradicional = (double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL,
                                PlazoTotal = LstPln.LastOrDefault().PlnCuota,
                                PlazoActual = LstPln.LastOrDefault().PlnCuota - infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS,
                                NuevoSaldoReducido = (double)SaldoNuevo,
                                NuevoSaldoTradicional = SaldoCapitalTradNuevo,
                                FechaFinalizaPagos = LstPln.LastOrDefault().PlnFechaPago,
                                PagoExtraOrdinario = (double)request.ValorPago
                            };
                            #endregion
                            response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.AplicaPagoReducirPlazo(requestPagoAbonoCapital, infoCredito, request));
                        }
                    }
                    else
                    {
                        //Hipotecario
                        ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                        if (responsePlanPagoDto != null)
                        {
                            List<Hipotecario> LstConformacionHipotecario = new();
                            var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                            var PlazoActual = infoCredito.CRE_PLAZO_ACTUAL;
                            var ValorSaldoActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);
                            //var SaldoNuevo = ValorSaldoActual - request.ValorPago;
                            var saldoActualPlan = responsePlanPagoDto.PlanPago
                                .Where(x => x.Cuota == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                                .Select(x => x.Saldo)
                                .FirstOrDefault();
                            var SaldoNuevo = saldoActualPlan - request.ValorPago;

                            var fartorCuotai = 0;
                            var fartorCuotaf = 1;
                            //Para Hipotecario es Cre_Saldo_Capital
                            //if (Math.Abs(ValorSaldoActual - infoCredito.CRE_SALDO_CAPITAL) > 90)
                            //{
                            //    //Devuelvo Alerta para verificar porque los saldos no estan igual.
                            //    var difSaldo = Math.Abs(ValorSaldoActual - infoCredito.CRE_SALDO_CAPITAL);
                            //    return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "Alerta!! El saldo no coincide, verifique los pagos/abonos realizados." };
                            //}
                            if (responsePlanPagoDto.PlanPago[0].Cuota == 1)
                            {
                                fartorCuotai = 1;
                                fartorCuotaf = 0;
                            }
                            #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                            for (int i = 0; i < infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - fartorCuotai; i++)
                            {
                                InsertPlanPagosDto insertPlanPagosDto = new()
                                {
                                    PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                    PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                                    PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                    PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].Saldo,
                                    PlnCapital = responsePlanPagoDto.PlanPago[i].Capital,
                                    PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                                    PlnInteres = responsePlanPagoDto.PlanPago[i].Interes,
                                    PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                                    PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                    PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                    PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                                    PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                    PlnValorDeudaRemanente = responsePlanPagoDto.PlanPago[i].DeudaRemanente
                                };
                                // FIX APLICADO AQUI: se quitó el bloque
                                //   if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - fartorCuotai)
                                //       insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                                // que pisaba el saldo inicial de la última cuota YA PAGADA
                                // (noviembre) con el saldo nuevo post-abono. El saldo nuevo
                                // se aplica correctamente más abajo, en el for de
                                // reconformación, sobre la primera cuota NO pagada (diciembre).
                                LstPln.Add(insertPlanPagosDto);
                            }
                            #endregion
                            // FIX SEGURO HOGAR: el seguro hogar se fija UNA SOLA VEZ al origen del
                            // crédito y no debe recalcularse en cada disminución de plazo/cuota.
                            // Si se recalcula con el avalúo vigente (infoCredito.CRE_VALOR_AVALUO_COMERCIAL),
                            // y el avalúo fue actualizado después, el valor del seguro cambia -- lo cual
                            // no debe pasar. Por eso se toma el valor que YA tiene el plan existente
                            // (constante en todas las cuotas) y se repite tal cual en las cuotas nuevas.
                            var SeguroHogarFijo = responsePlanPagoDto.PlanPago[infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS].SeguroHogar;
                            for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i <= infoCredito.CRE_PLAZO_TOTAL; i++)

                            {
                                var SaldoEvaluado = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS ? (double)SaldoNuevo : LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo;
                                Hipotecario H = new();
                                if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                                {
                                    // Esta cuota YA fue pagada con sus valores históricos (capital, interés,
                                    // seguros). El abono aplicado ahora es un pago extra que no cambia lo que
                                    // ya se cobró ese mes -- solo reduce el saldo que queda después. Por eso
                                    // se conservan los valores originales del plan para esta fila y SOLO se
                                    // sobreescribe el Saldo con SaldoNuevo.
                                    var cuotaOriginal = responsePlanPagoDto.PlanPago[i - fartorCuotai];
                                    H.Capital = (double)cuotaOriginal.Capital;
                                    H.Intereses = (double)cuotaOriginal.Interes;
                                    H.Seguro_Vida = (double)cuotaOriginal.SeguroVida;
                                    H.Seguro_Hogar = (double)cuotaOriginal.SeguroHogar;
                                    H.Cuota_Pagadora = (double)cuotaOriginal.CuotaUnidadPagadora;
                                    H.Saldo = (double)SaldoNuevo;
                                }
                                else
                                {
                                    H.Seguro_Vida = Math.Round(SaldoEvaluado * (infoCredito.CRE_TASA_SEGURO_VIDA / 100), 0);
                                    H.Seguro_Hogar = (double)SeguroHogarFijo;
                                    H.Cuota_Pagadora = Math.Round((double)infoCredito.CRE_VALOR_CUOTA, 0);
                                    H.Intereses = Math.Round(SaldoEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                    H.Capital = H.Cuota_Pagadora - H.Seguro_Vida - H.Seguro_Hogar - H.Intereses;
                                    H.Saldo = SaldoEvaluado - H.Capital;
                                }
                                if (H.Saldo < 0)
                                {
                                    H.Intereses = Math.Round(SaldoEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                                    H.Cuota_Pagadora = SaldoEvaluado + H.Seguro_Vida + H.Seguro_Hogar + H.Intereses;
                                    H.Capital = SaldoEvaluado;
                                    H.Saldo = SaldoEvaluado - H.Capital;
                                    LstConformacionHipotecario.Add(H);
                                    //Insertamos las nuevas cuotas
                                    InsertPlanPagosDto insertPlanPagosDto = new()
                                    {
                                        PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                        PlnCuota = i,
                                        PlnFechaPago = responsePlanPagoDto.PlanPago[i - fartorCuotai].Fecha,
                                        PlnCapital = (decimal)H.Capital,
                                        PlnSaldoIncial = (decimal)H.Saldo,
                                        PlnCapitalPorPagar = 0,
                                        PlnInteres = (decimal)H.Intereses,
                                        PlnInteresPorPagar = 0,
                                        PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                        PlnSeguroVida = (decimal)H.Seguro_Vida,
                                        PlnAPagar = (decimal)H.Cuota_Pagadora,
                                        PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                        PlnValorDeudaRemanente = 0
                                    };
                                    LstPln.Add(insertPlanPagosDto);
                                    foreach (InsertPlanPagosDto item in LstPln)
                                        item.PlnPlazo = i;
                                    break;
                                }
                                else
                                {
                                    LstConformacionHipotecario.Add(H);
                                    //Insertamos las nuevas cuotas
                                    InsertPlanPagosDto insertPlanPagosDto = new()
                                    {
                                        PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                        PlnCuota = i,
                                        PlnFechaPago = responsePlanPagoDto.PlanPago[i - fartorCuotai].Fecha,
                                        PlnCapital = (decimal)H.Capital,
                                        PlnSaldoIncial = (decimal)H.Saldo,
                                        PlnCapitalPorPagar = 0,
                                        PlnInteres = (decimal)H.Intereses,
                                        PlnInteresPorPagar = 0,
                                        PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                        PlnSeguroVida = (decimal)H.Seguro_Vida,
                                        PlnAPagar = (decimal)H.Cuota_Pagadora,
                                        PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                        PlnValorDeudaRemanente = 0
                                    };
                                    LstPln.Add(insertPlanPagosDto);
                                }
                            }
                            #region Comienzo de Abono Pago
                            RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                            {
                                LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                                NumeroCredito = request.NumeroCredito,
                                Identificacion = request.Identificacion,
                                Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                                Fecha_Pago = request.Fecha_Pago,
                                NuevaCuotaReducida = (double)infoCredito.CRE_VALOR_CUOTA,
                                NuevaCuotaTradicional = (double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL,
                                PlazoTotal = LstPln.LastOrDefault().PlnCuota,
                                PlazoActual = LstPln.LastOrDefault().PlnCuota - infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS,
                                NuevoSaldoReducido = (double)SaldoNuevo,
                                NuevoSaldoTradicional = 0,
                                FechaFinalizaPagos = LstPln.LastOrDefault().PlnFechaPago,
                                PagoExtraOrdinario = (double)request.ValorPago
                            };
                            #endregion
                            response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.AplicaPagoReducirPlazo(requestPagoAbonoCapital, infoCredito, request));
                        }
                    }
                }

                else if (request.Codigo == "ABONO")
                {
                    List<InsertPlanPagosDto> LstPln = new();
                    var insertPlanLista = new List<InsertPlanPagosDto>();

                    if (request.Tcr_id == 1)
                    {
                        ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                        var planPagos = responsePlanPagoDto.PlanPago;
                        var cuotaActual = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1;
                        var infoCuota = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == cuotaActual).FirstOrDefault();
                        if ((infoCredito.ESC_ID == 1 || infoCredito.ESC_ID == 4) && request.ValorPago >= infoCuota.CuotaUnidadPagadora)
                        {
                            return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "El valor del pago no debe ser mayor  a la cuota unidad pagadora." };
                        }
                        var abonosRealizados = new List<AbonoHistorico>();
                        decimal saldo = request.ValorPago;
                        var todosLosAbonos = await _carteraRepository.ObtenerAbonoHistorico(
                            infoCredito.CRE_NRO_CREDITO.ToString(), infoCredito.TCR_ID);
                        while (saldo > 0)
                        {
                            var cuota = planPagos.FirstOrDefault(x => x.Cuota == cuotaActual);
                            if (cuota == null) break; // No hay más cuotas

                            // Obtener abonos anteriores de esta cuota específica
                            var abonosPrevios = todosLosAbonos.Where(x => x.AH_NUMERO_CUOTA == cuotaActual);
                            decimal pagadoHogar = abonosPrevios.Sum(x => x.AH_SEGURO_HOGAR);
                            decimal pagadoVida = abonosPrevios.Sum(x => x.AH_SEGURO_VIDA);
                            decimal pagadoInteres = abonosPrevios.Sum(x => x.AH_INTERES);
                            decimal pagadoCapital = abonosPrevios.Sum(x => x.AH_CAPITAL);

                            // Valores pendientes
                            decimal pendienteHogar = cuota.SeguroHogar - pagadoHogar;
                            decimal pendienteVida = cuota.SeguroVida - pagadoVida;
                            decimal pendienteInteres = cuota.InteresReducido - pagadoInteres;
                            decimal pendienteCapital = cuota.CapitalReducido - pagadoCapital;

                            // Crear registro de abono
                            var abono = new AbonoHistorico
                            {
                                AH_NUMERO_CUOTA = cuotaActual,
                                AH_SEGURO_HOGAR = 0,
                                AH_SEGURO_VIDA = 0,
                                AH_INTERES = 0,
                                AH_CAPITAL = 0,
                                AH_NUMERO_CREDITO = infoCredito.CRE_NRO_CREDITO.ToString(),
                                AH_TIPO_CREDITO = infoCredito.TCR_ID,
                                AH_VALOR_PAGO = request.ValorPago,
                                AH_FECHA_CREACION = request.Fecha_Pago,
                                CreadoPor = request.CreadoPor
                            };

                            // 1. Seguro Hogar
                            if (saldo > 0 && pendienteHogar > 0)
                            {
                                var pago = Math.Min(saldo, pendienteHogar);
                                abono.AH_SEGURO_HOGAR = pago;
                                saldo -= pago;
                                pendienteHogar -= pago;
                            }

                            // 2. Seguro Vida
                            if (saldo > 0 && pendienteVida > 0)
                            {
                                var pago = Math.Min(saldo, pendienteVida);
                                abono.AH_SEGURO_VIDA = pago;
                                saldo -= pago;
                                pendienteVida -= pago;
                            }

                            // 3. Interés
                            if (saldo > 0 && pendienteInteres > 0)
                            {
                                var pago = Math.Min(saldo, pendienteInteres);
                                abono.AH_INTERES = pago;
                                saldo -= pago;
                                pendienteInteres -= pago;
                            }

                            // 4. Capital
                            if (saldo > 0 && pendienteCapital > 0)
                            {
                                var pago = Math.Min(saldo, pendienteCapital);
                                abono.AH_CAPITAL = pago;
                                saldo -= pago;
                                pendienteCapital -= pago;
                            }

                            // Corregir el valor del pago con lo realmente abonado en esta cuota
                            abono.AH_VALOR_PAGO = abono.AH_SEGURO_HOGAR + abono.AH_SEGURO_VIDA + abono.AH_INTERES + abono.AH_CAPITAL;

                            // Registrar el abono
                            await _carteraRepository.InsertarAbonoHistorico(abono);

                            // Calcular totales pagados hasta ahora incluyendo el abono actual
                            decimal totalPagadoCapital = pagadoCapital + abono.AH_CAPITAL;
                            decimal totalPagadoInteres = pagadoInteres + abono.AH_INTERES;

                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = cuotaActual,
                                PlnFechaPago = request.Fecha_Pago,
                                PlnSaldoIncial = cuota.Saldo - abono.AH_CAPITAL,
                                PlnCapital = abono.AH_CAPITAL,
                                PlnCapitalPorPagar = cuota.Capital - totalPagadoCapital,
                                PlnInteres = abono.AH_INTERES,
                                PlnInteresPorPagar = cuota.Interes - totalPagadoInteres,
                                PlnSeguroHogar = abono.AH_SEGURO_HOGAR,
                                PlnSeguroVida = abono.AH_SEGURO_VIDA,
                                PlnAPagar = abono.AH_VALOR_PAGO,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = cuota.DeudaRemanente
                            };

                            // Insertar solo este registro (sin acumular en LstPln)
                            insertPlanLista = new List<InsertPlanPagosDto> { insertPlanPagosDto };
                            var insertPlan = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(insertPlanLista);
                            await _carteraRepository.CrearPlanPagosProductoClienteCartera(insertPlan, infoCredito.TCR_ID);

                            // Calcular el total de la cuota
                            decimal totalCuota = cuota.SeguroHogar + cuota.SeguroVida + cuota.InteresReducido + cuota.CapitalReducido;
                            decimal totalPagadoCuota = (pagadoHogar + abono.AH_SEGURO_HOGAR) +
                                                       (pagadoVida + abono.AH_SEGURO_VIDA) +
                                                       (pagadoInteres + abono.AH_INTERES) +
                                                       (pagadoCapital + abono.AH_CAPITAL);

                            // Si la cuota quedó totalmente pagada pasamos a la siguiente
                            if (totalPagadoCuota >= totalCuota)
                            {
                                cuotaActual++;  // Avanza a la siguiente cuota  
                                await _carteraRepository.ActualizarNumeroCuota(abono);
                            }
                            else
                            {
                                // Si aún queda pendiente, se detiene aquí
                                break;
                            }
                        }
                    }
                    else
                    {
                        ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                        var planPagos = responsePlanPagoDto.PlanPago;
                        var cuotaActual = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1;
                        var infoCuota = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == cuotaActual).FirstOrDefault();
                        if ((infoCredito.ESC_ID == 1 || infoCredito.ESC_ID == 4) && request.ValorPago >= infoCuota.CuotaUnidadPagadora)
                        {
                            return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "El valor del pago no debe ser mayor  a la cuota unidad pagadora." };
                        }
                        var abonosRealizados = new List<AbonoHistorico>();
                        decimal saldo = request.ValorPago;
                        var todosLosAbonos = await _carteraRepository.ObtenerAbonoHistorico(infoCredito.CRE_NRO_CREDITO.ToString(), infoCredito.TCR_ID);
                        while (saldo > 0)
                        {
                            var cuota = planPagos.FirstOrDefault(x => x.Cuota == cuotaActual);
                            if (cuota == null) break; // No hay más cuotas

                            // Obtener abonos anteriores de esta cuota específica
                            var abonosPrevios = todosLosAbonos.Where(x => x.AH_NUMERO_CUOTA == cuotaActual);
                            decimal pagadoHogar = abonosPrevios.Sum(x => x.AH_SEGURO_HOGAR);
                            decimal pagadoVida = abonosPrevios.Sum(x => x.AH_SEGURO_VIDA);
                            decimal pagadoInteres = abonosPrevios.Sum(x => x.AH_INTERES);
                            decimal pagadoCapital = abonosPrevios.Sum(x => x.AH_CAPITAL);

                            // Valores pendientes
                            decimal pendienteHogar = cuota.SeguroHogar - pagadoHogar;
                            decimal pendienteVida = cuota.SeguroVida - pagadoVida;
                            decimal pendienteInteres = cuota.Interes - pagadoInteres;
                            decimal pendienteCapital = cuota.Capital - pagadoCapital;

                            // Crear registro de abono
                            var abono = new AbonoHistorico
                            {
                                AH_NUMERO_CUOTA = cuotaActual,
                                AH_SEGURO_HOGAR = 0,
                                AH_SEGURO_VIDA = 0,
                                AH_INTERES = 0,
                                AH_CAPITAL = 0,
                                AH_NUMERO_CREDITO = infoCredito.CRE_NRO_CREDITO.ToString(),
                                AH_TIPO_CREDITO = infoCredito.TCR_ID,
                                AH_VALOR_PAGO = request.ValorPago,
                                AH_FECHA_CREACION = request.Fecha_Pago,
                                CreadoPor = request.CreadoPor,
                                TipoRecurso = request.TipoRecurso
                            };

                            // 1. Seguro Hogar
                            if (saldo > 0 && pendienteHogar > 0)
                            {
                                var pago = Math.Min(saldo, pendienteHogar);
                                abono.AH_SEGURO_HOGAR = pago;
                                saldo -= pago;
                                pendienteHogar -= pago;
                            }

                            // 2. Seguro Vida
                            if (saldo > 0 && pendienteVida > 0)
                            {
                                var pago = Math.Min(saldo, pendienteVida);
                                abono.AH_SEGURO_VIDA = pago;
                                saldo -= pago;
                                pendienteVida -= pago;
                            }

                            // 3. Interés
                            if (saldo > 0 && pendienteInteres > 0)
                            {
                                var pago = Math.Min(saldo, pendienteInteres);
                                abono.AH_INTERES = pago;
                                saldo -= pago;
                                pendienteInteres -= pago;
                            }

                            // 4. Capital
                            if (saldo > 0 && pendienteCapital > 0)
                            {
                                var pago = Math.Min(saldo, pendienteCapital);
                                abono.AH_CAPITAL = pago;
                                saldo -= pago;
                                pendienteCapital -= pago;
                            }

                            // Corregir el valor del pago con lo realmente abonado en esta cuota
                            abono.AH_VALOR_PAGO = abono.AH_SEGURO_HOGAR + abono.AH_SEGURO_VIDA + abono.AH_INTERES + abono.AH_CAPITAL;

                            // Registrar el abono
                            await _carteraRepository.InsertarAbonoHistorico(abono);

                            // Calcular totales pagados hasta ahora incluyendo el abono actual
                            decimal totalPagadoCapital = pagadoCapital + abono.AH_CAPITAL;
                            decimal totalPagadoInteres = pagadoInteres + abono.AH_INTERES;

                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = cuotaActual,
                                PlnFechaPago = request.Fecha_Pago,
                                PlnSaldoIncial = cuota.Saldo - abono.AH_CAPITAL,
                                PlnCapital = abono.AH_CAPITAL,
                                PlnCapitalPorPagar = cuota.Capital - totalPagadoCapital,
                                PlnInteres = abono.AH_INTERES,
                                PlnInteresPorPagar = cuota.Interes - totalPagadoInteres,
                                PlnSeguroHogar = abono.AH_SEGURO_HOGAR,
                                PlnSeguroVida = abono.AH_SEGURO_VIDA,
                                PlnAPagar = abono.AH_VALOR_PAGO,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = cuota.DeudaRemanente
                            };

                            insertPlanLista = new List<InsertPlanPagosDto> { insertPlanPagosDto };
                            var insertPlan = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(insertPlanLista);
                            await _carteraRepository.CrearPlanPagosProductoClienteCartera(insertPlan, infoCredito.TCR_ID);

                            // Calcular el total de la cuota
                            decimal totalCuota = cuota.SeguroHogar + cuota.SeguroVida + cuota.Interes + cuota.Capital;
                            decimal totalPagadoCuota = (pagadoHogar + abono.AH_SEGURO_HOGAR) +
                                                       (pagadoVida + abono.AH_SEGURO_VIDA) +
                                                       (pagadoInteres + abono.AH_INTERES) +
                                                       (pagadoCapital + abono.AH_CAPITAL);

                            // Si la cuota quedó totalmente pagada pasamos a la siguiente
                            if (totalPagadoCuota >= totalCuota)
                            {
                                cuotaActual++;  // Avanza a la siguiente cuota  
                                await _carteraRepository.ActualizarNumeroCuota(abono);
                            }
                        }
                    }
                    if (insertPlanLista.Count > 0)
                    {
                        response.AbonoCapital = insertPlanLista.Sum(x => x.PlnCapital);
                        response.ValorPago = insertPlanLista.Sum(x => x.PlnAPagar);
                        response.AbonoSeguroHogar = insertPlanLista.Sum(x => x.PlnSeguroHogar);
                        response.AbonoSeguroVida = insertPlanLista.Sum(x => x.PlnSeguroVida);
                        response.InteresRemanente = insertPlanLista.Sum(x => x.PlnInteres);
                    }
                    return new Response<AplicacionPagoDto>() { Data = response };
                }
                else if (request.Codigo == "OTROS_PAGOS")
                {
                    var aplicationPayment = await _carteraRepository.ObtenerAplicacionPago(infoCredito.CRE_ID);

                    //  Obtener TODOS los pagos del mes (sin incluir el actual aún)
                    var allPayments = aplicationPayment.Where(x =>
                        x.CPG_FECHA_APLICACION.Date == request.Fecha_Pago_Cuota.Date
                        && x.CPG_VALOR_PAGO > 0
                        && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2)).ToList();

                    if (allPayments.Any())
                    {
                        var requetPlanPayment = new ObtenerProductoCartera
                        {
                            Nro_Credito = infoCredito.CRE_NRO_CREDITO,
                            Tcr_Id = infoCredito.TCR_ID
                        };
                        var getPlanPayment = await ObtenerPlanPagosCredito(requetPlanPayment);
                        var infoPaymentDate = getPlanPayment.Data.PlanPago.Where(x => x.Fecha.Date == request.Fecha_Pago_Cuota.Date).FirstOrDefault();

                        //  Calcular pendientes ANTES de aplicar nuevo pago
                        var seguroVidaPendiente = infoPaymentDate.SeguroVida - allPayments.Sum(x => x.CPG_ABONO_SEGURO_VIDA);
                        var seguroHogarPendiente = infoPaymentDate.SeguroHogar - allPayments.Sum(x => x.CPG_ABONO_SEGURO_HOGAR);

                        //  CORRECCIÓN 1: Restar lo que ya se pagó de interés mora
                        var interesMorePendiente = infoCredito.CRE_ACUMULADO_INTERES_MORA;

                        var interesCorrientePendiente = (infoCredito.TCR_ID == 1
                            ? infoPaymentDate.InteresReducido
                            : infoPaymentDate.Interes) - allPayments.Sum(x => x.CPG_ABONO_INTERES_CORRIENTE);

                        //  CORRECCIÓN 2: Restar lo que ya se pagó de capital en mora
                        var capitalMorePendiente = Math.Max(0, infoCredito.CRE_SALDO_CAPITAL_MORA);

                        var capitalPendiente = (infoCredito.TCR_ID == 1
                            ? infoPaymentDate.CapitalReducido
                            : infoPaymentDate.Capital) - allPayments.Sum(x => x.CPG_ABONO_CAPITAL);

                        // ═══ APLICAR JERARQUÍA DE PAGO ═══
                        decimal valorRestante = request.ValorPago;
                        decimal pagoSeguroVida = 0, pagoSeguroHogar = 0, pagoInteresMora = 0;
                        decimal pagoInteresCorriente = 0, pagoCapitalMora = 0, pagoCapital = 0;

                        // 1. Seguro vida
                        if (valorRestante > 0 && seguroVidaPendiente > 0)
                        {
                            pagoSeguroVida = Math.Min(valorRestante, seguroVidaPendiente);
                            valorRestante -= pagoSeguroVida;
                        }

                        // 2. Seguro hogar
                        if (valorRestante > 0 && seguroHogarPendiente > 0)
                        {
                            pagoSeguroHogar = Math.Min(valorRestante, seguroHogarPendiente);
                            valorRestante -= pagoSeguroHogar;
                        }

                        // 3. Interés mora ← APLICAR JERARQUÍA PRIMERO
                        if (valorRestante > 0 && interesMorePendiente > 0)
                        {
                            pagoInteresMora = Math.Min(valorRestante, interesMorePendiente);
                            valorRestante -= pagoInteresMora;
                        }

                        // 4. Interés corriente
                        if (valorRestante > 0 && interesCorrientePendiente > 0)
                        {
                            pagoInteresCorriente = Math.Min(valorRestante, interesCorrientePendiente);
                            valorRestante -= pagoInteresCorriente;
                        }

                        // 5. Capital en mora ← DESPUÉS de interés mora
                        if (valorRestante > 0 && capitalMorePendiente > 0)
                        {
                            pagoCapitalMora = Math.Min(valorRestante, capitalMorePendiente);
                            valorRestante -= pagoCapitalMora;
                        }

                        // 6. Capital normal
                        if (valorRestante > 0 && capitalPendiente > 0)
                        {
                            pagoCapital = Math.Min(valorRestante, capitalPendiente);
                        }

                        //  CALCULAR TOTALES INCLUYENDO EL PAGO ACTUAL
                        decimal totalPagadoMes = allPayments.Sum(x =>
                            x.CPG_ABONO_SEGURO_VIDA + x.CPG_ABONO_SEGURO_HOGAR +
                            x.CPG_ABONO_INTERES_MORA + x.CPG_ABONO_INTERES_CORRIENTE +
                            x.CPG_ABONO_CAPITAL
                        ) + pagoSeguroVida + pagoSeguroHogar + pagoInteresMora + pagoInteresCorriente + pagoCapitalMora + pagoCapital;

                        //  Calcular total plan de la cuota
                        var interes = infoCredito.TCR_ID == 1
                            ? infoPaymentDate.InteresReducido
                            : infoPaymentDate.Interes;
                        var capital = infoCredito.TCR_ID == 1
                            ? infoPaymentDate.CapitalReducido
                            : infoPaymentDate.Capital;

                        //  CORRECCIÓN 3: Restar lo ya pagado de mora para calcular nuevo "moraPendiente"
                        decimal moraPendiente = Math.Max(0, allPayments.Sum(x => x.CPG_ABONO_INTERES_MORA));
                        decimal totalPlanMes = infoPaymentDate.SeguroVida + infoPaymentDate.SeguroHogar + moraPendiente + interes + capital + interesMorePendiente;

                        //  Asignar valores pagados
                        request.SeguroVidaPendiente = pagoSeguroVida;
                        request.SeguroHogarPendiente = pagoSeguroHogar;
                        request.InteresMorePendiente = pagoInteresMora;
                        request.InteresCorrientePendiente = pagoInteresCorriente;
                        request.CapitalMorePendiente = 0;
                        request.CapitalPendiente = pagoCapitalMora + pagoCapital;
                        request.NumeroCredito = infoCredito.CRE_NRO_CREDITO;
                        request.TotalPagadoMes = totalPagadoMes;  //  Incluye pagos anteriores + actual
                        request.TotalPlanMes = totalPlanMes;

                        var result = await _carteraRepository.AplicarPagoMora(request);
                        response = _mapper.Map<AplicacionPagoDto>(result);

                    }
                }
                else
                {
                    var movimientosAplicados = this._mapper.Map<IEnumerable<AplicacionPagoDto>>(await this._carteraRepository.AplicarPago(request)).ToList();
                    AplicacionPagoDto cuotaAnteriorDto = null;
                    AplicacionPagoDto cuotaActualDto;

                    if (movimientosAplicados.Count == 2)
                    {
                        cuotaAnteriorDto = this._mapper.Map<AplicacionPagoDto>(movimientosAplicados[0]);
                        cuotaActualDto = this._mapper.Map<AplicacionPagoDto>(movimientosAplicados[1]);
                    }
                    else
                    {
                        cuotaActualDto = this._mapper.Map<AplicacionPagoDto>(movimientosAplicados[0]);
                    }

                    response = cuotaActualDto; // esto y solo esto se retorna al front, igual que hoy

                    if (cuotaAnteriorDto != null)
                    {
                        var requestCuotaAnterior = new RequestCarteraSeven
                        {
                            AplicaSimulacion = request.AplicaSimulacion,       // TODO: confirmar nombre real
                            numeroCredito = request.NumeroCredito,           // TODO: confirmar nombre real
                            tipoIdentificacion = 1,   // TODO: ¿viene en request o en infoCredito?
                            NumeroDocumentoCliente = request.Identificacion, // TODO
                            cpgId = cuotaAnteriorDto.IdAplicacionPago,
                            codigo = request.Codigo,
                            nCuota = request.nCuota - 1,                        // TODO: confirmar que la cuota anterior es exactamente nCuota-1
                            tipoCredito = infoCredito.TCR_ID,                   // TODO: confirmar nombre
                            FechaPago = cuotaAnteriorDto.FechaPago   // clave: fecha de LA CUOTA ANTERIOR
                        };

                        await CrearCuotaCartera(requestCuotaAnterior, default);
                    }
                    if (request.Codigo == "PAGO_CUOTA_CREDITO" && infoCredito.ESC_ID == 2)
                    {
                        var pagoRegistrado = await _carteraRepository.RegistrarPagoEnMora(
                            creId: infoCredito.CRE_ID,
                            fechaPago: request.Fecha_Pago_Cuota,
                            fechaAplicacion: request.Fecha_Pago,
                            monto: request.ValorPago,
                            diasMoraAlPago: infoCredito.CRE_DIAS_MORA,
                            causacionMoraAlPago: infoCredito.CRE_ACUMULADO_INTERES_MORA
);
                    }
                }

                //// ACTUALIZAS EL TIPO DE RECURSO  
                ///

                await _carteraRepository.ActualizarTipoRecurso(response.IdAplicacionPago, request.TipoRecurso);


                return new Response<AplicacionPagoDto>() { Data = response };
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return new Response<AplicacionPagoDto>()
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Ocurrió un error al aplicar el pago: {ex.Message}",
                };
            }
        }
        public async Task<Response<List<AbonoHistoricoDto>>> ObtenerAbonoHistorico(string nroCredito, int tipoCredito)
        {
            var abonos = await _carteraRepository.ObtenerAbonoHistorico(nroCredito, tipoCredito);
            if (abonos.Count() > 0)
            {
                ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
                {
                    Nro_Credito = long.Parse(nroCredito),
                    Tcr_Id = tipoCredito
                };

            }


            return new Response<List<AbonoHistoricoDto>>()
            {
                Data = null,
                IsSuccess = false,
                ReturnMessage = $"Ocurrió un error al aplicar el pago.",
            };
        }
        public async Task<Response<List<AbonoHistoricoDto>>> ObtenerEstadoCuotasCredito(string nroCredito, int tipoCredito)
        {
            // Abonos realizados
            var abonos = await _carteraRepository.ObtenerAbonoHistorico(nroCredito, tipoCredito);
            if (abonos.Count() > 0)
            {
                ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
                {
                    Nro_Credito = long.Parse(nroCredito),
                    Tcr_Id = tipoCredito
                };

                // Plan de pagos completo
                var planPagos = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data.PlanPago;

                // Agrupar abonos por cuota
                var abonosPorCuota = abonos
                    .GroupBy(x => x.AH_NUMERO_CUOTA)
                    .Select(g => new
                    {
                        Cuota = g.Key,
                        SeguroHogar = g.Sum(x => x.AH_SEGURO_HOGAR),
                        SeguroVida = g.Sum(x => x.AH_SEGURO_VIDA),
                        Interes = g.Sum(x => x.AH_INTERES),
                        Capital = g.Sum(x => x.AH_CAPITAL),
                        ValorPago = g.Sum(x => x.AH_VALOR_PAGO)
                    })
                    .ToList();

                var resultado = new List<AbonoHistoricoDto>();

                foreach (var abono in abonosPorCuota)
                {
                    var cuota = planPagos.FirstOrDefault(x => x.Cuota == abono.Cuota);

                    if (cuota == null)
                        continue; // La cuota no existe en el plan de pagos, se omite.


                    // Obtener valores de la cuota según el tipo de crédito
                    decimal cuotaInteres = tipoCredito == 1 ? cuota.InteresReducido : cuota.Interes;
                    decimal cuotaCapital = tipoCredito == 1 ? cuota.CapitalReducido : cuota.Capital;

                    // Limitar los abonos al máximo de la cuota (evitar negativos)
                    decimal abonoSeguroHogarLimitado = Math.Min(abono.SeguroHogar, cuota.SeguroHogar);
                    decimal abonoSeguroVidaLimitado = Math.Min(abono.SeguroVida, cuota.SeguroVida);
                    decimal abonoInteresLimitado = Math.Min(abono.Interes, cuotaInteres);
                    decimal abonoCapitalLimitado = Math.Min(abono.Capital, cuotaCapital);

                    // Calcular pendientes (siempre >= 0)
                    decimal pendSeguroHogar = Math.Max(0, cuota.SeguroHogar - abono.SeguroHogar);
                    decimal pendSeguroVida = Math.Max(0, cuota.SeguroVida - abono.SeguroVida);
                    decimal pendInteres = Math.Max(0, cuotaInteres - abono.Interes);
                    decimal pendCapital = Math.Max(0, cuotaCapital - abono.Capital);

                    resultado.Add(new AbonoHistoricoDto
                    {
                        NumeroCuota = cuota.Cuota,
                        TipoCredito = tipoCredito,
                        NumeroCredito = nroCredito,
                        SeguroHogar = abonoSeguroHogarLimitado,
                        SeguroVida = abonoSeguroVidaLimitado,
                        Interes = abonoInteresLimitado,
                        Capital = abonoCapitalLimitado,
                        ValorPago = abono.ValorPago,
                        EstadoSeguroHogar = pendSeguroHogar == 0 ? "P" :
                                           abono.SeguroHogar > 0 ? "NC" : "ND",

                        EstadoSeguroVida = pendSeguroVida == 0 ? "P" :
                                          abono.SeguroVida > 0 ? "NC" : "ND",

                        EstadoInteres = pendInteres == 0 ? "P" :
                                        abono.Interes > 0 ? "NC" : "ND",

                        EstadoCapital = pendCapital == 0 ? "P" :
                                       abono.Capital > 0 ? "NC" : "ND",

                        PendienteSeguroHogar = pendSeguroHogar,
                        PendienteSeguroVida = pendSeguroVida,
                        PendienteInteres = pendInteres,
                        PendienteCapital = pendCapital,

                        AbonadoTotal = abonoSeguroHogarLimitado + abonoSeguroVidaLimitado + abonoInteresLimitado + abonoCapitalLimitado,
                        PendienteTotal = pendSeguroHogar + pendSeguroVida + pendInteres + pendCapital
                    });
                }

                return new Response<List<AbonoHistoricoDto>>()
                {
                    Data = resultado,
                    IsSuccess = true,
                    ReturnMessage = $"Abonos encontrados.",
                };
            }



            return new Response<List<AbonoHistoricoDto>>()
            {
                Data = null,
                IsSuccess = false,
                ReturnMessage = $"No se encontraron abonos realizado  para este credito",
            };
        }

        public async Task<Response<AplicacionPagoDto>> ReversarPago(RequestReversionPagoDto request)
        {
            AplicacionPagoDto response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.ReversarPago(request));

            if (response != null)
            {
                ComprobanteCarteraUpdate comprobanteCarteraUpdate = new();

                if (request.Estado_Id == 1)
                {
                    comprobanteCarteraUpdate.CC_ID = request.Comprobante_Id;
                    comprobanteCarteraUpdate.ESTADO = "Cancelado";
                    await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);
                }
                else if (request.Estado_Id == 2 || request.Estado_Id == 5)
                {
                    comprobanteCarteraUpdate.CC_ID = request.Comprobante_Id;
                    comprobanteCarteraUpdate.ESTADO = "Reversado";
                    await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);
                }
                //// 2. Cancelar comprobante garantía si existe
                var garantiaId = await _carteraRepository.ObtenerComprobanteGarantiaCartera(request.Comprobante_Id);

                if (garantiaId > 0 && garantiaId != null)
                {
                    string estadoNuevo = request.Estado_Id == 1 ? "Cancelado" : "Reversado";
                    comprobanteCarteraUpdate.CC_ID = garantiaId;
                    comprobanteCarteraUpdate.ESTADO = estadoNuevo;
                    await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);
                }
            }
            return new Response<AplicacionPagoDto>() { Data = response };
        }


        public async Task<Response<bool>> ReversarPagosLista(int idProceso)
        {
            //AplicacionPagoDto response = tActualizarAplicacionDePagosis._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.ReversarProcesoAplicacionPagos(request));
            var response = await _carteraRepository.ReversarProcesoAplicacionPagos(idProceso);

            return new Response<bool> { Data = response };
        }
        public async Task<Response<bool>> ReversarPagoUnitarioDeLista(RequestReversionPagoListaDto request)
        {
            var response = await _carteraRepository.ReversarPagoUnitarioDeLista(request);

            return new Response<bool> { Data = response };
        }

        /// <summary>
        /// Aplicar cuota
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>19/10/2021</Date>
        public async Task<Response<ComprobanteCarteraDto>> CrearCuotaCartera(RequestCarteraSeven request, CancellationToken cancellationToken)
        {
            Response<ComprobanteCarteraDto> response = new();
            if (request.codigo != "ADELANTO_CUOTA")
            {

                Dictionary<string, string> TipoOperacion = new Dictionary<string, string>
                {
                    { "DISMINUYE_OBLIGACION", "Disminuir el plazo de la obligación" },
                    { "DISMINUYE_CUOTA", "Disminuir el valor de la cuota" },
                    { "ADELANTO_CUOTA", "Adelantar cuotas" },
                    { "PAGO_CUOTA_CREDITO", "Pagar la cuota del crédito" },
                    { "DISMINUYE_REMANENTE", "Disminuir deuda remanente" },
                    { "CANCELACION_TOTAL", "Cancelación total" },
                    { "RESTITUCION_INMUEBLE", "Restitucion de inmueble" },
                    { "LIQUIDACION_LEASING", "Liquidacion Leasing" },
                    { "OPCION_COMPRA", "Opcion Compra" },
                    { "ACELERACION", "Aceleracion de deuda" },
                    { "REESTRUCTURACION", "restructuracion" },
                    { "ABONO", "Abono a la deuda" },
                    { "OTROS_PAGOS", "OTROS PAGOS" },
                };

                InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
                List<ResponseMov> detalleMovConta = new List<ResponseMov>();
                //List<ResponseMov> dataCabeceraMovConta = new List<ResponseMov>();

                var dataCabeceraMovConta = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();

                decimal totalDmcVacr = 0;
                decimal totalDmcCadb = 0;


                //List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovConta = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                //List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var items = new List<ResponseMov>();
                //var detalleMovConta = new List<ResponseMov>();

                var comprobante = "";
                int? comprobanteGarantiaId = 0;

                IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());


                if (request.codigo == "CANCELACION_TOTAL" || request.codigo == "OPCION_COMPRA"
                    || request.codigo == "LIQUIDACION_LEASING" || request.codigo == "RESTITUCION_INMUEBLE"
                    || request.codigo == "ACELERACION" || request.codigo == "REESTRUCTURACION")
                {
                    var PlanPago = await _carteraRepository.ConsultarPlanPago(request.numeroCredito, request.cpgId);

                    ReporteAPNRCAR reporteAPNRCAR = new ReporteAPNRCAR();

                    var fechaAjustada = request.FechaPago.AddMonths(-1);

                    ReporteAPNRCARParametersDto requestAPNRCAR = new ReporteAPNRCARParametersDto
                    {
                        Identificacion = (request.NumeroDocumentoCliente),
                        Mes = fechaAjustada.Month,
                        Anio = fechaAjustada.Year,
                        TipoCredito = request.tipoCredito
                    };

                    reporteAPNRCAR = (await this._reportesRepository.GenerarReporteAPNRCARPorAfiliado(requestAPNRCAR)).FirstOrDefault();

                    if (request.codigo == "ACELERACION" || request.codigo == "REESTRUCTURACION")
                    {
                        if (PlanPago.PLN_CAPITAL > PlanPago.PLN_A_PAGAR)
                        {
                            PlanPago.PLN_CAPITAL = PlanPago.PLN_A_PAGAR;

                        }
                        var dataProductClient = await _carteraRepository.ObtenerNovedadesCartera(PlanPago.CRE_ID);
                        PlanPago.PLN_CAPITAL = dataProductClient.Where(x => x.TNV_ID == 1 || x.TNV_ID == 3).FirstOrDefault().NVC_CAPITAL;
                        PlanPago.PLN_INTERESES = dataProductClient.Where(x => x.TNV_ID == 1 || x.TNV_ID == 3).FirstOrDefault().NVC_INTERES_REMANENTE;
                        PlanPago.PLN_A_PAGAR = dataProductClient.Where(x => x.TNV_ID == 1 || x.TNV_ID == 3).FirstOrDefault().NVC_TOTAL_DEUDA;
                    }



                    RequestProvisionIndSeven requestMovGarantia = new RequestProvisionIndSeven
                    {
                        numeroDocumentoCliente = request.NumeroDocumentoCliente,
                        tipoCredito = request.tipoCredito,
                        valorPagoCapital = request.codigo == "ACELERACION" || request.codigo == "REESTRUCTURACION" ? PlanPago.PLN_CAPITAL : reporteAPNRCAR.SALDO_CREDITO,
                        valorPagoInteres = request.codigo == "ACELERACION" || request.codigo == "REESTRUCTURACION" ? PlanPago.PLN_INTERESES : reporteAPNRCAR.SALDO_CUENTA_POR_COBRAR,
                        valorPagoTotal = request.codigo == "ACELERACION" || request.codigo == "REESTRUCTURACION"  ? PlanPago.PLN_A_PAGAR : reporteAPNRCAR.SALDO_CUENTA_POR_COBRAR,
                        numeroCredito = (request.numeroCredito).ToString(),
                        fechaProvision = request.FechaPago,
                        codigo = request.codigo
                    };
                    var comprobanteGarantia = await this._creditoBusinessLogic.CrearMovContableProvisionIndividualCredito(requestMovGarantia, cancellationToken);

                    comprobante = comprobanteGarantia.ReturnMessage;
                    // ← Directo, sin .Data.Comprobante_Id
                    comprobanteGarantiaId = comprobanteGarantia.IsSuccess
                        ? comprobanteGarantia.Data
                        : null;
                }


                if (request.tipoCredito == 1)
                {
                    var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);

                    dataCabeceraMovConta.Emp_codi = 294;
                    dataCabeceraMovConta.Top_codi = 9065;
                    dataCabeceraMovConta.Mco_nume = 0;
                    dataCabeceraMovConta.Mco_fech = request.FechaPago;
                    dataCabeceraMovConta.Mod_codi = 7;
                    dataCabeceraMovConta.Arb_csuc = "1001";
                    dataCabeceraMovConta.Mco_desc = TipoOperacion[request.codigo] + " " + request.FechaPago.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                    items = ObtenerDeTalleCuota(equivalenciaCuentasLeasing, request, unidadesEjecutoras).Result;


                    detalleMovConta.AddRange(items);
                    //dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();
                    //var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                    //movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                    totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
                    totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);
                }
                else if (request.tipoCredito == 2)
                {
                    var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);


                    dataCabeceraMovConta.Emp_codi = 294;
                    dataCabeceraMovConta.Top_codi = 9003;
                    dataCabeceraMovConta.Mco_nume = 0;
                    dataCabeceraMovConta.Mco_fech = request.FechaPago;
                    dataCabeceraMovConta.Mod_codi = 7;
                    dataCabeceraMovConta.Arb_csuc = "1001";
                    dataCabeceraMovConta.Mco_desc = TipoOperacion[request.codigo] + " " + request.FechaPago.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                    items = ObtenerDeTalleCuota(equivalenciaCuentasHipotecario, request, unidadesEjecutoras).Result;

                    detalleMovConta.AddRange(items);

                    totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
                    totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);
                }

                RequestDetallePago requestDetallePagoComprobante = new RequestDetallePago();
                requestDetallePagoComprobante.Cpg_id = request.cpgId;
                requestDetallePagoComprobante.Emp_codi = dataCabeceraMovConta.Emp_codi;
                requestDetallePagoComprobante.Top_codi = dataCabeceraMovConta.Top_codi;
                requestDetallePagoComprobante.Mco_fech = dataCabeceraMovConta.Mco_fech;
                requestDetallePagoComprobante.Mod_codi = dataCabeceraMovConta.Mod_codi;
                requestDetallePagoComprobante.Mco_desc = dataCabeceraMovConta.Mco_desc;
                requestDetallePagoComprobante.Fecha_Ejecucion = DateTime.Now;

                if (detalleMovConta.Count > 0)
                {
                    string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovConta);
                    requestDetallePagoComprobante.Mov_Contable_Seven = jsonMovimientosContables;
                    requestDetallePagoComprobante.Respuesta_Seven = response.ReturnMessage;
                }


                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoComprobante, "CAC", totalDmcVacr, totalDmcCadb);
                requestDetallePagoComprobante.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;

                UpdatePlanPago updatePlanPago = new UpdatePlanPago();
                updatePlanPago.numeroCredito = long.Parse(detalleMovConta[0].Dmc_refe);
                updatePlanPago.cpgId = request.cpgId;
                updatePlanPago.ccId = requestDetallePagoComprobante.Cc_Id ?? 0;
                updatePlanPago.mcoCont = 0;
                updatePlanPago.mcoNume = 0;

                var respuesta = await _carteraRepository.UpdatePlanPago(updatePlanPago);

                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoComprobante);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoComprobante);

                if (numeroComprobante.IsSuccess)
                {
                    try
                    {
                        await _carteraRepository.ActualizarComprobanteGarantiaCartera((int)requestDetallePagoComprobante.Cc_Id,(int)comprobanteGarantiaId);
                    }
                    catch (Exception)
                    {

                    }
                    response.ReturnMessage += "Comprobante pagos ventanilla " + numeroComprobante.Data.Comprobante_Id
                        + " creado correctamente " + comprobante;
                    response.IsSuccess = true;
                    response.Data = numeroComprobante.Data;
                }

            }

            return response;
        }
        private async Task<List<ResponseMov>> ObtenerDeTalleCuota(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, RequestCarteraSeven request, IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras)
        {

            ReporteGeneralGarantias responseGarantia = new ReporteGeneralGarantias();
            ReporteAPNRCAR reporteAPNRCAR = new ReporteAPNRCAR();
            RestitucionInmuebleCarteraDto restitucionRecursos = new RestitucionInmuebleCarteraDto();

            if (request.codigo == "ABONO")
            {
                var pagosAbonoHistorico = await _carteraRepository.ObtenerPagoAbonoHistorico(request.numeroCredito.ToString(), request.tipoCredito);
                if (pagosAbonoHistorico.Count > 0)
                {
                    request.cpgId = pagosAbonoHistorico.FirstOrDefault().CPG_ID;
                }
            }
            var PlanPago = await _carteraRepository.ConsultarPlanPago(request.numeroCredito, request.cpgId);
            var items = new List<ResponseMov>();
            IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContablesDisminucion;
            cuentasContablesDisminucion = cuentasContables;


            var resultado = await _carteraRepository.ObtenerUltimosAjustesGarantiaPorCedula(request.NumeroDocumentoCliente);

            var resultadoGarantia = _mapper.Map<ObtenerGarantiaActualizadaDto>(resultado);

            // ===== FILTRADO DE CUENTAS CONTABLES =====
            if (request.codigo == "PAGO_CUOTA_CREDITO" || request.codigo == "OTROS_PAGOS")
            {
                var excludeFields = new List<string>
                { 
                    "inmuebles", "inmueblesAct", "cxc", "Dev-Recursos", "Categoria A remanentes","CPGG_PAGO_CRE_HOP","CPG_CREDITO_HIP"
                };

                // Excluir componentes que NO tiene plan
                if (PlanPago.PLN_SEGURO_VIDA == 0)
                    excludeFields.Add("CPG_ABONO_SEGURO_VIDA");

                if (PlanPago.PLN_SEGURO_HOGAR == 0)
                    excludeFields.Add("CPG_ABONO_SEGURO_HOGAR");

                if (PlanPago.PLN_INTERESES == 0)
                    excludeFields.Add("CPG_ABONO_INTERES_CORRIENTE");

                if (PlanPago.PLN_CAPITAL == 0)
                {
                    excludeFields.Add("CPG_ABONO_CAPITAL");
                    excludeFields.Add("desembolso");
                }

                cuentasContablesDisminucion = cuentasContables.Where(x => !excludeFields.Contains(x.REF_COLUMN_APLICA_PAGO));
            }
            else if (request.codigo != "DISMINUYE_REMANENTE" && request.codigo != "OPCION_COMPRA"
                && request.codigo != "LIQUIDACION_LEASING" && request.codigo != "CANCELACION_TOTAL"
                && request.codigo != "RESTITUCION_INMUEBLE" && request.codigo != "ACELERACION" && request.codigo != "REESTRUCTURACION")
            {
                cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "desembolso");
            }
            else if (request.codigo == "CANCELACION_TOTAL")
            {
                cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                || x.REF_COLUMN_APLICA_PAGO == "inmuebles" || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO" || x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes");

            }
            else if (request.codigo == "REESTRUCTURACION")
            {
                cuentasContablesDisminucion = cuentasContables.Where(
                 x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL"
                || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                || x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes");
                var requestObtenerProductClient = new SolicitudProductoPorClienteDto
                {
                    Identificacion = request.NumeroDocumentoCliente,
                    TipoIdentificacion = 1
                };
                var dataProduct = await ObtenerProductoCliente(requestObtenerProductClient);
                var valorInteres = dataProduct.Data.Select(x => x.InteresesRemanenteFinal).FirstOrDefault();
            }
            else if (request.codigo == "ACELERACION")
            {
                cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL"
                || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                || x.REF_COLUMN_APLICA_PAGO == "inmuebles"
                || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes");
                if (request.codigo == "ACELERACION")
                {
                    if (PlanPago.PLN_CAPITAL > PlanPago.PLN_A_PAGAR)
                    {
                        PlanPago.PLN_CAPITAL = PlanPago.PLN_A_PAGAR;

                    }
                    var dataProductClient = await _carteraRepository.ObtenerNovedadesCartera(PlanPago.CRE_ID);
                    PlanPago.PLN_CAPITAL = dataProductClient.Where(x => x.TNV_ID == 1).FirstOrDefault().NVC_CAPITAL;
                    PlanPago.PLN_INTERESES = dataProductClient.Where(x => x.TNV_ID == 1).FirstOrDefault().NVC_INTERES_REMANENTE;
                    PlanPago.PLN_A_PAGAR = dataProductClient.Where(x => x.TNV_ID == 1).FirstOrDefault().NVC_TOTAL_DEUDA;
                }

            }
            else if (request.codigo == "RESTITUCION_INMUEBLE")
            {
                cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                || x.REF_COLUMN_APLICA_PAGO == "inmuebles" || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO" || x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes"
                || x.REF_COLUMN_APLICA_PAGO == "Dev-Recursos");

            }
            else if (request.codigo == "OPCION_COMPRA")
            {
                cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                || x.REF_COLUMN_APLICA_PAGO == "inmuebles" || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct");

            }
            else if (request.codigo == "LIQUIDACION_LEASING")
            {
                cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                || x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes");

            }
            else
            {
                if (PlanPago.CPG_ABONO_INTERES_REMANENTE > 0 && PlanPago.CPG_ABONO_CAPITAL_REMANENTE > 0)
                {
                    cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                        || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE"
                        || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                        || ((request.codigo == "CANCELACION_TOTAL" || request.codigo == "OPCION_COMPRA") &&
                        (x.REF_COLUMN_APLICA_PAGO == "inmuebles" || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")));
                }
                else if (PlanPago.CPG_ABONO_INTERES_REMANENTE > 0 && PlanPago.CPG_ABONO_CAPITAL_REMANENTE <= 0)
                {
                    cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                        || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE" || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                        || ((request.codigo == "CANCELACION_TOTAL" || request.codigo == "OPCION_COMPRA") &&
                        (x.REF_COLUMN_APLICA_PAGO == "inmuebles" || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")));
                }
                else
                {
                    cuentasContablesDisminucion = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"
                        || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "desembolso"
                        || ((request.codigo == "CANCELACION_TOTAL" || request.codigo == "OPCION_COMPRA") &&
                        (x.REF_COLUMN_APLICA_PAGO == "inmuebles" || x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")));
                }
            }

            decimal capitalTotal;

            if (request.codigo == "CANCELACION_TOTAL" || request.codigo == "OPCION_COMPRA" || request.codigo == "RESTITUCION_INMUEBLE" || request.codigo == "ACELERACION")
            {

                var fechaAjustada = request.FechaPago.AddMonths(-1);
                ReporteAPNRCARParametersDto requestAPNRCAR = new ReporteAPNRCARParametersDto
                {
                    Identificacion = (request.NumeroDocumentoCliente),
                    Mes = fechaAjustada.Month,
                    Anio = fechaAjustada.Year,
                    TipoCredito = request.tipoCredito
                };

                reporteAPNRCAR = (await this._reportesRepository.GenerarReporteAPNRCARPorAfiliado(requestAPNRCAR)).FirstOrDefault();

                capitalTotal = 0;





                if (request.codigo == "CANCELACION_TOTAL")
                {
                    capitalTotal = PlanPago.CPG_ABONO_CAPITAL_REMANENTE + PlanPago.PLN_CAPITAL;

                }
                else if (request.codigo == "RESTITUCION_INMUEBLE")
                {
                    capitalTotal = PlanPago.CPG_ABONO_CAPITAL_REMANENTE + PlanPago.PLN_CAPITAL;
                    var respuestaRestitucion = await ObtenerRestitucionInmuebleCartera(reporteAPNRCAR.CRE_ID);

                    if (respuestaRestitucion != null && respuestaRestitucion.Data != null)
                    {
                        restitucionRecursos = respuestaRestitucion.Data.FirstOrDefault();
                    }
                    else
                    {
                        restitucionRecursos = new RestitucionInmuebleCarteraDto();
                    }

                }
                else if (request.codigo == "OPCION_COMPRA")
                {
                    capitalTotal = PlanPago.PLN_A_PAGAR;
                }
                else if (request.codigo == "ACELERACION")
                {
                    capitalTotal = PlanPago.PLN_CAPITAL;
                }


            }
            else
            {
                capitalTotal = PlanPago.CPG_ABONO_CAPITAL_REMANENTE + PlanPago.PLN_CAPITAL;
            }

            var interesesTotales = PlanPago.CPG_ABONO_INTERES_REMANENTE + PlanPago.PLN_INTERESES;
            if (request.codigo == "PAGO_CUOTA_CREDITO" && PlanPago.CPG_ABONO_INTERES_MORA > 0)
            {
                PlanPago.PLN_A_PAGAR = PlanPago.PLN_A_PAGAR - PlanPago.CPG_ABONO_INTERES_MORA;
            }

            Dictionary<string, decimal> mapeoPagos;

            if (request.codigo == "PAGO_CUOTA_CREDITO" || request.codigo == "OTROS_PAGOS")
            {
                // Usar valores reales pagados
                mapeoPagos = new Dictionary<string, decimal>
                    {
            { "CPG_ABONO_CAPITAL", PlanPago.PLN_CAPITAL },
            { "CPG_ABONO_INTERES_CORRIENTE", PlanPago.PLN_INTERESES },
            { "CPG_ABONO_SEGURO_HOGAR", PlanPago.PLN_SEGURO_HOGAR },
            { "CPG_ABONO_SEGURO_VIDA", PlanPago.PLN_SEGURO_VIDA },
            { "CPG_VALOR_PAGO", PlanPago.PLN_CAPITAL + PlanPago.PLN_INTERESES + PlanPago.PLN_SEGURO_HOGAR + PlanPago.PLN_SEGURO_VIDA },
            { "desembolso", PlanPago.PLN_CAPITAL },
            { "inmuebles", resultadoGarantia.ValorAjusteAnterior},
            { "inmueblesAct", resultadoGarantia.ValorAjuste },
            { "cxc", 0 },
            { "Categoria A remanentes", PlanPago.CPG_ABONO_INTERES_REMANENTE },
            { "Dev-Recursos", restitucionRecursos.ValorRestante }
       };
            }
            else
            {
                // Usar valores del plan
                mapeoPagos = new Dictionary<string, decimal>
        {
            { "CPG_ABONO_CAPITAL", capitalTotal },
            { "CPG_ABONO_INTERES_CORRIENTE", interesesTotales },
            { "CPG_ABONO_SEGURO_HOGAR", PlanPago.PLN_SEGURO_HOGAR },
            { "CPG_ABONO_SEGURO_VIDA", PlanPago.PLN_SEGURO_VIDA },
            { "CPG_VALOR_PAGO", PlanPago.PLN_A_PAGAR },
            { "desembolso", capitalTotal },
            { "inmuebles", resultadoGarantia.ValorAjusteAnterior},
            { "inmueblesAct", resultadoGarantia.ValorAjuste },
            { "cxc", 0 },
            { "Categoria A remanentes", PlanPago.CPG_ABONO_INTERES_REMANENTE },
            { "Dev-Recursos", restitucionRecursos.ValorRestante }
                };
            }

            var NombreTercero = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(new List<string> { request.NumeroDocumentoCliente }));  //.Distinct().ToList())).Data;

            foreach (var item in cuentasContablesDisminucion)
            {
                //var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();
                var itemDetalle = new ResponseMov();
                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = $"cliente: {(NombreTercero?.Data != null && NombreTercero.Data.Count > 0 && NombreTercero.Data[0] != null ? NombreTercero.Data[0].NombreCompleto : "Desconocido")}";
                //itemDetalle.Dmc_desc = $"cliente: {item.DESCRIPCION_CUENTA} Tipo: {(item.TIPO_OPERACION == "D" ? "Debito" : "Credito")}";

                itemDetalle.Dmc_cant = request.nCuota;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = request.numeroCredito.ToString();
                itemDetalle.Dmc_vadb = 0;
                itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = request.NumeroDocumentoCliente;
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = $"{unidadesEjecutoras.FirstOrDefault(ue => ue.Id == NombreTercero.Data[0].UnidadEjecutora).AreaNegocio}";
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";
                itemDetalle.Cpg_Id = request.cpgId;


                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }
            //  AGREGADO: Incluir 2 movimientos adicionales de MORA si aplica
            if ((request.codigo == "PAGO_CUOTA_CREDITO" || request.codigo == "OTROS_PAGOS") && PlanPago.CPG_ABONO_INTERES_MORA > 0)
            {
                // Obtener las cuentas de mora
                var cuentaMoraDebito = cuentasContables.FirstOrDefault(x => x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO");
                var cuentaMoraCredito = cuentasContables.FirstOrDefault(x => x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE");

                if (cuentaMoraDebito != null)
                {
                    // Movimiento 1: DÉBITO con CPG_VALOR_PAGO
                    var movimientoMoraDebito = new ResponseMov
                    {
                        Cue_codi = cuentaMoraDebito.CUENTA_CONTABLE_SEVEN,
                        Dmc_desc = $"cliente: {(NombreTercero?.Data != null && NombreTercero.Data.Count > 0 && NombreTercero.Data[0] != null ? NombreTercero.Data[0].NombreCompleto : "Desconocido")}",
                        Dmc_cant = request.nCuota,
                        Dmc_acti = 0,
                        Dmc_refe = request.numeroCredito.ToString(),
                        Dmc_vadb = PlanPago.CPG_ABONO_INTERES_MORA,  // Débito = CPG_ABONO_INTERES_MORA
                        Dmc_vacr = 0,
                        Dmc_vaba = 0,
                        Ter_coda = request.NumeroDocumentoCliente,
                        Arb_codc = cuentaMoraDebito.TCR_ID == 1 ? "601102" : "601103",
                        Arb_coda = $"{unidadesEjecutoras.FirstOrDefault(ue => ue.Id == NombreTercero.Data[0].UnidadEjecutora).AreaNegocio}",
                        Arb_codp = "099",
                        Arb_cods = "1001",
                        Ter_codm = "0",
                        Cpg_Id = request.cpgId
                    };
                    items.Add(movimientoMoraDebito);
                }

                if (cuentaMoraCredito != null)
                {
                    // Movimiento 2: CRÉDITO con CPG_ABONO_INTERES_CORRIENTE
                    var movimientoMoraCredito = new ResponseMov
                    {
                        Cue_codi = cuentaMoraCredito.CUENTA_CONTABLE_SEVEN,
                        Dmc_desc = $"cliente: {(NombreTercero?.Data != null && NombreTercero.Data.Count > 0 && NombreTercero.Data[0] != null ? NombreTercero.Data[0].NombreCompleto : "Desconocido")}",
                        Dmc_cant = request.nCuota,
                        Dmc_acti = 0,
                        Dmc_refe = request.numeroCredito.ToString(),
                        Dmc_vadb = 0,
                        Dmc_vacr = PlanPago.CPG_ABONO_INTERES_MORA,  // Crédito = CPG_ABONO_INTERES_MORA
                        Dmc_vaba = 0,
                        Ter_coda = request.NumeroDocumentoCliente,
                        Arb_codc = cuentaMoraCredito.TCR_ID == 1 ? "601102" : "601103",
                        Arb_coda = $"{unidadesEjecutoras.FirstOrDefault(ue => ue.Id == NombreTercero.Data[0].UnidadEjecutora).AreaNegocio}",
                        Arb_codp = "099",
                        Arb_cods = "1001",
                        Ter_codm = "0",
                        Cpg_Id = request.cpgId
                    };
                    items.Add(movimientoMoraCredito);
                }
            }
            return items;
        }
        public async Task<Response<InsertarMovContableResponse>> AdelantoCuotaCartera(RequestCarteraAdelantoCuotaSeven request, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            var PlanPagoAdelantoCuota = await _carteraRepository.ConsultarPlanPagoAdelantoCuota(request.numeroCredito, request.cpgId);

            if (PlanPagoAdelantoCuota.Count() > 0)
            {

                InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
                var dataCabeceraMovConta = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();

                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovConta = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                if (request.tipoCredito == 1)
                {
                    var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);

                    dataCabeceraMovConta.Emp_codi = 294;
                    dataCabeceraMovConta.Top_codi = 9065;
                    dataCabeceraMovConta.Mco_nume = 0;
                    dataCabeceraMovConta.Mco_fech = request.fechaCierre;
                    dataCabeceraMovConta.Mod_codi = 7;
                    dataCabeceraMovConta.Arb_csuc = "1001";
                    dataCabeceraMovConta.Mco_desc = "Adelantar cuotas Leasing" + request.fechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");


                    items = ObtenerDeTalleAdelantoCuota(equivalenciaCuentasLeasing, request, PlanPagoAdelantoCuota).Result;

                    detalleMovConta.AddRange(items);

                    dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();

                    var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                    movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                }
                else if (request.tipoCredito == 2)
                {

                    var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);
                    dataCabeceraMovConta.Emp_codi = 294;
                    dataCabeceraMovConta.Top_codi = 9003;
                    dataCabeceraMovConta.Mco_nume = 0;
                    dataCabeceraMovConta.Mco_fech = request.fechaCierre;
                    dataCabeceraMovConta.Mod_codi = 7;
                    dataCabeceraMovConta.Arb_csuc = "1001";
                    dataCabeceraMovConta.Mco_desc = "Adelantar cuotas Hipotecario";

                    items = ObtenerDeTalleAdelantoCuota(equivalenciaCuentasHipotecario, request, PlanPagoAdelantoCuota).Result;

                    detalleMovConta.AddRange(items);

                    dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();

                    var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                    movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                }

                response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado creando el SCnMcont en Seven.";

                if (response.Data != null)
                {
                    RequestDetallePago requestDetallePago = new RequestDetallePago();
                    foreach (var i in items)
                    {
                        requestDetallePago.Cpg_id = PlanPagoAdelantoCuota.FirstOrDefault().CRE_ID;
                        requestDetallePago.Emp_codi = dataCabeceraMovConta.Emp_codi;
                        requestDetallePago.Top_codi = dataCabeceraMovConta.Top_codi;
                        requestDetallePago.Mco_fech = dataCabeceraMovConta.Mco_fech;
                        requestDetallePago.Mod_codi = dataCabeceraMovConta.Mod_codi;
                        requestDetallePago.Mco_desc = dataCabeceraMovConta.Mco_desc;
                        requestDetallePago.Cue_codi = i.Cue_codi;
                        requestDetallePago.Dmc_desc = i.Dmc_desc;
                        requestDetallePago.Dmc_cant = i.Dmc_cant;
                        requestDetallePago.Dmc_refe = i.Dmc_refe;
                        requestDetallePago.Dmc_vadb = i.Dmc_vadb;
                        requestDetallePago.Dmc_vacr = i.Dmc_vacr;
                        requestDetallePago.Ter_coda = i.Ter_coda;
                        requestDetallePago.Arb_codc = i.Arb_codc;
                        requestDetallePago.Arb_coda = i.Arb_coda;
                        requestDetallePago.Arb_codp = i.Arb_codp;
                        requestDetallePago.Arb_cods = i.Arb_cods;
                        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
                        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                    }
                    RequestDetallePago requestDetallePagoComprobante = new RequestDetallePago();
                    requestDetallePagoComprobante.Emp_codi = dataCabeceraMovConta.Emp_codi;
                    requestDetallePagoComprobante.Top_codi = dataCabeceraMovConta.Top_codi;
                    requestDetallePagoComprobante.Mco_fech = dataCabeceraMovConta.Mco_fech;
                    requestDetallePagoComprobante.Mod_codi = dataCabeceraMovConta.Mod_codi;
                    requestDetallePagoComprobante.Mco_desc = dataCabeceraMovConta.Mco_desc;
                    if (detalleMovConta.Count > 0)
                    {
                        string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovConta);
                        requestDetallePagoComprobante.Mov_Contable_Seven = jsonMovimientosContables;
                        requestDetallePagoComprobante.Respuesta_Seven = response.ReturnMessage;
                    }
                    requestDetallePagoComprobante.Fecha_Ejecucion = DateTime.Now;
                    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoComprobante);

                    UpdatePlanPagoAdelantoCuota updatePlanPago = new UpdatePlanPagoAdelantoCuota();
                    int MCO_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarMovContableResult, 0);
                    int MCO_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarMovContableResult, 1);

                    updatePlanPago.numeroCredito = request.numeroCredito;
                    updatePlanPago.mcoCont = MCO_CONT;
                    updatePlanPago.mcoNume = MCO_NUME;
                    var respuesta = await _carteraRepository.UpdatePlanPagoAdelantoCuota(updatePlanPago);
                }
            }
            else
            {
                response.ReturnMessage = "El credito no tiene adelantos de cuota";
            }

            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleAdelantoCuota(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, RequestCarteraAdelantoCuotaSeven request, IEnumerable<PlanPagoAbonoCuota> PlanPagoAdelantoCuota)
        {
            IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContablesDisminucion;
            cuentasContablesDisminucion = cuentasContables;
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            foreach (var PlanPago in PlanPagoAdelantoCuota)
            {
                if (PlanPago.PLN_SEGURO_VIDA == 0)
                {
                    cuentasContablesDisminucion = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA"));
                }
                Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                {
                    { "CPG_ABONO_INTERES_CORRIENTE", PlanPago.PLN_INTERESES },
                    { "CPG_ABONO_SEGURO_HOGAR", PlanPago.PLN_SEGURO_HOGAR },
                    { "CPG_ABONO_SEGURO_VIDA", PlanPago.PLN_SEGURO_VIDA },
                    { "CPG_ABONO_CAPITAL", PlanPago.PLN_CAPITAL },
                    { "CPG_VALOR_PAGO", PlanPago.PLN_A_PAGAR },
                    { "desembolso", PlanPago.PLN_CAPITAL }
                };
                foreach (var item in cuentasContablesDisminucion)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: {request.NumeroDocumentoCliente}";
                    itemDetalle.Dmc_cant = PlanPagoAdelantoCuota.Count();
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = request.numeroCredito.ToString();
                    itemDetalle.Dmc_vadb = 0;
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = request.NumeroDocumentoCliente;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }

                }


            }


            return items;
        }
        public async Task<Response<List<ComprobanteCarteraPorProcesoDTO>>> AnulacionCuotaCartera(RequestAnulacionCarteraSeven request, CancellationToken cancellationToken)
        {
            Response<List<ComprobanteCarteraPorProcesoDTO>> response = new();
            AnularCnMcontRequest movContableRequest = new();
            ComprobanteCarteraUpdate comprobanteCarteraUpdate = new();
            ObtenerComprobantePorProcesoDto requestComprobantes = new();
            requestComprobantes.ComprobanteSevenId = request.Comprobante_Seven_Id;


            var bodyHipotecario = new AnularCnMcontRequestBody(294, request.Consecutivo_Seven_Id);
            movContableRequest = new AnularCnMcontRequest(bodyHipotecario);

            var anulacionResponse = await _wsMovContable.AnularCnMcontAsync(movContableRequest, cancellationToken);
            response.ReturnMessage = anulacionResponse != null ? anulacionResponse.Body.AnularCnMcontResult : "Error inesperado Anulando el SCnMcont en Seven.";

            if (anulacionResponse != null)
            {
                var matchRetorno = System.Text.RegularExpressions.Regex.Match(response.ReturnMessage, @"<RETORNO>(.*?)<\/RETORNO>");
                bool isSuccessful = matchRetorno.Success && matchRetorno.Groups[1].Value.Trim() == "0";

                List<ComprobanteCarteraPorProcesoDTO> reponseComprobantes = (await _carteraRepository.ObtenerComprobanteCartera(requestComprobantes, 1)).ToList();

                if (isSuccessful)
                {
                    List<ComprobanteCarteraPorProcesoDTO> updatedComprobantes = new();

                    foreach (var comprobante in reponseComprobantes)
                    {
                        comprobanteCarteraUpdate.CC_ID = comprobante.Comprobante_Id;
                        comprobanteCarteraUpdate.TIPO_OPERACION = comprobante.Tipo_Operacion;
                        comprobanteCarteraUpdate.ESTADO = "Anulado";

                        var comprobanteDto = await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);

                        if (comprobanteDto != null)
                        {
                            updatedComprobantes.Add(comprobanteDto);
                        }
                    }

                    response.Data = updatedComprobantes;
                    response.IsSuccess = true;
                    response.ReturnMessage = response.ReturnMessage;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "La operación no fue exitosa. " + response.ReturnMessage;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La respuesta del servicio fue nula.";
            }

            return response;
        }


        public async Task<Response<AnularCnMcontResponse>> AnulacionLista(int papId, CancellationToken cancellationToken)
        {
            Response<AnularCnMcontResponse> response = new();
            AnularCnMcontRequest movContableRequest = new AnularCnMcontRequest();
            var mco_cont = await _carteraRepository.ConsultarPapAplicacionPagosMasivosSeven(papId);
            var bodyHipotecario = new AnularCnMcontRequestBody(294, mco_cont);
            movContableRequest = new AnularCnMcontRequest(bodyHipotecario);


            response.Data = await _wsMovContable.AnularCnMcontAsync(movContableRequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.AnularCnMcontResult : "Error inesperado Anulando el SCnMcont en Seven.";
            if (response.Data != null)
            {
                await _carteraRepository.UpdatePapAplicacionPagosMasivos(papId, 0, 0);
            }

            return response;
        }
        public async Task<Response<InsertarMovContableResponse>> CierreMensualCarteraSeven(ParamCierreMensualCarteraSevenDto param, CancellationToken cancellationToken)
        {
            ParamCierreMensualCarteraDto request = new ParamCierreMensualCarteraDto();

            DateTime primerDiaDelSiguienteMes = new DateTime(param.AnioCierre, param.MesCierre, 1).AddMonths(1);
            DateTime ultimoDiaDelMes = primerDiaDelSiguienteMes.AddDays(-1);
            DateTime fechaCierre = new DateTime(ultimoDiaDelMes.Year, ultimoDiaDelMes.Month, ultimoDiaDelMes.Day);

            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            request.AnioCierre = param.AnioCierre;
            request.MesCierre = param.MesCierre;
            request.Actualiza = false;

            Response<InsertarMovContableResponse> response = new();
            List<ReporteCierreMensualCarteraDto> list = new List<ReporteCierreMensualCarteraDto>();
            var paramCierreMensualCartera = this._mapper.Map<ParamCierreMensualCartera>(request);
            var responseCierreMensual = await CierreMensualCartera(request);

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responseCierreMensual.Data.Select(x => x.Identificacion).Distinct().ToList())).Data;


            list = responseCierreMensual.Data.GroupJoin(listAfiliados, r => r.Identificacion, la => la.Documento, (r, la) =>
            {
                r.Nombres = la.FirstOrDefault()?.NombreCompleto ?? "";

                var unidadId = la.FirstOrDefault()?.UnidadEjecutora ?? 0;
                var unidadEjecutoraNombre = unidadesEjecutoras.FirstOrDefault(ue => ue.Id == unidadId)?.AreaNegocio ?? 0;
                r.UnidadEjecutoraID = unidadEjecutoraNombre;
                return r;
            }).ToList();

            var respuestaGarantias = await GarantiasCarteraSeven(param, responseCierreMensual.Data, fechaCierre, cancellationToken);
            string mensajeGarantias = $"Garantias Cartera: \n{respuestaGarantias.ReturnMessage}";


            var respuestaPrevision = await PrevisionCarteraSeven(param, responseCierreMensual.Data, fechaCierre, cancellationToken);
            string mensajePrevisionIndividual = $"Provisiones Individual Cartera: \n{respuestaPrevision.ReturnMessage}";


            var respuestaPrevisionGeneral = await PrevisionGeneralCarteraSeven(param, responseCierreMensual.Data, fechaCierre, cancellationToken);
            string mensajePrevision = $"Provisiones General Cartera: \n{respuestaPrevisionGeneral.ReturnMessage}";


            //var respuestaAmortizacion = await AmortizacionCarteraSeven(param, responseCierreMensual, fechaCierre, cancellationToken);
            ////response.ReturnMessage += "Amortizacion Cartera " + respuestaAmortizacion.ReturnMessage;
            //string mensajeAmortizacion = "Amortización Cartera: " + respuestaAmortizacion.ReturnMessage;


            //DateTime primerDiaMes = fechaCierre.AddDays(-fechaCierre.Day + 1);
            //var desembolsos = await this._carteraRepository.ObtenerDesembolsoSevenCierreMensual(fechaCierre);
            //var cancelacionTotal = await this._carteraRepository.ObtenerCancelacionTotalCierreMensual(primerDiaMes, fechaCierre);

            //foreach (var item in desembolsos)
            //{

            //    ReporteCierreMensualCarteraDto cierre = new ReporteCierreMensualCarteraDto();
            //    cierre.SaldoCapital = item.SaldoCapital;
            //    cierre.NumeroCredito = item.NumeroCredito;
            //    cierre.Identificacion = item.Identificacion;
            //    cierre.FechaDesembolso = item.FechaDesembolso;
            //    cierre.TipoCredito = item.TipoCredito;
            //    responseCierreMensual.Add(cierre);

            //}
            //foreach (var item in cancelacionTotal)
            //{

            //    ReporteCierreMensualCarteraDto cierre = new ReporteCierreMensualCarteraDto();
            //    cierre.SaldoCapital = (item.SaldoCapital + item.SaldoCapitalRemanente);
            //    cierre.NumeroCredito = item.NumeroCredito;
            //    cierre.Identificacion = item.Identificacion;
            //    cierre.FechaDesembolso = item.FechaDesembolso;
            //    cierre.TipoCredito = item.TipoCredito;
            //    responseCierreMensual.Add(cierre);

            //}
            //var respuestaCuentaOrden = await CuentaDeOrdenCarteraSeven(param, responseCierreMensual, fechaCierre, cancellationToken);
            //string mensajeCuentaOrden = "Cuenta de orden Cartera: " + respuestaCuentaOrden.ReturnMessage;

            response.ReturnMessage = "Comprobantes Cierre Mensual :\n"
                + $"- {mensajePrevision}\n"
                + $"- {mensajeGarantias}\n"
                + $"- {mensajePrevisionIndividual}\n"
                ;

            return response;
        }
        public async Task<Response<InsertarMovContableResponse>> PrevisionCarteraSeven(ParamCierreMensualCarteraSevenDto param, IEnumerable<ReporteCierreMensualCarteraDto> responseCierreMensual, DateTime fechaCierre, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();

            var catalogoValores = await this._catalogosBusinessLogic.ObtenerTipoCredito();
            var descripcionLeasing = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
            var descripcionHipotecario = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;

            var Leasing = responseCierreMensual.Where(x => x.TipoCredito == descripcionLeasing);
            var creditosHipotecarios = responseCierreMensual.Where(x => x.TipoCredito == descripcionHipotecario);
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (Leasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9072);

                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9072;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = fechaCierre;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Cierre de provisiones Leasing";
                foreach (var item in Leasing)
                {
                    items = ObtenerDeTallePrevisionMensualSeven(item, equivalenciaCuentasLeasing, fechaCierre).Result;
                    detalleMovContaLeasing.AddRange(items);
                }
                dataCabeceraMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaLeasing);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);


                /*se suspende consumo a seven solo se guarda en el core*/
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado creando el SCnMcont en Seven.";
                //respuestaSevenLeasing = response.Data.Body.InsertarMovContableResult;
            }
            if (creditosHipotecarios.Count() > 0)
            {
                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9004);

                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9004;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = fechaCierre;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Cierre de provisiones Hipotecario";
                foreach (var item in creditosHipotecarios)
                {

                    try
                    {
                        items = ObtenerDeTallePrevisionMensualSeven(item, equivalenciaCuentasHipotecario, fechaCierre).Result;
                        detalleMovContaHipotecario.AddRange(items);
                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                }
                dataCabeceraMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaHipotecario);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                /*se suspende consumo a seven solo se guarda en el core*/
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage += response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                //           "para créditos Hipotecarios";
                //respuestaSevenHipotecario = response.Data.Body.InsertarMovContableResult;
            }
            //int respuestaLeasing = GetValueResponseXmlSevenFactura(respuestaSevenLeasing, 2);
            //int respuestaHipotecario = GetValueResponseXmlSevenFactura(respuestaSevenHipotecario, 2);


            if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CPC", totalDmcVacr, totalDmcCadb);
                //var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;

                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                response.ReturnMessage += GenerarMensaje("leasing", "provisiones", requestDetallePago.Cc_Id);
                //response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable de privisiones leasing: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos Leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";


            }
            if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CPC", totalDmcVacr, totalDmcCadb);
                //var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                response.ReturnMessage += GenerarMensaje("hipotecario", "provisiones", requestDetallePago.Cc_Id);
                //response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable de privisiones Hipotecario: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";

            }



            //if (respuestaLeasing == 0 || respuestaHipotecario == 0)
            //{

            //    if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            //    {
            //        RequestDetallePago requestDetallePago = new RequestDetallePago();
            //        string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
            //        requestDetallePago.Cpg_id = new Guid();
            //        requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
            //        requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
            //        requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
            //        requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
            //        requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
            //        requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
            //        requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
            //        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
            //        var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
            //        requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;
            //        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            //        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
            //        response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable cierre mensual  hipotecario: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";


            //    }
            //    if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            //    {
            //        RequestDetallePago requestDetallePago = new RequestDetallePago();
            //        string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
            //        requestDetallePago.Cpg_id = new Guid();
            //        requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
            //        requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
            //        requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
            //        requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
            //        requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
            //        requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
            //        requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
            //        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
            //        var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
            //        requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;
            //        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            //        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
            //        response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable cierre mensual  hipotecario: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";

            //    }


            //}
            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTallePrevisionMensualSeven(ReporteCierreMensualCarteraDto responseCierreMensual, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, DateTime fechaCierre)
        {

            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            decimal totalProvision = 0;
            decimal totalProvisionI = 0;
            decimal totalProvisionR = 0;
            decimal totalProvisionAbono = 0;

            if (responseCierreMensual.EstadoCredito == "AL DIA" || responseCierreMensual.EstadoCredito == "EN MORA")
            {
                var obtenerCredito = await _carteraRepository.ObtenerProductoCliente(1, responseCierreMensual.Identificacion, null);
                var credito = obtenerCredito.Where(x => x.CRE_NRO_CREDITO == responseCierreMensual.NumeroCredito).FirstOrDefault();
                var detallesPagos = await _carteraRepository.ObtenerAplicacionPago(credito.CRE_ID);
                var aplicacionPago = detallesPagos.Where(x => x.CPG_FECHA_PAGO == fechaCierre && (x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN"
                || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA")).ToList();

                var cuentasAfectar = cuentasContables;

                if (responseCierreMensual.TipoCredito == "LEASING HABITACIONAL")
                {

                    if (responseCierreMensual.FechaDesembolso.Month == fechaCierre.Month &&
                        responseCierreMensual.FechaDesembolso.Year == fechaCierre.Year)
                    {
                        cuentasAfectar = cuentasContables
                            .Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual Total")
                            .ToList();

                        totalProvision = responseCierreMensual.Provision;

                    }
                    else if (responseCierreMensual.EstadoCredito == "EN MORA")
                    {
                        var fechaAjustada = fechaCierre.AddMonths(-1);
                        CarteraReporteGeneral reporteGeneralCartera = new CarteraReporteGeneral();
                        ReporteGeneralCarteraParametersDto requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                        {
                            PeriodoActual = false,
                            Mes = fechaAjustada.Month,
                            Anio = fechaAjustada.Year,
                            NumeroCredito = responseCierreMensual.NumeroCredito,
                            TipoCredito = 1
                        };
                        reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();
                        var saldoCapital = 0m;

                        totalProvisionR = Math.Abs(reporteGeneralCartera.CRE_VALOR_DEUDA_REMANENTE - responseCierreMensual.ValorDeudaRemanente) * 0.01M;

                        if (reporteGeneralCartera.MORA_ANTERIOR == 0)
                        {
                            requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                            {
                                PeriodoActual = false,
                                Mes = fechaAjustada.Month + 1,
                                Anio = fechaAjustada.Year,
                                NumeroCredito = responseCierreMensual.NumeroCredito,
                                TipoCredito = 1
                            };
                            var nowreporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();
                            if (nowreporteGeneralCartera == null)
                            {
                                totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.PLN_INTERESES) * 0.01M;
                            }
                            else
                            {
                                var requetsPlanPago = new ObtenerProductoCartera { Nro_Credito = responseCierreMensual.NumeroCredito, Tcr_Id = 1 };
                                var planPagos = await ObtenerPlanPagosCredito(requetsPlanPago);

                                if (nowreporteGeneralCartera.MORA_ANTERIOR != 0)
                                {
                                    reporteGeneralCartera = nowreporteGeneralCartera;
                                    var dateCierePlanPago = planPagos.Data.PlanPago.Where(x => x.Fecha <= fechaCierre && x.EstadoCuota == "PAGADA")
                                        .OrderByDescending(x => x.Fecha).Take(2)
                                        .OrderBy(x => x.Fecha).ToList();

                                    saldoCapital = dateCierePlanPago.First().SaldoCapital;
                                    if (dateCierePlanPago.Count == 1)
                                    {
                                        var capital = dateCierePlanPago.Select(x => x.CapitalReducido).First();
                                        saldoCapital = capital;
                                    }
                                    else
                                    {

                                        foreach (var item in dateCierePlanPago.Skip(1))
                                        {
                                            saldoCapital -= item.SaldoCapital;
                                        }

                                    }


                                    totalProvision = (saldoCapital * 0.01M);
                                    totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.MORA_ANTERIOR) * 0.01M;
                                }
                                else
                                {

                                    var dateCierePlanPago = planPagos.Data.PlanPago.Where(x => x.Fecha <= fechaCierre)
                                        .OrderByDescending(x => x.Fecha).Take(3).OrderBy(x => x.Fecha).ToList();
                                    var primero = dateCierePlanPago.First();
                                    var ultimo = dateCierePlanPago.Last();
                                    saldoCapital = primero.SaldoCapital;
                                    if (reporteGeneralCartera.ESC_DESCRIPCION == "AL DIA" && nowreporteGeneralCartera.CRE_SALDO_CAPITAL == reporteGeneralCartera.CRE_SALDO_CAPITAL)
                                    {
                                        totalProvision = 0;
                                    }
                                    else
                                    {
                                        totalProvision = (saldoCapital * 0.01M);
                                    }
                                    reporteGeneralCartera = nowreporteGeneralCartera;


                                    totalProvisionI = Math.Abs(reporteGeneralCartera.PLN_INTERESES) * 0.01M;
                                }
                            }


                            // totalProvisionI = (responseCierreMensual.InteresesCorrientesSeven - reporteGeneralCartera.MORA_ANTERIOR) * 0.01M;
                            //var variacioSaldo = (reporteGeneralCartera.CRE_SALDO_CAPITAL - responseCierreMensual.SaldoCapital);

                            //totalProvision = (responseCierreMensual.CapitalSeven * 0.01M);
                            cuentasAfectar = cuentasContables
                            .Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora"
                            || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente")
                            && x.CUENTA_CONTABLE_SEVEN != "41601102")
                            .ToList();
                        }
                        else
                        {
                            totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.PLN_INTERESES) * 0.01M;
                            var variacioSaldo = (reporteGeneralCartera.CRE_SALDO_CAPITAL - responseCierreMensual.SaldoCapital);
                            totalProvision = (responseCierreMensual.CapitalSeven * 0.01M);

                            cuentasAfectar = cuentasContables
                            .Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora"
                            || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente")
                            && x.CUENTA_CONTABLE_SEVEN != "41601102")
                            .ToList();
                        }
                    }
                    else
                    {

                        var fechaAjustada = fechaCierre.AddMonths(-1);

                        CarteraReporteGeneral reporteGeneralCartera = new CarteraReporteGeneral();
                        ReporteGeneralCarteraParametersDto requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                        {
                            PeriodoActual = false,
                            Mes = fechaAjustada.Month,
                            Anio = fechaAjustada.Year,
                            NumeroCredito = responseCierreMensual.NumeroCredito,
                            TipoCredito = 1
                        };
                        reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();

                        totalProvisionR = Math.Abs(reporteGeneralCartera.CRE_VALOR_DEUDA_REMANENTE - responseCierreMensual.ValorDeudaRemanente) * 0.01M;

                        var requestProductCliente = new SolicitudProductoPorClienteDto
                        {
                            Identificacion = responseCierreMensual.Identificacion,
                            TipoIdentificacion = 1,
                        };

                        var dataProductClient = await ObtenerProductoCliente(requestProductCliente);
                        var saldoAlCorte = dataProductClient.Data.FirstOrDefault().SaldoCapital;
                        var requetsPlanPago = new ObtenerProductoCartera { Nro_Credito = responseCierreMensual.NumeroCredito, Tcr_Id = 1 };
                        var planPagos = await ObtenerPlanPagosCredito(requetsPlanPago);

                        var saldoCapital = 0m;

                        var variacioSaldo = (saldoAlCorte - reporteGeneralCartera.CRE_SALDO_CAPITAL);
                        var porcentaje = reporteGeneralCartera.CRE_CALIFICACION == "A" ? 0.01M : 0.1M;
                        totalProvision = (saldoCapital * porcentaje);

                        requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                        {
                            PeriodoActual = false,
                            Mes = fechaAjustada.Month + 1,
                            Anio = fechaAjustada.Year,
                            NumeroCredito = responseCierreMensual.NumeroCredito,
                            TipoCredito = 1
                        };
                        var resultReporteGeneral = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();
                        if (resultReporteGeneral != null)
                        {
                            if (resultReporteGeneral.MORA_ANTERIOR > 0)
                            {
                                totalProvisionI = resultReporteGeneral.MORA_ANTERIOR * 0.01M;
                                if (totalProvisionI > 0)
                                {
                                    var dateCierePlanPago = planPagos.Data.PlanPago.Where(x => x.Fecha <= fechaCierre)
                                        .OrderByDescending(x => x.Fecha).Take(3)
                                        .OrderBy(x => x.Fecha).ToList();
                                    var primero = dateCierePlanPago.First();
                                    var ultimo = dateCierePlanPago.Last();
                                    saldoCapital = primero.SaldoCapital - ultimo.SaldoCapital;

                                    porcentaje = reporteGeneralCartera.CRE_CALIFICACION == "A" ? 0.01M : 0.1M;
                                    totalProvision = (saldoCapital * porcentaje);
                                    cuentasAfectar = cuentasContables
                                            .Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora")
                                            && x.CUENTA_CONTABLE_SEVEN != "41601102")
                                            .ToList();
                                }
                                else
                                {
                                    var dateCierePlanPago = planPagos.Data.PlanPago.Where(x => x.Fecha <= fechaCierre)
                                        .OrderByDescending(x => x.Fecha).Take(2)
                                        .OrderBy(x => x.Fecha).ToList();
                                    saldoCapital = dateCierePlanPago.First().SaldoCapital;

                                    foreach (var item in dateCierePlanPago.Skip(1))
                                    {
                                        saldoCapital -= item.SaldoCapital;
                                    }
                                    totalProvision = (saldoCapital * 0.01M);
                                    cuentasAfectar = cuentasContables
                                        .Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente")
                                        && x.CUENTA_CONTABLE_SEVEN != "41601102")
                                        .ToList();
                                }
                            }
                            else
                            {
                                var dateCierePlanPago = planPagos.Data.PlanPago.Where(x => x.Fecha <= fechaCierre)
                                    .OrderByDescending(x => x.Fecha).Take(2)
                                    .OrderBy(x => x.Fecha).ToList();
                                saldoCapital = dateCierePlanPago.First().SaldoCapital;

                                if (dateCierePlanPago.Count == 1)
                                {
                                    var capital = dateCierePlanPago.Select(x => x.CapitalReducido).First();
                                    saldoCapital = capital;
                                }
                                else
                                {
                                    foreach (var item in dateCierePlanPago.Skip(1))
                                    {
                                        saldoCapital -= item.SaldoCapital;
                                    }
                                }

                                totalProvision = (saldoCapital * 0.01M);
                                cuentasAfectar = cuentasContables
                                    .Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente")
                                    && x.CUENTA_CONTABLE_SEVEN != "41601102").ToList();
                            }

                        }
                        else
                        {
                            var dateCierePlanPago = planPagos.Data.PlanPago.Where(x => x.Fecha <= fechaCierre)
                                .OrderByDescending(x => x.Fecha).Take(2)
                                .OrderBy(x => x.Fecha).ToList();
                            saldoCapital = dateCierePlanPago.First().SaldoCapital;

                            if (dateCierePlanPago.Count == 1)
                            {
                                var capital = dateCierePlanPago.Select(x => x.CapitalReducido).First();
                                saldoCapital = capital;
                            }
                            else
                            {
                                foreach (var item in dateCierePlanPago.Skip(1))
                                {
                                    saldoCapital -= item.SaldoCapital;
                                }
                            }
                            totalProvision = (saldoCapital * 0.01M);
                            cuentasAfectar = cuentasContables
                                .Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente")
                                && x.CUENTA_CONTABLE_SEVEN != "41601102").ToList();
                        }


                        //cuentasAfectar = cuentasContables
                        //.Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual Remanente")
                        //&& x.CUENTA_CONTABLE_SEVEN != "41601102")
                        //.ToList();

                    }

                }
                else
                {

                    if (responseCierreMensual.FechaDesembolso.Month == fechaCierre.Month &&
                    responseCierreMensual.FechaDesembolso.Year == fechaCierre.Year)
                    {
                        cuentasAfectar = cuentasContables
                            .Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual Total")
                            .ToList();

                        totalProvision = responseCierreMensual.Provision;

                    }

                    else if (responseCierreMensual.EstadoCredito == "EN MORA")
                    {

                        var fechaAjustada = fechaCierre.AddMonths(-1);
                        CarteraReporteGeneral reporteGeneralCartera = new CarteraReporteGeneral();
                        ReporteGeneralCarteraParametersDto requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                        {
                            PeriodoActual = false,
                            Mes = fechaAjustada.Month + 1,
                            Anio = fechaAjustada.Year,
                            NumeroCredito = responseCierreMensual.NumeroCredito,
                            TipoCredito = 2
                        };
                        reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();

                        if ((reporteGeneralCartera?.MORA_ANTERIOR ?? 0) == 0)
                        {


                            totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora) * 0.01M;


                            reporteGeneralCartera = new CarteraReporteGeneral();
                            requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                            {
                                PeriodoActual = false,
                                Mes = fechaAjustada.Month,
                                Anio = fechaAjustada.Year,
                                NumeroCredito = responseCierreMensual.NumeroCredito,
                                TipoCredito = 2
                            };
                            reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();
                            var variacioSaldo = (reporteGeneralCartera.CRE_SALDO_CAPITAL - responseCierreMensual.SaldoCapital);
                            totalProvision = variacioSaldo * 0.01M;//(responseCierreMensual.CapitalSeven * 0.01M);
                            cuentasAfectar = cuentasContables
                            .Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual")
                            .ToList();

                        }
                        else
                        {
                            totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.PLN_INTERESES) * 0.01M;
                            requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                            {
                                PeriodoActual = false,
                                Mes = fechaAjustada.Month,
                                Anio = fechaAjustada.Year,
                                NumeroCredito = responseCierreMensual.NumeroCredito,
                                TipoCredito = 2
                            };
                            reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();

                            totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.PLN_INTERESES) * 0.01M;
                            var variacioSaldo = (reporteGeneralCartera.CRE_SALDO_CAPITAL - responseCierreMensual.SaldoCapital);
                            totalProvision = (responseCierreMensual.CapitalSeven * 0.01M);




                            cuentasAfectar = cuentasContables
                            .Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual")
                            .ToList();
                        }


                    }
                    else
                    {
                        var fechaAjustada = fechaCierre.AddMonths(-1);
                        CarteraReporteGeneral reporteGeneralCartera = new CarteraReporteGeneral();
                        ReporteGeneralCarteraParametersDto requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                        {
                            PeriodoActual = false,
                            Mes = fechaAjustada.Month,
                            Anio = fechaAjustada.Year,
                            NumeroCredito = responseCierreMensual.NumeroCredito,
                            TipoCredito = 2
                        };
                        reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();
                        var variacioSaldo = (reporteGeneralCartera.CRE_SALDO_CAPITAL - responseCierreMensual.SaldoCapital);

                        totalProvision = variacioSaldo * 0.01m;//(responseCierreMensual.CapitalSeven * 0.01M);
                        requestGeneralCartera = new ReporteGeneralCarteraParametersDto
                        {
                            PeriodoActual = false,
                            Mes = fechaAjustada.Month + 1,
                            Anio = fechaAjustada.Year,
                            NumeroCredito = responseCierreMensual.NumeroCredito,
                            TipoCredito = 2
                        };
                        var resultReporteGeneral = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera)).FirstOrDefault();
                        if (resultReporteGeneral == null)
                        {
                            totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.MORA_ANTERIOR) * 0.01M;
                            if (totalProvisionI > 0)
                            {
                                cuentasAfectar = cuentasContables
                                    .Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual")
                                    .ToList();
                            }
                            else
                            {
                                cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual");
                            }
                        }
                        else
                        {
                            reporteGeneralCartera = resultReporteGeneral;
                            totalProvisionI = Math.Abs(responseCierreMensual.AcumuladoInteresesMora - reporteGeneralCartera.MORA_ANTERIOR) * 0.01M;
                            if (totalProvisionI > 0)
                            {
                                cuentasAfectar = cuentasContables
                                    .Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual Mora" || x.REF_COLUMN_APLICA_PAGO == "Provision Individual")
                                    .ToList();
                            }
                            else
                            {
                                cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual");
                            }
                        }



                    }
                }
                ////if (aplicacionPago.Count() > 0)
                ////{
                ////    totalProvisionAbono = aplicacionPago != null
                ////        ? Math.Abs(aplicacionPago.FirstOrDefault().CPG_ABONO_CAPITAL) * 0.01M
                ////        : 0;
                ////}
                //else
                //{
                //    totalProvisionAbono = 0;
                //}

                totalProvision = totalProvision + totalProvisionAbono;

                Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                {
                    { "Cierres", totalProvision },
                    { "CierresI", totalProvisionI },
                    { "Provision Individual", totalProvision },
                    { "Provision Individual Total", totalProvision },
                    { "Provision Individual Remanente", totalProvisionR },
                    { "Provision Individual Mora", totalProvisionI }
                };

                foreach (var item in cuentasAfectar)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: " + responseCierreMensual.Nombres;
                    itemDetalle.Dmc_cant = 1;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = responseCierreMensual.NumeroCredito.ToString();
                    //itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? responseCierreMensual.Provision : 0;
                    //itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? responseCierreMensual.Provision : 0;
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    //itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? itemAplicacionPago.ValorCapital : 0;

                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = responseCierreMensual.Identificacion;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }
                }
            }


            return items;
        }

        public async Task<Response<InsertarMovContableResponse>> GarantiasCarteraSeven(ParamCierreMensualCarteraSevenDto param, IEnumerable<ReporteCierreMensualCarteraDto> responseCierreMensual, DateTime fechaCierre, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();

            var catalogoValores = await this._catalogosBusinessLogic.ObtenerTipoCredito();
            var descripcionLeasing = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
            var descripcionHipotecario = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;
            int anioActual = fechaCierre.Year;

            var Leasing = responseCierreMensual.Where(x =>
                x.TipoCredito == descripcionLeasing &&
                x.FechaDesembolso.Month == fechaCierre.Month &&
                x.FechaDesembolso.Year <= anioActual);

            var creditosHipotecarios = responseCierreMensual.Where(x =>
                x.TipoCredito == descripcionHipotecario &&
                x.FechaDesembolso.Month == fechaCierre.Month &&
                x.FechaDesembolso.Year <= anioActual);

            requestGarantiasDto requestGarantias = new requestGarantiasDto
            {
                Mes = fechaCierre.Month,
                Anio = fechaCierre.Year,
            };
            IEnumerable<ObtenerGarantiaActualizadaDto> resultadoGarantias = this._mapper.Map<IEnumerable<ObtenerGarantiaActualizadaDto>>(await _carteraRepository.ObtenerCreditosActualizanGarantia(requestGarantias));


            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (Leasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9099);

                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9099;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = fechaCierre;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Cierre de garantias Leasing";
                foreach (var item in Leasing)
                {
                    var resultadoGarantiasI = resultadoGarantias.Where(x =>
                    x.Identificacion == item.Identificacion);

                    items = ObtenerDetalleGarantiasActualizadas(item, equivalenciaCuentasLeasing, fechaCierre, resultadoGarantiasI).Result;
                    detalleMovContaLeasing.AddRange(items);
                }
                dataCabeceraMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaLeasing);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

            }
            if (creditosHipotecarios.Count() > 0)
            {
                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9005);

                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9005;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = fechaCierre;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Cierre de garantias Hipotecario";
                foreach (var item in creditosHipotecarios)
                {
                    items = ObtenerDetalleGarantiasActualizadas(item, equivalenciaCuentasHipotecario, fechaCierre, resultadoGarantias).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }
                dataCabeceraMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaHipotecario);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

            }


            if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;
                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CGI", totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                response.ReturnMessage += GenerarMensaje("leasing", "provisiones", requestDetallePago.Cc_Id);


            }
            if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CGI", totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                response.ReturnMessage += GenerarMensaje("hipotecario", "provisiones", requestDetallePago.Cc_Id);

            }

            return response;
        }

        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetalleGarantiasActualizadas(
            ReporteCierreMensualCarteraDto responseCierreMensual,
            IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
            DateTime fechaCierre,
            IEnumerable<ObtenerGarantiaActualizadaDto> resultadoGarantias)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            decimal valorGarantiaAct = 0;
            var cuentasAfectar = cuentasContables;

            var garantiaPorCedula = resultadoGarantias.FirstOrDefault(g => g.Identificacion == responseCierreMensual.Identificacion);

            if (responseCierreMensual.FechaDesembolso.Month == fechaCierre.Month)
            {
                if (responseCierreMensual.FechaDesembolso.Year <= fechaCierre.Year - 1)
                {
                    // Cumple con ser del mismo mes y tener al menos un año de diferencia
                    valorGarantiaAct = garantiaPorCedula?.ValorAjuste ?? 0;
                    cuentasAfectar = cuentasContables
                        .Where(x => x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")
                        .ToList();
                }
                else
                {
                    valorGarantiaAct = garantiaPorCedula?.ValorAjusteAnterior ?? 0;
                    cuentasAfectar = cuentasContables
                        .Where(x => x.REF_COLUMN_APLICA_PAGO == "garantiasIni")
                        .ToList();
                }
            }

            // Generar mapeoPagos dinámico basado en las cuentasAfectar
            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>();

            foreach (var cuenta in cuentasAfectar)
            {
                var clave = cuenta.REF_COLUMN_APLICA_PAGO;
                if (!string.IsNullOrEmpty(clave) && !mapeoPagos.ContainsKey(clave))
                {
                    mapeoPagos[clave] = valorGarantiaAct;
                }
            }

            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                {
                    Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                    Dmc_desc = $"cliente: " + responseCierreMensual.Identificacion,
                    Dmc_cant = 1,
                    Dmc_acti = 0,
                    Dmc_refe = responseCierreMensual.NumeroCredito.ToString(),
                    Dmc_vadb = item.TIPO_OPERACION == "D" && mapeoPagos.ContainsKey(item.REF_COLUMN_APLICA_PAGO)
                                ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0,
                    Dmc_vacr = item.TIPO_OPERACION == "C" && mapeoPagos.ContainsKey(item.REF_COLUMN_APLICA_PAGO)
                                ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0,
                    Dmc_vaba = 0,
                    Ter_coda = responseCierreMensual.Identificacion,
                    Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                    Arb_coda = "3011",
                    Arb_codp = "099",
                    Arb_cods = "1001",
                    Ter_codm = "0"
                };

                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }

        public async Task<Response<InsertarMovContableResponse>> PrevisionGeneralCarteraSeven(ParamCierreMensualCarteraSevenDto param, IEnumerable<ReporteCierreMensualCarteraDto> responseCierreMensual, DateTime fechaCierre, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();

            var catalogoValores = await this._catalogosBusinessLogic.ObtenerTipoCredito();
            var descripcionLeasing = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
            var descripcionHipotecario = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;
            int anioActual = fechaCierre.Year;

            var Leasing = responseCierreMensual.Where(x => x.TipoCredito == descripcionLeasing);
            var creditosHipotecarios = responseCierreMensual.Where(x => x.TipoCredito == descripcionHipotecario);

            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (Leasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9075);

                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9075;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = fechaCierre;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Cierre de Provision General Leasing";

                items = ObtenerDetallePrevisionGeneral(Leasing, equivalenciaCuentasLeasing, fechaCierre, 1).Result;
                detalleMovContaLeasing.AddRange(items);

                dataCabeceraMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaLeasing);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

            }
            if (creditosHipotecarios.Count() > 0)
            {
                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9004);

                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9004;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = fechaCierre;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Cierre de Provision General Hipotecario";

                items = ObtenerDetallePrevisionGeneral(creditosHipotecarios, equivalenciaCuentasHipotecario, fechaCierre, 2).Result;
                detalleMovContaHipotecario.AddRange(items);

                dataCabeceraMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaHipotecario);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

            }


            if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;
                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CPG", totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                response.ReturnMessage += GenerarMensaje("leasing", "provisiones", requestDetallePago.Cc_Id);


            }
            if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CPG", totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                response.ReturnMessage += GenerarMensaje("hipotecario", "provisiones", requestDetallePago.Cc_Id);

            }

            return response;
        }

        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetallePrevisionGeneral(IEnumerable<ReporteCierreMensualCarteraDto> responseCierreMensual, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, DateTime fechaCierre, int tipoCredito)
        {

            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables;


            var fechaAjustada = fechaCierre.AddMonths(-1);
            var fechaAjustada2 = fechaCierre.AddMonths(-2);

            IEnumerable<CarteraReporteGeneral> reporteGeneralCartera;
            ReporteGeneralCarteraParametersDto requestGeneralCartera = new ReporteGeneralCarteraParametersDto
            {
                PeriodoActual = false,
                Mes = fechaAjustada.Month,
                Anio = fechaAjustada.Year,
                TipoCredito = tipoCredito
            };
            reporteGeneralCartera = (await this._reportesRepository.GenerarReporteGeneralCartera(requestGeneralCartera));

            decimal difTotalCapital = 0;

            if (tipoCredito == 1)
            {
                decimal totalCapital = reporteGeneralCartera.Sum(x => x.CRE_SALDO_CAPITAL);
                decimal sumaCapital = responseCierreMensual.Sum(x => x.AcumuladoCapital);
                decimal sumaCapitalTradicional = responseCierreMensual.Sum(x => x.SaldoCapitalTradicional);
                decimal sumaCapitalTotal = sumaCapital + sumaCapitalTradicional;
                difTotalCapital = sumaCapitalTotal - totalCapital;
            }
            else
            {
                decimal totalCapital = reporteGeneralCartera.Sum(x => x.CRE_SALDO_CAPITAL);
                decimal sumaCapital = responseCierreMensual.Sum(x => x.SaldoCapital);
                difTotalCapital = sumaCapital - totalCapital;
            }


            decimal totalInteresRemanente = reporteGeneralCartera.Sum(x => x.CRE_VALOR_DEUDA_REMANENTE);
            decimal sumaRemanente = responseCierreMensual.Sum(x => x.ValorDeudaRemanente);
            decimal difTotalInteresRemanente = sumaRemanente - totalInteresRemanente;

            var creditosMoroso = reporteGeneralCartera.Where(x => x.CRE_FECHA_ULTIMO_PAGO.Month < fechaAjustada.Month);
            var creditosMoroso2 = reporteGeneralCartera.Where(x => x.CRE_FECHA_ULTIMO_PAGO.Month < fechaAjustada2.Month);
            decimal totalInteresMora = creditosMoroso.Sum(x => x.PLN_INTERESES);
            decimal totalInteresMora2 = creditosMoroso2.Sum(x => x.MORA_ANTERIOR);
            decimal totalMora = reporteGeneralCartera.Sum(x => x.MORA_ANTERIOR);
            var creditosMoraActual = responseCierreMensual.Where(x => x.EstadoCredito == "EN MORA").ToList();
            decimal sumaMora = responseCierreMensual.Sum(x => x.AcumuladoInteresesMora);
            decimal difTotalMora = sumaMora - (totalInteresMora + totalInteresMora2);


            var claves = new List<string>();

            claves.Add(difTotalCapital >= 0 ? "provisionSaldo" : "provisionSaldoC");
            claves.Add(difTotalMora >= 0 ? "provisionMora" : "provisionMoraC");

            if (tipoCredito == 1)
            {
                claves.Add(difTotalInteresRemanente >= 0 ? "provisionRemanente" : "provisionRemanenteC");
            }

            cuentasAfectar = cuentasContables
                .Where(x => claves.Contains(x.REF_COLUMN_APLICA_PAGO))
                .ToList();


            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
            {
                { "provisionSaldo", Math.Abs(difTotalCapital) * 0.01m },
                { "provisionSaldoC", Math.Abs(difTotalCapital) * 0.01m },
                { "provisionMora", Math.Abs(difTotalMora) * 0.01m },
                { "provisionMoraC", Math.Abs(difTotalMora) * 0.01m },
                { "provisionRemanente", Math.Abs(difTotalInteresRemanente) * 0.01m },
                { "provisionRemanenteC", Math.Abs(difTotalInteresRemanente) * 0.01m },
            };

            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = $"Provisión General";
                itemDetalle.Dmc_cant = 1;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = $"000201508";
                itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = $"000201508";
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = "3011";
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";

                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }

        public async Task<Response<InsertarMovContableResponse>> AmortizacionCarteraSeven(ParamCierreMensualCarteraSevenDto param, IEnumerable<ReporteCierreMensualCarteraDto> responseCierreMensual, DateTime fechaCierre, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            var catalogoValores = await this._catalogosBusinessLogic.ObtenerTipoCredito();
            var descripcionLeasing = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
            var descripcionHipotecario = catalogoValores.Data?.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;

            var Leasing = responseCierreMensual.Where(x => x.TipoCredito == descripcionLeasing);
            var creditosHipotecarios = responseCierreMensual.Where(x => x.TipoCredito == descripcionHipotecario);
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (Leasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);

                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9065;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = fechaCierre;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Cierre de amortizacion Leasing";
                foreach (var item in Leasing)
                {
                    items = ObtenerDeTalleAmortizacionMensualSeven(item, equivalenciaCuentasLeasing).Result;
                    detalleMovContaLeasing.AddRange(items);
                }
                dataCabeceraMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaLeasing);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                /*suspencion consumo seven solo guardado core*/
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado creando el SCnMcont en Seven.";
                //respuestaSevenLeasing = response.Data.Body.InsertarMovContableResult;
            }
            if (creditosHipotecarios.Count() > 0)
            {
                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);
                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9003;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = fechaCierre;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Cierre de amortizacion Hipotecario";
                foreach (var item in creditosHipotecarios)
                {
                    items = ObtenerDeTalleAmortizacionMensualSeven(item, equivalenciaCuentasHipotecario).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }
                dataCabeceraMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaHipotecario);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                /*suspencion consumo seven solo guardado core*/
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage += response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                //           "para créditos Hipotecarios";
                //respuestaSevenHipotecario = response.Data.Body.InsertarMovContableResult;
            }

            //int respuestaLeasing = GetValueResponseXmlSevenFactura(respuestaSevenLeasing, 2);
            //int respuestaHipotecario = GetValueResponseXmlSevenFactura(respuestaSevenHipotecario, 2);


            if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC", totalDmcVacr, totalDmcCadb);
                //var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                //response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable cierre mensual  leasing: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                response.ReturnMessage += GenerarMensaje("leasing", "cierre mensual", requestDetallePago.Cc_Id);

            }
            if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC", totalDmcVacr, totalDmcCadb);
                //var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                //response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable cierre mensual  hipotecario: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                response.ReturnMessage += GenerarMensaje("hipotecario", "cierre mensual", requestDetallePago.Cc_Id);

            }




            //if (respuestaLeasing == 0 || respuestaHipotecario == 0)
            //{

            //    if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            //    {
            //        RequestDetallePago requestDetallePago = new RequestDetallePago();
            //        string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
            //        requestDetallePago.Cpg_id = new Guid();
            //        requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
            //        requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
            //        requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
            //        requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
            //        requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
            //        requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
            //        requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
            //        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
            //        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            //        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
            //    }
            //    if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            //    {
            //        RequestDetallePago requestDetallePago = new RequestDetallePago();
            //        string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
            //        requestDetallePago.Cpg_id = new Guid();
            //        requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
            //        requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
            //        requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
            //        requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
            //        requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
            //        requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
            //        requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
            //        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
            //        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            //        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
            //    }



            //}
            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleAmortizacionMensualSeven(ReporteCierreMensualCarteraDto responseCierreMensual, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables)
        {
            var ValorCuota = Math.Round(responseCierreMensual.SeguroHogarSeven, 2) + Math.Round(responseCierreMensual.SeguroVidaSeven, 2) + Math.Round(responseCierreMensual.InteresesCorrientesSeven, 2) + Math.Round(responseCierreMensual.CapitalSeven, 2);
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables;
            if (responseCierreMensual.ValorCuotaSeven > 0 && responseCierreMensual.CapitalSeven > 0)
            {
                if (responseCierreMensual.TipoCredito == "LEASING HABITACIONAL")
                {

                    if (responseCierreMensual.SeguroVidaSeven == 0)
                    {
                        cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA")
                        && !(x.REF_COLUMN_APLICA_PAGO == "desembolso") && !(x.REF_COLUMN_APLICA_PAGO == "inmuebles")
                        && !(x.REF_COLUMN_APLICA_PAGO == "inmueblesAct") && !(x.REF_COLUMN_APLICA_PAGO == "cxc")
                        && !(x.REF_COLUMN_APLICA_PAGO == "Dev-Recursos") && !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes")).ToList();
                    }
                    else
                    {
                        cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "desembolso")
                        && !(x.REF_COLUMN_APLICA_PAGO == "inmuebles") && !(x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")
                        && !(x.REF_COLUMN_APLICA_PAGO == "cxc")
                        && !(x.REF_COLUMN_APLICA_PAGO == "Dev-Recursos") && !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes")).ToList();

                    }

                }
                if (responseCierreMensual.TipoCredito == "CRÉDITO HIPOTECARIO")
                {
                    if (responseCierreMensual.SeguroVidaSeven == 0)
                    {
                        cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA")
                        && !(x.REF_COLUMN_APLICA_PAGO == "desembolso") && !(x.REF_COLUMN_APLICA_PAGO == "inmuebles")
                        && !(x.REF_COLUMN_APLICA_PAGO == "inmueblesAct") && !(x.REF_COLUMN_APLICA_PAGO == "cxc")
                        && !(x.REF_COLUMN_APLICA_PAGO == "Dev-Recursos") && !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes")).ToList();
                    }
                    else
                    {
                        cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "desembolso")
                        && !(x.REF_COLUMN_APLICA_PAGO == "inmuebles") && !(x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")
                         && !(x.REF_COLUMN_APLICA_PAGO == "cxc")
                        && !(x.REF_COLUMN_APLICA_PAGO == "Dev-Recursos") && !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes")).ToList();
                    }
                }
                Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                    {
                        { "CPG_VALOR_PAGO", ValorCuota },
                        { "CPG_ABONO_SEGURO_HOGAR", responseCierreMensual.SeguroHogarSeven },
                        { "CPG_ABONO_SEGURO_VIDA", responseCierreMensual.SeguroVidaSeven },
                        { "CPG_ABONO_INTERES_CORRIENTE", responseCierreMensual.InteresesCorrientesSeven },
                        { "CPG_ABONO_CAPITAL", responseCierreMensual.CapitalSeven }
                    };
                Dictionary<string, decimal> mapeoPagosHipotecario = new Dictionary<string, decimal>
                    {
                        { "CPG_VALOR_PAGO", ValorCuota },
                        { "CPG_ABONO_SEGURO_VIDA", responseCierreMensual.SeguroVidaSeven },
                        { "CPG_ABONO_SEGURO_HOGAR", responseCierreMensual.SeguroHogarSeven },
                        { "CPG_ABONO_CAPITAL", responseCierreMensual.CapitalSeven },
                        { "CPG_ABONO_INTERES_CORRIENTE", responseCierreMensual.InteresesCorrientesSeven }

                    };


                foreach (var item in cuentasAfectar)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: " + responseCierreMensual.Identificacion;
                    itemDetalle.Dmc_cant = 1;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = responseCierreMensual.NumeroCredito.ToString();
                    if (item.TCR_ID == 1)
                    {
                        itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? (decimal)mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                        itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? (decimal)mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    }
                    else
                    {
                        itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? (decimal)mapeoPagosHipotecario[item.REF_COLUMN_APLICA_PAGO] : 0;
                        itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? (decimal)mapeoPagosHipotecario[item.REF_COLUMN_APLICA_PAGO] : 0;
                    }
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = responseCierreMensual.Identificacion;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }
                }
            }


            return items;
        }

        public async Task<Response<InsertarMovContableResponse>> CuentaDeOrdenCarteraSeven(ParamCierreMensualCarteraSevenDto param, IEnumerable<ReporteCierreMensualCarteraDto> responseCierreMensual, DateTime fechaCierre, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            var catalogoValoresResult = await this._catalogosBusinessLogic.ObtenerTipoCredito();
            IEnumerable<TipoCreditoDto> catalogoValores = catalogoValoresResult.Data;
            var descripcionCreditosLeasing = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
            var descripcionCreditosHipotecarios = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;
            var Leasing = responseCierreMensual.Where(x => x.TipoCredito == descripcionCreditosLeasing);
            var creditosHipotecarios = responseCierreMensual.Where(x => x.TipoCredito == descripcionCreditosHipotecarios);

            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (Leasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);

                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9065;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = fechaCierre;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Cierre de cuenta de orden Leasing";
                foreach (var item in Leasing)
                {
                    items = ObtenerDeTalleCuentaDeOrdenMensualSeven(item, equivalenciaCuentasLeasing).Result;
                    detalleMovContaLeasing.AddRange(items);
                }
                dataCabeceraMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaLeasing);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                /*se suspende consumo a seven solo se guarda en el core*/
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado creando el SCnMcont en Seven.";
                //respuestaSevenLeasing = response.Data.Body.InsertarMovContableResult;
            }
            if (creditosHipotecarios.Count() > 0)
            {
                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9005);

                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9005;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = fechaCierre;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Cierre de cuenta de orden Hipotecario";
                foreach (var item in creditosHipotecarios)
                {
                    items = ObtenerDeTalleCuentaDeOrdenMensualSeven(item, equivalenciaCuentasHipotecario).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }
                dataCabeceraMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaHipotecario);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                /*se suspende consumo a seven solo se guarda en el core*/
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage += response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                //           "para créditos Hipotecarios";
                //respuestaSevenHipotecario = response.Data.Body.InsertarMovContableResult;
            }
            //int respuestaLeasing = GetValueResponseXmlSevenFactura(respuestaSevenLeasing, 2);
            //int respuestaHipotecario = GetValueResponseXmlSevenFactura(respuestaSevenHipotecario, 2);
            //if (respuestaLeasing == 0 || respuestaHipotecario == 0)
            //{

            //    if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            //    {
            //        RequestDetallePago requestDetallePago = new RequestDetallePago();
            //        string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
            //        requestDetallePago.Cpg_id = new Guid();
            //        requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
            //        requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
            //        requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
            //        requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
            //        requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
            //        requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
            //        requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
            //        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
            //        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            //        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
            //    }
            //    if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            //    {
            //        RequestDetallePago requestDetallePago = new RequestDetallePago();
            //        string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
            //        requestDetallePago.Cpg_id = new Guid();
            //        requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
            //        requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
            //        requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
            //        requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
            //        requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
            //        requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
            //        requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
            //        requestDetallePago.Fecha_Ejecucion = DateTime.Now;
            //        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            //        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
            //    }


            //}
            if (dataCabeceraMovContaLeasing.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaLeasing.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;


                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMCO", totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                //response.ReturnMessage = requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable Cierre mensual leasing: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                response.ReturnMessage += GenerarMensaje("leasing", "cuentas de orden", requestDetallePago.Cc_Id);

            }
            if (dataCabeceraMovContaHipotecario.vDetalle.Count() > 0)
            {
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovContaHipotecario.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;


                decimal totalDmcVacr = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMCO", totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                //response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable cierre mensual  hipotecario: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                response.ReturnMessage += GenerarMensaje("hipotecario", "cuentas de orden", requestDetallePago.Cc_Id);

            }
            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleCuentaDeOrdenMensualSeven(ReporteCierreMensualCarteraDto responseCierreMensual, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables)
        {

            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContablesDeOrden;

            cuentasContablesDeOrden = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "desembolso"));

            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                {

                    { "desembolso", responseCierreMensual.SaldoCapital }
                };


            if (responseCierreMensual.EstadoCredito == "AL DIA" && responseCierreMensual.SaldoCapital > 0)
            {
                foreach (var item in cuentasContablesDeOrden)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: " + responseCierreMensual.Identificacion;
                    itemDetalle.Dmc_cant = 1;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = responseCierreMensual.NumeroCredito.ToString();
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    //itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? responseCierreMensual.SaldoCapital : 0;
                    //itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? responseCierreMensual.SaldoCapital : 0;
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = responseCierreMensual.Identificacion;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }
                }
            }
            return items;
        }


        private string GenerarMensaje(string tipo, string operacion, int? comprobanteId)
        {
            return comprobanteId is not null
                ? $"Comprobante {operacion} {tipo}: {comprobanteId}"
                : $"Error al generar: Comprobante {operacion} {tipo}";
        }

        private int GetValueResponseXmlSevenFactura(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("MCO_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("MCO_NUME");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                case 2:
                    XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
                    Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }
        /// <summary>
        /// Obtener Credito Cartera By Identificacion
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        private async Task<Response<InformacionCreditoDto>> ObtenerCreditoCarteraByIdentificacion(string identificacion)
        {
            var credito = await this._carteraRepository.ObtenerCreditoCarteraPorIdentificacion(identificacion);

            return new Response<InformacionCreditoDto>() { Data = this._mapper.Map<InformacionCreditoDto>(credito) };
        }

        public async Task<Response<ProductoClienteDto>> EjecutarDesembolso(RequestEjecucionDesembolsoDto request)
        {
            var producto = await this._carteraRepository.PrepararEjecucionDesembolso(request.TipoIdentificacion, request.Identificacion);

            producto.CRE_FECHA_CREACION = producto.CRE_FECHA_DESEMBOLSO = producto.CRE_FECHA_ESTADO = producto.CRE_FECHA_MODIFICACION = producto.CRE_FECHA_ULTIMO_PAGO =
            producto.CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE = producto.CRE_FECHA_ULTIMO_PAGO_INTERES_MORA = producto.CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR =
            producto.CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA = DateTime.Today;

            producto.CRE_FECHA_PROXIMO_PAGO = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1);

            producto.ESC_ID = 1;
            var ejecutado = await this._carteraRepository.EjecutarDesembolso(producto);

            return new Response<ProductoClienteDto>() { Data = _mapper.Map<ProductoClienteDto>(ejecutado) };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>19/10/2021</Date>
        public async Task<Response<InformacionCreditoDto>> ObtenerCreditoCartera(ObtenerProductoCartera obtenerProductoCartera)
        {
            return new Response<InformacionCreditoDto> { Data = this._mapper.Map<InformacionCreditoDto>(await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera)) };
        }

        /// <summary>
        /// Obtener parametros pagos
        /// </summary>
        /// <returns></returns>
        public async Task<Response<ParametrosAplicacionPagosDto>> ObtenerParametrosAplicacionPagos()
        {
            return new Response<ParametrosAplicacionPagosDto>() { Data = this._mapper.Map<ParametrosAplicacionPagosDto>(await this._carteraRepository.ObtenerParametrosAplicacionPagos()) };
        }

        /// <summary>
        /// Obtiene productos por cliente
        /// </summary>
        /// <param name="productoCliente"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>6/09/2021</Date>

        public async Task<Response<IEnumerable<ProductoClienteDto>>> ObtenerProductoCliente(SolicitudProductoPorClienteDto request)
        {
            IEnumerable<ProductoClienteDto> response = this._mapper.Map<IEnumerable<ProductoClienteDto>>(await this._carteraRepository.ObtenerProductoCliente(request.TipoIdentificacion, request.Identificacion, request.IdProducto));
            IEnumerable<TipoActorDto> tipoActores = this._mapper.Map<IEnumerable<TipoActorDto>>(await this._carteraRepository.ObtenerTipoActor());

            foreach (ProductoClienteDto item in response)
            {
                var requestPlanPago = new ObtenerProductoCartera
                {
                    Nro_Credito = item.NumeroCredito,
                    Tcr_Id = item.IdTipoCredito
                };
                var dataPlanPago = await ObtenerPlanPagosCredito(requestPlanPago);
                var cuotaCanceladas = dataPlanPago.Data.PlanPago.Where(x => x.EstadoCuota == "PAGADA").ToList();
                var cuotaPagaParcialmente = dataPlanPago.Data.PlanPago.Where(x => x.EstadoCuota == "PAGADA PARCIALMENTE").ToList();

                if (item.IdTipoCredito == 1) //solo para leasing
                {
                    var validateNovedatesRestructuracion = new NovedadCartera();
                    var obtenerNovedadesCarteras = await _carteraRepository.ObtenerNovedadesCartera(item.IdCredito);
                    if (obtenerNovedadesCarteras.Count() > 0)
                    {
                        validateNovedatesRestructuracion = obtenerNovedadesCarteras.Where(x => x.TNV_ID == 3).FirstOrDefault();
                    }

                    if (item.SaldoCapital != 0 && item.SaldoTotalCorte != 0 || validateNovedatesRestructuracion is not null)
                    {
                        var valorCapitalCancelado = cuotaCanceladas.Sum(x => x.CapitalReducido);
                        var interesPagado = cuotaCanceladas.Sum(x => x.InteresPorPagar);

                        if (validateNovedatesRestructuracion?.TNV_ID != 0)
                        {
                            var cuotaActual = dataPlanPago.Data.PlanPago
                                .Where(x => x.Cuota == item.NumeroCuotasCanceladas)
                                .FirstOrDefault();

                            var saldoActualCuota = cuotaActual?.SaldoCapital ?? item.DeudaActual;
                            var capitalActualCuota = cuotaActual?.CapitalReducido ?? 0;

                            item.SaldoCapital = saldoActualCuota;              //  se mantiene igual
                            item.CapitalRemanenteCorte = capitalActualCuota;   //  ya está OK, no se toca

                            // ⚠️ Interés Remanente: solo para créditos EN MORA se recalcula como
                            // sumatoria desde la cuota 1 hasta la cuota del corte (incluida la cuota en mora)


                            // Si NO está en mora, se deja el comportamiento actual (que ya funciona bien) sin tocar

                            if (obtenerNovedadesCarteras.Count() > 0)
                            {
                                validateNovedatesRestructuracion = obtenerNovedadesCarteras.Where(x => x.TNV_ID == 1 && (x.ENC_ID == 1 || x.ENC_ID == 2)).FirstOrDefault();
                                if (validateNovedatesRestructuracion is null)
                                {
                                    item.CapitalRemanenteCorte = 0;
                                    item.InteresRemanente = 0;
                                    item.InteresesRemanenteCorte = 0;
                                }
                            }

                            item.SaldoTotalCorte = item.SaldoCapital + item.InteresesRemanenteCorte;
                        }
                        else
                        {
                            if (item.NumeroCuotasCanceladas == 0)
                            {
                                item.SaldoCapital = item.DeudaActual;
                            }
                            else
                            {

                                bool estaEnMora = item.EstadoCredito == "EN MORA" ? true : false;
                                if (estaEnMora)
                                {
                                    int cuotaCorteActual = item.NumeroCuotasCanceladas + 1; // la cuota en mora (no pagada)
                                    decimal saldoCapital = dataPlanPago.Data.PlanPago
                                       .Where(x => x.Cuota == item.NumeroCuotasCanceladas)
                                       .Sum(x => x.SaldoCapital);

                                    decimal interesRemanenteCuotaCorte = dataPlanPago.Data.PlanPago
                                        .Where(x => x.Cuota <= cuotaCorteActual)
                                        .Sum(x => x.InteresPorPagar);
                                    decimal capitalRemantenCuotaCorte = dataPlanPago.Data.PlanPago
                                        .Where(x => x.Cuota <= cuotaCorteActual)
                                        .Sum(x => x.CapitalPorPagar);
                                    item.SaldoCapital = saldoCapital;
                                    item.InteresesRemanenteCorte =  interesRemanenteCuotaCorte;
                                    item.InteresRemanente =  interesRemanenteCuotaCorte;
                                    item.CapitalRemanenteCorte = capitalRemantenCuotaCorte;
                                    interesPagado = item.InteresesRemanenteCorte;
                                    item.SaldoTotalCorte = item.SaldoCapital + item.InteresesRemanenteCorte;
                                }
                                else
                                {
                                    item.NumeroCuotasCanceladas = item.NumeroCuotasCanceladas == 0 ? 1 : item.NumeroCuotasCanceladas;
                                    var saldoActualCuota = dataPlanPago.Data.PlanPago.Where(x => x.Cuota == item.NumeroCuotasCanceladas).FirstOrDefault()?.SaldoCapital ?? 0;
                                    item.SaldoCapital = saldoActualCuota;
                                }

                            }

                            item.SaldoTotalCorte = item.SaldoCapital + interesPagado;
                            if (obtenerNovedadesCarteras.Count() > 0)
                            {
                                validateNovedatesRestructuracion = obtenerNovedadesCarteras.Where(x => x.TNV_ID == 1 && (x.ENC_ID == 1 || x.ENC_ID == 2)).FirstOrDefault();
                                if (validateNovedatesRestructuracion is not null)
                                {
                                    item.InteresesRemanenteFinal = validateNovedatesRestructuracion.NVC_INTERES_REMANENTE;
                                }
                            }
                        }

                        foreach (var pagadaParcial in cuotaPagaParcialmente)
                        {
                            var movimientosPago = await _carteraRepository.ObtenerAplicacionPago(item.IdCredito);
                            var ultimoPago = movimientosPago
                                .Where(x => x.CPG_VALOR_PAGO > 0
                                         && x.CPG_FECHA_APLICACION == pagadaParcial.Fecha
                                         && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2))
                                .OrderByDescending(x => x.CPG_FECHA_CREACION)
                                .ToList();

                            if (ultimoPago.Any())
                            {
                                decimal capitalPagado = ultimoPago.Sum(x => x.CPG_ABONO_CAPITAL);
                                decimal interesPagadoParcial = ultimoPago.Sum(x => x.CPG_ABONO_INTERES_CORRIENTE);
                                decimal seguroVidaPagadoParcial = ultimoPago.Sum(x => x.CPG_ABONO_SEGURO_VIDA);
                                decimal seguroHogarPagadoParcial = ultimoPago.Sum(x => x.CPG_ABONO_SEGURO_HOGAR);

                                var ultimaCuotaPagada = cuotaCanceladas
                                    .Where(x => x.Cuota < pagadaParcial.Cuota)
                                    .OrderByDescending(x => x.Cuota)
                                    .FirstOrDefault();

                                decimal saldoCapitalUltimaCuotaPagada = ultimaCuotaPagada?.SaldoCapital ?? item.DeudaActual;

                                cuotaCanceladas.Add(pagadaParcial);
                                interesPagado = cuotaCanceladas.Sum(x => x.InteresPorPagar);

                                item.SaldoCapital = saldoCapitalUltimaCuotaPagada - capitalPagado;
                                item.SaldoTotalCorte = item.SaldoCapital + interesPagado;
                            }
                        }

                        if (item.EstadoCredito == "CANCELADO")
                        {
                            item.SaldoCapital = 0;
                            item.SaldoTotalCorte = 0;
                            item.CapitalRemanenteCorte = 0;
                            item.InteresRemanente = 0;
                        }
                    }
                }

                if (item.IdTipoCredito == 2 || item.IdTipoCredito == 5)
                {
                    var saldoCapital = cuotaCanceladas?.LastOrDefault()?.Saldo ?? 0;
                    item.SaldoCapital = saldoCapital;
                    item.SaldoTotalCorte = saldoCapital;
                    if (cuotaPagaParcialmente.Count > 0)
                    {
                        foreach (var pagadaParcial in cuotaPagaParcialmente)
                        {
                            var valorCapitalCancelado = cuotaCanceladas.Sum(x => x.CapitalReducido);
                            var interesPagado = cuotaCanceladas.Sum(x => x.InteresPorPagar);
                            var movimientosPago = await _carteraRepository.ObtenerAplicacionPago(item.IdCredito);
                            var ultimoPago = movimientosPago.Where(x => x.CPG_VALOR_PAGO > 0
                                && x.CPG_FECHA_APLICACION == pagadaParcial.Fecha
                                && x.CC_ESTADO_ID == 1)
                                .OrderByDescending(x => x.CPG_FECHA_CREACION)
                                .ToList();
                            if (ultimoPago.Any())
                            {
                                cuotaCanceladas.AddRange(cuotaPagaParcialmente);
                                interesPagado = cuotaCanceladas.Sum(x => x.InteresPorPagar);
                                item.SaldoCapital = item.SaldoCapital - ultimoPago.Sum(x => x.CPG_ABONO_CAPITAL);
                                item.SaldoTotalCorte = item.SaldoCapital + interesPagado;
                            }
                        }
                    }
                }

                item.AcumuladoSeguroHogar = cuotaCanceladas.Sum(x => x.SeguroHogar);
                item.AcumuladoSeguroVida = cuotaCanceladas.Sum(x => x.SeguroVida);

                BUAConsultaBE bUAConsultaBE = new BUAConsultaBE
                {
                    identificacion = item.Identificacion,
                    tipo_Id = int.Parse(item.TipoIdentificacion),
                };
                var Cliente = await _buaService.ObtenerAfiliadoBuaCH(bUAConsultaBE);
                item.NombreCliente = Cliente.Data != null ? Cliente.Data.primerNombre + " " + Cliente.Data.segundoNombre + " " + Cliente.Data.primerApellido + " " + Cliente.Data.segundoApellido : "";
            }

            return new Response<IEnumerable<ProductoClienteDto>> { Data = response };
        }
        public async Task<Response<IEnumerable<ProductoClienteDto>>> ObtenerProductosCliente(SolicitudProductoPorClienteDto request)
        {
            //cambios realizados tiene mapper
            IEnumerable<ProductoClienteDto> response = this._mapper.Map<IEnumerable<ProductoClienteDto>>(await this._carteraRepository.ObtenerProductosCliente(request.TipoIdentificacion, request.Identificacion));

            return new Response<IEnumerable<ProductoClienteDto>> { Data = response };
        }

        public async Task<Response<ProductoClienteDto>> PrepararEjecucionDesembolso(SolicitudProductoPorClienteDto request)
        {
            //VALORES QUE SE DEBEN REVISAR ESTAN QUEMNADOS
            var response = await this._carteraRepository.PrepararEjecucionDesembolso(request.TipoIdentificacion, request.Identificacion);
            var abonoExtra = await this._carteraRepository.ObtenerAbonoExtra(request.Identificacion);
            if (response != null && abonoExtra != null)
            {
                response.TIPO_IDENTIFICACION = request.TipoIdentificacion;
                response.CLI_IDENTIFICACION = request.Identificacion;
                response.CRE_TIPO_ABONO_EXTRA = abonoExtra.TIPO_ABONO_EXTRA;
                response.CRE_VALOR_ABONO_EXTRA = (int)abonoExtra.VALOR_ABONO_EXTRA;
                response.CRE_FECHA_CREACION = response.CRE_FECHA_DESEMBOLSO = response.CRE_FECHA_ESTADO = response.CRE_FECHA_MODIFICACION =
                response.CRE_FECHA_ULTIMO_PAGO = response.CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE = response.CRE_FECHA_ULTIMO_PAGO_INTERES_MORA =
                response.CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR = response.CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA = DateTime.Now;
                response.CRE_SALDO_CAPITAL = response.CRE_VALOR_DESEMBOLSO;
                response.CRE_DIAS_MORA = 0;
                response.CRE_SALDO_CAPITAL_MORA = response.CRE_VALOR_DEUDA_REMANENTE = response.CRE_VALOR_OPCION_COMPRA =
                response.CRE_ACUMULADO_INTERES_MORA = response.CRE_ACUMULADO_INTERES_CORRIENTE = response.CRE_ACUMULADO_SEGURO_HOGAR =
                response.CRE_TASA_SEGURO_VIDA = 0;

                response.CRE_TASA_CREDITO_PER = ((decimal)(Math.Pow((double)(1 + response.CRE_TASA_CREDITO_EA), Convert.ToDouble(1) / Convert.ToDouble(12)) - 1)); ;//Aplicar formula para obtener tasa nominal a partir de response.CRE_TASA_CREDITO_EA

                if (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) > 29)
                {
                    response.CRE_DIA_PAGO = 30;
                    if (DateTime.Now > DateTime.Parse($"{response.CRE_DIA_PAGO}/{DateTime.Now.Month}/{DateTime.Now.Month}"))
                    {
                        var fechaProximoMes = DateTime.Now.AddMonths(1);
                        if (DateTime.DaysInMonth(fechaProximoMes.Year, fechaProximoMes.Month) > 29)
                        {
                            response.CRE_FECHA_PROXIMO_PAGO = DateTime.Parse($"{30}/{fechaProximoMes.Month}/{fechaProximoMes.Year}");
                        }
                        else
                        {
                            response.CRE_FECHA_PROXIMO_PAGO = DateTime.Parse($"{DateTime.DaysInMonth(fechaProximoMes.Year, fechaProximoMes.Month)}/{fechaProximoMes.Month}/{fechaProximoMes.Year}");
                        }
                    }
                }
                else
                {
                    response.CRE_DIA_PAGO = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                    var fechaProximoMes = DateTime.Now.AddMonths(1);
                    if (DateTime.DaysInMonth(fechaProximoMes.Year, fechaProximoMes.Month) > 29)
                    {
                        response.CRE_FECHA_PROXIMO_PAGO = DateTime.Parse($"{30}/{fechaProximoMes.Month}/{fechaProximoMes.Year}");
                    }
                    else
                    {
                        response.CRE_FECHA_PROXIMO_PAGO = DateTime.Parse($"{DateTime.DaysInMonth(fechaProximoMes.Year, fechaProximoMes.Month)}/{fechaProximoMes.Month}/{fechaProximoMes.Year}");
                    }
                }

                var data = await this._carteraRepository.EjecutarDesembolso(response);
                return new Response<ProductoClienteDto> { Data = this._mapper.Map<ProductoClienteDto>(response) };
            }

            return null;
        }

        /// <summary>
        /// Obtiene todos los registros de parametros 
        /// </summary>
        /// <param name="consultarTasas"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>03/12/2021</Date>

        public async Task<Response<IEnumerable<TasaSimuladorDto>>> ObtenerTasasSimulador()
        {
            var catalogoTasas = this._mapper.Map<IEnumerable<TasaSimuladorDto>>(await this._carteraRepository.ObtenerTasasSimulador());
            return new Response<IEnumerable<TasaSimuladorDto>> { Data = catalogoTasas };
        }

        /// <summary>
        /// Crear registro Simulacion
        /// </summary>
        /// <param name="tasas"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<TasaSimuladorDto>> CrearTasasSimulacion(TasaSimuladorDto tasas)
        {
            var tasaCreate = this._mapper.Map<TasaSimulador>(tasas);
            var tasa = tasas.TasaEA / 100;
            var tasaUsuara = (decimal)Math.Pow(1 + (double)tasa, 1.0 / 12.0) - 1;
            tasaCreate.SIM_TASA_NOMINAL_MENSUAL = tasaUsuara * 100;
            var response = this._mapper.Map<TasaSimuladorDto>(await this._carteraRepository.CrearTasasSimulacion(tasaCreate));
            return new Response<TasaSimuladorDto> { Data = response };
        }


        /// <summary>
        /// Elimina un registro de SIM_SIMULADOR
        /// </summary>
        /// <param name="tasas"></param>
        /// /// <Author>Sergio ortega</Author>
        /// <Date>03/12/2021</Date>
        /// <returns></returns>
        public async Task<Response<TasaSimuladorDto>> EliminarRegistroSimuladorPorId(TasaSimuladorDto tasas)
        {
            var tasaDelete = this._mapper.Map<TasaSimulador>(tasas);

            var response = this._mapper.Map<TasaSimuladorDto>(await this._carteraRepository.EliminarRegistroSimuladorPorId(tasaDelete));

            return new Response<TasaSimuladorDto> { Data = response };
        }

        /// <summary>
        /// Obtiene todos los registros de las excepciones
        /// </summary>
        /// <param name="consultarExcepciones"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>03/12/2021</Date>

        public async Task<Response<IEnumerable<TasaSimuladorDto>>> ConsultarTipoActorById(TasaSimuladorDto tasas)

        {
            var consultaExcepcion = this._mapper.Map<TasaSimulador>(tasas);

            var response = this._mapper.Map<IEnumerable<TasaSimuladorDto>>(await this._carteraRepository.ConsultarTipoActorPorId(consultaExcepcion));
            return new Response<IEnumerable<TasaSimuladorDto>> { Data = response };
        }


        /// <summary>
        /// Obtiene todos los tipos de actores
        /// </summary>
        /// <param name="obtenerTipoActores"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>29/12/2021</Date>

        public async Task<Response<IEnumerable<TipoActorDto>>> ObtenerTipoActor()

        {
            var response = this._mapper.Map<IEnumerable<TipoActorDto>>(await this._carteraRepository.ObtenerTipoActor());
            return new Response<IEnumerable<TipoActorDto>> { Data = response };
        }

        /// <summary>
        /// Crear actor Simulacion
        /// </summary>
        /// <param name="tasas"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<ParametrosTasaActorDto>> CrearTasaActor(ParametrosTasaActorDto actores)
        {
            var actorCreate = this._mapper.Map<ParametrosTasaActor>(actores);

            var response = this._mapper.Map<ParametrosTasaActorDto>(await this._carteraRepository.CrearTasaActor(actorCreate));
            return new Response<ParametrosTasaActorDto> { Data = response };
        }

        /// <summary>
        /// Actualiza parametros simulador actor
        /// </summary>
        /// <param name="tasas"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<ParametrosTasaActorDto>> ActualizarTasaSimuladorActor(ParametrosTasaActorDto actores)
        {
            var actorUpdate = this._mapper.Map<ParametrosTasaActor>(actores);

            var response = this._mapper.Map<ParametrosTasaActorDto>(await this._carteraRepository.ActualizarTasaSimuladorActor(actorUpdate));
            return new Response<ParametrosTasaActorDto> { Data = response };
        }

        /// <summary>
        /// Elimina un registro de PAR_TASA_SIMULADOR_ACTOR
        /// </summary>
        /// <param name="tasas"></param>
        /// /// <Author>Sergio ortega</Author>
        /// <Date>05/01/2022</Date>
        /// <returns></returns>
        public async Task<Response<ParametrosTasaActorDto>> EliminarParametroSimuladorActor(ParametrosTasaActorDto actores)
        {
            var actorDelete = this._mapper.Map<ParametrosTasaActor>(actores);

            var response = this._mapper.Map<ParametrosTasaActorDto>(await this._carteraRepository.EliminarParametroSimuladorActor(actorDelete));

            return new Response<ParametrosTasaActorDto> { Data = response };

        }

        /// <summary>
        /// Consulta las unidades ejecutoras
        /// </summary>
        /// <param name="tasas"></param>
        /// /// <Author>Sergio ortega</Author>
        /// <Date>09/01/2022</Date>
        /// <returns></returns>
        public async Task<Response<IEnumerable<UnidadesEjecutorasDto>>> ObtenerUnidadesEjecutorasPorId(UnidadesEjecutorasDto unidades)
        {
            var consultaUnidadEjecutora = this._mapper.Map<UnidadesEjecutoras>(unidades);

            var response = this._mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(await this._carteraRepository.ObtenerUnidadesEjecutorasPorId(consultaUnidadEjecutora));

            return new Response<IEnumerable<UnidadesEjecutorasDto>> { Data = response };
        }
        /// <summary>
        /// Crea parametros unidad ejecutora
        /// </summary>
        /// <param name="unidadesEjecutoras"></param>
        /// /// /// <Author>Sergio ortega</Author>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<Response<UnidadesEjecutorasDto>> CrearUnidadEjecutoraSimulador(UnidadesEjecutorasDto unidades)
        {
            var unidadCreate = this._mapper.Map<UnidadesEjecutoras>(unidades);

            var response = this._mapper.Map<UnidadesEjecutorasDto>(await this._carteraRepository.CrearUnidadEjecutoraSimulador(unidadCreate));

            return new Response<UnidadesEjecutorasDto> { Data = response };

        }
        /// <summary>
        /// Consulta nombres de las unidades ejecutoras
        /// </summary>
        /// <param name="unidadesEjecutoras"></param>
        /// <Author>Sergio ortega</Author>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<Response<IEnumerable<UnidadesEjecutorasDto>>> ConsultarUnidadEjecutora()

        {
            var response = this._mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(await this._carteraRepository.ConsultarUnidadEjecutora());
            return new Response<IEnumerable<UnidadesEjecutorasDto>> { Data = response };
        }


        /// <summary>
        /// Actualiza parametros unidad ejecutora
        /// </summary>
        /// <param name="unidades"></param>
        /// <Author>Sergio ortega</Author>
        /// <returns></returns>        
        public async Task<Response<UnidadesEjecutorasDto>> ActualizarUnidadEjecutoraSimulador(UnidadesEjecutorasDto unidades)
        {
            var unidadUpdate = this._mapper.Map<UnidadesEjecutoras>(unidades);

            var response = this._mapper.Map<UnidadesEjecutorasDto>(await this._carteraRepository.ActualizarUnidadEjecutoraSimulador(unidadUpdate));
            return new Response<UnidadesEjecutorasDto> { Data = response };
        }

        /// <summary>
        /// Elimina un registro de PAR_TASA_SIMULADOR_UNDEJ
        /// </summary>
        /// <param name="tasas"></param>
        /// /// <Author>Sergio ortega</Author>
        /// <Date>012/01/2022</Date>
        /// <returns></returns>
        public async Task<Response<UnidadesEjecutorasDto>> EliminarUnidadEjecutoraSimulador(UnidadesEjecutorasDto unidades)
        {
            var unidadEjecutoraDelete = this._mapper.Map<UnidadesEjecutoras>(unidades);

            var response = this._mapper.Map<UnidadesEjecutorasDto>(await this._carteraRepository.EliminarUnidadEjecutoraSimulador(unidadEjecutoraDelete));

            return new Response<UnidadesEjecutorasDto> { Data = response };

        }

        /// <summary>
        /// Obtiene todos los parametros generales de credito y cartera
        /// </summary>
        /// <param name="obtenerParametrosGeneralesCreditoYCartera"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>29/12/2021</Date>

        public async Task<Response<ParamGeneralesCreditoYCarteraDto>> ObtenerParamGeneralesCreditoYCartera()

        {
            var response = this._mapper.Map<ParamGeneralesCreditoYCarteraDto>(await this._carteraRepository.ObtenerParamGeneralesCreditoYCartera());
            return new Response<ParamGeneralesCreditoYCarteraDto> { Data = response };
        }

        /// <summary>
        /// Actualiza parametros generales de credito y cartera
        /// </summary>
        /// <param name="parametros"></param>
        ///  <Author>Sergio ortega</Author>
        /// <returns></returns>
        public async Task<Response<ParamGeneralesCreditoYCarteraDto>> ActualizarParamGeneralesCreditoYCartera(ParamGeneralesCreditoYCarteraDto parametros)
        {
            var paramGeneralesUpdate = this._mapper.Map<ParamGeneralesCreditoYCartera>(parametros);
            var tasa = parametros.valorTasaUsura / 100;
            var tasaUsuara = (decimal)Math.Pow(1 + (double)tasa, 1.0 / 12.0) - 1;
            paramGeneralesUpdate.PRM_TASA_USURA_PERIODICA = tasaUsuara * 100;
            var response = this._mapper.Map<ParamGeneralesCreditoYCarteraDto>(await this._carteraRepository.ActualizarParamGeneralesCreditoYCartera(paramGeneralesUpdate));
            return new Response<ParamGeneralesCreditoYCarteraDto> { Data = response };
        }

        /// <summary>
        /// Cargar Archivo Aplicacion Pago Masivo
        /// </summary>
        /// <param name="unidadEjecutora"></param>
        ///  <Author>Sergio ortega</Author>
        /// <returns></returns>
        public async Task<Response<List<AplicacionPagoDto>>> CargarArchivoAplicacionPagoMasivo(IFormFile request, int unidadEjecutora)
        {
            var extensionArchivo = request.FileName.Split('.')[1];

            var stream = (new StreamReader(request.OpenReadStream()));
            List<Response<AplicacionPagoDto>> listaPagosAplicados = new List<Response<AplicacionPagoDto>>();
            var idUnidadEje = string.Empty;
            var listaUnidades = (await ConsultarUnidadEjecutora()).Data;
            UnidadesEjecutorasDto unidadEjecutora_ = listaUnidades.FirstOrDefault(x => x.idUnidadEjecutora == unidadEjecutora);
            var validacionViabilidad = new ResponseFromValidation();
            if (extensionArchivo.Equals("xlx") || extensionArchivo.Equals("xlsx"))
            {

                var FileJson = await HandlerFileToJson.ConvertXLXtoJson(stream);
                IEnumerable<PagoMasivoNominaDto> listaRegistros = JsonConvert.DeserializeObject<IEnumerable<PagoMasivoNominaDto>>(FileJson);
                idUnidadEje = listaRegistros.ToList()[0].NIT;
                unidadEjecutora_ = (await this.ConsultarUnidadEjecutora()).Data.Where(x => x.NitUnidadEjecutora.ToString() == idUnidadEje.ToString()).FirstOrDefault();
                List<KeyValuePair<string, string>> diccionarioPagos = new List<KeyValuePair<string, string>>();
                foreach (var registro in listaRegistros)
                {
                    diccionarioPagos.Add(new KeyValuePair<string, string>(registro.IDENTIFICACION, registro.VALOR.ToString()));
                }

                validacionViabilidad = (await this.ValidarViabilidadPagoMasivo(diccionarioPagos)).Data;
                if (validacionViabilidad.ValidezLista != false)
                {
                    foreach (var item in diccionarioPagos)
                    {
                        listaPagosAplicados.Add(await this.AplicarPago(new PagoAplicarDto { Identificacion = item.Key, ValorPago = decimal.Parse(item.Value) }));
                    }
                }
                else
                {
                    var listaClientes = string.Empty;
                    foreach (var item in validacionViabilidad.ListaPagos)
                    {

                        listaClientes += listaClientes == string.Empty ? item.Key : $", {item.Key}";
                    }
                    throw new Exception($"No existen creditos en la base de datos para los {validacionViabilidad.ListaPagos.Count} clientes: " + listaClientes);
                }


            }
            else if (extensionArchivo.Equals("csv"))
            {
                var FileJson = await HandlerFileToJson.ConvertCSVtoJson(stream);
                IEnumerable<PagoMasivoNominaDto> listaRegistros = JsonConvert.DeserializeObject<IEnumerable<PagoMasivoNominaDto>>(FileJson);
                List<KeyValuePair<string, string>> diccionarioPagos = new List<KeyValuePair<string, string>>();


                idUnidadEje = listaRegistros.ToList()[0].FUERZA;
                unidadEjecutora_ = (await this.ConsultarUnidadEjecutora()).Data.Where(x => x.nombreUnidadEjecutora.ToUpper() == idUnidadEje.ToUpper()).FirstOrDefault();
                foreach (var registro in listaRegistros)
                {
                    diccionarioPagos.Add(new KeyValuePair<string, string>(registro.CEDULA, registro.VALOR.ToString()));
                }

                validacionViabilidad = (await this.ValidarViabilidadPagoMasivo(diccionarioPagos)).Data;
                if (validacionViabilidad.ValidezLista != false)
                {
                    foreach (var item in diccionarioPagos)
                    {
                        //listaPagosAplicados.Add(await this.AplicarPago(new PagoAplicarDto { Identificacion = item.Key, ValorPago = decimal.Parse(item.Value) })); DESCOMENTAR
                        listaPagosAplicados.Add(new Response<AplicacionPagoDto> { Data = new AplicacionPagoDto { ValorPago = decimal.Parse(item.Value) } });
                    }
                }
                else
                {
                    var listaClientes = string.Empty;
                    foreach (var item in validacionViabilidad.ListaPagos)
                    {

                        listaClientes += listaClientes == string.Empty ? item.Key : $", {item.Key}";
                    }
                    throw new Exception($"No existen creditos en la base de datos para los {validacionViabilidad.ListaPagos.Count} clientes: " + listaClientes);
                }
            }
            else if (extensionArchivo.Equals("txt"))
            {
                var FileJson = await HandlerFileToJson.ConvertTxtToJson(stream);
                List<KeyValuePair<string, string>> listaPagos = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(FileJson.Item1);

                var unidades = JsonConvert.DeserializeObject<List<string>>(FileJson.Item2);
                foreach (var unidad in unidades)
                {
                    unidadEjecutora_ = (await this.ConsultarUnidadEjecutora()).Data.Where(x => x.SiglaUnidadEjecutora.ToUpper() == unidad.ToUpper()).FirstOrDefault();
                    if (unidadEjecutora_ != null)
                    {
                        throw new ArgumentException(typeof(UnidadesEjecutorasDto).ToString());
                    }
                }



                validacionViabilidad = (await this.ValidarViabilidadPagoMasivo(listaPagos)).Data;
                if (validacionViabilidad.ValidezLista != false)
                {
                    foreach (var item in listaPagos)
                    {
                        listaPagosAplicados.Add(await this.AplicarPago(new PagoAplicarDto { Identificacion = item.Key, ValorPago = decimal.Parse(item.Value) }));
                    }
                }
                else
                {
                    var listaClientes = string.Empty;
                    foreach (var item in validacionViabilidad.ListaPagos)
                    {

                        listaClientes += listaClientes == string.Empty ? item.Key : $", {item.Key}";
                    }
                    throw new Exception($"No existen creditos en la base de datos para los {validacionViabilidad.ListaPagos.Count} clientes: " + listaClientes);
                }

            }
            else if (extensionArchivo.ToLower().Equals("pdf"))
            {
                var jsonFile = await HandlerFileToJson.ConvertPdfToJson(stream.BaseStream);

                var listaPagos = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(jsonFile);

                validacionViabilidad = (await this.ValidarViabilidadPagoMasivo(listaPagos)).Data;
                if (validacionViabilidad.ValidezLista != false)
                {
                    foreach (var item in listaPagos)
                    {
                        listaPagosAplicados.Add(await this.AplicarPago(new PagoAplicarDto { Identificacion = item.Key, ValorPago = decimal.Parse(item.Value) }));
                    }
                }
                else
                {
                    var listaClientes = string.Empty;
                    foreach (var item in validacionViabilidad.ListaPagos)
                    {

                        listaClientes += listaClientes == string.Empty ? item.Key : $", {item.Key}";
                    }
                    throw new Exception($"No existen creditos en la base de datos para los {validacionViabilidad.ListaPagos.Count} clientes: " + listaClientes);
                }
            }
            var listaResult = new List<AplicacionPagoDto>();
            foreach (var registro in listaPagosAplicados)
            {
                listaResult.Add(registro.Data);
            }
            return new Response<List<AplicacionPagoDto>>() { Data = listaResult };
        }

        private async Task<Response<ResponseFromValidation>> ValidarViabilidadPagoMasivo(List<KeyValuePair<string, string>> diccionarioPagos)
        {
            ResponseFromValidation responseValidation = new ResponseFromValidation();
            var dictionaryTrue = new List<KeyValuePair<string, string>>();
            var dictionaryFalse = new List<KeyValuePair<string, string>>();
            var listPersonalData = new List<SimulacionDatosPersonalesDto>();
            var datosPersonales = new SimulacionDatosPersonalesDto();
            foreach (var item in diccionarioPagos)
            {
                var credit = (await this.ObtenerCreditoCarteraByIdentificacion(item.Key)).Data;

                datosPersonales = (await this._creditoBusinessLogic.ObtenerSimulacionPersonales(item.Key)).Data;
                if (credit != null && datosPersonales != null)
                {
                    dictionaryTrue.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                    listPersonalData.Add(datosPersonales);
                }
                else
                {
                    dictionaryFalse.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                }

            }
            var finalDictionaryFalse = new List<KeyValuePair<string, string>>();
            foreach (var item in dictionaryFalse)
            {
                try
                {
                    var reintento = (await this.PrepararEjecucionDesembolso(new SolicitudProductoPorClienteDto { Identificacion = item.Key, TipoIdentificacion = 1 })).Data;
                    dictionaryTrue.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                    listPersonalData.Add((await this._creditoBusinessLogic.ObtenerSimulacionPersonales(item.Key)).Data);
                }
                catch (Exception)
                {
                    finalDictionaryFalse.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                }
            }
            var dataIfFalse = new ResponseFromValidation()
            {
                ListaDatosPersonales = listPersonalData,
                ListaPagos = finalDictionaryFalse,
                ValidezLista = false,
                UnidadEjecutora = null
            };
            var dataIfTrue = new ResponseFromValidation()
            {
                ListaDatosPersonales = listPersonalData,
                ListaPagos = finalDictionaryFalse,
                ValidezLista = true,
                UnidadEjecutora = null
            };

            Response<ResponseFromValidation> response = finalDictionaryFalse.Count() > 0 ? new Response<ResponseFromValidation>() { Data = dataIfFalse } : new Response<ResponseFromValidation>() { Data = dataIfTrue };
            return response;
        }

        internal class ResponseFromValidation
        {
            public List<KeyValuePair<string, string>> ListaPagos { get; set; }
            public bool ValidezLista { get; set; }
            public List<SimulacionDatosPersonalesDto> ListaDatosPersonales { get; set; }
            public UnidadEjecutoraDto UnidadEjecutora { get; set; }
        }

        /// <summary>
        /// Obtiene todos los plazos generales de los creditos
        /// </summary>
        /// <param name="obtenerTipoActores"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>08/02/2022</Date>

        public async Task<Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>>> ObtenerParametrosGeneralesPlazoCredito()

        {
            var response = this._mapper.Map<IEnumerable<ParamGeneralesExcepcionPlazoDto>>(await this._carteraRepository.ObtenerParametrosGeneralesPlazoCredito());
            return new Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>> { Data = response };
        }

        /// <summary>
        /// Crear expecion plazo
        /// </summary>
        /// <param name="plazoCreate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<ParamGeneralesExcepcionPlazoDto>> CrearExcepcionPlazo(ParamGeneralesExcepcionPlazoDto plazoCreate)
        {
            var plazoCreate_ = this._mapper.Map<ParamGeneralesExcepcionPlazo>(plazoCreate);

            var response = this._mapper.Map<ParamGeneralesExcepcionPlazoDto>(await this._carteraRepository.CrearExcepcionPlazo(plazoCreate_));
            return new Response<ParamGeneralesExcepcionPlazoDto> { Data = response };
        }

        /// <summary>
        /// Obtiene todos los plazos generales de los creditos
        /// </summary>
        /// <param name="obtenerTipoActores"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>08/02/2022</Date>

        public async Task<Response<ParamGeneralesExcepcionPlazoDto>> ActualizarParametrosGeneralesPlazoCredito(ParamGeneralesExcepcionPlazoDto plazos)
        {
            var plazoUpdate = this._mapper.Map<ParamGeneralesExcepcionPlazo>(plazos);

            var response = this._mapper.Map<ParamGeneralesExcepcionPlazoDto>(await this._carteraRepository.ActualizarParametrosGeneralesPlazoCredito(plazoUpdate));
            return new Response<ParamGeneralesExcepcionPlazoDto> { Data = response };
        }


        /// <summary>
        /// Elimina un registro de PARAM_PLAZOS
        /// </summary>
        /// <param name="plazoDelete"></param>
        /// /// <Author>Sergio ortega</Author>
        /// <Date>11/02/2022</Date>
        /// <returns></returns>
        public async Task<Response<ParamGeneralesExcepcionPlazoDto>> EliminarExcepcionPlazo(ParamGeneralesExcepcionPlazoDto plazoDelete)
        {
            var plazoDelete_ = this._mapper.Map<ParamGeneralesExcepcionPlazo>(plazoDelete);

            var response = this._mapper.Map<ParamGeneralesExcepcionPlazoDto>(await this._carteraRepository.EliminarExcepcionPlazo(plazoDelete_));

            return new Response<ParamGeneralesExcepcionPlazoDto> { Data = response };

        }


        /// <summary>
        /// Actualiza una excepcion del plazo
        /// </summary>
        /// <param name="plazos"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>08/02/2022</Date>

        public async Task<Response<ParamGeneralesExcepcionPlazoDto>> ActualizarExcepcionesPlazo(ParamGeneralesExcepcionPlazoDto plazos)
        {
            var excepcionUpdate = this._mapper.Map<ParamGeneralesExcepcionPlazo>(plazos);

            var response = this._mapper.Map<ParamGeneralesExcepcionPlazoDto>(await this._carteraRepository.ActualizarExcepcionesPlazo(excepcionUpdate));
            return new Response<ParamGeneralesExcepcionPlazoDto> { Data = response };
        }

        /// <summary>
        /// Obtiene todos las excepciones de los plazos
        /// </summary>
        /// <param name="obtenerTipoActores"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>14/02/2022</Date>

        public async Task<Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>>> ObtenerExcepcionesPlazo(ParamGeneralesExcepcionPlazoDto getPlazo)

        {
            var obtenerPlazos = this._mapper.Map<ParamGeneralesExcepcionPlazo>(getPlazo);

            var response = this._mapper.Map<IEnumerable<ParamGeneralesExcepcionPlazoDto>>(await this._carteraRepository.ObtenerExcepcionesPlazo(obtenerPlazos));
            return new Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>> { Data = response };
        }

        /// <summary>
        /// Obtiene todos las excepciones de los plazos por unidad ejecutora
        /// </summary>
        /// <param name="obtenerTipoActores"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>21/02/2022</Date>

        public async Task<Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>>> ObtenerExcepcionPlazoPorUnidadEjecutora(ParamGeneralesExcepcionPlazoDto getPlazo)

        {
            var obtenerPlazos = this._mapper.Map<ParamGeneralesExcepcionPlazo>(getPlazo);

            var response = this._mapper.Map<IEnumerable<ParamGeneralesExcepcionPlazoDto>>(await this._carteraRepository.ObtenerExcepcionPlazoPorUnidadEjecutora(obtenerPlazos));
            return new Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>> { Data = response };
        }

        /// <summary>
        /// Crear expecion plazo por unidad ejecutora
        /// </summary>
        /// <param name="plazoCreate"></param>
        /// <returns></returns>
        public async Task<Response<ParamGeneralesExcepcionPlazoDto>> CrearExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazoDto plazoCreate)
        {
            var plazoCreate_ = this._mapper.Map<ParamGeneralesExcepcionPlazo>(plazoCreate);

            var response = this._mapper.Map<ParamGeneralesExcepcionPlazoDto>(await this._carteraRepository.CrearExcepcionPlazoUnidadEjecutora(plazoCreate_));
            return new Response<ParamGeneralesExcepcionPlazoDto> { Data = response };
        }


        /// <summary>
        /// Actualiza una excepcion del plazo por unidad ejecutora
        /// </summary>
        /// <param name="plazoUpdate"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>22/02/2022</Date>

        public async Task<Response<ParamGeneralesExcepcionPlazoDto>> ActualizarExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazoDto plazoUpdate)
        {
            var excepcionUpdate = this._mapper.Map<ParamGeneralesExcepcionPlazo>(plazoUpdate);

            var response = this._mapper.Map<ParamGeneralesExcepcionPlazoDto>(await this._carteraRepository.ActualizarExcepcionPlazoUnidadEjecutora(excepcionUpdate));
            return new Response<ParamGeneralesExcepcionPlazoDto> { Data = response };
        }

        /// <summary>
        /// Obtiene todos los plazos generales de los seguros
        /// </summary>
        /// <param name="obtenerTipoActores"></param>
        /// <returns></returns>
        /// <Author>Sergio ortega</Author>
        /// <Date>22/02/2022</Date>

        public async Task<Response<IEnumerable<ParamGeneralesSegurosDto>>> ObtenerSeguroGeneral()
        {
            var response = this._mapper.Map<IEnumerable<ParamGeneralesSegurosDto>>(await this._carteraRepository.ObtenerSeguroGeneral());
            return new Response<IEnumerable<ParamGeneralesSegurosDto>> { Data = response };
        }

        public async Task<Response<ParamGeneralesSegurosDto>> ActualizarSeguroGeneral(ParamGeneralesSegurosDto seguroUpdate)
        {
            var actualizarSeguro = this._mapper.Map<ParamGeneralesSeguros>(seguroUpdate);
            var response = this._mapper.Map<ParamGeneralesSegurosDto>(await this._carteraRepository.ActualizarSeguroGeneral(actualizarSeguro));

            return new Response<ParamGeneralesSegurosDto> { Data = response };
        }

        public async Task<Response<IEnumerable<ParamGeneralesSegurosDto>>> ObtenerExcepSeguroPorMunicipio()
        {
            var ciudades = (await _catalogosBusinessLogic.ObtenerCiudades()).Data;
            var departamentos = (await _catalogosBusinessLogic.ObtenerDepartamentos()).Data;
            var response = this._mapper.Map<IEnumerable<ParamGeneralesSegurosDto>>(await this._carteraRepository.ObtenerExcepSeguroPorMunicipio());
            foreach (var respuesta in response)
            {
                respuesta.CiudadNombre = ciudades.FirstOrDefault(c => c.IdDepartamento == respuesta.departamentoId && c.Id == respuesta.ciudadId).Nombre;
                respuesta.DepartamentoNombre = departamentos.FirstOrDefault(c => c.Id == respuesta.departamentoId).Nombre;
            }
            return new Response<IEnumerable<ParamGeneralesSegurosDto>> { Data = response };
        }

        public async Task<Response<ParamGeneralesSegurosDto>> CrearExcepcionPorMunicipio(ParamGeneralesSegurosDto excepcionCreate)

        {
            var crearExcepcionSeguroMunicipio = this._mapper.Map<ParamGeneralesSeguros>(excepcionCreate);
            var response = this._mapper.Map<ParamGeneralesSegurosDto>(await this._carteraRepository.CrearExcepcionPorMunicipio(crearExcepcionSeguroMunicipio));
            return new Response<ParamGeneralesSegurosDto> { Data = response };
        }

        public async Task<Response<ParamGeneralesSegurosDto>> ActualizarExcepSeguroPorMunicipio(ParamGeneralesSegurosDto excepcionUpdate)
        {
            var actualizarExcepcionSeguro = this._mapper.Map<ParamGeneralesSeguros>(excepcionUpdate);
            var response = this._mapper.Map<ParamGeneralesSegurosDto>(await this._carteraRepository.ActualizarExcepSeguroPorMunicipio(actualizarExcepcionSeguro));
            return new Response<ParamGeneralesSegurosDto> { Data = response };
        }

        public async Task<Response<ParamGeneralesSegurosDto>> EliminarExcepcionSeguroPorMunicipio(ParamGeneralesSegurosDto excepcionDelete)
        {
            var eliminarExcepcionSeguroMunicipio = this._mapper.Map<ParamGeneralesSeguros>(excepcionDelete);
            var response = this._mapper.Map<ParamGeneralesSegurosDto>(await this._carteraRepository.EliminarExcepcionSeguroPorMunicipio(eliminarExcepcionSeguroMunicipio));
            return new Response<ParamGeneralesSegurosDto> { Data = response };
        }

        /// <summary>
        /// Carga archivo masivo leasing en tabla cre credito
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> CargaMasivaLeasing(IFormFile request)
        {
            var TiposVivienda = (await _catalogosBusinessLogic.ObtenerTipoVivienda()).Data;
            var EstadosVivienda = (await _catalogosBusinessLogic.ObtenerTiposDeVivienda()).Data;
            var parametrosGenerales = (await this.ObtenerParamGeneralesCreditoYCartera()).Data;

            var extensionArchivo = request.FileName.Split('.')[1];

            var stream = new StreamReader(request.OpenReadStream());
            if (extensionArchivo.Equals("xlx") || extensionArchivo.Equals("xlsx"))
            {
                var FileJson = (await HandlerFileToJson.ConvertXLXtoJson(stream)).Replace("\r", "").Replace("\n", "").Replace(@"\/", "/");
                var jsonUnescaped = Regex.Replace(FileJson, " \"", "\"");
                var listaLeasigs = JsonConvert.DeserializeObject<IEnumerable<CargueMasivoLeasingDto>>(jsonUnescaped);
                foreach (var leasing in listaLeasigs)
                {
                    try
                    {
                        int estadoVivienda = 0;
                        if (leasing.Estado_vivienda != null)
                        {
                            estadoVivienda = leasing.Estado_vivienda.ToUpper().Equals("NUEVA") ? 3 : 4;
                        }
                        else
                        {
                            estadoVivienda = 4;
                        }

                        bool vis = (double.Parse(leasing.Valor_Comercial_inmueble) / double.Parse(parametrosGenerales.parametroSalarioMinimo.ToString())) >
                                        double.Parse(parametrosGenerales.limiteSalariosVis.ToString()) ? false : true;

                        //Prueba de variables
                        // Dejar todos en var

                        //var fechafinpagos = DateTime.Parse(leasing.Fecha_Finaliza_Pagos);
                        //var fechaUltimoPago = DateTime.Parse(leasing.Fecha_Finaliza_Pagos);
                        //var categoria = leasing.Categoria;

                        //Tasa efectiva anual calculada
                        var tea = (decimal.Parse(leasing.tasa_Credito) / 100) * 12;
                        var tem = (decimal.Parse(leasing.tasa_Credito) / 100);
                        var producto = new ProductoCliente()
                        {
                            CRE_CANON_EXTRAORDINARIO = decimal.Parse(leasing.Canon_Extraordinario),
                            CATEGORIA = leasing.Categoria,
                            CIUDAD_INMUEBLE = leasing.Ciudad_Inmueble,
                            CLI_EDAD = int.Parse(leasing.Edad),
                            CLI_IDENTIFICACION = leasing.Identificacion,
                            CRE_ACUMULADO_INTERES_CORRIENTE = decimal.Parse(leasing.Valor_Causación_Acumulada_Interes_Corriente),
                            CRE_ACUMULADO_INTERES_MORA = decimal.Parse(leasing.Valor_Causación_Acumulada_Interes_Remanente),
                            CRE_ACUMULADO_SEGURO_HOGAR = decimal.Parse(leasing.Valor_Causación_Seguro_Hogar),
                            CRE_ACUMULADO_SEGURO_VIDA = decimal.Parse(leasing.Valor_Causación_Seguro_Vida),
                            CRE_ALIVIO_FRECH = 0,
                            CRE_CREADO_POR = default(Guid),
                            CRE_DIAS_MORA = 0,
                            CRE_DIA_PAGO = DateTime.ParseExact(leasing.Fecha_proximo_Pago, "dd/MM/yyyy", null).Day,
                            CRE_DIRECCION_INMUEBLE = leasing.Dirección_Inmueble,
                            CRE_FECHA_CREACION = DateTime.Now,
                            CRE_FECHA_DESEMBOLSO = DateTime.ParseExact(leasing.Fecha_Desembolso, "dd/MM/yyyy", null),
                            CRE_FECHA_ESTADO = DateTime.Now,
                            CRE_FECHA_FINALIZA_PAGOS = DateTime.ParseExact(leasing.Fecha_Finaliza_Pagos, "dd/MM/yyyy", null),
                            CRE_FECHA_MODIFICACION = DateTime.Now,
                            CRE_FECHA_PROXIMO_PAGO = DateTime.ParseExact(leasing.Fecha_proximo_Pago, "dd/MM/yyyy", null),
                            CRE_FECHA_ULTIMO_PAGO = DateTime.ParseExact(leasing.Fecha_ultimo_Pago, "dd/MM/yyyy", null),
                            CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE = DateTime.ParseExact(leasing.Fecha_ultimo_Pago, "dd/MM/yyyy", null),
                            CRE_FECHA_ULTIMO_PAGO_INTERES_MORA = DateTime.ParseExact(leasing.Fecha_ultimo_Pago, "dd/MM/yyyy", null),
                            CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR = DateTime.ParseExact(leasing.Fecha_ultimo_Pago, "dd/MM/yyyy", null),
                            CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA = DateTime.ParseExact(leasing.Fecha_ultimo_Pago, "dd/MM/yyyy", null),
                            CRE_ID = Guid.NewGuid(),
                            CRE_MODIFICADO_POR = default(Guid),
                            CRE_NRO_CREDITO = long.Parse(leasing.Nro_Credito),
                            CRE_NUMERO_CUOTAS_CANCELADAS = int.Parse(leasing.Plazo) - int.Parse(leasing.Cuotas_Pendientes),
                            CRE_PLAZO_ACTUAL = int.Parse(leasing.Cuotas_Pendientes),
                            CRE_PLAZO_TOTAL = int.Parse(leasing.Plazo),
                            CRE_POR_CANON_INICIAL = decimal.Parse(leasing.Canon_inicial),
                            CRE_POR_OPCION_COMPRA = decimal.Parse(leasing.Valor_Opcion_Compra),
                            CRE_SALDO_CAPITAL = decimal.Parse(leasing.Saldo_Capital),
                            CRE_SALDO_CAPITAL_MORA = 0,//Preguntar -> queda en 0 
                            CRE_TASA_CREDITO_EA = tea,
                            CRE_TASA_CREDITO_PER = tem,//calcular
                            CRE_TASA_FRECH = 0, // calcular -> queda 0
                            CRE_TASA_SEGURO_HOGAR = decimal.Parse(leasing.Porc_Seguro_Hogar),
                            CRE_TASA_SEGURO_VIDA = decimal.Parse(leasing.Porc_Seguro_Vida),
                            CRE_TIPO_ABONO_EXTRA = "1", // preguntar, no puede ser nulo, en un principio todos estaran en 0
                            CRE_VALOR_ABONO_EXTRA = 0, // preguntar, queda en 0 
                            CRE_VALOR_ALIVIO_CUOTA = 0, //preguntar, tambien queda en 0
                            CRE_VALOR_CANON_INICIAL = decimal.Parse(leasing.Canon_inicial), //->desde aca
                            CRE_VALOR_CUOTA = decimal.Parse(leasing.Vlor_Cuota),
                            CRE_VALOR_DESEMBOLSO = decimal.Parse(leasing.Monto_Aprobado),
                            CRE_VALOR_DEUDA_REMANENTE = 0, // preguntar, dejar en 0
                            CRE_VALOR_OPCION_COMPRA = decimal.Parse(leasing.Valor_Opcion_Compra),
                            CRE_VALOR_VIVIENDA = decimal.Parse(leasing.Valor_Comercial_inmueble),
                            DESCRIPCION_INMUEBLE = leasing.Descripción_Inmueble,
                            ESCRITURA_INMUEBLE = leasing.Escritura_Inmueble,
                            ESC_ID = 1,
                            FECHA_ESCRITURA = DateTime.ParseExact(leasing.Fecha_Escritura, "dd/MM/yyyy", null),
                            FECHA_SOLICITUD = DateTime.ParseExact(leasing.Fecha_Solicitud, "dd/MM/yyyy", null),
                            GRADO = leasing.Grado,
                            NOTARIA = leasing.Notaria,
                            NUMERO_MATRICULA = leasing.Matricula_Inmueble,
                            SIJ_LINDEROS = leasing.Linderos_inmueble,
                            SOC_SCORE = int.Parse(leasing.ScoreTranunion),
                            TCR_ID = 1,
                            TIPO_IDENTIFICACION = 1,
                            TVV_ID = estadoVivienda, //preguntar,
                            TIV_ID = vis ? 1 : 2,//preguntar
                            CLASE_INMUEBLE = leasing.Clase_Inmueble,
                            CRE_FECHA_CAUSACION = DateTime.ParseExact(leasing.Fecha_Desembolso, "dd/MM/yyyy", null)

                        };

                        await _carteraRepository.EjecutarDesembolso(producto);
                    }
                    catch (Exception ex)
                    {
                        var log = ex;
                    }



                }



            }

            return new OkObjectResult("Ok");
        }

        /// <summary>
        /// Obtiene los datos de liquidacion general
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<DatosLiquidacionDto>> ObtenerLiquidacionGeneral()
        {
            var response = this._mapper.Map<DatosLiquidacionDto>(await this._carteraRepository.ObtenerLiquidacionGeneral());
            return new Response<DatosLiquidacionDto> { Data = response };
        }

        /// <summary>
        /// Actualizar Liquidacion General
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<DatosLiquidacionDto>> ActualizarLiquidacionGeneral(DatosLiquidacionDto request)
        {
            var peticion = this._mapper.Map<DatosLiquidacion>(request);
            var response = this._mapper.Map<DatosLiquidacionDto>(await this._carteraRepository.ActualizarLiquidacionGeneral(peticion));
            return new Response<DatosLiquidacionDto> { Data = response };
        }

        /// <summary>
        /// ObtenerAplicacionPago
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<IEnumerable<AplicacionPagoDto>>> ObtenerAplicacionPago(Guid request)
        {
            var response = this._mapper.Map<IEnumerable<AplicacionPagoDto>>(await this._carteraRepository.ObtenerAplicacionPago(request));
            return new Response<IEnumerable<AplicacionPagoDto>> { Data = response };
        }

        /// <summary>
        /// ObtenerCuotasAdelantadas
        /// </summary>
        /// <param name="idCredito"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<IEnumerable<CuotaAdelantadaDto>>> ObtenerCuotasAdelantadas(Guid idCredito)
        {
            var response = this._mapper.Map<IEnumerable<CuotaAdelantadaDto>>(await this._carteraRepository.ObtenerCuotasAdelantadas(idCredito));
            return new Response<IEnumerable<CuotaAdelantadaDto>> { Data = response };
        }



        /// <summary>
        /// AplicarCuotasAdelantadasByFechaActual
        /// </summary>
        /// <param name="FechaAplicacion"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<IEnumerable<responseAdelantadaFechaCierreDto>>> AplicarCuotasAdelantadasByFechaActual(DateTime FechaAplicacion, CancellationToken cancellationToken)
        {
            var cuotasAdelantadas = await this._carteraRepository.AplicarCuotasAdelantadasByFechaActual(FechaAplicacion);
            //var response = this._mapper.Map<IEnumerable<CuotaAdelantadaFechaCierreDto>>(cuotasAdelantadas);
            List<responseAdelantadaFechaCierreDto> insertResponses = new List<responseAdelantadaFechaCierreDto>();
            RequestCarteraAdelantoCuotaSeven datosRequest = new RequestCarteraAdelantoCuotaSeven();

            if (cuotasAdelantadas != null)
            {
                foreach (var item in cuotasAdelantadas)
                {
                    datosRequest.numeroCredito = item.CRE_NRO_CREDITO;
                    datosRequest.tipoCredito = item.TCR_ID;
                    datosRequest.NumeroDocumentoCliente = item.CLI_IDENTIFICACION;
                    datosRequest.tipoIdentificacion = item.TIPO_IDENTIFICACION;
                    datosRequest.cpgId = item.CPG_ID;
                    datosRequest.fechaCierre = FechaAplicacion;
                    Response<InsertarMovContableResponse> insertMovimientoResponse = await AdelantoCuotaCartera(datosRequest, cancellationToken);
                    int Retorno = GetValueResponseXmlSevenFactura(insertMovimientoResponse.Data.Body.InsertarMovContableResult, 2);
                    if (insertMovimientoResponse.Data != null && Retorno == 0)
                    {
                        var xml = XDocument.Parse(insertMovimientoResponse.Data.Body.InsertarMovContableResult);

                        var responseDatoUno = new responseAdelantadaFechaCierreDto
                        {
                            numeroCredito = item.CRE_NRO_CREDITO,
                            mensaje = insertMovimientoResponse.ReturnMessage,
                            integracionSeven = xml.Descendants("RETORNO").FirstOrDefault()?.Value == "0"
                        };

                        insertResponses.Add(responseDatoUno);

                    }
                    else
                    {

                        var responseDatoUno = new responseAdelantadaFechaCierreDto
                        {
                            numeroCredito = item.CRE_NRO_CREDITO,
                            mensaje = insertMovimientoResponse.ReturnMessage,
                            integracionSeven = false
                        };
                        insertResponses.Add(responseDatoUno);

                    }
                }
            }

            return new Response<IEnumerable<responseAdelantadaFechaCierreDto>> { Data = insertResponses };
        }

        /// <summary>
        /// Validar que los clientes del plano existan en BUA
        /// </summary>
        /// <param name="diccionarioPagos"></param>
        /// <returns></returns>
        public async Task<List<ArchivoAplicacionPagoTXTDto>> ClientesBUAAsync(List<ArchivoAplicacionPagoTXTDto> listArchivos, List<PagoMasivoNominaDto> listArchivosPagos)
        {
            List<ArchivoAplicacionPagoTXTDto> listUsuariosExistenBUA = new List<ArchivoAplicacionPagoTXTDto>();
            Response<ClienteDto> clienteDto = new Response<ClienteDto>();
            ArchivoAplicacionPagoTXTDto archivos = new ArchivoAplicacionPagoTXTDto();

            if (listArchivos != null && listArchivosPagos == null)
            {
                foreach (var item in listArchivos)
                {
                    clienteDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, string.Format("{0,10:G}", Convert.ToInt32(item.Documento.ToString())));

                    if (clienteDto.Data != null)
                    {
                        archivos.Documento = clienteDto.Data.Identificacion;
                        archivos.Valor = item.Valor;
                        listUsuariosExistenBUA.Add(archivos);
                    }

                }
            }

            if (listArchivosPagos != null && listArchivos == null)
            {
                foreach (var item in listArchivosPagos)
                {
                    clienteDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, string.Format("{0,10:G}", Convert.ToInt32(item.CEDULA.ToString())));

                    if (clienteDto.Data != null)
                    {
                        archivos.Documento = clienteDto.Data.Identificacion;
                        archivos.Valor = item.VALOR;
                        listUsuariosExistenBUA.Add(archivos);
                    }

                }
            }

            return listUsuariosExistenBUA;
        }

        /// <summary>
        /// CargarArchivoPagoMasivo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fechaAplicacion"></param>
        /// <returns></returns>
        public async Task<Response<List<ResultadoAplicacionPagoDto>>> CargarArchivoPagoMasivo(IFormFile request, ParametrosAplicacionPagoDto parametrosAplicacionPago)
        {
            var extensionArchivo = request.FileName.Split('.')[1];
            var stream = new StreamReader(request.OpenReadStream());
            List<Response<AplicacionPagoDto>> listaPagosAplicados = new();
            List<ArchivoAplicacionPagoTXTDto> listUsuariosExistenBUA = new();
            List<ArchivoAplicacionPagoExcelDto> listAplicacionPagosExcel = new();
            Response<List<ResultadoAplicacionPagoDto>> respuesta = new();
            List<ResultadoAplicacionPagoDto> listaPagosAplicacionPago = new();

            decimal cuotaTotal = 0;

            if (stream.Peek() == -1)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = ("El archivo está vacío. Por favor, seleccione un documento valido");
                return respuesta;
            }

            // Validacion de archivo xlx o xlsx
            if (parametrosAplicacionPago.FormatoAplicar == 3
                && (extensionArchivo.Equals("xlx") || extensionArchivo.Equals("xlsx")))
            {
                var FileJson = HandlerFileToJson.ConvertXLXtoList(stream);

                foreach (var item in FileJson.Hipotecario)
                {
                    cuotaTotal += decimal.Parse(item.ValorAplicado.Replace("$", "").Replace(",", ""));
                }

                foreach (var item in FileJson.Leasing)
                {
                    cuotaTotal += decimal.Parse(item.ValorAplicado.Replace("$", "").Replace(",", ""));
                }

                if (parametrosAplicacionPago.ValorPago == cuotaTotal)
                {
                    await _carteraRepository.CrearTempTableAplicacionPagos();
                    foreach (var item in FileJson.Hipotecario)
                    {
                        if (!string.IsNullOrEmpty(item.Documento))
                        {
                            PagoMasivoNominaDto pagoMasivo = new()
                            {
                                FUNCIONARIO = string.Empty,
                                CEDULA = item.Documento,
                                CONCEPTO_COD = item.Concepto,
                                VALOR = decimal.Parse(item.ValorAplicado.Replace("$", "").Replace(",", "")),
                            };

                            ResultadoAplicacionPagoDto result = _mapper.Map<ResultadoAplicacionPagoDto>(await _carteraRepository.ActualizarAplicacionDePagos(pagoMasivo, parametrosAplicacionPago));
                            listaPagosAplicacionPago.Add(result);
                        }
                    }

                    foreach (var item in FileJson.Leasing)
                    {
                        if (!string.IsNullOrEmpty(item.Documento))
                        {
                            PagoMasivoNominaDto pagoMasivo = new()
                            {
                                FUNCIONARIO = string.Empty,
                                CEDULA = item.Documento,
                                CONCEPTO_COD = item.Concepto,
                                VALOR = decimal.Parse(item.ValorAplicado.Replace("$", "").Replace(",", "")),
                            };

                            ResultadoAplicacionPagoDto result = _mapper.Map<ResultadoAplicacionPagoDto>(await _carteraRepository.ActualizarAplicacionDePagos(pagoMasivo, parametrosAplicacionPago));
                            listaPagosAplicacionPago.Add(result);
                        }
                    }
                }
                else
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = ($"El valor total ingresado: {parametrosAplicacionPago.ValorPago} no coincide con el valor total que se encuentra en el archivo.");
                    return respuesta;
                }
            }

            // Validacion de archivo plano
            if (parametrosAplicacionPago.FormatoAplicar == 2 && extensionArchivo.Equals("txt"))
            {
                List<ArchivoAplicacionPagoTXTDto> listArchivos = new();
                listArchivos = await HandlerFileToJson.ConvertTxtToList(stream);

                foreach (var item in listArchivos)
                {
                    cuotaTotal += item.Valor;
                }

                if (parametrosAplicacionPago.ValorPago == cuotaTotal)
                {
                    await _carteraRepository.CrearTempTableAplicacionPagos();
                    foreach (var item in listArchivos)
                    {
                        PagoMasivoNominaDto pagoMasivo = new()
                        {
                            FUNCIONARIO = item.Funcionario,
                            CEDULA = item.Documento,
                            CONCEPTO_COD = item.ConceptoCod.ToString(),
                            VALOR = item.Valor,
                        };

                        ResultadoAplicacionPagoDto result = _mapper.Map<ResultadoAplicacionPagoDto>(await _carteraRepository.ActualizarAplicacionDePagos(pagoMasivo, parametrosAplicacionPago));
                        listaPagosAplicacionPago.Add(result);
                    }
                }
                else
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = ($"El valor total ingresado: {parametrosAplicacionPago.ValorPago} no coincide con el valor total que se encuentra en el archivo.");
                    return respuesta;
                }
            }

            // Validacion de archivo separado por comas
            if (parametrosAplicacionPago.FormatoAplicar == 1 && extensionArchivo.Equals("csv"))
            {
                List<PagoMasivoNominaDto> listPagoMasivo = await HandlerFileToJson.ConvertCSVToList(stream);

                if (listPagoMasivo == null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = ("FORMATO DE ARCHIVO NO COINCIDE.");
                    return respuesta;
                }

                foreach (var item in listPagoMasivo)
                {
                    cuotaTotal += item.VALOR;
                }

                if (parametrosAplicacionPago.ValorPago == cuotaTotal)
                {
                    await _carteraRepository.CrearTempTableAplicacionPagos();
                    foreach (var item in listPagoMasivo)
                    {
                        ResultadoAplicacionPagoDto result = _mapper.Map<ResultadoAplicacionPagoDto>(await _carteraRepository.ActualizarAplicacionDePagos(item, parametrosAplicacionPago));
                        listaPagosAplicacionPago.Add(result);
                    }
                }
                else
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = ($"El valor total ingresado: {parametrosAplicacionPago.ValorPago} no coincide con el valor total que se encuentra en el archivo.");
                    return respuesta;
                }
            }

            // Validacion de archivo separado por comas para listas internas
            if ((parametrosAplicacionPago.FormatoAplicar == 4 || parametrosAplicacionPago.FormatoAplicar == 3)
                && extensionArchivo.Equals("csv"))
            {
                List<CargueArchivoListaInterna> listPagoMasivo = await HandlerFileToJson.ConvertCSVToInternList(stream);

                if (listPagoMasivo == null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = ("FORMATO DE ARCHIVO NO COINCIDE.");
                    return respuesta;
                }

                foreach (var item in listPagoMasivo)
                {
                    cuotaTotal += item.VALOR;
                }

                if (parametrosAplicacionPago.ValorPago == cuotaTotal)
                {
                    await _carteraRepository.CrearTempTableAplicacionPagos();
                    foreach (var item in listPagoMasivo)
                    {
                        PagoMasivoNominaDto pagoMasivo = new()
                        {
                            FUNCIONARIO = item.FUNCIONARIO,
                            CEDULA = item.CEDULA,
                            CONCEPTO_COD = item.CONCEPTO_COD.ToString(),
                            VALOR = item.VALOR,
                            CODIGO_MILITAR = item.CODIGO_MILITAR
                        };
                        ResultadoAplicacionPagoDto result = _mapper.Map<ResultadoAplicacionPagoDto>(await _carteraRepository.ActualizarAplicacionDePagos(pagoMasivo, parametrosAplicacionPago));
                        listaPagosAplicacionPago.Add(result);
                    }
                }
                else
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = ($"El valor total ingresado: {parametrosAplicacionPago.ValorPago} no coincide con el valor total que se encuentra en el archivo.");
                    return respuesta;
                }
            }

            //Se verifica si se proceso algun pago
            if (listaPagosAplicacionPago.Count == 0 && parametrosAplicacionPago.AplicanPagos)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = ($"No se procesó ningún pago. Por favor, intentelo más tarde.");
                return respuesta;
            }

            return new Response<List<ResultadoAplicacionPagoDto>>() { Data = listaPagosAplicacionPago };
        }

        public async Task<Response<IEnumerable<ResultadoAplicacionPagoDto>>> AplicarPagoMasivoDesdeLista(int idProceso, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await ObtenerProcesoAplicacionPagosConDetalle(new() { IdProceso = idProceso });
                var procesoPago = response.FirstOrDefault();

                if (procesoPago == null)
                {
                    return new Response<IEnumerable<ResultadoAplicacionPagoDto>>()
                    {
                        IsSuccess = false,
                        ReturnMessage = "No existe la lista"
                    };
                }

                if (procesoPago.Estado != "PENDIENTE")
                {
                    return new Response<IEnumerable<ResultadoAplicacionPagoDto>>()
                    {
                        IsSuccess = false,
                        ReturnMessage = $"No se puede aplicar pagos de una lista que no este en estado PENDIENTE"
                    };
                }

                var datosPago = procesoPago.Pagos;

                if (!datosPago.Any())
                {
                    return new Response<IEnumerable<ResultadoAplicacionPagoDto>>()
                    {
                        IsSuccess = false,
                        ReturnMessage = $"La Lista no tiene pagos relacionados"
                    };
                }



                var parametrosAplicacionPago = new ParametrosAplicacionPagoMasivoDto()
                {
                    IdDetalles = datosPago.Select(x => x.IdDetalle).ToList(),
                    IdProceso = idProceso,
                    CreadoPor = userId,
                    AplicanPagos = true,
                };

                var result = _mapper.Map<IEnumerable<ResultadoAplicacionPagoDto>>(await _carteraRepository.ActualizarAplicacionDePagos(parametrosAplicacionPago));


                return result.BuildSuccess();
            }
            catch (SqlException ex)
            {
                var detalle = new StringBuilder();
                detalle.AppendLine(ex.Message);
                detalle.AppendLine("Linea: " + ex.LineNumber.ToString());

                foreach (SqlError sqlError in ex.Errors)
                {
                    detalle.AppendLine();
                    detalle.AppendLine(sqlError.Message);
                    detalle.AppendLine("Procedure: " + sqlError.Procedure);
                    detalle.AppendLine("Linea: " + sqlError.LineNumber);
                }

                detalle.AppendLine();
                detalle.AppendLine(ex.StackTrace);

                // Para segumiento de error
                return new Response<IEnumerable<ResultadoAplicacionPagoDto>>()
                {
                    IsSuccess = false,
                    ReturnMessage = detalle.ToString()
                };
            }
            catch (Exception ex)
            {
                // Para segumiento de error
                return new Response<IEnumerable<ResultadoAplicacionPagoDto>>()
                {
                    IsSuccess = false,
                    ReturnMessage = ex.StackTrace
                };
            }

        }
        public async Task<Response<InsertarMovContableResponse>> AplicarPagoMasivoMovContable(RequestPagoMasivoSevenDto Proceso, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();

            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecarioCuantaOrden = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            List<ResponseMov> detalleMovContaLeasing = new List<ResponseMov>();
            List<ResponseMov> detalleMovContaHipotecario = new List<ResponseMov>();
            List<ResponseMov> detalleMovContaHipotecarioCuantaOrden = new List<ResponseMov>();
            List<ResponseMov> items = new List<ResponseMov>();
            List<PagoListaDetalleDto> Pagos = new List<PagoListaDetalleDto>();
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());

            var responsePagos = await this._carteraRepository.ObtenerProcesoAplicacionPagosConDetalleSeven(Proceso.IdProceso);

            //var creditosLeasing = responsePagos.Where(x => x.Concepto == "LEASING HABITACIONAL");
            var creditosLeasing = responsePagos.Where(x => x.Concepto.ToUpper().Contains("LEASING"));
            var creditosHipotecarios = responsePagos.Where(x => x.Concepto.ToUpper().Contains("HIPOTECARIO"));


            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responsePagos.Select(x => x.NroDocumento).Distinct().ToList())).Data;

            var list = responsePagos.GroupJoin(listAfiliados, r => r.NroDocumento, la => la.Documento, (causac, doc) =>
            {
                causac.Nombre = doc.FirstOrDefault()?.NombreCompleto ?? "";
                var unidadId = doc.FirstOrDefault()?.UnidadEjecutora ?? 0;
                var unidadEjecutoraNombre = unidadesEjecutoras.FirstOrDefault(ue => ue.Id == unidadId)?.AreaNegocio ?? 0;

                causac.UnidadEjecutora = unidadEjecutoraNombre;

                return causac;
            }).ToList();

            if (creditosLeasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);
                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9065;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = Proceso.FechaPago;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Pagos listas leasing habitacional " + Proceso.FechaPago.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                items = await ObtenerDeTalleCuotaMasivo(equivalenciaCuentasLeasing, creditosLeasing, unidadesEjecutoras);

                detalleMovContaLeasing.AddRange(items);


            }
            if (creditosHipotecarios.Count() > 0)
            {

                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);
                //var cuentasAfectar = equivalenciaCuentasHipotecario.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "desembolso"));
                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9003;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = Proceso.FechaPago;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Pagos listas hipotecario " + Proceso.FechaPago.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                items = await ObtenerDeTalleCuotaMasivo(equivalenciaCuentasHipotecario, creditosHipotecarios, unidadesEjecutoras);

                detalleMovContaHipotecario.AddRange(items);

                //var equivalenciaCuentasHipotecarioCuantaOrden = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9005);

                //dataCabeceraMovContaHipotecarioCuantaOrden.Emp_codi = 294;
                //dataCabeceraMovContaHipotecarioCuantaOrden.Top_codi = 9005;
                //dataCabeceraMovContaHipotecarioCuantaOrden.Mco_nume = 0;
                //dataCabeceraMovContaHipotecarioCuantaOrden.Mco_fech = Proceso.FechaPago;
                //dataCabeceraMovContaHipotecarioCuantaOrden.Mod_codi = 7;
                //dataCabeceraMovContaHipotecarioCuantaOrden.Arb_csuc = "1001";
                //dataCabeceraMovContaHipotecarioCuantaOrden.Mco_desc = "Pagos listas hipotecario " + Proceso.FechaPago;

                //items = await ObtenerDeTalleCuotaMasivoCuentasDeOrden(equivalenciaCuentasHipotecarioCuantaOrden, creditosHipotecarios);

                //detalleMovContaHipotecarioCuantaOrden.AddRange(items);

            }

            if (detalleMovContaLeasing.Count() > 0)
            {
                string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovContaLeasing);

                RequestDetallePago requestDetallePagoLeasing = new RequestDetallePago();
                requestDetallePagoLeasing.Cpg_id = Guid.NewGuid();
                requestDetallePagoLeasing.Emp_codi = dataCabeceraMovContaLeasing.Emp_codi;
                requestDetallePagoLeasing.Top_codi = dataCabeceraMovContaLeasing.Top_codi;
                requestDetallePagoLeasing.Mco_fech = dataCabeceraMovContaLeasing.Mco_fech;
                requestDetallePagoLeasing.Mod_codi = dataCabeceraMovContaLeasing.Mod_codi;
                requestDetallePagoLeasing.Mco_desc = dataCabeceraMovContaLeasing.Mco_desc;
                requestDetallePagoLeasing.Mov_Contable_Seven = jsonMovimientosContables;
                requestDetallePagoLeasing.Respuesta_Seven = response.ReturnMessage;
                requestDetallePagoLeasing.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(detalleMovContaLeasing.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(detalleMovContaLeasing.Sum(x => x.Dmc_vadb), 2);
                //decimal totalDmcVacr = detalleMovContaLeasing.Where(item => item.Cue_codi != "84050302").Sum(item => item.Dmc_vacr);
                //decimal totalDmcCadb = detalleMovContaLeasing.Where(item => item.Cue_codi != "82810201").Sum(item => item.Dmc_vadb);

                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoLeasing, "CPLU", totalDmcCadb, totalDmcVacr);
                requestDetallePagoLeasing.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;

                var listaCpgDmcInfoUnica2 = detalleMovContaLeasing
                .GroupBy(item => new { item.Cpg_Id, item.Dmc_refe })
                .Select(grupo => grupo.First())
                .ToList();

                foreach (var item1 in listaCpgDmcInfoUnica2)
                {

                    UpdatePlanPago updatePlanPago = new UpdatePlanPago();
                    //int MCO_CONT = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 0);
                    //int MCO_NUME = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 1);
                    updatePlanPago.numeroCredito = long.Parse(item1.Dmc_refe);
                    updatePlanPago.cpgId = item1.Cpg_Id;
                    updatePlanPago.ccId = requestDetallePagoLeasing.Cc_Id ?? 0; // Aquí 0 es el valor predeterminado si Cc_Id es null
                    updatePlanPago.mcoCont = 0;
                    updatePlanPago.mcoNume = 0;

                    var respuesta = await _carteraRepository.UpdatePlanPago(updatePlanPago);

                }







                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoLeasing);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoLeasing);
                //response.ReturnMessage += "Comprobante listas leasing " + requestDetallePagoLeasing.Cc_Id + " creado correctamente ";
                response.ReturnMessage += "Movimientos hipotecario insertados correctamente, " + numeroComprobante.ReturnMessage;

                response.Data = null;
            }

            if (detalleMovContaHipotecario.Count() > 0)
            {

                string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovContaHipotecario);

                RequestDetallePago requestDetallePagoHipotecario = new RequestDetallePago();
                requestDetallePagoHipotecario.Cpg_id = Guid.NewGuid();
                requestDetallePagoHipotecario.Emp_codi = dataCabeceraMovContaHipotecario.Emp_codi;
                requestDetallePagoHipotecario.Top_codi = dataCabeceraMovContaHipotecario.Top_codi;
                requestDetallePagoHipotecario.Mco_fech = dataCabeceraMovContaHipotecario.Mco_fech;
                requestDetallePagoHipotecario.Mod_codi = dataCabeceraMovContaHipotecario.Mod_codi;
                requestDetallePagoHipotecario.Mco_desc = dataCabeceraMovContaHipotecario.Mco_desc;
                requestDetallePagoHipotecario.Mov_Contable_Seven = jsonMovimientosContables;
                requestDetallePagoHipotecario.Respuesta_Seven = response.ReturnMessage;
                requestDetallePagoHipotecario.Fecha_Ejecucion = DateTime.Now;

                decimal totalDmcVacr = Math.Round(detalleMovContaHipotecario.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(detalleMovContaHipotecario.Sum(x => x.Dmc_vadb), 2);
                //decimal totalDmcVacr = detalleMovContaLeasing.Where(item => item.Cue_codi != "84050302").Sum(item => item.Dmc_vacr);
                //decimal totalDmcCadb = detalleMovContaLeasing.Where(item => item.Cue_codi != "82810201").Sum(item => item.Dmc_vadb);

                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoHipotecario, "CPLU", totalDmcVacr, totalDmcCadb);
                requestDetallePagoHipotecario.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;

                var listaCpgDmcInfoUnica2 = detalleMovContaHipotecario
                .GroupBy(item => new { item.Cpg_Id, item.Dmc_refe })
                .Select(grupo => grupo.First())
                .ToList();

                foreach (var item1 in listaCpgDmcInfoUnica2)
                {

                    UpdatePlanPago updatePlanPago = new UpdatePlanPago();
                    //int MCO_CONT = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 0);
                    //int MCO_NUME = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 1);

                    updatePlanPago.numeroCredito = long.Parse(item1.Dmc_refe);
                    updatePlanPago.cpgId = item1.Cpg_Id;
                    updatePlanPago.ccId = requestDetallePagoHipotecario.Cc_Id ?? 0; // Aquí 0 es el valor predeterminado si Cc_Id es null
                    updatePlanPago.mcoCont = 0;
                    updatePlanPago.mcoNume = 0;

                    var respuesta = await _carteraRepository.UpdatePlanPago(updatePlanPago);

                }

                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoHipotecario);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoHipotecario);
                response.ReturnMessage += "Movimientos hipotecario insertados correctamente, " + numeroComprobante.ReturnMessage;


                response.Data = null;

            }
            if (detalleMovContaHipotecarioCuantaOrden.Count() > 0)
            {

                decimal totalDmcVacr = Math.Round(detalleMovContaHipotecarioCuantaOrden.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(detalleMovContaHipotecarioCuantaOrden.Sum(x => x.Dmc_vadb), 2);

                //decimal totalDmcVacr = detalleMovContaLeasing.Where(item => item.Cue_codi != "84050302").Sum(item => item.Dmc_vacr);
                //decimal totalDmcCadb = detalleMovContaLeasing.Where(item => item.Cue_codi != "82810201").Sum(item => item.Dmc_vadb);

                string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovContaHipotecarioCuantaOrden);

                RequestDetallePago requestDetallePagoHipotecario = new RequestDetallePago();
                requestDetallePagoHipotecario.Cpg_id = Guid.NewGuid();
                requestDetallePagoHipotecario.Emp_codi = dataCabeceraMovContaHipotecarioCuantaOrden.Emp_codi;
                requestDetallePagoHipotecario.Top_codi = dataCabeceraMovContaHipotecarioCuantaOrden.Top_codi;
                requestDetallePagoHipotecario.Mco_fech = dataCabeceraMovContaHipotecarioCuantaOrden.Mco_fech;
                requestDetallePagoHipotecario.Mod_codi = dataCabeceraMovContaHipotecarioCuantaOrden.Mod_codi;
                requestDetallePagoHipotecario.Mco_desc = dataCabeceraMovContaHipotecarioCuantaOrden.Mco_desc;
                requestDetallePagoHipotecario.Mov_Contable_Seven = jsonMovimientosContables;
                requestDetallePagoHipotecario.Respuesta_Seven = response.ReturnMessage;
                requestDetallePagoHipotecario.Fecha_Ejecucion = DateTime.Now;

                var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoHipotecario, "CPLU", totalDmcVacr, totalDmcCadb);
                requestDetallePagoHipotecario.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;


                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoHipotecario);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoHipotecario);

                response.ReturnMessage += " Movimientos cuentas de orden hipotecario insertados correctamente ";

                response.Data = null;

            }
            return response;

        }
        private async Task<List<ResponseMov>> ObtenerDeTalleCuotaMasivo(
    IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
    IEnumerable<AplicacionListaModel> aplicacionPago,
    IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras)
        {
            var items = new List<ResponseMov>();

            //  FILTRADO INICIAL DE CUENTAS (excluye tipos especiales)
            var cuentasContablesFiltro = cuentasContables.Where(x =>
                !(x.REF_COLUMN_APLICA_PAGO == "inmuebles")
                && !(x.REF_COLUMN_APLICA_PAGO == "inmueblesAct")
                && !(x.REF_COLUMN_APLICA_PAGO == "cxc")
                && !(x.REF_COLUMN_APLICA_PAGO == "Dev-Recursos")
                && !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes")).ToList();

            foreach (var itemAplicacionPago in aplicacionPago)
            {
                // ═════════════════════════════════════════════════════════════
                // PASO 1: CREAR LISTA DE EXCLUSIÓN DINÁMICA SEGÚN LO REALMENTE PAGADO
                // ═════════════════════════════════════════════════════════════
                var excludeFields = new List<string>
                {
                    "inmuebles", "inmueblesAct", "cxc", "Dev-Recursos", "Categoria A remanentes"
                };

                //  Excluir conceptos que NO se pagaron (valor = 0)
                if (itemAplicacionPago.ValorSeguroVida == 0)
                    excludeFields.Add("CPG_ABONO_SEGURO_VIDA");

                if (itemAplicacionPago.ValorSeguroHogar == 0)
                    excludeFields.Add("CPG_ABONO_SEGURO_HOGAR");

                if (itemAplicacionPago.ValorInteresCorriente == 0)
                    excludeFields.Add("CPG_ABONO_INTERES_CORRIENTE");

                if (itemAplicacionPago.ValorCapital == 0)
                {
                    excludeFields.Add("CPG_ABONO_CAPITAL");
                    excludeFields.Add("desembolso");
                }

                // Aplicar filtros dinámicos
                var cuentasContablesDisminucion = cuentasContablesFiltro
                    .Where(x => !excludeFields.Contains(x.REF_COLUMN_APLICA_PAGO))
                    .ToList();

                // ═════════════════════════════════════════════════════════════
                // PASO 2: MAPEO CON TUPLE (valor, concepto descriptivo)
                // ═════════════════════════════════════════════════════════════
                Dictionary<string, (decimal valor, string concepto)> mapeoPagos =
                    new Dictionary<string, (decimal, string)>
                {
            { "CPG_ABONO_CAPITAL",
                (itemAplicacionPago.ValorCapital, "CAPITAL") },

            { "CPG_ABONO_INTERES_CORRIENTE",
                (itemAplicacionPago.ValorInteresCorriente, "INTERÉS CORRIENTE") },

            { "CPG_ABONO_SEGURO_HOGAR",
                (itemAplicacionPago.ValorSeguroHogar, "SEGURO HOGAR") },

            { "CPG_ABONO_SEGURO_VIDA",
                (itemAplicacionPago.ValorSeguroVida, "SEGURO VIDA") },

            { "CPG_VALOR_PAGO",
                (itemAplicacionPago.ValorCuota, "PAGO TOTAL") },

            { "desembolso",
                (itemAplicacionPago.ValorCapital, "CAPITAL") }
                };

                // ═════════════════════════════════════════════════════════════
                // PASO 3: GENERAR MOVIMIENTOS SOLO PARA CONCEPTOS PAGADOS
                // ═════════════════════════════════════════════════════════════
                foreach (var item in cuentasContablesDisminucion)
                {
                    //  Obtener valor y concepto
                    if (!mapeoPagos.TryGetValue(item.REF_COLUMN_APLICA_PAGO, out var resultado))
                        continue; // Saltar si no existe en mapeo

                    var (valor, concepto) = resultado;

                    //  SOLO GENERAR MOVIMIENTO SI HAY VALOR PAGADO
                    if (valor > 0)
                    {
                        var itemDetalle = new ResponseMov
                        {
                            Cue_codi = item.CUENTA_CONTABLE_SEVEN,

                            //  Descripción con concepto identificado
                            Dmc_desc = $"cliente: {itemAplicacionPago.Nombre}",

                            Dmc_cant = 0,
                            Dmc_acti = 0,
                            Dmc_refe = itemAplicacionPago.NumeroCredito.ToString(),

                            //  Usar valor real del concepto
                            Dmc_vadb = item.TIPO_OPERACION == "D" ? valor : 0,
                            Dmc_vacr = item.TIPO_OPERACION == "C" ? valor : 0,

                            Dmc_vaba = 0,
                            Ter_coda = itemAplicacionPago.NroDocumento,
                            Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                            Arb_coda = $"{itemAplicacionPago.AreaNegocio}",
                            Arb_codp = "099",
                            Arb_cods = "1001",
                            Ter_codm = "0",
                            Cpg_Id = itemAplicacionPago.Cpg_Id
                        };
                        if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                        {
                            items.Add(itemDetalle);
                        }
                    }
                }
            }

            return items;
        }

        private async Task<List<ResponseMov>> ObtenerDeTalleCuotaMasivoCuentasDeOrden(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, IEnumerable<AplicacionListaModel> aplicacionPago)
        {

            var items = new List<ResponseMov>();

            foreach (var itemAplicacionPago in aplicacionPago)
            {

                foreach (var item in cuentasContables)
                {
                    var itemDetalle = new ResponseMov();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: {itemAplicacionPago.NroDocumento}";
                    itemDetalle.Dmc_cant = 0;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = itemAplicacionPago.NumeroCredito.ToString();
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? itemAplicacionPago.ValorCapital : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? itemAplicacionPago.ValorCapital : 0;
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = itemAplicacionPago.NroDocumento;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    //itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_coda = $"{itemAplicacionPago.UnidadEjecutora}";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";
                    itemDetalle.Cpg_Id = itemAplicacionPago.Cpg_Id;

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }
                }

            }

            return items;
        }

        public async Task<Response<InsertarMovContableResponse>> AplicarPagoMasivoMovContableTotalizado(DateTime fechaAplicacion, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataCabeceraMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            var dataCabeceraMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<PagoListaDetalleDto> Pagos = new List<PagoListaDetalleDto>();

            ReporteContableParametersDto parametros = new ReporteContableParametersDto();
            parametros.FechaInicial = fechaAplicacion;
            parametros.FechaFinal = fechaAplicacion;

            var datosReporte = await _reportesRepository.GenerarReporteContablePagos(parametros);
            var creditosLeasing = datosReporte.Where(x => x.TIPO_OPERACION == 9065);
            var creditosHipotecarios = datosReporte.Where(x => x.TIPO_OPERACION == 9003);



            if (creditosLeasing.Count() > 0)
            {
                var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);
                dataCabeceraMovContaLeasing.Emp_codi = 294;
                dataCabeceraMovContaLeasing.Top_codi = 9065;
                dataCabeceraMovContaLeasing.Mco_nume = 0;
                dataCabeceraMovContaLeasing.Mco_fech = fechaAplicacion;
                dataCabeceraMovContaLeasing.Mod_codi = 7;
                dataCabeceraMovContaLeasing.Arb_csuc = "1001";
                dataCabeceraMovContaLeasing.Mco_desc = "Aplicación de pagos leasing habitacional " + fechaAplicacion.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                var itemsLeasing = ObtenerDeTalleAplicacionPagosTotalizados(creditosLeasing, equivalenciaCuentasLeasing, fechaAplicacion).Result;
                detalleMovContaLeasing.AddRange(itemsLeasing);

                if (detalleMovContaLeasing.Count > 0)
                {
                    dataCabeceraMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                    var bodyLeasing = new InsertarMovContableRequestBody(dataCabeceraMovContaLeasing);
                    movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);

                    response.ReturnMessage += response.Data is not null ? "Resultado aplicación de pagos leasing : " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos de leasing";
                }
                else
                    response.ReturnMessage += "No hay créditos leasing para crear un mov. contable en seven en el rango de fechas seleccionado.";

            }
            if (creditosHipotecarios.Count() > 0)
            {

                var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);
                dataCabeceraMovContaHipotecario.Emp_codi = 294;
                dataCabeceraMovContaHipotecario.Top_codi = 9003;
                dataCabeceraMovContaHipotecario.Mco_nume = 0;
                dataCabeceraMovContaHipotecario.Mco_fech = fechaAplicacion;
                dataCabeceraMovContaHipotecario.Mod_codi = 7;
                dataCabeceraMovContaHipotecario.Arb_csuc = "1001";
                dataCabeceraMovContaHipotecario.Mco_desc = "Aplicación de pagos hipotecario " + fechaAplicacion.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");


                var itemsHipotecario = ObtenerDeTalleAplicacionPagosTotalizados(creditosHipotecarios, equivalenciaCuentasHipotecario, fechaAplicacion).Result;
                detalleMovContaHipotecario.AddRange(itemsHipotecario);

                if (detalleMovContaHipotecario.Count > 0)
                {
                    dataCabeceraMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                    var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovContaHipotecario);
                    movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);

                    response.ReturnMessage += response.Data is not null ? "Resultado aplicación de pagos Hipotecario : " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos de Hipotecario";
                }
                else
                    response.ReturnMessage += "No hay créditos hipotecario para crear un mov. contable en seven en el rango de fechas seleccionado.";
            }

            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleAplicacionPagosTotalizados(IEnumerable<ReporteContable> aplicacionPago, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, DateTime fechaCierreDiarioSeven)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables;
            decimal seguroHogar = 0, seguroVida = 0, capital = 0, valorCuotaTotal = 0, intereses = 0;

            foreach (var itemAplicacionPago in aplicacionPago)
            {
                if (itemAplicacionPago.TIPO_OPERACION == 9065)
                {
                    switch (itemAplicacionPago.CUENTA_SEVEN)
                    {
                        case "25957005": //debito valor cuota
                            valorCuotaTotal += itemAplicacionPago.DEBITO;
                            break;
                        case "14598901": //credito capital
                            capital += itemAplicacionPago.CREDITO;
                            break;
                        case "16080201": //credito intereses
                            intereses += itemAplicacionPago.CREDITO;
                            break;
                        case "1636050401": //credito seguro vida
                            seguroVida += itemAplicacionPago.CREDITO;
                            break;
                        case "1636050402": //credito seguro hogar
                            seguroHogar += itemAplicacionPago.CREDITO;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (itemAplicacionPago.CUENTA_SEVEN)
                    {
                        case "25957008": //debito valor cuota
                            valorCuotaTotal += itemAplicacionPago.DEBITO;
                            break;
                        case "14040504": //credito capital
                            capital += itemAplicacionPago.CREDITO;
                            break;
                        case "16050602": //credito intereses
                            intereses += itemAplicacionPago.CREDITO;
                            break;
                        case "1636050301": //credito seguro vida
                            seguroVida += itemAplicacionPago.CREDITO;
                            break;
                        case "1636050302": //credito seguro hogar
                            seguroHogar += itemAplicacionPago.CREDITO;
                            break;
                        default:
                            break;
                    }

                }

            }
            if (seguroVida == 0)
            {
                cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA"));

            }
            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                    {
                        { "CPG_ABONO_CAPITAL", capital },
                        { "CPG_ABONO_INTERES_CORRIENTE", intereses },
                        { "CPG_ABONO_SEGURO_HOGAR", seguroHogar },
                        { "CPG_ABONO_SEGURO_VIDA", seguroVida },
                        { "CPG_VALOR_PAGO", valorCuotaTotal },
                        { "desembolso", capital }
                    };
            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = "Aplicación de cuotas";
                itemDetalle.Dmc_cant = 1;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = "000201508";
                itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = "000201508";
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = "3011";
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";

                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }
        public async Task<Response<IEnumerable<PagoListaDetalleDto>>> GenerarLista(GenerarListaParametersDto parameters)
        {
            var response = this._mapper.Map<List<PagoListaDetalleDto>>(await this._carteraRepository.GenerarLista(parameters));

            if (response.Count() == 0)
            {
                return new()
                {
                    Data = response,
                    IsSuccess = false,
                    ReturnMessage = $"No existe información relacionada con la Unidad Ejecutora"
                };
            }
            var requestApplicationPayment = new ParametrosObtenerProcesoAplicacionPagosDto
            {
                Periodo = int.Parse(parameters.FechaAplicacion.ToString("yyyyMM")),
                IdTipoCredito = parameters.TipoCredito,
                IdUnidadEjecutora = parameters.UnidadEjecutora,
                Estado = "APROBADA"
            };
            var paymentAplication = await ObtenerProcesoAplicacionPagos(requestApplicationPayment);
            if (paymentAplication.Data.Any())
            {
                var paymentLast = paymentAplication.Data
                    .Where(x => x.IdProceso > 0)
                    .SelectMany(x => x.Pagos)  //  ProcesoAplicacionPagosMasivoDto contiene Pagos
                    .ToList();

                var idCreditosYaPagados = paymentLast
                    .Select(x => x.IdCredito)  //  PagoListaDetalleDto.IdCredito (Guid)
                    .Distinct()
                    .ToList();
                response = response.Where(x => !idCreditosYaPagados.Contains(x.IdCredito)).ToList();
            }

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(response.
                Select(x => x.NroDocumento).Distinct().ToList())).Data;

            var list = response.GroupJoin(listAfiliados, r => r.NroDocumento, la => la.Documento, (r, la) =>
            {
                r.Funcionario = la.FirstOrDefault()?.NombreCompleto ?? "";
                return r;
            }).ToList();

            return new() { Data = list };
        }

        private async Task<IEnumerable<ColumnasArchivoCargueMasivoDto>> ObtenerColumnasArchivoCargueMasivoPorUnidadEjecutora(int unidadEjecutora)
        {
            var response = this._mapper.Map<List<ColumnasArchivoCargueMasivoDto>>(
                await this._carteraRepository.ObtenerColumnasArchivoCargueMasivoPorUnidadEjecutora(unidadEjecutora)
            );

            return response;
        }

        //private CsvConfiguration GetCsvConfigurationAplicacionPago()
        //{
        //    var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        //    {
        //        PrepareHeaderForMatch = arg => Regex.Replace(arg.Header.Trim(), @"\s+", " ").RemoveDiacritics().ToLower(),
        //        MissingFieldFound = arg =>
        //        {
        //            var strBuilder = new StringBuilder();
        //            var encabezado = arg.HeaderNames?.ElementAtOrDefault(arg.Index);


        //            strBuilder.AppendLine(encabezado != null 
        //                ? $"No se ha encontrado el valor para el encabezado \"{encabezado}\""
        //                : "El archivo no cumple con la estructura correcta"
        //            );
        //            strBuilder.AppendLine($"Línea: {arg.Context.Parser.Row}");

        //            throw new CsvFieldNotFoundException(strBuilder.ToString());
        //        },
        //        Delimiter = ",",
        //        DetectDelimiter = true,
        //    };

        //    return csvConfig;
        //}

        //private async Task<IEnumerable<ArchivoAplicacionPagoCSVDto>> ObtenerListadoDesdeCSV(CsvReader csvReader, IEnumerable<ColumnasArchivoCargueMasivoDto> columnasInfo)
        //{
        //    var listaCsv = new List<ArchivoAplicacionPagoCSVDto>();

        //    if (csvReader.Read() && csvReader.ReadHeader())
        //    {
        //        while (await csvReader.ReadAsync())
        //        {
        //            listaCsv.Add(LeerRegistroArchivoAplicacionPago(csvReader, columnasInfo));
        //        }
        //    }

        //    return listaCsv;
        //}

        private ArchivoAplicacionPagoCSVDto LeerRegistroArchivoAplicacionPago(CsvReader reader, IEnumerable<ColumnasArchivoCargueMasivoDto> columnasInfo)
        {
            const string
                IDENTIFICACION = "IDENTIFICACION",
                NOMBRE_COMPLETO = "NOMBRE_COMPLETO",
                NOMBRES = "NOMBRES",
                APELLIDOS = "APELLIDOS",
                VALOR_CUOTA = "VALOR_CUOTA";

            var colIdentificacion = columnasInfo.First(x => x.IdColumna == IDENTIFICACION);
            var colValorCuota = columnasInfo.First(x => x.IdColumna == VALOR_CUOTA);
            var colNombreCompleto = columnasInfo.FirstOrDefault(x => x.IdColumna == NOMBRE_COMPLETO);

            string funcionario;

            if (colNombreCompleto != null)
            {
                reader.TryGetField(colNombreCompleto.Encabezado, out funcionario);
            }
            else
            {
                string nombres = null, apellidos = null;
                var colNombres = columnasInfo.FirstOrDefault(x => x.IdColumna == NOMBRES);
                var colApellidos = columnasInfo.FirstOrDefault(x => x.IdColumna == APELLIDOS);

                if (colNombres != null)
                    reader.TryGetField(colNombres.Encabezado, out nombres);

                if (colApellidos != null)
                    reader.TryGetField(colApellidos.Encabezado, out apellidos);


                funcionario = $"{nombres?.Trim()} {apellidos?.Trim()}";
            }

            reader.TryGetField("Fuerza Nombre", out string fuerza);

            if (fuerza == null)
                reader.TryGetField("Fuerza", out fuerza);

            var itemCsv = new ArchivoAplicacionPagoCSVDto()
            {
                FuerzaNombre = fuerza,
                Funcionario = funcionario ?? string.Empty,
                NroDocumento = reader.GetField<string>(colIdentificacion.Encabezado),
                ValorCuota = reader.GetField<decimal>(colValorCuota.Encabezado),
            };

            return itemCsv;
        }

        //private ResultadoComparacionArchivoAplicacionPagoCSVDto CompararListadoConCSV<T>(T listaAplicacion, T listaCsv) where T : IEnumerable<ArchivoAplicacionPagoCSVDto>
        //{
        //    var matchList = new List<ArchivoAplicacionPagoCSVComparacionDto>();
        //    var noMatchList = new List<ArchivoAplicacionPagoCSVComparacionDto>();

        //    var currencyFormat = "$ #,0.######";

        //    foreach (var itemApp in listaAplicacion)
        //    {
        //        var itemCsv = listaCsv.FirstOrDefault(x => x.NroDocumento == itemApp.NroDocumento);
        //        var result = _mapper.Map<ArchivoAplicacionPagoCSVDto, ArchivoAplicacionPagoCSVComparacionDto>(itemApp);

        //        if (itemCsv != null)
        //        {
        //            if (itemApp.ValorCuota != itemCsv.ValorCuota)
        //            {
        //                var csvCuotaFormat = itemCsv.ValorCuota.ToString(currencyFormat, CultureInfo.InvariantCulture);
        //                var appCuotaFormat = itemApp.ValorCuota.ToString(currencyFormat, CultureInfo.InvariantCulture);

        //                result.Observaciones =
        //                    $"El valor de la cuota importado del archivo ({csvCuotaFormat}) es diferente " +
        //                    $"al esperado por la aplicacion ({appCuotaFormat})";
        //            }

        //            matchList.Add(result);
        //        }
        //        else
        //        {
        //            result.Observaciones = "No se halló en el CSV";
        //            noMatchList.Add(result);
        //        }
        //    }

        //    var comparer = new ArchivoAplicacionPagoCSVDtoComparer();
        //    var listaSoloCsv = listaCsv.Except(listaAplicacion, comparer);

        //    foreach (var item in listaSoloCsv)
        //    {
        //        var result = _mapper.Map<ArchivoAplicacionPagoCSVDto, ArchivoAplicacionPagoCSVComparacionDto>(item);
        //        result.FechaDesde = item.FechaDesde == default ? null : result.FechaDesde;
        //        result.FechaHasta = item.FechaHasta == default ? null : result.FechaHasta;
        //        result.Observaciones = "No se halló en el listado generado por el sistema";
        //        noMatchList.Add(result);
        //    }

        //    return new ResultadoComparacionArchivoAplicacionPagoCSVDto(matchList, noMatchList);
        //}

        public async Task<Response<ExcepcionTasaActorDto>> ObtenerExcepcionesPorTipoAfiliacion(int request)
        {
            var response = this._mapper.Map<ExcepcionTasaActorDto>(await this._carteraRepository.ObtenerExcepcionesPorTipoAfiliacion(request));
            return new Response<ExcepcionTasaActorDto> { Data = response };
        }

        public async Task<Response<ExcepcionTasaUEJDto>> ObtenerExcepcionesPlazoUEj(int simId, int uEJ)
        {
            var response = this._mapper.Map<ExcepcionTasaUEJDto>(await this._carteraRepository.ObtenerExcepcionesTasaUEj(simId, uEJ));
            return new Response<ExcepcionTasaUEJDto> { Data = response };
        }

        /// <summary>
        /// CierreMensualCartera OPTIMIZADO
        /// Mejoras: 
        /// - Consultas en batch (evita N+1 queries)
        /// - Cache de productos para evitar consultas duplicadas
        /// - Procesamiento paralelo donde es seguro
        /// - Refactorización para mejor legibilidad
        /// </summary>
        public async Task<Response<IEnumerable<ReporteCierreMensualCarteraDto>>> CierreMensualCartera(ParamCierreMensualCarteraDto param)
        {
            // ===== VALIDACIÓN INICIAL =====
            var responseValidacion = await ExisteCierreMensualCartera(param);
            if (responseValidacion.Data)
            {
                return new Response<IEnumerable<ReporteCierreMensualCarteraDto>>
                {
                    IsSuccess = false,
                    ReturnMessage = responseValidacion.ReturnMessage
                };
            }

            // ===== OBTENER DATOS BASE =====
            var paramCierreMensualCartera = this._mapper.Map<ParamCierreMensualCartera>(param);
            var response = (await this._carteraRepository.CierreMensualCartera(paramCierreMensualCartera))
                .Select(x => this._mapper.Map<ReporteCierreMensualCarteraDto>(x))
                .ToList();

            if (!response.Any())
            {
                return new Response<IEnumerable<ReporteCierreMensualCarteraDto>> { Data = response };
            }

            // ===== OBTENER TODAS LAS NOVEDADES EN BATCH (evita N+1) =====
            var creditoIds = response.Select(x => x.CreditoId).Distinct().ToList();
            var novedadesPorCredito = await ObtenerNovedadesEnBatch(creditoIds);

            // ===== OBTENER TODOS LOS PRODUCTOS EN BATCH (evita consultas duplicadas) =====
            var identificaciones = response.Select(x => x.Identificacion).Distinct().ToList();
            var productosPorIdentificacion = await ObtenerProductosEnBatch(identificaciones);

            // ===== PROCESAR PROVISIONES (Calificación C, B, D) =====
            await ProcesarProvisiones(response, productosPorIdentificacion);

            // ===== PROCESAR REESTRUCTURACIONES Y LEASING =====
            await ProcesarNovedadesYLeasing(response, novedadesPorCredito, productosPorIdentificacion);

            // ===== OBTENER NOMBRES DE CLIENTES EN BATCH =====
            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(identificaciones)).Data;

            // ===== COMBINAR NOMBRES =====
            var resultado = response.GroupJoin(
                listAfiliados,
                r => r.Identificacion,
                la => la.Documento,
                (r, la) =>
                {
                    r.Nombres = la.FirstOrDefault()?.NombreCompleto ?? "";
                    return r;
                }).ToList();

            return new Response<IEnumerable<ReporteCierreMensualCarteraDto>> { Data = resultado };
        }

        // ========================================================================
        // MÉTODOS AUXILIARES PARA OPTIMIZACIÓN
        // ========================================================================

        /// <summary>
        /// Obtiene todas las novedades en una sola consulta batch
        /// </summary>
        public async Task<Dictionary<Guid, IEnumerable<NovedadCartera>>> ObtenerNovedadesEnBatch(List<Guid> creditoIds)
        {
            try
            {
                // Opción 1: Si tu repositorio soporta consulta por múltiples IDs
                var todasNovedades = await _carteraRepository.ObtenerNovedadesCarteraPorCreditos(creditoIds);

                return todasNovedades.GroupBy(n => n.CRE_ID)
                    .ToDictionary(g => g.Key, g => (IEnumerable<NovedadCartera>)g.ToList());  // ← Cast explícito
            }
            catch
            {
                // Fallback: Si no existe el método batch, crear uno temporal con consultas paralelas
                var tasks = creditoIds.Select(async creditoId =>
                    new { CreditoId = creditoId, Novedades = await _carteraRepository.ObtenerNovedadesCartera(creditoId) }
                );

                var resultados = await Task.WhenAll(tasks);
                return resultados.ToDictionary(r => r.CreditoId, r => r.Novedades);
            }
        }

        /// <summary>
        /// Obtiene todos los productos de clientes en una sola consulta batch
        /// Cache para evitar consultas duplicadas
        /// </summary>
        public async Task<Dictionary<string, List<ProductoClienteDto>>> ObtenerProductosEnBatch(List<string> identificaciones)
        {
            var cache = new Dictionary<string, List<ProductoClienteDto>>();

            // Procesar en lotes de 50 para evitar timeouts
            const int batchSize = 50;
            for (int i = 0; i < identificaciones.Count; i += batchSize)
            {
                var batch = identificaciones.Skip(i).Take(batchSize).ToList();

                var tasks = batch.Select(async identificacion =>
                {
                    var request = new SolicitudProductoPorClienteDto
                    {
                        Identificacion = identificacion,
                        TipoIdentificacion = 1
                    };

                    try
                    {
                        var resultado = await ObtenerProductoCliente(request);
                        return new { Identificacion = identificacion, Productos = resultado.Data?.ToList() ?? new List<ProductoClienteDto>() };
                    }
                    catch (Exception ex)
                    {
                        // Log del error pero continuar procesando
                        Console.WriteLine($"Error obteniendo productos para {identificacion}: {ex.Message}");
                        return new { Identificacion = identificacion, Productos = new List<ProductoClienteDto>() };
                    }
                });

                var resultados = await Task.WhenAll(tasks);

                foreach (var resultado in resultados)
                {
                    cache[resultado.Identificacion] = resultado.Productos;
                }
            }

            return cache;
        }
        /// <summary>
        /// <summary>
        /// Obtiene todos los planes de pago en una sola consulta batch por número de crédito
        /// Filtra por fecha del periodo
        /// </summary>
        public async Task<Dictionary<(long NumeroCredito, int TipoCredito), PlanPagoDto>> ObtenerPlanPagoEnBatch(
            List<(long NumeroCredito, int TipoCredito)> creditos,
            DateTime fechaPeriodo)
        {
            var cache = new Dictionary<(long, int), PlanPagoDto>();

            // Función local para obtener plan de un crédito
            async Task<((long, int) Credito, PlanPagoDto Plan)> ObtenerPlanParaCredito((long NumeroCredito, int TipoCredito) credito)
            {
                var request = new ObtenerProductoCartera
                {
                    Nro_Credito = credito.NumeroCredito,
                    Tcr_Id = credito.TipoCredito
                };

                try
                {

                    var resultado = await ObtenerPlanPagosCredito(request);

                    // Filtrar por fecha del periodo
                    var planFechaPeriodo = resultado.Data?.PlanPago
                        .FirstOrDefault(p => p.Fecha.Date == fechaPeriodo.Date);

                    return (credito, planFechaPeriodo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error obteniendo plan para crédito {credito.NumeroCredito}: {ex.Message}");
                    return (credito, null);
                }
            }

            // Procesar en lotes de 50 para evitar timeouts
            const int batchSize = 50;
            for (int i = 0; i < creditos.Count; i += batchSize)
            {
                var batch = creditos.Skip(i).Take(batchSize).ToList();
                var tasks = batch.Select(ObtenerPlanParaCredito);
                var resultados = await Task.WhenAll(tasks);

                foreach (var (credito, plan) in resultados)
                {
                    if (plan != null)
                    {
                        cache[credito] = plan;
                    }
                }
            }

            return cache;
        }
        /// <summary>
        /// Procesa provisiones para créditos con calificación C, B, D
        /// </summary>
        public async Task ProcesarProvisiones(
            List<ReporteCierreMensualCarteraDto> response,
            Dictionary<string, List<ProductoClienteDto>> productosPorIdentificacion)
        {
            var itemsConProvision = response.Where(x => x.Calificacion == "C" || x.Calificacion == "B" || x.Calificacion == "D").ToList();

            if (!itemsConProvision.Any()) return;

            foreach (var item in itemsConProvision)
            {
                if (!productosPorIdentificacion.TryGetValue(item.Identificacion, out var productos) || !productos.Any())
                    continue;

                var producto = productos.FirstOrDefault();
                if (producto == null) continue;

                switch (item.Calificacion)
                {
                    case "C":
                        var daysMora = item.DiasMora > 0 ? item.InteresesCorrientesSeven * 0.01m : 0;
                        var saldoCapital = producto.SaldoCapital;
                        var saldoCapitalCalculado = Math.Round(saldoCapital * 0.10m, 0);
                        var saldoDeudaRemantente = Math.Round(item.ValorDeudaRemanente * 0.01m);
                        item.Provision = saldoCapitalCalculado + daysMora + saldoDeudaRemantente;
                        break;

                    case "B":
                        // TODO: Implementar lógica para calificación B
                        break;

                    case "D":
                        // TODO: Implementar lógica para calificación D
                        break;
                }
            }
        }

        /// <summary>
        /// Procesa reestructuraciones y créditos Leasing
        /// </summary>
        public async Task ProcesarNovedadesYLeasing(
            List<ReporteCierreMensualCarteraDto> response,
            Dictionary<Guid, IEnumerable<NovedadCartera>> novedadesPorCredito,
            Dictionary<string, List<ProductoClienteDto>> productosPorIdentificacion)
        {
            foreach (var item in response)
            {
                // ===== PROCESAR REESTRUCTURACIONES (TNV_ID = 3) =====
                if (novedadesPorCredito.TryGetValue(item.CreditoId, out var novedades) && novedades.Any())
                {
                    var tieneReestructuracion = novedades.Any(x => x.TNV_ID == 3);

                    if (tieneReestructuracion && item.TipoCreditoId == 1)
                    {
                        if (productosPorIdentificacion.TryGetValue(item.Identificacion, out var productos) && productos.Any())
                        {
                            var producto = productos.FirstOrDefault();
                            item.SaldoCapital = producto.SaldoCapital;
                            item.AcumuladoCapital = producto.SaldoCapital;
                            item.ValorDeudaRemanente = 0;
                            item.Provision = item.SaldoCapital * 0.01m;
                        }
                    }
                }

                // ===== PROCESAR HIPOTECARIOS (TipoCreditoId = 2) =====
                if (item.TipoCreditoId == 2 || item.TipoCreditoId == 5)
                {
                    if (productosPorIdentificacion.TryGetValue(item.Identificacion, out var productos))
                    {
                        var productoHipotecarios = productos.FirstOrDefault(x => x.NumeroCredito == item.NumeroCredito);
                        if (productoHipotecarios != null)
                        {
                            item.SaldoCapital = productoHipotecarios.SaldoCapital;
                            if (item.EstadoCredito == "AL DIA")
                            {
                                item.Provision = item.SaldoCapital * 0.01m;
                            }
                        }
                    }
                }
            }
        }


        ///// <summary>
        ///// CierreMensualCartera
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<Response<IEnumerable<ReporteCierreMensualCarteraDto>>> CierreMensualCartera(ParamCierreMensualCarteraDto param)
        //{
        //    Task<Response<bool>> responseValidacion = ExisteCierreMensualCartera(param);

        //    DateTime primerDiaDelSiguienteMes = new DateTime(param.AnioCierre, param.MesCierre, 1).AddMonths(1);
        //    DateTime ultimoDiaDelMes = primerDiaDelSiguienteMes.AddDays(-1);
        //    DateTime fechaCierre = new DateTime(ultimoDiaDelMes.Year, ultimoDiaDelMes.Month, ultimoDiaDelMes.Day);

        //    if (responseValidacion.Result.Data)
        //    {
        //        return new Response<IEnumerable<ReporteCierreMensualCarteraDto>> { IsSuccess = false, ReturnMessage = responseValidacion.Result.ReturnMessage };

        //    }
        //    else
        //    {
        //        List<ReporteCierreMensualCarteraDto> list = new List<ReporteCierreMensualCarteraDto>();
        //        var paramCierreMensualCartera = this._mapper.Map<ParamCierreMensualCartera>(param);
        //        var response = this._mapper.Map<IEnumerable<ReporteCierreMensualCarteraDto>>(await this._carteraRepository.
        //            CierreMensualCartera(paramCierreMensualCartera));

        //        var filterReport = response.Where(x => x.Calificacion == "C" || x.Calificacion == "B" || x.Calificacion == "D");
        //        if (filterReport.Count() > 0)
        //        {
        //            foreach (var item in filterReport)
        //            {
        //                switch (item.Calificacion)
        //                {
        //                    case "C":
        //                        //implementar la consultar del producto para poder obtener el saldo al corete
        //                        var request = new SolicitudProductoPorClienteDto
        //                        {
        //                            Identificacion = item.Identificacion,
        //                            TipoIdentificacion = 1
        //                        };
        //                        var dataProducto = await ObtenerProductoCliente(request);
        //                        var daysMora = item.DiasMora > 0 ? item.InteresesCorrientesSeven * 0.01m : 0;
        //                        var saldoCapital = dataProducto.Data.FirstOrDefault().SaldoCapital;
        //                        var saldoCapitalCalculado = Math.Round(saldoCapital * 0.10m, 0);
        //                        var saldoDeudaRemantente = Math.Round(item.ValorDeudaRemanente * 0.01m);
        //                        item.Provision = saldoCapitalCalculado + daysMora + saldoDeudaRemantente;
        //                        break;
        //                    case "B":
        //                        break;
        //                    case "D":
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //        foreach (var item in response)
        //        {
        //            var validateNoveda = await _carteraRepository.ObtenerNovedadesCartera(item.CreditoId);
        //            if (validateNoveda.Count() > 0)
        //            {
        //                var validateRestructuracion = validateNoveda.Where(x => x.TNV_ID == 3);
        //                if (validateRestructuracion != null && item.TipoCreditoId == 1)
        //                {
        //                    var request = new SolicitudProductoPorClienteDto
        //                    {
        //                        Identificacion = item.Identificacion,
        //                        TipoIdentificacion = 1
        //                    };
        //                    var obtenerInformacion = await ObtenerProductoCliente(request);
        //                    item.SaldoCapital = obtenerInformacion.Data.FirstOrDefault().SaldoCapital;
        //                    item.AcumuladoCapital = obtenerInformacion.Data.FirstOrDefault().SaldoCapital;
        //                    item.ValorDeudaRemanente = 0;
        //                    item.Provision = item.SaldoCapital * 0.01m;
        //                }
        //            }
        //            if (item.TipoCreditoId == 2)
        //            {
        //                var request = new SolicitudProductoPorClienteDto
        //                {
        //                    Identificacion = item.Identificacion,
        //                    TipoIdentificacion = 1
        //                };
        //                try
        //                {
        //                    var obtenerInformacion = await ObtenerProductoCliente(request);
        //                    item.SaldoCapital = obtenerInformacion.Data.Where(x => x.NumeroCredito == item.NumeroCredito).FirstOrDefault().SaldoCapital;

        //                }
        //                catch (Exception e)
        //                {

        //                    throw;
        //                }
        //            }
        //        }

        //        var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(response.
        //            Select(x => x.Identificacion).Distinct().ToList())).Data;

        //        try
        //        {
        //            list = response.GroupJoin(listAfiliados, r => r.Identificacion, la => la.Documento, (r, la) =>
        //            {
        //                r.Nombres = la.FirstOrDefault()?.NombreCompleto ?? "";
        //                return r;
        //            }).ToList();
        //        }
        //        catch (Exception w)
        //        {

        //            throw;
        //        }
        //        return new Response<IEnumerable<ReporteCierreMensualCarteraDto>> { Data = list };
        //    }

        //}

        public async Task<Response<IEnumerable<ReporteCierreAnualCarteraDto>>> CierreAnualCartera(RequestCierreAnualCartera request)
        {
            var ultimosCierres = this._mapper.Map<IEnumerable<UltimoCierreDto>>(await _carteraRepository.ConsultarUltimosCierres());

            var cierreMensualUltimoMes = ultimosCierres.FirstOrDefault(
                x => x.IdTipoCierre == 2 // Cierre Mensual
                && (x.FechaCierre?.Year, x.FechaCierre?.Month) == (request.AnioCierre, 12)
            );

            var cierreAnioActual = ultimosCierres.FirstOrDefault(
                x => x.IdTipoCierre == 4 // Cierre Anual
                && x.FechaCierre?.Year == request.AnioCierre
            );

            var ultimoCierreAnual = ultimosCierres.FirstOrDefault(x => x.IdTipoCierre == 4);

            if (request.Actualiza && cierreAnioActual != null)
            {
                return $"Ya se realizo el cierre anual del {request.AnioCierre}"
                    .BuildError<IEnumerable<ReporteCierreAnualCarteraDto>>();
            }

            if (ultimoCierreAnual != null && request.AnioCierre < ultimoCierreAnual.FechaCierre?.Year)
            {
                return $"NO puede realizar un cierre anual anterior al ultimo cierre anual realizado ({ultimoCierreAnual.FechaCierre?.Year})"
                    .BuildError<IEnumerable<ReporteCierreAnualCarteraDto>>();
            }

            if (request.Actualiza && cierreMensualUltimoMes == null)
            {
                return $"Debe realizar el cierre mesual del periodo {request.AnioCierre} / {12}"
                    .BuildError<IEnumerable<ReporteCierreAnualCarteraDto>>();
            }

            var responseCierre = _mapper.Map<IEnumerable<ReporteCierreAnualCarteraDto>>(
                await _carteraRepository.CierreAnualCartera(request)
            ).ToList();

            if (responseCierre.Any())
            {
                var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(
                    responseCierre.Select(x => x.Identificacion).Distinct().ToList())).Data;

                responseCierre.Join(listAfiliados, x => x.Identificacion, y => y.Documento, (cierre, doc) =>
                {
                    cierre.NombreCompleto = doc.NombreCompleto;
                    return cierre;
                }).ToList();
            }



            return new Response<IEnumerable<ReporteCierreAnualCarteraDto>> { Data = responseCierre };
        }
        public async Task<Response<InsertarMovContableResponse>> CierreAnualCarteraSeven(RequestCierreAnualCartera request, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            if (request.Actualiza)
            {
                // Obtener el primer día del siguiente mes
                DateTime primerDiaDelSiguienteMes = new DateTime(request.AnioCierre, 12, 1).AddMonths(1);

                // Restar un día para obtener el último día del mes actual
                DateTime ultimoDiaDelMes = primerDiaDelSiguienteMes.AddDays(-1);

                DateTime fechaCierre = new DateTime(ultimoDiaDelMes.Year, ultimoDiaDelMes.Month, ultimoDiaDelMes.Day);

                request.Actualiza = false;
                var responseCierre = await CierreAnualCartera(request);

                IEnumerable<TipoCreditoDto> catalogoValores = this._mapper.Map<IEnumerable<TipoCreditoDto>>(await this._catalogosBusinessLogic.ObtenerTipoCredito());
                var descripcionCreditosLeasing = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
                var descripcionCreditosHipotecarios = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;

                var creditosLeasing = responseCierre.Data.Where(x => x.TipoCredito == descripcionCreditosLeasing);
                var creditosHipotecarios = responseCierre.Data.Where(x => x.TipoCredito == descripcionCreditosHipotecarios);
                var dataMovContaLeasing = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var dataMovContaHipotecario = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

                if (creditosLeasing.Count() > 0)
                {
                    dataMovContaLeasing.Emp_codi = 294;
                    dataMovContaLeasing.Top_codi = 9072;
                    dataMovContaLeasing.Mco_nume = 0;
                    dataMovContaLeasing.Mco_fech = fechaCierre;
                    dataMovContaLeasing.Mod_codi = 7;
                    dataMovContaLeasing.Arb_csuc = "1001";
                    dataMovContaLeasing.Mco_desc = "Cierre anual leasing";
                    foreach (var item in creditosLeasing)
                    {
                        var items = ObtenerDeTalleCierreAnualSeven(item).Result;
                        detalleMovContaLeasing.AddRange(items);
                    }

                    if (detalleMovContaLeasing.Count > 0)
                    {
                        dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();
                        /*se suspende consumo seven*/
                        //var bodyLeasing = new InsertarMovContableRequestBody(dataMovContaLeasing);
                        //movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                        //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);

                        //response.ReturnMessage = response.Data is not null ? "Resultado Mov. Contable Seven Créditos leasing: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        //    "para créditos de Leasing";
                    }
                    else
                        response.ReturnMessage = "No hay créditos leasing para crear un mov. contable en seven en el rango de fechas seleccionado.";

                }

                if (creditosHipotecarios.Count() > 0)
                {
                    dataMovContaHipotecario.Emp_codi = 294;
                    dataMovContaHipotecario.Top_codi = 9004;
                    dataMovContaHipotecario.Mco_nume = 0;
                    dataMovContaHipotecario.Mco_fech = fechaCierre;
                    dataMovContaHipotecario.Mod_codi = 7;
                    dataMovContaHipotecario.Arb_csuc = "1001";
                    dataMovContaHipotecario.Mco_desc = "cierre anual hipotecario";
                    foreach (var item in creditosHipotecarios)
                    {
                        var items = ObtenerDeTalleCierreAnualSeven(item).Result;
                        detalleMovContaHipotecario.AddRange(items);
                    }

                    if (detalleMovContaHipotecario.Count > 0)
                    {
                        dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();
                        /*se suspende consumo seven*/
                        //var bodyHipotecario = new InsertarMovContableRequestBody(dataMovContaHipotecario);
                        //movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                        //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                        //response.ReturnMessage += response.Data is not null ? " Resultado Mov.Contable Seven Créditos Hipotecario: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        //    "para créditos Hipotecarios";
                    }
                    else
                        response.ReturnMessage = "No hay créditos Hípotecarios para crear un mov. contable en seven en el rango de fechas seleccionado";

                }


                if (dataMovContaLeasing.vDetalle.Count() > 0)
                {
                    RequestDetallePago requestDetallePago = new RequestDetallePago();
                    string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataMovContaLeasing.vDetalle);
                    requestDetallePago.Cpg_id = new Guid();
                    requestDetallePago.Emp_codi = dataMovContaLeasing.Emp_codi;
                    requestDetallePago.Top_codi = dataMovContaLeasing.Top_codi;
                    requestDetallePago.Mco_fech = dataMovContaLeasing.Mco_fech;
                    requestDetallePago.Mod_codi = dataMovContaLeasing.Mod_codi;
                    requestDetallePago.Mco_desc = dataMovContaLeasing.Mco_desc;
                    requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                    requestDetallePago.Respuesta_Seven = string.Empty;
                    requestDetallePago.Fecha_Ejecucion = DateTime.Now;


                    decimal totalDmcVacr = Math.Round(dataMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr), 2);
                    decimal totalDmcCadb = Math.Round(dataMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb), 2);
                    var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CAC", totalDmcVacr, totalDmcCadb);
                    //var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
                    requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                    response.ReturnMessage = requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable Cierre Anual leasing: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                }
                if (dataMovContaHipotecario.vDetalle.Count() > 0)
                {
                    RequestDetallePago requestDetallePago = new RequestDetallePago();
                    string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataMovContaHipotecario.vDetalle);
                    requestDetallePago.Cpg_id = new Guid();
                    requestDetallePago.Emp_codi = dataMovContaHipotecario.Emp_codi;
                    requestDetallePago.Top_codi = dataMovContaHipotecario.Top_codi;
                    requestDetallePago.Mco_fech = dataMovContaHipotecario.Mco_fech;
                    requestDetallePago.Mod_codi = dataMovContaHipotecario.Mod_codi;
                    requestDetallePago.Mco_desc = dataMovContaHipotecario.Mco_desc;
                    requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                    requestDetallePago.Respuesta_Seven = string.Empty;
                    requestDetallePago.Fecha_Ejecucion = DateTime.Now;



                    decimal totalDmcVacr = Math.Round(dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                    decimal totalDmcCadb = Math.Round(dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);
                    var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CAC", totalDmcVacr, totalDmcCadb);
                    //var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CMC");
                    requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                    response.ReturnMessage += requestDetallePago.Cc_Id is not null ? "Resultado Comprobante Contable cierre Anual  hipotecario: " + requestDetallePago.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos hipotecarios, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                }
                return response;
            }
            return response;
        }
        /// ═════════════════════════════════════════════════════════════════════════════════
        /// FUNCIONALIDAD: Crear Comprobante de Reversión y Enviar a Seven
        /// ═════════════════════════════════════════════════════════════════════════════════
        /// Después de eliminar afiliados, se crea un comprobante ESPEJO (inversión de débitos/créditos)
        /// y se envía a Seven automáticamente

        public async Task<Response<CrearComprobanteReversionDto>> CrearComprobanteReversionYEnviarASevenAsync(
            int ccIdOriginal)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────
                // PASO 1: Obtener comprobante original de BD
                // ─────────────────────────────────────────────────────────────────────
                var comprobanteOriginal = await _carteraRepository.ObtenerDetallesPagosSevenCartera(ccIdOriginal);

                if (!comprobanteOriginal.Any())
                {
                    return new Response<CrearComprobanteReversionDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = $"Comprobante {ccIdOriginal} no encontrado"
                    };
                }    
                var comprobanteData = comprobanteOriginal.FirstOrDefault();
                // ─────────────────────────────────────────────────────────────────────
                // PASO 2: Obtener movimientos contables originales
                // ─────────────────────────────────────────────────────────────────────
                MovimientosContableParametersDto parametros = new MovimientosContableParametersDto
                {
                    IdComprobante = comprobanteData.Cpg_id,
                    NumeroPagina = 500,
                    RegistrosPorPagina = 100
                };

                var movimientosOriginales = this._mapper.Map<List<MovimientoComprobantePorProcesoDto>>(
                    await _carteraRepository.ObtenerMovimientosContableCartera(parametros));

                if (movimientosOriginales == null || movimientosOriginales.Count == 0)
                {
                    return new Response<CrearComprobanteReversionDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = $"No hay movimientos para el comprobante {ccIdOriginal}"
                    };
                }
                // ─────────────────────────────────────────────────────────────────────
                // PASO 3:  INVERTIR débitos y créditos (REVERSIÓN)
                // ─────────────────────────────────────────────────────────────────────
                var movimientosInvertidos = new List<MovimientoComprobantePorProcesoDto>();



                foreach (var movOriginal in movimientosOriginales)
                {
                    var movInvertido = new MovimientoComprobantePorProcesoDto
                    {
                        //  Se mantienen igual (identificación)
                        Movimiento_Comprobante_CarteraId = movOriginal.Movimiento_Comprobante_CarteraId,
                        Comprobante_CarteraId = movOriginal.Comprobante_CarteraId,
                        Numero_Identificacion = movOriginal.Numero_Identificacion,
                        Nombre_Tercero = movOriginal.Nombre_Tercero,

                        // Datos contables (se mantienen)
                        Cuenta_Contable = movOriginal.Cuenta_Contable,
                        Descripcion_Cuenta = movOriginal.Descripcion_Cuenta,
                        Tipo_Credito = movOriginal.Tipo_Credito,
                        Centro_Costo = movOriginal.Centro_Costo,
                        Centro_Costo_Credito = movOriginal.Centro_Costo_Credito,
                        Unidad_Ejecutora = movOriginal.Unidad_Ejecutora,
                        Detalle = movOriginal.Detalle,

                        //  INVERSIÓN: Crédito → Débito
                        Debito = movOriginal.Credito,  // Lo que era crédito ahora es débito

                        // INVERSIÓN: Débito → Crédito
                        Credito = movOriginal.Debito,  // Lo que era débito ahora es crédito

                        // Otros datos se mantienen
                        TIPO_OPERACION = movOriginal.TIPO_OPERACION,
                        Numero_Credito = movOriginal.Numero_Credito,
                        Comprobante_Interno = movOriginal.Comprobante_Interno,
                        Comprobante_Contable = movOriginal.Comprobante_Contable,
                        Tipo_Comprobante = movOriginal.Tipo_Comprobante,
                        Total_Registros = movOriginal.Total_Registros
                    };

                    movimientosInvertidos.Add(movInvertido);

                }
                // ─────────────────────────────────────────────────────────────────────
                // PASO 4: Calcular nuevos totales (ahora invertidos)
                // ─────────────────────────────────────────────────────────────────────
                decimal totalCreditoInvertido = Math.Round(movimientosInvertidos.Sum(x => x.Credito), 2);
                decimal totalDebitoInvertido = Math.Round(movimientosInvertidos.Sum(x => x.Debito), 2);

                // ─────────────────────────────────────────────────────────────────────
                // PASO 5: Armar objeto TSCnDmcon de Seven (estructura de detalles)
                // ─────────────────────────────────────────────────────────────────────
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovimientosInvertidos =
                    new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

                foreach (var mov in movimientosInvertidos)
                {
                    // Aquí mapeas a la estructura de Seven
                    // Asumiendo que tienes un método MapearASevenDmcon o similar
                    var dmconItem = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                    {
                        // Mapear tus campos a los campos de Seven
                        // Esto depende de tu estructura exacta de Seven
                    };

                    detalleMovimientosInvertidos.Add(dmconItem);
                }


                // ─────────────────────────────────────────────────────────────────────
                // PASO 6: Armar objeto TSCnMcont de Seven (cabecera del movimiento)
                // ─────────────────────────────────────────────────────────────────────
                var dataMovContaReversion = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont
                {
                    Emp_codi = comprobanteData.Emp_codi,           // Empresa (original)
                    Top_codi = comprobanteData.Top_codi,           // Tipo de operación (original)
                    Mco_nume = 0,                                  // Número 0 (Seven lo asigna)
                    Mco_fech = DateTime.Now,                       // Fecha de hoy
                    Mod_codi = comprobanteData.Mod_codi,           // Módulo (original)
                    Arb_csuc = "1001",                             // Sucursal
                    Mco_desc = $"REVERSIÓN - {comprobanteData.Mco_desc}", // Descripción
                    vDetalle = detalleMovimientosInvertidos.ToArray()     // Detalles invertidos
                };
                // ─────────────────────────────────────────────────────────────────────
                // PASO 7: Serializar movimientos a JSON
                // ─────────────────────────────────────────────────────────────────────
                string jsonMovimientosInvertidos = System.Text.Json.JsonSerializer.Serialize(
                    dataMovContaReversion.vDetalle,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // ─────────────────────────────────────────────────────────────────────
                // PASO 8: Crear RequestDetallePago para insertar
                // ─────────────────────────────────────────────────────────────────────
                RequestDetallePago requestDetallePagoReversion = new RequestDetallePago
                {
                    Cpg_id = Guid.NewGuid(),                       // Nuevo GUID para la reversión
                    Emp_codi = comprobanteData.Emp_codi,
                    Top_codi = comprobanteData.Top_codi,
                    Mco_fech = DateTime.Now,
                    Mod_codi = comprobanteData.Mod_codi,
                    Mco_desc = $"REVERSIÓN - {comprobanteData.Mco_desc}",
                    Mov_Contable_Seven = jsonMovimientosInvertidos,
                    Respuesta_Seven = string.Empty,               // Aún sin respuesta
                    Fecha_Ejecucion = DateTime.Now
                };


                // ─────────────────────────────────────────────────────────────────────
                // PASO 9: Generar comprobante interno
                // ─────────────────────────────────────────────────────────────────────
                // IMPORTANTE: Usas tu método existente GenerarComprobanteContabilidad
                // que ya sabe cómo crear el comprobante en tu BD

                //var resultadoComprobanteReversion = await GenerarComprobanteContabilidad(
                //    requestDetallePagoReversion,
                //    "CAC",  // Tipo de comprobante (asumiendo que es el mismo que usas)
                //    totalCreditoInvertido,
                //    totalDebitoInvertido);

                //if (!resultadoComprobanteReversion.IsSuccess)
                //{
                //    return new Response<CrearComprobanteReversionDto>
                //    {
                //        Data = null,
                //        IsSuccess = false,
                //        ReturnMessage = $"Error al generar comprobante de reversión: {resultadoComprobanteReversion.ReturnMessage}"
                //    };
                //}

               // requestDetallePagoReversion.Cc_Id = resultadoComprobanteReversion.Data.Comprobante_Id;
                // ─────────────────────────────────────────────────────────────────────
                // PASO 10: Insertar en tablas internas
                // ─────────────────────────────────────────────────────────────────────
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoReversion);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoReversion);


                // ─────────────────────────────────────────────────────────────────────
                // PASO 11: OPCIONAL - Enviar a Seven (si está habilitado)
                // ─────────────────────────────────────────────────────────────────────
                // Tu código menciona que está suspendido (_wsMovContable)
                // Aquí iría si lo reactivas:
                /*
                var bodyReversion = new InsertarMovContableRequestBody(dataMovContaReversion);
                var movContableRequest = new InsertarMovContableRequest(bodyReversion);
                var responseSevenReversion = await _wsMovContable.InsertarMovContableAsync(
                    movContableRequest, 
                    cancellationToken);

                if (responseSevenReversion is not null)
                {
                    requestDetallePagoReversion.Respuesta_Seven = responseSevenReversion.Body.InsertarMovContableResult;
                    await _carteraRepository.ActualizarRespuestaSevenPago(requestDetallePagoReversion);
                }
                */


                // ─────────────────────────────────────────────────────────────────────
                // PASO 12: Preparar respuesta
                // ─────────────────────────────────────────────────────────────────────
                var respuesta = new CrearComprobanteReversionDto
                {
                    CC_ID_Original = ccIdOriginal,
                    CC_ID_Reversion = requestDetallePagoReversion.Cc_Id,
                    Cpg_Id_Reversion = requestDetallePagoReversion.Cpg_id,
                    TotalCreditoInvertido = totalCreditoInvertido,
                    TotalDebitoInvertido = totalDebitoInvertido,
                    CantidadMovimientosInvertidos = movimientosInvertidos.Count,
                    FechaCreacion = requestDetallePagoReversion.Fecha_Ejecucion,
                    Estado = "CREADO",
                    Descripcion = $"Comprobante de reversión para CC_ID {ccIdOriginal}"
                };

                Console.WriteLine($"[REVERSION]  Comprobante de reversión creado exitosamente");

                return new Response<CrearComprobanteReversionDto>
                {
                    Data = respuesta,
                    IsSuccess = true,
                    ReturnMessage = $"Comprobante de reversión creado: CC_ID {requestDetallePagoReversion.Cc_Id}"
                };
            }
            catch (Exception ex)
            {
                return new Response<CrearComprobanteReversionDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = ex.Message
                };
            }
        }

        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleCierreAnualSeven(ReporteCierreAnualCarteraDto pago)
        {

            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (pago.ProvisionGenerica > 0 && pago.CalificacionCartera == "A")
            {
                var equivalenciaCuentasCategoriaA = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9072);
                var cuentasAfectar = equivalenciaCuentasCategoriaA.Where(x => x.REF_COLUMN_APLICA_PAGO == "Cierres");

                foreach (var item in cuentasAfectar)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: " + pago.Identificacion;
                    itemDetalle.Dmc_cant = 1;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = pago.NumeroCredito.ToString();
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? pago.ProvisionGenerica : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? pago.ProvisionGenerica : 0;
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = pago.Identificacion;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }

                }

            }
            if (pago.ProvisionGenerica > 0 && pago.CalificacionCartera != "A")
            {
                IEnumerable<EquivalenciaCuentasContablesSeven> equivalenciaCuentasCategorias;
                IEnumerable<EquivalenciaCuentasContablesSeven> cuentasAfectar;

                if (pago.TipoCredito == "LEASING HABITACIONAL")
                {
                    equivalenciaCuentasCategorias = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9072);
                    cuentasAfectar = equivalenciaCuentasCategorias.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "Cierres"));
                }
                else
                {
                    equivalenciaCuentasCategorias = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9004);
                    cuentasAfectar = equivalenciaCuentasCategorias.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "Cierres"));
                }
                if (pago.CalificacionCartera == "B")
                {
                    cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_B_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual");
                    if (pago.AbonoInteresRemanente > 0)
                    {
                        cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_B_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Intereses_Anual");
                    }
                }
                else if (pago.CalificacionCartera == "C")
                {
                    cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_C_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual");
                    if (pago.AbonoInteresRemanente > 0)
                    {
                        cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_C_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Intereses_Anual");
                    }
                }
                else if (pago.CalificacionCartera == "D")
                {
                    cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_D_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual");
                    if (pago.AbonoInteresRemanente > 0)
                    {
                        cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_D_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Intereses_Anual");
                    }
                }
                else if (pago.CalificacionCartera == "E")
                {
                    cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_E_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual");
                    if (pago.AbonoInteresRemanente > 0)
                    {
                        cuentasAfectar = equivalenciaCuentasCategorias.Where(x => x.REF_COLUMN_APLICA_PAGO == "Categoria_E_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Cartera_Anual" || x.REF_COLUMN_APLICA_PAGO == "Provision_Intereses_Anual");
                    }
                }
                foreach (var item in cuentasAfectar)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: " + pago.Identificacion;
                    itemDetalle.Dmc_cant = 1;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = pago.NumeroCredito.ToString();
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? pago.ProvisionGenerica : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? pago.ProvisionGenerica : 0;
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = pago.Identificacion;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }

                }
            }

            return items;
        }
        /// <summary>
        /// ExisteCierreMensualCartera
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<bool>> ExisteCierreMensualCartera(ParamCierreMensualCarteraDto request)
        {
            Response<bool> respuesta = new()
            {
                Data = false,
                IsSuccess = false
            };
            Task<bool> existeCierre = ExisteCierreMensualCarteraAsync(request.AnioCierre, request.MesCierre);

            if (existeCierre.Result == true)
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Ya existe un Cierre Mensual de Cartera programado para el periodo " + request.AnioCierre + "-" + request.MesCierre + " seleccione un periodo diferente.";
                return respuesta;
            }

            return respuesta;
        }

        private async Task<bool> ExisteCierreMensualCarteraAsync(int anio, int mes)
        {
            bool encontroProgramacion = await _carteraRepository.ExisteCierreMensualCartera(anio, mes);
            return encontroProgramacion;
        }


        public async Task<Response<NovedadCarteraDto>> CrearNovedadCartera(NovedadCarteraDto novedad)
        {
            try
            {
                // PASO 1: VALIDACIONES COMUNES
                var validacionCredito = await ValidarCreditoParaNovedad(novedad.IdCredito);
                if (!validacionCredito.IsSuccess)
                    return new Response<NovedadCarteraDto> { Data = null, IsSuccess = false, ReturnMessage = validacionCredito.ReturnMessage };

                var credito = validacionCredito.Data;
                var novedadCartera = this._mapper.Map<NovedadCartera>(novedad);

                // PASO 2: CONFIGURAR VALORES ESPECÍFICOS POR TIPO
                ConfigurarNovedadPorTipo(ref novedadCartera, novedad, credito);

                // PASO 3: PROCESAR SEGÚN TIPO DE NOVEDAD
                return novedad.IdTipoNovedad switch
                {
                    (int)TipoNovedadCarteraEnum.RESTITUCIÓN_INMUEBLE =>
                        await ProcesarRestitucionInmueble(novedad, credito, novedadCartera),
                    (int)TipoNovedadCarteraEnum.REDIFERIDO =>
                        await ProcesarRediferido(novedad, credito, novedadCartera),
                    (int)TipoNovedadCarteraEnum.REESTRUCTURACIÓN =>
                        await ProcesarReestructuracion(novedad, credito, novedadCartera),
                    _ => await ProcesarNovedadGeneral(novedad, credito, novedadCartera)
                };
            }
            catch (Exception ex)
            {
                return $"Error al crear novedad: {ex.Message}".BuildError<NovedadCarteraDto>();
            }
        }

        // ================================================================================
        // VALIDACIONES
        // ================================================================================
        private async Task<Response<InformacionCreditoDto>> ValidarCreditoParaNovedad(Guid idCredito)
        {
            var credito = _mapper.Map<InformacionCreditoDto>(
                await _carteraRepository.ObtenerCreditoCarteraById(idCredito)
            );

            if (credito == null)
                return "No se encontró el crédito".BuildError<InformacionCreditoDto>();

            if (credito.SaldoCapitalMora > 0)
                return "Para realizar la novedad el crédito debe estar al día".BuildError<InformacionCreditoDto>();

            var novedadesVigentes = _mapper.Map<IEnumerable<NovedadCarteraDto>>(
                await _carteraRepository.ObtenerNovedadesCartera(idCredito)
            );

            if (novedadesVigentes.Any(x => x.CodigoEstadoNovedad == "VIGENTE"))
                return "Hay otra novedad en estado VIGENTE".BuildError<InformacionCreditoDto>();

            return new Response<InformacionCreditoDto> { Data = credito, IsSuccess = true };
        }

        private async Task<Response<bool>> ValidarRediferidoYReestructuracion(InformacionCreditoDto credito)
        {
            if (credito.DiasMora > 180)
                return new Response<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    ReturnMessage = "Se ha superado el límite de 180 días de mora."
                };

            var novedadesAnteriores = await _carteraRepository.ObtenerNovedadRediferido(credito.IdCredito);
            if (!novedadesAnteriores?.Any() ?? true)
                return new Response<bool> { Data = true, IsSuccess = true };

            var lastDate = novedadesAnteriores.OrderByDescending(x => x.FechaCreacion).First().FechaCreacion;
            if (DateTime.Now >= lastDate.AddYears(1))
                return new Response<bool> { Data = true, IsSuccess = true };
            return new Response<bool>
            {
                Data = false,
                IsSuccess = false,
                ReturnMessage = "Solo se puede hacer una reestructuración/rediferido por año."
            };
        }

        // ================================================================================
        // CONFIGURACIÓN DE VALORES POR TIPO DE NOVEDAD
        // ================================================================================
        private void ConfigurarNovedadPorTipo(ref NovedadCartera novedadCartera, NovedadCarteraDto novedad, InformacionCreditoDto credito)
        {
            switch (novedad.IdTipoNovedad)
            {
                case 5: // Cancelación
                    novedadCartera.VALOR_PAGADO = novedad.TotalDeuda;
                    break;
                case (int)TipoNovedadCarteraEnum.REDIFERIDO:
                    novedadCartera.CAN_ID = 5;
                    break;
                case (int)TipoNovedadCarteraEnum.OTROS:
                    novedadCartera.NVC_VALOR_NUEVA_TASA /= 100;
                    break;
                case (int)TipoNovedadCarteraEnum.REESTRUCTURACIÓN:
                    novedadCartera.CAN_ID = 6;
                    break;
            }
        }

        // ================================================================================
        // PROCESADORES POR TIPO DE NOVEDAD
        // ================================================================================
        private async Task<Response<NovedadCarteraDto>> ProcesarNovedadGeneral(
            NovedadCarteraDto novedad, InformacionCreditoDto credito, NovedadCartera novedadCartera)
        {
            // Actualizar tasa si aplica
            if (novedad.IdTipoNovedad == (int)TipoNovedadCarteraEnum.OTROS)
                await ActualizacionTasaCredito(novedad);

            var resultado = await _carteraRepository.CrearNovedadCartera(novedadCartera);
            return new Response<NovedadCarteraDto>
            {
                Data = _mapper.Map<NovedadCarteraDto>(resultado),
                IsSuccess = true
            };
        }

        private async Task<Response<NovedadCarteraDto>> ProcesarRestitucionInmueble(
            NovedadCarteraDto novedad, InformacionCreditoDto credito, NovedadCartera novedadCartera)
        {
            var responseData = await CrearRestitucionInmueble(new RestitucionInmuebleCarteraDto
            {
                TipoNovedad = novedad.IdTipoNovedad,
                TipoDpcumento = credito.TipoIdentificacion,
                NumeroDocumento = credito.NumeroIdentificacion,
                IdCredito = novedad.IdCredito,
                IdTipoCredito = credito.IdTipoCredito,
                NumeroCredito = credito.NumeroCredito,
                FechaNovedad = novedad.FechaNovedad,
                FechaFinalizacion = novedad.FechaFinalizacion,
                CreadoPor = novedad.CreadoPor,
                FechaCreacion = DateTime.Now,
                Capital = novedad.Capital,
                SeguroVida = novedad.SeguroVida,
                SeguroHogar = novedad.SeguroHogar,
                InteresCorriente = novedad.InteresCorriente,
                InteresMora = novedad.InteresMora,
                CapitalRemanente = novedad.CapitalRemanente,
                InteresRemanente = novedad.InteresRemanente,
                TotalDeuda = 0,
                ValorPagado = novedad.TotalDeuda,
                ValorRestante = novedad.ValorRestante,
            });

            if (!responseData.IsSuccess)
                return new Response<NovedadCarteraDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = responseData.ReturnMessage
                };

            await CrearCuotaCartera(new RequestCarteraSeven
            {
                NumeroDocumentoCliente = credito.NumeroIdentificacion,
                tipoIdentificacion = credito.TipoIdentificacion,
                AplicaSimulacion = false,
                numeroCredito = credito.NumeroCredito,
                cpgId = responseData.Data,
                tipoCredito = credito.IdTipoCredito,
                nCuota = 1,
                codigo = "RESTITUCION_INMUEBLE",
                FechaPago = novedad.FechaFinalizacion
            }, default);

            var resultado = await _carteraRepository.CrearNovedadCartera(novedadCartera);
            return new Response<NovedadCarteraDto>
            {
                Data = _mapper.Map<NovedadCarteraDto>(resultado),
                IsSuccess = true
            };
        }

        private async Task<Response<NovedadCarteraDto>> ProcesarRediferido(
            NovedadCarteraDto novedad, InformacionCreditoDto credito, NovedadCartera novedadCartera)
        {
            // Validar condiciones
            var validacion = await ValidarRediferidoYReestructuracion(credito);
            if (!validacion.IsSuccess)
                return new Response<NovedadCarteraDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = validacion.ReturnMessage
                };

            if (credito.DiasMora > 0)
                return new Response<NovedadCarteraDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Tiene {credito.DiasMora} días de mora. Para aplicar rediferido, debe pagar ${credito.AcumuladoInteresCorriente:N2}"
                };

            // Crear novedad base
            var novedadDb = await _carteraRepository.CrearNovedadCartera(novedadCartera);

            // Crear novedad en tabla rediferido
            var requestRediferido = new RequestNovedadRediferido
            {
                NumeroDocumentoCliente = credito.NumeroIdentificacion,
                TipoIdentificacion = credito.TipoIdentificacion,
                NumeroCredito = credito.NumeroCredito,
                CreditoId = credito.IdCredito,
                TipoCredito = credito.IdTipoCredito,
                FechaCierre = novedad.FechaFinalizacion,
                CuotaCredito = (int)novedad.CuotaCredito
            };

            try
            {
                await CrearNovedadRediferido(requestRediferido);

                // Aplicar ampliación de cuota
                var requestPago = new PagoAplicarDto
                {
                    Tcr_id = credito.IdTipoCredito,
                    NumeroCredito = credito.NumeroCredito,
                    Identificacion = credito.NumeroIdentificacion,
                    NumeroCuotas = (int)novedad.CuotaCredito,
                    Codigo = "REDIFERIDO_CARTERA",
                    Fecha_Pago_Cuota = novedad.FechaNovedad,
                    CreadoPor = novedad.CreadoPor,
                    ValorPago = (decimal)novedad.ValorPagado
                };

                await AmpliacionCuota(requestPago);

                return new Response<NovedadCarteraDto>
                {
                    Data = _mapper.Map<NovedadCarteraDto>(novedadDb),
                    IsSuccess = true,
                    ReturnMessage = "Rediferido creado y aplicado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return $"Novedad creada pero error al generar plan de pagos: {ex.Message}".BuildError<NovedadCarteraDto>();
            }
        }

        private async Task<Response<NovedadCarteraDto>> ProcesarReestructuracion(
            NovedadCarteraDto novedad, InformacionCreditoDto credito, NovedadCartera novedadCartera)
        {
            // Validar condiciones
            var validacion = await ValidarRediferidoYReestructuracion(credito);
            if (!validacion.IsSuccess)
                return new Response<NovedadCarteraDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = validacion.ReturnMessage
                };

            // Generar informe
            var requestPago = new PagoAplicarDto
            {
                Tcr_id = credito.IdTipoCredito,
                NumeroCredito = credito.NumeroCredito,
                Identificacion = credito.NumeroIdentificacion,
                NumeroCuotas = (int)novedad.CuotaCredito,
                Codigo = "REESTRUCTURACION",
                Fecha_Pago_Cuota = novedad.FechaNovedad,
                Fecha_Pago = novedad.FechaNovedad,
                CreadoPor = novedad.CreadoPor,
                ValorPago = (decimal)novedad.ValorPagado,
            };
            try
            {

                var valorRestructurado = novedad.TotalDeuda - novedad.ValorPagado;
                var requestConsultaCreditoCartera = new ObtenerProductoCartera
                {
                    Nro_Credito = credito.NumeroCredito,
                    Tcr_Id = credito.IdTipoCredito
                };

                var obtenerInformacionCredito = await _carteraRepository.ObtenerCreditoCartera(requestConsultaCreditoCartera);
                var requestObtenerProductoCliente = new SolicitudProductoPorClienteDto
                {
                    Identificacion = obtenerInformacionCredito.CLI_IDENTIFICACION,
                    TipoIdentificacion = obtenerInformacionCredito.TIPO_IDENTIFICACION
                };
                var obtenerProductoCliente = await ObtenerProductoCliente(requestObtenerProductoCliente);
                var obtenerPlanPagos = await ObtenerPlanPagosCredito(requestConsultaCreditoCartera);
                var ultimoPlanPago = obtenerPlanPagos.Data.PlanPago.LastOrDefault();
                obtenerInformacionCredito.CRE_TASA_SEGURO_HOGAR = (float)novedad.PorcentajeSeguroHogar;
                obtenerInformacionCredito.CRE_TASA_SEGURO_VIDA = (float)novedad.PorcentajeSeguroVida;
                var acumuladoCapitalPorPagar = obtenerProductoCliente.Data.Where(x => x.NumeroCredito == credito.NumeroCredito).FirstOrDefault().SaldoCapitalRemanenteFinal;
                var acumuladoInteresPorPagar = obtenerProductoCliente.Data.Where(x => x.NumeroCredito == credito.NumeroCredito).FirstOrDefault().InteresesRemanenteFinal;
                var acumuladoPorPagar = acumuladoCapitalPorPagar + acumuladoInteresPorPagar + obtenerInformacionCredito.CRE_VALOR_OPCION_COMPRA;
                obtenerInformacionCredito.CRE_SALDO_CAPITAL = acumuladoPorPagar - (decimal)novedad.ValorPagado;
                var responsePlanPago = await ReCalcularPlanHipotecarioRestructuracion(obtenerInformacionCredito,(int)novedad.CuotaCredito, ultimoPlanPago);
                var nuevoPlazo = obtenerInformacionCredito.CRE_PLAZO_TOTAL + novedad.CuotaCredito;
                await ActualizarPlanPagosMasivoOptimizado(responsePlanPago, credito.NumeroCredito, credito.IdTipoCredito);
                await _carteraRepository.ActualizarTasaEACredito(obtenerInformacionCredito.CRE_ID, (decimal)obtenerInformacionCredito.CRE_TASA_SEGURO_HOGAR, (int)nuevoPlazo, (decimal)obtenerInformacionCredito.CRE_TASA_SEGURO_VIDA, 0);
                novedadCartera.NVC_TOTAL_DEUDA = (decimal)novedad.ValorPagado;
                novedadCartera.NVC_CAPITAL = (decimal)novedad.ValorPagado - acumuladoInteresPorPagar;
                novedadCartera.NVC_CAPITAL_REMANENTE = acumuladoCapitalPorPagar;

                var updateStatu = await _carteraRepository.AplicarPagoCancelacionRestructuracion(new PagoAplicarDto
                {
                    NumeroCredito = (long)credito.NumeroCredito,
                    ValorPago = (decimal)novedad.ValorPagado,
                    Fecha_Pago = novedad.FechaFinalizacion,
                    Fecha_Pago_Cuota = novedad.FechaFinalizacion,
                    CreadoPor = novedad.CreadoPor,
                    AplicaSimulacion = false,
                    Tcr_id = credito.IdTipoCredito,
                    ValorCapitalRemanente = acumuladoCapitalPorPagar,
                    ValorInteresRemanente = acumuladoInteresPorPagar
                });

                var novedadDb = await _carteraRepository.CrearNovedadCartera(novedadCartera);
                await CrearCuotaCartera(new RequestCarteraSeven
                {
                    NumeroDocumentoCliente = credito.NumeroIdentificacion,
                    tipoIdentificacion = credito.TipoIdentificacion,
                    AplicaSimulacion = false,
                    numeroCredito = credito.NumeroCredito,
                    cpgId = updateStatu,
                    tipoCredito = credito.IdTipoCredito,
                    nCuota = 1,
                    codigo = "REESTRUCTURACION",
                    FechaPago = novedad.FechaFinalizacion
                }, default);
                var resultado = _mapper.Map<NovedadCarteraDto>(novedadDb);
                return new Response<NovedadCarteraDto>
                {
                    Data = resultado,
                    IsSuccess = true,
                    ReturnMessage = "Reestructuración aplicada exitosamente. El nuevo plan de pagos ha sido generado."
                };
            }
            catch (Exception ex)
            {
                return $"Novedad creada pero error al aplicar reestructuración: {ex.Message}".BuildError<NovedadCarteraDto>();
            }
        }

        public async Task<Response<AplicacionPagoDto>> Reestructuracion(PagoAplicarDto request)
        {
            try
            {
                // PASO 1: OBTENER INFORMACIÓN DEL CRÉDITO (igual que AmpliacionCuota)
                ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
                {
                    Nro_Credito = request.NumeroCredito,
                    Tcr_Id = request.Tcr_id
                };

                InformacionCredito infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);

                // PASO 2: VALIDACIONES ESPECÍFICAS DE REESTRUCTURACIÓN (HU 30)
                var resultadoValidacion = await ValidarCondicionesReestructuracion(infoCredito, request);
                if (!resultadoValidacion.IsSuccess)
                {
                    return new Response<AplicacionPagoDto>()
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = resultadoValidacion.ReturnMessage
                    };
                }

                // PASO 3: CALCULAR Y VALIDAR PAGO MÍNIMO (intereses corrientes + mora)
                var datosPagoMinimo = await CalcularPagoMinimoReestructuracion(infoCredito);
                if (request.ValorPago < datosPagoMinimo.MontoMinimo)
                {
                    return new Response<AplicacionPagoDto>()
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = $"Pago mínimo requerido: ${datosPagoMinimo.MontoMinimo:N2}. " +
                                       $"Intereses corrientes: ${datosPagoMinimo.InteresesCorrientes:N2}, " +
                                       $"Intereses mora: ${datosPagoMinimo.InteresesMora:N2}. " +
                                       $"Fecha límite: {datosPagoMinimo.FechaLimite:dd/MM/yyyy HH:mm}"
                    };
                }

                // PASO 4: VALIDAR FECHA LÍMITE (48 horas)
                if (!ValidarFechaLimitePago(DateTime.Now.AddDays(-2), request.Fecha_Pago))
                {
                    return new Response<AplicacionPagoDto>()
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "Ha excedido el plazo de 48 horas para realizar el pago de reestructuración."
                    };
                }

                // PASO 5: OBTENER DATOS BASE (igual que AmpliacionCuota)
                var CapitalAbonado = await this._carteraRepository.ObtenerCapitalNeto_AbonosExtra(request.Identificacion);
                var cuotasAcumuladas = await this._iClientRequestProvider.ObtenerCuotasNumeroIdentificacion(infoCredito.CLI_IDENTIFICACION);

                AplicacionPagoDto response = new AplicacionPagoDto();
                List<InsertPlanPagosDto> LstPln = new();
                double NuevaCuotaReducido = 0.0;
                double NuevaCuotaTrad = 0.0;

                // PASO 6: OBTENER TASA VIGENTE PARA REESTRUCTURACIÓN
                var tasaVigenteReestructuracion = await ObtenerTasaVigenteReestructuracion(request.Tcr_id);

                infoCredito.CRE_PLAZO_ACTUAL = request.NumeroCuotas;
                infoCredito.CRE_PLAZO_TOTAL = request.NumeroCuotas;
                // IMPORTANTE: Usar la tasa vigente de reestructuración
                double tasaOriginal = infoCredito.CRE_TASA_CREDITO_PER;
                infoCredito.CRE_TASA_CREDITO_PER = (float)tasaVigenteReestructuracion;

                // PASO 7: PROCESAR SEGÚN TIPO DE CRÉDITO (misma lógica que AmpliacionCuota)
                if (request.Tcr_id == 1)
                {
                    //==============================================================
                    // LEASING - REESTRUCTURACIÓN
                    //==============================================================
                    ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                    if (responsePlanPagoDto != null)
                    {
                        List<ConformacionLeasing> LstConformacionLeasing = new List<ConformacionLeasing>();
                        var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                        var PlazoActual = request.NumeroCuotas - PlazoCancelado;
                        var ValorSaldoActual = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == 1).FirstOrDefault()?.SaldoInicial - (decimal)CapitalAbonado;

                        if (ValorSaldoActual == null)
                        {
                            return new Response<AplicacionPagoDto>()
                            {
                                Data = null,
                                IsSuccess = false,
                                ReturnMessage = "No se pudo calcular el saldo actual del crédito."
                            };
                        }

                        var SaldoNuevo = ValorSaldoActual.Value;

                        var ultimaCuotaPagada = responsePlanPagoDto.PlanPago
                            .Where(x => x.EstadoCuota.Equals("PAGADA") && x.Cuota == PlazoCancelado)
                            .LastOrDefault();

                        if (ultimaCuotaPagada == null)
                        {
                            return new Response<AplicacionPagoDto>()
                            {
                                Data = null,
                                IsSuccess = false,
                                ReturnMessage = "No se encontró la última cuota pagada."
                            };
                        }

                        var DeudaRemanenteNueva = ultimaCuotaPagada.DeudaRemanente - request.ValorPago;
                        var SaldoCapitalTradPagado = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalReducido) +
                                                    (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                        var SaldoCapitalTradActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);
                        var CapitalPorPagar = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                        var SaldoCapitalTradNuevo = (double)SaldoCapitalTradActual - (double)request.ValorPago - CapitalPorPagar;

                        #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                        for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - 1; i++)
                        {
                            double DeudaRemanenteMes = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - 1
                                ? (double)responsePlanPagoDto.PlanPago[i].DeudaRemanente - (double)request.ValorPago
                                : (double)responsePlanPagoDto.PlanPago[i].DeudaRemanente;

                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                                PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].SaldoInicial,
                                PlnCapital = responsePlanPagoDto.PlanPago[i].CapitalReducido,
                                PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                                PlnInteres = responsePlanPagoDto.PlanPago[i].InteresReducido,
                                PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                                PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = (decimal)DeudaRemanenteMes
                            };
                            if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                            {
                                insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                            }
                            LstPln.Add(insertPlanPagosDto);
                        }
                        #endregion

                        // Recalcular plan de pagos con tasa de reestructuración
                        for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i <= infoCredito.CRE_PLAZO_TOTAL - 1; i++)
                        {
                            if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                            {
                                ConformacionLeasing CL = new ConformacionLeasing();
                                CL.Mes = i;
                                //Tradicional
                                var cuotaReferencia = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault();
                                CL.Seguros = cuotaReferencia != null ? (double)cuotaReferencia.SeguroHogar + (double)cuotaReferencia.SeguroVida : 0;
                                CL.Canon = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros, 0);
                                CL.Intereses = Math.Round(SaldoCapitalTradNuevo * tasaVigenteReestructuracion, 0); // USO TASA VIGENTE
                                CL.AporteCapital = CL.Canon - CL.Intereses;
                                CL.SaldoaCapital = SaldoCapitalTradNuevo - CL.AporteCapital;
                                CL.CanonSeguro = CL.Canon + CL.Seguros;

                                //Leasing
                                CL.InteresReducido = Math.Round((double)SaldoNuevo * tasaVigenteReestructuracion, 0); // USO TASA VIGENTE
                                CL.CanonReducido = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros;

                                var cuotaSiguiente = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i + 1).FirstOrDefault();
                                CL.InteresRemanente = infoCredito.CLC_ID != 3 && cuotaSiguiente != null ? (double)cuotaSiguiente.InteresPorPagar : 0;
                                CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                                CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 && cuotaSiguiente != null ? (double)cuotaSiguiente.CapitalPorPagar : 0;
                                CL.SaldoaCapitalReducido = (double)SaldoNuevo - CL.AporteCapitalReducido;
                                CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                                CL.DeudaRemanente = (double)DeudaRemanenteNueva - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                                CL.DeudaRemanenteAnterior = (double)LstPln[i - 1].PlnValorDeudaRemanente;

                                LstConformacionLeasing.Add(CL);

                                InsertPlanPagosDto insertPlanPagosDto = new()
                                {
                                    PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                    PlnCuota = i + 1,
                                    PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                    PlnSaldoIncial = (decimal)LstPln[i - 1].PlnValorDeudaRemanente,
                                    PlnCapital = (decimal)CL.AporteCapitalReducido,
                                    PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                    PlnInteres = (decimal)CL.InteresReducido,
                                    PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                    PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                    PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                    PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                    PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                    PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                                };
                                LstPln.Add(insertPlanPagosDto);

                                // Calcular nueva cuota con tasa de reestructuración
                                NuevaCuotaReducido = Math.Round(await CalcularPago(tasaVigenteReestructuracion, infoCredito.CRE_PLAZO_TOTAL - (i + 1), -CL.SaldoaCapitalReducido, 0, 0), 0);
                                var DifCuota = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros - NuevaCuotaReducido;
                                NuevaCuotaTrad = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros - DifCuota, 0);
                            }
                            else
                            {
                                ConformacionLeasing CL = new ConformacionLeasing();
                                CL.Mes = i;
                                //Tradicional
                                CL.Canon = (double)NuevaCuotaTrad;
                                CL.Intereses = Math.Round(LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital * tasaVigenteReestructuracion, 0);
                                CL.AporteCapital = CL.Canon - CL.Intereses;
                                CL.SaldoaCapital = LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital - CL.AporteCapital;

                                var cuotaReferencia = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault();
                                CL.Seguros = cuotaReferencia != null ? (double)cuotaReferencia.SeguroHogar + (double)cuotaReferencia.SeguroVida : 0;
                                CL.CanonSeguro = CL.Canon + CL.Seguros;

                                //Leasing
                                CL.InteresReducido = Math.Round(LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido * tasaVigenteReestructuracion, 0);
                                CL.CanonReducido = i == infoCredito.CRE_PLAZO_TOTAL ? LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido + CL.InteresReducido : (double)NuevaCuotaReducido;
                                CL.InteresRemanente = infoCredito.CLC_ID != 3 ? CL.Intereses - CL.InteresReducido : 0;
                                CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                                CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? CL.AporteCapital - CL.AporteCapitalReducido : 0;
                                CL.SaldoaCapitalReducido = LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido - CL.AporteCapitalReducido;
                                CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                                CL.DeudaRemanente = LstConformacionLeasing[LstConformacionLeasing.Count - 1].DeudaRemanente - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                                LstConformacionLeasing.Add(CL);

                                InsertPlanPagosDto insertPlanPagosDto = new()
                                {
                                    PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                    PlnCuota = i + 1,
                                    PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                    PlnSaldoIncial = (decimal)LstPln[i - 1].PlnValorDeudaRemanente,
                                    PlnCapital = (decimal)CL.AporteCapitalReducido,
                                    PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                    PlnInteres = (decimal)CL.InteresReducido,
                                    PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                    PlnSeguroHogar = responsePlanPagoDto.PlanPago[i - 1].SeguroHogar,
                                    PlnSeguroVida = responsePlanPagoDto.PlanPago[i - 1].SeguroVida,
                                    PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                    PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                    PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                                };
                                LstPln.Add(insertPlanPagosDto);
                            }
                        }

                        #region Aplicar reestructuración
                        RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                        {
                            LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                            NumeroCredito = request.NumeroCredito,
                            Identificacion = request.Identificacion,
                            Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                            Fecha_Pago = request.Fecha_Pago,
                            NuevaCuotaReducida = NuevaCuotaReducido,
                            NuevaCuotaTradicional = NuevaCuotaTrad,
                            PlazoTotal = infoCredito.CRE_PLAZO_TOTAL,
                            PlazoActual = infoCredito.CRE_PLAZO_ACTUAL,
                            NuevoSaldoReducido = (double)SaldoNuevo,
                            NuevoSaldoTradicional = SaldoCapitalTradNuevo,
                            FechaFinalizaPagos = null,
                            PagoExtraOrdinario = 0
                        };
                        #endregion

                        response = this._mapper.Map<AplicacionPagoDto>(
                            await this._carteraRepository.AplicaPagoReducirCuota(requestPagoAbonoCapital, infoCredito, request)
                        );
                    }
                }
                else
                {
                    //==============================================================
                    // HIPOTECARIO - REESTRUCTURACIÓN
                    //==============================================================
                    ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                    if (responsePlanPagoDto != null)
                    {
                        List<Hipotecario> LstConformacionHipotecario = new();
                        var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                        var PlazoActual = infoCredito.CRE_PLAZO_ACTUAL;
                        var ValorSaldoActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);

                        var SaldoNuevo = ValorSaldoActual - request.ValorPago;

                        #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                        for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i++)
                        {
                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                                PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].Saldo,
                                PlnCapital = responsePlanPagoDto.PlanPago[i].Capital,
                                PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                                PlnInteres = responsePlanPagoDto.PlanPago[i].Interes,
                                PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                                PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = responsePlanPagoDto.PlanPago[i].DeudaRemanente
                            };
                            if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                            {
                                insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                                // Calcular nueva cuota con tasa de reestructuración
                                NuevaCuotaReducido = -Math.Round(await CalcularPago(
                                    tasaVigenteReestructuracion,
                                    infoCredito.CRE_PLAZO_TOTAL - i,
                                    (double)SaldoNuevo,
                                    0,
                                    0), 0) + (double)responsePlanPagoDto.PlanPago[i].SeguroHogar;
                            }
                            LstPln.Add(insertPlanPagosDto);
                        }
                        #endregion

                        // Recalcular plan de pagos con tasa de reestructuración
                        for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1; i <= infoCredito.CRE_PLAZO_TOTAL; i++)
                        {
                            var SaldoEvaluado = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1 ? (double)SaldoNuevo : LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo;
                            Hipotecario H = new();
                            H.Cuota = i;
                            H.Seguro_Vida = Math.Round(SaldoEvaluado * (infoCredito.CRE_TASA_SEGURO_VIDA / 100), 0);
                            H.Seguro_Hogar = Math.Round((double)infoCredito.CRE_VALOR_VIVIENDA * (infoCredito.CRE_TASA_SEGURO_HOGAR / 100), 0);
                            H.Cuota_Pagadora = Math.Round((double)infoCredito.CRE_VALOR_CUOTA, 0);
                            H.Intereses = Math.Round(SaldoEvaluado * tasaVigenteReestructuracion, 0); // USO TASA VIGENTE
                            H.Capital = H.Cuota_Pagadora - H.Seguro_Vida - H.Seguro_Hogar - H.Intereses;
                            H.Saldo = SaldoEvaluado - H.Capital;

                            if (H.Saldo < 0)
                            {
                                H.Intereses = Math.Round(SaldoEvaluado * tasaVigenteReestructuracion, 0);
                                H.Cuota_Pagadora = SaldoEvaluado + H.Seguro_Vida + H.Seguro_Hogar + H.Intereses;
                                H.Capital = SaldoEvaluado;
                                H.Saldo = SaldoEvaluado - H.Capital;
                                LstConformacionHipotecario.Add(H);

                                InsertPlanPagosDto insertPlanPagosDto = new()
                                {
                                    PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                    PlnCuota = i,
                                    PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                    PlnCapital = (decimal)H.Capital,
                                    PlnSaldoIncial = (decimal)H.Saldo,
                                    PlnCapitalPorPagar = 0,
                                    PlnInteres = (decimal)H.Intereses,
                                    PlnInteresPorPagar = 0,
                                    PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                    PlnSeguroVida = (decimal)H.Seguro_Vida,
                                    PlnAPagar = (decimal)H.Cuota_Pagadora,
                                    PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                    PlnValorDeudaRemanente = 0
                                };

                                LstPln.Add(insertPlanPagosDto);
                                foreach (InsertPlanPagosDto item in LstPln)
                                    item.PlnPlazo = i;
                                break;
                            }
                            else
                            {
                                LstConformacionHipotecario.Add(H);

                                InsertPlanPagosDto insertPlanPagosDto = new()
                                {
                                    PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                    PlnCuota = i,
                                    PlnFechaPago = responsePlanPagoDto.PlanPago[i - 1].Fecha,
                                    PlnCapital = (decimal)H.Capital,
                                    PlnSaldoIncial = (decimal)H.Saldo,
                                    PlnCapitalPorPagar = 0,
                                    PlnInteres = (decimal)H.Intereses,
                                    PlnInteresPorPagar = 0,
                                    PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                    PlnSeguroVida = (decimal)H.Seguro_Vida,
                                    PlnAPagar = (decimal)H.Cuota_Pagadora,
                                    PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                    PlnValorDeudaRemanente = 0
                                };

                                LstPln.Add(insertPlanPagosDto);
                            }
                        }

                        #region Aplicar reestructuración
                        RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                        {
                            LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                            NumeroCredito = request.NumeroCredito,
                            Identificacion = request.Identificacion,
                            Fecha_Pago_Cuota = request.Fecha_Pago,
                            Fecha_Pago = request.Fecha_Pago,
                            NuevaCuotaReducida = NuevaCuotaReducido,
                            NuevaCuotaTradicional = NuevaCuotaTrad,
                            PlazoTotal = infoCredito.CRE_PLAZO_TOTAL,
                            PlazoActual = infoCredito.CRE_PLAZO_ACTUAL,
                            NuevoSaldoReducido = (double)SaldoNuevo,
                            NuevoSaldoTradicional = 0,
                            FechaFinalizaPagos = null,
                            PagoExtraOrdinario = 0
                        };
                        #endregion

                        response = this._mapper.Map<AplicacionPagoDto>(
                            await this._carteraRepository.AplicaPagoReducirCuota(requestPagoAbonoCapital, infoCredito, request)
                        );
                    }
                }

                // PASO 8: GENERAR TRANSACCIONES CONTABLES (HU 30)
                await GenerarTransaccionesReestructuracion(infoCredito, request);

                // PASO 9: MARCAR BLOQUEO/NOVEDAD (CÓDIGO 24 - REESTRUCTURADO)
                await MarcarCreditoReestructurado(request.NumeroCredito, request.CreadoPor.ToString());

                // PASO 10: GUARDAR EN HISTÓRICO
                await GuardarHistoricoReestructuracion(infoCredito, request, datosPagoMinimo, tasaOriginal, tasaVigenteReestructuracion);



                return new Response<AplicacionPagoDto>()
                {
                    Data = response,
                    IsSuccess = true,
                    ReturnMessage = "Reestructuración aplicada exitosamente."
                };
            }
            catch (Exception ex)
            {

                return new Response<AplicacionPagoDto>()
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Error al procesar reestructuración: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Etapa 1: crear la solicitud de reestructuración (solo valida y retorna monto mínimo)
        /// </summary>
        public async Task<Response<SolicitudReestructuracionDto>> CrearSolicitudReestructuracion(PagoAplicarDto request)
        {
            try
            {
                var obtenerProductoCartera = new ObtenerProductoCartera
                {
                    Nro_Credito = request.NumeroCredito,
                    Tcr_Id = request.Tcr_id
                };

                var infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);

                var resultadoValidacion = await ValidarCondicionesReestructuracion(infoCredito, request);
                if (!resultadoValidacion.IsSuccess)
                {
                    return new Response<SolicitudReestructuracionDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = resultadoValidacion.ReturnMessage
                    };
                }

                var datosPagoMinimo = await CalcularPagoMinimoReestructuracion(infoCredito);


                var dto = new SolicitudReestructuracionDto
                {
                    NumeroCredito = request.NumeroCredito,
                    MontoMinimo = datosPagoMinimo.MontoMinimo,
                    InteresesCorrientes = datosPagoMinimo.InteresesCorrientes,
                    InteresesMora = datosPagoMinimo.InteresesMora,
                    FechaLimitePago = datosPagoMinimo.FechaLimite
                };

                return new Response<SolicitudReestructuracionDto>
                {
                    Data = dto,
                    IsSuccess = true,
                    ReturnMessage = "Solicitud creada. Debe pagar el monto mínimo para aplicar la reestructuración."
                };
            }
            catch (Exception ex)
            {

                return new Response<SolicitudReestructuracionDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Error al crear solicitud de reestructuración: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Etapa 2: aplicar la reestructuración solo si el pago mínimo ya se realizó
        /// </summary>
        public async Task<Response<AplicacionPagoDto>> AplicarReestructuracion(PagoAplicarDto request)
        {
            try
            {


                var obtenerProductoCartera = new ObtenerProductoCartera
                {
                    Nro_Credito = request.NumeroCredito,
                    Tcr_Id = request.Tcr_id
                };

                var infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);

                var resultadoValidacion = await ValidarCondicionesReestructuracion(infoCredito, request);
                if (!resultadoValidacion.IsSuccess)
                {
                    return new Response<AplicacionPagoDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = resultadoValidacion.ReturnMessage
                    };
                }

                var datosPagoMinimo = await CalcularPagoMinimoReestructuracion(infoCredito);
                if (request.ValorPago < datosPagoMinimo.MontoMinimo)
                {
                    return new Response<AplicacionPagoDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = $"Pago mínimo requerido: ${datosPagoMinimo.MontoMinimo:N2}. " +
                                        $"Intereses corrientes: ${datosPagoMinimo.InteresesCorrientes:N2}, " +
                                        $"Intereses mora: ${datosPagoMinimo.InteresesMora:N2}. " +
                                        $"Fecha límite: {datosPagoMinimo.FechaLimite:dd/MM/yyyy HH:mm}"
                    };
                }

                if (!ValidarFechaLimitePago(request.Fecha_Pago, request.Fecha_Pago))
                {
                    return new Response<AplicacionPagoDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "Ha excedido el plazo de 48 horas para realizar el pago de reestructuración."
                    };
                }

                // Reutiliza el método completo existente
                return await Reestructuracion(request);
            }
            catch (Exception ex)
            {
                return new Response<AplicacionPagoDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Error al aplicar reestructuración: {ex.Message}"
                };
            }
        }

        // ================================================================================
        // MÉTODOS AUXILIARES PARA REESTRUCTURACIÓN
        // ================================================================================

        /// <summary>
        /// Valida todas las condiciones previas para permitir la reestructuración (HU 30)
        /// </summary>
        private async Task<Response<bool>> ValidarCondicionesReestructuracion(
            InformacionCredito infoCredito,
            PagoAplicarDto request)
        {
            // 1. VALIDAR ESTADO DEL CRÉDITO
            if (infoCredito.ESC_ID == 4)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSuccess = false,
                    ReturnMessage = $"El crédito se encuentra en estado CANCELADO y no puede ser reestructurado."
                };
            }

            // 2. VALIDAR DÍAS DE MORA MÍNIMOS (parámetro > 90 días)
            var parametroDiasMoraMinimo = await this._carteraRepository.ObtenerParametro("DIAS_MORA_MINIMO_REESTRUCTURACION");
            int diasMoraMinimo = 0;
            if (parametroDiasMoraMinimo != null)
            {
                // Si es un DapperRow, accede a la propiedad PAR_VALOR
                if (parametroDiasMoraMinimo is IDictionary<string, object> dict && dict.ContainsKey("PAR_VALOR"))
                {
                    var valor = dict["PAR_VALOR"];
                    diasMoraMinimo = valor != null ? int.Parse(valor.ToString()) : 90;
                }
                else if (int.TryParse(parametroDiasMoraMinimo.ToString(), out int resultado))
                {
                    diasMoraMinimo = resultado;
                }
            }

            if (infoCredito.CRE_DIAS_MORA <= diasMoraMinimo)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSuccess = false,
                    ReturnMessage = $"El crédito debe tener más de {diasMoraMinimo} días de mora para ser reestructurado. Mora actual: {infoCredito.CRE_DIAS_MORA} días."
                };
            }

            // 3. VALIDAR QUE NO HAYA TENIDO MORA EXCESIVA EN ÚLTIMOS 180 DÍAS
            var parametroDiasValidacion = await this._carteraRepository.ObtenerParametro("DIAS_VALIDACION_MORA_REESTRUCTURACION");
            var parametroMoraMaxima = await this._carteraRepository.ObtenerParametro("DIAS_MORA_MAXIMA_PERIODO");




            // 4. VALIDAR NÚMERO MÁXIMO DE REESTRUCTURACIONES (1 por año)
            var parametroMaxReestructuraciones = await this._carteraRepository.ObtenerParametro("MAX_REESTRUCTURACIONES_ANIO");

            return new Response<bool>() { Data = true, IsSuccess = true, ReturnMessage = "OK" };
        }

        /// <summary>
        /// Calcula el pago mínimo requerido (intereses corrientes + intereses de mora) y fecha límite (48h)
        /// </summary>
        private async Task<(decimal MontoMinimo, decimal InteresesCorrientes, decimal InteresesMora, DateTime FechaLimite)>
            CalcularPagoMinimoReestructuracion(InformacionCredito infoCredito)
        {
            // Calcular intereses corrientes pendientes
            var interesesCorrientes = infoCredito.CRE_ACUMULADO_INTERES_CORRIENTE;


            // Calcular intereses de mora
            var interesesMora = infoCredito.CRE_ACUMULADO_INTERES_MORA;
            decimal montoMinimo = interesesCorrientes + interesesMora;
            var parametrizacionHora = 0;
            // Fecha límite: 48 horas desde ahora
            var parametroHorasLimite = await this._carteraRepository.ObtenerParametro("HORAS_LIMITE_PAGO_REESTRUCTURACION");
            if (parametroHorasLimite != null)
            {
                // Si es un DapperRow, accede a la propiedad PAR_VALOR
                if (parametroHorasLimite is IDictionary<string, object> dict && dict.ContainsKey("PAR_VALOR"))
                {
                    var valor = dict["PAR_VALOR"];
                    parametrizacionHora = valor != null ? int.Parse(valor.ToString()) : 90;
                }
            }
            int horasLimite = int.Parse(parametroHorasLimite.ToString() ?? "48");
            DateTime fechaLimite = DateTime.Now.AddHours(horasLimite);

            return (montoMinimo, interesesCorrientes, interesesMora, fechaLimite);
        }

        /// <summary>
        /// Valida que el pago se haya realizado dentro del plazo (48 horas)
        /// </summary>
        private bool ValidarFechaLimitePago(DateTime fechaSolicitud, DateTime fechaPago)
        {
            var horasTranscurridas = (fechaPago - fechaSolicitud).TotalHours;
            return horasTranscurridas <= 48;
        }

        /// <summary>
        /// Obtiene la tasa vigente para reestructuración según tipo de crédito
        /// </summary>
        private async Task<double> ObtenerTasaVigenteReestructuracion(int tipoCredito)
        {
            var tasaVigente = await this._carteraRepository.ObtenerTasaVigenteReestructuracion(tipoCredito);
            return tasaVigente;
        }

        /// <summary>
        /// Genera las dos transacciones contables requeridas: PAGO y CARGO (HU 30)
        /// </summary>
        private async Task GenerarTransaccionesReestructuracion(
            InformacionCredito infoCredito,
            PagoAplicarDto request)
        {
            // Transacción 1: PAGO REESTRUCTURACIÓN
            var transaccionPago = new TransaccionContable
            {
                NumeroCredito = request.NumeroCredito,
                TipoTransaccion = "PAGO_REESTRUCTURACION",
                Monto = request.ValorPago,
                Fecha = request.Fecha_Pago,
                Descripcion = $"Pago por reestructuración de deuda - Crédito {request.NumeroCredito}",
                CreadoPor = request.CreadoPor.ToString() ?? "SYSTEM"
            };

            await this._carteraRepository.RegistrarTransaccion(transaccionPago);

            // Transacción 2: CARGO REESTRUCTURACIÓN (saldo total del crédito)
            decimal saldoTotal = infoCredito.CRE_SALDO_CAPITAL +
                                 infoCredito.CRE_ACUMULADO_INTERES_CORRIENTE +
                                 infoCredito.CRE_ACUMULADO_INTERES_MORA;

            var transaccionCargo = new TransaccionContable
            {
                NumeroCredito = request.NumeroCredito,
                TipoTransaccion = "CARGO_REESTRUCTURACION",
                Monto = saldoTotal,
                Fecha = request.Fecha_Pago,
                Descripcion = $"Cargo por reestructuración de deuda - Crédito {request.NumeroCredito}",
                CreadoPor = request.CreadoPor.ToString() ?? "SYSTEM"
            };

            await this._carteraRepository.RegistrarTransaccion(transaccionCargo);
        }

        /// <summary>
        /// Marca el crédito con bloqueo código 24 - REESTRUCTURADO (HU 32)
        /// </summary>
        private async Task MarcarCreditoReestructurado(long numeroCredito, string usuario)
        {
            await this._carteraRepository.AsignarBloqueoCredito(
                numeroCredito,
                codigoBloqueo: 24, // REESTRUCTURADO
                concepto: "REESTRUCTURADO",
                fechaAsignacion: DateTime.Now
            );
        }

        /// <summary>
        /// Guarda en histórico todos los detalles de la reestructuración
        /// </summary>
        private async Task GuardarHistoricoReestructuracion(
            InformacionCredito infoCredito,
            PagoAplicarDto request,
            (decimal MontoMinimo, decimal InteresesCorrientes, decimal InteresesMora, DateTime FechaLimite) datosPago,
            double tasaAnterior,
            double tasaNueva)
        {
            var historicoReestructuracion = new HistoricoReestructuracion
            {
                NumeroCredito = request.NumeroCredito,
                TipoCredito = request.Tcr_id,
                FechaReestructuracion = request.Fecha_Pago,
                FechaSolicitud = DateTime.Now,
                PlazoAnterior = infoCredito.CRE_PLAZO_ACTUAL,
                PlazoNuevo = request.NumeroCuotas,
                TasaAnterior = tasaAnterior,
                TasaNueva = tasaNueva,
                ValorPagoRealizado = request.ValorPago,
                ValorPagoMinimo = datosPago.MontoMinimo,
                InteresesCorrientes = datosPago.InteresesCorrientes,
                InteresesMora = datosPago.InteresesMora,
                SaldoCapitalAnterior = infoCredito.CRE_SALDO_CAPITAL,
                DiasMoraAlReestructurar = infoCredito.CRE_DIAS_MORA,
                CuotaAnterior = infoCredito.CRE_VALOR_CUOTA,
                CuotaNueva = (decimal)request.NumeroCuotas,
                CreadoPor = request.CreadoPor.ToString() ?? "SYSTEM",
                FechaCreacion = DateTime.Now
            };

            await this._carteraRepository.InsertarHistoricoReestructuracion(historicoReestructuracion);
        }

        ///// <summary>
        ///// Registra log de auditoría (HU 30, 31, 32)
        ///// </summary>
        //private async Task RegistrarLogAuditoria(
        //    string usuario,
        //    string tipoEvento,
        //    string parametroAfectado,
        //    string resultado,
        //    DateTime fecha)
        //{
        //    var logAuditoria = new Log
        //    {
        //        Usuario = usuario,
        //        TipoEvento = tipoEvento,
        //        Fecha = fecha,
        //        Hora = fecha.TimeOfDay,
        //        Resultado = resultado,
        //        ParametroAfectado = parametroAfectado
        //    };

        //    await this._carteraRepository.InsertarLogAuditoria(logAuditoria);
        //}



        private async Task<string> ActualizacionTasaCredito(NovedadCarteraDto request)
        {
            var dataCredito = await _carteraRepository.ObtenerCreditoCarteraById(request.IdCredito);
            var requestPlanPago = new ObtenerProductoCartera
            {
                Nro_Credito = dataCredito.CRE_NRO_CREDITO,
                Tcr_Id = dataCredito.TCR_ID
            };
            var dataPlanPago = await ObtenerPlanPagosCredito(requestPlanPago);
            var datePayment = dataPlanPago.Data.PlanPago.Where(x => x.Fecha.Year == request.FechaProximoPago.Value.Year
             && x.Fecha.Month < request.FechaProximoPago.Value.Month).ToList();
            var applyUpdatePlan = dataPlanPago.Data.PlanPago.Where(x => x.Fecha.Year == request.FechaProximoPago.Value.Year
            && x.Fecha.Month == request.FechaProximoPago.Value.Month).FirstOrDefault();
            if (request.IdTipoCausal == 7)
            {

                if (applyUpdatePlan is not null)
                {

                    dataCredito.CRE_TASA_CREDITO_EA = (float)request.ValorNuevaTasa;
                    var dataHipotecario = await NewPlanHipotecario(dataCredito, applyUpdatePlan.Cuota, datePayment.LastOrDefault());

                    foreach (var item in dataHipotecario)
                    {
                        var requestUpdate = new InsertPlanPagosDto()
                        {
                            PlnNroCredito = dataCredito.CRE_NRO_CREDITO,
                            PlnCuota = item.Cuota,
                            PlnFechaPago = item.Fecha_Pago,
                            PlnSaldoIncial = (decimal)item.Saldo,
                            PlnCapital = (decimal)item.Capital,
                            PlnCapitalPorPagar = 0,
                            PlnInteres = (decimal)item.Intereses,
                            PlnInteresPorPagar = 0,
                            PlnSeguroVida = Math.Round((decimal)item.Seguro_Vida),
                            PlnSeguroHogar = Math.Round((decimal)item.Seguro_Hogar),
                            PlnAPagar = (decimal)(item.Capital + item.Intereses + item.Seguro_Vida + item.Seguro_Hogar),
                            PlnValorDeudaRemanente = 0,
                            PlnPlazo = dataCredito.CRE_PLAZO_TOTAL
                        };
                        var newPlan = _mapper.Map<InsertPlanPagosProductoCartera>(requestUpdate);
                        var response = await _carteraRepository.UpdateNuevaTasaEACredito(newPlan, request.IdTipoCausal, dataCredito.TCR_ID);
                    }
                    var cuota = dataHipotecario.Select(x => x.Cuota_Pagadora).FirstOrDefault();

                    var tasaCredito = (decimal)dataCredito.CRE_TASA_CREDITO_EA / 100m;
                    double tasaPeriodicaNMVDouble = Math.Pow(1 + (double)tasaCredito, 1.0 / 12.0) - 1;
                    decimal tasaPeriodicaNMV = Math.Round((decimal)(tasaPeriodicaNMVDouble * 100), 10, MidpointRounding.AwayFromZero);

                    request.ValorNuevaTasa /= 100;
                    tasaPeriodicaNMV /= 100;
                    var responseUpdateTasa = await _carteraRepository.ActualizarTasaEACredito(dataCredito.CRE_ID, (decimal)request.ValorNuevaTasa, cuota, tasaPeriodicaNMV, request.IdTipoCausal);
                }
            }
            else if (request.IdTipoCausal == 8)
            {
                dataCredito.CRE_TASA_SEGURO_VIDA = (float)request.ValorNuevaTasa;
                if (dataCredito.TCR_ID == 1)
                {
                    var dataHipotecario = await UpdatePlanTasaVidaHipotecario(dataCredito, applyUpdatePlan.Cuota, datePayment.LastOrDefault());
                    foreach (var item in dataHipotecario)
                    {

                        var dtaCuota = dataPlanPago.Data.PlanPago.Where(x => x.Cuota == item.Cuota).FirstOrDefault();
                        var requestUpdate = new InsertPlanPagosDto()
                        {
                            PlnNroCredito = dataCredito.CRE_NRO_CREDITO,
                            PlnCuota = item.Cuota,
                            PlnFechaPago = item.Fecha_Pago,
                            PlnSaldoIncial = (decimal)item.Saldo,
                            PlnCapital = (decimal)item.Capital,
                            PlnCapitalPorPagar = 0,
                            PlnInteres = (decimal)item.Intereses,
                            PlnInteresPorPagar = 0,
                            PlnSeguroVida = Math.Round((decimal)item.Seguro_Vida),
                            PlnSeguroHogar = Math.Round((decimal)item.Seguro_Hogar),
                            PlnAPagar = (dtaCuota.CapitalReducido + dtaCuota.InteresReducido + dtaCuota.SeguroVida + (decimal)item.Seguro_Hogar),
                            PlnValorDeudaRemanente = 0,
                            PlnPlazo = dataCredito.CRE_PLAZO_TOTAL
                        };
                        var newPlan = _mapper.Map<InsertPlanPagosProductoCartera>(requestUpdate);
                        var response = await _carteraRepository.UpdateNuevaTasaEACredito(newPlan, request.IdTipoCausal, dataCredito.TCR_ID);
                    }
                    // var responseUpdateTasa = await _carteraRepository.ActualizarTasaEACredito(dataCredito.CRE_ID, (decimal)request.ValorNuevaTasa, 0, 0, request.IdTipoCausal);
                }
                else
                {
                    var dataHipotecario = await UpdatePlanTasaVidaHipotecario(dataCredito, applyUpdatePlan.Cuota, datePayment.LastOrDefault());
                    foreach (var item in dataHipotecario)
                    {

                        var dtaCuota = dataPlanPago.Data.PlanPago.Where(x => x.Cuota == item.Cuota).FirstOrDefault();
                        var requestUpdate = new InsertPlanPagosDto()
                        {
                            PlnNroCredito = dataCredito.CRE_NRO_CREDITO,
                            PlnCuota = item.Cuota,
                            PlnFechaPago = item.Fecha_Pago,
                            PlnSaldoIncial = (decimal)item.Saldo,
                            PlnCapital = (decimal)item.Capital,
                            PlnCapitalPorPagar = 0,
                            PlnInteres = (decimal)item.Intereses,
                            PlnInteresPorPagar = 0,
                            PlnSeguroVida = Math.Round((decimal)item.Seguro_Vida),
                            PlnSeguroHogar = Math.Round((decimal)item.Seguro_Hogar),
                            PlnAPagar = (dtaCuota.CapitalReducido + dtaCuota.InteresReducido + dtaCuota.SeguroVida + (decimal)item.Seguro_Hogar),
                            PlnValorDeudaRemanente = 0,
                            PlnPlazo = dataCredito.CRE_PLAZO_TOTAL
                        };
                        var newPlan = _mapper.Map<InsertPlanPagosProductoCartera>(requestUpdate);
                        var response = await _carteraRepository.UpdateNuevaTasaEACredito(newPlan, request.IdTipoCausal, dataCredito.TCR_ID);
                    }
                }
            }
            else if (request.IdTipoCausal == 9)
            {
                dataCredito.CRE_TASA_SEGURO_HOGAR = (float)request.ValorNuevaTasa;
                if (dataCredito.TCR_ID == 1)
                {
                    var obtenerTodoPlanPago = await ObtenerPlanPagosCredito(requestPlanPago);
                    var obtenerTodoPlanPagoFilter = obtenerTodoPlanPago.Data.PlanPago.Where(x => x.Cuota >= applyUpdatePlan.Cuota).ToList();      
                    var dataHipotecario = await UpdatePlanTasaHogarLeasing(dataCredito, applyUpdatePlan.Cuota, obtenerTodoPlanPagoFilter);
                    foreach (var item in dataHipotecario)
                    {

                        var dtaCuota = dataPlanPago.Data.PlanPago.Where(x => x.Cuota == item.Cuota).FirstOrDefault();
                        var requestUpdate = new InsertPlanPagosDto()
                        {


                            PlnNroCredito = dataCredito.CRE_NRO_CREDITO,
                            PlnCuota = item.NroCuota,
                            PlnFechaPago = item.FechaPago,
                            PlnSaldoIncial = (decimal)item.DeudaRemanente,
                            PlnCapital = (decimal)item.CapitalAPagar,
                            PlnCapitalPorPagar = (decimal)item.CapitalPorPagar,
                            PlnInteres = (decimal)item.InteresesCorrientesAPagar,
                            PlnInteresPorPagar = (decimal)item.InteresesCorrientesPorPagar,
                            PlnSeguroVida = Math.Round((decimal)item.SegVida, 0),
                            PlnSeguroHogar = Math.Round((decimal)item.SegHogar, 0),
                            PlnAPagar = (decimal)item.CuotaUnidadPagadora,
                            PlnValorDeudaRemanente = (decimal)item.DeudaRemanente,
                            PlnPlazo = dataCredito.CRE_PLAZO_TOTAL
                        };
                        var newPlan = _mapper.Map<InsertPlanPagosProductoCartera>(requestUpdate);
                        var response = await _carteraRepository.UpdateNuevaTasaEACredito(newPlan, request.IdTipoCausal, dataCredito.TCR_ID);
                    }
                    var responseUpdateTasa = await _carteraRepository.ActualizarTasaEACredito(dataCredito.CRE_ID, (decimal)request.ValorNuevaTasa, 0, 0, request.IdTipoCausal);
                }
                else
                {

                    var dataHipotecario = await UpdatePlanHipotecarioTasaHogar(dataCredito, applyUpdatePlan.Cuota, datePayment.LastOrDefault());

                    foreach (var item in dataHipotecario)
                    {
                        var requestUpdate = new InsertPlanPagosDto()
                        {
                            PlnNroCredito = dataCredito.CRE_NRO_CREDITO,
                            PlnCuota = item.Cuota,
                            PlnFechaPago = item.Fecha_Pago,
                            PlnSaldoIncial = (decimal)item.Saldo,
                            PlnCapital = (decimal)item.Capital,
                            PlnCapitalPorPagar = 0,
                            PlnInteres = (decimal)item.Intereses,
                            PlnInteresPorPagar = 0,
                            PlnSeguroVida = Math.Round((decimal)item.Seguro_Vida),
                            PlnSeguroHogar = Math.Round((decimal)item.Seguro_Hogar),
                            PlnAPagar = (decimal)item.Capital + (decimal)item.Intereses + (decimal)item.Seguro_Vida + (decimal)item.Seguro_Hogar,
                            PlnValorDeudaRemanente = 0,
                            PlnPlazo = dataCredito.CRE_PLAZO_TOTAL
                        };
                        var newPlan = _mapper.Map<InsertPlanPagosProductoCartera>(requestUpdate);
                        var response = await _carteraRepository.UpdateNuevaTasaEACredito(newPlan, request.IdTipoCausal, dataCredito.TCR_ID);
                    }
                    var responseUpdateTasa = await _carteraRepository.ActualizarTasaEACredito(dataCredito.CRE_ID, (decimal)request.ValorNuevaTasa, 0, 0, request.IdTipoCausal);
                }

            }
            return "";
        }

        public static decimal CalcularPago(decimal tasa, int plazo, decimal monto)
        {
            if (tasa == 0)
                return monto / plazo;

            var tasaDouble = (double)tasa;
            var montoDouble = (double)monto;

            var pago = (tasaDouble * montoDouble) / (1 - Math.Pow(1 + tasaDouble, -plazo));

            return (decimal)Math.Round(pago, 0); // redondear a 0 decimales como en Excel
        }
        /// <summary>
        /// Recalcula TODO el plan de pagos Leasing desde la fecha de novedad en adelante
        /// Usa el canon reducido del ultimoPlanPago (no cambia cuando solo se actualiza seguro hogar)
        /// </summary>
        /// <param name="crearProductoCliente">Información del crédito</param>
        /// <param name="cuotaCancelada">Número de la cuota desde donde recalcular (fecha novedad)</param>
        /// <param name="ultimoPlanPago">Último plan de pago con CanonReducido, saldos y deuda remanente</param>
        /// <returns>Plan de pagos completo recalculado desde la cuota de novedad en adelante</returns>
        private async Task<List<Leasing>> UpdatePlanTasaHogarLeasing(
            InformacionCredito crearProductoCliente, 
            int cuotaCancelada, 
            List<PlanPagoDto> ultimoPlanPago)
        {
            List<Leasing> PlanSimulacion = new();
            List<ConformacionLeasing> LstConformacionLeasing = new();

            // ========== PASO 1: VALORES VIVIENDA ==========
            var valorViviendaDeseado = Math.Max(
                crearProductoCliente.CRE_VALOR_VIVIENDA,
                     crearProductoCliente.CRE_VALOR_AVALUO_COMERCIAL ?? 0m
            );
            var valorViviendaReal = crearProductoCliente.CRE_VALOR_AVALUO_COMERCIAL ?? 0m;

            // ========== PASO 2: TASAS Y PARÁMETROS ==========
            var porcentajeCanonInicial = crearProductoCliente.CRE_POR_CANON_INICIAL / 100;
            var porcentajeOpcionCompra = crearProductoCliente.CRE_POR_OPCION_COMPRA / 100;
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA / 100;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)tasaCredito), 1.0 / 12.0) - 1;
            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL;

            // ========== PASO 3: CÁLCULOS INICIALES ==========
            var factor = (tasaPeriodicaNMV * Math.Pow((1 + tasaPeriodicaNMV), plazo)) / 
                         (Math.Pow(1 + tasaPeriodicaNMV, plazo) - 1);
            var valorCanonInicial = valorViviendaReal * (decimal)porcentajeCanonInicial;
            var VrCredito = valorViviendaReal - valorCanonInicial;
            var valorSeguroVida = VrCredito * (decimal)tasaSeguroVida;
            var valorSeguroHogar = valorViviendaDeseado * (decimal)tasaSeguroHogar;
            var valorOpcionDeCompra = (decimal)(valorViviendaReal * (decimal)porcentajeOpcionCompra);


            var interesRemanenteTotal = ultimoPlanPago.Sum(x => x.InteresReducido);

            foreach (var item in ultimoPlanPago)
            {
                Leasing L = new Leasing();
                L.FechaCorte = item.Fecha;
                L.FechaPago = item.Fecha;
                L.NroCuota = item.Cuota;
                L.Capital = (double)item.Capital;
                L.IntCte = (double)item.Interes;

                // Seguro de vida: sobre VrCredito + suma de interés remanente
                var baseSeguroVida = (double)VrCredito + (double)interesRemanenteTotal;
                L.SegVida = (double)item.SeguroVida;

                // Seguro hogar: valor actualizado
                L.SegHogar = Math.Round((double)valorSeguroHogar, 0);

                L.Cuota = item.Cuota;
                L.Saldo = (double)item.SaldoCapital;
                L.InteresesCorrientesAPagar = (double)item.InteresReducido;
                L.InteresesCorrientesPorPagar = (double)item.InteresPorPagar;
                L.CapitalAPagar = (double)item.CapitalReducido;
                L.CapitalPorPagar = (double)item.CapitalPorPagar;
                L.SaldoCapitalReducido = (double)item.SaldoCapital;
                L.CuotaUnidadPagadora = L.CapitalAPagar + L.InteresesCorrientesAPagar + L.SegHogar + L.SegVida;
                L.DeudaRemanente = (double)item.DeudaRemanente;

                PlanSimulacion.Add(L);
            }

            return PlanSimulacion;
        }

        private async Task<List<Hipotecario>> UpdatePlanTasaVidaHipotecario(InformacionCredito crearProductoCliente, int cuotaCancelada, PlanPagoDto ultimoPlanPago)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)(tasaCredito)), 1.0 / 12.0) - 1;


            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = (decimal)crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var tasaMixta = tasaPeriodicaNMV + tasaSeguroVida;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL - (cuotaCancelada - 1);

            var baseCuota = CalcularPago((decimal)tasaMixta, plazo, ultimoPlanPago.Saldo);
            if (baseCuota == 0)
            {
                baseCuota = CalcularPago((decimal)tasaMixta, plazo, ultimoPlanPago.Capital);
            }

            var ValorSeguroHogar = ultimoPlanPago.SeguroHogar; //Math.Round(crearProductoCliente.CRE_VALOR_VIVIENDA * (tasaSeguroHogar), 0);
            var cuota = baseCuota + (decimal)ValorSeguroHogar + 1;
            #region Simulacion
            for (int i = cuotaCancelada; i <= crearProductoCliente.CRE_PLAZO_TOTAL; i++)
            {
                var fechaCorte = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                    .AddMonths(2).AddDays(-1);
                int mes = fechaCorte.Month;
                int año = fechaCorte.Year;
                Hipotecario CH = new Hipotecario();
                var saldo = planillaSimulacion.Count == 0
                   ? ultimoPlanPago.Saldo :
                   (decimal)planillaSimulacion.LastOrDefault().Saldo;

                CH.Fecha_Corte = fechaCorte;
                CH.Fecha_Pago = fechaCorte;
                CH.Intereses = (double)Math.Round(saldo * (decimal)tasaPeriodicaNMV, 0, MidpointRounding.AwayFromZero);
                CH.Seguro_Vida = Math.Round((double)saldo * (double)tasaSeguroVida, 0);
                CH.Cuota = i;
                CH.Seguro_Hogar = (double)ValorSeguroHogar;
                CH.Cuota_Pagadora = i == crearProductoCliente.CRE_PLAZO_TOTAL ?
                    Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                    : (double)baseCuota;
                CH.Capital = Math.Round(CH.Cuota_Pagadora - CH.Intereses - CH.Seguro_Vida - CH.Seguro_Hogar, 0);
                CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);
                if (CH.Saldo < 0)
                {
                    CH.Capital = (double)saldo;
                    CH.Cuota_Pagadora = Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);
                    CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);
                    if (CH.Saldo == 0)
                    {
                        planillaSimulacion.Add(CH);
                        break;
                    }
                }
                planillaSimulacion.Add(CH);
            }

            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }


            return planillaSimulacionRound;
            #endregion
        }
        /// <summary>
        /// Recalcula el plan de pagos hipotecario aplicando pagos extraordinarios
        /// Soporta: DISMINUIR EL PLAZO y DISMINUIR EL VALOR DE LA CUOTA
        /// </summary>
        private async Task<List<Hipotecario>> UpdatePlanTasaVidaHipotecario(
             InformacionCredito crearProductoCliente,
             int cuotaCancelada,
             PlanPagoDto ultimoPlanPago,
             decimal valorSeguroHogar)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();

            // Tasas
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)(tasaCredito)), 1.0 / 12.0) - 1;
            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = (decimal)crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var tasaMixta = tasaPeriodicaNMV + tasaSeguroVida;

            // Obtener pagos extraordinarios
            var obtenerPagosExtraOrdinarios = await _carteraRepository.ObtenerAplicacionPago(crearProductoCliente.CRE_ID);
            var pagoFechaActual = obtenerPagosExtraOrdinarios
                .Where(x => x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN"
                         || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA")
                .FirstOrDefault();

            // ========== CAMBIO PRINCIPAL: Calcular saldo ajustado y plazo según modalidad ==========
            // ultimoPlanPago trae la cuota donde se aplicó el pago (cuota 14 - agosto)
            // El saldo de agosto YA tiene el pago extraordinario aplicado
            decimal saldoParaCalculo = ultimoPlanPago.Saldo; // Saldo final de agosto = 239,632,946.67
            int plazoParaCalculo = crearProductoCliente.CRE_PLAZO_TOTAL - cuotaCancelada; // 180 - 14 = 166
            bool esDisminucionCuota = false;

            if (pagoFechaActual != null && pagoFechaActual.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA")
            {
                // Mantener el plazo restante, recalcular cuota con el nuevo saldo
                esDisminucionCuota = true;
            }
            // Si es "DISMINUIR EL PLAZO", el plazo se ajustará dinámicamente cuando el saldo llegue a 0

            // Calcular cuota base con el saldo ajustado y plazo correspondiente
            var baseCuota = CalcularPago((decimal)tasaMixta, plazoParaCalculo, saldoParaCalculo);
            if (baseCuota == 0)
            {
                baseCuota = CalcularPago((decimal)tasaMixta, plazoParaCalculo, ultimoPlanPago.Capital);
            }

            var ValorSeguroHogar = valorSeguroHogar;
            var cuota = baseCuota + (decimal)ValorSeguroHogar + 1;

            #region Simulacion
            // Si hay pago extraordinario, el plan empieza DESPUÉS de la cuota donde se aplicó
            int cuotaInicio = pagoFechaActual != null ? cuotaCancelada  : cuotaCancelada;

            for (int i = cuotaInicio; i <= crearProductoCliente.CRE_PLAZO_TOTAL; i++)
            {
                var fechaCorte = new DateTime();
                if (i == cuotaInicio)
                {
                    // Si hay pago extraordinario, sumamos 3 meses (julio -> septiembre cuota 15)
                    // Si no hay pago, sumamos 2 meses (julio -> agosto cuota 14)
                    int mesesSumar = pagoFechaActual != null ? 3 : 2;
                    fechaCorte = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                       .AddMonths(mesesSumar).AddDays(-1);
                }
                else
                {
                    fechaCorte = new DateTime(planillaSimulacion.LastOrDefault().Fecha_Pago.Year,
                                             planillaSimulacion.LastOrDefault().Fecha_Pago.Month, 1)
                       .AddMonths(2).AddDays(-1);
                }

                Hipotecario CH = new Hipotecario();

                // ========== CAMBIO: Usar saldo ajustado en la primera cuota ==========
                var saldo = planillaSimulacion.Count == 0
                    ? saldoParaCalculo  // Ya incluye el pago extraordinario
                    : (decimal)planillaSimulacion.LastOrDefault().Saldo;

                CH.Fecha_Corte = fechaCorte;
                CH.Fecha_Pago = fechaCorte;
                CH.Intereses = (double)Math.Round(saldo * (decimal)tasaPeriodicaNMV, 0, MidpointRounding.AwayFromZero);
                CH.Seguro_Vida = Math.Round((double)saldo * (double)tasaSeguroVida, 0);
                CH.Cuota = i;
                CH.Seguro_Hogar = (double)ValorSeguroHogar;

                CH.Cuota_Pagadora = i == crearProductoCliente.CRE_PLAZO_TOTAL
                    ? Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                    : (double)cuota;

                CH.Capital = Math.Round(CH.Cuota_Pagadora - CH.Intereses - CH.Seguro_Vida - CH.Seguro_Hogar, 0);
                CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);

                // Aplicar prorrata en cuota 1
                if (i == 1)
                {
                    var diasProrrata = (crearProductoCliente.CRE_FECHA_DESEMBOLSO - crearProductoCliente.FECHA_SOLICITUD).Days;
                    var fecha = crearProductoCliente.FECHA_SOLICITUD;
                    int diasDelMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);

                    var seguroVidaProrrata = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaSeguroVida * (diasProrrata + 1) / 30, 7);
                    var seguroHogarProrrata = Math.Round((double)ValorSeguroHogar / 30, 7) * (diasProrrata);
                    var valorCredito = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaPeriodicaNMV, 0);
                    var valorInteresCorriente = (valorCredito / diasDelMes) * diasProrrata;

                    CH.Intereses = CH.Intereses + valorInteresCorriente;
                    CH.Seguro_Vida += seguroVidaProrrata;
                    CH.Seguro_Hogar += seguroHogarProrrata;
                    CH.Cuota_Pagadora = Math.Round(CH.Capital + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);
                }

                // ========== ELIMINADO: Ya no restar aquí porque ya fue aplicado al saldoParaCalculo ==========
                // El pago ya está incluido en el saldo inicial

                // Validar si el saldo es negativo (terminar plan antes de tiempo)
                if (CH.Saldo < 0)
                {
                    CH.Capital = (double)saldo;
                    CH.Cuota_Pagadora = Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);
                    CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);

                    if (CH.Saldo == 0)
                    {
                        planillaSimulacion.Add(CH);
                        break;
                    }
                }

                planillaSimulacion.Add(CH);
            }

            // Redondear valores
            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }

            return planillaSimulacionRound;
            #endregion
        }


        private async Task<List<Hipotecario>> ReCalcularPlanTasaVidaHipotecario(InformacionCredito crearProductoCliente, int cuotaCancelada, PlanPagoDto ultimoPlanPago)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();
            var valorViviendaDeseado = Math.Max(
                crearProductoCliente.CRE_VALOR_VIVIENDA,
                (decimal)crearProductoCliente.CRE_VALOR_AVALUO_COMERCIAL);
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)(tasaCredito)), 1.0 / 12.0) - 1;
            var tasaSeguroVida = Math.Round(crearProductoCliente.CRE_TASA_SEGURO_VIDA,7) / 100;
            var tasaSeguroHogar = Math.Round((decimal)crearProductoCliente.CRE_TASA_SEGURO_HOGAR,7) / 100;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL - (cuotaCancelada - 1);



            var tasaMixta = ((decimal)crearProductoCliente.CRE_TASA_SEGURO_VIDA + (decimal)tasaPeriodicaNMV * 100) / 100;

            #region Paso4
            //Calculo el factor (tasaMixta*((1+Tasamixta)^plazo))/((1+tasaMixta)^plazo)-1
            var firstP = tasaMixta * (decimal)Math.Pow((1 + (double)tasaMixta), plazo);
            var secondP = (decimal)(Math.Pow(1 + (double)tasaMixta, plazo));
            var factor = firstP / (secondP - 1);
            decimal valorPrestamo = crearProductoCliente.CRE_VALOR_DESEMBOLSO;
            var ValorSeguroHogar = Math.Round(valorViviendaDeseado * (tasaSeguroHogar), 7);
            // var cuota1 = Math.Round((valorPrestamo * factor + ValorSeguroHogar), 0);
            var cuota = Math.Ceiling(valorPrestamo * factor + ValorSeguroHogar);
            var baseCuota = CalcularPago((decimal)tasaMixta, plazo, ultimoPlanPago.Saldo);
            if (baseCuota == 0)
            {
                baseCuota = CalcularPago((decimal)tasaMixta, plazo, ultimoPlanPago.Capital);
            }
            var obtnerPagosExtraOrdinarios = await _carteraRepository.ObtenerAplicacionPago(crearProductoCliente.CRE_ID);
            var pagoFechaActual = obtnerPagosExtraOrdinarios.Where(x => x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN"
            || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA").FirstOrDefault();

            #region Simulacion
            for (int i = cuotaCancelada; i <= crearProductoCliente.CRE_PLAZO_TOTAL; i++)
            {
                var fechaCorte = new DateTime();
                if (i == cuotaCancelada)
                {
                    fechaCorte = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                       .AddMonths(2).AddDays(-1);
                }
                else
                {
                    fechaCorte = new DateTime(planillaSimulacion.LastOrDefault().Fecha_Pago.Year, planillaSimulacion.LastOrDefault().Fecha_Pago.Month, 1)
                       .AddMonths(2).AddDays(-1);
                }
                Hipotecario CH = new Hipotecario();
                var saldo = planillaSimulacion.Count == 0
                   ? ultimoPlanPago.Saldo :
                   (decimal)planillaSimulacion.LastOrDefault().Saldo;

                CH.Fecha_Corte = fechaCorte;
                CH.Fecha_Pago = fechaCorte;
                CH.Intereses = (double)Math.Round(saldo * (decimal)tasaPeriodicaNMV, 0, MidpointRounding.AwayFromZero);
                CH.Seguro_Vida = Math.Round((double)saldo * (double)tasaSeguroVida, 0);
                CH.Cuota = i;
                CH.Seguro_Hogar = (double)ValorSeguroHogar;
                CH.Cuota_Pagadora = i == crearProductoCliente.CRE_PLAZO_TOTAL ?
                    Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                    : (double)cuota;
                CH.Capital = Math.Round(CH.Cuota_Pagadora - CH.Intereses - CH.Seguro_Vida - CH.Seguro_Hogar, 0);
                CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);
                // Aplicar prorrata en cuota 1
                if (i == 1)
                {
                    var diasProrrata =(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Date- crearProductoCliente.FECHA_SOLICITUD.Date).Days;

                    var fecha = crearProductoCliente.FECHA_SOLICITUD;

                    int diasDelMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);

                    DateTime ultimoDia = new DateTime(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Year, crearProductoCliente.CRE_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Year, crearProductoCliente.CRE_FECHA_DESEMBOLSO.Month));

                    var seguroVidaProrrata = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaSeguroVida * (diasProrrata) / 30, 0);

                    var seguroHogarProrrata = Math.Round(((decimal)ValorSeguroHogar / 30) * (diasProrrata), 0);

                    decimal tasaMensual = (decimal)Math.Pow((double)(1 + tasaCredito), 1.0 / 12.0) - 1m;
                    decimal tasaMensualTrunc = Math.Truncate(tasaMensual * 10_000_000m) / 10_000_000m;
                    decimal interesMensual = Math.Truncate(crearProductoCliente.CRE_VALOR_DESEMBOLSO * tasaMensualTrunc);
                    decimal interesDiario = interesMensual / 30m;
                    decimal interesProrrata = interesDiario * diasProrrata;
                    decimal interesFinal = Math.Round(interesProrrata, 0, MidpointRounding.AwayFromZero);



                    decimal tasaDiaria = (decimal)Math.Pow((double)(1 + tasaCredito), 1.0 / 360.0) - 1m;
                    decimal interesDiarioV = crearProductoCliente.CRE_VALOR_DESEMBOLSO * tasaDiaria;
                    decimal interesProrrataV = interesDiarioV * diasProrrata;
                    decimal interesFinalV = Math.Round(interesProrrataV, 0, MidpointRounding.AwayFromZero);



                    var valorCredito = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaPeriodicaNMV, 0);
                    var valorInteresCorriente = (valorCredito / diasDelMes) * diasProrrata;
                    var valorInteres = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaPeriodicaNMV * (diasProrrata / 31), 0);
                    var Intereses = CH.Intereses + valorInteresCorriente;
                    var Seguro_Vida =   CH.Seguro_Vida + seguroVidaProrrata;
                    var Seguro_Hogar =  CH.Seguro_Hogar + (double)seguroHogarProrrata;
                   var Cuota_Pagadora = Math.Round(CH.Capital + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);

                    CH.Intereses = CH.Intereses + valorInteresCorriente;
                    CH.Seguro_Vida = CH.Seguro_Vida + (double)seguroVidaProrrata;
                    CH.Seguro_Hogar = CH.Seguro_Hogar + (double)seguroHogarProrrata;
                    CH.Cuota_Pagadora = Math.Round(CH.Capital + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);
                }

                if (i == cuotaCancelada && pagoFechaActual != null)
                {

                    CH.Saldo -= (double)pagoFechaActual.CPG_VALOR_PAGO;

                }
                if (CH.Saldo < 0)
                {
                    CH.Capital = (double)saldo;
                    CH.Cuota_Pagadora = Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);
                    CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);
                    if (CH.Saldo == 0)
                    {
                        planillaSimulacion.Add(CH);
                        break;
                    }
                }
                planillaSimulacion.Add(CH);
            }

            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }


            return planillaSimulacionRound;
            #endregion
        }

        /// <summary>
        /// Recalcula el plan de pagos hipotecario cuando se amplía el plazo del crédito
        /// </summary>
        /// <param name="crearProductoCliente">Información del crédito</param>
        /// <param name="cuotasAmpliadas">Número de cuotas adicionales otorgadas</param>
        /// <param name="ultimoPlanPago">Último registro del plan de pagos actual</param>
        /// <returns>Lista con el nuevo plan de pagos</returns>
        private async Task<List<Hipotecario>> ReCalcularPlanHipotecarioRestructuracion(
            InformacionCredito crearProductoCliente,
            int cuotasAmpliadas,
            PlanPagoDto ultimoPlanPago)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();
            List<Double> SaldoCapital = new();

            // ========== TASAS ==========
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)tasaCredito), 1.0 / 12.0) - 1;
            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = (decimal)crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var tasaMixta = tasaPeriodicaNMV + tasaSeguroVida;

            // ========== CÁLCULO DE PLAZO Y NUMERACIÓN ==========
            // Si el crédito estaba en cuota 72 de 72, y amplían 15:
            // - Plazo nuevo total: 72 + 15 = 87
            // - Cuotas a generar: 73-87
            // - Plazo restante para cálculo: 15 cuotas
            int cuotaActual = ultimoPlanPago.Cuota; // Ejemplo: 72
            int cuotaInicio = cuotaActual + 1; // Primera cuota nueva: 73
            int plazoNuevoTotal = crearProductoCliente.CRE_PLAZO_TOTAL + cuotasAmpliadas; // 87
            int plazoParaCalculo = cuotasAmpliadas; // 15 cuotas para amortizar el saldo

            // ========== SALDO Y CUOTA ==========
            decimal saldoActual = crearProductoCliente.CRE_SALDO_CAPITAL;
            var baseCuota = 0m;
            var cuotaMensual = 0d;
            if (crearProductoCliente.TCR_ID == 1)
            {
                baseCuota =  (decimal)Math.Round(await CalcularPago(tasaPeriodicaNMV, plazoParaCalculo, -(double)saldoActual, 0, 0), 0);
            }
            else
            {
                baseCuota = CalcularPago((decimal)tasaMixta, plazoParaCalculo, saldoActual);
            }

            // Seguro hogar sobre el valor mayor entre vivienda y avalúo
            var valorVivienda = Math.Max(
                crearProductoCliente.CRE_VALOR_VIVIENDA,
                (decimal)crearProductoCliente.CRE_VALOR_AVALUO_COMERCIAL);
            var valorSeguroHogar = Math.Round(valorVivienda * tasaSeguroHogar, 0);
            var valorSeguroVida = Math.Round((double)saldoActual * tasaSeguroVida, 0);

            // Cuota total
            if (crearProductoCliente.TCR_ID == 1)
            {
                cuotaMensual = (double)baseCuota + (double)valorSeguroHogar + valorSeguroVida;
            }
            else
            {
                 cuotaMensual = (double)baseCuota + (double)valorSeguroHogar;
            }
           

            // ========== PAGOS EXTRAORDINARIOS ==========
            var pagoExtraordinario = await _carteraRepository.ObtenerAplicacionPago(crearProductoCliente.CRE_ID);
            var pagoActivo = pagoExtraordinario
                .Where(x => x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN"
                         || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA")
                .FirstOrDefault();

            // ========== GENERAR PLAN DE PAGOS ==========
            for (int i = cuotaInicio; i <= plazoNuevoTotal; i++)
            {
                Hipotecario CH = new Hipotecario();

                // ===== FECHA =====
                DateTime fechaCorte;
                if (i == cuotaInicio)
                {
                    // Primera cuota nueva: 2 meses después de la última cuota
                    fechaCorte = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                        .AddMonths(2)
                        .AddDays(-1);
                }
                else
                {
                    // Siguientes cuotas: 2 meses después de la anterior
                    var ultimaCuota = planillaSimulacion.LastOrDefault();
                    fechaCorte = new DateTime(ultimaCuota.Fecha_Pago.Year, ultimaCuota.Fecha_Pago.Month, 1)
                        .AddMonths(2)
                        .AddDays(-1);
                }

                // ===== SALDO =====
                var saldo = planillaSimulacion.Count == 0
                    ? saldoActual // Primera cuota: saldo actual del crédito
                    : (decimal)SaldoCapital.LastOrDefault(); // Siguientes: saldo anterior

                // ===== CÁLCULOS =====
                CH.Fecha_Corte = fechaCorte;
                CH.Fecha_Pago = fechaCorte;
                CH.Cuota = i;
                CH.Intereses = (double)Math.Round(saldo * (decimal)tasaPeriodicaNMV, 0, MidpointRounding.AwayFromZero);
                CH.Seguro_Vida = valorSeguroVida;
                CH.Seguro_Hogar = (double)valorSeguroHogar;

                CH.Cuota_Pagadora = i == plazoNuevoTotal
                    ? Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                    : (double)cuotaMensual;

                CH.Capital = Math.Round(CH.Cuota_Pagadora - CH.Intereses - CH.Seguro_Vida - CH.Seguro_Hogar, 0);
                CH.Saldo = planillaSimulacion.Count == 0 ? (double)saldoActual
                    :(double) saldo;

                SaldoCapital.Add(CH.Saldo - CH.Capital);
                // ===== APLICAR PAGO EXTRAORDINARIO (si existe) =====
                // Solo en la primera cuota nueva si hay un pago extraordinario pendiente
                if (i == cuotaInicio && pagoActivo != null)
                {
                    CH.Saldo -= (double)pagoActivo.CPG_VALOR_PAGO;

                    // Si el pago es para disminuir cuota, recalcular desde aquí
                    if (pagoActivo.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA")
                    {
                        // Recalcular cuota con el nuevo saldo
                        decimal nuevoSaldo = (decimal)CH.Saldo;
                        int cuotasRestantes = plazoNuevoTotal - i;
                        baseCuota = CalcularPago((decimal)tasaMixta, cuotasRestantes, nuevoSaldo);
                        cuotaMensual = (double)baseCuota + (double)valorSeguroHogar;
                    }
                }

                // ===== VALIDAR SALDO NEGATIVO =====
                if (CH.Saldo < 0)
                {
                    // Ajustar última cuota para cerrar el crédito
                    CH.Capital = (double)saldo;
                    CH.Cuota_Pagadora = Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);
                    CH.Saldo = 0;
                    planillaSimulacion.Add(CH);
                    break;
                }

                planillaSimulacion.Add(CH);
            }

            // ========== REDONDEO FINAL ==========
            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }

            return planillaSimulacionRound;
        }
        /// <summary>
        /// Actualiza el plan de pagos Leasing cuando cambia la tasa o valor del seguro hogar
        /// </summary>
        /// <param name="crearProductoCliente">Información del crédito</param>
        /// <param name="cuotaCancelada">Número de la cuota desde donde actualizar</param>
        /// <param name="ultimoPlanPago">Último plan de pago registrado</param>
        /// <param name="vrCreditoInicial">Monto del crédito original (VrCredito = ValorVivienda - CanonInicial)</param>
        /// <param name="interesRemanenteTotal">Suma total de InteresRemanente de todo el plan</param>
        /// <returns>Plan de pagos actualizado desde la cuota cancelada en adelante</returns>
        private async Task<List<Leasing>> UpdatePlanTasaHogarLeasing(
            InformacionCredito crearProductoCliente,
            int cuotaCancelada,
            PlanPagoDto ultimoPlanPago,
            decimal vrCreditoInicial,
            double interesRemanenteTotal)
        {
            List<Leasing> planillaSimulacion = new();
            List<Leasing> planillaSimulacionRound = new();

            // ========== TASAS Y PARÁMETROS ==========
            var tasaCredito = (decimal)crearProductoCliente.CRE_TASA_CREDITO_EA / 100m;
            var tasaPeriodicaNMV = Math.Pow(1 + (double)tasaCredito, 1.0 / 12.0) - 1;
            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;

            // ========== CÁLCULO DE SEGURO HOGAR ==========
            // Usar el mayor entre valor vivienda y valor para seguro hogar
            var valorVivienda = Math.Max(
                crearProductoCliente.CRE_VALOR_VIVIENDA,
                   crearProductoCliente.CRE_VALOR_VIVIENDA
            );

            // Seguro hogar mensual
            var valorSeguroHogar = Math.Round(valorVivienda * (decimal)tasaSeguroHogar, 0);

            // ========== SEGURO DE VIDA ==========
            // En Leasing, el seguro de vida se calcula sobre: VrCredito + suma de InteresRemanente
            var valorSeguroVida = ultimoPlanPago.SeguroVida;

            // ========== SALDO Y PLAZO ==========
            // Usar saldo reducido si está disponible, sino el saldo normal
            decimal saldoInicial = ultimoPlanPago.SaldoReducido > 0
                ? ultimoPlanPago.SaldoReducido
                : ultimoPlanPago.Saldo;

            int plazoRestante = crearProductoCliente.CRE_PLAZO_TOTAL - (cuotaCancelada - 1);

            // ========== CALCULAR NUEVA CUOTA BASE ==========
            // Tasa mixta (interés + seguro vida)
            var tasaMixta = (decimal)tasaPeriodicaNMV + (decimal)tasaSeguroVida;

            // Cuota sin seguro hogar
            var baseCuota = CalcularPago(tasaMixta, plazoRestante, saldoInicial);

            // Si es cero, intentar con capital reducido
            if (baseCuota == 0 && ultimoPlanPago.CapitalReducido > 0)
            {
                baseCuota = CalcularPago(tasaMixta, plazoRestante, ultimoPlanPago.CapitalReducido);
            }

            // Cuota total (incluye seguro hogar + ajuste de redondeo)
            var cuotaMensual = baseCuota + valorSeguroHogar + 1;

            // ========== GENERAR PLAN ACTUALIZADO ==========
            for (int i = cuotaCancelada; i <= crearProductoCliente.CRE_PLAZO_TOTAL; i++)
            {
                Leasing L = new Leasing();

                // ===== CALCULAR FECHAS CORRECTAMENTE =====
                DateTime fechaCorte;
                if (i == cuotaCancelada)
                {
                    // Primera cuota: 1 mes después del último plan
                    var fechaTemp = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                        .AddMonths(1);
                    fechaCorte = new DateTime(fechaTemp.Year, fechaTemp.Month,
                        DateTime.DaysInMonth(fechaTemp.Year, fechaTemp.Month));
                }
                else
                {
                    // Siguientes cuotas: 1 mes después de la anterior
                    var ultimaCuota = planillaSimulacion.LastOrDefault();
                    var fechaTemp = new DateTime(ultimaCuota.FechaCorte.Year, ultimaCuota.FechaCorte.Month, 1)
                        .AddMonths(1);
                    fechaCorte = new DateTime(fechaTemp.Year, fechaTemp.Month,
                        DateTime.DaysInMonth(fechaTemp.Year, fechaTemp.Month));
                }

                // ===== SALDO =====
                var saldoReducido = planillaSimulacion.Count == 0
                    ? saldoInicial  // Primera cuota: saldo del último plan
                    : (decimal)planillaSimulacion.LastOrDefault().SaldoCapitalReducido;  // Siguientes: saldo anterior

                // ===== CÁLCULOS =====
                L.FechaCorte = fechaCorte;
                L.FechaPago = fechaCorte;
                L.NroCuota = i;

                // Intereses reducidos sobre saldo reducido
                L.InteresesCorrientesAPagar = Math.Round((double)saldoReducido * tasaPeriodicaNMV, 0);

                // Seguro vida: CONSTANTE en todo el plan (sobre VrCredito + InteresRemanente total)
                L.SegVida = (double)valorSeguroVida;

                // Seguro hogar: NUEVO VALOR actualizado
                L.SegHogar = (double)valorSeguroHogar;

                // Cuota unidad pagadora (última cuota liquida todo)
                if (i == crearProductoCliente.CRE_PLAZO_TOTAL)
                {
                    L.CapitalAPagar = (double)saldoReducido;
                    L.CuotaUnidadPagadora = Math.Round(L.CapitalAPagar + L.InteresesCorrientesAPagar + L.SegVida + L.SegHogar, 0);
                }
                else
                {
                    L.CuotaUnidadPagadora = (double)cuotaMensual;
                    L.CapitalAPagar = Math.Round(L.CuotaUnidadPagadora - L.InteresesCorrientesAPagar - L.SegVida - L.SegHogar, 0);
                }

                // Nuevo saldo reducido
                L.SaldoCapitalReducido = Math.Round((double)saldoReducido - L.CapitalAPagar, 0);

                // ===== VALIDAR SALDO NEGATIVO =====
                if (L.SaldoCapitalReducido < 0)
                {
                    // Ajustar para cerrar el crédito
                    L.CapitalAPagar = (double)saldoReducido;
                    L.CuotaUnidadPagadora = Math.Round(L.CapitalAPagar + L.InteresesCorrientesAPagar + L.SegVida + L.SegHogar, 0);
                    L.SaldoCapitalReducido = 0;
                    planillaSimulacion.Add(L);
                    break;
                }

                planillaSimulacion.Add(L);
            }

            // ========== REDONDEO FINAL ==========
            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Leasing
                {
                    FechaCorte = item.FechaCorte,
                    FechaPago = item.FechaPago,
                    NroCuota = item.NroCuota,
                    CapitalAPagar = Math.Round(item.CapitalAPagar, 0),
                    InteresesCorrientesAPagar = Math.Round(item.InteresesCorrientesAPagar, 0),
                    SegHogar = Math.Round(item.SegHogar, 0),
                    SegVida = Math.Round(item.SegVida, 0),
                    CuotaUnidadPagadora = Math.Round(item.CuotaUnidadPagadora, 0),
                    SaldoCapitalReducido = Math.Round(item.SaldoCapitalReducido, 0),
                    // Mantener otras propiedades para compatibilidad
                    Saldo = item.Saldo,
                    Capital = item.Capital,
                    IntCte = item.IntCte,
                    Cuota = item.Cuota
                });
            }

            return planillaSimulacionRound;
        }
        private async Task<List<Hipotecario>> UpdatePlanHipotecarioTasaHogar(InformacionCredito crearProductoCliente, int cuotaCancelada, PlanPagoDto ultimoPlanPago)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();
            var tasaCredito = (decimal)crearProductoCliente.CRE_TASA_CREDITO_EA / 100m;
            double tasaPeriodicaNMVDouble = Math.Pow(1 + (double)tasaCredito, 1.0 / 12.0) - 1;

            decimal tasaPeriodicaNMV = Math.Round((decimal)(tasaPeriodicaNMVDouble * 100), 10, MidpointRounding.AwayFromZero);


            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = (decimal)crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var tasaPeriodica = tasaPeriodicaNMV / 100m;
            var tasaMixta = tasaPeriodica + (decimal)tasaSeguroVida;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL - (cuotaCancelada - 1);

            var baseCuota = CalcularPago(tasaMixta, plazo, ultimoPlanPago.Saldo);


            var ValorSeguroHogar = Math.Round(crearProductoCliente.CRE_VALOR_VIVIENDA * (tasaSeguroHogar), 0);
            var cuota = cuotaCancelada > 2
                ? ultimoPlanPago.CuotaUnidadPagadora
                : baseCuota + (decimal)ValorSeguroHogar + 1;
            #region Simulacion
            for (int i = cuotaCancelada; i <= crearProductoCliente.CRE_PLAZO_TOTAL; i++)
            {
                var fechaCorte = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                    .AddMonths(2).AddDays(-1);
                int mes = fechaCorte.Month;
                int año = fechaCorte.Year;
                Hipotecario CH = new Hipotecario();
                var saldo = planillaSimulacion.Count == 0
                   ? ultimoPlanPago.Saldo :
                   (decimal)planillaSimulacion.LastOrDefault().Saldo;

                CH.Fecha_Corte = fechaCorte;
                CH.Fecha_Pago = fechaCorte;
                CH.Intereses = (double)Math.Round(saldo * tasaPeriodica, 0, MidpointRounding.AwayFromZero);
                CH.Seguro_Vida = Math.Round((double)saldo * (double)tasaSeguroVida, 0);
                CH.Cuota = i;
                CH.Seguro_Hogar = (double)ValorSeguroHogar;
                CH.Cuota_Pagadora = i == crearProductoCliente.CRE_PLAZO_TOTAL ?
                    Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                    : (double)cuota;
                CH.Capital = Math.Round(CH.Cuota_Pagadora - CH.Intereses - CH.Seguro_Vida - CH.Seguro_Hogar, 0);
                CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);

                planillaSimulacion.Add(CH);
            }

            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }


            return planillaSimulacionRound;
            #endregion
        }
        private async Task<List<Hipotecario>> NewPlanHipotecario(InformacionCredito crearProductoCliente, int cuotaCancelada, PlanPagoDto ultimoPlanPago)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();
            var tasaCredito = (decimal)crearProductoCliente.CRE_TASA_CREDITO_EA / 100m;
            double tasaPeriodicaNMVDouble = Math.Pow(1 + (double)tasaCredito, 1.0 / 12.0) - 1;

            decimal tasaPeriodicaNMV = Math.Round((decimal)(tasaPeriodicaNMVDouble * 100), 10, MidpointRounding.AwayFromZero);


            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaPeriodica = tasaPeriodicaNMV / 100m;
            var tasaMixta = tasaPeriodica + (decimal)tasaSeguroVida;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL - (cuotaCancelada - 1);

            var baseCuota = CalcularPago(tasaMixta, plazo, ultimoPlanPago.Saldo);

            var ValorSeguroHogar = ultimoPlanPago.SeguroHogar;
            var cuota = baseCuota + ValorSeguroHogar + 1;
            #region Simulacion
            for (int i = cuotaCancelada; i <= crearProductoCliente.CRE_PLAZO_TOTAL; i++)
            {
                var fechaCorte = new DateTime(ultimoPlanPago.Fecha.Year, ultimoPlanPago.Fecha.Month, 1)
                    .AddMonths(2).AddDays(-1);
                int mes = fechaCorte.Month;
                int año = fechaCorte.Year;
                Hipotecario CH = new Hipotecario();
                var saldo = planillaSimulacion.Count == 0
                   ? ultimoPlanPago.Saldo :
                   (decimal)planillaSimulacion.LastOrDefault().Saldo;

                CH.Fecha_Corte = fechaCorte;
                CH.Fecha_Pago = fechaCorte;
                CH.Intereses = (double)Math.Round(saldo * tasaPeriodica, 0, MidpointRounding.AwayFromZero);
                CH.Seguro_Vida = Math.Round((double)saldo * (double)tasaSeguroVida, 0);
                CH.Cuota = i;
                CH.Seguro_Hogar = (double)ValorSeguroHogar;
                CH.Cuota_Pagadora = i == crearProductoCliente.CRE_PLAZO_TOTAL ?
                    Math.Round((double)saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                    : (double)cuota;
                CH.Capital = Math.Round(CH.Cuota_Pagadora - CH.Intereses - CH.Seguro_Vida - CH.Seguro_Hogar, 0);
                CH.Saldo = Math.Round((double)saldo - CH.Capital, 0);

                planillaSimulacion.Add(CH);
            }

            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }


            return planillaSimulacionRound;
            #endregion
        }
        private async Task<FileResult> GeneracionInformeRestructuracion(PagoAplicarDto request, string nombreCliente)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            try
            {
                ObtenerSolicitud obtenerSolicitud = new ObtenerSolicitud();
                //var response2 = await _clientsRepository.ObtenerCuentasMovimientosAfiliado(data.identificacion);                //var cuentas = this._mapper.Map<List<CuentaDto>>(await _clientsRepository.ObtenerCuentas(Convert.ToInt32(data.identificacion)));


                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\InformeRestructuracionDeuda.rdlc";
                report.ReportPath = reportPath;
                //var dataUser = await _usersRepository.ObtenerUsuarioPorId(Guid.Parse(userid));
                //var clienteDetallado = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(data.TipoIdentificacion, data.Identificacion);
                report = await ParametrosReporteRestructuracionDeuda(report, request, nombreCliente);
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }
            byte[] bytes = null;
            try
            {
                bytes = report.Render("PDF");
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }
            return await GenerarReporte(bytes, false);
        }
        private async Task<FileResult> GenerarReporte(byte[] byteArray, bool requiredPass, string identificacion = "")
        {

            // Asigno valor secreto de accesibilidad al pdf si el parametro es true
            if (requiredPass)
            {
                using MemoryStream input = new(byteArray);
                using MemoryStream output = new();
                string credential = identificacion;
                PdfReader reader = new PdfReader(input);
                PdfEncryptor.Encrypt(reader, output, true, credential, credential, PdfWriter.ALLOW_SCREENREADERS);
                byteArray = output.ToArray();
            }

            // Asigno nombre aleatorio al archivo 
            string hash = Guid.NewGuid().ToString();
            string namePDF = hash + ".pdf";

            // Valido que si el container no existe se crea

            string carpeta = Path.Combine(_config.CarpetaServidorStorage, "reportes-sfc");
            //    BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "reportes-sfc");
            //await container.CreateIfNotExistsAsync();

            //// Conexion al storage
            //var cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.CadenaOne);
            //var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            //var cloudBlobContainer = cloudBlobClient.GetContainerReference("reportes-sfc");
            //var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(namePDF);
            //cloudBlockBlob.Properties.ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf;

            //// Subo el archivo al container
            //using (Stream stream = new MemoryStream(byteArray))
            //{
            //    await cloudBlockBlob.UploadFromStreamAsync(stream);
            //}

            //var fileName = cloudBlockBlob.Uri.ToString().Split("/");

            var fileName = namePDF;

            // Se asigna el nombre con el que quedo guardado el archivo en el storage
            MemoryStream msPdf = new(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = fileName
            };

            await archivoPdf.FileStream.FlushAsync();

            return archivoPdf;

        }
        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosReporteRestructuracionDeuda(Microsoft.Reporting.NETCore.LocalReport reportParam, PagoAplicarDto request, string name)
        {

            var reportParameters = ParametrosStaticosPlanillaPagos(request, name).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();
            reportParam.SetParameters(reportParameters);
            return reportParam;
        }


        public async Task<Dictionary<string, string>> ParametrosStaticosPlanillaPagos(PagoAplicarDto request, string name)
        {
            return new Dictionary<string, string>
                {
                    { "FecPlanilla", DateTime.Now.ToString()},
                    { "NroPlanilla",request.NumeroCredito.ToString()},
                    { "FecElaboracion", DateTime.Now.ToString()},
                    { "PrimerNombreApellido",name},
                    { "NroIdentificacion",request.Identificacion},
                    { "Cuotas",request.nCuota.ToString()},
                    { "Valor",request.ValorPago.ToString("C")},
            };
        }

        private async Task<int> CrearNovedadRediferido(RequestNovedadRediferido request)
        {
            var result = await _carteraRepository.CrearNovedadRediferido(request);
            return result;
        }
        public async Task<Response<NovedadCarteraDto>> ActualizarNovedadCartera(NovedadCarteraDto novedad)
        {
            NovedadCartera NovedadCartera = this._mapper.Map<NovedadCartera>(novedad);
            return new Response<NovedadCarteraDto> { Data = this._mapper.Map<NovedadCarteraDto>(await this._carteraRepository.ActualizarNovedadCartera(NovedadCartera)) };
        }

        public async Task<Response<IEnumerable<NovedadCarteraDto>>> ObtenerNovedadesCartera(Guid IdCredito)
        {

            var result = await this._carteraRepository.ObtenerNovedadesCartera(IdCredito);
            foreach (var item in result)
            {
                if (item.TIPO_NOVEDAD == "RESTITUCIÓN INMUEBLE")
                {
                    item.VALOR_PAGADO = item.NVC_TOTAL_DEUDA;
                    item.NVC_TOTAL_DEUDA = 0;
                }
            }
            return new Response<IEnumerable<NovedadCarteraDto>> { Data = this._mapper.Map<IEnumerable<NovedadCarteraDto>>(result) };
        }

        public async Task<Response<IEnumerable<TiposNovedadesCarteraDto>>> ObtenerTiposNovedadesCartera()
        {
            return new Response<IEnumerable<TiposNovedadesCarteraDto>> { Data = this._mapper.Map<IEnumerable<TiposNovedadesCarteraDto>>(await this._carteraRepository.ObtenerTiposNovedadesCartera()) };
        }

        public async Task<Response<IEnumerable<TiposCausalesAceleracionCarteraDto>>> ObtenerTiposCausalesAceleracionCartera()
        {
            return new Response<IEnumerable<TiposCausalesAceleracionCarteraDto>>
            {
                Data = _mapper.Map<IEnumerable<TiposCausalesAceleracionCarteraDto>>(
                    await _carteraRepository.ObtenerTiposCausalesAceleracionCartera())
            };
        }

        public async Task<Response<SaldoObligacionDto>> ObtenerSaldoObligacion(Guid IdCredito)
        {
            return new Response<SaldoObligacionDto>
            {
                Data = _mapper.Map<SaldoObligacionDto>(
                    await _carteraRepository.ObtenerSaldoObligacion(IdCredito))
            };
        }

        public async Task<Response<AplicacionPagoDto>> AplicarPagoNovedadCartera(RequestPagoNovedadDto request)
        {

            var novedad = _mapper.Map<NovedadCarteraDto>(await _carteraRepository.ObtenerNovedadCartera(request.IdNovedad));

            if (novedad == null)
                return "No se encontro la noveda".BuildError<AplicacionPagoDto>();

            if (novedad.CodigoEstadoNovedad != "VIGENTE")
                return $"No se puede aplicar el pago a la novedad se encuentra en estado: {novedad.EstadoNovedad}"
                            .BuildError<AplicacionPagoDto>();

            if (request.ValorPago <= 0)
                return "El valor del pago es inválido".BuildError<AplicacionPagoDto>();

            if (request.ValorPago > novedad.ValorRestante)
            {
                var message = "El valor del pago no puede ser superior al valor restante " +
                     $"({novedad.ValorRestante.ToString("C2", CultureInfo.GetCultureInfo("en-US"))}) por pagar de la novedad";

                return message.BuildError<AplicacionPagoDto>();
            }


            var response = _mapper.Map<AplicacionPagoDto>(await _carteraRepository.AplicarPagoNovedadCartera(request, novedad.Capital, novedad.InteresRemanente));
            var infoCredito = await _carteraRepository.ObtenerCreditoCarteraById(response.IdCredito);
            if (novedad.IdTipoNovedad == 1 && request.AplicarPago)
            {
                await CrearCuotaCartera(new RequestCarteraSeven
                {
                    NumeroDocumentoCliente = infoCredito.CLI_IDENTIFICACION,
                    tipoIdentificacion = infoCredito.TIPO_IDENTIFICACION,
                    AplicaSimulacion = false,
                    numeroCredito = infoCredito.CRE_NRO_CREDITO,
                    cpgId = response.IdAplicacionPago,
                    tipoCredito = infoCredito.TCR_ID,
                    nCuota = 1,
                    codigo = "ACELERACION",
                    FechaPago = novedad.FechaFinalizacion
                }, default);
            }
            return new Response<AplicacionPagoDto>
            {
                Data = response
            };
        }

        public async Task<Response<IEnumerable<EstadosNovedadCarteraDto>>> ObtenerEstadosNovedadCartera()
        {
            return new Response<IEnumerable<EstadosNovedadCarteraDto>> { Data = this._mapper.Map<IEnumerable<EstadosNovedadCarteraDto>>(await this._carteraRepository.ObtenerEstadosNovedadCartera()) };
        }

        public async Task<Response<IEnumerable<AlternativosAplicacionPagoDto>>> ObtenerAlternativosAplicacionPago()
        {
            return new Response<IEnumerable<AlternativosAplicacionPagoDto>> { Data = this._mapper.Map<IEnumerable<AlternativosAplicacionPagoDto>>(await this._carteraRepository.ObtenerAlternativosAplicacionPago()) };
        }

        public async Task<Response<ValidarAplicacionPagoDto>> ValidarAplicacionPago(ObtenerProductoCartera request)
        {
            InformacionCreditoDto credito = (await this.ObtenerCreditoCartera(request)).Data;
            ValidarAplicacionPagoDto response = new();

            response.AcumuladosAlDia = (credito.AcumuladoInteresCorriente + credito.AcumuladoInteresMora +
                    credito.AcumuladoSeguroHogar + credito.AcumuladoSeguroVida);

            if (credito.SaldoCapitalMora > 0)
            {
                //var diasMora = (int)(credito.FechaProximoPago - credito.FechaUltimoPago).TotalDays;
                //var interesMora = (float)credito.SaldoCapitalMora * diasMora * credito.TasaCreditoPer * 1.5;
                //var valor_pago = (decimal)((float)credito.ValorCuota + (float)credito.SaldoCapitalMora + (float)credito.AcumuladoInteresMora);

                //response.ValorPago = valor_pago;
                //response.Mora = true;
                response.ValorPagoCuota = credito.ValorCuota;
                response.ValorPago = credito.IdTipoCredito == 1 ? credito.AcumuladoCapital + credito.ValorDeudaRemanente : credito.SaldoCapital;
                response.Mora = false;
                return new Response<ValidarAplicacionPagoDto> { Data = response };

            }
            else
            {
                response.ValorPagoCuota = credito.ValorCuota;
                response.ValorPago = credito.IdTipoCredito == 1 ? credito.AcumuladoCapital + credito.ValorDeudaRemanente : credito.SaldoCapital;
                response.Mora = false;
                return new Response<ValidarAplicacionPagoDto> { Data = response };
            }
        }
        public async Task<Response<ResponsePlanPagoDto>> ObtenerPlanPagosCredito(ObtenerProductoCartera obtenerProductoCartera)
        {
            ResponsePlanPagoDto response = new ResponsePlanPagoDto();
            InformacionCreditoDto credito = (await this.ObtenerCreditoCartera(obtenerProductoCartera)).Data;
            IEnumerable<PlanPagoDto> planPagoDtos = this._mapper.Map<IEnumerable<PlanPagoDto>>(await this._carteraRepository.ObtenerPlanPagosCredito(obtenerProductoCartera));
            var validateNovedades = await _carteraRepository.ObtenerNovedadesCartera(credito.IdCredito);
            var validateRestructuracion = validateNovedades?.Where(x => x.TNV_ID == 3).FirstOrDefault();

            int index = 0;
            decimal saldoAnterior = 0;
            foreach (var item in planPagoDtos)
            {
                if (index == 0)
                {
                    item.SaldoCapital = obtenerProductoCartera.Tcr_Id == 1 && (item.CapitalPorPagar > 0 || credito.IdLineaCredito == 3 || credito.IdLineaCredito == 4)
                        ? credito.ValorDesembolso - item.CapitalReducido
                        : validateRestructuracion is not null ? item.SaldoInicial - item.CapitalReducido : credito.ValorDesembolso - item.Capital;
                }
                else
                {

                    if (obtenerProductoCartera.Tcr_Id == 1 && (item.CapitalPorPagar > 0 || credito.IdLineaCredito == 3 || credito.IdLineaCredito == 4))
                    {
                        item.SaldoCapital = saldoAnterior - item.CapitalReducido;
                        if (item.DeudaRemanente == 0 && validateRestructuracion != null)
                        {
                            var CuotaRestructurada = planPagoDtos.Where(x => x.CapitalPorPagar == 0 && x.InteresPorPagar == 0)
                                .Select(x => x.SaldoInicial).FirstOrDefault();
                            item.SaldoCapital = CuotaRestructurada;
                        }
                    }
                    else if (validateRestructuracion != null)
                    {

                        item.SaldoCapital = item.SaldoInicial - item.CapitalReducido;

                    }
                    else if (obtenerProductoCartera.Tcr_Id == 1)
                    {
                        item.SaldoCapital = saldoAnterior - item.CapitalReducido;
                    }
                    else
                    {
                        item.SaldoCapital = saldoAnterior - item.Capital;
                    }
                }
                index++;
                saldoAnterior = item.SaldoCapital;
            }

            // ═══ OBTENER MOVIMIENTOS DE PAGO UNA SOLA VEZ ═══
            var movimientosPago = await _carteraRepository.ObtenerAplicacionPago(credito.IdCredito);

            // ═══ CORRECCIÓN 1: RESTAR ABONOS DIRECTOS AL CAPITAL (DISMINUIR CUOTA O PLAZO) ═══
            // Esto es CRÍTICO: los abonos que disminuyen el valor de la cuota o plazo
            // deben restar del SaldoCapital de TODAS las cuotas posteriores

            decimal abonosTotalCapital = 0;
            if (credito.IdTipoCredito == 1)
            {
                foreach (var item in planPagoDtos)
                {
                    // Obtener abonos de esta cuota con modalidades DISMINUIR
                    var abonosDirectos = movimientosPago
                        .Where(x => x.CPG_FECHA_APLICACION <= item.Fecha
                        && x.CPG_VALOR_PAGO > 0
                        && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2)
                        && (x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN"
                            || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA"))
                        .ToList();

                    if (abonosDirectos.Any())
                    {
                        abonosTotalCapital = abonosDirectos.Sum(x => x.CPG_ABONO_CAPITAL);

                        // RESTAR EL ABONO AL SALDO CAPITAL
                        item.SaldoCapital = item.SaldoCapital - abonosTotalCapital;
                        if (credito.IdTipoCredito == 2)
                        {

                            item.Saldo = item.SaldoCapital;
                        }
                        // Asegurar que no sea negativo
                        if (item.SaldoCapital < 0)
                        {
                            item.SaldoCapital = 0;
                        }
                    }
                }
            }
            
            


            // ═══ VALIDACIÓN DE CUOTA(S) "PAGADA" SEGÚN ESTADO DEL CRÉDITO ═══
            // Al día (ESC_ID = 1): se valida solo la última cuota pagada.
            // En mora (ESC_ID = 2): se validan las últimas 2 cuotas pagadas.
            int cantidadCuotasAValidar = credito.IdEstadoCartera == 2 ? 2 : 1;

            var cuotasPagadas = planPagoDtos
                .Where(x => x.EstadoCuota == "PAGADA")
                .OrderByDescending(x => x.Fecha)
                .Take(cantidadCuotasAValidar)
                .ToList();

            var cuotasPagadasAll = planPagoDtos
                .Where(x => x.EstadoCuota == "PAGADA")
                .OrderByDescending(x => x.Fecha)
                .ToList();

            foreach (var cuota in cuotasPagadas)
            {
                // Obtener pagos de esa cuota específica
                var pagosDelaCuota = movimientosPago
                    .Where(x => x.CPG_FECHA_APLICACION == cuota.Fecha
                    && x.CPG_VALOR_PAGO > 0
                    && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2)
                    && (x.MODALIDAD_PAGO == "PAGAR LA CUOTA DEL CRÉDITO" || x.MODALIDAD_PAGO == "OTROS PAGOS"))
                    .OrderByDescending(x => x.CPG_FECHA_CREACION)
                    .ToList();

                if (pagosDelaCuota.Any())
                {
                    // ═══ FIX: si ese mismo día hubo un abono de disminución de plazo
                    // o de cuota, el plan se recalculó y el "requerido" ya no es
                    // comparable contra lo que se pagó en su momento -- no se valida
                    // esta cuota como parcial, se respeta el estado que ya tenía.
                    bool tuvoAbonoDisminucionElMismoDia = movimientosPago.Any(x =>
                        x.CPG_FECHA_APLICACION == cuota.Fecha
                        && x.CPG_VALOR_PAGO > 0
                        && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2)
                        && (x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN"
                            || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA"));

                    if (!tuvoAbonoDisminucionElMismoDia)
                    {
                        decimal capitalPagado = pagosDelaCuota.Sum(x => x.CPG_ABONO_CAPITAL);
                        decimal interesPagado = pagosDelaCuota.Sum(x => x.CPG_ABONO_INTERES_CORRIENTE);

                        decimal capitalRequerido = obtenerProductoCartera.Tcr_Id == 1
                            ? cuota.CapitalReducido
                            : cuota.Capital;
                        decimal interesRequerido = obtenerProductoCartera.Tcr_Id == 1
                            ? cuota.InteresReducido
                            : cuota.Interes;

                        const decimal holgura = 2.0m;

                        // Si no pagó 100% → cambiar a PAGADA PARCIALMENTE
                        if (capitalPagado < (capitalRequerido - holgura) || interesPagado < (interesRequerido - holgura))
                        {
                            cuota.EstadoCuota = "PAGADA PARCIALMENTE";
                        }
                    }
                }
            }

            foreach (var cuota in cuotasPagadasAll)
            {
                // Obtener pagos de esa cuota específica CON MODALIDAD DISMINUIR
                var pagosDelaCuota = movimientosPago
                    .Where(x => x.CPG_FECHA_APLICACION == cuota.Fecha
                    && x.CPG_VALOR_PAGO > 0
                    && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2)
                    && (x.MODALIDAD_PAGO == "DISMINUIR EL PLAZO DE LA OBLIGACIÓN" || x.MODALIDAD_PAGO == "DISMINUIR EL VALOR DE LA CUOTA"))
                    .ToList();

                if (pagosDelaCuota.Any())
                {
                    // CORRECCIÓN 2: Guardar el abono en AbonosCuota para visualización
                    cuota.AbonosCuota = pagosDelaCuota.Sum(x => x.CPG_VALOR_PAGO);
                }
            }

            if (obtenerProductoCartera.Tcr_Id == 1)
            {
                var capitalRemante = planPagoDtos.Sum(x => x.CapitalPorPagar);
                var interesRemante = planPagoDtos.Sum(x => x.InteresPorPagar);
                var deudaRemanente = capitalRemante + interesRemante;
                var validateDeudaRemanente = planPagoDtos.LastOrDefault().DeudaRemanente;
                if (validateDeudaRemanente > 0)
                {
                    planPagoDtos.LastOrDefault().DeudaRemanente = deudaRemanente;
                }
            }

            response.TipoCredito = credito.CodigoTipoCredito;
            response.PlanPago = (List<PlanPagoDto>)planPagoDtos;

            return new Response<ResponsePlanPagoDto> { Data = response };
        }
        public async Task<Response<ResponsePlanPagoDto>> ObtenerPlanPago(ObtenerProductoCartera obtenerProductoCartera)
        {
            ResponsePlanPagoDto response = new ResponsePlanPagoDto();
            InformacionCreditoDto credito = (await this.ObtenerCreditoCartera(obtenerProductoCartera)).Data;
            IEnumerable<PlanPagoDto> planPagoDtos = this._mapper.Map<IEnumerable<PlanPagoDto>>(await this._carteraRepository.ObtenerPlanPago(obtenerProductoCartera));
            response.TipoCredito = credito.CodigoTipoCredito;
            response.PlanPago = (List<PlanPagoDto>)planPagoDtos;
            return new Response<ResponsePlanPagoDto> { Data = response };
        }

        
        public async Task<Response<ProductoClienteDto>> CrearProductoCredito(ProductoClienteDto productoCliente)
        {

            ProductoCliente producto = this._mapper.Map<ProductoCliente>(productoCliente);
            return new Response<ProductoClienteDto> { Data = this._mapper.Map<ProductoClienteDto>(await this._carteraRepository.CrearProductoCredito(producto)) };

        }
        public async Task<Response<IEnumerable<UltimoCierreDto>>> ConsultarUltimosCierres()
        {
            var ultimosCierres = this._mapper.Map<IEnumerable<UltimoCierreDto>>(await this._carteraRepository.ConsultarUltimosCierres());
            return new Response<IEnumerable<UltimoCierreDto>> { Data = ultimosCierres };
        }

        public async Task<FileResult> GenerarReporteAltasBajasAsync(RequestReporteAltasBajasDto request)
        {
            var listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(await _carteraRepository.ConsultarUnidadEjecutora())
                .Select(x => new { x.idUnidadEjecutora, x.nombreUnidadEjecutora })
                .ToDictionary(x => x.idUnidadEjecutora, y => y.nombreUnidadEjecutora);

            var nombreUnidad = request.IdUnidadEjecutora == 0
                   ? "TODAS_LAS_UNIDADES"
                   : listUnidades[request.IdUnidadEjecutora];


            // Todas las unidades o más de un tipo de reporte generará un zip
            if (request.IdUnidadEjecutora == 0 || request.TipoReporte == TipoReporteUnidades.ALTAS_Y_BAJAS)
            {
                Dictionary<int, string> listUnidadesReporte = request.IdUnidadEjecutora == 0
                    ? listUnidades
                    : new() { { request.IdUnidadEjecutora, nombreUnidad } };


                var zipBytes = await GenerateZipBytesFromReports(request, listUnidadesReporte);
                return GetFileContentResult(zipBytes, "application/zip", GetFilenameZip(request, nombreUnidad));

            }
            else
            {
                var report = new ReporteAltasBajasParametersDto()
                {
                    IdUnidadEjecutora = request.IdUnidadEjecutora,
                    PeriodoGeneracion = request.PeriodoGeneracion,
                    TipoReporte = (int)request.TipoReporte
                };

                var (bytes, filename) = await GenerateReportFile(report, nombreUnidad);
                return GetFileContentResult(bytes, "text/csv", filename);

            }

        }

        private static FileResult GetFileContentResult(byte[] bytes, string contentType, string filename)
        {
            return new FileContentResult(bytes, contentType)
            {
                FileDownloadName = filename
            };
        }

        private async Task<byte[]> GenerateZipBytesFromReports(RequestReporteAltasBajasDto request, IDictionary<int, string> listUnidadesReporte)
        {
            using var memoryStream = new MemoryStream();
            using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var reports = GenerateReportList(request, listUnidadesReporte.Keys);

                foreach (var report in reports)
                {
                    var nombreUnidadReport = listUnidadesReporte[report.IdUnidadEjecutora];
                    var (bytes, filename) = await GenerateReportFile(report, nombreUnidadReport);
                    var entry = zip.CreateEntry(filename);

                    using var zipStream = entry.Open();
                    zipStream.Write(bytes);
                }
            }

            var zipBytes = memoryStream.ToArray();
            return zipBytes;
        }

        private static List<ReporteAltasBajasParametersDto> GenerateReportList(RequestReporteAltasBajasDto request, IEnumerable<int> listUnidadesReporte)
        {
            var reports = new List<ReporteAltasBajasParametersDto>();

            if (request.TipoReporte is TipoReporteUnidades.BAJAS or TipoReporteUnidades.ALTAS_Y_BAJAS)
            {
                reports.AddRange(listUnidadesReporte.Select(idUnidadEjecutora => new ReporteAltasBajasParametersDto()
                {
                    IdUnidadEjecutora = idUnidadEjecutora,
                    PeriodoGeneracion = request.PeriodoGeneracion,
                    TipoReporte = 0
                }));
            }

            if (request.TipoReporte is TipoReporteUnidades.ALTAS or TipoReporteUnidades.ALTAS_Y_BAJAS)
            {
                reports.AddRange(listUnidadesReporte.Select(idUnidadEjecutora => new ReporteAltasBajasParametersDto()
                {
                    IdUnidadEjecutora = idUnidadEjecutora,
                    PeriodoGeneracion = request.PeriodoGeneracion,
                    TipoReporte = 1
                }));
            }

            return reports;
        }

        private async Task<(byte[] bytes, string filename)> GenerateReportFile(ReporteAltasBajasParametersDto request, string nombreUnidad)
        {

            var resultadoAltasBajas = _mapper.Map<IEnumerable<ReporteAltasBajasDto>>
                (
                    await _carteraRepository.GenerarReporteAltasBajas(request.IdUnidadEjecutora, request.TipoReporte, request.PeriodoGeneracion)
                );

            List<ReporteAltasBajasDto> list = new();
            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos
                (resultadoAltasBajas.Select(x => x.Identificacion).Distinct().ToList())).Data;

            var columnas = await _reportesRepository.ConsultarColumnasPorUnidadEjecutora(request.IdUnidadEjecutora, request.TipoReporte);
            var propertiesToShow = columnas.ToDictionary(c => c.NOMBRE_COLUMNA, c => c.NOMBRE_ENCABEZADO);

            var reporteAltasBajas = resultadoAltasBajas.Join(listAfiliados, r => r.Identificacion, a => a.Documento,
                (r, a) =>
                {
                    r.NombreCompleto = a.NombreCompleto;
                    return r;
                }
            ).ToList();

            var nombreArchivo = GetFilenameReport(request, nombreUnidad);
            var bytesArchivoCsv = reporteAltasBajas.GenerateCsvBytes(propertiesToShow, nombreArchivo);

            return (bytesArchivoCsv, nombreArchivo);
        }

        private static string GetFilenameReport(ReporteAltasBajasParametersDto request, string nombreUnidad)
        {
            var tipoNombre = request.TipoReporte == 1 ? "ALTAS" : "BAJAS";
            return $"REPORTE_{tipoNombre}_{request.PeriodoGeneracion}_{Regex.Replace(nombreUnidad.Trim(), @"\s+", "_").RemoveDiacritics()}.csv";
        }

        private static string GetFilenameZip(RequestReporteAltasBajasDto request, string nombreUnidad)
        {
            return $"REPORTES_{request.TipoReporte}_{request.PeriodoGeneracion}_{Regex.Replace(nombreUnidad.Trim(), @"\s+", "_").RemoveDiacritics()}.zip";
        }

        public async Task<Response<ProcesoAplicacionPagosMasivoDto>> InsertarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivoDto procesoAplicacionPagosMasivo)
        {
            var procesoAplicacion = this._mapper.Map<ProcesoAplicacionPagosMasivo>(procesoAplicacionPagosMasivo);
            var response = this._mapper.Map<ProcesoAplicacionPagosMasivoDto>(await _carteraRepository.InsertarProcesoAplicacionPagos(procesoAplicacion));
            return new Response<ProcesoAplicacionPagosMasivoDto> { Data = response };
        }

        public async Task<Response<ProcesoAplicacionPagosMasivoDto>> ActualizarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivoDto procesoAplicacionPagosMasivo)
        {
            var procesoAplicacion = this._mapper.Map<ProcesoAplicacionPagosMasivo>(procesoAplicacionPagosMasivo);
            var response = this._mapper.Map<ProcesoAplicacionPagosMasivoDto>(await _carteraRepository.ActualizarProcesoAplicacionPagos(procesoAplicacion));
            return new Response<ProcesoAplicacionPagosMasivoDto> { Data = response };
        }

        public async Task<Response<bool>> EliminarProcesoAplicacionPagos(int idProceso)
        {
            var response = await _carteraRepository.EliminarProcesoAplicacionPagos(idProceso);
            return new Response<bool> { Data = response };
        }

        private async Task<IEnumerable<ProcesoAplicacionPagosMasivoDto>> ObtenerProcesoAplicacionPagosConDetalle(ParametrosObtenerProcesoAplicacionPagosDto request)
        {

            var response = this._mapper.Map<IEnumerable<ProcesoAplicacionPagosMasivoDto>>(
                await _carteraRepository.ObtenerProcesoAplicacionPagosConDetalle(request)
            );
            var opciones = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.ForEach(proceso =>
            {
                if (!string.IsNullOrWhiteSpace(proceso.JsonPagos))
                {


                    proceso.Pagos = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<PagoListaDetalleDto>>(proceso.JsonPagos, opciones);
                }
            });

            var detalles = response.SelectMany(x => x.Pagos);

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(
                detalles.Select(x => x.NroDocumento).Distinct().ToList())).Data;

            detalles = detalles.GroupJoin(listAfiliados, det => det.NroDocumento, la => la.Documento, (det, la) =>
            {
                det.Funcionario = la.FirstOrDefault()?.NombreCompleto ?? "";
                return det;
            }).ToList();

            return response;
        }

        public async Task<Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>> ObtenerProcesoAplicacionPagos(ParametrosObtenerProcesoAplicacionPagosDto request)
        {
            var response = await ObtenerProcesoAplicacionPagosConDetalle(request);
            return new Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>> { Data = response };
        }

        public async Task<Response<IEnumerable<ConsultaSaldosResponse>>> ConsultaSaldosYCartera(ConsultaSaldosRequest request)
        {
            var requeteReporte = new ReporteGeneralCarteraParametersDto
            {
                Anio = int.Parse(request.Anio),
                Mes = int.Parse(request.Mes),
                TipoCredito = request.TipoCredito == 0 ? null : request.TipoCredito,
                PeriodoActual = request.PeriodoActual,
                NumeroCredito = request.NumeroObligacion == string.Empty ? null : long.Parse(request.NumeroObligacion)
            };

            var responseConsultCartera = await _reportesBusinessLogic.Value.GenerarReporteGeneralCartera(requeteReporte);

            var totalCredito = responseConsultCartera.Data.Sum(x => x.SaldoCapital);
            var totalProvision = responseConsultCartera.Data.Sum(x => x.ProvisionTotal);
            var totalCapital= responseConsultCartera.Data.Sum(x => x.ValorCapital);
            // Agrupar por obligación para calcular totales
            var datosAgrupados = responseConsultCartera.Data
                .GroupBy(x => new
                {
                    x.NumeroCredito,
                    x.Identificacion,
                    x.NombreCompleto
                })
                .Select(grupo => new ConsultaSaldosResponse
                {
                    NumeroObligacion = grupo.Key.NumeroCredito,
                    Identificacion = grupo.Key.Identificacion,
                    Nombre = grupo.Key.NombreCompleto,
                    // Saldo según tipo de crédito (1=Leasing, 2=Hipotecario, ajusta según tus IDs)
                    SaldoLeasing = grupo
                        .Where(x => x.IdTipoCredito == 1) // Ajusta el ID de Leasing
                        .Sum(x => x.SaldoCapital),

                    SaldoHipotecario = grupo
                        .Where(x => x.IdTipoCredito == 2) // Ajusta el ID de Hipotecario
                        .Sum(x => x.SaldoCapital),

                    Provision = grupo.Sum(x => x.ProvisionTotal),

                    // Totales por tipo
                    totalCreditoLeasing = grupo
                        .Where(x => x.IdTipoCredito == 1)
                        .Sum(x => x.ValorDesembolso),

                    totalCreditoHipotecario = grupo
                        .Where(x => x.IdTipoCredito == 2)
                        .Sum(x => x.ValorDesembolso),

                    TipoCredito = string.Join(", ", grupo.Select(x => x.TipoCredito).Distinct())
                });
            //var response = this._mapper.Map<IEnumerable<ConsultaSaldosResponse>>(await _carteraRepository.ConsultarDataCreditoReportes(request));
            var response = datosAgrupados.AsEnumerable();
            List<string> listaCedulas = new();
            listaCedulas = response.Select(x => x.Identificacion).ToList();

            //Response<IEnumerable<ClienteDto>> cuentas = await _iClientRequestProvider.ObtenerNombresClientes(listaCedulas);

            //response.ForEach(x =>
            //{
            //    ClienteDto nombre = cuentas.Data.FirstOrDefault(y => y.Identificacion == x.Identificacion);
            //    if (nombre != null)
            //    {
            //        x.Nombre = $"{nombre.PrimerNombre} {nombre.SegundoNombre} {nombre.PrimerApellido} {nombre.SegundoApellido} ";
            //    }
            //});

            return new Response<IEnumerable<ConsultaSaldosResponse>> { Data = response };
        }


        public async Task<Response<CreditoCarteraProducto>> ObtenerProductoCreditoCartera(string numeroDocumento)
        {
            // Inicialización del objeto con todas sus propiedades
            CreditoCarteraProducto creditoCartera = new CreditoCarteraProducto
            {
                DatosAfiliadoCarteraProducto = new DatosAfiliadoCarteraProducto(),
                InformacionInmueble = new InformacionInmuebleCarteraProducto
                {
                    MatriculasInmueble = new List<MatriculaCredito>()
                },
                DatosProductoCarteraProducto = new DatosProductoCarteraProducto(),
                DatosSeguros = new DatosSeguros(),
                BeneficiosAliviosCarteraProducto = new BeneficiosAliviosCarteraProducto()
            };


            try
            {



                // Obtener datos del afiliado y cliente
                var afiliadoProducto = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, numeroDocumento))?.Data;
                var clienteProducto = await _creditoRepository.ObtenerSimulacionPersonales(numeroDocumento);
                // Validación antes de acceder a `inmuebleProducto.Data`
                var inmuebleProducto = await _creditoBusinessLogic.ObtenerInformacionSolicitudCreditoCartera(1, numeroDocumento);
                if (inmuebleProducto?.Data == null)
                {
                    return new Response<CreditoCarteraProducto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontró información del inmueble",
                        Data = creditoCartera
                    };
                }

                // Validación antes de acceder a `SOC_ID`
                var socId = inmuebleProducto.Data.SOC_ID;
                if (socId == null)
                {
                    return new Response<CreditoCarteraProducto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontró SOC_ID en la información del inmueble",
                        Data = creditoCartera
                    };
                }

                RequestSolicCreditoInfJuridicaDto requestSolicCreditoInfJuridicaDto = new RequestSolicCreditoInfJuridicaDto();
                requestSolicCreditoInfJuridicaDto.idSolicitudCredito = socId;
                requestSolicCreditoInfJuridicaDto.indicador = 2;
                var datosAdicionalesInmueble = await this._creditoBusinessLogic.ObtenerSolicCreditoInfJuridica(requestSolicCreditoInfJuridicaDto);

                PeticionSolicitudCreditoProducto peticion = new PeticionSolicitudCreditoProducto();
                peticion.SOC_ID = socId;
                peticion.IND = 2; 

                RespuestaCreditoProducto resultadoInmuebleInformacion = await _creditoRepository.ObtenerSolicCreditoProducto(peticion);

                Guid slsId = inmuebleProducto.Data.SLS_ID;

                UbicacionInmuebleCredito ubicacionInmueble = await _creditoRepository.ObtenerUbicacionViviendaCredito(slsId);

                ValoresProyectados valoresProyectado = await this._creditoRepository.ObtenerValoresProyectados(slsId);

                IEnumerable<MatriculaCreditoDto> informacionMatricula = await _creditoRepository.ObtenerMatriculasCreditoporSolicitud(socId);
                SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(socId);
                Escritura informacionNotaria = await _escrituraRepository.ObtenerEscritura(new Escritura { CLI_IDENTIFICACION = numeroDocumento, SOC_ID_SOLICITUD = socId });
                SolicCreditoInfJuridica responseInfoJur = await _creditoRepository.ObtenerSolicCreditoInfJuridica(new RequestSolicCreditoInfJuridica { SOC_ID = socId, IND = 2 });
                var obtenerInformacionCredito = await this._creditoBusinessLogic.ObtenerSolicCreditoProducto(new PeticionSolicitudCreditoProductoDto { SOC_ID = socId, IND = 2 });

                ConsultaSolicitudCredito request = new ConsultaSolicitudCredito();
                request.CLI_IDENTIFICACION = numeroDocumento;
                request.SOC_ESTADO = "0"; 

                ConsultaSolicitudCredito datosSegurosCreditoAseguradoras = await _creditoRepository.ConsultarSolicitudCredito(request); 


                FacturaSevenCredito facturaSeven = await _creditoRepository.ConsultarDatosFactura(socId);

                if (facturaSeven != null)
                {
                    string dia = facturaSeven.FAC_FECHA_REGISTRO_TERCERO.Day.ToString();
                    string mes = facturaSeven.FAC_FECHA_REGISTRO_TERCERO.Month.ToString();
                    string anio = facturaSeven.FAC_FECHA_REGISTRO_TERCERO.Year.ToString();
                    creditoCartera.DatosProductoCarteraProducto.NumeroCredito = anio + "" + mes + "" + dia + "" + inmuebleProducto.Data.SOC_NUMERO_SOLICITUD.ToString();

                }


                decimal recursosFuturos = 0;
                if (valoresProyectado != null)
                {
                    recursosFuturos = valoresProyectado.SVP_AHORROS + valoresProyectado.SVP_SUBSIDIO + valoresProyectado.SVP_CESANTIAS;
                }

                // Validación antes de acceder a `TRS_ID`
                var trsId = inmuebleProducto.Data.TRS_ID;
                if (trsId == null)
                {
                    return new Response<CreditoCarteraProducto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontró TRS_ID en la información del inmueble",
                        Data = creditoCartera
                    };
                }

                var aprobaciones = await _creditoRepository.ConsultarAprobacionAsesor(trsId);
                var tasas = await _carteraRepository.ObtenerTasasSimulador();
                var TasasSeguroGeneral = await _carteraRepository.ObtenerSeguroGeneral();
                var informacionTecnicaInmuble = await _creditoRepository.ObtenerSolicCreditoInfTecnica(new RequestSolicCreditoInfTecnica { SOC_ID = socId.ToString(), IND = 2 });

                // Validación de `clienteProducto.DPD_ID` antes de llamar a `ObtenerTasasHogarUbicacion`
                if (clienteProducto?.DPD_ID == null)
                {
                    return new Response<CreditoCarteraProducto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontró DPD_ID en la información del cliente",
                        Data = creditoCartera
                    };
                }

                var tasasHogar = await _creditoBusinessLogic.ObtenerTasasHogarUbicacion(clienteProducto.DPD_ID);
                if (tasasHogar?.Data == null || !tasasHogar.Data.Any())
                {
                    return new Response<CreditoCarteraProducto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontraron tasas de hogar para la ubicación",
                        Data = creditoCartera
                    };
                }

                var SeguroHogar = (tasasHogar.Data.First().ValorTasa / 1000) / 12;



                #region Datos Afiliado
                if (clienteProducto != null)
                {
                    creditoCartera.DatosAfiliadoCarteraProducto.Identificacion = clienteProducto.SDP_NUMERO_DOCUMENTO;
                    creditoCartera.DatosAfiliadoCarteraProducto.Edad = clienteProducto.SDP_EDAD;
                }
                if (afiliadoProducto != null)
                {
                    creditoCartera.DatosAfiliadoCarteraProducto.PrimerNombre = afiliadoProducto.PrimerNombre;
                    creditoCartera.DatosAfiliadoCarteraProducto.SegundoNombre = afiliadoProducto.SegundoNombre;
                    creditoCartera.DatosAfiliadoCarteraProducto.PrimerApellido = afiliadoProducto.PrimerApellido;
                    creditoCartera.DatosAfiliadoCarteraProducto.SegundoApellido = afiliadoProducto.SegundoApellido;
                    creditoCartera.DatosAfiliadoCarteraProducto.Fuerza = afiliadoProducto.IdFuerza;
                }
                creditoCartera.DatosAfiliadoCarteraProducto.RecursosFuturos = recursosFuturos > 0 ? recursosFuturos : 0;

                #endregion

                var idActor = afiliadoProducto.IdTipoActor;
                string tipoVinculacionAfiliado = "";
                foreach (var tipoVinculacion in afiliadoProducto.TiposVinculacion)
                {
                    if (idActor == tipoVinculacion.IdTipoActor)
                    {
                        tipoVinculacionAfiliado = tipoVinculacion.DescripcionTipoActor; break;
                    }
                }


                var tipoCredito = inmuebleProducto.Data.TCR_ID == 2 ? "HIPOTECARIO" : "LEASING";


                var tasaEA = tasas
                            .Where(a => a.SIM_DESCRIPCION == tipoCredito + " " + tipoVinculacionAfiliado)
                            .Select(a => a.SIM_TASA_EA)
                            .FirstOrDefault();

                #region Información del Inmueble
                if (clienteProducto != null)
                {
                    creditoCartera.InformacionInmueble.ValorInmueble = clienteProducto.SDP_VALOR_INMUEBLE;
                    creditoCartera.InformacionInmueble.ValorAvaluo = clienteProducto.SDP_VALOR_AVALUO_INMUEBLE;
                    creditoCartera.InformacionInmueble.TipoViviendaID = clienteProducto.TVV_ID;
                    creditoCartera.InformacionInmueble.TipoViviendaDescripcion = clienteProducto.TIPO_VIVIENDA;
                    creditoCartera.InformacionInmueble.Direccion = inmuebleProducto.Data.SOP_DIRECCION_INMUEBLE;
                    creditoCartera.InformacionInmueble.IdCiudad = int.Parse(ubicacionInmueble.DPC_CODIGO);
                    creditoCartera.InformacionInmueble.CiudadDescripcion = clienteProducto.CIUDAD_INMUEBLE;
                    creditoCartera.InformacionInmueble.IdDepartamento = int.Parse(ubicacionInmueble.DPD_CODIGO);
                    creditoCartera.InformacionInmueble.DepartamentoDescripcion = clienteProducto.DEPARTAMENTO_INMIUEBLE;
                    creditoCartera.InformacionInmueble.DescripcionTipoVivienda = clienteProducto.SMC_VIVIENDA_VIS;
                    creditoCartera.InformacionInmueble.IdTipoVivienda = clienteProducto.SMC_VIVIENDA_VIS.Equals("V") ? 1 : 2;
                    creditoCartera.InformacionInmueble.ClaseInmuebleID = inmuebleProducto.Data.SOP_TIPO_INMUEBLE.Equals("APARTAMENTO") ? 109 : 100;
                    creditoCartera.InformacionInmueble.TasasEA = clienteProducto.SMC_VALOR_TASA_EA;
                    creditoCartera.InformacionInmueble.dpd_codigo = resultadoInmuebleInformacion.DPD_ID.ToString();
                    creditoCartera.InformacionInmueble.dpc_codigo = resultadoInmuebleInformacion.DPC_ID.ToString();
                    creditoCartera.InformacionInmueble.DescripcionInmueble = datosAdicionalesInmueble.Data.DescripcionInmueble;
                }
                if (inmuebleProducto?.Data != null)
                {
                    creditoCartera.InformacionInmueble.ClaseInmueble = inmuebleProducto.Data.SOP_TIPO_INMUEBLE;
                }
                if (informacionMatricula != null)
                {
                    foreach (var matricula in informacionMatricula)
                    {
                        creditoCartera.InformacionInmueble.MatriculasInmueble.Add(new MatriculaCredito
                        {
                            NumeroMatricula = matricula.NumeroMatricula,
                            IdMatricula = matricula.IdMatricula,
                            Tipo_Matricula = matricula.Tipo_Matricula,
                            SocId = matricula.SocId
                        });
                    }
                }
                if (informacionNotaria != null && informacionNotaria.ESC_FECHA >= new DateTime(1753, 1, 1) && informacionNotaria.ESC_FECHA <= new DateTime(9999, 12, 31))
                {
                    creditoCartera.InformacionInmueble.FechaEscritura = informacionNotaria.ESC_FECHA;
                    creditoCartera.InformacionInmueble.Notaria = informacionNotaria.ESC_NOTARIA;
                }
                if (responseInfoJur != null)
                {
                    creditoCartera.InformacionInmueble.Linderos = responseInfoJur.SIJ_LINDEROS;
                    creditoCartera.InformacionInmueble.Escritura = responseInfoJur.SJI_ESCRITURA;
                }
                #endregion

                #region Datos Producto
                if (inmuebleProducto?.Data != null)
                {
                    creditoCartera.DatosProductoCarteraProducto.TipoCredito = inmuebleProducto.Data.TCR_ID;
                    creditoCartera.DatosProductoCarteraProducto.LineaCredito = clienteProducto.CLC_ID;

                    if (inmuebleProducto.Data.SOC_FECHA_SOLICITUD >= new DateTime(1753, 1, 1) && inmuebleProducto.Data.SOC_FECHA_SOLICITUD <= new DateTime(9999, 12, 31))
                    {
                        DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));
                        creditoCartera.DatosProductoCarteraProducto.FechaSolicitud = ultimoDia;
                    }
                }

                var valorDesembolso = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(inmuebleProducto.Data.SOC_ID)).Where(x => x.FRC_DESCRIPCION == "Recursos Caja");

                if (informacionDesembolso != null)
                {

                    decimal tasaCredito = clienteProducto.SMC_VALOR_TASA_EA * 100;
                    creditoCartera.DatosProductoCarteraProducto.FechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;
                    creditoCartera.DatosProductoCarteraProducto.ValorDesembolso = valorDesembolso.Select(x => x.SID_VALOR_DESEMBOLSO).FirstOrDefault();
                    creditoCartera.DatosProductoCarteraProducto.OpcionDeCompra = clienteProducto.SMC_PORCEN_OPCION_DE_COMPRA;
                    creditoCartera.DatosProductoCarteraProducto.CanonInial = clienteProducto.SMC_PORCEN_CANON_INICIAL;
                    creditoCartera.DatosProductoCarteraProducto.Score = inmuebleProducto.Data.SOC_SCORE_TU;
                    creditoCartera.DatosProductoCarteraProducto.PlazoTotal = obtenerInformacionCredito.Data.SOP_PLAZO_FINANCIACION;
                    creditoCartera.DatosProductoCarteraProducto.NumeroRadicado = inmuebleProducto.Data.SOC_NUMERO_RADICADO;
                    creditoCartera.DatosProductoCarteraProducto.TasaCreditoEA = tasaCredito.ToString("0.##");
                    creditoCartera.DatosProductoCarteraProducto.TipoAbonoExtra = 0;
                    creditoCartera.DatosProductoCarteraProducto.ValorAbonoExtra = 0;
                }

                var aprobadoPasoCartera = aprobaciones.Any(a =>
                            a.TAC_DESCRIPCION == "CREACIÓN DE CRÉDITO" &&
                            a.ACT_ESTADO == 4);

                creditoCartera.AprobadoPasoCartera = aprobadoPasoCartera;

                #endregion

                #region Datos Seguro
                var tasaSeguroVida = TasasSeguroGeneral
                .Where(a => a.SEG_ID == 1)
                .Select(a => a.SEG_VIDA)
                .FirstOrDefault();

                creditoCartera.DatosSeguros.SeguroVida = inmuebleProducto.Data.TCR_ID == 2 ? clienteProducto.SMC_TASA_SEGURO_VIDA : clienteProducto.SMC_TASA_SEGURO_VIDA * 100;
                creditoCartera.DatosSeguros.SeguroHogar = inmuebleProducto.Data.TCR_ID == 2 ? clienteProducto.SMC_TASA_SEGURO_HOGAR : clienteProducto.SMC_TASA_SEGURO_HOGAR * 100;
                creditoCartera.DatosSeguros.AseguradoraVida = datosSegurosCreditoAseguradoras.ASEGURADORA;
                creditoCartera.DatosSeguros.IdAseguradoraVida = datosSegurosCreditoAseguradoras.ASE_ID; 
                #endregion

                #region Beneficios y Alivios

                creditoCartera.BeneficiosAliviosCarteraProducto.AlivioCuotaFrech = inmuebleProducto.Data.SOC_VALOR_ALIVIO > 0 ? true : false;
                creditoCartera.BeneficiosAliviosCarteraProducto.AlivioTasaFrechPorcentaje = inmuebleProducto.Data.SOC_TASA_FECH;

                #endregion


                creditoCartera.SOC_ID = inmuebleProducto.Data.SOC_ID;
                creditoCartera.SMC_ID = clienteProducto.SMC_ID;
            }
            catch (Exception ex)
            {
                return new Response<CreditoCarteraProducto>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error al obtener información: {ex.Message}",
                    Data = creditoCartera
                };
            }
            return new Response<CreditoCarteraProducto> { Data = creditoCartera };
        }





        #region
        private async Task<Response<AplicacionPagoDto>> AmpliacionCuota(PagoAplicarDto request)
        {
            ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
            {
                Nro_Credito = request.NumeroCredito,
                Tcr_Id = request.Tcr_id
            };
            InformacionCredito infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);
            var CapitalAbonado = await this._carteraRepository.ObtenerCapitalNeto_AbonosExtra(request.Identificacion);
            var cuotasAcumuladas = await this._iClientRequestProvider.ObtenerCuotasNumeroIdentificacion(infoCredito.CLI_IDENTIFICACION);
            AplicacionPagoDto response = new AplicacionPagoDto();

            List<InsertPlanPagosDto> LstPln = new();
            double NuevaCuotaReducido = 0.0;
            double NuevaCuotaTrad = 0.0;
            infoCredito.CRE_PLAZO_ACTUAL = request.NumeroCuotas;
            infoCredito.CRE_PLAZO_TOTAL = request.NumeroCuotas;
            if (request.Tcr_id == 1)
            {
                //Leasing
                ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                if (responsePlanPagoDto != null)
                {
                    List<ConformacionLeasing> LstConformacionLeasing = new List<ConformacionLeasing>();
                    var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                    var PlazoActual = request.NumeroCuotas - PlazoCancelado;
                    var ValorSaldoActual = responsePlanPagoDto.PlanPago.Where(x => x.Cuota == 1).FirstOrDefault().SaldoInicial - (decimal)CapitalAbonado;

                    var SaldoNuevo = ValorSaldoActual;

                    var DeudaRemanenteNueva = responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA") && x.Cuota == PlazoCancelado).LastOrDefault().DeudaRemanente - request.ValorPago;
                    var SaldoCapitalTradPagado = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalReducido) + (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                    var SaldoCapitalTradActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);
                    var CapitalPorPagar = (double)responsePlanPagoDto.PlanPago.Where(x => x.EstadoCuota.Equals("PAGADA")).Sum(x => x.CapitalPorPagar);
                    var SaldoCapitalTradNuevo = (double)SaldoCapitalTradActual - (double)request.ValorPago - CapitalPorPagar;


                    #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                    for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - 1; i++)
                    {
                        double DeudaRemanenteMes = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS - 1
                            ? (double)responsePlanPagoDto.PlanPago[i].DeudaRemanente - (double)request.ValorPago
                            : (double)responsePlanPagoDto.PlanPago[i].DeudaRemanente;

                        InsertPlanPagosDto insertPlanPagosDto = new()
                        {
                            PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                            PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                            PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                            PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].SaldoInicial,
                            PlnCapital = responsePlanPagoDto.PlanPago[i].CapitalReducido,
                            PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                            PlnInteres = responsePlanPagoDto.PlanPago[i].InteresReducido,
                            PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                            PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                            PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                            PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                            PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                            PlnValorDeudaRemanente = (decimal)DeudaRemanenteMes
                        };
                        if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                        {
                            insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                        }
                        LstPln.Add(insertPlanPagosDto);
                    }
                    #endregion

                    for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i <= infoCredito.CRE_PLAZO_TOTAL - 1; i++)
                    {
                        if (i == infoCredito.CRE_PLAZO_TOTAL - 1)
                        {
                            Console.WriteLine($"Índice fuera de rango: i = {i - 1}, Tamaño de PlanPago = {responsePlanPagoDto.PlanPago.Count}");
                        }
                        if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                        {
                            ConformacionLeasing CL = new ConformacionLeasing();
                            CL.Mes = i;
                            //Tradicional
                            CL.Seguros = (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroHogar + (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroVida;
                            CL.Canon = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros, 0);
                            CL.Intereses = Math.Round(SaldoCapitalTradNuevo * infoCredito.CRE_TASA_CREDITO_PER, 0);
                            CL.AporteCapital = CL.Canon - CL.Intereses;
                            CL.SaldoaCapital = SaldoCapitalTradNuevo - CL.AporteCapital;
                            CL.CanonSeguro = CL.Canon + CL.Seguros;

                            //Leasing
                            CL.InteresReducido = Math.Round((double)SaldoNuevo * infoCredito.CRE_TASA_CREDITO_PER, 0);
                            CL.CanonReducido = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros;
                            CL.InteresRemanente = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i + 1).FirstOrDefault().InteresPorPagar : 0;
                            CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                            CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == i + 1).FirstOrDefault().CapitalPorPagar : 0;
                            CL.SaldoaCapitalReducido = (double)SaldoNuevo - CL.AporteCapitalReducido;
                            CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                            CL.DeudaRemanente = (double)DeudaRemanenteNueva - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                            CL.DeudaRemanenteAnterior = (double)LstPln[i - 1].PlnValorDeudaRemanente;

                            LstConformacionLeasing.Add(CL);
                            //Insertamos las nuevas cuotas
                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = i + 1,
                                PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                PlnSaldoIncial = (decimal)LstPln[i - 1].PlnValorDeudaRemanente,
                                PlnCapital = (decimal)CL.AporteCapitalReducido,
                                PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                PlnInteres = (decimal)CL.InteresReducido,
                                PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                                PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                                PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                            };
                            LstPln.Add(insertPlanPagosDto);
                            //Luego de que quede el mes siguiente bien, calculamos la nueva cuota
                            NuevaCuotaReducido = Math.Round(await CalcularPago(infoCredito.CRE_TASA_CREDITO_PER, infoCredito.CRE_PLAZO_TOTAL - (i + 1), -CL.SaldoaCapitalReducido, 0, 0), 0);
                            var DifCuota = (double)infoCredito.CRE_VALOR_CUOTA - CL.Seguros - NuevaCuotaReducido;
                            NuevaCuotaTrad = Math.Round((double)infoCredito.CRE_VALOR_CUOTA_TRADICIONAL - CL.Seguros - DifCuota, 0);

                        }
                        else
                        {
                            ConformacionLeasing CL = new ConformacionLeasing();
                            CL.Mes = i;
                            //Tradicional
                            CL.Canon = (double)NuevaCuotaTrad;
                            CL.Intereses = Math.Round(LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital * infoCredito.CRE_TASA_CREDITO_PER, 0);
                            CL.AporteCapital = CL.Canon - CL.Intereses;
                            CL.SaldoaCapital = LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapital - CL.AporteCapital;
                            CL.Seguros = (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroHogar + (double)responsePlanPagoDto.PlanPago.Where(x => x.Cuota == PlazoCancelado).FirstOrDefault().SeguroVida;
                            CL.CanonSeguro = CL.Canon + CL.Seguros;

                            //Leasing
                            CL.InteresReducido = Math.Round(LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido * infoCredito.CRE_TASA_CREDITO_PER, 0);
                            CL.CanonReducido = i == infoCredito.CRE_PLAZO_TOTAL ? LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido + CL.InteresReducido : (double)NuevaCuotaReducido;
                            CL.InteresRemanente = infoCredito.CLC_ID != 3 ? CL.Intereses - CL.InteresReducido : 0;
                            CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                            CL.CapitalNoAmortizado = infoCredito.CLC_ID != 3 ? CL.AporteCapital - CL.AporteCapitalReducido : 0;
                            CL.SaldoaCapitalReducido = LstConformacionLeasing[LstConformacionLeasing.Count - 1].SaldoaCapitalReducido - CL.AporteCapitalReducido;
                            CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                            CL.DeudaRemanente = LstConformacionLeasing[LstConformacionLeasing.Count - 1].DeudaRemanente - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;
                            LstConformacionLeasing.Add(CL);
                            //Insertamos las nuevas cuotas

                            //decimal NuevoCalculoSaldoTradicional = SaldoCapitalTradicional - ValorPago;



                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = i + 1,
                                PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                PlnSaldoIncial = (decimal)LstPln[i - 1].PlnValorDeudaRemanente,
                                PlnCapital = (decimal)CL.AporteCapitalReducido,
                                PlnCapitalPorPagar = (decimal)CL.CapitalNoAmortizado,
                                PlnInteres = (decimal)CL.InteresReducido,
                                PlnInteresPorPagar = (decimal)CL.InteresRemanente,
                                PlnSeguroHogar = responsePlanPagoDto.PlanPago[i - 1].SeguroHogar,
                                PlnSeguroVida = responsePlanPagoDto.PlanPago[i - 1].SeguroVida,
                                PlnAPagar = (decimal)CL.CanonReducidoSeguros,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = (decimal)CL.DeudaRemanente
                            };
                            LstPln.Add(insertPlanPagosDto);
                        }
                    }
                    #region Comienzo de abono pago
                    RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                    {
                        LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                        NumeroCredito = request.NumeroCredito,
                        Identificacion = request.Identificacion,
                        Fecha_Pago_Cuota = request.Fecha_Pago_Cuota,
                        Fecha_Pago = request.Fecha_Pago,
                        NuevaCuotaReducida = NuevaCuotaReducido,
                        NuevaCuotaTradicional = NuevaCuotaTrad,
                        PlazoTotal = infoCredito.CRE_PLAZO_TOTAL,
                        PlazoActual = infoCredito.CRE_PLAZO_ACTUAL,
                        NuevoSaldoReducido = (double)SaldoNuevo,
                        NuevoSaldoTradicional = SaldoCapitalTradNuevo,
                        FechaFinalizaPagos = null,
                        PagoExtraOrdinario = 0
                    };
                    #endregion
                    response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.AplicaPagoReducirCuota(requestPagoAbonoCapital, infoCredito, request));

                }

            }
            else
            {
                //Hipotecario
                ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
                if (responsePlanPagoDto != null)
                {
                    List<Hipotecario> LstConformacionHipotecario = new();
                    var PlazoCancelado = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                    var PlazoActual = infoCredito.CRE_PLAZO_ACTUAL;
                    var ValorSaldoActual = infoCredito.CRE_VALOR_DESEMBOLSO - Math.Round((decimal)CapitalAbonado, 0);

                    var SaldoNuevo = ValorSaldoActual - request.ValorPago;

                    //Para Hipotecario es Cre_Saldo_Capital
                    //if (!(Math.Abs(ValorSaldoActual - infoCredito.CRE_SALDO_CAPITAL) <= 10))
                    //{
                    //    //Devuelvo Alerta para verificar porque los saldos no estan igual.
                    //    return new Response<AplicacionPagoDto>() { Data = null, IsSuccess = false, ReturnMessage = "Alerta!! El saldo no coincide, verifique los pagos/abonos realizados." };
                    //}

                    #region Llenamos el nuevo Plan de pagos hasta la cuota que no se modifica
                    for (int i = 0; i <= infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS; i++)
                    {
                        InsertPlanPagosDto insertPlanPagosDto = new()
                        {
                            PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                            PlnCuota = responsePlanPagoDto.PlanPago[i].Cuota,
                            PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                            PlnSaldoIncial = responsePlanPagoDto.PlanPago[i].Saldo,
                            PlnCapital = responsePlanPagoDto.PlanPago[i].Capital,
                            PlnCapitalPorPagar = responsePlanPagoDto.PlanPago[i].CapitalPorPagar,
                            PlnInteres = responsePlanPagoDto.PlanPago[i].Interes,
                            PlnInteresPorPagar = responsePlanPagoDto.PlanPago[i].InteresPorPagar,
                            PlnSeguroHogar = responsePlanPagoDto.PlanPago[i].SeguroHogar,
                            PlnSeguroVida = responsePlanPagoDto.PlanPago[i].SeguroVida,
                            PlnAPagar = responsePlanPagoDto.PlanPago[i].CuotaUnidadPagadora,
                            PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                            PlnValorDeudaRemanente = responsePlanPagoDto.PlanPago[i].DeudaRemanente
                        };
                        if (i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS)
                        {
                            insertPlanPagosDto.PlnSaldoIncial = SaldoNuevo;
                            //NuevaCuotaReducido = (decimal)Math.Round(await CalcularPago(infoCredito.CRE_TASA_CREDITO_PER, infoCredito.CRE_PLAZO_TOTAL - i, (double)SaldoNuevo, 0, 0), 0) + responsePlanPagoDto.PlanPago[i].SeguroHogar;

                            NuevaCuotaReducido = -Math.Round(await CalcularPago(
                            infoCredito.CRE_TASA_CREDITO_PER,
                            infoCredito.CRE_PLAZO_TOTAL - i,
                            (double)SaldoNuevo,
                            0,
                            0), 0) + (double)responsePlanPagoDto.PlanPago[i].SeguroHogar;


                            //NuevaCuotaReducido = Math.Round(await CalcularPago(infoCredito.CRE_TASA_CREDITO_PER, infoCredito.CRE_PLAZO_TOTAL - i, (double)SaldoNuevo, 0, 0), 0) + responsePlanPagoDto.PlanPago[i].SeguroHogar;

                        }
                        LstPln.Add(insertPlanPagosDto);
                    }
                    #endregion

                    for (int i = infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1; i <= infoCredito.CRE_PLAZO_TOTAL; i++)
                    {
                        var SaldoEvaluado = i == infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS + 1 ? (double)SaldoNuevo : LstConformacionHipotecario[LstConformacionHipotecario.Count - 1].Saldo;
                        Hipotecario H = new();
                        H.Cuota = i;
                        H.Seguro_Vida = Math.Round(SaldoEvaluado * (infoCredito.CRE_TASA_SEGURO_VIDA / 100), 0);
                        H.Seguro_Hogar = Math.Round((double)infoCredito.CRE_VALOR_VIVIENDA * (infoCredito.CRE_TASA_SEGURO_HOGAR / 100), 0);
                        H.Cuota_Pagadora = Math.Round((double)infoCredito.CRE_VALOR_CUOTA, 0);
                        H.Intereses = Math.Round(SaldoEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                        H.Capital = H.Cuota_Pagadora - H.Seguro_Vida - H.Seguro_Hogar - H.Intereses;
                        H.Saldo = SaldoEvaluado - H.Capital;
                        if (H.Saldo < 0)
                        {
                            H.Intereses = Math.Round(SaldoEvaluado * infoCredito.CRE_TASA_CREDITO_PER, 0);
                            H.Cuota_Pagadora = SaldoEvaluado + H.Seguro_Vida + H.Seguro_Hogar + H.Intereses;
                            H.Capital = SaldoEvaluado;
                            H.Saldo = SaldoEvaluado - H.Capital;
                            LstConformacionHipotecario.Add(H);
                            //Insertamos las nuevas cuotas
                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = i,
                                PlnFechaPago = responsePlanPagoDto.PlanPago[i].Fecha,
                                PlnCapital = (decimal)H.Capital,
                                PlnSaldoIncial = (decimal)H.Saldo,
                                PlnCapitalPorPagar = 0,
                                PlnInteres = (decimal)H.Intereses,
                                PlnInteresPorPagar = 0,
                                PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                PlnSeguroVida = (decimal)H.Seguro_Vida,
                                PlnAPagar = (decimal)H.Cuota_Pagadora,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = 0
                            };
                            try
                            {

                                LstPln.Add(insertPlanPagosDto);
                                foreach (InsertPlanPagosDto item in LstPln)
                                    item.PlnPlazo = i;
                                break;
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                        else
                        {
                            try
                            {

                                LstConformacionHipotecario.Add(H);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            //Insertamos las nuevas cuotas
                            InsertPlanPagosDto insertPlanPagosDto = new()
                            {
                                PlnNroCredito = infoCredito.CRE_NRO_CREDITO,
                                PlnCuota = i,
                                PlnFechaPago = responsePlanPagoDto.PlanPago[i - 1].Fecha,
                                PlnCapital = (decimal)H.Capital,
                                PlnSaldoIncial = (decimal)H.Saldo,
                                PlnCapitalPorPagar = 0,
                                PlnInteres = (decimal)H.Intereses,
                                PlnInteresPorPagar = 0,
                                PlnSeguroHogar = (decimal)H.Seguro_Hogar,
                                PlnSeguroVida = (decimal)H.Seguro_Vida,
                                PlnAPagar = (decimal)H.Cuota_Pagadora,
                                PlnPlazo = infoCredito.CRE_PLAZO_TOTAL,
                                PlnValorDeudaRemanente = 0
                            };
                            try
                            {

                                LstPln.Add(insertPlanPagosDto);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }

                    #region Comienzo de Abono Capital
                    RequestPagoAbonoCapital requestPagoAbonoCapital = new()
                    {
                        LstPln = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstPln),
                        NumeroCredito = request.NumeroCredito,
                        Identificacion = request.Identificacion,
                        Fecha_Pago_Cuota = DateTime.Now,
                        Fecha_Pago = DateTime.Now,
                        NuevaCuotaReducida = NuevaCuotaReducido,
                        NuevaCuotaTradicional = NuevaCuotaTrad,
                        PlazoTotal = infoCredito.CRE_PLAZO_TOTAL,
                        PlazoActual = infoCredito.CRE_PLAZO_ACTUAL,
                        NuevoSaldoReducido = (double)SaldoNuevo,
                        NuevoSaldoTradicional = 0,
                        FechaFinalizaPagos = null,
                        PagoExtraOrdinario = 0
                    };
                    #endregion
                    response = this._mapper.Map<AplicacionPagoDto>(await this._carteraRepository.AplicaPagoReducirCuota(requestPagoAbonoCapital, infoCredito, request));
                    return new Response<AplicacionPagoDto>() { Data = response, IsSuccess = true, ReturnMessage = "OK" };
                }
                return new Response<AplicacionPagoDto>() { Data = response, IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<AplicacionPagoDto>() { Data = response, IsSuccess = true, ReturnMessage = "OK" };


        }
        #endregion


        #region CrearProductoCartera
        public async Task<Response<List<InsertPlanPagosDto>>> CrearPlanPagoDesdeSimulacion(InsertProductoClienteDto request, CancellationToken cancellationToken)
        {
            List<InsertPlanPagosDto> LstinsertPlanPagosDto = new List<InsertPlanPagosDto>();
            var MapeoCredito = (await MapeoCreCredito(request));
            if (!MapeoCredito.IsSuccess)
            {
                return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = MapeoCredito.ReturnMessage };
            }

            // TODO CONFIRMAR: nombres reales de propiedades de InsertProductoClienteDto
            var requestSimulacion = new CreacionSimulacionClienteDto // TODO CONFIRMAR nombre real del tipo
            {
                valor = (double)request.ValorDesembolso,
                documento = request.NumeroIdentificacion,
                tipoIdentificacion = request.TipoIdentificacion,
                tipoVivienda = MapeoCredito.Data.TIV_ID == 1 ? "U" : "N",
                TipoAbono = 1,
                AplicaBeneficio = false,
                AbonoUnico = false,
                valorCuota = 0,
                valorPrestamo = 0,
                ValorViviendaSeguro = (double)request.ValorViviendaSeguroHogar,
                TasaVoluntariaAportes = false,
                plazo = request.PlazoTotal,
                simId = MapeoCredito.Data.TCR_ID,
                avaluoInmueble = 0,
                tipoSimularOProspectar = 0,
                Usr_id = null,
                afl_id = 0
            };

            var dataSimulacion = await _prospeccionBusinessLogic.Value.CrearSimulacionCliente(requestSimulacion);
            if (!dataSimulacion.IsSuccess)
            {
                return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = dataSimulacion.ReturnMessage };
            }

            switch (MapeoCredito.Data.TCR_ID)
            {
                case 1: // LEASING — columnas confirmadas por relacion_columnas_credito_leasing.pdf
                    var filasLeasing = dataSimulacion.Data.ToList(); // List<SimulacionDto>
                                                                   // TODO CONFIRMAR: ¿arranca en índice 0 (fila de desembolso, se salta) o ya arranca en cuota 1?
                    for (int i = 1; i < filasLeasing.Count; i++)
                    {
                        LstinsertPlanPagosDto.Add(new InsertPlanPagosDto()
                        {
                            PlnNroCredito = MapeoCredito.Data.CRE_NRO_CREDITO,
                            PlnCuota = filasLeasing[i].Cuota,
                            PlnFechaPago = filasLeasing[i].FechaPago,
                            // TODO CONFIRMAR: saldo de la fila actual (SaldoLeasing) vs remanente de la fila anterior
                            PlnSaldoIncial = filasLeasing[i - 1].SaldoLeasing,
                            PlnCapital = filasLeasing[i].Capitalpagar,
                            // TODO CONFIRMAR: el PDF no lista "capital/interés por pagar" para leasing
                            PlnCapitalPorPagar = filasLeasing[i].CapitalPorPagar,
                            PlnInteres = filasLeasing[i].InteresLeasing, // TODO CONFIRMAR: el PDF no incluye "Intereses" para leasing, solo Canon/Capital/Saldo
                            PlnInteresPorPagar = filasLeasing[i].InteresesPorPagar,
                            PlnSeguroVida = Math.Round(filasLeasing[i].SeguroVidaLeasing, 0),
                            PlnSeguroHogar = Math.Round((decimal)filasLeasing[i].SeguroHogar, 0),
                            PlnAPagar = filasLeasing[i].CuotaLeasing,
                            // TODO CONFIRMAR: el PDF no lo lista; dejo ValorDeudaRemanente como en el método viejo
                            PlnValorDeudaRemanente = filasLeasing[i].ValorDeudaRemanente,
                            PlnPlazo = MapeoCredito.Data.CRE_PLAZO_TOTAL
                        });
                    }
                    break;

                case 2: // HIPOTECARIO — columnas confirmadas por relacion_columnas_lstProyeccion.pdf
                        // TODO CONFIRMAR: ¿siempre hay exactamente 1 SimulacionDto con el LstProyeccion completo?
                    var filasHipotecario = dataSimulacion.Data.ToList();
                    // TODO CONFIRMAR: ¿arranca en índice 0 o hay que saltar la primera fila como en leasing?
                    foreach (var item in filasHipotecario)
                    {
                        LstinsertPlanPagosDto.Add(new InsertPlanPagosDto()
                        {
                            PlnNroCredito = MapeoCredito.Data.CRE_NRO_CREDITO,
                            PlnCuota = item.Cuota,
                            PlnFechaPago = item.FechaPago,
                            PlnSaldoIncial = (decimal)item.Saldo,
                            PlnCapital = (decimal)item.Capital,
                            PlnCapitalPorPagar = item.CapitalPorPagar,
                            PlnInteres = (decimal)item.Intereses,
                            PlnInteresPorPagar = item.InteresesPorPagar,
                            PlnSeguroVida = Math.Round((decimal)item.SeguroVida),
                            PlnSeguroHogar = Math.Round((decimal)item.SeguroHogar),
                            PlnAPagar = item.Cuota == MapeoCredito.Data.CRE_PLAZO_TOTAL
                                ? (decimal)(item.Capital + item.Intereses + item.SeguroVida + item.SeguroHogar)
                                : item.CuotaUnidadPagadora,
                            PlnValorDeudaRemanente = 0,
                            PlnPlazo = MapeoCredito.Data.CRE_PLAZO_TOTAL
                        });
                    }
                    break;

                default:
                    return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se encontro la linea de credito para generar plan de pagos" };
            }

            return new Response<List<InsertPlanPagosDto>> { Data = LstinsertPlanPagosDto };
        }
        /// <summary>
        /// Mapea el plan de pagos que llega ya armado en el request (request.PlanPago)
        /// al objeto InsertPlanPagosDto que se guarda en base de datos, sin pasar por
        /// PLNPAGOSLEASING/PLNPAGOSHIPOTECARIO (esos generan el plan desde cero; este
        /// método asume que el plan ya viene correcto y solo lo traduce de formato).
        /// </summary>
        private List<InsertPlanPagosDto> MapearPlanPagoDesdeRequest(List<PlanPagoDto> planPago, long nroCredito, int plazoTotal)
        {
            List<InsertPlanPagosDto> lista = new();

            foreach (var item in planPago)
            {
                lista.Add(new InsertPlanPagosDto()
                {
                    PlnNroCredito = nroCredito,
                    PlnCuota = item.Cuota,
                    PlnFechaPago = item.Fecha,
                    PlnSaldoIncial = item.Saldo,
                    PlnCapital = item.Capital,
                    PlnCapitalPorPagar = item.CapitalPorPagar,
                    PlnInteres = item.Interes,
                    PlnInteresPorPagar = item.InteresPorPagar,
                    PlnSeguroVida = Math.Round(item.SeguroVida, 0),
                    PlnSeguroHogar = Math.Round(item.SeguroHogar, 0),
                    PlnAPagar =  item.CuotaUnidadPagadora,
                    PlnValorDeudaRemanente = item.DeudaRemanente,
                    PlnPlazo = plazoTotal
                });
            }

            return lista;
        }
        public async Task<Response<object>> CrearProdutoClienteCartera(InsertProductoClienteDto request, CancellationToken cancellationToken)
        {
            //Consultamos si el cliente ya cuenta con un credito activo ESC_ID <> 4
            ProductoClienteDto response = this._mapper.Map<IEnumerable<ProductoClienteDto>>(await this._carteraRepository.ObtenerProductoCliente(request.TipoIdentificacion, request.NumeroIdentificacion, null)).FirstOrDefault();
            if (response != null && response.IdEstadoCredito != 4 && request.NumeroCredito == response.NumeroCredito)
            {
                return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "Error!! El usuario ya tiene un credito activo." };
            }
            else
            {
                List<InsertPlanPagosDto> LstinsertPlanPagosDto = new List<InsertPlanPagosDto>();
                //var planPago = await CrearPlanPagoDesdeSimulacion(request, cancellationToken);
                //Verify_PLNPagos
                //var VerificacionPlanPagos = request.ArchivoPlanPagos != null ? await ValidatePLNPagos(request) : await CrearProductoClientePlanPago(request, cancellationToken);
                 
                    if (request.PlanPago != null && request.PlanPago.Any())
                {
                    LstinsertPlanPagosDto = MapearPlanPagoDesdeRequest(
                        request.PlanPago,
                        request.NumeroCredito,
                        request.PlazoTotal);
                }
                if (LstinsertPlanPagosDto.Count > 0)
                {
                    //int numeroCuota = request.TipoCreditoModalidad == 1 ? 0 : 1; // 1 = Leasing, 2 = Hipotecario
                    //request.ValorCuota = VerificacionPlanPagos.Data
                    //    .FirstOrDefault(p => p.PlnNumeroCuota == numeroCuota)?.PlnAPagar ?? 0;

                    int posicionCuota = request.TipoCreditoModalidad == 1 ? 0 : 1; // 1 = Leasing, 2 = Hipotecario
                    request.ValorCuota = LstinsertPlanPagosDto[posicionCuota].PlnAPagar;
                    var tasaEA = request.TasaEA / 100;
                    request.TasaPER = (decimal)Math.Pow((1 + (double)(tasaEA)), 1.0 / 12.0) - 1;
                    //MapeoCreCredito
                    var MapeoCredito = await MapeoCreCredito(request);
                    if (MapeoCredito.IsSuccess)
                    {
                        #region CreacionCreditoCompleto
                        if (await this._carteraRepository.CrearProdutoClienteCartera(MapeoCredito.Data))
                        {
                            #region InsertPln
                            var PlanPagos = this._mapper.Map<List<InsertPlanPagosProductoCartera>>(LstinsertPlanPagosDto);
                            if (!await this._carteraRepository.CrearPlanPagosProductoClienteCartera(PlanPagos, request.TipoCreditoModalidad))
                            {
                                return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "Error al insertar Plan de Pagos." };
                            }
                            else
                            {
                                #region InsertCPG
                                var MapeoPagos = await MapeoCPG(MapeoCredito.Data, LstinsertPlanPagosDto);
                                if (MapeoPagos.IsSuccess)
                                {
                                    if (!await this._carteraRepository.CrearCpgProductoClienteCartera(MapeoPagos.Data))
                                    {
                                        return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "Error al insertar Pagos." };
                                    }
                                    else
                                    {
                                        #region ConsumoDesembolsoSeven
                                        DateTime ultimoDia = new DateTime(request.FechaDesembolso.Year, request.FechaDesembolso.Month, DateTime.DaysInMonth(request.FechaDesembolso.Year, request.FechaDesembolso.Month));

                                        RequestDesembolsoSeven requestDesembolsoSeven = new RequestDesembolsoSeven()
                                        {
                                            fechaDesembolso = ultimoDia,
                                            numeroCredito = MapeoCredito.Data.CRE_NRO_CREDITO.ToString(),
                                            numeroDocumentoCliente = MapeoCredito.Data.CLI_IDENTIFICACION,
                                            tipoCredito = MapeoCredito.Data.TCR_ID,
                                            valorDesembolso = MapeoCredito.Data.CRE_VALOR_DESEMBOLSO,
                                            idSolicitudCredito =  MapeoCredito.Data.CRE_ID
                                        };
                                        var responseComprobante = await CrearComprobanteDesemboldoCredito(requestDesembolsoSeven, cancellationToken);
                                        if (responseComprobante.IsSuccess && responseComprobante.Data is not null)
                                        {
                                            return new Response<object> { Data = null, IsSuccess = true, ReturnMessage = $"Creacion del credito realizada con exito. Comprobante de desembolso N.° {responseComprobante.Data}" };
                                        }

                                        return new Response<object> { Data = null, IsSuccess = true, ReturnMessage = "Creacion del credito realizada con exito." };
                                        #endregion

                                    }
                                }
                                else
                                {
                                    return new Response<object> { Data = null, IsSuccess = MapeoPagos.IsSuccess, ReturnMessage = MapeoPagos.ReturnMessage };
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "Error al insertar Credito" };
                        }
                        #endregion
                    }
                    else
                    {
                        return new Response<object> { Data = null, IsSuccess = MapeoCredito.IsSuccess, ReturnMessage = MapeoCredito.ReturnMessage };
                    }
                }
                else
                {
                    return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "Verifique al mapear plan de pago" };
                }
            }
        }
        // ═══════════════════════════════════════════════════════════════════════════
        // ObtenerDetalleMovContableAprobacionCredito
        // ═══════════════════════════════════════════════════════════════════════════
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetalleMovContableAprobacionCredito(
            IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
            RequestDesembolsoSeven desembolso,
            TipoComprobante tipo)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasContable = cuentasContables.ToList();
            var cuentasContablesFiltro = new List<EquivalenciaCuentasContablesSeven>();

            // ═══════════════════════════════════════════════════════
            // Whitelist explícito por REF_COLUMN_APLICA_PAGO según el
            // comprobante que se esté generando. Reemplaza al esquema
            // anterior de listas de exclusión (blacklist), que hacía que
            // Comprobante 1 y 2 trajeran siempre el mismo set de cuentas.
            // ═══════════════════════════════════════════════════════
            var columnasPorTipo = new Dictionary<TipoComprobante, string[]>
    {
        // Principal (Leasing C1 / Hipotecario C2) -> cuentas de capital
        { TipoComprobante.Principal, new[] { "desembolso", "CPG_ABONO_CAPITAL" } },
 
        // AprobadoLeasing (Leasing C2) -> "Constructoras Pagos Bienes Leasing Habitacional"
        { TipoComprobante.AprobadoLeasing, new[] { "CPG_PAGOS_BIENES_LEASING_HABITACIONAL" } },
 
        // Adicional (Leasing C3 / Hipotecario C3) -> cuentas "Aprobados" (reversión)
        { TipoComprobante.Adicional, new[] { "LeasingAprobados", "HipotecariosAprobados" } },
 
        // SegundoPagoHip (Hipotecario C1, Cod 4137) -> "Créditos Hipotecarios" / "Pagos Créditos Hipotecarios"
        { TipoComprobante.SegundoPagoHip, new[] { "CPG_CREDITO_HIP", "CPGG_PAGO_CRE_HOP" } },
    };

            cuentasContablesFiltro = cuentasContable
                .Where(x => columnasPorTipo[tipo].Contains(x.REF_COLUMN_APLICA_PAGO))
                .ToList();

            // ═══════════════════════════════════════════════════════
            // PASO 2: MAPEO DE VALORES PARA DESEMBOLSO
            // ═══════════════════════════════════════════════════════
            var mapeoPagos = new Dictionary<string, decimal>
    {
        { "desembolso",        desembolso.valorDesembolso },
        { "CPG_ABONO_CAPITAL", desembolso.valorDesembolso },
        { "CPG_CREDITO_HIP", desembolso.valorDesembolso },
        { "CPGG_PAGO_CRE_HOP", desembolso.valorDesembolso },
        { "CPG_PAGOS_BIENES_LEASING_HABITACIONAL", desembolso.valorDesembolso },
        { "HipotecariosAprobados", desembolso.valorDesembolso },
        { "LeasingAprobados", desembolso.valorDesembolso },
        { "CPG_APRABADO_LEASING", 0 },
        { "CPG_APROBADO_HIP", 0 },
    };

            var NombreTercero = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, desembolso.numeroDocumentoCliente);

            var area = await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras();
            var areaCodigo = area.Data.Where(x => x.Id == NombreTercero.Data.IdUnidadEjecutora).FirstOrDefault().AreaNegocio;

            // ═══════════════════════════════════════════════════════
            // PASO 3: GENERAR MOVIMIENTOS SOLO SI HAY VALOR
            // ═══════════════════════════════════════════════════════
            foreach (var item in cuentasContablesFiltro)
            {
                if (!mapeoPagos.TryGetValue(item.REF_COLUMN_APLICA_PAGO, out var valor))
                    continue;

                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                {
                    Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                    Dmc_desc = $"cliente: {(NombreTercero?.Data != null ? NombreTercero?.Data?.PrimerNombre + " " + NombreTercero?.Data?.SegundoNombre + " " + NombreTercero?.Data?.PrimerApellido + " " + NombreTercero?.Data?.SegundoApellido : "Desconocido")}",
                    Dmc_cant = 0,
                    Dmc_acti = 0,
                    Dmc_refe = desembolso.numeroCredito.ToString(),
                    Dmc_vadb = item.TIPO_OPERACION == "D" ? valor : 0,
                    Dmc_vacr = item.TIPO_OPERACION == "C" ? valor : 0,
                    Dmc_vaba = 0,
                    Ter_coda = desembolso.numeroDocumentoCliente,
                    Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                    Arb_coda = areaCodigo.ToString(),
                    Arb_codp = "099",
                    Arb_cods = "1001",
                    Ter_codm = "0"
                };

                items.Add(itemDetalle);
            }
            return items;
        }
        private enum TipoComprobante
        {
            Principal,          // Leasing C1 / Hipotecario C2 - REF_COLUMN_APLICA_PAGO: "desembolso" / "CPG_ABONO_CAPITAL"
            AprobadoLeasing,    // Leasing C2 - REF_COLUMN_APLICA_PAGO: "CPG_PAGOS_BIENES_LEASING_HABITACIONAL"
            Adicional,          // Leasing C3 / Hipotecario C3 - REF_COLUMN_APLICA_PAGO: "LeasingAprobados" / "HipotecariosAprobados"
            SegundoPagoHip      // Hipotecario C1 (Cod 9003, antes 4137) - REF_COLUMN_APLICA_PAGO: "CPG_CREDITO_HIP" / "CPGG_PAGO_CRE_HOP"
        }
        public async Task<Response<string>> CrearComprobanteDesemboldoCredito(
    RequestDesembolsoSeven desembolso, CancellationToken cancellationToken)
        {
            string comprobanteId1 = null;
            string comprobanteId2 = null;
            string comprobanteId3 = null;

            if (desembolso.tipoCredito == 1) // LEASING
            {
                // ═══════════════════════════════
                // COMPROBANTE 1 - Cod 9062 - Cuentas principales de desembolso
                // ═══════════════════════════════
                comprobanteId1 = await ProcesarComprobanteDesembolso(
                    desembolso,
                    codOperacion: 9062,
                    descripcion: $"Desembolso Credito leasing {desembolso.fechaDesembolso}",
                    tipo: TipoComprobante.Principal
                );

                // ═══════════════════════════════
                // COMPROBANTE 2 - Cod 9062 - Cuentas "LeasingAprobados"
                // ═══════════════════════════════
                comprobanteId2 = await ProcesarComprobanteDesembolso(
                    desembolso,
                    codOperacion: 9062,
                    descripcion: $"Desembolso Credito leasing {desembolso.fechaDesembolso}",
                    tipo: TipoComprobante.AprobadoLeasing
                );

                // ═══════════════════════════════
                // COMPROBANTE 3 - Cod 9062 - Adicional (se queda igual, funcionaba bien)
                // ═══════════════════════════════
                comprobanteId3 = await ProcesarComprobanteDesembolso(
                    desembolso,
                    codOperacion: 9062,
                    descripcion: $"Desembolso Credito leasing {desembolso.fechaDesembolso}",
                    tipo: TipoComprobante.Adicional
                );
            }
            else // HIPOTECARIO (tipoCredito == 2)
            {
                // ═══════════════════════════════
                // COMPROBANTE 1 - Cod 9003 (antes 4137) - Segundo pago (Créditos Hipotecarios / Pagos Créditos Hipotecarios)
                // ═══════════════════════════════
               
                var nombreTipoCredito = desembolso.tipoCredito == 5 ? "Compra Cartera" :"Hipotecario";
                comprobanteId1 = await ProcesarComprobanteDesembolso(
                    desembolso,
                    codOperacion: 9003,
                    descripcion: $"Desembolso Credito {nombreTipoCredito} {desembolso.fechaDesembolso}",
                    tipo: TipoComprobante.SegundoPagoHip
                );

                // ═══════════════════════════════
                // COMPROBANTE 2 - Cod 9005 - Capital (Crédito Hipotecario)
                // ═══════════════════════════════
                comprobanteId2 = await ProcesarComprobanteDesembolso(
                    desembolso,
                    codOperacion: 9005,
                    descripcion: $"Desembolso Credito  {nombreTipoCredito} {desembolso.fechaDesembolso}",
                    tipo: TipoComprobante.Principal
                );

                // ═══════════════════════════════
                // COMPROBANTE 3 - Cod 9005 - Reversión Créditos Hipotecarios Aprobados
                // ═══════════════════════════════
                comprobanteId3 = await ProcesarComprobanteDesembolso(
                    desembolso,
                    codOperacion: 9005,
                    descripcion: $"Desembolso Credito  {nombreTipoCredito} {desembolso.fechaDesembolso}",
                    tipo: TipoComprobante.Adicional
                );
            }
            bool exito = comprobanteId1 != null || comprobanteId2 != null || comprobanteId3 != null;
            if (exito)
            {
                await _carteraRepository.ActualizarNumeroComprobanteCpgAplicacionPago(desembolso.idSolicitudCredito, int.Parse(comprobanteId2));
            }

            return new Response<string>
            {
                IsSuccess = exito,
                ReturnMessage = exito
                    ? $"Comprobantes creados: {comprobanteId1} - {comprobanteId2}"
                    : "No se generaron comprobantes exitosamente",
                Data = comprobanteId1
            };
        }

        // ═══════════════════════════════════════════════════════════════
        // MÉTODO AUXILIAR — procesa un comprobante por cod de operación
        // ═══════════════════════════════════════════════════════════════
        private async Task<string> ProcesarComprobanteDesembolso(
            RequestDesembolsoSeven desembolso,
            int codOperacion,
            string descripcion, TipoComprobante tipo)
        {
            var equivalencias = await _carteraRepository
                .ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(codOperacion);

            var cuentasFiltradas = equivalencias;

        var detalleMovConta = await ObtenerDetalleMovContableAprobacionCredito(
    cuentasFiltradas, desembolso, tipo);

            if (!detalleMovConta.Any())
                return null;

            var dataCabecera = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont
            {
                Emp_codi = 294,
                Mco_nume = 0,
                Mco_fech = desembolso.fechaDesembolso,
                Mod_codi = 7,
                Arb_csuc = "1001",
                Top_codi = codOperacion,
                Mco_desc = descripcion,
                vDetalle = detalleMovConta.ToArray()
            };

            var requestDetallePago = new RequestDetallePago
            {
                Cpg_id = Guid.NewGuid(),
                Emp_codi = dataCabecera.Emp_codi,
                Top_codi = dataCabecera.Top_codi,
                Mco_fech = dataCabecera.Mco_fech,
                Mod_codi = dataCabecera.Mod_codi,
                Mco_desc = dataCabecera.Mco_desc,
                Mov_Contable_Seven = System.Text.Json.JsonSerializer.Serialize(dataCabecera.vDetalle),
                Respuesta_Seven = string.Empty,
                Fecha_Ejecucion = DateTime.Now
            };

            decimal totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
            decimal totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);

            var numeroComprobante = await GenerarComprobanteContabilidad(
                requestDetallePago, "CDE", totalDmcVacr, totalDmcCadb);

            requestDetallePago.Cc_Id = numeroComprobante.IsSuccess
                ? numeroComprobante.Data.Comprobante_Id
                : null;

            await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
            await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);

            return numeroComprobante.IsSuccess
                ? numeroComprobante.Data.Comprobante_Id.ToString()
                : null;
        }
        public async Task<Response<ComprobanteCarteraDto>> GenerarComprobanteCreditoSeven(RequestDetallePago detallepago, string proceso, decimal total_Debito, decimal total_Credito)
        {
            try
            {
                var tipoComprobanteId = (await _catalogosBusinessLogic.ObtenerTipoComprobanteCartera()).Data.FirstOrDefault(x => x.Codigo.Equals(proceso));
                ComprobanteCarteraDto ReqComprobante = new()
                {
                    Fecha_Comprobante = DateTime.Now,
                    Tipo_Comprobante = tipoComprobanteId.TipoComprobanteId,
                    Ciudad = 1,
                    Departamento = 5,
                    Concepto = 1,
                    Beneficiario = 1,
                    LstMovimientoComprobante = null,
                    Total_Comprobante_Credito = total_Credito,
                    Total_Comprobante_Debito = total_Debito,
                    estado = false
                };

                var crearComprobanteCartera = this._mapper.Map<ComprobanteCartera>(ReqComprobante);
                var response = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.CrearComprobanteCartera(crearComprobanteCartera));

                return new Response<ComprobanteCarteraDto>
                {
                    IsSuccess = true,
                    ReturnMessage = "Comprobante Creado desde crédito: " + +response.Comprobante_Id,
                    Data = response
                };

            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "Error al generar comprobante desde crédito." };
            }
        }
        private async Task<Response<List<InsertPlanPagosDto>>> ValidatePLNPagos(InsertProductoClienteDto request)
        {
            List<InsertPlanPagosDto> LstInsertPlanPagosDto = new();
            try
            {
                const int ColumnasArchivoPlnPagos = 14;
                using (var stream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    request.ArchivoPlanPagos.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        if (ColumnasArchivoPlnPagos != colCount)
                        {
                            return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "El número de columnas supera las permitidas, columnas permitidas: 14" };
                        }
                        else
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                //Retornamos problemas en [row,column]
                                InsertPlanPagosDto insertPlanPagos = new InsertPlanPagosDto();
                                #region PlnNroCredito
                                int Column = 1;
                                if (!await ValidateCell<long>(worksheet, row, Column))
                                {
                                    return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "Error en la Fila: " + row + ", Columna: " + Column };
                                }
                                else
                                {
                                    if (worksheet.Cells[row, Column].GetCellValue<long>() == request.NumeroCredito)
                                    {
                                        insertPlanPagos.PlnNroCredito = worksheet.Cells[row, Column].GetCellValue<long>();
                                    }
                                    else
                                    {
                                        return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "El número del plan de pagos no corresponde al digitado en la creación de credito." };
                                    }

                                }
                                #endregion
                                #region PlnCuota
                                Column = 2;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "Error en la Fila: " + row + ", Columna: " + Column };
                                }
                                else
                                {
                                    insertPlanPagos.PlnCuota = int.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnFechaPago
                                Column = 3;
                                if (!await ValidateCell<DateTime>(worksheet, row, Column))
                                {
                                    return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "Error en la Fila: " + row + ", Columna: " + Column };
                                }
                                else
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                #endregion
                                #region PlnSaldoIncial
                                Column = 4;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnSaldoIncial = Decimal.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnCapital
                                Column = 5;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnCapital = Decimal.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnCapitalPorPagar
                                Column = 6;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {

                                    insertPlanPagos.PlnCapitalPorPagar = Decimal.Parse(request.TipoCreditoModalidad == 1 ? worksheet.Cells[row, Column].GetCellValue<double>().ToString() : "0");
                                }
                                #endregion
                                #region PlnInteres
                                Column = 7;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnInteres = Decimal.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnInteresPorPagar
                                Column = 8;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnInteresPorPagar = Decimal.Parse(request.TipoCreditoModalidad == 1 ? worksheet.Cells[row, Column].GetCellValue<double>().ToString() : "0");
                                }
                                #endregion
                                #region PlnSeguroVida
                                Column = 9;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnSeguroVida = Decimal.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnSeguroHogar
                                Column = 10;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnSeguroHogar = Decimal.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnAPagar
                                Column = 11;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnAPagar = Decimal.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnPlazo
                                Column = 12;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnPlazo = int.Parse(worksheet.Cells[row, Column].GetCellValue<double>().ToString());
                                }
                                #endregion
                                #region PlnValorDeudaRemanente
                                Column = 13;
                                if (!await ValidateCell<double>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnFechaPago = worksheet.Cells[row, Column].GetCellValue<DateTime>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnValorDeudaRemanente = Decimal.Parse(request.TipoCreditoModalidad == 1 ? worksheet.Cells[row, Column].GetCellValue<double>().ToString() : "0");
                                }
                                #endregion
                                #region PlnF14
                                Column = 14;
                                if (!await ValidateCell<string>(worksheet, row, Column))
                                {
                                    insertPlanPagos.PlnF14 = worksheet.Cells[row, Column].GetCellValue<string>();
                                }
                                else
                                {
                                    insertPlanPagos.PlnF14 = null;
                                }
                                #endregion
                                LstInsertPlanPagosDto.Add(insertPlanPagos);
                            }
                            return new Response<List<InsertPlanPagosDto>> { Data = LstInsertPlanPagosDto, IsSuccess = true, ReturnMessage = "" };
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "Ocurrio un error procesando el Plan de pagos" };
            }

        }
        private async Task<Response<CrearProductoCliente>> MapeoCreCredito(InsertProductoClienteDto request)
        {
            try
            {
                var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.TipoIdentificacion, request.NumeroIdentificacion)).Data;
                var ClaseInmueble = (await _catalogosBusinessLogic.ObtenerValoresCatalogosPorIdCatalogo(43)).Data;
                var Categorias = (await _catalogosBusinessLogic.ObtenerCategorias()).Data;
                var Grados = (await _catalogosBusinessLogic.ObtenerGrados()).Data;
                #region MapeoCredito
                var tasaPeriodicaNMV = Math.Pow((1 + (double)(request.TasaEA / 100)), 1 / 12) - 1;
                tasaPeriodicaNMV = Math.Round(tasaPeriodicaNMV, 10);

                CrearProductoCliente P = new CrearProductoCliente()
                {
                    CRE_ID = Guid.NewGuid(),
                    CRE_FECHA_DESEMBOLSO = request.FechaSolicitud,
                    CRE_VALOR_DESEMBOLSO = request.ValorDesembolso,
                    CRE_VALOR_CUOTA = request.ValorCuota,
                    CRE_PLAZO_TOTAL = request.PlazoTotal,
                    TCR_ID = request.TipoCreditoModalidad,
                    CRE_DIA_PAGO = 30,
                    ESC_ID = 1,
                    CRE_FECHA_ESTADO = request.FechaSolicitud,
                    CRE_DIAS_MORA = 0,
                    CRE_VALOR_VIVIENDA = request.ValorViviendaSeguroHogar,
                    CRE_VALOR_VIVIENDA_SEGURO_HOGAR = request.valorInmueblePlanPagos,
                    TIV_ID = request.ViviendaVis,
                    TVV_ID = request.EstadoVivienda,
                    CRE_SALDO_CAPITAL = request.ValorDesembolso,
                    CRE_SALDO_CAPITAL_MORA = 0,
                    CRE_PLAZO_ACTUAL = request.PlazoTotal,
                    CRE_TASA_SEGURO_VIDA = request.TasaSeguroVida,
                    CRE_TASA_SEGURO_HOGAR = request.TasaSeguroHogar,
                    CRE_TASA_CREDITO_EA = request.TasaEA,
                    CRE_TASA_CREDITO_PER = Math.Round((Math.Pow(1 + (double)(request.TasaEA / 100), 1.0 / 12.0) - 1) * 100, 10),
                    CRE_TASA_FRECH = 0,
                    CRE_VALOR_ALIVIO_CUOTA = 0,
                    CRE_FECHA_ULTIMO_PAGO = request.FechaSolicitud,
                    CRE_FECHA_PROXIMO_PAGO = request.FechaSolicitud.AddMonths(1),
                    CRE_TIPO_ABONO_EXTRA = request.TipoAbonoExtra,
                    CRE_VALOR_ABONO_EXTRA = request.ValorAbonoExtra,
                    CRE_VALOR_DEUDA_REMANENTE = 0,
                    CRE_VALOR_CANON_INICIAL = request.TipoCreditoModalidad == 1 ? (request.valorInmueblePlanPagos * (request.PorCanonInicial / 100)) : 0,
                    CRE_VALOR_OPCION_COMPRA = request.TipoCreditoModalidad == 1 ? (request.valorInmueblePlanPagos * (request.PorOpcionCompra / 100)) : 0,
                    CRE_POR_CANON_INICIAL = request.PorCanonInicial,
                    CRE_POR_OPCION_COMPRA = request.PorOpcionCompra,
                    CRE_ACUMULADO_INTERES_MORA = 0,
                    CRE_FECHA_ULTIMO_PAGO_INTERES_MORA = request.FechaSolicitud,
                    CRE_ACUMULADO_INTERES_CORRIENTE = 0,
                    CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE = request.FechaSolicitud,
                    CRE_ACUMULADO_SEGURO_HOGAR = 0,
                    CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR = request.FechaSolicitud,
                    CRE_ACUMULADO_SEGURO_VIDA = 0,
                    CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA = request.FechaSolicitud,
                    CRE_ALIVIO_FRECH = request.AlivioFrech ? 1 : 0,
                    CRE_NUMERO_CUOTAS_CANCELADAS = 0,
                    CRE_CREADO_POR = request.CreadoPor,
                    CRE_FECHA_CREACION = DateTime.Now,
                    CRE_MODIFICADO_POR = request.CreadoPor,
                    CRE_FECHA_MODIFICACION = DateTime.Now,
                    CRE_NRO_CREDITO = request.NumeroCredito,
                    CLI_IDENTIFICACION = request.NumeroIdentificacion,
                    TIPO_IDENTIFICACION = request.TipoIdentificacion,
                    CRE_DIRECCION_INMUEBLE = request.DireccionInmueble,
                    NUMERO_MATRICULA = request.NumeroMatricula,
                    ESCRITURA_INMUEBLE = request.EscrituraInmueble,
                    FECHA_ESCRITURA = request.FechaEscritura,
                    NOTARIA = request.Notaria,
                    SIJ_LINDEROS = request.Linderos,
                    DESCRIPCION_INMUEBLE = request.DescripcionInmueble,
                    SOC_SCORE = request.Score,
                    FECHA_SOLICITUD = request.FechaDesembolso,
                    CRE_FECHA_FINALIZA_PAGOS = request.FechaDesembolso.AddMonths(request.PlazoTotal),
                    CRE_CANON_EXTRAORDINARIO = 0,
                    CRE_FECHA_CAUSACION = request.FechaDesembolso,
                    CRE_PROVISION = 0,
                    CRE_PROVISION_PORCENTAJE = 1,
                    CRE_CALIFICACION = request.Calificacion,
                    CRE_SALDO_CAPITAL_TRADICIONAL = request.TipoCreditoModalidad == 1 ? request.ValorDesembolso : 0,
                    CRE_SALDO_INICIAL_REDUCIDO = 0,
                    CRE_VALOR_CUOTA_TRADICIONAL = 0,
                    CRE_ACUMULADO_CAPITAL = 0,
                    CRE_PROVISION_ADICIONAL = 0,
                    TCR_ID_ESTADO_CREDITO = 1,
                    CLC_ID = request.LineaCredito,
                    CLASE_INMUEBLE = ClaseInmueble.Where(z => z.IdValor == request.ClaseInmueble).FirstOrDefault().Descripcion,
                    CIUDAD_INMUEBLE = request.CiudadInmueble,
                    CATEGORIA = Categorias.Where(x => x.Id == cliente.IdCategoria).FirstOrDefault().Descripcion,
                    GRADO = Grados.Where(x => x.Id == cliente.IdGrado).FirstOrDefault().Descripcion,
                    UEJ_ID = cliente.IdUnidadEjecutora,
                    CLI_EDAD = DateTime.Now < cliente.FechaNacimiento.AddYears(DateTime.Now.Year - cliente.FechaNacimiento.Year) ? (DateTime.Now.Year - cliente.FechaNacimiento.Year) - 1 : (DateTime.Now.Year - cliente.FechaNacimiento.Year),
                    RECURSOS_FUTUROS = request.TipoCreditoModalidad == 1 ? request.RecursosFuturos : 0,
                    CRE_PORCENTAJE_SEGURO_VIDA = request.PorcentajeSeguroVida,
                    CRE_PORCENTAJE_EXTRA_PRIMA_SEGURO_VIDA = request.PorcentajeExtraPrimaSeguroVida,
                    CRE_ASEGURADORA_SOLIDARIA = request.AseguradoraVida
                };
                #endregion
                return new Response<CrearProductoCliente> { Data = P, IsSuccess = true, ReturnMessage = "" };
            }
            catch (Exception)
            {
                return new Response<CrearProductoCliente> { Data = null, IsSuccess = false, ReturnMessage = "Ocurrio un error al asignar el credito" };
            }

        }
        private async Task<Response<AplicacionPago>> MapeoCPG(CrearProductoCliente request, List<InsertPlanPagosDto> LstPlnPagos)
        {
            try
            {
                AplicacionPago aplicacionPago = new AplicacionPago()
                {
                    CPG_ID = Guid.NewGuid(),
                    CRE_ID = request.CRE_ID,
                    CPG_FECHA_PAGO = request.CRE_FECHA_ULTIMO_PAGO,
                    CPG_FECHA_APLICACION = request.CRE_FECHA_ULTIMO_PAGO,
                    CPG_ABONO_SEGURO_HOGAR = 0,
                    CPG_ABONO_SEGURO_VIDA = 0,
                    CPG_ABONO_INTERES_CORRIENTE = 0,
                    CPG_ABONO_INTERES_MORA = 0,
                    CPG_ABONO_CAPITAL = request.CRE_VALOR_DESEMBOLSO,
                    CPG_VALOR_PAGO = request.CRE_VALOR_DESEMBOLSO,
                    DCT_ID = 0,
                    CPG_CREADO_POR = request.CRE_CREADO_POR,
                    CPG_FECHA_CREACION = DateTime.Now,
                    CPG_MODIFICADO_POR = request.CRE_CREADO_POR,
                    CPG_FECHA_MODIFICACION = DateTime.Now,
                    TPP_ID = 3,
                    CPG_ABONO_CAPITAL_REMANENTE = 0,
                    CPG_ABONO_INTERES_REMANENTE = 0,
                    CPG_ABONO_CAPITAL_MORA = 0
                };
                return new Response<AplicacionPago> { Data = aplicacionPago, IsSuccess = true, ReturnMessage = "" };
            }
            catch (Exception)
            {
                return new Response<AplicacionPago> { Data = null, IsSuccess = true, ReturnMessage = "Ocurrio un error en el llenado de pagos." };
            }
        }
        private async Task<bool> ValidateCell<T>(ExcelWorksheet worksheet, int row, int column)
        {
            string CellValue = worksheet.Cells[row, column].GetValue<string>();
            //Realizamos la verificación por tipo de dato
            if (typeof(T) == typeof(long))
            {
                long result;
                if (long.TryParse(CellValue, out result))
                {
                    if (result <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (typeof(T) == typeof(decimal))
            {
                decimal result;
                if (decimal.TryParse(CellValue, out result))
                {
                    if (result <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (typeof(T) == typeof(DateTime))
            {
                DateTime result;
                if (!DateTime.TryParse(CellValue, out result))
                {
                    return false;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                int result;
                if (int.TryParse(CellValue, out result))
                {
                    if (result <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region ComprobanteCartera-MovimientosComprobanteCartera
        public async Task<Response<ComprobanteCarteraDto>> CrearComprobanteCartera(ComprobanteCarteraDto request, CancellationToken cancellationToken)
        {
            var crearComprobanteCartera = this._mapper.Map<ComprobanteCartera>(request);
            var response = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.CrearComprobanteCartera(crearComprobanteCartera));

            return new Response<ComprobanteCarteraDto> { Data = response };
        }
        public async Task<Response<MovimientoComprobanteDto>> CrearMovimientoComprobanteCartera(MovimientoComprobanteDto Request, CancellationToken cancellationToken)
        {
            var CrearMovimientoComprobante = this._mapper.Map<MovimientoComprobanteEntity>(Request);
            var response = this._mapper.Map<MovimientoComprobanteDto>(await _carteraRepository.InsertarMovimientoComprobanteCartera(CrearMovimientoComprobante));
            return new Response<MovimientoComprobanteDto> { Data = response };
        }

        public async Task<Response<ComprobanteCarteraDto>> ObtenerComprobanteCartera(ObtenerComprobanteDto Request, CancellationToken cancellationToken)
        {
            IEnumerable<MovimientoComprobanteDto> movimientoComprobanteDto;
            if (Request.ComprobanteId != null && Request.ComprobanteId != 0)
            {
                ComprobanteCarteraDto responseComprobante = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.ObtenerComprobanteCarteraByComprobanteId(Request.ComprobanteId, 1));
                if (responseComprobante != null)
                {
                    movimientoComprobanteDto = (await ObtenerMovimientosComprobante(responseComprobante.Comprobante_Id)).Data;
                    responseComprobante.LstMovimientoComprobante = movimientoComprobanteDto;
                    return new Response<ComprobanteCarteraDto> { Data = responseComprobante };
                }
                else
                {
                    return new Response<ComprobanteCarteraDto> { Data = null, IsSuccess = false, ReturnMessage = "No se encontro el comprobante" };
                }
            }
            else if (Request.ComprobanteSevenId != null && Request.ComprobanteSevenId != 0)
            {
                var responseComprobante = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.ObtenerComprobanteCarteraByComprobanteId(Request.ComprobanteSevenId, 2));
                if (responseComprobante != null)
                {
                    movimientoComprobanteDto = (await ObtenerMovimientosComprobante(responseComprobante.Comprobante_Id)).Data;
                    responseComprobante.LstMovimientoComprobante = movimientoComprobanteDto;
                    return new Response<ComprobanteCarteraDto> { Data = responseComprobante };
                }
                else
                {
                    return new Response<ComprobanteCarteraDto> { Data = null, IsSuccess = false, ReturnMessage = "No se encontro el comprobante" };
                }
            }
            else
            {
                return new Response<ComprobanteCarteraDto> { Data = null, IsSuccess = false, ReturnMessage = "No se envio ningun ID con el cual consultar" };
            }
        }
        public async Task<Response<List<ComprobanteCarteraPorProcesoDTO>>> ObtenerComprobanteCarteraPorProceso(ObtenerComprobantePorProcesoDto Request, CancellationToken cancellationToken)
        {
            List<int> codigosOperacion = new();
            List<ComprobanteCarteraPorProcesoDTO> listaIerar = new();
            ReporteContableParametersDto parametros = new ReporteContableParametersDto();
            List<ComprobanteCarteraPorProcesoDTO> listaComprobantes = new();

            listaComprobantes = (await _carteraRepository.ObtenerComprobanteCartera(Request, 1)).ToList();
            listaIerar.AddRange(listaComprobantes.GroupBy(x => x.Comprobante_Id).Select(x => x.FirstOrDefault()).ToList());

            if (Request.CodigoProceso == "CDE" || Request.CodigoProceso == "CAC")
            {
                foreach (var item in listaIerar)
                {
                    var request = new MovimientosContableParametersDto
                    {
                        IdComprobante = (Guid)item.Detalle_Movimiento_Id,
                        NumeroPagina = 500,
                        RegistrosPorPagina = 1
                    };
                    var detallesIterar = this._mapper.Map<List<MovimientoComprobantePorProcesoDto>>(await _carteraRepository.ObtenerMovimientosContableCartera(request));
                    if (detallesIterar.Count > 0)
                    {
                        bool isUniqueIdentification = detallesIterar
                            .Select(x => x.Numero_Identificacion)
                            .Distinct()
                            .Count() <= 1;
                        if (isUniqueIdentification)
                        {

                            item.Identificacion = detallesIterar.FirstOrDefault().Numero_Identificacion;
                        }
                    }

                }
            }
            if (listaIerar?.Count() > 0)
                return new Response<List<ComprobanteCarteraPorProcesoDTO>> { Data = listaIerar };
            else
                return new Response<List<ComprobanteCarteraPorProcesoDTO>> { Data = listaIerar, IsSuccess = true, ReturnMessage = "No se encontraron Comprobantes contables" };

        }
        /// <summary>
        /// Convierte TSCnMcont del servicio antiguo al nuevo
        /// </summary>
        public static ServiceSeven.TSCnMcont MapToServiceSeven(ReferenciasSvc.Seven.SCnMcont.TSCnMcont antiguoMovimiento)
        {
            if (antiguoMovimiento == null) return null;

            var nuevoMovimiento = new ServiceSeven.TSCnMcont
            {
                Emp_codi = antiguoMovimiento.Emp_codi,
                Top_codi = antiguoMovimiento.Top_codi,
                Mco_nume = antiguoMovimiento.Mco_nume,
                Mco_fech = antiguoMovimiento.Mco_fech,
                Mod_codi = antiguoMovimiento.Mod_codi,
                Arb_csuc = antiguoMovimiento.Arb_csuc,
                Mco_desc = antiguoMovimiento.Mco_desc
            };

            // Mapear detalles
            if (antiguoMovimiento.vDetalle != null && antiguoMovimiento.vDetalle.Length > 0)
            {
                nuevoMovimiento.vDetalle = new ServiceSeven.TSCnDmcon[antiguoMovimiento.vDetalle.Length];

                for (int i = 0; i < antiguoMovimiento.vDetalle.Length; i++)
                {
                    var detalleAntiguo = antiguoMovimiento.vDetalle[i];
                    nuevoMovimiento.vDetalle[i] = new ServiceSeven.TSCnDmcon
                    {
                        Cue_codi = detalleAntiguo.Cue_codi,
                        Dmc_desc = detalleAntiguo.Dmc_desc,
                        Dmc_cant = detalleAntiguo.Dmc_cant,
                        Dmc_acti = detalleAntiguo.Dmc_acti,
                        Dmc_refe = detalleAntiguo.Dmc_refe,
                        Dmc_vadb = detalleAntiguo.Dmc_vadb,
                        Dmc_vacr = detalleAntiguo.Dmc_vacr,
                        Dmc_vaba = detalleAntiguo.Dmc_vaba,
                        Ter_coda = detalleAntiguo.Ter_coda,
                        Arb_codc = detalleAntiguo.Arb_codc,
                        Arb_coda = detalleAntiguo.Arb_coda,
                        Arb_codp = detalleAntiguo.Arb_codp,
                        Arb_cods = detalleAntiguo.Arb_cods,
                        Ter_codm = detalleAntiguo.Ter_codm
                    };
                }
            }

            return nuevoMovimiento;
        }

        public async Task<Response<List<CausacionDiariaInteresesCorrientesDto>>> ConstruirMovimientosContables(RequestCausacionDiariaInteresesCorrientesDto Request, CancellationToken cancellationToken)
        {
            List<CausacionDiariaInteresesCorrientesDto> causacionDiariaInteresesCorrientesDto = new();
            RequestCierresCausacionesRealizadasDto requestCierresCausacionesRealizadasDto = new();
            requestCierresCausacionesRealizadasDto.FechaCierreInicial = Request.FechaCierre;
            requestCierresCausacionesRealizadasDto.FechaCierreFinal = Request.FechaCierre;
            requestCierresCausacionesRealizadasDto.TiposCierre = Request.TipoCierre.HasValue ? new List<int> { Request.TipoCierre.Value } : new List<int>();

            var resultado = await this._creditoBusinessLogic.ObtenerCierresCausacionesRealizadas(requestCierresCausacionesRealizadasDto);

            //var itemsCierres = resultado.Data.Where(array => array.Contains(searchString) == ).ToList();


            var itemsCierres = resultado.Data.ToList();

            if (itemsCierres.Count() != 0)
            {
                ObtenerComprobantePorProcesoDto obtenerComprobantePorProcesoDto = new();

                if (Request.TipoCierre == 3)
                {
                    obtenerComprobantePorProcesoDto.FechaInicio = Request.FechaCierre.ToString();
                    obtenerComprobantePorProcesoDto.FechaFinal = Request.FechaCierre.ToString();
                    obtenerComprobantePorProcesoDto.CodigoProceso = "CCIC";

                    var listaComprobantes = await _carteraRepository.ObtenerComprobanteCartera(obtenerComprobantePorProcesoDto, 1);

                    if (listaComprobantes?.Count() == 0)
                    {
                        Request.Actualiza = true;
                        var resultadoData = await this._creditoBusinessLogic.RecuperacionCausacionDiariaInteresesCorrientes(Request);
                        causacionDiariaInteresesCorrientesDto = resultadoData.Data?.ToList();

                    }
                }
                if (Request.TipoCierre == 1)
                {
                    obtenerComprobantePorProcesoDto.FechaInicio = Request.FechaCierre.ToString();
                    obtenerComprobantePorProcesoDto.FechaFinal = Request.FechaCierre.ToString();
                    obtenerComprobantePorProcesoDto.CodigoProceso = "CIM";

                    var listaComprobantes = await _carteraRepository.ObtenerComprobanteCartera(obtenerComprobantePorProcesoDto, 1);

                    if (listaComprobantes?.Count() == 0)
                    {
                        Request.Actualiza = true;
                        var resultadoData = await this._creditoBusinessLogic.RecuperacionCausacionDiariaInteresesMoratorios(Request);
                        causacionDiariaInteresesCorrientesDto = resultadoData.Data?.ToList();
                    }


                }

            }



            if (causacionDiariaInteresesCorrientesDto?.Count() > 0)
                return new Response<List<CausacionDiariaInteresesCorrientesDto>> { Data = causacionDiariaInteresesCorrientesDto, IsSuccess = true, ReturnMessage = "Construccion Exitosa" };
            else
                return new Response<List<CausacionDiariaInteresesCorrientesDto>> { Data = null, IsSuccess = true, ReturnMessage = "No se realizo la recontruccion el cierre ya existe" };





        }

        public async Task<Response<List<ComprobanteCarteraPorProcesoDTO>>> GenerarComprobanteSevenPorProceso(GenerarComprobanteSevenPorProcesoDto Request, CancellationToken cancellationToken)
        {
            List<Response<ComprobanteCarteraPorProcesoDTO>> responseList = new List<Response<ComprobanteCarteraPorProcesoDTO>>();

            if (Request.Comprobantes.Count == 0)
            {
                return new Response<List<ComprobanteCarteraPorProcesoDTO>>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "No hay comprobantes para el proceso contable"
                };
            }

            if (string.IsNullOrEmpty(Request.CodigoProceso))
            {
                return new Response<List<ComprobanteCarteraPorProcesoDTO>>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "Por favor valide el código del proceso para los comprobantes contables"
                };
            }

            Response<InsertarMovContableResponse> response = new();
            Response<ComprobanteCarteraPorProcesoDTO> responseComprobante = new();
            List<ComprobanteCarteraPorProcesoDTO> comprobantesActualizados = new List<ComprobanteCarteraPorProcesoDTO>();

            var dataMovConta = new ReferenciasSvc.Seven.SCnMcont.TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovCUentaContable = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalles = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            List<RequestDetallePago> listaMovimientosComprobante = new();
            ComprobanteCarteraUpdate comprobanteCarteraUpdate = new();
            List<ComprobanteCarteraPorProcesoDTO> dataComprobantes = new();
            List<string> listaCPG_ID = new List<string>();
            HashSet<string> uniqueCPG_IDs = new HashSet<string>();
            List<CpgDmcInfo> listaCpgDmcInfo = new List<CpgDmcInfo>();

            // Obtención de datos necesarios
            foreach (var item in Request.Comprobantes)
            {
                dataComprobantes.AddRange(await _carteraRepository.ObtenerComprobanteCarteraPorId(item.Comprobante_Id, Request.CodigoProceso));
                var movimientos = await _carteraRepository.ObtenerDetallesPagosSevenCartera(item.Comprobante_Id);
                listaMovimientosComprobante.AddRange(movimientos);
            }

            await ConstruirDatosSevenAsync();

            // Retorno final
            return new Response<List<ComprobanteCarteraPorProcesoDTO>>
            {
                Data = comprobantesActualizados,
                IsSuccess = comprobantesActualizados.Any(),
                ReturnMessage = response.ReturnMessage
            };
            async Task ConstruirDatosSevenAsync()
            {
                var tipoOperacionAgrupado = listaMovimientosComprobante
                    .GroupBy(x => x.Top_codi)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                foreach (var operacion in tipoOperacionAgrupado)
                {
                    detalles.Clear();
                    detalleMovCUentaContable.Clear();
                    dataMovConta.Emp_codi = operacion.Emp_codi;
                    dataMovConta.Top_codi = operacion.Top_codi;
                    dataMovConta.Mco_nume = 0;
                    dataMovConta.Mco_fech = operacion.Mco_fech;
                    dataMovConta.Mod_codi = operacion.Mod_codi;
                    dataMovConta.Arb_csuc = "1001";

                    string cleanedDesc = LimpiarDescripcion(operacion.Mco_desc);
                    dataMovConta.Mco_desc = cleanedDesc;

                    var movimientosOperacion = listaMovimientosComprobante
                        .Where(x => x.Top_codi == operacion.Top_codi)
                        .ToList();

                    bool contieneCuentaCxC = false;


                    foreach (var item in movimientosOperacion)
                    {
                        // Completar campos faltantes en el JSON antes de deserializar
                        var jsonArray = JArray.Parse(item.Mov_Contable_Seven);
                        foreach (JObject obj in jsonArray)
                        {
                            if (!obj.ContainsKey("Dmc_cant")) obj.Add("Dmc_cant", 1);
                            if (!obj.ContainsKey("Dmc_acti")) obj.Add("Dmc_acti", 0);
                            if (!obj.ContainsKey("Dmc_vaba")) obj.Add("Dmc_vaba", 0);
                        }

                        var vDetalle = JsonConvert.DeserializeObject<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon[]>(jsonArray.ToString());
                        contieneCuentaCxC = vDetalle.Any(detalle => detalle.Cue_codi == "1687950301" || detalle.Cue_codi == "25959519"
                        || detalle.Cue_codi == "16080202");

                        if (contieneCuentaCxC)
                        {
                            if (movimientosOperacion.Count() != 1)
                            {
                                response.Data = null;
                                response.IsSuccess = false;
                                response.ReturnMessage = "Solo se puede enviar comprobantes unificados cuando no contienen cuentas por cobrar.";
                                return;
                            }
                        }

                        foreach (var detalle in vDetalle)
                        {
                            var dataIdentificacion = detalle.Ter_coda.Split(";");
                            detalle.Ter_coda = dataIdentificacion[0] ?? string.Empty;
                            detalle.Dmc_desc = dataMovConta.Mco_desc;
                        }

                        if (Request.CodigoProceso == "CAC" || Request.CodigoProceso == "CPLU")
                        {
                            var vDetalleCpg = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(item.Mov_Contable_Seven);

                            foreach (var detalle in vDetalleCpg)
                            {
                                if (detalle.TryGetValue("Cpg_Id", out var cpgId) && !string.IsNullOrEmpty(cpgId))
                                {
                                    listaCPG_ID.Add(cpgId);
                                    uniqueCPG_IDs.Add(cpgId);

                                    if (detalle.TryGetValue("Dmc_refe", out var dmcRefe) && !string.IsNullOrEmpty(dmcRefe))
                                    {
                                        listaCpgDmcInfo.Add(new CpgDmcInfo
                                        {
                                            CpgId = cpgId,
                                            DmcRefe = long.Parse(dmcRefe)
                                        });
                                    }
                                }
                            }
                        }

                        detalles.AddRange(vDetalle);
                    }

                    var cuentaContableAgrupado = detalles.GroupBy(x => x.Cue_codi).ToList();

                    foreach (var item in cuentaContableAgrupado)
                    {
                        // Sumar los valores de débito y crédito sin redondeo
                        decimal debitoTotal = item.Sum(c => c.Dmc_vadb);
                        decimal creditoTotal = item.Sum(c => c.Dmc_vacr);

                        // Redondear los valores a dos decimales
                        decimal debito = Math.Round(debitoTotal, 2, MidpointRounding.AwayFromZero);
                        decimal credito = Math.Round(creditoTotal, 2, MidpointRounding.AwayFromZero);

                        // Verificar si ambos son diferentes de cero
                        if (debito != 0 && credito != 0)
                        {
                            decimal diferenciaDC = Math.Abs(debito - credito);
                            if (debito > credito)
                            {
                                debito = diferenciaDC;
                                credito = 0;
                            }
                            else
                            {
                                credito = diferenciaDC;
                                debito = 0;
                            }
                        }

                        var detalleCuentaContable = item.FirstOrDefault();

                        detalleCuentaContable.Ter_coda = contieneCuentaCxC ? detalleCuentaContable.Ter_coda : "000201508";
                        detalleCuentaContable.Dmc_vadb = debito;
                        detalleCuentaContable.Dmc_vacr = credito;

                        detalleMovCUentaContable.Add(detalleCuentaContable);
                    }


                    // 🔹 Sumar los valores finales de débitos y créditos
                    decimal totalDebitos = detalleMovCUentaContable.Sum(x => x.Dmc_vadb);
                    decimal totalCreditos = detalleMovCUentaContable.Sum(x => x.Dmc_vacr);

                    // 🔹 Calcular la diferencia
                    decimal diferencia = totalDebitos - totalCreditos;

                    // 🔹 Verificar si hay descuadre
                    if (Math.Abs(diferencia) <= 0.05m)  // Permitir solo una diferencia mínima de redondeo
                    {

                        // 🔹 Buscar el último registro con Cue_codi = 41962601
                        var registroAjuste = detalleMovCUentaContable.LastOrDefault(x => x.Cue_codi == "41962601");

                        if (registroAjuste != null)
                        {
                            registroAjuste.Dmc_vacr += diferencia; // Ajusta crédito si el débito es mayor
                        }

                    }

                    dataMovConta.vDetalle = detalleMovCUentaContable.ToArray();
                    await EnviarConsumoContableSevenAsync(dataMovConta);
                }
            }

            async Task EnviarConsumoContableSevenAsync(ReferenciasSvc.Seven.SCnMcont.TSCnMcont dataMovContable)
            {

                // Modificar la fecha antes de pasar dataMovContable a bodyLeasing
                dataMovContable.Mco_fech = DateTime.Now;


                var bodyLeasing = new InsertarMovContableRequestBody(dataMovContable);
                movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                var movContableRequestNew = MapToServiceSeven(bodyLeasing.pContable);

                var newResponse = await _IMovimientoContableService.InsertarMovContableAsync(movContableRequestNew, cancellationToken);
                // Adaptar la respuesta al formato esperado
                if (newResponse.IsSuccess)
                {
                    // Simular la estructura del servicio antiguo
                    response.Data = new InsertarMovContableResponse(
                        new InsertarMovContableResponseBody(newResponse.Data) // XML de SEVEN
                    );
                    response.ReturnMessage = "Resultado Mov. Contable Seven Créditos leasing: " + newResponse.Data;
                    response.IsSuccess = true;
                }
                else
                {
                    // Error
                    response.Data = null;
                    response.ReturnMessage = newResponse.ReturnMessage; // Mensaje de error ya formateado
                    response.IsSuccess = false;
                }
                //response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                //response.ReturnMessage = response.Data is not null
                //    ? "Resultado Mov. Contable Seven Créditos leasing: " + response.Data.Body.InsertarMovContableResult
                //    : "Error inesperado al crear el mov. contable en seven.";

                //responseComprobante.ReturnMessage = response.ReturnMessage;
                await ActualizarComprobantesContablesAsync();
            }
            
            async Task ActualizarComprobantesContablesAsync()
            {

                var ComprobantesActualizar = dataComprobantes
                    .Where(x => x.Tipo_Operacion == dataMovConta.Top_codi)
                    .Select(x => x.Comprobante_Id)
                    .Distinct()
                    .ToList();

                foreach (var item in ComprobantesActualizar)
                {
                    if (Request.CodigoProceso == "CAC" || Request.CodigoProceso == "CPLU")
                    {
                        var listaCpgDmcInfoUnica2 = listaCpgDmcInfo
                            .GroupBy(info => new { info.CpgId, info.DmcRefe })
                            .Select(grupo => grupo.First())
                            .ToList();

                        foreach (var item1 in listaCpgDmcInfoUnica2)
                        {
                            var updatePlanPago = new UpdatePlanPago
                            {
                                numeroCredito = item1.DmcRefe,
                                cpgId = Guid.Parse(item1.CpgId),
                                mcoCont = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 0),
                                mcoNume = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 1)
                            };

                            await _carteraRepository.UpdatePlanPago(updatePlanPago);
                        }
                    }

                    comprobanteCarteraUpdate.RESPUESTA_SEVEN = response.Data?.Body?.InsertarMovContableResult;
                    comprobanteCarteraUpdate.ES_EXITOSO = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 2) == 0;
                    comprobanteCarteraUpdate.CC_ID = item;
                    comprobanteCarteraUpdate.TIPO_OPERACION = dataMovConta.Top_codi;
                    comprobanteCarteraUpdate.CC_APROBADO_POR = Request.CreadoPor;
                    comprobanteCarteraUpdate.CC_COMPROBANTE_SEVEN_ID = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 1);
                    comprobanteCarteraUpdate.ESTADO = (comprobanteCarteraUpdate.ES_EXITOSO ?? false) ? "Aprobado" : "Rechazado";

                    responseComprobante.Data = await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);
                    if (responseComprobante.Data != null)
                    {
                        comprobantesActualizados.Add(responseComprobante.Data);
                    }
                }
            }

            string LimpiarDescripcion(string descripcion)
            {
                var regex = new Regex(@"\b(\d{1,2}/\d{1,2}/\d{4})\s+\d{1,2}:\d{2}:\d{2}\s+[AP]M\b");
                var match = regex.Match(descripcion);
                if (match.Success)
                {
                    var formattedDate = DateTime.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture).ToString("d/M/yyyy");
                    return regex.Replace(descripcion, formattedDate);
                }
                return descripcion;
            }
        }



        //public async Task<Response<ComprobanteCarteraPorProcesoDTO>> GenerarComprobanteSevenPorProceso(GenerarComprobanteSevenPorProcesoDto Request, CancellationToken cancellationToken)
        //{
        //    if (Request.Comprobantes.Count == 0)
        //        return new Response<ComprobanteCarteraPorProcesoDTO> { Data = null, IsSuccess = false, ReturnMessage = "No hay comprobantes para el proceso contable" };
        //    if (string.IsNullOrEmpty(Request.CodigoProceso))
        //        return new Response<ComprobanteCarteraPorProcesoDTO> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el codigo del proceso para los comprobantes Contables" };

        //    Response<InsertarMovContableResponse> response = new();
        //    Response<ComprobanteCarteraPorProcesoDTO> responseComprobante = new();



        //    var dataMovConta = new TSCnMcont();
        //    List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovCUentaContable = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();


        //    List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalles = new();
        //    InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();

        //    List<RequestDetallePago> listaMovimientosComprobante = new();
        //    ComprobanteCarteraUpdate comprobanteCarteraUpdate = new();
        //    List<ComprobanteCarteraPorProcesoDTO> dataComprobantes = new();
        //    List<string> listaCPG_ID = new List<string>();
        //    HashSet<string> uniqueCPG_IDs = new HashSet<string>();
        //    List<CpgDmcInfo> listaCpgDmcInfo = new List<CpgDmcInfo>();


        //    foreach (var item in Request.Comprobantes)
        //    {
        //        dataComprobantes.AddRange(await _carteraRepository.ObtenerComprobanteCarteraPorId(item.Comprobante_Id, Request.CodigoProceso));
        //        //item.Estado = item.Aprobado && item.Comprobante_Seven_Id != null ? "Aprobado Y enviado" : "Sin Aprobar";
        //        var movimientos = await _carteraRepository.ObtenerDetallesPagosSevenCartera(item.Comprobante_Id);
        //        listaMovimientosComprobante.AddRange(movimientos);
        //    }

        //    construirDatosSeven();

        //    void construirDatosSeven()
        //    {
        //        var tipoOperacionAgrupado = listaMovimientosComprobante.GroupBy(x => x.Top_codi).Select(x => x.FirstOrDefault()).ToList();

        //        foreach (var operacion in tipoOperacionAgrupado)
        //        {
        //            detalles = new();
        //            detalleMovCUentaContable = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
        //            dataMovConta.Emp_codi = operacion.Emp_codi;
        //            dataMovConta.Top_codi = operacion.Top_codi;
        //            dataMovConta.Mco_nume = 0;
        //            dataMovConta.Mco_fech = operacion.Mco_fech;
        //            dataMovConta.Mod_codi = operacion.Mod_codi;
        //            dataMovConta.Arb_csuc = "1001";

        //            var regex = new Regex(@"\b(\d{1,2}/\d{1,2}/\d{4})\s+\d{1,2}:\d{2}:\d{2}\s+[AP]M\b");
        //            string cleanedDesc;

        //            var match = regex.Match(operacion.Mco_desc);
        //            if (match.Success)
        //            {
        //                var formattedDate = DateTime.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture).ToString("d/M/yyyy");
        //                cleanedDesc = regex.Replace(operacion.Mco_desc, formattedDate);
        //            }
        //            else
        //            {
        //                cleanedDesc = operacion.Mco_desc;
        //            }

        //            dataMovConta.Mco_desc = cleanedDesc;



        //            var movimientosOperacion = listaMovimientosComprobante.Where(x => x.Top_codi == operacion.Top_codi).ToList();
        //            foreach (var item in movimientosOperacion)
        //            {
        //                ReferenciasSvc.Seven.SCnMcont.TSCnDmcon[] vDetalle = JsonConvert.DeserializeObject<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon[]>(item.Mov_Contable_Seven);

        //                for (int i = 0; i < vDetalle.Length; i++)
        //                {
        //                    var dataIdentificacion = vDetalle[i].Ter_coda.Split(";");
        //                    vDetalle[i].Ter_coda = dataIdentificacion[0] ?? string.Empty;
        //                    vDetalle[i].Dmc_desc = dataMovConta.Mco_desc;
        //                }

        //                if (Request.CodigoProceso == "CAC" || Request.CodigoProceso == "CPLU")
        //                {
        //                    var vDetalleCpg = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(item.Mov_Contable_Seven);

        //                    foreach (var detalle in vDetalleCpg)
        //                    {
        //                        if (detalle.TryGetValue("Cpg_Id", out var cpgId) && !string.IsNullOrEmpty(cpgId))
        //                        {
        //                            listaCPG_ID.Add(cpgId);
        //                            uniqueCPG_IDs.Add(cpgId);

        //                            // Guardar también el valor de Dmc_refe si existe
        //                            if (detalle.TryGetValue("Dmc_refe", out var dmcRefe) && !string.IsNullOrEmpty(dmcRefe))
        //                            {
        //                                listaCpgDmcInfo.Add(new CpgDmcInfo
        //                                {
        //                                    CpgId = cpgId,
        //                                    DmcRefe = long.Parse(dmcRefe)
        //                                });
        //                            }
        //                        }
        //                    }
        //                }

        //                detalles.AddRange(vDetalle);
        //            }

        //            var cuentaContableAgrupado = detalles.GroupBy(x => x.Cue_codi).ToList();
        //            foreach (var item in cuentaContableAgrupado)
        //            {
        //                decimal debito = 0;
        //                decimal credito = 0;
        //                ReferenciasSvc.Seven.SCnMcont.TSCnDmcon detalleCuentaContable = new();
        //                detalleCuentaContable = item.FirstOrDefault();
        //                detalleCuentaContable.Ter_coda = "000201508";//tercero seven para causaciones diarias 




        //                foreach (var c in item)
        //                {
        //                    debito += Math.Round(c.Dmc_vadb, 5);
        //                    credito += Math.Round(c.Dmc_vacr, 5);
        //                }
        //                detalleCuentaContable.Dmc_vadb = Math.Round(debito, 5);
        //                detalleCuentaContable.Dmc_vacr = Math.Round(credito, 5);

        //                detalleMovCUentaContable.Add(detalleCuentaContable);
        //            }
        //            dataMovConta.vDetalle = detalleMovCUentaContable.ToArray();
        //            enviarConsumoContableSeven(dataMovConta);


        //        }
        //    }
        //    void enviarConsumoContableSeven(TSCnMcont dataMovContable)
        //    {
        //        var bodyLeasing = new InsertarMovContableRequestBody(dataMovContable);
        //        movContableRequest = new InsertarMovContableRequest(bodyLeasing);
        //        response.Data = _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken).Result;
        //        //respuestaSevenLeasing = response.Data.Body.InsertarMovContableResult;
        //        response.ReturnMessage = response.Data is not null ? "Resultado Mov. Contable Seven Créditos leasing: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, ";
        //        responseComprobante.ReturnMessage = response.Data is not null ? "Resultado Mov. Contable Seven Créditos leasing: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, ";

        //        actualizarComprobantesContables();

        //    }
        //    async Task actualizarComprobantesContables()
        //    {
        //        List<int> ComprobantesActualizar = new();
        //        ComprobantesActualizar.AddRange(dataComprobantes.Where(x => x.Tipo_Operacion == dataMovConta.Top_codi).Select(x => x.Comprobante_Id).Distinct().ToList());
        //        var respuestaSevenConsumo = response.Data?.Body?.InsertarMovContableResult;
        //        int respuestaSeven = GetValueResponseXmlSevenFactura(respuestaSevenConsumo, 2);

        //        /*si la respuesta es exitosa*/
        //        foreach (var item in ComprobantesActualizar)
        //        {
        //                               if (Request.CodigoProceso == "CAC" || Request.CodigoProceso == "CPLU")
        //            {
        //                var listaCpgDmcInfoUnica2 = listaCpgDmcInfo
        //                .GroupBy(item => new { item.CpgId, item.DmcRefe }) // Agrupar por CpgId y DmcRefe
        //                .Select(grupo => grupo.First()) // Tomar solo el primer elemento de cada grupo
        //                .ToList();

        //                foreach (var item1 in listaCpgDmcInfoUnica2)
        //                {

        //                    UpdatePlanPago updatePlanPago = new UpdatePlanPago();
        //                    int MCO_CONT = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 0);
        //                    int MCO_NUME = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 1);
        //                    updatePlanPago.numeroCredito = item1.DmcRefe;
        //                    updatePlanPago.cpgId = Guid.Parse(item1.CpgId);
        //                    updatePlanPago.mcoCont = MCO_CONT;
        //                    updatePlanPago.mcoNume = MCO_NUME;

        //                    var respuesta = await _carteraRepository.UpdatePlanPago(updatePlanPago);

        //                }

        //            }

        //            if (respuestaSeven == 0)
        //            {
        //                int MCO_CONT = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 0);
        //                int MCO_NUME = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 1);
        //                comprobanteCarteraUpdate.RESPUESTA_SEVEN = respuestaSevenConsumo;
        //                comprobanteCarteraUpdate.ES_EXITOSO = true;
        //                comprobanteCarteraUpdate.CC_ID = item;
        //                comprobanteCarteraUpdate.TIPO_OPERACION = dataMovConta.Top_codi;
        //                comprobanteCarteraUpdate.CC_COMPROBANTE_SEVEN_ID = MCO_NUME;
        //                comprobanteCarteraUpdate.CC_APROBADO_POR = Request.CreadoPor;
        //                comprobanteCarteraUpdate.ESTADO = "Aprobado";

        //            }
        //            else
        //            {
        //                comprobanteCarteraUpdate.RESPUESTA_SEVEN = respuestaSevenConsumo;
        //                comprobanteCarteraUpdate.ES_EXITOSO = false;
        //                comprobanteCarteraUpdate.CC_ID = item;
        //                comprobanteCarteraUpdate.TIPO_OPERACION = dataMovConta.Top_codi;
        //                comprobanteCarteraUpdate.CC_APROBADO_POR = Request.CreadoPor;
        //                //comprobanteCarteraUpdate.ESTADO = "Aprobado";

        //            }

        //            responseComprobante.Data = await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);
        //            //UpdateComprobante = UpdateComprobante

        //        }

        //    }
        //    return responseComprobante;
        //}



        private async Task<Response<IEnumerable<MovimientoComprobanteDto>>> ObtenerMovimientosComprobante(int Comprobante)
        {
            var response = this._mapper.Map<IEnumerable<MovimientoComprobanteDto>>(await _carteraRepository.ObtenerMovimientosComprobanteCartera(Comprobante));
            return new Response<IEnumerable<MovimientoComprobanteDto>> { Data = response };
        }

        public async Task<Response<List<MovimientoComprobantePorProcesoDto>>> ObtenerMovimientosContableCartera(MovimientosContableParametersDto requiest, CancellationToken cancellationToken)
        {
            var detallesIterar = this._mapper.Map<List<MovimientoComprobantePorProcesoDto>>(await _carteraRepository.ObtenerMovimientosContableCartera(requiest));

            //foreach (var movimiento in detallesIterar)
            //{
            //    var dataTercero = movimiento.Nombre_Tercero.Split(":");
            //    var dataIdentificacion = movimiento.Numero_Identificacion.Split(";");
            //    var nombre = dataTercero.Length == 2 ? dataTercero[1] ?? string.Empty : string.Empty;
            //    var unidadEjecutora = dataIdentificacion.Length == 2 ? dataIdentificacion[1] ?? string.Empty : string.Empty;
            //    var identificacion = dataIdentificacion[0] ?? string.Empty;

            //    //movimiento.Nombre_Tercero = nombre;
            //    //movimiento.Numero_Identificacion = identificacion;
            //    movimiento.Unidad_Ejecutora = unidadEjecutora;
            //}
            ////movimientos.ToList().ForEach(x => { x.Nombre_Tercero = (await _iReportesRequestProvider.ObtenerNombresCliente(x.Numero_Identificacion)).Data?.NombreCompleto; });



            return new Response<List<MovimientoComprobantePorProcesoDto>> { Data = detallesIterar };

        }

        public async Task<FileResult> GenerarExcelComprobanteDetalle(Guid parametersDto, CancellationToken cancellationToken)
        {
            var reportResponse = await ObtenerMovimientosContableCarteraTotal(parametersDto, cancellationToken);


            List<ComprobanteCarteraPorProcesoDTO> listaComprobantes = new();

            ObtenerComprobantePorProcesoDto request = new()
            {
                DetalleMovimientoId = parametersDto,
                FechaInicio = null,
                FechaFinal = null,
                CodigoProceso = null,
                TipoOperacion = null,
                ComprobanteId = null,
                Estado = null
            };


            listaComprobantes = (await _carteraRepository.ObtenerComprobanteCartera(request, 1)).ToList();

            var columnsInfo = new ColumnHeaderInfo[]
            {
                "Tipo Operación",
                "Cuenta contable",
                "Descripción de la cuenta",
                "Número Identificación",
                "Nombre",
                "Fecha Creación",
                "Fecha Transaccion",
                "Detalle",
                "Tipo Comprobante",
                "Comprobante Interno",
                "Comprobante Seven",
                "Numero Crédito",
                "Tipo Crédito",
                "Centro de costo",
                "Area",
                ("Débito", ColumnFormatType.Decimal),
                ("Crédito", ColumnFormatType.Decimal),
            };

            try
            {
                if (reportResponse == null || !reportResponse.Any())
                {
                    throw new Exception("No se pudo obtener los datos para generar el reporte.");
                }

                var totalDebito = reportResponse.Sum(x => x.Debito);
                var totalCredito = reportResponse.Sum(x => x.Credito);

                var data = reportResponse.Select(x =>
                    new string[]
                    {
                        x.TIPO_OPERACION,
                        x.Cuenta_Contable,
                        x.Descripcion_Cuenta,
                        x.Numero_Identificacion,
                        x.Nombre_Tercero,
                        listaComprobantes[0].Fecha_Comprobante.ToString("dd/MM/yyyy"),
                        listaComprobantes[0].Fecha_Transaccion.ToString("dd/MM/yyyy"),
                        x.Detalle,
                        listaComprobantes[0].Tipo_Comprobante.ToString(),
                        listaComprobantes[0].Comprobante_Id.ToString(),
                        listaComprobantes[0].Comprobante_Seven_Id.ToString(),
                        x.Numero_Credito,
                        x.Tipo_Credito,
                        x.Centro_Costo,
                        x.Unidad_Ejecutora,
                        x.Debito.ToString(CultureInfo.InvariantCulture),
                        x.Credito.ToString(CultureInfo.InvariantCulture)
                    }
                ).ToList();

                var totalRow = new string[]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "Total",
                    totalDebito.ToString(CultureInfo.InvariantCulture),
                    totalCredito.ToString(CultureInfo.InvariantCulture)
                };

                data.Add(totalRow);
                var finalData = data.ToArray();

                var fileResult = await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatDto(columnsInfo, finalData));
                return fileResult;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo generar el reporte", ex);
            }
        }

        private async Task<IEnumerable<MovimientoComprobantePorProcesoDto>> ObtenerMovimientosContableCarteraTotal(Guid request, CancellationToken cancellationToken)
        {
            var requestExcel = new MovimientosContableParametersDto
            {
                IdComprobante = request
            };

            // Obtener los datos del repositorio.
            var detallesIterar = await _carteraRepository.ObtenerMovimientosContableCartera(requestExcel);

            var resultados = new List<MovimientoComprobantePorProcesoDto>();

            // Procesar cada movimiento para limpiar los datos.
            foreach (var movimiento in detallesIterar)
            {
                var movimientoC = new MovimientoComprobantePorProcesoDto
                {
                    Detalle = movimiento.Detalle,
                    TIPO_OPERACION = movimiento.TIPO_OPERACION,
                    Cuenta_Contable = movimiento.Cuenta_Contable,
                    Descripcion_Cuenta = movimiento.Descripcion_Cuenta,
                    Tipo_Credito = movimiento.Tipo_Credito,
                    Numero_Identificacion = movimiento.Numero_Identificacion,
                    Nombre_Tercero = movimiento.Nombre_Tercero,
                    Centro_Costo = movimiento.Centro_Costo_Credito,
                    Unidad_Ejecutora = movimiento.Centro_Costo,
                    Debito = movimiento.Debito,
                    Credito = movimiento.Credito,
                    Numero_Credito = movimiento.Numero_Credito,
                    Cuenta_NIF = movimiento.Cuenta_NIF
                };

                resultados.Add(movimientoC);
            }

            return resultados; // Retornar la lista de resultados
        }

        public async Task<Response<List<MovimientoComprobanteDto>>> ActualizaMovimientoComprobanteCartera(MovimientosComprobanteRequest request, CancellationToken cancellationToken)
        {
            var responseList = new List<MovimientoComprobanteDto>();
            List<ComprobanteCarteraPorProcesoDTO> dataComprobantes = new();

            HashSet<string> cuentasEquivalentes;

            if (request.Tipo_Credito.Contains("leasing", StringComparison.OrdinalIgnoreCase))
            {
                if (request.CodigoProceso == "CAC")
                {
                    var equivalenciaLeasing = await _carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);
                    cuentasEquivalentes = equivalenciaLeasing.Select(e => e.CUENTA_CONTABLE_SEVEN).ToHashSet();

                }
                else if (request.CodigoProceso == "CPC")
                {
                    var equivalenciaLeasing = await _carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9072);
                    cuentasEquivalentes = equivalenciaLeasing.Select(e => e.CUENTA_CONTABLE_SEVEN).ToHashSet();
                }
                else
                {
                    cuentasEquivalentes = null;
                }

            }
            else if (request.Tipo_Credito.Contains("hipotecario", StringComparison.OrdinalIgnoreCase))
            {
                var equivalenciaHipotecario = await _carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);
                cuentasEquivalentes = equivalenciaHipotecario.Select(e => e.CUENTA_CONTABLE_SEVEN).ToHashSet();
            }
            else
            {
                return new Response<List<MovimientoComprobanteDto>>
                {
                    Data = { },
                    IsSuccess = false,
                    ReturnMessage = "Tipo de crédito no válido. Debe ser 'leasing' o 'hipotecario'.",
                };
            }
            var movimientos = await _carteraRepository.ObtenerDetallesPagosSevenCartera(request.Comprobante_Id);

            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalles = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

            foreach (var item in movimientos)
            {
                ReferenciasSvc.Seven.SCnMcont.TSCnDmcon[] vDetalle = JsonConvert.DeserializeObject<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon[]>(item.Mov_Contable_Seven);

                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detallesTemp = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

                var movimientosAgregados = request.Movimientos.Where(m => m.agregado).ToList();

                foreach (var movimientoAgregado in movimientosAgregados)
                {
                    if (vDetalle.Length > 0)
                    {
                        var filaBase = vDetalle[0];

                        var nuevaFila = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                        {
                            Cue_codi = movimientoAgregado.Cuenta_Contable.ToString(),
                            Dmc_desc = filaBase.Dmc_desc,
                            Dmc_cant = filaBase.Dmc_cant,
                            Dmc_acti = filaBase.Dmc_acti,
                            Dmc_vadb = movimientoAgregado.Debito ?? filaBase.Dmc_vadb,
                            Dmc_vacr = movimientoAgregado.Credito ?? filaBase.Dmc_vacr,
                            Dmc_vaba = filaBase.Dmc_vaba,
                            Ter_coda = filaBase.Ter_coda,
                            Arb_codc = filaBase.Arb_codc,
                            Arb_coda = filaBase.Arb_coda,
                            Arb_codp = filaBase.Arb_codp,
                            Arb_cods = filaBase.Arb_cods,
                            Ter_codm = filaBase.Ter_codm,
                            Dmc_refe = filaBase.Dmc_refe,
                        };

                        detallesTemp.Add(nuevaFila);
                    }
                }

                for (int i = 0; i < vDetalle.Length; i++)
                {
                    if (int.TryParse(vDetalle[i].Cue_codi, out int cueCodi))
                    {
                        var matchingMovimiento = request.Movimientos.FirstOrDefault(m => m.Cuenta_ContableId == cueCodi);

                        if (matchingMovimiento != null)
                        {
                            var detalleModificado = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                            {
                                Cue_codi = matchingMovimiento.Cuenta_Contable.ToString(),
                                Dmc_desc = vDetalle[i].Dmc_desc,
                                Dmc_cant = vDetalle[i].Dmc_cant,
                                Dmc_acti = vDetalle[i].Dmc_acti,
                                Dmc_vadb = matchingMovimiento.Debito ?? vDetalle[i].Dmc_vadb,
                                Dmc_vacr = matchingMovimiento.Credito ?? vDetalle[i].Dmc_vacr,
                                Dmc_vaba = vDetalle[i].Dmc_vaba,
                                Ter_coda = vDetalle[i].Ter_coda,
                                Arb_codc = vDetalle[i].Arb_codc,
                                Arb_coda = vDetalle[i].Arb_coda,
                                Arb_codp = vDetalle[i].Arb_codp,
                                Arb_cods = vDetalle[i].Arb_cods,
                                Ter_codm = vDetalle[i].Ter_codm,
                                Dmc_refe = vDetalle[i].Dmc_refe,
                            };

                            detallesTemp.Add(detalleModificado);
                        }
                        else
                        {
                            detallesTemp.Add(vDetalle[i]);
                        }
                    }
                    else
                    {
                        detallesTemp.Add(vDetalle[i]);
                    }
                }

                detalles.AddRange(detallesTemp);
            }

            decimal totalDmcVacr = Math.Round(detalles.Sum(x => x.Dmc_vacr), 2);
            decimal totalDmcCadb = Math.Round(detalles.Sum(x => x.Dmc_vadb), 2);

            bool todasCuentasValidas = detalles.All(detalle => cuentasEquivalentes.Contains(detalle.Cue_codi));

            var cuentasRepetidas = detalles.GroupBy(detalle => detalle.Cue_codi)
            .Where(group => group.Count() > 1).Select(group => group.Key).ToList();

            if (totalDmcVacr == totalDmcCadb && todasCuentasValidas && !cuentasRepetidas.Any())
            {
                string jsonDetallesActualizados = JsonConvert.SerializeObject(detalles);

                var requestUpdate = new ComprobanteCarteraUpdate
                {
                    CC_ID = request.Comprobante_Id,
                    RESPUESTA_SEVEN = jsonDetallesActualizados
                };

                var movimientosActualizados = await _carteraRepository.UpdateDetallesComprobanteContableCartera(requestUpdate);
            }
            else
            {
                string errorMessage = "Actualización fallida. ";

                if (totalDmcVacr != totalDmcCadb)
                {
                    errorMessage += "Los totales de crédito y débito no coinciden. ";
                }

                if (!todasCuentasValidas)
                {
                    errorMessage += "Existen cuentas no válidas en los registros.";
                }
                if (cuentasRepetidas.Any())
                {
                    errorMessage += $"Se encontraron cuentas repetidas: {string.Join(", ", cuentasRepetidas)}.";
                }

                return new Response<List<MovimientoComprobanteDto>>
                {
                    Data = responseList,
                    IsSuccess = false,
                    ReturnMessage = errorMessage.Trim(),
                };
            }

            return new Response<List<MovimientoComprobanteDto>>
            {
                Data = responseList,
                IsSuccess = true,
                ReturnMessage = "Se Actualizaron los movimientos con Exito",
            };
        }

        #endregion

        #region Actualizacion Credito - Plan Pagos
        public async Task<Response<object>> ActualizarProdutoClienteCartera(RequestActualizacionProducto request, CancellationToken cancellationToken)
        {
            ProductoClienteDto response = this._mapper.Map<IEnumerable<ProductoClienteDto>>(await this._carteraRepository.ObtenerProductoCliente(request.TipoIdentificacion, request.NumeroIdentificacion, null)).FirstOrDefault();
            if (response != null)
            {
                if (response.IdTipoCredito == 1)
                {
                    await UpdatePlnLeasing(response, request);
                }
                else
                {


                }
            }
            throw new NotImplementedException();
        }

        private async Task<List<Leasing>> UpdatePlnLeasing(ProductoClienteDto productoClienteDto, RequestActualizacionProducto requestActualizacionProducto)
        {
            List<Leasing> PlanSimulacion = new();
            List<ConformacionLeasing> LstConformacionLeasing = new();
            PlanPagoDto planPagoDtoNew = new();
            ObtenerProductoCartera obtenerProductoCartera = new()
            {
                Nro_Credito = productoClienteDto.NumeroCredito,
                Tcr_Id = productoClienteDto.IdTipoCredito
            };
            ResponsePlanPagoDto PlanPagos = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;
            foreach (PlanPagoDto cuota in PlanPagos.PlanPago.Where(x => x.EstadoCuota.Equals("")))
            {
            }
            #region Paso3
            var tasaSeguroVida = (productoClienteDto.TasaSeguroVida == requestActualizacionProducto.Tasa_Seguro_Vida ? productoClienteDto.TasaSeguroVida : requestActualizacionProducto.Tasa_Seguro_Vida) / 100;
            var tasaSeguroHogar = (productoClienteDto.TasaSeguroHogar == requestActualizacionProducto.Tasa_Seguro_Hogar ? productoClienteDto.TasaSeguroHogar : requestActualizacionProducto.Tasa_Seguro_Hogar) / 100;
            #endregion

            return null;
        }
        #endregion

        #region Crear Planes de Pago - Creacion Credito
        private DateTime ObtenerFechaPrimerCorte(DateTime FechaDesembolso)
        {
            var fechaActual = FechaDesembolso.AddMonths(1).Day;
            var dateCorte = FechaDesembolso.AddMonths(1).AddDays((fechaActual) * -1);
            return dateCorte;
        }
        private DateTime ObtenerFechaPrimerCortepago(int addmes, int year)
        {
            if (addmes == 13)
            {
                addmes = 1;
            }
            int mes = addmes;
            int año = year;
            DateTime primerDia = new DateTime(año, mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);
            return ultimoDia;
        }

        #region Leasing 108, 107, Cesantias
        private class Leasing
        {
            public int NroCuota { get; set; }
            public DateTime FechaCorte { get; set; }
            public DateTime FechaPago { get; set; }
            //Tradicional
            public double Capital { get; set; }
            public double IntCte { get; set; }
            public double SegVida { get; set; }
            public double SegHogar { get; set; }
            public double Cuota { get; set; }
            public double Saldo { get; set; }

            //Leasing
            public double InteresesCorrientesAPagar { get; set; }
            public double InteresesCorrientesPorPagar { get; set; }
            public double CapitalAPagar { get; set; }
            public double CapitalPorPagar { get; set; }
            public double SaldoCapitalReducido { get; set; }
            public double CuotaUnidadPagadora { get; set; }
            public double DeudaRemanente { get; set; }
        }
        private class ConformacionLeasing
        {
            public DateTime Fecha_Corte { get; set; }
            public DateTime Fecha_Pago { get; set; }
            public int Mes { get; set; }
            public double Canon { get; set; }
            public double Intereses { get; set; }
            public double AporteCapital { get; set; }
            public double SaldoaCapital { get; set; }
            public double Seguros { get; set; }
            public double CanonSeguro { get; set; }
            public double CanonReducido { get; set; }
            public double InteresReducido { get; set; }
            public double InteresRemanente { get; set; }
            public double AporteCapitalReducido { get; set; }
            public double CapitalNoAmortizado { get; set; }
            public double SaldoaCapitalReducido { get; set; }
            public double CanonReducidoSeguros { get; set; }
            public double DeudaRemanente { get; set; }
            public double DeudaRemanenteAnterior { get; set; }
        }
        public static decimal Truncar(decimal valor, int decimales)
        {
            decimal factor = (decimal)Math.Pow(10, decimales);
            return Math.Truncate(valor * factor) / factor;
        }
        private async Task<List<Leasing>> PLNPAGOSLEASING(CrearProductoCliente crearProductoCliente)
        {
            List<Leasing> PlanSimulacion = new();
            List<ConformacionLeasing> LstConformacionLeasing = new();
            #region Paso1
            var valorViviendaDeseado = Math.Max(
                crearProductoCliente.CRE_VALOR_VIVIENDA,
                crearProductoCliente.CRE_VALOR_VIVIENDA_SEGURO_HOGAR
            );
            var valorViviendaReal = crearProductoCliente.CRE_VALOR_VIVIENDA_SEGURO_HOGAR;
            #endregion
            #region Paso3
            var porcentajeCanonInicial = crearProductoCliente.CRE_POR_CANON_INICIAL / 100;
            var porcentajeOpcionCompra = crearProductoCliente.CRE_POR_OPCION_COMPRA / 100;
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA / 100;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)(tasaCredito)), 1.0 / 12.0) - 1;
            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL;
            var AnosPlazo = plazo % 12 == 0 ? (int)(plazo / 12) : (int)(plazo / 12) + 1;
            #endregion
            #region Paso4
            var factor = (tasaPeriodicaNMV * Math.Pow((1 + (double)tasaPeriodicaNMV), plazo) / (Math.Pow(1 + (double)tasaPeriodicaNMV, plazo) - 1));
            var valorCanonInicial = valorViviendaReal * porcentajeCanonInicial;
            #endregion
            #region Paso5
            var VrCredito = valorViviendaReal - valorCanonInicial;
            var valorSeguroVida = VrCredito * tasaSeguroVida;
            //var valorSeguroHogar = (double)(valorViviendaDeseado * (tasaSeguroHogar / 12)) * 1.19;
            var valorSeguroHogar = (double)(valorViviendaDeseado * (tasaSeguroHogar));
            var VrCuotaSinOpcC = (VrCredito * (decimal)factor) + (decimal)valorSeguroHogar + valorSeguroVida;
            var valorOpcionDeCompra = (decimal)(valorViviendaReal * porcentajeOpcionCompra);
            var VrDescOpcCompra = (valorOpcionDeCompra / (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), plazo)) * (decimal)factor;
            var canonTradicional = VrCuotaSinOpcC - VrDescOpcCompra;
            var RecursosFuturos = (double)crearProductoCliente.RECURSOS_FUTUROS;
            #endregion
            #region Paso6
            var DivCuoEquiVF = Math.Pow((double)(1 + tasaPeriodicaNMV), plazo);
            var CuotaEquivalenteVF = (valorOpcionDeCompra * (decimal)factor) / (decimal)DivCuoEquiVF;
            var CanonReducido = Math.Abs(VrCuotaSinOpcC - CuotaEquivalenteVF);
            //var CapitalReducido = (CanonReducido - valorSeguroHogar - valorSeguroVida) / (decimal)factor;
            #endregion
            #region Simulacion
            for (int i = 0; i <= plazo; i++)
            {
                if (i == 0)
                {
                    ConformacionLeasing CL = new ConformacionLeasing();
                    CL.Fecha_Corte = crearProductoCliente.CRE_FECHA_DESEMBOLSO;
                    CL.Fecha_Pago = crearProductoCliente.CRE_FECHA_DESEMBOLSO;
                    CL.Mes = i;
                    CL.AporteCapital = Math.Round((double)valorCanonInicial, 0);
                    CL.SaldoaCapital = Math.Round((double)VrCredito, 0);
                    CL.AporteCapitalReducido = Math.Round((double)valorCanonInicial);
                    CL.SaldoaCapitalReducido = Math.Round((((double)VrCredito) - await CalcularValorActual(tasaPeriodicaNMV, plazo, 0, -RecursosFuturos, 0)), 0);
                    CL.DeudaRemanente = CL.SaldoaCapitalReducido;
                    LstConformacionLeasing.Add(CL);

                }
                else
                {
                    ConformacionLeasing CL = new ConformacionLeasing();
                    var FechaCorteTemp = LstConformacionLeasing[i - 1].Fecha_Corte.AddMonths(1);
                    CL.Fecha_Corte = new DateTime(FechaCorteTemp.Year, FechaCorteTemp.Month, DateTime.DaysInMonth(FechaCorteTemp.Year, FechaCorteTemp.Month));
                    var FechaPagoTemp = LstConformacionLeasing[i - 1].Fecha_Pago.AddMonths(1).AddDays(-LstConformacionLeasing[i - 1].Fecha_Pago.Day).AddMonths(1);
                    CL.Fecha_Pago = new DateTime(FechaPagoTemp.Year, FechaPagoTemp.Month, DateTime.DaysInMonth(FechaPagoTemp.Year, FechaPagoTemp.Month));
                    CL.Mes = i;
                    //Tradicional
                    CL.Canon = Math.Round(await CalcularPago(tasaPeriodicaNMV, plazo, -(double)VrCredito, (double)valorOpcionDeCompra, 0));
                    CL.Intereses = Math.Round(LstConformacionLeasing[i - 1].SaldoaCapital * tasaPeriodicaNMV);
                    CL.AporteCapital = CL.Canon - CL.Intereses;
                    CL.SaldoaCapital = LstConformacionLeasing[i - 1].SaldoaCapital - CL.AporteCapital;
                    CL.Seguros = Math.Round((double)valorSeguroVida + valorSeguroHogar, 0);
                    CL.CanonSeguro = CL.Canon + CL.Seguros;

                    //Leasing
                    CL.InteresReducido = Math.Round(LstConformacionLeasing[i - 1].SaldoaCapitalReducido * tasaPeriodicaNMV, 0);
                    CL.CanonReducido = i == plazo ? LstConformacionLeasing[i - 1].SaldoaCapitalReducido + CL.InteresReducido : Math.Round(await CalcularPago(tasaPeriodicaNMV, plazo, -(double)VrCredito, RecursosFuturos, 0), 0);
                    CL.InteresRemanente = crearProductoCliente.CLC_ID == (int)TipoLineaCreditoEnum.LEASING_VETERANOS ? 0 : CL.Intereses - CL.InteresReducido;
                    CL.AporteCapitalReducido = CL.CanonReducido - CL.InteresReducido;
                    CL.CapitalNoAmortizado = crearProductoCliente.CLC_ID == (int)TipoLineaCreditoEnum.LEASING_VETERANOS ? 0 : CL.AporteCapital - CL.AporteCapitalReducido;
                    CL.SaldoaCapitalReducido = LstConformacionLeasing[i - 1].SaldoaCapitalReducido - CL.AporteCapitalReducido;
                    CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                    CL.DeudaRemanente = LstConformacionLeasing[i - 1].DeudaRemanente - CL.AporteCapitalReducido + CL.CapitalNoAmortizado + CL.InteresRemanente;

                    if (i == 1)
                    {
                        DateTime ultimoDia = new DateTime(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Year, crearProductoCliente.CRE_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Year, crearProductoCliente.CRE_FECHA_DESEMBOLSO.Month));

                        var diasProrrata = (crearProductoCliente.CRE_FECHA_DESEMBOLSO - crearProductoCliente.FECHA_SOLICITUD).Days;
                        var fecha = crearProductoCliente.FECHA_SOLICITUD;

                        int diasDelMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);
                        var seguroVidaProrrata = Math.Round((double)VrCredito * (double)tasaSeguroVida * (diasProrrata) / diasDelMes, 0);

                        var seguroHogarProrrata = Math.Round(valorSeguroHogar / diasDelMes, 0) * (diasProrrata);

                        decimal tasaMensual = (decimal)Math.Pow((double)(1 + tasaCredito), 1.0 / 12.0) - 1m;
                        decimal tasaMensualTrunc = Math.Truncate(tasaMensual * 10_000_000m) / 10_000_000m;
                        decimal interesMensual = Math.Truncate(VrCredito * tasaMensualTrunc);
                        decimal interesDiario = interesMensual / 30m;
                        decimal interesProrrata = interesDiario * diasProrrata;
                        decimal interesFinal = Math.Round(interesProrrata, 0, MidpointRounding.AwayFromZero);



                        decimal tasaDiaria = (decimal)Math.Pow((double)(1 + tasaCredito), 1.0 / 360.0) - 1m;
                        decimal interesDiarioV = VrCredito * tasaDiaria;
                        decimal interesProrrataV = interesDiarioV * diasProrrata;
                        decimal interesFinalV = Math.Round(interesProrrataV, 0, MidpointRounding.AwayFromZero);


                        if (crearProductoCliente.CLC_ID != (int)TipoLineaCreditoEnum.LEASING_VETERANOS)
                        {
                            CL.InteresRemanente = CL.InteresRemanente + (double)interesFinalV;

                            CL.DeudaRemanente = CL.DeudaRemanente + (double)interesFinalV;
                        }
                        else {
                            var valorInteresCorriente = (VrCredito / diasDelMes) * diasProrrata;
                            CL.InteresReducido =Math.Round( CL.InteresReducido + (double)interesFinalV,0);
                        }
                        CL.Seguros += seguroVidaProrrata + seguroHogarProrrata;
                        CL.CanonSeguro = CL.Canon + CL.Seguros;
                        CL.CanonReducidoSeguros = CL.Seguros + CL.InteresReducido + CL.AporteCapitalReducido;
                    }

                    LstConformacionLeasing.Add(CL);
                }
            }

            foreach (var item in LstConformacionLeasing)
            {
                Leasing L = new Leasing();
                L.FechaCorte = item.Fecha_Corte;
                L.FechaPago = item.Fecha_Pago;
                L.NroCuota = item.Mes;
                L.Capital = item.AporteCapital;
                L.IntCte = item.Intereses;

                double segVidaFinal = 0;
                double segHogarFinal = 0;

                if (item.Mes == 1)
                {
                    var diasProrrata = (crearProductoCliente.CRE_FECHA_DESEMBOLSO - crearProductoCliente.FECHA_SOLICITUD).Days;

                    var baseSeguroVida = (double)VrCredito + LstConformacionLeasing.Sum(x => x.InteresRemanente);
                    var seguroVidaTotal = Math.Round((double)baseSeguroVida * (double)tasaSeguroVida, 0);
                    var seguroVidaProrrata = Math.Round((double)VrCredito * (double)tasaSeguroVida * (diasProrrata) / 30, 0);
                    segVidaFinal = seguroVidaTotal + seguroVidaProrrata;

                    segHogarFinal = Math.Round(valorSeguroHogar / 30, 0, MidpointRounding.AwayFromZero) * (diasProrrata + 0) + Math.Round(valorSeguroHogar, 0);


                }
                else
                {
                    var baseSeguroVida = (double)VrCredito + LstConformacionLeasing.Sum(x => x.InteresRemanente);
                    segVidaFinal = Math.Round(baseSeguroVida * (double)tasaSeguroVida, 0);
                    segHogarFinal = Math.Round(valorSeguroHogar, 0);
                }

                L.SegVida = segVidaFinal;
                L.SegHogar = segHogarFinal;

                L.Cuota = item.Canon;
                L.Saldo = item.SaldoaCapital;
                L.InteresesCorrientesAPagar = item.InteresReducido;
                L.InteresesCorrientesPorPagar = item.InteresRemanente;
                L.CapitalAPagar = item.AporteCapitalReducido;
                L.CapitalPorPagar = item.CapitalNoAmortizado;
                L.SaldoCapitalReducido = item.SaldoaCapitalReducido;
                L.CuotaUnidadPagadora = L.CapitalAPagar + L.InteresesCorrientesAPagar + segHogarFinal + segVidaFinal;
                L.DeudaRemanente = item.DeudaRemanente;

                PlanSimulacion.Add(L);
            }

            #endregion
            return PlanSimulacion;
        }
        #endregion

        #region Hipotecario
        public class Hipotecario
        {
            public int Cuota { get; set; }
            public DateTime Fecha_Corte { get; set; }
            public DateTime Fecha_Pago { get; set; }
            public double Saldo { get; set; }
            public double Capital { get; set; }
            public double Intereses { get; set; }
            public double Seguro_Vida { get; set; }
            public double Seguro_Hogar { get; set; }
            public double Abono_Unico { get; set; }
            public double Abono_Extra { get; set; }
            public double Cuota_Pagadora { get; set; }
            public double Interes_Reduccion { get; set; }
            public double Cuota_Reduccion { get; set; }
            public double Cobro_Fonvivienda { get; set; }

        }
        private async Task<List<Hipotecario>> PLNPAGOSHIPOTECARIO(CrearProductoCliente crearProductoCliente)
        {
            List<Hipotecario> planillaSimulacion = new();
            List<Hipotecario> planillaSimulacionRound = new();
            #region Paso1
            var valorViviendaDeseado = Math.Max(
                crearProductoCliente.CRE_VALOR_VIVIENDA,
                crearProductoCliente.CRE_VALOR_VIVIENDA_SEGURO_HOGAR
            );
            var valorViviendaReal = crearProductoCliente.CRE_VALOR_VIVIENDA_SEGURO_HOGAR;
            var interesesCuota = 0.0;
            var cuotaSinSeguros = 0M;
            #endregion
            #region Paso3
            var tasaCredito = crearProductoCliente.CRE_TASA_CREDITO_EA / 100;
            var tasaPeriodicaNMV = Math.Pow((1 + (double)(tasaCredito)), 1.0 / 12.0) - 1;
            var tasaSeguroVida = crearProductoCliente.CRE_TASA_SEGURO_VIDA / 100;
            var tasaSeguroHogar = crearProductoCliente.CRE_TASA_SEGURO_HOGAR / 100;
            var tasaMixta = (crearProductoCliente.CRE_TASA_SEGURO_VIDA + (decimal)tasaPeriodicaNMV * 100) / 100;
            var plazo = crearProductoCliente.CRE_PLAZO_TOTAL;
            var AnosPlazo = plazo % 12 == 0 ? (int)(plazo / 12) : (int)(plazo / 12) + 1;
            #endregion
            #region Paso4
            //Calculo el factor (tasaMixta*((1+Tasamixta)^plazo))/((1+tasaMixta)^plazo)-1
            var firstP = tasaMixta * (decimal)Math.Pow((1 + (double)tasaMixta), plazo);
            var secondP = (decimal)(Math.Pow(1 + (double)tasaMixta, plazo));
            var factor = firstP / (secondP - 1);
            decimal valorPrestamo = crearProductoCliente.CRE_VALOR_DESEMBOLSO;
            var ValorSeguroHogar = Math.Round(valorViviendaDeseado * (tasaSeguroHogar), 7);
            // var cuota1 = Math.Round((valorPrestamo * factor + ValorSeguroHogar), 0);
            var cuota = Math.Ceiling(valorPrestamo * factor + ValorSeguroHogar);

            //var diasProrrata = (crearProductoCliente.CRE_FECHA_DESEMBOLSO - crearProductoCliente.FECHA_SOLICITUD).Days;


            #endregion
            #region Simulacion
            for (int i = 0; i <= plazo; i++)
            {
                if (i == 0)
                {
                    planillaSimulacion.Add(new Hipotecario
                    {
                        Fecha_Corte = crearProductoCliente.CRE_FECHA_DESEMBOLSO,
                        Fecha_Pago = crearProductoCliente.CRE_FECHA_DESEMBOLSO,
                        Cuota = i,
                        Capital = 0,
                        Intereses = 0,
                        Seguro_Hogar = 0,
                        Seguro_Vida = 0,
                        Cuota_Pagadora = 0,
                        Saldo = (double)valorPrestamo
                    });
                }
                else
                {
                    var fechaCorte = new DateTime(planillaSimulacion[i - 1].Fecha_Corte.Year, planillaSimulacion[i - 1].Fecha_Corte.Month, 1)
                        .AddMonths(2).AddDays(-1);
                    int mes = fechaCorte.Month;
                    int año = fechaCorte.Year;

                    DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                    Hipotecario CH = new Hipotecario();

                    CH.Fecha_Corte = fechaCorte;
                    CH.Fecha_Pago = fechaCorte;
                    CH.Intereses = Math.Round(planillaSimulacion[i - 1].Saldo * tasaPeriodicaNMV, 0);
                    CH.Seguro_Vida = Math.Round(planillaSimulacion[i - 1].Saldo * (double)tasaSeguroVida, 7);
                    CH.Cuota = i;
                    CH.Seguro_Hogar = (double)ValorSeguroHogar;
                    CH.Cuota_Pagadora = i == plazo ?
                        Math.Round(planillaSimulacion[i - 1].Saldo + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0)
                        : (double)cuota;
                    CH.Capital = Math.Round(CH.Cuota_Pagadora - (CH.Seguro_Vida + CH.Seguro_Hogar + CH.Intereses), 0);
                    CH.Saldo = Math.Round(planillaSimulacion[i - 1].Saldo - CH.Capital, 0);

                    // Aplicar prorrata en cuota 1
                    if (i == 1)
                    {

                        var diasProrrata = (crearProductoCliente.CRE_FECHA_DESEMBOLSO.Date - crearProductoCliente.FECHA_SOLICITUD.Date).Days;

                        var fecha = crearProductoCliente.FECHA_SOLICITUD;

                        int diasDelMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);

                        DateTime ultimoDia = new DateTime(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Year, crearProductoCliente.CRE_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(crearProductoCliente.CRE_FECHA_DESEMBOLSO.Year, crearProductoCliente.CRE_FECHA_DESEMBOLSO.Month));

                        var seguroVidaProrrata = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaSeguroVida * (diasProrrata) / diasDelMes, 0);

                        var seguroHogarProrrata = Math.Round(((decimal)ValorSeguroHogar / diasDelMes) * (diasProrrata), 0);

                        decimal tasaMensual = (decimal)Math.Pow((double)(1 + tasaCredito), 1.0 / 12.0) - 1m;
                        decimal tasaMensualTrunc = Math.Truncate(tasaMensual * 10_000_000m) / 10_000_000m;
                        decimal interesMensual = Math.Truncate(crearProductoCliente.CRE_VALOR_DESEMBOLSO * tasaMensualTrunc);
                        decimal interesDiario = interesMensual / 30m;
                        decimal interesProrrata = interesDiario * diasProrrata;
                        decimal interesFinal = Math.Round(interesProrrata, 0, MidpointRounding.AwayFromZero);



                        decimal tasaDiaria = (decimal)Math.Pow((double)(1 + tasaCredito), 1.0 / 360.0) - 1m;
                        decimal interesDiarioV = crearProductoCliente.CRE_VALOR_DESEMBOLSO * tasaDiaria;
                        decimal interesProrrataV = interesDiarioV * diasProrrata;
                        decimal interesFinalV = Math.Round(interesProrrataV, 0, MidpointRounding.AwayFromZero);



                        var valorCredito = Math.Round((double)crearProductoCliente.CRE_VALOR_DESEMBOLSO * (double)tasaPeriodicaNMV, 0);
                        var valorInteresCorriente = (valorCredito / diasDelMes) * diasProrrata;

                        CH.Intereses = CH.Intereses + valorInteresCorriente;
                        CH.Seguro_Vida = CH.Seguro_Vida + (double)seguroVidaProrrata;
                        CH.Seguro_Hogar = CH.Seguro_Hogar + (double)seguroHogarProrrata;
                        CH.Cuota_Pagadora = Math.Round(CH.Capital + CH.Intereses + CH.Seguro_Vida + CH.Seguro_Hogar, 0);

                    }

                    planillaSimulacion.Add(CH);
                }

            }

            foreach (var item in planillaSimulacion)
            {
                planillaSimulacionRound.Add(new Hipotecario
                {
                    Fecha_Corte = item.Fecha_Corte,
                    Fecha_Pago = item.Fecha_Pago,
                    Cuota = item.Cuota,
                    Capital = Math.Round(item.Capital, 0),
                    Intereses = Math.Round(item.Intereses, 0),
                    Seguro_Hogar = Math.Round(item.Seguro_Hogar, 0),
                    Seguro_Vida = Math.Round(item.Seguro_Vida, 0),
                    Cuota_Pagadora = Math.Round(item.Cuota_Pagadora, 0),
                    Saldo = Math.Round(item.Saldo, 0)
                });
            }


            return planillaSimulacionRound;
            #endregion
        }
        #endregion

        public async Task<Response<List<InsertPlanPagosDto>>> CrearProductoClientePlanPago(InsertProductoClienteDto request, CancellationToken cancellationToken)
        {
            List<InsertPlanPagosDto> LstinsertPlanPagosDto = new List<InsertPlanPagosDto>();
            var MapeoCredito = (await MapeoCreCredito(request));
            if (MapeoCredito.IsSuccess)
            {
                switch (MapeoCredito.Data.TCR_ID)
                {
                    case 1://LEASING
                        var LeasingPLN107 = await PLNPAGOSLEASING(MapeoCredito.Data);
                        for (int i = 1; i < LeasingPLN107.Count; i++)
                        {
                            LstinsertPlanPagosDto.Add(new InsertPlanPagosDto()
                            {
                                PlnNroCredito = MapeoCredito.Data.CRE_NRO_CREDITO,
                                PlnCuota = LeasingPLN107[i].NroCuota,
                                PlnFechaPago = LeasingPLN107[i].FechaPago,
                                PlnSaldoIncial = (decimal)LeasingPLN107[i - 1].DeudaRemanente,
                                PlnCapital = (decimal)LeasingPLN107[i].CapitalAPagar,
                                PlnCapitalPorPagar = (decimal)LeasingPLN107[i].CapitalPorPagar,
                                PlnInteres = (decimal)LeasingPLN107[i].InteresesCorrientesAPagar,
                                PlnInteresPorPagar = (decimal)LeasingPLN107[i].InteresesCorrientesPorPagar,
                                PlnSeguroVida = Math.Round((decimal)LeasingPLN107[i].SegVida, 0),
                                PlnSeguroHogar = Math.Round((decimal)LeasingPLN107[i].SegHogar, 0),
                                PlnAPagar = (decimal)LeasingPLN107[i].CuotaUnidadPagadora,
                                PlnValorDeudaRemanente = (decimal)LeasingPLN107[i].DeudaRemanente,
                                PlnPlazo = MapeoCredito.Data.CRE_PLAZO_TOTAL
                            });
                        }
                        break;
                    case 2://HIPOTECARIO
                    case 5: //COMPRA CARTERA
                        var HIPPLN = await PLNPAGOSHIPOTECARIO(MapeoCredito.Data);
                        foreach (var item in HIPPLN)
                        {
                            LstinsertPlanPagosDto.Add(new InsertPlanPagosDto()
                            {
                                PlnNroCredito = MapeoCredito.Data.CRE_NRO_CREDITO,
                                PlnCuota = item.Cuota,
                                PlnFechaPago = item.Fecha_Pago,
                                PlnSaldoIncial = (decimal)item.Saldo,
                                PlnCapital = (decimal)item.Capital,
                                PlnCapitalPorPagar = 0,
                                PlnInteres = (decimal)item.Intereses,
                                PlnInteresPorPagar = 0,
                                PlnSeguroVida = Math.Round((decimal)item.Seguro_Vida),
                                PlnSeguroHogar = Math.Round((decimal)item.Seguro_Hogar),
                                PlnAPagar =  item.Cuota == MapeoCredito.Data.CRE_PLAZO_TOTAL
                                ? (decimal)(item.Capital + item.Intereses + item.Seguro_Vida + item.Seguro_Hogar)
                                : (decimal)(item.Cuota_Pagadora),
                                PlnValorDeudaRemanente = 0,
                                PlnPlazo = MapeoCredito.Data.CRE_PLAZO_TOTAL
                            });
                        }
                        break;
                    default:
                        return new Response<List<InsertPlanPagosDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se encontro la linea de credito para generar plan de pagos" };
                }
            }
            return new Response<List<InsertPlanPagosDto>> { Data = LstinsertPlanPagosDto };
        }
        private async Task<double> CalcularPago(double tasa, int nper, double va, double vf, int tipo)
        {
            if (tasa == 0)
            {
                return -(va + vf) / nper;
            }
            else
            {
                double pago = (va * tasa * Math.Pow(1 + tasa, nper) + vf * tasa) / (Math.Pow(1 + tasa, nper) - 1);
                if (tipo == 1)
                {
                    pago /= (1 + tasa);
                }
                return -pago;
            }
        }

        private async Task<double> CalcularValorActual(double tasa, int nper, double pago, double vf, int tipo)
        {
            if (tasa == 0)
            {
                return -(pago * nper + vf);
            }
            else
            {
                double valorActual = (pago * (1 - Math.Pow(1 + tasa, -nper)) / tasa) + (vf / Math.Pow(1 + tasa, nper));
                if (tipo == 1)
                {
                    valorActual *= (1 + tasa);
                }
                return -valorActual;
            }
        }
        #endregion

        #region CausacionesComprobantes
        public async Task<Response<object>> GenerarCausacionesComprobante(MovimientoReportesCartera movimientoReportes, CancellationToken cancellationToken = default)
        {
            try
            {
                ReporteContableParametersDto parametros = new ReporteContableParametersDto();
                parametros.FechaInicial = movimientoReportes.FechaComprobante;
                parametros.FechaFinal = movimientoReportes.FechaComprobante;
                IEnumerable<ReporteContableDto> listaMovimientos;

                if (movimientoReportes.Tipo == 1)
                {
                    var list = new List<ReporteContableDto>();
                    await foreach (var item in await _reportesRepository.GenerarReporteCausaciones(parametros))
                        list.Add(_mapper.Map<ReporteContableDto>(item));
                    listaMovimientos = list;
                }
                else
                {
                    listaMovimientos = this._mapper.Map<IEnumerable<ReporteContableDto>>(await _reportesRepository.GenerarReporteContablePagos(parametros));
                }

                var listaRetorna = await MapearMovimientosComprobante(listaMovimientos);

                ComprobanteCarteraDto ReqComprobante = new()
                {
                    Fecha_Comprobante = movimientoReportes.FechaComprobante,
                    Tipo_Comprobante = 1,
                    Ciudad = 1,
                    Departamento = 5,
                    Concepto = 1,
                    Beneficiario = 3,
                    LstMovimientoComprobante = listaRetorna
                };

                var Comprobante = await CrearComprobanteCartera(ReqComprobante, cancellationToken);
                return new Response<object>
                {
                    IsSuccess = true,
                    ReturnMessage = "Comprobante Creado: " + +Comprobante.Data.Comprobante_Id,
                    Data = null
                };

            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "Error al generar comprobante." };
            }
        }

        public async Task<Response<ComprobanteCarteraDto>> GenerarComprobanteContabilidad(RequestDetallePago detallepago, string proceso, decimal total_Debito, decimal total_Credito)
        {
            try
            {
                var tipoComprobanteId = (await _catalogosBusinessLogic.ObtenerTipoComprobanteCartera()).Data.FirstOrDefault(x => x.Codigo.Equals(proceso));
                ComprobanteCarteraDto ReqComprobante = new()
                {
                    Fecha_Comprobante = DateTime.Now,
                    Tipo_Comprobante = tipoComprobanteId.TipoComprobanteId,
                    Ciudad = 1,
                    Departamento = 5,
                    Concepto = 1,
                    Beneficiario = 1,
                    LstMovimientoComprobante = null,
                    Total_Comprobante_Credito = total_Credito,
                    Total_Comprobante_Debito = total_Debito,
                    estado = false
                };

                var crearComprobanteCartera = this._mapper.Map<ComprobanteCartera>(ReqComprobante);
                var response = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.CrearComprobanteCartera(crearComprobanteCartera));

                //return new Response<ComprobanteCarteraDto> { Data = response };

                //var Comprobante = await CrearComprobanteCartera(ReqComprobante, cancellationToken);
                return new Response<ComprobanteCarteraDto>
                {
                    IsSuccess = true,
                    ReturnMessage = "Comprobante Creado: " + +response.Comprobante_Id,
                    Data = response
                };

            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "Error al generar comprobante." };
            }
        }

        private async Task<IEnumerable<MovimientoComprobanteDto>> MapearMovimientosComprobante(IEnumerable<ReporteContableDto> MovReportes)
        {
            var movimientosComprobante = new List<MovimientoComprobanteDto>();

            foreach (var movimiento in MovReportes)
            {
                MovimientoComprobanteDto movimientoComprobante = new MovimientoComprobanteDto
                {
                    Cuenta_Contable = Convert.ToInt32(movimiento.CuentaNif),
                    Descripcion_Cuenta = movimiento.DescripcionCuenta,
                    Centro_Costo = 0,
                    Detalle = 10,
                    Tipo = movimiento.Credito > 0 ? "C" : "D",
                    Monto = movimiento.Credito > 0 ? movimiento.Credito : movimiento.Debito
                };
                movimientosComprobante.Add(movimientoComprobante);
            }

            return movimientosComprobante;
        }

        #endregion
        #region ComprobantesCartera

        private async Task<IEnumerable<MovimientoComprobanteDto>> MapearMovimientosInteresesMoraComprobante(IEnumerable<ReporteContableDto> MovReportes)
        {
            var movimientosComprobante = new List<MovimientoComprobanteDto>();

            foreach (var movimiento in MovReportes)
            {
                MovimientoComprobanteDto movimientoComprobante = new MovimientoComprobanteDto
                {
                    Cuenta_Contable = Convert.ToInt32(movimiento.CuentaNif),
                    Descripcion_Cuenta = movimiento.DescripcionCuenta,
                    Centro_Costo = 0,
                    Detalle = 10,
                    Tipo = movimiento.Credito > 0 ? "C" : "D",
                    Monto = movimiento.Credito > 0 ? movimiento.Credito : movimiento.Debito
                };
                movimientosComprobante.Add(movimientoComprobante);
            }

            return movimientosComprobante;
        }

        #endregion


        #region BalanceCartera

        public async Task<Response<object>> GenerarBalanceCartera(
    BalanceCarteraDto request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.FechaCorte))
                    return new Response<object> { IsSuccess = false, ReturnMessage = "La fecha de corte no puede ser nula." };

                var fechaCorte = DateTime.ParseExact(request.FechaCorte, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);

                var fechaInicial = new DateTime(fechaCorte.Year, fechaCorte.Month, 1);
                var fechaFinal = fechaInicial.AddMonths(1).AddDays(-1);

                var requestComprobantes = new ObtenerComprobantePorProcesoDto
                {
                    FechaInicio = fechaInicial.ToString("yyyy-MM-dd"),
                    FechaFinal = fechaFinal.ToString("yyyy-MM-dd"),
                };

                // 1️⃣ Comprobantes aprobados — igual que el auxiliar
                var listaComprobantes = (await _carteraRepository.ObtenerComprobanteCartera(requestComprobantes, 1))
                    .Where(x => x.Comprobante_Seven_Id != null && x.Estado == "Aprobado")
                    .ToList();

                // 2️⃣ Movimientos — misma fuente que el auxiliar
                var resultadosProcesados = new List<MovimientoComprobantePorProcesoDto>();

                foreach (var comprobante in listaComprobantes)
                {
                    var movimientos = await ObtenerMovimientosContableCarteraTotal(
                        (Guid)comprobante.Detalle_Movimiento_Id, cancellationToken);

                    if (movimientos != null && movimientos.Any())
                        resultadosProcesados.AddRange(movimientos);
                    else
                        Console.WriteLine(movimientos);
                }
                // 1️⃣ Obtener cuentas de cartera (Leasing + Hipotecarios)
                var cuentaLeasing = await _carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByTipoCredito(1);
                var cuentaHipotecarios = await _carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByTipoCredito(2);

                var cuentasCartera = cuentaLeasing
                    .Concat(cuentaHipotecarios)
                    .Select(c => c.CUENTA_CONTABLE_SEVEN)  // ← ajusta el nombre de la propiedad según tu DTO
                    .ToHashSet();

                // 2️⃣ Movimientos agrupados por cuenta
                var resumenMovimientos = resultadosProcesados
                    .GroupBy(m => m.Cuenta_Contable)
                    .Select(g => new
                    {
                        Cuenta_Contable = g.Key,
                        Descripcion = g.FirstOrDefault()?.Descripcion_Cuenta ?? "",
                        CuentasNif = g.FirstOrDefault()?.Cuenta_NIF ?? "",
                        Debito = g.Sum(x => x.Debito),
                        Credito = g.Sum(x => x.Credito)
                    }).ToList();

                // 3️⃣ Saldo Inicial desde BLC
                IEnumerable<BalanceCarteraPorPeriodoDto> resultadoSaldos =
                    _mapper.Map<IEnumerable<BalanceCarteraPorPeriodoDto>>(
                        await _carteraRepository.ConsultarBalancePorFecha(requestComprobantes));

                // Cuentas fijas de cartera que siempre deben aparecer aunque no tengan movimientos
                var cuentasFijasCartera = new HashSet<string>
{
    "25909503",
    "25909519",
    "25909525",
    "25909526",
    "25909527",
    "25909528",
    "41981501",
    "61050003",
    "62200002",
    "8195950702",
    "83050002" };
                // 4️⃣ Cuentas CON movimientos
                var resultadoMov = resumenMovimientos.Select(mov =>
                {
                    var saldo = resultadoSaldos.FirstOrDefault(s => s.CuentasNif == mov.CuentasNif);
                    var saldoInicial = saldo?.SaldoInicial ?? 0;
                    return new
                    {
                        id = (saldo?.Id ?? Guid.NewGuid()).ToString(),
                        periodo = saldo?.Periodo ?? fechaInicial,
                        cuentasNif = saldo?.CuentasNif,
                        cuentaContable = mov.Cuenta_Contable,
                        descripcionCuenta = saldo?.DescripcionCuenta ?? mov.Descripcion,
                        saldoInicial = saldoInicial,
                        debitos = mov.Debito,
                        creditos = mov.Credito,
                        saldoFinal = saldoInicial + mov.Debito - mov.Credito
                    };
                }).ToList();
                var validatecionCuenta = resultadoSaldos.Where(s => cuentasFijasCartera.Contains(s.CuentaContable));
                var nifConMovimientos = resumenMovimientos
       .Select(m => m.CuentasNif)
       .ToHashSet();

                var cuentasSinMovimientos = resultadoSaldos
                    .Where(s => !nifConMovimientos.Contains(s.CuentasNif))
                    .GroupBy(s => s.CuentasNif)
                    .Select(g => g.First())
                    .Select(s => new
                    {
                        id = s.Id.ToString(),
                        periodo = s.Periodo,
                        cuentasNif = s.CuentasNif,
                        cuentaContable = s.CuentaContable,
                        descripcionCuenta = s.DescripcionCuenta,
                        saldoInicial = s.SaldoInicial,
                        debitos = (decimal)0,
                        creditos = (decimal)0,
                        saldoFinal = s.SaldoInicial
                    })
                    .OrderBy(x => x.cuentaContable)
                    .ToList();
                resultadoMov.AddRange(cuentasSinMovimientos);
                return new Response<object>
                {
                    IsSuccess = true,
                    ReturnMessage = "Balance Generado",
                    Data = resultadoMov
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error al generar Balance: {ex.Message}"
                };
            }
        }


        public async Task<FileResult> GenerarAuxiliarContableCartera(BalanceCarteraDto request, CancellationToken cancellationToken)
        {
            List<int> codigosOperacion = new();
            List<ComprobanteCarteraPorProcesoDTO> listaIerar = new();
            ReporteContableParametersDto parametros = new ReporteContableParametersDto();
            List<ComprobanteCarteraPorProcesoDTO> listaComprobantes = new();
            List<ComprobanteCarteraPorProcesoDTO> listaComprobantesTotal = new();

            var fechaCorteString = request.FechaCorte;
            //if (string.IsNullOrWhiteSpace(fechaCorteString))
            //    return new Response<object> { IsSuccess = false, ReturnMessage = "La fecha de corte no puede ser nula." };
            var fechaCorte = DateTime.ParseExact(fechaCorteString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var fechaInicial = new DateTime(fechaCorte.Year, fechaCorte.Month, 1);
            var fechaFinal = fechaInicial.AddMonths(1).AddDays(-1);

            var requestComprobantes = new ObtenerComprobantePorProcesoDto
            {
                FechaInicio = fechaInicial.ToString("yyyy-MM-dd"),
                FechaFinal = fechaFinal.ToString("yyyy-MM-dd"),
            };
            // Obtener todos los comprobantes
            listaComprobantesTotal = (await _carteraRepository.ObtenerComprobanteCartera(requestComprobantes, 1)).ToList();

            listaComprobantes = listaComprobantesTotal
                .Where(x => x.Comprobante_Seven_Id != null && x.Estado == "Aprobado") // Filtrar los elementos donde Comprobante_Seven_Id no sea null
                .ToList();

            var resultadosProcesados = new List<MovimientoComprobantePorProcesoDto>();

            foreach (var comprobante in listaComprobantes)
            {
                var movimientosComprobante = await ObtenerMovimientosContableCarteraTotal((Guid)comprobante.Detalle_Movimiento_Id, cancellationToken);

                if (movimientosComprobante != null && movimientosComprobante.Any())
                {
                    foreach (var movimiento in movimientosComprobante)
                    {
                        movimiento.Comprobante_Interno = comprobante.Comprobante_Id;
                        movimiento.Tipo_Comprobante = comprobante.Tipo_Comprobante;
                        movimiento.Comprobante_Contable = comprobante.Comprobante_Seven_Id;
                    }

                    resultadosProcesados.AddRange(movimientosComprobante);
                }
            }

            var columnsInfo = new ColumnHeaderInfo[]
            {
    "Tipo Operación",
    "Cuenta contable",
    "Cuenta NIF",                      // ← agregar
    "Descripción de la cuenta",
    "Número Identificación",
    "Nombre",
    "Fecha Creación",
    "Fecha Transaccion",
    "Detalle",
    "Tipo Comprobante",
    "Comprobante Interno",
    "Comprobante Seven",
    "Numero Crédito",
    "Tipo Crédito",
    "Centro de costo",
    "Area",
    ("Débito", ColumnFormatType.Decimal),
    ("Crédito", ColumnFormatType.Decimal),
            };

            try
            {
                if (resultadosProcesados == null || !resultadosProcesados.Any())
                {
                    throw new Exception("No se pudo obtener los datos para generar el reporte.");
                }
                // Crear un diccionario para buscar fechas por comprobante
                var fechasPorComprobante = listaComprobantes.ToDictionary(
                c => c.Comprobante_Id,
                c => c
            );

                var totalDebito = resultadosProcesados.Sum(x => x.Debito);
                var totalCredito = resultadosProcesados.Sum(x => x.Credito);
                var data = resultadosProcesados.Select(x =>
                {
                    return new string[]
                    {
                    x.TIPO_OPERACION,
                    x.Cuenta_Contable,
                    x.Cuenta_NIF ?? "",                                          // ← agregar
                    x.Descripcion_Cuenta,
                    x.Numero_Identificacion,
                    x.Nombre_Tercero,
                        listaComprobantes[0].Fecha_Comprobante.ToString("yyyy-MM-dd"),
                        listaComprobantes[0].Fecha_Transaccion.ToString("yyyy-MM-dd"),               // ✅ fecha correcta
                    x.Detalle,
                    x.Tipo_Comprobante,
                    x.Comprobante_Interno.ToString(),
                    x.Comprobante_Contable.ToString(),
                    x.Numero_Credito,
                    x.Tipo_Credito,
                    x.Centro_Costo,
                    x.Unidad_Ejecutora,
                    x.Debito.ToString(CultureInfo.InvariantCulture),
                    x.Credito.ToString(CultureInfo.InvariantCulture)
                    };
                }).ToList();

                //  Fila de totales — agregar celda vacía para la nueva columna NIF
                var totalRow = new string[]
                {
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                "Total",
                totalDebito.ToString(CultureInfo.InvariantCulture),
                    totalCredito.ToString(CultureInfo.InvariantCulture)
                };
                data.Add(totalRow);
                var finalData = data.ToArray();
                var fileResult = await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatDto(columnsInfo, finalData));
                return fileResult;
            }
            catch (Exception ex)
            {
                if (resultadosProcesados == null || !resultadosProcesados.Any())
                {
                    throw new Exception("No existen comprobantes APROBADOS para el período seleccionado.");
                }
                else
                {
                    throw new Exception("No se pudo generar el reporte", ex);
                }
            }
        }

        #endregion

        public async Task<Response<string>> ActualizarPlanPagosMasivo(ActualizarPlanPagoRequestDto request)
        {
            using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream);

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            var filas = worksheet.RangeUsed().RowsUsed().Skip(1);
            var registros = new List<ActualizacionPlanPago>();

            foreach (var fila in filas)
            {
                registros.Add(new ActualizacionPlanPago
                {
                    NumeroCredito = fila.Cell(1).GetString(),
                    Cedula = fila.Cell(2).GetString(),
                    PorcentajeSeguroVida = (decimal)fila.Cell(3).GetDouble(),
                    ValorSeguroHogar = (decimal)fila.Cell(4).GetDouble()
                });
            }
            if (request.TipoCredito == 2)
            {
                foreach (var item in registros)
                {
                    await UpdatePlanPagoHipotecario(item, request.TipoCredito, request.FechaCorte);
                }
            }


            return new Response<string>() { Data = "Proceso Finalizado", IsSuccess = true, ReturnMessage = "Ok" };
        }
        public static string NormalizarDepartamento(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return string.Empty;

            var texto = nombre.ToUpperInvariant().Trim();

            texto = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in texto)
            {
                if (Char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb
                .ToString()
                .Replace(",", "")
                .Replace(".", "")
                .Trim();
        }
        public async Task<double> ObtenerRegionPorDepartamento(string nombreCiudad)
        {
            var departamento = await _catalogsRepository.ObteneDepartamentoPorCodigoCiudad(nombreCiudad);
            var parametizacionTasa = await _creditoRepository.ObtenerParametrosporSeguroRegion();
            switch (departamento.DPD_NOMBRE)
            {
                case "BOGOTÁ, D.C.":
                case "CUNDINAMARCA":
                    return parametizacionTasa.Where(x => x.REG_ID == 1).Select(x => x.REG_SEGURO).FirstOrDefault();

                case "ATLÁNTICO":
                case "BOLÍVAR":
                case "CESAR":
                case "CÓRDOBA":
                case "LA GUAJIRA":
                case "MAGDALENA":
                case "SUCRE":
                    return parametizacionTasa.Where(x => x.REG_ID == 2).Select(x => x.REG_SEGURO).FirstOrDefault();

                // RESTO DEL PAÍS
                default:
                    return parametizacionTasa.Where(x => x.REG_ID == 3).Select(x => x.REG_SEGURO).FirstOrDefault();
            }
        }


        public async Task<Response<string>> UpdatePlanPagoHipotecario(ActualizacionPlanPago request, int tipoCredito, DateTime fechaCorte)
        {
            ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
            {
                Nro_Credito = long.Parse(request.NumeroCredito),
                Tcr_Id = tipoCredito
            };
            InformacionCredito infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);
            ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;

            if (responsePlanPagoDto.PlanPago.Count > 0 && infoCredito != null)
            {
                var datePayment = responsePlanPagoDto.PlanPago.Where(x => x.Fecha.Year == fechaCorte.Year && x.Fecha.Month < fechaCorte.Month).ToList();
                var applyUpdatePlan = responsePlanPagoDto.PlanPago.Where(x => x.Fecha.Year == fechaCorte.Year && x.Fecha.Month == fechaCorte.Month).FirstOrDefault();
                infoCredito.CRE_TASA_SEGURO_VIDA = (float)request.PorcentajeSeguroVida * 100;
                var valueSeguroHogar = await ObtenerRegionPorDepartamento(infoCredito.DPC_NOMBRE);
                infoCredito.CRE_TASA_SEGURO_HOGAR = (float)valueSeguroHogar;
                var dataHipotecario = await UpdatePlanTasaVidaHipotecario(infoCredito, applyUpdatePlan.Cuota, datePayment.LastOrDefault(), request.ValorSeguroHogar);
                await ActualizarPlanPagosMasivoOptimizado(dataHipotecario, long.Parse(request.NumeroCredito), tipoCredito);
                var cuota = dataHipotecario.LastOrDefault().Cuota;
                var cuotaCalculada = infoCredito.CRE_PLAZO_ACTUAL + infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                var cuotaPendiente = cuotaCalculada - cuota;
                var plazoActual = infoCredito.CRE_PLAZO_ACTUAL - cuotaPendiente;
                await _carteraRepository.ActualizarPorcentajeSeguro(infoCredito.CRE_TASA_SEGURO_VIDA, infoCredito.CRE_TASA_SEGURO_HOGAR, long.Parse(request.NumeroCredito), cuota, plazoActual);
            }
            return new Response<string>() { Data = "Proceso Finalizado", IsSuccess = true, ReturnMessage = "Ok" };
        }
        public async Task<Response<string>> RecalcularPlanPagoHipotecario(ActualizacionPlanPago request, int tipoCredito, DateTime fechaCorte)
        {
            ObtenerProductoCartera obtenerProductoCartera = new ObtenerProductoCartera()
            {
                Nro_Credito = long.Parse(request.NumeroCredito),
                Tcr_Id = tipoCredito
            };
            InformacionCredito infoCredito = await this._carteraRepository.ObtenerCreditoCartera(obtenerProductoCartera);
            ResponsePlanPagoDto responsePlanPagoDto = (await ObtenerPlanPagosCredito(obtenerProductoCartera)).Data;

            if (responsePlanPagoDto.PlanPago.Count > 0 && infoCredito != null)
            {
                var datePayment = responsePlanPagoDto.PlanPago.Where(x => x.Fecha.Year == fechaCorte.Year && x.Fecha.Month < fechaCorte.Month).ToList();
                var applyUpdatePlan = responsePlanPagoDto.PlanPago.Where(x => x.Fecha.Year == fechaCorte.Year && x.Fecha.Month == fechaCorte.Month).FirstOrDefault();
                var valueSeguroHogar = await ObtenerRegionPorDepartamento(infoCredito.DPC_NOMBRE);
                var dataHipotecario = await ReCalcularPlanTasaVidaHipotecario(infoCredito, applyUpdatePlan.Cuota, datePayment.LastOrDefault());
                await ActualizarPlanPagosMasivoOptimizado(dataHipotecario, long.Parse(request.NumeroCredito), tipoCredito);
                var cuota = dataHipotecario.LastOrDefault().Cuota;
                var cuotaCalculada = infoCredito.CRE_PLAZO_ACTUAL + infoCredito.CRE_NUMERO_CUOTAS_CANCELADAS;
                var cuotaPendiente = cuotaCalculada - cuota;
                var plazoActual = infoCredito.CRE_PLAZO_ACTUAL - cuotaPendiente; }
            return new Response<string>() { Data = "Proceso Finalizado", IsSuccess = true, ReturnMessage = "Ok" };
        }

        public async Task<Response<string>> ActualizarPlanPagosMasivoOptimizado(
            List<Hipotecario> dataHipotecario,
            long numeroCredito,
            int tipoCredito)
        {
            try
            {
                var dt = CrearDataTablePlanPagos();

                foreach (var item in dataHipotecario)
                {
                    dt.Rows.Add(
                        numeroCredito,
                        item.Cuota,
                        item.Capital,
                        item.Intereses,
                        item.Seguro_Vida,
                        item.Seguro_Hogar,
                        item.Cuota_Pagadora,
                        item.Saldo,
                        item.Fecha_Pago

                    );
                }

                await _carteraRepository.ActualizarPlanPagoMasivos(dt, tipoCredito);

                return new Response<string>
                {
                    Data = "Proceso Finalizado",
                    IsSuccess = true,
                    ReturnMessage = "Ok"
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = ex.Message
                };
            }
        }
        private DataTable CrearDataTablePlanPagos()
        {
            var dt = new DataTable();
            dt.Columns.Add("PLN_NUMERO_CREDITO", typeof(double));
            dt.Columns.Add("PLN_NUMERO_CUOTA", typeof(int));
            dt.Columns.Add("PLN_CAPITAL", typeof(decimal));
            dt.Columns.Add("PLN_INTERESES", typeof(decimal));
            dt.Columns.Add("PLN_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("PLN_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("PLN_A_PAGAR", typeof(decimal));
            dt.Columns.Add("PLN_SALDO_INICIAL", typeof(decimal));
            dt.Columns.Add("PLN_FECHA_PAGO", typeof(DateTime));
            return dt;
        }


    }
    #endregion
}
