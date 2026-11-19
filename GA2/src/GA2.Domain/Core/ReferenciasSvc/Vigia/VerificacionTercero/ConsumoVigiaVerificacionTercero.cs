using GA2.Application.Dto;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero
{
    /// <summary>
    /// Consumidor de vigia verficicion tercero
    /// </summary>
    public class ConsumoVigiaVerificacionTercero : IConsumoVigiaVerificacionTercero
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
        /// <summary>
        /// Cliente de conexion a verficacion
        /// </summary>
        private readonly SOAP_VerificaTerceroSoapClient _verificacionTercero;
        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoVigiaVerificacionTercero> _logger;

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoVigiaVerificacionTercero(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoVigiaVerificacionTercero> logger)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.VigiaVerificarTerceroUrlSvc);
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

            /// creacion de instancia consumo
            _verificacionTercero = this.ObtenerInstanciaCliente();
        }

        /// <summary>
        /// Obtener la instancia de conexion Verificacion tercero
        /// </summary>
        /// <returns>Instancia de servicio verificacion</returns>
        private SOAP_VerificaTerceroSoapClient ObtenerInstanciaCliente() => new SOAP_VerificaTerceroSoapClient(basicHttpBinding, endpointAddress);

        public async Task<Resultado> VertificacionTerceros(VerificacionTerceroRequest verificacionTercero)
        {
            _logger.LogInformation(_verificacionTercero.State.ToString());
            await this._verificacionTercero.OpenAsync();
            _logger.LogInformation(_verificacionTercero.State.ToString());
            var verify= await _verificacionTercero.F_VERIFICATERCEROV3Async(verificacionTercero.Pe_Origen,
                                                                       verificacionTercero.Pe_Dato,
                                                                       verificacionTercero.Pe_TipoVerificacion,
                                                                       verificacionTercero.Pe_Porcentaje);
            await Task.CompletedTask;
            await this._verificacionTercero.CloseAsync();
            _logger.LogInformation(_verificacionTercero.State.ToString());
            return verify;
        }

        public async Task<Resultado> VertificacionTerceroSolicitud(VerificacionTerceroRequest verificacionTercero)
        {
            _logger.LogInformation(_verificacionTercero.State.ToString());
            var verify = await _verificacionTercero.F_VERIFICATERCEROV3Async(verificacionTercero.Pe_Origen,
                                                                       verificacionTercero.Pe_Dato,
                                                                       verificacionTercero.Pe_TipoVerificacion,
                                                                       verificacionTercero.Pe_Porcentaje);
            await Task.CompletedTask;
            return verify;
        }
    }
}
