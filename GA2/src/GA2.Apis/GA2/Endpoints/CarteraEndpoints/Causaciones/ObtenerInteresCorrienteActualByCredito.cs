using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Causaciones
{
    public class ObtenerInteresCorrienteActualByCredito :
         BaseAsyncEndpoint.WithRequest<ObtenerCausacionCorrienteDto>
        .WithResponse<Response<List<CausacionSaldoActualDto>>>
    {

        private readonly ICreditoBusinessLogic _creditoSolicitudBusinessLogic;

        /// <summary>
        /// Constructor de ObtenerInteresCorrienteActualByCredito
        /// </summary>
        /// <param name="ObtenerInteresCorrienteActualByCredito"></param>
        /// <author>Alejandro Lara</author>
        /// <date>07/01/2026</date>
        public ObtenerInteresCorrienteActualByCredito(ICreditoBusinessLogic creditoSolicitudBusinessLogic)
        {
            _creditoSolicitudBusinessLogic = creditoSolicitudBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cartera/ObtenerInteresCorrienteActualByCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CausacionSaldoActualDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Interes Corriente Actual por Credito",
           Description = "Obtener Interes Corriente Actual por Credito",
           OperationId = "Credito.ObtenerInteresCorrienteActualByCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<List<CausacionSaldoActualDto>>>> HandleAsync(ObtenerCausacionCorrienteDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoSolicitudBusinessLogic.ObtenerSaldoActualAsync(request.NumeroCredito,request.TipoCredito);
        }
    }
}
