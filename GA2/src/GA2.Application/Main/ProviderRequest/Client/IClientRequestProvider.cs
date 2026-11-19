using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Credito;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    /// <summary>
    /// Metodo que expone la logica de negocio para consultar api y taer clientes no creados
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public interface IClientRequestProvider
    {
        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificationType">Tipo de identificacion</param>
        /// <param name="identificationNumber">Número de identificación</param>
        /// <param name="idCliente">Identificacion del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        Task<Response<ClienteCesantiasDto>> ObtenerInformacionClientePorDocumentoYTipo(int identificationType, string identificationNumber, int idCliente);

        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="tipoDocumentoId">Identificacion del cliente</param>
        /// <param name="numeroDocumento">Tipo de identificacion</param>
        /// <returns></returns>
        /// <author>Ezequiel Arevalo</author>
        /// <date>05/10/2021</date>
        Task<Response<ClienteDto>> ObtenerClientePorTipoIdentificationYNumero(int tipoDocumentoId, string numeroDocumento);

        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="IdCliente">Id del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        Task<Response<ClienteDto>> ObtenerClienteInformacion(int IdCliente);


        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificaciones">Identificaciones, separadas por ,</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        Task<Response<IEnumerable<ClienteIdentificacionDto>>> ConsultarClientePorIdentificacion(string identificaciones);

        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificacionCliente">Identificacion del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        Task<Response<ClienteCedulaDto>> ObtenerInformacionClienteCedula(int identificacionCliente);
        Task<Response<IEnumerable<ClienteSinCrearDTO>>> RegistrarClientesCargueNomina(string componentes);
        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="cedula">Id del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        Task<Response<CuentaDto>> ObtenerCuentaById(string cedula);

        Task<Response<CuentaDto>> ObtenerCuentaClientePorIdentificadorNumeroCuenta(int idCilente, int numeroCuenta);

        Task<Response<IEnumerable<ClienteDto>>> ObtenerClientesById(string ids);

        Task<Response<ClienteCedulaDto>> ObtenerClientesByNumeroIdentificacion(string identificacion);

        /// <summary>
        /// Método para obtener la información de la cuenta por el identificador de la cuenta
        /// </summary>
        /// <param name="idCuenta">Identificador de la cuenta</param>
        /// <returns></returns>
        Task<Response<IEnumerable<CuentaDto>>> ObtenerCuentaIdCuenta(string idCuenta);

        Task<Response<InfoClienteDto>> ObtenerInfoCliente(int tpo_identificacion, string num_identificacion);

        Task<Response<IEnumerable<CuentaClienteDto>>> ObtenerCuentasClienteId(int idCilente);

        Task<Response<IEnumerable<CuentaPorConceptoDto>>> obtenerCuentaConcepto(int idCuenta);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoDocumentoId"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        Task<Response<ArchivoAplicacionPagoCSVDto>> ObtenerClienteAplicacionPagos(int tipoDocumentoId, string numeroDocumento);

        Task<Response<IEnumerable<CuentaPorConceptoDto>>> ObtenerCuentaConceptoIdCuenta(int IdCuenta);

        Task<Response<CuentaPorConceptoDto>> ObtenerConceptoPorClientePorCuentaPorConcepto(int clienteId, int cuentaId, int conceptoId);

        Task<Response<InfoClienteDto>> ObtenerCuentasConceptosPorTipoNumeroIdentificacion(int identificationType, string identificationNumber);

        Task<Response<ResponseDatosSimulacionParaEstudioDto>> ObtenerCuotasNumeroIdentificacion(string NumeroDocumento);

        Task<Response<Domain.Entities.Movimiento>> InsertarMovimiento(Domain.Entities.Movimiento movimiento);

        Task<Response<ConceptoCuenta>> ActualizarSaldoDebitoCuenta(Domain.Entities.Movimiento movimiento);

        Task<Response<Domain.Entities.Movimiento>> InsertarMovimientoAbonosIntereses(Domain.Entities.Movimiento movimiento);
        Task<Response<ClienteReporteDto>> ObtenerClienteReporte(string identificaciones);
        Task<Response<ClienteNominaDto>> ObtenerClienteNomina(int tipoDocumentoId, string numeroDocumento);
        Task<Response<IEnumerable<ClienteDto>>> ObtenerNombresClientes(List<string> identificaciones);
        Task<Response<ResponseInsertarPermisoActualizacionDto>> InsertarPermisoActualizacion(InsertarPermisoActualizacionDto request);
        Task<Response<IEnumerable<InformacionBasicaClienteDto>>> ObtenerInformacionClientesPorIdentificacion(IEnumerable<string> documentos);
        Task<Response<CausalEstadoClienteDto>> InsertarEstadoCausalCliente(CausalEstadoClienteDto causalCliente);
    }
}