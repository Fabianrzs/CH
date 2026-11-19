using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.PuntosAtencion;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.PuntosAtencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2.PuntosAtencion
{
    public class PuntoAtencionMapper : Profile
    {
        public PuntoAtencionMapper(){
            CreateMap<PuntosAtencionDto,PuntoAtencion>()
                .ForMember(x=>x.PA_ID, map=>map.MapFrom(dto=>dto.Id_PuntoAtencion))
                .ForMember(x => x.PA_PUNTO, map => map.MapFrom(dto => dto.PuntoAtencion))
                .ForMember(x => x.PA_TIPO_AFILIADO, map => map.MapFrom(dto => dto.TipoAfiliadoPunto))
                .ReverseMap();
        }
    }
}
