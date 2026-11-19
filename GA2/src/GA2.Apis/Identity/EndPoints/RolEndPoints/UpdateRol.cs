using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class UpdateRol :
        BaseAsyncEndpoint.WithRequest<RolDto>
        .WithResponse<Response<RolDto>>
    {

        private readonly IRolBusinessLogic _rolBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rolBusinessLogic"></param>
        public UpdateRol(IRolBusinessLogic rolBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _rolBusinessLogic = rolBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/UpdateRol")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<RolDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza un rol",
           Description = "Actualiza un rol",
           OperationId = "roles.update",
           Tags = new[] { "RolesEndpoint" })]
        public async override Task<ActionResult<Response<RolDto>>> HandleAsync(RolDto request, CancellationToken cancellationToken = default)
        {
            var rol = await _rolBusinessLogic.GetByIdRol(request.Id);
            if (rol != null)
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
                        Descripcion = "Actualizar un rol",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(usrId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await this._rolBusinessLogic.UpdateRol(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, rol.Data, request, usrId);
                    return dataResponse;

                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._rolBusinessLogic.UpdateRol(request);
        }
    }
}
