using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Entities;

namespace GA2.Transversals.Mappers
{
    public class ReintegrosMapper : Profile
    {
        public ReintegrosMapper()
        {
            CreateMap<DetalleReintegro, DetalleReintegroDto>()
                .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.CREADO_POR))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FECHA_CREACION))
                .ForMember(x => x.FechaModificado, map => map.MapFrom(dto => dto.FECHA_MODIFICACION))
                .ForMember(x => x.IdDetalleReintegro, map => map.MapFrom(dto => dto.RT_REI_DET_ID))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.IdReintegro, map => map.MapFrom(dto => dto.RT_REI_ID))
                .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.MODIFICADO_POR))
                .ReverseMap();

            CreateMap<EstadoReintegros, EstadoReintegrosDto>()
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
                .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.CREADO_POR))
                .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.DESCRIPCION))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FECHA_CREACION))
                .ForMember(x => x.FechaModificado, map => map.MapFrom(dto => dto.FECHA_MODIFICACION))
                .ForMember(x => x.IdEstadoReintegro, map => map.MapFrom(dto => dto.RT_ESTADO_ID))
                .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.MODIFICADO_POR))
                .ReverseMap();

            CreateMap<Reintegros, ReintegrosDto>()
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FECHA_CREACION))
                .ForMember(x => x.FechaModificado, map => map.MapFrom(dto => dto.FECHA_MODIFICACION))
                .ForMember(x => x.FechaReintegro, map => map.MapFrom(dto => dto.FECHA_REINTEGRO))
                .ForMember(x => x.IdEstadoReintegro, map => map.MapFrom(dto => dto.RT_ESTADO_ID))
                .ForMember(x => x.IdReintegro, map => map.MapFrom(dto => dto.RT_REI_ID))
                .ForMember(x => x.IdTipoReintegro, map => map.MapFrom(dto => dto.RT_TIPO_ID))
                .ForMember(x => x.IdUnidadEjecutora, map => map.MapFrom(dto => dto.UEJ_ID))
                .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.MODIFICADO_POR))
                .ForMember(x => x.NumeroRadicado, map => map.MapFrom(dto => dto.NRO_RADICADO))
                .ForMember(x => x.ValorReintegro, map => map.MapFrom(dto => dto.VALOR_SOLICITADO))
                .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.CREADO_POR))
                .ReverseMap();

            CreateMap<TipoReintegros, TipoReintegrosDto>()
                .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.CREADO_POR))
                .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.DESCRIPCION))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FECHA_CREACION))
                .ForMember(x => x.FechaModificado, map => map.MapFrom(dto => dto.FECHA_MODIFICACION))
                .ForMember(x => x.IdTipoReintegro, map => map.MapFrom(dto => dto.RT_TIPO_ID))
                .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.MODIFICADO_POR))
                .ReverseMap();

            CreateMap<ReintegrosParams,ReintegrosParamsDto>()
                .ForMember(x => x.IdTipoReintegro, map => map.MapFrom(dto => dto.RT_TIPO_ID))
                .ForMember(x => x.IdUnidadEjecutora, map => map.MapFrom(dto => dto.UEJ_ID))
                .ForMember(x => x.NumeroRadicado, map => map.MapFrom(dto => dto.NRO_RADICADO))
                .ReverseMap();


            CreateMap<ListaReintegros, ListaReintegrosDto>()
                .ForMember(x => x.Id_Reintegro, map => map.MapFrom(dto => dto.Id_Reintegro))
                .ForMember(x => x.Numero_Radicado, map => map.MapFrom(dto => dto.Numero_Radicado))
                .ForMember(x => x.Id_Unidad_Ejecutora, map => map.MapFrom(dto => dto.Id_Unidad_Ejecutora))
                .ForMember(x => x.Unidad_Ejecutora, map => map.MapFrom(dto => dto.Unidad_Ejecutora))
                .ForMember(x => x.TipoReintegro, map => map.MapFrom(dto => dto.TipoReintegro))
                .ForMember(x => x.NumeroDocumento, map => map.MapFrom(dto => dto.NumeroDocumento))
                .ForMember(x => x.NombreAfiliado, map => map.MapFrom(dto => dto.NombreAfiliado))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FechaCreacion))
                .ForMember(x => x.ValorSueldo, map => map.MapFrom(dto => dto.ValorSueldo))
                .ForMember(x => x.EstadoReintegro, map => map.MapFrom(dto => dto.EstadoReintegro))
                .ForMember(x => x.IdUsuario, map => map.MapFrom(dto => dto.UsuarioCreacion))
                .ForMember(x => x.NombreUsuario, map => map.MapFrom(dto => dto.NombreUsuario))
                .ForMember(x => x.FechaFin, map => map.MapFrom(dto => dto.FechaFin))
                .ReverseMap();

            CreateMap<ReintefroDto, Reintegro>()
                .ForMember(x => x.Id_Reintegro, map => map.MapFrom(dto => dto.Id_Reintegro))
                .ForMember(x => x.Id_Unidad_Ejecutora, map => map.MapFrom(dto => dto.Id_Unidad_Ejecutora))
                .ForMember(x => x.Unidad_Ejecutora, map => map.MapFrom(dto => dto.Unidad_Ejecutora))
                .ForMember(x => x.Id_Tipo_Reintegro, map => map.MapFrom(dto => dto.Id_Tipo_Reintegro))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FechaCreacion))
                .ForMember(x => x.UsuarioCreacion, map => map.MapFrom(dto => dto.UsuarioCreacion))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.Estado))
                .ForMember(x => x.Observacion, map => map.MapFrom(dto => dto.Observacion))
                .ReverseMap();

            CreateMap<AfiliadosReintegrosDto, AfiliadosReintegro>()
                 .ForMember(x => x.Id_Afiliado, map => map.MapFrom(dto => dto.Id_Afiliado))
                 .ForMember(x => x.IdReintegro, map => map.MapFrom(dto => dto.IdReintegro))
                 .ForMember(x => x.TipoDocumentoId, map => map.MapFrom(dto => dto.TipoDocumentoId))
                 .ForMember(x => x.NumeroDocumento, map => map.MapFrom(dto => dto.NumeroDocumento))
                 .ForMember(x => x.Nombres, map => map.MapFrom(dto => dto.Nombres))
                 .ForMember(x => x.Apellidos, map => map.MapFrom(dto => dto.Apellidos))
                 .ForMember(x => x.Id_Tipo_Reintegro, map => map.MapFrom(dto => dto.Id_Tipo_Reintegro))
                  .ForMember(x => x.EstadoAfiliao, map => map.MapFrom(dto => dto.EstadoAfiliao))
                .ForMember(x => x.ValorSueldo, map => map.MapFrom(dto => dto.ValorSolicitado))
                 .ForMember(x => x.ObservacionAfiliado, map => map.MapFrom(dto => dto.ObservationAfiliado))
                 .ForMember(x => x.TipoValor, map => map.MapFrom(dto => dto.TipoValor))
                 .ForMember(x => x.FechaFin, map => map.MapFrom(dto => dto.FechaFin))
                 .ReverseMap();

            CreateMap<AfiliadoReintegroListDto, AfiliadoReinegroList>()
                .ForMember(x => x.IdAfiliadoReintegro, map => map.MapFrom(dto => dto.IdAfiliadoReintegro))
                 .ForMember(x => x.IdReintegro, map => map.MapFrom(dto => dto.IdReintegro))
                 .ForMember(x => x.TipoDocumentoId, map => map.MapFrom(dto => dto.TipoDocumentoId))
                 .ForMember(x => x.NumeroDocumento, map => map.MapFrom(dto => dto.NumeroDocumento))
                 .ForMember(x => x.Nombres, map => map.MapFrom(dto => dto.Nombres))
                 .ForMember(x => x.Apellidos, map => map.MapFrom(dto => dto.Apellidos))
                 .ForMember(x => x.Id_Tipo_Reintegro, map => map.MapFrom(dto => dto.Id_Tipo_Reintegro))
                 .ForMember(x => x.EstadoAfiliao, map => map.MapFrom(dto => dto.EstadoAfiliao))
                 .ForMember(x => x.ValorSueldo, map => map.MapFrom(dto => dto.ValorSolicitado))
                 .ReverseMap();

            CreateMap<ReintegroResponseDTO, AfiliadoResponse>()
              .ForMember(x => x.Id_Reintegro, map => map.MapFrom(dto => dto.Id_Reintegro))
               .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.FechaCreacion))
               .ForMember(x => x.ValorSueldoPorcentaje, map => map.MapFrom(dto => dto.ValorSueldoPorcentaje))
               .ForMember(x => x.Unidad_Ejecutora, map => map.MapFrom(dto => dto.Unidad_Ejecutora))
               .ForMember(x => x.FechaFin, map => map.MapFrom(dto => dto.FechaFin))
               .ForMember(x => x.EstadoAfiliao, map => map.MapFrom(dto => dto.EstadoAfiliao))
               .ForMember(x => x.ValorSueldo, map => map.MapFrom(dto => dto.ValorSueldo))
               .ForMember(x => x.ModalidadReintegro, map => map.MapFrom(dto => dto.ModalidadReintegro))
                .ForMember(x => x.Numero_Radicado, map => map.MapFrom(dto => dto.Numero_Radicado))
                 .ForMember(x => x.NumerSolicitud, map => map.MapFrom(dto => dto.NumerSolicitud))
               .ReverseMap();


            CreateMap<DataGenerarPago, DataGenerarPagoDto>().ReverseMap();
        }
    }
}

