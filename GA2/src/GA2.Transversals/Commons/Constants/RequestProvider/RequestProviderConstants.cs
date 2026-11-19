namespace GA2.Transversals.Commons
{
    /// <summary>
    /// COnstantes de providerrequest
    /// </summary>
    /// <author>Oscar Julian ROjas</author>
    /// <date>06/05/2021</date>
    public static class RequestProviderConstants
    {
        /// <summary>
        /// Url login de ga2 a Bua
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>22/05/2021</date>
        public const string UrlLoginGA2Paa =
            "clientes/ObtenerClientePorTipoDocumentoNumeroFechaExpedicionCelular?TipoDocumento={0}&NumeroIdentificacion={1}&NumeroCelular={2}&FechaExpedicion={3}";

        /// <summary>
        /// Url api bua validar clientes no creados
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>22/05/2021</date>
        public const string ApiBuaValidarClientes = "validarClientes";

        public const string ObtenerCuentaConceptoIdCuenta = "ObtenerCuentaConceptoIdCuenta";

        public const string ObtenerConceptoPorClientePorCuentaPorConcepto = "ObtenerConceptoPorClientePorCuentaPorConcepto";

        public const string ObtenerCuentasConceptosPorTipoNumeroIdentificacion = "ObtenerCuentasConceptosPorTipoNumeroIdentidad";

        /// <summary>
        /// Url api bua obtener cliente
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public const string ObtenerInformacionClientePorDocumentoYTipo = "clientes/ObtenerInformacionClientePorDocumentoYTipo";

        /// <summary>
        /// Url api bua obtener cliente
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public const string ObtenerClienteInformacion = "clientes";


        /// <summary>
        /// Url api bua obtener cliente
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>02/03/2023</date>
        public const string ConsultarClientePorIdentificacion = "clientes/ConsultarClientePorIdentificacion";

        /// <summary>
        /// Url api bua obtener cliente por cedula
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>05/10/2021</date>
        public const string ObtenerInformacionClienteCedula = "clientes/ObtenerInformacionClienteCedula";

        /// <summary>
        /// Url api login username credential
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>25/11/2021</date>
        public const string LoginExternalRequest = "login/external";

        public const string ObtenerCuentaById = "clientes/ObtenerCuentaById";

        public const string ObtenerClientePorTipoIdentificationYNumero = "clientes/obtenerclienteportipoidentificationynumero";

        public const string ObtenerCuentaClientePorIdentificadorNumeroCuenta = "clientes/ObtenerCuentaClientePorIdentificadorNumeroCuenta";

        public const string ObtenerClientesById = "clientes/ObtenerClientesById";

        public const string ObtenerInformacionByCedula = "clientes/ObtenerInformacionByCedula";

        public const string ObtenerCuentaIdCuenta = "ObtenerCuentaIdCuenta";

        public const string ObtenerInformacionCliente = "ObtenerDatosAdministracionCuentas";

        public const string obtenerCuentaConcepto = "ObtenerCuentaConceptoIdCuenta";

        /// <summary>
        /// Url api bua CierreMensual
        /// </summary>
        /// <author>Edwin Lopez</author>
        /// <date>05/05/2022</date>
        public const string CierreMensual = "CierreMensual";

        /// <summary>
        /// Url api bua ConsultarProgramacionCierreMensual
        /// </summary>
        public const string ConsultarProgramacionCierreMensual = "ConsultarProgramacionCierreMensual";


        /// <summary>
        /// Url api bua CierreAnual
        /// </summary>
        public const string CierreAnual = "CierreAnual";


        /// <summary>
        /// ConsultarProgramacionCierreAnual
        /// </summary>
        public const string ConsultarProgramacionCierreAnual = "ConsultarProgramacionCierreAnualCuentas";

        /// <summary>
        /// ReporteCierreMensual
        /// </summary>
        public const string ReporteCierreMensual = "ReporteCierreMensual";


        /// <summary>
        /// ObtenerCuentasClienteId
        /// </summary>
        public const string ObtenerCuentasClienteId = "ObtenerCuentasClienteId";

        /// <summary>
        /// ObtenerClienteAplicacionPagos
        /// </summary>
        public const string ObtenerClienteAplicacionPagos = "clientes/ObtenerClienteAplicacionPagos";

        /// <summary>
        /// ReporteInformacionCliente
        /// </summary>
        public const string ReporteInformacionCliente = "ReporteInformacionCliente";

        /// <summary>
        /// ReporteInformacionCliente
        /// </summary>
        public const string ReporteObtenerInformacionCliente = "ReporteObtenerInformacionCliente";

        /// <summary>
        /// ObtenerInformacionClientesPorIdentificacion
        /// </summary>
        public const string ObtenerInformacionClientesPorIdentificacion = "clientes/ObtenerInformacionClientesPorIdentificacion";

        /// <summary>
        /// ObtenerEmbargoConceptoCuentaPorIdentificadorCuenta
        /// </summary>
        public const string ObtenerEmbargoConceptoCuentaPorIdentificadorCuenta = "ObtenerEmbargosConceptoCuentaByCuentaId";

        /// <summary>
        /// ObtenerDatosOficioRespuesta
        /// </summary>
        public const string ObtenerDatosOficioRespuesta = "ObtenerDatosOficioRespuesta";

        /// <summary>
        /// ObtenerDatosOficioImprocedente
        /// </summary>
        public const string ObtenerDatosOficioImprocedente = "ObtenerDatosOficioImprocedente";


        /// <summary>
        /// GenerarReportesAfiliacion
        /// </summary>
        public const string GenerarReportesAfiliacion = "GenerarReportesAfiliacion";


        /// <summary>
        /// GenerarReportesAfiliacion
        /// </summary>
        public const string ObtenerNombresCliente = "ObtenerNombresCliente";

        /// <summary>
        /// ObtenerCuotasNumeroIdentificacion
        /// </summary>
        public const string ObtenerCuotasNumeroIdentificacion = "clientes/ObtenerCuotasNumeroIdentificacion";


        /// <summary>
        /// ObtenerNombresClientesPorDocumentos
        /// </summary>
        public const string ObtenerNombresClientesPorDocumentos = "ObtenerNombresClientesPorDocumentos";
        /// <summary>
        /// InsertarMovimiento
        /// </summary>
        public const string InsertarMovimiento = "clientes/InsertarMovimiento";
        /// <summary>
        /// ActualizarSaldosDebitoCuentaConceptos
        /// </summary>
        public const string ActualizarSaldoDebitoCuenta = "clientes/ActualizaSaldosCtaConceptoDebito";
        /// <summary>
        /// Obtener clientes correos errados
        /// </summary>
        public const string ObtenerClientesCorreosErrados = "ClienteCorreosErrados";

        /// <summary>
        /// Obtener clientes para la verificacion de derechos y abonar intereses
        /// </summary>
        public const string ObtenerDatosVerificacionDerechosCuentas = "ObtenerDatosVerificacionDerechosCuentas";

        /// <summary>
        /// Obtener clientes cuentas administracion cesantias para abonar intereses
        /// </summary>
        public const string ObtenerDatosCuentasAdministracionCesantias = "ObtenerDatosCuentasAdministracionCesantias";

        /// <summary>
        /// Obtener clientes cuentas AVC  para abonar intereses
        /// </summary>
        public const string ObtenerDatosCuentasAV = "ObtenerDatosCuentasAV";

        public const string InsertarEstadoCausalCliente = "clientes/InsertarEstadoCausalCliente";


        public const string ActualizarECTCuentas = "cuentas/ActualizarEctCuentas";
        public const string ActualizarECTCuentasList = "cuentas/ActualizarECTCuentasList";
        public const string ObtenerConceptosGA2 = "Catalogos/ObtenerConceptos";
        public const string ObtenerTiposDeCuenta = "catalogos/ObtenerTipoCuenta";
        public const string ObtenerEstadosDeCuenta = "Catalogos/ObtenerEctEstadoCuenta";
        public const string ObtenerEstadosDeCliente = "solicitud/ObtenerEstadosCliente";
        public const string ObtenerCausalesEstadosDeCliente = "solicitud/ObtenerCausalEstadoCliente";
        public const string ActualizarSaldoPorcentajeGA2 = "Reintegros/ActualizarSaldoPorcentaje";
        public const string UpdateValueSolicitado = "Reintegros/UpdateValueSolicitado";
        public const string CleanValueAfiliado = "Reintegros/CleanValueAfiliado";
        public const string InsertarMovimientoAbonosIntereses = "clientes/InsertarMovimientoAbonosIntereses";
        public const string ObtenerSolicitudesGA2 = "solicitud/ConsultarSolicitudCesantias";
        public const string ObtenerSolicitudesReintegroGA2 = "Reintegros/GetDataReintegro";
        public const string ObtenerCausalesEstadosCuenta = "catalogos/ObtenerCausalBloqueo";
        public const string ObtenerModalidadesAfiliacionGA2 = "catalogos/obtenerTiposAfiliacion";
        public const string CrearSolicitudGA2 = "solicitud/crearsolicitud";
        public const string ObtenerTiposIdentificacionGA2 = "Catalogos/ObtenerTiposIdentificacion"; 
        public const string ObtenerTiposAfiliacionGA2 = "cartera/obtenerTipoActor";
        public const string ObtenerDatosCuentasAVAC = "ObtenerCuentasAVAC";
        public const string ObtenerDatosCuentasAVAV = "ObtenerCuentasAVAV";
        public const string ObtenerCedulaCuentaAVAV = "ObtenerCedulaCuentaAVAV";
        public const string ObtenerCedulaCuentaAVC = "ObtenerCedulaCuentaAVC";
        public const string ObtenerCedulaCuentaCesantias = "ObtenerCedulaCuentaCesantias";
        public const string ObtenerDatosCuentasAVC = "ObtenerCuentasAVC";
        public const string ObtenerInfoCuentaByPeriodo = "ObtenerInfoCuentaByPeriodo";
        public const string ObtenerValidacionBiometriaGA2 = "credito/validarBiometria";
        public const string ObtenerValoresCatalogosPorIdCatalogo = "Catalogos/ObtenerValoresCatalogosPorIdCatalogo";
        public const string ObtenerConceptosAfectadosSolicitudGA2 = "solicitud/ObtenerConceptosAfectadosSolicitud";
        public const string ObtenerEmbargosGA2 = "ObtenerEmbargoByIdentificacion";
        public const string ListadoSolicitudByAfiliado = "Reintegros/ListadoSolicitudByAfiliado";
        public const string ObtenerFactorSubsidioCategoriaGA2 = "Categoria/ObtenerValoresCategoria";
        public const string ObtenerProyeccionClienteGA2 = "ObtenerProyeccionCliente";
        public const string ObtenerSimCargoGA2 = "ObtenerSimCargo";
        public const string ObtenerParamGeneralesCreditoYCarteraGA2 = "cartera/obtenerParamGeneralesCreditoYCartera";
        public const string ObtenerSimulacionCargoPorCategoriaGA2 = "ObtenerSimulacionCargoPorCategoria";
        public const string ObtenerSalarioMinimoAnualGA2 = "ObtenerSalarioMinimoAnual";
        public const string ObtenerPaisesGA2 = "Catalogos/obtenerpaises";
        public const string ObtenerCiudadesGA2 = "Catalogos/obtenerciudades";
        public const string ObtenerDepartamentosGA2 = "Catalogos/obtenerdepartamentos";
        public const string ObtenerUnidadesEjecutorasGA2 = "Catalogos/obtenerunidadesejecutoras";
        public const string ObtenerGradosGA2 = "Catalogos/obtenergrados";
        public const string EliminarSolicitudes = "Entidades/EliminarSolicitudes";
        public const string ObtenerFuerzasGA2 = "Catalogos/obtenerfuerzas";
        public const string ObtenerEstadosCivilesGA2 = "catalogos/obtenerEstadosCiviles";
        public const string ObtenerOcupacionesGA2 = "catalogos/obtenerProfesiones";
        public const string ObtenerCategoriasGA2 = "Catalogos/obtenercategorias";
        public const string ObtenerActividadesEconomicasGA2 = "catalogos/obtenerActividadesEconomicas";
        public const string ObtenerMonedasGA2 = "catalogos/obtenerTiposMoneda";
        public const string ObtenerTiposDireccionGA2 = "Catalogos/ObtenerTiposDireccion";
        public const string ObtenerTiposTelefonoGA2 = "catalogos/obtenerTiposTelefonos";
        public const string ObtenerTiposCorreoGA2 = "catalogos/obtenerTiposCorreos";
        public const string ObtenerTiposSolicitudGA2 = "catalogos/ObtenerTipoSolicitud";
        public const string ObtenerEmbargosJuzgadosGA2 = "ObtenerEmbargosRangoFiltros";
        public const string ConsultarBloqueoCuentaCredito = "credito/ConsultarBloqueoCuentaCredito";
        public const string ObtenerJuzgadoPorIdGA2 = "ObtenerJuzgadoById";
        public const string obtenerEstadoSolicitudFondo = "postulante/obtenerEstadoSolicitudFondo";

        /// <summary>
        /// ObtenerEmbargoConceptoCuentaPorIdentificadorCuenta
        /// </summary>
        public const string AfectacionEnbargoMasivo = "cuentas/AfectacionCuentasConcepto";

        public const string ObtenerClienteReporte = "clientes/ObtenerClienteReporte";
        public const string ObtenerClienteNomina = "clientes/ObtenerClienteNomina";

        public const string ObtenerNombresAfiliados = "Clientes/ObtenerNombreClientePorCedulas";
        public const string InsertarPermisoActualizacion = "InsertarPermisoActualizacion";

        /// <summary>
        /// InsertarLogAuditoria
        /// </summary>
        public const string InsertarLogAuditoria = "InsertarLogAuditoria";

        /// <summary>
        /// 
        /// </summary>
        public const string ObtenerParametrosIntegraciones = "parametrizacion/ObtenerParametrosIntegraciones";
    }
}
