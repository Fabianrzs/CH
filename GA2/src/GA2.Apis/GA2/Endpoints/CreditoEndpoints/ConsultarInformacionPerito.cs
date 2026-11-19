using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{


    namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
    {
        [Authorize]
        public class ConsultarInformacionPerito : BaseAsyncEndpoint
            .WithRequest<string>
            .WithResponse<Response<IEnumerable<InformacionPerito>>>
        {
            private readonly ICreditoBusinessLogic _creditoBusinessLogic;

            public ConsultarInformacionPerito(
                ICreditoBusinessLogic creditoBusinessLogic)
            {
                _creditoBusinessLogic = creditoBusinessLogic;
            }

            [HttpGet("api/Credito/ConsultarInformacionPerito/{usuarioPerito}")]
            [ProducesResponseType(typeof(Response<IEnumerable<InformacionPerito>>), StatusCodes.Status200OK)]
            [SwaggerOperation(
                Summary = "Consultar solicitudes trámite perito",
                Description = "Permite consultar las solicitudes asignadas a un perito.",
                OperationId = "credito.ConsultarInformacionPerito",
                Tags = new[] { "CreditoEndpoint" })]
            public override async Task<ActionResult<Response<IEnumerable<InformacionPerito>>>> HandleAsync(
                [FromRoute] string usuarioPerito,
                CancellationToken cancellationToken = default)
            {
                var dataResponse = await _creditoBusinessLogic.ConsultarSolicitudesTramitePerito(usuarioPerito);

                return Ok(dataResponse);
            }
        }
    }
}