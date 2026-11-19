using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class ConsultarDatosProspeccionAfiliado : BaseAsyncEndpoint.WithRequest<IdentificacionProspeccion>.WithResponse<Response<DatosProspeccionEndudamiento>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar prospeccion
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarDatosProspeccionAfiliado(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }


        [HttpPost("api/prospeccion/ConsultarDatosProspeccionAfiliado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DatosProspeccionEndudamiento>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta valores de endudamiento del afiliado por numero de documento",
         Description = "Consulta valores de endudamiento del afiliado por numero de documento",
         OperationId = "prospeccion.ConsultarDatosProspeccionAfiliado",
         Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<DatosProspeccionEndudamiento>>> HandleAsync(IdentificacionProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarPropeccionPorNumeroDocumento(request); 
        }
    }
}
