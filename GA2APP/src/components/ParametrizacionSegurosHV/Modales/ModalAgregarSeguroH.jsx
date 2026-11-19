import { Button, Card, CardContent, Container, createStyles, Dialog, Grid, makeStyles, MenuItem, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CreditApi from "../../../services/api-credit.service";
import NotificationService from "../../../services/notification.service";
import CHSelect from "../../ch-select/CHSelect";
import CHTextField from "../../ch-text-field/CHTextField";

const useStyles = makeStyles(() =>
    createStyles({   
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },       
    }),
);


const ModalAgregarSeguroH= (props) => {
    const {
        open,
        handleClose,
        obtenerAseguradorasCreadas,
        itemActulizar,
         } = props;

        const apiCredito = CreditApi()

    const formularioSeguroHogar = useForm({
        defaultValues: {
            razonSocial: "",
            sitioWeb: "",
            estado: 0,
            convenio: "S",
            ramo: "H"
}
    })
    const [mostrarBtnActulizar, setMostrarBtnActulizar] =useState(false)

    useEffect(()=> {
        
        if(itemActulizar !== undefined ){
            llenarFormularioActulizar()
            setMostrarBtnActulizar(false)
        } else{
            setMostrarBtnActulizar(true)
            formularioSeguroHogar.reset()
        }
        
    }, [itemActulizar])

  
  
    const llenarFormularioActulizar = () => {
            formularioSeguroHogar.setValue('razonSocial',itemActulizar.razonSocial)
            formularioSeguroHogar.setValue('sitioWeb',itemActulizar.sitioWeb)
            formularioSeguroHogar.setValue('estado',itemActulizar.estado)
            formularioSeguroHogar.setValue('convenio',itemActulizar.convenio)
            formularioSeguroHogar.setValue('ramo',itemActulizar.ramo)
    }


    const agregarSeuro = async (data) => {
        
        const response = {
            razonSocial: data.razonSocial,
            sitioWeb: data.sitioWeb,
            estado: data.estado,
            convenio: data.convenio,
            ramo: data.ramo
        }
        await apiCredito
            .postCrearAseguradoras(response)
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {
                        obtenerAseguradorasCreadas(response.data)
                        formularioSeguroHogar.reset()
                        handleClose(false)
                            await NotificationService.genericAlert(
                                'Se creo con éxito',
                                'Se realizó con éxito la creación del seguro',
                            );
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const actualizarSeguro = async(data) => {
       const response = {
        id: itemActulizar.id ,
        razonSocial: data.razonSocial,
        sitioWeb: data.sitioWeb,
        estado: data.estado,
        convenio: data.convenio,
        ramo: data.ramo
       }
       await apiCredito
       .putAseguradorasActulizar(response)
       .then(async (response) => {
           if (response) {
               if (response.isSuccess) {
                obtenerAseguradorasCreadas(response.data)
                   formularioSeguroHogar.reset()
                   handleClose(false)
                       await NotificationService.genericAlert(
                           'Se actualizó con éxito',
                           'Se realizó con éxito la actulización del seguro',
                       );
               }
           }
       })
       .catch(() => {
           toast('Error del sistema', { type: toast.TYPE.ERROR });
       });
    }

    const classes = useStyles();

   

        return (
            <>
                <Dialog
                    open={open}
                    onClose={handleClose}
                    aria-labelledby="form-dialog-title"
                >
                    <Container maxWidth="lg">
                        <Card style={{ width: '500px' }}>
                            <CardContent>
                                {
                                    mostrarBtnActulizar?
                                  <Grid item>
                                    <Typography
                                        className={classes.titleform}
                                        variant="h5"
                                        component="h6"
                                    >
                                        AGREGAR UN NUEVO SERGURO
                                    </Typography>
                                </Grid>:  <Grid item>
                                    <Typography
                                        className={classes.titleform}
                                        variant="h5"
                                        component="h6"
                                    >
                                        ACTUALIZAR UN SERGURO
                                    </Typography>
                                </Grid>
                                }
                              
                                <Grid container spacing={3}>
                                    <Grid item xs={12}>
                                        <Controller
                                            name="razonSocial"
                                            control={formularioSeguroHogar.control}
                                            rules={{
                                                required: 'Este campo es requerido.',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    InputLabelProps={{ shrink: true }}
                                                    label="Razon Social *"
                                                    value={value}
                                                    onChange={onChange}
                                                    error={!!error}
                                                    helperText={
                                                        error ? error.message : null
                                                    }
                                                />
                                            )}
                                        />
                                    </Grid>

                                    <Grid item xs={12}>
                                        <Controller
                                            name="sitioWeb"
                                            control={formularioSeguroHogar.control}

                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    InputLabelProps={{ shrink: true }}
                                                    label="Sitio Web"
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    <Grid item md={12} >
                                        <Controller
                                            name="estado"
                                            control={
                                                formularioSeguroHogar.control
                                            }
                                            rules={{
                                                required: 'Este campo es requerido.',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHSelect
                                                    error={!!error}
                                                    label="Estado *"
                                                    value={value}
                                                    helperText={
                                                        error ? error.message : null
                                                    }
                                                    onChange={onChange}
                                                >
                                                    <MenuItem key="0" value="0">
                                                        INACTIVO
                                                    </MenuItem>
                                                    <MenuItem key="1" value="1">
                                                        ACTIVO
                                                    </MenuItem>
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item md={12} >
                                        <Controller
                                            name="convenio"
                                            control={
                                                formularioSeguroHogar.control
                                            }
                                            rules={{
                                                required: 'Este campo es requerido.',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHSelect
                                                    error={!!error}
                                                    label="Convenio *"
                                                    value={value}
                                                    helperText={
                                                        error ? error.message : null
                                                    }
                                                    onChange={onChange}
                                                >
                                                    <MenuItem key="S" value="S">
                                                        SI
                                                    </MenuItem>
                                                    <MenuItem key="N" value="N">
                                                        NO
                                                    </MenuItem>
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item md={12} >
                                        <Controller
                                            name="ramo"
                                            control={
                                                formularioSeguroHogar.control
                                            }
                                            rules={{
                                                required: 'Este campo es requerido.',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHSelect
                                                    error={!!error}
                                                    label="Tipo de Seguro *"
                                                    value={value}
                                                    helperText={
                                                        error ? error.message : null
                                                    }
                                                    onChange={onChange}
                                                >
                                                    <MenuItem key="H" value="H">
                                                        SEGURO HOGAR
                                                    </MenuItem>
                                                    <MenuItem key="V" value="V">
                                                        SEGURO VIDA
                                                    </MenuItem>
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                </Grid>

                                <Grid container spacing={3}>
                                    <Grid item xs={12}>
                                        {
                                            mostrarBtnActulizar &&
                                            <Button
                                                size="small"
                                                variant="contained"
                                                color="primary"
                                                onClick={formularioSeguroHogar.handleSubmit(agregarSeuro)}
                                            >
                                                Agregar
                                            </Button>
                                        }
                                        {
                                            !mostrarBtnActulizar &&
                                            <Button
                                                size="small"
                                                variant="contained"
                                                color="primary"
                                                onClick={formularioSeguroHogar.handleSubmit(actualizarSeguro)}
                                            >
                                                Actualizar
                                            </Button>
                                        }

                                        <Button
                                            size="small"
                                            variant="contained"
                                            color="default"
                                            onClick={handleClose}
                                            style={{ marginLeft: "10px" }}
                                        >
                                            Cancelar
                                        </Button>
                                    </Grid>
                                </Grid>
                            </CardContent>
                        </Card>
                    </Container>
                </Dialog>
            </>
        )
}

export default ModalAgregarSeguroH;