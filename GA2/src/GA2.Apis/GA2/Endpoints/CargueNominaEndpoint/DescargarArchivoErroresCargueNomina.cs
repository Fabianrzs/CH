using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Endpoints.FileEndpoints
{
    /// <summary>
    /// Descargar archivo errores nominas
    /// </summary>
    [Authorize]
    public class DescargarArchivoErroresCargueNomina : BaseAsyncEndpoint.WithRequest<int>.WithResponse<IFormFile>
    {
        private readonly IMovimientoBusinessLogic _movimeintoBusinessLogic;
        
        /// <summary>
        /// Ctr descargue de errores 
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        public DescargarArchivoErroresCargueNomina(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimeintoBusinessLogic = movimientoBusinessLogic;
        }

        /// <summary>
        /// Aprueba Datos Comite Credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/descargarArchivoErroresCargueNomina/{request}")]
        [ProducesResponseType(typeof(IFormFile), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Acta Comite",
            Description = "Generar Acta Comite",
            OperationId = "credito.GenerarActaComite",
            Tags = new[] { "CargueNominaEndpoint" })]
        public async override Task<ActionResult<IFormFile>> HandleAsync([FromRoute] int request, CancellationToken cancellationToken = default)
        {
            return await this._movimeintoBusinessLogic.DescargarArchivoErroresCargueNomina(request);
        }        
    }
}
