using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para consultar las cuentas afectadas
    /// </summary>
    /// <author>Alejandro Lara</author>
    /// <date>11/03/2024</date>
    public class ObtenerCuentasAfectadasCliente : BaseAsyncEndpoint.WithRequest<ObtenerConceptosAfectadosClientes>
        .WithResponse<Response<IEnumerable<CuentasAfectadasSolicitud>>>
    {


        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar las cuentas afectadas
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="solicitudBusinessLogic"></param>
        /// <date>11/03/2024</date>
        public ObtenerCuentasAfectadasCliente(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// Aquí se llama el metodo que consulta las cuentas afectadas
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ObtenerCuentasAfectadasCliente")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CuentasAfectadasSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar los conceptos afectados",
           Description = "Consultar los conceptos afectados",
           OperationId = "Solicitud.ObtenerCuentasAfectadasCliente",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CuentasAfectadasSolicitud>>>> HandleAsync(ObtenerConceptosAfectadosClientes request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ObtenerCuentasAfectadas(request); 
        }
    }
}
