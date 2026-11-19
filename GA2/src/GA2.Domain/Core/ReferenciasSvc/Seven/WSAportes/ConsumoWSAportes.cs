using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes
{
    public class ConsumoWSAportes : IConsumoWSAportes
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
        private readonly ILogger<ConsumoWSAportes> _logger;

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
        public ConsumoWSAportes(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoWSAportes> logger,
            IMapper mapper,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWSAportes);
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
        private ValidarAportesSoapClient ObtenerInstanciaCliente() => new ValidarAportesSoapClient(basicHttpBinding, endpointAddress);

        /// <summary>
        /// Metodo de instancia GetAporteAsync
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<GetAporteResponse> GetAporteAsync(GetAporteRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _waportes = this.ObtenerInstanciaCliente();
            await _waportes.OpenAsync();
            _logger.LogInformation(_waportes.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<GetAporteRequest, GetAporteResponse>(
            _waportes.GetAporteAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }
    }
}
