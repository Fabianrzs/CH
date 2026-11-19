using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Clase del Endpoint para Actualizar una inconsistencia de solicitud
    /// </summary>
    public class ActualizarInconsistenciasSeguridadSolicitud :
         BaseAsyncEndpoint.WithRequest<InconsistenciaSeguridadDto>
        .WithResponse<Response<InconsistenciaSeguridadDto>>
    {


        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        public ActualizarInconsistenciasSeguridadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// EndPoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="actualizarInconsistenciaSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/solicitud/ActualizarInconsistenciaSeguridadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InconsistenciaSeguridadDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Inconsistencia",
           Description = "Actualizar Inconsistencia",
           OperationId = "Solicitud.ActualizarInconsistenciaSeguridadSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<InconsistenciaSeguridadDto>>> HandleAsync(InconsistenciaSeguridadDto actualizarInconsistenciaSolicitud, CancellationToken cancellationToken = default)
        {
            var inconsistencias = await _solicitudBusinessLogic.ConsultarInconsistenciaSeguridadSolicitud((Guid)actualizarInconsistenciaSolicitud.Solicitudid);
            var data = inconsistencias.Data.Where(x => x.InconsistenciaId == actualizarInconsistenciaSolicitud.InconsistenciaId.ToString()).FirstOrDefault();
            if (data is not null)
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
                        Modulo = "GESTION SOLICITUDES",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizar Inconsistencia solicitud",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await _solicitudBusinessLogic.ActualizarInconsistenciaSeguridadSolicitud(actualizarInconsistenciaSolicitud);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse.Data, actualizarInconsistenciaSolicitud, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await _solicitudBusinessLogic.ActualizarInconsistenciaSeguridadSolicitud(actualizarInconsistenciaSolicitud); 
        }
    }
}
