using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints.UsersEndPoint
{
    /// <summary>
    /// UserController
    /// </summary>
    /// <author>Oscar Julian Rojas Garces</author>
    /// <date>24/02/2021</date>
    [Authorize]
    public class DeleteUser : BaseAsyncEndpoint
        .WithRequest<UsuarioDto>.WithResponse<Response<UsuarioDto>>
    {

        /// <summary>
        /// Core Negocio Users
        /// </summary>
        private readonly IUsersBusinessLogic _usersBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Controlador Users
        /// </summary>
        /// <param name="usersBusinessLogic"></param>
        public DeleteUser(IUsersBusinessLogic usersBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _usersBusinessLogic = usersBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// Eliminar usuario
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <author>Oscar Julian Rojas Garces</author>
        /// <date>10/03/2021</date>
        /// <returns></returns>
        //[HttpDelete("api/users/{Id}")]
        [HttpPost("api/DeleteUser")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<UsuarioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Eliminar usuario",
           Description = "Elimina el usuario registrado",
           OperationId = "users.delete",
           Tags = new[] { "UsersEndpoint" })]
        public async override Task<ActionResult<Response<UsuarioDto>>> HandleAsync(UsuarioDto request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var user = name + " " + surnNme;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);


            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "ADMINISTRACION DE USUARIO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminacion del usuario registrado",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await this._usersBusinessLogic.DeleteUser(Guid.Parse(request.Id.ToString()));
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._usersBusinessLogic.DeleteUser(Guid.Parse(request.Id.ToString()));
        }
    }
}
