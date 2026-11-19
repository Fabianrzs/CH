using System;
using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Entidades;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Entidades;

namespace GA2.Transversals.Mappers
{
    public class EntidadesMapper : Profile
    {
        public EntidadesMapper()
        {
            // Model SedeEntidadEducativaDto SedesEntidadEducativa
            CreateMap<SedeEntidadEducativa, SedeEntidadEducativaDto>()
                .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID))
                .ForMember(x => x.CiudadId, map => map.MapFrom(dto => dto.SNE_SEDE_DPC_ID))
                .ForMember(x => x.IdSedeEntidad, map => map.MapFrom(dto => dto.SNE_SEDE_ID))
                .ForMember(x => x.NombreSede, map => map.MapFrom(dto => dto.SNE_SEDE_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.SNE_SEDE_ESTADO))
                .ForMember(x => x.NombreCiudad, map => map.MapFrom(dto => dto.DPC_NOMBRE))
                .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.SNE_CREADOPOR))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.SNE_FECHA_CREACION))
                .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.SNE_MODIFICADOPOR))
                .ForMember(x => x.FechaModificacion, map => map.MapFrom(dto => dto.SNE_FECHA_MODIFICACION))
                .ReverseMap();

            // Model SedeEntidadEducativaDto SedesEntidadEducativa
            CreateMap<Fideicomiso, FidecomisoDto>()
                .ForMember(x => x.entidad_id, map => map.MapFrom(dto => dto.ENE_ID))
                .ForMember(x => x.idTipoIdentificacion, map => map.MapFrom(dto => dto.ENE_TIPO_IDENTIFICACION))
                .ForMember(x => x.nroIdentificacion, map => map.MapFrom(dto => dto.ENE_NUMERO_IDENTIFICACION))
                .ForMember(x => x.fecCreacion, map => map.MapFrom(dto => dto.ENE_FEC_CRE))
                 .ForMember(x => x.nombreRazonSocial, map => map.MapFrom(dto => dto.ENE_RAZON_SOCIAL_FIDEICOMISO))
                 .ForMember(x => x.TipoDocumento, map => map.MapFrom(dto => dto.TipoDocumento))
                .ReverseMap();

            //// Model EntidadEducativaDto EntidadEducativa
            CreateMap<EntidadEducativa, EntidadEducativaDto>()
                    .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID))
                    .ForMember(x => x.TipoIdentificacion, map => map.MapFrom(dto => dto.ENE_TIPO_IDENTIFICACION))
                    .ForMember(x => x.Nit, map => map.MapFrom(dto => dto.ENE_NIT))
                    .ForMember(x => x.RazonSocial, map => map.MapFrom(dto => dto.ENE_NOMBRE_RAZON_SOCIAL))
                    .ForMember(x => x.CorreoElectronico, map => map.MapFrom(dto => dto.ENE_CORREO_ELECTRONICO))
                    .ForMember(x => x.Ciudad, map => map.MapFrom(dto => dto.ENE_DPC_ID))
                    .ForMember(x => x.Direccion, map => map.MapFrom(dto => dto.ENE_DIRECCION))
                    .ForMember(x => x.Telefono, map => map.MapFrom(dto => dto.ENE_TELEFONO))
                    .ForMember(x => x.Contacto, map => map.MapFrom(dto => dto.ENE_NOMBRE_CONTACTO))
                    .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ENE_ESTADO))
                    .ForMember(x => x.NombreCiudad, map => map.MapFrom(dto => dto.ENE_NOMBRE_CIUDAD))
                    .ForMember(x => x.NombreDepartamento, map => map.MapFrom(dto => dto.ENE_NOMBRE_DEPARTAMENTO))
                    .ForMember(x => x.Departamento, map => map.MapFrom(dto => dto.DPD_ID))
                    .ForMember(x => x.esAcreditada, map => map.MapFrom(dto => dto.ENE_CVL_ACREDITADA_ID))
                    .ForMember(x => x.tipoPersona, map => map.MapFrom(dto => dto.ENE_CVL_TIPO_PERSONA_ID))
                    .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.ENE_CREADOPOR))
                    .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.ENE_FECHA_CREACION))
                    .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.ENE_MODIFICADOPOR))
                    .ForMember(x => x.FechaModificacion, map => map.MapFrom(dto => dto.ENE_FECHA_MODIFICACION))
                    .ForMember(x => x.FechaCreacionArchivo, map => map.MapFrom(dto => dto.ENE_PAM_FECHACREACION))
                    .ForMember(x => x.bloqueo, map => map.MapFrom(dto => dto.ENE_BLOQUEO))
                    .ForMember(x => x.renovarArchivo, map => map.MapFrom(dto => dto.RENOVAR_ARCHIVO))
                    .ForMember(x => x.SolicitudesProcesadas, map => map.MapFrom(dto => dto.SOLICITUDES_PROCESADAS))
                    .ReverseMap();

            // Model ProgramaEducativoDto ProgramaEducativo
            CreateMap<ProgramaEducativo, ProgramaEducativoDto>()
                    .ForMember(x => x.Id, map => map.MapFrom(dto => dto.PGE_ID))
                    .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.PGE_ENE_ID))
                    .ForMember(x => x.idTipoPrograma, map => map.MapFrom(dto => dto.PGE_CTL_ID))
                    .ForMember(x => x.nivelEducativoId, map => map.MapFrom(dto => dto.PGE_NVE_ID))
                    .ForMember(x => x.Programa, map => map.MapFrom(dto => dto.PGE_DESCRIPCION))
                    .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.PGE_ESTADO))
                    .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.PGE_CREATEDOPOR))
                    .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.PGE_FECHACREACION))
                    .ForMember(x => x.ModificacionPor, map => map.MapFrom(dto => dto.PGE_MODIFICADOPOR))
                    .ForMember(x => x.FechaModificacion, map => map.MapFrom(dto => dto.PGE_FECHAMODIFICACION))
                    .ReverseMap();

            // Model ListarProgramaEducativoDto ListarProgramaEducativo
            CreateMap<ListarProgramaEducativo, ListarProgramaEducativoDto>()
                .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID))
                .ForMember(x => x.RazonSocialEntidad, map => map.MapFrom(dto => dto.ENE_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.IdPrograma, map => map.MapFrom(dto => dto.PGE_ID))
                .ForMember(x => x.Programa, map => map.MapFrom(dto => dto.PGE_DESCRIPCION))
                .ForMember(x => x.NivelEducativoId, map => map.MapFrom(dto => dto.NVE_ID))
                .ForMember(x => x.NivelEducativo, map => map.MapFrom(dto => dto.NVE_DESCRIPCION))
                .ForMember(x => x.CatalogoValorId, map => map.MapFrom(dto => dto.CVL_ID))
                .ForMember(x => x.CatalogoValorDescripcion, map => map.MapFrom(dto => dto.CVL_DESCRIPCION))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.PGE_ESTADO))
                .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.PGE_CREADOPOR))
                .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.PGE_FECHACREACION))
                .ForMember(x => x.ModificadoPor, map => map.MapFrom(dto => dto.PGE_MODIFICADOPOR))
                .ForMember(x => x.FechaModificacion, map => map.MapFrom(dto => dto.PGE_FECHAMODIFICACION))
                .ForMember(x => x.IdProgramaEntidad, map => map.MapFrom(dto => dto.PGN_ID))
                .ReverseMap();

            // Model NivelEducativoDto NivelEducativo
            CreateMap<NivelEducativoObsolete, NivelEducativoDto>()
                    .ForMember(x => x.id, map => map.MapFrom(dto => dto.PRN_ID))
                    .ForMember(x => x.descripcion, map => map.MapFrom(dto => dto.NVE_DESCRIPCION))
                    .ReverseMap();


            CreateMap<NivelEducativo, NivelEducativoDto>()
                    .ForMember(x => x.id, map => map.MapFrom(dto => dto.NVE_ID))
                    .ForMember(x => x.descripcion, map => map.MapFrom(dto => dto.NVE_DESCRIPCION))
                    .ReverseMap();

            // Model BloqueoEntidadEducativaDto BloqueoEntidadEducativa
            CreateMap<BloqueoEntidadEducativa, BloqueoEntidadEducativaDto>()
                .ForMember(x => x.IdBloqueo, map => map.MapFrom(dto => dto.ENE_BLOQUEO))
                .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID))
                .ForMember(x => x.IdTipoOperacion, map => map.MapFrom(dto => dto.ENE_TIPO_OPERACION))
                .ForMember(x => x.IdCausalBloqueo, map => map.MapFrom(dto => dto.ENE_CAUSAL_BLOQUEO))
                .ForMember(x => x.FechaBloqueo, map => map.MapFrom(dto => dto.ENE_FECHA_BLOQUEO))
                .ForMember(x => x.FuncionarioBloqueo, map => map.MapFrom(dto => dto.ENE_FUNCIONARIO_BLOQUEO))
                .ForMember(x => x.TipoOperacionId, map => map.MapFrom(dto => dto.TIPO_OPERACION_ID))
                .ForMember(x => x.NombreOperacion, map => map.MapFrom(dto => dto.NOMBRE_OPEREACION))
                .ForMember(x => x.CausalOperacionId, map => map.MapFrom(dto => dto.CAUSAL_OPERACION_ID))
                .ForMember(x => x.NombreCausal, map => map.MapFrom(dto => dto.NOMBRE_CAUSAL))
                .ForMember(x => x.UserName, map => map.MapFrom(dto => dto.USR_USERNAME))
                .ForMember(x => x.Observaciones, map => map.MapFrom(dto => dto.ENE_OBSERVACIONES))
                .ReverseMap();

            // Model CuentaBancariaEntidadEducativaDto CuentaBancariaEntidadEducativa
            CreateMap<CuentaBancariaEntidadEducativa, CuentaBancariaEntidadEducativaDto>()
                .ForMember(x => x.IdCuentaBancaria, map => map.MapFrom(dto => dto.ENE_CTA_BANCARIA))
                .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ENE_CTA_ESTADO))
                .ForMember(x => x.RazonSocialEntidad, map => map.MapFrom(dto => dto.ENT_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.IdEntidadBancaria, map => map.MapFrom(dto => dto.ENE_ENT_ID))
                .ForMember(x => x.NumeroCuenta, map => map.MapFrom(dto => dto.ENE_NUMERO_CTA))
                .ForMember(x => x.idTipoCuenta, map => map.MapFrom(dto => dto.ENE_CVL_ID))
                .ForMember(x => x.NombreTipoCuenta, map => map.MapFrom(dto => dto.CVL_DESCRIPCION))
                .ReverseMap();

            // Model CuentaBancariaEntidadEducativaDto CuentaBancariaEntidadEducativa
            CreateMap<ConsultarArchivosPorEntidad, ConsultarArchivosPorEntidadDto>()
                .ForMember(x => x.IdArchivoParametrizado, map => map.MapFrom(dto => dto.ENE_PAM_ID))
                .ForMember(x => x.NombreArchivo, map => map.MapFrom(dto => dto.ENE_PAM_NOMBRE_ARCHIVO))
                .ForMember(x => x.DescripcionArchivo, map => map.MapFrom(dto => dto.ENE_PAM_DESCRIPCION_ARCHIVO))
                .ForMember(x => x.Orden, map => map.MapFrom(dto => dto.ENE_PAM_ORDEN))
                .ForMember(x => x.EsMultiple, map => map.MapFrom(dto => dto.ENE_PAM_MULTIPLE_ARCHIVO))
                .ForMember(x => x.EsRequerido, map => map.MapFrom(dto => dto.ENE_PAM_REQUERIDO))
                .ForMember(x => x.Formato, map => map.MapFrom(dto => dto.ENE_PAM_FORMATO))
                .ForMember(x => x.EstadoParametrizacion, map => map.MapFrom(dto => dto.ENE_PAM_ESTADO))
                .ForMember(x => x.TipoPersonaId, map => map.MapFrom(dto => dto.CVL_ID))
                .ForMember(x => x.TipoPersona, map => map.MapFrom(dto => dto.CVL_DESCRIPCION))
                .ForMember(x => x.IdArchivoCargado, map => map.MapFrom(dto => dto.ENE_ARCHIVOS_ID))
                .ForMember(x => x.NombreArchivoCargado, map => map.MapFrom(dto => dto.ENE_NOMBRE_ARCHIVO))
                .ForMember(x => x.RutaStorage, map => map.MapFrom(dto => dto.ENE_RUTA_STORAGE))
                .ForMember(x => x.Extension, map => map.MapFrom(dto => dto.ENE_EXTENSION))
                .ForMember(x => x.NumeroFolio, map => map.MapFrom(dto => dto.ENE_PAM_NUMERO_FOLIO))
                .ForMember(x => x.FechaCargue, map => map.MapFrom(dto => dto.ENE_FECHA_CARGUE))
                .ForMember(x => x.NuevoCargue, map => map.MapFrom(dto => DateTime.Compare(dto.ENE_FECHA_CARGUE, DateTime.Now.AddYears(-1)) == -1))
                .ReverseMap();

            CreateMap<TramiteEntidades, TramiteEntidadesDto>().ReverseMap();

            CreateMap<HistorialCambios, HistorialCambiosDto>()
                .ForMember(x => x.Campo, map => map.MapFrom(dto => dto.ENEC_CAMPO))
                .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID))
                .ForMember(x => x.Pestaña, map => map.MapFrom(dto => dto.ENEC_PESTANA))
                .ForMember(x => x.VlrNuevo, map => map.MapFrom(dto => dto.ENEC_VLR_NUEVO))
                .ForMember(x => x.VlrAnterior, map => map.MapFrom(dto => dto.ENEC_VLR_ANTERIOR))
                .ForMember(x => x.FechaModificacion, map => map.MapFrom(dto => dto.ENEC_FECMOD))
                .ForMember(x => x.IdUsuario, map => map.MapFrom(dto => dto.ENEC_USER))
                .ForMember(x => x.NombreUsuario, map => map.MapFrom(dto => dto.NOMBRE_COMPLETO))
                .ForMember(x => x.IdEntidadCambio, map => map.MapFrom(dto => dto.ENEC_ID))
                .ReverseMap();

            CreateMap<HistorialCambiosInterviniente, HistorialCambiosIntervinienteDto>()
              .ForMember(x => x.Campo, map => map.MapFrom(dto => dto.ITRC_CAMPO))
              .ForMember(x => x.idInterviniente, map => map.MapFrom(dto => dto.ITR_ID))
              .ForMember(x => x.Pestaña, map => map.MapFrom(dto => dto.ITRC_PESTANA))
              .ForMember(x => x.VlrNuevo, map => map.MapFrom(dto => dto.ITRC_VLR_NUEVO))
              .ForMember(x => x.VlrAnterior, map => map.MapFrom(dto => dto.ITRC_VLR_ANTERIOR))
              .ForMember(x => x.FechaModificacion, map => map.MapFrom(dto => dto.ITRC_FECMOD))
              .ForMember(x => x.NombreUsuario, map => map.MapFrom(dto => dto.NOMBRE_COMPLETO))
              .ForMember(x => x.IdCambio, map => map.MapFrom(dto => dto.ITRC_ID))
              .ForMember(x => x.Modulo, map => map.MapFrom(dto => dto.ITRC_MODULO))
              .ReverseMap();

            CreateMap<crearAnotacion, CrearAnotacionDTO>()
               .ForMember(x => x.Anotacion, map => map.MapFrom(dto => dto.ENE_ANOTACION))
               .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.ENE_CREADOPOR))
               .ForMember(x => x.IdEntidad, map => map.MapFrom(dto => dto.ENE_ID_FK))
               .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.ENE_FECHA_REGISTRO))
               .ReverseMap();

            CreateMap<RespuestaAnotacion, RespuestaAnotacionDTO>()
               .ForMember(x => x.Anotacion, map => map.MapFrom(dto => dto.ENE_ANOTACION))
               .ForMember(x => x.CreadoPor, map => map.MapFrom(dto => dto.ENE_CREADOPOR))
               .ForMember(x => x.FechaCreacion, map => map.MapFrom(dto => dto.ENE_FECHA_REGISTRO))
               .ReverseMap();

            CreateMap<NovedadesEntidadEducativaRequest, NovedadesEntidadEducativaRequestDto>().ReverseMap();
        }
    }
}
