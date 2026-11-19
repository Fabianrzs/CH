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
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarSegurosCredito : BaseAsyncEndpoint.WithRequest<ConsultaSegurosCreditoDto>.WithResponse<Response<IEnumerable<ConsultaSegurosCredito>>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar seguros
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarSegurosCredito(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarSegurosCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ConsultaSegurosCredito>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Registrar seguros de solicitud de credito",
         Description = "Registrar seguros de solicitud de credito",
         OperationId = "prospeccion.ConsultarSegurosCredito",
         Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultaSegurosCredito>>>> HandleAsync(ConsultaSegurosCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarSegurosSolicitudCredito(request); 
        }
    }
}
