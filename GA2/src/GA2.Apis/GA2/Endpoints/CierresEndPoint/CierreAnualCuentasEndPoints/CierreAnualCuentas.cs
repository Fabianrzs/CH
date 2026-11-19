using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{

    [Authorize]
    /// <summary>
    /// CierreAnualEndPoint
    /// </summary>
    public class CierreAnualCuentas : BaseAsyncEndpoint
        .WithRequest<ParametrosCierreAnualDto>
        .WithResponse<Response<bool>>
    {

        /// <summary>
        /// 
        /// </summary>
        public CierreAnualCuentas()
        {
        }


        [HttpPost("api/CierreAnualCuentas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Cierre Anual Cuentas",
            Description = "Generar Cierre Anual Cuentas",
            OperationId = "CierreAnual.CierreAnualCuentas",
            Tags = new[] { "CierreAnualEndpoint" })]


        public override async Task<ActionResult<Response<bool>>> HandleAsync(ParametrosCierreAnualDto request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
