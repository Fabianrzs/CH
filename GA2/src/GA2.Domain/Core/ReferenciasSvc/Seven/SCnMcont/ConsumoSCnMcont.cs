using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using static IronOcr.OcrResult;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont
{
    public class ConsumoSCnMcont : IConsumoSCnMcont
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
        private readonly ILogger<ConsumoSCnMcont> _logger;

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
        public ConsumoSCnMcont(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoSCnMcont> logger,
            IMapper mapper, 
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.SevenWSMovContable);
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
        private SCNMCONTSoapClient ObtenerInstanciaCliente() => new SCNMCONTSoapClient(basicHttpBinding, endpointAddress);


        /// <summary>
        /// Metodo de instancia InsertarCnMCont
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarCnMcontResponse> InsertarCnMcontAsync(InsertarCnMcontRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());

            //var respuesta = await _mcont.InsertarCnMcontAsync(request);

            var respuesta = await _auditoria.EjecutarServicio<InsertarCnMcontRequest, InsertarCnMcontResponse>(
            _mcont.InsertarCnMcontAsync, request, ReferenciaSvc.Seven);

            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia AnularCnMCont
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<AnularCnMcontResponse> AnularCnMcontAsync(AnularCnMcontRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<AnularCnMcontRequest, AnularCnMcontResponse>(
            _mcont.AnularCnMcontAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia AplicarCnMCont
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<AplicarCnMcontResponse> AplicarCnMcontAsync(AplicarCnMcontRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<AplicarCnMcontRequest, AplicarCnMcontResponse>(
            _mcont.AplicarCnMcontAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia InsertarCnDmcon
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarCnDmconResponse> InsertarCnDmconAsync(InsertarCnDmconRequest request, CancellationToken cancellationToken)
        {

            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<InsertarCnDmconRequest, InsertarCnDmconResponse>(
            _mcont.InsertarCnDmconAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia InsertarMovContableRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<InsertarMovContableResponse> InsertarMovContableAsync(InsertarMovContableRequest request, CancellationToken cancellationToken)
        {

            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<InsertarMovContableRequest, InsertarMovContableResponse>(
            _mcont.InsertarMovContableAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia RevertirCnMcontRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<RevertirCnMcontResponse> RevertirCnMcontAsync(RevertirCnMcontRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<RevertirCnMcontRequest, RevertirCnMcontResponse>(
            _mcont.RevertirCnMcontAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }

        /// <summary>
        /// Metodo de instancia SetContabToperRequest
        /// </summary>
        /// <param name="request">modelo request </param>
        /// <returns>modelo response servicio</returns>
        public async Task<SetContabToperResponse> SetContabToperAsync(SetContabToperRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _mcont = this.ObtenerInstanciaCliente();
            await _mcont.OpenAsync();
            _logger.LogInformation(_mcont.State.ToString());
            var respuesta = await _auditoria.EjecutarServicio<SetContabToperRequest, SetContabToperResponse>(
            _mcont.SetContabToperAsync, request, ReferenciaSvc.Seven);
            return respuesta;
        }
    }
}
