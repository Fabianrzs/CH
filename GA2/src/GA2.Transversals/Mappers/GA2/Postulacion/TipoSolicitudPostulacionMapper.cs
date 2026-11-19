using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Postulacion;
using GA2.Application.Dto.Postulacion.Tipo;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Postulacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers.GA2
{
    public class TipoSolicitudPostulacionMapper:Profile
    {
        public TipoSolicitudPostulacionMapper()
        {
            CreateMap<TipoSolictudPostulante, TipoSolicitudPostulacionDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.TSP_ID))
                .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.TSP_NOMBRE))
                .ForMember(x => x.FechaInsercion, map => map.MapFrom(dto => dto.TSP_FECHA_INSERCION))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.TSP_ESTADO))
                //.ForMember(x => x.TipoIdentificacion.Id, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ReverseMap();
            
            CreateMap<TipoModalidadPostulacion, TipoModaliadPostulacionDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.MPO_ID))
                .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.MPO_NOMBRE))
                .ForMember(x => x.FechaInsercion, map => map.MapFrom(dto => dto.MPO_FECHA_INSERCION))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.MPO_ESTADO))
                 .ForMember(x => x.IdSolicitud, map => map.MapFrom(dto => dto.MPO_ID_SOLICITUD))
                 .ReverseMap();

            CreateMap<TipoSubModalidadPostulacion, TipoSubModalidadPostulacionDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.SPO_ID))
                .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.SPO_NOMBRE))
                .ForMember(x => x.FechaInsercion, map => map.MapFrom(dto => dto.SPO_FECHA_INSERCION))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.SPO_ESTADO))
                 .ForMember(x => x.IdModalidad, map => map.MapFrom(dto => dto.SPO_ID_MODALIDAD))
                 .ReverseMap();

            CreateMap<EstadoPostulacion, TipoEstadoSolicitudPostulacionDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.ESP_ID))
                .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.ESP_NOMBRE))
     .ReverseMap();
            CreateMap<TipoListaArchivo, TipoListaCargarArchivoPostulacionDto>()
             .ForMember(x => x.Id, map => map.MapFrom(dto => dto.DFS_ID))
             .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.DFS_MOMBRE))
             .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.DFS_DESCRIPCION))
              .ForMember(x => x.Requerido, map => map.MapFrom(dto => dto.DFS_REQUERIDO_))
               .ForMember(x => x.Orden, map => map.MapFrom(dto => dto.DFS_ORDEN))
             .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.DFS_ESTADO))
             .ForMember(x => x.Formato, map => map.MapFrom(dto => dto.DFS_FORMATO))
              .ForMember(x => x.SolicitudId, map => map.MapFrom(dto => dto.Asp_Id))
             .ForMember(x => x.ArchivoCargado, map => map.MapFrom(dto => dto.ARCHIVO_CARGADO))
              .ForMember(x => x.NumeroFolio, map => map.MapFrom(dto => dto.NUMERO_FOLIO))
              .ReverseMap();

            CreateMap<DocumentoModalidadPostulacion, DocumentoModalidadPostulacionDto>()
                .ForMember(x => x.IdModalidad, map => map.MapFrom(dto => dto.IdModalidad))
                .ForMember(x => x.IdSolicitud, map => map.MapFrom(dto => dto.IdSolicitud))
                .ReverseMap();


            CreateMap<TipoICatalogoPostulacionDto, TipoCatalogoPostulacion>()
                .ForMember(x => x.CVL_CODIGO, map => map.MapFrom(dto => dto.Codigo))
                .ForMember(x => x.CVL_DESCRIPCION, map => map.MapFrom(dto => dto.Descripcion))
                .ReverseMap();
        }
        }
}
