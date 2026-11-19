using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class ReporteOficio : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<object>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// AgregarAfiliadoReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ReporteOficio(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// AfiliadoReintegroParamDto
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/ReporteOficio")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Agregar Afiliado Reintegro",
           Description = "Crear Reintegro",
           OperationId = "Reintegros.ReporteOficio",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<object>> HandleAsync(string radicado, CancellationToken cancellationToken = default)
        {
           var result =  await _reintegrosBusinessLogic.GenerarReporteOficio(radicado);

            if (result.IsSuccess)
                return result.Data;

            return BadRequest(result);
        }
    }
}
