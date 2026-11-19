using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes.ReporteMasivo
{
    /// <summary>
    /// DescargarReporteMasivoEndPoint
    /// </summary>
    [Authorize]
    [ApiController]
    public class DescargarReporteMasivoEndPoint : ControllerBase
    {
        private readonly IReportesBusinessLogic reportesBusinessLogic;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// DescargarReporteMasivoEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <param name="distributedCache"></param>
        /// <exception cref="ArgumentException"></exception>
        public DescargarReporteMasivoEndPoint(IReportesBusinessLogic reportesBusinessLogic,
            IDistributedCache distributedCache)
        {
            this.reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(null, nameof(reportesBusinessLogic));
            this.distributedCache = distributedCache ?? throw new ArgumentException(null, nameof(distributedCache));
        }


        /// <summary>
        /// DescargarReporteMasivo
        /// </summary>
        /// <param name="idProceso"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/DescargarReporteMasivo")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            Summary = "Descargar reporte masivo",
            Description = "Descargar reporte masivo",
            OperationId = "Reportes.DescargarReporteMasivo",
            Tags = new[] { "ReportesEndpoint" })]
        public async Task<IActionResult> DescargarReporteMasivo(Guid idProceso)
        {
            var result = await reportesBusinessLogic.DescargarReporteMasivo(idProceso);
            if (result.IsSuccess)
            {
                return File(result.Data, "application/octet-stream", result.ReturnMessage);
            }

            return BadRequest(new Response<object>()
            {
                ReturnMessage = result.ReturnMessage,
                IsSuccess = false
            });
        }

        /// <summary>
        /// GenerarTokenReporteMasivo
        /// </summary>
        [HttpGet("api/Reportes/GenerarTokenReporteMasivo")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar token reporte masivo",
            Description = "Generar token reporte masivo",
            OperationId = "Reportes.GenerarTokenReporteMasivo",
            Tags = new[] { "ReportesEndpoint" })]
        public async Task<Response<string>> GenerarTemporalToken(Guid idProceso)
        {
            var idUsuario = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "USR_ID").Value);
            var result = await reportesBusinessLogic.ConsultarProcesoGeneracionReporte(idUsuario, idProceso);

            var tokenTemp = await GenerarTokenTemporal(idProceso);

            if (result.IsSuccess && tokenTemp != null)
            {
                return new Response<string>() { Data = tokenTemp };
            }

            return new Response<string>() { Data = null, IsSuccess = false, ReturnMessage = "No se pudo generar el token" };
        }

        /// <summary>
        /// DescargarReporteMasivo
        /// </summary>
        /// <param name="idProceso"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/Reportes/DescargarReporteMasivo/{idProceso}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            Summary = "Descargar reporte masivo",
            Description = "Descargar reporte masivo",
            OperationId = "Reportes.DescargarReporteMasivoToken",
            Tags = new[] { "ReportesEndpoint" })]
        public async Task<IActionResult> DescargarReporteMasivoConToken(Guid idProceso, string key)
        {
            if (await ValidateTempToken(idProceso, key))
            {
                var result = await reportesBusinessLogic.DescargarReporteMasivo(idProceso);

                if (result.IsSuccess)
                {
                    return File(result.Data, "application/octet-stream", result.ReturnMessage);
                }

            }

            return NotFound(null);
        }

        private async Task<string> GenerarTokenTemporal(Guid idProceso)
        {
            try
            {
                var tokenTemp = GenerarHash();

                await distributedCache.SetStringAsync($"reporte_masivo_{idProceso}", tokenTemp, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

                return tokenTemp;
            }
            catch (Exception)
            {
                return null;
            }

            static string GenerarHash()
            {
                Span<byte> randomBytes = stackalloc byte[32];

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }

                byte[] hashBytes = SHA256.HashData(randomBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.AppendFormat("{0:x2}", b);
                }

                return builder.ToString();
            }
        }

        private async Task<bool> ValidateTempToken(Guid idProceso, string token)
        {
            var key = $"reporte_masivo_{idProceso}";
            var value = await distributedCache.GetStringAsync(key);
            await distributedCache.RemoveAsync(key);

            return !string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(token) && value == token;
        }
    }
}