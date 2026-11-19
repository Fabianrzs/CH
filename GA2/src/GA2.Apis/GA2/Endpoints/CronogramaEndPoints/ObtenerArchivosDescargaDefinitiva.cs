using Ardalis.ApiEndpoints;
using GA2.Application.Dto.CronogramaActividades;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    /// <summary>
	/// Endpoint para obtener descargas definitivas de archivos de novedades
	/// </summary>
	/// <author>Michel Parra</author>
	/// <date>06/03/2024</date>
    [Authorize]
    public class ObtenerArchivosDescargaDefinitiva :
         BaseAsyncEndpoint.WithRequest<DescargaDefinitivaDto>.WithResponse<Response<IEnumerable<DescargaDefinitivaDto>>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Michel Parra</author>
        /// <date>06/03/2024</date>
        public ObtenerArchivosDescargaDefinitiva(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
        }
        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cronograma/ObtenerArchivosDescargaDefinitiva")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DescargaDefinitivaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "obtener descargas definitivas de archivos de novedades",
           Description = "obtener descargas definitivas de archivos de novedades",
           OperationId = "Cronograma.ObtenerArchivosDescargaDefinitiva",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<DescargaDefinitivaDto>>>> HandleAsync(DescargaDefinitivaDto request, CancellationToken cancellationToken = default)
        {
            return await _cronogramaBusinessLogic.ObtenerArchivosDescargaDefinitiva(request);
        }
    }
}
