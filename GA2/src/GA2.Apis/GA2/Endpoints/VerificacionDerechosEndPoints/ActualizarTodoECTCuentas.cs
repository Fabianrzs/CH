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
    /// Endpoint para obtener parametrizacion legal tasa
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>20/04/2021</date>
    /// </summary>
   // [Authorize]
    public class ActualizarTodoECTCuentas : BaseAsyncEndpoint
        .WithRequest<VerificacionDerechosPeriodo>
        .WithResponse<Response<ParametrosVerificacionDerechoDto>>
    {

        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="verificacionDerechosBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ActualizarTodoECTCuentas(IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic)
        {
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
        }

        /// <summary>
        /// Obtener parametrizacion de tasas
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> 
        [HttpPatch("api/VerificacionDerechos/ActualizarTodoEctCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ParametrosVerificacionDerechoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualiza el ECT de todas las cuentas despues de aprobarse la Verificacion de derechos",
            Description = "Actualiza el ECT de todas las cuentas despues de aprobarse la Verificacion de derechos",
            OperationId = "VerificacionDerechos.ActualizarTodoEctCuentas",
            Tags = new[] { "VerificacionDerechosEndPoints" })]
        public async override Task<ActionResult<Response<ParametrosVerificacionDerechoDto>>> HandleAsync(VerificacionDerechosPeriodo request, CancellationToken cancellationToken = default)
        {
            return await _verificacionDerechosBusinessLogic.ActualizarTodoECTCuentas(request);
        }
    }
}
