using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarMediosCreditosVivienda : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<MediosCreditoVivienda>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint consultar puntos de referencias
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ConsultarMediosCreditosVivienda(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Mostrar los puntos de referencias
        /// </summary>
        /// <param name="cancellationToken"></param>                
        [HttpGet("api/credito/ConsultarMediosCreditosVivienda")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Mostrar los puntos de referencias",
           Description = "Mostrar los puntos de referencias",
           OperationId = "Credito.ConsultarMediosCreditosVivienda",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<MediosCreditoVivienda>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarMediosCreditosVivienda(); 
        }
    }
}
