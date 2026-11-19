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

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    [Authorize]

    public class EliminarEmbargoCuentaConcepto : BaseAsyncEndpoint.WithRequest<EmbargoCuentaConceptosDto>.WithResponse<Response<List<EmbargoCuentaConceptoDto>>>
    {

        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;


        public EliminarEmbargoCuentaConcepto(IEmbargosBusinessLogic embargosBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }


        [HttpPost("api/EliminarEmbargoCuentaConcepto")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EmbargosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Eliminar un embargo cuenta concepto por id",
            Description = "Eliminar un embargo cuenta concepto por id",
            OperationId = "Embargos.EliminarEmbargoCuentaConcepto",
            Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<List<EmbargoCuentaConceptoDto>>>> HandleAsync(EmbargoCuentaConceptosDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "GESTIÓN DE EMBARGOS",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion de  embargo cuenta concepto ",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataDelete = new List<EmbargoCuentaConceptoDto>(); 
                var dataEmbargo = await _embargosBusinessLogic.ObtenerEmbargosCuentaConcepto();
                foreach (var item in request.Id)
                {
                    var data = dataEmbargo.Data.Where(x => x.Id == item).FirstOrDefault();
                    dataDelete.Add(data);   
                }
                var dataResponse = await _embargosBusinessLogic.EliminarEmbargoCuentaConcepto(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataDelete, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._embargosBusinessLogic.EliminarEmbargoCuentaConcepto(request);
        }
    }
}
