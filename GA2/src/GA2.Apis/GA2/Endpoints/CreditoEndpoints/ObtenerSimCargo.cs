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
    public class ObtenerSimCargo : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<ProyeccionClienteDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerSimCargo(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [Authorize]

        /// <summary>
        /// Obtiene todos seguros por ubicacion
        /// <param name="request"></param>
        /// <author>nelson redondo</author>
        /// <date>12/12/2022</date>
        /// <returns>Listado de los cargos para la proyeccion</returns>
        [HttpGet("api/ObtenerSimCargo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProyeccionClienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Listado de los cargos para la proyeccion",
           Description = "Listado de los cargos para la proyeccion",
           OperationId = "credito.ObtenerSimCargo",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ProyeccionClienteDto>>>> HandleAsync(string request,CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerSimCargo(request);
        }
    }
}

