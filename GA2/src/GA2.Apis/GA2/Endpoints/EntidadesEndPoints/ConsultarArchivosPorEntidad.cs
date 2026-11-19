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


namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    /// <summary>
    /// Endpoint para consultar los archivos de una solicitud
    /// </summary>
    /// <author>Hanson Restrepo</author>
    [Authorize]
    public class ConsultarArchivosPorEntidad :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<ConsultarArchivosPorEntidadDto>>>
    {
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;

        public ConsultarArchivosPorEntidad(IEntidadesBusinessLogic entidadesBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
        }

        [HttpGet("api/Entidades/ConsultarArchivosPorEntidad")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar archivos por entidade",
           Description = "Consultar archivos por entidad",
           OperationId = "Entidades.ConsultarArchivosPorEntidad",
           Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ConsultarArchivosPorEntidadDto>>>> HandleAsync(Guid IdEntidad, CancellationToken cancellationToken = default)
        {
            return await this._entidadesBusinessLogic.ConsultarArchivosPorEntidad(IdEntidad);
        }
    }
}
