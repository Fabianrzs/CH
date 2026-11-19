using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ActualizarSeguroGeneral : BaseAsyncEndpoint.WithRequest<ParamGeneralesSegurosDto>.WithResponse<Response<ParamGeneralesSegurosDto>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public ActualizarSeguroGeneral(ICarteraBusinessLogic carteraBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;       
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cartera/actualizarSeguroGeneral")]
        [ProducesResponseType(typeof(Response<ParamGeneralesSegurosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar parametros de seguro general ",
            Description = "Actualizar parametros de seguro general",
            OperationId = "cartera.ActualizarSeguroGeneral",
            Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<ParamGeneralesSegurosDto>>> HandleAsync(ParamGeneralesSegurosDto request, CancellationToken cancellationToken = default)
        {
            var parametrosGenerales = await _carteraBusinessLogic.ObtenerSeguroGeneral();
            var tasaLegal = parametrosGenerales.Data.Where(x => x.idSeguro == request.idSeguro).FirstOrDefault();
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
                    Modulo = "PARAMETRIZACION DE SEGURO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizar  parametrizacion seguros",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "MODIFICACIONES"

                };
                var dataResponse = await this._carteraBusinessLogic.ActualizarSeguroGeneral(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, tasaLegal, request, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._carteraBusinessLogic.ActualizarSeguroGeneral(request);
        }
    }
}
