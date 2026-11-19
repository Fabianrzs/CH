using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using GA2.Domain.Core.SolicitudConvocatoria;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{/// <summary>
/// 
/// </summary>
    public class ObtenerSolicituPostulanteEndPoint : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<TipoSolicitudPostulacionDto>>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ObtenerSolicituPostulanteEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/ConsultarTipoSolicitudPostulante")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoSolicitudPostulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
    Summary = "Obtiene todos los tipo de solicitud",
    Description = "Obtiene todos los tipo de solicitud para la postulacion ",
    OperationId = "postulante.ConsultarTipoSolicitudPostulante",
    Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoSolicitudPostulacionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ConsultarSolicitud();
        }
    }
}
