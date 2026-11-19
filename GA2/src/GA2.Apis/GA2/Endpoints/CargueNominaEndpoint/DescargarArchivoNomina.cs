using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    /// <summary>
    /// Descargar archivo nómina
    /// </summary>
    [Authorize]
    public class DescargarArchivoNomina : BaseAsyncEndpoint.WithRequest<int>.WithResponse<IFormFile>
    {
        private readonly IMovimientoBusinessLogic _movimeintoBusinessLogic;

        /// <summary>
        /// Constructor Descargar archivo nómina
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        public DescargarArchivoNomina(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimeintoBusinessLogic = movimientoBusinessLogic;
        }

        /// <summary>
        /// Descargar archivo nómina
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/descargarArchivoNomina/{request}")]
        [ProducesResponseType(typeof(IFormFile), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Descargar archivo nómina",
            Description = "Descargar archivo nómina",
            OperationId = "credito.descargarArchivoNomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        public async override Task<ActionResult<IFormFile>> HandleAsync([FromRoute] int request, CancellationToken cancellationToken = default)
        {
            return await this._movimeintoBusinessLogic.DescargarArchivoNomina(request);
        }
    }
}
