using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;

namespace GA2.Transversals.Mappers
{
    public class ProspeccionMapper : Profile
    {
        public ProspeccionMapper()
        {
            CreateMap<DatosProspeccion, DatosProspeccionDto>()
                .ForMember(x=>x.IdDatosProspeccion, map => map.MapFrom(dto => dto.ID_DATOS_PROSPECCION))
                .ForMember(x=>x.IdSimulacionPersonales, map => map.MapFrom(dto => dto.SDP_ID))
                .ForMember(x=>x.IdEstrato, map => map.MapFrom(dto => dto.ETR_ID))
                .ForMember(x=>x.IdNivelAcademico, map => map.MapFrom(dto => dto.ID_NVA))
                .ForMember(x=>x.IdViviendaPropia, map => map.MapFrom(dto => dto.ID_TVP))
                .ForMember(x=>x.IdTipoContrato, map => map.MapFrom(dto => dto.ID_TIC))
                .ForMember(x=>x.IdPersonasACargo, map => map.MapFrom(dto => dto.ID_PEC))
                .ForMember(x=>x.TotalActivos, map => map.MapFrom(dto => dto.TOTAL_ACTIVOS))
                .ForMember(x=>x.TotalPasivos, map => map.MapFrom(dto => dto.TOTAL_PASIVOS))
                .ForMember(x=>x.DesicionHomologadaTU, map => map.MapFrom(dto => dto.DESICION_HOMOLOGADA))
                .ForMember(x=>x.RiegoCredito, map => map.MapFrom(dto => dto.RIESGO_CREDITO))
                .ForMember(x=>x.CapacidadPagoLibranza, map => map.MapFrom(dto => dto.CAPACIDAD_PAGO_LIBRANZA))
                .ForMember(x=>x.Viabilidad, map => map.MapFrom(dto => dto.VIABILIDAD))
                .ReverseMap(); 
            
            CreateMap<EqivalenciaSimulador, EqivalenciaSimuladorDto>()
                .ForMember(x=>x.Id, map => map.MapFrom(dto => dto.SIM_TCR_ID))
                .ForMember(x=>x.TipoCredito, map => map.MapFrom(dto => dto.TCR_ID))
                .ForMember(x=>x.TipoSimulador, map => map.MapFrom(dto => dto.SIM_ID))
                .ForMember(x=>x.CuotasMinimas, map => map.MapFrom(dto => dto.SIM_TCR_CUOTAS_MINIMAS))
                .ReverseMap();

            CreateMap<DocumentoCargarSolicitud, DocumentoCargarSolicitudDto>()
                .ForMember(x=>x.Id, map => map.MapFrom(dto => dto.TDC_ID))
                .ForMember(x=>x.Nombre, map => map.MapFrom(dto => dto.TDC_DESCRIPCION))
                .ForMember(x=>x.Obligatorio, map => map.MapFrom(dto => dto.DCA_OBLIGATORIO))
                .ReverseMap();
            
            CreateMap<TasaHogarCiudad, TasaHogarCiudadDto>()
                .ForMember(x=>x.TasaId, map => map.MapFrom(dto => dto.SEU_ID))
                .ForMember(x=>x.ValorTasa, map => map.MapFrom(dto => dto.SEU_TODO_RIESGO))
                .ForMember(x=>x.DpcId, map => map.MapFrom(dto => dto.DPC_ID))
                .ForMember(x=>x.DpdId, map => map.MapFrom(dto => dto.DPD_ID))
                .ForMember(x => x.LimiteVis, map => map.MapFrom(dto => dto.SEU_LIMITE_VIS))
                .ForMember(x => x.LimiteVip, map => map.MapFrom(dto => dto.SEU_LIMITE_VIP))
                .ReverseMap();
            
            CreateMap<SolicitudSimulacion, SolicitudSimulacionDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.SLS_ID))
                .ForMember(x => x.FechaSolicitud, map => map.MapFrom(dto => dto.SLS_FECHA_SOLICITUD))
                .ForMember(x => x.TcrId, map => map.MapFrom(dto => dto.TCR_ID))
                .ForMember(x => x.NumeroPreAprobado, map => map.MapFrom(dto => dto.SLS_NUMERO_PREAPROBADO))
                .ForMember(x => x.ProblemaEmail, map => map.MapFrom(dto => dto.SLS_PROBLEMA_EMAIL))
                .ForMember(x => x.EnvioNotificacion, map => map.MapFrom(dto => dto.SLS_ENVIO_NOTIFICACION))
                .ForMember(x => x.Estado, map => map.MapFrom(dto => dto.SLS_ESTADO))
                .ForMember(x => x.UsuarioActualiza, map => map.MapFrom(dto => dto.SLS_USUARIO_ACTUALIZA))
                .ForMember(x => x.FechaActualiza, map => map.MapFrom(dto => dto.SLS_FECHA_ACTUALIZA))
                .ForMember(x => x.Pa_Id, map => map.MapFrom(dto => dto.PA_ID))
                .ForMember(x => x.EndeudamientoCCH, map => map.MapFrom(dto => dto.SLS_PORCENDEUDAMIENTO_CCH))
                .ForMember(x => x.EndeudamientoSCH, map => map.MapFrom(dto => dto.SLS_PORC_ENDEUDAMIENTO_SCH))
                .ForMember(x => x.Score, map => map.MapFrom(dto => dto.SLS_SCORE_TU))
                .ForMember(x => x.Mvc_id, map => map.MapFrom(dto => dto.MEDIOS_CREDITO))
                .ForMember(x => x.NumFolios, map => map.MapFrom(dto => dto.NUM_FOLIOS))
                .ReverseMap();


            CreateMap<SolicitudCredito, SolicitudCreditoDto>().ReverseMap();

            CreateMap<ProyeccionCliente, ProyeccionClienteDto>()
                .ForMember(x => x.Id, map => map.MapFrom(dto => dto.CRG_ID))
                .ForMember(x => x.Descripcion, map => map.MapFrom(dto => dto.CRG_DESCRIPCION))
                .ForMember(x => x.categoriaId, map => map.MapFrom(dto => dto.CTG_ID))
                .ForMember(x => x.factorSubsidio, map => map.MapFrom(dto => dto.CRG_FACTOR_SUBS))
                .ForMember(x => x.AñoAscenso, map => map.MapFrom(dto => dto.CRG_AÑOS_ASCENSO))
                .ForMember(x => x.orden, map => map.MapFrom(dto => dto.CRG_ORDEN))
                .ForMember(x => x.estado, map => map.MapFrom(dto => dto.CRG_ESTADO))
                .ForMember(x => x.color, map => map.MapFrom(dto => dto.ID_COLOR))
                .ForMember(x => x.salariobasico, map => map.MapFrom(dto => dto.CRA_SALARIO_BASICO))
                .ForMember(x => x.añosalariobasico, map => map.MapFrom(dto => dto.CRA_AÑO))
                .ForMember(x => x.ProyectaSM, map => map.MapFrom(dto => dto.CRG_PROYECTA_SM))
                .ForMember(x => x.ProyectaSubsidio, map => map.MapFrom(dto => dto.CRG_PROYECTA_SUBSIDIO))
                .ReverseMap();

            CreateMap<VLL_VALIDACION_LOG_OTP, VLL_VALIDACION_LOG_OTPDto>()
                .ReverseMap();

        }
    }
}
