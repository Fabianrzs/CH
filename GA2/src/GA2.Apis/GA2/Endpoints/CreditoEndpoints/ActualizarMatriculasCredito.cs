using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Crear Tipo Actividad
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>11/05/2021</date>
    [Authorize]
    public class ActualizarMatriculasCredito : BaseAsyncEndpoint.WithRequest<MatriculaCreditoDto>.WithResponse<Response<IEnumerable<MatriculaCreditoDto>>>     
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ActualizarMatriculasCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/credito/ActualizarMatriculasCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<MatriculaCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Matricula Credito",
           Description = "Actualizar Matricula Credito",
           OperationId = "Credito.ActualizarMatriculasCredito",
           Tags = new[] { "CreditoEndpoint" })]
   
        public async override Task<ActionResult<Response<IEnumerable<MatriculaCreditoDto>>>> HandleAsync(MatriculaCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ActualizarMatriculasCredito(request);
        }
    }
}

