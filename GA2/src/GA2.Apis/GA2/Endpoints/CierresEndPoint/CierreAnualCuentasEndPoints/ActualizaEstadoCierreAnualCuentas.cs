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

namespace GA2.Apis.GA2.Endpoints.CierreAnualEndPoints
{
    /// <summary>
    /// ActualizaEstadoCierreAnualCuentas
    /// </summary>
    [Authorize]
    public class ActualizaEstadoCierreAnualCuentas : BaseAsyncEndpoint
        .WithRequest<ActualizaCierreAnualCuentasDto>
        .WithResponse<Response<ProgramacionCierreAnualCuentasDto>>
    {

        private readonly ICierreAnualCuentasBusinessLogic _cierreAnualBusinessLogic;

        /// <summary>
        /// ActualizaEstadoCierreAnualCuentas
        /// </summary>
        /// <param name="cierreAnualBusinessLogic"></param>
        public ActualizaEstadoCierreAnualCuentas(ICierreAnualCuentasBusinessLogic cierreAnualBusinessLogic)
        {
            _cierreAnualBusinessLogic = cierreAnualBusinessLogic;
        }



        /// <summary>
        /// ActualizaEstadoCierreAnual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ActualizaEstadoCierreAnualCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProgramacionCierreAnualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Actualiza el estado del proceso de cierre Anual",
            Description = "Actualiza el estado del proceso de cierre Anual",
            OperationId = "CierreAnualEndpoint.ActualizaEstadoCierreAnual",
            Tags = new[] { "CierreAnualEndpoint" })]
        public override async Task<ActionResult<Response<ProgramacionCierreAnualCuentasDto>>> HandleAsync(ActualizaCierreAnualCuentasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cierreAnualBusinessLogic.ActualizaEstadoCierreAnualCuentas(request);
        }
    }
}
