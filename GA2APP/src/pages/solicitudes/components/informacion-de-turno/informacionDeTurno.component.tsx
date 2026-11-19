import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { createStyles, Grid, MenuItem, RadioGroup, Radio, FormLabel, makeStyles, MuiThemeProvider, createTheme } from '@material-ui/core';
import DateFnsUtils from '@date-io/date-fns';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { InfoTurnoDto, sucursalesValor, recepcionesValor, CatalogData } from '../../../../types/catalog.type';
import { ConyugueModel } from '../../../../models/conyuge-solicitudes.model';
import { InformacionTurnosModel } from '../../../../models/Informacion-turno.model';
import ApiCatalog from '../../../../services/api-catalog.service';

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

const InformacionDeTurno = ({
  formulario,
  informacionDeTurno,
  responseSolicitud,
  turnoForm,
  campoSoloLectura
}: {
  formulario: UseFormReturn<any>,
  informacionDeTurno: CatalogData[];
  responseSolicitud: any
  turnoForm: InformacionTurnosModel;
  campoSoloLectura: boolean;
}) => {
  const classes = useStyles();
  const [sucursalesTurno, setSucursalesTurnos] = useState<any[]>([]);
  const [recepcionesTurno, setRecepcionesTurnos] = useState<any[]>([]);
  const [tipoTurno, settipoTurnos] = useState<any[]>([]);
  const [disabledtipoDeRecepcion, setDisabledtipoDeRecepcion] = useState<boolean>(false);
  const [disabledradPreTramite, setDisabledradPreTramite] = useState<boolean>(false);
  const [disablednumeroMemorando, setDisablednumeroMemorando] = useState<boolean>(false);
  const [disabledtipoTurno, setDisabledtipoTurno] = useState<boolean>(false);
  const [disablednumeroTurno, setDisablednumeroTurno] = useState<boolean>(false);
  const [disabledsucursal, setDisabledsucursal] = useState<boolean>(false);
  const [disabledfechaMemorando, setDisabledfechaMemorando] = useState<boolean>(false);
  const [requiereMemorando, setrequiereMemorando] = useState<boolean>(false);

  const informacionTurnov = responseSolicitud?.informacionTurnoDto;

  const esRadicacionDevueltaPorRechazo =
    responseSolicitud?.soL_RECHAZADA === '1' &&
    String(responseSolicitud?.nombreTarea || '')
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .includes('radicacion');


  useEffect(() => {
    consultarSucursalesTurnos();
    consultarRecepcionesTurnos();
    consultarTipoTurnos();
  }, []);

  useEffect(() => {
    bloquearCamporTurnosPost();
  }, [
    responseSolicitud?.informacionTurnoDto,
    responseSolicitud?.soL_RECHAZADA,
    responseSolicitud?.nombreTarea,
  ]);



  const informacionTurno = useForm({
    defaultValues: {
      tipoDeRecepcion: '',
      radPreTramite: '',
      nombreComision: '',
      tipoTurno: '',
      numeroTurno: '',
      sucursal: '',
    },
  });



  const { control } = useForm();
  const apiCatalog = ApiCatalog();
  const today = new Date();

  const consultarSucursalesTurnos = async () => {
    await apiCatalog.getConsultarInformacionTurno()
      .then(response => {
        if (response.data) {
          setSucursalesTurnos(response.data.sucursales);
        }
      }).catch(() => {
        toast(`Error del sistema`, { type: toast.TYPE.ERROR });
      })
  }

  const consultarRecepcionesTurnos = async () => {
    await apiCatalog.getConsultarInformacionTurno()
      .then(response => {
        if (response.data) {
          setRecepcionesTurnos(response.data.recepciones);
        }
      }).catch(() => {
        toast(`Error del sistema`, { type: toast.TYPE.ERROR });
      })
  }

  const consultarTipoTurnos = async () => {
    await apiCatalog.getConsultarInformacionTurno()
      .then(response => {
        if (response.data) {
          settipoTurnos(response.data.turnos);
        }
      }).catch(() => {
        toast(`Error del sistema`, { type: toast.TYPE.ERROR });
      })
  }

  const aplicarReglasTipoRecepcion = (
    selectedValue: string,
    limpiarCampos: boolean
  ) => {
    if (selectedValue === 'PRESENCIAL') {
      setDisabledtipoTurno(false);
      setDisabledsucursal(false);
      setDisablednumeroTurno(false);
  
      setDisabledradPreTramite(true);
      setDisablednumeroMemorando(true);
      setDisabledfechaMemorando(true);
  
      setrequiereMemorando(false);
  
      if (limpiarCampos) {
        formulario.setValue('radPreTramite', '');
        formulario.setValue('numeroMemorando', '');
        formulario.setValue('fechaMemorando', null);
      }
  
      return;
    }
  
    if (selectedValue === 'CORREO ELECTRONICO') {
      setDisabledradPreTramite(true);
      setDisablednumeroMemorando(true);
      setDisabledfechaMemorando(true);
      setDisabledtipoTurno(true);
  
      setDisablednumeroTurno(false);
      setDisabledsucursal(false);
  
      setrequiereMemorando(false);
  
      if (limpiarCampos) {
        formulario.setValue('radPreTramite', '');
        formulario.setValue('numeroMemorando', '');
        formulario.setValue('fechaMemorando', null);
        formulario.setValue('tipoTurno', '');
      }
  
      return;
    }
  
    if (selectedValue === 'ENLACES FUERZA') {
      setDisabledradPreTramite(true);
      setDisabledtipoTurno(true);
      setDisablednumeroTurno(true);
  
      setDisablednumeroMemorando(false);
      setDisabledfechaMemorando(false);
      setDisabledsucursal(false);
  
      setrequiereMemorando(true);
  
      if (limpiarCampos) {
        formulario.setValue('radPreTramite', '');
        formulario.setValue('tipoTurno', '');
        formulario.setValue('numeroTurno', '');
      }
  
      return;
    }
  
    if (selectedValue === 'PRETRAMITE') {
      setDisablednumeroMemorando(true);
      setDisabledfechaMemorando(true);
      setDisabledtipoTurno(true);
      setDisablednumeroTurno(true);
  
      setDisabledradPreTramite(false);
      setDisabledsucursal(false);
  
      setrequiereMemorando(false);
  
      if (limpiarCampos) {
        formulario.setValue('numeroMemorando', '');
        formulario.setValue('tipoTurno', '');
        formulario.setValue('numeroTurno', '');
        formulario.setValue('fechaMemorando', null);
      }
  
      return;
    }
  
    if (selectedValue === 'TURNO VIRTUAL') {
      setDisablednumeroMemorando(true);
      setDisabledfechaMemorando(true);
  
      setDisabledradPreTramite(false);
      setDisabledtipoTurno(false);
      setDisablednumeroTurno(false);
      setDisabledsucursal(false);
  
      setrequiereMemorando(false);
  
      if (limpiarCampos) {
        formulario.setValue('numeroMemorando', '');
        formulario.setValue('fechaMemorando', null);
      }
  
      return;
    }
  
    setDisabledradPreTramite(false);
    setDisablednumeroMemorando(false);
    setDisabledtipoTurno(false);
    setDisablednumeroTurno(false);
    setDisabledsucursal(false);
    setDisabledfechaMemorando(false);
    setrequiereMemorando(false);
  };


  const bloquearCamporTurnosPost = () => {
    if (campoSoloLectura) {
      setDisabledtipoDeRecepcion(true);
      setDisabledradPreTramite(true);
      setDisablednumeroMemorando(true);
      setDisabledtipoTurno(true);
      setDisablednumeroTurno(true);
      setDisabledsucursal(true);
      setDisabledfechaMemorando(true);
      return;
    }

    setDisabledtipoDeRecepcion(false);

    const tipoRecepcionId =
      formulario.getValues('tipoDeRecepcion') ||
      informacionTurnov?.tipoDeRecepcion ||
      informacionTurnov?.tipoRecepcionId ||
      '';

    const recepcionSeleccionada =
      recepcionesTurno.find(
        (option: any) =>
          option.riT_ID === tipoRecepcionId
      );

    const descripcionRecepcion =
      recepcionSeleccionada?.riT_DESCRIPCION || '';

    aplicarReglasTipoRecepcion(
      descripcionRecepcion,
      false
    );
  };

 


  const handleTipoRecepcionChange = (
    selectedValue: string
  ) => {
    if (campoSoloLectura) {
      return;
    }
  
    aplicarReglasTipoRecepcion(
      selectedValue,
      true
    );
  };

  useEffect(() => {
    bloquearCamporTurnosPost();
  }, [
    responseSolicitud?.informacionTurnoDto,
    responseSolicitud?.soL_RECHAZADA,
    responseSolicitud?.nombreTarea,
    campoSoloLectura,
    recepcionesTurno,
  ]);



  return (
    <>

      <CHPaper title='Información Turno'>
        <Grid container>

          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name="tipoDeRecepcion"
              control={formulario.control}
              rules={{
                required: 'Este campo es requerido.',
              }}
              render={({ field: { onChange, value }, fieldState: { error } }) => (
                <CHSelect
                  label="Canal de recepcion *"
                  value={value}
                  onChange={(newValue) => {
                    const selectedValue = newValue.target.value as string;

                    onChange(selectedValue);

                    const recepcionSeleccionada = recepcionesTurno.find(
                      option => option.riT_ID === selectedValue,
                    );

                    handleTipoRecepcionChange(
                      recepcionSeleccionada?.riT_DESCRIPCION || '',
                    );
                  }}
                  error={!!error}
                  disabled={disabledtipoDeRecepcion}
                  helperText={
                    error ? error.message : null
                  }
                >
                  {recepcionesTurno?.map(
                    (option: any) => (
                      <MenuItem key={option.riT_ID}
                        value={option.riT_ID}
                      >
                        {option.riT_DESCRIPCION}
                      </MenuItem>
                    ),
                  )}
                </CHSelect>
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='radPreTramite'
              control={formulario.control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (

                <CHTextField
                  error={!!error}
                  disabled={disabledradPreTramite}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Rad Pre Trámite'
                  value={value}
                  // onChange={onChange}
                  onChange={(e) => {
                    const filteredValue = e.target.value.replace(/[^0-9-]/g, '');
                    const truncatedValue = filteredValue.slice(0, 25);
                    onChange(truncatedValue);
                  }}
                />
              )}
            />
          </Grid>

          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='numeroMemorando'
              control={formulario.control}
              render={({ field: { onChange, value }, fieldState: { error } }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  disabled={disablednumeroMemorando}
                  label='Numero de memorando'
                  value={value}
                  onChange={(e) => {
                    const filteredValue = e.target.value.replace(/[^0-9-]/g, '');
                    const truncatedValue = filteredValue.slice(0, 25);
                    onChange(truncatedValue);
                  }}
                  required={requiereMemorando}
                />
              )}
            />
          </Grid>



          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name="fechaMemorando"
              control={formulario.control}
              render={({ field: { onChange, value }, fieldState: { error } }) => (
                <CHDatePicker
                  error={!!error}
                  helperText={error ? error.message : ""}
                  label="Fecha de memorando"
                  onChange={onChange}
                  value={value}
                  disabled={disabledfechaMemorando}
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
              name="tipoTurno"
              control={formulario.control}
              render={({ field: { onChange, value }, fieldState: { error } }) => (
                <CHSelect
                  label={
                    disabledtipoTurno
                      ? 'Tipo Turno'
                      : 'Tipo Turno *'
                  }
                  value={value}
                  onChange={onChange}
                  error={!!error}
                  disabled={disabledtipoTurno}
                  helperText={
                    error ? error.message : null
                  }
                >
                  {tipoTurno?.map(
                    (option: any) => (
                      <MenuItem key={option.tiT_ID}
                        value={option.tiT_ID}
                      >
                        {option.tiT_NOMBRE}
                      </MenuItem>
                    ),
                  )}
                </CHSelect>
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='numeroTurno'
              control={formulario.control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (

                <CHTextField
                  numberMask
                  error={!!error}
                  disabled={disablednumeroTurno}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label='Turno'
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name="sucursal"
              control={formulario.control}
              rules={{
                required: 'Este campo es requerido.',
              }}
              render={({ field: { onChange, value }, fieldState: { error } }) => (
                <CHSelect
                  label="Sucursal *"
                  value={value}
                  disabled={disabledsucursal}
                  onChange={onChange}
                  error={!!error}
                  helperText={
                    error ? error.message : null
                  }
                >
                  {sucursalesTurno?.map(
                    (option: any) => (
                      <MenuItem key={option.siT_ID}
                        value={option.siT_ID}
                      >
                        {option.siT_DESCRICION}
                      </MenuItem>
                    ),
                  )}
                </CHSelect>
              )}
            />
          </Grid>
        </Grid>

      </CHPaper>

    </>


  )
}

export default InformacionDeTurno;