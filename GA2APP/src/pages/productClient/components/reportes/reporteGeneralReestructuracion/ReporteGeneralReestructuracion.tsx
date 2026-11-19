import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import ApiReportes from '../../../../../services/api-reportes.service';
import ControllerNumeroCredito from '../controllers/ControllerNumeroCredito';
import ControllerPeriodoActual from '../controllers/ControllerPeriodoActual';
import ControllerFuerza from '../controllers/ControllerFuerza';
import CatalogApi from '../../../../../services/api-catalog.service';
import Fuerza from '../types/Fuerza';
// import Categoria from '../types/Categoria';
// import ControllerCategoria from '../controllers/ControllerCategoria';
import CHPageTitle from '../../../../../components/ch-page-title/CHPageTitle';
import TablaReporteGeneralReestructuracion from './TablaReporteGeneralReestructuracion';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';



type FormData = {
    tipoCredito: number,
    periodoActual: boolean,
    numeroCredito?: number,
    periodoReporte?: Date | null,
    fuerza?: number | 'all',
    // categoria?: number | 'all'
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

// const obtenerCategorias = (callBack: (unidades: Categoria[]) => void) => {
//     const apiCatalog = CatalogApi();
//     apiCatalog.getCategories()
//         .then((response: any) => response?.isSuccess && callBack(response.data));
// }

const obtenerFuerzas = (callBack: (unidades: Fuerza[]) => void) => {
    const apiCatalog = CatalogApi();
    apiCatalog.getAllForces()
        .then((response: any) => response?.isSuccess && callBack(response.data));
}

const ReporteGeneralReestructuracion = () => {
    const apiReportes = ApiReportes();
    const classes = useStyles();

    const [data, setData] = useState<any[] | null>(null);
    // const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [fuerzas, setFuerzas] = useState<Fuerza[]>([]);
    const [loading, setLoading] = useState(false);
    const [periodo, setPeriodo] = useState<Date | null>(null);

    const formulario = useForm<FormData>({
        mode: 'onChange',
        defaultValues: {
            periodoActual: false,
            periodoReporte: new Date(),
            tipoCredito: 0,
            fuerza: 'all',
            // categoria: 'all'
        },
    });

    const valorPeriodoActual = formulario.watch('periodoActual');

    useEffect(() => {
        // obtenerCategorias(data => setCategorias(data));
        obtenerFuerzas(data => setFuerzas(data));
    }, []);


    useEffect(() => {
        if (valorPeriodoActual) {
            formulario.setValue("periodoReporte", null, { shouldValidate: true });
        }
    }, [valorPeriodoActual]);

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

    const onSubmit = async (data: any) => {

        setData(null);
        setLoading(true);
        setPeriodo(data.periodoReporte);

        const request = {
            periodoActual: data.periodoActual ?? true,
            tipoCredito: data.tipoCredito ? data.tipoCredito : null,
            anio: data.periodoReporte?.getFullYear() ?? null,
            mes: data.periodoReporte ? data.periodoReporte.getMonth() + 1 : null,
            numeroCredito: data.numeroCredito ? data.numeroCredito : null,
            fuerza: (!data.fuerza || data.fuerza === 'all') ? null : data.fuerza,
            // categoria: (!data.categoria || data.categoria === 'all') ? null : data.categoria
        }

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
            modulo: 'Reporte general de reestructuración',
            hostname: document.location.host
        } as Auditoria
        let tipoCredito = request.tipoCredito === 1 ? 'LEASING HABITACIONAL' : '';
        tipoCredito = request.tipoCredito === 2 ? 'HIPOTECARIO' : 'CONSULTA TOTAL REESTRUCTURACIÓN';
        apiReportes
            .reporteGeneralReestructuracion(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        setData(response.data ?? []);
                        requestAuditoria.tipoAccion = 'TRANSACCIONES';
                        requestAuditoria.idTipoAccion = 2;
                        requestAuditoria.descripcion = `Generación reporte general de reestructuración parámetros:     
                         \n Periodo actual:  ${request.periodoActual ? 'Sí' : 'No'}                    
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte === null || data.periodoReporte === undefined ? '' : moment(data.periodoReporte).format("YYYY/MM")}
                         \n Número crédito:  ${request.numeroCredito === null ? '' : request.numeroCredito} 
                         \n Fuerza:  ${request.fuerza === null ? 'TODAS' : fuerzas.find(x => x.id === request.fuerza)?.descripcion}
                          `
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        setData([]);
                        requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion = 8;
                        requestAuditoria.descripcion = `Generación reporte general de reestructuracion parámetros:     
                         \n Periodo actual:  ${request.periodoActual ? 'Sí' : 'No'}                    
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte === null || data.periodoReporte === undefined ? '' : moment(data.periodoReporte).format("YYYY/MM")}
                         \n Número crédito:  ${request.numeroCredito === null ? '' : request.numeroCredito} 
                         \n Fuerza:  ${request.fuerza === null ? 'TODAS' : fuerzas.find(x => x.id === request.fuerza)?.descripcion}
                        \n returnApiMessage: ${response.returnMessage}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                }
            })
            .catch((error: any) => {
                toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 8;
                requestAuditoria.descripcion = `Generación reporte general de reestructuracion parámetros:     
                         \n Periodo actual:  ${request.periodoActual ? 'Sí' : 'No'}                    
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte === null || data.periodoReporte === undefined ? '' : moment(data.periodoReporte).format("YYYY/MM")}
                         \n Número crédito:  ${request.numeroCredito === null ? '' : request.numeroCredito} 
                         \n Fuerza:  ${request.fuerza === null ? 'TODAS' : fuerzas.find(x => x.id === request.fuerza)?.descripcion}
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
                    title1="Reporte "
                    title2="General de Reestructuracion"
                />

                <CardContent>
                    <Grid container>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerTipoCredito form={formulario} />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerNumeroCredito form={formulario} />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerPeriodo
                                form={formulario} type="month"
                                disabled={valorPeriodoActual} required={!valorPeriodoActual}
                            />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerPeriodoActual form={formulario} />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerFuerza form={formulario} fuerzas={fuerzas} />
                        </Grid>
                        {/* <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerCategoria form={formulario} categorias={categorias} />
                        </Grid> */}
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
            {data && <TablaReporteGeneralReestructuracion data={data} periodo={periodo} />}
        </>
    );
};

export default ReporteGeneralReestructuracion;