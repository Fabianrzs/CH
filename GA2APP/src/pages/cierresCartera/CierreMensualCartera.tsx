import React, { useEffect, useState, useRef } from 'react';
import { Button, Card, CardContent, CircularProgress, createStyles, FormControlLabel, Grid, IconButton, makeStyles, Radio, RadioGroup, Tooltip } from '@material-ui/core';
import { ArrowUpwardOutlined, CloudDownload } from '@material-ui/icons';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable, { MUIDataTableProps, MUIDataTableState } from 'mui-datatables';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import CHContainerSubtitle from '../../components/container-subtittle/container-subtittle';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHDatePickerV3 from '../../components/ch-datepicker/CHDatepickerV3';
import CreditApi from '../../services/api-credit.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import NotificationService from '../../services/notification.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import UtilService from '../../utils/util.service';
import CarteraApi from '../../services/api-cartera-service';
import './TablasCierres.scss';
import ApiproductoClients from '../../services/api-producto.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import { ColumnFormatType } from '../../enums/ColumnFormatType';
import DownloadBlob from '../../utils/blob';
import ApiReportes from '../../services/api-reportes.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuthService from '../../services/auth.service';
import CatalogApi from '../../services/api-catalog.service';
import { useIp } from '../../shared/context/IpContext';


const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },

        tablaCierre: {
            overflow: 'auto !important'

        },

        waitMessage: {
            textAlign: 'center',
            padding: '1rem',
            fontWeight: 'bold'
        }
    }),
);


function Cierre(props: any) {

 
    return <>
        <Grid>
            <Grid item xs={12}>
                <Card>
                    <CardContent>
                        <Grid container justifyContent="space-around">
                            <Grid item xs={3}>
                                <CHContainerSubtitle
                                    subTitle="de Datos"
                                    title="Actualización"
                                    icon="fch-Procedimientos-solicitudes-0-1"
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <br />
                                <Controller
                                    name="fecha"
                                    control={props.formularioCierreMensulaCartera.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        pattern: {
                                            value: /^([1][0-9][2-9][2-9]|[2][0-9][0-9][0-9])([/.]|[-.])([0-9])([0-9])/g,
                                            message: 'Fecha no válida'
                                        }
                                    }}
                                    render={({
                                        field: { onChange, value, onBlur },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePickerV3
                                            views={['year', 'month']}
                                            error={!!error}
                                            invalidDateMessage="Fecha no válida"
                                            maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                            minDateMessage='La fecha no debe ser anterior a la fecha mínima'
                                            minDate={moment('1910-12').format('YYYY-MM')}
                                            label="Fecha *"
                                            value={value}
                                            onChange={onChange}
                                            onBlur={onBlur}

                                        />
                                        
                                    )}
                                    
                                />
                            </Grid>
                            
                            <Grid item xs={3}>
                                <br />
                                <Controller
                                    name="actualizacion"
                                    control={props.formularioCierreMensulaCartera.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <RadioGroup
                                            value={value}
                                            onChange={onChange}
                                            row
                                            aria-label="actualización"

                                            name="row-radio-buttons-group"
                                        >
                                            <FormControlLabel
                                                value="1"
                                                control={
                                                    <Radio color="primary" />
                                                }
                                                label="DEFINITIVO"
                                            />
                                            <FormControlLabel
                                                value="0"
                                                control={
                                                    <Radio color="primary" />
                                                }
                                                label="SIMULADO"
                                            />
                                        </RadioGroup>
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <br />
                    </CardContent>
                </Card>
            </Grid>
        </Grid>
            <Grid className="buttonssolicitud">
                <Button
                    disabled={props.loadingCierres}
                    className={props.classes.buttonprincipal}
                    variant="outlined"
                    onClick={props.formularioCierreMensulaCartera.handleSubmit(props.onSubmit)}
                >
                    CIERRE
                </Button>
                <Button
                    disabled={props.loadingCierres}
                    className={props.classes.buttonprincipal}
                    variant="outlined"
                    // onClick={() => props.formularioCierreMensulaCartera.insertarCierreMensualSeven()}
                    onClick={props.formularioCierreMensulaCartera.handleSubmit(props.insertarCierreMensualSeven)}

                >
                    SEVEN
                </Button>
        </Grid>
         
        <br />
     
        { props.loadingCierres && <div className={props.classes.waitMessage}> <span>Espere un momento...</span> </div> }
        <div className='MuiTableCell-stickyHeader; jss86'>
            <CierreTable 
            data={props.dataCierreMes} 
            mostrarTabla={props.mostrarTabla} 
            date={props.formularioCierreMensulaCartera?.getValues().fecha} />
        </div>
    </>
}

function CierreTable({
    data, mostrarTabla, date
}:
    {
        data: any[],
        mostrarTabla: any,
        date: any

    }) {

        // const dataReporte = data.map((x: any) => ({
        //     ...x,
        //     saldoAlCorte: x.tipoCreditoId === 2 
        //         ? (x.saldoCapital || 0)
        //         : ((x.saldoCapitalTradicional || 0) + (x.acumuladoCapital || 0))
        // }));

        const dataReporte = data.map((x: any) => ({
            ...x,
            saldoAlCorte: x.saldoCapital ?? 0
        }));
        

        const tableRef = useRef<React.Component<MUIDataTableProps, MUIDataTableState>>();
        const [downloading, setDownloading] = useState(false);

        const columns = [
            {
                name: 'numeroCredito',
                label: 'Número de Crédito',
                options: {
    
                    setCellProps: () => ({
                       
                    }),
                    setCellHeaderProps: () => ({
             
                       className: 'elemento'
                    }),
                
                },
                formatType: ColumnFormatType.Numerico
            },
            {
                name: 'tipoIdentificacion',
                label: 'Tipo de Identificación',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'identificacion',
                label: 'Número de Identificación',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Numerico
            },
            {
                name: 'nombres',
                label: 'Apellidos y nombres',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },

            {
                name: 'tipoCredito',
                label: 'Tipo Crédito',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'estadoCredito',
                label: 'Estado Crédito',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'plazoTotal',
                label: 'Plazo Total',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Numerico
            },
                
            {
                name: 'plazoActual',
                label: 'Plazo Actual',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Numerico
            },
            {
                name: 'cuotasCanceladas',
                label: 'Cuotas pagadas',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Numerico
            },
            {
                name: 'tasaSeguroVida',
                label: 'Tasa Seguro Vida',
                options: {
                    setCellHeaderProps: () => ({
                     className:'filas'
                    }),
                    customBodyRender: (value: number) => UtilService.formatPercent3(value / 100)
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'tasaSeguroHogar',
                label: 'Tasa Seguro Hogar',
                options: {
                    setCellHeaderProps: () => ({
                     className:'filas'
                    }),
                    customBodyRender: (value: number) => UtilService.formatPercent3(value / 100)
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'tasaCreditoEA',
                label: 'Tasa Crédito EA',
                options: {
                    setCellHeaderProps: () => ({
                     className:'filas'
                    }),
                    customBodyRender: (value: number) => UtilService.formatPercent3(value / 100)
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'tasaPeriodica',
                label: 'Tasa Crédito NMV',
                options: {
                    setCellHeaderProps: () => ({
                     className:'filas'
                    }),
                    customBodyRender: (value: number) => UtilService.formatPercent3(value)
                },
                formatType: ColumnFormatType.Decimal
            },
    
            {
                name: 'valorCuota',
                label: 'Valor Cuota',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'saldoAlCorte',
                label: 'Saldo al corte',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'saldoMora',
                label: 'Saldo Capital Mora',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'diasMora',
                label: 'Días Mora',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Numerico
            },
    
            {
                name: 'valorDeudaRemanente',
                label: 'Valor Deuda Remanente',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
    
            {
                name: 'fechaUltimoPago',
                label: 'Fecha Último Pago',
                options: {
                    customBodyRender: (value: Date) => UtilService.getDate(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Fecha
            },
           
    
            {
                name: 'fechaProximoPago',
                label: 'Fecha Próximo Pago',
                options: {
                    customBodyRender: (value: Date) => UtilService.getDate(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Fecha
            },
    
            {
                name: 'acumuladoSeguroHogar',
                label: 'Acumulado Seguro Hogar',
                options: {
                    customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
    
            {
                name: 'acumuladoSeguroVida',
                label: 'Acumulado Seguro Vida',
                options: {
                    customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },

            {
                name: 'acumuladoInteresesMora',
                label: 'Interés Corriente en Mora',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
    
            {
                name: 'fechaUltimoPagoInteresesMora',
                label: 'Fecha ultimo Intereses Mora',
                options: {
                    customBodyRender: (value: Date) => UtilService.getDate(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Fecha
            },
            
           
            {
                name: 'acumuladoInteresesCorrientes',
                label: 'Acumulado Interés Corriente',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Decimal
            },
    
            
            {
                name: 'fechaUltimoPagoInteresesCorrientes',
                label: 'Fecha Último Pago Interés Corriente',
                options: {
                    customBodyRender: (value: Date) => UtilService.getDate(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
                formatType: ColumnFormatType.Fecha
            },
            {
                name: 'calificacion',
                label: 'Calificación de Cartera',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'valorAjusteGarantia',
                label: 'Valor de la Garantía',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                        setCellHeaderProps: () => ({
                            className: 'filas'
                        }),
                },
                formatType: ColumnFormatType.Decimal
            },
            {
                name: 'provision',
                label: 'Provisión de Cartera',
                options: {
                    customBodyRender: (value: number | bigint) =>
                        UtilService.formatCurrency(value),
                        setCellHeaderProps: () => ({
                            className: 'filas'
                        }),
                },
                formatType: ColumnFormatType.Decimal
            },
    
            {
                name: 'provisionPorcentaje',
                label: '% Provisión',
                options: {
                  
                    setCellHeaderProps: () => ({
                     className:'filas'
                    }),
                    customBodyRender: (value:number) => (
                        (value).toLocaleString('es-CO',{ style: 'percent' })
                      )
                },
                formatType: ColumnFormatType.Porcentaje
            },
            // {
            //     name: 'saldoCapitalTradicional',
            //     label: 'Saldo Capital Tradicional',
            //     options: {
            //         customBodyRender: (value: number | bigint) =>
            //             UtilService.formatCurrency(value),
            //         setCellHeaderProps: () => ({
            //             className: 'filas'
            //         }),
            //     },
            // },
            // {
            //     name: 'acumuladoCapital',
            //     label: 'Acumulado Capital',
            //     options: {
            //         customBodyRender: (value: number | bigint) =>
            //             UtilService.formatCurrency(value),
            //         setCellHeaderProps: () => ({
            //             className: 'ultimaFila'
            //         }),
            //     },
            // },
        ];

        const handleClick = async () => {
            try {
                setDownloading(true);
                await handleExportToExcel();
            } finally {
                setDownloading(false);
            }
        }

        const { publicIp } = useIp();

        const handleExportToExcel = async () => {
            const apiReportes = ApiReportes();

            const visibleColumns = (tableRef?.current?.state?.columns
                ?.filter(x => x.display === 'true' || x.display === true)
                ?.map(x => x.name) ?? columns.map(x => x.name)
            );

            const selectedColumns = columns.filter(x => visibleColumns?.includes(x.name));

            const columnDataList = dataReporte.map((row: any) => ({
                data: selectedColumns.map(x => {
    
                    if(x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                        const parseDate = moment(row[x.name]);
                        return parseDate.isValid()
                            ? parseDate.format("YYYY-MM-DD")
                            : '';
                    }

                    if(x.formatType === ColumnFormatType.Porcentaje){
                        return row[x.name].toLocaleString('es-CO',{ style: 'percent' });
                    }
    
                    return row[x.name]?.toString() ?? ''
                })
            }));

            
            if (columnDataList) { 
                const user = AuthService.getUserInfo();
                const auditoriaApi= AuditoriaApi();                 
                const nav =(window.navigator as any);
                const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
                const requestAuditoria={
                        navegador:usrAg,
                        sistemaOperativo:nav.userAgentData.platform,
                        nombreUsuario:user.USR_USERNAME, 
                        usurioId:user.USR_ID,
                        ip: publicIp,
                        modulo:'Cierre Mensual',
                        hostname:document.location.host
                } as Auditoria;
                const request = {
                    columnData: columnDataList.map(item => item.data),
                    columnsInfo: selectedColumns.map(x => ({
                        name: x.label,
                        formatType: x.formatType
                    }))
                };
                const filename = `Cierre mensual ${moment(date).format('YYYY-MM')}.xlsx`;
                const response = await apiReportes.GenerarExcelInvariantCulture(request)
                                       .catch((error:any)=>{
                                        requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                                        requestAuditoria.idTipoAccion=8;
                                        requestAuditoria.descripcion=`Descarga de reporte cierre mensual en formato excel:                 
                                        \n archivo: ${filename}
                                        \n Error:  ${error}
                                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                                        auditoriaApi.InsertAuditoria(requestAuditoria);
                                       });

                const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
               
                DownloadBlob(blob, filename);
                requestAuditoria.tipoAccion='DESCARGAS';
                requestAuditoria.idTipoAccion=7;
                requestAuditoria.descripcion=`Descarga de reporte cierre mensual en formato excel:                 
                 \n archivo: ${filename}`;
                await auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        }

        return mostrarTabla && (
            <MUIDataTable
                title=""
                data={dataReporte}
                columns={columns}
                innerRef={tableRef}
                options={{
                    selectableRows: 'none',
                    filter: false,
                    print: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    responsive: 'vertical',
                    fixedHeader: true,
                    fixedSelectColumn: true,
                    tableBodyHeight: '500px',
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
                    download: false,
                    customToolbar: () => (
                        <>
                            <Tooltip title="Descargar Excel">
                                {
                                    downloading
                                        ? <CircularProgress size={20} style={{'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px'}}/>
                                        :  <IconButton onClick={() => handleClick()} >
                                                <CloudDownload/>
                                            </IconButton>
                                }
                            </Tooltip>
                        </>
                    )
                }}
            />
        );
}

const CierreMensualCartera = () => {

    const creditApi = CreditApi();
    const carteraApi = CarteraApi();
    const ApiCartera = ApiproductoClients();
    const auditoriaApi= AuditoriaApi();
    let fechaUltimoCierreMensual: any = null;
    let fechaUltCausacionDiariaIntMoratorios: any = null;
    let fechaUltCausacionDiariaIntCorrientes: any = null;
    let ejecutarProceso = true;

    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreMes, setDataCierreMes] = useState<any[]>([]);
    const [mostrarTabla, setMostrarTabla] = useState(false);
    const [loadingCierres, setLoadingCierres] = useState(false);
    const [isLoading, setIsLoading] = useState (false);
    const [dato, setDato] = useState<any[]> ([]);

   
    const classes = useStyles();
    const fechaFormato = moment(new Date()).subtract(1, 'months').format('YYYY-MM');
    const fechaConvertida = moment(fechaFormato, 'YYYY-MM').toDate();
    const formularioCierreMensulaCartera = useForm({
        'mode':'onChange',
        defaultValues: {
            fecha: fechaConvertida,
            actualizacion: '0',
        },

    });

    useEffect(() => {
        cargarCierresRealizados();
    }, []);

    const cargarCierresRealizados = async () => {
        setLoadingCierres(true);
        await carteraApi
        .getUltimosCierres()
        .then(async (response: any) => {
            if (response) {

                if (response.data != null && response.isSuccess) {
                    setDato(response.data)
                    fechaUltimoCierreMensual = response.data.filter((value:any) => value.idTipoCierre === 2)
                    fechaUltCausacionDiariaIntMoratorios = response.data.filter((value:any) => value.idTipoCierre === 1)
                    fechaUltCausacionDiariaIntCorrientes = response.data.filter((value:any) => value.idTipoCierre === 3)
                } else {
                    toast(`No se encontraron cierres de cartera`, { type: toast.TYPE.WARNING });
                }
            }
        }).finally(() => setLoadingCierres(false));
    }

    const insertarCierreMensualSeven =  (dataCierre:any) => {

        setIsLoading(true)
        const cierreMensualSeven = dato.filter((value:any) => value.idTipoCierre === 2)
        const request = {
            anioCierre: moment(dataCierre.fecha).year(),
            mesCierre: moment(dataCierre.fecha).month() + 1,
            actualiza: dataCierre.actualizacion === "1"
        }
        // const request = {
        //     id: cierreMensualSeven[0].idCierre,
        //     anioCierre: cierreMensualSeven[0].anio,
        //     mesCierre: cierreMensualSeven[0].mes
        // }

        ApiCartera
        .postCierreMensualSeven(request)
        .then(response => {
            if(response.isSuccess) {
                setIsLoading(false)
                toast(`${response.returnMessage}`, { type: toast.TYPE.SUCCESS });

            }
        })
        .catch((error) => {
            if (error.response && error.response.data ) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000});
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
        }).finally(() => setIsLoading(false));
    }
    // const insertarCierreMensualSeven =  () => {
        
    //     setIsLoading(true)
    //     const cierreMensualSeven = dato.filter((value:any) => value.idTipoCierre === 2)
    //     const request = {
    //         id: cierreMensualSeven[0].idCierre,
    //         anioCierre: cierreMensualSeven[0].anio,
    //         mesCierre: cierreMensualSeven[0].mes
    //     }

    //     ApiCartera
    //     .postCierreMensualSeven(request)
    //     .then(response => {
    //         if(response.isSuccess) {
    //             setIsLoading(false)
    //             const leasingMatch = response.returnMessage.match(/Provisiones Cartera (.*?)<\/NewDataSet>/);
    //             const hipotecarioMatch = response.returnMessage.match(/Resultado Mov.Contable Seven Créditos Hipotecario:(.*?)<\/NewDataSet>/);
    
    //             const retornoLeasing = leasingMatch[1].match(/<RETORNO>(.*?)<\/RETORNO>/);
    //             const retornoHipotecario = hipotecarioMatch[1].match(/<RETORNO>(.*?)<\/RETORNO>/);
    //             const errorLeasing = leasingMatch[1].match(/<TXTERROR>(.*?)<\/TXTERROR>/);
    //             const errorHipotecario = hipotecarioMatch[1].match(/<TXTERROR>(.*?)<\/TXTERROR>/);

    //             if (retornoLeasing && retornoLeasing[1] === '0') {
    //                 toast('Cierre Mensual Leasing a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
    //             } else {
    //                 toast(`Cierre Mensual Leasing a Seven FALLO: ${errorLeasing[1]}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
    //             }
    
    //             if (retornoHipotecario && retornoHipotecario[1] === '0') {
    //                 toast('Cierre Mensual Hipotecario a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
    //             } else {
    //                 toast(`Cierre Mensual Hipotecario a Seven FALLO: ${errorHipotecario[1]}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
    //             }

    //         }
    //     })
    //     .catch((error) => {
    //         if (error.response && error.response.data ) {
    //             toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000});
    //         } else {
    //             toast("Error del sistema", { type: toast.TYPE.ERROR });
    //         }
    //     }).finally(() => setIsLoading(false));
    // }


    const aplicarCuotasAdelentadas = () => {
        setIsLocalLoading(true)
        const fechaFormato = moment(new Date()).format('YYYY-MM-DD');
        carteraApi
        .AplicarCuotasAdelantadas(fechaFormato)
        .then(response => {
            if(response.isSuccess) {
           console.log('response',response)

            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
            setIsLocalLoading(false)
        })
        
    }

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
        await catalogApi 
            .getObtenerIpLocal(hexadecimal)
            .then(async (response: any) => {
    
                if (response) {
                    if (response.isSuccess) {
                        setPublicIpAddress(response.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    
    }

    useEffect(() => {
        const fetchLocalIPAddress = () => {
            const peerConnection = new RTCPeerConnection({ iceServers: [] });
            peerConnection.createDataChannel('');
            peerConnection.createOffer()
                .then((offer) => peerConnection.setLocalDescription(offer))
                .catch((error) => console.error('Error creating offer:', error));

            peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    const ip: any = event.candidate.address;
                    if (isValidIPv4(ip) || isValidIPv6(ip)) {
                        enviarIp(ip);
                    } else {
                        enviarIp(ip);
                    }
                }
            };

            return () => {
                peerConnection.close();
            };
        };
        fetchLocalIPAddress();

    }, []);

    

    const onSubmit = async (data: any) => {
        
        ejecutarProceso = true;
        const inputFecha = moment(data.fecha)
        const nuevaFecha = moment(data.fecha).year()

        aplicarCuotasAdelentadas()
        if(nuevaFecha <= 1910 )  {
            return
        }

        if(!inputFecha.isValid()){
            return
        }
       
        const request = {
            anioCierre: moment(data.fecha).year(),
            mesCierre: moment(data.fecha).month() + 1,
            actualiza: data.actualizacion === "1"
        }


        setDataCierreMes([]);
        await cargarCierresRealizados();

        if(request.actualiza) {      
            if (fechaUltimoCierreMensual !== null && fechaUltimoCierreMensual.length > 0 && fechaUltimoCierreMensual[0].fechaCierre !== null) {
                const fechaUltimoCierreMensualRealizado = new Date(fechaUltimoCierreMensual[0]?.fechaCierre);
                const tieneCierreMensual = moment(inputFecha).isSame(fechaUltimoCierreMensualRealizado, 'month')
                if (tieneCierreMensual) {
                    NotificationService.genericAlertError(
                        'Error',               
                        'Ya existe un cierre mensual',
                        'error',
                    );
                    setIsLocalLoading(false);
                    return
                }
            }

            let tieneIntMoratorios = false;
            const cantIntMoratorios = fechaUltCausacionDiariaIntMoratorios?.length ?? 0;
            if (cantIntMoratorios > 0 && fechaUltCausacionDiariaIntMoratorios[0].fechaCierre != null) {
                const fechaUltimoCierreDiarioRealizado = new Date(fechaUltCausacionDiariaIntMoratorios[0]?.fechaCierre);
                const ultimoDiaMes =  moment(inputFecha).endOf('month');
                tieneIntMoratorios = moment(fechaUltimoCierreDiarioRealizado).isSame(ultimoDiaMes.toDate(),'day');
            }

            if (!tieneIntMoratorios) {
                NotificationService.genericAlertError(
                    'Error',
                    'Es necesario ejecutar la causacion de intereses diarios moratorios del último día del mes a cerrar',
                    'error',
                );
                setIsLocalLoading(false);
                return;
            }

            let tieneIntCorr = false;
            const cantIntCorr = fechaUltCausacionDiariaIntCorrientes?.length ?? 0;
            if (cantIntCorr > 0 && fechaUltCausacionDiariaIntCorrientes[0].fechaCierre != null) {
                const fechaUltimaCausacionDiariaRealizada = new Date(fechaUltCausacionDiariaIntCorrientes[0]?.fechaCierre);
                const ultimoDiaMes =  moment(inputFecha).endOf('month');
                tieneIntCorr = moment(fechaUltimaCausacionDiariaRealizada).isSame(ultimoDiaMes.toDate(),'day');
            }

            if (!tieneIntCorr) {
                NotificationService.genericAlertError(
                    'Error',               
                    'Es necesario ejecutar la causación de intereses diaria del último día del mes a cerrar',
                    'error',
                );
                return;
            }
        }
        
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
        
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Cierre mensual de cartera',
            hostname:document.location.host
        } as Auditoria
        try {
            
            if (request.actualiza) {
                await swal({
                    text: 'Este proceso actualizará los datos. ¿Desea continuar?',
                    icon: 'warning',
                    buttons: { 

                        cancel: {
                            text: "No",
                            value: false,
                            visible: true,
                        },
                        confirm: {
                            text: "Sí",
                            value: true,
                            visible: true,
                        }
                     },
                }).then((respuesta) => {
                    if(!respuesta){
                        ejecutarProceso = false;
                    }
                })
            }
                 
            if(ejecutarProceso) {
                setIsLocalLoading(true);
                const response = await creditApi.postCierreMensualCartera(request);
                // const { data, isSuccess } = response;
               
                if (response.data != null && response.isSuccess) {
                    if(request.actualiza === true){
                        await cargarCierresRealizados();
                        insertarCierreMensualSeven(data)
                    }
                    setDataCierreMes(response.data);
                    setMostrarTabla(true);
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Cierre mensual  de  cartera parámetros:
                         \n Periodo:  ${inputFecha.format("YYYY/MM")}                         
                         \n Actualizó Seven:  ${request.actualiza?'Sí':'No'}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                } else {
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Cierre mensual  de  cartera parámetros:
                    \n Periodo:  ${inputFecha.format("YYYY/MM")}
                    \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                    \n Error:  ${response?.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                  toast("No se encontraron resultados", { type: toast.TYPE.ERROR });
                  setMostrarTabla(false);
                }
            }
          } catch (error:any) {
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=8;
            requestAuditoria.descripcion=String(`Cierre mensual  de  cartera parámetros:
            \n Periodo:  ${inputFecha.format("YYYY/MM")}
            \n Actualizó Seven:  ${request.actualiza?'Sí':'No'}
            \n Error: ${error}   
            \n ReturnApiMessage: ${error?.response?.data?.returnMessage}`)
            await auditoriaApi.InsertAuditoria(requestAuditoria);
            toast("Error del sistema", { type: toast.TYPE.ERROR });
          } finally {
            setIsLocalLoading(false);
          }
    };

    // if (isLocalLoading) {
    //     return  <LoadingV2Component key="loader" loading={isLocalLoading} tittle='Cierre Mensual...' />
    //     // return <CHLoadingProcess />;
    // }

    return (
        <>

        <div 
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',
                
            }}>
          

            <LoadingV2Component key="loader" loading={isLoading} tittle='Insertando Cierre a Seven...' />
            <LoadingV2Component key="loader" loading={isLocalLoading} tittle='Cierre Mensual...' />
            <PageIndicators
                topTextNivel1="Cierres Cartera"
                topTextNivel2="Cierre Mensual"
            />
            <IconTitle
                title="Cierre"
                subTitle="Mensual"
                icon="fch-Verifique-Docuementos-1-1"
            />
        

            <Cierre {...{formularioCierreMensulaCartera, classes, onSubmit, dataCierreMes, mostrarTabla, loadingCierres, insertarCierreMensualSeven}}/>
        </div>
        </>
    );
};

export default CierreMensualCartera;