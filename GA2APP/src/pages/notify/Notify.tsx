import {
    HttpTransportType,
    HubConnection,
    HubConnectionBuilder,
    LogLevel,
} from '@microsoft/signalr';
import { connect } from 'react-redux';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { routega2api } from '../../utils/baserouteapi.route';
import StorageService from '../../services/storage.service';
import notifyMe from '../../utils/notificacion';
import { AppState } from '../../store/rootReducer';
import { startGetNotification } from '../../actions/notification.action';
import NotificationService from '../../services/notification.service';
import AuthService from '../../services/auth.service';

const Notify = (props: any) => {

    const [connection, setConnection] = useState<null | HubConnection>(null);
    const { startGetNotification } = props;
    const GRUPO_ID = "60";
    const EMAIL = "adriana.moreno@cajahonor.gov.co";
    const user = AuthService.getUserInfo();

    useEffect(() => {
        const connect = new HubConnectionBuilder()
            .withUrl(`${routega2api}hubs/notificacions`, {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets,
                accessTokenFactory: () =>
                    StorageService.getSessionToken() as string,
            })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        setConnection(connect);
    }, []);

    useEffect(() => {
        if (connection) {
            connection
                .start()
                .then(async () => {
                    try{
                        const connectionId = await connection.invoke<string>('GetConnectionId');
                        localStorage.setItem('connectionId', connectionId);
                    }catch(error: any){
                        toast(`Error al consultar id ${error.message}`, { type: toast.TYPE.ERROR });
                    }

                    connection.on('SincronizacionLiquidacionInteres',(data) => {
                        if(user?.GRP_ID === GRUPO_ID && user?.USR_EMAIL === EMAIL){
                            NotificationService.genericAlert(
                                '¡Exitoso!',
                                'Proceso de tarea sincronización finalizada.'
                            );
                        }
                    })

                    connection.on('RecibirNotificacionPorCliente', mensaje => {
                        startGetNotification();
                        notifyMe(mensaje.mensaje);
                    });
                    connection.on('RecibirNotificacionPorGrupo', mensaje => {
                        startGetNotification();
                        notifyMe(mensaje.mensaje);
                    });
                })
                .catch(error =>{
                    toast('Error',  {type: toast.TYPE.ERROR})
                });
                
        }
    }, [connection]);

    return <></>;
};

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    notificationState: state.notification,
});

const mapDispatchToProps = {
    startGetNotification,
};

export default connect(mapStateToProps, mapDispatchToProps)(Notify);
