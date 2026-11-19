using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class CargarArchivosClientes : BaseAsyncEndpoint.WithRequest<SolicitudSimulacionDto>.WithResponse<Response<SolicitudSimulacionDto>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Ctor e inyeccion dependencias
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public CargarArchivosClientes(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        /// <summary>
        /// Cargar Archivos Clientes
        /// </summary>
        /// <param name = "request" > Peticion usuario</param>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        [HttpPost("api/prospeccion/CargarArchivosClientes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SimulacionClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cargar Archivos Clientes",
           Description = "Cargar Archivos Clientes",
           OperationId = "prospeccion.CargarArchivosClientes",
           Tags = new[] { "ProspeccionEndpoint" })]

        public override async Task<ActionResult<Response<SolicitudSimulacionDto>>> HandleAsync(SolicitudSimulacionDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.CrearPreAprobado(request);
        }
    }
}
