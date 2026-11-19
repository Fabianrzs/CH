using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class CrearIncosistenciaFondoEndPoint : BaseAsyncEndpoint.WithRequest<IncosistenciaFondoRequest>
        .WithResponse<Response<string>>
    {
        private readonly IPostulacionBussineLogic _IPostulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        public CrearIncosistenciaFondoEndPoint(IPostulacionBussineLogic IPostulacionBussineLogic)
        {
            _IPostulacionBussineLogic = IPostulacionBussineLogic;
        }

        [HttpPost("api/postulante/GuardarIncosistenciaSolicitudFondo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear incosistencia de la solciitud de fondo de solidaridad",
           Description = "Crear incosistencia de la solciitud de fondo de solidaridad",
           OperationId = "postulante.GuardarIncosistenciaSolicitudFondo",
           Tags = new[] { "GuardarIncosistenciaSolicitudFondo" })]

        public override async Task<ActionResult<Response<string>>> HandleAsync(IncosistenciaFondoRequest request, CancellationToken cancellationToken = default)
        {
            return await _IPostulacionBussineLogic.GuardarIncosistenciaSolicitudFondo(request);
        }
    }
}
