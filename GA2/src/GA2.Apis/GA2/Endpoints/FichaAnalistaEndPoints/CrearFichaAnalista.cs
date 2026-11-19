using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.FichaAnalista;
using GA2.Domain.Core.FichaAnalista;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using GA2.Application.Dto;
using System.Linq;
using System;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints.FichaAnalistaEndPoints
{
    [Authorize]
    public class CrearFichaAnalista : BaseAsyncEndpoint.WithRequest<FichaAnalistaDto>.WithResponse<Response<FichaAnalistaDto>>
    {
        private readonly IFichaAnalistaBusinessLogic _iFichaAnalistaBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearFichaAnalista(IFichaAnalistaBusinessLogic iFichaAnalistaBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _iFichaAnalistaBusinessLogic = iFichaAnalistaBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/FichaAnalista/CrearFichaAnalista")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<FichaAnalistaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear Ficha Analista",
            Description = "Crear Ficha Analista",
            OperationId = "fichaAnalista.CrearFichaAnalista",
            Tags = new[] { "FichaAnalistaEndpoint" })]
        public override async Task<ActionResult<Response<FichaAnalistaDto>>> HandleAsync(FichaAnalistaDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "FICHA ANALISTA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion Ficha Analista",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._iFichaAnalistaBusinessLogic.CreaFichaAnalista(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._iFichaAnalistaBusinessLogic.CreaFichaAnalista(request);
        }
    }
}
