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

namespace GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// Crear solicitud Credito informacion Juridica
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
    //[Authorize]
    public class CrearSolicCreditoInfJuridica : BaseAsyncEndpoint.WithRequest<SolicCreditoInfJuridicaDto>.WithResponse<Response<SolicCreditoInfJuridicaDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearSolicCreditoInfJuridica(ICreditoBusinessLogic creditoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Crea Solicitud Credito Informacion Juridica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/crearSolicCreditoInfJuridica")]
        [ProducesResponseType(typeof(Response<SolicCreditoInfJuridicaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear Solicitud Credito Informacion Juridica",
            Description = "Crear Solicitud Credito Informacion Juridica",
            OperationId = "credito.CrearSolicCreditoInfJuridica",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<SolicCreditoInfJuridicaDto>>> HandleAsync(SolicCreditoInfJuridicaDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                return await this._creditoBusinessLogic.CrearSolicCreditoInfJuridica(request);
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
                    Descripcion = "Guardar Informacion Solicitud Credito Informacion Juridica",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._creditoBusinessLogic.CrearSolicCreditoInfJuridica(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {
                throw;
            }
            return await this._creditoBusinessLogic.CrearSolicCreditoInfJuridica(request);
        }
    }
}
