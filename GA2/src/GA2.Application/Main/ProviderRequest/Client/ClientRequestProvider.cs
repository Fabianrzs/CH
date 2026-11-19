using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Credito;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    /// <summary>
    /// Clase que hace llamado a api BUA para consultar si existe un cliente
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public class ClientRequestProvider : ProviderRequestFactory, IClientRequestProvider
    {
        private readonly IOptions<ApisOptions> _optionsApis;
        public ClientRequestProvider(ITokenClaims tokenClaims, IOptions<ApisOptions> optionsApis) : base(tokenClaims)
        {
            _optionsApis = optionsApis;
        }

        /// <summary>
        /// Metodo que consult a la api
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        public async Task<Response<IEnumerable<ClienteSinCrearDTO>>> RegistrarClientesCargueNomina(string componentes)
        {
            var response = await PostAsyncFile<Response<IEnumerable<ClienteSinCrearDTO>>>(componentes, new Uri(
                 this._optionsApis.Value.BUA + RequestProviderConstants.ApiBuaValidarClientes));

            return response;
        }

        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificationType">Tipo de identificacion</param>
        /// <param name="identificationNumber">Número de identificación</param>
        /// <param name="idCliente">Identificacion del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<ClienteCesantiasDto>> ObtenerInformacionClientePorDocumentoYTipo(int identificationType, string identificationNumber, int idCliente) {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerInformacionClientePorDocumentoYTipo +
                     $"?tipoDocumentoId={identificationType}&numeroDocumento={identificationNumber}" + 
                     $"&idCLiente={idCliente}";
            var response = await GetAsync<Response<ClienteCesantiasDto>>(new Uri(url));

            return response;
        }





        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificationNumber">Identificacion del cliente</param>
        /// <param name="identificationType">Tipo de identificacion</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<IEnumerable<CuentaPorConceptoDto>>> ObtenerCuentaConceptoIdCuenta(int IdCuenta)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuentaConceptoIdCuenta +
                     $"?idCuenta={IdCuenta}";
            var response = await GetAsync<Response<IEnumerable<CuentaPorConceptoDto>>>(new Uri(url));

            return response;
        }


        //

        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="tipoDocumentoId">Identificacion del cliente</param>
        /// <param name="numeroDocumento">Tipo de identificacion</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        /// int tipoDocumentoId, string numeroDocumento
        public async Task<Response<ClienteDto>> ObtenerClientePorTipoIdentificationYNumero(int tipoDocumentoId, string numeroDocumento)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClientePorTipoIdentificationYNumero +
                     $"?tipoDocumentoId={tipoDocumentoId}&numeroDocumento={numeroDocumento}";
            var response = await GetAsync<Response<ClienteDto>>(new Uri(url));

            return response;
        }





        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificationNumber">Identificacion del cliente</param>
        /// <param name="identificationType">Tipo de identificacion</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<InfoClienteDto>> ObtenerCuentasConceptosPorTipoNumeroIdentificacion(int identificationType, string identificationNumber)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuentasConceptosPorTipoNumeroIdentificacion +
                     $"?tipoDocumentoId={identificationType}&numeroDocumento={identificationNumber}";
            var response = await GetAsync<Response<InfoClienteDto>>(new Uri(url));

            return response;
        }



        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="IdCliente">Id del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<ClienteDto>> ObtenerClienteInformacion(int IdCliente) {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClienteInformacion +
                     $"?request={IdCliente}";
            var response = await GetAsync<Response<ClienteDto>>(new Uri(url));

            return response;
        }


        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificaciones">Identificaciones, separadas por ,</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<IEnumerable<ClienteIdentificacionDto>>> ConsultarClientePorIdentificacion(string identificaciones)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ConsultarClientePorIdentificacion +
                     $"?request={identificaciones}";
            var response = await GetAsync<Response<IEnumerable<ClienteIdentificacionDto>>>(new Uri(url));

            return response;
        }


        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="identificacionCliente">Identificacion del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<ClienteCedulaDto>> ObtenerInformacionClienteCedula(int identificacionCliente) {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerInformacionClienteCedula +
                     $"?identificacionCliente={identificacionCliente}";
            var response = await GetAsync<Response<ClienteCedulaDto>>(new Uri(url));

            return response;
        }

        /// <summary>
        /// Metodo que consult a la api de bua, obtener cliente
        /// </summary>
        /// <param name="cedula">Id del cliente</param>
        /// <returns></returns>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public async Task<Response<CuentaDto>> ObtenerCuentaById(string cedula)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuentaById +
                     $"?request={cedula}";
            var response = await GetAsync<Response<CuentaDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<CuentaDto>> ObtenerCuentaClientePorIdentificadorNumeroCuenta(int idCilente, int numeroCuenta)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuentaClientePorIdentificadorNumeroCuenta +
                    $"?NumeroCuenta={numeroCuenta}&Idcliente={idCilente}";
            var response = await GetAsync<Response<CuentaDto>>(new Uri(url));

            return response;
        }


        public async Task<Response<IEnumerable<ClienteDto>>> ObtenerClientesById(string ids)
        {
            try
            {
                string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClientesById + $"?request={ids}";
                var response = await GetAsync<Response<IEnumerable<ClienteDto>>>(new Uri(url));

                return response;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<Response<ClienteCedulaDto>> ObtenerClientesByNumeroIdentificacion(string identificacion)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerInformacionByCedula + "?identificacion=" +identificacion;
            var response = await GetAsync<Response<ClienteCedulaDto>>(new Uri(url));

            return response;
        }

        /// <summary>
        /// Método para obtener la información de la cuenta por el identificador de la cuenta
        /// </summary>
        /// <param name="idCuenta">Identificador de la cuenta</param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<CuentaDto>>> ObtenerCuentaIdCuenta(string idCuenta)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuentaIdCuenta +
                    $"?request={idCuenta}";
            var response = await GetAsync<Response<IEnumerable<CuentaDto>>>(new Uri(url));

            return response;
        }

        public async Task<Response<InfoClienteDto>> ObtenerInfoCliente(int tpo_identificacion, string num_identificacion)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerInformacionCliente+
                    $"?tipoDocumentoId={tpo_identificacion}&numeroDocumento={num_identificacion}";
            var response = await GetAsync<Response<InfoClienteDto>>(new Uri(url));

            return response;

        }

        public async Task<Response<IEnumerable<CuentaClienteDto>>> ObtenerCuentasClienteId(int idCilente)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuentasClienteId +
                    $"?request={idCilente}";
            var response = await GetAsync<Response<IEnumerable<CuentaClienteDto>>>(new Uri(url));

            return response;
        
        }

        public async Task<Response<IEnumerable<CuentaPorConceptoDto>>> obtenerCuentaConcepto(int idCuenta)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.obtenerCuentaConcepto +
                    $"?idCuenta={idCuenta}";
            var response = await GetAsync<Response<IEnumerable<CuentaPorConceptoDto>>>(new Uri(url));

            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoDocumentoId"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public async Task<Response<ArchivoAplicacionPagoCSVDto>> ObtenerClienteAplicacionPagos(int tipoDocumentoId, string numeroDocumento)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClienteAplicacionPagos +
                     $"?tipoDocumentoId={tipoDocumentoId}&numeroDocumento={numeroDocumento}";
            var response = await GetAsync<Response<ArchivoAplicacionPagoCSVDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<CuentaPorConceptoDto>> ObtenerConceptoPorClientePorCuentaPorConcepto(int clienteId, int cuentaId, int conceptoId)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerConceptoPorClientePorCuentaPorConcepto +
                     $"?clienteId={clienteId}&cuentaId={cuentaId}&conceptoId={conceptoId}";
            var response = await GetAsync<Response<CuentaPorConceptoDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<ResponseDatosSimulacionParaEstudioDto>> ObtenerCuotasNumeroIdentificacion(string NumeroDocumento)
        {

            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerCuotasNumeroIdentificacion +
                    $"?request={NumeroDocumento}";
            var response = await GetAsync<Response<ResponseDatosSimulacionParaEstudioDto>>(new Uri(url));

            return response;


        }

        public async Task<Response<Domain.Entities.Movimiento>> InsertarMovimiento(Domain.Entities.Movimiento movimiento)
        {
            var response = await PostAsyncFile<Response<Domain.Entities.Movimiento>>(movimiento,
                new Uri(this._optionsApis.Value.BUA + RequestProviderConstants.InsertarMovimiento));

            return response;
        }

        public async Task<Response<ConceptoCuenta>> ActualizarSaldoDebitoCuenta(Domain.Entities.Movimiento movimiento)
        {
            var response = await PostAsyncFile<Response<ConceptoCuenta>>(movimiento,
               new Uri(this._optionsApis.Value.BUA + RequestProviderConstants.ActualizarSaldoDebitoCuenta));

            return response;
        }
       

        public async Task<Response<Domain.Entities.Movimiento>> InsertarMovimientoAbonosIntereses(Domain.Entities.Movimiento movimiento)
        {
            var response = await PostAsyncFile<Response<Domain.Entities.Movimiento>>(movimiento,
                new Uri(this._optionsApis.Value.BUA + RequestProviderConstants.InsertarMovimientoAbonosIntereses));

            return response;
        }

        public async Task<Response<ClienteReporteDto>> ObtenerClienteReporte(string identificaciones)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClienteReporte +
                     $"?request={identificaciones}";
            var response = await GetAsync<Response<ClienteReporteDto>>(new Uri(url));

            return response;
        }
        //public async Task<Response<ClienteDto>> ObtenerClientePorTipoIdentificationYNumero(int tipoDocumentoId, string numeroDocumento)
        //{

        //    string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClientePorTipoIdentificationYNumero +
        //             $"?tipoDocumentoId={tipoDocumentoId}&numeroDocumento={numeroDocumento}";
        //    var response = await GetAsync<Response<ClienteDto>>(new Uri(url));

        //    return response;
        //}
        public async Task<Response<ClienteNominaDto>> ObtenerClienteNomina(int tipoDocumentoId, string numeroDocumento)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerClienteNomina +
                   $"?tipoDocumentoId={tipoDocumentoId}&numeroDocumento={numeroDocumento}";
            var response = await GetAsync<Response<ClienteNominaDto>>(new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<ClienteDto>>> ObtenerNombresClientes(List<string> identificaciones)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerNombresAfiliados;
            var response = await PostAsyncFile<Response<IEnumerable<ClienteDto>>>(identificaciones, new Uri(url));

            return response;
        }
        public async Task<Response<ResponseInsertarPermisoActualizacionDto>> InsertarPermisoActualizacion(InsertarPermisoActualizacionDto request)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.InsertarPermisoActualizacion;
                     
            var response = await PostAsyncV2<Response<ResponseInsertarPermisoActualizacionDto>>(request, new Uri(url));

            return response;
        }

        public async Task<Response<IEnumerable<InformacionBasicaClienteDto>>> ObtenerInformacionClientesPorIdentificacion(IEnumerable<string> documentos)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.ObtenerInformacionClientesPorIdentificacion;
            var response = await PostAsyncV2<Response<IEnumerable<InformacionBasicaClienteDto>>>(documentos, new Uri(url));
            return response;

        }
        public async Task<Response<CausalEstadoClienteDto>> InsertarEstadoCausalCliente(CausalEstadoClienteDto causalCliente)
        {
            string url = this._optionsApis.Value.BUA + RequestProviderConstants.InsertarEstadoCausalCliente;
            var response = await PostAsyncV2<Response<CausalEstadoClienteDto>>(causalCliente, new Uri(url));
            return response;

        }


    }
}
