import { Dispatch } from 'redux';
import { AppActions } from '../types/appactions.type';
import {
    LOGIN_USER,
    LOGIN_USER_SUCCESS,
    LOGIN_USER_FAILURE,
} from '../types/login.actions.type';
import { Login } from '../types/login.type';
import Api from '../services/api.core.service';
import history from '../utils/history.router';
import StorageService from '../services/storage.service';
import NotificationService from '../services/notification.service';
import { errorInternoServidor, usuarioNoExiste } from '../pages/login/constants/login.constants';
import {
    NotificationCardModel,
    NotificationCardModelEnum,
} from '../models/requests/notification-cards-model';
import { GrupoOpcionesModel } from '../types/grupo-opciones.model.type';
import { Auditoria } from '../types/Auditoria.model.type';
import AuditoriaApi from '../services/api-auditorias.servise';


const api = Api();

export const LoginUser = (login: Login): AppActions => ({
    type: LOGIN_USER,
    login,
});


export const LoginUserSuccess = (opciones:GrupoOpcionesModel[],claims: string ): AppActions => ({
    type: LOGIN_USER_SUCCESS,
    opciones,
    claims,

});

export const LoginUserFailure = (error: any): AppActions => ({
    type: LOGIN_USER_FAILURE,
    error,
});

		
export const startLoginUser =
(login: Login) =>
async (dispatch: Dispatch<AppActions>): Promise<void> => {
    dispatch(LoginUser(login));
    const nav =(window.navigator as any);
    const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
    const auditoriaApi = AuditoriaApi();
    const requestAuditoria={
        navegador:usrAg,
        sistemaOperativo:nav.userAgentData.platform,
        nombreUsuario:login.userName,         
        ip:'192.168.217.19',
        modulo:'Login',
        hostname:document.location.host
    } as Auditoria
    api.login(login).then(async (response)=> {
        if (!response.isAxiosError) {
            if (!response.data) {
               await NotificationService.showNotification({
                    message: response.returnMessage ?? usuarioNoExiste,
                    image: null,
                    type: NotificationCardModelEnum.error,
                } as NotificationCardModel);
                dispatch(LoginUserFailure('server-error'));
                const msg= response.returnMessage ?? usuarioNoExiste
                    requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion=`Inicio de sesión:
                         \n Usuario:  ${login.userName}                        
                         \n Validación: ${msg}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
            } else {
                dispatch(LoginUserSuccess(response.data.opciones,response.data.token));
                StorageService.setSessionToken(response.data.token);
                history.push('/Inicio');
                requestAuditoria.tipoAccion='INICIO DE SESIÓN';
                requestAuditoria.idTipoAccion=1;
                requestAuditoria.descripcion=`Inicio de sesión:
                     \n Usuario:  ${login.userName}
                     `
                await auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        } else {
            dispatch(LoginUserFailure('server-error'));
            const error=response?.returnMessage;
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=5;
            requestAuditoria.descripcion=`Inicio de sesión:
                 \n Usuario:  ${login.userName}                  
                 \n Error:  ${error}
                 \n returnApiMessage: ${response?.returnMessage}`
            await auditoriaApi.InsertAuditoria(requestAuditoria);
            await NotificationService.showNotification({
                message: errorInternoServidor,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);
        }
    });
};