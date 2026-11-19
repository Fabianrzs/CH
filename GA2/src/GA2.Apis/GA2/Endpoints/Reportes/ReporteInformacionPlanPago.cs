using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    public class ReporteInformacionPlanPago : BaseAsyncEndpoint
        .WithRequest<RequestProductoClienteDto>
        .WithResponse<Response<string>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteInformacionPlanPago
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteInformacionPlanPago(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteInformacionPlanPago")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte informacion plan pago",
            Description = "Generar Reporte Reporte informacion plan pago",
            OperationId = "Reportes.ReporteInformacionPlanPago",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<string>>> HandleAsync([FromBody] RequestProductoClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarPlanPagosHipotecarioPdfBase64Async(request);
        }
    }
}
