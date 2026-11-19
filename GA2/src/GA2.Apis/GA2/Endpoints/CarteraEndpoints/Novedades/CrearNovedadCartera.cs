using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Novedades
{
    /// <summary>
    /// </summary>
    //[Authorize]
    public class CrearNovedadCartera :
        BaseAsyncEndpoint.WithRequest<NovedadCarteraDto>.WithResponse<Response<NovedadCarteraDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CrearNovedadCartera(ICarteraBusinessLogic carteraBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Novedades/CrearNovedadCartera")]
        [ProducesResponseType(typeof(Response<NovedadCarteraDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear una novedad de cartera",
            Description = "Crear una novedad de cartera",
            OperationId = "credito.CrearNovedadCartera",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<NovedadCarteraDto>>> HandleAsync(NovedadCarteraDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var user = name + " " + surnNme;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);

            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CARTERA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de una novedad de cartera",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _carteraBusinessLogic.CrearNovedadCartera(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }

            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);

            return await _carteraBusinessLogic.CrearNovedadCartera(request);
        }
    }
}
