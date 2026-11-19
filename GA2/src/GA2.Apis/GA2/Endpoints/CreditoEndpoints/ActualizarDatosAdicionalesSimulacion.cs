using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
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
    public class ActualizarDatosAdicionalesSimulacion : BaseAsyncEndpoint.WithRequest<DatosAdicionalesSimulacionDto>.WithResponse<Response<DatosAdicionalesSimulacion>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint actualizar datos de la simulacion
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ActualizarDatosAdicionalesSimulacion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// actualizar datos de la simulacion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/ActualizarDatosAdicionalesSimulacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DatosAdicionalesSimulacion>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar datos de la simulacion",
           Description = "Actualizar datos de la simulacion",
           OperationId = "Credito.ActualizarDatosAdicionalesSimulacion",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<DatosAdicionalesSimulacion>>> HandleAsync(DatosAdicionalesSimulacionDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ActualizarDatosAdicionalesSimulacion(request); 
        }
    }
}
