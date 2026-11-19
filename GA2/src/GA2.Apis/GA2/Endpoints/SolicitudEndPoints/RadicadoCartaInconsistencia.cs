using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class RadicadoCartaInconsistencia : BaseAsyncEndpoint.WithRequest<CrearRadicadoCartaIncosistencias>
        .WithResponse<Response<string>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Crear solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Erwin Pantoja</author>
        /// <date>20/10/2021</date>
        public RadicadoCartaInconsistencia(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para crear una solicitud
        /// </summary>
        /// <param name="crearSolicitudDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/CrearRadicadoCartaIncosistencia")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Radicado declaracion carta de inconsistencas de una Solicitud",
           Description = "Creaacion Radicado declaracion carta de inconsistencas de una Solicitud",
           OperationId = "Solicitud.CrearRadicadoCartaIncosistencia",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(CrearRadicadoCartaIncosistencias crearSolicitudDto, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.CrearRadicadoCartaIncosistencia(crearSolicitudDto);
        }
    }
}
