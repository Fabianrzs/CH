using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Entidades;

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    public class GetDataTramiteEntidades: BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<TramiteEntidadesDto>>>
    {
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;

    public GetDataTramiteEntidades(IEntidadesBusinessLogic entidadesBusinessLogic)
    {
        _entidadesBusinessLogic = entidadesBusinessLogic;
    }

    [HttpGet("api/Entidades/GetDataTramiteEntidades")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Response<TramiteEntidadesDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(
       Summary = "Consultar archivos por entidade",
       Description = "Consultar archivos por entidad",
       OperationId = "Entidades.ConsultarArchivosPorEntidad",
       Tags = new[] { "EntidadesEndpoint" })]
    public async override Task<ActionResult<Response<IEnumerable<TramiteEntidadesDto>>>> HandleAsync(Guid IdEntidad, CancellationToken cancellationToken = default)
    {
        return await this._entidadesBusinessLogic.ConsultarDataTramiteEntidades(IdEntidad);
    }
}
}
