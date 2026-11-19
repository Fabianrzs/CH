using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using GA2.Domain.Entities;
using SixLabors.ImageSharp.ColorSpaces;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class GetStagePostulation : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<List<StagePostulationDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bussigneLogic"></param>
    public GetStagePostulation(IPostulacionBussineLogic bussigneLogic)
    {
        _postulacionBussineLogic = bussigneLogic;
    }
    [HttpGet("api/postulante/GetStagePostulation")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "obtener los estado y actividad de fondo solidaridad",
        Description = "obtener los estado y actividad de  fondo solidaridad po id de solicitud",
        OperationId = "postulante.GetStagePostulation",
        Tags = new[] { "PostulanteEndPoint" })]
    public async override Task<ActionResult<Response<List<StagePostulationDto>>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _postulacionBussineLogic.GetStagePostulacion(id);
    }
}
}
