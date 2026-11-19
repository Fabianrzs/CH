using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class UpdateValuePercent : BaseAsyncEndpoint
        .WithRequest<UpdateValorPercent>
        .WithResponse<Response<int>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public UpdateValuePercent(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/ActualizarSaldoPorcentaje")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<int>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar saldo porcentaje afiliado  Reintegro",
           Description = "Actualizar saldo porcentaje afiliado  Reintegro",
           OperationId = "Reintegros.ActualizarSaldoPorcentaje",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<int>>> HandleAsync(UpdateValorPercent request, CancellationToken cancellationToken = default)
        {
            return await this._reintegrosBusinessLogic.UpdateValuePercent(request);
        }
    }
}
