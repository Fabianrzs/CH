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

    public class CrearGurpoOpciones : BaseAsyncEndpoint
        .WithRequest<GrupoOpcionesDto>.WithResponse<Response<GrupoOpcionesDto>>
    {

        private readonly IGruposOpcionesLogic _gruposOpcionesLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearGurpoOpciones(IGruposOpcionesLogic gruposusuariosLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _gruposOpcionesLogic = gruposusuariosLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/CrearGurpoOpciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<GrupoOpcionesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "crear un  grupos de opciones",
         Description = "crea un grupo de opciones",
         OperationId = "gruposopciones.create",
         Tags = new[] { "GrupoOpcionesEndpoint" })]
        public override async Task<ActionResult<Response<GrupoOpcionesDto>>> HandleAsync(GrupoOpcionesDto request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
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
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de crear un  grupos de opciones",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._gruposOpcionesLogic.CrearGruposOpciones(request, userId);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._gruposOpcionesLogic.CrearGruposOpciones(request, userId);   
        }
    }
}
