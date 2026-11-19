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

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]
    public class ActualizarCategoria : BaseAsyncEndpoint.WithRequest<CategoriaDto>.WithResponse<Response<CategoriaDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public ActualizarCategoria(ICatalogosBusinessLogic catalogosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPatch("api/Catalogos/ActualizarCategoria")]
        [ProducesResponseType(typeof(Response<CategoriaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualiza Categoria",
            Description = "Actualiza Categoria",
            OperationId = "catalogos.ActualizarCategoria",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<CategoriaDto>>> HandleAsync(CategoriaDto request, CancellationToken cancellationToken = default)
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
                    Descripcion = "Actualizacion de Categoria",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var categorias = await _catalogosBusinessLogic.ObtenerCategorias();
                var categoria = categorias.Data.Where(x => x.Id == request.Id).FirstOrDefault();
                var response = await _catalogosBusinessLogic.ActualizarCategoria(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, categoria, request, userId);
                return response;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _catalogosBusinessLogic.ActualizarCategoria(request);
        }
    }

}
