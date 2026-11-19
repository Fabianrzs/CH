
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints
{
    /// <summary>
    /// Endpoint Obtener usuario por id
    /// <author>Nicolas Florez Sarrazola</author>
    /// </summary>
    [Authorize]
    public class ObtenerClienteReporte : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ClienteReporteDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBussinesLogic"></param>
        public ObtenerClienteReporte(IClientesBusinessLogic clientsBussinesLogic)
        {
            _clientsBusinessLogic = clientsBussinesLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> 
        ///         
        [HttpGet("api/clientes/ObtenerClienteReporte")]
        [ProducesResponseType(typeof(Response<ClienteReporteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(

        Summary = "Trae fechas para reporte",
           Description = "Trae fechas para reporte Haberes",
           OperationId = "Client.ObtenerClienteReporte",
           Tags = new[] { "ClientEndpoint" })]

    

        /// <returns></returns> 

        public override async Task<ActionResult<Response<ClienteReporteDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ObtenerClienteReporte(request);
        }
    }
}
