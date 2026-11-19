using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Linq;
using System.Net;
using System;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class UpdateDataConvocatoria : BaseAsyncEndpoint.WithRequest<ConvocatoriaDTO>
        .WithResponse<Response<ConvocatoriaDTO>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;        

        public UpdateDataConvocatoria(IConvocatoriaBussigneLogic estadosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic )
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
        [HttpPost("api/convocatoria/UpdateDataConvocatoria")]
        [ProducesResponseType(typeof(Response<ConvocatoriaDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear convocatoria",
           Description = "Crear convocatoria",
           OperationId = "convocatoria.UpdateDataConvocatoria",
           Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<ConvocatoriaDTO>>> HandleAsync(ConvocatoriaDTO convocatoriaDTO, CancellationToken cancellationToken = default)
        {
            var request = new ConsultarConvocatoriaRequest()
            {
                NoConvocatoria = (int)convocatoriaDTO.NumeroConvocatoria
            };
            var dataOld = await _convocatoriaBussigneLogic.ConsultarConvocatoriaById(request);
            var response =  await _convocatoriaBussigneLogic.UpdateDataConvocatoria(convocatoriaDTO);
            
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
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
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizacion de convocatoria",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    TipoAccion = "MODIFICACIONES"

                };
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataOld.Data.FirstOrDefault(), convocatoriaDTO, userId);
            }
            catch (System.Exception e)
            {

                throw;
            }

            return response;
        }


    }
}
