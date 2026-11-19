import React, { useEffect, useState } from 'react';
import {
	Button,
	createMuiTheme,
	createStyles,
	createTheme,
	Grid,
	IconButton,
	makeStyles,
	MenuItem,
	MuiThemeProvider,
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
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import {
	ENTIDADBANCARIA,
	IDENTIFICATIONTYPEDTO,
	VALORCATALOGO,
} from '../../../../types/catalog.type';
import { TerceroBeneficiarioModel } from '../../../../models/tercero-beneficiario.model';
import GenerarGuid from '../funciones/guid.funtions';
import NotificationService from '../../../../services/notification.service';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import UtilService from '../../../../utils/util.service';
import { ID_SUBMODALIDAD_SEGUNDO_PAGO } from '../../../../constants/constants';

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
		}, columnTextAdd: {
			textAlign: 'right',
			width: '200px',
		},
	}),
);

function TerceroBeneficiario({
	listaBeneficiario,
	listaValorCatalogo,
	listaEntidadBancaria,
	listaTipoDocumentos,
	responseSolicitud,
	tercerosBeneficiarioAdicionados,
	obtenerTipoCuenta,
	validarBeneficiario,
	validarRetiro,
}: {
	listaBeneficiario: TerceroBeneficiarioModel[];
	listaValorCatalogo: VALORCATALOGO[];
	listaEntidadBancaria: ENTIDADBANCARIA[];
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
	responseSolicitud: ResponseSeveranceModel | undefined;
	tercerosBeneficiarioAdicionados: (terceros: TerceroBeneficiarioModel[]) => void;
	obtenerTipoCuenta: (catalogoId: number) => void;
	validarBeneficiario: any;
	validarRetiro: any;
}) {
	const classes = useStyles();
	const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');
	const [numDocAnterior, setNumDocAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [readOnlyEntidad, setReadOnlyEntidad] = useState<boolean>(false);
	const [readOnlyNumeroCuenta, setReadOnlyNumeroCuenta] = useState<boolean>(false);
	const [valorARetirar, setvalorARetirar] = useState(
		Number(responseSolicitud?.valorRetirar),
	);
	const [validarValorTipo, setValidarValorType] = useState<any>("");
	const [validarButtonRetiro, setValidarButtonRetiro] = useState<any>("");
	const [textoTitulo, setTextoTirulo] = useState("");

	useEffect(() => {
		obtenerTipoCuenta(67);
		if (listaBeneficiario !== null) {
			listaBeneficiario?.forEach(x => { x.entidad = x.entidad === '00000000-0000-0000-0000-000000000000' ? null : x.entidad });
			setTerceroBeneficiarioModel(listaBeneficiario);
		}
		controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
		controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' });
		loadData(responseSolicitud);
		if(responseSolicitud?.timCodigo ==='VINUEVAPP'){
			const mensaje = 'Información Beneficiario de Pago Nuevo'
			setTextoTirulo(mensaje)
		}else {
			const mensaje = 'Información Beneficiario'
			setTextoTirulo(mensaje)
		}
	}, []);

	const controlBeneficiario = useForm({
		defaultValues: {
			tipoDocObj: '',
			numDoc: '',
			razonsocial: '',
			entidadObj: '',
			tipoCuentaObj: '',
			numeroCuenta: '',
			valorRetirarDos: '',
			ConfirmarnumeroCuenta: '',
		},
	});

	const cancelarTerceroBeneficiario = async () => {
		setDescEstadoBoton('AGREGAR BENEFICIARIO');
		controlBeneficiario.setValue('tipoDocObj', '');
		controlBeneficiario.setValue('numDoc', '');
		controlBeneficiario.setValue('razonsocial', '');
		controlBeneficiario.setValue('entidadObj', '');
		controlBeneficiario.setValue('tipoCuentaObj', '');
		controlBeneficiario.setValue('numeroCuenta', '');
		controlBeneficiario.setValue('valorRetirarDos', '');
		controlBeneficiario.setValue('ConfirmarnumeroCuenta', '');
		controlBeneficiario.reset();
		controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
		controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' });

	};

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


	const agregarTerceroBeneficiario = async (data: any) => {

		const subModalidadSegundoPago = ID_SUBMODALIDAD_SEGUNDO_PAGO.some(item => item === responseSolicitud?.tipoSubModalidadId);
		const valorDos = UtilService.cleanCurrencyValue(data.valorRetirarDos);
		if (valorDos !== Number(validarRetiro) && subModalidadSegundoPago === false) {
			toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
		}

		const datoEncontrado = terceroBeneficiarioModel.findIndex(
			x => x.numDoc === data.numDoc && x.numDoc !== numDocAnterior,
		);
		if (datoEncontrado === -1) {

			const quitatPesos = UtilService.cleanCurrencyValue(
				data.valorRetirarDos,
			);
			controlBeneficiario.setValue('valorRetirarDos', String(quitatPesos));




			if (data.numeroCuenta !== data.ConfirmarnumeroCuenta) {
				toast(
					'Número de Cuenta y Confirmación de Número deben ser iguales.',
					{ type: toast.TYPE.ERROR },
				);
				data.numeroCuenta = "";

			} else if (Number(quitatPesos) <= Number(responseSolicitud?.valorTotal)) {
				setvalorARetirar(valorARetirar - quitatPesos);

				const guid = GenerarGuid();
				const entidadId =
					data.entidadObj !== '' ? data.entidadObj.split('_')[0] : null;

				const terceroNuevo = {
					id: guid.newGuid(),
					tipoDoc: data.tipoDocObj.split('_')[0],
					tipoDocObj: {
						id: data.tipoDocObj.split('_')[0],
						descripcion: data.tipoDocObj.split('_')[1],
					},
					numDoc: data.numDoc,
					razonsocial: data.razonsocial,
					entidad:
						entidadId === '00000000-0000-0000-0000-000000000000'
							? null
							: entidadId,
					entidadObj: {
						id:
							entidadId === '00000000-0000-0000-0000-000000000000'
								? null
								: entidadId,
						descripcion:
							data.entidadObj !== ''
								? data.entidadObj.split('_')[1]
								: null,
					},
					tipoCuenta: data.tipoCuentaObj.split('_')[0],
					tipoCuentaObj: {
						id: data.tipoCuentaObj.split('_')[0],
						descripcion: data.tipoCuentaObj.split('_')[1],
					},
					numeroCuenta: data.numeroCuenta,
					ConfirmarnumeroCuenta: data.ConfirmarnumeroCuenta,
					valorRetirarDos: String(quitatPesos)
				};

				let terceros: TerceroBeneficiarioModel[] = [];
				if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {
					setTerceroBeneficiarioModel(oldArray => [
						...oldArray,
						terceroNuevo,
					]);
					terceros = [...terceroBeneficiarioModel];
					terceros.push(terceroNuevo);
				} else {
					const temp = [...terceroBeneficiarioModel];
					const posicionActual = temp.findIndex(
						x => x.numDoc === numDocAnterior,
					);
					temp.splice(posicionActual, 1);
					terceroNuevo.id = idAnterior;
					temp.splice(posicionActual, 0, terceroNuevo);
					terceros = temp;
					setTerceroBeneficiarioModel(temp);
				}

				tercerosBeneficiarioAdicionados(terceros);
				setNumDocAnterior('');
				setIdAnterior('');
				await cancelarTerceroBeneficiario();
			} else {
				// toast(
				// 	`valor a retirar es mayor al valor ${UtilService.formatCurrency(
				// 		Number(responseSolicitud?.valorTotal),
				// 	)} que usted posee`,
				// 	{ type: toast.TYPE.ERROR },
				// );
			}
		} else {
			toast(
				'Ya existe un Beneficiario con este mismo número de documento en la lista',
				{ type: toast.TYPE.ERROR },
			);
		}


	};

	const eliminarRegistroTercero = async (item: string) => {
		const confirmAlert = await NotificationService.confirmAlert('¿Eliminando registro?');
		if (confirmAlert) {
			const temp = [...terceroBeneficiarioModel];
			temp.splice(
				temp.findIndex(x => x.id === item),
				1,
			);
			setTerceroBeneficiarioModel(temp);
			tercerosBeneficiarioAdicionados(temp);
		}
	};

	const seleccionarRegistroTercero = async (
		item: TerceroBeneficiarioModel,
	) => {
		setNumDocAnterior(item.numDoc);
		setIdAnterior(item.id);
		controlBeneficiario.setValue('tipoDocObj', `${item.tipoDocObj.id}_${item.tipoDocObj.descripcion}`);
		controlBeneficiario.setValue('numDoc', item.numDoc);
		controlBeneficiario.setValue('razonsocial', item.razonsocial);
		if (item.entidadObj.descripcion !== null) {
			controlBeneficiario.setValue('entidadObj', `${item.entidadObj.id}_${item.entidadObj.descripcion}`);
		}
		controlBeneficiario.setValue('tipoCuentaObj', `${item.tipoCuentaObj.id}_${item.tipoCuentaObj.descripcion}`);
		controlBeneficiario.setValue('numeroCuenta', item.numeroCuenta);
		if (item.tipoCuentaObj.id.toString() === '322') {
			controlBeneficiario.register('entidadObj', { required: false });
			controlBeneficiario.register('numeroCuenta', { required: false });
		} else {
			controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
			controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' });
		}
		setDescEstadoBoton('EDITAR BENEFICIARIO');
	};

	const loadData = (data: ResponseSeveranceModel | undefined) => {
		if (data?.tipoIdentificacion !== undefined) {
			const tipoDoc = listaTipoDocumentos.find((i) => i.id === parseInt(data?.tipoIdentificacion, 10));
			if (tipoDoc) {
				if (data?.identificacionCliente === undefined ||
					responseSolicitud?.timCodigo === 'LIBERGRAV' ||
					responseSolicitud?.timCodigo === 'APTGHGR' ||
					responseSolicitud?.timCodigo === 'AHOPROEDU' ||
					responseSolicitud?.timCodigo === 'CAEXLEHA' ||
					responseSolicitud?.timCodigo === 'READACPROES' ||
					responseSolicitud?.timCodigo === 'PAGOPREDIAL' ||
					responseSolicitud?.timCodigo === 'VINUEVAPP') {
					controlBeneficiario.setValue('tipoDocObj', '');
				}
				else {
					controlBeneficiario.setValue('tipoDocObj', `${tipoDoc.id}_${tipoDoc.descripcion}`);
				}

			}
		}

		if (data?.identificacionCliente !== undefined && responseSolicitud?.timCodigo !== 'LIBERGRAV' &&
		    responseSolicitud?.timCodigo !== 'APTGHGR' &&
			responseSolicitud?.timCodigo !== 'AHOPROEDU' &&
			responseSolicitud?.timCodigo !== 'CAEXLEHA' &&
			responseSolicitud?.timCodigo !== 'READACPROES' &&
			responseSolicitud?.timCodigo !== 'PAGOPREDIAL' &&
			responseSolicitud?.timCodigo !== 'VINUEVAPP') {
			controlBeneficiario.setValue('numDoc', data.identificacionCliente);
		}


		let nombre = '';
		if (data?.nombresCliente !== undefined) {
			const nombres = data?.nombresCliente?.split(' ') || [];
			const primerNombre = nombres.length > 0 ? nombres[0] : '';
			const segundoNombre = nombres.length > 1 ? nombres[1] : '';
			nombre = `${primerNombre} ${segundoNombre}`;
		}

		let apellido = '';
		if (data?.apellidosCliente !== undefined) {
			const apellidos = data?.apellidosCliente?.split(' ') || [];
			const primerApellido = apellidos.length > 0 ? apellidos[0] : '';
			const segundoApellido = apellidos.length > 1 ? apellidos[1] : '';
			apellido = `${primerApellido} ${segundoApellido}`;
		}

		if (responseSolicitud?.timCodigo === 'LIBERGRAV') {

			controlBeneficiario.setValue('razonsocial', '')

		}
		else if (responseSolicitud?.timCodigo === 'APTGHGR') {
			controlBeneficiario.setValue('razonsocial', '')

		}
		else if (responseSolicitud?.timCodigo === 'AHOPROEDU') {
			controlBeneficiario.setValue('razonsocial', '')

		}
		else if (responseSolicitud?.timCodigo === 'CAEXLEHA') {
			controlBeneficiario.setValue('razonsocial', '')
		}
		else if (responseSolicitud?.timCodigo === 'READACPROES') {
			controlBeneficiario.setValue('razonsocial', '')
		}
		else if (responseSolicitud?.timCodigo === 'PAGOPREDIAL') {
			controlBeneficiario.setValue('razonsocial', '')
		}
		else if(responseSolicitud?.timCodigo === 'VINUEVAPP'){
			controlBeneficiario.setValue('razonsocial', '')
		}


		else {
			controlBeneficiario.setValue('razonsocial', `${nombre} ${apellido}`)
		}
	}

	return (
		<CHPaper title={textoTitulo}>
			{
				responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?

					<Grid container>
						<Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="tipoDocObj"
								control={controlBeneficiario.control}
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
											label="Tipo de Documento *"
											value={value}
											required
											helperText={error ? error.message : null}
											onChange={value => {
												onChange(value);
											}}
										>
											{listaTipoDocumentos?.map(
												(option: IDENTIFICATIONTYPEDTO) => {
													const dato = `${option.id}_${option.descripcion}`;

													return (
														<MenuItem
															key={option.id}
															value={dato}
														>
															{option.descripcion}
														</MenuItem>
													);
												},
											)}
										</CHSelect>
									</MuiThemeProvider>
								)}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="numDoc"
								control={controlBeneficiario.control}
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
											required
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Numero de documento"
											value={value}
											onChange={onChange}
										/>
									</MuiThemeProvider>
								)}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="razonsocial"
								control={controlBeneficiario.control}
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
											isReadOnly={false}
											required
											label="Nombre / Razón Social"
											value={value}
											onChange={onChange}
										/>
									</MuiThemeProvider>
								)}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="tipoCuentaObj"
								control={controlBeneficiario.control}
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
											label="Tipo de Cuenta *"
											value={value}
											required
											helperText={error ? error.message : null}
											onChange={value => {
												const dato = value.target
													.value as string;
												const idEntidadBancaria =
													dato.split('_')[0];
												if (idEntidadBancaria === '322') {
													setReadOnlyEntidad(true);
													setReadOnlyNumeroCuenta(false);
													controlBeneficiario.setValue('entidadObj', '');
													controlBeneficiario.setValue('numeroCuenta', '');
													controlBeneficiario.register('entidadObj', { required: false });
													controlBeneficiario.register('numeroCuenta', { required: false });
													controlBeneficiario.clearErrors('entidadObj');
													controlBeneficiario.clearErrors('numeroCuenta');
												} else {
													setReadOnlyEntidad(false);
													setReadOnlyNumeroCuenta(false);
													controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
													controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' },
													);
												}
												onChange(value);
												setValidarValorType(value.target.value)
											}}
										>
											{listaValorCatalogo?.map(
												(option: VALORCATALOGO) => {
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
									</MuiThemeProvider>
								)}
							/>
						</Grid>
						{validarValorTipo !== "322_CHEQUE" &&
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="entidadObj"
									control={controlBeneficiario.control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHSelect
												error={!!error}
												label="Entidad Bancaria *"
												readOnly={readOnlyEntidad}
												value={value}
												required
												helperText={error ? error.message : null}
												onChange={value => {
													onChange(value);
												}}
											>
												{listaEntidadBancaria?.map(
													(option: ENTIDADBANCARIA) => {
														const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
														return (
															<MenuItem
																key={
																	option.idEntidadBancaria
																}
																value={dato}
															>
																{option.nombreRazonSocial}
															</MenuItem>
														);
													},
												)}
											</CHSelect>
										</MuiThemeProvider>
									)}
								/>
							</Grid>
						}

						{validarValorTipo !== "322_CHEQUE" &&
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numeroCuenta"
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
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHTextField
												onPasteCapture={e => {
													e.preventDefault();
												}}
												numberOnlyMask
												error={!!error}
												required
												helperText={error ? error.message : null}
												isReadOnly={readOnlyNumeroCuenta}
												label={validarValorTipo === "322_CHEQUE" ? "Número De Obligación" : "Número de Cuenta"}
												value={value}
												onChange={onChange}
											/>
										</MuiThemeProvider>
									)}
								/>
							</Grid>
						}

						{validarValorTipo === "322_CHEQUE" && (responseSolicitud.timCodigo === 'LIBERGRAV' || responseSolicitud.timCodigo === 'APTGHGR')  &&
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numeroCuenta"
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
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHTextField
												onPasteCapture={e => {
													e.preventDefault();
												}}
												numberOnlyMask
												error={!!error}
												required
												helperText={error ? error.message : null}
												isReadOnly={readOnlyNumeroCuenta}
												label={validarValorTipo === "322_CHEQUE" ? "Número De Obligación" : "Número de Cuenta"}
												value={value}
												onChange={onChange}
											/>
										</MuiThemeProvider>
									)}
								/>
							</Grid>
						}


						{validarValorTipo !== "322_CHEQUE" &&
							<Grid className={classes.column} item xs={12} md={3}>
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
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHTextField
												onPasteCapture={e => {
													e.preventDefault();
												}}
												error={!!error}
												helperText={error ? error.message : null}
												label={validarValorTipo === "322_CHEQUE" ? "Confirmación Número De Obligación" : "Confirmación Número de Cuenta"}
												numberOnlyMask
												value={value}
												onChange={e => {
													e.preventDefault();

													if (e.target.value.length <= 21) {
														onChange(e)
													}
												}}
											/>

										</MuiThemeProvider>
									)}
								/>
							</Grid>
						}

						{validarValorTipo === "322_CHEQUE" && (responseSolicitud.timCodigo === 'LIBERGRAV' || responseSolicitud.timCodigo === 'APTGHGR') &&
							<Grid className={classes.column} item xs={12} md={3}>
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
										<MuiThemeProvider theme={formLabelsTheme}>
											<CHTextField
												onPasteCapture={e => {
													e.preventDefault();
												}}
												error={!!error}
												helperText={error ? error.message : null}
												label={validarValorTipo === "322_CHEQUE" ? "Confirmación Número De Obligación" : "Confirmación Número de Cuenta"}
												numberOnlyMask
												value={value}
												onChange={e => {
													e.preventDefault();

													if (e.target.value.length <= 21) {
														onChange(e)
													}
												}}
											/>

										</MuiThemeProvider>
									)}
								/>
							</Grid>
						}



						<Grid className={classes.column2} item xs={12} md={3}>
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
									<MuiThemeProvider theme={formLabelsTheme}>
										<CHTextField className={classes.columnTextAdd}
											numberMask
											required
											currencyMask
											error={!!error}
											helperText={error ? error.message : null}
											inputProps={{
												style: {
													textAlign: 'right',
												},
											}}
											label="Valor a Retirar"
											value={value}
											onChange={onChange}
											onBlur={e => {
												setValidarButtonRetiro(e.target.value)
											}

											}
										/>
									</MuiThemeProvider>
								)}
							/>
						</Grid>

					</Grid> : ''
			}

			{/* <Grid className={classes.column} item xs={12} md={3}>
				<Controller
					name="numDoc"
					control={controlBeneficiario.control}
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
							label="Numero de documento"
							value={value}
							onChange={onChange}
						/>
					)}
				/>
			</Grid> */}
			{/* <Grid className={classes.column} item xs={12} md={3}>
				<Controller
					name="razonsocial"
					control={controlBeneficiario.control}
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
							label="Nombre / Razón Social"
							value={value}
							onChange={onChange}
						/>
					)}
				/>
			</Grid> */}
			{/* <Grid className={classes.column} item xs={12} md={3}>
				<Controller
					name="tipoCuentaObj"
					control={controlBeneficiario.control}
					rules={{
						required: 'Este campo es requerido.',
					}}
					render={({
						field: { onChange, value },
						fieldState: { error },
					}) => (
						<CHSelect
							error={!!error}
							label="Tipo de Cuenta"
							value={value}
							helperText={error ? error.message : null}
							onChange={value => {
								const dato = value.target.value as string;
								const idEntidadBancaria = dato.split('_')[0];
								if (idEntidadBancaria === '322') {
									setReadOnlyEntidad(true);
									setReadOnlyNumeroCuenta(true);
									controlBeneficiario.setValue('entidadObj', '');
									controlBeneficiario.setValue('numeroCuenta', '');
									controlBeneficiario.register('entidadObj', { required: false });
									controlBeneficiario.register('numeroCuenta', { required: false });
									controlBeneficiario.clearErrors('entidadObj');
									controlBeneficiario.clearErrors('numeroCuenta');
								} else {
									setReadOnlyEntidad(false);
									setReadOnlyNumeroCuenta(false);
									controlBeneficiario.register('entidadObj', { required: 'Este campo es requerido.' });
									controlBeneficiario.register('numeroCuenta', { required: 'Este campo es requerido.' });
								}
								onChange(value);
							}}
						>
							{listaValorCatalogo?.map(
								(option: VALORCATALOGO) => {
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
			</Grid> */}
			{/* <Grid className={classes.column} item xs={12} md={3}>
				<Controller
					name="entidadObj"
					control={controlBeneficiario.control}
					render={({
						field: { onChange, value },
						fieldState: { error },
					}) => (
						<CHSelect
							error={!!error}
							label="Entidad"
							readOnly={readOnlyEntidad}
							value={value}
							helperText={error ? error.message : null}
							onChange={value => {
								onChange(value);
							}}
						>
							{listaEntidadBancaria?.map(
								(option: ENTIDADBANCARIA) => {
									const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
									return (
										<MenuItem
											key={
												option.idEntidadBancaria
											}
											value={dato}
										>
											{option.nombreRazonSocial}
										</MenuItem>
									);
								},
							)}
						</CHSelect>
					)}
				/>
			</Grid> */}
			{/* <Grid className={classes.column} item xs={12} md={3}>
				<Controller
					name="numeroCuenta"
					control={controlBeneficiario.control}
					render={({
						field: { onChange, value },
						fieldState: { error },
					}) => (
						<CHTextField
							numberMask
							error={!!error}
							helperText={error ? error.message : null}
							isReadOnly={readOnlyNumeroCuenta}
							label="Número"
							value={value}
							onChange={onChange}
						/>
					)}
				/>
			</Grid> */}

			<Grid container>
				{
					responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?


						<Grid
							className={classes.columnButtonAdd}
							item
							xs={12}
							md={12}
						><Button
							className={classes.buttonAdd}
							variant="outlined"
							onClick={controlBeneficiario.handleSubmit(
								agregarTerceroBeneficiario,
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
						</Grid> : ''
				}

			</Grid>
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
								<TableCell>Número de Cuenta</TableCell>
								<TableCell>Valor a retirar</TableCell>
								<TableCell align="center">
									Acciones
								</TableCell>
							</TableRow>
						</TableHead>
						<TableBody>
							{terceroBeneficiarioModel?.map(
								(option: TerceroBeneficiarioModel) => (
									<TableRow key={option.id}>
										<TableCell>
											{option.tipoDocObj.descripcion}
										</TableCell>
										<TableCell>
											{option.numDoc}
										</TableCell>
										<TableCell>
											{option.razonsocial}
										</TableCell>
										<TableCell>
											{option.tipoCuentaObj.descripcion}
										</TableCell>
										<TableCell>
											{option.entidadObj.descripcion !== null ? option.entidadObj.descripcion : ''}
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
											responseSolicitud?.nombreTarea === 'Radicación solicitud retiro parcial de cesantías' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 8' || responseSolicitud?.nombreTarea === 'Solicitud de retiro V 14' || responseSolicitud?.nombreTarea === 'null' && responseSolicitud?.solicitudTareaEstadoId === 327 ?


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
												</TableCell> : ''
										}

									</TableRow>
								),
							)}
						</TableBody>
					</Table>
				</Grid>
			</TableContainer>
		</CHPaper>
	);
}

export default TerceroBeneficiario;
