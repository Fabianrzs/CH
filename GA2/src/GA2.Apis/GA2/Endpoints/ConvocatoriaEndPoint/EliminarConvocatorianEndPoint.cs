using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Convocatoria;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class EliminarConvocatorianEndPoint : BaseAsyncEndpoint.WithRequest<EliminarConvocatoriaRequest>
        .WithResponse<Response<string>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public EliminarConvocatorianEndPoint(IConvocatoriaBussigneLogic estadosBusinessLogic,IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _convocatoriaBussigneLogic = estadosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        /// <summary>
        /// Crear la solicitud de convocatoria
        /// </summary>
        /// <param name="convocatoriaDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/convocatoria/EliminarConvocatoria")]
        [ProducesResponseType(typeof(Response<ConvocatoriaDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Eliminar convocatoria",
           Description = "Eliminar convocatoria",
           OperationId = "convocatoria.EliminarConvocatoria",
           Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(EliminarConvocatoriaRequest convocatoriaDTO, CancellationToken cancellationToken = default)
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
                    Modulo = "CONVOCATORIA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion de la convocatoria",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse =  await _convocatoriaBussigneLogic.EliminarConvocatoria(convocatoriaDTO);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, convocatoriaDTO, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}

