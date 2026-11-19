using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Core.ValoresCategoria;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.ValoresCategoriaEndpoints
{
    public class ObtenerValoresCategoria : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<IEnumerable<ValoresCategoriaDto>>>
    {
        private readonly IValoresCategoriaBusinessLogic _iValoresCategoriaBusinessLogic;

        public ObtenerValoresCategoria(IValoresCategoriaBusinessLogic iValoresCategoriaBusinessLogic)
        {
            _iValoresCategoriaBusinessLogic = iValoresCategoriaBusinessLogic;
        }

        [HttpPost("api/Categoria/ObtenerValoresCategoria")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ValoresCategoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Valores Categoria",
            Description = "Obtener Valores Categoria",
            OperationId = "categoria.ObtenerValoresCategoria",
            Tags = new[] { "CategoriaEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ValoresCategoriaDto>>>> HandleAsync(int Anio, CancellationToken cancellationToken = default)
        {
            return await this._iValoresCategoriaBusinessLogic.ObtenerValoresCategoria(Anio);
        }
    }
}
