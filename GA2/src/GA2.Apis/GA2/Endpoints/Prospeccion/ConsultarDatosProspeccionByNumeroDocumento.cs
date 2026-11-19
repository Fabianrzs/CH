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

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class ConsultarDatosProspeccionByNumeroDocumento : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<DatosEndeudamientoProspeccion>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar prospeccion
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarDatosProspeccionByNumeroDocumento(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpGet("api/prospeccion/ConsultarDatosProspeccionByNumeroDocumento")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DatosEndeudamientoProspeccion>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta lineas de credito del afiliado por numero de documento",
         Description = "Consulta lineas de credito del afiliado por numero de documento",
         OperationId = "prospeccion.ConsultarDatosProspeccionByNumeroDocumento",
         Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<DatosEndeudamientoProspeccion>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarEndeudamientoProspeccion(request); 
        }
    }
}
