import {
    Button,
    Card,
    CardContent,
    createStyles,
    Grid,
    makeStyles,
    Typography,
} from '@material-ui/core';
import MUIDataTable from 'mui-datatables';
import { Controller, useForm } from 'react-hook-form';
import React, { useEffect, useState } from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import { CreditCreateSimulationDataModel } from '../../models/credit-create-simulation-data.model';
import UtilService from '../../utils/util.service';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import ApiCredit from '../../services/api-credit.service';
import ApiReportes from '../../services/api-reportes.service';
import NotificationService from '../../services/notification.service';
import { CreditFlowDocumentsRequestModel } from '../../models/requests/credit-flow-documents-request.model';



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

export function GridDataCreditoHipotecario({
    createSimulationData,
    aplicaBeneficio,
    procedureRequest,
    estadoRestaurar,
}: {
    createSimulationData: CreditCreateSimulationDataModel[];
    aplicaBeneficio: boolean;
    procedureRequest: any;
    estadoRestaurar: any;
}) {
    const api = ApiCredit();
    const apiCredit = ApiCredit();
    const apiReportes = ApiReportes();
    const [createSimulationDataUno, setCreateSimulationDataUno] = useState<any[]>([]);


    useEffect(() => {
        ObtenerDatosSimulacion();
        
    }, [estadoRestaurar]);

    const ObtenerDatosSimulacion = async () => {
        const request = {
            smc_id: procedureRequest.simulationID,
        };
        await apiCredit
            .getObtenerDatosSimulacion(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setCreateSimulationDataUno(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };


 

    const { control } = useForm<any>({
        defaultValues: {
            fecha: new Date().toISOString().slice(0, 10),
            valorPrestamo: 0,
            plazo: 0,
            anios: 0,
        },
    });

    const classes = useStyles();
    const columns = [
        {
            name: 'cuota',
            label: 'cuota',
        },
        {
            name: 'fechaCorte',
            label: 'Fecha Corte',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {
            name: 'fechaPago',
            label: 'Fecha Pago',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
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
                        textAlign: 'right',
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
            },
        },
        {
            name: 'intereses',
            label: 'Intereses',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'seguroVida',
            label: 'Seguro Vida',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'seguroHogar',
            label: 'Seguro Hogar',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoUnico',
            label: 'Abono Único',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'valorAbonoExtra',
            label: 'Abono Extra',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'cuotaUnidadPagadora',
            label: 'Cuota Und Pagadora',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
 
        {
            name: 'cobroFonvivienda',
            label: 'Cobro Fonvivienda',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },

        aplicaBeneficio
            ? {
                  name: 'totalIntereses',
                  label: 'Total Intereses',
                  options: {
                      customBodyRender: (value: number | bigint) =>
                          UtilService.formatCurrency(value),
                      setCellProps: () => ({
                          style: {
                              textAlign: 'right',
                          },
                      }),
                  },
              }
            : '',
    ];

    const handleExportToExcel = async () => {
        const columnDataList = createSimulationDataUno.map((row) => ({
			data: [				
				String(row.cuota) || '',
				String(row.fechaCorte) || '',
				String(row.fechaPago) || '',
				String(row.saldo )|| '',
				String(row.capital )|| '',
				String(row.intereses )|| '',
				String(row.seguroVida) || '',
				String(row.seguroHogar) || '',
				String(row.abonoUnico) || '',
				String(row.valorAbonoExtra) || '',
				String(row.cuotaUnidadPagadora) || '',
				String(row.totalIntereses) || '',
				'',
			]

		}));

		if (columnDataList) { 
			const request = {
				columnDataList: columnDataList.map(item => ({
					data: item.data
				})),
				columnName: [
                "cuota", "Fecha Corte", 
                "Fecha Pago", "Saldo", 
                "Capital", "Intereses", "Seguro Vida",
                "Seguro Hogar","Abono Único",
                "Abono Extra","Cuota Und Pagadora",
                "Cobro Fonvivienda","Intereses Reducción",
                 ]
			};
			const response = await apiReportes.GenerarExcels(request);
			const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
			const url = window.URL.createObjectURL(blob);
			const link = document.createElement('a');
			link.href = url;
			link.download = `Simulacion-Fecha:${moment().format()}.csv`;
			document.body.appendChild(link);
			link.click();
			window.URL.revokeObjectURL(url);
		}
	}

    const verArchivos = async (item: string) => {
        try {
            const data = await apiCredit.postCreditDownloadFileS(item);
            const ext = UtilService.getFileExtension(item);
            if (ext) {
                const mimeType = UtilService.getMimeTypeFromExtension(ext);
                if (mimeType) {
                    const file = new Blob([data], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                }
            }
        } catch (e) {
            await NotificationService.genericAlert(
                'Warning',
                'No se puede generar archivo para este Usuarido.',
                'warning',
            );
        }
    };
    const handleExportToPdf = async () => {
            
        const request: CreditFlowDocumentsRequestModel = {
            tcR_ID: procedureRequest.creditTypeID,
            orden: procedureRequest.order,
            principal: procedureRequest.principalActivity,
            soC_ID: procedureRequest.requestID,
            afL_ID: procedureRequest.flowActivityID,
        };

        await api
        
            .postCreditFiles(request) 
            .then(async response => {

               
                if (response) {               
                    if (response.isSuccess) {
                                                
                        const docAfiliados = response.data 
                        const rutaSimulacionPdf = docAfiliados.find((x: { tdC_ID: number }) => x.tdC_ID === 83);     
                        verArchivos(rutaSimulacionPdf.rutA_IMAGEN)  
                     
                    } else {
                        await NotificationService.genericAlert(
                            'Error en actualización',
                            'Ocurrio un error al intentar actualizar, intente de nuevo.',
                            'error',
                        );
                        
                    }
                }
            })
            .catch(() => {
               
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

    
    return (
        <CHPaper title="Datos Simulación Crédito Hipotecario">
            <div
                style={{
                    display: 'table',
                    tableLayout: 'fixed',
                    width: '100%',
                }}
            >
                {createSimulationDataUno.length > 0 &&
                <Card>
                    <CardContent>
                        <Typography>
                            <Grid container>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="fecha"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                label="Fecha"
                                                value={UtilService.getDate(
                                                    value,
                                                )}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                                isReadOnly
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        currencyMask
                                        label="Valor Préstamo"
                                        name="valorPrestamo"
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        value={
                                            createSimulationDataUno[0]?.saldo
                                        }
                                        isReadOnly
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Plazo "
                                        value={createSimulationDataUno?.length - 1}
                                        isReadOnly
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Años"
                                        isReadOnly
                                        value={
                                            ((createSimulationDataUno?.length - 1) /
                                                12) %
                                                1 ===
                                            0
                                                ? (createSimulationDataUno?.length -
                                                      1) /
                                                  12
                                                : (
                                                      (createSimulationDataUno?.length -
                                                          1) /
                                                      12
                                                  ).toFixed(2)
                                        }
                                    />
                                </Grid>
                            </Grid>
                        </Typography>
                    </CardContent>
                </Card>
                }

                <div 
    style={{
        display: 'table',
        tableLayout: 'fixed',
        width: '100%',
        
    }}>


    <div className='MuiTableCell-stickyHeader; jss86'>

                <MUIDataTable
                    title=""
                    data={createSimulationDataUno}
                    columns={columns}
                    options={{
            
                        selectableRows: 'none',
                        filter: false,
                        print: false,
                        elevation: 2,
                        filterType: 'dropdown',
                        responsive: 'vertical',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        tableBodyHeight: '430px',
                        rowsPerPageOptions: [5, 15, 20, 100, 200, 500 ], 
                        textLabels: {
                            
                            body: {
                                noMatch: 'No se encontraron resultados',
                            },
                            pagination: {
                            
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
                            filename: `Simulacion-${procedureRequest.nombreCliente}_Fecha:${moment().format()}.csv`,
                            separator: ',',

                        },
                        customToolbar: () => (
                            <>
                               {/* Botón CSV */}
                            <button
                            onClick={() => handleExportToExcel()}
                            style={{
                                backgroundColor: 'green',   // Fondo azul
                                padding: '10px',           // Espacio interior
                                color: 'white',            // Color de texto blanco
                                border: 'none',            // Sin borde
                                marginRight: '10px',       // Margen derecho para separación
                                cursor: 'pointer',         // Cambia el cursor al pasar el ratón
                                borderRadius: '5px',  

                            }}
                            >
                            CSV
                            </button>
                            {/* <button
                            onClick={() => handleExportToPdf()}
                            style={{
                                backgroundColor: 'red',   // Fondo azul
                                padding: '10px',           // Espacio interior
                                color: 'white',            // Color de texto blanco
                                border: 'none',            // Sin borde
                                marginRight: '10px',       // Margen derecho para separación
                                cursor: 'pointer',         // Cambia el cursor al pasar el ratón
                                borderRadius: '5px',  
                            }}
                            >
                            PDF
                            </button> */}
                        </>
                        )
                    }}
                />
            </div>
         </div>
    </div>
        </CHPaper>
    );
}
