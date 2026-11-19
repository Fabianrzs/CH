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
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { CreditCreateSimulationDataModel } from '../../models/credit-create-simulation-data.model';
import UtilService from '../../utils/util.service';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import ApiCredit from '../../services/api-credit.service';
import ApiReportes from '../../services/api-reportes.service';
import { CreditFlowDocumentsRequestModel } from '../../models/requests/credit-flow-documents-request.model';
import NotificationService from '../../services/notification.service';

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

export function GridDataCreditoLeasing({
    createSimulationData,
    procedureRequest,
    estadoRestaurar,
}: {
    createSimulationData: CreditCreateSimulationDataModel[];
    procedureRequest: any;
    estadoRestaurar: any;
}) {


    const apiCredit = ApiCredit();
    const apiReportes = ApiReportes();

    const [createSimulationDataUno, setCreateSimulationDataUno] = useState<any[]>([]);
    const [proyecciones, setProyecciones] = useState<any>({});
    const [simulacionPersonales, setSimulacionPersonales] = useState<any>({});


    const ultimoElemento = createSimulationDataUno[createSimulationDataUno.length - 1];



    useEffect(() => {
        
        ObtenerDatosSimulacion();
        obtenerValoresProyectados();
        obtnerSimulacionPersonales();
    }, [estadoRestaurar]);

    

    const obtnerSimulacionPersonales = () => {
        apiCredit
            .getObtenerSimulacionPersonales(
                String(procedureRequest?.clientIdentificationNumber)
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        setSimulacionPersonales(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

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

    const obtenerValoresProyectados = async () => {
        await apiCredit
            .postObtenerValoresProyectados(procedureRequest.simulationID)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setProyecciones(response.data)
                        
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

    const handleClickOpen = () => {
        console.log('dfd')
    }

    const classes = useStyles();
    const columns = [
        {
            name: 'cuota',
            label: 'Cuota',
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
            name: 'saldoLeasing',
            label: 'Saldo',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'capitalpagar',
            label: 'Capital a pagar',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'capitalPorPagar',
            label: 'Diferencia de capital',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'capitalLeasing',
            label: 'Capital Total',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'capitalInteresporPagar',
            label: 'Intereses a pagar',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'interesPorPagar',
            label: 'Diferencia de intereses',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'interesLeasing',
            label: 'Intereses Totales',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'seguroHogar',
            label: 'Todo Riesgo',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'seguroVidaLeasing',
            label: 'Seguro Vida',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        // campo nuevo
        {
            name: 'segurosTotal',
            label: 'Seguros Total',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'cuotaUnidadPagadora',
            label: 'Canon a pagar',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        // nuevo campo
        {
            name: 'capitalPorPagarLeasing',
            label: 'Diferencia de Canon',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'totalCuota',
            label: 'Canon Total',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'afectacionCuentaIndividual',
            label: 'Afectación Cuenta Individual',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
        {
            name: 'capitalSaldoTotal',
            label: 'Deuda Remanente',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                }),
            },
        },
    ];
    // if (simulacionPersonales.tipoAfiliadoId !== 14) {
    //     // Agrega las columnas adicionales que deseas mostrar cuando tipoAfiliado es 14
    //     columns.push(

    //     // {
    //     //     name: 'capitalReducido',
    //     //     label: 'Saldo Reducido',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     // {
    //     //     name: 'interesPorPagar',
    //     //     label: 'Intereses Pagados',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     // {
    //     //     name: 'capitalPorPagar',
    //     //     label: 'Capital Pagado',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     // {
    //     //     name: 'cuotaUnidadPagadora',
    //     //     label: 'Cuota Unidad Pagadora',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     // {
    //     //     name: 'capitalpagar',
    //     //     label: 'Capital a pagar',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     {
    //         name: 'capitalSaldo',
    //         label: 'Saldo Capital',
    //         options: {
    //             customBodyRender: (value: number | bigint) =>
    //                 UtilService.formatCurrency(value),
    //             setCellProps: () => ({
    //                 style: {
    //                     textAlign: 'left',
    //                 },
    //             }),
    //         },
    //     },

    //     // {
    //     //     name: 'capitalInteresporPagar',
    //     //     label: 'Intereses a pagar',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },

    //     // {
    //     //     name: 'capitalSaldoTotal',
    //     //     label: 'Saldo Total',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },

    //     // {
    //     //     name: 'afectacionCuentaIndividual',
    //     //     label: 'Afectación Cuenta Individual',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     // {
    //     //     name: 'cobroFonvivienda',
    //     //     label: 'Cobro Fonvivienda',
    //     //     options: {
    //     //         customBodyRender: (value: number | bigint) =>
    //     //             UtilService.formatCurrency(value),
    //     //         setCellProps: () => ({
    //     //             style: {
    //     //                 textAlign: 'left',
    //     //             },
    //     //         }),
    //     //     },
    //     // },
    //     );
    // }

   
    const handleExportToExcel = async () => {
        const columnNames = [
            "cuota",
            "fechaCorte",
            "fechaPago",
            "saldoLeasing",
            "capitalLeasing",
            "interesLeasing",
            "seguroVidaLeasing",
            "seguroHogar",
            "totalCuota",
        ];
    
        if (simulacionPersonales.tipoAfiliadoId !== 14) {
            columnNames.push(
                "capitalReducido",
                "interesPorPagar",
                "capitalPorPagar",
                "cuotaUnidadPagadora",
                "capitalpagar",
                "capitalSaldo",
                "capitalInteresporPagar",
                "capitalSaldoTotal",
                "afectacionCuentaIndividual",
                "cobroFonvivienda"
            );
        }
        const columnDataList = createSimulationDataUno.map((row) => ({

            data: columnNames.map((columnName) => String(row[columnName] || ''))
            
		}));

        if (columnDataList) { // Comprobación de nulidad
            const request = {
                columnDataList: columnDataList.map(item => ({
                    data: item.data
                })),
                columnName: columnNames, 
        };
			const response = await apiReportes.GenerarExcels(request);
			const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
			const url = window.URL.createObjectURL(blob);
			const link = document.createElement('a');
			link.href = url;
			link.download = `Simulacion-Leasing-Fecha:${moment().format()}.csv`;
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
                'No se puede generar archivo para este Usuario.',
                'warning',
            );
        }
    };
    // const handleExportToPdf = async () => {
            
    //     const request: CreditFlowDocumentsRequestModel = {
    //         tcR_ID: procedureRequest.creditTypeID,
    //         orden: procedureRequest.order,
    //         principal: procedureRequest.principalActivity,
    //         soC_ID: procedureRequest.requestID,
    //         afL_ID: procedureRequest.flowActivityID,
    //     };

    //     await apiCredit
        
    //         .postCreditFiles(request) 
    //         .then(async response => {

               
    //             if (response) {               
    //                 if (response.isSuccess) {
                                                
    //                     const docAfiliados = response.data 
    //                     const rutaSimulacionPdf = docAfiliados.find((x: { tdC_ID: number }) => x.tdC_ID === 83);
    //                     verArchivos(rutaSimulacionPdf.rutA_IMAGEN)  
                     
    //                 } else {
    //                     await NotificationService.genericAlert(
    //                         'Error en actualización',
    //                         'Ocurrio un error al intentar actualizar, intente de nuevo.',
    //                         'error',
    //                     );
                 
    //                 }
    //             }
    //         })
    //         .catch(() => {
         
    //             toast('Error del sistema', { type: toast.TYPE.ERROR });
    //         });

    // }

    return (
        <CHPaper title="Datos Simulación Crédito Leasing">
            <div
                style={{
                    display: 'table',
                    tableLayout: 'fixed',
                    width: '100%',
                }}
            >
                {createSimulationDataUno.length !== 0 &&
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
                                        InputLabelProps={{ shrink: true }}
                                        label="Valor Total Solicitado"
                                        name="valorPrestamo"
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        value={createSimulationDataUno[0]?.saldoLeasing}

                                        isReadOnly
                                    />
                                  
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Valor a Financiar"
                                        value={UtilService.formatCurrency(
                                            createSimulationDataUno[0]
                                                .valorFinanciar,
                                        )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                                {/* <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Aportes Propios"
                                        value={UtilService.formatCurrency(
                                            createSimulationDataUno[0]
                                                .aportesPropios,
                                        )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid> */}
                             
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Plazo "
                                        value={createSimulationDataUno.length - 1}
                                        isReadOnly
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Años"
                                        isReadOnly
                                        value={
                                            ((createSimulationDataUno.length - 1) /
                                                12) %
                                                1 ===
                                            0
                                                ? (createSimulationDataUno.length -
                                                      1) /
                                                  12
                                                : (
                                                      (createSimulationDataUno.length -
                                                          1) /
                                                      12
                                                  ).toFixed(2)
                                        }
                                    />
                                </Grid>

                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Sueldo Proyectado"
                                        value={UtilService.formatCurrency(
                                            proyecciones?.svp_Sueldo === null ? 0 :
                                            proyecciones?.svp_Sueldo )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>

                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Ahorros Proyectado"
                                        value={UtilService.formatCurrency(
                                            proyecciones?.svp_Ahorros === null ? 0 :
                                            proyecciones?.svp_Ahorros)}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>

                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Cesantias Proyectado"
                                        value={UtilService.formatCurrency(
                                            proyecciones?.svp_Cesantias === null ? 0 :
                                            proyecciones?.svp_Cesantias )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>

                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Subsidio"
                                        value={UtilService.formatCurrency(
                                            proyecciones?.svp_Subsidio === null? 0 :
                                            proyecciones?.svp_Subsidio )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Recursos Futuros"
                                        value={UtilService.formatCurrency(
                            
                                            (proyecciones?.svp_Subsidio + proyecciones?.svp_Cesantias + proyecciones?.svp_Ahorros ))}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>

                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Canon Inicial"
                                        value={UtilService.formatCurrency(
                                            (simulacionPersonales.valorInmueble*(simulacionPersonales.porcentajeCanonInicial/100))
                                            )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Opcion de compra"
                                        value={UtilService.formatCurrency(
                                            (simulacionPersonales.valorInmueble*(simulacionPersonales.porcentajeOpciondeCompra/100))
                                            )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Canon Extraordinario"
                                        value={UtilService.formatCurrency(
                                            ultimoElemento.capitalSaldoTotal
                                            
                                            )}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Tasa EA (%)"
                                        value={proyecciones?.svp_EA === null ? 0 :
                                               (proyecciones?.svp_EA)*100}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <CHTextField
                                        label="Score Transunion"
                                        value={createSimulationDataUno[0]?.riesgoMinimoTransUnion}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                </Grid>
                              
                                        <Grid item md={3} className={classes.column}>
                                        <Button variant="outlined" color="primary" 
                                          onClick={handleClickOpen}>
                                           Ver Proyección
                                        </Button>
                                       </Grid>
                                   
                          


                            </Grid>
                        </Typography>
                    </CardContent>
                </Card>
                }
                <MUIDataTable
                    title=""
                    data={createSimulationDataUno}
                    columns={columns}
                    options={{
                        selectableRows: 'none',
                        responsive: 'vertical',
                        filter: false,
                        print: false,
                        elevation: 0,
                        textLabels: {
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
        </CHPaper>
    );
}
