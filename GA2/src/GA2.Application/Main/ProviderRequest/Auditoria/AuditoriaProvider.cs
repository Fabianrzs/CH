using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Main.ProviderRequest.Auditoria
{
    public class AuditoriaProvider : ProviderRequestFactory, IAuditoriaProvider
    {
        private readonly IOptions<ApisOptions> _optionsApis;
        public AuditoriaProvider(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }
        public async Task<Response<bool>> GuardarAuditoria(AuditoriaDto request)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.InsertarLogAuditoria;

            var response = await PostAsyncV2<Response<bool>>(request, new Uri(url), null);
            return response;
        }
    }
}
