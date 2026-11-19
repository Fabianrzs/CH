import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn, useForm } from 'react-hook-form';
import { createStyles, Grid, MenuItem, RadioGroup, Radio, FormLabel, makeStyles, MuiThemeProvider, createTheme } from '@material-ui/core';
import  CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { InfoTurnoDto, sucursalesValor, recepcionesValor, CatalogData } from '../../../../types/catalog.type';
import { InformacionTurnosModel } from '../../../../models/Informacion-turno.model';

type RecepcionOption = {
  riT_ID: string;
  riT_DESCRIPCION: string;
};

const useStyles = makeStyles(() =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center'
    },
    columnLabel: {
      padding: '10px'
    }
  })
);

const InformacionDeEscrituras = ({
  formulario,
  responseSolicitud
}: {
  formulario: UseFormReturn<any>,
  responseSolicitud:any
}) => {
  const classes = useStyles();
  const [disabledradPreTramite, setDisabledradPreTramite] = useState<boolean>(false);
  const today = new Date();

  const [disabledDetalle, setdisabledDetalle] = useState<boolean>(false);

	useEffect(() => {
  
    if (responseSolicitud?.solicitudEscrituracion !== undefined && responseSolicitud?.solicitudEscrituracion !== null) {
      formulario.setValue('numeroDeEscrituraIE', responseSolicitud?.solicitudEscrituracion.siE_NUMERO_ESCRITURACION);
      formulario.setValue('fechaDeEscrituraciónIE', responseSolicitud?.solicitudEscrituracion.siE_FECHA_ESCRITURACION);
      formulario.setValue('notariaIE', responseSolicitud?.solicitudEscrituracion.siE_NOTARIA);
      setdisabledDetalle(true);
    }

	}, []);

  return (
    <>
     {/* */}
  <CHPaper title='Información de escritura'>
    <Grid container>


        <Grid className={classes.column} item xs={12} md={6} lg={3}>
          <Controller
            name='numeroDeEscrituraIE'
            control={formulario.control}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
             
                <CHTextField
                  numberMask
                  error={!!error}
                  disabled={disabledradPreTramite}
                  helperText={error ? error.message : null}
                  isReadOnly={disabledDetalle}
                  label='Número de escritura *'
                  value={value}
                  onChange={onChange}
                />
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={6} lg={3}>
          <Controller
            name="fechaDeEscrituraciónIE"
            control={formulario.control}
            render={({ field: { onChange, value }, fieldState: { error } }) => (
              <CHDatePicker
                error={!!error}
                helperText={error ? error.message : ""}
                label="Fecha de escrituración *"
                onChange={onChange}
                value={value}
                disabled={disabledDetalle}
                inputProps={{ readOnly: true }}
                InputLabelProps={{ shrink: true }}
                autoOk
                minDate={new Date("1-01-1950")}
                maxDate={today}
              />
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={6} lg={3}>
          <Controller
            name='notariaIE'
            control={formulario.control}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={disabledDetalle}
                  label='Notaria *'
                  value={value}
                  onChange={onChange}
                />
            )}
          />
        </Grid>
      </Grid>
    </CHPaper>
    {/* } */}
    </>
  )
}

export default InformacionDeEscrituras;