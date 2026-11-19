using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Afiliaciones
{
    /// <summary>
    /// ObtenerNombresCliente
    /// </summary>
    [Authorize]
    public class ObtenerNombresCliente : BaseAsyncEndpoint
        .WithRequest<DocumentoDTO>
        .WithResponse<Response<AfiliadoDto>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public ObtenerNombresCliente(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

        /// <summary>
        /// ObtenerNombresCliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ObtenerNombresCliente")]
        [SwaggerOperation(
           Summary = "Obtener Nombres Cliente",
           Description = "Obtener Nombres Cliente",
           OperationId = "afiliaciones.ObtenerNombresCliente",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<AfiliadoDto>>> HandleAsync([FromQuery] DocumentoDTO request, CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.ObtenerNombresCliente(request.numeroDocumento);
        }
    }
}
