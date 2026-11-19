using Azure;
using GA2.Application.Dto.Credito.TU.Requests;
using GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceVigiaVerificacionTercero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Resultado = ServiceVigiaVerificacionTercero.Resultado;

namespace GA2.Domain.Core.Connected_Services.ServiceVigiaVerificacionTercero
{
    public class VerificacionTerceroVigiaBusinessLogic : IVerificacionTerceroVigia
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
        private readonly ApplicationServicesPortTypeClient _verificacionTercero;
        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoVigiaVerificacionTercero> _logger;

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public VerificacionTerceroVigiaBusinessLogic(
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
        private ApplicationServicesPortTypeClient ObtenerInstanciaCliente() => new ApplicationServicesPortTypeClient(basicHttpBinding, endpointAddress);
       

        public async Task<Transversals.Commons.Response<Resultado>> VertificacionTerceroSolicitud(string param1, string param2, string param3)
        {
            _logger.LogInformation(_verificacionTercero.State.ToString());
            var verify = await _verificacionTercero.F_VERIFICATERCEROAsync(param1,param2,param3);
            await Task.CompletedTask;
            return new Transversals.Commons.Response<Resultado>{
                Data = verify,
            };
        }
    }
}
