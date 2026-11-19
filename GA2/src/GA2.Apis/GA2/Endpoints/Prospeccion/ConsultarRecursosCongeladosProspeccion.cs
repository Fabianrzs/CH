using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class ConsultarRecursosCongeladosProspeccion : BaseAsyncEndpoint.WithRequest<ConceptosCongeladosProspeccion>.WithResponse<Response<InformacionConceptosCongelados>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar recursos congelados
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarRecursosCongeladosProspeccion(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarRecursosCongeladosProspeccion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InformacionConceptosCongelados>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta lineas de credito del afiliado por numero de documento",
         Description = "Consulta lineas de credito del afiliado por numero de documento",
         OperationId = "prospeccion.ConsultarRecursosCongeladosProspeccion",
         Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<InformacionConceptosCongelados>>> HandleAsync(ConceptosCongeladosProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarCongelamientoRecursos(request);
        }
    }
}
