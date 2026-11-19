using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Afiliaciones
{
    [Authorize]
    public class EliminarAfiliacion : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<AfiliacionDto>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public EliminarAfiliacion(IAfiliacionesBusinessLogic afiliacionesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        /// <summary>
        /// Update client info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        //[ValidateAntiForgeryToken]
        [HttpDelete("api/EliminarAfiliacion")]
        [ProducesResponseType(typeof(Response<IEnumerable<AfiliacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene la afiliacion de un cliente",
           Description = "Obtiene la afiliacion un cliente",
           OperationId = "afiliaciones.ObtenerAfiliacionesByIdentificacion",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<AfiliacionDto>>> HandleAsync(int request, CancellationToken cancellationToken = default)
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
                    Modulo = "AFILIACION",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion  afiliacion de un cliente",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await _afiliacionesBusinessLogic.EliminarAfiliacion(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse.Data, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _afiliacionesBusinessLogic.EliminarAfiliacion(request);
        }
    }
}
