import { Button, createStyles, Grid, makeStyles } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import afiliadoApi from "../../services/api-afiliado.service";
import NotificationService from "../../services/notification.service";
import UtilService from "../../utils/util.service";
import CHPaper from "../ch-paper/CHPaper";
import CHTextField from "../ch-text-field/CHTextField";

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '10px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
    }),
);

function ValidacionTelefonica(props: any) {

    const {
        procedureRequest
    } = props

    const apiAfiliado = afiliadoApi()
    const formularioValidacionTelefonica = useForm({
        defaultValues: {

            valorInmueble: 0,
            anosEnFuerza: 0,
            lugarTrabajo: "",
            cargoActual: "",
            valorArriendo: "",
            valorActivos: "",
            valorPasivos: "",
            montoRecursos: "",
            observaciones: "",

        },
    })

    useEffect(() => {

        obtenerValidacionTelefonica()

    }, [])

    const obtenerValidacionTelefonica = async () => {
        const response = {
            soc_Id: procedureRequest.requestID,
        }
        await apiAfiliado
            .postObtenerValidacionTelefonica(response)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        if (response.data === null) {
                            return
                        }
                        const beneficiario = response.data

                        formularioValidacionTelefonica.setValue('valorInmueble', beneficiario.conf_Valor_Inmueble)
                        formularioValidacionTelefonica.setValue('anosEnFuerza', beneficiario.anio_Fuerzas)
                        formularioValidacionTelefonica.setValue('lugarTrabajo', beneficiario.lugar_Trabajo)
                        formularioValidacionTelefonica.setValue('cargoActual', beneficiario.cargo_Actual)
                        formularioValidacionTelefonica.setValue('valorArriendo', beneficiario.valor_Arriendo)
                        formularioValidacionTelefonica.setValue('valorActivos', beneficiario.valor_Activos)
                        formularioValidacionTelefonica.setValue('valorPasivos', beneficiario.valor_Pasivos)
                        formularioValidacionTelefonica.setValue('montoRecursos', beneficiario.monto_Recursos_Propios)
                        formularioValidacionTelefonica.setValue('observaciones', beneficiario.observaciones)

                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }



    const guardar = async (data: any) => {


        const response = {
            val_Id: 0,
            soc_Id: procedureRequest.requestID,
            conf_Valor_Inmueble: UtilService.cleanCurrencyValue(data.valorInmueble),
            anio_Fuerzas: data.anosEnFuerza,
            lugar_Trabajo: data.lugarTrabajo,
            cargo_Actual: data.cargoActual,
            valor_Arriendo: UtilService.cleanCurrencyValue(data.valorArriendo),
            valor_Activos: UtilService.cleanCurrencyValue(data.valorActivos),
            valor_Pasivos: UtilService.cleanCurrencyValue(data.valorPasivos),
            monto_Recursos_Propios: UtilService.cleanCurrencyValue(data.montoRecursos),
            observaciones: data.observaciones,

        }



        await apiAfiliado
            .postGuardarValidacionTelefonica(response)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        await NotificationService.genericAlert(
                            'Guardado',
                            'guardado con éxito.',

                        );
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const classes = useStyles();

    return (
        <>
            <CHPaper title="Validacion Telefonica">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorInmueble"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Valor Del Inmueble *"
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="anosEnFuerza"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberAniosMask
                                    label="Años En La Fuerza *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    error={!!error}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="lugarTrabajo"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    helperText={error ? error.message : null}
                                    error={!!error}
                                    label="Unidad de Trabajo *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="cargoActual"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    helperText={error ? error.message : null}
                                    error={!!error}
                                    label="Cargo Actual *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorArriendo"
                            control={formularioValidacionTelefonica.control}

                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Valor Arriendo Si Paga"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorActivos"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    helperText={error ? error.message : null}
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    error={!!error}
                                    label="Valor Activos *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorPasivos"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    helperText={error ? error.message : null}
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    error={!!error}
                                    label="Valor Pasivos *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>


                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="montoRecursos"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    helperText={error ? error.message : null}
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    error={!!error}
                                    label="Monto Recursos Propios *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="observaciones"
                            control={formularioValidacionTelefonica.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    helperText={error ? error.message : null}
                                    error={!!error}
                                    label="Observaciones*"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                </Grid>
                <Grid item xs={12} md={3}>
                    <Button
                        onClick={formularioValidacionTelefonica.handleSubmit(guardar)}
                        variant="contained"
                        style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                    >
                        Guardar
                    </Button>
                </Grid>
            </CHPaper>
        </>
    )
}



export default ValidacionTelefonica


