using Ardalis.ApiEndpoints;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using System.Linq;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class AnularVericatedPostulacion : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<string>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public AnularVericatedPostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/AnularVericatedPostulacion")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Anular verificacion de postulacion",
            Description = "Anular la verificacion de postulacion",
            OperationId = "postulante.AnularVericatedPostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
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
            return await _postulacionBussineLogic.AnularVerificatePostulacion(request, userIdParce);
        }
    }
}
