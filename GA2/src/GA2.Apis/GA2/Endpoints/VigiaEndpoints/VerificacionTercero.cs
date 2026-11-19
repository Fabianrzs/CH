using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.VigiaEndpoints
{
    /// <summary>
    /// Endpoint genera validaciontercero vigia
    /// </summary>
    public class VerificacionTercero : BaseAsyncEndpoint.WithRequest<VerificacionTerceroRequest>.WithResponse<Response<ServiceVigiaVerificacionTercero.Resultado>>
    {
        private readonly IVigiaBusinessLogic _vigiaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="vigiaBusinessLogic"></param>
        public VerificacionTercero(IVigiaBusinessLogic vigiaBusinessLogic)
        {
            _vigiaBusinessLogic = vigiaBusinessLogic;
        }

        /// <summary>
        /// verificacionTercero
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/verificacionTercero")]
        [ProducesResponseType(typeof(Response<ServiceVigiaVerificacionTercero.Resultado>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Verificacion Tercero",
            Description = "Verificacion Tercero",
            OperationId = "Cartera.VerificacionTercero",
            Tags = new[] { "Vigia" })]
        public async override Task<ActionResult<Response<ServiceVigiaVerificacionTercero.Resultado>>> HandleAsync(VerificacionTerceroRequest request, CancellationToken cancellationToken = default)
        {
            return await this._vigiaBusinessLogic.ServiceVertificacionTerceroAsync(request);
        }
    }
}
