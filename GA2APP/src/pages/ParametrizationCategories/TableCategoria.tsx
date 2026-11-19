import React, { useState } from 'react';
import swal from 'sweetalert';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import MaterialTable from 'material-table';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { AppState } from '../../store/rootReducer';
import { ModalAgregarCategoria } from './components/ModalAgregarCategoria';
import { ModalEditarCategoria } from './components/ModalEditarCategoria';
import { CatalogState } from '../../reducers/catalog.reducer';
import { CATEGORIES } from '../../types/catalog.type';
import {
    startPostInsertCategory,
    startUpdateCategory,
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

const TableCategoria = (props: props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const { catalogState, load, startPostInsertCategory, startUpdateCategory } =
        props;

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);

    const formularioCategoria = useForm ({
        defaultValues:{
            id: 0,
            nombre: '',
            descripcion: '',
            expresion: '',
            esEditable: true,
        }
    })

    const columns: any = [
        {
            title: 'Categoria',
            field: 'descripcion',
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
    ];

    const handleOpenModalAgregar = async () => {
        formularioCategoria.reset();
        setOpenModalAgregar(true);
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleOpenModalEditar = async (rowData: any) => {
        formularioCategoria.reset();
        setOpenModalEditar(true);
        formularioCategoria.setValue('id',rowData.id);
        formularioCategoria.setValue('descripcion',rowData.descripcion);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };

    const setNewData = async () => {
        await load();
    };

    const handleAgregar = async (data: any) => {
            const datosCategoria: CATEGORIES= {
                id: 0,
                nombre: '',
                descripcion: String(data.descripcion).toLocaleUpperCase(),
                expresion: '',
                esEditable: true,
            } 
      
            await startPostInsertCategory(datosCategoria);
            setNewData();
            toast.success('Información Guardada');
            handleCloseModalAgregar();
    };

    const handleEditar = async (data: any) => {

        const datosCategoria: CATEGORIES= {
            id: data.id,
            nombre: '',
            descripcion: String(data.descripcion).toLocaleUpperCase(),
            expresion: '',
            esEditable: true,
        } 
       
        await startUpdateCategory(datosCategoria);
        setNewData();
        toast.success('Información Actualizada');
        handleCloseModalEditar();
    };

    const handleOnClickEliminar = (rowData: any) => {
        swal({
            text: '¿Está seguro que desea eliminar el registro?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                eliminarCategorias(rowData.id);
            }
        });
    };

    const eliminarCategorias = async (id: number) => {
        await apiCatalog
            .postDeleteCategoria(
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
                    data={catalogState.categories}
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
            <ModalAgregarCategoria
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                handleAgregar={handleAgregar}
                formularioCategoria={formularioCategoria}
            />
            <ModalEditarCategoria
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                handleEditar={handleEditar}
                formularioCategoria={formularioCategoria}
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
    startPostInsertCategory: (category: CATEGORIES) => void;
    startUpdateCategory: (category: CATEGORIES) => void;
}

const mapDispatchToProps = {
    startPostInsertCategory,
    startUpdateCategory,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableCategoria);
