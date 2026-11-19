import React from 'react';
import { Grid } from '@material-ui/core';
import { connect } from 'react-redux';
import MaterialTable from 'material-table';
import { RemoveCircleOutlineOutlined } from '@material-ui/icons';
import swal from 'sweetalert';
// import { toast } from 'react-toastify';
import { AppState } from '../../store/rootReducer';
import { EmbargosState } from '../../reducers/embargos.reducer';
import { EmbargoConceptoDto } from '../../types/embargo.model.type';
import {
    // startDeleteEmbargoConcepto,
    startPostInsertDesembargo,
} from '../../actions/embargos.action';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';

const TableEmbargoConceptos = (props: props) => {
    const {
        embargoState,
        load,
        // startDeleteEmbargoConcepto,
        startPostInsertDesembargo,
        onClickDetalleEmbargo,
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
            title: 'Cuenta de afiliado',
            field: 'numeroCuenta',
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
            title: 'Tipo de embargo',
            field: 'tipoembargo',
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
            title: 'Valor de embargo',
            field: 'valor',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) =>
                rowData.tipoembargo === 'Valor' ? (
                    UtilService.formatCurrency(rowData.valor)
                ) : (
                    <span>{`${rowData.valor}%`}</span>
                ),
        },
        {
            title: 'Fecha Inicio Embargo',
            field: 'fechaInicio',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) => UtilService.getDate(rowData.fechaInicio),
        },
        {
            title: 'Fecha Fin Embargo',
            field: 'fechaFin',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) => UtilService.getDate(rowData.fechaFin),
        },
        {
            title: 'Tipo de Retención',
            field: 'tipoRetencion',
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
            title: 'Indicador Cesantías',
            field: 'indicadorCesantias',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) =>
                rowData.indicadorCesantias === true ? 'SI' : 'NO',
        },
    ];

    const desembargoCuenta = async (embargoConcepto: EmbargoConceptoDto) => {
        await startPostInsertDesembargo(embargoConcepto);
        load();
        await NotificationService.genericAlert(
            'Desembargo de cuenta',
            'Desembargo de cuenta realizado con éxito',
        );
    };

    const handleOnClick = (embargoConcepto: any) => {
        swal({
            title: 'Desembargo de cuenta',
            text: '¿Está seguro que desea realizar esta acción?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                desembargoCuenta(embargoConcepto);
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
                        data={embargoState.embargosConcepto}
                        columns={columns}
                        actions={[
                            {
                                icon: () => <RemoveCircleOutlineOutlined />,
                                tooltip: 'Desembargo',
                                onClick: (event, rowData: any) => {
                                    handleOnClick(rowData);
                                },
                            },
                            {
                                icon: 'note',
                                tooltip: 'Detalle',
                                onClick: (event, rowData: any) => {
                                    onClickDetalleEmbargo(rowData.embardoId);
                                },
                            },
                            // {
                            //     icon: 'delete',
                            //     tooltip: 'Eliminar',
                            //     onClick: (event, rowData: any) => {
                            //         handleOnClickDelete(rowData);
                            //     },
                            // },
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
        </>
    );
};
interface LinkMapStateToProps {
    embargoState: EmbargosState;
}

const mapStateToProps = (state: AppState) => ({
    embargoState: state.embargo,
});
interface LinkMapDispatchToProps {
    onClickDetalleEmbargo: (value: any) => void;
    load: () => void;
    // startDeleteEmbargoConcepto: (embargo: EmbargoConceptoDto) => void;
    startPostInsertDesembargo: (embargoConcepto: EmbargoConceptoDto) => void;
}

const mapDispatchToProps = {
    // startDeleteEmbargoConcepto,
    startPostInsertDesembargo,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(TableEmbargoConceptos);
