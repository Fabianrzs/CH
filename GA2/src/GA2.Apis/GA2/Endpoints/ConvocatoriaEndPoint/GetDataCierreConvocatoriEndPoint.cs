
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Ardalis.ApiEndpoints;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class GetDataCierreConvocatoriEndPoint : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<CierreConvocatoriaDto>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

    public GetDataCierreConvocatoriEndPoint(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
    {
        _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
    }
    [HttpGet("api/Convocatoria/GetDataCierreConvocatoria")]
    [ProducesResponseType(typeof(Response<CierreConvocatoriaDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Obtiene todos los dato  del cierre de convocatoria",
        Description = "Obtiene todos los dato  del cierre de  convocatoria ",
        OperationId = "convocatoria.GetDataCierreConvocatoria",
        Tags = new[] { "ConvocatoriaEndPoint" })]
    public async override Task<ActionResult<Response<CierreConvocatoriaDto>>> HandleAsync(int idConvocatoria, CancellationToken cancellationToken = default)
    {
        return await _convocatoriaBussigneLogic.GetDataCierreConvocatoria(idConvocatoria);
    }
}
}
