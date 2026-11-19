using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class MovContableReintegroSeven
     : BaseAsyncEndpoint
        .WithRequest<ParametrosMovContableReintegroDto>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public MovContableReintegroSeven(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }
        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de cesantias en seven primer paso crear el tercero en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Reintegros/MovContableReintegroSeven")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Proceso de movimientos contables intereses de cesantias seven",
            Description = "Proceso de movimientos contables de Ahorros y cesantias seven",
            OperationId = "Reintegros.MovContableReintegroSeven",
            Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(ParametrosMovContableReintegroDto request, CancellationToken cancellationToken = default)
        {
            return await this._reintegrosBusinessLogic.MovContableReintegroSeven(request, cancellationToken);
        }
    }
}
