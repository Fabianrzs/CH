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
using GA2.Domain.Entities.Entidades;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Eliminar Programa Educativo
    /// </summary>
    /// <author>Edwin Lopez</author>
    /// <date>15/03/2022</date>
    
    [Authorize]
    public class EliminarProgramaEducativo :
         BaseAsyncEndpoint.WithRequest<IdentificadoresEntidadEducativa>
        .WithResponse<Response<ProgramaEducativoDto>>
    {

        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// EliminarProgramaEducativo
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public EliminarProgramaEducativo(IEntidadesBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// Eliminar Entidad Educativa
        /// </summary>
        /// <param name="entidadEducativa"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Entidades/EliminarProgramaEducativoPorId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProgramaEducativoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Eliminar Entidad Educativa",
          Description = "Eliminar Entidad Educativa",
          OperationId = "Entidades.EntidadEducativa",
          Tags = new[] { "EntidadesEndpoint" }
        )]
        public async override Task<ActionResult<Response<ProgramaEducativoDto>>> HandleAsync(IdentificadoresEntidadEducativa entidadEducativa, CancellationToken cancellationToken = default)
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
                    Modulo = "ENTIDAD",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion  Entidad Educativa",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await this._catalogsBusinessLogic.EliminarProgramaEducativoPorId(entidadEducativa);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse.Data  , null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}

