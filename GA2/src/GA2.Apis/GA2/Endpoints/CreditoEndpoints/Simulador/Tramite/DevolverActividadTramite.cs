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
    /// Devolver Actividad Tramite
    /// </summary>
    /// <author>Daniel Fonseca Guerrero</author>
    [Authorize]
    public class DevolverActividadTramite : BaseAsyncEndpoint.WithRequest<DevolverActividadTramiteDto>.WithResponse<Response<ActualizaActividadTramiteDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public DevolverActividadTramite(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Devolver Actividad Tramite
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [HttpPut("api/credito/devolverActividadTramite")]
        [ProducesResponseType(typeof(Response<ActualizaActividadTramiteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Devolver Actividad Tramite",
        Description = "DevolverActividadTramite",
            OperationId = "credito.DevolverActividadTramite",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ActualizaActividadTramiteDto>>> HandleAsync(DevolverActividadTramiteDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.DevolverActividadTramite(request);
        }
    }
}
