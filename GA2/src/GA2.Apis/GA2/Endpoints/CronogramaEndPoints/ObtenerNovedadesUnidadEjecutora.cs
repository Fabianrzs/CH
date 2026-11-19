using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    public class ObtenerNovedadesUnidadEjecutora :
         BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<IEnumerable<NovedadesCronogramaDto>>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerNovedadesUnidadEjecutora(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "idUnidadEjecutora"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/cronograma/ObtenerNovedadesUnidadEjecutora")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CronogramaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "ObtenerNovedadesUnidadEjecutora",
           Description = "ObtenerNovedadesUnidadEjecutora",
           OperationId = "Cronograma.ObtenerNovedadesUnidadEjecutora",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<NovedadesCronogramaDto>>>> HandleAsync(int idUnidadEjecutora, CancellationToken cancellationToken = default)
        {
            return await this._cronogramaBusinessLogic.ObtenerNovedadesUnidadEjecutora(idUnidadEjecutora);
        }
    }
}
