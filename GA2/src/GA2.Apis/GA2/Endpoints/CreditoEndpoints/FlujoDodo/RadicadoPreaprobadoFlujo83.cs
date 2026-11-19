using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Core.Reportes.DiseñoReportes.TransUnion;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.FlujoDodo
{
    public class RadicadoPreaprobadoFlujo83 : BaseAsyncEndpoint.WithRequest<DatosIdentificacionDto>
        .WithResponse<Response<ResponseFlujo83>>
    { 
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint que crea el flujo 83 de DODO
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public RadicadoPreaprobadoFlujo83(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Crea el flujo 83 de DODO
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/RadicadoPreaprobadoFlujo83")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ResponseFlujo83>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crea el flujo 83 de DODO",
           Description = "Crea el flujo 83 de DODO",
           OperationId = "Credito.RadicadoPreaprobadoFlujo83",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ResponseFlujo83>>> HandleAsync(DatosIdentificacionDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.CrearRadicadoPreAprobadoFlujo83(request); 
        }
    }
}
