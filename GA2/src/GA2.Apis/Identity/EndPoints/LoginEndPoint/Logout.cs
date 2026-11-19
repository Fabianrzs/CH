using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// LoginController
    /// </summary>
    [AllowAnonymous]
    public class Logout : BaseAsyncEndpoint
        .WithRequest<string>
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
        public Logout(ILoginBusinessLogic loginBusinessLogic)
        {
            this._loginBusinessLogic = loginBusinessLogic;
        }

        /// <summary>
        /// LoginEndPoint
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/logout")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            summary: "Login user into app",
            description: "Login to users",
            OperationId = "login.login",
            Tags = new[] { "LoginEndpoint" }
            )]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(string idUsuario, CancellationToken cancellationToken = default)
        {
            return await this._loginBusinessLogic.Logout(idUsuario);
        }
    }
}
