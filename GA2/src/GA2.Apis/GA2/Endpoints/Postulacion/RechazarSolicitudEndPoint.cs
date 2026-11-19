
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Postulacion;
using System.Linq;

namespace GA2.Apis.GA2.Endpoints
{
    public class RechazarSolicitudEndPoint :  BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<SolicituPostulancionResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        public RechazarSolicitudEndPoint(IPostulacionBussineLogic bussigneLogic)
    {
        _postulacionBussineLogic = bussigneLogic;
    }
    [HttpPost("api/postulante/declineSolicitud")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Rechazar una solicitud postulancion",
        Description = "Rechazar la solicitud de postulacion por solicitud id",
        OperationId = "postulante.declineSolicitud",
        Tags = new[] { "PostulanteEndPoint" })]
    public async override Task<ActionResult<Response<SolicituPostulancionResponseDto>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
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
            var userIdParce = Guid.Parse(userId);
            return await _postulacionBussineLogic.UpdateStatusRecline(id,userIdParce);
    }
}
}
