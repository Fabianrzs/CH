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

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor
{
    /// <summary>
    /// Consumo Otp service
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>22/11/2021</date>
    public class ConsumoWSProveedor : IConsumoWSProveedor
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
        private readonly ILogger<ConsumoWSProveedor> _logger;

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
        public ConsumoWSProveedor(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoWSProveedor> logger,
            IMapper mapper,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWSProveedor);
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
        private SIEWSSECSoapClient ObtenerInstanciaCliente() => new SIEWSSECSoapClient(basicHttpBinding, endpointAddress);

        /// <summary>
        /// Metodo de instancia ChangeNitRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<ChangeNitResponse> InsertarCnMcontAsync(ChangeNitRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());

            
            var respuesta = await _auditoria.EjecutarServicio<ChangeNitRequest, ChangeNitResponse>(
            _wssec.ChangeNitAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SyncFaContaRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SyncFaContaResponse> SyncFaContaAsync(SyncFaContaRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<SyncFaContaRequest, SyncFaContaResponse>(
           _wssec.SyncFaContaAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia ActModalidadCupoRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<ActModalidadCupoResponse> ActModalidadCupoAsync(ActModalidadCupoRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<ActModalidadCupoRequest, ActModalidadCupoResponse>(
           _wssec.ActModalidadCupoAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SyncEmpleadoPvdorRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SyncEmpleadoPvdorResponse> SyncEmpleadoPvdorAsync(SyncEmpleadoPvdorRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<SyncEmpleadoPvdorRequest, SyncEmpleadoPvdorResponse>(
            _wssec.SyncEmpleadoPvdorAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SyncEmpleadoPvdorRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SyncFaClienResponse> SyncFaClienAsync(SyncFaClienRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<SyncFaClienRequest, SyncFaClienResponse>(
            _wssec.SyncFaClienAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SyncFaClienComplexRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SyncFaClienComplexResponse> SyncFaClienComplexAsync(SyncFaClienComplexRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<SyncFaClienComplexRequest, SyncFaClienComplexResponse>(
            _wssec.SyncFaClienComplexAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SyncFaContaRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SyncPoPvdorResponse> SyncPoPvdorAsync(SyncPoPvdorRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<SyncPoPvdorRequest, SyncPoPvdorResponse>(
            _wssec.SyncPoPvdorAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SyncGnTerceRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SyncGnTerceResponse> SyncGnTerceAsync(SyncGnTerceRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _wssec = this.ObtenerInstanciaCliente();
            await _wssec.OpenAsync();
            _logger.LogInformation(_wssec.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<SyncGnTerceRequest, SyncGnTerceResponse>(
            _wssec.SyncGnTerceAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }
    }
}
