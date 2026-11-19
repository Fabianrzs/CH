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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{

    [Authorize]

    public class CrearOpciones : BaseAsyncEndpoint
        .WithRequest<OpcionesDto>.WithResponse<Response<OpcionesDto>>
    {

        private readonly IGruposOpcionesLogic _gruposOpcionesLogic; 
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearOpciones(IGruposOpcionesLogic gruposOpcionesLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _gruposOpcionesLogic = gruposOpcionesLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }



        [HttpPost("api/CrearOpciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<OpcionesDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "crear una opciones",
         Description = "crea una opciones",
         OperationId = "gruposopciones.CrearOpciones",
         Tags = new[] { "GrupoOpcionesEndpoint" })]
        public override async Task<ActionResult<Response<OpcionesDto>>> HandleAsync(OpcionesDto request, CancellationToken cancellationToken = default)
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
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de grupo opciones",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._gruposOpcionesLogic.CrearOpciones(request, userId);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._gruposOpcionesLogic.CrearOpciones(request, userId);
        }
    }
}
