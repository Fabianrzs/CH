import { Button, Container, Grid, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import 'date-fns';
import MaterialTable from 'material-table';
import { connect } from 'react-redux';
import '../../../estiloprincipal.scss';
import {
    startUpdateLock,
    startInsertLock,
    startGetLock,
    startDeleteLock,
} from '../../../actions/lock.action';
import { AppState } from '../../../store/rootReducer';
import AlertDialog from '../../../components/dialog/dialog.component';
import { isNumeric } from '../../../utils/regex';
import { BloqueoDto } from '../../../types/lock.model.type';
import { LockForm } from '../constants/form.model.type';
import { LoginState } from '../../../reducers/login.reducer';
import { LockState } from '../../../reducers/lock.reducer';
import { ModalEditar } from './ModalEditar';
import { ModalAgregar } from './ModalAgregar';

const LocksTable = (props: Props) => {
    const [formLock, setFormLock] = useState(LockForm);
    const { startInsertLock, startGetLock, startUpdateLock, load, startDeleteLock, lockState, loginState } = props;

    const [confirmDialog, setConfirmDialog] = useState({
        isOpen: false,
        title: '',
        bodyText: '',
        onConfirm: () => { },
    });

    const setNewData = () => {
        setTimeout(() => {
            load();
        }, 500);
    };

    const setFormData = () => {
        setFormLock(lockState.lock);
    };

    useEffect(() => {
        setFormData();
    }, [0]);

    const handleonblurNumeric = ({ target }: any) => {
        if (!isNumeric(target.value)) {
            alert('Los caracteres son incorrectos');
            setFormLock({
                ...formLock,
                [target.name]: '0',
            });
        }
    };

    const handleInputChangeNumeric = ({ target }: any) => {
        if (target.value >= 0) {
            setFormLock({
                ...formLock,
                [target.name]: target.value,
            });
        }
    };

    const handleonblurText = ({ target }: any) => {
        setFormLock({
            ...formLock,
            [target.name]: target.value,
        });
    };

    const handleInputChangeText = ({ target }: any) => {
        let updatedValue = target.value;
        if (updatedValue === 'true' || updatedValue === 'false') {
            updatedValue = JSON.parse(updatedValue);
        }
        setFormLock({
            ...formLock,
            [target.name]: updatedValue,
        });
    };

    const handleOnClickUpdate = () => {
        setConfirmDialog({
            isOpen: true,
            title: 'Confirmación',
            bodyText: '¿Está seguro que quiere actualizar este bloqueo?',
            onConfirm: () => {
                updateState();
            },
        });
    };

    const updateState = () => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });
        handleClose1();
        handleClose2();
        startUpdateLock(formLock, loginState.claims);
        setNewData();
    };

    const handleOnClickInsert = () => {
        setConfirmDialog({
            isOpen: true,
            title: 'Confirmación',
            bodyText: '¿Está seguro que quiere agregar este bloqueo?',
            onConfirm: () => {
                insertState();
            },
        });
    };

    const insertState = () => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });
        handleClose1();
        handleClose2();
        startInsertLock(formLock, loginState.claims);
        setNewData();
    };

    const handleOnClickDelete = (formLockLocal: any) => {
        setConfirmDialog({
            isOpen: true,
            title: 'Confirmación',
            bodyText: '¿Está seguro que quiere eliminar este bloqueo?',
            onConfirm: () => {
                deleteState(formLockLocal);
            },
        });
    };

    const deleteState = (formLockLocal: any) => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });
        handleClose1();
        handleClose2();
        startDeleteLock(formLockLocal, loginState.claims);
        setNewData();
    };

    const [open1, setOpen1] = React.useState(false);
    const [open2, setOpen2] = React.useState(false);

    const handleClickOpen1 = () => {
        setOpen1(true);
    };

    const handleClose1 = () => {
        setOpen1(false);
    };
    const handleClickOpen2 = () => {
        setOpen2(true);
        setFormLock(LockForm);
    };

    const handleClose2 = () => {
        setOpen2(false);
    };

    useEffect(() => {
        startGetLock(loginState.claims);
    }, []);

    const columns: any = [
        {
            title: 'Codigo',
            field: 'codigo',
        },
        {
            title: 'Concepto',
            field: 'concepto',
        },
        {
            title: 'Dias Mora',
            field: 'diasMora',
        },
        {
            title: 'Reversible',
            field: 'reversible',
            render: (rowData: any) =>
                rowData.reversible === true ? 'Si' : 'No',
        },
        {
            title: 'Aceleracion Deuda',
            field: 'aceleracionDeuda',
            render: (rowData: any) =>
                rowData.aceleracionDeuda === true ? 'Si' : 'No',
        },
    ];

    return (
        <div>
            <Container maxWidth="lg">
                <Typography variant="h6" align="center" className="titulo1">
                    Modulo
                </Typography>
                <Typography variant="h6" align="center" className="titulo2">
                    de bloqueos
                </Typography>
                <Grid container spacing={3}>
                    <Grid item xs={3}>
                        <Button
                            size="small"
                            variant="contained"
                            color="primary"
                            onClick={handleClickOpen2}
                        >
                            Agregar
                        </Button>
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item xs={12}>
                        <MaterialTable
                            title=""
                            columns={columns}
                            data={lockState.locks}
                            actions={[
                                {
                                    icon: 'edit',
                                    tooltip: 'Editar',
                                    onClick: (event, rowData: any) => {
                                        handleClickOpen1();
                                        setFormLock(rowData);
                                    },
                                },
                                {
                                    icon: 'delete',
                                    tooltip: 'Eliminar',
                                    onClick: (event, rowData: any) => {
                                        handleOnClickDelete(rowData);
                                    },
                                },
                            ]}
                            options={{
                                actionsColumnIndex: -1,
                            }}
                        />
                    </Grid>
                </Grid>
            </Container>
            <AlertDialog
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
            <ModalEditar
                handleClose={handleClose1}
                open={open1}
                handleOnClickSave={handleOnClickUpdate}
                handleonblurText={handleonblurText}
                handleInputChangeText={handleInputChangeText}
                handleonblurNumeric={handleonblurNumeric}
                handleInputChangeNumeric={handleInputChangeNumeric}
                formLock={formLock}
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
            <ModalAgregar
                handleClose={handleClose2}
                open={open2}
                handleOnClickSave={handleOnClickInsert}
                handleonblurText={handleonblurText}
                handleInputChangeText={handleInputChangeText}
                handleonblurNumeric={handleonblurNumeric}
                handleInputChangeNumeric={handleInputChangeNumeric}
                formLock={formLock}
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
        </div>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    lockState: LockState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    lockState: state.lock,
});

interface LinkMapDispatchToProps {
    startUpdateLock: (State: BloqueoDto, props: string) => void;
    startInsertLock: (State: BloqueoDto, props: string) => void;
    startDeleteLock: (State: BloqueoDto, props: string) => void;
    startGetLock: (props: string) => void;
    load: () => void;
}

const mapDispatchToProps = {
    startUpdateLock,
    startInsertLock,
    startDeleteLock,
    startGetLock,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(LocksTable);
