using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    public class GenerarReporteDatosBasicosAfiliado : BaseAsyncEndpoint.WithRequest<ReporteDatosBasicosRequestDto>.WithResponse<Response<IEnumerable<ClienteDto>>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Constructor e inyección de dependencias
        /// </summary>
        /// <param name="clientsBussinesLogic"></param>
        public GenerarReporteDatosBasicosAfiliado(IClientesBusinessLogic clientsBussinesLogic)
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
           OperationId = "Client.GenerarReporteDatosBasicosAfiliado",
           Tags = new[] { "ClientEndpoint" })]
        [HttpGet("api/clientes/GenerarReporteDatosBasicosAfiliado")]
        public override async Task<ActionResult<Response<IEnumerable<ClienteDto>>>> HandleAsync([FromQuery]ReporteDatosBasicosRequestDto idFuerza, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.GenerarReporteDatosBasicosAfiliado(idFuerza);
        }
    }
}
