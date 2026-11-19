using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class GeneracionFechaFin : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<string>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public GeneracionFechaFin(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="idTipo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/UpdateDateEndReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Generacion de fecha fin Reintegro by Id",
           Description = "Generacion de fecha fin Reintegro by Id",
           OperationId = "Reintegros.UpdateDateEndReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<string>>> HandleAsync(Guid idReintegro, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.UpdateDateEndReintegro(idReintegro);
        }
    }
}
