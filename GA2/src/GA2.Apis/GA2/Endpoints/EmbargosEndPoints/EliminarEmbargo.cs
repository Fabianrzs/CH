using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    [Authorize]

    public class EliminarEmbargo : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<EmbargosDto>>
    {
        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;


        public EliminarEmbargo(IEmbargosBusinessLogic embargosBusinessLogic,IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        [HttpDelete("api/EliminarEmbargo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EmbargosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Eliminar un embargo por Id",
            Description = "Eliminar un embargo por Id",
            OperationId = "Embargos.EliminarEmbargo",
            Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<EmbargosDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
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
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion de  embargo",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataDelete = _embargosBusinessLogic.ObtenerEmbargoBy(request);
                var dataResponse = await _embargosBusinessLogic.EliminarEmbargo(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataDelete, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._embargosBusinessLogic.EliminarEmbargo(request);
        }
    }
}
