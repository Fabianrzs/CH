using Ardalis.ApiEndpoints;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    public class ConsultarArchivoCorregidoSolicitudNomina
    : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<DescargarArchivoCorregidoSolicitudNomina>>
    {
        private readonly IDocumentoBusinessLogic _documentoBusinessLogic;
        /// <summary>
        /// Contrustutor de archivo corregido
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        public ConsultarArchivoCorregidoSolicitudNomina(IDocumentoBusinessLogic documentoBusinessLogic)
        {
            _documentoBusinessLogic = documentoBusinessLogic;
        }
        /// <summary>
        /// Endpoint para consultar archivo corregido
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Reportes/ConsultarArchivoCorregidoSolicitudNomina")]
        [ProducesResponseType(typeof(Response<SolicitudNominaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Solicitud Nomina",
            Description = "Consultar archivo corregido",
            OperationId = "fileupload.consultarArchivoCorregido",
            Tags = new[] { "CargueNominaEndpoint" })]
        public override async Task<ActionResult<Response<DescargarArchivoCorregidoSolicitudNomina>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await _documentoBusinessLogic.ArchivoCorregidoSolicitudNomina(request); 
        }
    }
}
