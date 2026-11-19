using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    [Authorize]

    public class ObtenerEmbargosRangoFiltros : BaseAsyncEndpoint.WithRequest<ConsultaEmbargosfiltroDto>.WithResponse<Response<IEnumerable<EmbargosDto>>>
    {
        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerEmbargosRangoFiltros(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpPost("api/ObtenerEmbargosRangoFiltros")]
        [Consumes("application/json")]
        [SwaggerOperation(
       Summary = "Obtiene lista de embargos por filtros",
          Description = "Obtiene lista de embargos por filtros",
          OperationId = "Embargos.ObtenerEmbargosRangoFiltros",
          Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<IEnumerable<EmbargosDto>>>> HandleAsync(ConsultaEmbargosfiltroDto request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ObtenerEmbargosRangoFiltros(request) ;
        }
    }
}
