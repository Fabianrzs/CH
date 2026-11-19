using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    ///
    [Authorize]
    public class ActualizarProcesoAplicacionPagos : BaseAsyncEndpoint.WithRequest<ProcesoAplicacionPagosMasivoDto>.WithResponse<Response<ProcesoAplicacionPagosMasivoDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ActualizarProcesoAplicacionPagos(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// 
        [HttpPost("api/credito/ActualizarProcesoAplicacionPagos")]
        [ProducesResponseType(typeof(Response<ProcesoAplicacionPagosMasivoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar proceso aplicación pagos masivo",
            Description = "Actualizar proceso aplicación pagos masivo",
            OperationId = "credito.ActualizarProcesoAplicacionPagos",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<ProcesoAplicacionPagosMasivoDto>>> HandleAsync(ProcesoAplicacionPagosMasivoDto request, CancellationToken cancellationToken = default)
        {
            request.ModificadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await _carteraBusinessLogic.ActualizarProcesoAplicacionPagos(request);
        }
    }
}
