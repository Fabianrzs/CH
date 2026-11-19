using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GA2.Application.Main
{

    public class CierreMensualRequestProvider : ProviderRequestFactory, ICierreMensualRequestProvider
    {
        private readonly IOptions<ApisOptions> _optionsApis;

        public CierreMensualRequestProvider(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }

        public async Task<Response<RespuestaCierreMensualCuentasDto>> CierreMensualCuentas(ProgramacionCierreMensualCuentas programacion)
        {
            ProgramacionCierreMensualCuentasDto cierreMensualDto = new ProgramacionCierreMensualCuentasDto() {
                TipoProceso = programacion.TIPO_PROCESO,
                ConceptosAhorro = programacion.CONCEPTOS_AHORRO,
                ConceptosCesantias = programacion.CONCEPTOS_CESANTIAS,
                CuentaAbonoAhorro = programacion.CUENTA_ABONO_AHORRO,
                CuentaAbonoCesantias = programacion.CUENTA_ABONO_CESANTIAS,
                Estado = programacion.ESTADO,
                FechaCargue = programacion.FECHA_CARGUE,
                FechaProcesamiento = programacion.FECHA_PROCESAMIENTO,
                GeneraActualizacion = programacion.GENERA_ACTUALIZACION,
                Id = programacion.ID,
                IPC = programacion.IPC,
                Anio = programacion.ANIO,
                Mes = programacion.MES,
                UsuarioProcesamiento = programacion.USUARIO_PROCESAMIENTO,
                CreadoPor = programacion.CCT_MENSUAL_CREADOPOR,
                FechaCreacion = programacion.CCT_MENSUAL_FECHA_CREACION,
                ModificadoPor = programacion.CCT_MENSUAL_MODIFICADOPOR,
                FechaModificacion = programacion.CCT_MENSUAL_FECHA_MODIFICACION

            };         

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.CierreMensual;

            return await PostAsyncV2<Response<RespuestaCierreMensualCuentasDto>>(cierreMensualDto, new Uri(url), null);

        }

        public async Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensualCuentas()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ConsultarProgramacionCierreMensual;

            return await GetAsync<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>>(new Uri(url));

        }

        public async Task<Response<IEnumerable<ReporteCierreMensualCuentasDto>>> ReporteCierreMensualCuentas(Guid Id)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ReporteCierreMensual +
                    $"?request={Id}";
            var response = await GetAsync< Response<IEnumerable<ReporteCierreMensualCuentasDto>>>(new Uri(url));

            return response;

        }

        public async Task<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>> ObtenerTodoDatosVerificacionDerechos(VerificacionDerechosByFechaDto request)
        {
            var ini = request.FechaInicial.ToString("yyyy-MM-dd");
            var fin = request.FechaFinal.ToString("yyyy-MM-dd");
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosVerificacionDerechosCuentas + $"?FechaInicial={ini}&FechaFinal={fin}";
        

            return await GetAsync<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>>(new Uri(url)); ;
        }

        public async Task<Response<DatosVerificacionDerechoCuentaDto>> ActualizarECTCuentas(ActualizarEctCuentasDto request)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ActualizarECTCuentas;

            var response = await PostAsyncFile<Response<DatosVerificacionDerechoCuentaDto>> (request, new Uri(url));

           
            return response;
        }
        public async Task<Response<List<DatosVerificacionDerechoCuentaDto>>> ActualizarECTCuentasList(List<ActualizarEctCuentasDto> request)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ActualizarECTCuentasList;
            try
            {
                var response = await PostAsync<Response<List<DatosVerificacionDerechoCuentaDto>>>(request, new Uri(url));


                return response;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<Response<IEnumerable<DatosCuentasAdministracionCesantiasDto>>> ObtenerDatosCuentasAdministracionCesantias(VerificacionDerechosByFechaDto request)
        {
            var ini = request.FechaInicial.ToString("yyyy-MM-dd");
            var fin = request.FechaFinal.ToString("yyyy-MM-dd");
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosCuentasAdministracionCesantias + $"?FechaInicial={ini}&FechaFinal={fin}";


            return await GetAsync<Response<IEnumerable<DatosCuentasAdministracionCesantiasDto>>>(new Uri(url)); ;
        }


        public async Task<Response<IEnumerable<DatosCuentasAVDto>>> ObtenerDatosCuentasAV(VerificacionDerechosByFechaDto request)
        {
            var ini = request.FechaInicial.ToString("yyyy-MM-dd");
            var fin = request.FechaFinal.ToString("yyyy-MM-dd");
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosCuentasAV + $"?FechaInicial={ini}&FechaFinal={fin}";


            return await GetAsync<Response<IEnumerable<DatosCuentasAVDto>>>(new Uri(url)); ;
        }

        public async Task<Response<IEnumerable<ObtenerCuentasAVACDto>>> ObtenerDatosCuentasAVAC(VerificacionDerechosByFechaDto request)
        {
            var ini = request.FechaInicial.ToString("yyyy-MM-dd");
            var fin = request.FechaFinal.ToString("yyyy-MM-dd");
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosCuentasAVAC + $"?FechaInicial={ini}&FechaFinal={fin}";


            return await GetAsync<Response<IEnumerable<ObtenerCuentasAVACDto>>>(new Uri(url)); ;
        }

        public async Task<Response<IEnumerable<ObtenerCuentasAVAVDto>>> ObtenerDatosCuentasAVAV(VerificacionDerechosByFechaDto request)
        {
            var ini = request.FechaInicial.ToString("yyyy-MM-dd");
            var fin = request.FechaFinal.ToString("yyyy-MM-dd");
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosCuentasAVAV + $"?FechaInicial={ini}&FechaFinal={fin}";


            return await GetAsync<Response<IEnumerable<ObtenerCuentasAVAVDto>>>(new Uri(url)); ;
        }
        public async Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtemerCedulaCuentaAVA()
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCedulaCuentaAVAV;


            return await GetAsync<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>(new Uri(url)); ;
        }
        public async Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtemerCedulaCuentaAVC()
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCedulaCuentaAVC;


            return await GetAsync<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>(new Uri(url)); ;
        }
        public async Task<Response<IEnumerable<DatosCedulasCesantiasDto>>> ObtenerCedulaCuentaCesantias()
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCedulaCuentaCesantias;


            return await GetAsync<Response<IEnumerable<DatosCedulasCesantiasDto>>>(new Uri(url)); ;
        }
        public async Task<Response<DatosInfoCensatiaByCuentaPeriodoDto>> ObtenerInfoCuentaByPeriodo(ObtenerInfoCedulaRequestDto request)
        {
            var ini = request.CuentaId;
            var fin = request.Periodo;
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerInfoCuentaByPeriodo + $"?CuentaId={ini}&Periodo={fin}";


            return await GetAsync<Response<DatosInfoCensatiaByCuentaPeriodoDto>>(new Uri(url)); ;
        }

        public async Task<Response<IEnumerable<ObtenerCuentasAVCDto>>> ObtenerDatosCuentasAVC(VerificacionDerechosByFechaDto request)
        {
            var ini = request.FechaInicial.ToString("yyyy-MM-dd");
            var fin = request.FechaFinal.ToString("yyyy-MM-dd");
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerDatosCuentasAVC + $"?FechaInicial={ini}&FechaFinal={fin}";


            return await GetAsync<Response<IEnumerable<ObtenerCuentasAVCDto>>>(new Uri(url)); ;
        }
    }
}
