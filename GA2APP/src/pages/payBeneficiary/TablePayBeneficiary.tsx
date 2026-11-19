import React from 'react';
import { Grid } from '@material-ui/core';
import MaterialTable from 'material-table';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { AppState } from '../../store/rootReducer';
import { ModalAddPayBeneficiary } from './components/ModalAddPayBeneficiary';
import { ModalEditPayBeneficiary } from './components/ModalEditPayBeneficiary';
import { CatalogState } from '../../reducers/catalog.reducer';
import { EmbargosState } from '../../reducers/embargos.reducer';
import { startDeleteEmbargoBeneficiario } from '../../actions/embargos.action';
import { EmbargoBeneficiariosDto } from '../../types/embargo.model.type';

const TablePayBeneficiary = (props: props) => {
    const {
        catalogState,
        embargoState,
        load,
        handleOnChangeEdit,
        handleOnChangeModo,
        startDeleteEmbargoBeneficiario,
    } = props;

    const columns: any = [
        {
            title: 'Radicado Work Manager',
            field: 'radicadoWorkManager',
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
            title: 'Tipo de Identificación',
            field: 'tipoIdentificacion',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'No. Identificación',
            field: 'numeroIdentificacion',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            field: 'primerNombre',
            align: 'center',
            cellStyle: {
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
                border: '0',
            },
        },
        {
            title: 'Nombres y Apellidos',
            field: 'segundoNombre',
            align: 'center',
            cellStyle: {
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
                border: '0',
            },
        },
        {
            field: 'primerApellido',
            align: 'center',
            cellStyle: {
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
                border: '0',
            },
        },
        {
            field: 'segundoApellido',
            align: 'center',
            cellStyle: {
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
                border: '0',
            },
        },
    ];

    const deleteState = async (rowData: EmbargoBeneficiariosDto) => {
        await startDeleteEmbargoBeneficiario(rowData);
        load();
        toast.success('Estado modificado con éxito');
    };

    const handleOnClickDelete = (rowData: any) => {
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
                <Grid item xs={12}>
                    <MaterialTable
                        style={{
                            borderTopLeftRadius: '12px',
                            borderTopRightRadius: '12px',
                        }}
                        title=""
                        data={embargoState.embargoBeneficiarios}
                        columns={columns}
                        actions={[
                            {
                                icon: 'edit',
                                tooltip: 'Editar',
                                onClick: (event, rowData: any) => {
                                    handleOnChangeEdit(rowData);
                                    handleOnChangeModo(true);
                                },
                            },
                            {
                                icon: 'delete',
                                tooltip: 'Eliminar',
                                onClick: (event, rowData: any) => {
                                    console.log('delete', rowData);
                                    handleOnClickDelete(rowData);
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
                            pageSizeOptions: [10, 20, 30],
                            filtering: false,
                            filterCellStyle: {
                                width: '50%',
                            },

                            pageSize: 10,
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
            <ModalAddPayBeneficiary
                tiposIdentificacion={catalogState.IdentificationType}
            />
            <ModalEditPayBeneficiary
                tiposIdentificacion={catalogState.IdentificationType}
            />
        </>
    );
};
interface LinkMapStateToProps {
    catalogState: CatalogState;
    embargoState: EmbargosState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    embargoState: state.embargo,
});
interface LinkMapDispatchToProps {
    load: () => void;
    handleOnChangeEdit: (value: any) => void;
    handleOnChangeModo: (value: boolean) => void;
    startDeleteEmbargoBeneficiario: (
        embargoBeneficiario: EmbargoBeneficiariosDto,
    ) => void;
}
const mapDispatchToProps = {
    startDeleteEmbargoBeneficiario,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(TablePayBeneficiary);
