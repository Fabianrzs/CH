using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    /// <summary>
    /// AgregarAfiliadoReintegro
    /// </summary>
    /// [Authorize]
    public class AgregarAfiliadoReintegro : BaseAsyncEndpoint
        .WithRequest<AfiliadoReintegroParamDto>
        .WithResponse<Response<IEnumerable<AfiliadoReintegroDto>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// AgregarAfiliadoReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public AgregarAfiliadoReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// AfiliadoReintegroParamDto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/AgregarAfiliadoReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<List<AfiliadoReintegroDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Agregar Afiliado Reintegro",
           Description = "Crear Reintegro",
           OperationId = "Reintegros.AgregarAfiliadoReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<AfiliadoReintegroDto>>>> HandleAsync(AfiliadoReintegroParamDto request, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.AgregarAfiliadoReintegro(request);
        }
    }
}