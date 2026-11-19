
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera;
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
namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes.Balance
{
    /// <summary>
    /// ReporteExcelCausaciones
    /// </summary>
    [Authorize]
    public class GenerarAuxiliarContableCartera : BaseAsyncEndpoint
        .WithRequest<BalanceCarteraDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly ICarteraBusinessLogic _carterabusinesslogic;
        /// <summary>
        /// ReporteExcelCausaciones
        /// </summary>
        /// <param name="carterabusinesslogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public GenerarAuxiliarContableCartera(ICarteraBusinessLogic carterabusinesslogic)
        {
            _carterabusinesslogic = carterabusinesslogic;
        }
        /// <summary>
        /// GenerarAuxiliarContableCartera
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/GenerarAuxiliarContableCartera")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Auxiliar Contable Cartera",
            Description = "Generar Auxiliar Contable Cartera",
            OperationId = "cartera.GenerarAuxiliarContableCartera",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(BalanceCarteraDto request, CancellationToken cancellationToken)
        {
            return await _carterabusinesslogic.GenerarAuxiliarContableCartera(request, cancellationToken);
        }
    }
}