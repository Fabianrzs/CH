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
    [Authorize]
    public class InsertarActor : BaseAsyncEndpoint.WithRequest<TipoactorDto>.WithResponse<Response<JuzgadosDto>>
    {
        private readonly IEmbargosBusinessLogic _juzgadosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public InsertarActor(IEmbargosBusinessLogic juzgadosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _juzgadosBusinessLogic = juzgadosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        [HttpPost("api/Embargos/TipoActor")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<JuzgadosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Crear un tipo de actor",
            Description = "Crear un tipo de actor",
            OperationId = "Embargos.TipoActor",
            Tags = new[] { "EmbargosEndPoints" })]
       
        public async override Task<ActionResult<Response<JuzgadosDto>>> HandleAsync(TipoactorDto tipoActor, CancellationToken cancellationToken = default)
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
                    Modulo = "EMBARGO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de  tipo de actor",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _juzgadosBusinessLogic.InsertarActor(tipoActor); ;
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, tipoActor, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _juzgadosBusinessLogic.InsertarActor(tipoActor);
        }
    }
}
