import { createStyles, makeStyles } from '@material-ui/core';
import MUIDataTable from 'mui-datatables';
import React from 'react';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import UtilService from '../../../utils/util.service';

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '110px',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
            
        },
    }),
);

function TablaListaCartera ({listaCarteraGeneral}:
    {listaCarteraGeneral:any}) {
    
        const classes = useStyles();
        const columns = [
            {
              name: 'numeroRadicacion',
              label: '# Radicado' ,
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
            },    
            },
            {
                name: 'pagare',
                label: 'Pagare',
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
                name: 'cliente',
                label: 'Cliente',
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
                name: 'estado',
                label: 'Estado',
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
                name: 'identificacion',
                label: 'Identificacion',
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
                name: 'nombres',
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
                name: 'pagaduria',
                label: '#Pagaduria',
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
                name: 'nombrePagaduria',
                label: 'Nombre Pagaduria',
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
                name: 'apertura',
                label: 'Apertura',
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
                name: 'terminacion',
                label: 'Terminación',
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
                name: 'tasaLinea',
                label: 'Tasa Linea',
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
                name: 'destinacion',
                label: 'Destinacion',
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
                name: 'desembolso',
                label: 'Desembolso',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        className: classes.columnDate,
                        style: {
                            textAlign: 'right',
                            
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
                name: 'monto',
                label: 'Monto',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'saldo',
                label: 'Saldo',
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
                name: 'cuota',
                label: 'Cuota',
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
                name: 'dias',
                label: 'Dias', 
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
                name: 'ultimoPago',
                label: 'Ultimo Pago',
                options: {
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                    setCellProps: () => ({
                        className: classes.columnDate,
                    }),
                    customBodyRender: (value: string) => UtilService.getDate(value),
                },
            },
            {
                name: 'próximoPago',
                label: 'Próximo Pago',
                options: {
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                    setCellProps: () => ({
                        className: classes.columnDate,
                    }),
                    customBodyRender: (value: string) => UtilService.getDate(value),
                },
            },
            {
                name: 'moraInterior',
                label: 'Mora Interior', 
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
                name: 'interes',
                label: 'Interes',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'seguros',
                label: 'Seguros', 
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                label: 'Capital',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'totalPagar',
                label: 'Total Pagar',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'valorInmueble',
                label: 'Valor Inmueble',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'plazoObligacion',
                label: 'Plazo Obligacion ',
                options: {                   
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                },            
            },   
            {
                name: 'calificacion',
                label: 'Calificacion',
                options: {                   
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                },                         
            },   
            {
                name: 'numeroCuotasFaltantes',
                label: '#Cuotas Faltantes',
                options: {                   
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                },                         
            },
            {
                name: 'valorCuotaExtraordinaria',
                label: 'valor Cuota Extraordinaria',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'valorOpcionCompra',
                label: 'Valor Opcion Compra',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right',
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
                name: 'ciudadInmueble',
                label: 'ciudad Inmueble',
                options: {               
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                },                  
            },
            {
                name: 'puntajeScoring',
                label: 'Puntaje Scoring' ,
                options: {               
                    setCellHeaderProps: () => ({
                        style: {
                            backgroundColor: '#FEC917',
                            color: '#676767',
                        },
                    }),
                },                              
            }
        ];
        return (
            <div  
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',
                
            }}>      
            <CHPageTitle
                topText="Reporte General de Cartera."
                subText=""
                title1="Reporte General de Cartera. "
                title2="Cartera."
            />
            <MUIDataTable
            title=""
            data={listaCarteraGeneral}
            columns={columns}
            options={{
                selectableRows: 'none',
                responsive: 'vertical',
                filter: false,
                print: false,
                elevation: 0,
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
                downloadOptions: {
                    filename: 'Reporte general cartera.csv',
                    separator: ',',
                },
            }}
            />
            </div>
     
        );
    
}
export default TablaListaCartera;