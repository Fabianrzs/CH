using Ardalis.ApiEndpoints;
using GA2.Application.Dto.PuntosAtencion;
using GA2.Domain.Core.PuntosAtencion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.PuntosAtencionEndPoints
{
    public class ActualizarPuntoAtencion : BaseAsyncEndpoint.WithRequest<PuntosAtencionDto>.WithResponse<Response<PuntosAtencionDto>>
    {
        private readonly IPuntoAtencionBusinessLogic _puntoAtencionBusinessLogic;

        public ActualizarPuntoAtencion(IPuntoAtencionBusinessLogic puntoAtencionBusinessLogic)
        {
            _puntoAtencionBusinessLogic = puntoAtencionBusinessLogic;
        }

        [HttpPost("api/PuntoAtencion/ActualizarPuntoAtencion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PuntosAtencionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar Punto Atencion",
            Description = "Actualizar Punto Atencion",
            OperationId = "PuntoAtencion.ActualizarPuntoAtencion",
            Tags = new[] { "PuntoAtencionEndpoint" })]
        public override async Task<ActionResult<Response<PuntosAtencionDto>>> HandleAsync(PuntosAtencionDto request, CancellationToken cancellationToken = default)
        {
            return await this._puntoAtencionBusinessLogic.ActualizarPuntoAtencion(request);
        }

    }
}
