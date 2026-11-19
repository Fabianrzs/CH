import {createStyles, FormControlLabel, Grid, makeStyles,  Radio, RadioGroup } from '@material-ui/core';
import React,  { useEffect } from 'react';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import CHPaper from '../../../../components/ch-paper/CHPaper';

const useStyles = makeStyles(() =>
  createStyles({
    radio: {
      color: '#BDBDBD',
      '&.Mui-checked': {
        color: '#1C3F8B',
      },
    },
    labelSeleccionado: {
      '& .MuiFormControlLabel-label': {
        color: 'rgba(0, 0, 0, 0.87)',
        opacity: 1,
      },
    },
    labelNoSeleccionado: {
      '& .MuiFormControlLabel-label': {
        color: 'rgba(0, 0, 0, 0.38)',
        opacity: 1,
      },
    },
  }),
);

const DestinacionDineros = (props: PropsType) => {

    const classes = useStyles();
    const {formulario, campoSoloLecturaRadicacionAprobada, setDestinacionDinerosSeleccionado} = props;

    return (
        <CHPaper title='Destinación de los dineros'>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <Controller
            name="destinacionDinero"
            control={formulario.control}
            render={({ field: { onChange, value } }) => (
              <RadioGroup
                row 
                value={value}
                onChange={(e) =>{
                  onChange(e.target.value)
                  setDestinacionDinerosSeleccionado(e.target.value);
                }}
              >
                <FormControlLabel
                  className={
                    value === 'retiro'
                      ? classes.labelSeleccionado
                      : classes.labelNoSeleccionado
                  }
                  value="retiro"
                  control={
                    <Radio
                      className={classes.radio}
                      disabled={campoSoloLecturaRadicacionAprobada}
                    />
                  }
                  label="Retiro de ahorros"
                />

                <FormControlLabel
                 className={
                  value === 'transferencia'
                    ? classes.labelSeleccionado
                    : classes.labelNoSeleccionado
                  } 
                  value="transferencia"
                  control={
                    <Radio
                      className={classes.radio}
                      disabled={campoSoloLecturaRadicacionAprobada}
                    />
                  }
                  label="Transferencia entre cuentas"
                />
              </RadioGroup>
            )}
          />
        </Grid>
      </Grid>
    </CHPaper>
    )

}

type PropsType = {
  formulario: UseFormReturn<any>;
  campoSoloLecturaRadicacionAprobada: boolean;
  setDestinacionDinerosSeleccionado: (value: string) => void;
}

export default DestinacionDineros;