using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Mappers
{
    public class ConvocatoriaMapper:Profile
    {
        public ConvocatoriaMapper()
        {
            CreateMap<ConvocatoriaDTO, Convocatoria>()
                    .ForMember(x => x.ID_CONVOCATORIA, map => map.MapFrom(dto => dto.IdConvocatoria))
                    .ForMember(x => x.FECHA_APERTURA, map => map.MapFrom(dto => dto.FechaApertura))
                    .ForMember(x => x.FECHA_CIERRE, map => map.MapFrom(dto => dto.FechaCierre))
                    .ForMember(x => x.FECHA_PUBLICACION, map => map.MapFrom(dto => dto.FechaPublicacion))
                    .ForMember(x => x.NUMERO_BENEFICIARIO, map => map.MapFrom(dto => dto.NumeroBeneficiario))
                    .ForMember(x => x.NUMERO_RESOLUCION, map => map.MapFrom(dto => dto.NumeroResolucion))
                    .ForMember(x => x.NUMERO_CONVOCATORIA, map => map.MapFrom(dto => dto.NumeroConvocatoria))
                    .ForMember(x => x.NUMERO_DIARIOFICIAL, map => map.MapFrom(dto => dto.NumeroDiariOficial))
                    .ForMember(x => x.ESTADO_CONVOCATORIA_ID, map => map.MapFrom(dto => dto.EstadoConvocatoriaId))
                    .ForMember(x => x.FECHARESOLUCION, map => map.MapFrom(dto => dto.FechaResolucion))
                    .ReverseMap();

            CreateMap<Convocatoria, ConvocatoriaDTO>()
                    .ForMember(x => x.IdConvocatoria, map => map.MapFrom(dto => dto.ID_CONVOCATORIA))
                    .ForMember(x => x.FechaApertura, map => map.MapFrom(dto => dto.FECHA_APERTURA))
                    .ForMember(x => x.FechaCierre, map => map.MapFrom(dto => dto.FECHA_CIERRE))
                    .ForMember(x => x.FechaPublicacion, map => map.MapFrom(dto => dto.FECHA_PUBLICACION))
                    .ForMember(x => x.NumeroBeneficiario, map => map.MapFrom(dto => dto.NUMERO_BENEFICIARIO))
                    .ForMember(x => x.NumeroResolucion, map => map.MapFrom(dto => dto.NUMERO_RESOLUCION))
                    .ForMember(x => x.NumeroConvocatoria, map => map.MapFrom(dto => dto.NUMERO_CONVOCATORIA))
                    .ForMember(x => x.NumeroDiariOficial, map => map.MapFrom(dto => dto.NUMERO_DIARIOFICIAL))
                    .ForMember(x => x.EstadoConvocatoriaId, map => map.MapFrom(dto => dto.ESTADO_CONVOCATORIA_ID))
                    .ForMember(x => x.FechaResolucion, map => map.MapFrom(dto => dto.FECHARESOLUCION))
                    .ReverseMap();
            
            CreateMap<ConvocatoriaResponseDto, ConvocatoriaResponse>()
                    .ForMember(x => x.ID_CONVOCATORIA, map => map.MapFrom(dto => dto.IdConvocatoria))
                    .ForMember(x => x.FECHA_APERTURA, map => map.MapFrom(dto => dto.FechaApertura))
                    .ForMember(x => x.FECHA_CIERRE, map => map.MapFrom(dto => dto.FechaCierre))
                    .ForMember(x => x.FECHA_PUBLICACION, map => map.MapFrom(dto => dto.FechaPublicacion))
                    .ForMember(x => x.NUMERO_BENEFICIARIO, map => map.MapFrom(dto => dto.NumeroBeneficiario))
                    .ForMember(x => x.NUMERO_RESOLUCION, map => map.MapFrom(dto => dto.NumeroResolucion))
                    .ForMember(x => x.NUMERO_CONVOCATORIA, map => map.MapFrom(dto => dto.NumeroConvocatoria))
                    .ForMember(x => x.NUMERO_DIARIOFICIAL, map => map.MapFrom(dto => dto.NumeroDiariOficial))
                    .ForMember(x => x.NOMBRE_ESTADO, map => map.MapFrom(dto => dto.NombreEstado))
                    .ForMember(x => x.CON_DELETE, map => map.MapFrom(dto => dto.Delete))
                    .ReverseMap();

            CreateMap<ConvocatoriaResponse, ConvocatoriaResponseDto>()
                    .ForMember(x => x.IdConvocatoria, map => map.MapFrom(dto => dto.ID_CONVOCATORIA))
                    .ForMember(x => x.FechaApertura, map => map.MapFrom(dto => dto.FECHA_APERTURA))
                    .ForMember(x => x.FechaCierre, map => map.MapFrom(dto => dto.FECHA_CIERRE))
                    .ForMember(x => x.FechaPublicacion, map => map.MapFrom(dto => dto.FECHA_PUBLICACION))
                    .ForMember(x => x.NumeroBeneficiario, map => map.MapFrom(dto => dto.NUMERO_BENEFICIARIO))
                    .ForMember(x => x.NumeroResolucion, map => map.MapFrom(dto => dto.NUMERO_RESOLUCION))
                    .ForMember(x => x.NumeroConvocatoria, map => map.MapFrom(dto => dto.NUMERO_CONVOCATORIA))
                    .ForMember(x => x.NumeroDiariOficial, map => map.MapFrom(dto => dto.NUMERO_DIARIOFICIAL))
                    .ForMember(x => x.NombreEstado, map => map.MapFrom(dto => dto.NOMBRE_ESTADO))
                    .ForMember(x => x.Delete, map => map.MapFrom(dto => dto.CON_DELETE  ))
                    .ForMember(x => x.FechaResolucion, map => map.MapFrom(dto => dto.FechaResolucion))
                    .ReverseMap();

            CreateMap<ConvocatoriaResponse, ConvocatoriaDTO>()
        .ForMember(x => x.IdConvocatoria, map => map.MapFrom(dto => dto.ID_CONVOCATORIA))
        .ForMember(x => x.FechaApertura, map => map.MapFrom(dto => dto.FECHA_APERTURA))
        .ForMember(x => x.FechaCierre, map => map.MapFrom(dto => dto.FECHA_CIERRE))
        .ForMember(x => x.FechaPublicacion, map => map.MapFrom(dto => dto.FECHA_PUBLICACION))
        .ForMember(x => x.NumeroBeneficiario, map => map.MapFrom(dto => dto.NUMERO_BENEFICIARIO))
        .ForMember(x => x.NumeroResolucion, map => map.MapFrom(dto => dto.NUMERO_RESOLUCION))
        .ForMember(x => x.NumeroConvocatoria, map => map.MapFrom(dto => dto.NUMERO_CONVOCATORIA))
        .ForMember(x => x.NumeroDiariOficial, map => map.MapFrom(dto => dto.NUMERO_DIARIOFICIAL))
        .ForMember(x => x.FechaResolucion, map => map.MapFrom(dto => dto.FechaResolucion))
        .ReverseMap();



            CreateMap<ConvocatoriaRequest, ConsultarConvocatoriaRequest>()
                    .ForMember(x => x.NoConvocatoria, map => map.MapFrom(dto => dto.NoConvocatoria))
                    .ForMember(x => x.EstadoConvocatoriaId, map => map.MapFrom(dto => dto.EstadoConvocatoriaId))
                    .ForMember(x => x.FechaApertura, map => map.MapFrom(dto => dto.FechaApertura))
                    .ForMember(x => x.FechaCierre, map => map.MapFrom(dto => dto.FechaCierre))
                    .ForMember(x => x.FechaPublicacion, map => map.MapFrom(dto => dto.FechaPublicacion))
                    .ReverseMap();

            CreateMap<ConsultarConvocatoriaRequest, ConvocatoriaRequest>()
        .ForMember(x => x.NoConvocatoria, map => map.MapFrom(dto => dto.NoConvocatoria))
        .ForMember(x => x.EstadoConvocatoriaId, map => map.MapFrom(dto => dto.EstadoConvocatoriaId))
        .ForMember(x => x.FechaApertura, map => map.MapFrom(dto => dto.FechaApertura))
        .ForMember(x => x.FechaCierre, map => map.MapFrom(dto => dto.FechaCierre))
        .ForMember(x => x.FechaPublicacion, map => map.MapFrom(dto => dto.FechaPublicacion))
        .ReverseMap();

            CreateMap<CierreConvocatoriaDto, CierreConvocatoria>()
                .ForMember(x => x.IdConvocatoria, map => map.MapFrom(dto => dto.IdConvocatoria))
                .ForMember(x => x.NumeroResolucionCierre, map => map.MapFrom(dto => dto.NumeroResolucionCierre))
                .ForMember(x => x.FechaCierre, map => map.MapFrom(dto => dto.FechaCierre))
                .ForMember(x => x.FechaApertura, map => map.MapFrom(dto => dto.FechaApertura))
                .ForMember(x => x.NumeroBeneficiarios, map => map.MapFrom(dto => dto.NumeroBeneficiarios))
                .ForMember(x => x.NumeroConvocatoria, map => map.MapFrom(dto => dto.NumeroConvocatoria))
                .ForMember(x => x.ValorOficial, map => map.MapFrom(dto => dto.ValorOficial))
                .ForMember(x => x.ValorSubOficial, map => map.MapFrom(dto => dto.ValorSubOficial))
                .ForMember(x => x.ValorSoldado, map => map.MapFrom(dto => dto.ValorSoldado))
                .ForMember(x => x.Acuerdo, map => map.MapFrom(dto => dto.Acuerdo))
                .ForMember(x => x.FechaPublicacionDiario, map => map.MapFrom(dto => dto.FechaPublicacionDiario))
                .ForMember(x => x.FechaNotificacion, map => map.MapFrom(dto => dto.FechaNotificacion))
                .ReverseMap();
        }
    }
}
