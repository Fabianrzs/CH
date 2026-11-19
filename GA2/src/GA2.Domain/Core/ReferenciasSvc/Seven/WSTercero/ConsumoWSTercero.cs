using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Entities;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero
{
    public class ConsumoWSTercero : IConsumoWSTercero
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
        private readonly ILogger<ConsumoWSTercero> _logger;

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
        public ConsumoWSTercero(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoWSTercero> logger,
            IMapper mapper,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWSTercero);
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
        private SGNTERCESoapClient ObtenerInstanciaCliente() => new SGNTERCESoapClient(basicHttpBinding, endpointAddress);


        /// <summary>
        /// Metodo de instancia InsertarGnTerceRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarGnTerceResponse> InsertarGnTerceAsync(InsertarGnTerceRequest request, CancellationToken cancellationToken)
        {
            
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _tercero = this.ObtenerInstanciaCliente();
            await _tercero.OpenAsync();
            _logger.LogInformation(_tercero.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<InsertarGnTerceRequest, InsertarGnTerceResponse>(
             _tercero.InsertarGnTerceAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }
    }
}
