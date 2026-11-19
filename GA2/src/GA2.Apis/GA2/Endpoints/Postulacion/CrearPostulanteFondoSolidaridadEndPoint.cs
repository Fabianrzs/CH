using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Linq;
using System;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class CrearPostulanteFondoSolidaridadEndPoint : 
        BaseAsyncEndpoint.WithRequest<IEnumerable<PostulanteDto>>
        .WithResponse<Response<IEnumerable<PostulanteDto>>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public CrearPostulanteFondoSolidaridadEndPoint(IPostulacionBussineLogic bussigneLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        [HttpPost("api/postulante/crearPostulante")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear postulante fondo de solidaridad",
            Description = "Crear postulante fondo de solidaridad",
            OperationId = "postulante.crearPostulante",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteDto>>>> HandleAsync(IEnumerable<PostulanteDto> request, CancellationToken cancellationToken = default)
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
                    Modulo = "FONDO SOLIDARIDAD",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion postulante fondo de solidaridad",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _postulacionBussineLogic.CrearPostulanteFondoSolidaridad(request);

                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _postulacionBussineLogic.CrearPostulanteFondoSolidaridad(request);
        }
    }
}

