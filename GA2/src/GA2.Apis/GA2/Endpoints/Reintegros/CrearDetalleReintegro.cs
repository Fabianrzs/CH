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
    /// CrearDetalleReintegro
    /// </summary>
    /// [Authorize]
    public class CrearDetalleReintegro : BaseAsyncEndpoint
        .WithRequest<DetalleReintegroDto>
        .WithResponse<Response<DetalleReintegroDto>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// CrearDetalleReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public CrearDetalleReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearDetalleReintegro
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/CrearDetalleReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DetalleReintegroDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Reintegro",
           Description = "Crear Reintegro",
           OperationId = "Reintegros.CrearDetalleReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<DetalleReintegroDto>>> HandleAsync(DetalleReintegroDto request, CancellationToken cancellationToken = default)
        {
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await this._reintegrosBusinessLogic.CrearDetalleReintegro(request);
        }
    }
}