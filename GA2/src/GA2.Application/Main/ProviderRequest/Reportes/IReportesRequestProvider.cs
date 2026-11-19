using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main
{

    /// <summary>
    /// IReportesRequestProvider
    /// </summary>
    public interface IReportesRequestProvider
    {
        Task<Response<IEnumerable<InformacionClienteDto>>> ReporteInformacionCliente(string identificacion);

        Task<Response<IEnumerable<TransferenciaCesantiasDto>>> ObtenerDatosOficioRespuesta(int idAfiliacion);

        Task<Response<IEnumerable<OficioImprocedenteDto>>> ObtenerDatosOficioImprocedente(int idAfiliacion);

        Task<Response<IEnumerable<OficiosAfiliacionDto>>> GenerarReportesAfiliacion(ParametrosReportesAfiliacionesDto parametros);

        Task<Response<AfiliadoDto>> ObtenerNombresCliente(string identificacion);

        Task<Response<List<AfiliadoDto>>> ObtenerNombresClientesPorDocumentos(List<string> documentos);

        Task<Response<List<ReporteClienteCorreosErradosDto>>> ObtenerClienteCorreosErrados();
        Task<Response<ReporteInformacionClienteDto>> ReporteObtenerInformacionCliente(string identificacion);
    }
}