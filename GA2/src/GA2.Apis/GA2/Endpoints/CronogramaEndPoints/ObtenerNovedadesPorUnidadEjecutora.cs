using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    public class ObtenerNovedadesPorUnidadEjecutora :
         BaseAsyncEndpoint.WithRequest<NovedadPorUnidadEjecutoraDto>.WithResponse<Response<IEnumerable<NovedadPorUnidadEjecutoraDto>>>
    {

        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerNovedadesPorUnidadEjecutora(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cronograma/ObtenerNovedadesPorUnidadEjecutora")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CronogramaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "ObtenerNovedadesPorUnidadEjecutora",
           Description = "ObtenerNovedadesPorUnidadEjecutora",
           OperationId = "Cronograma.ObtenerNovedadesPorUnidadEjecutora",
           Tags = new[] { "ObtenerNovedadesPorUnidadEjecutora" })]
        public async override Task<ActionResult<Response<IEnumerable<NovedadPorUnidadEjecutoraDto>>>> HandleAsync(NovedadPorUnidadEjecutoraDto request, CancellationToken cancellationToken = default)
        {
            return await this._cronogramaBusinessLogic.ObtenerNovedadesPorUnidadEjecutora(request);
        }
    }
}
