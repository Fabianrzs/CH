import { Button, createStyles, Dialog, DialogContent, DialogTitle, Grid, makeStyles } from "@material-ui/core";
import React, { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import swal from 'sweetalert';
import CreditApi from "../../../services/api-credit.service";
import NotificationService from "../../../services/notification.service";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import ApiClients from "../../../services/api-clients.service";
import AuthService from "../../../services/auth.service";
import history from '../../../utils/history.router';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        columnDate: {
            minWidth: '110px',

        },
    }),
);


export const PopupObservacionesAnotaciones = (props: any) => {
    const api = ApiClients();
    const user = AuthService.getUserInfo();

    const { open, handleModalClose, clientModel, updateAnotaciones, appendAnotacion } = props;

    const [observacion, setObservacion] = useState('');
    const [idUser, setIdUser] = useState();
    const fechaActual = new Date();

    const InsertarAnotacionCliente = async () => {

        try {
            const response = await api.InsertarAnotacionCliente({
                anotacionClienteId: 0,
                anotacion: observacion,
                usuario: user.USR_USERNAME,
                fechaAnotacion: fechaActual,
                clienteId: clientModel.idCliente
            });
            return response;

        } catch (error) {
            toast.error(`Error al enviar datos descarga definitiva: ${error}`);
            return null;
        }
    };

    const obtenerAnotacionesActualizadas = async (): Promise<any[]> => {
        try {
            await InsertarAnotacionCliente();

            const anotacionesParaEnviar = clientModel.anotaciones.map((anotacion: any) => ({
                anotacionClienteId: anotacion.anotacioN_ID,
                anotacion: anotacion.anotacion,
                usuario: anotacion.usuario,
                fechaAnotacion: anotacion.fechA_ANOTACION,
                clienteId: anotacion.clI_ID
            }));


            return anotacionesParaEnviar;
        } catch (error) {

            toast.error('Error al obtener las actualizaciones', { type: toast.TYPE.ERROR });
            throw error;
        }
    };


    const handleConfirmar = async () => {
        try {
            const resp = await api.InsertarAnotacionCliente({
                anotacionClienteId: 0,
                anotacion: observacion,
                usuario: user.USR_USERNAME,
                fechaAnotacion: new Date(),
                clienteId: clientModel.idCliente
            });

            const nueva = {
                anotacioN_ID: resp?.anotacionClienteId ?? resp?.id ?? Date.now(),
                anotacion: observacion,
                usuario: user.USR_USERNAME,
                fechA_ANOTACION: new Date(),
                clI_ID: clientModel.idCliente,
            };

            appendAnotacion(nueva);
            handleModalClose();
            swal({ text: 'Observación agregada correctamente.', icon: 'success' });
        } catch (e: any) {
            toast.error(`Error al confirmar: ${e?.message || "Error desconocido"}`);
        }
    };




    return (
        <>
            <Dialog
                open={open}
                onClose={handleModalClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogTitle>OBSERVACIONES</DialogTitle>
                <DialogContent style={{ width: '600px' }}>
                    <Grid container>
                        <Grid className="item-containerModal" item xs={12} md={12}>

                            <CHTextField
                                label="Observaciones"
                                multiline
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                                value={observacion}
                                onChange={(event) => setObservacion(event.target.value.toUpperCase())}
                            />

                        </Grid>

                    </Grid>


                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={handleConfirmar}
                            >
                                CONFIRMAR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                style={{ marginLeft: '10px' }}
                                onClick={handleModalClose}
                            >
                                CANCELAR
                            </Button>
                        </Grid>
                    </Grid>
                    <br />
                </DialogContent>
            </Dialog>
        </>
    );
}
