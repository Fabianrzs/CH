using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.Credito.WorkManager;
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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.LegalizacionDesembolso
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class CrearSolicCreditoDesembolsos : BaseAsyncEndpoint.WithRequest<IEnumerable<SolicitudCreditosDesembolsosDto>>.WithResponse<Response<IEnumerable<SolicitudCreditosDesembolsosDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearSolicCreditoDesembolsos(ICreditoBusinessLogic creditoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Aprueba actividad tramite
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/crearSolicCreditoDesembolsos")]
        [ProducesResponseType(typeof(Response<IEnumerable<SolicitudCreditosDesembolsosDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear Solic Credito Desembolsos",
            Description = "Crear Solic Credito Desembolsos",
            OperationId = "credito.CrearSolicCreditoDesembolsos",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<SolicitudCreditosDesembolsosDto>>>> HandleAsync(IEnumerable<SolicitudCreditosDesembolsosDto> request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                return await this._creditoBusinessLogic.CrearSolicCreditoDesembolsos(request);
            }

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
                    Descripcion = "Guardar Informacion Solicitud Credito Desembolsos",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._creditoBusinessLogic.CrearSolicCreditoDesembolsos(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {
                throw;
            }
            return await this._creditoBusinessLogic.CrearSolicCreditoDesembolsos(request);
        }
    }
}
