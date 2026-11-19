import { Dispatch } from 'redux';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import { AppActions } from '../types/appactions.type';
import {
    UPDATE_PRODUCTO,
    UPDATE_PRODUCTO_SUCCESS,
    UPDATE_PRODUCTO_FAILURE,
    OBTENER_PRODUCTO,
    RESET_CREDIT_STATE,
    CLEAR_DETAIL_REQUEST_INFORMATION,
    ERROR_PRODUCTO,
    APLICACION_DE_PAGO,
    OBTENER_APLICACION_PAGO,
    OBTENER_APLICACION_PAGO_FAILURE,
    OBTENER_CIERRE_DIARIO_CARTERA,
    OBTENER_CIERRE_DIARIO_CARTERA_FAILURE,
    CREAR_COMPROBANTE_CARTERA,
    CREAR_COMPROBANTE_CARTERA_FAILURE,
    ACTUALIZAR_MOVIMIENTO_COMPROBANTE,
    ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE,
    CREAR_COMPROBANTE,
    OBTENER_COMPROBANTE,
    OBTENER_COMPROBANTE_CARTERA,
    OBTENER_COMPROBANTE_CARTERA_FAILURE,
    APLICACION_MASIVA_DE_PAGO,
    APLICACION_MASIVA_DE_PAGO_FAILURE,
    OBTENER_COMPROBANTE_PROCESO_SUCCESSS,
    OBTENER_COMPROBANTE_PROCESO,
    OBTENER_COMPROBANTE_PROCESO_FAILURE,
    GENERAR_COMPROBANTE_PROCESO_SUCCESSS,
    GENERAR_COMPROBANTE_PROCESO,
    GENERAR_COMPROBANTE_PROCESO_FAILURE,
    OBTENER_COMPROBANTE_CREDITO,
    OBTENER_COMPROBANTE_CREDITO_FAILURE,
    OBTENER_COMPROBANTE_CREDITO_SUCCESSS,
    RESET_COMPROBANTE,
    OBTENER_DETALLES_COMPROBANTE,
    OBTENER_DETALLES_COMPROBANTE_FAILURE,
    OBTENER_DETALLES_COMPROBANTE_SUCCESSS

} from '../types/producto.action.type';
import { AplicacionMasivaPagos, AplicacionPago, CierreDiarioCartera, InsertAplicacionPagoBody, procesoAplicacionMasivaPago, ProductoDto } from '../types/producto.model.type';
import ApiproductoClients from '../services/api-producto.service';
import NotificationService from '../services/notification.service';
import AuthService from '../services/auth.service';
import CarteraApi from '../services/api-cartera-service';
import CreditApi from '../services/api-credit.service';

const api = ApiproductoClients();
const apiCartera = CarteraApi();
const apicredito = CreditApi();

export const ObtenerProductoByIdentificationFailure = (
    error: any,
): AppActions => ({
    type: ERROR_PRODUCTO,
    error: error,
});

export const resetProducState = (): AppActions => ({
    type: RESET_CREDIT_STATE,
});

export const clearDetailRequestInformation = (): AppActions => ({
    type: CLEAR_DETAIL_REQUEST_INFORMATION,
});

export const aplicacionDePagos = (result: AplicacionPago): AppActions => ({
    type: APLICACION_DE_PAGO,
    aplicacionPagos: result,
});

export const PostAplicacionMasivaPago = (aplicacionMasivaPagos: AplicacionMasivaPagos[]): AppActions => ({
    type: APLICACION_MASIVA_DE_PAGO,
    aplicacionMasivaPagos: aplicacionMasivaPagos,
});

export const AplicacionMasivaPagoClearDataAction = (): AppActions => ({
    type: APLICACION_MASIVA_DE_PAGO,
    aplicacionMasivaPagos: [],
});

export const GetAllAplicacionMasivaPagoFailure = (
    error: any,
): AppActions => ({
    type: APLICACION_MASIVA_DE_PAGO_FAILURE,
    error: error
});

export const GetProductoByIdentificationSuccess = (
    productos: ProductoDto[],
): AppActions => ({
    type: OBTENER_PRODUCTO,
    productos: productos,
});

export const GetAllAplicacionPago = (
    aplicacionPagos: AplicacionPago[],
): AppActions => ({
    type: OBTENER_APLICACION_PAGO,
    aplicacionPagos: aplicacionPagos
});

export const GetAllAplicacionPagoFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_APLICACION_PAGO_FAILURE,
    error: error
});

export const PostCierreDiarioCartera = (
    cierreDiarioCartera: CierreDiarioCartera[],
): AppActions => ({
    type: OBTENER_CIERRE_DIARIO_CARTERA,
    cierreDiarioCartera: cierreDiarioCartera
});
export const PostActualizarMovimientoComprobante = (
    comprobanteCartera: any,
): AppActions => ({
    type: ACTUALIZAR_MOVIMIENTO_COMPROBANTE,
    comprobanteCartera: comprobanteCartera
});
export const PostObtenerComprobanteCarteraSuccess = (
    comprobanteCartera: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_CARTERA,
    comprobanteCartera: comprobanteCartera
});
export const PostObtenerComprobantePorCodigoProcesoSuccess = (
    comprobanteCartera: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_PROCESO_SUCCESSS,
    comprobanteCartera: comprobanteCartera
});
export const PostGenerarComprobanteSevenPorProcesoSuccess = (
    comprobanteCartera: any,
): AppActions => ({
    type: GENERAR_COMPROBANTE_PROCESO_SUCCESSS,
    comprobanteCartera: comprobanteCartera
});
export const PostAnularMovimientoContableSuccess = (
    comprobanteCartera: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_PROCESO_SUCCESSS,
    comprobanteCartera: comprobanteCartera
});
export const PostObtenerMovimientosContableCarteraSuccess = (
    detalleComprobanteCartera: any,
): AppActions => ({
    type: OBTENER_DETALLES_COMPROBANTE_SUCCESSS,
    detalleComprobanteCartera: detalleComprobanteCartera
});
export const PostObtenerValorCdpSuccess = (
    comprobanteCredito: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_CREDITO_SUCCESSS,
    comprobanteCredito: comprobanteCredito
});
export const PostCrearComprobanteCartera = (
    comprobanteCartera: any,
): AppActions => ({
    type: CREAR_COMPROBANTE,
    comprobanteCartera: comprobanteCartera
});
export const PostObtenerComprobanteCartera = (
    comprobanteCartera: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE,
    comprobanteCartera: comprobanteCartera
});
export const PostObtenerComprobantePorCodigoProceso = (
    comprobanteCartera: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_PROCESO,
    comprobanteCartera: comprobanteCartera

});
export const PostGenerarComprobanteSevenPorProceso = (
    comprobanteCartera: any,
): AppActions => ({
    type: GENERAR_COMPROBANTE_PROCESO,
    comprobanteCartera: comprobanteCartera

});
export const PostAnularMovimientoContable = (
    comprobanteCartera: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_PROCESO,
    comprobanteCartera: comprobanteCartera

});
export const PostObtenerMovimientosContableCartera = (
    detalleMovimientoId: any,
): AppActions => ({
    type: OBTENER_DETALLES_COMPROBANTE,
    detalleMovimientoId: detalleMovimientoId

});
export const PostObtenerValorCdp = (
    comprobanteCredito: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_CREDITO,
    comprobanteCredito: []
});
export const PostCrearComprobanteCarteraSuccess = (
    comprobanteCartera: any,
): AppActions => ({
    type: CREAR_COMPROBANTE_CARTERA,
    comprobanteCartera: comprobanteCartera
});
export const PostCierreDiarioCarteraFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_CIERRE_DIARIO_CARTERA_FAILURE,
    error: error
});
export const PostCrearComprobanteCarteraFailure = (
    error: any,
): AppActions => ({
    type: CREAR_COMPROBANTE_CARTERA_FAILURE,
    error: error
});
export const PostActualizarMovimientoComprobanteFailure = (
    error: any,
): AppActions => ({
    type: ACTUALIZAR_MOVIMIENTO_COMPROBANTE_FAILURE,
    error: error
});
export const PostObtenerComprobanteCarteraFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_CARTERA_FAILURE,
    error: error
});
export const PostObtenerComprobantePorCodigoProcesoFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_PROCESO_FAILURE,
    error: error
});
export const PostGenerarComprobanteSevenPorProcesoFailure = (
    error: any,
): AppActions => ({
    type: GENERAR_COMPROBANTE_PROCESO_FAILURE,
    error: error
});
export const PostAnularMovimientoContableFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_PROCESO_FAILURE,
    error: error
});
export const PostObtenerMovimientosContableCarteraFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_DETALLES_COMPROBANTE_FAILURE,
    error: error
});
export const PostObtenerValorCdpFailure = (
    error: any,
): AppActions => ({
    type: OBTENER_COMPROBANTE_CREDITO_FAILURE,
    error: error
});

export const startGetProductoByIdentification =
    (identificationType: number, idNumber: string, idProducto: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {   
            const request = {
                identificacion: idNumber,
                tipoIdentificacion: identificationType,
                idProducto: idProducto && idProducto !== "" ? idProducto : null, // 💡 Esto es clave
            };
    
                const response = await api.ObtenerProductoByidentificacionRequest(request);
            if (!response?.isSuccess) {
                dispatch(ObtenerProductoByIdentificationFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(GetProductoByIdentificationSuccess(response.data));
                if (response.data == null) {
                    toast('Producto Afiliado no encontrado.');
                }
              
            }
        } catch (e) {
           
            dispatch(ObtenerProductoByIdentificationFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };
export const startGetProductosByIdentification =
    (identificationType: number, idNumber: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {   
            const request = {
                identificacion: idNumber,
                tipoIdentificacion: identificationType,
            };
    
                const response = await api.ObtenerProductosByidentificacionRequest(request);
            if (!response?.isSuccess) {
                dispatch(ObtenerProductoByIdentificationFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(GetProductoByIdentificationSuccess(response.data));
                if (response.data == null) {
                    toast('Producto Afiliado no encontrado.');
                }
              
            }
        } catch (e) {
           
            dispatch(ObtenerProductoByIdentificationFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };

    export const startAplicacionDePago =
    (props: InsertAplicacionPagoBody) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.aplicacionDePago(props);
            if (!response?.isSuccess) {
                dispatch(aplicacionDePagos(response.data));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(aplicacionDePagos(response.data));
                if (response.data == null) {
                    toast('Error al aplicar el pago.');
                }
            }
        } catch (e) {
           
            dispatch(ObtenerProductoByIdentificationFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };

export const UpdateProductos = (productos: ProductoDto): AppActions => ({
    type: UPDATE_PRODUCTO,
    producto: productos,
});

export const UpdateProductosSuccess = (productos: ProductoDto): AppActions => ({
    type: UPDATE_PRODUCTO_SUCCESS,
    producto: productos,
});

export const UpdateproductFailure = (error: any): AppActions => ({
    type: UPDATE_PRODUCTO_FAILURE,
    error: error,
});

export const startUpdateProduct =
    (product: ProductoDto, props: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateProductos(product));
        const response = await api.ActualizarProductoRequest(product);
        if (response?.isSuccess == null) {
            dispatch(UpdateproductFailure(product));
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(UpdateProductosSuccess(response.data));
            await NotificationService.genericAlert(
                'Actualizado con éxito',
                'Se realiz� con �xito la actualizaci�n.',
            );
        }
    };

export const startGetAllAplicacionPago =
(idCredito: string) => async (dispatch: Dispatch<AppActions>) => {

    try {
        const response = await api.getAllAplicacionPago(idCredito);
        if (response.isSuccess === false) {
            dispatch(GetAllAplicacionPagoFailure(response));
        } else {
            dispatch(GetAllAplicacionPago(response.data));
        }
    } catch (e) {
       
        dispatch(GetAllAplicacionPagoFailure('server-error'));
    }
};

export const startPostAplicacionMasivaPagos =
(aplicacionMasiva: procesoAplicacionMasivaPago, archivo: FormData) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const user = AuthService.getUserInfo();
        aplicacionMasiva.creadoPor = user.USR_ID;
      
        const response = await api.postAplicacionMasivaPagos(aplicacionMasiva,archivo);
        if (response.isSuccess === false) {
            dispatch(PostAplicacionMasivaPago(response.data));
            swal({
                text: response.returnMessage,
                icon: 'warning',
            });
        } else {
            swal({
                text: 'El proceso ha finalizado correctamente',
                icon: 'success',
            });
            dispatch(PostAplicacionMasivaPago(response.data));
        }
    } catch (e) {
        dispatch(GetAllAplicacionMasivaPagoFailure('server-error'));
        toast('Error del sistema', { type: toast.TYPE.ERROR });
    }
};

export const ClearDataAplicacionMasivaPagos = () => (dispatch: Dispatch<AppActions>) => {
    dispatch(AplicacionMasivaPagoClearDataAction());
}

export const startPostCierreDiarioCartera =
    (actualizacionDatos: boolean) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {   
            const response = await api.postCierreDiarioCartera(
                    actualizacionDatos
            );
            if (!response?.isSuccess) {
                dispatch(PostCierreDiarioCarteraFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostCierreDiarioCartera(response.data));
                if (response.data == null) {
                    toast('No existen registros.');
                }
            }
        } catch (e) {
           
            dispatch(PostCierreDiarioCarteraFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };

export const startPostCrearComprobanteCartera =
    (actualizacionDatos: any) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(PostCrearComprobanteCartera(actualizacionDatos))
        try {   
            const response = await api.postCrearComprobanteCartera(
                    actualizacionDatos
            );
            if (!response?.isSuccess) {
                dispatch(PostCrearComprobanteCarteraFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostCrearComprobanteCarteraSuccess(response.data));
                if (response.data == null) {
                    toast('No existen registros.');
                }
            }
        } catch (e) {
           
            dispatch(PostCrearComprobanteCarteraFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };

export const startPostActualizarMovimientoComprobante =
    (actualizacionDatos: any) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {   
            const response = await api.postActualizarMovimientoComprobante(
                    actualizacionDatos
            );
            if (!response?.isSuccess) {
                dispatch(PostActualizarMovimientoComprobanteFailure(response));
                toast(`Error: ${response.returnMessage}`,{ type: toast.TYPE.ERROR });
            } else {
                dispatch(PostActualizarMovimientoComprobante(response.data));
                    toast(`${response.returnMessage}` ,{ type: toast.TYPE.SUCCESS });
            }
        } catch (e) {
           
            dispatch(PostActualizarMovimientoComprobanteFailure(null));
            toast('Error: respuesta errada del servidor',{ type: toast.TYPE.ERROR });
        }
    };

export const resetComprobante = (): AppActions => ({
    type: RESET_COMPROBANTE,
});

export const startPostObtenerComprobanteCartera =
    (actualizacionDatos: boolean) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(PostObtenerComprobanteCartera(actualizacionDatos))
        try {   
            const response = await api.postObtenerComprobanteCartera(
                    actualizacionDatos
            );
            if (!response?.isSuccess) {
                dispatch(PostObtenerComprobanteCarteraFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostObtenerComprobanteCarteraSuccess(response.data));
                if (response.data == null) {
                    toast('No existen registros.');
                }
            }
        } catch (e) {
           
            dispatch(PostObtenerComprobanteCarteraFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };

    


export const startPostObtenerComprobantePorCodigoProceso =
    (actualizacionDatos: boolean) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(PostObtenerComprobantePorCodigoProceso(actualizacionDatos))
        try {   
            const response = await apiCartera.postObtenerComprobantePorCodigoProceso(actualizacionDatos);
            if (!response?.isSuccess) {
                dispatch(PostObtenerComprobantePorCodigoProcesoFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostObtenerComprobantePorCodigoProcesoSuccess(response.data));
                if (response.data == null) {
                    toast('No existen registros.');
                }
            }
        } catch (e) {
           
            dispatch(PostObtenerComprobantePorCodigoProcesoFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };

export const startPostObtenerMovimientosContableCartera =
    (actualizacionDatos: boolean) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(PostObtenerMovimientosContableCartera(actualizacionDatos))
        try {   
            const response = await apiCartera.postObtenerMovimientosContableCartera(actualizacionDatos);
            if (!response?.isSuccess) {
                dispatch(PostObtenerMovimientosContableCarteraFailure(response));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostObtenerMovimientosContableCarteraSuccess(response.data));
                if (response.data == null) {
                    toast('No existen registros.');
                }
            }
        } catch (e) {
           
            dispatch(PostObtenerMovimientosContableCarteraFailure(null));
            toast('Error: respuesta errada del servidor');
        }
    };



    // export const startPostGenerarComprobanteSevenPorProceso = 
    // (actualizacionDatos: any) => 
    // async (dispatch: Dispatch<AppActions>) => { 
    //     dispatch(PostGenerarComprobanteSevenPorProceso(actualizacionDatos));
        
    //     const response = await apiCartera.postGenerarComprobanteSevenPorProceso(actualizacionDatos);

    //     if (!response?.isSuccess) {
    //         toast('Error: respuesta errada del servidor', { type: toast.TYPE.ERROR });
    //         dispatch(PostGenerarComprobanteSevenPorProcesoFailure(response));
    //         return; // Salida temprana en caso de error
    //     } 
    //     const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
    //     if (retornoExitoso && retornoExitoso[1] === "0") {
    //         toast('Comprobante generado exitosamente', { type: toast.TYPE.SUCCESS });
    //         dispatch(PostGenerarComprobanteSevenPorProcesoSuccess(response.data));
    //     } else {
    //         const retornoError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
    //         const errorMessage = retornoError ? retornoError[1] : 'Error desconocido';
    //         dispatch(PostGenerarComprobanteSevenPorProcesoFailure(null));
    //         toast(`Error: ${errorMessage}`, { type: toast.TYPE.ERROR });
    //     }
    // };

    export const startPostGenerarComprobanteSevenPorProceso = 
    (actualizacionDatos: any) => 
    async (dispatch: Dispatch<AppActions>) => { 
        try {
            
            dispatch(PostGenerarComprobanteSevenPorProceso(actualizacionDatos));
            
            const response = await apiCartera.postGenerarComprobanteSevenPorProceso(actualizacionDatos);
    
            if (!response?.isSuccess) {
                const retornoError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                const errorMessage = retornoError ? retornoError[1] : response.returnMessage;
                toast(`Error: ${errorMessage}`, { type: toast.TYPE.ERROR });
                dispatch(PostGenerarComprobanteSevenPorProcesoFailure(response));
                return; // Salida temprana en caso de error
            } 
    
            const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
            if (retornoExitoso && retornoExitoso[1] === "0") {
                toast('Comprobante generado exitosamente', { type: toast.TYPE.SUCCESS });
                dispatch(PostGenerarComprobanteSevenPorProcesoSuccess(response.data));
            } else {
                const retornoError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                const errorMessage = retornoError ? retornoError[1] : 'Error desconocido';
                dispatch(PostGenerarComprobanteSevenPorProcesoFailure(null));
                toast(`Error: ${errorMessage}`, { type: toast.TYPE.ERROR });
            }
        } catch (e) {
            const errorMessage = (e as Error).message || 'Error inesperado';
            toast(`Error al consultar información básica: ${errorMessage}`, { type: toast.TYPE.ERROR });
            dispatch(PostGenerarComprobanteSevenPorProcesoFailure(null));
        }
    };
    
export const startPostObtenerValorCdp  =
    (actualizacionDatos: boolean) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(PostObtenerValorCdp(actualizacionDatos))
        try {   
            const response = await apicredito.postObtenerValorCdp(
                    actualizacionDatos
            );
            if (!response?.isSuccess) {
                
                dispatch(PostObtenerValorCdpFailure(response));
                toast('Error: respuesta errada del servidor', { type: toast.TYPE.ERROR });
            } else {
                dispatch(PostObtenerValorCdpSuccess(response.data));
                if (response.data == null) {
                    toast('Consulta exitosa.', { type: toast.TYPE.SUCCESS });
                }
            }
        } catch (e) {
           
            dispatch(PostObtenerComprobantePorCodigoProcesoFailure(null));
            toast('Error: respuesta errada del servidor', { type: toast.TYPE.ERROR });

        }
    };


    // const anularCuotaCartera = (rowData: any) => {
    //     const request = {
    //         numeroCredito: Number(producto.numeroCredito),
    //         cpgId: String(rowData.idAplicacionPago)
    //     }
    //     api
    //     .postAnularCuotaCartera(request)


// export const startPostAnularMovimientoContable  =
//     (request: boolean) =>
//     async (dispatch: Dispatch<AppActions>) => {
//       //  dispatch(PostAnularMovimientoContable(request))
//         try { 
//             debugger  
//             const response = await api.postAnularCuotaCartera(
//                 request
//             );
//             if (!response?.isSuccess) {
//             //    dispatch(PostAnularMovimientoContableFailure(response));
//                 toast('Error: respuesta errada del servidor', { type: toast.TYPE.ERROR });
                
//             } else {
//                 const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
//                 if(retornoExitoso[1] === "0"){
//                     toast('Comprobante Anulado con éxito',{ type: toast.TYPE.SUCCESS });
//                 }else{
//                     const retornoError = response.returnMessage.match(/<TXTERROR>\s*([\s\S]*?)\s*<\/TXTERROR>/);
//                     toast( `Error: ${retornoError[1]}`, { type: toast.TYPE.ERROR });
//                 }
//             }
//         } catch (e) {
           
//            // dispatch(PostAnularMovimientoContableFailure(null));
//             toast('Error: respuesta errada del servidor', { type: toast.TYPE.ERROR });

//         }
//     };

export const startPostAnularMovimientoContable = 
(actualizacionDatos: any) => 
async (dispatch: Dispatch<AppActions>) => { 
    try {
        dispatch(PostGenerarComprobanteSevenPorProceso(actualizacionDatos));
        
        const response = await api.postAnularCuotaCartera(actualizacionDatos);

        if (!response?.isSuccess) {
            const retornoError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
            const errorMessage = retornoError ? retornoError[1] : 'Error desconocido';
            toast(`Error: ${errorMessage}`, { type: toast.TYPE.ERROR });
            dispatch(PostGenerarComprobanteSevenPorProcesoFailure(response));
            return; // Salida temprana en caso de error
        } 
        
        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
        if (retornoExitoso && retornoExitoso[1] === "0") {
            toast('Comprobante generado exitosamente', { type: toast.TYPE.SUCCESS });
            dispatch(PostGenerarComprobanteSevenPorProcesoSuccess(response.data));
        } else {
            const retornoError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
            const errorMessage = retornoError ? retornoError[1] : 'Error desconocido';
            dispatch(PostGenerarComprobanteSevenPorProcesoFailure(null));
            toast(`Error: ${errorMessage}`, { type: toast.TYPE.ERROR });
        }
    } catch (error) {
        // Manejo del error general
        console.error('Error al anular el movimiento contable:', error);
        toast('Error inesperado al procesar la solicitud', { type: toast.TYPE.ERROR });
        dispatch(PostGenerarComprobanteSevenPorProcesoFailure(null));
    }
};
