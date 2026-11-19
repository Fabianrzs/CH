using Azure.Core;
using GA2.Application.Dto;
using GA2.Application.Dto.Catalogs;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Embargos;
using GA2.Application.Dto.Parametrization;
using GA2.Application.Dto.Postulacion;
using GA2.Application.Dto.Reintegros;
using GA2.Application.Dto.Solicitud;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Main.ProviderRequest
{
    public class EmbargoProvider : ProviderRequestFactory, IEmbargosProvider
    {
        private readonly IOptions<ApisOptions> _optionsApis;
        public EmbargoProvider(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }
        public async Task<Response<IEnumerable<EmbargoCuentaConceptoDto>>> ObtenerEmbargoByConceptoCuentaId(int idCuenta)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerEmbargoConceptoCuentaPorIdentificadorCuenta + $"?idCuenta={idCuenta}";
            var response = await GetAsync<Response<IEnumerable<EmbargoCuentaConceptoDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<ConceptoDto>>> ObtenerConceptosGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerConceptosGA2;
            var response = await GetAsync<Response<IEnumerable<ConceptoDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<TipoCuentaDto>>> ObtenerTiposDeCuentasGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposDeCuenta;
            var response = await GetAsync<Response<IEnumerable<TipoCuentaDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<GA2.Application.Dto.Catalogs.EstadoCuentaDto>>> ObtenerEstadosDeCuentasGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerEstadosDeCuenta;
            var response = await GetAsync<Response<IEnumerable<GA2.Application.Dto.Catalogs.EstadoCuentaDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<EstadosClienteDto>>> ObtenerEstadosDeClientesGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerEstadosDeCliente;
            var response = await GetAsync<Response<IEnumerable<EstadosClienteDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<EstadoCausalClienteDto>>> ObtenerCausalesEstadosDeClientesGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerCausalesEstadosDeCliente;
            var response = await GetAsync<Response<IEnumerable<EstadoCausalClienteDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<bool>> AfectacionEmbargoConcepto(EmbargarMasivoDto embargarMasivoDto)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.AfectacionEnbargoMasivo;
            var response = await PostAsyncV2<Response<bool>>(embargarMasivoDto, new Uri(url), null);


            return response;
        }

        public async Task<Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>> ObtenerSolicitudesGA2(string identificacion)
        {
            PeticionConsultarSolicitudCesantiasDto peticionConsultarSolicitudCesantiasDto = new PeticionConsultarSolicitudCesantiasDto();
            peticionConsultarSolicitudCesantiasDto.Identificacion = identificacion;
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerSolicitudesGA2;
            var response = await PostAsyncV2<Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>>(peticionConsultarSolicitudCesantiasDto, new Uri(url), null);

            return response;
        }

        public async Task<Response<IEnumerable<ReintegroResponseDTO>>> ObtenerSolicitudesReintegroGA2(int tipoIdentificacion, string identificacion)
        {
            AfiliadoConsulta afiladoConsulta = new AfiliadoConsulta();
            afiladoConsulta.NumeroDocumento = Convert.ToInt64(identificacion);
            afiladoConsulta.TipoDocumento = tipoIdentificacion;
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerSolicitudesReintegroGA2 + "?TipoDocumento=" + afiladoConsulta.TipoDocumento + "&NumeroDocumento=" + afiladoConsulta.NumeroDocumento;
            var response = await GetAsync<Response<IEnumerable<ReintegroResponseDTO>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<CausalBloqueoDto>>> ObtenerCausalesEstadosDeCuentaGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerCausalesEstadosCuenta;
            var response = await GetAsync<Response<IEnumerable<CausalBloqueoDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<ObtenerTramiteSolicitudDto>> CrearSolicitudGA2(CrearSolicitudDto crearSolicitudDto)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.CrearSolicitudGA2;
            var response = await PostAsyncV2<Response<ObtenerTramiteSolicitudDto>>(crearSolicitudDto, new Uri(url), null);

            return response;
        }

        public async Task<Response<IEnumerable<TipoIdentificacionDto>>> ObtenerTiposIdentificacionGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposIdentificacionGA2;
            var response = await GetAsync<Response<IEnumerable<TipoIdentificacionDto>>>(new Uri(url));

            return response;
        }
     
        public async Task<Response<IEnumerable<TipoActorDto>>> ObtenerTiposAfiliacionGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposAfiliacionGA2;
            var response = await PostAsync<Response<IEnumerable<TipoActorDto>>>(null, new Uri(url));

            return response;
        }

        public async Task<Response<diffgram>> ObtenerValidacionBiometriaGA2(string identificacion)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerValidacionBiometriaGA2 + "?request=" + identificacion;
            var response = await GetAsync<Response<diffgram>>(new Uri(url));

            return response;
        }
        
        public async Task<Response<List<CuentasAfectadasSolicitud>>>ObtenerConceptosAfectadosSolicitudGA2(string identificacion)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerConceptosAfectadosSolicitudGA2 + "?request=" + identificacion;
            var response = await PostAsync<Response<List<CuentasAfectadasSolicitud>>>(null, new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<EmbargoByDto>>> ObtenerEmbargosGA2(string identificacion)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerEmbargosGA2 + "?request=" + identificacion;
            var response = await GetAsync<Response<IEnumerable<EmbargoByDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<CatalogoValorDto>>> ObtenerValoresCatalogosPorIdCatalogo(int CAT_ID)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerValoresCatalogosPorIdCatalogo + "?idCatalogo=" + CAT_ID;
            var response = await GetAsync<Response<IEnumerable<CatalogoValorDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<ValoresCategoriaDto>>> ObtenerFactorSubsidioCategoriaGA2(int anio)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerFactorSubsidioCategoriaGA2 + "?Anio=" + anio;
            var response = await PostAsync<Response<IEnumerable<ValoresCategoriaDto>>>(null,new Uri(url));

            return response;
        }

        public async Task<Response<ProyeccionClienteDto>> ObtenerProyeccionClienteGA2(string gradoId)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerProyeccionClienteGA2 + "?request=" + gradoId;
            var response = await GetAsync<Response<ProyeccionClienteDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<ProyeccionClienteDto>>> ObtenerSimCargoGA2(string gradoId)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerSimCargoGA2 + "?request=" + gradoId;
            var response = await GetAsync<Response<IEnumerable<ProyeccionClienteDto>>>( new Uri(url));

            return response;
        }

        public async Task<Response<ParamGeneralesCreditoYCarteraDto>> ObtenerParamGeneralesCreditoYCarteraGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerParamGeneralesCreditoYCarteraGA2;
            var response = await GetAsync<Response<ParamGeneralesCreditoYCarteraDto>>(new Uri(url));

            return response;
        }
        public async Task<Response<SmlvAnualDto>> ObtenerSalarioMinimoAnualGA2(ObtenerSmlvAnualDto anio)
        {
            string url = $"{this._optionsApis.Value.GA2}{RequestProviderConstants.ObtenerSalarioMinimoAnualGA2}?SMLV_ANIO={anio.SMLV_ANIO}&CTG_ID={anio.CTG_ID}";

            var response = await GetAsync<Response<SmlvAnualDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<CargoCategoriaDto>> ObtenerSimulacionCargoPorCategoriaGA2(SimulacionCargoCategoriaDto request)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerSimulacionCargoPorCategoriaGA2;
            var response = await PostAsyncV2<Response<CargoCategoriaDto>>(request, new Uri(url), null);

            return response;
        }

        public async Task<Response<IEnumerable<PaisDto>>> ObtenerPaisesGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerPaisesGA2;
            var response = await GetAsync<Response<IEnumerable<PaisDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<DepartamentoDto>>> ObtenerDepartamentosGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerDepartamentosGA2;
            var response = await GetAsync<Response<IEnumerable<DepartamentoDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<CiudadDto>>> ObtenerCiudadesGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerCiudadesGA2;
            var response = await GetAsync<Response<IEnumerable<CiudadDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<UnidadEjecutoraDto>>> ObtenerUnidadesEjecutorasGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerUnidadesEjecutorasGA2;
            var response = await GetAsync<Response<IEnumerable<UnidadEjecutoraDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<GradoDto>>> ObtenerGradosGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerGradosGA2;
            var response = await GetAsync<Response<IEnumerable<GradoDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<FuerzasDto>>> ObtenerFuerzasGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerFuerzasGA2;
            var response = await GetAsync<Response<IEnumerable<FuerzasDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<EstadoCivilDto>>> ObtenerEstadosCivilesGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerEstadosCivilesGA2;
            var response = await GetAsync<Response<IEnumerable<EstadoCivilDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<ProfesionDto>>> ObtenerOcupacionesGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerOcupacionesGA2;
            var response = await GetAsync<Response<IEnumerable<ProfesionDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<CategoriaDto>>> ObtenerCategoriasGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerCategoriasGA2;
            var response = await GetAsync<Response<IEnumerable<CategoriaDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<ActividadEconomicaDto>>> ObtenerActividadesEconomicasGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerActividadesEconomicasGA2;
            var response = await GetAsync<Response<IEnumerable<ActividadEconomicaDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<TipoMonedaDto>>> ObtenerMonedasGA2()
        { 
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerMonedasGA2;
            var response = await GetAsync<Response<IEnumerable<TipoMonedaDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<TipoDireccionDto>>> ObtenerTiposDireccionGA2()
        { 
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposDireccionGA2;
            var response = await GetAsync<Response<IEnumerable<TipoDireccionDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<TipoTelefonoDto>>> ObtenerTiposTelefonoGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposTelefonoGA2;
            var response = await GetAsync<Response<IEnumerable<TipoTelefonoDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<TipoCorreoDto>>> ObtenerTiposCorreoGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposCorreoGA2;
            var response = await GetAsync<Response<IEnumerable<TipoCorreoDto>>>(new Uri(url));

            return response;
        }
        public async Task<Response<IEnumerable<TipoSolicitudDto>>> ObtenerTiposSolicitudGA2()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerTiposSolicitudGA2;
            var response = await GetAsync<Response<IEnumerable<TipoSolicitudDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<EmbargosDto>>> ObtenerEmbargosJuzgadosGA2(ConsultaEmbargosfiltroDto request)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerEmbargosJuzgadosGA2;
            var response = await PostAsyncV2<Response<IEnumerable<EmbargosDto>>>(request, new Uri(url), null);

            return response;
        }

        public async Task<Response<List<BloqueoRecursosCreditoRespuesta>>> ConsultarBloqueoCuentaCredito(BloqueoRecursosCreditoDto request)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ConsultarBloqueoCuentaCredito;
            var response = await PostAsyncV2<Response<List<BloqueoRecursosCreditoRespuesta>>>(request, new Uri(url), null);

            return response;
        }

        public async Task<Response<JuzgadosGuidDto>> ObtenerJuzgadoPorIdGA2(Guid id)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerJuzgadoPorIdGA2 + "?request=" + id.ToString();
            var response = await GetAsync<Response<JuzgadosGuidDto>>(new Uri(url));

            return response;
        }
        public async Task<Response<EstadoSolicitudFondoDto>> ObtenerEstadoSolicitudFondo(string radicado)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.obtenerEstadoSolicitudFondo + "?noRadicado=" +  radicado;
            var response = await GetAsync<Response<EstadoSolicitudFondoDto>>(new Uri(url));

            return response;
        }
        public async Task<Response<string>> EliminarSolicitudes(string Identificacion)
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.EliminarSolicitudes + "?Identificacion=" + Identificacion;
            var response = await GetAsync<Response<string>>( new Uri(url)); 

            return response;
        }
        public async Task<Response<IEnumerable<ParametroIntegracionDto>>> ObtenerParametrosIntegraciones()
        {
            string url = this._optionsApis.Value.GA2 + RequestProviderConstants.ObtenerParametrosIntegraciones;
            var response = await GetAsync<Response<IEnumerable<ParametroIntegracionDto>>>(new Uri(url));

            return response;
        }

    }
}
