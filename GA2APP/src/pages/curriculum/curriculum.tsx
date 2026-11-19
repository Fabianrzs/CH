import React, { useEffect, useState } from 'react';
import { Button, Tab, Tabs, withStyles } from '@material-ui/core';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import { useForm, UseFormTrigger } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { AppState } from '../../store/rootReducer';
import {
	startGetAllGrades,
	startGetAllCities,
	startGetAllLivingPlaces,
	startGetAllForces,
	startGetAllCivilState,
	startGetAllSexTypes,
	startGetAllCountries,
	startGetAllDepartments,
	startGetAllEconomicActivities,
	startGetAllProfessions,
	startGetCategory,
	startGetAllExecutingUnits,
	startGetEmailDomains,
	startGetAddressTypes,
	startGetCurrencyTypes,
} from '../../actions/catalog.action';
import { startUpdateClient,startGetClientByIdentificationTypeIdentificationNumber,startValidatePermisoActualizacion } from '../../actions/client.action';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import PersonalInformation from './components/personalInfo.component';
import FinancialInformation from './components/financialInfo.component';
import ContactInformation from './components/contactInfo.component';
import SpouseInformation from './components/spouseInfo.component';
import { startGetGruposUsuarioPorId } from '../../actions/grupoUsers.action';
import {
	ClienteDto,
	RestaurarIntegracionDto,
} from '../../types/client.model.type';
import { ClientForm } from './constants/form.mode.type';
import NotificationService from '../../services/notification.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import ApiClients from '../../services/api-clients.service';
import { LoginState } from '../../reducers/login.reducer';
import { ClientState } from '../../reducers/client.reducer';
import { CatalogState } from '../../reducers/catalog.reducer';
import BeneficiariesComponent from './components/beneficiaries.component';
import history from '../../utils/history.router';
import SolicitudesComponent from './components/solicitudes.component';
import EmbargosComponent from './components/embargos.component';
import BloqueosComponent from './components/bloqueos.component';
import AportesComponent from './components/aportes.component';
import EstadosComponent from './components/estados.component';
import AnotacionesComponent from './components/anotaciones.component';
import HistorialComponent from './components/historialCuentas.component';
import { ReporteConsultasRestrictivas } from '../productClient/components/TablasReportes';
import AuthService from '../../services/auth.service';

const TABS_INDEX = {
	BASIC_DATA: 0,
	FINANCIAL_INFO: 1,
	CONTACT_INFO: 2,
	SPOUSE_INFO: 3,
	BENEFICIARIES_INFO: 4,
	SOLICITUDES_INFO: 5,
	EMBARGOS: 6,
	BLOQUEOS: 7,
	APORTES: 8,
	ESTADOS: 9,
	ANOTACIONES: 10
};

const clientApi = ApiClients();

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
			borderRadius: '10%',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		restoreButton: {
			width: '250px',
			marginLeft: '.5rem',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		column2: {
			padding: '10px',
			textAlign: 'left',
		},
	}),
);

const AntTabs = withStyles({
	root: {
		borderBottom: '1px solid #e8e8e8',
		marginBottom: '1rem',
		background: '#1C3F8B'
	},
	indicator: {
		backgroundColor: '#FEC917',
	},
})(Tabs);

const AntTab = withStyles((theme: Theme) =>
	createStyles({
		root: {
			textTransform: 'none',
			minWidth: 72,
			marginRight: theme.spacing(4),
			color: '#FFFFFF',
			'&:hover': {
				color: '#1C3F8B',
				opacity: 1,
			},
			'&$selected': {
				color: '#FFFFFF',
				background: '#1C3F8B',
				fontWeight: theme.typography.fontWeightMedium,
			},
			'&:focus': {
				color: '#FEC917',
			},
		},
		selected: {},
	}),
)((props: StyledTabProps) => <Tab disableRipple {...props} />);

interface StyledTabProps {
	label: string;
	value: any;
}

function ViewTabs({
	currentTab,
	onTabChange,
	clientModel
}: {
	currentTab: number;
	onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
	clientModel:ClienteDto;
}) {
	const classes = useStyles();
	return (
		<>  <div>
				<b><h4>{clientModel.idActividadEconomica===0?'INFORMACIÓN FINANCIERA DESACTUALIZADA O NO COMPLETA':' '}</h4></b><br/>
			
			</div>
		<AntTabs
			value={currentTab}
			onChange={onTabChange}
			aria-label="ant example"
		>
			<AntTab
				label="Personal"
				value={TABS_INDEX.BASIC_DATA} />
			<AntTab
				label="Financiera"
				value={TABS_INDEX.FINANCIAL_INFO} />
			<AntTab
				label="Contacto"
				value={TABS_INDEX.CONTACT_INFO} />
			<AntTab
				label="Cónyuge"
				value={TABS_INDEX.SPOUSE_INFO} />
			<AntTab
				label="Beneficiarios"
				value={TABS_INDEX.BENEFICIARIES_INFO} />
			<AntTab
				label="Solicitudes"
				value={TABS_INDEX.SOLICITUDES_INFO} />
			<AntTab
				label="Embargos"
				value={TABS_INDEX.EMBARGOS} />
			<AntTab
				label="Bloqueos"
				value={TABS_INDEX.BLOQUEOS} />
			<AntTab
				label="Aportes"
				value={TABS_INDEX.APORTES} />
			<AntTab
				label="Estados"
				value={TABS_INDEX.ESTADOS} />
			<AntTab
				label="Anotaciones"
				value={TABS_INDEX.ANOTACIONES} />
		</AntTabs></>
	);
}

function Panel({
	currentTabValue,
	loginState,
	clientState,
	catalogState,
	clientModel,
	control,
	setValue,
	watch,
	handleSubmit,
	fechaNacimiento,
	fechaAlta,
	categoriaSoldado,
	estadoCivil,
	ingresoAdicional,
	tipoTelefono,
	tipoIdentificacion,
	tipoDireccion,
	lugarNacimientoExtranjero,
	idTipoIdentificacion,
	idTipoIdentificacionConyuge,
	totalActivos,
	totalPasivos,
	trigger,
	getValues
}: {
	currentTabValue: number;
	loginState: LoginState;
	clientState: ClientState;
	catalogState: CatalogState;
	clientModel: ClienteDto;
	control: any;
	setValue: any;
	watch: any;
	handleSubmit:any;
	fechaNacimiento: any;
	fechaAlta: any;
	categoriaSoldado: boolean;
	estadoCivil: string;
	ingresoAdicional: any;
	tipoTelefono: any;
	tipoIdentificacion: boolean;
	tipoDireccion: boolean;
	lugarNacimientoExtranjero: string;
	idTipoIdentificacion: number | null;
	idTipoIdentificacionConyuge: number | null;
	totalActivos: any;
	totalPasivos: any;
	trigger: UseFormTrigger<any>;
	getValues: any
}) {
	switch (currentTabValue) {
		case TABS_INDEX.BASIC_DATA:
			return (
				<PersonalInformation
					clientState={clientState}
					catalogState={catalogState}
					clientModel={clientModel}
					control={control}
					setValue={setValue}
					watch={watch}
					handleSubmit={handleSubmit}
					fechaNacimiento={fechaNacimiento}
					fechaAlta={fechaAlta}
					categoriaSoldado={categoriaSoldado}
					tipoIdentificacion={tipoIdentificacion}
					lugarNacimientoExtranjero={lugarNacimientoExtranjero}
					idTipoIdentificacion={idTipoIdentificacion} 
					
					/>
			);
		case TABS_INDEX.FINANCIAL_INFO:
			return (
				<FinancialInformation
				    clientState={clientState}
					clientModel={clientModel}
					control={control}
					setValue={setValue}
					watch={watch}
					handleSubmit={handleSubmit}
					ingresoAdicional={ingresoAdicional}
					totalActivos={totalActivos}
					totalPasivos={totalPasivos}
				/>
			);
		case TABS_INDEX.CONTACT_INFO:
			return (
				<ContactInformation
					clientModel={clientModel}
					control={control}
					setValue={setValue}
					handleSubmit={handleSubmit}
					watch={watch}
					tipoTelefono={tipoTelefono}
					tipoDireccion={tipoDireccion}
					trigger={trigger}
					getValues={getValues}
				/>
			);
		case TABS_INDEX.SPOUSE_INFO:
			return (
				<SpouseInformation
					control={control}
					estadoCivil={estadoCivil}
					idTipoIdentificacionConyuge={idTipoIdentificacionConyuge}
				/>
			);
		case TABS_INDEX.BENEFICIARIES_INFO:
			return (
				<BeneficiariesComponent catalogState={catalogState} clientModel={clientModel} />
			);
		case TABS_INDEX.SOLICITUDES_INFO:
			return (
				<SolicitudesComponent catalogState={catalogState} clientModel={clientModel} />
			);
		case TABS_INDEX.EMBARGOS:
			return (
				<EmbargosComponent catalogState={catalogState} clientModel={clientModel} />
			);
		case TABS_INDEX.BLOQUEOS:
			return (
				<BloqueosComponent catalogState={catalogState} clientModel={clientModel} />
			);
		case TABS_INDEX.APORTES:
			return (
				<AportesComponent catalogState={catalogState} clientModel={clientModel} />
			);
		case TABS_INDEX.ESTADOS:
			return (
				<EstadosComponent catalogState={catalogState} clientModel={clientModel} setClientModel={undefined} loginState={loginState} />
			);
		case TABS_INDEX.ANOTACIONES:
			return (
				<AnotacionesComponent catalogState={catalogState} clientModel={clientModel} setClientModel={undefined} loginState={loginState}/>
			);
		default:
			return null;
	}
}

function Curriculum({
	loginState,
	clientState,
	catalogState,
	startGetAllGrades,
	startGetAllCities,
	startGetAllLivingPlaces,
	startGetAllForces,
	startGetAllCivilState,
	startGetAllSexTypes,
	startGetAllCountries,
	startGetAllDepartments,
	startGetAllEconomicActivities,
	startGetAllProfessions,
	startGetCategory,
	startUpdateClient,
	startGetClientByIdentificationTypeIdentificationNumber,
	startGetAllExecutingUnits,
	startGetEmailDomains,
	startGetAddressTypes,
	startGetCurrencyTypes,
	startValidatePermisoActualizacion,
	startGetGruposUsuarioPorId
}: {
	loginState: LoginState;
	clientState: ClientState;
	catalogState: CatalogState;
	startGetAllGrades: (claims: string) => void;
	startGetAllCities: () => void;
	startGetAllLivingPlaces: (claims: string) => void;
	startGetAllForces: (claims: string) => void;
	startGetAllCivilState: (claims: string) => void;
	startGetAllSexTypes: (claims: string) => void;
	startGetAllCountries: (claims: string) => void;
	startGetAllDepartments: () => void;
	startGetAllEconomicActivities: (claims: string) => void;
	startGetAllProfessions: (claims: string) => void;
	startGetCategory: (claims: string) => void;
	startUpdateClient: (client: ClienteDto, claims: string) => void;
	startGetAllExecutingUnits: (claims: string) => void;
	startGetEmailDomains: () => void;
	startGetAddressTypes: () => void;
	startGetCurrencyTypes: () => void;
	startGetClientByIdentificationTypeIdentificationNumber:(tIde:number,ide:string)=>void;
	startValidatePermisoActualizacion: (perfil:any) => any;
	startGetGruposUsuarioPorId: (idUsuario:string)=>void ;
}) {
	const [hasLoadedFields, setHasLoadedFields] = useState(false);
	const [clientInfo, setClientInfo] = React.useState(ClientForm);
	const [isLocalLoading, setIsLocalLoading] = useState(false);
	const [searchInfo, setSearchInfo] = useState(false);
	const [account, setAccount] = useState(false);
	const [currentTabValue, setCurrentTabValue] = useState(
		TABS_INDEX.BASIC_DATA,
	);

	useEffect(() => {
		startGetAllGrades(loginState.claims);
		startGetAllLivingPlaces(loginState.claims);
		startGetAllForces(loginState.claims);
		startGetAllCivilState(loginState.claims);
		startGetAllSexTypes(loginState.claims);
		startGetAllCountries(loginState.claims);
		startGetAllEconomicActivities(loginState.claims);
		startGetAllProfessions(loginState.claims);
		startGetAllDepartments();
		startGetAllCities();
		startGetAllExecutingUnits(loginState.claims);
		startGetCategory(loginState.claims);
		setToResponseClient(clientState.client);
		startGetEmailDomains();
		startGetAddressTypes();
		startGetCurrencyTypes();
	}, []);

	const setToResponseClient = (client: ClienteDto | null) => {
		setClientInfo({ ...clientInfo, ...client });
	};

	if (searchInfo) {
		setSearchInfo(false);
		history.push('/HojaDeVida');
	}

	if (account) {
		setAccount(false);
		history.push('/Account');
	}

	const handleTabChange = (
		event: React.ChangeEvent<any>,
		newValue: number,
	) => {
		setCurrentTabValue(newValue);
	};

	const classes = useStyles();

	const { handleSubmit, control, setValue, watch, getValues, trigger } =
		useForm<ClienteDto>({
			defaultValues: ClientForm,
		});

	const fechaNacimiento = watch('fechaNacimiento');
	const fechaAlta = watch('fechaAlta');
	const estadoCivil = watch('estadoCivil');
	const lugarNacimientoExtranjero = watch('lugarNacimientoExtranjero');

	const categoria = watch('idCategoria');
	const idPaisNacimiento = watch('idPaisNacimiento');
	const idPaisResidencia = watch('idPaisResidencia');

	const [categoriaSoldado, setCategoriaSoldado] = React.useState(
		categoria === 4,
	);
	useEffect(() => {
		setCategoriaSoldado(categoria === 4);
	}, [categoria]);


	const tipoIdentificacion = watch('idTipoIdentificacion');
	const idTipoIdentificacionConyuge = watch('idTipoIdentificacionConyuge');
	const [tipoIdentificacionValue, setTipoIdentificacionValue] =
		React.useState(tipoIdentificacion === 1);
	useEffect(() => {
		setTipoIdentificacionValue(tipoIdentificacion === 1);
	}, [tipoIdentificacion]);

	const ingresoAdicional = watch('ingresoAdicional');
	const totalActivos = watch('totalActivos');
	const totalPasivos = watch('totalPasivos');
	const tipoTelefono = watch('idTipoTelefono');
	const tipoCorreo = watch('idTipoCorreo');
	const correo = watch('correo');

	const tipoDireccionId = watch('idTipoDireccion');
	const [tipoDireccionValue, setTipoDireccionValue] = React.useState(
		tipoDireccionId === 1,
	);
	useEffect(() => {
		setTipoDireccionValue(tipoDireccionId === 1);
	}, [tipoDireccionId]);

	if (!hasLoadedFields && clientInfo.identificacion !== '') {
		setHasLoadedFields(true);
		const fields = Object.keys(clientInfo);
		fields.forEach(field => {
			const value = clientInfo[field as keyof ClienteDto];
			setValue(field as any, value);
		});
	}

	
	const clientData = getValues();

	const respuesta = async (data:any) => {
		setHasLoadedFields(true);
		const fields = Object.keys(data);
		fields.forEach(field => {
			const value = data[field as keyof ClienteDto];
			setValue(field as any, value);
		});
		await NotificationService.genericAlert(
			'Actualización con éxito',
			'Se realizó con éxito la actualización.',
		);	
		window.location.reload();
    };

	const handleRestoreInformation = async () => {
		
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Está seguro?',
		);
		try {
			if (confirmAlert) {
				setIsLocalLoading(true);
				const data: RestaurarIntegracionDto = {
					idCliente: clientData.idCliente,
					idInformacionFinanciera: clientData.idInformacionEconomica,
					idDireccion: clientData.idDireccion,
					idTipoIdentificacion: clientData.idTipoIdentificacion,
					numeroIdentificacion: clientData.identificacion,
				};
				const response = await clientApi.restoreIntegration(data);
				if (response.data != null) {
					await respuesta(response.data);
				}
			}
		} catch (e) {
			await NotificationService.genericAlert(
				'Error en la actualización',
				'Ocurrio un error al intentar restaurar, intente de nuevo.',
				'error',
			);
			setIsLocalLoading(false);
			console.error(e);
		}
	};

	if (isLocalLoading) {
		return <CHLoadingProcess />;
	}

	return (
		<CHPrivateContainer>
			<CHPageTitle
				topText="Información afiliados"
				subText=""
				title1="Hoja de"
				title2="Vida"
			/>
			<ViewTabs
				currentTab={currentTabValue}
				onTabChange={handleTabChange}
				clientModel={clientInfo}
			/>
			<Panel
				currentTabValue={currentTabValue}
				loginState={loginState}
				clientState={clientState}
				catalogState={catalogState}
				clientModel={clientInfo}
				control={control}
				setValue={setValue}
				watch={watch}
				fechaNacimiento={fechaNacimiento}
				fechaAlta={fechaAlta}
				categoriaSoldado={categoriaSoldado}
				estadoCivil={estadoCivil}
				ingresoAdicional={ingresoAdicional}
				tipoTelefono={tipoTelefono}
				tipoIdentificacion={tipoIdentificacionValue}
				tipoDireccion={tipoDireccionValue}
				lugarNacimientoExtranjero={lugarNacimientoExtranjero}
				idTipoIdentificacion={tipoIdentificacion}
				idTipoIdentificacionConyuge={idTipoIdentificacionConyuge}
				totalActivos={totalActivos}
				totalPasivos={totalPasivos}
				handleSubmit={handleSubmit}
				trigger={trigger}
				getValues={getValues}
			/>
			<div className="buttons">
				
				<Button
					className={classes.restoreButton}
					variant="outlined"
					onClick={handleRestoreInformation}
				>
					ACTUALIZAR INFORMACIÓN
				</Button>
				<Button
					className={classes.restoreButton}
					variant="outlined"
					onClick={() => setAccount(true)}
				>
					CUENTAS
				</Button>
				<Button
					className={classes.restoreButton}
					variant="outlined"
					type="submit"
					onClick={() => setSearchInfo(true)}
				>
					NUEVA CONSULTA
				</Button>
			</div>
		</CHPrivateContainer>
	);
}

const mapStateToProps = (state: AppState) => ({
	clientState: state.client,
	catalogState: state.catalog,
	loginState: state.login,
});

const mapDispatchToProps = {
	startGetAllGrades,
	startGetAllCities,
	startGetAllForces,
	startGetAllCivilState,
	startGetAllSexTypes,
	startGetAllCountries,
	startGetAllDepartments,
	startGetAllEconomicActivities,
	startGetAllProfessions,
	startGetCategory,
	startUpdateClient,
	startGetAllExecutingUnits,
	startGetEmailDomains,
	startGetAddressTypes,
	startGetAllLivingPlaces,
	startGetCurrencyTypes,
	startValidatePermisoActualizacion
};
export default connect(mapStateToProps, mapDispatchToProps)(Curriculum);
