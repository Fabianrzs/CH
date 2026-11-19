import React from 'react';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonAdd: {
            width: '200px',
            marginRight: '15px',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
    }),
);

function FirmaSolicitud() {
    const classes = useStyles();

    const firma = useForm({
        defaultValues: {
            tipoIdentificacion: 0,
            numeroIdentificacion: '',
            fechaExpedicion: new Date(),
            numeroCelular: 0,
        },
    });

    return (
        <>
            <CHPaper title="Firma de la solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoIdentificacion"
                            control={firma.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Tipo de Identificación"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        CEDULA
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroIdentificacion"
                            control={firma.control}
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
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número de Identificación"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaExpedicion"
                            control={firma.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Expedición"
                                    onChange={onChange}
                                    value={value}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroCelular"
                            control={firma.control}
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
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Celular"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            // onClick={matriculasControl.handleSubmit(
                            //     agregarMatricula,
                            // )}
                        >
                            FIRMA DOCUMENTO
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
}

export default FirmaSolicitud;
