using GA2.Domain.Core.ServicesOtpSvc;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Otp
{
    /// <summary>
    /// Interface Consumo Service Otp
    /// </summary>
    public interface IConsumoServiceOtp
    {
        /// <summary>
        /// Genera el otp
        /// </summary>
        /// <param name="OtpCode">modelo de otp</param>
        /// <returns>Otp Respon</returns>
        Task<CrearOTPResponse> GenerarOtpAsync(CrearOTPRequest OtpCode, CancellationToken cancellationToken);
        /// <summary>
        /// Validar Otp
        /// </summary>
        /// <param name="OtpCode">Modelo otp</param>
        /// <returns>Otp Validado</returns>
        Task<ValidaOTPResponse> ValidarOtpAsync(ValidaOTPRequest otpCode, CancellationToken cancellationToken);

        /// <summary>
        /// Generar Otp via email
        /// </summary>
        /// <param name="otpEmail">Email to send request code</param>
        /// <param name="cancellationToken">CAncellation request</param>
        /// <returns>Email response otp created</returns>
        Task<CrearOTPEmailResponse> CrearOTPEmailAsync(CrearOTPEmailRequest otpEmail, CancellationToken cancellationToken);
    }
}