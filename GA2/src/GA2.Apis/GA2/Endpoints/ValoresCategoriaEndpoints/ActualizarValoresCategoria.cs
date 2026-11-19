using Ardalis.ApiEndpoints;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Core.ValoresCategoria;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.ValoresCategoriaEndpoints
{
    public class ActualizarValoresCategoria : BaseAsyncEndpoint.WithRequest<ValoresCategoriaDto>.WithResponse<Response<ValoresCategoriaDto>>
    {
        private readonly IValoresCategoriaBusinessLogic _iValoresCategoriaBusinessLogic;

        public ActualizarValoresCategoria(IValoresCategoriaBusinessLogic iValoresCategoriaBusinessLogic)
        {
            _iValoresCategoriaBusinessLogic = iValoresCategoriaBusinessLogic;
        }

        [HttpPost("api/Categoria/ActualizarValoresCategoria")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ValoresCategoriaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar Valores Categoria",
            Description = "Actualizar Valores Categoria",
            OperationId = "categoria.ActualizarValoresCategoria",
            Tags = new[] { "CategoriaEndpoint" })]
        public override async Task<ActionResult<Response<ValoresCategoriaDto>>> HandleAsync(ValoresCategoriaDto RequestValores, CancellationToken cancellationToken = default)
        {
            return await this._iValoresCategoriaBusinessLogic.ActualizarValoresCategoria(RequestValores);
        }
    }
}
