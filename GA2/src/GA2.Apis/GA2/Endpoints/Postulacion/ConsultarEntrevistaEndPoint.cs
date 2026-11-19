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
using GA2.Application.Dto.Postulacion;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ConsultarEntrevistaEndPoint : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<EntresvistaResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ConsultarEntrevistaEndPoint(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Consulta Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/consultarEntrevistaById")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EntresvistaResponseDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar entrevita por solicitud",
           Description = "Consultar entrevita por solicitud",
           OperationId = "postulante.consultarEntrevistaById",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<EntresvistaResponseDto>>> HandleAsync(Guid IdSolicitud, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerEntrevitsById(IdSolicitud);
        }
    }
}
