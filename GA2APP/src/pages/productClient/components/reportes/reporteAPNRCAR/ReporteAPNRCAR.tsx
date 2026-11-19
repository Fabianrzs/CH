import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import ApiReportes from '../../../../../services/api-reportes.service';
import Fuerza from '../types/Fuerza';
import Categoria from '../types/Categoria';
import CHPageTitle from '../../../../../components/ch-page-title/CHPageTitle';
import TablaReporteAPNRCAR from './TablaReporteAPNRCAR';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import CatalogApi from '../../../../../services/api-catalog.service';


type FormData = {
    tipoCredito: number,
    periodoReporte?: Date | null,
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


const ReporteAPNRCAR = () => {
    const apiReportes = ApiReportes();
    const classes = useStyles();

    const [data, setData] = useState<any[]|null>(null);
    const [loading, setLoading] = useState(false);
    const [periodo, setPeriodo] = useState<Date|null>(null);

    const formulario = useForm<FormData>({
        mode: 'onChange',
        defaultValues: {
            periodoReporte: new Date(),
            tipoCredito: 0,
        },
    });

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
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

        setData(null);
        setLoading(true);
        setPeriodo(data.periodoReporte);

        const request = {
            tipoCredito: data.tipoCredito ? data.tipoCredito : null,
            anio: data.periodoReporte?.getFullYear() ?? null,
            mes: data.periodoReporte ? data.periodoReporte.getMonth() + 1 : null,
        }
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
    
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Reporte APNRCAR',
            hostname:document.location.host
        } as Auditoria
        let tipoCredito=request.tipoCredito===1?'LEASING HABITACIONAL':'';       
        tipoCredito=request.tipoCredito===2?'HIPOTECARIO':'CONSULTA TOTAL CARTERA';
        apiReportes
            .reporteAPNRCAR(request)
            .then(async response => {
                if (response && response.isSuccess) {
                    setData(response.data ?? []);

                    if((response.data?.length ?? 0) === 0) {
                        toast('No se encontraron resultados', { 
                            type: toast.TYPE.WARNING , style: {'color': 'black', 'fontWeight': 'bold'} 
                        });
                    }
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Generación reporte APNRCAR parámetros:                         
                     \n Tipo crédito:  ${tipoCredito}
                     \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                    `
                auditoriaApi.InsertAuditoria(requestAuditoria);
                } else {
                    setData([]);
                    toast(response?.returnMessage ?? 'No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion=8;
                        requestAuditoria.descripcion=`Generación reporte APNRCAR parámetros:                         
                        \n Tipo crédito:  ${tipoCredito}
                        \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                        \n returnApiMessage: ${response.returnMessage}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            })
            .catch((error:any) => {
                toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Generación reporte APNRCAR parámetros:                         
                        \n Tipo crédito:  ${tipoCredito}
                        \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }).finally(() => setLoading(false));
       
    };

    return (
        <>
           
            <Card>
                <CHPageTitle
                    topText="Consulta Reportes"
                    subText=""
                    title1="Reporte"
                    title2="APNRCAR"
                />
                
                <CardContent>
                    <Grid container justifyContent='center'>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerTipoCredito form={formulario} />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerPeriodo 
                                form={formulario} type="month" required
                            />
                        </Grid>
                    </Grid>
                    <Grid container justifyContent='center'>
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
            {data && <TablaReporteAPNRCAR data={data} periodo={periodo}  />}
        </>
    );
};

export default ReporteAPNRCAR;