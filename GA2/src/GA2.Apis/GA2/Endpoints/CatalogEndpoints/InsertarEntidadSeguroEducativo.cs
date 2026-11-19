using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para insertar Entidad Seguro Educativo
    /// </summary>
    [Authorize]
    public class InsertarEntidadSeguroEducativo : BaseAsyncEndpoint
        .WithRequest<EntidadSeguroEducativoDto>
        .WithResponse<Response<EntidadSeguroEducativoDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;

        public InsertarEntidadSeguroEducativo(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }

        [HttpPost("api/catalogos/InsertarEntidadSeguroEducativo")]
        [ProducesResponseType(typeof(Response<EntidadSeguroEducativoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Inserta entidad de seguro educativo",
            Description = "Inserta entidad de seguro educativo (valida NIT no duplicado)",
            OperationId = "catalogos.InsertarEntidadSeguroEducativo",
            Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<EntidadSeguroEducativoDto>>> HandleAsync(EntidadSeguroEducativoDto request, CancellationToken cancellationToken = default)
        {
            return await _catalogsBusinessLogic.InsertarEntidadSeguroEducativo(request);
        }
    }
}
