using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Parametrization;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.ParametrizationEndPoints
{
    /// <summary>
    /// Endpoint para agregar registros en tablas parámetros
    /// <author>Michel Parra</author>
    /// <date>23/08/2023</date>
    /// </summary>
    [Authorize]
    public class AgregarTablasParametros : BaseAsyncEndpoint.WithRequest<dynamic>.WithResponse<Response<dynamic>>
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint AgregarTablasParametros 
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Michel Parra</author>
        /// <date>23/08/2023</date>
        /// </summary>
        public AgregarTablasParametros(IParametrizacionBusinessLogic parametrizationBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Parametrizacion/AgregarTablasParametros")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<dynamic>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Agrega agregar registros en tablas parámetros",
            Description = "Agrega agregar registros en tablas parámetros",
            OperationId = "parametrizacion.AgregarTablasParametros",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<dynamic>>> HandleAsync(dynamic data, CancellationToken cancellationToken = default)
        {
            return await _parametrizationBusinessLogic.AgregarTablasParametros(data);
        }
    }
}
