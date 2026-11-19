using Ardalis.ApiEndpoints;
using GA2.Domain.Entities.Solicitud;
using System.Collections.Generic;
using System;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using GA2.Application.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks; 

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para obtener los tipos de novedad segun el id de novedad se le envie 
    /// </summary>
    public class ConsultarTipoNovedadSolicitud : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<ConsultarTipoNovedad>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsultarTipoNovedadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// Obtener los tipos de novedad segun el id de novedad se le envie 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/ConsultarTipoNovedadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarTipoNovedad>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Tipos de novededades",
           Description = "Obtener los tipos de novedad segun el id de novedad se le envie ",
           OperationId = "Solicitud.ConsultarTipoNovedadSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultarTipoNovedad>>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarTipoNovedadSolicitud(request); 
        }
    }
}
