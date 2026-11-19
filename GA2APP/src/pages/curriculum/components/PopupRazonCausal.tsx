import { Button, createStyles, Dialog, DialogContent, DialogTitle, Grid, makeStyles, MenuItem } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import swal from 'sweetalert';
import { startGetAllIdentificationTypes } from "../../../actions/catalog.action";
import CreditApi from "../../../services/api-credit.service";
import NotificationService from "../../../services/notification.service";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import CHSelect from "../../../components/ch-select/CHSelect";
import ApiClients from "../../../services/api-clients.service";
import { ClienteDto } from "../../../types/client.model.type";
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

interface CausalEstadoCliente {
    causal: string;
    estadoCausalClienteId: number;
}

export const PopupRazonCausal = (props: any) => {
    const { open, handleModalRazonClose, clientModel, updateRazon,causaEstadoId } = props;
    const user = AuthService.getUserInfo();
    const api = ApiClients();
    const { control, watch, setValue, handleSubmit, reset } = useForm({
        defaultValues: {
            razon: '',
        },
    });
    const [idUser, setIdUser] = useState();
    const [estadoCausalClienteId, setEstadoCausalClienteId] = useState<number | null>(null);
    const [estadoClienteId, setEstadoClienteId] = useState<number | null>(null);
    const [causalesEstadoCliente, setCausalesEstadoCliente] = useState<CausalEstadoCliente[]>([]);
    const [razon, setRazon] = useState('');
    const fechaActual = new Date();
    // const razon = watch('razon');

    useEffect(() => {
        if (clientModel) {
            const razones = clientModel.causalesEstadoCliente.map((item: any) => ({
                razonn: item.razon,
                estadoCausalClienteId: item.causalEstadoId,
            }));
            setCausalesEstadoCliente(razones);
        }
    }, [clientModel]);

    const InsertarRazonCausal = async () => {

        try {
            
            const response = await api.LevantarEstadoCausalCliente({

                causalEstadoId: causaEstadoId,
                causalId: 0,
                estadoId: 0,
                clienteId: clientModel.idCliente,
                descripcionCausal: '',
                nombreEstado: '',
                razon: razon,
                observaciones: '',
                fecha: fechaActual,
                usuario: user.USR_USERNAME,

            });
            return response;

        } catch (error) {
            toast.error(`Error al enviar datos descarga definitiva: ${error}`);
            return null;
        }
    };

    const handleConfirmar = async () => {
        try {
            const nuevasAnotaciones = await InsertarRazonCausal();

            updateRazon(nuevasAnotaciones);

            handleModalRazonClose();
            swal({
                text: 'Razón del levantamiento Agregada, consultar nuevamente.',
                icon: 'success',
            });
            history.push('/HojaDeVida');
        } catch (error: any) {
            toast.error(`Error al confirmar la observación: ${error.message || "Error desconocido"}`);
        }
    };


    return (
        <>
            <Dialog
                open={open}
                onClose={handleModalRazonClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >

                <DialogTitle>RAZÓN DEL LEVANTAMIENTO DE LA CAUSAL</DialogTitle>
                <DialogContent style={{ width: '600px' }}>
                    <Grid container>
                        <Grid className="item-containerModal" item xs={12} md={12}>
                            <Controller
                                name="razon"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        label="Razón"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        value={razon}
                                        onChange={(event) => {
                                            const inputValue = event.target.value;
                                            setRazon(inputValue);
                                            onChange(inputValue);
                                        }}
                                    />
                                )}
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
                                onClick={handleSubmit(handleConfirmar)}
                            >
                                CONFIRMAR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"

                                style={{ marginLeft: '10px' }}
                                onClick={handleModalRazonClose}
                            >
                                CANCELAR
                            </Button>
                        </Grid>
                    </Grid>
                    <br />
                </DialogContent>

            </Dialog>
        </>
    )
}