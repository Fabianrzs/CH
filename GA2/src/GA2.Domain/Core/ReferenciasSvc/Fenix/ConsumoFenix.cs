using GA2.Domain.Core.BuaServiceSvc;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using static GA2.Application.Dto.Credito.TU.Responses.ResponseGeneralTu;

namespace GA2.Domain.Core.ReferenciasSvc.Fenix
{
    public class ConsumoFenix : IConsumoFenix
    {
        /// <summary>
        /// Instancias de configuracion
        /// </summary>
        private readonly ConfiguracionesSvc _configuration;
        /// <summary>
        /// Instancia de endpoint direccion
        /// </summary>
        private readonly EndpointAddress endpointAddress;
        /// <summary>
        /// Tipo de binding realizado al servicio
        /// </summary>
        public readonly BasicHttpBinding basicHttpBinding;
        private readonly WebServicesBiometriasSoapClient _fenix;

        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoFenix> _logger;

        private readonly IAuditoriaServicesBusinessLogic _auditoria;
        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoFenix(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoFenix> logger,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.FenixServiceSvc);
            basicHttpBinding = new BasicHttpBinding(endpointAddress.Uri.Scheme.ToLower() == "http" ?
            BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport)
            {
                OpenTimeout = TimeSpan.MaxValue,
                CloseTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = 1_000_000
            };

            _logger = logger;
            _fenix = ObtenerInstanciaCliente();
            _auditoria = auditoria;
        }

        /// <summary>
        /// Obtener la instancia de conexion workmanager
        /// </summary>
        /// <returns>Instancia de servicio workmanager</returns>
        private WebServicesBiometriasSoapClient ObtenerInstanciaCliente() =>
            new WebServicesBiometriasSoapClient(basicHttpBinding, endpointAddress);

        public async Task<ValidarBiometriaResponse> ValidarBiometriaAsync(ValidarBiometriaRequest identification)
        {
            _logger.LogInformation(_fenix.State.ToString());
            await _fenix.OpenAsync();
            _logger.LogInformation(_fenix.State.ToString());
            var validation= await _auditoria.EjecutarServicio<ValidarBiometriaRequest, ValidarBiometriaResponse>(
                 _fenix.ValidarBiometriaAsync, identification, ReferenciaSvc.Fenix);
            await Task.CompletedTask;
            await _fenix.CloseAsync();
            _logger.LogInformation(_fenix.State.ToString());
            return validation;
        }
    }
}
