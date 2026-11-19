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

    [Authorize]
    /// <summary>
    /// Programar Cierre Anual
    /// </summary>

    public class ProgramarCierreAnualCuentas : BaseAsyncEndpoint
        .WithRequest<ParametrosCierreAnualDto>
        .WithResponse<Response<ProgramacionCierreAnualCuentasDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICierreAnualCuentasBusinessLogic _cierreAnualBusinessLogic;

        /// <summary>
        /// ProgramarCierreAnualEndPoint Construct
        /// </summary>
        /// <param name="cierreAnualBusinessLogic"></param>
        public ProgramarCierreAnualCuentas(ICierreAnualCuentasBusinessLogic cierreAnualBusinessLogic)
        {
            _cierreAnualBusinessLogic = cierreAnualBusinessLogic;
        }


        /// <summary>
        /// ProgramarCierreAnual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPost("api/ProgramarCierreAnualCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProgramacionCierreAnualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Programar Cierre Anual Cuentas",
            Description = "Programar Cierre Anual Cuentas",
            OperationId = "CierreAnual.ProgramarCierreAnualCuentas",
            Tags = new[] { "CierreAnualEndpoint" })]
        public override async Task<ActionResult<Response<ProgramacionCierreAnualCuentasDto>>> HandleAsync(ParametrosCierreAnualDto request, CancellationToken cancellationToken = default)
        {
            return await this._cierreAnualBusinessLogic.ProgramarCierreAnualCuentas(request);
        }
    }
}
