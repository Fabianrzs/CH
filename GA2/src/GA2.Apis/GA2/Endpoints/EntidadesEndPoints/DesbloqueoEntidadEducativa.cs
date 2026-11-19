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
    /// Endpoint para Desbloqueo de Entidad Educativa
    /// </summary>
    /// <author>Edwin Lopez</author>
    /// <date>15/03/2022</date>
    
    [Authorize]
    public class DesbloqueoEntidadEducativa :
         BaseAsyncEndpoint.WithRequest<BloqueoEntidadEducativaDto>
        .WithResponse<Response<BloqueoEntidadEducativaDto>>
    {

        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint DesbloqueoEntidadEducativa
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public DesbloqueoEntidadEducativa(IEntidadesBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// DesbloqueoEntidadEducativa
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Entidades/DesbloqueoEntidadEducativa")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<BloqueoEntidadEducativaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Desbloqueo Entidad Educativa",
            Description = "Desbloqueo Entidad Educativa",
            OperationId = "Entidades.DesbloqueoEntidadEducativa",
            Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<BloqueoEntidadEducativaDto>>> HandleAsync(BloqueoEntidadEducativaDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "ENTIDADES",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion Desbloqueo Entidad Educativ",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._catalogsBusinessLogic.DesbloqueoEntidadEducativa(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._catalogsBusinessLogic.DesbloqueoEntidadEducativa(request);
        }

    }
}

