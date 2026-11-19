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
    [Authorize]

    public class ObtenerJuzgadosPorDepartamentoCiudadCodigo : BaseAsyncEndpoint
        .WithRequest<JuzgadoDptoCiudadDto>.WithResponse<Response<IEnumerable<JuzgadosDto>>>
    {

        private readonly IEmbargosBusinessLogic _juzgadosBusinessLogic;

        public ObtenerJuzgadosPorDepartamentoCiudadCodigo(IEmbargosBusinessLogic juzgadosBusinessLogic)
        {
            _juzgadosBusinessLogic = juzgadosBusinessLogic;
        }


        /// <summary>
        /// ObtenerJuzgadosPorCiudad
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Embargos/ObtenerJuzgadosPorDepartamentoCiudadCodigo")]
        [ProducesResponseType(typeof(Response<IEnumerable<JuzgadosDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene todas los juzgados por departamento ciudad y codigo",
           Description = "Obtiene todas los juzgados por departamento ciudad y codigo",
           OperationId = "Embargos.ObtenerJuzgadosPorDepartamentoCiudadCodigo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<IEnumerable<JuzgadosDto>>>> HandleAsync(JuzgadoDptoCiudadDto request, CancellationToken cancellationToken = default)
        {
            return await _juzgadosBusinessLogic.ObtenerJuzgadosPorDepartamentoCiudadCodigo(request);
        }
    }
}
