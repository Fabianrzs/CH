using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Parametrization;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.ParametrizationEndPoints
{
    public class ObtenerParametroIntegracion : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ParametroIntegracionDto>>>
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Get ObtenerParametrosIntegraciones
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>18/05/2021</date>
        public ObtenerParametroIntegracion(IParametrizacionBusinessLogic parametrizationBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
        }
        /// <summary>
        /// Obtener parametrizacion de tasas
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/parametrizacion/ObtenerParametrosIntegraciones")]
        [ProducesResponseType(typeof(Response<IEnumerable<ParametroIntegracionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene parametrizacion de integracion",
            Description = "Obtiene parametrizacion de integracion del aplicativo",
            OperationId = "parametrizacion.ObtenerParametrosIntegraciones",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ParametroIntegracionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _parametrizationBusinessLogic.ObtenerParametrosIntegraciones();
        }
    }
}
