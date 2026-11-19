using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarRecursosActualesCredito : BaseAsyncEndpoint.WithRequest<IdentificacionProspeccion>.WithResponse<Response<InformacionCongelamientoRecursos>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar recursos congelados
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarRecursosActualesCredito(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarRecursosActualesCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InformacionCongelamientoRecursos>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta recursos a la fecha del afiliado",
         Description = "Consulta recursos a la fecha del afiliado",
         OperationId = "prospeccion.ConsultarRecursosActualesCredito",
         Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<InformacionCongelamientoRecursos>>> HandleAsync(IdentificacionProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.RecursosActualesAfiliadoCredito(request); 
        }
    }
}
