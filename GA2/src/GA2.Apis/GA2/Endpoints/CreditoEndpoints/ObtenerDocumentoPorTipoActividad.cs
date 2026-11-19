using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ObtenerDocumentoPorTipoActividad
        : BaseAsyncEndpoint
            .WithRequest<DocumentosActSolicitud>
            .WithResponse<Response<IEnumerable<DocumentosActRespuesta>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Obtener documentos por tipo de actividad
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>
        public ObtenerDocumentoPorTipoActividad(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Obtiene los documentos configurados para un tipo de actividad
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("api/credito/ObtenerDocumentoPorTipoActividad")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<DocumentosActRespuesta>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener documentos por tipo de actividad",
            Description = "Obtiene los documentos configurados para un tipo de actividad (TAC_ID).",
            OperationId = "Credito.ObtenerDocumentoPorTipoActividad",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<DocumentosActRespuesta>>>> HandleAsync(
            DocumentosActSolicitud request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerDocumentosPorTAC(request);
        }
    }
}