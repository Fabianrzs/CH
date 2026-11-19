import { combineReducers } from 'redux';
import { ClientReducer } from '../reducers/client.reducer';
import { CatalogReducer } from '../reducers/catalog.reducer';
import { LoginReducer } from '../reducers/login.reducer';
import { RolReducer } from '../reducers/rol.reducer';
import { UserReducer } from '../reducers/user.reducer';
import { ParametrizationReducer } from '../reducers/parametrization.reducer';
import { ParametrizationLegalReducer } from '../reducers/parametrizationLegal.reducer';
import { StateReducer } from '../reducers/state.reducer';
import { LockReducer } from '../reducers/lock.reducer';
import { AddressReducer } from '../reducers/address.reducer';
import { ProductoReducer } from '../reducers/producto.reducer';
import { SimuladorReducer } from '../reducers/simulador.reducer';
import { SimuladorGeneralReducer } from '../reducers/simuladorGeneral.reducer';
import { CargueNominaReducer } from '../reducers/cargue-nomina.reducer';
import CreditReducer from '../reducers/credit.reducer';
import { notificationReducer } from '../reducers/notification.reducer';
import { MessageReducer } from '../reducers/message.reducer';
import SeveranceReducer from '../reducers/severance.reducer';
import SolicitudReducer from '../reducers/solicitud.reducer';
import { ScheduleReducer } from '../reducers/schedule.reducer';
import { GrupoUsuariosReducer } from  '../reducers/grupousuarios.recducer';
import { GrupoOpcionesReducer } from '../reducers/grupo-opciones.reducer';
import { RealStateRegistrationsReducer } from '../reducers/realStateRegistrations.reducer';
import { EducationalEntitiesReducer } from '../reducers/educationalEntities.reducer';
import { CourtsReducer } from '../reducers/courts.reducer';
import { AffiliateExtraordinaryReducer } from '../reducers/affiliateExtraordinary.reducer';
import { EmbargosReducer } from '../reducers/embargos.reducer';
import { AuditoriaReducer } from '../reducers/auditoria.reducer';
import { CierreMensualReducer} from '../reducers/cierreMensual.reducer';
import { DesembargosReducer } from '../reducers/desembargos.reducer';
import { DataReducer } from '../reducers/tablaParametros.reducer';


export const rootReducer = combineReducers({
    users: UserReducer,
    login: LoginReducer,
    rol: RolReducer,
    client: ClientReducer,
    catalog: CatalogReducer,
    parametrization: ParametrizationReducer,
    parametrizationLegal: ParametrizationLegalReducer,
    state: StateReducer,
    data:DataReducer,
    lock: LockReducer,
    notification: notificationReducer,
    address: AddressReducer,
    producto: ProductoReducer,
    simulador: SimuladorReducer,
    simuladorGeneral: SimuladorGeneralReducer,
    cargueNomina: CargueNominaReducer,
    credit: CreditReducer,
    severance: SeveranceReducer,
    message: MessageReducer,
    schedule: ScheduleReducer,
    solicitud: SolicitudReducer,
    grupoUsuarios:GrupoUsuariosReducer,
    grupoOpciones:GrupoOpcionesReducer,
    realStateRegistrations: RealStateRegistrationsReducer,
    informacionBasica:RealStateRegistrationsReducer,
    educationalEntities: EducationalEntitiesReducer,
    courts: CourtsReducer,
    affiliateExtraordinary: AffiliateExtraordinaryReducer,
    embargo: EmbargosReducer,
    desembargo: DesembargosReducer,
    auditoria:AuditoriaReducer,
    cierreMensual:CierreMensualReducer

});

export type AppState = ReturnType<typeof rootReducer>;
