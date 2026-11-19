using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    public class EditarNovedadAfiliado :
         BaseAsyncEndpoint.WithRequest<NovedadesAfiliadoDto>.WithResponse<Response<NovedadesAfiliadoDto>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public EditarNovedadAfiliado(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cronograma/EditarNovedadAfiliado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CronogramaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "EditarNovedadAfiliado",
           Description = "EditarNovedadAfiliado",
           OperationId = "Cronograma.EditarNovedadAfiliado",
           Tags = new[] { "EditarNovedadAfiliado" })]
        public async override Task<ActionResult<Response<NovedadesAfiliadoDto>>> HandleAsync(NovedadesAfiliadoDto request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CRONOGRAMA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "actualizar novedades afilia.",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var novedadAdiliado = await _cronogramaBusinessLogic.ObtenerNovedadAfiliadoPorId(request.IdNovedad);
                var response = await this._cronogramaBusinessLogic.EditarNovedadAfiliado(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, novedadAdiliado.Data, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._cronogramaBusinessLogic.EditarNovedadAfiliado(request);
        }
    }
}
