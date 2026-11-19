import MUIDataTable from 'mui-datatables';
import React from 'react';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import UtilService from '../../../utils/util.service';

const TablaReporteEstadoIntegraciones = (props: Props) => {
    const { data } = props;

    const columns = [
        {
            name: 'tidiD',
            label: 'Tipo Identificación',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'identificacion',
            label: 'Número Identificación',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'pNombre',
            label: 'Nombres',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'sNombre',
            label: ' ',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'pApellidos',
            label: 'Apellidos',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'sApellidos',
            label: ' ',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fecha',
            label: 'Fecha de Ejecución',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'resultado',
            label: 'Resultado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Usuario que Ejecuta',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Estado Integración"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Reporte-Estado-Integraciones.csv',
                        separator: ',',
                    },
                    textLabels: {
                        body: {
                            noMatch: 'No se encontraron resultados',
                          },
                        pagination: {
                            displayRows: 'de',
                            next: 'Siguiente',
                            previous: 'Anterior',
                            rowsPerPage: 'Filas por página',
                            jumpToPage: '',
                        },
                        toolbar: {
                            downloadCsv: 'Descargar CSV',
                            search: 'Buscar',
                            viewColumns: 'Mostrar Columnas',
                        },
                        viewColumns: {
                            title: 'Mostrar Columnas',
                        },
                    },
                }}
            />
        </>
    );
};

interface LinkMapStateToProps {
    data: any[];
}

type Props = LinkMapStateToProps;

export default TablaReporteEstadoIntegraciones;
