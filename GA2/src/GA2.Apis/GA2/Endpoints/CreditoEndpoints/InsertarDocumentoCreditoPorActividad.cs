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
    public class InsertarDocumentoCreditoPorActividad
        : BaseAsyncEndpoint
            .WithRequest<DocumentosCredito>
            .WithResponse<Response<ValidacionCreditoProspeccion>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Insertar Documento Crédito por Actividad
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>
        public InsertarDocumentoCreditoPorActividad(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Inserta o actualiza la configuración de un documento asociado a una actividad.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("api/credito/InsertarDocumentoCreditoPorActividad")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ValidacionCreditoProspeccion>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar documento crédito por actividad",
            Description = "Inserta o actualiza la configuración de un documento asociado a una actividad.",
            OperationId = "Credito.InsertarDocumentoCreditoPorActividad",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ValidacionCreditoProspeccion>>> HandleAsync(
            DocumentosCredito request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarDocumentoActividadCredito(request);
        }
    }
}