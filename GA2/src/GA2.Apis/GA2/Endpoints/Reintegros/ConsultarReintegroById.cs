
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class ConsultarReintegroById : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<ReintefroDto>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

    /// <summary>
    /// ObtenerTiposReintegroById
    /// </summary>
    /// <param name="reintegrosBusinessLogic"></param>
    public ConsultarReintegroById(IReintegrosBusinessLogic reintegrosBusinessLogic)
    {
        _reintegrosBusinessLogic = reintegrosBusinessLogic;
    }

    /// <summary>
    /// ObtenerTiposReintegroById
    /// </summary>
    /// <param name="idTipo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("api/Reintegros/ConsultarReintegroById")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Response<ReintefroDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(
       Summary = "Obtener la data de  Reintegro by Id",
       Description = "Obtener la data de  Reintegro by Id",
       OperationId = "Reintegros.ConsultarReintegroById",
       Tags = new[] { "ReintegrosEndpoint" })]
    public override async Task<ActionResult<Response<ReintefroDto>>> HandleAsync(Guid idTipo, CancellationToken cancellationToken = default)
    {
        return await _reintegrosBusinessLogic.GetDataReintregoByID(idTipo);
    }
}
}
