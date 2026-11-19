using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ObtenerEstadoSolicitudFondo : BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<Response<EstadoSolicitudFondoDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ObtenerEstadoSolicitudFondo(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Consulta Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/obtenerEstadoSolicitudFondo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EstadoSolicitudFondoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar estado por radicado",
           Description = "Consultar estado por radicado",
           OperationId = "postulante.obtenerEstadoSolicitudFondo",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<EstadoSolicitudFondoDto>>> HandleAsync(string noRadicado, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerEstadoSolicitudFondo(noRadicado);
        }
    }
}
