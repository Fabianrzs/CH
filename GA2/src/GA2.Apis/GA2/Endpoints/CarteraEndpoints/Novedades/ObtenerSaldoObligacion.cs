using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Novedades;
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

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Novedades
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ObtenerSaldoObligacion : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<SaldoObligacionDto>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerSaldoObligacion(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Novedades/ObtenerSaldoObligacion")]
        [ProducesResponseType(typeof(Response<SaldoObligacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener el saldo de la obligacion para realizar la novedad del credito",
           Description = "Obtener el saldo de la obligacion para realizar la novedad del credito",
           OperationId = "credito.ObtenerSaldoObligacion",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<SaldoObligacionDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerSaldoObligacion(request);
        }
    }
}
