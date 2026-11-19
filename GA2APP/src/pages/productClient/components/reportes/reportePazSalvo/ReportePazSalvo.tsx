import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import ApiReportes from '../../../../../services/api-reportes.service';
import CHPageTitle from '../../../../../components/ch-page-title/CHPageTitle';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import DownloadBlob from '../../../../../utils/blob';
import ControllerNumeroCredito from '../controllers/ControllerNumeroCredito';
import NotificationService from '../../../../../services/notification.service';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import CatalogApi from '../../../../../services/api-catalog.service';


type FormData = {
    tipoCredito?: number,
    numeroCredito?: number,
}

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
        }
    }),
);

const ReportePazSalvo = () => {
    const apiReportes = ApiReportes();
    const classes = useStyles();

    const [loading, setLoading] = useState(false);

    const formulario = useForm<FormData>({ mode: 'onChange' });

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
    }, []);

    const onSubmit = async (data: any) => {
        setLoading(true);

        const request = {
            tipoCredito: data.tipoCredito,
            numeroCredito: data.numeroCredito
        };
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const user = AuthService.getUserInfo();
        const usrAg = nav.userAgentData?.brands?.filter((x: { brand: string; }) =>
            x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0]?.brand ?? "Desconocido";

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Reporte general de garantía',
            hostname: document.location.host
        } as Auditoria

        apiReportes
            .reportePazSalvo(request)
            .then(async response => {
                if (response?.type === "application/pdf") {
                    const blob = new Blob([response], { type: response.type });
                    const filename = `Reporte paz y salvo - ${request.numeroCredito}.pdf`;
                    DownloadBlob(blob, filename);

                    
                    requestAuditoria.tipoAccion = 'TRANSACCIONES';
                    requestAuditoria.idTipoAccion = 2;
                    requestAuditoria.descripcion = `Generación reporte paz y salvo\nNúmero crédito: ${request.numeroCredito}`;
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            })
            .catch(async (e) => {
                let message;

                if (e.response) {
                    const result = JSON.parse(await e.response.data.text());
                    message = result.returnMessage;
                } else {
                    message = 'No se pudo generar el reporte';
                }


                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 8;
                requestAuditoria.descripcion = `Error al generar paz y salvo\nNúmero crédito: ${request.numeroCredito}\nMensaje: ${message}`;
                auditoriaApi.InsertAuditoria(requestAuditoria);

                NotificationService.genericAlwarning('Reporte de paz y salvo', message);
            })
            .finally(() => setLoading(false));
    };


    return (
        <>
            <Card>
                <CHPageTitle
                    topText="Consulta Reportes"
                    subText=""
                    title1="Reporte"
                    title2="Paz y Salvo"
                />

                <CardContent>
                    <Grid container>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerTipoCredito form={formulario} required all={false} />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerNumeroCredito form={formulario} required />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(onSubmit)}
                            >
                                GENERAR
                            </Button>
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>

            <br />

            <LoadingV2Component key="loader" loading={loading} tittle='Generando reporte...' />
        </>
    );
};

export default ReportePazSalvo;