

import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import {
	createStyles,
	Grid,
	makeStyles,
	TableContainer,
	Table,
	Paper,
	TableHead,
	TableRow,
	TableCell,
	TableBody,
	IconButton,
	Button,
	MenuItem,
	createMuiTheme,
	MuiThemeProvider,
	createTheme,
	FormLabel,
	RadioGroup,
	FormControlLabel,
	Radio,
} from '@material-ui/core';
import { Edit, Delete, Search } from '@material-ui/icons';
import { toast } from 'react-toastify';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { MatriculaInmobiliariaModel } from '../../../../models/matricula-inmobiliaria.model';
import NotificationService from '../../../../services/notification.service';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import { DEPARTAMENTODTO, CITYDTO, OFICINASREGISTRODTO } from '../../../../types/catalog.type';
import { Address } from '../../../../types/address.type';
import {
	startGetAllCities,
	startGetAllDepartments,
	startGetRegistrationOffice,
} from '../../../../actions/catalog.action';
import { AppState } from '../../../../store/rootReducer';
import { RealStateRegistrationsState } from '../../../../reducers/realStateRegistrations.reducer';
import GenerarGuid from '../funciones/guid.funtions';

import {
	startGetOperacionesBloqueos,
	startGetBloqueosById,
} from '../../../../actions/realStateRegistrations.action';
import ApiRealState from '../../../../services/api-realstateregistrations.service';
import ApiSolicitudes from '../../../../services/api-solicitud.service';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import AddressComponent from '../../../../components/modals/address/address.component';
import { setFormDataAddres } from '../../../../components/modals/address/constants/address.components.constants';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { ModalMatriculaInmobiliaria } from '../matricula-inmobiliaria/modal-matriculaInmobiliaria-bloqueo-desbloqueo';

const useStyles = makeStyles(() =>
	createStyles({
		columnButtonAdd: {
			textAlign: 'right',
		},
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
		},
		column2: {
			padding: '10px',
			textAlign: 'left',
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
		}
	}),
);

function MatriculaInmobiliariaNueva({
	title,
	formulario,
	listaMatriculas,
	listaDepartamentos,
	listaCiudades,
	listaOficinas,
	realState,
	matriculasAdicionadas,
	obtenerOperacionesBloqueos,
	obtenerCausalesPorId,
	responseSolicitud,
	conyugeActivo = false,
	onChangePropietario,



}: {
	title: string,
	formulario: UseFormReturn<any>,
	listaMatriculas: MatriculaInmobiliariaModel[];
	listaDepartamentos: DEPARTAMENTODTO[];
	listaCiudades: CITYDTO[];
	listaOficinas: OFICINASREGISTRODTO[];
	realState: RealStateRegistrationsState;
	matriculasAdicionadas: (matriculas: MatriculaInmobiliariaModel[]) => void;
	obtenerOperacionesBloqueos: () => void;
	obtenerCausalesPorId: (typeIdOperation: string) => void;
	responseSolicitud: ResponseSeveranceModel | undefined;
	conyugeActivo?: boolean,
	onChangePropietario: (id: string) => void,

}) {
	const classes = useStyles();
	const api = ApiRealState();
	const apiSolicitud = ApiSolicitudes();
	const [matriculaInmobiliariaModel, setMatriculaInmobiliariaModel] = useState<MatriculaInmobiliariaModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR MATRÍCULA');
	const [matriculaAnterior, setmatriculaAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [departamentos, setDepartamentos] = useState<DEPARTAMENTODTO[]>([]);
	const [ciudadesFilter, setCiudadesFilter] = useState<CITYDTO[]>([]) || undefined;
	const [ciudades, setCiudades] = useState<boolean>(false);
	const [openModal, setOpenModal] = useState(false);
	const [opcion, setOpcion] = useState(false);
	const [causal, setCausal] = useState('');
	const [existeSolicitud, setExisteSolicitud] = useState(false);
	const [datosMatricula, setDatosMatricula] = useState<any>([]);
	const [readOnly, setReadOnly] = useState(true);
	const [disabledOffice, setDisabledOffice] = useState(true);
	const [disabled, setDisabled] = useState(true);
	const [disabledDireccion, setDisabledDireccion] = useState(true);
	const [idDepto, setIdDepto] = useState('');
	const [oficinaRegistro, setOficinaRegistro] = useState<OFICINASREGISTRODTO | null>(null);
	const [ciudadRegistro, setCiudadRegistro] = useState<CITYDTO | null>(null);
	const [openAddressForm, setOpenAddressForm] = useState(false);
	const [esBloqueada, setEsBloqueada] = useState(false);
	const [matriculaBloqueo, setMatriculaBloqueo] = useState({
		id: '',
		numeroMatricula: '',
		esPrincipal: false,
		direccion: '',
		ciudadId: 0,
		departamentoId: 0,
		departamento: '',
		ciudad: '',
		bloqueo: false,
		causalId: '',
		tipoOperacionId: '',
	});


	const [buttonDisabled, setButtonDisabled] = useState<boolean>(false);

	const matriculasControl = useForm({
		defaultValues: {
			departamentoInmueble: '',
			ciudadInmueble: '',
			numeroMatricula: '',
			direccion: '',
			esPrincipal: false,
			bloqueo: false,
			causalId: '',
			tipoOperacionId: '',
			numeroMatriculaBuscar: '',
			oficinaInmueble: '',
			codigoOficina: ''
		},
	});

	const { control, handleSubmit, reset } = useForm({
		defaultValues: {
			causalId: '',
		},
	});

	useEffect(() => {
		if (listaMatriculas) {
			if (listaMatriculas.length > 0) {
				setMatriculaInmobiliariaModel(listaMatriculas);
			}
			setExisteSolicitud(true);
		} else {
			setExisteSolicitud(false);
		}
	}, []);

	useEffect(() => {
		if (datosMatricula?.dpdId) {
			setIdDepto(datosMatricula?.dpdId);
		}

		getCitys(datosMatricula?.dpdId);

		if (datosMatricula?.maiNumeroMatricula?.indexOf('-') > 0) {
			const matriculaCompuesta = datosMatricula?.maiNumeroMatricula.split('-');
			matriculasControl.setValue('numeroMatricula', matriculaCompuesta[1]);
			const oficina = listaOficinas?.find(or => or.codigoOficina === matriculaCompuesta[0]);
			if (oficina) {
				setOficinaRegistro(oficina);
				matriculasControl.setValue('oficinaInmueble', oficina?.idOficina.toString());
				matriculasControl.setValue('codigoOficina', oficina?.codigoOficina);
			}
		} else {
			matriculasControl.setValue('numeroMatricula', datosMatricula?.maiNumeroMatricula);
		}
		matriculasControl.setValue('direccion', datosMatricula?.maiDireccion);
		const departamento = departamentos.find(
			item => item.id === datosMatricula?.dpdId,
		);
		matriculasControl.setValue(
			'departamentoInmueble',
			`${datosMatricula?.dpdId}_${departamento?.nombre}`,
		);

		const ciudadDepartamento = listaCiudades.find(
			item => item.idDepartamento === datosMatricula?.dpdId && item.id === datosMatricula?.dpcId,
		);

		matriculasControl.setValue(
			'ciudadInmueble',
			`${datosMatricula?.dpcId}_${ciudadDepartamento?.nombre}`,
		);
		const ciudad = listaCiudades.find(x => x.codigo.toString() === datosMatricula?.dpcId?.toString() || '');
		if (ciudad) {
			setCiudadRegistro(ciudad as CITYDTO);
		}
		matriculasControl.setValue('esPrincipal', datosMatricula?.maiPrincipal);
	}, [datosMatricula]);

	const formLabelsTheme = createTheme({
		overrides: {
			MuiFormLabel: {
				asterisk: {
					color: '#db3131',
					'&$error': {
						color: '#db3131',
					},
				},
			},
		},
	});

	const seleccionarRegistro = async (item: MatriculaInmobiliariaModel) => {
		setIdAnterior(item.id);
		setDepartamentos(listaDepartamentos.filter(x => x.id.toString() === item.departamentoId.toString()));
		await getCitys(item.departamento.id);
		if (item?.numeroMatricula?.indexOf('-') > 0) {
			const numMatricula = item.numeroMatricula.split('-');
			matriculasControl.setValue('numeroMatricula', numMatricula[1]);
			setmatriculaAnterior(numMatricula[1]);
			const office = listaOficinas?.find(o => o.codigoOficina === numMatricula[0]);
			if (office) {
				setOficinaRegistro(office);
				matriculasControl.setValue('oficinaInmueble', office?.idOficina.toString());
				matriculasControl.setValue('codigoOficina', office?.codigoOficina);
			}
		} else {
			matriculasControl.setValue('numeroMatricula', item.numeroMatricula);
			setmatriculaAnterior(item.numeroMatricula);
		}
		matriculasControl.setValue('direccion', item.direccion);
		matriculasControl.setValue('esPrincipal', item.esPrincipal);
		matriculasControl.setValue('departamentoInmueble', `${item.departamento.id}_${item.departamento.descripcion}`);
		matriculasControl.setValue('ciudadInmueble', `${item.ciudad.id}_${item.ciudad.descripcion}`);
		const ciudad = listaCiudades.find(x => x.id.toString() === item.ciudad.id.toString());
		setCiudadRegistro(ciudad as CITYDTO);
		setDescEstadoBoton('EDITAR MATRICULA');
	};

	const getCitys = async (idDeptartamento: any) => {
		const ciudad = listaCiudades?.filter(
			item => item.idDepartamento.toString() === (idDeptartamento === undefined ? '' : idDeptartamento.toString())
		);
		setCiudadesFilter(ciudad);
	}

	const eliminarRegistro = async (item: string) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...matriculaInmobiliariaModel];
			temp.splice(
				temp.findIndex(x => x.numeroMatricula === item),
				1,
			);
			setMatriculaInmobiliariaModel(temp);
			matriculasAdicionadas(temp);
		}
	};

	const resetearMatricula = () => {

		setDescEstadoBoton('AGREGAR MATRÍCULA');
		setOficinaRegistro(null);
		setCiudadRegistro(null);
		setDisabledDireccion(true);
		setEsBloqueada(false);
		matriculasControl.reset();
	};

	const agregarMatricula = async (data: any) => {

		setButtonDisabled(true);
		const datoEncontrado = matriculaInmobiliariaModel.findIndex(
			x =>
				x.numeroMatricula === data.numeroMatricula &&
				x.numeroMatricula !== matriculaAnterior,
		);
		if (datoEncontrado === -1) {
			const existePrincipal = matriculaInmobiliariaModel.findIndex(
				x =>
					x.esPrincipal === true &&
					data.esPrincipal === true &&
					x.numeroMatricula !== matriculaAnterior,
			);
			if (existePrincipal === -1 || descEstadoBoton === 'EDITAR MATRICULA') {
				if (matriculaInmobiliariaModel.length === 0) {
					if (data.esPrincipal === false) {
						toast(
							'El primer registro debe ser matrícula principal',
							{ type: toast.TYPE.ERROR },
						);
					} else {
						await agregarRegistromatricula(data);
					}
				} else {
					await agregarRegistromatricula(data);
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
		setButtonDisabled(false);
	};

	const agregarRegistromatriculaBloqueo = async (
		data: any,
		causal: string,
	) => {
		const array = await matriculaInmobiliariaModel.map(item => {
			if (item.id === data.id) {
				return {
					...item,
					bloqueo: data.bloqueo as boolean,
					causalId: causal,
					tipoOperacionId: data.tipoOperacionId as string,
				};
			}
			return item;
		});
		await setMatriculaInmobiliariaModel(array);
		const request = {
			matriculaId: data.id as string,
			tipoOperacionId: data.tipoOperacionId as string,
			causalId: causal as string,
		};
		await insertarNovedadMatricula(request);
	};

	const insertarNovedadMatricula = async (request: any) => {
		await api
			.AddNoveltyBlocks(request)
			.then(async response => {
				if (response && response.isSuccess) {
					NotificationService.genericAlert(
						'Guardado',
						'Se ha guardado correctamente la novedad de matricula.',
					);
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

	const agregarRegistromatricula = async (data: any) => {
		// OficinaRegistroId
		const guid = GenerarGuid();
		const matriculaNueva = {
			id: guid.newGuid(),
			esPrincipal: data.esPrincipal,
			numeroMatricula: `${data.codigoOficina}-${data.numeroMatricula}`,
			direccion: data.direccion,
			ciudadId: data.ciudadInmueble.split('_')[0],
			departamentoId: data.departamentoInmueble.split('_')[0],
			departamento: {
				id: data.departamentoInmueble.split('_')[0],
				descripcion: data.departamentoInmueble.split('_')[1],
			},
			ciudad: {
				id: data.ciudadInmueble.split('_')[0],
				descripcion: data.ciudadInmueble.split('_')[1],
			},
			bloqueo: false,
			causalId: '98efdd13-2459-42fc-8787-6034f9935c35',
			tipoOperacionId: '38d1812e-eff8-48ea-9cc7-cdd0c159d0dd',
			oficinaRegistroId: data.oficinaInmueble,
			esBloqueada: esBloqueada
		};
		let matriculas: MatriculaInmobiliariaModel[] = [];
		if (descEstadoBoton === 'AGREGAR MATRÍCULA') {
			setMatriculaInmobiliariaModel(oldArray => [
				...oldArray,
				matriculaNueva,
			]);
			matriculas = [...matriculaInmobiliariaModel];
			matriculas.push(matriculaNueva);
		} else {
			const temp = [...matriculaInmobiliariaModel];
			const posicionActual = temp.findIndex(
				x => x.numeroMatricula === matriculaAnterior,
			);
			temp.splice(posicionActual, 1);
			matriculaNueva.id = idAnterior;
			temp.splice(posicionActual, 0, matriculaNueva);
			matriculas = temp;
			setMatriculaInmobiliariaModel(temp);
		}


		matriculasAdicionadas(matriculas);
		setmatriculaAnterior('');
		resetearMatricula();
	};

	const handleOpenModal = (matriculaSelected: MatriculaInmobiliariaModel) => {
		obtenerOperacionesBloqueos();
		if (matriculaSelected.bloqueo) {
			const tipoOperacion = realState.operacionesBloqueos?.find(
				item => item.nombre === 'Desbloqueo',
			);
			setOpcion(false);
			const tipoId = tipoOperacion !== undefined ? tipoOperacion.id : '';
			obtenerCausalesPorId(tipoId);
			setMatriculaBloqueo({
				...matriculaBloqueo,
				id: matriculaSelected.id,
				numeroMatricula: matriculaSelected.numeroMatricula,
				direccion: matriculaSelected.direccion,
				esPrincipal: matriculaSelected.esPrincipal,
				ciudadId: matriculaSelected.ciudadId,
				departamentoId: matriculaSelected.departamentoId,
				departamento: `${matriculaSelected.departamento.id}_${matriculaSelected.departamento.descripcion}`,
				ciudad: `${matriculaSelected.ciudad.id}_${matriculaSelected.ciudad.descripcion}`,
				bloqueo: false,
				causalId: '',
				tipoOperacionId: tipoId,
			});
		} else {
			const tipoOperacion = realState.operacionesBloqueos?.find(
				item => item.nombre === 'Bloqueo',
			);
			setOpcion(true);
			const tipoId = tipoOperacion !== undefined ? tipoOperacion.id : '';
			obtenerCausalesPorId(tipoId);
			setMatriculaBloqueo({
				...matriculaBloqueo,
				id: matriculaSelected.id,
				numeroMatricula: matriculaSelected.numeroMatricula,
				direccion: matriculaSelected.direccion,
				ciudadId: matriculaSelected.ciudadId,
				departamentoId: matriculaSelected.departamentoId,
				esPrincipal: matriculaSelected.esPrincipal,
				departamento: `${matriculaSelected.departamento.id}_${matriculaSelected.departamento.descripcion}`,
				ciudad: `${matriculaSelected.ciudad.id}_${matriculaSelected.ciudad.descripcion}`,
				bloqueo: true,
				causalId: '',
				tipoOperacionId: tipoId,
			});
		}
		setOpenModal(true);
	};

	const handleCloseModal = () => {
		setOpenModal(false);
		setOpcion(false);
		reset();
		setMatriculaBloqueo({
			id: '',
			numeroMatricula: '',
			esPrincipal: false,
			direccion: '',
			ciudadId: 0,
			departamentoId: 0,
			departamento: '',
			ciudad: '',
			bloqueo: false,
			causalId: '',
			tipoOperacionId: '',
		});
	};

	const hadlOnChangeCausal = (value: any) => {
		const causal = value as string;
		setCausal(causal);
	};

	const handleInsertarNovedad = () => {
		agregarRegistromatriculaBloqueo(matriculaBloqueo, causal);
		handleCloseModal();
	};

	const buscarMatricula = async (numeroMatricula: string) => {
		resetearMatricula();
		await apiSolicitud
			.GetConsultarExisteMatricula(numeroMatricula)
			.then(async response => {
				if (response) {
					if (response?.isSuccess) {
						if (response?.data?.maiEstado) {

							setEsBloqueada(response?.data?.maiBloqueo);

							if (response?.data?.maiBloqueo) {
								// toast('Advertencia: Matricula bloqueada', { type: toast.TYPE.WARNING });
								NotificationService.genericAlwarning(
									'Alerta',
									`Advertencia: Matricula bloqueada`,
									'warning',)
							}

							setCiudades(true);
							setReadOnly(true);
							setDisabledOffice(true);


							setDepartamentos(listaDepartamentos.filter(x => x.id.toString() === response?.data?.dpdId?.toString() || ''));
							await getCitys(response?.data?.dpdId);
							matriculasControl.setValue(
								'departamentoInmueble',
								`${response?.data?.dpdId}_${response?.data?.dpdNombre}`,
							);

							matriculasControl.setValue('direccion', response?.data?.maiDireccion);
							if (response?.data?.maiNumeroMatricula?.indexOf('-') > 0) {
								const numMatricula = response?.data?.maiNumeroMatricula.split('-');
								matriculasControl.setValue('numeroMatricula', numMatricula[1]);
							} else {
								matriculasControl.setValue('numeroMatricula', response?.data?.maiNumeroMatricula);
							}

							const office = listaOficinas?.find(o => o.codigoOficina === response?.data?.ofcCodigo);
							if (office) {
								setOficinaRegistro(office);
								matriculasControl.setValue('oficinaInmueble', office?.idOficina.toString());
								matriculasControl.setValue('codigoOficina', office?.codigoOficina);
							}

							const ciudad = listaCiudades.find(x => x.id.toString() === response?.data?.dpcId.toString());
							setCiudadRegistro(ciudad as CITYDTO);
							matriculasControl.setValue('ciudadInmueble', `${response?.data?.dpcId}_${response?.data?.dpcNombre}`);


						} else {
							// toast('Advertencia: Matricula inactiva', { type: toast.TYPE.WARNING });
							NotificationService.genericAlwarning(
								'Alerta',
								`Advertencia: Matricula inactiva`,
								'warning',)
						}
					} else {
						toast(`Advertencia: ${response.returnMessage}`, { type: toast.TYPE.WARNING });
						setCiudades(false);
						setOficinaRegistro(null);
						resetearMatricula();
						setReadOnly(false);
						setDisabledOffice(false);
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

	const onchangeOffice = (_: any, newValue: OFICINASREGISTRODTO) => {
		setCiudadRegistro(null);
		setOficinaRegistro(newValue);
		matriculasControl.setValue('oficinaInmueble', newValue?.idOficina.toString());
		matriculasControl.setValue('codigoOficina', newValue?.codigoOficina);
		matriculasControl.resetField('departamentoInmueble');
		setDepartamentos(listaDepartamentos.filter(item => item.id === newValue?.codigoDepartamento));
		setDisabled(false);
		setDisabledDireccion(false);
	};

	const onchangeCiudad = (_: any, newValue: CITYDTO) => {
		matriculasControl.setValue('ciudadInmueble', `${newValue?.id.toString()}_${newValue?.nombre}`);
		setCiudadRegistro(newValue);

	};


	const onOpenAddressForm = () => {
		setOpenAddressForm(true);
	};

	const onCloseAddressForm = (value: Address) => {
		if (value !== null) {
			const result = setFormDataAddres(value);
			matriculasControl.setValue('direccion', result);
			setOpenAddressForm(false);
		} else {
			matriculasControl.setValue('direccion', '');
			setOpenAddressForm(false);
		}
	};

	return (
		<>
			<CHPaper title={title}>
				{/* {
					responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' || responseSolicitud?.timCodigo !== 'LIBERGRAV' && responseSolicitud?.solicitudTareaEstadoId === 327 ?




						<Grid container>
							<Grid className={classes.column} item xs={12} md={12}>
								<Controller
									name="numeroMatriculaBuscar"
									control={matriculasControl.control}
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
												onChange={onChange}
											/>
											<Button
												aria-label="Eliminar"
												color="secondary"
												onClick={() => {
													buscarMatricula(value);
												}}
											>
												<Search />
											</Button>
										</>
									)}
								/>
							</Grid>
						</Grid> : ''
				} */}

				{
					responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?




						<Grid container>
							<Grid className={classes.column} item xs={12} md={2}>
								<Controller
									name="oficinaInmueble"
									rules={{
										required: 'Este campo es requerido',
									}}
									control={matriculasControl.control}
									render={(
										{
											field: { onChange, value },
											fieldState: { error },
										}) => (
										<CHAutoComplete
											// disabled={disabledOffice}
											value={oficinaRegistro}
											options={listaOficinas}
											getOptionLabel={option => option?.descripcionOficina}
											onChange={onchangeOffice}
											renderInput={(params) =>
												<MuiThemeProvider theme={formLabelsTheme}>
													<CHTextField
														error={!!error}
														helperText={error ? error.message : null}
														{...params}
														required
														label="Oficina del inmueble"
													/>
												</MuiThemeProvider>
											}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={1}>
								<Controller
									name="codigoOficina"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHTextField
												error={!!error}
												helperText={error ? error.message : null}
												disabled
												label="Código oficina"
												required
												value={value}
											/>
										</MuiThemeProvider>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={2}>
								<Controller
									name="numeroMatricula"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
										maxLength: 13
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHTextField
												error={!!error}
												helperText={error ? error.message : null}
												inputProps={{
													maxLength: 11,
												}}
												// isReadOnly={readOnly}
												// disabled={disabled}
												label="Matrícula Inmobiliaria"
												value={value}
												required
												onChange={e => {
													onChange(
														e.target.value.replace(
															/[^A-Z\d-]/g,
															'',
														),
													);
												}}
											/>
										</MuiThemeProvider>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="direccion"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									defaultValue=""
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										
											<CHTextField
												label="Dirección del inmueble "
												error={!!error}
												helperText={error ? error.message : null}
												isReadOnly={disabledDireccion}
												disabled={disabledDireccion}
												value={value}
												required={matriculasControl.getValues('direccion') === ''}
												onChange={onChange}
												onClick={() => {
													if (!disabled) {
														onOpenAddressForm();
													}
												}}
											/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={2}>
								<Controller
									name="departamentoInmueble"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHSelect
												error={!!error}
												label="Departamento del Inmueble *"
												value={value}
												required
												// readOnly={readOnly}
												// disabled={disabled}
												helperText={error ? error.message : null}
												onChange={value => {
													const dato = value.target
														.value as string;
													const idDepartamento =
														dato.split('_')[0];
													matriculasControl.setValue(
														'ciudadInmueble',
														'',
													);
													if (idDepartamento !== '0') {
														getCitys(idDepartamento);
														setCiudades(true);
													} else {
														setCiudades(false);
													}

													onChange(value);
												}}
											>
												{departamentos
													.filter(item => item.id !== 0)
													?.map((option: DEPARTAMENTODTO) => {
														const dato = `${option.id}_${option.nombre}`;
														return (
															<MenuItem
																key={option.id}
																value={dato}
															>
																{option.nombre}
															</MenuItem>
														);
													})}
											</CHSelect>
										</MuiThemeProvider>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={2}>
								<Controller
									name="ciudadInmueble"

									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHAutoComplete
											value={ciudadRegistro}
											options={ciudadesFilter}
											onChange={onchangeCiudad}
											disabled={disabled}
											getOptionLabel={option => option?.nombre}
											renderInput={(params) =>
												<MuiThemeProvider theme={formLabelsTheme}>
													<CHTextField
														error={!!error}
														helperText={error ? error.message : null}
														{...params}
														required
														label="Ciudad del Inmueble "
														disabled={disabled}
													/>
												</MuiThemeProvider>
											}
										/>
									)}
								/>
							</Grid>
						</Grid> : ''
				}
				{/* {
					responseSolicitud?.timCodigo !== 'LIBERGRAV' &&
					<Grid container>
						<Grid className={classes.columnLabel} item xs={12} md={3}>
							<FormLabel component='legend'>Propietario</FormLabel>
							<Controller
								name='propietario'
								control={matriculasControl.control}
								render={({ field: { onChange } }) => (
									<RadioGroup
										onChange={(value) => {
											const id = value.target.value as string;
											onChange(id);
											onChangePropietario(id)

										}}
										row
										aria-label='propietario'
										defaultValue={conyugeActivo ? '137' : '87'}
										name='row-radio-buttons-group'>
										<FormControlLabel
											value='87'
											control={<Radio />}
											label='Afiliado'
										/>
										<FormControlLabel
											value='137'
											control={<Radio />}
											label='Cónyuge'
										/>
									</RadioGroup>
								)}
							/>
						</Grid>
					</Grid>
				} */}
				{
					(responseSolicitud?.timCodigo !== 'CONSNUEVA' &&
					 responseSolicitud?.timCodigo !== 'CSLGR')?

						<Grid container>
							<Grid className={classes.columnLabel} item xs={12} md={3}>
								<Controller
									name="esPrincipal"
									control={matriculasControl.control}
									render={({ field: { onChange, value } }) => (
										<CHCheckbox
											label="Principal / Relacionado"
											onChange={onChange}
											checked={value}
										/>
									)}
								/>
							</Grid>
						</Grid> : ''

				}
				{



					responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?


						<Grid container>
							<Grid
								className={classes.columnButtonAdd}
								item
								xs={12}
								md={12}
							>
								<Button
									className={classes.buttonAdd}
									variant="outlined"
									disabled={buttonDisabled}
									onClick={matriculasControl.handleSubmit(
										agregarMatricula,
									)}
								>
									{descEstadoBoton}
								</Button>
								<Button
									className={classes.buttonAdd}
									variant="outlined"
									disabled={buttonDisabled}
									onClick={resetearMatricula}
								>
									CANCELAR
								</Button>
							</Grid>
						</Grid> : ''
				}

				<h3>Lista de matrículas inmobiliarias </h3>
				<TableContainer component={Paper}>
					<Grid
						className={classes.columnButtonAdd}
						item
						xs={12}
						md={12}
					>
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
								{matriculaInmobiliariaModel.map(
									(option: MatriculaInmobiliariaModel) => (
										<TableRow key={option.numeroMatricula}>
											<TableCell>
												<span className={option.esBloqueada ? classes.bloqueoMatricula : ''}>{option.numeroMatricula}</span>  <span className={classes.bloqueoMatricula}>{option.esBloqueada ? ' -  (Bloqueada)' : ''}</span>
											</TableCell>
											<TableCell>
												{option.esPrincipal ? 'Principal' : 'Relacionado'}
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
												{
													responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?



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
														</IconButton> : ''
												}
												{
													responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?



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
														</IconButton> : ''
												}

											</TableCell>
										</TableRow>
									),
								)}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>
			</CHPaper>
			<ModalMatriculaInmobiliaria
				open={openModal}
				onClose={handleCloseModal}
				control={matriculasControl.control}
				opcion={opcion}
				listaCausales={realState.causalesPortipo}
				handleInsertarNovedad={handleInsertarNovedad}
				hadlOnChangeCausal={hadlOnChangeCausal}
				handleSubmit={matriculasControl.handleSubmit}
			/>
			<AddressComponent
				onOpen={openAddressForm}
				onClose={onCloseAddressForm}
				control={matriculasControl.control}
				getValues={matriculasControl.watch}
				tipoDireccion // is urban address by default
				setValue={matriculasControl.setValue}
			/>
		</>
	);
}

const mapStateToProps = (state: AppState) => ({
	realState: state.realStateRegistrations,
});

const mapDispatchToProps = (dispatch: any) => ({
	obtenerOperacionesBloqueos: () => dispatch(startGetOperacionesBloqueos()),
	obtenerCausalesPorId: (typeIdOperation: string) => dispatch(startGetBloqueosById(typeIdOperation)),
});

export default connect(mapStateToProps, mapDispatchToProps,)(MatriculaInmobiliariaNueva);
