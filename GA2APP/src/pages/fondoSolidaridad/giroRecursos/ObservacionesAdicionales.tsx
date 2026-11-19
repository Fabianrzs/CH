import {createStyles, Grid, makeStyles } from '@material-ui/core';
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

const ObservacionesAdicionales = (props: Props) => {

    const classes = useStyles();
    const { formulario } = props;

   return(
       <>
         <CHPaper title="Observaciones">
          <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observacion"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Si tiene observaciones adicionales ingreselas"
                                    value={value}
                                    onChange={onChange}
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

type Props = {
    formulario: UseFormReturn<any>;
}

export default ObservacionesAdicionales;