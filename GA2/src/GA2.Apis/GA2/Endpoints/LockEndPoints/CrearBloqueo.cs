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
    /// Endpoint para actualizar el Bloqueo
    /// </summary>
    [Authorize]
    public class CrearBloqueo : BaseAsyncEndpoint.WithRequest<BloqueoDto>.WithResponse<Response<BloqueoDto>>
    {
        private readonly IBloqueoBusinessLogic _lockBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockBusinessLogic"></param>
        public CrearBloqueo(IBloqueoBusinessLogic lockBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _lockBusinessLogic = lockBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/bloqueo/CrearBloqueo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<BloqueoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene Bloqueo",
            Description = "Agrega Bloqueo",
            OperationId = "Bloqueo.insert",
            Tags = new[] { "lockEndPoint" })]
        public async override Task<ActionResult<Response<BloqueoDto>>> HandleAsync(BloqueoDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "BLOQUEO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion Bloqueo",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _lockBusinessLogic.CrearBloqueo(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _lockBusinessLogic.CrearBloqueo(request);
        }
    }
}