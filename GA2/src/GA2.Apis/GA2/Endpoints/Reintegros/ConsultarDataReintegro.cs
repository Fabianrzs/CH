using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Application.Dto.Reintegros;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class ConsultarDataReintegro : BaseAsyncEndpoint
        .WithRequest<AfiliadoConsulta>
        .WithResponse<Response<IEnumerable<ReintegroResponseDTO>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ConsultarDataReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="idTipo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/GetDataReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ReintegroResponseDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener la data de  Reintegro",
           Description = "Obtener la data de  Reintegro",
           OperationId = "Reintegros.GetDataReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReintegroResponseDTO>>>> HandleAsync([FromQuery] AfiliadoConsulta idTipo, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.GetDataReintegro(idTipo);
        }
    }
}
