using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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
    public class ObtenerPuntoAtencion : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<PuntosAtencionDto>>>
    {
        private readonly IPuntoAtencionBusinessLogic _puntoAtencionBusinessLogic;

        public ObtenerPuntoAtencion(IPuntoAtencionBusinessLogic puntoAtencionBusinessLogic)
        {
            _puntoAtencionBusinessLogic = puntoAtencionBusinessLogic;
        }

        [HttpGet("api/PuntoAtencion/ObtenerPuntoAtencion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<PuntosAtencionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Punto Atencion",
            Description = "Obtener Punto Atencion",
            OperationId = "PuntoAtencion.ObtenerPuntoAtencion",
            Tags = new[] { "PuntoAtencionEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<PuntosAtencionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._puntoAtencionBusinessLogic.ObtenerPuntoAtencion();
        }
    }
}
