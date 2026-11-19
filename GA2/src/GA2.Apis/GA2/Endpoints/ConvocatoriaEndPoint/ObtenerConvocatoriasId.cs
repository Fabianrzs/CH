using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using System.Collections.Generic;
using GA2.Transversals.Commons;
using GA2.Application.Dto;

namespace GA2.Apis.GA2.Endpoints
{
    public class ObtenerConvocatoriasId: BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<ConvocatoriaDTO>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

    public ObtenerConvocatoriasId(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
    {
        _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
    }
    [HttpGet("api/Convocatoria/ConsultarConvocatoriasId")]
    [ProducesResponseType(typeof(Response<ConvocatoriaDTO>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Obtiene todos los id de las convocatoria",
        Description = "Obtiene todos los id de las  convocatoria ",
        OperationId = "convocatoria.ConsultarConvocatoriasId",
        Tags = new[] { "ConvocatoriaEndPoint" })]
    public async override Task<ActionResult<Response<ConvocatoriaDTO>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return await _convocatoriaBussigneLogic.Obtener_Id_Convocatoria();
    }
     
    }
}
