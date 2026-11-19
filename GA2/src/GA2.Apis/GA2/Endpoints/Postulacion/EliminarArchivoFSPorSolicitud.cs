using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class EliminarArchivoFSPorSolicitud : BaseAsyncEndpoint.WithRequest<EliminarArchivoPorSolicitudDto>
        .WithResponse<Response<bool>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussine;

        /// <summary>
        /// Constructor del Endpoint Eliminar Archivo de Solicitud Por Id
        /// </summary>
        /// <param name="PostulacionBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>08/03/2022</date>
        public EliminarArchivoFSPorSolicitud(IPostulacionBussineLogic postulacionBussine)
        {
            _postulacionBussine = postulacionBussine;
        }


        /// <param name="eliminarArchivoPorSolicitudDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/postulante/EliminarArchivoPorSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
               Summary = "Eliminar Archivos de Solicitud por Id",
               Description = "Eliminar Archivos de Solicitud por Id",
               OperationId = "postulante.EliminarArchivoPorSolicitud",
               Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(EliminarArchivoPorSolicitudDto eliminarArchivoPorSolicitudDto, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussine.EliminarArchivoPorSolicitud(eliminarArchivoPorSolicitudDto);
        }
    }
}
