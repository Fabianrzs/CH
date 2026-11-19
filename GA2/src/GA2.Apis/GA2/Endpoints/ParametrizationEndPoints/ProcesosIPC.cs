using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
   
    //[Authorize]
    public class ProcesosIPC : BaseAsyncEndpoint.WithRequest<IPCDto>.WithResponse<Response<IPCDto>>
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ProcesosIPC(IParametrizacionBusinessLogic parametrizationBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/parametrizacion/ProcesosIPC")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IPCDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = " crear los procesos de parametrizacion legal Gmf",
            Description = "crear los procesos de parametrizacion legal Gmf",
            OperationId = "crearprocesoparametrizacion.IdIPC",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<IPCDto>>> HandleAsync(IPCDto request, CancellationToken cancellationToken = default)
        {
            return await _parametrizationBusinessLogic.ProcesosIPC(request);
        }
    }
}
