import React, { useEffect, useState } from 'react';
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
	Button,
	MenuItem,
	IconButton,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { Delete, Edit } from '@material-ui/icons';
import { toast } from 'react-toastify';
import SearchIcon from '@material-ui/icons/Search';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { MatriculaInmobiliariaModel } from '../../../../models/matricula-inmobiliaria.model';
import { TipoMatricula } from '../../../../models/propietario-matricula.model';
import NotificationService from '../../../../services/notification.service';
import SolicitudApi from '../../../../services/api-solicitud.service';
import CreditoApi from '../../../../services/api-credit.service';
import ApiClients from '../../../../services/api-clients.service';
import { PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION, USER_SARLAFT_FOUND } from '../../../../constants/constants';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';

const useStyles = makeStyles(() =>
	createStyles({
		columnButtonAdd: {
			textAlign: 'right',
			marginTop: '1rem',
		},
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
	}),
);

function PropietarioMatricula({
	title,
	tiposIdentificacion,
	listaPropietarios,
	matriculas,
	propietariosAdicionados,
	setValidarBeneficiario,
	responseSolicitud,
	siEsAprobadaSolcitud,
	campoSoloLecturaRadicacionAprobada,
}: {
	title: string;
	tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
	listaPropietarios: any[];
	matriculas: MatriculaInmobiliariaModel[];
	propietariosAdicionados: (propietarios: any[]) => void;
	setValidarBeneficiario: any;
	responseSolicitud: ResponseSeveranceModel | undefined;
	siEsAprobadaSolcitud: boolean;
	campoSoloLecturaRadicacionAprobada: any;
}) {
	const api = SolicitudApi();
	const apiCredito = CreditoApi();
	const apiDos = ApiClients();
	const classes = useStyles();
	const [propietarioMatriculaModel, setPropietarioMatriculaModel] = useState<any[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR PROPIETARIO');
	const [tipoIdentificaciones, setTipoIdentificaciones] = useState<any>('');
	const [buttonDisabled, setButtonDisabled] = useState<boolean>(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [disabledCamponNombre, setDisabledCamponNombre] = useState<boolean>(true);
	const [listadoTipoIdentificacion, setListadoTipoIdentificacion] = useState(tiposIdentificacion);
	const [hablitarLupaBusqueda, setHablitarLupaBusqueda] = useState(false)
	const [isloading, setIsLoading] = useState(false)
	const [campoTipoyNumDocumentoSoloLectura, setCampoTipoyNumDocumentoSoloLectura] = useState(false);

	useEffect(() => {

		if (listaPropietarios) {
			if (listaPropietarios.length > 0 && propietarioMatriculaModel.length <= 0) {
				const tipoIden = listaPropietarios[0].tipoIdentificacion.id
				if (tipoIden === 1) {
					setPropietarioMatriculaModel(listaPropietarios);
				} else {
					const matr = listaPropietarios[0].matricula[0];
					const newMatricula = {
						estado: matr.estado,
						id: matr.id,
						numeroMatricula: matr.numeroMatricula
					};
					setPropietarioMatriculaModel([...listaPropietarios, newMatricula]);
				}

			}
		}
	}, []);

	useEffect(() => {
		soloListarNit()
		setValidarBeneficiario(propietarioMatriculaModel)
	}, []);

	const propietariosControl = useForm({
		defaultValues: {
			numeroIdentificacion: '',
			razonSocial: '',
			tipoIdentificacion: '',
			matriculaId: '',
			numeroIdentificacionBuscar: '',
			primerNombre: '',
			segundoNombre: '',
			primerApellido: '',
			segundoApellido: '',
			numeroDocumentoBuscar: '',
			verificaNit: ''
		},
	});

	// const handleSearchClick = async (data: any) => {
	// 	setIsLoading(true)

	// 	const numeroDoc = data
	// 	try {
	// 		const response = await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(tipoIdentificaciones, numeroDoc);
	// 		if (response?.isSuccess && response?.data !== null) {
	// 			propietariosControl.setValue('primerNombre', response.data.primerNombre);
	// 			propietariosControl.setValue('segundoNombre', response.data.segundoNombre);
	// 			propietariosControl.setValue('primerApellido', response.data.primerApellido);
	// 			propietariosControl.setValue('segundoApellido', response.data.segundoApellido);
	// 			setDisabledCamponNombre(true);
	// 			setIsLoading(false)
	// 		} else if (response?.isSuccess && response?.data === null) {
	// 			try {
	// 				const request =
	// 				{
	// 					tipoDocumento: tipoIdentificaciones,
	// 					numeroDocumento: numeroDoc,
	// 					nombre: '',
	// 					actorId: "00000000-0000-0000-0000-000000000000"
	// 				}
	// 				const responseInterviniente = await api.PostBuscarIntervinientesActores(request);

	// 				if (responseInterviniente?.data && responseInterviniente?.data.length > 0) {
	// 					if (tipoIdentificaciones === 1) {
	// 						propietariosControl.setValue('primerNombre', responseInterviniente.data[0].itR_NOMBRE);
	// 						propietariosControl.setValue('segundoNombre', responseInterviniente.data[0].itR_SEGUNDONOMBRE);
	// 						propietariosControl.setValue('primerApellido', responseInterviniente.data[0].itR_PRIMERAPELLIDO);
	// 						propietariosControl.setValue('segundoApellido', responseInterviniente.data[0].itR_SEGUNDOAPELLIDO);
	// 						setDisabledCamponNombre(true);
	// 						setIsLoading(false)
	// 					} else if (tipoIdentificaciones === 2) {
	// 						propietariosControl.setValue('numeroIdentificacion', responseInterviniente.data[0].itR_NUMERO_DOCUMENTO);
	// 						propietariosControl.setValue('razonSocial', responseInterviniente.data[0].itR_NOMBRE);
	// 						setDisabledCamponNombre(true);
	// 						setIsLoading(false)
	// 					}
	// 				}

	// 				if (responseInterviniente?.data && responseInterviniente?.data.length === 0) {
	// 					NotificationService.genericAlwarning(
	// 						'',
	// 						`El propietario no existe`,
	// 						'warning',
	// 					);
	// 					setDisabledCamponNombre(false);
	// 					setIsLoading(false)
	// 				}
	// 			} catch (e: any) {
	// 				toast(`Error al consultar intervinientes en solicitudes propietario matricula ${e.message}`, { type: toast.TYPE.ERROR });
	// 				setIsLoading(false)
	// 			}
	// 		}
	// 	} catch (_) {
	// 		toast('Error del sistema al consultar afiliado Propietario Matricula', { type: toast.TYPE.ERROR });
	// 		setIsLoading(false)
	// 	}
	// }

	const handleSearchClick = async (data: any) => {
		setIsLoading(true);

		const numeroDoc = data;

		try {
			const response =
				await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(
					tipoIdentificaciones,
					numeroDoc,
				);

			if (response?.isSuccess && response?.data !== null) {
				propietariosControl.setValue('primerNombre', response.data.primerNombre);
				propietariosControl.setValue('segundoNombre', response.data.segundoNombre);
				propietariosControl.setValue('primerApellido', response.data.primerApellido);
				propietariosControl.setValue('segundoApellido', response.data.segundoApellido);

				setDisabledCamponNombre(true);

				// BLOQUEA TIPO Y NÚMERO DE DOCUMENTO
				setCampoTipoyNumDocumentoSoloLectura(true);

				// APAGA LA LUPA
				setHablitarLupaBusqueda(false);

				setIsLoading(false);

			} else if (response?.isSuccess && response?.data === null) {
				try {
					const request = {
						tipoDocumento: tipoIdentificaciones,
						numeroDocumento: numeroDoc,
						nombre: '',
						actorId: '00000000-0000-0000-0000-000000000000',
					};

					const responseInterviniente =
						await api.PostBuscarIntervinientesActores(request);

					if (
						responseInterviniente?.data &&
						responseInterviniente?.data.length > 0
					) {
						if (Number(tipoIdentificaciones) === 1) {
							propietariosControl.setValue(
								'primerNombre',
								responseInterviniente.data[0].itR_NOMBRE || '',
							);
							propietariosControl.setValue(
								'segundoNombre',
								responseInterviniente.data[0].itR_SEGUNDONOMBRE || '',
							);
							propietariosControl.setValue(
								'primerApellido',
								responseInterviniente.data[0].itR_PRIMERAPELLIDO || '',
							);
							propietariosControl.setValue(
								'segundoApellido',
								responseInterviniente.data[0].itR_SEGUNDOAPELLIDO || '',
							);

							// INTERVINIENTES: permite editar nombres y apellidos
							setDisabledCamponNombre(false);

							// Pero mantiene bloqueado tipo y número de documento
							setCampoTipoyNumDocumentoSoloLectura(true);

							// Bloquea la lupa
							setHablitarLupaBusqueda(false);

							setIsLoading(false);

						} else if (Number(tipoIdentificaciones) === 2) {
							propietariosControl.setValue(
								'numeroIdentificacion',
								responseInterviniente.data[0].itR_NUMERO_DOCUMENTO || numeroDoc,
							);
							propietariosControl.setValue(
								'razonSocial',
								responseInterviniente.data[0].itR_NOMBRE || '',
							);

							// INTERVINIENTES: permite editar razón social
							setDisabledCamponNombre(false);

							// Pero mantiene bloqueado tipo y número de documento
							setCampoTipoyNumDocumentoSoloLectura(true);

							// Bloquea la lupa
							setHablitarLupaBusqueda(false);

							setIsLoading(false);
						}
					}

					if (
						responseInterviniente?.data &&
						responseInterviniente?.data.length === 0
					) {
						NotificationService.genericAlwarning(
							'',
							`El propietario no existe`,
							'warning',
						);


						// NO EXISTE EN BUA NI INTERVINIENTES:
						// permite editar nombres / apellidos / razón social
						setDisabledCamponNombre(false);

						// Pero mantiene bloqueado tipo y número de documento
						setCampoTipoyNumDocumentoSoloLectura(true);

						// Bloquea la lupa
						setHablitarLupaBusqueda(false);

						setIsLoading(false);
					}
				} catch (e: any) {
					toast(
						`Error al consultar intervinientes en solicitudes propietario matricula ${e.message}`,
						{ type: toast.TYPE.ERROR },
					);

					setIsLoading(false);
				}
			}
		} catch (_) {
			toast(
				'Error del sistema al consultar afiliado Propietario Matricula',
				{ type: toast.TYPE.ERROR },
			);

			setIsLoading(false);
		}
	};

	const eliminarRegistro = async (
		matriculaId: string,
		numeroIdentificacion: string,
	) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...propietarioMatriculaModel];
			const propietarioEncontrado = temp.find(
				x => x.numeroIdentificacion === numeroIdentificacion,
			);
			propietarioEncontrado?.matricula.splice(
				propietarioEncontrado?.matricula.findIndex(
					(x: any) => x.id === matriculaId,
				),
				1,
			);
			if (propietarioEncontrado?.matricula.length === 0) {
				temp.splice(
					temp.findIndex(
						x => x.numeroIdentificacion === numeroIdentificacion,
					),
					1,
				);
			}
			setPropietarioMatriculaModel(temp);
			propietariosAdicionados(temp);
			soloListarNit()
			cancelarPropietario();

		}
	};

	const cancelarPropietario = () => {
		setListadoTipoIdentificacion(tiposIdentificacion);
		setHablitarLupaBusqueda(false)
		setDisabledCamponNombre(true)
		setDescEstadoBoton('AGREGAR PROPIETARIO');
		propietariosControl.reset();
		soloListarNit()
		setCampoTipoyNumDocumentoSoloLectura(false);
	};

	const agregarPropietario = async (data: any) => {

		if (data.numeroIdentificacion !== data.verificaNit && data.tipoIdentificacion === 2) {
			NotificationService.genericAlert(
				'',
				'no coinciden los números de Nit.',
				'warning'
			);
			return
		}

		setLoading(true)
		const newValue = data.numeroIdentificacion;

		if (data.numeroIdentificacion === responseSolicitud?.identificacionCliente && responseSolicitud?.timCodigo !== "LIBERGRAV" && responseSolicitud?.timCodigo !== "APTGHGR") {
			toast('El número de identificación no puede ser igual número de identificación beneficiario', { type: toast.TYPE.ERROR });
			setLoading(false)
			return
		}

		if (data.numeroIdentificacion.length < 5 || data.numeroIdentificacion.length > 12) {
			toast('El número de documento debe tener como mínimo entre 5 y 12 dígitos.', { type: toast.TYPE.ERROR });
			setLoading(false)
			return
		}

		if (!/^\d*$/.test(newValue)) {
			toast('El número de identificación no puede ser negativo', { type: toast.TYPE.ERROR });
			return
		}

		let canContinue = true;

		try {
			const sarlaftResponse = await apiCredito.PostVerificacionTerc({
				pe_Origen: PE_ORIGEN,
				pe_Dato: data.numeroIdentificacion.toString(),
				pe_TipoVerificacion: PE_TIPO_VERIFICACION,
				pe_Porcentaje: PE_PORCENTAJE // Para tipo de verificación 1 este campo va vacio
			})
			if (sarlaftResponse?.isSuccess && sarlaftResponse?.data) {
				canContinue = !((sarlaftResponse?.data?.encontrado as string) === USER_SARLAFT_FOUND.SI);
			}
		} catch (_) {
			toast('API-ERROR', { type: toast.TYPE.ERROR, });
			setButtonDisabled(false);
		}

		if (canContinue) {

			const datoEncontrado = propietarioMatriculaModel.find(
				x => x.numeroIdentificacion === data.numeroIdentificacion,
			);
			const matriculaEncontrada = datoEncontrado?.matricula.findIndex(
				(x: any) => x.id === data.matriculaId.split('_')[0],
			);
			const posicionPropietario = propietarioMatriculaModel.findIndex(
				x => x.numeroIdentificacion === data.numeroIdentificacion,
			);

			if (matriculaEncontrada === 0 && descEstadoBoton === 'AGREGAR PROPIETARIO') {
				cancelarPropietario();
				setButtonDisabled(false);
				toast('Ya existe una matrícula asociada para este propietario', {
					type: toast.TYPE.ERROR,
				});
				setLoading(false);
				return;
			}

			const propietarios: any[] = [...propietarioMatriculaModel];

			const tipoMatricula: TipoMatricula = {
				id: data.matriculaId.split('_')[0],
				numeroMatricula: data.matriculaId.split('_')[1],
				estado: data.matriculaId.split('_')[2],
			};

			const descripcionIdentificacion = listadoTipoIdentificacion.find(tipo => tipo.id === parseInt(data.tipoIdentificacion, 10))?.descripcion;

			const propietarioNuevo = {
				numeroIdentificacion: data.numeroIdentificacion,
				razonSocial: data?.razonSocial,
				matricula: [],
				tipoIdentificacion: {
					id: parseInt(data.tipoIdentificacion, 10),
					descripcion: descripcionIdentificacion,
				},
				primerNombre: data?.primerNombre === undefined ? '' : data?.primerNombre,
				segundoNombre: data?.segundoNombre === undefined ? '' : data?.segundoNombre,
				primerApellido: data?.primerApellido === undefined ? '' : data?.primerApellido,
				segundoApellido: data?.segundoApellido === undefined ? '' : data?.segundoApellido
			} as any;
			propietarioNuevo.matricula.push(tipoMatricula);

			if ((matriculaEncontrada === -1 || matriculaEncontrada === undefined) && descEstadoBoton === 'AGREGAR PROPIETARIO') {

				propietarios.push(propietarioNuevo);
				setPropietarioMatriculaModel(propietarios);
				propietariosAdicionados(propietarios);
				cancelarPropietario();
				// Información para otro componente
				setValidarBeneficiario(propietarios);
				setLoading(false);

			} else if (descEstadoBoton === 'EDITAR PROPIETARIO') {

				if (posicionPropietario >= 0 && posicionPropietario < propietarios.length) {
					propietarios[posicionPropietario] = propietarioNuevo;
					setPropietarioMatriculaModel(propietarios);
					propietariosAdicionados(propietarios);
					cancelarPropietario();
					// Información para otro componente
					setValidarBeneficiario(propietarios);
					setLoading(false);
				}

			}

		} else {

			console.log('.');
			
			setLoading(false)
		}
	};

	const soloListarNit = () => {

		if (responseSolicitud?.timCodigo === "LIBERGRAV" ||
			responseSolicitud?.timCodigo === "APTGHGR" ||
			responseSolicitud?.timCodigo === "CAEXLEHAPP" ||
			responseSolicitud?.timCodigo === "CESOCLEAS" ||
			responseSolicitud?.timCodigo === "EJEOPCOMP") {
			const listadoTiposDocumentos = tiposIdentificacion.filter(x =>
				x.id === 2

			)
			setListadoTipoIdentificacion(listadoTiposDocumentos)
		}

	}

	const seleccionarRegistro = async (item: any, matricula: any) => {

		setIsLoading(true)
		let permiteEditar = true;
		setCampoTipoyNumDocumentoSoloLectura(true);

		try {
			const response = await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(parseInt(item?.tipoIdentificacion?.id, 10), String(item.numeroIdentificacion));
			if (response?.isSuccess && response?.data !== null) {
				setDisabledCamponNombre(true);
				permiteEditar = false;

			} else if (response?.isSuccess && response?.data === null) {

				try {
					const request =
					{
						ttipoDocumento: parseInt(item?.tipoIdentificacion?.id, 10),
						numeroDocumento: String(item.numeroIdentificacion),
						nombre: '',
						actorId: "00000000-0000-0000-0000-000000000000"
					}
					const responseInterviniente = await api.PostBuscarIntervinientesActores(request);
					if (responseInterviniente?.data && responseInterviniente?.data.length > 0) {
						setDisabledCamponNombre(false);
						permiteEditar = true;
					}
				}
				catch (e: any) {
					toast('Error al buscar intervinientes en seleccionar registro propietarios inmueble', { type: toast.TYPE.ERROR });
					setIsLoading(false)
				}
			}
		} catch (e: any) {
			toast(`Error al consultar afiliado en selección de registro propietario${e.message}`, { type: toast.TYPE.ERROR });
			setIsLoading(false)
		}

		if (permiteEditar) {
			setDisabledCamponNombre(false);
		} else {
			setDisabledCamponNombre(true);
		}

		setIsLoading(false)
		if (parseInt(item?.tipoIdentificacion?.id, 10) === 1) {
			setTipoIdentificaciones(1)
		} else if (parseInt(item?.tipoIdentificacion?.id, 10) === 2) {
			setTipoIdentificaciones(2)
		}

		propietariosControl.setValue('numeroIdentificacion', item.numeroIdentificacion);
		propietariosControl.setValue('tipoIdentificacion', `${item.tipoIdentificacion.id}`);
		propietariosControl.setValue('matriculaId',
			`${matricula.id}_${matricula.numeroMatricula}_${matricula.estado}`,
		);

		if (tipoIdentificaciones !== 2 && item?.tipoIdentificacion?.id !== 2) {
			propietariosControl.setValue('primerNombre', item?.primerNombre === undefined ? '' : item?.primerNombre);
			propietariosControl.setValue('segundoNombre', item?.segundoNombre === undefined ? '' : item?.segundoNombre);
			propietariosControl.setValue('primerApellido', item?.primerApellido === undefined ? '' : item?.primerApellido);
			propietariosControl.setValue('segundoApellido', item?.segundoApellido === undefined ? '' : item?.segundoApellido);
		}
		else if (item?.tipoIdentificacion?.id === 2) {
			propietariosControl.setValue('razonSocial', item?.razonSocial);
		}
		setDescEstadoBoton('EDITAR PROPIETARIO');
	};

	setValidarBeneficiario(propietarioMatriculaModel)

	if (loading) {
		return <LoadingComponent loading={loading} />
	}


	return (
		<>
			<CHPaper title={title}>
				<LoadingV2Component key="loader" loading={isloading} tittle='Consultando Documento' />
				{
					((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :

						<Grid container spacing={2}>

							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="tipoIdentificacion"
									control={propietariosControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Tipo de Documento *"
											required
											value={value}
											helperText={error ? error.message : null}
											disabled={campoTipoyNumDocumentoSoloLectura}
											onChange={value => {
												const tipoDoc = value.target.value
												setHablitarLupaBusqueda(false)
												setTipoIdentificaciones(tipoDoc)
												onChange(value)
											}}
										>

											{listadoTipoIdentificacion?.map((option: IDENTIFICATIONTYPEDTO) => (
												<MenuItem key={option.id} value={option.id}>
													{option.descripcion}
												</MenuItem>
											))}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid item xs={12} md={6} lg={3}>
								<Controller
									name="numeroIdentificacion"
									control={propietariosControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({ field: { onChange, value }, fieldState: { error } }) => (
										<Grid container spacing={2} alignItems="center">
											<Grid item xs={9}>
												<CHTextField
													error={!!error}
													helperText={error ? error.message : null}
													type="number"
													required
													isReadOnly={campoTipoyNumDocumentoSoloLectura}
													label="Número de Documento"
													InputLabelProps={{ shrink: true }}
													value={value}
													onChange={(e) => {
														const valor = e.target.value
														if (valor.length <= 12) {
															if (valor.length >= 5) {
																setHablitarLupaBusqueda(true)

															} else {
																setHablitarLupaBusqueda(false)
															}
															onChange(e)
														}
													}}
												/>
											</Grid>
											<Grid item xs={3}>
												<IconButton
													onClick={() => handleSearchClick(propietariosControl.getValues('numeroIdentificacion'))}
													disabled={!hablitarLupaBusqueda || campoTipoyNumDocumentoSoloLectura}
												>
													<SearchIcon />
												</IconButton>
											</Grid>
										</Grid>
									)}
								/>
							</Grid>
							{
								tipoIdentificaciones === 2 ?
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="verificaNit"
											control={propietariosControl.control}
											rules={{
												required: 'Este campo es requerido.',
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (

												<CHTextField
													onCopy={e => e.preventDefault()}
													onPaste={e => e.preventDefault()}
													onContextMenu={e => e.preventDefault()}
													onPasteCapture={e => {
														e.preventDefault();
														const clipboardText = e.clipboardData.getData('text/plain');
														const sanitizedText = clipboardText.replace(/[^0-9]/g, '');
														onChange(sanitizedText);
													}}
													error={!!error}
													helperText={error ? error.message : null}
													label="Confirmación Número Nit"
													numberOnlyMask
													value={value}
													onKeyDown={e => {
														const allowedKeys = [8, 9, 35, 36, 37, 39, 46]; // Teclas de navegación y borrado
														if (
															(e.key >= '0' && e.key <= '9') ||
															allowedKeys.includes(e.keyCode)
														) {
															return;
														}
														e.preventDefault();
													}}
													onChange={e => {
														const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
														if (sanitizedText.length <= 21) {
															onChange(sanitizedText)
														}
													}}
												/>

											)}
										/>
									</Grid> : null
							}

							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="matriculaId"
									control={propietariosControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Matrícula Inmobiliaria *"
											value={value}
											helperText={error ? error.message : null}
											required
											onChange={value => {
												onChange(value);
											}}
										>
											{(matriculas || []).map((option: MatriculaInmobiliariaModel) => {
												const matricula = `Matrícula ${option.numeroMatricula
													} - ${option.esPrincipal
														? 'Principal'
														: 'Relacionado'
													}`;
												return (
													<MenuItem
														key={`item-${option.numeroMatricula}`}
														value={`${option.id}_${option.numeroMatricula
															}_${option.esPrincipal
																? 'Principal'
																: 'Relacionado'
															}`}
													>
														{
															matricula
														}

													</MenuItem>
												);
											},
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							{tipoIdentificaciones === 2 &&
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="razonSocial"
										control={propietariosControl.control}
										rules={{
											required: 'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (

											<CHTextField
												inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 50 }} // mayusculas
												error={!!error}
												helperText={error ? error.message : null}
												label="Razón Social"
												value={value}
												isReadOnly={disabledCamponNombre}
												required
												InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
												onChange={onChange}
											/>

										)}
									/>
								</Grid>
							}
							{tipoIdentificaciones !== 2 &&
								<Grid container>
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="primerNombre"
											control={propietariosControl.control}
											rules={{
												required: 'Este campo es requerido.',
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (

												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }} // mayusculas
													nameMask
													label="Primer Nombre"
													error={!!error}
													required
													isReadOnly={disabledCamponNombre}
													helperText={error ? error.message : null}
													InputLabelProps={{ shrink: true }}
													value={value}
													onChange={(e) => onChange(e)}

												/>

											)}
										/>
									</Grid>
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="segundoNombre"
											control={propietariosControl.control}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (
												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }} // mayusculas
													nameMask
													label="Segundo Nombre"
													error={!!error}
													helperText={error ? error.message : null}
													InputLabelProps={{ shrink: true }}
													value={value}
													onChange={(e) => onChange(e)}
													isReadOnly={disabledCamponNombre}
												/>
											)}
										/>
									</Grid>
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="primerApellido"
											control={propietariosControl.control}
											rules={{
												required: 'Este campo es requerido.',
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (

												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }} // mayusculas
													nameMask
													label="Primer Apellido"
													error={!!error}
													required
													helperText={error ? error.message : null}
													InputLabelProps={{ shrink: true }}
													value={value}
													onChange={(e) => onChange(e)}
													isReadOnly={disabledCamponNombre}
												/>

											)}
										/>
									</Grid>
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="segundoApellido"
											control={propietariosControl.control}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (
												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' }, maxLength: 25 }} // mayusculas
													nameMask
													label="Segundo Apellido"
													error={!!error}
													helperText={error ? error.message : null}
													InputLabelProps={{ shrink: true }}
													value={value}
													onChange={(e) => onChange(e)}
													isReadOnly={disabledCamponNombre}
												/>
											)}
										/>
									</Grid>
								</Grid>
							}
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
									disabled={buttonDisabled}
									onClick={propietariosControl.handleSubmit(
										agregarPropietario,
									)}
								>
									{descEstadoBoton}
								</Button>
								<Button
									className={classes.buttonAdd}
									variant="outlined"
									disabled={buttonDisabled}
									onClick={cancelarPropietario}
								>
									CANCELAR
								</Button>
							</Grid>
						</Grid>
				}

				<h3>Lista de Propietarios</h3>
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
									<TableCell>Tipo de Identificación</TableCell>
									<TableCell>Número de Identificación</TableCell>
									<TableCell>Nombre Completo / Razón Social</TableCell>
									<TableCell>Matrícula Asociada</TableCell>
									{
										((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? null :
											<TableCell align="center">
												Acciones
											</TableCell>
									}
								</TableRow>
							</TableHead>
							<TableBody>
								{propietarioMatriculaModel?.map(
									(option: any) => (
										<>
											{option?.matricula?.map(
												(matricula: TipoMatricula) => (
													<TableRow
														key={`${option.tipoIdentificacion}_${matricula.id}_${option.numeroIdentificacion}`}
													>
														<TableCell>
															{
																option.tipoIdentificacion.descripcion
															}
														</TableCell>
														<TableCell>
															{
																option.numeroIdentificacion
															}
														</TableCell>

														<TableCell> {option.primerNombre !== "" && option.primerNombre !== null ? `${option.primerNombre.toLocaleUpperCase()} ${option.segundoNombre.toLocaleUpperCase()} ${option.primerApellido.toLocaleUpperCase()} ${option.segundoApellido.toLocaleUpperCase()}` : option.razonSocial.toLocaleUpperCase()} </TableCell>
														<TableCell>{`Matrícula ${matricula.numeroMatricula} - ${matricula.estado}`}</TableCell>
														{
															((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
																<TableCell align="center">
																	{/* {permiteEditar && ( */}
																	<IconButton
																		aria-label="Editar"
																		color="primary"
																		onClick={() => {
																			seleccionarRegistro(
																				option,
																				matricula,
																			);
																		}}
																	>
																		<Edit />
																	</IconButton>
																	{/* )} */}
																	<IconButton
																		aria-label="Eliminar"
																		color="secondary"
																		onClick={() => {
																			eliminarRegistro(
																				matricula.id,
																				option.numeroIdentificacion,
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
										</>
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

export default PropietarioMatricula;