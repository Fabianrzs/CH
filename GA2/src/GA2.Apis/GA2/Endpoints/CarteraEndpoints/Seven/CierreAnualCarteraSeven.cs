
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Seven
{
    public class CierreAnualCarteraSeven : BaseAsyncEndpoint
        .WithRequest<RequestCierreAnualCartera>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CierreAnualCarteraSeven(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Cierre Mensual Seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/CierreAnualCarteraSeven")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cierre anual de los diferentes abonos en Seven",
           Description = "Cierre anual en seven",
           OperationId = "Cartera.CierreAnualCarteraSeven",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(RequestCierreAnualCartera request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.CierreAnualCarteraSeven(request, cancellationToken);
        }
    }
}
