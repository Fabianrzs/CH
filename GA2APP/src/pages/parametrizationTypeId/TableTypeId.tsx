import React, { useState } from 'react';
import swal from 'sweetalert';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import MaterialTable from 'material-table';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import { CatalogState } from '../../reducers/catalog.reducer';
import {
    startPostInsertTypeId,
    startUpdateTypeId,
} from '../../actions/catalog.action';
import { ModalAgregarTipoDocumento } from './components/ModalAgregarTipoDocumento';
import { ModalEditarTipoDocumento } from './components/ModalEditarTipoDocumento';
import { IDENTIFICATIONTYPEDTO } from '../../types/catalog.type';
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

const TableTypeId = (props: props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const { load, catalogState, startPostInsertTypeId, startUpdateTypeId } =
        props;

    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);

    const formularioTipoDocumento = useForm({
        defaultValues: {
            id: 0,
            codigo: '',
            descripcion: '',
            esEmpresarial: true,
            esActivo: true,
            ERP: 0,
            vigia: 0,
            embargo: 0,
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
    };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
        formularioTipoDocumento.reset();
    };

    const handleOpenModalEditar = (rowData: any) => {
        setOpenModalEditar(true);
        formularioTipoDocumento.setValue('id',rowData.id);
        formularioTipoDocumento.setValue('codigo',rowData.codigo);
        formularioTipoDocumento.setValue('descripcion',rowData.descripcion);
        formularioTipoDocumento.setValue('esEmpresarial',rowData.esEmpresarial);
        formularioTipoDocumento.setValue('esActivo',rowData.esActivo);
        formularioTipoDocumento.setValue('ERP',rowData.ERP);
        formularioTipoDocumento.setValue('vigia',rowData.vigia);
        formularioTipoDocumento.setValue('embargo',rowData.embargo);
    };

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
        formularioTipoDocumento.reset();
    };

    const setNewData = async () => {
        await load();
    };

    const handleAgregar = async (data: any) => {

        const datosTipoDocumento: IDENTIFICATIONTYPEDTO = {
            id: data.id,
            codigo: String(data.codigo).toLocaleUpperCase(),
            descripcion: String(data.descripcion).toLocaleUpperCase(),
            esEmpresarial: true,
            esActivo: true,
            ERP: 0,
            vigia: 0,
            embargo: 0
        }
      
        await startPostInsertTypeId(datosTipoDocumento);
        setNewData();
        toast.success('Información Guardada');
        handleCloseModalAgregar();
    };

    const handleEditar = async (data: any) => {
        const datosTipoDocumento: IDENTIFICATIONTYPEDTO = {
            id: data.id,
            codigo: String(data.codigo).toLocaleUpperCase(),
            descripcion: String(data.descripcion).toLocaleUpperCase(),
            esEmpresarial: data.esEmpresarial,
            esActivo: data.esActivo,
            ERP: data.ERP,
            vigia: data.vigia,
            embargo: data.embargo
        }
        await startUpdateTypeId(datosTipoDocumento);
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
                eliminarTipoDocumento(rowData.id);
            }
        });
    }

    const eliminarTipoDocumento = async (id: number) => {
        await apiCatalog
            .postDeleteTipoDocumento(
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
                    data={catalogState.IdentificationType}
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
            <ModalAgregarTipoDocumento
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                handleAgregar={handleAgregar}
                formularioTipoDocumento={formularioTipoDocumento}
            />
            <ModalEditarTipoDocumento
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                handleEditar={handleEditar}
                formularioTipoDocumento={formularioTipoDocumento}
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
    startPostInsertTypeId: (typeId: IDENTIFICATIONTYPEDTO) => void;
    startUpdateTypeId: (typeId: IDENTIFICATIONTYPEDTO) => void;
}
const mapDispatchToProps = {
    startPostInsertTypeId,
    startUpdateTypeId,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableTypeId);
