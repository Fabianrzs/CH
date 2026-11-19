using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Authorization;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    [Authorize]
    public class ObtenerHistorialCambiosIntervinientes :
        BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<HistorialCambiosIntervinienteDto>>>
    {
        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Obtener Programa Educativo
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>26/01/2022</date>
        public ObtenerHistorialCambiosIntervinientes(IEntidadesBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }

        /// <summary>
        /// Obtiene todos los Programas Educativos
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Entidades/ObtenerHistorialCambiosIntervinientes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<HistorialCambiosIntervinienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "Entidades.ObtenerHistorialCambios",
            Tags = new[] { "EntidadesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<HistorialCambiosIntervinienteDto>>>> HandleAsync(Guid idIntervinientes, CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerHistorialCambiosIntervinientes(idIntervinientes);
        }
    }
}
