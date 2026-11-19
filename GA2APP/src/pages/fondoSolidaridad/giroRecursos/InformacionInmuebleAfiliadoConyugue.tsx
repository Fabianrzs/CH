import { createStyles,
         Button, 
         FormControlLabel, 
         Grid, 
         IconButton, 
         makeStyles,
         MenuItem, 
         Paper, 
         Radio, 
         RadioGroup, 
         Table, 
         TableBody, 
         TableCell, 
         TableContainer, 
         TableHead, 
         TableRow} from '@material-ui/core';
import { Delete, Edit, Search } from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import AddressComponent from '../../../components/modals/address/address.component';
import { MatriculaInmobiliariaModel } from '../../../models/matricula-inmobiliaria.model';
import { CITYDTO, DEPARTAMENTODTO, IDENTIFICATIONTYPEDTO, OFICINASREGISTRODTO } from '../../../types/catalog.type';
import ApiCatalogo from '../../../services/api-catalog.service';
import { Address } from '../../../types/address.type';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import GenerarGuid from '../../AdministracionCuentas/components/funciones/guid.funtions';
import NotificationService from '../../../services/notification.service';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import { startGetAllDepartments, startGetAllIdentificationTypes } from '../../../actions/catalog.action';

const useStyles = makeStyles(() =>
	createStyles({
		columnButtonAdd: {
			textAlign: 'right',
		},
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnLabel: {
			padding: '10px',
		},
		bloqueoMatricula: {
			color: 'Red'
		},
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        selectStyle: {
            marginTop: "15px"
        },
        typeLabel: {
            fontSize: '1rem',
            fontFamily: 'OpenSans-Regular'
        }
	}),
);

const InformacionInmuebleAfiliadoConyugue = (props: Props) => {
    const classes = useStyles();
    const apiCatalogo = ApiCatalogo();
    const {formulario, 
        catalogState, 
        loginState, 
        startGetAllDepartments,
        startGetAllIdentificationTypes,
        setMatriculasInmobiliarias}= props;

    const [listaOficinas, setListaOficinas]= useState([]);
    const [oficinaRegistroValue, setOficinaRegistroValue] = useState<OFICINASREGISTRODTO | null>(null);
    const [nullOficinaValue, setNullOficinaValue] = useState(false);
    const [open, setOpen] = useState(false);
    const [ciudades,setCiudades] = useState<CITYDTO[]>([]);

    const [matriculasInmobiliariasModel, setMatriculasInmobiliariasModel] = useState<MatriculaInmobiliariaModel[]>([]);
    const [matriculaAnterior, setmatriculaAnterior] = useState<string>('');
    const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR MATRÍCULA');
    const [idAnterior, setIdAnterior] = useState<string>('');
    const [matriculaBloqueada, setMatriculaBloqueada] = useState(false);

    const [mostrarFormularioConyugue,setMostrarFormularioConyugue] = useState(false);

    useEffect(() => {
        consultarOficinasInmueble();
        startGetAllDepartments(loginState.claims);
        startGetAllIdentificationTypes(loginState.claims);
    },[])

    const consultarOficinasInmueble = async() => {
        await apiCatalogo.getRegistrationOffice()
        .then(async response => {
            if(response.data){
                setListaOficinas(response.data);
            }
        })
        .catch(() => {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		});
    } 

    const onchangeOficinaRegistro = ( evento: any, newValue: OFICINASREGISTRODTO) => {
        setNullOficinaValue(newValue == null);
        setOficinaRegistroValue(newValue);
        if (newValue) {
            formulario.setValue('oficinaRegistro',String(newValue?.idOficina));
            formulario.setValue('codigoOficina',newValue?.codigoOficina);
        } 
	};

    const handleOpenAddress = () => {
        setOpen(true);
    };

    const onCloseAddress = (value: Address) => {

        if (value !== null) {
            const result = setFormDataAddres(value);
            formulario.setValue('direccion', result);
            setOpen(false);
        } else {
            formulario.setValue('direccion', '');
            setOpen(false);
        }
    };

    const obtenerCiudades = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalogo.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudades(datos);
        }
    };

    const agregarMatricula = async (data: any) => {
		const datoEncontrado = matriculasInmobiliariasModel.findIndex(
			x =>
				x.numeroMatricula === data.numeroMatricula &&
				x.numeroMatricula !== matriculaAnterior,
		);
		if (datoEncontrado === -1) {
			const existePrincipal = matriculasInmobiliariasModel.findIndex(
				x =>
					x.esPrincipal === true &&
					data.esPrincipal === true &&
					x.numeroMatricula !== matriculaAnterior,
			);
			if (existePrincipal === -1 || descEstadoBoton === 'EDITAR MATRICULA') {
				if (matriculasInmobiliariasModel.length === 0) {
					if (data.esPrincipal === false) {
						toast(
							'El primer registro debe ser matrícula principal',
							{ type: toast.TYPE.ERROR },
						);
					} else {
						await agregarRegistroMatricula(data);
					}
				} else {
					await agregarRegistroMatricula(data);
				}
			} else {
				toast('Ya existe una matrícula como principal', {
					type: toast.TYPE.ERROR,
				});
			}
		} else {
			toast('El número de matrícula ya existe en la lista', {
				type: toast.TYPE.ERROR,
			});
		}
	};

    const agregarRegistroMatricula = async (data: any) => {
        const nombreDepartamento = catalogState.departments.find((item: DEPARTAMENTODTO) => item.id === data.departamentoId);
        const nombreCiudad = ciudades.find(item => item.id === data.ciudadId);
		const guid = GenerarGuid();
		const matriculaNueva = {
			id: guid.newGuid(),
			esPrincipal: data.esPrincipal,
			numeroMatricula: `${data.codigoOficina}-${data.numeroMatricula}`,
			direccion: data.direccion.toUpperCase(),
			ciudadId: data.ciudadId,
			departamentoId: data.departamentoId,
			departamento: {
				id: data.departamentoId,
				descripcion: nombreDepartamento?.nombre || '',
			},
			ciudad: {
				id: data.ciudadId,
				descripcion: nombreCiudad?.nombre || '',
			},
			bloqueo: false,
			causalId: '98efdd13-2459-42fc-8787-6034f9935c35',
			tipoOperacionId: '38d1812e-eff8-48ea-9cc7-cdd0c159d0dd',
			oficinaRegistroId: data.oficinaRegistro,
			esBloqueada: matriculaBloqueada
		};
		let matriculas: MatriculaInmobiliariaModel[] = [];
		if (descEstadoBoton === 'AGREGAR MATRÍCULA') {
			setMatriculasInmobiliariasModel(oldArray => [
				...oldArray,
				matriculaNueva,
			]);
			matriculas = [...matriculasInmobiliariasModel];
			matriculas.push(matriculaNueva);
		} else {
			const temp = [...matriculasInmobiliariasModel];
			const posicionActual = temp.findIndex(
				x => x.numeroMatricula === matriculaAnterior,
			);
			temp.splice(posicionActual, 1);
			matriculaNueva.id = idAnterior;
			temp.splice(posicionActual, 0, matriculaNueva);
			matriculas = temp;
			setMatriculasInmobiliariasModel(temp);
		}
	    setMatriculasInmobiliarias(matriculas);
		setmatriculaAnterior('');
		resetMatricula();
	};

    const seleccionarRegistro = async (item: MatriculaInmobiliariaModel) => {
        setmatriculaAnterior(item.numeroMatricula);
        setIdAnterior(item.numeroMatricula);
        const numeroMatricula = item.numeroMatricula.split('-');
        const oficinaInmueble:any = listaOficinas.find((item:OFICINASREGISTRODTO)  => item.codigoOficina === numeroMatricula[0]);
        setNullOficinaValue(oficinaInmueble == null);
        setOficinaRegistroValue(oficinaInmueble || null);
        obtenerCiudades(item.departamentoId);
        formulario.setValue('id', item.id);
        if(oficinaInmueble){
            formulario.setValue('oficinaRegistro', String(oficinaInmueble?.idOficina));
        }
        formulario.setValue('codigoOficina', numeroMatricula[0]);
        formulario.setValue('numeroMatricula', numeroMatricula[1]);
        formulario.setValue('direccion', item.direccion);
        formulario.setValue('departamentoId', String(item.departamentoId));
        formulario.setValue('ciudadId', String(item.ciudadId));
        formulario.setValue('esPrincipal', item.esPrincipal);
        setDescEstadoBoton('EDITAR MATRÍCULA');
    };

    const eliminarRegistro = async (item: string) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...matriculasInmobiliariasModel];
			temp.splice(
				temp.findIndex(x => x.numeroMatricula === item),
				1,
			);
			setMatriculasInmobiliariasModel(temp);
			setMatriculasInmobiliarias(temp);
		}
	};

    const resetMatricula = () => {
		setDescEstadoBoton('AGREGAR MATRÍCULA');
        setOficinaRegistroValue(null);
		setMatriculaBloqueada(false);
		formulario.reset();
	};

    return (
        <>
          <AddressComponent
                onOpen={open}
                onClose={onCloseAddress}
                control={formulario.control}
                getValues={formulario.watch}
                tipoDireccion
                setValue={formulario.setValue}
          />
          <CHPaper title="Información del Inmueble">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="numeroMatriculaBuscar"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Número de matrícula a buscar"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                    <Button
                                        color="secondary"
                                        // onClick={() => {
                                        //     buscarMatricula(value);
                                        // }}
                                    >
                                        <Search />
                                    </Button>
                                </>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                          <Controller
                              name="oficinaRegistro"
                              control={formulario.control}
                              rules={{
                                  required:
                                      'Este campo es requerido.',
                              }}
                              render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={nullOficinaValue? null: oficinaRegistroValue}
                                    options={listaOficinas}
                                    getOptionLabel={option => option.descripcionOficina}
                                    onChange={(
                                        evento: any,
                                        oficina: any,
                                    ) => {
                                        onchangeOficinaRegistro(
                                            evento,
                                            oficina,
                                        );
                                    }}
                                    renderInput={params => (
                                        <CHTextField
                                            {...params}
                                            error={!!error}
                                            helperText={
                                                error
                                                    ? error.message
                                                    : null
                                            }
                                            InputLabelProps={{
                                                shrink: true,
                                            }}
                                            label="Oficina del Inmueble *"
                                        />
                                    )}
                                />
                              )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="codigoOficina"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly
                                        label="Código Oficina"
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroMatricula"
                            control={formulario.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        label="Matrícula Inmobiliaria *"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={e => {
                                            if(e.target.value.length <= 13){
                                                onChange(
                                                    e.target.value.replace(
                                                        /[^A-Z\d-]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        }}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="direccion"
                            control={formulario.control}
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
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="departamentoId"
                                control={formulario.control}
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
                                name="ciudadId"
                                control={formulario.control}
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
                                    label="Ciudad del Inmueble *"
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
                <Grid className={classes.column} item xs={12} md={4}>
                           <Controller
                                name="propietario"
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <RadioGroup
                                        value={value}
                                        onChange={ e => {
                                            if(e.target.value === 'CONYUGUE'){
                                                setMostrarFormularioConyugue(true);
                                            }else{
                                                setMostrarFormularioConyugue(false);
                                            }
                                            onChange(e);
                                        }}
                                        row
                                        defaultValue="0"
                                        name="row-radio-buttons-group"
                                    >
                                        <FormControlLabel
                                            value="AFILIADO"
                                            control={
                                                <Radio color='primary' size='small'/>
                                            }
                                            label="Afiliado"
                                        />
                                        <FormControlLabel
                                            value="CONYUGUE"
                                            control={
                                                <Radio color='primary' size='small'/>
                                            }
                                            label="Cónyugue"
                                        />
                                    </RadioGroup>
                                )}
                            />
                        </Grid>
                    </Grid>
                 <br/>
                {
                    mostrarFormularioConyugue && (
                        <CHPaper title="Información del Cónyugue (Si Aplica)">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="tipoDocumentoConyugueId"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tipo documento *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                >
                                    {catalogState?.IdentificationType.map(
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
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroDocumentoConyugue"
                            rules={{
                                required: 'Este campo es requerido.',
                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 digito',
                                },
                            }}
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                  //  numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Documento *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
										if(e.target.value.length <= 12){
											onChange(e);
										}
									}}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="nombresConyugue"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombres *"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '')
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                  </Grid>
                </CHPaper>
                )}
                <Grid container>
                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={formulario.handleSubmit(
                                agregarMatricula,
                            )}
                        >
                            {descEstadoBoton}
                        </Button>
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetMatricula}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid> 
                <h3>Lista de matrículas inmobiliarias </h3>
				<TableContainer component={Paper}>
				<Grid className={classes.columnButtonAdd} item xs={12}md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>Número Matrícula</TableCell>
									<TableCell>Estado</TableCell>
									<TableCell>Dirección</TableCell>
									<TableCell>Departamento</TableCell>
									<TableCell>Ciudad</TableCell>
									<TableCell align="center">
										Acciones
									</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{matriculasInmobiliariasModel.map(
									(option: MatriculaInmobiliariaModel) => (
										<TableRow key={option.numeroMatricula}>
											<TableCell>
												<span>{option.numeroMatricula}</span>  
											</TableCell>
											<TableCell>
												{option.esPrincipal ? 'PRINCIPAL' : 'RELACIONADO'}
											</TableCell>
											<TableCell>
												{option.direccion}
											</TableCell>
											<TableCell>
												{option.departamento.descripcion}
											</TableCell>
											<TableCell>
												{option.ciudad.descripcion}
											</TableCell>
											<TableCell align="center">
                                                <IconButton
                                                    aria-label="Editar"
                                                    color="primary"
                                                    onClick={() => {
                                                    	seleccionarRegistro(
                                                    		option,
                                                    	);
                                                    }}
                                                >
                                                    <Edit />
                                                </IconButton> 
                                                <IconButton
                                                    aria-label="Eliminar"
                                                    color="secondary"
                                                    onClick={() => {
                                                        eliminarRegistro(
                                                            option.numeroMatricula,
                                                        );
                                                    }}
                                                >
                                                    <Delete />
                                                </IconButton> 
											</TableCell>
										</TableRow>
									),
								)}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>
          </CHPaper>
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
    formulario:  UseFormReturn<any>;
    setMatriculasInmobiliarias: (value: MatriculaInmobiliariaModel[]) => void;
    startGetAllDepartments: (claims: string) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllDepartments,
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(InformacionInmuebleAfiliadoConyugue);