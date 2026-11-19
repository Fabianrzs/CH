using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Cierres.RequestCierreDiarioCartera;
using GA2.Domain.Core;
using GA2.Domain.Entities.Cartera.Pagos;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpCompress;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// CierreDiarioCartera
    /// </summary>
    [Authorize]
    public class CierreDiarioCartera : BaseAsyncEndpoint
        .WithRequest<RequestCierreDiarioCarteraDto>
        .WithResponse<Response<IEnumerable<CierreDiarioCarteraDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CierreDiarioCartera(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CierreDiarioCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<CierreDiarioCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Ejecutar Cierre Diario",
           Description = "Ejecutar Cierre Diario",
           OperationId = "Credito.CierreDiarioCartera",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CierreDiarioCarteraDto>>>> HandleAsync(RequestCierreDiarioCarteraDto request, CancellationToken cancellationToken = default)
        {
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            if (request.Actualiza)
            {

                var response = await this._creditoBusinessLogic.CierreDiarioCartera(request);
                if (response.IsSuccess && response.Data.Any())
                {
                    var responseCierre = await this._creditoBusinessLogic.CierreDiarioCarteraInteresMoraSeven(request, cancellationToken, response.Data.ToList());

                    var regexLeasing = Regex.Match(
                        responseCierre.ReturnMessage,
                        @"leasing:\s*(\d+)",
                        RegexOptions.IgnoreCase);

                    int? ccIdLeasing = regexLeasing.Success
                        ? int.Parse(regexLeasing.Groups[1].Value)
                        : null;

                    var regexHipotecario = Regex.Match(
                        responseCierre.ReturnMessage,
                        @"hipotecarios:\s*(\d+)",
                        RegexOptions.IgnoreCase);

                    int? ccIdHipotecario = regexHipotecario.Success
                        ? int.Parse(regexHipotecario.Groups[1].Value)
                        : null;
                    response.Data.Where(x => x.TipoCredito == "CRÉDITO HIPOTECARIO").ToList()
                        .ForEach(x =>x.ComprobanteInternoHipotecario = (int)ccIdHipotecario);

                    response.Data.Where(x => x.TipoCredito == "LEASING HABITACIONAL").ToList()
                        .ForEach(x => x.ComprobanteInternoLeasing = (int)ccIdLeasing );
                }
                return response;
            }
            return await this._creditoBusinessLogic.CierreDiarioCartera(request);
        }
    }
}