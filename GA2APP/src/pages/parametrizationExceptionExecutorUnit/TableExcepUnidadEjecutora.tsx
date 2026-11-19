import React, { useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    Theme,
} from '@material-ui/core';
import MaterialTable from 'material-table';
import CheckIcon from '@material-ui/icons/Check';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import { useForm } from 'react-hook-form';
import {
    startGetAllUnidadEjecutora,
    startPostInsertSimuladorUnidadEjecutora,
    startPostUpdateSimuladorUnidadEjecutora,
    startPostDeleteSimuladorUnidadEjecutora,
} from '../../actions/simulador.action';
import { ModalAgregarExcepcionUnidadEjecutora } from './components/ModalAgregarExcepcionUnidadEjecutora';
import { ModalEditarExcepcionUnidadEjecutora } from './components/ModalEditarExcepcionUnidadEjecutora';
import { SimuladorUndEjDto } from '../../types/simulador.model.type';
import { AppState } from '../../store/rootReducer';
import { SimuladorState } from '../../reducers/simulador.reducer';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        checkContainer: {
            width: '100%',
            alignItems: 'center',
            paddingRight: 30,
            color: theme.palette.success.main,
        },
        inactiveContainer: {
            width: '100%',
            alignItems: 'center',
            paddingRight: 10,
            color: '#C44',
        },
    }),
);

const TableExcepUnidadEjecutora = (props: Props) => {
    const classes = useStyles();
    const {
        simuladorState,
        load,
        selectedSimulacion,
        startGetAllUnidadEjecutora,
        startPostInsertSimuladorUnidadEjecutora,
        startPostUpdateSimuladorUnidadEjecutora,
        startPostDeleteSimuladorUnidadEjecutora,
    } = props;

    const [
        openModalAgregarUnidadEjecutora,
        setOpenModalAgregarUnidadEjecutora,
    ] = useState(false);
    const [openModalEditarUnidadEjecutora, setOpenModalEditarUnidadEjecutora] =
        useState(false);

    const handleAllExecutorUnit = () => {
        startGetAllUnidadEjecutora();
    };

    const formularioUnidadEjecutora = useForm({
        defaultValues: {
          idParametro: 0,
          idSimulacion: 0,
          idParametroUnidadEjecutora: '',
          tasaEA: '',
          estado: '1',
        }
    })

    const columns: any = [
        {
            title: 'Unidad Ejecutora',
            field: 'nombreUnidadEjecutora',
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
            title: 'Tasa EA',
            field: 'tasaEa',
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
            title: 'Estado',
            field: 'estado',
            align: 'center',
            render: (rowData: any) => (
                <>
                    {' '}
                    {rowData.estado === 1 ? (
                        <span className={classes.checkContainer}>
                            <b>
                                <CheckIcon /> ACTIVO
                            </b>
                        </span>
                    ) : (
                        <span className={classes.inactiveContainer}>
                            <b>INACTIVO</b>
                        </span>
                    )}
                </>
            ),
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
    ];

    const handleOpenModalAgregarUnidadEjecutora = () => {
        formularioUnidadEjecutora.reset();
        handleAllExecutorUnit();
        setOpenModalAgregarUnidadEjecutora(true);
    };

    const handleCloseModalAgregarUnidaEjecutora = () => {
        setOpenModalAgregarUnidadEjecutora(false);
    };

    const handleOpenModalEditarUnidadEjecutora = (simulador: any) => {
        formularioUnidadEjecutora.reset();
        handleAllExecutorUnit();
        formularioUnidadEjecutora.setValue('idParametro',simulador.idParametro);
        formularioUnidadEjecutora.setValue('idParametroUnidadEjecutora',simulador.idUnidadEjecutora);
        formularioUnidadEjecutora.setValue('idSimulacion',simulador.idSimulacion);
        formularioUnidadEjecutora.setValue('tasaEA',simulador.tasaEa);
        formularioUnidadEjecutora.setValue('estado',simulador.estado);
        setOpenModalEditarUnidadEjecutora(true);
    };

    const handleCloseModalEditarUnidadEjecutora = () => {
        setOpenModalEditarUnidadEjecutora(false);
    };

    const setNewData = async () => {
        await load();
    };

    const handleOnClickAgregar = async (data: any) => {
            const unidadEjecutora: SimuladorUndEjDto = {
                idParametro: 0,
                idSimulacion: 0,
                idParametroUnidadEjecutora: data.idParametroUnidadEjecutora,
                tasaEA: Number(data.tasaEA),
                estado: data.estado,
            }
            await startPostInsertSimuladorUnidadEjecutora(
                selectedSimulacion,
                unidadEjecutora,
            );
            handleCloseModalAgregarUnidaEjecutora();
            setNewData();
            toast.success('Información Guardada');
    };

    const handleOnClickEditar = async (data: any) => {
        
        const unidadEjecutora: SimuladorUndEjDto = {
            idParametro: data.idParametro,
            idSimulacion: data.idSimulacion,
            idParametroUnidadEjecutora: data.idParametroUnidadEjecutora,
            tasaEA: Number(data.tasaEA),
            estado: data.estado,
        }
            await startPostUpdateSimuladorUnidadEjecutora(
                unidadEjecutora,
            );
            handleCloseModalEditarUnidadEjecutora();
            setNewData();
            toast.success('Información actualizada');
    };

    const deleteState = async (simulador: SimuladorUndEjDto) => {
        await startPostDeleteSimuladorUnidadEjecutora(simulador);
        setNewData();
        toast.success('Estado modificado con éxito');
    };

    const handleOnClickEliminar = (rowData: any) => {
        const simuladorEliminar = {
            idParametro: rowData.idParametro,
        };
        swal({
            text: 'El estado del registro se modificará como INACTIVO.¿Desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteState(simuladorEliminar as SimuladorUndEjDto);
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
            <Grid container>
                <Button
                    size="small"
                    variant="contained"
                    color="primary"
                    onClick={handleOpenModalAgregarUnidadEjecutora}
                >
                    Agregar
                </Button>
            </Grid>
            <Grid container>
                <Grid item xs={12}>
                    <MaterialTable
                        style={{
                            borderTopLeftRadius: '12px',
                            borderTopRightRadius: '12px',
                        }}
                        title=""
                        data={simuladorState.simuladorUniEjecutoras}
                        columns={columns}
                        actions={[
                            {
                                icon: 'edit',
                                tooltip: 'Editar',
                                onClick: (event, rowData: any) => {
                                    handleOpenModalEditarUnidadEjecutora(
                                        rowData,
                                    );
                                },
                            },
                            {
                                icon: 'delete',
                                tooltip: 'Eliminar',
                                onClick: (event, rowData: any) => {
                                    if(rowData.estado === 0){
                                        swal({
                                            text: 'El estado del registro es INACTIVO',
                                            icon: 'warning',
                                        });
                                    }else{
                                        handleOnClickEliminar(rowData);
                                    }
                                  
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
                </Grid>
            </Grid>
            <ModalAgregarExcepcionUnidadEjecutora
                handleClose={handleCloseModalAgregarUnidaEjecutora}
                open={openModalAgregarUnidadEjecutora}
                handleOnClickAgregar={handleOnClickAgregar}
                allExecutorUnit={simuladorState.simuladorunidadEjecutora}
                formularioUnidadEjecutora={formularioUnidadEjecutora}
            />
            <ModalEditarExcepcionUnidadEjecutora
                handleClose={handleCloseModalEditarUnidadEjecutora}
                open={openModalEditarUnidadEjecutora}
                handleOnClickUpdate={handleOnClickEditar}
                allExecutorUnit={simuladorState.simuladorunidadEjecutora}
                formularioUnidadEjecutora={formularioUnidadEjecutora}
            />
        </>
    );
};

interface LinkMapStateToProps {
    simuladorState: SimuladorState;
}

const mapStateToProps = (state: AppState) => ({
    simuladorState: state.simulador,
});
interface LinkMapDispatchToProps {
    startGetAllUnidadEjecutora: () => void;
    startPostInsertSimuladorUnidadEjecutora: (
        idSimulacion: number,
        simuladorUniej: SimuladorUndEjDto,
    ) => void;
    startPostUpdateSimuladorUnidadEjecutora: (
        simuladorUniej: SimuladorUndEjDto,
    ) => void;
    startPostDeleteSimuladorUnidadEjecutora: (
        simuladorUniej: SimuladorUndEjDto,
    ) => void;
    load: () => void;
    selectedSimulacion: number;
}

const mapDispatchToProps = {
    startGetAllUnidadEjecutora,
    startPostInsertSimuladorUnidadEjecutora,
    startPostUpdateSimuladorUnidadEjecutora,
    startPostDeleteSimuladorUnidadEjecutora,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(TableExcepUnidadEjecutora);
