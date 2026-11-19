using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class CargarDocumentosPreAprobASocCredito : BaseAsyncEndpoint.WithRequest<RequestCargarDocumentosPreAprobASocCreditoDto>.WithResponse<Response<bool>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;

        public CargarDocumentosPreAprobASocCredito(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        [HttpPost("api/prospeccion/CargarArchivosPreAprobado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SimulacionClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cargar Archivos PreAprobado",
           Description = "Cargar Archivos PreAprobado",
           OperationId = "prospeccion.CargarArchivosPreAprobado",
           Tags = new[] { "ProspeccionEndpoint" })]

        public override async Task<ActionResult<Response<bool>>> HandleAsync(RequestCargarDocumentosPreAprobASocCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.CargarArchivosPreAprobado(request);
        }
    }
}
