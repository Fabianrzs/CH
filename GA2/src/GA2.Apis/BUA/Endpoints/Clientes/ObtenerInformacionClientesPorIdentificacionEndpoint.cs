using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using GA2.Application.Dto.BUA;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ObtenerInformacionClientesPorIdentificacionEndpoint : BaseAsyncEndpoint
        .WithRequest<IEnumerable<string>>
        .WithResponse<Response<IEnumerable<InformacionBasicaClienteDto>>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public ObtenerInformacionClientesPorIdentificacionEndpoint(IClientesBusinessLogic clientsBusinessLogic)
        {
            _clientsBusinessLogic = clientsBusinessLogic;
        }

        /// <summary>
        /// ObtenerInformacionClientesPorIdentificacion
        /// </summary>
        /// <param name="documentos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/clientes/ObtenerInformacionClientesPorIdentificacion")]
        [SwaggerOperation(
           Summary = "Reporte Informacion Cliente",
           Description = "Reporte Informacion Cliente",
           OperationId = "Client.ObtenerInformacionClientesPorIdentificacion",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<InformacionBasicaClienteDto>>>> HandleAsync(IEnumerable<string> documentos, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ObtenerInformacionClientesPorIdentificacion(documentos);
        }
    }
}
