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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para actualizar el Notificacion
    /// <date>17/05/2021</date>
    /// </summary>
    [Authorize]
    public class CrearNotificacion : BaseAsyncEndpoint.WithRequest<NotificacionDto>.WithoutResponse
    {
        private readonly INotificacionBusinessLogic _notificationBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationBusinessLogic"></param>
        public CrearNotificacion(INotificacionBusinessLogic notificationBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _notificationBusinessLogic = notificationBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/notificacion/crearnotificacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<NotificacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Agrega Notificacion",
            Description = "Agrega Notificacion",
            OperationId = "Notificacion.insert",
            Tags = new[] { "NotificationEndPoint" })]
        public async override Task<ActionResult> HandleAsync(NotificacionDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "NOTIFICACION",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion Notificacion",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                await _notificationBusinessLogic.CrearNotificacionClienteId(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return Ok();
            }
            catch (System.Exception e)
            {

                throw;
            }
            await _notificationBusinessLogic.CrearNotificacionClienteId(request);
            return Ok();
        }
    }
}