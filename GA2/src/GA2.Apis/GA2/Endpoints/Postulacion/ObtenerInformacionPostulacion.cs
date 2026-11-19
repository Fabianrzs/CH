using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ObtenerInformacionPostulacion : BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<Response<SolicituPostulancionResponseDto>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ObtenerInformacionPostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/ObtenerInformcacionPostulacion")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener la informacion de la postulacion por radicado",
            Description = "Obtener la informacion de la postulacion por radicado",
            OperationId = "postulante.ObtenerInformcacionPostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicituPostulancionResponseDto>>> HandleAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerInformcacionPostulacion(id);
        }
    }
}
