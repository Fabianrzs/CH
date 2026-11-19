import React, { useEffect } from 'react';
import { Grid, makeStyles, createStyles, Button, Dialog, DialogContent, DialogTitle, MenuItem, Typography, TextField } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { BloqueosEntidadesEducativas, BloqueosEntidadesEducativasInitial } from '../../../types/educational.entities.type';
import AuthService from '../../../services/auth.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
  createStyles({
		button: {
			width: '120px',
			marginLeft: '.5rem',
			borderRadius: '10%',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		restoreButton: {
			width: '250px',
			marginLeft: '.5rem',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
    title: {
      color: '#42b7ef',
      textAlign: 'center',
    },
    table: {
      color: '#f1c3b90ff',
    }
  }),
);

function LockEntities({
  idEntidad,
  isOpen,
  tiposOperacion,
  causalesBloqueo,
  tipoOperacion,
  onClose,
  obtenerCausales,
  bloquearDesbloquearEntidad,
  getEducationalEntitiesDetail
}: {
  idEntidad: string;
  isOpen: boolean;
  tiposOperacion: any;
  causalesBloqueo: any;
  tipoOperacion: string;
  onClose: (value: any) => void;
  obtenerCausales: (nombreCatalogo: string) => void;
  bloquearDesbloquearEntidad: (data: BloqueosEntidadesEducativas, lock: boolean) => void;
  getEducationalEntitiesDetail:(idEntidad:string) => void;
}) {
  const classes = useStyles();
  const handleClose = () => {
    reset();
    const value: any = {};
    onClose(value);
    getEducationalEntitiesDetail(idEntidad);
  };
  const { handleSubmit, control, watch, reset } = useForm<BloqueosEntidadesEducativas>({
    defaultValues: BloqueosEntidadesEducativasInitial,
  });
  useEffect(() => {
    if (isOpen) {
      obtenerCausales(tipoOperacion);
    }
  }, [isOpen])

  const updateData = (data: BloqueosEntidadesEducativas) => {
    const user = AuthService.getUserInfo();
    const sendData: BloqueosEntidadesEducativas = {
      idEntidad: idEntidad,
      funcionarioBloqueo: user.USR_ID,
      idCausalBloqueo: data.idCausalBloqueo,
      observaciones: data.observaciones
    };
    bloquearDesbloquearEntidad(sendData, tipoOperacion === 'BLOQUEO');
    handleClose();
  }

  return (
    <Dialog className={classes.table}
      fullWidth
      maxWidth="md"
      open={isOpen}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle className={classes.title} id="form-dialog-title">BLOQUEOS Y DESBLOQUEOS</DialogTitle>
      <DialogContent>
        <Grid item container>
          <Grid container md={12}>
            <Grid className="item-containerModal" md={5}>
              <Grid className='subcontainerModal'>
                <Controller
                  name="idTipoOperacion"
                  control={control}
                  rules={{
                    validate: () => tipoOperacion === '' ? 'Este campo es requerido' : true,
                  }}
                  render={({ fieldState: { error } }) => (
                    <CHSelect
                      defaultValue=""
                      error={!!error}
                      helperText={error ? error.message : null}
                      label="Tipo de operación *"
                      value={tipoOperacion}
                      disabled
                    >
                      {tiposOperacion.map(
                        (option: any) => (
                          <MenuItem
                            key={option.descripcion}
                            value={option.descripcion}
                          >
                            {option.descripcion}
                          </MenuItem>
                        ),
                      )}
                    </CHSelect>
                  )}
                />
              </Grid>
            </Grid>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <Controller
                  name="idCausalBloqueo"
                  control={control}
                  rules={{
                    validate: () => watch('idCausalBloqueo') === 0 ? 'Este campo es requerido' : true,
                  }}
                  render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHSelect
                      defaultValue=""
                      error={!!error}
                      helperText={error ? error.message : null}
                      label="Causal de operación *"
                      onChange={onChange}
                      value={value}
                      required
                    >
                      {causalesBloqueo.map(
                        (option: any) => (
                          <MenuItem
                            key={option.idValor}
                            value={option.idValor}
                          >
                            {option.descripcion}
                          </MenuItem>
                        ),
                      )}
                    </CHSelect>
                  )}
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid container md={11}>
          <Grid className="item-containerModal" md={11}>
          <Grid className="subcontainerModal">
            <Grid>
            <Controller
                  name="observaciones"
                  control={control}
                  rules={{
                      required: 'Este campo es requerido.',
                      maxLength: {
                        value: 300,
                        message: "Máximo 300 caracteres permitidos."
                    }
									}}
                  render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHTextField
                        InputLabelProps={{ shrink: true }}
                        error={!!error}
                        helperText={error ? error.message : null}
                        id="outlined-multiline-flexible"
                        name="observaciones"
                        label="Obsevaciones *"
                        multiline
                        maxRows={5} 
                        inputProps={{
                          maxLength: 300
                        }}
                        onChange={onChange}                                   
                    />
                  )}
                />
                  
            </Grid>
          </Grid>
          </Grid>
            
          </Grid>
        </Grid>
        <div className="buttons">
          <Button
            className={classes.button}
            type="button"
            onClick={handleClose}
          >
            REGRESAR
          </Button>
          <Button
            className={classes.restoreButton}
            onClick={handleSubmit(updateData)}
            type="submit">
            ACTUALIZAR
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}

export default LockEntities;


