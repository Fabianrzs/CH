using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    public class ObtenerNovedadAfiliadoPorId :
         BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<NovedadesAfiliadoDto>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerNovedadAfiliadoPorId(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/cronograma/ObtenerNovedadAfiliadoPorId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CronogramaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "ObtenerNovedadAfiliadoPorId",
           Description = "ObtenerNovedadAfiliadoPorId",
           OperationId = "Cronograma.ObtenerNovedadAfiliadoPorId",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<NovedadesAfiliadoDto>>> HandleAsync(int idNovedad, CancellationToken cancellationToken = default)
        {
            return await this._cronogramaBusinessLogic.ObtenerNovedadAfiliadoPorId(idNovedad);
        }
    }
}
