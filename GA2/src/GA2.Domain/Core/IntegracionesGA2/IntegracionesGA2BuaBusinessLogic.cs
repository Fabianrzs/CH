using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class IntegracionesGA2BuaBusinessLogic : IIntegracionesGa2BuaBusinessLogic
    {
        //instancia obligatoria para IProvider
        // Instancia obligatoria
        private readonly IIntegracionesGA2Bua _integraciones;
        private readonly CHKeysOptions _optionsCH;

        public IntegracionesGA2BuaBusinessLogic(
            IIntegracionesGA2Bua integracionesGA2,
            IOptions<CHKeysOptions> optionsCH )
        {
            _integraciones = integracionesGA2;
            _optionsCH = optionsCH.Value;
        }

        public async Task<Response<string>> ObtenerBua(ObtenerBua bua)
        {
            var url = _optionsCH.UrlCajaHonor + "api/obtenerafiliadobua";
            var uri = new Uri(url);
            var response = new Response<string>();
            var buaResult = await _integraciones.ObtenetBua<object>(uri, bua, response.GetCorrelation(), response.GetCorrelation());
            response.Data = JsonConvert.SerializeObject(buaResult);
            return response;
        }
    }
}
