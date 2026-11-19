using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using System.Linq;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class AprobarCargeDocumentoPostulacionEndPoint : BaseAsyncEndpoint.WithRequest<AprobarCargueDocumentoDto>
        .WithResponse<Response<SolicituPostulancionResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public AprobarCargeDocumentoPostulacionEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/aprovarCargeDocumento")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aprobar cargue de documento postulancion",
            Description = "Aprobar cargue de document de postulacion",
            OperationId = "postulante.aprovarCargeDocumento",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicituPostulancionResponseDto>>> HandleAsync(AprobarCargueDocumentoDto request, CancellationToken cancellationToken = default)
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
            return await _postulacionBussineLogic.AprobarCargoDocumento(request, userIdParce);
        }
    }
}
