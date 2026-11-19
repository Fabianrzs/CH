using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class ConsultaSaldosYCartera : BaseAsyncEndpoint.WithRequest<ConsultaSaldosRequest>.WithResponse<Response<IEnumerable<ConsultaSaldosResponse>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ConsultaSaldosYCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Cartera/ConsultaSaldosYCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultaSaldosResponse>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar tipo actor by id",
            Description = "Consulta la excepcion segun el id",
            OperationId = "credito.ConsultarActor",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<ConsultaSaldosResponse>>>> HandleAsync(ConsultaSaldosRequest request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ConsultaSaldosYCartera(request);
        }
    }
}
