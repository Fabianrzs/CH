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

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para actualizar una sede de una Entidad Educativa
    /// </summary>
    /// <author>David Pineda</author>
    /// <date>14/03/2023</date>
    [Authorize]
    public class ActualizarSedeEntidadEducativa :
         BaseAsyncEndpoint.WithRequest<SedeEntidadEducativaDto>
        .WithResponse<Response<SedeEntidadEducativaDto>>
    {

        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public ActualizarSedeEntidadEducativa(IEntidadesBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        [HttpPut("api/Entidades/ActualizarSedeEntidadEducativa")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SedeEntidadEducativaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Actualizar Sede Entidad Educativa",
            Description = "Actualizar Sede Entidad Educativa",
            OperationId = "Entidades.ActualizarSedeEntidadEducativa",
            Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<SedeEntidadEducativaDto>>> HandleAsync(SedeEntidadEducativaDto request, CancellationToken cancellationToken = default)
        {
            var sede = await _catalogsBusinessLogic.ObtenerSedesPorEntidadEducativa((Guid)request.IdEntidad);
            var data = sede.Data.Where(x => x.IdSedeEntidad == request.IdSedeEntidad).FirstOrDefault();
            if (data != null)
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
                        Modulo = "GESTIÓN DE ENTIDADES EDUCATIVA",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizar Sede Entidad Educativa",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var response = await this._catalogsBusinessLogic.ActualizarSedeEntidadEducativa(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, data, request, userId);
                    return response;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._catalogsBusinessLogic.ActualizarSedeEntidadEducativa(request);
        }
    }
}

