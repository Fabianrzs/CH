using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    [Authorize]
    public class CrearNoSolicitudCredito :BaseAsyncEndpoint.WithRequest<ConsultaSolicitudCreditoDto>.WithResponse<Response<ConsultaSolicitudCreditoDto>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearNoSolicitudCredito(ICreditoBusinessLogic creditoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/CrearNoSolicitudCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ConsultaSolicitudCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Crear No Solicitud Credito",
           Description = "Crear Crear No Solicitud Credito",
           OperationId = "Credito.CrearNoSolicitudCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ConsultaSolicitudCreditoDto>>> HandleAsync(ConsultaSolicitudCreditoDto request, CancellationToken cancellationToken = default)
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
                        Descripcion = "Guardar Informacion  No Solicitud Credito",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await this._creditoBusinessLogic.CrearNoSolicitudCredito(request);
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
                        Descripcion = "Guardar Informacion  No Solicitud Credito",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await this._creditoBusinessLogic.CrearNoSolicitudCredito(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }

            return await this._creditoBusinessLogic.CrearNoSolicitudCredito(request);
        }
    }
}
