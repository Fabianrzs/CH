using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    public class CrearNovedadEntidadEducativa:BaseAsyncEndpoint.WithRequest<NovedadesEntidadEducativaRequestDto>
        .WithResponse<Response<string>>
    {

        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Creacion noevaded  Entidad Educativa
        /// </summary>
        /// <param name="entidadesBusinessLogic"></param>
        /// <author>Edwin Lopez</author>
        /// <date>11/05/2021</date>
        public CrearNovedadEntidadEducativa(IEntidadesBusinessLogic entidadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Creacion de novedades de entidades educativas
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Entidades/CrearNovedadEntidadeEducativa")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [SwaggerOperation(
       Summary = "Creacion de novedades de entidades educativas",
        Description = "Creacion de novedades de entidades educativas",
        OperationId = "Entidades.CrearNovedadEntidadeEducativa",
        Tags = new[] { "EntidadesEndpoint" })]
    public async override Task<ActionResult<Response<string>>> HandleAsync(NovedadesEntidadEducativaRequestDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "ENTIDADES",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de novedades de entidades educativass",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _entidadesBusinessLogic.CrearNovedadesEntidadEducativa(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await _entidadesBusinessLogic.CrearNovedadesEntidadEducativa(request);
    }
}
}
