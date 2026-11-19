
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Core.InfoVendedor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using GA2.Domain.Core;
using GA2.Application.Dto;
using System;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.InfVendedorEndpoint
{
    [Authorize]

    public class CrearInformacionVendedor : BaseAsyncEndpoint.WithRequest<InformacionVendedorDto>.WithResponse<Response<InformacionVendedorDto>>
    {
        private readonly IInformacionVendedorBusinessLogic _informacionVendedorBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;


        public CrearInformacionVendedor(IInformacionVendedorBusinessLogic informacionVendedorBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _informacionVendedorBusinessLogic = informacionVendedorBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        [HttpPost("api/credito/CrearInformacionVendedor")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InformacionVendedorDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Informacion Vendedor",
           Description = "Crear Informacion Vendedor",
           OperationId = "InfVendedor.CrearInformacionVendedor",
           Tags = new[] { "VendedorEndpoint" })]
        public override async Task<ActionResult<Response<InformacionVendedorDto>>> HandleAsync(InformacionVendedorDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                var userId = "83E91A3C-02A3-4D49-995E-87154620F98D";
                var user = "AFILIADO";
                var ip = _auditoriaBusinessLogic.Ip();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
                try
                {
                    var requestAuditoria = new AuditoriaDto()
                    {
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar informacion Matricula Credito",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _informacionVendedorBusinessLogic.CrearInformacionVendedor(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            else
            {
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
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar Informacion de  Vendedor",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _informacionVendedorBusinessLogic.CrearInformacionVendedor(request);
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

