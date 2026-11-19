using Ardalis.ApiEndpoints;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Core.ValoresCategoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.ValoresCategoriaEndpoints
{
    public class CrearValoresCategoria : BaseAsyncEndpoint.WithRequest<CrearValoresDto>.WithResponse<Response<IEnumerable<ValoresCategoriaDto>>>
    {
        private readonly IValoresCategoriaBusinessLogic _iValoresCategoriaBusinessLogic;

        public CrearValoresCategoria(IValoresCategoriaBusinessLogic iValoresCategoriaBusinessLogic)
        {
            _iValoresCategoriaBusinessLogic = iValoresCategoriaBusinessLogic;
        }

        [HttpPost("api/Categoria/CrearValoresCategoria")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ValoresCategoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear Valores Categoria",
            Description = "Genera Valores Categoria para Nuevo Año",
            OperationId = "categoria.CrearValoresCategoria",
            Tags = new[] { "CategoriaEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ValoresCategoriaDto>>>> HandleAsync(CrearValoresDto RequestValores, CancellationToken cancellationToken = default)
        {
            return await this._iValoresCategoriaBusinessLogic.CrearValoresCategoria(RequestValores);
        }
    }
}
