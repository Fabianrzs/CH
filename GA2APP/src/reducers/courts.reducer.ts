import { JuzgadoDto } from "../types/court.model.type";
import { DELETE_COURT_SUCCESS, CourtsType, 
         ON_ERROR,
         POST_ALL_COURTS_BY_CITY_SUCCESS,
         POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS, 
         POST_INSERT_COURT_SUCCESS,
         POST_UPDATE_COURT_SUCCESS} from "../types/courts.actions.type";


export interface CourtsState {
    courts: JuzgadoDto [];
    court: JuzgadoDto | null;
    loading: boolean;
    error: any;
}

const CourtsStateInitial: CourtsState = {
    courts: [],
    court: null,
    loading: false,
    error: null
}

export const CourtsReducer = (
    state: CourtsState = CourtsStateInitial,
    action: CourtsType
): CourtsState => {
    switch (action.type){
        case POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS:
            return {
                ...state,
                loading: false,
                courts: action.courts,
            };
        break;
        case POST_ALL_COURTS_BY_CITY_SUCCESS:
            return {
                ...state,
                loading: false,
                courts: action.courts,
            };
        break;
        case POST_INSERT_COURT_SUCCESS:
            return {
                ...state,
                loading: false,
                court: action.court,
            };
        break;
        case POST_UPDATE_COURT_SUCCESS:
            return {
                ...state,
                loading: false,
                court: action.court,
            };
        case DELETE_COURT_SUCCESS:
            return {
                ...state,
                loading: false,
                court: action.court,
            };
        break;
        case ON_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        break;
        default:
            return state;
    }
    return state;
}