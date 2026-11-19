import { Button, createStyles, 
         Grid, 
         IconButton, 
         makeStyles, 
         MenuItem } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { Delete, Edit } from '@material-ui/icons';
import { toast } from 'react-toastify';
import { startGetAllDepartments, startGetAllCities } from '../../../../actions/catalog.action';
import CHPageTitle from '../../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import AddressComponent from '../../../../components/modals/address/address.component';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { LoginState } from '../../../../reducers/login.reducer';
import { AppState } from '../../../../store/rootReducer';
import { Address } from '../../../../types/address.type';
import { CITYDTO, DEPARTAMENTODTO } from '../../../../types/catalog.type';
import { setFormDataAddres } from '../../../productClient/components/address.component.constants';
import { Constructora } from '../../constants/fondosolidaridadform.model.type';
import ApiCatalog from '../../../../services/api-catalog.service';
import CHPrivateContainer from '../../../../components/ch-private-container/CHPrivateContainer';
import NotificationService from '../../../../services/notification.service';
import ApiFondoSolidaridad from '../../../../services/api-fondoSolidaridad';
import { LoadingComponent } from '../../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnRight: {
            padding: '10px',
            textAlign: 'right',
        },
        columnButtonAdd: {
            padding: '0px',
            textAlign: 'right',
        },
        button: {
            marginTop: '1.5rem',
            marginRight: '.5rem',
        },
        textTable: {
			textAlign: 'center'
		}
    }),
);

function ConstructorasTable({
    constructoras,
    catalogState,
    seleccionarConstructora,
    eliminarConstructora
}:{
    constructoras: Constructora[];
    catalogState: CatalogState;
    seleccionarConstructora: (constructora: any) => void;
    eliminarConstructora: (idConstructora: string) => void;
}) {
    const classes = useStyles();

    const obtenerNombreDepartamento = (idDepartamento: number) => {
        const departamentoEncontrado = catalogState.departments.find(departamento => departamento.id === idDepartamento);
        return departamentoEncontrado?.nombre || '';
    }

    const obtenerNombreCiudad = (idCiudad: number) => {
        const ciudadEncontrada = catalogState.City.find(ciudad => ciudad.id === idCiudad);
        return ciudadEncontrada?.nombre || '';
    }

	return (
	  <>
		<table className="credit-table">
				<thead>
					<tr>
						<th className={classes.textTable}>Nombre Constructora</th>
						<th className={classes.textTable}>Número Identificación Tributaria</th>
						<th className={classes.textTable}>Dirección</th>
						<th className={classes.textTable}>Teléfono</th>
						<th className={classes.textTable}>Departamento</th>
						<th className={classes.textTable}>Municipio</th>
						<th className={classes.textTable}>Email</th>
                        <th className={classes.textTable}>Acciones</th>
					</tr>
				</thead>
                <tbody>
                {constructoras.map((item: Constructora) => 
                       <tr key={item.nit}>
                       <td>{item.nombreConstructora}</td>
                       <td>{item.nit}</td>
                       <td>{item.direccion}</td>
                       <td>{item.telefono}</td>
                       <td>{obtenerNombreDepartamento(item.idDepartamento)}</td>
                       <td>{obtenerNombreCiudad(item.idCiudad)}</td>
                       <td>{item.email}</td>
                       <td> <IconButton
                                 aria-label="Editar"
                                 color="primary"
                                 onClick={() => seleccionarConstructora(item)}
                             >
                               <Edit />
                             </IconButton>
                             <IconButton
                                 aria-label="Eliminar"
                                 color="primary"
                                 size="small"
                                 onClick={() => eliminarConstructora(String(item?.id_Constructuora) || '')}
                             >
                                 <Delete />
                         </IconButton>
                         </td>
                     </tr>
                )}
              </tbody>
			</table>
		</>
	)
};

const ConstructoraProyecto = (props: Props) => {

    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {loginState,
           catalogState,
           setOpenConstructoras,
           startGetAllDepartments,
           startGetAllCities} = props;

    const [ciudades,setCiudades] = useState<CITYDTO[]>([]);
    const [open, setOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [constructoras, setConstructoras] = useState<Constructora[]>([]);
    const [estadoBoton, setEstadoBoton] = useState('AGREGAR CONSTRUCTORA');
    const formularioConstructora = useForm({
        defaultValues: {
            id: '',
            nombreConstructora: '',
            nit: '',
            direccion: '',
            telefono: '',
            idDepartamento: '',
            departamento: '',
            idCiudad: '',
            ciudad: '',
            email: '',
        },
    });

    useEffect(() => {
        startGetAllDepartments(loginState.claims);
        startGetAllCities();
        consultarConstructoras();
    },[])

     const handleOpenAddress = () => {
        setOpen(true);
    };

    const onCloseAddress = (value: Address) => {
        if (value !== null) {
            const result = setFormDataAddres(value);
            formularioConstructora.setValue('direccion', result);
            formularioConstructora.clearErrors('direccion');
            setOpen(false);
        } else {
            formularioConstructora.setValue('direccion', '');
            formularioConstructora.setError('direccion', {type: 'custom', message: 'Este campo es requerido.'});
            setOpen(false);
        }
    };

    const obtenerCiudades = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudades(datos);
        }
    };

    const consultarConstructoras = async () => {
        setLoading(true);
        await apiFondoSolidaridad.getConsultarConstructoras()
            .then((response:any) => {
                if(response.data){
                   setConstructoras(response.data);
                   setLoading(false);
                }
            })
            .catch( (e) => {
                setLoading(false);
                toast(`Error en consultar constructoras ${e.message}`, { type: toast.TYPE.ERROR })
            })
    };

    const agregaryActualizarConstructora = async(data: any) => {
        setLoading(true);
        if(estadoBoton === 'AGREGAR CONSTRUCTORA'){
            const constructora: Constructora = {
                nombreConstructora: String(data.nombreConstructora).toUpperCase(),
                nit: Number(data.nit),
                direccion: String(data.direccion).toUpperCase(),
                telefono: Number(data.telefono),
                idDepartamento: data.idDepartamento,
                idCiudad: data.idCiudad,
                email: data.email
            }
            await apiFondoSolidaridad.postAgregarConstructora(constructora)
            .then(response => {
                if(response.data && response.returnMessage === 'OK'){
                    setLoading(false);
                    cancelar();
                    consultarConstructoras();
                    NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                }
            })
            .catch( (e) => {
                setLoading(false);
                toast(`Error en agregar constructora ${e.message}`, { type: toast.TYPE.ERROR })
            })
        }else{
            const constructora: Constructora = {
                id_Constructuora: data.id,
                nombreConstructora: String(data.nombreConstructora).toUpperCase(),
                nit: Number(data.nit),
                direccion: String(data.direccion).toUpperCase(),
                telefono: Number(data.telefono),
                idDepartamento: data.idDepartamento,
                idCiudad: data.idCiudad,
                email: data.email
            }
            await apiFondoSolidaridad.postActualizarConstructora(constructora)
            .then(response => {
                if(response.returnMessage === 'OK'){
                    setLoading(false);
                    cancelar();
                    consultarConstructoras();
                    NotificationService.genericAlert(
                        'Actualizado con éxito',
                        'Se realizó con éxito la actualización.',
                    )
                }
            })
            .catch( (e) => {
                setLoading(false);
                toast(`Error en actualizar constructora ${e.message}`, { type: toast.TYPE.ERROR })
            })
        }
    }

    const seleccionarConstructora = async(constructora: any) => {
        setEstadoBoton('EDITAR CONSTRUCTORA');
        formularioConstructora.setValue('id',constructora?.id_Constructuora);
        formularioConstructora.setValue('nombreConstructora',constructora?.nombreConstructora);
        formularioConstructora.setValue('nit',String(constructora?.nit));
        formularioConstructora.setValue('direccion',constructora?.direccion);
        formularioConstructora.setValue('telefono',String(constructora?.telefono));
        formularioConstructora.setValue('idDepartamento',String(constructora?.idDepartamento));
        obtenerCiudades(constructora?.idDepartamento);
        formularioConstructora.setValue('idCiudad',String(constructora?.idCiudad));
        formularioConstructora.setValue('email',constructora?.email);
    }

    const eliminarConstructora = async(idConstructora: string) => {
        swal({
            text: '¿Está seguro que desea eliminar el registro?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                eliminar(idConstructora);
            }
        });
    }

    const eliminar = async(idConstructora: string) => {
       apiFondoSolidaridad.postEliminarConstructora(idConstructora)
       .then(response => {
            if(response.data === 'Exitoso'){
                consultarConstructoras();
                NotificationService.genericAlert(
                    'Exitoso!',
                    'Se realizó la petición con éxito.',
                );
            }
       })
    }

    const cancelar = () => {
        setEstadoBoton('AGREGAR CONSTRUCTORA');
        formularioConstructora.clearErrors();
        formularioConstructora.setValue('id','');
        formularioConstructora.setValue('nombreConstructora','');
        formularioConstructora.setValue('nit','');
        formularioConstructora.setValue('direccion','');
        formularioConstructora.setValue('telefono','');
        formularioConstructora.setValue('idDepartamento','');
        formularioConstructora.setValue('departamento','');
        formularioConstructora.setValue('idCiudad','');
        formularioConstructora.setValue('ciudad','');
        formularioConstructora.setValue('email','');
    }

    const regresarInicio = () => {
        setOpenConstructoras(false);
    }

    return(
        <>
            <LoadingComponent loading={loading} />
            <AddressComponent
                onOpen={open}
                onClose={onCloseAddress}
                control={formularioConstructora.control}
                getValues={formularioConstructora.watch}
                tipoDireccion
                setValue={formularioConstructora.setValue}
            />
            <CHPrivateContainer>
            <CHPageTitle
                    topText="Gestión de Proyectos"
                    subText=""
                    title1="Constructoras"
                    title2="Administración de Proyectos de Vivienda"
            />
            <CHPaper title="Información Constructora">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreConstructora"
                            control={formularioConstructora.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombre Constructora *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 40){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nit"
                            control={formularioConstructora.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Identificación Tributaria *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="direccion"
                            control={formularioConstructora.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Dirección *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onClick={handleOpenAddress}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="telefono"
                                control={formularioConstructora.control}
                                rules={{
                                    pattern:
                                        /\(?([3,6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        label="Teléfono *"
                                        type="number"
                                        error={!!error}
                                        helperText={
                                            error
                                                ? 'El teléfono debe empezar por 3 si es celular o 6 si es fijo y tener una longitud de 10 dígitos'
                                                : null
                                        }
                                        isReadOnly={false}
                                        value={value}
                                        onChange={e => {
                                            if (
                                                e.target.value.length <= 10
                                            ) {
                                                onChange(e);
                                            }
                                        }}
                                    />
                                )}
                            />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="idDepartamento"
                                control={formularioConstructora.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={e => {
                                            formularioConstructora.setValue('idCiudad','');
                                            obtenerCiudades(Number(e.target.value));
                                            onChange(e);
                                        }}
                                        label="Departamento *"
                                    >
                                    {catalogState?.departments.map(
                                        (option: DEPARTAMENTODTO) => {
                                            if(option.id !== 0){
                                                return (
                                                    <MenuItem key={option.id} 
                                                      value={option.id}
                                            >
                                                     {option.nombre}
                                                     </MenuItem>
                                                )
                                            }
                                            return null;
                                       }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="idCiudad"
                                control={formularioConstructora.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Municipio *"
                                >
                                    {ciudades?.map(
                                        (option: CITYDTO) => {   
                                            if(option.id !== 0){
                                             return (
                                                <MenuItem key={option.id} 
                                                              value={option.id}
                                                >
                                                   {option.nombre}
                                                </MenuItem>
                                              )
                                            }
                                            return null;
                                        }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="email"
                            control={formularioConstructora.control}
                            rules={{
                                pattern:
                                    /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Email *"
                                    error={!!error}
                                    helperText={
                                        error
                                            ? 'Ingrese un correo electrónico válido'
                                            : null
                                    }
                                    isReadOnly={false}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container> 
            <Grid className={classes.columnRight} item xs={12} md={12}>
                <Button
                    className={classes.button}
                    variant="contained"
                    color="primary"
                    style={{ width: '250px' }}
                    onClick={formularioConstructora.handleSubmit(agregaryActualizarConstructora)}
                >
                   {estadoBoton}
                </Button>
                <Button
                    className={classes.button}
                    variant="contained"
                    color="default"
                    style={{ width: '200px' }}
                    onClick={cancelar}
                >
                    CANCELAR
                </Button>
             </Grid>
            </Grid>
          </CHPaper>
          <ConstructorasTable  
            constructoras={constructoras}
            catalogState={catalogState}
            seleccionarConstructora={seleccionarConstructora}
            eliminarConstructora={eliminarConstructora}
          />
          <Grid container>
                <Grid className={classes.columnButtonAdd} item xs={12} md={12} >
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        style={{ width: '130px' }}
                        onClick={regresarInicio}
                    >
                        REGRESAR
                    </Button>
                </Grid>
            </Grid>
          </CHPrivateContainer>
        </>
    )
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    setOpenConstructoras: (value: boolean) => void;
    startGetAllDepartments: (claims: string) => void;
    startGetAllCities: () => void;
}

const mapDispatchToProps = {
    startGetAllDepartments,
    startGetAllCities
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ConstructoraProyecto);