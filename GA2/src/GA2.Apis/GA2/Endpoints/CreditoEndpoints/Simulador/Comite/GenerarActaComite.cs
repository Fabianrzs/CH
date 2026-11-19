using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.Comite
{
    /// <summary>
    /// Generar actas de comite
    /// </summary>
    [Authorize]
    public class GenerarActaComite: BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<FileResult>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public GenerarActaComite(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Aprueba Datos Comite Credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/generarActaComite")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Acta Comite",
            Description = "Generar Acta Comite",
            OperationId = "credito.GenerarActaComite",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<FileResult>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.GenerarActaComite(request);
        }
    }
}
