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
    /// CierreMensualCuentas
    /// </summary>
    [Authorize]
    public class CierreMensualCuentas : BaseAsyncEndpoint
        .WithRequest<ParametrosCierreMensualCuentasDto>
        .WithResponse<Response<RespuestaCierreMensualCuentasDto>>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly ICierreMensualCuentasBusinessLogic _cierreMensualBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        public CierreMensualCuentas(ICierreMensualCuentasBusinessLogic cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/CierreMensualCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Remuneracion de Intereses",
            Description = "Generar Remuneracion de Intereses",
            OperationId = "CierreMensual.CierreMensual",
            Tags = new[] { "CierreMensualEndpoint" })]


        public override async Task<ActionResult<Response<RespuestaCierreMensualCuentasDto>>> HandleAsync(ParametrosCierreMensualCuentasDto request, CancellationToken cancellationToken = default)
        {
            return await _cierreMensualBusinessLogic.CierreMensualCuentas(request);
        }
    }
}
