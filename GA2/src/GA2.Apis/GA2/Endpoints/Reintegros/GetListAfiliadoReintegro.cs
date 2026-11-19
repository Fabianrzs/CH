using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Reintegros;
using System.Collections.Generic;
using System;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class GetListAfiliadoReintegro : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<IEnumerable<AfiliadoReintegroListDto>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public GetListAfiliadoReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="idReintegro"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/GetListAfiliado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<AfiliadoReintegroListDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener la lista de afiliado de reintegro",
           Description = "Obtener la lista de afiliado de reintegro",
           OperationId = "Reintegros.GetListAfiliado",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<AfiliadoReintegroListDto>>>> HandleAsync(Guid idReintegro, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.GetSaveListAfiliado(idReintegro);
        }
    }
}
