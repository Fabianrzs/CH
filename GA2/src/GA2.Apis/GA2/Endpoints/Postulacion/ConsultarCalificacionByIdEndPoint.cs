using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ConsultarCalificacionByIdEndPoint : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<CalificacionResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ConsultarCalificacionByIdEndPoint(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Consulta Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/ConsultarCalificacionFsPorSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CalificacionResponseDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar calificacion por solicitud",
           Description = "Consultar calificacion por solicitud",
           OperationId = "postulante.ConsultarCalificacionFsPorSolicitud",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<CalificacionResponseDto>>> HandleAsync(Guid IdSolicitud, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ConsultarCalificacionByIdSolcitud(IdSolicitud);
        }
    }
}
