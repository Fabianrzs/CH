using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints
{
    public class CrearPostulanteEndPoint : BaseAsyncEndpoint.WithRequest<SolicitudPostulacionDto>
        .WithResponse<Response<SolicitudPostulacionDto>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public CrearPostulanteEndPoint(IPostulacionBussineLogic bussigneLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        [HttpPost("api/postulante/crearSolicitdPostulante")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear la solicitud de postulacion",
            Description = "Crear la solicitud de postulacion",
            OperationId = "postulante.crearSolicitdPostulante",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicitudPostulacionDto>>> HandleAsync(SolicitudPostulacionDto request, CancellationToken cancellationToken = default)
        {;

            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var user = name + " " + surnNme;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);

            request.UserId = Guid.Parse(userId);
            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "FONDO SOLIDARIDAD",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion solicitud de postulacio",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _postulacionBussineLogic.CrearSolicitudPostante(request);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _postulacionBussineLogic.CrearSolicitudPostante(request);
        }
    }
}
