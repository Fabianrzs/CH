using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Domain.Entities.Escritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2.Escrituras
{
    public class EscrituraMapper : Profile
    {
        public EscrituraMapper(){
            CreateMap<CreacionEscritura,Escritura>()
                .ForMember(x=>x.ESC_ID, map=>map.MapFrom(dto=>dto.id_escritura))
                .ForMember(x => x.SOC_ID_SOLICITUD, map => map.MapFrom(dto => dto.Solicitud_id))
                .ForMember(x => x.CLI_IDENTIFICACION, map => map.MapFrom(dto => dto.cli_identificacion))
                .ForMember(x => x.ESC_ESCRITURA, map => map.MapFrom(dto => dto.escritura))
                .ForMember(x => x.ESC_FECHA, map => map.MapFrom(dto => dto.fecha_escritura))
                .ForMember(x => x.ESC_NOTARIA, map => map.MapFrom(dto => dto.notaria))
                .ReverseMap();
            CreateMap<ActualizacionEscrituras, Escritura>()
                .ForMember(x => x.ESC_ID, map => map.MapFrom(dto => dto.id_escritura))
                .ForMember(x => x.SOC_ID_SOLICITUD, map => map.MapFrom(dto => dto.solicitud_id))
                .ForMember(x => x.CLI_IDENTIFICACION, map => map.MapFrom(dto => dto.cli_identificacion))
                .ForMember(x => x.ESC_ESCRITURA, map => map.MapFrom(dto => dto.escritura))
                .ForMember(x => x.ESC_FECHA, map => map.MapFrom(dto => dto.fecha_escritura))
                .ForMember(x => x.ESC_NOTARIA, map => map.MapFrom(dto => dto.notaria))
                .ReverseMap();
            CreateMap<RequestObtenerEscrituras, Escritura>()
                .ForMember(x => x.SOC_ID_SOLICITUD, map => map.MapFrom(dto => dto.solicitud_id))
                .ForMember(x => x.CLI_IDENTIFICACION, map => map.MapFrom(dto => dto.cli_identificacion))
                .ReverseMap();
            CreateMap<ObtenerEscrituras, Escritura>()
                .ForMember(x => x.ESC_ID, map => map.MapFrom(dto => dto.id_escritura))
                .ForMember(x => x.SOC_ID_SOLICITUD, map => map.MapFrom(dto => dto.soliditud_id))
                .ForMember(x => x.CLI_IDENTIFICACION, map => map.MapFrom(dto => dto.cli_identificacion))
                .ForMember(x => x.ESC_ESCRITURA, map => map.MapFrom(dto => dto.escritura))
                .ForMember(x => x.ESC_FECHA, map => map.MapFrom(dto => dto.fecha_escritura))
                .ForMember(x => x.ESC_NOTARIA, map => map.MapFrom(dto => dto.notaria))
                .ReverseMap();
        }
    }
}
