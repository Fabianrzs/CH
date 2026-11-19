using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima;
using GA2.Domain.Entities.CargueNonima.Document;

namespace GA2.Transversals.Mappers
{
    /// <summary>
    /// Mapper del documento a la tabla DCT
    /// <author>Nicolas Florez Sarrazola</author>
    /// <date>12/04/2021</date>
    /// </summary>
    public class DocumentMapper : Profile
    {
        public DocumentMapper()
        {
            //Model DocumentoDto a Document
            CreateMap<DocumentoDto, Documento>()
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.DocumentoId))
                .ForMember(x => x.TDC_ID, map => map.MapFrom(dto => dto.TipoDocumentoId))
                .ForMember(x => x.DCT_NOMBRE, map => map.MapFrom(dto => dto.Nombre))
                .ForMember(x => x.DCT_FECHA_INICIAL, map => map.MapFrom(dto => dto.FechaInicialDocumento))
                .ForMember(x => x.ESD_ID, map => map.MapFrom(dto => dto.EstadoDocumento))
                .ForMember(x => x.EST_ID, map => map.MapFrom(dto => dto.EstadoArchivo))
                .ForMember(x => x.CED_ID, map => map.MapFrom(dto => dto.CausalEstadoDocumento))
                .ForMember(x => x.UEJ_ID, map => map.MapFrom(dto => dto.UnidadEjecutora))
                .ForMember(x => x.DCT_FECHA_FINAL, map => map.MapFrom(dto => dto.FechaFinalDocumento))
                .ForMember(x => x.DCT_ID_ANULA, map => map.MapFrom(dto => dto.AnulaId))
                .ForMember(x => x.DCT_ALERTA, map => map.MapFrom(dto => dto.Alerta))
                .ForMember(x => x.DCT_CREADOPOR, map => map.MapFrom(dto => dto.CreadoPor))
                .ForMember(x => x.DCT_FECHACREACION, map => map.MapFrom(dto => dto.FechaCreacion))
                .ForMember(x => x.DCT_FECHAMODIFICACION, map => map.MapFrom(dto => dto.FechaModificacion))
                .ForMember(x => x.DCT_MODIFICADOPOR, map => map.MapFrom(dto => dto.ModificadoPor))
                .ForMember(x => x.DCT_REGISTROS, map => map.MapFrom(dto => dto.Registros))
                .ReverseMap();

            //Model DocumentoDto a Document
            CreateMap<DocumentoDto, ConsultarDocumentosCarga>()
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.DocumentoId))
                .ForMember(x => x.TDC_ID, map => map.MapFrom(dto => dto.TipoDocumentoId))
                .ForMember(x => x.DCT_NOMBRE, map => map.MapFrom(dto => dto.Nombre))
                .ForMember(x => x.DCT_FECHA_INICIAL, map => map.MapFrom(dto => dto.FechaInicialDocumento))
                .ForMember(x => x.ESD_ID, map => map.MapFrom(dto => dto.EstadoDocumento))
                .ForMember(x => x.CED_ID, map => map.MapFrom(dto => dto.CausalEstadoDocumento))
                .ForMember(x => x.UEJ_ID, map => map.MapFrom(dto => dto.UnidadEjecutora))
                .ForMember(x => x.DCT_FECHA_FINAL, map => map.MapFrom(dto => dto.FechaFinalDocumento))
                .ForMember(x => x.DCT_ID_ANULA, map => map.MapFrom(dto => dto.AnulaId))
                .ForMember(x => x.DCT_ALERTA, map => map.MapFrom(dto => dto.Alerta))
                .ForMember(x => x.DCT_CREADOPOR, map => map.MapFrom(dto => dto.CreadoPor))
                .ForMember(x => x.DCT_FECHACREACION, map => map.MapFrom(dto => dto.FechaCreacion))
                .ForMember(x => x.DCT_FECHAMODIFICACION, map => map.MapFrom(dto => dto.FechaModificacion))
                .ForMember(x => x.DCT_MODIFICADOPOR, map => map.MapFrom(dto => dto.ModificadoPor))
                .ForMember(x => x.ESTADO, map => map.MapFrom(dto => dto.Estado))
                .ForMember(x => x.USUARIO, map => map.MapFrom(dto => dto.Usuario))
                .ForMember(x => x.DCT_REGISTROS, map => map.MapFrom(dto => dto.Registros))
                .ReverseMap();

            //Model DocumentoDto a Document
            CreateMap<ErrorCargueArchivoDto, ErrorCargueArchivo>()
                .ForMember(x => x.NMN_ID, map => map.MapFrom(dto => dto.NmnId))
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.DctId))
                .ForMember(x => x.NUMERO_LINEA, map => map.MapFrom(dto => dto.NumeroLinea))
                .ForMember(x => x.INCONSISTENCIAS_NOMBRE, map => map.MapFrom(dto => dto.InconsistenciasNombre))
                .ForMember(x => x.INCONSISTENCIAS_ENCABEZADO, map => map.MapFrom(dto => dto.InconsistenciasEncabezado))
                .ForMember(x => x.INCONSISTENCIAS_CUERPO, map => map.MapFrom(dto => dto.InconsistenciasCuerpo))
                .ForMember(x => x.INCONSISTENCIAS_FIN, map => map.MapFrom(dto => dto.InconsistenciasFin))
                .ForMember(x => x.CODIGO_INCIDENCIA, map => map.MapFrom(dto => dto.CodigoIncidencia))
                .ForMember(x => x.DESCRIPCION, map => map.MapFrom(dto => dto.Descripcion))
                .ForMember(x => x.TIPO_ERROR, map => map.MapFrom(dto => dto.TipoError))
                .ForMember(x => x.FECHA_CREACION, map => map.MapFrom(dto => dto.FechaCreacion))                
                .ForMember(x => x.ESTADO_ERROR, map => map.MapFrom(dto => dto.EstadoError))
                .ReverseMap();

            //Model EstadoArchivoDto a NMN_ESTADOS_ARCHIVOS_NOMINA
            CreateMap<EstadoArchivoDto, NMN_ESTADOS_ARCHIVOS_NOMINA>()
                .ForMember(x => x.EST_ID, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.EST_DESCRIPCION, map => map.MapFrom(dto => dto.DESCRIPCION))
                .ForMember(x => x.EST_TIPO, map => map.MapFrom(dto => dto.TIPO))
                .ForMember(x => x.EST_ESTADO, map => map.MapFrom(dto => dto.ESTADO))
                .ForMember(x => x.EST_CONCEPTO, map => map.MapFrom(dto => dto.CONCEPTO))
                .ReverseMap();


            //Model SolicitudNominaDto a SolicitudNomina
            CreateMap<SolicitudNominaDto, SolicitudNomina>()
                .ForMember(dest => dest.DCT_ID, opt => opt.MapFrom(src => src.DocumentoId))
                .ForMember(dest => dest.SNMN_ID, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.SNMN_NUMERO_SOLICITUD, opt => opt.MapFrom(src => src.NumeroSolicitud))
                .ForMember(dest => dest.SNMN_ESTADO_SOLICITUD, opt => opt.MapFrom(src => src.EstadoSolicitud))
                .ForMember(dest => dest.SNMN_ESTADO, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.SNMN_FECHA_INICIAL, opt => opt.MapFrom(src => src.FechaInicialDocumento))
                .ForMember(dest => dest.SNMN_FECHA_FINAL, opt => opt.MapFrom(src => src.FechaFinalDocumento))
                .ForMember(dest => dest.SNMN_OBSERVACION, opt => opt.MapFrom(src => src.Observacion))
                .ForMember(dest => dest.SNMN_AUTORIZADOPOR, opt => opt.MapFrom(src => src.AutorizadoPor))
                .ForMember(dest => dest.SNMN_PROCESADOPOR, opt => opt.MapFrom(src => src.ProcesadoPor))
                .ForMember(dest => dest.SNMN_APROBADOPOR, opt => opt.MapFrom(src => src.AprobadoPor))
                .ForMember(dest => dest.SNMN_FECHAMODIFICACION, opt => opt.MapFrom(src => src.FechaModificacion)) 
                .ForMember(dest => dest.SNMN_FECHACREACION, opt => opt.MapFrom(src => src.FechaCreacion))
                .ForMember(dest => dest.SNMN_RADICADO, opt => opt.MapFrom(src => src.NumeroRadicado))
                .ForMember(dest => dest.SNMN_MONTO, opt => opt.MapFrom(src => src.Monto))
                .ForMember(dest => dest.SNMN_PERIODO, opt => opt.MapFrom(src => src.Periodo))
                .ForMember(dest => dest.SNMN_REGISTROS, opt => opt.MapFrom(src => src.Registros))
                .ForMember(dest => dest.SNMN_TIPO_ARCHIVO, opt => opt.MapFrom(src => src.TipoArchivo))
                .ForMember(dest => dest.SNMN_NOMBRE_ARCHIVO, opt => opt.MapFrom(src => src.Nombre))
                .ReverseMap();

        }
    }
}
