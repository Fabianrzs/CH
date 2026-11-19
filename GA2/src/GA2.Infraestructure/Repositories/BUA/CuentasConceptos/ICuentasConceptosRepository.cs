using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVC;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categoria = GA2.Application.Dto.Categoria;

namespace GA2.Infraestructure.Repositories
{
    public interface ICuentasConceptosRepository
    {
        Task<IEnumerable<CausalBloqueoCuenta>> ObtenerCausalBloqueoCuentasConceptos();
        Task<BloqueoCuentaConcepto> InsertarBoqueoCuentaConcecto(BloqueoCuentaConcepto bloqueo);
        Task<InsertCuentaConcepto> InsertarNuevaCuenta(InsertCuentaConcepto bloqueo);
        Task<InsertCuentaConcepto> ValidarExisteCuenta(int num_cuenta);
        Task<InsertCuentaConcepto> ValidarExisteCuentaTipo(int idCuenta, int? tipoCuenta);
        Task<InfoCliente> ObtenerInformacionCliente(int tpo_identificacion, string num_identificacion);
        Task<IEnumerable<CuentaCliente>> ObtenerCuentasCliente(int clienteId);
        Task<IEnumerable<ConceptoCuenta>> ObtenerConceptosCuentas(int cuentaId);
        Task<IEnumerable<AportesCliente>> ObtenerAportesCategoriaCliente(int clienteId);
        Task<IEnumerable<MovimientosCuenta>> ObtenerMovimientosCuentaAfiliado(int cuentaId);
        Task<IEnumerable<MovimientosCuenta>> ObtenerMovimientosConceptos(int cuentaId, int conceptoId);
        Task<IEnumerable<CuentaPorConcepto>> ObtenerCuentaConcepto(int idCuenta);
        Task<CuentaPorConcepto> ObtenerConceptoPorClientePorCuentaPorConcepto(int clienteId, int cuentaId, int conceptoId);
        Task<IEnumerable<BloqueoCuentaConcepto>> ObtenerBloqueosCuentasConceptos(int cuentaId);
        Task<IEnumerable<InclusionAportesRequest>> ObtenerInclusionSaldos(int cuentaId);
        Task<IEnumerable<InclusionAportes>> ObtenerInclusionSaldoOpciones(int cuentaId);

        Task<IEnumerable<DatosVerificacionDerechoCuenta>> ObtenerDatosVerificacionDerechosCuentas(VerificacionDerechosByFechaDto request);

        Task<DatosVerificacionDerechoCuenta> ActualizarECTCuentas(ActualizarEctCuentasDto request);
        Task<int> ActualizarECTCuentasList(List<ActualizarEctCuentasDto> request);

        Task<IEnumerable<DatosCuentasAdministracionCesantias>> ObtenerDatosCuentasAdministracionCesantias(VerificacionDerechosByFechaDto request);
        Task<IEnumerable<DatosCuentasAV>> ObtenerDatosCuentasAV(VerificacionDerechosByFechaDto request);
        Task<IEnumerable<ObtenerCuentasAVAC>> ObtenerDatosCuentasAVAC(VerificacionDerechosByFechaDto request);
        Task<IEnumerable<ObtenerCuentasAVAV>> ObtenerDatosCuentasAVAV(VerificacionDerechosByFechaDto request);
        Task<IEnumerable<ObtenerCuentaAVAVResponse>> ObtenerCedulaCuentaAVAV();
        Task<IEnumerable<ObtenerCuentaAVAVResponse>> ObtenerCedulaCuentaAVAC();
        Task<IEnumerable<ObtenerCuentaAVAVResponse>> ObtenerCedulaCuentaAVC();
        Task<IEnumerable<DatosCedulasCesantias>> ObtenerCedulaCuentaCesantidas();
        Task<IEnumerable<ObtenerCuentasAVC>> ObtenerDatosCuentasAVC(VerificacionDerechosByFechaDto request);
        Task<DatosInfoCensatiaByCuentaPerioso> ObtenerDatosCesantiasByCuentaId(ObtenerInfoCedulaRequest request);
        Task<ConceptoCuenta> validarSaldoCuentaConcepto(int conceptoId, int cuentaId);
        Task<ConceptoCuenta> ActualizarSaldoAndValorDevolver(int coceptoId, int clienteId, double valorSueldo, double bloque,int tipoReintegro);
        Task<bool> AfectacionConceptoMasivoEmbargo(int CTA_ID, int CNC_ID, int CCT_SALDO);
        Task<ConceptoCuenta> ActualizarSaldoAndValorDevolverDR(int coceptoId, int cuentaId, double valorSueldo, double bloqueo, double valorDisponible);
        Task<ConceptoCuenta> CreateLogMovimientoCuenta(int coceptoId, int cuentaId, double valorBloqueo, double valorSaldo, int idReintegro);
        Task<LogConceptoCuenta> GetDataLogConceptoCuenta(int coceptoId, int cuentaId);
        Task<bool> ActualizarValorRegresar(int coceptoId, int cuentaId,double valorBloqueo,double valorSaldo,int idReintrro);
        Task<bool> ProcesarSaldoReintegro(int coceptoId, int cuentaId, double valorSaldo);
        Task<Domain.Entities.Movimiento> InsertarMovimientoReitegroUnidadEjecutora(Domain.Entities.Movimiento movimiento);
        Task<Domain.Entities.Movimiento> InsertarLogMovimientoReintegro(Domain.Entities.Movimiento movimiento);
        Task<IEnumerable<MovimientoReintro>> ObtenerMovimientoReintegro();
    }

}
