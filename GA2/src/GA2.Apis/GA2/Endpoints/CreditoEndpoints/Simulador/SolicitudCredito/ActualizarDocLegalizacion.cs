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

namespace GA2.Endpoints.Credito.Simulador
{
    /// <summary>
    /// Endpoint para actualizar el estado de documentos Act Solicitud
    /// <author>Nicolas Florez Sarrazola</author>
    /// </summary>
    [Authorize]
    public class ActualizarDocLegalizacion : BaseAsyncEndpoint.WithRequest<DocumentosActEstadoRequestDto>.WithResponse<Response<DocumentosActRespuestaDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ActualizarDocLegalizacion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Endpoint para actualizar el estado de documentos Act Solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ActualizarDocLegalizacion")]
        [ProducesResponseType(typeof(Response<DocumentosActRespuestaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "actualiza los estados de los Documentos Act",
            Description = "actualiza los estados de los Documentos Act",
            OperationId = "credito.ActualizarDocLegalizacion",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<DocumentosActRespuestaDto>>> HandleAsync(DocumentosActEstadoRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ActualizarDocLegalizacion(request);
        }
    }
}
