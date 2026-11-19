using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    /// <summary>
    /// EndPoint para actualizar saldos cta y conceptos cta por pago en seven en Crédito y Césantias.
    /// </summary>
    [Authorize]
    public class ActualizarSaldoDebitoCuenta : BaseAsyncEndpoint
        .WithRequest<Domain.Entities.Movimiento>
        .WithResponse<Response<ConceptoCuenta>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public ActualizarSaldoDebitoCuenta(IClientesBusinessLogic clientesBusinessLogic)
        {
            _clientsBusinessLogic = clientesBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/clientes/ActualizaSaldosCtaConceptoDebito")]
        [ProducesResponseType(typeof(Response<ConceptoCuenta>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza los movientos obtenidos del proceso de pagos en seven de césantias y crédito",
           Description = "Actualiza tabla movimientos cta-conceptos.",
           OperationId = "client.InsertarMovimiento",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<ConceptoCuenta>>> HandleAsync(Domain.Entities.Movimiento request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ActualizarSaldoDebitoCuenta(request);
        }
    }
}
