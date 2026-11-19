using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class UpdateRequestStage : BaseAsyncEndpoint.WithRequest<UpdateRequestStageDto>
        .WithResponse<Response<SolicituPostulancionResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public UpdateRequestStage(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/UpdateRequestStageDto")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar etapa de la solicitud",
            Description = "Poner el cargue de documento como pendiete y rechazar la etapa",
            OperationId = "postulante.UpdateRequestStageDto",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicituPostulancionResponseDto>>> HandleAsync(UpdateRequestStageDto request, CancellationToken cancellationToken = default)
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
            return await _postulacionBussineLogic.UpdateStatusRequestStage(request,userIdParce);
        }
    }
}
