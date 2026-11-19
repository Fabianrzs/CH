using AutoMapper;
using GA2.Application.Dto.Postulacion;
using GA2.Application.Dto.Proyecto;
using GA2.Domain.Entities.Postulacion;
using GA2.Domain.Entities.Proyecto;
using System.Collections.Generic;

namespace GA2.Transversals.Mappers.GA2.Proyecto
{
    public class ProyectoConstructoraMapper : Profile
    {
        public ProyectoConstructoraMapper()
        {
            CreateMap<ProyectoConstructoraDto, ProyectoConstructoraModel>().ReverseMap();

            CreateMap<LicenciaUrbanismoDto, LicenciaUrbanismoModel>().ReverseMap();

            CreateMap<SupervisorProyecoDto, SupervisorProyecoModel>().ReverseMap(); 

            CreateMap<BeneficiarioProyectoDto, BeneficiarioProyectoModel>().ReverseMap();

            CreateMap<ProyectoConstructoraResponseDto, ProyectoConstructoraResponse>().ReverseMap();

            CreateMap<PolizaProyectoDto, PolizaProyectoModel>().ReverseMap();

            CreateMap<LicenciaUrbanismoResponseDto, LicenciaUrbanismoResponse>().ReverseMap();

            CreateMap<SupervisorResponseDto, SupervisorResponse>().ReverseMap();


            CreateMap<BeneficiarioResponseDto, BeneficiarioResponse>().ReverseMap();

            CreateMap<PolizaProyectoResponseDto, PolizaProyectoResponse>().ReverseMap();

            CreateMap<TipoIncmueblesDto, TipoIncmuebles>().ReverseMap();

            CreateMap<InmueblesDto, InmublesProyecto>().ReverseMap();

            CreateMap<ClaseInmuebleDto, ClaseInmueble>().ReverseMap();

            CreateMap<ListaAvanceDto, ListaAvance>().ReverseMap();

            CreateMap<InmublesProyectoResponseDto, InmublesProyectoResponse>().ReverseMap();

            CreateMap<ListaAvanceResponseDto, ListaAvanceResponse>().ReverseMap();

            CreateMap<ClaseInmuebleResponseDto, ClaseInmuebleResponse>().ReverseMap();

            CreateMap<RequestConstructoraProyectoDto, RequestConstructoraProyecto>().ReverseMap();

            CreateMap<AdjudicacionProyectoDto, AdjudicacionProyecto>().ReverseMap();

            CreateMap<LogisticaAdjudicacionDto, LogisticaAdjudicacion>().ReverseMap();

            CreateMap<EntregaViviendaAdjudicacionDto, EntregaViviendaAdjudicacion>().ReverseMap();

            CreateMap<SupervicionTecnica, SupervicionTecnicaDto>().ReverseMap();

        }
    }
}
