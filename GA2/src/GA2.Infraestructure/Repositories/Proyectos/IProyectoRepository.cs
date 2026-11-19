using GA2.Application.Dto.Proyecto;
using GA2.Domain.Entities.Proyecto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.Proyectos
{
    public interface IProyectoRepository
    {
        Task<int> CREATE_BENEFICIARIO_PROYECTOS(BeneficiarioProyectoModel model);
        Task<int> CREATE_SUPERVISOR_PROYECTOS(SupervisorProyecoModel model);
        Task<Guid> CREAR_PROYECTO_CONSTRUCTORA(ProyectoConstructoraModel model);
        Task<int> CREAR_LICENCIA_URBANISMO_PROYECTO(LicenciaUrbanismoModel model);
        Task<int> LICENCIA_CONSTRUCCION_PROYECTO(LicenciaUrbanismoModel model);
        Task<List<ProyectoConstructoraResponse>> ObtenerProyecto(RequestConstructoraProyecto request);
        Task<int> CREATE_PP_POLIZA_PROYECTO(PolizaProyectoModel model);
        Task<LicenciaUrbanismoResponse> ObtenerLicenciaConduccionByProyectoId(Guid proyectoId);
        Task<ProyectoConstructoraResponse> ObtenerProyectoById(Guid proyectoId);
        Task<LicenciaUrbanismoResponse> ObtenerLicenciaUrbanimosByProyectoId(Guid proyectoId);
        Task<List<SupervisorResponse>> ObtenerSupervisorByProyectoId(Guid proyectoId);
        Task<List<BeneficiarioResponse>> ObtenerBeneficiarioByProyectoId(Guid proyectoId);
        Task<List<PolizaProyectoResponse>> ObtenerPolizaByProyectoId(Guid model);
        Task<int> ActualizarPolizaProyecto(PolizaProyectoModel model);
        Task<int> ActualizarProyectoConstructora(ProyectoConstructoraModel model);
        Task<List<TipoIncmuebles>> ObtenerTipoInmuble();
        Task<List<TipoIncmuebles>> ObtenerTipoViviendaProyecto(int codigo);
        Task<int> CrearTipoInmuebleProyecto(InmublesProyecto inmuebles);
        Task<int> CrearClaseInmueble(ClaseInmueble clases);
        Task<int> CrearListaAvance(ListaAvance clases);
        Task<List<InmublesProyectoResponse>> ObtenerTipoInmuebleProyecto(Guid proyectoId);
        Task<List<ClaseInmuebleResponse>> ObtenerClaseInmueble(Guid proyectoId);
        Task<List<ListaAvanceResponse>> ObtenerListaAvance(Guid proyectoId);
        Task<int> ActualizarEstadoProyectoContrato(Guid proyectoId, int codigoEstado);
        Task<Guid> CrearAdjudicacionProyecto(AdjudicacionProyecto adjudicacion);
        Task<int> CrearLogisticaAdjudicacion(LogisticaAdjudicacion logistica);
        Task<int> CrearEntregaViviendaAdjudicacion(EntregaViviendaAdjudicacion entrega);
        Task<int> GuardaInformacionSupervisorTecnico(SupervicionTecnica request);
        Task<IEnumerable<SupervicionTecnica>> ObtenerGuardaInformacionSupervisorTecnico(Guid request);
    }
}
