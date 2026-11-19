using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Actualizar Juzgado
    /// </summary>
    /// <author>Edwin Lopez</author>
    /// <date>05/04/2022</date>

    [Authorize]
    public class ActualizarJuzgado :
         BaseAsyncEndpoint.WithRequest<JuzgadosDto>
        .WithResponse<Response<JuzgadosDto>>
    {

        private readonly IEmbargosBusinessLogic _juzgadosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor clase ActualizarJuzgados
        /// </summary>
        /// <param name="juzgadosBusinessLogic"></param>
        public ActualizarJuzgado(IEmbargosBusinessLogic juzgadosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _juzgadosBusinessLogic = juzgadosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// Actualizar Juzgado
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Embargos/ActualizarJuzgado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<JuzgadosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Juzgado",
            Description = "Actualizar Juzgados",
            OperationId = "Embargos.ActualizarJuzgado",
            Tags = new[] { "EmbargosEndPoints" })]
        public async override Task<ActionResult<Response<JuzgadosDto>>> HandleAsync(JuzgadosDto request, CancellationToken cancellationToken = default)
        {
            var obtenerJuzgado = await _juzgadosBusinessLogic.ObtenerJuzgadosPorNombre(request.Nombre);
            var juzgado = obtenerJuzgado.Data.Where(x => x.IdJuzgado == request.IdJuzgado).FirstOrDefault();
            if (juzgado is not null)
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
                        Modulo = "GESTIÓN DE EMBARGOS",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualiza un embargo de cuenta y concepto",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var response = await _juzgadosBusinessLogic.ActualizarJuzgado(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, juzgado, request, userId);
                    return response;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await _juzgadosBusinessLogic.ActualizarJuzgado(request);
        }
    }
}

