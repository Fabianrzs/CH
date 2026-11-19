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
    /// Endpoint para Crear Juzgado
    /// </summary>
    /// <author>Edwin Lopez</author>
    /// <date>05/04/2022</date>

    [Authorize]
    public class CrearJuzgado :
         BaseAsyncEndpoint.WithRequest<JuzgadosDto>
        .WithResponse<Response<JuzgadosDto>>
    {

        private readonly IEmbargosBusinessLogic _juzgadoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Crear Juzgados
        /// </summary>
        /// <param name="juzgadosBusinessLogic"></param>
        public CrearJuzgado(IEmbargosBusinessLogic juzgadosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _juzgadoBusinessLogic = juzgadosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }


        /// <summary>
        /// CrearJuzgado
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Embargos/CrearJuzgado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<JuzgadosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Crear Juzgado",
            Description = "Crear Juzgado",
            OperationId = "Embargos.CrearJuzgado",
            Tags = new[] { "EmbargosEndPoints" })]
        public async override Task<ActionResult<Response<JuzgadosDto>>> HandleAsync(JuzgadosDto request, CancellationToken cancellationToken = default)
        {
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
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar informacion de  Juzgado",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _juzgadoBusinessLogic.CrearJuzgado(request);
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
}

