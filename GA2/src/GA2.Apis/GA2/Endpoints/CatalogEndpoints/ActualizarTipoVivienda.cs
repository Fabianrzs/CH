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
    /// Endpoint para Actualizar Tipo Vivienda
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>11/05/2021</date>
    [Authorize]
    public class ActualizarTipoVivienda :
         BaseAsyncEndpoint.WithRequest<TipoViviendaDto>
        .WithResponse<Response<TipoViviendaDto>>
    {

        private readonly ICatalogosBusinessLogic _tipoviviendaBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Actualizar Tipo Vivienda
        /// </summary>
        /// <param name="tipoviviendaBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ActualizarTipoVivienda(ICatalogosBusinessLogic tipoviviendaBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _tipoviviendaBusinessLogic = tipoviviendaBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/ActualizarTipoVivienda")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TipoViviendaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Tipo Vivienda",
            Description = "Actualizar Tipo Vivienda",
            OperationId = "catalogos.ActualizarTipoVivienda",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<TipoViviendaDto>>> HandleAsync(TipoViviendaDto request, CancellationToken cancellationToken = default)
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
                    Descripcion = "Actualizacion de Tipo Vivienda",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var tiposVivienda = await _tipoviviendaBusinessLogic.ObtenerTiposDeVivienda();
                var tipoVivienda = tiposVivienda.Data.Where(x => x.Id == request.Id).FirstOrDefault();
                var response = await this._tipoviviendaBusinessLogic.ActualizarTipoVivienda(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, tipoVivienda, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._tipoviviendaBusinessLogic.ActualizarTipoVivienda(request);
        }
    }
}

