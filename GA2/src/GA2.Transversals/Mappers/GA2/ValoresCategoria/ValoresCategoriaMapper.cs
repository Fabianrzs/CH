using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.ValoresCategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2.ValoresCategoria
{
    public class ValoresCategoriaMapper : Profile
    {
        public ValoresCategoriaMapper()
        {
            CreateMap<ValoresCategoriaDto, ValoresCategorias>()
                .ForMember(x => x.CRG_ID, map => map.MapFrom(dto => dto.Crg_Id))
                .ForMember(x => x.CRG_DESCRIPCION, map => map.MapFrom(dto => dto.Crg_Descripcion))
                .ForMember(x => x.CTG_ID, map => map.MapFrom(dto => dto.Ctg_Id))
                .ForMember(x => x.CRG_FACTOR_SUBS, map => map.MapFrom(dto => dto.Crg_Factor_Subs))
                .ForMember(x => x.CRG_ANIOS_ASCENSO, map => map.MapFrom(dto => dto.Crg_Anios_Ascenso))
                .ForMember(x => x.CRA_SALARIO_BASICO, map => map.MapFrom(dto => dto.Cra_Salario_Basico))
                .ForMember(x => x.CRA_SALARIO_TOTAL, map => map.MapFrom(dto => dto.Cra_Salario_Total))
                .ForMember(x => x.CRA_ANIO, map => map.MapFrom(dto => dto.Cra_Anio))
                .ForMember(x => x.CRG_PROYECTA_SM, map => map.MapFrom(dto => dto.Crg_ProyectaSM))
                .ForMember(x => x.CRG_PROYECTA_SUBSIDIO, map => map.MapFrom(dto => dto.Crg_ProyectaSubsidio))
                .ReverseMap();
        }
    }
}
