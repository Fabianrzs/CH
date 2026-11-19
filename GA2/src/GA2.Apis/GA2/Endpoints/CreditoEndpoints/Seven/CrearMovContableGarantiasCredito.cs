using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Seven
{
    /// <summary>
    /// Crea SCnMcont en seven al momento de la aprobación comite de crédito
    /// </summary>
    public class CrearMovContableGarantiasCredito : BaseAsyncEndpoint
        .WithRequest<RequestGarantiasSeven>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearMovContableGarantiasCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CrearMovContableGarantiasCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<InsertarMovContableResponse>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Integración Seven SCnMcont al momento de insertar la garantia crédito",
           Description = "Ejecutar Mov. Contable en Seven Cierre Diario",
           OperationId = "Credito.CrearMovContableGarantiasCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(RequestGarantiasSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearMovContableGarantiasCredito(request, cancellationToken);
        }
    }
}
