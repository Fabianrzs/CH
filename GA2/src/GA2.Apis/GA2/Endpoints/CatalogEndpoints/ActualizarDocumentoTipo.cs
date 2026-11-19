using Ardalis.ApiEndpoints;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
using AutoMapper;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]
    public class ActualizarDocumentoTipo:BaseAsyncEndpoint.WithRequest<TipoIdentificacionDto>.WithResponse<Response<TipoIdentificacionDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public ActualizarDocumentoTipo(ICatalogosBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPatch("api/catalogos/ActualizarDocumentoTipo")]
        [ProducesResponseType(typeof(Response<TipoIdentificacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar Tipo de Documento",
            Description = "Actualizar Tipo de Documento",
            OperationId = "catalogos.ActualizarDocumentoTipo",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<TipoIdentificacionDto>>> HandleAsync(TipoIdentificacionDto request, CancellationToken cancellationToken = default)
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
                    Descripcion = "Actualizacion de Tipo de Documento",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var tipoDocumentos = await _catalogsBusinessLogic.ObtenerDocumentoTipo();
                var typeDocumento = tipoDocumentos.Data.Where(x => x.Id == request.Id).FirstOrDefault(); 
                var response = await _catalogsBusinessLogic.ActualizarDocumentoTipo(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, typeDocumento, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _catalogsBusinessLogic.ActualizarDocumentoTipo(request);
        }
    }
}
