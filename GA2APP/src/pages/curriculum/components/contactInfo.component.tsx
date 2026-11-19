import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import { Grid, MenuItem, makeStyles, createStyles, Button, SnackbarClassKey } from '@material-ui/core';
import { Controller, UseFormTrigger } from 'react-hook-form';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import AddressComponent from '../../../components/modals/address/address.component';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { Address } from '../../../types/address.type';
import {
	ADDRESSTYPE,
	CITYDTO,
	DEPARTAMENTODTO,
	MAILTYPE,
	PAISDTO,
	PHONETYPE,
} from '../../../types/catalog.type';
import {
	startGetCitiesByDepartment,
	startGetPhoneTypes,
	startGetMailTypes,
} from '../../../actions/catalog.action';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import { startUpdateClient, startValidatePermisoActualizacion } from '../../../actions/client.action';
import { ClientForm } from '../constants/form.mode.type';
import NotificationService from '../../../services/notification.service';
import history from '../../../utils/history.router';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		check: {
			padding: '10px',
			textAlign: 'left',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
			borderRadius: '10%',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		disabledAutocomplete: {
			'& .MuiInputBase-root.Mui-disabled': {
				color: 'rgba(0, 0, 0, 0.87)',
				WebkitTextFillColor: 'rgba(0, 0, 0, 0.87)',
				opacity: 1,
			},
		},
	}),
);

function ContactInformation({
	catalogState,
	clientModel,
	loginState,
	control,
	watch,
	setValue,
	tipoTelefono,
	startGetPhoneTypes,
	startGetMailTypes,
	tipoDireccion,
	startUpdateClient,
	handleSubmit,
	trigger,
	getValues
}: {
	catalogState: CatalogState;
	clientModel: ClienteDto;
	loginState: LoginState;
	control: any;
	watch: any;
	setValue: any;
	handleSubmit: any;
	tipoTelefono: any;
	startGetPhoneTypes: () => void;
	startGetMailTypes: () => void;
	tipoDireccion: boolean;
	startUpdateClient: (client: ClienteDto, claims: string) => void;
	trigger: UseFormTrigger<any>;
	getValues: any;
}) {
	const classes = useStyles();

	useEffect(() => {
		startGetPhoneTypes();
		startGetMailTypes();
	}, []);

	const [tipoDireccionRural, setTipoDireccionRural] = React.useState(false);
	const idTipoDireccion = watch('idTipoDireccion');
	const [isUpdating, setIsUpdating] = useState(false);

	useEffect(() => {
		setTipoDireccionRural(idTipoDireccion === 2);
	}, [idTipoDireccion]);


	const [paisValue, setPaisValue] = React.useState<PAISDTO | null>(null);
	const [isForeing, setIsForeing] = React.useState(
		clientModel.idPaisResidencia !== 169,
	);
	const catalogCity = [...catalogState.City];

	const defaultCountry = catalogState.countries?.find(
		(x: PAISDTO) => x.codigo === clientModel.idPaisResidencia,
	);

	if (defaultCountry != null && paisValue == null) {
		setPaisValue(defaultCountry);
	}

	const onChangeCountry = (event: any, newValue: PAISDTO | null) => {
		setValue('idPaisResidencia', newValue == null ? 0 : newValue?.codigo);
		setPaisValue(newValue);
		clientModel.idPaisResidencia = newValue?.codigo;
		setIsForeing(newValue?.codigo !== 169);
	};


	const [departamentoValue, setDepartamentoValue] =
		React.useState<DEPARTAMENTODTO | null>(null);

	const [ciudadResidenciaValues, setCiudadResidenciaValues] =
		React.useState<CITYDTO[]>(catalogCity);

	const [nullCiudadResidenciaValue, setNullCiudadResidenciaValue] =
		React.useState(false);

	const defaultDepto = catalogState.departments?.find(
		(x: DEPARTAMENTODTO) =>
			x.codigo === clientModel.idDepartamentoResidencia,
	);

	if (defaultDepto != null && departamentoValue == null) {
		setCiudadResidenciaValues(
			catalogCity.filter(x => x.idDepartamento === defaultDepto.codigo),
		);
		setDepartamentoValue(defaultDepto);
	}

	const onChangeDepartment = (
		event: any,
		newValue: DEPARTAMENTODTO | null,
	) => {
		clientModel.idDepartamentoResidencia = newValue?.codigo;
		const ciudadesArray: React.SetStateAction<CITYDTO[]> = [];
		setNullCiudadResidenciaValue(true);
		setValue(
			'idDepartamentoResidencia',
			newValue == null ? 0 : newValue?.codigo,
		);
		setDepartamentoValue(newValue);
		catalogCity.forEach(element => {
			if (element.idDepartamento === newValue?.id) {
				ciudadesArray.push(element);
			}
		});
		setCiudadResidenciaValues(ciudadesArray);
	};

	const [ciudadResidenciaValue, setCiudadResidenciaValue] =
		React.useState<CITYDTO | null>(null);

	const defaultCity = catalogCity?.find(
		(x: CITYDTO) =>
			x.codigo === clientModel.idCiudadResidencia &&
			x.idDepartamento === clientModel.idDepartamentoResidencia,
	);

	if (defaultCity != null && ciudadResidenciaValue == null) {
		setCiudadResidenciaValue(defaultCity);
	}

	const onChangeCity = (event: any, newValue: CITYDTO | null) => {
		setNullCiudadResidenciaValue(false);
		setValue('idCiudadResidencia', newValue == null ? 0 : newValue?.codigo);
		setCiudadResidenciaValue(newValue);
	};
	const [disabled, setDisabled] = useState(false)
	const [phoneTypeValue, setPhoneTypeValue] = React.useState(false);
	useEffect(() => {
		if (tipoTelefono === 1) {
			setPhoneTypeValue(true);
		} else {
			setPhoneTypeValue(false);
		}
		if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVCONTACTO').length > 0) {
			setDisabled(false)
		} else {
			setDisabled(true)
		}
	}, [tipoTelefono]);


	const [open, setOpen] = useState(false);

	const handleOpenAddress = () => {
		setOpen(true);
	};


	const onClose = (value: Address | null) => {
		if (!value) {
			setOpen(false);
			return;
		}
		const result = setFormDataAddres(value);
		setValue('direccion', result);
		setOpen(false);
	};

	const [clientInfo, setClientInfo] = React.useState(ClientForm);

	const toDecimal = (v: any): number | null => {
		if (v === null || v === undefined || v === '') return null;

		if (typeof v === 'number') {
			return Number.isFinite(v) ? v : null;
		}

		const s = String(v).trim();

		if (!s) return null;

		const cleaned = s
			.replace(/\$/g, '')
			.replace(/\s/g, '')
			.replace(/,/g, '');

		const n = Number(cleaned);

		return Number.isFinite(n) ? n : null;
	};


	const handleClickUpdate = async (data: ClienteDto) => {
		if (isUpdating) {
			return;
		}

		if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVCONTACTO').length === 0) {
			NotificationService.genericAlwarning(
				'Advertencia',
				'No cuenta con permisos para realizar esta acción.',
			);
			return;
		}

		try {
			setIsUpdating(true);
			await validatePermisoActualizar(data);
		} finally {
			setIsUpdating(false);
		}
	};


	function cleanBackendMessage(raw: any): string {
		const msg = String(raw ?? '').trim();

		if (!msg) {
			return 'Ocurrió un error al actualizar.';
		}

		const firstLine = msg.split(/\r?\n/)[0];
		const withoutStack = firstLine.split(' at ')[0];

		const sqlExceptionIndex = withoutStack.indexOf('): ');
		if (sqlExceptionIndex !== -1) {
			return withoutStack.substring(sqlExceptionIndex + 3).trim();
		}

		const colonIndex = withoutStack.indexOf(': ');
		if (colonIndex !== -1) {
			return withoutStack.substring(colonIndex + 2).trim();
		}

		return withoutStack;
	}

	const validarFechaNacimiento = (fechaNacimiento: any): boolean => {
		const fecha = moment(fechaNacimiento);

		if (
			fechaNacimiento === null ||
			fechaNacimiento === undefined ||
			fechaNacimiento === '' ||
			!fecha.isValid()
		) {
			NotificationService.genericAlwarning(
				'Advertencia',
				'Ingrese una fecha de nacimiento válida.'
			);
			return false;
		}

		if (fecha.isSameOrBefore(moment().subtract(100, 'years'), 'day')) {
			NotificationService.genericAlwarning(
				'Advertencia',
				'Ingrese una fecha de nacimiento válida.'
			);
			return false;
		}

		if (fecha.isAfter(moment(), 'day')) {
			NotificationService.genericAlwarning(
				'Advertencia',
				'La fecha de nacimiento no debe ser mayor a la actual.'
			);
			return false;
		}

		return true;
	};

	// async function validatePermisoActualizar(data: any) {
	// 	try {
	// 		data.lugarNacimientoExtranjero = data.lugarNacimientoExtranjero?.toString();

	// 		if (await validateRequiredFields(data)) {
	// 			data.actualiza = 3;
	// 			const user = AuthService.getUserInfo();
	// 			data.usuario = user.USR_USERNAME;

	// 			const payload = {
	// 				...data,
	// 				ingresoMensual: toDecimal(data.ingresoMensual),
	// 				egresoMensual: toDecimal(data.egresoMensual),
	// 				totalActivos: toDecimal(data.totalActivos),
	// 				totalPasivos: toDecimal(data.totalPasivos),
	// 			  };

	// 			await startUpdateClient(payload, loginState.claims);
	// 		}

	// 		return true;
	// 	} catch (err: any) {
	// 		const raw =
	// 			err?.response?.data?.returnMessage ??
	// 			err?.response?.data?.message ??
	// 			err?.data?.returnMessage ??
	// 			err?.message ??
	// 			'Ocurrió un error al actualizar.';

	// 		const clean = cleanBackendMessage(raw);

	// 		if (/tel[eé]fono ya est[aá] registrado/i.test(clean)) {
	// 			setValue('telefono', clientModel?.telefono ?? '', { shouldValidate: true, shouldDirty: false });
	// 			setValue('telefono2', clientModel?.telefono2 ?? '', { shouldValidate: true, shouldDirty: false });
	// 		}

	// 		NotificationService.genericAlwarning('Advertencia', clean);
	// 		return false;
	// 	}
	// }

	async function validatePermisoActualizar(data: any) {
		try {
			data.lugarNacimientoExtranjero = data.lugarNacimientoExtranjero?.toString();

			if (await validateRequiredFields(data)) {
				data.actualiza = 3;
				const user = AuthService.getUserInfo();
				data.usuario = user.USR_USERNAME;

				const payload = {
					...data,
					ingresoMensual: toDecimal(data.ingresoMensual),
					egresoMensual: toDecimal(data.egresoMensual),
					totalActivos: toDecimal(data.totalActivos),
					totalPasivos: toDecimal(data.totalPasivos),
					totalPatrimonio: toDecimal(data.totalPatrimonio),
					montoTransaccionesExtranjero: toDecimal(data.montoTransaccionesExtranjero),
					ingresoAdicional: toDecimal(data.ingresoAdicional),
				};

				await startUpdateClient(payload, loginState.claims);
			}

			return true;
		} catch (err: any) {
			const raw =
				err?.response?.data?.returnMessage ??
				err?.response?.data?.message ??
				err?.data?.returnMessage ??
				err?.returnMessage ??
				err?.message ??
				'Ocurrió un error al actualizar.';

			const clean = cleanBackendMessage(raw);

			if (/tel[eé]fono ya est[aá] registrado/i.test(clean)) {
				setValue('telefono', clientModel?.telefono ?? '', {
					shouldValidate: true,
					shouldDirty: false,
				});

				setValue('telefono2', clientModel?.telefono2 ?? '', {
					shouldValidate: true,
					shouldDirty: false,
				});
			}

			NotificationService.genericAlwarning('Advertencia', clean);
			return false;
		}
	}




	async function validateRequiredFields(data: any) {
		let retBool = false;
		const client = data;
		retBool = true;
		return retBool;
	}
	return (
		<CHPaper title="Información de contacto">

			<AddressComponent
				onOpen={disabled ? false : open}
				onClose={onClose}
				control={control}
				getValues={getValues}
				tipoDireccion={tipoDireccion}
				setValue={setValue}
			/>
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHAutoComplete
						disabled={disabled}
						value={paisValue}
						options={catalogState?.countries}
						getOptionLabel={option => option.nombre}
						onChange={onChangeCountry}
						renderInput={params => (
							<CHTextField {...params} label="País residencia"
								className={disabled ? classes.disabledAutocomplete : ''}
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHAutoComplete
						disabled={disabled ? true : isForeing}
						value={!isForeing ? departamentoValue : null}
						options={catalogState?.departments}
						getOptionLabel={option => option.nombre}
						onChange={onChangeDepartment}
						renderInput={params => (
							<CHTextField
								{...params}
								label="Departamento residencia"
								className={disabled ? classes.disabledAutocomplete : ''}
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					{!isForeing ? (
						<CHAutoComplete
							value={
								nullCiudadResidenciaValue
									? null
									: ciudadResidenciaValue
							}
							options={ciudadResidenciaValues}
							getOptionLabel={option => option.nombre}
							onChange={onChangeCity}
							disabled={disabled}
							renderInput={params => (
								<CHTextField
									{...params}
									label="Ciudad residencia"
									className={disabled ? classes.disabledAutocomplete : ''}
								/>
							)}
						/>
					) : (
						<Controller
							name="ciudadResidenciaExtranjero"
							control={control}
							defaultValue=""
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={disabled}
									label="Ciudad Residencia"
									value={value}
									onChange={onChange}
								/>
							)}
						/>
					)}
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						control={control}
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
								defaultValue=""
								label="Tipo de dirección"
								value={value}
								onChange={onChange}
								readOnly={disabled}
							>
								{catalogState.addressTypes?.map(
									(option: ADDRESSTYPE) => (
										<MenuItem
											key={option.tipoDireccionId}
											value={option.tipoDireccionId}
										>
											{option.descripcion}
										</MenuItem>
									),
								)}
							</CHSelect>
						)}
						name="idTipoDireccion"
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					{!tipoDireccionRural ? (
						<Controller
							name="direccion"

							control={control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							defaultValue=""
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={disabled}
									label="Dirección"
									// value={clientModel.direccion}
									value={value ?? ''}
									onChange={onChange}
									onClick={handleOpenAddress}
								/>
							)}
						/>
					) : (
						<Controller
							name="direccion"
							control={control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							defaultValue=""
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									label="Dirección"
									value={value}
									onChange={onChange}
									isReadOnly={disabled}
								/>
							)}
						/>
					)}
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="barrio"
						control={control}
						defaultValue=""
						rules={{
							required: {
								value: tipoDireccion,
								message: 'Este campo es requerido.',
							},
						}}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<CHTextField
								error={!!error}
								helperText={error ? error.message : null}
								isReadOnly={disabled}
								label="Barrio"
								value={value}
								onChange={onChange}
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						control={control}
						render={({ field: { onChange, value } }) => (
							<CHSelect
								defaultValue=""
								label="Tipo Teléfono preferido"
								value={value}
								onChange={onChange}
								readOnly={disabled}
							>
								{catalogState.phoneTypes?.map(
									(option: PHONETYPE) => (
										<MenuItem
											key={option.idTipoTelefono}
											value={option.idTipoTelefono}
										>
											{option.descripcionTipoTelefono}
										</MenuItem>
									),
								)}
							</CHSelect>
						)}
						name="idTipoTelefono"
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					{phoneTypeValue ? (
						<Controller
							name="telefono"
							control={control}
							defaultValue=""
							rules={{
								required: 'Este campo es requerido.',
								pattern: /\(?([6-6]{1}[0-0]{1}[0-9]{1})\)?([ .-]?) ([0-9]{3})-\2([0-9]{4})/,
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? 'El teléfono debe empezar por 60 y tener una longitud de 10 dígitos' : null}
									isReadOnly={disabled}
									label="Teléfono preferido"
									value={value}
									onChange={onChange}
									cellPhoneMask
								/>
							)}
						/>
					) : (
						<Controller
							name="telefono"
							control={control}
							defaultValue=""
							rules={{
								required: 'Este campo es requerido.',
								pattern: /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
									isReadOnly={disabled}
									label="Teléfono"
									value={value}
									onChange={onChange}
									cellPhoneMask
								/>
							)}
						/>
					)}
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="telefono2"
						control={control}
						defaultValue=""
						rules={{
							pattern: /\(?([0-9]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
						}}
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<CHTextField
								error={!!error}
								helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
								isReadOnly={disabled}
								label="Teléfono 2"
								value={value}
								onChange={onChange}
								cellPhoneMask
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="envioInformacionSMS"
						control={control}
						render={({ field: { onChange, value } }) => (
							<CHCheckbox
								disabled={disabled ? true : phoneTypeValue}
								label="Envío De Información Por SMS"
								onChange={onChange}
								checked={
									phoneTypeValue ? !phoneTypeValue : value
								}

							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						control={control}
						render={({ field: { onChange, value } }) => (
							<CHSelect
								defaultValue=""
								label="Tipo Correo Electrónico Preferido"
								value={value}
								onChange={onChange}
								readOnly={disabled}
							>
								{catalogState.mailTypes?.map(
									(option: MAILTYPE) => (
										<MenuItem
											key={option.idTipoCorreo}
											value={option.idTipoCorreo}
										>
											{option.descripcionTipoCorreo}
										</MenuItem>
									),
								)}
							</CHSelect>
						)}
						name="idTipoCorreo"
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="correo2"
						control={control}
						rules={{
							pattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
							required: 'Este campo es requerido.',
						}}
						defaultValue=""
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<CHTextField
								error={!!error}
								helperText={
									error
										? 'Ingrese un correo electrónico válido'
										: null
								}
								isReadOnly={disabled}
								label="Correo Electrónico"
								value={value}
								onChange={onChange}
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="correo"
						control={control}
						rules={{
							pattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,

						}}
						defaultValue=""
						render={({
							field: { onChange, value },
							fieldState: { error },
						}) => (
							<CHTextField
								error={!!error}
								helperText={
									error
										? 'Ingrese un correo electrónico válido'
										: null
								}
								isReadOnly={disabled}
								label="Correo Electrónico 2"
								value={value}
								onChange={onChange}
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.check} item xs={12} md={3}>
					<Controller
						name="envioInformacionCorreo"
						control={control}
						render={({ field: { onChange, value } }) => (
							<CHCheckbox
								label="Autorizo a la CAJA PROMOTORA DE VIVIENDA MILITAR Y POLICIA el envío de información a las cuentas de correo electrónico."
								onChange={onChange}
								checked={value}
								disabled={disabled}
							/>
						)}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<Controller
						name="fechaActualizaDatosContacto"
						control={control}
						render={({ field: { onChange, value } }) => (

							<CHTextField
								isReadOnly
								defaultValue=""
								label="Fecha actualización contacto"
								value={UtilService.getFullDate(value)}
								InputLabelProps={{
									shrink: true,
								}}
							/>
						)}
					/>

				</Grid>
			</Grid>
			<div className="buttons">
				<Button
					className={classes.button}
					onClick={handleSubmit(handleClickUpdate)}
					disabled={disabled || isUpdating}
				>
					{isUpdating ? 'Guardando...' : 'Actualizar'}
				</Button>

			</div>
		</CHPaper>
	);
}

const mapStateToProps = (state: AppState) => ({
	clientState: state.client,
	catalogState: state.catalog,
	loginState: state.login,
});

const mapDispatchToProps = {
	startGetCitiesByDepartment,
	startGetPhoneTypes,
	startGetMailTypes,
	startUpdateClient,
	startValidatePermisoActualizacion
};

export default connect(mapStateToProps, mapDispatchToProps)(ContactInformation);
