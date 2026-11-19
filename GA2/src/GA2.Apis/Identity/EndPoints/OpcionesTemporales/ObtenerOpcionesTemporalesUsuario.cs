using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using GA2.Transversals.Commons;

namespace GA2.Apis.Identity.EndPoints.OpcionesTemporales
{
    public class ObtenerOpcionesTemporalesUsuario :
        BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<OpcionTemporalUsuarioDto>>>
    {

        private readonly IOpcionesTemporalesBusinessLogic _opcionesTemporalesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opcionesTemporalesBusinessLogic"></param>
        public ObtenerOpcionesTemporalesUsuario(IOpcionesTemporalesBusinessLogic opcionesTemporalesBusinessLogic)
        {
            _opcionesTemporalesBusinessLogic = opcionesTemporalesBusinessLogic;
        }

        /// <summary>
        /// Endpoint Menus dtos
        /// </summary>
        /// <param name="cancellationToken">Cancelacion Token</param>
        /// <returns></returns>
        [HttpGet("api/ObtenerOpcionesTemporalesUsuario")]
        [ProducesResponseType(typeof(Response<OpcionTemporalUsuarioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener opciones temporales",
           Description = "Obtener opciones temporales",
           OperationId = "roles.ObtenerOpcionesTemporalesUsuario",
           Tags = new[] { "RolesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<OpcionTemporalUsuarioDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._opcionesTemporalesBusinessLogic.ObtenerOpcionesTemporalesUsuario(request);
        }
    }
}
