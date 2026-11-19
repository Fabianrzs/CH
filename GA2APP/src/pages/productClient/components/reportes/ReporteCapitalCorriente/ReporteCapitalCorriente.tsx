import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import ControllerTipoCredito from '../controllers/ControllerTipoCredito';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import ApiReportes from '../../../../../services/api-reportes.service';
import CHPageTitle from '../../../../../components/ch-page-title/CHPageTitle';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import TablaReporteCapitalCorriente, { ObtenerColumnasCapitalCorriente } from './TablaReporteCapitalCorriente';
import { CustomTabItems, CustomTabs } from '../componentes/CustomTabs';
import { DatosReporteCapitalCorriente } from '../types/DatosReporteCapitalCorriente';
import { ReporteOpcionCompra } from '../types/ReporteOpcionCompra.type';
import TablaReporteOpcionCompra, { ObtenerColumnasOpcionCompra } from './TablaReporteOpcionCompra';
import exportToExcel from '../../../../Reports/helpers/exportToExcel';
import DownloadButton from '../componentes/DownloadButton';
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import CatalogApi from '../../../../../services/api-catalog.service';

const TAB_INDEX = {
    CAPITAL_CORRIENTE: 0,
    OPCION_COMPRA: 1,
};


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

const ViewTabs = ({currentTab, tipoCredito, onTabChange}: {
    currentTab: number;
    tipoCredito: number;
    onTabChange: (newValue: number) => void;
}) => (
    <CustomTabs
        value={currentTab}
        onChange={(_, value) => onTabChange(value)}
        aria-label="ant example"
    >
        <CustomTabItems label="Capital corriente"  value={TAB_INDEX.CAPITAL_CORRIENTE}/>
        {tipoCredito !== 2 && <CustomTabItems label="Opción de compra" value={TAB_INDEX.OPCION_COMPRA}/>}
    </CustomTabs>
);

const ReporteCapitalCorriente = () => {
    const apiReportes = ApiReportes();
    const classes = useStyles();
    const defaultValues = {
        periodoReporte: new Date(),
        tipoCredito: 0,
    };

    const [dataCapitalCorriente, setDataCapitalCorriente] = useState<DatosReporteCapitalCorriente[]|null>(null);
    const [dataOpcionCompra, setDataOpcionCompra] = useState<ReporteOpcionCompra[]|null>(null);
    const [loading, setLoading] = useState(false);
    const [request, setRequest] = useState<FormData>(defaultValues);
    const [currentTabIndex, setCurrentTabIndex] = useState(0);

    const formulario = useForm<FormData>({
        mode: 'onChange',
        defaultValues: defaultValues,
    });

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
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

    const handleDownload = async () => {

        if(request.periodoReporte) {
            const format = moment(request.periodoReporte).format('YYYY-MM');
            const fileName = `Reporte capital corriente - Periodo ${format}.xlsx`;
    
            const columnsCapitalCorriente = ObtenerColumnasCapitalCorriente(request.periodoReporte);
            const sheets: any[] = [
                {
                    data: dataCapitalCorriente ?? [],
                    columns: columnsCapitalCorriente,
                    sheetName: "Capital Corriente"
                },
            ];

            if(request.tipoCredito !== 2) {
                const columnsOpcionCompra = ObtenerColumnasOpcionCompra();
                sheets.push({
                    data: dataOpcionCompra ?? [],
                    columns: columnsOpcionCompra,
                    sheetName: "Opción de compra"
                });
            }

            await exportToExcel(fileName, sheets, publicIpAddress);
        }
    }

    

    const onSubmit = async (data: FormData) => {

        setCurrentTabIndex(0);
        setDataCapitalCorriente(null);
        setDataOpcionCompra(null);
        setLoading(true);
        setRequest(data);

        const request = {
            tipoCredito: data.tipoCredito ? data.tipoCredito : null,
            anio: data.periodoReporte?.getFullYear() ?? null,
            mes: data.periodoReporte ? data.periodoReporte.getMonth() + 1 : null
        };

        const results: Promise<any>[] = [];
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
            modulo:'Reporte capital corriente',
            hostname:document.location.host
        } as Auditoria
        let tipoCredito=request.tipoCredito===1?'LEASING HABITACIONAL':'';       
        tipoCredito=request.tipoCredito===2?'HIPOTECARIO':'CONSULTA TOTAL CARTERA';
        const resultCapitalCorriente = apiReportes
            .reporteCapitalCorriente(request)
            .then(response => {
                if (response && response.isSuccess) {
                    const mapData = (response.data ?? []).map(item => ({
                        ...item,
                        ...item.periodos.map(x => ({[x.mes]: x.capital}))
                            .reduce((obja, objb) => ({...obja, ...objb}), {})
                    }));
                    setDataCapitalCorriente(mapData);
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Generación reporte capital corriente parámetros:                         
                     \n Tipo crédito:  ${tipoCredito}
                     \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                    `
                auditoriaApi.InsertAuditoria(requestAuditoria);
                } else {
                    setDataCapitalCorriente([]);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion=8;
                        requestAuditoria.descripcion=`Generación reporte capital corriente parámetros:                         
                        \n Tipo crédito:  ${tipoCredito}
                        \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                        \n returnApiMessage: ${response.returnMessage}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            }).catch((error:any)=>{
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Generación reporte capital corriente parámetros:                         
                        \n Tipo crédito:  ${tipoCredito}
                        \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            });

        results.push(resultCapitalCorriente);
        
        if(request.tipoCredito !== 2) {
            const resultOpcionCompra = apiReportes
                .reporteOpcionCompra(request)
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDataOpcionCompra(response.data ?? []);
                            requestAuditoria.tipoAccion='TRANSACCIONES';
                            requestAuditoria.idTipoAccion=2;
                            requestAuditoria.descripcion=`Generación reporte capital corriente opción a compra parámetros:                         
                                \n Tipo crédito:  ${tipoCredito}
                                \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                                `
                            auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            setDataOpcionCompra([]);
                            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                            requestAuditoria.idTipoAccion=8;
                            requestAuditoria.descripcion=`Generación reporte capital corriente opción a compra parámetros:                         
                            \n Tipo crédito:  ${tipoCredito}
                            \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                            \n returnApiMessage: ${response.returnMessage}`
                            auditoriaApi.InsertAuditoria(requestAuditoria);
                        }
                    }
                }).catch((error:any)=>{
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Generación reporte capital corriente parámetros:                         
                        \n Tipo crédito:  ${tipoCredito}
                        \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });

            results.push(resultOpcionCompra);
        }

        Promise.all(results).catch(() => {
            toast('Error al generar el reporte', { type: toast.TYPE.ERROR });
        }).finally(() => setLoading(false));
       
    };

    return (
        <>  
            <Card>
                <CHPageTitle
                    topText="Consulta Reportes"
                    subText=""
                    title1="Reporte "
                    title2="Capital Corriente"
                />

                <CardContent>
                    <Grid container>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerTipoCredito form={formulario} />
                        </Grid>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerPeriodo 
                                form={formulario} type="month" 
                                required
                            />
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
            
            <LoadingV2Component key="loader" loading={loading} tittle='Generando reporte...' />

            {
                (dataCapitalCorriente || dataOpcionCompra) && 
                <Grid container justifyContent='space-between' alignItems='center'
                    style={{'marginTop': '2rem', 'background': 'white', 'padding': '1rem'}}>
                    <Grid item>
                        <ViewTabs currentTab={currentTabIndex} onTabChange={setCurrentTabIndex} tipoCredito={request.tipoCredito} />
                    </Grid>
                    <Grid item>
                        <DownloadButton  downloadAction={handleDownload} />
                    </Grid>
                </Grid>
            }

            { currentTabIndex === TAB_INDEX.CAPITAL_CORRIENTE && dataCapitalCorriente && request.periodoReporte && 
                <TablaReporteCapitalCorriente data={dataCapitalCorriente} periodo={request.periodoReporte} />
            }

            { currentTabIndex === TAB_INDEX.OPCION_COMPRA && dataOpcionCompra && request.periodoReporte && 
                <TablaReporteOpcionCompra data={dataOpcionCompra} periodo={request.periodoReporte} />
            }
            
        </>
    );
};

export default ReporteCapitalCorriente;