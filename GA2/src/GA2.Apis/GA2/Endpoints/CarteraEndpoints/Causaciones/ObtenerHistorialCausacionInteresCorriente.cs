using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Causaciones
{
    public class ObtenerHistorialCausacionInteresCorriente :
         BaseAsyncEndpoint.WithRequest<ObtenerCausacionCorrienteHistoricoDto>
        .WithResponse<Response<List<CausacionHistoricoDto>>>
    {

        private readonly ICreditoBusinessLogic _creditoSolicitudBusinessLogic;

        /// <summary>
        /// Constructor de ObtenerInteresCorrienteActualByCredito
        /// </summary>
        /// <param name="ObtenerInteresCorrienteActualByCredito"></param>
        /// <author>Alejandro Lara</author>
        /// <date>07/01/2026</date>
        public ObtenerHistorialCausacionInteresCorriente(ICreditoBusinessLogic creditoSolicitudBusinessLogic)
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
        [HttpPost("api/cartera/ObtenerHistorialCausacionInteresCorriente")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CausacionSaldoActualDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Historico Interes Corriente Actual por Credito",
           Description = "Obtener Historico Interes Corriente Actual por Credito",
           OperationId = "Credito.ObtenerHistorialCausacionInteresCorriente",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<List<CausacionHistoricoDto>>>> HandleAsync(ObtenerCausacionCorrienteHistoricoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoSolicitudBusinessLogic.ObtenerHistorialCausacionInteresCorrienteAsync(request.NumeroCredito, request.TipoCredito,request.FechaInicial,request.FechaFinal);
        }
    }
}
