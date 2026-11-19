import React, { useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import MaterialTable from 'material-table';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { AppState } from '../../store/rootReducer';
import { ModalAgregarFuerzas } from './components/ModalAgregarFuerzas';
import { ModalEditarFuerzas } from './components/ModalEditarFuerzas';
import { CatalogState } from '../../reducers/catalog.reducer';
import { FORCEDTO } from '../../types/catalog.type';
import {
    startPostInsertForce,
    startUpdateForce,
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

const TableFuerzas = (props: props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const { load, catalogState, startPostInsertForce, startUpdateForce } =
        props;

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);

    const formularioFuerza = useForm({
        defaultValues:{
            id: 0,
            codigo: '',
            descripcion: '',
            esSoldado: false,
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
            title: 'Fuerza',
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
        {
            title: 'Soldado',
            field: 'esSoldado',
            align: 'center',
            render: (rowData: any) => (
                <span>{rowData.esSoldado === true ? 'SI' : 'NO'}</span>
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

    const handleOpenModalAgregar = () => {
        setOpenModalAgregar(true);
        formularioFuerza.reset();
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleOpenModalEditar = (rowData: any) => {
        formularioFuerza.reset();
        setOpenModalEditar(true);
        formularioFuerza.setValue('id',rowData.id);
        formularioFuerza.setValue('codigo',rowData.codigo);
        formularioFuerza.setValue('descripcion',rowData.descripcion);
        formularioFuerza.setValue('esSoldado',rowData.esSoldado);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    };

    const setNewData = async () => {
        await load();
    };

    const handleAgregar = async (data: any) => {
        const datosFuerza: FORCEDTO = {
            id: 0,
            codigo: data.codigo.toLocaleUpperCase(),
            descripcion: String(data.descripcion).toLocaleUpperCase(),
            esSoldado: data.esSoldado,
        }
        await startPostInsertForce(datosFuerza);
        setNewData();
        toast.success('Información Guardada');
        handleCloseModalAgregar();
    };

    const handleEditar = async (data: any) => {

        const datosFuerza: FORCEDTO = {
            id: data.id,
            codigo: data.codigo.toLocaleUpperCase(),
            descripcion: String(data.descripcion).toLocaleUpperCase(),
            esSoldado: data.esSoldado,
        }
        await startUpdateForce(datosFuerza);
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
                eliminarFuerzas(rowData.id);
            }
        });
    }

    const eliminarFuerzas = async (id: number) => {
        await apiCatalog
            .postDeleteFuerza(
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
                    data={catalogState.Force}
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
            <ModalAgregarFuerzas
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                handleAgregar={handleAgregar}
                formularioFuerza={formularioFuerza}
            />
            <ModalEditarFuerzas
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                handleEditar={handleEditar}
                formularioFuerza={formularioFuerza}
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
    startPostInsertForce: (force: FORCEDTO) => void;
    startUpdateForce: (force: FORCEDTO) => void;
}

const mapDispatchToProps = {
    startPostInsertForce,
    startUpdateForce,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableFuerzas);
