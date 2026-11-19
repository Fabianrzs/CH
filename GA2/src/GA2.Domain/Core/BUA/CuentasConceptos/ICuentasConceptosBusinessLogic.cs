using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Embargos;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface ICuentasConceptosBusinessLogic
    {
        Task<Response<IEnumerable<CausalBloqueoCuentaDto>>> ObtenerCausalBloqueoCuentasConceptos();
        
        Task<Response<BloqueoCuentaConceptoDto>> InsertarBoqueoCuentaConcecto(BloqueoCuentaConceptoDto bloqueo);

        Task<Response<InfoClienteDto>> ObtenerDatosAdministracionCuentas(int tpo_identificacion, string num_identificacion);

        Task<Response<IEnumerable<CuentaClienteDto>>> ObtenerCuentasCliente(int id);



        Task<Response<IEnumerable<CuentaPorConceptoDto>>> ObtenerCuentaConceptoIdCuenta(int idCuenta);

        Task<Response<CuentaPorConceptoDto>> ObtenerConceptoPorClientePorCuentaPorConcepto(int clienteId, int cuentaId,int conceptoId);


        Task<Response<InfoClienteDto>> ObtenerCuentasConceptosPorTipoNumeroIdentidad(int tpo_identificacion, string num_identificacion);

       Task<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>> ObtenerDatosVerificacionDerechosCuentas(VerificacionDerechosByFechaDto request);

        Task<Response<IEnumerable<DatosCuentasAdministracionCesantiasDto>>> ObtenerDatosCuentasAdministracionCesantias(VerificacionDerechosByFechaDto request);

        Task<Response<DatosVerificacionDerechoCuentaDto>> ActualizarECTCuentas(ActualizarEctCuentasDto request);
        Task<Response<List<DatosVerificacionDerechoCuentaDto>>> ActualizarECTCuentasList(List<ActualizarEctCuentasDto> request);
        Task<Response<IEnumerable<DatosCuentasAVDto>>> ObtenerDatosCuentasAV(VerificacionDerechosByFechaDto request);
        Task<Response<IEnumerable<ObtenerCuentasAVACDto>>> ObtenerDatosCuentasAVAC(VerificacionDerechosByFechaDto request);
        Task<Response<IEnumerable<ObtenerCuentasAVAVDto>>> ObtenerDatosCuentasAVAV(VerificacionDerechosByFechaDto request);
        Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtenerCedulaCuentaAVAV();
        Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtenerCedulaCuentaAVAC();
        Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtenerCedulaCuentaAVC();
        Task<Response<IEnumerable<DatosCedulasCesantiasDto>>> ObtenerCedulaCuentaCesantidas();
        Task<Response<DatosInfoCensatiaByCuentaPeriodoDto>> ObtenerDatosCesantiasByCuentaId(ObtenerInfoCedulaRequestDto request);
        Task<Response<IEnumerable<ObtenerCuentasAVCDto>>> ObtenerDatosCuentasAVC(VerificacionDerechosByFechaDto request);

        Task<Response<IEnumerable<ConceptoCuentaDto>>> ActualizarSaldoConceptoCuenta(List<CuentaConceptoDto> conceptos);

        Task<Response<bool>> AfectacionConceptoMasivoEmbargo(EmbargarMasivoDto embargarMasivoDTO);
        Task<Response<string>> ClealConceptoCuenta(List<ConceptoCuentaCleanDto> conceptos);

        Task<Response<List<ConceptoCuentaDto>>> CrearLogMovimientoCuenta(List<LogMovimientoDto> conceptos);
        Task<Response<string>> ProcesarSaldoConceptoCuenta(List<ConceptoUpdate> conceptos);
    }
}
