using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    public class ObtenerNovedadesAfiliado :
         BaseAsyncEndpoint.WithRequest<DocumentoDTO>.WithResponse<Response<IEnumerable<NovedadesAfiliadoDto>>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerNovedadesAfiliado(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/cronograma/ObtenerNovedadesAfiliado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<NovedadesAfiliadoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "ObtenerNovedadesAfiliado",
           Description = "Obtener Novedades Afiliado",
           OperationId = "Cronograma.ObtenerNovedadesAfiliado",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<NovedadesAfiliadoDto>>>> HandleAsync([FromQuery] DocumentoDTO request, CancellationToken cancellationToken = default)

        {
            return await this._cronogramaBusinessLogic.ObtenerNovedadesAfiliado(request.tipoDocumentoId, request.numeroDocumento);
        }
    }
}
