using Ardalis.ApiEndpoints;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Domain.Core.DatosAliadoEstrategico;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto;
using System.Linq;
using System;
using GA2.Domain.Core;

namespace GA2.Apis.GA2.Endpoints.DatosAliadoEstrategico
{
    public class CrearDatosAliadoEstrategico : BaseAsyncEndpoint.WithRequest<DatosAliadoEstrategicoDto>.WithResponse<Response<DatosAliadoEstrategicoDto>>
    {
        private readonly IDatosAliadoEstrategicoBusinessLogic _datosAliadoEstrategicoBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public CrearDatosAliadoEstrategico(IDatosAliadoEstrategicoBusinessLogic datosAliadoEstrategicoBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _datosAliadoEstrategicoBusinessLogic = datosAliadoEstrategicoBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/CrearAliadoEstrategico")]
        [ProducesResponseType(typeof(Response<DatosAliadoEstrategicoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Aliado Estrategico",
           Description = "Crear Aliado Estrategico",
           OperationId = "credito.CrearAliadoEstrategico",
           Tags = new[] { "AliadoEstrategicoEndpoint" })]
        public override async Task<ActionResult<Response<DatosAliadoEstrategicoDto>>> HandleAsync(DatosAliadoEstrategicoDto request, CancellationToken cancellationToken = default)
        {
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
                        Modulo = "CRONOGRAMA",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar informacion de  aliado estrategicos",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await this._datosAliadoEstrategicoBusinessLogic.CrearDatosAliadoEstrategico(request);
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
    
}