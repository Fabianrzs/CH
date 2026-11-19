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
    /// <summary>
    /// CierreAnual
    /// </summary>
    [Authorize]
    public class CierreAnualCuentas : BaseAsyncEndpoint.WithRequest<ProgramacionCierreAnualCuentasDto>
        .WithResponse<Response<RespuestaCierreAnualCuentasDto>>
    {
        /// <summary>
        /// ICierreAnualBusinessLogicBua
        /// </summary>
        private readonly ICierreAnualCuentasBusinessLogicBua _CierreAnualBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CierreAnualBusinessLogic"></param>
        public CierreAnualCuentas(ICierreAnualCuentasBusinessLogicBua CierreAnualBusinessLogic)
        {
            _CierreAnualBusinessLogic = CierreAnualBusinessLogic;
        }


        /// <summary>
        /// CierreAnual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/CierreAnualCuentas")]
        [ProducesResponseType(typeof(Response<RespuestaCierreAnualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Cierre Anual",
            Description = "Generar Cierre Anual",
            OperationId = "CierreAnualEndpoint.CierreAnual",
            Tags = new[] { "CierreAnualEndpoint" })]

        public override async Task<ActionResult<Response<RespuestaCierreAnualCuentasDto>>> HandleAsync(ProgramacionCierreAnualCuentasDto request, CancellationToken cancellationToken = default)
        {
            return await _CierreAnualBusinessLogic.CierreAnualCuentas(request);
        }

    }
}
