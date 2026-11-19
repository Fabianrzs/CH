import { Button, createStyles, Dialog, DialogContent, DialogTitle, Grid, makeStyles } from "@material-ui/core";
import React, { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CreditApi from "../../../services/api-credit.service";
import NotificationService from "../../../services/notification.service";
import CHTextField from "../../ch-text-field/CHTextField";


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
export const  ModalCrearInvitado= (props:any) => {
    const {open, cerrarModal} = props;
   const apiCredit = CreditApi();
    const formularioCrearInvitados = useForm({
        defaultValues:{
            tipoIntegrante: '',
            nombre: '',
            estado: 0,
            cargo: ''
        }
    })


   const  crearInvitado = async(data:any) => {
    const respose = {
        tipoIntegrante: 'Invitado',
        nombre: data.nombre,
        estado: 1,
        cargo: data.cargo 
    }
    await apiCredit
    .postCrearInvitadoCmite(respose)
    .then(async response => {
        if (response) {
            if (response.isSuccess) {
               
                cerrarModal()
                formularioCrearInvitados.reset()
                await NotificationService.genericAlert(
                    'Guardado con éxito ',
                    'Se creó el invitado comité',
                );
            }
         
        }
    
    })
    .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
    });
   }
    const classes = useStyles();

 

    return(
        <>
           <Dialog
                open={open}
                onClose={cerrarModal}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogTitle>AGREGAR UN NUEVO INVITADO</DialogTitle>
                <DialogContent style={{ width: '600px' }}>
                    <Grid container>
                   
                        <Grid item md={10}>

                            <Controller
                                name="nombre"
                                control={formularioCrearInvitados.control}
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
                                        label="Nombre *"
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                    />
                                )}
                            />

                        </Grid>
                        <Grid item md={10}>

                            <Controller
                                name="cargo"
                                control={formularioCrearInvitados.control}
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
                                        label="Cargo *"
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
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
                               onClick={formularioCrearInvitados.handleSubmit(crearInvitado)}
                            >
                                CONFIRMAR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                              
                                style={{ marginLeft: '10px' }}
                                onClick={cerrarModal}
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

