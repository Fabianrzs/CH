using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.GrupoUsuarios;
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

namespace GA2.Apis.Identity.EndPoints.GrupoUsuarios
{
    [Authorize]

    public class CreateGrupoUsuarios : BaseAsyncEndpoint
        .WithRequest<GruposUsuariosDto>.WithResponse<Response<GruposUsuariosDto>>
    {

        private readonly IGruposUsuariosLogic _gruposusuariosLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CreateGrupoUsuarios(IGruposUsuariosLogic gruposusuariosLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _gruposusuariosLogic = gruposusuariosLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }


        /// <summary>
        ///  Crea un nuevo usuario
        /// </summary>
        /// <param name="request">Peticion usuario</param>
        /// <param name="cancellationToken">cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/CreateGrupoUsuarios")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<GruposUsuariosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crea un nuevo grupo de usuario",
           Description = "Crea un nuevo grupo de usuario",
           OperationId = "grupos.create",
           Tags = new[] { "GrupoUsuariosEndpoint" })]
        public override async Task<ActionResult<Response<GruposUsuariosDto>>> HandleAsync(GruposUsuariosDto request, CancellationToken cancellationToken = default)
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
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de un nuevo grupo de usuario",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._gruposusuariosLogic.CreateGroupUser(request, userId);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._gruposusuariosLogic.CreateGroupUser(request, userId);
        }
    }
}
