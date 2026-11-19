using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.TU;
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
    public class ReporteSolicitudTransUnion : BaseAsyncEndpoint

        .WithRequest<ProspeccionTransUnionRequestDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteSolicitudTransUnion
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteSolicitudTransUnion(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteSolicitudTransUnion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteSolicitudTransUnion")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte TransUnion",
            Description = "Generar Reporte TransUnion",
            OperationId = "Reportes.ReporteSolicitudTransUnion",
            Tags = new[] { "ReportesEndpoint" })]


        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ProspeccionTransUnionRequestDto xml, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteSolicitudTransUnion(xml);

        }
    }

}
