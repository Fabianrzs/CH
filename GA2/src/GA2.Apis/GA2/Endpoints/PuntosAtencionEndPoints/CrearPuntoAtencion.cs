using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.PuntosAtencion;
using GA2.Domain.Core.Escrituras;
using GA2.Domain.Core.PuntosAtencion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto;
using System.Linq;
using System;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints.PuntosAtencionEndPoints
{
    public class CrearPuntoAtencion : BaseAsyncEndpoint.WithRequest<PuntosAtencionDto>.WithResponse<Response<PuntosAtencionDto>>
    {
        private readonly IPuntoAtencionBusinessLogic _puntoAtencionBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;


        public CrearPuntoAtencion(IPuntoAtencionBusinessLogic puntoAtencionBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _puntoAtencionBusinessLogic = puntoAtencionBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/PuntoAtencion/CrearPuntoAtencion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PuntosAtencionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear Punto Atencion",
            Description = "Crear Punto Atencion",
            OperationId = "PuntoAtencion.CrearPuntoAtencion",
            Tags = new[] { "PuntoAtencionEndpoint" })]
        public override async Task<ActionResult<Response<PuntosAtencionDto>>> HandleAsync(PuntosAtencionDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "PUNTO ATENCION",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion Punto Atencion",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._puntoAtencionBusinessLogic.CrearPuntoAtencion(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}
