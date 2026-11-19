using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using System.Collections.Generic;
using System;
using GA2.Domain.Entities.Solicitud;
using System.Threading;
using GA2.Transversals.Commons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using GA2.Application.Dto.Solicitud;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para consultar los conceptos afectados
    /// </summary>
    /// <author>Alejandro Lara</author>
    /// <date>11/03/2024</date>
    public class ObtenerConceptosAfectadosCliente : BaseAsyncEndpoint.WithRequest<ObtenerConceptosAfectadosClientes>
        .WithResponse<Response<IEnumerable<CuentasAfectadasSolicitud>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar los conceptos afectados
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="solicitudBusinessLogic"></param>
        /// <date>11/03/2024</date>
        public ObtenerConceptosAfectadosCliente(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// Aquí se llama el metodo que consulta los conceptos afectados 
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ObtenerConceptosAfectadosCliente")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CuentasAfectadasSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar los conceptos afectados",
           Description = "Consultar los conceptos afectados",
           OperationId = "Solicitud.ObtenerConceptosAfectadosCliente",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<CuentasAfectadasSolicitud>>>> HandleAsync(ObtenerConceptosAfectadosClientes request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ObtenerConceptosAfectado(request);
        }
    }
}
