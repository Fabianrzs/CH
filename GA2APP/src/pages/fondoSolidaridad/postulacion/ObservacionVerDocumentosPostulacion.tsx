import {createStyles, 
        makeStyles, 
        Grid } from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
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

const ObservacionVerificacionPostulacion = (props: props) => {

    const classes = useStyles();
   
    const {formulario, 
           nombreCampo,
           titulo,
           camposVerificacionSoloLectura} = props;

    return (
        <>
        <CHPaper title={titulo}>
          <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name={nombreCampo}
                            control={formulario.control}
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
                                    label='Observaciones adicionales *'
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={camposVerificacionSoloLectura}
                                    multiline
                                    rows={2}
                                    InputLabelProps={{ shrink: true }}
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
            </CHPaper>
        </>
    )
}

type props = {
    formulario: UseFormReturn<any>;
    nombreCampo: string;
    titulo: string;
    camposVerificacionSoloLectura: boolean;
}

export default ObservacionVerificacionPostulacion;

