using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;


namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class ConsultarProspeccionPorNumeroDocumento : BaseAsyncEndpoint.WithRequest<IdentificacionProspeccion>.WithResponse<Response<InformacionProspeccion>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar prospeccion
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarProspeccionPorNumeroDocumento(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarProspeccionPorNumeroDocumento")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SimulacionClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Consulta la prospeccion del afiliado por numero de documento",
          Description = "Consulta la prospeccion del afiliado por numero de documento",
          OperationId = "prospeccion.ConsultarProspeccionPorNumeroDocumento",
          Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<InformacionProspeccion>>> HandleAsync(IdentificacionProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarInformacionPropeccionPorNumeroDocumento(request); 
        }
    }
}
