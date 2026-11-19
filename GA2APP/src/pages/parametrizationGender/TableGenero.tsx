import React, { useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import MaterialTable from 'material-table';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { AppState } from '../../store/rootReducer';
import { ModalAgregarGenero } from './components/ModalAgregarGenero';
import { ModalEditarGenero } from './components/ModalEditarGenero';
import { CatalogState } from '../../reducers/catalog.reducer';
import { SEXODTO } from '../../types/catalog.type';
import {
    startPostInsertTypeGenero,
    startUpdateTypeGenero,
} from '../../actions/catalog.action';
import ApiCatalog from '../../services/api-catalog.service';
import NotificationService from '../../services/notification.service';

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

const TableGenero = (props: props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const {
        catalogState,
        load,
        startPostInsertTypeGenero,
        startUpdateTypeGenero,
    } = props;

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);

    const formularioGenero = useForm({
        defaultValues: {
            id: 0,
            idCatalogo: 0,
            codigo: '',
            descripcion: '',
            estado: true,
        }
    })

    const columns: any = [
        {
            title: 'Abreviatura',
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
            title: 'Descripción',
            field: 'descripcion',
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

    const handleOpenModalAgregar = () => {
        setOpenModalAgregar(true);
        formularioGenero.reset();
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleOpenModalEditar = (rowData: any) => {
        setOpenModalEditar(true);
        formularioGenero.setValue('id', rowData.id);
        formularioGenero.setValue('idCatalogo', rowData.idCatalogo);
        formularioGenero.setValue('codigo', rowData.codigo);
        formularioGenero.setValue('descripcion', rowData.descripcion);
        formularioGenero.setValue('estado', rowData.estado);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };


    const setNewData = async () => {
        await load();
    };

    const handleAgregar = async (data: any) => {

            const datosGenero: SEXODTO = {
                id: data.id,
                idCatalogo: data.idCatalogo,
                codigo: String(data.codigo).toLocaleUpperCase(),
                descripcion: String(data.descripcion).toLocaleUpperCase(),
                estado: data.estado
            }
            await startPostInsertTypeGenero(datosGenero);
            setNewData();
            toast.success('Información Guardada');
            handleCloseModalAgregar();
    };

    const handleEditar = async (data: any) => {
        const datosGenero: SEXODTO = {
            id: data.id,
            idCatalogo: data.idCatalogo,
            codigo: String(data.codigo).toLocaleUpperCase(),
            descripcion: String(data.descripcion).toLocaleUpperCase(),
            estado: data.estado
        }

            await startUpdateTypeGenero(datosGenero);
            setNewData();
            toast.success('Información Actualizada');
            handleCloseModalEditar();
    };

    const handleEliminar = (rowData: any) => {
        swal({
            text: '¿Está seguro que desea eliminar el registro?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                eliminarGenero(rowData.idCatalogo);
            }
        });
    }

    const eliminarGenero = async (id: number) => {
        await apiCatalog
            .postDeleteGenero(
              id
            )
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        await NotificationService.genericAlert(
                            'Eliminado con éxito',
                            'Se realizó con éxito la eliminación del registro.',
                        );
                        setNewData();
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
            <Grid className={classes.column} item xs={12}>
                <MaterialTable
                    style={{
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '12px',
                    }}
                    title=""
                    data={catalogState.sextypes}
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
                                handleEliminar(rowData);
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
            </Grid>
            <ModalAgregarGenero
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                handleAgregar={handleAgregar}
                formularioGenero={formularioGenero}
            />
            <ModalEditarGenero
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                handleEditar={handleEditar}
                formularioGenero={formularioGenero}
            />
        </Grid>
    );
};
interface LinkMapStateToProps {
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});
interface LinkMapDispatchToProps {
    load: () => void;
    startPostInsertTypeGenero: (sextype: SEXODTO) => void;
    startUpdateTypeGenero: (sextype: SEXODTO) => void;
}
const mapDispatchToProps = {
    startPostInsertTypeGenero,
    startUpdateTypeGenero,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableGenero);
