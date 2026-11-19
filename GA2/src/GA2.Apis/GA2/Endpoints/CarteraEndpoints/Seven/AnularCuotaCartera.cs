using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using Microsoft.AspNetCore.Authorization;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.Comprobantes;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Seven
{
    [Authorize]
    public class AnularCuotaCartera : BaseAsyncEndpoint
        .WithRequest<RequestAnulacionCarteraSeven>
        .WithResponse<Response<List<ComprobanteCarteraPorProcesoDTO>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public AnularCuotaCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Anula Cuota en seven
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response con el resultado de la anulación</returns>
        [HttpPost("api/credito/AnularCuotaCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<List<ComprobanteCarteraPorProcesoDTO>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Anula los movimientos contables",
           Description = "Anula los movimientos contables en seven",
           OperationId = "Cartera.AnularCuotaCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<List<ComprobanteCarteraPorProcesoDTO>>>> HandleAsync(RequestAnulacionCarteraSeven request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.AnulacionCuotaCartera(request, cancellationToken);
        }
    }
}
