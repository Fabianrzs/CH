import { Button, Card, CardContent, CircularProgress, Grid, makeStyles, MenuItem, createStyles, Checkbox, ListItemText } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CHAutoCompleteV3 from "../../../components/ch-auto-complete/CHAutoCompleteV3";
import CHPaper from "../../../components/ch-paper/CHPaper";
import CHPrivateContainer from "../../../components/ch-private-container/CHPrivateContainer";
import CHSelect from "../../../components/ch-select/CHSelect";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import { LoadingComponent } from "../../../components/loading/loading.component";
import CatalogApi from "../../../services/api-catalog.service";
import CreditApi from "../../../services/api-credit.service";
import AuthService from "../../../services/auth.service";
import { Auditoria } from "../../../types/Auditoria.model.type";
import { EXECUTINGUNITDTO } from "../../../types/catalog.type";
import AuditoriaApi from '../../../services/api-auditorias.servise';


const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        submitReportSectionLoader: {
            display: 'block',
            margin: 'auto'
        },
        submitReportSectionText: {
            display: 'block',
            textAlign: 'center',
            marginTop: '1rem',
            fontWeight: 'bold'
        }
    }),
);

const generatePeriods = () => {
    const year = (new Date()).getFullYear();
    const periods = Array.from({length: 12}).map((_, i) => `${year}${(i + 1).toString().padStart(2, '0')}`);
    return periods;
}

const NovedadesCredito = () => {

    const creditApi = CreditApi();
    const catalogoApi = CatalogApi();
    const [periods, setPeriods] = useState<string[]>([]);
    const [reportProcess, setReportProcess] = useState(false);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const classes = useStyles();

    const tiposReporte: { value: any, description: string }[] = [
        {
            value: 1,
            description: "ALTAS"
        },
        {
            value: 0,
            description: "BAJAS"
        },
    ]

    const formReport = useForm({
        mode: 'onChange',
        defaultValues: {
            idUnidadEjecutora: '',
            tipoReporte: Array<number>(),
            periodoGeneracion: ''
        },
    });

    useEffect(() => {
        catalogoApi
            .getAllEXECUTINGUNITSRequest()
            .then((response: any) => {
                if(response.isSuccess && response.data) {
                    setUnidadesEjecutoras([
                        { id: 0, nombre: "TODAS" },
                        ...response.data
                    ]);
                }
            });

        setPeriods(generatePeriods());
    }, [])


    const downloadFile = (url: string, filename: string) => {
        const link = document.createElement('a');
        link.href = url;
        link.download =  filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    }

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

    const onSubmitReport = async (request: any) => {
        const auditoriaApi= AuditoriaApi();     
        const user = AuthService.getUserInfo();                          
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
        const requestAuditoria={
                    navegador:usrAg,
                    sistemaOperativo:nav.userAgentData.platform,
                    nombreUsuario:user.USR_USERNAME, 
                    usurioId:user.USR_ID,
                    ip: publicIpAddress,
                    modulo:'Novedades de crédito',
                     hostname:document.location.host
                } as Auditoria;

        const payload = {
                ...request,
                tipoReporte: request.tipoReporte.length > 1 ? 2 : request.tipoReporte[0]
        };
        const unidadEjecutora = payload.idUnidadEjecutora === 0
        ? "TODAS_LAS_UNIDADES"
        : unidadesEjecutoras.find(x => x.id === payload.idUnidadEjecutora)?.nombre?.replace(/\s+/g, '_');
        const reportes = ["BAJAS", "ALTAS", "ALTAS_Y_BAJAS"];
        const tipoReporte = reportes[payload.tipoReporte];
         // Todas las unidades o más de un tipo de reporte generará un zip
         const filename = (payload.idUnidadEjecutora === 0 || payload.tipoReporte === 2)
         ? `REPORTES_${tipoReporte}_${payload.periodoGeneracion}_${unidadEjecutora}.zip`
         : `REPORTE_${tipoReporte}_${payload.periodoGeneracion}_${unidadEjecutora}.csv`;
         
        try {
            setReportProcess(true);           
            const response = await creditApi.getDescargarReporteAltasBajas(payload);
            const url = window.URL.createObjectURL(response);
            downloadFile(url, response.filename || filename);    

            requestAuditoria.tipoAccion='DESCARGAS';
            requestAuditoria.idTipoAccion=5;
            requestAuditoria.descripcion=`Descarga de novedades de crédito en formato excel:
                        \n unidad ejecutora: ${unidadEjecutora}
                        \n tipo reporte: ${tipoReporte}
                        \n periodo: ${payload.periodoGeneracion}
                        \n archivo: ${filename}`;
            await auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error:any) {
            toast("Error al generar el reporte", { type: toast.TYPE.ERROR });           
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=8;
            requestAuditoria.descripcion=`Error Descarga de novedades de crédito en formato excel:
                        \n unidad ejecutora: ${unidadEjecutora}
                        \n tipo reporte: ${tipoReporte}
                        \n periodo: ${request.periodoGeneracion}
                        \n archivo: ${filename}                
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
            await auditoriaApi.InsertAuditoria(requestAuditoria);
        }
        finally {
            setReportProcess(false);
        }
    };

    return <>

        <CHPrivateContainer>
            <CHPaper title="Novedades de Crédito">
                <Grid container>
                        <Grid item xs={12} >
                            <Card>
                                <CardContent>
                                    <Grid container justifyContent="space-around">
                                        <Grid item xs={3}>
                                            <Controller name="idUnidadEjecutora" control={formReport.control}
                                                rules={{
                                                    required: {
                                                        value: true,
                                                        message: 'Este campo es requerido.',
                                                    },
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHSelect defaultValue="" label="Unidad Ejecutora"
                                                        value={value} onChange={onChange} error={!!error}
                                                        helperText={error ? error.message : null}>
                                                        {unidadesEjecutoras?.map((option: EXECUTINGUNITDTO) => (
                                                                <MenuItem key={option.id} value={option.id}>
                                                                    {option.nombre}
                                                                </MenuItem>
                                                            ),
                                                        )}
                                                    </CHSelect>
                                                )}
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <Controller name="tipoReporte" control={formReport.control}
                                                rules={{
                                                    required: {
                                                        value: true,
                                                        message: 'Este campo es requerido.',
                                                    },
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHSelect label="Tipo Reporte" multiple
                                                        renderValue={(selected: any) => selected.map((value: any) => tiposReporte.find(x => x.value === value)?.description).join(', ')}
                                                        value={value} onChange={onChange} error={!!error}
                                                        helperText={error ? error.message : null}>
                                                             {
                                                                tiposReporte.map((item) => (
                                                                    <MenuItem key={item.value} value={item.value}>
                                                                        <Checkbox checked={value.indexOf(item.value) > -1} color="primary" />
                                                                        <ListItemText primary={item.description} />
                                                                    </MenuItem>
                                                                ))
                                                            }
                                                    </CHSelect>
                                                )}
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <Controller name="periodoGeneracion" control={formReport.control}
                                                rules={{ 
                                                    required: { value: true, message: 'Este campo es requerido.'},
                                                    pattern: {
                                                        value: /^(?:19|2\d)\d{2}(0[1-9]|1[0-2])$/,
                                                        message: 'Período o formato (YYYYMM) no válido'
                                                    }
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHAutoCompleteV3
                                                            value={value}
                                                            onChange={(event, item) => onChange(item)}
                                                            id="usuario"
                                                            options={periods}
                                                            freeSolo
                                                            getOptionLabel={(option) => option}
                                                            helperText={error ? error.message : null}
                                                            error={!!error}
                                                            renderInput={(params) => <CHTextField numberOnlyMask {...params} onChange={(e: any) => {
                                                                if(e.target) {
                                                                    onChange(e.target.value);
                                                                }
                                                            }} InputLabelProps={{ shrink: true}} label="Periodo"/>}
                                                        />
                                                )}
                                            />
                                        </Grid>
                                    </Grid>
                                </CardContent>
                            </Card>
                        </Grid>
                </Grid>
                
                <Grid className="buttonssolicitud">
                    { reportProcess 
                        ?   <div>
                                <p className={classes.submitReportSectionText}>Por favor espere un momento, se están generando los reportes …</p>
                                <CircularProgress size={30} className={classes.submitReportSectionLoader}/>
                            </div>
                        : <Button
                            type='submit'
                            className={classes.buttonprincipal}
                            variant="outlined"
                            onClick={formReport.handleSubmit(onSubmitReport)}
                            disabled={reportProcess}
                            > Generar Reporte
                        </Button>
                    }
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    </>;
}

export default NovedadesCredito;