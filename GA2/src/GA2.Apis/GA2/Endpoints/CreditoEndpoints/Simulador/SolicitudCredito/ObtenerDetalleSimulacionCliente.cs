using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// Endpoint para consultar el detalle de una simulación de crédito
    /// </summary>
 
    public class ObtenerDetalleSimulacionCliente : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<SimulacionDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerDetalleSimulacionCliente(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// Obtiene el Detalle de la Simulación de un Crédito
        /// </summary>
        /// <param name="request">Guid de la simulación de crédito</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/obtenerDetalleSimulacionCliente")]
        [ProducesResponseType(typeof(Response<IEnumerable<SimulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation("Obtener Detalle Simulacion Cliente",
            Description = "Obtener Detalle Simulacion Cliente",
            OperationId = "credito.ObtenerDetalleSimulacionCliente",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<SimulacionDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerDetalleSimulacionCliente(request);
        }
    }
}
