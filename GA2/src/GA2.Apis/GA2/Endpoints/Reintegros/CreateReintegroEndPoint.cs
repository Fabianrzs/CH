using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class CreateReintegroEndPoint : BaseAsyncEndpoint
        .WithRequest<ReintefroDto>
        .WithResponse<Response<ReintefroDto>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public CreateReintegroEndPoint(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/CrearteReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ReintefroDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Reintegro",
           Description = "Crear Reintegro",
           OperationId = "Reintegros.CrearteReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<ReintefroDto>>> HandleAsync(ReintefroDto request, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid().ToString();

            try
            {
                userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }

            request.UsuarioCreacion = Guid.Parse(userId);
            return await _reintegrosBusinessLogic.CreateReintegro(request);
        }
    }
}
