import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { toast } from 'react-toastify';
import CHPaper from '../../../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../../../components/ch-private-container/CHPrivateContainer';
import Nullable from '../types/Nullable';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import ControllerUnidadEjecutora from '../controllers/ControllerUnidadEjecutora';
import UnidadEjecutora from '../types/UnidadEjecutora';
import CatalogApi from '../../../../../services/api-catalog.service';
import ApiproductoClients from '../../../../../services/api-producto.service';
import NotificationService from '../../../../../services/notification.service';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
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
            overflowY: 'auto'
        }
    }),
);

const obtenerUnidadesEjecutoras = (): Promise<UnidadEjecutora[]> => {
    const apiCatalog = CatalogApi();
    return apiCatalog.getAllEXECUTINGUNITSRequest()
        .then((response: any) => (response?.isSuccess && response.data) || [])
        .catch(() => []);
}

const ExtractoMensual = (props: any) => {
    const apiProductos = ApiproductoClients();
    const classes = useStyles();
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<UnidadEjecutora[]>([]);
    const [loading, setLoading] = useState(false);
    const formulario = useForm({
        mode: 'onChange',
        defaultValues: {
            tipoCredito: 0,
            periodoReporte: null as Nullable<Date>,
            unidadEjecutora: 0 as Nullable<number>,
        },
    });

    useEffect(() => {
        obtenerUnidadesEjecutoras()
            .then(data => setUnidadesEjecutoras(data));
    }, []);

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

    const onSubmit = async (data: any) => {
        setLoading(true);
        const CONSULTA_TOTAL = 0;
        const LEASING = 1;

        const mes = data.periodoReporte.getMonth() + 1;
        const anio = data.periodoReporte.getFullYear();

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Extracto Mensual',
            hostname: document.location.host
        } as Auditoria

        let tipoCredito = data.tipoCredito === 1 ? 'LEASING HABITACIONAL' : '';
        tipoCredito = data.tipoCredito === 2 ? 'HIPOTECARIO' : 'CONSULTA TOTAL PLAN PAGOS';

        try {
            const periodo = `${anio}-${String(mes).padStart(2, '0')}`;
            const result = await apiProductos.generarReportePlanPagosExtractoMasivo({
                // anio, 
                // mes,
                // tipoCredito: data.tipoCredito ? data.tipoCredito : null,
                // unidadEjecutora: data.unidadEjecutora ? data.unidadEjecutora : null,
                // tipoExtracto: 1
                periodo,
                tipoCredito: data.tipoCredito ?? 0,
            })

            if (result) {

                const blob = new Blob([result], { type: 'application/zip' });
                const url = window.URL.createObjectURL(blob);

                const link = document.createElement('a');
                link.href = url;
                let nombreArchivo: string;

                if (data.tipoCredito === CONSULTA_TOTAL) {
                    nombreArchivo = 'Reporte Extracto Mensual Consulta Total.zip';
                } else if (data.tipoCredito === LEASING) {
                    nombreArchivo = 'Reporte Extracto Mensual Leasing.zip';
                } else {
                    nombreArchivo = 'Reporte Extracto Mensual Hipotecario.zip';
                }

                link.download = nombreArchivo;
                document.body.appendChild(link);
                link.click();
                window.URL.revokeObjectURL(url);
                setLoading(false);

                requestAuditoria.tipoAccion = 'TRANSACCIONES';
                requestAuditoria.idTipoAccion = 2;
                requestAuditoria.descripcion = `Generación de extracto mensual parámetros:                  
                 \n Tipo crédito:  ${tipoCredito}
                 \n Unidad Ejecutora:  ${data.unidadEjecutora === null ? 'TODAS' : unidadesEjecutoras.find(x => x.id === data.unidadEjecutora)?.sigla}
                 \n Periodo:  ${data.periodoReporte} `
                await auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        }
        catch (error: any) {
            requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion = 8;
            requestAuditoria.descripcion = `Error Generación de extracto mensual parámetros:                  
             \n Tipo crédito:  ${tipoCredito}
             \n Unidad Ejecutora:  ${data.unidadEjecutora === null ? 'TODAS' : unidadesEjecutoras.find(x => x.id === data.unidadEjecutora)?.sigla}
             \n Periodo:  ${data.periodoReporte}
             \n Error:  ${error}
             \n returnApiMessage: ${error?.response?.data?.returnMessage}`
            auditoriaApi.InsertAuditoria(requestAuditoria);
            NotificationService.genericAlertError(`Generación de extracto mensual`, "Error al generar el reporte");
            setLoading(false);
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <>
            <LoadingV2Component key="loader" loading={loading} tittle='Generando Reporte...' />
            <CHPrivateContainer>
                <CHPaper title="Extracto Mensual">
                    <Grid container>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerTipoCredito form={formulario} />
                        </Grid>
                        {/* <Grid item xs={12} md={3} className={classes.column}>
                        <ControllerUnidadEjecutora form={formulario} unidadesEjecutoras={unidadesEjecutoras} />
                    </Grid> */}
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerPeriodo form={formulario} required type="month" />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Button className={classes.button} variant="contained" disabled={loading} color="primary"
                                onClick={formulario.handleSubmit(onSubmit)}
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

export default ExtractoMensual;