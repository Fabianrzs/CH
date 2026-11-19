using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using GA2.Domain.Entities;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    [Authorize]

    public class ActualizarComunicacionesSolicitud :
         BaseAsyncEndpoint.WithRequest<IEnumerable<ComunicacionSolicitudDto>>
        .WithResponse<Response<IEnumerable<ComunicacionSolicitudDto>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public ActualizarComunicacionesSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/ActualizarComunicacionesSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ComunicacionSolicitudDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar comunicaciones solicitud",
           Description = "Actualizar comunicaciones solicitud",
           OperationId = "Solicitud.InsertarComunicacionesSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ComunicacionSolicitudDto>>>> HandleAsync(IEnumerable<ComunicacionSolicitudDto> request, CancellationToken cancellationToken = default)
        {
            var id = request.Select(x => x.IdSolicitud).FirstOrDefault();
            var comunicacionSolicitudes = await _solicitudBusinessLogic.ObtenerComunicacionesSolicitud((Guid)id);
            var comunicacionSolicitud = comunicacionSolicitudes.Data;
            if (comunicacionSolicitud.Any())
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
                        Descripcion = "Actualizar comunicaciones solicitud",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await this._solicitudBusinessLogic.ActualizarComunicacionesSolicitud(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, comunicacionSolicitud, request, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._solicitudBusinessLogic.ActualizarComunicacionesSolicitud(request);
        }
    }
}
