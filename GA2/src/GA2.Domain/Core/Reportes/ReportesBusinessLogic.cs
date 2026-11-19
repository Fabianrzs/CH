using AspNetCore.Reporting;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Application.Main;
using GA2.Domain.Core.Reportes.ReportesXMlClosed;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LocalReport = AspNetCore.Reporting.LocalReport;
using GA2.Application.Dto.Credito.TU.Responses;
using ClosedXML.Excel;
using GA2.Application.Main.TU;
using GA2.Application.Dto.Credito;
using System.IO.Compression;
using Microsoft.Reporting.NETCore;
using GA2.Domain.Core.Embargos.GestorDocumental;
using GA2.Application.Dto.Credito.TU;
using GA2.Domain.Entities.Credito.TransUnion;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Application.Dto.Cartera.Reportes.ResumenConceptosFinancieros;
using GA2.Application.Dto.Haberes;
using GA2.Domain.Entities.ValidacionTelAfiliado;
using GA2.Infraestructure.Repositories.ValidacionTelAfiliado;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Transversals.Commons.Extensions;
using SelectPdf;
using Humanizer;
using GA2.Domain.Entities.Cartera.Reportes;
using System.Threading;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using GA2.Application.Dto.Cartera;
using PdfSharpIO = PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using Microsoft.ReportingServices.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Catalogs;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Application.Dto.Solicitud;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using DocumentFormat.OpenXml.Drawing;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Entities.Credito;
using System.Configuration;
using Newtonsoft.Json;
using Path = System.IO.Path;
using GA2.Infraestructure.Repositories.ServiciosDinamicosRep;
using GA2.Domain.Entities.ServiciosDinamicosEnt;
using static Org.BouncyCastle.Math.EC.ECCurve;
using GA2.Domain.Core.Postulacion;
using GA2.Application.Dto.Postulacion;
using BitMiracle.LibTiff.Classic;
using GA2.Domain.Entities.Catalogs;
using System.Drawing;
using Pais = GA2.Domain.Entities.Pais;
using GA2.Infraestructure.Repositories.Escrituras;
using GA2.Domain.Entities.Escritura;
using GA2.Infraestructure.Repositories.InfoVendedor;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Entities.InfoVendedor;
using Departamento = GA2.Domain.Entities.Departamento;
using Azure.Storage;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Domain.Core.Credito.WorkManager;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito.Documento;
using DocumentFormat.OpenXml.Wordprocessing;
using GA2.Domain.Entities.Cartera.Novedades;
using System.ComponentModel;
using SharpCompress;
using GA2.Domain.Core.Dodo.Forms.Services;
using GA2.Domain.Core.Dodo.Forms.Models;
using GA2.Domain.Core.Dodo.Common;
using GA2.Infraestructure.Repositories.EmailNotificacion;
using GA2.Domain.Core.Reintegros.GestorDocumental;
using GA2.Infraestructure.Data.Credito;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using SixLabors.Fonts.Tables.TrueType;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;

namespace GA2.Domain.Core
{
    public class ReportesBusinessLogic : ProviderDocumentos, IReportesBusinessLogic
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ReportesBusinessLogic> _logger;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportesBusinessLogic> logger;
        private readonly IOptions<AqmOptions> _AqmOptions;
        private readonly IReportesRepository _reportesRepository;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly IReportesRequestProvider _iReportesRequestProvider;
        private readonly IEmbargosRepository _juzgadosRepository;
        private readonly ICreditoRepository _creditoRepository;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly ICarteraRepository _carteraRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ITemplateExcel _templateExcel;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        private readonly IGestorDocumentarEmbargosBL _iGestorDocumentarEmbargosBL;
        private readonly IGestorDocumentalReintegrosBL _iGestorDocumentalReintegrosBL;
        private readonly IClienteRepository _clientsRepository;
        private readonly ITransUnionRepository _transUnionRepository;
        private readonly IReportesExcelInvariantCultureLogic _reportesExcel;
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IValidacionTelAfiliadoRepository _validacionTelAfiliadoRepository;
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        private readonly IGestorTransUnionBusinessLogic _gestorTransUnionBusinessLogic;
        private readonly ICuentasClientesBusinessLogic _cuentasClientesBusinessLogic;
        private readonly ITemplateExcelReporteRCredito _templateExcelReporteRCredito;
        private readonly IUsersRepository _usersRepository;
        private readonly LocalBlogStorageConfig _configureOptions;
        private readonly IServiciosDinamicosRepository _serviciosDinamicosRepository;
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private readonly IEscriturasRepository _escrituraRepository;
        private readonly IInformacionVendedorRepository _informacionVendedorRepository;
        private readonly IGestorDocumentalBusinessLogic _gestorDocumentalBusinessLogic;
        private readonly ICarteraBusinessLogic _carteraBusinesLogic;
        private readonly IDodoFormsService _dodoFormsService;
        private readonly IConsumoWorkManager _workFormsService;
        private readonly IEmailNotificacionRepository _emailNotificacionRepository;




        public ReportesBusinessLogic(IWebHostEnvironment webHostEnvironment,
                                        
                                     IOptions<BlobStorageOptions> options,
                                     IMapper mapper,
                                     ILogger<ReportesBusinessLogic> logger,
                                     IOptions<AqmOptions> AqmOptions,
                                     IReportesRepository reportesRepository,
                                     ICatalogosRepository catalogsRepository,
                                     IClientRequestProvider clientRequestProvider,
                                     IReportesRequestProvider iReportesRequestProvider,
                                     IEmbargosRepository juzgadosRepository,
                                     ITransUnionRepository transUnionRepository,
                                     ISolicitudBusinessLogic solicitudBusinessLogic,
                                     ISolicitudRepository solicitudRepository,
                                     ICreditoRepository creditoRepository,
                                     IClientRequestProvider iClientRequestProvider,
                                     ICarteraRepository carteraRepository,
                                     ITemplateExcel templateExcel,
                                     ICarteraBusinessLogic carteraBusinessLogic,
                                     IGestorDocumentarEmbargosBL iGestorDocumentarEmbargosBL,
                                     IGestorDocumentalReintegrosBL iGestorDocumentalReintegrosBL,
                                     IClienteRepository clienteRepository,
                                     IReportesExcelInvariantCultureLogic reportesExcel,
                                     ICreditoBusinessLogic creditoBusinessLogic,
                                     IValidacionTelAfiliadoRepository validacionTelAfiliadoRepository,
                                     ICatalogosBusinessLogic catalogsBusinessLogic,
                                     IGestorTransUnionBusinessLogic gestorTransUnionBusinessLogic,
                                     ICuentasClientesBusinessLogic cuentasClientesBusinessLogic,
                                     ITemplateExcelReporteRCredito templateExcelReporteRCredito,
                                     IUsersRepository usersRepository,
                                     IOptions<LocalBlogStorageConfig> configureOptions,
                                     IServiciosDinamicosRepository serviciosDinamicosRepository,
                                     IPostulacionBussineLogic postulacionBussineLogic,
                                     IEscriturasRepository escrituraRepository,
                                     IInformacionVendedorRepository informacionVendedorRepository,
                                     IGestorDocumentalBusinessLogic gestorDocumentalBusinessLogic,
                                     ICarteraBusinessLogic carteraBusinesLogic,
                                     IDodoFormsService dodoFormsService,
                                     IConsumoWorkManager workFormsService,
            IEmailNotificacionRepository emailNotificacionRepository

                                     )
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger;
            _solicitudRepository = solicitudRepository;
            _AqmOptions = AqmOptions;
            _transUnionRepository = transUnionRepository;
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _clientsRepository = clienteRepository;
            _clientRequestProvider = clientRequestProvider;
            _reportesRepository = reportesRepository ?? throw new ArgumentNullException(nameof(reportesRepository));
            _catalogsRepository = catalogsRepository ?? throw new ArgumentNullException(nameof(catalogsRepository));
            _iReportesRequestProvider = iReportesRequestProvider ?? throw new ArgumentNullException(nameof(iReportesRequestProvider));
            _juzgadosRepository = juzgadosRepository ?? throw new ArgumentNullException(nameof(juzgadosRepository));
            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
            _iClientRequestProvider = iClientRequestProvider ?? throw new ArgumentNullException(nameof(iClientRequestProvider));
            _carteraRepository = carteraRepository ?? throw new ArgumentNullException(nameof(carteraRepository));
            _templateExcel = templateExcel ?? throw new ArgumentNullException(nameof(templateExcel));
            _carteraBusinessLogic = carteraBusinessLogic ?? throw new ArgumentNullException(nameof(carteraBusinessLogic));
            _iGestorDocumentarEmbargosBL = iGestorDocumentarEmbargosBL;
            _iGestorDocumentalReintegrosBL = iGestorDocumentalReintegrosBL;
            _reportesExcel = reportesExcel;
            _creditoBusinessLogic = creditoBusinessLogic;
            _validacionTelAfiliadoRepository = validacionTelAfiliadoRepository;
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _gestorTransUnionBusinessLogic = gestorTransUnionBusinessLogic;
            _cuentasClientesBusinessLogic = cuentasClientesBusinessLogic;
            _templateExcelReporteRCredito = templateExcelReporteRCredito;
            _usersRepository = usersRepository;
            _configureOptions = configureOptions.Value;
            _serviciosDinamicosRepository = serviciosDinamicosRepository;
            _postulacionBussineLogic = postulacionBussineLogic;
            _escrituraRepository = escrituraRepository;
            _informacionVendedorRepository = informacionVendedorRepository;
            _gestorDocumentalBusinessLogic = gestorDocumentalBusinessLogic;
            _carteraBusinesLogic = carteraBusinesLogic;
            _dodoFormsService = dodoFormsService;
            _workFormsService = workFormsService;
            _emailNotificacionRepository = emailNotificacionRepository;


        }

        private async Task<IEnumerable<InformacionContableDto>> ObtenerInformacionContable(ReporteInformacionContableParametersDto parametersDto)
        {
            var report = _mapper.Map<IEnumerable<InformacionContableDto>>(await _reportesRepository.ReporteInformacionContable(parametersDto)).ToList();
            var cliIdentifications = report.Where(x => x.Tercero == "AFILIADO").Select(x => x.NumeroIdentificacion).Distinct().ToList();
            var afiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(cliIdentifications))?.Data ?? new();
            var groupById = report.Where(x => x.Tercero == "AFILIADO").GroupBy(x => x.NumeroIdentificacion);

            foreach (var item in groupById)
            {
                var name = afiliados.FirstOrDefault(x => x.Documento == item.Key)?.NombreCompleto ?? "";

                foreach (var info in item)
                    info.NombreTercero = name;
            }

            return report;
        }

        public async Task<Response<IEnumerable<InformacionContableDto>>> ObtenerReporteInformacionContable(ReporteInformacionContableParametersDto parametersDto)
        {
            var report = await ObtenerInformacionContable(parametersDto);
            return new Response<IEnumerable<InformacionContableDto>> { Data = report };
        }

        public async Task<Response<IEnumerable<EquivalenciaCuentasContableSevenDto>>> ObtenerEquivalenciaCuentasContables()
        {
            var result = _mapper.Map<IEnumerable<EquivalenciaCuentasContableSevenDto>>(await _reportesRepository.ObtenerEquivalenciaCuentasContables()).ToList();
            return new Response<IEnumerable<EquivalenciaCuentasContableSevenDto>> { Data = result };
        }

        public async Task<FileResult> GenerarExcelReporteInformacionContable(ReporteInformacionContableParametersDto parametersDto)
        {
            var report = await ObtenerInformacionContable(parametersDto);

            var columnsInfo = new ColumnHeaderInfo[]
            {
                new("Fecha Movimiento", ColumnFormatType.Fecha),
                new("Comprobante", ColumnFormatType.Numerico),
                new("Tipo Comprobante", ColumnFormatType.Numerico),
                "Detalle",
                "Movimiento",
                new("Valor", ColumnFormatType.Decimal),
                new("Saldo", ColumnFormatType.Decimal),
                "Concepto",
                "Tercero",
                "Tipo Identificación",
                "Número Identificación",
                "Nombre Tercero",
                "Número Cuenta Contable",
                "Nombre Cuenta Contable",
            };
            var data = report.Select(x =>
                new string[]
                {
                    x.FechaMovimiento.ToString("yyyy/MM/dd"),
                    x.Comprobante?.ToString() ?? "",
                    x.TipoComprobante.ToString(),
                    x.Detalle,
                    x.Movimiento,
                    x.Valor.ToString(CultureInfo.InvariantCulture),
                    x.Saldo?.ToString(CultureInfo.InvariantCulture),
                    x.Concepto,
                    x.Tercero,
                    x.TipoIdentificacion,
                    x.NumeroIdentificacion,
                    x.NombreTercero,
                    x.NumeroCuentaContable,
                    x.NombreCuentaContable,
                }
            ).ToArray();

            return await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatDto(columnsInfo, data));
        }


        public async Task<IEnumerable<ReporteContableDto>> ObtenerReporteCausaciones(ReporteContableParametersDto parametersDto)
        {
            var list = new List<ReporteContableDto>();

            await foreach (var item in await _reportesRepository.GenerarReporteCausaciones(parametersDto))
                list.Add(_mapper.Map<ReporteContableDto>(item));

            return list;
        }

        private async IAsyncEnumerable<string[]> DataExcelReporteCausaciones(ReporteContableParametersDto parametersDto)
        {
            await foreach (var item in await _reportesRepository.GenerarReporteCausaciones(parametersDto))
            {
                var data = _mapper.Map<ReporteContableDto>(item);

                yield return new string[]
                {
                    data.TipoOperacion.ToString(),
                    data.DescripcionOperacion,
                    data.FechaOperacion.ToString("yyyy/MM/dd"),
                    data.NumeroIdentificacion,
                    data.NumeroCredito.ToString(),
                    data.CuentaNif,
                    data.CuentaSeven,
                    data.ComprobanteInterno,
                    data.DescripcionCuenta,
                    data.Debito.ToString(CultureInfo.InvariantCulture),
                    data.Credito.ToString(CultureInfo.InvariantCulture),
                };
            }
        }

        private async Task<IEnumerable<ReporteContableDto>> ObtenerReporteContablePagos(ReporteContableParametersDto parametersDto)
        {
            var report = _mapper.Map<IEnumerable<ReporteContableDto>>(await _reportesRepository.GenerarReporteContablePagos(parametersDto)).ToList();
            return report;
        }

        private async Task<IEnumerable<ReportePagoDto>> ObtenerReportePagos(ReportePagosParametersDto parametersDto)
        {
            var report = _mapper.Map<IEnumerable<ReportePagoDto>>(await _reportesRepository.GenerarReportePagos(parametersDto)).ToList();
            return report;
        }

        public async Task<Response<ResultadoReportContable>> GenerarReporteCausaciones(ReporteContableParametersDto parametersDto)
        {
            try
            {
                var report = await ObtenerReporteCausaciones(parametersDto);

                ComprobanteCarteraDto comprobante = new ComprobanteCarteraDto();
                comprobante.Fecha_Comprobante = parametersDto.FechaFinal;

                return new() { Data = new(report) };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
            }
        }




        public async Task<Response<ResumenReporteContable>> GenerarReporteTotalCausaciones(ReporteContableParametersDto parametersDto)
        {
            try
            {
                var totalCausacion = await _reportesRepository.GenerarReporteTotalCausaciones(parametersDto);
                return GenerarResumenReporteCausacion(totalCausacion).BuildSuccess();
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
            }
        }

        private ResumenReporteContable GenerarResumenReporteCausacion(IEnumerable<TotalReporteContable> reporte)
        {
            if (reporte is null)
                return new();

            var causacionPorTipoCredito = reporte.Select(item =>
                 new ResumenReporteContablePorTipoCredito()
                 {
                     IdTipoCredito = item.TCR_ID,
                     CodigoTipoCredito = item.TCR_DESCRIPCION?.ToLower()?.Pascalize(),
                     TipoCredito = item.TCR_DESCRIPCION?.ToLower()?.Titleize(),
                     TotalCreditos = item.TOTAL_CREDITOS,
                     TotalValorDebito = Math.Round(item.DEBITO, 2),
                     TotalValorCredito = Math.Round(item.CREDITO, 2)
                 });

            return new()
            {
                TotalCreditos = causacionPorTipoCredito.Sum(x => x.TotalCreditos),
                TotalValorDebito = causacionPorTipoCredito.Sum(x => x.TotalValorDebito),
                TotalValorCredito = causacionPorTipoCredito.Sum(x => x.TotalValorCredito),
                TotalPorTipoCredito = causacionPorTipoCredito,
            };
        }

        public async Task<Response<IEnumerable<ReporteContableDto>>> GenerarReporteContablePagos(ReporteContableParametersDto parametersDto)
        {
            try
            {
                return new() { Data = await ObtenerReporteContablePagos(parametersDto) };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
            }
        }

        private ResumenReportePagos GenerarResumenReportePago(IEnumerable<ReportePagoDto> reporte, IEnumerable<TipoCredito> tiposCreditos)
        {
            if (reporte is null)
                return new();

            IEnumerable<ResumenReportePorTipoCredito> pagosPorTipoCredito;

            var pagosPorUnidad = reporte
                .GroupBy(x => x.IdUnidadEjecutora)
                .Select(pagos => new ResumenReportePorUnidadEjecutora()
                {
                    UnidadEjecutora = pagos.FirstOrDefault()?.UnidadEjecutora ?? "SIN UNIDAD EJECUTORA",
                    TiposCredito = tiposCreditos.GroupJoin(pagos, x => x.TCR_ID, x => x.IdTipoCredito, (tipo, pagos) =>
                    new ResumenReportePorTipoCredito()
                    {
                        IdTipoCredito = tipo.TCR_ID,
                        CodigoTipoCredito = tipo.TCR_DESCRIPCION?.Pascalize(),
                        TipoCredito = tipo.TCR_DESCRIPCION,
                        TotalCreditos = pagos.Select(x => x.NumeroCredito).Distinct().Count(),
                        TotalValor = pagos.Sum(x => x.ValorPago)
                    })
                });

            if (pagosPorUnidad.Any())
            {
                pagosPorTipoCredito = pagosPorUnidad
                    .SelectMany(t => t.TiposCredito)
                    .GroupBy(t => t.IdTipoCredito)
                    .Select(tipo => new ResumenReportePorTipoCredito()
                    {
                        IdTipoCredito = tipo.Key,
                        CodigoTipoCredito = tipo.FirstOrDefault()?.CodigoTipoCredito,
                        TipoCredito = tipo.FirstOrDefault()?.TipoCredito,
                        TotalCreditos = tipo.Sum(x => x.TotalCreditos),
                        TotalValor = tipo.Sum(x => x.TotalValor)
                    }).ToList();
            }
            else
            {
                pagosPorTipoCredito = tiposCreditos.Select(tipo => new ResumenReportePorTipoCredito()
                {
                    IdTipoCredito = tipo.TCR_ID,
                    CodigoTipoCredito = tipo.TCR_DESCRIPCION?.Pascalize(),
                    TipoCredito = tipo.TCR_DESCRIPCION,
                    TotalCreditos = 0,
                    TotalValor = 0
                });
            }

            return new()
            {
                TotalCreditos = pagosPorTipoCredito.Sum(x => x.TotalCreditos),
                TotalValor = pagosPorTipoCredito.Sum(x => x.TotalValor),
                TotalPorTipoCredito = pagosPorTipoCredito,
                TotalPorUnidadEjecutora = pagosPorUnidad
            };
        }

        public async Task<Response<ResultadoReportePagos>> GenerarReportePagos(ReportePagosParametersDto parametersDto)
        {
            try
            {
                var result = await ObtenerReportePagos(parametersDto);
                foreach (var item in result)
                {
                    var data = await _carteraBusinessLogic.ObtenerAplicacionPago(item.IdCredito);
                    var fechaInicial = parametersDto.FechaInicial?.Date;
                    var fechaFinal = parametersDto.FechaFinal?.Date.AddDays(1);
                    var filterData = data.Data
                        .Where(x => x.FechaPago >= fechaInicial
                                 && x.FechaPago < fechaFinal)
                        .ToList();
                    if (filterData.Count > 0)
                    {
                        foreach (var x in filterData)
                        {
                            if (item.TipoPago == x.ModalidadPago
                                && item.ValorPago == x.ValorPago
                                && item.FechaAplicacion == x.FechaAplicacion
                                && item.FechaPago == x.FechaPago)
                            {
                                item.TipoPago = x.TipoPago;
                                item.ModalidadPago = x.ModalidadPago == null || x.ModalidadPago == "" ? "N/A" : x.ModalidadPago;
                                item.ComprobanteInterno = x.ComprobanteInterno;
                                item.TipoRecurso = x.TipoRecurso;
                            }

                        }
                    }
                }
                var documentos = result.Select(x => x.Identificacion).Distinct().ToList();
                var afiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(documentos))?.Data;

                var tiposCredito = (await _catalogsRepository.ObtenerTipoCredito())
                    .Where(x => x.TCR_ID is 1 or 2)
                    .Select(x => { x.TCR_DESCRIPCION = x.TCR_DESCRIPCION?.ToLower()?.Titleize(); return x; })
                    .OrderBy(x => x.TCR_ID);

                var reportes = result.GroupJoin(afiliados, s => s.Identificacion, s => s.Identificacion, (rep, cli) =>
                {
                    var cliente = cli?.FirstOrDefault();
                    rep.Afiliado = cliente?.NombreCompletoInvertido ?? "";
                    return rep;
                }).ToList();

                return new() { Data = new(result, GenerarResumenReportePago(result, tiposCredito)) };
            }
            catch (Exception ex)
            {
                return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
            }
        }
        public async Task<Response<string>> GenerarPlanPagosHipotecarioPdfBase64Async(RequestProductoClienteDto modelo)
        {
            if (modelo is null)
            {
                throw new ArgumentNullException(nameof(modelo), "El modelo del Plan de Pagos no puede ser nulo.");
            }

            var rutaImagenes = new[]
            {
                Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "images", "ENCABEZADOIZ2026.png"),
                Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "images", "ENCABEZADO2026.png"),
                Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "images", "PIEDEPAGINA2022.png"),
            };

            var ciudades = await _catalogsRepository.ObtenerCiudades();
            modelo.CiudadInmueble = ciudades.Where(x => x.DPC_ID == int.Parse(modelo.CiudadInmueble)).FirstOrDefault().DPC_NOMBRE;
            var lineaCreditos = await _catalogsRepository.ObtenerLineasCredito(modelo.IdTipoCredito);
            string lineaCreditoTexto = (modelo.LineaCredito == "1" || modelo.LineaCredito == "2" || modelo.LineaCredito == "3"
                || modelo.LineaCredito == "4" || modelo.LineaCredito == "5")
                ? lineaCreditos.Where(x => x.CLC_ID == int.Parse(modelo.LineaCredito)).FirstOrDefault().CLC_DESCRIPCION
                : modelo.LineaCredito; 
            modelo.LineaCredito =lineaCreditoTexto;
            using var stream = new GA2.Domain.Core.TemplateReportesCartera().ReportePlanPagosHipotecario(modelo, rutaImagenes);
            var base64 = Convert.ToBase64String(stream.ToArray());


            return new Response<string> { Data = base64, IsSuccess = true, ReturnMessage = "Reporte generado con exito!" };
        }


        private static bool TryParsearPeriodo(string periodo, out int anio, out int mes)
        {
            anio = 0;
            mes = 0;

            if (string.IsNullOrWhiteSpace(periodo))
                return false;

            periodo = periodo.Trim();

            // Formatos "2025-11" o "2025/11"
            if (periodo.Contains("-") || periodo.Contains("/"))
            {
                var separador = periodo.Contains("-") ? '-' : '/';
                var partes = periodo.Split(separador);

                if (partes.Length == 2
                    && int.TryParse(partes[0], out var anioParseado)
                    && int.TryParse(partes[1], out var mesParseado)
                    && mesParseado >= 1 && mesParseado <= 12
                    && anioParseado > 1900)
                {
                    anio = anioParseado;
                    mes = mesParseado;
                    return true;
                }

                return false;
            }

            if (periodo.Length == 6 && int.TryParse(periodo, out _))
            {
                if (int.TryParse(periodo.Substring(0, 4), out var anioSinSep)
                    && int.TryParse(periodo.Substring(4, 2), out var mesSinSep)
                    && mesSinSep >= 1 && mesSinSep <= 12
                    && anioSinSep > 1900)
                {
                    anio = anioSinSep;
                    mes = mesSinSep;
                    return true;
                }
            }

            return false;
        }
        public async Task<Response<byte[]>> GenerarPlanesPagoMasivoAnualZipBase64Async(RequestPlanPagoGeneradoDto request)
        {
            // tipoCredito: 0 = todos, 1 = leasing, 2 = hipotecario
            if (request is null || string.IsNullOrWhiteSpace(request.Periodo))
            {
                return new Response<byte[]>
                {
                    IsSuccess = false,
                    ReturnMessage = "Debe indicar el periodo (año/mes)."
                };
            }

            if (!TryParsearPeriodo(request.Periodo, out var anio, out var mes))
            {
                return new Response<byte[]>
                {
                    IsSuccess = false,
                    ReturnMessage = $"El periodo '{request.Periodo}' no tiene un formato válido. Use 'AAAA-MM' (ej. 2025-11)."
                };
            }

            // >>> CAMBIO: ya no se buscan créditos "desembolsados en el periodo",
            // sino créditos que tuvieron pagos aplicados durante el año (@Anio),
            // sin importar cuándo se desembolsaron. El SP ya excluye pagos reversados.
            var creditosDelPeriodo = (await _carteraRepository.ObtenerCreditosConPagosEnAnio(anio, request.TipoCredito)).ToList();

            if (creditosDelPeriodo == null || !creditosDelPeriodo.Any())
            {
                return new Response<byte[]>
                {
                    IsSuccess = false,
                    ReturnMessage = $"No se encontraron créditos con pagos aplicados en {anio} para el tipo solicitado."
                };
            }

            var ciudades = await _catalogsRepository.ObtenerCiudades();

            var pdfsGenerados = new List<PlanPagoGeneradoDto>();
            var errores = new List<string>();

            foreach (var credito in creditosDelPeriodo)
            {
                try
                {
                    // 1. Obtener la info completa del crédito (igual que hace el front hoy)
                    var requestProducto = new SolicitudProductoPorClienteDto
                    {
                        Identificacion = credito.CLI_IDENTIFICACION,
                        TipoIdentificacion = 1
                    };
                    var respuestaProducto = await _carteraBusinessLogic.ObtenerProductoCliente(requestProducto);

                    var productoCliente = respuestaProducto?.Data
                        ?.FirstOrDefault(x => x.NumeroCredito == credito.CRE_NRO_CREDITO);

                    if (productoCliente == null)
                    {
                        errores.Add($"Crédito {credito.CRE_NRO_CREDITO}: no se encontró información en ObtenerProductoCliente.");
                        continue;
                    }

                    // 2. Plan de pago proyectado (cuotas pendientes / próximas)
                    var requestPlanPago = new ObtenerProductoCartera
                    {
                        Nro_Credito = credito.CRE_NRO_CREDITO,
                        Tcr_Id = credito.TCR_ID
                    };
                    var respuestaPlanPago = await _carteraBusinessLogic.ObtenerPlanPagosCredito(requestPlanPago);
                    var planPagoCompleto = respuestaPlanPago?.Data?.PlanPago ?? new List<PlanPagoDto>();
                    var planPago = planPagoCompleto
    .Where(x => x.Fecha.Year == anio + 1)
    .OrderBy(x => x.Fecha)
    .ToList();
                    // 3. >>> NUEVO: pagos realmente aplicados a ESTE crédito durante el año
                    //    (ya sin reversados, gracias al filtro del SP).
                    var pagosRealizados = (await _carteraRepository.ObtenerPagosAplicadosCreditoAnio(anio, credito.CRE_ID)).ToList();

                    var modelo = MapearARequestProductoClienteDto(productoCliente, planPago);

                    // 4. Generar el PDF individual, ahora con las dos listas separadas
                    var respuestaPdf = await GenerarExtractoAnualPdfBase64Async(modelo, pagosRealizados, anio);

                    if (respuestaPdf.IsSuccess && !string.IsNullOrEmpty(respuestaPdf.Data))
                    {
                        pdfsGenerados.Add(new PlanPagoGeneradoDto
                        {
                            NombreArchivo = $"PlanPago {credito.CRE_NRO_CREDITO}.pdf",
                            Bytes = Convert.FromBase64String(respuestaPdf.Data)
                        });
                    }
                    else
                    {
                        errores.Add($"Crédito {credito.CRE_NRO_CREDITO}: {respuestaPdf.ReturnMessage}");
                    }
                }
                catch (Exception ex)
                {
                    // No se detiene el proceso completo por un crédito individual con error
                    errores.Add($"Crédito {credito.CRE_NRO_CREDITO}: {ex.Message}");
                }
            }

            if (!pdfsGenerados.Any())
            {
                return new Response<byte[]>
                {
                    IsSuccess = false,
                    ReturnMessage = $"No se pudo generar ningún plan de pago para {anio}. Errores: {string.Join(" | ", errores)}"
                };
            }

            var zipBytes = GenerarZipBytes(pdfsGenerados);

            var mensaje = $"Se generaron {pdfsGenerados.Count} de {creditosDelPeriodo.Count} planes de pago.";
            if (errores.Any())
            {
                mensaje += $" Créditos con error: {errores.Count}.";
            }

            return new Response<byte[]>
            {
                Data = zipBytes,
                IsSuccess = true,
                ReturnMessage = mensaje
            };
        }

        // ==========================================================================
        // GenerarExtractoAnualPdfBase64Async: ahora recibe también la lista de pagos
        // realmente aplicados (pagosRealizados), separada del plan de pago proyectado
        // que ya viene dentro de "modelo" (modelo.PlanPago).
        // ==========================================================================
        private async Task<Response<string>> GenerarExtractoAnualPdfBase64Async(
            RequestProductoClienteDto modelo,
            List<PagosAplicadosCreditoAnio> pagosRealizados,
            int periodo,
            IEnumerable<CiudadDto> ciudadesCache = null) // ajusta CiudadDto al tipo real que retorna ObtenerCiudades()
        {
            if (modelo is null)
            {
                throw new ArgumentNullException(nameof(modelo), "El modelo del Extracto Anual no puede ser nulo.");
            }

            var rutaImagenes = new[]
            {
        Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "images", "ENCABEZADOIZ2026.png"),
        Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "images", "ENCABEZADO2026.png"),
        Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "images", "PIEDEPAGINA2022.png"),
    };

            var ciudades =  await _catalogsRepository.ObtenerCiudades();

            if (int.TryParse(modelo.CiudadInmueble, out var idCiudad))
            {
                var ciudad = ciudades.FirstOrDefault(x => x.DPC_ID == idCiudad);
                if (ciudad != null)
                {
                    modelo.CiudadInmueble = ciudad.DPC_NOMBRE;
                }
            }

            using var stream = new GA2.Domain.Core.TemplateReportesPdfAnualizadoCartera()
                .ReporteExtractoAnualPlanPagos(modelo, pagosRealizados, periodo, rutaImagenes);

            var base64 = Convert.ToBase64String(stream.ToArray());
            return new Response<string> { Data = base64, IsSuccess = true, ReturnMessage = "Extracto anual generado con éxito!" };
        }
        public async Task<Response<byte[]>> GenerarPlanesPagoMasivoZipBase64Async(RequestPlanPagoGeneradoDto request)
                {
                    // tipoCredito: 0 = todos, 1 = leasing, 2 = hipotecario
                    if (request is null || string.IsNullOrWhiteSpace(request.Periodo))
                    {
                        return new Response<byte[]>
                        {
                            IsSuccess = false,
                            ReturnMessage = "Debe indicar el periodo (año/mes)."
                        };
                    }

                    if (!TryParsearPeriodo(request.Periodo, out var anio, out var mes))
                    {
                        return new Response<byte[]>
                        {
                            IsSuccess = false,
                            ReturnMessage = $"El periodo '{request.Periodo}' no tiene un formato válido. Use 'AAAA-MM' (ej. 2025-11)."
                        };
                    }

                    var creditosDelPeriodo = await _carteraRepository.ObtenerCreditosDesembolsadosPorPeriodo(anio, mes, request.TipoCredito);

                    if (creditosDelPeriodo == null || !creditosDelPeriodo.Any())
                    {
                        return new Response<byte[]>
                        {
                            IsSuccess = false,
                            ReturnMessage = $"No se encontraron créditos desembolsados en {anio}/{mes:00} para el tipo solicitado."
                        };
                    }

                    var pdfsGenerados = new List<PlanPagoGeneradoDto>();
                    var errores = new List<string>();

                    foreach (var credito in creditosDelPeriodo)
                    {
                        try
                        {
                            // 1. Obtener la info completa del crédito (igual que hace el front hoy)
                            var requestProducto = new SolicitudProductoPorClienteDto
                            {
                                Identificacion = credito.CLI_IDENTIFICACION,
                                TipoIdentificacion = 1
                            };
                            var respuestaProducto = await _carteraBusinessLogic.ObtenerProductoCliente(requestProducto);

                            var productoCliente = respuestaProducto?.Data
                                ?.FirstOrDefault(x => x.NumeroCredito == credito.CRE_NRO_CREDITO);

                            if (productoCliente == null)
                            {
                                errores.Add($"Crédito {credito.CRE_NRO_CREDITO}: no se encontró información en ObtenerProductoCliente.");
                                continue;
                            }

                            // 2. Obtener el plan de pago y asignarlo al modelo (igual que hace el front hoy)
                            var requestPlanPago = new ObtenerProductoCartera
                            {
                                Nro_Credito = credito.CRE_NRO_CREDITO,
                                Tcr_Id = credito.TCR_ID
                            };
                            var respuestaPlanPago = await _carteraBusinessLogic.ObtenerPlanPagosCredito(requestPlanPago);
                            var planPago = respuestaPlanPago?.Data?.PlanPago ?? new List<PlanPagoDto>();

                            var modelo = MapearARequestProductoClienteDto(productoCliente, planPago);
                            modelo.CiudadInmueble = modelo.CiudadInmueble;
                            modelo.LineaCredito = modelo.LineaCredito;

                            // 4. Generar el PDF individual reutilizando el método/plantilla existente
                            var respuestaPdf = await GenerarPlanPagosHipotecarioPdfBase64Async(modelo);

                            if (respuestaPdf.IsSuccess && !string.IsNullOrEmpty(respuestaPdf.Data))
                            {
                                pdfsGenerados.Add(new PlanPagoGeneradoDto
                                {
                                    NombreArchivo = $"PlanPago {credito.CRE_NRO_CREDITO}.pdf",
                                    Bytes = Convert.FromBase64String(respuestaPdf.Data)
                                });
                            }
                            else
                            {
                                errores.Add($"Crédito {credito.CRE_NRO_CREDITO}: {respuestaPdf.ReturnMessage}");
                            }
                        }
                        catch (Exception ex)
                        {
                            // No se detiene el proceso completo por un crédito individual con error
                            errores.Add($"Crédito {credito.CRE_NRO_CREDITO}: {ex.Message}");
                        }
                    }

                    if (!pdfsGenerados.Any())
                    {
                        return new Response<byte[]>
                        {
                            IsSuccess = false,
                            ReturnMessage = $"No se pudo generar ningún plan de pago para {anio}/{mes:00}. Errores: {string.Join(" | ", errores)}"
                        };
                    }

                    var zipBytes = GenerarZipBytes(pdfsGenerados);

                    var mensaje = $"Se generaron {pdfsGenerados.Count} de {creditosDelPeriodo} planes de pago.";
                    if (errores.Any())
                    {
                        mensaje += $" Créditos con error: {errores.Count}.";
                    }

                    return new Response<byte[]>
                    {
                        Data = zipBytes,
                        IsSuccess = true,
                        ReturnMessage = mensaje
                    };
                }
                // >>> NUEVO: mapeo de ProductoClienteDto (lo que retorna ObtenerProductoCliente)
                // a RequestProductoClienteDto (lo que espera ReportePlanPagosHipotecario),
                // agregando el PlanPago que se obtiene aparte con ObtenerPlanPagosCredito.
                private static RequestProductoClienteDto MapearARequestProductoClienteDto(
                    ProductoClienteDto origen,
                    List<PlanPagoDto> planPago)
                {
                    return new RequestProductoClienteDto
                    {
                        IdCredito = origen.IdCredito,
                        FechaDesembolso = origen.FechaDesembolso,
                        ValorDesembolso = origen.ValorDesembolso,
                        ValorCuota = origen.ValorCuota,
                        PlazoTotal = origen.PlazoTotal,
                        IdTipoCredito = origen.IdTipoCredito,
                        DiaPago = origen.DiaPago,
                        IdEstadoCredito = origen.IdEstadoCredito,
                        FechaEstadoCredito = origen.FechaEstadoCredito,
                        DiasMora = origen.DiasMora,
                        ValorGarantia = origen.ValorGarantia,
                        ValorVivienda = origen.ValorVivienda,
                        ValorViviendaSeguroHogar = origen.ValorViviendaSeguroHogar,
                        IdEstadoVivienda = origen.IdEstadoVivienda,
                        IdTipoVivienda = origen.IdTipoVivienda,
                        SaldoCapital = origen.SaldoCapital,
                        SaldoCapitalMora = origen.SaldoCapitalMora,
                        PlazoActual = origen.PlazoActual,
                        TasaSeguroVida = origen.TasaSeguroVida,
                        TasaSeguroHogar = origen.TasaSeguroHogar,
                        TasaCreditoEA = origen.TasaCreditoEA,
                        TasaCreditoPer = origen.TasaCreditoPer,
                        TasaFrech = origen.TasaFrech,
                        ValorAlivioCuota = origen.ValorAlivioCuota,
                        FechaUltimoPago = origen.FechaUltimoPago,
                        FechaProximoPago = origen.FechaProximoPago,
                        TipoAbonoExtra = origen.TipoAbonoExtra,
                        ValorAbonoExtra = origen.ValorAbonoExtra,
                        ValorDeudaRemanente = origen.ValorDeudaRemanente,
                        ValorCanonInicial = origen.ValorCanonInicial,
                        ValorOpcionCompra = origen.ValorOpcionCompra,
                        PorcentajeCanonInicial = origen.PorcentajeCanonInicial,
                        PorcentajeOpcionCompra = origen.PorcentajeOpcionCompra,
                        AcumuladoInteresMora = origen.AcumuladoInteresMora,
                        CanonExtraordinario = origen.CanonExtraordinario,
                        UltimoPagoInteresMora = origen.UltimoPagoInteresMora,
                        AcumuladoInteresCorriente = origen.AcumuladoInteresCorriente,
                        UltimoPagoInteresCorriente = origen.UltimoPagoInteresCorriente,
                        AcumuladoSeguroHogar = origen.AcumuladoSeguroHogar,
                        UltimoPagoSeguroHogar = origen.UltimoPagoSeguroHogar,
                        AcumuladoSeguroVida = origen.AcumuladoSeguroVida,
                        UltimoPagoSeguroVida = origen.UltimoPagoSeguroVida,
                        AlivioFrech = origen.AlivioFrech,
                        NumeroCuotasCanceladas = origen.NumeroCuotasCanceladas,
                        CreadoPor = origen.CreadoPor,
                        InteresRemanente = origen.InteresRemanente,
                        InteresRemanenteFin = origen.InteresRemanenteFin,
                        CapitalRemanenteFin = origen.CapitalRemanenteFin,
                        SaldoTotalFin = origen.SaldoTotalFin,
                        TotalObligacion = origen.TotalObligacion,
                        TotalGarantia = origen.TotalGarantia,
                        ValSeguroHogar = origen.ValSeguroHogar,
                        ValSeguroVida = origen.ValSeguroVida,
                        LineaCredito = origen.LineaCredito,
                        FechaCreacion = origen.FechaCreacion,
                        ModificadoPor = origen.ModificadoPor,
                        FechaModificacion = origen.FechaModificacion,
                        NumeroCredito = origen.NumeroCredito,
                        TipoIdentificacion = origen.TipoIdentificacion,
                        Identificacion = origen.Identificacion,
                        Edad = origen.Edad,
                        Categoria = origen.Categoria,
                        IdCategoria = origen.IdCategoria,
                        Grado = origen.Grado,
                        IdGrado = origen.IdGrado,
                        DireccionInmueble = origen.DireccionInmueble,
                        CiudadInmueble = origen.CiudadInmueble,
                        NoMatricula = origen.NoMatricula,
                        Escritura = origen.Escritura,
                        FechaEscritura = origen.FechaEscritura,
                        Notaria = origen.Notaria,
                        Linderos = origen.Linderos,
                        DescripcionInmueble = origen.DescripcionInmueble,
                        Score = origen.Score,
                        FechaSolicitud = origen.FechaSolicitud,
                        FechaFinPagos = origen.FechaFinPagos,
                        TipoVivienda = origen.TipoVivienda,
                        ClaseInmueble = origen.ClaseInmueble,
                        EstadoCredito = origen.EstadoCredito,
                        saldoTotal = origen.saldoTotal,
                        saldoCapitalLeasing = origen.saldoCapitalLeasing,
                        SaldoCapitalTradicional = origen.SaldoCapitalTradicional,
                        SaldoInicialRedicido = origen.SaldoInicialRedicido,
                        ValorCuotaTradicional = origen.ValorCuotaTradicional,
                        UnidadEjecutora = origen.UnidadEjecutora,
                        VisOrNVis = origen.VisOrNVis,
                        NombreCliente = origen.NombreCliente,
                        TipoCredito = origen.TipoCredito,
                        InteresesRemanenteCorte = origen.InteresesRemanenteCorte,
                        SaldoTotalCorte = origen.SaldoTotalCorte,
                        CapitalRemanenteCorte = origen.CapitalRemanenteCorte,
                        CanonExtraordinarioCorte = origen.CanonExtraordinarioCorte,
                        InteresesRemanenteFinal = origen.InteresesRemanenteFinal,
                        SaldoCapitalRemanenteFinal = origen.SaldoCapitalRemanenteFinal,
                        CanonExtraordinarioFinal = origen.CanonExtraordinarioFinal,
                        ValorTotalAdeudadoFinal = origen.ValorTotalAdeudadoFinal,
                        DeudaActual = origen.DeudaActual,
                        DeudaActualTotal = origen.DeudaActualTotal,
                        IdLineaCredito = origen.IdLineaCredito,
                        FechaCausacion = origen.FechaCausacion,
                        ValorProvision = origen.ValorProvision,
                        PorcentajeProvision = origen.PorcentajeProvision,
                        Calificacion = origen.Calificacion,
                        AcumuladoCapital = origen.AcumuladoCapital,
                        ProvisionAdicional = origen.ProvisionAdicional,
                        // No existen en ProductoClienteDto: ArchivoPlanPagos, PorcentajeSeguroVida,
                        // PorcentajeExtraPrimaSeguroVida -- no se mapean, quedan fuera.
                        PlanPago = planPago ?? new List<PlanPagoDto>()
                    };
                }
                // >>> CAMBIO: ya no convierte a Base64, retorna el arreglo de bytes crudo.
                private byte[] GenerarZipBytes(List<PlanPagoGeneradoDto> pdfs)
                {
                    using var zipStream = new MemoryStream();

                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
                    {
                        foreach (var pdf in pdfs)
                        {
                            var entry = archive.CreateEntry(pdf.NombreArchivo, CompressionLevel.Optimal);
                            using var entryStream = entry.Open();
                            entryStream.Write(pdf.Bytes, 0, pdf.Bytes.Length);
                        }
                    }

                    return zipStream.ToArray();
                }
                public async Task<Response<IEnumerable<ReporteAPNRCARDto>>> GenerarReporteAPNRCAR(ReporteAPNRCARParametersDto parametersDto)
                {
                    try
                    {
                        var report = _mapper.Map<IEnumerable<ReporteAPNRCARDto>>(await _reportesRepository.GenerarReporteAPNRCAR(parametersDto));
                        var requeteReporte = new ReporteGeneralCarteraParametersDto
                        {
                            Anio = parametersDto.Anio,
                            Mes = parametersDto.Mes,
                            TipoCredito = null
                        };
                        var reportGarantia = new ReporteGeneralGarantiaParametersDto
                        {
                            Anio = parametersDto.Anio,
                            Mes = parametersDto.Mes,
                            TipoCredito = null
                        };
                        var valorGarantia = await ReporteGeneralGarantias(reportGarantia);
                        var dataReportList = valorGarantia.Data.ToList();
                        report = report.OrderBy(x => x.ConsecutivoNumeroDeContrato);
                        var responseConsultCartera = await GenerarReporteGeneralCartera(requeteReporte);
                        var diccionario = responseConsultCartera.Data
                            .ToDictionary(x => (x.NumeroCredito, x.Identificacion));
                        var reportList = report.ToList();
                        foreach (var item in reportList)
                        {
                            var key = (item.ConsecutivoNumeroDeContrato, item.Nit);
                            if (diccionario.TryGetValue(key, out var match))
                            {
                                item.SaldoCredito = match.SaldoCapital;
                                item.Provision = match.ProvisionTotal;
                            }
                        }
                        var diccionarioGarantia = dataReportList
                            .ToDictionary(x => (x.NumeroRadicacion, x.Identificacion));
                        foreach (var item in reportList)
                        {
                            var key = (item.ConsecutivoNumeroDeContrato.ToString(), item.Nit);
                            if (diccionarioGarantia.TryGetValue(key, out var match))
                            {
                                item.ValorDeLaGarantiaDelCredito = match.ValorAjustado;
                                item.CoberturaDeLaGarantiaValoracion = item.ValorDeLaGarantiaDelCredito;

                            }
                        }
                        return report.BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
                    }
                }

                public async Task<Response<IEnumerable<ReporteEstadoCuentaAfiliadoDto>>> GenerarReporteEstadoCuentaAfiliado(ReporteEstadoCuentaParametersDto parametersDto)
                {
                    try
                    {
                        var result = _mapper.Map<IEnumerable<ReporteEstadoCuentaAfiliadoDto>>(await _reportesRepository.GenerarReporteEstadoCuentaAfiliado(parametersDto));
                        var documentos = result.Select(x => x.NumeroIdentificacion).Distinct();
                        var afiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(documentos))?.Data;

                        var reportes = result.GroupJoin(afiliados, s => s.NumeroIdentificacion, s => s.Identificacion, (rep, cli) =>
                        {
                            var cliente = cli?.FirstOrDefault();
                            rep.NombreAfiliado = cliente?.NombreCompletoInvertido ?? "";
                            return rep;
                        });

                        return reportes.BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
                    }
                }

                public async Task<Response<IEnumerable<ReporteEstadoCuentaPlanPagoDto>>> GenerarReporteEstadoCuentaPlanPagos(Guid IdCredito)
                {
                    try
                    {
                        var reportes = _mapper.Map<IEnumerable<ReporteEstadoCuentaPlanPagoDto>>(await _reportesRepository.GenerarReporteEstadoCuentaPlanPagos(IdCredito));
                        return reportes.BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
                    }
                }

                public async Task<Response<IEnumerable<ReporteEstadoMovimientosDto>>> GenerarReporteEstadoMovimientos(ReporteEstadoMovimientosParametersDto parametersDto)
                {
                    try
                    {
                        var reportes = _mapper.Map<IEnumerable<ReporteEstadoMovimientosDto>>(await _reportesRepository.GenerarReporteEstadoMovimientos(parametersDto));
                        return reportes.BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
                    }
                }

                /// <summary>
                /// Generación Reporte Mensual Seguros
                /// </summary>
                /// <returns></returns>
                public async Task<Response<IEnumerable<ReporteMensualSegurosDto>>> GenerarReporteMensualSeguros(ReporteMensualSegurosParametersDto parametersDto)
                {
                    try
                    {
                        IEnumerable<ReporteMensualSegurosDto> response;

                        if (parametersDto.TipoCredito == null)
                        {
                            var response1 = _mapper.Map<IEnumerable<ReporteMensualSegurosDto>>(
                                await _reportesRepository.GenerarReporteMensualSeguros(new ReporteMensualSegurosParametersDto
                                {
                                    Anio = parametersDto.Anio,
                                    Mes = parametersDto.Mes,
                                    TipoCredito = 1
                                })
                            );

                            var response2 = _mapper.Map<IEnumerable<ReporteMensualSegurosDto>>(
                                await _reportesRepository.GenerarReporteMensualSeguros(new ReporteMensualSegurosParametersDto
                                {
                                    Anio = parametersDto.Anio,
                                    Mes = parametersDto.Mes,
                                    TipoCredito = 2
                                })
                            );

                            response = response1.Concat(response2);
                        }
                        else
                        {
                            response = _mapper.Map<IEnumerable<ReporteMensualSegurosDto>>(
                                await _reportesRepository.GenerarReporteMensualSeguros(parametersDto)
                            );
                        }

                        var documentos = response.Select(x => x.Identificacion).Distinct().ToList();
                        var afiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(documentos))?.Data;
                        var categorias = (await _catalogsBusinessLogic.ObtenerCategorias()).Data;
                        var grados = (await _catalogsBusinessLogic.ObtenerGrados()).Data;

                        var reportes = response.GroupJoin(
                            afiliados,
                            s => s.Identificacion,
                            s => s.Identificacion,
                            (rep, cli) =>
                            {
                                var cliente = cli?.FirstOrDefault();
                                rep.Nombres = cliente?.Nombres ?? "";
                                rep.Apellidos = cliente?.Apellidos ?? "";
                                rep.FechaExpedicionDocumento = cliente?.FechaExpedicionDocumento;
                                rep.FechaNacimiento = cliente?.FechaNacimiento;
                                rep.Direccion = cliente?.Direccion;
                                rep.Telefono = cliente?.Telefono;
                                rep.Correo = cliente?.Correo;
                                rep.Grado = grados.FirstOrDefault(x => x.Id == (cliente?.IdGrado ?? 0))?.Descripcion;
                                rep.Categoria = categorias.FirstOrDefault(x => x.Id == (cliente?.IdCategoria ?? 0))?.Descripcion;
                                return rep;
                            }
                        ).ToList();

                        return new Response<IEnumerable<ReporteMensualSegurosDto>>
                        {
                            Data = reportes,
                            IsSuccess = true
                        };
                    }
                    catch (Exception ex)
                    {
                        return new Response<IEnumerable<ReporteMensualSegurosDto>>
                        {
                            ReturnMessage = $"Error al generar el reporte: {ex.Message}",
                            IsSuccess = false
                        };
                    }
                }
                /// <summary>
                /// Generación Reporte Capital Corriente
                /// </summary>
                /// <returns></returns>
                public async Task<Response<IEnumerable<ReporteCapitalCorrienteDto>>> GenerarReporteCapitalCorriente(ReporteCapitalCorrienteParametersDto parametersDto)
                {
                    try
                    {
                        var cultureEs = CultureInfo.GetCultureInfo("es-ES");
                        var response = await _reportesRepository.GenerarReporteCapitalCorriente(parametersDto);

                        IEnumerable<ReporteCapitalCorrienteDto> capitalCorriente = response.GroupBy(item => (item.CRE_NRO_CREDITO, item.CLI_IDENTIFICACION))
                            .Select(item =>
                            {
                                var credito = item.First();
                                return new ReporteCapitalCorrienteDto()
                                {
                                    NumeroCredito = credito.CRE_NRO_CREDITO,
                                    NumeroIdentificacion = credito.CLI_IDENTIFICACION,
                                    FechaApertura = credito.CRE_FECHA_DESEMBOLSO,
                                    FechaFinalizacion = credito.CRE_FECHA_FINALIZA_PAGOS,
                                    IdTipoCredito = credito.TCR_ID,
                                    TipoCredito = credito.TCR_DESCRIPCION,
                                    Periodos = item.Select(capital => new CapitalCorrienteDto()
                                    {
                                        Capital = capital.PLN_CAPITAL,
                                        FechaPago = capital.PLN_FECHA_PAGO,
                                        Mes = capital.PLN_FECHA_PAGO.ToString("MMMM", cultureEs).ToUpper()
                                    }),
                                    TotalCapital = item.Sum(x => x.PLN_CAPITAL)
                                };
                            }).ToList();

                        return capitalCorriente.BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return new() { ReturnMessage = "Error al generar el reporte", IsSuccess = false };
                    }
                }

                /// <summary>
                /// Generación Reporte Opcion Compra
                /// </summary>
                /// <returns></returns>
                public async Task<Response<IEnumerable<ReporteOpcionCompraDto>>> GenerarReporteOpcionCompra(ReporteOpcionCompraParametersDto parametersDto)
                {
                    try
                    {
                        var result = _mapper.Map<IEnumerable<ReporteOpcionCompraDto>>(await _reportesRepository.GenerarReporteOpcionCompra(parametersDto));

                        var documentos = result.Select(x => x.NumeroIdentificacion).Distinct();
                        var afiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(documentos))?.Data;
                        var fechaPeriodo = new DateTime((int)parametersDto.Anio, (int)parametersDto.Mes,
                            DateTime.DaysInMonth((int)parametersDto.Anio, (int)parametersDto.Mes));
                        // Crear lista de tuplas (más ligero que objetos)
                        // 1 = credito leasing
                        var creditos = result
                            .Select(x => (x.NumeroCredito, 1))
                            .Distinct()
                            .ToList();
                        var dtaPlanPago = await _carteraBusinesLogic.ObtenerPlanPagoEnBatch(creditos, fechaPeriodo);

                        var resultFinal = result
                            .Select(item =>
                            {
                                var key = (item.NumeroCredito, 1);

                                if (dtaPlanPago.TryGetValue(key, out var planPago))
                                {
                                    item.CapitalRemanente = planPago.SaldoCapital + item.ValorOpcionCompra;
                                }

                                return item;
                            })
                            .ToList();
                        result = result.GroupJoin(afiliados, s => s.NumeroIdentificacion, s => s.Identificacion, (rep, cli) =>
                        {
                            var cliente = cli?.FirstOrDefault();
                            rep.NombreAfiliado = cliente?.NombreCompletoInvertido ?? "";
                            return rep;
                        }).ToList();

                        return result.BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
                    }
                }

                private async Task<FileResult> GenerarExcelReporteContable(IEnumerable<ReporteContableDto> report)
                {
                    var columnsInfo = new ColumnHeaderInfo[]
                    {
                        "Tipo Operación",
                        "Descripción Operación",
                        ("Fecha Operación", ColumnFormatType.Fecha),
                        "Número Identificación",
                        "Número Crédito",
                        "Cuenta Seven",
                        "Comprobante Interno",
                        "Descripción Cuenta",
                        ("Débito", ColumnFormatType.Decimal),
                        ("Crédito", ColumnFormatType.Decimal),
                    };

                    try
                    {
                        var data = report.Select(x =>
                            new string[]
                            {
                                x.TipoOperacion.ToString(),
                                x.DescripcionOperacion,
                                x.FechaOperacion.ToString("yyyy/MM/dd"),
                                x.NumeroIdentificacion,
                                x.NumeroCredito.ToString(),
                                x.CuentaSeven,
                                x.ComprobanteInterno,
                                x.DescripcionCuenta,
                                x.Debito.ToString(CultureInfo.InvariantCulture),
                                x.Credito.ToString(CultureInfo.InvariantCulture),
                            }
                        ).ToArray();

                        return await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatDto(columnsInfo, data));
                    }
                    catch (Exception)
                    {
                        throw new Exception("No se pudo generar el reporte");
                    }
                }

                public async Task<FileResult> GenerarExcelReporteCausaciones(ReporteContableParametersDto parametersDto)
                {
                    var columnsInfo = new ColumnHeaderInfo[]
                    {
                        "Tipo Operación",
                        "Descripción Operación",
                        ("Fecha Operación", ColumnFormatType.Fecha),
                        "Número Identificación",
                        "Número Crédito",
                        "Cuenta NIF",
                        "Cuenta Local",
                        "Comprobante Interno",
                        "Descripción Cuenta",
                        ("Débito", ColumnFormatType.Decimal),
                        ("Crédito", ColumnFormatType.Decimal),
                    };

                    try
                    {
                        var report = DataExcelReporteCausaciones(parametersDto);
                        return await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatAsyncDto(columnsInfo, report));
                    }
                    catch (Exception)
                    {
                        throw new Exception("No se pudo generar el reporte");
                    }
                }

                public async Task<FileResult> GenerarExcelReportePagos(ReportePagosParametersDto parametersDto)
                {
                    var columnsInfo = new ColumnHeaderInfo[]
                    {
                        "Unidad Ejecutora",
                        "Tipo Crédito",
                        "Número Crédito",
                        "Identificación",
                        "Afiliado",
                        ("Valor Pago", ColumnFormatType.Decimal),
                        ("Fecha Aplicación", ColumnFormatType.Fecha),
                        ("Fecha Pago", ColumnFormatType.Fecha),
                    };

                    try
                    {
                        var report = await ObtenerReportePagos(parametersDto);
                        var data = report.Select(x =>
                            new string[]
                            {
                                x.UnidadEjecutora,
                                x.TipoCredito,
                                x.NumeroCredito.ToString(),
                                x.Identificacion,
                                x.Afiliado,
                                x.ValorPago.ToString(CultureInfo.InvariantCulture),
                                x.FechaAplicacion.ToString("yyyy/MM/dd"),
                                x.FechaPago.ToString("yyyy/MM/dd")
                            }
                        ).ToArray();

                        return await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatDto(columnsInfo, data));
                    }
                    catch (Exception)
                    {
                        throw new Exception("No se pudo generar el reporte");
                    }
                }

                public async Task<FileResult> GenerarExcelReporteContablePagos(ReporteContableParametersDto parametersDto)
                {
                    try
                    {
                        var report = await ObtenerReporteContablePagos(parametersDto);
                        return await GenerarExcelReporteContable(report);
                    }
                    catch (Exception)
                    {
                        throw new Exception("No se pudo generar el reporte");
                    }
                }


                /// <summary>
                /// CargarListadoReportes
                /// </summary>
                /// <returns></returns>
                public async Task<Response<IEnumerable<CargarListadoReportesDto>>> CargarListadoReportes()
                {
                    var response = _mapper.Map<List<CargarListadoReportesDto>>(await _reportesRepository.CargarListadoReportes());
                    return new Response<IEnumerable<CargarListadoReportesDto>> { Data = response };
                }

                // Reportes de extractos

                public async Task<FileResult> GenerarReporteExtractoMensual(ReporteExtractoParametersDto parametersDto)
                {
                    var report = (await BuildReportExtractoMensual(parametersDto)).Archivo;
                    var file = await GenerarArchivoPDF(report.Bytes, report.Nombre);
                    return file;
                }

                public async Task<FileResult> GenerarReporteExtractoAnual(ReporteExtractoParametersDto parametersDto)
                {
                    var report = (await BuildReportExtractoAnual(parametersDto)).Archivo;
                    var file = await GenerarArchivoPDF(report.Bytes, report.Nombre);
                    return file;
                }

                public async Task<FileResult> GenerarReporteExtractoMasivo(ReporteExtractoMasivoParametersDto parametersDto, CancellationToken cancellationToken = default)
                {
                    try
                    {
                        var creditos = (parametersDto.TipoExtracto is TipoExtracto.Mensual
                            ? await _reportesRepository.ConsultarCreditosParaGeneracionExtractoMensual(parametersDto)
                            : await _reportesRepository.ConsultarCreditosParaGeneracionExtractoAnual(parametersDto)
                        ).ToList();

                        return await ProcesarReporteMasivo(new()
                        {
                            DatosReportes = BuilReportesExtractosMasivos(creditos, parametersDto),
                            Total = creditos.Count,
                            IdProceso = parametersDto.IdProceso
                        }, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        await ActualizarEstadoProcesoGeneracionReporte(parametersDto.IdProceso, EstadoProcesoReporte.CANCELADO);
                        throw;
                    }
                    catch (Exception e)
                    {
                        await ActualizarEstadoProcesoGeneracionReporte(parametersDto.IdProceso, EstadoProcesoReporte.FALLIDO + e.Message + e.InnerException);
                        throw;
                    }
                }

                public async Task<Response<ProcesoGeneracionReporteDto>> CrearProcesoGeneracionReporteExtractoMasivo(ReporteExtractoMasivoParametersDto parametersDto)
                {
                    try
                    {
                        var idProceso = Guid.NewGuid();
                        var tipoExtracto = parametersDto.Mes is null ? "ANUAL" : "MENSUAL";
                        var blobContenedor = "reportes-sfc";
                        var blobRutaAcceso = $"extractos/{tipoExtracto.ToLower()}es/{idProceso}.zip";

                        var unidad = (await ConsultarUnidadesEjecutoras())
                            .FirstOrDefault(x => x.Id == parametersDto.UnidadEjecutora)?.Sigla?.ToUpper() ?? "";

                        var tipoCredito = parametersDto.TipoCredito switch
                        {
                            1 => "LEASING HABITACIONAL",
                            2 => "HIPOTECARIO",
                            _ => ""
                        };

                        var result = await _reportesRepository.InsertarProcesoGeneracionReporte(new()
                        {
                            IdProceso = idProceso,
                            BlobContenedor = blobContenedor,
                            BlobRutaAcceso = blobRutaAcceso,
                            Estado = EstadoProcesoReporte.PENDIENTE,
                            NombreReporte = string.Join(" - ", new[] {
                                    $"EXTRACTOS {tipoExtracto}ES", unidad, tipoCredito, $"{parametersDto.Anio}{parametersDto.Mes:00}.zip"
                                }.Where(x => x.Any())
                            ),
                            TipoReporte = $"EXTRACTO_{tipoExtracto}_MASIVO",
                            Progreso = 0,
                            DetalleProceso = "",
                            CreadoPor = parametersDto.IdUsuario,
                        });


                        return _mapper.Map<ProcesoGeneracionReporteDto>(result).BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return "No fue posible crear el proceso para la generación del reporte"
                            .BuildError<ProcesoGeneracionReporteDto>();
                    }
                }

                public async Task<Response<ProcesoGeneracionReporteDto>> CrearProcesoGeneracionReporteAnioGravable(ReporteAnioGravableParametersDto parametersDto)
                {
                    try
                    {
                        var idProceso = Guid.NewGuid();
                        var blobContenedor = "reportes-sfc";
                        var blobRutaAcceso = $"retefuente-año-gravable/{idProceso}.zip";

                        var tipoCredito = parametersDto.TipoCredito switch
                        {
                            1 => " - LEASING HABITACIONAL",
                            2 => " - HIPOTECARIO",
                            _ => ""
                        };

                        var result = await _reportesRepository.InsertarProcesoGeneracionReporte(new()
                        {
                            IdProceso = idProceso,
                            BlobContenedor = blobContenedor,
                            BlobRutaAcceso = blobRutaAcceso,
                            Estado = EstadoProcesoReporte.PENDIENTE,
                            NombreReporte = $"RETEFUENTE AÑO GRAVABLE {parametersDto.Anio}{tipoCredito}.zip",
                            TipoReporte = "RETEFUENTE_AÑO_GRAVABLE_MASIVO",
                            Progreso = 0,
                            DetalleProceso = "",
                            CreadoPor = parametersDto.IdUsuario,
                        });


                        return _mapper.Map<ProcesoGeneracionReporteDto>(result).BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return "No fue posible crear el proceso para la generación del reporte"
                            .BuildError<ProcesoGeneracionReporteDto>();
                    }
                }

                public async Task<Response<IEnumerable<ProcesoGeneracionReporteDto>>> ConsultarProcesosGeneracionReportePorUsuario(Guid idUsuario, ReporteProcesoGeneracionReporteParametersDto tipoReporte)
                {
                    try
                    {
                        var procesos = await _reportesRepository.ConsultarProcesoGeneracionReporte(new()
                        {
                            CreadoPor = idUsuario,
                            Estado = $"{EstadoProcesoReporte.PENDIENTE},{EstadoProcesoReporte.COMPLETADO},{EstadoProcesoReporte.FALLIDO}",
                            TipoReporte = tipoReporte.TipoReporte,
                            IdProceso = tipoReporte.IdProceso
                        });
                        if (procesos.Any() && tipoReporte.TipoCredito > 0)
                        {
                            procesos = tipoReporte.TipoCredito == 1 ? procesos.Where(x => x.PGR_NOMBRE_REPORTE.Contains("LEASING HABITACIONAL")).ToList()
                                : procesos.Where(x => x.PGR_NOMBRE_REPORTE.Contains("HIPOTECARIO")).ToList();
                        }
                        return _mapper.Map<IEnumerable<ProcesoGeneracionReporteDto>>(procesos)
                            .BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return "No fue posible consultar los procesos de generación de reportes"
                            .BuildError<IEnumerable<ProcesoGeneracionReporteDto>>();
                    }
                }

                public async Task<Response<ProcesoGeneracionReporteDto>> ConsultarProcesoGeneracionReporte(Guid idUsuario, Guid idProceso)
                {
                    try
                    {

                        var proceso = (await _reportesRepository.ConsultarProcesoGeneracionReporte(new()
                        {
                            CreadoPor = idUsuario,
                            IdProceso = idProceso

                        })).FirstOrDefault();

                        if (proceso == null)
                        {
                            return "No se encontro el procesos de generación de reporte".BuildError<ProcesoGeneracionReporteDto>();
                        }

                        return _mapper.Map<ProcesoGeneracionReporteDto>(proceso).BuildSuccess();
                    }
                    catch (Exception)
                    {
                        return "No fue posible consultar el procesos de generación de reportes"
                            .BuildError<ProcesoGeneracionReporteDto>();
                    }
                }

                public async Task<Response<ProcesoGeneracionReporteDto>> CancelarProcesoGeneracionReporte(Guid idProceso)
                {
                    var proceso = await ActualizarEstadoProcesoGeneracionReporte(idProceso, EstadoProcesoReporte.CANCELADO);

                    if (proceso == null)
                    {
                        return "No fue posible cancelar el proceso de generación de reporte"
                           .BuildError<ProcesoGeneracionReporteDto>();
                    }

                    return _mapper.Map<ProcesoGeneracionReporteDto>(proceso).BuildSuccess();
                }

                public async Task<Response<ProcesoGeneracionReporteDto>> EliminarProcesoGeneracionReporte(Guid idProceso)
                {
                    var proceso = await ActualizarEstadoProcesoGeneracionReporte(idProceso, EstadoProcesoReporte.ELIMINADO);

                    if (proceso == null)
                    {
                        return "No fue posible eliminar el proceso de generación de reporte"
                           .BuildError<ProcesoGeneracionReporteDto>();
                    }

                    return _mapper.Map<ProcesoGeneracionReporteDto>(proceso).BuildSuccess();
                }

                public async Task<Response<Stream>> DescargarReporteMasivo(Guid idProceso)
                {
                    try
                    {
                        var proceso = (await _reportesRepository.ConsultarProcesoGeneracionReporte(new() { IdProceso = idProceso })).FirstOrDefault();

                        if (proceso == null)
                        {
                            return "No se encontró el proceso de generación de reporte".BuildError<Stream>();
                        }

                        var contenedor = new BlobContainerClient(_options.Value.CadenaOne, proceso.PGR_BLOB_CONTENEDOR);
                        var blobExtracto = contenedor.GetBlobClient(proceso.PGR_BLOB_RUTA_ACCESO);

                        if (!await blobExtracto.ExistsAsync())
                        {
                            return "No se encontro el reporte".BuildError<Stream>();
                        }

                        var result = await blobExtracto.DownloadStreamingAsync();
                        return result.Value.Content.BuildSuccess(proceso.PGR_NOMBRE_REPORTE);

                    }
                    catch (Exception)
                    {
                        return "No se pudo descargar el reporte".BuildError<Stream>();
                    }
                }

                private async Task<ProcesoGeneracionReporte> ActualizarEstadoACompletado(Guid idProceso)
                {
                    return await ActualizarEstadoProcesoGeneracionReporte(new()
                    {
                        IdProceso = idProceso,
                        Progreso = 100,
                        Estado = EstadoProcesoReporte.COMPLETADO
                    });
                }

                private async Task<ProcesoGeneracionReporte> ActualizarEstadoProcesoGeneracionReporte(Guid idProceso, string estado)
                {
                    return await ActualizarEstadoProcesoGeneracionReporte(new()
                    {
                        IdProceso = idProceso,
                        Estado = estado
                    });
                }

                private async Task<ProcesoGeneracionReporte> ActualizarEstadoProcesoGeneracionReporte(ActualizacionEstadoProcesoGeneracionReporteDto proceso)
                {
                    try
                    {
                        return await _reportesRepository.ActualizarEstadoProcesoGeneracionReporte(proceso);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }


                private ArchivoReporte GenerarExcelDeReportes(IEnumerable<DatosReporte> datosReporte, string nombreArchivo)
                {
                    try
                    {
                        var columnsInfo = new ColumnHeaderInfo[]
                        {
                            ("Identificación", ColumnFormatType.Numerico),
                            "Nombres",
                            ("Número Credito", ColumnFormatType.Numerico),
                            "Tipo Crédito",
                            "Unidad Ejecutora",
                            "Email",
                        };

                        var data = datosReporte.Select(x =>
                            new string[]
                            {
                                x.Identificacion,
                                x.Nombres,
                                x.NumeroCredito.ToString(),
                                x.TipoCredito,
                                x.UnidadEjecutora,
                                x.Email,
                            }
                        ).ToArray();

                        return new ArchivoReporte()
                        {
                            Nombre = nombreArchivo,
                            Bytes = _reportesExcel.GenerarExcelBytes(new ReportRequestExcelFormatDto(columnsInfo, data))
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ArchivoReporte()
                        {
                            Nombre = nombreArchivo,
                            Message = $"No se pudo generar el reporte excel",
                            Error = true,
                            Exception = ex
                        };
                    }
                }
                private async IAsyncEnumerable<ArchivoReporte> BuilReportesExtractosMasivos(IEnumerable<InformacionCredito> creditos, ReporteExtractoMasivoParametersDto parametersDto)
                {
                    var reportes = new List<DatosReporte>();

                    foreach (var credito in creditos)
                    {
                        var paramExtracto = new ReporteExtractoParametersDto(
                            credito.TCR_ID, credito.CRE_NRO_CREDITO, parametersDto.Anio, parametersDto.Mes
                        );

                        var result = paramExtracto.TipoExtracto is TipoExtracto.Mensual
                            ? await BuildReportExtractoMensual(paramExtracto)
                            : await BuildReportExtractoAnual(paramExtracto);

                        yield return result.Archivo;

                        if (!result.Archivo.Error)
                        {
                            reportes.Add(result);
                        }
                    }

                    var mes = parametersDto.TipoExtracto is TipoExtracto.Mensual
                        ? $"{parametersDto.Mes:00}"
                        : "";

                    yield return GenerarExcelDeReportes(reportes,
                        $"Reporte Extractos {parametersDto.TipoExtracto} {parametersDto.Anio}{mes}.xlsx"
                    );

                }
                private async Task<DatosReporte> BuildReportExtractoMensual(ReporteExtractoParametersDto parametersDto)
                {
                    var datosExtracto = new DatosReporte();

                    try
                    {
                        // Se mantiene para los campos ESTÁTICOS del crédito (plazo, tasas,
                        // conteos de cuotas, tipo de crédito, ciudad, etc.) que no dependen
                        // del período específico del extracto.
                        var extracto = await _reportesRepository.ObtenerInformacionExtractoMensual(parametersDto);

                        if (extracto == null)
                        {
                            datosExtracto.Archivo = new()
                            {
                                Message = $"No se encontró información del crédito {parametersDto.NumeroCredito} para generar el extracto",
                                Error = true,
                            };
                            return datosExtracto;
                        }

                        datosExtracto.NumeroCredito = extracto.CRE_NRO_CREDITO;
                        datosExtracto.TipoCredito = extracto.TIPO_CREDITO;
                        datosExtracto.UnidadEjecutora = extracto.UNIDAD_EJECUTORA;

                        var informacionBasica = (await _iReportesRequestProvider
                            .ReporteObtenerInformacionCliente(extracto.CLI_IDENTIFICACION))?.Data;

                        if (informacionBasica == null)
                        {
                            datosExtracto.Archivo = new()
                            {
                                Message = $"No se encontró información del cliente con identificación {extracto.CLI_IDENTIFICACION} para generar el extracto",
                                Error = true,
                            };
                            return datosExtracto;
                        }

                        datosExtracto.Identificacion = informacionBasica.CLI_IDENTIFICACION;
                        datosExtracto.Nombres = informacionBasica.CLI_NOMBRES;
                        datosExtracto.Email = informacionBasica.DCE_CORREO;

                        var requestCredito = new ObtenerProductoCartera
                        {
                            Nro_Credito = extracto.CRE_NRO_CREDITO,
                            Tcr_Id = extracto.TIPO_CREDITO_ID
                        };
                        var obtenerCredito = await _carteraBusinessLogic.ObtenerCreditoCartera(requestCredito);
                        var requestProducto = new SolicitudProductoPorClienteDto
                        {
                            Identificacion = obtenerCredito.Data.NumeroIdentificacion,
                            TipoIdentificacion = 1
                        };
                        var credito = await _carteraBusinessLogic.ObtenerProductoCliente(requestProducto);
                        var Datacredito = credito.Data.Where(x => x.NumeroCredito == extracto.CRE_NRO_CREDITO).FirstOrDefault();

                        var requestPlanPago = new ObtenerProductoCartera
                        {
                            Nro_Credito = extracto.CRE_NRO_CREDITO,
                            Tcr_Id = extracto.TIPO_CREDITO_ID
                        };
                        var planPago = await _carteraBusinessLogic.ObtenerPlanPagosCredito(requestPlanPago);

                        var primerDiaMesExtracto = new DateTime(parametersDto.Anio, (int)parametersDto.Mes, 1);
                        var fechaCorte = new DateTime(parametersDto.Anio, (int)parametersDto.Mes,
                            DateTime.DaysInMonth(parametersDto.Anio, (int)parametersDto.Mes));

                        // ═══════════════════════════════════════════════════════════
                        // REGLA 1: VALORES APLICADOS = último mes EFECTIVAMENTE pagado
                        // (no necesariamente el mes anterior -- si hay meses en mora sin
                        // pago, sigue siendo el último que sí quedó "PAGADA").
                        // ═══════════════════════════════════════════════════════════
                        var ultimaCuotaPagada = planPago.Data.PlanPago
                            .Where(c => (c.EstadoCuota == "PAGADA" || c.EstadoCuota == "PAGADA PARCIALMENTE")
                                     && c.Fecha < primerDiaMesExtracto)
                            .OrderByDescending(c => c.Fecha)
                            .FirstOrDefault();

                        decimal valAplCapital = 0, valAplInteresCorriente = 0, valAplInteresMora = 0,
                                valAplSeguroVida = 0, valAplSeguroHogar = 0, valAplTotalAplicado = 0;

                        if (ultimaCuotaPagada != null)
                        {
                            var movimientosPago = await _carteraRepository.ObtenerAplicacionPago(Datacredito.IdCredito);
                            var pagosDelMes = movimientosPago
                                .Where(x => x.CPG_FECHA_APLICACION == ultimaCuotaPagada.Fecha
                                         && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2))
                                .ToList();

                            valAplCapital = pagosDelMes.Sum(x => x.CPG_ABONO_CAPITAL);
                            valAplInteresCorriente = pagosDelMes.Sum(x => x.CPG_ABONO_INTERES_CORRIENTE);
                            valAplInteresMora = pagosDelMes.Sum(x => x.CPG_ABONO_INTERES_MORA);
                            valAplSeguroVida = pagosDelMes.Sum(x => x.CPG_ABONO_SEGURO_VIDA);
                            valAplSeguroHogar = pagosDelMes.Sum(x => x.CPG_ABONO_SEGURO_HOGAR);
                            valAplTotalAplicado = pagosDelMes.Sum(x => x.CPG_VALOR_PAGO);
                        }

                        // ═══════════════════════════════════════════════════════════
                        // REGLA 2/3: DISCRIMINACIÓN A PAGAR = mes del extracto (sin
                        // importar si está al día o en mora, siempre es el período
                        // que se está facturando).
                        // ═══════════════════════════════════════════════════════════
                        var cuotaDelMes = planPago.Data.PlanPago
                            .Where(c => c.Fecha.Year == parametersDto.Anio && c.Fecha.Month == (int)parametersDto.Mes)
                            .FirstOrDefault();

                        // ═══════════════════════════════════════════════════════════
                        // "Saldo de capital antes del pago" -- del plan de pago, el
                        // saldo que quedó tras la ÚLTIMA cuota anterior a la del extracto
                        // (no el saldo tras la cuota del extracto, que sería "después").
                        // ═══════════════════════════════════════════════════════════
                        var cuotaAnteriorAlExtracto = planPago.Data.PlanPago
                            .Where(c => c.Fecha < (cuotaDelMes?.Fecha ?? primerDiaMesExtracto))
                            .OrderByDescending(c => c.Fecha)
                            .FirstOrDefault();
                        decimal saldoCapitalAntesDelPago = cuotaAnteriorAlExtracto?.SaldoCapital ?? extracto.SALDO;

                        // ═══════════════════════════════════════════════════════════
                        // Interés remanente -- del plan de pago (solo leasing, no
                        // reestructurado). Se mantiene igual que ya estaba.
                        // ═══════════════════════════════════════════════════════════
                        decimal interesRemanenteExtracto = 0;
                        if (extracto.TIPO_CREDITO_ID == 1)
                        {
                            var novedad = await _carteraRepository.ObtenerNovedadesCartera(Datacredito.IdCredito);
                            var esReestructurado = novedad != null && novedad.Any(n => n.TNV_ID == 3 && n.ENC_ID == 5);

                            if (!esReestructurado)
                            {
                                interesRemanenteExtracto = planPago.Data.PlanPago
                                    .Where(x => x.Fecha < primerDiaMesExtracto)
                                    .Sum(x => x.InteresPorPagar);
                            }
                        }

                        var creditoEnMora = Datacredito.IdEstadoCredito == 2;

                        // ═══════════════════════════════════════════════════════════
                        // REGLA 3: DÍAS DE MORA -- desde el vencimiento de la PRIMERA
                        // cuota impaga (no pagada) hasta la fecha de corte del extracto.
                        // ═══════════════════════════════════════════════════════════
                        var primeraCuotaImpaga = planPago.Data.PlanPago
                            .Where(c => c.EstadoCuota != "PAGADA" && c.Fecha < fechaCorte)
                            .OrderBy(c => c.Fecha)
                            .FirstOrDefault();

                        int diasMoraExtracto = primeraCuotaImpaga != null
                            ? Math.Max((fechaCorte - primeraCuotaImpaga.Fecha).Days, 0)
                            : 0;

                        // Faltante de cuota: si la cuota DEL EXTRACTO quedó "PAGADA PARCIALMENTE"
                        decimal faltanteCuota = 0;
                        if (cuotaDelMes?.EstadoCuota == "PAGADA PARCIALMENTE")
                        {
                            var movimientosPago = await _carteraRepository.ObtenerAplicacionPago(Datacredito.IdCredito);
                            var ultimoPago = movimientosPago
                                .Where(x => x.CPG_VALOR_PAGO > 0
                                         && x.CPG_FECHA_APLICACION == cuotaDelMes.Fecha
                                         && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2))
                                .OrderByDescending(x => x.CPG_FECHA_CREACION)
                                .ToList();

                            if (ultimoPago.Any())
                            {
                                decimal capitalPagado = ultimoPago.Sum(x => x.CPG_ABONO_CAPITAL);
                                decimal capitalRequerido = Datacredito.IdTipoCredito == 1
                                    ? cuotaDelMes.CapitalReducido
                                    : cuotaDelMes.Capital;

                                faltanteCuota = Math.Round(capitalRequerido - capitalPagado, 0);
                            }
                        }

                        var datosBasicos = new ReportDataSource("DatosBasicos", new[]
                        {
                    new {
                        informacionBasica.CLI_NOMBRES,
                        extracto.CLI_IDENTIFICACION,
                        informacionBasica.DCE_CORREO,
                        extracto.CRE_NRO_CREDITO,
                        extracto.TIPO_CREDITO,
                        CIUDAD = extracto.CIUDAD_INMUEBLE?.ToUpper(),
                        EN_MORA = creditoEnMora
                    }
                });

                        var datosCuotas = new ReportDataSource("DatosCuotas", new[]
                        {
                    new {
                        TOTAL_CUOTA = extracto.PLN_PAGO_A_PAGAR,
                        extracto.PLAZO_TOTAL,
                        extracto.CUOTAS_PAGADAS,
                        extracto.CUOTAS_PENDIENTES,
                        extracto.CUOTAS_MORA,
                        extracto.INTERES_CORRIENTE_EA,
                        extracto.INTERES_MORA_EA,
                        FECHA_CORTE = fechaCorte,
                        // >>> AJUSTADO: ahora viene del plan de pago (saldo antes
                        // de aplicar el pago del período), no del SP.
                        SALDO = saldoCapitalAntesDelPago,
                        INTERES_REMANENTE = interesRemanenteExtracto,
                         TIPO_CREDITO_ID = extracto.TIPO_CREDITO_ID
                    }
                });

                        // >>> AJUSTADO: "Valores aplicados" ahora sale del historial de
                        // pagos de la ÚLTIMA cuota efectivamente pagada, no del SP.
                        var datosValores = new ReportDataSource("DatosValores", new[]
                        {
                    new {
                        CAPITAL = valAplCapital,
                        INTERES_CORRIENTE = valAplInteresCorriente,
                        INTERES_MORA = valAplInteresMora,
                        SEGUROS = valAplSeguroVida + valAplSeguroHogar,
                        extracto.ANTICIPOS,        //  sin equivalente en el historial de pagos, se mantiene del SP -- confirmar
                        extracto.OTROS_CARGOS,     //  ídem
                        extracto.COBERTURA_FRECH,  // ídem
                        TOTAL_APLICADO = valAplTotalAplicado
                    }
                });
                        var capital = Datacredito.IdTipoCredito == 1
                            ? (cuotaDelMes?.CapitalReducido ?? 0)
                            : (cuotaDelMes?.Capital ?? 0);
                        var  interes = Datacredito.IdTipoCredito == 1
                            ? (cuotaDelMes?.InteresReducido ?? 0)
                            : (cuotaDelMes?.Interes ?? 0);

                        // >>> AJUSTADO: "Discriminación a pagar" ahora sale de la cuota
                        // del plan de pagos correspondiente al mes del extracto.
                        var datosDiscriminados = new ReportDataSource("DatosDiscriminados", new[]
                        {
                    new
                    {
                        CAPITAL =capital,
                        INTERES_MORA = 0,
                        INTERES_CORRIENTE =interes,
                        SEGURO_VIDA = cuotaDelMes?.SeguroVida, 
                        SEGURO_TODO_RIESGO = cuotaDelMes?.SeguroHogar ,//  ídem
                        extracto.OTROS_GASTOS,     //  ídem
                        FALTANTE_CUOTA = faltanteCuota,
                        TOTAL_PAGAR =capital + interes + cuotaDelMes?.SeguroVida + cuotaDelMes?.SeguroHogar + faltanteCuota ,
                        PAGUE_ANTES_DE = fechaCorte,
                    }
                });

                        var path = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "Extractos", "ReporteCreditoExtractoMensual.rdlc");
                        using var report = new Microsoft.Reporting.NETCore.LocalReport();
                        report.ReportPath = path;
                        report.DataSources.Add(datosBasicos);
                        report.DataSources.Add(datosCuotas);
                        report.DataSources.Add(datosValores);
                        report.DataSources.Add(datosDiscriminados);

                        datosExtracto.Archivo = new()
                        {
                            Nombre = $"Extracto Mensual {parametersDto.Anio}{parametersDto.Mes:00} - {parametersDto.NumeroCredito} - {informacionBasica.CLI_IDENTIFICACION}.pdf",
                            Bytes = report.Render("PDF")
                        };
                    }
                    catch (Exception ex)
                    {
                        datosExtracto.Archivo = new()
                        {
                            Message = $"No se pudo generar el extracto del credito {parametersDto.NumeroCredito}",
                            Error = true,
                            Exception = ex
                        };
                    }

                    return datosExtracto;
                }
                private async Task<DatosReporte> BuildReportExtractoAnual(ReporteExtractoParametersDto parametersDto)
                {
                    var datosExtracto = new DatosReporte();

                    try
                    {
                        var infoCredito = await _carteraRepository.ObtenerCreditoCartera(new(parametersDto.NumeroCredito, parametersDto.TipoCredito));

                        if (infoCredito == null)
                        {
                            datosExtracto.Archivo = new()
                            {
                                Message = $"No se encontró información del crédito {parametersDto.NumeroCredito} para generar el extracto",
                                Error = true,
                            };

                            return datosExtracto;
                        }

                        datosExtracto.NumeroCredito = infoCredito.CRE_NRO_CREDITO;
                        datosExtracto.TipoCredito = infoCredito.TCR_DESCRIPCION;
                        datosExtracto.UnidadEjecutora = infoCredito.UEJ_NOMBRE;


                        var informacionBasica = (await _iReportesRequestProvider
                            .ReporteObtenerInformacionCliente(infoCredito.CLI_IDENTIFICACION))?.Data;

                        if (informacionBasica == null)
                        {
                            datosExtracto.Archivo = new()
                            {
                                Message = $"No se encontró información del cliente con identificación {infoCredito.CLI_IDENTIFICACION} para generar el extracto",
                                Error = true,
                            };

                            return datosExtracto;
                        }

                        datosExtracto.Identificacion = informacionBasica.CLI_IDENTIFICACION;
                        datosExtracto.Nombres = informacionBasica.CLI_NOMBRES;
                        datosExtracto.Email = informacionBasica.DCE_CORREO;


                        var proyeccion = await _reportesRepository.ObtenerInformacionProyeccionExtractoAnual(parametersDto);
                        var relacionPagos = await _reportesRepository.ObtenerInformacionRelacionPagosExtractoAnual(parametersDto);

                        var datosBasicos = new ReportDataSource("InfoCliente", new[]
                        {
                            new {
                              CIUDAD = infoCredito.DPC_NOMBRE,
                              CODIGO = "",
                              NOMBRES = informacionBasica.CLI_NOMBRES,
                              IDENTIFICACION = infoCredito.CLI_IDENTIFICACION,
                              CONTRATO = parametersDto.NumeroCredito,
                              TIPO_CLIENTE = "Afiliado",
                              DIRECCION = infoCredito.CRE_DIRECCION_INMUEBLE,
                              TELEFONO = informacionBasica.DTL_TELEFONO
                            }
                        });

                        var dataSetInfoProyeccion = new ReportDataSource("InfoProyeccion", proyeccion);
                        var dataSetInfoRelacion = new ReportDataSource("InfoRelacion", relacionPagos);

                        var nameReport = infoCredito.TCR_ID == 1 ? "ReporteCreditoExtractoAnual.rdlc" : "ReporteCreditoExtractoAnualHipotecario.rdlc";
                        var path = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "Extractos", nameReport);

                        using var report = new Microsoft.Reporting.NETCore.LocalReport();
                        report.ReportPath = path;
                        report.DataSources.Add(datosBasicos);
                        report.DataSources.Add(dataSetInfoProyeccion);
                        report.DataSources.Add(dataSetInfoRelacion);

                        datosExtracto.Archivo = new()
                        {
                            Nombre = $"Extracto Anual {parametersDto.Anio} - {parametersDto.NumeroCredito} - {infoCredito.CLI_IDENTIFICACION}.pdf",
                            Bytes = report.Render("PDF")
                        };

                    }
                    catch (Exception ex)
                    {
                        datosExtracto.Archivo = new()
                        {
                            Message = $"No se pudo generar el extracto del credito {parametersDto.NumeroCredito}",
                            Error = true,
                            Exception = ex
                        };
                    }

                    return datosExtracto;
                }

                private void EliminarArchivoTemporal(FileInfo file)
                {
                    try
                    {
                        if (file?.Exists ?? false)
                        {
                            file.Delete();
                        }
                    }
                    catch
                    {

                    }
                }

                private async Task<IEnumerable<UnidadEjecutoraDto>> ConsultarUnidadesEjecutoras()
                {
                    try
                    {
                        var unidades = (await _catalogsBusinessLogic.ObtenerUnidadesEjecutoras()).Data;
                        return unidades;
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<UnidadEjecutoraDto>();
                    }
                }

                //private FileStreamResult GenerarReportesRar(string tempDirectory)
                //{
                //    // Ruta donde se guardará el archivo RAR resultante
                //    string rarFilePath = Path.Combine(Path.GetTempPath(), "example.rar");

                //    // Comprime los archivos en un archivo RAR
                //    using (var archive = File.OpenWrite(rarFilePath))
                //    using (var writer = WriterFactory.Open(archive, ArchiveType.Rar, new WriterOptions(CompressionType.Rar)))
                //    {
                //        writer.WriteAll(tempDirectory, "*", SearchOption.AllDirectories);
                //    }

                //    // Devuelve el archivo RAR como un FileStreamResult
                //    var rarFileStream = new FileStream(rarFilePath, FileMode.Open);
                //    return new FileStreamResult(rarFileStream, "application/rar")
                //    {
                //        FileDownloadName = "informes.rar"
                //    };
                //}

                /// <summary>
                /// Generacion Reporte Certificado Ingresos Retenciones
                /// </summary>
                /// <param name="parametrosAplicacionPago"></param>
                /// <returns></returns>
                public async Task<FileResult> CertificadoIngresosRetenciones(string identificacion)
                {
                    // Ruta donde esta ubicado el reporte
                    var path = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\CertificadoIngresosRetenciones.rdlc";
                    string mimtype = string.Empty;
                    int extension = 1;

                    // Inicio parametros para reporte local
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("IDENTIFICACION", identificacion);

                    var responseInformacionBasica = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
                    var responseInformacionMontos = _mapper.Map<List<InformacionMontosCertificadoIYRDto>>(await _reportesRepository.InformacionMontosCertificadoIYR(identificacion));

                    if (responseInformacionBasica.Data != null)
                    {
                        LocalReport localReport = new LocalReport(path);

                        // Consulta a BD y serializacion del objeto en un DT
                        DataTable dataTableInformacionBasica = ObtainDataTableFromIEnumerable(responseInformacionBasica.Data);
                        DataTable dataTableInformacionMontos = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionMontos));

                        // Asigno al DS el DT para pintar los datos
                        localReport.AddDataSource("DatosBasicos", dataTableInformacionBasica);
                        localReport.AddDataSource("DatosMontos", dataTableInformacionMontos);

                        // Convierto el reporte en un arreglo de bytes
                        byte[] byteArray = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype).MainStream;

                        FileResult file = await GenerarReporte(byteArray, false);

                        return file;
                    }
                    else
                    {
                        throw new Exception($"No existe información en la base de datos para el documento: {identificacion}");
                    }
                }
                public async Task<FileResult> ReporteEntidades(EntidadEducativaDto request)
                {

                    BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
                    BlobClient blobBase = container.GetBlobClient("TemplateEntidades.html");
                    BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

                    BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                    BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

                    var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                    var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

                    await baseHtlm.FileStream.FlushAsync();
                    await baseCss.FileStream.FlushAsync();



                    Dictionary<string, string> Values = new Dictionary<string, string>();
                    string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
                    string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();


                    // Reemplazar valores en el HTML
                    Values.Add("PARAM_Fecha", DateTime.Now.ToString());
                    Values.Add("PARAM_NIT", request.Nit);
                    Values.Add("PARAM_ENTIDAD", request.RazonSocial);

                    return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);


                }

                //private byte[] GenerarArchivoPDF()
                //{
                //    // Aquí generas el contenido del archivo PDF utilizando la biblioteca o método de tu elección
                //    // Puedes utilizar librerías como iTextSharp, Syncfusion, PdfSharp, etc.
                //    // En este ejemplo, se generará un archivo PDF vacío como prueba

                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        // Crear el documento PDF
                //        var document = new iTextSharp.text.Document();

                //        // Crear el escritor PDF
                //        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                //        // Abrir el documento
                //        document.Open();

                //        // Agregar un párrafo de texto al documento
                //        var paragraph = new iTextSharp.text.Paragraph("Este es un ejemplo de contenido en el PDF.");
                //        paragraph.Font.Color = iTextSharp.text.BaseColor.BLACK;
                //        document.Add(paragraph);

                //        // Cerrar el documento
                //        document.Close();

                //        // Retornar los bytes del archivo PDF generado
                //        return ms.ToArray();
                //    }
                //}
                /// <summary>
                /// Generacion Reporte CertificadoHaberes
                /// </summary>
                /// <param name="parametrosAplicacionPago"></param>
                /// <returns></returns>
                public async Task<FileResult> CertificadoHaberes(string identificacion)
                {
                    var responseInformacionBasica = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);

                    if (responseInformacionBasica.Data != null)
                    {
                        // Consulta a BD y serializacion del objeto en un DT
                        DataTable dataTableInformacionBasica = ObtainDataTableFromIEnumerable(responseInformacionBasica.Data);

                        // Obtener datos iniciales para cargar nombre del documento en el DT 
                        TipoIdentificacionDto documento = _mapper.Map<TipoIdentificacionDto>(await _juzgadosRepository.
                            ObtenerTipoIdentificacionPorId(int.Parse(dataTableInformacionBasica.Rows[0].ItemArray[2].ToString())));

                        // Con LINQ relleno los valores de BUA que se extraen de GA2
                        dataTableInformacionBasica.Rows.Cast<DataRow>()
                            .Where(x => Convert.ToString(x["TIPO_IDENTIFICACION"]) == "TIPO_IDENTIFICACION").ToList()
                            .ForEach(x => x.SetField("TIPO_IDENTIFICACION", documento.Descripcion));

                        FileResult file = await GenerarDocumentoPDF(dataTableInformacionBasica, "ReporteCertificadoHaberes");

                        return file;
                    }
                    else
                    {
                        throw new Exception($"No existe información en la base de datos para el documento: {identificacion}");
                    }
                }

                /// <summary>
                /// Generacion Reporte EstadoDeCuenta
                /// </summary>
                /// <param name="parametrosAplicacionPago"></param>
                /// <returns></returns>
                public async Task<FileResult> EstadoDeCuenta(string identificacion)
                {
                    var path = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\EstadoCuentaAfiliado.rdlc";
                    string mimtype = string.Empty;
                    int extension = 1;

                    // Inicio parametros para reporte local
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("IDENTIFICACION", identificacion);

                    var responseInformacionBasica = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
                    var responseInformacionObligacion = _mapper.Map<List<InformacionObligacionEstadoCuentaDto>>(await _reportesRepository.InformacionObligacionEstadoCuenta(identificacion));
                    var responseDistribucionValores = _mapper.Map<List<InformacionDistribucionValoresEstadoCuentaDto>>(await _reportesRepository.InformacionDistribucionValoresEstadoCuenta(identificacion));

                    if (!responseInformacionBasica.Data.Any())
                    {
                        throw new InvalidOperationException($"No existe información en la base de datos para el afiliado: {identificacion}");
                    }

                    if (!responseInformacionObligacion.Any())
                    {
                        throw new InvalidOperationException($"No existe Información Obligacion en la base de datos para la Afiliacion: {identificacion}");
                    }

                    if (!responseDistribucionValores.Any())
                    {
                        throw new InvalidOperationException($"No existe Información Distribucion de Valores en la base de datos para la Afiliacion: {identificacion}");
                    }

                    LocalReport localReport = new LocalReport(path);

                    // Consulta a BD y serializacion del objeto en un DT
                    DataTable dataTableInformacionBasica = ObtainDataTableFromIEnumerable(responseInformacionBasica.Data);
                    DataTable dataTableInformacionObligacion = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionObligacion));
                    DataTable dataTableDistribucionValores = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseDistribucionValores));

                    // Asigno al DS el DT para pintar los datos
                    localReport.AddDataSource("DatosBasicos", dataTableInformacionBasica);
                    localReport.AddDataSource("DatosObligacion", dataTableInformacionObligacion);
                    localReport.AddDataSource("DatosDistribucionValores", dataTableDistribucionValores);

                    // Obtener datos iniciales para cargar nombre del documento en el DT 
                    TipoIdentificacionDto documento = _mapper.Map<TipoIdentificacionDto>(await _juzgadosRepository.
                        ObtenerTipoIdentificacionPorId(int.Parse(dataTableInformacionBasica.Rows[0].ItemArray[2].ToString())));

                    // Con LINQ relleno los valores de BUA que se extraen de GA2
                    dataTableInformacionBasica.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["TIPO_IDENTIFICACION"]) == "TIPO_IDENTIFICACION").ToList().ForEach(x => x.SetField("TIPO_IDENTIFICACION", documento.Descripcion));

                    // Convierto el reporte en un arreglo de bytes
                    byte[] byteArray = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype).MainStream;

                    FileResult file = await GenerarReporte(byteArray, false);

                    return file;

                }

                /// <summary>
                /// Generacion Reporte MovimientoDeCartera
                /// </summary>
                /// <param name="parametrosAplicacionPago"></param>
                /// <returns></returns>
                public async Task<FileResult> MovimientoDeCartera(string identificacion)
                {
                    var path = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\MovimientoCartera.rdlc";
                    string mimtype = string.Empty;
                    int extension = 1;

                    // Inicio parametros para reporte local
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("IDENTIFICACION", identificacion);
                    var request = new SolicitudProductoPorClienteDto
                    {
                        TipoIdentificacion = 1,
                        Identificacion = identificacion,
                    };

                    var dataProducto = await _carteraBusinessLogic.ObtenerProductoCliente(request);



                    var responseInformacionBasica = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
                    var responseMovimiento = _mapper.Map<List<InformacionMovimientosCarteraDto>>(await _reportesRepository.InformacionMovimientosCartera(identificacion));
                    var responseMovimientoDetalle = _mapper.Map<List<InformacionMovimientosCarteraDetalleDto>>(await _reportesRepository.InformacionMovimientosCarteraDetalle(identificacion));

                    if (dataProducto.IsSuccess && dataProducto.Data.Count() > 0)
                    {
                        responseInformacionBasica.Data.FirstOrDefault().CODIGO = responseInformacionBasica.Data.FirstOrDefault().CLI_IDENTIFICACION;

                        var requestPlanPago = new ObtenerProductoCartera
                        {
                            Nro_Credito = dataProducto.Data.FirstOrDefault().NumeroCredito,
                            Tcr_Id = dataProducto.Data.FirstOrDefault().IdTipoCredito
                        };
                        var dataPlanPago = await _carteraBusinessLogic.ObtenerPlanPagosCredito(requestPlanPago);

                        responseMovimiento.FirstOrDefault().OPCION_COMPRA = dataProducto.Data.FirstOrDefault().ValorOpcionCompra;
                        responseMovimiento.FirstOrDefault().PERIODO = dataProducto.Data.FirstOrDefault().FechaEstadoCredito.ToString("dd/MM/yyyy");
                        responseMovimiento.FirstOrDefault().VALOR_DESEMBOLSO = dataProducto.Data.FirstOrDefault().ValorDesembolso;
                        responseMovimiento.FirstOrDefault().VALOR_VIVIENDA = dataProducto.Data.FirstOrDefault().ValorDesembolso;
                        responseMovimiento.FirstOrDefault().FECHA_EMISION = dataProducto.Data.FirstOrDefault().FechaDesembolso;
                        responseMovimiento.FirstOrDefault().VALOR_VIVIENDA = dataProducto.Data.FirstOrDefault().ValorVivienda;
                        responseMovimiento.FirstOrDefault().SALDO = dataProducto.Data.FirstOrDefault().SaldoCapital;
                        responseMovimiento.FirstOrDefault().CANON_EXTRA = dataProducto.Data.FirstOrDefault().ValorAbonoExtra;

                        var cuotaCanceladas = dataPlanPago.Data.PlanPago.Where(x => x.EstadoCuota == "PAGADA").Count();
                        var cuotaCalculadas = requestPlanPago.Tcr_Id == 1 ? cuotaCanceladas + 1 : cuotaCanceladas;

                        var detalle = dataPlanPago.Data.PlanPago.Where(x => x.Cuota == cuotaCalculadas).FirstOrDefault();
                        responseMovimientoDetalle.FirstOrDefault().CUOTAS_CANCELADAS = cuotaCalculadas;
                        responseMovimientoDetalle.FirstOrDefault().VALOR_CAPITAL = requestPlanPago.Tcr_Id == 1 ? detalle.CapitalReducido : detalle.Capital;
                        responseMovimientoDetalle.FirstOrDefault().INTERESES_CORRIENTES = requestPlanPago.Tcr_Id == 1 ? detalle.InteresReducido : detalle.Interes;
                        responseMovimientoDetalle.FirstOrDefault().SEGURO_VIDA = detalle.SeguroVida;
                        responseMovimientoDetalle.FirstOrDefault().SEGURO_HOGAR = detalle.SeguroHogar;
                        responseMovimientoDetalle.FirstOrDefault().INTERESES_MORA = 0;
                        responseMovimientoDetalle.FirstOrDefault().FECHA_CUOTA = detalle.Fecha;
                        var saldo = responseMovimiento.FirstOrDefault().SALDO * -1;
                        var saldoCapital = requestPlanPago.Tcr_Id == 1 ? detalle.CapitalReducido : detalle.Capital;
                        responseMovimientoDetalle.FirstOrDefault().SALDO_CAPITAL = responseMovimiento.FirstOrDefault().SALDO - saldoCapital;
                    }

                    if (!responseInformacionBasica.Data.Any())
                    {
                        throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {identificacion}");
                    }

                    if (!responseMovimiento.Any())
                    {
                        throw new InvalidOperationException($"No existe información de Movimientos en la base de datos para la Afiliacion: {identificacion}");
                    }

                    if (!responseMovimientoDetalle.Any())
                    {
                        throw new InvalidOperationException($"No existe información de Movimiento-Detalle en la base de datos para la Afiliacion: {identificacion}");
                    }

                    LocalReport localReport = new LocalReport(path);

                    // Consulta a BD y serializacion del objeto en un DT
                    DataTable dataTableInformacionBasica = ObtainDataTableFromIEnumerable(responseInformacionBasica.Data);
                    DataTable dataTableMovimiento = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseMovimiento));
                    DataTable dataTableMovimientoDetalle = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseMovimientoDetalle));

                    // Asigno al DS el DT para pintar los datos
                    localReport.AddDataSource("DatosBasicos", dataTableInformacionBasica);
                    localReport.AddDataSource("DatosMovimiento", dataTableMovimiento);
                    localReport.AddDataSource("DatosMovimientoDetalle", dataTableMovimientoDetalle);

                    // Obtener datos iniciales para cargar nombre del documento en el DT 
                    TipoIdentificacionDto documento = _mapper.Map<TipoIdentificacionDto>(await _juzgadosRepository.
                        ObtenerTipoIdentificacionPorId(int.Parse(dataTableInformacionBasica.Rows[0].ItemArray[2].ToString())));

                    // Con LINQ relleno los valores de BUA que se extraen de GA2
                    dataTableInformacionBasica.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["TIPO_IDENTIFICACION"]) == "TIPO_IDENTIFICACION").ToList().ForEach(x => x.SetField("TIPO_IDENTIFICACION", documento.Descripcion));

                    // Convierto el reporte en un arreglo de bytes
                    byte[] byteArray = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype).MainStream;

                    FileResult file = await GenerarReporte(byteArray, false);

                    return file;

                }

                /// <summary>
                /// Generacion ReporteSFCF343
                /// </summary>
                /// <param name="parametros"></param>
                /// <returns></returns>
                public async Task<FileResult> ReporteSFCF343(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
                {
                    // Obtener los datos necesarios para el reporte
                    var reportFecha = parametrosAplicacionPago.FechaAplicacion.Date.ToString().Split(' ')[0];
                    /*var response453Y454 = _mapper.Map<List<ReporteSFCF343>>(await _reportesRepository.ReporteSFCF343(parametrosAplicacionPago));

                    if (response453Y454.Count == 0)
                    {
                        throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
                    }*/

        // Generar el archivo plano 453 y 454
        byte[] buffer = await GenerarArchivoPlano343(reportFecha);

            // Convertir el arreglo de bytes en un FileResult con tipo MIME para texto plano
            var file = new FileContentResult(buffer, "text/plain");

            return file;
        }
        private async Task<byte[]> GenerarArchivoPlano343(String dateReport)
        {
            StringBuilder constructorLineal = new StringBuilder();

            // Obtener la fecha actual en el formato "DDMMAAAA"
            String[] dateTimeClean = dateReport.ToString().Split('/');

            // Agregar encabezado
            constructorLineal.AppendLine($"00001122000011{dateTimeClean[0]}{dateTimeClean[1]}{dateTimeClean[2]}00040IOECAJAPROM0");
            constructorLineal.AppendLine("000022000001100000");

            // Iterar sobre cada registro y agregarlo al constructor lineal
            /*foreach (var registro in registros)
            {
                constructorLineal.AppendLine(GenerarLineaRep543Y545(registro));
            }*/

            // Convertir el texto a un arreglo de bytes
            byte[] buffer = Encoding.ASCII.GetBytes(constructorLineal.ToString());
            return buffer;
        }
        /// <summary>
        /// Generacion ReporteSFCF453Y454
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReporteSFCF453Y454(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            // Obtener los datos necesarios para el reporte
            var response453Y454 = _mapper.Map<List<ReporteSFCF453Y454>>(await _reportesRepository.ReporteSFCF453Y454(parametrosAplicacionPago));
            var reportFecha = parametrosAplicacionPago.FechaAplicacion.Date.ToString().Split(' ')[0];

            if (response453Y454.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            // Generar el archivo plano 453 y 454
            byte[] buffer = await GenerarArchivoPlano453y454(response453Y454, reportFecha);

            // Convertir el arreglo de bytes en un FileResult con tipo MIME para texto plano
            var file = new FileContentResult(buffer, "text/plain");

            return file;
        }

        private async Task<byte[]> GenerarArchivoPlano453y454(List<ReporteSFCF453Y454> registros, String dateReport)
        {
            StringBuilder constructorLineal = new StringBuilder();

            // Obtener la fecha actual en el formato "DDMMAAAA"
            String[] dateTimeClean = dateReport.ToString().Split('/');

            // Agregar encabezado
            if (int.Parse(dateTimeClean[0]) <= 9)
            {
                constructorLineal.AppendLine($"00001122000011{dateTimeClean[1]}0{dateTimeClean[0]}{dateTimeClean[2]}00040IOECAJAPROM0160");
            }
            else
            {
                constructorLineal.AppendLine($"00001122000011{dateTimeClean[1]}{dateTimeClean[0]}{dateTimeClean[2]}00040IOECAJAPROM0160");
            }
            constructorLineal.AppendLine("000022000001100000");

            // Iterar sobre cada registro y agregarlo al constructor lineal
            foreach (var registro in registros)
            {
                constructorLineal.AppendLine(GenerarLineaRep543Y545(registro));
            }

            // Convertir el texto a un arreglo de bytes
            byte[] buffer = Encoding.ASCII.GetBytes(constructorLineal.ToString().TrimEnd());
            return buffer;
        }

        private string GenerarLineaRep543Y545(ReporteSFCF453Y454 registro)
        {
            return $"{registro.consecutivo_formateado}{registro.cuenta_formateada}{registro.formato}{registro.columnaTable}{registro.columna2_formateada}{registro.subcuenta}{registro.valor_formateado}";
        }


        /// <summary>
        /// Generacion ReporteSFCF414
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReporteSFCF414(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            // Ruta donde esta ubicado el reporte
            var path = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteSFCF14.rdlc";
            string mimtype = string.Empty;
            int extension = 1;

            // Inicio parametros para reporte local
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var responseF414 = _mapper.Map<List<ReporteSFCF414Dto>>(await _reportesRepository.ReporteSFCF414(parametrosAplicacionPago));

            if (responseF414.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            List<ReporteSFCF414Dto> list = new();

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responseF414.
                Select(x => x.CLI_IDENTIFICACION).Distinct().ToList())).Data;

            list = (from r in responseF414
                    join la in listAfiliados on r.CLI_IDENTIFICACION equals la.Documento
                    select new ReporteSFCF414Dto
                    {
                        CLI_IDENTIFICACION = r.CLI_IDENTIFICACION,
                        MARGEN_ADICIONAL_VARIACIÓN_CREDITOS_UVR = r.MARGEN_ADICIONAL_VARIACIÓN_CREDITOS_UVR,
                        MONTOS_DESEMBOLSADOS = r.MONTOS_DESEMBOLSADOS,
                        NUMERO_CREDITOS_DESEMBOLSADOS = r.NUMERO_CREDITOS_DESEMBOLSADOS,
                        PLAZO_CREDITO = r.PLAZO_CREDITO,
                        PRODUCTO_CREDITO = r.PRODUCTO_CREDITO,
                        SEXO = la.Sexo,
                        SUBCUENTA = r.SUBCUENTA,
                        TAMANO_EMPRESA = r.TAMANO_EMPRESA,
                        TASA_EFECTIVA_PROMEDIO_PONDERADA = r.TASA_EFECTIVA_PROMEDIO_PONDERADA,
                        TIPO_CREDITO = r.TIPO_CREDITO,
                        TIPO_GARANTIA = r.TIPO_GARANTIA,
                        TIPO_PERSONA = r.TIPO_PERSONA,
                        UNIDAD_CAPTURA = r.UNIDAD_CAPTURA
                    }).ToList();

            if (list.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            LocalReport localReport = new LocalReport(path);

            // Consulta a BD y serializacion del objeto en un DT
            DataTable dataTable = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(list));

            // Asigno al DS el DT para pintar los datos
            localReport.AddDataSource("InformacionSFC414", dataTable);

            // Convierto el reporte en un arreglo de bytes
            byte[] byteArray = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype).MainStream;

            FileResult file = await GenerarReporte(byteArray, false);

            return file;

        }

        /// <summary>
        /// Generacion ReporteSFCF416
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<Response<List<ReporteSFCF416Dto>>>ReporteSFCF416(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            
           
            var responseF416 = _mapper.Map<List<ReporteSFCF416Dto>>(await _reportesRepository.ReporteSFCF416(parametrosAplicacionPago));

            if (responseF416.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }


            return new Response<List<ReporteSFCF416Dto>>
            {
                Data = responseF416
            };
        }

        /// <summary>
        /// Generacion reporte cartera
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteCarteraDto>>> ReporteCartera()
        {
            IEnumerable<ReporteCarteraDto> list = new List<ReporteCarteraDto>();
            var response = _mapper.Map<IEnumerable<ReporteCarteraDto>>(await _reportesRepository.ReporteCartera());

            foreach (var item in response)
            {
                Response<IEnumerable<InformacionClienteDto>> respuestaInformacionClienteDto = (Response<IEnumerable<InformacionClienteDto>>)await _iReportesRequestProvider.
                    ReporteInformacionCliente(item.Identificacion);

                if (respuestaInformacionClienteDto.Data != null)
                {
                    //item.Nombres = respuestaInformacionClienteDto.Data.;
                    //item.Direccion = respuestaInformacionClienteDto.Data.DRC_DIRECCION;
                    //item.Telefono = respuestaInformacionClienteDto.Data.DTL_TELEFONO;
                    //item.Correo = respuestaInformacionClienteDto.Data.DCE_CORREO;
                    //list.Add(item);
                }
            }

            return new Response<IEnumerable<ReporteCarteraDto>> { Data = list };
        }

        /// <summary>
        /// Generacion Reporte Aseguradora
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteAseguradoraDto>>> ReporteAseguradora()
        {
            var response = _mapper.Map<List<ReporteAseguradoraDto>>(await _reportesRepository.ReporteAseguradora());
            return new Response<IEnumerable<ReporteAseguradoraDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Avaluos 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteAvaluosDto>>> ReporteAvaluos()
        {
            var response = _mapper.Map<List<ReporteAvaluosDto>>(await _reportesRepository.ReporteAvaluos());
            return new Response<IEnumerable<ReporteAvaluosDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Carga Analista 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteCargaAnalistaDto>>> ReporteCargaAnalista()
        {
            var response = _mapper.Map<List<ReporteCargaAnalistaDto>>(await _reportesRepository.ReporteCargaAnalista());
            return new Response<IEnumerable<ReporteCargaAnalistaDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Cesiones
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteCesionesDto>>> ReporteCesiones()
        {
            var response = _mapper.Map<List<ReporteCesionesDto>>(await _reportesRepository.ReporteCesiones());
            return new Response<IEnumerable<ReporteCesionesDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Consultas Restrictivas
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteConsultasRestrictivasDto>>> ReporteConsultasRestrictivas()
        {
            var response = _mapper.Map<List<ReporteConsultasRestrictivasDto>>(await _reportesRepository.ReporteConsultasRestrictivas());
            return new Response<IEnumerable<ReporteConsultasRestrictivasDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Estudio Titulos
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteEstudioTitulosDto>>> ReporteEstudioTitulos()
        {
            var response = _mapper.Map<List<ReporteEstudioTitulosDto>>(await _reportesRepository.ReporteEstudioTitulos());
            return new Response<IEnumerable<ReporteEstudioTitulosDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Valor Venta
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteValorVentaDto>>> ReporteValorVenta()
        {
            var response = _mapper.Map<List<ReporteValorVentaDto>>(await _reportesRepository.ReporteValorVenta());
            return new Response<IEnumerable<ReporteValorVentaDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte De Mora  
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteDeMoraDto>>> ReporteDeMora()
        {
            var response = _mapper.Map<List<ReporteDeMoraDto>>(await _reportesRepository.ReporteDeMora());
            return new Response<IEnumerable<ReporteDeMoraDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte General Cartera
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteGeneralCarteraDto>>> ReporteGeneralCartera()
        {
            var resultData = await _iReportesRequestProvider.ObtenerClienteCorreosErrados();
            var Clientes = _mapper.Map<List<ReporteClienteCorreosErradosDto>>(resultData.Data);
            var response = _mapper.Map<List<ReporteGeneralCarteraDto>>(await _reportesRepository.ReporteGeneralCartera());
            var unidad_ejecutoraData = await _reportesRepository.ConsultaUnidadEjecutora();
            var undidad_ejecutora = unidad_ejecutoraData.ToList();

            List<ReporteGeneralCarteraDto> list = (from r in response
                                                   join la in Clientes on r.Identificacion equals la.IDENTIFICACION
                                                   join un in undidad_ejecutora on la.UEJ_ID equals un.UEJ_ID
                                                   select new ReporteGeneralCarteraDto
                                                   {
                                                       NumeroRadicacion = r.NumeroRadicacion,
                                                       Pagare = r.Pagare,
                                                       Cliente = la.ID_CLIENTE.ToString(),
                                                       Estado = r.Estado,
                                                       Identificacion = r.Identificacion,
                                                       Pagaduria = un.UEJ_IDENTIFICACION,
                                                       NombrePagaduria = un.UEJ_NOMBRE,
                                                       Apertura = r.Apertura,
                                                       Terminacion = r.Terminacion,
                                                       TasaLinea = r.TasaLinea,
                                                       Destinacion = r.Destinacion,
                                                       Desembolso = r.Desembolso,
                                                       Monto = r.Monto,
                                                       Saldo = r.Saldo,
                                                       Cuota = r.Cuota,
                                                       Dias = r.Dias,
                                                       UltimoPago = r.UltimoPago,
                                                       PróximoPago = r.PróximoPago,
                                                       MoraInterior = r.MoraInterior,
                                                       Interes = r.Interes,
                                                       Seguros = r.Seguros,
                                                       Capital = r.Capital,
                                                       TotalPagar = r.TotalPagar,
                                                       ValorInmueble = r.ValorInmueble,
                                                       PlazoObligacion = r.PlazoObligacion,
                                                       Calificacion = r.Calificacion,
                                                       NumeroCuotasFaltantes = r.NumeroCuotasFaltantes,
                                                       ValorCuotaExtraordinaria = r.ValorCuotaExtraordinaria,
                                                       ValorOpcionCompra = r.ValorOpcionCompra,
                                                       CiudadInmueble = r.CiudadInmueble,
                                                       PuntajeScoring = r.PuntajeScoring,
                                                       Nombres = $"{la.SEGUNDO_APELLIDO} {la.PRIMER_APELLIDO} {la.PRIMER_NOMBRE} {la.SEGUNDO_NOMBRE}",

                                                   }).ToList();


            if (list != null && list.Count > 0)
            {
                var result = _templateExcel.GetReporteCarteraTemplateExcel(list);
                list[0].fileExcel = Get(result);
            }
            else
            {
                list = null;
            }


            return new Response<IEnumerable<ReporteGeneralCarteraDto>> { Data = list };

        }


        private FileResult Get(byte[] bytes, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileContentResult = new FileContentResult(bytes, contentType)
            {
                FileDownloadName = $"{name}.xlsx",
                FileContents = bytes
            };

            return fileContentResult;
        }

        /// <summary>
        /// Generacion Reporte Preliminar Cartera
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReportePreliminarCarteraDto>>> ReportePreliminarCartera()
        {
            var response = _mapper.Map<List<ReportePreliminarCarteraDto>>(await _reportesRepository.ReportePreliminarCartera());
            var resultData = await _iReportesRequestProvider.ObtenerClienteCorreosErrados();
            var Clientes = _mapper.Map<List<ReporteClienteCorreosErradosDto>>(resultData.Data);
            var unidad_ejecutoraData = await _reportesRepository.ConsultaUnidadEjecutora();
            var undidad_ejecutora = unidad_ejecutoraData.ToList();

            List<ReportePreliminarCarteraDto> list = (from r in response
                                                      join la in Clientes on r.Identificacion equals la.IDENTIFICACION
                                                      join un in undidad_ejecutora on la.UEJ_ID equals un.UEJ_ID
                                                      select new ReportePreliminarCarteraDto
                                                      {
                                                          Nro = r.Nro,
                                                          NumeroRadicacion = r.NumeroRadicacion,
                                                          Nombres = $"{la.SEGUNDO_APELLIDO} {la.PRIMER_APELLIDO} {la.PRIMER_NOMBRE} {la.SEGUNDO_NOMBRE}",
                                                          Cliente = la.ID_CLIENTE.ToString(),
                                                          Identificacion = r.Identificacion,
                                                          NombrePagaduria = r.NombrePagaduria,
                                                          Apertura = r.Apertura,
                                                          Terminacion = r.Terminacion,
                                                          TasaLinea = r.TasaLinea,
                                                          Monto = r.Monto,
                                                          Saldo = r.Saldo,
                                                          Cuota = r.Cuota,
                                                          UltimoPago = r.UltimoPago,
                                                          ValorInmueble = r.ValorInmueble
                                                      }).ToList();

            if (list != null && list.Count > 0)
            {
                var result = _templateExcel.GetReportePreliminarCarteraTemplateExcel(list);
                list[0].fileExcel = Get(result);
            }
            else
            {
                list = null;
            }
            return new Response<IEnumerable<ReportePreliminarCarteraDto>> { Data = list };
        }

        /// <summary>
        /// Generacion reporte de Garantias
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteGeneralGarantiasDto>>> ReporteGeneralGarantias(ReporteGeneralGarantiaParametersDto parametersDto)
        {
            try
            {

                //requestGarantiasDto requestGarantias = new requestGarantiasDto
                //{
                //    Mes = parametersDto.Mes,
                //    Anio = parametersDto.Anio
                //};
                //IEnumerable<ObtenerGarantiaActualizadaDto> resultadoSaldos = this._mapper.Map<IEnumerable<ObtenerGarantiaActualizadaDto>>(await _carteraRepository.ObtenerCreditosActualizanGarantia(requestGarantias));
                var paramerReport = new ReporteAPNRCARParametersDto()
                {
                    Anio = parametersDto.Anio,
                    Mes = parametersDto.Mes,
                    TipoCredito = parametersDto.TipoCredito,
                    Identificacion = parametersDto.Identificacion
                };
                var report = _mapper.Map<IEnumerable<ReporteAPNRCARDto>>(await _reportesRepository.GenerarReporteAPNRCAR(paramerReport));
                var response = _mapper.Map<IEnumerable<ReporteGeneralGarantiasDto>>(await _reportesRepository.ReporteGeneralGarantias(parametersDto));
                var cc = response.Where(x => x.Identificacion == "1024551295");
                response = response
                    .GroupBy(x => new
                    {
                        x.Nombres,
                        x.FechaTerminacion,
                        x.FechaApertura,
                        x.file,
                        x.Identificacion,
                        x.NumeroRadicacion,
                        x.AjusteGarantia,
                        x.Apellidos,
                        x.TipoCredito,
                        x.ValorAjustado,
                        x.ValorInmueble,
                        x.TipoIdentificacion,
                        x.CiudadInmueble,
                        x.DireccionInmuble,
                        x.NumeroEscrituraInmueble,
                        x.MatriculaInmueble
                    })
                    .SelectMany(g => g) // aplanar los grupos
                    .ToList();
                if (response.Count() > 0 && report.Count() > 0)
                {
                    foreach (var item in response)
                    {
                        item.PorcentajeGarantias = report?.FirstOrDefault(x => x?.Nit == item?.Identificacion)?.PorcentajeDeGarantia ?? 0;
                    }
                }

                if (parametersDto.NumeroCredito != string.Empty && parametersDto.NumeroCredito != null)
                {
                    response = response.Where(x => x.NumeroRadicacion == parametersDto.NumeroCredito).ToList();
                }
                var documentos = response.Select(x => x.Identificacion).Distinct().ToList();
                var afiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(documentos))?.Data;
                var NombreTercero = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(documentos));  //.Distinct().ToList())).Data;

                var reportes = response.GroupJoin(afiliados, s => s.Identificacion, s => s.Identificacion, (rep, cli) =>
                {
                    var cliente = cli?.FirstOrDefault();
                    rep.Nombres = cliente?.Nombres ?? "";
                    rep.Apellidos = cliente?.Apellidos ?? "";
                    return rep;
                }).ToList();

                return response.BuildSuccess();
            }
            catch (Exception)
            {
                return new() { ReturnMessage = "Error al generar el reporte", IsSuccess = false };
            }

        }

        /// <summary>
        /// Generacion reporte RCredito
        /// </summary>
        /// <returns></returns>
        public async Task<Response<FileResult>> GenerarReporteRCredito(ReporteRCreditoParametersDto parametersDto)
        {
            try
            {
                var datosGenerales = _mapper.Map<IEnumerable<ReporteRCreditoDatosGeneralesDto>>(await _reportesRepository.GenerarDatosGeneralesReporteRCredito(parametersDto));

                if (!datosGenerales.Any())
                {
                    return new() { ReturnMessage = "No hay información disponible", IsSuccess = false };
                }

                var informacionLibranza = (parametersDto.Mes is 3 or 6 or 9 or 12)
                    ? _mapper.Map<IEnumerable<ReporteRCreditoInformacionLibranzaDto>>(await _reportesRepository.GenerarInformacionLibranzaReporteRCredito(parametersDto))
                    : null;

                var reporte = _templateExcelReporteRCredito.GenerarReporteRCredito(new()
                {
                    Parametros = parametersDto,
                    DatosGenerales = datosGenerales,
                    InformacionLibranza = informacionLibranza
                });

                return new FileContentResult(reporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    .BuildSuccess<FileResult>();
            }
            catch (Exception)
            {
                return new() { ReturnMessage = "Error al generar el reporte", IsSuccess = false };
            }

        }


        /// <summary>
        /// Generacion reporte de seguro hogar
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteSeguroHogarDto>>> ReporteSeguroHogar()
        {
            var response = _mapper.Map<List<ReporteSeguroHogarDto>>(await _reportesRepository.ReporteSeguroHogar());
            return new Response<IEnumerable<ReporteSeguroHogarDto>> { Data = response };
        }

        /// <summary>
        /// Generacion reporte de 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteSeguroVidaDto>>> ReporteSeguroVida()
        {
            var response = _mapper.Map<List<ReporteSeguroVidaDto>>(await _reportesRepository.ReporteSeguroVida());
            return new Response<IEnumerable<ReporteSeguroVidaDto>> { Data = response };
        }

        /// <summary>
        /// Generacion Reporte Descuentos Nomina
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteDescuentosNominaDto>>> ReporteDescuentosNomina()
        {
            var response = _mapper.Map<List<ReporteDescuentosNominaDto>>(await _reportesRepository.ReporteDescuentosNomina());
            return new Response<IEnumerable<ReporteDescuentosNominaDto>> { Data = response };
        }

        /// <summary>
        /// Generacion de reporte de embargo
        /// </summary>
        /// <param name="idEmbargo"></param>
        /// <returns></returns>
        public async Task<FileResult> ObtenerRespuestaEmbargo(Guid idEmbargo)
        {
            var responseInformacionEmbargo = _mapper.Map<List<ObtenerRespuestaEmbargoDto>>(await _reportesRepository.ObtenerRespuestaEmbargo(idEmbargo));
            var correspondenciaEnviada = await _iGestorDocumentarEmbargosBL.ObtenerCorrespondenciaEmbargos(responseInformacionEmbargo[0].CEDULA_DEMANDADO);
            IEnumerable<EmbargoCuentaConceptoDto> conceptos = _mapper.Map<IEnumerable<EmbargoCuentaConceptoDto>>(await this._juzgadosRepository.ObtenerEmbargoCuentaConceptoPorId(idEmbargo));
            Response<InfoClienteDto> clienteDto = await _cuentasClientesBusinessLogic.ObtenerCuentasConceptosPorTipoNumeroIdentificacion(1, responseInformacionEmbargo[0].CEDULA_DEMANDADO);
            List<EmbargoCuentaConceptoDto> lista = new List<EmbargoCuentaConceptoDto>();

            foreach (var cuenta in clienteDto.Data.Cuentas)
            {
                foreach (var conceptoCuenta in cuenta.Conceptos)
                {
                    foreach (var item in conceptos)
                    {
                        if (conceptoCuenta.Id == item.ConceptoId)
                        {
                            item.Descripcion = conceptoCuenta.ConceptoDescripcion;
                        }
                    }
                }
            }

            lista.AddRange(conceptos);
            var descripcionesEmbargos = new List<string>();
            foreach (var cuenta in clienteDto.Data.Cuentas)
            {
                foreach (var concepto in cuenta.Conceptos)
                {
                    foreach (var item in conceptos)
                    {
                        if (concepto.Id == item.ConceptoId)
                        {
                            string porcentaje = item.TipoEmbargo == "PORCENTAJE" ? "%" : "";
                            string descripcion = $"{cuenta.NombreCuenta}: {item.Descripcion} POR {item.Valor}{porcentaje} {item.TipoRetencion}";
                            descripcionesEmbargos.Add(descripcion);
                        }
                    }
                }
            }

            string descripcionCompletaEmbargo = string.Join(", ", descripcionesEmbargos);
            responseInformacionEmbargo[0].DESCRIPCION = descripcionCompletaEmbargo;


            if (responseInformacionEmbargo != null && responseInformacionEmbargo.Count > 0)
            {
                if (responseInformacionEmbargo[0].Conceptos == null)
                {
                    responseInformacionEmbargo[0].Conceptos = new List<EmbargoCuentaConceptoDto>();
                }
                responseInformacionEmbargo[0].Conceptos.AddRange(lista);

            }

            foreach (var item in correspondenciaEnviada.Data.Form_GetDataResult.Data)
            {
                if (item.Key == "C0002" && item.Value != "")
                {
                    responseInformacionEmbargo[0].NOMBRE_DEMANDADO = item.Value;
                }

                if (item.Key == "C0001" && item.Value != "")
                {
                    responseInformacionEmbargo[0].CEDULA_DEMANDADO = item.Value;
                }
            }

            DataTable dataTableInformacionEmbargo = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionEmbargo));


            FileResult file = await GenerarDocumentoPDF(dataTableInformacionEmbargo, "ReporteRespuestaEmbargo");

            return file;
        }

        public async Task<FileResult> ObtenerRespuestaSolicitudInconsistente(ConsultarSolicitudDto request)
        {
            var resp = await _solicitudBusinessLogic.ConsultarExisteSolicitud(request);
            var response = resp?.Data;

            var trazabilidad = await _solicitudBusinessLogic
                .ObtenerTrazabilidadSolicitud(response.solicitudId);

            Response<ClienteDto> cliente =
                await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(
                    request.tipoIdentificacion,
                    request.identificacion);

            var ciudades = cliente?.Data?.IdDepartamentoResidencia != null
                ? await _catalogsRepository.ObtenerCiudadesPorDepartamento(
                    cliente.Data.IdDepartamentoResidencia)
                : null;

            string ciudad = ciudades?
                .FirstOrDefault(x => x.DPC_CODIGO == cliente?.Data?.IdCiudadResidencia)?
                .DPC_NOMBRE ?? string.Empty;

            // AQUÍ DEBES OBTENER EL TEXTO DE LA INCONSISTENCIA
            string textoInconsistencia =
                request.tareaNombre ?? "N/A";

            var RequestRadicado = new CrearRadicadoCartaIncosistencias
            {
                Asunto = "Información Cumplimiento de Obligaciones V-8",
                clienteId = response.clienteId,
                Codigo = "378",
                IdComunicacion = 0,
                NoFolios = response.Numero_Folio,
                Radicado = response.NumeroRadicadoSG,
                SolicitudId = response.solicitudId
            };

            var respuesta = await _solicitudBusinessLogic.CrearRadicadoCartaIncosistencia(RequestRadicado);

            if (respuesta.IsSuccess)
            {
                await _solicitudRepository.ActualizarEstadoInconsistente(
                    response.solicitudId,
                    respuesta.Data
                );
            }

            var modeloPdf = new CartaInconsistenciaPdfDto
            {
                // Encabezado
                CiudadFecha = $"Bogotá D.C., {DateTime.Now:dd/MM/yyyy}",

                // Datos afiliado
                NombreAfiliado = NormalizarTexto(
                    $"{response?.nombresCliente} {response?.apellidosCliente}"
                ),

                CorreoAfiliado = NormalizarTexto(
                    response?.correoCliente
                ),

                DireccionAfiliado = NormalizarTexto(
                    response?.direccionCliente
                ),

                CiudadAfiliado = NormalizarTexto(
                    ciudad
                ),

                // Caja Radicado
                Radicado = NormalizarTexto(
                    response?.NumeroRadicadoSG
                ),

                Radicado378 = NormalizarTexto(
                   respuesta.Data
                ),

                FechaGeneracion = DateTime.Now.ToString(
                    "dd/MM/yyyy HH:mm"
                ),

                // Cuerpo
                FechaTramite = response?.fechaSolicitud != null
                    ? Convert.ToDateTime(response.fechaSolicitud)
                        .ToString("dd/MM/yyyy")
                    : "N/A",

                TextoInconsistencia = NormalizarTexto(
                    textoInconsistencia
                ),

                NombreSolicitud = NormalizarTexto(
                    response?.descripcionProceso
                ),

                // Firma
                NombreAsesor = response?.SOL_GRUPO_ASIGNADO ?? response?.SOL_NOMBREASIGNADO ?? "N/A",

                CargoAsesor = "Cargo: Asesor"
            };

            var listaModeloPdf = new List<CartaInconsistenciaPdfDto>
            {
                modeloPdf
            };

            DataTable dataTableCarta =
                Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(
                    Newtonsoft.Json.JsonConvert.SerializeObject(listaModeloPdf)
                );

            FileResult file = await GenerarDocumentoPDF(
                dataTableCarta,
                "CartaInconsistencia"
            );
            //await _solicitudRepository.ActualizarEstadoInconsistente(response.solicitudId, Radicado378);

            try
            {
                var fileStreamResult = (FileStreamResult)file;

                fileStreamResult.FileStream.Position = 0;

                var base64 = await file.ToBase64Async();

                fileStreamResult.FileStream.Position = 0;

                var attachRequestDto = new GA2.Application.Dto.AttachRequestDto()
                {
                    Radicado = respuesta.Data,
                    UserOperation = response?.SOL_GRUPO_ASIGNADO ?? response?.SOL_NOMBREASIGNADO ?? "Sin asignar",
                    Files = new List<Application.Dto.FileDto>
                    {
                        new Application.Dto.FileDto
                        {
                            Base64String = base64,
                            Description = respuesta.Data,
                            Ext = "pdf",
                            DirectoryCode = "00",
                            DocumentTypeCode = "00",


                        }
                    }
                };

                await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error generando Carta de Inconsistencia: {ex.Message}"
                );
            }

            return file;
        }

        public async Task<FileResult> ObtenerRespuestaSolicitudDePago(ConsultarSolicitudDto request)
        {
            var resp = await this._solicitudBusinessLogic.ConsultarExisteSolicitud(request);
            var response = resp?.Data;
            var resps = await this._solicitudBusinessLogic.ObtenerTrazabilidadSolicitud(response.solicitudId);

            Response<ClienteDto> cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(
                request.tipoIdentificacion,
                request.identificacion
            );

            var unidades = await _catalogsRepository?.ObtenerUnidadesEjecutoras();
            var grados = await _catalogsRepository.ObtenerGrados();
            var categorias = await _catalogsRepository.ObtenerCategorias();
            var fuerzas = await _catalogsRepository.ObtenerFuerzas();
            var listAfiliaciones = await _catalogsRepository.obtenerTipoActor();
            var documento = await _catalogsRepository.ObtenerDocumentoTipo();
            var estadosCivil = await _catalogsRepository.ObtenerEstadosCiviles();

            var paises = cliente?.Data?.IdPaisResidencia != null
                ? await _catalogsRepository.ObtenerPaises()
                : null;

            var pais = paises?
                    .FirstOrDefault(f => f.DPP_CODIGO == cliente?.Data?.IdPaisResidencia)?
                    .DPP_NOMBRE ?? string.Empty;

            var departamento = cliente?.Data?.IdDepartamentoResidencia != null
                ? await _catalogsRepository.ObtenerDepartamentoPorId(cliente.Data.IdDepartamentoResidencia)
                : null;

            var ciudades = cliente?.Data?.IdDepartamentoResidencia != null
                ? await _catalogsRepository.ObtenerCiudadesPorDepartamento(cliente.Data.IdDepartamentoResidencia)
                : null;

            string fuerzaDescripcion = string.Empty;
            string unidadEjecutoraNombre = string.Empty;
            string gradoDescripcion = string.Empty;
            string ciudad = string.Empty;
            string estadoCivil = string.Empty;

            if (cliente?.Data != null)
            {
                fuerzaDescripcion = fuerzas?
                    .FirstOrDefault(f => f.FRZ_ID == cliente.Data.IdFuerza)?
                    .FRZ_DESCRIPCION ?? string.Empty;

                unidadEjecutoraNombre = unidades?
                    .FirstOrDefault(u => u.UEJ_ID == cliente.Data.IdUnidadEjecutora)?
                    .UEJ_NOMBRE ?? string.Empty;

                gradoDescripcion = grados?
                    .FirstOrDefault(g => g.GRD_ID == cliente.Data.IdGrado)?
                    .GRD_DESCRIPCION ?? string.Empty;

                ciudad = ciudades?
                    .FirstOrDefault(f => f.DPC_CODIGO == cliente.Data.IdCiudadResidencia)?
                    .DPC_NOMBRE ?? string.Empty;

                estadoCivil = estadosCivil?
                   .FirstOrDefault(f => f.CVL_CODIGO == cliente.Data.EstadoCivil)?
                   .CVL_DESCRIPCION ?? string.Empty;
            }

            var beneficiarioEstudio = response?.solicitudTerceroBeneficiarioEstudioDto?.FirstOrDefault();
            var entidadEducativa = response?.solicitudTerceroEntidadEducativaDto?.FirstOrDefault();
            var matricula = response?.solicitudMatriculaDto?.FirstOrDefault();
            var propietario = response?.solicitudPropietarioDto?.FirstOrDefault();
            var destinatario = response?.solicitudTerceroBeneficiarioDto?.FirstOrDefault();

            var modalidad = response?.descripcionModalidad?.Trim();
            var subModalidad = response?.descripcionSubModalidad?.Trim();
            var descripcionProceso = response?.descripcionProceso?.Trim();

            var Escrituracion =
                response?.FechaFirmaEscritura != null &&
                !(NormalizarTexto(descripcionProceso)
                    ?.Contains("RETIRO PARCIAL DE CESANTÍAS") ?? false)
                    ? response.FechaFirmaEscritura.Value.ToString("yyyy-MM-dd")
                    : "N/A";

            var modalidadesSinVendedor = new List<string>
            {
                "ABONO O PAGO TOTAL DEL GRAVAMEN HIPOTECARIO",
                "ADQUISICIÓN DE TERRENO O LOTE SOLAMENTE",
                "AMPLIACIÓN, REPARACIÓN O MEJORA DE LA VIVIENDA",
                "CONSTRUCCIÓN SOBRE LOTE",
                "PAGO IMPUESTO PREDIAL"
            };


            var nombreVendedor = !string.IsNullOrWhiteSpace(propietario?.razonSocial)
                ? propietario.razonSocial
                : string.Join(" ",
                    new[]
                    {
                propietario?.PrimerNombre,
                propietario?.SegundoNombre,
                propietario?.PrimerApellido,
                propietario?.SegundoApellido
                    }.Where(x => !string.IsNullOrWhiteSpace(x)));

            var tipoVivienda = modalidad?.IndexOf("NUEVA", StringComparison.OrdinalIgnoreCase) >= 0
                ? "NUEVA"
                : modalidad?.IndexOf("USADA", StringComparison.OrdinalIgnoreCase) >= 0
                    ? "USADA"
                    : "N/A";

            var modeloPdf = new RespuestaSolicitudDePagoPdfDto
            {
                // Encabezado
                Radicado = NormalizarTexto(response?.NumeroRadicadoSG),
                PuntoRadicacion = "Fisico",
                FechaRadicacion = response?.fechaSolicitud != null
                    ? Convert.ToDateTime(response.fechaSolicitud).ToString("yyyy-MM-dd")
                    : "N/A",
                Folios = response?.Numero_Folio != null ? response.Numero_Folio.ToString() : "N/A",

                // Información afiliado
                Identificacion = NormalizarTexto(response?.identificacionCliente),
                Nombres = NormalizarTexto(response?.nombresCliente),
                Apellidos = NormalizarTexto(response?.apellidosCliente),
                Grado = NormalizarTexto(gradoDescripcion),
                Fuerza = NormalizarTexto(fuerzaDescripcion),
                UnidadEjecutora = NormalizarTexto(unidadEjecutoraNombre),
                Pais = NormalizarTexto(pais),
                Departamento = NormalizarTexto(departamento?.DPD_NOMBRE),
                CiudadMunicipio = NormalizarTexto(ciudad),
                Direccion = NormalizarTexto(response?.direccionCliente),
                Barrio = NormalizarTexto(cliente?.Data?.Barrio),
                EmailPersonal = NormalizarTexto(response?.correoCliente),
                EmailInstitucional = NormalizarTexto(cliente?.Data?.Correo2),
                Telefono = NormalizarTexto(response?.telefonoCliente),
                Celular = NormalizarTexto(response?.celularCliente),
                EstadoCivil = NormalizarTexto(estadoCivil),

                // Cónyuge
                // Cónyuge
                TipoDocumentoConyuge = !string.IsNullOrWhiteSpace(cliente?.Data?.IdentificacionConyuge)
                ? NormalizarTexto(
                documento?.FirstOrDefault(x => x.TID_ID == cliente?.Data?.IdTipoIdentificacionConyuge)?.TID_DESCRIPCION
                  )
                : "N/A",
                NumeroIdentificacionConyuge = NormalizarTexto(cliente?.Data?.IdentificacionConyuge),
                NombresConyuge = NormalizarTexto(string.Join(" ",
                    new[]
                    {
                cliente?.Data?.PrimerNombreConyuge,
                cliente?.Data?.SegundoNombreConyuge
                    }.Where(x => !string.IsNullOrWhiteSpace(x)))),
                ApellidosConyuge = NormalizarTexto(string.Join(" ",
                    new[]
                    {
                cliente?.Data?.PrimerApellidoConyuge,
                cliente?.Data?.SegundoApellidoConyuge
                    }.Where(x => !string.IsNullOrWhiteSpace(x)))),

                // Trámite
                TipoTramite = NormalizarTexto(response?.descripcionProceso),
                Modalidad = NormalizarTexto(
                    string.Equals(modalidad, subModalidad, StringComparison.OrdinalIgnoreCase)
                        ? (modalidad ?? string.Empty)
                        : $"{modalidad} {subModalidad}".Trim()
                ),
                ValorSolicitadoPrimerPago = response?.valorRetirar != null
                    ? response.valorRetirar.ToString()
                    : "N/A",
                IdentificacionEstudiante = NormalizarTexto(beneficiarioEstudio?.numDocEstudio),
                NombreApellidosEstudiante = NormalizarTexto(beneficiarioEstudio?.razonsocialEstudio),
                ProgramaAcademico = NormalizarTexto(entidadEducativa?.ProgramaEducativo),
                FechaPago = entidadEducativa?.fechalimite != null
                    ? Convert.ToDateTime(entidadEducativa.fechalimite).ToString("yyyy-MM-dd")
                    : "N/A",

                // Inmueble
                NumeroMatriculaInmobiliaria = NormalizarTexto(matricula?.numeroMatricula),
                NumeroEscrituraInmueble = NormalizarTexto(response?.SolicitudEscrituracion?.SIE_NUMERO_ESCRITURACION),
                DireccionInmueble = NormalizarTexto(matricula?.direccion),
                DepartamentoInmueble = NormalizarTexto(matricula?.departamento?.descripcion),
                CiudadMunicipioInmueble = NormalizarTexto(matricula?.ciudad?.descripcion),
                Notaria = NormalizarTexto(response?.SolicitudEscrituracion?.SIE_NOTARIA),
                FechaEscrituracion = Escrituracion,


                // Vendedor
                IdentificacionVendedor = !modalidadesSinVendedor.Contains(modalidad)
                ? NormalizarTexto(propietario?.numeroIdentificacion) : "N/A",

                NombreApellidosVendedor =
                !modalidadesSinVendedor.Contains(modalidad)
                    ? NormalizarTexto(nombreVendedor)
                    : "N/A",

                // Vivienda
                TipoVivienda = NormalizarTexto(tipoVivienda),
                ValorSegundoPago = response?.SolicitudAdjudicacion?.SIA_VALOR_SUBSIDIADO != null
                    ? response.SolicitudAdjudicacion.SIA_VALOR_SUBSIDIADO.ToString()
                    : "N/A",

                // Beneficiarios
                Beneficiarios = "N/A",

                // Destinatario pago
                DestinatarioNombre = NormalizarTexto(destinatario?.razonsocial),
                DestinatarioTipoDocumento = NormalizarTexto(
                    documento?.FirstOrDefault(x => x.TID_ID == destinatario?.tipoDoc)?.TID_DESCRIPCION
                ),
                DestinatarioNumeroDocumento = NormalizarTexto(destinatario?.numDoc),
                DestinatarioBanco = NormalizarTexto(destinatario?.entidadObj?.descripcion),
                DestinatarioCheque = "N/A",
                DestinatarioTipoCuenta = NormalizarTexto(destinatario?.tipoCuentaObj?.descripcion),
                DestinatarioNumeroCuenta = NormalizarTexto(destinatario?.numeroCuenta),

                // Declaraciones
                NotificacionEmail = NormalizarTexto(response?.correoCliente),
                NotificacionDireccionCorrespondencia = NormalizarTexto(response?.direccionCliente),

                // Final
                Observaciones = NormalizarTexto(resps?.Data.LastOrDefault()?.Observacion ?? "N/A"),
                FechaImpresion = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var listaModeloPdf = new List<RespuestaSolicitudDePagoPdfDto> { modeloPdf };

            DataTable dataTableInformacionEmbargo =
                Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(
                    Newtonsoft.Json.JsonConvert.SerializeObject(listaModeloPdf)
                );

            FileResult file = await GenerarDocumentoPDF(
                dataTableInformacionEmbargo,
                "ObtenerRespuestaSolicitudDePago"
            );

            try
            {
                var fileStreamResult = (FileStreamResult)file;

                fileStreamResult.FileStream.Position = 0;

                var base64 = await file.ToBase64Async();

                fileStreamResult.FileStream.Position = 0;

                var attachRequestDto = new GA2.Application.Dto.AttachRequestDto()
                {
                    Radicado = response?.NumeroRadicadoSG,
                    UserOperation = response?.SOL_GRUPO_ASIGNADO ?? response?.SOL_NOMBREASIGNADO ?? "Sin asignar",
                    Files = new List<Application.Dto.FileDto>
                    {
                        new Application.Dto.FileDto
                        {
                            Base64String = base64,
                            Description = "Formulario Único de Pago",
                            Ext = "pdf",
                            DirectoryCode = "00",
                            DocumentTypeCode = "00",


                        }
                    }
                };

                await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

            }
            catch (Exception ex)
            {
                Console.WriteLine("el error ha occurido en el FUP");
            }

            return file;
        }

        private static string NormalizarTexto(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return "N/A";

            string texto = valor.ToString()?.Trim();
            return string.IsNullOrWhiteSpace(texto) ? "N/A" : texto;
        }

        public async Task<FileResult> ObtenerRespuestaSolicitud(ConsultarSolicitudDto consultarSolicitudDto)
        {
            var clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(
                consultarSolicitudDto.tipoIdentificacion,
                consultarSolicitudDto.identificacion,
                0
            );

            var consultarSolicitud = new ConsultarSolicitudPorActividad
            {
                SLT_ID = consultarSolicitudDto.solicitudTareaId
            };

            var respuesta = await _solicitudRepository.ConsultarSolicitudPorActividad(consultarSolicitud);


            if (respuesta.TRA_NOMBRE != null && respuesta.TRA_NOMBRE.Contains("Seguridad documental") && respuesta.INC_INCONSISTENCIA == "INFORME 114 - INCONSISTENCIA")
            {
                respuesta.PCS_DESCRIPCION = "Teniendo en cuenta lo anterior, no es procedente su solicitud de " + respuesta.PCS_DESCRIPCION + ", razón por la cual la presente solicitud se da por finalizada.";

            }
            else
            {
                respuesta.PCS_DESCRIPCION = "Teniendo en cuenta lo anterior, su solicitud de " + respuesta.PCS_DESCRIPCION + ", estara en proceso de verificación.";
            }
            var mappedRespuesta = _mapper.Map<ObtenerTramiteSolicitudDto>(respuesta);

            if (mappedRespuesta != null)
            {
                mappedRespuesta.direccionCliente = clienteDto.Data.Direccion;
                mappedRespuesta.correoCliente = clienteDto.Data.CorreoPersonal;
                mappedRespuesta.direccionCliente = clienteDto.Data.Direccion;
                mappedRespuesta.nombresCliente =
                clienteDto.Data.PrimerNombre + " " +
                clienteDto.Data.SegundoNombre + " " +
                clienteDto.Data.PrimerApellido + " " +
                clienteDto.Data.SegundoApellido;
                mappedRespuesta.observacionSolicitud = consultarSolicitudDto.tareaNombre;
                mappedRespuesta.descripcionSubModalidad = consultarSolicitudDto.solicitante;

            }

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(mappedRespuesta);


            if (!jsonData.TrimStart().StartsWith("["))
            {
                jsonData = $"[{jsonData}]";
            }

            try
            {
                DataTable dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(jsonData);
                FileResult file = await GenerarDocumentoPDF(dataTableInformacionSolicitud, "ReporteRespuestaSolicitud");

                var fileStreamResult = (FileStreamResult)file;

                fileStreamResult.FileStream.Position = 0;

                var base64 = await file.ToBase64Async();

                fileStreamResult.FileStream.Position = 0;

                var attachRequestDto = new GA2.Application.Dto.AttachRequestDto()
                {
                    Radicado = respuesta?.SOL_RADICADO_GD,
                    UserOperation = respuesta?.SOL_GRUPO_ASIGNADO ?? respuesta?.SOL_NOMBREASIGNADO ?? "Sin asignar",
                    Files = new List<Application.Dto.FileDto>
                    {
                        new Application.Dto.FileDto
                        {
                            Base64String = base64,
                            Description = "Carta De Inconsistencia",
                            Ext = "pdf",
                            DirectoryCode = "00",
                            DocumentTypeCode = "00",


                        }
                    }
                };

                await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                return file;
            }
            catch (JsonSerializationException ex)
            {
                throw new InvalidOperationException("Error al deserializar la información de la solicitud.", ex);
            }
        }


        /// <summary>
        /// ReportePreliminarGarantias
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReportePreliminarGarantiasDto>>> ReportePreliminarGarantias()
        {
            List<ReportePreliminarGarantiasDto> list = new List<ReportePreliminarGarantiasDto>();
            var response = _mapper.Map<List<ReportePreliminarGarantiasDto>>(await _reportesRepository.ReportePreliminarGarantias());

            return new Response<IEnumerable<ReportePreliminarGarantiasDto>> { Data = list };
        }

        /// <summary>
        /// Funcion para Reporte de Transferencia Cesantias PONAL
        /// </summary>
        /// <param name="idAfiliacion"></param>
        /// <returns></returns>
        public async Task<FileResult> TransferenciaCesantiasPONAL(int idAfiliacion)
        {
            // Consulto en BD la informacion
            var responseInformacionAfiliado = await _iReportesRequestProvider.ObtenerDatosOficioRespuesta(idAfiliacion);

            if (!responseInformacionAfiliado.Data.Any())
            {
                throw new InvalidOperationException($"No existe información en la base de datos para el Id Afiliacion: {idAfiliacion}");
            }

            // Consulta a BD y serializacion del objeto en un DT
            DataTable dataTableInformacionAfiliado = ObtainDataTableFromIEnumerable(responseInformacionAfiliado.Data);

            // Obtener datos iniciales para cargar nombre del documento en el DT 
            TipoIdentificacionDto documento = _mapper.Map<TipoIdentificacionDto>(await _juzgadosRepository.
                ObtenerTipoIdentificacionPorId(int.Parse(dataTableInformacionAfiliado.Rows[0].ItemArray[5].ToString())));

            // Con LINQ relleno los valores de BUA que se extraen de GA2
            dataTableInformacionAfiliado.Rows.Cast<DataRow>()
                .Where(x => Convert.ToString(x["DOCUMENTO"]) == "").ToList()
                .ForEach(x => x.SetField("DOCUMENTO", documento.Descripcion));

            FileResult file = await GenerarDocumentoPDF(dataTableInformacionAfiliado, "ReporteTransferenciaCesantiasPONAL");

            return file;

        }

        /// <summary>
        /// Funcion para Reporte de Transferencia Cesantias
        /// </summary>
        /// <param name="idAfiliacion"></param>
        /// <returns></returns>
        public async Task<FileResult> TransferenciaCesantias(int idAfiliacion)
        {
            // Consulto en BD la informacion
            var responseInformacionAfiliado = await _iReportesRequestProvider.ObtenerDatosOficioRespuesta(idAfiliacion);


            if (!responseInformacionAfiliado.Data.Any())
            {
                throw new InvalidOperationException($"No existe información en la base de datos para el Id Afiliacion: {idAfiliacion}");
            }

            // Consulta a BD y serializacion del objeto en un DT
            DataTable dataTableInformacionAfiliado = ObtainDataTableFromIEnumerable(responseInformacionAfiliado.Data);

            // Obtener datos iniciales para cargar nombre del documento en el DT 
            TipoIdentificacionDto documento = _mapper.Map<TipoIdentificacionDto>(await _juzgadosRepository.
                ObtenerTipoIdentificacionPorId(int.Parse(dataTableInformacionAfiliado.Rows[0].ItemArray[5].ToString())));

            // Con LINQ relleno los valores de BUA que se extraen de GA2
            dataTableInformacionAfiliado.Rows
                .Cast<DataRow>()
                .Where(x => Convert.ToString(x["DOCUMENTO"]) == "").ToList()
                .ForEach(x => x.SetField("DOCUMENTO", documento.Descripcion));

            FileResult file = await GenerarDocumentoPDF(dataTableInformacionAfiliado, "ReporteTransferenciaCesantias");

            return file;

        }

        /// <summary>
        /// RespuestaNoProcedente
        /// </summary>
        /// <param name="idAfiliacion"></param>
        /// <returns></returns>
        public async Task<FileResult> RespuestaNoProcedente(int idAfiliacion)
        {
            // Consulto en BD la informacion
            var responseInformacionAfiliado = await _iReportesRequestProvider.ObtenerDatosOficioImprocedente(idAfiliacion);

            if (!responseInformacionAfiliado.Data.Any())
            {
                throw new InvalidOperationException($"No existe información en la base de datos para el Id Afiliacion: {idAfiliacion}");
            }

            // Consulta a BD y serializacion del objeto en un DT
            DataTable dataTableInformacionAfiliado = ObtainDataTableFromIEnumerable(responseInformacionAfiliado.Data);

            FileResult file = await GenerarDocumentoPDF(dataTableInformacionAfiliado, "ReporteRespuestaNoProcedente");

            return file;

        }

        /// <summary>
        /// PreaprobadoLeasing
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        public async Task<FileResult> PreaprobadoLeasing(string identificacion)
        {

            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blogStorage.SRD_ESTADO)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");



                BlobClient blobBase = container.GetBlobClient("TemplateLeasing.html");
                BlobClient blobCss = container.GetBlobClient("TemplateLeasing.css");

                BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

                var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

                await baseHtlm.FileStream.FlushAsync();
                await baseCss.FileStream.FlushAsync();

                // Consulto en BD la informacion
                var responseInformacionAfiliado = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
                var dataCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(responseInformacionAfiliado.Data.FirstOrDefault().TID_ID, identificacion);
                var responseInformacionSolicitud = _mapper.Map<List<PreaprobadoClienteDto>>(await _creditoRepository.SimulacionCliente(identificacion));
                var responseParametrizacionCarterra = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();
                if (!responseInformacionAfiliado.Data.Any())
                {
                    throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {identificacion}");
                }

                if (!responseInformacionSolicitud.Any())
                {
                    throw new InvalidOperationException($"No existe información de la solicitud en la base de datos para la Afiliacion: {identificacion}");
                }

                DataTable dataTableInformacionAfiliado = new DataTable();
                DataTable dataTableInformacionSolicitud = new DataTable();

                // Consulta a BD y serializacion del objeto en un DT
                // dataTableInformacionAfiliado = ObtainDataTableFromIEnumerable(responseInformacionAfiliado.Data);

                var dataInformacionAfiliado = responseInformacionAfiliado.Data.ToList<InformacionClienteDto>();
                dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionSolicitud));

                Dictionary<string, string> Values = new Dictionary<string, string>();
                string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
                string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();
                var ciudad = _mapper.Map<CiudadDto>(await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(int.Parse(dataInformacionAfiliado[0].DPD_ID.ToString()), int.Parse(dataInformacionAfiliado[0].DPC_ID.ToString()))).Nombre.ToString();
                List<CategoriaDto> listCategorias = _mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias()).Where(x => x.Id == int.Parse(dataCliente.Data.IdCategoria.ToString())).ToList();
                List<UnidadesEjecutorasDto> listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(await this._carteraRepository.ConsultarUnidadEjecutora()).Where(x => x.idUnidadEjecutora == int.Parse(dataCliente.Data.IdUnidadEjecutora.ToString())).ToList();
                string data = string.Empty;
                string modalidad = string.Empty;
                string puntoatencion = string.Empty;
                foreach (var item in responseInformacionSolicitud)
                {
                    data = $"AL CONTESTAR CITE:{item.NUMERO_RADICADO}<br>Fecha:{item.FECHA_ACTUALIZA.ToString("dd/MM/yyyy")}<br>Área: {item.PUNTOATENCION}";
                    modalidad = item.MODALIDAD;
                    puntoatencion = item.PUNTOATENCION;
                }
                var paramCartera = responseParametrizacionCarterra.Data;
                var totalMeses = int.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
                BlobContainerClient img = new BlobContainerClient(this._options.Value.CadenaOne, "imagenes-caja");
                BlobClient header = img.GetBlobClient("Header.html");
                BlobClient footer = img.GetBlobClient("Footer.html");

                BlobDownloadInfo downloadHeader = await header.DownloadAsync();
                BlobDownloadInfo downloadFooter = await footer.DownloadAsync();

                var baseHeaderhtml = new FileStreamResult(downloadHeader.Content, downloadHeader.ContentType);
                var baseFooterhtml = new FileStreamResult(downloadFooter.Content, downloadFooter.ContentType);

                await baseHeaderhtml.FileStream.FlushAsync();
                await baseFooterhtml.FileStream.FlushAsync();
                string Header = await (new StreamReader(baseHeaderhtml.FileStream)).ReadToEndAsync();
                string Footer = await (new StreamReader(baseFooterhtml.FileStream)).ReadToEndAsync();

                Double anos = totalMeses / 12.0;


                double valor = Convert.ToDouble(totalMeses);
                double mesesRedondeado = Math.Round(valor, 1);


                // Reemplazar valores en el HTML
                Values.Add("PARAM_modalidad", !string.IsNullOrEmpty(modalidad) ? modalidad : "");
                Values.Add("PARAM_data", data);
                Values.Add("PARAM_Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                Values.Add("PARAM_Correoelectronico", dataInformacionAfiliado[0].DCE_CORREO);
                Values.Add("PARAM_Afiliado", dataInformacionAfiliado[0].CLI_NOMBRES.ToString().ToUpper());
                Values.Add("PARAM_Correo", dataInformacionAfiliado[0].DCE_CORREO.ToString());
                Values.Add("PARAM_ValorCredito", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[4].ToString()).ToString("N0").Replace("€", ""));
                Values.Add("PARAM_ValorCuota", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[3].ToString()).ToString("N0").Replace("€", ""));
                Values.Add("PARAM_Plazo", dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
                Values.Add("PARAM_me", mesesRedondeado.ToString());
                Values.Add("PARAM_tarifaunica", paramCartera.tarifaUnicaAvaluos.ToString());
                Values.Add("PARAM_tarifaestudiotitulos", paramCartera.tarifaEstudioTitulos.ToString());
                Values.Add("Param_atencion", puntoatencion);
                Values.Add("PARAM_Cargo", listCategorias[0].Descripcion);
                Values.Add("PARAM_Entidad", listUnidades.FirstOrDefault().nombreUnidadEjecutora);
                Values.Add("PARAM_Direccion", dataInformacionAfiliado[0].DRC_DIRECCION);
                Values.Add("PARAM_Ciudad", ciudad);
                Values.Add("PARAM_an", anos.ToString());

                string base64Documento = await CrearDocumentoBase64(
                                        Basedocument,
                                        Styles,
                                        TipoDocumentoArchivo.PDF,
                                        Guid.NewGuid().ToString(),
                                        Values,
                                        identificacion.Substring(identificacion.Length - 4),
                                        Header,
                                        Footer,
                                        true,
                                        10,
                                        10
                                    );

                await CargarDocumentoProspeccionBase64(base64Documento, identificacion);

                return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), Header, Footer, Values, identificacion.Substring(identificacion.Length - 4), true);
            }
            else
            {
                return await PreaprobadoLeasingLocal(identificacion);
            }
        }

        public async Task<FileResult> PreaprobadoLeasingLocal(string identificacion)
        {



            var responseInformacionAfiliado = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
            var dataCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(
                responseInformacionAfiliado.Data.FirstOrDefault().TID_ID, identificacion);
            var responseInformacionSolicitud = _mapper.Map<List<PreaprobadoClienteDto>>(await _creditoRepository.SimulacionCliente(identificacion));
            var responseParametrizacionCarterra = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();
            decimal saldosCanonRecursos = 0; 
            var informacionSimulacion = await _creditoRepository.ObtenerSolicitudSimulacionCreditoNumeroDocumento(identificacion);

            string localPath = _configureOptions.CarpetaServidorStorage + "\\reportes-sfc";
            string imgPath = _configureOptions.CarpetaServidorStorage + "\\imagenes-caja";

            string filePathHtml = "";
            string filePathCss = "";
            var simulacion = await this._creditoRepository.ConsultarSimulacionCliente(identificacion);

            decimal valorCanonInicial = simulacion.SMC_VALOR_VIVIENDA * (simulacion.SMC_PORCEN_CANON_INICIAL / 100); 

            if (informacionSimulacion.SLS_SIMULADOR_DESC == "LEASING VETERANOS")
            {
                filePathHtml = Path.Combine(localPath, "TemplateLeasingVeterano.html");
                filePathCss = Path.Combine(localPath, "TemplateLeasingVeterano.css");
            }
            else
            {
                filePathHtml = Path.Combine(localPath, "TemplateLeasing.html");
                filePathCss = Path.Combine(localPath, "TemplateLeasing.css");

            }



            string filePathHeader = Path.Combine(imgPath, "Header.html");
            string filePathFooter = Path.Combine(imgPath, "Footer.html");

            if (!File.Exists(filePathHtml) || !File.Exists(filePathCss))
            {
                throw new FileNotFoundException("No se encontraron los archivos de plantilla HTML o CSS en la ruta especificada.");
            }

            string baseHtml = await File.ReadAllTextAsync(filePathHtml);
            string styles = await File.ReadAllTextAsync(filePathCss);

            if (!File.Exists(filePathHeader) || !File.Exists(filePathFooter))
            {
                throw new FileNotFoundException("No se encontraron los archivos de encabezado o pie de página en la ruta especificada.");
            }

            string header = await File.ReadAllTextAsync(filePathHeader);
            string footer = await File.ReadAllTextAsync(filePathFooter);


            if (!responseInformacionAfiliado.Data.Any())
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliación: {identificacion}");
            }

            if (!responseInformacionSolicitud.Any())
            {
                throw new InvalidOperationException($"No existe información de la solicitud en la base de datos para la Afiliación: {identificacion}");
            }

            DataTable dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(
                Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionSolicitud));
            var dataInformacionAfiliado = responseInformacionAfiliado.Data.ToList<InformacionClienteDto>();
            var ciudad = _mapper.Map<CiudadDto>(
                await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(
                    int.Parse(dataInformacionAfiliado[0].DPD_ID.ToString()),
                    int.Parse(dataInformacionAfiliado[0].DPC_ID.ToString())
                )).Nombre;

            List<CategoriaDto> listCategorias = _mapper.Map<IEnumerable<CategoriaDto>>(
                await this._catalogsRepository.ObtenerCategorias()).Where(
                x => x.Id == int.Parse(dataCliente.Data.IdCategoria.ToString())).ToList();

            List<UnidadesEjecutorasDto> listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(
                await this._carteraRepository.ConsultarUnidadEjecutora()).Where(
                x => x.idUnidadEjecutora == int.Parse(dataCliente.Data.IdUnidadEjecutora.ToString())).ToList();

            Dictionary<string, string> values = new Dictionary<string, string>();
            string data = string.Empty;
            string modalidad = string.Empty;
            string puntoatencion = string.Empty;



            foreach (var item in responseInformacionSolicitud)
            {
                data = $"AL CONTESTAR CITE:{item.SLS_NUMERO_RADICADO_83}<br>Fecha:{item.FECHA_ACTUALIZA.ToString("dd/MM/yyyy HH:mm:ss")}<br>Área: Grupo de Originación de crédito";
                modalidad = item.MODALIDAD;
                puntoatencion = item.PUNTOATENCION;
            }

            var paramCartera = responseParametrizacionCarterra.Data;
            var totalMeses = int.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
            double anos = totalMeses / 12.0;

            double valor = Convert.ToDouble(totalMeses);
            double mesesRedondeado = Math.Round(valor, 1);

            foreach(var item in dataCliente.Data.Saldos)
            {
                saldosCanonRecursos += item.Valor; 
            }

            string valorCanonInicialTotal = valorCanonInicial > saldosCanonRecursos ? saldosCanonRecursos.ToString("C2", new System.Globalization.CultureInfo("es-CO")) : valorCanonInicial.ToString("C2", new System.Globalization.CultureInfo("es-CO")); 

            values.Add("PARAM_modalidad", !string.IsNullOrEmpty(modalidad) ? modalidad : "");
            values.Add("PARAM_data", data);
            values.Add("PARAM_Fecha", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            values.Add("PARAM_Correoelectronico", dataCliente.Data.Correo2);
            values.Add("PARAM_Afiliado", dataInformacionAfiliado[0].CLI_NOMBRES.ToUpper());
            values.Add("PARAM_Correo", dataCliente.Data.Correo2);
            values.Add("PARAM_ValorCredito", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[4].ToString()).ToString("N0").Replace("€", ""));
            values.Add("PARAM_ValorCuota", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[3].ToString()).ToString("C2", new System.Globalization.CultureInfo("es-CO")));
            values.Add("PARAM_valorCanonInicial", valorCanonInicialTotal);
            values.Add("PARAM_Plazo", dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
            values.Add("PARAM_me", mesesRedondeado.ToString());
            values.Add("PARAM_tarifaunica", paramCartera.tarifaUnicaAvaluos.ToString("C0", new System.Globalization.CultureInfo("es-CO")));
            values.Add("PARAM_tarifaestudiotitulos", paramCartera.tarifaEstudioTitulos.ToString("C0", new System.Globalization.CultureInfo("es-CO")));
            values.Add("Param_atencion", puntoatencion);
            values.Add("PARAM_Cargo", listCategorias[0].Descripcion);
            values.Add("PARAM_Entidad", listUnidades.FirstOrDefault().nombreUnidadEjecutora);
            values.Add("PARAM_Direccion", dataInformacionAfiliado[0].DRC_DIRECCION);
            values.Add("PARAM_Ciudad", ciudad);
            values.Add("PARAM_an", anos.ToString("F2"));


            string base64Documento = await CrearDocumentoBase64(
                                        baseHtml,
                                        styles,
                                        TipoDocumentoArchivo.PDF,
                                        Guid.NewGuid().ToString(),
                                        values,
                                        identificacion.Substring(identificacion.Length - 4),
                                        header,
                                        footer,
                                        true,
                                        10,
                                        10
                                    );

            var email = new Entities.Correo.Email
            {
                Modulo = "credito",

                RutaHtml = "TemplatePreaprobadoHipotecario.html",

                RutaImagenEncabezado = "encabezadoRechazoCredito.png",

                Asunto = "Comunicación oficial de Preaprobado de crédito leasing",

                Destinatarios = new List<string>
                {
                    "analisis.procesos@grupokriterion.com.co"
                },

                Parametros = new Dictionary<string, string>
                {
                    { "NombreCompleto", $"{dataCliente.Data.PrimerNombre} {dataCliente.Data.SegundoNombre} {dataCliente.Data.PrimerApellido} {dataCliente.Data.SegundoApellido}" }
                },
                AdjuntoBase64 = base64Documento,
                EnvioBase64 = true, 
                NombreDocumentoAdjunto = "Comunicación oficial de preaprobado"

            };

            await _emailNotificacionRepository.EnviarAsync(email);

            await CargarDocumentoProspeccionBase64(base64Documento, identificacion);

            byte[] bytes = Convert.FromBase64String(base64Documento);

            using var stream = new MemoryStream(bytes);

            IFormFile file = new FormFile(stream, 0, bytes.Length, "archivo", "documento.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var rutaImagen = await _creditoBusinessLogic.CargaDocumentoSolicCredito(file);


            #region Busqueda Documnetos PreAprobado
            DocumentosActSimulacionDto documentosActSimulacionDto = new DocumentosActSimulacionDto()
            {
                SLS_ID = informacionSimulacion.SLS_ID,
                AFL_ID = 1
            };
            var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(documentosActSimulacionDto);
            var Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Contains("Comunicación oficial de preaprobado")).Select(x => x.TDC_ID).FirstOrDefault();

            #endregion
            RequestDocumentoSimDto requestDocumentoSimDto = new RequestDocumentoSimDto()
            {
                idTipoDocumento = 86,
                codigoBarras = "",
                creadoPor = new Guid(),
                fechaCreacion = DateTime.Now,
                rutaImagen = rutaImagen.Data,
                idSimulacion = informacionSimulacion.SLS_ID
            };

              //await this._creditoBusinessLogic.CrearDocumentoSimulacion(requestDocumentoSimDto);


            // Crear el documento PDF
            return await CrearDocumentos(baseHtml, styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), header, footer, values, identificacion.Substring(identificacion.Length - 4), true);
        }


        /// <summary>
        /// PreaprobadoHipotecario
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        public async Task<FileResult> PreaprobadoHipotecario(string identificacion)
        {
            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blogStorage.SRD_ESTADO)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");


                var simulacionInformacion = await this._creditoRepository.ObtenerSimulacionPersonales(identificacion);

                BlobClient blobBase = null;
                BlobClient blobCss = null;

                if (simulacionInformacion.SDP_COMPRA_CARTERA)
                {
                    blobBase = container.GetBlobClient("TemplateCompraCartera.html");
                    blobCss = container.GetBlobClient("TemplateCompraCartera.css");
                }
                else
                {
                    blobBase = container.GetBlobClient("TemplateHipotecario.html");
                    blobCss = container.GetBlobClient("TemplateHipotecario.css");
                }


                BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

                var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

                await baseHtlm.FileStream.FlushAsync();
                await baseCss.FileStream.FlushAsync();

                // Consulto en BD la informacion
                var responseInformacionAfiliadoHP = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
                var dataCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(responseInformacionAfiliadoHP.Data.FirstOrDefault().TID_ID, identificacion);
                var responseInformacionSolicitudHP = _mapper.Map<List<PreaprobadoClienteDto>>(await _creditoRepository.SimulacionCliente(identificacion));
                var responseParametrizacionCarterra = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();

                if (!responseInformacionAfiliadoHP.Data.Any())
                {
                    throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {identificacion}");
                }

                if (!responseInformacionSolicitudHP.Any())
                {
                    throw new InvalidOperationException($"No existe información de la solicitud en la base de datos para la Afiliacion: {identificacion}");
                }



                DataTable dataTableInformacionAfiliadoHP = new DataTable();
                DataTable dataTableInformacionSolicitud = new DataTable();

                // Consulta a BD y serializacion del objeto en un DT
                //dataTableInformacionAfiliadoHP = ObtainDataTableFromIEnumerable(responseInformacionAfiliadoHP.Data);
                var dataInformacionAfiliado = responseInformacionAfiliadoHP.Data.ToList<InformacionClienteDto>();
                dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionSolicitudHP));

                var paramCartera = responseParametrizacionCarterra.Data;
                BlobContainerClient img = new BlobContainerClient(this._options.Value.CadenaOne, "imagenes-caja");
                BlobClient header = img.GetBlobClient("Header.html");
                BlobClient footer = img.GetBlobClient("Footer.html");

                BlobDownloadInfo downloadHeader = await header.DownloadAsync();
                BlobDownloadInfo downloadFooter = await footer.DownloadAsync();

                var baseHeaderhtml = new FileStreamResult(downloadHeader.Content, downloadHeader.ContentType);
                var baseFooterhtml = new FileStreamResult(downloadFooter.Content, downloadFooter.ContentType);

                await baseHeaderhtml.FileStream.FlushAsync();
                await baseFooterhtml.FileStream.FlushAsync();
                string Header = await (new StreamReader(baseHeaderhtml.FileStream)).ReadToEndAsync();
                string Footer = await (new StreamReader(baseFooterhtml.FileStream)).ReadToEndAsync();
                Dictionary<string, string> Values = new Dictionary<string, string>();
                string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
                string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();
                var ciudad = _mapper.Map<CiudadDto>(await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(int.Parse(dataInformacionAfiliado.FirstOrDefault().DPD_ID.ToString()), int.Parse(dataInformacionAfiliado.FirstOrDefault().DPC_ID.ToString()))).Nombre.ToString();
                List<CategoriaDto> listCategorias = _mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias()).Where(x => x.Id == int.Parse(dataCliente.Data.IdCategoria.ToString())).ToList();
                List<UnidadesEjecutorasDto> listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(await this._carteraRepository.ConsultarUnidadEjecutora()).Where(x => x.idUnidadEjecutora == int.Parse(dataCliente.Data.IdUnidadEjecutora.ToString())).ToList();
                string data = string.Empty;
                string modalidad = string.Empty;
                string puntoAtencion = string.Empty;
                foreach (var item in responseInformacionSolicitudHP)
                {
                    data = $"AL CONTESTAR CITE:{item.NUMERO_RADICADO}<br>Fecha:{item.FECHA_ACTUALIZA.ToString("dd/MM/yyyy")}<br>Área: {item.PUNTOATENCION}";
                    modalidad = item.MODALIDAD;
                    puntoAtencion = item.PUNTOATENCION;
                }
                var totalMeses = int.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
                Double anos = totalMeses / 12.0;

                double valor = Convert.ToDouble(totalMeses);
                double mesesRedondeado = Math.Round(valor, 1);

                // Reemplazar valores en el HTML
                Values.Add("PARAM_modalidad", !string.IsNullOrEmpty(modalidad) ? modalidad : "");
                Values.Add("PARAM_data", data);
                Values.Add("PARAM_Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                Values.Add("PARAM_Correoelectronico", dataInformacionAfiliado[0].DCE_CORREO);
                Values.Add("PARAM_Afiliado", dataInformacionAfiliado[0].CLI_NOMBRES.ToString().ToUpper());
                Values.Add("PARAM_Correo", dataInformacionAfiliado[0].DCE_CORREO.ToString());
                Values.Add("PARAM_ValorCredito", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[4].ToString()).ToString("N0").Replace("€", ""));
                Values.Add("PARAM_ValorCuota", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[3].ToString()).ToString("N0").Replace("€", ""));
                Values.Add("PARAM_Plazo", dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
                Values.Add("PARAM_me", mesesRedondeado.ToString());
                Values.Add("PARAM_tarifaunica", paramCartera.tarifaUnicaAvaluos.ToString());
                Values.Add("PARAM_tarifaestudiotitulos", paramCartera.tarifaEstudioTitulos.ToString());
                Values.Add("Param_atencion", puntoAtencion);
                Values.Add("PARAM_Cargo", listCategorias[0].Descripcion);
                Values.Add("PARAM_Entidad", listUnidades.FirstOrDefault().nombreUnidadEjecutora);
                Values.Add("PARAM_Direccion", dataInformacionAfiliado[0].DRC_DIRECCION);
                Values.Add("PARAM_Ciudad", ciudad);
                Values.Add("PARAM_an", anos.ToString());

                string base64Documento = await CrearDocumentoBase64(
                                        Basedocument,                       // HTML
                                        Styles,                              // CSS
                                        TipoDocumentoArchivo.PDF,            // Tipo documento
                                        Guid.NewGuid().ToString(),           // Nombre archivo
                                        Values,                              // datosReemplazo
                                        identificacion.Substring(identificacion.Length - 4),  // contraseña
                                        Header,                              // header
                                        Footer,                              // footer
                                        true,                                // margins
                                        10,                                  // marginLeft
                                        10                                   // marginRight
                                    );

                await CargarDocumentoProspeccionBase64(base64Documento, identificacion);


                return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), Header, Footer, Values, identificacion.Substring(identificacion.Length - 4), true);
            }
            else
            {
                return await PreaprobadoHipotecarioLocal(identificacion);
            }

        }


        public async Task<FileResult> PreaprobadoHipotecarioLocal(string identificacion)
        {
            string basePath = _configureOptions.CarpetaServidorStorage + "\\reportes-sfc";
            string imgPath = _configureOptions.CarpetaServidorStorage + "\\imagenes-caja";


            var simulacionInformacion = await this._creditoRepository.ObtenerSimulacionPersonales(identificacion);
            var informacionSimulacion = await _creditoRepository.ObtenerSolicitudSimulacionCreditoNumeroDocumento(identificacion);


            string templateHtmlPath = "";
            string templateCssPath = "";

            if (simulacionInformacion.SDP_OPCION_COMPRA_CARTERA)
            {
                templateHtmlPath = Path.Combine(basePath, "TemplateCompraCartera.html");
                templateCssPath = Path.Combine(basePath, "TemplateCompraCartera.css");
            }
            else
            {
                templateHtmlPath = Path.Combine(basePath, "TemplateHipotecario.html");
                templateCssPath = Path.Combine(basePath, "TemplateHipotecario.css");
            }



            string headerPath = Path.Combine(imgPath, "Header.html");
            string footerPath = Path.Combine(imgPath, "Footer.html");

            if (!File.Exists(templateHtmlPath) || !File.Exists(templateCssPath))
            {
                throw new FileNotFoundException("No se encontraron las plantillas necesarias en la carpeta 'reportes-sfc'.");
            }

            if (!File.Exists(headerPath) || !File.Exists(footerPath))
            {
                throw new FileNotFoundException("No se encontraron las plantillas 'Header.html' o 'Footer.html' en la carpeta 'imagenes-caja'.");
            }

            string baseHtml = await File.ReadAllTextAsync(templateHtmlPath);
            string baseCss = await File.ReadAllTextAsync(templateCssPath);
            string headerHtml = await File.ReadAllTextAsync(headerPath);
            string footerHtml = await File.ReadAllTextAsync(footerPath);

            var responseInformacionAfiliadoHP = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
            if (!responseInformacionAfiliadoHP.Data.Any())
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la afiliación: {identificacion}");
            }

            var dataCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(
                responseInformacionAfiliadoHP.Data.FirstOrDefault().TID_ID, identificacion);

            var responseInformacionSolicitudHP = _mapper.Map<List<PreaprobadoClienteDto>>(await _creditoRepository.SimulacionCliente(identificacion));
            if (!responseInformacionSolicitudHP.Any())
            {
                throw new InvalidOperationException($"No existe información de la solicitud en la base de datos para la afiliación: {identificacion}");
            }

            var responseParametrizacionCarterra = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();
            var paramCartera = responseParametrizacionCarterra.Data;

            var ciudad = _mapper.Map<CiudadDto>(
                await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(
                    int.Parse(responseInformacionAfiliadoHP.Data.FirstOrDefault().DPD_ID.ToString()),
                    int.Parse(responseInformacionAfiliadoHP.Data.FirstOrDefault().DPC_ID.ToString())
                )
            ).Nombre.ToString();

            List<CategoriaDto> listCategorias = _mapper.Map<IEnumerable<CategoriaDto>>(
                await _catalogsRepository.ObtenerCategorias()
            ).Where(x => x.Id == int.Parse(dataCliente.Data.IdCategoria.ToString())).ToList();

            List<UnidadesEjecutorasDto> listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(
                await _carteraRepository.ConsultarUnidadEjecutora()
            ).Where(x => x.idUnidadEjecutora == int.Parse(dataCliente.Data.IdUnidadEjecutora.ToString())).ToList();

            DataTable dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(
                Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionSolicitudHP));

            var totalMeses = int.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
            double anos = totalMeses / 12.0;

            double valor = Convert.ToDouble(totalMeses);
            double mesesRedondeado = Math.Round(valor, 1);

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string data = string.Empty;
            string modalidad = string.Empty;
            string puntoAtencion = string.Empty;

            foreach (var item in responseInformacionSolicitudHP)
            {

                data = $"AL CONTESTAR CITE:{item.SLS_NUMERO_RADICADO_83}<br>Fecha:{item.FECHA_ACTUALIZA.ToString("dd/MM/yyyy hh:mm:ss tt").Replace("AM", "a.m.").Replace("PM", "p.m.")}<br>Área: Grupo de Originación de crédito";
                modalidad = item.MODALIDAD;
                puntoAtencion = item.PUNTOATENCION;
            }

            Values.Add("PARAM_modalidad", !string.IsNullOrEmpty(modalidad) ? modalidad : "");
            Values.Add("PARAM_data", data);
            Values.Add("PARAM_Fecha", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").Replace("AM", "a.m.").Replace("PM", "p.m."));
            Values.Add("PARAM_Correoelectronico", dataCliente.Data.Correo2);
            Values.Add("PARAM_Afiliado", responseInformacionAfiliadoHP.Data.FirstOrDefault().CLI_NOMBRES.ToUpper());
            Values.Add("PARAM_Correo", dataCliente.Data.Correo2);
            Values.Add("PARAM_ValorCredito", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[4].ToString()).ToString("N0"));
            Values.Add("PARAM_ValorCuota", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[3].ToString()).ToString("N0"));
            Values.Add("PARAM_Plazo", dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
            Values.Add("PARAM_me", mesesRedondeado.ToString());
            Values.Add("PARAM_tarifaunica", paramCartera.tarifaUnicaAvaluos.ToString());
            Values.Add("PARAM_tarifaestudiotitulos", paramCartera.tarifaEstudioTitulos.ToString());
            Values.Add("Param_atencion", puntoAtencion);
            Values.Add("PARAM_Cargo", listCategorias[0].Descripcion);
            Values.Add("PARAM_Entidad", listUnidades.FirstOrDefault()?.nombreUnidadEjecutora);
            Values.Add("PARAM_Direccion", responseInformacionAfiliadoHP.Data.FirstOrDefault().DRC_DIRECCION);
            Values.Add("PARAM_Ciudad", ciudad);
            Values.Add("PARAM_an", anos.ToString());
            Values.Add("PARAM_tarifaUnicaAvaluos", responseParametrizacionCarterra.Data.tarifaUnicaAvaluos.ToString("C0", new System.Globalization.CultureInfo("es-CO")));
            Values.Add("PARAM_tarifaTitulos", responseParametrizacionCarterra.Data.tarifaEstudioTitulos.ToString("C0", new System.Globalization.CultureInfo("es-CO")));


            string base64Documento = await CrearDocumentoBase64(
                                        baseHtml,
                                        baseCss,
                                        TipoDocumentoArchivo.PDF,
                                        Guid.NewGuid().ToString(),
                                        Values,
                                        identificacion.Substring(identificacion.Length - 4),
                                        headerHtml,
                                        footerHtml,
                                        true,
                                        10,
                                        10
                                    );

            var email = new Entities.Correo.Email
            {
                Modulo = "credito",

                RutaHtml = "TemplatePreaprobadoHipotecario.html",

                RutaImagenEncabezado = "encabezadoRechazoCredito.png",

                Asunto = "Comunicación oficial de Preaprobado de crédito hipotecario",

                Destinatarios = new List<string>
                {
                    "analisis.procesos@grupokriterion.com.co"
                },

                Parametros = new Dictionary<string, string>
                {
                    { "NombreCompleto", $"{dataCliente.Data.PrimerNombre} {dataCliente.Data.SegundoNombre} {dataCliente.Data.PrimerApellido} {dataCliente.Data.SegundoApellido}" }
                },
                AdjuntoBase64 = base64Documento,
                EnvioBase64 = true, 
                NombreDocumentoAdjunto = "Comunicación oficial de preaprobado"

            };

            await _emailNotificacionRepository.EnviarAsync(email);


            await CargarDocumentoProspeccionBase64(base64Documento, identificacion);


            byte[] bytes = Convert.FromBase64String(base64Documento);

            using var stream = new MemoryStream(bytes);

            IFormFile file = new FormFile(stream, 0, bytes.Length, "archivo", "documento.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var rutaImagen = await _creditoBusinessLogic.CargaDocumentoSolicCredito(file);


            #region Busqueda Documnetos PreAprobado
            DocumentosActSimulacionDto documentosActSimulacionDto = new DocumentosActSimulacionDto()
            {
                SLS_ID = informacionSimulacion.SLS_ID,
                AFL_ID = 1
            };
            var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(documentosActSimulacionDto);
            var Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Contains("Comunicación oficial de preaprobado")).Select(x => x.TDC_ID).FirstOrDefault();

            #endregion
            RequestDocumentoSimDto requestDocumentoSimDto = new RequestDocumentoSimDto()
            {
                idTipoDocumento = 86,
                codigoBarras = "",
                creadoPor = new Guid(),
                fechaCreacion = DateTime.Now,
                rutaImagen = rutaImagen.Data,
                idSimulacion = informacionSimulacion.SLS_ID
            };

            await this._creditoBusinessLogic.CrearDocumentoSimulacion(requestDocumentoSimDto);

            return await CrearDocumentos(baseHtml, baseCss, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), headerHtml, footerHtml, Values, identificacion.Substring(identificacion.Length - 4), true);
        }

        public async Task<Response<ResponseDocumentoSimDto>> CargarDocumentoProspeccionBase64(string base64Documento, string numeroDocumento)
        {
            try
            {
                // -------------------------------
                // Consultar simulación por documento
                // -------------------------------
                var informacionSimulacion =
                    await _creditoRepository.ObtenerSolicitudSimulacionCreditoNumeroDocumento(numeroDocumento);

                if (informacionSimulacion == null)
                {
                    return new Response<ResponseDocumentoSimDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = $"No existe simulación asociada al documento {numeroDocumento}"
                    };
                }
                // -------------------------------
                // Crear estructura AttachRequest
                // -------------------------------

                #region cargue 361
                var attachRequestDto = new Application.Dto.AttachRequestDto
                {
                    UserOperation = "web",
                    Files = new List<Application.Dto.FileDto>(),
                    Radicado = informacionSimulacion.SLS_NUMERO_RADICADO
                };

                // Extensión asumida PDF (puedes parametrizarla si lo necesitas)
                string extension = "pdf";

                // Obtener info tipo documento
                var infoTipoDocument =
                    ObtenerTipoDocumentoPorId("86").Result.Data;

                // -------------------------------
                // Crear el DTO para gestor documental
                // -------------------------------
                attachRequestDto.Files.Add(
                    CrearObjetoWorkmanager(base64Documento, infoTipoDocument, extension, false)
                );

                // Subir al gestor documental
                if (attachRequestDto.Files.Count > 0)
                {
                    await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);
                }
                #endregion


                #region cargue 83
                var attachRequestDto83 = new Application.Dto.AttachRequestDto
                {
                    UserOperation = "web",
                    Files = new List<Application.Dto.FileDto>(),
                    Radicado = informacionSimulacion.SLS_NUMERO_RADICADO_83
                };

                // Extensión asumida PDF (puedes parametrizarla si lo necesitas)
                extension = "pdf";

                // Obtener info tipo documento
                var infoTipoDocument83 =
                    ObtenerTipoDocumentoPorId("86").Result.Data;

                // -------------------------------
                // Crear el DTO para gestor documental
                // -------------------------------
                attachRequestDto83.Files.Add(
                    CrearObjetoWorkmanager(base64Documento, infoTipoDocument83, extension, true)
                );


                DocumentoCargadoDodo dod_dodo = new DocumentoCargadoDodo();

                dod_dodo.DCD_CARGADO = true;
                dod_dodo.DCD_DESCRIPCION = infoTipoDocument83.Descripcion;
                dod_dodo.RUTA = "";
                dod_dodo.SLS_ID = informacionSimulacion.SLS_ID;
                dod_dodo.TDC_ID = 83;



                // Subir al gestor documental
                DocumentoCargadoDodo documentoCargado = await _creditoRepository.ConsultaDocumentoCargadoDodo(informacionSimulacion.SLS_ID, infoTipoDocument83.Descripcion);

                if (documentoCargado == null || !dod_dodo.DCD_CARGUE_83)
                {
                    dod_dodo.DCD_CARGUE_83 = true;

                    await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto83);
                    await _creditoRepository.InsertarDocumentoCargadoDodo(dod_dodo);
                }

                #endregion

                // -------------------------------
                // Crear registro en la base de datos
                // -------------------------------
                var documento = new ResponseDocumentoSimulacion
                {
                    DSC_ID = Guid.NewGuid(),
                    SLS_ID = informacionSimulacion.SLS_ID,
                    TDC_ID = 153,
                    DSC_CODIGO_BARRAS = "codigo",
                    DSC_FECHA_DOCUMENTO = DateTime.Now,
                    DSC_NUMERO_FOLIOS = 0,
                    DSC_TAMAÑO = base64Documento.Length,
                    DSC_RUTA_IMAGEN = "",
                    DSC_ESTADO = "1",
                    DSC_CREADO_POR = Guid.NewGuid(),
                    DSC_FECHA_CREACION = DateTime.Now
                };

                // Guardar en BD
                //await _creditoRepository.InsertarDocumentoSimulacion(documento);

                return new Response<ResponseDocumentoSimDto>
                {
                    Data = null,
                    IsSuccess = true,
                    ReturnMessage = "Documento cargado correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<ResponseDocumentoSimDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "Error cargando documento: " + ex.Message
                };
            }
        }



        public async Task<Response<ResponseDocumentoSimDto>> CargarDocumentoProspeccionBase64(
    string base64Documento,
    string numeroDocumento,
    int idTipoDocumento,
    string codigoBarras,
    string creadoPor)
        {
            try
            {
                // -------------------------------
                // Consultar simulación por documento
                // -------------------------------
                var informacionSimulacion =
                    await _creditoRepository.ObtenerSolicitudSimulacionCreditoNumeroDocumento(numeroDocumento);

                if (informacionSimulacion == null)
                {
                    return new Response<ResponseDocumentoSimDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = $"No existe simulación asociada al documento {numeroDocumento}"
                    };
                }

                // -------------------------------
                // Crear estructura AttachRequest
                // -------------------------------
                var attachRequestDto = new Application.Dto.AttachRequestDto
                {
                    UserOperation = "web",
                    Files = new List<Application.Dto.FileDto>(),
                    Radicado = informacionSimulacion.SLS_NUMERO_RADICADO
                };

                // Extensión asumida PDF (puedes parametrizarla si lo necesitas)
                string extension = "pdf";

                // Obtener info tipo documento
                var infoTipoDocument = ObtenerTipoDocumentoPorId("153").Result.Data;

                // -------------------------------
                // Crear el DTO para gestor documental
                // -------------------------------
                attachRequestDto.Files.Add(
                    CrearObjetoWorkmanager(base64Documento, infoTipoDocument, extension)
                );

                // Subir al gestor documental
                if (attachRequestDto.Files.Count > 0)
                {
                    await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);
                }

                // -------------------------------
                // Crear registro en la base de datos
                // -------------------------------
                var documento = new ResponseDocumentoSimulacion
                {
                    DSC_ID = Guid.NewGuid(),
                    SLS_ID = informacionSimulacion.SLS_ID,
                    TDC_ID = idTipoDocumento,
                    DSC_CODIGO_BARRAS = codigoBarras,
                    DSC_FECHA_DOCUMENTO = DateTime.Now,
                    DSC_NUMERO_FOLIOS = 0,
                    DSC_TAMAÑO = base64Documento.Length,
                    DSC_RUTA_IMAGEN = "",
                    DSC_ESTADO = "1",
                    DSC_CREADO_POR = Guid.NewGuid(),
                    DSC_FECHA_CREACION = DateTime.Now
                };

                // Guardar en BD
                //await _creditoRepository.InsertarDocumentoSimulacion(documento);

                return new Response<ResponseDocumentoSimDto>
                {
                    Data = null,
                    IsSuccess = true,
                    ReturnMessage = "Documento cargado correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Response<ResponseDocumentoSimDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "Error cargando documento: " + ex.Message
                };
            }
        }

        private Application.Dto.FileDto CrearObjetoWorkmanager(
    string base64Archivo,
    TipoDocumentoDto infoDocumento,
    string extension, bool flujo83 = false)
        {
            if (string.IsNullOrWhiteSpace(base64Archivo))
                throw new ArgumentException("El archivo base64 no puede ser vacío.");

            if (infoDocumento == null)
                throw new ArgumentNullException(nameof(infoDocumento));

            if (string.IsNullOrWhiteSpace(extension))
                extension = "pdf"; // por si quieres default

            if (flujo83)
            {
                return new Application.Dto.FileDto(base64Archivo, infoDocumento.Descripcion, "00", "00", extension, "");

            }

            return new Application.Dto.FileDto(
                base64Archivo,
                infoDocumento.Descripcion ?? "",
                infoDocumento.DirectoryCode.ToString() ?? "",
                infoDocumento.DocumentTypeCode.ToString() ?? "",
                extension,
                string.Empty
            );
        }

        public async Task<Response<TipoDocumentoDto>> ObtenerTipoDocumentoPorId(string tipodocumentoid)
        {
            TipoDocumentoDto ResponseData = new TipoDocumentoDto();
            try
            {
                ResponseData = this._mapper.Map<TipoDocumentoDto>(
                    await this._creditoRepository.ObtenerTipoDocumentoPorId(tipodocumentoid));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ObtenerTipoDocumentoPorId");
            }

            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }

            return new Response<TipoDocumentoDto>
            {
                Data = ResponseData,
                ReturnMessage = "Documento consultado"
            };

        }



        /// <summary>
        /// PreaprobadoHipotecario
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        public async Task<FileResult> PreaprobadoEducativo(string identificacion)
        {
            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blogStorage.SRD_ESTADO)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
                BlobClient blobBase = container.GetBlobClient("TemplateEducativo.html");
                BlobClient blobCss = container.GetBlobClient("TemplateEducativo.css");

                BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

                var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

                await baseHtlm.FileStream.FlushAsync();
                await baseCss.FileStream.FlushAsync();

                // Consulto en BD la informacion
                var responseInformacionAfiliadoHP = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
                var dataCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(responseInformacionAfiliadoHP.Data.FirstOrDefault().TID_ID, identificacion);
                var responseInformacionSolicitudHP = _mapper.Map<List<PreaprobadoClienteDto>>(await _creditoRepository.SimulacionCliente(identificacion));
                var responseParametrizacionCarterra = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();

                if (!responseInformacionAfiliadoHP.Data.Any())
                {
                    throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {identificacion}");
                }

                if (!responseInformacionSolicitudHP.Any())
                {
                    throw new InvalidOperationException($"No existe información de la solicitud en la base de datos para la Afiliacion: {identificacion}");
                }



                DataTable dataTableInformacionAfiliadoHP = new DataTable();
                DataTable dataTableInformacionSolicitud = new DataTable();

                // Consulta a BD y serializacion del objeto en un DT
                //dataTableInformacionAfiliadoHP = ObtainDataTableFromIEnumerable(responseInformacionAfiliadoHP.Data);
                var dataInformacionAfiliado = responseInformacionAfiliadoHP.Data.ToList<InformacionClienteDto>();
                dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionSolicitudHP));

                var paramCartera = responseParametrizacionCarterra.Data;
                BlobContainerClient img = new BlobContainerClient(this._options.Value.CadenaOne, "imagenes-caja");
                BlobClient header = img.GetBlobClient("Header.html");
                BlobClient footer = img.GetBlobClient("Footer.html");

                BlobDownloadInfo downloadHeader = await header.DownloadAsync();
                BlobDownloadInfo downloadFooter = await footer.DownloadAsync();

                var baseHeaderhtml = new FileStreamResult(downloadHeader.Content, downloadHeader.ContentType);
                var baseFooterhtml = new FileStreamResult(downloadFooter.Content, downloadFooter.ContentType);

                await baseHeaderhtml.FileStream.FlushAsync();
                await baseFooterhtml.FileStream.FlushAsync();
                string Header = await (new StreamReader(baseHeaderhtml.FileStream)).ReadToEndAsync();
                string Footer = await (new StreamReader(baseFooterhtml.FileStream)).ReadToEndAsync();
                Dictionary<string, string> Values = new Dictionary<string, string>();
                string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
                string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();
                var ciudad = _mapper.Map<CiudadDto>(await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(int.Parse(dataInformacionAfiliado.FirstOrDefault().DPD_ID.ToString()), int.Parse(dataInformacionAfiliado.FirstOrDefault().DPC_ID.ToString()))).Nombre.ToString();
                List<CategoriaDto> listCategorias = _mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias()).Where(x => x.Id == int.Parse(dataCliente.Data.IdCategoria.ToString())).ToList();
                List<UnidadesEjecutorasDto> listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(await this._carteraRepository.ConsultarUnidadEjecutora()).Where(x => x.idUnidadEjecutora == int.Parse(dataCliente.Data.IdUnidadEjecutora.ToString())).ToList();
                string data = string.Empty;
                string modalidad = string.Empty;
                string puntoAtencion = string.Empty;
                foreach (var item in responseInformacionSolicitudHP)
                {
                    data = $"AL CONTESTAR CITE:{item.NUMERO_RADICADO}<br>Fecha:{item.FECHA_ACTUALIZA.ToString("dd/MM/yyyy")}<br>Área: {item.PUNTOATENCION}";
                    modalidad = "CREDITO EDUCATIVO";
                    puntoAtencion = item.PUNTOATENCION;
                }
                var totalMeses = int.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
                Double anos = totalMeses / 12.0;

                double valor = Convert.ToDouble(totalMeses);
                double mesesRedondeado = Math.Round(valor, 1);

                // Reemplazar valores en el HTML
                Values.Add("PARAM_modalidad", "CREDITO EDUCATIVO");
                Values.Add("PARAM_data", data);
                Values.Add("PARAM_Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                Values.Add("PARAM_Correoelectronico", dataInformacionAfiliado[0].DCE_CORREO);
                Values.Add("PARAM_Afiliado", dataInformacionAfiliado[0].CLI_NOMBRES.ToString().ToUpper());
                Values.Add("PARAM_Correo", dataInformacionAfiliado[0].DCE_CORREO.ToString());
                Values.Add("PARAM_ValorCredito", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[4].ToString()).ToString("N0").Replace("€", ""));
                Values.Add("PARAM_ValorCuota", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[3].ToString()).ToString("N0").Replace("€", ""));
                Values.Add("PARAM_Plazo", dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
                Values.Add("PARAM_me", mesesRedondeado.ToString());
                Values.Add("PARAM_tarifaunica", paramCartera.tarifaUnicaAvaluos.ToString());
                Values.Add("PARAM_tarifaestudiotitulos", paramCartera.tarifaEstudioTitulos.ToString());
                Values.Add("Param_atencion", puntoAtencion);
                Values.Add("PARAM_Cargo", listCategorias[0].Descripcion);
                Values.Add("PARAM_Entidad", listUnidades.FirstOrDefault().nombreUnidadEjecutora);
                Values.Add("PARAM_Direccion", dataInformacionAfiliado[0].DRC_DIRECCION);
                Values.Add("PARAM_Ciudad", ciudad);
                Values.Add("PARAM_an", anos.ToString());
                return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), Header, Footer, Values, identificacion.Substring(identificacion.Length - 4), true);
            }
            else
            {
                return await PreaprobadoEducativoLocal(identificacion);
            }

        }


        public async Task<FileResult> PreaprobadoEducativoLocal(string identificacion)
        {
            string basePath = _configureOptions.CarpetaServidorStorage + "\\reportes-sfc";
            string imgPath = _configureOptions.CarpetaServidorStorage + "\\imagenes-caja";

            string templateHtmlPath = Path.Combine(basePath, "TemplateEducativo.html");
            string templateCssPath = Path.Combine(basePath, "TemplateEducativo.css");
            string headerPath = Path.Combine(imgPath, "Header.html");
            string footerPath = Path.Combine(imgPath, "Footer.html");

            if (!File.Exists(templateHtmlPath) || !File.Exists(templateCssPath))
            {
                throw new FileNotFoundException("No se encontraron las plantillas necesarias en la carpeta 'reportes-sfc'.");
            }

            if (!File.Exists(headerPath) || !File.Exists(footerPath))
            {
                throw new FileNotFoundException("No se encontraron las plantillas 'Header.html' o 'Footer.html' en la carpeta 'imagenes-caja'.");
            }

            string baseHtml = await File.ReadAllTextAsync(templateHtmlPath);
            string baseCss = await File.ReadAllTextAsync(templateCssPath);
            string headerHtml = await File.ReadAllTextAsync(headerPath);
            string footerHtml = await File.ReadAllTextAsync(footerPath);

            var responseInformacionAfiliadoHP = await _iReportesRequestProvider.ReporteInformacionCliente(identificacion);
            if (!responseInformacionAfiliadoHP.Data.Any())
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la afiliación: {identificacion}");
            }

            var dataCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(
                responseInformacionAfiliadoHP.Data.FirstOrDefault().TID_ID, identificacion);

            var responseInformacionSolicitudHP = _mapper.Map<List<PreaprobadoClienteDto>>(await _creditoRepository.SimulacionCliente(identificacion));
            if (!responseInformacionSolicitudHP.Any())
            {
                throw new InvalidOperationException($"No existe información de la solicitud en la base de datos para la afiliación: {identificacion}");
            }

            var responseParametrizacionCarterra = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();
            var paramCartera = responseParametrizacionCarterra.Data;

            var ciudad = _mapper.Map<CiudadDto>(
                await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(
                    int.Parse(responseInformacionAfiliadoHP.Data.FirstOrDefault().DPD_ID.ToString()),
                    int.Parse(responseInformacionAfiliadoHP.Data.FirstOrDefault().DPC_ID.ToString())
                )
            ).Nombre.ToString();

            List<CategoriaDto> listCategorias = _mapper.Map<IEnumerable<CategoriaDto>>(
                await _catalogsRepository.ObtenerCategorias()
            ).Where(x => x.Id == int.Parse(dataCliente.Data.IdCategoria.ToString())).ToList();

            List<UnidadesEjecutorasDto> listUnidades = _mapper.Map<IEnumerable<UnidadesEjecutorasDto>>(
                await _carteraRepository.ConsultarUnidadEjecutora()
            ).Where(x => x.idUnidadEjecutora == int.Parse(dataCliente.Data.IdUnidadEjecutora.ToString())).ToList();

            DataTable dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(
                Newtonsoft.Json.JsonConvert.SerializeObject(responseInformacionSolicitudHP));

            var totalMeses = int.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
            double anos = totalMeses / 12.0;

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string data = string.Empty;
            string modalidad = string.Empty;
            string puntoAtencion = string.Empty;
            double valor = Convert.ToDouble(totalMeses);
            double mesesRedondeado = Math.Round(valor, 1);

            foreach (var item in responseInformacionSolicitudHP)
            {
                data = $"AL CONTESTAR CITE:{item.NUMERO_RADICADO}<br>Fecha:{item.FECHA_ACTUALIZA.ToString("dd/MM/yyyy")}<br>Área: {item.PUNTOATENCION}";
                modalidad = "CREDITO EDUCATIVO";
                puntoAtencion = item.PUNTOATENCION;
            }

            Values.Add("PARAM_modalidad", "CREDITO EDUCATIVO");
            Values.Add("PARAM_data", data);
            Values.Add("PARAM_Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
            Values.Add("PARAM_Correoelectronico", responseInformacionAfiliadoHP.Data.FirstOrDefault().DCE_CORREO);
            Values.Add("PARAM_Afiliado", responseInformacionAfiliadoHP.Data.FirstOrDefault().CLI_NOMBRES.ToUpper());
            Values.Add("PARAM_Correo", responseInformacionAfiliadoHP.Data.FirstOrDefault().DCE_CORREO);
            Values.Add("PARAM_ValorCredito", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[4].ToString()).ToString("N0"));
            Values.Add("PARAM_ValorCuota", double.Parse(dataTableInformacionSolicitud.Rows[0].ItemArray[3].ToString()).ToString("N0"));
            Values.Add("PARAM_Plazo", dataTableInformacionSolicitud.Rows[0].ItemArray[5].ToString());
            Values.Add("PARAM_me", mesesRedondeado.ToString());
            Values.Add("PARAM_tarifaunica", paramCartera.tarifaUnicaAvaluos.ToString());
            Values.Add("PARAM_tarifaestudiotitulos", paramCartera.tarifaEstudioTitulos.ToString());
            Values.Add("Param_atencion", puntoAtencion);
            Values.Add("PARAM_Cargo", listCategorias[0].Descripcion);
            Values.Add("PARAM_Entidad", listUnidades.FirstOrDefault()?.nombreUnidadEjecutora);
            Values.Add("PARAM_Direccion", responseInformacionAfiliadoHP.Data.FirstOrDefault().DRC_DIRECCION);
            Values.Add("PARAM_Ciudad", ciudad);
            Values.Add("PARAM_an", anos.ToString());

            return await CrearDocumentos(baseHtml, baseCss, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), headerHtml, footerHtml, Values, identificacion.Substring(identificacion.Length - 4), true);
        }



        /// <summary>
        /// AfiliacionExtemporaneaProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> AfiliacionExtemporaneaProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateAfiliacionExtemporaneaProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", datosAfiliacion.First().MesActivacionDescuento);

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// AfiliacionAdminCesantiasProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> AfiliacionAdminCesantiasProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateAfiliacionAdminCesantiasProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", datosAfiliacion.First().MesActivacionDescuento);

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// AfiliacionVoluntariaProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> AfiliacionVoluntariaProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateAfiliacionVoluntariaProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesActivacion", ObtenerFecha(DateTime.Parse(datosAfiliacion.First().MesActivacionDescuento)));

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// AfiliacionVoluntariaVeteranosProcedente Línea de Ahorro  por Consignación Bancaria
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> AfiliacionVoluntariaVeteranosProcedenteCB(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateAfiliacionVoluntariaVeteranosProcedenteCB.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", ObtenerFecha(DateTime.Parse(datosAfiliacion.First().MesActivacionDescuento)));

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// AfiliacionVoluntariaVeteranosCredito
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> AfiliacionVoluntariaVeteranosCredito(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateAfiliacionVoluntariaVeteranosCredito.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", ObtenerFecha(DateTime.Parse(datosAfiliacion.First().MesActivacionDescuento)));

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// AfiliacionVoluntariaVeteranosProcedenteCB Línea de Ahorro por Activación De Descuento
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> AfiliacionVoluntariaVeteranosProcedenteAD(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateAfiliacionVoluntariaVeteranosProcedenteAD.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", ObtenerFecha(DateTime.Parse(datosAfiliacion.First().MesActivacionDescuento)));

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// BeneficiariosProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> BeneficiariosProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateBeneficiariosProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", datosAfiliacion.First().MesActivacionDescuento);

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// Improcedentes
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> Improcedentes(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateImprocedentes.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesInicio", datosAfiliacion.First().MesActivacionDescuento);

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// NuevaVinculacionProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> NuevaVinculacionProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateNuevaVinculacionProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            var categorias = await _catalogsRepository.ObtenerCategorias();
            var fuerzas = await _catalogsRepository.ObtenerFuerzas();

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_categoria", categorias.FirstOrDefault(x => x.CTG_ID == responseAfiliacion.FirstOrDefault().Categoria).CTG_DESCRIPCION);
            Values.Add("PARAM_fuerza", fuerzas.FirstOrDefault(x => x.FRZ_ID == responseAfiliacion.FirstOrDefault().Fuerza).FRZ_DESCRIPCION);

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// PensionRetiroProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> PensionRetiroProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplatePensionRetiroProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// DesafiliacionVoluntariaProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> DesafiliacionVoluntariaProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateDesafiliacionVoluntariaProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                MesActivacionDescuento = s.MesActivacionDescuento,
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_MesActivacion", ObtenerFecha(DateTime.Parse(datosAfiliacion.First().MesActivacionDescuento)));

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// ReincorporacionProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReincorporacionProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateReincorporacionProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                FechaPrimeraCuota = s.FechaPrimeraCuota
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_primerAporte", ObtenerFecha(datosAfiliacion.First().FechaPrimeraCuota));

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// ReintegroProcedente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReintegroProcedente(ParametrosReportesAfiliacionesDto parametros)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "reportes-sfc");
            BlobClient blobBase = container.GetBlobClient("TemplateReintegroProcedente.html");
            BlobClient blobCss = container.GetBlobClient("TemplateAfiliaciones.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            // Consulto en BD la informacion
            IEnumerable<OficiosAfiliacionDto> responseAfiliacion = _iReportesRequestProvider.GenerarReportesAfiliacion(parametros).Result.Data;

            if (responseAfiliacion.Count() == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos para la Afiliacion: {parametros.IdAfiliacion}");
            }

            Dictionary<string, string> Values = new Dictionary<string, string>();
            string Basedocument = await (new StreamReader(baseHtlm.FileStream)).ReadToEndAsync();
            string Styles = await (new StreamReader(baseCss.FileStream)).ReadToEndAsync();

            var datosAfiliacion = responseAfiliacion.Select(s => new
            {
                NumeroRadicado = s.NumeroRadicado,
                RtaRadicado = s.NumeroRadicado,
                NombreCompleto = s.NombreCompleto,
                Correo = s.Correo,
                RatificacionCuotas = s.RatificacionCuotas
            });

            // Reemplazar valores en el HTML
            Values.Add("PARAM_Fecha", DateTime.Now.ToString());
            Values.Add("PARAM_Radicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_RtaRadicado", datosAfiliacion.First().NumeroRadicado);
            Values.Add("PARAM_Afiliado", datosAfiliacion.First().NombreCompleto);
            Values.Add("PARAM_Correo", datosAfiliacion.First().Correo);
            Values.Add("PARAM_NroAportes", datosAfiliacion.First().RatificacionCuotas.ToString());

            return await CrearDocumentos(Basedocument, Styles, TipoDocumentoArchivo.PDF, Guid.NewGuid().ToString(), "", "", Values, null);
        }

        /// <summary>
        /// ReporteCierreMensualCartera
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ReporteCierreMensualCarteraDto>>> ReporteCierreMensualCartera(ParamReporteCierreMensualCarteraDto request)
        {
            List<ReporteCierreMensualCarteraDto> list = new();
            var response = _mapper.Map<List<ReporteCierreMensualCarteraDto>>(await _reportesRepository.
                ReporteCierreMensualCartera(request));

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(response.
                Select(x => x.Identificacion).Distinct().ToList())).Data;

            list = (from r in response
                    join la in listAfiliados on r.Identificacion equals la.Documento
                    select new ReporteCierreMensualCarteraDto
                    {
                        NumeroCredito = r.NumeroCredito,
                        TipoIdentificacion = r.TipoIdentificacion,
                        Identificacion = r.Identificacion,
                        Nombres = la.NombreCompleto,
                        UnidadEjecutora = r.UnidadEjecutora,
                        Categoria = r.Categoria,
                        Grado = r.Grado,
                        FechaSolicitud = r.FechaSolicitud,
                        TipoCredito = r.TipoCredito,
                        EstadoCredito = r.EstadoCredito,
                        FechaDesembolso = r.FechaDesembolso,
                        FechaFinalizaPagos = r.FechaFinalizaPagos,
                        ValorDesembolso = r.ValorDesembolso,
                        ValorVivienda = r.ValorVivienda,
                        TipoVivienda = r.TipoVivienda,
                        ClasificacionVivienda = r.ClasificacionVivienda,
                        CanonInicial = r.CanonInicial,
                        ValorOpcionCompra = r.ValorOpcionCompra,
                        PorcentajeCanonInicial = r.PorcentajeCanonInicial,
                        PorcentajeOpcionCompra = r.PorcentajeOpcionCompra,
                        CanonExtraordinario = r.CanonExtraordinario,
                        PlazoTotal = r.PlazoTotal,
                        PlazoActual = r.PlazoActual,
                        CuotasCanceladas = r.CuotasCanceladas,
                        TasaSeguroVida = r.TasaSeguroVida,
                        TasaSeguroHogar = r.TasaSeguroHogar,
                        TasaCreditoEA = r.TasaCreditoEA,
                        TasaPeriodica = r.TasaPeriodica,
                        ValorCuota = r.ValorCuota,
                        SaldoCapital = r.SaldoCapital,
                        SaldoMora = r.SaldoMora,
                        DiasMora = r.DiasMora,
                        ValorDeudaRemanente = r.ValorDeudaRemanente,
                        FechaEstado = r.FechaEstado,
                        FechaUltimoPago = r.FechaUltimoPago,
                        FechaProximoPago = r.FechaProximoPago,
                        AcumuladoSeguroHogar = r.AcumuladoSeguroHogar,
                        FechaUltimoPagoSeguroHogar = r.FechaUltimoPagoSeguroHogar,
                        AcumuladoSeguroVida = r.AcumuladoSeguroVida,
                        FechaUltimoPagoSeguroVida = r.FechaUltimoPagoSeguroVida,
                        AcumuladoInteresesMora = r.AcumuladoInteresesMora,
                        FechaUltimoPagoInteresesMora = r.FechaUltimoPagoInteresesMora,
                        AcumuladoInteresesCorrientes = r.AcumuladoInteresesCorrientes,
                        FechaUltimoPagoInteresesCorrientes = r.FechaUltimoPagoInteresesCorrientes,
                        Calificacion = r.Calificacion,
                        Provision = r.Provision,
                        ProvisionPorcentaje = r.ProvisionPorcentaje
                    }).ToList();

            return new Response<IEnumerable<ReporteCierreMensualCarteraDto>> { Data = list };
        }

        public async Task<Response<IEnumerable<ReporteResultadoIntegracionDto>>> ReporteResultadoIntegracion(RequestReporteResultadoIntegracionDto request)
        {

            IEnumerable<ReporteResultadoIntegracionDto> reporte = _mapper.Map<IEnumerable<ReporteResultadoIntegracionDto>>(await _reportesRepository.ReporteResultadoIntegracion(request));

            List<ReporteResultadoIntegracionDto> ListreporteDto = new List<ReporteResultadoIntegracionDto>();

            foreach (ReporteResultadoIntegracionDto listreport in reporte)
            {
                Response<ClienteCedulaDto> clienteDto = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(listreport.Identificacion);

                if (clienteDto.Data != null)
                {
                    listreport.PNombre = clienteDto.Data.PrimerNombre;
                    listreport.SNombre = clienteDto.Data.SegundoNombre;
                    listreport.PApellidos = clienteDto.Data.PrimerApellido;
                    listreport.SApellidos = clienteDto.Data.SegundoApellido;

                    ListreporteDto.Add(listreport);
                }

            }

            return new Response<IEnumerable<ReporteResultadoIntegracionDto>> { Data = ListreporteDto };
        }

        /// <summary>
        /// Generacion ReporteSFCF414
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReportePlanoSFCF414(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF414 = _mapper.Map<List<ReporteSFCF414Dto>>(await _reportesRepository.ReporteSFCF414(parametrosAplicacionPago));

            if (responseF414.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            Task<byte[]> bufferT = ArmarPlano414(responseF414, parametrosAplicacionPago);
            byte[] buffer = await bufferT;

            MemoryStream stream = new MemoryStream();
            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;
        }
        public async Task<Response<List<ReporteSFCF414Dto>>> ReporteFCF414(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF414 = _mapper.Map<List<ReporteSFCF414Dto>>(await _reportesRepository.ReporteSFCF414(parametrosAplicacionPago));

            return new Response<List<ReporteSFCF414Dto>>
            {
                
                Data = responseF414
            };
        }

        public async Task<Response<List<ReporteSFCF453Y454>>> ReporteFCF453Y454(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            // Obtener los datos necesarios para el reporte
            var response453Y454 = _mapper.Map<List<ReporteSFCF453Y454>>(await _reportesRepository.ReporteSFCF453Y454(parametrosAplicacionPago));

            if (response453Y454.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            return new Response<List<ReporteSFCF453Y454>>
            {

                Data = response453Y454
            };


        }

        public async Task<Response<List<DocumentoPorPeriodoDto>>> ReporteFCF341(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF341 = _mapper.Map<List<DocumentoPorPeriodoDto>>(await _reportesRepository.AfiliadosPorPeriodo(parametrosAplicacionPago));

            if (responseF341.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            return new Response<List<DocumentoPorPeriodoDto>>
            {

                Data = responseF341
            };

        }


        public async Task<FileResult> ReportePlanoSFCF414_V2(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(parametrosAplicacionPago.FechaAplicacion, timeZoneInfo);
            //parametrosAplicacionPago.FechaAplicacion = dateTime;
            var responseF414 = _mapper.Map<List<ReporteSFCF414Dto>>(await _reportesRepository.ReporteSFCF414(parametrosAplicacionPago));

            if (responseF414.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            Task<byte[]> bufferT = ArmarPlano414_V2(responseF414, parametrosAplicacionPago);
            byte[] buffer = await bufferT;

            MemoryStream stream = new MemoryStream();
            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;
        }
        /// <summary>
        /// Generacion ReporteSFCF416
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReportePlanoSFCF416(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF416 = _mapper.Map<List<ReporteSFCF416Dto>>(await _reportesRepository.ReporteSFCF416(parametrosAplicacionPago));

            if (responseF416.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            Task<byte[]> bufferT = ArmarPlano416(responseF416, parametrosAplicacionPago);
            byte[] buffer = await bufferT;

            MemoryStream stream = new MemoryStream();
            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;

        }

        public async Task<FileResult> ReportePlanoSFCF477(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF477 = _mapper.Map<List<ReporteSFCF477Dto>>
            (
                await _reportesRepository.ReportePlanoSFCF477(parametrosAplicacionPago)
            );

            if (responseF477.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            Task<byte[]> bufferT = ArmarPlano477(responseF477, parametrosAplicacionPago);

            byte[] buffer = await bufferT;

            MemoryStream stream = new MemoryStream();

            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;
        }

        public async Task<FileResult> ReportePlanoSFCF536(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF536 = _mapper.Map<List<ReporteSFCF536>>
            (
                await _reportesRepository.ReportePlanoSFCF536(parametrosAplicacionPago)
            );

            if (responseF536.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            Task<byte[]> bufferT = ArmarPlano536(responseF536, parametrosAplicacionPago);

            byte[] buffer = await bufferT;

            MemoryStream stream = new MemoryStream();

            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;
        }


        public async Task<FileResult> ReportePlanoSFCF507(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF477 = _mapper.Map<List<ReporteSFCF477Dto>>
            (
                await _reportesRepository.ReportePlanoSFCF507(parametrosAplicacionPago)
            );

            if (responseF477.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            Task<byte[]> bufferT = ArmarPlano477(responseF477, parametrosAplicacionPago);

            byte[] buffer = await bufferT;

            MemoryStream stream = new MemoryStream();

            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;
        }


        public async Task<Response<List<Resumen477Dto>>> ReporteFCF477(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF477 = _mapper.Map<List<Resumen477Dto>>
            (
                await _reportesRepository.ReporteSFCF477(parametrosAplicacionPago)
            );


            return new Response<List<Resumen477Dto>>
            {
                Data = responseF477
            };
        }

        public async Task<Response<List<Resumen536Dto>>> ReporteSFC536(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF477 = _mapper.Map<List<Resumen536Dto>>
            (
                await _reportesRepository.ReporteSFC536(parametrosAplicacionPago)
            );


            return new Response<List<Resumen536Dto>>
            {
                Data = responseF477
            };
        }


        private List<string> ObtenerPropiedades(Type tipo)
        {
            var ColumnData = new List<string>();
            PropertyInfo[] columns = tipo.GetProperties();
            foreach (var item in columns)
            {
                ColumnData.Add(item.Name);
            }
            return ColumnData;
        }
        public async Task<Response<object>> GenerarReporteFondoSolidaridad(ReportFondoSolidaridadRequetsDto parametersDto)
        {
            if (parametersDto.ReportModel != null)
            {
                var dataResponse = await _postulacionBussineLogic.ObtenerDataReportModelFondoSolidaridad(parametersDto.ReportModel);

                if (dataResponse is not null && dataResponse?.Count > 0)
                {
                    return new Response<object> { Data = dataResponse, IsSuccess = true, ReturnMessage = "Informacion consultada exitosamente" };
                }
                return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "No Existe informacion con esos filtros." };
            }
            else
            {
                var response = await _postulacionBussineLogic.ObtenerDataReporlFondoSolidaridadPorEstadoTramite(parametersDto.ReportStatus);
                if (response is not null && response?.Count > 0)
                {
                    return new Response<object> { Data = response, IsSuccess = true, ReturnMessage = "Informacion consultada exitosamente" };
                }
                return new Response<object> { Data = null, IsSuccess = false, ReturnMessage = "No Existe informacion con esos filtros." };
            }
        }
        /// <summary>
        /// Generacion ReporteSFCF414
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReporteSFCF341(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            // Ruta donde esta ubicado el reporte
            var path = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteSFCF41.rdlc";
            string mimtype = string.Empty;
            int extension = 1;

            // Inicio parametros para reporte local
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var responseF341 = _mapper.Map<List<ReporteSFCF341Dto>>(await _reportesRepository.ReporteSFCF341(parametrosAplicacionPago));

            if (responseF341.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            LocalReport localReport = new LocalReport(path);

            // Consulta a BD y serializacion del objeto en un DT
            DataTable dataTable = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(responseF341));

            // Asigno al DS el DT para pintar los datos
            localReport.AddDataSource("InformacionSFC341", dataTable);

            // Convierto el reporte en un arreglo de bytes
            byte[] byteArray = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype).MainStream;

            FileResult file = await GenerarReporte(byteArray, false);

            return file;

        }

        /// <summary>
        /// Generacion ReporteSFCF416
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public async Task<FileResult> ReportePlanoSFCF341(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseF341 = _mapper.Map<List<DocumentoPorPeriodoDto>>(await _reportesRepository.AfiliadosPorPeriodo(parametrosAplicacionPago));

            if (responseF341.Count == 0)
            {
                throw new InvalidOperationException($"No existe información en la base de datos con el filtro de fechas ingresado.");
            }

            byte[] buffer = await ArmarPlano341(responseF341, parametrosAplicacionPago);

            MemoryStream stream = new MemoryStream();
            stream.Write(buffer, 0, buffer.Length);

            FileResult file = await GenerarReporte(stream, ".txt");

            return file;

        }


        #region Funciones 

        /// <summary>
        /// Funcion que convierte un IEnum en DT para pasar parametros al reporte 
        /// </summary>
        /// <param name="ien"></param>
        /// <returns></returns>
        private DataTable ObtainDataTableFromIEnumerable(IEnumerable ien)
        {
            DataTable dt = new DataTable();

            foreach (object obj in ien)
            {
                Type t = obj.GetType();
                PropertyInfo[] pis = t.GetProperties();
                if (dt.Columns.Count == 0)
                {
                    foreach (PropertyInfo pi in pis)
                    {
                        dt.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pi in pis)
                {
                    object value = pi.GetValue(obj, null);
                    dr[pi.Name] = value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Funcion que genera los reportes de PDF en el storage
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private async Task<FileResult> GenerarReporte(byte[] byteArray, bool requiredPass, string identificacion = "", bool guardarRegistro = false, bool guardarCartaRechazo = false)
        {
            var blobStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

            if (blobStorage.SRD_ESTADO)
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
                BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "reportes-sfc");
                await container.CreateIfNotExistsAsync();

                // Conexion al storage
                var cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.CadenaOne);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("reportes-sfc");
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(namePDF);
                cloudBlockBlob.Properties.ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf;

                // Subo el archivo al container
                using (Stream stream = new MemoryStream(byteArray))
                {
                    await cloudBlockBlob.UploadFromStreamAsync(stream);
                }

                var fileName = cloudBlockBlob.Uri.ToString().Split("/");

                // Se asigna el nombre con el que quedo guardado el archivo en el storage
                MemoryStream msPdf = new(byteArray);
                var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
                {
                    FileDownloadName = fileName[4]
                };

                await archivoPdf.FileStream.FlushAsync();

                return archivoPdf;
            }
            else
            {
                return await GenerarReporteLocal(byteArray, requiredPass, identificacion, guardarRegistro, guardarCartaRechazo);
            }

        }

        private async Task<FileResult> GenerarReporteLocal(byte[] byteArray, bool requiredPass, string identificacion = "", bool guardarRegistro = false, bool guardarCartaRechazo = false)
        {

            //  Se asigna valor secreto de accesibilidad al PDF si el parámetro es true
            if (requiredPass)
            {
                using MemoryStream input = new(byteArray);
                using MemoryStream output = new();
                string credential = identificacion;
                PdfReader reader = new PdfReader(input);
                PdfEncryptor.Encrypt(reader, output, true, credential, credential, PdfWriter.ALLOW_SCREENREADERS);
                byteArray = output.ToArray();
            }


            // Se asigna nombre aleatorio al archivo
            string namePDF = Guid.NewGuid() + ".pdf";

            // Definir la carpeta local donde se guardarán los archivos
            string localFolderPath = _configureOptions.CarpetaServidorStorage + "\\reportes-sfc";

            // Crear la carpeta si no existe
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }

            // Ruta completa del archivo
            string filePath = Path.Combine(localFolderPath, namePDF);

            // Guardar el archivo en la carpeta local
            await File.WriteAllBytesAsync(filePath, byteArray);

            // Leer el archivo para retornarlo como FileStreamResult
            MemoryStream msPdf = new(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = namePDF
            };

            await archivoPdf.FileStream.FlushAsync();

            if (guardarRegistro)
            {
                DocumentosAdicionalesCargados documentosAdicionales = new DocumentosAdicionalesCargados();
                documentosAdicionales.DSA_DOCUMENTO = "Reclasificación";
                documentosAdicionales.DSA_RUTA = namePDF;
                documentosAdicionales.DSA_CARPETA = "reportes-sfc";
                documentosAdicionales.DSA_MODULO = "credito";
                documentosAdicionales.DSA_CARGADO_DODO = false;
                documentosAdicionales.DSA_NUMERO_DOCUMENTO = identificacion;
                await _reportesRepository.GuardarDocumentoAdicionalCargado(documentosAdicionales);
            }
            if (guardarCartaRechazo)
            {
                DocumentosAdicionalesCargados documentosAdicionales = new DocumentosAdicionalesCargados();
                documentosAdicionales.DSA_DOCUMENTO = "Comunicado oficial - rechazo";
                documentosAdicionales.DSA_RUTA = namePDF;
                documentosAdicionales.DSA_CARPETA = "reportes-sfc";
                documentosAdicionales.DSA_MODULO = "credito";
                documentosAdicionales.DSA_CARGADO_DODO = false;
                documentosAdicionales.DSA_NUMERO_DOCUMENTO = identificacion;
                await _reportesRepository.GuardarDocumentoAdicionalCargado(documentosAdicionales);
            }


            return archivoPdf;
        }

        private async Task<FileResult> GenerarArchivoPDF(byte[] byteArray, string filename)
        {
            var msPdf = new MemoryStream(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = filename
            };

            await archivoPdf.FileStream.FlushAsync();
            return archivoPdf;
        }

        static byte[] ProtectPdf(byte[] pdfBytes, string password)
        {
            using var inputStream = new MemoryStream(pdfBytes);
            using var document = PdfSharpIO.PdfReader.Open(inputStream, PdfSharpIO.PdfDocumentOpenMode.Modify);

            var securitySettings = document.SecuritySettings;
            securitySettings.OwnerPassword = password;
            securitySettings.UserPassword = password;
            securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted128Bit;

            using var outputStream = new MemoryStream();
            document.Save(outputStream);
            return outputStream.ToArray();
        }

        //private async Task<MemoryStream> GenerarReportesLocales(byte[] byteArray, bool requiredPass, string identificacion = "")
        //{
        //    // Asigno valor secreto de accesibilidad al pdf si el parametro es true
        //    if (requiredPass)
        //    {
        //        using MemoryStream input = new MemoryStream(byteArray);
        //        using MemoryStream output = new MemoryStream();
        //        string credential = identificacion;
        //        PdfReader reader = new PdfReader(input);
        //        PdfEncryptor.Encrypt(reader, output, true, credential, credential, PdfWriter.ALLOW_SCREENREADERS);
        //        byteArray = output.ToArray();
        //    }

        //    // Devuelvo los datos del archivo PDF en un MemoryStream
        //    MemoryStream msPdf = new MemoryStream(byteArray);
        //    return msPdf;
        //}
        /// <summary>
        /// GenerarReporte
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<FileResult> GenerarReporte(MemoryStream stream, string extension)
        {
            var blobStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

            if (blobStorage.SRD_ESTADO)
            {
                // Asigno nombre aleatorio al archivo 
                string hash = Guid.NewGuid().ToString();
                string filename = hash + extension;

                // Valido que si el container no existe se crea
                BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "reportes-sfc");
                await container.CreateIfNotExistsAsync();

                // Conexion al storage
                var cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.CadenaOne);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("reportes-sfc");
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
                cloudBlockBlob.Properties.ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf;

                var streamTwo = new MemoryStream();
                streamTwo.CopyTo(stream);
                streamTwo.CopyTo(stream);

                // Subo el archivo al container
                await cloudBlockBlob.UploadFromStreamAsync(streamTwo);
                byte[] bytePdf = stream.ToArray();

                // Obtengo el nombre del archivo
                var fileName = cloudBlockBlob.Uri.ToString().Split("/");

                if (extension == ".pdf")
                {
                    // Se asigna el nombre con el que quedo guardado el archivo en el storage
                    MemoryStream msPdf = new(bytePdf);
                    var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
                    {
                        FileDownloadName = fileName[4]
                    };

                    await archivoPdf.FileStream.FlushAsync();

                    return archivoPdf;
                }

                if (extension == ".txt")
                {
                    // Se asigna el nombre con el que quedo guardado el archivo en el storage
                    MemoryStream msPdf = new(bytePdf);
                    var archivoPdf = new FileStreamResult(msPdf, "text/plain")
                    {
                        FileDownloadName = fileName[4]
                    };

                    await archivoPdf.FileStream.FlushAsync();

                    return archivoPdf;
                }

                return null;
            }
            else
            {
                return await GenerarReporteLocal(stream, extension);
            }


        }

        private async Task<FileResult> GenerarReporteLocal(MemoryStream stream, string extension)
        {
            // Genera un nombre aleatorio para el archivo
            string hash = Guid.NewGuid().ToString();
            string filename = hash + extension;

            // Define la ruta local para guardar el archivo
            string localPath = _configureOptions.CarpetaServidorStorage + "\\reportes-sfc";

            // Verifica si la carpeta existe, si no, la crea
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            // Ruta completa del archivo
            string filePath = Path.Combine(localPath, filename);

            // Guarda el archivo en local
            await File.WriteAllBytesAsync(filePath, stream.ToArray());

            // Devuelve el archivo como FileResult
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            MemoryStream fileStream = new MemoryStream(fileBytes);

            string contentType = extension switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                _ => "application/octet-stream" // Tipo genérico para otros archivos
            };

            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = filename
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<FileResult> GenerarDocumentoPDF(DataTable data, string TipoReporte)
        {
            string pathEncabezado = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\HTML\\images\\ENCABEZADO2022.png";
            string pathPiePagina = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\HTML\\images\\PIEDEPAGINA2022.png";

            string[] imagenes =
            {
                pathEncabezado,
                pathPiePagina
            };

            MemoryStream stream = new MemoryStream();
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            TemplateReportes template = new TemplateReportes();

            switch (TipoReporte)
            {
                case "ReporteTransferenciaCesantias":
                    stream = template.ReporteTransferenciaCesantias(data, imagenes);
                    break;
                case "ReporteTransferenciaCesantiasPONAL":
                    stream = template.ReporteTransferenciaCesantiasPONAL(data, imagenes);
                    break;
                case "ReporteRespuestaNoProcedente":
                    stream = template.ReporteRespuestaNoProcedente(data, imagenes);
                    break;
                case "ReporteRespuestaEmbargo":
                    stream = template.ReporteRespuestaEmbargo(data, imagenes);
                    break;
                case "ReporteRespuestaSolicitud":
                    stream = template.ReporteRespuestaSolicitud(data, imagenes);
                    break;
                case "CartaInconsistencia":
                    stream = template.ObtenerCartaInconsistencia(data, imagenes);
                    break;
                case "ReporteCertificadoHaberes":
                    stream = template.ReporteCertificadoHaberes(data, imagenes);
                    break;
                case "ObtenerRespuestaSolicitudDePago":
                    stream = template.ObtenerRespuestaSolicitudDePago(data, imagenes);
                    break;
                default:
                    break;
            }

            FileResult file = await GenerarReporte(stream, ".pdf");

            return file;


        }

        /// <summary>
        /// ArmarPlano414
        /// </summary>
        /// <param name="responseF414"></param>
        /// <param name="parametrosAplicacionPago"></param>
        /// <returns></returns>
        private async Task<byte[]> ArmarPlano414(List<ReporteSFCF414Dto> responseF414, ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            StringBuilder constructorEncabezado = new StringBuilder();
            StringBuilder constructor = new StringBuilder();
            int consecutivo = 1;
            string value = string.Empty;

            // Columnas requeridas en el reporte
            var columnas414 = ColumnasRequeridas414();

            RegistroTipo2_414Dto informacion = new RegistroTipo2_414Dto
            {
                NumeroRegistro = (consecutivo + 1).ToString("D5"),
                TipoRegistro = 2,
                CodigoOficina = "0000",
                Constante = 0,
                TipoMoneda = 1,
                TipoInformacion = 1,
                TipoFideicomiso = 0,
                CodigoFideicomiso = "0000"
            };

            consecutivo++;

            var binformacion = informacion.NumeroRegistro + "" +
                informacion.TipoRegistro + "" +
                informacion.CodigoOficina + "" +
                informacion.Constante + "" +
                informacion.TipoMoneda + "" +
                informacion.TipoInformacion + "" +
                informacion.TipoFideicomiso + "" +
                informacion.CodigoFideicomiso;
            constructor.AppendLine(binformacion);

            // Con los documentos de GA2 consulto la informacion de BUA
            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responseF414.
                Select(x => x.CLI_IDENTIFICACION).Distinct().ToList())).Data;

            // Recorro cada registro para actualizar el sexo del afiliado
            try
            {
                for (int i = 0; i < responseF414.Count; i++)
                {
                    var sexo = listAfiliados.SingleOrDefault(s => s.Documento == responseF414[i].CLI_IDENTIFICACION)?.Sexo;
                    responseF414[i].SEXO = sexo == "M" ? "2" : "1";
                }
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }


            // Con toda la informacion lista, agrupo la lista en una nueva lista que usare para pintar los datos
            var listaGroup = AgrupacionDatos414(responseF414);


            foreach (var item in columnas414)
            {
                for (int i = 0; i < listaGroup.Count; i++)
                {

                    // Recorro columna por columna para hallar su valor en la lista
                    switch (item.Key)
                    {
                        case "TIPO_PERSONA":
                            value = listaGroup[i].TipoPersona;
                            break;

                        case "SEXO":
                            value = listaGroup[i].Sexo;
                            break;

                        case "TAMANO_EMPRESA":
                            value = listaGroup[i].TamanioEmpresa;
                            break;

                        case "TIPO_CREDITO":
                            value = listaGroup[i].TipoCredito;
                            break;

                        case "TIPO_GARANTIA":
                            value = listaGroup[i].TipoGarantia;
                            break;

                        case "PRODUCTO_CREDITO":
                            value = listaGroup[i].ProductoCredito;
                            break;

                        case "PLAZO_CREDITO":
                            value = listaGroup[i].PlazoCredito.ToString();
                            break;

                        case "TASA_EFECTIVA_PROMEDIO_PONDERADA":
                            value = listaGroup[i].TasaEfectivaPromedio.ToString();
                            break;

                        case "MONTOS_DESEMBOLSADOS":
                            value = listaGroup[i].MontoSolicitudes.ToString();
                            break;

                        case "NUMERO_CREDITOS_DESEMBOLSADOS":
                            value = /*listaGroup[i].NumeroSolicitudes.ToString()*/ "1";
                            break;
                    }

                    RegistroTipo4_414Dto detalle = new RegistroTipo4_414Dto()
                    {
                        NumeroSecuencia = (consecutivo + 1).ToString("D5"),
                        TipoRegistro = 4,
                        CodigoFormato = "414",
                        CodigoColumna = item.Value,
                        CodigoUnidadCaptura = "01",
                        CodigoSubCuenta = (i + 1).ToString("D3"),
                        Signo = "+",
                        Valor = value.PadLeft(20, '0').Replace(',', '.')
                    };

                    consecutivo++;

                    var data = detalle.NumeroSecuencia + "" +
                    detalle.TipoRegistro + "" +
                    detalle.CodigoFormato + "" +
                    detalle.CodigoColumna + "" +
                    detalle.CodigoUnidadCaptura + "" +
                    detalle.CodigoSubCuenta + "" +
                    detalle.Signo + "" +
                    detalle.Valor;

                    constructor.AppendLine(data);
                }
            }

            consecutivo++;

            RegistroTipo5_414Dto fin = new RegistroTipo5_414Dto
            {
                NumeroSecuencia = consecutivo.ToString("D5"),
                TipoRegistro = 5
            };

            RegistroTipo1_414Dto encabezado = new RegistroTipo1_414Dto
            {
                CodigoEntidad = "000011",
                Entidad = "IOECAJAPROM",
                FechaCorte = parametrosAplicacionPago.FechaAplicacion.ToString("ddMMyyyy"),
                NumeroSecuencia = 1.ToString("D5"),
                TipoEntidad = 22,
                TipoInforme = "08",
                TipoRegistro = 1,
                TotalRegistros = consecutivo.ToString("D5"),
                AreaInformacion = "01"
            };


            // Fecha proporcionada
            DateTime fechaCorte = parametrosAplicacionPago.FechaAplicacion;


            encabezado.FechaCorte = fechaCorte.ToString("ddMMyyyy");

            var aencabezado = encabezado.NumeroSecuencia + "" +
            encabezado.TipoRegistro + "" +
            encabezado.TipoEntidad + "" +
            encabezado.CodigoEntidad + "" +
            encabezado.FechaCorte + "" +
            encabezado.TotalRegistros + "" +
            encabezado.Entidad + "" +
            encabezado.AreaInformacion + "" +
            encabezado.TipoInforme;
            constructorEncabezado.AppendLine(aencabezado);


            var dfin = (fin.NumeroSecuencia + fin.TipoRegistro.ToString()).TrimEnd();
            constructor.AppendLine(dfin.TrimEnd());
            constructorEncabezado.AppendLine(constructor.ToString());

            byte[] buffer = Encoding.ASCII.GetBytes(constructorEncabezado.ToString());

            return buffer;
        }
        private async Task<byte[]> ArmarPlano414_V2(List<ReporteSFCF414Dto> responseF414, ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            var responseParamGenerales = this._mapper.Map<ParamGeneralesCreditoYCarteraDto>(await this._carteraRepository.ObtenerParamGeneralesCreditoYCartera());
            var Ciudades = (await _catalogsBusinessLogic.ObtenerCiudades()).Data;
            StringBuilder constructorEncabezado = new StringBuilder();
            StringBuilder constructor = new StringBuilder();
            int consecutivo = 1;
            string value = string.Empty;

            // Columnas requeridas en el reporte
            var columnas414 = ColumnasRequeridas414_V2();

            RegistroTipo2_414Dto informacion = new RegistroTipo2_414Dto
            {
                NumeroRegistro = (consecutivo + 1).ToString("D8"),
                TipoRegistro = 3,
                CodigoOficina = "0000",
                Constante = 0,
                TipoMoneda = 1,
                TipoInformacion = 1,
                TipoFideicomiso = 0,
                CodigoFideicomiso = "0000000000000000000000000000"
            };

            consecutivo++;

            var binformacion = informacion.NumeroRegistro + "" +
                informacion.TipoRegistro + "" +
                informacion.CodigoOficina + "" +
                informacion.Constante + "" +
                //informacion.TipoMoneda + "" +
                //informacion.TipoInformacion + "" +
                informacion.TipoFideicomiso + "" +
                informacion.CodigoFideicomiso;
            constructor.AppendLine(binformacion);

            //Linea 3 Solicitada
            informacion = new RegistroTipo2_414Dto
            {
                NumeroRegistro = (consecutivo + 1).ToString("D8"),
                TipoRegistro = 4,
                CodigoOficina = "0000",
                Constante = 0,
                TipoMoneda = 1,
                TipoInformacion = 1,
                TipoFideicomiso = 0,
                CodigoFideicomiso = "0000000000000002"
            };
            consecutivo++;
            binformacion = informacion.NumeroRegistro + "" +
                informacion.TipoRegistro + "" +
                informacion.CodigoOficina + "" +
                informacion.Constante + "" +
                informacion.TipoFideicomiso + "" +
                informacion.CodigoFideicomiso;
            constructor.AppendLine(binformacion);
            //string linea = "000000034      0000   0   0   0000000000000002";
            //constructor.AppendLine(linea);

            // Con los documentos de GA2 consulto la informacion de BUA
            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responseF414.
                Select(x => x.CLI_IDENTIFICACION).Distinct().ToList())).Data;

            // Recorro cada registro para actualizar el sexo del afiliado
            try
            {
                for (int i = 0; i < responseF414.Count; i++)
                {
                    var sexo = listAfiliados.SingleOrDefault(s => s.Documento == responseF414[i].CLI_IDENTIFICACION)?.Sexo;
                    var depto = Ciudades.Where(x => x.Id == responseF414[i].CRE_CIUDAD).FirstOrDefault().IdDepartamento;
                    var ciudad = Ciudades.Where(x => x.Id == responseF414[i].CRE_CIUDAD).FirstOrDefault().Codigo;
                    responseF414[i].SEXO = sexo == "M" ? "2" : "1";
                    responseF414[i].DEPARTAMENTO = depto < 10 ? $"0{depto}" : depto.ToString();
                    responseF414[i].CIUDAD = ciudad < 10 ? $"00{ciudad}" : (ciudad < 100 ? $"0{ciudad}" : ciudad.ToString());
                }
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }

            foreach (var item in responseF414)
            {
                decimal CantSM = item.MONTOS_DESEMBOLSADOS / responseParamGenerales.parametroSalarioMinimo;
                if (CantSM <= 135)
                {
                    item.RMD = "401";
                }
                else if (CantSM > 135 && CantSM <= 235)
                {
                    item.RMD = "402";
                }
                else if (CantSM > 235 && CantSM <= 335)
                {
                    item.RMD = "403";
                }
                else if (CantSM > 335 && CantSM <= 500)
                {
                    item.RMD = "404";
                }
                else
                {
                    item.RMD = "405";
                }
            }
            // Con toda la informacion lista, agrupo la lista en una nueva lista que usare para pintar los datos
            var listaGroup = AgrupacionDatos414(responseF414);

            foreach (var item in columnas414)
            {
                for (int i = 0; i < listaGroup.Count; i++)
                {

                    // Recorro columna por columna para hallar su valor en la lista
                    switch (item.Key)
                    {
                        case "TIPO_PERSONA":
                            value = listaGroup[i].TipoPersona;
                            break;

                        case "SEXO":
                            value = listaGroup[i].Sexo;
                            break;

                        case "TAMANO_EMPRESA":
                            value = listaGroup[i].TamanioEmpresa;
                            break;

                        case "TIPO_CREDITO":
                            value = listaGroup[i].TipoCredito;
                            break;

                        case "TIPO_GARANTIA":
                            value = listaGroup[i].TipoGarantia;
                            break;

                        case "PRODUCTO_CREDITO":
                            value = listaGroup[i].ProductoCredito;
                            break;

                        case "PLAZO_CREDITO":
                            value = listaGroup[i].PlazoCredito.ToString();
                            break;

                        case "TASA_EFECTIVA_PROMEDIO_PONDERADA":
                            value = listaGroup[i].TasaEfectivaPromedio.ToString();
                            break;

                        case "MONTOS_DESEMBOLSADOS":
                            value = listaGroup[i].MontoSolicitudes.ToString();
                            break;

                        case "MARGEN":
                            value = "0";
                            break;

                        case "NUMERO_CREDITOS_DESEMBOLSADOS":
                            value = listaGroup[i].NumeroSolicitudes.ToString();
                            break;
                        //NUEVAS COLUMNAS 29/09/2023

                        case "GRUPO_ETNICO":
                            value = "7";
                            break;

                        case "ANTIGUEDAD_EMPRESA":
                            value = "10";
                            break;
                        case "TIPO_TASA":
                            value = "FS";
                            break;
                        case "RANGO_MONTO_DESEMBOLSADO":
                            value = listaGroup[i].RANGO_MONTO_DESEMBOLSADO;
                            break;
                        case "CLASE_DEUDOR":
                            value = "1";
                            break;
                        case "CODIGO_CIIU":
                            value = "10";
                            break;
                        case "CODIGO_MUNICIPIO":
                            value = listaGroup[i].Municipio;
                            break;
                    }

                    RegistroTipo4_414Dto detalle = new RegistroTipo4_414Dto()
                    {
                        NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                        TipoRegistro = 5,
                        CodigoFormato = "414",
                        CodigoColumna = item.Value,
                        CodigoUnidadCaptura = "01",
                        CodigoSubCuenta = (i + 1).ToString("D6"),
                        Signo = "+",
                        Valor = item.Key == "TIPO_TASA" ? value : value.PadLeft(17, '0').Replace(',', '.')
                    };

                    consecutivo++;

                    var data = detalle.NumeroSecuencia + "" +
                    detalle.TipoRegistro + "" +
                    detalle.CodigoFormato + "" +
                    detalle.CodigoColumna + "" +
                    detalle.CodigoUnidadCaptura + "" +
                    detalle.CodigoSubCuenta + "" +
                    detalle.Signo + "" +
                    detalle.Valor;

                    constructor.AppendLine(data);
                }
            }

            consecutivo++;

            RegistroTipo5_414Dto fin = new RegistroTipo5_414Dto
            {
                NumeroSecuencia = consecutivo.ToString("D8"),
                TipoRegistro = 6
            };

            RegistroTipo1_414Dto encabezado = new RegistroTipo1_414Dto
            {
                CodigoEntidad = "000011",
                Entidad = "IOECAJAPROM",
                FechaCorte = parametrosAplicacionPago.FechaAplicacion.ToString("ddMMyyyy"),
                NumeroSecuencia = 1.ToString("D8"),
                TipoEntidad = 22,
                TipoInforme = "47",
                TipoRegistro = 1,
                TotalRegistros = consecutivo.ToString("D8"),
                AreaInformacion = "09"
            };
            // Fecha proporcionada
            //DateTime fechaCorte = parametrosAplicacionPago.FechaAplicacion.AddDays(-3);
            //encabezado.FechaCorte = fechaCorte.ToString("ddMMyyyy");

            var aencabezado = encabezado.NumeroSecuencia + "" +
            encabezado.TipoRegistro + "" +
            encabezado.TipoEntidad + "" +
            encabezado.CodigoEntidad + "" +
            encabezado.FechaCorte + "" +
            encabezado.TotalRegistros + "" +
            encabezado.Entidad + "" +
            encabezado.AreaInformacion + "" +
            encabezado.TipoInforme;
            constructorEncabezado.AppendLine(aencabezado);


            var dfin = (fin.NumeroSecuencia + fin.TipoRegistro.ToString()).TrimEnd();
            constructor.AppendLine(dfin.TrimEnd());
            constructorEncabezado.AppendLine(constructor.ToString().TrimEnd());

            byte[] buffer = Encoding.ASCII.GetBytes(constructorEncabezado.ToString().TrimEnd());

            return buffer;
        }
        /// <summary>
        /// ArmarPlano416
        /// </summary>
        /// <param name="responseF414"></param>
        /// <param name="parametrosAplicacionPago"></param>
        /// <returns></returns>
        private async Task<byte[]> ArmarPlano416(List<ReporteSFCF416Dto> responseF416, ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            StringBuilder constructorEncabezado = new StringBuilder();
            StringBuilder constructor = new StringBuilder();
            int consecutivo = 1;
            string value = string.Empty;

            // Columnas requeridas en el reporte
            var columnas416 = ColumnasRequeridas416();

            // Con los documentos de GA2 consulto la informacion de BUA
            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responseF416.
                Select(x => x.CLI_IDENTIFICACION).Distinct().ToList())).Data;

            // Recorro cada registro para actualizar el departamento del afiliado
            for (int i = 0; i < responseF416.Count; i++)
            {
                if (responseF416[i].CODIGO_DEPARTAMENTO == "DPT")
                {
                    var dpto = listAfiliados.Single(s => s.Documento == responseF416[i].CLI_IDENTIFICACION && responseF416[i].CODIGO_DEPARTAMENTO == "DPT").Departamento.ToString();
                    if (dpto == null)
                    {
                        responseF416[i].CODIGO_DEPARTAMENTO = "0";
                    }
                    else
                    {
                        responseF416[i].CODIGO_DEPARTAMENTO = dpto == "0" ? "11" : dpto;
                    }
                }

            }

            // Creo la linea del tipo registro 3
            RegistroTipo3_416Dto evaluacion = new RegistroTipo3_416Dto()
            {
                NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                TipoRegistro = 3,
                TipoEvaluacion = 0,
                TipoFideicomiso = "00",
                CodigoFideicomiso = "000000",
                TipoEntidad = "00",
                CodigoEntidad = "000000",
                ValorFideicomiso = "00000000000000000"
            };

            consecutivo++;

            var bevaluacion = evaluacion.NumeroSecuencia + "" +
                evaluacion.TipoRegistro + "" +
                evaluacion.TipoEvaluacion + "" +
                evaluacion.TipoFideicomiso + "" +
                evaluacion.CodigoFideicomiso + "" +
                evaluacion.TipoEntidad + "" +
                evaluacion.CodigoEntidad + "" +
                evaluacion.ValorFideicomiso;
            constructor.AppendLine(bevaluacion);
            CultureInfo cultureInfo = new CultureInfo("es-ES");
            // Creo la lista de tipo registro 4
            RegistroTipo4_416Dto formato = new RegistroTipo4_416Dto()
            {
                NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                TipoRegistro = 4,
                TipoIdentificacion = 0,
                NumeroIdentificacion = "000000000000",
                CodigoCIIU = "0000",
                CodigoNaturaleza = 0,
                Variable1 = 0,
                Variable2 = 0,
                Variable3 = 0,
                CodigoReportado = 2
            };

            consecutivo++;

            var bformato = formato.NumeroSecuencia + "" +
                formato.TipoRegistro + "" +
                formato.TipoIdentificacion + "" +
                formato.NumeroIdentificacion + "" +
                formato.CodigoCIIU + "" +
                formato.CodigoNaturaleza + "" +
                formato.Variable1 + "" +
                formato.Variable2 + "" +
                formato.Variable3 + "" +
                formato.CodigoReportado;
            constructor.AppendLine(bformato);

            // Con toda la informacion lista, agrupo la lista en una nueva lista que usare para pintar los datos
            var listaGroup = AgrupacionDatos416(responseF416);

            // Utilizo la lista agrupada y las columnas requeridad para armar el tipo registro 5
            foreach (var item in columnas416)
            {
                for (int i = 0; i < listaGroup.Count; i++)
                {
                    int totalSubCuentas = listaGroup.Count();
                    // Recorro columna por columna para hallar su valor en la lista
                    switch (item.Key)
                    {
                        case "TIPO_SOLICITUD":
                            value = listaGroup[i].TipoSolicitud;
                            break;
                        case "ESTADO_SOLICITUD":
                            value = listaGroup[i].EstadoSolicitud;
                            break;
                        case "CAUSAL_RECHAZO":
                            value = listaGroup[i].CausalRechazo;
                            break;
                        case "MODALIDAD_CREDITO":
                            value = listaGroup[i].ModalidadCredito;
                            break;
                        case "CODIGO_DEPARTAMENTO":
                            value = listaGroup[i].CodigoDpto;
                            break;
                        case "CODIGO_CIIU":
                            value = /*listaGroup[i].CodigoCIIU*/ "10";
                            break;
                        case "MONTO_SOLICITUDES":
                            //value = listaGroup[i].MontoSolicitudes.ToString("N", cultureInfo).Replace(cultureInfo.NumberFormat.CurrencyGroupSeparator, "").Replace(cultureInfo.NumberFormat.CurrencyDecimalSeparator, "");
                            if (listaGroup[i].MontoSolicitudes <= 0)
                            {
                                value = "x";
                            }
                            else
                            {
                                value = ((int)listaGroup[i].MontoSolicitudes).ToString();
                            }
                            break;
                        case "NUMERO_SOLICITUDES":
                            value = listaGroup[i].NumeroSolicitudes.ToString();
                            break;
                    }

                    if (value != "0")
                    {
                        if (value.Equals("x"))
                        {
                            value = "0";
                        }
                        RegistroTipo5_416Dto detalle = new RegistroTipo5_416Dto()
                        {
                            NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                            TipoRegistro = 5,
                            CodigoFormato = "416",
                            CodigoColumna = item.Value,
                            CodigoUnidadCaptura = "01",
                            CodigoSubCuenta = (i + 1).ToString("D6"),
                            Signo = "+",
                            Valor = value.PadLeft(17, '0').Replace(',', '0')
                        };

                        consecutivo++;

                        var data = detalle.NumeroSecuencia + "" +
                        detalle.TipoRegistro + "" +
                        detalle.CodigoFormato + "" +
                        detalle.CodigoColumna + "" +
                        detalle.CodigoUnidadCaptura + "" +
                        detalle.CodigoSubCuenta + "" +
                        detalle.Signo + "" +
                        detalle.Valor;
                        constructor.AppendLine(data);
                    }

                };
            }

            consecutivo++;

            // Creo la lista de tipo registro 6
            RegistroTipo6_416Dto fin = new RegistroTipo6_416Dto
            {
                NumeroSecuencia = consecutivo.ToString("D8"),
                TipoRegistro = 6
            };
            //Fecha del corte
            DateTime fechaCorte = parametrosAplicacionPago.FechaAplicacion;



            // Obtener el último día del mes
            int ultimoDiaDelMes = DateTime.DaysInMonth(fechaCorte.Year, fechaCorte.Month);
            fechaCorte = new DateTime(fechaCorte.Year, fechaCorte.Month, ultimoDiaDelMes);


            // Creo la lista de tipo registro 1
            RegistroTipo1_416Dto encabezado = new RegistroTipo1_416Dto
            {
                NumeroSecuencia = 1.ToString("D8"), // Por defecto 1 para iniciar
                TipoRegistro = 1,
                TipoEntidad = 22,
                CodigoEntidad = "000011",
                FechaCorte = fechaCorte.ToString("dd/MM/yyyy"),
                TotalRegistros = consecutivo.ToString("D8"),
                Entidad = "IOECAJAPROM",
                AreaInformacion = "09",
                TipoInforme = "42"
            };

            consecutivo++;

            var aencabezado = encabezado.NumeroSecuencia + "" +
                encabezado.TipoRegistro + "" +
                encabezado.TipoEntidad + "" +
                encabezado.CodigoEntidad + "" +
                encabezado.FechaCorte + "" +
                encabezado.TotalRegistros + "" +
                encabezado.Entidad + "" +
                encabezado.AreaInformacion + "" +
                encabezado.TipoInforme;
            constructorEncabezado.AppendLine(aencabezado);

            var dfin = (fin.NumeroSecuencia + fin.TipoRegistro.ToString()).TrimEnd();

            // En este string builder va el tipo 3-4-5-6
            constructor.AppendLine(dfin.TrimEnd());

            // En este string builder va el tipo 1 y lo unifico con los 3-4-5-6
            constructorEncabezado.AppendLine(constructor.ToString().TrimEnd());

            byte[] buffer = Encoding.ASCII.GetBytes(constructorEncabezado.ToString().TrimEnd().Replace("/", ""));

            return buffer;
        }

        /// <summary>
        /// ArmarPlano477
        /// </summary>
        /// <param name="responseF477"></param>
        /// <param name="parametrosAplicacionPago"></param>
        /// <returns></returns>
        private async Task<byte[]> ArmarPlano477(
            List<ReporteSFCF477Dto> responseF477,
            ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            StringBuilder constructor = new StringBuilder();

            foreach (var registro in responseF477.OrderBy(x => x.Orden))
            {
                constructor.AppendLine(registro.Registro);
            }

            return await Task.FromResult(
                Encoding.ASCII.GetBytes(constructor.ToString().TrimEnd()));
        }

        private async Task<byte[]> ArmarPlano536(
            List<ReporteSFCF536> responseF477,
            ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            StringBuilder constructor = new StringBuilder();

            foreach (var registro in responseF477.OrderBy(x => x.ITEM))
            {
                constructor.AppendLine(registro.REGISTRO);
            }

            return await Task.FromResult(
                Encoding.ASCII.GetBytes(constructor.ToString().TrimEnd()));
        }

        /// <summary>
        /// ArmarPlano416
        /// </summary>
        /// <param name="responseF414"></param>
        /// <param name="ArmarPlano341"></param>
        /// <returns></returns>
        private async Task<byte[]> ArmarPlano341(List<DocumentoPorPeriodoDto> responseF341, ParametrosSuperfinancieraReportesDto parametrosAplicacionPago)
        {
            AfiliadoDto afiliadoPrueba = new AfiliadoDto();
            try
            {
                StringBuilder constructorEncabezado = new StringBuilder();
                StringBuilder constructor = new StringBuilder();
                int consecutivo = 1;
                string value = string.Empty;
                string valorUnidadCaptura = string.Empty;

                // Columnas requeridas en el reporte
                var columnas341 = ColumnasRequeridas341();

                // Con los documentos de GA2 consulto la informacion de BUA
                var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(responseF341
                 .Select(x => x.Identificacion).ToList())).Data;


                // Creacion registro tipo 2
                foreach (var item in listAfiliados)
                {
                    var nombre = item.NombreCompletoInvertido.PadRight(50, ' ');
                    var nombreSinTildes = SinTildes(nombre).Replace("Ñ", "N").Replace("ñ", "n").Replace("¿", "U").Replace("?", "U").Replace("  ", " ");
                    RegistroTipo2_341Dto informacion = new RegistroTipo2_341Dto()
                    {
                        NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                        TipoRegistro = 2,
                        TipoIdentificacion = item.TipoDocumento,
                        Identificacion = item.Documento.PadLeft(12, '0'),
                        DigitoChequeo = "N",
                        Nombres = nombreSinTildes
                    };

                    consecutivo++;

                    var ainformacion = informacion.NumeroSecuencia + "" +
                        informacion.TipoRegistro + "" +
                        informacion.TipoIdentificacion + "" +
                        informacion.Identificacion + "" +
                        informacion.DigitoChequeo + "" +
                         informacion.Nombres.PadRight(50, ' '); ;
                    constructor.AppendLine(ainformacion);
                }

                //  Creacion registro tipo 3
                RegistroTipo3_341Dto registro = new RegistroTipo3_341Dto()
                {
                    NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                    TipoRegistro = 3,
                    TipoEvaluacion = "0",
                    TipoFideicomiso = "00",
                    CodigoFideicomiso = "000000",
                    TipoEntidad = "00",
                    CodigoEntidad = "000000",
                    ValorFideicomiso = "00000000000000000"
                };

                consecutivo++;

                var aregistro = registro.NumeroSecuencia + "" +
                    registro.TipoRegistro + "" +
                    registro.TipoEvaluacion + "" +
                    registro.TipoFideicomiso + "" +
                    registro.CodigoFideicomiso + "" +
                    registro.TipoEntidad + "" +
                    registro.CodigoEntidad + "" +
                    registro.ValorFideicomiso;

                constructor.AppendLine(aregistro);
                // Creacion registro tipo 4
                foreach (var item in listAfiliados)
                {
                    afiliadoPrueba = item;
                    RegistroTipo4_341Dto registroTipo4 = new RegistroTipo4_341Dto()
                    {
                        NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                        TipoRegistro = 4,
                        TipoIdentificacion = item.TipoDocumento,
                        Identificacion = item.Documento.PadLeft(12, '0'),
                        CodigoCIIU = "6499",
                        Naturaleza = "0",
                        CodigoReestructurado = "2",
                        CodigoAcrededores = "0",
                        CodigoAcuerdo = "0",
                        CodigoReportado = "0"
                    };

                    consecutivo++;

                    var aregistroTipo4 = registroTipo4.NumeroSecuencia + "" +
                        registroTipo4.TipoRegistro + "" +
                        registroTipo4.TipoIdentificacion + "" +
                        registroTipo4.Identificacion + "" +
                        registroTipo4.CodigoCIIU + "" +
                        registroTipo4.Naturaleza + "" +
                        registroTipo4.CodigoReestructurado + "" +
                        registroTipo4.CodigoAcrededores + "" +
                        registroTipo4.CodigoAcuerdo + "" +
                        registroTipo4.CodigoReportado;
                    constructor.AppendLine(aregistroTipo4);

                    ParametrosReportePlano341 parametros = new ParametrosReportePlano341()
                    {
                        FechaAplicacion = parametrosAplicacionPago.FechaAplicacion,
                        Identificacion = item.Documento
                    };
                    parametros.FechaAplicacion = Convert.ToDateTime(parametros.FechaAplicacion.ToString("yyyy-MM-dd"));
                    var response = _mapper.Map<List<Datos_341Dto>>(await _reportesRepository.ReportePlano341(parametros));

                    try
                    {
                        foreach (var itemCol in columnas341)
                        {

                            if (response.Count > 0)
                            {
                                for (int i = 0; i < response.Count; i++)
                                {
                                    string tipoColumna = itemCol.Key;
                                    valorUnidadCaptura = response[i].TipoVivienda.ToString() == "1" ? "09" : "10";
                                    #region Validamos si se muestra o no
                                    if (response[i].TipoCredito.Equals(2))
                                    {
                                        if (itemCol.Key.Equals("CONCEPTOS"))
                                        {
                                            if (!response[i].EstadoCredito.Equals("2"))
                                                break;
                                        }
                                        if (itemCol.Key.Equals("SALDO_INTERESES"))
                                        {
                                            if (!response[i].EstadoCredito.Equals("2"))
                                                break;
                                            if (response[i].SaldoIntereses == 0)
                                                break;
                                        }
                                    }

                                    if (response[i].TipoCredito.Equals(1))
                                    {
                                        if (itemCol.Key.Equals("CONCEPTOS"))
                                        {
                                            if (!response[i].EstadoCredito.Equals("2"))
                                                break;
                                        }
                                        if (itemCol.Key.Equals("SALDO_INTERESES"))
                                        {
                                            if (response[i].SaldoIntereses == 0)
                                                break;
                                        }
                                    }

                                    if (itemCol.Key.Equals("CAPITAL17"))
                                    {
                                        if (response[i].CuotaCapital == 0)
                                            break;
                                    }

                                    if (itemCol.Key.Equals("INTERESES"))
                                    {
                                        if (response[i].CuotaInteres == 0)
                                            break;
                                    }

                                    if (itemCol.Key.Equals("CAPITALNORMAL"))
                                    {
                                        if (response[i].CapitalPagadoPeriodo == 0)
                                            break;
                                    }

                                    if (itemCol.Key.Equals("INTERESESNORMAL"))
                                    {
                                        if (response[i].InteresPagadoPeriodo == 0)
                                            break;
                                    }

                                    #endregion
                                    switch (tipoColumna)
                                    {
                                        case "NRO_OPERACIONES":
                                            value = response[i].NumeroOperacion.ToString();
                                            break;

                                        case "CAPITAL":
                                            value = response[i].SaldoCapital.ToString();
                                            break;

                                        case "CONCEPTOS":
                                            if (response[i].EstadoCredito.Equals("2"))
                                            {
                                                value = Double.Parse(response[i].Concepto.ToString()).ToString("0").Replace(",", ".");
                                            }
                                            break;

                                        case "SALDO_INTERESES":
                                            value = response[i].SaldoIntereses.ToString();
                                            break;
                                        case "PROVISIONES":
                                            value = response[i].Provisiones < 1 ? "0" : Double.Parse(response[i].Provisiones.ToString()).ToString("0.00").Replace(",", ".");

                                            break;
                                        case "PROVISIONES2":
                                            value = "0.00";
                                            break;

                                        case "VALORGARANTIA":
                                            value = response[i].ValorGarantia.ToString();
                                            break;
                                        case "TIPOGARANTIA":
                                            value = response[i].TipoGarantia.ToString();
                                            break;

                                        case "FECHAULTIMOAVALUO":
                                            string fechaDesembolsoStr = response[i].FecAvaluo;

                                            value = fechaDesembolsoStr;

                                            // Ahora, fechaDesembolsoStr contiene la fecha ajustada según la lógica proporcionada.

                                            break;
                                        case "CAPITAL17"://17
                                            value = response[i].CuotaCapital.ToString();
                                            break;
                                        case "INTERESES"://19
                                            value = response[i].CuotaInteres.ToString();
                                            break;
                                        case "CAPITALNORMAL"://21
                                            value = response[i].CapitalPagadoPeriodo.ToString();
                                            break;
                                        case "INTERESESNORMAL"://23
                                            value = (Double.Parse(response[i].InteresPagadoPeriodo.ToString())).ToString("0.00").Replace(",", ".");
                                            break;
                                        case "REDESCUENTO":
                                            value = response[i].Redescuento;
                                            break;
                                        case "FECHAINICIALCREDITO":
                                            value = response[i].FechaDesembolso;
                                            break;
                                        case "FECHAVENCIMIENTOCREDITO":
                                            value = response[i].FechaVencimiento;
                                            break;
                                        case "TASAPROMEDIOCREDITO":
                                            string tasa = (Double.Parse(response[i].TasaEA.ToString())).ToString("0.00").Replace(",", ".");
                                            value = $"{tasa}";
                                            break;
                                        case "DIASDEMORA":
                                            var mora = response[i].DiasMora;
                                            value = mora == 0 ? mora.ToString("0") : mora.ToString();
                                            break;
                                        case "MODELO":
                                            value = response[i].Modelo.ToString();
                                            break;
                                        case "PROBABILIDADINCUMPLIMIENTO":
                                            value = "0.00";
                                            break;
                                        case "EXPOSICION":
                                            if (response[i].TipoCredito.Equals(1))
                                            {
                                                var exposicion = response[i].SaldoCapital;
                                                if (response[i].EstadoCredito.Equals("2"))
                                                {
                                                    exposicion += response[i].SaldoIntereses + response[i].Concepto;
                                                }
                                                else
                                                {
                                                    exposicion += response[i].SaldoIntereses;
                                                }
                                                value = exposicion.ToString("0.00").Replace(",", ".");
                                            }
                                            else
                                            {
                                                var exposicion = response[i].SaldoCapital;
                                                if (response[i].EstadoCredito.Equals("2"))
                                                {
                                                    exposicion += response[i].SaldoIntereses + response[i].Concepto;
                                                }
                                                value = exposicion.ToString("0.00").Replace(",", ".");
                                            }
                                            break;
                                    }

                                    RegistroTipo5_341Dto registroTipo5 = new RegistroTipo5_341Dto()
                                    {
                                        NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                                        TipoRegistro = 5,
                                        CodigoFormato = "341",
                                        CodigoColumna = itemCol.Value,
                                        Signo = "+",
                                        CodigoSubCuenta = "010",
                                        CodigoUnidadCaptura = valorUnidadCaptura,
                                        Valor = value.PadLeft(17, '0').Replace(',', '.')
                                    };

                                    consecutivo++;

                                    var aregistroTipo5 = registroTipo5.NumeroSecuencia + "" +
                                        registroTipo5.TipoRegistro + "" +
                                        registroTipo5.CodigoFormato + "" +
                                        registroTipo5.CodigoColumna + "" +
                                        registroTipo5.CodigoUnidadCaptura + "" +
                                        registroTipo5.CodigoSubCuenta + "" +
                                        registroTipo5.Signo + "" +
                                        registroTipo5.Valor;
                                    constructor.AppendLine(aregistroTipo5);

                                    if (itemCol.Key == "NRO_OPERACIONES" || itemCol.Key == "CAPITAL"
                                        || itemCol.Key == "SALDO_INTERESES" || itemCol.Key == "PROVISIONES"
                                        || itemCol.Key == "VALORGARANTIA" || itemCol.Key == "CAPITAL17"
                                        || itemCol.Key == "INTERESES" || itemCol.Key == "CAPITALNORMAL"
                                        || itemCol.Key == "INTERESESNORMAL" || itemCol.Key == "EXPOSICION"
                                        || itemCol.Key == "CONCEPTOS"
                                        || itemCol.Key == "PROVISIONES2")
                                    {
                                        RegistroTipo5_341Dto registroTipo5A = new RegistroTipo5_341Dto()
                                        {
                                            NumeroSecuencia = (consecutivo + 1).ToString("D8"),
                                            TipoRegistro = 5,
                                            CodigoFormato = "341",
                                            CodigoColumna = itemCol.Value,
                                            Signo = "+",
                                            CodigoSubCuenta = "005",
                                            CodigoUnidadCaptura = "13",
                                            Valor = value.PadLeft(17, '0').Replace(',', '.')
                                        };

                                        consecutivo++;

                                        var aregistroTipo5A = registroTipo5A.NumeroSecuencia + "" +
                                            registroTipo5A.TipoRegistro + "" +
                                            registroTipo5A.CodigoFormato + "" +
                                            registroTipo5A.CodigoColumna + "" +
                                            registroTipo5A.CodigoUnidadCaptura + "" +
                                            registroTipo5A.CodigoSubCuenta + "" +
                                            registroTipo5A.Signo + "" +
                                            registroTipo5A.Valor;
                                        constructor.AppendLine(aregistroTipo5A);
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {

                        throw e.InnerException;
                        Console.WriteLine("el error ha occurido en el contador :");
                    }

                }

                consecutivo += 1;
                // Creacion registro tipo 6
                RegistroTipo6_341Dto fin = new RegistroTipo6_341Dto
                {
                    NumeroSecuencia = consecutivo.ToString("D8"),
                    TipoRegistro = 6
                };

                // Creacion registro tipo 1
                RegistroTipo1_341Dto encabezado = new RegistroTipo1_341Dto
                {
                    NumeroSecuencia = 1.ToString("D8"),
                    TipoRegistro = 1,
                    TipoEntidad = 22,
                    CodigoEntidad = "000011",
                    FechaCorte = parametrosAplicacionPago.FechaAplicacion.ToString("ddMMyyyy"),
                    TotalRegistros = consecutivo.ToString("D8"),
                    PalabraClave = "IOECAJAPROM",
                    AreaInformacion = "03",
                    TipoInforme = "00"
                };

                var aencabezado = encabezado.NumeroSecuencia + "" +
                    encabezado.TipoRegistro + "" +
                    encabezado.TipoEntidad + "" +
                    encabezado.CodigoEntidad + "" +
                    encabezado.FechaCorte + "" +
                    encabezado.TotalRegistros + "" +
                    encabezado.PalabraClave + "" +
                    encabezado.AreaInformacion + "" +
                    encabezado.TipoInforme;
                constructorEncabezado.AppendLine(aencabezado);

                var dfin = (fin.NumeroSecuencia + fin.TipoRegistro.ToString()).TrimEnd();

                // En este string builder va el tipo 3-4-5-6
                constructor.AppendLine(dfin.TrimEnd());

                // En este string builder va el tipo 1 y lo unifico con los 3-4-5-6
                constructorEncabezado.AppendLine(constructor.ToString());

                var result = constructorEncabezado.ToString().TrimEnd();
                byte[] buffer = Encoding.ASCII.GetBytes(result);

                return buffer;
            }
            catch (Exception e)
            {
                var prueba = afiliadoPrueba;
                throw; // Lanzar la excepción original sin modificarla
            }
        }

        /// <summary>
        /// Metodo que retorna una cadena sin tildes
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        private string SinTildes(string texto) => new String(texto.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()).Normalize(NormalizationForm.FormC);


        /// <summary>
        /// ObtenerFecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private string ObtenerFecha(DateTime fecha)
        {
            int mes = fecha.Month;
            int año = fecha.Year;

            string fechaString = "";
            switch (mes)
            {
                case 1:
                    fechaString = "enero de ";
                    break;
                case 2:
                    fechaString = "febrero de ";
                    break;
                case 3:
                    fechaString = "marzo de ";
                    break;
                case 4:
                    fechaString = "abril de ";
                    break;
                case 5:
                    fechaString = "mayo de ";
                    break;
                case 6:
                    fechaString = "junio de ";
                    break;
                case 7:
                    fechaString = "julio de ";
                    break;
                case 8:
                    fechaString = "agosto de ";
                    break;
                case 9:
                    fechaString = "septiembre de ";
                    break;
                case 10:
                    fechaString = "octubre de ";
                    break;
                case 11:
                    fechaString = "noviembre de ";
                    break;
                case 12:
                    fechaString = "diciembre de ";
                    break;
                default:
                    break;
            }


            return fechaString + año.ToString();
        }

        /// <summary>
        /// ColumnasRequeridas416
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> ColumnasRequeridas416()
        {
            // Columnas requeridas en el reporte
            var columnas416 = new Dictionary<string, string>
            {
                { "TIPO_SOLICITUD", "01" },
                { "ESTADO_SOLICITUD", "02" },
                { "CAUSAL_RECHAZO", "03" },
                { "MODALIDAD_CREDITO", "04" },
                { "CODIGO_DEPARTAMENTO", "05" },
                { "CODIGO_CIIU", "06" },
                { "MONTO_SOLICITUDES", "07" },
                { "NUMERO_SOLICITUDES", "08" }
            };

            return columnas416;
        }

        /// <summary>
        /// AgrupacionDatos416
        /// </summary>
        /// <param name="responseF416"></param>
        /// <returns></returns>
        private static List<ColumnasReporte_416Dto> AgrupacionDatos416(List<ReporteSFCF416Dto> responseF416)
        {
            var listaGroup = responseF416
         .GroupBy(gcs => new { gcs.CODIGO_DEPARTAMENTO, gcs.TIPO_SOLICITUD })
         .Select(group => new ColumnasReporte_416Dto
         {
             CodigoDpto = group.Key.CODIGO_DEPARTAMENTO.ToString(),
             TipoSolicitud = group.Key.TIPO_SOLICITUD.ToString(),
             EstadoSolicitud = group.Select(g => g.ESTADO_SOLICITUD.ToString()).FirstOrDefault(),
             CausalRechazo = group.Select(g => g.CAUSAL_RECHAZO.ToString()).FirstOrDefault(),
             CodigoCIIU = group.Select(g => g.CODIGO_CIIU.ToString()).FirstOrDefault(),
             ModalidadCredito = group.Select(g => g.MODALIDAD_CREDITO.ToString()).FirstOrDefault(),
             MontoSolicitudes = group.Sum(g => g.MONTO_SOLICITUDES),
             NumeroSolicitudes = group.Sum(g => g.NUMERO_SOLICITUDES),
             CodigoSubCuenta = group.Select(g => g.SUBCUENTA.ToString()).FirstOrDefault()
         })
         .ToList();

            return listaGroup;
        }


        /// <summary>
        /// ColumnasRequeridas414
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> ColumnasRequeridas414()
        {
            var columnas414 = new Dictionary<string, string>
            {
                { "TIPO_PERSONA", "01" },
                { "SEXO", "02" },
                { "TAMANO_EMPRESA", "03" },
                { "TIPO_CREDITO", "04" },
                { "TIPO_GARANTIA", "05" },
                { "PRODUCTO_CREDITO", "06" },
                { "PLAZO_CREDITO", "07" },
                { "TASA_EFECTIVA_PROMEDIO_PONDERADA", "08" },
                { "MONTOS_DESEMBOLSADOS", "10" },
                { "NUMERO_CREDITOS_DESEMBOLSADOS", "11" },
            };

            return columnas414;
        }
        private static Dictionary<string, string> ColumnasRequeridas414_V2()
        {
            var columnas414 = new Dictionary<string, string>
            {
                { "TIPO_PERSONA", "01" },
                { "SEXO", "02" },
                { "TAMANO_EMPRESA", "03" },
                { "TIPO_CREDITO", "04" },
                { "TIPO_GARANTIA", "05" },
                { "PRODUCTO_CREDITO", "06" },
                { "PLAZO_CREDITO", "07" },
                { "TASA_EFECTIVA_PROMEDIO_PONDERADA", "08" },
                { "MARGEN", "09" },
                { "MONTOS_DESEMBOLSADOS", "10" },
                { "NUMERO_CREDITOS_DESEMBOLSADOS", "11" },
                //Nuevas Columnas 29/09/2023
                { "GRUPO_ETNICO", "12" },
                { "ANTIGUEDAD_EMPRESA", "13" },
                { "TIPO_TASA", "14" },
                { "RANGO_MONTO_DESEMBOLSADO", "15" },
                { "CLASE_DEUDOR", "16" },
                { "CODIGO_CIIU", "17" },
                { "CODIGO_MUNICIPIO", "18" },
            };

            return columnas414;
        }

        /// <summary>
        /// AgrupacionDatos414
        /// </summary>
        /// <param name="responseF414"></param>
        /// <returns></returns>
        private List<ColumnasReporte_414Dto> AgrupacionDatos414(List<ReporteSFCF414Dto> responseF414)
        {
            int subcuentaCounter = 1;

            var grupos = responseF414
                .GroupBy(r => new
                {
                    r.TIPO_PERSONA,
                    r.SEXO,
                    r.TAMANO_EMPRESA,
                    r.TIPO_CREDITO,
                    r.TIPO_GARANTIA,
                    r.PRODUCTO_CREDITO,
                    r.PLAZO_CREDITO,
                    r.CIUDAD,
                    r.DEPARTAMENTO
                })
                .ToList();

            var agrupados = new List<ColumnasReporte_414Dto>();

            foreach (var g in grupos)
            {
                var subgruposRMD = g.GroupBy(x => x.RMD).ToList();

                foreach (var subgrupo in subgruposRMD)
                {
                    decimal sumaProductos = subgrupo.Sum(x => x.TASA_EFECTIVA_PROMEDIO_PONDERADA * x.MONTOS_DESEMBOLSADOS);
                    decimal sumaMontos = subgrupo.Sum(x => x.MONTOS_DESEMBOLSADOS);
                    decimal promedioTasaEfectiva = sumaMontos > 0 ? Math.Round(sumaProductos / sumaMontos, 2) : 0;

                    decimal montoTotal = subgrupo.Sum(x => x.MONTOS_DESEMBOLSADOS);
                    int numeroCreditos = subgrupo.Count();

                    var columna = new ColumnasReporte_414Dto
                    {
                        CodigoSubCuenta = (subcuentaCounter++).ToString("D3"),
                        Sexo = g.Key.SEXO,
                        PlazoCredito = g.Key.PLAZO_CREDITO,
                        MontoSolicitudes = montoTotal,
                        ProductoCredito = g.Key.PRODUCTO_CREDITO,
                        TamanioEmpresa = g.Key.TAMANO_EMPRESA,
                        TasaEfectivaPromedio = promedioTasaEfectiva,
                        TipoGarantia = g.Key.TIPO_GARANTIA,
                        TipoPersona = g.Key.TIPO_PERSONA,
                        Municipio = $"{g.Key.DEPARTAMENTO}{subgrupo.FirstOrDefault().CIUDAD}",
                        Margen = 0,
                        TipoCredito = g.Key.TIPO_CREDITO,
                        NumeroSolicitudes = 1,
                        RANGO_MONTO_DESEMBOLSADO = subgrupo.Key
                    };

                    agrupados.Add(columna);
                }
            }
            return agrupados;
        }

        /// <summary>
        /// ColumnasRequeridas341
        /// </summary>
        /// <returns></returns>
        //private static Dictionary<string, string> ColumnasRequeridasLeasing341()
        //{
        //    var columnas341 = new Dictionary<string, string>
        //    {
        //        { "NRO_OPERACIONES","01"},
        //        { "CAPITAL","03"},
        //        { "SALDO_INTERESES","05"},
        //        { "CONCEPTOS","07"},
        //        { "PROVISIONES","11"},
        //        { "PROVISIONES2","12"},
        //        { "VALORGARANTIA","13"},
        //        { "TIPOGARANTIA","15"},
        //        { "FECHAULTIMOAVALUO","16"},
        //        { "CAPITAL17","17"},
        //        { "INTERESES","19"},
        //        { "CAPITALNORMAL","21"},
        //        { "INTERESESNORMAL","23"},
        //        { "REDESCUENTO","25"},
        //        { "FECHAINICIALCREDITO","26"},
        //        { "FECHAVENCIMIENTOCREDITO","27"},
        //        { "TASAPROMEDIOCREDITO","28"},
        //        { "DIASDEMORA","29" },
        //        { "MODELO","30"},
        //        { "PROBABILIDADINCUMPLIMIENTO","31"},
        //        { "EXPOSICION","36"},

        //    };

        //    return columnas341;
        //}
        private static Dictionary<string, string> ColumnasRequeridas341()
        {
            var columnas341 = new Dictionary<string, string>
            {
                { "NRO_OPERACIONES","01"},
                { "CAPITAL","03"},
                { "SALDO_INTERESES","05"},
                { "CONCEPTOS","07"},
                { "PROVISIONES","11"},
                { "PROVISIONES2","12"},
                { "VALORGARANTIA","13"},
                { "TIPOGARANTIA","15"},
                { "FECHAULTIMOAVALUO","16"},
                { "CAPITAL17","17"},
                { "INTERESES","19"},
                { "CAPITALNORMAL","21"},
                { "INTERESESNORMAL","23"},
                { "REDESCUENTO","25"},
                { "FECHAINICIALCREDITO","26"},
                { "FECHAVENCIMIENTOCREDITO","27"},
                { "TASAPROMEDIOCREDITO","28"},
                { "DIASDEMORA","29" },
                { "MODELO","30"},
                { "PROBABILIDADINCUMPLIMIENTO","31"},
                { "EXPOSICION","36"},

            };

            return columnas341;
        }
        public async Task<Response<IEnumerable<SettingsDto>>> ConsultaReportesGenerales()
        {
            var response = _mapper.Map<List<SettingsDto>>(await _reportesRepository.ConsultaReportesGenerales());
            return new Response<IEnumerable<SettingsDto>> { Data = response };
        }

        public async Task<Response<IEnumerable<ReporteGeneralMoralDto>>> ReporteGeneralMora()
        {
            var resultData = await _iReportesRequestProvider.ObtenerClienteCorreosErrados();
            var Clientes = _mapper.Map<List<ReporteClienteCorreosErradosDto>>(resultData.Data);
            var response = _mapper.Map<List<ReporteGeneralMoralDto>>(await _reportesRepository.ConsultaReporteGeneralMora());

            List<ReporteGeneralMoralDto> list = (from r in response
                                                 join la in Clientes on r.Identificacion equals la.IDENTIFICACION
                                                 select new ReporteGeneralMoralDto
                                                 {

                                                     NumeroRadicacion = r.NumeroRadicacion,
                                                     Estado = r.Estado,
                                                     CodigoCliente = la.ID_CLIENTE.ToString(),
                                                     NombreCliente = $"{la.SEGUNDO_APELLIDO} {la.PRIMER_APELLIDO} {la.PRIMER_NOMBRE} {la.SEGUNDO_NOMBRE}",
                                                     CodigoEmpresa = r.CodigoEmpresa,
                                                     Empresa = r.Empresa,
                                                     SaldoCapital = r.SaldoCapital,
                                                     ValorCuota = r.ValorCuota,
                                                     CodigoPeriocidad = r.CodigoPeriocidad,
                                                     Periocidad = r.Periocidad,
                                                     TipoRecaudo = r.TipoRecaudo,
                                                     CuotaVencida = r.CuotaVencida,
                                                     VencimientoProximoPago = r.VencimientoProximoPago,
                                                     UltimoPago = r.UltimoPago,
                                                     VencimientoUltimoPago = r.VencimientoUltimoPago,
                                                     ValorUltimoPago = r.ValorUltimoPago,
                                                     Identificacion = r.Identificacion


                                                 }).ToList();

            if (list != null && list.Count > 0)
            {
                var result = _templateExcel.GetReporteMoraCarteraExcel(list);
                list[0].file = result.ToArray();
                return new Response<IEnumerable<ReporteGeneralMoralDto>> { Data = list };

            }
            else
            {

                throw new InvalidOperationException($"No existe información suficiente en la base de datos para generar el reporte general mora cartera");

            }


        }

        public async Task<Response<byte[]>> ReporteCorreosErrados(ParamCorreosErrados paramCorreosErrados)
        {
            var result = await _iReportesRequestProvider.ObtenerClienteCorreosErrados();
            var correosClienteErrados = _mapper.Map<List<ReporteClienteCorreosErradosDto>>(result.Data);
            var dataTableInformacionSolicitud = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(correosClienteErrados));
            return null;
        }

        public async Task<FileResult> ReporteTransUnion(string NumeroIdentificacion)
        {
            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(NumeroIdentificacion)).Data;
            if (datosFinancieros != null)
            {
                var datosPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(NumeroIdentificacion)).Data;
                var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, NumeroIdentificacion)).Data;

                #region ConsumoTU
                ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                {
                    Apellidos = recursos.PrimerApellido,
                    FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                    Identificacion = NumeroIdentificacion,
                    SalarioMensual = (decimal)((double)datosFinancieros.TotalIngresos),
                    TipoIdentificacion = 1,
                    Telefono = recursos.Telefono,
                    TraerHistorialCrediticio = "Y",
                    SLS_ID = datosPersonales.IdSimulacion
                };

                #endregion
                try
                {
                    var ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                    if (!ProspeccionTransUnion.IsSuccess)
                    {
                        throw new ApplicationException("Ocurrio un error generando el reporte: " + ProspeccionTransUnion.Data.detalleError);
                    }
                    string reportFilePath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteTransUnion.rdlc";
                    var report = new Microsoft.Reporting.NETCore.LocalReport();
                    report.ReportPath = reportFilePath;

                    var resultadoParamReporte = await ParamReporteTransUnion(report, await XmlToDCResponse(ProspeccionTransUnion.Data.response));
                    report = resultadoParamReporte.Item1;
                    byte[] reportBytes = null;
                    try
                    {
                        reportBytes = report.Render("PDF");
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException("Ocurrio un error generando el reporte, no´es posible renderizar a PDF.");
                    }
                    return await GenerarReporte(reportBytes, false);
                }
                catch (Exception)
                {
                    throw new ApplicationException("Ocurrio un error generando el reporte, respuesta de transunion incorrecta.");
                }
            }
            else
            {
                throw new ApplicationException("Ocurrio un error generando el reporte, no se tiene el total de ingresos del afiliado.");
            }
        }
        public async Task<FileResult> ReporteInformeCredito(string documento)
        {
            string reportFilePath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReportInformeCredito.rdlc";
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            report.ReportPath = reportFilePath;
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, documento)).Data;
            var resultadoParamReporte = await ParamReporteInformeCredito(report);
            report = resultadoParamReporte;
            byte[] reportBytes = null;
            try
            {
                reportBytes = report.Render("PDF");
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }


            return await GenerarReporte(reportBytes, false);
        }
        private async Task<Microsoft.Reporting.NETCore.LocalReport> ParamReporteInformeCredito(Microsoft.Reporting.NETCore.LocalReport reportParam)
        {
            var dataSets = await ParametrosDataSetReporteInformeCredito(reportParam);
            reportParam = dataSets;

            return reportParam;
        }
        private async Task<Dictionary<string, string>> ParametrosReporteInformeCredito(ResponseGeneralTu.DCResponse data)
        {
            return null;
        }
        private async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosDataSetReporteInformeCredito(Microsoft.Reporting.NETCore.LocalReport reportParam)
        {
            var InfoCarteraDataSet = new Microsoft.Reporting.NETCore.ReportDataSource("InformacionBasica", new List<object>());
            reportParam.DataSources.Add(InfoCarteraDataSet);

            var InfoCarteraDataSet1 = new Microsoft.Reporting.NETCore.ReportDataSource("FinancieroAfiliado", new List<object>());
            reportParam.DataSources.Add(InfoCarteraDataSet1);

            var InfoCarteraDataSet2 = new Microsoft.Reporting.NETCore.ReportDataSource("InformacionInmueble", new List<object>());
            reportParam.DataSources.Add(InfoCarteraDataSet2);

            var InfoCarteraDataSet3 = new Microsoft.Reporting.NETCore.ReportDataSource("Financiacion", new List<object>());
            reportParam.DataSources.Add(InfoCarteraDataSet3);
            var InfoCarteraDataSet4 = new Microsoft.Reporting.NETCore.ReportDataSource("SectorFinanciero", new List<object>());
            reportParam.DataSources.Add(InfoCarteraDataSet4);

            var InfoCarteraDataSet5 = new Microsoft.Reporting.NETCore.ReportDataSource("HistorialCrediticio", new List<object>());
            reportParam.DataSources.Add(InfoCarteraDataSet5);

            return reportParam;
        }
        public async Task<FileResult> ReporteSolicitudTransUnion(ProspeccionTransUnionRequestDto req)
        {
            string xmlTransUnion = "";




            var validacion = new Response<SocSlsSimulacionCreditoDto> { Data = this._mapper.Map<SocSlsSimulacionCreditoDto>(await this._creditoRepository.ObtenerSlsSocSolicitudCreditoNroDocumento(req.Identificacion)) };
            DateTime slsFecha = new DateTime();

            DateTime fechaActual = DateTime.Today;
            if (validacion.Data != null)
            {
                slsFecha = validacion.Data.slsFecha;
            }

            // Calcular la diferencia en meses entre la fecha actual y slsFecha
            TimeSpan diferencia = fechaActual - slsFecha;

            // Validar si la diferencia en meses es mayor a 1
            if (validacion.Data != null)
            {

                try
                {
                    if (validacion.Data.slsId != null && validacion.Data.socId == null)
                    {
                        xmlTransUnion = await DescargarXMLTransUnion(validacion.Data.slsXml);
                    }
                    else if (validacion.Data.slsId != null && validacion.Data.socId != null)
                    {
                        if (validacion.Data.slsXml != null && validacion.Data.socXml == null)
                        {
                            xmlTransUnion = await DescargarXMLTransUnion(validacion.Data.slsXml);
                        }
                        else
                        {
                            xmlTransUnion = await DescargarXMLTransUnion(validacion.Data.socXml);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al descargar el archivo XML: " );
                }
                //return new Response<ResponseTu> { Data = responseTu };
            }
            else
            {
                ResponseTu responseTu = new ResponseTu
                {
                    idAplication = "0",
                    codigoRechazo = 0,
                    detalleRechazo = "OK"
                };
                ValidacionesTU tu = new();
                DCRequest dCRequest = new()
                {
                    Authentication = new() { UserId = _AqmOptions.Value.usuarioAqm, Password = _AqmOptions.Value.claveAqm, Type = "OnDemand" },
                    RequestInfo = new() { SolutionSetId = _AqmOptions.Value.solucionIdAqm, ExecutionMode = "NewWithContext", ExecuteLatestVersion = "true" }
                };
                //Megachcbasv.2026
                var fechaExpedicion = Convert.ToDateTime(req.FechaExpedicionIdentificacion).ToString("dd/MM/yyyy");
                Application.Dto.Applicant ap = new()
                {
                    IdNumber = req.Identificacion,
                    IdType = req.TipoIdentificacion,
                    IdExpeditionDate = fechaExpedicion,
                    RecentPhoneNumber = req.Telefono,
                    LastName = req.Apellidos,
                    Occupation = "EMPLEADO",
                    MonthlyIncome = req.SalarioMensual.ToString(),
                };

                Application.Dto.ApplicationData apData = new()
                {
                    Product = "CONSUMO"
                };

                dCRequest.Fields = new() { Field = new List<Application.Dto.Field>() { new Application.Dto.Field { Key = "PrimaryApplicant", Text = ap.Serialize<Application.Dto.Applicant>() }, new Application.Dto.Field { Key = "ApplicationData", ApplicationData = apData } } };
                //dCRequest.Fields.Add(new() { Key = "ApplicationData", ApplicationData = apd });

                //Guarda consumo de TransUnion
                var respuesta = tu.ConsumirTU(dCRequest, out string request, out string response);
                xmlTransUnion = response;


                if (respuesta.Status.ToString() == "Success" && (respuesta.Authentication.Status.ToString() == "Success" || respuesta.Authentication.Status.ToString() == "FirstTimeLogIn"))
                {
                    Application.Dto.ApplicationData fieldValidacion = Serialization.DeserializeRC(respuesta.ContextData.Field.Where(x => x.Key == "ApplicationData").FirstOrDefault().Text);
                    if (req.TraerHistorialCrediticio.Equals("Y"))
                    {
                        DCRequest dCRequestHistorial = new()
                        {
                            Authentication = new() { UserId = _AqmOptions.Value.usuarioAqm, Password = _AqmOptions.Value.claveAqm, Type = "OnDemand" },
                            RequestInfo = new() { ExecutionMode = "Send", ApplicationId = respuesta.ResponseInfo.ApplicationId, QueueName = "DocVerify" },
                            Fields = new() { Field = new List<Application.Dto.Field>() { new Application.Dto.Field() { Key = "GenerateReport", Text = "Y" } } }
                        };

                        Application.Dto.DCResponse respuesta1 = tu.ConsumirTU(dCRequestHistorial, out string request1, out string response1);
                        if (respuesta1.Status.ToString() == "Success")
                        {
                            var applicantsR = respuesta1.ContextData.Field.Where(x => x.Key.Equals("Applicants"));

                            #region Creacion y Guardado XML-TU
                            byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(response1);
                            using (var memoryStream = new MemoryStream(xmlBytes))
                            {
                                IFormFile formFile = new FormFile(memoryStream, 0, xmlBytes.Length, "XMLTransunion - " + req.Identificacion, "XMLTransunion - " + req.Identificacion + ".xml");
                                await GuardarXmlTU(formFile, req.SLS_ID);
                            }
                            #endregion
                            xmlTransUnion = response1;
                            if (applicantsR.Count<Application.Dto.Field>() > 0)
                            {
                                Applicants ar = Serialization.DeserializeApps(applicantsR.FirstOrDefault().Text);
                                if (ar != null)
                                {
                                    var JsonHistoria = System.Text.Json.JsonSerializer.Serialize(ar.Applicant.DSOnlineBureauCOLData);
                                    responseTu.historialCredito = JsonHistoria;
                                    responseTu.score = ar.Applicant.Score;
                                    responseTu.porcentajeEndeudamiento = decimal.Parse(ar.Applicant.PorcEndFinal);
                                    responseTu.capacidadEndeudamiento = decimal.Parse(ar.Applicant.CapacidadEndeudamiento);
                                }

                            }

                            await CrearLogTu(new SolicitudCreditoProductoDto()
                            {
                                VLI_ID = Guid.NewGuid(),
                                VLT_ID = Guid.NewGuid(),
                                VLT_SERVICIO = "AQM con reporte",
                                VLT_PASO_TRANSACCION = "AQM",
                                VLT_XML_SOLICITUD = request1,
                                VLT_XML_RESPUESTA = response1,
                                VLT_ID_APICACION = respuesta.ResponseInfo.ApplicationId,
                                VLT_CODIGO_RESPUESTA = "true",
                                VLT_TIPO_RESPUESTA = "true",
                                VLT_DESCRIPCION_RESPUESTA = respuesta.ContextData.Field.Where(x => x.Key == "Decision").FirstOrDefault().Text,
                                VLT_FECHA_ACTUALIZA = DateTime.Now
                            });
                        }
                        else
                        {
                            try
                            {
                                var error = await XmlToDcResponse(responseTu.response);
                                var aplicante = error.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants?.Applicant;
                                var aplicanteData = error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
                                responseTu = new ResponseTu
                                {
                                    detalleError = fieldValidacion.DecisionHomologada + " ," + aplicanteData.UltimaEtapaFlujo,
                                    idAplication = error.ResponseInfo.ApplicationId,
                                    response = string.Empty
                                };
                            }
                            catch (Exception)
                            {
                                var error = await XmlToDcResponseDos(responseTu.response);
                                var aplicanteData = error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
                                string errores = string.Join(", ", aplicanteData.InputValReasonCodes.Reason
                                .Where(x => x.Description != null)
                                .Select(x => x.Description));


                                responseTu = new ResponseTu
                                {
                                    detalleError = errores,
                                    idAplication = error.ResponseInfo.ApplicationId,
                                    response = string.Empty
                                };
                                //return new Response<ResponseTu> { Data = responseTu };
                            }

                        }
                    }
                    else
                    {
                        await CrearLogTu(new SolicitudCreditoProductoDto()
                        {
                            VLI_ID = Guid.NewGuid(),
                            VLT_ID = Guid.NewGuid(),
                            VLT_SERVICIO = "AQM sin reporte",
                            VLT_PASO_TRANSACCION = "AQM",
                            VLT_XML_SOLICITUD = request,
                            VLT_XML_RESPUESTA = response,
                            VLT_ID_APICACION = respuesta.ResponseInfo.ApplicationId,
                            VLT_CODIGO_RESPUESTA = "true",
                            VLT_TIPO_RESPUESTA = "true",
                            VLT_DESCRIPCION_RESPUESTA = respuesta?.ContextData?.Field?.FirstOrDefault(x => x.Key == "DecisionHomologada")?.Text ?? "Sin datos",
                            VLT_FECHA_ACTUALIZA = DateTime.Now,
                        });
                    }

                    responseTu.detalleRechazo = fieldValidacion.DecisionHomologada;
                }


            }
            string reportFilePath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteTransUnion.rdlc";
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            report.ReportPath = reportFilePath;

            var resultadoParamReporte = await ParamReporteTransUnion(report, await XmlToDCResponse(xmlTransUnion));
            report = resultadoParamReporte.Item1;
            byte[] reportBytes = null;
            try
            {
                reportBytes = report.Render("PDF");
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }
            return await GenerarReporte(reportBytes, false);
        }
        public async Task<Response<SolicitudCreditoProductoDto>> CrearLogTu(SolicitudCreditoProductoDto datosLog)
        {
            var registroLog = this._mapper.Map<SolicitudCreditoProductoGral>(datosLog);

            return new Response<SolicitudCreditoProductoDto> { Data = this._mapper.Map<SolicitudCreditoProductoDto>(await this._transUnionRepository.CrearLogTu(registroLog)) };
        }
        public async Task<string> DescargarXMLTransUnion(string filename)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "trans-union");
            BlobClient blob = container.GetBlobClient(filename);
            BlobDownloadInfo download = await blob.DownloadAsync();
            string xml = "";
            using (Stream stream = download.Content)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = await reader.ReadToEndAsync();
                }
            }
            return xml;
        }
        private async Task<ResponseErrorTu.DCResponse> XmlToDcResponse(string xml)
        {
            xml = xml
                    .Replace("&amp;", "&")
                    .Replace(":q1", "")
                    .Replace("q1:", "")
                    .Replace("&lt;", "<")
                    .Replace("&gt;", ">");
            XmlSerializer serializer = new XmlSerializer(typeof(ResponseErrorTu.DCResponse));

            using (StringReader reader = new StringReader(xml))
            {
                var data = await Task.Run(() => serializer.Deserialize(reader));
                return (ResponseErrorTu.DCResponse)data;
            }
        }
        private async Task<bool> GuardarXmlTU(IFormFile FileXMLTU, Guid IdSolicitud)
        {
            var RutaImagen = await CargarXMLTransUnion(FileXMLTU);
            RequestUpdateSlsTuDto requestUpdateSlsTuDto = new RequestUpdateSlsTuDto()
            {
                Sls_Id = IdSolicitud,
                xml = RutaImagen.Data,
                Fecha_Reporte_TU = DateTime.Now
            };
            var respuesta = await _creditoRepository.ActualizarSlsReporteTU(this._mapper.Map<UpdateSlsTu>(requestUpdateSlsTuDto));
            return respuesta != null ? true : false;
        }
        public async Task<Response<string>> CargarXMLTransUnion(IFormFile request)
        {
            try
            {
                var _request = request;
                FileInfo fi = new FileInfo(_request.FileName);
                BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "trans-union");
                await container.CreateIfNotExistsAsync();
                var newName = request.Name + fi.Extension;
                BlobClient blob = container.GetBlobClient(newName);
                var result = await blob.UploadAsync(request.OpenReadStream(), true);
                return new Response<string> { Data = newName };
            }
            catch (Exception ex)
            {
                return new Response<string> { Data = string.Empty, IsSuccess = false, ReturnMessage = "Error guardado documento en el Blob Storage" };
            }
        }
        private async Task<ResponseErrorTuDos.DCResponse> XmlToDcResponseDos(string xml)
        {

            xml = xml
                    .Replace("&amp;", "&")
                    .Replace(":q1", "")
                    .Replace("q1:", "")
                    .Replace("&lt;", "<")
                    .Replace("&gt;", ">");
            XmlSerializer serializer = new XmlSerializer(typeof(ResponseErrorTuDos.DCResponse));
            using (StringReader reader = new StringReader(xml))
            {
                var data = await Task.Run(() => serializer.Deserialize(reader));
                return (ResponseErrorTuDos.DCResponse)data;
            }

        }
        public async Task<Response<SimulacionCreditoArchivo>> ReporteTransUnionPropiedades(string xml)
        {
            SimulacionCreditoArchivo result = new SimulacionCreditoArchivo();
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            var reportFilePath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteTransUnion.rdlc";
            report.ReportPath = reportFilePath;
            var XMLToDCResponse = await XmlToDCResponse(xml);
            var resultadoParamReporte = await ParamReporteTransUnion(report, XMLToDCResponse);
            result = new SimulacionCreditoArchivo
            {
                FileName = "ReporteTransUnion.pdf",
                ArchivoBytes = report.Render("PDF"),
                cuotasSectorReal = resultadoParamReporte.Item2,
                cuotasSectorFinanciero = resultadoParamReporte.Item3,
                riesgoMinimo = resultadoParamReporte.Item4,
                estadoDesicion = resultadoParamReporte.Item5,
                descripcionDecision = resultadoParamReporte.Item6,
                totalObligacionMora = resultadoParamReporte.Item7,
                fechaPermanenciaVigente = resultadoParamReporte.Item8,
                comprarCarteraSinViiviendaValidacion = resultadoParamReporte.Item9
            };
            return new Response<SimulacionCreditoArchivo> { Data = result, IsSuccess = true, ReturnMessage = "Solicitud Exitosa" };
        }
        private async Task<ResponseGeneralTu.DCResponse> XmlToDCResponse(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ResponseGeneralTu.DCResponse));
            xml = xml
               .Replace("&amp;", "&")
               .Replace(":q1", "")
               .Replace("q1:", "")
               .Replace("&lt;", "<")
               .Replace("&gt;", ">")
               .Replace("&quot;", "\"");
            using (StringReader reader = new StringReader(xml))
            {
                ResponseGeneralTu.DCResponse data = (ResponseGeneralTu.DCResponse)await Task.Run(() => serializer.Deserialize(reader));
                return data;
            }

        }

        public decimal ValorCompraCartera (decimal valor)
        {
            return valor; 
        }

        private async Task<(Microsoft.Reporting.NETCore.LocalReport, double, double, int, bool, string, double, string, bool)> ParamReporteTransUnion(Microsoft.Reporting.NETCore.LocalReport reportParam, ResponseGeneralTu.DCResponse data)
        {
            // Información básica
            var aplicante = data.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants?.Applicant;
            var aplicanteData = data.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
            var numeroDocumento = data.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants.Applicant.IdNumber;
            double valorSectorReal = 0;
            double valorSectorRealOficial = 0;
            double valorSectorFinan = 0;
            int riesgoMinimo = 0;
            bool estadoDesicion = false;
            bool sinViviendaCompraCartera = false; 
            string descripcionDesicion = aplicanteData != null ? aplicanteData.DecisionHomologada : "";
            string fechaPermanenciaVigente = "";
            double totalSectorRealServicioContrato = 0; 
            double sectorRealSaldo = 0;
            IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, numeroDocumento);
            double totalMoraObligacion = 0;
            if (aplicante.DSOnlineBureauCOLData != null)
            {

                // Si se encuentra un número, convertirlo a entero y devolverlo
                riesgoMinimo = string.IsNullOrEmpty(aplicante.Score) ? 0 : int.Parse(aplicante.Score);

                try
                {
                    await _creditoRepository.LimpiarLineasCreditoObligacion(1, numeroDocumento); 
                    List<string> TiposCreFinExcluidos = new List<string> {  
                        "LIBZ", "LISE", "LIBR", "LBAG", "LADS", "LBCL", "LBCO", "LBCC",
                    "LCON", "LICN", "LBDE", "LBED", "LBEE", "LBEM", "LAAJ", "LBFS",
                    "LBIN", "LBLD", "LBLI", "LBMC", "LBME", "LBNO", "LMOF", "LBOR",
                    "LBPD", "LBRC", "LBRM", "LBRT", "LBSA", "LDSF", "LBSM", "LBVH",
                    "LBMB", "LBVI", "LVIV"};
                    List<string> TiposModalidadCreFinExcluidos = new List<string> {  
                        "LIBZ", "LISE", "LIBR", "LBAG", "LADS", "LBCL", "LBCO", "LBCC",
                    "LCON", "LICN", "LBDE", "LBED", "LBEE", "LBEM", "LAAJ", "LBFS",
                    "LBIN", "LBLD", "LBLI", "LBMC", "LBME", "LBNO", "LMOF", "LBOR",
                    "LBPD", "LBRC", "LBRM", "LBRT", "LBSA", "LDSF", "LBSM", "LBVH",
                    "LBMB", "LBVI", "LVIV" };
                    List<string> TiposCodeudor = new List<string> { "CODE", "DSOL", "AVAL", "COOT" };
                    List<double> LstSectorFinanExclu = new List<double>();
                    List<double> LstModaliaddSectorFinanExclu = new List<double>();
                    List<double> TiposCodeudorTotal = new List<double>();
                    var simulacionInformacion = await this._creditoRepository.ObtenerSimulacionPersonales(numeroDocumento);
                    // Si se encuentra coincidencia true
                    estadoDesicion = aplicanteData.DecisionHomologada.Equals("NO VIABLE") ? true : false;
                    estadoDesicion = aplicanteData.DecisionHomologada.Equals("PENDIENTE") ? true : true;

                    var datosGenerales = await this._carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();
                    double valorTasaUsura = (Math.Pow(1 + ((double)datosGenerales.Data.valorTasaUsura / 100), 1.0 / datosGenerales.Data.MesesDiferir) - 1);


                    valorSectorReal = Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Real"))
                        .Select(x => x.CuotaObligacionesDia).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Real"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000;


                    

                    var listaObligacionSectorRealExcliudos =
    aplicante.DSOnlineBureauCOLData.Response?.Tercero?.SectorRealAlDia?.Obligacion == null
        ? new List<ResponseGeneralTu.Obligacion>()
        : aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorRealAlDia.Obligacion;
                    double restaObligacionesSectorRealExclucios = 0; 
                    foreach(var item in listaObligacionSectorRealExcliudos)
                    {
                        string lineaCreditoSectorReal = item.LineaCredito; 
                        foreach(var excepcionesSectorReal in TiposCreFinExcluidos)
                        {
                            if (item.ValorCuota == "")
                            {
                                item.ValorCuota = "0";
                            }
                            if (excepcionesSectorReal == lineaCreditoSectorReal)
                            {
                                if(double.Parse(item.ValorCuota) == 0 && double.Parse(item.SaldoObligacion) > 0)
                                {
                                    restaObligacionesSectorRealExclucios += (double.Parse(item.SaldoObligacion) * 1000 * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));

                                }
                                else
                                {
                                    restaObligacionesSectorRealExclucios = +double.Parse(item.ValorCuota) * 1000;
                                }
                            }
                        }
                    }

                    valorSectorReal = valorSectorReal - restaObligacionesSectorRealExclucios;

                    if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.SectorRealAlDia != null)
                    {
                        foreach (var item in aplicante.DSOnlineBureauCOLData.Response?.Tercero?.SectorRealAlDia.Obligacion)
                        {
                            if (item.TipoContrato == "SRV" && item.LineaCredito == "TELC" && Convert.ToDouble(item.SaldoObligacion) > 0)
                            {
                                valorSectorReal += Convert.ToDouble(item.SaldoObligacion) * 1000;
                            }
                            else if (Convert.ToDouble(item.SaldoObligacion) > 0 && item.ValorCuota == "0")
                            {
                                if (Convert.ToDouble(item.SaldoObligacion) * 1000 < 100000)
                                {
                                    valorSectorReal += Convert.ToDouble(item.SaldoObligacion) * 1000;                                    
                                }
                                else
                                {
                                    valorSectorReal += (Convert.ToDouble(item.SaldoObligacion) * 1000 * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                }
                            }
                        }
                    }
                 
                   

                    //var SaldoSectorReal = Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                    //    .Where(x => x.PaqueteInformacion.Equals("Sector Real"))
                    //    .Select(x => x.SaldoObligacionesDia).FirstOrDefault() ?? "0") * 1000;

                    //valorSectorReal += (SaldoSectorReal * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));

                    valorSectorFinan = Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Solidario"))
                        .Select(x => x.CuotaObligacionesDia).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Solidario"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Financiero"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Financiero"))
                        .Select(x => x.CuotaObligacionesDia).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Tarjetas De Credito"))
                        .Select(x => x.CuotaObligacionesDia).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Tarjetas De Credito"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000;

                    //Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                    //    .Where(x => x.PaqueteInformacion.Equals("Sector Financiero"))
                    //    .Select(x => x.CuotaObligacionesDia).FirstOrDefault() ?? "0") * 1000 +

                    valorSectorFinan = valorSectorFinan + Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenDiferentePrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Subtotal Codeudores"))
                        .Select(x => x.CuotaObligacionesDia).FirstOrDefault() ?? "0") * 1000;


                    totalMoraObligacion = Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Solidario"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Financiero"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Tarjetas De Credito"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Sector Real"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000 +
                        Convert.ToDouble(aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenDiferentePrincipal?.Registro?
                        .Where(x => x.PaqueteInformacion.Equals("Subtotal Codeudores"))
                        .Select(x => x.ValorMora).FirstOrDefault() ?? "0") * 1000;


                    bool ExcluidoCodeudor = false; 
                    if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.SectorFinancieroAlDia != null)
                    {


                        LstSectorFinanExclu = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion.Where(item => TiposCreFinExcluidos.Any(valor => valor == item.LineaCredito)).Select(x => Convert.ToDouble(x.ValorCuota) * 1000).ToList();
                        if (LstSectorFinanExclu.Count > 0)
                        {
                            valorSectorFinan = valorSectorFinan - LstSectorFinanExclu.Sum();
                            ExcluidoCodeudor = true; 
                        }


                        LstSectorFinanExclu = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion.Where(item => "VIVI" == item.ModalidadCredito).Select(x => Convert.ToDouble(x.ValorCuota) * 1000).ToList();
                        if (LstSectorFinanExclu.Count > 0 && simulacionInformacion.SDP_OPCION_COMPRA_CARTERA)
                        {
                            valorSectorFinan = valorSectorFinan - LstSectorFinanExclu.Sum();
                            ExcluidoCodeudor = true;
                        }



                        var obligacionesModalidadLineaCredito = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorFinancieroAlDia?.Obligacion;

                        foreach (var obligacion in obligacionesModalidadLineaCredito)
                        {
                            if ((obligacion.Calidad == "CODE" || obligacion.Calidad == "DSOL" || obligacion.Calidad == "COOT" || obligacion.Calidad == "AVAL") && !ExcluidoCodeudor)
                            {
                                TiposCodeudorTotal = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion.Where(item => TiposCodeudor.Any(valor => valor == item.Calidad)).Select(x => Convert.ToDouble(x.ValorCuota) * 1000).ToList();
                                if (TiposCodeudorTotal.Count > 0)
                                {
                                    valorSectorFinan = valorSectorFinan - TiposCodeudorTotal.Sum();
                                    break;
                                }
                            }
                        }



                        //bool excluirVivienda = false; 
                        if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.SectorFinancieroAlDia != null)
                        {
                            //LstSectorFinanExclu = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion.Where(item => TiposCreFinExcluidos.Any(valor => valor == item.LineaCredito)).Select(x => Convert.ToDouble(x.ValorCuota) * 1000).ToList();
                            //if (LstSectorFinanExclu.Count > 0)
                            //{
                            //    valorSectorFinan = valorSectorFinan - LstSectorFinanExclu.Sum();
                            //}
                            string numeroObligacionCartera = simulacionInformacion.SDP_NUMERO_OBLIGACION_COMPRA_CARTERA; 
                            var obligaciones = aplicante.DSOnlineBureauCOLData?
    .Response?
    .Tercero?
    .SectorFinancieroAlDia?
    .Obligacion?
    .Where(item => !TiposCreFinExcluidos.Contains(item.LineaCredito))
    .Where(item => !TiposModalidadCreFinExcluidos.Contains(item.ModalidadCredito))
    .ToList();

                            if(obligaciones == null && simulacionInformacion.SDP_OPCION_COMPRA_CARTERA)
                            {
                                sinViviendaCompraCartera = true;
                            }

                            if (obligaciones != null && simulacionInformacion.SDP_OPCION_COMPRA_CARTERA)
                            {
                                var obligacionesVivi = obligaciones
                                    .Where(x => x.ModalidadCredito == "VIVI" && x.Calidad == "PRIN")
                                    .ToList();

                                if (obligacionesVivi.Count == 0)
                                {
                                    sinViviendaCompraCartera = true;
                                }

                                if (obligacionesVivi.Any())
                                {
                                    bool obligacionEliminada = false;

                                    // Intentar eliminar por número de obligación
                                    if (!string.IsNullOrWhiteSpace(numeroObligacionCartera))
                                    {
                                        var obligacionPorNumero = obligacionesVivi.FirstOrDefault(x =>
                                            x.NumeroObligacion == numeroObligacionCartera);

                                        if (obligacionPorNumero != null)
                                        {
                                            obligaciones.Remove(obligacionPorNumero);
                                            obligacionEliminada = true;
                                        }
                                    }

                                    // Si no se eliminó por número, aplicar la lógica actual
                                    if (!obligacionEliminada)
                                    {
                                        decimal valorObjetivo = simulacionInformacion.SDP_VALOR_PRESTAMO_COMPRA_CARTERA;
                                        decimal margen = valorObjetivo * 0.05m;

                                        var obligacionEliminar = obligacionesVivi
                                            .Select(x =>
                                            {
                                                decimal saldo = Convert.ToDecimal(x.SaldoObligacion) * 1000m;

                                                return new
                                                {
                                                    Obligacion = x,
                                                    Diferencia = Math.Abs(saldo - valorObjetivo)
                                                };
                                            })
                                            .OrderBy(x => x.Diferencia)
                                            .FirstOrDefault();

                                        if (obligacionEliminar != null &&
                                            obligacionEliminar.Diferencia <= margen)
                                        {
                                            obligaciones.Remove(obligacionEliminar.Obligacion);
                                        }
                                        else
                                        {
                                            estadoDesicion = true;
                                            descripcionDesicion = "PENDIENTE";
                                        }
                                    }
                                }
                            }

                            if (obligaciones != null)
                            {
                                foreach (var obligacion in obligaciones)
                                {

                                  

                                    if (obligacion.ValorCuota == "")
                                    {
                                        obligacion.ValorCuota = "0";
                                    }

                                    double saldoObligacion;
                                    double valorCuotaReclasificacion = 0; 
                                    double numeroCuotasPactadas = 0;
                                    double numeroCuotasCanceladas = 0;
                                    double numeroCuotasRestantes = 0;
                                    double mesesDiferir = 0;

                                    if (obligacion.LineaCredito != "TCR")
                                    {
                                        numeroCuotasPactadas = obligacion.NumeroCuotasPactadas != "" && obligacion.NumeroCuotasPactadas != null ? double.Parse(obligacion.NumeroCuotasPactadas) : 0;
                                        numeroCuotasCanceladas = obligacion.CuotasCanceladas != "" && obligacion.CuotasCanceladas != null ? double.Parse(obligacion.CuotasCanceladas) : 0;
                                        numeroCuotasRestantes = numeroCuotasPactadas - numeroCuotasCanceladas;
                                    }

                                    mesesDiferir = numeroCuotasRestantes == 0 ? datosGenerales.Data.MesesDiferir : numeroCuotasRestantes;

               
                                        if (double.TryParse(obligacion.SaldoObligacion, out saldoObligacion) && obligacion.ValorCuota == "0" && saldoObligacion > 0)
                                        {
                                            double SaldoCero = saldoObligacion * 1000;
                                            //double valorTasaUsura = ((double?)datosGenerales.Data.valorTasaUsura / datosGenerales.Data.MesesDiferir / 100.0 ?? 0);

                                            if (obligacion.LineaCredito == "TCR" || obligacion.LineaCredito == "LINV")
                                            {
                                                valorCuotaReclasificacion = (SaldoCero * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -mesesDiferir));
                                                valorSectorFinan += (SaldoCero * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -mesesDiferir));
                                            }
                                            else if ((obligacion.LineaCredito == "VIVI" || obligacion.LineaCredito == "VEHI"))
                                            {
                                                if (obligacion.Calidad == "CODE" || obligacion.Calidad == "DSOL" || obligacion.Calidad == "AVAL" || obligacion.Calidad == "COOT")
                                                {
                                                    valorCuotaReclasificacion = ((SaldoCero / 2) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                                    valorSectorFinan += ((SaldoCero / 2) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                                }
                                                else if (obligacion.Calidad == "PRIN")
                                                {
                                                    valorCuotaReclasificacion = ((SaldoCero) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                                    valorSectorFinan += ((SaldoCero) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                                }
                                            }
                                            else if (obligacion.LineaCredito == "LFIN" || obligacion.LineaCredito == "ANOM")
                                            {
                                                valorCuotaReclasificacion += ((SaldoCero) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                                valorSectorFinan += ((SaldoCero) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                            }
                                            else if (obligacion.LineaCredito == "EDUC" && obligacion.Calidad == "PRIN")
                                            {
                                                valorCuotaReclasificacion += ((SaldoCero) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                                valorSectorFinan += ((SaldoCero) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                            }
                                            else if (obligacion.LineaCredito != "VIVI")
                                            {
                                                valorSectorFinan += ((Convert.ToDouble(obligacion.SaldoObligacion) * 1000) * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                            }
                                        }
                                        else if (double.TryParse(obligacion.SaldoObligacion, out saldoObligacion) && double.Parse(obligacion.ValorCuota) > 0)
                                        {
                                            if ((obligacion.LineaCredito == "VIVI" || obligacion.LineaCredito == "VEHI") && !simulacionInformacion.SDP_OPCION_COMPRA_CARTERA)
                                            {
                                                if (obligacion.Calidad == "CODE" || obligacion.Calidad == "DSOL" || obligacion.Calidad == "AVAL" || obligacion.Calidad == "COOT")
                                                {
                                                    valorCuotaReclasificacion += (double.Parse(obligacion.ValorCuota) * 1000) / 2;
                                                    valorSectorFinan += (double.Parse(obligacion.ValorCuota) * 1000) / 2;
                                                }                                               
                                            }
                                        }

                                        if(obligacion.ModalidadCredito == "VIVI" && simulacionInformacion.SDP_OPCION_COMPRA_CARTERA && Convert.ToDouble(obligacion.SaldoObligacion)>0 && obligacion.ValorCuota=="0")
                                        {
                                            valorSectorFinan += (Convert.ToDouble(obligacion.SaldoObligacion)*1000 * valorTasaUsura) / (1 - Math.Pow(1 + valorTasaUsura, -datosGenerales.Data.MesesDiferir));
                                        }
                                        else if (obligacion.ModalidadCredito == "VIVI" && simulacionInformacion.SDP_OPCION_COMPRA_CARTERA && Convert.ToDouble(obligacion.SaldoObligacion) > 0)
                                        {
                                            valorSectorFinan += Convert.ToDouble(obligacion.ValorCuota)*1000; 
                                        }




                                    //int cantidadVivi = obligaciones.Count(o => o.ModalidadCredito == "VIVI");

                                    //if (simulacionInformacion.SDP_OPCION_COMPRA_CARTERA && obligacion.ModalidadCredito == "VIVI" && cantidadVivi>=2)
                                    //{
                                    //    double valorPorcentajeTolerancia = Math.Abs(Convert.ToDouble(obligacion.SaldoObligacion) - 390000000);

                                    //    double valorPorcentaje = (valorPorcentajeTolerancia/ 390000000)*100; 

                                    //    if (valorPorcentaje <= 5)
                                    //    {
                                    //        valorSectorFinan -= Convert.ToDouble(obligacion.SaldoObligacion)*1000;
                                    //        excluirVivienda = true; 
                                    //    }
                                    //}




                                    LineasCreditoObligacionDto lineasCredito = new LineasCreditoObligacionDto();

                                    lineasCredito.OLN_LINEA_CREDITO = obligacion.LineaCredito + " - " + obligacion.ModalidadCredito;
                                    lineasCredito.OLN_SALDO_OBLIGACION = obligacion.SaldoObligacion == "" ? 0 : decimal.Parse(obligacion.SaldoObligacion);
                                    lineasCredito.OLN_VALOR_CUOTA = obligacion.ValorCuota == "0" ? (decimal)valorCuotaReclasificacion/1000 : decimal.Parse(obligacion.ValorCuota);
                                    lineasCredito.OLN_ESTADO = obligacion.EstadoObligacion;
                                    lineasCredito.OLN_CUOTAS_PACTADAS = obligacion.NumeroCuotasPactadas == "" ? 0 : int.Parse(obligacion.NumeroCuotasPactadas);
                                    lineasCredito.OLN_CUOTAS_CANCELADAS = obligacion.CuotasCanceladas == "" ? 0 : int.Parse(obligacion.CuotasCanceladas);
                                    lineasCredito.OLN_CUOTAS_RESTANTES = (obligacion.CuotasCanceladas == "" || obligacion.NumeroCuotasPactadas == "") ? 0 : int.Parse(obligacion.NumeroCuotasPactadas) - int.Parse(obligacion.CuotasCanceladas);
                                    lineasCredito.OLN_NUMERO_DOCUMENTO = numeroDocumento;
                                    lineasCredito.OLN_CALIDAD = obligacion.Calidad;
                                     await _creditoRepository.InsertarLineaCreditoObligacion(lineasCredito);
                                }
                            }
                        }
                    }
                    var reportParameters = ParametrosEstaticosTransUnion(data).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();

                    if (reportParam == null)
                    {
                        reportParam = new Microsoft.Reporting.NETCore.LocalReport();
                    }

                    if (ParametrosDataSet(aplicante, reportParam).Result.Item1 == null)
                    {
                        // Aquí, crea e inicializa el DataSet si es nulo
                        var InfoEndeudaSectorRealAlDia = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealAlDia", new List<object>());
                        var InfoEndeudaFinancieroAlDia = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaFinancieroAlDia", new List<object>());
                        var InfoEndeudaFinancieroExtinguidas = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaFinancieroExtinguidas", new List<object>());
                        var InfoEndeudaSectorRealExtinguidas = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealExtinguidas", new List<object>());
                        var InfoCarteras = new Microsoft.Reporting.NETCore.ReportDataSource("InfoCarteras", new List<object>());
                        var InfoHuellas = new Microsoft.Reporting.NETCore.ReportDataSource("InfoHuellas", new List<object>());
                        reportParam.DataSources.Add(InfoEndeudaSectorRealAlDia);
                        reportParam.DataSources.Add(InfoEndeudaFinancieroAlDia);
                        reportParam.DataSources.Add(InfoEndeudaFinancieroExtinguidas);
                        reportParam.DataSources.Add(InfoEndeudaSectorRealExtinguidas);
                        reportParam.DataSources.Add(InfoCarteras);
                        reportParam.DataSources.Add(InfoHuellas);
                    }
                    reportParam.SetParameters(reportParameters);

                    fechaPermanenciaVigente = ParametrosDataSet(aplicante, reportParam).Result.Item2;
                   
                    return (reportParam, valorSectorReal, valorSectorFinan, riesgoMinimo, estadoDesicion, descripcionDesicion, totalMoraObligacion, fechaPermanenciaVigente, sinViviendaCompraCartera);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Ocurrio un error con la respuesta de TU, no se puede procesar el reporte.: " + e.Message);
                }
            }
            else
            {
                reportParam = new Microsoft.Reporting.NETCore.LocalReport();

                fechaPermanenciaVigente = ParametrosDataSet(aplicante, reportParam).Result.Item2;

                return (reportParam, valorSectorReal, valorSectorFinan, riesgoMinimo, estadoDesicion, descripcionDesicion, totalMoraObligacion, fechaPermanenciaVigente, sinViviendaCompraCartera);
            }
        }

        public async Task<Dictionary<string, string>> ParametrosEstaticosTransUnion(ResponseGeneralTu.DCResponse data)
        {
            var aplicante = data.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault().Applicants.Applicant;
            var aplicanteData = data.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault().ApplicationData;
            var tasaSura = _carteraRepository.ObtenerParamGeneralesCreditoYCartera().Result.PRM_TASA_USURA;
            List<string> TiposCreFinExcluidos = new List<string>
                {
                    "LIBZ", "LISE", "LIBR", "LBAG", "LADS", "LBCL", "LBCO", "LBCC",
                    "LCON", "LICN", "LBDE", "LBED", "LBEE", "LBEM", "LAAJ", "LBFS",
                    "LBIN", "LBLD", "LBLI", "LBMC", "LBME", "LBNO", "LMOF", "LBOR",
                    "LBPD", "LBRC", "LBRM", "LBRT", "LBSA", "LDSF", "LBSM", "LBVH",
                    "LBMB", "LBVI", "LVIV"
                };

            List<string> TiposModalidadCreFinExcluidos = new List<string>
                {
                    "LIBZ", "LISE", "LIBR", "LBAG", "LADS", "LBCL", "LBCO", "LBCC",
                    "LCON", "LICN", "LBDE", "LBED", "LBEE", "LBEM", "LAAJ", "LBFS",
                    "LBIN", "LBLD", "LBLI", "LBMC", "LBME", "LBNO", "LMOF", "LBOR",
                    "LBPD", "LBRC", "LBRM", "LBRT", "LBSA", "LDSF", "LBSM", "LBVH",
                    "LBMB", "LBVI", "LVIV"
                };
            List<string> listaLineaRecalcular = new List<string> { "ANOM" };
            List<string> listaDeCalidadCodeudore = new List<string> { "DSOL", "CODE" };
            List<double> listaLineaRecalcularValor = new List<double>();
            List<double> LstModaliaddSectorFinanExclu = new List<double>();
            var numeroDocumento = data.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants.Applicant.IdNumber;
            var simulacionInformacion = await this._creditoRepository.ObtenerSimulacionPersonales(numeroDocumento);


            List<double> LstSectorFinanExclu = new List<double>();
            List<double> LstSectorFinanExcluSaldo = new List<double>();
            List<double> LstSectorFinanExcluParticionDeuda = new List<double>();
            List<double> LstModaliaddSectorCodeExclu = new List<double>();
            var datosGenerales = await this._carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();


            ResponseGeneralTu.Registro tarjetaCredito = new ResponseGeneralTu.Registro();
            ResponseGeneralTu.Registro sectorReal = new ResponseGeneralTu.Registro();
            ResponseGeneralTu.Registro subTotal = new ResponseGeneralTu.Registro();
            ResponseGeneralTu.Registro sectorFinanciero = new ResponseGeneralTu.Registro();
            List<ResponseGeneralTu.Registro> SectorFinanAlDia = new List<ResponseGeneralTu.Registro>();
            ResponseGeneralTu.Registro sectorSolidario = new ResponseGeneralTu.Registro();
            ResponseGeneralTu.Registro sectorSolidarioCodeudores = new ResponseGeneralTu.Registro();
            double valorSectorSolidarioCodeudores = 0;
            double valorSectorSolidarioPrincipal = 0;

            double tasaPeriodica = Math.Pow(1 + Convert.ToDouble(tasaSura / 100), 1.0 / 12.0) - 1;
            double numeroDePagos = (double)datosGenerales.Data.MesesDiferir;

            if (aplicante.DSOnlineBureauCOLData != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?.Where(x => x.PaqueteInformacion.Equals("Tarjetas De Credito")).FirstOrDefault() != null)
                {
                    tarjetaCredito = aplicante.DSOnlineBureauCOLData.Response.Tercero.Consolidado.ResumenPrincipal.Registro.Where(x => x.PaqueteInformacion.Equals("Tarjetas De Credito")).FirstOrDefault();
                }
                if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?.Where(x => x.PaqueteInformacion.Equals("Sector Real")).FirstOrDefault() != null)
                {
                    sectorReal = aplicante.DSOnlineBureauCOLData.Response.Tercero.Consolidado.ResumenPrincipal.Registro.Where(x => x.PaqueteInformacion.Equals("Sector Real")).FirstOrDefault();
                }
                if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?.Where(x => x.PaqueteInformacion.Equals("Sector Financiero")).FirstOrDefault() != null)
                {
                    sectorFinanciero = aplicante.DSOnlineBureauCOLData.Response.Tercero.Consolidado.ResumenPrincipal.Registro.Where(x => x.PaqueteInformacion.Equals("Sector Financiero")).FirstOrDefault();
                }
                if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro?.Where(x => x.PaqueteInformacion.Equals("Sector Solidario")).FirstOrDefault() != null)
                {
                    sectorSolidario = aplicante.DSOnlineBureauCOLData.Response.Tercero.Consolidado.ResumenPrincipal.Registro.Where(x => x.PaqueteInformacion.Equals("Sector Solidario")).FirstOrDefault();
                }
                if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenPrincipal?.Registro.Where(x => x.PaqueteInformacion.Equals("Subtotal Principal")).FirstOrDefault() != null)
                {
                    subTotal = aplicante.DSOnlineBureauCOLData.Response.Tercero.Consolidado.ResumenPrincipal.Registro.Where(x => x.PaqueteInformacion.Equals("Subtotal Principal")).FirstOrDefault();
                }
                if (aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenDiferentePrincipal?.Registro?.Where(x => x.PaqueteInformacion.Equals("Subtotal Codeudores")) != null)
                {
                    sectorSolidarioCodeudores = aplicante.DSOnlineBureauCOLData.Response?.Tercero?.Consolidado?.ResumenDiferentePrincipal?.Registro?.Where(x => x.PaqueteInformacion.Equals("Subtotal Codeudores")).FirstOrDefault();
                }






            }


            #region codigo para el sector financiero
            if (tarjetaCredito.CuotaObligacionesDia == "0")
            {
                int totalCuota = 0;
                int saldo = 0;
                int pago = 0;


                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia != null)
                {


                    if (tarjetaCredito != null)
                    {
                        saldo = saldo + Convert.ToInt32(tarjetaCredito.TotalSaldo);
                        pago = Convert.ToInt32(Convert.ToInt32(tarjetaCredito.TotalSaldo) * tasaPeriodica / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos)));
                        tarjetaCredito.CuotaObligacionesDia = pago.ToString();

                    }
                    if (sectorReal != null)
                    {
                        saldo = saldo + Convert.ToInt32(sectorReal.SaldoObligacionesDia);
                        pago = pago + Convert.ToInt32(sectorReal.CuotaObligacionesDia);
                    }

                    if (sectorFinanciero != null)
                    {
                        saldo = saldo + Convert.ToInt32(sectorFinanciero.SaldoObligacionesDia);
                        pago = pago + Convert.ToInt32(sectorFinanciero.CuotaObligacionesDia);
                    }

                    subTotal.TotalSaldo = saldo.ToString();
                    subTotal.CuotaObligacionesDia = pago.ToString();

                    // Calcular el pago utilizando la fórmula de pago

                    var re = pago;
                }

            }

            double cuotaSectorFinanciero = sectorFinanciero.CuotaObligacionesDia != null ? double.Parse(sectorFinanciero.CuotaObligacionesDia) : 0;

            #endregion

            #region Tarjeta de credito
            //calcular tarjetas de crdito 
            var obligacionesGenerales = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorFinancieroAlDia?.Obligacion.Where(item => "TCR" == item.LineaCredito);
            double totalObligacionesTarjeta = 0;
            if (obligacionesGenerales != null)
            {
                foreach (var obligacion in obligacionesGenerales)
                {
                    if (obligacion.ValorCuota == "")
                    {
                        obligacion.ValorCuota = "0";
                    }

                    double saldoObligacion;
                    if (double.TryParse(obligacion.SaldoObligacion, out saldoObligacion) && obligacion.ValorCuota == "0" && saldoObligacion > 0)
                    {
                        double SaldoCero = saldoObligacion;

                        double numeroCuotasPactadas = 0;
                        double numeroCuotasCanceladas = 0;
                        double numeroCuotasRestantes = 0;
                        double mesesDiferir = 0;
                        if (obligacion.LineaCredito == "TCR")
                        {
                            numeroCuotasPactadas = obligacion.NumeroCuotasPactadas != "" && obligacion.NumeroCuotasPactadas != null ? double.Parse(obligacion.NumeroCuotasPactadas) : 0;
                            numeroCuotasCanceladas = obligacion.CuotasCanceladas != "" && obligacion.CuotasCanceladas != null ? double.Parse(obligacion.CuotasCanceladas) : 0;
                            numeroCuotasRestantes = numeroCuotasPactadas - numeroCuotasCanceladas;
                        }

                        mesesDiferir = numeroCuotasRestantes == 0 ? datosGenerales.Data.MesesDiferir : numeroCuotasRestantes;

                        if (obligacion.LineaCredito == "TCR")
                        {
                            double calculoTarjeta = (SaldoCero * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -(mesesDiferir)));

                            if (double.IsInfinity(calculoTarjeta))
                            {
                                calculoTarjeta = 0;
                            }

                            totalObligacionesTarjeta += calculoTarjeta;
                        }
                    }
                    else if (obligacion.LineaCredito == "TCR")
                    {
                        totalObligacionesTarjeta += double.Parse(obligacion.ValorCuota);
                    }
                }

                if (double.IsInfinity(totalObligacionesTarjeta))
                {
                    totalObligacionesTarjeta = 0;
                }
            }
            #endregion


            #region Sector financiero

            var obligacionesSectorFinanciero = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorFinancieroAlDia?.Obligacion.Where(item => !TiposCreFinExcluidos.Contains(item.LineaCredito)).Where(item => !TiposModalidadCreFinExcluidos.Contains(item.ModalidadCredito));

            double totalFinanciero = 0;
            double totalCodeudores = 0;


            if (obligacionesSectorFinanciero != null)
            {
                foreach (var obligacion in obligacionesSectorFinanciero)
                {

                    if (obligacion.ValorCuota == "")
                    {
                        obligacion.ValorCuota = "0";
                    }

                    double saldoObligacion;

                    double numeroCuotasPactadas = 0;
                    double numeroCuotasCanceladas = 0;
                    double numeroCuotasRestantes = 0;
                    double mesesDiferir = 0;

                    if (obligacion.LineaCredito != "TCR")
                    {
                        numeroCuotasPactadas = obligacion.NumeroCuotasPactadas != "" && obligacion.NumeroCuotasPactadas != null ? double.Parse(obligacion.NumeroCuotasPactadas) : 0;
                        numeroCuotasCanceladas = obligacion.CuotasCanceladas != "" && obligacion.CuotasCanceladas != null ? double.Parse(obligacion.CuotasCanceladas) : 0;
                        numeroCuotasRestantes = numeroCuotasPactadas - numeroCuotasCanceladas;
                    }

                    mesesDiferir = numeroCuotasRestantes == 0 ? datosGenerales.Data.MesesDiferir : numeroCuotasRestantes;

                    if (double.TryParse(obligacion.SaldoObligacion, out saldoObligacion) && obligacion.ValorCuota == "0" && saldoObligacion > 0)
                    {
                        double SaldoCero = saldoObligacion;


                        if ((obligacion.LineaCredito == "VIVI" || obligacion.LineaCredito == "VEHI") && !simulacionInformacion.SDP_COMPRA_CARTERA)
                        {
                            if (obligacion.Calidad == "CODE" || obligacion.Calidad == "DSOL")
                            {
                                totalCodeudores += ((SaldoCero / 2) * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -datosGenerales.Data.MesesDiferir));
                            }
                            else if (obligacion.Calidad == "PRIN")
                            {
                                totalFinanciero += ((SaldoCero) * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -datosGenerales.Data.MesesDiferir));
                            }
                        }
                        else if (obligacion.LineaCredito == "LFIN" || obligacion.LineaCredito == "ANOM")
                        {
                            totalFinanciero += ((SaldoCero) * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -datosGenerales.Data.MesesDiferir));
                        }
                        else if (obligacion.LineaCredito == "EDUC" && obligacion.Calidad == "PRIN")
                        {
                            totalFinanciero += ((SaldoCero) * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -datosGenerales.Data.MesesDiferir));
                        }
                    }
                    else if (double.TryParse(obligacion.SaldoObligacion, out saldoObligacion) && double.Parse(obligacion.ValorCuota) > 0)
                    {
                        if ((obligacion.LineaCredito == "VIVI" || obligacion.LineaCredito == "VEHI") && !simulacionInformacion.SDP_COMPRA_CARTERA)
                        {
                            if (obligacion.Calidad == "CODE" || obligacion.Calidad == "DSOL")
                            {
                                totalCodeudores += ((double.Parse(obligacion.ValorCuota) / 2) * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -mesesDiferir));
                            }
                            else if (obligacion.Calidad == "PRIN" && obligacion.LineaCredito != "VIVI")
                            {
                                totalFinanciero += ((double.Parse(obligacion.ValorCuota)) * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -mesesDiferir));
                            }
                            else if (obligacion.LineaCredito == "VIVI" && obligacion.Calidad == "PRIN")
                            {
                                totalFinanciero += double.Parse(obligacion.ValorCuota);
                            }
                        }
                        else if (obligacion.LineaCredito == "EDUC" && (obligacion.Calidad == "CODE" || obligacion.Calidad == "DSOL"))
                        {
                            totalCodeudores += (double.Parse(obligacion.ValorCuota));

                        }
                        else if (obligacion.LineaCredito != "TCR" && obligacion.LineaCredito != "EDUC")
                        {
                            totalFinanciero += double.Parse(obligacion.ValorCuota);
                        }
                    }
                }
            }
            #endregion



            #region validaciones para valores null

            if (sectorFinanciero.NumeroObligaciones == null)
            {
                sectorFinanciero.NumeroObligacionesDia = "0";
                sectorFinanciero.TotalSaldo = "0";
                sectorFinanciero.ParticipacionDeuda = "0";
                sectorFinanciero.CantidadObligacionesMora = "0";
                sectorFinanciero.SaldoObligacionesMora = "0";
                sectorFinanciero.CuotaObligacionesMora = "0";
                sectorFinanciero.ValorMora = "0";
                sectorFinanciero.CuotaObligacionesDia = "0";
            }


            if (sectorReal.NumeroObligaciones == null)
            {
                sectorReal.NumeroObligacionesDia = "0";
                sectorReal.TotalSaldo = "0";
                sectorReal.ParticipacionDeuda = "0";
                sectorReal.CantidadObligacionesMora = "0";
                sectorReal.SaldoObligacionesMora = "0";
                sectorReal.CuotaObligacionesMora = "0";
                sectorReal.ValorMora = "0";
                sectorReal.CuotaObligacionesDia = "0";
            }

            if (sectorSolidario.NumeroObligaciones == null)
            {
                sectorSolidario.NumeroObligacionesDia = "0";
                sectorSolidario.TotalSaldo = "0";
                sectorSolidario.ParticipacionDeuda = "0";
                sectorSolidario.CantidadObligacionesMora = "0";
                sectorSolidario.SaldoObligacionesMora = "0";
                sectorSolidario.CuotaObligacionesMora = "0";
                sectorSolidario.ValorMora = "0";
                sectorSolidario.CuotaObligacionesDia = "0";
            }

            if (tarjetaCredito.NumeroObligacionesDia == null)
            {
                tarjetaCredito.NumeroObligacionesDia = "0";
                tarjetaCredito.TotalSaldo = "0";
                tarjetaCredito.ParticipacionDeuda = "0";
                tarjetaCredito.CantidadObligacionesMora = "0";
                tarjetaCredito.SaldoObligacionesMora = "0";
                tarjetaCredito.CuotaObligacionesMora = "0";
                tarjetaCredito.ValorMora = "0";
                tarjetaCredito.CuotaObligacionesDia = "0";
            }

            if (sectorSolidarioCodeudores.NumeroObligacionesDia == null)
            {
                sectorSolidarioCodeudores.NumeroObligacionesDia = "0";
                sectorSolidarioCodeudores.TotalSaldo = "0";
                sectorSolidarioCodeudores.ParticipacionDeuda = "0";
                sectorSolidarioCodeudores.CantidadObligacionesMora = "0";
                sectorSolidarioCodeudores.SaldoObligacionesMora = "0";
                sectorSolidarioCodeudores.CuotaObligacionesMora = "0";
                sectorSolidarioCodeudores.ValorMora = "0";
                sectorSolidarioCodeudores.CuotaObligacionesDia = "0";
            }
            #endregion


            #region Inicializar Valores totales
            string totalNumeroObligacionesDia = "0";
            string totalParticionDeuda = "0";
            string subtotalTotalSaldo = "0";
            string totalCuotaObligacionesDia = "0";
            string totalCantidadObligacionesMora = "0";
            string subtotalTotalSaldoMora = "0";
            string totalCuotaObligacionesDiaMora = "0";
            string totalValorMora = "0";

            #endregion


            #region variables sector financiero
            int sectorFinancieroNumeroObligacionesDia = (sectorFinanciero.NumeroObligacionesDia != null && sectorFinanciero.NumeroObligacionesDia != "") ? int.Parse(sectorFinanciero.NumeroObligacionesDia) : 0;
            int sectorFinancieroParticipacionDeuda = (sectorFinanciero.ParticipacionDeuda != null && sectorFinanciero.ParticipacionDeuda != "") ? int.Parse(sectorFinanciero.ParticipacionDeuda) : 0;
            int sectorFinancieroTotalSaldo = (sectorFinanciero.TotalSaldo != null && sectorFinanciero.TotalSaldo != "") ? int.Parse(sectorFinanciero.TotalSaldo) : 0;
            double sectorFinancieroTotalCuotaObligacionesDia = totalFinanciero;
            double sectorFinancieroCuota = sectorFinancieroTotalCuotaObligacionesDia;
            double totalValorCuotaSectorFinanciero = sectorFinancieroCuota == 0 ? sectorFinancieroTotalCuotaObligacionesDia : sectorFinancieroCuota;

            #region varibles mora SF
            int sectorFinancieroTotalCantidadObligacionesMora = (sectorFinanciero.CantidadObligacionesMora != null && sectorFinanciero.CantidadObligacionesMora != "") ? int.Parse(sectorFinanciero.CantidadObligacionesMora) : 0;
            int sectorFinancieroTotalSaldoObligacionesMora = (sectorFinanciero.SaldoObligacionesMora != null && sectorFinanciero.SaldoObligacionesMora != "") ? int.Parse(sectorFinanciero.SaldoObligacionesMora) : 0;
            double sectorFinancieroTotalCuotaObligacionesMora = (sectorFinanciero.SaldoObligacionesMora != null && sectorFinanciero.SaldoObligacionesMora != "") ? Math.Truncate(((double)sectorFinancieroTotalSaldoObligacionesMora * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double sectorFinancieroCuotaMora = (sectorFinanciero.CuotaObligacionesMora != null && sectorFinanciero.CuotaObligacionesMora != "") ? int.Parse(sectorFinanciero.CuotaObligacionesMora) : 0;
            double totalValorCuotaSectorFinancieroMora = sectorFinancieroCuotaMora == 0 ? sectorFinancieroTotalCuotaObligacionesDia : sectorFinancieroCuotaMora;
            int sectorFinancieroTotalValorMora = (sectorFinanciero.ValorMora != null && sectorFinanciero.ValorMora != "") ? int.Parse(sectorFinanciero.ValorMora) : 0;
            #endregion
            #endregion


            #region variables sector real
            int sectorRealNumeroObligacionesDia = (sectorReal.NumeroObligacionesDia != null && sectorReal.NumeroObligacionesDia != "") ? int.Parse(sectorReal.NumeroObligacionesDia) : 0;
            int sectorRealNumeroParticipacionDeuda = (sectorReal.ParticipacionDeuda != null && sectorReal.ParticipacionDeuda != "") ? int.Parse(sectorReal.ParticipacionDeuda) : 0;
            int sectorRealNumeroTotalSaldo = (sectorReal.TotalSaldo != null && sectorReal.TotalSaldo != "") ? int.Parse(sectorReal.TotalSaldo) : 0;
            double sectorRealTotalCuotaObligacionesDia = (sectorReal.TotalSaldo != null && sectorReal.TotalSaldo != "") ? Math.Truncate((double)sectorRealNumeroTotalSaldo) : 0;
            double sectorRealNumeroCuota = (sectorReal.CuotaObligacionesDia != null && sectorReal.CuotaObligacionesDia != "" && sectorReal.CuotaObligacionesDia != "0") ? int.Parse(sectorReal.CuotaObligacionesDia) : 0;
            double totalValorCuotaSectorReal = sectorRealNumeroCuota > 0 ? sectorRealNumeroCuota : sectorRealTotalCuotaObligacionesDia;
            #region variables mora SR
            int sectorRealTotalCantidadObligacionesMora = (sectorReal.CantidadObligacionesMora != null && sectorReal.CantidadObligacionesMora != "") ? int.Parse(sectorReal.CantidadObligacionesMora) : 0;
            int sectorRealNumeroTotalSaldoObligacionesMora = (sectorReal.SaldoObligacionesMora != null && sectorReal.SaldoObligacionesMora != "") ? int.Parse(sectorReal.SaldoObligacionesMora) : 0;
            double sectorRealNumeroTotalCuotaObligacionesMora = (sectorReal.SaldoObligacionesMora != null && sectorReal.SaldoObligacionesMora != "") ? Math.Truncate(((double)sectorRealNumeroTotalSaldoObligacionesMora * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double sectorRealNumeroCuotaMora = (sectorReal.CuotaObligacionesMora != null && sectorReal.CuotaObligacionesMora != "") ? int.Parse(sectorReal.CuotaObligacionesMora) : 0;
            double totalValorCuotaSectorRealMora = sectorRealNumeroCuotaMora == 0 ? sectorRealTotalCuotaObligacionesDia : sectorFinancieroCuotaMora;
            int sectorRealNumeroTotalValorMora = (sectorReal.ValorMora != null && sectorReal.ValorMora != "") ? int.Parse(sectorReal.ValorMora) : 0;
            #endregion
            #endregion

            #region variables sector solidario
            int sectorSolidarioNumeroObligacionesDia = (sectorSolidario.NumeroObligacionesDia != null && sectorSolidario.NumeroObligacionesDia != "") ? int.Parse(sectorSolidario.NumeroObligacionesDia) : 0;
            int sectorSolidarioParticipacionDeuda = (sectorSolidario.ParticipacionDeuda != null && sectorSolidario.ParticipacionDeuda != "") ? int.Parse(sectorSolidario.ParticipacionDeuda) : 0;
            int sectorSolidarioTotalSaldo = (sectorSolidario.TotalSaldo != null && sectorSolidario.TotalSaldo != "") ? int.Parse(sectorSolidario.TotalSaldo) : 0;
            double sectorSolidarioTotalCuotaObligacionesDia = (sectorSolidario.TotalSaldo != null && sectorSolidario.TotalSaldo != "") ? Math.Truncate(((double)sectorSolidarioTotalSaldo * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double sectorSolidarioCuota = (sectorSolidario.CuotaObligacionesDia != null && sectorSolidario.CuotaObligacionesDia != "") ? int.Parse(sectorSolidario.CuotaObligacionesDia) : 0;
            double totalValorCuotaSectorSolidario = sectorSolidarioCuota == 0 ? sectorSolidarioTotalCuotaObligacionesDia : sectorSolidarioCuota;
            #region variables mora SS
            int sectorSolidarioTotalCantidadObligacionesMora = (sectorSolidario.CantidadObligacionesMora != null && sectorSolidario.CantidadObligacionesMora != "") ? int.Parse(sectorSolidario.CantidadObligacionesMora) : 0;
            int sectorSolidarioTotalSaldoObligacionesMora = (sectorSolidario.SaldoObligacionesMora != null && sectorSolidario.SaldoObligacionesMora != "") ? int.Parse(sectorSolidario.SaldoObligacionesMora) : 0;
            double sectorSolidarioTotalCuotaObligacionesMora = (sectorSolidario.SaldoObligacionesMora != null && sectorSolidario.SaldoObligacionesMora != "") ? Math.Truncate(((double)sectorSolidarioTotalSaldoObligacionesMora * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double sectorSolidarioCuotaMora = (sectorSolidario.CuotaObligacionesMora != null && sectorSolidario.CuotaObligacionesMora != "") ? int.Parse(sectorSolidario.CuotaObligacionesMora) : 0;
            double totalValorCuotaSectorSolidarioMora = sectorSolidarioCuotaMora == 0 ? sectorSolidarioTotalCuotaObligacionesDia : sectorSolidarioCuotaMora;
            int sectorSolidarioTotalValorMora = (sectorSolidario.ValorMora != null && sectorSolidario.ValorMora != "") ? int.Parse(sectorSolidario.ValorMora) : 0;
            #endregion
            #endregion

            #region variables tarjeta de credito
            int tarjetaCreditoNumeroObligacionesDia = (tarjetaCredito.NumeroObligacionesDia != null && tarjetaCredito.NumeroObligacionesDia != "") ? int.Parse(tarjetaCredito.NumeroObligacionesDia) : 0;
            int tarjetaCreditoParticipacionDeuda = (tarjetaCredito.ParticipacionDeuda != null && tarjetaCredito.ParticipacionDeuda != "") ? int.Parse(tarjetaCredito.ParticipacionDeuda) : 0;
            int tarjetaCreditoTotalSaldo = (tarjetaCredito.TotalSaldo != null && tarjetaCredito.TotalSaldo != "") ? int.Parse(tarjetaCredito.TotalSaldo) : 0;
            double tarjetaCreditoTotalCuotaObligacionesDia = ((int)totalObligacionesTarjeta);
            double tarjetaCreditoCuota = tarjetaCreditoTotalCuotaObligacionesDia;
            double totalValorCuotaTarjetaCredito = tarjetaCreditoCuota == 0 ? tarjetaCreditoTotalCuotaObligacionesDia : tarjetaCreditoCuota;
            #region variables mora TC
            int tarjetaCreditoTotalCantidadObligacionesMora = (tarjetaCredito.CantidadObligacionesMora != null && tarjetaCredito.CantidadObligacionesMora != "") ? int.Parse(tarjetaCredito.CantidadObligacionesMora) : 0;
            int tarjetaCreditoTotalSaldoObligacionesMora = (tarjetaCredito.SaldoObligacionesMora != null && tarjetaCredito.SaldoObligacionesMora != "") ? int.Parse(tarjetaCredito.SaldoObligacionesMora) : 0;
            double tarjetaCreditoTotalCuotaObligacionesMora = (tarjetaCredito.SaldoObligacionesMora != null && tarjetaCredito.SaldoObligacionesMora != "") ? Math.Truncate(((double)tarjetaCreditoTotalSaldoObligacionesMora * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double tarjetaCreditoCuotaMora = (tarjetaCredito.CuotaObligacionesMora != null && tarjetaCredito.CuotaObligacionesMora != "") ? int.Parse(tarjetaCredito.CuotaObligacionesMora) : 0;
            double totalValorCuotaTarjetaCreditoMora = tarjetaCreditoCuotaMora == 0 ? tarjetaCreditoTotalCuotaObligacionesDia : tarjetaCreditoCuotaMora;
            int tarjetaCreditoTotalValorMora = (tarjetaCredito.ValorMora != null && tarjetaCredito.ValorMora != "") ? int.Parse(tarjetaCredito.ValorMora) : 0;
            #endregion
            #endregion

            #region variables sector codeudores
            int sectorSolidarioCodeudoresNumeroObligacionesDia = (sectorSolidarioCodeudores.NumeroObligacionesDia != null && sectorSolidarioCodeudores.NumeroObligacionesDia != "") ? int.Parse(sectorSolidarioCodeudores.NumeroObligacionesDia) : 0;
            int sectorSolidarioCodeudoresParticipacionDeuda = (sectorSolidarioCodeudores.ParticipacionDeuda != null && sectorSolidarioCodeudores.ParticipacionDeuda != "") ? int.Parse(sectorSolidarioCodeudores.ParticipacionDeuda) : 0;
            int sectorSolidarioCodeudoresTotalSaldo = (sectorSolidarioCodeudores.TotalSaldo != null && sectorSolidarioCodeudores.TotalSaldo != "") ? int.Parse(sectorSolidarioCodeudores.TotalSaldo) : 0;
            double sectorSolidarioCodeudoresCuotaObligacionesDia = (sectorSolidarioCodeudores.TotalSaldo != null && sectorSolidarioCodeudores.TotalSaldo != "") ? Math.Truncate(((double)sectorSolidarioCodeudoresTotalSaldo * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double sectorSolidarioCodeudoresCuota = totalCodeudores;
            double totalValorCuotaSectorSolidarioCodeudores = sectorSolidarioCodeudoresCuota == 0 ? sectorSolidarioCodeudoresCuotaObligacionesDia : sectorSolidarioCodeudoresCuota;


            #region variables mora SC
            int sectorSolidarioCodeudoresTotalCantidadObligacionesMora = (sectorSolidarioCodeudores.CantidadObligacionesMora != null && sectorSolidarioCodeudores.CantidadObligacionesMora != "") ? int.Parse(sectorSolidarioCodeudores.CantidadObligacionesMora) : 0;
            int sectorSolidarioCodeudoresTotalSaldoObligacionesMora = (sectorSolidarioCodeudores.SaldoObligacionesMora != null && sectorSolidarioCodeudores.SaldoObligacionesMora != "") ? int.Parse(sectorSolidarioCodeudores.SaldoObligacionesMora) : 0;
            double sectorSolidarioCodeudoresCuotaObligacionesMora = (sectorSolidarioCodeudores.SaldoObligacionesMora != null && sectorSolidarioCodeudores.SaldoObligacionesMora != "") ? Math.Truncate(((double)sectorSolidarioCodeudoresTotalSaldoObligacionesMora * tasaPeriodica) / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos))) : 0;
            double sectorSolidarioCodeudoresCuotaMora = (sectorSolidarioCodeudores.CuotaObligacionesMora != null && sectorSolidarioCodeudores.CuotaObligacionesMora != "") ? int.Parse(sectorSolidarioCodeudores.CuotaObligacionesMora) : 0;
            double totalValorCuotaSectorSolidarioCodeudoresMora = sectorSolidarioCodeudoresCuotaMora == 0 ? sectorSolidarioCodeudoresCuotaObligacionesDia : sectorSolidarioCodeudoresCuotaMora;
            int sectorSolidarioCodeudoresTotalValorMora = (sectorSolidarioCodeudores.ValorMora != null && sectorSolidarioCodeudores.ValorMora != "") ? int.Parse(sectorSolidarioCodeudores.ValorMora) : 0;
            #endregion
            #endregion


            #region obligaciones excluidas 
            var obligacionesModalidadLineaCredito = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorFinancieroAlDia?.Obligacion;
            valorSectorSolidarioPrincipal = sectorSolidario != null ? Convert.ToDouble(sectorSolidario.CuotaObligacionesDia) : 0;

            if (obligacionesModalidadLineaCredito != null)
            {


                LstSectorFinanExclu = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion
                    .Where(item => !listaDeCalidadCodeudore.Contains(item.Calidad) && TiposCreFinExcluidos.Any(valor => valor == item.LineaCredito))
                    .Select(x => Convert.ToDouble(x.ValorCuota))
                    .ToList();

                //descontar del saldo las obligaciones excluidas 
                LstSectorFinanExcluSaldo = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion
                    .Where(item => TiposCreFinExcluidos.Any(valor => valor == item.LineaCredito)).Select(x => Convert.ToDouble(x.SaldoObligacion)).ToList();

                //descontar de numero de particiones de las obligaciones excluidas
                LstSectorFinanExcluParticionDeuda = aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia.Obligacion
                                                    .Where(item => !listaDeCalidadCodeudore.Contains(item.Calidad) && TiposCreFinExcluidos.Any(valor => valor == item.LineaCredito))
                                                    .Select(x =>
                                                        double.TryParse(x.ParticipacionDeuda, out double participacion) ? participacion : 0
                                                    ).ToList();
            }
            #endregion

            #region variables totales
            totalNumeroObligacionesDia = (sectorFinancieroNumeroObligacionesDia + sectorRealNumeroObligacionesDia + tarjetaCreditoNumeroObligacionesDia).ToString();
            totalParticionDeuda = (sectorFinancieroParticipacionDeuda + sectorRealNumeroParticipacionDeuda + tarjetaCreditoParticipacionDeuda - LstSectorFinanExcluParticionDeuda.Count).ToString();
            subtotalTotalSaldo = (sectorFinancieroTotalSaldo + sectorRealNumeroTotalSaldo + tarjetaCreditoTotalSaldo).ToString();
            totalCuotaObligacionesDia = (Math.Round(totalFinanciero, 0) + totalValorCuotaSectorReal + totalValorCuotaSectorSolidario + totalValorCuotaTarjetaCredito + Math.Round(totalCodeudores, 0)).ToString();
            #endregion

            #region variables totales mora
            totalCantidadObligacionesMora = (sectorFinancieroTotalCantidadObligacionesMora + sectorRealTotalCantidadObligacionesMora + sectorSolidarioTotalCantidadObligacionesMora + sectorSolidarioCodeudoresTotalCantidadObligacionesMora).ToString();
            subtotalTotalSaldoMora = (sectorFinancieroTotalSaldoObligacionesMora + sectorRealNumeroTotalSaldoObligacionesMora + sectorSolidarioTotalSaldoObligacionesMora + sectorSolidarioCodeudoresTotalSaldoObligacionesMora).ToString();
            totalCuotaObligacionesDiaMora = (totalValorCuotaSectorFinancieroMora + totalValorCuotaSectorRealMora + totalValorCuotaSectorSolidarioMora + totalValorCuotaSectorSolidarioCodeudoresMora).ToString();
            totalValorMora = (sectorFinancieroTotalValorMora + sectorRealNumeroTotalValorMora + sectorSolidarioTotalValorMora + sectorSolidarioCodeudoresTotalValorMora).ToString();
            #endregion


            TipoIdentificacion tipoIdentificacion = await _creditoRepository.ObtenerTipoIdentificacionPorId(int.Parse(aplicante.IdType));
            Response<ClienteDto> clienteDetallado = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(int.Parse(aplicante.IdType), aplicante.IdNumber);

            int codigoActividadEconomica = clienteDetallado.Data.IdActividadEconomica != 0 ? clienteDetallado.Data.IdActividadEconomica : 428;


            ActividadEconomica actividadEconomica = await _catalogsRepository.ObtenerActividadEconomica(codigoActividadEconomica);

            var tiempo = DateTime.Now;

            return new Dictionary<string, string>
                {
                    { "InfoComercialTipoId", tipoIdentificacion.TID_CODIGO },
                    { "InfoComercialNroId", aplicante.IdNumber },
                    { "InfoComercialNombres", aplicante.FirstName + " " + aplicante.SecondName + " " + aplicante.FirstLastName + " " + aplicante.SecondLastName },
                    { "InfoComercialActEconomica", actividadEconomica.ACC_DESCRIPCION },
                    { "InfoComercialEstadoDocumento", aplicante.DocumentState },
                    { "InfoComercialFechaExp", aplicante.IdExpeditionDate },
                    { "InfoComercialLugarExp", aplicante.DSOnlineBureauCOLData != null ? aplicante.DSOnlineBureauCOLData.Response.Tercero.LugarExpedicion ?? "" : "" },
                    { "InfoComercialRangoEdad", aplicante.DSOnlineBureauCOLData != null ? aplicante.DSOnlineBureauCOLData.Response.Tercero.RangoEdad ?? "" : "" },
                    { "InfoComercialFecha", tiempo.ToString("dd/MM/yyyy") },
                    { "InfoComercialHora",  tiempo.ToString("HH:mm:ss") },
                    { "InfoComercialNomUsuario",aplicanteData.User },
                     { "InfoComercialNroInforme", aplicante.DSOnlineBureauCOLData != null ? aplicante.DSOnlineBureauCOLData.Response.Tercero.NumeroInforme ?? "" : "" },

                    // Resumen Obligatorio INICIO TARJETA DE CREDITO
                    { "ObliTotalesTcCant", tarjetaCredito != null ? tarjetaCredito.NumeroObligaciones : "" },
                    { "ObliTotalesTcPade", tarjetaCredito != null ? tarjetaCredito.ParticipacionDeuda : "" },
                    { "ObliTotalesTcSaldoTotal", tarjetaCredito != null ? tarjetaCredito.TotalSaldo : "" },
                    { "ObliDiaTcSaldoTotal", tarjetaCredito != null ? tarjetaCredito.TotalSaldo : "" },
                    { "ObliMoraTcCant", tarjetaCredito != null ? tarjetaCredito.CantidadObligacionesMora : "" },
                    { "ObliMoraTcSaldoTotal", tarjetaCredito != null ? tarjetaCredito.SaldoObligacionesMora : "" },
                    { "ObliDiaTcCuota", tarjetaCredito != null ? tarjetaCredito.CuotaObligacionesDia : "" },
                    { "ObliDiaTcCant", tarjetaCredito != null ? tarjetaCredito.NumeroObligacionesDia : "" },
                    { "ObliMoraTcVlrMora", tarjetaCredito != null ? tarjetaCredito.ValorMora : "" },
                    // Resumen Obligatorio FIN TARJETA DE CREDITO

                    // INICIO SECTOR REAL 'Sc'
                    { "ObliTotalesScCant", sectorReal != null ? sectorRealNumeroObligacionesDia.ToString() : "" },
                    { "ObliTotalesScSaldoTotal", sectorReal != null ? sectorReal.TotalSaldo : "" },
                    { "ObliTotalesScPade", sectorReal != null ? sectorReal.ParticipacionDeuda : "" },
                    { "ObliDiaScCant", sectorReal != null ? sectorReal.NumeroObligacionesDia : "" },
                    { "ObliDiaScSaldoTotal", sectorReal != null ? sectorReal.TotalSaldo : "" },
                    { "ObliDiaScCuota", sectorReal != null ? totalValorCuotaSectorReal.ToString() : "" },
                    { "ObliMoraScCant", sectorReal != null ? sectorReal.CantidadObligacionesMora : "" },
                    { "ObliMoraScSaldoTotal", sectorReal != null ? sectorReal.SaldoObligacionesMora : "" },
                    { "ObliMoraScCuota", sectorReal != null ? sectorReal.CuotaObligacionesMora : "" },
                    { "ObliMoraScVlrMora", sectorReal != null ? sectorReal.ValorMora : "" },
                // FIN SECTOR REAL 'Sc'

                      // INICIO SECTOR Financiero 'Sf' (Convert.ToInt32(tarjetaCredito.TotalSaldo) * tasaPeriodica / (1 - Math.Pow(1 + tasaPeriodica, -numeroDePagos)))
                    { "ObliTotalesSfCant", sectorFinanciero != null ? (sectorFinancieroNumeroObligacionesDia).ToString(): "" },
                    { "ObliTotalesSfSaldoTotal", sectorFinanciero != null ? (sectorFinancieroTotalSaldo).ToString() : "" },
                    { "ObliTotalesSfPade", sectorFinanciero != null ? (sectorFinancieroParticipacionDeuda).ToString() : "" },
                    { "ObliDiaSfCant", sectorFinanciero != null ? (sectorFinancieroNumeroObligacionesDia).ToString() : "" },
                    { "ObliDiaSfSaldoTotal", sectorFinanciero != null ? (sectorFinancieroTotalSaldo).ToString() : "" },
                    { "ObliDiaSfCuota", sectorFinanciero != null ? sectorFinanciero.CuotaObligacionesDia : "" },
                    { "ObliMoraSfCant", sectorFinanciero != null ? sectorFinanciero.CantidadObligacionesMora : "" },
                    { "ObliMoraSfSaldoTotal", sectorFinanciero != null ? sectorFinanciero.SaldoObligacionesMora : "" },
                    { "ObliMoraSfCuota", sectorFinanciero != null ? sectorFinanciero.CuotaObligacionesMora : "" },
                    { "ObliMoraSfVlrMora", sectorFinanciero != null ? sectorFinanciero.ValorMora : "" },
                // FIN SECTOR Financiero 'Sf'

                 // INICIO SECTOR Solidario 'Ss'
                    { "ObliTotalesSsCant", sectorSolidario != null ? sectorSolidarioCodeudoresNumeroObligacionesDia.ToString() : "" },
                    { "ObliTotalesSsSaldoTotal", sectorSolidario != null ? sectorSolidarioCodeudoresTotalSaldo.ToString() : "" },
                    { "ObliTotalesSsPade", sectorSolidario != null ? sectorSolidario.ParticipacionDeuda : "" },
                    { "ObliDiaSsCant", sectorSolidario != null ? (int.Parse(sectorSolidario.NumeroObligacionesDia) + sectorSolidarioCodeudoresNumeroObligacionesDia).ToString() : "" },
                    { "ObliDiaSsSaldoTotal", sectorSolidario != null ? (sectorSolidarioCodeudoresTotalSaldo).ToString("0") : "" },
                    { "ObliDiaSsCuota", sectorSolidario != null ? (valorSectorSolidarioPrincipal + sectorSolidarioCodeudoresCuota).ToString("0") : "" },
                    { "ObliMoraSsCant", sectorSolidario != null ? sectorSolidario.CantidadObligacionesMora : "" },
                    { "ObliMoraSsSaldoTotal", sectorSolidario != null ? sectorSolidario.SaldoObligacionesMora : "" },
                    { "ObliMoraSsCuota", sectorSolidario != null ? sectorSolidario.CuotaObligacionesMora : "" },
                    { "ObliMoraSsVlrMora", sectorSolidario != null ? sectorSolidario.ValorMora : "" },
                // FIN SECTOR Solidario 'Ss'

                // INICIO SUBTOTAL 'Sp'
                    { "ObliTotalesSpCant", subTotal != null ? totalNumeroObligacionesDia : "" },
                    { "ObliTotalesSpSaldoTotal", subTotal != null ?  subtotalTotalSaldo.ToString() : "" },
                    { "ObliTotalesSpPade", subTotal != null ? totalParticionDeuda.ToString() : "" },
                    { "ObliDiaSpCant", subTotal != null ? totalNumeroObligacionesDia.ToString() : "" },
                    { "ObliDiaSpCantSaldoTotal", subTotal != null ? subtotalTotalSaldo.ToString() : "" },
                    { "ObliDiaSpCuota", subTotal != null ? subTotal.CuotaObligacionesDia.ToString() : "" },
                    { "ObliMoraSpCant", subTotal != null ? subTotal.CantidadObligacionesMora : "" },
                    { "ObliMoraSpSldoTotal", subTotal != null ? subTotal.SaldoObligacionesMora : "" },
                    { "ObliMoraSpCuota", subTotal != null ? subTotal.CuotaObligacionesMora : "" },
                    { "ObliMoraSpValorMora", subTotal != null ? subTotal.ValorMora : "" },
                // Fin SubTotal 'sp'

                //INICIO Listado Huellas count
                { "BusquedaHuellas", aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.HuellaConsulta?.Consulta?.Count().ToString() } };

            //Resumen de totales de obligaciones
        }

        public async Task<(Microsoft.Reporting.NETCore.LocalReport, string, double)> ParametrosDataSet(ResponseGeneralTu.Applicant aplicante, Microsoft.Reporting.NETCore.LocalReport reportParam)
        {
            if (aplicante.DSOnlineBureauCOLData == null)
            {
                return (null, "", 0);
            }

            double valorTotalSectorRealServicio = 0; 
            string fechaPermanecia = "";

            #region CuentasExtinguidas
            // Información de cuentas
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.CuentasVigentes != null)
            {
                var projectedDataInfoCuentas = aplicante.DSOnlineBureauCOLData.Response.Tercero.CuentasVigentes.Obligacion
                .Select(d => new
                {
                    FECHA_CORTE = d.FechaCorte,
                    TIPO_CONTRATO = d.TipoContrato,
                    No_Cuenta = d.NumeroObligacion,
                    ESTADO = d.EstadoObligacion,
                    TIPO_ENT = d.TipoEntidad,
                    ENTIDAD = d.NombreEntidad,
                    CIUDAD = d.Ciudad,
                    SUCURSAL = d.Sucursal,
                    FECHA_APERTURA = d.FechaApertura,
                    CUPO_SOBREGIRO = "N.A",
                    DIAS_AUTOR = "N.A",
                    FECHA_PERMANENCIA = string.IsNullOrWhiteSpace(d.FechaPermanencia) ? "-" : d.FechaPermanencia,
                    CHEQ_DEVUELTOS_ULTIMO_MES = string.IsNullOrWhiteSpace(d.ChequesDevueltos) ? "N.A" : d.ChequesDevueltos
                })
                .ToList();

                var InfoCarteraDataSet = projectedDataInfoCuentas != null ? new ReportDataSource("InfoCarteras", projectedDataInfoCuentas) : new ReportDataSource("InfoCarteras", new List<object>());
                InfoCarteraDataSet.Value = projectedDataInfoCuentas;
                reportParam.DataSources.Add(InfoCarteraDataSet);
            }
            else
            {
                var InfoCarteraDataSet = new Microsoft.Reporting.NETCore.ReportDataSource("InfoCarteras", new List<object>());
                reportParam.DataSources.Add(InfoCarteraDataSet);
            }
            #endregion

            #region Sector Financiero AlDia
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroAlDia != null)
            {
                // Sector financiero al día
                var projectedDataFinancieroAlDia = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorFinancieroAlDia?.Obligacion?
                    .Select(d => new
                    {
                        FECHA_CORTE = d.FechaCorte,
                        TIPO_CONT = d.TipoContrato,
                        PADE = d.ParticipacionDeuda,
                        MODA = d.ModalidadCredito,
                        LCRE = d.LineaCredito,
                        NRO_OBLIG = d.NumeroObligacion,
                        EST_CONTR = d.EstadoContrato,
                        TIPO_ENT = d.TipoEntidad,
                        CLF = d.Calificacion,
                        NOMBRE_ENT = d.NombreEntidad,
                        ORIGEN_CARTERA = d.EntidadOriginadoraCartera,
                        CIUDAD = d.Ciudad,
                        SUCURSAL = d.Sucursal,
                        CAL = d.Calidad,
                        EST_TITU = d.EstadoTitular,
                        MRC = d.MarcaTarjeta,
                        CLS = d.ClaseTarjeta,
                        TIPO_GAR = d.TipoGarantia,
                        COB_GAR = d.CubrimientoGarantia,
                        F_INICIO = d.FechaApertura,
                        F_TERM = d.FechaTerminacion,
                        NRO_CUOTAS = d.NumeroCuotasMora,
                        PAC = d.NumeroCuotasPactadas,
                        PAG = d.CuotasCanceladas,
                        MOR = "-",
                        PER = d.Periodicidad,
                        CUPO_APR_VLR_INI = d.ValorInicial,
                        CUPO_UTIL_SALDO_CORTE = d.SaldoObligacion,
                        PAGO_MIN_VLR_CUOTA = d.ValorCuota,
                        SIT_OBLIG = d.EstadoObligacion,
                        VALOR_MORA = d.ValorMora,
                        NATU_REES = d.NaturalezaReestructuracion,
                        REES = d.Reestructurado,
                        NO_REE = d.NumeroReestructuraciones,
                        MOR_MAX = d.MoraMaxima,
                        TIP_PAG = d.TipoPago,
                        MOD_EXT = d.ModoExtincion,
                        F_PAGO_F_EXTIN = "-",
                        F_PERMAN = d.FechaPermanencia,
                        COMPORTAMIENTOS = d.Comportamientos,
                        VALOR_CARGO_FIJO = d.ValorCargoFijo
                    })
                    .ToList();

                foreach (var dato in projectedDataFinancieroAlDia)
                {
                    if (!string.IsNullOrEmpty(dato.F_PERMAN))
                    {
                        fechaPermanecia = "NO VIABLE: Evaluación de políticas de riesgo (Presenta fecha de permanencia)";
                        break;
                    }
                }

                var InfoEndeudamientoFinancieroAlDia = projectedDataFinancieroAlDia != null ? new ReportDataSource("InfoEndeudaFinancieroAlDia", projectedDataFinancieroAlDia) : new ReportDataSource("InfoEndeudaFinancieroAlDia", new List<object>());
                if (projectedDataFinancieroAlDia != null)
                {
                    InfoEndeudamientoFinancieroAlDia.Value = projectedDataFinancieroAlDia;
                }
                reportParam.DataSources.Add(InfoEndeudamientoFinancieroAlDia);
            }
            else
            {
                var InfoEndeudamientoFinancieroAlDia = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaFinancieroAlDia", new List<object>());
                reportParam.DataSources.Add(InfoEndeudamientoFinancieroAlDia);
            }
            #endregion

            #region SectorFinancieroExtinguidas
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorFinancieroExtinguidas != null)
            {
                // Sector financiero extinguídas
                var DataSetInfoEndeudamientoFinancieroExtinguidas = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorFinancieroExtinguidas?.Obligacion?
                    .Select(d => new
                    {
                        FECHA_CORTE = d.FechaCorte,
                        TIPO_CONT = d.TipoContrato,
                        PADE = d.ParticipacionDeuda,
                        MODA = d.ModalidadCredito,
                        LCRE = d.LineaCredito,
                        NRO_OBLIG = d.NumeroObligacion,
                        EST_CONTR = d.EstadoContrato,
                        TIPO_ENT = d.TipoEntidad,
                        CLF = d.Calificacion,
                        NOMBRE_ENT = d.NombreEntidad,
                        ORIGEN_CARTERA = d.EntidadOriginadoraCartera,
                        CIUDAD = d.Ciudad,
                        SUCURSAL = d.Sucursal,
                        CAL = d.Calidad,
                        EST_TITU = d.EstadoTitular,
                        MRC = d.MarcaTarjeta,
                        CLS = d.ClaseTarjeta,
                        TIPO_GAR = d.TipoGarantia,
                        COB_GAR = d.CubrimientoGarantia,
                        F_INICIO = d.FechaApertura,
                        F_TERM = d.FechaTerminacion,
                        NRO_CUOTAS = d.NumeroCuotasMora,
                        PAC = d.NumeroCuotasPactadas,
                        PAG = d.CuotasCanceladas,
                        MOR = "-",
                        PER = d.Periodicidad,
                        CUPO_APR_VLR_INI = d.ValorInicial,
                        CUPO_UTIL_SALDO_CORTE = d.SaldoObligacion,
                        PAGO_MIN_VLR_CUOTA = d.ValorCuota,
                        SIT_OBLIG = d.EstadoObligacion,
                        VALOR_MORA = d.ValorMora,
                        NATU_REES = d.NaturalezaReestructuracion,
                        REES = d.Reestructurado,
                        NO_REE = d.NumeroReestructuraciones,
                        MOR_MAX = d.MoraMaxima,
                        TIP_PAG = d.TipoPago,
                        MOD_EXT = d.ModoExtincion,
                        F_PAGO_F_EXTIN = "-",
                        F_PERMAN = d.FechaPermanencia,
                        COMPORTAMIENTOS = d.Comportamientos
                    })
                    .ToList();



                var InfoEndeudamientoFinancieroExtinguidas = DataSetInfoEndeudamientoFinancieroExtinguidas != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaFinancieroExtinguidas", DataSetInfoEndeudamientoFinancieroExtinguidas) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaFinancieroExtinguidas", new List<object>());
                if (DataSetInfoEndeudamientoFinancieroExtinguidas != null)
                {
                    InfoEndeudamientoFinancieroExtinguidas.Value = DataSetInfoEndeudamientoFinancieroExtinguidas;
                }
                reportParam.DataSources.Add(InfoEndeudamientoFinancieroExtinguidas);
            }
            else
            {
                var InfoEndeudamientoFinancieroExtinguidas = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaFinancieroExtinguidas", new List<object>());
                reportParam.DataSources.Add(InfoEndeudamientoFinancieroExtinguidas);
            }
            #endregion

            #region SectorRealAlDia2
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorRealAlDia != null)
            {
                // InfoEndeudamientoSectorRealAlDia
                var InfoEndeudamientoSectorRealAlDiaDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorRealAlDia?.Obligacion?
                    .Select(d => new
                    {
                        FECHA_CORTE = d.FechaCorte,
                        TIPO_CONT = d.TipoContrato,
                        CATE_LCRE = d.LineaCredito,
                        NO_OBLIG = d.NumeroObligacion,
                        EST_CONTR = d.EstadoContrato,
                        NOM_ENT = d.NombreEntidad,
                        TIPO_ENT = d.TipoEntidad,
                        TIPO_EMP = d.TipoEntidad,
                        CIUDAD = d.Ciudad,
                        SUCURSAL = d.Sucursal,
                        CALD = d.Calidad,
                        EST_TITU = d.EstadoTitular,
                        VIG = d.Vigencia,
                        MES = d.NumeroMesesContrato,
                        CLA_PER = "-",
                        F_INICIO = d.FechaApertura,
                        F_TERM = d.FechaTerminacion,
                        PAC = d.NumeroCuotasPactadas,
                        PAG = d.CuotasCanceladas,
                        MOR = "-",
                        PER = d.Periodicidad,
                        CUPO_APRO_VLR_INIC = d.ValorInicial,
                        CUPO_UTIL_SALDO_CORTE = d.SaldoObligacion,
                        PAGO_MIN_VLR_CUOTA = d.ValorCuota,
                        SIT_OBLIG = d.EstadoObligacion,
                        VALOR_MORA = d.ValorMora,
                        TIP_PAG = d.TipoPago,
                        MOD_EXT = d.ModoExtincion,
                        REF = d.Reestructurado,
                        MOR_MAX = d.MoraMaxima,
                        F_PAGO_F_EXTIN = "-",
                        F_PERMAN = d.FechaPermanencia,
                        COMPORTAMIENTOS = d.Comportamientos,
                        VALOR_CARGO_FIJO = d.ValorCargoFijo

                    })
                    .ToList();

                foreach (var dato in InfoEndeudamientoSectorRealAlDiaDataSet)
                {
                    if (!string.IsNullOrEmpty(dato.F_PERMAN))
                    {
                        fechaPermanecia = "NO VIABLE: Evaluación de políticas de riesgo (Presenta fecha de permanencia)";
                        break;
                    }
                }

                foreach (var dato in InfoEndeudamientoSectorRealAlDiaDataSet)
                {
                    if (dato.CATE_LCRE == "TELC" && dato.TIPO_CONT=="SRV" && dato.PAGO_MIN_VLR_CUOTA == "0")
                    {
                        valorTotalSectorRealServicio += double.Parse(dato.CUPO_UTIL_SALDO_CORTE)*1000;
                        break;
                    }
                }

                var InfoEndeudamientoSectorRealAlDi = InfoEndeudamientoSectorRealAlDiaDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealAlDia", InfoEndeudamientoSectorRealAlDiaDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealAlDia", new List<object>());
                if (InfoEndeudamientoSectorRealAlDiaDataSet != null)
                {
                    InfoEndeudamientoSectorRealAlDi.Value = InfoEndeudamientoSectorRealAlDiaDataSet;
                }
                reportParam.DataSources.Add(InfoEndeudamientoSectorRealAlDi);
            }
            else
            {
                var InfoEndeudamientoSectorRealAlDi = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealAlDia", new List<object>());
                reportParam.DataSources.Add(InfoEndeudamientoSectorRealAlDi);
            }
            #endregion

            #region SectorRealExtinguidas
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.SectorRealExtinguidas != null)
            {
                // InfoEndeudamientoSectorRealExtinguidas
                var InfoEndeudamientoSectorRealExtinguidasDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.SectorRealExtinguidas?.Obligacion?
                    .Select(d => new
                    {
                        FECHA_CORTE = d.FechaCorte,
                        TIPO_CONT = d.TipoContrato,
                        CATE_LCRE = d.LineaCredito,
                        NO_OBLIG = d.NumeroObligacion,
                        EST_CONTR = d.EstadoContrato,
                        NOM_ENT = d.NombreEntidad,
                        TIPO_ENT = d.TipoEntidad,
                        TIPO_EMP = d.TipoEntidad,
                        CIUDAD = d.Ciudad,
                        SUCURSAL = d.Sucursal,
                        CALD = d.Calidad,
                        EST_TITU = d.EstadoTitular,
                        VIG = d.Vigencia,
                        MES = d.NumeroMesesContrato,
                        CLA_PER = "-",
                        F_INICIO = d.FechaApertura,
                        F_TERM = d.FechaTerminacion,
                        PAC = d.CuotasCanceladas,
                        PAG = d.NumeroCuotasPactadas,
                        MOR = "-",
                        PER = d.Periodicidad,
                        CUPO_APR_VLR_INI = d.ValorInicial,
                        CUPO_UTIL_SALDO_CORTE = d.SaldoObligacion,
                        PAGO_MIN_VLR_CUOTA = d.ValorCuota,
                        SIT_OBLIG = d.EstadoObligacion,
                        VALOR_MORA = d.ValorMora,
                        TIP_PAG = d.TipoPago,
                        MOD_EXT = d.ModoExtincion,
                        REF = d.Reestructurado,
                        MOR_MAX = d.MoraMaxima,
                        F_PAGO_F_EXTIN = "-",
                        F_PERMAN = d.FechaPermanencia,
                        COMPORTAMIENTOS = d.Comportamientos,
                        VALOR_CARGO_FIJO = d.ValorCargoFijo
                    })
                    .ToList();



                var InfoEndeudamientoSectorRealExtinguidas = InfoEndeudamientoSectorRealExtinguidasDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealExtinguidas", InfoEndeudamientoSectorRealExtinguidasDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealExtinguidas", new List<object>());
                if (InfoEndeudamientoSectorRealExtinguidasDataSet != null)
                {
                    InfoEndeudamientoSectorRealExtinguidas.Value = InfoEndeudamientoSectorRealExtinguidasDataSet;
                }
                reportParam.DataSources.Add(InfoEndeudamientoSectorRealExtinguidas);
            }
            else
            {
                var InfoEndeudamientoSectorRealExtinguidas = new Microsoft.Reporting.NETCore.ReportDataSource("InfoEndeudaSectorRealExtinguidas", new List<object>());
                reportParam.DataSources.Add(InfoEndeudamientoSectorRealExtinguidas);
            }
            #endregion

            #region HuellaConsulta
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.HuellaConsulta != null)
            {
                //INFO HUELLAS
                var infoHuellsDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.HuellaConsulta?.Consulta?
                   .Select(d => new
                   {
                       ENTIDAD = d.NombreEntidad,
                       FECHA = d.FechaConsulta,
                       CIUDAD = d.Ciudad,
                       SUCURSAL = d.Sucursal,

                   })
                   .ToList();

                var infoHuellas = infoHuellsDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoHuellas", infoHuellsDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoHuellas", new List<object>());
                if (infoHuellsDataSet != null)
                {
                    infoHuellas.Value = infoHuellsDataSet;
                }
                reportParam.DataSources.Add(infoHuellas);
            }
            else
            {
                var infoHuellas = new Microsoft.Reporting.NETCore.ReportDataSource("InfoHuellas", new List<object>());
                reportParam.DataSources.Add(infoHuellas);
            }
            #endregion

            #region  InfoConTrimeste 1
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimI != null)
                {
                    //INFO TRIMESTRE 1
                    var InfoConTrimesteDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.Endeudamiento?.EndeudamientoTrimI?.Endeudamiento71?
                       .Select(d => new
                       {
                           Calf = d.Calificacion,
                           TipoMoneda = d.TipoModena,
                           NroDeudaCial = d.NumeroOperacionesComercial,
                           NroDeudaCons = d.NumeroOperacionesConsumo,
                           NroDeudaVivi = d.NumeroOperacionesVivienda,
                           NroDeudaMcr = d.ValorDeudaMicrocredito,
                           VlrDeudaCial = d.ValorDeudaComercial,
                           VlrDeudaCons = d.ValorDeudaConsumo,
                           VlrDeudaVivi = d.ValorDeudaVivienda,
                           VlrDeudaMcr = d.ValorDeudaMicrocredito,
                           Total = d.Total,
                           Pade = d.ParticipacionTotalDeudas,
                           CubriGarCial = d.CubrimientoGarantiaComercial,
                           CubriGarCons = d.CubrimientoGarantiaConsumo,
                           CubriGarVivi = d.CubrimientoGarantiaVivienda,
                           CubriGarMcr = d.CubrimientoGarantiaMicrocredito,
                           CalfDos = d.Calificacion,
                           NroDeudasNumero = d.ValorDeudaMicrocredito,
                           NroDeudasValor = d.NumeroOperacionesMicrocredito,
                           CuotaEsperada = aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimI.Endeudamiento72.FirstOrDefault().CuotaEsperada,
                           PorcentajeCumpli = aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimI.Endeudamiento72.Where(x => x.CumplimientoCuota != null).FirstOrDefault().CumplimientoCuota,

                       })
                       .ToList();

                    var InfoConTrimeste = InfoConTrimesteDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre1", InfoConTrimesteDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre1", new List<object>());
                    if (InfoConTrimesteDataSet != null)
                    {
                        InfoConTrimeste.Value = InfoConTrimesteDataSet;
                    }
                    reportParam.DataSources.Add(InfoConTrimeste);
                }
                else
                {
                    var InfoConTrimeste = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre1", new List<object>());
                    reportParam.DataSources.Add(InfoConTrimeste);
                }
            }
            else
            {
                var InfoConTrimeste = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre1", new List<object>());
                reportParam.DataSources.Add(InfoConTrimeste);
            }

            #endregion

            #region  InfoConTrimeste Detale 1
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimI != null)
                {
                    //INFO InfoConTrimeste Detalle 1
                    var InfoConTrimesteDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.Endeudamiento?.EndeudamientoTrimI?.Endeudamiento73?
                        .Select(d => new
                        {
                            TipoEnt = d.TipoEntidad,
                            NomEntidad = d.NombreEntidad,
                            TipoEntDos = "-",
                            NomEntCartera = d.EntidadOriginadoraCartera,
                            TipoFind = d.TipoFideicomiso,
                            NoFideico = d.NumeroFideicomiso,
                            ModaCreed = d.ModalidadCredito,
                            Calf = d.Calificacion,
                            TipoMon = d.TipoMoneda,
                            NoDeu = d.NumeroOperadores,
                            VlrDeudas = d.ValorDeuda,
                            Pade = d.ParticipacionTotalDeudas,
                            Gar = d.CubrimientoGarantia,
                            FechaEvaluo = d.FechaUltimoAvaluo,
                            CuotaEsperada = d.CuotaEsperada,
                            PorcentajeCumpli = d.CumplimientoCuota,
                            TipoGarantia = d.TipoGarantia,

                        })
                        .ToList();

                    var InfoConTrimeste = InfoConTrimesteDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InformacionConsolidadaDetalleTrimestre1", InfoConTrimesteDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InformacionConsolidadaDetalleTrimestre1", new List<object>());
                    if (InfoConTrimesteDataSet != null)
                    {
                        InfoConTrimeste.Value = InfoConTrimesteDataSet;
                    }
                    reportParam.DataSources.Add(InfoConTrimeste);
                }
                else
                {
                    var InfoConTrimeste = new Microsoft.Reporting.NETCore.ReportDataSource("InformacionConsolidadaDetalleTrimestre1", new List<object>());
                    reportParam.DataSources.Add(InfoConTrimeste);
                }
            }
            else
            {
                var InfoConTrimeste = new Microsoft.Reporting.NETCore.ReportDataSource("InformacionConsolidadaDetalleTrimestre1", new List<object>());
                reportParam.DataSources.Add(InfoConTrimeste);
            }

            #endregion

            #region  InfoConTrimeste 2
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimII != null)
                {
                    var InfoConTrimesteDosDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.Endeudamiento?.EndeudamientoTrimII?.Endeudamiento81?
                       .Select(d => new
                       {
                           Calf = d.Calificacion,
                           TipoMoneda = d.TipoModena,
                           NroDeudaCial = d.NumeroOperacionesComercial,
                           NroDeudaCons = d.NumeroOperacionesConsumo,
                           NroDeudaVivi = d.NumeroOperacionesVivienda,
                           NroDeudaMcr = d.ValorDeudaMicrocredito,
                           VlrDeudaCial = d.ValorDeudaComercial,
                           VlrDeudaCons = d.ValorDeudaConsumo,
                           VlrDeudaVivi = d.ValorDeudaVivienda,
                           VlrDeudaMcr = d.ValorDeudaMicrocredito,
                           Total = d.Total,
                           Pase = d.ParticipacionTotalDeudas,
                           CubriGarCial = d.CubrimientoGarantiaComercial,
                           CubriGarCons = d.CubrimientoGarantiaConsumo,
                           CubriGarVivi = d.CubrimientoGarantiaVivienda,
                           CubriGarMcr = d.CubrimientoGarantiaMicrocredito,
                           CalfDos = d.Calificacion,
                           NroDeudasNumero = d.ValorDeudaMicrocredito,
                           NroDeudasValor = d.NumeroOperacionesMicrocredito,
                           CuotaEsperada = aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimII.Endeudamiento82.FirstOrDefault().CuotaEsperada,
                           PorcentajeCumpli = aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimII.Endeudamiento82.Where(x => x.CumplimientoCuota != null).FirstOrDefault().CumplimientoCuota,

                       })
                       .ToList();

                    var InfoConTrimeste2 = InfoConTrimesteDosDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre2", InfoConTrimesteDosDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre2", new List<object>());
                    if (InfoConTrimesteDosDataSet != null)
                    {
                        InfoConTrimeste2.Value = InfoConTrimesteDosDataSet;
                    }
                    reportParam.DataSources.Add(InfoConTrimeste2);
                }
                else
                {
                    var InfoConTrimeste2 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre2", new List<object>());
                    reportParam.DataSources.Add(InfoConTrimeste2);
                }
            }
            else
            {
                var InfoConTrimeste2 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre2", new List<object>());
                reportParam.DataSources.Add(InfoConTrimeste2);
            }

            #endregion

            #region  InfoConTrimeste Detalle 2
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimII != null)
                {
                    //INFO  InfoConTrimeste Detalle 2
                    var InfoConTrimesteDosDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.Endeudamiento?.EndeudamientoTrimII?.Endeudamiento83?
                       .Select(d => new
                       {
                           TipoEnt = d.TipoEntidad,
                           NomEntidad = d.NombreEntidad,
                           TipoEntDos = "-",
                           NomEntCartera = d.EntidadOriginadoraCartera,
                           TipoFind = d.TipoFideicomiso,
                           NoFideico = d.NumeroFideicomiso,
                           ModaCreed = d.ModalidadCredito,
                           Calf = d.Calificacion,
                           TipoMon = d.TipoMoneda,
                           NoDeu = d.NumeroOperadores,
                           VlrDeudas = d.ValorDeuda,
                           Pade = d.ParticipacionTotalDeudas,
                           Gar = d.CubrimientoGarantia,
                           FechaEvaluo = d.FechaUltimoAvaluo,
                           CuotaEsperada = d.CuotaEsperada,
                           PorcentajeCumpli = d.CumplimientoCuota,
                           TipoGarantia = d.TipoGarantia,
                       })
                       .ToList();



                    var InfoConTrimeste2 = InfoConTrimesteDosDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre2", InfoConTrimesteDosDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre2", new List<object>());
                    if (InfoConTrimesteDosDataSet != null)
                    {
                        InfoConTrimeste2.Value = InfoConTrimesteDosDataSet;
                    }
                    reportParam.DataSources.Add(InfoConTrimeste2);
                }
                else
                {
                    var InfoConTrimeste2 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre2", new List<object>());
                    reportParam.DataSources.Add(InfoConTrimeste2);
                }
            }
            else
            {
                var InfoConTrimeste2 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre2", new List<object>());
                reportParam.DataSources.Add(InfoConTrimeste2);
            }

            #endregion

            #region  InfoConTrimeste 3
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimIII != null)
                {
                    //INFO InfoConTrimeste 3
                    var InfoConTrimesteTresDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.Endeudamiento?.EndeudamientoTrimIII?.Endeudamiento91?
                        .Select(d => new
                        {
                            Calf = d.Calificacion,
                            TipoMoneda = d.TipoModena,
                            NroDeudaCial = d.NumeroOperacionesComercial,
                            NroDeudaCons = d.NumeroOperacionesConsumo,
                            NroDeudaVivi = d.NumeroOperacionesVivienda,
                            NroDeudaMcr = d.ValorDeudaMicrocredito,
                            VlrDeudaCial = d.ValorDeudaComercial,
                            VlrDeudaCons = d.ValorDeudaConsumo,
                            VlrDeudaVivi = d.ValorDeudaVivienda,
                            VlrDeudaMcr = d.ValorDeudaMicrocredito,
                            Total = d.Total,
                            Pade = d.ParticipacionTotalDeudas,
                            CubriGarCial = d.CubrimientoGarantiaComercial,
                            CubriGarCons = d.CubrimientoGarantiaConsumo,
                            CubriGarVivi = d.CubrimientoGarantiaVivienda,
                            CubriGarMcr = d.CubrimientoGarantiaMicrocredito,
                            CalfDos = d.Calificacion,
                            NroDeudasNumero = d.ValorDeudaMicrocredito,
                            NroDeudasValor = d.NumeroOperacionesMicrocredito,
                            CuotaEsperada = aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimIII.Endeudamiento92.FirstOrDefault().CuotaEsperada,
                            PorcensajteCumpli = aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimIII.Endeudamiento92.Where(x => x.CumplimientoCuota != null).FirstOrDefault().CumplimientoCuota,

                        })
                        .ToList();

                    var InfoConTrimeste3 = InfoConTrimesteTresDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre3", InfoConTrimesteTresDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre3", new List<object>());
                    if (InfoConTrimesteTresDataSet != null)
                    {
                        InfoConTrimeste3.Value = InfoConTrimesteTresDataSet;
                    }
                    reportParam.DataSources.Add(InfoConTrimeste3);
                }
                else
                {
                    var InfoConTrimeste3 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre3", new List<object>());
                    reportParam.DataSources.Add(InfoConTrimeste3);
                }
            }
            else
            {
                var InfoConTrimeste3 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoTrimestre3", new List<object>());
                reportParam.DataSources.Add(InfoConTrimeste3);
            }

            #endregion

            #region  InfoConTrimeste Detalle 3
            if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento != null)
            {
                if (aplicante.DSOnlineBureauCOLData.Response.Tercero.Endeudamiento.EndeudamientoTrimIII != null)
                {
                    //InfoConTrimeste
                    var InfoConTrimesteTresDataSet = aplicante.DSOnlineBureauCOLData?.Response?.Tercero?.Endeudamiento?.EndeudamientoTrimIII?.Endeudamiento93?
                      .Select(d => new
                      {
                          TipoEnt = d.TipoEntidad,
                          NomEntidad = d.NombreEntidad,
                          TipoEntDos = "-",
                          NomEntCartera = d.EntidadOriginadoraCartera,
                          TipoFind = d.TipoFideicomiso,
                          NoFideico = d.NumeroFideicomiso,
                          ModaCreed = d.ModalidadCredito,
                          Calf = d.Calificacion,
                          TipoMon = d.TipoMoneda,
                          NoDeu = d.NumeroOperadores,
                          VlrDeudas = d.ValorDeuda,
                          Pade = d.ParticipacionTotalDeudas,
                          Gar = d.CubrimientoGarantia,
                          FechaEvaluo = d.FechaUltimoAvaluo,
                          CuotaEsperada = d.CuotaEsperada,
                          PorcensajeCumpli = d.CumplimientoCuota,
                          TipoGarantia = d.TipoGarantia,
                      })
                      .ToList();
                    var InfoConTrimeste3 = InfoConTrimesteTresDataSet != null ? new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre3", InfoConTrimesteTresDataSet) : new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre3", new List<object>());
                    if (InfoConTrimesteTresDataSet != null)
                    {
                        InfoConTrimeste3.Value = InfoConTrimesteTresDataSet;
                    }
                    reportParam.DataSources.Add(InfoConTrimeste3);
                }
                else
                {
                    var InfoConTrimeste3 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre3", new List<object>());
                    reportParam.DataSources.Add(InfoConTrimeste3);
                }
            }
            else
            {
                var InfoConTrimeste3 = new Microsoft.Reporting.NETCore.ReportDataSource("InfoConsoDetalleTrimestre3", new List<object>());
                reportParam.DataSources.Add(InfoConTrimeste3);
            }

            #endregion

            return (reportParam, fechaPermanecia, valorTotalSectorRealServicio);
        }
        #endregion
        public async Task<Response<SimulacionCreditoArchivo>> ReporteSimulacionCredito(List<SimulacionDto> data, int tipoDocumento)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            string reportPath = "";
            if (data.Sum(x => x.SaldoLeasing) > 0)
            {
                reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReportSimulacionCreditoLeasing.rdlc";
            }
            else
            {
                reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReportSimulacionCreditoHipotecario.rdlc";
            }

            report.ReportPath = reportPath;
            //Agregar parametros en el DataSet
            report = await ParametrosReporteSimulacionCredito(report, data);
            byte[] reportBytes = null;
            try
            {
                reportBytes = report.Render("PDF");
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }
            string renderType = tipoDocumento == 1 ? "PDF" : "EXCEL";


            SimulacionCreditoArchivo propiedadesArchivo = new SimulacionCreditoArchivo
            {
                ArchivoBytes = report.Render(renderType),
                FileName = "ReporteSimulacionCredito"
            };

            return new Response<SimulacionCreditoArchivo> { Data = propiedadesArchivo, IsSuccess = true, ReturnMessage = "Solicitud Exitosa" };

        }
        public async Task<Response<SimulacionCreditoArchivo>> ReporteSimulacion60PorCiento(ReporteSimulacion60PorCientoDto data)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteSimulacionCredito60%.rdlc";
            report.ReportPath = reportPath;
            report.SetParameters(ParametrosStaticos60PorCiento(data).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList());


            SimulacionCreditoArchivo propiedadesArchivo = new SimulacionCreditoArchivo
            {
                ArchivoBytes = report.Render("PDF"),
                FileName = "ReporteSimulacionCredito60%"
            };

            return new Response<SimulacionCreditoArchivo> { Data = propiedadesArchivo, IsSuccess = true, ReturnMessage = "Solicitud Exitosa" };

        }
        public async Task<Dictionary<string, string>> ParametrosStaticos60PorCiento(ReporteSimulacion60PorCientoDto data)
        {

            var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(data.NumeroDocumento).Result;

            var EndeuGlobalConCanon = datos.EDP_SALARIO_TOTAL - datos.EDP_ENDEUDAMIENTO_GLOBAL_CUOTA_PORC;
            return new Dictionary<string, string>
                {
                    { "SalarioTotal", datos.EDP_SALARIO_TOTAL.ToString("C", new CultureInfo("es-CO")) },
                    { "Deducciones", datos.EDP_DEDUCCIONES.ToString("C", new CultureInfo("es-CO"))},
                    { "SalarioNeto", datos.EDP_SALARIO_NETO.ToString("C", new CultureInfo("es-CO"))},
                    { "CapacidadPago", datos.EDP_CAPACIDAD_PAGO.ToString("C", new CultureInfo("es-CO"))},
                    { "CanonOCuotaMensual", datos.EDP_CANON_MENSUAL.ToString("C", new CultureInfo("es-CO"))},
                    { "disponibleLibranza", datos.EDP_DISPONIBLE_LIBRANZA.ToString("C", new CultureInfo("es-CO"))},
                    { "ControlCapacidadPago", datos.EDP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC},
                    { "EndeudamientoGlobal", datos.EDP_ENDEUDAMIENTO_GLOBAL_CUOTA_PORC.ToString()},
                    { "CuotasSectoresFinaSoliAseg", datos.EDP_SECTOR_FINANCIERO.ToString("C", new CultureInfo("es-CO"))},
                    { "CuotaSectorReal",datos.EDP_SECTOR_REAL.ToString("C", new CultureInfo("es-CO"))},
                    { "TotalObligaAntesNuevoCredito",(datos.EDP_SECTOR_REAL+datos.EDP_SECTOR_FINANCIERO+datos.EDP_DEDUCCIONES).ToString("C", new CultureInfo("es-CO"))},
                    { "TotalObliNuevoCredito", decimal.Parse(data.totalObliNuevoCredito).ToString("C", new CultureInfo("es-CO")) },
                    { "TotalObliNuevoCredAjustado",(datos.EDP_SECTOR_REAL+datos.EDP_SECTOR_FINANCIERO+datos.EDP_DEDUCCIONES).ToString("C", new CultureInfo("es-CO"))},
                    { "EndeuGlobalSinCanon",(datos.EDP_SALARIO_TOTAL-datos.EDP_SECTOR_REAL-datos.EDP_SECTOR_FINANCIERO-datos.EDP_DEDUCCIONES-datos.EDP_CANON_MENSUAL).ToString("C", new CultureInfo("es-CO"))},
                    { "EndeuGlobalConCanon",EndeuGlobalConCanon.ToString("C", new CultureInfo("es-CO"))},
                    { "ControlNivelEndeudamiento",(decimal.Parse(data.totalObliNuevoCredito)/datos.EDP_SALARIO_TOTAL)*100 > 80 ? "NO APLICA" : "SI APLICA"},
                    { "EndeuGlobalNuevoCanon",(decimal.Parse(data.totalObliNuevoCredito)/datos.EDP_SALARIO_TOTAL).ToString("P2")},
                    { "NuevoCanon",datos.EDP_CANON_MENSUAL.ToString()},
            };
        }
        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosReporteSimulacionCredito(Microsoft.Reporting.NETCore.LocalReport reportParam, List<SimulacionDto> data)
        {

            var simulacionCredito = new Microsoft.Reporting.NETCore.ReportDataSource("SimulacionCredito", data);
            simulacionCredito.Value = data;
            reportParam.DataSources.Add(simulacionCredito);

            return reportParam;
        }
        public async Task<FileResult> ReportePlanillaPagoIndiCesantias(ConsultarSolicitudDto data)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            GA2.Transversals.Commons.Response<GA2.Application.Dto.ObtenerTramiteSolicitudDto> response = null;
            try
            {
                response = await _solicitudBusinessLogic.ConsultarExisteSolicitud(data);
                ObtenerSolicitud obtenerSolicitud = new ObtenerSolicitud();
                //var response2 = await _clientsRepository.ObtenerCuentasMovimientosAfiliado(data.identificacion);
                //var cuentas = this._mapper.Map<List<CuentaDto>>(await _clientsRepository.ObtenerCuentas(Convert.ToInt32(data.identificacion)));
                if (response.Data != null)
                {
                    obtenerSolicitud = await this._solicitudRepository.ConsultaSolicitud(response.Data.solicitudId);
                    if (obtenerSolicitud.FAC_NUME == null && obtenerSolicitud.FAC_CONT == null)
                    {
                        throw new Exception($"No se envio el numeró de la planilla");
                    }
                }
                data.nroPlanilla = obtenerSolicitud.FAC_CONT;
                data.cdp = obtenerSolicitud.FAC_NUME;
                data.comprobante = obtenerSolicitud.MPR_CONT;

                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReportPlanillaPagosIndiCesantias.rdlc";
                report.ReportPath = reportPath;
                var clienteDetallado = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(data.tipoIdentificacion, data.identificacion);

                report = await ParametrosReportePlanillaPagoIndiCesantias(report, response.Data, data, clienteDetallado.Data);

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
            FileResult file = await GenerarReporte(bytes, false);


            var fileStreamResult = (FileStreamResult)file;
            fileStreamResult.FileStream.Position = 0;
            var base64 = await file.ToBase64Async();

            fileStreamResult.FileStream.Position = 0;

            var attachRequestDto = new GA2.Application.Dto.AttachRequestDto()
            {
                Radicado = response?.Data.NumeroRadicadoSG,
                UserOperation = response?.Data.SOL_GRUPO_ASIGNADO ?? response?.Data.SOL_NOMBREASIGNADO ?? "Sin asignar",
                Files = new List<Application.Dto.FileDto>
                    {
                        new Application.Dto.FileDto
                        {
                            Base64String = base64,
                            Description = "Planilla De Pago",
                            Ext = "pdf",
                            DirectoryCode = "00",
                            DocumentTypeCode = "00",


                        }
                    }
            };

            await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);
            return file;
        }

        public async Task<FileResult> ReporteProspeccionReclasificacion(IdentificacionProspeccion data)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            try
            {
                IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(data.tipoDocumento, data.numeroDocumento);

                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReporteProspeccionReclasificacion.rdlc";
                report.ReportPath = reportPath;
                var clienteDetallado = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(data.tipoDocumento, data.numeroDocumento);
                var user = await _usersRepository.ObtenerUsuarioPorId(data.USR_ID);
                report = await ParametrosReporteProspeccionReclasificacion(report, prospeccionResult, data, clienteDetallado.Data, user);
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
            return await GenerarReporte(bytes, false, data.numeroDocumento, true);
        }



        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosReportePlanillaPagoIndiCesantias(Microsoft.Reporting.NETCore.LocalReport reportParam, ObtenerTramiteSolicitudDto data, ConsultarSolicitudDto solicitud, ClienteDto cliente)
        {

            var reportParameters = ParametrosStaticosPlanillaPagos(data, cliente, solicitud).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();

            //INFO BENEFICIARIOS
            var infoBeneficariosDataSet = data.solicitudTerceroBeneficiarioDto
               .Select(d => new
               {
                   Nombre = d.razonsocial ?? "",
                   Cedula = d.numDoc ?? "",
                   Telefono = "" ?? "",
                   Banco = d.entidadObj.descripcion ?? "",
                   Cuenta = d.numeroCuenta + " " + d.tipoCuentaObj.descripcion ?? "",
                   OrdenPago = d.entidadObj.id,
                   Anotacion = "",
                   Direccion = "NO REGISTRA" ?? "",
                   Ciudad = "BOGOTÁ" ?? "",
                   Departamento = "BOGOTÁ D.C" ?? "",
                   Valor = (d.valorRetirarDos ?? 0).ToString("C") ?? ""
               })
               .ToList();


            var beneficiario = new Microsoft.Reporting.NETCore.ReportDataSource("Beneficiarios", infoBeneficariosDataSet);
            beneficiario.Value = infoBeneficariosDataSet;
            reportParam.DataSources.Add(beneficiario);



            ObtenerConceptosAfectadosClientes obtenerConceptosAfectados = new ObtenerConceptosAfectadosClientes();
            obtenerConceptosAfectados.SolicitudId = data.solicitudId;
            obtenerConceptosAfectados.Identificacion = data.identificacionCliente;
            var conceptosAfectadosSolicitud = await _solicitudRepository.ObtenerConceptosAfectados(obtenerConceptosAfectados);

            var ConceptosDataSet = conceptosAfectadosSolicitud.Select(c => new
            {
                NombreConcepto = c.CTC_DESCRIPCION,
                ValorConcepto = c.CAC_VALOR_DESCONTADO.ToString("C") ?? ""

            })
               .ToList();

            var conceptos = new Microsoft.Reporting.NETCore.ReportDataSource("Conceptos", ConceptosDataSet);
            conceptos.Value = ConceptosDataSet;
            reportParam.DataSources.Add(conceptos);
            reportParam.SetParameters(reportParameters);
            return reportParam;
        }


        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosReporteProspeccionReclasificacion(Microsoft.Reporting.NETCore.LocalReport reportParam, IEnumerable<LineaCreditoObligacion> prospeccionResult, IdentificacionProspeccion identificacionProspeccion, ClienteDto cliente, User usuario)
        {

            var reportParameters = ParametrosEstaticosProspeccionReclasificacion(cliente, identificacionProspeccion, usuario).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();

            ////INFO LINEAS INICIALES
            var lineasObligacionInicialDataSet = prospeccionResult
               .Select(d => new
               {
                   OLN_LINEA_CREDITO = d.OLN_LINEA_CREDITO,
                   OLN_SALDO_OBLIGACION = d.OLN_SALDO_OBLIGACION.ToString("C", new CultureInfo("es-CO")),
                   OLN_VALOR_CUOTA = d.OLN_VALOR_CUOTA.ToString("C", new CultureInfo("es-CO")),
                   OLN_ESTADO = d.OLN_ESTADO,
                   OLN_CUOTAS_PACTADAS = d.OLN_CUOTAS_PACTADAS.ToString(),
                   OLN_CUOTAS_CANCELADAS = d.OLN_CUOTAS_CANCELADAS.ToString(),
                   OLN_CUOTAS_RESTANTES = d.OLN_CUOTAS_RESTANTES.ToString(),
                   OLN_ESTADO_RECALCULAR = d.OLN_ESTADO_RECALCULAR.ToString()
               })
               .ToList();


            var lineasObligacionInicialParam = new Microsoft.Reporting.NETCore.ReportDataSource("LineasObligacion", lineasObligacionInicialDataSet);
            lineasObligacionInicialParam.Value = lineasObligacionInicialDataSet;
            reportParam.DataSources.Add(lineasObligacionInicialParam);



            var lineasObligacionInicialFiltrada = prospeccionResult
                .Where(d => d.OLN_ESTADO_RECALCULAR == false)
                .Select(d => new
                {
                    OLN_LINEA_CREDITO = d.OLN_LINEA_CREDITO,
                    OLN_SALDO_OBLIGACION = d.OLN_SALDO_OBLIGACION.ToString("C", new CultureInfo("es-CO")),
                    OLN_VALOR_CUOTA = d.OLN_VALOR_CUOTA.ToString("C", new CultureInfo("es-CO")),
                    OLN_ESTADO = d.OLN_ESTADO,
                    OLN_CUOTAS_PACTADAS = d.OLN_CUOTAS_PACTADAS.ToString(),
                    OLN_CUOTAS_CANCELADAS = d.OLN_CUOTAS_CANCELADAS.ToString(),
                    OLN_CUOTAS_RESTANTES = d.OLN_CUOTAS_RESTANTES.ToString(),
                    OLN_ESTADO_RECALCULAR = d.OLN_ESTADO_RECALCULAR.ToString()
                })
                .ToList();


            var lineasObligacionInicialFiltradaParam = new Microsoft.Reporting.NETCore.ReportDataSource("LineasObligacionFiltrada", lineasObligacionInicialFiltrada);
            lineasObligacionInicialFiltradaParam.Value = lineasObligacionInicialFiltrada;
            reportParam.DataSources.Add(lineasObligacionInicialFiltradaParam);



            reportParam.SetParameters(reportParameters);
            return reportParam;
        }

        public async Task<Dictionary<string, string>> ParametrosEstaticosProspeccionReclasificacion(ClienteDto cliente, IdentificacionProspeccion identificacionProspeccion, User usuario)
        {
            return new Dictionary<string, string>
            {
                { "Dia", DateTime.Now.Day.ToString() },
                { "Mes", DateTime.Now.ToString("MMMM", new CultureInfo("es-ES")) },  // Nombre del mes en español
                { "Anio", DateTime.Now.Year.ToString() },
                { "Nombres", $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" },
                { "Asesor", $"{usuario.USR_NOMBRE} {usuario.USR_SEGUNDONOMBRE} {usuario.USR_APELLIDO} {usuario.USR_SEGUNDOAPELLIDO}" }
            };



        }


        public async Task<Dictionary<string, string>> ParametrosStaticosPlanillaPagos(ObtenerTramiteSolicitudDto data, ClienteDto cliente, ConsultarSolicitudDto solicitud)
        {
            var fuerza = await _solicitudRepository.ObtenerFuerzaPorId(cliente.IdFuerza);
            var UnidadEjecutora = await _solicitudRepository.ObtenerFuerzaPorId(cliente.IdFuerza);
            var categoria = _mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias()).Where(x => x.Id == cliente.IdCategoria).FirstOrDefault();
            return new Dictionary<string, string>
                {
                    { "FecAprobacion", DateTime.Now.ToString() },
                    { "FecPlanilla", DateTime.Now.ToString()},
                    { "NroPlanilla", solicitud.nroPlanilla},
                    { "TipoSolicitud", data.NombreTarea},
                    { "Radicado", data.NumeroRadicadoSG},
                    { "PrimerNombreApellido", data.nombresCliente+ " " + data.apellidosCliente},
                    { "UnidadEjecutora", UnidadEjecutora.FirstOrDefault().FRZ_DESCRIPCION},
                    { "Fuerza", fuerza.FirstOrDefault().FRZ_DESCRIPCION},
                    { "FecImpresion", DateTime.Now.ToString()},
                    { "FecElaboracion", DateTime.Now.ToString()},
                    { "Regimen",cliente.Regimen.ToString()},
                    { "VlrCesantias", data.valorRetirar.ToString("C") },
                    { "NombreAfiliado",data.nombresCliente},
                    { "NroIdentificacion",data.identificacionCliente},
                    { "Categoria",categoria.Descripcion},
                    { "Cuotas",cliente.NumeroCuotas.ToString()},
                    { "Comprobante",solicitud.cdp.ToString()},
                    { "CDP", solicitud.comprobante.ToString()}

            };



        }
        public async Task<string> GenerarTxt(string line)
        {
            string fileBase64 = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(line);
                memoryStream.Write(bytes, 0, bytes.Length);
                byte[] memoryBytes = memoryStream.ToArray();
                fileBase64 = Convert.ToBase64String(memoryBytes);
            }
            return fileBase64.ToString();
        }

        public async Task<string> GenerarExcel(DataTable data, string nameReport)
        {
            string fileBase64 = string.Empty;
            if (data.Rows.Count > 0)
            {
                var memoryStream = new MemoryStream();
                var wb = new XLWorkbook();
                wb.Worksheets.Add(data, nameReport);
                wb.ColumnWidth = 100;
                wb.SaveAs(memoryStream);
                byte[] memoryBytes = memoryStream.ToArray();
                fileBase64 = Convert.ToBase64String(memoryBytes);
            }
            return fileBase64.ToString();

        }
        public async Task<string> GenerarExcelStyle(ReportRequestExcelDto request)
        {
            var workbook = new XLWorkbook();
            string fileBase64 = string.Empty;

            var worksheet = workbook.Worksheets.Add("Hoja1");
            //var worksheet = workbook.Worksheet("Hoja1");

            // Agregar encabezado con imagen en la parte superior
            var header = $"{_webHostEnvironment.ContentRootPath}\\Img\\Header.png";
            var footer = $"{_webHostEnvironment.ContentRootPath}\\Img\\FOOTER.png";

            var headerPicture = worksheet.AddPicture(header); // Cambia a la ruta de tu imagen
            var headerImageCell = worksheet.Cell(1, 1); // Celda donde deseas que aparezca la imagen
            headerPicture.MoveTo(headerImageCell);
            workbook = EncabezadosExcel(workbook, request.ColumnName);
            workbook = DatosExcel(workbook, request.ColumnDataList);
            // Agregar pie de página con imagen
            var footerPicture = worksheet.AddPicture(footer); // Cambia a la ruta de tu imagen
            var footerImageCell = worksheet.Cell(worksheet.LastRowUsed().RowNumber() + 1, 1); // Celda después de la última fila usada
            footerPicture.MoveTo(footerImageCell);

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var excelBytes = stream.ToArray();
                fileBase64 = Convert.ToBase64String(excelBytes);

                return string.Format(fileBase64.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

        }

        public XLWorkbook EncabezadosExcel(XLWorkbook workbook, string[] encabezados)
        {
            var worksheet = workbook.Worksheet("Hoja1");
            var headerRow = worksheet.Row(12);

            for (int col = 0; col < encabezados.Length; col++)
            {
                var cell = headerRow.Cell(col + 1);
                cell.Value = encabezados[col];

                // Aplicar estilo al encabezado
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1c3f8b");
                cell.Style.Font.FontColor = XLColor.White;
            }

            return workbook;
        }
        public XLWorkbook DatosExcel(XLWorkbook workbook, List<ColumnData> datos)
        {
            // Alternar colores de fondo
            XLColor dataColor1 = XLColor.FromHtml("#ffe27f");
            XLColor dataColor2 = XLColor.White;
            XLColor currentDataColor = dataColor1;

            var worksheet = workbook.Worksheet("Hoja1");

            // Agregar celdas con datos en columnas separadas
            for (int col = 0; col < datos.Count; col++)
            {
                var data = datos[col].Data;
                var dataRow = worksheet.Row(13 + col);

                for (int row = 0; row < data.Count; row++)
                {
                    var cell = dataRow.Cell(row + 1); // +1 para contar la columna de encabezados
                    cell.Value = data[row];

                    // Aplicar estilo a la celda de datos
                    cell.Style.Fill.BackgroundColor = currentDataColor;
                    currentDataColor = currentDataColor == dataColor1 ? dataColor2 : dataColor1;
                }
            }

            return workbook;
        }


        public async Task<FileResult> ReporteResumenFinanciero(ReporteResumenFinancieroDto reporteResumenFinanciero)
        {

            var ObtenerSolicitudCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, reporteResumenFinanciero.identification);

            if (ObtenerSolicitudCredito != null)
            {

                try
                {
                    var datosPersonales = new SimulacionDatosPersonalesDto();
                    var datosTecnico = new ValidacionTelAfiliados();

                    //var usuarios = new ClienteCedulaDto();
                    ClienteCedulaDto usuarios = new ClienteCedulaDto();


                    // var ObtenerSolicitudCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, reporteResumenFinanciero.identification);

                    // if (ObtenerSolicitudCredito != null )
                    // {
                    var RequestCreditoProducto = new PeticionSolicitudCreditoProductoDto { IND = 2, SOC_ID = (Guid)ObtenerSolicitudCredito.SOC_ID };

                    datosPersonales = (await this._creditoBusinessLogic.ObtenerSimulacionPersonales(reporteResumenFinanciero.identification)).Data;

                    var peticion = this._mapper.Map<PeticionSolicitudCreditoProducto>(RequestCreditoProducto);
                    var CreditoProducto2 = await this._creditoRepository.ObtenerSolicCreditoProducto(peticion);

                    var RequestCreditoInfJurInm = new RequestSolicCreditoInfJurInmDto { indicador = 3, idSolicitudCredito = (Guid)ObtenerSolicitudCredito.SOC_ID, identificacion = reporteResumenFinanciero.identification };
                    var CreditoInfJurInm = (await this._creditoBusinessLogic.ObtenerSolicCreditoInfJurInm(RequestCreditoInfJurInm)).Data;

                    var MatriculasCreditoSolicitud = this._creditoBusinessLogic.ObtenerMatriculasCreditoporSolicitud((Guid)ObtenerSolicitudCredito.SOC_ID).Result.Data;
                    var TipoMatricula = await _catalogsRepository.ObtenerValoresCatalogosPorIdCatalogo(90);

                    var requestInmTecnica = new RequestSolicCreditoInfTecnicaDto { Indicador = 2, SolicitudCredito = ObtenerSolicitudCredito.SOC_ID.ToString() };
                    var InmTecnica = this._creditoBusinessLogic.ObtenerSolicCreditoInfTecnica(requestInmTecnica).Result.Data;

                    var clienteCuentas = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, reporteResumenFinanciero.identification)).Data;


                    IEnumerable<EstadoCivilDto> estadoCivilTypes = this._mapper.Map<IEnumerable<EstadoCivilDto>>(await this._catalogsRepository.ObtenerEstadosCiviles());
                    //credito.EstadosCiviles = (List<EstadoCivilDto>)estadoCivilTypes;

                    var estadoCivil = (await _catalogsBusinessLogic.ObtenerEstadosCiviles()).Data.First(x => x.Id == clienteCuentas.EstadoCivil).Descripcion;


                    //     usuarios = this._mapper.Map<ClienteCedula, ClienteCedulaDto>(await _clientsRepository.ObtenerInformacionClienteCedula(int.Parse( reporteResumenFinanciero.identification)));

                    //Response<ClienteCedulaDto> clienteDto = await  _iClientRequestProvider.ObtenerInformacionClienteCedula(int.Parse(reporteResumenFinanciero.identification));


                    #region Reporte resumen financiero 

                    var informacionBasicaDto = new InformacionBasicaDto
                    {
                        NOMBRES_COMPLETOS = datosPersonales.NombresApellidos,
                        EDAD = datosPersonales.Edad,
                        ESTADO_CIVIL = estadoCivil,
                        SEXO = clienteCuentas.Sexo == "M" ? "MASCULINO" : "FEMENINO",
                        FECHA_NACIMIENTO = new DateTime(1987, 3, 15),
                        LUGAR_NACIMIENTO = usuarios.IdCiudadNacimiento.ToString(),
                        DIRECCION_RESIDENCIA = clienteCuentas.Direccion,
                        TELEFONO_CELULAR = datosPersonales.TelefonoCelular,
                        TELEFONO_FIJO = datosPersonales.TelefonoFijo,
                        EMAIL = datosPersonales.EMail,
                        UNIDAD_EJECUTORA = datosPersonales.UnidadEjecutora,
                        CATEGORIA = datosPersonales.Categoria,
                        GRADO = datosPersonales.Grado,
                        CUOTAS_APORTADAS = datosPersonales.Cuotas.ToString(), 
                        FECHA_REPORTE = DateTime.Now.ToString("d 'de' MMMM 'de' yyyy", new CultureInfo("es-CO"))
                    };

                    var informacionFinancieraDto = new InformacionFinancieraDto
                    {
                        SALARIO_TOTAL = 21,
                        SCORE_CLEAR = ObtenerSolicitudCredito.SOC_SCORE == null ? 0 : int.Parse(ObtenerSolicitudCredito.SOC_SCORE),
                        SCORE_CAJA_HONOR = 22,
                        CAPACIDAD_PAGO = ObtenerSolicitudCredito.SOC_CAPACIDAD_ENDEUDAMIENTO.ToString(),
                        HABITO_PAGO = "F2",
                        PRE_APROBADO_EXPERTO = ObtenerSolicitudCredito.SOC_NUMERO_SOLICITUD.ToString(),
                        CONSULTA_CIFIN = "F4",
                        CONSULTA_CIFIN_DIAS = 23,
                        TOTAL_ACTIVOS = datosTecnico.VAL_VALOR_ACTIVOS,
                        DESCRIPCION_ACTIVOS = "Propiedades, vehículos",
                        TOTAL_PASIVOS = datosTecnico.VAL_VALOR_PASIVOS
                    };



                    var historialCrediticioDto = new HistorialCrediticioDto
                    {
                        PRODUCTO = "Tarjeta de crédito",
                        ENTIDAD = "Banco XYZ",
                        //CUPO_APROBADO = 150000000,
                        CUPO_APROBADO = ObtenerSolicitudCredito.SOP_VALOR_CREDITO,
                        ESTADO = "Activo"
                    };

                    var informacionProductoDto = new InformacionProductoDto
                    {
                        PRODUCTO = "Préstamo personal",
                        ENTIDAD = "Cooperativa ABC",
                        CUPO_APROBADO = ObtenerSolicitudCredito.SOP_VALOR_CREDITO,
                        SALDO = 4700000,
                        CUOTA = 1542000,
                        CLASIFICACION = "Buena"
                    };

                    var informacionInmuebleDto = new InformacionInmuebleDto
                    {
                        DESCRIPCION = String.Concat(ObtenerSolicitudCredito.SOP_TIPO_INMUEBLE, " pisos ", ObtenerSolicitudCredito.SOP_PISO_INMUEBLE),
                        AREA_CONSTRUIDA = ((from InfJuridica in CreditoInfJurInm

                                            join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula

                                            join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID

                                            where tipoM.CVL_DESCRIPCION == "APARTAMENTO" || tipoM.CVL_DESCRIPCION == "CASA"

                                            select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()).ToString(),
                        TIPO_VIVIENDA = CreditoProducto2.TIPO_VIVIENDA,
                        VENDEDORES = CreditoProducto2.SOP_NOMBRE_VENDEDOR,
                        ANTIGUEDAD = String.Concat(ObtenerSolicitudCredito.SOP_AÑOS_CONSTRUCCION.ToString(), " año"),
                        VALOR_AVALUO = InmTecnica.ValorAvaluoComercial,
                        VALOR_INMUEBLE = CreditoProducto2.SOP_VALOR_INMUEBLE
                    };

                    var informesDto = new InformesDto
                    {
                        ESTUDIO_AVALUO = InmTecnica.ConceptoEstTecnico,
                        ESTUDIO_TITULOS = InmTecnica.ConceptoEstTecnico,
                        ESTUDIO_SARLAFT = "El Informe de Riesgo, bajo el marco del Estudio SARLAFT, tiene como"
                    };


                    var financiacionDto = new FinanciacionDto
                    {
                        VALOR_INMUEBLE = CreditoProducto2.SOP_VALOR_INMUEBLE,
                        RECURSOS_PROPIOS = 30000,
                        TOTAL_APORTADO = 70000,
                        PORCENTAJE_TOTAL_APORTADO = 70,
                        PLAZO = ObtenerSolicitudCredito.SOP_PLAZO_FINANCIACION.ToString(),
                        TASA_FIJA = 0.05M,
                        MODALIDAD_PAGO = "Mensual",
                        OPCION_COMPRA = 45510000,
                        CANON_MENSUAL = 2751256
                    };


                    var informacionElaboradores = new
                    {
                        ELABORADOR = "CAROL MARITZA VARGAS DIAZ",
                        REVISOR = "GYNETH SEPULVEDO"
                    };

                    #endregion


                    var report = new Microsoft.Reporting.NETCore.LocalReport();
                    var path = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "Financiera", "ResumenConceptosFinancieros.rdlc");
                    report.ReportPath = path;

                    report.DataSources.Add(new ReportDataSource("InformacionBasica", new[] { informacionBasicaDto }));
                    report.DataSources.Add(new ReportDataSource("InformacionFinanciera", new[] { informacionFinancieraDto }));
                    report.DataSources.Add(new ReportDataSource("InformacionProducto", new[] { informacionProductoDto }));
                    report.DataSources.Add(new ReportDataSource("HistorialCrediticio", new[] { historialCrediticioDto }));
                    report.DataSources.Add(new ReportDataSource("InformacionInmueble", new[] { informacionInmuebleDto }));
                    report.DataSources.Add(new ReportDataSource("Informes", new[] { informesDto }));
                    report.DataSources.Add(new ReportDataSource("Financiacion", new[] { financiacionDto }));
                    report.DataSources.Add(new ReportDataSource("InformacionElaboradores", new[] { informacionElaboradores }));

                    byte[] reportBytes = report.Render("PDF");

                    var filename = $"Reporte Concepto Financiero {reporteResumenFinanciero.identification}.pdf";
                    return await GenerarArchivoPDF2(reportBytes, filename, (Guid)ObtenerSolicitudCredito.SOC_ID);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            else
            {
                throw new Exception("Error al obtener datos del Credito");
            }
        }

        private async Task<FileResult> GenerarArchivoPDF2(byte[] byteArray, string filename, Guid soc_id)
        {

            ServiciosDinamicos blobStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blobStorage.SRD_ESTADO)
            {
                FileInfo fi = new FileInfo(filename);
                BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "credito");
                await container.CreateIfNotExistsAsync();
                var newName = Guid.NewGuid() + fi.Extension;
                BlobClient blob = container.GetBlobClient(newName);

                using (Stream stream = new MemoryStream(byteArray))
                {
                    var result = await blob.UploadAsync(stream);

                    //await cloudBlockBlob.UploadFromStreamAsync(stream);

                }

                var Tdc_id = 0;
                DocumentosActSolicitudDto documentosActSimulacionDto = new DocumentosActSolicitudDto()
                {
                    SOC_ID = soc_id,
                    AFL_ID = 16
                };
                var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSolicitud(documentosActSimulacionDto);
                Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Contains("Informe de analista", StringComparison.OrdinalIgnoreCase)).Select(x => x.TDC_ID).FirstOrDefault();


                RequestDocumentoSolicCreditoDto requestDocumentoSolicCreditoDto = new RequestDocumentoSolicCreditoDto()
                {
                    codigoBarras = "",
                    creadoPor = new Guid(),
                    fechaCreacion = DateTime.Now,
                    idSolicitudCredito = soc_id,
                    idTipoDocumento = Tdc_id,
                    rutaImagen = newName
                };



                var Response = await this._creditoBusinessLogic.CrearDocumentoSolicCredito(requestDocumentoSolicCreditoDto);
                var msPdf = new MemoryStream(byteArray);
                var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
                {
                    FileDownloadName = filename
                };
                await archivoPdf.FileStream.FlushAsync();

                return archivoPdf;
            }
            else
            {
                return await GenerarArchivoPDF2Local(byteArray, filename, soc_id);
            }


        }

        private async Task<FileResult> GenerarArchivoPDF2Local(byte[] byteArray, string filename, Guid soc_id)
        {
            if (byteArray == null || byteArray.Length == 0)
                throw new ArgumentException("El archivo no puede estar vacío.", nameof(byteArray));
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("El nombre del archivo no puede estar vacío.", nameof(filename));
            if (soc_id == Guid.Empty)
                throw new ArgumentException("El identificador de la solicitud no es válido.", nameof(soc_id));

            string folderPath = Path.Combine(_configureOptions.CarpetaServidorStorage, "credito");

            Directory.CreateDirectory(folderPath);

            var newName = Guid.NewGuid() + Path.GetExtension(filename);
            var localFilePath = Path.Combine(folderPath, newName);

            try
            {
                await File.WriteAllBytesAsync(localFilePath, byteArray);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el archivo en la carpeta configurada.", ex);
            }

            int Tdc_id;
            DocumentosActSolicitudDto documentosActSimulacionDto = new DocumentosActSolicitudDto()
            {
                SOC_ID = soc_id,
                AFL_ID = 16
            };

            try
            {
                var documentosActRespuestaDto = await _creditoBusinessLogic.ObtenerDocumentosActSolicitud(documentosActSimulacionDto);
                Tdc_id = documentosActRespuestaDto.Data
                           .FirstOrDefault(x => x.TIPO_DOCUMENTO.Contains("Informe de analista", StringComparison.OrdinalIgnoreCase))?.TDC_ID ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los documentos asociados a la solicitud.", ex);
            }

            if (Tdc_id == 0)
                throw new Exception("No se encontró un tipo de documento válido para 'Informe de analista'.");

            RequestDocumentoSolicCreditoDto requestDocumentoSolicCreditoDto = new RequestDocumentoSolicCreditoDto()
            {
                codigoBarras = "",
                creadoPor = Guid.NewGuid(),
                fechaCreacion = DateTime.Now,
                idSolicitudCredito = soc_id,
                idTipoDocumento = Tdc_id,
                rutaImagen = localFilePath
            };

            await _creditoBusinessLogic.CrearDocumentoSolicCredito(requestDocumentoSolicCreditoDto);

            var msPdf = new MemoryStream(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = filename
            };

            await archivoPdf.FileStream.FlushAsync();
            return archivoPdf;
        }


        public async Task<FileResult> ReporteSFCF454()
        {
            var datosReporte = await _reportesRepository.ReporteSFCF454();

            throw new NotImplementedException();
        }
        /// <summary>
        /// Procesa reestructuraciones y créditos Leasing
        /// </summary>
        public async Task ProcesarNovedadesAPNRCAR(
            List<ReporteAPNRCARDto> response,
            Dictionary<Guid, IEnumerable<NovedadCartera>> novedadesPorCredito,
            Dictionary<string, List<ProductoClienteDto>> productosPorIdentificacion)
        {
            foreach (var item in response)
            {
                // ===== PROCESAR REESTRUCTURACIONES (TNV_ID = 3) =====
                if (novedadesPorCredito.TryGetValue(item.IdCredito, out var novedades) && novedades.Any())
                {
                    var tieneReestructuracion = novedades.Any(x => x.TNV_ID == 3);

                    if (tieneReestructuracion && item.TipoDeCartera == 1)
                    {
                        if (productosPorIdentificacion.TryGetValue(item.Nit, out var productos) && productos.Any())
                        {
                            var producto = productos.FirstOrDefault();
                            item.SaldoCredito = producto.SaldoCapital;
                            item.Provision = item.SaldoCredito * 0.01m;
                        }
                    }
                }

                // ===== PROCESAR HIPOTECARIOS (TipoCreditoId = 2) =====
                if (item.TipoDeCartera == 2)
                {
                    if (productosPorIdentificacion.TryGetValue(item.Nit, out var productos))
                    {
                        var productoHipotecarios = productos.FirstOrDefault(x => x.NumeroCredito == item.ConsecutivoNumeroDeContrato);
                        if (productoHipotecarios != null)
                        {
                            item.SaldoCredito = productoHipotecarios.SaldoCapital;
                            if (productoHipotecarios.IdEstadoCredito == 1)
                            {
                                item.Provision = item.SaldoCredito * 0.01m;
                            }
                        }
                    }
                }
            }
        }

        public async Task ProcesarReporteGeneral(
    List<CarteraReporteGeneralDto> response,
    Dictionary<Guid, IEnumerable<NovedadCartera>> novedadesPorCredito,
    Dictionary<(long NumeroCredito, int TipoCredito), PlanPagoDto> planesPorCredito)
        {
            foreach (var item in response)
            {
                // ===== PROCESAR REESTRUCTURACIONES (TNV_ID = 3) =====
                if (novedadesPorCredito.TryGetValue(item.CreditoId, out var novedades) && novedades.Any())
                {
                    var tieneReestructuracion = novedades.Any(x => x.TNV_ID == 3);

                    if (tieneReestructuracion && item.IdTipoCredito == 1)
                    {
                        // Buscar plan histórico para este crédito en la fecha del periodo
                        if (planesPorCredito.TryGetValue((item.NumeroCredito, item.IdTipoCredito), out var plan))
                        {
                            item.SaldoCapital = plan.SaldoCapital;
                            item.ValorDeudaRemanente = 0;
                            item.ProvisionTotal = item.SaldoCapital * 0.01m;
                            item.ValorOpcionCompra = 0;
                            item.CanonExtraordinarioFinal = 0;
                            if (item.DiasMora > 0)
                            {
                                switch (item.Calificacion)
                                {
                                    case "A":
                                        item.ValorMoraAnterior = item.ValorMoraAnterior * 0.01M;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                // ===== PROCESAR HIPOTECARIOS (TipoCreditoId = 2) =====
                if (item.IdTipoCredito == 2)
                {
                    // Buscar plan histórico para este crédito en la fecha del periodo
                    if (planesPorCredito.TryGetValue((item.NumeroCredito, item.IdTipoCredito), out var planHipo))
                    {
                        // item.SaldoCapital = planHipo.SaldoCapital;
                        //if (item.Estado_Descripcion == "AL DIA")
                        //{
                        //    item.ProvisionTotal = item.SaldoCapital * 0.01m;
                        //}
                        //else
                        //{
                        var p = item.ProvisionTotal;
                        //}
                    }
                    //if (item.DiasMora > 0)
                    //{
                    //    item.ProvisionTotal  =+ item.ValorIntereses;
                    //}
                }
            }
        }

        /// <summary>
        /// Procesa reestructuraciones y créditos Leasing
        /// </summary>
        public async Task ProcesarNovedadesYLeasing(
            List<CarteraReporteGeneralDto> response,
            Dictionary<Guid, IEnumerable<NovedadCartera>> novedadesPorCredito,
            Dictionary<string, List<ProductoClienteDto>> productosPorIdentificacion)
        {
            foreach (var item in response)
            {
                // ===== PROCESAR REESTRUCTURACIONES (TNV_ID = 3) =====
                if (novedadesPorCredito.TryGetValue(item.CreditoId, out var novedades) && novedades.Any())
                {
                    var tieneReestructuracion = novedades.Any(x => x.TNV_ID == 3);

                    if (tieneReestructuracion && item.IdTipoCredito == 1)
                    {
                        if (productosPorIdentificacion.TryGetValue(item.Identificacion, out var productos) && productos.Any())
                        {
                            var producto = productos.FirstOrDefault();
                            item.SaldoCapital = producto.SaldoCapital;
                            item.ValorDeudaRemanente = 0;
                            item.ProvisionTotal = item.SaldoCapital * 0.01m;
                            item.ValorOpcionCompra = 0;
                            item.CanonExtraordinarioFinal = 0;

                        }
                    }
                }

                // ===== PROCESAR HIPOTECARIOS (TipoCreditoId = 2) =====
                if (item.IdTipoCredito == 2)
                {
                    if (productosPorIdentificacion.TryGetValue(item.Identificacion, out var productos))
                    {
                        var productoHipotecarios = productos.FirstOrDefault(x => x.NumeroCredito == item.NumeroCredito);
                        if (productoHipotecarios != null)
                        {
                            item.SaldoCapital = productoHipotecarios.SaldoCapital;
                            if (item.Estado_Descripcion == "AL DIA")
                            {
                                if (item.Calificacion == "C" || item.Calificacion == "B")
                                {
                                    item.ProvisionTotal = item.SaldoCapital * 0.1m;
                                }
                                else
                                {
                                    item.ProvisionTotal = item.SaldoCapital * 0.01m;
                                }
                            }
                            else
                            {
                                if (item.Calificacion == "C" || item.Calificacion == "B")
                                {
                                    var ProvisionTotal = (item.SaldoCapital * 0.1m) + (item.ValorIntereses * 0.1m) + (item.ValorMoraAnterior * 0.1m);
                                }
                                else
                                {
                                    var saldo = (item.SaldoCapital * 0.01m);
                                    var interes = (item.ValorIntereses * 0.01m);
                                    var ProvisionTotal = saldo + interes;
                                    item.ProvisionTotal = ProvisionTotal;
                                    //item.ProvisionTotal = item.SaldoCapital * 0.01m;
                                }
                            }

                        }
                    }
                }
            }
        }
        public bool CumpleFiltroFecha(NovedadCartera novedad, DateTime fechaPeriodo)
        {
            return novedad.TNV_ID == 3
                ? novedad.FECHA_NOVEDAD <= fechaPeriodo
                : novedad.FECHA_NOVEDAD == fechaPeriodo;
        }


        public async Task<Response<IEnumerable<CarteraReporteGeneralDto>>> GenerarReporteGeneralCartera(
        ReporteGeneralCarteraParametersDto parametros)
        {
            try
            {
                var result = _mapper.Map<IEnumerable<CarteraReporteGeneralDto>>(
                    await _reportesRepository.GenerarReporteGeneralCarteraOptimizado(
                        parametros));

                // Si el reporte está vacío, retornar error
                if (!result.Any())
                {
                    return result.BuildSuccess();
                }

                // ════════════════════════════════════════════════════════════════════════
                // [2] OBTENER DATOS DE AFILIADOS (no se puede mover a BD)
                // ════════════════════════════════════════════════════════════════════════
                // Esto SÍ debe estar en C# porque viene de COINS (servicio externo)

                var docs = result.Select(x => x.Identificacion).Distinct().ToList();
                var listAfiliados = (await _clientRequestProvider
                    .ObtenerInformacionClientesPorIdentificacion(docs)).Data;

                // ════════════════════════════════════════════════════════════════════════
                // [3] OBTENER CATÁLOGOS (Fuerzas, Categorías)
                // ════════════════════════════════════════════════════════════════════════

                var fuerzas = (await _catalogsBusinessLogic.ObtenerFuerzas()).Data;
                var categorias = (await _catalogsBusinessLogic.ObtenerCategorias()).Data;

                // ════════════════════════════════════════════════════════════════════════
                // [4] UNIR DATOS EN MEMORIA (simple join de datos ya procesados)
                // ════════════════════════════════════════════════════════════════════════
                // Los datos del SP ya vienen procesados y listos
                // Solo necesitamos agregar información de afiliados y catálogos

                return result
                    .GroupJoin(
                        listAfiliados,
                        s => s.Identificacion,
                        s => s.Identificacion,
                        (rep, cli) =>
                        {
                            var cliente = cli?.FirstOrDefault();

                            // Agregar datos de afiliado
                            rep.NombreCompleto = cliente?.NombreCompleto ?? "";
                            rep.IdFuerza = cliente?.IdFuerza ?? 0;
                            rep.IdGrado = cliente?.IdGrado ?? 0;
                            rep.IdCategoria = cliente?.IdCategoria ?? 0;

                            // Agregar descripciones de catálogos
                            rep.Fuerza = fuerzas.FirstOrDefault(x => x.Id == rep.IdFuerza)?.Descripcion;
                            rep.Categoria = categorias.FirstOrDefault(x => x.Id == rep.IdCategoria)?.Descripcion;

                            return rep;
                        })
                    // Aplicar filtros finales (opcional, pueden estar ya en el SP)
                    .Where(x =>
                        (parametros.Fuerza == null || parametros.Fuerza == x.IdFuerza) &&
                        (parametros.Categoria == null || parametros.Categoria == x.IdCategoria)
                    )
                    .BuildSuccess();
            }
            catch (Exception ex)
            {
                // Log de error (implementar según tu framework)
                // _logger.LogError(ex, "Error generando reporte general de cartera");

                return new Response<IEnumerable<CarteraReporteGeneralDto>>
                {
                    ReturnMessage = "No se pudo generar el reporte",
                    IsSuccess = false
                };
            }
        }


        public async Task<Response<IEnumerable<CarteraReporteGeneralDto>>> GenerarReporteGeneralCarteraReestructurado(
       ReporteGeneralCarteraParametersDto parametros)
        {
            try
            {
                var result = _mapper.Map<IEnumerable<CarteraReporteGeneralDto>>(
                    await _reportesRepository.GenerarReporteGeneralCarteraReestructurado(
                        parametros));

                // Si el reporte está vacío, retornar error
                if (!result.Any())
                {
                    return result.BuildSuccess();
                }

                // ════════════════════════════════════════════════════════════════════════
                // [2] OBTENER DATOS DE AFILIADOS (no se puede mover a BD)
                // ════════════════════════════════════════════════════════════════════════
                // Esto SÍ debe estar en C# porque viene de COINS (servicio externo)

                var docs = result.Select(x => x.Identificacion).Distinct().ToList();
                var listAfiliados = (await _clientRequestProvider
                    .ObtenerInformacionClientesPorIdentificacion(docs)).Data;

                // ════════════════════════════════════════════════════════════════════════
                // [3] OBTENER CATÁLOGOS (Fuerzas, Categorías)
                // ════════════════════════════════════════════════════════════════════════

                var fuerzas = (await _catalogsBusinessLogic.ObtenerFuerzas()).Data;
              

                // ════════════════════════════════════════════════════════════════════════
                // [4] UNIR DATOS EN MEMORIA (simple join de datos ya procesados)
                // ════════════════════════════════════════════════════════════════════════
                // Los datos del SP ya vienen procesados y listos
                // Solo necesitamos agregar información de afiliados y catálogos

                return result
                    .GroupJoin(
                        listAfiliados,
                        s => s.Identificacion,
                        s => s.Identificacion,
                        (rep, cli) =>
                        {
                            var cliente = cli?.FirstOrDefault();

                            // Agregar datos de afiliado
                            rep.NombreCompleto = cliente?.NombreCompleto ?? "";
                            rep.IdFuerza = cliente?.IdFuerza ?? 0;
                            rep.IdGrado = cliente?.IdGrado ?? 0;
                            rep.IdCategoria = cliente?.IdCategoria ?? 0;

                            // Agregar descripciones de catálogos
                            rep.Fuerza = fuerzas.FirstOrDefault(x => x.Id == rep.IdFuerza)?.Descripcion;
                           

                            return rep;
                        })
                    // Aplicar filtros finales (opcional, pueden estar ya en el SP)
                    .Where(x =>
                        (parametros.Fuerza == null || parametros.Fuerza == x.IdFuerza) 
                    )
                    .BuildSuccess();
            }
            catch (Exception ex)
            {
                // Log de error (implementar según tu framework)
                // _logger.LogError(ex, "Error generando reporte general de cartera");

                return new Response<IEnumerable<CarteraReporteGeneralDto>>
                {
                    ReturnMessage = "No se pudo generar el reporte",
                    IsSuccess = false
                };
            }
        }
        //public async Task<Response<IEnumerable<CarteraReporteGeneralDto>>> GenerarReporteGeneralCartera(ReporteGeneralCarteraParametersDto parametros)
        //{
        //    try
        //    {
        //        var result = _mapper.Map<IEnumerable<CarteraReporteGeneralDto>>(await _reportesRepository.GenerarReporteGeneralCartera(parametros));
        //        var parametroMesSiguiente = new ReporteGeneralCarteraParametersDto()
        //        {
        //            Mes = parametros.Mes,
        //            Anio = parametros.Anio,
        //            PeriodoActual = parametros.PeriodoActual,
        //            Fuerza = parametros.Fuerza,
        //            Categoria = parametros.Categoria,
        //            NumeroCredito = parametros.NumeroCredito,
        //            TipoCredito = parametros.TipoCredito
        //        };
        //        parametroMesSiguiente.Mes = parametroMesSiguiente.Mes + 1;
        //        if (parametroMesSiguiente.Mes == 13)
        //        {
        //            parametroMesSiguiente.Mes = 1;
        //            parametroMesSiguiente.Anio = parametroMesSiguiente.Anio + 1;
        //        }
        //        var resultMesSiguientes = _mapper.Map<IEnumerable<CarteraReporteGeneralDto>>(await _reportesRepository.GenerarReporteGeneralCartera(parametroMesSiguiente));
        //        if (resultMesSiguientes.Count() == 0)
        //        {
        //            result = _mapper.Map<IEnumerable<CarteraReporteGeneralDto>>(await _reportesRepository.GenerarReporteGeneralCarteraNew(parametros));
        //        }
        //        var docs = result.Select(x => x.Identificacion).Distinct().ToList();
        //        var datosConMora = result.Where(r => r.DiasMora > 0).ToList();

        //        // ===== OBTENER TODAS LAS NOVEDADES EN BATCH (evita N+1) =====
        //        var creditoIds = result.Select(x => x.CreditoId).Distinct().ToList();
        //        var novedadesLista = await _carteraBusinessLogic.ObtenerNovedadesEnBatch(creditoIds);

        //        var fechaPeriodo = new DateTime((int)parametros.Anio, (int)parametros.Mes,
        //            DateTime.DaysInMonth((int)parametros.Anio, (int)parametros.Mes));

        //        // Filtrar el diccionario por fecha Y excluir entradas vacías
        //        Dictionary<Guid, IEnumerable<NovedadCartera>> novedadesPorCredito = novedadesLista
        //            .Select(kvp => new
        //            {
        //                kvp.Key,
        //                Novedades = kvp.Value.Where(n => CumpleFiltroFecha(n,fechaPeriodo)).ToList()
        //            })
        //            .Where(x => x.Novedades.Any())  // ← Excluir créditos sin novedades en esa fecha
        //            .ToDictionary(
        //                x => x.Key,
        //                x => (IEnumerable<NovedadCartera>)x.Novedades
        //            );


        //        // ===== OBTENER TODOS LOS PRODUCTOS EN BATCH (evita consultas duplicadas) =====
        //        var identificaciones = result.Select(x => x.Identificacion).Distinct().ToList();
        //        if (resultMesSiguientes.Count() > 0)
        //        {
        //            // Crear lista de tuplas (más ligero que objetos)
        //            var creditos = result
        //                .Select(x => (x.NumeroCredito, x.IdTipoCredito))
        //                .Distinct()
        //                .ToList();

        //            var planesPorCredito = await _carteraBusinesLogic.ObtenerPlanPagoEnBatch(creditos, fechaPeriodo);
        //            await ProcesarReporteGeneral(result.ToList(), novedadesPorCredito, planesPorCredito);
        //        }
        //        else
        //        {
        //            var productosPorIdentificacion = await _carteraBusinesLogic.ObtenerProductosEnBatch(identificaciones);
        //            await ProcesarNovedadesYLeasing(result.ToList(), novedadesPorCredito, productosPorIdentificacion);
        //        }

        //        var listAfiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(docs)).Data;

        //        var fuerzas = (await _catalogsBusinessLogic.ObtenerFuerzas()).Data;
        //        var categorias = (await _catalogsBusinessLogic.ObtenerCategorias()).Data;

        //        return
        //            result.GroupJoin(listAfiliados, s => s.Identificacion, s => s.Identificacion, (rep, cli) =>
        //            {
        //                var cliente = cli?.FirstOrDefault();
        //                rep.NombreCompleto = cliente?.NombreCompleto ?? "";
        //                rep.IdFuerza = cliente?.IdFuerza ?? 0;
        //                rep.IdGrado = cliente?.IdGrado ?? 0;
        //                rep.IdCategoria = cliente?.IdCategoria ?? 0;

        //                rep.Fuerza = fuerzas.FirstOrDefault(x => x.Id == rep.IdFuerza)?.Descripcion;
        //                rep.Categoria = categorias.FirstOrDefault(x => x.Id == rep.IdCategoria)?.Descripcion;
        //                return rep;
        //            })
        //            .Where(x =>
        //                (parametros.Fuerza == null || parametros.Fuerza == x.IdFuerza) &&
        //                (parametros.Categoria == null || parametros.Categoria == x.IdCategoria)
        //            ).BuildSuccess();

        //    }
        //    catch (Exception)
        //    {
        //        return new() { ReturnMessage = "No se pudo generar el reporte", IsSuccess = false };
        //    }
        //}

        public async Task<Response<IEnumerable<CarteraReporteGeneralMoraDto>>> GenerarReporteGeneralMora(ReporteGeneralMoraParametersDto parametros)
        {
            try
            {
                var result = _mapper.Map<IEnumerable<CarteraReporteGeneralMoraDto>>(await _reportesRepository.GenerarReporteGeneralMora(parametros));
                var docs = result.Select(x => x.NumeroIdentificacion).Distinct().ToList();
                var listAfiliados = (await _clientRequestProvider.ObtenerInformacionClientesPorIdentificacion(docs)).Data;

                return
                    result.GroupJoin(listAfiliados, s => s.NumeroIdentificacion, s => s.Identificacion, (rep, cli) =>
                    {
                        var cliente = cli?.FirstOrDefault();
                        rep.NombreCompleto = cliente?.NombreCompleto ?? "";
                        return rep;
                    }).BuildSuccess();
            }
            catch (Exception)
            {
                return new() { ReturnMessage = "No se pudo generar el reporte", IsSuccess = false };
            }
        }

        private async Task<byte[]> BuildReportePazSalvo(ReportePazSalvo data)
        {
            var path = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "Cartera", "PazSalvo");

            var html = new StringBuilder(await File.ReadAllTextAsync(System.IO.Path.Join(path, "main-content.html")));
            var header = await File.ReadAllTextAsync(System.IO.Path.Join(path, "header.html"));
            var footer = await File.ReadAllTextAsync(System.IO.Path.Join(path, "footer.html"));

            var cultureEs = CultureInfo.GetCultureInfo("es-ES");

            html.Replace("[@cli_nombres]", data.NOMBRE_CLIENTE);
            html.Replace("[@cli_identificacion]", FormatearIdentificacion(data.IDENTIFICACION_CLIENTE));
            html.Replace("[@valor_desembolso]", FormatearMonto(data.VALOR_DESEMBOLSO, cultureEs));
            html.Replace("[@plazo]", FormatearPlazoTotal(data.PLAZO_TOTAL, cultureEs));
            html.Replace("[@fecha_pago_inicial]", FormatearFecha(data.FECHA_DESEMBOLSO, cultureInfo: cultureEs));
            html.Replace("[@fecha_expedicion]", FormatearFecha(data.FECHA_EXPEDICION_REPORTE, extensa: true, cultureEs));
            html.Replace("[@ciudad_expedicion]", data.CIUDAD_EXPEDICION);
            html.Replace("[@jefe_area]", data.JEFE_AREA_CARTERA);
            html.Replace("[@elaboro]", data.PROYECTO_ELABORO);

            var htmlToPdf = new HtmlToPdf();
            htmlToPdf.Options.DisplayHeader = true;
            htmlToPdf.Options.DisplayFooter = true;
            htmlToPdf.Options.PdfPageSize = PdfPageSize.Letter;
            htmlToPdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdf.Options.MarginTop = 72;
            htmlToPdf.Options.MarginRight = 72;
            htmlToPdf.Options.MarginBottom = 36;
            htmlToPdf.Options.MarginLeft = 72;
            htmlToPdf.Options.JavaScriptEnabled = false;
            htmlToPdf.Options.DenyLocalFileAccess = true;
            htmlToPdf.Footer.Height = 82;

            var headerHtml = new PdfHtmlSection(htmlString: header, null);
            var footerHtml = new PdfHtmlSection(htmlString: footer, null);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

            htmlToPdf.Header.Add(headerHtml);
            htmlToPdf.Footer.Add(footerHtml);

            var doc = htmlToPdf.ConvertHtmlString(html.ToString());
            return doc.Save();
        }

        private string FormatearMonto(decimal monto, CultureInfo cultureInfo = null)
        {
            var format = new NumberFormatInfo()
            {
                NumberGroupSeparator = ".",
                NumberDecimalSeparator = ",",
            };

            var cifra = Convert.ToInt64(monto);
            var valorLetras = cifra.ToWords(cultureInfo).ToUpper();

            return $"{valorLetras} PESOS M/CTE {cifra.ToString("($ #,##0.00)", format)}";
        }
        private string FormatearIdentificacion(string identificacion)
        {
            if (long.TryParse(identificacion, NumberStyles.Integer, CultureInfo.InvariantCulture, out long numero))
            {
                var format = new NumberFormatInfo()
                {
                    NumberGroupSeparator = ".",
                };

                return numero.ToString("#,##0", format);

            }

            return identificacion;
        }

        private string FormatearPeriodo(int numero, string periodo, string plural = "s", CultureInfo cultureInfo = null)
        {
            var texto = $"{numero.ToWords(WordForm.Abbreviation, cultureInfo)} ({numero}) {periodo}";

            return numero switch
            {
                _ when numero == 1 => texto,
                _ when numero > 1 => $"{texto}{plural}",
                _ => string.Empty
            };
        }

        private string FormatearPlazoTotal(int plazo, CultureInfo cultureInfo = null)
        {
            var (plazoAnios, plazoMeses) = (plazo / 12, plazo % 12);

            var plazoTexto = (anios: FormatearPeriodo(plazoAnios, "año", cultureInfo: cultureInfo),
                              meses: FormatearPeriodo(plazoMeses, "mes", "es", cultureInfo));

            return plazoTexto switch
            {
                var (anios, meses) when string.IsNullOrEmpty(anios) => meses,
                var (anios, meses) when string.IsNullOrEmpty(meses) => anios,
                var (anios, meses) => $"{anios} y {meses}"
            };
        }

        private string FormatearFecha(DateTime date, bool extensa = false, CultureInfo cultureInfo = null)
        {
            if (!extensa)
                return date.ToString(@"dd \de MMMM \del año yyyy", cultureInfo);

            var diaTexto = $"a los {FormatearPeriodo(date.Day, "día", cultureInfo: cultureInfo)}";

            return $"{diaTexto} del mes de {date.ToString(@"MMMM \del año yyyy", cultureInfo)}";
        }

        public async Task<Response<FileResult>> GenerarReportePazSalvo(ReportePazSalvoParemetersDto parameters)
        {
            var report = await _reportesRepository.GenerarReportePazSalvo(parameters);

            if (report == null)
                return $"No se encontraron datos relacionados al número de crédito".BuildError<FileResult>();

            if (!report.GENERAR)
                return $"{report.OBSERVACION}".BuildError<FileResult>();

            var afiliado = (await _iReportesRequestProvider.ObtenerNombresCliente(report.IDENTIFICACION_CLIENTE))?.Data ?? new();
            report.NOMBRE_CLIENTE = afiliado.NombreCompleto;

            byte[] reportBytes = await BuildReportePazSalvo(report);
            var filename = $"Paz Salvo {parameters.NumeroCredito}.pdf";
            var file = await GenerarArchivoPDF(reportBytes, filename);

            return file.BuildSuccess();
        }

        public async Task<Response<FileResult>> GenerarReporteRetefuente(ReporteAnioGravableParametersDto parameters)
        {
            try
            {
                if (parameters.NumeroCredito is null)
                    return $"No se especifico el número de crédito".BuildError<FileResult>();

                var data = (await _reportesRepository.GenerarReporteRetefuenteAnioGravable(parameters)).FirstOrDefault();

                if (data == null)
                    return $"No se encontraron datos relacionados al número de crédito".BuildError<FileResult>();


                var afiliado = (await _iReportesRequestProvider
                    .ObtenerNombresClientesPorDocumentos(new[] { data.CLI_IDENTIFICACION }.ToList()))?.Data.FirstOrDefault() ?? new();
                data.NOMBRE_TITULAR = afiliado?.NombreCompletoInvertido ?? "";


                var (interesMoraPagado, saldoInteresRemanente) = await CalcularDatosRetefuente(data);
                var report = BuildReportRetefuente(data, interesMoraPagado, saldoInteresRemanente);

                if (report.Error)
                    return report.Message.BuildError<FileResult>();

                return (await GenerarArchivoPDF(report.Bytes, report.Nombre)).BuildSuccess();
            }
            catch (Exception)
            {
                return $"Error al generar el certificado".BuildError<FileResult>();
            }
        }
        public async Task<FileResult> GenerarReporteMasivoRetefuente(
            ReporteAnioGravableParametersDto parameters,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = (await _reportesRepository.GenerarReporteRetefuenteAnioGravable(parameters)).ToList();
                var documentos = result.Select(x => x.CLI_IDENTIFICACION).Distinct().ToList();
                var afiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(documentos))?.Data ?? new();
                var reportes = result.GroupJoin(afiliados, s => s.CLI_IDENTIFICACION, s => s.Documento, (rep, cli) =>
                {
                    var cliente = cli?.FirstOrDefault();
                    rep.NOMBRE_TITULAR = cliente?.NombreCompletoInvertido ?? "";
                    return rep;
                });

                // Devuelves el archivo directamente
                var datosRetefuentePorCredito = new Dictionary<long, (decimal InteresMoraPagado, decimal SaldoInteresRemanente)>();
                foreach (var rep in reportes)
                {
                    datosRetefuentePorCredito[rep.CRE_NRO_CREDITO] = await CalcularDatosRetefuente(rep);
                }
                var datosReporte = BuilReportesMasivoRetefuente(reportes, datosRetefuentePorCredito).ToList();
                var data = ToAsyncEnumerable(datosReporte);
                return await ProcesarReporteMasivo(new()
                {
                    DatosReportes = data,
                    Total = result.Count,
                    IdProceso = parameters.IdProceso
                });
            }
            catch (OperationCanceledException)
            {
                await ActualizarEstadoProcesoGeneracionReporte(parameters.IdProceso, EstadoProcesoReporte.CANCELADO);
                throw; // para que el controlador sepa que fue cancelado
            }
            catch (Exception e)
            {
                await ActualizarEstadoProcesoGeneracionReporte(parameters.IdProceso,
                    EstadoProcesoReporte.FALLIDO + e.Message + e.InnerException);
                throw;
            }
        }
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                yield return item;
                await Task.Yield(); // cede el control para simular asincronía
            }
        }

        private async Task<FileResult> ProcesarReporteMasivo(
           ParametroReporteMasivo parameters,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var proceso = (await _reportesRepository.ConsultarProcesoGeneracionReporte(
                    new() { IdProceso = parameters.IdProceso }))
                    .FirstOrDefault();

                if (proceso == null)
                    return null;

                using var memoryStream = new MemoryStream();
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    await ProcesarEntradaZip(zip, parameters, cancellationToken);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var result = new FileContentResult(memoryStream.ToArray(), "application/zip")
                {
                    FileDownloadName = $"reporte_{parameters.IdProceso}.zip"
                };

                // Si igual quieres marcar el proceso como completado en BD
                await ActualizarEstadoACompletado(proceso.PGR_ID);

                return result;
            }
            catch (Exception e) { throw; }
        }

        private static async Task ProcesarEntradaZip(
            ZipArchive zip,
            ParametroReporteMasivo parameters,
            CancellationToken cancellationToken = default)
        {
            await foreach (var data in parameters.DatosReportes)
            {
                if (!data.Error)
                {
                    var entry = zip.CreateEntry(data.Nombre, CompressionLevel.Optimal);
                    using var zipStream = entry.Open();
                    zipStream.Write(data.Bytes);
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        // ═══════════════════════════════════════════════════════════
        // Cálculo compartido de los 2 valores nuevos -- lo usan ambos
        // flujos (individual y masivo).
        // ═══════════════════════════════════════════════════════════
        private async Task<(decimal InteresMoraPagado, decimal SaldoInteresRemanente)> CalcularDatosRetefuente(
            ReporteAnioGravable reporte)
        {
            var anioFiscal = reporte.FECHA_CERTIFICACION.Year;
            var primerDiaAnio = new DateTime(anioFiscal, 1, 1);
            var ultimoDiaAnio = new DateTime(anioFiscal, 12, 31);

            var requestPlanPago = new ObtenerProductoCartera
            {
                Nro_Credito = reporte.CRE_NRO_CREDITO,
                Tcr_Id = reporte.TCR_ID
            };
            var credito = await _carteraRepository.ObtenerCreditoCartera(requestPlanPago);
            var listPayment = await _carteraRepository.ObtenerAplicacionPago(credito.CRE_ID);
            var interesMoraPagadoAnio = listPayment
                .Where(p => p.CPG_ID_REVERSION == Guid.Empty
                         && (p.CC_ESTADO_ID == 1 || p.CC_ESTADO_ID == 2)
                         && p.CPG_FECHA_PAGO.Date >= primerDiaAnio
                         && p.CPG_FECHA_PAGO.Date <= ultimoDiaAnio)
                .Sum(p => p.CPG_ABONO_INTERES_MORA);

            decimal saldoInteresRemanenteAnio = 0;
            if (reporte.TCR_ID == 1)
            {
                var planPago = await _carteraBusinessLogic.ObtenerPlanPagosCredito(requestPlanPago);
                saldoInteresRemanenteAnio = planPago.Data.PlanPago
                    .Where(x => x.Fecha <= ultimoDiaAnio)
                    .Sum(x => x.InteresPorPagar);
            }

            return (interesMoraPagadoAnio, saldoInteresRemanenteAnio);
        }

        // ═══════════════════════════════════════════════════════════
        // VERSIÓN SÍNCRONA -- la usa el flujo MASIVO (dentro del
        // generador IEnumerable/yield return). Recibe los 2 valores
        // ya calculados, no consulta nada.
        // ═══════════════════════════════════════════════════════════
        private ArchivoReporte BuildReportRetefuente(
            ReporteAnioGravable reporte,
            decimal interesMoraPagado,
            decimal saldoInteresRemanente,
            bool incluirNumeroCredito = false)
        {
            var sufijo = incluirNumeroCredito ? $" - {reporte.CRE_NRO_CREDITO}" : "";
            var nombreReporte = $"Retefuente Año Gravable {reporte.FECHA_CERTIFICACION.Year} - {reporte.CLI_IDENTIFICACION}{sufijo}.pdf";
            try
            {
                var cultureEs = CultureInfo.GetCultureInfo("es-ES");
                var DatosReporte = new ReportDataSource("DatosReporte", new[]
                {
            new {
                  reporte.CUOTAS_PERIODO,
                  reporte.COSTOS_FINANCIEROS,
                  reporte.INTERESES_PERIODO,
                  reporte.FECHA_CERTIFICACION,
                  reporte.NOMBRE_TITULAR,
                  ANIO = reporte.FECHA_CERTIFICACION.Year,
                  FECHA_DESEMBOLSO = reporte.CRE_FECHA_DESEMBOLSO,
                  TIPO_CREDITO = reporte.TCR_ID,
                  LINEA_CREDITO = reporte.CLC_DESCRIPCION ?? reporte.TCR_DESCRIPCION,
                  MONTO_INICIAL = reporte.CRE_VALOR_DESEMBOLSO,
                  NUMERO_CONTRATO = reporte.CRE_NRO_CREDITO,
                  NUMERO_IDENTIFICACION = reporte.CLI_IDENTIFICACION,
                  SALDO = reporte.SALDO_CAPITAL,
                  FECHA_EXPEDICION = FormatearFecha(reporte.FECHA_EXPEDICION_REPORTE, extensa: true, cultureEs).ToUpper(),
                  INTERES_MORA_PAGADO = interesMoraPagado,
                  SALDO_INTERES_REMANENTE = saldoInteresRemanente
            }
        });
                var path = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "Cartera", "CertificacionAnioGravable.rdlc");
                var rdlcReport = new Microsoft.Reporting.NETCore.LocalReport();
                rdlcReport.ReportPath = path;
                rdlcReport.DataSources.Add(DatosReporte);
                return new()
                {
                    Bytes = rdlcReport.Render("PDF"),
                    Nombre = nombreReporte
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Nombre = nombreReporte,
                    Message = "No se pudo generar el reporte",
                    Error = true,
                    Exception = ex
                };
            }
        }

        // ═══════════════════════════════════════════════════════════
        // VERSIÓN ASYNC -- la usa el flujo INDIVIDUAL. Calcula y llama
        // a la versión síncrona de arriba.
        // ═══════════════════════════════════════════════════════════
        private async Task<ArchivoReporte> BuildReportRetefuenteAsync(
            ReporteAnioGravable reporte, bool incluirNumeroCredito = false)
        {
            var (interesMoraPagado, saldoInteresRemanente) = await CalcularDatosRetefuente(reporte);
            return BuildReportRetefuente(reporte, interesMoraPagado, saldoInteresRemanente, incluirNumeroCredito);
        }
        private static async Task<List<T>> ToListAsync<T>(
            IAsyncEnumerable<T> source,
            CancellationToken cancellationToken = default)
        {
            var list = new List<T>();
            await foreach (var item in source.WithCancellation(cancellationToken))
            {
                list.Add(item);
            }
            return list;
        }


        private IEnumerable<ArchivoReporte> BuilReportesMasivoRetefuente(
            IEnumerable<ReporteAnioGravable> reportes,
            Dictionary<long, (decimal InteresMoraPagado, decimal SaldoInteresRemanente)> datosRetefuentePorCredito)
        {
            var identificacionRepetida = reportes
                .Select(x => x.CLI_IDENTIFICACION)
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();
            foreach (var reporte in reportes)
            {
                var incluirNumeroCredito = identificacionRepetida.Contains(reporte.CLI_IDENTIFICACION);
                var (interesMoraPagado, saldoInteresRemanente) = datosRetefuentePorCredito[reporte.CRE_NRO_CREDITO];
                yield return BuildReportRetefuente(reporte, interesMoraPagado, saldoInteresRemanente, incluirNumeroCredito);
            }
        }




        // Reporte Cierre Mensual Contables

        public async Task<Response<IEnumerable<TotalReporteCierreMensualContableDto>>> GenerarReporteTotalCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto)
        {
            try
            {
                var report = await _reportesRepository.GenerarReporteTotalCierreMensualContable(parametersDto);
                return new() { Data = _mapper.Map<IEnumerable<TotalReporteCierreMensualContableDto>>(report) };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
            }
        }

        public async Task<Response<ResultadoReportContable>> GenerarReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto)
        {
            try
            {
                var list = new List<ReporteContableDto>();

                await foreach (var item in _reportesRepository.GenerarReporteCierreMensualContable(parametersDto))
                    list.Add(_mapper.Map<ReporteContableDto>(item));

                return new() { Data = new(list) };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "No se pudo recuperar la información del reporte" };
            }
        }

        public async Task<FileResult> GenerarExcelReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto)
        {
            try
            {
                var columns = ObtenerColumnasReporteCierreMensualContable();
                var data = ObtenerDatosReporteCierreMensualContable(parametersDto);

                return await _reportesExcel.GenerarExcel(new ReportRequestExcelFormatAsyncDto(columns, data));
            }
            catch (Exception)
            {
                throw new Exception("No se pudo generar el reporte");
            }
        }

        private static ColumnHeaderInfo[] ObtenerColumnasReporteCierreMensualContable()
        {
            return new ColumnHeaderInfo[] {
                "Tipo Operación",
                "Descripción Operación",
                ("Fecha Operación", ColumnFormatType.Fecha),
                "Número Identificación",
                "Número Crédito",
                "Cuenta NIF",
                "Descripción Cuenta",
                ("Débito", ColumnFormatType.Decimal),
                ("Crédito", ColumnFormatType.Decimal),
            };
        }

        private async IAsyncEnumerable<string[]> ObtenerDatosReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto)
        {
            await foreach (var item in _reportesRepository.GenerarReporteCierreMensualContable(parametersDto))
            {
                var data = _mapper.Map<ReporteContableDto>(item);

                yield return new string[]
                {
                    data.TipoOperacion.ToString(),
                    data.DescripcionOperacion,
                    data.FechaOperacion.ToString("yyyy/MM/dd"),
                    data.NumeroIdentificacion,
                    data.NumeroCredito.ToString(),
                    data.CuentaNif,
                    data.DescripcionCuenta,
                    data.Debito.ToString(CultureInfo.InvariantCulture),
                    data.Credito.ToString(CultureInfo.InvariantCulture),
                };
            }
        }

        public async Task<Response<IEnumerable<MovimientoComprobanteEntity>>> GenerarCausacionesComprobante(MovimientoReportesCartera movimientoReportes)
        {
            try
            {
                ReporteContableParametersDto parametros = new ReporteContableParametersDto();
                parametros.FechaInicial = movimientoReportes.FechaComprobante;
                parametros.FechaFinal = movimientoReportes.FechaComprobante;

                if (movimientoReportes.Tipo == 1)
                {
                    IEnumerable<ReporteContableDto> listaMovimientos = await ObtenerReporteCausaciones(parametros);

                    var listaRetorna = MapearReporteCausaciones(listaMovimientos);

                    return new Response<IEnumerable<MovimientoComprobanteEntity>>
                    {
                        IsSuccess = true,
                        ReturnMessage = "Comprobante Creado: " + listaRetorna.FirstOrDefault().CC_ID,
                        Data = listaRetorna
                    };
                }
                else
                {

                    IEnumerable<ReporteContable> listaMovimientos = await _reportesRepository.GenerarReporteContablePagos(parametros);

                    var listaRetorna = MapearReporteContablePagos(listaMovimientos);

                    return new Response<IEnumerable<MovimientoComprobanteEntity>>
                    {
                        IsSuccess = true,
                        ReturnMessage = "Comprobante Creado: " + listaRetorna.FirstOrDefault().CC_ID,
                        Data = listaRetorna
                    };
                }

            }
            catch (Exception ex)
            {

                return new Response<IEnumerable<MovimientoComprobanteEntity>>
                {
                    IsSuccess = false,
                    ReturnMessage = ex.Message
                };
            }
        }

        private IEnumerable<MovimientoComprobanteEntity> MapearReporteCausaciones(IEnumerable<ReporteContableDto> reporteCausaciones)
        {
            var movimientosComprobante = new List<MovimientoComprobanteEntity>();

            foreach (var movimiento in reporteCausaciones)
            {
                MovimientoComprobanteEntity movimientoComprobante = new MovimientoComprobanteEntity
                {
                    MCC_CUENTA_CONTABLE = Convert.ToInt32(movimiento.CuentaNif),
                    MCC_DESCRIPCION_CUENTA = movimiento.DescripcionCuenta,
                    MCC_CENTRO_COSTO = 0,
                    MCC_DETALLE = 10,
                    MCC_TIPO = movimiento.Credito > 0 ? "C" : "D",
                    MCC_MONTO = movimiento.Credito > 0 ? movimiento.Credito : movimiento.Debito
                };

                movimientosComprobante.Add(movimientoComprobante);
            }

            return movimientosComprobante;
        }

        private IEnumerable<MovimientoComprobanteEntity> MapearReporteContablePagos(IEnumerable<ReporteContable> reporteCausaciones)
        {
            var movimientosComprobante = new List<MovimientoComprobanteEntity>();

            foreach (var movimiento in reporteCausaciones)
            {
                MovimientoComprobanteEntity movimientoComprobante = new MovimientoComprobanteEntity
                {
                    MCC_CUENTA_CONTABLE = Convert.ToInt32(movimiento.CUENTAS_NIF),
                    MCC_DESCRIPCION_CUENTA = movimiento.DESCRIPCION_CUENTA,
                    MCC_CENTRO_COSTO = 0,
                    MCC_DETALLE = Convert.ToDecimal(movimiento.DESCRIPCION_OPERACION),
                    MCC_TIPO = movimiento.CREDITO > 0 ? "C" : "D",
                    MCC_MONTO = movimiento.DEBITO > 0 ? movimiento.CREDITO : movimiento.DEBITO
                };

                movimientosComprobante.Add(movimientoComprobante);
            }

            return movimientosComprobante;
        }

        public async Task<FileContentResult> ReporteTransUnionPropiedadesInformacionComercial(DatosPersonalesProspeccionDto request)
        {
            try
            {
                var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.TipoIdentificacion, request.Identificacion)).Data;
                var datosFinancieros = (await _creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.Identificacion)).Data;

                ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                {
                    Apellidos = recursos.PrimerApellido,
                    FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                    Identificacion = request.Identificacion,
                    SalarioMensual = datosFinancieros.TotalIngresos,
                    TipoIdentificacion = request.TipoIdentificacion,
                    Telefono = recursos.Telefono,
                    TraerHistorialCrediticio = "Y",
                    SLS_ID = request.SLS_ID
                };

                var prospeccionTransUnion = await _gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                var respuestaTU = await ReporteTransUnionPropiedades(prospeccionTransUnion.Data.response);

                return new FileContentResult(respuestaTU.Data.ArchivoBytes, "application/pdf")
                {
                    FileDownloadName = respuestaTU.Data.FileName
                };

            }
            catch (Exception ex)
            {
                return new FileContentResult(null, ex.Message);
            }
        }


        public async Task<FileResult> ReporteSolicitudCreditoVivienda(PeticionCreditoBasicaDto peticionCredito)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            try
            {
                var peticion = this._mapper.Map<PeticionCreditoBasica>(peticionCredito);
                var credito = await this._creditoRepository.ObtenerSolicCreditoBasica(peticion);

                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\SolicitudCredito\\ReporteCreditoSolicitudProducto.rdlc";
                report.ReportPath = reportPath;

                SimulacionCliente simulacionCliente = await this._creditoRepository.ConsultarSimulacionCliente(credito.SOB_NUMERO_DOCUMENTO);
                Response<ClienteDto> clienteDetallado = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(credito.TID_ID, credito.SOB_NUMERO_DOCUMENTO);
                SolicitudCredito solicitudGeneralCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(credito.TID_ID, credito.SOB_NUMERO_DOCUMENTO);


                report = await ParametrosReporteSolicitudCreditoVivienda(report, credito, clienteDetallado, simulacionCliente, peticionCredito, solicitudGeneralCredito);
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

        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosReporteSolicitudCreditoVivienda(
            Microsoft.Reporting.NETCore.LocalReport reportParam,
            RespuestaCreditoBasica infoBasicaSolicitud,
            Response<ClienteDto> clienteDetallado,
            SimulacionCliente simulacionCliente,
            PeticionCreditoBasicaDto peticionCredito,
            SolicitudCredito solicitudGeneralCredito)
        {
            try
            {
                if (infoBasicaSolicitud == null) infoBasicaSolicitud = new RespuestaCreditoBasica();

                infoBasicaSolicitud.CREDITO_HIPOTECARIO = infoBasicaSolicitud?.TIPO_CREDITO == 2 ? "X" : "";
                infoBasicaSolicitud.CREDITO_LEASING = infoBasicaSolicitud?.TIPO_CREDITO == 1 ? "X" : "";

                Departamento departamento = null;
                try
                {
                    departamento = await _catalogsRepository.ObtenerDepartamentoPorId(simulacionCliente?.DPD_INMUEBLE ?? 0);
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerDepartamentoPorId inmueble: "); }

                Ciudad ciudadSolicitud = null;
                try
                {
                    ciudadSolicitud = await _catalogsRepository.ObtenerCiudadPorId(simulacionCliente?.DPC_INMUEBLE ?? 0);
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerCiudadPorId inmueble: "); }

                infoBasicaSolicitud.CIUDAD_SOLICITUD = ciudadSolicitud?.DPC_NOMBRE ?? "";

                Departamento departamentoSolicitudResidencia = null;
                Ciudad ciudadSolicitudResidencia = null;
                try
                {
                    departamentoSolicitudResidencia = await _catalogsRepository.ObtenerDepartamentoPorId(clienteDetallado?.Data?.IdDepartamentoResidencia ?? 0);
                    ciudadSolicitudResidencia = await _catalogsRepository.ObtenerCiudadPorDepartamentoCiudad(clienteDetallado?.Data?.IdDepartamentoResidencia ?? 0, clienteDetallado?.Data?.IdCiudadResidencia ?? 0);
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerDepartamento/Ciudad Residencia: "); }

                infoBasicaSolicitud.CIUDAD_RESIDENCIA = ciudadSolicitudResidencia?.DPC_NOMBRE ?? "";
                infoBasicaSolicitud.DEPARTAMENTO_RESIDENCIA = departamentoSolicitudResidencia?.DPD_NOMBRE ?? "";

                infoBasicaSolicitud.ACTIVIDAD = clienteDetallado?.Data?.IdTipoActor == 14 ? "Pensionado" : "Servidor Público";

                UnidadEjecutora unidad = null;
                try
                {
                    unidad = await _carteraRepository.ConsultarUnidadEjecutoraPorId(clienteDetallado?.Data?.IdUnidadEjecutora ?? 0);
                }
                catch (Exception ex){ _logger.LogError(ex, "Error en ConsultarUnidadEjecutoraPorId "); }

                infoBasicaSolicitud.EMPRESA_TRABAJA = unidad?.UEJ_NOMBRE ?? "";

                MedioAtencionCredito mediosRecepcion = null;
                try
                {
                    mediosRecepcion = await _catalogsRepository.ConsultarMediosAtencionPorId(simulacionCliente.MEDIO_ATENCION_CREDITO);
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ConsultarMediosAtencionPorId. "); }

                infoBasicaSolicitud.MEDIO_CONTACTO = mediosRecepcion?.MCV_DESCRIPCION ?? "";

                PeticionCreditoBasica creditoBasica = new PeticionCreditoBasica
                {
                    SOC_ID = peticionCredito?.SOC_ID ?? Guid.Empty,
                    IND = 2
                };

                InformacionConyugue informacionConyugue = null;
                try
                {
                    informacionConyugue = await _creditoRepository.ObtenerSolicCreditoConyugue(creditoBasica);
                    if (informacionConyugue != null)
                    {
                        var tipoIdentificacion = await _creditoRepository.ObtenerTipoIdentificacionPorId(informacionConyugue.TID_ID);
                        infoBasicaSolicitud.TIPO_IDENTIFICACION_CONYUGUE = tipoIdentificacion?.TID_DESCRIPCION ?? "";
                        var ciudadExpedicionConyugue = await _catalogsRepository.ObtenerCiudadPorId(infoBasicaSolicitud?.DPC_ID_EXP ?? 0);
                        infoBasicaSolicitud.CIUDAD_EXPEDICION_CONYUGUE = ciudadExpedicionConyugue?.DPC_NOMBRE ?? "";
                    }
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerSolicCreditoConyugue: "); throw; }

                SolicCreditoRefererncias referenciaPersonal = null;
                try
                {
                    referenciaPersonal = await _creditoRepository.ObtenerSolicCreditoReferenciasPorTipo(peticionCredito?.SOC_ID ?? Guid.Empty, "Personal");
                    var ciudadReferenciaPersonal = await _catalogsRepository.ObtenerCiudadPorId(referenciaPersonal?.DPC_ID ?? 0);
                    infoBasicaSolicitud.CIUDAD_REFERENCIA_PERSONAL = ciudadReferenciaPersonal?.DPC_NOMBRE ?? "";
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en Error en ObtenerSolicCreditoReferenciasPorTipo Personal: "); throw; }

                SolicCreditoRefererncias referenciaFamiliar = null;
                try
                {
                    referenciaFamiliar = await _creditoRepository.ObtenerSolicCreditoReferenciasPorTipo(peticionCredito?.SOC_ID ?? Guid.Empty, "Familiar");
                    var ciudadReferenciaFamiliar = await _catalogsRepository.ObtenerCiudadPorId(referenciaFamiliar?.DPC_ID ?? 0);
                    infoBasicaSolicitud.CIUDAD_REFERENCIA_FAMILIAR = ciudadReferenciaFamiliar?.DPC_NOMBRE ?? "";
                }
                catch (Exception ex) { _logger.LogError(ex,"Error en ObtenerSolicCreditoReferenciasPorTipo Familiar: " ); throw; }

                DatosFinancierosPersonales datosFinancieros = null;
                try
                {
                    datosFinancieros = await _creditoRepository.ObtenerDatosFinancierosPersonales(clienteDetallado?.Data?.Identificacion ?? "");
                    infoBasicaSolicitud.OTROS_INGRESOS =
                        (datosFinancieros?.SDF_VALOR_OTROS_INGRESOS ?? 0) +
                        (datosFinancieros?.SDF_VALOR_OTROS_INGRESOS1 ?? 0) +
                        (datosFinancieros?.SDF_VALOR_OTROS_INGRESOS2 ?? 0) +
                        (datosFinancieros?.SDF_VALOR_OTROS_INGRESOS3 ?? 0) +
                        (datosFinancieros?.SDF_VALOR_OTROS_INGRESOS4 ?? 0) +
                        (datosFinancieros?.SDF_VALOR_OTROS_INGRESOS5 ?? 0);
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerDatosFinancierosPersonales: "); throw; }


                PeticionSolicitudCreditoProducto peticion = new PeticionSolicitudCreditoProducto();
                peticion.SOC_ID = peticionCredito.SOC_ID;
                peticion.IND = 2;

                RespuestaCreditoProducto resultadoInmuebleInformacion = await _creditoRepository.ObtenerSolicCreditoProducto(peticion);


                PeticionCreditoFinanciero creditoFinanciero = new PeticionCreditoFinanciero
                {
                    SOC_ID = peticionCredito?.SOC_ID ?? Guid.Empty,
                    IND = 2
                };

                RespuestaCreditoFinanciero informacionActivos = null;
                try
                { 
                    informacionActivos = await _creditoRepository.ObtenerSolicCreditoFinancieros(creditoFinanciero);
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerSolicCreditoFinancieros: " + ex.Message); throw; }

                infoBasicaSolicitud.MONEDA_EXTRANJERA_SI = infoBasicaSolicitud?.SOB_TIENE_TRANSACCION_ME == "SI" ? "X" : "";
                infoBasicaSolicitud.MONEDA_EXTRANJERA_NO = infoBasicaSolicitud?.SOB_TIENE_TRANSACCION_ME == "NO" ? "X" : "";

                var tipoTrasaccions = await _catalogsRepository.ObtenerValoresCatalogosPorIdCatalogo(6);
                foreach (var item in tipoTrasaccions ?? Enumerable.Empty<CatalogoValor>())
                {
                    if (item.CVL_CODIGO == infoBasicaSolicitud?.SOB_TIPO_TRANSACCION_ME)
                    {
                        infoBasicaSolicitud.TIPO_TRANSACION_DESCRIPCION = item.CVL_DESCRIPCION;
                        break;
                    }
                }

                var tipoProductoTransaciones = await _catalogsRepository.ObtenerValoresCatalogosPorIdCatalogo(67);
                foreach (var item in tipoProductoTransaciones ?? Enumerable.Empty<CatalogoValor>())
                {
                    if (item.CVL_CODIGO == infoBasicaSolicitud?.SOB_TIPO_TRANSACCION_ME)
                    {
                        infoBasicaSolicitud.TIPO_PRODUCTO_TRANSACCIONES = item.CVL_DESCRIPCION;
                        break;
                    }
                }

                Pais paisTransaccion = null;
                try
                {
                    paisTransaccion = await _catalogsRepository.ObtenerPaisesPorId(infoBasicaSolicitud?.DPP_ID_ME ?? 0);
                    infoBasicaSolicitud.PAIS_DESCRIPCION = paisTransaccion?.DPP_NOMBRE ?? "";
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerPaisesPorId: " ); throw; }

                infoBasicaSolicitud.VIS = simulacionCliente?.SMC_VIVIENDA_VIS == "Si" ? "X" : "";
                infoBasicaSolicitud.NO_VIS = simulacionCliente?.SMC_VIVIENDA_VIS == "No" ? "X" : "";

                infoBasicaSolicitud.TIPO_VIVIENDA_NUEVA = solicitudGeneralCredito?.TVV_ID == 4 ? "X" : "";
                infoBasicaSolicitud.TIPO_VIVIENDA_USADA = solicitudGeneralCredito?.TVV_ID == 3 ? "X" : "";

                infoBasicaSolicitud.TIENE_GARAJE = solicitudGeneralCredito?.SOP_TIENE_GARAJE == "SI" ? "X" : "";
                infoBasicaSolicitud.NO_TIENE_GARAJE = solicitudGeneralCredito?.SOP_TIENE_GARAJE == "NO" ? "X" : "";

                infoBasicaSolicitud.TIENE_ASCENSOR = solicitudGeneralCredito?.SOP_POSEE_ASENSOR == "SI" ? "X" : "";
                infoBasicaSolicitud.NO_TIENE_ASCENSOR = solicitudGeneralCredito?.SOP_POSEE_ASENSOR == "NO" ? "X" : "";

                Escritura escrituraDatos = null;
                try
                {
                    var escrituraRequest = new Escritura
                    {
                        SOC_ID_SOLICITUD = peticionCredito?.SOC_ID ?? Guid.Empty,
                        CLI_IDENTIFICACION = clienteDetallado?.Data?.Identificacion ?? ""
                    };
                    escrituraDatos = await _escrituraRepository.ObtenerEscritura(escrituraRequest);
                    infoBasicaSolicitud.NOTARIA = escrituraDatos?.ESC_NOTARIA ?? "";
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerEscritura: " + ex.Message); throw; }

                InformacionVendedor informacionVendedor = null;
                try
                {
                    informacionVendedor = await _informacionVendedorRepository.ObtenerInformacionVendedorPorIdSolicCredito(peticionCredito?.SOC_ID ?? Guid.Empty);
                    var ciudadVendedor = await _catalogsRepository.ObtenerCiudadPorId(int.Parse(informacionVendedor?.IVEN_CIUDAD ?? "0"));
                    var departamentoVendedor = await _catalogsRepository.ObtenerDepartamentoPorId(int.Parse(informacionVendedor?.IVEN_DEPARTAMANTO ?? "0"));
                    var tipoIdentificacionVendedor = await _creditoRepository.ObtenerTipoIdentificacionPorId(informacionVendedor?.IVEN_TIPO_DOC ?? 0);

                    informacionVendedor.IVEN_TIPO_DOC_DESCRIPCION = tipoIdentificacionVendedor?.TID_DESCRIPCION ?? "";
                    informacionVendedor.CiudadDescripcionVendedor = ciudadVendedor?.DPC_NOMBRE ?? "";
                    informacionVendedor.DepartamentoDescripcionVendedor = departamentoVendedor?.DPD_NOMBRE ?? "";

                    var tiposCuentas = await _catalogsRepository.ObtenerValoresCatalogosPorIdCatalogo(67);
                    foreach (var item in tiposCuentas ?? Enumerable.Empty<CatalogoValor>())
                    {
                        if (int.TryParse(item.CVL_CODIGO, out int codigo) && codigo == informacionVendedor?.IVEN_TIPO_CUENTA)
                        {
                            informacionVendedor.TIPO_CUENTA_VENDEDOR = item.CVL_DESCRIPCION;
                            break;
                        }
                    }
                }
                catch (Exception ex) { _logger.LogError(ex, "Error en ObtenerInformacionVendedorPorIdSolicCredito: "); throw; }

                var reportParameters = (await ParametrosEstaticosReporteSolicitudCreditoVivienda(
                    infoBasicaSolicitud,
                    clienteDetallado,
                    informacionConyugue,
                    referenciaFamiliar,
                    datosFinancieros,
                    informacionActivos,
                    simulacionCliente,
                    solicitudGeneralCredito,
                    informacionVendedor,
                    referenciaPersonal, resultadoInmuebleInformacion))
                    .Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value))
                    .ToList();

                reportParam.SetParameters(reportParameters);
                return reportParam;
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 ERROR en ParametrosReporteSolicitudCreditoVivienda");
                Console.WriteLine($"Mensaje: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, $"InnerException: {ex.InnerException.Message}");
                    _logger.LogError(ex, $"Inner StackTrace: {ex.InnerException.StackTrace}");
                }
                throw;
            }
        }


        private async Task<Dictionary<string, string>> ParametrosEstaticosReporteSolicitudCreditoVivienda(RespuestaCreditoBasica infoBasicaSolicitud, Response<ClienteDto> clienteDetallado, InformacionConyugue informacionConyugue, SolicCreditoRefererncias referenciaFamiliar, DatosFinancierosPersonales datosFinancieros, RespuestaCreditoFinanciero informacionActivos, SimulacionCliente simulacionCliente, SolicitudCredito solicitudGeneralCredito, InformacionVendedor informacionVendedor, SolicCreditoRefererncias referenciaPersonal, RespuestaCreditoProducto resultadoInmuebleInformacion)
        {
            return new Dictionary<string, string>
            {
                //Inicio formulario
                {"Dia", infoBasicaSolicitud.FECHA_SOLICITUD.Day.ToString() ?? ""},
                {"Mes", infoBasicaSolicitud.FECHA_SOLICITUD.Month.ToString() ?? ""},
                {"Anio", infoBasicaSolicitud.FECHA_SOLICITUD.Year.ToString() ?? ""},
                {"CiudadSolicitud", resultadoInmuebleInformacion.CIUDAD_INMUEBLE ?? ""},
                {"CreditoHipotecario", infoBasicaSolicitud.CREDITO_HIPOTECARIO ?? ""},
                {"CreditoLeasing", infoBasicaSolicitud.CREDITO_LEASING ?? ""},
                //1. Información Personal - Información Basica
                {"NombreAfiliado", clienteDetallado.Data.PrimerNombre ?? ""},
                {"SegundoNombreAfiliado", clienteDetallado.Data.SegundoNombre ?? ""},
                {"ApellidoAfiliado", clienteDetallado.Data.PrimerApellido ?? ""},
                {"SegundoApellidoAfiliado", clienteDetallado.Data.SegundoApellido ?? ""},
                {"TipoIdentificacionAfiliado", infoBasicaSolicitud.TIPO_DOCUMENTO_CLIENTE ?? ""},
                {"NumeroIdentificacionAfiliado", clienteDetallado.Data.Identificacion ?? ""},
                {"FechaExpedicion", clienteDetallado.Data.FechaExpedicionDocumento.ToString("dd/MM/yyyy") ?? ""},
                {"CiudadExpedicion", infoBasicaSolicitud.CIUDAD_EXPEDICION ?? ""},
                {"EstadoCivil", infoBasicaSolicitud.ESTADO_CIVIL ?? ""},
                {"FechaNacimiento", clienteDetallado.Data.FechaNacimiento.ToString("dd/MM/yyyy") ?? ""},
                {"CiudadNacimiento", infoBasicaSolicitud.CIUDAD_NACIMIENTO ?? ""},
                {"Nacionalidad",  "COLOMBIA"},
                {"Sexo",  clienteDetallado.Data.Sexo ?? ""},
                {"NumeroPersonasCargo",  ""},
                 //1. Información Personal - Información de ubicación
                {"DireccionResidencia",  infoBasicaSolicitud.SOB_DIRECCION_RESIDENCIA ?? ""},
                {"CiudadResidencia",  infoBasicaSolicitud.CIUDAD_RESIDENCIA ?? ""},
                {"DepartamentoResidencia",  infoBasicaSolicitud.DEPARTAMENTO_RESIDENCIA ?? ""},
                {"CorreoElectronico",  clienteDetallado.Data.Correo ?? ""},
                {"TelefonoFijo",  clienteDetallado.Data.Telefono2 ?? ""},
                {"Celular",  clienteDetallado.Data.Telefono ?? ""},
                //1. Información Personal - Información de profesional/laboral
                {"Profesion",  infoBasicaSolicitud.PROFESION.ToUpper() ?? ""},
                {"Actividad",  infoBasicaSolicitud.ACTIVIDAD ?? ""},
                {"EmpresaTrabaja", infoBasicaSolicitud.EMPRESA_TRABAJA ?? ""},
                {"Cargo", infoBasicaSolicitud.GRADO ?? ""},
                {"DireccionOficina", infoBasicaSolicitud.SOB_DIRECCION_OFICINA ?? ""},
                {"CiudadOficina", infoBasicaSolicitud.CIUDAD_OFICINA ?? "" },
                {"DepartamentoOficina", infoBasicaSolicitud.DEPARTAMENTO_OFICINA ?? ""},
                {"CorreoEmpresarial", clienteDetallado.Data.Correo2 ?? ""},
                {"TelefonoOficina", infoBasicaSolicitud.SOB_TELEFONO_OFICINA ?? ""},
                {"MedioContacto", infoBasicaSolicitud.MEDIO_CONTACTO.ToUpper() ?? ""},
                //1. Información Personal - Información del conyugue
                {"NombreConyugue", informacionConyugue != null ? informacionConyugue.SOY_PRIMER_NOMBRE.ToUpper() : ""},
                {"SegundoNombreConyugue", informacionConyugue != null ? informacionConyugue.SOY_SEGUNDO_NOMBRE.ToUpper() : ""},
                {"ApellidoConyugue", informacionConyugue != null ? informacionConyugue.SOY_PRIMER_APELLIDO.ToUpper() : ""},
                {"SegundoApellidoConyugue", informacionConyugue != null ? informacionConyugue.SOY_SEGUNDO_APELLIDO.ToUpper() : ""},
                {"TipoIdentificacionConyugue", informacionConyugue != null ? infoBasicaSolicitud.TIPO_IDENTIFICACION_CONYUGUE.ToUpper() : ""},
                {"NumeroIdentificacionConyugue", informacionConyugue != null ? informacionConyugue.SOY_NUMERO_DOCUMENTO : ""},
                {"FechaExpedicionConyugue",""},
                {"CiudadExpedicionConyugue", informacionConyugue != null ? infoBasicaSolicitud.CIUDAD_EXPEDICION_CONYUGUE.ToUpper() : ""},
                {"FechaNacimientoConyugue",""},
                {"TelefonoConyugue",informacionConyugue != null ? informacionConyugue.SOY_CELULAR : ""},
                {"CelularConyugue", informacionConyugue != null ? informacionConyugue.SOY_CELULAR : ""},
                {"ActividadConyugue",informacionConyugue != null ? informacionConyugue.DESCRIPCION_ACTIVIDAD_CONYUGUE.ToUpper() : ""},
                {"EmpresaConyugue",informacionConyugue != null ? informacionConyugue.SOY_EMPRESA_LABORA : ""},
                {"IngresosConyugue",""},
                {"EgresosConyugue",""},
                {"IngresosAdicionalesConyugue",""},
                {"TotalActivos","" },
                {"TotalPasivos",""},
                //2. REFERENCIAS PERSONALES - REFERENCIA FAMILIAR
                {"NombreFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.SOR_NOMBRES : ""},
                {"PrimerApellidoFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.SOR_PRIMER_APELLIDO : ""},
                {"SegundoApellidoFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.SOR_SEGUNDO_APELLIDO : ""},
                {"ParentescoFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.DESCRIPCION_PARENTESCO : ""},
                {"DireccionFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.SOR_DIRECCION : ""},
                {"CiudadFamiliar",referenciaFamiliar!=null ? infoBasicaSolicitud.CIUDAD_REFERENCIA_FAMILIAR : ""},
                {"TelefonoFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.SOR_TELEFONO : ""},
                {"CelularFamiliar",referenciaFamiliar!=null ? referenciaFamiliar.SOR_CELULAR : ""},
                //2. REFERENCIAS PERSONALES - REFERENCIA PERSONAL
                {"NombrePersonal",referenciaPersonal != null ? referenciaPersonal.SOR_NOMBRES : ""},
                {"PrimerApellidoPersonal", referenciaPersonal != null ? referenciaPersonal.SOR_PRIMER_APELLIDO : ""},
                {"SegundoApellidoPersonal",referenciaPersonal != null ? referenciaPersonal.SOR_SEGUNDO_APELLIDO : ""},
                {"ParentescoPersonal",referenciaPersonal != null ? referenciaPersonal.DESCRIPCION_PARENTESCO : ""},
                {"DireccionPersonal",referenciaPersonal != null ? referenciaPersonal.SOR_DIRECCION : ""},
                {"CiudadPersonal", infoBasicaSolicitud.CIUDAD_REFERENCIA_PERSONAL ?? ""},
                {"TelefonoPersonal",referenciaPersonal != null ? referenciaPersonal.SOR_TELEFONO : ""},
                {"CelularPersonal",referenciaPersonal != null ? referenciaPersonal.SOR_CELULAR : ""},
                
                //3. INFORMACIÓN FINANCIERA Y ECONÓMICA INFORMACIÓN FINANCIERA DEL AFILIADO
                {"IngresosMensuales", datosFinancieros.SDF_VALOR_SALARIO.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"OtrosIngresosMensuales", infoBasicaSolicitud.OTROS_INGRESOS.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"TotalIngresos", datosFinancieros.SDF_VALOR_TOTAL_INGRESOS.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"EgresosMensuales", datosFinancieros.SDF_VALOR_TOTAL_EGRESOS.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"TotalEgresosMensuales", datosFinancieros.SDF_VALOR_TOTAL_EGRESOS.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"MatriculaVivienda", informacionActivos.SOF_MATRICULA_VIVIENDA ?? ""},
                {"DireccionVivienda", informacionActivos.SOF_DIRECCION_VIVIENDA ?? ""},
                {"ValorComercialVivienda", informacionActivos.SOF_VALOR_COMERCIAL_VIVIENDA.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"MarcaVehiculo", informacionActivos.SOF_MARCA_VEHICULO ?? ""},
                {"PlacaVehiculo", informacionActivos.SOF_PLACA_VEHICULO ?? ""},
                {"ValorComercialVehiculo", informacionActivos.SOF_VALOR_COMERCIAL_VEHICULO.ToString("C", new CultureInfo("es-CO")) ?? ""},
                 //3. INFORMACIÓN FINANCIERA Y INFORMACIÓN ECONÓMICA
                {"TieneMonedaExtrajera", infoBasicaSolicitud.MONEDA_EXTRANJERA_SI ?? ""},
                {"NoTieneMonedaExtrajera", infoBasicaSolicitud.MONEDA_EXTRANJERA_NO ?? ""},
                {"TipoTransaccion", infoBasicaSolicitud.TIPO_TRANSACION_DESCRIPCION ?? ""},
                {"TipoProductoTransaccion", infoBasicaSolicitud.TIPO_PRODUCTO_TRANSACCIONES ?? ""},
                {"BancoTrasaccion", infoBasicaSolicitud.SOB_BANCO_TRANSACCION_ME.ToUpper() ?? ""},
                {"NumeroProductoTrasaccion", infoBasicaSolicitud.SOB_NUMERO_PRODUCTO_ME ?? ""},
                {"MontoTransaccion", infoBasicaSolicitud.SOB_SALDO_PROMEDIO_CUENTAS_ME.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"MonedaTransaccion", infoBasicaSolicitud.SOB_MONEDA_TRANSACCION_ME ?? ""},
                {"PaisTransaccion", infoBasicaSolicitud.PAIS_DESCRIPCION ?? ""},
                //4. INFORMACIÓN CRÉDITO VIVIENDA - DETALLE PRODUCTO - INFORMACIÓN INMUEBLE 
                {"ValorVenta", simulacionCliente.SMC_VALOR_VIVIENDA.ToString("C", new CultureInfo("es-CO")) ?? ""},
                {"VIS", infoBasicaSolicitud.VIS ?? ""},
                {"NO_VIS", infoBasicaSolicitud.NO_VIS ?? ""},
                {"TipoInmueble", solicitudGeneralCredito.SOP_PROPIEDAD_HORIZONTAL ?? ""},
                {"TipoNueva", infoBasicaSolicitud.TIPO_VIVIENDA_NUEVA ?? ""},
                {"TipoUsada", infoBasicaSolicitud.TIPO_VIVIENDA_USADA ?? ""},
                {"DireccionInmueble", solicitudGeneralCredito.SOP_DIRECCION_INMUEBLE ?? ""},
                {"BarrioInmueble", solicitudGeneralCredito.SOP_BARRIO_INMUEBLE ?? ""},
                {"EdadInmueble", solicitudGeneralCredito.SOP_AÑOS_CONSTRUCCION.ToString() ?? ""},
                {"TieneGaraje", infoBasicaSolicitud.TIENE_GARAJE ?? ""},
                {"NoTieneGaraje", infoBasicaSolicitud.NO_TIENE_GARAJE ?? ""},
                {"Estrato", "No presenta"},
                {"CiudadInmueble", simulacionCliente.CIUDAD ?? ""},
                {"PisoInmueble", solicitudGeneralCredito.SOP_PISO_INMUEBLE.ToString() ?? ""},
                {"TieneAscensor", infoBasicaSolicitud.TIENE_ASCENSOR ?? ""},
                {"NoTieneAscensor", infoBasicaSolicitud.NO_TIENE_ASCENSOR ?? ""},
                {"Notaria", infoBasicaSolicitud.NOTARIA ?? ""},
                //4. INFORMACIÓN CRÉDITO VIVIENDA - INFORMACIÓN DEL VENDEDOR DEL INMUEBLE
                {"RazonSocial", informacionVendedor.IVEN_NOMBRE.ToUpper() ?? ""},
                {"TipoIdentificacionVendedor", informacionVendedor.IVEN_TIPO_DOC_DESCRIPCION.ToUpper() ?? ""},
                {"NumeroDocumentoVendedor", informacionVendedor.IVEN_NRO_DOC ?? ""},
                {"DireccionVendedor", informacionVendedor.IVEN_DIRECCION.ToUpper() ?? ""},
                {"CiudadVendedor", informacionVendedor.CiudadDescripcionVendedor.ToUpper() ?? ""},
                {"DepartamentoVendedor", informacionVendedor.DepartamentoDescripcionVendedor.ToUpper() ?? ""},
                {"CorreoVendedor", informacionVendedor.IVEN_CORREO.ToUpper() ?? ""},
                {"TelefonoVendedor", ""},
                {"CelularVendedor", informacionVendedor.IVEN_TELEFONO ?? ""},
                {"TipoCuentaVendedor", informacionVendedor.TIPO_CUENTA_VENDEDOR ?? ""},
                {"NumeroCuentaVendedor", informacionVendedor.IVEN_NRO_CUENTA.ToString() ?? ""},
                {"BancoVendedor", informacionVendedor.BANCO_VENDEDOR ?? ""},
                {"FechaEntrega", informacionVendedor!=null ? "" : "No presenta"},
                //6. DECLARACIÓN DE ORIGEN DE FONDOS Y AUTORIZACIÓN CONSULTAS - Declaración voluntaria origen y destino de fondos
                {"DeclaracionVoluntariaDescripcion",solicitudGeneralCredito.SOC_DECLARACION_ORIGEN_FONDOS.ToUpper()  ?? ""},
                {"AutorizacionConsultaCentrales",solicitudGeneralCredito.SOC_AUTORIZA_CONSULTA_CENTRALES =="S"? "Si acepto" : ""},
                {"AutorizacionDatosPersonales",solicitudGeneralCredito.SOC_AUTORIZA_USO_DATOS =="S"? "Si acepto" : ""},
                {"DeclaracionAsegurabilidad",solicitudGeneralCredito.SOC_DECLARACION_ASEGURABILIDAD =="S"? "Si acepto" : ""},

            };

        }

        public async Task<FileResult> FacturaSevenCredito(Guid SOC_ID)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            try
            {
                FacturaSevenCredito facturaSeven = await _creditoRepository.ConsultarDatosFactura(SOC_ID);
                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\FacturaSevenCredito.rdlc";
                report.ReportPath = reportPath;
                DatosFacturaSevenParam datosFacturaSevenParam = new DatosFacturaSevenParam();
                report = await ParametrosFacturaSevenCredito(report, facturaSeven, datosFacturaSevenParam);
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

        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosFacturaSevenCredito(Microsoft.Reporting.NETCore.LocalReport reportParam, FacturaSevenCredito facturaSeven, DatosFacturaSevenParam datosFacturaSevenParam)
        {

            var tipoDocumento = (await this._catalogsRepository.ObtenerDocumentoTipo()).Where(tipoDocumento => tipoDocumento.TID_ID == facturaSeven.FAC_TIPO_DOCUMENTO);
            var tipoDocumentoVendedor = (await this._catalogsRepository.ObtenerDocumentoTipo()).Where(tipoDocumento => tipoDocumento.TID_ID == facturaSeven.FAC_TIPO_DOCUMENTO_VENDEDOR);
            var bancoVendedor = (await this._catalogsRepository.ObtenerEntidadBancaria()).Where(x => x.ENT_ID == facturaSeven.FAC_BANCO_VENDEDOR).FirstOrDefault();


            datosFacturaSevenParam.TipoCredito = facturaSeven.FAC_TIPO_CREDITO == 2 ? "HIPOTECARIO" : "LEASING";
            datosFacturaSevenParam.TipoDocumentoAfiliado = tipoDocumento.Select(x => x.TID_DESCRIPCION).FirstOrDefault();
            datosFacturaSevenParam.TipoDocumentoVendedor = tipoDocumento.Select(x => x.TID_DESCRIPCION).FirstOrDefault();
            datosFacturaSevenParam.TipoCuentaVendedor = facturaSeven.FAC_TIPO_CUENTA_VENDEDOR == 1 ? "AHORRO" : "CORRIENTE";
            datosFacturaSevenParam.BancoVendedor = bancoVendedor.ENT_NOMBRE_RAZON_SOCIAL;



            var reportParameters = ParametrosEstaticosFacturaSevenCredito(facturaSeven, datosFacturaSevenParam).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();
            reportParam.SetParameters(reportParameters);
            return reportParam;
        }

        public async Task<Dictionary<string, string>> ParametrosEstaticosFacturaSevenCredito(FacturaSevenCredito facturaSeven, DatosFacturaSevenParam datosFacturaSevenParam)
        {
            return new Dictionary<string, string>
            {
                {"FechaDesembolo",facturaSeven.FAC_FECHA_REGISTRO_FACTURA.ToString("dd/MM/yyyy") },
                {"TipoCredito",datosFacturaSevenParam.TipoCredito},
                {"NumeroFactura",facturaSeven.FAC_NUMERO_FACTURA },
                {"Radicado",facturaSeven.FAC_RADICADO},
                {"TipoDocumentoAfiliado",datosFacturaSevenParam.TipoDocumentoAfiliado},
                {"NumeroDocumentoAfiliado",facturaSeven.FAC_NUMERO_DOCUMENTO},
                {"NombreAfiliado",$"{facturaSeven.FAC_NOMBRE_AFILIADO} {facturaSeven.FAC_SEGUNDO_NOMBRE_AFILIADO} {facturaSeven.FAC_APELLIDO_AFILIADO} {facturaSeven.FAC_SEGUNDO_APELLIDO_AFILIADO}" },
                {"TipoDocumentoVendedor",datosFacturaSevenParam.TipoDocumentoVendedor},
                {"NumeroDocumentoVendedor",facturaSeven.FAC_NUMERO_DOCUMENTO_VENDEDOR},
                {"NombreVendedor", facturaSeven.FAC_NOMBRE_COMPLETO_VENDEDOR},
                {"Modulo", "Credito" },
                {"TipoCuenta", datosFacturaSevenParam.TipoCuentaVendedor },
                {"BancoVendedor", datosFacturaSevenParam.BancoVendedor},
                {"AreaNegocio", "CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICÍA" },
                {"CDP", facturaSeven.FAC_CDP.ToString() },
                {"ValorDesembolso", facturaSeven.FAC_VALOR_DESEMBOLSO.ToString("C",new System.Globalization.CultureInfo("es-CO"))},
                {"NumeroCuentaVendedor", facturaSeven.FAC_NUMERO_CUENTA_VENDEDOR},

            };



        }

        public async Task<FileResult> ReporteProspeccionCartaRechazo(IdentificacionProspeccion data)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            try
            {

                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\NotificacionRechazo.rdlc";
                report.ReportPath = reportPath;
                var clienteDetallado = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(data.tipoDocumento, data.numeroDocumento);
                SolicitudSimulacion simulacionInformacion = await _creditoRepository.ObtenerSolicitudSimulacionCreditoNumeroDocumento(data.numeroDocumento);
                SolicitudSimulacion informacionSimulacion = await _creditoRepository.ObtenerSolicitudInformacionSimulacion(data.SLS_ID);
                report = await ParametrosReporteProspeccionRechazo(report, clienteDetallado.Data, informacionSimulacion);
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
            return await GenerarReporte(bytes, false, data.numeroDocumento, false, true);
        }

        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosReporteProspeccionRechazo(Microsoft.Reporting.NETCore.LocalReport reportParam, ClienteDto cliente, SolicitudSimulacion simulacionInformacion)
        {

            var reportParameters = ParametrosEstaticosProspeccionRechazo(cliente, simulacionInformacion).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();

            reportParam.SetParameters(reportParameters);
            return reportParam;
        }

        public async Task<Dictionary<string, string>> ParametrosEstaticosProspeccionRechazo(ClienteDto cliente, SolicitudSimulacion simulacionInformacion)
        {
            return new Dictionary<string, string>
            {
                { "Nombres", $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" },
                { "Correo", $"{cliente.Correo2}" },
                { "Radicado", $"{simulacionInformacion.SLS_NUMERO_RADICADO_83}" },
                { "Area", "Originación" },
                { "FechaHora", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").Replace("AM", "a.m.").Replace("PM", "p.m.") }
            };



        }


        public async Task<ReporteGeneradoDto> ReporteApruebaActividadTramite(
     IdentificacionProspeccion data)
        {
            try
            {
                // ============================================
                // VALIDACIONES DE ENTRADA
                // ============================================

                if (data == null)
                {
                    throw new ArgumentNullException(
                        nameof(data),
                        "La información de la identificación no puede ser nula.");
                }

                if (data.tipoDocumento==null)
                {
                    throw new ArgumentException(
                        "El tipo de documento es obligatorio.",
                        nameof(data.tipoDocumento));
                }

                if (string.IsNullOrWhiteSpace(data.numeroDocumento))
                {
                    throw new ArgumentException(
                        "El número de documento es obligatorio.",
                        nameof(data.numeroDocumento));
                }

                if (data.SLS_ID == Guid.Empty)
                {
                    throw new ArgumentException(
                        "El SLS_ID es obligatorio.",
                        nameof(data.SLS_ID));
                }

                // ============================================
                // VALIDAR ARCHIVO RDLC
                // ============================================

                string reportPath = Path.Combine(
                    _webHostEnvironment.ContentRootPath,
                    "Reportes",
                    "NotificacionAprobacion.rdlc");

                if (!File.Exists(reportPath))
                {
                    throw new FileNotFoundException(
                        $"No se encontró el archivo RDLC en la ruta: {reportPath}");
                }

                var report = new Microsoft.Reporting.NETCore.LocalReport();

                // ============================================
                // CARGAR REPORTE
                // ============================================

                report.ReportPath = reportPath;

                // ============================================
                // OBTENER INFORMACIÓN DEL CLIENTE
                // ============================================

                var clienteDetallado =
                    await _iClientRequestProvider
                        .ObtenerClientePorTipoIdentificationYNumero(
                            data.tipoDocumento,
                            data.numeroDocumento);

                if (clienteDetallado == null)
                {
                    throw new Exception(
                        "No fue posible obtener la información del cliente.");
                }

                if (!clienteDetallado.IsSuccess)
                {
                    throw new Exception(
                        $"No fue posible obtener la información del cliente. " +
                        $"Detalle: {clienteDetallado.ReturnMessage}");
                }

                if (clienteDetallado.Data == null)
                {
                    throw new Exception(
                        "La información del cliente retornó vacía.");
                }

                // ============================================
                // OBTENER INFORMACIÓN DE LA SIMULACIÓN
                // ============================================

                SolicitudSimulacion informacionSimulacion =
                    await _creditoRepository
                        .ObtenerSolicitudInformacionSimulacion(data.SLS_ID);

                if (informacionSimulacion == null)
                {
                    throw new Exception(
                        $"No se encontró información de la simulación para el SLS_ID: {data.SLS_ID}");
                }

                // ============================================
                // CONFIGURAR PARÁMETROS DEL REPORTE
                // ============================================

                report = await ParametrosApruebaActividadTramite(
                    report,
                    clienteDetallado.Data,
                    informacionSimulacion);

                if (report == null)
                {
                    throw new Exception(
                        "No fue posible configurar el reporte.");
                }

                // ============================================
                // GENERAR PDF
                // ============================================

                byte[] bytes;

                try
                {
                    bytes = report.Render("PDF");
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        $"Error renderizando el reporte " +
                        $"NotificacionAprobacion.rdlc: {ex.Message}",
                        ex);
                }

                if (bytes == null || bytes.Length == 0)
                {
                    throw new Exception(
                        "El reporte fue generado pero el archivo PDF está vacío.");
                }

                // ============================================
                // CONVERTIR A BASE64
                // ============================================

                string base64 = Convert.ToBase64String(bytes);

                if (string.IsNullOrWhiteSpace(base64))
                {
                    throw new Exception(
                        "No fue posible convertir el reporte a Base64.");
                }

                // ============================================
                // GENERAR ARCHIVO
                // ============================================

                var archivo = await GenerarReporte(
                    bytes,
                    false,
                    data.numeroDocumento,
                    false,
                    false);

                if (archivo == null)
                {
                    throw new Exception(
                        "No fue posible generar el archivo del reporte.");
                }

                // ============================================
                // RESPUESTA
                // ============================================

                return new ReporteGeneradoDto
                {
                    Archivo = archivo,
                    Base64 = base64
                };
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error generando el reporte de aprobación de actividad. " +
                    $"Documento: {data?.numeroDocumento ?? "NO DISPONIBLE"}. " +
                    $"Detalle: {ex.Message}",
                    ex);
            }
        }

        public async Task<Microsoft.Reporting.NETCore.LocalReport> ParametrosApruebaActividadTramite(Microsoft.Reporting.NETCore.LocalReport reportParam, ClienteDto cliente, SolicitudSimulacion simulacionInformacion)
        {

            var reportParameters = ParametrosEstaticosApruebaActividadTramite(cliente, simulacionInformacion).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();

            reportParam.SetParameters(reportParameters);
            return reportParam;
        }

        public async Task<Dictionary<string, string>> ParametrosEstaticosApruebaActividadTramite(ClienteDto cliente, SolicitudSimulacion simulacionInformacion)
        {
            return new Dictionary<string, string>
            {
                { "Nombres", $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" },
                { "Correo", $"{cliente.Correo2}" },
                { "Radicado", $"{simulacionInformacion.SLS_NUMERO_RADICADO_83}" },
                { "Area", "Originación" },
                { "FechaHora", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").Replace("AM", "a.m.").Replace("PM", "p.m.") }
            };



        }


    }

    public class ReporteGeneradoDto
    {
        public FileResult Archivo { get; set; }
        public string Base64 { get; set; }
    }

}