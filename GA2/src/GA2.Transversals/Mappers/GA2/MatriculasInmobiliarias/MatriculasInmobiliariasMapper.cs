using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.MatriculasInmobiliarias;
using GA2.Domain.Entities;
using GA2.Domain.Entities.MatriculasInmobilarias;
using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Transversals.Mappers
{ 
    public class MatriculasInmobiliariasMapper : Profile
    {

        public MatriculasInmobiliariasMapper() {

            CreateMap<MatriculaInmibiliaria, MatriculaInmibiliariaDto>()
                .ForMember(x => x.MaiId, map => map.MapFrom(dto => dto.MAI_ID))
                .ForMember(x => x.MaiNumeroMatricula, map => map.MapFrom(dto => dto.MAI_NUMERO_MATRICULA))
                .ForMember(x => x.MaiDireccion, map => map.MapFrom(dto => dto.MAI_DIRECCION))
                .ForMember(x => x.MaiFechaRegistro, map => map.MapFrom(dto => dto.MAI_FECHA_REGISTRO))
                .ForMember(x => x.DpdId, map => map.MapFrom(dto => dto.DPD_ID_FK))
                .ForMember(x => x.DcpId, map => map.MapFrom(dto => dto.DPC_ID_FK))
                .ForMember(x => x.PrpNombreRazonSocial, map => map.MapFrom(dto => dto.PRP_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.PrpPrimerNombre, map => map.MapFrom(dto => dto.PRP_PRIMER_NOMBRE))
                .ForMember(x => x.PrpSegundoNombre, map => map.MapFrom(dto => dto.PRP_SEGUNDO_NOMBRE))
                .ForMember(x => x.PrpPrimerApellido, map => map.MapFrom(dto => dto.PRP_PRIMER_APELLIDO))
                .ForMember(x => x.PrpSegundoApellido, map => map.MapFrom(dto => dto.PRP_SEGUNDO_APELLIDO))
                .ForMember(x => x.PrpNumeroIdentificacion, map => map.MapFrom(dto => dto.PRP_NUMERO_IDENTIFICACION))
                .ForMember(x => x.PrpTipoIdentificacion, map => map.MapFrom(dto => dto.PRP_TIPO_IDENTIFICACION))
                .ForMember(x => x.IdOficinaRegistro, map => map.MapFrom(dto => dto.OFC_ID))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
                .ForMember(x => x.nombreUsuario, map => map.MapFrom(dto => dto.NOMBRE_USUARIO))
                .ForMember(x => x.IdUsuario, map => map.MapFrom(dto => dto.ID_USUARIO))
                .ForMember(x => x.Correo, map => map.MapFrom(dto => dto.MAI_CORREO_ELECTRONICO))
                .ForMember(x => x.celular, map => map.MapFrom(dto => dto.MAI_CELULAR))
                .ReverseMap();

            CreateMap<PropietariosMatriculasInmobiliarias, PropietariosMatriculasInmobiliariasDto>()
                .ForMember(x => x.MaiId, map => map.MapFrom(dto => dto.MAI_ID))
                .ForMember(x => x.PrpId, map => map.MapFrom(dto => dto.PRP_ID))
                .ForMember(x => x.PrpNombreRazonSocial, map => map.MapFrom(dto => dto.PRP_NOMBRE_RAZON_SOCIAL))
                .ForMember(x => x.PrpIdTipoIdentificacion, map => map.MapFrom(dto => dto.TID_ID))
                .ForMember(x => x.PrpTipoIdentificacion, map => map.MapFrom(dto => dto.PRP_TIPO_IDENTIFICACION))
                .ForMember(x => x.PrpNumeroIdentificacion, map => map.MapFrom(dto => dto.PRP_NUMERO_IDENTIFICACION))
                .ForMember(x => x.Correo, map => map.MapFrom(dto => dto.CORREO))
                .ForMember(x => x.Telefono, map => map.MapFrom(dto => dto.TELEFONO))
                .ForMember(x => x.PrpPrimerNombre, map => map.MapFrom(dto => dto.PRP_PRIMER_NOMBRE))
                .ForMember(x => x.PrSegundoNombre, map => map.MapFrom(dto => dto.PRP_SEGUNDO_NOMBRE))
                .ForMember(x => x.PrpPrimerApellido, map => map.MapFrom(dto => dto.PRP_PRIMER_APELLIDO))
                .ForMember(x => x.PrpSegundoApellido, map => map.MapFrom(dto => dto.PRP_SEGUNDO_APELLIDO))
                .ForMember(x => x.nombreUsuario, map => map.MapFrom(dto => dto.NOMBRE_USUARIO))
                .ForMember(x => x.PmaFecCre, map => map.MapFrom(dto => dto.PRP_FECHACREACION))
                .ReverseMap();


            CreateMap<NovedadesMatriculasInmobiliarias, NovedadesMatriculasInmobiliariasDto>()
                .ForMember(x => x.MatriculaId, map => map.MapFrom(dto => dto.MAI_ID))
                .ForMember(x => x.FechaNovedad, map => map.MapFrom(dto => dto.NVM_FECHA_NOVEDAD))
                .ForMember(x => x.CausalId, map => map.MapFrom(dto => dto.CAN_ID))
                .ForMember(x => x.SolicitudId, map => map.MapFrom(dto => dto.SOL_ID))
                .ForMember(x => x.UserCreadorId, map => map.MapFrom(dto => dto.NVM_CREATEDOPOR))
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.NVM_ID))
                .ForMember(x => x.TipoOperacionId, map => map.MapFrom(dto => dto.TOP_ID))
                .ForMember(x => x.TipoSolicitudId, map => map.MapFrom(dto => dto.TSN_ID))
                .ForMember(x => x.nombreUsuario, map => map.MapFrom(dto => dto.NOMBRE_USUARIO))
                .ReverseMap();

            CreateMap<HistotialNovedadesMatriculasInmobiliarias, HistotialNovedadesMatriculasInmobiliariasDto>()
              .ForMember(x => x.CausalId, map => map.MapFrom(dto => dto.CAUSAL_ID))
              .ForMember(x => x.FechaNovedad, map => map.MapFrom(dto => dto.FECHA_NOVEDAD))
              .ForMember(x => x.Causal, map => map.MapFrom(dto => dto.CAUSAL))
              .ForMember(x => x.TipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD))
              .ForMember(x => x.TipoOperacion, map => map.MapFrom(dto => dto.TIPO_OPERACION))
              .ForMember(x => x.IdTipoSolicitud, map => map.MapFrom(dto => dto.TIPO_SOLICITUD_ID))
              .ForMember(x => x.NumeroMatricula, map => map.MapFrom(dto => dto.NUMERO_MATRICULA))
              .ForMember(x => x.CreatePorId, map => map.MapFrom(dto => dto.CREATEDOPOR_ID))
              .ForMember(x => x.CreatePor, map => map.MapFrom(dto => dto.CREATEDOPOR))
              .ForMember(x => x.Observaciones, map => map.MapFrom(dto => dto.OBSERVACIONES))
              .ForMember(x => x.FechaRegistro, map => map.MapFrom(dto => dto.FECHA_REGISTRO))
              .ForMember(x => x.IdTipoOperacion, map => map.MapFrom(dto => dto.ID_TIPO_OPERACION))
              .ForMember(x => x.IdMatricula, map => map.MapFrom(dto => dto.ID_MATRICULA))
              .ForMember(x => x.IdNovedad, map => map.MapFrom(dto => dto.ID_NOVEDAD))
              .ForMember(x => x.nombreUsuario, map => map.MapFrom(dto => dto.NOMBRE_USUARIO))
              .ForMember(x => x.TipoOperacionId, map => map.MapFrom(dto => dto.MAI_MATRICULA_INMOBILIARIA_CAUSAL_TIPO))
              .ForMember(x => x.IdUsuario, map => map.MapFrom(dto => dto.ID_USUARIO))

              .ReverseMap();

            CreateMap<TramitesMatriculasInmobiliarias, TramitesMatriculasInmobiliariasDto>()
            .ForMember(x => x.tipoDoc, map => map.MapFrom(dto => dto.TID_DESCRIPCION))
            .ForMember(x => x.nroDoc, map => map.MapFrom(dto => dto.PRP_NUMERO_IDENTIFICACION))
            .ForMember(x => x.nombreRazonSocial, map => map.MapFrom(dto => dto.PRP_NOMBRE_RAZON_SOCIAL))
            .ForMember(x => x.nroRadicado, map => map.MapFrom(dto => dto.SOL_RADICADO_GD))
            .ForMember(x => x.fecSolicitud, map => map.MapFrom(dto => dto.SOL_FECHA_SOLICITUD))
            .ForMember(x => x.valorSolicitud, map => map.MapFrom(dto => dto.SOL_VALOR_A_RETIRAR))
            .ForMember(x => x.estado, map => map.MapFrom(dto => dto.ESS_DESCRIPCION))
            .ForMember(x => x.modalidad, map => map.MapFrom(dto => dto.TIM_DESCRIPCION))
            .ForMember(x => x.tipoSolicitud, map => map.MapFrom(dto => dto.TPS_SUB_DESCRIPCION))
            .ForMember(x => x.idCliente, map => map.MapFrom(dto => dto.CLI_ID))

            .ReverseMap();

            CreateMap<HistorialCambiosMatriculas, HistorialCambiosMatriculasDto>()
           .ForMember(x => x.maiId, map => map.MapFrom(dto => dto.MAI_ID))
           .ForMember(x => x.matcCampo, map => map.MapFrom(dto => dto.MATC_CAMPO))
           .ForMember(x => x.matcId, map => map.MapFrom(dto => dto.MATC_ID))
           .ForMember(x => x.match_user, map => map.MapFrom(dto => dto.MATC_USER))
           .ForMember(x => x.matcVlrAnterior, map => map.MapFrom(dto => dto.MATC_VLR_ANTERIOR))
           .ForMember(x => x.matcVlrNuevo, map => map.MapFrom(dto => dto.MATC_VLR_NUEVO))
           .ForMember(x => x.matc_fecmood, map => map.MapFrom(dto => dto.MATC_FECMOD))
           .ForMember(x => x.matcPestaña, map => map.MapFrom(dto => dto.MATC_PESTAÑA))
           .ForMember(x => x.usuarioBloqueado, map => map.MapFrom(dto => dto.USUARIOBLOQUEO))
           .ReverseMap();

            CreateMap<OperacionesMatriculas, OperacionesMatriculasDto>()
             .ForMember(x => x.Id, map => map.MapFrom(dto => dto.TOP_ID))
             .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
             .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.NOMBRE))
             .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.DESCRIPCION))     
             .ReverseMap();


            CreateMap<CausalNovedamatricula, CausalNovedamatriculaDto>()
             .ForMember(x => x.Id, map => map.MapFrom(dto => dto.CAN_ID))
             .ForMember(x => x.Top_Id, map => map.MapFrom(dto => dto.TOP_ID))
             .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.ESTADO))
             .ForMember(x => x.Nombre, map => map.MapFrom(dto => dto.NOMBRE))
             .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.DESCRIPCION))
             .ReverseMap();

            CreateMap<CausalMatricula, CausalMatriculaInmobiliariaDto>()
             .ForMember(x => x.idCausal, map => map.MapFrom(dto => dto.MAI_MATRICULA_INMOBILIARIA_CAUSAL_ID))
             .ForMember(x => x.nombreCausal, map => map.MapFrom(dto => dto.MAI_MATRICULA_NOMBRE_CAUSAL))
             .ForMember(x => x.tipoCausal, map => map.MapFrom(dto => dto.MAI_MATRICULA_INMOBILIARIA_CAUSAL_TIPO))
             .ReverseMap();

            CreateMap<CrearMatriculaDto, CrearMatricula>()
             .ForMember(x => x.CORREO, map => map.MapFrom(dto => dto.Correo))
             .ForMember(x => x.DPC_NOMBRE_CIUDAD, map => map.MapFrom(dto => dto.Ciudad))
             .ForMember(x => x.DPD_DEPARTAMENTO, map => map.MapFrom(dto => dto.Departamento))
             .ForMember(x => x.MAI_DIRECCION, map => map.MapFrom(dto => dto.Direccion))
             .ForMember(x => x.MAI_FECHA_REGISTRO, map => map.MapFrom(dto => dto.FechaRegistro))
             .ForMember(x => x.MAI_NUMERO_MATRICULA, map => map.MapFrom(dto => dto.NumeroMatricula))
             .ForMember(x => x.PRP_NOMBRE_RAZON_SOCIAL, map => map.MapFrom(dto => dto.RazonSocial))
             .ForMember(x => x.PRP_PRIMER_NOMBRE, map => map.MapFrom(dto => dto.PrimerNombre))
             .ForMember(x => x.PRP_SEGUNDO_NOMBRE, map => map.MapFrom(dto => dto.SegundoNombre))
             .ForMember(x => x.PRP_PRIMER_APELLIDO, map => map.MapFrom(dto => dto.PrimerApellido))
             .ForMember(x => x.PRP_SEGUNDO_APELLIDO, map => map.MapFrom(dto => dto.SegundoApellido))
             .ForMember(x => x.PRP_NUMERO_IDENTIFICACION, map => map.MapFrom(dto => dto.NumeroIdentificacion))
             .ForMember(x => x.PRP_TIPO_IDENTIFICACION, map => map.MapFrom(dto => dto.TipoIdentificacion))
             .ForMember(x => x.TELEFONO, map => map.MapFrom(dto => dto.Celular))
             .ForMember(x => x.OFC_ID, map => map.MapFrom(dto => dto.IdOficinaRegistro))
             .ReverseMap();


            CreateMap<CrearMatriculRespondeDto, CrearMatriculaResponse>()
             .ForMember(x => x.MAI_ID, map => map.MapFrom(dto => dto.MAI_ID))
             .ForMember(x => x.PRP_ID, map => map.MapFrom(dto => dto.PRP_ID))
             .ForMember(x => x.MAI_CREATEDOPOR, map => map.MapFrom(dto => dto.MAI_CREATEDOPOR))
             .ReverseMap();

        }
    }
}
