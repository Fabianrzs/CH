using Ardalis.ApiEndpoints;
using Azure;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core.Postulacion;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class DescargarArchivoFondoSolidaridadEndPoint : 
        BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<IFormFile>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

    /// <summary>
    /// Constructor del Endpoint descargar archivo solicitud
    /// </summary>
    /// <param name="PostulacionBusinessLogic"></param>
    public DescargarArchivoFondoSolidaridadEndPoint(IPostulacionBussineLogic postulacionBussineLogic)
    {
        _postulacionBussineLogic = postulacionBussineLogic;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rutaStorage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    //[ValidateAntiForgeryToken]
    [HttpGet("api/postulante/DescargarArchivoPorSolicitud")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "DescargarArchivoPorSolicitud",
        Description = "Descargar Archivo por Solicitud",
        OperationId = "postulante.DescargarArchivoPorSolicitud",
        Tags = new[] { "PostulanteEndPoint" })]
    public async override Task<ActionResult<IFormFile>> HandleAsync(string rutaStorage, CancellationToken cancellationToken = default)
    {
        return await _postulacionBussineLogic.DescargarArchivoPorSolicitud(rutaStorage);
    }
}
}
