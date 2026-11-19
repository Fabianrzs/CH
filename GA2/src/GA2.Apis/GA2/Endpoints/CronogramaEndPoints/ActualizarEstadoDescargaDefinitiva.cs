using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.CronogramaActividades;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    /// <summary>
	/// Endpoint para actualizar estado descargas definitivas de archivos de novedades
	/// </summary>
	/// <author>Michel Parra</author>
	/// <date>06/03/2024</date>
    [Authorize]
    public class ActualizarEstadoDescargaDefinitiva :
         BaseAsyncEndpoint.WithRequest<DescargaDefinitivaDto>.WithResponse<Response<IEnumerable<DescargaDefinitivaDto>>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Michel Parra</author>
        /// <date>06/03/2024</date>
        public ActualizarEstadoDescargaDefinitiva(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPost("api/cronograma/ActualizarEstadoDescargaDefinitiva")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DescargaDefinitivaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "actualizar estado descargas definitivas de archivos de novedades",
           Description = "actualizar estado descargas definitivas de archivos de novedades",
           OperationId = "Cronograma.ActualizarEstadoDescargaDefinitiva",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<DescargaDefinitivaDto>>>> HandleAsync(DescargaDefinitivaDto request, CancellationToken cancellationToken = default)
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
                    Descripcion = "actualizar estado descargas definitivas de archivos de novedades.",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var archivoDescarga = await _cronogramaBusinessLogic.ObtenerArchivosDescargaDefinitiva(request);
                var descargaDefinitivaDto = archivoDescarga.Data.Where(x => x.NombreArchivo == request.NombreArchivo).FirstOrDefault();
                var response = await _cronogramaBusinessLogic.ActualizarEstadoDescargaDefinitiva(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, descargaDefinitivaDto, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _cronogramaBusinessLogic.ActualizarEstadoDescargaDefinitiva(request);
        }
    }
}
