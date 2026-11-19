using GA2.Application.Dto.Parametrization;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.IntegracionesConfig
{
    public class EstadoIntegracionClient : ProviderRequestFactory, IEstadoIntegracionProvider
    {
        private readonly IOptions<ApisOptions> _optionsApis;
        public EstadoIntegracionClient(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }
        private async Task<Response<IEnumerable<ParametroIntegracionDto>>> ApiObtenerParametrosIntegraciones()
        {
            try
            {
                string url = _optionsApis.Value.GA2 + RequestProviderConstants.ObtenerParametrosIntegraciones;

                var response = await GetAsync<Response<IEnumerable<ParametroIntegracionDto>>>( new Uri(url));

                return response;
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ParametroIntegracionDto>>()
                {
                    Data = new List<ParametroIntegracionDto>(),
                    IsSuccess = false,
                    ReturnMessage = ex.Message
                };
            }
        }

        public async Task<bool> IntegracionActiva(string integracion)
        {
            var parametrosIntegraciones = await ObtenerParametrosIntegraciones();
            if (!parametrosIntegraciones.Any())
            {
                return false;
            }
            return bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.ToUpper().Equals(integracion) && p.PARAMETRO.ToUpper().Equals("ESTADO")).VALOR);
        }

        public async Task<IEnumerable<ParametroIntegracion>> ObtenerParametrosIntegraciones()
        {
            var result = await ApiObtenerParametrosIntegraciones();
            if (result.IsSuccess)
            {
                return result.Data.ToList().Select(x => new ParametroIntegracion
                {
                    PARAM_ID = x.ParametroId,
                    INTEGRACION = x.Integracion,
                    PARAMETRO = x.Parametro,
                    VALOR = x.Valor,
                });

            }

            return new List<ParametroIntegracion> { };
        }
    }
}
