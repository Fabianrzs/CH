import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPrivateContainer from '../../../../../components/ch-private-container/CHPrivateContainer';
import Header from '../../../../../components/header/Header';
import IconTitle from '../../../../../components/page-title-icon/page-title-icon';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerNumeroIdentificacion from '../controllers/ControllerNumeroIdentificacion';
import ControllerNumeroCredito from '../controllers/ControllerNumeroCredito';
import TablaReporteCreditos from './TablaReporteCreditos';
import ApiReportes from '../../../../../services/api-reportes.service';
import ReporteEstadoCuentaAfiliado from '../types/ReporteEstadoCuentaAfiliado.type';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import AuthService from '../../../../../services/auth.service';
import CatalogApi from '../../../../../services/api-catalog.service';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';


interface FormData {
    numeroIdentificacion: string,
    numeroCredito: number,
    tipoCredito: number
}

const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        }
    }),
);


const ReporteEstadoCuenta = () => {

    const api = ApiReportes();
    const classes = useStyles();
    const [request, setRequest] = useState<FormData | null>(null);
    const [isProccessing, setProcessing] = useState(false);
    const [dataReporteGeneral, setDataReporteGeneral] = useState<ReporteEstadoCuentaAfiliado[] | null>(null);

    const formControls = useForm<FormData>({
        'mode': 'onChange',
        defaultValues: {
            tipoCredito: 0
        },
    });

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

    const onSubmit = async (request: any) => {

        setRequest(request);


        if (formControls.formState.isValid) {
            setProcessing(true);
            setDataReporteGeneral(null);

            api.reporteEstadoCuentaAfiliado({
                tipoCredito: request.tipoCredito ? request.tipoCredito : null,
                numeroCredito: request.numeroCredito ? request.numeroCredito : null,
                numeroIdentificacion: request.numeroIdentificacion ? request.numeroIdentificacion : null,
            })
                .then(x => {
                    if (x.isSuccess && x.data) {
                        setDataReporteGeneral(x.data);

                        const auditoriaApi = AuditoriaApi();
                        const user = AuthService.getUserInfo();
                        const nav = window.navigator as any;
                        const usrAg = nav.userAgentData.brands.find(
                            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                        )?.brand;

                        let descripcionTipoCredito = 'No especificado';

                        if (request.tipoCredito === 0) {
                            descripcionTipoCredito = 'CONSULTA TOTAL CARTERA'
                        } else if (request.tipoCredito === 1) {
                            descripcionTipoCredito = 'LEASING HABITACIONAL'
                        } else if (request.tipoCredito === 2) {
                            descripcionTipoCredito = 'HIPOTECARIO'
                        }

                        const requestAuditoria = {
                            navegador: usrAg,
                            sistemaOperativo: nav.userAgentData.platform,
                            nombreUsuario: user.USR_USERNAME,
                            usurioId: user.USR_ID,
                            ip: publicIpAddress,
                            modulo: 'Productos clientes / Reportes / Estados de cuenta',
                            hostname: document.location.host,
                            tipoAccion: 'CONSULTAS',
                            idTipoAccion: 3,
                            descripcion: `Consulta de estado de cuenta y movimientos con los siguientes filtros:
                                            - Tipo Crédito: ${descripcionTipoCredito || 'No especificada'}
                                            - Número obligación: ${request.numeroCredito || 'No especificada'}
                                            - Identificación: ${request.numeroIdentificacion || 'No especificada'}`,
                        } as Auditoria;

                        auditoriaApi.InsertAuditoria(requestAuditoria);

                    } else {
                        setDataReporteGeneral([]);
                        toast(x.returnMessage, { type: 'error' })
                    }
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
                title="Estado de cuenta y movimientos"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={12} md={4}>
                                    <ControllerTipoCredito form={formControls} />
                                </Grid>
                                <Grid item xs={12} md={3}>
                                    <ControllerNumeroCredito form={formControls} />
                                </Grid>
                                <Grid item xs={12} md={3}>
                                    <ControllerNumeroIdentificacion form={formControls} />
                                </Grid>
                            </Grid>
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
                    Consultar
                </Button>
            </Grid>

            <LoadingV2Component key="loader" loading={isProccessing} tittle='Generando reporte...' />

            <br />

            {
                dataReporteGeneral && request &&
                <Card>
                    <CardContent>
                        <TablaReporteCreditos tableData={dataReporteGeneral} />
                    </CardContent>
                </Card>
            }

        </CHPrivateContainer>
    </div>
    );
}

export default ReporteEstadoCuenta;