using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ActualizarSolicitudEndPoint : BaseAsyncEndpoint.WithRequest<SolicitudPostulacionDto>
        .WithResponse<Response<SolicitudPostulacionDto>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ActualizarSolicitudEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/updateSolicitudPostulacion")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar la solicitud de postulacion",
            Description = "Actualizar la solicitud de postulacion",
            OperationId = "postulante.updateSolicitudPostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicitudPostulacionDto>>> HandleAsync(SolicitudPostulacionDto solicitud, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.UpdateSolicitud(solicitud);
        }
    }
}
