using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Interfaz que expone la logica de negocio para manejar la informacion de los clientes
    /// </summary>
    public interface IClientesBusinessLogic
    {
        Task<Response<ClienteDto>> ActualizarCliente(ClienteDto client);
        Task<Response<ArchivoAplicacionPagoCSVDto>> ObtenerClienteAplicacionPagos(int identificationType, string identificationNumber, IEnumerable<Claim> claims);
        Task<Response<ClienteDto>> ObtenerClienteInformacion(int Id);
        Task<Response<ClienteDto>> ObtenerClientePorTipoDocumentoNumeroFechaExpedicionCelular(string tipoDocumento, string numeroIdentificacion, string numeroCelular, DateTime fechaExpedicion);
        Task<Response<ClienteDto>> ObtenerClientePorTipoIdentificationYNumero(int identificationType, string identificationNumber, IEnumerable<Claim> claims);
        Task<Response<IEnumerable<ClienteDto>>> ObtenerClientesById(string clientesIds);
        Task<Response<IEnumerable<ClienteIdentificacionDto>>> ConsultarClientePorIdentificacion(string identificaciones);
        Task<Response<CuentaDto>> ObtenerCuentaById(string request);
        Task<Response<CuentaDto>> ObtenerCuentaClientePorIdentificadorNumeroCuenta(int numeroCuenta, int idCliente);
        Task<Response<IEnumerable<CuentaDto>>> ObtenerCuentaIdCuenta(string idCuenta);
        Task<Response<IEnumerable<CuentaAfiliadoDTO>>> ObtenerCuentasMovimientosAfiliado(string identificacion);
        Task<Response<ClienteCedulaDto>> ObtenerInformacionClienteCedula(int identificacionCliente);
        Task<Response<ClienteCesantiasDto>> ObtenerInformacionClientePorDocumentoYTipo(int identificationType, string identificationNumber, int idCLiente);
        Task<Response<ClienteDto>> RestaurarIntegracionCliente(RestaurarIntegracionClienteDTO restaurarIntegracionCliente);
        Task<Response<IEnumerable<ClienteSinCrearDTO>>> ValidarClientes(string file);
        Task<Response<InclusionSaldoRequestDto>> InsertarInclusionSaldos(InclusionSaldoRequestDto file);
        Task<Response<InclusionSaldoRequestDto>> EliminarInclusionSaldosEncabezado(int request);
        Task<Response<InclusionSaldoDto>> EliminarInclusionSaldos(int request);

        Task<Response<ResponseDatosSimulacionParaEstudioDto>> ObtenerCuotasNumeroIdentificacion(string numeroDocumento);
        Task<Response<Domain.Entities.Movimiento>> InsertarMovimiento(Domain.Entities.Movimiento movimiento);
        Task<Response<ConceptoCuenta>> ActualizarSaldoDebitoCuenta(Domain.Entities.Movimiento movimiento);
        Task<Response<IEnumerable<ClienteDto>>> ObtenetClientesPorUnidadEjecutora(int idUnidadEjecutora);
        Task<Response<Domain.Entities.Movimiento>> InsertarMovimientoAbonoIntereses(Domain.Entities.Movimiento movimiento);
        Task<Response<ClienteReporteDto>> ObtenerClienteReporte(string Identificacion);
        Task<Response<ClienteNominaDto>> ObtenerClienteNomina(int identificationType, string identificationNumber);

        Task<Response<IEnumerable<ClienteDto>>> ObtenerNombreClientePorCedulas(List<string> Identificacion);
        Task<Response<ConsultarPermisoActualizacionDto>> ConsultarPermisoActualizacion(string request);
        Task<Response<ResponseInsertarPermisoActualizacionDto>> InsertarPermisoActualizacion(InsertarPermisoActualizacionDto request);
        Task<Response<ReporteFuerzaResumenDto>> GenerarReporteAfiliadosPorFuerza(int idFuerza);
        Task<Response<IEnumerable<ReporteTotalesDto>>> GenerarReporteTotalesPorConcepto();
        Task<Response<IEnumerable<ReporteAfiliadosResponseDto>>> GenerarReporteAfiliados(ReporteAfiliadosRequestDto request);
        Task<Response<IEnumerable<ClienteDto>>> GenerarReporteDatosBasicosAfiliado(ReporteDatosBasicosRequestDto request);
        Task<Response<IEnumerable<InformacionBasicaClienteDto>>> ObtenerInformacionClientesPorIdentificacion(IEnumerable<string> documentos);
        Task<Response<AnotacionClienteDto>> InsertarAnotacionCliente(AnotacionClienteDto request);
        Task<Response<IEnumerable<AnotacionClienteDto>>> ObtenerAnotacionesCliente(AnotacionClienteDto request);
        Task<Response<IEnumerable<CausalEstadoClienteDto>>> ObtenerCausalesEstadoCliente(CausalEstadoClienteDto request);
        Task<Response<CausalEstadoClienteDto>> InsertarEstadoCausalCliente(CausalEstadoClienteDto request);
        Task<Response<AnotacionClienteDto>> EliminarAnotacionCliente(AnotacionClienteDto request);
        Task<Response<CausalEstadoClienteDto>> EliminarEstadoCausalCliente(CausalEstadoClienteDto request);
        Task<Response<CausalEstadoClienteDto>> LevantarEstadoCausalCliente(CausalEstadoClienteDto request);
        UserAgentInfo ParseUserAgent(string request);
        string Ip();

    }
}