import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { Button, createStyles, Icon, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { AppState } from '../../../store/rootReducer';
import {
	CITYDTO,
	DEPARTAMENTODTO,
	IDENTIFICATIONTYPEDTO,
	VALORCATALOGO,
} from '../../../types/catalog.type';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import {
	EntidadesEducativasFiltro,
	EntidadesEducativasFiltroInitial,
} from '../../../types/educational.entities.type';
import { startGetArchivosPorEntidad, startUpdateEducationalEntities,startPostCrearHistorialEntidad, startGetEducationalEntitiesDetail } from '../../../actions/educationalEntities.actions';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import { LoadingComponent } from '../../../components/loading/loading.component';
import AddressComponent from '../../../components/modals/address/address.component';
import { Address } from '../../../types/address.type';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles(() =>
	createStyles({
		restoreButton: {
			width: '250px',
			marginLeft: '.5rem',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		link: {
			width: '100px',
			display: 'flex',
			flexDirection: 'column',
			justifyContent: 'center',
			alignItems: 'center',
			textDecoration: 'none'
		},
		textLink: {
			fontSize: '15px',
			color: '#42b7ef',
			margin: 0,
			padding: 0,
			textDecoration: 'none'
		},
		iconLink: {
			fontSize: '50px',
			color: '#42b7ef',
			margin: 0,
			padding: 0
		},
		row: {
			display: 'flex',
			flexDirection: 'row',
			alignItems: 'center'
		},
		customDiv: {
			display: 'flex',
			flexDirection: 'column',
			marginTop: '10px'
		}
	}),
);

function BasicInformationEntities({
	catalogState,
	educationalEntities,
	startUpdateEducationalEntities,
	loading,
}: {
	catalogState: CatalogState;
	educationalEntities: EducationalEntitiesState;
	startUpdateEducationalEntities: (
		entidadEducativa: EntidadesEducativasFiltro,
	) => void;
	loading: boolean;
}) {
	const classes = useStyles();
	const { control, setValue, watch, handleSubmit } =
		useForm<EntidadesEducativasFiltro>({
			defaultValues: EntidadesEducativasFiltroInitial,
		});

	useEffect(() => {
		const fields = Object.keys(educationalEntities.detalleEntidadEducativa);
		fields.forEach(field => {
			const value =
				educationalEntities.detalleEntidadEducativa[field as keyof EntidadesEducativasFiltro];
			setValue(field as any, value);
		});

	}, [educationalEntities.detalleEntidadEducativa]);

	const updateBasicInformation = (data: EntidadesEducativasFiltro) => {
		const user = AuthService.getUserInfo();
		data.modificadoPor = user.USR_ID;
		try {
			startUpdateEducationalEntities(data);
			
		} catch (error) {
			toast('Error al actualizar la información o al crear el historial.', { type: toast.TYPE.ERROR });
		}
	};

	const catalogCity = [...catalogState.City];
	const estado = educationalEntities.detalleEntidadEducativa.estado === true ? 'ACTIVA' : 'INACTIVA';
	const [departamentoValue, setDepartamentoValue] =
		React.useState<DEPARTAMENTODTO | null>(null);
	const [ciudadValues, setCiudadValues] =
		React.useState<CITYDTO[]>(catalogCity);

	let defaultDepartment: any = catalogState.departments?.find(
		(x: DEPARTAMENTODTO) => x.id === educationalEntities?.detalleEntidadEducativa?.departamento,
	);

	if (
		defaultDepartment != null &&
		departamentoValue == null &&
		defaultDepartment.id > 0
	) {
		setTimeout(() => {
			setDepartamentoValue(defaultDepartment);
		}, 2000);

	}

	// Autocomplete ciudad
	const [ciudadValue, setCiudadValue] = React.useState<CITYDTO | null>(null);
	const [nullCity, setNullCity] = React.useState(false);

	const defaultCity = catalogCity?.find(
		(x: CITYDTO) => x.id === educationalEntities?.detalleEntidadEducativa?.ciudad,
	);
	if (defaultCity != null && ciudadValue == null && defaultCity.id > 0) {
		setTimeout(() => {
			setCiudadValue(defaultCity);
			defaultDepartment = catalogState.departments?.find(
				(x: DEPARTAMENTODTO) => x.id === defaultCity?.idDepartamento,
			);
			setDepartamentoValue(defaultDepartment as DEPARTAMENTODTO);
		}, 2000);

	}

	const onChangeDepartment = (
		event: any,
		newValue: DEPARTAMENTODTO | null,
	) => {
		setCiudadValues(
			catalogCity.filter(x => x.idDepartamento === newValue?.codigo),
		);
		setDepartamentoValue(newValue);
		setNullCity(true);
		setValue('departamento', newValue?.id as number);
		educationalEntities.detalleEntidadEducativa.departamento = newValue?.id as number;
		educationalEntities.detalleEntidadEducativa.ciudad = 0;
		setValue('ciudad', 0);
	};

	const onChangeCity = (event: any, newValue: CITYDTO | null) => {
		setNullCity(false);
		setCiudadValue(newValue);
		setValue('ciudad', newValue?.id as number);
		educationalEntities.detalleEntidadEducativa.ciudad = newValue?.id as number;
	};

	const [open, setOpen] = useState(false);
	const handleOpenAddress = () => {
		setOpen(true);
	};

	const onClose = (value: Address) => {
		if (value) {
			const result = setFormDataAddres(value);
			setValue('direccion', result);
		}
		setOpen(false);
	};
	
	return (
		<Grid container>
			<LoadingComponent key="loader" loading={loading} />
			<Grid container md={12}>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="tipoIdentificacion"
							rules={{
								validate: () =>
									watch('tipoIdentificacion') === 0 ||
										watch('tipoIdentificacion') == null
										? 'El tipo de identificación es obligatorio'
										: true,
							}}
							control={control}
							render={({ field: { onChange, value }, fieldState: { error } }) => (
								<CHSelect
									disabled
									defaultValue=""
									label="Tipo de identificación *"
									onChange={onChange}
									value={value}
									error={!!error}
									helperText={error ? error.message : null}
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
				</Grid>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="nit"
							control={control}
							rules={{
								required: 'Este campo es requerido.',
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
								pattern: {
									value: /(^[0-9]+-?[0-9]{1})$/g,
									message: 'Ingrese un número de documento válido',
								},
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									disabled
									InputLabelProps={{ shrink: true }}
									label="Número de identificación *"
									onChange={onChange}
									value={value}
									error={!!error}
									helperText={
										error ? error.message : null
									}
								/>
							)}
						/>
					</Grid>
				</Grid>
			</Grid>
			<Grid container md={12}>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="razonSocial"
							rules={{
								required: 'Este campo es requerido.',
							}}
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									label="Nombre entidad educativa o razón social *"
									onChange={onChange}
									value={value}
									error={!!error}
									helperText={
										error ? error.message : null
									}
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="correoElectronico"
							rules={{
								pattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
								required: 'Este campo es requerido.',
							}}
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									label="Correo Electrónico *"
									onChange={onChange}
									value={value}
									error={!!error}
									helperText={
										error
											? 'Ingrese un correo electrónico válido'
											: null
									}
								/>
							)}
						/>
					</Grid>
				</Grid>
			</Grid>
			<Grid container md={12}>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="departamento"
							rules={{
								validate: () => watch('departamento') === 0 || watch('departamento') == null ? 'Este campo es requerido' : true,
							}}
							control={control}
							render={({ fieldState: { error } }) => (
								<CHAutoComplete
									value={departamentoValue}
									options={catalogState?.departments}
									getOptionLabel={option => option.nombre}
									onChange={onChangeDepartment}
									renderInput={params => (
										<CHTextField
											{...params}
											label="Departamento*"

											error={!!error}
											helperText={
												error ? error.message : null
											}
										/>
									)}
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="ciudad"
							control={control}
							rules={{
								validate: () => watch('ciudad') === 0 || watch('ciudad') == null ? 'Este campo es requerido' : true,
							}}
							render={({ fieldState: { error } }) => (
								<CHAutoComplete
									value={
										nullCity
											? null
											: ciudadValue
									}
									options={ciudadValues}
									getOptionLabel={option => option.nombre}
									onChange={onChangeCity}
									renderInput={params => (
										<CHTextField
											{...params}
											label="Ciudad*"
											error={!!error}
											helperText={
												error ? error.message : null
											}
										/>
									)}
								/>
							)}
						/>
					</Grid>
				</Grid>
			</Grid>
			<Grid container md={12}>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="direccion"
							control={control}
							rules={{
								required: {
									value: true,
									message: 'Este campo es requerido',
								},
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									label="Dirección *"
									onChange={onChange}
									value={value}
									error={!!error}
									helperText={
										error ? error.message : null
									}
									onClick={handleOpenAddress}
									isReadOnly
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="telefono"
							rules={{
								pattern: {
									value: /\(?([3-36-6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
									message:
										'El télefono debe empezar por 3 o 6 y tener 10 dígitos',
								},
							}}
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									InputLabelProps={{ shrink: true }}
									label="Teléfono *"
									inputProps={{
										inputMode: 'numeric',
										maxLength: 10,
									  }}
									  onChange={(e) => {
										const soloNumeros = e.target.value.replace(/\D/g, '');
										onChange(soloNumeros);
									}}
									value={value}
									error={!!error}
									helperText={
										error ? error.message : null
									}
								/>
							)}
						/>
					</Grid>
				</Grid>
			</Grid>
			<Grid container md={12}>
				<Grid className="item-containerModal" md={5}>
					<Grid className="subcontainerModal">
						<Controller
							name="contacto"
							rules={{
								pattern: {
									value: /^[ A-zÀ-ú ]+$/g,
									message: 'El nombre solo debe contener letras'
								}
							}}
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									label="Nombre contacto"
									onChange={onChange}
									value={value}
									error={!!error}
									helperText={
										error ? error.message : null
									}
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid className="item-containerModal" md={5}>
					<Grid className={classes.row}>
						<Controller
							name="esAcreditada"
							control={control}
							rules={{
								validate: () => watch('esAcreditada') === 0 ? 'Este campo es requerido' : true
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Entidad Acreditada *"
									value={value}
									helperText={error ? error.message : null}
									onChange={onChange}
								>
									{catalogState.cuentaAcreditada?.map(
										(option: VALORCATALOGO) => (
											<MenuItem
												key={option.idValor}
												value={option.idValor}
											>
												{option.descripcion}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
						<div className={classes.customDiv}>
							<Link className={classes.link} to={{ pathname: "http://siet.mineducacion.gov.co/siet/" }} target="_blank">
								<p className={classes.textLink}>SIET</p>
								<Icon className={classes.iconLink}>language</Icon>
							</Link>
							<Link className={classes.link} to={{ pathname: "https://hecaa.mineducacion.gov.co/consultaspublicas/programas" }} target="_blank">
								<p className={classes.textLink}>SNIES</p>
								<Icon className={classes.iconLink}>language</Icon>
							</Link>
						</div>
					</Grid>
				</Grid>
			</Grid>
			<Grid container md={12}>
				<Grid className="item-containerModal" md={5}>
					<Grid className={classes.row}>
						<Controller
							name="tipoPersona"
							control={control}
							rules={{
								validate: () => watch('tipoPersona') === 0 ? 'Este campo es requerido' : true
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									disabled
									error={!!error}
									label="Tipo de persona *"
									value={value}
									helperText={error ? error.message : null}
									onChange={onChange}
								>
									{catalogState.tipoPersona?.map(
										(option: VALORCATALOGO) => (
											<MenuItem
												key={option.idValor}
												value={option.idValor}
											>
												{option.descripcion}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
				</Grid>
				<Grid className="item-containerModal" md={5}>
					<Grid className={classes.row}>
						<Controller
							name="estado"
							control={control}
							render={({
								fieldState: { error },
							}) => (
								<CHTextField
									label="Estado"
									InputLabelProps={{ shrink: true }}
									value={estado}
									isReadOnly
									error={!!error}
									helperText={
										error ? error.message : null
									}
								/>
							)}
						/>
					</Grid>
				</Grid>
			</Grid>
			<Grid container md={12}>
				<Grid md={5}>
					<Button
						color="primary"
						className={classes.restoreButton}
						onClick={handleSubmit(updateBasicInformation)}
					>
						ACTUALIZAR
					</Button>
				</Grid>
			</Grid>
			<AddressComponent
				onOpen={open}
				onClose={onClose}
				control={control}
				getValues={watch}
				tipoDireccion={open}
				setValue={setValue}
			/>
		</Grid>
	);
}

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	educationalEntities: state.educationalEntities,
	loading: state.educationalEntities.loading
});

const mapDispatchToProps = (dispatch: any) => ({
	startUpdateEducationalEntities: (data: EntidadesEducativasFiltro) =>
		dispatch(startUpdateEducationalEntities(data)),
	obtenerArchivosPorEntidad: (idEntidad: string) =>
		dispatch(startGetArchivosPorEntidad(idEntidad))
});

export default connect(mapStateToProps, mapDispatchToProps)(BasicInformationEntities);