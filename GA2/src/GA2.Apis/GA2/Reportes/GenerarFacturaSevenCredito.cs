using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Reportes
{
    public class GenerarFacturaSevenCredito : BaseAsyncEndpoint

        .WithRequest<Guid>
        .WithResponse<Response<FileResult>>
    {


        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// Generar factura seven credito
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public GenerarFacturaSevenCredito(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// Generar factura seven credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/GenerarFacturaSevenCredito")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar factura seven credito",
            Description = "Generar factura seven credito",
            OperationId = "Reportes.GenerarFacturaSevenCredito",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.FacturaSevenCredito(request); 
        }
    }
}
