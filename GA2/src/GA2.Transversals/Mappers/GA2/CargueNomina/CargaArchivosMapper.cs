using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Domain.Entities.CargueNonima.Document;

namespace GA2.Transversals.Mappers
{
    /// <summary>
    /// Mapper del documento cargado para validar nombre del archivo
    /// <author>Nicolas Florez Sarrazola</author>
    /// <date>12/04/2021</date>
    /// </summary>
    public class CargaArchivosMapper : Profile
    {
        public CargaArchivosMapper()
        {
            //model TipoArchivo TypeFile
            CreateMap<NombreArchivoDto, FileNameReported>()
                .ForMember(x => x.NAN_TIPO, map => map.MapFrom(dto => dto.FILE_TIPO))
                .ForMember(x => x.NAN_CATEGORIA, map => map.MapFrom(dto => dto.FILE_CATEGORIA))
                .ForMember(x => x.NAN_PERIODOS_APORTES, map => map.MapFrom(dto => dto.FILE_PERIODOS_APORTES))
                .ForMember(x => x.UEJ_ID, map => map.MapFrom(dto => dto.FILE_UNIDAD_EJECUTORA))
                .ForMember(x => x.NAN_FECHA_ENVIO, map => map.MapFrom(dto => dto.FILE_FECHA_ENVIO))
                .ReverseMap();

            CreateMap<EncabezadoArchivoDto, HeaderFileReported>()
                .ForMember(x => x.CONSECUTIVE, map => map.MapFrom(dto => dto.CONSECUTIVE))
                .ForMember(x => x.FILE_TIPO, map => map.MapFrom(dto => dto.FILE_TIPO))
                .ForMember(x => x.NOMBRE_NOMINA_UNIDAD_EJECUTORA, map => map.MapFrom(dto => dto.NOMBRE_NOMINA_UNIDAD_EJECUTORA))
                .ForMember(x => x.ID_UNIDAD_EJECUTORA, map => map.MapFrom(dto => dto.ID_UNIDAD_EJECUTORA))
                .ForMember(x => x.FECHA_APORTES, map => map.MapFrom(dto => dto.FECHA_APORTES))
                .ReverseMap();

            CreateMap<CuerpoArchivoDto, BodyFile>()
                .ForMember(x => x.CONSECUTIVE, map => map.MapFrom(dto => dto.CONSECUTIVE))
                .ForMember(x => x.DIGITO_FUERZA, map => map.MapFrom(dto => dto.DIGITO_FUERZA))
                .ForMember(x => x.UNIDAD_EJECUTORA, map => map.MapFrom(dto => dto.UNIDAD_EJECUTORA))
                .ForMember(x => x.TIPO_IDENTIFICACION, map => map.MapFrom(dto => dto.TIPO_IDENTIFICACION))
                .ForMember(x => x.IDENTIFICACION, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.CODIGO_MILITAR, map => map.MapFrom(dto => dto.CODIGO_MILITAR))
                .ForMember(x => x.PRIMER_NOMBRE, map => map.MapFrom(dto => dto.PRIMER_NOMBRE))
                .ForMember(x => x.SEGUNDO_NOMBRE, map => map.MapFrom(dto => dto.SEGUNDO_NOMBRE))
                .ForMember(x => x.PRIMER_APELLIDO, map => map.MapFrom(dto => dto.PRIMER_APELLIDO))
                .ForMember(x => x.SEGUNDO_APELLIDO, map => map.MapFrom(dto => dto.SEGUNDO_APELLIDO))
                .ForMember(x => x.EMBARGO, map => map.MapFrom(dto => dto.EMBARGO))
                .ForMember(x => x.INGRESO_BASE_CALCULO, map => map.MapFrom(dto => dto.INGRESO_BASE_CALCULO))
                .ForMember(x => x.VALOR, map => map.MapFrom(dto => dto.VALOR))
                .ForMember(x => x.PERIODO, map => map.MapFrom(dto => dto.PERIODO))
                .ForMember(x => x.GRADO, map => map.MapFrom(dto => dto.GRADO))
                .ForMember(x => x.UNIDAD_OPERATIVA, map => map.MapFrom(dto => dto.UNIDAD_OPERATIVA))
                .ReverseMap();

            CreateMap<FinDeArchivoDto, EndFileReported>()
                .ForMember(x => x.CONSECUTIVE, map => map.MapFrom(dto => dto.CONSECUTIVE))
                .ForMember(x => x.FILE_CONTROL_INDICATOR, map => map.MapFrom(dto => dto.FILE_CONTROL_INDICATOR))
                .ForMember(x => x.SUM_CONTRIBUTIONS, map => map.MapFrom(dto => dto.SUM_CONTRIBUTIONS))
                .ForMember(x => x.END_FILE_CODE, map => map.MapFrom(dto => dto.END_FILE_CODE))
                .ReverseMap();

            //Model ArchivoDto a Archivo
            CreateMap<ArchivoDto, ConsultarArchivosCargados>()
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.IdDocumento))
                .ForMember(x => x.UEJ_ID, map => map.MapFrom(dto => dto.IdUnidadEjecutora))
                .ForMember(x => x.TAR_ID, map => map.MapFrom(dto => dto.IdTipoArchivo))
                .ForMember(x => x.NAN_TIPO, map => map.MapFrom(dto => dto.TipoArchivo))
                .ForMember(x => x.NAN_CATEGORIA, map => map.MapFrom(dto => dto.ClasificacionArchivo))
                .ForMember(x => x.FAN_SUMATORIA_APORTE, map => map.MapFrom(dto => dto.Monto))
                .ForMember(x => x.NAN_PERIODOS_APORTES, map => map.MapFrom(dto => dto.Periodo))
                .ForMember(x => x.NAN_FECHA_ENVIO, map => map.MapFrom(dto => dto.FechaCarga))
                .ForMember(x => x.DCT_NOMBRE, map => map.MapFrom(dto => dto.DocumentoNombre))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.Estado))
                .ForMember(x => x.DCT_REGISTROS, map => map.MapFrom(dto => dto.Registros))
                .ForMember(x => x.Usuario, map => map.MapFrom(dto => dto.Usuario))
                .ForMember(x => x.TotalEstadoError, map => map.MapFrom(dto => dto.TotalEstadoError))
                .ForMember(x => x.ESD_ID, map => map.MapFrom(dto => dto.EsdId))
                .ReverseMap();

            //Model ArchivoDto a Archivo
            CreateMap<ArchivoConErrorDto, GuardarArchivoCorregido>()
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.IdDocumento))
                .ForMember(x => x.TAR_ID, map => map.MapFrom(dto => dto.IdTipoArchivo))
                .ReverseMap();

            //Model ArchivoConErrorDto a ArchivosConError
            CreateMap<ArchivoConErrorDto, ArchivosConError>()
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.IdDocumento))
                .ForMember(x => x.TIPO_ERROR, map => map.MapFrom(dto => dto.TipoError))
                //ENCABEZADO
                .ForMember(x => x.EAN_CONSECUTIVO, map => map.MapFrom(dto => dto.ConsecutivoEncabezado))
                .ForMember(x => x.TAR_ID, map => map.MapFrom(dto => dto.IdTipoArchivo))
                .ForMember(x => x.TAR_DESCRIPCION, map => map.MapFrom(dto => dto.TipoArchivo))
                .ForMember(x => x.EAN_NOMBRE_UNIDAD_EJECUTORA, map => map.MapFrom(dto => dto.NombreUnidadEjecutora))
                .ForMember(x => x.UEJ_ID, map => map.MapFrom(dto => dto.IdUnidadEjecutora))
                .ForMember(x => x.EAN_PERIODO_APORTE, map => map.MapFrom(dto => dto.Periodo))
                //CUERPO
                .ForMember(x => x.CAN_CONSECUTIVO, map => map.MapFrom(dto => dto.ConsecutivoCuerpo))
                .ForMember(x => x.CAN_DIGITO_FUERZA, map => map.MapFrom(dto => dto.digitoFuerza))
                .ForMember(x => x.TID_ID, map => map.MapFrom(dto => dto.TipoDocumento))
                .ForMember(x => x.CAN_IDENTIFICACION, map => map.MapFrom(dto => dto.Identificacion))
                .ForMember(x => x.CAN_COD_MILITAR, map => map.MapFrom(dto => dto.CodigoMilitar))
                .ForMember(x => x.CAN_PRIMER_NOMBRE, map => map.MapFrom(dto => dto.PrimerNombre))
                .ForMember(x => x.CAN_SEGUNDO_NOMBRE, map => map.MapFrom(dto => dto.SegundoNombre))
                .ForMember(x => x.CAN_PRIMER_APELLIDO, map => map.MapFrom(dto => dto.PrimerApellido))
                .ForMember(x => x.CAN_SEGUNDO_APELLIDO, map => map.MapFrom(dto => dto.SegundoApellido))
                .ForMember(x => x.CAN_EMBARGO, map => map.MapFrom(dto => dto.Embargo))
                .ForMember(x => x.CAN_BASE_CALCULO, map => map.MapFrom(dto => dto.BaseCalculo))
                .ForMember(x => x.CAN_VALOR, map => map.MapFrom(dto => dto.ValorAporte))
                .ForMember(x => x.CAN_PERIODO, map => map.MapFrom(dto => dto.PeriodoCuerpo))
                .ForMember(x => x.GRD_ID, map => map.MapFrom(dto => dto.Grado))
                .ForMember(x => x.CAN_UNIDAD_OPERATIVA, map => map.MapFrom(dto => dto.UnidadOperativa))
                //FIN
                .ForMember(x => x.FAN_CONSECUTIVO, map => map.MapFrom(dto => dto.ConsecutivoFin))
                .ForMember(x => x.FAN_INDICADOR_CONTROL, map => map.MapFrom(dto => dto.IndicadorControl))
                .ForMember(x => x.FAN_SUMATORIA_APORTE, map => map.MapFrom(dto => dto.SumatoriaAporte))
                .ForMember(x => x.FAN_CODIGO_FIN_ARCHIVO, map => map.MapFrom(dto => dto.CodigoFinArchivo))
                .ReverseMap();

            CreateMap<ArchivosConError, ArchivoConErrorDto>()
                .ForMember(dest => dest.IdDocumento, opt => opt.MapFrom(src => src.DCT_ID))
                .ForMember(dest => dest.TipoError, opt => opt.MapFrom(src => src.TIPO_ERROR))
                // Encabezado
                .ForMember(dest => dest.ConsecutivoEncabezado, opt => opt.MapFrom(src => src.EAN_CONSECUTIVO))
                .ForMember(dest => dest.IdTipoArchivo, opt => opt.MapFrom(src => src.TAR_ID))
                .ForMember(dest => dest.TipoArchivo, opt => opt.MapFrom(src => src.TAR_DESCRIPCION))
                .ForMember(dest => dest.NombreUnidadEjecutora, opt => opt.MapFrom(src => src.EAN_NOMBRE_UNIDAD_EJECUTORA))
                .ForMember(dest => dest.IdUnidadEjecutora, opt => opt.MapFrom(src => src.UEJ_ID))
                .ForMember(dest => dest.Periodo, opt => opt.MapFrom(src => src.EAN_PERIODO_APORTE))
                // Cuerpo
                .ForMember(dest => dest.ConsecutivoCuerpo, opt => opt.MapFrom(src => src.CAN_CONSECUTIVO))
                .ForMember(dest => dest.digitoFuerza, opt => opt.MapFrom(src => src.CAN_DIGITO_FUERZA))
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TID_ID))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.CAN_IDENTIFICACION))
                .ForMember(dest => dest.CodigoMilitar, opt => opt.MapFrom(src => src.CAN_COD_MILITAR))
                .ForMember(dest => dest.PrimerNombre, opt => opt.MapFrom(src => src.CAN_PRIMER_NOMBRE))
                .ForMember(dest => dest.SegundoNombre, opt => opt.MapFrom(src => src.CAN_SEGUNDO_NOMBRE))
                .ForMember(dest => dest.PrimerApellido, opt => opt.MapFrom(src => src.CAN_PRIMER_APELLIDO))
                .ForMember(dest => dest.SegundoApellido, opt => opt.MapFrom(src => src.CAN_SEGUNDO_APELLIDO))
                .ForMember(dest => dest.Embargo, opt => opt.MapFrom(src => src.CAN_EMBARGO))
                .ForMember(dest => dest.BaseCalculo, opt => opt.MapFrom(src => src.CAN_BASE_CALCULO))
                .ForMember(dest => dest.ValorAporte, opt => opt.MapFrom(src => src.CAN_VALOR))
                .ForMember(dest => dest.PeriodoCuerpo, opt => opt.MapFrom(src => src.CAN_PERIODO))
                .ForMember(dest => dest.Grado, opt => opt.MapFrom(src => src.GRD_ID))
                .ForMember(dest => dest.UnidadOperativa, opt => opt.MapFrom(src => src.CAN_UNIDAD_OPERATIVA))
                // Fin
                .ForMember(dest => dest.ConsecutivoFin, opt => opt.MapFrom(src => src.FAN_CONSECUTIVO))
                .ForMember(dest => dest.IndicadorControl, opt => opt.MapFrom(src => src.FAN_INDICADOR_CONTROL))
                .ForMember(dest => dest.SumatoriaAporte, opt => opt.MapFrom(src => src.FAN_SUMATORIA_APORTE))
                .ForMember(dest => dest.CodigoFinArchivo, opt => opt.MapFrom(src => src.FAN_CODIGO_FIN_ARCHIVO))
                .ReverseMap();

            CreateMap<Ejecutor, EjecutorDto>()
                .ForMember(x => x.DCT_FECHA_INCIAL, map => map.MapFrom(dto => dto.DCT_FECHA_INCIAL))
                .ForMember(x => x.DCT_ID, map => map.MapFrom(dto => dto.DCT_ID))
                .ForMember(x => x.FILE_CATEGORIA, map => map.MapFrom(dto => dto.FILE_CATEGORIA))
                .ForMember(x => x.FILE_TIPO, map => map.MapFrom(dto => dto.FILE_TIPO))
                .ForMember(x => x.FILE_UNIDAD_EJECUTORA, map => map.MapFrom(dto => dto.FILE_UNIDAD_EJECUTORA))
                .ForMember(x => x.FILE_PERIODOS_APORTES, map => map.MapFrom(dto => dto.FILE_PERIODOS_APORTES))
                .ReverseMap();
            CreateMap<VerificarAfiliado, VerificarAfiliadoDTO>()
                .ForMember(x => x.ClienteId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.TipoCuentaId, map => map.MapFrom(dto => dto.TCT_ID))
                .ForMember(x => x.EstadodCuentaId, map => map.MapFrom(dto => dto.ECT_ID))
                .ForMember(x => x.CausalEstadId, map => map.MapFrom(dto => dto.CCN_ID))
                .ForMember(x => x.NumeroDocumetoId, map => map.MapFrom(dto => dto.DCT_ID))
                .ForMember(x => x.CuentaFechaCreacion, map => map.MapFrom(dto => dto.CTA_FECHA_CREACION))
                .ForMember(x => x.CuentaFechaCierre, map => map.MapFrom(dto => dto.CTA_FECHA_CIERRE))
                .ForMember(x => x.CuentaFechaPrimerAporte, map => map.MapFrom(dto => dto.CTA_FECHA_PRIMER_APORTE))
                .ForMember(x => x.CuentaSaldo, map => map.MapFrom(dto => dto.CTA_SALDO))
                .ForMember(x => x.CuentaSaldoCanje, map => map.MapFrom(dto => dto.CTA_SALDO_CANJE))
                .ForMember(x => x.CcuentaSaldoDisponible, map => map.MapFrom(dto => dto.CTA_SALDO_DISPONIBLE))
                .ForMember(x => x.CuentaCuotas, map => map.MapFrom(dto => dto.CTA_CUOTAS))
                .ForMember(x => x.MovimientoValor, map => map.MapFrom(dto => dto.MVT_VALOR))
                .ForMember(x => x.TipoMovimiento, map => map.MapFrom(dto => dto.CAT_TIPO_MOVIMIENTO))
                .ForMember(x => x.MovimientoFechaProceso, map => map.MapFrom(dto => dto.MVT_FECHA_PROCESO))
                .ForMember(x => x.MovimientoAnoPeriodo, map => map.MapFrom(dto => dto.MVT_ANO_PERIODO))
                .ForMember(x => x.FechaMovimiento, map => map.MapFrom(dto => dto.MVT_FECHA_MOVIMIENTO))
                .ReverseMap();
            CreateMap<AfiliadoporIdentificacion, AfiliadoporIdentificacionDTO>()
                .ForMember(x => x.NumeroIidentificacion, map => map.MapFrom(dto => dto.CLI_IDENTIFICACION))
                .ForMember(x => x.IdCliente, map => map.MapFrom(dto => dto.CLI_ID))
                .ReverseMap();

        }
    }
}
