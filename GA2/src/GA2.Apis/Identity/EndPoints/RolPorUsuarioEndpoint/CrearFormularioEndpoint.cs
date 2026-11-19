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
    /// Endpoint Rol por usuario
    /// </summary>
    [Authorize]
    public class CrearFormularioEndpoint : BaseAsyncEndpoint
        .WithRequest<FormularioDto>.WithResponse<Response<FormularioDto>>
    {
        private readonly IFormularioBusinessLogic _formularioBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formularioBusinessLogic"></param>
        public CrearFormularioEndpoint(IFormularioBusinessLogic formularioBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _formularioBusinessLogic = formularioBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost]
        [Route("roles/CrearFormularioEndpoint")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<FormularioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Crear un formulario para roles",
          Description = "Crear un formulario",
          OperationId = "roles.formulariocrear",
          Tags = new[] { "RolesEndpoint" })]
        public override async Task<ActionResult<Response<FormularioDto>>> HandleAsync(FormularioDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "GESTION DE USUARIO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Crear un formulario para roles",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _formularioBusinessLogic.CrearFormulario(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _formularioBusinessLogic.CrearFormulario(request);
        }
    }
}
