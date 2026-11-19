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

namespace GA2.Apis.BUA.Endpoints
{
    [Authorize]
    /// <summary>
    /// CierreMensual
    /// </summary>
    public class CierreMensualCuentas : BaseAsyncEndpoint
        .WithRequest<ProgramacionCierreMensualCuentasDto>
        .WithResponse<Response<RespuestaCierreMensualCuentasDto>>
    {
        /// <summary>
        /// ICierreMensualBusinessLogicBua
        /// </summary>
        private readonly ICierreMensualBusinessLogicBua _cierreMensualBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cierreMensualBusinessLogic"></param>
        public CierreMensualCuentas(ICierreMensualBusinessLogicBua cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }


        /// <summary>
        /// CierreMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/CierreMensualCuentas")]
        [ProducesResponseType(typeof(Response<RespuestaCierreMensualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Remuneracion de Intereses",
            Description = "Generar Remuneracion de Intereses",
            OperationId = "CierreMensualEndpoint.CierreMensual",
            Tags = new[] { "CierreMensualEndpoint" })]

        public override async Task<ActionResult<Response<RespuestaCierreMensualCuentasDto>>> HandleAsync(ProgramacionCierreMensualCuentasDto request, CancellationToken cancellationToken = default)
        {
            return await _cierreMensualBusinessLogic.CierreMensualCuentas(request);
        }

    }
}
