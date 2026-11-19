using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Postulacion;
using System.Linq;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class AprobarSolicitudPostulacionEndPoint : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<SolicituPostulancionResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public AprobarSolicitudPostulacionEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/aprovedSolicitud")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aprobar solicitud postulancion",
            Description = "Aprobar la solicitud de postulacion",
            OperationId = "postulante.aprovedSolicitud",
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
            var UserId = Guid.Parse(userId);
            return await _postulacionBussineLogic.UpdateStatusAproved(id, UserId);
        }
    }
}
