using GA2.Domain.Core.ServicesOtpSvc;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Otp
{
    /// <summary>
    /// Consumo Otp service
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>22/11/2021</date>
    public class ConsumoServiceOtp : IConsumoServiceOtp
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
        private readonly ILogger<ConsumoServiceOtp> _logger;

        private readonly IAuditoriaServicesBusinessLogic _auditoria;
        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoServiceOtp(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoServiceOtp> logger, IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.ServiceOtpSvc);
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
            _auditoria = auditoria;
        }

        /// <summary>
        /// Obtener la instancia de conexion service otp
        /// </summary>
        /// <returns>Instancia de servicio ServiceOtp</returns>
        private SCNMCONTSoapClient ObtenerInstanciaCliente() => new SCNMCONTSoapClient(basicHttpBinding, endpointAddress);

        /// <summary>
        /// Validar Otp
        /// </summary>
        /// <param name="OtpCode">Modelo otp</param>
        /// <returns>Otp Validado</returns>
        public async Task<ValidaOTPResponse> ValidarOtpAsync(ValidaOTPRequest otpCode, CancellationToken cancellationToken)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _otp = this.ObtenerInstanciaCliente();
            await _otp.OpenAsync();
            _logger.LogInformation(_otp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<ValidaOTPRequest, ValidaOTPResponse>(
            _otp.ValidaOTPAsync, otpCode, ReferenciaSvc.OTP);
            return respuesta; 
        }

        /// <summary>
        /// Genera el otp
        /// </summary>
        /// <param name="OtpCode">modelo de otp</param>
        /// <returns>Otp Respon</returns>
        public async Task<CrearOTPResponse> GenerarOtpAsync(CrearOTPRequest OtpCode, CancellationToken cancellationToken)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _otp = this.ObtenerInstanciaCliente();
            await _otp.OpenAsync();
            _logger.LogInformation(_otp.State.ToString());
            

            var respuesta = await _auditoria.EjecutarServicio<CrearOTPRequest, CrearOTPResponse>(
            _otp.CrearOTPAsync, OtpCode, ReferenciaSvc.OTP);
            return respuesta;
        }

        /// <summary>
        /// Genera el otp
        /// </summary>
        /// <param name="OtpCode">modelo de otp</param>
        /// <returns>Otp Respon</returns>
        public async Task<CrearOTPEmailResponse> CrearOTPEmailAsync(CrearOTPEmailRequest otpEmail, CancellationToken cancellationToken)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            if (cancellationToken.IsCancellationRequested) return null;
            /// creacion de instancia consumo
            using var _otp = this.ObtenerInstanciaCliente();
            await _otp.OpenAsync();
            _logger.LogInformation(_otp.State.ToString());

            var respuesta = await _auditoria.EjecutarServicio<CrearOTPEmailRequest, CrearOTPEmailResponse>(
            _otp.CrearOTPEmailAsync, otpEmail, ReferenciaSvc.OTP);
            return respuesta;
        }
    }
}
