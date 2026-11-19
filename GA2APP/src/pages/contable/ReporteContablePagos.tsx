import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, CardHeader, CircularProgress, createStyles, Grid, IconButton, makeStyles, MenuItem, Table, TableCell, TableRow, Tooltip } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable from 'mui-datatables';
import moment from 'moment';
import { toast } from 'react-toastify';
import { CloudDownload } from '@material-ui/icons';
import { Alert, AlertTitle } from '@material-ui/lab';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import Header from '../../components/header/Header';
import UtilService from '../../utils/util.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import ApiproductoClients from '../../services/api-producto.service';
import CHSelect from '../../components/ch-select/CHSelect';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import ReporteContable from './types/reporteContable.type';
import handleExportToExcel from '../Reports/helpers/handleExportToExcel';
import { ColumnFormatType } from '../../enums/ColumnFormatType';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';


interface tableProps {
    data: ReporteContable[],
    formData: FormData,
    cuentasOrden?: boolean
}

interface FormData {
    fechaInicial: Date,
    fechaFinal: Date,
    tipoCredito: number
}

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
        nombreCuentaContable: {
            minHeight: '200px'
        },
        errorMessage: {
            color: 'red'
        },
        summaryCell: {
            textAlign: 'right',
            fontWeight: 'bold',
        },
        summaryRow: {
            backgroundColor: '#f1f1f1'
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

const CUENTAS_ORDEN = [
    '84050302',
    '82810201',
    '82810202',
    '84050305',
];

const TablaReporteContable = (props: tableProps) => {

    const { data, formData, cuentasOrden } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);

    const defaultOptions = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };

    const defaultOptionsDate = {
        setCellProps: () => ({
            className: classes.columnDate,
            style: {
                textAlign: 'left',
            },

        }),
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            className: 'filas'
        }),

    };

    const defaultOptionsCurrency = {
        customBodyRender: (value: number | bigint) =>
            UtilService.formatCurrency(value),
        setCellProps: () => ({
            style: {
                textAlign: 'right'

            },
        }),
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };

    const columns = [
        {
            name: 'tipoOperacion',
            label: 'Tipo Operación',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'descripcionOperacion',
            label: 'Descripción Operación',
            options: defaultOptions
        },
        {
            name: 'fechaOperacion',
            label: 'Fecha Operación',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'numeroIdentificacion',
            label: 'Número Identificación',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'numeroCredito',
            label: 'Número Crédito',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'cuentaSeven',
            label: 'Cuenta Seven',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'descripcionCuenta',
            label: 'Descripción Cuenta',
            options: defaultOptions
        },
        {
            name: 'debito',
            label: 'Débito',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'credito',
            label: 'Crédito',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        }
    ];

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]
        const hexadecimal = convertIp
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

    const handleDownload = async () => {
        const formatFecha = `${moment(formData.fechaInicial).format('YYYY-MM-DD')} a ${moment(formData.fechaFinal).format('YYYY-MM-DD')}`;
        const fileName = `Reporte Contable Pagos - ${cuentasOrden ? 'Cuentas de Orden - ' : ''} Fecha ${formatFecha}.xlsx`;
        await handleExportToExcel(data, columns, fileName, publicIpAddress);
    }

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleDownload();
        } finally {
            setDownloading(false);
        }
    }

    const filterFunc = (sum: number, val: number) => sum + val;
    const mapFunc = (e: any) => (e.debito ?? 0);
    const uniqueValuesFunc = (array: string[]) => array.filter((valor, indice, self) => self.indexOf(valor) === indice).length;

    const movimientosLeasing = data?.filter(x => x.idTipoCredito === 1);
    const valorTotalLeasing = movimientosLeasing
        .map(mapFunc).reduce(filterFunc, 0).toFixed(2);

    const movimientosHipotecario = data?.filter(x => x.idTipoCredito === 2);
    const valorTotalHipotecario = movimientosHipotecario
        .map(mapFunc).reduce(filterFunc, 0).toFixed(2);

    const totalCausacion = parseFloat(valorTotalLeasing) + parseFloat(valorTotalHipotecario);

    const totalCreditosHipotecarios = uniqueValuesFunc(movimientosHipotecario.map(x => x.numeroIdentificacion));
    const totalCreditosLeasings = uniqueValuesFunc(movimientosLeasing.map(x => x.numeroIdentificacion));
    const totalCreditos = totalCreditosLeasings + totalCreditosHipotecarios;

    return (<div className='MuiTableCell-stickyHeader; jss86'>
        <Grid container>
            <Grid style={{ 'margin': '1rem 0' }} item xs={12}>
                <Alert severity="info">
                    <AlertTitle><strong>Reporte</strong></AlertTitle>
                    <div style={{ 'display': 'flex', 'flexWrap': 'wrap', 'gap': '2rem' }}>
                        <div className={classes.infoPanel}>
                            <strong>Total Creditos:</strong>
                            <span className={classes.totales}>{totalCreditos}</span>
                            <strong>Leasing Habitacional:</strong>
                            <span className={classes.totales}>{totalCreditosLeasings}</span>
                            <strong>Hipotecario:</strong>
                            <span className={classes.totales}>{totalCreditosHipotecarios}</span>
                        </div>
                        <div className={classes.infoPanel}>
                            <strong>Total Valor Pagos:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrency(totalCausacion)}</span>
                            <strong>Leasing Habitacional:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrency(valorTotalLeasing)}</span>
                            <strong>Hipotecario:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrency(valorTotalHipotecario)}</span>
                        </div>
                    </div>
                </Alert>
            </Grid>
        </Grid>

        <MUIDataTable
            title=""
            data={data}
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
                tableBodyHeight: '500px',
                rowsPerPageOptions: [5, 15, 20, 100, 200, 500],
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
                    <Tooltip title="Descargar Excel">
                        {
                            downloading
                                ? <CircularProgress size={20} style={{ 'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px' }} />
                                : <IconButton onClick={handleClick} >
                                    <CloudDownload />
                                </IconButton>
                        }
                    </Tooltip>
                ),
                customTableBodyFooterRender: (opts) => {

                    const valores = opts.data.reduce((total, item) => ({
                        credito: total.credito + (data[item.dataIndex]?.credito ?? 0),
                        debito: total.debito + (data[item.dataIndex]?.debito ?? 0),
                    }), { credito: 0, debito: 0 });

                    return <TableRow className={classes.summaryRow}>
                        {opts.data.length > 0 && (
                            opts.columns.map((col, index) => {
                                if (index === 0)
                                    return <TableCell key={index}><b>TOTAL</b></TableCell>

                                if (col.name === 'debito')
                                    return <TableCell key={index} className={classes.summaryCell}>{UtilService.formatCurrency(valores.debito)}</TableCell>

                                if (col.name === 'credito')
                                    return <TableCell key={index} className={classes.summaryCell}>{UtilService.formatCurrency(valores.credito)}</TableCell>

                                return <TableCell key={index} />
                            })
                        )}
                    </TableRow>
                }
            }}
        />
    </div>);
}


const ReporteContablePagos = () => {

    const classes = useStyles();
    const api = ApiproductoClients();
    const [request, setRequest] = useState<FormData | null>(null);
    const [isProccessing, setProcessing] = useState(false);
    const [dataReporteGeneral, setDataReporteGeneral] = useState<ReporteContable[] | null>(null);
    const [dataReporteCuentasOrden, setDataReporteCuentasOrden] = useState<ReporteContable[] | null>(null);

    const currentDate = new Date();
    const fechaInicial = moment(currentDate).startOf('month').toDate();
    const fechaFinal = moment(currentDate).startOf('day').toDate();

    const formControls = useForm<FormData>({
        'mode': 'onChange',
        defaultValues: {
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal,
            tipoCredito: 0
        },
    });

    const validateDate = (value: any) =>
        !moment(value).isValid()
            ? 'Seleccione una fecha valida'
            : undefined;

    // const onSubmit = async (request: any) => {

    //     setRequest(request);

    //     if(formControls.formState.isValid) {            
    //         setProcessing(true);
    //         setDataReporteGeneral(null);
    //         setDataReporteCuentasOrden(null);

    //         api.getReporteContablePagos(
    //             request.fechaInicial,
    //             request.fechaFinal,
    //             request.tipoCredito,
    //         )
    //         .then(x => {
    //             if(x.isSuccess && x.data) {

    //                 const datosReporteGeneral: ReporteContable[] = [];
    //                 const datosReporteCuentasOrden: ReporteContable[] = [];

    //                 x.data?.forEach((item: ReporteContable) => 
    //                     CUENTAS_ORDEN.includes(item.cuentaSeven)
    //                         ? datosReporteCuentasOrden.push(item)
    //                         : datosReporteGeneral.push(item)
    //                 );

    //                 setDataReporteGeneral(datosReporteGeneral);
    //                 setDataReporteCuentasOrden(datosReporteCuentasOrden);
    //             } else {
    //                 setDataReporteGeneral([]);
    //                 setDataReporteCuentasOrden([]);
    //                 toast(x.returnMessage, {type: 'error'})
    //             }
    //         })
    //         .finally(() => setProcessing(false));
    //     }
    // };

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]
        const hexadecimal = convertIp
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

    const onSubmit = async (request: any) => {
        setRequest(request);

        if (!formControls.formState.isValid) return;

        setProcessing(true);
        setDataReporteGeneral(null);
        setDataReporteCuentasOrden(null);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const response = await api.getReporteContablePagos(
                request.fechaInicial,
                request.fechaFinal,
                request.tipoCredito,
            );

            if (response.isSuccess && response.data) {
                const datosReporteGeneral: ReporteContable[] = [];
                const datosReporteCuentasOrden: ReporteContable[] = [];

                response.data.forEach((item: ReporteContable) =>
                    CUENTAS_ORDEN.includes(item.cuentaSeven)
                        ? datosReporteCuentasOrden.push(item)
                        : datosReporteGeneral.push(item)
                );

                setDataReporteGeneral(datosReporteGeneral);
                setDataReporteCuentasOrden(datosReporteCuentasOrden);


                const requestAuditoria: Auditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Reporte Contable de Pagos',
                    hostname: document.location.host,
                    tipoAccion: 'CONSULTAS',
                    idTipoAccion: 3,
                    descripcion: `Consulta de reporte contable con los siguientes filtros:
                        - Fecha Inicial: ${request.fechaInicial}
                        - Fecha Final: ${request.fechaFinal}
                        - Tipo de Crédito: ${request.tipoCredito ?? 'No especificado'}`
                };

                auditoriaApi.InsertAuditoria(requestAuditoria);

            } else {
                setDataReporteGeneral([]);
                setDataReporteCuentasOrden([]);
                toast(response.returnMessage, { type: 'error' });
            }

        } catch (error: any) {
            // Auditoría de error
            const descripcionError = `Error al consultar reporte contable.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Reporte Contable de Pagos',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al generar el reporte contable', { type: 'error' });
        } finally {
            setProcessing(false);
        }
    };

    return (<div
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',

        }}>
        <Header />
        <CHPrivateContainer>
            <IconTitle
                title="Reporte contable de pagos"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaInicial"
                                        control={formControls.control}
                                        rules={{
                                            required: 'Se requiere la Fecha Inicial',
                                            validate: validateDate
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <div>
                                                <CHDatePicker
                                                    error={!!error}
                                                    label="Fecha inicial"
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                                <span className={classes.errorMessage}>{error?.message}</span>
                                            </div>
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaFinal"
                                        control={formControls.control}
                                        rules={{
                                            required: 'Se requiere la Fecha Final',
                                            validate: validateDate
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <div>
                                                <CHDatePicker
                                                    error={!!error}
                                                    label="Fecha final"
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                                <span className={classes.errorMessage}>{error?.message}</span>
                                            </div>
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={5}>
                                    <br />
                                    <Controller
                                        name="tipoCredito"
                                        control={formControls.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label="Tipo Crédito"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }>
                                                <MenuItem value={0}>CONSULTA TOTAL CARTERA</MenuItem>
                                                <MenuItem value={1}>LEASING HABITACIONAL</MenuItem>
                                                <MenuItem value={2}>HIPOTECARIO</MenuItem>
                                            </CHSelect>
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
                    type='submit'
                    className={classes.buttonprincipal}
                    variant="outlined"
                    disabled={!formControls.formState.isValid}
                    onClick={formControls.handleSubmit(onSubmit)}>
                    Generar
                </Button>
            </Grid>

            <LoadingV2Component key="loader" loading={isProccessing} tittle='Generando reporte...' />

            <br />

            {
                dataReporteGeneral && request &&
                <Card>
                    <CardContent>
                        <TablaReporteContable data={dataReporteGeneral} formData={request} />
                    </CardContent>
                </Card>
            }

            <br />

            {
                dataReporteCuentasOrden && request &&
                <Card>
                    <CardContent>
                        <h3>CUENTAS DE ORDEN</h3>
                        <TablaReporteContable data={dataReporteCuentasOrden} formData={request} cuentasOrden />
                    </CardContent>
                </Card>
            }
        </CHPrivateContainer>
    </div>
    );
}

export default ReporteContablePagos;