using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteCierreMensualCuentas
    /// </summary>
    [Authorize]
    public class ReporteCierreMensualCuentas : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<IEnumerable<ReporteCierreMensualCuentasDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICierreMensualCuentasBusinessLogic _cierreMensualBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cierreMensualBusinessLogic"></param>
        public ReporteCierreMensualCuentas(ICierreMensualCuentasBusinessLogic cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }


        [HttpGet("api/ReporteCierreMensual")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ReporteCierreMensualCuentasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Cierre Mensual",
            Description = "Generar Remuneracion de Intereses",
            OperationId = "CierreMensual.ReporteCierreMensual",
            Tags = new[] { "CierreMensualEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteCierreMensualCuentasDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._cierreMensualBusinessLogic.ReporteCierreMensual(request);
        }
    }
}
