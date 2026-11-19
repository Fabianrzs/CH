import React, { useEffect, useState } from 'react';
import {
	Button,
	createStyles,
	Grid,
	IconButton,
	makeStyles,
	MenuItem,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { Delete, Edit } from '@material-ui/icons';
import { toast } from 'react-toastify';
import SearchIcon from '@material-ui/icons/Search';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import { TerceroApoderadoModel } from '../../../../models/tercero-apoderado.model';
import GenerarGuid from '../funciones/guid.funtions';
import NotificationService from '../../../../services/notification.service';
import {
	IDENTIFICATIONTYPEDTO,
	VALORCATALOGO,
} from '../../../../types/catalog.type';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import SolicitudApi from '../../../../services/api-solicitud.service';
import CreditoApi from '../../../../services/api-credit.service';
import ApiClients from '../../../../services/api-clients.service';
import { PropietarioMatriculaModel } from '../../../../models/propietario-matricula.model';
import { PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION, USER_SARLAFT_FOUND } from '../../../../constants/constants';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { LoadingComponent } from '../../../../components/loading/loading.component';


const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnButtonAdd: {
			textAlign: 'right',
		},
		columnLabel: {
			padding: '10px',
		},
	}),
);

function TerceroApoderado({
	listaApoderado,
	listaTipoDocumentos,
	listaParentesco,
	tercerosApoderadoAdicionados,
	obtenerParentesco,
	responseSolicitud,
	siEsAprobadaSolcitud,
	setDatosBeneficiarios,
	campoSoloLecturaRadicacionAprobada,
	setApoderadosAutorizados
}: {
	listaApoderado: TerceroApoderadoModel[];
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
	listaParentesco: VALORCATALOGO[];
	tercerosApoderadoAdicionados: (terceros: TerceroApoderadoModel[]) => void;
	obtenerParentesco: (catalogoId: number) => void;
	responseSolicitud: ResponseSeveranceModel | undefined;
	siEsAprobadaSolcitud: boolean;
	setDatosBeneficiarios: (dato: any) => void;
	campoSoloLecturaRadicacionAprobada: any;
	setApoderadosAutorizados: (update: any) => void;
}) {

	const api = SolicitudApi();
	const apiCredito = CreditoApi();
	const apiDos = ApiClients();
	const classes = useStyles();
	const [terceroApoderadoModel, setTerceroApoderadoModel] = useState<TerceroApoderadoModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR APODERADO');
	const [numDocAnterior, setNumDocAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [encontrarIdentificacion, setEncontrarIdentificacion] = useState(false);
	const [loading, setLoading] = useState(false);
	const [hablitarCampoNumeroIdentidad, setHabilitarCampoNumeroIdentidad] = useState(true)
	const [hablitarLupaBusqueda, setHablitarLupaBusqueda] = useState(false)
	const [isloading, setIsLoading] = useState(false)
	const [bloquearTipoDoc, setBloquearTipoDoc] = useState(false)
	const [siExisteBloTipoNume, setSiExisteBloTipoNume] = useState(false)
	const [camposNombresReadOnly, setCamposNombresReadOnly] = useState(false);
	const [deshabilitarBoton, setDeshabilitarBoton] = useState(false);
	const [tipoIdentificacion, setTipoIdentificacion] = useState<any>('');

	const controlApoderado = useForm({
		defaultValues: {
			parentescoObj: '',
			tipoDocObj: '',
			numDoc: '',
			razonsocial: '',
			esAutorizado: false,
			numeroIdentificacionBuscar: '',
			primerNombre: '',
			segundoNombre: '',
			primerApellido: '',
			segundoApellido: '',
		},
	});

	const esAutorizadoWatch = controlApoderado.watch("esAutorizado");

	useEffect(() => {

		obtenerParentesco(3);
		if (listaApoderado) {
			if (listaApoderado.length > 0) {
				setTerceroApoderadoModel(listaApoderado);
			}
		}
	}, []);

	useEffect(() => {
		if ((controlApoderado.getValues('tipoDocObj') !== 'undefined_undefined' && controlApoderado.getValues('tipoDocObj') !== '')
			&& (controlApoderado.getValues('numDoc') === undefined || controlApoderado.getValues('numDoc') === '')) {
			setDatosBeneficiarios(true)
		} else {
			setDatosBeneficiarios(false)
		}
	}, [controlApoderado.getValues('tipoDocObj')]);

	const handleSearchClick = async (data: any) => {

		const numeroDoc = data;
		const tipoDoc = Number(tipoIdentificacion.split('_')[0]);

		if (Number(responseSolicitud?.tipoIdentificacion) === tipoDoc &&
			responseSolicitud?.identificacionCliente === numeroDoc) {
			NotificationService.genericAlert(
				'Alerta',
				`Solicitud radicada con el mismo número de documento del apoderado`,
				'warning'
			);
			return;
		}

		if (data !== '') {
			setBloquearTipoDoc(true)
			setSiExisteBloTipoNume(true)
		} else {
			setBloquearTipoDoc(false)
			setSiExisteBloTipoNume(false)
		}

		setIsLoading(true)

		try {
			const response = await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(tipoDoc, numeroDoc);
			if (response?.isSuccess && response?.data !== null) {
				const cliente: any = response.data;

				if (tipoDoc === 2) {
					const razonSocial =
						cliente.razonSocial ??
						cliente.nombreRazonSocial ??
						cliente.nombre ??
						cliente.firstName ??
						'';

					controlApoderado.setValue('razonsocial', razonSocial);
				} else {
					controlApoderado.setValue('primerNombre', cliente.primerNombre ?? '');
					controlApoderado.setValue('segundoNombre', cliente.segundoNombre ?? '');
					controlApoderado.setValue('primerApellido', cliente.primerApellido ?? '');
					controlApoderado.setValue('segundoApellido', cliente.segundoApellido ?? '');
				}

				setCamposNombresReadOnly(true);
				setIsLoading(false);

			} else if (response?.isSuccess && response?.data === null) {

				try {
					const request =
					{
						tipoDocumento: tipoDoc,
						numeroDocumento: numeroDoc,
						nombre: '',
						actorId: "00000000-0000-0000-0000-000000000000"
					}
					const responseInterviniente = await api.PostBuscarIntervinientesActores(request);
					if (responseInterviniente?.data && responseInterviniente?.data.length > 0) {

						const interviniente = responseInterviniente?.data[0];
						if (tipoDoc === 1) {
							controlApoderado.setValue('primerNombre', responseInterviniente.data[0].itR_NOMBRE);
							controlApoderado.setValue('segundoNombre', responseInterviniente.data[0].itR_SEGUNDONOMBRE);
							controlApoderado.setValue('primerApellido', responseInterviniente.data[0].itR_PRIMERAPELLIDO);
							controlApoderado.setValue('segundoApellido', responseInterviniente.data[0].itR_SEGUNDOAPELLIDO);
							setCamposNombresReadOnly(false);
							setIsLoading(false)
						} else if (tipoDoc === 2) {
							controlApoderado.setValue('numDoc', responseInterviniente.data[0].itR_NUMERO_DOCUMENTO);
							controlApoderado.setValue('razonsocial', responseInterviniente.data[0].itR_NOMBRE);
							setCamposNombresReadOnly(false);
							setIsLoading(false)
						}
					}

					if (responseInterviniente?.data && responseInterviniente?.data.length === 0) {
						if (tipoDoc === 1) {
							setCamposNombresReadOnly(false);
							NotificationService.genericAlwarning(
								'',
								`Cedula no se encontro, El afiliado no existe`,
								'warning',
							);
							setIsLoading(false)
						} else if (tipoDoc === 2) {
							setCamposNombresReadOnly(false);
							NotificationService.genericAlwarning(
								'',
								`Nit no se encontro, El afiliado no existe`,
								'warning',
							);
							setIsLoading(false)
						} else {
							setCamposNombresReadOnly(false);
							NotificationService.genericAlwarning(
								'',
								`Documento no se encontro, el afiliado no existe.`,
								'warning',
							);
						}
					}
				} catch (e: any) {
					toast(`Error al consultar intervinientes en solicitudes ${e.message}`, { type: toast.TYPE.ERROR });
					setIsLoading(false)
				}
			}
		} catch (_) {
			toast('Error al consultar afiliado en apoderado', { type: toast.TYPE.ERROR });
			setIsLoading(false)
		} finally {
			setIsLoading(false)
		}
	}

	const cancelarTerceroApoderado = async () => {

		setDescEstadoBoton('AGREGAR APODERADO');
		if (terceroApoderadoModel && terceroApoderadoModel.length > 0) {
			setDeshabilitarBoton(true);
		}
		setBloquearTipoDoc(false)
		controlApoderado.reset();
		setEncontrarIdentificacion(false);
		setHablitarLupaBusqueda(false)
		setHabilitarCampoNumeroIdentidad(false)
		setSiExisteBloTipoNume(false)
		setCamposNombresReadOnly(false);
	};


	const agregarTerceroApoderado = async (data: any) => {

		if (data.esAutorizado && !data.parentescoObj) {
			toast('Debe seleccionar un parentesco cuando marca Es Autorizado.', {
				type: toast.TYPE.ERROR
			});
			return;
		}

		setLoading(true);
		try {
			const parentesco = data.parentescoObj === '' ? null : data.parentescoObj;
			let canContinue = true;

			try {
				const sarlaftResponse = await apiCredito.PostVerificacionTerc({
					pe_Origen: PE_ORIGEN,
					pe_Dato: data.numDoc.toString(),
					pe_TipoVerificacion: PE_TIPO_VERIFICACION,
					pe_Porcentaje: PE_PORCENTAJE
				});

				if (sarlaftResponse?.isSuccess && sarlaftResponse?.data) {
					canContinue = !((sarlaftResponse?.data?.encontrado as string) === USER_SARLAFT_FOUND.SI);
				}
			} catch (error) {
				toast('Error en la verificación SARLAFT.', { type: toast.TYPE.ERROR });
				canContinue = false;
			}

			// if (!canContinue) {
			// 	NotificationService.genericAlwarning(
			// 		'Alerta',
			// 		`Por favor comunicarse con el oficial de cumplimiento`,
			// 		'warning')
			// 	setLoading(false);
			// 	return;
			// }

			const datoEncontrado = terceroApoderadoModel.findIndex(
				x => x.numDoc === data.numDoc && x.numDoc !== numDocAnterior
			);

			if (datoEncontrado === -1) {

				const guid = GenerarGuid();
				const nuevoTercero = {
					id: guid.newGuid(),
					tipoDoc: data.tipoDocObj.split('_')[0],
					tipoDocObj: {
						id: data.tipoDocObj.split('_')[0],
						descripcion: data.tipoDocObj.split('_')[1],
					},
					parentesco: parentesco ? parseInt(data.parentescoObj.split('_')[0], 10) : null,
					parentescoObj: parentesco
						? {
							id: Number(String(parentesco).split('_')[0]),
							descripcion: String(parentesco).split('_')[1]
						}
						: null,
					numDoc: data.numDoc,
					razonSocial: data?.razonsocial || '',
					esAutorizado: data.esAutorizado,
					primerNombre: data?.primerNombre.toUpperCase() || '',
					segundoNombre: data?.segundoNombre.toUpperCase() || '',
					primerApellido: data?.primerApellido.toUpperCase() || '',
					segundoApellido: data?.segundoApellido.toUpperCase() || '',
					encontrado: camposNombresReadOnly ? 'SI' : 'NO'
				};

				let nuevosTerceros: TerceroApoderadoModel[] = [];

				if (descEstadoBoton === 'AGREGAR APODERADO') {

					nuevosTerceros = [...terceroApoderadoModel, nuevoTercero];

				} else if (descEstadoBoton === 'EDITAR APODERADO') {

					const temp = [...terceroApoderadoModel];
					const posicionActual = temp.findIndex(x => x.numDoc === numDocAnterior);
					temp.splice(posicionActual, 1, { ...nuevoTercero, id: idAnterior });
					nuevosTerceros = temp;
					await NotificationService.genericAlert(
						'Actualizó con éxito',
						'Se actualizó el apoderado con éxito.',
					);
				}

				setTerceroApoderadoModel(nuevosTerceros);
				const array = nuevosTerceros.map(({ encontrado, ...informacion }) => informacion);
				tercerosApoderadoAdicionados(array);

				const cumpleCondicion = (resp: any) => {
					const m = resp?.descripcionModalidad;
					const s = resp?.descripcionSubModalidad;
					return (
						(m === "DESAFILIACIÓN SÓLO APORTES" &&
							(s === "VOLUNTARIA" || s === "POR NO ACREDITACIÓN")) ||
						(m === "RETIRO DE LA INSTITUCION" &&
							(s === "RETIRO EXPRESS DE LA INSTITUCION" || s === "RETIRO NORMAL DE LA INSTITUCION"))
					);
				};

				if (data.esAutorizado && cumpleCondicion(responseSolicitud)) {

					setApoderadosAutorizados((prev: any[] = []) => {
						const lista = Array.isArray(prev) ? prev : [];
						const yaExiste = lista.some(p => p.numDoc === nuevoTercero.numDoc);
						if (yaExiste) return lista;
						return [...lista, nuevoTercero];
					});
				} else if (!data.esAutorizado) {
					setApoderadosAutorizados((prev: any[] = []) =>
						(Array.isArray(prev) ? prev : []).filter(p => p.numDoc !== data.numDoc)
					);
				}

				setDeshabilitarBoton(true);
				cancelarTerceroApoderado();
				setNumDocAnterior('');
				setIdAnterior('');
				setEncontrarIdentificacion(true);
			} else {
				toast('Ya existe un apoderado con este mismo número de documento en la lista', { type: toast.TYPE.ERROR });
			}
		} catch (error) {
			toast('Error al agregar tercero apoderado.', { type: toast.TYPE.ERROR });
		} finally {
			setLoading(false);
		}
	};

	const eliminarRegistroTercero = async (item: string) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);

		setBloquearTipoDoc(false);
		setHablitarLupaBusqueda(false);
		setHabilitarCampoNumeroIdentidad(true);
		setCamposNombresReadOnly(false);

		if (confirmAlert) {
			const temp = [...terceroApoderadoModel];
			temp.splice(
				temp.findIndex(x => x.id === item),
				1,
			);

			setTerceroApoderadoModel(temp);
			tercerosApoderadoAdicionados(temp);

			setApoderadosAutorizados((prev: any[] = []) => {
				const copia = Array.isArray(prev) ? [...prev] : [];
				const eliminado = terceroApoderadoModel.find(t => t.id === item);


				if (eliminado?.numDoc) {
					return copia.filter(p => p.numDoc !== eliminado.numDoc);
				}

				return copia;
			});


			cancelarTerceroApoderado();

			if (temp.length === 0) {
				setDeshabilitarBoton(false);
			}
		}
	};

	const verificarSiExiste = async (numeroDoc: any) => {

		const tipoDoc = Number(tipoIdentificacion.split('_')[0])
		try {
			const response = await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(tipoDoc, numeroDoc);
			if (response?.data !== null) {
				setEncontrarIdentificacion(true);

			} else {
				setEncontrarIdentificacion(false);
			}
		} catch (_) {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
			setIsLoading(false)
		}
	}

	const seleccionarRegistroTercero = async (item: TerceroApoderadoModel) => {

		if (item.encontrado === 'SI') {
			setCamposNombresReadOnly(true);
		} else {
			setCamposNombresReadOnly(false);
		}
		setHabilitarCampoNumeroIdentidad(true);
		setBloquearTipoDoc(true);
		const numeroDoc = item.numDoc;

		await api
			.PostConsultarExistePersona(numeroDoc)
			.then(async (response: any) => {
				if (response) {
					if (response.isSuccess) {
						setEncontrarIdentificacion(true);
					} else {
						verificarSiExiste(numeroDoc)
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});

		controlApoderado.reset();
		setNumDocAnterior(item.numDoc);
		setIdAnterior(item.id);
		controlApoderado.setValue(
			'tipoDocObj',
			`${item.tipoDocObj.id}_${item.tipoDocObj.descripcion}`,
		);
		if (item.parentesco != null) {
			controlApoderado.setValue(
				'parentescoObj',
				`${item.parentescoObj?.id}_${item.parentescoObj?.descripcion}`,
			);
		}
		controlApoderado.setValue('numDoc', item.numDoc);
		controlApoderado.setValue(
			'razonsocial',
			item.razonSocial === undefined ? '' : item.razonSocial,
		);
		controlApoderado.setValue('esAutorizado', item.esAutorizado);

		const esNitRegistro = esNit(item.tipoDocObj?.id ?? item.tipoDoc);

		if (!esNitRegistro) {
			controlApoderado.setValue(
				'primerNombre',
				item.primerNombre === undefined ? '' : item.primerNombre,
			);
			controlApoderado.setValue(
				'segundoNombre',
				item.segundoNombre === undefined ? '' : item.segundoNombre,
			);
			controlApoderado.setValue(
				'primerApellido',
				item.primerApellido === undefined ? '' : item.primerApellido,
			);
			controlApoderado.setValue(
				'segundoApellido',
				item.segundoApellido === undefined ? '' : item.segundoApellido,
			);
		}
		setDescEstadoBoton('EDITAR APODERADO');
		setDeshabilitarBoton(false);
	};

	const esNit = (tipoDocumento: any) => String(tipoDocumento) === '2';

	if (loading) {
		return <LoadingComponent loading={loading} />
	}

	return (
		<>
			<CHPaper title="Información del Apoderado Autorizado (Si Aplica)">
				<LoadingV2Component key="loader" loading={isloading} tittle='Consultando Documento' />
				{
					((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
						<Grid container spacing={2}>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="tipoDocObj"
									control={controlApoderado.control}
									rules={{ required: 'Este campo es requerido' }}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											helperText={error ? error.message : null}
											label="Tipo de Documento *"
											value={value}
											required
											onChange={e => {
												const valorSeleccionado = String(e.target.value);

												setTipoIdentificacion(valorSeleccionado);
												setHabilitarCampoNumeroIdentidad(false);
												setHablitarLupaBusqueda(false);
												setCamposNombresReadOnly(false);

												onChange(e);
											}}
										>
											{listaTipoDocumentos?.map(
												(option: IDENTIFICATIONTYPEDTO) => {
													const dato = `${option.id}_${option.descripcion}`;
													return (
														<MenuItem
															key={option.id}
															value={dato}
															disabled={bloquearTipoDoc || siExisteBloTipoNume}
														>
															{option.descripcion}
														</MenuItem>
													);
												},
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid item xs={12} md={6} lg={3}>
								<Controller
									name="numDoc"
									control={controlApoderado.control}
									rules={{
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
										required: 'Este campo es requerido',
									}}
									render={({ field: { onChange, value }, fieldState: { error } }) => (
										<Grid container spacing={2} alignItems="center">
											<Grid item xs={9}>
												<CHTextField
													error={!!error}
													helperText={error ? error.message : null}
													required
													label="Número de Documento"
													InputLabelProps={{ shrink: true }}
													value={value}
													isReadOnly={hablitarCampoNumeroIdentidad || siExisteBloTipoNume}
													onChange={e => {
														if (e.target.value.length <= 12) {
															onChange(
																e.target.value.replace(
																	/[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`-]/g,
																	'',
																),
															);
															if (e.target.value.length >= 5) {
																setHablitarLupaBusqueda(true);
															} else {
																setHablitarLupaBusqueda(false);
															}
														}
													}}
												/>
											</Grid>
											<Grid item xs={3}>
												<IconButton
													onClick={() => handleSearchClick(controlApoderado.getValues('numDoc'))}
													disabled={!hablitarLupaBusqueda}
												>
													<SearchIcon />
												</IconButton>
											</Grid>
										</Grid>
									)}
								/>
							</Grid>

							{tipoIdentificacion.split('_')[0] === '2' && (
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="razonsocial"
										control={controlApoderado.control}
										rules={{
											required: 'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												error={!!error}
												required
												helperText={
													error ? error.message : null
												}
												isReadOnly={encontrarIdentificacion || camposNombresReadOnly}
												InputLabelProps={{ shrink: true }}
												label="Nombre / Razón Social"
												value={value}
												onChange={e => {
													if (e.target.value.length <= 50) {
														onChange(e);
													}
												}}
												inputProps={{
													style: {
														textTransform: 'uppercase',
													},
												}}
											/>

										)}
									/>
								</Grid>
							)}
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="parentescoObj"
									control={controlApoderado.control}
									rules={{
										required:
											responseSolicitud?.nombreTarea === "Radicación V 8" ||
												responseSolicitud?.nombreTarea === "Radicación V 14"
												? 'Este campo es obligatorio'
												: false,

										validate: value => {
											if (controlApoderado.getValues("esAutorizado") && !value) {
												return 'Debe seleccionar un parentesco cuando marca Es Autorizado';
											}
											return true;
										}
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Parentesco *"
											value={value}
											helperText={error ? error.message : null}
											onChange={value => {
												onChange(value);
											}}
										>
											{listaParentesco?.map(
												(option: VALORCATALOGO) => {

													if (option.idValor === 137 ||
														option.idValor === 163 ||
														option.idValor === 170 ||
														option.idValor === 176 ||
														option.idValor === 198 ||
														option.idValor === 241) {
														const dato = `${option.idValor}_${option.descripcion}`;
														return (
															<MenuItem
																key={option.idValor}
																value={dato}
															>
																{option.descripcion}
															</MenuItem>
														);
													}
													return null;
												},
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.columnLabel} item xs={12} md={6} lg={3}>
								<Controller
									name="esAutorizado"
									control={controlApoderado.control}
									rules={{
										required: {
											value: responseSolicitud?.nombreTarea === "Radicación V 8" || responseSolicitud?.nombreTarea === "Radicación V 14",
											message: 'Este campo es requerido',
										},
									}}
									render={({ field: { onChange, value }, fieldState: { error } }) => (
										<>
											<CHCheckbox
												label="Es Autorizado"
												onChange={(event: any) => {

													const checked =
														typeof event === "boolean"
															? event
															: event?.target?.checked;

													onChange(checked);
													controlApoderado.trigger("parentescoObj");
												}}
												checked={Boolean(value)}
											/>
											{error && <p style={{ color: 'red' }}>{error.message}</p>}
										</>
									)}
								/>
							</Grid>
							{tipoIdentificacion.split('_')[0] !== '2' && (
								<Grid container>
									<Grid
										className={classes.column}
										item
										xs={12}
										md={6} lg={3}
									>
										<Controller
											name="primerNombre"
											control={controlApoderado.control}
											rules={{
												required: 'Este campo es requerido.',
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (
												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }}
													nameMask
													label="Primer Nombre"
													error={!!error}
													required
													helperText={
														error ? error.message : null
													}
													InputLabelProps={{ shrink: true }}
													isReadOnly={campoSoloLecturaRadicacionAprobada || camposNombresReadOnly}
													value={value}
													onChange={onChange}
												/>

											)}
										/>
									</Grid>
									<Grid
										className={classes.column}
										item
										xs={12}
										md={6} lg={3}
									>
										<Controller
											name="segundoNombre"
											control={controlApoderado.control}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (
												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }}
													label="Segundo Nombre"
													error={!!error}
													helperText={
														error ? error.message : null
													}
													InputLabelProps={{ shrink: true }}
													// isReadOnly={!selectTypeDoc || disabledCamponNombre}
													isReadOnly={campoSoloLecturaRadicacionAprobada || camposNombresReadOnly}
													value={value}
													onChange={onChange}
												/>
											)}
										/>
									</Grid>
									<Grid
										className={classes.column}
										item
										xs={12}
										md={6} lg={3}
									>
										<Controller
											name="primerApellido"
											control={controlApoderado.control}
											rules={{
												required: 'Este campo es requerido.',
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (
												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }}
													nameMask
													label="Primer Apellido"
													error={!!error}
													required
													helperText={
														error ? error.message : null
													}
													InputLabelProps={{ shrink: true }}
													isReadOnly={campoSoloLecturaRadicacionAprobada || camposNombresReadOnly}
													value={value}
													onChange={onChange}
												/>

											)}
										/>
									</Grid>
									<Grid
										className={classes.column}
										item
										xs={12}
										md={6} lg={3}
									>
										<Controller
											name="segundoApellido"
											control={controlApoderado.control}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (
												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }}
													nameMask
													label="Segundo Apellido"
													error={!!error}
													helperText={
														error ? error.message : null
													}
													InputLabelProps={{ shrink: true }}
													isReadOnly={campoSoloLecturaRadicacionAprobada || camposNombresReadOnly}
													value={value}
													onChange={onChange}
												/>
											)}
										/>
									</Grid>
								</Grid>
							)}
						</Grid>
				}
				{
					((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
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
									disabled={deshabilitarBoton}
									onClick={controlApoderado.handleSubmit(
										agregarTerceroApoderado,
									)}
								>
									{descEstadoBoton}
								</Button>
								<Button
									className={classes.buttonAdd}
									variant="outlined"
									onClick={cancelarTerceroApoderado}
								>
									CANCELAR
								</Button>
							</Grid>
						</Grid>
				}
				<h3>Lista de Apoderados</h3>
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
									<TableCell>
										Tipo de Identificación
									</TableCell>
									<TableCell>
										Número de Identificación
									</TableCell>
									<TableCell>Nombre / Razón Social</TableCell>
									<TableCell>Parentesco</TableCell>
									<TableCell>Autorizado</TableCell>
									{
										((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? null :
											<TableCell align="center">
												Acciones
											</TableCell>
									}
								</TableRow>
							</TableHead>
							<TableBody>
								{terceroApoderadoModel?.map(
									(option: TerceroApoderadoModel) => (
										<TableRow key={option.id}>
											<TableCell>
												{option?.tipoDocObj?.descripcion}
											</TableCell>
											<TableCell>
												{option.numDoc}
											</TableCell>
											<TableCell>
												{esNit(option.tipoDocObj?.id ?? option.tipoDoc)
													? option.razonSocial
													: [
														option.primerNombre,
														option.segundoNombre,
														option.primerApellido,
														option.segundoApellido,
													]
														.filter(Boolean)
														.join(' ')}
											</TableCell>
											<TableCell>
												{option.parentescoObj?.id
													? option.parentescoObj.descripcion
													: null}
											</TableCell>

											<TableCell>
												{option.esAutorizado
													? 'SI'
													: 'NO'}
											</TableCell>
											{
												((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :

													<TableCell align="center">
														<IconButton
															aria-label="Editar"
															color="primary"
															onClick={() => {
																seleccionarRegistroTercero(
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
																eliminarRegistroTercero(
																	option.id,
																);
															}}
														>
															<Delete />
														</IconButton>
													</TableCell>
											}
										</TableRow>
									),
								)}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>
			</CHPaper>
		</>
	);
}

export default TerceroApoderado;