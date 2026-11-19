using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class UpdateListAfiliados : BaseAsyncEndpoint
        .WithRequest<IEnumerable<AfiliadosReintegrosDto>>
        .WithResponse<Response<string>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;
        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public UpdateListAfiliados(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/UpdateListAfiliados")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "update la lista de afiliados de Reintegro",
           Description = "update la lista de afiliados de Reintegro",
           OperationId = "Reintegros.UpdateListAfiliados",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(IEnumerable<AfiliadosReintegrosDto> request, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.UpdateListAfiliado(request);
        }
    }
}
