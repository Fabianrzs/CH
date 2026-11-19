import React, { useEffect, useRef, useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    Card,
    CardContent,
    FormControlLabel,
    Grid,
    Radio,
    RadioGroup,
    makeStyles,
    createStyles,
    Tooltip,
    CircularProgress,
    IconButton,
} from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable, { MUIDataTableProps, MUIDataTableState } from 'mui-datatables';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import { Alert, AlertTitle } from '@material-ui/lab';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHContainerSubtitle from '../../components/container-subtittle/container-subtittle';
import { startPostCierreDiarioCartera } from '../../actions/producto.action';
import UtilService from '../../utils/util.service';
import Header from '../../components/header/Header';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import './TablasCierres.scss';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CreditApi from '../../services/api-credit.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { ProductState } from '../../reducers/producto.reducer';
import { AppState } from '../../store/rootReducer';
import CarteraApi from '../../services/api-cartera-service';
import NotificationService from '../../services/notification.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import ApiproductoClients from '../../services/api-producto.service';
import AuthService from '../../services/auth.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import ApiReportes from '../../services/api-reportes.service';
import { ColumnFormatType } from '../../enums/ColumnFormatType';
import DownloadBlob from '../../utils/blob';
import { Auditoria } from '../../types/Auditoria.model.type';
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
        columnDate: {
            minWidth: '110px',

        },

        tablaCierre: {
            overflow: 'auto !important'
     
    },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },

        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        totales: {
            textAlign: 'right'
        }

    }),
);


function CierreTable({ data, mostrarTabla, fechaCierre }: { data: any[], mostrarTabla: any, fechaCierre?: Date }) {

    const tableRef = useRef<React.Component<MUIDataTableProps, MUIDataTableState>>();
    const [downloading, setDownloading] = useState(false);
    
    const classes = useStyles();

    const columns = [
        {
            name: 'numeroCredito',
            label: 'Número de Crédito',
            options: {

                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                   
                }),
                setCellHeaderProps: () => ({
         
                   className: 'elemento'
                }),
            
            },
            formatType: ColumnFormatType.Numerico,
        },
        // {
        //     name: 'tipoIdentificacion',
        //     label: 'Tipo de Identificación',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        {
            name: 'identificacion',
            label: 'Número de Identificación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Numerico,
        },
        // {
        //     name: 'nombres',
        //     label: 'Apellidos y nombres',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        // {
        //     name: 'estadoCredito',
        //     label: 'Estado Crédito',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'plazoTotal',
        //     label: 'Plazo Total',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        
        // {
        //     name: 'plazoActual',
        //     label: 'Plazo Actual',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'cuotasCanceladas',
        //     label: 'Cuotas pagadas',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'tasaSeguroVida',
        //     label: 'Tasa Seguro Vida',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.roundValue1(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        // {
        //     name: 'tasaSeguroHogar',
        //     label: 'Tasa Seguro Hogar',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.roundValue1(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'tasaCreditoEA',
        //     label: 'Tasa Crédito EA',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'tasaPeriodica',
        //     label: 'Tasa Crédito Periodica',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
    

        // {
        //     name: 'valorCuota',
        //     label: 'Valor Cuota',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'saldoCapital',
        //     label: 'Saldo Capital',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'saldoMora',
        //     label: 'Saldo Capital Mora',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        // {
        //     name: 'diasMora',
        //     label: 'Días Mora',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        // {
        //     name: 'fechaUltimoPago',
        //     label: 'Fecha Último Pago',
        //     options: {
        //         setCellProps: () => ({
        //             className: classes.columnDate,
        //             style: {
        //                 textAlign: 'left',
        //             },
                    
        //         }),
        //         customBodyRender: (value: Date) => UtilService.getDate(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
       

        // {
        //     name: 'fechaProximoPago',
        //     label: 'Fecha Próximo Pago',
        //     options: {
        //         setCellProps: () => ({
        //             className: classes.columnDate,
        //             style: {
        //                 textAlign: 'left',
        //             },
                    
        //         }),
        //         customBodyRender: (value: Date) => UtilService.getDate(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        // {
        //     name: 'acumuladoSeguroHogar',
        //     label: 'Acumulado Seguro Hogar',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        {
            name: 'causacionSeguroHogar',
            label: 'Causacion Seguro Hogar',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        // {
        //     name: 'acumuladoSeguroVida',
        //     label: 'Acumulado Seguro Vida',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        {
            name: 'causacionSeguroVida',
            label: 'Causacion Seguro Vida',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        // {
        //     name: 'acumuladoInteresesMora',
        //     label: 'Interés Mora',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        // {
        //     name: 'fechaUltimoPagoInteresesMora',
        //     label: 'Fecha Último Pago Interés Mora',
        //     options: {
        //         setCellProps: () => ({
        //             className: classes.columnDate,
        //             style: {
        //                 textAlign: 'left',
        //             },
                    
        //         }),
        //         customBodyRender: (value: Date) => UtilService.getDate(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        
       
        // {
        //     name: 'acumuladoInteresesCorrientes',
        //     label: 'Acumulado Interés Corriente',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        
        // {
        //     name: 'fechaUltimoPagoInteresesCorrientes',
        //     label: 'Fecha Último Pago Interés Corriente',
        //     options: {
        //         setCellProps: () => ({
        //             className: classes.columnDate,
        //             style: {
        //                 textAlign: 'left',
        //             },
                    
        //         }),
        //         customBodyRender: (value: Date) => UtilService.getDate(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        {
            name: 'causacionInteresesCorriente',
            label: 'Causacion Intereses Corriente',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        // {
        //     name: 'valorDeudaRemanente',
        //     label: 'Valor Deuda Remanente',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        {
            name: 'causacionInteresesRemanente',
            label: 'Causacion Intereses Remanente',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },


        // {
        //     name: 'diasCausados',
        //     label: 'Días Causados',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        { 
            name: 'total',
            label: 'Total',
            options: {
                customBodyRender: (value: number | bigint) =>
                UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
    ];

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }

    const handleExportToExcel = async () => {
        const apiReportes = ApiReportes();

        const visibleColumns = (tableRef?.current?.state?.columns
            ?.filter(x => x.display === 'true' || x.display === true)
            ?.map(x => x.name) ?? columns.map(x => x.name)
        );

        const selectedColumns = columns.filter(x => visibleColumns?.includes(x.name));

        const columnDataList = data.map((row: any) => ({
            data: selectedColumns.map(x => {

                if(x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                    const parseDate = moment(row[x.name]);
                    return parseDate.isValid()
                        ? parseDate.format("YYYY-MM-DD")
                        : '';
                }

                return row[x.name]?.toString() ?? ''
            })
        }));

        
        if (columnDataList) { 
            const request = {
                columnData: columnDataList.map(item => item.data),
                columnsInfo: selectedColumns.map(x => ({
                    name: x.label,
                    formatType: x.formatType
                }))
            };

            const response = await apiReportes.GenerarExcelInvariantCulture(request);

            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const filename = `Causación Diaria Intereses Corrientes ${moment(fechaCierre).format('DD-MM-YYYY')}.xlsx`;
            DownloadBlob(blob, filename);
        }
    }


    const filterFunc = (sum: number, val: number) => sum + val;
    const mapFunc = (e: any) => 
          (e.causacionInteresesCorriente ?? 0) 
        + (e.causacionInteresesRemanente ?? 0) 
        + (e.causacionSeguroHogar ?? 0) 
        + (e.causacionSeguroVida ?? 0);
        
    const totalLeasing = data?.filter(x => x.tipoCredito === "LEASING HABITACIONAL")
        .map(mapFunc).reduce(filterFunc, 0).toFixed(2);

    const totalHipotecario = data?.filter(x => x.tipoCredito === "CRÉDITO HIPOTECARIO")
        .map(mapFunc).reduce(filterFunc, 0).toFixed(2);

    const totalCausacion = parseFloat(totalLeasing) + parseFloat(totalHipotecario)


    return mostrarTabla && (
        <>
            <Grid container>
                <Grid style={{'margin': '1rem 0'}} item xs={12}>
                    <Alert severity="info">
                        <AlertTitle><strong>Total Causaciones</strong></AlertTitle>
                        <div className={classes.infoPanel}>
                            <strong>Total cartera:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrency(totalCausacion)}</span>
                            <strong>Leasing:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrency(totalLeasing)}</span>
                            <strong>Hipotecario:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrency(totalHipotecario)}</span>
                        </div>
                    </Alert>
                </Grid>          
            </Grid>

            <MUIDataTable
                title=""
                data={data}
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
                    tableBodyHeight: '470px',
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
        </>
    );
}

const CierreDiarioCartera = (props: any) => {

    const creditApi = CreditApi();
    const carteraApi = CarteraApi();
    const ApiClients = ApiproductoClients();
    const auditoriaApi = AuditoriaApi();
    let fechaUltimoCierreMensual: any = null;
    let fechaUltimoCierreDiario: any = null;
    let ejecutarProceso = true;

    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [dataCierreDiario, setDataCierreDiario] = useState<any[]>([]);
    const [mostrarTabla, setMostrarTabla] = useState(false);


    const classes = useStyles();

    
    const formularioCierreDiarioCartera = useForm({
        'mode':'onChange',
        defaultValues: {
            fechaCierre: new Date(),
            actualizacion: '0'
        },

    });

        carteraApi
        .getUltimosCierres()
        .then(async (response: any) => {
            if (response) {
                if (response.data != null && response.isSuccess) {
                    fechaUltimoCierreMensual = response.data.filter((value:any) => value.idTipoCierre === 2)
                    fechaUltimoCierreDiario = response.data.filter((value:any) => value.idTipoCierre === 3)
                } else {
                    toast(`No se encontraron cierres de cartera`, { type: toast.TYPE.WARNING });
                }
            }
        })
    

    const construirMovimientosContables = async(request: any) => {
       

        setIsLocalLoading(true)
        formularioCierreDiarioCartera.getValues('fechaCierre')
        const fechaCierreValidar = moment(formularioCierreDiarioCartera.getValues('fechaCierre')).utcOffset('-05:00');
        const user = AuthService.getUserInfo();

        const data : any = {
            fechaCierre: fechaCierreValidar.format(),
            actualiza: false,
            creadoPor: user.USR_ID,
            tipoCierre: 3
        }
        ApiClients
        .postConstruirMovimientosContables(data)
        .then(response => {
            if(response.isSuccess) {

                if (response.isSuccess && response.data.length > 0){         
                    toast('Construcción de Movimientos EXITOSA', { type: toast.TYPE.SUCCESS });
                    const array: any[] = [];
                    response.data.forEach((element: any, index: any) => {
                        const resultados =
                            element.causacionInteresesRemanente +
                            element.causacionSeguroHogar +
                            element.causacionSeguroVida +
                            element.causacionInteresesCorriente;
                        array.push(resultados);
                    });
    
                    const arrayDos = response.data.map((item: any, index: any) => ({
                        ...item,
                        total: array[index],
                    }));
                    setDataCierreDiario(arrayDos);
                    setMostrarTabla(true);




                } else{
                    toast('Construcción de Movimientos FALLO', { type: toast.TYPE.ERROR });
                }

            }
        })
        .catch((error) => {
            if (error.response && error.response.data ) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
        }).finally(() => setIsLocalLoading(false));
    }

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
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

    
    
    
    const insertarCierreDiarioCorriente2 = async(request: any) => {
        const user = AuthService.getUserInfo();
        let usrAg = navigator.userAgent;    
        usrAg = usrAg.toLowerCase(); 
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:usrAg,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress
        } as Auditoria
        // await validacionCausacionIC(datos)


        setIsLocalLoading(true)
        formularioCierreDiarioCartera.getValues('fechaCierre')
        const fechaCierreValidar = moment(formularioCierreDiarioCartera.getValues('fechaCierre')).utcOffset('-05:00');
        const data : any = {
            fechaCierre: fechaCierreValidar.format(),
            actualiza: true,
            creadoPor: user.USR_ID,
        }
        ApiClients
        .postCierreDiarioSevenIC(data)
        .then(response => {
            if(response.isSuccess) {

                if (response.isSuccess && response.returnMessage !== null && response.returnMessage !== undefined) {

                    const leasingMatch = response.returnMessage.match(/Resultado causacion total leasing:(.*?)<\/NewDataSet>/);
                    const hipotecarioMatch = response.returnMessage.match(/Resultado causacion total Hipotecario:(.*?)<\/NewDataSet>/);
        
                    const retornoLeasing = leasingMatch[1].match(/<RETORNO>(.*?)<\/RETORNO>/);
                    const retornoHipotecario = hipotecarioMatch[1].match(/<RETORNO>(.*?)<\/RETORNO>/);
                    const errorLeasing = leasingMatch[1].match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                    const errorHipotecario = hipotecarioMatch[1].match(/<TXTERROR>(.*?)<\/TXTERROR>/);
    
                    if (retornoLeasing && retornoLeasing[1] === '0') {
                        toast('Causación Leasing a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
                    } else {
                        toast(`Causación Leasing FALLO: ${errorLeasing[1]}`, { type: toast.TYPE.ERROR });
                    }
        
                    if (retornoHipotecario && retornoHipotecario[1] === '0') {
                        toast('Causación Hipotecario a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
                    } else {
                        toast(`Causación Hipotecario FALLO: ${errorHipotecario[1]}`, { type: toast.TYPE.ERROR });
                    }
    
                }else{
                    toast('Simulacion de movimiento EXITOSA', { type: toast.TYPE.SUCCESS });
                }

            }
        })
        .catch((error) => {
            if (error.response && error.response.data ) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
        }).finally(() => setIsLocalLoading(false));
    } 

    const insertarCierreDiarioCorriente = (data: any, request: any) => {
        
        setIsLoading(true)
        if(!data.actualiza){
            ejecutarCierreDiarioCorriente(data)
        }else{
            ApiClients
            .postCierreDiarioSevenIC(data)
            .then(response => {
                if (response.isSuccess && response.returnMessage !== null && response.returnMessage !== undefined) {
                    const leasingMatch = response.returnMessage.match(/Resultado causacion total leasing:(.*?)<\/NewDataSet>/);
                    const hipotecarioMatch = response.returnMessage.match(/Resultado causacion total Hipotecario:(.*?)<\/NewDataSet>/);
        
                    const retornoLeasing = leasingMatch[1].match(/<RETORNO>(.*?)<\/RETORNO>/);
                    const retornoHipotecario = hipotecarioMatch[1].match(/<RETORNO>(.*?)<\/RETORNO>/);
                    const errorLeasing = leasingMatch[1].match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                    const errorHipotecario = hipotecarioMatch[1].match(/<TXTERROR>(.*?)<\/TXTERROR>/);
    
                    if (retornoLeasing && retornoLeasing[1] === '0') {
                        toast('Causación Leasing a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
                    } else {
                        toast(`Causación Leasing FALLO: ${errorLeasing[1]}`, { type: toast.TYPE.ERROR });
                    }
        
                    if (retornoHipotecario && retornoHipotecario[1] === '0') {
                        toast('Causación Hipotecario a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
                    } else {
                        toast(`Causación Hipotecario FALLO: ${errorHipotecario[1]}`, { type: toast.TYPE.ERROR });
                    }
                    if (retornoLeasing[1] === '0' && retornoHipotecario[1] === '0') {
                        ejecutarCierreDiarioCorriente(data)
                    }
    
                }
            })
            .catch((error) => {
                if (error.response && error.response.data ) {
                    toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
                    setIsLoading(false)
                } else {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                    setIsLoading(false)
                }
            });
        }

       
    }

    const { publicIp } = useIp();
    

    const ejecutarCierreDiarioCorriente = (data: any) => {
        debugger
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
        
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIp,
            modulo:'Cierre diario intereses corrientes de cartera',
            hostname:document.location.host
        } as Auditoria
        setIsLoading(true);
        creditApi.postCausacionDiariaInteresesCorrientes(data)
            .then((response: any) => {
                if (response.isSuccess && response.data !== null) {
                    const array: any[] = [];
                    response.data.forEach((element: any, index: any) => {
                        const resultados =
                            element.causacionInteresesRemanente +
                            element.causacionSeguroHogar +
                            element.causacionSeguroVida +
                            element.causacionInteresesCorriente;
                        array.push(resultados);
                    });
    
                    const arrayDos = response.data.map((item: any, index: any) => ({
                        ...item,
                        total: array[index],
                    }));
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(data.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${data.actualiza?'Sí':'No'}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                    setDataCierreDiario(arrayDos);
                    setMostrarTabla(true);
                } else {
                    const error='No se encontraron resultados'
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(data.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${data.actualiza?'Sí':'No'}
                         \n Error:  ${error}
                         \n returnApiMessage: ${response.returnMessage}`
                         auditoriaApi.InsertAuditoria(requestAuditoria);
                    toast(error, { type: toast.TYPE.ERROR });
                    setMostrarTabla(false);

                    NotificationService.genericAlertError(
                        'Warning',               
                        `${response?.returnMessage}`,
                        'warning',
                    );
          

                }
            })
            .catch((error: any) => {
                console.log('ver error' , error)
                if (error.response && error.response.data) {
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                   requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                 \n fecha:  ${moment(data.fechaCierre).format("DD/MM/YYYY HH:mm")}
                 \n Actualiza Seven:  ${data.actualiza?'Sí':'No'}
                 \n Error:  ${error}
                 \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                  auditoriaApi.InsertAuditoria(requestAuditoria);
                    toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
                } else {
                    NotificationService.genericAlertError(
                        'Warning',               
                        `${error?.response?.data?.returnMessage}`,
                        'warning',
                    );
                  
                }
            })
            .finally(() => {
                setIsLoading(false);
            });
    }

    const onSubmit = async (data: any) => {
        
        const user = AuthService.getUserInfo();
        const fechaCierreValidar = moment(formularioCierreDiarioCartera.getValues('fechaCierre')).utcOffset('-05:00');
        
        ejecutarProceso = true;

        const request = {
            fechaCierre: fechaCierreValidar.format(),
            actualiza: data.actualizacion === "1"
        }
        if (fechaUltimoCierreDiario !== null && fechaUltimoCierreDiario.length > 0 && fechaUltimoCierreDiario[0].fechaCierre !== null) {
            const fechaUltimoCierre = new Date(fechaUltimoCierreDiario[0]?.fechaCierre);
            const esfechaIgualAlUltimoCierre = moment(request.fechaCierre).isSame(fechaUltimoCierre, 'day');
            const esFechaAnteriorAlUltimoCierre = moment(request.fechaCierre).isBefore(fechaUltimoCierre,'day');
            const ultimoDiaMes = moment(fechaUltimoCierre).endOf('month').startOf('day');
            const ultimoCierreEsFinMes = moment(fechaUltimoCierre).isSame(ultimoDiaMes.toDate(), 'day');
            const esSuperiorAlMesCierre = (!(ultimoCierreEsFinMes) && moment(request.fechaCierre).isAfter(ultimoDiaMes.toDate(),'day'));
            const debeCerrarMesAnterior = ultimoCierreEsFinMes && moment(request.fechaCierre).isAfter(moment(ultimoDiaMes).add(1, 'months'), 'month');

            if(esfechaIgualAlUltimoCierre){        
                NotificationService.genericAlertError(
                    'Error',               
                    'Ya se ejecutó el cierre para esta fecha',
                    'error',
                );
                return
            }

            if(esFechaAnteriorAlUltimoCierre){        
                NotificationService.genericAlertError(
                    'Error',               
                    `No se puede ejecutar un cierre diario con fecha anterior al último cierre: ${moment(fechaUltimoCierre).format('DD/MM/YYYY')}`,
                    'error',
                );
                return

            }

            if(esSuperiorAlMesCierre){        
                NotificationService.genericAlertError(
                    'Error',               
                    `La fecha del cierre diario, no puede ser el siguiente mes. (Primero debe realizar el cierre diario de: ${moment(ultimoDiaMes).format('DD/MM/YYYY')})`,
                    'error',
                );
                return

            }

            if(debeCerrarMesAnterior){        
                NotificationService.genericAlertError(
                    'Error',               
                    `No se puede realizar el cierre con esta fecha, debe realizarce antes el cierre: ${moment(ultimoDiaMes).add(1, 'months').endOf('month').format('DD/MM/YYYY')}`,
                    'error',
                );
                return

            }
        }  

        try {
            
            if (request.actualiza) {
                
                await swal({
                    text: 'Este proceso actualizará los datos. ¿Desea continuar?',
                    icon: 'warning',
                    buttons: { 
                        confirm: {
                            text: "Sí",
                            value: true,
                            visible: true,
                        },
                        cancel: {
                            text: "No",
                            value: false,
                            visible: true,
                        }
                     },
                }).then((respuesta) => {
                    if(!respuesta){
                        ejecutarProceso = false;
                    }
                })
            }

            if(ejecutarProceso ){
                setIsLocalLoading(true);
                const datos = {
                    fechaCierre: request.fechaCierre,
                    actualiza: request.actualiza,
                    creadoPor: user.USR_ID,
                }
                // await insertarCierreDiarioCorriente(datos, request)
                ejecutarCierreDiarioCorriente(datos)


            }
        } catch (error: any) {
            if (error.response && error.response.data ) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
            } else {
                toast("Error del sistema ", { type: toast.TYPE.ERROR });
            }
        }
          finally {
            setIsLocalLoading(false);
          }
          

    };

    if (isLocalLoading ) {
        return <CHLoadingProcess />;
    }

    return (
        <div  
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',
            
        }}>
            
        <LoadingV2Component key="loader" loading={isLoading} tittle='Ejecutando Causacion Diaria...' />

         <Header />
         <CHPrivateContainer>
            <PageIndicators
                topTextNivel1="Cierres Cartera"
                topTextNivel2="Causación diaria de intereses corrientes"
            />
            <IconTitle
                title="Causación Diaria"
                subTitle="Intereses corrientes"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
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
                                <Controller
                                    name="fechaCierre"
                                    control={formularioCierreDiarioCartera.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        validate: {
                                            isValidDate: value => moment(value, 'YYYY-MM-DD', true).isValid() ? true : 'La fecha ingresada no es válida.'
                                        }
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHDatePicker
                                            error={!!error}
                                            helperText={error ? error.message : ''}
                                            label="Fecha de cierre diario *"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="actualizacion"
                                        control={formularioCierreDiarioCartera.control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <RadioGroup
                                                value={value}
                                                onChange={onChange}
                                                row
                                                aria-label="actualización"
                                                defaultValue="0"
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
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={formularioCierreDiarioCartera.handleSubmit(onSubmit)}
                >
                    CIERRE
                </Button>   
                    <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={


                        formularioCierreDiarioCartera.handleSubmit(construirMovimientosContables)
                    }
                >
                    SEVEN
                </Button>

            </Grid>

            <br />
            <div className='MuiTableCell-stickyHeader; jss86'>
            <CierreTable 
                data={dataCierreDiario} 
                mostrarTabla={mostrarTabla}
                fechaCierre={formularioCierreDiarioCartera.getValues('fechaCierre')}
            />
            </div>
            
            </CHPrivateContainer>
        </div>
        
    );
};




export default CierreDiarioCartera;