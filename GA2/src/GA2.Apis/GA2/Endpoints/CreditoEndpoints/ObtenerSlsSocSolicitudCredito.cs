using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{

    /// <summary>
    /// el soc y sls de Simulacion de credito
    /// </summary>
  
    public class ObtenerSlsSocSolicitudCredito : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<SocSlsSimulacionCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Obtiene el soc y sls de Simulacion de credito
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public ObtenerSlsSocSolicitudCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// Obtiene el soc y sls de Simulacion de credito
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerSocSlsSimulacionCredito")]
        [ProducesResponseType(typeof(Response<ValorDesembolsoCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el soc y sls de Simulacion de credito",
            Description = "Obtiene el soc y sls de Simulacion de credito",
            OperationId = "credito.ObtenerSocSlsSimulacionCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<SocSlsSimulacionCreditoDto>>> HandleAsync(Guid IdSolicitud, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerSocSlsSimulacionCredito(IdSolicitud);
        }

    }
}
