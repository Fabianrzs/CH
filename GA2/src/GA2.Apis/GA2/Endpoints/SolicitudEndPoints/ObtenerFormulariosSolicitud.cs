using System;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
	/// Endpoint para la lista de formularios por modalidad solicitud
	/// </summary>
	/// <author>David Pineda</author>
	/// <date>15/11/2022</date>
   // [Authorize]
    public class ObtenerFomulariosSolicitud :
        BaseAsyncEndpoint.WithRequest<string>.
        WithResponse<Response<FormulariosSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Aprobar solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>20/10/2021</date>
        public ObtenerFomulariosSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreActividad"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/solicitud/ObtenerFomulariosSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<FormulariosSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene los formularios parametrizados por sub modalidad",
           Description = "Obtener formualrios de solicitud",
           OperationId = "Solicitud.formularios",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<FormulariosSolicitudDto>>> HandleAsync(string timCodigo, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerFomulariosSolicitud(timCodigo);
        }
    }
}

