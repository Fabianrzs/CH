import React, { useState, ChangeEvent } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { createStyles, Grid, MenuItem, RadioGroup, Radio, FormLabel, makeStyles, MuiThemeProvider, createTheme } from '@material-ui/core';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { ConyugueModel } from '../../../../models/conyuge-solicitudes.model';

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

const InformacionConyugue = ({
  formulario,
  tiposIdentificacion,
  responseSolicitud,
  listaConyuge
}: {
  formulario: UseFormReturn<any>,
  tiposIdentificacion: IDENTIFICATIONTYPEDTO[]
  responseSolicitud:any
  listaConyuge: ConyugueModel[];
}) => {
  const classes = useStyles();

  // Obtener el estado actual del formulario
  const { errors, touchedFields } = formulario.formState;
  // Verificar si el campo numeroDocumentoConyugue ha sido tocado o tiene un error
  const numeroDocumentoConyugueTouched = touchedFields.numeroDocumentoConyugue;
  const numeroDocumentoConyugueError = errors.numeroDocumentoConyugue;

  const formLabelsTheme = createTheme({
    overrides: {
      MuiFormLabel: {
        asterisk: {
          color: '#db3131',
          '&$error': {
            color: '#db3131',
          },
        },
      },
    },
  });

  return (
    <>
     {/* {
      responseSolicitud.timCodigo === 'LIBERGRAV' ||  responseSolicitud.timCodigo === 'REPARVIV' && */}
      <CHPaper title='Información del cónyuge (si aplica)'>
      <Grid container>
        <Grid className={classes.column} item xs={12} md={3}>
          <Controller
            name='tipoDocumentoConyugue'
            control={formulario.control}
            rules={{
              required: 'Este campo es requerido.',
            }}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <MuiThemeProvider theme={formLabelsTheme}>
                <CHSelect
                  error={!!error}
                  label='Tipo de Documento *'
                  value={value !== "" ? parseInt(value, 10) : tiposIdentificacion[0]?.id}
                  helperText={error ? error.message : null}
                  disabled
                  onChange={(value) => {
                    onChange(value);
                  }}
                >
                  {tiposIdentificacion
											 .filter(item => item.id !== 0)
											 ?.map((option: IDENTIFICATIONTYPEDTO) => {
												 const dato = `${option.id}`;
												 return (
													 <MenuItem
														 key={option.id}
														 value={dato}
													 >
														 {option.descripcion}
													 </MenuItem>
												 );
											 })}
                </CHSelect>
              </MuiThemeProvider>
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
          <Controller
            name='numeroDocumentoConyugue'
            control={formulario.control}
            rules={{
              required: 'Este campo es requerido.',
            }}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <MuiThemeProvider theme={formLabelsTheme}>
                <CHTextField
                  numberMask
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Número de Documento.'
                  value={value}
                  onChange={onChange}
                  disabled
                />
              </MuiThemeProvider>
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
          <Controller
            name='primerNombreConyuge'
            control={formulario.control}
            rules={{
              required: 'Este campo es requerido.',
            }}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <MuiThemeProvider theme={formLabelsTheme}>
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Primer Nombre'
                  value={value}
                  onChange={onChange}
                  disabled
                />
              </MuiThemeProvider>
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
          <Controller
            name='segundoNombreConyuge'
            control={formulario.control}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <MuiThemeProvider theme={formLabelsTheme}>
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Segundo Nombre'
                  value={value}
                  onChange={onChange}
                  disabled
                />
              </MuiThemeProvider>
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
          <Controller
            name='primerApellidoConyuge'
            control={formulario.control}
            rules={{
              required: 'Este campo es requerido.',
            }}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <MuiThemeProvider theme={formLabelsTheme}>
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Primer Apellido'
                  value={value}
                  onChange={onChange}
                  disabled
                />
              </MuiThemeProvider>
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
          <Controller
            name='segundoApellidoConyuge'
            control={formulario.control}
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <MuiThemeProvider theme={formLabelsTheme}>
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Segundo Apellido'
                  value={value}
                  onChange={onChange}
                  disabled
                />
              </MuiThemeProvider>
            )}
          />
        </Grid>
        <Grid className={classes.column} item xs={12} md={12}>
        {formulario.getValues('numeroDocumentoConyugue') === '' || formulario.getValues('numeroDocumentoConyugue') === null ? (
          <p>Actualmente no tiene cónyuge, por favor actualice su información.</p>
        ) : (
          <p> </p>
        )}
        </Grid>
      </Grid>
    </CHPaper>
    {/* } */}
    </>
   
  
  )
}

export default InformacionConyugue;