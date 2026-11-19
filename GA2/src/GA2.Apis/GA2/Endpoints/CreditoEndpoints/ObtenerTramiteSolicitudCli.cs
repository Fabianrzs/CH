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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    [Authorize]

    public class ObtenerTramiteSolicitudCli : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<RespuestaSolicitudObtenerTramiteDto>>
    {
        //RespuestaSolicitudObtenerTramiteDto

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor endponit
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerTramiteSolicitudCli(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Endpoint para realizar el trámite de la solicitud de crédito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/obtenerTramiteSolicitudCli")]
        [ProducesResponseType(typeof(Response<IEnumerable<RespuestaSolicitudObtenerTramiteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
        Summary = "Obtiene Tramite de Solicitud",
            Description = "Obtiene Tramite de Solicitud",
            OperationId = "credito.ObtenerTramiteSolicitudCli",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<RespuestaSolicitudObtenerTramiteDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerTramiteSolicitudCli(request);
        }
    }
}

