using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Application.Dto.Convocatoria;
using GA2.Domain.Core.SolicitudConvocatoria;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class ReportePostulanteCalificadoEndpPoint : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<IEnumerable<PostulanteCalificadoFileDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _reportesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePostulanteCalificadoEndpPoint(IConvocatoriaBussigneLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteGeneralCartera
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/convocatoria/ReportePostulanteCalificado")]
        [ProducesResponseType(typeof(Response<IEnumerable<PostulanteCalificadoFileDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar resporte de postulante calificado",
            Description = "Generar reporte postulante calificado ",
            OperationId = "convocatoria.ReportePostulanteCalificado",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteCalificadoFileDto>>>> HandleAsync(int idConvocatoria, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GetReportePostulanteCalificado(idConvocatoria);
        }
    }
}
