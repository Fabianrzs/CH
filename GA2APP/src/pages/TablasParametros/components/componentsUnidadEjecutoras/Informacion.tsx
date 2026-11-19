import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const Informacion = (props: Props) => {
    const classes = useStyles();
    const {formulario} = props;

    return(
        <>
           <CHPaper title='Unidad Ejecutora'>
               <Grid container>
                 <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="identificacion"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Identificación *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="nombre"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Nombre *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
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
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="sigla"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Sigla *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
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
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fuerza"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Fuerza *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="codigoUE"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Código *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaAporte"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Tasa Aporte *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="areaNegocio"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Area Negocio *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="frmID"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="frmID *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="aval"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Aval *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="principalUE"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Principal *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                           />
                        )}
                    />
                </Grid>
              </Grid>
           </CHPaper>
        </>
    )
}

type Props = {
   formulario: UseFormReturn<any>;
}

export default Informacion;