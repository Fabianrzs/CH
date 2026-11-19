using GA2.Application.Dto.Constructora;
using GA2.Domain.Entities.Constructora;
using AutoMapper;

namespace GA2.Transversals.Mappers
{
    public class ConstructoraMapper : Profile
    {
        public ConstructoraMapper()
        {

            CreateMap<ConstructoraDto, Constructora>().ReverseMap();
        }

    }
}
