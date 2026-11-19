import {
	ArchivosEntidadesEducativas,
	BloqueosEntidadesEducativas,
	CuentaEntidadesEducativas,
	EntidadesEducativasAgregar,
	EntidadesEducativasFiltro,
	Fidecomiso,
	HistorialCambios,
	ProgramaEducativo,
	ProgramasEducativos,
	SedeEntidadEducativa,
	TramiteEntity
} from "./educational.entities.type";

// Especificacion de accion
export const GET_EDUCATIONAL_ENTITIES_FILTER = 'GET_EDUCATIONAL_ENTITIES_FILTER';
export const GET_EDUCATIONAL_ENTITIES_FILTER_SUCCESS = 'GET_EDUCATIONAL_ENTITIES_FILTER_SUCCESS';
export const GET_EDUCATIONAL_ENTITIES_FILTER_FAILURE = 'GET_EDUCATIONAL_ENTITIES_FILTER_FAILURE';

export const UPDATE_EDUCATIONAL_ENTITIES = 'UPDATE_EDUCATIONAL_ENTITIES';
export const UPDATE_EDUCATIONAL_ENTITIES_SUCCESS = 'UPDATE_EDUCATIONAL_ENTITIES_SUCCESS';
export const UPDATE_EDUCATIONAL_ENTITIES_FAILURE = 'UPDATE_EDUCATIONAL_ENTITIES_FAILURE';

export const GET_EDUCATIONAL_ENTITIES_CAMPUS = 'GET_EDUCATIONAL_ENTITIES_CAMPUS';
export const GET_EDUCATIONAL_ENTITIES_CAMPUS_SUCCESS = 'GET_EDUCATIONAL_ENTITIES_CAMPUS_SUCCESS';
export const GET_EDUCATIONAL_ENTITIES_CAMPUS_FAILURE = 'GET_EDUCATIONAL_ENTITIES_CAMPUS_FAILURE';

export const GET_EDUCATIONAL_PROGRAMS = 'GET_EDUCATIONAL_PROGRAMS';
export const GET_EDUCATIONAL_PROGRAMS_SUCCESS = 'GET_EDUCATIONAL_PROGRAMS_SUCCESS';
export const GET_EDUCATIONAL_PROGRAMS_FAILURE = 'GET_EDUCATIONAL_PROGRAMS_FAILURE';

export const GET_BLOCKS = 'GET_BLOCKS';
export const GET_BLOCKS_SUCCESS = 'GET_BLOCKS_SUCCESS';
export const GET_BLOCKS_FAILURE = 'GET_BLOCKS_FAILURE';

export const LOCK_ENTIDADES = 'LOCK_ENTIDADES';
export const LOCK_ENTIDADES_SUCCESS = 'LOCK_ENTIDADES_SUCCESS';
export const LOCK_ENTIDADES_FAILURE = 'LOCK_ENTIDADES_FAILURE';

export const GET_HISTORY = 'GET_HISTORY';
export const GET_HISTORY_SUCCESS = 'GET_HISTORY_SUCCESS';
export const GET_HISTORY_FAILURE = 'GET_HISTORY_FAILURE';

export const GET_ACCOUNTS_EDUCATIONAL_ENTITIES = 'GET_ACCOUNTS_EDUCATIONAL_ENTITIES';
export const GET_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS = 'GET_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
export const GET_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE = 'GET_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
export const ADD_ACCOUNTS_EDUCATIONAL_ENTITIES = 'ADD_ACCOUNTS_EDUCATIONAL_ENTITIES';
export const ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS = 'ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
export const ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE = 'ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
export const UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES = 'UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES';
export const UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS = 'UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
export const UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE = 'UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
export const DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES = 'DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES';
export const DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS = 'DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
export const DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE = 'DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';

export const ADD_EDUCATIONAL_ENTITIES = 'ADD_EDUCATIONAL_ENTITIES';
export const ADD_EDUCATIONAL_ENTITIES_SUCCESS = 'ADD_EDUCATIONAL_ENTITIES_SUCCESS';
export const ADD_EDUCATIONAL_ENTITIES_FAILURE = 'ADD_EDUCATIONAL_ENTITIES_FAILURE';
export const DELETE_EDUCATIONAL_ENTITIES = 'DELETE_EDUCATIONAL_ENTITIES';
export const DELETE_EDUCATIONAL_ENTITIES_SUCCESS = 'DELETE_EDUCATIONAL_ENTITIES_SUCCESS';
export const DELETE_EDUCATIONAL_ENTITIES_FAILURE = 'DELETE_EDUCATIONAL_ENTITIES_FAILURE';
export const GET_REPORT_EDUCATIONAL_ENTITIES_SUCCESS = 'GET_REPORT_EDUCATIONAL_ENTITIES_SUCCESS';
export const GET_REPORT_EDUCATIONAL_ENTITIES_FAILURE = 'GET_REPORT_EDUCATIONAL_ENTITIES_FAILURE';
export const GET_REPORT_EDUCATIONAL_ENTITIES = 'GET_REPORT_EDUCATIONAL_ENTITIES';

export const GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD = 'GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD';
export const ADD_PROGRAMS = 'ADD_PROGRAMS';
export const ADD_PROGRAMS_SUCCESS = 'ADD_PROGRAMS_SUCCESS';
export const ADD_PROGRAMS_FAILURE = 'ADD_PROGRAMS_FAILURE';
export const DELETE_PROGRAMA_SUCCESS = 'DELETE_PROGRAMA_SUCCESS';
export const UPDATE_PROGRAMA_SUCCESS = 'UPDATE_PROGRAMA_SUCCESS';

export const FIDECOMISO_SUCCESS = 'FIDECOMISO_SUCCESS';
export const UPDATE_FIDECOMISO_SUCCESS = 'UPDATE_FIDECOMISO_SUCCESS';
export const FIDEICOMISO_FAILURE = 'FIDEICOMISO_FAILURE';

export const ADD_CAMPUS = 'ADD_CAMPUS';
export const ADD_CAMPUS_SUCCESS = 'ADD_CAMPUS_SUCCESS';
export const ADD_CAMPUS_FAILURE = 'ADD_CAMPUS_FAILURE';
export const UPDATE_CAMPUS = 'UPDATE_CAMPUS';
export const UPDATE_CAMPUS_SUCCESS = 'UPDATE_CAMPUS_SUCCESS';
export const UPDATE_CAMPUS_FAILURE = 'UPDATE_CAMPUS_FAILURE';
export const DELETE_CAMPUS = 'DELETE_CAMPUS';
export const DELETE_CAMPUS_SUCCESS = 'DELETE_CAMPUS_SUCCESS';
export const DELETE_CAMPUS_FAILURE = 'DELETE_CAMPUS_FAILURE';

export const GET_EDUCATIONAL_ENTITIES_DETAIL = 'GET_EDUCATIONAL_ENTITIES_DETAIL';
export const GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS = 'GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS';
export const GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE = 'GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE';

export const GET_FILES_EDUCATIONAL_ENTITIES = 'GET_FILES_EDUCATIONAL_ENTITIES';
export const GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS = 'GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS';
export const GET_FILES_EDUCATIONAL_ENTITIES_FAILURE = 'GET_FILES_EDUCATIONAL_ENTITIES_FAILURE';

export const GET_TRAMITE_ENTITIES = 'GET_TRAMITE_ENTITIES';
export const GET_TRAMITE_ENTITIES_FAILURE = 'GET_TRAMITE_ENTITIES_FAILURE';
export const GET_TRAMITE_ENTITIES_SUCCESS = 'GET_TRAMITE_ENTITIES_SUCCESS';

export const SHOW_ROW_DETALLE_SUCCESS = 'SHOW_ROW_DETALLE_SUCCESS';


export interface GetEducationalEntitiesFilter {
	type: 'GET_EDUCATIONAL_ENTITIES_FILTER';
}

export interface ShowDetalleEntidadEducativa {
	type: 'SHOW_ROW_DETALLE_SUCCESS';
	dataRowDetalleEntidad: EntidadesEducativasFiltro;
}

export interface GetEducationalEntitiesFilterSuccess {
	type: 'GET_EDUCATIONAL_ENTITIES_FILTER_SUCCESS';
	EntidadesEducativas: EntidadesEducativasFiltro[];
}

export interface GetEducationalEntitiesFilterFailure {
	type: 'GET_EDUCATIONAL_ENTITIES_FILTER_FAILURE';
	EntidadesEducativas: EntidadesEducativasFiltro[] | null;
}

export interface UpdateEducationalEntities {
	type: 'UPDATE_EDUCATIONAL_ENTITIES';
}

export interface UpdateEducationalEntitiesSuccess {
	type: 'UPDATE_EDUCATIONAL_ENTITIES_SUCCESS';
}

export interface UpdateEducationalEntitiesFailure {
	type: 'UPDATE_EDUCATIONAL_ENTITIES_FAILURE';
}

export interface GetSedesEntidadEducativa {
	type: 'GET_EDUCATIONAL_ENTITIES_CAMPUS';
}

export interface GetSedesEntidadEducativaSuccess {
	type: 'GET_EDUCATIONAL_ENTITIES_CAMPUS_SUCCESS';
	sedesEntidadEducativa: SedeEntidadEducativa[];
}

export interface GetSedesEntidadEducativaFailure {
	type: 'GET_EDUCATIONAL_ENTITIES_CAMPUS_FAILURE';
	SedesEntidadEducativa: SedeEntidadEducativa[] | null;
}

export interface GetProgramasEducativos {
	type: 'GET_EDUCATIONAL_PROGRAMS';
}

export interface GetProgramasEducativosSuccess {
	type: 'GET_EDUCATIONAL_PROGRAMS_SUCCESS';
	programasEducativos: ProgramasEducativos[];
}

export interface GetProgramasEducativosFailure {
	type: 'GET_EDUCATIONAL_PROGRAMS_FAILURE';
	programasEducativos: ProgramasEducativos[] | null;
}

export interface GetBloqueos {
	type: 'GET_BLOCKS';
}

export interface GetBloqueosSuccess {
	type: 'GET_BLOCKS_SUCCESS';
	bloqueos: BloqueosEntidadesEducativas[];
}

export interface GetBloqueosFailure {
	type: 'GET_BLOCKS_FAILURE';
	bloqueos: BloqueosEntidadesEducativas[] | null;
}

export interface LockEntidades {
	type: 'LOCK_ENTIDADES';
}

export interface LockEntidadesSuccess {
	type: 'LOCK_ENTIDADES_SUCCESS';
	bloqueo: BloqueosEntidadesEducativas
}

export interface LockEntidadesFailure {
	type: 'LOCK_ENTIDADES_FAILURE';
	bloqueo: BloqueosEntidadesEducativas
}

export interface GetCuentas {
	type: 'GET_ACCOUNTS_EDUCATIONAL_ENTITIES';
}

export interface GetCuentasSuccess {
	type: 'GET_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
	cuentas: CuentaEntidadesEducativas[];
}

export interface GetCuentasFailure {
	type: 'GET_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
	cuentas: CuentaEntidadesEducativas[] | null;
}
export interface GetTramiteSuccess {
	type: 'GET_TRAMITE_ENTITIES_SUCCESS';
	tramite: TramiteEntity[];
}
export interface GetTramite {
	type: 'GET_TRAMITE_ENTITIES';
}


export interface GetTramiteFailure {
	type: 'GET_TRAMITE_ENTITIES_FAILURE';
	tramite: TramiteEntity[] | null;
}

export interface AddCuentas {
	type: 'ADD_ACCOUNTS_EDUCATIONAL_ENTITIES';
}

export interface AddCuentasSuccess {
	type: 'ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
	cuenta: CuentaEntidadesEducativas;
}

export interface AddCuentasFailure {
	type: 'ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
}

export interface UpdateCuenta {
	type: 'UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES';
}

export interface UpdateCuentaSuccess {
	type: 'UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
	cuenta: CuentaEntidadesEducativas;
}

export interface UpdateCuentaFailure {
	type: 'UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
}

export interface DeleteCuentas {
	type: 'DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES';
}

export interface DeleteCuentasSuccess {
	type: 'DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS';
	idCuenta: string;
	cuenta: CuentaEntidadesEducativas;
}

export interface DeleteCuentasFailure {
	type: 'DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE';
}

export interface AddEducationalEntities {
	type: 'ADD_EDUCATIONAL_ENTITIES';
}

export interface AddEducationalEntitiesSuccess {
	type: 'ADD_EDUCATIONAL_ENTITIES_SUCCESS';
	agregarEntidadEducativa: EntidadesEducativasAgregar;
}

export interface AddEducationalEntitiesFailure {
	type: 'ADD_EDUCATIONAL_ENTITIES_FAILURE';
	agregarEntidadEducativa: EntidadesEducativasAgregar | null;
}

export interface DeleteEducationalEntities {
	type: 'DELETE_EDUCATIONAL_ENTITIES';
}

export interface DeleteEducationalEntitiesSuccess {
	type: 'DELETE_EDUCATIONAL_ENTITIES_SUCCESS';
	idEntidad: string;
	entidadEducativa: EntidadesEducativasFiltro;
}

export interface DeleteEducationalEntitiesFailure {
	type: 'DELETE_EDUCATIONAL_ENTITIES_FAILURE';
}

export interface GetReportEducationEntitiesSuccess {
	type: 'GET_REPORT_EDUCATIONAL_ENTITIES_SUCCESS';
}
export interface GetReportEducationEntities {
	type: 'GET_REPORT_EDUCATIONAL_ENTITIES';
}
export interface GetReportEducationEntitiesFailure {
	type: 'GET_REPORT_EDUCATIONAL_ENTITIES_FAILURE';
}
export interface AddPrograms {
	type: 'ADD_PROGRAMS';
}

export interface AddProgramsSuccess {
	type: 'ADD_PROGRAMS_SUCCESS';
	programs: ProgramasEducativos;
}

export interface AddProgramsFailure {
	type: 'ADD_PROGRAMS_FAILURE';
	programs: ProgramasEducativos | null;
}

export interface AddCampus {
	type: 'ADD_CAMPUS';
}

export interface AddCampusSuccess {
	type: 'ADD_CAMPUS_SUCCESS';
	sede: SedeEntidadEducativa;
}

export interface AddCampusFailure {
	type: 'ADD_CAMPUS_FAILURE';
	sede: SedeEntidadEducativa | null;
}

export interface UpdateCampus {
	type: 'UPDATE_CAMPUS';
}

export interface UpdateCampusSuccess {
	type: 'UPDATE_CAMPUS_SUCCESS';
	sede: SedeEntidadEducativa;
}

export interface UpdateCampusFailure {
	type: 'UPDATE_CAMPUS_FAILURE';
}

export interface DeleteCampus {
	type: 'DELETE_CAMPUS';
}

export interface DeleteCampusSuccess {
	type: 'DELETE_CAMPUS_SUCCESS';
	idSede: string;
}

export interface DeleteCampusFailure {
	type: 'DELETE_CAMPUS_FAILURE';
}

export interface GetEducationalEntitiesDetail {
	type: 'GET_EDUCATIONAL_ENTITIES_DETAIL';
}

export interface GetEducationalEntitiesDetailSuccess {
	type: 'GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS';
	detalleEntidadEducativa: EntidadesEducativasFiltro;
}

export interface GetEducationalEntitiesDetailFailure {
	type: 'GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE';
	detalleEntidadEducativa: EntidadesEducativasFiltro | null;
}

export interface GetAllProgramaPorEntidad {
	type: 'GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD';
	programaPorEntidad: ProgramasEducativos[];
}

export interface DeleteProgramaEducativo {
	type: 'DELETE_PROGRAMA_SUCCESS';
	idProgramaEducativo: string;
}

export interface UpdateProgramaEducativo {
	type: 'UPDATE_PROGRAMA_SUCCESS';
	programaEducativo: ProgramaEducativo;
}

export interface GetFideicomisos {
	type: 'FIDECOMISO_SUCCESS';
}

export interface UpdateFideicomiso {
	type: 'UPDATE_FIDECOMISO_SUCCESS';
	fidecomiso: Fidecomiso[];
}

export interface GetFideicomisosFailure {
	type: 'FIDEICOMISO_FAILURE';
}

export interface GetFilesEducationalEntities {
	type: 'GET_FILES_EDUCATIONAL_ENTITIES';
}

export interface GetFilesEducationalEntitiesSuccess {
	type: 'GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS';
	archivos: ArchivosEntidadesEducativas[];
}

export interface GetFilesEducationalEntitiesFailure {
	type: 'GET_FILES_EDUCATIONAL_ENTITIES_FAILURE';
}

export interface GetHistory {
	type: 'GET_HISTORY';
}

export interface GetHistorySuccess {
	type: 'GET_HISTORY_SUCCESS';
	historialCambios: HistorialCambios[];
}

export interface GetHistoryFailure {
	type: 'GET_HISTORY_FAILURE';
}

export type EntidadesEducativasActionType =
	| GetEducationalEntitiesFilter
	| GetEducationalEntitiesFilterSuccess
	| GetEducationalEntitiesFilterFailure
	| UpdateEducationalEntities
	| UpdateEducationalEntitiesSuccess
	| UpdateEducationalEntitiesFailure
	| GetEducationalEntitiesFilterSuccess
	| GetEducationalEntitiesFilterFailure
	| GetSedesEntidadEducativa
	| GetSedesEntidadEducativaSuccess
	| GetSedesEntidadEducativaFailure
	| GetProgramasEducativos
	| GetProgramasEducativosSuccess
	| GetProgramasEducativosFailure
	| GetBloqueos
	| GetBloqueosSuccess
	| GetBloqueosFailure
	| LockEntidades
	| LockEntidadesSuccess
	| LockEntidadesFailure
	| GetCuentas
	| GetCuentasSuccess
	| GetCuentasFailure
	| AddCuentas
	| AddCuentasSuccess
	| AddCuentasFailure
	| UpdateCuenta
	| UpdateCuentaSuccess
	| UpdateCuentaFailure
	| DeleteCuentas
	| DeleteCuentasSuccess
	| DeleteCuentasFailure
	| AddEducationalEntities
	| AddEducationalEntitiesSuccess
	| AddEducationalEntitiesFailure
	| ShowDetalleEntidadEducativa
	| AddPrograms
	| AddProgramsSuccess
	| AddProgramsFailure
	| AddCampus
	| AddCampusSuccess
	| AddCampusFailure
	| UpdateCampus
	| UpdateCampusSuccess
	| UpdateCampusFailure
	| DeleteCampus
	| DeleteCampusSuccess
	| DeleteCampusFailure
	| GetEducationalEntitiesDetail
	| GetEducationalEntitiesDetailSuccess
	| GetEducationalEntitiesDetailFailure
	| DeleteEducationalEntities
	| DeleteEducationalEntitiesSuccess
	| DeleteEducationalEntitiesFailure
	| GetAllProgramaPorEntidad
	| DeleteProgramaEducativo
	| UpdateProgramaEducativo
	| GetFilesEducationalEntities
	| GetFilesEducationalEntitiesSuccess
	| GetFilesEducationalEntitiesFailure
	| GetReportEducationEntities
	| GetReportEducationEntitiesSuccess
	| GetReportEducationEntitiesFailure
	| GetTramiteSuccess
	| GetTramiteFailure
	| GetTramite
	| GetHistory
	| GetHistorySuccess
	| GetHistoryFailure
	| GetFideicomisos
	| UpdateFideicomiso
	| GetFideicomisosFailure;