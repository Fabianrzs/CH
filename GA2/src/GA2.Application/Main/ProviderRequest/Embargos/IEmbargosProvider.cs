using GA2.Application.Dto;
using GA2.Application.Dto.Catalogs;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Embargos;
using GA2.Application.Dto.Parametrization;
using GA2.Application.Dto.Postulacion;
using GA2.Application.Dto.Solicitud;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Application.Main.ProviderRequest
{
    public interface IEmbargosProvider
    {
        Task<Response<IEnumerable<EmbargoCuentaConceptoDto>>> ObtenerEmbargoByConceptoCuentaId(int idCuenta);
        Task<Response<IEnumerable<ConceptoDto>>> ObtenerConceptosGA2();
        Task<Response<IEnumerable<EmbargoByDto>>> ObtenerEmbargosGA2(string identificacion);
        Task<Response<IEnumerable<EmbargosDto>>> ObtenerEmbargosJuzgadosGA2(ConsultaEmbargosfiltroDto request);
        Task<Response<JuzgadosGuidDto>> ObtenerJuzgadoPorIdGA2(Guid id);
        Task<Response<IEnumerable<TipoActorDto>>> ObtenerTiposAfiliacionGA2();
        Task<Response<IEnumerable<TipoCuentaDto>>> ObtenerTiposDeCuentasGA2();
        Task<Response<IEnumerable<TipoIdentificacionDto>>> ObtenerTiposIdentificacionGA2();
        Task<Response<IEnumerable<CatalogoValorDto>>> ObtenerValoresCatalogosPorIdCatalogo(int CAT_ID);
        Task<Response<IEnumerable<GA2.Application.Dto.Catalogs.EstadoCuentaDto>>> ObtenerEstadosDeCuentasGA2();
        Task<Response<IEnumerable<EstadosClienteDto>>> ObtenerEstadosDeClientesGA2();
        Task<Response<IEnumerable<EstadoCausalClienteDto>>> ObtenerCausalesEstadosDeClientesGA2();
        Task<Response<IEnumerable<CausalBloqueoDto>>> ObtenerCausalesEstadosDeCuentaGA2();
        Task<Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>> ObtenerSolicitudesGA2(string identificacion);
        Task<Response<IEnumerable<ReintegroResponseDTO>>> ObtenerSolicitudesReintegroGA2(int tipoIdentificacion, string identificacion);
        Task<Response<List<CuentasAfectadasSolicitud>>> ObtenerConceptosAfectadosSolicitudGA2(string cuentasAfectadas);
        Task<Response<List<BloqueoRecursosCreditoRespuesta>>> ConsultarBloqueoCuentaCredito(BloqueoRecursosCreditoDto request);
        Task<Response<bool>> AfectacionEmbargoConcepto(EmbargarMasivoDto embargarMasivoDto);
        Task<Response<ObtenerTramiteSolicitudDto>> CrearSolicitudGA2(CrearSolicitudDto crearSolicitudDto);
        Task<Response<diffgram>> ObtenerValidacionBiometriaGA2(string identificacion); 
        Task<Response<IEnumerable<ValoresCategoriaDto>>> ObtenerFactorSubsidioCategoriaGA2(int anio);
        Task<Response<ProyeccionClienteDto>> ObtenerProyeccionClienteGA2(string gradoId);
        Task<Response<IEnumerable<ProyeccionClienteDto>>> ObtenerSimCargoGA2(string gradoId);
        Task<Response<ParamGeneralesCreditoYCarteraDto>> ObtenerParamGeneralesCreditoYCarteraGA2();
        Task<Response<SmlvAnualDto>> ObtenerSalarioMinimoAnualGA2(ObtenerSmlvAnualDto anio);
        Task<Response<CargoCategoriaDto>> ObtenerSimulacionCargoPorCategoriaGA2(SimulacionCargoCategoriaDto request);
        Task<Response<IEnumerable<PaisDto>>> ObtenerPaisesGA2();
        Task<Response<IEnumerable<CiudadDto>>> ObtenerCiudadesGA2();
        Task<Response<IEnumerable<DepartamentoDto>>> ObtenerDepartamentosGA2();
        Task<Response<IEnumerable<UnidadEjecutoraDto>>> ObtenerUnidadesEjecutorasGA2();
        Task<Response<IEnumerable<GradoDto>>> ObtenerGradosGA2();
        Task<Response<IEnumerable<FuerzasDto>>> ObtenerFuerzasGA2();
        Task<Response<IEnumerable<EstadoCivilDto>>> ObtenerEstadosCivilesGA2();
        Task<Response<IEnumerable<ProfesionDto>>> ObtenerOcupacionesGA2();
        Task<Response<IEnumerable<CategoriaDto>>> ObtenerCategoriasGA2();
        Task<Response<IEnumerable<ActividadEconomicaDto>>> ObtenerActividadesEconomicasGA2();
        Task<Response<IEnumerable<TipoMonedaDto>>> ObtenerMonedasGA2();
        Task<Response<IEnumerable<TipoDireccionDto>>> ObtenerTiposDireccionGA2();
        Task<Response<IEnumerable<TipoTelefonoDto>>> ObtenerTiposTelefonoGA2();
        Task<Response<IEnumerable<TipoCorreoDto>>> ObtenerTiposCorreoGA2();
        Task<Response<IEnumerable<TipoSolicitudDto>>> ObtenerTiposSolicitudGA2();
        Task<Response<EstadoSolicitudFondoDto>> ObtenerEstadoSolicitudFondo(string radicado);
        Task<Response<string>> EliminarSolicitudes(string Identificacion);

        Task<Response<IEnumerable<ParametroIntegracionDto>>> ObtenerParametrosIntegraciones();
    }
}
