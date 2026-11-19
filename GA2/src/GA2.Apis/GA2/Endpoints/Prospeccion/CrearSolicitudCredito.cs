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

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class CrearSolicitudCredito:BaseAsyncEndpoint.WithRequest<SolicitudCreditoDto>.WithResponse<Response<SolicitudCreditoDto>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        public CrearSolicitudCredito(IProspeccionBusinessLogic prospeccionBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        ///  Crea Preaprovado
        /// </summary>
        /// <param name = "request" > Peticion usuario</param>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        [HttpPost("api/prospeccion/CrearSolicitudCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SolicitudCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear simulacion Credito",
           Description = "Crea simulacion credito",
           OperationId = "prospeccion.CrearSolicitudCredito",
           Tags = new[] { "ProspeccionEndpoint" })]

        public override async Task<ActionResult<Response<SolicitudCreditoDto>>> HandleAsync(SolicitudCreditoDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                var userId = "83E91A3C-02A3-4D49-995E-87154620F98D";
                var user = "AFILIADO";
                var ip = _auditoriaBusinessLogic.Ip();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
                try
                {
                    var requestAuditoria = new AuditoriaDto()
                    {
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar informacion simulacion credito",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _prospeccionBusinessLogic.CrearSolicitudCredito(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            else
            {
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
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar Informacion simulacion credito",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _prospeccionBusinessLogic.CrearSolicitudCredito(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }
            return await _prospeccionBusinessLogic.CrearSolicitudCredito(request);
        }
    }
}
