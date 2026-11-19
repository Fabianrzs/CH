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
    //[Authorize]
    public class ActualizarParamGeneralesCreditoYCartera : BaseAsyncEndpoint.WithRequest<ParamGeneralesCreditoYCarteraDto>.WithResponse<Response<ParamGeneralesCreditoYCarteraDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ActualizarParamGeneralesCreditoYCartera(ICarteraBusinessLogic carteraBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPost("api/credito/actualizarParamGeneralesCreditoYCartera")]
        [ProducesResponseType(typeof(Response<ParamGeneralesCreditoYCarteraDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar parametros generales de credito y cartera en la simulacion",
            Description = "Actualizar parametros generales de credito y cartera en la simulacion",
            OperationId = "credito.ActualizarParamGeneralesCreditoYCartera",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<ParamGeneralesCreditoYCarteraDto>>> HandleAsync(ParamGeneralesCreditoYCarteraDto request, CancellationToken cancellationToken = default)
        {
            var parametrosGenerales = await _carteraBusinessLogic.ObtenerParamGeneralesCreditoYCartera();
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
                    Modulo = "PARAMETRIZACION DE CREDITO Y CARTERA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizar  parametrizacion generales de credito y cartera",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "MODIFICACIONES"

                };
                var dataResponse = await this._carteraBusinessLogic.ActualizarParamGeneralesCreditoYCartera(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, parametrosGenerales.Data, request, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._carteraBusinessLogic.ActualizarParamGeneralesCreditoYCartera(request);
        }
    }
}
