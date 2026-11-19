using GA2.Application.Dto;
using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GA2.Domain.Entities.BUA;
using GA2.Application.Dto.BUA;

namespace GA2.Infraestructure.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente> ActualizarCliente(Cliente client,ClienteDto datoAnterior);
        Task<IEnumerable<ClienteSinCrear>> CrearClientesPorCargaNomina(IEnumerable<ClienteSinCrear> clientes);
        Task<Cliente> ObtenerClienteInformacion(int Id);
        Task<ClienteCedula> ObtenerInformacionClienteCedula(int identificacionCliente);
        Task<Cliente> ObtenerClientePorTipoDocumentoNumeroFechaExpedicionCelular(string tipoDocumento, string numeroIdentificacion, string numeroCelular, DateTime fechaExpedicion);
        Task<Cliente> ObtenerClientePorTipoIdentificationYNumero(int identificationType, string identificationNumber);
        Task<IEnumerable<ClienteSinCrear>> ValidarNomina(string json);
        Task<Cliente> InsertarCliente(Cliente client);
        Task<Cliente> RestaurarIntegracionCliente(Cliente client);
        Task<Domain.Entities.Cuenta> InsertarCuentaIntegracion(Domain.Entities.Cuenta cuenta);
        Task<Dependiente> InsertarDependienteIntegracion(Dependiente dependiente);
        Task<AportesCliente> InsertarAportesIntegracion(AportesCliente aportesCliente);
        Task<ClienteCesantias> ObtenerInformacionClientePorDocumentoYTipo(int identificationType, string identificationNumber, int idCLiente);       
        Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerCuentas (int idCliente);        
        Task<IEnumerable<AportesCliente>> ObtenerAportesCliente(int idCliente);        
        Task<IEnumerable<Dependiente>> ObtenerDependientes(int idCliente);
        Task<Domain.Entities.Movimiento> InsertarMovimientoIntegracion(Domain.Entities.Movimiento movimiento);
        Task<IEnumerable<CuentaAfiliado>> ObtenerCuentasMovimientosAfiliado(string identificacion);
        Task<Domain.Entities.Cuenta> ObtenerCuentaById(string request);
        Task<IEnumerable<Domain.Entities.Movimiento>> ObtenerMovimientosCuenta(int idCuenta);
        Task<EstadoCuenta> ObetenerNombreEstadoCuenta(int idCuenta);
        Task<Domain.Entities.Cuenta> ObtenerCuentaClientePorIdentificadorNumeroCuenta(int numeroCuenta,int idCliente);
        Task<IEnumerable<Cliente>> ObtenerClientesById(string clientesIds);
        Task<IEnumerable<ClienteIdentificacion>> ConsultarClientePorIdentificacion(string identificaciones);
        Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerCuentaIdCuenta(string idCuenta);
        Task<Domain.Entities.ConceptosSaldosBloqueos> InsertarConceptosSaldosBloqueos(Domain.Entities.ConceptosSaldosBloqueos conceptosSaldosBloqueos);
        Task<IEnumerable<Domain.Entities.ConceptosSaldosBloqueos>> ObtenerConceptosSaldosBloqueos(int idCuenta);
        Task<ArchivoAplicacionPagoCSV> ObtenerClienteAplicacionPagos(int identificationType, string identificationNumber);
        Task<InclusionAportes> InsertarInclusionSaldo(InclusionAportes inclusionAportes);
        Task<InclusionAportesRequest> InsertarInclusionSaldoEncabezado(InclusionAportesRequest inclusionAportes);
        Task<ConsecutivoInclusionSaldos> ObtenerConsecutivoInclusionSaldos();
        Task<InclusionAportesRequest> InsertarRecaudos(InclusionAportesRequest inclusionAportes, InclusionAportes aportes);
        Task<IEnumerable<ConceptoCuenta>> ObtenerCuentasConceptoPorIdentificadorCliente(int idCliente);
        Task<ConceptoCuenta> ActualizarSaldosCuentasConceptos(decimal valor, int idConcepto, int idCliente);
        Task<MovimientosCuenta> InsertarMovimientoInclusionSaldos(MovimientosCuenta movimiento, int idCliente);
        Task<InclusionAportesRequest> EliminarInclusionSaldosEncabezado(int inclusionAportes);
        Task<InclusionAportes> EliminarInclusionSaldos(int inclusionAportes);

        Task<ResponseDatosSimulacionParaEstudio> ObtenerCuotasNumeroIdentificacion(string numeroDocumento);
        Task<Domain.Entities.Movimiento> InsertarMovimiento(Domain.Entities.Movimiento movimiento);
        Task<ConceptoCuenta> ActualizarSaldoDebitoCuenta(Domain.Entities.Movimiento movimiento);
        Task<IEnumerable<Cliente>> ObtenetClientesPorUnidadEjecutora(int idUnidadEjecutora);

        Task<Domain.Entities.Movimiento> InsertarMovimientoAbonoInteres(Domain.Entities.Movimiento movimiento);
        Task<AfiliadoDeBeneficiario> ObtenerAfiliadoDeBeneficiario(string identificacion);
        Task<ClienteCedulaReporte> ObtenerClienteReporte(string Identificacion);
        Task<ClienteNomina> ObtenerClienteNomina(string Identificacion);


        Task<IEnumerable<Cliente>> ObtenerNombreClientePorCedulas(string idUnidadEjecutora);
        Task<ConsultarPermisoActualizacionDto> ConsultarPermisoActualizacion(string request);
        Task<ResponseInsertarPermisoActualizacionDto> InsertarPermisoActualizacion(InsertarPermisoActualizacionDto request);
        Task<IEnumerable<ReporteFuerza>> ConsultaReporteAfiliadosPorFuerza(int idFuerza);
        Task<IEnumerable<ReporteTotalesDto>> GenerarReporteTotalesPorConcepto();
        Task<IEnumerable<ReporteAfiliadosResponseDto>> ConsultaReporteAfiliados(ReporteAfiliadosRequestDto requestDto);
        Task<IEnumerable<Cliente>> GenerarReporteDatosBasicosAfiliado(int idDepartamento, int idCiudad);
        Task<IEnumerable<InformacionBasicaCliente>> ObtenerInformacionClientesPorIdentificacion(IEnumerable<string> documentos);
        Task<AnotacionCliente> InsertarAnotacionCliente(AnotacionClienteDto request);
        Task<IEnumerable<AnotacionCliente>> ObtenerAnotacionesCliente(AnotacionClienteDto request);
        Task<IEnumerable<CausalEstadoCliente>> ObtenerCausalesEstadoCliente(CausalEstadoClienteDto request);
        Task<CausalEstadoCliente> InsertarEstadoCausalCliente(CausalEstadoClienteDto request);
        Task<AnotacionCliente> EliminarAnotacionCliente(AnotacionClienteDto request);
        Task<CausalEstadoCliente> EliminarEstadoCausalCliente(CausalEstadoClienteDto request);
        Task<CausalEstadoCliente> LevantarEstadoCausalCliente(CausalEstadoClienteDto request); 
    }
}