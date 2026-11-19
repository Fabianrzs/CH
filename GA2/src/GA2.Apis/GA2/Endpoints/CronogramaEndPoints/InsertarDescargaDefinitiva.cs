using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.CronogramaActividades;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    /// <summary>
	/// Endpoint para insertar descargas definitivas de archivos de novedades
	/// </summary>
	/// <author>Michel Parra</author>
	/// <date>06/03/2024</date>
    [Authorize]
    public class InsertarDescargaDefinitiva :
         BaseAsyncEndpoint.WithRequest<DescargaDefinitivaDto>.WithResponse<Response<IEnumerable<DescargaDefinitivaDto>>>
    {
        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Michel Parra</author>
        /// <date>06/03/2024</date>
        public InsertarDescargaDefinitiva(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPost("api/cronograma/InsertarDescargaDefinitiva")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<DescargaDefinitivaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Insertar archivos de descarga definitiva",
           Description = "Insertar archivos de descarga definitiva",
           OperationId = "Cronograma.InsertarDescargaDefinitiva",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<DescargaDefinitivaDto>>>> HandleAsync(DescargaDefinitivaDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "CRONOGRAMA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de descarga definitiva",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _cronogramaBusinessLogic.InsertarDescargaDefinitiva(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}
