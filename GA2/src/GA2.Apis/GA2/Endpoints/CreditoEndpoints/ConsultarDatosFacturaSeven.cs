using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities.Reportes;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarDatosFacturaSeven
     :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<FacturaSevenCredito>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint consultar datos factura
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>
        public ConsultarDatosFacturaSeven(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ConsultarDatosFacturaSeven")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TipoActividadDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Datos factura seven",
           Description = "Datos factura seven",
           OperationId = "credito.ConsultarDatosFacturaSeven",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<FacturaSevenCredito>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarDatosFactura(request); 
        }
    }
}
