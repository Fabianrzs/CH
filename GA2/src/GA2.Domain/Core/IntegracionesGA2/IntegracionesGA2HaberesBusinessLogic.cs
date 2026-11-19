using GA2.Application.Dto.Haberes;
using GA2.Application.Main;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class IntegracionesGA2HaberesBusinessLogic : IIntegracionesGA2HaberesBusinessLogic
    {
        private readonly IIntegracionesGA2Haberes _integraciones;
        private readonly CHKeysOptions _optionsCH;

        public IntegracionesGA2HaberesBusinessLogic(IIntegracionesGA2Haberes integraciones,
                                                    IOptions<CHKeysOptions> optionsCH)
        {
            _integraciones = integraciones;
            _optionsCH = optionsCH.Value;
        }

        public async Task<Response<string>> GenerarReporteHaberes(HaberesRequestDto request)
        {
            Response<string> response = new();
            var url = _optionsCH.UrlCajaHonor + "api/generarreportehaberes";
            var uri = new Uri(url);
            return await _integraciones.GenerarReporteHaberes<Response<string>>(uri, request, response.GetCorrelation(), response.GetCorrelation());
        }
    }
}
