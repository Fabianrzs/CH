import React from 'react';
import MaterialTable from 'material-table';
import UtilService from '../../utils/util.service';
import { EmbargosState } from '../../reducers/embargos.reducer';

const TableDesembargosCuentas = (props: props) => {
    const { embargoState } = props;
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
            field: 'cuenta',
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
            title: 'Radicado Juzgado',
            field: 'radicadoJuzgado',
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
            title: 'Expediente Cuenta Agrario',
            field: 'expedienteBancoAgrario',
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
            field: 'tipoEmbargo',
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
            title: 'Tipo Proceso',
            field: 'tipoProceso',
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
            render: (rowData: any) => UtilService.formatCurrency(rowData.valor),
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
            title: 'Juzgado',
            field: 'juzgado',
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
            title: 'Nombre Demandante',
            field: 'nombreDemandante',
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
            title: 'Nombre Demandado',
            field: 'nombreDemandado',
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
        <MaterialTable
            style={{
                borderTopLeftRadius: '12px',
                borderTopRightRadius: '12px',
            }}
            title=""
            data={embargoState.embargosConcepto}
            columns={columns}
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
    );
};

interface LinkMapStateToProps {
    embargoState: EmbargosState;
}

type props = LinkMapStateToProps;

export default TableDesembargosCuentas;
