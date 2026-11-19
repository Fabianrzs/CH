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
    public class CrearSolicitud :
         BaseAsyncEndpoint.WithRequest<CrearSolicitudDto>
        .WithResponse<Response<ObtenerTramiteSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Crear solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Erwin Pantoja</author>
        /// <date>20/10/2021</date>
        public CrearSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }



        /// <summary>
        /// EndPoint para crear una solicitud
        /// </summary>
        /// <param name="crearSolicitudDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/crearsolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Solicitud",
           Description = "Crear Solicitud",
           OperationId = "Solicitud.crearsolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<ObtenerTramiteSolicitudDto>>> HandleAsync(CrearSolicitudDto crearSolicitudDto, CancellationToken cancellationToken = default)
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
                    Modulo = "",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion solicitud ",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._solicitudBusinessLogic.CrearSolicitud(crearSolicitudDto);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, crearSolicitudDto, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._solicitudBusinessLogic.CrearSolicitud(crearSolicitudDto);
        }
    }
}
