using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class RestaurarSimulacionPlanCliente : BaseAsyncEndpoint.WithRequest<DatosSimulacionUsuarioDto>.WithResponse<Response<SimulacionPlanClienteDetalle>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar prospeccion por afiliado y afl_id
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public RestaurarSimulacionPlanCliente(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }


        [HttpPost("api/prospeccion/RestaurarSimulacionPlanCliente")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SimulacionPlanClienteDetalle>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta lineas de credito del afiliado por numero de documento",
         Description = "Consulta lineas de credito del afiliado por numero de documento",
         OperationId = "prospeccion.RestaurarSimulacionPlanCliente",
         Tags = new[] { "ProspeccionEndpoint" })]
        public async override System.Threading.Tasks.Task<ActionResult<Response<SimulacionPlanClienteDetalle>>> HandleAsync(DatosSimulacionUsuarioDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.RestaurarSimulacionCliente(request); 
        }
    }
}
