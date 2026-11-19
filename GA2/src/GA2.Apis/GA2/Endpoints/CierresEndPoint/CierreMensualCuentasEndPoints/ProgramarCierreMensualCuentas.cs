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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ProgramarCierreMensualCuentas
    /// </summary>
    [Authorize]
    public class ProgramarCierreMensualCuentas : BaseAsyncEndpoint
        .WithRequest<ParametrosCierreMensualCuentasDto>
        .WithResponse<Response<ProgramacionCierreMensualCuentasDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICierreMensualCuentasBusinessLogic _cierreMensualBusinessLogic;

        /// <summary>
        /// ProgramarCierreMensualEndPoint Construct
        /// </summary>
        /// <param name="cierreMensualBusinessLogic"></param>
        public ProgramarCierreMensualCuentas(ICierreMensualCuentasBusinessLogic cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }


        /// <summary>
        /// ProgramarCierreMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPost("api/ProgramarCierreMensualCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProgramacionCierreMensualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Programar Cierre Mensual",
            Description = "Programar Cierre Mensual",
            OperationId = "CierreMensual.ProgramarCierreMensual",
            Tags = new[] { "CierreMensualEndpoint" })]
        public override async Task<ActionResult<Response<ProgramacionCierreMensualCuentasDto>>> HandleAsync(ParametrosCierreMensualCuentasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cierreMensualBusinessLogic.ProgramarCierreMensual(request);
        }
    }
}
