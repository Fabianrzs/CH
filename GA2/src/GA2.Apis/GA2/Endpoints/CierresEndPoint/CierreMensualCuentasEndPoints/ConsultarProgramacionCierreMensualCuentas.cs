using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ConsultarProgramacionCierreMensualCuentas
    /// </summary>
    [Authorize]
    public class ConsultarProgramacionCierreMensualCuentas : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICierreMensualCuentasBusinessLogic _cierreMensualBusinessLogic;

        /// <summary>
        /// ConsultarProgramacionCierreMensualEndPoint Construct
        /// </summary>
        /// <param name="cierreMensualBusinessLogic"></param>
        public ConsultarProgramacionCierreMensualCuentas(ICierreMensualCuentasBusinessLogic cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }


        /// <summary>
        /// ProgramarCierreMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>


        [HttpGet("api/ConsultarProgramacionCierreMensual")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProgramacionCierreMensualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Consultar Programacion Cierre Mensual",
            Description = "Consultar Programacion Cierre Mensual",
            OperationId = "CierreMensualEndpoint.ConsultarProgramacionCierreMensual",
            Tags = new[] { "CierreMensualEndpoint" })]

        public override async Task<ActionResult<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._cierreMensualBusinessLogic.ConsultarProgramacionCierreMensual();
        }
    }
}
