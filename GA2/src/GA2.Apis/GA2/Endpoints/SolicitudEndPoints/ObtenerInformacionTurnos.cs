using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities.Solicitud;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint carga documento solicitud de credito
    /// </summary>

    public class ObtenerInformacionTurnos : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<InformacionTurnoResponse>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Nelson Redondo</author>
        /// <date>20/10/2021</date>
        public ObtenerInformacionTurnos(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Consultar Informacion del turno
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/solicitud/ObtenerInformacionTurno")]
        [Consumes("application/json")]
        [SwaggerOperation(
           Summary = "Obtener Obtener Informacion del Turno selects",
           Description = "Obtener Obtener Informacion del Turno selects",
           OperationId = "Solicitud.ObtenerInformacionTurno",
           Tags = new[] { "ObtenerInformacionTurno" })]
        public async override Task<ActionResult<Response<InformacionTurnoResponse>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerInformacionTurno();
        }
    }
}
