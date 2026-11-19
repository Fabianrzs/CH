using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ConsultarArchivoFsEndPoint : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<ArchivoSolicitudDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ConsultarArchivoFsEndPoint(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Consulta Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/ConsultarArchivoFsPorSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ArchivoSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar archivos por solicitud",
           Description = "Consultar archivos por solicitud",
           OperationId = "postulante.ConsultarArchivoFsPorSolicitud",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ArchivoSolicitudDto>>>> HandleAsync(Guid IdSolicitud, CancellationToken cancellationToken = default)
        {
            return await  _postulacionBussineLogic.ConsultarArchivoPorSolicitudPostulacion(IdSolicitud);
        }
    }
}
