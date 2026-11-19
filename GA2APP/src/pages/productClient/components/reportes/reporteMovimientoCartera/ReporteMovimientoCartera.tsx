import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import React, { useState, useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../../../../components/ch-text-field/CHTextField';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import ApiReportes from '../../../../../services/api-reportes.service';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import CatalogApi from '../../../../../services/api-catalog.service';
import DownloadBlob from '../../../../../utils/blob';

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
    }),
);

const ReporteMovimientoCartera = () => {
    const classes = useStyles();
    const catalogApi = CatalogApi();
    const apiReportes = ApiReportes();
    const [loading, setLoading] =  useState(false);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const formulario = useForm({
        defaultValues: {
            identificacion: '',
    }})

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };
    
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

    const generarReporteMovimientoCartera = async(data: any) => {
        setLoading(true);
        const user = AuthService.getUserInfo();
        const auditoriaApi= AuditoriaApi();                 
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
        const requestAuditoria={
                navegador:usrAg,
                sistemaOperativo:nav.userAgentData.platform,
                nombreUsuario:user.USR_USERNAME, 
                usurioId:user.USR_ID,
                ip: publicIpAddress,
                modulo:'Otros Reportes',
                hostname:document.location.host
        } as Auditoria;

        const request = {
            identificacion: data.identificacion,
        };

        await apiReportes
        .generarMovimientoCartera(request)
        .then(async response => {
            if(response){
                const blob = new Blob([response], { type: 'application/pdf' });
                const filename = `Movimiento Cartera - ${data.identificacion}.pdf`;
                DownloadBlob(blob, filename);
                setLoading(false);
                requestAuditoria.tipoAccion='DESCARGAS';
                requestAuditoria.idTipoAccion=5;
                requestAuditoria.descripcion=`Descarga de reporte
                 \n Identificación: ${data.identificacion}
                 \n archivo: Movimiento cartera`;
                await auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        })
        .catch((error:any) => {
            toast('Error: el número de identificación no existe', {
                type: toast.TYPE.ERROR,
            });
            setLoading(false);
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=8;
            requestAuditoria.descripcion=`Error Descarga de reporte
            \n Identificación: ${data.identificacion}
            \n archivo:  Movimiento cartera
            \n Error:  ${error}
            \n returnApiMessage: ${error?.response?.data?.returnMessage}`
            auditoriaApi.InsertAuditoria(requestAuditoria);
        });
    }

    return(
        <CHPrivateContainer>
             <LoadingV2Component key="loader" loading={loading} tittle='Generando Reporte...' />
             <CHPaper title="Movimiento Cartera">
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="identificacion"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        type="number"
                                        error={!!error}
                                        label="Número Identificación *"
                                        value={value}
                                        InputLabelProps={{ shrink: true }} 
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={(e) =>{
                                            if(e.target.value.length <= 12){
                                                onChange(e);
                                            }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(generarReporteMovimientoCartera)}
                            >
                                GENERAR
                            </Button>
                        </Grid>
                    </Grid>
             </CHPaper>
        </CHPrivateContainer>
    )
}

export default ReporteMovimientoCartera;