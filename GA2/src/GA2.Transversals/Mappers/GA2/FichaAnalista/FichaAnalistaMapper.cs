using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.FichaAnalista;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.FichaAnalista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2.FichaAnalista
{
    public class FichaAnalistaMapper : Profile
    {
        public FichaAnalistaMapper()
        {
            CreateMap<FichaAnalistaDto, FichaAnalistas>()
                .ForMember(x => x.FA_ID, map => map.MapFrom(dto => dto.Fa_Id))
                .ForMember(x => x.SOC_ID, map => map.MapFrom(dto => dto.Soc_id))
                .ForMember(x => x.FA_NOMBRE, map => map.MapFrom(dto => dto.Fa_Nombre))
                .ForMember(x => x.FA_CARGO, map => map.MapFrom(dto => dto.Fa_Cargo))
                .ReverseMap();
        }
    }
}
