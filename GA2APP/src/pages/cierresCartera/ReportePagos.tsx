import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, CircularProgress, createStyles, Grid, IconButton, makeStyles, MenuItem, Table, TableCell, TableRow, Tooltip } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable, { MUIDataTableColumnDef } from 'mui-datatables';
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
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import { useIp } from '../../shared/context/IpContext';
import { ColumnFormatType } from '../../enums/ColumnFormatType';
import handleReporteGarantia from '../Reports/helpers/handleReporteGarantia';

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

interface tableProps {
    data: any,
    mostrarTabla: boolean,
    currentRequest: any
}

const TablaReportePagos = (props: tableProps) => {

    const { data, mostrarTabla, currentRequest } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);

    const defaultOptions = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };

    const defaultOptionsCenter = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
        setCellProps: () => ({
            className: classes.columnDate,
            style: {
                textAlign: 'center',
            },
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
            name: 'unidadEjecutora',
            label: 'Unidad Ejecutora',
            options: defaultOptions
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Credito',
            options: defaultOptions
        },
        {
            name: 'numeroCredito',
            label: 'Número Crédito',
            options: defaultOptions,
        },
        {
            name: 'identificacion',
            label: 'Número Identificación',
            options: defaultOptions,
        },
        {
            name: 'afiliado',
            label: 'Afiliado',
            options: defaultOptions,
        },
        {
            name: 'comprobanteInterno',
            label: 'Comprobante Interno',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: number | string | null | undefined) => value ?? "N/A",
            },
        },
        {
            name: 'tipoPago',
            label: 'Tipo Pago',
            options: defaultOptions,
        },
        {
            name: 'modalidadPago',
            label: 'Modalidad de Pago',
            options: defaultOptions,
        },
        {
            name: 'tipoRecurso',
            label: 'Origen de Pago',
            options: {...defaultOptions, 
                customBodyRender: (value: string) => {

                if(value === 'RECURSOS_PROPIOS'){
                    return 'Recursos Propios'
                }
                if(value === 'CUENTA_INDIVIDUAL'){
                    return 'Cuenta Individual'
                }
                return  'N/A'},
            },
        },
        {
            name: 'fechaAplicacion',
            label: 'Fecha Aplicación',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaPago',
            label: 'Fecha Pago',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'valorPago',
            label: 'Valor Pago',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
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

    const handleExportToExcel = async () => {

        const fileName = `Reporte Pagos - Fecha ${moment(currentRequest.fechaInicial).format('YYYY-MM-DD')} a ${moment(currentRequest.fechaFinal).format('YYYY-MM-DD')}.xlsx`;
        await handleReporteGarantia(data?.reporte, columns, fileName, publicIpAddress, "Reporte de Pagos");

}

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }


    const columnsReportePorUnidad: MUIDataTableColumnDef[] = [
        {
            name: 'unidadEjecutora',
            label: 'Unidad Ejecutora',
            options: defaultOptions
        },
        ...
        (data.resumen?.totalPorTipoCredito?.flatMap((tipo: any) => [
            {
                name: `totalCreditos${tipo.codigoTipoCredito}`,
                label: `Total Créditos ${tipo.tipoCredito}`,
                options: defaultOptionsCenter
            },
            {
                name: `valorPagos${tipo.codigoTipoCredito}`,
                label: `Total Pago ${tipo.tipoCredito}`,
                options: defaultOptionsCurrency
            },
        ]) ?? [])
    ];

    const dataReportePorUnidad = data?.resumen?.totalPorUnidadEjecutora.map((unidad: any) =>
        Object.assign({ unidadEjecutora: unidad.unidadEjecutora },
            ...
            unidad.tiposCredito?.map((tipo: any) => ({
                [`totalCreditos${tipo.codigoTipoCredito}`]: tipo.totalCreditos,
                [`valorPagos${tipo.codigoTipoCredito}`]: tipo.totalValor
            }
            ))
        ));

    if (mostrarTabla && data?.resumen) {

        return <>
            <Grid container>
                <Grid style={{ 'margin': '1rem 0' }} item xs={12}>
                    <Alert severity="info">
                        <AlertTitle><strong>Reporte</strong></AlertTitle>
                        <div style={{ 'display': 'flex', 'flexWrap': 'wrap', 'gap': '6rem' }}>
                            <div className={classes.infoPanel}>
                                <strong>Total créditos:</strong>
                                <span className={classes.totales}>{data?.resumen?.totalCreditos ?? 0}</span>
                                {data?.resumen?.totalPorTipoCredito?.map((resumen: any) =>
                                    <React.Fragment key={resumen.tipoCredito}>
                                        <strong>{resumen?.tipoCredito ?? ''}: </strong>
                                        <span className={classes.totales}>{resumen?.totalCreditos ?? 0}</span>
                                    </React.Fragment>
                                )}
                            </div>
                            <div className={classes.infoPanel}>
                                <strong>Total valor pagos:</strong>
                                <span className={classes.totales}>{UtilService.formatCurrency(data?.resumen?.totalValor ?? 0)}</span>
                                {data?.resumen?.totalPorTipoCredito?.map((resumen: any) =>
                                    <React.Fragment key={resumen.tipoCredito}>
                                        <strong>{resumen?.tipoCredito ?? ''}: </strong>
                                        <span className={classes.totales}>
                                            {UtilService.formatCurrency(resumen?.totalValor ?? 0)}
                                        </span>
                                    </React.Fragment>
                                )}
                            </div>
                        </div>
                    </Alert>
                </Grid>
            </Grid>

            <MUIDataTable
                title=""
                data={data?.reporte ?? []}
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
                    tableBodyHeight: '600px',
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
                                    : <IconButton onClick={() => handleClick()} >
                                        <CloudDownload />
                                    </IconButton>
                            }
                        </Tooltip>
                    )
                }}
            />

            <br />

            <MUIDataTable
                title=""
                data={dataReportePorUnidad ?? []}
                columns={columnsReportePorUnidad}
                options={{
                    selectableRows: 'none',
                    filter: false,
                    print: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    responsive: 'vertical',
                    fixedHeader: true,
                    fixedSelectColumn: true,
                    tableBodyHeight: '400px',
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
                    }
                }}
            />
        </>
    }

    return <></>;
}

const ReportePagos = () => {

    const classes = useStyles();
    const api = ApiproductoClients();
    const catalogApi = CatalogApi();
    const [currentRequest, setRequest] = useState<any>(null);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const [isProccessing, setProcessing] = useState(false);
    const [mostrarTabla, setMostrarTabla] = useState(false);
    const [data, setData] = useState([]);

    const { publicIp } = useIp();

    useEffect(() => {
        cargarUnidadesEjecutoras();
    }, []);

    const currentDate = new Date();
    const fechaInicial = moment(currentDate).startOf('month').toDate();
    const fechaFinal = moment(currentDate).startOf('day').toDate();

    const formControls = useForm({
        'mode': 'onChange',
        defaultValues: {
            tipoCredito: 0,
            unidadEjecutora: 0,
            tipoFecha: 2,
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal,
        },
    });

    const cargarUnidadesEjecutoras = () => {
        catalogApi.getAllEXECUTINGUNITSRequest()
            .then(x => {
                if (x.isSuccess) {
                    setUnidadesEjecutoras(x.data)
                }
            })
            .catch(_ => setUnidadesEjecutoras([]));
    }


    const validateDate = (value: any) =>
        !moment(value).isValid()
            ? 'Seleccione una fecha valida'
            : undefined;

    const onSubmit = async (request: any) => {
        if (formControls.formState.isValid) {
            setProcessing(true);
            setMostrarTabla(false);
            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIp,
                modulo: 'Reporte de pagos',
                hostname: document.location.host
            } as Auditoria
            let tipoCredito = request.tipoCredito === 0 ? 'CONSULTA TOTAL CARTERA' : '';
            tipoCredito = request.tipoCredito === 1 ? 'LEASING HABITACIONAL' : tipoCredito;
            tipoCredito = request.tipoCredito === 2 ? 'HIPOTECARIO' : tipoCredito;
            api.getReportePagos(
                request.tipoFecha,
                request.fechaInicial,
                request.fechaFinal,
                request.tipoCredito,
                request.unidadEjecutora
            )
                .then(x => {

                    if (x.isSuccess) {
                        setData(x.data);
                        setMostrarTabla(true);
                        setRequest(request);

                        requestAuditoria.tipoAccion = 'TRANSACCIONES';
                        requestAuditoria.idTipoAccion = 2;
                        requestAuditoria.descripcion = `Reporte de pagos parámetros:
                         \n Tipo fecha:  ${request.tipoFecha === 1 ? 'FECHA DE APLICACIÓN' : 'FECHA DE PAGO'}
                         \n Fecha inicial:  ${moment(request.fechaInicial).format("DD/MM/YYYY")}
                         \n Fecha final:  ${moment(request.fechaFinal).format("DD/MM/YYYY")}
                         \n Tipo crédito:  ${tipoCredito}
                         \n Unidad ejecutora:  ${request.unidadEjecutora === 0 ? 'TODAS' : unidadesEjecutoras.find(x => x.id === request.unidadEjecutora).sigla}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        setData([]);
                        setMostrarTabla(false);
                        setRequest(request);
                        toast(x.returnMessage, { type: 'error' })
                        requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion = 8;
                        requestAuditoria.descripcion = `Reporte de pagos parámetros:
                    \n Tipo fecha:  ${request.tipoFecha === 1 ? 'FECHA DE APLICACIÓN' : 'FECHA DE PAGO'}
                    \n Fecha inicial:  ${moment(request.fechaInicial).format("DD/MM/YYYY")}
                    \n Fecha final:  ${moment(request.fechaFinal).format("DD/MM/YYYY")}
                    \n Tipo crédito:  ${tipoCredito}
                    \n Unidad ejecutora:  ${request.unidadEjecutora === 0 ? 'TODAS' : unidadesEjecutoras.find(x => x.id === request.unidadEjecutora).sigla}
                    \n returnApiMessage: ${x.returnMessage}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                }).catch((error: any) => {
                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion = 8;
                    requestAuditoria.descripcion = `Reporte de pagos parámetros:
                         \n Tipo fecha:  ${request.tipoFecha === 1 ? 'FECHA DE APLICACIÓN' : 'FECHA DE PAGO'}
                         \n Fecha inicial:  ${moment(request.fechaInicial).format("DD/MM/YYYY")}
                         \n Fecha final:  ${moment(request.fechaFinal).format("DD/MM/YYYY")}
                         \n Tipo crédito:  ${tipoCredito}
                         \n Unidad ejecutora:  ${request.unidadEjecutora === 0 ? 'TODAS' : unidadesEjecutoras.find(x => x.id === request.unidadEjecutora).sigla}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                })
                .finally(() => setProcessing(false));
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
                title="Reporte de pagos"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
                    <Card>
                        <CardContent>
                            <Grid container spacing={3}>
                                <Grid item xs={6} md={4}>
                                    <Controller
                                        name="tipoFecha"
                                        control={formControls.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label="Tipo Fecha"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={error ? error.message : null}>
                                                <MenuItem value={1}>FECHA APLICACIÓN</MenuItem>
                                                <MenuItem  value={2}>FECHA DE PAGO</MenuItem>
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={6} md={4}>
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
                                <Grid item xs={6} md={4}>
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
                                <Grid item xs={6} md={4}>
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
                                <Grid item xs={6} md={4}>
                                    <Controller
                                        name="unidadEjecutora"
                                        control={formControls.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                defaultValue=""
                                                label="Unidad Ejecutora"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            >
                                                <MenuItem value={0}>
                                                    TODAS
                                                </MenuItem>
                                                {unidadesEjecutoras?.map((item: any) => (
                                                    <MenuItem key={item.id} value={item.id}>
                                                        {item.nombre}
                                                    </MenuItem>
                                                ))}
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
            {
                isProccessing && <CHLoadingProcess />
            }
            <div className='MuiTableCell-stickyHeader; jss86'>
                <TablaReportePagos {...{ data, mostrarTabla, currentRequest }} />
            </div>
        </CHPrivateContainer>
    </div>)
}

export default ReportePagos;