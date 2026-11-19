import React, { useEffect, useState } from 'react';
import { createStyles, Tab, Tabs, Theme, withStyles } from '@material-ui/core';
import { useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import ResumenSolicitud from '../components/resumen-solicitud/resumen-solicitud.component';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import BotoneraInformacionSolicitudComponent from '../components/botonera/botonera-informacion-solicitud-component';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import NotificationService from '../../../services/notification.service';
import { DocumentosPorSubModalidadModel } from '../../../models/documentos-por-submodalidad.model';
import { BOTONERA_SOLICITUD, MODALIDAD_SOLICITUD } from '../../../constants/constants';
import DynamicPage from '../dynamicPage/DynamicPage.component';
import { FormulariosSolicitudDto, FormularioInconsistenciasDto, ComunicacionSolicitud } from '../../../models/requests/solicitud-tarea.model';
import TrazabilidadSolicitud from '../components/trazabilidad/trazabilidad.component';
import CargueDocumentosSolicitud from '../components/cargue-documentos-solicitud/cargue-documentos-solicitud.component';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import CesantiasApi from '../../../services/api-cesantias.service';
import SolicitudApi from '../../../services/api-solicitud.service';
import ModalPago from '../components/DesembolsoSeven/ModalPago';
import ApiReportes from '../../../services/api-reportes.service';
import DescargaSevenPlanilla from '../components/descargablePlanillaSeven/DescargaSevenPlanilla';
import ModalCuentasAfectadas from '../components/DesembolsoSeven/ModalCuentasAfectadas';
import { CatalogState } from '../../../reducers/catalog.reducer';
import HistorialInconsistenciaSolicitud from '../components/historialIconsistenciaSolicitud/HistorialInconsistenciaSolicitud';
import ObservacionesSolicitud from '../components/observaciones/observaciones';
import GenerarComunicaciones from '../components/generar-comunicaciones/generar-comunicaciones.component';
import FormularioInconsistenciasGrupo1 from '../components/formulario-inconsistencias/formulario-inconsistencias-grupo1.component';
import FormCuentasAfectadas from '../components/DesembolsoSeven/FormCuentasAfectadas';
import { UserState } from '../../../reducers/user.reducer';
import AuthService from '../../../services/auth.service';
import { AppState } from '../../../store/rootReducer';
import UtilService from '../../../utils/util.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import CatalogApi from '../../../services/api-catalog.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';

const api = SolicitudApi();
const apiReportes = ApiReportes()

const TABS_INDEX = {
	RESUMENSOLICITUD: 0,
	CARGUEDOCUMENTOS: 1,
	CARGAARCHIVOS: 2,
	VERARCHIVOS: 3,
	SEVEN: 5,
	HISTORIALINCONSISTENCIA: 6,
	COMUNICACIONES: 7,
	INCONSISTENCIAS: 8,
	VERDETALLESOLICITUD: 9,
	VERIFICACIONCUENTAINDIVIDUAL: 10,
	VERIFICACIONESDECUENTA: 11,
	TRAMITEDEPAGO: 12,
};

const AntTabs = withStyles({
	root: {
		borderBottom: '1px solid #e8e8e8',
		marginBottom: '1rem',
	},
	indicator: {
		backgroundColor: '#FEC917',
	},
})(Tabs);

const AntTab = withStyles((theme: Theme) =>
	createStyles({
		root: {
			textTransform: 'none',
			minWidth: 170,
			marginRight: theme.spacing(1),
			'&:hover': {
				color: '#FEC917',
				opacity: 1,
			},
			'&$selected': {
				color: '#FEC917',
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
	responseSolicitud,
	userAsig,
	grupo,
	userIngreso,
}: {
	currentTab: number;
	onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
	responseSolicitud: ResponseSeveranceModel | undefined;
	userAsig: string | undefined;
	grupo: any;
	userIngreso: string;
}) {

	const api = SolicitudApi()


	const gruposRestringidos = [
		"ASESOR_PTO",
		"ASESOR_PUNTO_ATENCION",
		"JEFE OFICINA ASESORA",
		"ASERA_J",
		"ASERA_P",
		"ASERA_T",
		"ASERA_A",
	];


	const AccesoCompleto = userAsig === userIngreso;
	const AccesoRestringido = !AccesoCompleto && gruposRestringidos.includes(grupo);
	const validacionVerificacion = responseSolicitud?.nombreTarea;

	const [trazabilidad, setTrazabilidad] = useState<any[]>([])

	useEffect(() => {
		if (responseSolicitud?.solicitudId) {
			obtenerTrazabilidadSolicitud();
		}
	}, [responseSolicitud?.solicitudId])

	const obtenerTrazabilidadSolicitud = async () => {
		await api.obtenerTrazabilidadSolicitud(responseSolicitud?.solicitudId)
			.then((response) => {
				setTrazabilidad(response.data)
			})
	}

	const normalizar = (texto: any) =>
		String(texto || '')
			.toLowerCase()
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '');

	const esTramitePagoRetiroParcialActual = trazabilidad?.some((item: any) =>
		normalizar(item?.descripcionTarea).includes('tramite de pago retiro parcial') &&
		['PENDIENTE', 'EN VERIFICACION', 'EN VERIFICACIÓN', 'EN TRAMITE', 'EN TRÁMITE']
			.includes(String(item?.descripcionEstadoSolicitud || '').toUpperCase())
	);

	const nombreTareaNormalizado = normalizar(
		responseSolicitud?.nombreTarea,
	);

	const debeMostrarVerificacionesCuenta =
		nombreTareaNormalizado.includes(
			'verificacion cuenta individual',
		) ||
		nombreTareaNormalizado.includes(
			'verificacion embargos',
		) ||
		nombreTareaNormalizado.includes(
			'verificacion novedades',
		) ||
		nombreTareaNormalizado.includes(
			'tramite de pago retiro parcial',
		) ||
		nombreTareaNormalizado.includes(
			'tramite de pago v 8',
		) ||
		nombreTareaNormalizado.includes(
			'tramite de pago v 14',
		);

	return (

		// <AntTabs
		// 	value={currentTab}
		// 	onChange={onTabChange}
		// 	aria-label="ant example"
		// >

		// 	<AntTab label="Resumen " value={TABS_INDEX.RESUMENSOLICITUD} />
		// 	<AntTab label="Cargue de documentos" value={TABS_INDEX.CARGUEDOCUMENTOS} />
		// 	{(AccesoCompleto || AccesoRestringido) && (
		// 		<AntTab label="Detalle Solicitud" value={TABS_INDEX.VERDETALLESOLICITUD} />
		// 	)}
		// 	{AccesoCompleto && !esTramitePagoRetiroParcialActual && (
		// 		<AntTab label="Seguridad documental" value={TABS_INDEX.INCONSISTENCIAS} />
		// 	)}

		// 	{AccesoCompleto && !esTramitePagoRetiroParcialActual && (
		// 		<AntTab label="Generar comunicaciones" value={TABS_INDEX.COMUNICACIONES} />
		// 	)}

		// 	{AccesoCompleto && !esTramitePagoRetiroParcialActual && (
		// 		<AntTab label="Historial de Inconsistencia" value={TABS_INDEX.HISTORIALINCONSISTENCIA} />
		// 	)}


		// 	{/* {(responseSolicitud?.nombreTarea?.includes('Verificación cuenta individual') ||
		// 		responseSolicitud?.nombreTarea?.includes('Verificación embargos') ||
		// 		responseSolicitud?.nombreTarea?.includes('Verificación novedades') ||
		// 		responseSolicitud?.nombreTarea?.includes('Verificación novedades en los datos V 8') ||
		// 		responseSolicitud?.nombreTarea?.includes('Trámite de pago retiro parcial') ||
		// 		responseSolicitud?.nombreTarea?.includes('Trámite de pago V 8') ||
		// 		responseSolicitud?.nombreTarea?.includes('Trámite de pago V 14')) &&
		// 		AccesoCompleto && (
		// 			<AntTab label="Verificaciones de cuenta" value={TABS_INDEX.VERIFICACIONESDECUENTA} />
		// 		)
		// 	} */}
		// 	{!esTramitePagoRetiroParcialActual &&
		// 		(responseSolicitud?.nombreTarea?.includes('Verificación cuenta individual') ||
		// 			responseSolicitud?.nombreTarea?.includes('Verificación embargos') ||
		// 			responseSolicitud?.nombreTarea?.includes('Verificación novedades') ||
		// 			responseSolicitud?.nombreTarea?.includes('Verificación novedades en los datos V 8') ||
		// 			responseSolicitud?.nombreTarea?.includes('Trámite de pago retiro parcial') ||
		// 			responseSolicitud?.nombreTarea?.includes('Trámite de pago V 8') ||
		// 			responseSolicitud?.nombreTarea?.includes('Trámite de pago V 14')) &&
		// 		AccesoCompleto && (
		// 			<AntTab label="Verificaciones de cuenta" value={TABS_INDEX.VERIFICACIONESDECUENTA} />
		// 		)
		// 	}
		// 	{AccesoCompleto &&
		// 		(validacionVerificacion?.includes('Trámite de pago')) && (
		// 			<AntTab
		// 				label="Verificación Beneficiarios de Pago"
		// 				value={TABS_INDEX.TRAMITEDEPAGO}
		// 			/>
		// 		)}
		// </AntTabs>

		<AntTabs
			value={currentTab}
			onChange={onTabChange}
			aria-label="ant example"
		>
			<AntTab
				label="Resumen"
				value={TABS_INDEX.RESUMENSOLICITUD}
			/>

			<AntTab
				label="Cargue de documentos"
				value={TABS_INDEX.CARGUEDOCUMENTOS}
			/>

			{(AccesoCompleto || AccesoRestringido) && (
				<AntTab
					label="Detalle Solicitud"
					value={
						TABS_INDEX.VERDETALLESOLICITUD
					}
				/>
			)}

			{AccesoCompleto && (
				<AntTab
					label="Seguridad documental"
					value={TABS_INDEX.INCONSISTENCIAS}
				/>
			)}

			{AccesoCompleto && (
				<AntTab
					label="Generar comunicaciones"
					value={TABS_INDEX.COMUNICACIONES}
				/>
			)}

			{AccesoCompleto &&
				!esTramitePagoRetiroParcialActual && (
					<AntTab
						label="Historial de Inconsistencia"
						value={
							TABS_INDEX.HISTORIALINCONSISTENCIA
						}
					/>
				)}

			{AccesoCompleto &&
				debeMostrarVerificacionesCuenta && (
					<AntTab
						label="Verificaciones de cuenta"
						value={
							TABS_INDEX.VERIFICACIONESDECUENTA
						}
					/>
				)}

			{AccesoCompleto &&
				nombreTareaNormalizado.includes(
					'tramite de pago',
				) && (
					<AntTab
						label="Verificación Beneficiarios de Pago"
						value={TABS_INDEX.TRAMITEDEPAGO}
					/>
				)}
		</AntTabs>
	);
}

function Panel({
	currentTabValue,
	responseSolicitud,
	formulariosSolicitud,
	documentosAgregados,
	observaciones,
	control,
	setValue,
	getValues,
	setEstadoSubsanar,
	grupo1,
	handleSubmit,
	reset,
	watch,
	listadoInconsistencias,
	addInconsistencia,
	updateInconsistencias,
	deleteInconsistencias,
	inconsistenciasSeleccionadas,
	deshablitarNinguna,
	setDeshabilitarNinguna,
	setCausalYNovedades,
	causalYNovedades,
	siEsAprobadaSolcitud,
	quitarTiposNovedades,
	setQuitarTiposNovedades,
	setTraerIdAntrior,
	caulsaSeleccionadas,
	setResetNovedad,
	resetNovedad,
	setTraerTodaData,
	estadoSubsanar,
	mostrarCheck,
	setLlamarNovedades,
	llamarNovedades,
	setMostrarCheck,
	deshativarBtnEliminarGp2,
	trazabilidad,
	formularioObservaciones,
	refresh,
	formularioInformacionComunicacion,
	formularioVerificacionBeneficiariosPago,
	obtenerPlanillaPdf
}: {
	currentTabValue: number;
	responseSolicitud: ResponseSeveranceModel | undefined;
	formulariosSolicitud: FormulariosSolicitudDto | any;
	documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
	observaciones: string
	control: any;
	setValue: any;
	getValues: any;
	setEstadoSubsanar: (value: boolean) => void;
	grupo1: boolean;
	handleSubmit: any;
	reset: any;
	watch: any;
	listadoInconsistencias: FormularioInconsistenciasDto[];
	addInconsistencia: any;
	updateInconsistencias: any;
	deleteInconsistencias: any;
	inconsistenciasSeleccionadas: any;
	deshablitarNinguna: any;
	setDeshabilitarNinguna: (novedad: boolean) => void;
	setCausalYNovedades: (causalNovedades: boolean) => void;
	causalYNovedades: any;
	siEsAprobadaSolcitud: boolean;
	quitarTiposNovedades: boolean;
	setQuitarTiposNovedades: (value: any) => void;
	setTraerIdAntrior: (idAnterior: any) => void;
	caulsaSeleccionadas: any;
	setResetNovedad: (reset: any) => void;
	resetNovedad: any;
	setTraerTodaData: (data: any) => void;
	estadoSubsanar: any;
	mostrarCheck: any;
	setLlamarNovedades: any;
	llamarNovedades: any;
	setMostrarCheck: any;
	deshativarBtnEliminarGp2: any;
	trazabilidad: any[];
	formularioObservaciones: any;
	refresh: boolean;
	formularioInformacionComunicacion: UseFormReturn<any>;
	formularioVerificacionBeneficiariosPago: UseFormReturn<any>;
	obtenerPlanillaPdf: () => void;
}) {

	const [indexTabValue, setTndexTabValue] = useState(
		TABS_INDEX.CARGAARCHIVOS,
	);

	const [disabledButtons, setTipoProcesar] = useState<boolean>(true);

	const [
		omitirAlertaCausalTemporalmente,
		setOmitirAlertaCausalTemporalmente,
	] = useState(false);

	switch (currentTabValue) {
		case TABS_INDEX.RESUMENSOLICITUD:
			return (
				<>
					<ResumenSolicitud responseSolicitud={responseSolicitud} observaciones={observaciones} />
					<TrazabilidadSolicitud
						responseSolicitud={responseSolicitud}
						observaciones={observaciones}
					/>
				</>
			);
		case TABS_INDEX.CARGUEDOCUMENTOS:
			return (
				<>
					<PanelCargarArchivo
						currentTabValue={indexTabValue}
						responseSolicitud={responseSolicitud}
						documentosAgregados={documentosAgregados}
					/>
					<ObservacionesSolicitud
						control={formularioObservaciones}
						seccion={String(currentTabValue)}
						trazabilidad={trazabilidad}
					/>
				</>
			);

		case TABS_INDEX.VERDETALLESOLICITUD:

			const tipoAccion = (responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.VINUEVA || responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.CNVGR) ? 0 : 5;
			const tipoAccionDetalleProcesar = (responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.VINUEVA || responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.CNVGR) ? disabledButtons : false;
			const { formularios } = formulariosSolicitud;
			return (
				<DynamicPage
					dynamicTitle={{ topText: "", title: "", icon: "" }}
					formsList={formularios}
					withButtons={false}
					responseSolicitud={responseSolicitud}
					tipoAccion={tipoAccion}
					tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
					modalidadSolicitud={responseSolicitud?.timCodigo as string}
					onItemGoBack={() => { }}
					onItemBack={() => { }}
					obtnerBoleano={undefined}
					inicioSolicitud={false}
					siEsAprobadaSolcitud={!false}
					tipoSolicitud={undefined}
				/>
			);
		case TABS_INDEX.COMUNICACIONES:
			return (
				<>
					<GenerarComunicaciones
						idSolicitud={responseSolicitud?.solicitudId || ''}
						responseSolicitud={responseSolicitud}
						formularioInformacionComunicacion={formularioInformacionComunicacion}
					/>
				</>
			);
		case TABS_INDEX.INCONSISTENCIAS:
			return (
				<>
					<FormularioInconsistenciasGrupo1
						control={control}
						setValue={setValue}
						getValues={getValues}
						idSolicitud={responseSolicitud?.solicitudId || ''}
						setEstadoSubsanar={setEstadoSubsanar}
						grupo1={grupo1}
						handleSubmit={handleSubmit}
						reset={reset}
						watch={watch}
						listadoInconsistencias={listadoInconsistencias}
						addInconsistencia={addInconsistencia}
						updateInconsistencias={updateInconsistencias}
						deleteInconsistencias={deleteInconsistencias}
						responseSolicitud={responseSolicitud}
						inconsistenciasSeleccionadas={inconsistenciasSeleccionadas}
						deshablitarNinguna={deshablitarNinguna}
						setDeshabilitarNinguna={setDeshabilitarNinguna}
						setCausalYNovedades={setCausalYNovedades}
						causalYNovedades={causalYNovedades}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						quitarTiposNovedades={quitarTiposNovedades}
						setQuitarTiposNovedades={setQuitarTiposNovedades}
						setTraerIdAntrior={setTraerIdAntrior}
						caulsaSeleccionadas={caulsaSeleccionadas}
						setResetNovedad={setResetNovedad}
						resetNovedad={resetNovedad}
						setTraerTodaData={setTraerTodaData}
						estadoSubsanar={estadoSubsanar}
						mostrarCheck={mostrarCheck}
						setLlamarNovedades={setLlamarNovedades}
						llamarNovedades={llamarNovedades}
						setMostrarCheck={setMostrarCheck}
						deshativarBtnEliminarGp2={deshativarBtnEliminarGp2}
						trazabilidad={trazabilidad}
						refresh={refresh}
						setOmitirAlertaCausalTemporalmente={
							setOmitirAlertaCausalTemporalmente
						}
					/>
					<ObservacionesSolicitud
						control={formularioObservaciones}
						seccion={String(currentTabValue)}
						trazabilidad={trazabilidad}
					/>
				</>
			);

		case TABS_INDEX.HISTORIALINCONSISTENCIA:
			return (<HistorialInconsistenciaSolicitud responseSolicitud={responseSolicitud} />);

		case TABS_INDEX.TRAMITEDEPAGO:
			return (

				<>
					<FormCuentasAfectadas
						responseSolicitud={responseSolicitud}
						formularioObservaciones={formularioObservaciones}
						seccion={String(currentTabValue)}
						trazabilidad={trazabilidad}
						formularioVerificacionBeneficiariosPago={formularioVerificacionBeneficiariosPago}
						obtenerPlanillaPdf={obtenerPlanillaPdf}
					/>
				</>
			)
		case TABS_INDEX.VERIFICACIONESDECUENTA:
			return (
				<>
					<ObservacionesSolicitud
						control={formularioObservaciones}
						seccion={String(currentTabValue)}
						trazabilidad={trazabilidad}
					/>
				</>

			);

		default:
			return null;
	}
}

function PanelCargarArchivo({
	documentosAgregados,
	currentTabValue,
	responseSolicitud,
}: {
	documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
	currentTabValue: number;
	responseSolicitud: ResponseSeveranceModel | undefined;
}) {

	switch (currentTabValue) {
		case TABS_INDEX.CARGAARCHIVOS:
			return (
				<>
					<CargueDocumentosSolicitud
						responseSolicitud={responseSolicitud}
						documentosAgregados={documentosAgregados}
						siEsAprobadaSolcitud={null}
					/>
				</>
			);
		default:
			return null;
	}
}

function ProcesoVerificacionEmbargo({
	responseSolicitud,
	tipoAccionDetalleProcesar,
	formulariosSolicitud,
	onItemGoBack,
	obtnerBoleano,
	siEsAprobadaSolcitud,
	setMensajesEjecutados,
	mensajesEjecutados,
	catalogState,
	userState,
	userAsig,
	grupo,
	userIngreso,
	estadoDeCliente

}: {
	responseSolicitud: ResponseSeveranceModel | undefined;
	tipoAccionDetalleProcesar: boolean;
	formulariosSolicitud: FormulariosSolicitudDto | any;
	onItemGoBack: () => void;
	obtnerBoleano: any | undefined;
	siEsAprobadaSolcitud: boolean;
	setMensajesEjecutados: (valor: any) => void;
	mensajesEjecutados: boolean;
	catalogState: CatalogState;
	userState: UserState;
	userAsig: string | undefined;
	grupo: any;
	userIngreso: string;
	estadoDeCliente: string;
}) {

	const [currentTabValue, setCurrentTabValue] = useState(
		TABS_INDEX.RESUMENSOLICITUD,
	);

	const apiCesantias = CesantiasApi();

	const [cargar, setCargar] = useState(false)
	const [tipoAccionNumber, setAccionNumber] = useState(BOTONERA_SOLICITUD.ACTUALIZAR);
	const [aprobar, setAprobar] = useState(true);
	const [subsanado, setSubsanado] = useState(true);
	const [estadoSubsanar, setEstadoSubsanar] = useState(false);
	const [rechazar, setRechazar] = useState(true);
	const [actualizar, setActualizar] = useState(false);
	const [aprobarBloquear, setaprobarBloquear] = useState(true);
	const [listadoInconsistencias, setListadoInconsistencias] = useState<FormularioInconsistenciasDto[]>([]);
	const [mostrarCheck, setMostrarCheck] = useState(false);
	const [resetNovedad, setResetNovedad] = useState(false);
	const [caulsaSeleccionadas, setCausalSeleccionadas] = useState<string[]>([]);
	const [inconsistenciasSeleccionadas, setInconsistenciasSeleccionadas] = useState<string[]>([]);
	const [deshablitarNinguna, setDeshabilitarNinguna] = useState(false);
	const [causalYNovedades, setCausalYNovedades] = useState(false);
	const [quitarTiposNovedades, setQuitarTiposNovedades] = useState(false);
	const [error, setError] = useState(false);
	const [loading, setLoading] = useState(false);
	const [hablitarActualiza, setHablitarActualiza] = useState(false);
	const [deshabilitarBTNRechazarGp2, setDeshabilitarBTNRechazarGp2] = useState(true);
	const [deshativarBtnEliminarGp2, setDeshativarBtnEliminarGp2] = useState(false);
	const [respoder, setResponder] = useState(false);
	const [traerIdAnterior, setTraerIdAntrior] = useState('');
	const [traerTodaData, setTraerTodaData] = useState<any>({});
	const [llamarNovedades, setLlamarNovedades] = useState(false);
	const [trazabilidad, setTrazabilidad] = useState<any[]>([]);

	const user = AuthService.getUserInfo();

	const formularioVerificacionBeneficiariosPago = useForm({
		defaultValues: {
			numeroCuenta: '',
			descripcionCuenta: '',
			valorPago: '',
			cuentaAfectados: '',
			conceptoAfectados: '',
			disponible: '',
			monto: '',
			interesAcumulado: '',
			entidadBancaria: '',
			numeroCuentaBeneficiario: '',
			valor: ''

		},
	});

	useEffect(() => {
		verificarEstadosNovedades()
		obtenerTrazabilidad();
	}, [respoder]);

	const [titulosIcon, setTitulosIcon] = useState({});
	const [open, setOpen] = useState(false);

	const [documentosAguardar, setDocumentosAguardar] = useState<
		DocumentosPorSubModalidadModel[]
	>([]);

	const { control, watch, getValues, setValue, handleSubmit, reset } = useForm({
		defaultValues: {
			listaInconsistenciaId: null,
			inconsistenciaId: null,
			novedad: 'Seleccione',
			causalInterrupcionId: null,
			herramientaId: null,
			puntoAtencionId: catalogState.puntosAtencion.find(x => x.idPuntosAtencion != null)?.idPuntosAtencion,
			grupoInconsistencia: 0,
			solicitudId: responseSolicitud?.solicitudId,
			tecnicoId: userState?.users?.find(x => x.id != null)?.id,
			objetoEstudio: '',
			analisis: '',
			observacion: '',
			subsanado: false,
			estado: false,
			tecnicoReporta: '',
			puntoAtencion: 1,
			procedeDesbloqueo: false,
			index: 0,
			tipoNovedad: '',
			pagoSucesivo: '',
			informe: '',
			userId: user.USR_ID,
		},
	});

	const obtenerTrazabilidad = async () => {
		await api.obtenerTrazabilidadSolicitud(responseSolicitud?.solicitudId)
			.then((response) => {
				setTrazabilidad(response.data);
			})
	}

	const addInconsistencia = async (data: any) => {

		setMostrarCheck(false);
		setResetNovedad(true);

		const nuevaInconsistencia: string = data?.listaInconsistenciaId;
		const nuevaCausalInterrupcion: string = data.causalInterrupcionId;


		if (!caulsaSeleccionadas.includes(nuevaCausalInterrupcion)) {
			setCausalSeleccionadas((prevCausalInterrupcion) => [
				...prevCausalInterrupcion,
				nuevaCausalInterrupcion,
			]);
		}


		if (!inconsistenciasSeleccionadas.includes(nuevaInconsistencia)) {
			setInconsistenciasSeleccionadas((prevInconsistencias) => [
				...prevInconsistencias,
				nuevaInconsistencia,
			]);
		}

		if (data.novedad === "NINGUNA") {
			setDeshabilitarNinguna(true);
			setValue('subsanado', true);
		}

		if (data.novedad === "PAGOS SUCESIVOS") {
			setDeshabilitarNinguna(true);
		}

		if (
			data.novedad === "NOVEDAD SUBSANABLE" ||
			data.novedad === "ANÁLISIS DESBLOQUEO MATRÍCULAS" ||
			data.novedad === "CAUSAL DE INTERRUPCIÓN"
		) {
			setCausalYNovedades(true);
		}

		if (data?.novedad === "INFORME 114 - INCONSISTENCIA") {
			setDeshabilitarNinguna(true);
		}

		if (data.novedad === 'Seleccione' || data.novedad === 0) {
			toast('Debe seleccionar un campo válido', { type: toast.TYPE.ERROR });
			return;
		}

		const algunSubsanadoFalse = listadoInconsistencias?.some(
			(item: any) => item.subsanado === false || data.subsanado === false
		);



		const isNovedadNingunaGrupo2 = responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 2") && data.novedad === "NINGUNA";

		if (isNovedadNingunaGrupo2) {
			data.subsanado = true;
		}


		setQuitarTiposNovedades(true);

		if (
			responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 2") &&
			!algunSubsanadoFalse
		) {
			setSubsanado(false);
			setRechazar(false);

		} else if (responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 1")) {
			setSubsanado(false);
			setRechazar(false);

		}


		data.grupoInconsistencia = 2;

		const solicitudTareaId = responseSolicitud?.solicitudTareaId
		const nuevaData = { ...data, solicitudTareaId }


		setListadoInconsistencias((prevListado) => [...prevListado, data]);

		if (currentTabValue === TABS_INDEX.INCONSISTENCIAS) {
			if (data.inconsistenciaId === null) {
				data.grupoInconsistencia = 2;
				await api.PostInsertarInconsistenciaSolicitud(nuevaData)
					.then((result) => {
						if (!result.isSuccess) {
							setError(true);
							setCurrentTabValue(TABS_INDEX.INCONSISTENCIAS);
							setAccionNumber(BOTONERA_SOLICITUD.ACTUALIZAR);
							// setResponder(true)
						}

					});
			} else {
				data.grupoInconsistencia = 2;
				await api.PostActualizarInconsistenciaSolicitud(nuevaData)
					.then((result) => {
						if (!result.isSuccess) {
							setError(true);
							setCurrentTabValue(TABS_INDEX.INCONSISTENCIAS);
							setAccionNumber(BOTONERA_SOLICITUD.ACTUALIZAR);
						}
					});
			}

			if (!error) {
				verificarEstadosNovedades();
				setResponder(true);
				toast('Información gurdada correctamente', { type: toast.TYPE.SUCCESS });
			} else {
				toast('Error interno del servidor', { type: toast.TYPE.ERROR });
			}

		}

		reset();
	};
	const updateInconsistencias = async (data: any) => {

		setLoading(true)
		if (data.novedad === "Seleccione") {
			toast('Error la novedad no es validad', { type: toast.TYPE.ERROR });
			setLoading(false)
			return
		}

		if (data.novedad === "NINGUNA") {
			toast('Error la novedad no es validad', { type: toast.TYPE.ERROR });
			setLoading(false)
			return
		}
		setResetNovedad(true)



		const anteriorInconsistencia = listadoInconsistencias.filter((item: any) => item.listaInconsistenciaId === traerIdAnterior);
		const anteriorCaulsaInterrupcion = listadoInconsistencias.filter((item: any) => item.causalInterrupcionId === traerIdAnterior);
		if (anteriorInconsistencia.length !== 0) {
			const nuevosIds = anteriorInconsistencia.map((item: any) => item.listaInconsistenciaId);
			setInconsistenciasSeleccionadas((prevInconsistencias) => prevInconsistencias.filter(id => !nuevosIds.includes(id)));
			const nuevaInconsistencia: string = data?.listaInconsistenciaId;
			if (!nuevosIds.includes(nuevaInconsistencia)) {
				setInconsistenciasSeleccionadas((prevInconsistencias) => [
					...prevInconsistencias,
					nuevaInconsistencia,
				]);
			}
		} else if (anteriorCaulsaInterrupcion.length !== 0) {
			const nuevosIdsCausalInterrupcion = anteriorCaulsaInterrupcion.map((item: any) => item.causalInterrupcionId);
			setCausalSeleccionadas((prevCaulsalInterrupcion) => prevCaulsalInterrupcion.filter(id => !nuevosIdsCausalInterrupcion.includes(id)));
			const nuevaCausalInterrupcion: string = data?.causalInterrupcionId;
			if (!nuevosIdsCausalInterrupcion.includes(nuevaCausalInterrupcion)) {
				setCausalSeleccionadas((prevCaulsalInterrupcion) => [
					...prevCaulsalInterrupcion,
					nuevaCausalInterrupcion,
				]);
			}
		}

		if (data.novedad === "NINGUNA" && !responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 1")) {
			setDeshabilitarNinguna(true);

		} else {

			if (data.novedad === "NOVEDAD SUBSANABLE" || data.novedad === "ANÁLISIS DESBLOQUEO MATRÍCULAS" ||
				data.novedad === "CAUSAL DE INTERRUPCIÓN") {
				setCausalYNovedades(true);

				// setDeshabilitarNinguna(true);
			}
			if (data?.novedad === "INFORME 114 - INCONSISTENCIA" || data?.novedad === "NINGUNA") {
				setDeshabilitarNinguna(true);

			}

		}
		if (data.novedad === "PAGOS SUCESIVOS") {
			setDeshabilitarNinguna(true)

		}

		const items = listadoInconsistencias;
		const item = data;
		const index = watch('index');
		items[index - 1] = item;

		const actualizarHabilitado = listadoInconsistencias?.every((novedad: any) => novedad.subsanado);

		if (responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 2")) {
			setAprobar(true);
			setSubsanado(false);
			setRechazar(false);
			setHablitarActualiza(actualizarHabilitado);
		} else {
			setAprobar(false);
			setSubsanado(false);
			setRechazar(false);
			setHablitarActualiza(actualizarHabilitado);
		}

		if (responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 1")) {
			setHablitarActualiza(true);
		}

		if (responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 1") || responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 2")) {

			if (traerIdAnterior !== '') {

				let request
				if (traerTodaData?.novedad === 'NOVEDAD SUBSANABLE') {
					request = {
						novedad: data.novedad,
						listaInconsistenciaId: traerIdAnterior,
						causalInterrupcionId: '',
						informe: '',
						subsanado: data.subsanado,
						inconsistenciaId_nuevo: data.listaInconsistenciaId,
						causalInterrupcionId_nuevo: '',
						informe_nuevo: '',
						GrupoInconsistencia: traerTodaData.grupoInconsistencia,
					}
				}

				if (traerTodaData?.novedad === "CAUSAL DE INTERRUPCIÓN") {

					request = {
						novedad: data.novedad,
						listaInconsistenciaId: '',
						causalInterrupcionId: traerIdAnterior,
						informe: '',
						subsanado: data.subsanado,
						inconsistenciaId_nuevo: '',
						causalInterrupcionId_nuevo: data.causalInterrupcionId,
						informe_nuevo: '',
						GrupoInconsistencia: traerTodaData.grupoInconsistencia,
						solicitudTareaId: responseSolicitud?.solicitudTareaId,
						Solicitudid: responseSolicitud?.solicitudId

					}
				}

				if (traerTodaData?.novedad === "INFORME 114 - INCONSISTENCIA") {

					request = {
						novedad: data.novedad,
						listaInconsistenciaId: '',
						causalInterrupcionId: '',
						informe: traerIdAnterior,
						subsanado: data.subsanado,
						inconsistenciaId_nuevo: '',
						causalInterrupcionId_nuevo: '',
						informe_nuevo: data.informe,
						GrupoInconsistencia: traerTodaData.grupoInconsistencia,
					}
				}

				await api.ActualizarInconsistencias(request)
					.then(async (response) => {
						if (response) {
							if (response.isSuccess) {


								setResponder(true)
								verificarEstadosNovedades()
								setMostrarCheck(false)
								toast('Se actualizó correctamente', { type: toast.TYPE.SUCCESS })
								setLoading(false)
							} else {
								toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
								setLoading(false)
								setMostrarCheck(false)
							}
						}
					}).catch(() => {
						toast("Error del sistema", { type: toast.TYPE.ERROR });
						setLoading(false)
						setMostrarCheck(false)
					});
			}
		}

		setListadoInconsistencias(items);
		reset();
	};

	const [refresh, setRefresh] = useState(false);

	const deleteInconsistencias = async (data: any) => {

		setResetNovedad(true)
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Desea eliminar la inconsistencia?',
		);

		if (confirmAlert) {

			setLoading(true)

			const updatedListado = listadoInconsistencias.filter(
				x => x.listaInconsistenciaId !== data.listaInconsistenciaId
			);
			setListadoInconsistencias(updatedListado); // Actualiza el listado

			// Aquí se usa el refresh para forzar el render
			setRefresh(prev => !prev);


			const nuevaInconsistencia: string = data?.listaInconsistenciaId;
			const nuevaCausalInterrupcion: string = data?.causalInterrupcionId;

			setInconsistenciasSeleccionadas((prevInconsistencias) =>
				prevInconsistencias.filter((inconsistencia) => inconsistencia !== nuevaInconsistencia)
			);
			setCausalSeleccionadas((prevCaulsalInterrupcion) =>
				prevCaulsalInterrupcion.filter((causalInterrupcion) => causalInterrupcion !== nuevaCausalInterrupcion)
			);

			// if (data.novedad === "NINGUNA") {
			//     setDeshabilitarNinguna(false);
			// }
			// if (data.novedad === "PAGOS SUCESIVOS") {
			//     setDeshabilitarNinguna(false)
			// }

			if (data?.novedad === "INFORME 114 - INCONSISTENCIA") {
				setDeshabilitarNinguna(false);
			}

			// const newListadoInconsistencias = listadoInconsistencias?.filter(x => x !== data);
			const newListadoInconsistencias = listadoInconsistencias?.filter(x => x.listaInconsistenciaId !== data.listaInconsistenciaId);
			setListadoInconsistencias((prevState) =>
				prevState.filter(x => x.listaInconsistenciaId !== data.listaInconsistenciaId)
			);

			// setListadoInconsistencias(newListadoInconsistencias);


			if (newListadoInconsistencias.length === 0) {
				setCausalYNovedades(false);
			}

			if (responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 2")) {
				setAprobar(true);
				setSubsanado(false);
				setRechazar(false);
				setHablitarActualiza(false);
			} else {
				setAprobar(true);
				setSubsanado(false);
				setRechazar(false);
				setHablitarActualiza(false);

			}

			if (responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 2") || responseSolicitud?.nombreTarea.includes("Seguridad documental grupo 1")) {
				if (listadoInconsistencias.length !== 0) {
					const almenosUna = listadoInconsistencias?.some((x: any) => x.grupoInconsistencia === 2 && (x.novedad !== "NOVEDAD SUBSANABLE" || x.novedad !== "CAUSAL DE INTERRUPCIÓN"))

					if (almenosUna) {
						setCausalYNovedades(false)
						setDeshabilitarNinguna(false)
					}
					const request = {
						SOL_ID_FK: responseSolicitud.solicitudId,
						inC_INCONSISTENCIA: data.novedad,
						tpO_NOV_ID: data.listaInconsistenciaId,
						caU_ID: data.causalInterrupcionId,
						inF_ID: data.informe,
						INC_GRUPO: data.grupoInconsistencia,
						SLT_ID: responseSolicitud?.solicitudTareaId
					}

					await api.EliminarNovedad(request)
						.then(async (response) => {

							if (response) {

								if (response.isSuccess) {

									toast('Se elimino correctamente.', { type: toast.TYPE.SUCCESS });
									verificarEstadosNovedades()
									setMostrarCheck(false)
									setLoading(false)
									setResponder(false)
									setLlamarNovedades(false)
								} else {
									toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
									setLoading(false)
								}
							}
						}).catch(() => {
							toast("Error del sistema", { type: toast.TYPE.ERROR });
							setLoading(false)
						});
				}

			}

			reset();
		}
	};

	const formularioInformacionComunicacion = useForm<ComunicacionSolicitud>({
		defaultValues: {
			idSolicitud: responseSolicitud?.solicitudId,
			idCausalComunicacion: 0,
			idComunicacion: null,
			textoIncluir: '',
			firmaDigital: false,
			radicado: '',
			radicadoRelacionado: '',
			idComunicacion2: null,
			idCausalComunicacion2: 0,
			textoIncluir2: '',
			radicado2: '',
			radicadoRelacionado2: '',
		}
	})

	useEffect(() => {
		setObservaciones(trazabilidad?.find(x => x.idTareaSolicitud === responseSolicitud?.solicitudTareaId)?.observacion);
		formularioObservaciones.setValue('observaciones', trazabilidad?.find(x => x.idTareaSolicitud === responseSolicitud?.solicitudTareaId)?.observacion || '')
	}, [trazabilidad])

	const verificarEstadosNovedades = async () => {

		setLoading(true)
		const request = responseSolicitud?.solicitudId as string;

		await api.GetConsultarInconsistencia(request)
			.then((result) => {
				const grupo2 = result?.data?.some((x: any) => x.grupoInconsistencia === 2);

				if (responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')) {

					const validarInforme114 = result?.data?.some((x: any) => x.novedad === 'INFORME 114 - INCONSISTENCIA');
					const validarCausal = result?.data?.some((x: any) => x.novedad === "CAUSAL DE INTERRUPCIÓN" && x.subsanado === false);
					if (validarInforme114) {

						setHablitarActualiza(false);
						setLoading(false)
						setListadoInconsistencias(result.data);
						setDeshabilitarNinguna(true);
						return
					}

					if (validarCausal) {

						setHablitarActualiza(true);
						setLoading(false)
						setListadoInconsistencias(result.data);
						setDeshabilitarNinguna(true);
						NotificationService.genericAlwarning(
							'CAUSAL DE INTERRUPCIÓN ',
							`La solicitud queda en revisión. \n ¡Por favor regresa a la pantalla de inicio!`,
							'warning'
						)

						return
					}
					if (!validarCausal) {
						setDeshabilitarNinguna(false);
					}
					setLoading(false)
					if (result.data.length > 0) {
						setHablitarActualiza(false);

					} else {
						setHablitarActualiza(true);
					}
				}

				if (result.data.length > 0 && responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 2')) {

					const siMatriculaBloqueada = responseSolicitud?.solicitudMatriculaDto?.some((item: any) => item.bloqueo === true || item.tipo_bloqueo === "1")
					const validarCausal = result?.data?.some((x: any) => x.novedad === "CAUSAL DE INTERRUPCIÓN" && x.subsanado === false);
					if (validarCausal) {

						setHablitarActualiza(true);
						setLoading(false)
						setListadoInconsistencias(result.data);
						setDeshabilitarNinguna(true);
						NotificationService.genericAlwarning(
							'CAUSAL DE INTERRUPCIÓN ',
							`La solicitud queda en revisión. \n ¡Por favor regresa a la pantalla de inicio!`,
							'warning'
						)

						return
					}

					if (siMatriculaBloqueada) {
						setHablitarActualiza(true);
						setLoading(false)
						setListadoInconsistencias(result.data);
						setDeshabilitarNinguna(true);
						setDeshativarBtnEliminarGp2(true)
						NotificationService.genericAlwarning(
							'MATRICULA BLOQUEADA ',
							`La solicitud requiere análisis de trazabilidad y uso de inmueble, en el modulo Matrículas inmobiliarias`,
							'warning'
						)

						return
					}

					setDeshativarBtnEliminarGp2(true)
					setLoading(false)

					const alungoSubsanable = result?.data?.some((x: any) => x.novedad === 'NOVEDAD SUBSANABLE' && x.subsanado === false)
					const algunSubsanadoFalseGrupo1 = result?.data?.some((item: any) =>
						item.grupoInconsistencia === 1 && item.subsanado === false
					);

					const algunSubsanadoFalseGrupo2 = result?.data?.some((item: any) =>
						item.grupoInconsistencia === 2 && item.subsanado === false
					);

					if (alungoSubsanable) {
						setDeshabilitarBTNRechazarGp2(false)
					} else {
						setDeshabilitarBTNRechazarGp2(true)
					}

					if (!algunSubsanadoFalseGrupo1 && !algunSubsanadoFalseGrupo2 && grupo2) {

						setHablitarActualiza(false);
						setLoading(false)
					}
					else {
						setHablitarActualiza(true);
						setLoading(false)
					}
				} else if (!grupo2 && !responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')) {
					setHablitarActualiza(true);
					setLoading(false)
				}
				setListadoInconsistencias(result.data);
			});
	}

	const documentosAgregados = (
		documentos: DocumentosPorSubModalidadModel[],
	): void => {
		setDocumentosAguardar(documentos);
	};

	const handleTabChange = (
		event: React.ChangeEvent<any>,
		newValue: number,
	) => {
		setCurrentTabValue(newValue);
	};

	useEffect(() => {

		if ((responseSolicitud?.nombreTarea === 'Trámite de pago retiro parcial' ||
			responseSolicitud?.nombreTarea === 'Trámite de pago V 14' ||
			responseSolicitud?.nombreTarea === 'Trámite de pago  Giro de recursos' ||
			responseSolicitud?.nombreTarea === 'Trámite de pago  Subsidios secuestrados' ||
			responseSolicitud?.nombreTarea === 'Trámite de pago V 8' ||
			responseSolicitud?.nombreTarea === 'Trámite de pago Desafiliaciones') &&
			currentTabValue === 4 && !tipoAccionDetalleProcesar) {
			setaprobarBloquear(false)
		}

	}, [])


	const integrarAprobar = async () => {
		const aprobarSolicitudTareaDto = {
			solicitudTareaId:
				responseSolicitud?.solicitudTareaId as string,
			clienteId:
				responseSolicitud?.clienteId as number,
		};

		try {
			const response =
				await api.PostAprobarSolicitudTarea(
					aprobarSolicitudTareaDto,
				);

			if (!response?.isSuccess) {
				toast(
					`Error: ${response?.returnMessage ||
					'No fue posible aprobar la solicitud'
					}`,
					{ type: toast.TYPE.ERROR },
				);
				setCargar(false);
				return;
			}

			toast(
				'Se aprobó la solicitud correctamente.',
				{ type: toast.TYPE.SUCCESS },
			);

			obtnerBoleano(true);
			onItemGoBack();
		} catch (error) {
			console.error(
				'Error al aprobar solicitud:',
				error,
			);

			toast(
				'Error al aprobar solicitud',
				{ type: toast.TYPE.ERROR },
			);

			setCargar(false);
		}
	};

	const obtenerCesantiasSeven = async () => {

		setCargar(true)

		const response = {
			solicitudId: responseSolicitud?.solicitudId,
			subModalidadId: responseSolicitud?.tipoSubModalidadId,
			idCliente: responseSolicitud?.clienteId
		}

		await apiCesantias
			.PostPagosCesantiasSeven(response)
			.then(async (response: any) => {

				if (response) {

					if (!response.isSuccess) {

						const mensajeValidar = response?.returnMessage.split('.')

						if (mensajeValidar[2].includes('[Documento no puede ser aplicado, no tiene detalle de movimiento]')) {
							await NotificationService.genericAlert(
								'Error',
								`Para procesar el pago, el estado de la cuenta debe ser vigente/inactivo.`,
								'error',
							);
							setCargar(false)
							setaprobarBloquear(false)
						} else {
							await NotificationService.genericAlert(
								'Error',
								`${response.returnMessage}`,
								'error',
							);

							setCargar(false)
							setaprobarBloquear(false)
						}


					} else if (response.isSuccess && response.data !== null) {

						const parsedResponse = JSON.parse(response.data.crearCDPResponse);
						const insertarFacturaPResponse = JSON.parse(response.data.insertarFacturaPResponse);
						// integrar aprobar
						const aprobarSolicitudTareaDto = {
							solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
							clienteId: responseSolicitud?.clienteId as number
						};

						await api
							.PostAprobarSolicitudTareaDocumentos(aprobarSolicitudTareaDto)
							.then(async (response) => {
								if (response) {
									if (response.isSuccess) {
										toast('Se aprobó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
										obtnerBoleano(true);
										onItemGoBack();
										await obtenerPlanillaPdf();
										await NotificationService.genericAlert(
											'Exitoso', `Se genero plantilla de pago con éxito \n CDP: ${insertarFacturaPResponse.NewDataSet.PO_FACTU.FAC_NUME} \n Factura generada: ${parsedResponse.NewDataSet.PG_MPRES.MPR_CONT}`,
											'success',
										);
									} else {
										toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
									}
								}
							}).catch(() => {
								toast("Error al aprobar solicitud", { type: toast.TYPE.ERROR });
							});

					} else if (response.isSuccess && response.data === null) {
						const insertarFacturaPResponse = JSON.parse(response.data.insertarFacturaPResponse);
						const retornoFact = insertarFacturaPResponse?.NewDataSet?.PO_FACTU?.RETORNO;
						// integrar aprobar
						const aprobarSolicitudTareaDto = {
							solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
							clienteId: responseSolicitud?.clienteId as number
						};

						await api
							.PostAprobarSolicitudTareaDocumentos(aprobarSolicitudTareaDto)
							.then(async (response) => {
								if (response) {
									if (response.isSuccess) {
										toast('Se aprobó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
										obtnerBoleano(true);
										onItemGoBack();
										await obtenerPlanillaPdf();
										await NotificationService.genericAlert(
											'Exitoso', `${response.returnMessage} ${retornoFact}`,
											'success',
										);
									} else {
										toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
									}
								}
							}).catch(() => {
								toast("Error al aprobar solicitud", { type: toast.TYPE.ERROR });
							});
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
				setCargar(false)
				setaprobarBloquear(false)
			});
	}

	const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
	const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
	const catalogApi = CatalogApi();

	const isValidIPv4 = (ip: string) => {
		const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
		return ipv4Regex.test(ip);
	};

	const isValidIPv6 = (ip: string) => {
		const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
		return ipv6Regex.test(ip);
	};

	const enviarIp = async (ip: any) => {
		const convertIp = ip.split('.')[0]

		const hexadecimal = convertIp
		await catalogApi
			.getObtenerIpLocal(hexadecimal)
			.then(async (response: any) => {

				if (response) {
					if (response.isSuccess) {
						setPublicIpAddress(response.data);
					}
				}
			})
			.catch(e => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});

	}

	useEffect(() => {
		const fetchLocalIPAddress = () => {
			const peerConnection = new RTCPeerConnection({ iceServers: [] });
			peerConnection.createDataChannel('');
			peerConnection.createOffer()
				.then((offer) => peerConnection.setLocalDescription(offer))
				.catch((error) => console.error('Error creating offer:', error));

			peerConnection.onicecandidate = (event) => {
				if (event.candidate) {
					const ip: any = event.candidate.address;
					if (isValidIPv4(ip) || isValidIPv6(ip)) {
						setLocalIpAddress(ip);
						enviarIp(ip);
					} else {
						setLocalIpAddress(ip);
						enviarIp(ip);
					}
				}
			};

			return () => {
				peerConnection.close();
			};
		};
		fetchLocalIPAddress();

	}, []);

	const obtenerPlanillaPdf = async () => {
		setCargar(true)
		const request = {
			tipoSubModalidadId: responseSolicitud?.tipoSubModalidadId,
			solicitudTareaId: responseSolicitud?.solicitudTareaId,
			identificacion: responseSolicitud?.identificacionCliente,
			tipoIdentificacion: responseSolicitud?.tipoIdentificacion,
			tipoCuenta: ""
		}

		try {
			const response = await apiReportes.PostPlanillaPdf(request);
			const mimeType = response.type;
			const file = new Blob([response], { type: mimeType });
			const fileURL = URL.createObjectURL(file);
			window.open(fileURL, '_blank');
			setCargar(false)

			const auditoriaApi = AuditoriaApi();
			const user = AuthService.getUserInfo();
			const nav = window.navigator as any;
			const usrAg = nav.userAgentData.brands.find(
				(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
			)?.brand;

			const requestAuditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData.platform,
				nombreUsuario: user.USR_USERNAME,
				usurioId: user.USR_ID,
				ip: publicIpAddress,
				modulo: 'Solicitudes / Tramite de pago',
				hostname: document.location.host,
				tipoAccion: 'DESCARGAS',
				idTipoAccion: 5,
				descripcion: `Descarga planilla de pago con los siguientes filtros:
                - Identificación: ${request.identificacion || 'No especificada'}
                - Id solicitud tarea: ${request.solicitudTareaId || 'No especificada'}
                - Tipo cuenta: ${request.tipoCuenta || 'No especificada'}
				- Tipo Identifiación: ${request.tipoIdentificacion || 'No especificada'}
				- Id tipo submodalidad: ${request.tipoSubModalidadId || 'No especificada'}`,
			} as Auditoria;

			auditoriaApi.InsertAuditoria(requestAuditoria);

		} catch (e: any) {
			await NotificationService.genericAlert(
				'Error',
				`Error al obtener el archivo. Intente nuevamente. ${e.message}`,
				'error',
			);
			setCargar(false)
			setaprobarBloquear(false)
		}

	}

	const actuliazarDatosSolicitud = async (datos: any) => {
		if (responseSolicitud?.nombreTarea.includes('Verificación cuenta individual') && datos) {
			const request = {
				solicitudTareaId: responseSolicitud?.idProcesoSolicitud,
				solicitudId: responseSolicitud?.solicitudId,
				tipoIdentificicion: 1,
				identificacion: '10181111',
				// confirmacionNumeroCuenta: "",
				entidadObj: datos.entidadObj,
				numeroCuenta: datos.numeroCuenta,
				// numeroIdentificacion: "",
				// razonSocial: "",
				// tipoCuenta: "",
				// tipoIdentificacion: "",
				valorPagar: 7700000,
			}

			await NotificationService.genericAlert(
				'Actualizado con éxito',
				'Se actuliazado los datos de la solicitud',
			);
		}
		onSubmitAprobar();
	}


	const onSubmitAprobar = async () => {
		setCargar(true);

		try {
			const nombreTarea = String(
				responseSolicitud?.nombreTarea || '',
			);

			const esTramitePago =
				nombreTarea.includes('Trámite de pago');

			const esVerificacionCuentaIndividual =
				nombreTarea.includes(
					'Verificación cuenta individual',
				);

			const esVerificacionEmbargos =
				nombreTarea.includes(
					'Verificación embargos',
				);

			if (esTramitePago) {
				const tramiteDePago = trazabilidad?.find(
					(item: any) =>
						String(
							item?.descripcionTarea || '',
						).includes('Trámite de pago'),
				);

				const observacionFormulario = String(
					formularioObservaciones.watch(
						'observaciones',
					) || '',
				).trim();

				const observacionTrazabilidad = String(
					tramiteDePago?.observacion || '',
				).trim();

				if (
					!observacionFormulario &&
					!observacionTrazabilidad
				) {
					toast(
						'Por favor agregue una observación',
						{ type: toast.TYPE.ERROR },
					);
					setCargar(false);
					return;
				}
			}

			const confirmAlert =
				await NotificationService.confirmAlert(
					'¿Está seguro que desea continuar?',
				);

			if (!confirmAlert) {
				setCargar(false);
				return;
			}

			if (esTramitePago) {
				await obtenerCesantiasSeven();
				return;
			}

			if (
				esVerificacionCuentaIndividual ||
				esVerificacionEmbargos
			) {
				const informacionObservaciones =
					formularioObservaciones.getValues();

				const observacion = String(
					informacionObservaciones?.observaciones ||
					'',
				).trim();

				if (!observacion) {
					toast(
						'Por favor ingrese alguna observación',
						{ type: toast.TYPE.ERROR },
					);
					setCargar(false);
					return;
				}

				const responseActualizar =
					await api.actualizarObservaciones({
						idSolicitudTarea:
							responseSolicitud?.solicitudTareaId,
						observaciones: observacion,
					});

				if (!responseActualizar?.isSuccess) {
					toast(
						responseActualizar?.returnMessage ||
						'No fue posible actualizar la observación.',
						{ type: toast.TYPE.ERROR },
					);
					setCargar(false);
					return;
				}

				await integrarAprobar();
				return;
			}

			toast(
				`La actividad "${nombreTarea}" no tiene configurado el proceso de aprobación.`,
				{ type: toast.TYPE.ERROR },
			);

			setCargar(false);
		} catch (error) {
			console.error(
				'Error al aprobar la solicitud:',
				error,
			);

			toast(
				'Error del sistema al aprobar la solicitud',
				{ type: toast.TYPE.ERROR },
			);

			setCargar(false);
		}
	};

	const onSubmitSubsanar = async () => {
		try {
			const confirmAlert = await NotificationService.confirmAlert(
				'Subsanacicón',
				'Esta a punto de confirmar la subsanación, desea continuar',
				'warning'
			);
			if (confirmAlert) {
				const informacionObservaciones = formularioObservaciones.getValues();

				api.actualizarObservaciones({
					idSolicitudTarea: responseSolicitud?.solicitudTareaId,
					observaciones: informacionObservaciones?.observaciones
				})
				const solicitudTareaDto = {
					solicitudTareaId: responseSolicitud?.solicitudTareaId as string
				};
				const responseSubsanar = await api.PutSubsanarSolicitudTarea(solicitudTareaDto);
				if (responseSubsanar && responseSubsanar?.isSuccess) {
					toast('Proceso completado correctamente.', { type: toast.TYPE.SUCCESS });
					obtnerBoleano(true);
					onItemGoBack();

				}
			}
		} catch (e) {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		}
	};

	const rechazarSolicitud = () => {
		onSubmitRechazar()
	}

	const onSubmitRechazar = async () => {

		const confirmAlert = await NotificationService.confirmAlert(
			'¿Está seguro que desea continuar?',
		);

		if (confirmAlert) {

			const informacionObservaciones = formularioObservaciones.getValues();
			api.actualizarObservaciones({
				idSolicitudTarea: responseSolicitud?.solicitudTareaId,
				observaciones: informacionObservaciones?.observaciones
			})
			const rechazarSolicitudTareaDto = {
				solicitudTareaId: responseSolicitud?.solicitudTareaId as string
			};
			await api
				.PostRechazarSolicitudTarea(rechazarSolicitudTareaDto)
				.then(async (response) => {
					if (response) {
						if (response.isSuccess) {
							toast('Se rechazó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
							onItemGoBack();
							obtnerBoleano(true)
						} else {
							toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
						}
					}
				}).catch(() => {
					toast("Error del sistema", { type: toast.TYPE.ERROR });
				});
		}
	};

	const onSubmitGenerarPago = () => {
		setOpen(true);
	};
	const handleClose = () => {
		setOpen(false);
	};

	const onSubmit2 = async () => {

		if (responseSolicitud && responseSolicitud?.solicitudTerceroBeneficiarioDto[0]) {
			const { entidadBancaria, numeroCuentaBeneficiario, valor } = formularioVerificacionBeneficiariosPago.getValues();
			if ((entidadBancaria && entidadBancaria.split("_")[0] !== responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.entidadObj?.id) ||
				(numeroCuentaBeneficiario && numeroCuentaBeneficiario !== responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.numeroCuenta) ||
				(valor && UtilService.cleanCurrencyValue(valor) !== Number(responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.valorRetirarDos))) {
				await NotificationService.genericAlert(
					'Alerta',
					`La información del beneficiario no coincide`,
					'warning'
				);
				return;
			}
		}

		const informacionObservaciones = formularioObservaciones.getValues();

		if (informacionObservaciones?.observaciones == null || informacionObservaciones?.observaciones === '') {
			toast('Por favor ingrese alguna observación', { type: toast.TYPE.ERROR });
			return;
		}

		setLoading(true);
		const api = SolicitudApi();
		await api.actualizarObservaciones({
			idSolicitudTarea: responseSolicitud?.solicitudTareaId,
			observaciones: informacionObservaciones?.observaciones
		}).then(response => {
			if (response.isSuccess) {
				setLoading(false);
				setAprobar(false);
				toast('Información actualizada correctamente', { type: toast.TYPE.SUCCESS });
			}
		}).catch(() => {
			setActualizar(false);
			setLoading(false);
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		});

		await obtenerTrazabilidad();
	};

	const [observaciones, setObservaciones] = useState(responseSolicitud?.observacion);

	useEffect(() => {

		let pageTitleProps;

		if (responseSolicitud?.descripcionProceso === "MODELO ANTICIPADO DE SOLUCIÓN DE VIVIENDA") {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Solicitud de Vivienda 8",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		} else if (responseSolicitud?.descripcionProceso === "RETIRO PARCIAL DE CESANTÍAS") {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Solicitud de Cesantías",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		} else if (responseSolicitud?.descripcionProceso === "DESAFILIACIONES") {
			pageTitleProps = {
				topText: "Desafiliaciones",
				title1: "DESAFILIACIÓN CON CESANTÍAS",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === "SOLICITUD DE SUBSIDIO") {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Solicitud de Vivienda 14",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === 'GIRO DE RECURSOS') {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Solicitud de Giro de Recursos",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === 'SUBSIDIO SECUESTRADOS') {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Subsidio Secuestrados",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionSubModalidad?.includes('AVAC')) {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "DEVOLUCIÓN DE AHORROS AVAC",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionSubModalidad?.includes('AVAV')) {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "DEVOLUCIÓN DE AHORROS AVAV",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === 'DEVOLUCIÓN DE SALDOS') {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "DEVOLUCIÓN DE SALDOS",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
	}, [responseSolicitud])

	const formularioObservaciones = useForm({
		defaultValues: {
			idSolicitud: responseSolicitud?.solicitudTareaId,
			observaciones: '',
			observacionesGrupo1: '',
		}
	})

	const actualizarObservacionCuentaIndividual = async (data: any) => {
		const observacion = String(data?.observaciones ?? '').trim();

		if (!observacion) {
			toast('Por favor ingrese alguna observación', {
				type: toast.TYPE.ERROR,
			});

			throw new Error('La observación es requerida.');
		}

		setLoading(true);

		try {
			const response = await api.actualizarObservaciones({
				idSolicitudTarea: responseSolicitud?.solicitudTareaId,
				observaciones: observacion,
			});

			if (!response?.isSuccess) {
				toast(
					response?.returnMessage ||
					'No fue posible actualizar la observación.',
					{ type: toast.TYPE.ERROR }
				);

				throw new Error(
					response?.returnMessage ||
					'No fue posible actualizar la observación.'
				);
			}

			setAprobar(false);

			toast('Información actualizada correctamente.', {
				type: toast.TYPE.SUCCESS,
			});

			await obtenerTrazabilidad();
		} finally {
			setLoading(false);
		}
	};

	const esActualizacionCuentaIndividual =
		currentTabValue === TABS_INDEX.VERIFICACIONESDECUENTA &&
		responseSolicitud?.nombreTarea?.includes(
			'Verificación cuenta individual'
		);

	const onSubmitActualizar = esActualizacionCuentaIndividual
		? formularioObservaciones.handleSubmit(
			actualizarObservacionCuentaIndividual
		)
		: formularioVerificacionBeneficiariosPago.handleSubmit(onSubmit2);

	if (cargar) {
		return <CHLoadingProcess />

	}



	const esVerificacionCuentaIndividual =
		currentTabValue === TABS_INDEX.VERIFICACIONESDECUENTA &&
		responseSolicitud?.nombreTarea?.includes(
			'Verificación cuenta individual'
		);

	const handleSubmitBotonera = esVerificacionCuentaIndividual
		? formularioObservaciones.handleSubmit
		: formularioVerificacionBeneficiariosPago.handleSubmit;

	const onSubmitBotonera = esVerificacionCuentaIndividual
		? actualizarObservacionCuentaIndividual
		: onSubmit2;

	return (
		<>
			<CHPageTitleV2 {...titulosIcon} />
			<ViewTabs
				currentTab={currentTabValue}
				onTabChange={handleTabChange}
				responseSolicitud={responseSolicitud}
				userAsig={userAsig}
				grupo={grupo}
				userIngreso={userIngreso}
			/>
			<Panel
				responseSolicitud={responseSolicitud}
				currentTabValue={currentTabValue}
				documentosAgregados={documentosAgregados}
				formulariosSolicitud={formulariosSolicitud}
				observaciones={observaciones as string}
				control={control}
				setValue={setValue}
				getValues={getValues}
				setEstadoSubsanar={setEstadoSubsanar}
				grupo1={false}
				handleSubmit={handleSubmit}
				reset={reset}
				watch={watch}
				listadoInconsistencias={listadoInconsistencias}
				addInconsistencia={addInconsistencia}
				updateInconsistencias={updateInconsistencias}
				deleteInconsistencias={deleteInconsistencias}
				inconsistenciasSeleccionadas={inconsistenciasSeleccionadas}
				deshablitarNinguna={deshablitarNinguna}
				setDeshabilitarNinguna={setDeshabilitarNinguna}
				setCausalYNovedades={setCausalYNovedades}
				causalYNovedades={causalYNovedades}
				siEsAprobadaSolcitud={siEsAprobadaSolcitud}
				quitarTiposNovedades={quitarTiposNovedades}
				setQuitarTiposNovedades={setQuitarTiposNovedades}
				setTraerIdAntrior={setTraerIdAntrior}
				caulsaSeleccionadas={caulsaSeleccionadas}
				setResetNovedad={setResetNovedad}
				resetNovedad={resetNovedad}
				setTraerTodaData={setTraerTodaData}
				estadoSubsanar={estadoSubsanar}
				mostrarCheck={mostrarCheck}
				setLlamarNovedades={setLlamarNovedades}
				llamarNovedades={llamarNovedades}
				setMostrarCheck={setMostrarCheck}
				deshativarBtnEliminarGp2={deshativarBtnEliminarGp2}
				formularioObservaciones={formularioObservaciones}
				trazabilidad={trazabilidad}
				refresh={refresh}
				formularioInformacionComunicacion={formularioInformacionComunicacion}
				formularioVerificacionBeneficiariosPago={formularioVerificacionBeneficiariosPago}
				obtenerPlanillaPdf={obtenerPlanillaPdf}
			/>
			{
				(responseSolicitud?.nombreTarea?.includes('Trámite de pago')) &&
					currentTabValue === 4 &&
					tipoAccionDetalleProcesar ?
					<DescargaSevenPlanilla obtenerPlanillaPdf={obtenerPlanillaPdf} /> : null
			}
			{

				(responseSolicitud?.nombreTarea.includes("Verificación cuenta individual")) &&
					currentTabValue === 4 ?
					<ModalCuentasAfectadas
						responseSolicitud={responseSolicitud}
					/> : null
			}
			<BotoneraInformacionSolicitudComponent
				onItemGoBack={onItemGoBack}
				tipoAccion={tipoAccionNumber}
				onSubmitAprobar={onSubmitAprobar}
				onSubmitSubsanar={onSubmitSubsanar}
				onSubmitRechazar={onSubmitRechazar}
				onSubmitGenerarPago={onSubmitGenerarPago}
				// onSubmit={formularioVerificacionBeneficiariosPago.handleSubmit(onSubmit2)}
				onSubmit={onSubmitBotonera}
				handleSubmit={handleSubmitBotonera}
				disabledActualizar={actualizar}
				disabledAprobar={aprobar}
				disabledSubsanar={subsanado}
				disabledRechazar={false}
				disabledRegresar={false}
				tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
				responseSolicitud={responseSolicitud}
				currentTabValue={currentTabValue}
				aprobarBloquear={aprobarBloquear}
				hablitarActualiza={hablitarActualiza}
				deshabilitarBTNRechazarGp2={undefined}
				deshalitarBotonesInconsistencia={false}
				userAsig={userAsig}
				grupo={grupo}
				userIngreso={userIngreso}
			/>

			{
				!responseSolicitud?.nombreTarea.includes("Verificación cuenta individual") ? '' :
					<ModalPago
						open={open}
						onClose={handleClose}
						responseSolicitud={responseSolicitud}
						// onSubmitAprobar={onSubmitAprobar}
						setCargar={setCargar}
						actuliazarDatosSolicitud={actuliazarDatosSolicitud}
						rechazarSolicitud={rechazarSolicitud}
					/>

			}

		</>
	);
}
const mapStateToProps = (state: AppState) => ({
	userState: state.users,
	catalogState: state.catalog
});

export default connect(mapStateToProps)(ProcesoVerificacionEmbargo);