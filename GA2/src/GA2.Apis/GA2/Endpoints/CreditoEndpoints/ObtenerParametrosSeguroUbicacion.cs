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

    public class ObtenerParametrosSeguroUbicacion : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ParametrosSeguroUbicacionDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerParametrosSeguroUbicacion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }



        /// <summary>
        /// Obtiene todos seguros por ubicacion
        /// </summary>
        /// <author>nelson redondo</author>
        /// <date>12/12/2022</date>
        /// <returns>Lista de seguros por region</returns>
        [HttpGet("api/ObtenerParametrosSeguroUbicacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ParametrosSeguroUbicacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Lista de seguros por region",
           Description = "Lista de seguros por region",
           OperationId = "credito.ObtenerParametrosSeguroUbicacion",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ParametrosSeguroUbicacionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerParametrosSeguroUbicacion();
        }
    }
}

