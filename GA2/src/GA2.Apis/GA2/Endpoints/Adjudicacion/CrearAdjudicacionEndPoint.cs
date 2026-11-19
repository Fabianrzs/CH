using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Proyecto;
using GA2.Domain.Core.Proyecto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using System.Linq;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GA2.Application.Dto;
using System;

namespace GA2.Apis.GA2.Endpoints.Adjudicacion
{
    public class CrearAdjudicacionEndPoint : BaseAsyncEndpoint.WithRequest<AdjudicacionViviendaDto>
        .WithResponse<Response<string>>
    {
       private readonly IProyectoBusiness _proyectoBusiness;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearAdjudicacionEndPoint(IProyectoBusiness proyectoBusiness, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _proyectoBusiness = proyectoBusiness;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/AdjudicacionVivienda/CrearAdjucacionVivienda")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Crear adjudicacion de vivienda",
                   Description = "Crear adjudicacion de vivienda",
                   OperationId = "AdjudicacionVivienda.CrearAdjucacionVivienda",
                   Tags = new[] { "AdjudicacionVivienda" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(AdjudicacionViviendaDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "ADJUDICACION",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de adjudicacion de vivienda",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario= user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _proyectoBusiness.CrearAdjudicacionVivienda(request);
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
