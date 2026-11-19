using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Postulacion;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints
{
    public class ValidarSolicitudPostulacion : BaseAsyncEndpoint.WithRequest<long>
        .WithResponse<Response<SolicituPostulancionResponseDto>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ValidarSolicitudPostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/validarSolicitud")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Validar si existe una solicitud",
            Description = "Validar si existe una solicitud para ese cliente",
            OperationId = "postulante.validarSolicitud",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicituPostulancionResponseDto>>> HandleAsync(long id,CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ValidarSolicitud(id);
        }
    }
}
