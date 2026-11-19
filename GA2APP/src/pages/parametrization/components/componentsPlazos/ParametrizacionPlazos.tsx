import React, { useState } from 'react';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { Grid } from '@material-ui/core';
import MaterialTable from 'material-table';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { ModalPlazosGeneral } from './componentsModal/ModalPlazosGeneral';
import { PlazoDto } from '../../../../types/parametrization.model.type';
import { ParametrizacionState } from '../../../../reducers/parametrization.reducer';
import { AppState } from '../../../../store/rootReducer';
import { startPostUpdateParametrizationPlazosGeneral } from '../../../../actions/parametrization.action';
import NotificationService from '../../../../services/notification.service';

const ParametrizacionPlazos = (props: props) => {
    const {
        load,
        parametrizacionState,
        startPostUpdateParametrizationPlazosGeneral,
    } = props;
    const [openModal, setOpenModal] = useState(false);

    const formularioParametrizacionPlazos = useForm({
        defaultValues: {
            plazoId: 0,
            plazoMinimoMes: 0,
            plazoMinimoAnio: 0,
            plazoMaximoMes: 0,
            plazoMaximoAnio: 0
        }
    })

    const columns: any = [
        {
            title: 'Línea / Tipo Crédito',
            field: 'plazoId',
            align: 'center',
            render: (rowData: any) => (
                <>
                    {rowData.plazoId === 1 ? (
                        <span> HIPOTECARIO </span>
                    ) : (
                        <span> LEASING HABITACIONAL </span>
                    )}
                </>
            ),
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

    const handleOpenModal = (rowData: any) => {
        formularioParametrizacionPlazos.reset();
        setOpenModal(true);
        formularioParametrizacionPlazos.setValue('plazoId', rowData.plazoId);
        formularioParametrizacionPlazos.setValue('plazoMinimoMes', rowData.plazoMinimoMes);
        formularioParametrizacionPlazos.setValue('plazoMinimoAnio', rowData.plazoMinimoAnio);
        formularioParametrizacionPlazos.setValue('plazoMaximoMes', rowData.plazoMaximoMes);
        formularioParametrizacionPlazos.setValue('plazoMaximoAnio', rowData.plazoMaximoAnio);
    };

    const handleCloseModal = () => {
        setOpenModal(false);
    };

    const loadList = async () => {
        await load();
    };

    const handleUpdate = async (data: any) => {
        const datosPlazosGenerales: PlazoDto = {
            plazoId: data.plazoId,
            plazoMinimoMes: data.plazoMinimoMes,
            plazoMinimoAnio: data.plazoMinimoAnio,
            plazoMaximoMes: data.plazoMaximoMes,
            plazoMaximoAnio: data.plazoMaximoAnio
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

        await startPostUpdateParametrizationPlazosGeneral(datosPlazosGenerales);
        loadList();
        handleCloseModal();
        await NotificationService.genericAlert(
            'Información Actualizada',
            'La actualización solo aplicara para créditos nuevos',
        );

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
                <Grid item xs={12}>
                    <MaterialTable
                        style={{
                            borderTopLeftRadius: '12px',
                            borderTopRightRadius: '12px',
                        }}
                        title=""
                        data={parametrizacionState.plazosGeneral}
                        columns={columns}
                        actions={[
                            {
                                icon: 'edit',
                                tooltip: 'Editar',
                                onClick: (event, rowData: any) => {
                                    handleOpenModal(rowData);
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
                            paging: false,
                            search: false,
                            toolbar: false,
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
            <ModalPlazosGeneral
                handleClose={handleCloseModal}
                open={openModal}
                handleUpdate={handleUpdate}
                formularioParametrizacionPlazos={formularioParametrizacionPlazos}
            />
        </>
    );
};

interface LinkMapStateToProps {
    parametrizacionState: ParametrizacionState;
}

const mapStateToProps = (state: AppState) => ({
    parametrizacionState: state.parametrization,
});

interface LinkMapDispatchToProps {
    load: () => void;
    startPostUpdateParametrizationPlazosGeneral: (
        plazoGeneral: PlazoDto,
    ) => void;
}

const mapDispatchToProps = {
    startPostUpdateParametrizationPlazosGeneral,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ParametrizacionPlazos);
