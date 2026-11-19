import React, { useState } from 'react';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import MaterialTable from 'material-table';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import { ModalAgregarBloqueo } from './components/ModalAgregarBloqueo';
import { ModalEditarBloqueo } from './components/ModalEditarBloqueo';
import { LockState } from '../../reducers/lock.reducer';
import { LockForm } from '../locks/constants/form.model.type';
import { BloqueoDto } from '../../types/lock.model.type';
import {
    startInsertLock,
    startUpdateLock,
    startDeleteLock,
} from '../../actions/lock.action';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const TableBloqueo = (props: props) => {
    const classes = useStyles();

    const {
        lockState,
        load,
        startInsertLock,
        startUpdateLock,
        startDeleteLock,
    } = props;

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);
    const [bloqueo, setBloqueo] = useState<BloqueoDto>(LockForm);

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
            title: 'Reversible',
            field: 'reversible',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) =>
                rowData.reversible === true ? 'SI' : 'NO',
        },
        {
            title: 'Aceleración Deuda',
            field: 'aceleracionDeuda',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) =>
                rowData.aceleracionDeuda === true ? 'SI' : 'NO',
        },
    ];

    const handleOpenModalAgregar = () => {
        setOpenModalAgregar(true);
    };

    const handleCloseModalAgregar = async () => {
        await setOpenModalAgregar(false);
        await setBloqueo(LockForm);
    };

    const handleOpenModalEditar = async (rowData: any) => {
        const bloqueoEdit = rowData as BloqueoDto;
        await setOpenModalEditar(true);
        await setBloqueo(bloqueoEdit);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };

    const handleOnChange = ({ target }: any) => {
        setBloqueo({
            ...bloqueo,
            [target.name]: target.value,
        });
    };

    const handleOnCheckedReversible = ({ target }: any) => {
        setBloqueo({
            ...bloqueo,
            reversible: target.checked,
        });
    };

    const handleOnCheckedAceleracionDeuda = ({ target }: any) => {
        setBloqueo({
            ...bloqueo,
            aceleracionDeuda: target.checked,
        });
    };

    const handleOnClickAgregar = async () => {
        if (bloqueo.codigo.toString() === '' || bloqueo.concepto === '') {
            swal({
                text: 'Deben diligenciarse todos los campos',
                icon: 'warning',
            });
        } else {
            await startInsertLock(bloqueo);
            load();
            await setBloqueo(LockForm);
            handleCloseModalAgregar();
            toast.success('Información Guardada');
        }
    };

    const handleOnClickEditar = async () => {
        if (bloqueo.codigo.toString() === '' || bloqueo.concepto === '') {
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

            await startUpdateLock(bloqueo);
            load();
            await setBloqueo(LockForm);
            handleCloseModalEditar();
            toast.success('Información Actualizada');
        }
    };

    const deleteState = async (bloqueoDelete: BloqueoDto) => {
        await startDeleteLock(bloqueoDelete);
        load();
        toast.success('Estado modificado con éxito');
    };

    const handleOnClickDelete = async (rowData: any) => {
        const bloqueoEdit = rowData as BloqueoDto;
        await setBloqueo(bloqueoEdit);
        swal({
            text: 'El estado del registro se modificará como INACTIVO.¿Desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteState(bloqueo);
            }
        });
    };

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
        <>
            <Grid className={classes.column} item xs={12}>
                <Grid className={classes.column} item xs={1}>
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        onClick={handleOpenModalAgregar}
                    >
                        Agregar
                    </Button>
                </Grid>
                <MaterialTable
                    style={{
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '12px',
                    }}
                    title=""
                    data={lockState.locks}
                    columns={columns}
                    actions={[
                        {
                            icon: 'edit',
                            tooltip: 'Editar',
                            onClick: (event, rowData: any) => {
                                handleOpenModalEditar(rowData);
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
                        headerStyle: {
                            backgroundColor: '#FEC917',
                            fontWeight: 'bold',
                            color: '#676767',
                            borderColor: '#FFF',
                            border: '1.5px solid #FFF',
                            borderTopRightRadius: '12px',
                        },
                        pageSizeOptions: [4, 8, 10],
                        pageSize: 4,
                        search: true,
                        toolbar: true,
                        rowStyle: (data, index) => intercalar(index),
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
                <ModalAgregarBloqueo
                    open={openModalAgregar}
                    handleClose={handleCloseModalAgregar}
                    handleOnClickAgregar={handleOnClickAgregar}
                    handleOnChange={handleOnChange}
                    handleOnCheckedReversible={handleOnCheckedReversible}
                    handleOnCheckedAceleracionDeuda={
                        handleOnCheckedAceleracionDeuda
                    }
                />
                <ModalEditarBloqueo
                    open={openModalEditar}
                    handleClose={handleCloseModalEditar}
                    handleOnClickEditar={handleOnClickEditar}
                    handleOnChange={handleOnChange}
                    handleOnCheckedReversible={handleOnCheckedReversible}
                    handleOnCheckedAceleracionDeuda={
                        handleOnCheckedAceleracionDeuda
                    }
                    formBloqueo={bloqueo}
                />
            </Grid>
        </>
    );
};

interface LinkMapStateToProps {
    lockState: LockState;
}

const mapStateToProps = (state: AppState) => ({
    lockState: state.lock,
});

interface LinkMapDispatchToProps {
    load: () => void;
    startInsertLock: (lock: BloqueoDto) => void;
    startUpdateLock: (lock: BloqueoDto) => void;
    startDeleteLock: (Lock: BloqueoDto) => void;
}

const mapDispatchToProps = {
    startInsertLock,
    startUpdateLock,
    startDeleteLock,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableBloqueo);
