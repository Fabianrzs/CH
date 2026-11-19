using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using Microsoft.AspNetCore.Authorization;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Seven
{
    public class AnulacionLista: BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<AnularCnMcontResponse>>
    {
        ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public AnulacionLista(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Anular Cuota en seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>AnularCuotaCartera</returns>
        [HttpPost("api/cartera/AnulacionLista")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<AnularCnMcontResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Anule los movimientos contables aplicacion de pagos",
           Description = "Anula los movimientos contables en seven aplicacion de pagos",
           OperationId = "Cartera.AnulacionLista",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<AnularCnMcontResponse>>> HandleAsync(int papId, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.AnulacionLista(papId, cancellationToken);
        }
    }
}
