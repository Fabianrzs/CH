using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    /// <summary>
    /// Constructor del Endpoint actuaalizar ActualizarSeguroPorRegion
    /// </summary>
    /// <author>Nelson Redondo Barros</author>
    /// <date>12/12/2022</date>
    [Authorize]
    public class ActualizarSeguroPorRegion : BaseAsyncEndpoint.WithRequest<ParametroSeguroRegionGuiDto>.WithResponse<Response<IEnumerable<ParametroSeguroRegionGuiDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint actuaalizar ActualizarSeguroPorRegion
        /// </summary>
        /// <author>Nelson Redondo Barros</author>
        /// <date>12/12/2022</date>

        public ActualizarSeguroPorRegion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>

        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("api/ActualizarSeguroPorRegion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ParametroSeguroRegionGuiDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Actualizar SeguroPorRegion",
         Description = "Actualizar SeguroPorRegion enviando el id de la region y la tasa de seguro",
         OperationId = "CreditoEndpoint.ActualizarSeguroPorRegion",
         Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ParametroSeguroRegionGuiDto>>>> HandleAsync(ParametroSeguroRegionGuiDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ActualizarSeguroPorRegion(request);
        }
    }
}
