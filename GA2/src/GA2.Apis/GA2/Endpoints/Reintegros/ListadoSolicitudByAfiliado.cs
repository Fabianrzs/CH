using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class ListadoSolicitudByAfiliado : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<bool>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ListadoSolicitudByAfiliado
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ListadoSolicitudByAfiliado(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ListadoSolicitudByAfiliado
        /// </summary>
        /// <param name="idAfiliado"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/ListadoSolicitudByAfiliado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ReintefroDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener listado de solicitud Reintegro by Id",
           Description = "Obtener listado de solicitud  Reintegro by Id",
           OperationId = "Reintegros.ConsultarReintegroById",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<bool>> HandleAsync(string idAfiliado, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.GetListSolicitud(idAfiliado);
        }
    }
}
