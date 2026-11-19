import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import {
	Button,
	createStyles,
	Grid,
	Icon,
	makeStyles,
	MenuItem,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { AppState } from '../../../store/rootReducer';
import history from '../../../utils/history.router';
import CHSelectV2 from '../../../components/ch-select/CHSelectV2';
import CHPageTitle2 from '../../../components/ch-page-title/CHPageTitleV2';
import {
	CITYDTO,
	DEPARTAMENTODTO,
	IDENTIFICATIONTYPEDTO,
	VALORCATALOGO,
} from '../../../types/catalog.type';
import CHTextFieldV2 from '../../../components/ch-text-field/CHTextFieldV2';
import {
	EntidadesEducativasAgregar, EntidadesEducativasAgregarInitial,
} from '../../../types/educational.entities.type';
import { startAddEducationalEntities } from '../../../actions/educationalEntities.actions';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import CHAutoCompleteV2 from '../../../components/ch-auto-complete/CHAutoCompleteV2';
import AddressComponent from '../../../components/modals/address/address.component';
import { Address } from '../../../types/address.type';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
	createStyles({
		button3: {
			width: '250px',
			marginLeft: '3rem',
			background: '#1C3F8B',
			color: '#FFFFFF',
			textAlign: 'center',
			borderRadius: '5%',
			marginBottom: '5px',
			marginTop: '10px',
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

const useStylesLabel = makeStyles(() =>
	createStyles({
		root: {
			width: '100%',
			maxWidth: '100%',
			textAlign: 'left',
			color: '#42b7ef',
			fontSize: '1.2rem'
		},
	}),
);

function AddEducationalEntities({
	catalogState,
	educationalEntitiesState,
	startAddEducationalEntities,
}: {
	catalogState: CatalogState;
	educationalEntitiesState: EducationalEntitiesState;
	startAddEducationalEntities: (data: EntidadesEducativasAgregar) => void;
}): any {

	const classesLabel = useStylesLabel();

	const { control, handleSubmit, watch, setValue } = useForm<EntidadesEducativasAgregar>({
		defaultValues: EntidadesEducativasAgregarInitial,
	});

	const classes = useStyles();
	const [estado, setEstado] = useState('Activo');

	// Autocomplete departamentoNacimiento
	const catalogCity = [...catalogState.City];
	const [departamentoValue, setDepartamentoValue] =
		React.useState<DEPARTAMENTODTO | null>(null);
	const [ciudadValues, setCiudadValues] =
		React.useState<CITYDTO[]>(catalogCity);

	// Autocomplete ciudad
	const [ciudadValue, setCiudadValue] =
		React.useState<CITYDTO | null>(null);
	const [nullCity, setNullCity] = React.useState(false);

	const onChangeDepartment = (
		event: any,
		newValue: DEPARTAMENTODTO | null,
	) => {
		setNullCity(true);
		setCiudadValue(null);
		setDepartamentoValue(newValue);
		setCiudadValues(catalogCity.filter(x => x.idDepartamento === newValue?.codigo));
		setValue('departamento', newValue?.id as number)
	};

	const onChangeCity = (event: any, newValue: CITYDTO | null) => {
		setNullCity(false);
		setCiudadValue(newValue);
		setValue('ciudad', newValue?.id as number)
	};

	const AddEntidadEducativa = async (data: EntidadesEducativasAgregar) => {
		if (departamentoValue === null || ciudadValue === null) {
			toast('Error: La ciudad o el departamento es requerido!.');
		}
		else if (data.tipoIdentificacion === 0) {
			toast('Error: El tipo de identificación es requerido!.');
		} else {
			data.ciudad = ciudadValue.id;
			data.departamento = departamentoValue.id;
			data.estado = true;

			startAddEducationalEntities(data);
		}
	}

	const [open, setOpen] = useState(false);
	const handleOpenAddress = () => {
		setOpen(true);
	};

	const onClose = (value: Address) => {
		
		if(value !== null){
			const result = setFormDataAddres(value);
			setValue('direccion', result);
		}
		setOpen(false);
	};

	const handleEstadoChange = (event: any) => {
	  setEstado(event.target.value);
	};

	return (
		<CHPrivateContainer>
			<CHPageTitle2
				topText="Gestión de afiliados"
				topText2="Gestión de entidades"
				title1="Agregar Entidad"
				title2="Educativa"
				icono="school"
			/>
			<LoadingComponent
				key="loader"
				loading={educationalEntitiesState.loading}
			/>
			<CHPaperNew>
				<Grid container md={12}>
					<Grid className="item-containerModal" md={5}>
						<Controller
							name="tipoIdentificacion"
							control={control}
							rules={{
								validate: () => watch('tipoIdentificacion') === 0 ? 'Este campo es requerido' : true
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Tipo de identificación *"
									value={value}
									helperText={error ? error.message : null}
									onChange={onChange}
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
					<Grid className="item-containerModal" md={5}>
						<Grid className="subcontainerModal">
							<Controller
								name="nit"
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
								}}
								control={control}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										numberMask
										error={!!error}
										helperText={error ? error.message : null}
										label="Número de identificación *"
										onChange={onChange}
										value={value}
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
								control={control}
								rules={{
									required: {
										value: true,
										message: 'Este campo es requerido.',
									},
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										error={!!error}
										helperText={error ? error.message : null}
										label="Nombre entidad educativa o razón social *"
										onChange={onChange}
										value={value}
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
									pattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g

								}}
								control={control}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										error={!!error}
										helperText={
											error
												? 'Ingrese un correo electrónico válido'
												: null
										}
										label="Correo Electrónico *"
										onChange={onChange}
										value={value}
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
								control={control}
								rules={{
									validate: () => watch('departamento') === 0 || watch('departamento') == null ? 'Este campo es requerido' : true
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHAutoCompleteV2
										value={departamentoValue}
										options={catalogState?.departments}
										getOptionLabel={option => option.nombre}
										onChange={onChangeDepartment}
										renderInput={params => (
											<CHTextField
												name="departamento"
												{...params}
												InputLabelProps={{ shrink: true }}
												label="Departamento *"
												value={departamentoValue}
												error={!!error}
												helperText={error ? error.message : null}
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
									validate: () => watch('ciudad') === 0 || watch('ciudad') == null ? 'Este campo es requerido' : true
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHAutoCompleteV2
										value={ciudadValue == null ? nullCity : ciudadValue}
										options={ciudadValues}
										getOptionLabel={option => option.nombre}
										onChange={onChangeCity}
										renderInput={params => (
											<CHTextField
												name="ciudad"
												{...params}
												InputLabelProps={{ shrink: true }}
												label="Ciudad *"
												value={ciudadValue}
												error={!!error}
												helperText={error ? error.message : null}
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
									required: 'Este campo es requerido.'
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										error={!!error}
										helperText={error ? error.message : null}
										label="Dirección *"
										onChange={onChange}
										value={value}
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
								control={control}
								rules={{
									pattern: {
										value: /\(?([3-36-6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
										message:
											'El télefono debe empezar por 3 o 6 y tener 10 dígitos',
									},
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										error={!!error}
										helperText={
											error ? error.message : null
										}
										label="Teléfono"
										onChange={onChange}
										value={value}
										cellPhoneMask
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
								control={control}
								rules={{
									pattern: {
										value: /^[ A-zÀ-ú ]+$/g,
										message: 'El nombre solo debe contener letras'
									}
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										error={!!error}
										helperText={error ? error.message : null}
										label="Nombre contacto"
										onChange={onChange}
										value={value}
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
								rules={{
									
									required: 'Este campo es requerido.',
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHSelect
										label="Estado"
										value={estado}
										onChange={handleEstadoChange}
									>
										
										<MenuItem value="Activo">ACTIVO</MenuItem>
										<MenuItem value="Inactivo">INACTIVO</MenuItem>
									</CHSelect>
								)}
							/>
						</Grid>
					</Grid>
				</Grid>
				<Grid item md={3}>
					<Button
						className={classes.button3}
						onClick={handleSubmit(AddEntidadEducativa)}
					>
						AGREGAR ENTIDAD EDUCATIVA
					</Button>
					<Button
						className={classes.button3}
						onClick={() => history.push('/EntidadesEducativas')}
					>
						REGRESAR
					</Button>
				</Grid>
			</CHPaperNew>
			<AddressComponent
				onOpen={open}
				onClose={onClose}
				control={control}
				getValues={watch}
				tipoDireccion={open}
				setValue={setValue} 
			/>
		</CHPrivateContainer>
	);
}

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	loginState: state.login,
	educationalEntitiesState: state.educationalEntities,
});

const mapDispatchToProps = {
	startAddEducationalEntities
};

export default connect(
	mapStateToProps,
	mapDispatchToProps,
)(AddEducationalEntities);
