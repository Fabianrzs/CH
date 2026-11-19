using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
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
    public class ReporteTransUnionPropiedades : BaseAsyncEndpoint
        .WithRequest<DatosPersonalesProspeccionDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        public ReporteTransUnionPropiedades(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        [HttpPost("api/Reportes/ReporteTransUnionPropiedades")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [SwaggerOperation(
            Summary = "Generar Propiedades ReporteTransUnion",
            Description = "Generar Propiedades ReporteTransUnion",
            OperationId = "Reportes.ReporteTransUnionPropiedades",
            Tags = new[] { "ReportesEndpoint" })]        
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(DatosPersonalesProspeccionDto request, CancellationToken cancellationToken = default)
        {
               return await _reportesBusinessLogic.ReporteTransUnionPropiedadesInformacionComercial(request);
        }
    }
}
