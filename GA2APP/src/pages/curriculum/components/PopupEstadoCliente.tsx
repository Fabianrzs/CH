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
    estadoId: number;
}

interface EstadoCliente {
    descriptions: string;
    secondsId: number;
}


export const PopupEstadoCliente = (props: any) => {

    const { open, handleModalClose, InsertarEstadoCausalCliente, clientModel, updateEstados } = props;
    const user = AuthService.getUserInfo();

    const { control, watch, setValue, handleSubmit, reset } = useForm({
        defaultValues: {
            nombreEstado: '',
            descripcionCausal: '',
            observaciones: '',
        },
    });

    const [estadoCausalClienteId, setEstadoCausalClienteId] = useState<number | null>(null);
    const [estadoClienteId, setEstadoClienteId] = useState<number | null>(null);
    const [causalesEstadoCliente, setCausalesEstadoCliente] = useState<CausalEstadoCliente[]>([]);
    const [estadosCliente, setEstadosCliente] = useState<EstadoCliente[]>([]);
    const [filteredCausales, setFilteredCausales] = useState<CausalEstadoCliente[]>([]);
    const [idUser, setIdUser] = useState();
    const estado = watch('nombreEstado');
    const causal = watch('descripcionCausal');

    useEffect(() => {
        if (clientModel) {
            
            if (Array.isArray(clientModel.causalesEstadoCliente)) {
                const causales = clientModel.causalesEstadoCliente.map((item: any) => ({
                    causal: item.causal,
                    estadoCausalClienteId: item.estadoCausalClienteId,
                    estadoId: item.estadoId,
                }));
                setCausalesEstadoCliente(causales);
            } else {
                setCausalesEstadoCliente([]); 
            }
    
        
            if (Array.isArray(clientModel.estadosCliente)) {
                const estados = clientModel.estadosCliente.map((item: any) => ({
                    descriptions: item.descriptions,
                    secondsId: item.secondsId,
                }));
                setEstadosCliente(estados);
            } else {
                setEstadosCliente([]);
            }
        }
    }, [clientModel]);
    
    useEffect(() => {
        if (estadoClienteId !== null) {
            console.log('estadoClienteId', estadoClienteId)
            const filtered = causalesEstadoCliente.filter(causal => causal.estadoId === estadoClienteId);
            setFilteredCausales(filtered);
        }
    }, [estadoClienteId, causalesEstadoCliente]);


    const handleConfirm = async (data: any) => {
       
        try {
            
                const newData = {
                    causalEstadoId: 0,
                    causalId: estadoCausalClienteId,
                    estadoId: estadoClienteId,
                    clienteId: clientModel.idCliente,
                    descripcionCausal: causal ?? '',
                    nombreEstado: estado,
                    razon: '',
                    observaciones: data.observaciones,
                    fecha: new Date().toISOString().slice(0, 10),
                    usuario: idUser === undefined ? user.USR_ID : idUser,
                };

                const nuevosEstados = await InsertarEstadoCausalCliente(newData, estado, causal);
                updateEstados(nuevosEstados);
                handleModalClose();
                swal({
                    text: 'Datos Agregados, consultar nuevamente.',
                    icon: 'success',
                });
                reset();
                setEstadoCausalClienteId(null);
                setEstadoClienteId(null);
                history.push('/HojaDeVida');
           
        } catch (error) {
            toast(`Error al agregar datos: ${error}`, { type: toast.TYPE.ERROR });
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

                <DialogTitle>ADICIONAR</DialogTitle>
                <DialogContent style={{ width: '600px' }}>
                    <Grid container>
                        <Grid className="item-containerModal" item xs={12} md={12}>
                            <Controller
                                name="nombreEstado"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Estado*"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={(e) => {
                                            const selectedId = e.target.value;
                                            const selectedEstado = estadosCliente.find(estado => estado.descriptions === selectedId);
                                            onChange(selectedId);
                                            if (selectedEstado) {
                                                setEstadoClienteId(selectedEstado.secondsId);
                                            }
                                        }}
                                    >
                                        {estadosCliente.map((estado, index) => (
                                            <MenuItem key={index} value={estado.descriptions}>
                                                {estado.descriptions}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                            {(estadoClienteId !== 0 && estadoClienteId !== 1) && (
                                <Controller
                                    name="descripcionCausal"
                                    control={control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Causales*"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={(e) => {
                                                const selectedId = e.target.value;
                                                const selectedCausal = filteredCausales.find(causal => causal.causal === selectedId);
                                                onChange(selectedId);
                                                if (selectedCausal) {
                                                    setEstadoCausalClienteId(selectedCausal.estadoCausalClienteId);
                                                }
                                            }}
                                        >
                                            {filteredCausales.map((causal, index) => (
                                                <MenuItem key={index} value={causal.causal}>
                                                    {causal.causal}
                                                </MenuItem>
                                            ))}

                                        </CHSelect>
                                    )}
                                />
                            )}
                            <Controller
                                name="observaciones"
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
                                        label="Observaciones*"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        value={value}
                                        onChange={onChange}
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
                                onClick={handleSubmit(handleConfirm)}
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
    )
}