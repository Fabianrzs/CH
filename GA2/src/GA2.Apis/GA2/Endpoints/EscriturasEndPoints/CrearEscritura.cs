using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using GA2.Application.Dto.Escrituras;
using GA2.Domain.Core.Escrituras;
using GA2.Application.Dto;
using System.Linq;
using System;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints.EscriturasEndPoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class CrearEscritura : BaseAsyncEndpoint.WithRequest<CreacionEscritura>.WithResponse<Response<CreacionEscritura>>
    {
        private readonly IEscrituraBusinessLogic _escrituraBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearEscritura(IEscrituraBusinessLogic escrituraBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _escrituraBusinessLogic = escrituraBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/CrearEscritura")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CreacionEscritura>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear escrituras",
            Description = "Crear escrituras",
            OperationId = "escritura.CrearEscritura",
            Tags = new[] { "EscrituraEndpoint" })]
        public override async Task<ActionResult<Response<CreacionEscritura>>> HandleAsync(CreacionEscritura request, CancellationToken cancellationToken = default)
        {
            //var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            //var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            //var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            //var user = name + " " + surnNme;
            //var ip = _auditoriaBusinessLogic.Ip();
            //var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            //var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);


            //try
            //{
            //    var requestAuditoria = new AuditoriaDto()
            //    {
            //        Modulo = "ESCRITURA",
            //        FechaRegistro = DateTime.Now,
            //        Hostname = ip,
            //        IdTipoAccion = 6,
            //        SistemaOperativo = dataNavegador.OperatingSystem,
            //        Descripcion = "Guardar informacion escrituras",
            //        Navegador = dataNavegador.Browser,
            //        UsurioId = Guid.Parse(userId),
            //        NombreUsuario = user,
            //        TipoAccion = "GUARDAR"

            //    };
            //    var dataResponse = await this._escrituraBusinessLogic.CrearEscritura(request);
            //    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
            //    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
            //    return dataResponse;
            //}
            //catch (System.Exception e)
            //{

            //    throw;
            //}
            return await this._escrituraBusinessLogic.CrearEscritura(request);
        }
    }
}
