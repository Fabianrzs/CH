import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import ContainerTitle from '../../../components/containe-title/container-title';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CarteraApi from '../../../services/api-cartera-service';
import AuthService from '../../../services/auth.service';
import TituloIcono from '../../AdministracionCuentas/components/titulo-icono/tituloIcono.component';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import CatalogApi from '../../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        root: {
            flexGrow: 1,
        },
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917'
        },
    }),
);
const AplicacionPagoMasivo = () => {
    const classes = useStyles();
    const carteraApi = CarteraApi();
    const formularioConsulta = useForm({
        defaultValues: {
            fechaConsulta: '',

        }
    })
    const [isLoading, setIsLoading] = useState(false);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
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

        const consultarMovimientosDiariosSeven = (data: any) => {
            const fechaMoment = moment(data.fechaConsulta);
            const fechaFormateada = fechaMoment.format("YYYY-MM-DD HH:mm:ss");
            setIsLoading(true);
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
                modulo: 'Aplicación pago masivo',
                hostname: document.location.host
            } as Auditoria
            carteraApi.AplicarPagoMasivoTotalizado(fechaFormateada)
                .then((response: any) => {
                    if (response.isSuccess) {
                        if (response.returnMessage && response.returnMessage.trim() !== '') {
                            if (response.isSuccess && response.returnMessage !== null && response.returnMessage !== undefined) {
                                const leasingRegex = /Resultado aplicación de pagos leasing : <NewDataSet>.*?<RETORNO>(.*?)<\/RETORNO>.*?<TXTERROR>(.*?)<\/TXTERROR>/s;
                                const leasingMatch = response.returnMessage.match(leasingRegex);
                                const retornoLeasing = leasingMatch ? leasingMatch[1] : null;
                                const errorLeasing = leasingMatch ? leasingMatch[2] : null;
                                if (retornoLeasing === '0') {
                                    toast('Inserción Leasing a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
                                    requestAuditoria.tipoAccion = 'TRANSACCIONES';
                                    requestAuditoria.idTipoAccion = 2;
                                    requestAuditoria.descripcion = `Aplicar pago masivo leasing a seven parámetros:
                            \n fecha: ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}`
                                    auditoriaApi.InsertAuditoria(requestAuditoria);
                                } else if (retornoLeasing === null) {
                                    // toast(`Causación Hipotecario : ${errorHipotecario}`, { type: toast.TYPE.ERROR, autoClose: 10000 })
                                } else {
                                    toast(`Inserción Leasing FALLO: ${errorLeasing}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
                                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                                    requestAuditoria.idTipoAccion = 8;
                                    requestAuditoria.descripcion = `Aplicar pago masivo leasing a seven parámetros:
                            \n fecha: ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}
                            \n returnApiMessage: ${errorLeasing}`
                                    auditoriaApi.InsertAuditoria(requestAuditoria);
                                }
                                const hipotecarioRegex = /Resultado aplicación de pagos Hipotecario : <NewDataSet>.*?<RETORNO>(.*?)<\/RETORNO>.*?<TXTERROR>(.*?)<\/TXTERROR>/s;
                                const hipotecarioMatch = response.returnMessage.match(hipotecarioRegex);
                                const retornoHipotecario = hipotecarioMatch ? hipotecarioMatch[1] : null;
                                const errorHipotecario = hipotecarioMatch ? hipotecarioMatch[2] : null;
                                if (retornoHipotecario === '0') {
                                    toast('Causación Hipotecario a Seven EXITOSA', { type: toast.TYPE.SUCCESS });
                                    requestAuditoria.tipoAccion = 'TRANSACCIONES';
                                    requestAuditoria.idTipoAccion = 2;
                                    requestAuditoria.descripcion = `Aplicar pago masivo, causasión hipotecario a seven parámetros:
                            \n fecha: ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}`
                                    auditoriaApi.InsertAuditoria(requestAuditoria);
                                } else if (retornoHipotecario === null) {
                                    // toast(`Causación Hipotecario : ${errorHipotecario}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
                                } else {
                                    toast(`Causación Hipotecario FALLO: ${errorHipotecario}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
                                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                                    requestAuditoria.idTipoAccion = 8;
                                    requestAuditoria.descripcion = `Aplicar pago masivo hipotecario a seven parámetros:
                            \n fecha: ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}
                            \n returnApiMessage: ${errorHipotecario}`
                                    auditoriaApi.InsertAuditoria(requestAuditoria);
                                }

                            } else {
                                toast('Simulacion de movimiento EXITOSA', { type: toast.TYPE.SUCCESS });
                                requestAuditoria.tipoAccion = 'TRANSACCIONES';
                                requestAuditoria.idTipoAccion = 2;
                                requestAuditoria.descripcion = `Simulación aplicar pago masivo parámetros:
                        \n fecha: ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}`
                                auditoriaApi.InsertAuditoria(requestAuditoria);
                            }

                        } else {
                            toast("No se encontraron resultados para esta fecha", { type: toast.TYPE.ERROR, autoClose: 10000 });
                            requestAuditoria.tipoAccion = 'VALIDACION';
                            requestAuditoria.idTipoAccion = 9;
                            requestAuditoria.descripcion = `Aplicar pago masivo parámetros:
                         \n fecha:  ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}
                         \n Validación: No se encontraron resultados para esta fecha`
                            auditoriaApi.InsertAuditoria(requestAuditoria);
                        }
                    }
                })
                .catch((error: any) => {
                    if (error.response && error.response.data) {
                        toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
                    } else {
                        toast("Error del sistema", { type: toast.TYPE.ERROR });
                    }
                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion = 5;
                    requestAuditoria.descripcion = `Aplicar pago masivo parámetros:
            \n fecha: ${moment(fechaFormateada).format("DD/MM/YYYY HH:mm")}
            \n Error:  ${error}
            \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                })
                .finally(() => {
                    setIsLoading(false);
                });
        }

        return (
            <div className={classes.root}>

                <PageIndicators
                    topTextNivel1='Movimientos Contables'
                    topTextNivel2='Gestion de Contabilidad'
                />
                <IconTitle
                    title='Inserción'
                    subTitle='Movimientos de Contables'
                    icon='fch-Buscar-0-1'
                />
                <LoadingV2Component key="loader" loading={isLoading} tittle='Insertando Movimientos...' />

                <Grid >
                    <Grid item xs={12}>
                        <Card>
                            <ContainerTitle title="Ejecutar movimientos de pagos por listas y ventanilla SEVEN" />

                            <CardContent>
                                <Grid container justifyContent="space-around">
                                    <Grid item xs={3}>
                                        <TituloIcono
                                            title1="Fecha"
                                            subtitle="Aplicación"
                                            icono="event_available"
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Controller
                                            name="fechaConsulta"
                                            rules={{
                                                required: 'Este campo es requerido.',
                                            }}
                                            control={formularioConsulta.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHDatePicker
                                                    label="Fecha de Aplicación*"
                                                    InputLabelProps={{ shrink: true }}
                                                    inputProps={{
                                                        readOnly: true
                                                    }}
                                                    onChange={onChange}
                                                    value={value}
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    autoOk
                                                />
                                            )}
                                        />
                                    </Grid>
                                </Grid>

                                <br />
                                <div className="buttonssolicitud">
                                    <Button
                                        className={classes.buttonprincipal}
                                        variant="outlined"
                                        onClick={formularioConsulta.handleSubmit(
                                            consultarMovimientosDiariosSeven,
                                        )}
                                    >
                                        APLICAR
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    </Grid>
                </Grid>
            </div>
        );
    }
export default AplicacionPagoMasivo;

