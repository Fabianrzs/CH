using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    //[Authorize]
    public class GeneracionArchivoNovedades :
         BaseAsyncEndpoint.WithRequest<GenerarArchivoRequest>.WithResponse<Response<GeneracionArchivosResponse>>
    {
        
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public GeneracionArchivoNovedades(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cronograma/GeneracionArchivoNovedades")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<GeneracionArchivosResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Generar archivo de novedades por concepto",
           Description = "Generar archivo de novedades por concepto",
           OperationId = "Cronograma.GeneracionArchivoNovedades",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<GeneracionArchivosResponse>>> HandleAsync(GenerarArchivoRequest request, CancellationToken cancellationToken = default)
        {
            return await this._cronogramaBusinessLogic.GeneracionArchivoNovedades(request);            
        }
    }
}
