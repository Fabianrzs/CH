using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class ConsultarValoresProspeccion : BaseAsyncEndpoint.WithRequest<IdentificacionProspeccion>.WithResponse<Response<ValoresProspeccion>>
    {


        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar prospeccion
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarValoresProspeccion(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarValoresProspeccion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InformacionProspeccion>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Consulta la prospeccion del afiliado por numero de documento",
          Description = "Consulta la prospeccion del afiliado por numero de documento",
          OperationId = "prospeccion.ConsultarValoresProspeccion",
          Tags = new[] { "ProspeccionEndpoint" })]
        public override async System.Threading.Tasks.Task<ActionResult<Response<ValoresProspeccion>>> HandleAsync(IdentificacionProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarValoresPropeccionPorNumeroDocumento(request); 
        }
    }
}
