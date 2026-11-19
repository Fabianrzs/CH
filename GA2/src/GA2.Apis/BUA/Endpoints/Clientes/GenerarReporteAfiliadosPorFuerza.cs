using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    public class GenerarReporteAfiliadosPorFuerza : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<ReporteFuerzaResumenDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Constructor e inyección de dependencias
        /// </summary>
        /// <param name="clientsBussinesLogic"></param>
        public GenerarReporteAfiliadosPorFuerza(IClientesBusinessLogic clientsBussinesLogic)
        {
            _clientsBusinessLogic = clientsBussinesLogic;
        }

        /// <summary>
        /// </summary>
        /// <param name="idFuerza"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> 
        [SwaggerOperation(
        Summary = "Trae los datos de un usuario",
           Description = "Trae los datos de un usuario al enviar un id por request",
           OperationId = "Client.GenerarReporteAfiliadosPorFuerza",
           Tags = new[] { "ClientEndpoint" })]
        [HttpGet("api/clientes/GenerarReporteAfiliadosPorFuerza")]
        public override async Task<ActionResult<Response<ReporteFuerzaResumenDto>>> HandleAsync(int idFuerza, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.GenerarReporteAfiliadosPorFuerza(idFuerza);
        }
    }
}
