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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    /// <summary>
    /// Enpoint para crear simulacion del cliente
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
    [Authorize]
    public class ObtenerMatriculasCreditoporSolicitud : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<MatriculaCreditoDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        public ObtenerMatriculasCreditoporSolicitud(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// crea simulacion del cliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("/api/prospeccion/ObtenerMatriculasCreditoporSolicitud")]
        [ProducesResponseType(typeof(Response<IEnumerable<MatriculaCreditoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation("Crear Simulacion Cliente",
            Description = "Crear Simulacion Clienteo",
            OperationId = "prospeccion.ObtenerMatriculasCreditoporSolicitud",
            Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<MatriculaCreditoDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerMatriculasCreditoporSolicitud(request);
        }

    }
}
