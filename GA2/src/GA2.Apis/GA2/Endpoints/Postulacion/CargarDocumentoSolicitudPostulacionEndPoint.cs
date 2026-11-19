using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class CargarDocumentoSolicitudPostulacionEndPoint : BaseAsyncEndpoint.WithRequest<IFormFileCollection>.WithResponse<Response<bool>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Erwin Pantoja</author>
        /// <date>20/10/2021</date>
        public CargarDocumentoSolicitudPostulacionEndPoint(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Carga Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/postulante/cargarDocumentosPostulacion/{idSolicitud}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation("Carga Archivo al blob storage para solicitud de cesantias",
            Description = "Carga Archivo al blob storage para solicitud de cesantias",
            OperationId = "Solicitud.cargarDocumentosPostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(IFormFileCollection request, CancellationToken cancellationToken = default)
        {
            List<CargarDocumentosSolicitudDto> archivosCargados = new();
            Guid idSolicitud = Guid.Parse(Request.RouteValues["idSolicitud"].ToString());

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
            var userId = Guid.NewGuid().ToString();

            try
            {
                userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }
            var userIdParce = Guid.Parse(userId);
            return await _postulacionBussineLogic.CargarDocumentosSolicitud(archivosCargados, idSolicitud, userIdParce);
        }
    }
}
