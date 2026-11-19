using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class RegistrarPeritoDetalle : BaseAsyncEndpoint.WithRequest<PeritoDetalleDto>.WithResponse<Response<PeritoDetalle>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor para registrar perito
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public RegistrarPeritoDetalle(IProspeccionBusinessLogic prospeccionBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/prospeccion/RegistrarPeritoDetalle")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PeritoDetalle>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Registrar perito",
          Description = "Registar perito",
          OperationId = "prospeccion.RegistrarPeritoDetalle",
          Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<PeritoDetalle>>> HandleAsync(PeritoDetalleDto request, CancellationToken cancellationToken = default)
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
            //        Modulo = "PROSPECCION",
            //        FechaRegistro = DateTime.Now,
            //        Hostname = ip,
            //        IdTipoAccion = 6,
            //        SistemaOperativo = dataNavegador.OperatingSystem,
            //        Descripcion = "Guardar informacion perito",
            //        Navegador = dataNavegador.Browser,
            //        UsurioId = Guid.Parse(userId),
            //        NombreUsuario = user,
            //        TipoAccion = "GUARDAR"

            //    };
            //    var dataResponse = await _prospeccionBusinessLogic.InsertarPeritoDetalle(request);
            //    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
            //    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
            //    return dataResponse;
            //}
            //catch (System.Exception e)
            //{

            //    throw;
            //}
            return await _prospeccionBusinessLogic.InsertarPeritoDetalle(request); 
        }
    }
}
