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

namespace GA2.Apis.GA2.Endpoints.CierreMensualEndPoints
{
    /// <summary>
    /// ActualizaEstadoCierreMensualCuentas
    /// </summary>
    [Authorize]
    public class ActualizaEstadoCierreMensualCuentas : BaseAsyncEndpoint.WithRequest<ActualizaCierreMensualCuentasDto>.WithResponse<Response<ProgramacionCierreMensualCuentasDto>>
    {

        private readonly ICierreMensualCuentasBusinessLogic _cierreMensualBusinessLogic;

        /// <summary>
        /// ActualizaEstadoCierreMensualCuentas
        /// </summary>
        /// <param name="cierreMensualBusinessLogic"></param>
        public ActualizaEstadoCierreMensualCuentas(ICierreMensualCuentasBusinessLogic cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }



        /// <summary>
        /// ActualizaEstadoCierreMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ActualizaEstadoCierreMensualCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProgramacionCierreMensualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Actualiza el estado del proceso de cierre mensual",
            Description = "Actualiza el estado del proceso de cierre mensual",
            OperationId = "CierreMensualEndpoint.ActualizaEstadoCierreMensual",
            Tags = new[] { "CierreMensualEndpoint" })]
        public override async Task<ActionResult<Response<ProgramacionCierreMensualCuentasDto>>> HandleAsync(ActualizaCierreMensualCuentasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cierreMensualBusinessLogic.ActualizaEstadoCierreMensual(request);
        }
    }
}
