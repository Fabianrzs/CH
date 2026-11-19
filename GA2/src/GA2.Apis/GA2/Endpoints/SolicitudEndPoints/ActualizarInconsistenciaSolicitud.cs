using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para Actualizar una inconsistencia de solicitud
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>02/06/2022</date>
    [Authorize]
    public class ActualizarInconsistenciaSolicitud :
         BaseAsyncEndpoint.WithRequest<InconsistenciaSolicitudDto>
        .WithResponse<Response<InconsistenciaSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public ActualizarInconsistenciaSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPatch("api/solicitud/ActualizarInconsistenciaSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InconsistenciaSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Inconsistencia",
           Description = "Actualizar Inconsistencia",
           OperationId = "Solicitud.ActualizarInconsistenciaSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<InconsistenciaSolicitudDto>>> HandleAsync(InconsistenciaSolicitudDto actualizarInconsistenciaSolicitud, CancellationToken cancellationToken = default)
        {
            var incosistencias = await _solicitudBusinessLogic.ConsultarInconsistenciaSolicitud((Guid)actualizarInconsistenciaSolicitud.SolicitudId);
            var data = incosistencias.Data.Where(x => x.InconsistenciaId == actualizarInconsistenciaSolicitud.InconsistenciaId).FirstOrDefault();
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
                    var dataResponse = await this._solicitudBusinessLogic.ActualizarInconsistenciaSolicitud(actualizarInconsistenciaSolicitud);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, data, actualizarInconsistenciaSolicitud, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._solicitudBusinessLogic.ActualizarInconsistenciaSolicitud(actualizarInconsistenciaSolicitud);
        }
    }
}
