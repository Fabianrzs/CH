using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Parametrization;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.ParametrizationEndPoints
{
    /// <summary>
    /// Endpoint para obtener los nombres de las tablas parámetros por categoría
    /// <author>Michel Parra</author>
    /// <date>18/07/2023</date>
    /// </summary>
    [Authorize]
    public class ValorInmueble :
        BaseAsyncEndpoint.WithRequest<ValorInmuebleDto>.WithResponse<Response<IEnumerable<ValorInmuebleResponseDto>>>
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint ObtenerNombresTablasParametrosPorCategoria 
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Michel Parra</author>
        /// <date>18/07/2023</date>
        /// </summary>
        public ValorInmueble(IParametrizacionBusinessLogic parametrizationBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Parametrizacion/ValorInmueble")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ValorInmuebleResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene los nombres de la tablas parametros por categoria",
            Description = "Obtiene los nombres de la tablas parametros por categoria",
            OperationId = "parametrizacion.ObtenerNombresTablasParametrosPorCategoria",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ValorInmuebleResponseDto>>>> HandleAsync(ValorInmuebleDto request, CancellationToken cancellationToken = default)
        {
            return await _parametrizationBusinessLogic.ValorInmueble(request);
        }
    }
}