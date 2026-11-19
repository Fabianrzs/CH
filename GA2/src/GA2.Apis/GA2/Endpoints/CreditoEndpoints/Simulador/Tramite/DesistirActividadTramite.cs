using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.Tramite
{
    /// <summary>
    /// Desistir Actividad Tramite
    /// </summary>
    /// <author>Daniel Fonseca Guerrero</author>
    [Authorize]
    public class DesistirActividadTramite : BaseAsyncEndpoint.WithRequest<ActividadTramiteSolicitudDto>.WithResponse<Response<ActualizaActividadTramiteDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public DesistirActividadTramite(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Rechaza actividad tramite
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPut("api/credito/desistirActividadTramite")]
        [ProducesResponseType(typeof(Response<ActividadTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Desistir Actividad Tramite",
        Description = "DesistirActividadTramite",
            OperationId = "credito.DesistirActividadTramite",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ActualizaActividadTramiteDto>>> HandleAsync(ActividadTramiteSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.DesistirActividadTramite(request);
        }
    }
}
