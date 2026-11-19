import { ClientActionTypes,GetPermisoActualizacion } from './client.actions.types';
import { CatalogActionTypes } from './catalog.actions.type';
import { LoginActionTypes } from './login.actions.type';
import { RolActionTypes } from './roles.actions.type';
import { UserActionTypes } from './user.actions.type';
import { ParametrizationActionTypes } from './parametrization.action.type';
import { ParametrizationLegalActionTypes } from './parametrizationLegal.action.type';
import { AddressActionTypes } from './address.actions.type';
import { ProductoActionTypes } from './producto.action.type';
import { CargueNominaActionTypes } from './cargue-nomina.action.type';
import { StateActionTypes } from './state.action.type';
import { DataActionTypes } from './tablaParametro.actions.type';
import { CreditActionsType } from './credit.actions.type';
import { LockActionTypes } from './lock.action.type';
import { NotificationActionTypes } from './notification.action.type';
import { MessageActionTypes } from './message.action.type';
import { ScheduleNoveltiesActionsType } from './schedulenovelties.actions.type';
import { SeveranceActionsType } from './severance.actions.type';
import { SolicitudActionsType } from './solicitud.actions.type';
import { CronogramaActionsType } from './cronograma.actions.type';
import { SimuladorActionTypes } from './simulador.action.type';
import { SimuladorGeneralActionTypes } from './simuladorGeneral.action.type';
import { GrupoUsersActionsTypes } from './grupoUser.actions.type'
import { MatriculasInmobiliariasActionTypes } from './realStateRegistrations.action.type';
import { TIPOAFILIACIONDTOActionTypes, } from './typeAffiliate.action.type';
import { EntidadesEducativasActionType } from './educational.entities.actions.type';
import { CourtsType } from './courts.actions.type';
import { EmbargosType } from './embargos.actions.type';
import { AuditoriaActionsTypes } from './auditoria.actions.type';
import { CierreMensualActionTypes } from './cierreMensual.actions.types';
import { DesembargosType } from './desembargos.actions.type';
import { NovedadesType } from './novedades.actions.type';
import { HistorialCambios } from './educational.entities.type';

export type AppActions =
    | UserActionTypes
    | LoginActionTypes
    | RolActionTypes
    | ClientActionTypes
    | CatalogActionTypes
    | ParametrizationActionTypes
    | ParametrizationLegalActionTypes
    | ProductoActionTypes
    | AddressActionTypes
    | CargueNominaActionTypes
    | StateActionTypes
    | CreditActionsType
    | SeveranceActionsType
    | SolicitudActionsType
    | LockActionTypes
    | NotificationActionTypes
    | ScheduleNoveltiesActionsType
    | MessageActionTypes
    | CronogramaActionsType
    | SimuladorActionTypes
    | SimuladorGeneralActionTypes
    | GrupoUsersActionsTypes
    | MatriculasInmobiliariasActionTypes
    | TIPOAFILIACIONDTOActionTypes
    | EntidadesEducativasActionType
    | CourtsType
    | EmbargosType
    | AuditoriaActionsTypes
    | CierreMensualActionTypes
    | DesembargosType
    | DataActionTypes
    | GetPermisoActualizacion
    | NovedadesType;
