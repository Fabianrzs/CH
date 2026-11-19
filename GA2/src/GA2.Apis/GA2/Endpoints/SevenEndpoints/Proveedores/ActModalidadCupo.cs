using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.Proveedores
{
    /// <summary>
    /// ActualizarModalidadCupo
    /// </summary>
    [Authorize]
    public class ActModalidadCupo : BaseAsyncEndpoint
        .WithRequest<ActModalidadCupoDto>
        .WithResponse<Response<ActModalidadCupoResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public ActModalidadCupo(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ActModalidadCupo")]
        [ProducesResponseType(typeof(Response<ActModalidadCupoResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Act Modalidad Cupo",
            Description = "Act Modalidad Cupo",
            OperationId = "Seven.ActModalidadCupo",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<ActModalidadCupoResponse>>> HandleAsync(ActModalidadCupoDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.ActModalidadCupoAsync(request, cancellationToken);
        }
    }
}
