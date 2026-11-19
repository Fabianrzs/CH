using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Embargos.Seven;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IEmbargosRepository
    {

        #region Juzgados


        Task<JuzgadosGuid> ObtenerJuzgadoPorId(Guid id);
        Task<Juzgados> ActualizarJuzgado(Juzgados juzgado);
        Task<Juzgados> CrearJuzgado(Juzgados juzgado);
        Task<Juzgados> InsertarActor(string juzgado);
        Task<Juzgados> EliminarJuzgadoPorId(string idJuzgado);
        Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorCiudad(int? idDpto, int? idCiudad);
        Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorDepartamento(int idDpto);
        Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorNombre(string nombreJuzgado);
       

        Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorDepartamentoCiudadCodigo(int? IdDepartamento,int? IdCiudad,string? Codigo, string? CuentaDeposito, int? Estado);
        Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorNumeroCuentaCodigo(string nCuenta, string codigoInterno);

        #endregion

        Task<IEnumerable<TiposProcesos>> ObtenerTiposProcesosEmbargos();

        Task<IEnumerable<TipoEmbargos>> ObtenerTiposEmbargos();

        Task<Embargo> InsertarEmbargo(InsertEmbargo insertEmbargo);

        Task<InsertarPagoEmbargo> InsertarPagoEmbargo(InsertarPagoEmbargo InsertarPagoEmbargo);

        Task<InsertarPagoEmbargo> ObtenerpagoEmbargo(DateTime InsertarPagoEmbargo);

        Task<IEnumerable<Embargo>> ObtenerEmbargos();

        Task<Embargo> EliminarEmbargo(Guid id);

        Task<Embargo> ActualizarEmbargo(InsertEmbargo insertEmbargo);

        Task<Embargo> ActualizarEstadoEmbargo(ActualizarEstadoEmbargo insertEmbargo);

        Task<EmbargoCuentaConcepto> InsertarEmbargoCuentaConcepto(EmbargoCuentaConcepto embargoCuentaConcepto);

        Task<Embargo> ObtenerEmbargoAdministradorDeTrabajo(string numero);

        Task<IEnumerable<EmbargoCuentaConcepto>> ObtenerEmbargosCuentaConcepto();

        Task<EmbargoCuentaConcepto> EliminarEmbargoCuentaConcepto(Guid id, string Descripcion);


        Task<EmbargoCuentaConcepto> ActualizarEmbargoCuentaConcepto(EmbargoCuentaConcepto embargoCuentaConcepto);


        Task<IEnumerable<TipoRetencion>> ObtenerTiposRetenciones();

        Task<BeneficiariosPagoEmbargo> InsertarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargo beneficiariosPagoEmbargo);

        Task<BeneficiariosPagoEmbargo>  ActualizarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargo beneficiariosPagoEmbargo);

        Task<BeneficiariosPagoEmbargo> EliminarBeneficiariosPagoEmbargos(Guid id);

        Task<IEnumerable<BeneficiariosPagoEmbargo>> ObtenerBeneficiariosEmbargo(Guid idEmbargo);

        Task<IEnumerable<Embargo>> ObtenerEmbargosRangoFiltros(ConsultaEmbargosfiltro filtro);

        Task<Desembargo> InsertarDesembargo(Desembargo id);

        Task<IEnumerable<ObtenerDesembargo>> ObtenerDesembargo();

        Task<TipoIdentificacion> ObtenerTipoIdentificacionPorId(int tipo);


        Task<Embargo> ObtenerEmbargoPorId(Guid idEmbargo);
        Task<IEnumerable<Embargo>> ObtenerEmbargoByIdentificacion(string idEmbargo);

        Task<IEnumerable<EmbargoCuentaConcepto>> ObtenerEmbargoCuentaConceptoPorId(Guid idEmbargo);

        Task<Embargo> InsertarEmbargoGestorDocumental(InsertEmbargoGestorDocumental embargo);
        Task<IEnumerable<EmbargoCuentaConcepto>> ObtenerEmbargosConceptoCuentaByCuentaId(int idCuenta);
        Task<bool> CambiarEstadoEmbargo(Guid idEmbargoCuentaConcepto);

        Task<bool> AfectacionEmbargoMasivo(EmbargarMasivoDto embargarMasivoDto);
        Task<IEnumerable<EstadoCuentaDto>> ObtenerEctEstadoCuenta();

        Task<bool> InsertarPagosEmbargosSeven(PagosEmbargosSevenRespuesta embargosSevenRespuesta);

        Task<PagosEmbargosSevenRespuesta> ConsultarDatosEmbargosPagosSeven(Guid idEmbargo);
    }
}