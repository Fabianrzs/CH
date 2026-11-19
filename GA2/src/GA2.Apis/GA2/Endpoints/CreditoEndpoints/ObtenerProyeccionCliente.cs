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

namespace GA2.Apis.GA2.Endpoints
{
    public class ObtenerProyeccionCliente : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ProyeccionClienteDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;


        public ObtenerProyeccionCliente(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
/// <inheritdoc/>


        [Authorize]
        /// <summary>
        /// Obtiene todos seguros por ubicacion
        /// <param name="request"></param>
        /// <author>nelson redondo</author>
        /// <date>12/12/2022</date>
        /// <returns>registro del cargo para la proyeccion</returns>
        [HttpGet("api/ObtenerProyeccionCliente")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProyeccionClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "registro del cargo para la proyeccion",
           Description = "registro del cargo para la proyeccion",
           OperationId = "credito.ObtenerProyeccionCliente",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ProyeccionClienteDto>>> HandleAsync(string request,CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerProyeccionCliente(request);
        }
    }
}

