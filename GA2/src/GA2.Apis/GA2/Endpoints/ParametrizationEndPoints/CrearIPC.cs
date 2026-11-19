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
    /// Endpoint para crear parametrizacion IPC
    /// <author>Cesar Hernandez</author>
    /// <date>20/04/2021</date>
    /// </summary>
    //[Authorize]
    public class CrearIPC : BaseAsyncEndpoint.WithRequest<IPCDto>.WithResponse<Response<IPCDto>>
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public CrearIPC(IParametrizacionBusinessLogic parametrizationBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/parametrizacion/CrearIPC")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IPCDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = " crear parametrizacion legal Gmf",
            Description = "crear parametrizacion legal Gmf",
            OperationId = "parametrizacion.IdIPC",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<IPCDto>>> HandleAsync(IPCDto request, CancellationToken cancellationToken = default)
        {

            if (request.Id != 0)
            {
                return await _parametrizationBusinessLogic.ActualizarIPC(request);
            }
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
                    Modulo = "LIQUIDACION INTERES",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion parametrizacion legal Gmf",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _parametrizationBusinessLogic.CrearIPC(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _parametrizationBusinessLogic.CrearIPC(request);
        }
    }
}
