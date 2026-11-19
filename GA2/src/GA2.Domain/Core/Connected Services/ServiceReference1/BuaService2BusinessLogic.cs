using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceReference1;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Connected_Services.ServiceReference1
{
    /// <summary>
    /// Servicio para consumir el API de BUA
    /// </summary>
    public class BuaService2BusinessLogic : IBuaService2BusinessLogic
    {
        private readonly ConfiguracionesSvc _configuration;
        private readonly EndpointAddress endpointAddress;
        private readonly BasicHttpBinding basicHttpBinding;
        private readonly ILogger<BuaService2BusinessLogic> _logger;

        /// <summary>
        /// Constructor del servicio BUA
        /// </summary>
        /// <param name="configurationes">Configuraciones del servicio</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public BuaService2BusinessLogic(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<BuaService2BusinessLogic> logger)
        {
            _logger = logger;
            _configuration = configurationes.Value;

            if (string.IsNullOrWhiteSpace(_configuration?.BuaUrlServiceApi))
            {
                _logger.LogError("La URL del servicio BUA no está configurada en appsettings.json");
                throw new InvalidOperationException("BuaUrlServiceApi no está configurado en ConfiguracionesSvc");
            }

            try
            {
                endpointAddress = new EndpointAddress(_configuration.BuaUrlServiceApi);

                var securityMode = endpointAddress.Uri.Scheme.ToLower() == "http"
                    ? BasicHttpSecurityMode.None
                    : BasicHttpSecurityMode.Transport;

                basicHttpBinding = new BasicHttpBinding(securityMode)
                {
                    OpenTimeout = TimeSpan.FromMinutes(5),
                    CloseTimeout = TimeSpan.FromMinutes(1),
                    ReceiveTimeout = TimeSpan.FromMinutes(5),
                    SendTimeout = TimeSpan.FromMinutes(5),
                    MaxReceivedMessageSize = 10_000_000,
                    MaxBufferSize = 10_000_000
                };

                _logger.LogInformation($"Servicio BUA configurado. Endpoint: {endpointAddress.Uri}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al configurar el servicio BUA. URL: {_configuration.BuaUrlServiceApi}");
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva instancia del cliente BUA
        /// </summary>
        /// <returns>Nueva instancia del cliente</returns>
        private BUA_APIClient ObtenerInstanciaCliente()
        {
            try
            {
                var client = new BUA_APIClient(basicHttpBinding, endpointAddress);

                if (client.State == CommunicationState.Faulted)
                {
                    _logger.LogError($"El cliente WCF está en estado Faulted. Endpoint: {endpointAddress.Uri}");
                    throw new InvalidOperationException("No se pudo crear el cliente WCF en un estado válido");
                }

                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear instancia del cliente BUA");
                throw;
            }
        }

        /// <summary>
        /// Obtiene información de BUA (Base Única de Afiliados)
        /// </summary>
        /// <param name="document">Número de documento</param>
        /// <param name="typeDocumento">Tipo de documento</param>
        /// <returns>Respuesta con datos de BUA</returns>
        public async Task<Transversals.Commons.Response<BUA>> ObtenerBUAAsync(string document, int typeDocumento)
        {
            BUA_APIClient client = null;

            try
            {
                client = ObtenerInstanciaCliente();

                var request = new BUAConsultaBE
                {
                    identificacion = document,
                    tipo_Id = typeDocumento
                };

                _logger.LogInformation($"Consultando BUA para documento: {document}, tipo: {typeDocumento}");

                var result = await client.ObtenerBUAAsync(request);

                if (client.State == CommunicationState.Opened)
                {
                    await client.CloseAsync();
                }

                _logger.LogInformation($"Consulta BUA exitosa para documento: {document}");

                return new Transversals.Commons.Response<BUA>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (FaultException fex)
            {
                _logger.LogError(fex, $"Error de SOAP Fault al consultar BUA para documento: {document}");
                AbortClient(client);

                return new Transversals.Commons.Response<BUA>
                {
                    IsSuccess = false
                };
            }
            catch (EndpointNotFoundException enfex)
            {
                _logger.LogError(enfex, $"No se pudo conectar al endpoint BUA: {_configuration.BuaUrlServiceApi}");
                AbortClient(client);

                return new Transversals.Commons.Response<BUA>
                {
                    IsSuccess = false
                };
            }
            catch (CommunicationException cex)
            {
                _logger.LogError(cex, $"Error de comunicación al consultar BUA para documento: {document}");
                AbortClient(client);

                return new Transversals.Commons.Response<BUA>
                {
                    IsSuccess = false
                };
            }
            catch (TimeoutException tex)
            {
                _logger.LogError(tex, $"Timeout al consultar BUA para documento: {document}");
                AbortClient(client);

                return new Transversals.Commons.Response<BUA>
                {
                    IsSuccess = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al consultar BUA. Documento: {document}, Tipo: {ex.GetType().Name}");
                AbortClient(client);

                return new Transversals.Commons.Response<BUA>
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Cierra o aborta el cliente WCF de forma segura
        /// </summary>
        /// <param name="client">Cliente a cerrar</param>
        private void AbortClient(BUA_APIClient client)
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

      
    }
}
