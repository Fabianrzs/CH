using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Reportes;
using GA2.Domain.Entities.Solicitud;
using System;

namespace GA2.Transversals.Mappers
{
    public class SolicitudMapper : Profile
    {

        public SolicitudMapper()
        {
            CreateMap<SolicitudPropietario, SolicitudPropietarioDto>()
                .ForMember(x => x.RazonSocial, map => map.MapFrom(dto => dto.PRP_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.NumeroIdentificacion, map => map.MapFrom(dto => dto.PRP_NUMERO_IDENTIFICACION))
                //.ForMember(x => x.TipoIdentificacion.Id, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ReverseMap();

            CreateMap<SolicitudMatricula, SolicitudMatriculaDto>()
                .ForMember(x => x.CiudadId, map => map.MapFrom(dto => dto.DPC_ID_FK))
                .ForMember(x => x.DepartamentoId, map => map.MapFrom(dto => dto.DPD_ID_FK))
                .ForMember(x => x.Direccion, map => map.MapFrom(dto => dto.MAI_DIRECCION))
                .ForMember(x => x.NumeroMatricula, map => map.MapFrom(dto => dto.MAI_NUMERO_MATRICULA))
                .ForMember(x => x.EsPrincipal, map => map.MapFrom(dto => dto.MAI_PRINCIPAL))
                .ForMember(x => x.OficinaRegistroId, map => map.MapFrom(dto => dto.OFC_ID))
                .ForMember(x => x.OficinaCodigo, map => map.MapFrom(dto => dto.OFC_CODIGO))
                .ForMember(x => x.ConyugeActivo, map => map.MapFrom(dto => dto.MAI_CONYUGE_ACTIVO))
                .ForMember(x => x.PrimerNombre, map => map.MapFrom(dto => dto.MAI_PRIMER_NOMBRE_CONYUGE))
                .ForMember(x => x.SegundoNombre, map => map.MapFrom(dto => dto.MAI_SEGUNDO_NOMBRE_CONYUGE))
                .ForMember(x => x.PrimerApellido, map => map.MapFrom(dto => dto.MAI_PRIMER_APELLIDO_CONYUGE))
                .ForMember(x => x.SegundoApellido, map => map.MapFrom(dto => dto.MAI_SEGUNDO_APELLIDO_CONYUGE))
                .ForMember(x => x.Documento, map => map.MapFrom(dto => dto.MAI_CEDULA_CONYUGE))
                .ReverseMap();

            CreateMap<CrearSolicitud, CrearSolicitudDto>()
                .ForMember(x => x.tipoSubModalidad, map => map.MapFrom(dto => dto.TPS_SUB_ID))
                .ForMember(x => x.cuentaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.creadoPor, map => map.MapFrom(dto => dto.SOL_CREATEDOPOR))
                .ForMember(x => x.clienteId, map => map.MapFrom(dto => dto.CLI_ID))
                .ReverseMap();

            CreateMap<ObtenerTramiteSolicitud, ObtenerTramiteSolicitudDto>()
                .ForMember(x => x.clienteId, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.fechaSolicitud, map => map.MapFrom(dto => dto.SOL_FECHA_SOLICITUD))
                .ForMember(x => x.SOL_NOMBREASIGNADO, map => map.MapFrom(dto => dto.SOL_NOMBREASIGNADO))
                .ForMember(x => x.Posee_Avac, map => map.MapFrom(dto => dto.Posee_Avac))
                .ForMember(x => x.Numero_Folio, map => map.MapFrom(dto => dto.NUMERO_FOLIO))
                .ForMember(x => x.cuentaId, map => map.MapFrom(dto => dto.CTA_ID))
                .ForMember(x => x.solicitudId, map => map.MapFrom(dto => dto.SOL_ID))
                .ForMember(x => x.estadoSolicitud, map => map.MapFrom(dto => dto.SOL_ESTADO))
                .ForMember(x => x.tipoModalidadId, map => map.MapFrom(dto => dto.TIM_ID))
                .ForMember(x => x.solicitudEstadoId, map => map.MapFrom(dto => dto.SOL_ESTADO_SOLICITUD))
                .ForMember(x => x.solicitudTareaEstadoId, map => map.MapFrom(dto => dto.SLT_ESTADO_SOLICITUD))
                .ForMember(x => x.volverPantalla, map => map.MapFrom(dto => dto.VOLVER_PANTALLA))
                .ForMember(x => x.solicitudTareaId, map => map.MapFrom(dto => dto.SLT_ID))
                .ForMember(x => x.descripcionModalidad, map => map.MapFrom(dto => dto.TIM_DESCRIPCION))
                .ForMember(x => x.descripcionSubModalidad, map => map.MapFrom(dto => dto.TPS_SUB_DESCRIPCION))
                .ForMember(x => x.descripcionProceso, map => map.MapFrom(dto => dto.PCS_DESCRIPCION))
                .ForMember(x => x.valorRetirar, map => map.MapFrom(dto => dto.SOL_VALOR_A_RETIRAR))
                .ForMember(x => x.tipoSubModalidadId, map => map.MapFrom(dto => dto.TPS_SUB_ID))
                .ForMember(x => x.TimCodigo, map => map.MapFrom(dto => dto.TIM_CODIGO))
                .ForMember(x => x.pestanas, map => map.MapFrom(dto => dto.PESTANAS))
                .ForMember(x => x.secciones, map => map.MapFrom(dto => dto.SECCIONES))
                .ForMember(x => x.EMBARGO, map => map.MapFrom(dto => dto.EMBARGO))
                .ForMember(x => x.ultimoSolicitudTareaId, map => map.MapFrom(dto => dto.ULTIMO_SLT_ID))
                .ForMember(x => x.EstadoCredito, map => map.MapFrom(dto => dto.EstadoCredito))
                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.SOL_NUMERO_SOLICITUD))
                .ForMember(x => x.IdProcesoSolicitud, map => map.MapFrom(dto => dto.PCS_ID))
                .ForMember(x => x.observacionSolicitud, map => map.MapFrom(dto => dto.SOL_OBSERVACION))
                .ForMember(x => x.Observacion, map => map.MapFrom(dto => dto.SOL_OBSERVACION))
                .ForMember(x => x.NombreTarea, map => map.MapFrom(dto => dto.TRA_NOMBRE))
                .ForMember(x => x.NumeroRadicadoSG, map => map.MapFrom(dto => dto.SOL_RADICADO_GD))
                .ForMember(x => x.Cdp, map => map.MapFrom(dto => dto.MPR_CONT))
                .ForMember(x => x.ConsecutivoFactura, map => map.MapFrom(dto => dto.FAC_CONT))
                .ForMember(x => x.NumeroFactura, map => map.MapFrom(dto => dto.FAC_NUME))
                .ForMember(x => x.SOL_GRUPO_ASIGNADO, map => map.MapFrom(dto => dto.SOL_GRUPO_ASIGNADO))
                .ForMember(x => x.Causal, map => map.MapFrom(dto => dto.SOL_CAUSAL))
                .ForMember(x => x.Acuerdo, map => map.MapFrom(dto => dto.SOL_ACUERDO))
                .ForMember(x => x.CumpleRequisito, map => map.MapFrom(dto => dto.SOL_CUMPLE_REQUISITO))
                .ForMember(x => x.AccionRealizar, map => map.MapFrom(dto => dto.SOL_ACCION_REALIZAR))
                .ReverseMap();

            CreateMap<SolicitudTerceroApoderado, SolicitudTerceroApoderadoDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ForMember(x => x.NumDoc, map => map.MapFrom(dto => dto.TER_NUMERO_DOCUMENTO))
                .ForMember(x => x.Razonsocial, map => map.MapFrom(dto => dto.TER_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.Parentesco, map => map.MapFrom(dto => dto.TER_PARENTESCO))
                .ForMember(x => x.EsAutorizado, map => map.MapFrom(dto => dto.TER_ESAUTORIZADO))
                .ForMember(x => x.PrimerNombre, map => map.MapFrom(dto => dto.TER_PRIMER_NOMBRE))
                .ForMember(x => x.SegundoNombre, map => map.MapFrom(dto => dto.TER_SEGUNDO_NOMBRE))
                .ForMember(x => x.PrimerApellido, map => map.MapFrom(dto => dto.TER_PRIMER_APELLIDO))
                .ForMember(x => x.SegundoApellido, map => map.MapFrom(dto => dto.TER_SEGUNDO_APELLIDO))
                .ReverseMap();


            CreateMap<SolicitudBeneficiario, SolicitudBeneficiarioDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ForMember(x => x.NumDoc, map => map.MapFrom(dto => dto.TER_NUMERO_DOCUMENTO))
                .ForMember(x => x.Parentesco, map => map.MapFrom(dto => dto.TER_PARENTESCO))
                .ForMember(x => x.PrimerNombre, map => map.MapFrom(dto => dto.TER_PRIMER_NOMBRE))
                .ForMember(x => x.SegundoNombre, map => map.MapFrom(dto => dto.TER_SEGUNDO_NOMBRE))
                .ForMember(x => x.PrimerApellido, map => map.MapFrom(dto => dto.TER_PRIMER_APELLIDO))
                .ForMember(x => x.SegundoApellido, map => map.MapFrom(dto => dto.TER_SEGUNDO_APELLIDO))
                .ForMember(x => x.ValorRetirarDos, map => map.MapFrom(dto => dto.TER_VALOR_RETIRAR))
                .ReverseMap()
                .ForMember(x => x.CVL_ID, opt => opt.Ignore())
                .ForMember(x => x.CVL_DESCRIPCION, opt => opt.Ignore())
                .ForMember(x => x.TID_DESCRIPCION, opt => opt.Ignore());

            CreateMap<SolicitudAdjudicacion, SolicitudAdjudicacionDto>()
                .ForMember(x => x.SIA_ID, map => map.MapFrom(dto => dto.SIA_ID))
                .ForMember(x => x.SOL_ID_FK, map => map.MapFrom(dto => dto.SOL_ID_FK))
                .ForMember(x => x.SIA_TIPO_VIVIENDA, map => map.MapFrom(dto => dto.SIA_TIPO_VIVIENDA))
                .ForMember(x => x.SIA_DIRECCION, map => map.MapFrom(dto => dto.SIA_DIRECCION))
                .ForMember(x => x.SIA_MATRICULA, map => map.MapFrom(dto => dto.SIA_MATRICULA))
                .ForMember(x => x.SIA_VALOR_SUBSIDIADO, map => map.MapFrom(dto => dto.SIA_VALOR_SUBSIDIADO))
                .ForMember(x => x.SIA_CIUDAD, map => map.MapFrom(dto => dto.SIA_CIUDAD))
                .ReverseMap();

            CreateMap<SolicitudEscrituracion, SolicitudEscrituracionDto>()
                .ForMember(x => x.SIE_ID, map => map.MapFrom(dto => dto.SIE_ID))
                .ForMember(x => x.SOL_ID_FK, map => map.MapFrom(dto => dto.SOL_ID_FK))
                .ForMember(x => x.SIE_NOTARIA, map => map.MapFrom(dto => dto.SIE_NOTARIA))
                .ForMember(x => x.SIE_NUMERO_ESCRITURACION, map => map.MapFrom(dto => dto.SIE_NUMERO_ESCRITURACION))
                .ForMember(x => x.SIE_FECHA_ESCRITURACION, map => map.MapFrom(dto => dto.SIE_FECHA_ESCRITURACION))
                .ReverseMap();

            CreateMap<InformacionAdjudicacion, InformacionAdjudicacionDto>()
                .ForMember(x => x.TipoVivienda, map => map.MapFrom(dto => dto.TipoVivienda))
                .ForMember(x => x.Direccion, map => map.MapFrom(dto => dto.Direccion))
                .ForMember(x => x.MatriculaInmobiliaria, map => map.MapFrom(dto => dto.MatriculaInmobiliaria))
                .ForMember(x => x.ValorSubsidio, map => map.MapFrom(dto => dto.ValorSubsidio))
                .ForMember(x => x.SIA_CREATEDOPOR, map => map.MapFrom(dto => dto.SIA_CREATEDOPOR))
                .ForMember(x => x.SOL_ID_FK, map => map.MapFrom(dto => dto.SOL_ID_FK))
                .ForMember(x => x.SIA_CIUDAD, map => map.MapFrom(dto => dto.SIA_CIUDAD))
                .ReverseMap();

            CreateMap<InformacionEscrituracion, InformacionEscrituracionDto>()
                .ForMember(x => x.NumeroEscritura, map => map.MapFrom(dto => dto.NumeroEscritura))
                .ForMember(x => x.FechaEscrituracion, map => map.MapFrom(dto => dto.FechaEscrituracion))
                .ForMember(x => x.NumeroEscritura, map => map.MapFrom(dto => dto.NumeroEscritura))
                .ReverseMap();

            CreateMap<SolicitudTerceroBeneficiario, SolicitudTerceroBeneficiarioDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ForMember(x => x.NumDoc, map => map.MapFrom(dto => dto.TER_NUMERO_DOCUMENTO))
                .ForMember(x => x.Razonsocial, map => map.MapFrom(dto => dto.TER_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.Entidad, map => map.MapFrom(dto => dto.ENT_ID_FK))
                .ForMember(x => x.TipoCuenta, map => map.MapFrom(dto => dto.TER_TIPO_CUENTA))
                .ForMember(x => x.NumeroCuenta, map => map.MapFrom(dto => dto.TER_NUMERO_CUENTA))
                .ForMember(x => x.ValorRetirarDos, map => map.MapFrom(dto => dto.TER_VALOR_RETIRAR))
                .ReverseMap();

            CreateMap<SolicitudTerceroConstructor, SolicitudTerceroConstructorDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ForMember(x => x.NumDoc, map => map.MapFrom(dto => dto.TER_NUMERO_DOCUMENTO))
                .ForMember(x => x.Razonsocial, map => map.MapFrom(dto => dto.TER_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.NumeroLicencia, map => map.MapFrom(dto => dto.TER_NUMERO_LICENCIA_CONSTRUCCION))
                .ReverseMap();

            CreateMap<SolicitudTerceroVendedor, SolicitudTerceroVendedorDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ForMember(x => x.NumDoc, map => map.MapFrom(dto => dto.TER_NUMERO_DOCUMENTO))
                .ForMember(x => x.Razonsocial, map => map.MapFrom(dto => dto.TER_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.Direccion, map => map.MapFrom(dto => dto.TER_DIRECCION))
                .ForMember(x => x.Ciudad, map => map.MapFrom(dto => dto.DPC_ID_FK))
                .ForMember(x => x.Correo, map => map.MapFrom(dto => dto.TER_CORREO_ELECTRONICO))
                .ForMember(x => x.Telefono, map => map.MapFrom(dto => dto.TER_TELEFONO))
                .ReverseMap();

            CreateMap<SolicitudTerceroAutorizado, SolicitudTerceroAutorizadoDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.TID_ID_FK))
                .ForMember(x => x.NumDoc, map => map.MapFrom(dto => dto.TER_NUMERO_DOCUMENTO))
                .ForMember(x => x.Razonsocial, map => map.MapFrom(dto => dto.TER_NOMBRE_RAZON_SOCIAL))
                .ReverseMap();

            CreateMap<RespuestaDocumentosPorSubModalidad, DocumentosPorSubModalidadDto>()
                .ForMember(x => x.descripcionArchivo, map => map.MapFrom(dto => dto.PAM_DESCRIPCION_ARCHIVO))
                .ForMember(x => x.esRequerido, map => map.MapFrom(dto => dto.PAM_REQUERIDO))
                .ForMember(x => x.idArchivo, map => map.MapFrom(dto => dto.PAM_ID))
                .ForMember(x => x.nombreArchivo, map => map.MapFrom(dto => dto.PAM_NOMBRE_ARCHIVO))
                .ForMember(x => x.ordenArchivo, map => map.MapFrom(dto => dto.PAM_ORDEN))
                .ForMember(x => x.esMultipleArchivo, map => map.MapFrom(dto => dto.PAM_MULTIPLE_ARCHIVO))
                .ForMember(x => x.formatoArchivo, map => map.MapFrom(dto => dto.PAM_FORMATO))
                .ForMember(x => x.esArchivoCargado, map => map.MapFrom(dto => dto.ARCHIVO_CARGADO))
                .ForMember(x => x.numeroFolio, map => map.MapFrom(dto => dto.NUMERO_FOLIO))
                .ReverseMap();


            CreateMap<InsertarArchivo, InsertarArchivoDto>()
                .ForMember(x => x.nombreArchivo, map => map.MapFrom(dto => dto.AST_NOMBRE_ARCHIVO))
                .ForMember(x => x.ruta, map => map.MapFrom(dto => dto.AST_RUTA_STORAGE))
                .ForMember(x => x.extension, map => map.MapFrom(dto => dto.AST_EXTENSION))
                .ForMember(x => x.fechaCargue, map => map.MapFrom(dto => dto.AST_FECHA_CARGUE))
                .ReverseMap();

            CreateMap<ConsultarArchivoPorSolicitud, ConsultarArchivoPorSolicitudDto>()
                .ForMember(x => x.idArchivo, map => map.MapFrom(dto => dto.AST_ID))
                .ForMember(x => x.nombreArchivo, map => map.MapFrom(dto => dto.AST_NOMBRE_ARCHIVO))
                .ForMember(x => x.rutaStorage, map => map.MapFrom(dto => dto.AST_RUTA_STORAGE))
                .ForMember(x => x.extension, map => map.MapFrom(dto => dto.AST_EXTENSION))
                .ForMember(x => x.tipoArchivo, map => map.MapFrom(dto => dto.PAM_NOMBRE_ARCHIVO))
                .ForMember(x => x.orden, map => map.MapFrom(dto => dto.PAM_ORDEN))
                .ForMember(x => x.TRA_ORDEN, map => map.MapFrom(dto => dto.TRA_ORDEN))
                .ReverseMap();

            

            CreateMap<PeticionConsultarSolicitudCesantias, PeticionConsultarSolicitudCesantiasDto>()
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.Identificacion))
                .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TipoSolicitud))
                .ForMember(x => x.NumRadicado, map => map.MapFrom(dto => dto.NumRadicado))
                .ForMember(x => x.FechaIni, map => map.MapFrom(dto => dto.FechaIni))
                .ForMember(x => x.FechaFin, map => map.MapFrom(dto => dto.FechaFin))
                .ForMember(x => x.Modalidad, map => map.MapFrom(dto => dto.Modalidad))
                .ForMember(x => x.submodalidad, map => map.MapFrom(dto => dto.SubModalidad))
                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.NumeroSolictud))
                .ForMember(x => x.UsuarioIngreso, map => map.MapFrom(dto => dto.UsuarioIngreso))
                .ForMember(x => x.solicitante, map => map.MapFrom(dto => dto.Solicitante))
                .ReverseMap();

            CreateMap<RespuestaConsultarSolicitudCesantias, RespuestaConsultarSolicitudCesantiasDto>()
                .ForMember(x => x.NumeroSolicitud, map => map.MapFrom(dto => dto.SOL_NUMERO_SOLICITUD))
                .ForMember(x => x.ProcesoDescripcion, map => map.MapFrom(dto => dto.PCS_DESCRIPCION))
                .ForMember(x => x.FechaSolicitud, map => map.MapFrom(dto => dto.SOL_FECHA_SOLICITUD))
                .ForMember(x => x.ValorRetirar, map => map.MapFrom(dto => dto.SOL_VALOR_A_RETIRAR))
                .ForMember(x => x.TareaNombre, map => map.MapFrom(dto => dto.TRA_NOMBRE))
                .ForMember(x => x.EstadoSolicitud, map => map.MapFrom(dto => dto.CVL_DESCRIPCION))
                .ForMember(x => x.IdCliente, map => map.MapFrom(dto => dto.CLI_ID))
                .ForMember(x => x.CedulaCliente, map => map.MapFrom(dto => dto.SOL_CEDULA))
                .ForMember(x => x.TimCodigo, map => map.MapFrom(dto => dto.TIM_CODIGO))
                .ForMember(x => x.TpsSubId, map => map.MapFrom(dto => dto.TPS_SUB_ID))
                .ForMember(x => x.TpsSubDescripcion, map => map.MapFrom(dto => dto.TPS_SUB_DESCRIPCION))
                .ForMember(x => x.RadicadoGestionDocumental, map => map.MapFrom(dto => dto.SOL_RADICADO_GD))
                .ForMember(x => x.EstadoSolicitudId, map => map.MapFrom(dto => dto.SLT_ESTADO_SOLICITUD))
                .ForMember(x => x.SolicitudTareaId, map => map.MapFrom(dto => dto.SLT_ID))
                .ForMember(x => x.NumeroRadicadoSG, map => map.MapFrom(dto => dto.SOL_RADICADO_GD))
                .ForMember(x => x.IdUsuarioAsignado, map => map.MapFrom(dto => dto.IdUsuarioAsignado))
                .ForMember(x => x.UsuarioAsignado, map => map.MapFrom(dto => dto.UsuarioAsignado))
                .ForMember(x => x.NUMFOLIOS, map => map.MapFrom(dto => dto.NUMFOLIOS))
                .ForMember(x => x.ModalidadDescripcion, map => map.MapFrom(dto => dto.TIM_DESCRIPCION))
                .ReverseMap();

            CreateMap<ObtenerFormularioSolicitud, ObtenerFormularioSolicitudDto>()
                .ForMember(x => x.id, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.titulo, map => map.MapFrom(dto => dto.TITULO))
                .ForMember(x => x.subtitulo, map => map.MapFrom(dto => dto.SUBTITULO))
                .ForMember(x => x.icono, map => map.MapFrom(dto => dto.ICONO))
                .ForMember(x => x.botonera, map => map.MapFrom(dto => dto.BOTONERA))
                .ForMember(x => x.formulario, map => map.MapFrom(dto => dto.FORMULARIO))
                .ReverseMap();

            CreateMap<ComunicacionSolicitud, ComunicacionSolicitudDto>()
                .ForMember(x => x.IdSolicitud, map => map.MapFrom(dto => dto.SOL_ID))
                .ForMember(x => x.IdComunicacion, map => map.MapFrom(dto => dto.CMN_ID))
                .ForMember(x => x.IdCausalComunicacion, map => map.MapFrom(dto => dto.CSC_ID))
                .ForMember(x => x.TextoIncluir, map => map.MapFrom(dto => dto.CMN_TEXTO_INCLUIR))
                .ForMember(x => x.FirmaDigital, map => map.MapFrom(dto => dto.CMN_FIRMA_DIGITAL))
                .ForMember(x => x.Radicado, map => map.MapFrom(dto => dto.CMN_RADICADO))
                .ForMember(x => x.RadicadoRelacionado, map => map.MapFrom(dto => dto.CMN_RADICADO_RELACIONADO))
                .ReverseMap();

            CreateMap<ReporteGeneralCartera, ReporteGeneralCarteraDto>()
                .ForMember(x => x.NombrePagaduria, map => map.MapFrom(dto => dto.NOMBRE_PAGADURIA))
                .ForMember(x => x.NumeroRadicacion, map => map.MapFrom(dto => dto.NUMERO_RADICACION))
                .ForMember(x=>x.UltimoPago,map=>map.MapFrom(dto=>dto.ULTIMO_PAGO))
                .ForMember(x=>x.PróximoPago,map=>map.MapFrom(dto=>dto.PRÓXIMO_PAGO))
                .ForMember(x=>x.MoraInterior,map=>map.MapFrom(dto=>dto.MORA_INTERIOR))
                .ForMember(x => x.TasaLinea, map => map.MapFrom(dto => dto.TASA_LINEA))
                .ForMember(x => x.TotalPagar, map => map.MapFrom(dto => dto.TOTAL_PAGAR))
                .ForMember(x => x.ValorInmueble, map => map.MapFrom(dto => dto.VALOR_INMUEBLE))
                .ForMember(x => x.PlazoObligacion, map => map.MapFrom(dto => dto.PLAZO_OBLIGACION))
                .ForMember(x=>x.NumeroCuotasFaltantes,map=>map.MapFrom(dto => dto.NUMERO_CUOTAS_FALTANTES))
                .ForMember(x=>x.ValorCuotaExtraordinaria,map=>map.MapFrom(dto=>dto.VALOR_CUOTA_EXTRAORDINARIA))
                .ForMember(x=>x.ValorOpcionCompra,map=>map.MapFrom(dto=>dto.VALOR_OPCION_COMPRA))
                .ForMember(x=>x.CiudadInmueble,map=>map.MapFrom(dto=>dto.CIUDAD_INMUEBLE))
                .ForMember(x=>x.PuntajeScoring,map=>map.MapFrom(dto=>dto.PUNTAJE_SCORING))
                .ReverseMap();

            CreateMap<Settings, SettingsDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.RPT_ID))
                .ForMember(x => x.IdReporte, map => map.MapFrom(dto => dto.RPT_ID_REPORTE))
                .ForMember(x => x.NombreReporte, map => map.MapFrom(dto => dto.RPT_NOMBRE_REPORTE))
                .ForMember(x => x.NombreCortoReporte, map => map.MapFrom(dto => dto.RPT_NOMBRE_CORTO_REPORTE))
                .ReverseMap();

            CreateMap<ReportePreliminarCartera, ReportePreliminarCarteraDto>()
                .ForMember(x => x.Nro, map => map.MapFrom(dto => dto.NRO))
                .ForMember(x => x.NumeroRadicacion, map => map.MapFrom(dto => dto.NUMERO_RADICACION))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.IDENTIFICACION))
                .ForMember(x => x.Nombres, map => map.MapFrom(dto => dto.NOMBRES))
                .ForMember(x => x.Cliente, map => map.MapFrom(dto => dto.CLIENTE))
                .ForMember(x => x.NombrePagaduria, map => map.MapFrom(dto => dto.NOMBRE_PAGADURIA))
                .ForMember(x => x.Cuota, map => map.MapFrom(dto => dto.CUOTA))
                .ForMember(x => x.UltimoPago, map => map.MapFrom(dto => dto.ULTIMO_PAGO))
                .ForMember(x => x.TasaLinea, map => map.MapFrom(dto => dto.TASA_LINEA))
                .ForMember(x => x.Monto, map => map.MapFrom(dto => dto.MONTO))
                .ForMember(x => x.Saldo, map => map.MapFrom(dto => dto.SALDO))
                .ForMember(x => x.ValorInmueble, map => map.MapFrom(dto => dto.VALOR_INMUEBLE))
                .ForMember(x => x.Apertura, map => map.MapFrom(dto => dto.APERTURA))
                .ForMember(x => x.Terminacion, map => map.MapFrom(dto => dto.TERMINACION))
                .ReverseMap();

            CreateMap<ObservacionSolicitud, ObservacionSolicitudDto>()
                .ForMember(x => x.IdSolicitudTarea, map => map.MapFrom(dto => dto.SLT_ID))
                .ForMember(x => x.Observaciones, map => map.MapFrom(dto => dto.SOL_OBSERVACIONES))
                .ReverseMap();

            CreateMap<TrazabilidadSolicitudDto, TrazabilidadSolicitud>()
                .ForMember(x => x.SLT_ID, map => map.MapFrom(dto => dto.IdTareaSolicitud))
                .ForMember(x => x.TRA_ID, map => map.MapFrom(dto => dto.IdTarea))
                .ForMember(x => x.TRA_NOMBRE, map => map.MapFrom(dto => dto.DescripcionTarea))
                .ForMember(x => x.SLT_CREATEDOPOR, map => map.MapFrom(dto => dto.IdUsuario))
                .ForMember(x => x.SLT_NOMBRE_USUARIO, map => map.MapFrom(dto => dto.NombreUsuario))
                .ForMember(x => x.SLT_ESTADO_SOLICITUD, map => map.MapFrom(dto => dto.IdEstadoSolicitud))
                .ForMember(x => x.CVL_DESCRIPCION, map => map.MapFrom(dto => dto.DescripcionEstadoSolicitud))
                .ForMember(x => x.SLT_OBSERVACION, map => map.MapFrom(dto => dto.Observacion))
                .ForMember(x => x.SLT_FECHACREACION, map => map.MapFrom(dto => dto.FechaCreacion))
                .ForMember(x => x.SLT_FECHAFIN, map => map.MapFrom(dto => dto.FechaFin))

                .ReverseMap();

            CreateMap<TareasSolicitudDto, TareaSolicitud>()
                .ForMember(x => x.AST_MENSAJE, map => map.MapFrom(dto => dto.Descripcion))
                .ForMember(x => x.AST_LINK, map => map.MapFrom(dto => dto.Link))
                .ForMember(x => x.AST_ESTADO, map => map.MapFrom(dto => dto.Estado))
                .ForMember(x => x.USR_ID, map => map.MapFrom(dto => dto.IdUsuario))
                .ForMember(x => x.FLJ_ID, map => map.MapFrom(dto => dto.IdFlujo))
                .ForMember(x => x.Radicado , map => map.MapFrom(dto => dto.Radicado))
                .ForMember(x => x.Modulo, map => map.MapFrom(dto => dto.Modulo))
                .ForMember(x => x.Estado_Tarea, map => map.MapFrom(dto => dto.Estado_Tarea))
                .ForMember(x => x.USR_ENVIA, map => map.MapFrom(dto => dto.USR_ENVIA))
                .ForMember(x => x.Estado_Tarea, map => map.MapFrom(dto => dto.Estado_Tarea))
                .ForMember(x => x.AST_FECHA_REGISTRO, map => map.MapFrom(dto => dto.AST_FECHA_REGISTRO))
                .ForMember(x => x.IdentificacionAfiliado, map => map.MapFrom(dto => dto.IdentificacionAfiliado))
                .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TipoSolicitud))
                .ForMember(x => x.TipoModalidad, map => map.MapFrom(dto => dto.TipoModalidad))
                .ForMember(x => x.TipoSubModalidad, map => map.MapFrom(dto => dto.TipoSubModalidad))
                .ForMember(x => x.IdTipoSolicitud, map => map.MapFrom(dto => dto.IdTipoSolicitud))
                .ForMember(x => x.IdTipoModalidad, map => map.MapFrom(dto => dto.IdTipoModalidad))
                .ForMember(x => x.IdTipoSubModalidad, map => map.MapFrom(dto => dto.IdTipoSubModalidad))
                .ForMember(x => x.NOMBRE_TAREA, map => map.MapFrom(dto => dto.TRA_NOMBRE))
                .ReverseMap();


            CreateMap<InconsistenciasDto, Inconsistencias>()
                .ForMember(x => x.ID, map => map.MapFrom(dto => dto.ID))
                .ForMember(x => x.SLT_TAREA, map => map.MapFrom(dto => dto.SLT_TAREA))
                .ForMember(x => x.IdSolicitud, map => map.MapFrom(dto => dto.IdSolicitud))
                .ForMember(x => x.NOMBRE_REGLA, map => map.MapFrom(dto => dto.NOMBRE_REGLA))
                .ForMember(x => x.CREADORPOR, map => map.MapFrom(dto => dto.CREADORPOR))
                .ForMember(x => x.FECHACREACION, map => map.MapFrom(dto => dto.FECHACREACION))
                .ForMember(x => x.TRA_NOMBRE, map => map.MapFrom(dto => dto.TRA_NOMBRE))
                .ForMember(x => x.PUBLICA, map => map.MapFrom(dto => dto.PUBLICA))
                .ReverseMap();


            CreateMap<EstadosClienteDto, EstadosCliente>()
                .ForMember(x => x.ECL_ID, map => map.MapFrom(dto => dto.FirtsId))
                .ForMember(x => x.CEC_ID, map => map.MapFrom(dto => dto.SecondsId))
                .ForMember(x => x.CEL_OBSERVACIONES, map => map.MapFrom(dto => dto.Descriptions))
                .ReverseMap();

            CreateMap<EstadoCausalClienteDto, EstadoCausalCliente>()
                .ForMember(x => x.ECL_ID, map => map.MapFrom(dto => dto.EstadoId))
                .ForMember(x => x.CEC_ID, map => map.MapFrom(dto => dto.EstadoCausalClienteId))
                .ForMember(x => x.CEC_CAUSAL, map => map.MapFrom(dto => dto.Causal))
                .ForMember(x => x.CEC_ACTIVO, map => map.MapFrom(dto => dto.Activo))
                .ReverseMap();


            CreateMap<RespuestaConyugue, ConyugueDto>()
                .ForMember(x => x.TipoDoc, map => map.MapFrom(dto => dto.CnyTipo))
                .ForMember(x => x.ClientId, map => map.MapFrom(dto => dto.CnyId))
                .ForMember(x => x.Identificacion, map => map.MapFrom(dto => dto.CnyIdentificacion))
                .ForMember(x => x.PrimerNombre, map => map.MapFrom(dto => dto.CnyPrimerNombre))
                .ForMember(x => x.SegundoNombre, map => map.MapFrom(dto => dto.CnySegundoNombre))
                .ForMember(x => x.PrimerApellido, map => map.MapFrom(dto => dto.CnyPrimerApellido))
                .ForMember(x => x.SegundoApellido, map => map.MapFrom(dto => dto.CnySegundoApellido))
                .ReverseMap();

            CreateMap<InformacionTurnoDto, InformacionTurno>()
                .ForMember(x => x.RIT_ID_FK, map => map.MapFrom(dto => dto.TipoRecepcion))
                .ForMember(x => x.SIT_ID_FK, map => map.MapFrom(dto => dto.TipoSucursal))
                .ForMember(x => x.TIT_ID_FK, map => map.MapFrom(dto => dto.TipoTurno))
                .ForMember(x => x.SIT_PRE_TRAMITE, map => map.MapFrom(dto => dto.RadPreTramite))
                .ForMember(x => x.SIT_TURNO_NUMERO, map => map.MapFrom(dto => dto.Turno))
                .ForMember(x => x.SIT_NUMERO_COMISION, map => map.MapFrom(dto => dto.NumeroComision))
                .ForMember(x => x.SIT_FECHA_COMISION, map => map.MapFrom(dto => dto.FechaComision))
                .ReverseMap();


           

            CreateMap<InconsistenciaSeguridadSolicitud, InconsistenciaSolicitudDto>()
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.INC_ESTADO))
                .ForMember(x => x.Novedad, map => map.MapFrom(dto => dto.INC_INCONSISTENCIA))                
                .ForMember(x => x.GrupoInconsistencia, map => map.MapFrom(dto => dto.INC_GRUPO))
                .ForMember(x => x.SolicitudId, map => map.MapFrom(dto => dto.SOL_ID_FK))
                .ForMember(x => x.UserId, map => map.MapFrom(dto => dto.USR_ID_FK))
                .ForMember(x => x.Subsanado, map => map.MapFrom(dto => dto.INC_SUBSANADO))
                .ForMember(x => x.ListaInconsistenciaId, map => map.MapFrom(dto => dto.TPO_NOV_ID))
                .ForMember(x => x.CausalInterrupcionId, map => map.MapFrom(dto => dto.CAU_ID))
                .ForMember(x => x.Informe, map => map.MapFrom(dto => dto.INF_ID))
                .ForMember(x => x.publica, map => map.MapFrom(dto => dto.INC_PUBLICA))
                .ForMember(x => x.Fecha_Subsanacion, map => map.MapFrom(dto => dto.FECHA_SUBSANACION))
                .ForMember(x => x.FechaRegistro, map => map.MapFrom(dto => dto.FECHA_REGISTRO))
                .ForMember(x => x.solicitudTareaId, map => map.MapFrom(dto => dto.SLT_ID))
                .ForMember(x => x.InconsistenciaId, map => map.MapFrom(dto => dto.INC_ID))
                .ReverseMap();

            CreateMap<InconsistenciaSeguridadDto,InconsistenciaSeguridadSolicitud>()
                .ForMember(x => x.INC_INCONSISTENCIA, map => map.MapFrom(dto => dto.Novedad))
                .ForMember(x => x.INC_SUBSANADO, map => map.MapFrom(dto => dto.Subsanado))
                .ForMember(x => x.TPO_NOV_ID, map => map.MapFrom(dto => dto.ListaInconsistenciaId))
                .ForMember(x => x.CAU_ID, map => map.MapFrom(dto => dto.CausalInterrupcionId))
                .ForMember(x => x.INF_ID, map => map.MapFrom(dto => dto.Informe))
                .ForMember(x => x.TPO_NOV_ID_NUEVO, map => map.MapFrom(dto => dto.InconsistenciaId_nuevo))
                .ForMember(x => x.CAU_ID_NUEVO, map => map.MapFrom(dto => dto.CausalInterrupcionId_nuevo))
                .ForMember(x => x.INF_ID_NUEVO, map => map.MapFrom(dto => dto.Informe_nuevo))
                .ForMember(x => x.SOL_ID_FK, map => map.MapFrom(dto => dto.Solicitudid))
                .ForMember(x => x.INC_GRUPO, map => map.MapFrom(dto => dto.GrupoInconsistencia))
                .ForMember(x => x.FECHA_SUBSANACION, map => map.MapFrom(dto => dto.Fecha_subsanacion))
                .ForMember(x => x.SLT_ID, map => map.MapFrom(dto => dto.solicitudTareaId))
                .ForMember(x => x.INC_ID, map => map.MapFrom(dto => dto.InconsistenciaId))
                 .ForMember(x => x.FECHA_REGISTRO, map => map.MapFrom(dto => dto.FechaRegistro))
                .ReverseMap();


            CreateMap<ConsultarRadicadoSolicitudDto, RespuestaConsultarRadicadoSolicitud>()
                .ForMember(x => x.SOL_ID, map => map.MapFrom(dto => dto.SolicitudId))
                .ForMember(x => x.INC_DESCRIPCION, map => map.MapFrom(dto => dto.Descripcion))                
                .ReverseMap();

            CreateMap<RespuestaConsultarRadicadoSolicitud, ConsultarRadicadoSolicitudDto>()
                .ForMember(x => x.SolicitudId, map => map.MapFrom(dto => dto.SOL_ID))
                .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.INC_DESCRIPCION))
                .ReverseMap();

            CreateMap<IntervinientesActorSolicitudDto, IntervinientesActorSolicitud>()
                .ForMember(x => x.ITR_TIPO_PERSONA, map => map.MapFrom(dto => dto.PersonaJuridicaNatural))
                .ForMember(x => x.ITR_TIPO_DOCUMENTO, map => map.MapFrom(dto => dto.TipoDocumento))
                .ForMember(x => x.ITR_NUMERO_DOCUMENTO, map => map.MapFrom(dto => dto.NumeroDocumento))
                .ForMember(x => x.ITR_NOMBRE, map => map.MapFrom(dto => dto.Nombre))
                .ForMember(x => x.ITR_SEGUNDONOMBRE, map => map.MapFrom(dto => dto.SegundoNombre    ))
                .ForMember(x => x.ITR_PRIMERAPELLIDO, map => map.MapFrom(dto => dto.PrimerApellido))
                .ForMember(x => x.ITR_SEGUNDOAPELLIDO, map => map.MapFrom(dto => dto.SegundoApellido))
                .ForMember(x => x.ITR_CELULAR, map => map.MapFrom(dto => dto.Celular))
                .ForMember(x => x.ITR_CORREO_ELECTRONICO, map => map.MapFrom(dto => dto.CorreoElectronico))
                .ForMember(x => x.ITR_DEPARTAMENTO, map => map.MapFrom(dto => dto.Departamento))
                .ForMember(x => x.ITR_CIUDAD, map => map.MapFrom(dto => dto.Ciudad))
                .ForMember(x => x.ITR_DIRECCION, map => map.MapFrom(dto => dto.Direccion))
                .ForMember(x => x.ITR_BARRIO, map => map.MapFrom(dto => dto.Barrio))
                .ForMember(x => x.ITR_TELEFONO, map => map.MapFrom(dto => dto.Telefono))
                .ForMember(x => x.ITA_ID_FK, map => map.MapFrom(dto => dto.TipoActor))
                .ForMember(x => x.ITR_TIPO_CUENTA, map => map.MapFrom(dto => dto.TipoCuenta))
                .ForMember(x => x.ITR_NUMERO_CUENTA_BANCARIA, map => map.MapFrom(dto => dto.NumeroCuentaBancaria))
                .ForMember(x => x.ITR_ENTIDAD_BANCARIA, map => map.MapFrom(dto => dto.EntidadBancaria))
                .ForMember(x => x.ITR_FECHA_EXPEDICION, map => map.MapFrom(dto => dto.FechaExpedicionCedula))
                .ForMember(x => x.ITR_CONSTRUCTORA, map => map.MapFrom(dto => dto.Constructora))
                .ForMember(x => x.ITR_CONVENIO_ENTIDAD, map => map.MapFrom(dto => dto.ConvenioEntidad))
                .ForMember(x => x.SOL_ID_FK, map => map.MapFrom(dto => dto.SolId))
                .ReverseMap();


            CreateMap<BuscarIntervinientesDto, IntervinientesActorSolicitud>()
                .ForMember(x => x.ITR_TIPO_DOCUMENTO, map => map.MapFrom(dto => dto.TipoDocumento))
                .ForMember(x => x.SOL_RADICADO_GD, map => map.MapFrom(dto => dto.Radicado))
                .ForMember(x => x.SOL_NUMERO_SOLICITUD, map => map.MapFrom(dto => dto.NumSolicitud))
                .ForMember(x => x.ITR_NUMERO_DOCUMENTO, map => map.MapFrom(dto => dto.NumeroDocumento))
                .ForMember(x => x.ITR_NOMBRE, map => map.MapFrom(dto => dto.Nombre))
                .ForMember(x => x.ITA_ID_FK, map => map.MapFrom(dto => dto.ActorId))
                .ReverseMap();

            CreateMap<ActualizarIntervinientesActorSolicitudDto, IntervinientesActorSolicitud>()
                .ForMember(x => x.ITR_ID, map => map.MapFrom(dto => dto.IntervinientesId))
                .ForMember(x => x.ITR_CELULAR, map => map.MapFrom(dto => dto.Celular))
                .ForMember(x => x.ITR_CORREO_ELECTRONICO, map => map.MapFrom(dto => dto.CorreoElectronico))
                .ForMember(x => x.ITR_DEPARTAMENTO, map => map.MapFrom(dto => dto.Departamento))
                .ForMember(x => x.ITR_CIUDAD, map => map.MapFrom(dto => dto.Ciudad))
                .ForMember(x => x.ITR_DIRECCION, map => map.MapFrom(dto => dto.Direccion))
                .ForMember(x => x.ITR_BARRIO, map => map.MapFrom(dto => dto.Barrio))
                .ForMember(x => x.ITR_TELEFONO, map => map.MapFrom(dto => dto.Telefono))
                .ForMember(x => x.ITR_MODIFICADOPOR, map => map.MapFrom(dto => dto.ModificadoPor))
                .ForMember(x => x.ITRC_MODULO, map => map.MapFrom(dto => dto.Modulo))
                .ReverseMap();


        }
    }
}
