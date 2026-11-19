using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Prospeccion;
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
    public class ReporteProspeccionReclasificacion : BaseAsyncEndpoint

        .WithRequest<IdentificacionProspeccion>
        .WithResponse<Response<FileResult>>
    {


        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// Reporte de reclasificacion en prospección
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteProspeccionReclasificacion(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// Reporte de reclasificacion en prospección
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteProspeccionReclasificacion")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte reclasificacion en prospección",
            Description = "Generar Reporte reclasificacion en prospección",
            OperationId = "Reportes.ReporteProspeccionReclasificacion",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<FileResult>>> HandleAsync(IdentificacionProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteProspeccionReclasificacion(request); 
        }
    }
}
