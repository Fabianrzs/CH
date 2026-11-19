using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Reportes;
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

namespace GA2.Apis.GA2.Endpoints.Reportes.InformacionContable
{
    /// <summary>
    /// Obtener Equivalencia Cuentas Contables
    /// </summary>
    [Authorize]
    public class EquivalenciaCuentasContables : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<EquivalenciaCuentasContableSevenDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ObtenerEquivalenciaCuentasContables
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public EquivalenciaCuentasContables(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ObtenerEquivalenciaCuentasContables
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ObtenerEquivalenciaCuentasContables")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Equivalencia Cuentas Contables",
            Description = "Obtener Equivalencia Cuentas Contables",
            OperationId = "Reportes.ObtenerEquivalenciaCuentasContables",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<EquivalenciaCuentasContableSevenDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ObtenerEquivalenciaCuentasContables();
        }
    }
}
