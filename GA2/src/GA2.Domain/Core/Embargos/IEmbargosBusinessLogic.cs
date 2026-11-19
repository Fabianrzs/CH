using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IEmbargosBusinessLogic
    {
        #region Juzgados

        Task<Response<JuzgadosGuidDto>> ObtenerJuzgadoById(Guid id);

        Task<Response<JuzgadosDto>> ActualizarJuzgado(JuzgadosDto juzgado);
        Task<Response<JuzgadosDto>> CrearJuzgado(JuzgadosDto juzgado);
        Task<Response<JuzgadosDto>> EliminarJuzgadoPorId(string idJuzgado);
        Task<Response<JuzgadosDto>> InsertarActor(TipoactorDto TipoActor);
        Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorCiudad(JuzgadoDptoCiudadDto juzgadoDptoCiudad);
        Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorDepartamento(int idDpto);
        Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorNombre(string nombreDpto);

        Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorDepartamentoCiudadCodigo(JuzgadoDptoCiudadDto juzgadoDptoCiudad);
        Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorNumeroCuentaCodigo(DespachoDto despachoDto);

        #endregion Juzgados


        Task<Response<IEnumerable<TiposProcesosDto>>> ObtenerTiposProcesosEmbargos();

        Task<Response<IEnumerable<TipoEmbargosDto>>> ObtenerTiposEmbargos();

        Task<Response<EmbargosDto>> InsertarEmbargo(InsetEmbargosDto embargo, Guid userId);

        Task<Response<PagoEmbargoDto>> InsertarPagoEmbargo(PagoEmbargoDto pagoembargo, Guid userId);
        Task<Response<PagoEmbargoDto>> ObtenerPagoEmbargo(DateTime pagoembargo);

        Task<Response<IEnumerable<EmbargosDto>>> ObtenerEmbargos();

        Task<Response<EmbargosDto>> EliminarEmbargo(Guid Id);

        Task<Response<EmbargosDto>> ActualizarEmbargo(InsetEmbargosDto embargo, Guid userId);

        Task<Response<EmbargosDto>> ActualizarEstadoEmbargo(ActualizarEstadoEmbargoDto embargo, Guid userId);

        Task<Response<EmbargoCuentaConceptoDto>> InsertarEmbargoCuentaConcepto(EmbargoCuentaConceptoDto dto, Guid userId);

        Task<Response<IEnumerable<EmbargoCuentaConceptoDto>>> ObtenerEmbargosCuentaConcepto();

        Task<Response<List <EmbargoCuentaConceptoDto>>> EliminarEmbargoCuentaConcepto(EmbargoCuentaConceptosDto conceptos );

        Task<Response<EmbargoCuentaConceptoDto>> ActualizarEmbargoCuentaConcepto(EmbargoCuentaConceptoDto emb, Guid userId);

        Task<Response<IEnumerable<TipoRetencionDto>>> ObtenerTiposRetenciones();

        Task<Response<BeneficiariosPagoEmbargoDto>> InsertarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargoDto be, Guid userId);

        Task<Response<BeneficiariosPagoEmbargoDto>> ActualizarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargoDto be, Guid userId);

        Task<Response<BeneficiariosPagoEmbargoDto>> EliminarBeneficiariosPagoEmbargos(Guid Id);

        Task<Response<IEnumerable<BeneficiariosPagoEmbargoDto>>> ObtenerBeneficiariosEmbargo();

        Task<Response<IEnumerable<EmbargosDto>>> ObtenerEmbargosRangoFiltros(ConsultaEmbargosfiltroDto filtros);

        Task<Response<IEnumerable<DesembargoInfoGeneralDto>>> ObtenerEmbargosADesembargar(ConsultaEmbargosfiltroDto filtros);

        Task<Response<DesembargoDto>> InsertarDesembargo(DesembargoDto Id);

        Task<Response<IEnumerable<ObtenerDesembargoDto>>> ObtenerDesembargo();

        Task<Response<IEnumerable<CuentaClienteDto>>> ObtenerCuentasPorRadicadoWordManager(string numeroRadicado);


        Task<Response<DetalleEmbargosDto>> ObtenerConceptosEmbargoById(Guid idEmbargo);

        Task<Response<IEnumerable<EmbargoByDto>>> ObtenerEmbargoByIdentificacion(string identificacion);
        Task<Response<EmbargosDto>> InsertarEmbargoGestorDocumental(InsertEmbargoGestorDocumentalDto embargo, Guid userId);
        Task<Response<IEnumerable<EmbargoCuentaConceptoDto>>> ObtenerEmbargosConceptoCuentaByCuentaId(int idCuenta);
        Task<Response<EmbargoByDto>> ObtenerEmbargoBy(Guid idEmbargo);
        Task<Response<bool>> AfectacionEmbargoMasivo(EmbargarMasivoDto embargarMasivoDto);


    }
}