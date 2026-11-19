using Ardalis.ApiEndpoints;
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
    public class ConsultarPeritoDetalle : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<PeritoDetalle>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar perito
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ConsultarPeritoDetalle(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpGet("api/prospeccion/ConsultarPeritoDetalle")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PeritoDetalle>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Consultar perito",
          Description = "Consultar perito",
          OperationId = "prospeccion.ConsultarPeritoDetalle",
          Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<PeritoDetalle>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarPeritoDetalle(request); 
        }
    }
}
