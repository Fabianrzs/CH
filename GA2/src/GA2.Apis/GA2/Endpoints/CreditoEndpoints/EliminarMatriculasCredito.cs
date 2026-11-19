using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Eliminar MatriculasCredito
    /// </summary>
    /// <author>Nelson Redondo Barros</author>
    /// <date>2/07/2023</date>
    [Authorize]
    public class EliminarMatriculasCredito :BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<MatriculaCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
   
        public EliminarMatriculasCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpDelete("api/credito/EliminarMatriculasCreditoSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<MatriculaCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Eliminar Matriculas Credito",
           Description = "Eliminar ActividadFlujo",
           OperationId = "Credito.EliminarMatriculasCreditoSolicitud",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<MatriculaCreditoDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {

            return await this._creditoBusinessLogic.EliminarMatriculasCreditoSolicitud(request);
        }
    }
}

