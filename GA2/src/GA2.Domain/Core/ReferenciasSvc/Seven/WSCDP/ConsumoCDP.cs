using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP
{
    /// <summary>
    /// Consumo Otp service
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>22/11/2021</date>
    public class ConsumoCDP : IConsumoCDP
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
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoCDP> _logger;

        /// <summary>
        /// Instancia Mapper
        /// </summary>
        private readonly IMapper _mapper;

        private readonly IAuditoriaServicesBusinessLogic _auditoria;

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoCDP(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoCDP> logger,
            IMapper mapper,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWSCdp);
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
            _mapper = mapper;
            _auditoria = auditoria;
        }

        /// <summary>
        /// Obtener la instancia de conexion service otp
        /// </summary>
        /// <returns>Instancia de servicio ServiceOtp</returns>
        private SPGOBLIGSoapClient ObtenerInstanciaCliente() => new SPGOBLIGSoapClient(basicHttpBinding, endpointAddress);

        /// <summary>
        /// Metodo de instancia AplicarCDPRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<AplicarCDPResponse> AplicarCDPAsync(AplicarCDPRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<AplicarCDPRequest, AplicarCDPResponse>(
            _cdp.AplicarCDPAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia AnularCDPRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<AnularCDPResponse> AnularCDPAsync(AnularCDPRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<AnularCDPRequest, AnularCDPResponse>(
            _cdp.AnularCDPAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia CrearCDPDto
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<CrearCDPResponse> CrearCDPAsync(CrearCDPRequest request, CancellationToken cancellationToken)
        {

            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<CrearCDPRequest, CrearCDPResponse>(
            _cdp.CrearCDPAsync, request, ReferenciaSvc.Seven);
            return respuesta;

        }

        /// <summary>
        /// Metodo de instancia CrearCDPDto
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarCDPResponse> InsertarCDPAsync(InsertarCDPRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<InsertarCDPRequest, InsertarCDPResponse>(
            _cdp.InsertarCDPAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia CrearCDPDto
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarDetCDPResponse> InsertarDetCDPAsync(InsertarDetCDPRequest request, CancellationToken cancellationToken)
        {
            
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<InsertarDetCDPRequest, InsertarDetCDPResponse>(
           _cdp.InsertarDetCDPAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }
    }
}
