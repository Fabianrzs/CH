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
using Microsoft.AspNetCore.Authorization;
using GA2.Domain.Entities;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    [Authorize]
    public class InsertarAuditoriaNovedades :
        BaseAsyncEndpoint.WithRequest<AuditoriaNovedadesDto>.WithResponse<Response<AuditoriaNovedadesDto>>
    {
        private readonly IAuditoriaNovedadesBussinessLogic _auditoriaNovedadesBussinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public InsertarAuditoriaNovedades(IAuditoriaNovedadesBussinessLogic auditoriaNovedadesBussinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _auditoriaNovedadesBussinessLogic = auditoriaNovedadesBussinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cronograma/InsertarAudoriaNovedades")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<AuditoriaNovedadesDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "InsertarAudoriaNovedades",
           Description = "InsertarAudoriaNovedades",
           OperationId = "AuditoriaNovedades.InsertarAudoriaNovedades",
           Tags = new[] { "AuditoriaNovedadesEndpoint" })]
        public async override Task<ActionResult<Response<AuditoriaNovedadesDto>>> HandleAsync(AuditoriaNovedadesDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "CRONOGRAMA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de auditoria de novedades",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _auditoriaNovedadesBussinessLogic.InsertarAuditoriaNovedades(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}
