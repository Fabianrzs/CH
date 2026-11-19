import { Button, Container, Grid } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import 'date-fns';
import MaterialTable from 'material-table';
import { connect } from 'react-redux';
import '../../../estiloprincipal.scss';
import {
    startUpdateState,
    startInsertState,
    startGetState,
    startDeleteState,
} from '../../../actions/state.action';
import { AppState } from '../../../store/rootReducer';
import AlertDialog from '../../../components/dialog/dialog.component';
import { isNumeric } from '../../../utils/regex';
import { EstadoDto } from '../../../types/state.model.type';
import { StateForm } from '../constants/form.model.type';
import { LoginState } from '../../../reducers/login.reducer';
import { StateState } from '../../../reducers/state.reducer';
import { ModalEditar } from './ModalEditar';
import { ModalAgregar } from './ModalAgregar';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';

const StateTable = (props: Props) => {
    const [formState, setFormState] = useState(StateForm);
    const {
        stateState,
        loginState,
        load,
        startUpdateState,
        startInsertState,
        startDeleteState,
        startGetState,
    } = props;

    const [confirmDialog, setConfirmDialog] = useState({
        isOpen: false,
        title: '',
        bodyText: '',
        onConfirm: () => {},
    });

    const setNewData = () => {
        setTimeout(() => {
            load();
        }, 500);
    };

    const setFormData = () => {
        setFormState(stateState.states);
    };

    useEffect(() => {
        setFormData();
    }, [0]);

    const handleonblurNumeric = ({ target }: any) => {
        if (!isNumeric(target.value)) {
            alert('Los caracteres son incorrectos');
            setFormState({
                ...formState,
                [target.name]: '0',
            });
        }
    };

    const handleInputChangeNumeric = ({ target }: any) => {
        if (target.value >= 0) {
            setFormState({
                ...formState,
                [target.name]: target.value,
            });
        }
    };

    const handleonblurText = ({ target }: any) => {
        setFormState({
            ...formState,
            [target.name]: target.value,
        });
    };

    const handleInputChangeText = ({ target }: any) => {
        setFormState({
            ...formState,
            [target.name]: target.value,
        });
    };

    const handleOnClickUpdate = async () => {
        if (
            formState?.codigo === 0 ||
            formState?.concepto.trim() === '' ||
            formState?.diasMoraActivaEstado.trim() === '' ||
            formState?.saldoDeuda.trim() === ''
        ) {
            swal({
                text: 'Deben diligenciarse todos los campos',
                icon: 'warning',
            });
        } else {
            const confirm = await swal({
                title: 'Confirmación',
                text: '¿Está seguro de que desea actualizar la información?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
                dangerMode: true,
            });

            if (!confirm) {
                return; // ❌ No actualiza
            }

            updateState();
        }

    };

    const updateState = () => {
        handleClose1();
        handleClose2();
        startUpdateState(formState, loginState.claims);
        setNewData();
    };

    const handleOnClickInsert = () => {
        if (
            formState?.codigo === 0 ||
            formState?.concepto.trim() === '' ||
            formState?.diasMoraActivaEstado.trim() === '' ||
            formState?.saldoDeuda.trim() === ''
        ) {
            swal({
                text: 'Deben diligenciarse todos los campos',
                icon: 'warning',
            });
        } else {
            insertState();
        }

    };

    const insertState = () => {

        handleClose1();
        handleClose2();
        startInsertState(formState, loginState.claims);
        setNewData();
    };

    const handleOnClickDelete = (formStateLocal: any) => {
        swal({
            text: 'El estado del registro se modificará como INACTIVO.¿Desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteState(formStateLocal);
            }
        });

    };

    const deleteState = (formStateLocal: any) => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });
        handleClose1();
        handleClose2();
        startDeleteState(formStateLocal, loginState.claims);
        toast.success('Estado modificado con éxito');
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
        setFormState(StateForm);
    };

    const handleClose2 = () => {
        setOpen2(false);
    };

    useEffect(() => {
        startGetState(loginState.claims);
    }, []);

    const columns: any = [
        {
            title: 'Código',
            field: 'codigo',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopLeftRadius: '12px',
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Concepto',
            field: 'concepto',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Dias Mora Activa Estado',
            field: 'diasMoraActivaEstado',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Saldo Mínimo Deuda',
            field: 'saldoDeuda',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
    ];

    const intercalar = (index: number) => {
        let color = '#f1f2f6';
        if (index % 2 === 0) {
            color = '#C4C4C4';
        }

        return {
            background: color,
            border: '1.5px solid #FFF',
        };
    };

    return (
        <CHPrivateContainer>
            <CHPaper title="Módulo Estados">
                <Container maxWidth="lg">
                    {/* <Typography variant="h6" align="center" className="titulo1">
                    Parametrización
                </Typography>
                <Typography variant="h6" align="center" className="titulo2">
                    de estados
                </Typography> */}
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
                                style={{
                                    borderTopLeftRadius: '12px',
                                    borderTopRightRadius: '12px',
                                }}
                                title=""
                                columns={columns}
                                data={stateState.allstates}
                                actions={[
                                    {
                                        icon: 'edit',
                                        tooltip: 'Editar',
                                        onClick: (event, rowData: any) => {
                                            handleClickOpen1();
                                            setFormState(rowData);
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
                                    rowStyle: (data, index) =>
                                        intercalar(index),
                                    actionsColumnIndex: -1,
                                    headerStyle: {
                                        backgroundColor: '#FEC917',
                                        fontWeight: 'bold',
                                        color: '#676767',
                                        borderColor: '#FFF',
                                        border: '1.5px solid #FFF',
                                        borderTopRightRadius: '12px',
                                    },
                                }}
                                localization={{
                                    header: {
                                        actions: 'Acciones',
                                    },
                                    toolbar: {
                                        searchPlaceholder: 'Buscar',
                                    },
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
                    formState={formState}
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
                    formState={formState}
                    confirmDialog={confirmDialog}
                    setConfirmDialog={setConfirmDialog}
                />
            </CHPaper>
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    stateState: StateState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    stateState: state.state,
});

interface LinkMapDispatchToProps {
    startUpdateState: (State: EstadoDto, props: string) => void;
    startInsertState: (State: EstadoDto, props: string) => void;
    startDeleteState: (State: EstadoDto, props: string) => void;
    startGetState: (props: string) => void;
    load: () => void;
}

const mapDispatchToProps = {
    startUpdateState,
    startInsertState,
    startDeleteState,
    startGetState,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(StateTable);
