//namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
//{
//    public class ActualizaSolicitudNomina
//    {
//    }
//}


using Ardalis.ApiEndpoints;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;


namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    /// <summary>
    /// Endpoint para carga de archivos de nómina
    /// </summary>
    [Authorize]

    public class ActualizaSolicitudNomina : BaseAsyncEndpoint.WithRequest<SolicitudNominaDto>.WithResponse<Response<SolicitudNominaDto>>
    {
        private readonly IMovimientoBusinessLogic _movimeintoBusinessLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        public ActualizaSolicitudNomina(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimeintoBusinessLogic = movimientoBusinessLogic;
        }
        /// <summary>
        /// Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/ActualizaSolicitudNomina")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<SolicitudNominaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Solicitud Nomina",
            Description = "Actualiza Solicitud Nomina",
            OperationId = "fileupload.ActualizaSolicitudNomina",
            Tags = new[] { "CargueNominaEndpoint" })]


        public override async Task<ActionResult<Response<SolicitudNominaDto>>> HandleAsync(SolicitudNominaDto request, CancellationToken cancellationToken = default)
        {
            return await _movimeintoBusinessLogic.ActualizaSolicitudNomina(request);
        }
    }
}

