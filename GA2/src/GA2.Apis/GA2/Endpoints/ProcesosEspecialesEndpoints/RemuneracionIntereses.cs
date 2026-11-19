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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para Remuneracion Intereses
    /// <author>Cesar Hernandez</author>
    /// <date>14/03/2023</date>
    /// </summary>
    [Authorize]
    public class RemuneracionIntereses : BaseAsyncEndpoint.WithRequest<RemuneracionInteresDto>.WithResponse<Response<bool>>
    {
        
        private readonly IProcesosEspecialesBusinessLogic _ProcesosEspecialesBusinessLogic;
        /// <summary>
        /// AfiliadoReintegroParamDto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ProcesosEspeciales/LiquidacionIntereses")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Liquidacion de Intereses",
           Description = "Liquidacion de Intereses",
           OperationId = "",
           Tags = new[] { "ParametrosEspecialesEndpoint" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(RemuneracionInteresDto request, CancellationToken cancellationToken = default)
        {
            return await _ProcesosEspecialesBusinessLogic.GenerarLiquidacionIntereses(request);
        }
    }
}
