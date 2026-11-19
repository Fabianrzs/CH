using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.TU;
using GA2.Application.Dto.Credito.TU.Responses;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Application.Main.TU;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Credito.TransUnion;
using GA2.Domain.Entities.ServiciosDinamicosEnt;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.LocalBlogStorage;
using GA2.Infraestructure.Repositories.ServiciosDinamicosRep;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Applicant = GA2.Application.Dto.Applicant;
using ApplicationData = GA2.Application.Dto.ApplicationData;
using Field = GA2.Application.Dto.Field;

namespace GA2.Domain.Core
{
    public class GestorTransUnionBusinessLogic : IGestorTransUnionBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ITransUnionRepository _transUnionRepository;
        private readonly IOptions<IdVisionOptions> _IdVisionoptions;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly IOptions<AqmOptions> _AqmOptions;
        private string _usuarioIDVision;
        private string _claveIDVision;
        private string _solucionId;
        private readonly ICreditoRepository _creditoRepository;
        private readonly ILocalBlogStorageRepository _localBlogStorageRepository;
        private readonly IServiciosDinamicosRepository _serviciosDinamicosRepository;
        private readonly ILogger<GestorTransUnionBusinessLogic> _logger;



        public GestorTransUnionBusinessLogic(IMapper mapper, ITransUnionRepository transUnionRepository, IOptions<AqmOptions> AqmOptions, 
            IOptions<IdVisionOptions> idVisionoptions, IOptions<BlobStorageOptions> options, ICreditoRepository creditoRepository,
            ILocalBlogStorageRepository localBlogStorageRepository,IServiciosDinamicosRepository serviciosDinamicosRepository, ILogger<GestorTransUnionBusinessLogic> logger )
        {
            _mapper = mapper;
            _IdVisionoptions = idVisionoptions;
            _options = options;
            _AqmOptions = AqmOptions;
            _transUnionRepository = transUnionRepository;
            _usuarioIDVision = _IdVisionoptions.Value.usuarioIDVision;
            _claveIDVision = _IdVisionoptions.Value.claveIDVision;
            _solucionId = _IdVisionoptions.Value.solucionId;
            _creditoRepository = creditoRepository;
            _localBlogStorageRepository = localBlogStorageRepository;
            _serviciosDinamicosRepository = serviciosDinamicosRepository;
            _logger = logger; 

        }
        public async Task<Response<VLL_VALIDACION_LOG_OTPDto>> CrearLogOTP(VLL_VALIDACION_LOG_OTPDto datosLog)
        {
            var registroLog = this._mapper.Map<VLL_VALIDACION_LOG_OTP>(datosLog);
            return new Response<VLL_VALIDACION_LOG_OTPDto> { Data = this._mapper.Map<VLL_VALIDACION_LOG_OTPDto>(await this._transUnionRepository.CrearLogOTP(registroLog)) };

        }

        //public async Task<Response<Guid>> CrearValidacion(VLI_VALIDACION_IDENTIDADDto valida)
        //{
        //    var actualiza = this._mapper.Map<VLI_VALIDACION_IDENTIDAD>(valida);

        //    return new Response<Guid> { Data = this._mapper.Map<Guid>(await this._transUnionRepository.CrearValidacion(actualiza)) };
        //}

        //public async Task<Response<int>> ActualizarOtpValidacion(VLI_VALIDACION_IDENTIDADDto valida)
        //{
        //    var actualiza = this._mapper.Map<VLI_VALIDACION_IDENTIDAD>(valida);

        //    return new Response<int> { Data = this._mapper.Map<int>(await this._transUnionRepository.ActualizarOtpValidacion(actualiza)) };
        //}


        public async Task<Response<SolicitudCreditoProductoDto>> CrearLogTu(SolicitudCreditoProductoDto datosLog)
        {
            var registroLog = this._mapper.Map<SolicitudCreditoProductoGral>(datosLog);

            return new Response<SolicitudCreditoProductoDto> { Data = this._mapper.Map<SolicitudCreditoProductoDto>(await this._transUnionRepository.CrearLogTu(registroLog)) };
        }

        public async Task<Response<ResponseTu>> ProspeccionCreditoTransUnion(ProspeccionTransUnionRequestDto prospeccionTransUnionRequest)
        {

            await _creditoRepository.RegistroLogTUSimulador("GestorTransUnionBL - ProspeccionCreditoTransUnion: ", prospeccionTransUnionRequest.ToString(), "INICIO DEL METODO NO HAY RESPUESTA"); 
            bool IsSuccess = true;
            Response<SocSlsSimulacionCreditoDto> validacion;

            try
            {
                _logger.LogInformation("Consulta de ObtenerSlsSocSolicitudCredito");

                validacion = new Response<SocSlsSimulacionCreditoDto>
                {
                    Data = this._mapper.Map<SocSlsSimulacionCreditoDto>(
                        await this._creditoRepository.ObtenerSlsSocSolicitudCredito(prospeccionTransUnionRequest.SLS_ID)
                    )
                };

                await _creditoRepository.RegistroLogTUSimulador("GestorTransUnionBL - ObtenerSlsSocSolicitudCredito try: ", prospeccionTransUnionRequest.SLS_ID.ToString(), validacion.ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError("[{method}]ObtenerSlsSocSolicitudCredito"+ ex);
                await _creditoRepository.RegistroLogTUSimulador("GestorTransUnionBL - ObtenerSlsSocSolicitudCredito catch: ", prospeccionTransUnionRequest.SLS_ID.ToString(), ex.ToString());

                IsSuccess = false;

                await CrearLogTu(new SolicitudCreditoProductoDto()
                {
                    VLI_ID = Guid.NewGuid(),
                    VLT_ID = Guid.NewGuid(),
                    VLT_SERVICIO = "TU - OBTENER SLS/SOC",
                    VLT_PASO_TRANSACCION = "OBTENER_SLS_SOC",
                    VLT_XML_SOLICITUD = prospeccionTransUnionRequest.SLS_ID.ToString(),
                    VLT_XML_RESPUESTA = ex.ToString(),
                    VLT_ID_APICACION = "ObtenerSlsSocSolicitudCredito",
                    VLT_CODIGO_RESPUESTA = "false",
                    VLT_TIPO_RESPUESTA = "ERROR",
                    VLT_DESCRIPCION_RESPUESTA = "Error al obtener SLS/SOC solicitud crédito",
                    VLT_FECHA_ACTUALIZA = DateTime.Now
                });

                validacion = new Response<SocSlsSimulacionCreditoDto> { Data = null };
            }

            DateTime slsFecha = new DateTime();
            string requestLog = string.Empty;
            string responseLog = string.Empty;
            Application.Dto.DCResponse respuestaLog = new Application.Dto.DCResponse();

            DateTime fechaActual = DateTime.Today;
            if (validacion.Data != null)
            {
                slsFecha = validacion.Data.slsFecha;
            }

            // Calcular la diferencia en meses entre la fecha actual y slsFecha
            TimeSpan diferencia = fechaActual - slsFecha;

            try
            {

                _logger.LogInformation("Validacion de 30 días de TU: "+diferencia);
                await _creditoRepository.RegistroLogTUSimulador("Validacion de 30 días de TU: ", diferencia.TotalDays.ToString(), "");


                // Validar si la diferencia en meses es mayor a 1
                if (diferencia.TotalDays < 30)
                {
                    await _creditoRepository.RegistroLogTUSimulador("Entra por el condicional de 30 dias: ", diferencia.TotalDays.ToString(), "");

                    ResponseTu responseTu = new ResponseTu();
                    try
                    {
                        try
                        {
                            if (validacion.Data.slsId != null && validacion.Data.socId == null)
                            {
                                responseTu.response = await DescargarXMLTransUnion(validacion.Data.slsXml);
                                await _creditoRepository.RegistroLogTUSimulador("DESCARGAR DEL ARCHIVO #1: ", validacion.Data.slsXml.ToString(), responseTu.response.ToString());

                                _logger.LogInformation("DESCARGA EL ARCHIVO #1: "+ responseTu.response);

                            }
                            else if (validacion.Data.slsId != null && validacion.Data.socId != null)
                            {
                                if (validacion.Data.slsXml != null && validacion.Data.socXml == null)
                                {
                                    responseTu.response = await DescargarXMLTransUnion(validacion.Data.slsXml);
                                    await _creditoRepository.RegistroLogTUSimulador("DESCARGAR DEL ARCHIVO #2: ", validacion.Data.slsXml.ToString(), responseTu.response.ToString());

                                    _logger.LogInformation("DESCARGA EL ARCHIVO #2: "+ responseTu.response);

                                }
                                else
                                {
                                    responseTu.response = await DescargarXMLTransUnion(validacion.Data.slsXml);
                                    await _creditoRepository.RegistroLogTUSimulador("DESCARGAR DEL ARCHIVO #3: ", validacion.Data.slsXml.ToString(), responseTu.response.ToString());

                                    _logger.LogInformation("DESCARGA EL ARCHIVO #3: "+ responseTu.response);

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            await _creditoRepository.RegistroLogTUSimulador("DESCARGAR DEL ARCHIVO CONDICIONAL 30 DIAS IF: ", validacion.Data.slsXml.ToString(), ex.ToString());

                            IsSuccess = false;

                            await CrearLogTu(new SolicitudCreditoProductoDto()
                            {
                                VLI_ID = Guid.NewGuid(),
                                VLT_ID = Guid.NewGuid(),
                                VLT_SERVICIO = "TU - DESCARGA XML",
                                VLT_PASO_TRANSACCION = "DESCARGAR_XML",
                                VLT_XML_SOLICITUD = validacion?.Data?.slsXml ?? validacion?.Data?.socXml,
                                VLT_XML_RESPUESTA = ex.ToString(),
                                VLT_ID_APICACION = "XML",
                                VLT_CODIGO_RESPUESTA = "false",
                                VLT_TIPO_RESPUESTA = "ERROR",
                                VLT_DESCRIPCION_RESPUESTA = "Error al descargar XML TransUnion",
                                VLT_FECHA_ACTUALIZA = DateTime.Now
                            });

                            responseTu.response = string.Empty;
                        }

                        try
                        {

                            await _creditoRepository.RegistroLogTUSimulador("INICIA EL TRY QUE DESERIALIZA EL XML DENTRO DE LOS 30 DIAS", "", "");

                            var responseFinal = Serialization.DeserializeDC(responseTu.response);

                            await _creditoRepository.RegistroLogTUSimulador("DeserializeDC ", responseTu.response.ToString(), responseFinal.ToString());

                            var Res = await XmlToDcResponse(responseTu.response);
                            await _creditoRepository.RegistroLogTUSimulador("XmlToDcResponse ", responseTu.response.ToString(), Res.ToString());


                            _logger.LogInformation("responseFinal linea 215 GestorTransUnionBL: "+ responseFinal);
                            _logger.LogInformation("responseFinal linea 216 GestorTransUnionBL: "+ Res);

                            

                            int scoreResponse = Res.ContextData.Field[0].Applicants.Applicant.Score != null ? int.Parse(Res.ContextData.Field[0].Applicants.Applicant.Score) : 0;

                            await _creditoRepository.RegistroLogTUSimulador("Res.Status ", validacion.Data.slsXml.ToString(), Res.Status.ToString());
                            await _creditoRepository.RegistroLogTUSimulador("DecisionHomologada ", validacion.Data.slsXml.ToString(), Res.ContextData.Field.Where(x => x.Key == "DecisionHomologada").FirstOrDefault().Text.ToString());
                            await _creditoRepository.RegistroLogTUSimulador("SCORE", validacion.Data.slsXml.ToString(), scoreResponse.ToString());



                            if ((Res.Status != "Success" || Res.ContextData.Field.Where(x => x.Key == "DecisionHomologada").FirstOrDefault().Text == "NO VIABLE"))
                            {
                                IsSuccess = false;
                                ApplicationData fieldValidacion = Serialization.DeserializeRC(responseFinal.ContextData.Field.Where(x => x.Key == "ApplicationData").FirstOrDefault().Text);
                                await _creditoRepository.RegistroLogTUSimulador("fieldValidacion", responseFinal.ContextData.Field.Where(x => x.Key == "ApplicationData").FirstOrDefault().Text.ToString(), fieldValidacion.ToString());

                                _logger.LogInformation("fieldValidacion linea 226 GestorTransUnionBL: ");


                                var aplicante = Res.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants?.Applicant;
                                await _creditoRepository.RegistroLogTUSimulador("aplicante", Res.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants?.Applicant.ToString(), aplicante.ToString());
                                _logger.LogInformation("aplicante linea 231 GestorTransUnionBL: ");

                                var aplicanteData = Res.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
                                await _creditoRepository.RegistroLogTUSimulador("aplicanteData", Res.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData.ToString(), aplicanteData.ToString());
                                _logger.LogInformation("aplicanteData linea 234 GestorTransUnionBL: ");

                                string descripcionResponse = aplicante.CriteriaPostBureauCOLData != null ? " Descripción: " + aplicante.CriteriaPostBureauCOLData.PostBureauReasons.Reason.Description : "";
                                await _creditoRepository.RegistroLogTUSimulador("aplicanteData", aplicante.CriteriaPostBureauCOLData != null ? " Descripción: " + aplicante.CriteriaPostBureauCOLData.PostBureauReasons.Reason.Description : "".ToString(), descripcionResponse.ToString());

                                _logger.LogInformation("descripcionResponse linea 237 GestorTransUnionBL: ");


                                responseTu = new ResponseTu
                                {
                                    detalleError = "Decision Homologada: " + fieldValidacion.DecisionHomologada + " ,Motivo: " + aplicanteData.UltimaEtapaFlujo + descripcionResponse,
                                    idAplication = Res.ResponseInfo.ApplicationId,
                                    response = null
                                };

                            }
                            await _creditoRepository.RegistroLogTUSimulador("FINALIZA EL TRY QUE DESERIALIZA EL XML DENTRO DE LOS 30 DIAS", "", responseTu.ToString());

                        }
                        catch (Exception e)
                        {

                            _logger.LogError("error linea 254 GestorTransUnionBL: "+ e);
                            await _creditoRepository.RegistroLogTUSimulador("INICIA EL CATCH QUE DESERIALIZA EL XML DENTRO DE LOS 30 DIAS", "", e.ToString());


                            IsSuccess = false;
                            var error = await XmlToDcResponseDos(responseTu.response);
                            _logger.LogError("(Nombre variable)error linea 254 GestorTransUnionBL: "+ error); 

                            var aplicanteData = error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
                            _logger.LogError("aplicanteData linea 258 GestorTransUnionBL: ", aplicanteData);


                            string errores = string.Join(", ", aplicanteData.InputValReasonCodes.Reason
                            .Where(x => x.Description != null)
                            .Select(x => x.Description));

                            _logger.LogError("(Lista errores)errores linea 262 GestorTransUnionBL: "+ errores);

                            await _creditoRepository.RegistroLogTUSimulador("errores", "", errores.ToString());
                            await _creditoRepository.RegistroLogTUSimulador("error = await XmlToDcResponseDos(responseTu.response);", "", error.ResponseInfo.ApplicationId.ToString());


                            responseTu = new ResponseTu
                            {
                                detalleError = errores,
                                idAplication = error.ResponseInfo.ApplicationId,
                                response = string.Empty
                            };
                            return new Response<ResponseTu> { Data = responseTu, IsSuccess = IsSuccess };
                        }
                    }
                    catch (Exception ex)
                    {
                        await _creditoRepository.RegistroLogTUSimulador("CATCH TRY PRINCIAL DEL CONDICIONAL DE 30 DIAS: ", responseTu.ToString(), ex.ToString());

                        _logger.LogError("Error al descargar el archivo XML linea 280 GestorTransUnionBL: "+ ex);
                    }
                    return new Response<ResponseTu> { Data = responseTu, IsSuccess = IsSuccess };
                }
                else
                {
                    await _creditoRepository.RegistroLogTUSimulador("ENTRA POR LA CONDICION ELSE SLS CON MAS DE 30 DIAS", "INICIA LA NUEVA PETICION A TU", "");

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



                    var fechaExpedicion = Convert.ToDateTime(prospeccionTransUnionRequest.FechaExpedicionIdentificacion).ToString("dd/MM/yyyy");
                    Applicant ap = new()
                    {
                        IdNumber = prospeccionTransUnionRequest.Identificacion,
                        IdType = prospeccionTransUnionRequest.TipoIdentificacion,
                        IdExpeditionDate = fechaExpedicion,
                        RecentPhoneNumber = prospeccionTransUnionRequest.Telefono,
                        LastName = prospeccionTransUnionRequest.Apellidos,
                        Occupation = "EMPLEADO",
                        MonthlyIncome = prospeccionTransUnionRequest.SalarioMensual.ToString(),
                    };


                    ApplicationData apData = new()
                    {
                        Product = "CONSUMO"
                    };

                    _logger.LogInformation("Parametros para petición en TU linea 302-317 GestorTransUnionBL: "+ ap);


                    dCRequest.Fields = new() { Field = new List<Field>() { new Field { Key = "PrimaryApplicant", Text = ap.Serialize<Applicant>() }, new Field { Key = "ApplicationData", ApplicationData = apData } } };
                    //dCRequest.Fields.Add(new() { Key = "ApplicationData", ApplicationData = apd });

                    //Guarda consumo de TransUnion
                    var respuesta = tu.ConsumirTU(dCRequest, out string request, out string response);

                    await _creditoRepository.RegistroLogTUSimulador("INICIA EL CONSUMO A TU AUTENTICACION", dCRequest.ToString(), respuesta.ToString());

                    await _creditoRepository.RegistroLogTUSimulador("Guarda consumo de TransUnion", request.ToString(), response.ToString());


                    _logger.LogInformation("request 326: "+ request);
                    _logger.LogInformation("response 326: "+ response);


                    responseTu.response = response;

                    requestLog = request; 
                    responseLog = response;
                    respuestaLog = respuesta;

                    await _creditoRepository.RegistroLogTUSimulador("VALIDACIONES PARA EL OK Y CONSULTAR EL HISTORIAL CREDITICIO", "", "");

                    await _creditoRepository.RegistroLogTUSimulador("VALOR ESPERADO: Success", respuesta.Status.ToString(), "");
                    await _creditoRepository.RegistroLogTUSimulador("VALOR ESPERADO: Success", respuesta.Authentication.Status.ToString(), "");
                    await _creditoRepository.RegistroLogTUSimulador("VALOR ESPERADO: FirstTimeLogIn", respuesta.Authentication.Status.ToString(),"");


                    _logger.LogInformation("respuesta 326: "+ respuesta.Status.ToString());

                    if (respuesta.Status.ToString() == "Success" && (respuesta.Authentication.Status.ToString() == "Success" || respuesta.Authentication.Status.ToString() == "FirstTimeLogIn"))
                    {
                        await _creditoRepository.RegistroLogTUSimulador("VALIDACIONES PARA EL OK Y CONSULTAR EL HISTORIAL CREDITICIO", "EXITOSOSO", respuesta.ToString());

                        ApplicationData fieldValidacion = Serialization.DeserializeRC(respuesta.ContextData.Field.Where(x => x.Key == "ApplicationData").FirstOrDefault().Text);
                        if (prospeccionTransUnionRequest.TraerHistorialCrediticio.Equals("Y"))
                        {
                            DCRequest dCRequestHistorial = new()
                            {
                                Authentication = new() { UserId = _AqmOptions.Value.usuarioAqm, Password = _AqmOptions.Value.claveAqm, Type = "OnDemand" },
                                RequestInfo = new() { ExecutionMode = "Send", ApplicationId = respuesta.ResponseInfo.ApplicationId, QueueName = "DocVerify" },
                                Fields = new() { Field = new List<Field>() { new Field() { Key = "GenerateReport", Text = "Y" } } }
                            };

                            await _creditoRepository.RegistroLogTUSimulador("INICIA EL CONSUMO PARA TU HISTORIAL CREDICIO", "dCRequestHistorial", dCRequestHistorial.ToString());


                            var respuesta1 = tu.ConsumirTU(dCRequestHistorial, out string request1, out string response1);


                            await _creditoRepository.RegistroLogTUSimulador("REQUEST", "var respuesta1 = tu.ConsumirTU ", request1.ToString());
                            await _creditoRepository.RegistroLogTUSimulador("RESPONSE", "var respuesta1 = tu.ConsumirTU ", response1.ToString());


                            _logger.LogInformation("request 352: "+ request1);
                            _logger.LogInformation("response 352: "+ response1);
                            _logger.LogInformation("response 352: "+ respuesta1.Status.ToString());



                            await _creditoRepository.RegistroLogTUSimulador("VALOR ESPERADO: Success", respuesta.Status.ToString(), "");
                            await _creditoRepository.RegistroLogTUSimulador("VALOR ESPERADO: VIABLE - NO VIABLE - PENDIENTE", respuesta1?.ContextData?.Field?.Where(x => x.Key == "DecisionHomologada").FirstOrDefault().Text.ToString(), "");
                        
                            if (respuesta1.Status.ToString() == "Success" || respuesta1?.ContextData?.Field?.Where(x => x.Key == "DecisionHomologada").FirstOrDefault().Text == "NO VIABLE")
                            {
                                var applicantsR = respuesta1.ContextData.Field.Where(x => x.Key.Equals("Applicants"));

                                #region Creacion y Guardado XML-TU
                                byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(response1);
                                using (var memoryStream = new MemoryStream(xmlBytes))
                                {

                                    _logger.LogInformation("memoryStream 366: "+ memoryStream);
                                    _logger.LogInformation("XMLTransunion 366: "+ "XMLTransunion - " + prospeccionTransUnionRequest.Identificacion);
                                    _logger.LogInformation("Cantidad de Bytes 370: "+ xmlBytes.Length);

                                    IFormFile formFile = new FormFile(memoryStream, 0, xmlBytes.Length, "XMLTransunion - " + prospeccionTransUnionRequest.Identificacion, "XMLTransunion - " + prospeccionTransUnionRequest.Identificacion + ".xml");
                          
                                    await _creditoRepository.RegistroLogTUSimulador("GUARDAR EL ARCHIVO CUANDO NO CUMPLE CON LAS CONDICIONES: FirstTimeLogIn", formFile.ToString(), prospeccionTransUnionRequest.SLS_ID.ToString());

                                    await GuardarXmlTU(formFile, prospeccionTransUnionRequest.SLS_ID);

                                    await _creditoRepository.RegistroLogTUSimulador("fin del cargue del archivo", "GuardarXmlTU","");

                                }
                                #endregion
                                responseTu.response = response1;
                                if (applicantsR.Count<Field>() > 0)
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
                                    _logger.LogInformation("Linea 388 punto ");

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
                                    await _creditoRepository.RegistroLogTUSimulador("VALIDACIONES EXISTOSA", "ELSE DE VALIDACIONES", "ES CORRECTO");

                                    IsSuccess = false;
                                    #region Creacion y Guardado XML-TU
                                    byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(responseTu.response);
                                    using (var memoryStream = new MemoryStream(xmlBytes))
                                    {
                                        _logger.LogInformation("memoryStream 415: "+ memoryStream);
                                        _logger.LogInformation("XMLTransunion 417: "+ "XMLTransunion - " + prospeccionTransUnionRequest.Identificacion);
                                        _logger.LogInformation("Cantidad de Bytes 418: "+ xmlBytes.Length);


                                        IFormFile formFile = new FormFile(memoryStream, 0, xmlBytes.Length, "XMLTransunion - " + prospeccionTransUnionRequest.Identificacion, "XMLTransunion - " + prospeccionTransUnionRequest.Identificacion + ".xml");
                                        await _creditoRepository.RegistroLogTUSimulador("GUARDAR EL ARCHIVO CUANDO CUMPLE CON LAS CONDICIONES: FirstTimeLogIn", formFile.ToString(), prospeccionTransUnionRequest.SLS_ID.ToString());

                                        await GuardarXmlTU(formFile, prospeccionTransUnionRequest.SLS_ID);

                                        await _creditoRepository.RegistroLogTUSimulador("fin del cargue del archivo", "GuardarXmlTU", "");
                                    }
                                    #endregion
                                    var error = await XmlToDcResponse(responseTu.response);


                                    await _creditoRepository.RegistroLogTUSimulador("XmlToDcResponse", responseTu.response.ToString(), error.ToString());
                                    _logger.LogInformation("(variable Error)error Linea 425"+ error); 

                                    var aplicante = error.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants?.Applicant;
                                    await _creditoRepository.RegistroLogTUSimulador("aplicante", error.ContextData.Field.Where(x => x.Applicants != null).FirstOrDefault()?.Applicants?.Applicant.ToString(), aplicante.ToString());

                                    _logger.LogInformation("aplicante Linea 429"+ aplicante);

                                    var aplicanteData = error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
                                    await _creditoRepository.RegistroLogTUSimulador("aplicanteData", error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData.ToString(), aplicanteData.ToString());

                                    _logger.LogInformation("aplicanteData Linea 432"+ aplicanteData);


                                    await _creditoRepository.RegistroLogTUSimulador("responseTu = new ResponseTu", "FIN DEL CONDICIONAL ELSE", responseTu.ToString());

                                    responseTu = new ResponseTu
                                    {
                                        detalleError = "Decision Homologada: " + fieldValidacion.DecisionHomologada + " ,Motivo: " + aplicanteData.UltimaEtapaFlujo,
                                        idAplication = error.ResponseInfo.ApplicationId,
                                        response = string.Empty
                                    };
                                }
                                catch (Exception ex)
                                {

                                    await _creditoRepository.RegistroLogTUSimulador("INICIAL EL CATCH DE 544","","");

                                    var error = await XmlToDcResponseDos(responseTu.response);
                                    _logger.LogError("(variable Error)error Linea 446"+ error);

                                    var aplicanteData = error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData;
                                    await _creditoRepository.RegistroLogTUSimulador("aplicanteData", error.ContextData.Field.Where(x => x.ApplicationData != null).FirstOrDefault()?.ApplicationData.ToString(), aplicanteData.ToString());

                                    string errores = string.Join(", ", aplicanteData.InputValReasonCodes.Reason
                                    .Where(x => x.Description != null)
                                    .Select(x => x.Description));

                                    _logger.LogError("aplicante Linea 452"+ errores);

                                    await _creditoRepository.RegistroLogTUSimulador("FINALIZA EL CACTH", errores, responseTu.ToString());


                                    responseTu = new ResponseTu
                                    {
                                        detalleError = errores,
                                        idAplication = error.ResponseInfo.ApplicationId,
                                        response = string.Empty
                                    };
                                    return new Response<ResponseTu> { Data = responseTu, IsSuccess = IsSuccess };
                                }
                            }
                        }
                        else
                        {

                            await _creditoRepository.RegistroLogTUSimulador("SOLO CUANDO NO MANDA LA Y PARA CONSULTAR EL HISTORIAL CREDITICIO", "", "");

                            _logger.LogError("request solicitud AQM - (Ex) 470"+ request);
                            _logger.LogError("response solicitud AQM - (Ex) 471"+ response);

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

                    return new Response<ResponseTu> { Data = responseTu, IsSuccess = IsSuccess };
                }

            }
            catch (Exception)
            {
                _logger.LogError("solicitud AQM - (Ex) 498");
                _logger.LogError("solicitud AQM - (Ex) 499");
                ResponseTu responseTu = new ResponseTu
                {
                    idAplication = "0",
                    codigoRechazo = 0,
                    detalleRechazo = "Error al cargar el archivo XML"
                };
                await CrearLogTu(new SolicitudCreditoProductoDto()
                {
                    VLI_ID = Guid.NewGuid(),
                    VLT_ID = Guid.NewGuid(),
                    VLT_SERVICIO = "TU",
                    VLT_PASO_TRANSACCION = "AQM",
                    VLT_XML_SOLICITUD = requestLog,
                    VLT_XML_RESPUESTA = responseLog,
                    VLT_ID_APICACION = respuestaLog.ResponseInfo.ApplicationId,
                    VLT_CODIGO_RESPUESTA = "true",
                    VLT_TIPO_RESPUESTA = "true",
                    VLT_DESCRIPCION_RESPUESTA = respuestaLog.ContextData.Field.Where(x => x.Key == "Decision").FirstOrDefault().Text,
                    VLT_FECHA_ACTUALIZA = DateTime.Now
                });

                return new Response<ResponseTu> { Data = responseTu, IsSuccess = IsSuccess };
            }
        }


        private async Task<ResponseGeneralTu.DCResponse> XmlToDcResponse(string xml)
        {
            xml = xml
               .Replace("&amp;", "&")
               .Replace(":q1", "")
               .Replace("q1:", "")
               .Replace("&lt;", "<")
               .Replace("&gt;", ">")
               .Replace("&quot;", "\"");
            XmlSerializer serializer = new XmlSerializer(typeof(ResponseGeneralTu.DCResponse));

            using (StringReader reader = new StringReader(xml))
            {
                var data = await Task.Run(() => serializer.Deserialize(reader));
                return (ResponseGeneralTu.DCResponse)data;
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



        public async Task<Response<SolicitudCreditoProductoDto>> RegistroLogTU(SolicitudCreditoProductoDto datosLog)
        {
            var registroLog = this._mapper.Map<SolicitudCreditoProductoGral>(datosLog);
            return new Response<SolicitudCreditoProductoDto> { Data = this._mapper.Map<SolicitudCreditoProductoDto>(await this._transUnionRepository.RegistroLogTU(registroLog)) };
        }

        private async Task<bool> GuardarXmlTU(IFormFile FileXMLTU, Guid IdSolicitud)
        {

            try
            {
                var RutaImagen = await CargarXMLTransUnion(FileXMLTU);
                _logger.LogInformation("Iniciar la actulización SLS del archivo cargado");
                await _creditoRepository.RegistroLogTUSimulador("Iniciar la actulización SLS del archivo cargado", "RUTA: ", RutaImagen.ToString());

                RequestUpdateSlsTuDto requestUpdateSlsTuDto = new RequestUpdateSlsTuDto()
                {
                    Sls_Id = IdSolicitud,
                    xml = RutaImagen.Data,
                    Fecha_Reporte_TU = DateTime.Now
                };

                _logger.LogInformation("data de la actulización SLS del archivo cargado: "+ requestUpdateSlsTuDto);

                var respuesta = await _creditoRepository
                    .ActualizarSlsReporteTU(this._mapper.Map<UpdateSlsTu>(requestUpdateSlsTuDto));

                bool esExitoso = respuesta != null;

                _logger.LogInformation("Actualización exitosa: " + esExitoso);

                await _creditoRepository.RegistroLogTUSimulador("CARGUE DEL ARCHIVO", "", esExitoso.ToString());


                return esExitoso;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Actualización exitosa: " + false);
                await _creditoRepository.RegistroLogTUSimulador("ERROR AL CARGUE DEL ARCHIVO", "", e.ToString());


                return false; 
            }
            
        }


        public async Task<Response<string>> CargarXMLTransUnion(IFormFile request)
        {
            try
            {
                _logger.LogInformation("Inicia el cargue del archivo TU ");
                await _creditoRepository.RegistroLogTUSimulador("Inicia el cargue del archivo TU", "", request.ToString());

                ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

                await _creditoRepository.RegistroLogTUSimulador("VALIDACION BLOB STORAGE", "", blogStorage.SRD_ESTADO.ToString());

                if (blogStorage.SRD_ESTADO)
                {
                    await _creditoRepository.RegistroLogTUSimulador("VALIDACION BLOB STORAGE", "ENTRO POR EL BLOB STORAGE", blogStorage.SRD_ESTADO.ToString());

                    _logger.LogInformation("BlobStorage: encendido");

                    var _request = request;
                    FileInfo fi = new FileInfo(_request.FileName);
                    BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "trans-union");
                    await container.CreateIfNotExistsAsync();
                    var newName = request.Name + fi.Extension;
                    BlobClient blob = container.GetBlobClient(newName);
                    var result = await blob.UploadAsync(request.OpenReadStream(), true);
                    return new Response<string> { Data = newName };
                }
                else
                {
                    _logger.LogInformation("BlobStorage: apagado");
                    await _creditoRepository.RegistroLogTUSimulador("VALIDACION BLOB STORAGE", "CARPETA LOCAL", blogStorage.SRD_ESTADO.ToString());

                    return await _localBlogStorageRepository.CargarXMLLocal(request); 
                }
                    
            }
            catch (Exception ex)
            {
                _logger.LogError("Error guardado documento en el Blob Storage"); 
                return new Response<string> { Data = string.Empty, IsSuccess = false, ReturnMessage = "Error guardado documento en el Blob Storage" };
            }
        }

        public async Task<string> DescargarXMLTransUnion(string filename)
        {

            try
            {
                _logger.LogInformation("Inicia la descargar del archivo: " + filename);

                ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
                if (blogStorage.SRD_ESTADO)
                {
                    _logger.LogInformation("BlobStorage: encendido"); 
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
                else
                {
                    _logger.LogInformation("BlobStorage: apagado");

                    return await _localBlogStorageRepository.DescargarXMLLocal(filename);
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al descargar el archivo: " + filename);
                return "Error al decargar el archivo" + filename; 
            }
            
                
        }




        //public async Task<Response<Guid>> CrearValidacion(VLI_VALIDACION_IDENTIDADDto valida)
        //{
        //    var actualiza = this._mapper.Map<VLI_VALIDACION_IDENTIDAD>(valida);

        //    return new Response<Guid> { Data = this._mapper.Map<Guid>(await this._transUnionRepository.CrearValidacion(actualiza)) };
        //}

        //public async Task<Response<int>> ActualizarOtpValidacion(VLI_VALIDACION_IDENTIDADDto valida)
        //{
        //    var actualiza = this._mapper.Map<VLI_VALIDACION_IDENTIDAD>(valida);

        //    return new Response<int> { Data = this._mapper.Map<int>(await this._transUnionRepository.ActualizarOtpValidacion(actualiza)) };
        //}


        //        public Guid CrearSolicitud(SLS_SOLICITUD_SIMULACION solicitud, SDP_SIMULACION_DATOS_PERSONALES datos, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SLS_ID = solicitud.SLS_ID,
        //                    SDP_ID = datos.SDP_ID,
        //                    TCR_ID = solicitud.TCR_ID,
        //                    SLS_FECHA_SOLICITUD = solicitud.SLS_FECHA_SOLICITUD,
        //                    FRZ_ID = datos.FRZ_ID,
        //                    CTG_ID = datos.CTG_ID,
        //                    GRD_ID = datos.GRD_ID,
        //                    SDP_NUMERO_DOCUMENTO = datos.SDP_NUMERO_DOCUMENTO,
        //                    SDP_NOMBRES_APELLIDOS = datos.SDP_NOMBRES_APELLIDOS,
        //                    SDP_EDAD = datos.SDP_EDAD,
        //                    SDP_FECHA_AFILIACION = datos.SDP_FECHA_AFILIACION,
        //                    DPD_ID = datos.DPD_ID,
        //                    DPC_ID = datos.DPC_ID,
        //                    SDP_DIRECCION = datos.SDP_DIRECCION,
        //                    SDP_TELEFONO_FIJO = datos.SDP_TELEFONO_FIJO,
        //                    SDP_TELEFONO_CELULAR = datos.SDP_TELEFONO_CELULAR,
        //                    SDP_E_MAIL = datos.SDP_E_MAIL,
        //                    SDP_CUOTAS = datos.SDP_CUOTAS,
        //                    SDP_REGIMEN = datos.SDP_REGIMEN,
        //                    SDP_VALOR_INMUEBLE = datos.SDP_VALOR_INMUEBLE,
        //                    TVV_ID = datos.TVV_ID,
        //                    DPD_ID_INMUEBLE = datos.DPD_ID_INMUEBLE,
        //                    DPC_ID_INMUEBLE = datos.DPC_ID_INMUEBLE,
        //                    SLS_USUARIO_ACTUALIZA = Guid.NewGuid(),
        //                    SLS_FECHA_ACTUALIZA = DateTime.Now
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SDP_SIMULACION_DATOS_PERSONALES>("CrearSolicitudSimulacion", objeto, CommandType.StoredProcedure, ref Error);
        //                return retorno.FirstOrDefault().SLS_ID;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        public Guid CrearDatosFinancierosSolicitud(SDF_SIMULACION_DATOS_FINANCIEROS datos, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SDF_ID = datos.SDF_ID,
        //                    SLS_ID = datos.SLS_ID,
        //                    SDF_VALOR_SALARIO = datos.SDF_VALOR_SALARIO,
        //                    SDF_VALOR_OTROS_INGRESOS = datos.SDF_VALOR_OTROS_INGRESOS,
        //                    SDF_VALOR_OTROS_INGRESOS1 = datos.SDF_VALOR_OTROS_INGRESOS1,
        //                    SDF_VALOR_OTROS_INGRESOS2 = datos.SDF_VALOR_OTROS_INGRESOS2,
        //                    SDF_VALOR_OTROS_INGRESOS3 = datos.SDF_VALOR_OTROS_INGRESOS3,
        //                    SDF_VALOR_OTROS_INGRESOS4 = datos.SDF_VALOR_OTROS_INGRESOS4,
        //                    SDF_DESCRIPCION_SALARIO = datos.SDF_DESCRIPCION_SALARIO,
        //                    SDF_DESCRIPCION_OTRO_INGRESOS = datos.SDF_DESCRIPCION_OTRO_INGRESOS,
        //                    SDF_DESCRIPCION_OTRO_ING1 = datos.SDF_DESCRIPCION_OTRO_ING1,
        //                    SDF_DESCRIPCION_OTRO_ING2 = datos.SDF_DESCRIPCION_OTRO_ING2,
        //                    SDF_DESCRIPCION_OTRO_ING3 = datos.SDF_DESCRIPCION_OTRO_ING3,
        //                    SDF_DESCRIPCION_OTRO_ING4 = datos.SDF_DESCRIPCION_OTRO_ING4,
        //                    SDF_VALOR_TOTAL_INGRESOS = datos.SDF_VALOR_TOTAL_INGRESOS,
        //                    SDF_VALOR_TOTAL_EGRESOS = datos.SDF_VALOR_TOTAL_EGRESOS,
        //                    SDF_USUARIO_ACTUALIZA = Guid.NewGuid(),
        //                    SDF_FECHA_ACTUALIZA = DateTime.Now
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SDF_SIMULACION_DATOS_FINANCIEROS>("CrearSimulacionFinan", objeto, CommandType.StoredProcedure, ref Error);
        //                return retorno.FirstOrDefault().SLS_ID;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando Datos Financieros de la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        
        //        
        //        public Guid CrearSolicitudCredito(SOC_SOLICITUD_CREDITO solicitud, SOP_SOLICITUD_PRODUCTO producto, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SOC_ID = solicitud.SOC_ID,
        //                    SOC_FECHA_SOLICITUD = solicitud.SOC_FECHA_SOLICITUD,
        //                    SOC_NUMERO_SOLICITUD = 0,
        //                    TCR_ID = solicitud.TCR_ID,
        //                    DPD_ID = solicitud.DPD_ID,
        //                    DPC_ID = solicitud.DPC_ID,
        //                    SLS_ID = solicitud.SLS_ID,
        //                    TID_ID = solicitud.TID_ID,
        //                    CLI_IDENTIFICACION = solicitud.CLI_IDENTIFICACION,
        //                    SOC_DECLARACION_ORIGEN_FONDOS = solicitud.SOC_DECLARACION_ORIGEN_FONDOS,
        //                    SOC_AUTORIZA_USO_DATOS = solicitud.SOC_AUTORIZA_USO_DATOS,
        //                    SOC_AUTORIZA_CONSULTA_CENTRALES = solicitud.SOC_AUTORIZA_CONSULTA_CENTRALES,
        //                    SOC_DECLARACION_ASEGURABILIDAD = solicitud.SOC_DECLARACION_ASEGURABILIDAD,
        //                    SOC_ESTADO = solicitud.SOC_ESTADO,
        //                    SOC_CREADO_POR = solicitud.SOC_CREADO_POR,
        //                    SOC_FECHA_CREACION = solicitud.SOC_FECHA_CREACION,
        //                    SOP_ID = producto.SOP_ID,
        //                    SOP_ESTADO_INMUEBLE = producto.SOP_ESTADO_INMUEBLE,
        //                    SOP_TIPO_INMUEBLE = producto.SOP_TIPO_INMUEBLE,
        //                    SOP_TIENE_GARAJE = producto.SOP_TIENE_GARAJE,
        //                    SOP_TIENE_DEPOSITO = producto.SOP_TIENE_DEPOSITO,
        //                    SOP_CONJUNTO_CERRADO = producto.SOP_CONJUNTO_CERRADO,
        //                    TVV_ID = producto.TVV_ID,
        //                    SOP_AÑOS_CONSTRUCCION = producto.SOP_AÑOS_CONSTRUCCION,
        //                    SOP_MATRICULA_INMOBILIARIA1 = producto.SOP_MATRICULA_INMOBILIARIA1,
        //                    SOP_MATRICULA_INMOBILIARIA2 = producto.SOP_MATRICULA_INMOBILIARIA2,
        //                    SOP_MATRICULA_INMOBILIARIA3 = producto.SOP_MATRICULA_INMOBILIARIA3,
        //                    SOP_VALOR_INMUEBLE = producto.SOP_VALOR_INMUEBLE,
        //                    SOP_DIRECCION_INMUEBLE = producto.SOP_DIRECCION_INMUEBLE,
        //                    DPD_ID_INMUEBLE = producto.DPD_ID,
        //                    DPC_ID_INMUEBLE = producto.DPC_ID,
        //                    SOP_VALOR_CREDITO = producto.SOP_VALOR_CREDITO,
        //                    SOP_PORC_FINANCIACION = producto.SOP_PORC_FINANCIACION,
        //                    SOP_PLAZO_FINANCIACION = producto.SOP_PLAZO_FINANCIACION,
        //                    TID_ID_CONS = producto.TID_ID,
        //                    SOP_NUMERO_DOCUMENTO_CONSTRUCTOR = producto.SOP_NUMERO_DOCUMENTO_CONSTRUCTOR,
        //                    SOP_NOMBRE_CONSTRUCTOR = producto.SOP_NOMBRE_CONSTRUCTOR,
        //                    SOP_FECHA_ENTREGA_INMUEBLE = producto.SOP_FECHA_ENTREGA_INMUEBLE,
        //                    SOP_NOMBRE_FIDUCIA = producto.SOP_NOMBRE_FIDUCIA,
        //                    SOP_NOMBRE_PROYECTO = producto.SOP_NOMBRE_PROYECTO
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SOC_SOLICITUD_CREDITO>("CrearSolicitudCredito", objeto, CommandType.StoredProcedure, ref Error);
        //                return retorno.FirstOrDefault().SOC_ID;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando Datos de la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        public Guid CrearSolicCreditoBasicos(SOB_SOLICITUD_INF_BASICA basica, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SOB_ID = basica.SOB_ID,
        //                    SOC_ID = basica.SOC_ID,
        //                    TID_ID = basica.TID_ID,
        //                    SOB_NUMERO_DOCUMENTO = basica.SOB_NUMERO_DOCUMENTO,
        //                    SOB_FECHA_EXPEDICION = basica.SOB_FECHA_EXPEDICION,
        //                    DPD_ID_EXP = basica.DPD_ID_EXP,
        //                    DPC_ID_EXP = basica.DPC_ID_EXP,
        //                    SOB_PRIMER_APELLIDO = basica.SOB_PRIMER_APELLIDO,
        //                    SOB_SEGUNDO_APELLIDO = basica.SOB_SEGUNDO_APELLIDO,
        //                    SOB_PRIMER_NOMBRE = basica.SOB_PRIMER_NOMBRE,
        //                    SOB_SEGUNDO_NOMBRE = basica.SOB_SEGUNDO_NOMBRE,
        //                    SOB_FECHA_NACIMIENTO = basica.SOB_FECHA_NACIMIENTO,
        //                    DPD_ID_NACIMIENTO = basica.DPD_ID_NACIMIENTO,
        //                    DPC_ID_NACIMIENTO = basica.DPC_ID_NACIMIENTO,
        //                    SOB_SEXO = basica.SOB_SEXO,
        //                    SOB_ESTADO_CIVIL = basica.SOB_ESTADO_CIVIL,
        //                    SOB_NIVEL_EDUCACION = basica.SOB_NIVEL_EDUCACION,
        //                    FRZ_ID = basica.FRZ_ID,
        //                    CTG_ID = basica.CTG_ID,
        //                    GRD_ID = basica.GRD_ID,
        //                    DPD_ID_RESIDENCIA = basica.DPD_ID_RESIDENCIA,
        //                    DPC_ID_RESIDENCIA = basica.DPC_ID_RESIDENCIA,
        //                    SOB_DIRECCION_RESIDENCIA = basica.SOB_DIRECCION_RESIDENCIA,
        //                    SOB_CORREO_PERSONAL = basica.SOB_CORREO_PERSONAL,
        //                    SOB_TELEFONO_RESIDENCIA = basica.SOB_TELEFONO_RESIDENCIA,
        //                    SOB_CELULAR = basica.SOB_CELULAR,
        //                    DPD_ID_OFICINA = basica.DPD_ID_OFICINA,
        //                    DPC_ID_OFICINA = basica.DPC_ID_OFICINA,
        //                    SOB_DIRECCION_OFICINA = basica.SOB_DIRECCION_OFICINA,
        //                    SOB_CORREO_INSTITUCIONAL = basica.SOB_CORREO_INSTITUCIONAL,
        //                    SOB_TELEFONO_OFICINA = basica.SOB_TELEFONO_OFICINA,
        //                    ACC_ID = basica.ACC_ID,
        //                    PRF_ID = basica.PRF_ID,
        //                    SOB_CREADO_POR = basica.SOB_CREADO_POR,
        //                    SOB_FECHA_CREACION = basica.SOB_FECHA_CREACION
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SOB_SOLICITUD_INF_BASICA>("CrearSolicCreditoBasicos", objeto, CommandType.StoredProcedure, ref Error);
        //                return retorno.FirstOrDefault().SOC_ID;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando Datos de la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        public Guid CrearSolicCreditoInformacion(SOY_SOLICITUD_INF_CONYUGUE conyugue, SOP_SOLICITUD_PRODUCTO producto, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SOY_ID = conyugue.SOY_ID,
        //                    SOC_ID = conyugue.SOC_ID,
        //                    TID_ID = conyugue.TID_ID,
        //                    SOY_NUMERO_DOCUMENTO = conyugue.SOY_NUMERO_DOCUMENTO,
        //                    SOY_FECHA_EXPEDICION = conyugue.SOY_FECHA_EXPEDICION,
        //                    DPD_ID_EXP = conyugue.DPD_ID_EXP,
        //                    DPC_ID_EXP = conyugue.DPC_ID_EXP,
        //                    SOY_PRIMER_APELLIDO = conyugue.SOY_PRIMER_APELLIDO,
        //                    SOY_SEGUNDO_APELLIDO = conyugue.SOY_SEGUNDO_APELLIDO,
        //                    SOY_PRIMER_NOMBRE = conyugue.SOY_PRIMER_NOMBRE,
        //                    SOY_SEGUNDO_NOMBRE = conyugue.SOY_SEGUNDO_NOMBRE,
        //                    SOY_FECHA_NACIMIENTO = conyugue.SOY_FECHA_NACIMIENTO,
        //                    DPD_ID_NACIMIENTO = conyugue.DPD_ID_NACIMIENTO,
        //                    DPC_ID_NACIMIENTO = conyugue.DPC_ID_NACIMIENTO,
        //                    SOY_CELULAR = conyugue.SOY_CELULAR,
        //                    SOY_CORREO_PERSONAL = conyugue.SOY_CORREO_PERSONAL,
        //                    SOY_ACTIVIDAD_LABORAL = conyugue.SOY_ACTIVIDAD_LABORAL,
        //                    SOY_OTRO_ACTIVIDAD = conyugue.SOY_OTRO_ACTIVIDAD,
        //                    SOY_EMPRESA_LABORA = conyugue.SOY_EMPRESA_LABORA,
        //                    SOY_CREADO_POR = conyugue.SOY_CREADO_POR,
        //                    SOY_FECHA_CREACION = conyugue.SOY_FECHA_CREACION
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SOY_SOLICITUD_INF_CONYUGUE>("CrearSolicCreditoConyugue", objeto, CommandType.StoredProcedure, ref Error);
        //                if (Error == "")
        //                {
        //                    if (producto.TID_ID_VENDEDOR != 0)
        //                    {
        //                        var objeto1 = new
        //                        {
        //                            SOP_ID = producto.SOP_ID,
        //                            SOC_ID = producto.SOC_ID,
        //                            TID_ID_VENDEDOR = producto.TID_ID_VENDEDOR,
        //                            SOP_NUMERO_DOCUMENTO_VENDEDOR = producto.SOP_NUMERO_DOCUMENTO_VENDEDOR,
        //                            SOP_NOMBRE_VENDEDOR = producto.SOP_NOMBRE_VENDEDOR,
        //                            SOP_DIRECCION_VENDEDOR = producto.SOP_DIRECCION_VENDEDOR,
        //                            DPD_ID_VENDEDOR = producto.DPD_ID_VENDEDOR,
        //                            DPC_ID_VENDEDOR = producto.DPC_ID_VENDEDOR,
        //                            SOP_CORREO_VENDEDOR = producto.SOP_CORREO_VENDEDOR,
        //                            SOP_TELEFONO_VENDEDOR = producto.SOP_TELEFONO_VENDEDOR,
        //                            SOP_ACTUALIZADO_POR = producto.SOP_ACTUALIZADO_POR,
        //                            SOP_FECHA_ACTUALIZA = producto.SOP_FECHA_ACTUALIZA
        //                        };
        //                        var retorno1 = actualiza.EjecutarQuery<SOP_SOLICITUD_PRODUCTO>("ActualizaSolicCreditoProducto", objeto1, CommandType.StoredProcedure, ref Error);
        //                    }
        //                }
        //                return retorno.FirstOrDefault().SOC_ID;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando Datos de la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        public Guid CrearSolicCreditoFinanciera(SOF_SOLICITUD_INF_FINANCIERA financiero, SOB_SOLICITUD_INF_BASICA basica, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SOF_ID = financiero.SOF_ID,
        //                    SOC_ID = financiero.SOC_ID,
        //                    SOF_CONCEPTO_SALARIO = financiero.SOF_CONCEPTO_SALARIO,
        //                    SOF_VALOR_CONCEPTO_SALARIO = financiero.SOF_VALOR_CONCEPTO_SALARIO,
        //                    SOF_CONCEPTO_1 = financiero.SOF_CONCEPTO_1,
        //                    SOF_VALOR_CONCEPTO_1 = financiero.SOF_VALOR_CONCEPTO_1,
        //                    SOF_CONCEPTO_2 = financiero.SOF_CONCEPTO_2,
        //                    SOF_VALOR_CONCEPTO_2 = financiero.SOF_VALOR_CONCEPTO_2,
        //                    SOF_CONCEPTO_3 = financiero.SOF_CONCEPTO_3,
        //                    SOF_VALOR_CONCEPTO_3 = financiero.SOF_VALOR_CONCEPTO_3,
        //                    SOF_CONCEPTO_4 = financiero.SOF_CONCEPTO_4,
        //                    SOF_VALOR_CONCEPTO_4 = financiero.SOF_VALOR_CONCEPTO_4,
        //                    SOF_CONCEPTO_5 = financiero.SOF_CONCEPTO_5,
        //                    SOF_VALOR_CONCEPTO_5 = financiero.SOF_VALOR_CONCEPTO_5,
        //                    SOF_CONCEPTO_6 = financiero.SOF_CONCEPTO_6,
        //                    SOF_VALOR_CONCEPTO_6 = financiero.SOF_VALOR_CONCEPTO_6,
        //                    SOF_VALOR_TOTAL_INGRESOS = financiero.SOF_VALOR_TOTAL_INGRESOS,
        //                    SOF_VALOR_TOTAL_EGRESOS = financiero.SOF_VALOR_TOTAL_EGRESOS,
        //                    SOF_CREADO_POR = financiero.SOF_CREADO_POR,
        //                    SOF_FECHA_CREACION = financiero.SOF_FECHA_CREACION
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SOF_SOLICITUD_INF_FINANCIERA>("CrearSolicCreditoFinancieros", objeto, CommandType.StoredProcedure, ref Error);
        //                if (Error == "")
        //                {
        //                    var objeto1 = new
        //                    {
        //                        SOB_ID = basica.SOB_ID,
        //                        SOC_ID = basica.SOC_ID,
        //                        SOB_TIENE_TRANSACCION_ME = basica.SOB_TIENE_TRANSACCION_ME,
        //                        SOB_TIPO_TRANSACCION_ME = basica.SOB_TIPO_TRANSACCION_ME,
        //                        SOB_TIPO_PRODUCTO_ME = basica.SOB_TIPO_PRODUCTO_ME,
        //                        SOB_BANCO_TRANSACCION_ME = basica.SOB_BANCO_TRANSACCION_ME,
        //                        SOB_NUMERO_PRODUCTO_ME = basica.SOB_NUMERO_PRODUCTO_ME,
        //                        SOB_SALDO_PROMEDIO_CUENTAS_ME = basica.SOB_SALDO_PROMEDIO_CUENTAS_ME,
        //                        DPP_ID_ME = basica.DPP_ID_ME,
        //                        DPD_ID_ME = basica.DPD_ID_ME,
        //                        SOB_ACTUALIZADO_POR = basica.SOB_ACTUALIZADO_POR,
        //                        SOB_FECHA_ACTUALIZA = basica.SOB_FECHA_ACTUALIZA
        //                    };
        //                    var retorno1 = actualiza.EjecutarQuery<SOP_SOLICITUD_PRODUCTO>("ActualizaSolicCreditoBasica", objeto1, CommandType.StoredProcedure, ref Error);
        //                }
        //                return retorno.FirstOrDefault().SOC_ID;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando Datos de la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        public Guid CrearSolicCreditoReferencias(SOF_SOLICITUD_INF_FINANCIERA financiero, List<SOR_SOLICITUD_INF_REFERENCIAS> referencias, ref string Error)
        //        {
        //            try
        //            {
        //                Actualizacion actualiza = new Actualizacion();
        //                Guid idRetorno = financiero.SOC_ID;
        //                if (financiero.SOF_DIRECCION_VIVIENDA != null || financiero.SOF_MARCA_VEHICULO != null)
        //                {
        //                    var objeto = new
        //                    {
        //                        SOF_ID = financiero.SOF_ID,
        //                        SOC_ID = financiero.SOC_ID,
        //                        SOF_DIRECCION_VIVIENDA = financiero.SOF_DIRECCION_VIVIENDA,
        //                        SOF_MATRICULA_VIVIENDA = financiero.SOF_MATRICULA_VIVIENDA,
        //                        SOF_VALOR_COMERCIAL_VIVIENDA = financiero.SOF_VALOR_COMERCIAL_VIVIENDA,
        //                        SOF_MARCA_VEHICULO = financiero.SOF_MARCA_VEHICULO,
        //                        SOF_PLACA_VEHICULO = financiero.SOF_PLACA_VEHICULO,
        //                        SOF_VALOR_COMERCIAL_VEHICULO = financiero.SOF_VALOR_COMERCIAL_VEHICULO,
        //                        SOF_ACTUALIZADO_POR = financiero.SOF_ACTUALIZADO_POR,
        //                        SOF_FECHA_ACTUALIZA = financiero.SOF_FECHA_ACTUALIZA
        //                    };
        //                    var retorno = actualiza.EjecutarQuery<SOF_SOLICITUD_INF_FINANCIERA>("ActualizaSolicCreditoFinancieros", objeto, CommandType.StoredProcedure, ref Error);
        //                    idRetorno = retorno.FirstOrDefault().SOC_ID;
        //                }
        //                if (Error == "")
        //                {
        //                    foreach (SOR_SOLICITUD_INF_REFERENCIAS refer in referencias)
        //                    {
        //                        var objeto1 = new
        //                        {
        //                            SOR_ID = refer.SOR_ID,
        //                            SOC_ID = refer.SOC_ID,
        //                            SOR_TIPO_REFERENCIA = refer.SOR_TIPO_REFERENCIA,
        //                            SOR_PRIMER_APELLIDO = refer.SOR_PRIMER_APELLIDO,
        //                            SOR_SEGUNDO_APELLIDO = refer.SOR_SEGUNDO_APELLIDO,
        //                            SOR_NOMBRES = refer.SOR_NOMBRES,
        //                            SOR_PARENTESCO = refer.SOR_PARENTESCO,
        //                            SOR_RELACION = refer.SOR_RELACION,
        //                            SOR_DIRECCION = refer.SOR_DIRECCION,
        //                            DPD_ID = refer.DPD_ID,
        //                            DPC_ID = refer.DPC_ID,
        //                            SOR_TELEFONO = refer.SOR_TELEFONO,
        //                            SOR_CELULAR = refer.SOR_CELULAR,
        //                            SOR_CREADO_POR = refer.SOR_CREADO_POR,
        //                            SOR_FECHA_CREACION = refer.SOR_FECHA_CREACION
        //                        };
        //                        var retorno1 = actualiza.EjecutarQuery<SOP_SOLICITUD_PRODUCTO>("CrearSolicCreditoRefererncias", objeto1, CommandType.StoredProcedure, ref Error);
        //                    }
        //                }
        //                return idRetorno;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Creando Datos de la Solicitud. " + ex.Message.ToString();
        //                return Guid.NewGuid();
        //            }
        //        }
        //        public string CrearEnvioAlerta(ENA_ENVIO_ALERTAS envioAlerta)
        //        {
        //            try
        //            {
        //                string Error = "";
        //                /// Obtengo el Texto de la Alerta
        //                GestorConsultas consulta = new GestorConsultas();
        //                var alerta = consulta.ObtenerAlertaAutomatica(ref Error);
        //                if (Error == "")
        //                {
        //                    var objeto = new
        //                    {
        //                        ENA_ID = envioAlerta.ENA_ID,
        //                        ENA_DESTINATARIO = envioAlerta.ENA_DESTINATARIO,
        //                        ENA_ASUNTO = envioAlerta.ENA_ASUNTO,
        //                        ENA_TEXTO = alerta.FirstOrDefault().ALA_MENSAJE.Replace("&1&", envioAlerta.NOMBRE_CLIENTE),
        //                        ENA_RUTA_ADJUNTO = envioAlerta.ENA_RUTA_ADJUNTO,
        //                        ENA_ENVIADO = envioAlerta.ENA_ENVIADO,
        //                        ENA_CREADO_POR = envioAlerta.ENA_CREADO_POR,
        //                        ENA_FECHA_CREACION = envioAlerta.ENA_FECHA_CREACION,
        //                    };
        //                    Actualizacion actualiza = new Actualizacion();
        //                    var retorno = actualiza.EjecutarQuery<ENA_ENVIO_ALERTAS>("CrearEnvioAlerta", objeto, CommandType.StoredProcedure, ref Error);
        //                    if (Error == "")
        //                        return "";
        //                    else
        //                        return Error;
        //                }
        //                else
        //                    return Error;
        //            }
        //            catch (Exception ex)
        //            {
        //                return "Error actualizando Envio Alerta. " + ex.Message.ToString();
        //            }
        //        }
        //        public List<SLS_SOLICITUD_SIMULACION> GenerarPreaprobado(SLS_SOLICITUD_SIMULACION simula, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SLS_ID = simula.SLS_ID,
        //                    TCR_ID = simula.TCR_ID,
        //                    SLS_USUARIO_ACTUALIZA = simula.SLS_USUARIO_ACTUALIZA,
        //                    SLS_FECHA_ACTUALIZA = simula.SLS_FECHA_ACTUALIZA
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SLS_SOLICITUD_SIMULACION>("CrearPreAprobado", objeto, CommandType.StoredProcedure, ref Error);
        //                return retorno;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Generando Preaprobado la Simulacion. " + ex.Message.ToString();
        //                return null;
        //            }
        //        }
        //        public List<SOC_SOLICITUD_CREDITO> GenerarSolicitudCredito(SOC_SOLICITUD_CREDITO solic, List<string> lFiles, ref string Error)
        //        {
        //            try
        //            {
        //                var objeto = new
        //                {
        //                    SOC_ID = solic.SOC_ID,
        //                    SOC_DECLARACION_ORIGEN_FONDOS = solic.SOC_DECLARACION_ORIGEN_FONDOS,
        //                    SOC_AUTORIZA_USO_DATOS = solic.SOC_AUTORIZA_USO_DATOS,
        //                    SOC_AUTORIZA_CONSULTA_CENTRALES = solic.SOC_AUTORIZA_CONSULTA_CENTRALES,
        //                    SOC_DECLARACION_ASEGURABILIDAD = solic.SOC_DECLARACION_ASEGURABILIDAD,
        //                    SOC_CONVENIO_ASEGURADORA = solic.SOC_CONVENIO_ASEGURADORA,
        //                    ASE_ID = solic.ASE_ID,
        //                    SOC_ESTADO = solic.SOC_ESTADO,
        //                    SOC_FECHA_ACTUALIZA = solic.SOC_FECHA_ACTUALIZA,
        //                    SOC_ACTUALIZADO_POR = solic.SOC_ACTUALIZADO_POR
        //                };
        //                Actualizacion actualiza = new Actualizacion();
        //                var retorno = actualiza.EjecutarQuery<SOC_SOLICITUD_CREDITO>("CrearNoSolicitudCredito", objeto, CommandType.StoredProcedure, ref Error);
        //                if (Error == "")
        //                {
        //                    foreach (string doc in lFiles)
        //                    {
        //                        var dat = doc.Split('_');
        //                        var tipo = dat[2].Split('.');
        //                        var objeto1 = new
        //                        {
        //                            DCS_ID = Guid.NewGuid(),
        //                            SOC_ID = solic.SOC_ID,
        //                            TDC_ID = tipo[0].Trim(),
        //                            DCS_CODIGO_BARRAS = "0",
        //                            DCS_FECHA_DOCUMENTO = DateTime.Now.Date,
        //                            DCS_NUMERO_FOLIOS = 0,
        //                            DCS_TAMAÑO = 0,
        //                            DCS_RUTA_IMAGEN = doc,
        //                            DCS_ESTADO = 1,
        //                            DCS_CREADO_POR = Guid.NewGuid(),
        //                            DCS_FECHA_CREACION = DateTime.Now
        //                        };
        //                        var retorno1 = actualiza.EjecutarQuery<SOP_SOLICITUD_PRODUCTO>("CrearDocumentoSolicCredito", objeto1, CommandType.StoredProcedure, ref Error);
        //                    }
        //                }
        //                return retorno;
        //            }
        //            catch (Exception ex)
        //            {
        //                Error = "Error Generando Numero Solicitud Credito. " + ex.Message.ToString();
        //                return null;
        //            }
        //        }
    }
}
