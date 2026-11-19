using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para iniciar el pago cuota inicial credito hipotecario en seven.
    /// </summary>
    /// <author>Reison Torres</author>
    [Authorize]
    public class PagoCuotaInicialSeven : BaseAsyncEndpoint
        .WithRequest<ParametrosObtenerCesantiasDto>
        .WithResponse<Response<PagosSevenDto>>
    {
        private readonly IHipotecaBusinessLogic _hipotecaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hipotecaBusinessLogic"></param>
        public PagoCuotaInicialSeven(IHipotecaBusinessLogic hipotecaBusinessLogic)
        {
            _hipotecaBusinessLogic = hipotecaBusinessLogic;
        }

        /// <summary>
        /// Endpoint para iniciar el pago cuota inicial credito hipotecario en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/pagoCuotaInicalSeven")]
        [ProducesResponseType(typeof(Response<PagosSevenDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Proceso de pago cuota inicial credito hipotecario en seven",
            Description = "Proceso de pago cuota inicial credito hipotecario en seven",
            OperationId = "credito.pagoCuotaInicalSeven",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<PagosSevenDto>>> HandleAsync(ParametrosObtenerCesantiasDto request, CancellationToken cancellationToken = default)
        {
            return await this._hipotecaBusinessLogic.PagoCuotaInicalSeven(request, cancellationToken);
        }
    }
}
