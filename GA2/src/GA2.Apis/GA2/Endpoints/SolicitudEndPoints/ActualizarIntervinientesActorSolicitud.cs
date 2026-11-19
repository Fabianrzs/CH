using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para actualizar intervinientes
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class ActualizarIntervinientesActorSolicitud :
        BaseAsyncEndpoint.WithRequest<ActualizarIntervinientesActorSolicitudDto>
        .WithResponse<Response<IntervinientesActorSolicitud>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint para actualizar intervinientes
        /// </summary>        
        public ActualizarIntervinientesActorSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }



        /// <summary>
        ///  Actualizar intervinientes
        /// </summary>        
        /// <param name="cancellationToken"></param>   
        /// <param name="request"></param>   
        [HttpPost("api/solicitud/ActualizarIntervinientes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<IntervinientesActorSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar intervinientes",
           Description = "Actualizar intervinientes",
           OperationId = "Solicitud.ActualizarIntervinientes",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IntervinientesActorSolicitud>>> HandleAsync(ActualizarIntervinientesActorSolicitudDto request, CancellationToken cancellationToken = default)
        {
            var intervinientes = await _solicitudBusinessLogic.ConsultarIntervinientesPorId(request.IntervinientesId);
            if(intervinientes.Data != null) {
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
                        Descripcion = "Actualizar intervinientes solicitud",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await this._solicitudBusinessLogic.ActualizarIntervinientes(request);
                    //var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    //await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, intervinientes.Data, dataResponse, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._solicitudBusinessLogic.ActualizarIntervinientes(request); 
        }
    }
}
