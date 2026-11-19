import { AplicacionMasivaPagos, AplicacionPago, CierreDiarioCartera, ProductoDto } from './producto.model.type';

// Especificacion de accion
export const CREAR_PRODUCTO = 'CREAR_PRODUCTO';
export const UPDATE_PRODUCTO_SUCCESS = 'UPDATE_PRODUCTO_SUCCESS';
export const UPDATE_PRODUCTO = 'UPDATE_PRODUCTO';
export const UPDATE_PRODUCTO_FAILURE = 'UPDATE_PRODUCTO_FAILURE';
export const OBTENER_PRODUCTO = 'OBTENER_PRODUCTO';
export const OBTENER_PRODUCTO_TYPE = 'OBTENER_PRODUCTO_TYPE';
export const ERROR_PRODUCTO = 'ERROR_PRODUCTO';
export const RESET_CREDIT_STATE = 'RESET_CREDIT_STATE';
export const APLICACION_DE_PAGO = 'APLICACION_DE_PAGO';
export const APLICACION_MASIVA_DE_PAGO = 'APLICACION_MASIVA_DE_PAGO';
export const APLICACION_MASIVA_DE_PAGO_FAILURE = 'APLICACION_MASIVA_DE_PAGO_FAILURE';
export const CLEAR_DETAIL_REQUEST_INFORMATION =
    'CLEAR_DETAIL_REQUEST_INFORMATION';
export const OBTENER_APLICACION_PAGO = 'OBTENER_APLICACION_PAGO';
export const OBTENER_APLICACION_PAGO_FAILURE = 'OBTENER_APLICACION_PAGO_FAILURE';

export const OBTENER_CIERRE_DIARIO_CARTERA = 'OBTENER_CIERRE_DIARIO_CARTERA';
export const OBTENER_CIERRE_DIARIO_CARTERA_FAILURE = 'OBTENER_CIERRE_DIARIO_CARTERA_FAILURE';
export const CREAR_COMPROBANTE = 'CREAR_COMPROBANTE';
export const CREAR_COMPROBANTE_CARTERA = 'CREAR_COMPROBANTE_CARTERA';
export const CREAR_COMPROBANTE_CARTERA_FAILURE = 'CREAR_COMPROBANTE_CARTERA_FAILURE';
export const ACTUALIZAR_MOVIMIENTO_COMPROBANTE = 'ACTUALIZAR_MOVIMIENTO_COMPROBANTE';
export const ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE = 'ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE';
export const OBTENER_COMPROBANTE = 'OBTENER_COMPROBANTE';
export const OBTENER_COMPROBANTE_CARTERA = 'OBTENER_COMPROBANTE_CARTERA';
export const OBTENER_COMPROBANTE_CARTERA_FAILURE = 'OBTENER_COMPROBANTE_CARTERA_FAILURE';
export const OBTENER_COMPROBANTE_PROCESO = 'OBTENER_COMPROBANTE_PROCESO';
export const OBTENER_COMPROBANTE_PROCESO_SUCCESSS = 'OBTENER_COMPROBANTE_PROCESO_SUCCESSS';
export const OBTENER_COMPROBANTE_PROCESO_FAILURE = 'OBTENER_COMPROBANTE_PROCESO_FAILURE';
export const GENERAR_COMPROBANTE_PROCESO = 'GENERAR_COMPROBANTE_PROCESO';
export const GENERAR_COMPROBANTE_PROCESO_SUCCESSS = 'GENERAR_COMPROBANTE_PROCESO_SUCCESSS';
export const GENERAR_COMPROBANTE_PROCESO_FAILURE = 'GENERAR_COMPROBANTE_PROCESO_FAILURE';
export const OBTENER_COMPROBANTE_CREDITO = 'OBTENER_COMPROBANTE_CREDITO';
export const OBTENER_COMPROBANTE_CREDITO_SUCCESSS = 'OBTENER_COMPROBANTE_CREDITO_SUCCESSS';
export const OBTENER_COMPROBANTE_CREDITO_FAILURE = 'OBTENER_COMPROBANTE_CREDITO_FAILURE';
export const RESET_COMPROBANTE = 'RESET_COMPROBANTE';
export const OBTENER_DETALLES_COMPROBANTE = 'OBTENER_DETALLES_COMPROBANTE';
export const OBTENER_DETALLES_COMPROBANTE_FAILURE= 'OBTENER_DETALLES_COMPROBANTE_FAILURE';
export const OBTENER_DETALLES_COMPROBANTE_SUCCESSS = 'OBTENER_DETALLES_COMPROBANTE_SUCCESSS';

export interface ResetComprobante {
    type: typeof RESET_COMPROBANTE;
}
export interface CrearProducto {
    type: 'CREAR_PRODUCTO';
    producto: ProductoDto;
}
export interface ActualizarProducto {
    type: 'UPDATE_PRODUCTO';
    producto: ProductoDto;
}
export interface ActualizarProductoSuccess {
    type: 'UPDATE_PRODUCTO_SUCCESS';
    producto: ProductoDto;
}
export interface UpdateProductFailure {
    type: 'UPDATE_PRODUCTO_FAILURE';
    error: any;
}
export interface GetProductoByIdentificationSuccess {
    type: 'OBTENER_PRODUCTO';
    productos: ProductoDto[];}
export interface GetProductoByIdentificationType {
    type: 'OBTENER_PRODUCTO_TYPE';
}
export interface PostAplicacionDePago {
    type: 'APLICACION_DE_PAGO';
    aplicacionPagos: AplicacionPago;
}

export interface PostAplicacionMasivaPago {
    type: 'APLICACION_MASIVA_DE_PAGO';
    aplicacionMasivaPagos: AplicacionMasivaPagos[];
}

export interface PostAplicacionMasivaPagoFailure {
    type: 'APLICACION_MASIVA_DE_PAGO_FAILURE';
    error: any;
}
export interface SetErrorProducto {
    type: 'ERROR_PRODUCTO';
    error: any;
}

export interface resetCreditState {
    type: 'RESET_CREDIT_STATE';
}

export interface clearDetailRequestInformation {
    type: 'CLEAR_DETAIL_REQUEST_INFORMATION';
}
export interface GetAllAplicacionPago {
    type: 'OBTENER_APLICACION_PAGO';
    aplicacionPagos: AplicacionPago [];
}

export interface GetAllAplicacionPagoFailure {
    type: 'OBTENER_APLICACION_PAGO_FAILURE';
    error: any;
}

export interface PostCierreDiarioCartera {
    type: 'OBTENER_CIERRE_DIARIO_CARTERA';
    cierreDiarioCartera: CierreDiarioCartera [];
}

export interface PostCierreDiarioCarteraFailure {
    type: 'OBTENER_CIERRE_DIARIO_CARTERA_FAILURE';
    error: any;
}

export interface PostCrearComprobanteCartera {
    type: 'CREAR_COMPROBANTE';
    comprobanteCartera: any;
}
export interface PostCrearComprobanteCarteraSuccess {
    type: 'CREAR_COMPROBANTE_CARTERA';
    comprobanteCartera: any;
}

export interface PostCrearComprobanteCarteraFailure {
    type: 'CREAR_COMPROBANTE_CARTERA_FAILURE';
    error: any;
}
export interface PostActualizarMovimientoComprobante {
    type: 'ACTUALIZAR_MOVIMIENTO_COMPROBANTE';
    comprobanteCartera: any;
}

export interface PostActualizarMovimientoComprobanteFailure {
    type: 'ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE';
    error: any;
}

export interface PostObtenerComprobanteCartera {
    type: 'OBTENER_COMPROBANTE';
    comprobanteCartera: any;
}

export interface PostObtenerComprobanteCarteraSucess {
    type: 'OBTENER_COMPROBANTE_CARTERA';
    comprobanteCartera: any;
}

export interface PostObtenerComprobanteCarteraFailure {
    type: 'OBTENER_COMPROBANTE_CARTERA_FAILURE';
    error: any;
}
export interface PostObtenerComprobantePorCodigoProceso {
    type: 'OBTENER_COMPROBANTE_PROCESO';
    comprobanteCartera: any;
}
export interface PostObtenerComprobantePorCodigoProcesoSuccess {
    type: 'OBTENER_COMPROBANTE_PROCESO_SUCCESSS';
    comprobanteCartera: any;
}
export interface PostObtenerComprobantePorCodigoProcesoFailure {
    type: 'OBTENER_COMPROBANTE_PROCESO_FAILURE';
    error: any;
}
export interface PostGenerarComprobanteSevenPorProceso {
    type: 'GENERAR_COMPROBANTE_PROCESO';
    comprobanteCartera: any;
}
export interface PostGenerarComprobanteSevenPorProcesoSuccess {
    type: 'GENERAR_COMPROBANTE_PROCESO_SUCCESSS';
    comprobanteCartera: any;
}
export interface PostGenerarComprobanteSevenPorProcesoFailure {
    type: 'GENERAR_COMPROBANTE_PROCESO_FAILURE';
    error: any;
}

export interface PostObtenerMovimientosContableCartera {
    type: 'OBTENER_DETALLES_COMPROBANTE';
    detalleMovimientoId: any;
}
export interface PostObtenerMovimientosContableCarteraSuccess {
    type: 'OBTENER_DETALLES_COMPROBANTE_SUCCESSS';
    detalleComprobanteCartera: any;
}
export interface PostObtenerMovimientosContableCarteraFailure {
    type: 'OBTENER_DETALLES_COMPROBANTE_FAILURE';
    error: any;
}

export interface PostObtenerValorCdp {
    type: 'OBTENER_COMPROBANTE_CREDITO';
    comprobanteCredito: any;
}
export interface PostObtenerValorCdpSuccess {
    type: 'OBTENER_COMPROBANTE_CREDITO_SUCCESSS';
    comprobanteCredito: any;
}
export interface PostObtenerValorCdpFailure {
    type: 'OBTENER_COMPROBANTE_CREDITO_FAILURE';
    error: any;
}

export type ProductoActionTypes =
    | SetErrorProducto
    | CrearProducto
    | ActualizarProducto
    | ActualizarProductoSuccess
    | UpdateProductFailure
    | GetProductoByIdentificationSuccess
    | GetProductoByIdentificationType
    | clearDetailRequestInformation
    | PostAplicacionDePago
    | PostAplicacionMasivaPago
    | PostAplicacionMasivaPagoFailure
    | resetCreditState
    | GetAllAplicacionPago
    | GetAllAplicacionPagoFailure
    | PostCierreDiarioCartera
    | PostCierreDiarioCarteraFailure
    |PostCrearComprobanteCartera
    |PostCrearComprobanteCarteraSuccess
    |PostCrearComprobanteCarteraFailure
    |PostObtenerComprobanteCartera
    |PostObtenerComprobanteCarteraSucess
    |PostObtenerComprobanteCarteraFailure
    |PostActualizarMovimientoComprobanteFailure
    |PostActualizarMovimientoComprobante
    |PostObtenerComprobantePorCodigoProceso
    |PostObtenerComprobantePorCodigoProcesoSuccess
    |PostObtenerComprobantePorCodigoProcesoFailure
    |PostObtenerValorCdp
    |PostObtenerValorCdpSuccess
    |PostObtenerValorCdpFailure
    |ResetComprobante
    |PostObtenerMovimientosContableCartera
    |PostObtenerMovimientosContableCarteraSuccess
    |PostObtenerMovimientosContableCarteraFailure
    |PostGenerarComprobanteSevenPorProceso
    |PostGenerarComprobanteSevenPorProcesoSuccess
    |PostGenerarComprobanteSevenPorProcesoFailure;
