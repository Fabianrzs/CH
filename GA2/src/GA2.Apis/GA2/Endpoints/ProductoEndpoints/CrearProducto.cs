using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Producto;
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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Crea un Producto
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    [Authorize]
    public class CrearProducto : BaseAsyncEndpoint.WithRequest<ProductoDTO>.WithResponse<Response<ProductoDTO>>
    {


        private readonly IProductoBusinessLogic _productoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor e inyeccion dependencias
        /// </summary>
        /// <param name="productoBusinessLogic"></param>
        public CrearProducto(IProductoBusinessLogic productoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _productoBusinessLogic = productoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        ///  Crea un nuevo producto
        /// </summary>
        /// <param name = "request" > Peticion usuario</param>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/producto")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ProductoDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crea un nuevo producto",
           Description = "Crea un nuevo producto",
           OperationId = "producto.create",
           Tags = new[] { "ProductosEndpoint" })]
        public async override Task<ActionResult<Response<ProductoDTO>>> HandleAsync(ProductoDTO request, CancellationToken cancellationToken = default)
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
                    Modulo = "PRODUCTO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion nuevo producto",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._productoBusinessLogic.CrearProducto(request); ;
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._productoBusinessLogic.CrearProducto(request);
        }
    }
}
