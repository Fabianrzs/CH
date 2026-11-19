import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { makeStyles, createStyles, Grid } from '@material-ui/core';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center',
    }
  }),
);

const ObservacionesInicioSolicitud = ({
  control,
  formularioAcreditacion,
  responseSolicitud,
  trazabilidad = []
}: {
  control: UseFormReturn<any>;
  formularioAcreditacion?: UseFormReturn<any>;
  responseSolicitud?: any;
  trazabilidad?: any[];
}) => {
  const classes = useStyles();


  const formularioActivo = formularioAcreditacion || control;
  const [soloLecturaAnulado, setSoloLecturaAnulado] = useState(false);

  const normalizarTexto = (texto: any) =>
    String(texto || '')
      .trim()
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '');

  const obtenerObservacionGuardada = () => {
    const trazabilidadSegura = Array.isArray(trazabilidad) ? trazabilidad : [];

    const trazabilidadOrdenadaDesc = [...trazabilidadSegura].sort(
      (a: any, b: any) =>
        new Date(b?.fechaCreacion || 0).getTime() -
        new Date(a?.fechaCreacion || 0).getTime()
    );

    const actividadAnuladaConObservacion = trazabilidadOrdenadaDesc.find((item: any) =>
      normalizarTexto(item?.descripcionEstadoSolicitud).includes('anulad') &&
      String(item?.observacion || '').trim() !== ''
    );

    const actividadRadicacionConObservacion = trazabilidadOrdenadaDesc.find((item: any) =>
      normalizarTexto(item?.descripcionTarea).includes('radicacion') &&
      String(item?.observacion || '').trim() !== ''
    );

    const cualquierActividadConObservacion = trazabilidadOrdenadaDesc.find((item: any) =>
      String(item?.observacion || '').trim() !== ''
    );

    return (
      actividadAnuladaConObservacion?.observacion ||
      actividadRadicacionConObservacion?.observacion ||
      responseSolicitud?.observacion ||
      responseSolicitud?.observaciones ||
      cualquierActividadConObservacion?.observacion ||
      ''
    );
  };

  useEffect(() => {
    const trazabilidadSegura = Array.isArray(trazabilidad) ? trazabilidad : [];
  
    const trazabilidadOrdenadaDesc = [...trazabilidadSegura].sort(
      (a: any, b: any) =>
        new Date(b?.fechaCreacion || 0).getTime() -
        new Date(a?.fechaCreacion || 0).getTime()
    );
  
    const ultimaActividad = trazabilidadOrdenadaDesc[0];
  
    const estaAnulada =
      normalizarTexto(responseSolicitud?.descripcionEstadoSolicitud).includes('anulad') ||
      normalizarTexto(responseSolicitud?.estadoSolicitud).includes('anulad') ||
      trazabilidadSegura.some((item: any) =>
        normalizarTexto(item?.descripcionEstadoSolicitud).includes('anulad')
      );
  
    setSoloLecturaAnulado(estaAnulada);
  
    const campoFueEditado =
      !!formularioActivo.formState.dirtyFields?.observaciones;
  
    const estaEnRadicacion =
      normalizarTexto(ultimaActividad?.descripcionTarea).includes('radicacion') ||
      normalizarTexto(responseSolicitud?.descripcionTarea).includes('radicacion');
  
    const estadoActualPendiente =
      normalizarTexto(ultimaActividad?.descripcionEstadoSolicitud).includes('pendiente') ||
      normalizarTexto(responseSolicitud?.descripcionEstadoSolicitud).includes('pendiente') ||
      normalizarTexto(responseSolicitud?.estadoSolicitud).includes('pendiente');
  
    const tieneRechazoPrevio = trazabilidadOrdenadaDesc.some((item: any, index: number) =>
      index > 0 &&
      normalizarTexto(item?.descripcionEstadoSolicitud).includes('rechaz')
    );
  
    const debeLimpiarObservacionesPorRechazo =
      !estaAnulada &&
      estaEnRadicacion &&
      estadoActualPendiente &&
      tieneRechazoPrevio;
  
    if (debeLimpiarObservacionesPorRechazo) {
      if (!campoFueEditado) {
        formularioActivo.setValue('observaciones', '', {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        });
      }
  
      return;
    }
  
    const observacionGuardada = obtenerObservacionGuardada();
  
    const valorActual = String(
      formularioActivo.getValues('observaciones') || ''
    ).trim();
  
    if (observacionGuardada && !campoFueEditado && !valorActual) {
      formularioActivo.setValue(
        'observaciones',
        String(observacionGuardada),
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        }
      );
    }
  
  }, [responseSolicitud, trazabilidad]);

  // useEffect(() => {
  //   const trazabilidadSegura = Array.isArray(trazabilidad) ? trazabilidad : [];

  //   const estaAnulada =
  //     normalizarTexto(responseSolicitud?.descripcionEstadoSolicitud).includes('anulad') ||
  //     normalizarTexto(responseSolicitud?.estadoSolicitud).includes('anulad') ||
  //     trazabilidadSegura.some((item: any) =>
  //       normalizarTexto(item?.descripcionEstadoSolicitud).includes('anulad')
  //     );

  //   setSoloLecturaAnulado(estaAnulada);

  //   const observacionGuardada = obtenerObservacionGuardada();

  //   const valorActual = String(
  //     formularioActivo.getValues('observaciones') || ''
  //   ).trim();

  //   const campoFueEditado =
  //     !!formularioActivo.formState.dirtyFields?.observaciones;

  //   if (observacionGuardada && !campoFueEditado && !valorActual) {
  //     formularioActivo.setValue(
  //       'observaciones',
  //       String(observacionGuardada),
  //       {
  //         shouldDirty: false,
  //         shouldTouch: false,
  //         shouldValidate: false,
  //       }
  //     );
  //   }

  // }, [responseSolicitud, trazabilidad]);

  return (
    <CHPaper title='Observaciones'>
      <Grid container>
        <Grid className={classes.column} item xs={12}>
          <Controller
            name='observaciones'
            control={formularioActivo.control}
            rules={
              soloLecturaAnulado
                ? {
                  maxLength: 1000
                }
                : {
                  required: 'Este campo es requerido.',
                  maxLength: 1000
                }
            }
            render={({
              field: { onChange, value },
              fieldState: { error },
            }) => (
              <CHTextField
                error={!!error}
                helperText={error ? error.message : null}
                InputLabelProps={{ shrink: true }}
                label={soloLecturaAnulado ? '' : 'Si tiene observaciones adicionales ingreselas'}
                value={value || ''}
                required={!soloLecturaAnulado}
                multiline
                minRows={2}
                onChange={onChange}
                isReadOnly={soloLecturaAnulado}
                inputProps={{ maxLength: 300 }}
              />
            )}
          />
        </Grid>
      </Grid>
    </CHPaper>
  );
};

export default ObservacionesInicioSolicitud;
