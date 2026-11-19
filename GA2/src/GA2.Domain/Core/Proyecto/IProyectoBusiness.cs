using GA2.Application.Dto.Proyecto;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Proyecto
{
    public interface IProyectoBusiness
    {
        Task<Response<Guid?>> CrearProyecto(ProyectoConstructoraDto constructoraDto);
        Task<Response<List<ProyectoConstructoraResponseDto>>> ObtenerProyectos(RequestConstructoraProyectoDto request);
        Task<Response<string>> CrearPolizas(List<PolizaProyectoDto> polizas);
        Task<Response<ProyectoConstructoraResponseDto>> ObtenerProyectosById(Guid id);
        Task<Response<string>> ActualizarPolizas(List<PolizaProyectoDto> polizas);
        Task<Response<string>> ActualizarProyectoPolizas(ProyectoConstructoraDto proyecto);
        Task<Response<List<TipoIncmueblesDto>>> ObtenerTipoInmueble();
        Task<Response<List<TipoIncmueblesDto>>> ObtenerTipoVivienda(int codigo);
        Task<Response<string>> CrearImuebleProyecto(InmublesProyectoDto inmueble);
        Task<Response<string>> ActualizarEstadoProyecto(ActualizarProyectoDto polizas);
        Task<Response<string>> CrearAdjudicacionVivienda(AdjudicacionViviendaDto adjudicacion);
        Task<Response<string>> GuardaInformacionSupervisorTecnico(List<SupervicionTecnicaDto> request);
        Task<Response<List<SupervicionTecnicaDto>>> ObtenerGuardaInformacionSupervisorTecnico(Guid request);
    }
}
