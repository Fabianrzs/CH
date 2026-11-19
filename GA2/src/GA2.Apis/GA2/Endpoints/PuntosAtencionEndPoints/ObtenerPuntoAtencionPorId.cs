using Ardalis.ApiEndpoints;
using GA2.Application.Dto.PuntosAtencion;
using GA2.Domain.Core.PuntosAtencion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.PuntosAtencionEndPoints
{
    public class ObtenerPuntoAtencionPorId : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<PuntosAtencionDto>>
    {
        private readonly IPuntoAtencionBusinessLogic _puntoAtencionBusinessLogic;

        public ObtenerPuntoAtencionPorId(IPuntoAtencionBusinessLogic puntoAtencionBusinessLogic)
        {
            _puntoAtencionBusinessLogic = puntoAtencionBusinessLogic;
        }

        [HttpGet("api/PuntoAtencion/ObtenerPuntoAtencionPorId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PuntosAtencionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Punto Atencion por id",
            Description = "Obtener Punto Atencion por id",
            OperationId = "PuntoAtencion.ObtenerPuntoAtencionPorId",
            Tags = new[] { "PuntoAtencionEndpoint" })]
        public override async Task<ActionResult<Response<PuntosAtencionDto>>> HandleAsync(int Id_PuntoAtencion,CancellationToken cancellationToken = default)
        {
            return await this._puntoAtencionBusinessLogic.ObtenerPuntoAtencionPorId(Id_PuntoAtencion);
        }

    }
}
