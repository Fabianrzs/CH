using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura
{
    public class ConsumoWSFactura : IConsumoWSFactura
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
        private readonly ILogger<ConsumoWSFactura> _logger;

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
        public ConsumoWSFactura(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoWSFactura> logger,
            IMapper mapper,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWSFactura);
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
        private SPOFACTUSoapClient ObtenerInstanciaCliente() => new SPOFACTUSoapClient(basicHttpBinding, endpointAddress);

        /// <summary>
        /// Metodo de instancia InsertarPoFactuRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarPoFactuResponse> InsertarPoFactuAsync(InsertarPoFactuRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());
            
            var respuesta = await _auditoria.EjecutarServicio<InsertarPoFactuRequest, InsertarPoFactuResponse>(
            _cdp.InsertarPoFactuAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia AnularPoFactuRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<AnularPoFactuResponse> AnularPoFactuAsync(AnularPoFactuRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<AnularPoFactuRequest, AnularPoFactuResponse>(
            _cdp.AnularPoFactuAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia AnularPoFactuRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<AnularFacturaResponse> AnularFacturaAsync(AnularFacturaRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<AnularFacturaRequest, AnularFacturaResponse>(
            _cdp.AnularFacturaAsync, request, ReferenciaSvc.Seven);
            return respuesta;

        }

        /// <summary>
        /// Metodo de instancia InsertarFacturaPRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarFacturaPResponse> InsertarFacturaPAsync(InsertarFacturaPRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<InsertarFacturaPRequest, InsertarFacturaPResponse>(
            _cdp.InsertarFacturaPAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia InsertarPoFactuPRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarPoFactuPResponse> InsertarPoFactuPAsync(InsertarPoFactuPRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _cdp = this.ObtenerInstanciaCliente();
            await _cdp.OpenAsync();
            _logger.LogInformation(_cdp.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<InsertarPoFactuPRequest, InsertarPoFactuPResponse>(
            _cdp.InsertarPoFactuPAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }
    }
}