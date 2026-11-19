using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reintegros.Seven
{
    public class PagosReintegrosSeven : BaseAsyncEndpoint
        .WithRequest<ParametroObtenerReintegroDto>
        .WithResponse<Response<PagosSevenDto>>
    {
        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public PagosReintegrosSeven(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de Reintegros en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Reintegros/PagosReintegrosSeven")]
        [ProducesResponseType(typeof(Response<PagosSevenDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Proceso de desembolso soliciitudes Reintegros en seven",
            Description = "Proceso de desembolso soliciitudes Reintegros en seven",
            OperationId = "Reintegros.PagosReintegrosSeven",
            Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<PagosSevenDto>>> HandleAsync(ParametroObtenerReintegroDto request, CancellationToken cancellationToken = default)
        {
            return await this._reintegrosBusinessLogic.PagosReintegrosSeven(request, cancellationToken);
        }
    }
}
