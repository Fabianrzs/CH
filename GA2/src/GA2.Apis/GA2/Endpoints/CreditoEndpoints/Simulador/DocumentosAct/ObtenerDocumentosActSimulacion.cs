using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.DocumentosAct
{
    [Authorize]
    public class ObtenerDocumentosActSimulacion:BaseAsyncEndpoint.WithRequest<DocumentosActSimulacionDto>.WithResponse<Response<IEnumerable<DocumentosActRespuestaDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerDocumentosActSimulacion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Metodo que obtiene los documentos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/obtenerDocumentosActSimulacion")]
        [ProducesResponseType(typeof(Response<IEnumerable<DocumentosActSimulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene Documentos Act",
            Description = "Obtiene Documentos Act",
            OperationId = "credito.ObtenerDocumentosActSimulacion",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<DocumentosActRespuestaDto>>>> HandleAsync(DocumentosActSimulacionDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(request);
        }
    }
}
