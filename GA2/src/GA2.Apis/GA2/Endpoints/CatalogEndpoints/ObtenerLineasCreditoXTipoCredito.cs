using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Catalogs;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    public class ObtenerLineasCreditoXTipoCredito :
        BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<IEnumerable<LineaCreditoDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;

        public ObtenerLineasCreditoXTipoCredito(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }

        [HttpGet("api/catalogos/ObtenerLineasCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<LineaCreditoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerLineasCredito",
            Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<LineaCreditoDto>>>> HandleAsync(int IdTipoCredito, CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerLineasCredito(IdTipoCredito);
        }
    }
}
