using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IAfiliacionesBusinessLogic
    {
        Task<Response<IEnumerable<AfiliacionDto>>> ObtenerAfiliacionByIdentificacion(ConsultaAfiliacionesRequestDTO consultaAfiliacion);
        Task<Response<IEnumerable<AfiliacionDto>>> ObtenerAfiliacionesRangoFiltros(ObtenerAfiliacionesRangoFiltrosDto consultaAfiliacion);
        Task<Response<AfiliacionDto>> InsertarAfiliacion(AfiliacionDto afiliacion);
        Task<Response<AfiliacionDto>> InsertarAfiliacionGestorDocumental(InsertarAfiliacionGestorDocumentalDto afiliacion);
        Task<Response<AfiliacionDto>> ActualizarAfiliacion(AfiliacionDto afiliacion);
        Task<Response<AfiliacionDto>> EliminarAfiliacion(int afiliacion);
        Task<Response<IEnumerable<InformacionClienteDto>>> ReportesInformacionCliente(string identificacion);
        Task<Response<IEnumerable<TransferenciaCesantiasDto>>> ObtenerDatosOficioRespuesta(int idAfiliacion); 
        Task<Response<IEnumerable<OficioImprocedenteDto>>> ObtenerDatosOficioImprocedente(int idAfiliacion);
        Task<Response<IEnumerable<OficiosAfiliacionDto>>> GenerarReportesAfiliacion(ParametrosReportesAfiliacionesDto parametros);
        Task<Response<AfiliadoDto>> ObtenerNombresCliente(string identificacion);
        Task<Response<List<AfiliadoDto>>> ObtenerNombresClientesPorDocumentos(List<string> documentos);

        Task<Response<IEnumerable<ReporteClienteCorreosErradosDto>>> ObtenerClientesCorreoErrados();
        Task<Response<ReporteInformacionClienteDto>> ReporteObtenerInformacionCliente(string identificacion);
    }
}
