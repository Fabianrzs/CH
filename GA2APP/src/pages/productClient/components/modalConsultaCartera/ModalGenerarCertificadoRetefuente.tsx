import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    CircularProgress,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import DownloadBlob from '../../../../utils/blob';
import ApiReportes from '../../../../services/api-reportes.service';
import AuthService from '../../../../services/auth.service';
import AuditoriaApi from '../../../../services/api-auditorias.servise';
import { Auditoria } from '../../../../types/Auditoria.model.type';
import CatalogApi from '../../../../services/api-catalog.service';


const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        progress: {
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
        }
    }),
);

export const ModalGenerarCertificadoRetefuente = (props: any) => {
    const {
        open,
        handleClose,
        producto,
        tipoCredito
    } = props;

    const classes = useStyles();
    const [loading, setLoading] = useState(false);

    const currentDate = moment()
        .subtract(1, "year")
        .startOf("day")
        .toDate();

    const defaultValues: {
        tipoExtracto: 'mensual' | 'anual',
        periodo?: Date
    } = {
        tipoExtracto: 'mensual',
        periodo: currentDate
    };

    const form = useForm({
        'mode': 'onChange',
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

    const generarReporte = async (data: any) => {
        
        if(producto == null)
            return;
            const auditoriaApi=AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav =(window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
                    
            const requestAuditoria={
                navegador:usrAg,
                sistemaOperativo:nav.userAgentData.platform,
                nombreUsuario:user.USR_USERNAME, 
                usurioId:user.USR_ID,
                ip: publicIpAddress,
                modulo:'Generar certificado retefuente',
                hostname:document.location.host
                } as Auditoria
        setLoading(true);

        if(data.periodo) {
            const anio = data.periodo.getFullYear();

            try {
                const apiReportes = ApiReportes();
                const response = await apiReportes.CertificadoRetefuenteCredito({
                    anio: anio,
                    tipoCredito: producto.idTipoCredito,
                    numeroCredito: producto.numeroCredito
                });

                if(response?.type === "application/pdf") {
                    const filename = `Retefuente Año Gravable ${anio} - ${producto.identificacion}.pdf`;
                    DownloadBlob(response, filename);
                    handleClose();
                    requestAuditoria.tipoAccion='DESCARGAS';
                    requestAuditoria.idTipoAccion=5;
                    requestAuditoria.descripcion=`Generar certificado retefuente parámetros:
                         \n Año: ${anio}
                         \n número de crédito:  ${producto.numeroCredito}
                         \n tipo de crédito:  ${tipoCredito}
                         \n nombre archivo:${filename}
                         `
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            }
            catch(e: any) {
                let result;
                if(e.response) {
                    result = JSON.parse(await e.response.data.text());
                    toast(result.returnMessage, { type: toast.TYPE.ERROR });
                } else {
                    result="Error al generar el certificado"
                    toast(result, { type: toast.TYPE.ERROR });
                }
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8; 
                requestAuditoria.descripcion=`Generar certificado retefuente parámetros:
                \n periodo:  ${anio}
                \n número de crédito:  ${producto.numeroCredito}
                \n tipo de crédito:  ${tipoCredito}
                \n Error:  ${e}
                \n returnApiMessage: ${e.response?result.returnMessage:result}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }
            finally {
                setLoading(false);
            }
        }
      
    };

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{ width: '600px' }}>
                    <Grid item>
                        <br />
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            GENERAR CERTIFICADO DE INGRESOS Y RETENCIONES
                        </Typography>
                    </Grid>
                    <Grid container>
                        <Grid item  xs={5} style={{ margin: 'auto' }}>
                            <br />
                            <Controller
                                name="periodo"
                                control={form.control}
                                rules={{
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        views={['year']}
                                        format="yyyy"
                                        error={!!error}
                                        label="Periodo"
                                        value={value}
                                        onChange={onChange}
                                    />                                          
                                )}                                        
                            />
                        </Grid>
                    </Grid>

                    <br />

                    {
                        loading
                            ?  <div className={classes.progress}>
                                    <CircularProgress />
                                </div>
                            : <Grid container xs={12}>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    style={{ marginLeft: 'auto'}}
                                    onClick={form.handleSubmit(generarReporte)}>
                                    Generar
                                </Button>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={handleClose}
                                    style={{ marginLeft: '10px',  marginRight: 'auto' }} >
                                    Cancelar
                                </Button>
                            </Grid>
                    }   

                    <br />
                </DialogContent>
            </Dialog>
        </div>
    );
};

