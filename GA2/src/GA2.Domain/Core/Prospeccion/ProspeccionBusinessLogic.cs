using AutoMapper;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Cartera;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.Simulador;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Application.Dto.Prospeccion;
using GA2.Application.Main;
using GA2.Domain.Core.Connected_Services.ServiceVigiaVerificacionTercero;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.ProductoPorCliente;
using GA2.Domain.Entities.Correo;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Prospeccion;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.EmailNotificacion;
using GA2.Transversals.Commons;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging; 
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class ProspeccionBusinessLogic : IProspeccionBusinessLogic
    {
        private const double ObjetivoEndeudameinto = 80;
        private int ConteoTU = 0;
        private int Score = 0;
        private decimal CuotaSectorReal = 0;
        private decimal CuotaSectorFinanciero = 0;
        private double EndeudamientoSCH = 0;
        private double EndeudamientoCCH = 0;
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly ICreditoRepository _creditoRepository;
        private readonly IOptions<AqmOptions> _optionsAqm;
        private readonly IOptions<IdVisionOptions> _optionsIDV;
        private readonly IClientRequestProvider _requestProvider;
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        private readonly IFenixBusinessLogic _fenixBusinessLogic;
        private readonly IVigiaBusinessLogic _vigiaBusinessLogic;
        private readonly IGestorIdVisionBusinessLogic _gestorIdVision;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly IMapper _mapper;
        private readonly IGestorTransUnionBusinessLogic _gestorActualizacionBusinessLogic;
        private readonly IReportesBusinessLogic _reportesBusinessLogic;
        private readonly IGestorTransUnionBusinessLogic _gestorTransUnionBusinessLogic;
        private readonly IVerificacionTerceroVigia _verificarTerceroVigia;
        private readonly ILogger<ProspeccionBusinessLogic> _logger;



        public ProspeccionBusinessLogic(ICreditoBusinessLogic creditoBusinessLogic,
                                        ICreditoRepository creditoRepository,
                                        IOptions<AqmOptions> optionsAqm,
                                        IClientRequestProvider requestProvider,
                                        ICatalogosBusinessLogic catalogsBusinessLogic,
                                        ICarteraBusinessLogic carteraBusinessLogic,
                                        IFenixBusinessLogic fenixBusinessLogic,
                                        IVigiaBusinessLogic vigiaBusinessLogic,
                                        IGestorIdVisionBusinessLogic gestorIdVision,
                                        IClientRequestProvider clientRequestProvider,
                                        IMapper mapper,
                                        IGestorTransUnionBusinessLogic gestorActualizacionBusinessLogic,
                                        IOptions<IdVisionOptions> optionsIDV,
                                        IReportesBusinessLogic reportesBusinessLogic,
                                        IGestorTransUnionBusinessLogic gestorTransUnionBusinessLogic, 
                                        IClienteRepository clienteRepository,
                                        IVerificacionTerceroVigia verificarTerceroVigia,
                                        ILogger<ProspeccionBusinessLogic> logger
                                        )
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _creditoRepository = creditoRepository;
            _optionsAqm = optionsAqm;
            _requestProvider = requestProvider;
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _carteraBusinessLogic = carteraBusinessLogic;
            _fenixBusinessLogic = fenixBusinessLogic;
            _vigiaBusinessLogic = vigiaBusinessLogic;
            _gestorIdVision = gestorIdVision;
            _clientRequestProvider = clientRequestProvider;
            _mapper = mapper;
            _gestorActualizacionBusinessLogic = gestorActualizacionBusinessLogic;
            _optionsIDV = optionsIDV;
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
            _gestorTransUnionBusinessLogic = gestorTransUnionBusinessLogic;
            _verificarTerceroVigia = verificarTerceroVigia;
            _logger = logger;

        }

        public async Task<Response<TipoCreditoSimulado>> ConsultarTipoCredito(ConsultarCreditoSimulacionDto creditoSimulacionDto)
        {
            

            ParametrosGenerales parametrosGenerales = new ParametrosGenerales();
            TipoCreditoSimulado tipoCredito = new TipoCreditoSimulado();

            var datosPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(creditoSimulacionDto.NumeroDocumento)).Data;

            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, creditoSimulacionDto.NumeroDocumento)).Data;


            bool pasoAValidacionAfilicionVeterano = false;
            bool pasoAValidacionAfilicionCesantias = false;

            if (creditoSimulacionDto.SimID == 2)
            {

                foreach (var solicitudes in cliente.Solicitudes)
                {

                    if (((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("AFILIADO VETERANO")) && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                        || ((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("SOLICITUD DE CRÉDITO AVAV")) && solicitudes.EstadoSolicitud.Equals("PROCESADA")))
                    {
                        pasoAValidacionAfilicionVeterano = true;

                    }

                    if (solicitudes.TipoSolicitud.Contains("SUBSIDIO (SEGUNDO PAGO)") && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                    {
                        pasoAValidacionAfilicionCesantias = true;

                    }

                }


                if (!pasoAValidacionAfilicionVeterano && !pasoAValidacionAfilicionCesantias && (cliente.NumeroCuotas >= 108 && cliente.NumeroCuotas <= 168) && (datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA))
                {
                    tipoCredito.TipoCredito = "LEASING 108 - 168";
                    await _creditoRepository.ActualizarTipoSimulador(creditoSimulacionDto.NumeroDocumento, tipoCredito.TipoCredito);
                    parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                    {
                        SimuladorId = (int)EnumSimuladorId.LEASING_108_168,
                        TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                        UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                        DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                    });
                }
                else if (!pasoAValidacionAfilicionVeterano && !pasoAValidacionAfilicionCesantias && (cliente.NumeroCuotas >= 24 && cliente.NumeroCuotas <= 107) && (datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA))
                {
                    tipoCredito.TipoCredito = "LEASING 24 - 107";
                    await _creditoRepository.ActualizarTipoSimulador(creditoSimulacionDto.NumeroDocumento, tipoCredito.TipoCredito);

                    parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                    {
                        SimuladorId = (int)EnumSimuladorId.LEASING_24_107,
                        TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                        UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                        DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                    });
                }
                else
                {
                    //Cesantias
                    if (pasoAValidacionAfilicionCesantias && datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_ADMINISTRACION_CESANTÍAS || datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.AFILIADO_ADMINISTRACION_CESANTÍAS && !pasoAValidacionAfilicionVeterano)
                    {
                        tipoCredito.TipoCredito = "LEASING CESANTIAS";
                        await _creditoRepository.ActualizarTipoSimulador(creditoSimulacionDto.NumeroDocumento, tipoCredito.TipoCredito);

                        parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                        {
                            SimuladorId = (int)EnumSimuladorId.LEASING_CESANTIAS,
                            TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                            UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                            DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                        });
                    }
                    else if (datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.VETERANOS || pasoAValidacionAfilicionVeterano)
                    {
                        tipoCredito.TipoCredito = "LEASING VETERANOS";
                        await _creditoRepository.ActualizarTipoSimulador(creditoSimulacionDto.NumeroDocumento, tipoCredito.TipoCredito);

                        parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                        {
                            SimuladorId = (int)EnumSimuladorId.LEASING_VETERANOS,
                            TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                            UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                            DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                        });
                    }
                    //Que deberia devolver si no es Hipotecario, Leasing?

                }

                tipoCredito.ParametrosGenerales = parametrosGenerales;

                return new Response<TipoCreditoSimulado> { Data = tipoCredito, ReturnMessage = "INFORMACION TIPO DE CRÉDITO LEASING" };
            }
            else
            {

                TasaSimulador tasaSimulador = new TasaSimulador();
               


                foreach (var solicitudes in cliente.Solicitudes)
                {

                    if (((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("AFILIADO VETERANO")) && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                        || ((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("SOLICITUD DE CRÉDITO AVAV")) && solicitudes.EstadoSolicitud.Equals("PROCESADA")))
                    {
                        pasoAValidacionAfilicionVeterano = true;

                    }

                  

                }

                var tipoActorRecursos = cliente.IdTipoActor;


                if ((!pasoAValidacionAfilicionVeterano && cliente.IdTipoActor == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_ADMINISTRACION_CESANTÍAS) || (!pasoAValidacionAfilicionVeterano && cliente.IdTipoActor == (int)EnumAfiliadoID.AFILIADO_ADMINISTRACION_CESANTÍAS))
                {

                    if (tipoActorRecursos == 2) //LEASING FUNCIONARIO CESANTIAS
                    {
                        tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO AFILIADO CESANTIAS");

                    }
                    else if (tipoActorRecursos == 16) //FUNCIONARIO AFILIADO CESANTIAS
                    {
                        tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO FUNCIONARIO CESANTIAS");
                    }

                    tipoCredito.TipoCredito = "HIPOTECARIO CENSANTIAS";

                    parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                    {
                        SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                        TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                        UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                        DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                    });
                }
                else if (pasoAValidacionAfilicionVeterano || cliente.IdTipoActor == (int)EnumAfiliadoID.VETERANOS)
                {

                    tipoCredito.TipoCredito = "HIPOTECARIO VETERANOS";

                    parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                    {
                        SimuladorId = (int)EnumSimuladorId.HIPOTECARIO_VETERANOS,
                        TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                        UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                        DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                    });
                }
                else if (!pasoAValidacionAfilicionVeterano && cliente.IdTipoActor == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || cliente.IdTipoActor == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA)
                {

                    if (tipoActorRecursos == 1) //LEASING FUNCIONARIO CESANTIAS
                    {
                        tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO AFILIADO SOLUCION VIVIENDA");

                    }
                    else if (tipoActorRecursos == 15) //FUNCIONARIO AFILIADO CESANTIAS
                    {
                        tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO FUNCIONARIO SOLUCION VIVIENDA");
                    }

                    tipoCredito.TipoCredito = "HIPOTECARIO SOLUCION VIVIENDA";

                    parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                    {
                        SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                        TipoAfiliacionId = Convert.ToInt32(cliente.IdTipoActor),
                        UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                        DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                    });
                }

                tipoCredito.ParametrosGenerales = parametrosGenerales;


                return new Response<TipoCreditoSimulado> { Data = tipoCredito, ReturnMessage = "INFORMACION TIPO DE CRÉDITO HIPOTECARIO" };

            }

        }

        public async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionCliente(CreacionSimulacionClienteDto request)
        {

            var methodName = nameof(CrearSimulacionCliente);

            try
            {
                Response<IEnumerable<SimulacionDto>> simulacion = new Response<IEnumerable<SimulacionDto>>(); 

                SimulacionDatosPersonalesDto simulacionDatos = new SimulacionDatosPersonalesDto();
                var datosPersonalesSimulacion = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento)).Data;



                simulacionDatos.ValorInmueble = (decimal)request.valor;
                simulacionDatos.AvaluoInmueble = (decimal)request.avaluoInmueble;
                simulacionDatos.NumeroDocumento = request.documento;
                simulacionDatos.TipoVivienda = request.tipoVivienda == "N" ? "3" : "4";
                simulacionDatos.Usr_id = request.Usr_id;
                simulacionDatos.DpdIdInmueble = datosPersonalesSimulacion.DpdIdInmueble;
                simulacionDatos.DpcIdInmueble = datosPersonalesSimulacion.DpcIdInmueble;
                simulacionDatos.CompraCartera = datosPersonalesSimulacion.CompraCartera;
                simulacionDatos.ValorPrestamoCompraCartera = (decimal)request.valorPrestamo;
                simulacionDatos.NumeroObligacionCompraCartera = request.NumeroObligacionSimulacion;

                await _creditoBusinessLogic.ActualizarPersonalesVivienda(simulacionDatos);

                var datosPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento)).Data;

                var idSimulacion = datosPersonales.IdSimulacion; 


                await _creditoRepository.RegistroLogTUSimulador("INICIA EL SIMULADOR", request.documento, ""); 


                await _creditoBusinessLogic.LimpiarSimulacionCliente(idSimulacion);
                if (request.simId != 3)
                {
                    request.ViviendaNueva = request.tipoVivienda.Equals("N") ? true : false;
                }
                if (request.simId == 1)//Hipotecario
                {
                    //var tasaSimulador = (await _carteraBusinessLogic.ObtenerTasasSimulador()).Data.First(t => t.Descripcion == "HIPOTECARIO AFILIADO SOLUCION VIVIENDA");
                    //request.plazo = request.plazo == 0 ? tasaSimulador.MaximoPlazoMeses : request.plazo; 

                    await _creditoRepository.RegistroLogTUSimulador("ENTRA POR CREDITO HIPOTECARIO", request.ToString(), "");

                    simulacion = await this.CrearSimulacionCreditoViviendaTradicional(request, datosPersonales, idSimulacion);

                }
                else if (request.simId == 2)//Leasing
                {



                    var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, request.documento)).Data;


                    bool pasoAValidacionAfilicionVeterano = false;
                    bool pasoAValidacionAfilicionCesantias = false;


                    foreach (var solicitudes in cliente.Solicitudes)
                    {

                        if (((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("AFILIADO VETERANO")) && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                            || ((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("SOLICITUD DE CRÉDITO AVAV")) && solicitudes.EstadoSolicitud.Equals("PROCESADA")))
                        {
                            pasoAValidacionAfilicionVeterano = true;

                        }

                        if (solicitudes.TipoSolicitud.Contains("SUBSIDIO (SEGUNDO PAGO)") && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                        {
                            pasoAValidacionAfilicionCesantias = true;

                        }

                    }

                    if (!pasoAValidacionAfilicionVeterano && !pasoAValidacionAfilicionCesantias && (datosPersonales.Cuotas >= 108 && datosPersonales.Cuotas <= 168) && (datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA))
                    {
                        await _creditoRepository.RegistroLogTUSimulador("ENTRA POR CREDITO LEASING 108", request.ToString(), "");

                        simulacion = await this.CrearSimulacionLeasing108(request, datosPersonales, idSimulacion);
                    }
                    else if (!pasoAValidacionAfilicionVeterano && !pasoAValidacionAfilicionCesantias && (datosPersonales.Cuotas >= 24 && datosPersonales.Cuotas <= 107) && (datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA))
                    {
                        await _creditoRepository.RegistroLogTUSimulador("ENTRA POR CREDITO LEASING 24", request.ToString(), "");

                        simulacion = await this.CrearSimulacionLeasing24(request, datosPersonales, idSimulacion);
                    }
                    else
                    {
                        //Cesantias
                        if (pasoAValidacionAfilicionCesantias && datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_ADMINISTRACION_CESANTÍAS || datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.AFILIADO_ADMINISTRACION_CESANTÍAS && !pasoAValidacionAfilicionVeterano)
                        {
                            await _creditoRepository.RegistroLogTUSimulador("ENTRA POR CREDITO LEASING CESANTIAS", request.ToString(), "");

                            simulacion = await this.CrearSimulacionLeasingCesantias(request, datosPersonales, idSimulacion);
                        }
                        else if (datosPersonales.TipoAfiliadoId == (int)EnumAfiliadoID.VETERANOS || pasoAValidacionAfilicionVeterano)
                        {
                            //Veteranos
                            await _creditoRepository.RegistroLogTUSimulador("ENTRA POR CREDITO LEASING VETERANOS", request.ToString(), "");

                            simulacion = await this.CrearSimulacionLeasingVeteranos(request, datosPersonales, idSimulacion);
                        }
                        else
                        {
                            return new Response<IEnumerable<SimulacionDto>> { Data = null, ReturnMessage = "Afiliado no cuenta con las condiciones necesarias para la solicitud de Crédito" };

                        }

                    } 
                }


                if (simulacion != null &&
                    simulacion.Data != null &&
                    simulacion.Data.Any(x => x.Sobrante != null))
                {

                    var datosPersonalesSimulacionDatos = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento)).Data;
                    RequestDocumentoSimDto requestDocumento = new RequestDocumentoSimDto();
                    
                    requestDocumento.idSimulacion = datosPersonalesSimulacionDatos.IdSimulacion;
                    requestDocumento.NumeroDocumento = request.documento; 

                    IdentificacionProspeccion identificacionProspeccion = new IdentificacionProspeccion();
                    identificacionProspeccion.numeroDocumento = request.documento;
                    identificacionProspeccion.SLS_ID = datosPersonalesSimulacionDatos.IdSimulacion; 

                    SolicitudSimulacion informacionSimulacion = await _creditoRepository.ObtenerSolicitudInformacionSimulacion(datosPersonalesSimulacionDatos.IdSimulacion);

                    if (simulacion?.Data != null)
                    {
                        foreach (var item in simulacion.Data)
                        {
                            if (!string.IsNullOrEmpty(item.Sobrante) && item.RespuestaRadicado)
                            { 
                                item.Sobrante += $" - RADICADO: {informacionSimulacion.SLS_NUMERO_RADICADO_83}";
                                await _reportesBusinessLogic.ReporteProspeccionCartaRechazo(identificacionProspeccion);
                                await _creditoBusinessLogic.CargarCartaRechazoProspeccionDODO(requestDocumento);

                            }
                        }
                    }

                }

                if(request.tipoSimularOProspectar == 3)
                {
                    await GuardarReporteTransUnion(idSimulacion, request.documento, datosPersonales.DepartamentoInmueble, datosPersonales.CiudadInmueble, request.simId, datosPersonalesSimulacion.CompraCartera, (decimal)request.valor, simulacion, request.Usr_id); 
                }

                return simulacion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[{method}] ERROR en simulación: {message}. Request: {@request}\\n"+
                    methodName + "\\n" +
                    ex.Message + "\\n" +
                    request);

                throw;
            }
            finally
            {
                _logger.LogInformation("[{method}] FINALIZA simulación\\n"+ methodName);
            }

        }

        private async Task<bool> GuardarReporteTransUnion(
        Guid idSimulacion,
        string numeroIdentificacion,
        string departamento,
        string ciudad,
        int tipoCredito,
        bool compraCartera,
        decimal valorInmueble,
        Response<IEnumerable<SimulacionDto>> simulacion,
        Guid? creadoPor)
        {
            Response<SocSlsSimulacionCreditoDto> validacion = new Response<SocSlsSimulacionCreditoDto>
            {
                Data = this._mapper.Map<SocSlsSimulacionCreditoDto>(
                        await this._creditoRepository.ObtenerSlsSocSolicitudCredito(idSimulacion))
            };

            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, numeroIdentificacion)).Data;

            string nombre = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}";

            string tipoCreditoSimulacion = tipoCredito == 1 ? "Crédito Hipotecario" : "Crédito Leasing"; 

            if(tipoCredito == 1 && compraCartera)
            {
                tipoCreditoSimulacion = "Compra de Cartera"; 
            }

            int estado = 1; //1 - VIABLE, 2 - PENDIENTE, 3 - RECHAZO

            string motivo = ""; 
            if (simulacion != null &&
                    simulacion.Data != null &&
                    simulacion.Data.Any(x => x.Sobrante != null))
            {

                if (simulacion?.Data != null)
                {
                    foreach (var item in simulacion.Data)
                    {
                        if (!string.IsNullOrEmpty(item.Sobrante) && item.RespuestaRadicado)
                        {
                            motivo = item.Sobrante;
                            estado = 3;
                        }
                        else
                        {
                            motivo = item.Sobrante;
                            estado = 2;
                        }
                    }
                }

            }

            var reporte = new ReporteProspeccionPreaprobado
            {
                RTU_FECHA_CONSULTA = validacion.Data.slsFecha,
                RTU_NUMERO_IDENTIFICACION = numeroIdentificacion,
                RTU_NOMBRE = nombre,
                RTU_DEPARTAMENTO = departamento,
                RTU_CIUDAD = ciudad,
                RTU_TIPO_CREDITO = tipoCreditoSimulacion,
                RTU_VALOR_INMUEBLE = valorInmueble,
                RTU_ESTADO = estado,
                RTU_MOTIVO = estado == 1 ? "Carta de preaprobado" : motivo,
                RTU_CREADO_POR = creadoPor
            };

            return await _creditoRepository.InsertarReporteTransUnion(reporte);
        }

        #region Hipotecario
        private async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionCreditoViviendaTradicional(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid simulacionId)
        {
            List<SimulacionDto> planillaSimulacion = new();
            List<TasaHogarCiudadDto> TasaHogarCiudad = new();           


            try
            {
                await _creditoBusinessLogic.LimpiarSimulacionCliente(simulacionId);
            }
            catch(Exception e)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: LimpiarSimulacionCliente; respuesta: "+e.Message,
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

            request.Usr_id = request.Usr_id == null ? null : request.Usr_id;
            var idSimulacionCliente = Guid.NewGuid();

            IEnumerable<EqivalenciaSimuladorDto> equivalenciasSimuladoCredito = null;
            DataClienteProspeccionDto cliente = null;
            ClienteDto recursos = null;

            try
            {
                _logger.LogInformation("[{method}]ObtenerEquivalenciasSimuladorCredito");
                equivalenciasSimuladoCredito =
                    (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;

            }
            catch (Exception e)
            {

                _logger.LogError("[{method}]ObtenerEquivalenciasSimuladorCredito", e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerEquivalenciasSimuladorCredito; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            try
            {
                _logger.LogInformation("[{method}]ObtenerClienteProspeccion");

                cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto
                {
                    TipoIdentificacion = request.tipoIdentificacion,
                    NumeroDocumento = request.documento
                })).Data;
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerClienteProspeccion: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerClienteProspeccion; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            try
            {
                _logger.LogInformation("[{method}]ObtenerClientePorTipoIdentificationYNumero");

                recursos =
                    (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(
                        request.tipoIdentificacion,
                        request.documento)).Data;
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerClientePorTipoIdentificationYNumero: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerClientePorTipoIdentificationYNumero; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            User usuarioEncontrado = null;

            try
            {
                _logger.LogInformation("[{method}]ConsultarUsuarioPorId");

                usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ConsultarUsuarioPorId: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ConsultarUsuarioPorId; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            var correoLogueado = usuarioEncontrado == null ? "" : usuarioEncontrado.USR_USERNAME;


            #region ParametrosGeneralesXTipoAfiliacion
            ParametrosGenerales parametrosGenerales = new ParametrosGenerales();
            TasaSimulador tasaSimulador = new TasaSimulador();
            bool pasoAValidacionAfilicionVeterano = false;
            bool pasoAValidacionAfilicionCesantias = false;


            foreach (var solicitudes in recursos.Solicitudes)
            {

                if (((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("AFILIADO VETERANO")) && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                    || ((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("SOLICITUD DE CRÉDITO AVAV")) && solicitudes.EstadoSolicitud.Equals("PROCESADA")))
                {
                    pasoAValidacionAfilicionVeterano = true;

                }



            }

            var tipoActorRecursos = recursos.IdTipoActor;


            if ((!pasoAValidacionAfilicionVeterano && recursos.IdTipoActor == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_ADMINISTRACION_CESANTÍAS) || (!pasoAValidacionAfilicionVeterano && recursos.IdTipoActor == (int)EnumAfiliadoID.AFILIADO_ADMINISTRACION_CESANTÍAS))
            {

                if (tipoActorRecursos == 2) //LEASING FUNCIONARIO CESANTIAS
                {
                    tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO AFILIADO CESANTIAS");

                }
                else if (tipoActorRecursos == 16) //FUNCIONARIO AFILIADO CESANTIAS
                {
                    tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO FUNCIONARIO CESANTIAS");
                }


                parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                {
                    SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                    TipoAfiliacionId = Convert.ToInt32(recursos.IdTipoActor),
                    UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                    DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                });
            }
            else if (pasoAValidacionAfilicionVeterano || recursos.IdTipoActor == (int)EnumAfiliadoID.VETERANOS)
            {


                parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                {
                    SimuladorId = (int)EnumSimuladorId.HIPOTECARIO_VETERANOS,
                    TipoAfiliacionId = Convert.ToInt32(recursos.IdTipoActor),
                    UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                    DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                });
            }
            else if (!pasoAValidacionAfilicionVeterano && recursos.IdTipoActor == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || recursos.IdTipoActor == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA)
            {

                if (tipoActorRecursos == 1) //LEASING FUNCIONARIO CESANTIAS
                {
                    tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO AFILIADO SOLUCION VIVIENDA");

                }
                else if (tipoActorRecursos == 15) //FUNCIONARIO AFILIADO CESANTIAS
                {
                    tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("HIPOTECARIO FUNCIONARIO SOLUCION VIVIENDA");
                }


                parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                {
                    SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                    TipoAfiliacionId = Convert.ToInt32(recursos.IdTipoActor),
                    UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                    DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                });
            }
            #endregion
            if (parametrosGenerales.Status != string.Empty)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = parametrosGenerales.Status
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

            

            DatosFinancierosPersonalesDto datosFinancieros = null;
            IEnumerable<TipoViviendaDto> tiposVivienda = null;

            try
            {
                _logger.LogInformation("[{method}]ObtenerDatosFinancierosPersonales");

                datosFinancieros =
                    (await _creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerDatosFinancierosPersonales: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerDatosFinancierosPersonales; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            try
            {
                _logger.LogInformation("[{method}]ObtenerTiposDeVivienda");

                tiposVivienda =
                    (await _catalogsBusinessLogic.ObtenerTiposDeVivienda()).Data;
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerTiposDeVivienda: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerTiposDeVivienda; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }



            var ValorViviendaSeg = request.ValorViviendaSeguro > 0 ? request.ValorViviendaSeguro : request.valor;
            // decimal valorSeguroHogar = (decimal)RedondearCifras(((double)ValorViviendaSeg * (double)parametrosGenerales.Tasas.SeguroHogar)).Result;

            var idActor = recursos.IdTipoActor;
            var tipoVinculacionHipotecario = ""; 
            foreach(var tipoVinculacion in recursos.TiposVinculacion)
            {
                if(idActor == tipoVinculacion.IdTipoActor)
                {
                    tipoVinculacionHipotecario = tipoVinculacion.DescripcionTipoActor; break;
                }
            }

            try
            {
                _logger.LogInformation("[{method}]LimpiarRecursosCongelados");

                await _creditoRepository.LimpiarRecursosCongelados(simulacionId, request.documento);
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]LimpiarRecursosCongelados: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: LimpiarRecursosCongelados; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            var datoSeguroSimulacion = await _creditoRepository.ConsultarSeguroSimulacionCredito(request.documento);

      


            decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;            

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",    
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                }
            };

            foreach (var recurso in recursosCongelados)
            {
                try
                {
                    _logger.LogInformation("[{method}]InsertarRecursosCongelados");

                    await _creditoRepository.InsertarRecursosCongelados(recurso);
                }
                catch (Exception e)
                {
                    _logger.LogError("[{method}]InsertarRecursosCongelados: "+ e);

                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Metodo: InsertarRecursosCongelados; respuesta: " + e.Message,
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }

            }


            try
            {
                _logger.LogInformation("[{method}]LimpiarConceptosCongelados");

                await _creditoRepository.LimpiarConceptosCongelados(simulacionId, request.documento);
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]LimpiarConceptosCongelados: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: LimpiarConceptosCongelados; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }



            List<string> descripcionesConceptos = new List<string>
            {
                "AHORROS OBLIGATORIOS",
                "AHORROS RETROACTIVOS",
                "AHORROS VOLUNTARIOS",
                "INTERESES APORTES",
                "CESANTÍAS",
                "CESANTÍAS RETROACTIVOS",
                "INTERESES CESANTÍAS"
            };

            List<ConceptosCongeladosProspeccion> listaCongeladosProspeccion = new List<ConceptosCongeladosProspeccion>();

            foreach (var cuentas in recursos.Cuentas)
            {
                foreach (var conceptos in cuentas.Conceptos)
                {
                    if (descripcionesConceptos.Contains(conceptos.ConceptoDescripcion))
                    {
                        listaCongeladosProspeccion.Add(new ConceptosCongeladosProspeccion
                        {
                            CON_DESCRIPCION = conceptos.ConceptoDescripcion,
                            CON_VALOR = conceptos.saldo,
                            SLS_ID = simulacionId,
                            CON_NUMERO_DOCUMENTO = request.documento
                        });
                    }
                }
            }



            foreach (var concepto in listaCongeladosProspeccion)
            {
                try
                {
                    _logger.LogInformation("[{method}]InsertarConceptosCongelados: "+concepto.CON_DESCRIPCION);

                    await _creditoRepository.InsertarConceptosCongelados(concepto);
                }
                catch (Exception e)
                {
                    _logger.LogError("[{method}]InsertarConceptosCongelados: "+ e);

                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Metodo: InsertarConceptosCongelados; respuesta: " + e.Message,
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }

            }


            var valorAvaluo = request.avaluoInmueble; 
            if (request.avaluoInmueble == 0)
            {
                valorAvaluo = (decimal)request.ValorViviendaSeguro; 
            }

            

            if (datoSeguroSimulacion.SSC_CONVENIO_HOGAR == "N")
            {
                parametrosGenerales.Tasas.SeguroHogar = 0;
            }
            decimal valorSeguroHogar = (decimal)request.valor > valorAvaluo ?  (decimal)((double)request.valor * (double)parametrosGenerales.Tasas.SeguroHogar) : (decimal)((double)valorAvaluo * (double)parametrosGenerales.Tasas.SeguroHogar);

            var viviendaVis = "";

            if ((request.valor / parametrosGenerales.SalarioMinimo) <= parametrosGenerales.LimiteVIP)
            {
                viviendaVis = "VIP";
            }
            else
            {
                if ((request.valor / parametrosGenerales.SalarioMinimo) <= parametrosGenerales.LimiteVIS)
                {
                    viviendaVis = "VIS";
                }
                else
                {
                    viviendaVis = "NO VIS";
                }
            }

            //verificar tipo de vivienda vis no vis
            var vivienda = 0;
            if (viviendaVis.Equals("VIP"))
            {
                vivienda = 1;
            }
            else
            {
                if (viviendaVis.Equals("VIS"))
                {
                    vivienda = 2;
                }
                else
                {
                    vivienda = 3;
                }
            }


            ProyeccionClienteDto proyeccionactual = null;
            CargoCategoriaDto subsidioCategoria = null;

            // 1. ObtenerProyeccionCliente
            try
            {
                _logger.LogInformation("[{method}]ObtenerProyeccionCliente");

                proyeccionactual =
                    (await _creditoBusinessLogic.ObtenerProyeccionCliente(cliente.GradoId.ToString())).Data;
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerProyeccionCliente: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerProyeccionCliente; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            // 2. ObtenerSimulacionCargoPorCategoria
            try
            {
                _logger.LogInformation("[{method}]ObtenerSimulacionCargoPorCategoria");

                subsidioCategoria =
                    (await _catalogsBusinessLogic.ObtenerSimulacionCargoPorCategoria(
                        cliente.CategoriaId,
                        proyeccionactual.Id)).Data;
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerSimulacionCargoPorCategoria: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerSimulacionCargoPorCategoria; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }


            var plazo = 0;
            double valorSubsidio = 0;
            decimal subsidioPresente = 0;
            var cuotasFaltanteasAlDesembolso = 0;

            
            if (request.AbonoUnico)
            {
                //Asigno plazo maximo seguntabla
                var factorIncremento = 0.04m;
                var cuotasFaltantes = 168 - datosPersonales.Cuotas;
                cuotasFaltanteasAlDesembolso = cuotasFaltantes - 3;
                var aniosParaSubsidio = (int)(cuotasFaltanteasAlDesembolso / 12);
                // pendiente el vaor del salario debe ser de los parametros. OJO CAMBIAR
                //valorSubsidio = (double)subsidioCategoria.FactorSubsidio * salariobasico * (Math.Pow((double)(1 + factorIncremento), aniosParaSubsidio));
                valorSubsidio = (double)subsidioCategoria.FactorSubsidio * parametrosGenerales.SalarioMinimo;

                var algo = Math.Pow(1 + (double)parametrosGenerales.Tasas.Mixta, cuotasFaltanteasAlDesembolso * -1);
                var otroalgo = valorSubsidio * algo;
                subsidioPresente = (decimal)(otroalgo);
                if (request.plazo == 0)
                    plazo = parametrosGenerales.MaximoPlazoMeses <= cuotasFaltanteasAlDesembolso ? parametrosGenerales.MaximoPlazoMeses : cuotasFaltanteasAlDesembolso;
                else if (request.plazo <= parametrosGenerales.MaximoPlazoMeses)
                    plazo = request.plazo;
            }
            else
            {
                if (request.plazo == 0 || request.plazo == parametrosGenerales.MaximoPlazoMeses)
                    plazo = parametrosGenerales.MaximoPlazoMeses;
                else if (request.plazo < parametrosGenerales.MaximoPlazoMeses)
                    plazo = request.plazo;
            }

            var tipoAlivio = string.Empty;
            var alivioTasa = 0.0;
            var alivioTasaMV = 0.0;
            var alivioCuota = 0.0M;
            if (request.AplicaBeneficio)
            {
                if (vivienda.Equals(1) || vivienda.Equals(2))
                {
                    tipoAlivio = "TASA FRECH";
                    if (vivienda.Equals(1))
                    {
                        alivioTasa = 0.05;
                        alivioTasaMV = Math.Pow(1 + alivioTasa, 0.0833333333333333) - 1;
                    }
                    else
                    {
                        alivioTasa = 0.04;
                        alivioTasaMV = Math.Pow(1 + alivioTasa, 0.0833333333333333) - 1;
                    }
                }
                else if (request.valor / parametrosGenerales.SalarioMinimo <= 500)
                {
                    tipoAlivio = "ALIVIO CUOTA";
                    alivioCuota = 42M * parametrosGenerales.SalarioMinimo / 84M;
                }
            }

            //Calculo el factor (tasaMixta*((1+Tasamixta)^plazo))/((1+tasaMixta)^plazo)-1
            var firstP = parametrosGenerales.Tasas.Mixta * (decimal)Math.Pow((1 + (double)parametrosGenerales.Tasas.Mixta), plazo);
            var secondP = (decimal)(Math.Pow(1 + (double)parametrosGenerales.Tasas.Mixta, plazo));
            var factor = firstP / (secondP - 1);

            

            var valorMinimoCredito = parametrosGenerales.ValorMinimoCuota / factor;

            //obtengo total ingresos y obtengo total egresos
            var totalIngresos = (double)datosFinancieros.TotalIngresos;
            var totalDescuentos = (double)datosFinancieros.TotalEgresos;

            //calculo cuota max totalIngresos*0.5-totalEgresos
            var cuotaMax = (decimal)((totalIngresos * 0.5) - totalDescuentos);
            var cuotaMaxdos = cuotaMax;
            if (request.valorCuota != 0 && request.valorCuota > (double)cuotaMax)
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "CUOTA EXCEDE EL MÁXIMO PERMITIDO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            else if(request.valorCuota>0)
            {
                cuotaMax = (decimal)request.valorCuota;
            }
            //si tiene abono extra mensual calculo el abono extra mensual
            var abonoExtra = 0M;
            var porAbonoExtra = 0.21;
            var interesesCuota = 0.0;
            if (request.TipoAbono == 2)
            {
                request.AbonoExtra = true;
                abonoExtra = (datosFinancieros.ValorSalario * 21) / 100;
                cuotaMaxdos = cuotaMax;
                cuotaMax = cuotaMax + abonoExtra;
            }
            decimal AbonoExtraCalculado = 0;
            decimal proyeccionAbonoExtra = 0;
            if (request.TipoAbono == 3)
            {
                request.AbonoExtra = true;
                var auxiliarFecha = DateTime.Now.AddMonths(3);
                for (var x = 0; x <= plazo; x++)
                {
                    AbonoExtraCalculado = 0;
                    if (auxiliarFecha.AddMonths(x).Month == 12)
                    {
                        if (x <= 12)
                            AbonoExtraCalculado = datosFinancieros.ValorSalario * (decimal)0.21 * x;
                        else
                            AbonoExtraCalculado = datosFinancieros.ValorSalario * (decimal)0.21 * 12;
                        proyeccionAbonoExtra += AbonoExtraCalculado * (decimal)(Math.Pow((1 + (double)parametrosGenerales.Tasas.Mixta), x * -1));
                    }

                }
            }
            //calculo valor credito segun cuota (cuotaMax-valorSeguro)/factor
            var creditoSegunCuota = Math.Round(((decimal)cuotaMax - valorSeguroHogar) / parametrosGenerales.Tasas.Mixta, 5);
            //calculo valor credito segun vivienda 
            var creditoSegunVivienda = (double)0;

            Response<SimulacionDatosPersonalesDto> simulacionDatosPersonales = await this._creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento);




            if (request.AbonoUnico)
            {
                creditoSegunCuota = (subsidioPresente + ((decimal)cuotaMax - valorSeguroHogar) / parametrosGenerales.Tasas.Mixta);
            }


            if (simulacionDatosPersonales.Data.CompraCartera)
            {
                creditoSegunVivienda = (double)request.valor;
            }
            else
            {
                if (vivienda == 1 || vivienda == 2)
                {
                    creditoSegunVivienda = (double)(request.valor * 0.80);
                }
                else
                {
                    creditoSegunVivienda = (double)(request.valor * 0.70);
                }
            }

            //150.000.000
            if (request.valorPrestamo > 0 && request.valorPrestamo < creditoSegunVivienda)
            {
                creditoSegunVivienda = request.valorPrestamo;
            }

            if (request.valorPrestamo > 0 && (decimal)request.valorPrestamo < creditoSegunCuota)
            {
                creditoSegunCuota = (decimal)request.valorPrestamo;
            }

            var abonomes = (datosFinancieros.ValorSalario * (decimal)0.21);
            //var seguhog = (double)request.valor * (double)tasaSeguroHogar;
            //var tasacompuesta = ((decimal)tasaMesVencido + (decimal)seguroVidaTasa);

            /// SE DEBE QUITAR
            var fechaSolicitud = DateTime.Now;
            //creditoSegunCuota = (double)proyeccionAbonoExtra + ((double)cuotaMax - ((double)request.valor * (double)tasaSeguroHogar)) / factor;
            //creditoSegunCuota = (double)(datosFinancieros.ValorSalario * (decimal)0.21) + ((double)cuotaMax - ((double)request.valor * (double)tasaSeguroHogar)) / factor;
            //valorSectorFinan += ((parametrosGenerales.Tasas.Mixta 2) * parametrosGenerales.Tasas.Mixta) / (1 - Math.Pow(1 + parametrosGenerales.Tasas.Mixta, -numeroCuotasRestantes));


            if (request.TipoAbono == 3)
                creditoSegunCuota = (((decimal)cuotaMax + abonomes - valorSeguroHogar)) / parametrosGenerales.Tasas.Mixta;

            


            var creditoSegunIntereses = ((((decimal)cuotaMax - (decimal)valorSeguroHogar)) / parametrosGenerales.Tasas.Mixta);

            if(request.valorPrestamo > 0 && (decimal)request.valorPrestamo < creditoSegunIntereses)
            {
                creditoSegunIntereses = (decimal)request.valorPrestamo; 
            }

            //calculo el credito segun los intereses (cuotaMax-valorSeguro)/tasaMixta)*0.9999
            //var creditoSegunIntereses = ((((decimal)cuotaMax - (decimal)valorSeguro) * 0.9999M) / ((decimal)tasaMixta + (decimal)seguroVidaTasa));
            //selecciono el valor menos de los creditos y se lo asigno a valor prestamo
            decimal valorPrestamo = 0;
            var listaValores = new double[]
            {
                creditoSegunVivienda,
                (double)creditoSegunCuota,
                (double)creditoSegunIntereses
            };

            if (simulacionDatosPersonales.Data.CompraCartera)
            {
                valorPrestamo = (decimal)creditoSegunVivienda;
            }
            else
            {
                valorPrestamo = (decimal)listaValores.Min<double>();
            }
            //valorPrestamo = (decimal)creditoSegunVivienda; 

            if (request.valorPrestamo > 0 && (decimal)request.valorPrestamo > valorPrestamo)
            {
                
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "VALOR DEL PRESTAMO NO PUEDE SER MAYOR AL VALOR DEL INMUEBLE", 
                    RespuestaRadicado = false
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

                if (request.valorPrestamo > 0 && (decimal)request.valorPrestamo < valorPrestamo)
                valorPrestamo = (int)request.valorPrestamo;


            if (simulacionDatosPersonales.Data.CompraCartera)
            {
                valorPrestamo = (decimal)creditoSegunVivienda;
            }
            else
            {
                valorPrestamo = Math.Max(valorPrestamo, valorMinimoCredito);

            }

            //calculo la cuota del credito valorPrestamo*factor+valorSeguro
            //var cuota = request.TipoAbono == 2 || request.TipoAbono == 3 ? (decimal)RedondearCifras((double)(valorPrestamo * factor + valorSeguroHogar)).Result : (decimal)RedondearCifras((double)(valorPrestamo * factor + valorSeguroHogar)).Result;
            var cuota = (decimal)(valorPrestamo * factor) + valorSeguroHogar;


            var tasaMixtaCalculada = (decimal)((double)parametrosGenerales.Tasas.Mixta * (Math.Pow((double)(1 + parametrosGenerales.Tasas.Mixta), plazo))) / (decimal)((Math.Pow((double)(1 + parametrosGenerales.Tasas.Mixta), plazo))-1); 
            
            if (request.valorPrestamo == 0 && cuotaMax < cuota)
            {
                valorPrestamo = (((decimal)cuotaMax - (decimal)valorSeguroHogar)) / tasaMixtaCalculada; 
            }

            if (request.valorCuota != 0 && request.valorCuota <= (double)cuotaMax)
                cuota = (decimal)RedondearCifras(request.valorCuota).Result;
            if ((decimal)RedondearCifras((double)valorPrestamo).Result != (decimal)RedondearCifras((double)creditoSegunCuota).Result)
            {
                cuota = (decimal)RedondearCifras((double)((valorPrestamo * factor) + valorSeguroHogar)).Result;
            }
            var proyAbonoExtra = 0M;
            var cuotaSinSeguros = 0M;
            decimal cuotaLibranza = cuota;
            //if (cuota > cuotaMax)
            //    cuota = cuotaMax


            SolicitudCredito solicitudGeneralCredito = null;

            try
            {
                _logger.LogInformation("[{method}]ObtenerSolicitudCreditoTipoNumeroIdentificacion");

                solicitudGeneralCredito =
                    await _creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, request.documento);
            }
            catch (Exception e)
            {
                _logger.LogError("[{method}]ObtenerSolicitudCreditoTipoNumeroIdentificacion: "+ e);

                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "Metodo: ObtenerSolicitudCreditoTipoNumeroIdentificacion; respuesta: " + e.Message,
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            DateTime fechaDesembolsoRespuesta = DateTime.Now; 


            if (plazo < 0)
                plazo = 0;
            if (!request.AplicaBeneficio)
            {
                for (int i = 0; i <= plazo; i++)
                {                    
                    if (i == 0)
                    {
                        DateTime fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Saldo = (double)valorPrestamo,
                            Cuota = i,
                            ViviendaVis = viviendaVis,
                            ValorVivienda = request.valor,
                            ValorPrestamo = valorPrestamo,
                            TotalCuota = (double)(cuota - abonoExtra),
                            TotalCuotaSinSeguros = (double)(cuota - valorSeguroHogar),
                            PlazoMeses = plazo,
                            PlazoAnos = plazo / 12,
                            ValorTasa = parametrosGenerales.Tasas.EfectivaAnual,
                            ValorTasaNominal = parametrosGenerales.Tasas.NominalMesVencido,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento,
                        });

                    }
                    else
                    {

                        var VrCreditoSegHogar = 0.0;
                        var valorViviendaDeseado = request.valor;
                        var tasaSeguroVida = (double)(parametrosGenerales.Tasas.SeguroVida);
                        if (datoSeguroSimulacion.SSC_CONVENIO_VIDA == "N")
                        {
                            tasaSeguroVida = 0;
                        }

                      

                        VrCreditoSegHogar = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

                        var seguroVidaCuota = Math.Round(planillaSimulacion[i - 1].Saldo * tasaSeguroVida,3);


                        interesesCuota = !request.AbonoUnico ? planillaSimulacion[i - 1].Saldo * (double)parametrosGenerales.Tasas.NominalMesVencido : planillaSimulacion[i - 1].Saldo * (double)parametrosGenerales.Tasas.NominalMesVencido;
                        var capital = 0M;
                        if (i == plazo)
                        {
                            capital = (decimal)planillaSimulacion[i - 1].Saldo;
                            cuota = capital + (decimal)interesesCuota + (decimal)seguroVidaCuota + valorSeguroHogar;
                        }
                        else
                        {
                            capital = cuota - valorSeguroHogar - (decimal)seguroVidaCuota - (decimal)interesesCuota;
                        }

                        var fechaCorte = new DateTime(planillaSimulacion[i - 1].FechaCorte.Year, planillaSimulacion[i - 1].FechaCorte.Month, 1).AddMonths(2).AddDays(-1);
                        int mes = fechaCorte.Month;
                        int año = fechaCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        var abonoUnico = request.AbonoUnico ? i == cuotasFaltanteasAlDesembolso ? valorSubsidio : 0 : 0;
                        if (i == 1)
                            cuotaSinSeguros = cuota - valorSeguroHogar;
                        if (request.TipoAbono == 3)
                        {
                            abonoExtra = fechaSolicitud.AddMonths(i).Month == 12
                                        ? i < 12
                                            ? datosFinancieros.ValorSalario * (decimal)porAbonoExtra * i
                                            : datosFinancieros.ValorSalario * (decimal)porAbonoExtra * 12
                                        : 0;
                            capital = (cuota + abonoExtra) - ((decimal)interesesCuota + valorSeguroHogar + (decimal)seguroVidaCuota);
                            proyAbonoExtra += abonoExtra * (decimal)Math.Pow(1 + (double)parametrosGenerales.Tasas.Mixta, i * -1);
                        }

                        var valorSaldo = 0.0;
                        if (abonoUnico != 0)
                        {
                            if ((double)(planillaSimulacion[i - 1].Saldo - (double)capital) < 1)
                            {
                                valorSaldo = 0.0;
                            }
                            else
                            {
                                valorSaldo = (double)(planillaSimulacion[i - 1].Saldo - abonoUnico - (double)capital);
                            }
                        }
                        else
                        {
                            if (abonoExtra != 0)
                            {
                                valorSaldo = (double)(planillaSimulacion[i - 1].Saldo - (double)capital);
                            }
                            else
                            {
                                valorSaldo = (double)(planillaSimulacion[i - 1].Saldo - (double)capital);
                            }

                        }

                        decimal temporal1 = 0.0M; //cuota unidad pagadora
                        decimal temporal2 = 0.0M; //cuota abono extra

                        if (request.TipoAbono == 3)
                        {
                            temporal1 = cuota;
                            temporal2 = abonoExtra;
                        }
                        else
                        {
                            if (cuota > cuotaMax)
                            {
                                temporal1 = cuota;
                                temporal2 = cuota - cuotaMax;
                            }
                            else
                            {
                                temporal1 = cuota;
                                temporal2 = 0;
                            }
                        }

                        double seguroVidaProrrata = 0;
                        double seguroHogarProrrata = 0;


                        if (i == 1)
                        {
                            if (solicitudGeneralCredito != null)
                            {

                                var fecha = request.FechaDesembolso;

                                if (fecha < new DateTime(1753, 1, 1))
                                {
                                    fecha = DateTime.Now; // o null
                                }

                                await _creditoRepository.ActualizarFechaDesembolsoCredito(
                                    solicitudGeneralCredito.SOC_ID,
                                    fecha);

                                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(solicitudGeneralCredito.SOC_ID);

                                if (datosFactura != null && datosFactura.FAC_NUMERO_FACTURA != null)
                                {
                                    SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(solicitudGeneralCredito.SOC_ID);

                                    DateTime? fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;
                                    fechaDesembolsoRespuesta = informacionDesembolso.SID_FECHA_DESEMBOLSO; 
                                    DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));

                                    var diasProrrata = (ultimoDia - informacionDesembolso.SID_FECHA_DESEMBOLSO).Days;
                                    seguroVidaProrrata = Math.Round((double)seguroVidaCuota * (diasProrrata + 1) / 30, 0);
                                    seguroHogarProrrata = Math.Round((double)valorSeguroHogar / 30, 0) * (diasProrrata + 0);
                                }
                            }
                        }


                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Cuota = i,
                            FechaCorte = fechaCorte,
                            FechaPago = fechaPrimerCortedos,
                            Capital = Math.Round((double)capital, 2, MidpointRounding.AwayFromZero),
                            Saldo = Math.Round(i == plazo ? Math.Abs(valorSaldo) : valorSaldo, 2, MidpointRounding.AwayFromZero),
                            Intereses = Math.Round(interesesCuota, 2, MidpointRounding.AwayFromZero),
                            SeguroVida = Math.Round(seguroVidaCuota + seguroVidaProrrata, 2, MidpointRounding.AwayFromZero),
                            SeguroHogar = Math.Round((double)valorSeguroHogar + seguroHogarProrrata, 2, MidpointRounding.AwayFromZero),
                            TotalCuota = Math.Round((double)(cuota - abonoExtra) + seguroHogarProrrata + seguroVidaProrrata, 2, MidpointRounding.AwayFromZero),
                            CuotaUnidadPagadora = (decimal)temporal1 + (decimal)seguroHogarProrrata + (decimal)seguroVidaProrrata,
                            ValorAbonoExtra = abonoExtra,
                            AbonoUnico = (decimal)abonoUnico,
                            AbonoExtra = (double)temporal2,
                            CuotaUnidadPagadorados = (decimal)cuotaMaxdos,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento
                        });
                        if (planillaSimulacion[i].Saldo <= 0)
                        {
                            plazo = i;
                            planillaSimulacion[0].PlazoMeses = i;
                            planillaSimulacion[0].PlazoAnos = (i) / 12;
                            break;
                        }
                    }
                };
            }
            else
            {
                for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        var fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Saldo = (double)valorPrestamo,
                            Cuota = i,
                            ViviendaVis = viviendaVis,
                            ValorVivienda = request.valor,
                            ValorPrestamo = (decimal)valorPrestamo,
                            TotalCuota = (double)cuota,
                            TotalCuotaSinSeguros = (double)(cuota - valorSeguroHogar),
                            PlazoMeses = plazo,
                            PlazoAnos = plazo / 12,
                            ValorTasa = parametrosGenerales.Tasas.EfectivaAnual,
                            ValorTasaNominal = (decimal)interesesCuota / valorPrestamo,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            ValorAbonoExtra = abonoExtra,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento
                        });
                    }
                    else
                    {
                        decimal reduccionInteres = (tipoAlivio.Equals("TASA FRECH")) && i <= 84 ? (decimal)(planillaSimulacion[i - 1].Saldo * alivioTasaMV) : 0m;
                        var reduccionCuota = 0.0m;
                        if (tipoAlivio.Equals("ALIVIO CUOTA") && i <= 84 && planillaSimulacion[i - 1].Saldo > 0)
                        {
                            reduccionCuota = (decimal)alivioCuota;
                        }

                        var VrCreditoSegHogar = 0.0;
                        var valorViviendaDeseado = request.valor;
                        var tasaSeguroVida = (double)(parametrosGenerales.Tasas.SeguroVida);


                        VrCreditoSegHogar = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

                        var seguroVidaCuota = VrCreditoSegHogar * tasaSeguroVida;


                        interesesCuota = planillaSimulacion[i - 1].Saldo * (double)parametrosGenerales.Tasas.NominalMesVencido - (double)reduccionInteres;
                        //var seguroHogar = planillaSimulacion[i - 1].Saldo > 0 ? (double)parametrosGenerales.Tasas.SeguroHogar * request.valor : 0;

                        decimal seguroHogar = (decimal)request.valor > request.avaluoInmueble ? (decimal)((double)request.valor * (double)parametrosGenerales.Tasas.SeguroHogar) : (decimal)((double)request.avaluoInmueble * (double)parametrosGenerales.Tasas.SeguroHogar);

                        var cobroFonvivienda = reduccionInteres <= reduccionCuota ? alivioCuota : reduccionInteres;
                        var capital = i <= 84 ? ((double)cuota + (double)cobroFonvivienda - (double)seguroHogar - seguroVidaCuota - interesesCuota) : ((double)cuota - (double)seguroHogar - seguroVidaCuota - interesesCuota);
                        var fechaCorte = planillaSimulacion[i - 1].FechaCorte.AddMonths(1);
                        int mes = fechaCorte.Month;
                        int año = fechaCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        var abonoUnico = request.AbonoUnico ? i == cuotasFaltanteasAlDesembolso ? valorSubsidio : 0 : 0;
                        if (i == 1)
                            cuotaSinSeguros = (decimal)((double)cuota - (double)seguroHogar);
                        var saldo = planillaSimulacion[i - 1].Saldo > 0 ? (double)(planillaSimulacion[i - 1].Saldo - capital) : 0;

                   
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Cuota = i,
                            FechaCorte = fechaCorte,
                            FechaPago = fechaPrimerCortedos,
                            Capital = (double)capital,
                            Saldo = saldo,
                            Intereses = interesesCuota,
                            SeguroVida = seguroVidaCuota,
                            SeguroHogar = (double)seguroHogar,
                            TotalCuota = (double)cuota,
                            CuotaUnidadPagadora = cuota,
                            ValorAbonoExtra = abonoExtra,
                            AbonoUnico = (decimal)abonoUnico,
                            InteresesReduccion = (double)reduccionInteres,
                            CobroFonvivienda = i <= 84 ? (double)cobroFonvivienda : 0,
                            CuotaReduccion = (double)reduccionCuota,
                            TotalIntereses = (decimal)interesesCuota + reduccionInteres, 
                            Afl_id = request.afl_id, 
                            NumeroDocumento = request.documento
                        });
                        if (planillaSimulacion[i].Saldo <= 0)
                        {
                            plazo = i;
                            planillaSimulacion[0].PlazoMeses = i;
                            planillaSimulacion[0].PlazoAnos = (i) / 12;
                            break;
                        }
                    }
                };
            }

            var vinculacion = cliente.TipoAfiliacion;
            ValidacionCreditoProspeccion validacionesExcepciones = new ValidacionCreditoProspeccion();

            if (validacionesExcepciones == null)
            {
                validacionesExcepciones.VCP_ESTADO = false;
                validacionesExcepciones.VCP_NUMERO_DOCUMENTO = request.documento;

                await _creditoRepository.InsertarActualizarValidacionCreditoProspeccion(validacionesExcepciones);
            }

            ValidacionCreditoProspeccion validacionesExcepcion = await _creditoRepository.ConsultarValidacionCreditoProspeccion(request.documento);



            double totalObligaciones = 0;
            #region EndeudamientoGlobal
            if (parametrosGenerales.ActiveTU && request.tipoSimularOProspectar==3)
            {
                _logger.LogInformation("Entro en la condición TU activo y Preaprobado");

                try
                {
                    #region ConsumoTU
                    ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                    {
                        Apellidos = recursos.PrimerApellido,
                        FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                        Identificacion = request.documento,
                        SalarioMensual = Math.Round((decimal)totalIngresos),
                        TipoIdentificacion = request.tipoIdentificacion,
                        Telefono = recursos.Telefono,
                        TraerHistorialCrediticio = "Y",
                        SLS_ID = simulacionId
                    };

                    if (prospeccionTransUnionRequestDto.Telefono.Length < 10 || prospeccionTransUnionRequestDto.Telefono == null)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el teléfono"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    if (prospeccionTransUnionRequestDto.SalarioMensual <= 0)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza el salario en la pestaña de información financiera"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                    int tipoDocumentoConyugue = recursos.IdTipoIdentificacionConyuge;
                    string numeroDocumentoConyugue = recursos.IdentificacionConyuge;
                    ConsultarCreditoProspeccion datosCreditoSolicitud = null;

                    try
                    {
                        _logger.LogInformation("[{method}]ConsultarNumeroTipoCreditoProspeccion");

                        datosCreditoSolicitud =
                            await _creditoRepository.ConsultarNumeroTipoCreditoProspeccion(
                                tipoDocumentoConyugue,
                                numeroDocumentoConyugue);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("[{method}]ConsultarNumeroTipoCreditoProspeccion: "+ e);

                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "Metodo: ConsultarNumeroTipoCreditoProspeccion; respuesta: " + e.Message,
                        });

                        return new Response<IEnumerable<SimulacionDto>>
                        {
                            Data = planillaSimulacion
                        };
                    }


                    if (datosCreditoSolicitud!= null)
                    {
                        var requestPlanPago = new ObtenerProductoCartera
                        {
                            Nro_Credito = datosCreditoSolicitud.CRE_NRO_CREDITO,
                            Tcr_Id = datosCreditoSolicitud.TCR_ID
                        };
                        if (requestPlanPago != null)
                        {
                            Response<ResponsePlanPagoDto> dataPlanPago = null;

                            try
                            {
                                _logger.LogInformation("[{method}]ObtenerPlanPagosCredito");


                                dataPlanPago =
                                    await _carteraBusinessLogic.ObtenerPlanPagosCredito(requestPlanPago);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError("[{method}]ObtenerPlanPagosCredito: "+ e);

                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "Metodo: ObtenerPlanPagosCredito; respuesta: " + e.Message,
                                });

                                return new Response<IEnumerable<SimulacionDto>>
                                {
                                    Data = planillaSimulacion
                                };
                            }

                            var cuotaCanceladas = dataPlanPago.Data.PlanPago.Where(x => x.EstadoCuota != "PAGADA").ToList();

                            if (cuotaCanceladas.Count > 0)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "BLOQUEO POR CONYUGE Motivo: cuenta con un crédito activo"
                                });
                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                        }
                    }



                    _logger.LogInformation("Se llega al metodo que enviar los parametros a TU");



                    var ProspeccionTransUnion = new Response<ResponseTu>(); 
                    try
                    {
                        ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);

                        if (ProspeccionTransUnion == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Error en el metodo: ProspeccionCreditoTransUnion"
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        await _creditoRepository.RegistroLogTUSimulador("ProspecciónBL ProspeccionCreditoTransUnion", prospeccionTransUnionRequestDto.ToString(), ProspeccionTransUnion.ToString());

                    }
                    catch (Exception e)
                    {
                        await _creditoRepository.RegistroLogTUSimulador("ProspecciónBL ProspeccionCreditoTransUnion", prospeccionTransUnionRequestDto.ToString(), ProspeccionTransUnion.ToString());

                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "Error en el metodo: ProspeccionCreditoTransUnion" + e
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }



                   
                    #endregion
                    ReporteSimulacion60PorCientoDto reporteSimulacion60PorCientoDto = new ReporteSimulacion60PorCientoDto();
                    reporteSimulacion60PorCientoDto.NumeroDocumento = request.documento; 
                    double TOT = totalIngresos - totalDescuentos;
                    #region Reporte 60%
                    reporteSimulacion60PorCientoDto.salarioTotal = String.Format("{0:N0}", totalIngresos);
                    reporteSimulacion60PorCientoDto.deducciones = String.Format("{0:N0}", totalDescuentos);
                    reporteSimulacion60PorCientoDto.salarioNeto = String.Format("{0:N0}", TOT);
                    reporteSimulacion60PorCientoDto.capacidadPago = String.Format("{0:N0}", cuotaMax);
                    reporteSimulacion60PorCientoDto.canonCuotaMensual = String.Format("{0:N0}", Math.Round(cuotaLibranza, 0, MidpointRounding.AwayFromZero));
                    #endregion
                    if (!string.IsNullOrEmpty(ProspeccionTransUnion.Data.response))
                    {
                        _logger.LogInformation("Se valida respuesta de TU != null: true");

                        var ReporteTrans =new Response<SimulacionCreditoArchivo>();

                        try
                        {
                            
                            ReporteTrans = await _reportesBusinessLogic.ReporteTransUnionPropiedades(ProspeccionTransUnion.Data.response);
                            if (ProspeccionTransUnion == null)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                                    RespuestaRadicado = false

                                });
                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                            await _creditoRepository.RegistroLogTUSimulador("ProspecciónBL ReporteTransUnionPropiedades", ProspeccionTransUnion.ToString(), ReporteTrans.ToString());
                        }
                        catch (Exception e)
                        {
                            await _creditoRepository.RegistroLogTUSimulador("ProspecciónBL ReporteTransUnionPropiedades", prospeccionTransUnionRequestDto.ToString(), ProspeccionTransUnion.ToString());

                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }


                        await _creditoRepository.RegistroLogTUSimulador("FINALIZA LA PETICION TU EN EL SIMULADOR", request.documento, ProspeccionTransUnion.ToString());

                        if (ReporteTrans == null)
                        {

                            _logger.LogError("ReporteTransUnionPropiedades responde: " + ReporteTrans); 

                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                                RespuestaRadicado = false

                            });  
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        #region Reclasificacion
                        IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, request.documento);
                        var sumaSaldoObligacion = prospeccionResult
                        .Where(linea => linea.OLN_ESTADO_RECALCULAR == false)
                        .Sum(linea => linea.OLN_VALOR_CUOTA == 0 ? linea.OLN_SALDO_OBLIGACION : linea.OLN_VALOR_CUOTA);
                        _logger.LogInformation("ConsultarLineaCreditoObligacion"); 
                        #endregion
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.cuotaSectorFinanSoliAse = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorFinanciero);
                        reporteSimulacion60PorCientoDto.cuotaSectorReal = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorReal);
                        #endregion
                        var EndeudamientoG_Sin_Cuota = ReporteTrans.Data.cuotasSectorReal + ReporteTrans.Data.cuotasSectorFinanciero - (double)sumaSaldoObligacion;
                        var EndeudamientoG_Sin_Cuota_Porc = await CalcularEndeudamiento(EndeudamientoG_Sin_Cuota, totalIngresos) * 100;

                        var EndeudamientoActual_Cuota = (double)cuotaLibranza + EndeudamientoG_Sin_Cuota + totalDescuentos;
                        var EndeudamientoActual_Cuota_Porc = await CalcularEndeudamiento(EndeudamientoActual_Cuota, totalIngresos) * 100;

                        var ControNivelEndeudamiento_Cuota_Porc = EndeudamientoActual_Cuota_Porc > (double)(parametrosGenerales.PorcentajeMinimoEndeudamiento) ? "NO APLICA" : "SI APLICA"; 
                        var ControNivelEndeudamiento_Sin_Cuota_Porc = EndeudamientoG_Sin_Cuota_Porc > (double)(parametrosGenerales.PorcentajeMinimoEndeudamiento) ? "NO APLICA" : "SI APLICA";

                        CuotaSectorFinanciero = (decimal)ReporteTrans.Data.cuotasSectorFinanciero;
                        CuotaSectorReal = (decimal)ReporteTrans.Data.cuotasSectorReal;
                        Score = ReporteTrans.Data.riesgoMinimo;

                        EndeudamientoSCH = EndeudamientoG_Sin_Cuota_Porc;
                        EndeudamientoCCH = EndeudamientoActual_Cuota_Porc;
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.totalObliAntesCredito = String.Format("{0:N0}", EndeudamientoG_Sin_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalSinCanon = Math.Round(EndeudamientoG_Sin_Cuota_Porc, 0, MidpointRounding.AwayFromZero).ToString() + "%";
                        reporteSimulacion60PorCientoDto.totalObliNuevoCredito = String.Format("{0:N0}", Math.Round(EndeudamientoActual_Cuota, 0, MidpointRounding.AwayFromZero));
                        reporteSimulacion60PorCientoDto.EndeudaGlobalCanon = Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero).ToString() + "%";
                        reporteSimulacion60PorCientoDto.disponibleLibranza = String.Format("{0:N0}", Math.Round((cuotaMax - cuota), 0, MidpointRounding.AwayFromZero));
                        reporteSimulacion60PorCientoDto.controlCapacidadPago = Math.Round(cuotaMax, 0, MidpointRounding.AwayFromZero) >= Math.Round(cuotaLibranza, 0, MidpointRounding.AwayFromZero) ? "SI APLICA" : "NO APLICA";


                        DatosEndeudamientoProspeccionDto datosEndeudamientoProspeccion = new DatosEndeudamientoProspeccionDto();
                        datosEndeudamientoProspeccion.NumeroDocumento = datosPersonales.NumeroDocumento;
                        datosEndeudamientoProspeccion.SalarioUltimoMes = (decimal)totalIngresos;
                        datosEndeudamientoProspeccion.Descuentos = (decimal)totalDescuentos;
                        datosEndeudamientoProspeccion.SalarioNeto = (decimal)TOT;
                        datosEndeudamientoProspeccion.CapacidadPago = cuotaMax;
                        datosEndeudamientoProspeccion.CanonMensual = (decimal)(planillaSimulacion[1].TotalCuota);
                        datosEndeudamientoProspeccion.SectorFinanciero = CuotaSectorFinanciero - sumaSaldoObligacion;
                        datosEndeudamientoProspeccion.SectorReal = CuotaSectorReal;
                        datosEndeudamientoProspeccion.Score = Score;
                        datosEndeudamientoProspeccion.DisponibleLibranza = (Math.Round(cuotaMax, MidpointRounding.AwayFromZero) - cuotaLibranza);
                        datosEndeudamientoProspeccion.EndeudamientoGlobalCuota = (decimal)totalIngresos-(decimal)EndeudamientoActual_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalCuota = reporteSimulacion60PorCientoDto.controlCapacidadPago;
                        datosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota = (decimal)totalIngresos-(decimal)EndeudamientoG_Sin_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalSinCuota = ControNivelEndeudamiento_Sin_Cuota_Porc;
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalCuota = EndeudamientoActual_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalSinCuota = EndeudamientoG_Sin_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.TotalCanonDeducciones = (cuotaLibranza + (decimal)totalDescuentos + (decimal)totalObligaciones);
                        datosEndeudamientoProspeccion.DecisionHomologada = ReporteTrans.Data.descripcionDecision; 
                        await _creditoRepository.InsertarDatosEndeudamientoProspeccion(datosEndeudamientoProspeccion);

                        _logger.LogInformation("InsertarDatosEndeudamientoProspeccion Se insertaron todos los campos de enduedamiento ");


                        var valoresSimulacionProspeccion = new ValoresSimulacionCreditoReclasificacion
                        {
                            NUMERO_DOCUMENTO = request.documento,
                            TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                            TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                            SEGURO_VIDA = decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                            SEGURO_HOGAR = decimal.Parse(planillaSimulacion[0].SeguroHogar.ToString()),
                            SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                            SEGURO_VIDA_EXTRAPRIMA = 0,
                            TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                            TASA_MIXTA_EXTRAPRIMA = 0,
                            FACTOR = 0,
                            FACTOR_EXTRA_PRIMA = 0,
                            PLAZO = plazo,
                            VALOR_PRESTAMO = planillaSimulacion[0].ValorFinanciar,
                            CUOTA_SIN_SEGURO_HOGAR = cuota - decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                            CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            CUOTA_SEGURO_HOGAR = cuota,
                            CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                            VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                            CAPACIDAD_PAGO = cuotaMax,
                            VALOR_PRESTAMO_SEGUN_CUOTA = valorPrestamo
                        };

                        bool guardado = await _creditoRepository
                            .InsertValoresSimulacionCredito(valoresSimulacionProspeccion);


                        #region Validacion Score y Viabilidad
                        if (!ReporteTrans.IsSuccess)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no viable por consulta TransUnion.",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (ReporteTrans.Data.comprarCarteraSinViiviendaValidacion)
                        {
                            planillaSimulacion.Clear();

                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "El afiliado no posee una vivienda que pueda ser objetivo de compra de cartera",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }


                        if (ReporteTrans.Data.fechaPermanenciaVigente != null && ReporteTrans.Data.fechaPermanenciaVigente != "" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = ReporteTrans.Data.fechaPermanenciaVigente
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (ReporteTrans.Data.totalObligacionMora > 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "NO VIABLE: Motivo evaluación de políticas (por presentar deuda en mora)"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (reporteSimulacion60PorCientoDto.controlCapacidadPago == "NO APLICA"
                            && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if ((ReporteTrans.Data.descripcionDecision.Equals("Pendiente") || ReporteTrans.Data.descripcionDecision.Equals("PENDIENTE")) && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                        
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Para continuar con el proceso, por favor acérquese a un punto de atención",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        else
                        {
                            if (ReporteTrans.Data.riesgoMinimo <= parametrosGenerales.Score_Min_TU && !validacionesExcepcion.VCP_ESTADO)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "No se puede presentar una simulación, no cumple con el score minimo: " + parametrosGenerales.Score_Min_TU
                                });

                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                        }


                        


                        #endregion
                        #region GuardadoReporteTransUnion
                        try
                        {
                            _logger.LogInformation("Se iniciar el cargue del reporte comercial"); 
                            using (var memoryStream = new MemoryStream(ReporteTrans.Data.ArchivoBytes))
                            {
                                IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                await SaveReportTU(formFile, simulacionId);
                            }
                        }
                        catch (Exception e)
                        {

                            _logger.LogError("Error al cargar reporte comercial");

                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = e.Message
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }


                        #endregion

                        //if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA")
                        //{
                        //    planillaSimulacion.Clear();
                        //    planillaSimulacion.Add(new SimulacionDto
                        //    {
                        //        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                        //    });

                        //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        //}
                        #endregion
                        #region Validacion y Credito segun endeudamiento Global
                        if (Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero) > ObjetivoEndeudameinto)
                        {
                            //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto)
                            //{
                                try
                                {
                                    cuota = (decimal)await CalcularCuotaNueva(ObjetivoEndeudameinto, 0, EndeudamientoActual_Cuota, 0, totalIngresos) - (decimal)EndeudamientoG_Sin_Cuota - (decimal)totalDescuentos;
                                    EndeudamientoActual_Cuota = await CalcularEndeudamiento((double)cuota + EndeudamientoG_Sin_Cuota + totalDescuentos, totalIngresos);
                                    //valorPrestamo = (cuota - valorSeguroHogar) / factor;

                                    #region Reporte 60%
                                    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = String.Format("{0:N0}", Math.Round(cuota, 0, MidpointRounding.AwayFromZero));
                                    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = String.Format("{0:N0}", ((double)cuota + EndeudamientoG_Sin_Cuota));
                                    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = Math.Round((EndeudamientoActual_Cuota * 100), 0, MidpointRounding.AwayFromZero).ToString() + "%";
                                    //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc <= ObjetivoEndeudameinto)
                                    //{
                                    //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                    //}
                                    //else
                                    //{
                                    //    if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc > ObjetivoEndeudameinto)
                                    //    {
                                    //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NUEVO VALOR CUOTA";
                                    //    }
                                    //    else
                                    //    {
                                    //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    //    }
                                    //}
                                    #endregion                                                                        


                                    if (Math.Round(valorPrestamo, 0, MidpointRounding.ToZero) <= 0)
                                    {
                                        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                        planillaSimulacion.Clear();
                                        planillaSimulacion.Add(new SimulacionDto
                                        {
                                            Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 80%"
                                        });
                                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    }

                                    ConteoTU++;
                                    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = ConteoTU > 5 ? "NO APLICA" : "SI APLICA";
                                #region Save Reporte 60%

                                try
                                {
                                    _logger.LogInformation("Se iniciar el cargue del reporte 80%");

                                    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                    {
                                        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                        await SaveReportTU60(formFile, simulacionId);
                                    }
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError("Error al cargar reporte 80%");

                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                                        RespuestaRadicado = false

                                    });

                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }


                                var datos1 = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;


                                var toleranciaIngresosTotales = (double)(datos1.EDP_SALARIO_TOTAL) * 0.592;
                                var plazoValorMaximo = plazo;
                                var diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos1.EDP_SECTOR_FINANCIERO - (double)datos1.EDP_SECTOR_REAL - (double)datos1.EDP_DEDUCCIONES;
                                var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                                var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                                var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                                var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                                var valorMaximoCalculado = prestamosMaximo / 1000000;
                                var valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;


                                usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);


                                var userNamePrueba = usuarioEncontrado != null ? usuarioEncontrado.USR_USERNAME : ""; 


                                if (valorMaximo <= 13000000 && userNamePrueba != "pruebas.prospeccion@cajahonor.gov.co")
                                    {
                                        planillaSimulacion.Clear();
                                        planillaSimulacion.Add(new SimulacionDto
                                        {
                                            Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                                        });
                                    
                                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    }

                                    if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && userNamePrueba != "pruebas.prospeccion@cajahonor.gov.co")
                                    {
                                        planillaSimulacion.Clear();
                                        planillaSimulacion.Add(new SimulacionDto
                                        {
                                            Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo " + valorMaximo.ToString("C0", new System.Globalization.CultureInfo("es-CO")) + ", por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                                        });

                                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    }

                                    #endregion
                                    //if (ConteoTU > 5)
                                    //{
                                    //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    //    planillaSimulacion.Clear();
                                    //    planillaSimulacion.Add(new SimulacionDto
                                    //    {
                                    //        Sobrante = "No es posible generar una simulación viable para su solicitud"
                                    //    });
                                    //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    //}
                                    //else
                                    //{
                                    //    request.valorPrestamo = (double)valorPrestamo;
                                    //    return await CrearSimulacionCreditoViviendaTradicional(request, datosPersonales, simulacionId);
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                                        RespuestaRadicado = false

                                    });

                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }
                            //}
                            //else
                            //{
                            //    #region Reporte 60%
                            //    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            //    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            //    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //    #endregion
                            //    #region Save Reporte 60%
                            //    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //    {
                            //        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //        await SaveReportTU60(formFile, simulacionId);
                            //    }
                            //    #endregion
                            //    planillaSimulacion.Clear();

                            //    var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                            //    double toleranciaIngresosTotales = (double)(datos.EDP_SALARIO_TOTAL) * 0.598;
                            //    var plazoValorMaximo = plazo;
                            //    double diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos.EDP_SECTOR_FINANCIERO - (double)datos.EDP_SECTOR_REAL - (double)datos.EDP_DEDUCCIONES;
                            //    var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                            //    var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                            //    var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                            //    var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                            //    double valorMaximoCalculado = prestamosMaximo / 1000000;
                            //    double valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                            //    usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

                            //    if (valorMaximo<=13000000 && usuarioEncontrado.USR_USERNAME != "pruebas.prospeccion@cajahonor.gov.co")
                            //    {
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }


                            //    if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && usuarioEncontrado.USR_USERNAME != "pruebas.prospeccion@cajahonor.gov.co")
                            //    {                                    
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo  " + valorMaximo + " , por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }

                            //    //planillaSimulacion.Add(new SimulacionDto
                            //    //{
                            //    //    Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //    //});
                            //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //}
                        }
                        else
                        {
                           
                                #region Reporte 60%
                                reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                                reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                                reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                                reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                #endregion
                                #region Save Reporte 60%
                                try
                                {
                                    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                    {
                                        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                        await SaveReportTU60(formFile, simulacionId);
                                    }
                                }
                                catch (Exception e)
                                {

                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                                        RespuestaRadicado = false

                                    });

                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }

                                #endregion
                            
                        }
                        #endregion
                    }
                    else
                    {
                        _logger.LogWarning("Respuesta: ProspeccionTransUnion.Data.detalleError: " + ProspeccionTransUnion.Data.detalleError); 
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = ProspeccionTransUnion.Data.detalleError,
                            RespuestaRadicado = false



                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error "+ex); 
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Lo sentimos, no pudimos procesar tu simulación en este momento. Por favor, vuelve a intentarlo más tarde",
                        RespuestaRadicado = false

                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }
            #endregion

            //if (Math.Round(cuota,0,MidpointRounding.AwayFromZero) > cuotaMax)
            //{
            //    var toleranciaIngresosTotales = (double)(datosFinancieros.TotalIngresos) * 0.592;
            //    var plazoValorMaximo = plazo;
            //    double diferenciaToleranciaDeducciones = 0; 

            //    if (parametrosGenerales.ObligacionesExternasCapacidadPago)
            //    {
            //        diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)CuotaSectorFinanciero - (double)CuotaSectorReal - (double)datosFinancieros.TotalEgresos;
            //    }
            //    else
            //    {
            //        diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datosFinancieros.TotalEgresos;
            //    }

            //    var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
            //    var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
            //    var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
            //    var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
            //    var valorMaximoCalculado = prestamosMaximo / 1000000;
            //    var valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

            //    request.valor = valorMaximo;
            //    request.valorPrestamo = valorMaximo; 
            //    return await CrearSimulacionCreditoViviendaTradicional(request, datosPersonales, simulacionId);

            //}

            var algoMAs = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito;
            var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
            {
                Id = simulacionId,
                EnvioNotificacion = false,
                Estado = "I",
                FechaActualiza = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                NumeroPreAprobado = 0,
                ProblemaEmail = false,
                UsuarioActualiza = Guid.NewGuid(),
                TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                Score = Score,
                EndeudamientoCCH = (int)EndeudamientoCCH,
                EndeudamientoSCH = (int)EndeudamientoSCH
            });
            string tipoAbono = string.Empty;
            if (request.AbonoUnico)
                tipoAbono = "U";
            else if (request.TipoAbono == 1)
                tipoAbono = "N";
            else if (request.TipoAbono == 2)
                tipoAbono = "M";
            else
                tipoAbono = "A";

            string tipoAlivioV = string.Empty;
            if (tipoAlivio.Equals("TASA FRECH"))
                tipoAlivioV = "F";
            else
                tipoAlivioV = "X";

            planillaSimulacion[0].factor = factor;
            planillaSimulacion[0].CuotaSectorFinanciero = CuotaSectorFinanciero;
            planillaSimulacion[0].CuotaSectorReal = CuotaSectorReal;

            if (request.tipoSimularOProspectar == 3)
            {                
                var smc = new SMCSimulacionClienteDto()
                {
                    Documento = request.documento,
                    OpcionCompra = 0,
                    TotalCuotas = planillaSimulacion[0].PlazoMeses,
                    FechaSimulacion = DateTime.Now,
                    IdDatosPersonales = datosPersonales.Id,
                    IdSimulacion = simulacionId,
                    IdSimulacionCliente = datosPersonales.IdSimulacion,
                    NumeroPreaprobado = 0,
                    TasaEa = parametrosGenerales.Tasas.EfectivaAnual,
                    TasaFrech = alivioTasaMV,
                    TasaMv = (double)parametrosGenerales.Tasas.NominalMesVencido,
                    TipoAbono = tipoAbono,
                    TipoAlivio = tipoAlivioV,
                    TipoVivienda = request.tipoVivienda,
                    TomaSeguro = "N",
                    UsaRecursosAcumulados = "N",
                    ValorPrestamo = (decimal)valorPrestamo,
                    ValorPrestamosLeasing = 0,
                    ValorVivienda = request.valor,
                    ViviendaVis = viviendaVis,
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    alivioTasaporcentaje = alivioTasa,
                    TasaSeguroHogar = (double)parametrosGenerales.Tasas.SeguroHogar * 100,
                    TasaSeguroVida = (double)parametrosGenerales.Tasas.SeguroVida * 100,
                    ValorViviendaSeguro = request.ValorViviendaSeguro,
                    factor = planillaSimulacion[0].factor, 
                    Usr_id = request.Usr_id,
                    afl_id = request.afl_id, 
                    CLC_ID = 5
                    
                };
                var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(smc);
                foreach (SimulacionDto item in planillaSimulacion)
                {
                   
                        item.Capital = item.Capital;
                        item.Saldo = item.Saldo;
                        item.Intereses = item.Intereses;
                        item.SeguroVida = item.SeguroVida;
                        item.SeguroHogar = item.SeguroHogar;
                        item.TotalCuota = item.TotalCuota;
                        item.CuotaUnidadPagadora = (decimal)RedondearCifras((double)item.CuotaUnidadPagadora).Result;
                        item.ValorAbonoExtra = item.ValorAbonoExtra;
                        item.AbonoUnico = item.AbonoUnico;
                        item.AbonoExtra = item.AbonoExtra;
                        item.CuotaUnidadPagadorados = item.CuotaUnidadPagadorados;
                    
                }


                ValoresSimulacionAfiliadoDto valoresSimulacion = new ValoresSimulacionAfiliadoDto();
                valoresSimulacion.ValorFinanciar = planillaSimulacion[0].ValorFinanciar;
                valoresSimulacion.NumeroDocumento = request.documento;
                valoresSimulacion.SMS_ID = simulacionId;
                valoresSimulacion.RiesgoMinimo = planillaSimulacion[0].RiesgoMinimoTransUnion;
                valoresSimulacion.ValorSectorFinanciero = planillaSimulacion[0].CuotaSectorFinanciero;
                valoresSimulacion.ValorSectorReal = planillaSimulacion[0].CuotaSectorReal;
                valoresSimulacion.Factor = planillaSimulacion[0].factor;

                await _creditoRepository.InsertarValoresSimulacionAfiliado(valoresSimulacion);

                await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
                var consultaSMC = await _creditoBusinessLogic.ConsultarSimulacionCliente(request.documento);
                smc.TotalCuotas = planillaSimulacion[0].PlazoMeses;
            }

            
            planillaSimulacion[0].FechaDesembolso = fechaDesembolsoRespuesta; 

            _logger.LogInformation("Se guarda la simulación"); 
            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        #endregion

        #region Credito educativo
        private async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionCreditoEducativo(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid simulacionId)
        {
            List<SimulacionDto> planillaSimulacion = new();
            List<TasaHogarCiudadDto> TasaHogarCiudad = new();
            await _creditoBusinessLogic.LimpiarSimulacionCliente(simulacionId);
            request.Usr_id = request.Usr_id == null ? null : request.Usr_id;
            var idSimulacionCliente = Guid.NewGuid();
            var equivalenciasSimuladoCredito = (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;
            var cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto { TipoIdentificacion = request.tipoIdentificacion, NumeroDocumento = request.documento })).Data;
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento)).Data;

            var usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

            var correoLogueado = usuarioEncontrado == null ? "" : usuarioEncontrado.USR_USERNAME;


            #region ParametrosGeneralesXTipoAfiliacion
            ParametrosGenerales parametrosGenerales = new ParametrosGenerales();
            if (Int32.Parse(cliente.TipoAfiliacion) == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_ADMINISTRACION_CESANTÍAS || Int32.Parse(cliente.TipoAfiliacion) == (int)EnumAfiliadoID.AFILIADO_ADMINISTRACION_CESANTÍAS)
            {
                parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                {
                    SimuladorId = (int)EnumSimuladorId.EDUCATIVO_CESANTIAS,
                    TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                    UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                    DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                });
            }
            else if (Int32.Parse(cliente.TipoAfiliacion) == (int)EnumAfiliadoID.VETERANOS)
            {
                parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                {
                    SimuladorId = (int)EnumSimuladorId.EDUCATIVO_VETERANOS,
                    TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                    UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                    DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                });
            }
            else if (Int32.Parse(cliente.TipoAfiliacion) == (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_SOLUCION_VIVIENDA || Int32.Parse(cliente.TipoAfiliacion) == (int)EnumAfiliadoID.AFILIADO_SOLUCION_VIVIENDA)
            {
                parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
                {
                    SimuladorId = (int)EnumSimuladorId.EDUCATIVO_VIVIENDA,
                    TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                    UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                    DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
                });
            }
            #endregion
            if (parametrosGenerales.Status != string.Empty)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = parametrosGenerales.Status
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;
            var tiposVivienda = (await _catalogsBusinessLogic.ObtenerTiposDeVivienda()).Data;
            var ValorViviendaSeg = request.ValorViviendaSeguro > 0 ? request.ValorViviendaSeguro : request.valor;
            // decimal valorSeguroHogar = (decimal)RedondearCifras(((double)ValorViviendaSeg * (double)parametrosGenerales.Tasas.SeguroHogar)).Result;

            var idActor = recursos.IdTipoActor;
            var tipoVinculacionHipotecario = "";
            foreach (var tipoVinculacion in recursos.TiposVinculacion)
            {
                if (idActor == tipoVinculacion.IdTipoActor)
                {
                    tipoVinculacionHipotecario = tipoVinculacion.DescripcionTipoActor; break;
                }
            }

            await _creditoRepository.LimpiarRecursosCongelados(simulacionId, request.documento);

            decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = tipoVinculacionHipotecario
                }
            };

            foreach (var recurso in recursosCongelados)
            {
                await _creditoRepository.InsertarRecursosCongelados(recurso);
            }


            await _creditoRepository.LimpiarConceptosCongelados(simulacionId, request.documento);


            List<string> descripcionesConceptos = new List<string>
            {
                "AHORROS OBLIGATORIOS",
                "AHORROS RETROACTIVOS",
                "AHORROS VOLUNTARIOS",
                "INTERESES APORTES",
                "CESANTÍAS",
                "CESANTÍAS RETROACTIVOS",
                "INTERESES CESANTÍAS"
            };

            List<ConceptosCongeladosProspeccion> listaCongeladosProspeccion = new List<ConceptosCongeladosProspeccion>();

            foreach (var cuentas in recursos.Cuentas)
            {
                foreach (var conceptos in cuentas.Conceptos)
                {
                    if (descripcionesConceptos.Contains(conceptos.ConceptoDescripcion))
                    {
                        listaCongeladosProspeccion.Add(new ConceptosCongeladosProspeccion
                        {
                            CON_DESCRIPCION = conceptos.ConceptoDescripcion,
                            CON_VALOR = conceptos.saldo,
                            SLS_ID = simulacionId,
                            CON_NUMERO_DOCUMENTO = request.documento
                        });
                    }
                }
            }



            foreach (var concepto in listaCongeladosProspeccion)
            {
                await _creditoRepository.InsertarConceptosCongelados(concepto);
            }

            decimal valorSeguroHogar = 0; 
            var valorAvaluo = request.avaluoInmueble;
            if (request.simId != 3)
            {
                if (request.avaluoInmueble == 0)
                {
                    valorAvaluo = 0; 
                }
                valorSeguroHogar = (decimal)request.valor > valorAvaluo ? (decimal)RedondearCifras(((double)request.valor * 0)).Result : (decimal)RedondearCifras(((double)valorAvaluo * 0)).Result;

                var viviendaVis = "";

                if ((request.valor / parametrosGenerales.SalarioMinimo) <= parametrosGenerales.LimiteVIP)
                {
                    viviendaVis = "VIP";
                }
                else
                {
                    if ((request.valor / parametrosGenerales.SalarioMinimo) <= parametrosGenerales.LimiteVIS)
                    {
                        viviendaVis = "VIS";
                    }
                    else
                    {
                        viviendaVis = "NO VIS";
                    }
                }

                //verificar tipo de vivienda vis no vis
                var vivienda = 0;
                if (viviendaVis.Equals("VIP"))
                {
                    vivienda = 1;
                }
                else
                {
                    if (viviendaVis.Equals("VIS"))
                    {
                        vivienda = 2;
                    }
                    else
                    {
                        vivienda = 3;
                    }
                }
            }
            var proyeccionactual = (await _creditoBusinessLogic.ObtenerProyeccionCliente(cliente.GradoId.ToString())).Data;
            var subsidioCategoria = (await _catalogsBusinessLogic.ObtenerSimulacionCargoPorCategoria(cliente.CategoriaId, proyeccionactual.Id)).Data;

            var plazo = 0;
            double valorSubsidio = 0;
            decimal subsidioPresente = 0;
            var cuotasFaltanteasAlDesembolso = 0;
            //if (request.AbonoUnico)
            //{
            //    //Asigno plazo maximo seguntabla
            //    var factorIncremento = 0.04m;
            //    var cuotasFaltantes = 168 - datosPersonales.Cuotas;
            //    cuotasFaltanteasAlDesembolso = cuotasFaltantes - 3;
            //    var aniosParaSubsidio = (int)(cuotasFaltanteasAlDesembolso / 12);
            //    // pendiente el vaor del salario debe ser de los parametros. OJO CAMBIAR
            //    //valorSubsidio = (double)subsidioCategoria.FactorSubsidio * salariobasico * (Math.Pow((double)(1 + factorIncremento), aniosParaSubsidio));
            //    valorSubsidio = (double)subsidioCategoria.FactorSubsidio * parametrosGenerales.SalarioMinimo;

            //    var algo = Math.Pow(1 + (double)parametrosGenerales.Tasas.Mixta, cuotasFaltanteasAlDesembolso * -1);
            //    var otroalgo = valorSubsidio * algo;
            //    subsidioPresente = (decimal)(otroalgo);
            //    if (request.plazo == 0)
            //        plazo = parametrosGenerales.MaximoPlazoMeses <= cuotasFaltanteasAlDesembolso ? parametrosGenerales.MaximoPlazoMeses : cuotasFaltanteasAlDesembolso;
            //    else if (request.plazo <= parametrosGenerales.MaximoPlazoMeses)
            //        plazo = request.plazo;
            //}
            //else
            //{
                if (request.plazo == 0 || request.plazo == parametrosGenerales.MaximoPlazoMeses)
                    plazo = parametrosGenerales.MaximoPlazoMeses;
                else if (request.plazo < parametrosGenerales.MaximoPlazoMeses)
                    plazo = request.plazo;
            //}

            var tipoAlivio = string.Empty;
            var alivioTasa = 0.0;
            var alivioTasaMV = 0.0;
            var alivioCuota = 0.0M;
            //if (request.AplicaBeneficio)
            //{
            //    if (vivienda.Equals(1) || vivienda.Equals(2))
            //    {
            //        tipoAlivio = "TASA FRECH";
            //        if (vivienda.Equals(1))
            //        {
            //            alivioTasa = 0.05;
            //            alivioTasaMV = Math.Pow(1 + alivioTasa, 0.0833333333333333) - 1;
            //        }
            //        else
            //        {
            //            alivioTasa = 0.04;
            //            alivioTasaMV = Math.Pow(1 + alivioTasa, 0.0833333333333333) - 1;
            //        }
            //    }
            //    else if (request.valor / parametrosGenerales.SalarioMinimo <= 500)
            //    {
            //        tipoAlivio = "ALIVIO CUOTA";
            //        alivioCuota = 42M * parametrosGenerales.SalarioMinimo / 84M;
            //    }
            //}

            //Calculo el factor (tasaMixta*((1+Tasamixta)^plazo))/((1+tasaMixta)^plazo)-1
            var firstP = parametrosGenerales.Tasas.Mixta * (decimal)Math.Pow((1 + (double)parametrosGenerales.Tasas.Mixta), plazo);
            var secondP = (decimal)(Math.Pow(1 + (double)parametrosGenerales.Tasas.Mixta, plazo));
            var factor = firstP / (secondP - 1);



            var valorMinimoCredito = parametrosGenerales.ValorMinimoCuota / factor;

            //obtengo total ingresos y obtengo total egresos
            var totalIngresos = (double)datosFinancieros.TotalIngresos;
            var totalDescuentos = (double)datosFinancieros.TotalEgresos;

            //calculo cuota max totalIngresos*0.5-totalEgresos
            var cuotaMax = (decimal)((totalIngresos * 0.5) - totalDescuentos);
            var cuotaMaxdos = cuotaMax;
            if (request.valorCuota != 0 && request.valorCuota <= (double)cuotaMax)
                cuotaMax = (decimal)request.valorCuota;
            //si tiene abono extra mensual calculo el abono extra mensual
            var abonoExtra = 0M;
            var porAbonoExtra = 0.21;
            var interesesCuota = 0.0;
            if (request.TipoAbono == 2)
            {
                request.AbonoExtra = true;
                abonoExtra = (datosFinancieros.ValorSalario * 21) / 100;
                cuotaMaxdos = cuotaMax;
                cuotaMax = cuotaMax + abonoExtra;
            }
            decimal AbonoExtraCalculado = 0;
            decimal proyeccionAbonoExtra = 0;
            if (request.TipoAbono == 3)
            {
                request.AbonoExtra = true;
                var auxiliarFecha = DateTime.Now.AddMonths(3);
                for (var x = 0; x <= plazo; x++)
                {
                    AbonoExtraCalculado = 0;
                    if (auxiliarFecha.AddMonths(x).Month == 12)
                    {
                        if (x <= 12)
                            AbonoExtraCalculado = datosFinancieros.ValorSalario * (decimal)0.21 * x;
                        else
                            AbonoExtraCalculado = datosFinancieros.ValorSalario * (decimal)0.21 * 12;
                        proyeccionAbonoExtra += AbonoExtraCalculado * (decimal)(Math.Pow((1 + (double)parametrosGenerales.Tasas.Mixta), x * -1));
                    }

                }
            }
            //calculo valor credito segun cuota (cuotaMax-valorSeguro)/factor
            var creditoSegunCuota = Math.Round(((decimal)cuotaMax - valorSeguroHogar) / parametrosGenerales.Tasas.Mixta, 5);
            //calculo valor credito segun vivienda 
            var creditoSegunVivienda = (double)0;
            //if (request.AbonoUnico)
            //{
            //    creditoSegunCuota = (subsidioPresente + ((decimal)cuotaMax - valorSeguroHogar) / parametrosGenerales.Tasas.Mixta);
            //}
            //if (vivienda == 1 || vivienda == 2)
            //{
            //    creditoSegunVivienda = (double)(request.valor * 0.80);
            //}
            //else
            //{
            //    creditoSegunVivienda = (double)(request.valor * 0.70);
            //}
            //if (request.valorPrestamo > 0 && request.valorPrestamo < creditoSegunVivienda)
            //{
            //    creditoSegunVivienda = request.valorPrestamo;
            //}

            //if (request.valorPrestamo > 0 && (decimal)request.valorPrestamo < creditoSegunCuota)
            //{
            //    creditoSegunCuota = (decimal)request.valorPrestamo;
            //}

            var abonomes = (datosFinancieros.ValorSalario * (decimal)0.21);
            //var seguhog = (double)request.valor * (double)tasaSeguroHogar;
            //var tasacompuesta = ((decimal)tasaMesVencido + (decimal)seguroVidaTasa);

            /// SE DEBE QUITAR
            var fechaSolicitud = DateTime.Now;
            if (request.TipoAbono == 3)
                //creditoSegunCuota = (double)proyeccionAbonoExtra + ((double)cuotaMax - ((double)request.valor * (double)tasaSeguroHogar)) / factor;
                //creditoSegunCuota = (double)(datosFinancieros.ValorSalario * (decimal)0.21) + ((double)cuotaMax - ((double)request.valor * (double)tasaSeguroHogar)) / factor;
                //valorSectorFinan += ((parametrosGenerales.Tasas.Mixta 2) * parametrosGenerales.Tasas.Mixta) / (1 - Math.Pow(1 + parametrosGenerales.Tasas.Mixta, -numeroCuotasRestantes));



                creditoSegunCuota = (((decimal)cuotaMax + abonomes - valorSeguroHogar)) / parametrosGenerales.Tasas.Mixta;




            var creditoSegunIntereses = ((((decimal)cuotaMax - (decimal)valorSeguroHogar)) / parametrosGenerales.Tasas.Mixta);

            if (request.valorPrestamo > 0 && (decimal)request.valorPrestamo < creditoSegunIntereses)
            {
                creditoSegunIntereses = (decimal)request.valorPrestamo;
            }

            //calculo el credito segun los intereses (cuotaMax-valorSeguro)/tasaMixta)*0.9999
            //var creditoSegunIntereses = ((((decimal)cuotaMax - (decimal)valorSeguro) * 0.9999M) / ((decimal)tasaMixta + (decimal)seguroVidaTasa));
            //selecciono el valor menos de los creditos y se lo asigno a valor prestamo
            decimal valorPrestamo = 0;
            var listaValores = new double[]
            {
                creditoSegunVivienda,
                (double)creditoSegunCuota,
                (double)creditoSegunIntereses
            };
            valorPrestamo = (decimal)listaValores.Min<double>();
            //valorPrestamo = (decimal)creditoSegunVivienda; 

            if (request.valorPrestamo > 0 && (decimal)request.valorPrestamo < valorPrestamo)
                valorPrestamo = (int)request.valorPrestamo;

            valorPrestamo = Math.Max(valorPrestamo, valorMinimoCredito);

            //calculo la cuota del credito valorPrestamo*factor+valorSeguro
            //var cuota = request.TipoAbono == 2 || request.TipoAbono == 3 ? (decimal)RedondearCifras((double)(valorPrestamo * factor + valorSeguroHogar)).Result : (decimal)RedondearCifras((double)(valorPrestamo * factor + valorSeguroHogar)).Result;
            var cuota = (decimal)(valorPrestamo * factor) + valorSeguroHogar;


            var tasaMixtaCalculada = (decimal)((double)parametrosGenerales.Tasas.Mixta * (Math.Pow((double)(1 + parametrosGenerales.Tasas.Mixta), plazo))) / (decimal)((Math.Pow((double)(1 + parametrosGenerales.Tasas.Mixta), plazo)) - 1);

            if (request.valorPrestamo == 0 && cuotaMax < cuota)
            {
                valorPrestamo = (((decimal)cuotaMax - (decimal)valorSeguroHogar)) / tasaMixtaCalculada;
            }

            if (request.valorCuota != 0 && request.valorCuota <= (double)cuotaMax)
                cuota = (decimal)RedondearCifras(request.valorCuota).Result;
            if ((decimal)RedondearCifras((double)valorPrestamo).Result != (decimal)RedondearCifras((double)creditoSegunCuota).Result)
            {
                cuota = (decimal)RedondearCifras((double)((valorPrestamo * factor) + valorSeguroHogar)).Result;
            }
            var proyAbonoExtra = 0M;
            var cuotaSinSeguros = 0M;
            //if (cuota > cuotaMax)
            //    cuota = cuotaMax;
            if (plazo < 0)
                plazo = 0;
            if (!request.AplicaBeneficio)
            {
                for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        DateTime fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Saldo = (double)valorPrestamo,
                            Cuota = i,
                            ViviendaVis = "",
                            ValorVivienda = request.ValorViviendaSeguro,
                            ValorPrestamo = 999,
                            TotalCuota = (double)(cuota - abonoExtra),
                            TotalCuotaSinSeguros = (double)(cuota - valorSeguroHogar),
                            PlazoMeses = plazo,
                            PlazoAnos = plazo / 12,
                            ValorTasa = parametrosGenerales.Tasas.EfectivaAnual,
                            ValorTasaNominal = parametrosGenerales.Tasas.NominalMesVencido,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento

                        });

                    }
                    else
                    {

                        var VrCreditoSegHogar = 0.0;
                        var valorViviendaDeseado = request.valor;
                        var tasaSeguroVida = (double)(parametrosGenerales.Tasas.SeguroVida);


                        VrCreditoSegHogar = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

                        var seguroVidaCuota = Math.Round(planillaSimulacion[i - 1].Saldo * tasaSeguroVida, 3);


                        interesesCuota = !request.AbonoUnico ? RedondearCifras(planillaSimulacion[i - 1].Saldo * (double)parametrosGenerales.Tasas.NominalMesVencido).Result : RedondearCifras(planillaSimulacion[i - 1].Saldo * (double)parametrosGenerales.Tasas.NominalMesVencido).Result;
                        var capital = 0M;
                        if (i == plazo)
                        {
                            capital = (decimal)planillaSimulacion[i - 1].Saldo;
                            cuota = capital + (decimal)interesesCuota + (decimal)seguroVidaCuota + valorSeguroHogar;
                        }
                        else
                        {
                            capital = cuota - valorSeguroHogar - (decimal)seguroVidaCuota - (decimal)interesesCuota;
                        }

                        var fechaCorte = new DateTime(planillaSimulacion[i - 1].FechaCorte.Year, planillaSimulacion[i - 1].FechaCorte.Month, 1).AddMonths(2).AddDays(-1);
                        int mes = fechaCorte.Month;
                        int año = fechaCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        var abonoUnico = request.AbonoUnico ? i == cuotasFaltanteasAlDesembolso ? valorSubsidio : 0 : 0;
                        if (i == 1)
                            cuotaSinSeguros = cuota - valorSeguroHogar;
                        if (request.TipoAbono == 3)
                        {
                            abonoExtra = fechaSolicitud.AddMonths(i).Month == 12
                                        ? i < 12
                                            ? datosFinancieros.ValorSalario * (decimal)porAbonoExtra * i
                                            : datosFinancieros.ValorSalario * (decimal)porAbonoExtra * 12
                                        : 0;
                            capital = (cuota + abonoExtra) - ((decimal)interesesCuota + valorSeguroHogar + (decimal)seguroVidaCuota);
                            proyAbonoExtra += abonoExtra * (decimal)Math.Pow(1 + (double)parametrosGenerales.Tasas.Mixta, i * -1);
                        }

                        var valorSaldo = 0.0;
                        if (abonoUnico != 0)
                        {
                            if ((double)(planillaSimulacion[i - 1].Saldo - (double)capital) < 1)
                            {
                                valorSaldo = 0.0;
                            }
                            else
                            {
                                valorSaldo = (double)(planillaSimulacion[i - 1].Saldo - abonoUnico - (double)capital);
                            }
                        }
                        else
                        {
                            if (abonoExtra != 0)
                            {
                                valorSaldo = (double)(planillaSimulacion[i - 1].Saldo - (double)capital);
                            }
                            else
                            {
                                valorSaldo = (double)(planillaSimulacion[i - 1].Saldo - (double)capital);
                            }

                        }

                        decimal temporal1 = 0.0M; //cuota unidad pagadora
                        decimal temporal2 = 0.0M; //cuota abono extra

                        if (request.TipoAbono == 3)
                        {
                            temporal1 = cuota;
                            temporal2 = abonoExtra;
                        }
                        else
                        {
                            if (cuota > cuotaMax)
                            {
                                temporal1 = cuota;
                                temporal2 = cuota - cuotaMax;
                            }
                            else
                            {
                                temporal1 = cuota;
                                temporal2 = 0;
                            }
                        }
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Cuota = i,
                            FechaCorte = fechaCorte,
                            FechaPago = fechaPrimerCortedos,
                            Capital = (double)capital,
                            Saldo = i == plazo ? Math.Abs(valorSaldo) : valorSaldo,
                            Intereses = interesesCuota,
                            SeguroVida = seguroVidaCuota,
                            SeguroHogar = (double)valorSeguroHogar,
                            TotalCuota = (double)(cuota - abonoExtra),
                            CuotaUnidadPagadora = (decimal)temporal1,
                            ValorAbonoExtra = abonoExtra,
                            AbonoUnico = (decimal)abonoUnico,
                            AbonoExtra = (double)temporal2,
                            CuotaUnidadPagadorados = (decimal)cuotaMaxdos,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento
                        });
                        if (planillaSimulacion[i].Saldo <= 0)
                        {
                            plazo = i;
                            planillaSimulacion[0].PlazoMeses = i;
                            planillaSimulacion[0].PlazoAnos = (i) / 12;
                            break;
                        }
                    }
                };
            }
            else
            {
                for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        var fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Saldo = (double)valorPrestamo,
                            Cuota = i,
                            ViviendaVis = "",
                            ValorVivienda = request.valor,
                            ValorPrestamo = (decimal)valorPrestamo,
                            TotalCuota = (double)cuota,
                            TotalCuotaSinSeguros = (double)(cuota - valorSeguroHogar),
                            PlazoMeses = plazo,
                            PlazoAnos = plazo / 12,
                            ValorTasa = parametrosGenerales.Tasas.EfectivaAnual,
                            ValorTasaNominal = (decimal)interesesCuota / valorPrestamo,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            ValorAbonoExtra = abonoExtra,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento
                        });
                    }
                    else
                    {
                        decimal reduccionInteres = (tipoAlivio.Equals("TASA FRECH")) && i <= 84 ? (decimal)(planillaSimulacion[i - 1].Saldo * alivioTasaMV) : 0m;
                        var reduccionCuota = 0.0m;
                        if (tipoAlivio.Equals("ALIVIO CUOTA") && i <= 84 && planillaSimulacion[i - 1].Saldo > 0)
                        {
                            reduccionCuota = (decimal)alivioCuota;
                        }

                        var VrCreditoSegHogar = 0.0;
                        var valorViviendaDeseado = request.valor;
                        var tasaSeguroVida = (double)(parametrosGenerales.Tasas.SeguroVida);


                        VrCreditoSegHogar = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

                        var seguroVidaCuota = VrCreditoSegHogar * tasaSeguroVida;


                        interesesCuota = planillaSimulacion[i - 1].Saldo * (double)parametrosGenerales.Tasas.NominalMesVencido - (double)reduccionInteres;
                        //var seguroHogar = planillaSimulacion[i - 1].Saldo > 0 ? (double)parametrosGenerales.Tasas.SeguroHogar * request.valor : 0;

                        decimal seguroHogar = (decimal)request.valor > request.avaluoInmueble ? (decimal)RedondearCifras(((double)request.valor * (double)parametrosGenerales.Tasas.SeguroHogar)).Result : (decimal)RedondearCifras(((double)request.avaluoInmueble * (double)parametrosGenerales.Tasas.SeguroHogar)).Result;

                        var cobroFonvivienda = reduccionInteres <= reduccionCuota ? alivioCuota : reduccionInteres;
                        var capital = i <= 84 ? ((double)cuota + (double)cobroFonvivienda - (double)seguroHogar - seguroVidaCuota - interesesCuota) : ((double)cuota - (double)seguroHogar - seguroVidaCuota - interesesCuota);
                        var fechaCorte = planillaSimulacion[i - 1].FechaCorte.AddMonths(1);
                        int mes = fechaCorte.Month;
                        int año = fechaCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        var abonoUnico = request.AbonoUnico ? i == cuotasFaltanteasAlDesembolso ? valorSubsidio : 0 : 0;
                        if (i == 1)
                            cuotaSinSeguros = (decimal)((double)cuota - (double)seguroHogar);
                        var saldo = planillaSimulacion[i - 1].Saldo > 0 ? (double)(planillaSimulacion[i - 1].Saldo - capital) : 0;


                        SolicitudCredito solicitudGeneralCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, request.documento);
                        double seguroVidaProrrata = 0;
                        double seguroHogarProrrata = 0;
                        if (i == 1)
                        {
                            if (solicitudGeneralCredito != null)
                            {
                                var fecha = request.FechaDesembolso;

                                if (fecha < new DateTime(1753, 1, 1))
                                {
                                    fecha = DateTime.Now; // o null
                                }

                                await _creditoRepository.ActualizarFechaDesembolsoCredito(
                                    solicitudGeneralCredito.SOC_ID,
                                    fecha);

                                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(solicitudGeneralCredito.SOC_ID);

                                if (datosFactura != null && datosFactura.FAC_NUMERO_FACTURA != null)
                                {
                                    SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(solicitudGeneralCredito.SOC_ID);

                                    DateTime? fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;

                                    DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));

                                    var diasProrrata = (ultimoDia - informacionDesembolso.SID_FECHA_DESEMBOLSO).Days;
                                    seguroVidaProrrata = Math.Round((double)seguroVidaCuota * (diasProrrata + 1) / 30, 0);
                                    seguroHogarProrrata = Math.Round((double)valorSeguroHogar / 30, 0) * (diasProrrata + 0);
                                }
                            }
                        }
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            IdSimulacion = simulacionId,
                            IdDetalleSimulacion = Guid.NewGuid(),
                            IdSimulacionCliente = datosPersonales.IdSimulacion,
                            Cuota = i,
                            FechaCorte = fechaCorte,
                            FechaPago = fechaPrimerCortedos,
                            Capital = (double)capital,
                            Saldo = saldo,
                            Intereses = interesesCuota,
                            SeguroVida = seguroVidaCuota + seguroVidaProrrata,
                            SeguroHogar = (double)seguroHogar + seguroHogarProrrata,
                            TotalCuota = (double)cuota,
                            CuotaUnidadPagadora = cuota,
                            ValorAbonoExtra = abonoExtra,
                            AbonoUnico = (decimal)abonoUnico,
                            InteresesReduccion = (double)reduccionInteres,
                            CobroFonvivienda = i <= 84 ? (double)cobroFonvivienda : 0,
                            CuotaReduccion = (double)reduccionCuota,
                            TotalIntereses = (decimal)interesesCuota + reduccionInteres,
                            Afl_id = request.afl_id,
                            NumeroDocumento = request.documento
                        });
                        if (planillaSimulacion[i].Saldo <= 0)
                        {
                            plazo = i;
                            planillaSimulacion[0].PlazoMeses = i;
                            planillaSimulacion[0].PlazoAnos = (i) / 12;
                            break;
                        }
                    }
                };
            }


            var vinculacion = cliente.TipoAfiliacion;
            

            #region EndeudamientoGlobal
            if (parametrosGenerales.ActiveTU && request.tipoSimularOProspectar == 3)
            {
                try
                {
                    #region ConsumoTU
                    ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                    {
                        Apellidos = recursos.PrimerApellido,
                        FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                        Identificacion = request.documento,
                        SalarioMensual = Math.Round((decimal)totalIngresos),
                        TipoIdentificacion = request.tipoIdentificacion,
                        Telefono = recursos.Telefono,
                        TraerHistorialCrediticio = "Y",
                        SLS_ID = simulacionId
                    };

                    if (prospeccionTransUnionRequestDto.Telefono.Length < 10 || prospeccionTransUnionRequestDto.Telefono == null)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el teléfono"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    if (prospeccionTransUnionRequestDto.SalarioMensual <= 0)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el salario"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }


                    var ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                    #endregion
                    ReporteSimulacion60PorCientoDto reporteSimulacion60PorCientoDto = new ReporteSimulacion60PorCientoDto();
                    reporteSimulacion60PorCientoDto.NumeroDocumento = request.documento;
                    double TOT = totalIngresos - totalDescuentos;
                    #region Reporte 60%
                    reporteSimulacion60PorCientoDto.salarioTotal = String.Format("{0:N0}", totalIngresos);
                    reporteSimulacion60PorCientoDto.deducciones = String.Format("{0:N0}", totalDescuentos);
                    reporteSimulacion60PorCientoDto.salarioNeto = String.Format("{0:N0}", TOT);
                    reporteSimulacion60PorCientoDto.capacidadPago = String.Format("{0:N0}", cuotaMax);
                    reporteSimulacion60PorCientoDto.canonCuotaMensual = String.Format("{0:N0}", Math.Round(cuota, 0, MidpointRounding.AwayFromZero));
                    #endregion
                    if (!string.IsNullOrEmpty(ProspeccionTransUnion.Data.response))
                    {
                        var ReporteTrans = await _reportesBusinessLogic.ReporteTransUnionPropiedades(ProspeccionTransUnion.Data.response);

                        #region Reclasificacion
                        IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, request.documento);
                        var sumaSaldoObligacion = prospeccionResult
                        .Where(linea => linea.OLN_ESTADO_RECALCULAR == false)
                        .Sum(linea => linea.OLN_VALOR_CUOTA == 0 ? linea.OLN_SALDO_OBLIGACION : linea.OLN_VALOR_CUOTA);

                        #endregion
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.cuotaSectorFinanSoliAse = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorFinanciero);
                        reporteSimulacion60PorCientoDto.cuotaSectorReal = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorReal);
                        #endregion
                        var EndeudamientoG_Sin_Cuota = ReporteTrans.Data.cuotasSectorReal + ReporteTrans.Data.cuotasSectorFinanciero - (double)sumaSaldoObligacion;
                        var EndeudamientoG_Sin_Cuota_Porc = await CalcularEndeudamiento(EndeudamientoG_Sin_Cuota, totalIngresos) * 100;

                        var EndeudamientoActual_Cuota = (double)cuota + EndeudamientoG_Sin_Cuota + totalDescuentos;
                        var EndeudamientoActual_Cuota_Porc = await CalcularEndeudamiento(EndeudamientoActual_Cuota, totalIngresos) * 100;

                        var ControNivelEndeudamiento_Cuota_Porc = EndeudamientoActual_Cuota_Porc > 60 ? "NO APLICA" : "SI APLICA";
                        var ControNivelEndeudamiento_Sin_Cuota_Porc = EndeudamientoG_Sin_Cuota_Porc > 60 ? "NO APLICA" : "SI APLICA";

                        CuotaSectorFinanciero = (decimal)ReporteTrans.Data.cuotasSectorFinanciero;
                        CuotaSectorReal = (decimal)ReporteTrans.Data.cuotasSectorReal;
                        Score = ReporteTrans.Data.riesgoMinimo;

                        EndeudamientoSCH = EndeudamientoG_Sin_Cuota_Porc;
                        EndeudamientoCCH = EndeudamientoActual_Cuota_Porc;
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.totalObliAntesCredito = String.Format("{0:N0}", EndeudamientoG_Sin_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalSinCanon = Math.Round(EndeudamientoG_Sin_Cuota_Porc, 0, MidpointRounding.AwayFromZero).ToString() + "%";
                        reporteSimulacion60PorCientoDto.totalObliNuevoCredito = String.Format("{0:N0}", Math.Round(EndeudamientoActual_Cuota, 0, MidpointRounding.AwayFromZero));
                        reporteSimulacion60PorCientoDto.EndeudaGlobalCanon = Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero).ToString() + "%";
                        reporteSimulacion60PorCientoDto.disponibleLibranza = String.Format("{0:N0}", Math.Round((cuotaMax - cuota), 0, MidpointRounding.AwayFromZero));
                        reporteSimulacion60PorCientoDto.controlCapacidadPago = cuotaMax >= cuota ? "SI APLICA" : "NO APLICA";

                        

                        DatosEndeudamientoProspeccionDto datosEndeudamientoProspeccion = new DatosEndeudamientoProspeccionDto();
                        datosEndeudamientoProspeccion.NumeroDocumento = datosPersonales.NumeroDocumento;
                        datosEndeudamientoProspeccion.SalarioUltimoMes = (decimal)totalIngresos;
                        datosEndeudamientoProspeccion.Descuentos = (decimal)totalDescuentos;
                        datosEndeudamientoProspeccion.SalarioNeto = (decimal)TOT;
                        datosEndeudamientoProspeccion.CapacidadPago = cuotaMax;
                        datosEndeudamientoProspeccion.CanonMensual = (decimal)(planillaSimulacion[1].TotalCuota);
                        datosEndeudamientoProspeccion.SectorFinanciero = CuotaSectorFinanciero;
                        datosEndeudamientoProspeccion.SectorReal = CuotaSectorReal;
                        datosEndeudamientoProspeccion.Score = Score;
                        datosEndeudamientoProspeccion.DisponibleLibranza = (cuotaMax - cuota);
                        datosEndeudamientoProspeccion.EndeudamientoGlobalCuota = (decimal)totalIngresos - (decimal)EndeudamientoActual_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalCuota = reporteSimulacion60PorCientoDto.controlCapacidadPago;
                        datosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota = (decimal)totalIngresos - (decimal)EndeudamientoG_Sin_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalSinCuota = ControNivelEndeudamiento_Sin_Cuota_Porc;
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalCuota = EndeudamientoActual_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalSinCuota = EndeudamientoG_Sin_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.TotalCanonDeducciones = (cuota + (decimal)totalDescuentos);
                        datosEndeudamientoProspeccion.DecisionHomologada = ReporteTrans.Data.descripcionDecision;
                        await _creditoRepository.InsertarDatosEndeudamientoProspeccion(datosEndeudamientoProspeccion);





                        #region Validacion Score y Viabilidad
                        if (!ReporteTrans.IsSuccess)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no viable por consulta TransUnion."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if ((ReporteTrans.Data.descripcionDecision.Equals("PENDIENTE") && ControNivelEndeudamiento_Cuota_Porc == "NO APLICA") && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();

                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Para continuar con el proceso, por favor acérquese a un punto de atención",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        else
                        {
                            if (ReporteTrans.Data.riesgoMinimo <= parametrosGenerales.Score_Min_TU)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "No se puede presentar una simulación, no cumple con el score minimo: " + parametrosGenerales.Score_Min_TU
                                });

                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                        }


                        if (ReporteTrans.Data.fechaPermanenciaVigente != null && ReporteTrans.Data.fechaPermanenciaVigente != "" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = ReporteTrans.Data.fechaPermanenciaVigente
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (ReporteTrans.Data.totalObligacionMora > 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "NO VIABLE: Motivo evaluación de políticas (por presentar deuda en mora)"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (reporteSimulacion60PorCientoDto.controlCapacidadPago == "NO APLICA"
                            && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }





                        #endregion
                        #region GuardadoReporteTransUnion
                        using (var memoryStream = new MemoryStream(ReporteTrans.Data.ArchivoBytes))
                        {
                            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            await SaveReportTU(formFile, simulacionId);
                        }
                        #endregion

                        //if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA")
                        //{
                        //    planillaSimulacion.Clear();
                        //    planillaSimulacion.Add(new SimulacionDto
                        //    {
                        //        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                        //    });

                        //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        //}
                        #endregion
                        #region Validacion y Credito segun endeudamiento Global
                        if (Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero) > ObjetivoEndeudameinto)
                        {
                            //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto)
                            //{
                            try
                            {
                                cuota = (decimal)await CalcularCuotaNueva(ObjetivoEndeudameinto, 0, EndeudamientoActual_Cuota, 0, totalIngresos) - (decimal)EndeudamientoG_Sin_Cuota - (decimal)totalDescuentos;
                                EndeudamientoActual_Cuota = await CalcularEndeudamiento((double)cuota + EndeudamientoG_Sin_Cuota + totalDescuentos, totalIngresos);
                                //valorPrestamo = (cuota - valorSeguroHogar) / factor;

                                #region Reporte 60%
                                reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = String.Format("{0:N0}", Math.Round(cuota, 0, MidpointRounding.AwayFromZero));
                                reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = String.Format("{0:N0}", ((double)cuota + EndeudamientoG_Sin_Cuota));
                                reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = Math.Round((EndeudamientoActual_Cuota * 100), 0, MidpointRounding.AwayFromZero).ToString() + "%";
                                //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc <= ObjetivoEndeudameinto)
                                //{
                                //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                //}
                                //else
                                //{
                                //    if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc > ObjetivoEndeudameinto)
                                //    {
                                //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NUEVO VALOR CUOTA";
                                //    }
                                //    else
                                //    {
                                //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                //    }
                                //}
                                #endregion


                                if (Math.Round(valorPrestamo, 0, MidpointRounding.ToZero) <= 0)
                                {
                                    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 80%"
                                    });
                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }

                                ConteoTU++;
                                reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = ConteoTU > 5 ? "NO APLICA" : "SI APLICA";
                                #region Save Reporte 60%
                                var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                {
                                    IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                    await SaveReportTU60(formFile, simulacionId);
                                }

                                var datos1 = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                                var toleranciaIngresosTotales = (double)(datos1.EDP_SALARIO_TOTAL) * 0.592;
                                var plazoValorMaximo = plazo;
                                var diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos1.EDP_SECTOR_FINANCIERO - (double)datos1.EDP_SECTOR_REAL - (double)datos1.EDP_DEDUCCIONES;
                                var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                                var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                                var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                                var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                                var valorMaximoCalculado = prestamosMaximo / 1000000;
                                var valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                                usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

                                var userNamePrueba = usuarioEncontrado != null ? usuarioEncontrado.USR_USERNAME : "";


                                if (valorMaximo <= 13000000 && userNamePrueba != "pruebas.prospeccion@cajahonor.gov.co")
                                {
                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                                    });

                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }

                                if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && userNamePrueba != "pruebas.prospeccion@cajahonor.gov.co")
                                {
                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo " + valorMaximo.ToString("C0", new System.Globalization.CultureInfo("es-CO")) + ", por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                                    });

                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }

                                #endregion
                                //if (ConteoTU > 5)
                                //{
                                //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                //    planillaSimulacion.Clear();
                                //    planillaSimulacion.Add(new SimulacionDto
                                //    {
                                //        Sobrante = "No es posible generar una simulación viable para su solicitud"
                                //    });
                                //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                //}
                                //else
                                //{
                                //    request.valorPrestamo = (double)valorPrestamo;
                                //    return await CrearSimulacionCreditoViviendaTradicional(request, datosPersonales, simulacionId);
                                //}
                            }
                            catch (Exception ex)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = ex.Message
                                });

                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                            //}
                            //else
                            //{
                            //    #region Reporte 60%
                            //    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            //    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            //    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //    #endregion
                            //    #region Save Reporte 60%
                            //    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //    {
                            //        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //        await SaveReportTU60(formFile, simulacionId);
                            //    }
                            //    #endregion
                            //    planillaSimulacion.Clear();

                            //    var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                            //    double toleranciaIngresosTotales = (double)(datos.EDP_SALARIO_TOTAL) * 0.598;
                            //    var plazoValorMaximo = plazo;
                            //    double diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos.EDP_SECTOR_FINANCIERO - (double)datos.EDP_SECTOR_REAL - (double)datos.EDP_DEDUCCIONES;
                            //    var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                            //    var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                            //    var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                            //    var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                            //    double valorMaximoCalculado = prestamosMaximo / 1000000;
                            //    double valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                            //    usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

                            //    if (valorMaximo<=13000000 && usuarioEncontrado.USR_USERNAME != "pruebas.prospeccion@cajahonor.gov.co")
                            //    {
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }


                            //    if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && usuarioEncontrado.USR_USERNAME != "pruebas.prospeccion@cajahonor.gov.co")
                            //    {                                    
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo  " + valorMaximo + " , por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }

                            //    //planillaSimulacion.Add(new SimulacionDto
                            //    //{
                            //    //    Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //    //});
                            //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //}
                        }
                        //else
                        //{
                        //    if (ConteoTU == 0)
                        //    {
                        //        #region Reporte 60%
                        //        reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                        //        reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                        //        reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                        //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                        //        #endregion
                        //        #region Save Reporte 60%
                        //        var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                        //        using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                        //        {
                        //            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                        //            await SaveReportTU60(formFile, simulacionId);
                        //        }
                        //        #endregion
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = ProspeccionTransUnion.Data.detalleError
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                }
                catch (Exception ex)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = ex.Message
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }
            #endregion

            var algoMAs = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito;
            var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
            {
                Id = simulacionId,
                EnvioNotificacion = false,
                Estado = "I",
                FechaActualiza = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                NumeroPreAprobado = 0,
                ProblemaEmail = false,
                UsuarioActualiza = Guid.NewGuid(),
                TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                Score = Score,
                EndeudamientoCCH = (int)EndeudamientoCCH,
                EndeudamientoSCH = (int)EndeudamientoSCH
            });
            string tipoAbono = string.Empty;
            if (request.AbonoUnico)
                tipoAbono = "U";
            else if (request.TipoAbono == 1)
                tipoAbono = "N";
            else if (request.TipoAbono == 2)
                tipoAbono = "M";
            else
                tipoAbono = "A";

            string tipoAlivioV = string.Empty;
            if (tipoAlivio.Equals("TASA FRECH"))
                tipoAlivioV = "F";
            else
                tipoAlivioV = "X";

            planillaSimulacion[0].factor = factor;
            planillaSimulacion[0].CuotaSectorFinanciero = CuotaSectorFinanciero;
            planillaSimulacion[0].CuotaSectorReal = CuotaSectorReal;

            if (request.tipoSimularOProspectar == 3)
            {
                var smc = new SMCSimulacionClienteDto()
                {
                    Documento = request.documento,
                    OpcionCompra = 0,
                    TotalCuotas = planillaSimulacion[0].PlazoMeses,
                    FechaSimulacion = DateTime.Now,
                    IdDatosPersonales = datosPersonales.Id,
                    IdSimulacion = simulacionId,
                    IdSimulacionCliente = datosPersonales.IdSimulacion,
                    NumeroPreaprobado = 0,
                    TasaEa = parametrosGenerales.Tasas.EfectivaAnual,
                    TasaFrech = alivioTasaMV,
                    TasaMv = (double)parametrosGenerales.Tasas.NominalMesVencido,
                    TipoAbono = tipoAbono,
                    TipoAlivio = tipoAlivioV,
                    TipoVivienda = "N",
                    TomaSeguro = "N",
                    UsaRecursosAcumulados = "N",
                    ValorPrestamo = (decimal)valorPrestamo,
                    ValorPrestamosLeasing = 0,
                    ValorVivienda = request.valor,
                    ViviendaVis = "",
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    alivioTasaporcentaje = alivioTasa,
                    TasaSeguroHogar = (double)parametrosGenerales.Tasas.SeguroHogar * 100,
                    TasaSeguroVida = (double)parametrosGenerales.Tasas.SeguroVida * 100,
                    ValorViviendaSeguro = request.ValorViviendaSeguro,
                    factor = planillaSimulacion[0].factor,
                    Usr_id = request.Usr_id,
                    afl_id = request.afl_id,
                    CLC_ID = 6

                };
                var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(smc);
                foreach (SimulacionDto item in planillaSimulacion)
                {
                    item.Capital = RedondearCifras(item.Capital).Result;
                    item.Saldo = RedondearCifras(item.Saldo).Result;
                    item.Intereses = RedondearCifras(item.Intereses).Result;
                    item.SeguroVida = RedondearCifras(item.SeguroVida).Result;
                    item.SeguroHogar = RedondearCifras(item.SeguroHogar).Result;
                    item.TotalCuota = RedondearCifras(item.TotalCuota).Result;
                    item.CuotaUnidadPagadora = (decimal)RedondearCifras((double)item.CuotaUnidadPagadora).Result;
                    item.ValorAbonoExtra = (decimal)RedondearCifras((double)item.ValorAbonoExtra).Result;
                    item.AbonoUnico = (decimal)RedondearCifras((double)item.AbonoUnico).Result;
                    item.AbonoExtra = RedondearCifras(item.AbonoExtra).Result;
                    item.CuotaUnidadPagadorados = (decimal)RedondearCifras((double)item.CuotaUnidadPagadorados).Result;
                }


                ValoresSimulacionAfiliadoDto valoresSimulacion = new ValoresSimulacionAfiliadoDto();
                valoresSimulacion.ValorFinanciar = planillaSimulacion[0].ValorFinanciar;
                valoresSimulacion.NumeroDocumento = request.documento;
                valoresSimulacion.SMS_ID = simulacionId;
                valoresSimulacion.RiesgoMinimo = planillaSimulacion[0].RiesgoMinimoTransUnion;
                valoresSimulacion.ValorSectorFinanciero = planillaSimulacion[0].CuotaSectorFinanciero;
                valoresSimulacion.ValorSectorReal = planillaSimulacion[0].CuotaSectorReal;
                valoresSimulacion.Factor = planillaSimulacion[0].factor;

                await _creditoRepository.InsertarValoresSimulacionAfiliado(valoresSimulacion);

                await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
                var consultaSMC = await _creditoBusinessLogic.ConsultarSimulacionCliente(request.documento);
                smc.TotalCuotas = planillaSimulacion[0].PlazoMeses;
            }


            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        #endregion

        #region Credito Leasing 108-168
        public class Leasing108
        {
            public int NroCuota { get; set; }
            public DateTime FechaCorte { get; set; }
            public DateTime FechaPago { get; set; }
            public decimal Capital { get; set; }
            public decimal IntCte { get; set; }
            public decimal SegVida { get; set; }
            public decimal SegHogar { get; set; }
            public decimal Cuota { get; set; }
            public decimal Saldo { get; set; }
            public decimal SaldoCapitalReducido { get; set; }
            public decimal CapitalAPagar { get; set; }
            public decimal CapitalPorPagar { get; set; }
            public decimal InteresesCorrientesAPagar { get; set; }
            public decimal InteresesCorrientesPorPagar { get; set; }
            public decimal SeguroHogar { get; set; }
            public decimal SeguvoVida { get; set; }
            public decimal SeguroPorPagar { get; set; }
            public decimal CuotaUnidadPagadora { get; set; }
            public decimal AfectacionCuentaIndividual { get; set; }
            public decimal DeudaRemanente { get; set; }
            public decimal BeneficioTasaFresh { get; set; }
            public decimal BeneficioAlivioCuota { get; set; }
            public decimal ValoraCobrarFonvivienda { get; set; }    
            public decimal SaldoLeasingTradicional { get; set; }
            public decimal SegurosTotal { get; set; }
            public decimal ValorFinanciar { get; set; }

        }
        public async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionLeasing108(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid simulacionId, bool recalcular=false)
        {
            #region DatosSimulacion
            ValoresProyectadosDto valoresProyectadosDto = new ValoresProyectadosDto();
            List<Leasing108> PlanSimulacion = new();
            List<SimulacionDto> planillaSimulacion = new();
            SimulacionDto simulacionDto = new SimulacionDto();
            var equivalenciasSimuladoCredito = (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento)).Data;
            var cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto { TipoIdentificacion = request.tipoIdentificacion, NumeroDocumento = request.documento })).Data;

            var usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

            var correoLogueado = usuarioEncontrado == null ? "" : usuarioEncontrado.USR_USERNAME;

            var tipoActorRecursos = recursos.IdTipoActor;
            var descripcionVinculacion = "";
            foreach (var item in recursos.TiposVinculacion)
            {
                if (tipoActorRecursos == item.IdTipoActor)
                {
                    descripcionVinculacion = item.DescripcionTipoActor;
                }
            }

            var TasaEATipoCredito = await _creditoRepository.ObtenerTasasSimuladorByTipoCredito(descripcionVinculacion, 2);

            TasaSimulador tasaSimulador = new TasaSimulador(); 
            if (tipoActorRecursos == 1) //AFILIADO SOLUCION VIVIENDA
            {
                tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("LEASING AFILIADO 108 - 168"); 

            }else if(tipoActorRecursos == 15) //FUNCIONARIO AFILIADO SOLUCIÓN VIVIENDA
            {
                tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("LEASING FUNCIONARIO 108 - 168");
            }

            var parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
            {
                SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
            });
            if (parametrosGenerales.Status != string.Empty)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = parametrosGenerales.Status
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;
            var datosSimulacionPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento));
            var tiposVivienda = (await _catalogsBusinessLogic.ObtenerTiposDeVivienda()).Data;
            var proyeccionactual = (await _creditoBusinessLogic.ObtenerProyeccionCliente(cliente.GradoId.ToString())).Data;
            var simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(proyeccionactual.Id.ToString())).Data;

            //parametrosGenerales.Tasas.EfectivaAnual = TasaEATipoCredito.SIM_TASA_EA / 100;
            var categoriaCivil = false;
            if (parametrosGenerales.SeleccionCargoConSalario)
            {
                if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
                {
                    proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
                    categoriaCivil = true;
                }
                else if (proyeccionactual.Id == 10 || proyeccionactual.Id == 11 || proyeccionactual.Id == 12)
                {
                    double cuotasAfiliado = (double)recursos.NumeroCuotas / 12;

                    int cuotas = (int)Math.Round(cuotasAfiliado, 0, MidpointRounding.AwayFromZero);

                    var cargo = simcargo
                                .Where(x => x.salariobasico <= datosFinancieros.ValorSalario)
                                .OrderByDescending(x => x.salariobasico)
                                .FirstOrDefault()
                                ??
                                simcargo
                                    .Where(x => x.salariobasico > datosFinancieros.ValorSalario)
                                    .OrderBy(x => x.salariobasico)
                                    .FirstOrDefault();

                    if (cuotas < 7)
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 10;
                    }
                    else if (cuotas >= 7 && cuotas < 9)
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 11;
                    }
                    else
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 12;
                    }
                }
                else
                {
                    var cargo = simcargo
                                .Where(x => x.salariobasico <= datosFinancieros.ValorSalario)
                                .OrderByDescending(x => x.salariobasico)
                                .FirstOrDefault()
                                ??
                                simcargo
                                    .Where(x => x.salariobasico > datosFinancieros.ValorSalario)
                                    .OrderBy(x => x.salariobasico)
                                    .FirstOrDefault();

                    proyeccionactual.salariobasico = cargo.salariobasico;
                    proyeccionactual.Id = cargo.Id;

                }

            } else if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
                {
                    proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
                    categoriaCivil = true;
                }
            

            int tipoAfiliacionId = int.Parse(cliente.TipoAfiliacion.ToString());
            var viviendaVis = (request.valor / (int)parametrosGenerales.SalarioMinimo) < (double)parametrosGenerales.LimiteVIS ? "VIS" : "NO VIS";

            var valorInmuebleAlmacenado = request.valor > (double)request.avaluoInmueble ? request.valor : (double)request.avaluoInmueble;

            request.valor = request.valorInicialSimuladoInmueble > 0 ? (double)request.valorInicialSimuladoInmueble : valorInmuebleAlmacenado;

            #endregion
            #region Paso1
            var valorViviendaDeseado = request.valor;
            bool bloquearCondicion = true;

            //Este Valor se llena en el paso 7
            var valorViviendaMaximo = 0M;
            #endregion
            #region Paso3
            var tasaEfectivaAnualMensualizada = Math.Pow((double)(parametrosGenerales.Tasas.EfectivaAnual + 1), (1.0 / 12)) - 1;
            var porcentajeCanonInicial = 0.0;
            var porcentajeOpcionCompra = 0.0;
            var tasaCredito = parametrosGenerales.Tasas.EfectivaAnual;            
            var tasaSeguroVida = parametrosGenerales.Tasas.SeguroVida;
            var tasaSeguroHogar = parametrosGenerales.Tasas.SeguroHogar;
            var tasaPeriodicaNMV = (decimal)tasaEfectivaAnualMensualizada;


            var datoSeguroSimulacion = await _creditoRepository.ConsultarSeguroSimulacionCredito(request.documento);

            if (datoSeguroSimulacion.SSC_CONVENIO_VIDA == "N")
            {
                tasaSeguroVida = 0;
            }

            if (datoSeguroSimulacion.SSC_CONVENIO_HOGAR == "N")
            {
                tasaSeguroHogar = 0;
            }

            if (request.CanonInicial >= parametrosGenerales.Tasas.Min_Canon_Inicial && request.CanonInicial <= parametrosGenerales.Tasas.Max_Canon_Inicial)
            {
                porcentajeCanonInicial = (double)(request.CanonInicial / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "CANON INICIAL FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            if (request.porOpcionCompra <= parametrosGenerales.Tasas.Max_Opcion_Compra)
            {
                porcentajeOpcionCompra = (double)(request.porOpcionCompra / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser menor a 30%
            }

          

            var plazo = 0;
            var obtenerInfoCliente = await _requestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento);
            if (request.plazo != 0 && (request.plazo >= parametrosGenerales.MinimoPlazoMeses && request.plazo <= parametrosGenerales.MaximoPlazoMeses))
            {
                plazo = request.plazo;
            }
            else
            {
                plazo = parametrosGenerales.MaximoPlazoMeses;
            }

            if (((12 * 14) - recursos.NumeroCuotas) < 60)
            {
                plazo = 72; 
            }
            else
            {
                plazo = (12 * 14) - recursos.NumeroCuotas; 
            }

            var AnosPlazo = plazo % 12 == 0 ? (int)(plazo / 12) : (int)(plazo / 12) + 1;
            #endregion
            #region Paso4
            var factor = (decimal)(tasaPeriodicaNMV * (decimal)Math.Pow((1 + (double)tasaPeriodicaNMV), plazo) / (decimal)(Math.Pow(1 + (double)tasaPeriodicaNMV, plazo) - 1));
            var valorCanonInicial = valorInmuebleAlmacenado * porcentajeCanonInicial;
            #endregion
            #region Paso5
            var VrCredito = 0.0;
            var VrCreditoSegHogar = 0.0;
            
            if (request.valorPrestamo > 0 && request.valorPrestamo < (valorViviendaDeseado - valorCanonInicial))
            {
                valorCanonInicial = valorViviendaDeseado - request.valorPrestamo;
                VrCredito = request.valorPrestamo;
            }
            else
            {
                VrCredito = (double)(valorViviendaDeseado - valorCanonInicial);
            }

            

            var valorSeguroVida = (decimal)VrCredito * tasaSeguroVida;

            
            if (request.avaluoInmueble > 0)
            {
                VrCreditoSegHogar = (double)request.avaluoInmueble > (double)valorInmuebleAlmacenado ? (double)request.avaluoInmueble : (double)valorInmuebleAlmacenado;
            }
            else
            {
                VrCreditoSegHogar = (double)request.ValorViviendaSeguro > 0 ? (double)request.ValorViviendaSeguro : (double)valorViviendaDeseado;
            }


            var valorSeguroHogar = recalcular ? (decimal)request.ValorViviendaSeguro*tasaSeguroHogar :  (decimal)VrCreditoSegHogar * tasaSeguroHogar;
            var VrCuotaSinOpcC = ((decimal)(valorViviendaDeseado-valorCanonInicial) * factor) + valorSeguroHogar + valorSeguroVida;
            var valorOpcionDeCompra = (decimal)(valorViviendaDeseado * porcentajeOpcionCompra);
            if (valorOpcionDeCompra > (decimal)VrCredito && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA MAYOR AL VALOR DEL CREDITO", 
                    RespuestaRadicado = false
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            var VrDescOpcCompra = (valorOpcionDeCompra / (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), plazo)) * factor;
            var canonTradicional = VrCuotaSinOpcC - VrDescOpcCompra;
            #region Ahorros y Cesantias

            await _creditoRepository.LimpiarRecursosCongelados(simulacionId, request.documento);

            decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;
            decimal RecursosCuentaIndiv = ahorroProgramado + cesantiasProgramadas + intCesantiasProgramadas + intAhorroProgramados;
            var MaximoUsar = RecursosCuentaIndiv * (decimal)(1 - 0.15);

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 108-168"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 108-168"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 108-168"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 108-168"
                }
            };


            foreach (var recurso in recursosCongelados)
            {
                await _creditoRepository.InsertarRecursosCongelados(recurso);
            }


            
            await _creditoRepository.LimpiarConceptosCongelados(simulacionId, request.documento);


            List<string> descripcionesConceptos = new List<string>
            {
                "AHORROS OBLIGATORIOS",
                "AHORROS RETROACTIVOS",
                "AHORROS VOLUNTARIOS",
                "INTERESES APORTES",
                "CESANTÍAS",
                "CESANTÍAS RETROACTIVOS",
                "INTERESES CESANTÍAS"
            };

            List<ConceptosCongeladosProspeccion> listaCongeladosProspeccion = new List<ConceptosCongeladosProspeccion>();

            foreach (var cuentas in recursos.Cuentas)
            {
                foreach (var conceptos in cuentas.Conceptos)
                {
                    if (descripcionesConceptos.Contains(conceptos.ConceptoDescripcion))
                    {
                        listaCongeladosProspeccion.Add(new ConceptosCongeladosProspeccion
                        {
                            CON_DESCRIPCION = conceptos.ConceptoDescripcion,
                            CON_VALOR = conceptos.saldo,
                            SLS_ID = simulacionId,
                            CON_NUMERO_DOCUMENTO = request.documento
                        });
                    }
                }
            }

            // Ordenar la lista de acuerdo al orden en descripcionesConceptos
            listaCongeladosProspeccion = listaCongeladosProspeccion
                .OrderBy(concepto => descripcionesConceptos.IndexOf(concepto.CON_DESCRIPCION))
                .ToList();

            // Insertar cada concepto en el orden correcto
            foreach (var concepto in listaCongeladosProspeccion)
            {
                await _creditoRepository.InsertarConceptosCongelados(concepto);
            }








            #endregion
            var RecursosCI = 0M;
            if (MaximoUsar > (decimal)valorCanonInicial)
            {
                RecursosCI = (decimal)valorCanonInicial;
            }
            else
            {
                RecursosCI = RecursosCuentaIndiv;
            }
            decimal RecursosPropios = (decimal)valorCanonInicial - RecursosCI;
            #region RecursosProyectados
            var antiguedadcalculo = Math.Round(((decimal)obtenerInfoCliente.Data.NumeroCuotas / 12), 1);
            var antiguedadaños = (int)Math.Round(antiguedadcalculo, 0, MidpointRounding.AwayFromZero);
            #region Factores
            decimal factorIncremento = parametrosGenerales.Factores.FactorIncremento / 100;
            decimal factorIncrementoSM = parametrosGenerales.Factores.FactorIncrementoSubsidio / 100;
            decimal factorAhorro = request.TasaVoluntariaAportes ? (parametrosGenerales.Factores.FactorAhorro + cliente.TasaVoluntariaAportes) / 100 : parametrosGenerales.Factores.FactorAhorro / 100;
            decimal factorInflacion = parametrosGenerales.Factores.FactorInflacion / 100;
            decimal factorCesantias = parametrosGenerales.Factores.FactorCesantias / 100;
            decimal factorInteresesCesantias = 12.0M / 100;
            #endregion
            #region Valores Para Proyeccion
            #region Uso Canon Inicial

            var valorIntCesantiasCanonInicial = intCesantiasProgramadas < (decimal)valorCanonInicial ? intCesantiasProgramadas : (decimal)valorCanonInicial;
            var valorIntAhorroCanonInicial = intAhorroProgramados < ((decimal)valorCanonInicial - valorIntCesantiasCanonInicial) ? intAhorroProgramados : ((decimal)valorCanonInicial - valorIntCesantiasCanonInicial);
            var valorCesantiasCanonInicial = cesantiasProgramadas < ((decimal)valorCanonInicial - intAhorroProgramados - valorIntCesantiasCanonInicial) ? cesantiasProgramadas : ((decimal)valorCanonInicial - valorIntAhorroCanonInicial - valorIntCesantiasCanonInicial);
            var valorAhorroCanonInicial = ahorroProgramado < ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial)) ? ahorroProgramado : ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial));

            #endregion
            #region Actualizacion Ahorros y Cesantias Para Proyeccion
            ahorroProgramado = ahorroProgramado - valorAhorroCanonInicial;
            cesantiasProgramadas = cesantiasProgramadas - valorCesantiasCanonInicial;
            intCesantiasProgramadas = intCesantiasProgramadas - valorIntCesantiasCanonInicial;
            intAhorroProgramados = intAhorroProgramados - valorIntAhorroCanonInicial;
            #endregion
            #region Proyeccion
            //Ejercicio valor i del for en 0
            
            int CountAnioRegimen = int.Parse(datosPersonales.Regimen) - DateTime.Now.Year;
            
            decimal sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
            decimal SMProyectado = (decimal)parametrosGenerales.SalarioMinimo;
            decimal AhorrosAnt = ahorroProgramado;
            decimal AcumuladoAhorros = ahorroProgramado + intAhorroProgramados;
            decimal IntAhorrosAnt = intAhorroProgramados;
            decimal AcumuladoIntAhorros = intAhorroProgramados;
            decimal CesantiasAnt = cesantiasProgramadas;
            decimal AcumuladoCesantias = cesantiasProgramadas;
            decimal IntCesantiasAnt = intCesantiasProgramadas;
            decimal AcumuladoIntCesantias = intCesantiasProgramadas;
            decimal TOTAcumuladoAhorros = 0M;
            decimal TOTAcumuladoCesantias = 0M;
            double sueldoProyeccion = 0;
            decimal IntAhorro = 0M;
            var countAnios = 1;
            var countCategoria = 0;
            var ctg = proyeccionactual.Id;
            var antiguedadañosTemp = antiguedadaños;
            string StInfoProy = string.Empty;
            //while (antiguedadañosTemp >= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault())
            //{
            //    countCategoria++;
            //    countAnios = antiguedadañosTemp - simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
            //    antiguedadañosTemp = antiguedadañosTemp - simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
            //    if (simcargo.Any(x => x.Id == ctg + 1))
            //    {
            //        ctg = proyeccionactual.Id + countCategoria;
            //        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
            //        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault();
            //    }
            //}
            List<string> LstProyeccion = new List<string>();

            StInfoProy = "Años Antiguedad: " + antiguedadaños;
            

            List<InformacionProyeccion> listaInformacionProyeccion = new List<InformacionProyeccion>();

            InformacionProyeccion informacionProyeccion = new InformacionProyeccion();

            informacionProyeccion.IdProyeccion = 0;
            informacionProyeccion.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
            informacionProyeccion.AniosCargo = countAnios;
            informacionProyeccion.Sueldo = (double)sueldo;
            informacionProyeccion.AhorroProgramado = ahorroProgramado + intAhorroProgramados;
            informacionProyeccion.InteresesAhorroProgramado = intAhorroProgramados;
            informacionProyeccion.Cesantias = cesantiasProgramadas;
            informacionProyeccion.InteresesCesantias = intCesantiasProgramadas;
            informacionProyeccion.AniosAntiguedad = antiguedadaños;
            int aniosCargoProyecion = antiguedadaños == 0 ? countAnios : antiguedadaños;
            listaInformacionProyeccion.Add(informacionProyeccion);


            decimal interesAhorradoAnual = 0;
            decimal totalAhorradoAnual = 0;
            decimal AcumuladoIntAhorrosDos = 0;
            decimal totalAhorroProgramado = 0;
            decimal totalCesantiasProgramadas = 0;
            decimal interesCesantiasAnual = 0;
            decimal acumuladoIntCesantiasDos = 0;
            int aumentoCategoria = 1;
            decimal salaraMinimoProyectadoAntiguedadMax = 0;
            decimal acumuladoCesantiasTotales = 0; 

            for (int i = 1; i <= AnosPlazo; i++)
            {
                countAnios++;
                var aumentosueldo = Math.Pow((1 + (double)factorIncremento), i);
                sueldoProyeccion = Math.Round((double)sueldo * aumentosueldo, 2);


                decimal Ahorro = Math.Round((decimal)sueldoProyeccion * factorAhorro * 12, 2);

                if (i < 3)
                {
                    AcumuladoAhorros += Ahorro;

                    AcumuladoIntAhorrosDos += i > 0 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;


                    AcumuladoIntAhorros = i > 0 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;


                    totalAhorradoAnual = i > 0 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;

                    AhorrosAnt = Ahorro;
                }
                else
                {
                    AcumuladoAhorros += Ahorro;

                    AcumuladoIntAhorrosDos += i > 0 ? Math.Round((AhorrosAnt) * factorInflacion, 2) : 0;


                    AcumuladoIntAhorros = i > 0 ? Math.Round((AhorrosAnt) * factorInflacion, 2) : 0;


                    totalAhorradoAnual = i > 0 ? Math.Round((AhorrosAnt) * factorInflacion, 2) : 0;

                    AhorrosAnt = Ahorro;
                }

                


                decimal Cesantias = Math.Round((decimal)sueldoProyeccion * factorCesantias * 12, 2);

                AcumuladoCesantias += Cesantias;

                acumuladoIntCesantiasDos += i > 0 ? Math.Round((CesantiasAnt) * factorInflacion, 2) : 0;

                acumuladoCesantiasTotales = AcumuladoCesantias + acumuladoIntCesantiasDos; 


                AcumuladoIntCesantias = i > 0 ? Math.Round((CesantiasAnt) * factorInflacion, 2) : 0;

                interesCesantiasAnual = i > 0 ? Math.Round((CesantiasAnt) * factorInflacion, 2) : 0;


                CesantiasAnt += Cesantias + interesCesantiasAnual;


                if (antiguedadaños > 0 && i == 1)
                {
                    aniosCargoProyecion++;
                }

              


                if (i <= AnosPlazo)
                {
                    SMProyectado = simcargo.Where(x => x.Id == ctg).Select(x => x.ProyectaSubsidio).FirstOrDefault() ? SMProyectado + (SMProyectado * factorIncrementoSM) : parametrosGenerales.SalarioMinimo;
                }


                InformacionProyeccion informacionProyeccionCredito = new InformacionProyeccion();

                informacionProyeccionCredito.IdProyeccion = i;
                informacionProyeccionCredito.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
                informacionProyeccionCredito.AniosCargo = countAnios;
                informacionProyeccionCredito.Sueldo = sueldoProyeccion;
                informacionProyeccionCredito.AhorroProgramado = i > 0 ? AcumuladoAhorros + AcumuladoIntAhorrosDos : AcumuladoAhorros;
                informacionProyeccionCredito.InteresesAhorroProgramado = i > 0 ? AcumuladoIntAhorros : 0;
                informacionProyeccionCredito.Cesantias = i > 0 ? AcumuladoCesantias + acumuladoIntCesantiasDos : AcumuladoCesantias;
                informacionProyeccionCredito.InteresesCesantias = i > 0 ? AcumuladoIntCesantias : 0;
                informacionProyeccionCredito.AniosAntiguedad = aniosCargoProyecion++;


                if (informacionProyeccionCredito.AniosAntiguedad == 14)
                {
                    salaraMinimoProyectadoAntiguedadMax = (decimal)SMProyectado; 
                }

                totalAhorroProgramado = informacionProyeccionCredito.AhorroProgramado;
                totalCesantiasProgramadas = informacionProyeccionCredito.Cesantias;

                listaInformacionProyeccion.Add(informacionProyeccionCredito);


                if (simcargo.Where(x => x.Id == ctg).Select(x => x.AñoAscenso).FirstOrDefault() == countAnios)
                {
                    countCategoria++;
                    if (simcargo.Any(x => x.Id == ctg + 1))
                    {
                        //si es igual a estas categorias no tiene porque subir o ascender de categoria. 
                        //7   Civil Profesional
                        //8   Civil Tecnico
                        //9   Civil Asistencial
                        if (ctg != 7 && ctg != 8 && ctg != 9)
                        {
                            ctg = proyeccionactual.Id + countCategoria;
                        }
                        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
                        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                        countAnios = 0;
                    }
                }
            }
            #endregion
            #region Valores Proyectados
            var subsidioCategoria = (await _catalogsBusinessLogic.ObtenerSimulacionCargoPorCategoria(cliente.CategoriaId, ctg)).Data.FactorSubsidio;
            var subsidiovivienda = Math.Round(((double)salaraMinimoProyectadoAntiguedadMax * subsidioCategoria), 2);
            TOTAcumuladoAhorros = AcumuladoAhorros + AcumuladoIntAhorros;
            TOTAcumuladoCesantias = AcumuladoCesantias + AcumuladoIntCesantias;
            var RecursosCanonInicial = valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial + valorAhorroCanonInicial;
            var TotalOpcionCompra = (decimal)subsidiovivienda + totalAhorroProgramado + totalCesantiasProgramadas;
            var DivCuoEquiVF = Math.Pow((double)(1 + tasaPeriodicaNMV), plazo);
            var CuotaEquivalenteVF = (TotalOpcionCompra * factor) / (decimal)DivCuoEquiVF;
            var CanonReducido = VrCuotaSinOpcC - CuotaEquivalenteVF;
            var CapitalReducido = (CanonReducido - valorSeguroHogar - valorSeguroVida) / factor;


            decimal valorSubsidioCalculado = recursos.ValorSubsidio;
            int anioRegimen = recursos.Regimen; 


            #region Valores a Guardar
            valoresProyectadosDto.Svp_Id = Guid.NewGuid();
            valoresProyectadosDto.Sls_Id = datosPersonales.IdSimulacion;
            valoresProyectadosDto.Svp_EA = parametrosGenerales.Tasas.EfectivaAnual;
            valoresProyectadosDto.Svp_Cesantias = totalCesantiasProgramadas;
            valoresProyectadosDto.Svp_Sueldo = (decimal)sueldoProyeccion;
            valoresProyectadosDto.Svp_Ahorros = totalAhorroProgramado;
            valoresProyectadosDto.Svp_Subsidio =
                Math.Round(
                    anioRegimen < DateTime.Now.Year
                        ? valorSubsidioCalculado
                        : (decimal)subsidiovivienda,
                    0,
                    MidpointRounding.AwayFromZero);
            var ValoresProyectados = await _creditoBusinessLogic.GuardarValoresProyectados(valoresProyectadosDto);
            #endregion
            #endregion
            #endregion
            #endregion
            #endregion
            #region Paso6
            var totalIngresos = (double)datosFinancieros.TotalIngresos;
            var totalEgresos = (double)datosFinancieros.TotalEgresos;
            var capacidadPago = (totalIngresos * 0.5) - totalEgresos;
            #endregion
            #region Paso7
            var Valor1 = (decimal)(valorViviendaDeseado * (1 - porcentajeCanonInicial - porcentajeOpcionCompra));
            var Valor2 = ((decimal)capacidadPago + CuotaEquivalenteVF) / (factor + (tasaSeguroHogar / (1 / (decimal)porcentajeCanonInicial)));
            var VrCredito2 = Math.Min(Valor1, Valor2);
            valorViviendaMaximo = VrCredito2 / (1 - (decimal)(porcentajeCanonInicial + porcentajeOpcionCompra));
            //if (bloquearCondicion && Math.Round(valorViviendaMaximo, 0, MidpointRounding.AwayFromZero) < Math.Round((decimal)valorViviendaDeseado, 0, MidpointRounding.AwayFromZero))
            //{
            //    request.valor = (double?)valorViviendaMaximo;
            //    return await CrearSimulacionLeasing108(request, datosPersonales, simulacionId);
            //}
            #endregion
            

            int vinculacion = recursos.IdTipoActor;
            string estadoCuenta = ""; 

            if (vinculacion != 14)
            {


                //vinculacion 16 y 2 pertecenten a FUNCIONARIO AFILIADO ADMINISTRACION CESANTÍAS y AFILIADO ADMINISTRACION CESANTIAS respectivamente
                foreach (var cuentas in recursos.Cuentas)
                {
                    if (vinculacion == 16 || vinculacion == 2)
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;
                           
                        }
                    }
                    else
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;
                           
                        }
                    }
                }

                if (estadoCuenta == "MOROSA")
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "El afiliado presenta cuenta con estado MOROSA"
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }


            if (CanonReducido < parametrosGenerales.ValorMinimoCuota)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "La cuota debe ser mayor a la mínima permitida (" + parametrosGenerales.ValorMinimoCuota.ToString("C0", new CultureInfo("es-CO")) + ") por la entidad, intente nuevamente con un valor mayor del inmueble.",
                    RespuestaRadicado = false
                });  

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }
            ValidacionCreditoProspeccion validacionesExcepciones = new ValidacionCreditoProspeccion();

            if (validacionesExcepciones == null)
            {
                validacionesExcepciones.VCP_ESTADO = false;
                validacionesExcepciones.VCP_NUMERO_DOCUMENTO = request.documento;

                await _creditoRepository.InsertarActualizarValidacionCreditoProspeccion(validacionesExcepciones);
            }

            ValidacionCreditoProspeccion validacionesExcepcion = await _creditoRepository.ConsultarValidacionCreditoProspeccion(request.documento);

            if (VrCredito < (double)subsidiovivienda && !recalcular)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "El valor a financiar no puede ser menor al subsidio de vivienda proyectado",
                    RespuestaRadicado = false
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };

            }

            if (VrCredito < (double)subsidiovivienda && recalcular)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "No se encontró un valor a financiar que sea superior al subsidio de vivienda proyectado",
                    RespuestaRadicado = false
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };

            }

            double totalObligaciones = 0;
            string validacionEndeudamientoGlobal = ""; 
            #region EndeudamientoGlobal
            if (parametrosGenerales.ActiveTU && request.tipoSimularOProspectar == 3)
            {
                try
                {
                    #region ConsumoTU
                    ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                    {
                        Apellidos = recursos.PrimerApellido,
                        FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                        Identificacion = request.documento,
                        SalarioMensual = Math.Round((decimal)totalIngresos),
                        TipoIdentificacion = request.tipoIdentificacion,
                        Telefono = recursos.Telefono,
                        TraerHistorialCrediticio = "Y",
                        SLS_ID = simulacionId
                    };
                    if (prospeccionTransUnionRequestDto.Telefono.Length < 10 || prospeccionTransUnionRequestDto.Telefono == null)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el teléfono"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    if (prospeccionTransUnionRequestDto.SalarioMensual <= 0)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el salario"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                    var ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                    #endregion
                    ReporteSimulacion60PorCientoDto reporteSimulacion60PorCientoDto = new ReporteSimulacion60PorCientoDto();
                    reporteSimulacion60PorCientoDto.NumeroDocumento = request.documento;
                    double TOT = totalIngresos - totalEgresos;
                    #region Reporte 60%
                    reporteSimulacion60PorCientoDto.salarioTotal = String.Format("{0:N0}", totalIngresos);
                    reporteSimulacion60PorCientoDto.deducciones = String.Format("{0:N0}", totalEgresos);
                    reporteSimulacion60PorCientoDto.salarioNeto = String.Format("{0:N0}", TOT);
                    reporteSimulacion60PorCientoDto.capacidadPago = String.Format("{0:N0}", capacidadPago);
                    reporteSimulacion60PorCientoDto.canonCuotaMensual = String.Format("{0:N0}", Math.Round(CanonReducido, 0, MidpointRounding.AwayFromZero));
                    #endregion
                    if (!string.IsNullOrEmpty(ProspeccionTransUnion.Data.response))
                    {
                        var ReporteTrans = await _reportesBusinessLogic.ReporteTransUnionPropiedades(ProspeccionTransUnion.Data.response);
                        #region Validacion Score y Viabilidad
                        if (!ReporteTrans.IsSuccess)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no viable por consulta TransUnion."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                       

                        #endregion

                        #region GuardadoReporteTransUnion
                        using (var memoryStream = new MemoryStream(ReporteTrans.Data.ArchivoBytes))
                        {
                            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            await SaveReportTU(formFile, simulacionId);
                        }
                        #endregion
                        #region Reclasificacion
                        IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, request.documento);
                        var sumaSaldoObligacion = prospeccionResult
                        .Where(linea => linea.OLN_ESTADO_RECALCULAR == false)
                        .Sum(linea => linea.OLN_SALDO_OBLIGACION);
                        #endregion
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.cuotaSectorFinanSoliAse = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorFinanciero);
                        reporteSimulacion60PorCientoDto.cuotaSectorReal = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorReal);
                        #endregion
                        var EndeudamientoG_Sin_Cuota = ReporteTrans.Data.cuotasSectorReal + ReporteTrans.Data.cuotasSectorFinanciero - (double)sumaSaldoObligacion;
                        totalObligaciones = EndeudamientoG_Sin_Cuota; 
                        var EndeudamientoG_Sin_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoG_Sin_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        var EndeudamientoActual_Cuota = (double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos - (double)sumaSaldoObligacion;
                        var EndeudamientoActual_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoActual_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        Score = ReporteTrans.Data.riesgoMinimo;
                        CuotaSectorFinanciero = (decimal)ReporteTrans.Data.cuotasSectorFinanciero;
                        CuotaSectorReal = (decimal)ReporteTrans.Data.cuotasSectorReal;
                        EndeudamientoSCH = EndeudamientoG_Sin_Cuota_Porc;
                        EndeudamientoCCH = EndeudamientoActual_Cuota_Porc;


                        
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.totalObliAntesCredito = String.Format("{0:N0}", EndeudamientoG_Sin_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalSinCanon = (EndeudamientoG_Sin_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.totalObliNuevoCredito = String.Format("{0:N0}", EndeudamientoActual_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalCanon = (EndeudamientoActual_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.disponibleLibranza = String.Format("{0:N0}", (capacidadPago - (double)CanonReducido));
                        reporteSimulacion60PorCientoDto.controlCapacidadPago = capacidadPago >= (double)CanonReducido ? "SI APLICA" : "NO APLICA";


                        var ControNivelEndeudamiento_Cuota_Porc = EndeudamientoActual_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";
                        var ControNivelEndeudamiento_Sin_Cuota_Porc = EndeudamientoG_Sin_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";

                        

                        DatosEndeudamientoProspeccionDto datosEndeudamientoProspeccion = new DatosEndeudamientoProspeccionDto();
                        datosEndeudamientoProspeccion.NumeroDocumento = datosPersonales.NumeroDocumento;
                        datosEndeudamientoProspeccion.SalarioUltimoMes = (decimal)totalIngresos;
                        datosEndeudamientoProspeccion.Descuentos = (decimal)totalEgresos;
                        datosEndeudamientoProspeccion.SalarioNeto = (decimal)TOT;
                        datosEndeudamientoProspeccion.CapacidadPago = (decimal)capacidadPago;
                        datosEndeudamientoProspeccion.CanonMensual = CanonReducido;
                        datosEndeudamientoProspeccion.SectorFinanciero = CuotaSectorFinanciero - sumaSaldoObligacion;
                        datosEndeudamientoProspeccion.SectorReal = CuotaSectorReal;
                        datosEndeudamientoProspeccion.Score = Score;
                        datosEndeudamientoProspeccion.DisponibleLibranza = ((decimal)capacidadPago - CanonReducido);
                        datosEndeudamientoProspeccion.EndeudamientoGlobalCuota = (decimal)EndeudamientoActual_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalCuota = ControNivelEndeudamiento_Cuota_Porc;
                        datosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota = (decimal)EndeudamientoG_Sin_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalSinCuota = ControNivelEndeudamiento_Sin_Cuota_Porc;
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalCuota = EndeudamientoActual_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalSinCuota = EndeudamientoG_Sin_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.TotalCanonDeducciones = ((decimal)totalEgresos + CanonReducido + (decimal)totalObligaciones);
                        datosEndeudamientoProspeccion.DecisionHomologada = ReporteTrans.Data.descripcionDecision;
                        await _creditoRepository.InsertarDatosEndeudamientoProspeccion(datosEndeudamientoProspeccion);


                        var valoresSimulacionProspeccionInsert = new ValoresSimulacionCreditoReclasificacion
                        {
                            NUMERO_DOCUMENTO = request.documento,
                            TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                            TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                            SEGURO_VIDA = valorSeguroVida,
                            SEGURO_HOGAR = valorSeguroHogar,
                            SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                            SEGURO_VIDA_EXTRAPRIMA = 0,
                            TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                            TASA_MIXTA_EXTRAPRIMA = 0,
                            FACTOR = 0,
                            FACTOR_EXTRA_PRIMA = 0,
                            PLAZO = plazo,
                            VALOR_PRESTAMO = (decimal)request.valor - (decimal)valorCanonInicial,
                            CUOTA_SIN_SEGURO_HOGAR = CanonReducido - valorSeguroVida,
                            CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            CUOTA_SEGURO_HOGAR = CanonReducido,
                            CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                            VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                            CAPACIDAD_PAGO = (decimal)(capacidadPago),
                            VALOR_PRESTAMO_SEGUN_CUOTA = (decimal)request.valor
                        };

                        await _creditoRepository.InsertValoresSimulacionCredito(valoresSimulacionProspeccionInsert);

                        if (ReporteTrans.Data.fechaPermanenciaVigente != null && ReporteTrans.Data.fechaPermanenciaVigente != "" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = ReporteTrans.Data.fechaPermanenciaVigente
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        if (ReporteTrans.Data.totalObligacionMora > 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "NO VIABLE: Motivo evaluación de políticas (por presentar deuda en mora)"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        if (request.valorPrestamo > 0 && reporteSimulacion60PorCientoDto.controlCapacidadPago == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co" && !recalcular)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if ((ReporteTrans.Data.descripcionDecision.Equals("PENDIENTE") && ControNivelEndeudamiento_Cuota_Porc == "NO APLICA") && request.Usr_id == null && Score <= parametrosGenerales.Score_Min_TU)
                        {
                            planillaSimulacion.Clear();

                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Para continuar con el proceso, por favor acérquese a un punto de atención",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        else
                        {
                            if (ReporteTrans.Data.riesgoMinimo <= parametrosGenerales.Score_Min_TU && !validacionesExcepcion.VCP_ESTADO)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "No se puede presentar una simulación, no cumple con el score minimo: " + parametrosGenerales.Score_Min_TU
                                });

                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                        }

                        #endregion
                        #region Validacion y Credito segun endeudamiento Global

                        if (Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero) > ObjetivoEndeudameinto && !bloquearCondicion)
                        {
                            //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto)
                            //{
                                //try
                                //{
                                //    //CanonReducido = Math.Round((decimal)await CalcularCuotaNueva(ObjetivoEndeudameinto, 0, EndeudamientoActual_Cuota, 0, totalIngresos), 0, MidpointRounding.AwayFromZero) - (decimal)EndeudamientoG_Sin_Cuota - (decimal)totalEgresos;
                                //    //EndeudamientoActual_Cuota = await CalcularEndeudamiento((double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos, totalIngresos);
                                //    //VrCredito = (double)(((CanonReducido - valorSeguroVida - valorSeguroHogar) / factor) + valorOpcionDeCompra);
                                //    //request.valorPrestamo = VrCredito;
                                //    //#region Reporte 60%
                                //    //reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = String.Format("{0:N0}", Math.Round(CanonReducido, 0, MidpointRounding.AwayFromZero));
                                //    //reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = String.Format("{0:N0}", ((double)CanonReducido + EndeudamientoG_Sin_Cuota));
                                //    //reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = Math.Round((EndeudamientoActual_Cuota * 100), 0, MidpointRounding.AwayFromZero).ToString() + "%";
                                //    //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc <= ObjetivoEndeudameinto)
                                //    //{
                                //    //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                //    //}
                                //    //else
                                //    //{
                                //    //    if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc > ObjetivoEndeudameinto)
                                //    //    {
                                //    //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NUEVO VALOR CUOTA";
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                //    //    }
                                //    //}
                                //    #endregion
                                //    if (Math.Round(VrCredito, 0, MidpointRounding.ToZero) <= 0)
                                //    {
                                    //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    //    planillaSimulacion.Clear();
                                    //    planillaSimulacion.Add(new SimulacionDto
                                    //    {
                                    //        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 80%"
                                    //    });
                                    //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    //}
                                    //valorViviendaMaximo = (decimal)(VrCredito / (1 - (porcentajeCanonInicial + porcentajeOpcionCompra)));
                                    //request.valor = (double)valorViviendaMaximo;
                                    //ConteoTU++;
                                    ////reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = ConteoTU > 5 ? "NO APLICA" : "SI APLICA";

                                    ////var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                                    ////double toleranciaIngresosTotales = (double)(datos.EDP_SALARIO_TOTAL) * 0.592;
                                    ////var plazoValorMaximo = plazo; 
                                    ////double diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos.EDP_SECTOR_FINANCIERO - (double)datos.EDP_SECTOR_REAL - (double)datos.EDP_DEDUCCIONES;                                    
                                    ////var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1; 
                                    ////var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                                    ////var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                                    ////var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo; 
                                    ////double valorMaximoCalculado = prestamosMaximo / 1000000;
                                    ////double valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                                    ////if (valorMaximo <= 13000000 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                                    ////{
                                    ////    planillaSimulacion.Add(new SimulacionDto 
                                    ////    {
                                    ////        Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                                    ////    });

                                    ////    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    ////}

                                    ////if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                                    ////{
                                    ////    planillaSimulacion.Clear();
                                    ////    planillaSimulacion.Add(new SimulacionDto
                                    ////    {
                                    ////        Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo " + valorMaximo.ToString("C0", new System.Globalization.CultureInfo("es-CO")) + ", por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                                    ////    });

                                    ////    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    ////}

                                    //#region Save Reporte 60%
                                    //var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                    //using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                    //{
                                    //    IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                    //    await SaveReportTU60(formFile, simulacionId);
                                    //}

                                    //#endregion
                                    ////if (ConteoTU > 5)
                                    ////{
                                    ////    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    ////    planillaSimulacion.Clear();
                                    ////    planillaSimulacion.Add(new SimulacionDto
                                    ////    {
                                    ////        Sobrante = "No se puede presentar una simulación, Verificar endeudamiento"
                                    ////    });
                                    ////    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    ////}
                                    ////else
                                    ////{
                                    ////    return await CrearSimulacionLeasing108(request, datosPersonales, simulacionId);
                                    //}

                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        planillaSimulacion.Clear();
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = ex.Message
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }
                            ////}
                            //else
                            //{
                            //    #region Reporte 60%
                            //    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            //    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            //    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //    #endregion
                            //    #region Save Reporte 60%
                            //    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //    {
                            //        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //        await SaveReportTU60(formFile, simulacionId);
                            //    }
                            //    #endregion
                            //    planillaSimulacion.Add(new SimulacionDto
                            //    {
                            //        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //    });
                            //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //}
                        }
                        else
                        {
                            
                                #region Reporte 60%
                                reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                                reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                                reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                                reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                #endregion
                                #region Save Reporte 60%
                                var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                {
                                    IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                    await SaveReportTU60(formFile, simulacionId);
                                }
                                #endregion
                            
                        }
                        #endregion
                    }
                    else
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = ProspeccionTransUnion.Data.detalleError
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                }
                catch (Exception ex)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = ex.Message
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }
            #endregion
            if (CanonReducido > (decimal)capacidadPago && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co" || validacionEndeudamientoGlobal == "NO APLICA")
            {

                //decimal factorFinanciero = (decimal)(1 - Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo));
                //decimal factorDescuento = (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo);
                //decimal totalRecursosFuturos = totalCesantiasProgramadas + totalAhorroProgramado + (decimal)subsidiovivienda;
                //// Numerador


                //decimal capacidadPagoCalculada = ((decimal)totalIngresos / 2) - (decimal)totalObligaciones - (decimal)totalEgresos - valorSeguroHogar;
                //decimal valorViviendaMaximoNumerador = 0;
                //if (parametrosGenerales.ObligacionesExternasCapacidadPago)
                //{
                //    valorViviendaMaximoNumerador = (decimal)valorCanonInicial + ((((decimal)capacidadPago - valorSeguroHogar - valorSeguroVida - (decimal)totalObligaciones) / tasaPeriodicaNMV) * factorFinanciero) + totalRecursosFuturos * factorDescuento;
                //}
                //else
                //{
                //    valorViviendaMaximoNumerador = (decimal)valorCanonInicial + ((((decimal)capacidadPago - valorSeguroHogar - valorSeguroVida) / tasaPeriodicaNMV) * factorFinanciero) + totalRecursosFuturos * factorDescuento;
                //}


                //// Denominador
                //decimal valorViviendaMaximoDenominador = 1;

                // Resultado
                //valorViviendaMaximo = valorViviendaMaximoNumerador / valorViviendaMaximoDenominador;

                valorViviendaMaximo = (decimal) BuscarValorMaximoFinanciar(VrCredito,totalIngresos,(double) parametrosGenerales.PorcentajeMinimoEndeudamiento, totalObligaciones); 
                
                decimal valorMaximoPrestarCalculado = Math.Truncate(valorViviendaMaximo / 1000000.0m) * 1000000.0m;

                request.valorPrestamo = (double)valorMaximoPrestarCalculado;

                request.valor = (double)valorMaximoPrestarCalculado;

                if ((double)valorMaximoPrestarCalculado < 0)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento global"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }
                //ConteoTU = 0;
                if (ConteoTU < 100)
                {
                    ConteoTU++;

                    return await CrearSimulacionLeasing108(request, datosPersonales, simulacionId, true);

                }
                else
                {
                    ConteoTU = 0; // Opcional: reiniciar para futuras ejecuciones

                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }

            }
            else
            {
                #region Crear Simulacion CLiente
                var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
                {
                    Id = datosPersonales.IdSimulacion,
                    EnvioNotificacion = false,
                    Estado = "I",
                    FechaActualiza = DateTime.Now,
                    FechaSolicitud = DateTime.Now,
                    NumeroPreAprobado = 0,
                    ProblemaEmail = false,
                    UsuarioActualiza = Guid.NewGuid(),
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    Score = Score,
                    EndeudamientoCCH = (int)EndeudamientoCCH,
                    EndeudamientoSCH = (int)EndeudamientoSCH
                });
                var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(new SMCSimulacionClienteDto()
                {
                    Documento = request.documento,
                    OpcionCompra = (double)valorOpcionDeCompra,
                    TotalCuotas = plazo,
                    FechaSimulacion = DateTime.Now,
                    IdDatosPersonales = datosPersonales.Id,
                    IdSimulacion = simulacionId,
                    IdSimulacionCliente = datosPersonales.IdSimulacion,
                    NumeroPreaprobado = 0,
                    TasaEa = tasaCredito,
                    TasaFrech = 0,
                    TasaMv = (double)tasaPeriodicaNMV,
                    TipoAbono = "N",
                    TipoAlivio = "N",
                    TipoVivienda = request.ViviendaNueva ? "N" : "U",
                    TomaSeguro = "N",
                    UsaRecursosAcumulados = RecursosPropios > 0 ? "S" : "N",
                    ValorPrestamo = (decimal)VrCredito,
                    ValorPrestamosLeasing = (double)VrCredito2,
                    ValorVivienda = (double?)valorViviendaMaximo,
                    ViviendaVis = viviendaVis,
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    TasaSeguroHogar = (double)parametrosGenerales.Tasas.SeguroHogar,
                    TasaSeguroVida = (double)parametrosGenerales.Tasas.SeguroVida,
                    ValorPrestamoSolicitado = request.valorPrestamo,
                    PorcentajeCanonInicial = request.CanonInicial,
                    PorcentajeOpcionCompra = request.porOpcionCompra,
                    ValorViviendaSeguro = request.ValorViviendaSeguro,
                    AportesVoluntarios = request.TasaVoluntariaAportes ? 1 : 0,
                    Usr_id = request.Usr_id, 
                    afl_id = request.afl_id,
                    CLC_ID = 2
                });
                #endregion

                SolicitudCredito solicitudGeneralCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, request.documento);


                #region Simulacion
                for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        var fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        PlanSimulacion.Add(new Leasing108()
                        {
                            Capital = 0,
                            IntCte = 0,
                            SegVida = 0,
                            SegHogar = 0,
                            Cuota = 0,
                            Saldo = (decimal)request.valor - (decimal)valorCanonInicial,
                            NroCuota = i,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            SaldoCapitalReducido = CapitalReducido,
                            CapitalAPagar = 0,
                            CapitalPorPagar = 0,
                            InteresesCorrientesAPagar = 0,
                            InteresesCorrientesPorPagar = 0,
                            SeguroHogar = 0,
                            SeguvoVida = 0,
                            SeguroPorPagar = 0,
                            CuotaUnidadPagadora = 0,
                            AfectacionCuentaIndividual = 0,
                            DeudaRemanente = CapitalReducido,
                            BeneficioTasaFresh = 0,
                            BeneficioAlivioCuota = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = (decimal)(request.valor - valorCanonInicial)
                        });
                    }
                    else
                    {
                        var IntCte = i == 1 ? (decimal)(PlanSimulacion[i - 1].Saldo - (decimal)valorCanonInicial) * tasaPeriodicaNMV : (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional) * tasaPeriodicaNMV;
                        var Capital = canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;
                        var SaldoLeasingTradicional = (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional - Capital);
                        var CapitalTardicional = canonTradicional - IntCte;
                        var FechaCorteTemp = PlanSimulacion[i - 1].FechaCorte.AddMonths(1);
                        var FechaCorte = new DateTime(FechaCorteTemp.Year, FechaCorteTemp.Month, DateTime.DaysInMonth(FechaCorteTemp.Year, FechaCorteTemp.Month));
                        var FechaPagoTemp = PlanSimulacion[i - 1].FechaPago.AddMonths(1).AddDays(-PlanSimulacion[i - 1].FechaPago.Day).AddMonths(1);
                        var FechaPago = new DateTime(FechaPagoTemp.Year, FechaPagoTemp.Month, DateTime.DaysInMonth(FechaPagoTemp.Year, FechaPagoTemp.Month));
                        var InteresesCorrientesAPagar = 0M;
                        var CapitalAPagar = 0M;
                        var CapitalPorPagar = 0M;
                        var InteresesCorrientesPorPagar = 0M;
                        var SaldoCapitalReducido = 0M;
                        var SaldoTotalRestante = 0M;
                        var TotalSeguros = 0M; 
                        if (i == plazo)
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            CanonReducido = CapitalAPagar + InteresesCorrientesAPagar + valorSeguroVida + valorSeguroHogar;
                            canonTradicional = IntCte + Capital + valorSeguroHogar + valorSeguroVida;
                            SaldoTotalRestante = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar;
                        }
                        else
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = CanonReducido - InteresesCorrientesAPagar - valorSeguroHogar - valorSeguroVida;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            Capital = canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;

                            
                        }

                        double seguroVidaProrrata = 0;
                        double seguroHogarProrrata = 0;
                        if (i == 1)
                        {
                            if (solicitudGeneralCredito != null)
                            {
                                var fecha = request.FechaDesembolso;

                                if (fecha < new DateTime(1753, 1, 1))
                                {
                                    fecha = DateTime.Now; // o null
                                }

                                await _creditoRepository.ActualizarFechaDesembolsoCredito(
                                    solicitudGeneralCredito.SOC_ID,
                                    fecha);

                                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(solicitudGeneralCredito.SOC_ID);

                                if (datosFactura != null && datosFactura.FAC_NUMERO_FACTURA != null)
                                {
                                    SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(solicitudGeneralCredito.SOC_ID);

                                    DateTime? fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;

                                    DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));

                                    var diasProrrata = (ultimoDia - informacionDesembolso.SID_FECHA_DESEMBOLSO).Days;
                                    seguroVidaProrrata = Math.Round((double)valorSeguroVida * (diasProrrata + 1) / 30, 0);
                                    seguroHogarProrrata = Math.Round((double)valorSeguroHogar / 30, 0) * (diasProrrata + 0);
                                }
                            }
                        }
                        TotalSeguros = valorSeguroHogar + valorSeguroVida;

                        PlanSimulacion.Add(new Leasing108()
                        {
                            NroCuota = i,
                            Cuota = canonTradicional,
                            SegHogar = valorSeguroHogar,
                            SegVida = valorSeguroVida,
                            IntCte = IntCte,
                            Capital = Capital,
                            Saldo = i == 1 ? CapitalReducido : PlanSimulacion[i - 2].DeudaRemanente - PlanSimulacion[i - 1].CapitalAPagar + PlanSimulacion[i - 1].CapitalPorPagar + PlanSimulacion[i - 1].InteresesCorrientesPorPagar,
                            FechaCorte = FechaCorte,
                            FechaPago = FechaPago,
                            CuotaUnidadPagadora = CanonReducido + (decimal)seguroHogarProrrata + (decimal)seguroVidaProrrata,
                            SeguroHogar = valorSeguroHogar+ (decimal)(seguroHogarProrrata),
                            SeguvoVida = valorSeguroVida+ (decimal)seguroVidaProrrata,
                            SeguroPorPagar = 0,
                            InteresesCorrientesAPagar = InteresesCorrientesAPagar,
                            InteresesCorrientesPorPagar = InteresesCorrientesPorPagar,
                            CapitalAPagar = CapitalAPagar,
                            CapitalPorPagar = CapitalPorPagar,
                            SaldoCapitalReducido = SaldoCapitalReducido,
                            DeudaRemanente = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar,
                            AfectacionCuentaIndividual = SaldoTotalRestante,
                            BeneficioAlivioCuota = 0,
                            BeneficioTasaFresh = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = SaldoLeasingTradicional,
                            SegurosTotal = TotalSeguros

                        });
                    }
                }

                
                foreach (Leasing108 item in PlanSimulacion)
                {
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Cuota = item.NroCuota,
                        FechaCorte = item.FechaCorte,
                        FechaPago = item.FechaPago,
                        SaldoLeasing = item.Saldo,
                        CapitalLeasing = item.Capital,
                        InteresLeasing = item.IntCte,
                        SeguroVidaLeasing = item.SeguvoVida,
                        SeguroHogar = (double)item.SeguroHogar,
                        TotalCuota = (double)item.Cuota,
                        CuotaUnidadPagadora = item.CuotaUnidadPagadora,
                        SaldoCapital = item.Capital,
                        CapitalInteresporPagar = item.InteresesCorrientesAPagar,
                        CapitalSaldoTotal = item.DeudaRemanente,
                        SaldoCapitalReducido = item.SaldoCapitalReducido,
                        CuotaLeasing = item.CuotaUnidadPagadora,
                        ValorDeudaRemanente = item.DeudaRemanente,
                        IdSimulacion = simulacionId,
                        IdDetalleSimulacion = Guid.NewGuid(),
                        IdSimulacionCliente = datosPersonales.IdSimulacion,
                        AfectacionCuentaIndividual = item.AfectacionCuentaIndividual,
                        /// nueva extructura
                        InteresPorPagar = item.InteresesCorrientesPorPagar,
                        CapitalPorPagar = item.CapitalPorPagar,
                        CapitalReducido = item.SaldoCapitalReducido,
                        Capitalpagar = item.CapitalAPagar,
                        InteresesPorPagar = item.InteresesCorrientesPorPagar,
                        CapitalSaldo = item.Capital,
                        SaldoTotal = item.Saldo,
                        NoventaCapacidadDePago = (decimal)capacidadPago,
                        ValorMaximoVivienda = (decimal)request.valor,
                        SaldoLeasingTradicional = item.Saldo - item.Capital, 
                        SegurosTotal = Math.Round(item.SegurosTotal, 2), 
                        Afl_id = request.afl_id, 
                        NumeroDocumento = request.documento,
                        ValorFinanciar = item.SaldoLeasingTradicional - (decimal)valorCanonInicial,
                        ViviendaVis = viviendaVis


                    });
                }
                #endregion
            }
            #region Guardado en BD


            planillaSimulacion[0].ValorOpcionCompra = (decimal)valorOpcionDeCompra;
            planillaSimulacion[0].ValorCanonInicial = (decimal)valorCanonInicial;
            planillaSimulacion[0].factor = factor;
            planillaSimulacion[0].CuotaSectorFinanciero = CuotaSectorFinanciero;
            planillaSimulacion[0].CuotaSectorReal = CuotaSectorReal;
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].RiesgoMinimoTransUnion = (Score == 0) ? 0 : Score;
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].AportesPropios = (decimal)request.ValorViviendaSeguro - (decimal)valorViviendaDeseado; 

            if (request.tipoSimularOProspectar == 3)
            {
                await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
                var consultaSMC = await _creditoBusinessLogic.ConsultarSimulacionCliente(request.documento);

                var smc_id = consultaSMC.Data.smc_id;


                ValoresSimulacionAfiliadoDto valoresSimulacion = new ValoresSimulacionAfiliadoDto();
                valoresSimulacion.ValorFinanciar = planillaSimulacion[0].ValorFinanciar;
                valoresSimulacion.NumeroDocumento = request.documento;
                valoresSimulacion.SMS_ID = smc_id;
                valoresSimulacion.RiesgoMinimo = planillaSimulacion[0].RiesgoMinimoTransUnion;
                valoresSimulacion.ValorSectorFinanciero = planillaSimulacion[0].CuotaSectorFinanciero;
                valoresSimulacion.ValorSectorReal = planillaSimulacion[0].CuotaSectorReal;
                valoresSimulacion.Factor = planillaSimulacion[0].factor;

                await _creditoRepository.InsertarValoresSimulacionAfiliado(valoresSimulacion);
            }
            #endregion
            simulacionDto.LstProyeccion = listaInformacionProyeccion;

            List<InformacionProyeccion> proyecciones = new List<InformacionProyeccion>(); 
            foreach (var item in listaInformacionProyeccion)
            {
                proyecciones.Add(item);
            }

            
            planillaSimulacion[0].LstProyeccion = proyecciones;

            var valoresSimulacionProspeccion = new ValoresSimulacionCreditoReclasificacion
            {
                NUMERO_DOCUMENTO = request.documento,
                TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                SEGURO_VIDA = decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                SEGURO_HOGAR = decimal.Parse(planillaSimulacion[0].SeguroHogar.ToString()),
                SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                SEGURO_VIDA_EXTRAPRIMA = 0,
                TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                TASA_MIXTA_EXTRAPRIMA = 0,
                FACTOR = 0,
                FACTOR_EXTRA_PRIMA = 0,
                PLAZO = plazo,
                VALOR_PRESTAMO = planillaSimulacion[0].ValorFinanciar,
                CUOTA_SIN_SEGURO_HOGAR = canonTradicional - decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                CUOTA_SEGURO_HOGAR = canonTradicional,
                CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                CAPACIDAD_PAGO = (decimal)capacidadPago,
                VALOR_PRESTAMO_SEGUN_CUOTA = 0
            };

            bool guardado = await _creditoRepository
                .InsertValoresSimulacionCredito(valoresSimulacionProspeccion);

            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        #endregion

        #region Credito Leasing 24-107
        public class Leasing24
        {
            public int NroCuota { get; set; }
            public DateTime FechaCorte { get; set; }
            public DateTime FechaPago { get; set; }
            public decimal Capital { get; set; }
            public decimal IntCte { get; set; }
            public decimal SegVida { get; set; }
            public decimal SegHogar { get; set; }
            public decimal Cuota { get; set; }
            public decimal Saldo { get; set; }
            public decimal SaldoCapitalReducido { get; set; }
            public decimal CapitalAPagar { get; set; }
            public decimal CapitalPorPagar { get; set; }
            public decimal InteresesCorrientesAPagar { get; set; }
            public decimal InteresesCorrientesPorPagar { get; set; }
            public decimal SeguroHogar { get; set; }
            public decimal SeguvoVida { get; set; }
            public decimal SeguroPorPagar { get; set; }
            public decimal CuotaUnidadPagadora { get; set; }
            public decimal AfectacionCuentaIndividual { get; set; }
            public decimal DeudaRemanente { get; set; }
            public decimal BeneficioTasaFresh { get; set; }
            public decimal BeneficioAlivioCuota { get; set; }
            public decimal ValoraCobrarFonvivienda { get; set; }
            public decimal SaldoLeasingTradicional { get; set; }
            public decimal SegurosTotal { get; set; }
            public decimal CapitalPorPagarLeasing { get; set; }
        }
        public async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionLeasing24(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid simulacionId, bool recalcular = false)
        {
            #region DatosSimulacion
            ValoresProyectadosDto valoresProyectadosDto = new ValoresProyectadosDto();
            List<Leasing24> PlanSimulacion = new();
            List<SimulacionDto> planillaSimulacion = new();
            var equivalenciasSimuladoCredito = (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento)).Data;
            var cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto { TipoIdentificacion = request.tipoIdentificacion, NumeroDocumento = request.documento })).Data;

            var usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

            var correoLogueado = usuarioEncontrado == null ? "" : usuarioEncontrado.USR_USERNAME;


            var tipoActorRecursos = recursos.IdTipoActor;
            var descripcionVinculacion = ""; 
            foreach(var item in recursos.TiposVinculacion)
            {
                if (tipoActorRecursos == item.IdTipoActor)
                {
                    descripcionVinculacion = item.DescripcionTipoActor; 
                }
            }

            var TasaEATipoCredito = await _creditoRepository.ObtenerTasasSimuladorByTipoCredito(descripcionVinculacion,2);


            TasaSimulador tasaSimulador = new TasaSimulador();
            if (tipoActorRecursos == 1) //AFILIADO SOLUCION VIVIENDA
            {
                tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("LEASING AFILIADO 24 - 107");

            }
            else if (tipoActorRecursos == 15) //FUNCIONARIO AFILIADO SOLUCIÓN VIVIENDA
            {
                tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("LEASING FUNCIONARIO 24 - 107");
            }

            var parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
            {
                SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
            });
            if (parametrosGenerales.Status != string.Empty)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = parametrosGenerales.Status
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            var valorInmuebleAlmacenado = request.valor > (double)request.avaluoInmueble ? request.valor : (double)request.avaluoInmueble;

            request.valor = request.valorInicialSimuladoInmueble > 0 ? (double)request.valorInicialSimuladoInmueble : valorInmuebleAlmacenado;


            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;

            var datosSimulacionPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento));
            var tiposVivienda = (await _catalogsBusinessLogic.ObtenerTiposDeVivienda()).Data;
            var proyeccionactual = (await _creditoBusinessLogic.ObtenerProyeccionCliente(cliente.GradoId.ToString())).Data;
            var simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(proyeccionactual.Id.ToString())).Data;
            bool categoriaCivil = false;
            if (parametrosGenerales.SeleccionCargoConSalario)
            {
                if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
                {
                    proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
                    categoriaCivil = true;
                }
                else if (proyeccionactual.Id == 10 || proyeccionactual.Id == 11 || proyeccionactual.Id == 12)
                {
                    double cuotasAfiliado = (double)recursos.NumeroCuotas / 12;

                    int cuotas = (int)Math.Round(cuotasAfiliado, 0, MidpointRounding.AwayFromZero);

                    var cargo = simcargo
                                .Where(x => x.salariobasico <= datosFinancieros.ValorSalario)
                                .OrderByDescending(x => x.salariobasico)
                                .FirstOrDefault()
                                ??
                                simcargo
                                    .Where(x => x.salariobasico > datosFinancieros.ValorSalario)
                                    .OrderBy(x => x.salariobasico)
                                    .FirstOrDefault();

                    if (cuotas < 7)
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 10;
                    }
                    else if (cuotas >= 7 && cuotas < 9)
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 11;
                    }
                    else
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 12;
                    }
                }
                else
                {
                    var cargo = simcargo
                                .Where(x => x.salariobasico <= datosFinancieros.ValorSalario)
                                .OrderByDescending(x => x.salariobasico)
                                .FirstOrDefault()
                                ??
                                simcargo
                                    .Where(x => x.salariobasico > datosFinancieros.ValorSalario)
                                    .OrderBy(x => x.salariobasico)
                                    .FirstOrDefault();

                    proyeccionactual.salariobasico = cargo.salariobasico;
                    proyeccionactual.Id = cargo.Id;

                }

            }
            else if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
            {
                proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
                categoriaCivil = true;
            }


            int tipoAfiliacionId = int.Parse(cliente.TipoAfiliacion.ToString());
            var viviendaVis = (request.valor / (int)parametrosGenerales.SalarioMinimo) < (double)parametrosGenerales.LimiteVIS ? "VIS" : "NO VIS";
            #endregion
            #region Paso1
            var valorViviendaDeseado = request.valor; 
            //Este Valor se llena en el paso 7
            var valorViviendaMaximo = 0M;
            #endregion
            #region Paso3

            if (recursos.EstadoCliente != "VIGENTE")
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "No es viable, afiliado con estado " + recursos.EstadoCliente
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            if (recursos.NumeroCuotas <= 12 && Int32.Parse(cliente.TipoAfiliacion) != (int)EnumAfiliadoID.FUNCIONARIO_AFILIADO_ADMINISTRACION_CESANTÍAS && Int32.Parse(cliente.TipoAfiliacion) != (int)EnumAfiliadoID.AFILIADO_ADMINISTRACION_CESANTÍAS)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "NO CUMPLE CON LAS CUOTAS APORTADAS\r\n\r\nSe requieren mínimo 12 cuotas aportadas"
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

            var tasaEfectivaAnualMensualizada = Math.Pow((double)(parametrosGenerales.Tasas.EfectivaAnual + 1), (1.0 / 12))-1; 
            var porcentajeCanonInicial = 0.0;
            var porcentajeOpcionCompra = 0.0;
            var tasaCredito = parametrosGenerales.Tasas.EfectivaAnual;
            decimal tasaPeriodicaNMV = (decimal)tasaEfectivaAnualMensualizada;
            var tasaSeguroVida = parametrosGenerales.Tasas.SeguroVida;
            var tasaSeguroHogar = parametrosGenerales.Tasas.SeguroHogar;
            
            var datoSeguroSimulacion = await _creditoRepository.ConsultarSeguroSimulacionCredito(request.documento);

            if (datoSeguroSimulacion.SSC_CONVENIO_VIDA == "N")
            {
                tasaSeguroVida = 0; 
            }

            if (datoSeguroSimulacion.SSC_CONVENIO_HOGAR == "N")
            {
                tasaSeguroHogar = 0;
            }

            if (request.CanonInicial >= parametrosGenerales.Tasas.Min_Canon_Inicial && request.CanonInicial <= parametrosGenerales.Tasas.Max_Canon_Inicial)//Revisar donde se puede obtener para no dejarlo quemado
            {
                porcentajeCanonInicial = (double)(request.CanonInicial / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "CANON INICIAL FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser entre el 5% y el 15%
            }
            if (request.porOpcionCompra <= parametrosGenerales.Tasas.Max_Opcion_Compra)
            {
                porcentajeOpcionCompra = (double)(request.porOpcionCompra / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser menor a 30%
            }

            var plazo = 0;
            var obtenerInfoCliente = await _requestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento);


            if (request.plazo != 0 && (request.plazo >= parametrosGenerales.MinimoPlazoMeses && request.plazo <= parametrosGenerales.MaximoPlazoMeses))
            {
                plazo = request.plazo;
            }
            else
            {
                plazo = 168 - obtenerInfoCliente.Data.NumeroCuotas /*parametrosGenerales.MaximoPlazoMeses*/;
            }

            double numeroCuotasCalculado = (double)Math.Round((decimal)recursos.NumeroCuotas / 12, 20);

            plazo = (int)Math.Round((Math.Round(numeroCuotasCalculado - 14, 10) * 12) * (-1));

            var AnosPlazo = plazo % 12 == 0 ? (int)(plazo / 12) : (int)(plazo / 12) + 1;
            #endregion
            #region Paso4
            var factor = (decimal)(tasaPeriodicaNMV * (decimal)Math.Pow((1 + (double)tasaPeriodicaNMV), plazo) / (decimal)(Math.Pow(1 + (double)tasaPeriodicaNMV, plazo) - 1));
            var valorCanonInicial = request.valor * porcentajeCanonInicial;
            #endregion
            #region Paso5
            

            decimal ValorViviendaSeg = 0; 
            if (request.avaluoInmueble > 0)
            {
                ValorViviendaSeg = (decimal)request.avaluoInmueble > (decimal)valorInmuebleAlmacenado ? (decimal)request.avaluoInmueble : (decimal)valorInmuebleAlmacenado;
            }
            else
            {
                ValorViviendaSeg = request.ValorViviendaSeguro > 0 ? (decimal)request.ValorViviendaSeguro : (decimal)valorInmuebleAlmacenado;
            }

            var valorSeguroHogar = recalcular ? (decimal)request.ValorViviendaSeguro * tasaSeguroHogar : (decimal)ValorViviendaSeg * tasaSeguroHogar;
      
            #region Ahorros y Cesantias

            await _creditoRepository.LimpiarRecursosCongelados(simulacionId, request.documento);

            decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;
            decimal RecursosCuentaIndiv = ahorroProgramado + cesantiasProgramadas + intCesantiasProgramadas + intAhorroProgramados;
            var MaximoUsar = RecursosCuentaIndiv * (decimal)(1 - 0.15);

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.documento, 
                    CGR_TIPO_CREDITO = "LEASING 24-107"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 24-107"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 24-107"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING 24-107"
                }
            };

            

            foreach (var recurso in recursosCongelados)
            {
                await _creditoRepository.InsertarRecursosCongelados(recurso);
            }


            await _creditoRepository.LimpiarConceptosCongelados(simulacionId, request.documento);


            List<string> descripcionesConceptos = new List<string>
            {
                "AHORROS OBLIGATORIOS",
                "AHORROS RETROACTIVOS",
                "AHORROS VOLUNTARIOS",
                "INTERESES APORTES",
                "CESANTÍAS",
                "CESANTÍAS RETROACTIVOS",
                "INTERESES CESANTÍAS"
            };

            List<ConceptosCongeladosProspeccion> listaCongeladosProspeccion = new List<ConceptosCongeladosProspeccion>();

            foreach (var cuentas in recursos.Cuentas)
            {
                foreach (var conceptos in cuentas.Conceptos)
                {
                    if (descripcionesConceptos.Contains(conceptos.ConceptoDescripcion))
                    {
                        listaCongeladosProspeccion.Add(new ConceptosCongeladosProspeccion
                        {
                            CON_DESCRIPCION = conceptos.ConceptoDescripcion,
                            CON_VALOR = conceptos.saldo,
                            SLS_ID = simulacionId,
                            CON_NUMERO_DOCUMENTO = request.documento
                        });
                    }
                }
            }



            foreach (var concepto in listaCongeladosProspeccion)
            {
                await _creditoRepository.InsertarConceptosCongelados(concepto);
            }


            #endregion
            var RecursosCI = 0M;
            if (MaximoUsar > (decimal)valorCanonInicial)
            {
                RecursosCI = (decimal)valorCanonInicial;
            }
            else
            {
                RecursosCI = RecursosCuentaIndiv;
            }

            decimal RecursosPropios = (decimal)valorCanonInicial - RecursosCI;
            #region RecursosProyectados
            var antiguedadcalculo = Math.Round(((decimal)obtenerInfoCliente.Data.NumeroCuotas / 12), 1);
            var antiguedadaños = 0; 
            if (parametrosGenerales.RangoAscenso)
            {
                antiguedadaños = (int)Math.Round(antiguedadcalculo);
            }
            else
            {
                antiguedadaños = (int)Math.Floor(antiguedadcalculo);

            }
            #region Factores
            decimal factorIncremento = parametrosGenerales.Factores.FactorIncremento / 100;
            decimal factorIncrementoSM = parametrosGenerales.Factores.FactorIncrementoSubsidio / 100;
            decimal factorAhorro = request.TasaVoluntariaAportes ? (parametrosGenerales.Factores.FactorAhorro + cliente.TasaVoluntariaAportes) / 100 : parametrosGenerales.Factores.FactorAhorro / 100;
            decimal factorInflacion = parametrosGenerales.Factores.FactorInflacion / 100;
            decimal factorCesantias = parametrosGenerales.Factores.FactorCesantias / 100;
            decimal factorInteresesCesantias = 12.0M / 100;
            #endregion
            #region Valores Para Proyeccion
            #region Uso Canon Inicial
            var valorIntCesantiasCanonInicial = intCesantiasProgramadas < (decimal)valorCanonInicial ? intCesantiasProgramadas : (decimal)valorCanonInicial;
            var valorIntAhorroCanonInicial = intAhorroProgramados < ((decimal)valorCanonInicial - intCesantiasProgramadas) ? intAhorroProgramados : ((decimal)valorCanonInicial - intCesantiasProgramadas);
            var valorCesantiasCanonInicial = cesantiasProgramadas < ((decimal)valorCanonInicial - intAhorroProgramados - intCesantiasProgramadas) ? cesantiasProgramadas : ((decimal)valorCanonInicial - intAhorroProgramados - intCesantiasProgramadas);
            var valorAhorroCanonInicial = ahorroProgramado < ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial)) ? ahorroProgramado : ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial));

            decimal valorRecursosAcumuladosCanonInicial = valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial + valorAhorroCanonInicial;

            if ((double)valorRecursosAcumuladosCanonInicial < valorCanonInicial)
            {
                valorCanonInicial =  (double)valorRecursosAcumuladosCanonInicial; 
            }

            var VrCredito = 0.0;
            var VrCreditoSegVida = 0.0;

            if (request.valorPrestamo > 0 && request.valorPrestamo < (valorViviendaDeseado - valorCanonInicial))
            {
                valorCanonInicial = valorViviendaDeseado - request.valorPrestamo;
                VrCredito = request.valorPrestamo;
            }
            else
            {
                VrCredito = (double)(valorViviendaDeseado - valorCanonInicial);
            }

            VrCreditoSegVida = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

            var valorSeguroVida = (decimal)VrCredito * tasaSeguroVida;


            var VrCuotaSinOpcC = recalcular ? ((decimal)(request.valorPrestamo - valorCanonInicial) * factor) + valorSeguroHogar + valorSeguroVida : ((decimal)(valorViviendaDeseado - valorCanonInicial) * factor) + valorSeguroHogar + valorSeguroVida;
            var valorOpcionDeCompra = (decimal)(valorViviendaDeseado * porcentajeOpcionCompra);

            var VrDescOpcCompra = (valorOpcionDeCompra / (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), plazo)) * factor;
            var canonTradicional = VrCuotaSinOpcC - VrDescOpcCompra;
            if (valorOpcionDeCompra > (decimal)VrCredito && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA MAYOR AL VALOR DEL CREDITO",
                    RespuestaRadicado = false
                    
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

            #endregion
            #region Actualizacion Ahorros y Cesantias Para Proyeccion
            ahorroProgramado = ahorroProgramado - valorAhorroCanonInicial;
            cesantiasProgramadas = cesantiasProgramadas - valorCesantiasCanonInicial;
            intCesantiasProgramadas = intCesantiasProgramadas - valorIntCesantiasCanonInicial;
            intAhorroProgramados = intAhorroProgramados - valorIntAhorroCanonInicial;
            #endregion
            #region Proyeccion
            //Ejercicio valor i del for en 0

            bool reiniciarAnioCargo = false;
            int CountAnioRegimen = int.Parse(datosPersonales.Regimen) - DateTime.Now.Year;

            decimal sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
            decimal SMProyectado = (decimal)parametrosGenerales.SalarioMinimo;
            decimal AhorrosAnt = ahorroProgramado;
            decimal InteresAhorroAnterio = 0;
            double sueldoProyeccion = 0;
            decimal AcumuladoAhorros = ahorroProgramado;
            decimal IntAhorrosAnt = intAhorroProgramados;
            decimal AcumuladoIntAhorros = intAhorroProgramados;
            decimal CesantiasAnt = cesantiasProgramadas;
            decimal AcumuladoCesantias = cesantiasProgramadas;
            decimal IntCesantiasAnt = intCesantiasProgramadas;
            decimal AcumuladoIntCesantias = intCesantiasProgramadas;
            decimal TOTAcumuladoAhorros = 0M;
            decimal TOTAcumuladoCesantias = 0M;
            decimal intAhorroCalculado = 0;
            var countAnios = 1;
            var countCategoria = 0;
            var ctg = proyeccionactual.Id;
            var antiguedadañosTemp = antiguedadaños;
            string StInfoProy = string.Empty;
            bool aniosAntiguedadMenorAscenso = false;
            int diferenciaAnios = 0;
            int aniosCargoProyecion = 0; 
            List<string> LstProyeccion = new List<string>();
            int aniosAntiguedadUltimaCategoria = 0; 

            List<InformacionProyeccion> listaInformacionProyeccion = new List<InformacionProyeccion>();

            InformacionProyeccion informacionProyeccion = new InformacionProyeccion();

            if (parametrosGenerales.RangoAscenso)
            {
                 reiniciarAnioCargo = false;
                 CountAnioRegimen = int.Parse(datosPersonales.Regimen) - DateTime.Now.Year;

                 sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                 SMProyectado = (decimal)parametrosGenerales.SalarioMinimo;
                 AhorrosAnt = ahorroProgramado;
                 InteresAhorroAnterio = 0;
                 sueldoProyeccion = 0;
                 AcumuladoAhorros = ahorroProgramado;
                 IntAhorrosAnt = intAhorroProgramados;
                 AcumuladoIntAhorros = intAhorroProgramados;
                 CesantiasAnt = cesantiasProgramadas;
                 AcumuladoCesantias = cesantiasProgramadas;
                 IntCesantiasAnt = intCesantiasProgramadas;
                 AcumuladoIntCesantias = intCesantiasProgramadas;
                 TOTAcumuladoAhorros = 0M;
                 TOTAcumuladoCesantias = 0M;
                 intAhorroCalculado = 0;
                 countAnios = 1;
                 countCategoria = 0;
                 ctg = proyeccionactual.Id;
                 antiguedadañosTemp = antiguedadaños;
                 StInfoProy = string.Empty;

                
                StInfoProy = "Años Antiguedad: " + antiguedadaños;

                

                informacionProyeccion.IdProyeccion = 0;
                informacionProyeccion.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
                informacionProyeccion.AniosCargo = antiguedadaños == 0 ? countAnios : antiguedadaños;
                informacionProyeccion.Sueldo = (double)sueldo;
                informacionProyeccion.AhorroProgramado = ahorroProgramado;
                informacionProyeccion.InteresesAhorroProgramado = intAhorroProgramados;
                informacionProyeccion.Cesantias = cesantiasProgramadas;
                informacionProyeccion.InteresesCesantias = intCesantiasProgramadas;
                informacionProyeccion.AniosAntiguedad = antiguedadaños;
                aniosCargoProyecion = antiguedadaños == 0 ? countAnios : antiguedadaños;
                aniosAntiguedadUltimaCategoria = informacionProyeccion.AniosAntiguedad;
                listaInformacionProyeccion.Add(informacionProyeccion);

                if (antiguedadaños > simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.AñoAscenso).FirstOrDefault())
                {
                    antiguedadaños = antiguedadaños - simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.AñoAscenso).FirstOrDefault();
                    ctg = proyeccionactual.Id++;
                    reiniciarAnioCargo = true;
                }

                if (antiguedadañosTemp > 0)
                {
                    countAnios = antiguedadañosTemp;
                }

                if (reiniciarAnioCargo)
                {
                    countAnios = 0;
                }

                diferenciaAnios = ctg != 21 && ctg != 6 && ctg != 30 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.AñoAscenso).FirstOrDefault() - antiguedadaños : antiguedadaños;

                aniosAntiguedadMenorAscenso = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.AñoAscenso).FirstOrDefault() >= antiguedadaños ? true : false;


                while (antiguedadañosTemp >= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault() && ctg!= 21)
                {
                    countCategoria++;
                    antiguedadañosTemp -= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
                    if (simcargo.Any(x => x.Id == ctg + 1))
                    {
                        ctg = proyeccionactual.Id;
                        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
                        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                    }
                }

                if (simcargo.Where(x => x.Id == ctg).Select(x => x.AñoAscenso).FirstOrDefault() == countAnios && ctg != 21 && ctg!=6 && ctg != 30)
                {
                    proyeccionactual.Id = proyeccionactual.Id + 1;
                    sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                    countAnios = 0;
                    ctg++;
                    antiguedadaños = 0;
                }
            }
            else
            {
                //Ejercicio valor i del for en 0
                reiniciarAnioCargo = false;
                CountAnioRegimen = int.Parse(datosPersonales.Regimen) - DateTime.Now.Year;
                
                sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                SMProyectado = (decimal)parametrosGenerales.SalarioMinimo;
                AhorrosAnt = ahorroProgramado;
                sueldoProyeccion = 0;
                AcumuladoAhorros = ahorroProgramado;
                IntAhorrosAnt = intAhorroProgramados;
                AcumuladoIntAhorros = intAhorroProgramados;
                CesantiasAnt = cesantiasProgramadas;
                AcumuladoCesantias = cesantiasProgramadas;
                IntCesantiasAnt = intCesantiasProgramadas;
                AcumuladoIntCesantias = intCesantiasProgramadas;
                TOTAcumuladoAhorros = 0M;
                TOTAcumuladoCesantias = 0M;
                countAnios = 1;
                countCategoria = 0;
                ctg = proyeccionactual.Id;
                antiguedadañosTemp = antiguedadaños;
                StInfoProy = string.Empty;

                StInfoProy = "Años Antiguedad: " + antiguedadaños;

     

                informacionProyeccion.IdProyeccion = 0;
                informacionProyeccion.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
                informacionProyeccion.AniosCargo = antiguedadaños == 0 ? countAnios : (int)Math.Round(antiguedadcalculo);
                informacionProyeccion.Sueldo = (double)sueldo;
                informacionProyeccion.AhorroProgramado = ahorroProgramado;
                informacionProyeccion.InteresesAhorroProgramado = intAhorroProgramados;
                informacionProyeccion.Cesantias = cesantiasProgramadas;
                informacionProyeccion.InteresesCesantias = intCesantiasProgramadas;
                informacionProyeccion.AniosAntiguedad = (int)Math.Round(antiguedadcalculo);

                listaInformacionProyeccion.Add(informacionProyeccion);


                if (antiguedadaños > simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.AñoAscenso).FirstOrDefault())
                {
                  
                    antiguedadaños = 1;
                    reiniciarAnioCargo = true;
                }

                if (antiguedadañosTemp > 0)
                {
                    countAnios = antiguedadañosTemp;
                }

                if (reiniciarAnioCargo)
                {
                    countAnios = 0;
                }

                while (antiguedadañosTemp >= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault())
                {
                    countCategoria++;
                    antiguedadañosTemp -= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
                    if (simcargo.Any(x => x.Id == ctg + 1))
                    {
                        ctg = proyeccionactual.Id + countCategoria;
                        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
                        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                    }
                }
            }
            decimal totalAhorradoAnual = 0;
            decimal AcumuladoIntAhorrosDos = 0;
            decimal totalAhorroProgramado = 0;
            decimal totalCesantiasProgramadas = 0;
            decimal interesCesantiasAnual = 0;
            decimal acumuladoIntCesantiasDos = 0;
            int aumentoCategoria = 1;
            int aniosAntiguedadProyec = (int)antiguedadcalculo++;
           
            for (int i = 1; i <= CountAnioRegimen; i++)
            {


                countAnios++;

                var aumentosueldo = Math.Pow((1 + (double)factorIncremento), i);
                sueldoProyeccion = Math.Round((double)sueldo * aumentosueldo, 2);

                if (parametrosGenerales.AhorroIntereses)
                {
                    decimal Ahorro = Math.Round((decimal)sueldoProyeccion * factorAhorro * 12, 2);

                    if (i < 3)
                    {
                        AcumuladoAhorros += Ahorro;

                        AcumuladoIntAhorrosDos += i > 1 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;


                        AcumuladoIntAhorros = i > 1 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;


                        totalAhorradoAnual = i > 1 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;

                        AhorrosAnt = Ahorro;
                    }
                    else
                    {
                        AcumuladoAhorros += Ahorro;

                        AcumuladoIntAhorrosDos += i > 1 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;


                        AcumuladoIntAhorros = i > 1 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;


                        totalAhorradoAnual = i > 1 ? Math.Round((AhorrosAnt + AcumuladoIntAhorros) * factorInflacion, 2) : 0;

                        AhorrosAnt = Ahorro;
                    }
                }
                else
                {
                    decimal Ahorro = Math.Round((decimal)sueldoProyeccion * factorAhorro * 12, 0, MidpointRounding.AwayFromZero);

                    //AHORRO DEL AÑO ANTERIOR + INTERES DEL AÑO ANTERIOR ) * INFLACION
                    AcumuladoAhorros += Ahorro;
                    AcumuladoIntAhorros += i > 0 ? Math.Round(AhorrosAnt * factorInflacion, 0, MidpointRounding.AwayFromZero) : 0;
                    AhorrosAnt = Ahorro;
                    
                }



                if (parametrosGenerales.CesantiasIntereses)
                {
                    decimal Cesantias = Math.Round((decimal)sueldoProyeccion * factorCesantias * 12, 2);
                    AcumuladoCesantias += Cesantias;
                    AcumuladoIntCesantias = i > 1 ? Math.Round((CesantiasAnt) * factorInflacion, 2) : 0;

                    acumuladoIntCesantiasDos += i > 1 ? Math.Round((CesantiasAnt) * factorInflacion, 2) : 0;
                    interesCesantiasAnual = i > 1 ? Math.Round(CesantiasAnt * factorInflacion, 2) : 0;


                    CesantiasAnt = Cesantias;



                }
                else
                {
                    decimal Cesantias = Math.Round((decimal)sueldoProyeccion * factorCesantias * 12, 0, MidpointRounding.AwayFromZero);
                    AcumuladoCesantias += Cesantias;
                    AcumuladoIntCesantias += i > 1 ? Math.Round(CesantiasAnt * factorInflacion, 0, MidpointRounding.AwayFromZero) : 0;
                    CesantiasAnt = Cesantias;
                }

                if (parametrosGenerales.RangoAscenso)
                {
                    SMProyectado = simcargo.Where(x => x.Id == ctg).Select(x => x.ProyectaSubsidio).FirstOrDefault() ? SMProyectado + (SMProyectado * factorIncrementoSM) : parametrosGenerales.SalarioMinimo;


                    if (antiguedadaños > 0 && i == 1 && !aniosAntiguedadMenorAscenso)
                    {
                        countAnios = diferenciaAnios;
                        aniosCargoProyecion++;
                    }
                    if (antiguedadaños > 0 && i == 1 && aniosAntiguedadMenorAscenso)
                    {
                        aniosCargoProyecion++;
                        countAnios = antiguedadaños + 1;
                    }

                    aniosAntiguedadUltimaCategoria++; 

                    InformacionProyeccion informacionProyeccionCredito = new InformacionProyeccion();
                    informacionProyeccionCredito.IdProyeccion = i;
                    informacionProyeccionCredito.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
                    informacionProyeccionCredito.AniosCargo = countAnios;
                    informacionProyeccionCredito.Sueldo = sueldoProyeccion;
                    informacionProyeccionCredito.AhorroProgramado = i > 1 ? AcumuladoAhorros + AcumuladoIntAhorrosDos : AcumuladoAhorros;
                    informacionProyeccionCredito.InteresesAhorroProgramado = i > 1 ? AcumuladoIntAhorros : 0;
                    informacionProyeccionCredito.Cesantias = i > 1 ? AcumuladoCesantias + acumuladoIntCesantiasDos : AcumuladoCesantias;
                    informacionProyeccionCredito.InteresesCesantias = i > 1 ? AcumuladoIntCesantias : 0;
                    informacionProyeccionCredito.AniosAntiguedad = aniosAntiguedadUltimaCategoria;

                    totalAhorroProgramado = informacionProyeccionCredito.AhorroProgramado;
                    totalCesantiasProgramadas = informacionProyeccionCredito.Cesantias;

                    listaInformacionProyeccion.Add(informacionProyeccionCredito);



                    if (simcargo.Where(x => x.Id == ctg).Select(x => x.AñoAscenso).FirstOrDefault() <= countAnios && ctg!=21)
                    {
                        if (simcargo.Any(x => x.Id == ctg + 1))
                        {

                            //si es igual a estas categorias no tiene porque subir o ascender de categoria. 
                            //7   Civil Profesional
                            //8   Civil Tecnico
                            //9   Civil Asistencial
                            if (ctg != 7 && ctg != 8 && ctg != 9)
                            {

                                ctg = proyeccionactual.Id + (aumentoCategoria++);
                            }
                            simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
                            sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                            countAnios = 0;
                        }
                    }
                }
                else
                {
                    SMProyectado = simcargo.Where(x => x.Id == ctg).Select(x => x.ProyectaSubsidio).FirstOrDefault() ? SMProyectado + (SMProyectado * factorIncrementoSM) : parametrosGenerales.SalarioMinimo;


                    InformacionProyeccion informacionProyeccionCredito = new InformacionProyeccion();

                    informacionProyeccionCredito.IdProyeccion = i;
                    informacionProyeccionCredito.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
                    informacionProyeccionCredito.AniosCargo = countAnios;
                    informacionProyeccionCredito.Sueldo = sueldoProyeccion;
                    informacionProyeccionCredito.AhorroProgramado = AcumuladoAhorros;
                    informacionProyeccionCredito.InteresesAhorroProgramado = AcumuladoIntAhorros;
                    informacionProyeccionCredito.Cesantias = AcumuladoCesantias;
                    informacionProyeccionCredito.InteresesCesantias = AcumuladoIntCesantias;
                    informacionProyeccionCredito.AniosAntiguedad = (int)Math.Round(antiguedadcalculo++);

                    listaInformacionProyeccion.Add(informacionProyeccionCredito);

                    if (simcargo.Where(x => x.Id == ctg).Select(x => x.AñoAscenso).FirstOrDefault() == countAnios)
                    {
                        countCategoria++;
                        if (simcargo.Any(x => x.Id == ctg + 1))
                        {
                            //si es igual a estas categorias no tiene porque subir o ascender de categoria. 
                            //7   Civil Profesional
                            //8   Civil Tecnico
                            //9   Civil Asistencial
                            if (ctg != 7 && ctg != 8 && ctg != 9)
                            {
                                ctg = proyeccionactual.Id + countCategoria;
                            }
                            simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
                            sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
                            countAnios = 0;
                        }
                    }
                }
            }

                
            

         
            #endregion
            #region Valores Proyectados

            decimal subsidioCategoria = (decimal)(await _catalogsBusinessLogic.ObtenerSimulacionCargoPorCategoria(cliente.CategoriaId, ctg)).Data.FactorSubsidio;
            decimal subsidiovivienda = Math.Round((SMProyectado * subsidioCategoria), 2);
            TOTAcumuladoAhorros = AcumuladoAhorros + AcumuladoIntAhorros;
            TOTAcumuladoCesantias = AcumuladoCesantias + AcumuladoIntAhorrosDos;
            var RecursosCanonInicial = valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial + valorAhorroCanonInicial;
            var TotalOpcionCompra = (decimal)subsidiovivienda + totalAhorroProgramado + totalCesantiasProgramadas;
            var DivCuoEquiVF = Math.Pow((double)(1 + tasaPeriodicaNMV), plazo);
            var CuotaEquivalenteVF = (TotalOpcionCompra * factor) / (decimal)DivCuoEquiVF;
            var CanonReducido = VrCuotaSinOpcC - CuotaEquivalenteVF;
            var CapitalReducido = (CanonReducido - valorSeguroHogar - valorSeguroVida) / factor;
            #region Valores a Guardar
            valoresProyectadosDto.Svp_Id = Guid.NewGuid();
            valoresProyectadosDto.Sls_Id = datosPersonales.IdSimulacion;
            valoresProyectadosDto.Svp_EA = parametrosGenerales.Tasas.EfectivaAnual;
            valoresProyectadosDto.Svp_Cesantias = totalCesantiasProgramadas;
            valoresProyectadosDto.Svp_Sueldo = (decimal)sueldoProyeccion;
            valoresProyectadosDto.Svp_Ahorros = totalAhorroProgramado;
            valoresProyectadosDto.Svp_Subsidio =
                Math.Round(subsidiovivienda,
                    0,
                    MidpointRounding.AwayFromZero);
            var ValoresProyectados = await _creditoBusinessLogic.GuardarValoresProyectados(valoresProyectadosDto);
            #endregion
            #endregion
            #endregion
            #endregion
            #endregion
            #region Paso6
            var totalIngresos =  (double)datosFinancieros.TotalIngresos;
            var totalEgresos =  (double)datosFinancieros.TotalEgresos;
            var capacidadPago = (totalIngresos * 0.5) - totalEgresos; 
            #endregion
            #region Paso7
            var Valor1 = (decimal)(valorViviendaDeseado * (1 - porcentajeCanonInicial - porcentajeOpcionCompra));
            var Valor2 = ((decimal)capacidadPago + CuotaEquivalenteVF) / (factor + (tasaSeguroHogar / (1 / (decimal)porcentajeCanonInicial)));
            var VrCredito2 = Math.Min(Valor1, Valor2);
            valorViviendaMaximo = VrCredito2 / (1 - (decimal)(porcentajeCanonInicial + porcentajeOpcionCompra));
            //if (Math.Round(valorViviendaMaximo, 0, MidpointRounding.AwayFromZero) < Math.Round((decimal)valorViviendaDeseado, 0, MidpointRounding.AwayFromZero))
            //{
            //    request.valor = (double?)valorViviendaMaximo;
            //    return await CrearSimulacionLeasing24(request, datosPersonales, simulacionId);
            //}
            #endregion

            int vinculacion = recursos.IdTipoActor;
            string estadoCuenta = "";

            if (vinculacion != 14)
            {


                //vinculacion 16 y 2 pertecenten a FUNCIONARIO AFILIADO ADMINISTRACION CESANTÍAS y AFILIADO ADMINISTRACION CESANTIAS respectivamente
                foreach (var cuentas in recursos.Cuentas)
                {
                    if (vinculacion == 16 || vinculacion == 2)
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;

                        }
                    }
                    else
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;

                        }
                    }
                }

                if (estadoCuenta == "MOROSA")
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "El afiliado presenta cuenta con estado MOROSA"
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }
            ValidacionCreditoProspeccion validacionesExcepciones = new ValidacionCreditoProspeccion();

            if (validacionesExcepciones == null)
            {
                validacionesExcepciones.VCP_ESTADO = false;
                validacionesExcepciones.VCP_NUMERO_DOCUMENTO = request.documento;

                await _creditoRepository.InsertarActualizarValidacionCreditoProspeccion(validacionesExcepciones);
            }

            ValidacionCreditoProspeccion validacionesExcepcion = await _creditoRepository.ConsultarValidacionCreditoProspeccion(request.documento);
            double totalObligaciones = 0;
            string validacionEndeudamientoGlobal = ""; 

            if ((request.valor - valorCanonInicial) < (double)subsidiovivienda && !recalcular)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "El valor a financiar no puede ser menor al subsidio de vivienda proyectado",
                    RespuestaRadicado = false
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };

            }

            if ((request.valor - valorCanonInicial) < (double)subsidiovivienda && recalcular)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "No se encontró un valor a financiar que sea superior al subsidio de vivienda proyectado",
                    RespuestaRadicado = false
                });
                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };

            }
            #region EndeudamientoGlobal
            if (parametrosGenerales.ActiveTU && request.tipoSimularOProspectar == 3)
            {
                try
                {
                    #region ConsumoTU
                    ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                    {
                        Apellidos = recursos.PrimerApellido,
                        FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                        Identificacion = request.documento,
                        SalarioMensual = Math.Round((decimal)totalIngresos),
                        TipoIdentificacion = request.tipoIdentificacion,
                        Telefono = recursos.Telefono,
                        TraerHistorialCrediticio = "Y",
                        SLS_ID = simulacionId
                    };

                    if (prospeccionTransUnionRequestDto.Telefono.Length < 10 || prospeccionTransUnionRequestDto.Telefono == null)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el teléfono"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    if (prospeccionTransUnionRequestDto.SalarioMensual <= 0)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el salario"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    var ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                    #endregion
                    ReporteSimulacion60PorCientoDto reporteSimulacion60PorCientoDto = new ReporteSimulacion60PorCientoDto();
                    reporteSimulacion60PorCientoDto.NumeroDocumento = request.documento; 
                    double TOT = totalIngresos - totalEgresos;
                    #region Reporte 60%
                    reporteSimulacion60PorCientoDto.salarioTotal = String.Format("{0:N0}", totalIngresos);
                    reporteSimulacion60PorCientoDto.deducciones = String.Format("{0:N0}", totalEgresos);
                    reporteSimulacion60PorCientoDto.salarioNeto = String.Format("{0:N0}", TOT);
                    reporteSimulacion60PorCientoDto.capacidadPago = String.Format("{0:N0}", capacidadPago);
                    reporteSimulacion60PorCientoDto.canonCuotaMensual = String.Format("{0:N0}", CanonReducido);
                    #endregion
                    if (!string.IsNullOrEmpty(ProspeccionTransUnion.Data.response))
                    {
                        var ReporteTrans = await _reportesBusinessLogic.ReporteTransUnionPropiedades(ProspeccionTransUnion.Data.response);
                        #region Validacion Score y Viabilidad
                        if (!ReporteTrans.IsSuccess)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no viable por consulta TransUnion."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                       

                        #endregion
                        #region GuardadoReporteTransUnion
                        using (var memoryStream = new MemoryStream(ReporteTrans.Data.ArchivoBytes))
                        {
                            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            await SaveReportTU(formFile, simulacionId);
                        }
                        #endregion
                        #region Reclasificacion
                        IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, request.documento);
                        var sumaSaldoObligacion = prospeccionResult
                        .Where(linea => linea.OLN_ESTADO_RECALCULAR == false)
                        .Sum(linea => linea.OLN_SALDO_OBLIGACION);
                        #endregion
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.cuotaSectorFinanSoliAse = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorFinanciero);
                        reporteSimulacion60PorCientoDto.cuotaSectorReal = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorReal);
                        #endregion
                        var EndeudamientoG_Sin_Cuota = ReporteTrans.Data.cuotasSectorReal + ReporteTrans.Data.cuotasSectorFinanciero - (double)sumaSaldoObligacion;
                        totalObligaciones = EndeudamientoG_Sin_Cuota; 
                        var EndeudamientoG_Sin_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoG_Sin_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        var EndeudamientoActual_Cuota = (double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos - (double)sumaSaldoObligacion;
                        var EndeudamientoActual_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoActual_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        Score = ReporteTrans.Data.riesgoMinimo;
                        CuotaSectorFinanciero = (decimal)ReporteTrans.Data.cuotasSectorFinanciero;
                        CuotaSectorReal = (decimal)ReporteTrans.Data.cuotasSectorReal; 

                        EndeudamientoSCH = EndeudamientoG_Sin_Cuota_Porc;
                        EndeudamientoCCH = EndeudamientoActual_Cuota_Porc;

                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.totalObliAntesCredito = String.Format("{0:N0}", EndeudamientoG_Sin_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalSinCanon = (EndeudamientoG_Sin_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.totalObliNuevoCredito = EndeudamientoActual_Cuota.ToString();
                        reporteSimulacion60PorCientoDto.EndeudaGlobalCanon = (EndeudamientoActual_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.disponibleLibranza = String.Format("{0:N0}", (capacidadPago - (double)CanonReducido));
                        reporteSimulacion60PorCientoDto.controlCapacidadPago = capacidadPago >= (double)CanonReducido ? "SI APLICA" : "NO APLICA";
                        var ControNivelEndeudamiento_Cuota_Porc = EndeudamientoActual_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";
                        var ControNivelEndeudamiento_Sin_Cuota_Porc = EndeudamientoG_Sin_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";

                        validacionEndeudamientoGlobal = ControNivelEndeudamiento_Cuota_Porc; 



                        DatosEndeudamientoProspeccionDto datosEndeudamientoProspeccion = new DatosEndeudamientoProspeccionDto();
                        datosEndeudamientoProspeccion.NumeroDocumento = datosPersonales.NumeroDocumento;
                        datosEndeudamientoProspeccion.SalarioUltimoMes = (decimal)totalIngresos;
                        datosEndeudamientoProspeccion.Descuentos = (decimal)totalEgresos;
                        datosEndeudamientoProspeccion.SalarioNeto = (decimal)TOT;
                        datosEndeudamientoProspeccion.CapacidadPago = (decimal)capacidadPago;
                        datosEndeudamientoProspeccion.CanonMensual = CanonReducido;
                        datosEndeudamientoProspeccion.SectorFinanciero = CuotaSectorFinanciero - sumaSaldoObligacion;
                        datosEndeudamientoProspeccion.SectorReal = CuotaSectorReal;
                        datosEndeudamientoProspeccion.Score = Score;
                        datosEndeudamientoProspeccion.DisponibleLibranza = ((decimal)capacidadPago - CanonReducido);
                        datosEndeudamientoProspeccion.EndeudamientoGlobalCuota = (decimal)EndeudamientoActual_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalCuota = ControNivelEndeudamiento_Cuota_Porc;
                        datosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota = (decimal)EndeudamientoG_Sin_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalSinCuota = ControNivelEndeudamiento_Sin_Cuota_Porc;
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalCuota = EndeudamientoActual_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalSinCuota = EndeudamientoG_Sin_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.TotalCanonDeducciones = ((decimal)totalEgresos + CanonReducido+(decimal)totalObligaciones);
                        datosEndeudamientoProspeccion.DecisionHomologada = ReporteTrans.Data.descripcionDecision;
                        await _creditoRepository.InsertarDatosEndeudamientoProspeccion(datosEndeudamientoProspeccion);

                        var valoresSimulacionProspeccionInsert = new ValoresSimulacionCreditoReclasificacion
                        {
                            NUMERO_DOCUMENTO = request.documento,
                            TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                            TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                            SEGURO_VIDA = valorSeguroVida,
                            SEGURO_HOGAR = valorSeguroHogar,
                            SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                            SEGURO_VIDA_EXTRAPRIMA = 0,
                            TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                            TASA_MIXTA_EXTRAPRIMA = 0,
                            FACTOR = 0,
                            FACTOR_EXTRA_PRIMA = 0,
                            PLAZO = plazo,
                            VALOR_PRESTAMO = (decimal)request.valor - (decimal)valorCanonInicial,
                            CUOTA_SIN_SEGURO_HOGAR = CanonReducido - valorSeguroVida,
                            CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            CUOTA_SEGURO_HOGAR = CanonReducido,
                            CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                            VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                            CAPACIDAD_PAGO = (decimal)(capacidadPago),
                            VALOR_PRESTAMO_SEGUN_CUOTA = (decimal)request.valor
                        };

                        await _creditoRepository.InsertValoresSimulacionCredito(valoresSimulacionProspeccionInsert);

                        if (ReporteTrans.Data.fechaPermanenciaVigente != null && ReporteTrans.Data.fechaPermanenciaVigente != "" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = ReporteTrans.Data.fechaPermanenciaVigente
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        if (ReporteTrans.Data.totalObligacionMora > 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "NO VIABLE: Motivo evaluación de políticas (por presentar deuda en mora)"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        if (!recalcular && request.valorPrestamo>0 && reporteSimulacion60PorCientoDto.controlCapacidadPago == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if ((ReporteTrans.Data.descripcionDecision.Equals("PENDIENTE") && ControNivelEndeudamiento_Cuota_Porc == "NO APLICA") && request.Usr_id == null && Score <= parametrosGenerales.Score_Min_TU)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Para continuar con el proceso, por favor acérquese a un punto de atención",
                                RespuestaRadicado = false
                        });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        else
                        {
                            if (ReporteTrans.Data.riesgoMinimo <= parametrosGenerales.Score_Min_TU && !validacionesExcepcion.VCP_ESTADO)
                            {
                                planillaSimulacion.Clear();
                                planillaSimulacion.Add(new SimulacionDto
                                {
                                    Sobrante = "No se puede presentar una simulación, no cumple con el score minimo: " + parametrosGenerales.Score_Min_TU
                                });

                                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            }
                        }
                        bool bloquearCondicion = false; 
                        #endregion
                        #region Validacion y Credito segun endeudamiento Global
                        if (bloquearCondicion && Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero) > ObjetivoEndeudameinto)
                        {
                            //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto)
                            ////{
                            //    try
                            //    {
                            //        CanonReducido = Math.Round((decimal)await CalcularCuotaNueva(ObjetivoEndeudameinto, 0, EndeudamientoActual_Cuota, 0, totalIngresos), 0, MidpointRounding.AwayFromZero) - (decimal)EndeudamientoG_Sin_Cuota - (decimal)totalEgresos;
                            //        EndeudamientoActual_Cuota = await CalcularEndeudamiento((double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos, totalIngresos);
                            //        VrCredito = (double)(((CanonReducido - valorSeguroVida - valorSeguroHogar) / factor) + valorOpcionDeCompra);
                            //        request.valorPrestamo = VrCredito;
                            //        #region Reporte 60%
                            //        reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = String.Format("{0:N0}", Math.Round(CanonReducido, 0, MidpointRounding.AwayFromZero));
                            //        reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = String.Format("{0:N0}", ((double)CanonReducido + EndeudamientoG_Sin_Cuota));
                            //        reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = Math.Round((EndeudamientoActual_Cuota * 100), 0, MidpointRounding.AwayFromZero).ToString() + "%";
                            //        //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc <= ObjetivoEndeudameinto)
                            //        //{
                            //        //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                            //        //}
                            //        //else
                            //        //{
                            //        //    if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc > ObjetivoEndeudameinto)
                            //        //    {
                            //        //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NUEVO VALOR CUOTA";
                            //        //    }
                            //        //    else
                            //        //    {
                            //        //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //        //    }
                            //        //}
                            //        #endregion
                            //        if (Math.Round(VrCredito, 0, MidpointRounding.ToZero) <= 0)
                            //        {
                            //            reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //            planillaSimulacion.Clear();
                            //            planillaSimulacion.Add(new SimulacionDto
                            //            {
                            //                Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 80%"
                            //            });
                            //            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        }
                            //        valorViviendaMaximo = (decimal)(VrCredito / (1 - (porcentajeCanonInicial + porcentajeOpcionCompra)));
                            //        request.valor = (double)valorViviendaMaximo;
                            //        ConteoTU++;
                            //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = ConteoTU > 5 ? "NO APLICA" : "SI APLICA";
                            //        var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                            //        double toleranciaIngresosTotales = (double)(datos.EDP_SALARIO_TOTAL) * 0.592;
                            //        var plazoValorMaximo = plazo;
                            //        double diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos.EDP_SECTOR_FINANCIERO - (double)datos.EDP_SECTOR_REAL - (double)datos.EDP_DEDUCCIONES;
                            //        var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                            //        var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                            //        var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                            //        var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                            //        double valorMaximoCalculado = prestamosMaximo / 1000000;
                            //        double valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                            //        if (valorMaximo <= 13000000 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                            //        {
                            //            planillaSimulacion.Add(new SimulacionDto
                            //            {
                            //                Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                            //            });

                            //            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        }

                            //        if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                            //        {
                            //            planillaSimulacion.Clear();
                            //            planillaSimulacion.Add(new SimulacionDto
                            //            {
                            //                Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo " + valorMaximo.ToString("C0",new System.Globalization.CultureInfo("es-CO")) + ", por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                            //            });

                            //            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        }

                            //        #region Save Reporte 60%
                            //        var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //        using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //        {
                            //            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //            await SaveReportTU60(formFile, simulacionId);
                            //        }

                            //        #endregion
                            //        //if (ConteoTU > 5)
                            //        //{
                            //        //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //        //    planillaSimulacion.Clear();
                            //        //    planillaSimulacion.Add(new SimulacionDto
                            //        //    {
                            //        //        Sobrante = "No se puede presentar una simulación, Verificar endeudamiento"
                            //        //    });
                            //        //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        //}
                            //        //else
                            //        //{
                            //        //    return await CrearSimulacionLeasing24(request, datosPersonales, simulacionId);
                            //        //}
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        planillaSimulacion.Clear();
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = ex.Message
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }
                            ////}
                            //else
                            //{
                            //    #region Reporte 60%
                            //    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            //    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            //    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //    #endregion
                            //    #region Save Reporte 60%
                            //    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //    {
                            //        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //        await SaveReportTU60(formFile, simulacionId);
                            //    }
                            //    #endregion
                            //    planillaSimulacion.Add(new SimulacionDto
                            //    {
                            //        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //    });
                            //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //}
                        }
                        else
                        {
                          
                            #region Reporte 60%
                            reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                            #endregion
                            #region Save Reporte 60%
                            var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            {
                                IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                await SaveReportTU60(formFile, simulacionId);
                            }
                            #endregion
                            
                        }
                        #endregion
                    }
                    else
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "Respuesta Consulta->" + ProspeccionTransUnion.Data.detalleError
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                }
                catch (Exception ex)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = ex.Message
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }

            if (CanonReducido < parametrosGenerales.ValorMinimoCuota)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "La cuota debe ser mayor a la mínima permitida (" + parametrosGenerales.ValorMinimoCuota.ToString("C0", new CultureInfo("es-CO")) + ") por la entidad, intente nuevamente con un valor mayor del inmueble.",
                    RespuestaRadicado = false
                });  

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            #endregion
            if (CanonReducido > (decimal)capacidadPago && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co" || validacionEndeudamientoGlobal == "NO APLICA")
            {
                //decimal factorFinanciero = (decimal)(1 - Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo));
                //decimal factorDescuento = (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo);
                decimal totalRecursosFuturos = totalCesantiasProgramadas + totalAhorroProgramado + (decimal)subsidiovivienda;
                //// Numerador


                //decimal capacidadPagoCalculada = ((decimal)totalIngresos / 2) - (decimal)totalObligaciones - (decimal)totalEgresos - valorSeguroHogar;
                //decimal valorViviendaMaximoNumerador = 0;
                //if (parametrosGenerales.ObligacionesExternasCapacidadPago)
                //{
                //    valorViviendaMaximoNumerador = (decimal)valorCanonInicial + ((((decimal)capacidadPago - valorSeguroHogar - valorSeguroVida - (decimal)totalObligaciones) / tasaPeriodicaNMV) * factorFinanciero) + totalRecursosFuturos * factorDescuento;
                //}
                //else
                //{
                //    valorViviendaMaximoNumerador = (decimal)valorCanonInicial + ((((decimal)capacidadPago - valorSeguroHogar - valorSeguroVida) / tasaPeriodicaNMV) * factorFinanciero) + totalRecursosFuturos * factorDescuento;
                //}


                //// Denominador
                //decimal valorViviendaMaximoDenominador = 1;

                //// Resultado
                //valorViviendaMaximo = valorViviendaMaximoNumerador / valorViviendaMaximoDenominador;

                valorViviendaMaximo = (decimal) BuscarValorMaximoFinanciar(VrCredito,totalIngresos,(double) parametrosGenerales.PorcentajeMinimoEndeudamiento, totalObligaciones); 
                decimal valorMaximoPrestarCalculado = Math.Truncate(valorViviendaMaximo / 1000000.0m) * 1000000.0m;

                request.valorPrestamo = (double)valorMaximoPrestarCalculado;
                request.valor = (double)valorMaximoPrestarCalculado;

                if ((double)valorMaximoPrestarCalculado < 0)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento global"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }

                if (ConteoTU < 100)
                {
                    ConteoTU++;

                    return await CrearSimulacionLeasing24(request, datosPersonales, simulacionId, true);
                }
                else
                {
                    ConteoTU = 0; // Opcional: reiniciar para futuras ejecuciones

                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento global"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }

            }
            else
            {
                #region Crear Simulacion CLiente
                var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
                {
                    Id = datosPersonales.IdSimulacion,
                    EnvioNotificacion = false,
                    Estado = "I",
                    FechaActualiza = DateTime.Now,
                    FechaSolicitud = DateTime.Now,
                    NumeroPreAprobado = 0,
                    ProblemaEmail = false,
                    UsuarioActualiza = Guid.NewGuid(),
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    Score = Score,
                    EndeudamientoCCH = (int)EndeudamientoCCH,
                    EndeudamientoSCH = (int)EndeudamientoSCH
                });
                var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(new SMCSimulacionClienteDto()
                {
                    Documento = request.documento,
                    OpcionCompra = (double)valorOpcionDeCompra,
                    TotalCuotas = plazo,
                    FechaSimulacion = DateTime.Now,
                    IdDatosPersonales = datosPersonales.Id,
                    IdSimulacion = simulacionId,
                    IdSimulacionCliente = datosPersonales.IdSimulacion,
                    NumeroPreaprobado = 0,
                    TasaEa = tasaCredito,
                    TasaFrech = 0,
                    TasaMv = (double)tasaPeriodicaNMV,
                    TipoAbono = "N",
                    TipoAlivio = "N",
                    TipoVivienda = request.ViviendaNueva ? "N" : "U",
                    TomaSeguro = "N",
                    UsaRecursosAcumulados = RecursosPropios > 0 ? "S" : "N",
                    ValorPrestamo = (decimal)VrCredito,
                    ValorPrestamosLeasing = (double)VrCredito2,
                    ValorVivienda = (double?)valorViviendaMaximo,
                    ViviendaVis = viviendaVis,
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    TasaSeguroHogar = (double)parametrosGenerales.Tasas.SeguroHogar,
                    TasaSeguroVida = (double)parametrosGenerales.Tasas.SeguroVida,
                    ValorPrestamoSolicitado = request.valorPrestamo,
                    PorcentajeCanonInicial = request.CanonInicial,
                    PorcentajeOpcionCompra = request.porOpcionCompra,
                    ValorViviendaSeguro = request.valor,
                    AportesVoluntarios = request.TasaVoluntariaAportes ? 1 : 0,
                    Usr_id = request.Usr_id,
                    afl_id = request.afl_id,
                    CLC_ID = 1

                });
                #endregion

                SolicitudCredito solicitudGeneralCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, request.documento);


                #region Simulacion
                for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        var fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        PlanSimulacion.Add(new Leasing24()
                        {
                            Capital = 0,
                            IntCte = 0,
                            SegVida = 0,
                            SegHogar = 0,
                            Cuota = 0,
                            Saldo = (decimal)request.valor- (decimal)valorCanonInicial,
                            NroCuota = i,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCorte,
                            SaldoCapitalReducido = CapitalReducido,
                            CapitalAPagar = 0,
                            CapitalPorPagar = 0,
                            InteresesCorrientesAPagar = 0,
                            InteresesCorrientesPorPagar = 0,
                            SeguroHogar = 0,
                            SeguvoVida = 0,
                            SeguroPorPagar = 0,
                            CuotaUnidadPagadora = 0,
                            AfectacionCuentaIndividual = 0,
                            DeudaRemanente = CapitalReducido,
                            BeneficioTasaFresh = 0,
                            BeneficioAlivioCuota = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = (decimal)(request.valor - valorCanonInicial)                            
                        });
                    }
                    else
                    {
                        
                        var IntCte = i==1 ? (decimal)(PlanSimulacion[i - 1].Saldo-(decimal)valorCanonInicial) * tasaPeriodicaNMV : (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional) * tasaPeriodicaNMV;
                        var Capital = canonTradicional - valorSeguroVida - valorSeguroHogar  - IntCte;
                        var SaldoLeasingTradicional = (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional - Capital);
                        var CapitalTardicional = canonTradicional - IntCte; 
                        var FechaCorteTemp = PlanSimulacion[i - 1].FechaCorte.AddMonths(1);
                        var FechaCorte = new DateTime(FechaCorteTemp.Year, FechaCorteTemp.Month, DateTime.DaysInMonth(FechaCorteTemp.Year, FechaCorteTemp.Month));
                        var FechaPagoTemp = PlanSimulacion[i - 1].FechaPago.AddMonths(1).AddDays(-PlanSimulacion[i - 1].FechaPago.Day).AddMonths(1);
                        var FechaPago = new DateTime(FechaPagoTemp.Year, FechaPagoTemp.Month, DateTime.DaysInMonth(FechaPagoTemp.Year, FechaPagoTemp.Month));
                        var InteresesCorrientesAPagar = 0M;
                        var CapitalAPagar = 0M;
                        var CapitalPorPagar = 0M;
                        var InteresesCorrientesPorPagar = 0M;
                        var SaldoCapitalReducido = 0M;
                        var SaldoTotalRestante = 0M;
                        var TotalSeguros = 0M;
                        var CapitalPorPagarLeasing = 0M; 
                        if (i == plazo)
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            CanonReducido = CapitalAPagar + InteresesCorrientesAPagar + valorSeguroVida + valorSeguroHogar;
                            canonTradicional = IntCte + Capital + valorSeguroHogar + valorSeguroVida;
                            SaldoTotalRestante = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar;
                            

                        }
                        else
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = CanonReducido - InteresesCorrientesAPagar - valorSeguroHogar - valorSeguroVida;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            Capital = canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;                            
                        }
                        TotalSeguros = valorSeguroHogar + valorSeguroVida;
                        CapitalPorPagarLeasing = canonTradicional - CanonReducido;


                        double seguroVidaProrrata = 0;
                        double seguroHogarProrrata = 0;

                        if (i == 1)
                        {
                            if (solicitudGeneralCredito != null)
                            {
                                var fecha = request.FechaDesembolso;

                                if (fecha < new DateTime(1753, 1, 1))
                                {
                                    fecha = DateTime.Now; // o null
                                }

                                await _creditoRepository.ActualizarFechaDesembolsoCredito(
                                    solicitudGeneralCredito.SOC_ID,
                                    fecha);

                                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(solicitudGeneralCredito.SOC_ID);

                                if (datosFactura != null && datosFactura.FAC_NUMERO_FACTURA != null)
                                {
                                    SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(solicitudGeneralCredito.SOC_ID);

                                    DateTime? fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;

                                    DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));

                                    var diasProrrata = (ultimoDia - informacionDesembolso.SID_FECHA_DESEMBOLSO).Days;
                                    seguroVidaProrrata = Math.Round((double)valorSeguroVida * (diasProrrata + 1) / 30, 0);
                                    seguroHogarProrrata = Math.Round((double)valorSeguroHogar / 30, 0) * (diasProrrata + 0);
                                }
                            }
                        }

                        PlanSimulacion.Add(new Leasing24()
                        {
                            NroCuota = i,
                            Cuota = canonTradicional,
                            SegHogar = valorSeguroHogar,
                            SegVida = valorSeguroVida,
                            IntCte = IntCte,
                            Capital = Capital,
                            Saldo = i==1 ? CapitalReducido : PlanSimulacion[i-2].DeudaRemanente - PlanSimulacion[i - 1].CapitalAPagar + PlanSimulacion[i - 1].CapitalPorPagar + PlanSimulacion[i - 1].InteresesCorrientesPorPagar,
                            FechaCorte = FechaCorte,
                            FechaPago = FechaPago,
                            CuotaUnidadPagadora = CanonReducido + (decimal)seguroHogarProrrata+ (decimal)seguroVidaProrrata,
                            SeguroHogar = valorSeguroHogar+ (decimal)seguroHogarProrrata,
                            SeguvoVida = valorSeguroVida+(decimal)seguroVidaProrrata,
                            SeguroPorPagar = 0,
                            InteresesCorrientesAPagar = InteresesCorrientesAPagar,
                            InteresesCorrientesPorPagar = InteresesCorrientesPorPagar,
                            CapitalAPagar = CapitalAPagar,
                            CapitalPorPagar = CapitalPorPagar,
                            SaldoCapitalReducido = SaldoCapitalReducido,
                            DeudaRemanente = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar,
                            AfectacionCuentaIndividual = SaldoTotalRestante,
                            BeneficioAlivioCuota = 0,
                            BeneficioTasaFresh = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = SaldoLeasingTradicional,
                            SegurosTotal = TotalSeguros, 
                            CapitalPorPagarLeasing = CapitalPorPagarLeasing
                        });
                    }
                }
                foreach (Leasing24 item in PlanSimulacion)
                {
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Cuota = item.NroCuota,
                        FechaCorte = item.FechaCorte,
                        FechaPago = item.FechaPago,
                        SaldoLeasing = item.Saldo,
                        CapitalLeasing = item.Capital,
                        InteresLeasing = item.IntCte,
                        SeguroVidaLeasing = item.SeguvoVida,
                        SeguroHogar = (double)item.SeguroHogar,
                        TotalCuota = (double)item.Cuota,
                        CuotaUnidadPagadora = item.CuotaUnidadPagadora,
                        SaldoCapital = item.Capital,
                        CapitalInteresporPagar = item.InteresesCorrientesAPagar,
                        CapitalSaldoTotal = item.DeudaRemanente,
                        SaldoCapitalReducido = item.SaldoCapitalReducido,
                        CuotaLeasing = item.CuotaUnidadPagadora,
                        ValorDeudaRemanente = item.DeudaRemanente,
                        IdSimulacion = simulacionId,
                        IdDetalleSimulacion = Guid.NewGuid(),
                        IdSimulacionCliente = datosPersonales.IdSimulacion,
                        AfectacionCuentaIndividual = item.AfectacionCuentaIndividual,
                        /// nueva extructura
                        InteresPorPagar = item.InteresesCorrientesPorPagar,
                        CapitalPorPagar = item.CapitalPorPagar,
                        CapitalReducido = item.SaldoCapitalReducido,
                        Capitalpagar = item.CapitalAPagar,
                        InteresesPorPagar = item.InteresesCorrientesPorPagar,
                        CapitalSaldo = item.Capital,
                        SaldoTotal = item.Saldo,
                        NoventaCapacidadDePago = (decimal)capacidadPago,
                        ValorMaximoVivienda = (decimal)request.valor,
                        SaldoLeasingTradicional = item.Saldo - item.Capital, 
                        SegurosTotal = Math.Round(item.SegurosTotal, 2), 
                        CapitalPorPagarLeasing = item.CapitalPorPagarLeasing,
                        Afl_id = request.afl_id,
                        NumeroDocumento = request.documento, 
                        ValorFinanciar = item.SaldoLeasingTradicional - (decimal)valorCanonInicial
                    });
                }
                #endregion
            }
            #region Guardado en BD


            planillaSimulacion[0].ValorOpcionCompra = (decimal)valorOpcionDeCompra;
            planillaSimulacion[0].ValorCanonInicial = (decimal)valorCanonInicial;
            planillaSimulacion[0].factor = factor;
            planillaSimulacion[0].CuotaSectorFinanciero = CuotaSectorFinanciero; 
            planillaSimulacion[0].CuotaSectorReal = CuotaSectorReal; 
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].RiesgoMinimoTransUnion = (Score == 0) ? 0 : Score;
            planillaSimulacion[0].AportesPropios = (decimal)request.ValorViviendaSeguro - (decimal)valorViviendaDeseado;


            if (request.tipoSimularOProspectar == 3)
            {
                await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
                var consultaSMC = await _creditoBusinessLogic.ConsultarSimulacionCliente(request.documento);
                var smc_id = consultaSMC.Data.smc_id;


                ValoresSimulacionAfiliadoDto valoresSimulacion = new ValoresSimulacionAfiliadoDto();
                valoresSimulacion.ValorFinanciar = planillaSimulacion[0].ValorFinanciar;
                valoresSimulacion.NumeroDocumento = request.documento;
                valoresSimulacion.SMS_ID = smc_id;
                valoresSimulacion.RiesgoMinimo = planillaSimulacion[0].RiesgoMinimoTransUnion;
                valoresSimulacion.ValorSectorFinanciero = planillaSimulacion[0].CuotaSectorFinanciero;
                valoresSimulacion.ValorSectorReal = planillaSimulacion[0].CuotaSectorReal;
                valoresSimulacion.Factor = planillaSimulacion[0].factor; 

                await _creditoRepository.InsertarValoresSimulacionAfiliado(valoresSimulacion); 
            }
            #endregion
            
            List<InformacionProyeccion> proyecciones = new List<InformacionProyeccion>();
            foreach (var item in listaInformacionProyeccion)
            {
                proyecciones.Add(item);
            }

            planillaSimulacion[0].LstProyeccion = proyecciones;

            var valoresSimulacionProspeccion = new ValoresSimulacionCreditoReclasificacion
            {
                NUMERO_DOCUMENTO = request.documento,
                TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                SEGURO_VIDA = decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                SEGURO_HOGAR = decimal.Parse(planillaSimulacion[0].SeguroHogar.ToString()),
                SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                SEGURO_VIDA_EXTRAPRIMA = 0,
                TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                TASA_MIXTA_EXTRAPRIMA = 0,
                FACTOR = 0,
                FACTOR_EXTRA_PRIMA = 0,
                PLAZO = plazo,
                VALOR_PRESTAMO = planillaSimulacion[0].ValorFinanciar,
                CUOTA_SIN_SEGURO_HOGAR = canonTradicional - decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                CUOTA_SEGURO_HOGAR = canonTradicional,
                CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                CAPACIDAD_PAGO = (decimal)capacidadPago,
                VALOR_PRESTAMO_SEGUN_CUOTA = 0
            };

            bool guardado = await _creditoRepository
                .InsertValoresSimulacionCredito(valoresSimulacionProspeccion);

            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        #endregion

        #region Credito Leasing Cesantias
        public class Cesantias
        {
            public int NroCuota { get; set; }
            public DateTime FechaCorte { get; set; }
            public DateTime FechaPago { get; set; }
            public decimal Capital { get; set; }
            public decimal IntCte { get; set; }
            public decimal SegVida { get; set; }
            public decimal SegHogar { get; set; }
            public decimal Cuota { get; set; }
            public decimal Saldo { get; set; }
            public decimal SaldoCapitalReducido { get; set; }
            public decimal CapitalAPagar { get; set; }
            public decimal CapitalPorPagar { get; set; }
            public decimal InteresesCorrientesAPagar { get; set; }
            public decimal InteresesCorrientesPorPagar { get; set; }
            public decimal SeguroHogar { get; set; }
            public decimal SeguvoVida { get; set; }
            public decimal SeguroPorPagar { get; set; }
            public decimal CuotaUnidadPagadora { get; set; }
            public decimal AfectacionCuentaIndividual { get; set; }
            public decimal DeudaRemanente { get; set; }
            public decimal BeneficioTasaFresh { get; set; }
            public decimal BeneficioAlivioCuota { get; set; }
            public decimal ValoraCobrarFonvivienda { get; set; }
            public decimal SaldoLeasingTradicional { get; set; }
            public decimal SegurosTotal { get; set; }
        }
        public async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionLeasingCesantias(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid simulacionId, bool recalcular = false)
        {
            #region DatosSimulacion
            ValoresProyectadosDto valoresProyectadosDto = new ValoresProyectadosDto();
            List<Cesantias> PlanSimulacion = new();
            List<SimulacionDto> planillaSimulacion = new();
            SimulacionDto simulacionDto = new SimulacionDto();
            var equivalenciasSimuladoCredito = (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento)).Data;
            var cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto { TipoIdentificacion = request.tipoIdentificacion, NumeroDocumento = request.documento })).Data;

            var usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

            var correoLogueado = usuarioEncontrado == null ? "" : usuarioEncontrado.USR_USERNAME;

            var tipoActorRecursos = recursos.IdTipoActor;
            var descripcionVinculacion = "";
            foreach (var item in recursos.TiposVinculacion)
            {
                if (tipoActorRecursos == item.IdTipoActor)
                {
                    descripcionVinculacion = item.DescripcionTipoActor;
                }
            }

            var TasaEATipoCredito = await _creditoRepository.ObtenerTasasSimuladorByTipoCredito(descripcionVinculacion, 2);


            TasaSimulador tasaSimulador = new TasaSimulador();
            if (tipoActorRecursos == 2) //LEASING FUNCIONARIO CESANTIAS
            {
                tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("LEASING AFILIADO CESANTIAS");

            }
            else if (tipoActorRecursos == 16) //FUNCIONARIO AFILIADO CESANTIAS
            {
                tasaSimulador = await _creditoRepository.ConsultarTasaPorTipoCredito("LEASING FUNCIONARIO CESANTIAS");
            }

            var parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
            {
                SimuladorId = int.Parse(tasaSimulador.SIM_ID),
                TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
            });
            if (parametrosGenerales.Status != string.Empty)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = parametrosGenerales.Status
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

            //parametrosGenerales.Tasas.EfectivaAnual = TasaEATipoCredito.SIM_TASA_EA / 100;
            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;
            var datosSimulacionPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento));
            var tiposVivienda = (await _catalogsBusinessLogic.ObtenerTiposDeVivienda()).Data;
            var proyeccionactual = (await _creditoBusinessLogic.ObtenerProyeccionCliente(cliente.GradoId.ToString())).Data;
            var simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(proyeccionactual.Id.ToString())).Data;
            var categoriaCivil = false;
            if (parametrosGenerales.SeleccionCargoConSalario)
            {
                if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
                {
                    proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
                    categoriaCivil = true;
                }
                else if (proyeccionactual.Id == 10 || proyeccionactual.Id == 11 || proyeccionactual.Id == 12)
                {
                    double cuotasAfiliado = (double)recursos.NumeroCuotas / 12;

                    int cuotas = (int)Math.Round(cuotasAfiliado, 0, MidpointRounding.AwayFromZero);

                    var cargo = simcargo
                                .Where(x => x.salariobasico <= datosFinancieros.ValorSalario)
                                .OrderByDescending(x => x.salariobasico)
                                .FirstOrDefault()
                                ??
                                simcargo
                                    .Where(x => x.salariobasico > datosFinancieros.ValorSalario)
                                    .OrderBy(x => x.salariobasico)
                                    .FirstOrDefault();

                    if (cuotas < 7)
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 10;
                    }
                    else if (cuotas >= 7 && cuotas < 9)
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 11;
                    }
                    else
                    {
                        proyeccionactual.salariobasico = cargo.salariobasico;
                        proyeccionactual.Id = 12;
                    }
                }
                else
                {
                    var cargo = simcargo
                                .Where(x => x.salariobasico <= datosFinancieros.ValorSalario)
                                .OrderByDescending(x => x.salariobasico)
                                .FirstOrDefault()
                                ??
                                simcargo
                                    .Where(x => x.salariobasico > datosFinancieros.ValorSalario)
                                    .OrderBy(x => x.salariobasico)
                                    .FirstOrDefault();

                    proyeccionactual.salariobasico = cargo.salariobasico;
                    proyeccionactual.Id = cargo.Id;

                }

            }
            else if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
            {
                proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
                categoriaCivil = true;
            }
            int tipoAfiliacionId = int.Parse(cliente.TipoAfiliacion.ToString());
            var viviendaVis = (request.valor / (int)parametrosGenerales.SalarioMinimo) < (double)parametrosGenerales.LimiteVIS ? "VIS" : "NO VIS";

            var valorInmuebleAlmacenado = request.valor > (double)request.avaluoInmueble ? request.valor : (double)request.avaluoInmueble;


            request.valor = request.valorInicialSimuladoInmueble > 0 ? (double)request.valorInicialSimuladoInmueble : valorInmuebleAlmacenado;

            #endregion
            #region Paso1
            var valorViviendaDeseado = request.valor;
            //Este Valor se llena en el paso 7
            var valorViviendaMaximo = 0M;
            #endregion
            #region Paso3
            var tasaEfectivaAnualMensualizada = Math.Pow((double)(parametrosGenerales.Tasas.EfectivaAnual + 1), (1.0 / 12)) - 1;
            var porcentajeCanonInicial = 0.0;
            var porcentajeOpcionCompra = 0.0;
            var tasaCredito = parametrosGenerales.Tasas.EfectivaAnual;
            var tasaPeriodicaNMV = (decimal)tasaEfectivaAnualMensualizada;
            var tasaSeguroVida = parametrosGenerales.Tasas.SeguroVida;
            var tasaSeguroHogar = parametrosGenerales.Tasas.SeguroHogar;

            var datoSeguroSimulacion = await _creditoRepository.ConsultarSeguroSimulacionCredito(request.documento);

            if (datoSeguroSimulacion.SSC_CONVENIO_VIDA == "N")
            {
                tasaSeguroVida = 0;
            }

            if (datoSeguroSimulacion.SSC_CONVENIO_HOGAR == "N")
            {
                tasaSeguroHogar = 0;
            }

            if (request.CanonInicial >= parametrosGenerales.Tasas.Min_Canon_Inicial && request.CanonInicial <= parametrosGenerales.Tasas.Max_Canon_Inicial)//Revisar donde se puede obtener para no dejarlo quemado
            {
                porcentajeCanonInicial = (double)(request.CanonInicial / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "CANON INICIAL FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser entre el 5% y el 15%
            }
            if (request.porOpcionCompra <= parametrosGenerales.Tasas.Max_Opcion_Compra)
            {
                porcentajeOpcionCompra = (double)(request.porOpcionCompra / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser menor a 30%
            }

            var plazo = 0;
            var obtenerInfoCliente = await _requestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento);
            if (request.plazo != 0 && (request.plazo >= parametrosGenerales.MinimoPlazoMeses && request.plazo <= parametrosGenerales.MaximoPlazoMeses))
            {
                plazo = request.plazo;
            }
            else
            {
                plazo = parametrosGenerales.MaximoPlazoMeses;
            }
            var AnosPlazo = plazo % 12 == 0 ? (int)(plazo / 12) : (int)(plazo / 12) + 1;
            #endregion
            #region Paso4
            var factor = (decimal)(tasaPeriodicaNMV * (decimal)Math.Pow((1 + (double)tasaPeriodicaNMV), plazo) / (decimal)(Math.Pow(1 + (double)tasaPeriodicaNMV, plazo) - 1));
            var valorCanonInicial = recalcular ? request.valorPrestamo * porcentajeCanonInicial : request.valor * porcentajeCanonInicial;
            #endregion
            #region Paso5
            var VrCredito = 0.0;
            var VrCreditoSegVida = 0.0;
            if (request.valorPrestamo > 0 && request.valorPrestamo < (valorViviendaDeseado - valorCanonInicial))
            {
                valorCanonInicial = recalcular ? valorCanonInicial : valorViviendaDeseado - request.valorPrestamo;
                VrCredito = request.valorPrestamo;
            }
            else
            {
                VrCredito = (double)(valorViviendaDeseado - valorCanonInicial);
            }
            VrCreditoSegVida = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

            var valorSeguroVida = (decimal)VrCredito * tasaSeguroVida;


            var VrCreditoSegHogar = 0.0; 
            if (request.avaluoInmueble > 0)
            {
                VrCreditoSegHogar = (double)request.avaluoInmueble > (double)valorInmuebleAlmacenado ? (double)request.avaluoInmueble : (double)valorInmuebleAlmacenado;
            }
            else
            {
                VrCreditoSegHogar = (double)request.ValorViviendaSeguro > 0 ? (double)request.ValorViviendaSeguro : (double)valorViviendaDeseado;
            }
            var valorSeguroHogar = recalcular ? (decimal)request.ValorViviendaSeguro * tasaSeguroHogar : (decimal)VrCreditoSegHogar * tasaSeguroHogar;
            var VrCuotaSinOpcC =  ((decimal)(valorViviendaDeseado - valorCanonInicial) * factor) + valorSeguroHogar + valorSeguroVida;
            var valorOpcionDeCompra = (decimal)(valorViviendaDeseado * porcentajeOpcionCompra);
            if (valorOpcionDeCompra > (decimal)VrCredito && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA MAYOR AL VALOR DEL CREDITO", 
                    RespuestaRadicado = false
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            var VrDescOpcCompra = (valorOpcionDeCompra / (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), plazo)) * factor;
            var canonTradicional = VrCuotaSinOpcC - VrDescOpcCompra;
            #region Ahorros y Cesantias

            await _creditoRepository.LimpiarRecursosCongelados(simulacionId, request.documento);

            decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;
            decimal RecursosCuentaIndiv = ahorroProgramado + cesantiasProgramadas + intCesantiasProgramadas + intAhorroProgramados;
            var MaximoUsar = RecursosCuentaIndiv * (decimal)(1 - 0.15);

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING CESANTÍAS"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING CESANTÍAS"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING CESANTÍAS"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING CESANTÍAS"
                }
            };

            

            foreach (var recurso in recursosCongelados)
            {
                await _creditoRepository.InsertarRecursosCongelados(recurso);
            }


            await _creditoRepository.LimpiarConceptosCongelados(simulacionId, request.documento);


            List<string> descripcionesConceptos = new List<string>
            {
                "AHORROS OBLIGATORIOS",
                "AHORROS RETROACTIVOS",
                "AHORROS VOLUNTARIOS",
                "INTERESES APORTES",
                "CESANTÍAS",
                "CESANTÍAS RETROACTIVOS",
                "INTERESES CESANTÍAS"
            };

            List<ConceptosCongeladosProspeccion> listaCongeladosProspeccion = new List<ConceptosCongeladosProspeccion>();

            foreach (var cuentas in recursos.Cuentas)
            {
                foreach (var conceptos in cuentas.Conceptos)
                {
                    if (descripcionesConceptos.Contains(conceptos.ConceptoDescripcion))
                    {
                        listaCongeladosProspeccion.Add(new ConceptosCongeladosProspeccion
                        {
                            CON_DESCRIPCION = conceptos.ConceptoDescripcion,
                            CON_VALOR = conceptos.saldo,
                            SLS_ID = simulacionId,
                            CON_NUMERO_DOCUMENTO = request.documento
                        });
                    }
                }
            }



            foreach (var concepto in listaCongeladosProspeccion)
            {
                await _creditoRepository.InsertarConceptosCongelados(concepto);
            }

            #endregion
            var RecursosCI = 0M;
            if (MaximoUsar > (decimal)valorCanonInicial)
            {
                RecursosCI = (decimal)valorCanonInicial;
            }
            else
            {
                RecursosCI = RecursosCuentaIndiv;
            }
            decimal RecursosPropios = (decimal)valorCanonInicial - RecursosCI;
            #region RecursosProyectados
            var antiguedadcalculo = Math.Round(((decimal)datosPersonales.Cuotas / 12), 1);
            var antiguedadaños = (antiguedadcalculo - (int)antiguedadcalculo > 0.5m) ? (int)Math.Ceiling(antiguedadcalculo) : (int)Math.Floor(antiguedadcalculo);
            #region Factores
            decimal factorIncremento = parametrosGenerales.Factores.FactorIncremento / 100;
            decimal factorIncrementoSM = parametrosGenerales.Factores.FactorIncrementoSubsidio / 100;
            decimal factorAhorro = request.TasaVoluntariaAportes ? (parametrosGenerales.Factores.FactorAhorro + cliente.TasaVoluntariaAportes) / 100 : parametrosGenerales.Factores.FactorAhorro / 100;
            decimal factorInflacion = parametrosGenerales.Factores.FactorInflacion / 100;
            decimal factorCesantias = parametrosGenerales.Factores.FactorCesantias / 100;
            decimal factorInteresesCesantias = 12.0M / 100;
            #endregion
            #region Valores Para Proyeccion
            #region Uso Canon Inicial
            var valorIntCesantiasCanonInicial = intCesantiasProgramadas < (decimal)valorCanonInicial ? intCesantiasProgramadas : (decimal)valorCanonInicial;
            var valorIntAhorroCanonInicial = intAhorroProgramados < ((decimal)valorCanonInicial - intCesantiasProgramadas) ? intAhorroProgramados : ((decimal)valorCanonInicial - intCesantiasProgramadas);
            var valorCesantiasCanonInicial = cesantiasProgramadas < ((decimal)valorCanonInicial - intAhorroProgramados - intCesantiasProgramadas) ? cesantiasProgramadas : ((decimal)valorCanonInicial - intAhorroProgramados - intCesantiasProgramadas);
            var valorAhorroCanonInicial = ahorroProgramado < ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial)) ? ahorroProgramado : ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial));

            #endregion
            #region Actualizacion Ahorros y Cesantias Para Proyeccion
            ahorroProgramado = ahorroProgramado - valorAhorroCanonInicial;
            cesantiasProgramadas = cesantiasProgramadas - valorCesantiasCanonInicial;
            intCesantiasProgramadas = intCesantiasProgramadas - valorIntCesantiasCanonInicial;
            intAhorroProgramados = intAhorroProgramados - valorIntAhorroCanonInicial;
            #endregion
            #region Proyeccion
            //Ejercicio valor i del for en 0
            decimal sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
            decimal SMProyectado = (decimal)parametrosGenerales.SalarioMinimo;
            decimal SueldoPlanDePagos = sueldo; 
            decimal AhorrosAnt = ahorroProgramado;
            decimal AcumuladoAhorros = ahorroProgramado;
            decimal IntAhorrosAnt = intAhorroProgramados;
            decimal AcumuladoIntAhorros = intAhorroProgramados;
            decimal CesantiasAnt = cesantiasProgramadas;
            decimal AcumuladoCesantias = cesantiasProgramadas;
            decimal IntCesantiasAnt = intCesantiasProgramadas;
            decimal AcumuladoIntCesantias = intCesantiasProgramadas;
            decimal TOTAcumuladoAhorros = 0M;
            decimal TOTAcumuladoCesantias = 0M;
            double sueldoProyeccion = 0;
            var countAnios = 1;
            var countCategoria = 0;
            var ctg = proyeccionactual.Id;
            var antiguedadañosTemp = antiguedadaños;
            string StInfoProy = string.Empty;
            //while (antiguedadañosTemp >= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault())
            //{
            //    countCategoria++;
            //    countAnios = antiguedadañosTemp - simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
            //    antiguedadañosTemp = antiguedadañosTemp - simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
            //    if (simcargo.Any(x => x.Id == ctg + 1))
            //    {
            //        ctg = proyeccionactual.Id + countCategoria;
            //        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
            //        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault();
            //    }
            //}
            List<string> LstProyeccion = new List<string>();
            StInfoProy = "Años Antiguedad: " + antiguedadaños;
           


            List<InformacionProyeccion> listaInformacionProyeccion = new List<InformacionProyeccion>();

            InformacionProyeccion informacionProyeccion = new InformacionProyeccion();

            informacionProyeccion.IdProyeccion = 0;
            informacionProyeccion.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
            informacionProyeccion.AniosCargo = countAnios;
            informacionProyeccion.Sueldo = (double)sueldo;
            informacionProyeccion.AhorroProgramado = ahorroProgramado;
            informacionProyeccion.InteresesAhorroProgramado = intAhorroProgramados;
            informacionProyeccion.Cesantias = cesantiasProgramadas;
            informacionProyeccion.InteresesCesantias = intCesantiasProgramadas;
            informacionProyeccion.AniosAntiguedad = antiguedadaños;

            listaInformacionProyeccion.Add(informacionProyeccion);

            for (int i = 1; i <= AnosPlazo; i++)
            {
                countAnios++;
                var aumentosueldo = Math.Pow((1 + (double)factorIncremento), i);
                sueldoProyeccion = Math.Round((double)sueldo * aumentosueldo, 0, MidpointRounding.AwayFromZero);
                decimal Ahorro = Math.Round((decimal)sueldoProyeccion * factorAhorro * 12, 0, MidpointRounding.AwayFromZero);
                AcumuladoAhorros += Ahorro;
                AcumuladoIntAhorros += i > 0 ? Math.Round(AhorrosAnt * factorInflacion, 0, MidpointRounding.AwayFromZero) : 0;
                AhorrosAnt = Ahorro;
                decimal Cesantias = Math.Round((decimal)sueldoProyeccion * factorCesantias * 12, 0, MidpointRounding.AwayFromZero);
                var IntCesantias = Math.Round((AcumuladoCesantias + AcumuladoIntCesantias) * factorInflacion, 0, MidpointRounding.AwayFromZero);
                AcumuladoCesantias += Cesantias;
                AcumuladoIntCesantias += IntCesantias;
                CesantiasAnt = Cesantias;
                IntCesantiasAnt = IntCesantias;
                SMProyectado = simcargo.Where(x => x.Id == ctg).Select(x => x.ProyectaSubsidio).FirstOrDefault() ? SMProyectado + (SMProyectado * factorIncrementoSM) : parametrosGenerales.SalarioMinimo;               

                InformacionProyeccion informacionProyeccionCredito = new InformacionProyeccion();


                informacionProyeccionCredito.IdProyeccion = i;
                informacionProyeccionCredito.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
                informacionProyeccionCredito.AniosCargo = countAnios;
                informacionProyeccionCredito.Sueldo = sueldoProyeccion;
                informacionProyeccionCredito.AhorroProgramado = AcumuladoAhorros;
                informacionProyeccionCredito.InteresesAhorroProgramado = AcumuladoIntAhorros;
                informacionProyeccionCredito.Cesantias = AcumuladoCesantias;
                informacionProyeccionCredito.InteresesCesantias = AcumuladoIntCesantias;


                listaInformacionProyeccion.Add(informacionProyeccionCredito);

                if (simcargo.Where(x => x.Id == ctg).Select(x => x.AñoAscenso).FirstOrDefault() == countAnios)
                {
                    countCategoria++;
                    if (simcargo.Any(x => x.Id == ctg + 1))
                    {
                        if (ctg != 7 && ctg != 8 && ctg != 9)
                        {
                            ctg = proyeccionactual.Id + countCategoria;
                        }
                        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
                        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;

                        countAnios = 0;
                    }

                    
                }
            }
            #endregion
            #region Valores Proyectados
            var subsidioCategoria = (await _catalogsBusinessLogic.ObtenerSimulacionCargoPorCategoria(cliente.CategoriaId, ctg)).Data.FactorSubsidio;
            //var subsidiovivienda = Math.Round(((double)SMProyectado * subsidioCategoria), 0, MidpointRounding.AwayFromZero);
            TOTAcumuladoAhorros = AcumuladoAhorros + AcumuladoIntAhorros;
            TOTAcumuladoCesantias = AcumuladoCesantias + AcumuladoIntCesantias;
            var RecursosCanonInicial = valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial + valorAhorroCanonInicial;
            var TotalOpcionCompra = /*(decimal)subsidiovivienda + */TOTAcumuladoAhorros + TOTAcumuladoCesantias;
            var DivCuoEquiVF = Math.Pow((double)(1 + tasaPeriodicaNMV), plazo);
            var CuotaEquivalenteVF = (TotalOpcionCompra * factor) / (decimal)DivCuoEquiVF;
            var CanonReducido = VrCuotaSinOpcC - CuotaEquivalenteVF;
            var CapitalReducido = (CanonReducido - valorSeguroHogar - valorSeguroVida) / factor;
            #region Valores a Guardar
            valoresProyectadosDto.Svp_Id = Guid.NewGuid();
            valoresProyectadosDto.Sls_Id = datosPersonales.IdSimulacion;
            valoresProyectadosDto.Svp_EA = parametrosGenerales.Tasas.EfectivaAnual;
            valoresProyectadosDto.Svp_Cesantias = TOTAcumuladoCesantias;
            valoresProyectadosDto.Svp_Sueldo = (decimal)sueldoProyeccion;
            valoresProyectadosDto.Svp_Ahorros = TOTAcumuladoAhorros;
            valoresProyectadosDto.Svp_Subsidio = 0M;
            var ValoresProyectados = await _creditoBusinessLogic.GuardarValoresProyectados(valoresProyectadosDto);
            #endregion
            #endregion
            #endregion
            #endregion
            #endregion
            #region Paso6
            double valorSeguroContraActual = (((double)datosFinancieros.ValorSalario * 8) / 100); 
            var totalIngresos = (double)datosFinancieros.TotalIngresos;
            var totalEgresos = (double)datosFinancieros.TotalEgresos + valorSeguroContraActual;
            var capacidadPago = (totalIngresos * 0.5) - totalEgresos;
            #endregion
            #region Paso7
            var Valor1 = (decimal)(valorViviendaDeseado * (1 - porcentajeCanonInicial - porcentajeOpcionCompra));
            var Valor2 = ((decimal)capacidadPago + CuotaEquivalenteVF) / (factor + (tasaSeguroHogar / (1 / (decimal)porcentajeCanonInicial)));
            var VrCredito2 = Math.Min(Valor1, Valor2);
            valorViviendaMaximo = VrCredito2 / (1 - (decimal)(porcentajeCanonInicial + porcentajeOpcionCompra));
            //if (Math.Round(valorViviendaMaximo, 0, MidpointRounding.AwayFromZero) < Math.Round((decimal)valorViviendaDeseado, 0, MidpointRounding.AwayFromZero))
            //{
            //    request.valor = (double?)valorViviendaMaximo;
            //    return await CrearSimulacionLeasingCesantias(request, datosPersonales, simulacionId);
            //}
            #endregion
            int vinculacion = recursos.IdTipoActor;
            string estadoCuenta = "";

            if (vinculacion != 14)
            {


                //vinculacion 16 y 2 pertecenten a FUNCIONARIO AFILIADO ADMINISTRACION CESANTÍAS y AFILIADO ADMINISTRACION CESANTIAS respectivamente
                foreach (var cuentas in recursos.Cuentas)
                {
                    if (vinculacion == 16 || vinculacion == 2)
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;

                        }
                    }
                    else
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;

                        }
                    }
                }

                if (estadoCuenta == "MOROSA")
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "El afiliado presenta cuenta con estado MOROSA"
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }



            if (CanonReducido < parametrosGenerales.ValorMinimoCuota)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "La cuota debe ser mayor a la mínima permitida (" + parametrosGenerales.ValorMinimoCuota.ToString("C0", new CultureInfo("es-CO")) + ") por la entidad, intente nuevamente con un valor mayor del inmueble.",
                    RespuestaRadicado = false
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }

            ValidacionCreditoProspeccion validacionesExcepciones = new ValidacionCreditoProspeccion();

            if (validacionesExcepciones == null)
            {
                validacionesExcepciones.VCP_ESTADO = false;
                validacionesExcepciones.VCP_NUMERO_DOCUMENTO = request.documento;

                await _creditoRepository.InsertarActualizarValidacionCreditoProspeccion(validacionesExcepciones);
            }

            ValidacionCreditoProspeccion validacionesExcepcion = await _creditoRepository.ConsultarValidacionCreditoProspeccion(request.documento);

            double totalObligaciones = 0;
            string validacionEndeudamientoGlobal = ""; 
            #region EndeudamientoGlobal
            if (parametrosGenerales.ActiveTU && request.tipoSimularOProspectar == 3)
            {
                try
                {
                    #region ConsumoTU
                    ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                    {
                        Apellidos = recursos.PrimerApellido,
                        FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                        Identificacion = request.documento,
                        SalarioMensual = Math.Round((decimal)totalIngresos),
                        TipoIdentificacion = request.tipoIdentificacion,
                        Telefono = recursos.Telefono,
                        TraerHistorialCrediticio = "Y",
                        SLS_ID = simulacionId
                    };

                    if (prospeccionTransUnionRequestDto.Telefono.Length < 10 || prospeccionTransUnionRequestDto.Telefono == null)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el teléfono"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    if (prospeccionTransUnionRequestDto.SalarioMensual <= 0)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el salario"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                    var ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                    #endregion
                    ReporteSimulacion60PorCientoDto reporteSimulacion60PorCientoDto = new ReporteSimulacion60PorCientoDto();
                    reporteSimulacion60PorCientoDto.NumeroDocumento = request.documento;
                    double TOT = totalIngresos - totalEgresos;
                    #region Reporte 60%
                    reporteSimulacion60PorCientoDto.salarioTotal = String.Format("{0:N0}", totalIngresos);
                    reporteSimulacion60PorCientoDto.deducciones = String.Format("{0:N0}", totalEgresos);
                    reporteSimulacion60PorCientoDto.salarioNeto = String.Format("{0:N0}", TOT);
                    reporteSimulacion60PorCientoDto.capacidadPago = String.Format("{0:N0}", capacidadPago);
                    reporteSimulacion60PorCientoDto.canonCuotaMensual = String.Format("{0:N0}", CanonReducido);
                    #endregion
                    if (!string.IsNullOrEmpty(ProspeccionTransUnion.Data.response))
                    {
                        var ReporteTrans = await _reportesBusinessLogic.ReporteTransUnionPropiedades(ProspeccionTransUnion.Data.response);
                        #region Validacion Score y Viabilidad
                        if (!ReporteTrans.IsSuccess)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no viable por consulta TransUnion."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                       

                        #endregion
                        #region GuardadoReporteTransUnion
                        using (var memoryStream = new MemoryStream(ReporteTrans.Data.ArchivoBytes))
                        {
                            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            await SaveReportTU(formFile, simulacionId);
                        }
                        #endregion
                        #region Reclasificacion
                        IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, request.documento);
                        var sumaSaldoObligacion = prospeccionResult
                        .Where(linea => linea.OLN_ESTADO_RECALCULAR == false)
                        .Sum(linea => linea.OLN_SALDO_OBLIGACION);
                        #endregion
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.cuotaSectorFinanSoliAse = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorFinanciero);
                        reporteSimulacion60PorCientoDto.cuotaSectorReal = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorReal);
                        #endregion
                        var EndeudamientoG_Sin_Cuota = ReporteTrans.Data.cuotasSectorReal + ReporteTrans.Data.cuotasSectorFinanciero - (double)sumaSaldoObligacion;
                        totalObligaciones = EndeudamientoG_Sin_Cuota; 
                        var EndeudamientoG_Sin_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoG_Sin_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        var EndeudamientoActual_Cuota = (double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos - (double)sumaSaldoObligacion;
                        var EndeudamientoActual_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoActual_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        Score = ReporteTrans.Data.riesgoMinimo;
                        CuotaSectorFinanciero = (decimal)ReporteTrans.Data.cuotasSectorFinanciero;
                        CuotaSectorReal = (decimal)ReporteTrans.Data.cuotasSectorReal;
                        EndeudamientoSCH = EndeudamientoG_Sin_Cuota_Porc;
                        EndeudamientoCCH = EndeudamientoActual_Cuota_Porc;
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.totalObliAntesCredito = String.Format("{0:N0}", EndeudamientoG_Sin_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalSinCanon = (EndeudamientoG_Sin_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.totalObliNuevoCredito = String.Format("{0:N0}", EndeudamientoActual_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalCanon = (EndeudamientoActual_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.disponibleLibranza = String.Format("{0:N0}", (capacidadPago - (double)CanonReducido));
                        reporteSimulacion60PorCientoDto.controlCapacidadPago = capacidadPago >= (double)CanonReducido ? "SI APLICA" : "NO APLICA";

                        var ControNivelEndeudamiento_Cuota_Porc = EndeudamientoActual_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";
                        var ControNivelEndeudamiento_Sin_Cuota_Porc = EndeudamientoG_Sin_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";

                        DatosEndeudamientoProspeccionDto datosEndeudamientoProspeccion = new DatosEndeudamientoProspeccionDto();
                        datosEndeudamientoProspeccion.NumeroDocumento = datosPersonales.NumeroDocumento;
                        datosEndeudamientoProspeccion.SalarioUltimoMes = (decimal)totalIngresos;
                        datosEndeudamientoProspeccion.Descuentos = (decimal)totalEgresos;
                        datosEndeudamientoProspeccion.SalarioNeto = (decimal)TOT;
                        datosEndeudamientoProspeccion.CapacidadPago = (decimal)capacidadPago;
                        datosEndeudamientoProspeccion.CanonMensual = CanonReducido;
                        datosEndeudamientoProspeccion.SectorFinanciero = CuotaSectorFinanciero - sumaSaldoObligacion;
                        datosEndeudamientoProspeccion.SectorReal = CuotaSectorReal;
                        datosEndeudamientoProspeccion.Score = Score;
                        datosEndeudamientoProspeccion.DisponibleLibranza = ((decimal)capacidadPago - CanonReducido);
                        datosEndeudamientoProspeccion.EndeudamientoGlobalCuota = (decimal)EndeudamientoActual_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalCuota = ControNivelEndeudamiento_Cuota_Porc;
                        datosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota = (decimal)EndeudamientoG_Sin_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalSinCuota = ControNivelEndeudamiento_Sin_Cuota_Porc;
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalCuota = EndeudamientoActual_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalSinCuota = EndeudamientoG_Sin_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.TotalCanonDeducciones = ((decimal)totalEgresos + CanonReducido + (decimal)totalObligaciones);
                        datosEndeudamientoProspeccion.DecisionHomologada = ReporteTrans.Data.descripcionDecision;
                        await _creditoRepository.InsertarDatosEndeudamientoProspeccion(datosEndeudamientoProspeccion);

                        var valoresSimulacionProspeccionInsert = new ValoresSimulacionCreditoReclasificacion
                        {
                            NUMERO_DOCUMENTO = request.documento,
                            TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                            TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                            SEGURO_VIDA = valorSeguroVida,
                            SEGURO_HOGAR = valorSeguroHogar,
                            SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                            SEGURO_VIDA_EXTRAPRIMA = 0,
                            TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                            TASA_MIXTA_EXTRAPRIMA = 0,
                            FACTOR = 0,
                            FACTOR_EXTRA_PRIMA = 0,
                            PLAZO = plazo,
                            VALOR_PRESTAMO = (decimal)request.valor - (decimal)valorCanonInicial,
                            CUOTA_SIN_SEGURO_HOGAR = CanonReducido - valorSeguroVida,
                            CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            CUOTA_SEGURO_HOGAR = CanonReducido,
                            CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                            VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                            CAPACIDAD_PAGO = (decimal)(capacidadPago),
                            VALOR_PRESTAMO_SEGUN_CUOTA = (decimal)request.valor
                        };

                        await _creditoRepository.InsertValoresSimulacionCredito(valoresSimulacionProspeccionInsert);

                        if (ReporteTrans.Data.fechaPermanenciaVigente != null && ReporteTrans.Data.fechaPermanenciaVigente != "" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = ReporteTrans.Data.fechaPermanenciaVigente
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (ReporteTrans.Data.totalObligacionMora > 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "NO VIABLE: Motivo evaluación de políticas (por presentar deuda en mora)"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!recalcular && request.valorPrestamo > 0 && reporteSimulacion60PorCientoDto.controlCapacidadPago == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        } 

                    if ((ReporteTrans.Data.descripcionDecision.Equals("PENDIENTE") && ControNivelEndeudamiento_Cuota_Porc == "NO APLICA") && request.Usr_id == null && Score <= parametrosGenerales.Score_Min_TU)
                    {
                        planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "Para continuar con el proceso, por favor acérquese a un punto de atención",
                                RespuestaRadicado = false

                            });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (ReporteTrans.Data.riesgoMinimo <= parametrosGenerales.Score_Min_TU && !validacionesExcepcion.VCP_ESTADO)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no cumple con el score minimo: " + parametrosGenerales.Score_Min_TU
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        #endregion
                        #region Validacion y Credito segun endeudamiento Global
                        if (Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero) > ObjetivoEndeudameinto)
                        {
                            ////if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto)
                            ////{
                            //    try
                            //    {
                            //        CanonReducido = Math.Round((decimal)await CalcularCuotaNueva(ObjetivoEndeudameinto, 0, (double)EndeudamientoActual_Cuota, 0, totalIngresos), 0, MidpointRounding.AwayFromZero) - (decimal)EndeudamientoG_Sin_Cuota - (decimal)totalEgresos;
                            //        EndeudamientoActual_Cuota = await CalcularEndeudamiento((double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos, totalIngresos);
                            //        VrCredito = (double)(((CanonReducido - valorSeguroVida - valorSeguroHogar) / factor) + valorOpcionDeCompra);
                            //        request.valorPrestamo = VrCredito;
                            //        #region Reporte 60%
                            //        reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = String.Format("{0:N0}", Math.Round(CanonReducido, 0, MidpointRounding.AwayFromZero));
                            //        reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = String.Format("{0:N0}", ((double)CanonReducido + EndeudamientoG_Sin_Cuota));
                            //        reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = Math.Round((EndeudamientoActual_Cuota * 100), 0, MidpointRounding.AwayFromZero).ToString() + "%";
                            //        //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc <= ObjetivoEndeudameinto)
                            //        //{
                            //        //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                            //        //}
                            //        //else
                            //        //{
                            //        //    if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc > ObjetivoEndeudameinto)
                            //        //    {
                            //        //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NUEVO VALOR CUOTA";
                            //        //    }
                            //        //    else
                            //        //    {
                            //        //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //        //    }
                            //        //}
                            //        #endregion
                            //        //if (Math.Round(VrCredito, 0, MidpointRounding.ToZero) <= 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                            //        //{
                            //        //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //        //    planillaSimulacion.Clear();
                            //        //    planillaSimulacion.Add(new SimulacionDto
                            //        //    {
                            //        //        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //        //    });
                            //        //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        //}
                            //        valorViviendaMaximo = (decimal)(VrCredito / (1 - (porcentajeCanonInicial + porcentajeOpcionCompra)));
                            //        request.valor = (double)valorViviendaMaximo;
                            //        //ConteoTU++;
                            //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = ConteoTU > 5 ? "NO APLICA" : "SI APLICA";

                            //        var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                            //        double toleranciaIngresosTotales = (double)(datos.EDP_SALARIO_TOTAL) * 0.592;
                            //        var plazoValorMaximo =  plazo;
                            //        double diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos.EDP_SECTOR_FINANCIERO - (double)datos.EDP_SECTOR_REAL - (double)datos.EDP_DEDUCCIONES;
                            //        var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                            //        var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                            //        var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                            //        var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                            //        double valorMaximoCalculado = prestamosMaximo / 1000000;
                            //        double valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                            //        if (valorMaximo <= 13000000 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                            //        {
                            //            planillaSimulacion.Add(new SimulacionDto
                            //            {
                            //                Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                            //            });

                            //            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        }

                            //        if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                            //        {
                            //            planillaSimulacion.Clear();
                            //            planillaSimulacion.Add(new SimulacionDto
                            //            {
                            //                Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo " + valorMaximo.ToString("C0", new System.Globalization.CultureInfo("es-CO")) + ", por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                            //            });

                            //            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        }

                            //        #region Save Reporte 60%
                            //        var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //        using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //        {
                            //            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //            await SaveReportTU60(formFile, simulacionId);
                            //        }

                            //        #endregion
                            //        //if (ConteoTU > 5)
                            //        //{
                            //        //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //        //    planillaSimulacion.Clear();
                            //        //    planillaSimulacion.Add(new SimulacionDto
                            //        //    {
                            //        //        Sobrante = "No se puede presentar una simulación, Verificar endeudamiento"
                            //        //    });
                            //        //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //        //}
                            //        //else
                            //        //{
                            //        //    return await CrearSimulacionLeasingCesantias(request, datosPersonales, simulacionId);
                            ////        //}
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        planillaSimulacion.Clear();
                            //        planillaSimulacion.Add(new SimulacionDto
                            //        {
                            //            Sobrante = ex.Message
                            //        });

                            //        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //    }
                            //}
                            //else
                            //{
                            //    #region Reporte 60%
                            //    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            //    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            //    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //    #endregion
                            //    #region Save Reporte 60%
                            //    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //    {
                            //        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //        await SaveReportTU60(formFile, simulacionId);
                            //    }
                            //    #endregion
                            //    planillaSimulacion.Add(new SimulacionDto
                            //    {
                            //        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //    });
                            //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //}
                        }
                        else
                        {
                            
                                #region Reporte 60%
                                reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                                reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                                reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                                reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                #endregion
                                #region Save Reporte 60%
                                var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                {
                                    IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                    await SaveReportTU60(formFile, simulacionId);
                                }
                                #endregion
                            
                        }
                        #endregion
                    }
                    else
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = ProspeccionTransUnion.Data.detalleError
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                }
                catch (Exception ex)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = ex.Message
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }
            #endregion
            if (Math.Round(CanonReducido, 0) > Math.Round((decimal)capacidadPago, 0) && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co" || validacionEndeudamientoGlobal == "NO APLICA")
            {
                //decimal factorFinanciero = (decimal)(1 - Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo));
                //decimal factorDescuento = (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo);
                //decimal totalRecursosFuturos = TOTAcumuladoCesantias + TOTAcumuladoAhorros;
                //// Numerador
                //decimal capacidadPagoCalculada = 0;

                //if (parametrosGenerales.ObligacionesExternasCapacidadPago)
                //{
                //    capacidadPagoCalculada = ((decimal)totalIngresos / 2) - (decimal)totalObligaciones - (decimal)totalEgresos - valorSeguroHogar;

                //}
                //else
                //{
                //    capacidadPagoCalculada = ((decimal)totalIngresos / 2) - (decimal)totalEgresos - valorSeguroHogar;

                //}

                //decimal valorViviendaMaximoNumerador =
                //    ((((decimal)capacidadPagoCalculada) / (decimal)tasaPeriodicaNMV) * factorFinanciero) +
                //    (TotalOpcionCompra * factorDescuento);

                //// Denominador
                //decimal valorViviendaMaximoDenominador =
                //    (decimal)(1 - porcentajeCanonInicial) +
                //    (
                //        (((1 - (decimal)porcentajeCanonInicial) * tasaSeguroVida))
                //        / tasaPeriodicaNMV
                //    ) * factorFinanciero;

                //// Resultado
                //valorViviendaMaximo = valorViviendaMaximoNumerador / valorViviendaMaximoDenominador;

                valorViviendaMaximo = (decimal)BuscarValorMaximoFinanciar(VrCredito, totalIngresos, (double)parametrosGenerales.PorcentajeMinimoEndeudamiento, totalObligaciones);

                decimal valorMaximoPrestarCalculado = Math.Truncate(valorViviendaMaximo / 1000000.0m) * 1000000.0m;


                request.valorPrestamo = (double)valorMaximoPrestarCalculado;

                request.valor = (double)valorMaximoPrestarCalculado;

                if ((double)valorMaximoPrestarCalculado < 0)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento global"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }
                //ConteoTU = 0;

                if (ConteoTU < 100)
                {
                    ConteoTU++;

                    return await CrearSimulacionLeasingCesantias(request, datosPersonales, simulacionId, true);

                }
                else
                {
                    ConteoTU = 0; // Opcional: reiniciar para futuras ejecuciones

                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }
            }
            else
            {
                #region Crear Simulacion CLiente
                var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
                {
                    Id = datosPersonales.IdSimulacion,
                    EnvioNotificacion = false,
                    Estado = "I",
                    FechaActualiza = DateTime.Now,
                    FechaSolicitud = DateTime.Now,
                    NumeroPreAprobado = 0,
                    ProblemaEmail = false,
                    UsuarioActualiza = Guid.NewGuid(),
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    Score = Score,
                    EndeudamientoCCH = (int)EndeudamientoCCH,
                    EndeudamientoSCH = (int)EndeudamientoSCH
                });
                var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(new SMCSimulacionClienteDto()
                {
                    Documento = request.documento,
                    OpcionCompra = (double)valorOpcionDeCompra,
                    TotalCuotas = plazo,
                    FechaSimulacion = DateTime.Now,
                    IdDatosPersonales = datosPersonales.Id,
                    IdSimulacion = simulacionId,
                    IdSimulacionCliente = datosPersonales.IdSimulacion,
                    NumeroPreaprobado = 0,
                    TasaEa = tasaCredito,
                    TasaFrech = 0,
                    TasaMv = (double)tasaPeriodicaNMV,
                    TipoAbono = "N",
                    TipoAlivio = "N",
                    TipoVivienda = request.ViviendaNueva ? "N" : "U",
                    TomaSeguro = "N",
                    UsaRecursosAcumulados = RecursosPropios > 0 ? "S" : "N",
                    ValorPrestamo = (decimal)VrCredito,
                    ValorPrestamosLeasing = (double)VrCredito2,
                    ValorVivienda = (double?)valorViviendaMaximo,
                    ViviendaVis = viviendaVis,
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    TasaSeguroHogar = (double)parametrosGenerales.Tasas.SeguroHogar,
                    TasaSeguroVida = (double)parametrosGenerales.Tasas.SeguroVida,
                    ValorPrestamoSolicitado = request.valorPrestamo,
                    PorcentajeCanonInicial = request.CanonInicial,
                    PorcentajeOpcionCompra = request.porOpcionCompra,
                    ValorViviendaSeguro = request.ValorViviendaSeguro,
                    AportesVoluntarios = request.TasaVoluntariaAportes ? 1 : 0, 
                    Usr_id = request.Usr_id,
                    afl_id = request.afl_id,
                    CLC_ID = 4
                });
                #endregion

                SolicitudCredito solicitudGeneralCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, request.documento);


                    #region Simulacion
                    for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        var fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        PlanSimulacion.Add(new Cesantias()
                        {
                            Capital = 0,
                            IntCte = 0,
                            SegVida = 0,
                            SegHogar = 0,
                            Cuota = 0,
                            Saldo = (decimal)request.valor - (decimal)valorCanonInicial,
                            NroCuota = i,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            SaldoCapitalReducido = CapitalReducido,
                            CapitalAPagar = 0,
                            CapitalPorPagar = 0,
                            InteresesCorrientesAPagar = 0,
                            InteresesCorrientesPorPagar = 0,
                            SeguroHogar = 0,
                            SeguvoVida = 0,
                            SeguroPorPagar = 0,
                            CuotaUnidadPagadora = 0,
                            AfectacionCuentaIndividual = 0,
                            DeudaRemanente = CapitalReducido,
                            BeneficioTasaFresh = 0,
                            BeneficioAlivioCuota = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = (decimal)(request.valor - valorCanonInicial)
                        });
                    }
                    else
                    {
                        var IntCte = i == 1 ? (decimal)(PlanSimulacion[i - 1].Saldo - (decimal)valorCanonInicial) * tasaPeriodicaNMV : (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional) * tasaPeriodicaNMV;
                        var Capital = canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;
                        var SaldoLeasingTradicional = (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional - Capital);
                        var CapitalTardicional = canonTradicional - IntCte;
                        var FechaCorteTemp = PlanSimulacion[i - 1].FechaCorte.AddMonths(1);
                        var FechaCorte = new DateTime(FechaCorteTemp.Year, FechaCorteTemp.Month, DateTime.DaysInMonth(FechaCorteTemp.Year, FechaCorteTemp.Month));
                        var FechaPagoTemp = PlanSimulacion[i - 1].FechaPago.AddMonths(1).AddDays(-PlanSimulacion[i - 1].FechaPago.Day).AddMonths(1);
                        var FechaPago = new DateTime(FechaPagoTemp.Year, FechaPagoTemp.Month, DateTime.DaysInMonth(FechaPagoTemp.Year, FechaPagoTemp.Month));
                        var InteresesCorrientesAPagar = 0M;
                        var CapitalAPagar = 0M;
                        var CapitalPorPagar = 0M;
                        var InteresesCorrientesPorPagar = 0M;
                        var SaldoCapitalReducido = 0M;
                        var SaldoTotalRestante = 0M;
                        var TotalSeguros = 0M; 
                        if (i == plazo)
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            CanonReducido = CapitalAPagar + InteresesCorrientesAPagar + valorSeguroVida + valorSeguroHogar;
                            canonTradicional = IntCte + Capital + valorSeguroHogar + valorSeguroVida;
                            SaldoTotalRestante = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar;
                        }
                        else
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = CanonReducido - InteresesCorrientesAPagar - valorSeguroHogar - valorSeguroVida;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            Capital = canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;
                        }

                        TotalSeguros = valorSeguroHogar + valorSeguroVida;

                        double seguroVidaProrrata = 0;
                        double seguroHogarProrrata = 0;

                        if (i == 1)
                        {
                            if (solicitudGeneralCredito != null)
                            {
                                var fecha = request.FechaDesembolso;

                                if (fecha < new DateTime(1753, 1, 1))
                                {
                                    fecha = DateTime.Now; // o null
                                }

                                await _creditoRepository.ActualizarFechaDesembolsoCredito(
                                    solicitudGeneralCredito.SOC_ID,
                                    fecha);

                                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(solicitudGeneralCredito.SOC_ID);

                                if (datosFactura != null && datosFactura.FAC_NUMERO_FACTURA != null)
                                {
                                    SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(solicitudGeneralCredito.SOC_ID);

                                    DateTime? fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;

                                    DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));

                                    var diasProrrata = (ultimoDia - informacionDesembolso.SID_FECHA_DESEMBOLSO).Days;
                                    seguroVidaProrrata = Math.Round((double)valorSeguroVida * (diasProrrata + 1) / 30, 0);
                                    seguroHogarProrrata = Math.Round((double)valorSeguroHogar / 30, 0) * (diasProrrata + 0);
                                }
                            }
                        }
                        PlanSimulacion.Add(new Cesantias()
                        {
                            NroCuota = i,
                            Cuota = canonTradicional,
                            SegHogar = valorSeguroHogar+(decimal)seguroHogarProrrata,
                            SegVida = valorSeguroVida+(decimal)seguroVidaProrrata,
                            IntCte = IntCte,
                            Capital = Capital,
                            Saldo = i == 1 ? CapitalReducido : PlanSimulacion[i - 2].DeudaRemanente - PlanSimulacion[i - 1].CapitalAPagar + PlanSimulacion[i - 1].CapitalPorPagar + PlanSimulacion[i - 1].InteresesCorrientesPorPagar,
                            FechaCorte = FechaCorte,
                            FechaPago = FechaPago,
                            CuotaUnidadPagadora = CanonReducido+(decimal)seguroHogarProrrata + (decimal)seguroVidaProrrata,
                            SeguroHogar = valorSeguroHogar,
                            SeguvoVida = valorSeguroVida,
                            SeguroPorPagar = 0,
                            InteresesCorrientesAPagar = InteresesCorrientesAPagar,
                            InteresesCorrientesPorPagar = InteresesCorrientesPorPagar,
                            CapitalAPagar = CapitalAPagar,
                            CapitalPorPagar = CapitalPorPagar,
                            SaldoCapitalReducido = SaldoCapitalReducido,
                            DeudaRemanente = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar,
                            AfectacionCuentaIndividual = SaldoTotalRestante,
                            BeneficioAlivioCuota = 0,
                            BeneficioTasaFresh = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = SaldoLeasingTradicional,
                            SegurosTotal = TotalSeguros
                        });
                    }
                }
                foreach (Cesantias item in PlanSimulacion)
                {
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Cuota = item.NroCuota,
                        FechaCorte = item.FechaCorte,
                        FechaPago = item.FechaPago,
                        SaldoLeasing = item.Saldo,
                        CapitalLeasing = item.Capital,
                        InteresLeasing = item.IntCte,
                        SeguroVidaLeasing = item.SeguvoVida,
                        SeguroHogar = (double)item.SeguroHogar,
                        TotalCuota = (double)item.Cuota,
                        CuotaUnidadPagadora = recalcular ? Math.Round(item.CuotaUnidadPagadora,0) : item.CuotaUnidadPagadora,
                        SaldoCapital = item.Capital,
                        CapitalInteresporPagar = item.InteresesCorrientesAPagar,
                        CapitalSaldoTotal = item.DeudaRemanente,
                        SaldoCapitalReducido = item.SaldoCapitalReducido,
                        CuotaLeasing = item.CuotaUnidadPagadora,
                        ValorDeudaRemanente = item.DeudaRemanente,
                        IdSimulacion = simulacionId,
                        IdDetalleSimulacion = Guid.NewGuid(),
                        IdSimulacionCliente = datosPersonales.IdSimulacion,
                        AfectacionCuentaIndividual = item.AfectacionCuentaIndividual,
                        /// nueva extructura
                        InteresPorPagar = item.InteresesCorrientesPorPagar,
                        CapitalPorPagar = item.CapitalPorPagar,
                        CapitalReducido = item.SaldoCapitalReducido,
                        Capitalpagar = item.CapitalAPagar,
                        InteresesPorPagar = item.InteresesCorrientesPorPagar,
                        CapitalSaldo = item.Capital,
                        SaldoTotal = item.Saldo,
                        NoventaCapacidadDePago = (decimal)capacidadPago,
                        ValorMaximoVivienda = (decimal)request.valor,
                        SaldoLeasingTradicional = item.Saldo - item.Capital,
                        SegurosTotal = Math.Round(item.SegurosTotal, 2),
                        Afl_id = request.afl_id,
                        NumeroDocumento = request.documento,
                        ValorFinanciar = item.SaldoLeasingTradicional - (decimal)valorCanonInicial,
                        ViviendaVis = viviendaVis,
                        AhorroVoluntarioContraActual = (decimal)(((double)datosFinancieros.ValorSalario * 8)/100)

                    });
                }
                #endregion
            }
            #region Guardado en BD

            planillaSimulacion[0].AhorroVoluntarioContraActual = 0;
            planillaSimulacion[0].ValorCanonInicial = (decimal)valorCanonInicial;
            planillaSimulacion[0].ValorOpcionCompra = valorOpcionDeCompra; 
            planillaSimulacion[0].factor = factor;
            planillaSimulacion[0].CuotaSectorFinanciero = CuotaSectorFinanciero;
            planillaSimulacion[0].CuotaSectorReal = CuotaSectorReal;
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].RiesgoMinimoTransUnion = (Score == 0) ? 0 : Score;
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].AportesPropios = (decimal)request.ValorViviendaSeguro - (decimal)valorViviendaDeseado;


            if (request.tipoSimularOProspectar == 3)
            {
                await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
                var consultaSMC = await _creditoBusinessLogic.ConsultarSimulacionCliente(request.documento);

                var smc_id = consultaSMC.Data.smc_id;


                ValoresSimulacionAfiliadoDto valoresSimulacion = new ValoresSimulacionAfiliadoDto();
                valoresSimulacion.ValorFinanciar = planillaSimulacion[0].ValorFinanciar;
                valoresSimulacion.NumeroDocumento = request.documento;
                valoresSimulacion.SMS_ID = smc_id;
                valoresSimulacion.RiesgoMinimo = planillaSimulacion[0].RiesgoMinimoTransUnion;
                valoresSimulacion.ValorSectorFinanciero = planillaSimulacion[0].CuotaSectorFinanciero;
                valoresSimulacion.ValorSectorReal = planillaSimulacion[0].CuotaSectorReal;
                valoresSimulacion.Factor = planillaSimulacion[0].factor;

                await _creditoRepository.InsertarValoresSimulacionAfiliado(valoresSimulacion);
            }            
            #endregion
            List<InformacionProyeccion> proyecciones = new List<InformacionProyeccion>();
            foreach (var item in listaInformacionProyeccion)
            {
                proyecciones.Add(item);
            }

            var valoresSimulacionProspeccion = new ValoresSimulacionCreditoReclasificacion
            {
                NUMERO_DOCUMENTO = request.documento,
                TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                SEGURO_VIDA = decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                SEGURO_HOGAR = decimal.Parse(planillaSimulacion[0].SeguroHogar.ToString()),
                SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                SEGURO_VIDA_EXTRAPRIMA = 0,
                TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                TASA_MIXTA_EXTRAPRIMA = 0,
                FACTOR = 0,
                FACTOR_EXTRA_PRIMA = 0,
                PLAZO = plazo,
                VALOR_PRESTAMO = planillaSimulacion[0].ValorFinanciar,
                CUOTA_SIN_SEGURO_HOGAR = canonTradicional - decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                CUOTA_SEGURO_HOGAR = canonTradicional,
                CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                CAPACIDAD_PAGO = (decimal)capacidadPago,
                VALOR_PRESTAMO_SEGUN_CUOTA = 0
            };

            bool guardado = await _creditoRepository
                .InsertValoresSimulacionCredito(valoresSimulacionProspeccion);


            planillaSimulacion[0].LstProyeccion = proyecciones;
            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        #endregion

        #region Credito Leasing Veteranos
        public class Veteranos
        {
            public int NroCuota { get; set; }
            public DateTime FechaCorte { get; set; }
            public DateTime FechaPago { get; set; }
            public decimal Capital { get; set; }
            public decimal IntCte { get; set; }
            public decimal SegVida { get; set; }
            public decimal SegHogar { get; set; }
            public decimal Cuota { get; set; }
            public decimal Saldo { get; set; }
            public decimal SaldoCapitalReducido { get; set; }
            public decimal CapitalAPagar { get; set; }
            public decimal CapitalPorPagar { get; set; }
            public decimal InteresesCorrientesAPagar { get; set; }
            public decimal InteresesCorrientesPorPagar { get; set; }
            public decimal SeguroHogar { get; set; }
            public decimal SeguvoVida { get; set; }
            public decimal SeguroPorPagar { get; set; }
            public decimal CuotaUnidadPagadora { get; set; }
            public decimal AfectacionCuentaIndividual { get; set; }
            public decimal DeudaRemanente { get; set; }
            public decimal BeneficioTasaFresh { get; set; }
            public decimal BeneficioAlivioCuota { get; set; }
            public decimal ValoraCobrarFonvivienda { get; set; }
            public decimal SaldoLeasingTradicional { get; set; }
            public decimal SegurosTotal { get; set; }
        }
        public async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionLeasingVeteranos(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid simulacionId, bool recalcular=false)
        {
            #region DatosSimulacion
            ValoresProyectadosDto valoresProyectadosDto = new ValoresProyectadosDto();
            List<Veteranos> PlanSimulacion = new();
            List<SimulacionDto> planillaSimulacion = new();
            bool bloquearCondicion = false;

            SimulacionDto simulacionDto = new SimulacionDto();
            var equivalenciasSimuladoCredito = (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento)).Data;
            var cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto { TipoIdentificacion = request.tipoIdentificacion, NumeroDocumento = request.documento })).Data;

            var usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(request.Usr_id);

            var correoLogueado = usuarioEncontrado == null ? "" : usuarioEncontrado.USR_USERNAME;

            var tipoActorRecursos = recursos.IdTipoActor;
            var descripcionVinculacion = "";
            foreach (var item in recursos.TiposVinculacion)
            {
                if (tipoActorRecursos == item.IdTipoActor)
                {
                    descripcionVinculacion = item.DescripcionTipoActor;
                }
            }

            var TasaEATipoCredito = await _creditoRepository.ObtenerTasasSimuladorByTipoCredito(descripcionVinculacion, 1);

            var parametrosGenerales = await this.ObtenerParametrosGeneralesSimulacion(new ParametrosGeneralesRequest
            {
                SimuladorId = (int)EnumSimuladorId.LEASING_VETERANOS,
                TipoAfiliacionId = Convert.ToInt32(cliente.TipoAfiliacion),
                UnidadEjecutoraId = cliente.IdUnidadEjecutora,
                DepartamentoInmuebleId = datosPersonales.DpdIdInmueble
            });
            if (parametrosGenerales.Status != string.Empty)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = parametrosGenerales.Status
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }

            //parametrosGenerales.Tasas.EfectivaAnual = TasaEATipoCredito.SIM_TASA_EA / 100;

            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;
            var datosSimulacionPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.documento));
            var tiposVivienda = (await _catalogsBusinessLogic.ObtenerTiposDeVivienda()).Data;
            //var proyeccionactual = (await _creditoBusinessLogic.ObtenerProyeccionCliente(cliente.GradoId.ToString())).Data;
            //var simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(proyeccionactual.Id.ToString())).Data;

            //if (proyeccionactual.Id == 7 || proyeccionactual.Id == 8 || proyeccionactual.Id == 9)
            //{
            //    proyeccionactual.salariobasico = (int)datosFinancieros.ValorSalario;
            //}

            int tipoAfiliacionId = int.Parse(cliente.TipoAfiliacion.ToString());
            var viviendaVis = (request.valor / (int)parametrosGenerales.SalarioMinimo) < (double)parametrosGenerales.LimiteVIS ? "VIS" : "NO VIS";

            var valorInmuebleAlmacenado = request.valor > (double)request.avaluoInmueble ? request.valor : (double)request.avaluoInmueble;


            request.valor = request.valorInicialSimuladoInmueble > 0 ? (double)request.valorInicialSimuladoInmueble : valorInmuebleAlmacenado;


            #endregion
            #region Paso1
            var valorViviendaDeseado = request.valor;
            //Este Valor se llena en el paso 7
            var valorViviendaMaximo = 0M;
            #endregion
            #region Paso3
            var tasaEfectivaAnualMensualizada = Math.Pow((double)(parametrosGenerales.Tasas.EfectivaAnual + 1), (1.0 / 12)) - 1;
            var porcentajeCanonInicial = 0.0;
            var porcentajeOpcionCompra = 0.0;
            var tasaCredito = parametrosGenerales.Tasas.EfectivaAnual;
            var tasaPeriodicaNMV = (decimal)tasaEfectivaAnualMensualizada;
            var tasaSeguroVida = parametrosGenerales.Tasas.SeguroVida;
            var tasaSeguroHogar = parametrosGenerales.Tasas.SeguroHogar;

            var datoSeguroSimulacion = await _creditoRepository.ConsultarSeguroSimulacionCredito(request.documento);

            if (datoSeguroSimulacion.SSC_CONVENIO_VIDA == "N")
            {
                tasaSeguroVida = 0;
            }

            if (datoSeguroSimulacion.SSC_CONVENIO_HOGAR == "N")
            {
                tasaSeguroHogar = 0;
            }

            if (request.CanonInicial >= parametrosGenerales.Tasas.Min_Canon_Inicial && request.CanonInicial <= parametrosGenerales.Tasas.Max_Canon_Inicial)//Revisar donde se puede obtener para no dejarlo quemado
            {
                porcentajeCanonInicial = (double)(request.CanonInicial / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "CANON INICIAL FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser entre el 5% y el 15%
            }
            if (request.porOpcionCompra <= parametrosGenerales.Tasas.Max_Opcion_Compra)
            {
                porcentajeOpcionCompra = (double)(request.porOpcionCompra / 100);
            }
            else
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA FUERA DE RANGO"
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };//Revisar como enviar que debe ser menor a 30%
            }

            var plazo = 0;
            var obtenerInfoCliente = await _requestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento);
            if (request.plazo != 0 && (request.plazo >= parametrosGenerales.MinimoPlazoMeses && request.plazo <= parametrosGenerales.MaximoPlazoMeses))
            {
                plazo = request.plazo;
            }
            else
            {
                plazo = parametrosGenerales.MaximoPlazoMeses;
            }
            var AnosPlazo = plazo % 12 == 0 ? (int)(plazo / 12) : (int)(plazo / 12) + 1;
            #endregion
            #region Paso4
            var factor = (decimal)(tasaPeriodicaNMV * (decimal)Math.Pow((1 + (double)tasaPeriodicaNMV), plazo) / (decimal)(Math.Pow(1 + (double)tasaPeriodicaNMV, plazo) - 1));
            var valorCanonInicial = recalcular ? request.valorPrestamo * porcentajeCanonInicial : request.valor * porcentajeCanonInicial;
            #endregion
            #region Paso5
            var VrCredito = 0.0;
            var VrCreditoSegVida = 0.0;
            if (request.valorPrestamo > 0 && request.valorPrestamo > (valorViviendaDeseado - valorCanonInicial))
            {
                valorCanonInicial = recalcular ? valorCanonInicial : valorViviendaDeseado - request.valorPrestamo;
                VrCredito = request.valorPrestamo;

            }
            else
            {
                VrCredito = (double)(valorViviendaDeseado - valorCanonInicial);
            }
            VrCreditoSegVida = ((double)(request.avaluoInmueble) <= valorViviendaDeseado) ? (double)valorViviendaDeseado : (double)(request.avaluoInmueble);

            var valorSeguroVida = recalcular ? (decimal)(valorViviendaDeseado - valorCanonInicial) * tasaSeguroVida : (decimal)(VrCredito) * tasaSeguroVida;

            var VrCreditoSegHogar = 0.0; 
            if (request.avaluoInmueble > 0)
            {
                VrCreditoSegHogar = (double)request.avaluoInmueble > (double)valorInmuebleAlmacenado ? (double)request.avaluoInmueble : (double)valorInmuebleAlmacenado;
            }
            else
            {
                VrCreditoSegHogar = (double)request.ValorViviendaSeguro > 0 ? (double)request.ValorViviendaSeguro : (double)valorViviendaDeseado;
            }
            //var valorSeguroHogar = (decimal)VrCreditoSegHogar * tasaSeguroHogar;
            var valorSeguroHogar = recalcular ? (decimal)request.ValorViviendaSeguro*tasaSeguroHogar : (decimal)request.valor * tasaSeguroHogar;
            var VrCuotaSinOpcC = ((decimal)(valorViviendaDeseado - valorCanonInicial) * factor) + valorSeguroHogar + valorSeguroVida;
            var valorOpcionDeCompra = (decimal)(valorViviendaDeseado * porcentajeOpcionCompra);
            if (valorOpcionDeCompra > (decimal)VrCredito && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "OPCION DE COMPRA MAYOR AL VALOR DEL CREDITO", 
                    RespuestaRadicado = false
                });

                return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
            }
            var VrDescOpcCompra = (valorOpcionDeCompra / (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), plazo)) * factor;
            var canonTradicional = VrCuotaSinOpcC - VrDescOpcCompra;


            #region Ahorros y Cesantias

            await _creditoRepository.LimpiarRecursosCongelados(simulacionId, request.documento);

            //decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            //decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            //decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            //decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;
            decimal ahorroProgramado = 0;
            decimal cesantiasProgramadas = 0;
            decimal intCesantiasProgramadas = 0;
            decimal intAhorroProgramados = 0;
            decimal RecursosCuentaIndiv = ahorroProgramado + cesantiasProgramadas + intCesantiasProgramadas + intAhorroProgramados;
            var MaximoUsar = RecursosCuentaIndiv * (decimal)(1 - 0.15);

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING VETERANOS"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING VETERANOS"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING VETERANOS"
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = simulacionId,
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.documento,
                    CGR_TIPO_CREDITO = "LEASING VETERANOS"
                }
            };

            

            foreach (var recurso in recursosCongelados)
            {
                await _creditoRepository.InsertarRecursosCongelados(recurso);
            }


            await _creditoRepository.LimpiarConceptosCongelados(simulacionId, request.documento);


            List<string> descripcionesConceptos = new List<string>
            {
                "AHORROS OBLIGATORIOS",
                "AHORROS RETROACTIVOS",
                "AHORROS VOLUNTARIOS",
                "INTERESES APORTES",
                "CESANTÍAS",
                "CESANTÍAS RETROACTIVOS",
                "INTERESES CESANTÍAS"
            };

            List<ConceptosCongeladosProspeccion> listaCongeladosProspeccion = new List<ConceptosCongeladosProspeccion>();

            foreach (var cuentas in recursos.Cuentas)
            {
                foreach (var conceptos in cuentas.Conceptos)
                {
                    if (descripcionesConceptos.Contains(conceptos.ConceptoDescripcion))
                    {
                        listaCongeladosProspeccion.Add(new ConceptosCongeladosProspeccion
                        {
                            CON_DESCRIPCION = conceptos.ConceptoDescripcion,
                            CON_VALOR = conceptos.saldo,
                            SLS_ID = simulacionId,
                            CON_NUMERO_DOCUMENTO = request.documento
                        });
                    }
                }
            }



            foreach (var concepto in listaCongeladosProspeccion)
            {
                await _creditoRepository.InsertarConceptosCongelados(concepto);
            }

            #endregion
            var RecursosCI = 0M;
            if (MaximoUsar > (decimal)valorCanonInicial)
            {
                RecursosCI = (decimal)valorCanonInicial;
            }
            else
            {
                RecursosCI = RecursosCuentaIndiv;
            }
            decimal RecursosPropios = (decimal)valorCanonInicial - RecursosCI;
            #region RecursosProyectados
            var antiguedadcalculo = Math.Round(((decimal)datosPersonales.Cuotas / 12), 1);
            var antiguedadaños = (antiguedadcalculo - (int)antiguedadcalculo > 0.5m) ? (int)Math.Ceiling(antiguedadcalculo) : (int)Math.Floor(antiguedadcalculo);
            #region Factores
            decimal factorIncremento = parametrosGenerales.Factores.FactorIncremento / 100;
            decimal factorIncrementoSM = parametrosGenerales.Factores.FactorIncrementoSubsidio / 100;
            decimal factorAhorro = request.TasaVoluntariaAportes ? (parametrosGenerales.Factores.FactorAhorro + cliente.TasaVoluntariaAportes) / 100 : parametrosGenerales.Factores.FactorAhorro / 100;
            decimal factorInflacion = parametrosGenerales.Factores.FactorInflacion / 100;
            decimal factorCesantias = parametrosGenerales.Factores.FactorCesantias / 100;
            decimal factorInteresesCesantias = 12.0M / 100;
            #endregion
            #region Valores Para Proyeccion
            #region Uso Canon Inicial
            var valorIntCesantiasCanonInicial = intCesantiasProgramadas < (decimal)valorCanonInicial ? intCesantiasProgramadas : (decimal)valorCanonInicial;
            var valorIntAhorroCanonInicial = intAhorroProgramados < ((decimal)valorCanonInicial - intCesantiasProgramadas) ? intAhorroProgramados : ((decimal)valorCanonInicial - intCesantiasProgramadas);
            var valorCesantiasCanonInicial = cesantiasProgramadas < ((decimal)valorCanonInicial - intAhorroProgramados - intCesantiasProgramadas) ? cesantiasProgramadas : ((decimal)valorCanonInicial - intAhorroProgramados - intCesantiasProgramadas);
            var valorAhorroCanonInicial = ahorroProgramado < ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial)) ? ahorroProgramado : ((decimal)valorCanonInicial - (valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial));

            #endregion
            #region Actualizacion Ahorros y Cesantias Para Proyeccion
            ahorroProgramado = ahorroProgramado - valorAhorroCanonInicial;
            cesantiasProgramadas = cesantiasProgramadas - valorCesantiasCanonInicial;
            intCesantiasProgramadas = intCesantiasProgramadas - valorIntCesantiasCanonInicial;
            intAhorroProgramados = intAhorroProgramados - valorIntAhorroCanonInicial;
            #endregion
            //#region Proyeccion
            ////Ejercicio valor i del for en 0
            //decimal sueldo = simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() != 0 ? simcargo.Where(x => x.Id == proyeccionactual.Id).Select(x => x.salariobasico).FirstOrDefault() : proyeccionactual.salariobasico;
            //decimal SMProyectado = (decimal)parametrosGenerales.SalarioMinimo;
            //decimal AhorrosAnt = ahorroProgramado;
            //decimal AcumuladoAhorros = ahorroProgramado;
            //decimal IntAhorrosAnt = intAhorroProgramados;
            //decimal AcumuladoIntAhorros = intAhorroProgramados;
            //decimal CesantiasAnt = cesantiasProgramadas;
            //decimal AcumuladoCesantias = cesantiasProgramadas;
            //decimal IntCesantiasAnt = intCesantiasProgramadas;
            //decimal AcumuladoIntCesantias = intCesantiasProgramadas;
            //decimal TOTAcumuladoAhorros = 0M;
            //decimal TOTAcumuladoCesantias = 0M;
            //double sueldoProyeccion = 0;
            //var countAnios = 1;
            //var countCategoria = 0;
            //var ctg = proyeccionactual.Id;
            //var antiguedadañosTemp = antiguedadaños;
            //string StInfoProy = string.Empty;
            ////while (antiguedadañosTemp >= simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault())
            ////{
            ////    countCategoria++;
            ////    countAnios = antiguedadañosTemp - simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
            ////    antiguedadañosTemp = antiguedadañosTemp - simcargo.Where(x => x.Id == ctg).Select(X => X.AñoAscenso).FirstOrDefault();
            ////    if (simcargo.Any(x => x.Id == ctg + 1))
            ////    {
            ////        ctg = proyeccionactual.Id + countCategoria;
            ////        simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
            ////        sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault();
            ////    }
            ////}
            //List<string> LstProyeccion = new List<string>();
            //StInfoProy = "Años Antiguedad: " + antiguedadaños;
    

            //List<InformacionProyeccion> listaInformacionProyeccion = new List<InformacionProyeccion>();

            //InformacionProyeccion informacionProyeccion = new InformacionProyeccion();

            //informacionProyeccion.IdProyeccion = 0;
            //informacionProyeccion.Cargo = simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString();
            //informacionProyeccion.AniosCargo = countAnios;
            //informacionProyeccion.Sueldo =(double)sueldo;
            //informacionProyeccion.AhorroProgramado = ahorroProgramado;
            //informacionProyeccion.InteresesAhorroProgramado = intAhorroProgramados;
            //informacionProyeccion.Cesantias = cesantiasProgramadas;
            //informacionProyeccion.InteresesCesantias = intCesantiasProgramadas;
            //informacionProyeccion.AniosAntiguedad = antiguedadaños;

            //listaInformacionProyeccion.Add(informacionProyeccion);

            ////for (int i = 1; i <= AnosPlazo; i++)
            ////{
            ////    countAnios++;
            ////    var aumentosueldo = Math.Pow((1 + (double)factorIncremento), i);
            ////    sueldoProyeccion = Math.Round(sueldo * aumentosueldo, 0, MidpointRounding.AwayFromZero);
            ////    decimal Ahorro = Math.Round((decimal)sueldoProyeccion * factorAhorro * 12, 0, MidpointRounding.AwayFromZero);
            ////    AcumuladoAhorros += Ahorro;
            ////    AcumuladoIntAhorros += i > 1 ? Math.Round(AhorrosAnt * factorInflacion, 0, MidpointRounding.AwayFromZero) : 0;
            ////    AhorrosAnt = Ahorro;
            ////    decimal Cesantias = Math.Round((decimal)sueldoProyeccion * factorCesantias * 12, 0, MidpointRounding.AwayFromZero);
            ////    AcumuladoCesantias += Cesantias;
            ////    AcumuladoIntCesantias += i > 1 ? Math.Round(CesantiasAnt * factorInflacion, 0, MidpointRounding.AwayFromZero) : 0;
            ////    CesantiasAnt = Cesantias;
            ////    SMProyectado = simcargo.Where(x => x.Id == ctg).Select(x => x.ProyectaSM).FirstOrDefault() ? SMProyectado + Math.Round(SMProyectado * factorIncrementoSM, 0, MidpointRounding.AwayFromZero) : parametrosGenerales.SalarioMinimo;
            ////    StInfoProy = "IdProyeccion: " + i + ",Cargo: " + simcargo.Where(x => x.Id == ctg).Select(x => x.Descripcion).FirstOrDefault().ToString() + ",Años Cargo: " + countAnios + ",Sueldo: " + sueldoProyeccion + ",Ahorro: " + AcumuladoAhorros + ", InteresAhorro: " + AcumuladoIntAhorros + ", Cesantias: " + AcumuladoCesantias + ", InteresesCesantias: " + AcumuladoIntCesantias;
            ////    if (simcargo.Where(x => x.Id == ctg).Select(x => x.AñoAscenso).FirstOrDefault() == countAnios)
            ////    {
            ////        countCategoria++;
            ////        if (simcargo.Any(x => x.Id == ctg + 1))
            ////        {
            ////            ctg = proyeccionactual.Id + countCategoria;
            ////            simcargo = (await _creditoBusinessLogic.ObtenerSimCargo(ctg.ToString())).Data;
            ////            sueldo = simcargo.Where(x => x.Id == ctg).Select(x => x.salariobasico).FirstOrDefault();
            ////            countAnios = 0;
            ////        }
            ////    }
            ////    LstProyeccion.Add(StInfoProy);
            ////}
            //#endregion
            #region Valores Proyectados
            //var subsidioCategoria = (await _catalogsBusinessLogic.ObtenerSimulacionCargoPorCategoria(cliente.CategoriaId, ctg)).Data.FactorSubsidio;
            ////var subsidiovivienda = Math.Round(((double)SMProyectado * subsidioCategoria), 0, MidpointRounding.AwayFromZero);
            //TOTAcumuladoAhorros = Math.Round(AhorrosAnt * factorInflacion, 0, MidpointRounding.AwayFromZero);
            //TOTAcumuladoCesantias = Math.Round(AcumuladoCesantias + AcumuladoIntCesantias, 0, MidpointRounding.AwayFromZero);
            //var RecursosCanonInicial = valorIntCesantiasCanonInicial + valorIntAhorroCanonInicial + valorCesantiasCanonInicial + valorAhorroCanonInicial;
            var TotalOpcionCompra =0;
            var DivCuoEquiVF = Math.Pow((double)(1 + tasaPeriodicaNMV), plazo);
            var CuotaEquivalenteVF = Math.Round((TotalOpcionCompra * factor) / (decimal)DivCuoEquiVF, 0, MidpointRounding.AwayFromZero);
            var CanonReducido = VrCuotaSinOpcC - CuotaEquivalenteVF;
            var CapitalReducido = Math.Round((CanonReducido - valorSeguroHogar - valorSeguroVida) / factor, 0, MidpointRounding.AwayFromZero);
            #region Valores a Guardar
            //valoresProyectadosDto.Svp_Id = Guid.NewGuid();
            //valoresProyectadosDto.Sls_Id = datosPersonales.IdSimulacion;
            //valoresProyectadosDto.Svp_EA = parametrosGenerales.Tasas.EfectivaAnual;
            //valoresProyectadosDto.Svp_Cesantias = TOTAcumuladoCesantias;
            //valoresProyectadosDto.Svp_Sueldo = (decimal)sueldoProyeccion;
            //valoresProyectadosDto.Svp_Ahorros = TOTAcumuladoAhorros;
            //valoresProyectadosDto.Svp_Subsidio = (decimal)0;
            //var ValoresProyectados = await _creditoBusinessLogic.GuardarValoresProyectados(valoresProyectadosDto);
            #endregion
            #endregion
            #endregion
            #endregion
            #endregion
            #region Paso6
            var totalIngresos = (double)datosFinancieros.TotalIngresos;
            var totalEgresos = (double)datosFinancieros.TotalEgresos;
            var capacidadPago = (totalIngresos * 0.5) - totalEgresos;
            #endregion
            #region Paso7
            var Valor1 = (decimal)(valorViviendaDeseado * (1 - porcentajeCanonInicial - porcentajeOpcionCompra));
            var Valor2 = ((decimal)capacidadPago + CuotaEquivalenteVF) / (factor + (tasaSeguroHogar / (1 / (decimal)porcentajeCanonInicial)));
            var VrCredito2 = Math.Min(Valor1, Valor2);
            valorViviendaMaximo = VrCredito2 / (1 - (decimal)(porcentajeCanonInicial + porcentajeOpcionCompra));
            //if (bloquearCondicion && Math.Round(valorViviendaMaximo, 0, MidpointRounding.AwayFromZero) < Math.Round((decimal)valorViviendaDeseado, 0, MidpointRounding.AwayFromZero))
            //{
            //    request.valor = (double?)valorViviendaMaximo;
            //    return await CrearSimulacionLeasingVeteranos(request, datosPersonales, simulacionId);
            //}
            #endregion
            int vinculacion = recursos.IdTipoActor;
            string estadoCuenta = "";

            if (vinculacion != 14)
            {


                //vinculacion 16 y 2 pertecenten a FUNCIONARIO AFILIADO ADMINISTRACION CESANTÍAS y AFILIADO ADMINISTRACION CESANTIAS respectivamente
                foreach (var cuentas in recursos.Cuentas)
                {
                    if (vinculacion == 16 || vinculacion == 2)
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;

                        }
                    }
                    else
                    {
                        if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA")
                        {
                            estadoCuenta = cuentas.EstadoCuenta;

                        }
                    }
                }

                if (estadoCuenta == "MOROSA")
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "El afiliado presenta cuenta con estado MOROSA"
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }


            if (CanonReducido < parametrosGenerales.ValorMinimoCuota)
            {
                planillaSimulacion.Clear();
                planillaSimulacion.Add(new SimulacionDto
                {
                    Sobrante = "La cuota debe ser mayor a la mínima permitida (" + parametrosGenerales.ValorMinimoCuota.ToString("C0", new CultureInfo("es-CO")) + ") por la entidad, intente nuevamente con un valor mayor del inmueble.",
                    RespuestaRadicado = false
                });

                return new Response<IEnumerable<SimulacionDto>>
                {
                    Data = planillaSimulacion
                };
            }
            ValidacionCreditoProspeccion validacionesExcepciones = new ValidacionCreditoProspeccion();

            if (validacionesExcepciones == null)
            {
                validacionesExcepciones.VCP_ESTADO = false;
                validacionesExcepciones.VCP_NUMERO_DOCUMENTO = request.documento;

                await _creditoRepository.InsertarActualizarValidacionCreditoProspeccion(validacionesExcepciones);
            }

            ValidacionCreditoProspeccion validacionesExcepcion = await _creditoRepository.ConsultarValidacionCreditoProspeccion(request.documento);

            double totalObligaciones = 0;
            string validacionEndeudamientoGlobal = ""; 
            #region EndeudamientoGlobal
            if (parametrosGenerales.ActiveTU && request.tipoSimularOProspectar == 3)
            {
                try
                {
                    #region ConsumoTU
                    ProspeccionTransUnionRequestDto prospeccionTransUnionRequestDto = new ProspeccionTransUnionRequestDto()
                    {
                        Apellidos = recursos.PrimerApellido,
                        FechaExpedicionIdentificacion = recursos.FechaExpedicionDocumento.ToString(),
                        Identificacion = request.documento,
                        SalarioMensual = Math.Round((decimal)totalIngresos),
                        TipoIdentificacion = request.tipoIdentificacion,
                        Telefono = recursos.Telefono,
                        TraerHistorialCrediticio = "Y",
                        SLS_ID = simulacionId
                    };

                    if (prospeccionTransUnionRequestDto.Telefono.Length < 10 || prospeccionTransUnionRequestDto.Telefono == null)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el teléfono"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    if (prospeccionTransUnionRequestDto.SalarioMensual <= 0)
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = "La consulta no se puede hacer en Transunion, por favor actualiza en la hoja de vida el salario"
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }

                    var ProspeccionTransUnion = await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(prospeccionTransUnionRequestDto);
                    #endregion
                    ReporteSimulacion60PorCientoDto reporteSimulacion60PorCientoDto = new ReporteSimulacion60PorCientoDto();
                    reporteSimulacion60PorCientoDto.NumeroDocumento = request.documento;
                    double TOT = totalIngresos - totalEgresos;
                    #region Reporte 60%
                    reporteSimulacion60PorCientoDto.salarioTotal = String.Format("{0:N0}", totalIngresos);
                    reporteSimulacion60PorCientoDto.deducciones = String.Format("{0:N0}", totalEgresos);
                    reporteSimulacion60PorCientoDto.salarioNeto = String.Format("{0:N0}", TOT);
                    reporteSimulacion60PorCientoDto.capacidadPago = String.Format("{0:N0}", capacidadPago);
                    reporteSimulacion60PorCientoDto.canonCuotaMensual = String.Format("{0:N0}", CanonReducido);
                    #endregion
                    if (!string.IsNullOrEmpty(ProspeccionTransUnion.Data.response))
                    {
                        var ReporteTrans = await _reportesBusinessLogic.ReporteTransUnionPropiedades(ProspeccionTransUnion.Data.response);
                        #region Validacion Score y Viabilidad
                        if (!ReporteTrans.IsSuccess)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no viable por consulta TransUnion."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        
                        
                        #endregion
                        #region GuardadoReporteTransUnion
                        using (var memoryStream = new MemoryStream(ReporteTrans.Data.ArchivoBytes))
                        {
                            IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            await SaveReportTU(formFile, simulacionId);
                        }
                        #endregion
                        #region Reclasificacion
                        IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(1, request.documento);
                        var sumaSaldoObligacion = prospeccionResult
                        .Where(linea => linea.OLN_ESTADO_RECALCULAR==false)
                        .Sum(linea => linea.OLN_SALDO_OBLIGACION);
                        #endregion
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.cuotaSectorFinanSoliAse = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorFinanciero);
                        reporteSimulacion60PorCientoDto.cuotaSectorReal = String.Format("{0:N0}", ReporteTrans.Data.cuotasSectorReal);
                        #endregion
                        var EndeudamientoG_Sin_Cuota = ReporteTrans.Data.cuotasSectorReal + ReporteTrans.Data.cuotasSectorFinanciero - (double)(sumaSaldoObligacion);
                        totalObligaciones = EndeudamientoG_Sin_Cuota; 
                        var EndeudamientoG_Sin_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoG_Sin_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        var EndeudamientoActual_Cuota = (double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos - (double)sumaSaldoObligacion;
                        var EndeudamientoActual_Cuota_Porc = Math.Round(await CalcularEndeudamiento(EndeudamientoActual_Cuota, totalIngresos) * 100, 0, MidpointRounding.AwayFromZero);
                        Score = ReporteTrans.Data.riesgoMinimo;
                        CuotaSectorFinanciero = (decimal)ReporteTrans.Data.cuotasSectorFinanciero;
                        CuotaSectorReal = (decimal)ReporteTrans.Data.cuotasSectorReal;
                        EndeudamientoSCH = EndeudamientoG_Sin_Cuota_Porc;
                        EndeudamientoCCH = EndeudamientoActual_Cuota_Porc;
                        #region Reporte 60%
                        reporteSimulacion60PorCientoDto.totalObliAntesCredito = String.Format("{0:N0}", EndeudamientoG_Sin_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalSinCanon = (EndeudamientoG_Sin_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.totalObliNuevoCredito = String.Format("{0:N0}", EndeudamientoActual_Cuota);
                        reporteSimulacion60PorCientoDto.EndeudaGlobalCanon = (EndeudamientoActual_Cuota_Porc).ToString() + "%";
                        reporteSimulacion60PorCientoDto.disponibleLibranza = String.Format("{0:N0}", (capacidadPago - (double)CanonReducido));
                        reporteSimulacion60PorCientoDto.controlCapacidadPago = capacidadPago >= (double)CanonReducido ? "SI APLICA" : "NO APLICA";
                        #endregion

                        var ControNivelEndeudamiento_Cuota_Porc = EndeudamientoActual_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";
                        var ControNivelEndeudamiento_Sin_Cuota_Porc = EndeudamientoG_Sin_Cuota_Porc > (double)parametrosGenerales.PorcentajeMinimoEndeudamiento ? "NO APLICA" : "SI APLICA";

                       

                        DatosEndeudamientoProspeccionDto datosEndeudamientoProspeccion = new DatosEndeudamientoProspeccionDto();
                        datosEndeudamientoProspeccion.NumeroDocumento = datosPersonales.NumeroDocumento;
                        datosEndeudamientoProspeccion.SalarioUltimoMes = (decimal)totalIngresos;
                        datosEndeudamientoProspeccion.Descuentos = (decimal)totalEgresos;
                        datosEndeudamientoProspeccion.SalarioNeto = (decimal)TOT;
                        datosEndeudamientoProspeccion.CapacidadPago = (decimal)capacidadPago;
                        datosEndeudamientoProspeccion.CanonMensual = CanonReducido;
                        datosEndeudamientoProspeccion.SectorFinanciero = CuotaSectorFinanciero - sumaSaldoObligacion;
                        datosEndeudamientoProspeccion.SectorReal = CuotaSectorReal;
                        datosEndeudamientoProspeccion.Score = Score;
                        datosEndeudamientoProspeccion.DisponibleLibranza = ((decimal)capacidadPago - CanonReducido);
                        datosEndeudamientoProspeccion.EndeudamientoGlobalCuota = (decimal)EndeudamientoActual_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalCuota = ControNivelEndeudamiento_Cuota_Porc;
                        datosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota = (decimal)EndeudamientoG_Sin_Cuota;
                        datosEndeudamientoProspeccion.ControlNivelEndeudamientoGlobalSinCuota = ControNivelEndeudamiento_Sin_Cuota_Porc;
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalCuota = EndeudamientoActual_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.PorcentajeEndeudamientoGlobalSinCuota = EndeudamientoG_Sin_Cuota_Porc.ToString();
                        datosEndeudamientoProspeccion.TotalCanonDeducciones = ((decimal)totalEgresos + CanonReducido +(decimal)totalObligaciones);
                        datosEndeudamientoProspeccion.DecisionHomologada = ReporteTrans.Data.descripcionDecision;
                        await _creditoRepository.InsertarDatosEndeudamientoProspeccion(datosEndeudamientoProspeccion);

                        var valoresSimulacionProspeccionInsert = new ValoresSimulacionCreditoReclasificacion
                        {
                            NUMERO_DOCUMENTO = request.documento,
                            TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                            TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                            SEGURO_VIDA = valorSeguroVida,
                            SEGURO_HOGAR = valorSeguroHogar,
                            SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                            SEGURO_VIDA_EXTRAPRIMA = 0,
                            TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                            TASA_MIXTA_EXTRAPRIMA = 0,
                            FACTOR = 0,
                            FACTOR_EXTRA_PRIMA = 0,
                            PLAZO = plazo,
                            VALOR_PRESTAMO = (decimal)request.valor - (decimal)valorCanonInicial,
                            CUOTA_SIN_SEGURO_HOGAR = CanonReducido - valorSeguroVida,
                            CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            CUOTA_SEGURO_HOGAR = CanonReducido,
                            CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                            VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                            VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                            CAPACIDAD_PAGO = (decimal)(capacidadPago),
                            VALOR_PRESTAMO_SEGUN_CUOTA = (decimal)request.valor
                        };

                        await _creditoRepository.InsertValoresSimulacionCredito(valoresSimulacionProspeccionInsert);

                        if (ReporteTrans.Data.fechaPermanenciaVigente != null && ReporteTrans.Data.fechaPermanenciaVigente != "" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = ReporteTrans.Data.fechaPermanenciaVigente
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }
                        if (ReporteTrans.Data.totalObligacionMora > 0 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "NO VIABLE: Motivo evaluación de políticas (por presentar deuda en mora)"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!recalcular && request.valorPrestamo > 0 && reporteSimulacion60PorCientoDto.controlCapacidadPago == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                            });

                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if ((ReporteTrans.Data.descripcionDecision.Equals("PENDIENTE") && ControNivelEndeudamiento_Cuota_Porc == "NO APLICA") && request.Usr_id == null && Score <= parametrosGenerales.Score_Min_TU)
                        {
                            planillaSimulacion.Clear();

                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "Para continuar con el proceso, por favor acérquese a un punto de atención",
                                RespuestaRadicado = false

                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (!ReporteTrans.Data.estadoDesicion && request.Usr_id == null)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No es viable, por favor acercarse a un punto de atención."
                            });
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }

                        if (ReporteTrans.Data.riesgoMinimo <= parametrosGenerales.Score_Min_TU && !validacionesExcepcion.VCP_ESTADO)
                        {
                            planillaSimulacion.Clear();
                            planillaSimulacion.Add(new SimulacionDto
                            {
                                Sobrante = "No se puede presentar una simulación, no cumple con el score minimo: " + parametrosGenerales.Score_Min_TU,
                                RiesgoMinimoTransUnion = ReporteTrans.Data.riesgoMinimo
                            });
                            // planillaSimulacion[0].RiesgoMinimoTransUnion = 
                            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                        }


                        #region Validacion y Credito segun endeudamiento Global
                        if (bloquearCondicion && Math.Round(EndeudamientoActual_Cuota_Porc, 0, MidpointRounding.AwayFromZero) > ObjetivoEndeudameinto)
                        {
                            //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto)
                            //{
                                try
                                {
                                    CanonReducido = Math.Round((decimal)await CalcularCuotaNueva(ObjetivoEndeudameinto, 0, EndeudamientoActual_Cuota, 0, totalIngresos), 0, MidpointRounding.AwayFromZero) - (decimal)EndeudamientoG_Sin_Cuota - (decimal)totalEgresos;
                                    EndeudamientoActual_Cuota = await CalcularEndeudamiento((double)CanonReducido + EndeudamientoG_Sin_Cuota + totalEgresos, totalIngresos);
                                    VrCredito = (double)(((CanonReducido - valorSeguroVida - valorSeguroHogar) / factor) + valorOpcionDeCompra);
                                    request.valorPrestamo = VrCredito;
                                    #region Reporte 60%
                                    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = String.Format("{0:N0}", Math.Round(CanonReducido, 0, MidpointRounding.AwayFromZero));
                                    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = String.Format("{0:N0}", ((double)CanonReducido + EndeudamientoG_Sin_Cuota));
                                    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = Math.Round((EndeudamientoActual_Cuota * 100), 0, MidpointRounding.AwayFromZero).ToString() + "%";
                                    //if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc <= ObjetivoEndeudameinto)
                                    //{
                                    //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                    //}
                                    //else
                                    //{
                                    //    if (EndeudamientoG_Sin_Cuota_Porc <= ObjetivoEndeudameinto && EndeudamientoActual_Cuota_Porc > ObjetivoEndeudameinto)
                                    //    {
                                    //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NUEVO VALOR CUOTA";
                                    //    }
                                    //    else
                                    //    {
                                    //        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    //    }
                                    //}
                                    #endregion
                                    if (Math.Round(VrCredito, 0, MidpointRounding.ToZero) <= 0)
                                    {
                                        reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                        planillaSimulacion.Clear();
                                        planillaSimulacion.Add(new SimulacionDto
                                        {
                                            Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 80%"
                                        });
                                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    }
                                    valorViviendaMaximo = (decimal)(VrCredito / (1 - (porcentajeCanonInicial + porcentajeOpcionCompra)));
                                    request.valor = (double)valorViviendaMaximo;
                                    ConteoTU++;
                                    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = ConteoTU > 5 ? "NO APLICA" : "SI APLICA";
                                    var datos = _creditoRepository.ConsultarDatosEndeudamientoProspeccion(request.documento).Result;

                                    
                                    double toleranciaIngresosTotales = (double)(datos.EDP_SALARIO_TOTAL) * 0.592;
                                    var plazoValorMaximo = plazo;
                                    double diferenciaToleranciaDeducciones = toleranciaIngresosTotales - (double)datos.EDP_SECTOR_FINANCIERO - (double)datos.EDP_SECTOR_REAL - (double)datos.EDP_DEDUCCIONES;
                                    var valorMaximoNumerador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) - 1;
                                    var valorMaximoDenomirador = Math.Pow(((double)parametrosGenerales.Tasas.Mixta + 1), plazo) * (double)parametrosGenerales.Tasas.Mixta;
                                    var factorValorMaximo = valorMaximoNumerador / valorMaximoDenomirador;
                                    var prestamosMaximo = diferenciaToleranciaDeducciones * factorValorMaximo;
                                    double valorMaximoCalculado = prestamosMaximo / 1000000;
                                    double valorMaximo = Math.Round(valorMaximoCalculado, 0, MidpointRounding.ToZero) * 1000000;

                                    if (valorMaximo <= 13000000 && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                                    {
                                        planillaSimulacion.Add(new SimulacionDto
                                        {
                                            Sobrante = "Su nivel de endudamiento global no le permite acceder a ningún estudio de credito"
                                        });

                                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    }

                                    //if (ControNivelEndeudamiento_Cuota_Porc == "NO APLICA" && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co")
                                    //{
                                    //    planillaSimulacion.Clear();
                                    //    planillaSimulacion.Add(new SimulacionDto
                                    //    {
                                    //        Sobrante = "Su nivel de endeudamiento global solo le permite acceder a un estudio de máximo " + valorMaximo.ToString("C0", new System.Globalization.CultureInfo("es-CO")) + ", por favor intente nuevamente con una cantidad inferior o igual al valor máximo"
                                    //    });

                                    //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    //}
                              
                                    //if (ConteoTU > 5)
                                    //{
                                    //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                                    //    planillaSimulacion.Clear();
                                    //    planillaSimulacion.Add(new SimulacionDto
                                    //    {
                                    //        Sobrante = "No se puede presentar una simulación, Verificar endeudamiento"
                                    //    });
                                    //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                    //}
                                    //else
                                    //{
                                    //    return await CrearSimulacionLeasingVeteranos(request, datosPersonales, simulacionId);
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    planillaSimulacion.Clear();
                                    planillaSimulacion.Add(new SimulacionDto
                                    {
                                        Sobrante = ex.Message
                                    });

                                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                                }
                            //}
                            //else
                            //{
                            //    #region Reporte 60%
                            //    reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                            //    reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                            //    reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                            //    reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "NO APLICA";
                            //    #endregion
                            //    #region Save Reporte 60%
                            //    var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                            //    using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                            //    {
                            //        IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                            //        await SaveReportTU60(formFile, simulacionId);
                            //    }
                            //    #endregion
                            //    planillaSimulacion.Add(new SimulacionDto
                            //    {
                            //        Sobrante = "No se puede presentar una simulación, se tiene un endeudamiento superior al 60%"
                            //    });
                            //    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                            //}
                        }
                        else
                        {
                          
                                #region Reporte 60%
                                reporteSimulacion60PorCientoDto.nuevoCanonCuotaMensual = "N/A";
                                reporteSimulacion60PorCientoDto.totalObliNuevoCreditoAjustado = "N/A";
                                reporteSimulacion60PorCientoDto.EndeudaGlobalNuevoCanon = "N/A";
                                reporteSimulacion60PorCientoDto.controlNivelEndeudamiento = "SI APLICA";
                                #endregion
                                #region Save Reporte 60%
                                var ReporteTrans60 = await _reportesBusinessLogic.ReporteSimulacion60PorCiento(reporteSimulacion60PorCientoDto);
                                using (var memoryStream = new MemoryStream(ReporteTrans60.Data.ArchivoBytes))
                                {
                                    IFormFile formFile = new FormFile(memoryStream, 0, ReporteTrans60.Data.ArchivoBytes.Length, "archivo", ReporteTrans.Data.FileName);
                                    await SaveReportTU60(formFile, simulacionId);
                                }
                                #endregion
                            
                        }
                        #endregion
                    }
                    else
                    {
                        planillaSimulacion.Clear();
                        planillaSimulacion.Add(new SimulacionDto
                        {
                            Sobrante = ProspeccionTransUnion.Data.detalleError
                        });
                        return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                    }
                }
                catch (Exception ex)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = ex.Message
                    });
                    return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
                }
            }

      

            #endregion
            if (Math.Round(CanonReducido,0,MidpointRounding.AwayFromZero) > (decimal)capacidadPago && correoLogueado != "pruebas.prospeccion@cajahonor.gov.co" || validacionEndeudamientoGlobal == "NO APLICA")
            {
                //decimal factorFinanciero = (decimal)(1 - Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo));
                //decimal factorDescuento = (decimal)Math.Pow((double)(1 + tasaPeriodicaNMV), -plazo);
                //// Numerador
                //decimal capacidadPagoCalculada = 0;

                //if (parametrosGenerales.ObligacionesExternasCapacidadPago)
                //{
                //    capacidadPagoCalculada = ((decimal)totalIngresos / 2) - (decimal)totalObligaciones - (decimal)totalEgresos - valorSeguroHogar;

                //}
                //else
                //{
                //    capacidadPagoCalculada = ((decimal)totalIngresos / 2) - (decimal)totalEgresos - valorSeguroHogar;

                //}

                //decimal valorViviendaMaximoNumerador =
                //    ((((decimal)capacidadPagoCalculada) / (decimal)tasaPeriodicaNMV) * factorFinanciero) +
                //    (TotalOpcionCompra * factorDescuento);

                //// Denominador
                //decimal valorViviendaMaximoDenominador =
                //    (decimal)(1 - porcentajeCanonInicial) +
                //    (
                //        (((1 - (decimal)porcentajeCanonInicial) * tasaSeguroVida))
                //        / tasaPeriodicaNMV
                //    ) * factorFinanciero;

                //// Resultado
                //valorViviendaMaximo = valorViviendaMaximoNumerador / valorViviendaMaximoDenominador;

                valorViviendaMaximo = (decimal)BuscarValorMaximoFinanciar((double)(valorViviendaDeseado - valorCanonInicial), totalIngresos, (double)parametrosGenerales.PorcentajeMinimoEndeudamiento, totalObligaciones);

                decimal valorMaximoPrestarCalculado = Math.Truncate(valorViviendaMaximo / 1000000.0m) * 1000000.0m;

                request.valorPrestamo = (double)valorMaximoPrestarCalculado;

                request.valor = (double)valorMaximoPrestarCalculado;

                if ((double)valorMaximoPrestarCalculado < 0)
                {
                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento global"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }
                //ConteoTU = 0;

                if (ConteoTU < 100)
                {
                    ConteoTU++;

                    return await CrearSimulacionLeasingVeteranos(request, datosPersonales, simulacionId, true);

                }
                else
                {
                    ConteoTU = 0; // Opcional: reiniciar para futuras ejecuciones

                    planillaSimulacion.Clear();
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Sobrante = "Su solicitud no es viable, debido al control por endeudamiento de nómina"
                    });

                    return new Response<IEnumerable<SimulacionDto>>
                    {
                        Data = planillaSimulacion
                    };
                }

            }
            else
            {
                #region Crear Simulacion CLiente
                var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
                {
                    Id = datosPersonales.IdSimulacion,
                    EnvioNotificacion = false,
                    Estado = "I",
                    FechaActualiza = DateTime.Now,
                    FechaSolicitud = DateTime.Now,
                    NumeroPreAprobado = 0,
                    ProblemaEmail = false,
                    UsuarioActualiza = Guid.NewGuid(),
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    Score = Score,
                    EndeudamientoCCH = (int)EndeudamientoCCH,
                    EndeudamientoSCH = (int)EndeudamientoSCH
                });
                var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(new SMCSimulacionClienteDto()
                {
                    Documento = request.documento,
                    OpcionCompra = (double)valorOpcionDeCompra,
                    TotalCuotas = plazo,
                    FechaSimulacion = DateTime.Now,
                    IdDatosPersonales = datosPersonales.Id,
                    IdSimulacion = simulacionId,
                    IdSimulacionCliente = datosPersonales.IdSimulacion,
                    NumeroPreaprobado = 0,
                    TasaEa = tasaCredito,
                    TasaFrech = 0,
                    TasaMv = (double)tasaPeriodicaNMV,
                    TipoAbono = "N",
                    TipoAlivio = "N",
                    TipoVivienda = request.ViviendaNueva ? "N" : "U",
                    TomaSeguro = "N",
                    UsaRecursosAcumulados = RecursosPropios > 0 ? "S" : "N",
                    ValorPrestamo = (decimal)VrCredito,
                    ValorPrestamosLeasing = (double)VrCredito2,
                    ValorVivienda = (double?)valorViviendaMaximo,
                    ViviendaVis = viviendaVis,
                    TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                    TasaSeguroHogar = (double)parametrosGenerales.Tasas.SeguroHogar,
                    TasaSeguroVida = (double)parametrosGenerales.Tasas.SeguroVida,
                    ValorPrestamoSolicitado = request.valorPrestamo,
                    PorcentajeCanonInicial = request.CanonInicial,
                    PorcentajeOpcionCompra = request.porOpcionCompra,
                    ValorViviendaSeguro = request.ValorViviendaSeguro,
                    AportesVoluntarios = request.TasaVoluntariaAportes ? 1 : 0,
                    Usr_id = request.Usr_id,
                    afl_id = request.afl_id,
                    CLC_ID = 3
                });
                #endregion
                #region Simulacion
                SolicitudCredito solicitudGeneralCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, request.documento);


                for (int i = 0; i <= plazo; i++)
                {
                    if (i == 0)
                    {
                        var fechaPrimerCorte = this.ObtenerFechaPrimerCorte();
                        int mes = fechaPrimerCorte.Month;
                        int año = fechaPrimerCorte.Year;
                        DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                        PlanSimulacion.Add(new Veteranos()
                        {
                            Capital = 0,
                            IntCte = 0,
                            SegVida = 0,
                            SegHogar = 0,
                            Cuota = 0,
                            Saldo = (decimal)request.valor - (decimal)valorCanonInicial,
                            NroCuota = i,
                            FechaCorte = fechaPrimerCorte,
                            FechaPago = fechaPrimerCortedos,
                            SaldoCapitalReducido = CapitalReducido,
                            CapitalAPagar = 0,
                            CapitalPorPagar = 0,
                            InteresesCorrientesAPagar = 0,
                            InteresesCorrientesPorPagar = 0,
                            SeguroHogar = 0,
                            SeguvoVida = 0,
                            SeguroPorPagar = 0,
                            CuotaUnidadPagadora = 0,
                            AfectacionCuentaIndividual = 0,
                            DeudaRemanente = CapitalReducido,
                            BeneficioTasaFresh = 0,
                            BeneficioAlivioCuota = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = (decimal)(request.valor - valorCanonInicial)
                        });
                    }
                    else
                    {
                        var IntCte = i == 1 ? (decimal)(PlanSimulacion[i - 1].Saldo - (decimal)valorCanonInicial) * tasaPeriodicaNMV : (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional) * tasaPeriodicaNMV;
                        var Capital = i == plazo ? PlanSimulacion[i - 1].Saldo : canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;
                        var SaldoLeasingTradicional = (decimal)(PlanSimulacion[i - 1].SaldoLeasingTradicional - Capital);
                        var CapitalTardicional = canonTradicional - IntCte;
                        var FechaCorteTemp = PlanSimulacion[i - 1].FechaCorte.AddMonths(1);
                        var FechaCorte = new DateTime(FechaCorteTemp.Year, FechaCorteTemp.Month, DateTime.DaysInMonth(FechaCorteTemp.Year, FechaCorteTemp.Month));
                        var FechaPagoTemp = PlanSimulacion[i - 1].FechaPago.AddMonths(1).AddDays(-PlanSimulacion[i - 1].FechaPago.Day).AddMonths(1);
                        var FechaPago = new DateTime(FechaPagoTemp.Year, FechaPagoTemp.Month, DateTime.DaysInMonth(FechaPagoTemp.Year, FechaPagoTemp.Month));
                        var InteresesCorrientesAPagar = 0M;
                        var CapitalAPagar = 0M;
                        var CapitalPorPagar = 0M;
                        var InteresesCorrientesPorPagar = 0M;
                        var SaldoCapitalReducido = 0M;
                        var SaldoTotalRestante = 0M;
                        var TotalSeguros = 0M; 

                        if (i == plazo)
                        {
                            //IntCte = (decimal)(PlanSimulacion[i - 1].CapitalAPagar) * tasaPeriodicaNMV;

                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            CanonReducido = CapitalAPagar + InteresesCorrientesAPagar + valorSeguroVida + valorSeguroHogar;
                            canonTradicional = IntCte + Capital + valorSeguroHogar + valorSeguroVida;
                            SaldoTotalRestante = 0;
                        }
                        else
                        {
                            InteresesCorrientesAPagar = PlanSimulacion[i - 1].SaldoCapitalReducido * tasaPeriodicaNMV;
                            CapitalAPagar = CanonReducido - InteresesCorrientesAPagar - valorSeguroHogar - valorSeguroVida;
                            CapitalPorPagar = Capital - CapitalAPagar;
                            InteresesCorrientesPorPagar = IntCte - InteresesCorrientesAPagar;
                            SaldoCapitalReducido = PlanSimulacion[i - 1].SaldoCapitalReducido - CapitalAPagar;
                            Capital = canonTradicional - valorSeguroVida - valorSeguroHogar - IntCte;
                        }

                        TotalSeguros = valorSeguroHogar + valorSeguroVida;

                        double seguroVidaProrrata = 0;
                        double seguroHogarProrrata = 0;
                        if (i == 1)
                        {
                            if (solicitudGeneralCredito != null)
                            {
                                var fecha = request.FechaDesembolso;

                                if (fecha < new DateTime(1753, 1, 1))
                                {
                                    fecha = DateTime.Now; // o null
                                }

                                await _creditoRepository.ActualizarFechaDesembolsoCredito(
                                    solicitudGeneralCredito.SOC_ID,
                                    fecha);

                                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(solicitudGeneralCredito.SOC_ID);

                                if (datosFactura != null && datosFactura.FAC_NUMERO_FACTURA != null)
                                {
                                    SolicitudDesembolso informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(solicitudGeneralCredito.SOC_ID);

                                    DateTime? fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;

                                    DateTime ultimoDia = new DateTime(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month, DateTime.DaysInMonth(informacionDesembolso.SID_FECHA_DESEMBOLSO.Year, informacionDesembolso.SID_FECHA_DESEMBOLSO.Month));

                                    var diasProrrata = (ultimoDia - informacionDesembolso.SID_FECHA_DESEMBOLSO).Days;
                                    seguroVidaProrrata = Math.Round((double)valorSeguroVida * (diasProrrata + 1) / 30, 0);
                                    seguroHogarProrrata = Math.Round((double)valorSeguroHogar / 30, 0) * (diasProrrata + 0);
                                }
                            }
                        }
                        PlanSimulacion.Add(new Veteranos()
                        {
                            NroCuota = i,
                            Cuota = i == plazo ? (CanonReducido % 1 <0.5m ?   CanonReducido : CanonReducido ) : canonTradicional,
                            SegHogar = valorSeguroHogar,
                            SegVida = valorSeguroVida,
                            IntCte = IntCte,
                            Capital = Capital,
                            Saldo = i == 1 ? CapitalReducido : PlanSimulacion[i - 2].DeudaRemanente - PlanSimulacion[i - 1].CapitalAPagar + PlanSimulacion[i - 1].CapitalPorPagar + PlanSimulacion[i - 1].InteresesCorrientesPorPagar,
                            FechaCorte = FechaCorte,
                            FechaPago = FechaPago,
                            CuotaUnidadPagadora = CanonReducido + (decimal)seguroHogarProrrata + (decimal)seguroVidaProrrata,
                            SeguroHogar = valorSeguroHogar+(decimal)seguroHogarProrrata,
                            SeguvoVida = valorSeguroVida+(decimal)seguroVidaProrrata,
                            SeguroPorPagar = 0,
                            InteresesCorrientesAPagar = InteresesCorrientesAPagar,
                            InteresesCorrientesPorPagar = InteresesCorrientesPorPagar,
                            CapitalAPagar = CapitalAPagar,
                            CapitalPorPagar = CapitalPorPagar,
                            SaldoCapitalReducido = SaldoCapitalReducido,
                            DeudaRemanente = PlanSimulacion[i - 1].DeudaRemanente - CapitalAPagar + CapitalPorPagar + InteresesCorrientesPorPagar,
                            AfectacionCuentaIndividual = SaldoTotalRestante,
                            BeneficioAlivioCuota = 0,
                            BeneficioTasaFresh = 0,
                            ValoraCobrarFonvivienda = 0,
                            SaldoLeasingTradicional = SaldoLeasingTradicional, 
                            SegurosTotal = TotalSeguros
                        });
                    }
                }
                foreach (Veteranos item in PlanSimulacion)
                {
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        Cuota = item.NroCuota,
                        FechaCorte = item.FechaCorte,
                        FechaPago = item.FechaPago,
                        CapitalLeasing = item.Capital,
                        InteresLeasing = item.IntCte,
                        SeguroVidaLeasing = item.SeguvoVida,
                        SeguroHogar = (double)item.SeguroHogar,
                        TotalCuota = (double)item.Cuota,
                        CuotaUnidadPagadora = item.Cuota,
                        SaldoLeasing = item.Saldo,
                        IdSimulacion = simulacionId,
                        IdDetalleSimulacion = Guid.NewGuid(),
                        InteresAPagar = item.InteresesCorrientesAPagar,
                        IdSimulacionCliente = datosPersonales.IdSimulacion,
                        AfectacionCuentaIndividual = item.AfectacionCuentaIndividual,
                        Capitalpagar = item.CapitalAPagar,
                        /// nueva extructura
                        NoventaCapacidadDePago = (decimal)capacidadPago,
                        ValorMaximoVivienda = (decimal)request.valor,
                        SaldoLeasingTradicional = item.Saldo - item.Capital, 
                        SegurosTotal = Math.Round(item.SegurosTotal, 2),
                        Afl_id = request.afl_id,
                        NumeroDocumento = request.documento,
                        ValorFinanciar = item.SaldoLeasingTradicional - (decimal)valorCanonInicial,
                        ValorDeudaRemanente = item.DeudaRemanente,
                        ViviendaVis = viviendaVis


                    });
                }
                #endregion
            }
            #region Guardado en BD


            planillaSimulacion[0].ValorCanonInicial = (decimal)valorCanonInicial;

            planillaSimulacion[0].factor = factor;
            planillaSimulacion[0].CuotaSectorFinanciero = CuotaSectorFinanciero;
            planillaSimulacion[0].CuotaSectorReal = CuotaSectorReal;
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].RiesgoMinimoTransUnion = (Score == 0) ? 0 : Score;
            planillaSimulacion[0].ValorFinanciar = (decimal)valorViviendaDeseado - (decimal)valorCanonInicial;
            planillaSimulacion[0].AportesPropios = (decimal)request.ValorViviendaSeguro - (decimal)valorViviendaDeseado;
            planillaSimulacion[0].ValorTasa = tasaCredito;

            if (request.tipoSimularOProspectar == 3)
            {
                await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
                var consultaSMC = await _creditoBusinessLogic.ConsultarSimulacionCliente(request.documento);

                var smc_id = consultaSMC.Data.smc_id;


                ValoresSimulacionAfiliadoDto valoresSimulacion = new ValoresSimulacionAfiliadoDto();
                valoresSimulacion.ValorFinanciar = planillaSimulacion[0].ValorFinanciar;
                valoresSimulacion.NumeroDocumento = request.documento;
                valoresSimulacion.SMS_ID = smc_id;
                valoresSimulacion.RiesgoMinimo = planillaSimulacion[0].RiesgoMinimoTransUnion;
                valoresSimulacion.ValorSectorFinanciero = planillaSimulacion[0].CuotaSectorFinanciero;
                valoresSimulacion.ValorSectorReal = planillaSimulacion[0].CuotaSectorReal;
                valoresSimulacion.Factor = planillaSimulacion[0].factor;

                await _creditoRepository.InsertarValoresSimulacionAfiliado(valoresSimulacion);
            }
            #endregion
            

            

  

            var valoresSimulacionProspeccion = new ValoresSimulacionCreditoReclasificacion
            {
                NUMERO_DOCUMENTO = request.documento,
                TASA_CREDITO = parametrosGenerales.Tasas.EfectivaAnual,
                TASA_NOMINAL = parametrosGenerales.Tasas.NominalMesVencido,
                SEGURO_VIDA = decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                SEGURO_HOGAR = decimal.Parse(planillaSimulacion[0].SeguroHogar.ToString()),
                SALARIO_MINIMO = parametrosGenerales.SalarioMinimo,
                SEGURO_VIDA_EXTRAPRIMA = 0,
                TASA_MIXTA = parametrosGenerales.Tasas.Mixta,
                TASA_MIXTA_EXTRAPRIMA = 0,
                FACTOR = 0,
                FACTOR_EXTRA_PRIMA = 0,
                PLAZO = plazo,
                VALOR_PRESTAMO = planillaSimulacion[0].ValorFinanciar,
                CUOTA_SIN_SEGURO_HOGAR = canonTradicional - decimal.Parse(planillaSimulacion[0].SeguroVida.ToString()),
                CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA = 0,
                CUOTA_SEGURO_HOGAR = canonTradicional,
                CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA = 0,
                VALOR_INMUEBLE = decimal.Parse(request.valor.ToString()),
                VALOR_AVALUO = decimal.Parse(request.avaluoInmueble.ToString()),
                CAPACIDAD_PAGO = (decimal)capacidadPago,
                VALOR_PRESTAMO_SEGUN_CUOTA = 0
            };

            bool guardado = await _creditoRepository
                .InsertValoresSimulacionCredito(valoresSimulacionProspeccion);

            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        #endregion

        private async Task<double> CalcularEndeudamiento(double Obligacion, double Ingresos)
        {
            var EndeudamientoG_ConCuota = Obligacion / Ingresos;
            return EndeudamientoG_ConCuota;
        }
        private async Task<double> CalcularCuotaNueva(double Objetivo, double Inicio, double Fin, double Tolerancia, double Ingresos)
        {
            double ValorMedio;
            double Endeudamiento;
            double CuotaInicio = Math.Round(await CalcularEndeudamiento(Inicio, Ingresos) * 100, 0, MidpointRounding.AwayFromZero);
            double CuotaFin = Math.Round(await CalcularEndeudamiento(Fin, Ingresos) * 100, 0, MidpointRounding.AwayFromZero);
            if (CuotaInicio > Objetivo || CuotaFin < Objetivo)
            {
                return Fin;
            }
            do
            {
                ValorMedio = (Inicio + Fin) / 2;
                Endeudamiento = Math.Round(await CalcularEndeudamiento(ValorMedio, Ingresos) * 100, 0, MidpointRounding.AwayFromZero);
                if (Endeudamiento < Objetivo)
                {
                    Inicio = ValorMedio;
                }
                else
                {
                    Fin = ValorMedio;
                }

            } while (Math.Abs(Endeudamiento - Objetivo) != Tolerancia);

            return ValorMedio;
        }
        public async Task<double> RedondearCifras(double Valor)
        {
            if (Math.Round(Valor, 3) - Math.Floor(Valor) >= 0.5)
            {
                return Math.Ceiling(Valor);
            }
            else
            {
                return Math.Floor(Valor);
            }
        }
        private async Task<Response<ResponseDocumentoSimDto>> SaveReportTU(IFormFile request, Guid IdSolicitud)
        {

            try
            {


                var RutaImagen = await _creditoBusinessLogic.CargaDocumentoSolicCredito(request);



                #region Busqueda Documnetos PreAprobado
                DocumentosActSimulacionDto documentosActSimulacionDto = new DocumentosActSimulacionDto()
                {
                    SLS_ID = IdSolicitud,
                    AFL_ID = 1
                };
                var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(documentosActSimulacionDto);
                var Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Contains("Reporte centrales de riesgo")).Select(x => x.TDC_ID).FirstOrDefault();

                #endregion
                RequestDocumentoSimDto requestDocumentoSimDto = new RequestDocumentoSimDto()
                {
                    idTipoDocumento = Tdc_id,
                    codigoBarras = "",
                    creadoPor = new Guid(),
                    fechaCreacion = DateTime.Now,
                    rutaImagen = RutaImagen.Data,
                    idSimulacion = IdSolicitud
                };

                _logger.LogInformation("Datos del archivo guardado" + requestDocumentoSimDto);
                return await this._creditoBusinessLogic.CrearDocumentoSimulacion(requestDocumentoSimDto);
            }
            catch (Exception e)
            {
                _logger.LogError("Error al cargar el archivo reporte comercial " + e);
                return new Response<ResponseDocumentoSimDto>{
                    Data = null, 
                    IsSuccess= false, 
                    ReturnMessage = "Error al cargar el archivo"
                }; 
            }
           
        }
        private async Task<Response<ResponseDocumentoSimDto>> SaveReportTU60(IFormFile request, Guid IdSolicitud)
        {
            try
            {
                Response<ParamGeneralesCreditoYCarteraDto> param = await this._carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera(); 

                int porcentaje = (int)Math.Round(param.Data.porcentajeMinimoEndeudamiento); 

                string documentoReporte = "Registro consulta centrales de riesgos";


                var RutaImagen = await _creditoBusinessLogic.CargaDocumentoSolicCredito(request);


                #region Busqueda Documnetos PreAprobado
                DocumentosActSimulacionDto documentosActSimulacionDto = new DocumentosActSimulacionDto()
                {
                    SLS_ID = IdSolicitud,
                    AFL_ID = 1
                };
                var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(documentosActSimulacionDto);
                var Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Contains(documentoReporte)).Select(x => x.TDC_ID).FirstOrDefault();
                _logger.LogInformation("[{method}] "+ documentoReporte);

                #endregion
                RequestDocumentoSimDto requestDocumentoSimDto = new RequestDocumentoSimDto()
                {
                    idTipoDocumento = Tdc_id,
                    codigoBarras = "",
                    creadoPor = new Guid(),
                    fechaCreacion = DateTime.Now,
                    rutaImagen = RutaImagen.Data,
                    idSimulacion = IdSolicitud
                };

                return await this._creditoBusinessLogic.CrearDocumentoSimulacion(requestDocumentoSimDto);
            }
            catch (Exception e)
            {
                return new Response<ResponseDocumentoSimDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "Error al cargar el archivo"
                };
            }
           

        }
        private async Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionCreditoConsumo(CreacionSimulacionClienteDto request, SimulacionDatosPersonalesDto datosPersonales, Guid idSimulacion)
        {
            List<SimulacionDto> planillaSimulacion = new();
            //await _creditoBusinessLogic.LimpiarSimulacionCliente(idSimulacion);
            var equivalenciasSimuladoCredito = (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data;
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoIdentificacion, request.documento)).Data;
            var cliente = (await this.ObtenerClienteProspeccion(new DataClienteProspeccionDto { TipoIdentificacion = request.tipoIdentificacion, NumeroDocumento = request.documento })).Data;
            var datosFinancieros = (await this._creditoBusinessLogic.ObtenerDatosFinancierosPersonales(request.documento)).Data;
            var tasaSeguroVida = (double)(await _carteraBusinessLogic.ObtenerSeguroGeneral()).Data.First().seguroVida / 100;
            var tasaSimulador = (await _carteraBusinessLogic.ObtenerTasasSimulador()).Data.First(t => t.IdSimulacion == request.simId.ToString());
            var tasaCredito = tasaSimulador.TasaEA / 100;
            var plazoMaximo = tasaSimulador.MaximoPlazoMeses;
            var plazo = 0;
            var salarioMinimo = datosFinancieros.ValorSalario;
            var montoMaxSMLV = 300;
            var montoMaximo = salarioMinimo * montoMaxSMLV;
            var ingresoTotal = datosFinancieros.TotalIngresos;
            var totalDescuentos = datosFinancieros.TotalEgresos;
            var capacidadPago = (ingresoTotal * (decimal)0.5) - totalDescuentos;
            double primero = (double)(1 + tasaCredito);
            double segundo = 0.0833333333333333;
            var tasaMesVencido = Math.Pow(primero, segundo) - 1;
            //Calcula tasa mixta(segurovida + tasa mes vencido)
            var tasaMixta = tasaSeguroVida + tasaMesVencido;
            var factor = 0M;
            var prestamoMaximo = 0M;
            var cuotaConSeguro = 0M;
            var cuotaSinSeguro = 0M;
            //si tiene abono extra calculo el abono extra
            var abonoExtra = 0M;
            if (request.AbonoExtra)
            {
                abonoExtra = (datosFinancieros.ValorSalario * 21) / 100;
            }
            if (request.valor > 0 && request.valorCuota <= 0 && request.plazo <= 0)
            {
                //Si calcula sobre el valor prestamos
                if (request.plazo <= 0 || request.plazo > plazoMaximo)
                    plazo = tasaSimulador.MaximoPlazoMeses;
                else
                    plazo = request.plazo;
                factor = (decimal)(tasaMixta * Math.Pow((1 + tasaMixta), plazo) / (Math.Pow(1 + tasaMixta, plazo) - 1));
                prestamoMaximo = capacidadPago / factor;

                if ((decimal)request.valor > prestamoMaximo)
                    request.valor = (double?)prestamoMaximo;
                cuotaConSeguro = (decimal)request.valor * factor;
                cuotaSinSeguro = cuotaConSeguro - ((decimal)request.valor * (decimal)tasaSeguroVida);
            }
            else if (request.valorCuota > 0 && request.valor <= 0 && request.plazo <= 0)
            {
                //Si calcula sobre el valor de la cuota
                plazo = plazoMaximo;
                factor = (decimal)(tasaMixta * Math.Pow((1 + tasaMixta), plazo) / (Math.Pow(1 + tasaMixta, plazo) - 1));
                prestamoMaximo = capacidadPago / factor;
                cuotaConSeguro = (decimal)request.valor * factor;
                request.valor = (double)prestamoMaximo;
                cuotaSinSeguro = cuotaConSeguro - ((decimal)request.valor * (decimal)tasaSeguroVida);
            }
            else if (request.plazo > 0 && request.valorCuota <= 0 && request.valor <= 0)
            {
                //Si calcula sobre el plazo
                if (request.plazo > plazoMaximo)
                    plazo = plazoMaximo;
                factor = (decimal)(tasaMixta * Math.Pow((1 + tasaMixta), plazo) / (Math.Pow(1 + tasaMixta, plazo) - 1));
                prestamoMaximo = capacidadPago / factor;
                cuotaConSeguro = (decimal)request.valor * factor;
                request.valor = (double)prestamoMaximo;
                cuotaSinSeguro = cuotaConSeguro - ((decimal)request.valor * (decimal)tasaSeguroVida);
            }
            else if (request.plazo > 0 && request.valorCuota <= 0 && request.valor > 0)
            {
                //Si calcula sobre el plazo y el valor del prestamo
                if (request.plazo > plazoMaximo)
                    plazo = plazoMaximo;
                factor = (decimal)(tasaMixta * Math.Pow((1 + tasaMixta), plazo) / (Math.Pow(1 + tasaMixta, plazo) - 1));
                prestamoMaximo = capacidadPago / factor;
                if ((decimal)request.valor > prestamoMaximo)
                    request.valor = (double?)prestamoMaximo;
                cuotaConSeguro = (decimal)request.valor * factor;
                cuotaSinSeguro = cuotaConSeguro - ((decimal)request.valor * (decimal)tasaSeguroVida);
            }
            else if (request.plazo > 0 && request.valorCuota > 0 && request.valor <= 0)
            {
                //Si calcula sobre el valor de la cuota y el plazo
                if (request.plazo > plazoMaximo)
                    plazo = plazoMaximo;
                factor = (decimal)(tasaMixta * Math.Pow((1 + tasaMixta), plazo) / (Math.Pow(1 + tasaMixta, plazo) - 1));
                prestamoMaximo = capacidadPago / factor;
                cuotaConSeguro = (decimal)request.valor * factor;
                request.valor = (double)prestamoMaximo;
                cuotaSinSeguro = cuotaConSeguro - ((decimal)request.valor * (decimal)tasaSeguroVida);
            }

            var solicitudSimulacion = await _creditoBusinessLogic.CrearSolicitudSimulacionCredito(new SolicitudSimulacionDto
            {
                Id = datosPersonales.IdSimulacion,
                EnvioNotificacion = false,
                Estado = "I",
                FechaActualiza = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                NumeroPreAprobado = 0,
                ProblemaEmail = false,
                UsuarioActualiza = Guid.NewGuid(),
                TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito
            });
            var simulacionCliente = await _creditoBusinessLogic.CrearSimulacionClienteSMC(new SMCSimulacionClienteDto()
            {
                Documento = request.documento,
                TotalCuotas = plazo,
                FechaSimulacion = DateTime.Now,
                IdDatosPersonales = datosPersonales.Id,
                IdSimulacion = datosPersonales.IdSimulacion,
                IdSimulacionCliente = datosPersonales.IdSimulacion,
                NumeroPreaprobado = 0,
                TasaEa = tasaCredito,
                TasaFrech = 0,
                TasaMv = tasaMesVencido,
                TipoAbono = "N",
                TipoAlivio = "N",
                TomaSeguro = "N",
                ValorPrestamo = (decimal)request.valor,
                TipoVivienda = "",
                ViviendaVis = "",
                UsaRecursosAcumulados = "",
                TcrId = equivalenciasSimuladoCredito.First(e => e.TipoSimulador == request.simId).TipoCredito,
                TasaSeguroVida = tasaSeguroVida * 100,
                TasaSeguroHogar = 0,
                ValorViviendaSeguro = request.ValorViviendaSeguro,
                afl_id = request.afl_id

            });


            for (int i = 0; i <= plazo; i++)
            {
                if (i == 0)
                {
                    var fechaPrimerCorte = DateTime.Now;
                    int mes = fechaPrimerCorte.Month;
                    int año = fechaPrimerCorte.Year;
                    DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);

                    planillaSimulacion.Add(new SimulacionDto
                    {
                        IdSimulacion = idSimulacion,
                        IdDetalleSimulacion = Guid.NewGuid(),
                        IdSimulacionCliente = datosPersonales.IdSimulacion,
                        Cuota = i,
                        ValorPrestamo = (decimal)request.valor,
                        TotalCuota = (double)cuotaConSeguro,
                        TotalCuotaSinSeguros = (double?)cuotaSinSeguro,
                        PlazoMeses = plazo,
                        PlazoAnos = plazo / 12,
                        ValorTasa = tasaCredito,
                        ValorTasaNominal = (decimal)tasaMesVencido,
                        Saldo = (double)request.valor,
                        FechaPago = fechaPrimerCortedos,
                        FechaCorte = fechaPrimerCorte
                    });

                }
                else
                {
                    decimal interesCuota = (decimal)(planillaSimulacion[i - 1].Saldo * tasaMesVencido);
                    decimal seguroVidaCuota = (decimal)(tasaSeguroVida * planillaSimulacion[i - 1].Saldo);
                    var capital = !request.AbonoExtra ? (cuotaConSeguro - seguroVidaCuota - interesCuota) : ((cuotaConSeguro + abonoExtra) - seguroVidaCuota - interesCuota);
                    var fechaCorte = planillaSimulacion[i - 1].FechaCorte.AddMonths(1);
                    int mes = fechaCorte.Month;
                    int año = fechaCorte.Year;
                    DateTime fechaPrimerCortedos = this.ObtenerFechaPrimerCortepago(mes + 1, año);
                    planillaSimulacion.Add(new SimulacionDto
                    {
                        IdSimulacion = idSimulacion,
                        IdDetalleSimulacion = Guid.NewGuid(),
                        IdSimulacionCliente = datosPersonales.IdSimulacion,
                        Cuota = i,
                        TotalCuota = (double)cuotaConSeguro,
                        SeguroVida = (double)seguroVidaCuota,
                        Intereses = (double)interesCuota,
                        Capital = (double)capital,
                        FechaCorte = fechaCorte,
                        FechaPago = fechaPrimerCortedos,
                        Saldo = planillaSimulacion[i - 1].Saldo - (double)capital,
                        ValorAbonoExtra = abonoExtra
                    });
                }
            }
            await _creditoBusinessLogic.CrearDetalleSimulacion(planillaSimulacion, request.documento);
            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }
        private async Task<ParametrosGenerales> ObtenerParametrosGeneralesSimulacion(ParametrosGeneralesRequest request)
        {
            ParametrosGenerales parametrosGenerales = new ParametrosGenerales();
            try
            {
                var tasaSimulador = (await _carteraBusinessLogic.ObtenerTasasSimulador()).Data.First(t => t.IdSimulacion == request.SimuladorId.ToString());
                decimal tasaCredito = tasaSimulador.TasaEA / 100;
                var excepcionesTipoAfiliacion = (await _carteraBusinessLogic.ObtenerExcepcionesPorTipoAfiliacion(request.TipoAfiliacionId)).Data;
                //tasaCredito = excepcionesTipoAfiliacion != null && excepcionesTipoAfiliacion.IdSimulador == request.SimuladorId ? (decimal)(excepcionesTipoAfiliacion.TasaEA / 100) : tasaCredito;

                var excepcionesUnidadEjecutora = (await this.ObtenerExcepcionesTasaUEj(request.SimuladorId, request.UnidadEjecutoraId)).Data;
                tasaCredito = excepcionesUnidadEjecutora != null && excepcionesUnidadEjecutora.UEJ == request.UnidadEjecutoraId ? (decimal)(excepcionesUnidadEjecutora.TasaEA / 100) : tasaCredito;

                var parametrosGeneralesCreditoCartera = (await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera()).Data;
                var limitesSeguroHogar = ((await _creditoBusinessLogic.ObtenerTasasHogarCiudad()).Data).First(t => t.DpdId == request.DepartamentoInmuebleId);
                decimal tasaNominalMesVencido = (decimal)(Math.Pow((double)(1 + tasaCredito), 0.083333333) - 1);
                decimal tasaSeguroVida = ((await _carteraBusinessLogic.ObtenerSeguroGeneral()).Data.First().seguroVida);
                decimal Iva = ((await _carteraBusinessLogic.ObtenerSeguroGeneral()).Data.First().Seg_Iva / 100) + 1;

                var SeguroHogar = ((await _creditoBusinessLogic.ObtenerTasasHogarUbicacion(request.DepartamentoInmuebleId)).Data.First().ValorTasa);


                parametrosGenerales.SalarioMinimo = parametrosGeneralesCreditoCartera.parametroSalarioMinimo;
                parametrosGenerales.LimiteVIP = limitesSeguroHogar.LimiteVip;
                parametrosGenerales.LimiteVIS = limitesSeguroHogar.LimiteVis;
                parametrosGenerales.ValorMinimoCuota = parametrosGeneralesCreditoCartera.valorMinimoCuota;
                parametrosGenerales.MaximoPlazoMeses = tasaSimulador.MaximoPlazoMeses;
                parametrosGenerales.MinimoPlazoMeses = tasaSimulador.MinimoPlazoMeses;
                parametrosGenerales.ActiveTU = parametrosGeneralesCreditoCartera.ActiveTu;
                parametrosGenerales.Score_Min_TU = parametrosGeneralesCreditoCartera.Score_Min_TU;
                parametrosGenerales.PorcentajeMinimoEndeudamiento = parametrosGeneralesCreditoCartera.porcentajeMinimoEndeudamiento;
                parametrosGenerales.AhorroIntereses = parametrosGeneralesCreditoCartera.AhorroIntereses; 
                parametrosGenerales.CesantiasIntereses = parametrosGeneralesCreditoCartera.CesantiasIntereses;
                parametrosGenerales.RangoAscenso = parametrosGeneralesCreditoCartera.RangoAscenso;
                parametrosGenerales.ObligacionesExternasCapacidadPago = parametrosGeneralesCreditoCartera.IncluirObligacionesExternasCapacidadPago;
                parametrosGenerales.SeleccionCargoConSalario = parametrosGeneralesCreditoCartera.AsignacionSalarioCargo; 
                parametrosGenerales.Factores = new Factores
                {
                    FactorAhorro = parametrosGeneralesCreditoCartera.factorAhorro,
                    FactorIncremento = parametrosGeneralesCreditoCartera.porcentajeIncrementoAnual,
                    FactorInflacion = parametrosGeneralesCreditoCartera.porcentajeInflacion,
                    FactorCesantias = parametrosGeneralesCreditoCartera.factorCesantias,
                    FactorIncrementoSubsidio = parametrosGeneralesCreditoCartera.factorSubsidio,
                    InteresesCesantias = 12M
                };
                parametrosGenerales.Tasas = new Tasas
                {
                    EfectivaAnual = tasaCredito,
                    NominalMesVencido = tasaNominalMesVencido,
                    SeguroVida = tasaSeguroVida/100,
                    SeguroHogar = SeguroHogar/100,
                    Mixta = (tasaSeguroVida + (tasaNominalMesVencido*100))/100,
                    Max_Canon_Inicial = tasaSimulador.Max_Canon_Inicial,
                    Min_Canon_Inicial = tasaSimulador.Min_Canon_Inicial,
                    Max_Opcion_Compra = tasaSimulador.Max_Opc_Compra
                };
                parametrosGenerales.Status = string.Empty;
            }
            catch (Exception ex)
            {
                parametrosGenerales.Status = "Faltan campos por diligenciar";
            }
            return parametrosGenerales;
        }
        private DateTime ObtenerFechaPrimerCorte()
        {
            var fechaActual = DateTime.Now.AddMonths(1).Day;
            var dateCorte = DateTime.Now.AddMonths(1).AddDays((fechaActual) * -1);
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

        /// <summary>
        /// Genera datos prospeccion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<DatosProspeccionDto>> GenerarDatosProspeccion(DatosProspeccionDto request)
        {
            ClienteDto cliente = (await _requestProvider.ObtenerClientePorTipoIdentificationYNumero(request.TipoIdentificacion, request.NumeroDocumento)).Data;
            var datos = request;
            var estrato = (await _catalogsBusinessLogic.ObtenerEstrato()).Data.First(x => x.IdEstrato == request.IdEstrato).IdEstrato;
            var nivelEstudios = (await _catalogsBusinessLogic.ObtenerNivelAcademico()).Data.First(x => x.NivelAcademicoId == request.IdNivelAcademico).NivelAcademicoId;
            var personasACargo = (await _catalogsBusinessLogic.ObtenerPersonasCargo()).Data.First(x => x.Id == request.IdPersonasACargo).IdPersonasCargo;
            var tipoVivienda = (await _catalogsBusinessLogic.ObtenerTipoViviendaPropia()).Data.First(x => x.IdViviendaPropia == request.IdViviendaPropia).IdViviendaPropia;
            var tipoContrato = (await _catalogsBusinessLogic.ObtenerTipoContrato()).Data.First(x => x.IdTipoContrato == request.IdTipoContrato).IdTipoContrato;
            var simulacionPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.NumeroDocumento)).Data;
            var estadoCivil = (await _catalogsBusinessLogic.ObtenerEstadosCiviles()).Data.First(x => x.Id == cliente.EstadoCivil).Id;
            var genero = (await _catalogsBusinessLogic.ObtenerTipoDeSexo()).Data.First(x => x.Codigo == cliente.Sexo).Codigo;
            var edad = cliente.FechaNacimiento == default(DateTime) || cliente.FechaNacimiento == DateTime.Parse("1900-01-01") ? 0 : DateTime.Today.AddTicks(-cliente.FechaNacimiento.Ticks).Year - 1;
            var antiguedad = DateTime.Today.AddTicks(-cliente.FechaAlta.Ticks).Year - 1;
            var ingresos = cliente.IngresoMensual + cliente.IngresoAdicional;/////////////////Pregunta
            var score = 0; //Averiguar donde esta el score
            //ToDO: Crear formaPago
            var formaPago = 0;
            //ToDo: Crear garantias
            var garantia = 0;
            double scoreRiesgo = 0;
            string DesicionHomologada = "VIABLE" /*string.Empty*/;
            //TODO: se deben ejecutar la integración con Transunion y tomar el valor del TAG <DecisionHomologada>


            datos.DesicionHomologadaTU = DesicionHomologada;
            //Si el resultado es “Viable” o “Pendiente”, debe ejecutar el modelo de consumo
            if (DesicionHomologada.ToUpper().Equals("VIABLE") || DesicionHomologada.ToUpper().Equals("PENDIENTE"))
            {
                /*-- Sumas o restas de la estimacion se deben hacer sobre el scoreRiesgo --*/

                //Estrato 3 o 4: -0.2420
                //Estrato 5 o 6: -0.5310
                if (estrato == 4 || estrato == 3)
                    scoreRiesgo -= 0.2420;
                else if (estrato == 6 || estrato == 5)
                    scoreRiesgo -= 0.5310;

                //Nivel de estudios = secundaria : -0.0961
                //Nivel de estudios = tecnico o tecnologo : -0.1142
                //Nivel de estudios = especializacion, maestria, doctorado, postdoctorado : -0.1767
                if (nivelEstudios == 2)
                    scoreRiesgo -= 0.0961;
                else if (nivelEstudios == 3 || nivelEstudios == 4)
                    scoreRiesgo -= 0.1142;
                else if (nivelEstudios > 5)
                    scoreRiesgo -= 0.1767;

                //Estado civil = Soltero: -0.0201
                //Estado civil = Casado: -0.0385
                //Estado civil = Divorciado: +0.0740
                //Estado civil = Viudo: -0.1417
                if (estadoCivil.Equals("S"))
                    scoreRiesgo -= 0.0201;
                else if (estadoCivil.Equals("C"))
                    scoreRiesgo -= 0.0385;
                else if (estadoCivil.Equals("D"))
                    scoreRiesgo += 0.0740;
                else if (estadoCivil.Equals("V"))
                    scoreRiesgo -= 0.1417;

                //Genero = Femenino: -0.5115
                if (genero.Equals("F"))
                    scoreRiesgo -= 0.5115;

                //Tipo vivienda = Propia: -0.9179
                if (tipoVivienda == 1)
                    scoreRiesgo -= 0.9179;

                //FormaPago=DebitoAutom: -0.4233
                //Forma Pago = Nomina: -0.4075 Defecto
                //TODO:Crear tabla y endpoint
                scoreRiesgo -= 0.4075;

                //garantia = pignoracion: -0.8002
                //garantia = Hipotecaria: -1.2189
                scoreRiesgo -= 1.2189;

                //TipoContrato = Prestacion = -0.0615
                //TipoContrato = Independiente = -0.3905
                //TipoContrato = TIndefinido = -0.2610
                //TipoContrato = Pensionado = -0.2813
                if (tipoContrato == 1)
                    scoreRiesgo -= 0.2610;
                else if (tipoContrato == 2)
                    scoreRiesgo -= 0.2813;
                else if (tipoContrato == 3)
                    scoreRiesgo -= 0.0615;
                else if (tipoContrato == 4)
                    scoreRiesgo -= 0.3905;

                //personas a cargo = Dos: -0.4468
                //personas a cargo = Una: -0.7615
                //personas a cargo = Ninguna: -1.1072
                if (personasACargo > 2)
                    scoreRiesgo -= 0.4468;
                else if (personasACargo == 1)
                    scoreRiesgo -= 0.7615;
                else if (personasACargo == 0)
                    scoreRiesgo -= -1.1072;

                //Edad 31 - 40: +0.3163
                //Edad 41 - 50: +0.3525
                //Edad 51 - 55: -0.2888
                //Edad 55 - 60: +0.1480
                //Edad > 60: +0.1807
                if (edad >= 31 && edad <= 40)
                    scoreRiesgo += 0.3163;
                else if (edad >= 41 && edad <= 50)
                    scoreRiesgo += 0.3525;
                else if (edad >= 51 && edad <= 55)
                    scoreRiesgo += 0.2888;
                else if (edad >= 55 && edad <= 60)
                    scoreRiesgo += 0.1480;
                else if (edad > 60)
                    scoreRiesgo += 0.1807;

                //Antiguedad afiliado 1: -0.3687 ////////////////////Preguntar si es uno o menos
                //Antiguedad afiliado 2-5: -0.8222
                //Antiguedad afiliado 6-34: -1.2003
                //Antiguedad afiliado > 34: -1.5906

                //Ingresos 1500000 - 3000000: -0.0625
                //Ingresos 3000000 - 5000000: -0.2466
                //Ingresos < 1500000: +0.7046

                //Score 700-800 : -0.5000
                //Score 800-900 : -1.4816
                //Score > 900 : -2.5416

                //Cesantias 2300000 - 6400000= -0.0431
                //Cesantias 6400000 - 12800000= -0.0045
                //Cesantias 12800000 - 24800000= -0.0275
                //Cesantias > 24800000= -0.2608

                //Activos > 150000000 = -0.9794
                if (request.TotalActivos > 150000000)
                    scoreRiesgo -= 0.9794;

                //Pasivos < 75000000: -1.2247
                if (request.TotalPasivos < 75000000)
                    scoreRiesgo -= 1.2247;

                //scoreRiesgo <=5 : viabilidad=>true
                //scoreRiesgo >5 : viabilidad=>false
                if (scoreRiesgo <= 5)
                    datos.Viabilidad = true;
                else datos.Viabilidad = false;

                datos.RiegoCredito = (decimal)scoreRiesgo;
            }


            //Retorna los datos almacenados
            return new Response<DatosProspeccionDto> { Data = datos };
        }

        public async Task<Response<object>> LlamadoIntegracionesAfiliado(RequestIntegracionesAfiliadoDto request)
        {
            Response<diffgram> validacionBiometria = new();
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            if (bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.ToUpper().Equals("FENIX") && p.PARAMETRO.ToUpper().Equals("ESTADO")).VALOR)) //reemplazar por opciones fenix
            {
                try
                {
                    validacionBiometria = await _fenixBusinessLogic.ValidarBiometria(request.Identificacion);
                    validacionBiometria.ReturnMessage = "";
                    SolicitudCreditoProductoDto logTu = new SolicitudCreditoProductoDto
                    {
                        VLT_ID = Guid.NewGuid(),
                        VLI_ID = Guid.NewGuid(),
                        VLT_SERVICIO = "ValidacionBiometria",
                        VLT_PASO_TRANSACCION = "FENIX",
                        VLT_XML_SOLICITUD = JsonConvert.SerializeObject(request),
                        VLT_XML_RESPUESTA = JsonConvert.SerializeObject(validacionBiometria.Data),
                        VLT_ID_APICACION = request.Identificacion,
                        VLT_CODIGO_RESPUESTA = validacionBiometria.IsSuccess.ToString(),
                        VLT_TIPO_RESPUESTA = "true",
                        VLT_DESCRIPCION_RESPUESTA = JsonConvert.SerializeObject(validacionBiometria.Data.NewDataSet.Table1)

                    };
                    var resapLog = await _gestorActualizacionBusinessLogic.CrearLogTu(logTu);
                }
                catch (Exception ex)
                {

                    SolicitudCreditoProductoDto logTu = new SolicitudCreditoProductoDto
                    {
                        VLT_ID = Guid.NewGuid(),
                        VLI_ID = Guid.NewGuid(),
                        VLT_SERVICIO = "ValidacionBiometria",
                        VLT_PASO_TRANSACCION = "FENIX",
                        VLT_XML_SOLICITUD = JsonConvert.SerializeObject(request),
                        VLT_XML_RESPUESTA = "No se encontro usuario en FENIX",
                        VLT_ID_APICACION = request.Identificacion,
                        VLT_CODIGO_RESPUESTA = "false",
                        VLT_TIPO_RESPUESTA = "false",
                        VLT_DESCRIPCION_RESPUESTA = ex.ToString()

                    };
                    var resapLog = await _gestorActualizacionBusinessLogic.CrearLogTu(logTu);
                    validacionBiometria = new()
                    {
                        ReturnMessage = $"No se pudo realizar la validacion de biometria para el documento {request.Identificacion}"
                    };
                    return new Response<object> { ReturnMessage = validacionBiometria.ReturnMessage, IsSuccess = false, Data = null };
                }
            }
            //Response<ResultadoVerificacionTercero> verificacionTercero = new();
            Response<ServiceVigiaVerificacionTercero.Resultado> verificarTerceroVigia = new(); 
            if (bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.ToUpper().Equals("VIGIA") && p.PARAMETRO.ToUpper().Equals("ESTADO")).VALOR))//vigia
            {
                try
                {
                    //verificacionTercero = await _vigiaBusinessLogic.VertificacionTerceroAsync(new Application.Dto.VerificacionTerceroRequest { Pe_Dato = request.Identificacion, Pe_TipoVerificacion = "1" });
                    verificarTerceroVigia = await _verificarTerceroVigia.VertificacionTerceroSolicitud(request.Identificacion, "1", "100");
                    verificarTerceroVigia.ReturnMessage = "";
                    SolicitudCreditoProductoDto logTu = new SolicitudCreditoProductoDto
                    {
                        VLT_ID = Guid.NewGuid(),
                        VLI_ID = Guid.NewGuid(),
                        VLT_SERVICIO = "VerificacionTercer",
                        VLT_PASO_TRANSACCION = "VIGIA",
                        VLT_XML_SOLICITUD = JsonConvert.SerializeObject(request),
                        VLT_XML_RESPUESTA = JsonConvert.SerializeObject(verificarTerceroVigia.Data),
                        VLT_ID_APICACION = request.Identificacion,
                        VLT_CODIGO_RESPUESTA = verificarTerceroVigia.IsSuccess.ToString(),
                        VLT_TIPO_RESPUESTA = "true",
                        VLT_DESCRIPCION_RESPUESTA = JsonConvert.SerializeObject(validacionBiometria.Data),

                    };
                    var resapLog = await _gestorActualizacionBusinessLogic.CrearLogTu(logTu);
                    if (!string.IsNullOrEmpty(verificarTerceroVigia.Data.Listas))
                        return new Response<object> { ReturnMessage = "Favor acercarse al punto de atención", IsSuccess = false };
                }
                catch (Exception ex)
                {
                    SolicitudCreditoProductoDto logTu = new SolicitudCreditoProductoDto
                    {
                        VLT_ID = Guid.NewGuid(),
                        VLI_ID = Guid.NewGuid(),
                        VLT_SERVICIO = "VerificacionTercer",
                        VLT_PASO_TRANSACCION = "VIGIA",
                        VLT_XML_SOLICITUD = JsonConvert.SerializeObject(request),
                        VLT_XML_RESPUESTA = JsonConvert.SerializeObject(request),
                        VLT_ID_APICACION = request.Identificacion,
                        VLT_CODIGO_RESPUESTA = "false",
                        VLT_TIPO_RESPUESTA = "false",
                        VLT_DESCRIPCION_RESPUESTA = ex.ToString(),

                    };
                    var resapLog = await _gestorActualizacionBusinessLogic.CrearLogTu(logTu);
                    verificarTerceroVigia = new()
                    {
                        ReturnMessage = $"No se pudo realizar la verificacion del tercero con documento {request.Identificacion}"
                    };
                    return new Response<object> { ReturnMessage = verificarTerceroVigia.ReturnMessage, IsSuccess = false, Data = null };
                }
            }
            var OptionsAqm = _optionsAqm.Value;
            RequestTuDto requestTrasnUnion = new()
            {
                IdValidation = Guid.NewGuid().ToString(),
                IdNumber = request.Identificacion,
                IdType = request.TipoIdentificacion,
                RecentPhoneNumber = request.Telefono,
                LastName = request.Apellidos,
                IdExpeditionDate = request.FechaExpedicionIdentificacion,
                usrId = OptionsAqm.usuarioAqm,
                pasword = OptionsAqm.claveAqm,
                solucionId = OptionsAqm.solucionIdAqm
            };

            GestorIdVisionBusinessLogic ConsultasTransUnion = new GestorIdVisionBusinessLogic();
            Response<ResponseTu> respuestaTransUnion = new();
            if (bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.Equals("IDVision")).VALOR))
            {
                try
                {
                    //respuestaTransUnion = new Response<ResponseTu> { Data = await _gestorIdVision.validarCliente(requestTrasnUnion) };
                    respuestaTransUnion = new Response<ResponseTu> { Data = await _gestorIdVision.EnvioCodigoIDVision(requestTrasnUnion) };

                    if (respuestaTransUnion.Data.codigoRechazo == 1)
                    {
                        respuestaTransUnion.ReturnMessage = "Error al enviar el codigo OTP";

                    }
                    else
                    {
                        respuestaTransUnion.ReturnMessage = "Código OTP generado correctamente";

                    }
                    //respuestaTransUnion = new Response<FileResul t> { Data = await _creditoBusinessLogic.ObtenerHistorialComercialBuro(requestTrasnUnion) };

                }
                catch (Exception ex)
                {
                    respuestaTransUnion.ReturnMessage = $"Error al generar validaciones en TransUnion para el documento {request.Identificacion}";
                    return new Response<object> { ReturnMessage = respuestaTransUnion.ReturnMessage, IsSuccess = false, Data = null };
                }
            }

            if (!string.IsNullOrEmpty(respuestaTransUnion.ReturnMessage))
                return new Response<object> { ReturnMessage = respuestaTransUnion.ReturnMessage, IsSuccess = true, Data = respuestaTransUnion.Data };
            else if (!string.IsNullOrEmpty(verificarTerceroVigia.ReturnMessage))
                return new Response<object> { ReturnMessage = verificarTerceroVigia.ReturnMessage, IsSuccess = false };
            else if (!string.IsNullOrEmpty(validacionBiometria.ReturnMessage))
                return new Response<object> { ReturnMessage = validacionBiometria.ReturnMessage, IsSuccess = false };
            else
                return new Response<object> { IsSuccess = true, ReturnMessage = "Validaciones realizadas exitosamente" };

        }


        /// <summary>
        /// Obtiene el cliente para la prospeccion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<DataClienteProspeccionDto>> ObtenerClienteProspeccion(DataClienteProspeccionDto request)
        {
            DataClienteProspeccionDto cliente = await this.ObtenerClienteTipoNumero(request.TipoIdentificacion, request.NumeroDocumento);

            return new Response<DataClienteProspeccionDto> { Data = cliente };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simId"></param>
        /// <param name="uEJ"></param>
        /// <returns></returns>
        public async Task<Response<ExcepcionTasaUEJDto>> ObtenerExcepcionesTasaUEj(int simId, int uEJ)
        {
            var response = (await this._carteraBusinessLogic.ObtenerExcepcionesPlazoUEj(simId, uEJ)).Data;

            return new Response<ExcepcionTasaUEJDto> { Data = response };
        }

        public async Task<DataClienteProspeccionDto> ObtenerClienteTipoNumero(int tipoIdentificacion, string identificacion)
        {
            ClienteDto cliente = (await _requestProvider.ObtenerClientePorTipoIdentificationYNumero(tipoIdentificacion, identificacion)).Data;
            var response = new DataClienteProspeccionDto()
            {
                NumeroDocumento = identificacion,
                TipoIdentificacion = tipoIdentificacion,
                CategoriaId = cliente.IdCategoria,
                TipoTelefonoId = cliente.IdTipoTelefono,
                Celular = cliente.Telefono,
                CiudadResidenciaId = cliente.IdCiudadResidencia,
                Direccion = cliente.Direccion,
                Email = cliente.Correo,
                FuerzaId = cliente.IdFuerza,
                GradoId = cliente.IdGrado,
                Nombres = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}",
                Regimen = cliente.Regimen.ToString(),
                CuotasAportadas = cliente.AportesCategoria.Count().ToString(),
                AntiguedadAños = DateTime.Now.Month - cliente.FechaAlta.Month >= 0 ? (DateTime.Now.Year - cliente.FechaAlta.Year) : (DateTime.Now.Year - cliente.FechaAlta.Year) - 1,
                FormaPago = "Nomina",
                TipoAfiliacion = cliente.IdTipoActor.ToString(),
                IdUnidadEjecutora = cliente.IdUnidadEjecutora,
                FechaAlta = cliente.FechaAlta,
                TasaVoluntariaAportes = cliente.TasaAportes
            };

            response.Categoria = (await _catalogsBusinessLogic.ObtenerCategorias()).Data.First(x => x.Id == response.CategoriaId).Descripcion;
            response.CiudadResidencia = (await _catalogsBusinessLogic.ObtenerCiudades()).Data.First(x => x.Id == response.CiudadResidenciaId).Nombre;
            response.Fuerza = (await _catalogsBusinessLogic.ObtenerFuerzas()).Data.First(x => x.Id == response.FuerzaId).Descripcion;
            response.Grado = (await _catalogsBusinessLogic.ObtenerGrados()).Data.First(x => x.Id == response.GradoId).Descripcion;

            //Temporalmente
            response.TipoContratoId = 1;

            return response;
        }

        public async Task<Response<SolicitudSimulacionDto>> CrearPreAprobado(SolicitudSimulacionDto request)
        {
            Response<SolicitudSimulacionDto> respuesta = await _creditoBusinessLogic.CrearPreAprobado(request);

            return respuesta;
        }

        public async Task<Response<SolicitudCreditoDto>> CrearSolicitudCredito(SolicitudCreditoDto request)
        {
            Response<SolicitudCreditoDto> respuesta = await _creditoBusinessLogic.CrearSolicitudCredito(request);

            return respuesta;
        }

        public async Task<Response<int>> ObtenerSeguroPorUbicacion(int dpcId, int dpdId)
        {
            return new Response<int> { Data = await this._creditoRepository.ObtenerSeguroPorUbicacion(dpcId, dpdId) };

        }

        public async Task<Response<IEnumerable<DocumentoCargarSolicitudDto>>> ObtenerDocumentosRequeridosSolicCredito(Guid idSolicitud)
        {
            var listaDocumentos = await this._creditoRepository.ObtenerDocumentosRequeridosSolicCredito();
            var response = new List<DocumentoCargarSolicitudDto>();
            foreach (var documento in listaDocumentos)
            {
                var respuesta = await this._creditoRepository.ObtenerDocumentosSubidosSolicCredito(new RequestDocumentosSubidosProspeccion { TipoDocumento = documento.TDC_ID, IdSolicitud = idSolicitud });
                response.Add(new DocumentoCargarSolicitudDto
                {
                    Cargado = respuesta != null ? true : false,
                    Id = documento.TDC_ID,
                    Nombre = documento.TDC_DESCRIPCION,
                    Obligatorio = documento.DCA_OBLIGATORIO,
                    Ruta = respuesta != null ? respuesta.DCS_RUTA_IMAGEN : string.Empty,
                    Carga = documento.DCA_CARGA,
                    CargaPreAprobado = documento.DCA_CARGA_PREAPRBADO,
                    VisaulizaCliente = documento.DCA_VISUALIZA_CLIENTE,
                    Visualiza = documento.DCA_VISUALIZA
                });
            }
            return new Response<IEnumerable<DocumentoCargarSolicitudDto>> { Data = response };
        }

        public async Task<EqivalenciaSimuladorDto> ObtenerEquivalenciasSimuladorCredito(int request)
        {
            return (await _creditoBusinessLogic.ObtenerEquivalenciasSimuladorCredito()).Data.First(e => e.TipoCredito == request);
        }

        public async Task<Response<ResponseTu>> ValidacionCodigoOtpIdVision(RequestTuDto request)
        {
            ValidacionOTPTemporalDto validacionOTP = new ValidacionOTPTemporalDto
            {
                Celular = request.celular,
                Identificacion = request.identificacion
            };

            ValidacionOTPTemporal validacionOTPTemporal = await _creditoRepository.ConsultarValidacionOTPTemporal(validacionOTP);

            if (validacionOTPTemporal != null)
            {
                TimeSpan diferencia = DateTime.Now - validacionOTPTemporal.VOT_FECHA;

                if (diferencia.TotalHours >= 24)
                {
                    await _creditoRepository.EliminarValidacionOTPTemporal(validacionOTP);
                    validacionOTPTemporal = null; 
                }
            }

            if (validacionOTPTemporal == null)
            {
                await _creditoRepository.InsertarValidacionOTPTemporal(validacionOTP);
                validacionOTPTemporal = await _creditoRepository.ConsultarValidacionOTPTemporal(validacionOTP);
            }

            if (validacionOTPTemporal.VOT_ESTADO)
            {
                await _creditoRepository.InsertarValidacionOTPTemporal(validacionOTP);

                return new Response<ResponseTu>
                {
                    Data = await _gestorIdVision.validacionCodigoOTPAsync(request)
                };
            }
            else
            {
                ResponseTu respuesta = new ResponseTu
                {
                    bloqueoDia = true
                };

                return new Response<ResponseTu>
                {
                    ReturnMessage = "Bloqueo 24 horas",
                    Data = respuesta
                };
            }
        }



        public async Task<Response<bool>> CargarArchivosPreAprobado(RequestCargarDocumentosPreAprobASocCreditoDto request)
        {
            try
            {
                #region Obtenemos Informacion Solicitd
                RequestSolicitudCreditoDto requestSolicitudCreditoDto = new RequestSolicitudCreditoDto()
                {
                    IdSolicitud = request.Soc_id.ToString(),
                    Indicador = 1
                };
                var InfoSoc = await this._creditoBusinessLogic.ObtenerSolicitudCredito(requestSolicitudCreditoDto);
                #endregion
                #region Obtenemos Documentos PreAprobado
                DocumentosActSimulacionDto documentosActSimulacionDto = new DocumentosActSimulacionDto()
                {
                    AFL_ID = 1,
                    SLS_ID = InfoSoc.Data.IdSls
                };
                var DocumentosPreAprobado = await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(documentosActSimulacionDto);
                #endregion
                #region Pase de Documentos PreAprobado a Solicitud
                foreach (DocumentosActRespuestaDto item in DocumentosPreAprobado.Data.Where(x => x.DOCPREAPROBADO == "S"))
                {
                    RequestDocumentoSolicCreditoDto requestDocumentoSolicCreditoDto = new RequestDocumentoSolicCreditoDto()
                    {
                        codigoBarras = "",
                        creadoPor = new Guid(),
                        fechaCreacion = DateTime.Now,
                        idSolicitudCredito = request.Soc_id,
                        idTipoDocumento = item.TDC_ID,
                        rutaImagen = item.RUTA_IMAGEN
                    };
                    var Response = await this._creditoBusinessLogic.CrearDocumentoSolicCredito(requestDocumentoSolicCreditoDto);
                }
                #endregion
                return new Response<bool> { Data = true, IsSuccess = true, ReturnMessage = "Documentos PreAprobado Cargados" };
            }
            catch (Exception ex)
            {
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = "Error Al Cargar Documentos PreAprobado, error: " + ex.Message };
            }
        }

        public async Task<Response<bool>> ConsultarVigenciaSimulacion(string request)
        {
            var Validacion = new Response<int> { Data = await _creditoRepository.ConsultarVigenciaSimulacion(request) };
            if (Validacion.Data == 1)
            {
                return new Response<bool> { Data = true };
            }
            else
            {
                return new Response<bool> { Data = false };
            }
        }


        public async Task<Response<InformacionProspeccion>> ConsultarInformacionPropeccionPorNumeroDocumento(IdentificacionProspeccion request)
        {
            int tipoDocumento = request.tipoDocumento;
            string numeroDocumento = request.numeroDocumento;

            InformacionProspeccion prospeccionResult = await _creditoRepository.ConsultarProspeccionPorNumeroDocumento(tipoDocumento, numeroDocumento);
            if (prospeccionResult != null)
            {
                return new Response<InformacionProspeccion> { Data = prospeccionResult, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<InformacionProspeccion> { Data = prospeccionResult, IsSuccess = false, ReturnMessage = $"No hay prospeccion registrada con {numeroDocumento} " };
            }
        }

        public async Task<Response<ValoresProspeccion>> ConsultarValoresPropeccionPorNumeroDocumento(IdentificacionProspeccion request)
        {
            int tipoDocumento = request.tipoDocumento;
            string numeroDocumento = request.numeroDocumento;

            ValoresProspeccion prospeccionResult = await _creditoRepository.ConsultarTasasGeneralesPorAfiliado(tipoDocumento, numeroDocumento);
            if (prospeccionResult != null)
            {
                return new Response<ValoresProspeccion> { Data = prospeccionResult, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<ValoresProspeccion> { Data = prospeccionResult, IsSuccess = false, ReturnMessage = $"No hay prospeccion registrada con {numeroDocumento} " };
            }
        }

        public async Task<Response<DatosProspeccionEndudamiento>> ConsultarPropeccionPorNumeroDocumento(IdentificacionProspeccion request)
        {
            int tipoDocumento = request.tipoDocumento;
            string numeroDocumento = request.numeroDocumento;

            DatosProspeccionEndudamiento prospeccionResult = await _creditoRepository.ConsultarProspeccionEndudamiento(tipoDocumento, numeroDocumento);
            if (prospeccionResult != null)
            {
                return new Response<DatosProspeccionEndudamiento> { Data = prospeccionResult, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<DatosProspeccionEndudamiento> { Data = prospeccionResult, IsSuccess = false, ReturnMessage = $"No hay prospeccion registrada con {numeroDocumento} " };
            }
        }


        public async Task<Response<IEnumerable<LineaCreditoObligacion>>> ConsultarLineasCreditoObligacion(IdentificacionProspeccion request)
        {
            int tipoDocumento = request.tipoDocumento;
            string numeroDocumento = request.numeroDocumento;

            IEnumerable<LineaCreditoObligacion> prospeccionResult = await _creditoRepository.ConsultarLineaCreditoObligacion(tipoDocumento, numeroDocumento);
            if (prospeccionResult != null)
            {
                return new Response<IEnumerable<LineaCreditoObligacion>> { Data = prospeccionResult, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<IEnumerable<LineaCreditoObligacion>> { Data = prospeccionResult, IsSuccess = false, ReturnMessage = $"No hay prospeccion registrada con {numeroDocumento} " };
            }
        }


        public async Task<Response<SimulacionPlanClienteDetalle>> RestaurarSimulacionCliente(DatosSimulacionUsuarioDto request)
        {
            int afl_id = request.Afl_id;
            string numeroDocumento = request.Numero_Documento;            
            SimulacionPlanCliente simulacionPlan = await _creditoRepository.ObtenerDatosSimulacionClienteSPC(request);
            IEnumerable<SimulacionDetallePlanCliente> simulacionDetallePlan = await _creditoRepository.ObtenerDetalleSimulacionClienteSPD(request);

            var datosPersonales = (await _creditoBusinessLogic.ObtenerSimulacionPersonales(request.Numero_Documento)).Data;

            var smc = new SMCSimulacionClienteDto()
            {
                Documento = simulacionPlan.SPC_NUMERO_DOCUMENTO,
                OpcionCompra = 0,
                TotalCuotas = (int)simulacionPlan.SPC_TOTAL_CUOTAS,
                FechaSimulacion = DateTime.Now,
                IdDatosPersonales = datosPersonales.Id,
                IdSimulacion = datosPersonales.IdSimulacion,
                IdSimulacionCliente = datosPersonales.IdSimulacion,
                NumeroPreaprobado = 0,
                TasaEa = simulacionPlan.SPC_VALOR_TASA_EA,
                TasaFrech = simulacionPlan.SPC_VALOR_TASA_MV, 
                TasaMv = (double)simulacionPlan.SPC_VALOR_TASA_MV,
                TipoAbono = simulacionPlan.SPC_TIPO_VIVIENDA,
                TipoAlivio = simulacionPlan.SPC_TIPO_ALIVIO,
                TipoVivienda = simulacionPlan.SPC_TIPO_VIVIENDA,
                TomaSeguro = "N",
                UsaRecursosAcumulados = "N",
                ValorPrestamo = simulacionPlan.SPC_VALOR_PRESTAMO,
                ValorPrestamosLeasing = 0,
                ValorVivienda = simulacionPlan.SPC_VALOR_VIVIENDA,
                ViviendaVis = simulacionPlan.SPC_VIVIENDA_VIS,                
                TasaSeguroHogar = (double)simulacionPlan.SPC_TASA_SEGURO_HOGAR* 100,
                TasaSeguroVida = (double)simulacionPlan.SPC_TASA_SEGURO_VIDA * 100,
                ValorViviendaSeguro = simulacionPlan.SPC_VALOR_VIVIENDA_SEGURO,
                factor = simulacionPlan.SPC_FACTOR,
                Usr_id = simulacionPlan.USR_ID,
                afl_id = simulacionPlan.AFL_ID
            };

            var datosPlanSimulacionCliente = _mapper.Map<SMCSimulacionCliente>(smc);

            await this._creditoRepository.RestaurarSimulacionCliente(datosPlanSimulacionCliente);

            List<SimulacionDetalle> detalles = new List<SimulacionDetalle>();            

            foreach (var simulacionDetalle in simulacionDetallePlan)
            {
                var detalleSimulacion = new SimulacionDetalle()
                {
                    SMD_ID = simulacionDetalle.SPD_ID,
                    SMC_ID = datosPersonales.IdSimulacion,
                    SMD_ABONO_EXTRA = simulacionDetalle.SPD_ABONO_EXTRA,
                    SMD_ABONO_MENSUAL = simulacionDetalle.SPD_ABONO_MENSUAL,
                    SMD_CAPITAL = simulacionDetalle.SPD_CAPITAL,
                    SMD_CAPITAL_LEASING = simulacionDetalle.SPD_CAPITAL_LEASING,
                    SMD_COBRO_FONVIVIENDA = simulacionDetalle.SPD_COBRO_FONVIVIENDA,
                    SMD_CUOTA = simulacionDetalle.SPD_CUOTA,
                    SMD_CUOTA_LEASING = simulacionDetalle.SPD_CUOTA_LEASING,
                    SMD_CUOTA_LEASING_SUBSIDIO = simulacionDetalle.SPD_CUOTA_LEASING_SUBSIDIO,
                    SMD_CUOTA_REDUCCION = simulacionDetalle.SPD_CUOTA_REDUCCION,
                    SMD_CUOTA_TOTAL = simulacionDetalle.SPD_CUOTA_TOTAL,
                    SMD_CUOTA_UNIDAD_PAGADORA = simulacionDetalle.SPD_CUOTA_UNIDAD_PAGADORA,
                    SMD_FECHA_CORTE = simulacionDetalle.SPD_FECHA_CORTE,
                    SMD_FECHA_PAGO = simulacionDetalle.SPD_FECHA_PAGO,
                    SMD_INTERESES = simulacionDetalle.SPD_INTERESES,
                    SMD_INTERESES_REDUCCION = simulacionDetalle.SPD_INTERESES_REDUCCION,
                    SMD_INTERES_LEASING = simulacionDetalle.SPD_INTERES_LEASING,
                    SMD_INTERES_LEASING_SUBSIDIO = simulacionDetalle.SPD_INTERES_LEASING_SUBSIDIO,
                    SMD_INTERES_TRADICIONAL_SUBSIDIO = simulacionDetalle.SPD_INTERES_TRADICIONAL_SUBSIDIO,
                    SMD_SALDO = simulacionDetalle.SPD_SALDO,
                    SMD_SALDO_LEASING = simulacionDetalle.SPD_SALDO_LEASING,
                    SMD_SEGURO_TERREMOTO = simulacionDetalle.SPD_SEGURO_TERREMOTO,
                    SMD_SEGURO_VIDA = simulacionDetalle.SPD_SEGURO_VIDA,
                    SMD_SEGURO_VIDA_LEASING = simulacionDetalle.SPD_SEGURO_VIDA_LEASING,
                    SMD_SALDO_REDUCIDO_LEASING = simulacionDetalle.SPD_SALDO_REDUCIDO_LEASING,
                    SMD_INTERESES_PAGADOS_LEASING = simulacionDetalle.SPD_INTERESES_PAGADOS_LEASING,
                    SMD_CAPITAL_PAGADO_LEASING = simulacionDetalle.SPD_CAPITAL_PAGADO_LEASING,
                    SMD_CAPITAL_A_PAGAR_LEASING = simulacionDetalle.SPD_CAPITAL_A_PAGAR_LEASING,
                    SMD_SALDO_CAPITAL_LEASING = simulacionDetalle.SPD_SALDO_CAPITAL_LEASING,
                    SMD_INTERES_A_PAGAR_LEASING = simulacionDetalle.SPD_INTERES_A_PAGAR_LEASING,
                    SMD_SALDO_TOTAL_LEASING = simulacionDetalle.SPD_SALDO_TOTAL_LEASING,
                    SMD_AFECTACION_INDIVIDUAL_LEASING = simulacionDetalle.SPD_AFECTACION_INDIVIDUAL_LEASING,
                    SMD_ABONO_UNICO = (double)simulacionDetalle.SPD_ABONO_UNICO,
                    AFL_ID = simulacionDetalle.AFL_ID,
                    NUMERO_DOCUMENTO = request.Numero_Documento
                };

                detalles.Add(detalleSimulacion);
            }

            foreach (var detalle in detalles)
            {
                await this._creditoRepository.RestaurarDetallePlanCliente(detalle);
            }

            SimulacionPlanClienteDetalle simulacionPlanRespuesta = new SimulacionPlanClienteDetalle();
            simulacionPlanRespuesta.SimulacionPlan = simulacionPlan;
            simulacionPlanRespuesta.SimulacionDetalles = simulacionDetallePlan; 


            if (request != null)
            {
                return new Response<SimulacionPlanClienteDetalle> { Data = simulacionPlanRespuesta, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<SimulacionPlanClienteDetalle> { Data = null, IsSuccess = false, ReturnMessage = $"No hay prospeccion registrada con {numeroDocumento} " };
            }
        }


        public async Task<Response<LineaCreditoObligacion>> LineasCreditoRestaurar(DatosLineaCreditoObligacionDto datosLinea)
        {
            var lineaCredito  = await _creditoRepository.RecalcularLineaCreditoObligacion(datosLinea);
            if (lineaCredito != null)
            {
                return new Response<LineaCreditoObligacion> { Data = lineaCredito, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<LineaCreditoObligacion> { Data = null, IsSuccess = false, ReturnMessage = $"No hay prospeccion registrada con {lineaCredito.OLN_NUMERO_DOCUMENTO} " };
            }
        }

        public async Task<Response<PeritoDetalle>> InsertarPeritoDetalle(PeritoDetalleDto perito)
        {
            var peritoDetalle = await _creditoRepository.InsertarPeritoDetalle(perito);
            if (peritoDetalle != null)
            {
                return new Response<PeritoDetalle> { Data = peritoDetalle, IsSuccess = true, ReturnMessage = "Registro exitoso" };
            }
            else
            {
                return new Response<PeritoDetalle> { Data = null, IsSuccess = false, ReturnMessage = $"Error, al registrar el perito" };
            }
        }

        public async Task<Response<PeritoDetalle>> ConsultarPeritoDetalle(string soc_id)
        {
            var peritoDetalle = await _creditoRepository.ConsultarPeritoDetalle(soc_id);
            if (peritoDetalle != null)
            {
                return new Response<PeritoDetalle> { Data = peritoDetalle, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<PeritoDetalle> { Data = null, IsSuccess = false, ReturnMessage = $"Error, al consultar" };
            }
        }

        public async Task<Response<DatosEndeudamientoProspeccion>> ConsultarEndeudamientoProspeccion(string numeroDocumento)
        {
            var endeudameinto = await _creditoRepository.ConsultarDatosEndeudamientoProspeccion(numeroDocumento);
            if (endeudameinto != null)
            {
                return new Response<DatosEndeudamientoProspeccion> { Data = endeudameinto, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<DatosEndeudamientoProspeccion> { Data = null, IsSuccess = false, ReturnMessage = $"Error, al consultar" };
            }
        }


        public async Task<Response<SegurosDatosSolicitudCredito>> RegistrarSeguroSolicitudCredito(SegurosDatosSolicitudCreditoDto segurosDatos)
        {

            SegurosDatosSolicitudCreditoDto seguro = new SegurosDatosSolicitudCreditoDto();

            seguro.numeroDocumento = segurosDatos.numeroDocumento;
            seguro.soC_CONVENIO_ASEGURADORA = segurosDatos.soC_CONVENIO_ASEGURADORA;            
            seguro.asE_ID = segurosDatos.asE_ID;
            seguro.soC_CONVENIO_ASEGURADORA_HOGAR = segurosDatos.soC_CONVENIO_ASEGURADORA_HOGAR;
            seguro.asE_ID_HOGAR = segurosDatos.asE_ID_HOGAR;

            var seguroResult = await _creditoRepository.RegistrarSeguroSolicitudCredito(seguro);
            if (seguroResult != null)
            {
                return new Response<SegurosDatosSolicitudCredito> { Data = seguroResult, IsSuccess = true, ReturnMessage = "Registro exitoso" };
            }
            else
            {
                return new Response<SegurosDatosSolicitudCredito> { Data = null, IsSuccess = false, ReturnMessage = $"Error, al registrar seguro" };
            }
        }

        public async Task<Response<IEnumerable<ConsultaSegurosCredito>>> ConsultarSegurosSolicitudCredito(ConsultaSegurosCreditoDto request)
        {

            IEnumerable<ConsultaSegurosCredito> prospeccionResult = await _creditoRepository.ConsultarSegurosSolicitudCredito(request);
            if (prospeccionResult != null)
            {
                return new Response<IEnumerable<ConsultaSegurosCredito>> { Data = prospeccionResult, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<IEnumerable<ConsultaSegurosCredito>> { Data = prospeccionResult, IsSuccess = false, ReturnMessage = $"Error no hay datos" };
            }
        }

        

        public async Task<Response<InformacionCongelamientoRecursos>> RecursosActualesAfiliadoCredito(IdentificacionProspeccion request)
        {
            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.tipoDocumento, request.numeroDocumento)).Data;

            decimal ahorroProgramado = recursos.Saldos.First(s => s.Concepto.Equals("Ahorro")).Valor;
            decimal cesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Cesantias")).Valor;
            decimal intCesantiasProgramadas = recursos.Saldos.First(s => s.Concepto.Equals("Int Cesantias")).Valor;
            decimal intAhorroProgramados = recursos.Saldos.First(s => s.Concepto.Equals("Int Ahorros")).Valor;

            InformacionCongelamientoRecursos informacionCongelamiento = new InformacionCongelamientoRecursos(); 

            List<CongelamientoRecursosProspeccion> recursosCongelados = new List<CongelamientoRecursosProspeccion>
            {
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES CESANTÍAS",
                    CGR_VALOR = intCesantiasProgramadas,
                    CGR_SIMULACION_ID = Guid.NewGuid(),
                    CGR_ORDEN = 1,
                    CGR_NUMERO_DOCUMENTO = request.numeroDocumento,
                    CGR_TIPO_CREDITO = ""                    
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "INTERESES AHORRO",
                    CGR_VALOR = intAhorroProgramados,
                    CGR_SIMULACION_ID = Guid.NewGuid(),
                    CGR_ORDEN = 2,
                    CGR_NUMERO_DOCUMENTO = request.numeroDocumento,
                    CGR_TIPO_CREDITO = ""
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "CESANTÍAS",
                    CGR_VALOR = cesantiasProgramadas,
                    CGR_SIMULACION_ID = Guid.NewGuid(),
                    CGR_ORDEN = 3,
                    CGR_NUMERO_DOCUMENTO = request.numeroDocumento,
                    CGR_TIPO_CREDITO = ""
                },
                new CongelamientoRecursosProspeccion
                {
                    CGR_DESCRIPCION = "AHORROS",
                    CGR_VALOR = ahorroProgramado,
                    CGR_SIMULACION_ID = Guid.NewGuid(),
                    CGR_ORDEN = 4,
                    CGR_NUMERO_DOCUMENTO = request.numeroDocumento,
                    CGR_TIPO_CREDITO = ""
                }
            };

            informacionCongelamiento.ListaRecursosCongelados = recursosCongelados;
            informacionCongelamiento.NumeroDocumento = recursos.Identificacion; 
            informacionCongelamiento.PrimerNombre = recursos.PrimerNombre;
            informacionCongelamiento.SegundoNombre = recursos.SegundoNombre;
            informacionCongelamiento.PrimerApellido = recursos.PrimerApellido;
            informacionCongelamiento.SegundoApellido = recursos.SegundoApellido; 

            if (recursosCongelados != null)
            {
                return new Response<InformacionCongelamientoRecursos> { Data = informacionCongelamiento, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<InformacionCongelamientoRecursos> { Data = null, IsSuccess = false, ReturnMessage = $"No hay recursos congelados" };
            }
        }


        public async Task<Response<InformacionConceptosCongelados>> ConsultarCongelamientoRecursos(ConceptosCongeladosProspeccion request)
        {

            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.CON_TIPO_IDENTIFICACION, request.CON_NUMERO_DOCUMENTO)).Data;

            InformacionConceptosCongelados informacionConceptos = new InformacionConceptosCongelados();
            ConceptosCongeladosProspeccion congeladosProspeccion = new ConceptosCongeladosProspeccion();


            congeladosProspeccion.SLS_ID = request.SLS_ID;
            congeladosProspeccion.CON_NUMERO_DOCUMENTO = request.CON_NUMERO_DOCUMENTO;

            IEnumerable<ConceptosCongeladosProspeccion> listaConceptosCongelados = await _creditoRepository.ConsultarRecursosCongelados(congeladosProspeccion);

            informacionConceptos.ListaRecursosCongelados = listaConceptosCongelados;
            informacionConceptos.NumeroDocumento = recursos.Identificacion;
            informacionConceptos.PrimerNombre = recursos.PrimerNombre;
            informacionConceptos.SegundoNombre = recursos.SegundoNombre;
            informacionConceptos.PrimerApellido = recursos.PrimerApellido;
            informacionConceptos.SegundoApellido = recursos.SegundoApellido;

            if (informacionConceptos != null)
            {
                return new Response<InformacionConceptosCongelados> { Data = informacionConceptos, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<InformacionConceptosCongelados> { Data = null, IsSuccess = false, ReturnMessage = $"No hay recursos congelados" };
            }
        }


        public async Task<Response<InformacionConceptosCongelados>> ConsultarRecursosCongeladosCredito(ConceptosCongeladosProspeccion request)
        {

            var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.CON_TIPO_IDENTIFICACION, request.CON_NUMERO_DOCUMENTO)).Data;

            InformacionConceptosCongelados informacionConceptos = new InformacionConceptosCongelados();
            ConceptosCongeladosProspeccion congeladosProspeccion = new ConceptosCongeladosProspeccion();
            List<ConceptosCongeladosProspeccion> ListaRecursosCongelarNuevos = new List<ConceptosCongeladosProspeccion>(); 


            congeladosProspeccion.SLS_ID = request.SLS_ID;
            congeladosProspeccion.CON_NUMERO_DOCUMENTO = request.CON_NUMERO_DOCUMENTO;
            IEnumerable<ConceptosCongeladosProspeccion> listaConceptosCongelados = await _creditoRepository.ConsultarRecursosCongelados(congeladosProspeccion);

            var tipoActor = recursos.IdTipoActor;
            var tipoVinculacion = ""; 
            foreach(var vinculacion in recursos.TiposVinculacion)
            {
                if(tipoActor == vinculacion.IdTipoActor)
                {
                    tipoVinculacion = vinculacion.DescripcionTipoActor; 
                }
            }

            var tipoVinculacionEncontrada = await _creditoRepository.ConsultarTipoVinculacionCredito(tipoVinculacion, request.CON_TIPO_CREDITO);


            foreach (var conceptos in listaConceptosCongelados)
            {
                foreach (var vinculacion in tipoVinculacionEncontrada)
                {
                    if (conceptos.CON_DESCRIPCION == vinculacion.DTV_CONCEPTO &&
                        vinculacion.DTV_DESCRIPCION == tipoVinculacion &&
                        request.CON_TIPO_CREDITO == vinculacion.DTV_TIPO_CREDITO)
                    {
                        ConceptosCongeladosProspeccion conceptosCongelados = new ConceptosCongeladosProspeccion();

                        conceptosCongelados.CON_DESCRIPCION = conceptos.CON_DESCRIPCION;
                        conceptosCongelados.CON_VALOR = Math.Round((conceptos.CON_VALOR),2);
                        conceptosCongelados.CON_PORCENTAJE = Math.Round((vinculacion.DTV_PORCENTAJE),2);
                        conceptosCongelados.CON_VALOR_CALCULADO = Math.Round((conceptos.CON_VALOR * (vinculacion.DTV_PORCENTAJE / 100)));

                        conceptosCongelados.CON_VALOR_FORMAT = conceptos.CON_VALOR.ToString("0.00");
                        conceptosCongelados.CON_PORCENTAJE_FORMAT = vinculacion.DTV_PORCENTAJE.ToString("0.00");
                        conceptosCongelados.CON_VALOR_CALCULADO_FORMAT = conceptosCongelados.CON_VALOR_CALCULADO.ToString("0.00");

                        

                        ListaRecursosCongelarNuevos.Add(conceptosCongelados);
                    }
                }
            }


            informacionConceptos.ListaRecursosCongelar = ListaRecursosCongelarNuevos; 

            if (informacionConceptos != null)
            {
                return new Response<InformacionConceptosCongelados> { Data = informacionConceptos, IsSuccess = true, ReturnMessage = "Consulta exitosa" };
            }
            else
            {
                return new Response<InformacionConceptosCongelados> { Data = null, IsSuccess = false, ReturnMessage = $"No hay recursos congelados" };
            }
        }

        public double BuscarValorMaximoFinanciar(
   double valorFinanciarInicial,
   double ingresos,
   double porcentajeObjetivo,
   double obligaciones)
        {
            double minimo = 0;
            double maximo = valorFinanciarInicial;

            double tolerancia = 0.0001;
            int maxIteraciones = 100;

            while (maxIteraciones-- > 0)
            {
                double valorActual = (minimo + maximo) / 2;

                double totalObligaciones = obligaciones;

                double endeudamiento = CalcularEndeudamiento(
                    totalObligaciones,
                    ingresos).Result;

                if (Math.Abs(endeudamiento - porcentajeObjetivo) <= tolerancia)
                    return Math.Round(valorActual, 0);

                if (endeudamiento > porcentajeObjetivo)
                {
                    // El endeudamiento supera el objetivo.
                    maximo = valorActual;
                }
                else
                {
                    // Todavía puede financiar más.
                    minimo = valorActual;
                }
            }

            return Math.Round((minimo + maximo) / 2, 0);
        }

    }
}