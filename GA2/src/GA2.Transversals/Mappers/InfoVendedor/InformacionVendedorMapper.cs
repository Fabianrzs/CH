using AutoMapper;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Entities.InfoVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GA2.Transversals.Mappers.GA2.InfoVendedor
{
    public class InformacionVendedorMapper : Profile
    {
        public InformacionVendedorMapper()
        {
            CreateMap<InformacionVendedorDto, InformacionVendedor>()
                .ForMember(x => x.IVEN_ID, map => map.MapFrom(dto => dto.Id))
                .ForMember(x => x.SOC_ID, map => map.MapFrom(dto => dto.Soc_id))
                .ForMember(x => x.IVEN_TIPO_DOC, map => map.MapFrom(dto => dto.Tipo_Doc))
                .ForMember(x => x.IVEN_NRO_DOC, map => map.MapFrom(dto => dto.Numero_Doc))
                .ForMember(x => x.IVEN_NOMBRE, map => map.MapFrom(dto => dto.Nombre_Vendedor))
                .ForMember(x => x.IVEN_DIRECCION, map => map.MapFrom(dto => dto.Direccion))
                .ForMember(x => x.IVEN_DEPARTAMANTO, map => map.MapFrom(dto => dto.Departamento))
                .ForMember(x => x.IVEN_CIUDAD, map => map.MapFrom(dto => dto.Ciudad))
                .ForMember(x => x.IVEN_CORREO, map => map.MapFrom(dto => dto.Correo_Electronico))
                .ForMember(x => x.IVEN_TELEFONO, map => map.MapFrom(dto => dto.Telefono))
                .ForMember(x => x.IVEN_FECHA_ENTREGA, map => map.MapFrom(dto => dto.Fecha_Entrega))
                .ForMember(x => x.IVEN_TIPO_CUENTA, map => map.MapFrom(dto => dto.Tipo_Cuenta))
                .ForMember(x => x.IVEN_BANCO, map => map.MapFrom(dto => dto.Banco))
                .ForMember(x => x.IVEN_NRO_CUENTA, map => map.MapFrom(dto => dto.Cuenta))
                .ForMember(x => x.PVD_CODD, map => map.MapFrom(dto => dto.CodProveeSeven))
                .ReverseMap();

        }
    }  

}
