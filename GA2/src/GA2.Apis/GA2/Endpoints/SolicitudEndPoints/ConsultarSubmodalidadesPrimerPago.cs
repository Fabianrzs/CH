using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para consultar todas submodaldades de primer pago
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class ConsultarSubmodalidadesPrimerPago : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ConsultarSubmodalidadPrimerPago>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor de la clase del endpoint donde se consultan las submodalidades de primer pago
        /// </summary>        
        public ConsultarSubmodalidadesPrimerPago(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        ///  Consulta las submodalidades de primer pago
        /// </summary>        
        /// <param name="cancellationToken"></param>        
        [HttpGet("api/ConsultarSubmodalidadesPrimerPago")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarSubmodalidadPrimerPago>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consulta las submodalidades de primer pago",
           Description = "Consulta las submodalidades de primer pago",
           OperationId = "Solicitud.ConsultarSubmodalidadPrimerPago",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultarSubmodalidadPrimerPago>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarSubmodalidadesDePrimerPago(); 
        }
    }
}
