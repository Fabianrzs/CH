import React, { ChangeEvent, useEffect, useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import {
  Button,
  createStyles,
  Grid,
  makeStyles,
  MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AppState } from '../../../../store/rootReducer';
import {
  startLoadUsers,
  startUpdateUser,
} from '../../../../actions/user.action';
import { UserState } from '../../../../reducers/user.reducer';
import { LoginState } from '../../../../reducers/login.reducer';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../../components/ch-paper-new/CHPaperNew';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../../components/container-control/container-control';
import CHSubContainer from '../../../../components/sub-container/sub-container';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { startGetAllTypeDocuments } from '../../../../actions/catalog.action';
import './editar-usuario.scss';
import TableGupoUsuarios from '../components/table-grupos-usuarios/table-grupos-usuarios';
import { startGetAllGrupoUsuarios, startGetAllOpciones } from '../../../../actions/grupoUsers.action';
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import NotificationService from '../../../../services/notification.service';
import {
  NotificationCardModel,
  NotificationCardModelEnum,
} from '../../../../models/requests/notification-cards-model';
import { GrupoUsuariosState } from '../../../../reducers/grupousuarios.recducer';
import { User } from '../../../../types/user.type';
import history from '../../../../utils/history.router';
import GrupoUsers from '../../../../services/api-grupoUsuarios.service';
import ContainerTitle from '../../../../components/containe-title/container-title';
import OpcionesTemporalesComponent from '../opciones-temporales/opciones-temporales.component';
import { GrupoOpcionesState } from '../../../../reducers/grupo-opciones.reducer';
import UtilService from '../../../../utils/util.service';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      flexGrow: 1,
    },

    buttonprincipal: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#FEC917',
    },
    buttonsecundario: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#43b5f1',
    },
    button: {
        backgroundColor: '#42b7ef',
        color: '#fff',
        width: '18%',
        marginLeft: '80%',
    },
  }),
);

const api = GrupoUsers();

function EditarUsuarios({
  loginState,
  users,
  catalogState,
  grupoOpciones,
  startGetAllTypeDocuments,
  startLoadUsers,
  startGetAllGrupoUsuarios,
  startUpdateUser,
  startGetAllOpciones,
  grupoUsuarios,
}: {
  loginState: LoginState;
  users: UserState;
  catalogState: CatalogState;
  grupoOpciones: GrupoOpcionesState,
  startGetAllTypeDocuments: () => void;
  startLoadUsers: (props: string) => void;
  startGetAllGrupoUsuarios: () => void;
  grupoUsuarios: GrupoUsuariosState;
  startUpdateUser: (user: User, props: string) => void;
  startGetAllOpciones: () => void;
}) {
  const classes = useStyles();
  const { control, handleSubmit, watch, setError, clearErrors } = useForm({
    defaultValues: {
      tipoIdentificationId: users.user.tipoIdentificationId,
      identificacion: users.user.identificacion,
      nombre: users.user.nombre,
      segundoNombre: users.user.segundoNombre,
      apellido: users.user.apellido,
      segundoApellido: users.user.segundoApellido,
      correo: users.user.correo,
      nombreUsuario: users.user.nombreUsuario,
      contrasena:'',
      repitaContrasena:'',
      id: users.user.id,
    },
  });

  const [listGroup, setListGroup] = useState<GrupoUsersModel[]>([]);
  const [estado, setEstado] = useState<boolean>(users.user.estado);
  const [directory, setValue] = useState<boolean>(users.user.directory);

  const [temporalOptions, setTemporalOptions] = useState<any[]>([]);

  const [data, setData] = useState(null);
  const [onFirstLoad, setOnFirstLoad] = useState(false);

  const handleChange = () => (event: ChangeEvent<HTMLInputElement>) => {
    setEstado(event.target.checked);
  };

  const handleChangeDirectory =
    () => (event: ChangeEvent<HTMLInputElement>) => {
      setValue(event.target.checked);
    };

  const temp = [...listGroup]  
  if (users.user.roles.length > 0 && !onFirstLoad) {
    setOnFirstLoad(true)
    users.user.roles.forEach(value => {
      if (listGroup.findIndex(x => x.id === value.id) === -1) {
        temp.push(value)
        setListGroup(temp)
      }
    });
  }

  const setGroup = (value: GrupoUsersModel) => {
    let temp = [...listGroup]
    if (temp.findIndex(x => x.id === value.id) === -1) {
      temp.push(value)
    } else {
      temp = temp.filter(x => x.id !== value.id)
    }
    setListGroup(temp)

  };

  const onSubmit = (data: any) => {
    if (data.contrasena !== data.repitaContrasena) {
      NotificationService.showNotification({
        message: 'La contraseña de verificación no coincide.',
        image: null,
        type: NotificationCardModelEnum.error,
      } as NotificationCardModel);

      return;
    }

    if (listGroup.length > 0) {
      swal({
        text: '¿Quieres actualizar este usuario?',
        icon: 'info',
        buttons: ['No', 'Sí'],
      }).then(respuesta => {
        if (respuesta) {
          data.id = users.user.id;
          data.roles = listGroup;
          data.estado = estado;
          data.directory = directory;
          startUpdateUser(data, loginState.claims);
        }
      });
    } else {
      swal({
        text: '¿Quieres guardar este usuario sin asignarle un grupo?',
        icon: 'warning',
        buttons: ['No', 'Sí'],
      }).then(respuesta => {
        if (respuesta) {
          data.id = users.user.id;
          data.roles = listGroup;
          data.estado = estado;
          data.directory = directory;
          startUpdateUser(data, loginState.claims);
        }
      });
    }
  };

  const onCancel = () => {
    history.push('/Usuarios');
  };

  useEffect(() => {
    startLoadUsers(loginState.claims);
    startGetAllTypeDocuments();
    startGetAllGrupoUsuarios();
    setEstado(users.user.estado);
    startGetTemporalOptions();
    startGetAllOpciones();
  }, []);

  const startGetTemporalOptions = () => {
    api.ObtenerOpcionesTemporales(watch('id'))
    .then(response => {
        setTemporalOptions(response?.data)
    })
    .catch(error => {
        toast('Error al consultar las opciones temporales')
    });
  };

  const validarCedula = () => {
    const cc = users.users.find(
      x => x.identificacion === watch('identificacion') && x.id !== watch('id') && !x.inhabilitado,
    );
    if (cc != null) {
      setError(
        'identificacion',
        {
          type: 'focus',
          message: 'Ya existe un usuario con este número de identificación',
        },
        { shouldFocus: true },
      );
    } else {
      clearErrors('identificacion');
    }
  };

  const validarUsuario = () => {
    const cc = users.users.find(
      x => x.nombreUsuario === watch('nombreUsuario') && x.id !== watch('id'),
    );
    if (cc != null) {
      setError(
        'nombreUsuario',
        { type: 'focus', message: 'Ya existe este nombre de usuario' },
        { shouldFocus: true },
      );
    } else {
      clearErrors('nombreUsuario');
    }
  };

  const validarCorreo = () => {
    const cc = users.users.find(
      x => x.correo === watch('correo') && x.id !== watch('id'),
    );
    if (cc != null) {
      setError(
        'correo',
        { type: 'focus', message: 'Ya existe un usuario con este correo' },
        { shouldFocus: true },
      );
    } else {
      clearErrors('correo');
    }
  };

  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [crear, setCrear] = useState<boolean>(false);
  
  const onClose = () => {
    setIsOpen(false)
  }

  const onSubmitOpciones = (data: any) => {
    if(temporalOptions.find(x => x.idOpcionTemporalUsuario === data.idOpcionTemporalUsuario) != null){
        setTemporalOptions(temporalOptions?.map(x => data.idOpcionTemporalUsuario === x.idOpcionTemporalUsuario ? data : x))
    } else {
        setTemporalOptions([...temporalOptions, data])
    }

    setIsOpen(false)
  }

  const agregarOpcionTemporal = () => {
    setCrear(true)
    setIsOpen(true)
    setData(null)
  }

  const editarOpcionTemporal = (data: any) => {
    setCrear(false)
    setIsOpen(true)
    setData(data)
  }

  return (
    <div className={classes.root}>
      <PageIndicators
        topTextNivel1="Roles y permisos"
        topTextNivel2="Administración de Usuarios"
      />

      <IconTitle
        subTitle="De Usuarios"
        title="Actualización"
        icon="fch-Administracion-Usuarios-0-1"
      />

      <CHPaperNew>
        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Documento"
              title="Tipo de "
              icon="fch-Tipo-Documento-2-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="tipoIdentificationId"
              control={control}
              rules={{
                required: 'Este campo es requerido.',
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHSelect
                  error={!!error}
                  label=""
                  value={value}
                  helperText={error ? error.message : null}
                  onChange={value => {
                    onChange(value);
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
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Documento*"
              title="Número de "
              icon="fch-Numero-Documento-2-1"
            />
          </Grid>

          <Grid container>
            <Controller
              name="identificacion"
              control={control}
              rules={{
                pattern: {
                  value: /^[0-9]+$/g,
                  message:
                    'Este campo es requerido y debe contener solo números',
                },
                required: {
                  value: true,
                  message:
                    'Este campo es requerido y debe contener solo números',
                },
                maxLength: {
                  value: 12,
                  message: 'La longitud máxima del campo es de 12 caracteres',
                },
                minLength: {
                  value: 4,
                  message: 'La longitud mínima del campo es de 4 caracteres',
                },

                min: {
                  value: 1,
                  message: 'Número de cédula inválido',
                },
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                  numberMask
                  onBlur={validarCedula}
                />
              )}
            />
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Nombre*"
              title="Primer"
              icon="fch-Nombre-0-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="nombre"
              control={control}
              rules={{
                required: {
                  value: true,
                  message: 'Este campo es requerido.',
                },
                pattern: {
                  value: /^[ A-zÀ-ú ]+$/g,
                  message: 'Solo se permiten letras.',
                },

                maxLength: {
                  value: 25,
                  message: 'La longitud máxima del campo es de 25 caracteres',
                },
                minLength: {
                  value: 1,
                  message: 'La longitud mínima del campo es de 1 caracteres',
                },
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Nombre"
              title="Segundo"
              icon="fch-Apellidos-0-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="segundoNombre"
              control={control}
              rules={{
                pattern: {
                  value: /^[ A-zÀ-ú ]+$/g,
                  message: 'Solo se permiten letras.',
                },

                maxLength: {
                  value: 25,
                  message: 'La longitud máxima del campo es de 25 caracteres',
                },
                minLength: {
                  value: 1,
                  message: 'La longitud mínima del campo es de 1 caracteres',
                },
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Apellido*"
              title="Primer"
              icon="fch-Nombre-0-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="apellido"
              control={control}
              rules={{
                required: {
                  value: true,
                  message: 'Este campo es requerido.',
                },
                pattern: {
                  value: /^[ A-zÀ-ú ]+$/g,
                  message: 'Solo se permiten letras.',
                },

                maxLength: {
                  value: 25,
                  message: 'La longitud máxima del campo es de 25 caracteres',
                },
                minLength: {
                  value: 4,
                  message: 'La longitud mínima del campo es de 4 caracteres',
                },
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Apellido"
              title="Segundo"
              icon="fch-Apellidos-0-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="segundoApellido"
              control={control}
              rules={{
                pattern: {
                  value: /^[ A-zÀ-ú ]+$/g,
                  message: 'Solo se permiten letras.',
                },

                maxLength: {
                  value: 25,
                  message: 'La longitud máxima del campo es de 25 caracteres',
                },
                minLength: {
                  value: 4,
                  message: 'La longitud mínima del campo es de 4 caracteres',
                },
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Usuario*"
              title="Editar"
              icon="fch-Administracion-Usuarios-0-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="nombreUsuario"
              control={control}
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
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                  onBlur={validarUsuario}
                />
              )}
            />
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Electrónico"
              title="Correo"
              icon="fch-Email-Institucional-0-1"
            />
          </Grid>
          <Grid container>
            <Controller
              name="correo"
              control={control}
              rules={{
                pattern: {
                  value: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                  message: 'Ingrese un correo electrónico válido',
                },
                required: 'Este campo es requerido.',
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly={false}
                  label=""
                  value={value}
                  onChange={onChange}
                  onBlur={validarCorreo}
                />
              )}
            />
          </Grid>
        </CHSubContainer>
        <CHSubContainer>
                        <Grid container>
                            <CHContainerSubtitle
                                subTitle='Contraseña*'
                                title='Crear'
                                icon='fch-Contrasena-0-1'     
                            />
                        </Grid>
                        <Grid container>
                            <Controller
                                name="contrasena"
                                control={control}
                                rules={{
                                    minLength: {
                                        value: 5,
                                        message:
                                            'La longitud mínima del campo es de 5 caracteres',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        label=""
                                        type="password"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    </CHSubContainer>    

                    <CHSubContainer>
                        <Grid container>
                            <CHContainerSubtitle
                                subTitle='Contraseña*'
                                title='Repetir'
                                icon='fch-Repetir-Contrasena-0-1'     
                            />
                        </Grid>
                        <Grid container>
                            <Controller
                                name="repitaContrasena"
                                control={control}
                                rules={{
                                    minLength: {
                                        value: 5,
                                        message:
                                            'La longitud mínima del campo es de 5 caracteres',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        label=""
                                        type="password"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    </CHSubContainer>
        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle="Active Directory"
              title="Autenticación"
              icon="fch-Administracion-Usuarios-0-1"
            />
          </Grid>
          <Grid container>
            <label className="container-14 cian" htmlFor="checkbox-directory">
              <input
                name="active_directory"
                type="checkbox"
                id="checkbox-directory"
                defaultChecked={directory}
                onChange={handleChangeDirectory()}
              />
              <span className="checkmark" />
            </label>
          </Grid>
        </CHSubContainer>

        <CHSubContainer>
          <Grid container>
            <CHContainerSubtitle
              subTitle=""
              title="Estado"
              icon="fch-Estado-Activo-Inactivo-0-1"
            />
          </Grid>
          <Grid container>
              <label className="container-14 cian" htmlFor="checkbox-estado">
              <input
                  name="active_estado"
                  type="checkbox"
                  id="checkbox-estado"
                  defaultChecked={estado}
                  onChange={handleChange()}
                />
                <span className="checkmark" />
              </label>
          </Grid>
        </CHSubContainer>
      </CHPaperNew>

      <br />
      <br />

      <CHPaperNew>
        <TableGupoUsuarios
          grupoUsuarios={grupoUsuarios.allgrupo}
          setGroup={setGroup}
          usuarioGrupo={users.user.roles}
        />
      </CHPaperNew>
      <CHPaperNew>
        <ContainerTitle title="Opciones temporales" />
        <div className="buttons">
            <Button
                className={classes.button}
                variant="outlined"
                type="submit"
                onClick={() => agregarOpcionTemporal()}
            >
                AGREGAR OPCIÓN
            </Button>
        </div>

        <br />

        <Grid>
          <Grid className="container-17">
            <table className="table">
              <thead>
                <th className="tab-space-01">Código</th>
                <th className="tab-space-01">Nombre</th>
                <th className="tab-space-01">Descripción</th>
                <th className="tab-space-01">Fecha Desde</th>
                <th className="tab-space-01">Fecha Hasta</th>
                <th className="tab-space-01">Acciones</th>
              </thead>
              <tbody>
                {temporalOptions?.map((row: any, index: number) => (
                  <tr key={`table-${index}`}>
                    <td data-label="Descripción" className="tab-space-01">
                      {row.codigoOpcionTemporalUsuario}
                    </td>
                    <td data-label="Tasa-EA" className="tab-space-01">
                      {row.nombreOpcionTemporalUsuario}
                    </td>
                    <td data-label="Tasa-EA" className="tab-space-01">
                      {row.descripcionOpcionTemporalUsuario}
                    </td>
                    <td data-label="Plazo-Mínimo" className="tab-space-01">
                      {UtilService.getDate(row.fechaDesdeOpcionTemporalUsuario)}
                    </td>
                    <td
                      data-label="Fecha Desde"
                      className="tab-space-01 actions"
                    >
                      {UtilService.getDate(row.fechaHastaOpcionTemporalUsuario)}
                    </td>
                    <td
                      data-label="Fecha Hasta"
                      className="tab-space-01 actions"
                    >
                      <Grid>
                        <span aria-label="Editar">
                          <span className="ico fch-Editar-0-1 icoMid-3" 
                          onClick={() => editarOpcionTemporal(row)}
                        />
                        </span>
                      </Grid>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Grid>
        </Grid>
      </CHPaperNew>

      <OpcionesTemporalesComponent
        data={data}
        open={isOpen}
        crear={crear}
        idUsuario={watch('id')}
        onClose={onClose}
        onSubmit={onSubmitOpciones}
      />

      <Grid className="buttonssolicitud">
        <Button
          className={classes.buttonprincipal}
          variant="outlined"
          onClick={handleSubmit(onSubmit)}
        >
          ACTUALIZAR
        </Button>

        <Button
          className={classes.buttonsecundario}
          variant="outlined"
          onClick={onCancel}
        >
          CANCELAR
        </Button>
      </Grid>
    </div>
  );
}

const mapStateToProps = (state: AppState) => ({
  loginState: state.login,
  users: state.users,
  catalogState: state.catalog,
  grupoUsuarios: state.grupoUsuarios,
  grupoOpcionesState:state.grupoOpciones,
});

const mapDispatchToProps = {
  startLoadUsers,
  startGetAllTypeDocuments,
  startGetAllGrupoUsuarios,
  startUpdateUser,
  startGetAllOpciones
};

export default connect(mapStateToProps, mapDispatchToProps)(EditarUsuarios);
