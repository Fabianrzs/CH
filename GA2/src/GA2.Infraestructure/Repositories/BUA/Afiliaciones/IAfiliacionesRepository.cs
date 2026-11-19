using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.BUA;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IAfiliacionesRepository
    {
        Task<IEnumerable<Afiliacion>> ObtenerAfiliacionByIdentificacion(ConsultaAfiliacion consultaAfiliacionesRequestDTO);
        Task<IEnumerable<Afiliacion>> ObtenerAfiliacionesRangoFiltros(ObtenerAfiliacionesRangoFiltros consultaAfiliacionesRequestDTO);
        Task<Afiliacion> InsertarAfiliacion(Afiliacion consultaAfiliacionesRequestDTO);
        Task<AfiliacionDto> InsertarAfiliacionGestorDocumental(InsertarAfiliacionGestorDocumental consultaAfiliacionesRequestDTO);
        Task<Afiliacion> ActualizarAfiliacion(Afiliacion consultaAfiliacionesRequestDTO);
        Task<Afiliacion> EliminarAfiliacion(int request);
        Task<IEnumerable<InformacionCliente>> ReporteInformacionCliente(string identificacion);
        Task<BeneficiariosAfiliaciones> ActualizarBeneficiario(BeneficiariosAfiliaciones beneficiario);
        Task<AportesCategoriaAfiliaciones> ActualizarAportes(AportesCategoriaAfiliaciones aporte);
        Task<IEnumerable<BeneficiariosAfiliaciones>> ObtenerBeneficiariosAfiliacion(int idCliente);
        Task<IEnumerable<AportesCategoriaAfiliaciones>> ObtenerAportesAfiliacion(int idCliente);
        Task<IEnumerable<TransferenciaCesantias>> ObtenerDatosOficioRespuesta(int idAfiliacion);
        Task<IEnumerable<OficioImprocedente>> ObtenerDatosOficioImprocedente(int idAfiliacion);
        Task<IEnumerable<TramiteDesafiliacion>> ActualizarTramiteDesafiliacion(TramiteDesafiliacion tramiteDesafiliacion);
        Task<IEnumerable<TramiteDesafiliacion>> ObtenerTramitesDesafiliacion(int idAfiliacion);
        Task<IEnumerable<OficiosAfiliacion>> GenerarReportesAfiliacion(ParametrosReportesAfiliaciones parametros);
        Task<Afiliado> ObtenerNombresCliente(string identificacion);
        Task<List<Afiliado>> ObtenerNombresClientesPorDocumentos(List<string> documentos);

        Task<IEnumerable<ReporteClienteCorreosErrados>> ObtenerClientesCorreoErrados();
        Task<ReporteInformacionClienteDto> ReporteObtenerInformacionCliente(string identificacion);
    }
}
