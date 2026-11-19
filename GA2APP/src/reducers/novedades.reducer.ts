
// import { AddressActionTypes } from '../types/address.actions.type';
// import {
//     DireccionAbecedario,
//     DireccionBis,
//     DireccionCardinal,
//     DireccionTipo,
// } from '../types/address.type';
// import { ON_ERROR } from '../types/catalog.actions.type';
// import { NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS } from '../types/novedades.actions.type';

// export interface NovedadesState {
//     direccionesTipos: DireccionTipo[];
//     direccionesCardinales: DireccionCardinal[];
//     direccionesAbecedario: DireccionAbecedario[];
//     direccionesBis: DireccionBis[];
//     loading: boolean;
//     error: any;
// }

// const InitialState: NovedadesState = {
//     direccionesTipos: [],
//     direccionesCardinales: [],
//     direccionesAbecedario: [],
//     direccionesBis: [],
//     loading: false,
//     error: null,
// };

// export const NovedadesReducer = (
//     state: NovedadesState = InitialState,
//     action: AddressActionTypes,
// ): NovedadesState => {
//     switch (action.type) {
//         case NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS:
//             return {
//                 ...state,
//                 loading: true,
//             };
      
//         case ON_ERROR:
//             return {
//                 ...state,
//                 loading: false,
//                 error: action.error,
//             };
//         default:
//             return state;
//     }
// };
