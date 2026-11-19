using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.ReporteSimulacion;
using GA2.Domain.Core.ReporteSimulacionBL;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.ReporteSimulacionEndpoint
{
    //[Authorize]
    public class GuardarReporteSimulaciones : BaseAsyncEndpoint.WithRequest<RequestReporteSimulacionDto>.WithResponse<Response<bool>>
    {
        private readonly IReporteSimulacionBusinessLogic _reporteSimulacionBusinessLogic;

        public GuardarReporteSimulaciones(IReporteSimulacionBusinessLogic reporteSimulacionBusinessLogic)
        {
            _reporteSimulacionBusinessLogic = reporteSimulacionBusinessLogic;
        }

        [HttpPost("api/Simulacion/GuardarReporteSimulacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Guardar Simulacion",
            Description = "Guardar Simulacion PreAprobado",
            OperationId = "simulacion.GuardarSimulacion",
            Tags = new[] { "GuardarSimulacionEndpoints" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(RequestReporteSimulacionDto request, CancellationToken cancellationToken = default)
        {
            return await this._reporteSimulacionBusinessLogic.GuardarReporteSimulaciones(request);
        }
    }
}
