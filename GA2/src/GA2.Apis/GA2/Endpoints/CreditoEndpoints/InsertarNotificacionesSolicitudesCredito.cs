using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class InsertarNotificacionesSolicitudesCredito : BaseAsyncEndpoint.WithRequest<NotificacionesSolicitudesDto>.WithResponse<bool>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public InsertarNotificacionesSolicitudesCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/InsertarNotificacionesSolicitudesCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consulta de notificaciones",
           Description = "Consulta de notificaciones",
           OperationId = "Credito.InsertarNotificacionesSolicitudesCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<bool>> HandleAsync(NotificacionesSolicitudesDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarNotificacionesSolicitudes(request);
        }
    }
}
