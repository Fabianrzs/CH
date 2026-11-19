using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main
{

    public class ReportesRequestProvider : ProviderRequestFactory, IReportesRequestProvider
    {
        private readonly IOptions<ApisOptions> _optionsApis;

        public ReportesRequestProvider(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }

        public async Task<Response<IEnumerable<InformacionClienteDto>>> ReporteInformacionCliente(string identificacion)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ReporteInformacionCliente +$"?numeroDocumento={identificacion}";

            var response = await GetAsync<Response<IEnumerable<InformacionClienteDto>>>(new Uri(url));

            return response;

        }

        public async Task<Response<ReporteInformacionClienteDto>> ReporteObtenerInformacionCliente(string identificacion)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ReporteObtenerInformacionCliente + $"?identificacion={identificacion}";

            var response = await GetAsync<Response<ReporteInformacionClienteDto>>(new Uri(url));

            return response;

        }

        public async Task<Response<IEnumerable<TransferenciaCesantiasDto>>> ObtenerDatosOficioRespuesta(int idAfiliacion)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosOficioRespuesta +
                     $"?idAfiliacion={idAfiliacion}";

            var response = await GetAsync<Response<IEnumerable<TransferenciaCesantiasDto>>>(new Uri(url));

            return response;

        }

        public async Task<Response<IEnumerable<OficioImprocedenteDto>>> ObtenerDatosOficioImprocedente(int idAfiliacion)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosOficioImprocedente +
                     $"?idAfiliacion={idAfiliacion}";

            var response = await GetAsync<Response<IEnumerable<OficioImprocedenteDto>>>(new Uri(url));

            return response;
        }


        public async Task<Response<IEnumerable<OficiosAfiliacionDto>>> GenerarReportesAfiliacion(ParametrosReportesAfiliacionesDto parametros)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.GenerarReportesAfiliacion +
                     $"?IdAfiliacion={parametros.IdAfiliacion}&OpcionReporte={parametros.OpcionReporte}";

            var response = await GetAsync<Response<IEnumerable<OficiosAfiliacionDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<AfiliadoDto>> ObtenerNombresCliente(string identificacion)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerNombresCliente +
                     $"?numeroDocumento={identificacion}";

            var response = await GetAsync<Response<AfiliadoDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<List<AfiliadoDto>>> ObtenerNombresClientesPorDocumentos(List<string> documentos)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerNombresClientesPorDocumentos;
            return await PostAsyncV2<Response<List<AfiliadoDto>>>(documentos, new Uri(url));
        }

        public async Task<Response<List<ReporteClienteCorreosErradosDto>>> ObtenerClienteCorreosErrados()
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClientesCorreosErrados;
            return await GetAsync<Response<List<ReporteClienteCorreosErradosDto>>>(new Uri(url));
        }
    }
}
