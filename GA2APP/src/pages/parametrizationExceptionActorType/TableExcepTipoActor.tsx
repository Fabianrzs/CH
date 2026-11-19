import React, { useState } from 'react';
import swal from 'sweetalert';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    Theme,
} from '@material-ui/core';
import MaterialTable from 'material-table';
import CheckIcon from '@material-ui/icons/Check';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { SimuladorState } from '../../reducers/simulador.reducer';
import { AppState } from '../../store/rootReducer';
import { ModalExcepAgregarTipoAfiliado } from './components/ModalAgregarExcepTipoAfiliado';
import { ModalEditarExcepTipoAfiliado } from './components/ModalEditarExcepTipoAfiliado';
import {
    startGetAllTipoActor,
    startPostInsertSimuladorTipoActor,
    startPostUpdateSimuladorTipoActor,
    startPostDeleteSimuladorTipoActor,
} from '../../actions/simulador.action';
import { SimuladorActorDto, TipoActorDto } from '../../types/simulador.model.type';
import { SimuladorActorForm } from '../parametrizationMortgageCredit/constans/simuladorform.model.type';

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

const TableExcepTipoActor = (props: Props) => {
    const classes = useStyles();
    const {
        simuladorState,
        selectedSimulacion,
        load,
        startGetAllTipoActor,
        startPostInsertSimuladorTipoActor,
        startPostUpdateSimuladorTipoActor,
        startPostDeleteSimuladorTipoActor,
    } = props;
    const [simuladorTipoActor, setSimuladorTipoActor] =
        useState<SimuladorActorDto>(SimuladorActorForm);
    const [estadoTipoActor, setEstadoTipoActor] = useState(true);
    const [estadoTasa, setEstadoTasa] = useState(true);
    const [valorTasa, setValorTasa] = useState(0);
    const [mensajeErrorTasa, setMensajeErrorTasa] = useState('Requerido');
    const [
        openModalAgregarExcepTipoAfiliado,
        setOpenModalAgregarExcepTipoAfiliado,
    ] = useState(false);
    const [
        openModalEditarExcepTipoAfiliado,
        setOpenModalEditarExcepTipoAfiliado,
    ] = useState(false);

    const handleAllActorType = () => {
        startGetAllTipoActor();
    };

    // useEffect(() => {
    //     filtrarTiposActor();
    //     console.log('listaTipoActor',listaTipoActor)
    //     console.log('state',simuladorState.simuladorTipoActor)
    // },[])

    // const filtrarTiposActor = () => {

    //     // const array = simuladorState.simuladorTipoActor.filter(item => (
    //     //        item.descripcionTipoActor === 'VETERANOS' ||
    //     //        item.descripcionTipoActor === 'FUNCIONARIO AFILIADO SOLUCIÓN VIVIENDA' ||
    //     //        item.descripcionTipoActor === 'FUNCIONARIO AFILIADO ADMINISTRACIÓN CESANTÍAS' ||
    //     //        item.descripcionTipoActor === 'AFILIADO ADMINISTRACIÓN CESANTÍAS' ||
    //     //        item.descripcionTipoActor === 'AFILIADO SOLUCIÓN VIVIENDA')
    //     // )
        
    //     const array = simuladorState.simuladorTipoActor.map(item => {
    //         debugger
    //         if(item.descripcionTipoActor === 'VETERANOS' ||
    //            item.descripcionTipoActor === 'FUNCIONARIO AFILIADO SOLUCIÓN VIVIENDA' ||
    //            item.descripcionTipoActor === 'FUNCIONARIO AFILIADO ADMINISTRACIÓN CESANTÍAS' ||
    //            item.descripcionTipoActor === 'AFILIADO ADMINISTRACIÓN CESANTÍAS' ||
    //            item.descripcionTipoActor === 'AFILIADO SOLUCIÓN VIVIENDA'){
    //             return item;
    //         }
    //     })

    //     setListaTipoActor(array);
    // }

    const formularioTipoActor = useForm({
        defaultValues: {
            parametroId: '',
            simuladorId: '',
            tipoId: '',
            parametroTasaEa: '',
            estado: '1'
        }
    })

    const columns: any = [
        {
            title: 'Tipo Afiliado',
            field: 'tipoParametroDescripcion',
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
            title: 'Tasa EA (%)',
            field: 'parametroTasaEa',
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
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) => (
                <>
                    {rowData.estado === '1' ? (
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
        },
    ];

    const handleOpenModalAgregarTipoAfiliado = () => {
        handleAllActorType();
        formularioTipoActor.reset();
        setOpenModalAgregarExcepTipoAfiliado(true);
    };

    const handleCloseModalAgregaTipoAfiliado = () => {
        setOpenModalAgregarExcepTipoAfiliado(false);
    };

    const handleOpenModalEditarTipoAfiliado = (simulador: any) => {
        formularioTipoActor.reset();
        handleAllActorType();
        formularioTipoActor.setValue('parametroId', simulador.parametroId);
        formularioTipoActor.setValue('simuladorId', simulador.simuladorId);
        formularioTipoActor.setValue('tipoId', simulador.tpaId);
        formularioTipoActor.setValue('parametroTasaEa', simulador.parametroTasaEa);
        formularioTipoActor.setValue('estado', simulador.estado);
        setOpenModalEditarExcepTipoAfiliado(true);
    };

    const handleCloseModalEditarTipoAfiliado = () => {
        setOpenModalEditarExcepTipoAfiliado(false);
    };

    const setNewData = () => {
        setTimeout(() => {
            load();
        }, 500);
    };

    const handleOnClickAgregar = async (data: any) => {
        const datosSimuladorTipoActor: SimuladorActorDto = {
            tipoId: data.tipoId,
            parametroTasaEa: data.parametroTasaEa,
            parametroEstado: 1,
        }
            await startPostInsertSimuladorTipoActor(
                selectedSimulacion,
                datosSimuladorTipoActor,
            );
            handleCloseModalAgregaTipoAfiliado();
            setNewData();
            toast.success('Información Guardada');
        
    };

    const handleOnClickEditar = async (data: any) => {
        const datosSimuladorTipoActor: SimuladorActorDto = {
            parametroId: data.parametroId,
            tipoId: data.tipoId,
            parametroTasaEa: data.parametroTasaEa,
            parametroEstado: data.estado,
            
        }
        await startPostUpdateSimuladorTipoActor(datosSimuladorTipoActor);
        handleCloseModalEditarTipoAfiliado();
        setNewData();
        toast.success('Información actualizada');
    };

    const deleteState = async (simulador: SimuladorActorDto) => {
        await startPostDeleteSimuladorTipoActor(simulador);
        setNewData();
        toast.success('Estado modificado con éxito');
    };

    const handleOnClickEliminar = (rowData: any) => {
        const simuladorEliminar = {
            parametroId: rowData.parametroId,
        };
        swal({
            text: 'El estado del registro se modificará como INACTIVO.¿Desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteState(simuladorEliminar as SimuladorActorDto);
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
                    onClick={handleOpenModalAgregarTipoAfiliado}
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
                        data={simuladorState.simuladorActores}
                        columns={columns}
                        actions={[
                            {
                                icon: 'edit',
                                tooltip: 'Editar',
                                onClick: (event, rowData: any) => {
                                    handleOpenModalEditarTipoAfiliado(rowData);
                                },
                            },
                            {
                                icon: 'delete',
                                tooltip: 'Eliminar',
                                onClick: (event, rowData: any) => {
                                    handleOnClickEliminar(rowData);
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
            <ModalExcepAgregarTipoAfiliado
                handleClose={handleCloseModalAgregaTipoAfiliado}
                open={openModalAgregarExcepTipoAfiliado}
                handleOnclickAgregar={handleOnClickAgregar}
                allActorType={simuladorState.simuladorTipoActor}
                formularioTipoActor={formularioTipoActor}
            />
            <ModalEditarExcepTipoAfiliado
                handleClose={handleCloseModalEditarTipoAfiliado}
                open={openModalEditarExcepTipoAfiliado}
                handleOnClickUpdate={handleOnClickEditar}
                allActorType={simuladorState.simuladorTipoActor}
                formularioTipoActor={formularioTipoActor}
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
    startGetAllTipoActor: () => void;
    startPostInsertSimuladorTipoActor: (
        idSimulador: string,
        simulador: SimuladorActorDto,
    ) => void;
    startPostUpdateSimuladorTipoActor: (
        simuladorActor: SimuladorActorDto,
    ) => void;
    startPostDeleteSimuladorTipoActor: (
        simuladorActor: SimuladorActorDto,
    ) => void;
    load: () => void;
    selectedSimulacion: string;
}

const mapDispatchToProps = {
    startGetAllTipoActor,
    startPostInsertSimuladorTipoActor,
    startPostUpdateSimuladorTipoActor,
    startPostDeleteSimuladorTipoActor,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(TableExcepTipoActor);
