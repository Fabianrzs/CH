using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Afiliaciones
{
    /// <summary>
    /// ObtenerNombresCliente
    /// </summary>
   [Authorize]
    public class ObtenerNombresClientesPorDocumentos : BaseAsyncEndpoint
        .WithRequest<List<string>>
        .WithResponse<Response<List<AfiliadoDto>>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public ObtenerNombresClientesPorDocumentos(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

        /// <summary>
        /// ObtenerNombresCliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ObtenerNombresClientesPorDocumentos")]
        [SwaggerOperation(
           Summary = "Obtener Nombres Clientes por documentos",
           Description = "Obtener Nombres Cliente por documentos",
           OperationId = "afiliaciones.ObtenerNombresClientesPorDocumentos",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<List<AfiliadoDto>>>> HandleAsync(List<string> request, CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.ObtenerNombresClientesPorDocumentos(request);
        }
    }
}
