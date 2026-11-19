using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using GA2.Application.Dto.Reintegros;

namespace GA2.Apis.GA2.Endpoints.Reintegros.GestorDocumental
{
    [Authorize]
    public class ObtenerCorrespondenciaReintegros : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<CorrespondenciaReintegro>>
    {
        private readonly IReintegrosBusinessLogic _iReintegrosBusinessLogic;

        public ObtenerCorrespondenciaReintegros(IReintegrosBusinessLogic iReintegrosBusinessLogic)
        {
            _iReintegrosBusinessLogic = iReintegrosBusinessLogic;
        }

        [HttpPost("api/Reintegros/ObtenerCorrespondenciaReintegros")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CorrespondenciaReintegro>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar informacion flujo 04",
           Description = "Consultar informacion flujo 04",
           OperationId = "Reintegros.ObtenerCorrespondenciaReintegros",
           Tags = new[] { "ObtenerCorrespondenciaReintegrosEndpoint" })]

        public override async Task<ActionResult<Response<CorrespondenciaReintegro>>> HandleAsync(string radicado, CancellationToken cancellationToken = default)
        {
            return await _iReintegrosBusinessLogic.ObtenerCorrespondenciaReintegros(radicado);
        }
    }
}
