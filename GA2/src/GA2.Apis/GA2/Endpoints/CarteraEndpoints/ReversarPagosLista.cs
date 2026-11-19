//using Ardalis.ApiEndpoints;
//using GA2.Application.Dto.Cartera.Pagos;
//using GA2.Application.Dto;
//using GA2.Domain.Core;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Threading;
//using System;

//namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
//{
//    public class ReversarPagosLista
//    {
//    }
//}



using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// Endpoint para aplicar reversión de un pago
    /// </summary>
    [Authorize]
    public class ReversarPagosLista : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<bool>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// constructor Reversión pago
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ReversarPagosLista(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Endpoint encargado de reversar un pago
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/ReversarPagosLista")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reversar Pagos Lista",
           Description = "Reversar Pago",
           OperationId = "cartera.ReversarPagosLista",
           Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ReversarPagosLista(request);
        }
    }
}
