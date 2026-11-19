import React, { useEffect, useRef, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import Header from '../../../components/header/Header';
import ApiproductoClients from '../../../services/api-producto.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import LoadingSpinner from '../../../components/ch-loading-process/LoadingSpinner';
import DownloadBlob from '../../../utils/blob';
import TablaReporteCierreMensualContable from './TablaReporteCierreMensualContable';
import TablaReporteTotalCierreMensualContable from './TablaReporteTotalCierreMensualContable';
import { TotalReporteCierreMensualContable } from '../types/TotalReporteCierreMensualContable';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalogApi from '../../../services/api-catalog.service';

type FormData = {
    periodo: Date,
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
        errorMessage: {
            color: 'red'
        },
    }),
);


const ReporteCierreMensualContable = () => {

    const classes = useStyles();
    const api = ApiproductoClients();
    const currentRequest = useRef<FormData>();
    const [isProccessing, setProcessing] = useState(false);
    const [data, setData] = useState<any[] | null>(null);
    const [totalRegistros, setTotalRegistros] = useState(0);

    const [resumen, setResumen] = useState<TotalReporteCierreMensualContable[] | null>();

    useEffect(() => {

    }, []);

    const currentDate = new Date();
    const periodo = moment(currentDate).endOf('month').startOf('day').toDate();

    const formControls = useForm<FormData>({
        'mode': 'onChange',
        defaultValues: {
            periodo: periodo,
            tipoCredito: 0
        },
    });

    const validateDate = (value: any) =>
        !moment(value).isValid()
            ? 'Seleccione una fecha valida'
            : undefined;

    // const onSubmit = async (request: any) => {
    //     if(formControls.formState.isValid) {            
    //         setProcessing(true);
    //         setResumen(null);
    //         setData(null);

    //         const anio = request.periodo.getFullYear();
    //         const mes = request.periodo.getMonth() + 1;

    //         const resumen = api.getReporteTotalCierreMensualContable(
    //             anio,
    //             mes,
    //             request.tipoCredito,
    //         )
    //         .then(x => {
    //             if(x.isSuccess) {
    //                 setResumen(x.data);
    //             } else {
    //                 setResumen([]);
    //             }
    //         });

    //         const report = api.getReporteCierreMensualContable({
    //             anio: anio,
    //             mes: mes,
    //             registrosPorPagina: 500,
    //             numeroPagina: 1
    //         })
    //         .then(x => {
    //             if(x.isSuccess) {
    //                 setData(x.data?.reporte ?? []);
    //                 setTotalRegistros(x.data?.reporte[0]?.totalRegistros ?? 0);
    //             } else {
    //                 setData([]);
    //                 setTotalRegistros(0);
    //                 toast(x.returnMessage, {type: 'error'});
    //             }
    //             currentRequest.current = request;
    //         });


    //         Promise.all([resumen, report]).finally(() => setProcessing(false));
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
        if (!formControls.formState.isValid) return;

        setProcessing(true);
        setResumen(null);
        setData(null);

        const anio = request.periodo.getFullYear();
        const mes = request.periodo.getMonth() + 1;

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const [resumenResp, reporteResp] = await Promise.all([
                api.getReporteTotalCierreMensualContable(anio, mes, request.tipoCredito),
                api.getReporteCierreMensualContable({
                    anio,
                    mes,
                    registrosPorPagina: 500,
                    numeroPagina: 1
                }),
            ]);

            if (resumenResp.isSuccess) {
                setResumen(resumenResp.data);
            } else {
                setResumen([]);
            }

            if (reporteResp.isSuccess) {
                const datos = reporteResp.data?.reporte ?? [];
                setData(datos);
                setTotalRegistros(datos[0]?.totalRegistros ?? 0);


                const requestAuditoria: Auditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Cierre mensual contable',
                    hostname: document.location.host,
                    tipoAccion: 'CONSULTAS',
                    idTipoAccion: 3,
                    descripcion: `Consulta del cierre mensual contable con los siguientes filtros:
                        - Año: ${anio}
                        - Mes: ${mes}
                        - Tipo de Crédito: ${request.tipoCredito ?? 'No especificado'}
                        - Total registros: ${datos.length}`
                };

                auditoriaApi.InsertAuditoria(requestAuditoria);

            } else {
                setData([]);
                setTotalRegistros(0);
                toast(reporteResp.returnMessage, { type: 'error' });
            }

            currentRequest.current = request;

        } catch (error: any) {
            const descripcionError = `Error al consultar cierre mensual contable.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Cierre mensual contable',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar el cierre mensual contable', { type: 'error' });
        } finally {
            setProcessing(false);
        }
    };


    const changePage = async (page: number, rowsPerPage: number) => {

        if (currentRequest.current) {
            setProcessing(true);

            const anio = currentRequest.current.periodo.getFullYear();
            const mes = currentRequest.current.periodo.getMonth() + 1;

            api.getReporteCierreMensualContable({
                anio: anio,
                mes: mes,
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

        if (req) {
            const anio = req.periodo.getFullYear();
            const mes = req.periodo.getMonth() + 1;

            await api.generarExcelReporteCierreMensualContable(anio, mes, req.tipoCredito)
                .then(response => {
                    const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                    const filename = `Reporte cierre contable ${moment(req.periodo).format('YYYY-MM')}.xlsx`;
                    DownloadBlob(blob, filename);
                }).catch(() => toast('No se pudo generar el reporte', { type: 'error' }));
        }
    }

    return (<div
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',

        }}>
        <Header />
        <CHPrivateContainer>
            <IconTitle
                title="Reporte cierre mensual"
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
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="periodo"
                                        control={formControls.control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                            validate: validateDate
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <div>
                                                <CHDatePicker
                                                    error={!!error}
                                                    label="Periodo"
                                                    value={value}
                                                    onChange={onChange}
                                                    views={['year', 'month']}
                                                    format="yyyy-MM"
                                                />
                                                <span className={classes.errorMessage}>{error?.message}</span>
                                            </div>
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <Grid container className="buttonssolicitud">
                                        <Button
                                            type='submit'
                                            className={classes.buttonprincipal}
                                            variant="outlined"
                                            disabled={!formControls.formState.isValid || isProccessing}
                                            onClick={formControls.handleSubmit(onSubmit)}>
                                            Generar
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Grid>
                            <br />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>

            {resumen && <TablaReporteTotalCierreMensualContable data={resumen} formData={currentRequest.current} />}
            {data && <TablaReporteCierreMensualContable {...{ data, totalRegistros, handleExportToExcel, changePage, isProccessing }} />}
            {!data && isProccessing && <LoadingSpinner size={40} />}

        </CHPrivateContainer>
    </div>)
}

export default ReporteCierreMensualContable;