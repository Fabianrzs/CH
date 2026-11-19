import React, { useEffect, useRef, useState, memo } from 'react';
import {
	Button,
	createMuiTheme,
	createStyles,
	createTheme,
	Grid,
	IconButton,
	ListItemText,
	makeStyles,
	MenuItem,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Typography,
} from '@material-ui/core';
import { Delete, Edit } from '@material-ui/icons';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import UtilService from '../../../../utils/util.service';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { BeneficiarioModel, TerceroBeneficiarioModel } from '../../../../models/tercero-beneficiario.model';
import NotificationService from '../../../../services/notification.service';
import CreditoApi from '../../../../services/api-credit.service';
import { ENTIDADBANCARIA, IDENTIFICATIONTYPEDTO, VALORCATALOGO } from '../../../../types/catalog.type';
import GenerarGuid from '../funciones/guid.funtions';
import { ID_SUBMODALIDAD_SEGUNDO_PAGO, PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION, USER_SARLAFT_FOUND } from '../../../../constants/constants';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import { TerceroApoderadoModel } from '../../../../models/tercero-apoderado.model';


const useStyles = makeStyles(() =>
	createStyles({
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
		columnButtonAdd: {
			textAlign: 'right',
		},
		columnLabel: {
			padding: '10px',
		},

		inline: {
			display: 'inline',
		}
	}),
);

function TerceroBeneficiarioPagoDesafilaciones({
	listaValorCatalogo,
	listaBeneficiario,
	listaEntidadBancaria,
	listaTipoDocumentos,
	responseSolicitud,
	beneficiarios,
	tercerosBeneficiarioAdicionados,
	obtenerTipoCuenta,
	validarBeneficiario,
	validarRetiro,
	siEsAprobadaSolcitud,
	estadoEntidadNueva,
	entidadAhorroProgramadoSeguroEducativo,
	campoSoloLecturaRadicacionAprobada,
	beneficiariosExternos,
	apoderadosAutorizados

}: {
	listaValorCatalogo: VALORCATALOGO[];
	listaBeneficiario: TerceroBeneficiarioModel[];
	listaEntidadBancaria: ENTIDADBANCARIA[];
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
	responseSolicitud: ResponseSeveranceModel | undefined;
	beneficiarios: any[];
	tercerosBeneficiarioAdicionados: (terceros: TerceroBeneficiarioModel[]) => void;
	obtenerTipoCuenta: (catalogoId: number) => void;
	validarBeneficiario: any;
	validarRetiro: any;
	siEsAprobadaSolcitud: boolean;
	estadoEntidadNueva: any;
	entidadAhorroProgramadoSeguroEducativo: any;
	campoSoloLecturaRadicacionAprobada: any;
	beneficiariosExternos: any;
	apoderadosAutorizados: TerceroApoderadoModel[];
}) {

	const [loading, setLoading] = useState(false);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');

	const classes = useStyles();
	const apiCredito = CreditoApi();
	const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioModel[]>([]);

	const [idTerceroBeneficiario, setIdTerceroBeneficiario] = useState<string>('');
	const [numDocAnterior, setNumDocAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [readOnlyEntidad, setReadOnlyEntidad] = useState<boolean>(false);

	const [readOnlyNumeroCuenta, setReadOnlyNumeroCuenta] = useState<boolean>(false);
	const [valorARetirar, setvalorARetirar] = useState(Number(responseSolicitud?.valorRetirar));

	const [entidadRegistro, setEntidadRegistro] = useState<ENTIDADBANCARIA | null>(null);

	const [validarValorTipo, setValidarValorType] = useState<any>("");


	const [nullEntidadRegistro, setNullEntidadRegistro] = React.useState(false);
	const [ciudadResidenciaValue, setCiudadResidenciaValue] = React.useState<any>(null);
	// const [nuevoArray, setNuevoArray] = React.useState<any>(beneficiarios);

	const [nuevoArray, setNuevoArray] = React.useState<any>([]);
	React.useEffect(() => {
		if (beneficiariosExternos?.length) {
			setNuevoArray(beneficiariosExternos);
		}
	}, [beneficiariosExternos]);

	const controlBeneficiario = useForm({
		defaultValues: {
			id: '',
			tipoDocObj: '',
			numDoc: '',
			razonsocial: '',
			entidadObj: '',
			tipoCuentaObj: '',
			numeroCuenta: '',
			ConfirmarnumeroCuenta: '',
			apoderado: '',
			valorRetirarDos: '',
		},
	});

	useEffect(() => {
		obtenerTipoCuenta(67);
		if (listaBeneficiario) {
			if (listaBeneficiario.length > 0) {
				listaBeneficiario.forEach(x => {
					x.entidad =
						x.entidad === '00000000-0000-0000-0000-000000000000'
							? null
							: x.entidad;
				});
				setTerceroBeneficiarioModel(listaBeneficiario);
			}
		}

	}, []);

	useEffect(() => {

		const validar: any = validarBeneficiario.find((e: any) => (Number(e.tipoIdentificacion.id)) === 2)
		const validarV8: any = validarBeneficiario.filter((e: any) => (Number(e.tipoIdentificacion.id)) === 2 || (Number(e.tipoIdentificacion.id)) === 1 && e.tipo !== 'Afiliado')
		const ahorroProgramado: any = entidadAhorroProgramadoSeguroEducativo.find((e: any) => (Number(e.tipoIdentificacion.id)) !== 2)

		if (validar !== undefined && (responseSolicitud?.timCodigo === 'VINUEVA' || responseSolicitud?.timCodigo === 'CNVGR' || responseSolicitud?.timCodigo !== "VINUEVAPP" && responseSolicitud?.timCodigo !== "CAEXLEHA")) {

			const resp = beneficiarios.filter((e: any) => e.esAutorizado === true)
			validar.tipo = "Propietario"
			validar.numDoc = validar.numeroIdentificacion
			validar.tipoDoc = Number(validar.tipoIdentificacion.id)
			const x = [...resp]
			x.push(validar)
			setNuevoArray(x)

		} else if (validarV8 !== undefined && (responseSolicitud?.timCodigo === "VINUEVAPP" ||
			responseSolicitud?.timCodigo === "VIUSADAPP" || responseSolicitud?.timCodigo === "CAEXLEHA")) {
			const x: any[] = [];
			validarV8.forEach((element: any) => {
				const nuevo = {
					...element,
					tipo: "Propietario",
					numDoc: element.numeroIdentificacion,
					tipoDoc: Number(element.tipoIdentificacion.id)

				}

				x.push(nuevo)

			});

			setNuevoArray(x)

		}
		else if (validarV8 !== undefined && (responseSolicitud?.timCodigo === "VINUEVASP")) {

			const x: any[] = [];
			validarV8.forEach((element: any) => {
				const nuevo = {
					...element,
					tipo: "Propietario",
					numDoc: element.numeroIdentificacion,
					tipoDoc: Number(element.tipoIdentificacion.id)

				}

				x.push(nuevo)

			});

			setNuevoArray(x)
		}
		else if (estadoEntidadNueva !== undefined && estadoEntidadNueva.length !== 0) {

			const x: any[] = [];
			estadoEntidadNueva.forEach((element: any) => {
				const nuevo = {
					...element,
					tipo: "Propietario",
					numDoc: element.numDoc,
					tipoDoc: 2

				}

				x.push(nuevo)

			});

			setNuevoArray(x)
		}

		else if (ahorroProgramado !== undefined && responseSolicitud?.timCodigo === "AHOPROEDU") {

			const x: any[] = [];
			entidadAhorroProgramadoSeguroEducativo.forEach((element: any) => {
				const nuevo = {
					...element,
					tipo: "Ahorro Programado",
					numDoc: element.numeroIdentificacion,
					tipoDoc: element.tipoIdentificacion

				}

				x.push(nuevo)

			});

			setNuevoArray(x)

		}
		else {

			const resp = beneficiarios.filter((e: any) => e.tipo === "Afiliado")
			setNuevoArray(resp)
		}

	}, [listaBeneficiario, validarBeneficiario, estadoEntidadNueva, entidadAhorroProgramadoSeguroEducativo])

	const cancelarTerceroBeneficiario = async () => {
		setDescEstadoBoton('AGREGAR BENEFICIARIO');
		controlBeneficiario.reset()
		setEntidadRegistro(null)
		setCiudadResidenciaValue("")
	};

	const agregarTerceroBeneficiario = async (data: any) => {
		setLoading(true)
		if (descEstadoBoton === 'EDITAR PROPIETARIO') {
			eliminarRegistroTerceroEditar(idTerceroBeneficiario);
			setLoading(false)
		}

		let canContinue = true;
		try {
			const sarlaftResponse = await apiCredito.PostVerificacionTerc({
				pe_Origen: PE_ORIGEN,
				pe_Dato: data.numDoc.toString(),
				pe_TipoVerificacion: PE_TIPO_VERIFICACION,
				pe_Porcentaje: PE_PORCENTAJE
			})
			if (sarlaftResponse?.isSuccess && sarlaftResponse?.data) {
				canContinue = !((sarlaftResponse?.data?.encontrado as string) === USER_SARLAFT_FOUND.SI);
				setLoading(false)
			}
		} catch (_) {

			setLoading(false)

		}

		if (canContinue) {

			const subModalidadSegundoPago = ID_SUBMODALIDAD_SEGUNDO_PAGO.some(item => item === responseSolicitud?.tipoSubModalidadId);
			const quitatPesos = UtilService.cleanCurrencyValue(data.valorRetirarDos);
			controlBeneficiario.setValue('valorRetirarDos', String(quitatPesos));

			if (data.numeroCuenta !== data.ConfirmarnumeroCuenta) {
				toast('Número de Cuenta y Confirmación de Número deben ser iguales.', {
					type: toast.TYPE.ERROR,
				});
				data.numeroCuenta = "";
				controlBeneficiario.setValue('ConfirmarnumeroCuenta', '');
				setLoading(false);
			} else if (Number(quitatPesos) <= Number(responseSolicitud?.valorTotal)) {

				setvalorARetirar(valorARetirar - quitatPesos);

				const beneficiarioExistente = terceroBeneficiarioModel.some(
					(beneficiario) => beneficiario.numDoc === data.apoderado.split('_')[1]
				);

				if (!beneficiarioExistente || descEstadoBoton === 'EDITAR PROPIETARIO' || descEstadoBoton === "EDITAR BENEFICIARIOS") {
					const guid = GenerarGuid();
					const entidadId =
						data.entidadObj !== '' ? data.entidadObj.split('_')[0] : null;

					const tipoDocumento = listaTipoDocumentos.find(
						item => item.id === Number(data.apoderado.split('_')[0])
					);


					let terceroNuevo: any = {}
					if (estadoEntidadNueva === undefined || estadoEntidadNueva.length === 0) {
						terceroNuevo = {
							id: guid.newGuid(),
							tipoDoc: data.apoderado.split('_')[0],
							numDoc: data.apoderado.split('_')[1],
							razonsocial: data.apoderado.split('_')[2],
							tipoDocObj: {
								id: data.apoderado.split('_')[0],
								descripcion: tipoDocumento?.descripcion,
							},
							esAutorizado: data.apoderado.split('_')[3],
							entidad: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
							entidadObj: {
								id: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
								descripcion: data.entidadObj !== '' ? data.entidadObj.split('_')[1] : null,
							},
							tipoCuenta: data.tipoCuentaObj.split('_')[0],
							tipoCuentaObj: {
								id: data.tipoCuentaObj.split('_')[0],
								descripcion: data.tipoCuentaObj.split('_')[1],
							},
							numeroCuenta: data.numeroCuenta,
							ConfirmarnumeroCuenta: data.ConfirmarnumeroCuenta,
							valorRetirarDos: String(quitatPesos),
						} as TerceroBeneficiarioModel;
					} else {

						const razonSocial = estadoEntidadNueva[0].razonSocial || [];

						terceroNuevo = {
							id: guid.newGuid(),
							tipoDoc: data.apoderado.split('_')[0],
							numDoc: data.apoderado.split('_')[1],
							razonsocial: razonSocial,
							tipoDocObj: {
								id: data.apoderado.split('_')[0],
								descripcion: tipoDocumento?.descripcion,
							},
							esAutorizado: data.apoderado.split('_')[3],
							entidad: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
							entidadObj: {
								id: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
								descripcion: data.entidadObj !== '' ? data.entidadObj.split('_')[1] : null,
							},
							tipoCuenta: data.tipoCuentaObj.split('_')[0],
							tipoCuentaObj: {
								id: data.tipoCuentaObj.split('_')[0],
								descripcion: data.tipoCuentaObj.split('_')[1],
							},
							numeroCuenta: data.numeroCuenta,
							ConfirmarnumeroCuenta: data.ConfirmarnumeroCuenta,
							valorRetirarDos: String(quitatPesos),
						} as TerceroBeneficiarioModel;
					}

					let terceros: TerceroBeneficiarioModel[] = [];
					if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {
						if (Number(terceroNuevo?.valorRetirarDos) !== undefined) {

							if (Number(terceroNuevo?.valorRetirarDos) > validarRetiro && subModalidadSegundoPago === false) {
								toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
								setLoading(false);
								return;
							}
						}


						terceros = [...terceroBeneficiarioModel];
						let total = 0;
						if (terceros.length > 0) {

							terceros.forEach((element: any) => {
								total += Number(element.valorRetirarDos);
							})
						}
						total += Number(terceroNuevo?.valorRetirarDos) === undefined ? 0 : Number(terceroNuevo?.valorRetirarDos);
						if (total > validarRetiro && subModalidadSegundoPago === false) {
							toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
							setLoading(false);
							return;
						}
						if (total <= validarRetiro) {
							setTerceroBeneficiarioModel(oldArray => [...oldArray, terceroNuevo]);
							terceros.push(terceroNuevo);
						}



					} else {

						const temp = [...terceroBeneficiarioModel];
						const posicionActual = temp.findIndex(
							x => x.numDoc === numDocAnterior
						);
						temp.splice(posicionActual, 1);
						terceroNuevo.id = idAnterior;
						temp.splice(posicionActual, 0, terceroNuevo);
						terceros = temp;
						if (Number(terceroNuevo?.valorRetirarDos) !== undefined) {


							if (Number(terceroNuevo?.valorRetirarDos) > validarRetiro && subModalidadSegundoPago === false) {
								toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
								setLoading(false);
								return;
							}
						}

						let total = 0;
						if (temp.length > 0) {
							temp.forEach((element: any, index) => {
								if (posicionActual !== index) {

									total += Number(element.valorRetirarDos);
								}
							})
						}
						total += Number(terceroNuevo.valorRetirarDos);
						if (total > validarRetiro && subModalidadSegundoPago === false) {
							toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
							setLoading(false);
							return;
						}

						if (total <= validarRetiro) {
							setTerceroBeneficiarioModel(temp);
						}


					}
					if (estadoEntidadNueva === undefined || estadoEntidadNueva.length === 0) {
						tercerosBeneficiarioAdicionados(terceros);

						setNumDocAnterior(data.apoderado.split('_')[1]);
						await cancelarTerceroBeneficiario();
						setLoading(false);
					} else {
						tercerosBeneficiarioAdicionados(terceros);

						setNumDocAnterior(terceroNuevo);
						await cancelarTerceroBeneficiario();
						setLoading(false);
					}

				} else {
					toast('Ya existe un Beneficiario con este mismo número de documento en la lista', {
						type: toast.TYPE.ERROR,
					});
					setLoading(false);
				}
			} else {
				let terceroNuevo: any = {}
				terceroNuevo = {
					valorRetirarDos: String(quitatPesos),
				} as TerceroBeneficiarioModel;
				if (Number(terceroNuevo?.valorRetirarDos) !== undefined) {
					if (Number(terceroNuevo?.valorRetirarDos) > validarRetiro && subModalidadSegundoPago === false) {
						toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
						setLoading(false);
					}
				}


			}
		} else {
			toast(`La identificación ${data.numDoc.toString()} se encuentra en lista de Sarlaft`, {
				type: toast.TYPE.ERROR,
			});
			setLoading(false);
		}



	}

	const eliminarRegistroTercero = async (
		item: string,
		datoValorRetirar?: string,
	) => {

		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...terceroBeneficiarioModel];
			temp.splice(
				temp.findIndex(x => x.id === item),
				1,
			);
			setTerceroBeneficiarioModel(temp);
			tercerosBeneficiarioAdicionados(temp);
			const sumar = valorARetirar + Number(datoValorRetirar);
			cancelarTerceroBeneficiario();
		}
		return valorARetirar;
	};

	const eliminarRegistroTerceroEditar = async (item: string) => {
		const temp = [...terceroBeneficiarioModel];
		temp.splice(
			temp.findIndex(x => x.id === item),
			1,
		);
		setTerceroBeneficiarioModel(temp);
		tercerosBeneficiarioAdicionados(temp);
	};

	const seleccionarRegistro = async (item: TerceroBeneficiarioModel) => {

		setIdAnterior(item.id);
		setNumDocAnterior(item.numDoc);
		const newValores: ENTIDADBANCARIA = {
			idEntidadBancaria: item.entidadObj.id,
			numeroIdentificacion: '',
			nombreRazonSocial: item.entidadObj.descripcion,
			codigoBanco: 0,
			estado: true,
			creadoPor: '',
			fechaCreacion: '',
			modificadoPor: '',
			fechaModificacion: '',
		}

		setEntidadRegistro(newValores || null)
		setNullEntidadRegistro(false);

		controlBeneficiario.setValue(
			'entidadObj',
			`${item?.entidadObj?.id}_${item?.entidadObj?.descripcion}`,
		);

		setDescEstadoBoton('EDITAR BENEFICIARIOS');
		controlBeneficiario.setValue(
			'apoderado',
			`${item.tipoDoc}_${item.numDoc}_${item.razonsocial}_${item.esAutorizado}`,
		);

		controlBeneficiario.setValue(
			'tipoCuentaObj',
			`${item.tipoCuentaObj.id}_${item.tipoCuentaObj.descripcion}`,
		);


		controlBeneficiario.setValue(
			'entidadObj',
			`${item?.entidadObj?.id}_${item?.entidadObj?.descripcion}`,
		);
		controlBeneficiario.setValue('id', item?.id);


		controlBeneficiario.setValue('valorRetirarDos', item.valorRetirarDos || '');

		if (item.tipoCuenta.toString() === '322') {
			item.tipoCuentaObj.descripcion = '';


			controlBeneficiario.setValue('numeroCuenta', '');
			setReadOnlyEntidad(true);
			setReadOnlyNumeroCuenta(true);
		} else {
			setReadOnlyEntidad(false);
			setReadOnlyNumeroCuenta(false);
			controlBeneficiario.setValue('numeroCuenta', item.numeroCuenta);
		};
		// setRetiroValor( Number(item?.valorRetirarDos) + retiroValor);
	};

	const onChangeEntidad = (_: any, newValue: ENTIDADBANCARIA) => {

		setNullEntidadRegistro(newValue === null);
		setEntidadRegistro(newValue);

		if (newValue !== null) {
			const dato = `${newValue.idEntidadBancaria}_${newValue.nombreRazonSocial}`;
			controlBeneficiario.setValue('entidadObj', dato);
		}
	};

	const onKeyPressNoNumbers = (event: React.KeyboardEvent<HTMLInputElement>) => {
		const keyCode = event.which || event.keyCode;
		if (keyCode >= 48 && keyCode <= 57) {
			event.preventDefault();
		}
	};


	useEffect(() => {
		setNuevoArray((prev: any[] = []) => {
			const sinApoderados = prev.filter(
				(p) => p.tipo === "Afiliado" || !p.esAutorizado
			);
	
			const nuevosApoderados = (apoderadosAutorizados || []).map((a: any) => ({
				tipoDoc: a.tipoDoc,
				numDoc: a.numDoc,
				primerNombre: a.primerNombre,
				segundoNombre: a.segundoNombre,
				primerApellido: a.primerApellido,
				segundoApellido: a.segundoApellido,
				razonSocial: a.razonSocial,
				esAutorizado: true,
				tipo: a.tipoDocObj?.descripcion || a.tipo || '',
			}));
	
			const combinado = [...sinApoderados];
	
			nuevosApoderados.forEach((ap) => {
				const yaExiste = combinado.some((p) => p.numDoc === ap.numDoc);
				if (!yaExiste) combinado.push(ap);
			});
	
			return combinado;
		});
	}, [apoderadosAutorizados]);

	return (
		<>
			{loading ? (
				<LoadingComponent loading={loading} />
			) : (
				<CHPaper title="Información Beneficiario de Pago Nuevo">
					{
						((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
							<Grid container>
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="apoderado"
										control={controlBeneficiario.control}
										rules={{
											required: 'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (

											<CHSelect
												label="Beneficiario *"
												value={value}
												helperText={error ? error.message : null}
												onChange={value => {

													onChange(value);

												}}
											>
												{nuevoArray.map(
													(option: any) => {
														let apoderado = ''

														if (estadoEntidadNueva === undefined || estadoEntidadNueva.length > 0) {

															apoderado = `
												${option.tipo || option.tipoIdentificacion.descripcion} - 
												${option.numDoc || option.numeroIdentificacion} - 
												${option?.tipo !== "2" ?
																	`${option?.primerNombre} ${option?.segundoNombre} ${option?.primerApellido} ${option?.segundoApellido}` :
																	option?.razonSocial}
												${option.esAutorizado ? 'Autorizado ' : 'Principal '}`;

														} else {
															const nombreCompleto = `${option?.primerNombre} ${option?.segundoNombre} ${option?.primerApellido} ${option?.segundoApellido}`
															apoderado = `
														${option.tipo || option.tipoIdentificacion?.descripcion || option.tipoDocEstudioObj?.descripcion || ''} - 
														${option.numDoc || option.numeroIdentificacion || option.numDocEstudio || ''} - 
														${option?.razonSocial || option.nombreRazonSocial || nombreCompleto}
		
														${option.esAutorizado ? 'Autorizado ' : 'Principal '}`;
														}


														return (
															<MenuItem
																key={`item-${option.numDoc}`}
																value={`${option.tipoDoc}_${option.numDoc}_${option?.tipoDoc?.toString() !== "2" ?
																	`${option?.primerNombre} ${option?.segundoNombre} ${option?.primerApellido} ${option?.segundoApellido}`
																	: option?.razonSocial}_${option.esAutorizado ? 'Autorizado' : 'Principal'}`}
															>
																<ListItemText
																	primary={apoderado}
																	secondary={
																		<React.Fragment
																			key={`fragmento-${option.numDoc}`}
																		>
																			<Typography
																				component="span"
																				variant="body2"
																				className={
																					classes.inline
																				}
																				color="textPrimary"
																			>
																				{
																					option.tipoDoc
																				}
																			</Typography>
																			{/* {estadoEntidadNueva === undefined && estadoEntidadNueva.length > 0 ?

																			` — ${option.tipo}/ ${option?.tipoDoc?.toString() !== "2" ?

																				`${option?.primerNombre} ${option?.segundoNombre} ${option?.primerApellido} ${option?.segundoApellido}` : option?.razonSocial}` :

																			` — ${option.tipo}/ ${option?.tipoDoc?.toString() !== "2" ?

																				`${option?.primerNombre} ${option?.segundoNombre} ${option?.primerApellido} ${option?.segundoApellido}` :
																				option?.razonSocial || option.nombreRazonSocial}`
																		} */}
																			{estadoEntidadNueva === undefined || estadoEntidadNueva.length > 0
																				? ` — ${(option.tipo || '')} / ${option?.tipoDoc?.toString() !== "2"
																					? `${option?.primerNombre || ''} ${option?.segundoNombre || ''} ${option?.primerApellido || ''} ${option?.segundoApellido || ''}`
																					: option?.razonSocial || option?.razonsocialEstudio || ''}`
																				: ` — ${(option.tipo || '')} / ${option?.tipoDoc?.toString() !== "2"
																					? `${option?.primerNombre || ''} ${option?.segundoNombre || ''} ${option?.primerApellido || ''} ${option?.segundoApellido || ''}`
																					: option?.razonSocial || option?.razonsocialEstudio || option?.nombreRazonSocial || ''}`
																			}
																		</React.Fragment>
																	}
																/>
															</MenuItem>
														);
													},
												)}
											</CHSelect>

										)}
									/>
								</Grid>

								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller

										name="tipoCuentaObj"
										control={controlBeneficiario.control}

										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (

											<CHSelect


												label="Tipo de Cuenta *"
												value={value}

												onChange={e => {
													onChange(e);
													setValidarValorType(e.target.value)
												}}

											>
												{listaValorCatalogo?.map(
													(option: any) => {

														const dato = `${option.idValor}_${option.descripcion}`;



														return (
															<MenuItem
																key={option.idValor}
																value={dato}
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

								{validarValorTipo === "322_CHEQUE" && (
									<Grid className={classes.column} item xs={12} md={6} lg={3}>

										<Controller
											name="entidadObj"
											control={controlBeneficiario.control}
											rules={{
												required: false,
											}}
											render={({ field: { onChange, value } }) => (
												<CHAutoComplete
													value={nullEntidadRegistro ? null : entidadRegistro}
													onChange={onChangeEntidad}
													options={listaEntidadBancaria}
													getOptionLabel={option => option?.nombreRazonSocial}
													renderInput={params => (
														<CHTextField
															value={value}
															{...params}
															label="Entidad"
															onChange={onChange}
															onKeyPress={onKeyPressNoNumbers}
														/>
													)}
												/>
											)}
										/>
									</Grid>
								)
								}


								{validarValorTipo !== "322_CHEQUE" &&
									(<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="entidadObj"
											rules={{
												required: 'Este campo es requerido.',
											}}
											control={controlBeneficiario.control}
											render={(
												{
													field: { onChange, value },
													fieldState: { error },
												}) => (
												<CHAutoComplete
													value={

														nullEntidadRegistro ? null : entidadRegistro
													}

													onChange={onChangeEntidad}

													options={listaEntidadBancaria}
													getOptionLabel={option => option?.nombreRazonSocial}
													renderInput={(params) =>


														<CHTextField
															value={value}
															isReadOnly={false}
															error={!!error}
															helperText={error ? error.message : null}
															{...params}
															label="Entidad Bancaria *"
															onChange={onChange}
															onKeyPress={onKeyPressNoNumbers}
														/>

													}

												/>

											)}
										/>
									</Grid>)

								}
								{validarValorTipo !== "322_CHEQUE" ?
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="numeroCuenta"
											control={controlBeneficiario.control}
											rules={{
												maxLength: {
													value: 21,
													message: 'La longitud máxima del campo es de 21 caracteres',
												},
												required: true,
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (


												<CHTextField

													onPasteCapture={e => {
														e.preventDefault();
														const clipboardText = e.clipboardData.getData('text/plain');
														const sanitizedText = clipboardText.replace(/[^0-9]/g, '');
														onChange(sanitizedText);
													}}

													numberOnlyMask
													error={!!error}
													helperText={error ? error.message : null}
													label="Número de Cuenta"
													required
													value={value}
													InputLabelProps={{ shrink: true }}
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
													onChange={(e: any) => {
														const d = e.keyCode || e.which;

														const regex = /^[0-9]*$/;
														const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
														const numeros = regex.test(e.target.value)

														if (sanitizedText.length <= 21) {
															onChange(sanitizedText)
														}
													}}
												/>


											)}
										/>
									</Grid> : <Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="numeroCuenta"
											control={controlBeneficiario.control}
											rules={{
												maxLength: {
													value: 21,
													message: 'La longitud máxima del campo es de 21 caracteres',
												},
												required: false,
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (


												<CHTextField
													onPasteCapture={e => {
														e.preventDefault();
														const clipboardText = e.clipboardData.getData('text/plain');
														const sanitizedText = clipboardText.replace(/[^0-9]/g, '');
														onChange(sanitizedText);
													}}
													numberOnlyMask
													label="Número de Obligación"
													value={value}
													InputLabelProps={{ shrink: true }}
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
													onChange={(e: any) => {
														const regex = /^[0-9]*$/;
														const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
														if (sanitizedText.length <= 21) {
															onChange(sanitizedText)
														}
													}}
												/>


											)}
										/>
									</Grid>}

								{validarValorTipo !== "322_CHEQUE" ?
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="ConfirmarnumeroCuenta"
											control={controlBeneficiario.control}
											rules={{

												maxLength: {
													value: 21,
													message: 'La longitud máxima del campo es de 21 caracteres',
												},
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
													label="Confirmación Número de Cuenta"
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
									</Grid> :
									<Grid className={classes.column} item xs={12} md={6} lg={3}>
										<Controller
											name="ConfirmarnumeroCuenta"
											control={controlBeneficiario.control}
											rules={{

												maxLength: {
													value: 21,
													message: 'La longitud máxima del campo es de 21 caracteres',
												},
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
													}}
													error={!!error}
													helperText={error ? error.message : null}
													label="Confirmación Número de Obligación"
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
									</Grid>

								}
								<Grid className={classes.column2} item xs={12} md={6} lg={3}>
									<Controller
										name="valorRetirarDos"
										control={controlBeneficiario.control}
										rules={{
											required: 'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (

											<CHTextField

												currencyMask
												error={!!error}
												helperText={error ? error.message : null}
												inputProps={{
													style: {
														textAlign: 'right',
													},
												}}
												// label="Valor a Retirar"
												label="Valor a Retirar*"
												value={value}
												onChange={e => {

													const valor = e.target.value
													// setValorRestar(UtilService.cleanCurrencyValue(valor))
													onChange(e)
												}}
											/>
										)}
									/>
								</Grid>
								<Grid
									className={classes.columnButtonAdd}
									item
									xs={12}
									md={12}
								>
									<Button

										variant="outlined"
										onClick={controlBeneficiario.handleSubmit(
											agregarTerceroBeneficiario
										)}
									>
										{descEstadoBoton}
									</Button>


									<Button
										className={classes.buttonAdd}
										variant="outlined"

										onClick={cancelarTerceroBeneficiario}
									>
										CANCELAR
									</Button>
								</Grid>
							</Grid>

					}

					<h3>Lista de Beneficiarios</h3>
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
										<TableCell>Nombre / Razón Social</TableCell>
										<TableCell>Tipo de Cuenta</TableCell>
										<TableCell>Entidad Bancaria</TableCell>
										{
											validarValorTipo !== "322_CHEQUE" ?
												<TableCell>Número de Cuenta</TableCell> :
												<TableCell>Número de Obligación</TableCell>
										}

										<TableCell>Valor a retirar</TableCell>
										{
											((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? null :
												<TableCell align="center">
													Acciones
												</TableCell>
										}
									</TableRow>
								</TableHead>
								<TableBody>

									{terceroBeneficiarioModel.map(
										(option: any) => (
											<TableRow key={option.tipoDoc}>
												<TableCell>
													{option.tipoDocObj.descripcion}
												</TableCell>
												<TableCell>
													{option.numDoc}
												</TableCell>
												{
													estadoEntidadNueva.length !== 0 ?
														<TableCell>
															{option?.razonsocial}
														</TableCell> :
														<TableCell>
															{option?.razonsocial}
														</TableCell>
												}

												<TableCell>
													{
														option.tipoCuentaObj
															.descripcion
													}
												</TableCell>
												<TableCell>
													{option.entidadObj.descripcion !== null || option.entidadObj.descripcion === 'null'
														? option.entidadObj
															.descripcion
														: ''}
												</TableCell>
												<TableCell>
													{option.numeroCuenta}
												</TableCell>
												<TableCell>
													{UtilService.formatCurrency(
														Number(
															option.valorRetirarDos,
														),
													)}
												</TableCell>
												{
													((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
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
																	eliminarRegistroTercero(
																		option.id,
																		option.valorRetirarDos,
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


			)}

		</>
	);
}

export default memo(TerceroBeneficiarioPagoDesafilaciones);