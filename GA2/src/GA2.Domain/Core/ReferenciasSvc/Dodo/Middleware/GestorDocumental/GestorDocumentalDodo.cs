using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Main;
using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.Dodo.Forms.Constants;
using GA2.Domain.Core.Dodo.Forms.Extensions;
using GA2.Domain.Core.Dodo.Forms.Models;
using GA2.Domain.Core.Dodo.Forms.Services;
using GA2.Domain.Core.ReferenciasSvc.Dodo.Middleware.GestorDocumental;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.PuntosAtencion;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Credito.WorkManager
{
    public class GestorDocumentalDodo : IGestorDocumentalDodo
    {
        #region Fields

        private readonly IDodoFormsService _formsService;
        private readonly ILogger<GestorDocumentalDodo> _logger;
        private readonly ICreditoRepository _creditoRepository;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly ICuentasClientesRepository _cuentasClientesRepository;
        private readonly IMapper _mapper;
        private readonly IEmbargosRepository _juzgadosRepository;
        private readonly IPuntoAtencionRepository _puntoAtencionRepository;

        #endregion

        #region Constructor

        public GestorDocumentalDodo(
            IDodoFormsService formsService,
            ILogger<GestorDocumentalDodo> logger,
            ICreditoRepository creditoRepository,
            ICatalogosRepository catalogosRepository,
            IClientRequestProvider clientRequestProvider,
            ICuentasClientesRepository cuentasClientesRepository,
            IMapper mapper,
            IEmbargosRepository juzgadosRepository,
            IPuntoAtencionRepository puntoAtencionRepository)
        {
            _formsService = formsService ?? throw new ArgumentNullException(nameof(formsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
            _catalogosRepository = catalogosRepository ?? throw new ArgumentNullException(nameof(catalogosRepository));
            _clientRequestProvider = clientRequestProvider ?? throw new ArgumentNullException(nameof(clientRequestProvider));
            _cuentasClientesRepository = cuentasClientesRepository ?? throw new ArgumentNullException(nameof(cuentasClientesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _juzgadosRepository = juzgadosRepository;
            _puntoAtencionRepository = puntoAtencionRepository;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Flujo completo 361:
        /// 1. Resuelve contexto del cliente una sola vez
        /// 2. Valida si existe expediente padre (02), si no lo crea
        /// 3. Radica en formulario 361
        /// 4. Vincula ID_EXT del padre (02) en el hijo (361)
        /// </summary>
        public async Task<Response<DodoFileOperationResponse>> RadicarFlujo361(SolicitudSimulacionDto preAprobado)
        {
            try
            {
                _logger.LogInformation("GestorDocumentalDodo RadicarFlujo361 — inicio");

                // Resolver contexto una sola vez para evitar llamadas duplicadas
                var contexto = await ResolverContexto361(preAprobado);

                // 1. Validar expediente padre
                var validaExpPadre = await GetFormDataAsync(
                    FormCodes.Flujo02,
                    Fields02.Identificacion,
                    contexto.Cliente.Identificacion);

                if (validaExpPadre.Data?.Rows == 0)
                {
                    _logger.LogInformation("GestorDocumentalDodo RadicarFlujo361 — expediente padre no existe, creando");
                    await CrearExpedientePadre361(contexto);
                }

                // 2. Radicar en 361
                var insertRequest = BuildInsertRequest361(contexto);
                var insert = await _formsService.InsertAsync(insertRequest);

                if (!insert.IsSuccess)
                {
                    _logger.LogWarning("GestorDocumentalDodo RadicarFlujo361 — Insert falló: {Msg}", insert.ReturnMessage);
                    return insert;
                }

                _logger.LogInformation("GestorDocumentalDodo RadicarFlujo361 — BarCode: {BarCode}", insert.Data.BarCode);

                // 3. Vincular ID_EXT: busca el ID en el padre (02) y lo escribe en el hijo (361)
                await VincularIdExterno(
                    barCode: insert.Data.BarCode,
                    formCodeHijo: FormCodes.Flujo361,
                    formCodePadre: FormCodes.Flujo02,
                    filterField: Fields02.Identificacion,
                    filterValue: contexto.Cliente.Identificacion);

                return insert;
            }
            catch (Exception e)
            {
                return Response<DodoFileOperationResponse>.Fail(e);
            }
        }

        /// <summary>
        /// Flujo completo 83:
        /// 1. Valida si existe expediente padre (83), si no lo crea
        /// 2. Radica en formulario 83
        /// 3. Vincula ID_EXT del padre en el hijo
        /// </summary>
        public async Task<Response<DodoFileOperationResponse>> RadicarFlujo83(DatosIdentificacionDto datosIdentificacion)
        {
            try
            {
                _logger.LogInformation("GestorDocumentalDodo RadicarFlujo83 — Identificacion: {Id}", datosIdentificacion.Identificacion);

                // 1. Validar expediente padre
                var validaExpPadre = await GetFormDataAsync(
                    FormCodes.Flujo83,
                    Fields83.Identificacion,
                    datosIdentificacion.Identificacion);

                if (validaExpPadre.Data?.Rows == 0)
                {
                    _logger.LogInformation("GestorDocumentalDodo RadicarFlujo83 — expediente padre no existe, creando");
                    await CrearExpedientePadre83(datosIdentificacion);
                }

                // 2. Radicar en 83
                var insertRequest = await BuildInsertRequest83(datosIdentificacion);
                var insert = await _formsService.InsertAsync(insertRequest);

                if (!insert.IsSuccess)
                {
                    _logger.LogWarning("GestorDocumentalDodo RadicarFlujo83 — Insert falló: {Msg}", insert.ReturnMessage);
                    return insert;
                }

                _logger.LogInformation("GestorDocumentalDodo RadicarFlujo83 — BarCode: {BarCode}", insert.Data.BarCode);

                // 3. Vincular ID_EXT: busca el ID en el padre (83) y lo escribe en el hijo (83)
                await VincularIdExterno(
                    barCode: insert.Data.BarCode,
                    formCodeHijo: FormCodes.Flujo83,
                    formCodePadre: FormCodes.Flujo83,
                    filterField: Fields83.Identificacion,
                    filterValue: datosIdentificacion.Identificacion);

                return insert;
            }
            catch (Exception e)
            {
                return Response<DodoFileOperationResponse>.Fail(e);
            }
        }

        #endregion

        #region Contexto — resolución única de datos

        /// <summary>
        /// Resuelve todos los datos necesarios para el flujo 361 en una sola pasada,
        /// evitando llamadas duplicadas entre validación y construcción del request.
        /// </summary>
        private async Task<Contexto361> ResolverContexto361(SolicitudSimulacionDto preAprobado)
        {
            var tipoCreditoDinamico = preAprobado.TcrId == 1 ? 2 : 1;
            var solicitudSimulacion = _mapper.Map<SolicitudSimulacion>(preAprobado);
            var solicitudPreA = await _creditoRepository.ObtenerDatosSolicitudClienteByPreAprobado(solicitudSimulacion);
            var infoCliente = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(solicitudPreA.SMC_NUMERO_DOCUMENTO));
            var idTipoDoc = (await _clientRequestProvider.ObtenerClientesByNumeroIdentificacion(infoCliente.numeroDocumento)).Data.IdTipoIdentificacion;
            var tipoDocumento = await _juzgadosRepository.ObtenerTipoIdentificacionPorId(idTipoDoc);
            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(idTipoDoc, infoCliente.numeroDocumento)).Data;
            var clienteDto = await _clientRequestProvider.ObtenerInfoCliente(idTipoDoc, infoCliente.numeroDocumento);
            var detalleCliente = await _cuentasClientesRepository.ObtenerDetalleInfoCliente(_mapper.Map<InfoCliente>(clienteDto.Data));
            var unidadEjecutora = (await _catalogosRepository.ObtenerUnidadesEjecutoras()).First(x => x.UEJ_ID == infoCliente.UEJ_Id).UEJ_NOMBRE;
            var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.IdCategoria).CTG_DESCRIPCION;
            var codigoCiudad = cliente.IdCiudadResidencia == 0 ? cliente.IdCiudadNacimiento : cliente.IdCiudadResidencia;
            var ciudad = await _catalogosRepository.ObtenerCiudadPorId(codigoCiudad);
            var ciudadResidencia = await _catalogosRepository.ObtenerCiudadPorId(cliente.IdCiudadResidencia);
            var departamento = await _catalogosRepository.ObtenerDepartamentoPorId(ciudad.DPD_ID);
            var departamentoRes = await _catalogosRepository.ObtenerDepartamentoPorId(cliente.IdDepartamentoResidencia);
            var tipoCredito = (await _catalogosRepository.ObtenerTipoCredito()).Where(tc => tc.TCR_ID == tipoCreditoDinamico).Select(tp => tp.TCR_DESCRIPCION).First();
            var puntoAtencion = await _puntoAtencionRepository.ObtenerPuntosAtencionPorId(solicitudSimulacion.PA_ID);
            var medioRecepcion = await _creditoRepository.ObtenerMediosRecepcionById(solicitudPreA.MEDIO_RECEPCION);
            var medioCreditoVivienda = await _creditoRepository.ObtenerMediosCreditosViviendaById(solicitudPreA.MEDIOS_CREDITO);

            return new Contexto361
            {
                SolicitudPreA = solicitudPreA,
                InfoCliente = infoCliente,
                TipoDocumento = tipoDocumento,
                Cliente = cliente,
                DetalleCliente = detalleCliente,
                UnidadEjecutora = unidadEjecutora,
                Categoria = categoria,
                Ciudad = ciudad,
                CiudadResidencia = ciudadResidencia,
                Departamento = departamento,
                DepartamentoRes = departamentoRes,
                TipoCredito = tipoCredito,
                PuntoAtencion = puntoAtencion,
                MedioRecepcion = medioRecepcion,
                MedioCreditoVivienda = medioCreditoVivienda,
                IdTipoDoc = idTipoDoc
            };
        }

        /// <summary>
        /// Clase interna para transportar el contexto resuelto del flujo 361
        /// sin repetir llamadas a repositorios.
        /// </summary>
        private class Contexto361
        {
            public dynamic SolicitudPreA { get; set; }
            public SimulacionClienteDto InfoCliente { get; set; }
            public dynamic TipoDocumento { get; set; }
            public dynamic Cliente { get; set; }
            public dynamic DetalleCliente { get; set; }
            public string UnidadEjecutora { get; set; }
            public string Categoria { get; set; }
            public dynamic Ciudad { get; set; }
            public dynamic CiudadResidencia { get; set; }
            public dynamic Departamento { get; set; }
            public dynamic DepartamentoRes { get; set; }
            public string TipoCredito { get; set; }
            public dynamic PuntoAtencion { get; set; }
            public dynamic MedioRecepcion { get; set; }
            public dynamic MedioCreditoVivienda { get; set; }
            public int IdTipoDoc { get; set; }
        }

        #endregion

        #region Creación de expediente padre

        private async Task CrearExpedientePadre361(Contexto361 contexto)
        {
            var insertRequest = BuildInsertRequestExpedientePadre361(contexto);
            var result = await _formsService.InsertAsync(insertRequest);

            if (!result.IsSuccess)
                _logger.LogWarning("GestorDocumentalDodo CrearExpedientePadre361 falló: {Msg}", result.ReturnMessage);
        }

        private async Task CrearExpedientePadre83(DatosIdentificacionDto datosIdentificacion)
        {
            var insertRequest = await BuildInsertRequest83(datosIdentificacion);
            var result = await _formsService.InsertAsync(insertRequest);

            if (!result.IsSuccess)
                _logger.LogWarning("GestorDocumentalDodo CrearExpedientePadre83 falló: {Msg}", result.ReturnMessage);
        }

        #endregion

        #region Helpers privados

        private async Task<Response<FormGetDataResponse>> GetFormDataAsync(
            string formCode,
            string filterField,
            string filterValue)
        {
            var request = new FormGetDataRequest
            {
                FormCode = formCode,
                FilterParameters = new DodoFilterParameters
                {
                    Field = filterField,
                    Operator = "=",
                    Value = filterValue
                },
                Header = new DodoHeader()
            };

            return await _formsService.GetDataAsync(request);
        }

        /// <summary>
        /// Busca el ID en el formulario PADRE y lo escribe como ID_EXT en el formulario HIJO.
        /// Corrección del bug original donde se buscaba en el mismo formulario hijo.
        /// </summary>
        private async Task VincularIdExterno(
            string barCode,
            string formCodeHijo,
            string formCodePadre,
            string filterField,
            string filterValue)
        {
            try
            {
                // Busca el ID en el PADRE
                var queryResponse = await GetFormDataAsync(formCodePadre, filterField, filterValue);
                var idExterno = queryResponse.Data?.Data
                    .FirstOrDefault(x => x.Key == FieldsGeneral.Id)?.Value;

                if (string.IsNullOrEmpty(idExterno))
                {
                    _logger.LogWarning(
                        "GestorDocumentalDodo VincularIdExterno — Sin ID en padre {FormCodePadre}", formCodePadre);
                    return;
                }

                // Escribe ID_EXT en el HIJO
                var updateRequest = new FormUpdateRequest
                {
                    FormCode = formCodeHijo,
                    OperationUser = "Web",
                    FilterParameters = new DodoFilterParameters
                    {
                        Field = FieldsGeneral.CodigoBarras,
                        Operator = "=",
                        Value = barCode
                    },
                    Data = new List<DodoDataField>()
                        .Add(FieldsGeneral.IdExterno, idExterno),
                    Header = new DodoHeader()
                };

                var result = await _formsService.UpdateAsync(updateRequest);

                if (!result.IsSuccess)
                    _logger.LogWarning(
                        "GestorDocumentalDodo VincularIdExterno — Update falló: {Msg}", result.ReturnMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GestorDocumentalDodo VincularIdExterno — BarCode {BarCode}", barCode);
            }
        }

        #endregion

        #region Builders

        private FormInsertRequest BuildInsertRequest361(Contexto361 ctx)
        {
            var direccion = !string.IsNullOrEmpty(ctx.Cliente.Direccion)
                ? ctx.Cliente.Direccion
                : "No Registra";

            return new FormInsertRequest
            {
                CheckWorkflow = true,
                FormCode = FormCodes.Flujo361,
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new DodoHeader(),
                Data = new List<DodoDataField>().AddFlujo361Data(
                    puntoAtencion: $"{ctx.PuntoAtencion.PA_PUNTO}",
                    medioRecepcion: $"{ctx.MedioRecepcion.MR_DESCRIPCION}",
                    tipoCredito: $"{ctx.TipoCredito}",
                    medioCreditoVivienda: $"{ctx.MedioCreditoVivienda.MCV_DESCRIPCION}",
                    numFolios: $"{ctx.SolicitudPreA.NUM_FOLIOS}",
                    tipoDocumento: $"{ctx.TipoDocumento.TID_CODIGO}-{ctx.TipoDocumento.TID_DESCRIPCION}",
                    numeroDocumento: $"{ctx.InfoCliente.numeroDocumento}",
                    nombreCliente: $"{ctx.InfoCliente.nombreCliente}",
                    unidadEjecutora: $"{ctx.UnidadEjecutora}",
                    categoria: $"{ctx.Categoria}",
                    departamento:  $"{ctx.Departamento.DPD_NOMBRE}",
                    ciudad:  $"{ctx.Ciudad.DPC_NOMBRE}",
                    direccion:  $"{direccion}",
                    telefono: $"{ctx.Cliente.Telefono}",
                    correo: $"{ctx.Cliente.Correo}")
            };
        }

        private FormInsertRequest BuildInsertRequestExpedientePadre361(Contexto361 ctx)
        {
            string direccion = string.IsNullOrEmpty(ctx.Cliente.Direccion) ? "NO REGISTRA" : ctx.Cliente.Direccion;
            string correo = string.IsNullOrEmpty(ctx.Cliente.Correo) ? "NO REGISTRA" : ctx.Cliente.Correo;

            return new FormInsertRequest
            {
                CheckWorkflow = true,
                FormCode = FormCodes.Flujo02,
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new DodoHeader(),
                Data = new List<DodoDataField>().AddFlujo02Data(
                    identificacion: $"{ctx.Cliente.Identificacion}",
                    nombre: $"{ctx.InfoCliente.nombreCliente}",
                    tipoDocumento: $"{ctx.TipoDocumento.TID_CODIGO}-{ctx.TipoDocumento.TID_DESCRIPCION}",
                    direccion: $"{direccion}",
                    telefonoFijo: $"{ctx.InfoCliente.telefonoFijo}",
                    telefonoCelular: $"{ctx.InfoCliente.telefonoCelular}",
                    correo: $"{correo}",
                    unidadEjecutora: $"{ctx.DetalleCliente.UNIDADE_EJECUTORA}",
                    gradoCategoria: $"{ctx.DetalleCliente.GRADO} - {ctx.DetalleCliente.CATEGORIA}",
                    barrio: $"{ctx.Cliente.Barrio}",
                    departamento: $"{ctx.DepartamentoRes.DPD_NOMBRE}",
                    ciudad: $"{ctx.CiudadResidencia.DPC_NOMBRE}")
            };
        }

        private async Task<FormInsertRequest> BuildInsertRequest83(DatosIdentificacionDto identificacionDto)
        {
            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(identificacionDto.TipoIdentificacion, identificacionDto.Identificacion)).Data;
            var infoCliente = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(identificacionDto.Identificacion));

            string prestamo = "0";
            string plazo = "0";
            string valorCuota = "0";
            string tipoCredito = "Crédito Hipotecario";

            if (infoCliente != null)
            {
                prestamo = infoCliente.valorPrestamo.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
                plazo = infoCliente.plazo.ToString();
                valorCuota = infoCliente.valorCuota.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
                tipoCredito = infoCliente.tipoCredito == 2 ? "Crédito Hipotecario" : "Leasing Habitacional";
            }

            return new FormInsertRequest
            {
                CheckWorkflow = true,
                FormCode = FormCodes.Flujo83,
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new DodoHeader(),
                Data = new List<DodoDataField>().AddFlujo83Data(
                    identificacion: identificacionDto.Identificacion,
                    nombreCompleto: $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}",
                    tipoDocumento: $"{identificacionDto.TipoIdentificacion}",
                    correo: $"{cliente.Correo}",
                    prestamo: prestamo,
                    plazo: plazo,
                    valorCuota: valorCuota,
                    tipoCredito: tipoCredito)
            };
        }

        #endregion
    }
}