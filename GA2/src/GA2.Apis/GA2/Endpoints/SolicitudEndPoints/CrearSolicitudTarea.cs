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
        /// Endpoint para Crear una solicitud
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>20/10/2021</date>
       // [Authorize]
        public class CrearSolicitudTarea :
             BaseAsyncEndpoint.WithRequest<CrearSolicitudTareaDto>
            .WithResponse<Response<bool>>
        {
            private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        private const string USR_ID = "USR_ID";

        /// <summary>
        /// Constructor del Endpoint Crear solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Erwin Pantoja</author>
        /// <date>20/10/2021</date>
        public CrearSolicitudTarea(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
            {
                _solicitudBusinessLogic = solicitudBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
            }




            /// <summary>
            /// 
            /// </summary>
            /// <param name="crearSolicitudTareaDto"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            [HttpPost("api/solicitud/CrearSolicitudTarea")]
            [Consumes("application/json")]
            [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
            [SwaggerOperation(
               Summary = "Crear Solicitud de una tarea nueva",
               Description = "Crear Solicitud de de una tarea nueva",
               OperationId = "Solicitud.CrearSolicitudTarea",
               Tags = new[] { "SolicitudEndpoint" })]
            public async override Task<ActionResult<Response<bool>>> HandleAsync(CrearSolicitudTareaDto crearSolicitudTareaDto, CancellationToken cancellationToken = default)
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
                    Modulo = "SOLICITUD",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacionSolicitud de una tarea nueva",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._solicitudBusinessLogic.CrearSolicitudTarea(crearSolicitudTareaDto);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, crearSolicitudTareaDto, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._solicitudBusinessLogic.CrearSolicitudTarea(crearSolicitudTareaDto);
            }

        }
    
}
