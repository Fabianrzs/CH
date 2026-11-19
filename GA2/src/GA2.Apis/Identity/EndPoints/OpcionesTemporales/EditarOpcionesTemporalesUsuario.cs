using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.Identity.EndPoints.OpcionesTemporales
{
    public class EditarOpcionesTemporalesUsuario :
        BaseAsyncEndpoint.WithRequest<OpcionTemporalUsuarioDto>
        .WithResponse<Response<OpcionTemporalUsuarioDto>>
    {

        private readonly IOpcionesTemporalesBusinessLogic _opcionesTemporalesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opcionesTemporalesBusinessLogic"></param>
        public EditarOpcionesTemporalesUsuario(IOpcionesTemporalesBusinessLogic opcionesTemporalesBusinessLogic)
        {
            _opcionesTemporalesBusinessLogic = opcionesTemporalesBusinessLogic;
        }

        /// <summary>
        /// Endpoint Menus dtos
        /// </summary>
        /// <param name="cancellationToken">Cancelacion Token</param>
        /// <returns></returns>
        [HttpPost("api/EditarOpcionesTemporalesUsuario")]
        [ProducesResponseType(typeof(Response<OpcionTemporalUsuarioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Insertar opciones temporales",
           Description = "Insertar opciones temporales",
           OperationId = "roles.EditarOpcionesTemporalesUsuario",
           Tags = new[] { "RolesEndpoint" })]
        public async override Task<ActionResult<Response<OpcionTemporalUsuarioDto>>> HandleAsync(OpcionTemporalUsuarioDto request, CancellationToken cancellationToken = default)
        {
            return await this._opcionesTemporalesBusinessLogic.EditarOpcionesTemporalesUsuario(request);
        }
    }
}
