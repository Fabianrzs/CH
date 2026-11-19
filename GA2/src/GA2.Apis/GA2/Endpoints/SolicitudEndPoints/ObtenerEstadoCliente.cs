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

    public class ObtenerEstadoCliente : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<List<EstadosClienteDto>>>
    {
        
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Nelson Redondo</author>
        /// <date>20/10/2021</date>
        public ObtenerEstadoCliente(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Consultar Estado del Cliente 1
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/solicitud/ObtenerEstadosCliente")]
        [Consumes("application/json")]
        [SwaggerOperation(
           Summary = "Obtener Estado del Cliente",
           Description = "Obtener Estado del Cliente",
           OperationId = "Solicitud.ObtenerEstadoCliente",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<List<EstadosClienteDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerEstadosCliente();
        }
    }
}
