using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace GA2.Apis.GA2.Endpoints.CesantiasEndPoints.Seven
{
    /// <summary>
    /// Endpoint para iniciar el proceso de pago o desembolso de cesantías con Seven.
    /// </summary>
    /// <author>Camilo Brito</author>
    //[Authorize]
    public class MovContableCesantiasSeven : BaseAsyncEndpoint
        .WithRequest<ParametrosMovContableDto>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        private readonly ICesantiasBusinessLogic _cesantiasBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cesantiasBusinessLogic"></param>
        public MovContableCesantiasSeven(ICesantiasBusinessLogic cesantiasBusinessLogic)
        {
            _cesantiasBusinessLogic = cesantiasBusinessLogic;
        }
        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de cesantias en seven primer paso crear el tercero en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cesantias/MovContableCesantiasSeven")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Proceso de movimientos contables intereses de cesantias seven",
            Description = "Proceso de movimientos contables intereses de cesantias seven",
            OperationId = "cesantias.MovContableCesantiasSeven",
            Tags = new[] { "CesantiasEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(ParametrosMovContableDto request, CancellationToken cancellationToken = default)
        {
            return await this._cesantiasBusinessLogic.MovContableCesantiasSeven(request, cancellationToken);
        }
    }
}
