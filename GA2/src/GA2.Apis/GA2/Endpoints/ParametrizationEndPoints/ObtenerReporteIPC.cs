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
    /// Endpoint para obtener el IPC filtrado por periodo
    /// <author>Cesar Hernandez</author>
    /// <date>14/10/2021</date>
    /// </summary>
    [Authorize]
    public class ObtenerReporteIPC : BaseAsyncEndpoint.WithRequest<IPCDto>.WithResponse<Response<IEnumerable<IPCDto>>>
    {

        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ObtenerReporteIPC(IParametrizacionBusinessLogic parametrizationBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/parametrizacion/ObtenerReporteIPC")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<IPCDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = " Obtener reporte IPC Filtrado por rangos",
            Description = "Obtener reporte IPC Filtrado por rangos",
            OperationId = "parametrizacion.desde_ano y hasta_ano",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<IPCDto>>>> HandleAsync([FromQuery] IPCDto request, CancellationToken cancellationToken = default)
        {
            return await _parametrizationBusinessLogic.ObtenerReporteIPC(request);
        }


    }

}

