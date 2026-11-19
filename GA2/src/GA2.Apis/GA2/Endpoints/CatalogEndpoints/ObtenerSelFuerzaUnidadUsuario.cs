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
    public class ObtenerSelFuerzaUnidadUsuario : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<SelFuerzaUnidadUsuarioDto>>
    {

        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Crear Tipo Credito
        /// </summary>
        /// <param name="catalogosBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerSelFuerzaUnidadUsuario(ICatalogosBusinessLogic catalogosBusinessLogic)
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
        [HttpGet("api/ObtenerSelFuerzaUnidadUsuario")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SelFuerzaUnidadUsuarioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Obtener la selección fuerza y unidad del usuario",
            Description = "Obtener selección fuerza y unidad del usuario",
            OperationId = "catalogos.ObtenerSelFuerzaUnidadUsuario",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<SelFuerzaUnidadUsuarioDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._catalogosBusinessLogic.ObtenerSelFuerzaUnidadUsuario(request);
        }
    }
}

