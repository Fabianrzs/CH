using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using GA2.Application.Dto.Escrituras;
using GA2.Domain.Core.Escrituras;


namespace GA2.Apis.GA2.Endpoints.EscriturasEndPoints
{
    [Authorize]
    public class ObtenerEscritura:BaseAsyncEndpoint.WithRequest<RequestObtenerEscrituras>.WithResponse<Response<ObtenerEscrituras>>
    {
        private readonly IEscrituraBusinessLogic _escrituraBusinessLogic;

        public ObtenerEscritura(IEscrituraBusinessLogic escrituraBusinessLogic)
        {
            _escrituraBusinessLogic = escrituraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/ObtenerEscritura")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerEscrituras>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener escrituras",
            Description = "Obtener escrituras",
            OperationId = "escritura.ObtenerEscritura",
            Tags = new[] { "EscrituraEndpoint" })]
        public override async Task<ActionResult<Response<ObtenerEscrituras>>> HandleAsync(RequestObtenerEscrituras request, CancellationToken cancellationToken = default)
        {
            return await this._escrituraBusinessLogic.ObtenerEscritura(request);
        }
    }
}
