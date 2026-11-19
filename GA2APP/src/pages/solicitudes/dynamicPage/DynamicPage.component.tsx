import React, { useEffect, useMemo, useState, useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { FormProvider, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AppState } from "../../../store/rootReducer";
import ApiRealState from "../../../services/api-realstateregistrations.service"
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import DatosPersonales from '../components/datos-personales/datosPersonales.component';
import MatriculaInmobiliaria from '../components/matricula-inmobiliaria/matriculaInmobiliaria.component';
import PropietarioMatricula from '../components/propietario-matricula/propietarioMatricula.component';
import TerceroApoderado from '../components/terceros/tercero-apoderado.component';
import TerceroBeneficiarioPagosVu from '../components/terceros/Tercero-beneficiario-pagos-vu';
import InformacionInmueble from '../components/informacion-inmueble/informacionInmueble.component';
import EntidadEducativa from '../components/entidad-educativa/entidadEducativa.component';
import InformacionDeTurno from '../components/informacion-de-turno/informacionDeTurno.component';
import InformacionDeAdjudicacion from '../components/informacion-de-adjudicacion/informacionDeAdjudicacion.component';
import InformacionDeEscrituras from '../components/informacion-de-escrituras/informacionDeEscritura.component';
import TerceroEntidadSeguroEducativo from '../components/terceros/tercero-entidad-seguro-educativo';
import TerceroBeneficiarioEstudio from '../components/terceros/tercero-beneficiario-estudio.component';
import TerceroTenedorAcciones from '../components/terceros/tercero-tenedor-acciones.component';
import TerceroVendedor from '../components/terceros/tercero-vendedor.component';
import ObservacionesSolicitud from '../components/observaciones/observaciones';
import BotoneraSolicitud from '../components/botonera/botonera-solicitud.component';
import DatosPersonalesValor from '../components/datos-personales-dos/Datos-personales-valor';
import TerceroBeneficiarioDesafiliaciones from '../components/terceros/tercero-beneficiario-desafiliaciones';
import DatosPersonalesDesafiliacionCesantias from '../components/datos-personales-dos/datos-personales-desafiliacion-cesantias';
import DatosPersonalesValorCuotaInicial from '../components/datos-personales-tres/datosPersonales-ValorCuotaInicial.component';
import InformacionProcesoSolicitud from '../informacionProcesoSolicitud/informacionProcesoSolicitud';
import InformacionSolicitud from '../informacionSolicitud/InformacionSolicitud';
import IdentificacionPostulantesNucleoFamiliar from '../identificacionPostulantesNucleoFamiliar/IdentificacionPostulantesNucleoFamiliar';
import ManifestacionesAutorizaciones from '../manifestacionesAutorizaciones/ManifestacionesAutorizaciones';
import CiudadPreferenciaParaRecibirVivienda from '../ciudadPreferenciaParaRecibirVivienda/CiudadPreferenciaParaRecibirVivienda';
import FirmaSolicitud from '../firmaSolicitud/FirmaSolicitud';
import { ModalResumenPago } from '../resumenPagoCuotaInicial/ModalResumenPago';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import { MatriculaInmobiliariaModel } from '../../../models/matricula-inmobiliaria.model';
import { ConyugueModel } from '../../../models/conyuge-solicitudes.model';
import { PropietarioMatriculaModel } from '../../../models/propietario-matricula.model';
import { InformacionTurnosModel } from '../../../models/Informacion-turno.model';
import { TerceroApoderadoModel } from '../../../models/tercero-apoderado.model';
import { TerceroBeneficiarioModel, BeneficiarioModel } from '../../../models/tercero-beneficiario.model';
import { TerceroBeneficiarioEstudioModel } from '../../../models/tercero-beneficiario-estudio.model';
import { TerceroVendedorModel } from '../../../models/tercero-vendedor.model';
import { FormulariosSolicitudDto, SolicitudMisCuentasDto, SolicitudTareaModel } from '../../../models/requests/solicitud-tarea.model';
import { TerceroEntidadEducativaModel } from '../../../models/tercero-entidad-educativa.model';
import { TerceroEntidadSeguroEducativaModel } from '../../../models/tercero-entidad-seguro-educativa.model';
import { TerceroTenedorAccionesModel } from '../../../models/tercero-tenedor-acciones.model';
import { ParametrosDesembolsoSevenModel } from '../../../models/credito-desembolso-seven';
import { InformacionAdjudicacionModel } from "../../../models/Informacion-adjudicacion.model";
import { InformacionEscrituracionModel } from "../../../models/Informacion-escrituracion.model";
import {
	startGetAllDepartments,
	startGetAllCities,
	startGetAllTypeDocuments,
	startGetRegistrationOffice,
	startGetAllEntidadBancaria,
	startGetAllCitiesVendors,
	startGetAllEntidadSeguroEducativo,
	startGetAllCatalogoParentesco,
	startGetAllCatalogoPorId,
	startGetAllTypesForces,
	startGetCategory,
	startGetAllCivilState,
	startGetPhoneTypes,
	startGetAllGrades,
	startGetAllTipoCuenta,
	startGetAllEntidadEducativa,
} from '../../../actions/catalog.action';
import { startObtenerValorDesembolsoCredito } from '../../../actions/credit.actions';
import { ENTIDADEDUCATIVA, ENTIDADSEGUROEDUCATIVO, OFICINASREGISTRODTO, VALORCATALOGO } from '../../../types/catalog.type';
import { SolicitudTitleType } from '../../../types/types';
import { BOTONERA_SOLICITUD, FORMS_NAMES, GUID_DEFAULT, ID_SUBMODALIDAD_SEGUNDO_PAGO, MODALIDAD_SOLICITUD } from '../../../constants/constants';
import NotificationService from '../../../services/notification.service';
import SolicitudApi from '../../../services/api-solicitud.service';
import CreditApi from '../../../services/api-credit.service';
import AuthService from '../../../services/auth.service';
import UtilService from '../../../utils/util.service';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import ObservacionesInicioSolicitud from '../components/observaciones/observaciones-inicio-solicitud.component';
import TerceroBeneficiarioPago from '../components/terceros/tercero-beneficiario-pago.component';
import CatalogApi from '../../../services/api-catalog.service';
import ParametrizationApi from '../../../services/api-parametrization.service';
import TerceroBeneficiarioPagoDesafilaciones from '../components/terceros/TerceroBeneficiarioPagoDesafilacion';
import CartaInconsistencia from '../components/CartaInconsistencia';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import InformacionAcreditacion from '../components/InformacionAcreditacion/InformacionAcreditacion';
import ProcesoAcreditacion from '../components/procesoAcreditacion/procesoAcreditacion';
import CargueDocumentosSolicitud from '../components/cargue-documentos-solicitud/cargue-documentos-solicitud.component';
import { DocumentosPorSubModalidadModel } from '../../../models/documentos-por-submodalidad.model';
import { ConsultarSolicitudModel } from '../../../models/requests/consultar-solicitud.model';
import DeclaracionesAutorizaciones from '../components/declaraciones-autorizaciones/declaraciones-autorizaciones';
import ModalCausalAnularSolicitud from '../components/modal-causal-anular/modal-causal-anular';
import { useIp } from '../../../shared/context/IpContext';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import DestinacionDineros from '../components/destinacion-dineros/destinacion-dineros';
import MisCuentas from '../components/informacion-cuentas/informacion-cuentas';
import history from '../../../utils/history.router';
import ApiEntidadesEducativas from '../../../services/api-entidadesEducativas.service';
import Beneficiarios from '../components/terceros/beneficiario-component';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../../actions/client.action';

const DynamicPage = ({
	dynamicTitle,
	formsList,
	responseSolicitud,
	tipoAccion,
	tipoAccionDetalleProcesar,
	modalidadSolicitud,
	withButtons = true,
	onItemGoBack,
	onItemBack,
	obtnerBoleano,
	inicioSolicitud,
	siEsAprobadaSolcitud,
	tipoSolicitud,
	documentosAgregados,
	forzarBotonera,
	formularioObservaciones,
	nivelAfiliado
}: {
	dynamicTitle: SolicitudTitleType,
	formsList: string[],
	responseSolicitud: ResponseSeveranceModel | undefined,
	tipoAccion: number,
	tipoAccionDetalleProcesar: boolean,
	modalidadSolicitud: string,
	withButtons?: boolean,
	onItemGoBack: () => void,
	onItemBack: () => void
	obtnerBoleano: any,
	inicioSolicitud: boolean;
	siEsAprobadaSolcitud: boolean;
	tipoSolicitud: any;
	documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
	forzarBotonera?: boolean;
	formularioObservaciones?: any;
	nivelAfiliado: any;
}) => {

	const apiCatalogo = CatalogApi();
	const api = SolicitudApi();
	const apiEntidadesEducativas =
		ApiEntidadesEducativas();
	const apiCredit = CreditApi();
	const apiParametrization = ParametrizationApi();
	const apiRealState = ApiRealState();

	const dispatch = useDispatch();
	const catalogState = useSelector((state: AppState) => state.catalog);
	const creditState = useSelector((state: AppState) => state.credit);
	const loginClaims = useSelector((state: AppState) => state.login.claims);
	const clientStateData = useSelector((state: AppState) => state.client.client);

	const { topText, title, icon } = dynamicTitle;

	const [tipoAccionNumber, setTipoAccion] = useState<number>(BOTONERA_SOLICITUD.GUARDAR);
	const [matriculaInmobiliariaModel, setMatriculaInmobiliariaModel] = useState<MatriculaInmobiliariaModel[]>([]);
	const [propietarioMatriculaModel, setPropietarioMatriculaModel] = useState<PropietarioMatriculaModel[]>([]);
	const [terceroApoderadoModel, setTerceroApoderadoModel] = useState<TerceroApoderadoModel[]>([]);
	const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioModel[]>([]);
	const [terceroBeneficiarioEstudioModel, setTerceroBeneficiarioEstudioModel] = useState<any[]>([]);
	const [terceroVendedorModel, setTerceroVendedorModel] = useState<TerceroVendedorModel[]>([]);
	const [beneficiarioAfiliado, setBeneficiarioAfiliado] = useState<BeneficiarioModel[]>([]);
	const [beneficiarios, setBeneficiarios] = useState<BeneficiarioModel[]>([]);
	const [tercerosBeneficiarios, setTercerosBeneficiarios] = useState<any[]>([]);
	const [entidadesFinancieras, setEntidadesFinancieras] = useState<BeneficiarioModel[]>([]);
	const [oficinaRegistro, setOficinaRegistro] = useState<OFICINASREGISTRODTO | null>(null);
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [openModal, setOpenModal] = useState<boolean>(false);
	const [validarRetiro, setValidarRetiro] = useState<number>(0);
	const [validarBeneficiario, setValidarBeneficiario] = useState<any>([]);
	const [estadoEntidadNueva, setEstadoEntidadNueva] = useState<any[]>([]);
	const [entidadAhorroProgramadoSeguroEducativo, setEntidadAhorroProgramadoSeguroEducativo] = useState<any[]>([])
	const [oficinasData, setOficinasData] = useState<any[]>([]);
	const [maticulaUno, setMatriculaUno] = useState<boolean>(false);
	const [datosBeneficiarios, setDatosBeneficiarios] = useState(false);
	const [valorReciboTotal, setValorReciboTotal] = useState(0);
	const [mostrarMensaje, setMostrarMensaje] = useState(false);
	const [toastId, setToastId] = useState<React.ReactText | null | undefined>(null);
	const [campoSoloLecturaRadicacionAprobada, setCampoSoloLecturaRadicacionAprobada] = useState<any>(false);
	const [trazabilidad, setTrazabilidad] = useState<any[]>([]);
	const [beneficiariosCompartidos, setBeneficiariosCompartidos] = React.useState<any[]>([]);
	const [apoderadosAutorizados, setApoderadosAutorizados] = useState([]);
	const [formulariosSolicitud, setFormulariosSolicitud] = useState<any[]>([]);
	const [aceptarCheckValue, setAceptarCheckValue] = useState<boolean>(false);
	const [openModalAnular, setOpenModalAnular] = useState<boolean>(false);
	const [destinacionDinerosSeleccionado, setDestinacionDinerosSeleccionado] = useState<string>('');
	const [saldoCuenta, setSaldoCuenta] = useState<number | null>(null);
	const [nombreCuenta, setNombreCuenta] = useState('');
	const [aceptaTraslado, setAceptaTraslado] = useState(false);
	const [nivelRiesgoTusDatos, setNivelRiesgoTusDatos] = useState<number>(0);
	const [loadingAprobacion, setLoadingAprobacion] = useState(false);
	const [propietariosMatriculaValidacion, setPropietariosMatriculaValidacion] = useState<any[]>([]);
	const [matriculasPendientesEliminar, setMatriculasPendientesEliminar] = useState<any[]>([]);
	const [limpiezaPostAnulacionPendiente, setLimpiezaPostAnulacionPendiente] = useState(false);
	const solicitudAnuladaIdRef = useRef<string | null>(null);
	const accionFormularioRef = useRef<'APROBAR' | 'GUARDAR' | 'ANULAR' | null>(null);
	const [
		idsEntidadesCreadasFormulario,
		setIdsEntidadesCreadasFormulario,
	] = useState<string[]>([]);

	const user = AuthService.getUserInfo();
	const CODIGO_CONTROL_PREVENTIVO = 'CP';

	const OBSERVACION_CONTROL_PREVENTIVO =
		'CONTROL PREVENTIVO SOLICITUDES';


	const TABS_INDEX = {
		RESUMENSOLICITUD: 'resumen',
		CARGUEDOCUMENTOACREDITACION: 'carguededocumentosacreditacion',
	};

	const shouldShowTabs = useMemo(() => {
		if (!responseSolicitud?.nombreTarea) return false;

		return formsList?.some(f =>
			(f === FORMS_NAMES.DATOS_PERSONALES || f === FORMS_NAMES.INFORMACION_ACREDITACION) &&
			responseSolicitud.nombreTarea.includes("Verificación en tiempo")
		);
	}, [formsList, responseSolicitud]);

	const [activeTab, setActiveTab] = useState(TABS_INDEX.RESUMENSOLICITUD);

	const [mostrarBotonera, setMostrarBotonera] = useState(forzarBotonera || false);

	const normalizarTexto = (value: any): string =>
		String(value ?? '')
			.trim()
			.toUpperCase()
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '');

	const nombreTareaNormalizado = String(
		responseSolicitud?.nombreTarea || ''
	)
		.toLowerCase()
		.normalize('NFD')
		.replace(/[\u0300-\u036f]/g, '');

	const esTareaDeclaraciones =
		nombreTareaNormalizado.includes('v 8') ||
		nombreTareaNormalizado.includes('v 14') ||
		nombreTareaNormalizado.includes(
			'radicacion subsidio secuestrados'
		);

	const esRadicacionSubsidioSecuestrados =
		nombreTareaNormalizado.includes(
			'radicacion subsidio secuestrados'
		);

	const subModalidadNormalizada = normalizarTexto(
		responseSolicitud?.descripcionSubModalidad
	);

	const esSubModalidadPagoSubsidioSecuestrados =
		subModalidadNormalizada.includes('PRIMER PAGO') ||
		subModalidadNormalizada.includes('SEGUNDO PAGO') ||
		subModalidadNormalizada.includes('SIMULTANEO') ||
		subModalidadNormalizada.includes('(SP)') ||
		subModalidadNormalizada.includes('(SIMULTANEO)');

	const mostrarInformacionSubsidioSecuestrados =
		esRadicacionSubsidioSecuestrados &&
		esSubModalidadPagoSubsidioSecuestrados;



	const esRadicacionDevueltaPorRechazo =
		responseSolicitud?.soL_RECHAZADA === '1' &&
		nombreTareaNormalizado.includes('radicacion');

	const esTareaRadicacion =
		nombreTareaNormalizado.includes('radicacion');

	const detalleSolicitudSoloVisual =
		!esTareaRadicacion;


	const solicitudAnuladaStorage = sessionStorage.getItem('solicitudAnuladaParaLimpiar');

	const esMismaSolicitudAnulada =
		Boolean(responseSolicitud?.solicitudId) &&
		(
			solicitudAnuladaIdRef.current === responseSolicitud?.solicitudId ||
			solicitudAnuladaStorage === responseSolicitud?.solicitudId
		);

	const solicitudTieneEstadoAnuladoEnTrazabilidad =
		Array.isArray(trazabilidad) &&
		trazabilidad.some((item: any) =>
			normalizarTexto(item?.descripcionEstadoSolicitud).includes('anulad')
		);

	const esRadicacionSoloVisualPorAnulacion =
		nombreTareaNormalizado.includes('radicacion') &&
		(
			esMismaSolicitudAnulada ||
			solicitudTieneEstadoAnuladoEnTrazabilidad
		);

	const solicitudSoloLectura =
		esRadicacionDevueltaPorRechazo
			? false
			: detalleSolicitudSoloVisual ||
			siEsAprobadaSolcitud ||
			campoSoloLecturaRadicacionAprobada;

	const tieneFormularioPropietarioMatricula = formsList?.some(
		(formulario: string) => formulario === 'PropietarioMatricula',
	);


	useEffect(() => {
		if (!responseSolicitud?.solicitudId) return;

		if (esRadicacionDevueltaPorRechazo) {

			setLoading(false);
			setIsLoading(false);

			loadDataConyuge(responseSolicitud?.conyugeActivo, responseSolicitud);

			validarTrazabilidadSinLoading();

			return;
		}

		setLoading(true);

		loadDataConyuge(responseSolicitud?.conyugeActivo, responseSolicitud);
		validarTrazabilidad();

	}, [responseSolicitud, esRadicacionDevueltaPorRechazo]);

	useEffect(() => {
		let a = '';

		api.GetFormulariosSolicitud(modalidadSolicitud)
			.then((response: any) => {
				if (!response?.isSuccess) {
					return;
				}

				const formularios = Array.isArray(
					response.data?.formularios
				)
					? [...response.data.formularios]
					: [];

				const esRadicacionSubsidioSecuestrados =
					nombreTareaNormalizado.includes(
						'radicacion subsidio secuestrados'
					);

				if (
					esRadicacionSubsidioSecuestrados &&
					!formularios.includes(
						FORMS_NAMES.DECLARACIONES_AUTORIZACIONES
					)
				) {
					formularios.push(
						FORMS_NAMES.DECLARACIONES_AUTORIZACIONES
					);
				}


				setFormulariosSolicitud(formularios);

				[, , a] = formularios;

				if (a === 'InformacionInmueble') {
					setMatriculaUno(true);
				} else {
					setMatriculaUno(false);
				}
			})
			.catch(() => {
				toast('Error al consultar los formularios', {
					type: toast.TYPE.ERROR,
				});
			});

		apiCatalogo
			.getRegistrationOffice()
			.then((response: any) => {
				if (response?.isSuccess) {
					setOficinasData(response.data);
				}
			})
			.catch(() => {
				toast('Error del sistema', {
					type: toast.TYPE.ERROR,
				});
			});
	}, [
		modalidadSolicitud,
		nombreTareaNormalizado,
	]);

	useEffect(() => {
		loadCatalogInfo();
	}, []);

	// useEffect(() => {
	// 	loadDataTurno(responseSolicitud);
	// }, []);
	useEffect(() => {
		const turno = responseSolicitud?.informacionTurnoDto;

		if (!turno) {
			return;
		}

		loadDataTurno(responseSolicitud);
	}, [
		responseSolicitud?.solicitudId,
		responseSolicitud?.solicitudTareaId,
		responseSolicitud?.informacionTurnoDto,
	]);

	useEffect(() => {

		const beneficiariosBackend = Array.isArray(responseSolicitud?.solicitudTerceroBeneficiarioDto)
			? responseSolicitud?.solicitudTerceroBeneficiarioDto
			: [];

		setTerceroBeneficiarioModel(beneficiariosBackend as TerceroBeneficiarioModel[]);

		const apoderadosBackend = Array.isArray(
			responseSolicitud?.solicitudTerceroApoderadoDto,
		)
			? responseSolicitud?.solicitudTerceroApoderadoDto
			: [];

		setTerceroApoderadoModel(
			apoderadosBackend as TerceroApoderadoModel[],
		);

		const beneficiariosEstudioBackend = Array.isArray(responseSolicitud?.solicitudTerceroBeneficiarioEstudioDto)
			? responseSolicitud?.solicitudTerceroBeneficiarioEstudioDto
			: [];

		setTerceroBeneficiarioEstudioModel(beneficiariosEstudioBackend as any[]);

		const matriculasSolicitud = responseSolicitud?.solicitudMatriculaDto;

		const matriculasBackend: MatriculaInmobiliariaModel[] = Array.isArray(matriculasSolicitud)
			? matriculasSolicitud as MatriculaInmobiliariaModel[]
			: [];

		setMatriculaInmobiliariaModel(matriculasBackend);


		if (responseSolicitud?.valorRetirar !== null) {
			const valorRetirar = responseSolicitud?.valorRetirar ? responseSolicitud?.valorRetirar : '';
			formulario.setValue('valorSolicitud', valorRetirar.toString() === '0' || valorRetirar.toString() === '' ? '' : valorRetirar);
			formulario.setValue('numero_Folio', responseSolicitud?.numero_Folio === 0 ? '' : String(responseSolicitud?.numero_Folio))
			if (responseSolicitud?.fechaFirmaEscritura !== undefined) {
				formulario.setValue('fecha_Firma_Escritura', responseSolicitud?.fechaFirmaEscritura)
			}

		}

		const esRadicacionDevueltaPorRechazo =
			responseSolicitud?.soL_RECHAZADA === '1' &&
			nombreTareaNormalizado.includes('radicacion');

		if (esRadicacionDevueltaPorRechazo) {
			setCampoSoloLecturaRadicacionAprobada(false);
			setTipoAccion(BOTONERA_SOLICITUD.ACTUALIZAR);
		}

		const esTareaPosteriorARechazo =
			responseSolicitud?.soL_RECHAZADA === '1' &&
			!nombreTareaNormalizado.includes('radicacion') &&
			!nombreTareaNormalizado.includes('seguridad documental grupo 1') &&
			!nombreTareaNormalizado.includes('seguridad documental grupo 2') &&
			!nombreTareaNormalizado.includes('verificacion');

		if (esRadicacionDevueltaPorRechazo || esTareaPosteriorARechazo) {
			formulario.setValue('observaciones', '');
			formulario.setValue('observacion', '');
		} else {
			const observacionBackend = responseSolicitud?.observacion || '';

			formulario.setValue('observaciones', observacionBackend);
			formulario.setValue('observacion', observacionBackend);
		}



		// setTipoAccion(tipoAccion);
		if (esRadicacionDevueltaPorRechazo) {
			setTipoAccion(BOTONERA_SOLICITUD.ACTUALIZAR);
		} else {
			setTipoAccion(tipoAccion);
		}

		if (modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CNVGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVUGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVNSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVUSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.READACPROES ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.AHOPROEDU ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOPREDIAL ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DESCONCESANT ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOCRHIP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CCSLSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.RETINST) {
			loadDataViviendaNuevaUsada(responseSolicitud);
		} else if (modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CSLGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ADQUITER ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CCSLSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.REPARVIV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ARMVGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.LIBERGRAV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.APTGHGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CESOCLEAS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ACDDHSS
		) {
			// loadDataConstReparacDSVion(responseSolicitud);
			loadDataViviendaNuevaUsada(responseSolicitud);
		} else if (modalidadSolicitud === MODALIDAD_SOLICITUD.REPAES ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DESSOLAPORT) {
			loadDataViviendaNuevaUsada(responseSolicitud);
			formulario.setValue('fechaLimitePago', selectedMinDate());
		}
		else if (modalidadSolicitud === MODALIDAD_SOLICITUD.PCUOTAINI) {
			loadDataValorDesembolsoCredito(responseSolicitud);
		}else if (modalidadSolicitud === MODALIDAD_SOLICITUD.AVAC ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.AVAV) {
			registrarAfiliado(responseSolicitud);
		}else if (modalidadSolicitud === MODALIDAD_SOLICITUD.DSV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DAVAV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.RMI ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DAVAC) {
			loadDataDevolucionSaldo(responseSolicitud);
			registrarAfiliado(responseSolicitud);
		}

	}, [responseSolicitud]);

	useEffect(() => {
		let timer: number | undefined;

		if (responseSolicitud?.solicitudId) {
			const solicitudAnuladaStorage = sessionStorage.getItem('solicitudAnuladaParaLimpiar');

			const esMismaSolicitudAnulada =
				solicitudAnuladaIdRef.current === responseSolicitud?.solicitudId ||
				solicitudAnuladaStorage === responseSolicitud?.solicitudId;

			const esTareaRadicacion =
				String(responseSolicitud?.nombreTarea || '')
					.toLowerCase()
					.normalize('NFD')
					.replace(/[\u0300-\u036f]/g, '')
					.includes('radicacion');

			if (esMismaSolicitudAnulada && esTareaRadicacion) {
				timer = window.setTimeout(() => {
					setCampoSoloLecturaRadicacionAprobada(false);

					solicitudAnuladaIdRef.current = null;
					setLimpiezaPostAnulacionPendiente(false);

					sessionStorage.removeItem('solicitudAnuladaParaLimpiar');
				}, 200);
			}
		}

		return () => {
			if (timer !== undefined) {
				window.clearTimeout(timer);
			}
		};
	}, [
		responseSolicitud,
		formulariosSolicitud.length,
		beneficiarioAfiliado.length,
		beneficiarios.length,
		limpiezaPostAnulacionPendiente,
	]);

	useEffect(() => {
		const solicitudAnuladaStorage = sessionStorage.getItem(
			'solicitudAnuladaParaLimpiar',
		);

		const solicitudActualId = responseSolicitud?.solicitudId;

		if (
			solicitudAnuladaStorage &&
			solicitudAnuladaStorage !== solicitudActualId
		) {
			sessionStorage.removeItem('solicitudAnuladaParaLimpiar');

			solicitudAnuladaIdRef.current = null;
			setLimpiezaPostAnulacionPendiente(false);
			setCampoSoloLecturaRadicacionAprobada(false);
		}
	}, [responseSolicitud?.solicitudId]);

	useEffect(() => {
		listaBeneficiarios();
	}, [propietarioMatriculaModel, terceroApoderadoModel, beneficiarioAfiliado, entidadesFinancieras, terceroBeneficiarioModel]);

	useEffect(() => {

		if (modalidadSolicitud === 'PAGOPREDIAL') {
			let idOficina = 0;
			if (formulario.getValues('codigoOficina') === oficinaRegistro?.codigoOficina) {
				idOficina = Number(oficinaRegistro?.idOficina);
			}

			const matricula = {
				ciudadId: Number(formulario.getValues("ciudad")),
				direccion: formulario.getValues("direccion"),
				numeroMatricula: `${formulario.getValues('codigoOficina')}-${formulario.getValues('numeroMatricula')}`,
				esPrincipal: true,
				ciudad: { id: Number(formulario.getValues('ciudad')), descripcion: "" },
				departamento: { id: Number(formulario.getValues('departamento')), descripcion: "" },
				departamentoId: Number(formulario.getValues('departamento')),
				oficinaCodigo: `${formulario.getValues('codigoOficina')}`,
				oficinaRegistroId: (idOficina)
			} as MatriculaInmobiliariaModel;
			setMatriculaInmobiliariaModel([matricula])
		}

	}, []);

	useEffect(() => {
		if (mostrarMensaje) {
			localStorage.setItem('toastMessage', `${toastId}`);
		} else {
			toast.dismiss(toastId || undefined);
		}
	}, [mostrarMensaje]);


	const matriculasAdicionadas = (
		matriculas: MatriculaInmobiliariaModel[],
	) => {
		setMatriculaInmobiliariaModel([...matriculas]);
	};

	const validarTrazabilidadSinLoading = async () => {
		try {

			const response = await api.obtenerTrazabilidadSolicitud(
				responseSolicitud?.solicitudId
			);

			setTrazabilidad(response?.data || []);

			const radicacionAprobada = response?.data?.find(
				(item: any) =>
					(
						String(item?.descripcionTarea).includes('Radicación') ||
						String(item?.descripcionTarea).includes('Radicacion')
					) &&
					item?.descripcionEstadoSolicitud === 'APROBADA'
			);

			const radicacionPendiente = response?.data?.some(
				(item: any) =>
					(
						String(item?.descripcionTarea).includes('Radicación') ||
						String(item?.descripcionTarea).includes('Radicacion')
					) &&
					item?.descripcionEstadoSolicitud === 'PENDIENTE'
			);

			const acreditacionV8 = response?.data?.some(
				(item: any) =>
					String(item?.descripcionTarea).includes('Verificación en tiempo') &&
					item?.descripcionEstadoSolicitud === 'PENDIENTE'
			);

			if (esRadicacionDevueltaPorRechazo) {
				setCampoSoloLecturaRadicacionAprobada(false);
				return;
			}

			setCampoSoloLecturaRadicacionAprobada(
				!!radicacionAprobada && !radicacionPendiente && !acreditacionV8
			);

			const solicitudAnuladaStorage = sessionStorage.getItem('solicitudAnuladaParaLimpiar');

			const esMismaSolicitudAnulada =
				solicitudAnuladaIdRef.current === responseSolicitud?.solicitudId ||
				solicitudAnuladaStorage === responseSolicitud?.solicitudId;

			const esTareaRadicacion =
				String(responseSolicitud?.nombreTarea || '')
					.toLowerCase()
					.normalize('NFD')
					.replace(/[\u0300-\u036f]/g, '')
					.includes('radicacion');

			if (esMismaSolicitudAnulada && esTareaRadicacion) {
				setCampoSoloLecturaRadicacionAprobada(false);
				return;
			}

		} catch (error) {

			setTrazabilidad([]);

		} finally {
			setLoading(false);
			setIsLoading(false);
		}
	};

	const validarTrazabilidad = async () => {
		try {
			const response = await api.obtenerTrazabilidadSolicitud(responseSolicitud?.solicitudId);

			setTrazabilidad(response?.data || []);

			const radicacionAprobada = response?.data.find(
				(item: any) =>
					(String(item?.descripcionTarea).includes('Radicación') ||
						String(item?.descripcionTarea).includes('Radicacion')) &&
					item?.descripcionEstadoSolicitud === 'APROBADA'
			);

			const radicacionPendiente = response?.data.some(
				(item: any) =>
					(String(item?.descripcionTarea).includes('Radicación') ||
						String(item?.descripcionTarea).includes('Radicacion')) &&
					item?.descripcionEstadoSolicitud === 'PENDIENTE'
			);

			const acreditacionV8 = response?.data.some(
				(item: any) =>
					String(item?.descripcionTarea).includes('Verificación en tiempo') &&
					item?.descripcionEstadoSolicitud === 'PENDIENTE'
			);

			if (esRadicacionDevueltaPorRechazo) {
				setCampoSoloLecturaRadicacionAprobada(false);
				return;
			}

			setCampoSoloLecturaRadicacionAprobada(
				!!radicacionAprobada && !radicacionPendiente && !acreditacionV8
			);
			const solicitudAnuladaStorage = sessionStorage.getItem('solicitudAnuladaParaLimpiar');

			const esMismaSolicitudAnulada =
				solicitudAnuladaIdRef.current === responseSolicitud?.solicitudId ||
				solicitudAnuladaStorage === responseSolicitud?.solicitudId;

			const esTareaRadicacion =
				String(responseSolicitud?.nombreTarea || '')
					.toLowerCase()
					.normalize('NFD')
					.replace(/[\u0300-\u036f]/g, '')
					.includes('radicacion');

			if (esMismaSolicitudAnulada && esTareaRadicacion) {
				setCampoSoloLecturaRadicacionAprobada(false);
				return;
			}

		} catch (error) {

			toast('Error al consultar trazabilidad', {
				type: toast.TYPE.ERROR,
			});

		} finally {
			setLoading(false);
		}
	};


	const propietariosAdicionados = (
		propietarios: PropietarioMatriculaModel[],
	): void => {
		setPropietarioMatriculaModel(propietarios);
	};

	const tercerosApoderadoAdicionados = (
		terceros: TerceroApoderadoModel[]
	): void => {
		setTerceroApoderadoModel(terceros);
	};

	const tercerosBeneficiarioAdicionados = (
		terceros: TerceroBeneficiarioModel[]
	): void => {
		setTerceroBeneficiarioModel(terceros);
	};

	const tercerosBeneficiarioEstudioAdicionados = (
		terceros: any[]
	): void => {
		setTerceroBeneficiarioEstudioModel(terceros);
	};

	const tercerosVendedorAdicionados = (terceros: TerceroVendedorModel[]): void => {
		setTerceroVendedorModel(terceros);
	}

	const beneficiariosAdicionados = (
		beneficiarios: any[]
	): void => {
		setTercerosBeneficiarios(beneficiarios);
	};

	const limpiarCamposConyugue = () => {
		formulario.setValue('tipoDocumentoConyugue', '');
		formulario.setValue('numeroDocumentoConyugue', '');
		formulario.clearErrors('tipoDocumentoConyugue');
		formulario.clearErrors('numeroDocumentoConyugue');
	}

	const selectedMinDate = () => {
		let date = null;
		if (responseSolicitud?.solicitudTerceroEntidadEducativaDto?.fechalimite !== undefined) {
			date = new Date(responseSolicitud?.solicitudTerceroEntidadEducativaDto?.fechalimite);
		} else {
			date = new Date();
		}
		const dd = date.getDate() + 5;
		const mm = date.getMonth();
		const yyyy = date.getFullYear();
		return new Date(yyyy, mm, dd);
	}

	const normalizarNivelRiesgoTusDatos = (nivel: any): string =>
		nivel
			?.toString()
			.trim()
			.toLowerCase()
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '') || '';

	const obtenerDocumentTypeTusDatos = (tipoDocumento: any): string => {
		const tipo = Number(tipoDocumento?.id || tipoDocumento);


		switch (tipo) {
			case 1:
				return 'CC';

			case 2:
				return 'NIT';

			case 3:
				return 'CE';

			case 4:
				return 'TI';

			default:
				return tipoDocumento?.codigo || tipoDocumento?.descripcion || tipoDocumento?.toString?.() || '';
		}
	};


	const agregarPersonaParaValidacionTusDatos = (
		lista: any[],
		tipoDocumento: any,
		identificacion: any,
		origen: string
	) => {
		const documentType = obtenerDocumentTypeTusDatos(tipoDocumento);
		const numeroDocumento = identificacion?.toString()?.trim();

		if (!documentType || !numeroDocumento) {
			return;
		}

		const existe = lista.some(
			(item: any) =>
				item.identificacion === numeroDocumento &&
				item.documentType === documentType
		);

		if (!existe) {
			lista.push({
				identificacion: numeroDocumento,
				documentType,
				origen,
			});
		}
	};

	const obtenerPersonasValidacionTusDatos = () => {
		const personas: any[] = [];

		/**
		 * PropietarioMatricula
		 */
		if (
			formulariosSolicitud?.some((formulario: string) => formulario === 'PropietarioMatricula') &&
			propietarioMatriculaModel?.length > 0
		) {
			propietarioMatriculaModel.forEach((propietario: any) => {
				agregarPersonaParaValidacionTusDatos(
					personas,
					propietario?.tipoIdentificacion?.id || propietario?.tipoIdentificacion,
					propietario?.numeroIdentificacion,
					'PropietarioMatricula'
				);
			});
		}

		/**
		 * TerceroApoderado
		 */
		if (
			formulariosSolicitud?.some((formulario: string) => formulario === 'TerceroApoderado') &&
			terceroApoderadoModel?.length > 0
		) {
			terceroApoderadoModel.forEach((apoderado: any) => {
				agregarPersonaParaValidacionTusDatos(
					personas,
					apoderado?.tipoDoc,
					apoderado?.numDoc,
					'TerceroApoderado'
				);
			});
		}

		/**
		 * TerceroBeneficiarioPagosVu
		 */
		if (
			formulariosSolicitud?.some(
				(formulario: string) =>
					formulario === 'TerceroBeneficiarioPagosVu' ||
					formulario === 'TerceroBeneficiario'
			) &&
			terceroBeneficiarioModel?.length > 0
		) {
			terceroBeneficiarioModel.forEach((beneficiario: any) => {
				agregarPersonaParaValidacionTusDatos(
					personas,
					beneficiario?.tipoDoc,
					beneficiario?.numDoc,
					'TerceroBeneficiarioPagosVu'
				);
			});
		}

		/**
		 * TerceroBeneficiarioEstudio
		 */
		if (
			formulariosSolicitud?.some((formulario: string) => formulario === 'TerceroBeneficiarioEstudio') &&
			terceroBeneficiarioEstudioModel?.length > 0
		) {
			terceroBeneficiarioEstudioModel.forEach((beneficiarioEstudio: any) => {
				agregarPersonaParaValidacionTusDatos(
					personas,
					beneficiarioEstudio?.tipoDocEstudioObj?.id ||
					beneficiarioEstudio?.tipoDocEstudio,
					beneficiarioEstudio?.numDocEstudio,
					'TerceroBeneficiarioEstudio'
				);
			});
		}

		/**
		 * EntidadEducativa
		 * Normalmente es NIT, por eso se usa 2 si no llega tipoDoc.
		 */
		if (
			formulariosSolicitud?.some((formulario: string) => formulario === 'EntidadEducativa') &&
			estadoEntidadNueva?.length > 0
		) {
			estadoEntidadNueva.forEach((entidad: any) => {
				agregarPersonaParaValidacionTusDatos(
					personas,
					entidad?.tipoDoc || entidad?.tipoDocumento || 2,
					entidad?.numDoc || entidad?.nit,
					'EntidadEducativa'
				);
			});
		}

		return personas;
	};

	const obtenerMensajeRiesgoPorRol = (
		rolMensaje: string,
		nivelRiesgo: string
	): string => {
		if (nivelRiesgo === 'medio') {
			return `${rolMensaje} con riesgo medio, por favor validar pertinencia.`;
		}

		if (nivelRiesgo === 'alto') {
			return `${rolMensaje} riesgo alto. Se requiere revisar la operación. Por favor comunicarse con la Unidad de Cumplimiento.`;
		}

		return '';
	};

	const agregarPersonaAlertaRiesgo = (
		lista: any[],
		tipoDocumento: any,
		identificacion: any,
		rolMensaje: string,
		ordenMensaje: number
	) => {
		const documentType = obtenerDocumentTypeTusDatos(tipoDocumento);
		const numeroDocumento = identificacion?.toString()?.trim();

		if (!documentType || !numeroDocumento) {
			return;
		}

		const existe = lista.some(
			(item: any) =>
				item.rolMensaje === rolMensaje &&
				item.identificacion === numeroDocumento &&
				item.documentType === documentType
		);

		if (!existe) {
			lista.push({
				rolMensaje,
				ordenMensaje,
				documentType,
				identificacion: numeroDocumento,
			});
		}
	};


	const obtenerPersonasAlertaRiesgoTusDatos = () => {
		const personas: any[] = [];

		const tieneFormularioPropietario = formulariosSolicitud?.some(
			(formulario: string) => formulario === 'PropietarioMatricula'
		);

		const tieneFormularioApoderado = formulariosSolicitud?.some(
			(formulario: string) => formulario === 'TerceroApoderado'
		);

		const tieneFormularioBeneficiario = formulariosSolicitud?.some(
			(formulario: string) =>
				formulario === 'TerceroBeneficiarioPagosVu' ||
				formulario === 'TerceroBeneficiario' ||
				formulario === 'TerceroBeneficiarioPagosDesafilaciones'
		);

		const tieneFormularioBeneficiarioEstudio = formulariosSolicitud?.some(
			(formulario: string) =>
				formulario === 'TerceroBeneficiarioEstudio'
		);

		// 1. Propietario
		if (tieneFormularioPropietario && propietarioMatriculaModel?.length > 0) {
			propietarioMatriculaModel.forEach((propietario: any) => {
				agregarPersonaAlertaRiesgo(
					personas,
					propietario?.tipoIdentificacion?.id ||
					propietario?.tipoIdentificacion,
					propietario?.numeroIdentificacion,
					'Propietario',
					1
				);
			});
		}

		// 2. Apoderado
		if (tieneFormularioApoderado && terceroApoderadoModel?.length > 0) {
			terceroApoderadoModel.forEach((apoderado: any) => {
				agregarPersonaAlertaRiesgo(
					personas,
					apoderado?.tipoDoc,
					apoderado?.numDoc,
					'Apoderado',
					2
				);
			});
		}

		// 3. Beneficiario de pago
		if (tieneFormularioBeneficiario && terceroBeneficiarioModel?.length > 0) {
			terceroBeneficiarioModel.forEach((beneficiario: any) => {
				agregarPersonaAlertaRiesgo(
					personas,
					beneficiario?.tipoDoc,
					beneficiario?.numDoc,
					'Beneficiario',
					3
				);
			});
		}

		// 4. Beneficiario de estudio
		if (
			tieneFormularioBeneficiarioEstudio &&
			terceroBeneficiarioEstudioModel?.length > 0
		) {
			terceroBeneficiarioEstudioModel.forEach(
				(beneficiarioEstudio: any) => {
					agregarPersonaAlertaRiesgo(
						personas,
						beneficiarioEstudio?.tipoDocEstudioObj?.id ||
						beneficiarioEstudio?.tipoDocEstudio,
						beneficiarioEstudio?.numDocEstudio,
						'Beneficiario de estudio',
						4
					);
				}
			);
		}

		return personas.sort(
			(a: any, b: any) => Number(a.ordenMensaje) - Number(b.ordenMensaje)
		);
	};

	const mostrarAlertasRiesgoTusDatosActualizacion = async (): Promise<void> => {
		const personas = obtenerPersonasAlertaRiesgoTusDatos();

		if (personas.length === 0) {
			return;
		}

		const resultados = await Promise.all(
			personas.map(async (persona: any) => {

				try {
					const responseTusDatos = await apiCredit.validarTusDatosConPolling(
						persona.identificacion,
						persona.documentType
					);

					const detectionRiskRaw =
						responseTusDatos?.data?.historyDataCustomer?.detectionRisk ??
						responseTusDatos?.data?.detectionRisk;

					const detectionRiskNormalizado =
						normalizarNivelRiesgoTusDatos(detectionRiskRaw);

					return {
						...persona,
						detectionRiskRaw,
						detectionRiskNormalizado,
					};
				} catch (error) {
					console.error('Error validando riesgo TusDatos en actualización:', {
						persona,
						error,
					});

					return {
						...persona,
						detectionRiskRaw: null,
						detectionRiskNormalizado: '',
						error,
					};
				}
			})
		);

		const mensajes = resultados
			.filter((resultado: any) =>
				resultado.detectionRiskNormalizado === 'medio' ||
				resultado.detectionRiskNormalizado === 'alto'
			)
			.sort(
				(a: any, b: any) =>
					Number(a.ordenMensaje) - Number(b.ordenMensaje)
			)
			.map((resultado: any) =>
				obtenerMensajeRiesgoPorRol(
					resultado.rolMensaje,
					resultado.detectionRiskNormalizado
				)
			)
			.filter((mensaje: string) => mensaje)
			.filter(
				(mensaje: string, index: number, array: string[]) =>
					array.indexOf(mensaje) === index
			);

		if (mensajes.length === 0) {
			return;
		}

		await mensajes.reduce<Promise<void>>(
			(promesaAnterior, mensaje) =>
				promesaAnterior
					.then(() =>
						Promise.resolve(
							NotificationService.genericAlwarning(
								'Alerta',
								mensaje,
								'warning'
							)
						)
					)
					.then(() => undefined),
			Promise.resolve()
		);
	};


	const obtenerNivelRiesgoTusDatosAprobacion = async (): Promise<{
		nivelRiesgo: number;
		mostrarAlertaRiesgo: boolean;
	}> => {
		const personas = obtenerPersonasValidacionTusDatos();

		if (personas.length === 0) {
			setNivelRiesgoTusDatos(0);

			return {
				nivelRiesgo: 0,
				mostrarAlertaRiesgo: false,
			};
		}

		const resultadosRiesgo = await Promise.all(
			personas.map(async (persona: any) => {
				try {
					const responseTusDatos = await apiCredit.validarTusDatosConPolling(
						persona.identificacion,
						persona.documentType
					);

					const detectionRiskRaw =
						responseTusDatos?.data?.historyDataCustomer?.detectionRisk ??
						responseTusDatos?.data?.detectionRisk;

					const detectionRiskNormalizado =
						normalizarNivelRiesgoTusDatos(detectionRiskRaw);

					const tieneNivelRiesgo = !!detectionRiskNormalizado;
					const esAlto = detectionRiskNormalizado === 'alto';

					return {
						persona,
						detectionRiskRaw,
						detectionRiskNormalizado,
						tieneNivelRiesgo,
						esAlto,
					};
				} catch (error) {
					console.error('Error validando nivel de riesgo TusDatos:', {
						persona,
						error,
					});

					return {
						persona,
						detectionRiskRaw: null,
						detectionRiskNormalizado: null,
						tieneNivelRiesgo: false,
						esAlto: false,
						error,
					};
				}
			})
		);

		const existeNivelRiesgo = resultadosRiesgo.some(
			(resultado: any) => resultado.tieneNivelRiesgo
		);

		const existeRiesgoAlto = resultadosRiesgo.some(
			(resultado: any) => resultado.esAlto
		);

		const nivelRiesgo = existeRiesgoAlto ? 1 : 0;

		setNivelRiesgoTusDatos(nivelRiesgo);

		return {
			nivelRiesgo,
			mostrarAlertaRiesgo: false,
		};
	};


	const onSubmitAprobar = async () => {

		const errores = obtenerErroresFormularios(true);

		if (errores.length > 0) {
			return;
		}


		setLoading(true);

		if (modalidadSolicitud === MODALIDAD_SOLICITUD.PCUOTAINI) {
			handleOpenModalCreditoHipotecarioPagoCuotaInical();

			if (typeof obtnerBoleano === 'function') {
				obtnerBoleano(true);
			}
		} else {
			const mensajeConfirmacionAprobar = esRadicacionDevueltaPorRechazo
				? '¿Está seguro que desea continuar?'
				: '¿Está seguro de que la información ingresada en el FUP es correcta?';

			const confirmAlert = await NotificationService.confirmAlert(
				mensajeConfirmacionAprobar
			);

			if (confirmAlert) {
				if (esRadicacionDevueltaPorRechazo) {
					setLoadingAprobacion(true);
				}

				await aprobarSolicitudTarea();

				if (typeof obtnerBoleano === 'function') {
					obtnerBoleano(true);
				}
			} else {
				setLoading(false);
				setLoadingAprobacion(false);
			}
		}
	};

	const onSubmitAprobarCreditoHipotecarioPagoCuotaInical = async () => {

		const parametrosDesembolsoSevenDto = {
			solicitudId: responseSolicitud?.solicitudId as string,
			IdCliente: responseSolicitud?.clienteId as number,

		} as ParametrosDesembolsoSevenModel;

		const responsePagoCuotaInicalSeven = await apiCredit.posPagoCuotaInicalSeven(parametrosDesembolsoSevenDto);
		if (responsePagoCuotaInicalSeven.Data == null) {
			toast(`Error: ${responsePagoCuotaInicalSeven.returnMessage}`, { type: toast.TYPE.ERROR });
			return;
		}

		await NotificationService.genericAlert('Pago realizado con éxito.', "Se genero la planilla de pago con éxito");
		aprobarSolicitudTarea();
	}

	const aprobarSolicitudTarea = async () => {
		setLoading(true);

		try {

			let nivelRiesgo = 0;

			if (!esRadicacionDevueltaPorRechazo) {
				const resultadoRiesgo = await obtenerNivelRiesgoTusDatosAprobacion();
				nivelRiesgo = resultadoRiesgo.nivelRiesgo;
			}

			let nivelRiesgoFinal = nivelRiesgo;

			if (esRadicacionDevueltaPorRechazo) {
				nivelRiesgoFinal = 0;
			} else if (nivelAfiliado === 1) {
				nivelRiesgoFinal = nivelAfiliado;
			}

			if (esRadicacionDevueltaPorRechazo) {
				const observacionPantalla = String(
					formulario.getValues('observaciones') ||
					formulario.getValues('observacion') ||
					''
				).trim();

				const dataActualizar = {
					...modeloSolicitudData(),
					observacion: observacionPantalla,
				};

				const responseActualizar = await api.PostActualizarSolicitudTarea(dataActualizar);



				if (!responseActualizar?.isSuccess) {
					toast(
						`Error: ${responseActualizar?.returnMessage || 'No fue posible actualizar la observación antes de aprobar.'}`,
						{ type: toast.TYPE.ERROR }
					);

					setLoading(false);
					setLoadingAprobacion(false);
					return;
				}
			}


			const aprobarSolicitudTareaDto = {
				solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
				clienteId: responseSolicitud?.clienteId as number,
				soL_RECHAZADA: responseSolicitud?.soL_RECHAZADA,
				NivelRiesgo: nivelRiesgoFinal

			};

			const response = await api.PostAprobarSolicitudTarea(aprobarSolicitudTareaDto);


			if (response && response?.isSuccess) {
				agregaryActualizarIntervinientes();

				if (typeof obtnerBoleano === 'function') {
					obtnerBoleano(true);
				}

				toast('Se aprobó la solicitud correctamente.', {
					type: toast.TYPE.SUCCESS,
				});

				const modelConsultaPdf: ConsultarSolicitudModel = {
					solicitudTareaId: responseSolicitud?.solicitudTareaId || '',
					tipoSubModalidadId: GUID_DEFAULT,
					identificacion: responseSolicitud?.identificacionCliente || '',
					tipoIdentificacion: responseSolicitud?.tipoIdentificacion || 0,
					estadoTarea: 0,
					tipoSolicitud: responseSolicitud?.solicitudTareaId,
					numRadicado: '',
					tareaNombre: responseSolicitud?.nombreTarea || '',
					numeroSolicitud: '',
					usuarioIngreso: '',
					solicitante: '',
				};

				const solicitudFueRechazada = trazabilidad?.some(
					(item: any) =>
						item?.descripcionEstadoSolicitud?.toUpperCase?.() === 'RECHAZADA'
				);

				if (!solicitudFueRechazada) {
					try {
						const responsePdf = await api.PostObtenerPdfFup(modelConsultaPdf);

						const mimeType = responsePdf.type || 'application/pdf';
						const file = new Blob([responsePdf], { type: mimeType });
						const fileURL = URL.createObjectURL(file);

						const link = document.createElement('a');
						link.href = fileURL;
						link.download = `FUP-${responseSolicitud?.solicitudTareaId || 'solicitud'}.pdf`;
						document.body.appendChild(link);
						link.click();

						document.body.removeChild(link);
						URL.revokeObjectURL(fileURL);
					} catch (e: any) {
						toast(
							'La solicitud fue aprobada, pero no se pudo obtener el PDF FUP.',
							{ type: toast.TYPE.WARNING }
						);
					}
				}

				if (esRadicacionDevueltaPorRechazo) {

					setIsLoading(false);
					setLoading(false);

					if (typeof setLoadingAprobacion === 'function') {
						setLoadingAprobacion(false);
					}

					onItemBack();

					history.replace('/GestionSolicitudes');

					return;
				}

				setIsLoading(false);
				setLoading(false);

				if (typeof setLoadingAprobacion === 'function') {
					setLoadingAprobacion(false);
				}

				onItemBack();
				return;
			}
			toast(`Error: ${response?.returnMessage || 'No fue posible aprobar la solicitud'}`, {
				type: toast.TYPE.ERROR,
			});
		} catch (e) {
			console.error('Error aprobando solicitud:', e);
			console.error('ERROR EN aprobarSolicitudTarea:', e);
			toast('Error del sistema', {
				type: toast.TYPE.ERROR,
			});
		} finally {
			setIsLoading(false);
			setLoading(false);
			setLoadingAprobacion(false);
		}
	};

	const agregaryActualizarIntervinientes = async () => {

		const NIT = 2;
		// Propietarios Inmuebles
		const PROPIETARIO = '7d7783db-2134-430c-b44a-d2a9074d9469';
		if (propietarioMatriculaModel && propietarioMatriculaModel.length > 0) {
			propietarioMatriculaModel.forEach(async propietario => {

				try {
					const request =
					{
						tipoDocumento: propietario?.tipoIdentificacion?.id,
						numeroDocumento: propietario?.numeroIdentificacion,
						nombre: '',
						actorId: "00000000-0000-0000-0000-000000000000"
					}
					const responseInterviniente = await api.PostBuscarIntervinientesActores(request);
					if (responseInterviniente?.data && responseInterviniente?.data.length > 0 && Number(propietario?.tipoIdentificacion?.id) === 1) {

						const requestActualizar = {
							intervinientesId: responseInterviniente?.data[0]?.itR_ID,
							nombre: propietario?.primerNombre.toUpperCase(),
							segundoNombre: propietario?.segundoNombre.toUpperCase() || '',
							primerApellido: propietario?.primerApellido.toUpperCase(),
							segundoApellido: propietario?.segundoApellido.toUpperCase() || '',
							modificadoPor: user?.USR_ID,
							modulo: 'Solicitudes',
						}
						await api.
							PosttActualizarIntervinientes(requestActualizar)
							.then()
							.catch((e) => {
								toast(`Error al actualizar interviniente en propietario radicación ${e.message}`, { type: toast.TYPE.ERROR });
							});
					}

					if (responseInterviniente?.data && responseInterviniente?.data.length === 0) {
						const request = {
							solId: responseSolicitud?.solicitudId as string,
							tipoDocumento: propietario?.tipoIdentificacion?.id,
							numeroDocumento: propietario?.numeroIdentificacion,
							nombre: propietario?.tipoIdentificacion?.id === NIT ? propietario?.razonSocial : propietario?.primerNombre,
							segundoNombre: propietario?.segundoNombre || '',
							primerApellido: propietario?.primerApellido || '',
							segundoApellido: propietario?.segundoApellido || '',
							personaJuridicaNatural: personaJuridicaoNatural(propietario?.tipoIdentificacion?.id),
							celular: '',
							correoElectronico: '',
							departamento: 0,
							ciudad: 0,
							direccion: '',
							barrio: '',
							telefono: '',
							tipoActor: PROPIETARIO,
							tipoCuenta: '',
							numeroCuentaBancaria: '',
							entidadBancaria: 0,
							fechaExpedicionCedula: null,
							constructora: 'no',
							convenioEntidad: 'no',
						}
						crearTipoActor(request);
					}

				} catch (e: any) {
					toast(`Error al consultar intervinientes propietarios en radicación. ${e.message}`, { type: toast.TYPE.ERROR });
				}
			})
		}

		// Apoderado Autorizado
		const APODERADO_AUTORIZADO = 'f76b4351-c155-474b-9e47-307e8d0d6ffc';
		if (terceroApoderadoModel && terceroApoderadoModel.length > 0) {

			terceroApoderadoModel.forEach(async apoderadoAutorizado => {

				try {
					const request =
					{
						tipoDocumento: apoderadoAutorizado?.tipoDoc,
						numeroDocumento: apoderadoAutorizado?.numDoc,
						nombre: '',
						actorId: "00000000-0000-0000-0000-000000000000"
					}
					const responseInterviniente = await api.PostBuscarIntervinientesActores(request);
					if (responseInterviniente?.data && responseInterviniente?.data.length > 0 && Number(apoderadoAutorizado?.tipoDoc) === 1) {
						const requestActualizar = {
							intervinientesId: responseInterviniente?.data[0]?.itR_ID,
							nombre: apoderadoAutorizado?.primerNombre.toUpperCase(),
							segundoNombre: apoderadoAutorizado?.segundoNombre.toUpperCase() || '',
							primerApellido: apoderadoAutorizado?.primerApellido.toUpperCase(),
							segundoApellido: apoderadoAutorizado?.segundoApellido.toUpperCase() || '',
							modificadoPor: user?.USR_ID,
							modulo: 'Solicitudes',
						}
						await api.
							PosttActualizarIntervinientes(requestActualizar)
							.then()
							.catch((e) => {
								toast(`Error al actualizar interviniente en apoderado radicación ${e.message}`, { type: toast.TYPE.ERROR });
							});
					}
					if (responseInterviniente?.data && responseInterviniente?.data.length === 0) {
						const request = {
							solId: responseSolicitud?.solicitudId as string,
							tipoDocumento: apoderadoAutorizado?.tipoDoc,
							numeroDocumento: apoderadoAutorizado?.numDoc,
							nombre: apoderadoAutorizado?.primerNombre,
							segundoNombre: apoderadoAutorizado?.segundoNombre || '',
							primerApellido: apoderadoAutorizado?.primerApellido || '',
							segundoApellido: apoderadoAutorizado?.segundoApellido || '',
							personaJuridicaNatural: personaJuridicaoNatural(apoderadoAutorizado?.tipoDoc),
							celular: '',
							correoElectronico: '',
							departamento: 0,
							ciudad: 0,
							direccion: '',
							barrio: '',
							telefono: '',
							tipoActor: APODERADO_AUTORIZADO,
							tipoCuenta: '',
							numeroCuentaBancaria: '',
							entidadBancaria: 0,
							fechaExpedicionCedula: null,
							constructora: 'no',
							convenioEntidad: 'no',
						}
						crearTipoActor(request);
					}

				} catch (e: any) {
					toast(`Error al consultar intervinientes apoderado autorizado en radicación. ${e.message}`, { type: toast.TYPE.ERROR });
				}
			})
		}

		// Beneficiario de Pago
		const BENEFICIARIO_DE_PAGO = 'bbdae24c-b2d8-4c86-a733-291695043860';
		if (terceroBeneficiarioModel && terceroBeneficiarioModel.length > 0) {
			terceroBeneficiarioModel.forEach(beneficiarioPago => {
				const entidadBancaria = catalogState?.entidadBancaria.find(entidad => entidad.idEntidadBancaria === beneficiarioPago?.entidad);
				const request = {
					solId: responseSolicitud?.solicitudId as string,
					tipoDocumento: beneficiarioPago?.tipoDoc,
					numeroDocumento: beneficiarioPago?.numDoc,
					nombre: beneficiarioPago?.razonsocial,
					segundoNombre: '',
					primerApellido: '',
					segundoApellido: '',
					personaJuridicaNatural: personaJuridicaoNatural(beneficiarioPago?.tipoDoc),
					celular: '',
					correoElectronico: '',
					departamento: 0,
					ciudad: 0,
					direccion: '',
					barrio: '',
					telefono: '',
					tipoActor: BENEFICIARIO_DE_PAGO,
					tipoCuenta: `${beneficiarioPago?.tipoCuentaObj?.id}_${beneficiarioPago?.tipoCuentaObj?.descripcion}`,
					numeroCuentaBancaria: beneficiarioPago?.numeroCuenta,
					entidadBancaria: entidadBancaria?.codigoBanco,
					fechaExpedicionCedula: null,
					constructora: 'no',
					convenioEntidad: 'no',
				}
				crearTipoActor(request);
			})
		}

		// Beneficiario de estudio
		if (terceroBeneficiarioEstudioModel && terceroBeneficiarioEstudioModel.length > 0) {
			terceroBeneficiarioEstudioModel.forEach(async beneficiarioEstudio => {
				if (beneficiarioEstudio?.tipoDeBeneficiario === 'HIJO/HIJA') {

					try {
						const request =
						{
							tipoDocumento: Number(beneficiarioEstudio?.tipoDocEstudioObj?.id),
							numeroDocumento: beneficiarioEstudio?.numDocEstudio,
							nombre: '',
							actorId: "00000000-0000-0000-0000-000000000000"
						}
						const responseInterviniente = await api.PostBuscarIntervinientesActores(request);
						if (responseInterviniente?.data && responseInterviniente?.data.length > 0 && Number(beneficiarioEstudio?.tipoDocEstudioObj?.id) === 1) {
							const requestActualizar = {
								intervinientesId: responseInterviniente?.data[0]?.itR_ID,
								nombre: beneficiarioEstudio?.primerNombre.toUpperCase(),
								segundoNombre: beneficiarioEstudio?.segundoNombre.toUpperCase() || '',
								primerApellido: beneficiarioEstudio?.primerApellido.toUpperCase(),
								segundoApellido: beneficiarioEstudio?.segundoApellido.toUpperCase() || '',
								modificadoPor: user?.USR_ID,
								modulo: 'Solicitudes',
							}
							await api.
								PosttActualizarIntervinientes(requestActualizar)
								.then()
								.catch((e) => {
									toast(`Error al actualizar interviniente en beneficiario de estudio radicación ${e.message}`, { type: toast.TYPE.ERROR });
								});
						}

						if (responseInterviniente?.data && responseInterviniente?.data.length === 0) {
							try {
								const response = await api.GetConsultarIntervinientesActores();
								if (response?.data) {
									const CODIGO_BENEFICIARIO_ESTUDIO = "BEST";
									const tipoActor = response?.data.find((item: any) => item?.itA_CODIGO === CODIGO_BENEFICIARIO_ESTUDIO);

									if (tipoActor) {
										const tipoBeneficiarioHijoHija = tipoActor?.itA_ID;
										const request = {
											solId: responseSolicitud?.solicitudId as string,
											tipoDocumento: Number(beneficiarioEstudio?.tipoDocEstudioObj?.id),
											numeroDocumento: beneficiarioEstudio?.numDocEstudio,
											nombre: beneficiarioEstudio?.primerNombre.toUpperCase(),
											segundoNombre: beneficiarioEstudio?.segundoNombre.toUpperCase() || '',
											primerApellido: beneficiarioEstudio?.primerApellido.toUpperCase(),
											segundoApellido: beneficiarioEstudio?.segundoApellido.toUpperCase() || '',
											personaJuridicaNatural: 'natural',
											celular: '',
											correoElectronico: '',
											departamento: 0,
											ciudad: 0,
											direccion: '',
											barrio: '',
											telefono: '',
											tipoActor: tipoBeneficiarioHijoHija,
											tipoCuenta: '',
											numeroCuentaBancaria: '',
											entidadBancaria: 0,
											fechaExpedicionCedula: null,
											constructora: 'no',
											convenioEntidad: 'no',
											numeroDocumentoAfiliado: responseSolicitud?.identificacionCliente,
											tipoDocumentoAfiliado: String(responseSolicitud?.tipoIdentificacion)
										}
										crearTipoActor(request);
									}

								}
							}
							catch (error) {
								toast('Error al consultar tipo de  beneficiario de estudio en solicitudes', { type: toast.TYPE.ERROR });
							}
						}

					} catch (e: any) {
						toast(`Error al consultar intervinientes beneficiario de estudio en radicación. ${e.message}`, { type: toast.TYPE.ERROR });
					}
				}
			})
		}

	}

	const crearTipoActor = async (request: any) => {
		await api.
			PostGuadarTipoActor(request)
			.catch(() => {
				toast('Error al guardar el tipo actor', { type: toast.TYPE.ERROR });
				setLoading(false)
			});
	}

	const personaJuridicaoNatural = (tipoDocumento: number) => {
		let personaJuridicaNatural = '';
		const CEDULA_DE_CIUDADANIA = 1;
		const NIT = 2;
		if (tipoDocumento === CEDULA_DE_CIUDADANIA) {
			personaJuridicaNatural = 'natural';
		} else if (tipoDocumento === NIT) {
			personaJuridicaNatural = 'juridica';
		}

		return personaJuridicaNatural;
	}

	const onCloseAnular = () => setOpenModalAnular(false);

	const validarObservacionParaAnular = (): boolean => {
		const observacion = obtenerObservacionActual();

		if (!observacion) {
			toast('Debe ingresar una observación para anular la solicitud.', {
				type: toast.TYPE.ERROR
			});
			return false;
		}

		return true;
	};

	// const onSubmitAnular = async () => {
	// 	if (responseSolicitud?.nombreTarea === 'Radicación V 8') {
	// 		setOpenModalAnular(true);
	// 		return;
	// 	}

	// 	const confirmAlert = await NotificationService.confirmAlert(
	// 		'¿Está seguro que desea continuar?'
	// 	);

	// 	if (confirmAlert) {
	// 		await funcionAnular();
	// 	}
	// };

	const onSubmitAnular = async () => {
		accionFormularioRef.current = 'ANULAR';

		if (!validarObservacionParaAnular()) {
			return;
		}

		if (responseSolicitud?.nombreTarea === 'Radicación V 8') {
			setOpenModalAnular(true);
			return;
		}

		const confirmAlert = await NotificationService.confirmAlert(
			'¿Está seguro que desea continuar?'
		);

		if (confirmAlert) {
			await funcionAnular();
		}
	};


	const { publicIp } = useIp();
	const auditoriaApi = AuditoriaApi();

	const registrarAuditoriaAnular = (accion: string) => {
		if (!publicIp) {
			console.warn("IP aún no disponible");
			return;
		}

		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands.find(
			(x: { brand: string }) => x.brand === "Google Chrome" || x.brand === "Microsoft Edge"
		)?.brand || "Navegador desconocido";

		const requestAuditoria = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData?.platform || "Desconocido",
			nombreUsuario: user.USR_USERNAME,
			usurioId: user.USR_ID,
			ip: publicIp,
			modulo: "Solicitudes",
			hostname: document.location.host,
			tipoAccion: "ANULAR",
			idTipoAccion: 4,
			descripcion: `El usuario ${user.USR_USERNAME} realizó la acción: ${accion}`,
		};

		auditoriaApi.InsertAuditoria(requestAuditoria);
	};



	const obtenerObservacionActual = (): string => {
		const valoresObservacion = formularioObservacionesActivo?.getValues?.() || {};

		return String(
			valoresObservacion?.observaciones ??
			valoresObservacion?.observacion ??
			formulario.getValues('observaciones') ??
			formulario.getValues('observacion') ??
			''
		).trim();
	};

	const limpiarFormularioPorAnulacion = () => {
		formulario.reset({
			tipoDeRecepcion: '',
			radPreTramite: '',
			numeroMemorando: '',
			tipoTurno: '',
			numeroTurno: '',
			sucursal: '',
			fechaMemorando: '',

			tipoDocumentoConyugue: '',
			numeroDocumentoConyugue: '',
			primerNombreConyuge: '',
			segundoNombreConyuge: '',
			primerApellidoConyuge: '',
			segundoApellidoConyuge: '',

			observaciones: '',
			observacion: '',
			observacionSolicitud: '',

			oficinaInmueble: 0,
			codigoOficina: '',
			ciudad: '',
			departamento: '',
			direccion: '',
			numeroMatricula: '',
			numeroMatriculaBuscar: '',
			solicitudMatricula: {},

			valorSolicitud: '',
			numero_Folio: '',
			fecha_Firma_Escritura: new Date(),
			posee_Avac: false,

			razonSocial: '',
			programaentidad: '',
			nivelEducativo: '',
			numReciboPago: '',
			fechaLimitePago: new Date(),

			entidadSeguroEducativo: '',
			numeroPolizaSeguro: '',
			tipoDoc: '',
			numDoc: '',
			correo: '',
			telefono: '',

			aceptarDeclaracionesAutorizaciones: false,
			causal: '0',
			destinacionDinero: '',
			saldoCuenta: '',
			cuenta: '',
			autorizarTraslado: false,
		});

		formulario.clearErrors();

		formularioObservacionesActivo?.reset?.({
			observaciones: '',
			observacion: '',
		});

		setMatriculaInmobiliariaModel([]);
		setPropietarioMatriculaModel([]);
		setTerceroApoderadoModel([]);
		setTerceroBeneficiarioModel([]);
		setTerceroBeneficiarioEstudioModel([]);
		setTerceroVendedorModel([]);
		// setBeneficiarioAfiliado([]);
		setBeneficiarios([]);
		setEntidadesFinancieras([]);
		setValidarBeneficiario([]);
		setEstadoEntidadNueva([]);
		setEntidadAhorroProgramadoSeguroEducativo([]);
		setBeneficiariosCompartidos([]);
		setApoderadosAutorizados([]);
		setAceptarCheckValue(false);
		setDestinacionDinerosSeleccionado('');
		setSaldoCuenta(null);
		setNombreCuenta('');
		setAceptaTraslado(false);
		setNivelRiesgoTusDatos(0);
		setPropietariosMatriculaValidacion([]);
		setMatriculasPendientesEliminar([]);
		setActiveTab(TABS_INDEX.RESUMENSOLICITUD);
		setCampoSoloLecturaRadicacionAprobada(false);
	};


	const esGuidValido = (value: any): boolean =>
		typeof value === 'string' &&
		/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);



	const funcionAnular = async () => {
		if (!responseSolicitud?.solicitudTareaId) {
			toast('No fue posible identificar la tarea actual para anular.', {
				type: toast.TYPE.ERROR
			});
			return;
		}

		const observacionActual = obtenerObservacionActual();

		if (!observacionActual) {
			toast('Debe ingresar una observación antes de anular la solicitud.', {
				type: toast.TYPE.ERROR
			});
			return;
		}

		try {
			setIsLoading(true);

			const dataActualizar = modeloSolicitudData({ paraAnular: true });

			const responseActualizar = await api.PostActualizarSolicitudTarea(dataActualizar);

			if (!responseActualizar?.isSuccess) {
				toast(
					`Error: ${responseActualizar?.returnMessage || 'No fue posible actualizar la información antes de anular.'}`,
					{ type: toast.TYPE.ERROR }
				);
				return;
			}

			const solicitudTareaDto = {
				solicitudTareaId: responseSolicitud.solicitudTareaId as string
			};

			const responseAnular = await api.PostAnularSolicitudTarea(solicitudTareaDto);

			if (responseAnular?.isSuccess) {
				registrarAuditoriaAnular("ANULAR");

				solicitudAnuladaIdRef.current = responseSolicitud.solicitudId as string;

				sessionStorage.setItem(
					'solicitudAnuladaParaLimpiar',
					responseSolicitud.solicitudId as string
				);

				setLimpiezaPostAnulacionPendiente(true);

				await onItemGoBack(false);

				obtnerBoleano(true);

				toast('Se anuló la solicitud correctamente.', {
					type: toast.TYPE.SUCCESS
				});

				return;
			}

			toast(
				responseAnular?.returnMessage || 'No fue posible anular la solicitud.',
				{ type: toast.TYPE.ERROR }
			);
		} catch (e) {
			console.error('Error anulando solicitud:', e);

			toast('Error del sistema: Error función anular', {
				type: toast.TYPE.ERROR
			});
		} finally {
			setIsLoading(false);
		}
	};


	const onSubmitRechazar = async () => {

		const confirmAlert = await NotificationService.confirmAlert('¿Está seguro que desea continuar?');
		if (confirmAlert) {

			const rechazarSolicitudTareaDto = {
				solicitudTareaId: responseSolicitud?.solicitudTareaId as string
			};

			try {
				const response = await api.PostRechazarSolicitudTarea(rechazarSolicitudTareaDto);
				if (response && response?.isSuccess) {
					toast('Se rechazó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
					onItemBack();
				} else {
					toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
				}
			} catch (e) {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			}
		}
	}


	const obtenerGuidONull = (value: any): string | null => {
		if (!value) {
			return null;
		}

		if (typeof value === 'object') {
			const id = value?.id || value?.value || value?.guid;

			return esGuidValido(id) ? id : null;
		}

		return esGuidValido(String(value)) ? String(value) : null;
	};

	const GUID_SWAGGER = '3fa85f64-5717-4562-b3fc-2c963f66afa6';

	const obtenerGuidSeguro = (...values: any[]): string => {
		const valorEncontrado = values
			.map((value: any) => {
				if (!value) {
					return '';
				}

				const posibleValor =
					typeof value === 'object'
						? value?.id || value?.value || value?.guid
						: value;

				if (!posibleValor) {
					return '';
				}

				return String(posibleValor).trim();
			})
			.find((valor: string) =>
				esGuidValido(valor) &&
				valor !== GUID_SWAGGER
			);

		return valorEncontrado || GUID_DEFAULT;
	};

	const obtenerTextoSeguro = (...values: any[]): string => {
		const valorEncontrado = values
			.map((value: any) => {
				if (value === null || value === undefined) {
					return '';
				}

				return String(value).trim();
			})
			.find((valor: string) =>
				valor &&
				valor.toLowerCase() !== 'null' &&
				valor.toLowerCase() !== 'undefined'
			);

		return valorEncontrado || '0';
	};

	// const obtenerFechaTurnoSegura = (...values: any[]): Date => {
	// 	const fechaEncontrada = values
	// 		.map((value: any) => {
	// 			if (!value) {
	// 				return null;
	// 			}

	// 			const fecha = new Date(value);

	// 			if (Number.isNaN(fecha.getTime())) {
	// 				return null;
	// 			}

	// 			return fecha;
	// 		})
	// 		.find((fecha: Date | null) => fecha !== null);

	// 	return fechaEncontrada || new Date('1970-01-01T00:00:00.000Z');
	// };

	const obtenerNumeroSeguro = (value: any, valorDefecto = 0): number => {
		if (value === null || value === undefined || value === '') {
			return valorDefecto;
		}

		const numero = Number(UtilService.cleanCurrencyValue(value));

		return Number.isNaN(numero) ? valorDefecto : numero;
	};

	const obtenerBooleanSeguro = (value: any): boolean =>
		value === true;


	const obtenerFechaSegura = (value: any): Date => {
		if (!value || value === '0001-01-01T00:00:00') {
			return new Date();
		}

		const fecha = new Date(value);

		return Number.isNaN(fecha.getTime()) ? new Date() : fecha;
	};

	const limpiarObjetoVacio = (obj: any): any => {
		if (obj === null || obj === undefined) return undefined;

		if (Array.isArray(obj)) {
			return obj;
		}

		if (typeof obj !== 'object') {
			return obj;
		}

		const limpio: any = {};

		Object.entries(obj).forEach(([key, value]) => {
			if (value === null || value === undefined) return;

			if (typeof value === 'string') {
				const valor = value.trim();

				if (!valor) return;
				if (valor.toLowerCase() === 'null') return;
				if (valor.toLowerCase() === 'undefined') return;

				limpio[key] = valor;
				return;
			}

			if (Array.isArray(value)) {
				limpio[key] = value;
				return;
			}

			if (typeof value === 'object' && !(value instanceof Date)) {
				const objetoLimpio = limpiarObjetoVacio(value);

				if (
					objetoLimpio &&
					typeof objetoLimpio === 'object' &&
					Object.keys(objetoLimpio).length > 0
				) {
					limpio[key] = objetoLimpio;
				}

				return;
			}

			limpio[key] = value;
		});

		return Object.keys(limpio).length > 0 ? limpio : undefined;
	};


	const modeloSolicitudData = (opciones?: { paraAnular?: boolean }): SolicitudTareaModel => {
		const observacionFinal = obtenerObservacionActual();

		const valorRetirarFinal = obtenerNumeroSeguro(
			formulario.getValues('valorSolicitud'),
			0
		);

		const numeroFolioFinal = obtenerNumeroSeguro(
			formulario.getValues('numero_Folio'),
			0
		);

		const fechaFirmaEscritura = obtenerFechaSegura(
			formulario.getValues('fecha_Firma_Escritura')
		);


		const data: any = {
			solicitudId: responseSolicitud?.solicitudId as string,
			solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
			creadoPor: user.USR_ID,

			valorRetirar: valorRetirarFinal,
			observacion: observacionFinal,
			posee_Avac: obtenerBooleanSeguro(formulario.getValues('posee_Avac')),
			numero_Folio: numeroFolioFinal,
			fecha_Firma_Escritura: fechaFirmaEscritura,

			conyugueDto: [],
			solicitudMatriculaDto: Array.isArray(matriculaInmobiliariaModel)
				? matriculaInmobiliariaModel
				: [],

			solicitudPropietarioDto:
				tieneFormularioPropietarioMatricula &&
					Array.isArray(propietarioMatriculaModel)
					? propietarioMatriculaModel
					: [],

			solicitudTerceroApoderadoDto: Array.isArray(terceroApoderadoModel)
				? terceroApoderadoModel
				: [],

			solicitudBeneficiarioDto: Array.isArray(tercerosBeneficiarios)
				? tercerosBeneficiarios
				: [],

			solicitudTerceroBeneficiarioDto: Array.isArray(terceroBeneficiarioModel)
				? terceroBeneficiarioModel
				: [],

			solicitudTerceroVendedorDto: Array.isArray(terceroVendedorModel)
				? terceroVendedorModel
				: [],

			solicitudTerceroEntidadEducativaDto: [],
			solicitudTerceroBeneficiarioEstudioDto: Array.isArray(terceroBeneficiarioEstudioModel)
				? terceroBeneficiarioEstudioModel
				: [],

			informacionTurnoDto: construirInformacionTurnoDto(),
		};


		const dataLimpia = limpiarObjetoVacio(data) || data;

		return dataLimpia as SolicitudTareaModel;
	};

	const agregarMatriculasPendientesEliminar = (valor: any) => {
		const lista = Array.isArray(valor) ? valor : [valor];

		setMatriculasPendientesEliminar(prev => {
			const combinadas = [...prev, ...lista];

			const sinDuplicados = combinadas.filter((matricula, index, self) => {
				const idMatricula = String(matricula?.id ?? matricula?.maiId ?? '');
				const numeroMatricula = String(
					matricula?.numeroMatricula ?? matricula?.maiNumeroMatricula ?? ''
				);

				return index === self.findIndex((item: any) => {
					const idItem = String(item?.id ?? item?.maiId ?? '');
					const numeroItem = String(
						item?.numeroMatricula ?? item?.maiNumeroMatricula ?? ''
					);

					return (
						(idMatricula && idItem === idMatricula) ||
						(numeroMatricula && numeroItem === numeroMatricula)
					);
				});
			});

			return sinDuplicados;
		});
	};

	const eliminarMatriculasPendientesEnBackend = async () => {

		if (!matriculasPendientesEliminar.length) return;

		const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';

		const isGuid = (value: any) =>
			typeof value === 'string' &&
			/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);

		const idUsuario = isGuid(user?.USR_ID)
			? user.USR_ID
			: EMPTY_GUID;

		const responses = await Promise.all(
			matriculasPendientesEliminar.map(async (matricula: any) => {
				const codigoOficina =
					matricula?.codigoOficina ??
					matricula?.oficinaCodigo ??
					(matricula?.numeroMatricula?.includes('-')
						? matricula.numeroMatricula.split('-')[0]
						: '');

				const request = {
					maiId: matricula?.maiId ?? matricula?.id,
					maiNumeroMatricula: matricula?.maiNumeroMatricula ?? matricula?.numeroMatricula,
					maiDireccion: matricula?.maiDireccion ?? matricula?.direccion ?? '',
					maiFechaRegistro: matricula?.maiFechaRegistro ?? new Date().toISOString(),

					prpTipoIdentificacion: '',
					prpNumeroIdentificacion: '',
					prpNombreRazonSocial: '',
					prpPrimerNombre: '',
					prpSegundoNombre: '',
					prpPrimerApellido: '',
					prpSegundoApellido: '',

					dcpId: Number(matricula?.dcpId ?? matricula?.ciudad?.id ?? matricula?.ciudadId ?? 0),
					dpdId: Number(matricula?.dpdId ?? matricula?.departamento?.id ?? matricula?.departamentoId ?? 0),
					idOficinaRegistro: Number(matricula?.idOficinaRegistro ?? matricula?.oficinaRegistroId ?? 0),

					estado: matricula?.estado ?? (matricula?.esPrincipal ? 'Principal' : 'Relacionado'),

					nombreUsuario: user?.USR_USERNAME ?? '',
					idUsuario,
					correo: user?.USR_EMAIL ?? '',
					celular: user?.USR_CELULAR ?? '',

					nroCelular: '',
					tipoIdentificacion: '',
					mail: '',
					codigoOficina,

					propietarios: [],
					novedades: [],
					tramites: [],
					cambios: [],
					causales: [],
				};

				return apiRealState.DeleteRealStateRegistrations(request as any);
			}),
		);

		const errorResponse = responses.find((response: any) => response?.isSuccess === false);

		if (errorResponse) {
			throw new Error(
				errorResponse?.returnMessage ||
				'No fue posible eliminar una matrícula pendiente.',
			);
		}

		setMatriculasPendientesEliminar([]);
	};

	const GUID_TURNO_VACIO = '00000000-0000-0000-0000-000000000000';
	const GUID_TURNO_SWAGGER = '3fa85f64-5717-4562-b3fc-2c963f66afa6';

	const esGuidTurnoValido = (value: any): boolean => {
		const guid = String(value ?? '').trim();

		return (
			/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(guid) &&
			guid !== GUID_TURNO_VACIO &&
			guid !== GUID_TURNO_SWAGGER
		);
	};

	const esTextoTurnoValido = (value: any): boolean => {
		const texto = String(value ?? '').trim().toLowerCase();

		return (
			texto !== '' &&
			texto !== '0' &&
			texto !== 'null' &&
			texto !== 'undefined'
		);
	};

	const esFechaTurnoValida = (value: any): boolean => {
		if (!value) {
			return false;
		}

		const texto = String(value);

		if (
			texto.includes('1970-01-01') ||
			texto.includes('0001-01-01')
		) {
			return false;
		}

		const fecha = new Date(value);

		return !Number.isNaN(fecha.getTime());
	};

	const obtenerGuidTurnoSeguro = (...values: any[]): string => {
		const valorValido = values.find(esGuidTurnoValido);

		return valorValido
			? String(valorValido).trim()
			: GUID_TURNO_VACIO;
	};

	const obtenerTextoTurnoSeguro = (...values: any[]): string => {
		const valorValido = values.find(esTextoTurnoValido);

		return valorValido
			? String(valorValido).trim()
			: '0';
	};

	const obtenerFechaTurnoSegura = (...values: any[]): Date => {
		const valorValido = values.find(esFechaTurnoValida);

		return valorValido
			? new Date(valorValido)
			: new Date('1970-01-01T00:00:00.000Z');
	};

	const construirInformacionTurnoDto = (
		turnoEnData?: InformacionTurnosModel | null,
	): InformacionTurnosModel => {
		const turnoRespuesta = responseSolicitud?.informacionTurnoDto;

		return {
			tipoRecepcion: obtenerGuidTurnoSeguro(
				formulario.getValues('tipoDeRecepcion'),
				turnoEnData?.tipoRecepcion,
				turnoRespuesta?.tipoRecepcion,
			),

			radPreTramite: obtenerTextoTurnoSeguro(
				formulario.getValues('radPreTramite'),
				turnoEnData?.radPreTramite,
				turnoRespuesta?.radPreTramite,
			),

			numeroComision: obtenerTextoTurnoSeguro(
				formulario.getValues('numeroMemorando'),
				turnoEnData?.numeroComision,
				turnoRespuesta?.numeroComision,
			),

			tipoTurno: obtenerGuidTurnoSeguro(
				formulario.getValues('tipoTurno'),
				turnoEnData?.tipoTurno,
				turnoRespuesta?.tipoTurno,
			),

			turno: obtenerTextoTurnoSeguro(
				formulario.getValues('numeroTurno'),
				turnoEnData?.turno,
				turnoRespuesta?.turno,
			),

			tipoSucursal: obtenerGuidTurnoSeguro(
				formulario.getValues('sucursal'),
				turnoEnData?.tipoSucursal,
				turnoRespuesta?.tipoSucursal,
			),

			fechaComision: obtenerFechaTurnoSegura(
				formulario.getValues('fechaMemorando'),
				turnoEnData?.fechaComision,
				turnoRespuesta?.fechaComision,
			),
		} as InformacionTurnosModel;
	};

	const debeAgregarInformacionAdjudicacionEscritura = (): boolean => {
		const subModalidad = normalizarTexto(
			responseSolicitud?.descripcionSubModalidad
		);

		const esSubsidioSecuestrados =
			responseSolicitud?.timCodigo === 'ACDDHSS';

		const esPagoConfigurado =
			subModalidad.includes('SEGUNDO PAGO') ||
			subModalidad.includes('SIMULTANEO') ||
			subModalidad.includes('(SP)') ||
			subModalidad.includes('(SIMULTANEO)');

		const esPrimerPagoTemporalSubsidioSecuestrados =
			esSubsidioSecuestrados &&
			subModalidad.includes('PRIMER PAGO');

		return (
			esPagoConfigurado ||
			esPrimerPagoTemporalSubsidioSecuestrados
		);
	};

	const agregarInformacionAdjudicacionEscritura = (
		data: SolicitudTareaModel
	): void => {
		if (!debeAgregarInformacionAdjudicacionEscritura()) {
			return;
		}

		data.informacionAdjudicacionDto = {
			tipoVivienda:
				formulario.getValues('tipoDeVivienda'),
			direccion:
				formulario.getValues('direccionIA'),
			SIA_CIUDAD:
				formulario.getValues('ciudadIA'),
			matriculaInmobiliaria:
				formulario.getValues('numeroDeMatrículaIA'),
			valorSubsidio: String(
				UtilService.cleanCurrencyValue(
					formulario.getValues(
						'valorDelSubsidioIA'
					)
				)
			),
		} as InformacionAdjudicacionModel;

		const fechaEscrituracion =
			formulario.getValues(
				'fechaDeEscrituraciónIE'
			);

		data.informacionEscrituracionDto = {
			numeroEscritura:
				formulario.getValues(
					'numeroDeEscrituraIE'
				),
			fechaEscrituracion: fechaEscrituracion
				? new Date(fechaEscrituracion)
				: null,
			notaria:
				formulario.getValues('notariaIE'),
		} as InformacionEscrituracionModel;
	};

	const guardarActualizarSolicitudTarea = async (data: SolicitudTareaModel, tipoAccion: number, opciones?: { silencioso?: boolean }) => {

		const esSilencioso = opciones?.silencioso === true;

		const tipoAccionFinal = esRadicacionDevueltaPorRechazo
			? BOTONERA_SOLICITUD.ACTUALIZAR
			: tipoAccion;



		if (datosBeneficiarios) {
			NotificationService.genericAlwarning(
				'',
				`¡No se a completado el formulario del apoderado!. `,
				'warning',
			);
			return
		}

		setIsLoading(true);
		let valoresSumados = 0;


		data.informacionTurnoDto = construirInformacionTurnoDto(
			data.informacionTurnoDto,
		);

		data.solicitudTerceroBeneficiarioDto.forEach((item: any) => {
			if (item.valorRetirarDos) {
				valoresSumados += Number(item.valorRetirarDos)
			}

		})

		let idOficina = 0;
		if (formulario.getValues('codigoOficina') === oficinaRegistro?.codigoOficina) {
			idOficina = Number(oficinaRegistro?.idOficina);
		}

		if (responseSolicitud?.timCodigo === "LIBERGRAV" ||
			responseSolicitud?.timCodigo === "APTGHGR" ||
			responseSolicitud?.timCodigo === "ADQUITER" ||
			responseSolicitud?.timCodigo === "REPARVIV" ||
			responseSolicitud?.timCodigo === "ARMVGR" ||
			responseSolicitud?.timCodigo === "CONSNUEVA" ||
			responseSolicitud?.timCodigo === "CSLGR" ||
			responseSolicitud?.timCodigo === "PAGOPREDIAL" ||
			responseSolicitud?.timCodigo === "PAGOCRHIP" ||
			responseSolicitud?.timCodigo === "CCSLSS" ||
			responseSolicitud?.timCodigo === "CONSNUEVASP" ||
			responseSolicitud?.timCodigo === "RETINST" ||
			responseSolicitud?.timCodigo === "AVAC" ||
			responseSolicitud?.timCodigo === "AVAV" ||
			responseSolicitud?.timCodigo === "DSV" ||
			responseSolicitud?.timCodigo === "DAVAV" || 
			responseSolicitud?.timCodigo === "RMI" ||
			responseSolicitud?.timCodigo === "DAVAC") {

			if (Number(data.valorRetirar) !== valoresSumados) {
				toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR })
				setIsLoading(false);
				return
			}

			if (tipoAccionFinal === BOTONERA_SOLICITUD.GUARDAR) {

				try {
					const response = await api.PostSolicitudTarea(data);

					if (response && response?.isSuccess) {
						if (!esSilencioso) {
							NotificationService.genericAlert(
								'Guardado',
								'Se ha guardado correctamente la solicitud.'
							);
						}
						setTipoAccion(BOTONERA_SOLICITUD.ACTUALIZAR);
					} else {
						toast(`Error:  ${response.returnMessage}`, { type: toast.TYPE.ERROR });
					}
					setIsLoading(false);
				} catch (e) {
					toast('Error del sistema', { type: toast.TYPE.ERROR });
					setIsLoading(false);
				}
			} else if (tipoAccionFinal === BOTONERA_SOLICITUD.ACTUALIZAR) {

				try {
					if (!esSilencioso) {
						await mostrarAlertasRiesgoTusDatosActualizacion();
					}

					const response = await api.PostActualizarSolicitudTarea(data);

					if (response && response?.isSuccess) {
						setCambiarEstadoTramite(true);

						await eliminarMatriculasPendientesEnBackend();

						if (!esSilencioso) {
							NotificationService.genericAlert(
								'Actualizado',
								'Se ha actualizado correctamente la solicitud.'
							);
						}
					} else {
						toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
					}

					setIsLoading(false);
				} catch (e) {
					toast('Error del sistema', { type: toast.TYPE.ERROR });
					setIsLoading(false);
				}
			}
			return

		}

		if (responseSolicitud?.timCodigo !== "AHOPROEDU" &&
			responseSolicitud?.timCodigo !== 'REPAES' &&
			responseSolicitud?.timCodigo !== 'READACPROES' &&
			responseSolicitud?.timCodigo !== 'DESSOLAPORT' &&
			responseSolicitud?.timCodigo !== 'SDAPOR' &&
			responseSolicitud?.timCodigo !== 'AVAC' &&
			responseSolicitud?.timCodigo !== 'AVAV' &&
			responseSolicitud?.timCodigo !== 'DSV' &&
			responseSolicitud?.timCodigo !== 'DAVAV' &&
			responseSolicitud?.timCodigo !== 'RMI' &&
			responseSolicitud?.timCodigo !== 'DAVAC') {

			const oficinaCod = matriculaInmobiliariaModel[0]?.numeroMatricula?.split('-')
			const matricula = {
				ciudadId: Number(matriculaInmobiliariaModel[0]?.ciudadId),
				direccion: matriculaInmobiliariaModel[0]?.direccion,
				numeroMatricula: matriculaInmobiliariaModel[0]?.numeroMatricula,
				esPrincipal: matriculaInmobiliariaModel[0]?.esPrincipal,
				ciudad: { id: Number(matriculaInmobiliariaModel[0]?.ciudadInmueble?.id), descripcion: matriculaInmobiliariaModel[0]?.ciudadInmueble?.descripcion },
				departamento: { id: Number(matriculaInmobiliariaModel[0]?.departamentoInmueble?.id), descripcion: matriculaInmobiliariaModel[0]?.departamentoInmueble?.descripcion },
				departamentoId: Number(matriculaInmobiliariaModel[0]?.departamentoId),
				oficinaCodigo: oficinaCod[0],
				oficinaRegistroId: Number(matriculaInmobiliariaModel[0]?.oficinaRegistroId)

			}

			const matriculas: any[] = [];
			matriculas.push(matricula);
			data.solicitudMatriculaDto = matriculas;
		}

		if (Number(data.valorRetirar) !== valoresSumados && responseSolicitud?.timCodigo !== 'SDAPOR') {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR })
			setIsLoading(false);
			return
		}


		if (tipoAccionFinal === BOTONERA_SOLICITUD.GUARDAR) {
			const esRepaes =
				modalidadSolicitud === MODALIDAD_SOLICITUD.REPAES;

			let dataToSave = data;

			if (!esRepaes) {
				const {
					solicitudTerceroEntidadEducativaDto,
					...restoData
				} = data;

				dataToSave = restoData;
			}

			try {
				const response = await api.PostSolicitudTarea(
					dataToSave,
				);

				if (response?.isSuccess) {
					if (esRepaes) {
						const entidadesNuevas =
							estadoEntidadNueva.filter(
								(entidad: any) => {
									const idEntidad = String(
										entidad?.idEntidad || '',
									).trim();

									return idsEntidadesCreadasFormulario.includes(
										idEntidad,
									);
								},
							);



						if (entidadesNuevas.length > 0) {
							await bloquearEntidadesEducativas(
								entidadesNuevas,
							);

							setIdsEntidadesCreadasFormulario(
								[],
							);
						}
					}

					if (!esSilencioso) {
						NotificationService.genericAlert(
							'Guardado',
							'Se ha guardado correctamente la solicitud.',
						);
					}

					setTipoAccion(
						BOTONERA_SOLICITUD.ACTUALIZAR,
					);
				} else {
					toast(
						`Error: ${response?.returnMessage}`,
						{ type: toast.TYPE.ERROR },
					);
				}
			} catch (error) {
				console.error(
					'Error guardando la solicitud o bloqueando las entidades:',
					error,
				);

				toast('Error del sistema', {
					type: toast.TYPE.ERROR,
				});
			} finally {
				setIsLoading(false);
			}
		} else if (
			tipoAccionFinal ===
			BOTONERA_SOLICITUD.ACTUALIZAR
		) {
			try {
				if (!esSilencioso) {
					await mostrarAlertasRiesgoTusDatosActualizacion();
				}

				const response =
					await api.PostActualizarSolicitudTarea(data);

				if (response?.isSuccess) {
					setCambiarEstadoTramite(true);

					await eliminarMatriculasPendientesEnBackend();

					if (!esSilencioso) {
						NotificationService.genericAlert(
							'Actualizado',
							'Se ha actualizado correctamente la solicitud.',
						);
					}
				} else {
					toast(
						`Error: ${response?.returnMessage}`,
						{ type: toast.TYPE.ERROR },
					);
				}

				setIsLoading(false);
			} catch (error) {
				console.error(
					'Error actualizando la solicitud:',
					error,
				);

				toast('Error del sistema', {
					type: toast.TYPE.ERROR,
				});

				setIsLoading(false);
			}
		}
	}

	const onSubmitViviendaNuevaUsada = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {


		if (matriculaInmobiliariaModel.length === 0 && formulariosSolicitud.some((formulario: string) => formulario === 'MatriculaInmobiliaria')) {
			toast('Debe registrar al menos una matrícula', { type: toast.TYPE.ERROR });
			return;
		}


		if (propietarioMatriculaModel.length === 0 && formulariosSolicitud.some((formulario: string) => formulario === 'PropietarioMatricula')) {
			toast('Debe registrar al menos un propietario', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioModel.length === 0 && formulariosSolicitud.some((formulario: string) => formulario === 'TerceroBeneficiarioPagosVu')) {
			toast('Debe registrar al menos un tercero beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		data.informacionTurnoDto = construirInformacionTurnoDto(
			data.informacionTurnoDto,
		);

		data.informacionAdjudicacionDto = {
			tipoVivienda: formulario.getValues("tipoDeVivienda"),
			direccion: formulario.getValues("direccionIA"),
			SIA_CIUDAD: formulario.getValues("ciudadIA"),
			matriculaInmobiliaria: formulario.getValues("numeroDeMatrículaIA"),
			valorSubsidio: String(UtilService.cleanCurrencyValue(formulario.getValues("valorDelSubsidioIA"))),
		} as InformacionAdjudicacionModel;

		data.informacionEscrituracionDto = {
			numeroEscritura: formulario.getValues("numeroDeEscrituraIE"),
			fechaEscrituracion: new Date(formulario.getValues("fechaDeEscrituraciónIE")),
			notaria: formulario.getValues("notariaIE"),
		} as InformacionEscrituracionModel;

		data.solicitudMatriculaDto = matriculaInmobiliariaModel;
		data.solicitudPropietarioDto = propietarioMatriculaModel;

		for (let i = 0; i < data.solicitudTerceroApoderadoDto.length; i += 1) {
			for (let j = 0; j < data.solicitudPropietarioDto.length; j += 1) {
				if (data.solicitudTerceroApoderadoDto[j]?.numDoc === data.solicitudPropietarioDto[i]?.numeroIdentificacion) {
					if (data.solicitudTerceroApoderadoDto[j]?.razonSocial !== data.solicitudPropietarioDto[i]?.razonSocial) {
						toast(`si el apoderado es igual al propietario, debe digitar el mismo nombre `, { type: toast.TYPE.ERROR });
					}
				}
			}
		}

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);

		if ((Number(data.valorRetirar) === sumWithInitial)) {

			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		} else {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR })
		}

	}

	const onSubmitConstReparacion = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (matriculaInmobiliariaModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'MatriculaInmobiliaria' || formulario === 'InformacionInmueble')) {
			toast('Debe registrar al menos una matrícula', { type: toast.TYPE.ERROR });
			return;
		}

		if (propietarioMatriculaModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'PropietarioMatricula')) {
			toast('Debe registrar al menos un propietario', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'TerceroBeneficiarioPagosVu' || formulario === 'TerceroBeneficiario')) {
			toast('Debe registrar al menos un tercero beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		data.solicitudMatriculaDto = matriculaInmobiliariaModel;;
		data.solicitudPropietarioDto = propietarioMatriculaModel;

		agregarInformacionAdjudicacionEscritura(data);

		// if (responseSolicitud?.descripcionSubModalidad.includes("SEGUNDO PAGO") || responseSolicitud?.descripcionSubModalidad.includes("SIMULTÁNEO") ||
		// 	responseSolicitud?.descripcionSubModalidad.includes("(SP)") || responseSolicitud?.descripcionSubModalidad.includes("(SIMULTÁNEO)")) {
		// 	data.informacionAdjudicacionDto = {
		// 		tipoVivienda: formulario.getValues("tipoDeVivienda"),
		// 		direccion: formulario.getValues("direccionIA"),
		// 		SIA_CIUDAD: formulario.getValues("ciudadIA"),
		// 		matriculaInmobiliaria: formulario.getValues("numeroDeMatrículaIA"),
		// 		valorSubsidio: String(UtilService.cleanCurrencyValue(formulario.getValues("valorDelSubsidioIA"))),
		// 	} as InformacionAdjudicacionModel;

		// 	data.informacionEscrituracionDto = {
		// 		numeroEscritura: formulario.getValues("numeroDeEscrituraIE"),
		// 		fechaEscrituracion: new Date(formulario.getValues("fechaDeEscrituraciónIE")),
		// 		notaria: formulario.getValues("notariaIE"),
		// 	} as InformacionEscrituracionModel;
		// }

		data.informacionTurnoDto = construirInformacionTurnoDto(
			data.informacionTurnoDto,
		);

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);

		if (sumWithInitial !== Number(data.valorRetirar)) {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
			return;
		}

		const noConyuge = matriculaInmobiliariaModel.some((x: any) => x.conyugeActivo === false)

		if (noConyuge && (responseSolicitud?.timCodigo === "LIBERGRAV" ||
			responseSolicitud?.timCodigo === "APTGHGR" ||
			responseSolicitud?.timCodigo === "ADQUITER" ||
			responseSolicitud?.timCodigo === "REPARVIV" ||
			responseSolicitud?.timCodigo === "ARMVGR" ||
			responseSolicitud?.timCodigo === "CONSNUEVA" ||
			responseSolicitud?.timCodigo === "CSLGR" ||
			responseSolicitud?.timCodigo === "PAGOPREDIAL" ||
			responseSolicitud?.timCodigo === "PAGOCRHIP" ||
			responseSolicitud?.timCodigo === "CCSLSS" ||
			responseSolicitud?.timCodigo === "CONSNUEVASP")) {

			const nombres = responseSolicitud?.nombresCliente.split(' ')
			const apellidos = responseSolicitud?.apellidosCliente.split(' ')

			data.solicitudPropietarioDto = [{
				numeroIdentificacion: responseSolicitud?.identificacionCliente,

				primerApellido: apellidos[0],
				segundoApellido: apellidos[1],
				primerNombre: nombres[0],
				segundoNombre: nombres[1],
				tipoIdentificacion:
				{
					id: responseSolicitud.tipoIdentificacion,
					descripcion: 'CEDULA CIUDADANIA'
				},
				matricula: [
					{
						id: '',
						numeroMatricula: matriculaInmobiliariaModel[0]?.numeroMatricula,
						estado: ''
					}]

			}] as unknown as PropietarioMatriculaModel[];
		}

		if ((ID_SUBMODALIDAD_SEGUNDO_PAGO.some(item => item === responseSolicitud?.tipoSubModalidadId))) {
			if ((Number(data.valorRetirar) === sumWithInitial)) {
				await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
			} else {
				toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR })
			}
		}

		if ((Number(data.valorRetirar) <= Number(responseSolicitud?.valorTotal)) && (Number(data.valorRetirar) === sumWithInitial)) {
			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		}
	}

	const bloquearEntidadesEducativas = async (
		entidades: any[],
	): Promise<void> => {
		if (!Array.isArray(entidades) || entidades.length === 0) {
			return;
		}

		if (!user?.USR_ID) {
			console.error(
				'No fue posible ejecutar el bloqueo preventivo: no se encontró el funcionario.',
			);

			return;
		}

		try {
			const responseCatalogo =
				await apiCatalogo.getValoresCatalogoPorNombre(
					'BLOQUEO',
				);

			let valoresCatalogo: VALORCATALOGO[] = [];

			if (Array.isArray(responseCatalogo)) {
				valoresCatalogo = responseCatalogo;
			} else if (
				Array.isArray(responseCatalogo?.data)
			) {
				valoresCatalogo = responseCatalogo.data;
			}

			const causalControlPreventivo =
				valoresCatalogo.find(
					(item: VALORCATALOGO) =>
						normalizarTexto(item?.codigo) ===
						CODIGO_CONTROL_PREVENTIVO &&
						item?.esActivo !== false,
				);

			if (!causalControlPreventivo?.idValor) {
				console.error(
					'No se encontró la causal Control preventivo en el catálogo BLOQUEO.',
				);

				return;
			}

			const idsEntidades: string[] = [];

			entidades.forEach((entidad: any) => {
				const idEntidad = String(
					entidad?.idEntidad || '',
				).trim();

				if (
					idEntidad &&
					!idsEntidades.includes(idEntidad)
				) {
					idsEntidades.push(idEntidad);
				}
			});

			const operacionesBloqueo =
				idsEntidades.map(
					(idEntidad: string) => {
						const payloadBloqueo = {
							idEntidad,
							funcionarioBloqueo:
								user.USR_ID,
							idCausalBloqueo:
								causalControlPreventivo.idValor,
							observaciones:
								OBSERVACION_CONTROL_PREVENTIVO,
						};

						return apiEntidadesEducativas
							.lockEntidades(
								payloadBloqueo as any,
							);
					},
				);

			const resultados =
				await Promise.allSettled(
					operacionesBloqueo,
				);

			resultados.forEach(
				(resultado, index) => {
					if (
						resultado.status ===
						'rejected'
					) {
						console.error(
							'No fue posible registrar el bloqueo preventivo.',
							{
								idEntidad:
									idsEntidades[index],
								error:
									resultado.reason,
							},
						);
					}
				},
			);
		} catch (error) {
			console.error(
				'Error ejecutando los bloqueos preventivos de las entidades educativas:',
				error,
			);
		}
	};

	const onSubmitRetiroParcial = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (estadoEntidadNueva.length === 0 && formulariosSolicitud.length && formulariosSolicitud?.some((formulario: string) => formulario === 'EntidadEducativa')) {
			toast('Debe registrar al menos un entidad educativa', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioEstudioModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud?.some((formulario: string) => formulario === 'TerceroBeneficiarioEstudio')) {
			toast('Debe registrar al menos un beneficiario de estudio', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'TerceroBeneficiarioPagosVu' || formulario === 'TerceroBeneficiario')) {
			toast('Debe registrar al menos un tercero beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		// Validación Retiro parcial para educación
		if (responseSolicitud?.descripcionProceso === "RETIRO PARCIAL DE CESANTÍAS" &&
			modalidadSolicitud === MODALIDAD_SOLICITUD.REPAES &&
			valorReciboTotal < validarRetiro) {
			toast('Error: El Total de Valor Recibo de Entidad Educativa debe ser mayor o igual al Valor de la Solicitud',
				{ type: toast.TYPE.ERROR })
			return;
		}

		const entidadesEducativas = estadoEntidadNueva.map(entidad => {
			if (entidad) {
				// const terceroEntidad = {
				// 	nit: entidad.numDoc,
				// 	fechalimite: entidad?.fechaLimitePago,
				// 	idEntidad: entidad?.programaentidad,
				// 	idNivel: entidad?.nivelEducativo,
				// 	razonSocial: entidad?.razonSocial,
				// 	reciboPago: entidad?.numReciboPago,
				// 	departamento: entidad?.departamento,
				// 	ciudad: entidad?.ciudad,
				// 	modalidad_estudio: entidad?.modalidadEstudio,
				// 	periodoAcademico: entidad?.periodoAcademico,
				// 	horas: entidad?.horas,
				// 	Fecha_de_inicio: entidad?.Fecha_de_inicio,
				// 	Fecha_fin: entidad?.Fecha_fin,
				// 	acreditada: entidad?.acreditada,
				// 	valorRecibo: UtilService.cleanCurrencyValue(entidad?.valorRecibo),
				// } as unknown as TerceroEntidadEducativaModel;
				const terceroEntidad = {
					nit: entidad?.numDoc,
					fechalimite: entidad?.fechaLimitePago,

					/*
						Este campo se llama idEntidad en el DTO, pero el backend
						lo utiliza como PGN_ID: identificador de programa-entidad.
					*/
					idEntidad: entidad?.programaentidad,

					idNivel: entidad?.nivelEducativo,

					razonSocial: entidad?.razonSocial,
					programaEducativo: entidad?.nombrePrograma,
					nivelEducativo: entidad?.nombreNivelEducativo,

					reciboPago: entidad?.numReciboPago,
					departamento: entidad?.departamento,
					ciudad: entidad?.ciudad,
					modalidad_estudio: entidad?.modalidad_estudio,
					periodoAcademico: entidad?.periodoAcademico,
					horas: entidad?.horas,
					Fecha_de_inicio: entidad?.Fecha_de_inicio,
					Fecha_fin: entidad?.Fecha_fin,
					acreditada: entidad?.acreditada,
					valorRecibo: UtilService.cleanCurrencyValue(
						entidad?.valorRecibo
					),
				} as unknown as TerceroEntidadEducativaModel;
				return terceroEntidad;
			}
			return null;
		})

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);
		const valorCesantiasFormato = UtilService.formatCurrency(Number(responseSolicitud?.valorTotal));
		if (Number(data.valorRetirar) > Number(responseSolicitud?.valorTotal)) {
			toast(`Error: El valor de la solicitud supera al valor de la cuenta ${valorCesantiasFormato}`, { type: toast.TYPE.ERROR });
		} else if (formulario.getValues('fechaLimitePago') < selectedMinDate() && responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.REPAES) {
			toast(`Error: La fecha limite de pago debe ser mayor a 5 días`, { type: toast.TYPE.ERROR });
		} else if (Number(data.valorRetirar) !== sumWithInitial) {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
		} else if (responseSolicitud?.timCodigo === 'REPAES') {
			data.solicitudTerceroEntidadEducativaDto = entidadesEducativas as TerceroEntidadEducativaModel[];
			data.solicitudTerceroBeneficiarioEstudioDto = terceroBeneficiarioEstudioModel;

			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		} else {
			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		}
	}

	const onSubmitAhorroProgamado = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (entidadAhorroProgramadoSeguroEducativo.length === 0) {
			toast('Debe registrar al menos una entidad de ahorro programado o seguro educativo', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioEstudioModel.length === 0) {
			toast('Debe registrar al menos un tercero beneficiario de estudio', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioModel.length === 0) {
			toast('Debe registrar al menos un tercero beneficiario de pago', { type: toast.TYPE.ERROR });
			return;
		}

		const terceroEntidadSeguro = {
			tipoDocId: entidadAhorroProgramadoSeguroEducativo[0].tipoIdentificacion,
			numDoc: entidadAhorroProgramadoSeguroEducativo[0].numeroIdentificacion,
			razonSocialAseguradora: entidadAhorroProgramadoSeguroEducativo[0].razonSocial,
			entidadSeguroEducativoId: entidadAhorroProgramadoSeguroEducativo[0].idEntidadSeguroEducativo,
			numeroPoliza: entidadAhorroProgramadoSeguroEducativo[0].numeroPoliza
		} as any;

		data.solicitudTerceroBeneficiarioEstudioDto = terceroBeneficiarioEstudioModel;
		data.solicitudTerceroEntidadSeguroEducativoDto = terceroEntidadSeguro;

		await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
	}

	const onSubmitLeasing = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (matriculaInmobiliariaModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'MatriculaInmobiliaria' || formulario === 'InformacionInmueble')) {
			toast('Debe registrar al menos una matricula inmobiliaria', { type: toast.TYPE.ERROR });
			return;
		}

		if (propietarioMatriculaModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'PropietarioMatricula')) {
			toast('Debe registrar al menos un propietario', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'TerceroBeneficiarioPagosVu' || formulario === 'TerceroBeneficiario')) {
			toast('Debe registrar al menos un tercero beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);
		const valorCesantiasFormato = UtilService.formatCurrency(Number(responseSolicitud?.valorTotal));
		if (Number(data.valorRetirar) > Number(responseSolicitud?.valorTotal)) {
			toast(`Error: El valor de la solicitud supera al valor de la cuenta ${valorCesantiasFormato}`, { type: toast.TYPE.ERROR });
		} else if (Number(data.valorRetirar) !== sumWithInitial) {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
		} else {

			let idOficina = 0;
			if (formulario.getValues('codigoOficina') === oficinaRegistro?.codigoOficina) {
				idOficina = Number(oficinaRegistro?.idOficina);
			}

			const matricula = {
				ciudadId: Number(formulario.getValues("ciudad")),
				direccion: formulario.getValues("direccion"),
				numeroMatricula: `${formulario.getValues('codigoOficina')}-${formulario.getValues('numeroMatricula')}`,
				esPrincipal: true,
				ciudad: { id: Number(formulario.getValues('ciudad')), descripcion: "" },
				departamento: { id: Number(formulario.getValues('departamento')), descripcion: "" },
				departamentoId: Number(formulario.getValues('departamento')),
				oficinaCodigo: `${formulario.getValues('codigoOficina')}`,
				oficinaRegistroId: Number(idOficina)
			} as MatriculaInmobiliariaModel;
			const matriculas: MatriculaInmobiliariaModel[] = [];
			matriculas.push(matricula);

			if (modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP) {
				data.solicitudMatriculaDto = matriculas;
				data.solicitudPropietarioDto = propietarioMatriculaModel;
			} else {

				data.solicitudMatriculaDto = matriculaInmobiliariaModel;
				data.solicitudPropietarioDto = propietarioMatriculaModel;
			}
			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		}
	}

	const onSubmitAdquirirAcciones = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		const terceroTenedor = {
			tipoDoc: formulario.getValues('tipoDoc'),
			numDoc: formulario.getValues('numDoc'),
			razonSocial: formulario.getValues('razonSocial'),
			direccion: formulario.getValues('direccion'),
			ciudad: formulario.getValues('ciudad'),
			correo: formulario.getValues('correo'),
			telefono: formulario.getValues('telefono'),
			emisor: formulario.getValues('emisor'),
			departamento: formulario.getValues('departamento')
		} as TerceroTenedorAccionesModel;

		if (!terceroTenedor?.tipoDoc || !terceroTenedor?.numDoc ||
			!terceroTenedor?.razonSocial || !terceroTenedor?.correo ||
			!terceroTenedor?.telefono) {
			toast('Debe registrar la información completa de Emisores de Acciones', { type: toast.TYPE.ERROR });
			return;
		}

		data.solicitudTerceroTenedorAccionesDto = terceroTenedor;
		await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
	}

	const onSubmitAdquisicionTerreno = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		const matricula = {
			ciudadId: Number(formulario.getValues('ciudad')),
			direccion: formulario.getValues('direccion'),
			numeroMatricula: `${formulario.getValues('codigoOficina')}-${formulario.getValues('numeroMatricula')}`,
			esPrincipal: true,
			ciudad: { id: Number(formulario.getValues('ciudad')), descripcion: '' },
			departamento: { id: Number(formulario.getValues('departamento')), descripcion: '' }
		} as MatriculaInmobiliariaModel;
		const matriculas: MatriculaInmobiliariaModel[] = [];
		matriculas.push(matricula);

		if (terceroVendedorModel.length === 0) {
			toast('Debe registrar al menos un tercero vendedor', { type: toast.TYPE.ERROR });
			return;
		}

		data.solicitudTerceroVendedorDto = terceroVendedorModel;
		data.solicitudMatriculaDto = matriculas;
		await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);

	}

	const onSubmitPagoCuotaInicialCH = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (terceroBeneficiarioModel.length === 0) {
			toast('Debe registrar al menos un tercero beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);
		const valorCesantiasFormato = UtilService.formatCurrency(Number(responseSolicitud?.valorTotal));
		if (Number(data.valorRetirar) > Number(responseSolicitud?.valorTotal)) {
			toast(`Error: El valor de la solicitud supera al valor de la cuenta ${valorCesantiasFormato}`, { type: toast.TYPE.ERROR });
		} else if (Number(data.valorRetirar) !== sumWithInitial) {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
		} else {

			// if (responseSolicitud?.descripcionSubModalidad.includes("SEGUNDO PAGO") || responseSolicitud?.descripcionSubModalidad.includes("SIMULTÁNEO") ||
			// 	responseSolicitud?.descripcionSubModalidad.includes("(SP)") || responseSolicitud?.descripcionSubModalidad.includes("(SIMULTÁNEO)")) {
			// 	data.informacionAdjudicacionDto = {
			// 		tipoVivienda: formulario.getValues("tipoDeVivienda"),
			// 		direccion: formulario.getValues("direccionIA"),
			// 		SIA_CIUDAD: formulario.getValues("ciudadIA"),
			// 		matriculaInmobiliaria: formulario.getValues("numeroDeMatrículaIA"),
			// 		valorSubsidio: String(UtilService.cleanCurrencyValue(formulario.getValues("valorDelSubsidioIA"))),
			// 	} as InformacionAdjudicacionModel;

			// 	data.informacionEscrituracionDto = {
			// 		numeroEscritura: formulario.getValues("numeroDeEscrituraIE"),
			// 		fechaEscrituracion: new Date(formulario.getValues("fechaDeEscrituraciónIE")),
			// 		notaria: formulario.getValues("notariaIE"),
			// 	} as InformacionEscrituracionModel;
			// }
			agregarInformacionAdjudicacionEscritura(data);

			if (maticulaUno) {
				if ('137' === "137") {

					const cat = catalogState?.IdentificationType;
					let vartip = "";
					let vartipid = null;
					const td = formulario.getValues("tipoDocumentoConyugue");
					const valorSinCeros = parseInt(td, 10).toString();

					if (cat) {
						const itemEncontrado = cat.find(item => item.id === Number(valorSinCeros));
						if (itemEncontrado) {
							vartip = itemEncontrado.descripcion;
							vartipid = itemEncontrado.id;
						}
					}

					if (responseSolicitud?.timCodigo === "PAGOCRHIP") {
						const nombres = responseSolicitud?.nombresCliente.split(' ')
						const apellidos = responseSolicitud?.apellidosCliente.split(' ')

						data.solicitudPropietarioDto = [{
							numeroIdentificacion: responseSolicitud?.identificacionCliente,

							primerApellido: apellidos[0],
							segundoApellido: apellidos[1],
							primerNombre: nombres[0],
							segundoNombre: nombres[1],
							tipoIdentificacion:
							{
								id: responseSolicitud.tipoIdentificacion,
								descripcion: 'CEDULA CIUDADANIA'
							},
							matricula: [
								{
									id: '',
									numeroMatricula: matriculaInmobiliariaModel[0]?.numeroMatricula,
									estado: ''
								}]

						}] as unknown as PropietarioMatriculaModel[];

					}

					const numeroMatricula =
						formulario.getValues('numeroMatricula') ||
						(data.solicitudPropietarioDto?.[0]?.matricula?.[0]?.numeroMatricula) ||
						'';
					const matricula = {
						ciudadId: matriculaInmobiliariaModel[0]?.ciudadId,
						direccion: matriculaInmobiliariaModel[0]?.direccion,
						numeroMatricula: formulario.getValues('codigoOficina') && numeroMatricula
							? `${formulario.getValues('codigoOficina')}-${numeroMatricula}`
							: numeroMatricula,
						esPrincipal: true,
						ciudad: { id: matriculaInmobiliariaModel[0]?.ciudadId, descripcion: matriculaInmobiliariaModel[0]?.ciudadInmueble?.descripcion },
						departamento: { id: matriculaInmobiliariaModel[0]?.departamentoId, descripcion: matriculaInmobiliariaModel[0]?.departamentoInmueble?.descripcion },
						departamentoId: matriculaInmobiliariaModel[0]?.departamentoId,
						oficinaCodigo: numeroMatricula.split('-')[0],
						oficinaRegistroId: matriculaInmobiliariaModel[0]?.oficinaRegistroId
					} as MatriculaInmobiliariaModel;
					const matriculas: MatriculaInmobiliariaModel[] = [];
					matriculas.push(matricula);
					data.solicitudMatriculaDto = matriculas;

				} else if ("87" === "87") {

					const apellidos = responseSolicitud?.apellidosCliente ? responseSolicitud.apellidosCliente.split(" ") : ["", ""];
					const nombres = responseSolicitud?.nombresCliente ? responseSolicitud?.nombresCliente.split(" ") : ["", ""];
					const cat = catalogState?.IdentificationType;
					let vartip = "";
					let vartipid = null;

					if (cat) {
						const itemEncontrado = cat.find(item => item.id === Number(responseSolicitud?.tipoIdentificacion));
						if (itemEncontrado) {
							vartip = itemEncontrado.descripcion;
							vartipid = itemEncontrado.id;
						}
					}

					data.solicitudPropietarioDto = [{
						numeroIdentificacion: responseSolicitud?.identificacionCliente,
						razonSocial: `${responseSolicitud?.nombresCliente || ''} ${responseSolicitud?.apellidosCliente || ''}`,
						primerApellido: apellidos[0] || "",
						segundoApellido: apellidos[1] || "",
						primerNombre: nombres[0] || "",
						segundoNombre: nombres[1] || "",
						tipoIdentificacion:
						{
							id: vartipid,
							descripcion: vartip
						}
						,
						matricula: [
							{
								id: '',
								numeroMatricula: matriculaInmobiliariaModel[0]?.numeroMatricula,
								estado: ''
							}
						]
					}] as PropietarioMatriculaModel[];

					let idOficina = 0;
					if (formulario.getValues('codigoOficina') === oficinaRegistro?.codigoOficina) {
						idOficina = Number(oficinaRegistro?.idOficina);
					}

					const oficinaCod = matriculaInmobiliariaModel[0]?.numeroMatricula?.split('-')

					const matricula = {
						ciudadId: Number(matriculaInmobiliariaModel[0]?.ciudadId),
						direccion: matriculaInmobiliariaModel[0]?.direccion,
						numeroMatricula: matriculaInmobiliariaModel[0]?.numeroMatricula,
						esPrincipal: matriculaInmobiliariaModel[0]?.esPrincipal,
						ciudad: { id: Number(matriculaInmobiliariaModel[0]?.ciudadInmueble?.id), descripcion: matriculaInmobiliariaModel[0]?.ciudadInmueble?.descripcion },
						departamento: { id: Number(matriculaInmobiliariaModel[0]?.departamentoInmueble?.id), descripcion: matriculaInmobiliariaModel[0]?.departamentoInmueble?.descripcion },
						departamentoId: Number(matriculaInmobiliariaModel[0]?.departamentoId),
						oficinaCodigo: oficinaCod[0],
						oficinaRegistroId: Number(idOficina)

					}

					const matriculas: any[] = [];
					matriculas.push(matricula);
					data.solicitudMatriculaDto = matriculas;

				}
			} else {
				data.solicitudPropietarioDto = propietarioMatriculaModel;
				data.solicitudMatriculaDto = matriculaInmobiliariaModel;
			}

			data.solicitudTerceroBeneficiarioDto = terceroBeneficiarioModel;
			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		}

	}

	const onSubmitViviendaNuevaOcho = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (matriculaInmobiliariaModel.length === 0) {
			toast('Debe registrar al menos una matrícula', { type: toast.TYPE.ERROR });

		}

		if (propietarioMatriculaModel.length === 0 && formulariosSolicitud.length && formulariosSolicitud.some((formulario: string) => formulario === 'PropietarioMatricula')) {
			toast('Debe registrar al menos un propietario', { type: toast.TYPE.ERROR });
			return;
		}

		if (terceroBeneficiarioModel.length === 0) {
			toast('Debe registrar al menos un tercero beneficiario de pago', { type: toast.TYPE.ERROR });
			return;
		}

		data.solicitudMatriculaDto = matriculaInmobiliariaModel;
		data.solicitudPropietarioDto = propietarioMatriculaModel;
		data.solicitudTerceroApoderadoDto = terceroApoderadoModel;

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);

		if (Number(data.valorRetirar) !== sumWithInitial) {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
		} else {
			await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
		}
	}

	const onSubmitDevolucionSaldo = async (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {

		if (modalidadSolicitud === MODALIDAD_SOLICITUD.DSV) {


			if (terceroBeneficiarioModel.length === 0) {
				toast('Debe registrar al menos un tercero beneficiario de pago', { type: toast.TYPE.ERROR });
				return;
			}

			if (destinacionDinerosSeleccionado === 'transferencia' &&
				(saldoCuenta === null ||
					formulario.getValues('cuenta') === '')) {
				toast('Debe registrar la información del formulario Mis Cuentas', { type: toast.TYPE.ERROR });
				return;
			}

			if (destinacionDinerosSeleccionado === 'transferencia' &&
				aceptaTraslado === false) {
				toast('Debe aceptar el traslado para continuar',
					{ type: toast.TYPE.ERROR })
				return;
			}

			if (destinacionDinerosSeleccionado === 'transferencia') {
				const informacionCuenta: SolicitudMisCuentasDto = {
					saldoCuenta: saldoCuenta || 0,
					cuenta: formulario.getValues('cuenta'),
					trasladoAutorizado: aceptaTraslado
				}

				data.solicitudMiscuentasDto = informacionCuenta;
			}

			if (destinacionDinerosSeleccionado === 'retiro' && terceroBeneficiarioModel.length > 0) {

				const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
				const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);

				if (Number(data.valorRetirar) !== sumWithInitial) {
					toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
					return;
				}
			}

		}

		await guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
	}

	const onSubmitAvacAvav = (data: SolicitudTareaModel, opciones?: { silencioso?: boolean }) => {


		if (terceroBeneficiarioModel.length === 0) {
			toast('Debe registrar al menos un tercero beneficiario de pago', { type: toast.TYPE.ERROR });
			return;
		}

		if (responseSolicitud?.descripcionSubModalidad &&
			responseSolicitud?.descripcionSubModalidad === 'DEVOLUCIÓN DE AHORROS AVAV - MUERTE DEL AFILIADO' &&
			tercerosBeneficiarios.length === 0) {
			toast('Debe registrar al menos un beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		const arraySumatoriaValoresRetirar = data.solicitudTerceroBeneficiarioDto.map((item: any) => Number(item.valorRetirarDos));
		const sumWithInitial = arraySumatoriaValoresRetirar.reduce((previousValue, currentValue) => previousValue + currentValue, 0);

		if (Number(data.valorRetirar) !== sumWithInitial) {
			toast('Error: El valor a retirar es diferente al valor de la solicitud', { type: toast.TYPE.ERROR });
			return;
		}

		guardarActualizarSolicitudTarea(data, tipoAccionNumber, opciones);
	}

	const loadDataViviendaNuevaUsada = (data: ResponseSeveranceModel | undefined) => {

		if (data?.solicitudMatriculaDto !== null) {
			setMatriculaInmobiliariaModel(data?.solicitudMatriculaDto as MatriculaInmobiliariaModel[]);
		}
		if (data?.solicitudPropietarioDto !== null) {

			setPropietarioMatriculaModel(data?.solicitudPropietarioDto as PropietarioMatriculaModel[]);
		}
		if (data?.solicitudTerceroApoderadoDto !== null) {
			setTerceroApoderadoModel(data?.solicitudTerceroApoderadoDto as TerceroApoderadoModel[]);
		}
		registrarAfiliado(data);
	}

	const loadDataConstReparacion = (data: ResponseSeveranceModel | undefined) => {
		loadDataConyuge(data?.conyugeActivo, data);
		loadDataMatricula(data);
		loadDataTurno(data);
	}

	const loadDataConyuge = (conyugeActivo: boolean | undefined, data: ResponseSeveranceModel | undefined) => {

		if (conyugeActivo) {
			// formulario.setValue('propietario', '137');
			formulario.setValue('tipoDocumentoConyugue', data?.idTipoIdentificacionConyuge + 1?.toString() as string);
			formulario.setValue('numeroDocumentoConyugue', data?.identificacionConyuge as string);
			formulario.setValue('primerNombreConyuge', `${data?.primerNombreConyuge || ''}`);
			formulario.setValue('segundoNombreConyuge', `${data?.segundoNombreConyuge || ''}`);
			formulario.setValue('primerApellidoConyuge', `${data?.primerApellidoConyuge || ''}`);
			formulario.setValue('segundoApellidoConyuge', `${data?.segundoApellidoConyuge || ''}`);
		} else {
			// formulario.setValue('propietario', '87');
			limpiarCamposConyugue();
		}
	}


	const loadDataTurno = (data: ResponseSeveranceModel | undefined) => {
		const turno = data?.informacionTurnoDto;

		if (!turno) {
			return;
		}

		formulario.setValue(
			'tipoDeRecepcion',
			String(turno.tipoRecepcion ?? ''),
		);

		formulario.setValue(
			'radPreTramite',
			String(turno.radPreTramite ?? ''),
		);

		formulario.setValue(
			'numeroMemorando',
			String(turno.numeroComision ?? ''),
		);

		formulario.setValue(
			'tipoTurno',
			String(turno.tipoTurno ?? ''),
		);

		formulario.setValue(
			'numeroTurno',
			String(turno.turno ?? ''),
		);

		formulario.setValue(
			'sucursal',
			String(turno.tipoSucursal ?? ''),
		);

		const fechaMemorando =
			turno.fechaComision &&
				String(turno.fechaComision) !== '1970-01-01T00:00:00'
				? new Date(turno.fechaComision)
				: null;

		formulario.setValue('fechaMemorando', fechaMemorando);
	};

	const loadDataAdquisicionTerreno = (data: ResponseSeveranceModel | undefined) => {
		if (data?.solicitudTerceroVendedorDto !== null) {
			setTerceroVendedorModel(data?.solicitudTerceroVendedorDto as TerceroVendedorModel[]);
		}
		loadDataMatricula(data);
	}

	const loadCatalogInfo = () => {
		dispatch(startGetClientByIdentificationTypeIdentificationNumber(loginClaims,Number(responseSolicitud?.tipoIdentificacion) || 0,responseSolicitud?.identificacionCliente || ''));
		dispatch(startGetAllTypeDocuments());
		if (modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CNVGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVUGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVNSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVUSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PACRECAHO ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CSLGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.REPARVIV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ARMVGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.LIBERGRAV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.APTGHGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ADQUITER ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOPREDIAL ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.READACPROES ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CESOCLEAS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.RETINST ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DESSOLAPORT ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DESCONCESANT ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.REPAES ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVASP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CCSLSS ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOCRHIP) {
			dispatch(startGetAllDepartments());
			dispatch(startGetAllCities());
			dispatch(startGetRegistrationOffice());
			dispatch(startGetAllEntidadBancaria());
		}
		if (modalidadSolicitud === MODALIDAD_SOLICITUD.AHOPROEDU) {
			dispatch(startGetAllEntidadSeguroEducativo());
			dispatch(startGetAllEntidadBancaria());
		}
		if (modalidadSolicitud === MODALIDAD_SOLICITUD.BENEFALL ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.MECANES ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.REDEFIN) {
			dispatch(startGetAllDepartments());
			dispatch(startGetAllTypesForces());
			dispatch(startGetCategory());
			dispatch(startGetPhoneTypes());
			dispatch(startGetAllGrades(loginClaims));
		}
		if (modalidadSolicitud === MODALIDAD_SOLICITUD.AVAC ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.AVAV) {
			dispatch(startGetAllEntidadBancaria());
			obtenerParentesco(3);
		}
		if (modalidadSolicitud === MODALIDAD_SOLICITUD.DSV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DAVAV || 
			modalidadSolicitud === MODALIDAD_SOLICITUD.RMI || 
			modalidadSolicitud === MODALIDAD_SOLICITUD.DAVAC) {
			dispatch(startGetAllEntidadBancaria());
			obtenerParentesco(3);
		}
		dispatch(startGetAllTipoCuenta());
		dispatch(startGetAllEntidadEducativa());
	}

	const loadDataValorDesembolsoCredito = (data: ResponseSeveranceModel | undefined) => {
		if (data?.valorRetirar == null) {
			setMatriculaInmobiliariaModel(data?.solicitudMatriculaDto as MatriculaInmobiliariaModel[]);
		}
	}


	const loadDataDevolucionSaldo = (data: ResponseSeveranceModel | undefined) => {

		if (modalidadSolicitud === MODALIDAD_SOLICITUD.DSV) {

			if (data?.solicitudMiscuentasDto) {
				setDestinacionDinerosSeleccionado('transferencia');
				formulario.setValue('destinacionDinero', 'transferencia');
				formulario.setValue('saldoCuenta', String(data?.solicitudMiscuentasDto?.saldoCuenta));
				formulario.setValue('cuenta', data?.solicitudMiscuentasDto?.cuenta);
				setAceptaTraslado(true);
				setNombreCuenta(data?.solicitudMiscuentasDto?.cuenta);
				setSaldoCuenta(data?.solicitudMiscuentasDto?.saldoCuenta);
			} else if (data?.solicitudTerceroBeneficiarioDto) {
				setDestinacionDinerosSeleccionado('retiro');
				formulario.setValue('destinacionDinero', 'retiro');
				setAceptaTraslado(false);
				setNombreCuenta('');
				setSaldoCuenta(null);
			}

		}

	}

	const obtenerCiudadPorId = (stateId?: string) => dispatch(startGetAllCities(stateId));

	const obtenerCiudadPorVendedorId = (stateId?: string) => dispatch(startGetAllCitiesVendors(stateId));

	const obtenerParentesco = (catalogoId: number) => dispatch(startGetAllCatalogoParentesco(catalogoId));

	const obtenerTipoCuenta = (catalogoId: number) => dispatch(startGetAllCatalogoPorId(catalogoId));

	const obtenerEstadosCiviles = (civilId: string) => dispatch(startGetAllCivilState(civilId));

	const obtenerValorDesembolsoCredito = (numeroDocumento: string) => dispatch(startObtenerValorDesembolsoCredito(numeroDocumento));

	const loadDataMatricula = (data: ResponseSeveranceModel | undefined) => {
		if (
			data?.solicitudMatriculaDto !== null &&
			data?.solicitudMatriculaDto[0] !== undefined
		) {
			const idDepartamento =
				data?.solicitudMatriculaDto[0].departamento.id.toString();

			const requiereCatalogoCompletoCiudades = formsList?.some(
				(formulario: string) =>
					formulario === FORMS_NAMES.MATRICULA_INMOBILARIA ||
					formulario === FORMS_NAMES.INFORMACION_INMUEBLE,
			);

			if (!requiereCatalogoCompletoCiudades) {
				obtenerCiudadPorId(idDepartamento);
			}

			formulario.setValue(
				'numeroMatricula',
				data?.solicitudMatriculaDto[0].numeroMatricula as string,
			);

			formulario.setValue(
				'direccion',
				data?.solicitudMatriculaDto[0].direccion as string,
			);

			formulario.setValue('departamento', idDepartamento);

			formulario.setValue(
				'ciudad',
				data?.solicitudMatriculaDto[0].ciudad.descripcion.toString(),
			);

			formulario.setValue(
				'solicitudMatricula',
				data?.solicitudMatriculaDto[0],
			);
		}
	};

	const registrarAfiliado = (data: ResponseSeveranceModel | undefined) => {
		const nombres = data?.nombresCliente?.split(' ') || [];
		const apellidos = data?.apellidosCliente?.split(' ') || [];
		const nuevoBeneficiario: BeneficiarioModel = {
			tipo: 'Afiliado',
			tipoDoc: parseInt(data?.tipoIdentificacion || '1', 10),
			numDoc: data?.identificacionCliente || '',
			razonSocial: '',
			esAutorizado: true,
			parentesco: 'EL MISMO AFILIADO',
			primerNombre: nombres.length > 0 ? nombres[0] : '',
			segundoNombre: nombres.length > 1 ? nombres[1] : '',
			primerApellido: apellidos.length > 0 ? apellidos[0] : '',
			segundoApellido: apellidos.length > 1 ? apellidos[1] : '',
		};

		setBeneficiarioAfiliado([nuevoBeneficiario]);
	}


	const listaBeneficiarios = async () => {

		let array: BeneficiarioModel[]
		if (modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CVUGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DESSOLAPORT ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CNVGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ADQUITER ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVA ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.CSLGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.REPARVIV ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.ARMVGR ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOPREDIAL ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOCRHIP ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.DESCONCESANT ||
			modalidadSolicitud === MODALIDAD_SOLICITUD.RETINST) {
			array = [...beneficiarioAfiliado];
		} else if (modalidadSolicitud === MODALIDAD_SOLICITUD.AHOPROEDU) {
			array = [...entidadesFinancieras];
		} else {
			const datosPropietarios: BeneficiarioModel[] = [];
			const datosApoderados: BeneficiarioModel[] = [];
			if (propietarioMatriculaModel !== null) {

				await (propietarioMatriculaModel || []).forEach(item => {
					const nuevoBeneficiario: BeneficiarioModel = {
						tipo: 'Propietario',
						tipoDoc: item.tipoIdentificacion?.id,
						numDoc: item.numeroIdentificacion,
						razonSocial: item?.razonSocial,
						esAutorizado: false,
						parentesco: '',
						primerNombre: item?.primerNombre,
						segundoNombre: item?.segundoNombre,
						primerApellido: item?.primerApellido,
						segundoApellido: item?.segundoApellido,
						matricula: item?.matricula,
					};
					datosPropietarios.push(nuevoBeneficiario);
				});
			}
			if (terceroApoderadoModel !== null) {
				await (terceroApoderadoModel || []).forEach(item => {
					const nuevoBeneficiario: BeneficiarioModel = {
						tipo: 'Apoderado',
						tipoDoc: item.tipoDoc,
						numDoc: item.numDoc,
						razonSocial: item?.razonSocial,
						esAutorizado: item.esAutorizado,
						parentesco: item.parentescoObj?.descripcion,
						primerNombre: item?.primerNombre,
						segundoNombre: item?.segundoNombre,
						primerApellido: item?.primerApellido,
						segundoApellido: item?.segundoApellido,
					};
					datosApoderados.push(nuevoBeneficiario);
				});
			}
			array = [...beneficiarioAfiliado, ...datosPropietarios, ...datosApoderados];
		}

		setBeneficiarios(array);
	}
	const [cambiarEstadoTramite, setCambiarEstadoTramite] = useState(false);

	const obtenerEstadoMatriculaActual = (matricula: any) =>
		matricula?.estado ??
		(matricula?.esPrincipal ? 'Principal' : 'Relacionado');

	const validarMatriculasPropietariosActualizadas = () => {
		const inconsistencias =
			propietarioMatriculaModel?.flatMap((propietario: any) =>
				(propietario?.matricula || [])
					.map((matriculaGuardada: any) => {
						const matriculaActual = matriculaInmobiliariaModel.find(
							(matricula: any) =>
								String(matricula?.id) === String(matriculaGuardada?.id),
						);

						if (!matriculaActual) {
							return {
								propietario,
								matriculaGuardada,
								motivo: 'NO_EXISTE',
							};
						}

						const numeroGuardado = normalizarTexto(
							matriculaGuardada?.numeroMatricula,
						);

						const numeroActual = normalizarTexto(
							matriculaActual?.numeroMatricula,
						);

						const estadoGuardado = normalizarTexto(
							matriculaGuardada?.estado,
						);

						const estadoActual = normalizarTexto(
							obtenerEstadoMatriculaActual(matriculaActual),
						);

						if (
							numeroGuardado !== numeroActual ||
							estadoGuardado !== estadoActual
						) {
							return {
								propietario,
								matriculaGuardada,
								matriculaActual,
								motivo: 'DESACTUALIZADA',
							};
						}

						return null;
					})
					.filter(Boolean),
			) || [];

		if (inconsistencias.length > 0) {
			toast(
				'La información de propietarios tiene matrículas desactualizadas. Por favor elimine o edite el propietario asociado y seleccione nuevamente la matrícula actualizada.',
				{ type: toast.TYPE.ERROR },
			);

			return false;
		}

		return true;
	};

	const obtenerDataObservacionesActualizar = () => {
		const valoresObservacion = formularioObservacionesActivo?.getValues?.();

		const observaciones = String(
			valoresObservacion?.observaciones ||
			valoresObservacion?.observacion ||
			''
		).trim();

		return {
			idSolicitud: obtenerIdTareaSolicitudActual(),
			observaciones,
			causal: '0',
		};
	};

	const actualizacionSilenciosaRef = useRef(false);

	const ejecutarSubmitYEliminarPendientes = async (
		nombreRuta: string,
		ejecutarSubmit: () => Promise<void>,
	) => {

		await ejecutarSubmit();


		await eliminarMatriculasPendientesEnBackend();

	};

	const onSubmit = async (opciones?: { silencioso?: boolean }) => {

		const errores = obtenerErroresFormularios(true);


		if (errores.length > 0) {
			return;
		}

		actualizacionSilenciosaRef.current = !!opciones?.silencioso;

		try {
			const accionActual = esRadicacionDevueltaPorRechazo
				? BOTONERA_SOLICITUD.ACTUALIZAR
				: tipoAccionNumber;




			setCambiarEstadoTramite(false);


			if (
				formulariosSolicitud.some(
					(formulario: string) => formulario === 'PropietarioMatricula',
				) &&
				formulariosSolicitud.some(
					(formulario: string) => formulario === 'MatriculaInmobiliaria',
				)
			) {
				const matriculasActualizadas =
					validarMatriculasPropietariosActualizadas();

				if (!matriculasActualizadas) {
					return;
				}
			}

			// Validación en Vivienda 8 valor a superar del inmueble
			if (responseSolicitud?.nombreTarea.includes('V 8') &&
				formulariosSolicitud.some((formulario: string) => formulario === 'MatriculaInmobiliaria')) {

				const ID_CATEGORIA_OFICIAL = 1;
				const ID_CATEGORIA_SUBOFICIAL = 2;
				const ID_CATEGORIA_AGENTE = 3;

				if ((clientStateData?.idCategoria === ID_CATEGORIA_OFICIAL ||
					clientStateData?.idCategoria === ID_CATEGORIA_SUBOFICIAL ||
					clientStateData?.idCategoria === ID_CATEGORIA_AGENTE) &&
					matriculaInmobiliariaModel.length &&
					(matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 11 && Number(matricula?.ciudad?.id) === 149) || // Ciudad Bogotá
						matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 8 && Number(matricula?.ciudad?.id) === 126) || // Ciudad Barranquilla
						matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 68 && Number(matricula?.ciudad?.id) === 843) || // Ciudad Bucaramanga
						matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 76 && Number(matricula?.ciudad?.id) === 1003) || // Ciudad Cali
						matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 13 && Number(matricula?.ciudad?.id) === 150) || // Ciudad Cartagena
						matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 5 && Number(matricula?.ciudad?.id) === 1) || // Ciudad Medellin
						matriculaInmobiliariaModel.some((matricula: any) => Number(matricula?.departamento?.id) === 54 && Number(matricula?.ciudad?.id) === 1128)) // Ciudad Cucuta
				) {

					const request = {
						anio: 2026,
						iD_CATEGORIA: clientStateData?.idCategoria
					}

					try {
						const responseValorInmueble = await apiParametrization.PostParametrizacionValorInmueble(request);

						if (responseValorInmueble?.data.length > 0) {
							const valorInmueble = responseValorInmueble?.data.find((item: any) => item?.vlI_TIPO === 'SUPERA' && item?.estadO_DESC === 'Activo');

							if (valorInmueble && validarRetiro < valorInmueble?.vlI_VALOR) {
								toast(`Error: El valor solicitado está por debajo del valor ${UtilService.formatCurrency(valorInmueble?.vlI_VALOR)} de acuerdo a la ubicación del inmueble`,
									{ type: toast.TYPE.ERROR })
								return;
							}
						}


					} catch (e: any) {
						toast(`Error del sistema: Error al consultar valor del inmueble`,
							{ type: toast.TYPE.ERROR })
					}
				}
			}

			// Validación si debe aceptar declaraciones y autorizaciones solo si es Vivienda 8
			if (formulariosSolicitud.some((formulario: any) => formulario === 'DeclaracionesAutorizaciones')
				&& aceptarCheckValue === false) {
				toast('Debe aceptar las declaraciones y autorizaciones para continuar',
					{ type: toast.TYPE.ERROR })
				return;
			}

			if (tieneFormularioPropietarioMatricula) {
				const existenPropietariosConMatriculaNoVigente =
					propietarioMatriculaModel?.some(
						(propietario: any) =>
							propietario?.matricula?.some((matriculaPropietario: any) => {
								const existeMatriculaActual = matriculaInmobiliariaModel?.some(
									(matriculaActual: any) =>
										String(matriculaActual?.id ?? matriculaActual?.maiId) ===
										String(matriculaPropietario?.id ?? matriculaPropietario?.maiId) ||
										String(
											matriculaActual?.numeroMatricula ??
											matriculaActual?.maiNumeroMatricula,
										) ===
										String(
											matriculaPropietario?.numeroMatricula ??
											matriculaPropietario?.maiNumeroMatricula,
										),
								);

								return !existeMatriculaActual;
							}),
					);

				if (existenPropietariosConMatriculaNoVigente) {
					toast(
						'Existen propietarios asociados a una matrícula que ya fue eliminada o modificada. Por favor actualice la matrícula asociada en la lista de propietarios antes de continuar.',
						{ type: toast.TYPE.ERROR },
					);

					return;
				}

				const propietariosSinMatricula = propietarioMatriculaModel.filter(
					(propietario: any) =>
						!propietario?.matricula || propietario.matricula.length === 0,
				);

				if (propietariosSinMatricula.length > 0) {
					await NotificationService.genericAlert(
						'Propietario sin matrícula',
						'Debe editar el propietario y seleccionar la nueva matrícula antes de guardar la solicitud.',
						'warning',
					);

					return;
				}
			}

			const propietariosSinMatricula = propietarioMatriculaModel.filter(
				(propietario: any) => !propietario?.matricula || propietario.matricula.length === 0
			);

			if (propietariosSinMatricula.length > 0) {
				await NotificationService.genericAlert(
					'Propietario sin matrícula',
					'Debe editar el propietario y seleccionar la nueva matrícula antes de guardar la solicitud.',
					'warning',
				);

				return;
			}

			const data: SolicitudTareaModel = modeloSolicitudData();

			const dataObservaciones = obtenerDataObservacionesActualizar();

			(data as any).idSolicitud = dataObservaciones.idSolicitud;
			(data as any).observaciones = dataObservaciones.observaciones;
			(data as any).causal = dataObservaciones.causal;

			if (modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVA ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CNVGR ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADA ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CVUGR ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADAPP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CESOCLEAS ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.EJEOPCOMP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVASP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CVNSS ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CVUSS ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.VIUSADASP) {
				await ejecutarSubmitYEliminarPendientes(
					'onSubmitViviendaNuevaUsada',
					() => onSubmitViviendaNuevaUsada(data, opciones),
				);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVA ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CSLGR ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.ADQUITER ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVASP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CCSLSS ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.REPARVIV ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.ARMVGR ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.LIBERGRAV ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.APTGHGR ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOPREDIAL ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.ACDDHSS) {
				await onSubmitConstReparacion(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.REPAES ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.DESSOLAPORT ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.RETINST) {
				await onSubmitRetiroParcial(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.AHOPROEDU) {
				await onSubmitAhorroProgamado(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHA ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHASP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.PACRECAHO ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.REDEFIN) {
				await onSubmitLeasing(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.READACPROES) {
				await onSubmitAdquirirAcciones(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.ADQUITER ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CONSNUEVASP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CCSLSS ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.ARMVGR ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.LIBERGRAV ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.APTGHGR) {
				await onSubmitAdquisicionTerreno(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.DESAFAPO ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.DESAFCESA) {
				await guardarActualizarSolicitudTarea(data, accionActual, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.VINUEVAPP ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.CAEXLEHAPP) {
				await onSubmitViviendaNuevaOcho(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.PCUOTAINI ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.PAGOCRHIP) {
				await onSubmitPagoCuotaInicialCH(data, opciones);
				return;
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.AVAC ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.AVAV) {
				onSubmitAvacAvav(data, opciones);
			}
			if (modalidadSolicitud === MODALIDAD_SOLICITUD.DSV ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.DAVAV ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.RMI ||
				modalidadSolicitud === MODALIDAD_SOLICITUD.DAVAC) {
				onSubmitDevolucionSaldo(data, opciones);
			}

		} finally {
			actualizacionSilenciosaRef.current = false;
		}
	}


	const formulario = useForm({
		defaultValues: {
			tipoDeRecepcion: '',
			radPreTramite: '',
			numeroMemorando: '',
			tipoTurno: '',
			numeroTurno: '',
			sucursal: '',
			fechaMemorando: '',
			tipoDocumentoConyugue: '',
			numeroDocumentoConyugue: '',
			primerNombreConyuge: '',
			segundoNombreConyuge: '',
			primerApellidoConyuge: '',
			segundoApellidoConyuge: '',
			observaciones: '',
			oficinaInmueble: 0,
			codigoOficina: '',
			ciudad: '',
			razonSocial: '',
			programaentidad: '',
			nivelEducativo: '',
			numReciboPago: '',
			fechaLimitePago: new Date(),
			valorSolicitud: '',
			entidadSeguroEducativo: '',
			numeroPolizaSeguro: '',
			tipoDoc: '',
			numDoc: '',
			direccion: '',
			departamento: '',
			correo: '',
			telefono: '',
			emisor: '',
			numeroMatricula: '',
			propietario: '',
			numeroMatriculaBuscar: '',
			solicitudMatricula: {},
			posee_Avac: false,
			numero_Folio: '',
			tipoDeVivienda: '',
			ciudadIA: '',
			direccionIA: '',
			numeroDeMatrículaIA: '',
			valorDelSubsidioIA: '',
			numeroDeEscrituraIE: '',
			fechaDeEscrituraciónIE: '',
			notariaIE: '',
			fechaLiberacion: null,
			fecha_Firma_Escritura: new Date(),
			observacionSolicitud: '',
			observacion: '',
			aceptarDeclaracionesAutorizaciones: false,
			causal: '0',
			destinacionDinero: '',
			saldoCuenta: '',
			cuenta: '',
			autorizarTraslado: false
		},
	});

	const valorSolicitudTurno =
		formulario.watch('valorSolicitud');

	useEffect(() => {
		const contieneInformacionTurno =
			formulariosSolicitud.includes(
				FORMS_NAMES.INFORMACION_TURNOS,
			);

		const contieneInformacionAdjudicacion =
			formulariosSolicitud.includes(
				FORMS_NAMES.INFORMACION_DE_ADJUDICACION,
			);

		if (
			!contieneInformacionTurno ||
			!contieneInformacionAdjudicacion
		) {
			return;
		}

		const valorActualSubsidio =
			formulario.getValues(
				'valorDelSubsidioIA',
			);

		if (
			valorActualSubsidio ===
			valorSolicitudTurno
		) {
			return;
		}

		formulario.setValue(
			'valorDelSubsidioIA',
			valorSolicitudTurno || '',
			{
				shouldValidate: true,
				shouldDirty: true,
			},
		);
	}, [
		valorSolicitudTurno,
		formulariosSolicitud,
		formulario,
	]);

	const formularioObservacionesActivo = formularioObservaciones || formulario;

	const esTareaActualRadicacion =
		nombreTareaNormalizado.includes(
			'radicacion',
		);

	const actividadRadicacionAprobada = useMemo(() => {
		const actividades = Array.isArray(trazabilidad)
			? trazabilidad
			: [];

		return (
			[...actividades]
				.reverse()
				.find((item: any) => {
					const tarea = normalizarTexto(
						item?.descripcionTarea,
					);

					const estado = normalizarTexto(
						item?.descripcionEstadoSolicitud,
					);

					const observacion = String(
						item?.observacion || '',
					).trim();

					return (
						tarea.includes('RADICACION') &&
						estado === 'APROBADA' &&
						observacion !== ''
					);
				}) || null
		);
	}, [trazabilidad]);

	const observacionRadicacion = String(
		actividadRadicacionAprobada?.observacion || '',
	);

	const debeMostrarObservacionRadicacion =
		Boolean(responseSolicitud?.nombreTarea) &&
		!esTareaActualRadicacion &&
		Boolean(actividadRadicacionAprobada);


	useEffect(() => {
		if (!debeMostrarObservacionRadicacion) {
			return;
		}

		formulario.setValue(
			'observaciones',
			observacionRadicacion,
			{
				shouldValidate: false,
				shouldTouch: false,
				shouldDirty: false,
			},
		);

		formulario.setValue(
			'observacion',
			observacionRadicacion,
			{
				shouldValidate: false,
				shouldTouch: false,
				shouldDirty: false,
			},
		);
	}, [
		debeMostrarObservacionRadicacion,
		observacionRadicacion,
		formulario,
	]);


	useEffect(() => {
		if (!formulario.formState.isSubmitting) return;

		if (accionFormularioRef.current === 'ANULAR') {
			return;
		}

		const valorSolicitud = UtilService.cleanCurrencyValue(
			formulario.getValues('valorSolicitud')
		);

		const numeroFolio = formulario.getValues('numero_Folio');

		if (valorSolicitud < 1) {
			toast('Debe ingresar un valor de solicitud válido', {
				type: toast.TYPE.ERROR
			});
		}

		if (numeroFolio === '') {
			toast('Debe ingresar el número de folios', {
				type: toast.TYPE.ERROR
			});
		}
	}, [formulario.formState.isSubmitting]);


	const handleOpenModalCreditoHipotecarioPagoCuotaInical = () => {

		if (terceroBeneficiarioModel.length === 0) {
			toast('Debe registrar al menos un tercero beneficiario', { type: toast.TYPE.ERROR });
			return;
		}

		setOpenModal(true);
	}

	const handleCloseModal = () => setOpenModal(false);

	const handleOnClick = (data: any) => {
		onSubmitAprobarCreditoHipotecarioPagoCuotaInical();
		handleCloseModal();
	}

	const tituloActivo = responseSolicitud?.descripcionModalidad;

	const GUID_DEFAULT = '00000000-0000-0000-0000-000000000000';


	if (loadingAprobacion && esRadicacionDevueltaPorRechazo) {
		return (
			<div
				style={{
					position: 'fixed',
					top: 0,
					left: 0,
					width: '100vw',
					height: '100vh',
					background: '#fff',
					zIndex: 99999,
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
				}}
			>
				<CHLoadingProcess />
			</div>
		);
	}

	if (loading && !esRadicacionDevueltaPorRechazo) {
		return <CHLoadingProcess />;
	}

	const tieneFormulario = (...formularios: string[]) =>
		formulariosSolicitud?.some((formulario: string) =>
			formularios.includes(formulario)
		);

	const requiereTerceroBeneficiario = () =>
		tieneFormulario(
			FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS,
			FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS_VU,
			FORMS_NAMES.TERCERO_BENEFICIARIO
		);

	const valorEstaVacio = (valor: any): boolean => {
		if (valor === null || valor === undefined) {
			return true;
		}

		if (typeof valor === 'string') {
			return valor.trim() === '';
		}

		return false;
	};

	const requiereInformacionTurno = (): boolean =>
		tieneFormulario('InformacionTurno');

	const requiereInformacionAdjudicacion = (): boolean =>
		tieneFormulario(
			FORMS_NAMES.INFORMACION_DE_ADJUDICACION
		);

	const requiereInformacionEscrituras = (): boolean =>
		tieneFormulario(
			FORMS_NAMES.INFORMACION_DE_ESCRITURAS
		);

	const obtenerErroresFormularios = (mostrarToast = false) => {
		const errores: string[] = [];

		if (
			formulariosSolicitud.some(
				(formulario: string) => formulario === 'PropietarioMatricula',
			) &&
			formulariosSolicitud.some(
				(formulario: string) => formulario === 'MatriculaInmobiliaria',
			)
		) {
			const propietariosConMatriculaNoVigente =
				propietariosMatriculaValidacion?.filter((propietario: any) =>
					propietario?.matricula?.some((matriculaPropietario: any) => {
						const existeMatriculaActual =
							matriculaInmobiliariaModel?.some((matriculaActual: any) => {
								const idActual = String(
									matriculaActual?.id ?? matriculaActual?.maiId ?? '',
								);

								const idPropietario = String(
									matriculaPropietario?.id ??
									matriculaPropietario?.maiId ??
									'',
								);

								const numeroActual = String(
									matriculaActual?.numeroMatricula ??
									matriculaActual?.maiNumeroMatricula ??
									'',
								).trim();

								const numeroPropietario = String(
									matriculaPropietario?.numeroMatricula ??
									matriculaPropietario?.maiNumeroMatricula ??
									'',
								).trim();

								return (
									(idActual && idActual === idPropietario) ||
									(numeroActual && numeroActual === numeroPropietario)
								);
							});

						return !existeMatriculaActual;
					}),
				) || [];

			if (propietariosConMatriculaNoVigente.length > 0) {
				errores.push(
					'La información de propietarios tiene matrículas desactualizadas.',
				);
			}
		}

		if (mostrarMensaje) {
			errores.push('Existen errores pendientes por corregir en el formulario.');
		}

		if (
			requiereTerceroBeneficiario() &&
			(!Array.isArray(terceroBeneficiarioModel) || terceroBeneficiarioModel.length === 0)
		) {
			errores.push('Debe registrar al menos un tercero beneficiario.');
		}

		const valoresFormulario = formulario.getValues();

		if (requiereInformacionTurno()) {
			// console.log('[VALIDACIÓN INFORMACIÓN TURNO]', {
			// 	tipoDeRecepcion:
			// 		valoresFormulario.tipoDeRecepcion,
			// 	tipoTurno:
			// 		valoresFormulario.tipoTurno,
			// 	numeroTurno:
			// 		valoresFormulario.numeroTurno,
			// 	sucursal:
			// 		valoresFormulario.sucursal,
			// });

			const informacionTurnoIncompleta =
				!esGuidTurnoValido(
					valoresFormulario.tipoDeRecepcion
				) ||
				!esGuidTurnoValido(
					valoresFormulario.sucursal
				);

			if (informacionTurnoIncompleta) {
				errores.push(
					'Debe completar la información de turno.'
				);
			}
		}

		if (requiereInformacionAdjudicacion()) {
			const informacionAdjudicacionIncompleta =
				valorEstaVacio(
					valoresFormulario.tipoDeVivienda
				) ||
				valorEstaVacio(
					valoresFormulario.direccionIA
				) ||
				valorEstaVacio(
					valoresFormulario.ciudadIA
				) ||
				valorEstaVacio(
					valoresFormulario.numeroDeMatrículaIA
				) ||
				valorEstaVacio(
					valoresFormulario.valorDelSubsidioIA
				);

			// if (informacionAdjudicacionIncompleta) {
			// 	errores.push(
			// 		'Debe completar la información de adjudicación.'
			// 	);
			// }
		}

		if (requiereInformacionEscrituras()) {
			const informacionEscrituraIncompleta =
				valorEstaVacio(
					valoresFormulario.numeroDeEscrituraIE
				) ||
				valorEstaVacio(
					valoresFormulario.fechaDeEscrituraciónIE
				) ||
				valorEstaVacio(
					valoresFormulario.notariaIE
				);

			// if (informacionEscrituraIncompleta) {
			// 	errores.push(
			// 		'Debe completar la información de escritura.'
			// 	);
			// }
		}

		if (mostrarToast && errores.length > 0) {
			toast(errores[0], { type: toast.TYPE.ERROR });
		}

		return errores;
	};

	const erroresFormularios = obtenerErroresFormularios();

	const deshabilitarAprobar = erroresFormularios.length > 0;

	const obtenerIdTareaSolicitudActual = () => {
		const tareaActual = trazabilidad?.find((item: any) =>
			item?.descripcionTarea === responseSolicitud?.nombreTarea &&
			item?.descripcionEstadoSolicitud === 'PENDIENTE'
		);

		return tareaActual?.idTareaSolicitud || '';
	};

	let matriculasParaMostrar: MatriculaInmobiliariaModel[] = [];

	if (Array.isArray(matriculaInmobiliariaModel)) {
		matriculasParaMostrar = matriculaInmobiliariaModel;
	} else if (Array.isArray(responseSolicitud?.solicitudMatriculaDto)) {
		matriculasParaMostrar =
			responseSolicitud?.solicitudMatriculaDto as MatriculaInmobiliariaModel[];
	}

	const formulariosParaRender =
		formulariosSolicitud.length > 0
			? formulariosSolicitud
			: formsList;


	return (
		<CHPrivateContainer>
			<LoadingV2Component key="loader" loading={isLoading} tittle={' '} />
			{tituloActivo !== 'ACREDITACION V8' && title? (
				<CHPageTitleV2 topText={topText} title1={responseSolicitud?.descripcionProceso === "DEVOLUCIÓN DE SALDOS"? responseSolicitud?.descripcionProceso: title} icono={icon} />
			) : null}

			{shouldShowTabs ? (
				<>
					<FormProvider {...formulario}>
						{activeTab === TABS_INDEX.RESUMENSOLICITUD && (
							<>
								{formsList?.map((f, index) => {
									if (f === FORMS_NAMES.INFORMACION_ACREDITACION) {
										return (
											<>
												<ProcesoAcreditacion
													responseSolicitud={responseSolicitud}
													control={formulario}
													onItemGoBack={onItemGoBack}
													onSubmitAprobar={onSubmitAprobar}
													onSubmitRechazar={onSubmitRechazar}
													onSubmit={onSubmit}
													handleSubmit={formulario.handleSubmit}
													tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
													tipoSolicitud={tipoSolicitud}
													cambiarEstadoTramite={cambiarEstadoTramite}
													trazabilidad={trazabilidad}
													setValidarRetiro={setValidarRetiro}
													setMostrarMensaje={setMostrarMensaje}
													withButtons={mostrarBotonera}
													hideTitle
													obtnerBoleano={obtnerBoleano}
												/>
											</>

										)
									}
									return null;
								})}

							</>
						)}


						{activeTab === TABS_INDEX.CARGUEDOCUMENTOACREDITACION && (
							<>
								{!responseSolicitud?.solicitudId ||
									!responseSolicitud?.solicitudTareaId ||
									responseSolicitud?.solicitudId === GUID_DEFAULT ||
									responseSolicitud?.solicitudTareaId === GUID_DEFAULT ? (
									<CHLoadingProcess />
								) : (
									formsList?.map((f, index) => {
										if (f === FORMS_NAMES.INFORMACION_ACREDITACION) {
											return (
												<CargueDocumentosSolicitud
													responseSolicitud={responseSolicitud}
													documentosAgregados={documentosAgregados}
													siEsAprobadaSolcitud={siEsAprobadaSolcitud}
													key={index}
												/>
											);
										}
										return null;
									})
								)}
							</>
						)}
					</FormProvider>

				</>
			) : (

				formulariosParaRender?.map((f, index) => {
					if (f === FORMS_NAMES.DATOS_PERSONALES) {
						return (
							<DatosPersonales
								control={formulario}
								responseSolicitud={responseSolicitud}
								setValidarRetiro={setValidarRetiro}
								setMostrarMensaje={setMostrarMensaje}
								setToastId={setToastId}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}

					if (f === FORMS_NAMES.DATOS_PERSONALES_VALOR) {
						return (
							<DatosPersonalesValor
								control={formulario}
								responseSolicitud={responseSolicitud}
							/>
						)
					}
					if (f === FORMS_NAMES.DATOS_PERSONALES_DESAFILIACION) {
						return (
							<DatosPersonalesDesafiliacionCesantias
								control={formulario}
								responseSolicitud={responseSolicitud}
							/>
						)
					}
					if (f === FORMS_NAMES.DATOS_PERSONALES_VALOR_CUOTA_INICIAL) {
						return (
							<DatosPersonalesValorCuotaInicial
								control={formulario}
								responseSolicitud={responseSolicitud}
								valorDesembolsoCredito={creditState.valorDesembolsoCreditoDto}
								obtenerValorDesembolsoCredito={(numeroDocumento: string) => obtenerValorDesembolsoCredito(numeroDocumento)}
							/>
						)
					}
					if (f === FORMS_NAMES.ENTIDAD_EDUCATIVA) {
						return (
							<EntidadEducativa
								responseSolicitud={responseSolicitud}
								validarRetiro={validarRetiro}
								setValorReciboTotal={setValorReciboTotal}
								listaEntidadEducativa={catalogState.entidadEducativa}
								// setEstadoEntidadNueva={entidadEducativaAdicionada}
								setEstadoEntidadNueva={setEstadoEntidadNueva}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								listaTiposDocumento={catalogState.IdentificationType || []}
								listaTipoPersona={catalogState.tipoPersona || []}
								setIdsEntidadesCreadasFormulario={
									setIdsEntidadesCreadasFormulario
								}
							/>
						)
					}

					if (f === FORMS_NAMES.MATRICULA_INMOBILARIA) {
						return (
							<MatriculaInmobiliaria
								listaMatriculas={matriculasParaMostrar as MatriculaInmobiliariaModel[]}
								title="Información del inmueble"
								matriculasAdicionadas={matriculasAdicionadas}
								listaDepartamentos={catalogState.departments}
								listaCiudades={catalogState.City}
								listaOficinas={oficinasData}
								responseSolicitud={responseSolicitud}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								esRadicacionDevueltaPorRechazo={esRadicacionDevueltaPorRechazo}
								propietariosMatricula={propietarioMatriculaModel}
								matriculasPendientesPorEliminar={agregarMatriculasPendientesEliminar}
								propietariosMatriculaActualizados={setPropietarioMatriculaModel}
							/>
						)
					}
					if (f === FORMS_NAMES.INFORMACION_INMUEBLE) {

						return (
							<InformacionInmueble
								oficinasRegistro={catalogState.oficinasRegistro}
								oficinaRegistro={oficinaRegistro}
								// listaMatriculas={responseSolicitud?.solicitudMatriculaDto as MatriculaInmobiliariaModel[]}
								listaMatriculas={matriculasParaMostrar}
								matriculasAdicionadas={matriculasAdicionadas}
								responseSolicitud={responseSolicitud}
								listaDepartamentos={catalogState.departments}
								listaCiudades={catalogState.City}
								listaOficinas={oficinasData}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}
					if (f === FORMS_NAMES.TERCEERO_TENEDOR_ACCIONES) {
						return (
							<TerceroTenedorAcciones
								formulario={formulario}
								responseSolicitud={responseSolicitud}
								listaTipoDocumentos={catalogState.IdentificationType}
								obtenerCiudadPorVendedorId={(stateId?: string) => obtenerCiudadPorVendedorId(stateId)}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}

					if (f === FORMS_NAMES.PROPIETARIO_MATRICULA) {

						return (
							<PropietarioMatricula
								listaPropietarios={responseSolicitud?.solicitudPropietarioDto as PropietarioMatriculaModel[]}
								title="Propietarios Inmueble"
								tiposIdentificacion={catalogState.IdentificationType}
								matriculas={matriculaInmobiliariaModel}
								propietariosAdicionados={propietariosAdicionados}
								// setValidarBeneficiario={setValidarBeneficiario}
								setValidarBeneficiario={setPropietariosMatriculaValidacion}
								responseSolicitud={responseSolicitud}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}
					if (f === FORMS_NAMES.TERCERO_ENTIDAD_SEGURO_EDUCATIVO) {
						return (

							<TerceroEntidadSeguroEducativo
								responseSolicitud={responseSolicitud}
								listaEntidadSeguroEducativo={catalogState.entidadSeguroEducativo}
								setEntidadAhorroProgramadoSeguroEducativo={setEntidadAhorroProgramadoSeguroEducativo}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								onChangeEntidad={(entidad: ENTIDADSEGUROEDUCATIVO) => {
									if (entidad) {
										const nuevoEntidad: BeneficiarioModel = {
											tipo: 'Entidad',
											tipoDoc: Number(entidad.tipoIdentificacion),
											numDoc: entidad.numeroIdentificacion,
											razonSocial: entidad.nombreRazonSocial,
											esAutorizado: true,
											parentesco: "",
											primerNombre: entidad.nombreRazonSocial,
											segundoNombre: "",
											primerApellido: "",
											segundoApellido: "",
										};
										setEntidadesFinancieras([nuevoEntidad]);
									} else {
										setEntidadesFinancieras([]);
									}
								}}
							/>
						)
					}
					if (f === FORMS_NAMES.TERCERO_VENDEDOR) {
						return (
							<TerceroVendedor
								listaVendedor={responseSolicitud?.solicitudTerceroVendedorDto as TerceroVendedorModel[]}
								tercerosVendedorAdicionados={tercerosVendedorAdicionados}
								listaTipoDocumentos={catalogState.IdentificationType}
								listaDepartamentos={catalogState.departments}
								listaCiudades={catalogState.citiesVendors}
								obtenerCiudadPorVendedorId={(stateId?: string) => obtenerCiudadPorVendedorId(stateId)}
							/>
						)
					}

					if (f === FORMS_NAMES.TERCERO_APODERADO) {
						return (
							<TerceroApoderado
								listaApoderado={responseSolicitud?.solicitudTerceroApoderadoDto as TerceroApoderadoModel[]}
								tercerosApoderadoAdicionados={tercerosApoderadoAdicionados}
								listaTipoDocumentos={catalogState.IdentificationType}
								listaParentesco={catalogState.parentescoCatalogo}
								obtenerParentesco={(catalogoId) => obtenerParentesco(catalogoId)}
								responseSolicitud={responseSolicitud}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								setDatosBeneficiarios={setDatosBeneficiarios}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								setApoderadosAutorizados={setApoderadosAutorizados}
							/>
						)
					}

					if (f === FORMS_NAMES.DESTINACION_DE_LOS_DINEROS && responseSolicitud?.timCodigo === 'DSV') {
						return (
							<DestinacionDineros
								formulario={formulario}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								setDestinacionDinerosSeleccionado={setDestinacionDinerosSeleccionado}
							/>
						)
					}

					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS) {
						return (
							<TerceroBeneficiarioPagosVu
								responseSolicitud={responseSolicitud}
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								listaApoderados={terceroApoderadoModel}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								beneficiarios={beneficiarios}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								listaTipoDocumentos={catalogState.IdentificationType}
								formulariosSolicitud={formulariosSolicitud}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarRetiro={validarRetiro}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								listaEntidadesEducativas={estadoEntidadNueva}
								beneficiarioAfiliado={beneficiarioAfiliado}
								propietarioMatriculaModel={propietarioMatriculaModel}
								entidadAhorroProgramadoSeguroEducativo={entidadAhorroProgramadoSeguroEducativo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}

					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS_VU && beneficiarios.length !== 0 && responseSolicitud?.timCodigo !== 'DESCONCESANT' &&
						responseSolicitud?.timCodigo !== 'DESSOLAPORT' && responseSolicitud?.timCodigo !== 'RETINST' && responseSolicitud?.timCodigo !== 'SDAPOR') {

						return (
							<TerceroBeneficiarioPagosVu
								responseSolicitud={responseSolicitud}
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								listaApoderados={terceroApoderadoModel}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								beneficiarios={beneficiarios}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								listaTipoDocumentos={catalogState.IdentificationType}
								formulariosSolicitud={formulariosSolicitud}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarRetiro={validarRetiro}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								listaEntidadesEducativas={estadoEntidadNueva}
								beneficiarioAfiliado={beneficiarioAfiliado}
								propietarioMatriculaModel={propietarioMatriculaModel}
								entidadAhorroProgramadoSeguroEducativo={entidadAhorroProgramadoSeguroEducativo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}

					if (f === FORMS_NAMES.MIS_CUENTAS && destinacionDinerosSeleccionado === 'transferencia' && responseSolicitud?.timCodigo === 'DSV') {
						return (
							<MisCuentas
								formulario={formulario}
								clientStateData={clientStateData}
								nombreCuenta={nombreCuenta}
								setNombreCuenta={setNombreCuenta}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								setSaldoCuenta={setSaldoCuenta}
								setAceptaTraslado={setAceptaTraslado}
							/>
						)

					}

					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS_VU && (responseSolicitud?.timCodigo === "AHOPROEDU" || responseSolicitud?.timCodigo === "AVAC" || responseSolicitud?.timCodigo === "DSV" || (responseSolicitud?.timCodigo === "SDAPOR" && destinacionDinerosSeleccionado === 'retiro'))) {

						return (
							<TerceroBeneficiarioPagosVu
								responseSolicitud={responseSolicitud}
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								listaApoderados={terceroApoderadoModel}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								beneficiarios={beneficiarios}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								listaTipoDocumentos={catalogState.IdentificationType}
								formulariosSolicitud={formulariosSolicitud}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarRetiro={validarRetiro}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								listaEntidadesEducativas={estadoEntidadNueva}
								beneficiarioAfiliado={beneficiarioAfiliado}
								propietarioMatriculaModel={propietarioMatriculaModel}
								entidadAhorroProgramadoSeguroEducativo={entidadAhorroProgramadoSeguroEducativo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}

					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS_DESAFILACIONES && beneficiarios.length !== 0 && responseSolicitud?.timCodigo === 'DESCONCESANT' && 'DESSOLAPORT' && 'RETINST') {

						return (
							<TerceroBeneficiarioPagoDesafilaciones
								responseSolicitud={responseSolicitud}
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								beneficiarios={beneficiarios}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								listaTipoDocumentos={catalogState.IdentificationType}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarBeneficiario={validarBeneficiario}
								validarRetiro={validarRetiro}
								estadoEntidadNueva={estadoEntidadNueva}
								entidadAhorroProgramadoSeguroEducativo={entidadAhorroProgramadoSeguroEducativo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}
					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS_DESAFILACIONES && beneficiarios.length !== 0 && (responseSolicitud?.timCodigo === 'RETINST' || responseSolicitud?.timCodigo === 'DESSOLAPORT')) {

						return (
							<TerceroBeneficiarioPagoDesafilaciones
								// formulario dos
								responseSolicitud={responseSolicitud}
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								beneficiarios={
									responseSolicitud?.descripcionSubModalidad === "POR MUERTE DEL AFILIADO"
										? beneficiariosCompartidos
										: beneficiarios
								}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								listaTipoDocumentos={catalogState.IdentificationType}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarBeneficiario={validarBeneficiario}
								validarRetiro={validarRetiro}
								estadoEntidadNueva={estadoEntidadNueva}
								entidadAhorroProgramadoSeguroEducativo={entidadAhorroProgramadoSeguroEducativo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								beneficiariosExternos={beneficiariosCompartidos}
								apoderadosAutorizados={apoderadosAutorizados}
							/>
						)
					}

					if ((f === FORMS_NAMES.TERCERO_BENEFICIARIO
						&& responseSolicitud?.timCodigo !== 'ACDDHSS'
						&& responseSolicitud?.timCodigo !== 'LIBERGRAV')
						&& responseSolicitud?.timCodigo !== 'APTGHGR'
						&& responseSolicitud?.timCodigo !== "READACPROES"
						&& responseSolicitud?.timCodigo !== "CAEXLEHA"
						&& responseSolicitud?.timCodigo !== "AHOPROEDU"
						&& responseSolicitud?.timCodigo !== "PAGOPREDIAL"
						&& responseSolicitud?.timCodigo !== "CESOCLEAS"
						&& responseSolicitud?.timCodigo !== "PAGOCRHIP"
						&& responseSolicitud?.timCodigo !== "CAEXLEHAPP"
						&& responseSolicitud?.timCodigo !== "VIUSADAPP"
						&& responseSolicitud?.timCodigo !== "VIUSADASP"
						&& responseSolicitud?.timCodigo !== "CVUSS"
						&& responseSolicitud?.timCodigo !== "VINUEVAPP"
						&& responseSolicitud?.timCodigo !== "VINUEVASP"
						&& responseSolicitud?.timCodigo !== "CVNSS"
						&& responseSolicitud?.timCodigo !== "CONSNUEVASP"
						&& responseSolicitud?.timCodigo !== "CCSLSS"
						&& responseSolicitud?.timCodigo !== "EJEOPCOMP"
						&& responseSolicitud?.timCodigo !== "PACRECAHO"
						&& responseSolicitud?.timCodigo !== "DESCONCESANT"
						&& responseSolicitud?.timCodigo !== "DESSOLAPORT"
						&& responseSolicitud?.timCodigo !== "RETINST"
					) {
						return (
							<TerceroBeneficiarioPagosVu
								responseSolicitud={responseSolicitud}
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								listaApoderados={terceroApoderadoModel}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								beneficiarios={beneficiarios}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								listaTipoDocumentos={catalogState.IdentificationType}
								formulariosSolicitud={formulariosSolicitud}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarRetiro={validarRetiro}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								listaEntidadesEducativas={estadoEntidadNueva}
								beneficiarioAfiliado={beneficiarioAfiliado}
								propietarioMatriculaModel={propietarioMatriculaModel}
								entidadAhorroProgramadoSeguroEducativo={entidadAhorroProgramadoSeguroEducativo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}

					if (f === FORMS_NAMES.BENEFICIARIOS && responseSolicitud?.descripcionSubModalidad === 'DEVOLUCIÓN DE AHORROS AVAV - MUERTE DEL AFILIADO') {

						return (
							<Beneficiarios
								listaBeneficiarios={responseSolicitud?.solicitudBeneficiarioDto as BeneficiarioModel[]}
								listaTipoDocumentos={catalogState.IdentificationType}
								listaParentesco={catalogState.parentescoCatalogo}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								beneficiariosAdicionados={beneficiariosAdicionados}
							/>
						)
					}

					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO) {
						return (
							<TerceroBeneficiarioPago
								listaBeneficiario={responseSolicitud?.solicitudTerceroBeneficiarioDto as TerceroBeneficiarioModel[]}
								responseSolicitud={responseSolicitud}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								tercerosBeneficiarioAdicionados={tercerosBeneficiarioAdicionados}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
								validarRetiro={validarRetiro}
								listaTipoDocumentos={catalogState.IdentificationType}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}
					if (f === FORMS_NAMES.INFORMACION_TURNOS) {
						return (
							<InformacionDeTurno
								formulario={formulario}
								informacionDeTurno={catalogState.CatalogData}
								responseSolicitud={responseSolicitud}
								turnoForm={responseSolicitud?.informacionTurnoDto as InformacionTurnosModel}
								campoSoloLectura={solicitudSoloLectura}
							/>
						)
					}

					if (
						f === FORMS_NAMES.INFORMACION_DE_ADJUDICACION &&
						mostrarInformacionSubsidioSecuestrados
					) {
						return (
							<InformacionDeAdjudicacion
								key={`${f}-${index}`}
								formulario={formulario}
								responseSolicitud={responseSolicitud}
								matriculas={matriculaInmobiliariaModel}
							/>
						);
					}
					if (
						f === FORMS_NAMES.INFORMACION_DE_ESCRITURAS &&
						mostrarInformacionSubsidioSecuestrados
					) {
						return (
							<InformacionDeEscrituras
								key={`${f}-${index}`}
								formulario={formulario}
								responseSolicitud={responseSolicitud}
							/>
						);
					}
					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_DESAFILIACIONES) {
						return (
							<TerceroBeneficiarioDesafiliaciones
								controlBeneficiario={formulario}
								responseSolicitud={responseSolicitud}
								listaValorCatalogo={catalogState.valorCatalogo}
								listaEntidadBancaria={catalogState.entidadBancaria}
								obtenerTipoCuenta={(catalogoId) => obtenerTipoCuenta(catalogoId)}
							/>
						)
					}
					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_ESTUDIO && responseSolicitud?.descripcionSubModalidad !== "RETIRO EXPRESS DE LA INSTITUCION" && responseSolicitud?.descripcionSubModalidad !== "RETIRO NORMAL DE LA INSTITUCION") {
						return (
							<TerceroBeneficiarioEstudio
								// formulario uno
								listaBeneficiarioEstudio={responseSolicitud?.solicitudTerceroBeneficiarioEstudioDto as TerceroBeneficiarioEstudioModel[]}
								tercerosBeneficiarioEstudioAdicionados={tercerosBeneficiarioEstudioAdicionados}
								listaTipoDocumentos={catalogState.IdentificationType}
								listaParentesco={catalogState.parentescoCatalogo}
								obtenerParentesco={(catalogoId) => obtenerParentesco(catalogoId)}
								siEsAprobadaSolcitud={siEsAprobadaSolcitud}
								responseSolicitud={responseSolicitud}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
								onBeneficiariosChange={(listaActualizada: any) => {
									if (responseSolicitud?.descripcionSubModalidad === "POR MUERTE DEL AFILIADO") {

										setBeneficiariosCompartidos(listaActualizada);
										setBeneficiarios(listaActualizada);
									} else {
										setBeneficiarios(listaActualizada);
									}
								}}
							/>
						)
					}
					if (f === FORMS_NAMES.INFORMACION_SOLICITUD) {
						return (
							<InformacionSolicitud
								responseSolicitud={responseSolicitud}
								listaForce={catalogState.Force}
								listaGrade={catalogState.Grade}
								listaCategorias={catalogState.categories}
								listaEstadoCivil={catalogState.civilstates}
								listaDepartamentos={catalogState.departments}
								listaTipoTelefonos={catalogState.phoneTypes}
								obtenerEstadosCiviles={(props: string) => obtenerEstadosCiviles(props)}
							/>
						)
					}

					if (f === FORMS_NAMES.IDENTIFICACION_POSTULANTES_NUCLEO_FAMILIAR) {
						return (
							<IdentificacionPostulantesNucleoFamiliar />
						)
					}
					if (f === FORMS_NAMES.MANIFESTACIONES_AUTORIZACIONES) {
						return (
							<ManifestacionesAutorizaciones />
						)
					}
					// Formulario se muestra en solicitudes Vivienda 8, Vivienda 14 y subsidio secuestrados
					if (f === FORMS_NAMES.DECLARACIONES_AUTORIZACIONES && esTareaDeclaraciones) {
						return (
							<DeclaracionesAutorizaciones
								formulario={formulario}
								setAceptarCheckValue={setAceptarCheckValue}
								campoSoloLecturaRadicacionAprobada={solicitudSoloLectura}
							/>
						)
					}
					if (f === FORMS_NAMES.CIUDAD_PREFERENCIA_PARA_RECIBIR_VIVIENDA) {
						return (
							<CiudadPreferenciaParaRecibirVivienda />
						)
					}
					if (f === FORMS_NAMES.FIRMA_SOLICITUD) {
						return (
							<FirmaSolicitud />
						)
					}

					const esFormularioObservaciones =
						f === FORMS_NAMES.OBSERVACIONES_SOLICITUD;

					const nombreTareaActual = responseSolicitud?.nombreTarea || '';

					const nombreTareaNormalizadoLocal = nombreTareaActual
						.toLowerCase()
						.normalize('NFD')
						.replace(/[\u0300-\u036f]/g, '');

					const esTareaRadicacion =
						nombreTareaNormalizadoLocal.includes('radicacion');

					const esRadicacionDevuelta =
						responseSolicitud?.soL_RECHAZADA === '1' &&
						esTareaRadicacion;

					if (
						esFormularioObservaciones &&
						(inicioSolicitud || esRadicacionDevuelta || esTareaRadicacion)
					) {

						return (
							<ObservacionesInicioSolicitud
								control={formulario}
								responseSolicitud={responseSolicitud}
								trazabilidad={trazabilidad}
							/>
						);
					}

					// if (
					// 	f === FORMS_NAMES.OBSERVACIONES_SOLICITUD &&
					// 	(
					// 		responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 1') ||
					// 		responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 2') ||
					// 		responseSolicitud?.nombreTarea?.includes('Verificación')
					// 	)
					// ) {

					// 	return (
					// 		<ObservacionesSolicitud
					// 			control={formulario}
					// 			seccion='Detalle Solicitud'
					// 			trazabilidad={trazabilidad || []}
					// 		/>
					// 	);
					// }

					if (
						esFormularioObservaciones &&
						debeMostrarObservacionRadicacion
					) {
						return (
							<ObservacionesSolicitud
								control={formulario}
								seccion='Detalle Solicitud'
								trazabilidad={trazabilidad || []}
								nombreTareaActual={
									responseSolicitud?.nombreTarea || ''
								}
								soloLecturaVerificacion
							/>
						);
					}

					const nombreTarea =
						responseSolicitud?.nombreTarea || '';

					const esSeguridadDocumental =
						nombreTarea.includes(
							'Seguridad documental grupo 1',
						) ||
						nombreTarea.includes(
							'Seguridad documental grupo 2',
						);

					const esVerificacion =
						nombreTarea.includes('Verificación');

					if (
						esFormularioObservaciones &&
						(esSeguridadDocumental || esVerificacion)
					) {
						return (
							<ObservacionesSolicitud
								control={formulario}
								seccion='Detalle Solicitud'
								trazabilidad={trazabilidad || []}
								nombreTareaActual={nombreTarea}
							/>
						);
					}

					if (
						esFormularioObservaciones &&
						!inicioSolicitud &&
						!esTareaRadicacion &&
						!esRadicacionDevuelta &&
						(
							!responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 1 retiro parcial') &&
							!responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 1 Giro de recursos') &&
							!responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 1 Subsidios secuestrados') &&
							!responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 2 retiro parcial') &&
							!responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 2 Subsidios secuestrados') &&
							!responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 2 Giro de recursos') &&
							!responseSolicitud?.nombreTarea?.includes('Verificación cuenta individual')
						)
					) {
						return (
							<ObservacionesSolicitud
								control={formularioObservacionesActivo}
								seccion='Detalle Solicitud'
								trazabilidad={trazabilidad || []}
							/>
						);
					}


					if (f === FORMS_NAMES.TERCERO_BENEFICIARIO_PAGOS_VU && inicioSolicitud && responseSolicitud?.nombreTarea.includes('Verificación en tiempo')) {

						return (
							<ObservacionesSolicitud
								control={formulario}
								seccion='Detalle Solicitud'
								trazabilidad={trazabilidad || []}
								nombreTareaActual={responseSolicitud?.nombreTarea || ''}

							/>
						)
					}

					// cambiarEstadoTramite
					return <div key={index} />
				})
			)}


			<ModalCausalAnularSolicitud
				open={openModalAnular}
				formulario={formulario}
				onClose={onCloseAnular}
				funcionAnular={funcionAnular}
			/>

			{modalidadSolicitud === MODALIDAD_SOLICITUD.PCUOTAINI && openModal &&
				<ModalResumenPago
					open={openModal}
					handleClose={handleCloseModal}
					responseSolicitud={responseSolicitud}
					listaValorCatalogo={catalogState.valorCatalogo}
					listaEntidadBancaria={catalogState.entidadBancaria}
					handleOnClick={handleOnClick}
				/>
			}

			{/* {!tipoAccionDetalleProcesar && withButtons &&
				<BotoneraSolicitud
					onItemGoBack={onItemGoBack}
					tipoAccion={tipoAccionNumber}
					onSubmitAprobar={onSubmitAprobar}
					onSubmitRechazar={onSubmitRechazar}
					onSubmitAnular={onSubmitAnular}
					onSubmit={onSubmit}
					handleSubmit={formulario.handleSubmit}
					tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
					tipoSolicitud={tipoSolicitud}
					cambiarEstadoTramite={cambiarEstadoTramite}
					esRadicacionDevueltaPorRechazo={esRadicacionDevueltaPorRechazo}
					trazabilidad={trazabilidad}
				/>
			} */}


			{!tipoAccionDetalleProcesar && withButtons && (
				<BotoneraSolicitud
					onItemGoBack={onItemGoBack}
					tipoAccion={tipoAccionNumber}
					onSubmitAprobar={onSubmitAprobar}
					onSubmitRechazar={onSubmitRechazar}
					onSubmitAnular={onSubmitAnular}
					// onSubmit={onSubmit}
					onSubmit={(opciones) => onSubmit(opciones)}
					handleSubmit={formulario.handleSubmit}
					tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
					tipoSolicitud={tipoSolicitud}
					cambiarEstadoTramite={cambiarEstadoTramite}
					esRadicacionDevueltaPorRechazo={esRadicacionDevueltaPorRechazo}
					trazabilidad={trazabilidad}
					deshabilitarAprobar={deshabilitarAprobar}
				/>
			)}

		</CHPrivateContainer>
	)
}

export default DynamicPage;