import React, { useState, useEffect, useRef } from 'react';
import { Controller } from 'react-hook-form';
import { makeStyles, createStyles, Grid, Button, Typography } from '@material-ui/core';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';

const useStyles = makeStyles(() =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center',
    },
    titleform: {
      width: '100%',
      textAlign: 'left',
      color: '#1C3F6B',
      padding: 1,
      fontWeight: 'bold',
      fontSize: '1rem',
      marginLeft: '10px'
    },
  }),
);


const TABS_INDEX = {
  RESUMENSOLICITUD: 'resumen',
  CARGUEDOCUMENTOS: '0',
  CARGUEDOCUMENTOSEGURIDADOCUMENTAL: '1',
  CARGUERETIROPARCIAL: 'carguededocumentosretiroparcial',
  CARGUEDOCUMENTOSV14: 'carguededocumentosv14',
  CARGUEDOCUMENTOSV8: 'carguededocumentosv8',
  INCONSISTENCIAS: '4',
  SEGURIDADDOCUMENTAL: '8',
  VERDETALLESOLICITUD: 'Detalle Solicitud',
  VERDETALLESOLICITUDVERIFICACION: '9',
  VERIFICACIONCUENTAINDIVIDUAL: '10',
  VERIFICACIONESDECUENTA: '11',
  TRAMITEDEPAGO: '12',
  VERIFICACIONLISTASRESTRICTIVAS:
    'VERIFICACIONLISTASRESTRICTIVAS',
}

type OrigenObservacion =
  | 'DYNAMIC1'
  | 'DYNAMIC2'
  | 'DYNAMIC3'
  | 'DYNAMIC4'
  | 'DYNAMIC5'
  | 'INFO_PROCESO_CARGUE_DOCUMENTOS'
  | 'INFO_PROCESO_CARGUE_DOCUMENTOS1'
  | 'INFO_PROCESO_DETALLE'
  | 'VERIFICACIONES_CUENTA'
  | 'SEGURIDAD_DOCUMENTAL1'
  | 'SEGURIDAD_DOCUMENTAL2'
  | 'SEGURIDAD_DOCUMENTAL3'
  | '';



const ObservacionesSolicitud = ({
  control,
  seccion,
  trazabilidad,
  nombreTareaActual,
  forzarObservacionEditable = false,
  soloLecturaVerificacion = false,
  mostrarObservacionGrupo1EnGrupo2 = false,
  debugOrigen = '',
  origenObservacion = '',

}: {
  control: any;
  seccion: string;
  trazabilidad: any[];
  nombreTareaActual?: string;
  forzarObservacionEditable?: boolean;
  soloLecturaVerificacion?: boolean;
  mostrarObservacionGrupo1EnGrupo2?: boolean;
  debugOrigen?: string;
  origenObservacion?: OrigenObservacion;
}) => {


  const classes = useStyles();
  const [mostrarObservacionSoloLectura, setMostrarObservacionSoloLectura] = useState(false);
  const [mostrarObservacionGrupo2, setMostrarObservacionGrupo2] = useState(false);
  const [loading, setLoading] = useState(false);
  const alertaDepuracionMostrada = useRef('');


  const estadosActivos = [
    'PENDIENTE',
    'EN VERIFICACION',
    'EN VERIFICACIÓN',
    'BLOQUEADO',
    'INCONSISTENTE',
    'EN TRÁMITE',
    'EN TRAMITE',
  ];



  const esSeccionVerificacionesCuenta = () =>
    seccion === TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL ||
    seccion === TABS_INDEX.VERIFICACIONESDECUENTA;

  const esSeccionListasRestrictivas = () =>
    seccion ===
    TABS_INDEX.VERIFICACIONLISTASRESTRICTIVAS;

  const normalizar = (texto: any) =>
    String(texto || '')
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '');



  useEffect(() => {
    if (!debugOrigen) {
      return;
    }

    const identificador = `${debugOrigen}|${seccion}|${nombreTareaActual || ''}`;

    if (alertaDepuracionMostrada.current === identificador) {
      return;
    }

    alertaDepuracionMostrada.current = identificador;

    window.alert(
      `DEBUG OBSERVACIONES SOLICITUD\n\n` +
      `Origen: ${debugOrigen}\n` +
      `Sección: ${seccion}\n` +
      `Tarea: ${nombreTareaActual || 'No enviada'}`
    );
  }, [debugOrigen, seccion, nombreTareaActual]);


  const esActividadNovedades = (item: any) => {
    const tarea = normalizar(item?.descripcionTarea);

    return (
      tarea.includes('verificacion novedades en los datos') ||
      tarea.includes('verificacion novedades')
    );
  };

  const esActividadCuentaIndividual = (item: any) => {
    const tarea = normalizar(item?.descripcionTarea);

    return tarea.includes('verificacion cuenta individual');
  };

  const esActividadTramitePagoRetiroParcial = (
    item: any,
  ) =>
    normalizar(
      item?.descripcionTarea,
    ).includes(
      'tramite de pago retiro parcial',
    );

  const llegoATramitePagoRetiroParcial = () =>
    existeActividadPorTipo(
      esActividadTramitePagoRetiroParcial,
    );

  const esActividadEmbargos = (item: any) => {
    const tarea = normalizar(item?.descripcionTarea);

    return tarea.includes('verificacion embargos');
  };

  const existeActividadPorTipo = (
    validador: (item: any) => boolean
  ) => Boolean(obtenerUltimaActividadPorTipo(validador));

  const trazabilidadOrdenadaPorFecha = () =>
    [...(Array.isArray(trazabilidad) ? trazabilidad : [])].sort(
      (a: any, b: any) =>
        new Date(b?.fechaCreacion || 0).getTime() -
        new Date(a?.fechaCreacion || 0).getTime()
    );

  const obtenerUltimaActividadPorTipo = (validador: (item: any) => boolean) =>
    trazabilidadOrdenadaPorFecha().find(validador);

  const obtenerObservacionPorTipo = (validador: (item: any) => boolean) => {
    const actividadConObservacion = trazabilidadOrdenadaPorFecha().find(
      (item: any) =>
        validador(item) &&
        item?.observacion !== null &&
        item?.observacion !== undefined &&
        String(item.observacion).trim() !== ''
    );

    return String(actividadConObservacion?.observacion || '');
  };

  const obtenerObservacionUltimaActividadPorTipo = (
    validador: (item: any) => boolean
  ) => {
    const actividad = obtenerUltimaActividadPorTipo(validador);

    return String(actividad?.observacion ?? '');
  };

  const esFlujoNovedadesCuentaSinEmbargos = () =>
    esSeccionVerificacionesCuenta() &&
    existeActividadPorTipo(esActividadNovedades) &&
    existeActividadPorTipo(esActividadCuentaIndividual) &&
    !existeActividadPorTipo(esActividadEmbargos);

  const cuentaIndividualEstaAprobada = () => {
    const actividadCuentaIndividual = obtenerUltimaActividadPorTipo(
      esActividadCuentaIndividual
    );

    return (
      normalizar(actividadCuentaIndividual?.descripcionEstadoSolicitud) ===
      'aprobada'
    );
  };

  const tramitePagoRetiroParcialEstaAprobado = () =>
    trazabilidadOrdenadaPorFecha().some((item: any) => {
      const tarea = normalizar(item?.descripcionTarea);
      const estado = normalizar(
        item?.descripcionEstadoSolicitud,
      );

      return (
        tarea.includes('tramite de pago retiro parcial') &&
        estado === 'aprobada'
      );
    });



  const esEstadoAnulado = (estado: any) =>
    normalizar(estado).includes('anulad');

  const esEstadoRechazado = (estado: any) =>
    normalizar(estado).includes('rechazad');

  const esEstadoFinal = (estado: any) =>
    esEstadoAnulado(estado) || esEstadoRechazado(estado);


  const esSeccionCargueDocumentos = () =>
    seccion === TABS_INDEX.CARGUEDOCUMENTOS ||
    seccion === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
    seccion === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
    seccion === TABS_INDEX.CARGUEDOCUMENTOSEGURIDADOCUMENTAL ||
    seccion === TABS_INDEX.CARGUERETIROPARCIAL;

  const obtenerObservacionEstadoFinal = (
    trazabilidadOrdenadaDescGeneral: any[]
  ): string => {
    const actividadCargueFinalConObservacion =
      trazabilidadOrdenadaDescGeneral.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('cargue') &&
        esEstadoFinal(item?.descripcionEstadoSolicitud) &&
        String(item?.observacion || '').trim() !== ''
      );

    const actividadCargueConObservacion =
      trazabilidadOrdenadaDescGeneral.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('cargue') &&
        String(item?.observacion || '').trim() !== ''
      );

    const actividadFinalConObservacion =
      trazabilidadOrdenadaDescGeneral.find((item: any) =>
        esEstadoFinal(item?.descripcionEstadoSolicitud) &&
        String(item?.observacion || '').trim() !== ''
      );

    const actividadRadicacionConObservacion =
      trazabilidadOrdenadaDescGeneral.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('radicacion') &&
        String(item?.observacion || '').trim() !== ''
      );

    const cualquierActividadConObservacion =
      trazabilidadOrdenadaDescGeneral.find((item: any) =>
        String(item?.observacion || '').trim() !== ''
      );

    if (esSeccionCargueDocumentos()) {
      return (
        actividadCargueFinalConObservacion?.observacion ||
        actividadCargueConObservacion?.observacion ||
        actividadFinalConObservacion?.observacion ||
        cualquierActividadConObservacion?.observacion ||
        ''
      );
    }

    return (
      actividadFinalConObservacion?.observacion ||
      actividadRadicacionConObservacion?.observacion ||
      cualquierActividadConObservacion?.observacion ||
      ''
    );
  };


  const tareaActualNormalizada = normalizar(
    nombreTareaActual ||
    trazabilidad?.find((item: any) =>
      estadosActivos.includes(
        String(item?.descripcionEstadoSolicitud || '').toUpperCase()
      )
    )?.descripcionTarea
  );



  const esTareaGrupo2Actual =
    tareaActualNormalizada.includes('seguridad documental grupo 2') ||
    tareaActualNormalizada.includes('seguridad documental de grupo 2');


  const buscarUltimaActividad = (
    trazabilidadLista: any[],
    grupo: 'grupo 1' | 'grupo 2'
  ) => {
    const grupoNormalizado = normalizar(grupo);

    return [...trazabilidadLista]
      .reverse()
      .find((item: any) =>
        normalizar(item?.descripcionTarea).includes(grupoNormalizado) &&
        estadosActivos.includes(String(item?.descripcionEstadoSolicitud || '').toUpperCase())
      );
  };

  const obtenerObservacionAnulada = (trazabilidadOrdenadaDescGeneral: any[]) => {
    const esOrigenCargueDocumentos =
      origenObservacion === 'INFO_PROCESO_CARGUE_DOCUMENTOS' ||
      seccion === TABS_INDEX.CARGUEDOCUMENTOS ||
      seccion === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
      seccion === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
      seccion === TABS_INDEX.CARGUERETIROPARCIAL;

    const actividadCargueConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
      normalizar(item?.descripcionTarea).includes('cargue') &&
      String(item?.observacion || '').trim() !== ''
    );

    const actividadAnuladaConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
      esEstadoAnulado(item?.descripcionEstadoSolicitud) &&
      String(item?.observacion || '').trim() !== ''
    );

    const actividadRadicacionConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
      normalizar(item?.descripcionTarea).includes('radicacion') &&
      String(item?.observacion || '').trim() !== ''
    );

    const cualquierActividadConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
      String(item?.observacion || '').trim() !== ''
    );

    if (esOrigenCargueDocumentos) {
      return (
        actividadCargueConObservacion?.observacion ||
        actividadAnuladaConObservacion?.observacion ||
        cualquierActividadConObservacion?.observacion ||
        ''
      );
    }

    return (
      actividadAnuladaConObservacion?.observacion ||
      actividadRadicacionConObservacion?.observacion ||
      cualquierActividadConObservacion?.observacion ||
      ''
    );
  };

  const validaciones = () => {
    setLoading(true);

    const trazabilidadSegura = Array.isArray(trazabilidad) ? trazabilidad : [];

    const trazabilidadOrdenadaDescGeneral = [...trazabilidadSegura].sort(
      (a: any, b: any) =>
        new Date(b?.fechaCreacion || 0).getTime() -
        new Date(a?.fechaCreacion || 0).getTime()
    );

    const esSeccionCargueDocumentos =
      seccion === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
      seccion === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
      seccion === TABS_INDEX.CARGUEDOCUMENTOS ||
      seccion === TABS_INDEX.CARGUEDOCUMENTOSEGURIDADOCUMENTAL ||
      seccion === TABS_INDEX.CARGUERETIROPARCIAL;

    const esSeccionDetalle =
      seccion === TABS_INDEX.VERDETALLESOLICITUD ||
      seccion === TABS_INDEX.VERDETALLESOLICITUDVERIFICACION;

    const existeTareaActiva = trazabilidadOrdenadaDescGeneral.some((item: any) =>
      estadosActivos.includes(
        String(item?.descripcionEstadoSolicitud || '').toUpperCase()
      )
    );

    const solicitudFinalizada =
      !existeTareaActiva &&
      trazabilidadOrdenadaDescGeneral.some((item: any) =>
        esEstadoFinal(item?.descripcionEstadoSolicitud)
      );

    if (solicitudFinalizada && (esSeccionCargueDocumentos || esSeccionDetalle)) {
      const actividadCargueFinalConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('cargue') &&
        esEstadoFinal(item?.descripcionEstadoSolicitud) &&
        String(item?.observacion || '').trim() !== ''
      );

      const actividadCargueConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('cargue') &&
        String(item?.observacion || '').trim() !== ''
      );

      const actividadFinalConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
        esEstadoFinal(item?.descripcionEstadoSolicitud) &&
        String(item?.observacion || '').trim() !== ''
      );

      const cualquierActividadConObservacion = trazabilidadOrdenadaDescGeneral.find((item: any) =>
        String(item?.observacion || '').trim() !== ''
      );

      const observacionFinal = esSeccionCargueDocumentos
        ? (
          actividadCargueFinalConObservacion?.observacion ||
          actividadCargueConObservacion?.observacion ||
          actividadFinalConObservacion?.observacion ||
          cualquierActividadConObservacion?.observacion ||
          ''
        )
        : (
          actividadFinalConObservacion?.observacion ||
          cualquierActividadConObservacion?.observacion ||
          ''
        );

      control.setValue('observaciones', String(observacionFinal || ''), {
        shouldDirty: false,
        shouldTouch: false,
        shouldValidate: false,
      });

      control.setValue('observacionesGrupo1', '', {
        shouldDirty: false,
        shouldTouch: false,
        shouldValidate: false,
      });

      setMostrarObservacionSoloLectura(true);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
      return;
    }

    if (forzarObservacionEditable) {
      const actividadActual = trazabilidadSegura.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('radicacion') &&
        estadosActivos.includes(
          String(item?.descripcionEstadoSolicitud || '').toUpperCase()
        )
      );

      const valorActual = control.getValues('observaciones');

      if (
        valorActual === null ||
        valorActual === undefined ||
        String(valorActual).trim() === ''
      ) {
        control.setValue(
          'observaciones',
          actividadActual?.observacion ? String(actividadActual.observacion) : '',
          {
            shouldDirty: false,
            shouldTouch: false,
            shouldValidate: false,
          }
        );
      }

      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
      return;
    }


    const tareaActual = normalizar(
      trazabilidadSegura.find((item: any) =>
        ['PENDIENTE', 'EN VERIFICACION', 'EN VERIFICACIÓN', 'BLOQUEADO', 'INCONSISTENTE']
          .includes(String(item?.descripcionEstadoSolicitud || '').toUpperCase())
      )?.descripcionTarea
    );

    const esGrupo1Actual = tareaActual.includes('grupo 1');
    const esGrupo2Actual = tareaActual.includes('grupo 2');


    let actividad;

    if (
      seccion &&
      seccion === TABS_INDEX.VERDETALLESOLICITUD &&
      trazabilidadSegura.some((item: any, index: number) =>
        item?.descripcionEstadoSolicitud === 'RECHAZADA' && index === 1
      )
    ) {
      actividad = trazabilidadSegura.find((item: any, index: number) =>
        String(item?.descripcionTarea).includes('Radicación') && index === 0
      );

      const valorActual = control.getValues('observaciones');

      if (
        valorActual === null ||
        valorActual === undefined ||
        String(valorActual).trim() === ''
      ) {
        control.setValue(
          'observaciones',
          actividad?.observacion ? String(actividad.observacion) : '',
          {
            shouldDirty: false,
            shouldTouch: false,
            shouldValidate: false,
          }
        );
      }

      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    else if (seccion && (seccion === TABS_INDEX.VERDETALLESOLICITUD) &&
      trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación en tiempo') && (item?.descripcionEstadoSolicitud === 'EN TRÁMITE' || item?.descripcionEstadoSolicitud === 'PENDIENTE'))) {

      actividad = trazabilidad?.find((item: any) => String(item?.descripcionTarea).includes('Verificación en tiempo') && (item?.descripcionEstadoSolicitud === 'EN TRÁMITE' || item?.descripcionEstadoSolicitud === 'PENDIENTE'));
      control.setValue('observaciones', actividad?.observacion);
      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    else if (seccion && (seccion === TABS_INDEX.VERDETALLESOLICITUD) &&
      trazabilidad?.some((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'EN TRÁMITE')) {

      actividad = trazabilidad?.find((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'EN TRÁMITE');
      control.setValue('observaciones', actividad?.observacion);
      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    else if (seccion && (seccion === TABS_INDEX.VERDETALLESOLICITUD) &&
      trazabilidad?.some((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'APROBADA') &&
      trazabilidad?.some((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'PENDIENTE')) {

      actividad = trazabilidad?.find((item: any) => ((String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'APROBADA');
      control.setValue('observaciones', actividad?.observacion);
      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    else if (seccion && (seccion === TABS_INDEX.VERDETALLESOLICITUD || seccion === TABS_INDEX.VERDETALLESOLICITUDVERIFICACION) &&
      trazabilidad?.some((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'APROBADA') &&
      !trazabilidad?.some((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'PENDIENTE')) {

      actividad = trazabilidad?.find((item: any) => (String(item?.descripcionTarea).includes('Radicación') || String(item?.descripcionTarea).includes('Radicacion')) && item?.descripcionEstadoSolicitud === 'APROBADA');
      control.setValue('observaciones', actividad?.observacion);
      setMostrarObservacionSoloLectura(true);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    else if (
      seccion &&
      (
        seccion === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
        seccion === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
        seccion === TABS_INDEX.CARGUEDOCUMENTOS ||
        seccion === TABS_INDEX.CARGUEDOCUMENTOSEGURIDADOCUMENTAL ||
        seccion === TABS_INDEX.CARGUERETIROPARCIAL
      ) &&
      trazabilidad?.some((item: any) =>
        String(item?.descripcionTarea).includes('Cargue') &&
        (
          item?.descripcionEstadoSolicitud === 'PENDIENTE' ||
          item?.descripcionEstadoSolicitud === 'INCONSISTENTE'
        )
      )
    ) {
      actividad = trazabilidad?.find((item: any) =>
        String(item?.descripcionTarea).includes('Cargue') &&
        (
          item?.descripcionEstadoSolicitud === 'PENDIENTE' ||
          item?.descripcionEstadoSolicitud === 'INCONSISTENTE'
        )
      );

      control.setValue(
        'observaciones',
        String(actividad?.observacion === null ? '' : actividad?.observacion),
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        }
      );

      setMostrarObservacionSoloLectura(
        actividad?.descripcionEstadoSolicitud === 'INCONSISTENTE'
      );

      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    else if (seccion && (seccion === TABS_INDEX.CARGUEDOCUMENTOSV14 || seccion === TABS_INDEX.CARGUEDOCUMENTOSV8 || seccion === TABS_INDEX.CARGUEDOCUMENTOS || seccion === TABS_INDEX.CARGUEDOCUMENTOSEGURIDADOCUMENTAL || seccion === TABS_INDEX.CARGUERETIROPARCIAL)
      && trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Cargue') && item?.descripcionEstadoSolicitud === 'APROBADA')) {

      actividad = trazabilidad?.find((item: any) => String(item?.descripcionTarea).includes('Cargue') && item?.descripcionEstadoSolicitud === 'APROBADA');
      control.setValue('observaciones', actividad?.observacion);
      setMostrarObservacionSoloLectura(true);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }

    else if (
      (
        seccion === TABS_INDEX.INCONSISTENCIAS ||
        seccion === TABS_INDEX.SEGURIDADDOCUMENTAL ||
        seccion === TABS_INDEX.VERDETALLESOLICITUD
      ) &&
      (
        esTareaGrupo2Actual ||
        mostrarObservacionGrupo1EnGrupo2
      )
    ) {
      const trazabilidadOrdenadaDesc = [...trazabilidadSegura].sort(
        (a: any, b: any) =>
          new Date(b.fechaCreacion).getTime() - new Date(a.fechaCreacion).getTime()
      );

      const observacionGrupo1 = trazabilidadOrdenadaDesc.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('seguridad documental') &&
        normalizar(item?.descripcionTarea).includes('grupo 1') &&
        String(item?.observacion || '').trim() !== ''
      );

      actividad = trazabilidadOrdenadaDesc.find((item: any) =>
        normalizar(item?.descripcionTarea).includes('seguridad documental') &&
        normalizar(item?.descripcionTarea).includes('grupo 2') &&
        estadosActivos.includes(
          String(item?.descripcionEstadoSolicitud || '').toUpperCase()
        )
      );

      control.setValue(
        'observacionesGrupo1',
        observacionGrupo1?.observacion ? String(observacionGrupo1.observacion) : '',
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        }
      );

      control.setValue(
        'observaciones',
        actividad?.observacion ? String(actividad.observacion) : '',
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        }
      );

      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(true);
      setLoading(false);
    }
    else if (
      seccion === TABS_INDEX.TRAMITEDEPAGO
    ) {
      const actividadTramitePago = [...trazabilidadSegura]
        .sort(
          (a: any, b: any) =>
            new Date(b?.fechaCreacion || 0).getTime() -
            new Date(a?.fechaCreacion || 0).getTime(),
        )
        .find((item: any) =>
          normalizar(item?.descripcionTarea).includes(
            'tramite de pago retiro parcial',
          ),
        );

      control.setValue(
        'observaciones',
        String(actividadTramitePago?.observacion ?? ''),
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        },
      );

      setMostrarObservacionSoloLectura(
        Boolean(actividadTramitePago),
      );

      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }
    // else if ((seccion && seccion === TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL || seccion === TABS_INDEX.VERIFICACIONESDECUENTA) &&
    //   trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual')) &&
    //   !trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación novedades')) &&
    //   !trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación embargos'))) {

    //   actividad = trazabilidad?.find((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual'));
    //   control.setValue('observaciones', String(actividad?.observacion === null ? '' : actividad?.observacion));
    //   if (trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual') && item?.descripcionEstadoSolicitud === 'APROBADA')) {
    //     setMostrarObservacionSoloLectura(true);
    //   }
    //   if (!trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual') && item?.descripcionEstadoSolicitud === 'APROBADA') &&
    //     trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual') && item?.descripcionEstadoSolicitud !== 'APROBADA')) {
    //     setMostrarObservacionSoloLectura(false);
    //   }
    //   setMostrarObservacionGrupo2(false);
    //   setLoading(false);
    // }
    // else if ((seccion && seccion === TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL || seccion === TABS_INDEX.VERIFICACIONESDECUENTA) &&
    //   !trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual')) &&
    //   trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación novedades')) &&
    //   !trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación embargos'))) {

    //   actividad = trazabilidad?.find((item: any) => String(item?.descripcionTarea).includes('Verificación novedades'));
    //   control.setValue('observaciones', String(actividad?.observacion === null ? '' : actividad?.observacion));
    //   setMostrarObservacionGrupo2(false);
    //   setLoading(false);
    // }
    // else if ((seccion && seccion === TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL || seccion === TABS_INDEX.VERIFICACIONESDECUENTA) &&
    //   trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual')) &&
    //   trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación novedades')) &&
    //   !trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('Verificación embargos'))) {

    //   const observacionNovedades = trazabilidad?.find((item: any) => String(item?.descripcionTarea).includes('Verificación novedades'));
    //   control.setValue('observacionesGrupo1', observacionNovedades?.observacion);
    //   actividad = trazabilidad?.find((item: any) => String(item?.descripcionTarea).includes('Verificación cuenta individual'));
    //   control.setValue('observaciones', String(actividad?.observacion === null ? '' : actividad?.observacion));
    //   setMostrarObservacionGrupo2(true);
    //   setLoading(false);
    // }

    // else if (
    //   esSeccionVerificacionesCuenta() &&
    //   existeActividadPorTipo(esActividadCuentaIndividual) &&
    //   !existeActividadPorTipo(esActividadEmbargos)
    // ) {
    //   const actividadCuentaIndividual = obtenerUltimaActividadPorTipo(
    //     esActividadCuentaIndividual
    //   );

    //   const existeNovedades = existeActividadPorTipo(
    //     esActividadNovedades
    //   );

    //   const cuentaIndividualAprobada =
    //     normalizar(
    //       actividadCuentaIndividual?.descripcionEstadoSolicitud,
    //     ) === 'aprobada';

    //   const tramitePagoAprobado =
    //     tramitePagoRetiroParcialEstaAprobado();

    //   control.setValue(
    //     'observaciones',
    //     obtenerObservacionUltimaActividadPorTipo(
    //       esActividadCuentaIndividual
    //     ),
    //     {
    //       shouldDirty: false,
    //       shouldTouch: false,
    //       shouldValidate: false,
    //     }
    //   );

    //   if (existeNovedades) {
    //     control.setValue(
    //       'observacionesGrupo1',
    //       obtenerObservacionUltimaActividadPorTipo(
    //         esActividadNovedades
    //       ),
    //       {
    //         shouldDirty: false,
    //         shouldTouch: false,
    //         shouldValidate: false,
    //       }
    //     );

    //     setMostrarObservacionGrupo2(true);
    //   } else {
    //     control.setValue('observacionesGrupo1', '', {
    //       shouldDirty: false,
    //       shouldTouch: false,
    //       shouldValidate: false,
    //     });

    //     setMostrarObservacionGrupo2(false);
    //   }

    //   setMostrarObservacionSoloLectura(
    //     cuentaIndividualAprobada ||
    //     llegoATramitePagoRetiroParcial(),
    //   );
    //   setLoading(false);
    // }

    else if (
      esSeccionVerificacionesCuenta() &&
      existeActividadPorTipo(esActividadCuentaIndividual)
    ) {
      const actividadCuentaIndividual =
        obtenerUltimaActividadPorTipo(
          esActividadCuentaIndividual
        );

      const existeNovedades =
        existeActividadPorTipo(
          esActividadNovedades
        );

      const cuentaIndividualAprobada =
        normalizar(
          actividadCuentaIndividual
            ?.descripcionEstadoSolicitud
        ) === 'aprobada';

      control.setValue(
        'observaciones',
        obtenerObservacionPorTipo(
          esActividadCuentaIndividual
        ),
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        }
      );

      if (existeNovedades) {
        control.setValue(
          'observacionesGrupo1',
          obtenerObservacionPorTipo(
            esActividadNovedades
          ),
          {
            shouldDirty: false,
            shouldTouch: false,
            shouldValidate: false,
          }
        );

        setMostrarObservacionGrupo2(true);
      } else {
        control.setValue(
          'observacionesGrupo1',
          '',
          {
            shouldDirty: false,
            shouldTouch: false,
            shouldValidate: false,
          }
        );

        setMostrarObservacionGrupo2(false);
      }

      setMostrarObservacionSoloLectura(
        cuentaIndividualAprobada ||
        llegoATramitePagoRetiroParcial()
      );

      setLoading(false);
    }

    else if (
      seccion ===
      TABS_INDEX.VERIFICACIONLISTASRESTRICTIVAS
    ) {
      const actividadListasRestrictivas =
        trazabilidadOrdenadaDescGeneral.find(
          (item: any) =>
            normalizar(
              item?.descripcionTarea,
            ).includes(
              'verificacion de listas restrictivas',
            ),
        );

      const observacionTrazabilidad =
        String(
          actividadListasRestrictivas
            ?.observacion ?? '',
        ).trim();

      const observacionFormulario =
        String(
          control.getValues(
            'observaciones',
          ) ?? '',
        ).trim();

      const observacionAMostrar =
        observacionTrazabilidad ||
        observacionFormulario;

      control.setValue(
        'observaciones',
        observacionAMostrar,
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        },
      );

      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }

    else {
      control.setValue(
        'observaciones',
        '',
        {
          shouldDirty: false,
          shouldTouch: false,
          shouldValidate: false,
        },
      );

      setMostrarObservacionSoloLectura(false);
      setMostrarObservacionGrupo2(false);
      setLoading(false);
    }

    // else {
    //   control.setValue('observaciones', '');
    //   setMostrarObservacionSoloLectura(false);
    //   setMostrarObservacionGrupo2(false);
    //   setLoading(false);
    // }

  }

  useEffect(() => {
    if (!Array.isArray(trazabilidad)) {
      return;
    }
  
    validaciones();
  }, [
    seccion,
    trazabilidad,
    nombreTareaActual,
    forzarObservacionEditable,
    mostrarObservacionGrupo1EnGrupo2,
  ]);


  const tituloObservacionGrupo1 = () => {
    let titulo = '';

    if (
      trazabilidad?.some((item: any) =>
        normalizar(item?.descripcionTarea).includes('grupo 2')
      ) &&
      (
        seccion === TABS_INDEX.INCONSISTENCIAS ||
        seccion === TABS_INDEX.SEGURIDADDOCUMENTAL
      )
    ) {
      titulo = 'Observaciones Grupo 1';
    } else if (
      trazabilidad?.some((item: any) =>
        normalizar(item?.descripcionTarea).includes('verificacion')
      ) &&
      (
        seccion === TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL ||
        seccion === TABS_INDEX.VERIFICACIONESDECUENTA
      )
    ) {
      titulo = 'Observaciones Verificación de Novedades en los Datos';
    }

    return titulo;
  };


  const tituloObservacionGrupo2 = () => {
    let titulo = '';

    if (
      trazabilidad?.some((item: any) =>
        normalizar(item?.descripcionTarea).includes('grupo 2')
      ) &&
      (
        seccion === TABS_INDEX.INCONSISTENCIAS ||
        seccion === TABS_INDEX.SEGURIDADDOCUMENTAL
      )
    ) {
      titulo = 'Observaciones Grupo 2';
    } else if (
      esSeccionVerificacionesCuenta() &&
      existeActividadPorTipo(esActividadCuentaIndividual)
    ) {
      titulo = 'Observaciones Verificación de Cuenta Individual';
    }

    return titulo;
  };

  const labelObservacionGrupo2 = () => {
    let label = '';
    if ((trazabilidad?.some((item: any) => String(item?.descripcionTarea).includes('grupo 2') && item?.descripcionEstadoSolicitud === 'APROBADA'))
      && seccion && seccion === TABS_INDEX.SEGURIDADDOCUMENTAL) {
      label = '';
    }
    else if (
      esSeccionVerificacionesCuenta() &&
      cuentaIndividualEstaAprobada()
    ) {
      label = '';
    }
    else {
      label = 'Si tiene observaciones adicionales ingreselas';
    }
    return label;
  }

  const isReadOnlyGrupo2 = () => {
    if (mostrarObservacionGrupo1EnGrupo2) {
      return false;
    }

    if (
      trazabilidad?.some((item: any) =>
        normalizarTexto(item?.descripcionTarea).includes('grupo 2') &&
        String(item?.descripcionEstadoSolicitud || '').toUpperCase() === 'APROBADA'
      ) &&
      seccion === TABS_INDEX.SEGURIDADDOCUMENTAL
    ) {
      return true;
    }


    if (
      cuentaIndividualEstaAprobada() &&
      (
        esSeccionVerificacionesCuenta() ||
        seccion === TABS_INDEX.INCONSISTENCIAS
      )
    ) {
      return true;
    }

    return false;
  };

  if (loading) {
    return <CHLoadingProcess />;
  }

  const normalizarTexto = (texto: any) =>
    String(texto || '')
      .trim()
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '');

  const tareaPendienteTrazabilidad = trazabilidad?.find((item: any) =>
    normalizarTexto(item?.descripcionEstadoSolicitud) === 'pendiente'
  );

  const estaEnVerificacionPorTrazabilidad =
    normalizarTexto(tareaPendienteTrazabilidad?.descripcionTarea)
      .startsWith('verificacion');


  const esSeccionCargueActual =
    seccion === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
    seccion === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
    seccion === TABS_INDEX.CARGUEDOCUMENTOS ||
    seccion === TABS_INDEX.CARGUEDOCUMENTOSEGURIDADOCUMENTAL ||
    seccion === TABS_INDEX.CARGUERETIROPARCIAL;

  const mostrarObservacionCargueSoloLectura =
    soloLecturaVerificacion && esSeccionCargueActual;

  const mostrarSoloLecturaVerificacionReal =
    !mostrarObservacionGrupo1EnGrupo2 &&
    !esSeccionCargueActual &&
    !esSeccionListasRestrictivas() &&
    (
      soloLecturaVerificacion ||
      estaEnVerificacionPorTrazabilidad
    );

  const obtenerObservacionGrupo = (grupo: number) => {
    const actividad = [...(trazabilidad || [])]
      .reverse()
      .find((item: any) =>
        normalizarTexto(item?.descripcionTarea).includes(`seguridad documental grupo ${grupo}`) &&
        item?.observacion !== null &&
        item?.observacion !== undefined &&
        String(item?.observacion).trim() !== ''
      );

    return actividad?.observacion || '';
  };

  const observacionGrupo1 = obtenerObservacionGrupo(1);
  const observacionGrupo2 = obtenerObservacionGrupo(2);

  if (mostrarObservacionCargueSoloLectura) {
    return (
      <CHPaper title="Observaciones">
        <Grid container>
          <Grid className={classes.column} item xs={12}>
            <Controller
              name="observaciones"
              control={control.control}
              render={({ field: { value } }) => (
                <CHTextField
                  InputLabelProps={{ shrink: true }}
                  isReadOnly
                  value={value || ''}
                  multiline
                  minRows={2}
                  inputProps={{ maxLength: 300 }}
                />
              )}
            />
          </Grid>
        </Grid>
      </CHPaper>
    );
  }

  if (
    mostrarSoloLecturaVerificacionReal &&
    !esSeccionVerificacionesCuenta() &&
    seccion !== TABS_INDEX.VERDETALLESOLICITUD) {
    return (
      <>
        <CHPaper title={tituloObservacionGrupo1()}>
          <Grid container>
            <Grid className={classes.column} item xs={12}>
              <CHTextField
                InputLabelProps={{ shrink: true }}
                isReadOnly
                value={observacionGrupo1}
                multiline
                minRows={2}
                inputProps={{ maxLength: 300 }}
              />
            </Grid>
          </Grid>
        </CHPaper>

        <CHPaper title={tituloObservacionGrupo2()}>
          <Grid container>
            <Grid className={classes.column} item xs={12}>
              <CHTextField
                InputLabelProps={{ shrink: true }}
                isReadOnly
                value={observacionGrupo2}
                multiline
                minRows={2}
                inputProps={{ maxLength: 300 }}
              />
            </Grid>
          </Grid>
        </CHPaper>
      </>
    );
  }

  const tituloObservacionUnica = () => {

    if (esSeccionListasRestrictivas()) {
      return 'Observación verificación de listas restrictivas';
    }

    if (!esSeccionVerificacionesCuenta()) {
      return 'Observaciones';
    }

    if (existeActividadPorTipo(esActividadCuentaIndividual)) {
      return 'Observaciones Verificación de Cuenta Individual';
    }

    if (existeActividadPorTipo(esActividadNovedades)) {
      return 'Observaciones Verificación de Novedades en los Datos';
    }

    return 'Observaciones';
  };


  return (
    <>

      {!mostrarObservacionGrupo2 && (
        <CHPaper title={tituloObservacionUnica()}>
          <Grid container>
            <Grid className={classes.column} item xs={12}>
              <Controller
                name='observaciones'
                control={control.control}
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
                    isReadOnly={
                      soloLecturaVerificacion ||
                      (
                        forzarObservacionEditable
                          ? false
                          : mostrarObservacionSoloLectura
                      )
                    }
                    InputLabelProps={{ shrink: true }}
                    label={!mostrarObservacionSoloLectura ? 'Si tiene observaciones adicionales ingreselas' : ''}
                    value={value}
                    required
                    multiline
                    minRows={2}
                    onChange={onChange}
                    inputProps={{ maxLength: 300 }}
                  />
                )}
              />
            </Grid>
          </Grid>
        </CHPaper>
      )}

      {mostrarObservacionGrupo2 && (
        <>
          <CHPaper title={tituloObservacionGrupo1()}>
            <Grid container>
              <Grid className={classes.column} item xs={12}>
                <Controller
                  name="observacionesGrupo1"
                  control={control.control}
                  // rules={{
                  //   required: 'Este campo es requerido.',
                  // }}
                  render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHTextField
                      error={!!error}
                      helperText={error ? error.message : null}
                      InputLabelProps={{ shrink: true }}
                      isReadOnly
                      value={value}
                      required
                      multiline
                      minRows={2}
                      onChange={onChange}
                      inputProps={{ maxLength: 300 }}
                    />
                  )}
                />
              </Grid>
            </Grid>
          </CHPaper>

          <CHPaper title={tituloObservacionGrupo2()}>
            <Grid container>
              <Grid className={classes.column} item xs={12}>
                <Controller
                  name='observaciones'
                  control={control.control}
                  rules={{
                    required: 'Este campo es requerido.',
                  }}
                  render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHTextField
                      error={!!error}
                      helperText={error ? error.message : null}
                      InputLabelProps={{ shrink: true }}
                      label={labelObservacionGrupo2()}
                      isReadOnly={mostrarObservacionGrupo1EnGrupo2 ? false : isReadOnlyGrupo2()}
                      value={value}
                      required
                      multiline
                      minRows={2}
                      onChange={onChange}
                      inputProps={{ maxLength: 300 }}
                    />
                  )}
                />
              </Grid>
            </Grid>
          </CHPaper>
        </>
      )}
    </>
  );
}

export default ObservacionesSolicitud;