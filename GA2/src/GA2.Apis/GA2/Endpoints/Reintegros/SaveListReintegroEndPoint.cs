using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using GA2.Application.Dto.Reintegros;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class SaveListReintegroEndPoint : BaseAsyncEndpoint
        .WithRequest<IEnumerable<AfiliadoReintegroListDto>>
        .WithResponse<Response<string>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;
        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public SaveListReintegroEndPoint(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/SaveListReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Guadar la lista de afiliados de Reintegro",
           Description = "Guadar la lista de afiliados de Reintegro",
           OperationId = "Reintegros.SaveListReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(IEnumerable<AfiliadoReintegroListDto> request, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.SaveListAfiliado(request);
        }
    }
}

