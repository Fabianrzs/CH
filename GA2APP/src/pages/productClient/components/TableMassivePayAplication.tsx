import React from 'react';
import { Grid } from '@material-ui/core';
import MUIDataTable from 'mui-datatables';
import { ProductState } from '../../../reducers/producto.reducer';
import UtilService from '../../../utils/util.service';

const TableMassivePayAplication = (props: props) => {
    const { productState } = props;
    const columns = [
        {
            name: 'fecha_proceso',
            label: 'Fecha de Cierre',
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),

                setCellProps: () => ({
                    style: {
                        textAlign: 'center'
                       
                    },
                }),

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
            name: 'fecha_aplicacion',
            label: 'Fecha Aplicación',
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
               
                setCellProps: () => ({
                    style: {
                        textAlign: 'center'
                       
                    },
                }),

                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'concepto_Cód',
            label: 'Código de concepto',
            options: {
                customBodyRender: (value: string) =>
                    value,
                    setCellProps: () => ({
                        style: {
                            textAlign: 'center'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                   
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
            name: 'documento',
            label: 'Documento',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',    
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
            name: 'funcionario',
            label: 'Funcionario',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                     
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
            name: 'valor',
            label: 'Valor Pago',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                 
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'seguro_hogar',
            label: 'Abono Seguro Hogar',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'seguro_vida',
            label: 'Abono Seguro Vida',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'interes_cte',
            label: 'Abono Interes Corriente',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'interes_mora',
            label: 'Abono Interes Mora',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'capital',
            label: 'Abono Capital',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: "valorAplicado",
            label: "Valor aplicado",
            options: {
                customBodyRenderLite: (dataIndex: any, rowIndex: any) =>
                    UtilService.formatCurrency(
                        productState.aplicacionMasivaPagos[rowIndex].seguro_hogar +
                        productState.aplicacionMasivaPagos[rowIndex].interes_cte + 
                        productState.aplicacionMasivaPagos[rowIndex].interes_mora +
                        productState.aplicacionMasivaPagos[rowIndex].capital),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            }
        },
        {
            name: 'observaciones',
            label: 'Observaciones',
            options: {
                customBodyRender: (value: string) =>
                    value,
                    setCellProps: () => ({
                        style: {
                            textAlign: 'left'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                        borderTopRightRadius: '12px',
                    },
                }),
            },
        },
    ];

    return (
        <>
            {productState.aplicacionMasivaPagos ? (
                <Grid container>
                    <Grid item xs={12}>
                        <MUIDataTable
                            title=""
                            data={productState.aplicacionMasivaPagos}
                            columns={columns}
                            options={{
                                selectableRows: 'none',
                                responsive: 'vertical',
                                filter: false,
                                print: false,
                                elevation: 0,
                                fixedHeader: true,
                                fixedSelectColumn: true,
                                tableBodyHeight: '420px',
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
                    </Grid>
                </Grid>
            ) : null}
        </>
    );
};

interface LinkMapDispatchToProps {
    productState: ProductState;
}

type props = LinkMapDispatchToProps;

export default TableMassivePayAplication;
