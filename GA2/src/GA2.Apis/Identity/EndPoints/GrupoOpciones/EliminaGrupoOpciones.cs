using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.GrupoOpciones;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints.GrupoOpciones
{
    [Authorize]

    public class EliminaGrupoOpciones : BaseAsyncEndpoint
        .WithRequest<GrupoOpcionesDto>.WithResponse<Response<GrupoOpcionesDto>>
    {


        private readonly IGruposOpcionesLogic _gruposOpcionesLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public EliminaGrupoOpciones(IGruposOpcionesLogic gruposusuariosLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _gruposOpcionesLogic = gruposusuariosLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        [HttpPost("api/EliminaGrupoOpciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<GrupoOpcionesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Elimina grupo de opciones",
         Description = "elimina un gupo de opciones",
         OperationId = "gruposopciones.delete",
         Tags = new[] { "GrupoOpcionesEndpoint" })]
        public override async Task<ActionResult<Response<GrupoOpcionesDto>>> HandleAsync(GrupoOpcionesDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "ADMINISTRACION DE USUARIOS",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Elimina grupo de opciones",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await this._gruposOpcionesLogic.EliminarGruposOpciones(request, userId);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._gruposOpcionesLogic.EliminarGruposOpciones(request, userId);
            
        }
    }
}
