using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint carga documento solicitud de credito
    /// </summary>

    public class ObtenerDocumentosRadicado : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<FileResponseDto>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Nelson Redondo</author>
        /// <date>20/10/2021</date>
        public ObtenerDocumentosRadicado(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Consultar Informacion del turno
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/solicitud/ObtenerDocumentosRadicado")]
        [Consumes("application/json")]
        [SwaggerOperation(
           Summary = "Obtener Obtener Obtener Documentos Por Numero Radicado",
           Description = "Obtener Obtener Obtener Documentos Por Numero Radicado",
           OperationId = "Solicitud.ObtenerDocumentosRadicado",
           Tags = new[] { "ObtenerDocumentosRadicado" })]
        public async override Task<ActionResult<Response<FileResponseDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerDocumentosPorRadicado(request);
        }
    }
}
