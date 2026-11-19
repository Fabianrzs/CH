using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class InsertaCompraCarteraCredito :
         BaseAsyncEndpoint.WithRequest<CompraCarteraCreditoDto>
        .WithResponse<Response<CompraCarteraCredito>>
    {

        private readonly ICreditoBusinessLogic _creditoSolicitudBusinessLogic;

        /// <summary>
        /// Constructor de InsertaCompraCarteraCredito
        /// </summary>
        /// <param name="estadosolicitudBusinessLogic"></param>
        /// <author>Alejandro Lara</author>
        /// <date>07/01/2026</date>
        public InsertaCompraCarteraCredito(ICreditoBusinessLogic creditoSolicitudBusinessLogic)
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
        [HttpPost("api/credito/InsertaCompraCarteraCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CompraCarteraCredito>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta Compra Cartera Credito",
           Description = "Inserta Compra Cartera Credito",
           OperationId = "Credito.InsertaCompraCarteraCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<CompraCarteraCredito>>> HandleAsync(CompraCarteraCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoSolicitudBusinessLogic.InsertarValoresCompraCarteraCredito(request); 
        }
    }
}
