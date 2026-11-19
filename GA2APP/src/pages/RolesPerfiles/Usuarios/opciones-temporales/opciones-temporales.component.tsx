import React, { useEffect, useState } from 'react';
import {
  Button,
  createStyles,
  Dialog,
  DialogContent,
  DialogTitle,
  Grid,
  makeStyles,
  MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import moment from 'moment';
import CHDatePickerV2 from '../../../../components/ch-datepicker/CHDatepickerV2';
import CHSelectV2 from '../../../../components/ch-select/CHSelectV2';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import { AppState } from '../../../../store/rootReducer';
import { startGetAllOpciones } from '../../../../actions/grupoUsers.action';
import { GrupoOpcionesState } from '../../../../reducers/grupo-opciones.reducer';
import GrupoUsers from '../../../../services/api-grupoUsuarios.service';
import { GUID_DEFAULT } from '../../../../constants/constants';

const useStyles = makeStyles(() =>
  createStyles({
    button: {
      backgroundColor: '#42b7ef',
      color: '#fff',
      width: '12%',
      marginLeft: '69%',
    },
    button2: {
      backgroundColor: '#fec917',
      color: '#1c3f8b',
      width: '12%',
      marginLeft: '1%',
    },

    icon: {
      padding: '10px',
      textAlign: 'inherit',
      color: '#1C3F8B',
    },

    title: {
      color: '#42b7ef',
      textAlign: 'center',
    },
  }),
);

const api = GrupoUsers()

function OpcionesTemporales({
  data,
  open,
  grupoOpcionesState,
  crear,
  idUsuario,
  onClose,
  onSubmit
}: {
  data: any;
  open: boolean;
  grupoOpcionesState: GrupoOpcionesState;
  crear: boolean; 
  idUsuario: string;
  onClose: () => void;
  onSubmit: (data:any) => void;

}): any {
  const classes = useStyles();
  const handleClose = () => {
    onClose();
  };

  const { control, handleSubmit, setValue, reset, watch } = useForm({
    defaultValues: {
      idOpcionTemporalUsuario: 0,
      idOpcion: 0,
      idUsuario: GUID_DEFAULT,
      fechaDesdeOpcionTemporalUsuario: new Date(),
      fechaHastaOpcionTemporalUsuario: new Date(),
    },
  });

  const [hasLoadedFields, setHasLoadedFields] = useState(false);
  if (!hasLoadedFields && data != null && !crear) {
    setHasLoadedFields(true);

    setValue('idOpcionTemporalUsuario', data.idOpcionTemporalUsuario);
    setValue('idOpcion', data.idOpcion);
    setValue('idUsuario', idUsuario !== "" && idUsuario != null ? GUID_DEFAULT : idUsuario);
    setValue(
      'fechaDesdeOpcionTemporalUsuario',
      data.fechaDesdeOpcionTemporalUsuario,
    );
    setValue(
      'fechaHastaOpcionTemporalUsuario',
      data.fechaHastaOpcionTemporalUsuario,
    );
  }

  if(!hasLoadedFields && crear){
    reset()
    setHasLoadedFields(true);
    setValue('idUsuario', idUsuario);
  }

  useEffect(() => {
    setHasLoadedFields(!open);
  }, [open]);

  const AgregarOpciones = (data: any) => {
    if(idUsuario != null && idUsuario !== ""){
      if(data.idOpcionTemporalUsuario != null && data.idOpcionTemporalUsuario !== 0){
          api.ActualizarOpcionesTemporales(data)
          .then(respuesta => {
            if (respuesta.isSuccess) {
              toast('Se actualizó con éxito el registro')
              onSubmit(respuesta.data)
            }
          });
      } else {
          api.InsertarOpcionesTemporales(data)
          .then(respuesta => {
            if (respuesta.isSuccess) {
              toast('Se agregó con éxito el registro')
              onSubmit(respuesta.data)
            }
          });
      }
    } else {
      data.codigoOpcionTemporalUsuario = grupoOpcionesState.allOpciones.find(x => x.id === data.idOpcion)?.codigo
      data.nombreOpcionTemporalUsuario = grupoOpcionesState.allOpciones.find(x => x.id === data.idOpcion)?.nombre 
      data.descripcionOpcionTemporalUsuario = grupoOpcionesState.allOpciones.find(x => x.id === data.idOpcion)?.descripcion
      data.idUsuario = GUID_DEFAULT
      onSubmit(data)
    }
  }

  const validateOpcion = (data: any) => {
    if(data === 0){
      return 'Este campo es requerido'
    }
    return true
  }

  const validateFechaHasta = (data: Date) => {
    const date = new Date().toDateString()
    const dateControl = data.toDateString();
    if(data == null){
      return 'Este campo es requerido'
    }
    if(!moment(data).isValid()){
      return 'Ingrese una fecha válida'
    }
    if(new Date(dateControl) < new Date(date)){
      return 'La fecha hasta no puede ser menor a la fecha del sistema'
    }
    if(new Date(dateControl) < new Date(watch('fechaDesdeOpcionTemporalUsuario').toDateString())){
      return 'La fecha hasta no puede ser menor a la fecha desde'
    }
    return true
  }

  const validateFechaDesde = (data: Date) => {
    const date = new Date().toDateString()
    const dateControl = data.toDateString();

    if(data == null){
      return 'Este campo es requerido'
    }
    if(!moment(data).isValid()){
      return 'Ingrese una fecha válida'
    }
    if(new Date(dateControl) < new Date(date)){
      return 'La fecha desde no puede ser menor a la fecha del sistema'
    }
    if (new Date(dateControl) > new Date(watch('fechaHastaOpcionTemporalUsuario').toDateString())) {
      return 'La fecha desde no puede ser mayor a la fecha hasta'
    } 
    return true
  }

  return (
    <Dialog
      fullWidth
      maxWidth="md"
      open={open}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle className={classes.title} id="form-dialog-title">
        {data != null ? 'EDITAR OPCION TEMPORAL' : 'AGREGAR OPCION TEMPORAL'}
      </DialogTitle>
      <DialogContent>
        <Grid container>
          <Grid className="item-containerModal" md={5}>
            <Grid className="subcontainerModal">
              <Controller
                rules={{
                  validate: validateOpcion
                }}
                name="idOpcion"
                control={control}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHSelectV2
                    defaultValue=""
                    label="Opción*"
                    onChange={onChange}
                    value={value}
                    error={!!error}
                    helperText={error ? error.message : null}
                  >
                    {grupoOpcionesState?.allOpciones?.map((option: any) => (
                      <MenuItem key={option.id} value={option.id}>
                        {option.nombre}
                      </MenuItem>
                    ))}
                  </CHSelectV2>
                )}
              />
            </Grid>
          </Grid>
          <Grid className="item-containerModal" md={5}>
            <Grid className="subcontainerModal">
              <Controller
                rules={{
                  validate: validateFechaDesde

                }}
                name="fechaDesdeOpcionTemporalUsuario"
                control={control}
                render={({ field: { onChange, value }, fieldState: {error} }) => (
                  <CHDatePickerV2
                    error={!!error}
                    helperText={error ? error.message : null}
                    label="Fecha Inicial Permiso*"
                    onChange={onChange}
                    value={value}
                    invalidDateMessage="Fecha inválida"
                  />
                )}
              />
            </Grid>
          </Grid>
          <Grid className="item-containerModal" md={5}>
            <Grid className="subcontainerModal">
              <Controller
                rules={{
                  validate: validateFechaHasta

                }}
                name="fechaHastaOpcionTemporalUsuario"
                control={control}
                render={({ field: { onChange, value }, fieldState: {error} }) => (
                  <CHDatePickerV2
                    error={!!error}
                    helperText={error ? error.message : null}
                    label="Fecha Final Permiso*"
                    onChange={onChange}
                    value={value}
                    invalidDateMessage="Fecha inválida"
                  />
                )}
              />
            </Grid>
          </Grid>
        </Grid>
        <Grid>
          <Button
            className={classes.button}
            type="button"
            onClick={handleClose}
          >
            REGRESAR
          </Button>
          <Button
            className={classes.button2}
            onClick={handleSubmit(AgregarOpciones)}
            type="submit"
          >
            AGREGAR
          </Button>
        </Grid>
      </DialogContent>
    </Dialog>
  );
}

const mapStateToProps = (state: AppState) => ({
  grupoOpcionesState: state.grupoOpciones,
});

const mapDispatchToProps = {
  startGetAllOpciones,
};

export default connect(mapStateToProps, mapDispatchToProps)(OpcionesTemporales);
