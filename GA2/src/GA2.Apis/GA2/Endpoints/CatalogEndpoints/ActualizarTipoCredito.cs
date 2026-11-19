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

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Actualizar Tipo Credito
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>11/05/2021</date>
    [Authorize]
    public class ActualizarTipoCredito :
         BaseAsyncEndpoint.WithRequest<TipoCreditoDto>
        .WithResponse<Response<TipoCreditoDto>>
    {

        private readonly ICatalogosBusinessLogic _tipocreditoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Actualizar Tipo Credito
        /// </summary>
        /// <param name="tipocreditoBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ActualizarTipoCredito(ICatalogosBusinessLogic tipocreditoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _tipocreditoBusinessLogic = tipocreditoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/ActualizarTipoCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TipoCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Tipo Credito",
            Description = "Actualizar Tipo Credito",
            OperationId = "catalogos.ActualizarTipoCredito",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<TipoCreditoDto>>> HandleAsync(TipoCreditoDto request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CATALOGO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizacion de Tipo Credito",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var tiposCreditos = await _tipocreditoBusinessLogic.ObtenerTipoCredito();
                var tipoCredito = tiposCreditos.Data.Where(x => x.TCR_ID == request.TCR_ID).FirstOrDefault();
                var response = await this._tipocreditoBusinessLogic.ActualizarTipoCredito(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, tipoCredito, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._tipocreditoBusinessLogic.ActualizarTipoCredito(request);
        }
    }
}

