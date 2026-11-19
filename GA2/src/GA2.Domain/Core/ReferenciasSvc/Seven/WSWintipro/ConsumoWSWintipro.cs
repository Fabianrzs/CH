using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro
{
    public class ConsumoWSWintipro : IConsumoWSWintipro
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
        private readonly ILogger<ConsumoWSWintipro> _logger;

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
        public ConsumoWSWintipro(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoWSWintipro> logger,
            IMapper mapper,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWintipro);
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
        private SINTIPROSoapClient ObtenerInstanciaCliente() => new SINTIPROSoapClient(basicHttpBinding, endpointAddress);


        /// <summary>
        /// Metodo de instancia GetCuentaContableTORequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<GetCuentaContableTOResponse> GetCuentaContableTOAsync(GetCuentaContableTORequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _winpro = this.ObtenerInstanciaCliente();
            await _winpro.OpenAsync();
            _logger.LogInformation(_winpro.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<GetCuentaContableTORequest, GetCuentaContableTOResponse>(
             _winpro.GetCuentaContableTOAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }


        /// <summary>
        /// Metodo de instancia GetCuentaContableRequest 
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<GetCuentaContableResponse> GetCuentaContableAsync(GetCuentaContableRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _winpro = this.ObtenerInstanciaCliente();
            await _winpro.OpenAsync();
            _logger.LogInformation(_winpro.State.ToString());

            var resultado = await _auditoria.EjecutarServicio<GetCuentaContableRequest, GetCuentaContableResponse>(
             _winpro.GetCuentaContableAsync, request, ReferenciaSvc.Seven);
            return resultado;
        }
    }
}
