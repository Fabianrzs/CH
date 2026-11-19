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
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class GetDataVerificatePostulacion : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<VerificatePostulacionDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public GetDataVerificatePostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/GetDataVerificatePostulacion")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Get Data Verificate Postulacion de fondo solidaridad",
            Description = "Get Data Verificate Postulacion de fondo solidaridad",
            OperationId = "postulante.GetDataVerificatePostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<VerificatePostulacionDto>>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.GetDataVerificatePostulacion(id);
        }
    }
}
