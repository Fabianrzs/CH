using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using System.Collections.Generic;
using System;
using GA2.Domain.Entities.Solicitud;
using System.Threading;
using GA2.Transversals.Commons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using GA2.Application.Dto.Solicitud;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    public class GenerarReportePagoDeCesantis
     : BaseAsyncEndpoint
         .WithRequest<ReporteAfiliadosCesantiasRequestDto>
         .WithResponse<Response<ReporteAfiliadosCesantiasResponse>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar los conceptos afectados
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="solicitudBusinessLogic"></param>
        /// <date>11/03/2024</date>
        public GenerarReportePagoDeCesantis(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        [SwaggerOperation(
            Summary = "Genera reporte pagos cesantías",
            Description = "Retorna lista de afiliados con información enriquecida",
            OperationId = "Client.GenerarReportePagosCesantis",
            Tags = new[] { "ClientEndpoint" })]
        [HttpPost("api/reporte/GenerarReportePagoDeCesantias")]
        public override async Task<ActionResult<Response<ReporteAfiliadosCesantiasResponse>>> HandleAsync(
            ReporteAfiliadosCesantiasRequestDto request,
            CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ReporteDeCesantias(request);
        }
    }

}
