using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Consultar Anotaciones de una Entidad Educativa
    /// </summary>
    /// <author></author>
    /// <date></date>
    [Authorize]
    public class ConsultarAnotaciones :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<RespuestaAnotacionDTO>>>
    {
        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint ConsultarAnotaciones
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public ConsultarAnotaciones(IEntidadesBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }

        /// <summary>
        /// Consulta las Anotaciones de una Entidad Educativa
        /// </summary>
        /// <param name="request">Id de la entidad educativa</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Lista de anotaciones</returns>
        [HttpPost("api/Entidades/ConsultarAnotaciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<RespuestaAnotacionDTO>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Consulta Anotaciones de una Entidad Educativa",
          Description = "Consulta Anotaciones de una Entidad Educativa",
          OperationId = "Entidades.ConsultarAnotaciones",
          Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<RespuestaAnotacionDTO>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ConsultarAnotaciones(request);
            
        }
    }
}
