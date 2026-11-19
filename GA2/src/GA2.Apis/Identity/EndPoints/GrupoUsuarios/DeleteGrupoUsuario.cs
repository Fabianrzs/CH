
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

    public class DeleteGrupoUsuario : BaseAsyncEndpoint
        .WithRequest<GruposUsuariosDto>.WithResponse<Response<GruposUsuariosDto>>
    {

        private readonly IGruposUsuariosLogic _gruposusuariosLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public DeleteGrupoUsuario(IGruposUsuariosLogic gruposusuariosLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPost("api/DeleteGrupoUsuario")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<GruposUsuariosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Elimina un grupo de usuario",
           Description = "Elimina un grupo de usuario",
           OperationId = "grupos.delete",
           Tags = new[] { "GrupoUsuariosEndpoint" })]
        public override async Task<ActionResult<Response<GruposUsuariosDto>>> HandleAsync(GruposUsuariosDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
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
                    Descripcion = "Elimina grupo de usuario",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await this._gruposusuariosLogic.DeleteGrupoUsuario(request, userId);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._gruposusuariosLogic.DeleteGrupoUsuario(request, userId);
           
        }
    }
}
