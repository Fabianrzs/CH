using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ConsultarEtapaPostulacionEndPoint : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<EtapaPostulacionDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ConsultarEtapaPostulacionEndPoint(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Consulta Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/consultarEtapaFs")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EtapaPostulacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar etapa de postulacion ",
           Description = "Consultar etapa de postulacion",
           OperationId = "postulante.consultarEtapaFs",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EtapaPostulacionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerEtapaPostulacion();
        }
    }
}
