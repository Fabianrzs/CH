using System.Linq;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.IO;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint carga documento solicitud de credito
    /// </summary>
    [Authorize]
    public class CargarDocumentosSolicitudCredito : BaseAsyncEndpoint.WithRequest<IFormFileCollection>.WithResponse<Response<bool>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Nelson Redondo</author>
        /// <date>20/10/2021</date>
        public CargarDocumentosSolicitudCredito(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Carga Archivo al blob storage para solicitud de Credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/CargarDocumentosSolicitudCredito/{idSolicitud}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation("Carga Archivo al blob storage para solicitud de Credito",
            Description = "Carga Archivo al blob storage para solicitud de Credito",
            OperationId = "Credito.CargarDocumentosSolicitudCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(IFormFileCollection request, CancellationToken cancellationToken = default)
        {
            List<CargarDocumentosSolicitudDto> archivosCargados = new();
            Guid idSolicitud = Guid.Parse((Request.Path.Value.Split("/")[Request.Path.Value.Split("/").Length - 1]).Split("$")[0]);

            foreach (var i in Request.Form.Files)
            {
                var archivoParametrizadoId = i.FileName.Split("--")[0];
                archivosCargados.Add(new()
                {
                    archivoParametrizadoId = Guid.Parse(archivoParametrizadoId),
                    archivosCargados = i,
                    numeroFolio = i.FileName.Split("--")[2]
                });
            }


            if (archivosCargados.Count.Equals(default))
            {
                return new Response<bool>
                {
                    ReturnMessage = "No se cargaron archivos",
                    Data = false,
                    IsSuccess = false,
                };
            }

            return await _solicitudBusinessLogic.CargarDocumentosSolicitudCredito(archivosCargados, idSolicitud);
        }
    }
}
