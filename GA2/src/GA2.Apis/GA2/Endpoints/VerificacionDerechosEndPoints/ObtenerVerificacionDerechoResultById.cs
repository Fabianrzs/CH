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
 

    [Authorize]
    public class ObtenerVerificacionDerechoResultById : BaseAsyncEndpoint
                                                .WithRequest<int>
                                                .WithResponse<Response<IEnumerable<ResultVerificacionDerechoDto>>>
    {

        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="verificacionDerechosBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ObtenerVerificacionDerechoResultById(IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic)
        {
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
        }

        /// <summary>
        /// Obtener parametrizacion de tasas
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/VerificacionDerechos/ObtenerVerificacionDerechoResultById")]
        [ProducesResponseType(typeof(Response<IEnumerable<ResultVerificacionDerechoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos las Verificaciones de derechos",
            Description = "Obtiene todos las Verificaciones de derechos",
            OperationId = "VerificacionDerechos.getObtenerVerificacionDerechoResultById",
            Tags = new[] { "VerificacionDerechosEndPoints" })]
        public async override Task<ActionResult<Response<IEnumerable<ResultVerificacionDerechoDto>>>> HandleAsync(int ID, CancellationToken cancellationToken = default)
        {
            return await _verificacionDerechosBusinessLogic.ObtenerVerificacionDerechoResultById(ID);
        }
    }
}
