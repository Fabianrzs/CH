
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes.MovimientoComprobante
{
    /// <summary>
    /// ReporteExcelCausaciones
    /// </summary>
    [Authorize]
    public class GenerarExcelComprobanteDetalle : BaseAsyncEndpoint

        .WithRequest<Guid>
        .WithResponse<Response<FileResult>>
    {
        private readonly ICarteraBusinessLogic _carterabusinesslogic;

        /// <summary>
        /// ReporteExcelCausaciones
        /// </summary>
        /// <param name="carterabusinesslogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public GenerarExcelComprobanteDetalle(ICarteraBusinessLogic carterabusinesslogic)
        {
            _carterabusinesslogic = carterabusinesslogic;
        }

        /// <summary>
        /// GenerarExcelComprobanteDetalle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/cartera/GenerarExcelComprobanteDetalle")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Excel Comprobante Detalle",
            Description = "Generar Excel Comprobante Detalle",
            OperationId = "cartera.GenerarExcelComprobanteDetalle",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(Guid request, CancellationToken cancellationToken )
        {
            return await _carterabusinesslogic.GenerarExcelComprobanteDetalle(request, cancellationToken);
        }
    }
}
