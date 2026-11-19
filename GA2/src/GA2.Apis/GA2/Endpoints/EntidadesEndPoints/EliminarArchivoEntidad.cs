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
    /// Endpoint para Eliminar archivos de solicitud Por Id
    /// </summary>
    /// <author>Hanson Restrepo</author>
    /// <date>07/03/2022</date>
    [Authorize]
    public class EliminarArchivoEntidad :
         BaseAsyncEndpoint.WithRequest<EliminarArchivoEntidadDto>
        .WithResponse<Response<bool>>
    {
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint descargar archivo solicitud
        /// </summary>
        /// <param name="entidadesBusinessLogic"></param>
        public EliminarArchivoEntidad(IEntidadesBusinessLogic entidadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <param name="eliminarArchivoEntidadDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Entidades/EliminarArchivoEntidad")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
               Summary = "Eliminar Archivos de Solicitud por Id",
               Description = "Eliminar Archivos de Solicitud por Id",
               OperationId = "Solicitud.EliminarArchivoPorSolicitud",
               Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(EliminarArchivoEntidadDto eliminarArchivoEntidadDto, CancellationToken cancellationToken = default)
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
                    Descripcion = "Eliminar informacion Archivos de Solicitud por ID ",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await this._entidadesBusinessLogic.EliminarArchivoEntidad(eliminarArchivoEntidadDto);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, eliminarArchivoEntidadDto, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._entidadesBusinessLogic.EliminarArchivoEntidad(eliminarArchivoEntidadDto);
        }
    }
}

