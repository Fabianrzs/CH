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
    /// Servicio para registrar la selección de la fuerza y unidad del usuario
    /// </summary>
    [Authorize]
    public class ActualizarSelFuerzaUnidadUsuario : BaseAsyncEndpoint.WithRequest<SelFuerzaUnidadUsuarioDto>.WithResponse<Response<bool>>
    {

        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Crear Tipo Credito
        /// </summary>
        /// <param name="catalogosBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ActualizarSelFuerzaUnidadUsuario(ICatalogosBusinessLogic catalogosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/ActualizarSelFuerzaUnidadUsuario")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Actualizar la selección fuerza y unidad del usuario",
            Description = "Actualizar la selección fuerza y unidad del usuario",
            OperationId = "catalogos.ActualizarSelFuerzaUnidadUsuario",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(SelFuerzaUnidadUsuarioDto request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CATALOGO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizacion la selección fuerza y unidad del usuario",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var fuerzaUnidadUsuario = await _catalogosBusinessLogic.ObtenerSelFuerzaUnidadUsuario(request.Usr_Id);
                var response = await  this._catalogosBusinessLogic.ActualizarSelFuerzaUnidadUsuario(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, fuerzaUnidadUsuario.Data, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._catalogosBusinessLogic.ActualizarSelFuerzaUnidadUsuario(request);
        }
    }
}

