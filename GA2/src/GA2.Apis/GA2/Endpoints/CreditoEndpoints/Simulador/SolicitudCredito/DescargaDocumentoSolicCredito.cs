using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class DescargaDocumentoSolicCredito : BaseAsyncEndpoint.WithRequest<string>.WithResponse<IFormFile>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public DescargaDocumentoSolicCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Descararga Archivo del blob storage para solicitud credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/descargaDocumentoSolicCredito")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation("Descarga Archivo al blob storage para solicitud credito",
            Description = "Descarga Archivo al blob storage para solicitud credito",
            OperationId = "credito.DesargaDocumentoSolicCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<IFormFile>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.DescargaDocumentoSolicCredito(request);
        }
    }
}
