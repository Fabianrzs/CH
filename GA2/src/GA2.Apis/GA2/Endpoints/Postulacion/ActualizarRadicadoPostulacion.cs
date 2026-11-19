using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Entities.Postulacion;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ActualizarRadicadoPostulacion : BaseAsyncEndpoint.WithRequest<ActualizarPostulacionDto>.WithResponse<Response<string>>
    {
        private readonly IReintegrosBusinessLogic _iReintegrosBusinessLogic;
        private const string USR_ID = "USR_ID";
        public ActualizarRadicadoPostulacion(IReintegrosBusinessLogic iReintegrosBusinessLogic)
        {
            _iReintegrosBusinessLogic = iReintegrosBusinessLogic;
        }

        [HttpPost("api/postulante/ActualizarRadicadoPostulacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CorrespondenciaReintegro>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar folio flujo 39",
           Description = "Actualizar folio flujo 39",
           OperationId = "postulante.ActualizarRadicadoPostulacion",
           Tags = new[] { "ActualizarRadicadoPostulacion" })]

        public override async Task<ActionResult<Response<string>>> HandleAsync(ActualizarPostulacionDto request, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid().ToString();

            try
            {
                userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }
            request.CreadoPor = Guid.Parse(userId);
            return await _iReintegrosBusinessLogic.ActualizarRadicadoPostulacion(request);
        }
    }
}
