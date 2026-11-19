using AutoMapper;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2.DatosAliadoEstrategico
{
    public class DatosAliadoEstrategicoMapper : Profile
    {
        public DatosAliadoEstrategicoMapper()
        {
            CreateMap<DatosAliadoEstrategicos, DatosAliadoEstrategicoDto>()
                .ForMember(x => x.Dat_Id, map => map.MapFrom(dto => dto.DAT_ID))
                .ForMember(x => x.Soc_Id, map => map.MapFrom(dto => dto.SOC_ID))
                .ForMember(x => x.Nombre_Aliado, map => map.MapFrom(dto => dto.DAT_NOMBRE_ALIADO))
                .ForMember(x => x.Fecha_Pago_Estudio_Titulo, map => map.MapFrom(dto => dto.DAT_FECHA_PAGO_ESTUDIO_TITULO))
                .ForMember(x => x.Fecha_Recepcion_Estudio_Titulo, map => map.MapFrom(dto => dto.DAT_FECHA_RECEPCION_ESTUDIO_TITULO))
                .ForMember(x => x.Concepto, map => map.MapFrom(dto => dto.DAT_CONCEPTO))
                .ForMember(x => x.Tipo_Parqueadero, map => map.MapFrom(dto => dto.DAT_TIPO_PARQUEADERO))
                .ForMember(x => x.percentageExtraBonus, map => map.MapFrom(dto => dto.DAT_PORCENTAJE_EXTRAPRIMA))
                .ForMember(x => x.talla, map => map.MapFrom(dto => dto.SOC_TALLA))
                .ForMember(x => x.peso, map => map.MapFrom(dto => dto.SOC_PESO))
                .ForMember(x => x.dealInsurance, map => map.MapFrom(dto => dto.DAT_DEAL_INSURANCE))
                .ForMember(x => x.dealInsuranceHome, map => map.MapFrom(dto => dto.DAT_DEAL_INSURANCE_HOME))
                .ForMember(x => x.imc, map => map.MapFrom(dto => dto.DAT_IMC))
                .ForMember(x => x.insuranceID, map => map.MapFrom(dto => dto.DAT_INSURANCE_ID))
                .ForMember(x => x.homeInsuranceID, map => map.MapFrom(dto => dto.DAT_HOME_INSURANCE_ID))
                .ReverseMap();
        }
    }
}