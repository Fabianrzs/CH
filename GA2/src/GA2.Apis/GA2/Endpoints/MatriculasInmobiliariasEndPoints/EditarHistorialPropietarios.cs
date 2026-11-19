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

namespace GA2.Apis.GA2.Endpoints.MatriculasInmobiliariasEndPoints
{
    [Authorize]

    public class EditarHistorialPropietarios : BaseAsyncEndpoint.WithRequest<PropietariosMatriculasInmobiliariasDto>.WithResponse<Response<PropietariosMatriculasInmobiliariasDto>>
    {
        private readonly IMatriculasInmobiliariasLogic _matriculasInmobiliariasLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public EditarHistorialPropietarios(IMatriculasInmobiliariasLogic matriculasInmobiliariasLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _matriculasInmobiliariasLogic = matriculasInmobiliariasLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        [HttpPost("api/EditarHistorialPropietarios")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<PropietariosMatriculasInmobiliariasDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Historial de propietarios de matriculas inmoviliarias",
                   Description = "Optiene una lista con el historial de los propietarios de matricula inmoviliaria por Id",
                   OperationId = "matriculas.historialPropietarios",
                   Tags = new[] { "MatriculasEndpoint" })]
        public override async Task<ActionResult<Response<PropietariosMatriculasInmobiliariasDto>>> HandleAsync(PropietariosMatriculasInmobiliariasDto request, CancellationToken cancellationToken = default)
        {
            var propietario = await _matriculasInmobiliariasLogic.HistorialPropietariosMatriculaInmobiliariaById(request.MaiId);
            var data = propietario.Data.Where(x => x.PrpId == request.PrpId).FirstOrDefault();
            if (data != null)
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
                        Modulo = "GESTION MATRICULA INMOVILIARIA",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizar Historial de propietarios de matriculas inmoviliaria",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await this._matriculasInmobiliariasLogic.EditarHistorialPropietarios(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await this._matriculasInmobiliariasLogic.EditarHistorialPropietarios(request);
        }
    }
}
