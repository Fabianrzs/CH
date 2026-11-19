import React, { ChangeEvent, useEffect, useState} from 'react';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { Button, createStyles, Grid, makeStyles,MenuItem} from '@material-ui/core';
import { Controller,useForm } from 'react-hook-form';
import { AppState } from '../../../../store/rootReducer';
import { startGetAllGrupoUsuarios } from '../../../../actions/grupoUsers.action';
import { LoginState } from '../../../../reducers/login.reducer';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../../components/ch-paper-new/CHPaperNew';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../../components/container-control/container-control';
import CHSubContainer from '../../../../components/sub-container/sub-container';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import TableGupoUsuarios from '../components/table-grupos-usuarios/table-grupos-usuarios' 
import { GrupoUsuariosState } from '../../../../reducers/grupousuarios.recducer';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import NotificationService from '../../../../services/notification.service';
import {  startGetAllTypeDocuments } from '../../../../actions/catalog.action';
import { NotificationCardModel, NotificationCardModelEnum } from '../../../../models/requests/notification-cards-model';
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import { startAddUser, startLoadUsers } from '../../../../actions/user.action';
import { User } from '../../../../types/user.type';
import history from '../../../../utils/history.router';
import state from '../../../state/state';
import { UserState } from '../../../../reducers/user.reducer';
import UtilService from '../../../../utils/util.service';
import ContainerTitle from '../../../../components/containe-title/container-title';
import OpcionesTemporalesComponent from '../opciones-temporales/opciones-temporales.component';
import { GUID_DEFAULT } from '../../../../constants/constants';

const useStyles = makeStyles(() =>
    createStyles({

    root: {
        flexGrow: 1,
    },

    buttonprincipal: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#FEC917'
    },
    buttonsecundario: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#43b5f1'
    },
    button: {
        backgroundColor: '#42b7ef',
        color: '#fff',
        width: '18%',
        marginLeft: '80%',
    },

    }),
);


function  CrearUsuario({
    loginState,
    grupoUsuarios,
    catalogState,
    userState,
    startGetAllGrupoUsuarios,
    startAddUser,
    startGetAllTypeDocuments,
}:{

    startGetAllGrupoUsuarios: () => void;
    startAddUser: (user: User, props: string) => void;
    startGetAllTypeDocuments:() => void;
    loginState: LoginState;
    userState: UserState;
    grupoUsuarios: GrupoUsuariosState;
    catalogState:CatalogState;

}){


    const [listGroup]=useState<GrupoUsersModel[]>([]);
    const classes = useStyles();
    const [directory,setValue]=useState<boolean>(false);
    const [userGroupState, setUserGroupState] = useState<any[]>(grupoUsuarios.allgrupo)


    const setGroup = (value: GrupoUsersModel) => {
        if(listGroup.findIndex(x => x.id === value.id) === -1 ){
            listGroup.push(value); 
        }else{
            listGroup.splice(listGroup.findIndex(x => x.id === value.id), 1);   
        }
    };


    const handleChange=()=>(event: ChangeEvent<HTMLInputElement>) =>{
       setValue(event.target.checked);
    }


    const onSubmit = (data:any) => {

        data.directory=directory;
        data.opcionTemporales = temporalOptions

       if(data.contrasena!== data.repitaContrasena){
            NotificationService.showNotification({
                message: "La contraseña de verificación no coincide.",
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

            return;
        }

        if(listGroup.length > 0){  

            swal({
                text: "¿Quieres guardar este usuario?",
                icon: "info",
                buttons: ["No","Sí"]
            }).then (async respuesta =>{
                if(respuesta){       
                    data.id = GUID_DEFAULT      
                    data.roles=listGroup;
                    data.opcionTemporales = temporalOptions
                    startAddUser(data,loginState.claims);
                }
            })
    
        }else{
            swal({
                text: "¿Quieres guardar este usuario sin asignarle un grupo?",
                icon: "warning",
                buttons: ["No","Sí"]
            }).then (respuesta =>{
                if(respuesta){         
                   startAddUser(data,loginState.claims);
                }
            })
        }
    }

    const onCancel=()=>{
        history.push('/Usuarios');
    }

    useEffect(() => {
        setUserGroupState(grupoUsuarios.allgrupo)
    }, [grupoUsuarios.allgrupo]);

    useEffect(() => {
        startGetAllGrupoUsuarios();
        startGetAllTypeDocuments();     
        startLoadUsers(loginState.claims) 
    }, []);

    const { control,watch,handleSubmit, setError, clearErrors } = useForm({
        defaultValues: {
            tipoIdentificationId: '',
            identificacion: '',
            nombre: '',
            segundoNombre:'',
            apellido:'',
            segundoApellido:'',
            correo:'',
            nombreUsuario:'',
            contrasena:'',
            repitaContrasena:'',
            estado: true,
            id: ''
        },
    });

    const validarCedula = () => {
        const cc = userState.users.find(x => x.identificacion === watch('identificacion'))
        if(cc != null) {
            setError('identificacion', { type: "focus", message: "Ya existe un usuario con este número de identificación" }, { shouldFocus: true })
        } else {
            clearErrors('identificacion')
        }
    }

    const validarUsuario = () => {
        const cc = userState.users.find(x => x.nombreUsuario === watch('nombreUsuario'))
        if(cc != null) {
            setError('nombreUsuario', { type: "focus", message: "Ya existe este nombre de usuario" }, { shouldFocus: true })
        } else {
            clearErrors('nombreUsuario')
        }
    }

    const validarCorreo = () => {
        const cc = userState.users.find(x => x.correo === watch('correo'))
        if(cc != null) {
            setError('correo', { type: "focus", message: "Ya existe un usuario con este correo" }, { shouldFocus: true })
        } else {
            clearErrors('correo')
        }
    }

  const [crear, setCrear] = useState<boolean>(false);
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [data, setData] = useState(null);

  const [temporalOptions, setTemporalOptions] = useState<any[]>([]);

    const agregarOpcionTemporal = () => {
        setCrear(true)
        setIsOpen(true)
        setData(null)
    }

    const onClose = () => {
        setIsOpen(false)
      }

    const onSubmitOpciones = (data: any) => {
            setTemporalOptions([...temporalOptions, data])    
        setIsOpen(false)
      }
   
    return(
    
        <div className={classes.root}>
            <form  onSubmit={handleSubmit(onSubmit)}>

                <PageIndicators
                    topTextNivel1='Administración de Usuarios'
                    topTextNivel2='Agregar Usuario'
                />

                <IconTitle
                    subTitle='Usuario'
                    title='Agregar'
                    icon='fch-Administracion-Usuarios-0-1'
                />

                <CHPaperNew>

                    <CHSubContainer>
                        <Grid container>
                            <CHContainerSubtitle
                                subTitle='Documento*'
                                title='Tipo de '
                                icon='fch-Tipo-Documento-2-1'     
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
                                        onChange={(value) => {
                                            onChange(value);
                                        }}
                                    >
                                        
                                        {catalogState.IdentificationType?.map(
                                            (option: IDENTIFICATIONTYPEDTO) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
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
                                subTitle='Documento*'
                                title='Número de '
                                icon='fch-Numero-Documento-2-1'     
                            />
                        </Grid>
                        <Grid container>
                            <Controller
                                name="identificacion"
                                control={control}
                                rules={{
                                    pattern: {
                                        value: /^[0-9]+$/g,
                                        message: 'Este campo es requerido y debe contener solo números'
                                    },
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido y debe contener solo números',
                                    },
                                    maxLength: {
                                        value: 12,
                                        message:
                                            'La longitud máxima del campo es de 12 caracteres',
                                    },
                                    minLength: {
                                        value: 4,
                                        message:
                                            'La longitud mínima del campo es de 4 caracteres',
                                    },
                                    
                                    min: {
                                        value: 1,
                                        message: 'Número de cédula inválido'
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
                                        onBlur={validarCedula}
                                        numberMask
                                    />
                                )}
                            />
                        </Grid>  
                    </CHSubContainer>

                    <CHSubContainer>
                        <Grid container>
                            <CHContainerSubtitle
                                subTitle='Nombre*'
                                title='Primer'
                                icon='fch-Nombre-0-1'     
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
                                        message: 'Solo se permiten letras.'},
    
                                    maxLength: {
                                        value: 25,
                                        message:
                                            'La longitud máxima del campo es de 25 caracteres',
                                    },
                                    minLength: {
                                        value: 1,
                                        message:
                                            'La longitud mínima del campo es de 1 caracteres',
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
                                subTitle='Nombre'
                                title='Segundo'
                                icon='fch-Apellidos-0-1'     
                            />
                        </Grid>
                        <Grid container>
                            <Controller
                                name="segundoNombre"
                                control={control}
                                rules={{
                                
                                    pattern: {
                                        value: /^[ A-zÀ-ú ]+$/g,
                                        message: 'Solo se permiten letras.'},
    
                                    maxLength: {
                                        value: 25,
                                        message:
                                            'La longitud máxima del campo es de 25 caracteres',
                                    },
                                    minLength: {
                                        value: 1,
                                        message:
                                            'La longitud mínima del campo es de 1 caracteres',
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
                                subTitle='Apellido*'
                                title='Primer'
                                icon='fch-Nombre-0-1'     
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
                                        message: 'Solo se permiten letras.'},
    
                                    maxLength: {
                                        value: 25,
                                        message:
                                            'La longitud máxima del campo es de 25 caracteres',
                                    },
                                    minLength: {
                                        value: 1,
                                        message:
                                            'La longitud mínima del campo es de 1 caracteres',
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
                                subTitle='Apellido'
                                title='Segundo'
                                icon='fch-Apellidos-0-1'     
                            />
                        </Grid>
                        <Grid container>
                            <Controller
                                name="segundoApellido"
                                control={control}
                                rules={{
                                
                                    pattern: {
                                        value: /^[ A-zÀ-ú ]+$/g,
                                        message: 'Solo se permiten letras.'},
    
                                    maxLength: {
                                        value: 25,
                                        message:
                                            'La longitud máxima del campo es de 25 caracteres',
                                    },
                                    minLength: {
                                        value: 1,
                                        message:
                                            'La longitud mínima del campo es de 1 caracteres',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? 'El segundo apellido solo debe contener letras' : null}
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
                                subTitle='Usuario*'
                                title='Crear'
                                icon='fch-Administracion-Usuarios-0-1'     
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
                                subTitle='Electrónico*'
                                title='Correo'
                                icon='fch-Email-Institucional-0-1'     
                            />
                        </Grid>
                        <Grid container>
                            <Controller
                                name="correo"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                    pattern: {
                                        value: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                        message: 'Ingrese un correo electrónico válido'
                                    }
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error
                                                ? error.message
                                                : null
                                        }
                                    
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
                                    required: 'Este campo es requerido.',
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
                                    required: 'Este campo es requerido.',
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
                                subTitle='Active Directory'
                                title='Autenticación'
                                icon='fch-Administracion-Usuarios-0-1'     
                            />
                        </Grid>
                        <Grid container>
                            <label className="container-14 cian" htmlFor="checkbox-directory">
                                    <input 
                                        name="active_directory"      
                                        type="checkbox" 
                                        id="checkbox-directory"
                                    
                                        defaultChecked={ directory } 
                                        onChange={handleChange()}
                                    />
                                    <span className="checkmark"/>
                            </label>
                        </Grid>
                    </CHSubContainer>


                </CHPaperNew> 

                <br/>
                <br/>

                <CHPaperNew> 
                    <TableGupoUsuarios
                        grupoUsuarios={userGroupState}
                        setGroup={setGroup}
                        usuarioGrupo={[]}
                    />   
                </CHPaperNew>  

                <br />
      <br />

      <CHPaperNew>
        <ContainerTitle title="Opciones temporales" />
        <div className="buttons">
            <Button
                className={classes.button}
                variant="outlined"
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
        idUsuario={watch('id') as string}
        onClose={onClose}
        onSubmit={onSubmitOpciones}
      />

                 
                <Grid className='buttonssolicitud' >
                    <Button
                        className={classes.buttonprincipal}
                        variant="outlined"  
                        onClick={handleSubmit(onSubmit)} 
                    >
                        CREAR
                    </Button>

                    <Button
                        className={classes.buttonsecundario}
                        variant="outlined"
                        onClick={onCancel}
                    >
                        CANCELAR
                    </Button>
                </Grid>

            </form>
        </div>
            
    );
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    grupoUsuarios:state.grupoUsuarios,
    catalogState: state.catalog,
    userState: state.users
});

const mapDispatchToProps = {
    startGetAllGrupoUsuarios,
    startAddUser,
    startGetAllTypeDocuments
}

export default connect(mapStateToProps, mapDispatchToProps)(CrearUsuario);