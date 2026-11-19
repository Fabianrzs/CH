using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
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
    public class UpdateUser : BaseAsyncEndpoint
        .WithRequest<UsuarioDto>
        .WithResponse<Response<UsuarioDto>>
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
        public UpdateUser(IUsersBusinessLogic usersBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _usersBusinessLogic = usersBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Actualizar usuario
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>User actualizado</returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/UpdateUser")]
        [ProducesResponseType(typeof(Response<UsuarioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar usuario",
           Description = "Actualiza el usuario registrado",
           OperationId = "users.update",
           Tags = new[] { "UsersEndpoint" })]
        public override async Task<ActionResult<Response<UsuarioDto>>> HandleAsync(UsuarioDto request, CancellationToken cancellationToken = default)
        {
            var users = await _usersBusinessLogic.GetUsers();
            var data = users.Data.Where(x => x.Id == request.Id).FirstOrDefault();  
            if (data != null)
            {
                    var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
                    var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
                    var usrId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
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
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizacion de usuario registrado",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(usrId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await this._usersBusinessLogic.UpdateUser(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, data, request, usrId);
                    return dataResponse;

                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._usersBusinessLogic.UpdateUser(request);
        }
    }
}
