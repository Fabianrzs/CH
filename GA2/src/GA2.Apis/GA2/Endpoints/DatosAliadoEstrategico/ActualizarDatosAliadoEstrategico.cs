using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Domain.Core.DatosAliadoEstrategico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using System.Linq;
using System;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints.DatosAliadoEstrategico
{
    public class ActualizarDatosAliadoEstrategico : BaseAsyncEndpoint.WithRequest<DatosAliadoEstrategicoDto>.WithResponse<Response<DatosAliadoEstrategicoDto>>
    {
        private readonly IDatosAliadoEstrategicoBusinessLogic _datosAliadoEstrategicoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        public ActualizarDatosAliadoEstrategico (IDatosAliadoEstrategicoBusinessLogic datosAliadoEstrategicoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _datosAliadoEstrategicoBusinessLogic = datosAliadoEstrategicoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }

        [HttpPost("api/ActualizarAliadoEstrategico")]
        [ProducesResponseType(typeof(Response<DatosAliadoEstrategicoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Aliado Estrategico",
           Description = "Actualizar Aliado Estrategico",
           OperationId = "credito.ActualizarAliadoEstrategico",
           Tags = new[] { "AliadoEstrategicoEndpoint" })]
        public override async Task<ActionResult<Response<DatosAliadoEstrategicoDto>>> HandleAsync(DatosAliadoEstrategicoDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "AFILIADO",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 4,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Actualizar Aliado Estrategico",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "MODIFICACIONES"

                };
                var dataOld = await _datosAliadoEstrategicoBusinessLogic.ObtenerDatosAliadoEstrategico(request.Soc_Id);
                var dataResponse = await this._datosAliadoEstrategicoBusinessLogic.ActualizarDatosAliadoEstrategico(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataOld.Data, request, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            
            return await this._datosAliadoEstrategicoBusinessLogic.ActualizarDatosAliadoEstrategico(request);
        }
    }
}
