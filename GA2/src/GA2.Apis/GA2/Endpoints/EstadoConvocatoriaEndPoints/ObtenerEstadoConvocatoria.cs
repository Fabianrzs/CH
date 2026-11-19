using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.EstadoConvocatoriaEndPoints
{
    public class ObtenerEstadoConvocatoria : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<EstadoConvocatoriaDto>>>
    {
        private readonly IEstadoConvocatoriaBussinesLogic _estadoConvocatoriaLogic;

        public ObtenerEstadoConvocatoria(IEstadoConvocatoriaBussinesLogic estadosBusinessLogic)
        {
            _estadoConvocatoriaLogic = estadosBusinessLogic;
        }

        [HttpGet("api/estadoConvocatoria/obtenerEstadoConvocatoria")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadoConvocatoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Obtiene Estado Convocatoria",
                   Description = "Obtiene Estado Convocatoria",
                   OperationId = "estadoConvocatoria.obtenerEstadoConvocatoria",
                   Tags = new[] { "estadoConvocatoriaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EstadoConvocatoriaDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _estadoConvocatoriaLogic.ObtenerEstadosConvocatoria();
        }
    }
}
