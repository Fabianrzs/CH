import React, { useEffect, useState } from 'react';
import { createStyles, makeStyles, Tab, Tabs, Theme, withStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import ResumenSolicitud from '../components/resumen-solicitud/resumen-solicitud.component';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import BotoneraInformacionSolicitudComponent from '../components/botonera/botonera-informacion-solicitud-component';
import { BOTONERA_SOLICITUD, MODALIDAD_SOLICITUD } from '../../../constants/constants';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import CargueDocumentosSolicitud from '../components/cargue-documentos-solicitud/cargue-documentos-solicitud.component';
import { DocumentosPorSubModalidadModel } from '../../../models/documentos-por-submodalidad.model';
import SolicitudApi from '../../../services/api-solicitud.service';
import FormularioInconsistencias from '../components/formulario-inconsistencias/formulario-inconsistencias.component';
import NotificationService from '../../../services/notification.service';
import history from '../../../utils/history.router';
import ObservacionesSolicitud from '../components/observaciones/observaciones';
import TrazabilidadSolicitud from '../components/trazabilidad/trazabilidad.component';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import CartaInconsistencia from '../components/CartaInconsistencia';
import DynamicPage from '../dynamicPage/DynamicPage.component';
import { FormulariosSolicitudDto } from '../../../models/requests/solicitud-tarea.model';
import HistorialInconsistenciaSolicitud from '../components/historialIconsistenciaSolicitud/HistorialInconsistenciaSolicitud';
import ModalCausalAnularSolicitud from '../components/modal-causal-anular/modal-causal-anular';

const TABS_INDEX = {
	RESUMENSOLICITUD: 'resumen',
	CARGUEDOCUMENTOS: 'carguededocumentosretiroparcial',
	CARGUEDOCUMENTOSV14: 'carguededocumentosv14',
	CARGUEDOCUMENTOSV8: 'carguededocumentosv8',
	CARGUEDOCUMENTOSDESAFILIACION: 'carguededocumentosdesafiliaciones',
	CARGUEDOCUMENTOACREDITACION: 'carguededocumentosacreditacion',
	VALIDACIONDOCUMENTOS: 'validaciondedocumentos',
	CARGAARCHIVOS: 'cargaarchivos_2',
	VERARCHIVOS: 'verarchivos_2',
	FORMULARIO_INCONSISTENCIAS: 'formulario',
	VERDETALLESOLICITUD: 'Detalle Solicitud',
	VERIFICACIONCUENTAINDIVIDUAL: '10',
	VERIFICACIONESDECUENTA: '11',
	HISTORIALINCONSISTENCIA: '7',
	SEGURIDADDOCUMENTAL: '8',
	VERIFICACIONLISTASRESTRICTIVAS: 'VERIFICACIONLISTASRESTRICTIVAS',
};

const useStyles = makeStyles(() =>
	createStyles({
		buttonRechazar: {
			background: '#7cd1f9',
			color: '#595959'
		},
		buttonAnular: {
			backgroundColor: '#e64942',
		},
		buttonCancelar: {
			backgroundColor: '#efefef',
			color: '#595959'
		}
	}),
);

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

const normalizarTexto = (texto: any) =>
	String(texto || '')
		.trim()
		.toLowerCase()
		.normalize('NFD')
		.replace(/[\u0300-\u036f]/g, '');


function ViewTabs({
	currentTab,
	onTabChange,
	responseSolicitud,
	userIngreso,
	userAsig,
	esRadicacionDevueltaPorRechazo,
	esVerificacionListasRestrictivas
}: {
	currentTab: string;
	onTabChange: (event: React.ChangeEvent<any>, newValue: string) => void;
	responseSolicitud: ResponseSeveranceModel | undefined;
	userIngreso: any;
	userAsig: any;
	esRadicacionDevueltaPorRechazo?: boolean;
	esVerificacionListasRestrictivas?: boolean;
}) {

	const tabsToShow1 = [
		{ label: 'Resumen', value: TABS_INDEX.RESUMENSOLICITUD },
		{ label: 'Cargue de documentos', value: TABS_INDEX.CARGUEDOCUMENTOS },
		{ label: 'Detalle Solicitud', value: TABS_INDEX.VERDETALLESOLICITUD },

	];
	const tabsToShow2 = [
		{ label: 'Resumen', value: TABS_INDEX.RESUMENSOLICITUD },
		{ label: 'Cargue de documentos', value: TABS_INDEX.CARGUEDOCUMENTOS },
		{ label: 'Detalle Solicitud', value: TABS_INDEX.VERDETALLESOLICITUD },
		{ label: 'Historial Inconsistencia', value: TABS_INDEX.HISTORIALINCONSISTENCIA },
		{ label: 'Verificaciones de Cuenta', value: TABS_INDEX.VERIFICACIONESDECUENTA },

	];
	const tabsToShow3 = [
		{ label: 'Resumen', value: TABS_INDEX.RESUMENSOLICITUD },
		{ label: 'Cargue de documentos', value: TABS_INDEX.CARGUEDOCUMENTOACREDITACION },

	];

	const tabsToShowRadicacionDevuelta = [
		{ label: 'Resumen', value: TABS_INDEX.RESUMENSOLICITUD },
		{ label: 'Detalle Solicitud', value: TABS_INDEX.VERDETALLESOLICITUD },
	];


	const tabsToShowListasRestrictivas = [
		{ label: 'Resumen', value: TABS_INDEX.RESUMENSOLICITUD },
		{ label: 'Detalle Solicitud', value: TABS_INDEX.VERDETALLESOLICITUD },
		{ label: 'Verificación de listas restrictivas', value: TABS_INDEX.VERIFICACIONLISTASRESTRICTIVAS },
	];


	let filteredTabs = tabsToShow1;

	// if (responseSolicitud?.nombreTarea && responseSolicitud?.nombreTarea.includes('Verificación novedades en los datos')) {
	// 	if (userIngreso && userIngreso === userAsig) {
	// 		filteredTabs = tabsToShow2;
	// 	}
	// } else if (
	// 	responseSolicitud?.nombreTarea && responseSolicitud?.nombreTarea === 'Verificación en tiempo'
	// ) {
	// 	filteredTabs = tabsToShow3;
	// }

	if (esRadicacionDevueltaPorRechazo) {
		filteredTabs = tabsToShowRadicacionDevuelta;
	} else if (esVerificacionListasRestrictivas) {
		filteredTabs = tabsToShowListasRestrictivas;
	} else if (responseSolicitud?.nombreTarea && responseSolicitud?.nombreTarea.includes('Verificación novedades en los datos')) {
		if (userIngreso && userIngreso === userAsig) {
			filteredTabs = tabsToShow2;
		}
	} else if (
		responseSolicitud?.nombreTarea && responseSolicitud?.nombreTarea === 'Verificación en tiempo'
	) {
		filteredTabs = tabsToShow3;
	}

	return (
		<AntTabs value={currentTab} onChange={onTabChange} aria-label="ant example">
			{filteredTabs.map(tab => (
				<AntTab key={tab.value} label={tab.label} value={tab.value} />
			))}
		</AntTabs>
	);

}

function ViewTabsCargaArchivos({
	indexTab,
	onTabChange,
	siEsAprobadaSolcitud,
}: {
	indexTab: string;
	onTabChange: (event: React.ChangeEvent<any>, newValue: string) => void;
	siEsAprobadaSolcitud: boolean;
}) {
	return (
		<>
			{
				siEsAprobadaSolcitud ? <AntTabs
					value={indexTab}
					onChange={onTabChange}
					aria-label="ant example"
				>
					<AntTab label="Carga Archivos" value={TABS_INDEX.CARGAARCHIVOS} />
				</AntTabs> :
					<AntTabs
						value={indexTab}
						onChange={onTabChange}
						aria-label="ant example"
					>
						<AntTab label="Carga Archivos" value={TABS_INDEX.CARGAARCHIVOS} />
					</AntTabs>
			}
		</>
	);
}

function Panel({
	currentTabValue,
	responseSolicitud,
	formularioInfoInconsistencias,
	documentosAgregados,
	setEstadoSubsanar,
	formularioObservaciones,
	observaciones,
	siEsAprobadaSolcitud,
	deshalitarBotonesInconsistencia,
	setDeshalitarBotonesInconsistencia,
	formulariosSolicitud,
	trazabilidad,
	grupo,
	estadoDeCliente,
	esRadicacionDevueltaPorRechazo,
	onGeneradoExitoso,
	obtnerBoleano
}: {
	currentTabValue: string;
	responseSolicitud: ResponseSeveranceModel | undefined;
	formularioInfoInconsistencias: any;
	documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
	setEstadoSubsanar: (value: boolean) => void;
	formularioObservaciones?: any
	observaciones: string;
	siEsAprobadaSolcitud: boolean;
	deshalitarBotonesInconsistencia: boolean;
	setDeshalitarBotonesInconsistencia: any;
	formulariosSolicitud: FormulariosSolicitudDto | any;
	trazabilidad: any[];
	grupo: any;
	estadoDeCliente: string;
	esRadicacionDevueltaPorRechazo?: boolean;
	onGeneradoExitoso?: () => void;
	obtnerBoleano: any;
}) {
	const [indexTabValue, setTndexTabValue] = useState(
		TABS_INDEX.CARGAARCHIVOS,
	);

	const GUID_DEFAULT = '00000000-0000-0000-0000-000000000000';


	const handleTabChange = (
		event: React.ChangeEvent<any>,
		newValue: string,
	) => {
		setTndexTabValue(newValue);
	};
	const actividadActual = trazabilidad?.find((item: any) =>
		String(item?.idTareaSolicitud || '').toLowerCase() ===
		String(responseSolicitud?.solicitudTareaId || '').toLowerCase()
	);

	const tareaActual = normalizarTexto(
		actividadActual?.descripcionTarea || responseSolicitud?.nombreTarea
	);

	const estadoActual = normalizarTexto(
		actividadActual?.descripcionEstadoSolicitud
	);

	const cargueInconsistenteSoloLectura =
		tareaActual.includes('cargue') &&
		estadoActual === 'inconsistente';


	let formularioPanel = null;

	const existeCargueDocumentosPendiente = trazabilidad?.some((item: any) => {
		const tarea = normalizarTexto(item?.descripcionTarea);
		const estado = normalizarTexto(item?.descripcionEstadoSolicitud);

		return (
			(tarea.includes('cargue de documentos retiro parcial') ||
			 tarea.includes('cargue de documentos devolucion de ahorros avac - avav'))  &&
			 estado === 'pendiente'
		);
	});

	const existeRechazoPrevio = trazabilidad?.some((item: any) => {
		const estado = normalizarTexto(item?.descripcionEstadoSolicitud);

		return estado === 'rechazada';
	});

	const mostrarCartaInconsistencia =
		existeCargueDocumentosPendiente && existeRechazoPrevio;

	if (currentTabValue === TABS_INDEX.RESUMENSOLICITUD) {
		formularioPanel = (
			<>
				<ResumenSolicitud
					responseSolicitud={responseSolicitud}
					observaciones={observaciones}
				/>
				<TrazabilidadSolicitud
					responseSolicitud={responseSolicitud}
					observaciones={observaciones}
				/>
				{mostrarCartaInconsistencia ? (
					<CartaInconsistencia
						responseSolicitud={responseSolicitud}
						setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
						onGeneradoExitoso={onGeneradoExitoso}
					/>
				) : null}
			</>
		);
	}

	else if (
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOS ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSDESAFILIACION
	) {
		formularioPanel = (
			<>
				<ViewTabsCargaArchivos
					indexTab={indexTabValue}
					onTabChange={handleTabChange}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}
				/>

				<PanelCargarArchivo
					currentTabValue={indexTabValue}
					responseSolicitud={responseSolicitud}
					documentosAgregados={documentosAgregados}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}
					grupo={grupo}
					estadoDeCliente={estadoDeCliente}
					trazabilidad={trazabilidad}
					formularioObservaciones={formularioObservaciones}
					soloLecturaCargueDocumentos={cargueInconsistenteSoloLectura}
				/>

				<ObservacionesSolicitud
					control={formularioObservaciones}
					seccion={currentTabValue}
					trazabilidad={trazabilidad}
					soloLecturaVerificacion={cargueInconsistenteSoloLectura}

				/>
			</>
		);
	}
	else if (currentTabValue === TABS_INDEX.VERDETALLESOLICITUD) {

		const tipoAccion = (responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.VINUEVA || responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.CNVGR) ? 0 : 5;
		const { formularios } = formulariosSolicitud;
		formularioPanel = (
			<>


				<DynamicPage
					dynamicTitle={{ topText: "", title: "", icon: "" }}
					formsList={formularios}
					withButtons={!!esRadicacionDevueltaPorRechazo}
					responseSolicitud={responseSolicitud}
					tipoAccion={
						esRadicacionDevueltaPorRechazo
							? BOTONERA_SOLICITUD.ACTUALIZAR
							: tipoAccion
					}
					tipoAccionDetalleProcesar={false}
					modalidadSolicitud={responseSolicitud?.timCodigo as string}
					onItemGoBack={onGeneradoExitoso || (() => { })}
					onItemBack={onGeneradoExitoso || (() => { })}
					obtnerBoleano={obtnerBoleano}
					inicioSolicitud={false}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}
					tipoSolicitud={undefined}
					documentosAgregados={documentosAgregados}
					formularioObservaciones={formularioObservaciones}
				/>
			</>
		);
	}
	else if (currentTabValue === TABS_INDEX.FORMULARIO_INCONSISTENCIAS) {
		formularioPanel = (
			<>
				<FormularioInconsistencias
					formularioInfoInconsistencias={formularioInfoInconsistencias}
					idSolicitud={responseSolicitud?.solicitudId || ''}
					setEstadoSubsanar={setEstadoSubsanar}
				/>
			</>
		);
	}
	else if (currentTabValue === TABS_INDEX.VERIFICACIONESDECUENTA) {
		formularioPanel = (
			<>
				<ObservacionesSolicitud
					control={formularioObservaciones}
					seccion={String(currentTabValue)}
					trazabilidad={trazabilidad || []}
				/>
			</>
		);
	}
	else if (currentTabValue === TABS_INDEX.VERIFICACIONLISTASRESTRICTIVAS) {
		formularioPanel = (
			<>
				<ObservacionesSolicitud
					control={formularioObservaciones}
					seccion={TABS_INDEX.VERIFICACIONLISTASRESTRICTIVAS}
					trazabilidad={trazabilidad}
				/>
			</>
		);
	}
	else if (currentTabValue === TABS_INDEX.HISTORIALINCONSISTENCIA) {
		formularioPanel = (
			<>
				<HistorialInconsistenciaSolicitud responseSolicitud={responseSolicitud} />
			</>
		);
	}
	return formularioPanel;
}

function PanelCargarArchivo({
	documentosAgregados,
	currentTabValue,
	responseSolicitud,
	siEsAprobadaSolcitud,
	grupo,
	estadoDeCliente,
	trazabilidad,
	formularioObservaciones,
	soloLecturaCargueDocumentos

}: {
	documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
	currentTabValue: string;
	responseSolicitud: ResponseSeveranceModel | undefined;
	siEsAprobadaSolcitud: boolean;
	grupo: any;
	estadoDeCliente: any;
	trazabilidad: any[];
	formularioObservaciones: any;
	soloLecturaCargueDocumentos?: boolean;

}) {


	switch (currentTabValue) {
		case TABS_INDEX.CARGAARCHIVOS: {
			const GUID_DEFAULT = '00000000-0000-0000-0000-000000000000';

			const solicitudListaParaCargue =
				!!responseSolicitud &&
				!!responseSolicitud?.solicitudId &&
				responseSolicitud.solicitudId !== GUID_DEFAULT &&
				!!responseSolicitud?.solicitudTareaId &&
				responseSolicitud.solicitudTareaId !== GUID_DEFAULT;

			if (!solicitudListaParaCargue) {
				console.warn(
					'No se monta CargueDocumentosSolicitud porque responseSolicitud está incompleta:',
					responseSolicitud
				);

				return null;
			}

			return (
				<>
					<CargueDocumentosSolicitud
						responseSolicitud={responseSolicitud}
						documentosAgregados={documentosAgregados}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						soloLecturaCargueDocumentos={soloLecturaCargueDocumentos}
					/>
				</>
			);
		}
		case TABS_INDEX.VERIFICACIONESDECUENTA:
			return (
				<>

					<ObservacionesSolicitud
						control={formularioObservaciones}
						seccion={String(currentTabValue)}
						trazabilidad={trazabilidad || []}
					/>
				</>

			);
		default:
			return null;
	}
}

function HistorialInconsisenciaSolicitud({
	responseSolicitud,
	onItemGoBack,
	tipoAccion,
	tipoAccionDetalleProcesar,
	obtnerBoleano,
	siEsAprobadaSolcitud,
	deshalitarBotonesInconsistencia,
	setDeshalitarBotonesInconsistencia,
	formulariosSolicitud,
	grupo,
	estadoDeCliente,
	userIngreso,
	userAsig,
	esRadicacionDevueltaPorRechazo,
	esVerificacionListasRestrictivas
}: {
	responseSolicitud: any | undefined;
	onItemGoBack: () => void;
	tipoAccion: number;
	tipoAccionDetalleProcesar: boolean;
	obtnerBoleano: any | undefined;
	siEsAprobadaSolcitud: boolean;
	deshalitarBotonesInconsistencia: boolean;
	setDeshalitarBotonesInconsistencia: any;
	formulariosSolicitud: FormulariosSolicitudDto | any;
	grupo: any;
	estadoDeCliente: string;
	userIngreso: any;
	userAsig: any;
	esRadicacionDevueltaPorRechazo?: boolean;
	esVerificacionListasRestrictivas?: boolean;
}) {

	const classes = useStyles();
	const api = SolicitudApi();

	const [currentTabValue, setCurrentTabValue] = useState(
		TABS_INDEX.RESUMENSOLICITUD,
	);
	const [loading, setLoading] = useState(false);
	const [primerCargue, setPrimerCargue] = useState(false);
	const [trazabilidad, setTrazabilidad] = useState<any[]>([]);
	const [observaciones, setObservaciones] = useState('');
	const [openModalAnular, setOpenModalAnular] = useState(false);

	const obtenerTrazabilidad = async () => {
		await api.obtenerTrazabilidadSolicitud(responseSolicitud.solicitudId)
			.then((response) => {
				setTrazabilidad(response.data);
			})
	}

	if (responseSolicitud?.solicitudId != null && !primerCargue) {
		setPrimerCargue(true)
		obtenerTrazabilidad();
	}


	useEffect(() => {
		if (
			responseSolicitud?.nombreTarea?.includes(
				'Revisión area jurídica',
			)
		) {
			setDeshalitarBotonesInconsistencia(true);
			setSubsanado(true);
		}

		const actividadActual = trazabilidad?.find(
			(item: any) =>
				String(item?.idTareaSolicitud || '')
					.trim()
					.toLowerCase() ===
				String(
					responseSolicitud?.solicitudTareaId || '',
				)
					.trim()
					.toLowerCase(),
		);

		const observacionTrazabilidad = String(
			actividadActual?.observacion ?? '',
		).trim();

		const observacionFormulario = String(
			formularioObservaciones.getValues(
				'observaciones',
			) ?? '',
		).trim();

		const observacionAMostrar =
			observacionTrazabilidad ||
			observacionFormulario;

		formularioObservaciones.setValue(
			'observaciones',
			observacionAMostrar,
			{
				shouldValidate: false,
				shouldTouch: false,
				shouldDirty: false,
			},
		);

		setObservaciones(
			observacionAMostrar,
		);
	}, [
		trazabilidad,
		responseSolicitud?.solicitudTareaId,
		responseSolicitud?.nombreTarea,
	]);

	const [tipoAccionNumber, setAccionNumber] = useState(BOTONERA_SOLICITUD.ACTUALIZAR);
	const [aprobar, setAprobar] = useState(true);

	const [subsanado, setSubsanado] = useState(true);
	const [estadoSubsanar, setEstadoSubsanar] = useState(false);
	const [titulosIcon, setTitulosIcon] = useState({});



	const handleTabChange = (
		event: React.ChangeEvent<any>,
		newValue: string,
	) => {
		setCurrentTabValue(newValue);
	};

	const formularioInfoInconsistencias = useForm({
		defaultValues: {
			grupoInconsistencia: 0,
			listaInconsistenciaId: '',
			herramientaId: '',
			puntoAtencionId: '',
			solicitudId: responseSolicitud?.solicitudId,
			tecnicoId: '',
			objetoEstudio: '',
			analisis: '',
			observacion: '',
			subsanado: false,
			estado: false,
		},
	});

	useEffect(() => {
		if (
			currentTabValue !==
			TABS_INDEX.FORMULARIO_INCONSISTENCIAS
		) {
			return;
		}

		if (
			formularioInfoInconsistencias.getValues(
				'subsanado',
			)
		) {
			setAprobar(false);
			setSubsanado(true);
		} else {
			setAprobar(true);
			setSubsanado(false);
		}
	}, [
		estadoSubsanar,
		currentTabValue,
	]);

	const [documentosAguardar, setDocumentosAguardar] = useState<any>([]);
	const [numeroFolio, setNumeroFolio] = useState('');

	const [accionActual, setAccionActual] = useState<
		'ACTUALIZAR' | 'APROBAR' | 'RECHAZAR' | 'ANULAR' | null
	>(null);


	const documentosAgregados = (documentos: any,): void => {
		setDocumentosAguardar(documentos);
	};

	const onCloseAnular = () => setOpenModalAnular(false);

	const esTabCargueDocumentos =
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOS ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSDESAFILIACION ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOACREDITACION;

	const esTabVerificacionListasRestrictivas =
		currentTabValue ===
		TABS_INDEX.VERIFICACIONLISTASRESTRICTIVAS;

	const esTareaVerificacionListasRestrictivas =
		normalizarTexto(
			responseSolicitud?.nombreTarea,
		).includes(
			'verificacion de listas restrictivas',
		);

	const esFlujoVerificacionListasRestrictivas =
		esTabVerificacionListasRestrictivas &&
		esTareaVerificacionListasRestrictivas;

	useEffect(() => {
		if (!esFlujoVerificacionListasRestrictivas) {
			return;
		}

		setAprobar(true);
	}, [
		responseSolicitud?.solicitudTareaId,
		esFlujoVerificacionListasRestrictivas,
	]);

	const obtenerNombreDocumento = (documento: any): string =>
		String(
			documento?.nombreArchivo ||
			documento?.asC_NOMBRE_ARCHIVO ||
			documento?.description ||
			'Documento requerido'
		);


	const esDocumentoRequerido = (documento: any): boolean => {
		const valor = String(documento?.esRequerido ?? '')
			.trim()
			.toLowerCase();

		return (
			documento?.esRequerido === true ||
			documento?.esRequerido === 1 ||
			valor === 'true' ||
			valor === 'si' ||
			valor === 'sí'
		);
	};

	// const validarDocumentosDodoVerificados = (): boolean => {
	// 	if (!esTabCargueDocumentos) {
	// 		return true;
	// 	}

	// 	if (!Array.isArray(documentosAguardar) || documentosAguardar.length === 0) {
	// 		toast('Debe verificar los documentos requeridos en Dodo antes de actualizar.', {
	// 			type: toast.TYPE.ERROR,
	// 		});
	// 		return false;
	// 	}

	// 	const documentosRequeridos = documentosAguardar.filter(esDocumentoRequerido);

	// 	const documentosRequeridosNoVerificados = documentosRequeridos.filter(
	// 		(documento: any) => !documento?.verificado
	// 	);

	// 	if (documentosRequeridosNoVerificados.length > 0) {
	// 		documentosRequeridosNoVerificados.forEach((documento: any) => {
	// 			toast(
	// 				`Documento requerido pendiente por verificar en Dodo: ${obtenerNombreDocumento(documento)}`,
	// 				{ type: toast.TYPE.ERROR }
	// 			);
	// 		});

	// 		return false;
	// 	}

	// 	return true;
	// };

	const validarDocumentosDodoVerificados = (): boolean => {
		if (accionActual === 'ANULAR') {
			return true;
		}

		if (!esTabCargueDocumentos) {
			return true;
		}

		if (!Array.isArray(documentosAguardar) || documentosAguardar.length === 0) {
			toast('Debe verificar los documentos requeridos en Dodo antes de actualizar.', {
				type: toast.TYPE.ERROR,
			});
			return false;
		}

		const documentosRequeridos = documentosAguardar.filter(esDocumentoRequerido);

		const documentosRequeridosNoVerificados = documentosRequeridos.filter(
			(documento: any) => !documento?.verificado
		);

		if (documentosRequeridosNoVerificados.length > 0) {
			documentosRequeridosNoVerificados.forEach((documento: any) => {
				toast(
					`Documento requerido pendiente por verificar en Dodo: ${obtenerNombreDocumento(documento)}`,
					{ type: toast.TYPE.ERROR }
				);
			});

			return false;
		}

		return true;
	};

	const onSubmit2 = async (): Promise<boolean> => {
		setLoading(true);
		setAprobar(true);

		try {
			const informacionObservaciones =
				formularioObservaciones.getValues();

			const observacionActual = String(
				informacionObservaciones?.observaciones ?? '',
			).trim();

			if (!observacionActual) {
				toast(
					'Por favor ingrese alguna observación',
					{
						type: toast.TYPE.ERROR,
					},
				);

				return false;
			}

			const esFlujoVerificacionNovedades =
				currentTabValue ===
				TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL &&
				trazabilidad?.some((item: any) =>
					normalizarTexto(
						item?.descripcionTarea,
					).includes(
						'verificacion novedades',
					),
				);

			if (esFlujoVerificacionNovedades) {
				const novedades = trazabilidad?.find(
					(item: any) =>
						normalizarTexto(
							item?.descripcionTarea,
						).includes(
							'verificacion novedades',
						),
				);

				const observacionNovedadesVacia =
					!String(
						novedades?.observacion ?? '',
					).trim();

				if (
					!observacionActual &&
					observacionNovedadesVacia
				) {
					toast(
						'Por favor ingrese alguna observación',
						{
							type: toast.TYPE.ERROR,
						},
					);

					return false;
				}
			}

			const responseActualizar =
				await api.actualizarObservaciones({
					numeroFolio,
					idSolicitudTarea:
						responseSolicitud?.solicitudTareaId,
					observaciones: observacionActual,
				});

			if (
				responseActualizar?.isSuccess === false
			) {
				toast(
					responseActualizar?.returnMessage ||
					'No fue posible actualizar la observación',
					{
						type: toast.TYPE.ERROR,
					},
				);

				return false;
			}

			if (esTabCargueDocumentos) {
				const documentosOk =
					validarDocumentosDodoVerificados();

				if (!documentosOk) {
					return false;
				}

				const documentosVerificados =
					documentosAguardar
						.filter(
							(documento: any) =>
								documento?.verificado,
						)
						.map(
							(documento: any) =>
								documento?.nombreArchivo ||
								documento?.asC_NOMBRE_ARCHIVO,
						)
						.filter(Boolean);

				if (
					documentosVerificados.length === 0
				) {
					toast(
						'Debe verificar al menos un documento en Dodo antes de actualizar.',
						{
							type: toast.TYPE.ERROR,
						},
					);

					return false;
				}

				const request = {
					idSolicitud:
						responseSolicitud?.solicitudId,
					documentosVerificados,
					numeroFolio,
				};

				const responseDocumentos =
					await api.PostGuardarDocumentosDodoVerificados(
						request,
					);

				if (
					responseDocumentos?.isSuccess ===
					false
				) {
					toast(
						responseDocumentos?.returnMessage ||
						'No fue posible guardar la verificación de documentos.',
						{
							type: toast.TYPE.ERROR,
						},
					);

					return false;
				}
			}

			if (
				currentTabValue ===
				TABS_INDEX.FORMULARIO_INCONSISTENCIAS
			) {
				const inconsistencia = {
					grupoInconsistencia:
						formularioInfoInconsistencias.getValues(
							'grupoInconsistencia',
						),
					listaInconsistenciaId:
						formularioInfoInconsistencias.getValues(
							'listaInconsistenciaId',
						),
					herramientaId:
						formularioInfoInconsistencias.getValues(
							'herramientaId',
						),
					puntoAtencionId:
						formularioInfoInconsistencias.getValues(
							'puntoAtencionId',
						),
					solicitudId:
						responseSolicitud?.solicitudId ||
						'',
					tecnicoId:
						formularioInfoInconsistencias.getValues(
							'tecnicoId',
						),
					objetoEstudio:
						formularioInfoInconsistencias.getValues(
							'objetoEstudio',
						),
					analisis:
						formularioInfoInconsistencias.getValues(
							'analisis',
						),
					observacion:
						formularioInfoInconsistencias.getValues(
							'observacion',
						),
				};

				if (
					inconsistencia.listaInconsistenciaId !==
					''
				) {
					await api.PostInsertarInconsistenciaSolicitud(
						inconsistencia,
					);

					toast(
						'Inconsistencia guardada con éxito',
						{
							type: toast.TYPE.SUCCESS,
						},
					);
				}
			}

			formularioObservaciones.reset(
				{
					...formularioObservaciones.getValues(),
					observaciones: observacionActual,
				},
				{
					keepDirty: false,
					keepTouched: false,
					keepErrors: false,
				},
			);

			if (!esTabCargueDocumentos) {
				await obtenerTrazabilidad();
			}

			setAprobar(false);

			if (esTabCargueDocumentos) {
				toast(
					'Información actualizada correctamente.',
					{
						type: toast.TYPE.SUCCESS,
					},
				);
			} else {
				toast(
					esFlujoVerificacionListasRestrictivas
						? 'Información actualizada correctamente. Ya puede aprobar la verificación.'
						: 'Información actualizada correctamente',
					{
						type: toast.TYPE.SUCCESS,
					},
				);
			}

			return true;
		} catch (error) {
			console.error(
				'[ERROR AL ACTUALIZAR INFORMACIÓN]',
				error,
			);

			setAprobar(true);

			toast(
				`Error del sistema al actualizar la información: ${error}`,
				{
					type: toast.TYPE.ERROR,
				},
			);

			return false;
		} finally {
			setLoading(false);
		}
	};

	// const normalizarTexto = (texto: any) =>
	// 	String(texto || '')
	// 		.trim()
	// 		.toLowerCase()
	// 		.normalize('NFD')
	// 		.replace(/[\u0300-\u036f]/g, '');

	const esFlujoCargueDocumentos =
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOS ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSV14 ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSV8 ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOSDESAFILIACION ||
		currentTabValue === TABS_INDEX.CARGUEDOCUMENTOACREDITACION;

	const esTareaCargueDocumentos =
		normalizarTexto(responseSolicitud?.nombreTarea).includes('cargue de documentos');

	const debeAprobarPorApiDocumentos =
		esFlujoCargueDocumentos && esTareaCargueDocumentos;

	const onSubmitAprobar = async () => {

		setAccionActual('APROBAR');

		setLoading(true)
		const informacionObservaciones = formularioObservaciones.getValues();

		if (
			currentTabValue ===
			TABS_INDEX.VERIFICACIONCUENTAINDIVIDUAL &&
			trazabilidad?.some((item: any) =>
				normalizarTexto(
					item?.descripcionTarea,
				).includes(
					'verificacion novedades',
				),
			)
		) {
			const novedades = trazabilidad?.find(
				(item: any) =>
					normalizarTexto(
						item?.descripcionTarea,
					).includes(
						'verificacion novedades',
					),
			);

			const observacionNovedadesVacia =
				!String(
					novedades?.observacion ?? '',
				).trim();

			if (observacionNovedadesVacia) {
				toast(
					'Por favor ingrese alguna observación',
					{
						type: toast.TYPE.ERROR,
					},
				);

				setLoading(false);
				return;
			}
		}

		if (informacionObservaciones?.observaciones == null || informacionObservaciones?.observaciones === '') {
			toast('Por favor ingrese alguna observación', { type: toast.TYPE.ERROR });
			setLoading(false)
			return;
		}

		if (esTabCargueDocumentos) {
			const documentosOk = validarDocumentosDodoVerificados();

			if (!documentosOk) {
				setAprobar(true);
				setLoading(false);
				return;
			}
		}

		await api.actualizarObservaciones({
			idSolicitudTarea: responseSolicitud?.solicitudTareaId,
			observaciones: informacionObservaciones?.observaciones
		})

		const confirmAlert = await NotificationService.confirmAlert(
			'¿Está seguro que desea continuar?',
		);

		if (confirmAlert) {
			try {
				const aprobarSolicitudTareaDto = {
					solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
					clienteId: responseSolicitud?.clienteId as number
				};

				const response = debeAprobarPorApiDocumentos
					? await api.PostAprobarSolicitudTareaDocumentos(aprobarSolicitudTareaDto)
					: await api.PostAprobarSolicitudTarea(aprobarSolicitudTareaDto);

				if (response) {
					if (response.isSuccess) {
						toast('Verificación Exitosa', { type: toast.TYPE.SUCCESS });
						setAprobar(true);
						obtnerBoleano(true);
						onItemGoBack();
						setLoading(false);
						history.push('/GestionSolicitudes');
					} else {
						toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
						setLoading(false);
					}
				} else {
					toast('No se recibió respuesta del servidor.', { type: toast.TYPE.ERROR });
					setLoading(false);
				}
			} catch (e) {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
				setLoading(false);
			}
		} else {
			setLoading(false);
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

				await api.actualizarObservaciones({
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

	const funcionAnular = async () => {
		try {
			const solicitudTareaDto = {
				solicitudTareaId: responseSolicitud?.solicitudTareaId,
			};

			const responseAnular = await api.PostAnularSolicitudTarea(solicitudTareaDto);

			if (responseAnular && responseAnular.isSuccess) {
				toast('Se anuló la solicitud correctamente.', {
					type: toast.TYPE.SUCCESS
				});

				onItemGoBack();
				return;
			}

			toast(
				responseAnular?.returnMessage || 'No fue posible anular la solicitud.',
				{ type: toast.TYPE.ERROR }
			);
		}
		catch (e: any) {
			toast('Error al anular solicitud cargue de documentos', {
				type: toast.TYPE.ERROR
			});
		}
	};


	const onSubmitRechazar = () => {
		swal({
			title: 'Seleccione una opción',
			text: 'Seleccione si desea rechazar o anular la actividad',
			icon: 'warning',
			buttons: {
				rechazar: {
					text: 'Rechazar',
					value: 'rechazar',
					className: classes.buttonRechazar
				},
				anular: {
					text: 'Anular',
					value: 'anular',
					className: classes.buttonAnular
				},
				cancelar: {
					text: 'Cancelar',
					value: null,
					className: classes.buttonCancelar
				},
			},
			closeOnClickOutside: false,
		})
			.then(async respuesta => {
				if (!respuesta) {
					return;
				}

				const informacionObservaciones = formularioObservaciones.getValues();

				const observacionActual = String(
					informacionObservaciones?.observaciones || ''
				).trim();

				if (!observacionActual) {
					toast('Por favor ingrese alguna observación', {
						type: toast.TYPE.ERROR
					});
					setLoading(false);
					return;
				}

				const tieneVerificacionNovedades = trazabilidad?.some((item: any) =>
					String(item?.descripcionTarea).includes('Verificación novedades')
				);

				if (tieneVerificacionNovedades) {
					const novedades = trazabilidad?.find((item: any) =>
						String(item?.descripcionTarea).includes('Verificación novedades')
					);

					const observacionNovedadesVacia =
						novedades &&
						(
							novedades?.observacion === null ||
							novedades?.observacion === ''
						);

					if (observacionNovedadesVacia) {
						toast('Por favor ingrese alguna observación', {
							type: toast.TYPE.ERROR
						});
						setLoading(false);
						return;
					}
				}

				const responseActualizarObservacion = await api.actualizarObservaciones({
					idSolicitudTarea: responseSolicitud?.solicitudTareaId,
					observaciones: observacionActual
				});

				if (responseActualizarObservacion?.isSuccess === false) {
					toast(
						responseActualizarObservacion?.returnMessage ||
						`No fue posible guardar la observación antes de ${respuesta === 'anular' ? 'anular' : 'rechazar'
						}.`,
						{ type: toast.TYPE.ERROR }
					);
					setLoading(false);
					return;
				}

				const solicitudTareaDto = {
					solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
				};

				if (respuesta === 'anular') {
					const nombreTareaNormalizado = normalizarTexto(responseSolicitud?.nombreTarea);

					const requiereModalCausalAnular =
						nombreTareaNormalizado.includes('cargue de documentos') &&
						nombreTareaNormalizado.includes('v 8');

					if (requiereModalCausalAnular) {
						setOpenModalAnular(true);
						return;
					}

					await funcionAnular();
					return;
				}

				if (respuesta === 'rechazar') {
					const responseRechazar = await api.PostRechazarSolicitudTarea(
						solicitudTareaDto
					);

					if (responseRechazar && responseRechazar.isSuccess) {
						toast('Se rechazó la solicitud correctamente.', {
							type: toast.TYPE.SUCCESS
						});

						onItemGoBack();
						return;
					}

					toast(
						responseRechazar?.returnMessage ||
						'No fue posible rechazar la solicitud.',
						{ type: toast.TYPE.ERROR }
					);

					setLoading(false);
				}
			});
	};

	const formularioObservaciones = useForm({
		defaultValues: {
			idSolicitud: responseSolicitud?.solicitudTareaId,
			observaciones: '',
			// causal en caso de que se anule
			causal: '0'
		}
	})

	const observacionActual =
		formularioObservaciones.watch('observaciones');

	const observacionFueModificada =
		formularioObservaciones.formState.isDirty;

	useEffect(() => {
		if (!esFlujoVerificacionListasRestrictivas) {
			return;
		}

		if (observacionFueModificada) {
			setAprobar(true);
		}
	}, [
		observacionActual,
		observacionFueModificada,
		esFlujoVerificacionListasRestrictivas,
	]);

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
		} else if (responseSolicitud?.descripcionProceso === "SOLICITUD DE SUBSIDIO") {
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
				title1: "Subsidio Secuestrados ",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS') {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Devolución Parcial de Cesantías",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS') {
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Devolución Parcial de Cesantías",
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

	if (loading) {
		return <CHLoadingProcess />
	}

	const ocultarBotoneraExterna =
		esRadicacionDevueltaPorRechazo &&
		currentTabValue === TABS_INDEX.VERDETALLESOLICITUD;

	return (
		<>
			<CHPageTitleV2   {...titulosIcon} />
			<ViewTabs
				currentTab={currentTabValue}
				onTabChange={handleTabChange}
				responseSolicitud={responseSolicitud}
				userIngreso={userIngreso}
				userAsig={userAsig}
				esRadicacionDevueltaPorRechazo={esRadicacionDevueltaPorRechazo}
				esVerificacionListasRestrictivas={esVerificacionListasRestrictivas}
			/>
			<Panel
				responseSolicitud={responseSolicitud}
				currentTabValue={currentTabValue}
				documentosAgregados={documentosAgregados}
				formularioInfoInconsistencias={formularioInfoInconsistencias}
				setEstadoSubsanar={setEstadoSubsanar}
				formularioObservaciones={formularioObservaciones}
				observaciones={observaciones as string}
				siEsAprobadaSolcitud={siEsAprobadaSolcitud}
				deshalitarBotonesInconsistencia={deshalitarBotonesInconsistencia}
				setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
				formulariosSolicitud={formulariosSolicitud}
				trazabilidad={trazabilidad}
				grupo={grupo}
				estadoDeCliente={estadoDeCliente}
				esRadicacionDevueltaPorRechazo={esRadicacionDevueltaPorRechazo}
				onGeneradoExitoso={onItemGoBack}
				obtnerBoleano={obtnerBoleano}

			/>
			{!ocultarBotoneraExterna && (
				<BotoneraInformacionSolicitudComponent
					onItemGoBack={onItemGoBack}
					tipoAccion={tipoAccionNumber}
					onSubmitAprobar={onSubmitAprobar}
					onSubmitSubsanar={onSubmitSubsanar}
					onSubmitRechazar={onSubmitRechazar}
					onSubmitGenerarPago={null}
					onSubmit={onSubmit2}
					disabledActualizar={false}
					disabledAprobar={aprobar}
					disabledSubsanar={subsanado}
					disabledRechazar={!!esRadicacionDevueltaPorRechazo}
					disabledRegresar={false}
					tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
					responseSolicitud={responseSolicitud}
					currentTabValue={currentTabValue}
					aprobarBloquear={undefined}
					hablitarActualiza={undefined}
					deshabilitarBTNRechazarGp2={undefined}
					deshalitarBotonesInconsistencia={deshalitarBotonesInconsistencia}
					userAsig={userAsig}
					grupo={grupo}
					userIngreso={userIngreso}
					requiereActualizarParaAprobar={esRadicacionDevueltaPorRechazo}
					handleSubmit={formularioObservaciones.handleSubmit}
				/>
			)}
			<ModalCausalAnularSolicitud
				open={openModalAnular}
				formulario={formularioObservaciones}
				onClose={onCloseAnular}
				funcionAnular={funcionAnular}
			/>
		</>
	);
}
export default HistorialInconsisenciaSolicitud;