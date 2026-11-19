import { Card, CardHeader, createStyles, makeStyles } from '@material-ui/core';
import React, { useEffect } from 'react';
import 'date-fns';
import MaterialTable from 'material-table';
import { connect } from 'react-redux';
import '../../../estiloprincipal.scss';
import NotificationsActiveIcon from '@material-ui/icons/NotificationsActive';
import WarningIcon from '@material-ui/icons/Warning';
import { startGetNotification } from '../../../actions/notification.action';
import '../notification.style.scss';
import { NotificationState } from '../../../reducers/notification.reducer';
import { AppState } from '../../../store/rootReducer';

const useStyles = makeStyles(() =>
    createStyles({
        title: {
            backgroundColor: '#1C3F8B',
            color: 'white',
        },
        table: {
            minWidth: 700,
        },
        divStilo: {
            width: '100% !important',
        },
    }),
);

const NotificationsTable = (props: Props) => {
    const { startGetNotification, notificationState } = props;
    const classes = useStyles();

    const load = () => {
        startGetNotification();
    };

    useEffect(() => {
        load();
    }, []);

    const displayImageAlert = (rowData: any) => (
        <div className={classes.divStilo}>
            <WarningIcon style={{ width: '20%', fill: '#F1B90C' }} />
            <span style={{ marginLeft: '20px' }}>{rowData.tipo}</span>
        </div>
    );
    const displayImageNotification = (rowData: any) => (
        <div className={classes.divStilo}>
            <NotificationsActiveIcon
                style={{ width: '20%', fill: '#F1B90C' }}
            />
            <span style={{ marginLeft: '20px' }}>{rowData.tipo}</span>
        </div>
    );

    const columns: any = [
        {
            title: 'Tipo',
            field: 'tipo',
            render: (rowData: any) =>
                rowData.tipo === 'Alerta'
                    ? displayImageAlert(rowData)
                    : displayImageNotification(rowData),
        },
        {
            title: 'Descripción',
            field: 'descipcion',
        },
    ];

    return (
        <div>
            <Card variant="outlined">
                <CardHeader className={classes.title} title="Alertas" />
            </Card>
            <MaterialTable
                title=""
                columns={columns}
                data={notificationState.notifications}
                actions={[
                    {
                        icon: 'edit',
                        tooltip: 'Editar',
                        onClick: () => alert('entro'),
                    },
                    {
                        icon: 'delete',
                        tooltip: 'Eliminar',
                        onClick: () => alert('entro'),
                    },
                    {
                        icon: 'delete',
                        tooltip: 'Eliminar',
                        onClick: () => alert('entro'),
                    },
                    {
                        icon: 'delete',
                        tooltip: 'Eliminar',
                        onClick: () => alert('entro'),
                    },
                ]}
                options={{
                    actionsColumnIndex: -1,
                    search: false,
                    paging: false,
                    toolbar: false,
                    headerStyle: {
                        backgroundColor: '#F3F0EE',
                    },
                }}
            />
        </div>
    );
};

interface LinkMapStateToProps {
    notificationState: NotificationState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    notificationState: state.notification,
});

interface LinkMapDispatchToProps {
    startGetNotification: () => void;
}

const mapDispatchToProps = {
    startGetNotification,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(NotificationsTable);
