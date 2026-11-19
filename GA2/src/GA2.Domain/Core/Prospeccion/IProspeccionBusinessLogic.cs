using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.Simulador;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Prospeccion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IProspeccionBusinessLogic
    {
        Task<Response<DataClienteProspeccionDto>> ObtenerClienteProspeccion(DataClienteProspeccionDto request);
        Task<Response<DatosProspeccionDto>> GenerarDatosProspeccion(DatosProspeccionDto request);
        Task<Response<ExcepcionTasaUEJDto>> ObtenerExcepcionesTasaUEj(int simId, int uEJ);
        Task<Response<IEnumerable<SimulacionDto>>> CrearSimulacionCliente(CreacionSimulacionClienteDto request);
        Task<Response<object>> LlamadoIntegracionesAfiliado(RequestIntegracionesAfiliadoDto request);
        Task<Response<SolicitudSimulacionDto>> CrearPreAprobado(SolicitudSimulacionDto request);
        Task<Response<SolicitudCreditoDto>> CrearSolicitudCredito(SolicitudCreditoDto request);
        Task<Response<int>> ObtenerSeguroPorUbicacion(int dpcId, int dpdId);
        Task<Response<IEnumerable<DocumentoCargarSolicitudDto>>> ObtenerDocumentosRequeridosSolicCredito(Guid idSolicitud);
        Task<EqivalenciaSimuladorDto> ObtenerEquivalenciasSimuladorCredito(int request);
        Task<DataClienteProspeccionDto> ObtenerClienteTipoNumero(int tipoIdentificacion, string identificacion);
        Task<Response<ResponseTu>> ValidacionCodigoOtpIdVision(RequestTuDto request);
        Task<Response<bool>> CargarArchivosPreAprobado(RequestCargarDocumentosPreAprobASocCreditoDto request);
        Task<Response<bool>> ConsultarVigenciaSimulacion(string request);
        Task<Response<InformacionProspeccion>> ConsultarInformacionPropeccionPorNumeroDocumento(IdentificacionProspeccion request);
        Task<Response<ValoresProspeccion>> ConsultarValoresPropeccionPorNumeroDocumento(IdentificacionProspeccion request);
        Task<Response<DatosProspeccionEndudamiento>> ConsultarPropeccionPorNumeroDocumento(IdentificacionProspeccion request);
        Task<Response<IEnumerable<LineaCreditoObligacion>>> ConsultarLineasCreditoObligacion(IdentificacionProspeccion request);
        Task<Response<SimulacionPlanClienteDetalle>> RestaurarSimulacionCliente(DatosSimulacionUsuarioDto request);
        Task<Response<LineaCreditoObligacion>> LineasCreditoRestaurar(DatosLineaCreditoObligacionDto datosLinea);
        Task<Response<PeritoDetalle>> InsertarPeritoDetalle(PeritoDetalleDto perito);
        Task<Response<PeritoDetalle>> ConsultarPeritoDetalle(string soc_id);
        Task<Response<DatosEndeudamientoProspeccion>> ConsultarEndeudamientoProspeccion(string numeroDocumento);
        Task<Response<SegurosDatosSolicitudCredito>> RegistrarSeguroSolicitudCredito(SegurosDatosSolicitudCreditoDto segurosDatos);
        Task<Response<IEnumerable<ConsultaSegurosCredito>>> ConsultarSegurosSolicitudCredito(ConsultaSegurosCreditoDto request);
        Task<Response<InformacionConceptosCongelados>> ConsultarCongelamientoRecursos(ConceptosCongeladosProspeccion request); 
        Task<Response<InformacionCongelamientoRecursos>> RecursosActualesAfiliadoCredito(IdentificacionProspeccion request);
        Task<Response<InformacionConceptosCongelados>> ConsultarRecursosCongeladosCredito(ConceptosCongeladosProspeccion request);
        Task<Response<TipoCreditoSimulado>> ConsultarTipoCredito(ConsultarCreditoSimulacionDto creditoSimulacionDto); 
    }
}