using GA2.Application.Dto.Reintegros;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GA2.Application.Main.ProviderRequest.Reintegro
{
    public class ReintegroProvider : ProviderRequestFactory, IReintegroProviders
    {
        private readonly IOptions<ApisOptions> _optionsApis;
        public ReintegroProvider(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }
        public async Task<Response<string>> ActualizarValorSaldoPorcentaje(int idAfiliado, decimal valor)
        {
            var updatePorcentaje = new UpdateValorPercent()
            {
                IdAfiliado = idAfiliado,
                Valor = valor,
            };
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ActualizarSaldoPorcentajeGA2;
            return await PostAsyncV2<Response<string>>(updatePorcentaje, new Uri(url), null);
        }
        public async Task<Response<string>> ActualizarValorSaldo(int idAfiliado, decimal valor)
        {
            var updatePorcentaje = new UpdateValorPercent()
            {
                IdAfiliado = idAfiliado,
                Valor = valor,
            };
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.UpdateValueSolicitado;
            return await PostAsyncV2<Response<string>>(updatePorcentaje, new Uri(url), null);
        }

        public async Task<bool> ListadoSolicitudByAfiliado(int idAfiliado)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ListadoSolicitudByAfiliado;
            return await PostAsyncV2<bool>(idAfiliado, new Uri(url), null);
        }
        public async Task<Response<string>> CleanValorSaldo(int idAfiliado, decimal valor)
        {
            var updatePorcentaje = new UpdateValorPercent()
            {
                IdAfiliado = idAfiliado,
                Valor = valor,
            };
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.CleanValueAfiliado;
            return await PostAsyncV2<Response<string>>(updatePorcentaje, new Uri(url), null);
        }
    }

}
