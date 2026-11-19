using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Ardalis.ApiEndpoints;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class EliminarAfiliadoDelComprobante : BaseAsyncEndpoint.WithRequest<EliminarAfiliadoComprobanteDto>.WithResponse<Response<ComprobanteCarteraOperacionDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        public EliminarAfiliadoDelComprobante(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        [HttpPost("api/cartera/EliminarAfiliadoDelComprobante")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<ComprobanteCarteraPorProcesoDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Eliminar Afiliado Del Comprobante",
            Description = "Eliminar Afiliado Del Comprobante",
            OperationId = "cartera.EliminarAfiliadoDelComprobante",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<ComprobanteCarteraOperacionDto>>> HandleAsync(EliminarAfiliadoComprobanteDto request, CancellationToken cancellationToken = default)
        {
            //request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await _creditoBusinessLogic.EliminarAfiliadoDelComprobante(request);
        }
    }
}

