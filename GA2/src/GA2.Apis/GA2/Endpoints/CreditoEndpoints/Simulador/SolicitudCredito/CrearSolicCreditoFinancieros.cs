using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// Crear Solicitud Credito Financieros
    /// </summary>
    [Authorize]
    public class CrearSolicCreditoFinancieros : BaseAsyncEndpoint.WithRequest<CreditoFinancieroDto>.WithResponse<Response<CreditoFinancieroDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearSolicCreditoFinancieros(ICreditoBusinessLogic creditoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Carga Archivo al blob storage para solicitud credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/crearSolicCreditoFinancieros")]
        [ProducesResponseType(typeof(Response<CreditoFinancieroDto>), StatusCodes.Status200OK)]
        [SwaggerOperation("Crear Solicitud Credito Financieros",
            Description = "Crear Solicitud Credito Financieros",
            OperationId = "credito.CrearSolicCreditoFinancieros",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<CreditoFinancieroDto>>> HandleAsync(CreditoFinancieroDto request, CancellationToken cancellationToken = default)
        {
            //GUARDAR
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                return await _creditoBusinessLogic.CrearSolicCreditoFinancieros(request);
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
                    Descripcion = "Guardar Informacion  Solicitud Credito Financieros",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _creditoBusinessLogic.CrearSolicCreditoFinancieros(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {
                throw;
            }
            return await _creditoBusinessLogic.CrearSolicCreditoFinancieros(request);
        }
    }
}
