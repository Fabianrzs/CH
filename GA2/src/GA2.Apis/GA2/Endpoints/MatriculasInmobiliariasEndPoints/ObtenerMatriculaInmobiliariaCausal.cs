using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.MatriculasInmobiliarias;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.MatriculasInmobiliariasEndPoints
{
    [Authorize]

    public class ObtenerMatriculaInmobiliariaCausal : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<CausalMatriculaInmobiliariaDto>>>
    {
        private readonly IMatriculasInmobiliariasLogic _matriculasInmobiliariasLogic;

        public ObtenerMatriculaInmobiliariaCausal(IMatriculasInmobiliariasLogic matriculasInmobiliariasLogic)
        {
            _matriculasInmobiliariasLogic = matriculasInmobiliariasLogic;
        }


        [HttpGet("api/ObtenerMatriculaInmobiliariaCausal")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<CausalMatriculaInmobiliariaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                  Summary = "Listar operaciones matriculas inmobiliarias",
                  Description = "Listar operaciones matriculas inmobiliarias",
                  OperationId = "matriculas.listarMatriculas",
                  Tags = new[] { "MatriculasEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<CausalMatriculaInmobiliariaDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return await _matriculasInmobiliariasLogic.ObtenerMatriculaInmobiliariaCausal();

        }
    }
}
