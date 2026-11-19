import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import MaterialTable from 'material-table';
import { useForm } from 'react-hook-form';
import { SimuladorState } from '../../../../reducers/simulador.reducer';
import { AppState } from '../../../../store/rootReducer';
import { startGetAllUnidadEjecutora } from '../../../../actions/simulador.action';
import ModalPlazosAgregarExcepUnidadEjecutora from './componentsModal/ModalPlazosAgregarExcepUnidadEjecutora';
import ModalPlazosEditarExcepUnidadEjecutora from './componentsModal/ModalPlazosEditarExcepUnidadEjecutora';
import { ParametrizacionState } from '../../../../reducers/parametrization.reducer';
import { PlazoUndejDto } from '../../../../types/parametrization.model.type';
import {
    startPostInsertParametrizationPlazosExcepUniej,
    startPostUpdateParametrizationPlazoUniej,
    startPostDeleteParametrizationPlazoUniej,
} from '../../../../actions/parametrization.action';
import NotificationService from '../../../../services/notification.service';

const ExcepPlazosUnidadEjecutora = (props: props) => {
    const {
        simuladorState,
        parametrizationState,
        tipoCreditoSelected,
        loadUnidadEjecutora,
        startGetAllUnidadEjecutora,
        startPostInsertParametrizationPlazosExcepUniej,
        startPostUpdateParametrizationPlazoUniej,
        startPostDeleteParametrizationPlazoUniej,
    } = props;

    useEffect(() => {
        handleAllExecutorUnit();
    }, []);

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);

    const formularioPlazosUnidadEjecutora = useForm({
        defaultValues: {
            plazoId: 0,
            plazoMinimoAnio: 0,
            plazoMinimoMes: 0,
            plazoMaximoAnio: 0,
            plazoMaximoMes: 0,
            plazoEstado: false,
            unidadEjecutoraId: 1,
            nombreUnidadEjecutora: '',
            tipoCredito: 0
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

    const handleAllExecutorUnit = () => {
        startGetAllUnidadEjecutora();
    };

    const handleOpenModalAgregar = () => {
        handleAllExecutorUnit();
        formularioPlazosUnidadEjecutora.reset();
        setOpenModalAgregar(true);
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleOpenModalEditar = (rowData: any) => {
        formularioPlazosUnidadEjecutora.reset();
        handleAllExecutorUnit();
        formularioPlazosUnidadEjecutora.setValue('plazoId', rowData.plazoId);
        formularioPlazosUnidadEjecutora.setValue('plazoMinimoAnio', rowData.plazoMinimoAnio);
        formularioPlazosUnidadEjecutora.setValue('plazoMinimoMes', rowData.plazoMinimoMes);
        formularioPlazosUnidadEjecutora.setValue('plazoMaximoAnio', rowData.plazoMaximoAnio);
        formularioPlazosUnidadEjecutora.setValue('plazoMaximoMes', rowData.plazoMaximoMes);
        formularioPlazosUnidadEjecutora.setValue('unidadEjecutoraId', rowData.unidadEjecutoraId);
        formularioPlazosUnidadEjecutora.setValue('nombreUnidadEjecutora', rowData.nombreUnidadEjecutora);
        formularioPlazosUnidadEjecutora.setValue('tipoCredito', rowData.tipoCredito);
        setOpenModalEditar(true);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };

    const setNewData = async () => {
        await loadUnidadEjecutora();
    };

    const handleOnClickAgregar = async (data: any) => {
        const datosPlazosUnidadEjecutora: PlazoUndejDto = {
            plazoId: 0,
            plazoMinimoAnio: data.plazoMinimoAnio,
            plazoMinimoMes: data.plazoMinimoMes,
            plazoMaximoAnio: data.plazoMaximoAnio,
            plazoMaximoMes: data.plazoMaximoMes,
            plazoEstado: data.plazoEstado,
            unidadEjecutoraId: data.unidadEjecutoraId,
            nombreUnidadEjecutora: data.nombreUnidadEjecutora,
            tipoCredito: tipoCreditoSelected
        }

        const isConfirmed = await swal({
            title: 'Confirmación',
            text: '¿Está seguro de que desea actualizar la información?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
            dangerMode: true,
        });

        if (!isConfirmed) {
            return; // ❌ No actualiza
        }

        await startPostInsertParametrizationPlazosExcepUniej(
            datosPlazosUnidadEjecutora,
        );
        setNewData();
        handleCloseModalAgregar();
        toast.success('Información Guardada');
    };

    const handleOnClickEditar = async (data: any) => {

        const unidadEjecutora = simuladorState.simuladorunidadEjecutora.find(item => item.idUnidadEjecutora === data.unidadEjecutoraId)

        const datosPlazosUnidadEjecutora: PlazoUndejDto = {
            plazoId: data.plazoId,
            plazoMinimoAnio: data.plazoMinimoAnio,
            plazoMinimoMes: data.plazoMinimoMes,
            plazoMaximoAnio: data.plazoMaximoAnio,
            plazoMaximoMes: data.plazoMaximoMes,
            plazoEstado: data.plazoEstado,
            unidadEjecutoraId: data.unidadEjecutoraId,
            nombreUnidadEjecutora: unidadEjecutora?.nombreUnidadEjecutora || '',
            tipoCredito: 0
        }
        await startPostUpdateParametrizationPlazoUniej(
            datosPlazosUnidadEjecutora,
        );
        setNewData();
        handleCloseModalEditar();
        await NotificationService.genericAlert(
            'Información Actualizada',
            'La actualización solo aplicara para créditos nuevos',
        );
    };

    const deleteState = async (plazoUniej: PlazoUndejDto) => {
        await startPostDeleteParametrizationPlazoUniej(plazoUniej);
        setNewData();
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
                        data={parametrizationState.plazosUniej}
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
            <ModalPlazosAgregarExcepUnidadEjecutora
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                allExecutorUnit={simuladorState.simuladorunidadEjecutora}
                handleOnClickAgregar={handleOnClickAgregar}
                formularioPlazosUnidadEjecutora={formularioPlazosUnidadEjecutora}
            />
            <ModalPlazosEditarExcepUnidadEjecutora
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                allExecutorUnit={simuladorState.simuladorunidadEjecutora}
                handleOnClickEditar={handleOnClickEditar}
                formularioPlazosUnidadEjecutora={formularioPlazosUnidadEjecutora}
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
    startGetAllUnidadEjecutora: () => void;
    startPostInsertParametrizationPlazosExcepUniej: (
        plazoUniej: PlazoUndejDto,
    ) => void;
    startPostUpdateParametrizationPlazoUniej: (
        plazoUniej: PlazoUndejDto,
    ) => void;
    startPostDeleteParametrizationPlazoUniej: (
        plazoUniej: PlazoUndejDto,
    ) => void;
    loadUnidadEjecutora: () => void;
    tipoCreditoSelected: number;
}

const mapDispatchToProps = {
    startGetAllUnidadEjecutora,
    startPostInsertParametrizationPlazosExcepUniej,
    startPostUpdateParametrizationPlazoUniej,
    startPostDeleteParametrizationPlazoUniej,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ExcepPlazosUnidadEjecutora);
