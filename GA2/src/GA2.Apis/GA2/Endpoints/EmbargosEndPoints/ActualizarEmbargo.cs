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
    public class ActualizarEmbargo : BaseAsyncEndpoint.WithRequest<InsetEmbargosDto>.WithResponse<Response<EmbargosDto>>
    {

        IEmbargosBusinessLogic _embargosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public ActualizarEmbargo(IEmbargosBusinessLogic embargosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        [HttpPost("api/ActualizarEmbargo")]
        [ProducesResponseType(typeof(Response<EmbargosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar un embargo",
           Description = "Actualizrgoar un emba",
           OperationId = "Embargos.ActualizarEmbargo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<EmbargosDto>>> HandleAsync(InsetEmbargosDto request, CancellationToken cancellationToken = default)
        {
            var consulta = new ConsultaEmbargosfiltroDto()
            {
                RadicadoWorkManager = request.RadicadoWorkManager,
                Estado =1
            };
            var embargos = await _embargosBusinessLogic.ObtenerEmbargosRangoFiltros(consulta);
            var embargo = embargos.Data.Where(x => x.Id == request.Id).FirstOrDefault();
            if (embargo is not null)
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
                        Descripcion = "Guardar informacion embargo",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                   var response  = await this._embargosBusinessLogic.ActualizarEmbargo(request, Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value));
                   var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                   await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, embargo,response.Data, userId);
                    return response;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._embargosBusinessLogic.ActualizarEmbargo(request, Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value));
        }
    }
}
