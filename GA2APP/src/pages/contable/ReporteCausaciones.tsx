import React, { useEffect, useRef, useState } from 'react';
import { Button, Card, CardContent, CircularProgress, createStyles, Grid, IconButton, makeStyles, MenuItem, TableCell, TablePagination, TableRow, Tooltip } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable, { MUIDataTableColumnDef, MUIDataTableTextLabels } from 'mui-datatables';
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
import ResumenReporteContable from './types/resumenReporteContable.type';
import Nullable from '../../types/nullable.type';
import LoadingSpinner from '../../components/ch-loading-process/LoadingSpinner';
import DownloadBlob from '../../utils/blob';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise';

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
    data: any[],
    totalRegistros: number,
    resumen: Nullable<ResumenReporteContable>,
    handleExportToExcel: () => Promise<void>,
    changePage: (page: number, rowsPerPage: number) => Promise<void>,
    isProccessing: boolean
}


const TablaReporteContable = (props: tableProps) => {

    const { data, resumen, handleExportToExcel, changePage, totalRegistros, isProccessing } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);
    const [rowsPerPage, setRowsPerPage] = React.useState(500);
    const disableProps = { disabled: true };

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

    const columns: MUIDataTableColumnDef[] = [
        {
            name: 'comprobanteInterno',
            label: 'Comprobante',
            options: defaultOptions
        },
        {
            name: 'tipoOperacion',
            label: 'Tipo Operación',
            options: defaultOptions
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

        },
        {
            name: 'numeroIdentificacion',
            label: 'Número Identificación',
            options: defaultOptions,
        },
        {
            name: 'numeroCredito',
            label: 'Número Crédito',
            options: defaultOptions,
        },
        {
            name: 'centroCostos',
            label: 'Area',
            options: defaultOptions
        },
        {
            name: 'cuentaSeven',
            label: 'Cuenta Local',
            options: defaultOptions
        },
        {
            name: 'cuentaNif',
            label: 'Cuenta NIF',
            options: defaultOptions
        },
        {
            name: 'centroCostosCredito',
            label: 'Centro de costos',
            options: defaultOptions
        },
        {
            name: 'descripcionCuenta',
            label: 'Descripción Cuenta',
            options: defaultOptions
        },
        {
            name: 'debito',
            label: 'Débito',
            options: defaultOptionsCurrency
        },
        {
            name: 'credito',
            label: 'Crédito',
            options: defaultOptionsCurrency
        }
    ];

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();

        } finally {
            setDownloading(false);
        }
    }

    return (
        <div className='MuiTableCell-stickyHeader; jss86'>
            <MUIDataTable
                title=""
                data={data ?? []}
                columns={columns}
                options={{
                    onChangePage(currentPage) {
                        changePage(currentPage + 1, rowsPerPage);
                    },
                    selectableRows: 'none',
                    filter: false,
                    print: false,
                    search: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    responsive: 'vertical',
                    fixedHeader: true,
                    fixedSelectColumn: true,
                    tableBodyHeight: '500px',
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
                    customTableBodyFooterRender: (opts) =>
                        <TableRow className={classes.summaryRow}>
                            {opts.data.length > 0 && (
                                opts.columns.map((col, index) => {
                                    if (index === 0)
                                        return <TableCell key={index}><b>TOTAL</b></TableCell>

                                    if (col.name === 'debito')
                                        return <TableCell key={index} className={classes.summaryCell}>
                                            {UtilService.formatCurrency(resumen?.totalValorDebito ?? 0)}
                                        </TableCell>

                                    if (col.name === 'credito')
                                        return <TableCell key={index} className={classes.summaryCell}>
                                            {UtilService.formatCurrency(resumen?.totalValorCredito ?? 0)}
                                        </TableCell>

                                    return <TableCell key={index} />
                                })
                            )}
                        </TableRow>,

                    customFooter: (rowCount: number,
                        page: number,
                        rowsPerPage: number,
                        changeRowsPerPage: (page: string | number) => void,
                        changePage: (newPage: number) => void,
                        textLabels: Partial<MUIDataTableTextLabels>) => {
                        useEffect(() => changeRowsPerPage(500), []);
                        return (
                            <TablePagination
                                rowsPerPageOptions={[100, 300, 500]}
                                colSpan={3}
                                count={totalRegistros}
                                rowsPerPage={rowsPerPage}
                                page={page}
                                labelRowsPerPage='Filas por página'
                                onPageChange={(_: any, page: number) => changePage(page)}
                                onRowsPerPageChange={(event) => {
                                    const value = parseInt(event.target.value, 10);
                                    changeRowsPerPage(value);
                                    setRowsPerPage(value);
                                    changePage(0);
                                }}
                                backIconButtonProps={isProccessing ? disableProps : undefined}
                                nextIconButtonProps={isProccessing ? disableProps : undefined}
                                SelectProps={isProccessing ? disableProps : undefined}
                            />
                        );
                    }
                }}
            />
        </div>
    );
}

const ResumenInfo = (props: { resumen: ResumenReporteContable }) => {

    const classes = useStyles();
    const { resumen } = props;

    return (
        <Grid container>
            <Grid style={{ 'margin': '1rem 0' }} item xs={12}>
                <Alert severity="info" elevation={2}>
                    <AlertTitle><strong>Reporte</strong></AlertTitle>
                    <div style={{ 'display': 'flex', 'flexWrap': 'wrap', 'gap': '6rem' }}>
                        <div className={classes.infoPanel}>
                            <strong>Total créditos:</strong>
                            <span className={classes.totales}>{resumen?.totalCreditos ?? 0}</span>
                            {resumen?.totalPorTipoCredito?.map((resumen: any) =>
                                <React.Fragment key={resumen.tipoCredito}>
                                    <strong>{resumen?.tipoCredito ?? ''}: </strong>
                                    <span className={classes.totales}>{resumen?.totalCreditos ?? 0}</span>
                                </React.Fragment>
                            )}
                        </div>
                        <div className={classes.infoPanel}>
                            <strong>Total causación:</strong>
                            <span className={classes.totales}>{UtilService.formatCurrencyDos(resumen?.totalValorDebito ?? 0)}</span>
                            {resumen?.totalPorTipoCredito?.map((resumen: any) =>
                                <React.Fragment key={resumen.tipoCredito}>
                                    <strong>{resumen?.tipoCredito ?? ''}: </strong>
                                    <span className={classes.totales}>
                                        {UtilService.formatCurrencyDos(resumen?.totalValorDebito ?? 0)}
                                    </span>
                                </React.Fragment>
                            )}
                        </div>
                    </div>
                </Alert>
            </Grid>
        </Grid>
    );
}

const ReporteCausaciones = () => {

    const classes = useStyles();
    const api = ApiproductoClients();
    const currentRequest = useRef<any>(null);
    const [isProccessing, setProcessing] = useState(false);
    const [data, setData] = useState<Nullable<any[]>>(null);
    const [totalRegistros, setTotalRegistros] = useState(0);

    const [resumen, setResumen] = useState<Nullable<ResumenReporteContable>>(null);

    useEffect(() => {

    }, []);

    const currentDate = new Date();
    const fechaInicial = moment(currentDate).startOf('month').toDate();
    const fechaFinal = moment(currentDate).startOf('day').toDate();



    const formControls = useForm({
        'mode': 'onChange',
        defaultValues: {
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal,
            tipoCredito: 0
        },
    });

    const fechaIni = formControls.watch('fechaInicial');
    const fechaFin = formControls.watch('fechaFinal');
    const tipoCredito = formControls.watch('tipoCredito');

    const validateDate = (value: any) =>
        !moment(value).isValid()
            ? 'Seleccione una fecha valida'
            : undefined;

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
        if (formControls.formState.isValid) {
            setProcessing(true);
            setResumen(null);
            setData(null);

            const resumen = api.getReporteTotalCausaciones(
                request.fechaInicial,
                request.fechaFinal,
                request.tipoCredito,
            )
                .then(x => {
                    if (x.isSuccess) {
                        setResumen(x.data);
                    } else {
                        setResumen({
                            totalCreditos: 0,
                            totalValorDebito: 0,
                            totalValorCredito: 0,
                            totalPorTipoCredito: [],
                        });
                    }
                });

            const report = api.getReporteCausaciones({
                fechaInicial: request.fechaInicial,
                fechaFinal: request.fechaFinal,
                tipoCredito: request.tipoCredito,
                registrosPorPagina: 500,
                numeroPagina: 1
            })
                .then(x => {
                    if (x.isSuccess) {
                        setData(x.data?.reporte ?? []);
                        setTotalRegistros(x.data?.reporte[0]?.totalRegistros ?? 0);
                    } else {
                        setData([]);
                        setTotalRegistros(0);
                        toast(x.returnMessage, { type: 'error' });
                    }
                    currentRequest.current = request;
                });

            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

            let tipoCreditos = 'No especificado';

            if (tipoCredito === 0) {
                tipoCreditos = 'CONSULTA TOTAL CARTERA'
            } else if (tipoCredito === 1) {
                tipoCreditos = 'LEASING HABITACIONAL'
            } else if (tipoCredito === 2) {
                tipoCreditos = 'HIPOTECARIO'
            }

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Cierre mensual de cartera',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de reporte causación contable con los siguientes filtros:
                - Fecha Inicial: ${fechaIni || 'No especificada'}
                - Fecha Final: ${fechaFin || 'No especificada'}
                - Tipo Crédito: ${tipoCreditos || 'No especificada'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);


            Promise.all([resumen, report]).finally(() => setProcessing(false));
        }
    };

    const changePage = async (page: number, rowsPerPage: number) => {
        console.log(page, rowsPerPage);
        if (currentRequest.current) {
            setProcessing(true);
            api.getReporteCausaciones({
                fechaInicial: currentRequest.current.fechaInicial,
                fechaFinal: currentRequest.current.fechaFinal,
                tipoCredito: currentRequest.current.tipoCredito,
                registrosPorPagina: rowsPerPage,
                numeroPagina: page
            })
                .then(x => {
                    if (x.isSuccess) {
                        setData(x.data?.reporte ?? []);
                        setTotalRegistros(x.data?.reporte[0]?.totalRegistros ?? 0);
                    } else {
                        setData([]);
                        setTotalRegistros(0);
                        toast(x.returnMessage, { type: 'error' });
                    }
                }).finally(() => setProcessing(false));
        }
    };

    const handleExportToExcel = async () => {
        const req = currentRequest.current;
        const requestAuditoria: any = {
            modulo: 'Cierres Cartera',
            fecha: new Date(),
        };

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        try {
            const response = await api.generarExcelReporteCausaciones(
                req.fechaInicial,
                req.fechaFinal,
                req.tipoCredito,
            );

            const blob = new Blob([response], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            });
            const filename = `Reporte Causaciones - Fecha ${moment(req.fechaInicial).format('YYYY-MM-DD')} a ${moment(req.fechaFinal).format('YYYY-MM-DD')}.xlsx`;
            DownloadBlob(blob, filename);


            requestAuditoria.tipoAccion = 'DESCARGAS';
            requestAuditoria.idTipoAccion = 5;
            requestAuditoria.navegador = usrAg;
            requestAuditoria.sistemaOperativo = nav.userAgentData.platform;
            requestAuditoria.nombreUsuario = user.USR_USERNAME;
            requestAuditoria.usurioId = user.USR_ID;
            requestAuditoria.ip = publicIpAddress;

            await auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error) {
            let errorMessage = 'Error desconocido';
            if (error instanceof Error) {
                errorMessage = error.message;
            }

            requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion = 8;
            requestAuditoria.descripcion = `Error al generar el reporte de causaciones:
                \nFecha inicial: ${moment(req.fechaInicial).format("DD/MM/YYYY")}
                \nFecha final: ${moment(req.fechaFinal).format("DD/MM/YYYY")}
                \nTipo de crédito: ${req.tipoCredito || 'No especificado'}
                \nError: ${errorMessage}
            `;
            await auditoriaApi.InsertAuditoria(requestAuditoria);

            toast('No se pudo generar el reporte', { type: 'error' });
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
                title="Reporte Causación Contable Intereses y Seguros"
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

            {
                !isProccessing &&
                <Grid container className="buttonssolicitud">
                    <Button
                        type='submit'
                        className={classes.buttonprincipal}
                        variant="outlined"
                        disabled={!formControls.formState.isValid}
                        onClick={formControls.handleSubmit(onSubmit)}>
                        Generar
                    </Button>
                </Grid>
            }
            {resumen && <ResumenInfo resumen={resumen} />}
            {data && <TablaReporteContable {...{ data, totalRegistros, resumen, handleExportToExcel, changePage, isProccessing }} />}
            {isProccessing && <LoadingSpinner size={40} />}

        </CHPrivateContainer>
    </div>)
}

export default ReporteCausaciones;