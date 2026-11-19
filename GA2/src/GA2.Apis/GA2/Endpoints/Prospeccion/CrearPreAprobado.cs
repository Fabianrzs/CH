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

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    [Authorize]
    public class CrearPreAprobado:BaseAsyncEndpoint.WithRequest<SolicitudSimulacionDto>.WithResponse<Response<SolicitudSimulacionDto>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor e inyeccion dependencias
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public CrearPreAprobado(IProspeccionBusinessLogic prospeccionBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        ///  Crea Preaprovado
        /// </summary>
        /// <param name = "request" > Peticion usuario</param>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        [HttpPost("api/prospeccion/CrearPreAprobado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SolicitudSimulacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crea Preaprobado",
           Description = "Crea Preaprobado",
           OperationId = "prospeccion.CrearPreAprobado",
           Tags = new[] { "ProspeccionEndpoint" })]

        public override async Task<ActionResult<Response<SolicitudSimulacionDto>>> HandleAsync(SolicitudSimulacionDto request, CancellationToken cancellationToken = default)
        {
            var userId = "83E91A3C-02A3-4D49-995E-87154620F98D";
            var user = "parmenio.silva@cajahonor.gov.co";
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);


            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "PROSPECCION",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion Preaprobado",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _prospeccionBusinessLogic.CrearPreAprobado(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }

            return  await _prospeccionBusinessLogic.CrearPreAprobado(request);
        }
    }
}
