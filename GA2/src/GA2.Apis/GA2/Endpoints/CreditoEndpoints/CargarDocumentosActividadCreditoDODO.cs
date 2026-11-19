using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Credito
{
    public class CargarDocumentosActividadCreditoDODO
        : BaseAsyncEndpoint
            .WithRequest<CargarDocumentosSolicitudCreditoDto>
            .WithResponse<Response<ResponseDocumentoSimDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint CargarDocumentosActividadCreditoDODO
        /// </summary>
        public CargarDocumentosActividadCreditoDODO(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpPost("api/CargarDocumentosActividadCreditoDODO")]
        [ProducesResponseType(typeof(Response<ResponseDocumentoSimDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Carga documentos de la actividad de crédito a DODO",
            Description = "Carga múltiples documentos al gestor documental WorkManager/DODO.",
            OperationId = "credito.CargarDocumentosActividadCreditoDODO",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ResponseDocumentoSimDto>>> HandleAsync(
            CargarDocumentosSolicitudCreditoDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.CargarDocumentosActividadCredito(request);
        }
    }
}