using GA2.Application.Dto;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Entities.Postulacion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IReintegrosBusinessLogic
    {
        Task<Response<IEnumerable<TipoReintegrosDto>>> ObtenerTipoReintegros();

        Task<Response<TipoReintegrosDto>> ObtenerTipoReintegroById(int idTipo);

        Task<Response<IEnumerable<EstadoReintegrosDto>>> ObtenerEstadoReintegros();

        Task<Response<ReintegrosDto>> CrearReintegro(ReintegrosDto reintegro);

        Task<Response<DetalleReintegroDto>> CrearDetalleReintegro(DetalleReintegroDto reintegro);

        Task<Response<IEnumerable<ListaReintegrosDto>>> ObtenerListadoReintegros(ReintegrosParamsDto request);

        Task<Response<IEnumerable<AfiliadoReintegroDto>>> AgregarAfiliadoReintegro(AfiliadoReintegroParamDto request);
        Task<Response<ReintefroDto>> CreateReintegro(ReintefroDto request);
        Task<Response<ReintefroDto>> GetDataReintregoByID(Guid Id);
        Task<Response<AfiliadosReintegrosDto>> UpdateAfiliadoReintegro(AfiliadosReintegrosDto request);
        Task<Response<string>> SaveListAfiliado(IEnumerable<AfiliadoReintegroListDto> afiliados);
        Task<Response<IEnumerable<AfiliadoReintegroListDto>>> GetSaveListAfiliado(Guid idReintegro);
        Task<Response<IEnumerable<AfiliadosReintegrosDto>>> GetAfiliadoReintegro(Guid Id);
        Task<Response<string>> UpdateListAfiliado(IEnumerable<AfiliadosReintegrosDto> afiliados);
        Task<Response<int>> UpdateValuePercent(UpdateValorPercent percent);
        Task<Response<int>> UpdateValueSolicitado(UpdateValorPercent percent);
        Task<Response<IEnumerable<ReintegroResponseDTO>>> GetDataReintegro(AfiliadoConsulta afiliado);
        Task<Response<int>> CleanValueAfiliado(UpdateValorPercent percent);
        Task<Response<CorrespondenciaReintegro>> ObtenerCorrespondenciaReintegros(string radicado);
        Task<Response<PagosSevenDto>> PagosReintegrosSeven(ParametroObtenerReintegroDto solicitud, CancellationToken cancellationToken);
        Task<Response<bool>> GenerarPagoData(DataGenerarPagoDto data);
        Task<Response<InsertarMovContableResponse>> MovContableReintegroSeven(ParametrosMovContableReintegroDto solicitud, CancellationToken cancellationToken);
        Task<byte[]> ReporteOficioReintegro(string radicado);
        Task<Response<string>> ActualizarRadicadoPostulacion(ActualizarPostulacionDto actualizarPostulacionDto);
        Task<Response<FileResult>> GenerarReporteOficio(string radicado);
        Task<bool> GetListSolicitud(string afiliado);
        Task<FileResult> ReportePlanillaPagoReintegro(GenerarPanillaPagoReintegro data,string userId);
        Task<Response<string>> InsertarComunicaciones(insertarComunicacionDto reintegro);
        Task<Response<string>> UpdateDateEndReintegro(Guid idReintegro);
    }
}
