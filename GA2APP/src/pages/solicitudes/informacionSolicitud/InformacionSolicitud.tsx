import React, { useEffect, useState } from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import { AppState } from '../../../store/rootReducer';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../../actions/client.action';
import { LoginState } from '../../../reducers/login.reducer';
import { ClientState } from '../../../reducers/client.reducer';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
	CATEGORIES,
	ESTADOCIVILDTO,
	CITYDTO,
	DEPARTAMENTODTO,
	FORCEDTO,
	GRADEDTO,
	PHONETYPE,
} from '../../../types/catalog.type';
import apiCatalog from '../../../services/api-catalog.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

const InformacionSolicitud = (props: props) => {
	const classes = useStyles();
	const api = apiCatalog();
	const [ciudadLugarExpedicion, setCiudadLugarExpedicion] = useState<CITYDTO[]>([]);
	const [ciudad, setCiudad] = useState<CITYDTO[]>([]);
	const {
		loginState,
		clientState,
		listaForce,
		listaGrade,
		listaCategorias,
		listaDepartamentos,
		listaEstadoCivil,
		listaTipoTelefonos,
		responseSolicitud,
		obtenerEstadosCiviles,
		startGetClientByIdentificationTypeIdentificationNumber,
	} = props;

	useEffect(() => {
		startGetClientByIdentificationTypeIdentificationNumber(
			loginState.claims,
			Number(responseSolicitud?.tipoIdentificacion),
			responseSolicitud?.identificacionCliente !== undefined
				? responseSolicitud?.identificacionCliente
				: '',
		);
		obtenerEstadosCiviles(loginState.claims);
		obtenerCiudadesLugarExpedicion(
			clientState?.client?.idDepartamentoExpedicionDocumento !== undefined
				? clientState?.client?.idDepartamentoExpedicionDocumento
				: 0,
		);
		obtenerCiudades(
			clientState?.client?.idDepartamentoResidencia !== undefined
				? clientState?.client?.idDepartamentoResidencia
				: 0,
		);
	}, []);

	const obtenerCiudadesLugarExpedicion = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await api.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudadLugarExpedicion(datos);
		}
	};

	const obtenerCiudades = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await api.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudad(datos);
		}
	};

	return (
		<CHPaper title="Información solicitud">
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Nombres"
						InputLabelProps={{ shrink: true }}
						value={responseSolicitud?.nombresCliente || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Apellidos"
						InputLabelProps={{ shrink: true }}
						value={responseSolicitud?.apellidosCliente || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						numberMask
						isReadOnly
						label="Número de documento"
						InputLabelProps={{ shrink: true }}
						value={responseSolicitud?.identificacionCliente || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idCiudadExpedicionDocumento"
						label="Lugar expedición"
						value={
							clientState?.client?.idCiudadExpedicionDocumento || ''
						}
						open={false}
						readOnly
					>
						{ciudadLugarExpedicion?.map((option: CITYDTO) => (
							<MenuItem key={option.id} value={option.codigo}>
								{option.nombre}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
			</Grid>
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idFuerza"
						label="Fuerza"
						value={clientState?.client?.idFuerza}
						open={false}
						readOnly
					>
						{listaForce?.map((option: FORCEDTO) => (
							<MenuItem key={option.id} value={option.id}>
								{option.descripcion}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idGrado"
						label="Grado"
						value={clientState?.client?.idGrado}
						open={false}
						readOnly
					>
						{listaGrade?.map((option: GRADEDTO) => (
							<MenuItem key={option.id} value={option.id}>
								{option.descripcion}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idCategoria"
						label="Categoría"
						value={clientState?.client?.idCategoria}
						open={false}
						readOnly
					>
						{listaCategorias?.map(
							(option: CATEGORIES) => (
								<MenuItem key={option.id} value={option.id}>
									{option.descripcion}
								</MenuItem>
							),
						)}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="estadoCivil"
						label="Estado Civil"
						value={clientState?.client?.estadoCivil}
						open={false}
						readOnly
					>
						{listaEstadoCivil?.map(
							(option: ESTADOCIVILDTO) => (
								<MenuItem key={option.id} value={option.id}>
									{option.descripcion}
								</MenuItem>
							),
						)}
					</CHSelect>
				</Grid>
			</Grid>
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Dirección"
						InputLabelProps={{ shrink: true }}
						value={clientState?.client?.direccion || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Barrio"
						InputLabelProps={{ shrink: true }}
						value={clientState?.client?.barrio || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idDepartamentoResidencia"
						label="Departamento Residencia"
						value={clientState?.client?.idDepartamentoResidencia}
						open={false}
						readOnly
					>
						{listaDepartamentos?.map(
							(option: DEPARTAMENTODTO) => (
								<MenuItem key={option.id} value={option.id}>
									{option.nombre}
								</MenuItem>
							),
						)}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idCiudadResidencia"
						label="Ciudad Residencia"
						value={clientState?.client?.idCiudadResidencia}
						open={false}
						readOnly
					>
						{ciudad?.map((option: CITYDTO) => (
							<MenuItem key={option.id} value={option.codigo}>
								{option.nombre}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
			</Grid>
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idTipoTelefono"
						label="Tipo Teléfono"
						value={clientState?.client?.idTipoTelefono}
						open={false}
						readOnly
					>
						{listaTipoTelefonos?.map(
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
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Número de teléfono"
						InputLabelProps={{ shrink: true }}
						value={clientState?.client?.telefono || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Correo Electrónico"
						InputLabelProps={{ shrink: true }}
						value={clientState?.client?.correo || ''}
					/>
				</Grid>
			</Grid>
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Tipo de Solicitud"
						InputLabelProps={{ shrink: true }}
						value={responseSolicitud?.descripcionProceso || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Modalidad"
						InputLabelProps={{ shrink: true }}
						value={
							responseSolicitud?.descripcionModalidad || ''
						}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Sub-modalidad"
						InputLabelProps={{ shrink: true }}
						value={
							responseSolicitud?.descripcionSubModalidad || ''
						}
					/>
				</Grid>
			</Grid>
		</CHPaper>
	);
};

interface LinkMapStateToProps {
	loginState: LoginState;
	clientState: ClientState;
}

const mapStateToProps = (state: AppState) => ({
	loginState: state.login,
	clientState: state.client,
});

interface LinkMapDispatchToProps {
	responseSolicitud: ResponseSeveranceModel | undefined;
	listaForce: FORCEDTO[];
	listaGrade: GRADEDTO[];
	listaCategorias: CATEGORIES[];
	listaEstadoCivil: ESTADOCIVILDTO[];
	listaDepartamentos: DEPARTAMENTODTO[];
	listaTipoTelefonos: PHONETYPE[];
	obtenerEstadosCiviles: (props: string) => void;
	startGetClientByIdentificationTypeIdentificationNumber: (props: string, idType: number, idNumber: string) => void;
}

const mapDispatchToProps = {
	startGetClientByIdentificationTypeIdentificationNumber,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(InformacionSolicitud);
