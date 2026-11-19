using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class ConsultarConceptosCongeladosCalculados : BaseAsyncEndpoint.WithRequest<ConceptosCongeladosProspeccion>.WithResponse<Response<InformacionConceptosCongelados>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar recursos congelados
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarConceptosCongeladosCalculados(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/ConsultarConceptosCongeladosCalculados")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InformacionConceptosCongelados>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Consulta lineas de credito del afiliado por numero de documento",
         Description = "Consulta lineas de credito del afiliado por numero de documento",
         OperationId = "prospeccion.ConsultarConceptosCongeladosCalculados",
         Tags = new[] { "ProspeccionEndpoint" })]
        public override async System.Threading.Tasks.Task<ActionResult<Response<InformacionConceptosCongelados>>> HandleAsync(ConceptosCongeladosProspeccion request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarRecursosCongeladosCredito(request); 
        }
    }
}
