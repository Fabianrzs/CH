using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.Identity.EndPoints.OpcionesTemporales
{
    public class InsertarOpcionesTemporalesUsuario :
        BaseAsyncEndpoint.WithRequest<OpcionTemporalUsuarioDto>
        .WithResponse<Response<OpcionTemporalUsuarioDto>>
    {

        private readonly IOpcionesTemporalesBusinessLogic _opcionesTemporalesBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opcionesTemporalesBusinessLogic"></param>
        public InsertarOpcionesTemporalesUsuario(IOpcionesTemporalesBusinessLogic opcionesTemporalesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _opcionesTemporalesBusinessLogic = opcionesTemporalesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Endpoint Menus dtos
        /// </summary>
        /// <param name="cancellationToken">Cancelacion Token</param>
        /// <returns></returns>
        [HttpPost("api/InsertarOpcionesTemporalesUsuario")]
        [ProducesResponseType(typeof(Response<OpcionTemporalUsuarioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Insertar opciones temporales",
           Description = "Insertar opciones temporales",
           OperationId = "roles.InsertarOpcionesTemporalesUsuario",
           Tags = new[] { "RolesEndpoint" })]
        public async override Task<ActionResult<Response<OpcionTemporalUsuarioDto>>> HandleAsync(OpcionTemporalUsuarioDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "GESTION DE USUARIO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar opciones temporale",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._opcionesTemporalesBusinessLogic.InsertarOpcionesTemporalesUsuario(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._opcionesTemporalesBusinessLogic.InsertarOpcionesTemporalesUsuario(request);
        }
    }
}
