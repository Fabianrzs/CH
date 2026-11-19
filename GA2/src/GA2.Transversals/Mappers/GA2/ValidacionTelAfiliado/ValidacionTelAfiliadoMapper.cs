using AutoMapper;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using GA2.Domain.Entities.ValidacionTelAfiliado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2.ValidacionTelAfiliado
{
    public class ValidacionTelAfiliadoMapper : Profile
    {
        public ValidacionTelAfiliadoMapper()
        {
            CreateMap<ValidacionTelAfiliados, ValidacionTelAfiliadoDto>()
                .ForMember(x => x.Val_Id, map => map.MapFrom(dto => dto.VAL_ID))
                .ForMember(x => x.Soc_Id, map => map.MapFrom(dto => dto.SOC_ID))
                .ForMember(x => x.Conf_Valor_Inmueble, map => map.MapFrom(dto => dto.VAL_CONF_VAL_INM))
                .ForMember(x => x.Anio_Fuerzas, map => map.MapFrom(dto => dto.VAL_ANIOS_FUERZA))
                .ForMember(x => x.Lugar_Trabajo, map => map.MapFrom(dto => dto.VAL_LUGAR_TRABAJO))
                .ForMember(x => x.Cargo_Actual, map => map.MapFrom(dto => dto.VAL_CARGO_ACTUAL))
                .ForMember(x => x.Valor_Arriendo, map => map.MapFrom(dto => dto.VAL_VALOR_ARRIENDO))
                .ForMember(x => x.Valor_Activos, map => map.MapFrom(dto => dto.VAL_VALOR_ACTIVOS))
                .ForMember(x => x.Valor_Pasivos, map => map.MapFrom(dto => dto.VAL_VALOR_PASIVOS))
                .ForMember(x => x.Monto_Recursos_Propios, map => map.MapFrom(dto => dto.VAL_MONTOS_RECURSOS_PROPIOS))
                .ForMember(x => x.Observaciones, map => map.MapFrom(dto => dto.VAL_OBSERVACIONES))
                .ReverseMap();
        }
    }
}
