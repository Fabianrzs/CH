using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.Reportes
{
    [Authorize]
    public class ReporteEntidades : BaseAsyncEndpoint
    
        .WithRequest<EntidadEducativaDto>
        .WithResponse<Response<FileResult>>
    { 
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteEntidades(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteEntidades")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Entidad",
            Description = "Generar Reporte Entidad",
            OperationId = "Reportes.ReporteExtractoMensual",
            Tags = new[] { "ReportesEndpoint" })]


        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(EntidadEducativaDto request, CancellationToken cancellationToken = default)
        {
          return await _reportesBusinessLogic.ReporteEntidades(request);
  
        }
    }

}
