using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    /// <summary>
    /// CrearReintegro
    /// </summary>
    /// [Authorize]
    public class CrearReintegro : BaseAsyncEndpoint
        .WithRequest<ReintegrosDto>
        .WithResponse<Response<ReintegrosDto>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public CrearReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/CrearReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ReintegrosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Reintegro",
           Description = "Crear Reintegro",
           OperationId = "Reintegros.CrearReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<ReintegrosDto>>> HandleAsync(ReintegrosDto request, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid().ToString();
            
            try
            {
                userId = User.Claims.Where(c => c.Type == "AppIdentity").FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }

            request.CreadoPor = Guid.Parse(userId);
            return await this._reintegrosBusinessLogic.CrearReintegro(request);
        }
    }
}