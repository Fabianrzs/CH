using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito.Simulador.Comite;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.Comite
{
    [Authorize]
    public class EliminarIntegrateComiteCredito : BaseAsyncEndpoint.WithRequest<long>.WithResponse<Response<bool>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public EliminarIntegrateComiteCredito(ICreditoBusinessLogic creditoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/credito/EliminarIntegranteComiteCredito")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Eliminar Integrante Comite Credito",
            Description = "Eliminar Integrante Comite Credito",
            OperationId = "credito.EliminarIntegranteComiteCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(long Id_Integrante, CancellationToken cancellationToken = default)
        {
            var integrantes = await _creditoBusinessLogic.ObtenerIntegranteComite();
            var integrante = integrantes.Data.Where(x => x.idIntegranteComite == Id_Integrante).FirstOrDefault();
            if (integrante is not null)
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
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 7,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Eliminar Integrante Comite Credito",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "ELIMINAR"

                    };
                    var dataResponse = await this._creditoBusinessLogic.EliminarIntegranteComiteCredito(Id_Integrante);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, null, integrante, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }

            return await this._creditoBusinessLogic.EliminarIntegranteComiteCredito(Id_Integrante);
        }
    }
}
