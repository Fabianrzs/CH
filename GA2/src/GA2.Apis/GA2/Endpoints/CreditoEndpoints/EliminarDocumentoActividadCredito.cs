using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class EliminarDocumentoActividadCredito
        : BaseAsyncEndpoint
            .WithRequest<DocumentosCredito>
            .WithResponse<Response<bool>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Eliminar Documento Actividad Crédito
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>
        public EliminarDocumentoActividadCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Elimina un documento asociado a una actividad de crédito.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("api/credito/EliminarDocumentoActividadCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Eliminar documento actividad crédito",
            Description = "Elimina un documento asociado a una actividad de crédito.",
            OperationId = "Credito.EliminarDocumentoActividadCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(
            DocumentosCredito request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.EliminarDocumentoActividadCredito(request);
        }
    }
}