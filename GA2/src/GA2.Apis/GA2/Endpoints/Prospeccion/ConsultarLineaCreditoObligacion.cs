using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class ConsultarLineaCreditoObligacion : BaseAsyncEndpoint.WithRequest<IdentificacionProspeccion>.WithResponse<Response<IEnumerable<LineaCreditoObligacion>>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar prospeccion
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarLineaCreditoObligacion(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarLineaCreditoObligacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<LineaCreditoObligacion>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta lineas de credito del afiliado por numero de documento",
         Description = "Consulta lineas de credito del afiliado por numero de documento",
         OperationId = "prospeccion.ConsultarLineaCreditoObligacion",
         Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<LineaCreditoObligacion>>>> HandleAsync(IdentificacionProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarLineasCreditoObligacion(request); 
        }
    }
}
