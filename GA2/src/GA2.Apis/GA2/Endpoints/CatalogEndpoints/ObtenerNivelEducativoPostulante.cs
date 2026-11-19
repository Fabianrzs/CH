using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para Obtener niveles educativos del postulante al fondo de solidaridad y su núcleo familiar
    /// </summary>
    /// <author>Hanson Restrepo</author>
    /// <date>05/07/2022</date>
    [Authorize]
    public class ObtenerNivelEducativoPostulante :
        BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<NivelAcademicoCatalogoDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint para Obtener niveles educativos del postulante al fondo de solidaridad y su núcleo familiar
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/06/2022</date>
        public ObtenerNivelEducativoPostulante(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/catalogos/obtenerNivelEducativoPostulante")]
        [ProducesResponseType(typeof(Response<IEnumerable<NivelAcademicoCatalogoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerNivelEducativoPostulante",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<NivelAcademicoCatalogoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerNivelEducativoPostulante();
        }
    }
}