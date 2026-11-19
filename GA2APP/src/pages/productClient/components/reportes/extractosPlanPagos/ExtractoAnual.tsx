import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import CHPaper from '../../../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../../../components/ch-private-container/CHPrivateContainer';
import Nullable from '../types/Nullable';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import UnidadEjecutora from '../types/UnidadEjecutora';
import CatalogApi from '../../../../../services/api-catalog.service';
import ApiproductoClients from '../../../../../services/api-producto.service';
import NotificationService from '../../../../../services/notification.service';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '0',
            marginTop: '0',
        },
        procesoReportesContenedor: {
            marginTop: '2rem',
            marginRight: '1rem',
            maxHeight: '500px',
            overflowY: 'auto',
        },
    }),
);

const obtenerUnidadesEjecutoras = (): Promise<UnidadEjecutora[]> => {
    const apiCatalog = CatalogApi();

    return apiCatalog
        .getAllEXECUTINGUNITSRequest()
        .then((response: any) => (response?.isSuccess && response.data) || [])
        .catch(() => []);
};

const ExtractoAnual = (props: any) => {
    const apiProductos = ApiproductoClients();
    const classes = useStyles();

    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<
        UnidadEjecutora[]
    >([]);

    const [loading, setLoading] = useState(false);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);

    const catalogApi = CatalogApi();

    const formulario = useForm({
        mode: 'onChange',
        defaultValues: {
            tipoCredito: 0,
            periodoReporte: null as Nullable<Date>,
            unidadEjecutora: 0 as Nullable<number>,
        },
    });

    useEffect(() => {
        obtenerUnidadesEjecutoras().then(data =>
            setUnidadesEjecutoras(data),
        );
    }, []);

    const tipoCredito = formulario.watch('tipoCredito');
    const unidadEjecutora = formulario.watch('unidadEjecutora');
    const periodoReporte = formulario.watch('periodoReporte');

    const nombreUnidadEjecutora = unidadesEjecutoras.find(
        item => item.id === unidadEjecutora,
    )?.nombre;

    useEffect(() => {
        // Se conserva el efecto existente.
    }, [
        tipoCredito,
        unidadEjecutora,
        periodoReporte,
        nombreUnidadEjecutora,
    ]);

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex =
            /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;

        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;

        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0];
        const hexadecimal = convertIp;

        await catalogApi
            .getObtenerIpLocal(hexadecimal)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setPublicIpAddress(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', {
                    type: toast.TYPE.ERROR,
                });
            });
    };

    useEffect(() => {
        const fetchLocalIPAddress = () => {
            const peerConnection = new RTCPeerConnection({
                iceServers: [],
            });

            peerConnection.createDataChannel('');

            peerConnection
                .createOffer()
                .then(offer =>
                    peerConnection.setLocalDescription(offer),
                )
                .catch(error =>
                    console.error(
                        'Error creating offer:',
                        error,
                    ),
                );

            peerConnection.onicecandidate = event => {
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

    const extraerMensajeErrorBlob = async (
        error: any,
    ): Promise<string> => {
        const dataError = error?.response?.data;

        if (dataError instanceof Blob) {
            try {
                const texto = await dataError.text();
                const json = JSON.parse(texto);

                return json?.returnMessage || 'No disponible';
            } catch {
                return 'No disponible';
            }
        }

        return dataError?.returnMessage || 'No disponible';
    };

    const onSubmit = async (data: any) => {
        setLoading(true);

        const CONSULTA_TOTAL = 0;
        const LEASING = 1;

        const anio = data.periodoReporte.getFullYear();

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();

        const nav = window.navigator as any;

        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) =>
                x.brand === 'Google Chrome' ||
                x.brand === 'Microsoft Edge',
        )?.brand;

        try {
            /**
             * Se mantiene la lógica del reporte:
             *
             * Año + mes 12
             *
             * Ejemplo:
             * 2026 -> 202612
             */
            const periodo = `${anio}12`;

            const blob =
                await apiProductos.generarReportePlanPagosExtractoMasivoAnual(
                    {
                        periodo,
                        tipoCredito: data.tipoCredito ?? 0,
                    },
                );

            if (blob && blob.size > 0) {
                const url = window.URL.createObjectURL(blob);

                const link = document.createElement('a');

                link.href = url;

                let nombreArchivo: string;

                if (data.tipoCredito === CONSULTA_TOTAL) {
                    nombreArchivo =
                        'Reporte Extracto Anual Consulta Total.zip';
                } else if (data.tipoCredito === LEASING) {
                    nombreArchivo =
                        'Reporte Extracto Anual Leasing.zip';
                } else {
                    nombreArchivo =
                        'Reporte Extracto Anual Hipotecario.zip';
                }

                link.download = nombreArchivo;

                document.body.appendChild(link);

                link.click();

                link.remove();

                window.URL.revokeObjectURL(url);

                setLoading(false);

                let descripcionTipoCredito = 'No especificado';

                if (tipoCredito === 0) {
                    descripcionTipoCredito =
                        'CONSULTA TOTAL PLAN PAGOS';
                } else if (tipoCredito === 1) {
                    descripcionTipoCredito =
                        'LEASING HABITACIONAL';
                } else if (tipoCredito === 2) {
                    descripcionTipoCredito = 'HIPOTECARIO';
                }

                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo:
                        nav.userAgentData?.platform ||
                        nav.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo:
                        'Productos clientes / Extractos Financieros / Extracto Anual',
                    hostname: document.location.host,
                    tipoAccion: 'DESCARGAS',
                    idTipoAccion: 5,
                    descripcion: `Descarga de extracto anual con los siguientes filtros:
- Tipo Crédito: ${descripcionTipoCredito || 'No especificada'}
- Unidad Ejecutora: ${nombreUnidadEjecutora || 'No especificada'}
- Periodo: ${periodo || 'No especificada'}`,
                } as Auditoria;

                auditoriaApi.InsertAuditoria(
                    requestAuditoria,
                );
            } else {
                toast(
                    'No fue posible generar el reporte: la respuesta llegó vacía.',
                    {
                        type: toast.TYPE.ERROR,
                    },
                );

                setLoading(false);
            }
        } catch (error: any) {
            const esCaidaConexion =
                error?.code === 'ERR_NETWORK' ||
                error?.message === 'Network Error' ||
                !error?.response;

            const mensajeApi =
                await extraerMensajeErrorBlob(error);

            const descripcionError = esCaidaConexion
                ? 'El servidor no respondió (posible caída del backend durante la generación).'
                : `Error al consultar Extracto Anual.
- Error: ${error?.message || 'Error desconocido'}
- Mensaje API: ${mensajeApi}
- Código HTTP: ${
                      error?.response?.status ||
                      'No disponible'
                  }`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo:
                    nav.userAgentData?.platform ||
                    nav.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo:
                    'Productos clientes / Extractos Financieros / Extracto Anual',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };

            auditoriaApi.InsertAuditoria(
                requestAuditoriaError,
            );

            NotificationService.genericAlertError(
                esCaidaConexion
                    ? 'El servicio no respondió. Intenta nuevamente en unos minutos o contacta a soporte si persiste.'
                    : `Error al generar el reporte: ${mensajeApi}`,
                esCaidaConexion
                    ? 'Sin respuesta del servidor'
                    : 'Error al generar el reporte',
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <LoadingV2Component
                key="loader"
                loading={loading}
                tittle="Generando Reporte... Este proceso puede tardar varios minutos, por favor no cierres ni recargues esta pestaña."
            />

            <CHPrivateContainer>
                <CHPaper title="Extracto Anual">
                    <Grid container>
                        <Grid
                            item
                            xs={12}
                            md={3}
                            className={classes.column}
                        >
                            <ControllerTipoCredito
                                form={formulario}
                            />
                        </Grid>

                        {/*
                        <Grid
                            item
                            xs={12}
                            md={3}
                            className={classes.column}
                        >
                            <ControllerUnidadEjecutora
                                form={formulario}
                                unidadesEjecutoras={
                                    unidadesEjecutoras
                                }
                            />
                        </Grid>
                        */}

                        <Grid
                            item
                            xs={12}
                            md={3}
                            className={classes.column}
                        >
                            <ControllerPeriodo
                                form={formulario}
                                required
                                type="year"
                            />
                        </Grid>
                    </Grid>

                    <Grid container>
                        <Grid
                            className={classes.column}
                            item
                            xs={12}
                            md={3}
                        >
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(
                                    onSubmit,
                                )}
                                disabled={loading}
                            >
                                GENERAR
                            </Button>
                        </Grid>
                    </Grid>
                </CHPaper>
            </CHPrivateContainer>
        </>
    );
};

export default ExtractoAnual;