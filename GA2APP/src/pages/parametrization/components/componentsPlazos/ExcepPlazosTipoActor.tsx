import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import MaterialTable from 'material-table';
import { SimuladorState } from '../../../../reducers/simulador.reducer';
import { AppState } from '../../../../store/rootReducer';
import { startGetAllTipoActor } from '../../../../actions/simulador.action';
import {
    startPostInsertParametrizationPlazosExcepTipoActor,
    startPostUpdateParametrizationPlazoTipoActor,
    startPostDeleteParametrizationPlazoTipoActor,
} from '../../../../actions/parametrization.action';
import ModalPlazosAgregarExcepTipoActor from './componentsModal/ModalPlazosAgregarExcepTipoActor';
import ModalPlazosEditarExcepTipoActor from './componentsModal/ModalPlazosEditarExcepTipoActor';
import { PlazosTipoActorDtoForm } from '../../constants/form.model.type';
import { PlazoTipoActorDto } from '../../../../types/parametrization.model.type';
import { ParametrizacionState } from '../../../../reducers/parametrization.reducer';
import NotificationService from '../../../../services/notification.service';

const ExcepPlazosTipoActor = (props: props) => {
    const {
        simuladorState,
        parametrizationState,
        tipoCreditoSelected,
        loadTipoAfiliado,
        startGetAllTipoActor,
        startPostInsertParametrizationPlazosExcepTipoActor,
        startPostUpdateParametrizationPlazoTipoActor,
        startPostDeleteParametrizationPlazoTipoActor,
    } = props;

    useEffect(() => {
        handleAllActorType();
    }, []);

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);
    const [plazoTipoActor, setPlazoTipoActor] = useState<PlazoTipoActorDto>(
        PlazosTipoActorDtoForm,
    );

    const formularioPlazosTipoActor = useForm({
        defaultValues: {
            plazoId: 0,
            plazoMinimoAnio: 0,
            plazoMinimoMes: 0,
            plazoMaximoAnio: 0,
            plazoMaximoMes: 0,
            plazoEstado: false,
            idTipoActor: 1,
            descripcionTipoActor: '',
            tipoCredito: 0
        }
    })

    const columns: any = [
        {
            title: 'Tipo Afiliado',
            field: 'descripcionTipoActor',
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
            title: 'Plazo Mínimo (Meses)',
            field: 'plazoMinimoMes',
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
            title: 'Plazo Mínimo (Años)',
            field: 'plazoMinimoAnio',
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
            title: 'Plazo Máximo (Meses)',
            field: 'plazoMaximoMes',
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
            title: 'Plazo Máximo (Años)',
            field: 'plazoMaximoAnio',
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

    const handleAllActorType = () => {
        startGetAllTipoActor();
    };

    const handleOpenModalAgregar = () => {
        handleAllActorType();
        formularioPlazosTipoActor.reset();
        setOpenModalAgregar(true);
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleOpenModalEditar = (rowData: any) => {
        formularioPlazosTipoActor.reset();
        handleAllActorType();
        formularioPlazosTipoActor.setValue('plazoId', rowData.plazoId);
        formularioPlazosTipoActor.setValue('plazoMinimoAnio', rowData.plazoMinimoAnio);
        formularioPlazosTipoActor.setValue('plazoMinimoMes', rowData.plazoMinimoMes);
        formularioPlazosTipoActor.setValue('plazoMaximoAnio', rowData.plazoMaximoAnio);
        formularioPlazosTipoActor.setValue('plazoMaximoMes', rowData.plazoMaximoMes);
        formularioPlazosTipoActor.setValue('plazoEstado', rowData.plazoEstado);
        formularioPlazosTipoActor.setValue('idTipoActor', rowData.idTipoActor);
        formularioPlazosTipoActor.setValue('descripcionTipoActor', rowData.descripcionTipoActor);
        formularioPlazosTipoActor.setValue('tipoCredito', rowData.tipoCredito);
        setOpenModalEditar(true);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };

    const setNewData = async () => {
        await loadTipoAfiliado();
    };

    const handleOnClickAgregar = async (data: any) => {

        const datosPlazosTipoActor: PlazoTipoActorDto = {
            plazoId: 0,
            plazoMinimoAnio: data.plazoMinimoAnio,
            plazoMinimoMes: data.plazoMinimoMes,
            plazoMaximoAnio: data.plazoMaximoAnio,
            plazoMaximoMes: data.plazoMaximoMes,
            plazoEstado: data.plazoEstado,
            idTipoActor: data.idTipoActor,
            descripcionTipoActor: data.descripcionTipoActor,
            tipoCredito: tipoCreditoSelected
        }
        await startPostInsertParametrizationPlazosExcepTipoActor(
            datosPlazosTipoActor,
        );
        setNewData();
        handleCloseModalAgregar();
        toast.success('Información Guardada');
    };

    const handleOnClickEditar = async (data: any) => {
        const tipoActor = simuladorState.simuladorTipoActor.find(item => item.idTipoActor === data.idTipoActor);
        const datosPlazosTipoActor: PlazoTipoActorDto = {
            plazoId: data.plazoId,
            plazoMinimoAnio: data.plazoMinimoAnio,
            plazoMinimoMes: data.plazoMinimoMes,
            plazoMaximoAnio: data.plazoMaximoAnio,
            plazoMaximoMes: data.plazoMaximoMes,
            plazoEstado: data.plazoEstado,
            idTipoActor: data.idTipoActor,
            descripcionTipoActor: tipoActor?.descripcionTipoActor || '',
            tipoCredito: data.tipoCredito
        }

        const isConfirmed = await swal({
            title: 'Confirmación',
            text: '¿Está seguro de que desea actualizar la información?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
            dangerMode: true,
        });

        if (!isConfirmed) {
            return;
        }
        await startPostUpdateParametrizationPlazoTipoActor(datosPlazosTipoActor);
        setNewData();
        setPlazoTipoActor(PlazosTipoActorDtoForm);
        handleCloseModalEditar();
        await NotificationService.genericAlert(
            'Información Actualizada',
            'La actualización solo aplicara para créditos nuevos',
        );
    };

    const deleteState = async (plazoTipoActor: PlazoTipoActorDto) => {
        await startPostDeleteParametrizationPlazoTipoActor(plazoTipoActor);
        setNewData();
        setPlazoTipoActor(PlazosTipoActorDtoForm);
        toast.success('Estado modificado con éxito');
    };

    const handleOnClickEliminar = (rowData: any) => {
        swal({
            text: 'El estado del registro se modificará como INACTIVO.¿Desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                deleteState(rowData);
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
                    onClick={handleOpenModalAgregar}
                >
                    AGREGAR
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
                        data={parametrizationState.plazosTipoActor}
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
                            pageSizeOptions: [2, 5, 10],
                            pageSize: 2,
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
            <ModalPlazosAgregarExcepTipoActor
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                allActorType={simuladorState.simuladorTipoActor}
                handleOnClickAgregar={handleOnClickAgregar}
                formularioPlazosTipoActor={formularioPlazosTipoActor}
            />
            <ModalPlazosEditarExcepTipoActor
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                allActorType={simuladorState.simuladorTipoActor}
                handleOnClickEditar={handleOnClickEditar}
                formularioPlazosTipoActor={formularioPlazosTipoActor}
            />
        </>
    );
};

interface LinkMapStateToProps {
    simuladorState: SimuladorState;
    parametrizationState: ParametrizacionState;
}

const mapStateToProps = (state: AppState) => ({
    simuladorState: state.simulador,
    parametrizationState: state.parametrization,
});

interface LinkMapDispatchToProps {
    startGetAllTipoActor: () => void;
    startPostInsertParametrizationPlazosExcepTipoActor: (
        plazoTipoActor: PlazoTipoActorDto,
    ) => void;
    startPostUpdateParametrizationPlazoTipoActor: (
        plazoTipoActor: PlazoTipoActorDto,
    ) => void;
    startPostDeleteParametrizationPlazoTipoActor: (
        plazoTipoActor: PlazoTipoActorDto,
    ) => void;
    loadTipoAfiliado: () => void;
    tipoCreditoSelected: number;
}

const mapDispatchToProps = {
    startGetAllTipoActor,
    startPostInsertParametrizationPlazosExcepTipoActor,
    startPostUpdateParametrizationPlazoTipoActor,
    startPostDeleteParametrizationPlazoTipoActor,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ExcepPlazosTipoActor);
