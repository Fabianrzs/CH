using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class UpdateAfiliadoReintegro :  BaseAsyncEndpoint.WithRequest<AfiliadosReintegrosDto>
        .WithResponse<Response<AfiliadosReintegrosDto>>
    {
        private readonly IReintegrosBusinessLogic _reintegrosBusiness;

    public UpdateAfiliadoReintegro(IReintegrosBusinessLogic reintegrosBusiness)
    {
            _reintegrosBusiness = reintegrosBusiness;
    }
    /// <summary>
    /// Crear la solicitud de convocatoria
    /// </summary>
    /// <param name="convocatoriaDTO"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("api/Reintegros/UpdateAfiliadoReintegro")]
    [ProducesResponseType(typeof(Response<IEnumerable<AfiliadosReintegrosDto>>), StatusCodes.Status200OK)]
    [SwaggerOperation(
       Summary = "Update afiliado de cierre convocatoria",
       Description = "Update afiliado de cierre convocatoria",
       OperationId = "Reintegros.UpdateAfiliadoReintegro",
       Tags = new[] { "ReintegrosEndpoint" })]
    public async override Task<ActionResult<Response<AfiliadosReintegrosDto>>> HandleAsync(AfiliadosReintegrosDto convocatoriaDTO, CancellationToken cancellationToken = default)
    {
        return await _reintegrosBusiness.UpdateAfiliadoReintegro(convocatoriaDTO);
    }
}
}
