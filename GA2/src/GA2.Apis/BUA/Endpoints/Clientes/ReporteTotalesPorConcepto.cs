using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using GA2.Application.Dto.BUA;

namespace GA2.Apis.BUA.Endpoints.Reportes
{
    public class GenerarReporteTotalesPorConcepto
        : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ReporteTotalesDto>>>
    {
        private readonly IClientesBusinessLogic _clientesBusinessLogic;

        public GenerarReporteTotalesPorConcepto(IClientesBusinessLogic clientesBusinessLogic)
        {
            _clientesBusinessLogic = clientesBusinessLogic;
        }

        [HttpGet("api/reportes/GenerarReporteTotalesPorConcepto")]
        [SwaggerOperation(
            Summary = "Generar reporte de totales por concepto",
            Description = "Obtiene los totales de conceptos agrupados por unidad ejecutora y tipo de cuenta",
            OperationId = "Reportes.GenerarReporteTotalesPorConcepto",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteTotalesDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _clientesBusinessLogic.GenerarReporteTotalesPorConcepto();
        }
    }
}
