import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Grid } from '@material-ui/core';
import swal from 'sweetalert';
import MaterialTable from 'material-table';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import ModalEditarExcepSeguroHogar from './componentsModal/ModalEditarExcepSeguroHogar';
import { SeguroRegionForm } from '../../constants/form.model.type';
import ApiParametrization from '../../../../services/api-parametrization.service';
import NotificationService from '../../../../services/notification.service';
import { startGetAllParametrizationSeguroRegion } from '../../../../actions/parametrization.action';
import { ParametrizacionState } from '../../../../reducers/parametrization.reducer';

const ExcepSeguroHogar = (props: props) => {
    const apiParametrization = ApiParametrization();
    const { parametrizacionState, startGetAllParametrizationSeguroRegion } = props;
    const [openModalEditar, setModalEditar] = useState(false);
    const [arregloSeguros, setArregloSeguros] = useState<any[]>([]);

    const formularioSeguroUbicacion = useForm({
        defaultValues: SeguroRegionForm,
    });

    const columns: any = [
        {
            title: 'Región',
            field: 'nombre',
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
        // {
        //     title: 'Factor Tarifa (%)',
        //     field: 'seguro',
        //     align: 'center',
        //     cellStyle: {
        //         border: '1.5px solid #FFF',
        //         color: '#676767',
        //     },
        //     headerStyle: {
        //         borderTopRightRadius: '0px',
        //     },
        // },
        {
            title: 'Seguro (%)',
            field: 'seguro',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        // {
        //     title: 'Factor IVA (%)',
        //     field: 'columnaFactorIva',
        //     align: 'center',
        //     cellStyle: {
        //         border: '1.5px solid #FFF',
        //         color: '#676767',
        //     },
        //     headerStyle: {
        //         borderTopRightRadius: '0px',
        //     },
        // },
    ];

    useEffect(() => {
        startGetAllParametrizationSeguroRegion();
        }, []);

    useEffect(() => {
          const newData = [...parametrizacionState.segurosRegion];
          const newDataWithColumn = newData.map((item) => {
            const arregloDatos = {
              ...item,
              columnaFactorPeriodico: (item.seguro /12).toFixed(6),
              columnaFactorIva: ((item.seguro /12)*1.19).toFixed(6)
              
            };
            return arregloDatos; 
          });

          setArregloSeguros(newDataWithColumn)

        }, [parametrizacionState]);


    const handleOpenModalEditar = (rowData: any) => {
        formularioSeguroUbicacion.setValue('id', rowData.id);
        formularioSeguroUbicacion.setValue('nombre', rowData.nombre);
        formularioSeguroUbicacion.setValue('seguro', rowData.seguro);


        setModalEditar(true);
    };

    const handleCloseModalEditar = () => {
        setModalEditar(false);
        formularioSeguroUbicacion.clearErrors('seguro');
    };

    // const handleOnClickEditar = async (data: any) => {
    //     const request = {
    //         id: data.id,
    //         seguro: Number(data.seguro),
    //     };

        
    //     actualizarSeguro(request);
    //     handleCloseModalEditar();
    // };

    const handleOnClickEditar = async (data: any) => {
        const seguro = Number(
          String(data.seguro).replace(',', '.')
        );
      
        if (Number.isNaN(seguro) || seguro >= 1) {
          toast('El seguro debe ser un valor menor a 1', {
            type: toast.TYPE.ERROR,
          });
          return;
        }

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
      
        actualizarSeguro({
          id: data.id,
          seguro,
        });
      
        handleCloseModalEditar();
      };
      

    const actualizarSeguro = async (request: any) => {
        await apiParametrization
            .PutParametrizationSeguroRegion(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        await NotificationService.genericAlert(
                            'Actualizado con Éxito',
                            'Se actualizó la solicitud.',
                        );
                        startGetAllParametrizationSeguroRegion();
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
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
                <Grid item xs={12}>
                    <MaterialTable
                        style={{
                            borderTopLeftRadius: '12px',
                            borderTopRightRadius: '12px',
                        }}
                        title=""
                        data={arregloSeguros}
                        columns={columns}
                        actions={[
                            {
                                icon: 'edit',
                                tooltip: 'Editar',
                                onClick: (event, rowData: any) => {
                                    handleOpenModalEditar(rowData);
                                },
                            },
                        ]}
                        options={{
                            actionsColumnIndex: -1,
                            search: false,
                            headerStyle: {
                                backgroundColor: '#FEC917',
                                fontWeight: 'bold',
                                color: '#676767',
                                borderColor: '#FFF',
                                border: '1.5px solid #FFF',
                                borderTopRightRadius: '12px',
                            },
                            pageSizeOptions: [5, 10, 20],
                            pageSize: 5,
                            paging: false,
                            rowStyle: (data, index) => intercalar(index),
                        }}
                        localization={{
                            header: {
                                actions: 'Actualizar',
                            },
                            toolbar: {
                                searchPlaceholder: 'Buscar',
                            },
                        }}
                    />
                </Grid>
            </Grid>
            <ModalEditarExcepSeguroHogar
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                handleOnClickEditar={handleOnClickEditar}
                formularioSeguroUbicacion={formularioSeguroUbicacion}
                parametrizationState={parametrizacionState}
            />
        </>
    );
};

interface LinkMapStateToProps {
    parametrizacionState: ParametrizacionState;
    startGetAllParametrizationSeguroRegion: () => void;
}

const mapDispatchToProps = {
    startGetAllParametrizationSeguroRegion,
};

type props = LinkMapStateToProps;

export default connect(null, mapDispatchToProps)(ExcepSeguroHogar);
