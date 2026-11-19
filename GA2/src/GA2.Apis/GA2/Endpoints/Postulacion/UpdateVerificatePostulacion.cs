
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class UpdateVerificatePostulacion : BaseAsyncEndpoint.WithRequest<VeficatePostulacionDto>
        .WithResponse<Response<string>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public UpdateVerificatePostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/UpdateVerificatePostulacion")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar la informacion de observacion en verficacion de postulacion",
            Description = "Actualizar la observacion de verificacion de postulacion",
            OperationId = "postulante.UpdateVerificatePostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(VeficatePostulacionDto request, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.UpdateDateVerificatePostulacion(request);
        }
    }
}
