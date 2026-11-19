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
    [Authorize]

    public class ObtenerPeritos : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<UsuarioDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerPeritos(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }



        /// <summary>
        /// Obtiene todos los usuarios con Rol perito
        /// </summary>
        /// <author>Jefer Gomez</author>
        /// <date>29/09/2022</date>
        /// <returns>Lista de usuarios col Rol de perito</returns>
        [HttpGet("api/ObtenerPeritos")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<UsuarioDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Lista de usuarios col Rol de perito",
           Description = "Lista de usuarios col Rol de perito",
           OperationId = "users.perito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<UsuarioDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerPeritos();
        }
    }
}
