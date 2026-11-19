
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class GetDataAfiliado: BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<IEnumerable<AfiliadosReintegrosDto>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

    /// <summary>
    /// ObtenerTiposReintegroById
    /// </summary>
    /// <param name="reintegrosBusinessLogic"></param>
    public GetDataAfiliado(IReintegrosBusinessLogic reintegrosBusinessLogic)
    {
        _reintegrosBusinessLogic = reintegrosBusinessLogic;
    }

    /// <summary>
    /// ObtenerTiposReintegroById
    /// </summary>
    /// <param name="idTipo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("api/Reintegros/GetAfiliadoReintegro")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Response<AfiliadosReintegrosDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(
       Summary = "Obtener la data de  afiliados by Id",
       Description = "Obtener la data de  afiliados by Id",
       OperationId = "Reintegros.GetAfiliadoReintegro",
       Tags = new[] { "ReintegrosEndpoint" })]
    public override async Task<ActionResult<Response<IEnumerable<AfiliadosReintegrosDto>>>> HandleAsync(Guid idTipo, CancellationToken cancellationToken = default)
    {
        return await _reintegrosBusinessLogic.GetAfiliadoReintegro(idTipo);
    }
}
}
