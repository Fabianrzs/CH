import React, { useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles, Modal, Typography } from "@material-ui/core";
import { toast } from 'react-toastify';
import { Controller,  UseFormReturn } from 'react-hook-form';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import SolicitudApi from '../../../../services/api-solicitud.service';
import { ConsultarSolicitudModel } from '../../../../models/requests/consultar-solicitud.model';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1.5rem',

        },
        fullScreenDialog: {
            width: '100%',
        },
        formRow: {
            display: 'flex',
            flexDirection: 'row',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        card: {
            width: '70%'
        },
        columnLabel: {
            padding: '10px',
        },
        texfield: {
            width: '100%',
            fontWeight: 'bold',
            fontSize: 'large'
        },

        inline: {
            display: 'inline',
        },
        column: {
            padding: '10px',
            textAlign: 'center'
        },
    })
);

interface Props {
    open: boolean;
    onClose: () => void;
    responseSolicitud: any;
    formularioCartaComunicacion: UseFormReturn<any>;
    setMostrarCampoRadicado: (value: boolean) =>void;
    setMostrarCampoRadicado2: (value: boolean) =>void;
    informacionComunicaciones: any[];
    formularioInformacionComunicacion: UseFormReturn<any>;
}

const ModalComunicacion = (props: Props) => {

    const {
        open,
        onClose,
        responseSolicitud,
        formularioCartaComunicacion,
        setMostrarCampoRadicado,
        setMostrarCampoRadicado2,
        informacionComunicaciones,
        formularioInformacionComunicacion,
    } = props;

    const api = SolicitudApi();
    const classes = useStyles();
    const [isLoadingDos, setIsLoadingDos] = useState(false);

    const generarComunicacion = async (data: any) => {
        setIsLoadingDos(true);
        try {

            await api.GetConsultarInconsistencia(responseSolicitud?.solicitudId || '')
            .then(async (response) => {
                if(response?.data){
                    let request;

                    if(responseSolicitud && responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')){
                        const grupo1Codigo378 = response?.data?.find((x: any) => x.grupoInconsistencia === 1 && x.novedad !== 'INFORME 114 - INCONSISTENCIA');
                        const grupo1Informe114 = response?.data?.find((x: any) => x.grupoInconsistencia === 1 && x.novedad === 'INFORME 114 - INCONSISTENCIA');
                        if(grupo1Codigo378){
                            request = {
                                noFolios: 1,
                                asunto: 'Información Cumplimiento de Obligaciones V-8',
                                radicado: '21-01-20250306034093',
                                clienteId: responseSolicitud?.clienteId,
                                codigo: '378',
                                solicitudId: responseSolicitud?.solicitudId,
                                idComunicacion: formularioInformacionComunicacion.getValues('idCausalComunicacion')
                            }
                        }
                        else if(grupo1Informe114){
                            request = {  
                                noFolios: 0,
                                asunto: '',
                                radicado: '21-01-20250306034093',
                                clienteId: responseSolicitud?.clienteId,
                                codigo: '114',
                                solicitudId: responseSolicitud?.solicitudId, 
                                idComunicacion: formularioInformacionComunicacion.getValues('idCausalComunicacion')
                            }
                        }
                    }

                    if(responseSolicitud && responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 2')){
                        const grupo2Codigo378 = response?.data?.find((x: any) => x.grupoInconsistencia === 2 && x.novedad !== 'INFORME 114 - INCONSISTENCIA');
                        const grupo2Informe114 = response?.data?.find((x: any) => x.grupoInconsistencia === 2 && x.novedad === 'INFORME 114 - INCONSISTENCIA');
                        if(grupo2Codigo378){
                            request = {
                                noFolios: 1,
                                asunto: 'Información Cumplimiento de Obligaciones V-8',
                                radicado: '21-01-20250306034093',
                                clienteId: responseSolicitud?.clienteId,
                                codigo: '378',
                                solicitudId: responseSolicitud?.solicitudId,
                                idComunicacion: formularioInformacionComunicacion.getValues('idCausalComunicacion2')
                            }
                        }
                        else if(grupo2Informe114){
                            request = {  
                                noFolios: 0,
                                asunto: '',
                                radicado: '21-01-20250306034093',
                                clienteId: responseSolicitud?.clienteId,
                                codigo: '114',
                                solicitudId: responseSolicitud?.solicitudId, 
                                idComunicacion: formularioInformacionComunicacion.getValues('idCausalComunicacion2')
                            }
                        }
                    }

                        await api.CrearRadicadoCartaIncosistencia(request)
                        .then(response => {
                                if(response.data){
                                    if(informacionComunicaciones && informacionComunicaciones.length === 0){
                                        formularioInformacionComunicacion.setValue('radicado' as any, response?.data);
                                    }
                                    else if(informacionComunicaciones && informacionComunicaciones.length >= 1){
                                        formularioInformacionComunicacion.setValue('radicado2' as any, response?.data);
                                        formularioCartaComunicacion.setValue('descripcion','');
                                        setMostrarCampoRadicado2(true);
                                    }
                                    toast('Información actualizada correctamente', { type: toast.TYPE.SUCCESS });
                                }
                            }
                        ).catch((e) => {
                            toast("Error al crear carta inconsistencia", { type: toast.TYPE.ERROR });
                        });
                }

                }).catch(() => {
                toast("Error al consultar inconsistencias en generar comunicaciones", { type: toast.TYPE.ERROR });
            });
            const consultarSolicitudModel: ConsultarSolicitudModel = {
                tipoSubModalidadId: responseSolicitud?.tipoSubModalidadId,
                identificacion: responseSolicitud?.identificacionCliente,
                tipoIdentificacion: responseSolicitud?.tipoIdentificacion,
                solicitudTareaId: responseSolicitud?.solicitudTareaId,
                tareaNombre: data.descripcion,
                solicitante: "Carta de comunicación",
                tipoCuenta: "",
                nroPlanilla: "",
                comprobante: ""
            };

            const response = await api.ObtenerRespuestaSolicitud(consultarSolicitudModel);
            const mimeType = response.type;
            if (mimeType) {
                setMostrarCampoRadicado(true);
                const file = new Blob([response], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                const link = document.createElement("a");
                link.href = fileURL;
                link.download = "Carta De Comunicacion.pdf";
                link.click();
                URL.revokeObjectURL(fileURL);
            }
            onClose();
        } catch (error) {
            console.error("Error al generar carta:", error);
        }
        setIsLoadingDos(false);
        return null;
    };

    return (
        <>
            <Modal
                open={open}
                onClose={onClose}
                aria-labelledby="form-dialog-title"
                style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    height: '90vh',
                }}
            >
                {isLoadingDos ? (
                    <CHLoadingProcess />
                ) : (
                    <Card className={classes.card}>
                        <CardContent>
                            <br />
                            <br />
                                <>
                                    <Grid item>
                                        <Typography
                                            className={classes.titleform}
                                            variant="h5"
                                            component="h6"
                                        >
                                            GENERAR CARTA DE COMUNICACIÓN
                                        </Typography>
                                    </Grid>
                                    <br />
                                    <br />
                                    <Grid className={classes.column} item xs={12} md={5}>
                                        <Controller
                                            name="asunto"
                                            control={formularioCartaComunicacion.control}
                                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                                <CHTextField
                                                    className={classes.texfield}
                                                    inputProps={{ style: { textTransform: 'uppercase', fontSize: '19px' } }} // mayusculas
                                                    label="Asunto: CARTA DE COMUNICACIÓN TRÁMITE"
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    InputLabelProps={{ shrink: true }}
                                                    value={value}
                                                    onChange={onChange}
                                                    isReadOnly
                                                />
                                            )}
                                        />
                                    </Grid>
                                    <Grid>
                                        <Controller
                                            name="descripcion"
                                            control={formularioCartaComunicacion.control}
                                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                                <CHTextField
                                                    className={classes.texfield}
                                                    inputProps={{ style: { fontSize: '19px' } }} // mayusculas
                                                    InputLabelProps={{ shrink: true }}
                                                    label="Descripción de la comunicación:"
                                                    multiline
                                                    rows={10}
                                                    defaultValue=""
                                                    variant="outlined"
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                            )}
                                        />
                                    </Grid>
                                    <br />

                                    <Grid container justify="flex-end" spacing={3}>
                                        <Grid item>
                                            <Button
                                                size="small"
                                                variant="contained"
                                                color="primary"
                                                onClick={formularioCartaComunicacion.handleSubmit(generarComunicacion)}
                                            >
                                                Generar
                                            </Button>
                                        </Grid>
                                        <Grid item>
                                            <Button
                                                size="small"
                                                variant="contained"
                                                color="default"
                                                onClick={onClose}
                                            >
                                                Cancelar
                                            </Button>
                                        </Grid>
                                    </Grid>
                                </>
                        </CardContent>
                    </Card>
                )}

            </Modal>
        </>
    );
}

export default ModalComunicacion