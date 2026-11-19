using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    public class GenerarReporteAfiliados : BaseAsyncEndpoint.WithRequest<ReporteAfiliadosRequestDto>.WithResponse<Response<IEnumerable<ReporteAfiliadosResponseDto>>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Constructor e inyección de dependencias
        /// </summary>
        /// <param name="clientsBussinesLogic"></param>
        public GenerarReporteAfiliados(IClientesBusinessLogic clientsBussinesLogic)
        {
            _clientsBusinessLogic = clientsBussinesLogic;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> 
        [SwaggerOperation(
        Summary = "Trae los datos de un usuario",
           Description = "Trae los datos de un usuario al enviar un id por request",
           OperationId = "Client.GenerarReporteAfiliados",
           Tags = new[] { "ClientEndpoint" })]
        [HttpPost("api/clientes/GenerarReporteAfiliados")]
        public override async Task<ActionResult<Response<IEnumerable<ReporteAfiliadosResponseDto>>>> HandleAsync(ReporteAfiliadosRequestDto request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.GenerarReporteAfiliados(request);
        }
    }
}
