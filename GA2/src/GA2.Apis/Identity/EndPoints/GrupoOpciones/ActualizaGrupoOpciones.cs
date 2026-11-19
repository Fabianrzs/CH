using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.GrupoOpciones;
using GA2.Domain.Core;
using GA2.Domain.Entities;
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

    public class ActualizaGrupoOpciones : BaseAsyncEndpoint
        .WithRequest<GrupoOpcionesDto>.WithResponse<Response<GrupoOpcionesDto>>
    {

        private readonly IGruposOpcionesLogic _gruposOpcionesLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public ActualizaGrupoOpciones(IGruposOpcionesLogic gruposusuariosLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _gruposOpcionesLogic = gruposusuariosLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/ActualizaGrupoOpciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<GrupoOpcionesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "actualiza un  grupos de opciones",
         Description = "actualiza un  grupos de opciones",
         OperationId = "gruposopciones.actualizar",
         Tags = new[] { "GrupoOpcionesEndpoint" })]
        public override async Task<ActionResult<Response<GrupoOpcionesDto>>> HandleAsync(GrupoOpcionesDto request, CancellationToken cancellationToken = default)
        {
            var grupos = await _gruposOpcionesLogic.ObtenerGruposOpciones();
            var data = grupos.Data.Where(x => x.Id == request.Id).FirstOrDefault();
            if (data != null) {
                var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
                var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
                var usrId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
                var user = name + " " + surnNme;
                var ip = _auditoriaBusinessLogic.Ip();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);


                try
                {
                    var requestAuditoria = new AuditoriaDto()
                    {
                        Modulo = "ADMINISTRACION DE USUARIO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizar grupos de opciones",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(usrId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await this._gruposOpcionesLogic.ActualizaGruposOpciones(request, usrId);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, data, request, usrId);
                    return dataResponse;

                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            return await this._gruposOpcionesLogic.ActualizaGruposOpciones(request, userId);
        }
    }
}
