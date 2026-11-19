import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles, Modal, Typography } from "@material-ui/core";
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import CHTextField from '../../components/ch-text-field/CHTextField';
import SolicitudApi from '../../services/api-solicitud.service';
import { ConsultarSolicitudModel } from '../../models/requests/consultar-solicitud.model';
import { GUID_DEFAULT } from '../../constants/constants';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import NotificationService from '../../services/notification.service';

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
        btnConfirmarRojo: {
            backgroundColor: '#d32f2f !important',
            '&:hover': {
                backgroundColor: '#b71c1c !important',
            },
        },
    })
);
interface Props {
    open: boolean;
    onClose: () => void;
    responseSolicitud: any;
    setDeshalitarBotonesInconsistencia: any;
    onGeneradoExitoso?: () => void;

}

const ModalInconsitencia = (props: Props) => {

    const solicitudApi = SolicitudApi()

    const formularioCartaInconsistencia = useForm({
        defaultValues: {
            asunto: '',
            descripcion: '',

        },
    });
    const {
        open,
        onClose,
        responseSolicitud,
        setDeshalitarBotonesInconsistencia,
        onGeneradoExitoso,
    } = props;

    const api = SolicitudApi();
    const [isLoadingDos, setIsLoadingDos] = useState(false);
    const [documentoURL, setDocumentoURL] = useState('');


    const generarInconsistencias = async (data: any) => {
        setIsLoadingDos(true);
        try {
            const consultarSolicitudModel: ConsultarSolicitudModel = {
                tipoSubModalidadId: responseSolicitud?.tipoSubModalidadId,
                identificacion: responseSolicitud?.identificacionCliente,
                tipoIdentificacion: responseSolicitud?.tipoIdentificacion,
                solicitudTareaId: responseSolicitud?.solicitudTareaId,
                tareaNombre: data.descripcion,
                solicitante: "Carta de inconsistencia",
                tipoCuenta: "",
                nroPlanilla: "",
                comprobante: ""
            };

            const response = await api.ObtenerRespuestaSolicitudInconsistente(consultarSolicitudModel);

            const mimeType = response.type;

            if (mimeType) {
                const file = new Blob([response], { type: mimeType });
                const fileURL = URL.createObjectURL(file);

                const link = document.createElement("a");
                link.href = fileURL;
                link.download = "Carta De Inconsistencia.pdf";
                link.click();

                URL.revokeObjectURL(fileURL);
            }

            setDeshalitarBotonesInconsistencia(true);
            formularioCartaInconsistencia.reset();
            onClose();

            if (onGeneradoExitoso) {
                onGeneradoExitoso();
            }

        } catch (error) {
            console.error("Error al generar inconsistencia:", error);
            toast("Error al generar la carta de inconsistencia");
        } finally {
            setIsLoadingDos(false);
        }
    };

    const confirmarGeneracionCarta = async (data: any) => {
        const confirmar = await swal({
            title: 'Confirmación',
            text: '¿Está seguro que desea generar la carta de inconsistencia?',
            icon: 'warning',
            buttons: {
                cancel: {
                    text: 'Cancelar',
                    value: false,
                    visible: true,
                    closeModal: true,
                },
                confirm: {
                    text: 'Confirmar',
                    value: true,
                    visible: true,
                    closeModal: true,
                    className: classes.btnConfirmarRojo,
                    
                },
            },
            closeOnClickOutside: false,
        });

        if (!confirmar) {
            return;
        }

        await generarInconsistencias(data);
    };

    useEffect(() => {
        obtenerRadicadoSolicitud()

    }, [open])

    const obtenerRadicadoSolicitud = async () => {
        const request = {
            solicitudId: responseSolicitud?.solicitudId,
            descripcion: ""
        }
        await solicitudApi
            .PostConsultaRadicadoSolicitud(request)
            .then((response: any) => {
                if (response) {
                    formularioCartaInconsistencia.setValue('asunto', response?.data?.soL_RADICADO_GD)
                }
            }).catch(() => {
                toast('error al generar el codigo')
            })
    }


    const crearCarta = async (data: any) => {


        const request = {
            solicitudId: responseSolicitud?.solicitudId,
            descripcion: data.descripcion
        }
        await solicitudApi
            .PostInconsistenciaDescripcionSolicitud(request)
            .then((response: any) => {

                if (response) {
                    setDeshalitarBotonesInconsistencia(true)
                    onClose();
                    formularioCartaInconsistencia.reset()
                }
            }).catch(() => {
                toast('error al generar el codigo')
            })

    }
    const classes = useStyles();
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
                                        GENERAR CARTA DE INCONSISTENCIA
                                    </Typography>
                                </Grid>

                                <br />
                                <br />

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="asunto"
                                        control={formularioCartaInconsistencia.control}
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextField
                                                className={classes.texfield}
                                                inputProps={{ style: { textTransform: 'uppercase', fontSize: '19px' } }} // mayusculas
                                                label="Asunto: CARTA DE INCONSISTENCIA TRAMITE"
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
                                        control={formularioCartaInconsistencia.control}
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextField
                                                className={classes.texfield}
                                                inputProps={{ style: { fontSize: '19px' } }} // mayusculas
                                                InputLabelProps={{ shrink: true }}
                                                label="Descripción de la inconsistencia:"
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
                                        {/* <Button
                                                size="small"
                                                variant="contained"
                                                color="primary"
                                                onClick={formularioCartaInconsistencia.handleSubmit(generarInconsistencias)}
                                            >
                                                Generar
                                            </Button> */}
                                        <Button
                                            size="small"
                                            variant="contained"
                                            color="primary"
                                            disabled={isLoadingDos}
                                            onClick={formularioCartaInconsistencia.handleSubmit(confirmarGeneracionCarta)}
                                        >
                                            {isLoadingDos ? 'Generando...' : 'Generar'}
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


            {/* {documentoURL && (
                <div>
                    <h3>Vista previa del documento:</h3>
                    <iframe src={documentoURL} width="100%" height="500px" title="Vista previa del documento" />
                </div>
            )} */}
        </>
    );

}

export default ModalInconsitencia