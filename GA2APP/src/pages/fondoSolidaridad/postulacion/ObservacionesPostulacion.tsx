import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React, { useEffect } from 'react';
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

const ObservacionesPostulacion = (props: props) => {
    const classes = useStyles();

    const { formulario, 
            camposCargueDocumentosSoloLectura} = props;

    return (
        <>
         <CHPaper title="Observaciones">
          <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observacion"
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
                                    label="Observaciones Adicionales *"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={camposCargueDocumentosSoloLectura}
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
    camposCargueDocumentosSoloLectura: boolean;
}

export default ObservacionesPostulacion;