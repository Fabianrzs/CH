import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import {
  Grid,
  makeStyles,
  createStyles,
  Button,
  Dialog,
  DialogContent,
  DialogTitle,
  MenuItem
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { CuentaEntidadesEducativas, Fidecomiso } from '../../../types/educational.entities.type';
import ApiEntidadesEducativas from '../../../services/api-entidadesEducativas.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles(() =>
  createStyles({
    button: {
      width: '120px',
      marginLeft: '.5rem',
      borderRadius: '10%',
      background: '#1C3F8B',
      color: '#FFFFFF',
    },
    button2: {
      width: '120px',
      height: '40px',
      marginLeft: '.5rem',
      marginTop: '.5rem',
      borderRadius: '10%',
      background: '#1C3F8B',
      color: '#FFFFFF',
    },
    container: {
      alignSelf: 'center',
    },

    restoreButton: {
      width: '250px',
      marginLeft: '.5rem',
      background: '#1C3F8B',
      color: '#FFFFFF',
    },
    column: {
      padding: '10px',
      textAlign: 'center',
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
    table: {
      color: '#f1c3b90ff',
    }
  }),
);

function CreateAccount({
  isOpen,
  dataModel,
  catalogState,
  onClose,
  Fidecomiso,
  idEntidad,
  formularioFidecomiso,
  actualizarCuenta
}: {
  isOpen: boolean;
  dataModel: CuentaEntidadesEducativas | null;
  catalogState: CatalogState;
  onClose: (value: any) => void;
  Fidecomiso: (value: any) => void;
  idEntidad: string;
  formularioFidecomiso: UseFormReturn<any>;
  actualizarCuenta: (cuentaEntidad: Fidecomiso, idEntidad: string) => void;
}): any {

  const classes = useStyles();
  const apiEntidadesEducativas = ApiEntidadesEducativas()

  const handleClose = () => {
    const value: any = {};
    onClose(value);
  };

  const [title, setTitle] = useState<string>('');
  const [textButton, setTextButton] = useState<string>('');
  const user = AuthService.getUserInfo();

  useEffect(() => {
    if (dataModel !== null) {
      setTitle('ACTUALIZAR  FIDEICOMISO');
      setTextButton('ACTUALIZAR  FIDEICOMISO');
    } else {
      setTitle('AGREGAR  FIDEICOMISO');
      setTextButton('AGREGAR  FIDEICOMISO');
      formularioFidecomiso.reset();
    }

  }, [dataModel]);

  const SubmitFidecomiso = (data: Fidecomiso) => {
    data.entidad_id = idEntidad;
    data.ENE_FIDEICOMISO_ID = null;
    data.usr_id = null;
    apiEntidadesEducativas
      .AddFidecomiso(data, idEntidad)
      .then(async (response) => {
        if (response) {
          if (response.isSuccess) {
            Fidecomiso(response.data)
            onClose(true);
          }
        }
      }).catch(() => {
        toast("Error del sistema", { type: toast.TYPE.ERROR });
      });
  }

  const SubmitUpdateFidecomiso = (data: Fidecomiso) => {
    data.entidad_id = idEntidad;
    data.ENE_FIDEICOMISO_ID = dataModel?.enE_FIDEICOMISO_ID;
    data.usr_id = user.USR_ID;
    actualizarCuenta(data, idEntidad);
    onClose(true);
  }

  const CreateOrUpdateProgramHandleSubmit = (data:  any) => {
    if(dataModel != null){
      SubmitUpdateFidecomiso(data)
    }else{
      SubmitFidecomiso(data)
    }
  }

  return (
    <Dialog className={classes.table}
      fullWidth
      open={isOpen}
      maxWidth="md"
      aria-labelledby="form-dialog-title">
      <DialogTitle className={classes.title} id="form-dialog-title">{title}</DialogTitle>
      <DialogContent>
        <Grid item container>
          <Grid container md={12}>
            <Grid className="item-containerModal" md={5}>
              <Grid className="subcontainerModal">
                <Controller
                  name="idTipoIdentificacion"
                  control={formularioFidecomiso.control}
                  rules={{
                    validate: () => formularioFidecomiso.watch('idTipoIdentificacion') === 0 ? 'Este campo es requerido.' : true
                  }}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHSelect
                      error={!!error}
                      label="Tipo de Identificación *"
                      value={value}
                      helperText={error ? error.message : null}
                      onChange={e => {
                        onChange(e);
                        const { value } = e.target;

                      }}
                    >
                      {catalogState.IdentificationType?.map(
                        (option: IDENTIFICATIONTYPEDTO) => (
                          <MenuItem key={option.id} value={option.id}>
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
                  name="nroIdentificacion"
                  rules={{
                    required: {
                      value: true,
                      message: 'Este campo es requerido y debe contener solo números',
                    },
                    pattern: {
                      value: /^[0-9]+$/g,
                      message: 'Este campo es requerido y debe contener solo números'
                    },

                    minLength: {
                      value: 4,
                      message:
                        'La longitud mínima del campo es de 4 caracteres',
                    },
                    maxLength: {
                      value: 12,
                      message:
                        'La longitud máxima del campo es de 12 caracteres',
                    },
                    min: {
                      value: 1,
                      message: 'Número de cédula inválido'
                    },
                  }}
                  control={formularioFidecomiso.control}
                  render={({
                    field: { onChange, value },
                    fieldState: { error }
                  }) => (
                    <CHTextField
                      InputLabelProps={{ shrink: true }}
                      label="Número de Identificación *"
                      numberMask
                      onChange={onChange}
                      value={value}
                      helperText={error ? error.message : null}
                      error={!!error}

                    />
                  )}
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid className="item-containerModal" md={5}>
            <Grid className="subcontainerModal">
              <Controller
                name="nombreRazonSocial"
                rules={{
                  required: 'Este campo es requerido',
                }}
                control={formularioFidecomiso.control}
                defaultValue=""
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHTextField
                    InputLabelProps={{ shrink: true }}
                    label="Razón Social *"
                    onChange={(onChange)}
                    value={value}
                    error={!!error}
                    helperText={
                      error ? error.message : null
                    }
                  />
                )}
              />
            </Grid>
          </Grid>

          <Grid className={classes.container}>
            <Button
              className={classes.button}
              type="button"
              onClick={handleClose}

            >
              REGRESAR
            </Button>
            <Button
              className={classes.restoreButton}
              variant="outlined"
              onClick={formularioFidecomiso.handleSubmit(CreateOrUpdateProgramHandleSubmit)}
            >
              {textButton}
            </Button>
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
  );
}

export default CreateAccount;