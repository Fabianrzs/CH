import { AplicacionMasivaPagos, AplicacionPago, CierreDiarioCartera, ProductoDto } from '../types/producto.model.type';
import {
    ProductoActionTypes,
    CREAR_PRODUCTO,
    OBTENER_PRODUCTO,
    UPDATE_PRODUCTO,
    UPDATE_PRODUCTO_SUCCESS,
    UPDATE_PRODUCTO_FAILURE,
    ERROR_PRODUCTO,
    APLICACION_DE_PAGO,
    OBTENER_APLICACION_PAGO,
    OBTENER_APLICACION_PAGO_FAILURE,
    OBTENER_CIERRE_DIARIO_CARTERA,
    OBTENER_CIERRE_DIARIO_CARTERA_FAILURE,
    CREAR_COMPROBANTE,
    CREAR_COMPROBANTE_CARTERA,
    CREAR_COMPROBANTE_CARTERA_FAILURE,
    OBTENER_COMPROBANTE,
    OBTENER_COMPROBANTE_CARTERA,
    OBTENER_COMPROBANTE_CARTERA_FAILURE,
    APLICACION_MASIVA_DE_PAGO,
    ACTUALIZAR_MOVIMIENTO_COMPROBANTE,
    ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE,
    OBTENER_COMPROBANTE_PROCESO,
    OBTENER_COMPROBANTE_PROCESO_SUCCESSS,
    OBTENER_COMPROBANTE_PROCESO_FAILURE,
    OBTENER_COMPROBANTE_CREDITO,
    OBTENER_COMPROBANTE_CREDITO_SUCCESSS,
    OBTENER_COMPROBANTE_CREDITO_FAILURE,
    RESET_COMPROBANTE,
    OBTENER_DETALLES_COMPROBANTE,
    OBTENER_DETALLES_COMPROBANTE_SUCCESSS,
    OBTENER_DETALLES_COMPROBANTE_FAILURE,
    GENERAR_COMPROBANTE_PROCESO_FAILURE,
    GENERAR_COMPROBANTE_PROCESO_SUCCESSS,
    GENERAR_COMPROBANTE_PROCESO,


} from '../types/producto.action.type';
import { CarteraComprobanrtesDataModel, lstMovimientoComprobante } from '../models/cartera-comprobantes.model';
import CreditAdapter from '../adapters/credit.adapter';

export interface ProductState {
    id: number;
    error: any;
    productos: ProductoDto[];
    producto: ProductoDto | null;
    loading: boolean;
    aplicarPagos: AplicacionPago | null;
    aplicacionPagos: AplicacionPago [];
    aplicacionMasivaPagos: AplicacionMasivaPagos [];
    cierreDiarioCartera: CierreDiarioCartera [];
    comprobanteCartera: CarteraComprobanrtesDataModel[];
    comprobanteCredito: any[];
    detalleComprobanteCartera: lstMovimientoComprobante[];
    detalleMovimientoId: any;

}

const ProductoStateInicial: ProductState = {
    error: null,
    id: 0,
    loading: false,
    productos: [],
    producto: null,
    aplicarPagos: null,
    aplicacionPagos: [],
    aplicacionMasivaPagos: [],
    cierreDiarioCartera: [],
    // comprobanteCartera: CreditAdapter.adaptCarteraComprobanrtesData({}),
    comprobanteCartera: [],
    comprobanteCredito: [],
    detalleComprobanteCartera:[],
    detalleMovimientoId: null
};

export const ProductoReducer = (    
    state: ProductState = ProductoStateInicial,
    action: ProductoActionTypes,
): ProductState => {
    switch (action.type) {
        case OBTENER_PRODUCTO:
            return {
                ...state,
                loading: false,
                productos: action.productos,
            };
            break;
        case APLICACION_DE_PAGO:
                return {
                    ...state,
                aplicarPagos: action.aplicacionPagos,
                };
                break;
        case OBTENER_APLICACION_PAGO:
            return {
                ...state,
            aplicacionPagos: action.aplicacionPagos,
            };
            break;
        case APLICACION_MASIVA_DE_PAGO:
            return {
                ...state,
                aplicacionMasivaPagos: action.aplicacionMasivaPagos,
            };
            break;
        case OBTENER_APLICACION_PAGO_FAILURE:
            return {
                ...state,
            loading: false,
            error: action.error,
            };
            break;
        case CREAR_PRODUCTO:
            return {
                ...state,
                loading: false,
                producto: action.producto,
            };
            break;
        case UPDATE_PRODUCTO:
            return {
                ...state,
                producto: action.producto,
                loading: true,
            };
            break;
        case UPDATE_PRODUCTO_SUCCESS:
            return {
                ...state,
                producto: action.producto,
                loading: false,
            };
            break;
        case UPDATE_PRODUCTO_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case OBTENER_CIERRE_DIARIO_CARTERA:
            return {
                ...state,
                cierreDiarioCartera: action.cierreDiarioCartera,
                loading: false,
            };
        case OBTENER_CIERRE_DIARIO_CARTERA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case CREAR_COMPROBANTE_CARTERA:
            return {
                ...state,
                comprobanteCartera: action.comprobanteCartera,
                loading: false,
            };
        case CREAR_COMPROBANTE:
            return {
                ...state,
                comprobanteCartera: action.comprobanteCartera,
                loading: true,
            };
        case CREAR_COMPROBANTE_CARTERA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
            case OBTENER_COMPROBANTE:
                return {
                    ...state,
                    comprobanteCartera: action.comprobanteCartera,
                    loading: true,
                };
        case OBTENER_COMPROBANTE_CARTERA:
            return {
                ...state,
                comprobanteCartera: action.comprobanteCartera || [],
                loading: false,
            };
        case OBTENER_COMPROBANTE_CARTERA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case OBTENER_COMPROBANTE_PROCESO:
            return {
                ...state,
                comprobanteCartera: action.comprobanteCartera,
                loading: true,
            };
        case OBTENER_COMPROBANTE_PROCESO_SUCCESSS:
            return {
                ...state,
                comprobanteCartera: action.comprobanteCartera || [],
                loading: false,
            };
        case OBTENER_COMPROBANTE_PROCESO_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        case GENERAR_COMPROBANTE_PROCESO:
            return {
                ...state,
               // comprobanteCartera: action.comprobanteCartera,
                loading: true,
            };
        case GENERAR_COMPROBANTE_PROCESO_SUCCESSS:
            return {
                ...state,
                comprobanteCartera: state.comprobanteCartera.map(existing => 
                    action.comprobanteCartera.find((newItem: CarteraComprobanrtesDataModel) => newItem.comprobante_Id === existing.comprobante_Id) || existing
                ).concat(
                    action.comprobanteCartera.filter((newItem: CarteraComprobanrtesDataModel) => 
                        !state.comprobanteCartera.some((existing: CarteraComprobanrtesDataModel) => existing.comprobante_Id === newItem.comprobante_Id)
                    )
                ),
                loading: false,
            };
        case GENERAR_COMPROBANTE_PROCESO_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        case OBTENER_DETALLES_COMPROBANTE:
            return {
                ...state,
                detalleMovimientoId: action.detalleMovimientoId,
                loading: true,
            };
            case OBTENER_DETALLES_COMPROBANTE_SUCCESSS:
                return {
                    ...state,
                    comprobanteCartera: state.comprobanteCartera.map((comprobante) =>
                        comprobante.detalle_Movimiento_Id === state.detalleMovimientoId
                            ? {
                                ...comprobante,
                                lstMovimientoComprobante: action.detalleComprobanteCartera, 
                            }
                            : comprobante
                    ),
                    loading: false, 
                };
        case OBTENER_DETALLES_COMPROBANTE_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case OBTENER_COMPROBANTE_CREDITO:
            return {
                ...state,
                comprobanteCredito: action.comprobanteCredito,
                loading: true,
            };
        case OBTENER_COMPROBANTE_CREDITO_SUCCESSS:
            return {
                ...state,
                comprobanteCredito: action.comprobanteCredito || [],
                loading: false,
            };
        case OBTENER_COMPROBANTE_CREDITO_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case ACTUALIZAR_MOVIMIENTO_COMPROBANTE:
            return {
                ...state,
                comprobanteCartera: action.comprobanteCartera,
                loading: false,
            };
        case ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case RESET_COMPROBANTE:  
            return {
                ...state,
                comprobanteCartera: [], 
                loading: false,
                error: null,
            };
        case ERROR_PRODUCTO:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
            break;
        default:
            return state;
    }
    return state;
};
