using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.Identity.EndPoints.LoginEndPoint
{
    public class UpdateSession : BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<Response<bool>>
    {
        /// <summary>
        /// Inyeccion de negocio usuarios
        /// </summary>
        private readonly ILoginBusinessLogic _loginBusinessLogic;

        /// <summary>
        /// Controlador Login
        /// </summary>
        /// <param name="loginBusinessLogic"></param>
        public UpdateSession(ILoginBusinessLogic loginBusinessLogic)
        {
            this._loginBusinessLogic = loginBusinessLogic;
        }

        /// <summary>
        /// LoginEndPoint
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/UpdateSession")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            summary: "Valida el toquen de restablecimiento de contraseña",
            description: "Valida el toquen de restablecimiento de contraseña",
            OperationId = "login.tokenrestablecimiento",
            Tags = new[] { "LoginEndpoint" }
            )]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(string idUsuario, CancellationToken cancellationToken = default)
        {
            return await this._loginBusinessLogic.UpdateSession(idUsuario);
        }
    }
}
