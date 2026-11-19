using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.SolicitudConvocatoria;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class ObtenerIdConvocatoria : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<int>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public ObtenerIdConvocatoria(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/obtenetIdConvocatoria")]
        [ProducesResponseType(typeof(Response<int>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el id de la convocatoria",
            Description = "Obtiene el ID de la convocatoria siguiente",
            OperationId = "convocatoria.obtenetIdConvocatoria",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<int>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.Consultar_Id_Convocatoria();
        }
    }
}
