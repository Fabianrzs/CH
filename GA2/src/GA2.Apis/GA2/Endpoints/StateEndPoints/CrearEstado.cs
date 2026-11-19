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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para Insert Estado
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>17/05/2021</date>
    [Authorize]
    public class CrearEstado : BaseAsyncEndpoint.WithRequest<EstadoDto>.WithResponse<Response<EstadoDto>>
    {
        private readonly IEstadoBusinessLogic _stateBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Insert Estado
        /// </summary>
        /// <param name="stateBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>18/05/2021</date>
        public CrearEstado(IEstadoBusinessLogic stateBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _stateBusinessLogic = stateBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/estado/CrearEstado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EstadoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene estado",
            Description = "Agrega estado",
            OperationId = "estado.insert",
            Tags = new[] { "StateEndPoint" })]
        public async override Task<ActionResult<Response<EstadoDto>>> HandleAsync(EstadoDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "ESTADO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacioN estado",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _stateBusinessLogic.CrearEstado(request);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse.Data, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _stateBusinessLogic.CrearEstado(request);
        }
    }
}