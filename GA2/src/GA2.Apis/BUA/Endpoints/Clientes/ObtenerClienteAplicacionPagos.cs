using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    /// <summary>
    /// Endpoint Obtener usuario por tipo y número de documento
    /// <author>Edwin Lopez Bejarano</author>
    /// </summary>
    [Authorize]
    public class ObtenerClienteAplicacionPagos : BaseAsyncEndpoint.WithRequest<DocumentoDTO>.WithResponse<Response<ArchivoAplicacionPagoCSVDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Constructor e inyección de dependencias
        /// </summary>
        /// <param name="clientsBussinesLogic"></param>
        public ObtenerClienteAplicacionPagos(
            IClientesBusinessLogic clientsBussinesLogic)
        {
            _clientsBusinessLogic = clientsBussinesLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [SwaggerOperation(
        Summary = "Trae los datos de un usuario",
           Description = "Trae los datos de un usuario al enviar un id por request",
           OperationId = "Client.ObtenerClienteAplicacionPagos",
           Tags = new[] { "ClientEndpoint" })]
        [HttpGet("api/clientes/ObtenerClienteAplicacionPagos")]
        public override async Task<ActionResult<Response<ArchivoAplicacionPagoCSVDto>>> HandleAsync([FromQuery] DocumentoDTO request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ObtenerClienteAplicacionPagos(request.tipoDocumentoId, request.numeroDocumento, User.Claims);
        }
    }
}
