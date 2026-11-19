using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceSeven;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GA2.Domain.Core.Connected_Services.ServiceSeven
{
    /// <summary>
    /// Servicio para consumir el API de Movimientos Contables SEVEN ERP
    /// Compatible con ConsumoSCnMcont existente
    /// </summary>
    public class MovimientoContableService : IMovimientoContableService
    {
        private readonly ConfiguracionesSvc _configuration;
        private readonly ILogger<MovimientoContableService> _logger;

        /// <summary>
        /// Constructor del servicio Movimiento Contable
        /// </summary>
        /// <param name="configuration">Configuraciones del servicio</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public MovimientoContableService(
            IOptions<ConfiguracionesSvc> configuration,
            ILogger<MovimientoContableService> logger)
        {
            _logger = logger;
            _configuration = configuration.Value;

            if (string.IsNullOrWhiteSpace(_configuration?.SevenWSMovContable))
            {
                _logger.LogError("La URL del servicio SEVEN MovContable no está configurada en appsettings.json");
                throw new InvalidOperationException("SevenWSMovContable no está configurado en ConfiguracionesSvc");
            }

            _logger.LogInformation($"Servicio SEVEN MovContable inicializado. URL: {_configuration.SevenWSMovContable}");
        }

        /// <summary>
        /// Crea una nueva instancia del cliente SEVEN ERP con binding fresco
        /// </summary>
        /// <returns>Nueva instancia del cliente</returns>
        private SCNMCONTSoapClient ObtenerInstanciaCliente()
        {
            try
            {
                // Crear binding y endpoint FRESCOS en cada llamada
                var endpointAddress = new EndpointAddress(_configuration.SevenWSMovContable);

                var securityMode = endpointAddress.Uri.Scheme.ToLower() == "http"
                    ? BasicHttpSecurityMode.None
                    : BasicHttpSecurityMode.Transport;

                var basicHttpBinding = new BasicHttpBinding(securityMode)
                {
                    OpenTimeout = TimeSpan.FromSeconds(60),
                    CloseTimeout = TimeSpan.FromSeconds(60),
                    ReceiveTimeout = TimeSpan.FromSeconds(60),
                    SendTimeout = TimeSpan.FromSeconds(60),
                    MaxReceivedMessageSize = 1_000_000,
                    MaxBufferSize = 1_000_000
                };

                var client = new SCNMCONTSoapClient(basicHttpBinding, endpointAddress);

                if (client.State == CommunicationState.Faulted)
                {
                    _logger.LogError($"El cliente WCF está en estado Faulted. Endpoint: {_configuration.SevenWSMovContable}");
                    throw new InvalidOperationException("No se pudo crear el cliente WCF en un estado válido");
                }

                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear instancia del cliente SEVEN ERP. URL: {_configuration.SevenWSMovContable}");
                throw;
            }
        }
        /// <summary>
        /// Inserta un movimiento contable completo usando TSCnMcont
        /// </summary>
        public async Task<Response<string>> InsertarMovContableAsync(TSCnMcont dataMovContable, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        ReturnMessage = "Operación cancelada"
                    };
                }

                //  Validar movimiento antes de enviar
                var erroresValidacion = MovimientoContableValidator.ValidarMovimiento(dataMovContable);
                if (erroresValidacion.Count > 0)
                {
                    var mensajeErrores = string.Join(" | ", erroresValidacion);
                    _logger.LogError($"Validación fallida: {mensajeErrores}");

                    return new Response<string>
                    {
                        IsSuccess = false,
                        ReturnMessage = $"[VALIDATION_ERROR] {mensajeErrores}"
                    };
                }
                //  CONVERTIR cuentas LOCAL a NIF antes de enviar a SEVEN
                dataMovContable = MovimientoContableValidator.ConvertirCuentasLocalANif(dataMovContable);
                var match = System.Text.RegularExpressions.Regex.Match(dataMovContable.Mco_desc, @"\d{1,2}[/\-]\d{1,2}[/\-]\d{2,4}\s*$");
                string posibleFecha = match.Success ? match.Value.Trim() : string.Empty;

                string[] parts = dataMovContable.Mco_desc.Split(' ');

                DateTime fecha;
                if (!DateTime.TryParseExact(
                        posibleFecha,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out fecha))
                {
                    fecha = DateTime.Today;
                }

                _logger.LogInformation($"Fecha final: {fecha:dd-MM-yyyy}");

                _logger.LogInformation($"Cuentas convertidas de LOCAL a NIF para SEVEN ERP");

                // Usar using para gestión automática del cliente
                using var client = ObtenerInstanciaCliente();
                await client.OpenAsync();
                _logger.LogInformation($"Estado del cliente: {client.State}");


                    dataMovContable.Mco_fech = fecha;
            
                _logger.LogInformation($"Insertando movimiento contable. Concepto: {dataMovContable.Mco_desc}");

                var resultado = await client.InsertarMovContableAsync(dataMovContable);

                _logger.LogInformation($"Respuesta SEVEN ERP: {resultado}");

                // Parsear la respuesta XML de SEVEN para detectar errores
                return ParsearRespuestaXml(resultado);
            }
            catch (CommunicationObjectFaultedException cofex)
            {
                _logger.LogError(cofex, $"El canal WCF está en estado Faulted");

                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[WCF_FAULTED] El canal de comunicación está en estado Faulted: {cofex.Message}"
                };
            }
            catch (FaultException fex)
            {
                _logger.LogError(fex, $"Error de SOAP Fault al insertar movimiento contable");

                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[SOAP_FAULT] Error en el servicio SEVEN ERP: {fex.Message}"
                };
            }
            catch (EndpointNotFoundException enfex)
            {
                _logger.LogError(enfex, $"No se pudo conectar al endpoint SEVEN ERP: {_configuration.SevenWSMovContable}");

                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[ENDPOINT_NOT_FOUND] No se pudo conectar al servicio SEVEN ERP: {enfex.Message}"
                };
            }
            catch (CommunicationException cex)
            {
                _logger.LogError(cex, $"Error de comunicación al insertar movimiento contable");

                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[COMMUNICATION_ERROR] Error de comunicación con SEVEN ERP: {cex.Message}"
                };
            }
            catch (TimeoutException tex)
            {
                _logger.LogError(tex, $"Timeout al insertar movimiento contable");

                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[TIMEOUT] Tiempo de espera agotado: {tex.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al insertar movimiento contable. Tipo: {ex.GetType().Name}");

                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[UNEXPECTED_ERROR] Error inesperado: {ex.Message}"
                };
            }
        }


        /// <summary>
        /// Aplica un movimiento contable
        /// </summary>
        public async Task<Response<string>> AplicarCnMcontAsync(int empCodi, int mcoCont)
        {
            SCNMCONTSoapClient client = null;

            try
            {
                client = ObtenerInstanciaCliente();

                _logger.LogInformation($"Aplicando movimiento. Empresa: {empCodi}, Consecutivo: {mcoCont}");

                var resultado = await client.AplicarCnMcontAsync(empCodi, mcoCont);

                if (client.State == CommunicationState.Opened)
                {
                    await client.CloseAsync();
                }

                _logger.LogInformation($"Movimiento aplicado exitosamente");

                return new Response<string>(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al aplicar movimiento. Consecutivo: {mcoCont}");
                AbortClient(client);

                return ManejarError<string>(ex);
            }
        }

        /// <summary>
        /// Anula un movimiento contable
        /// </summary>
        public async Task<Response<string>> AnularCnMcontAsync(int empCodi, int mcoCont)
        {
            SCNMCONTSoapClient client = null;

            try
            {
                client = ObtenerInstanciaCliente();

                _logger.LogInformation($"Anulando movimiento. Empresa: {empCodi}, Consecutivo: {mcoCont}");

                var resultado = await client.AnularCnMcontAsync(empCodi, mcoCont);

                if (client.State == CommunicationState.Opened)
                {
                    await client.CloseAsync();
                }

                _logger.LogInformation($"Movimiento anulado exitosamente");

                return new Response<string>(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al anular movimiento. Consecutivo: {mcoCont}");
                AbortClient(client);

                return ManejarError<string>(ex);
            }
        }

        /// <summary>
        /// Revierte un movimiento contable
        /// </summary>
        public async Task<Response<string>> RevertirCnMcontAsync(int empCodi, int mcoCont)
        {
            SCNMCONTSoapClient client = null;

            try
            {
                client = ObtenerInstanciaCliente();

                _logger.LogInformation($"Revirtiendo movimiento. Empresa: {empCodi}, Consecutivo: {mcoCont}");

                var resultado = await client.RevertirCnMcontAsync(empCodi, mcoCont);

                if (client.State == CommunicationState.Opened)
                {
                    await client.CloseAsync();
                }

                _logger.LogInformation($"Movimiento revertido exitosamente");

                return new Response<string>(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al revertir movimiento. Consecutivo: {mcoCont}");
                AbortClient(client);

                return ManejarError<string>(ex);
            }
        }

        /// <summary>
        /// Crea movimientos contables con operación TOPER
        /// </summary>
        public async Task<Response<TRetToper[]>> SetContabToperAsync(TToper[] movimientos)
        {
            SCNMCONTSoapClient client = null;

            try
            {
                client = ObtenerInstanciaCliente();

                _logger.LogInformation($"Insertando {movimientos.Length} movimientos TOPER");

                var resultado = await client.SetContabToperAsync(movimientos);

                if (client.State == CommunicationState.Opened)
                {
                    await client.CloseAsync();
                }

                _logger.LogInformation($"Movimientos TOPER insertados exitosamente");

                return new Response<TRetToper[]>(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar movimientos TOPER");
                AbortClient(client);

                return ManejarErrorArray(ex);
            }
        }

        #region Métodos Privados

        /// <summary>
        /// Cierra o aborta el cliente WCF de forma segura
        /// </summary>
        /// <param name="client">Cliente a cerrar</param>
        private void AbortClient(SCNMCONTSoapClient client)
        {
            if (client == null) return;

            try
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else if (client.State == CommunicationState.Opened)
                {
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al cerrar cliente WCF");
                try
                {
                    client.Abort();
                }
                catch
                {
                    // Ignorar errores al abortar
                }
            }
        }

        /// <summary>
        /// Maneja errores y retorna Response<T> apropiado
        /// </summary>
        private Response<T> ManejarError<T>(Exception ex)
        {
            return ex switch
            {
                CommunicationObjectFaultedException => new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[WCF_FAULTED] El canal de comunicación está en estado Faulted: {ex.Message}"
                },
                FaultException => new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[SOAP_FAULT] Error en el servicio SEVEN ERP: {ex.Message}"
                },
                EndpointNotFoundException => new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[ENDPOINT_NOT_FOUND] No se pudo conectar al servicio SEVEN ERP: {ex.Message}"
                },
                CommunicationException => new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[COMMUNICATION_ERROR] Error de comunicación con SEVEN ERP: {ex.Message}"
                },
                TimeoutException => new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[TIMEOUT] Tiempo de espera agotado: {ex.Message}"
                },
                _ => new Response<T>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[UNEXPECTED_ERROR] Error inesperado: {ex.Message}"
                }
            };
        }

        /// <summary>
        /// Maneja errores para arrays
        /// </summary>
        private Response<TRetToper[]> ManejarErrorArray(Exception ex)
        {
            return ManejarError<TRetToper[]>(ex);
        }
        /// <summary>
        /// Parsea la respuesta XML de SEVEN ERP y detecta errores
        /// Formato esperado:
        /// <NewDataSet>
        ///   <CN_MCONT>
        ///     <RETORNO>0 o 1</RETORNO>
        ///     <TXTERROR>Mensaje de error</TXTERROR>
        ///     <MCO_CONT>Consecutivo</MCO_CONT>
        ///     <MCO_NUME>Numero</MCO_NUME>
        ///   </CN_MCONT>
        /// </NewDataSet>
        /// </summary>
        private Response<string> ParsearRespuestaXml(string xmlRespuesta)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlRespuesta))
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        ReturnMessage = "[SEVEN_ERROR] Respuesta vacía del servicio SEVEN ERP",
                        Data = xmlRespuesta
                    };
                }

                // Parsear XML
                var xdoc = XDocument.Parse(xmlRespuesta);
                var cnMcont = xdoc.Descendants("CN_MCONT").FirstOrDefault();

                if (cnMcont == null)
                {
                    // No tiene el formato esperado, retornar como está
                    _logger.LogWarning("Respuesta XML no tiene formato CN_MCONT esperado");
                    return new Response<string>(xmlRespuesta);
                }

                // Extraer campos
                var retorno = cnMcont.Element("RETORNO")?.Value;
                var txtError = cnMcont.Element("TXTERROR")?.Value;
                var mcoCont = cnMcont.Element("MCO_CONT")?.Value;
                var mcoNume = cnMcont.Element("MCO_NUME")?.Value;

                // RETORNO = 1 indica error en SEVEN
                if (retorno == "1")
                {
                    _logger.LogError($"SEVEN ERP retornó error: {txtError}");

                    return new Response<string>
                    {
                        IsSuccess = false,
                        ReturnMessage = $"[SEVEN_VALIDATION_ERROR] {txtError}",
                        Data = xmlRespuesta
                    };
                }

                // RETORNO = 0 indica éxito
                if (retorno == "0")
                {
                    var mensaje = $"Movimiento creado exitosamente. Consecutivo: {mcoCont}, Número: {mcoNume}";
                    _logger.LogInformation(mensaje);

                    return new Response<string>
                    {
                        IsSuccess = true,
                        ReturnMessage = mensaje,
                        Data = xmlRespuesta
                    };
                }

                // Retorno desconocido
                _logger.LogWarning($"RETORNO desconocido: {retorno}");
                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[SEVEN_UNKNOWN] RETORNO={retorno}, TXTERROR={txtError}",
                    Data = xmlRespuesta
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al parsear respuesta XML de SEVEN: {xmlRespuesta}");

                // Si falla el parsing, retornar el XML crudo
                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"[XML_PARSE_ERROR] Error al parsear respuesta: {ex.Message}",
                    Data = xmlRespuesta
                };
            }
        }

        #endregion
    }
}
