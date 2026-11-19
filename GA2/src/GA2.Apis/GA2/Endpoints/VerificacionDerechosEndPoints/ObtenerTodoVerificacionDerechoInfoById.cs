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
    public class ObtenerTodoVerificacionDerechoInfoById : BaseAsyncEndpoint
                                                .WithRequest<int>
                                                .WithResponse<Response<IEnumerable<InfoVerificacionDerechoDto>>>
    {

        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="verificacionDerechosBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ObtenerTodoVerificacionDerechoInfoById(IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic)
        {
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
        }

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/VerificacionDerechos/ObtenerTodoVerificacionDerechoInfoById")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InfoVerificacionDerechoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener La Informacion detallada de una verificacion de derecho",
           Description = "Obtiene todos las Verificaciones de derechos por Id",
           OperationId = "VerificacionDerechos.getObtenerTodoVerificacionDerechoInfoById",
           Tags = new[] { "VerificacionDerechosEndPoints" })]
        public async override Task<ActionResult<Response<IEnumerable<InfoVerificacionDerechoDto>>>> HandleAsync(int ID, CancellationToken cancellationToken = default)
        {
            return await _verificacionDerechosBusinessLogic.ObtenerTodoVerificacionDerechoInfoById(ID);
        }
    }
}
