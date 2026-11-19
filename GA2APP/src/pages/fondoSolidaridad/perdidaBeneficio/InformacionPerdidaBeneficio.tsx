import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React from 'react';
import { Controller } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

const InformacionPerdidaBeneficio = (props: Props) => {
    const classes = useStyles();
    const {formularioInformacionPerdidaBeneficio} = props;

    return(
        <>
         <CHPaper title="Información Pérdida de Beneficio o Renuncia Voluntaria ">
               <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="valorAjustar"
                            control={formularioInformacionPerdidaBeneficio.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        currencyMask
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Valor a Ajustar Cuenta Individual"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="radicado"
                            control={formularioInformacionPerdidaBeneficio.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly
                                        label="Radicado de la Postulación"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="numeroResolucion"
                            control={formularioInformacionPerdidaBeneficio.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Número de Resolución"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="fechaEjecutoria"
                            control={formularioInformacionPerdidaBeneficio.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Fecha de Ejecutoria"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                <Grid className={classes.column} item xs={12} md={12}>
                    <Controller
                            name="conceptoResolucion"
                            control={formularioInformacionPerdidaBeneficio.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Concepto de Resolución"
                                        multiline
                                        rows={3}
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                </Grid>
         </CHPaper>
        </>
    )
}

type Props = {
    formularioInformacionPerdidaBeneficio: any;
}

export default InformacionPerdidaBeneficio;