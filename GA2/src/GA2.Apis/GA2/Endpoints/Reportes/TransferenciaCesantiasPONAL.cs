using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteTransferenciaCesantiasPONAL
    /// </summary>
    [Authorize]
    public class TransferenciaCesantiasPONAL : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteTransferenciaCesantiasPONAL
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public TransferenciaCesantiasPONAL(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/TransferenciaCesantiasPONAL")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Transferencia Cesantias PONAL",
            Description = "Generar Reporte Transferencia Cesantias PONAL",
            OperationId = "Reportes.TransferenciaCesantiasPONAL",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(int idAfiliacion, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.TransferenciaCesantiasPONAL(idAfiliacion);
        }
    }
}
