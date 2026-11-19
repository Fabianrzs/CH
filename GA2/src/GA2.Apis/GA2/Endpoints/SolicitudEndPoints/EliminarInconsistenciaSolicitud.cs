using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    [Authorize]

    public class EliminarInconsistenciaSolicitud :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<InconsistenciaSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Crear una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>31/05/2022</date>
        public EliminarInconsistenciaSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para crear una inconsistencia de solicitud
        /// </summary>
        /// <param name="insertarInconsistenciaSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpDelete("api/solicitud/EliminarInconsistenciaSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InconsistenciaSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Eliminar Inconsistencia",
           Description = "Eliminar Inconsistencia",
           OperationId = "Solicitud.EliminarInconsistenciaSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<InconsistenciaSolicitudDto>>> HandleAsync(Guid inconsistenciaId, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.EliminarInconsistenciaSolicitud(inconsistenciaId);
        }
    }
}
