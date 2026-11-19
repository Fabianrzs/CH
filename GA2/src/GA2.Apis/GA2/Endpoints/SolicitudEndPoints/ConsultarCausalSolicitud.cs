using Ardalis.ApiEndpoints;
using GA2.Domain.Entities.Solicitud;
using System.Collections.Generic;
using System;
using GA2.Transversals.Commons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para obtener causal de seguridad documental grupo 1 y grupo 2
    /// </summary>
    public class ConsultarCausalSolicitud : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<ConsultarCausalInterrupcionSolicitud>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsultarCausalSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// Obtener los tipos las causales 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/ConsultarCausalSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarCausalInterrupcionSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Causales de interrupcion",
           Description = "Obtener la causales de interrupcion",
           OperationId = "Solicitud.ConsultarCausalSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultarCausalInterrupcionSolicitud>>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarCausalSolicitud(request); 
        }
    }
}
