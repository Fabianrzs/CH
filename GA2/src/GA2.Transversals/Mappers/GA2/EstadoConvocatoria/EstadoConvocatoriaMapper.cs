using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;

namespace GA2.Transversals.Mappers.GA2.EstadoConvocatoria
{
    public class EstadoConvocatoriaMapper : Profile
    {

        public EstadoConvocatoriaMapper()
        {
            CreateMap<EstadoConvocatoriaModel, EstadoConvocatoriaDto>()
                    .ForMember(x => x.IdEstado, map => map.MapFrom(dto => dto.IdEstado))
                    .ForMember(x => x.NombreEstado, map => map.MapFrom(dto => dto.NombreEstado))
                    .ForMember(x => x.FechaInsercion, map => map.MapFrom(dto => dto.FechaInsercion))
                    .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.Estado))
                    .ReverseMap();

            CreateMap<EstadoConvocatoriaDto, EstadoConvocatoriaModel>()
        .ForMember(x => x.IdEstado, map => map.MapFrom(dto => dto.IdEstado))
        .ForMember(x => x.NombreEstado, map => map.MapFrom(dto => dto.NombreEstado))
        .ForMember(x => x.FechaInsercion, map => map.MapFrom(dto => dto.FechaInsercion))
        .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.Estado))
        .ReverseMap();
        }
    }
}
