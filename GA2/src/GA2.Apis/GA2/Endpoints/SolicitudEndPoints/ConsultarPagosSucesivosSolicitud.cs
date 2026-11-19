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
    /// Endpoint para obtener pagos sucesivos de seguridad documental grupo 2
    /// </summary>
    public class ConsultarPagosSucesivosSolicitud : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<ConsultarPagoSucesivoSolicitud>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsultarPagosSucesivosSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
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
        [HttpPost("api/solicitud/ConsultarPagosSucesivosSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarPagoSucesivoSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Pagos sucesivos",
           Description = "Obtener los pagos sucesivos",
           OperationId = "Solicitud.ConsultarPagosSucesivosSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultarPagoSucesivoSolicitud>>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarPagoSucesivoSolicitud(request); 
        }
    }
}
