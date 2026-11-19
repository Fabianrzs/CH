using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Servicio para registrar la selección de la fuerza y unidad del usuario
    /// </summary>
    [Authorize]

    public class RegistrarSelFuerzaUnidadUsuario : BaseAsyncEndpoint.WithRequest<SelFuerzaUnidadUsuarioDto>.WithResponse<Response<bool>>
    {

        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Crear Tipo Credito
        /// </summary>
        /// <param name="catalogosBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public RegistrarSelFuerzaUnidadUsuario(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/RegistrarSelFuerzaUnidadUsuario")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Crear selección fuerza y unidad del usuario",
            Description = "Crear selección fuerza y unidad del usuario",
            OperationId = "catalogos.RegistrarSelFuerzaUnidadUsuario",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(SelFuerzaUnidadUsuarioDto request, CancellationToken cancellationToken = default)
        {
            return await this._catalogosBusinessLogic.RegistrarSelFuerzaUnidadUsuario(request);
        }
    }
}

