using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para insertar las inconsistencias en la carta de inconsistencias 
    /// </summary>
    public class InsertarInconsistenciaDescripcionSolicitud : BaseAsyncEndpoint.WithRequest<ConsultarRadicadoSolicitudDto>
        .WithResponse<Response<ConsultarRadicadoSolicitudDto>>
    {


        ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor para insertar las inconsistencias en la carta de inconsistencias 
        /// </summary>
        public InsertarInconsistenciaDescripcionSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// EndPoint para insertar las inconsistencias en la carta de inconsistencias
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/InsertarInconsistenciaDescripcionSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<RespuestaConsultarRadicadoSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Insertar Inconsistencia en la carta",
           Description = "Insertar Inconsistencia en la carta",
           OperationId = "Solicitud.InsertarInconsistenciaDescripcionSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<ConsultarRadicadoSolicitudDto>>> HandleAsync(ConsultarRadicadoSolicitudDto request, CancellationToken cancellationToken = default)
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
                    Descripcion = "Guardar informacion Inconsistencia en la carta",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _solicitudBusinessLogic.InsertarDescripcionInconsistenciaSolicitud(request);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _solicitudBusinessLogic.InsertarDescripcionInconsistenciaSolicitud(request); 
        }
    }
}
