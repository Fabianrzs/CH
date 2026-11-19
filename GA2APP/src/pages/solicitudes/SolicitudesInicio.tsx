import React, { useEffect, useState } from 'react';
import {
	Button,
	createStyles,
	Grid,
	makeStyles,
	MenuItem,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableRow,
	TableHead,
	TablePagination
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { useParams } from 'react-router-dom';
import { AppState } from '../../store/rootReducer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import {
	startGetAllTypeRequest,
	startGetAllStatesRequest,
} from '../../actions/catalog.action';
import { ESTADOTAREADTO, TYPEREQUEST } from '../../types/catalog.type';
import {
	ParametrosBusqueda,
	SolicitudDto,
} from '../../models/requests/solicitud-tarea.model';
import {
	startPostConsultarSolicitudes,
	getFormulariosSolicitud,
	documentosAgregados
} from '../../actions/solicitud.action'
import UtilService from '../../utils/util.service';
import { ResponseSeveranceModel } from '../../models/requests/response-severance.model';
import NotificationService from '../../services/notification.service';
import { BOTONERA_SOLICITUD, ESTADO_SOLICITUD, GUID_DEFAULT } from '../../constants/constants';
import DynamicPage from './dynamicPage/DynamicPage.component';
import InformacionProcesoSolicitud from './informacionProcesoSolicitud/informacionProcesoSolicitud';
import apiSolicitud from '../../services/api-solicitud.service';
import { ConsultarSolicitudModel } from '../../models/requests/consultar-solicitud.model';
import ProcesoSeguridadDocumental from './procesoSeguridadDocumental/procesoSeguridadDocumental';
import ProcesoVerificacionEmbargo from './procesoVerificacionEmbargos/procesoVerificacionEmbargos';
import CHPageTitleV2 from '../../components/ch-page-title/CHPageTitleV2';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CatalogApi from '../../services/api-catalog.service';
import Solicitudes from './solicitudes';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import AuthService from '../../services/auth.service';
import ModalSolicitanteBuscar from './components/modalesSolicitudInicio/ModalSolicitanteBuscar';
import ModalUsuarioIngresoBuscar from './components/modalesSolicitudInicio/ModalUsuarioIngresoBuscar';
import { LoadingComponent } from '../../components/loading/loading.component';
import ProcesoRadicacion from './procesoRadicacion/procesoRadicacion';
import GrupoUsers from '../../services/api-grupoUsuarios.service';
import ApiClients from '../../services/api-clients.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { Auditoria } from '../../types/Auditoria.model.type';
import { useIp } from '../../shared/context/IpContext';

const useStyles = makeStyles((theme) =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '400px',
			marginLeft: '.5rem',
		},
		buttonRegresar: {
			width: '120px',
		},
		processButton: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
		},
		margin: {
			marginLeft: '-100px',
			marginRight: '-90px'
		},
		marginBotonRegresar: {
			textAlign: 'end',
			marginLeft: '-90px',
			marginRight: '-90px'
		},
		root: {
			width: '100%',
			marginTop: theme.spacing(3),
			overflowX: 'auto',
		},
		container: {
			maxHeight: 920,
		},
		tableHeader: {
			backgroundColor: '#FEC917',
			borderBottom: '1px solid #e0e0e0',
			textAlign: 'center',
		},
		alternateRow: {
			backgroundColor: '#D3D3D3',
		},

	}));

const SolicitudesTable = ({ data, onClickItem, grupo }: { data: any; onClickItem: any, grupo: any }) => {
	const user = AuthService.getUserInfo();
	const classes = useStyles();

	const [page, setPage] = useState(0);
	const [rowsPerPage, setRowsPerPage] = useState(10);

	const handleChangePage = (event: any, newPage: any) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event: any) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	return data && data.length > 0 && (
		<>
			<br />
			<Paper className={classes.root}>
				<TableContainer className={classes.container}>
					<Table stickyHeader aria-label="sticky table">
						<TableHead>
							<TableRow>
								<TableCell className={classes.tableHeader}>Proceso</TableCell>
								<TableCell className={classes.tableHeader}>No. Solicitud</TableCell>
								<TableCell className={classes.tableHeader}>No. Radicado</TableCell>
								<TableCell className={classes.tableHeader}>Tipo de Solicitud</TableCell>
								<TableCell className={classes.tableHeader}>Tipo Modalidad</TableCell>
								<TableCell className={classes.tableHeader}>Tipo Sub - modalidad</TableCell>
								<TableCell className={classes.tableHeader}>No. Identificación</TableCell>
								<TableCell className={classes.tableHeader}>Nombre / Razón Social</TableCell>
								<TableCell className={classes.tableHeader}>Fecha / Hora</TableCell>
								<TableCell className={classes.tableHeader}>Valor</TableCell>
								<TableCell className={classes.tableHeader}>Actividad Actual</TableCell>
								<TableCell className={classes.tableHeader}>Asignado</TableCell>
								<TableCell className={classes.tableHeader}>Estado</TableCell>
							</TableRow>
						</TableHead>
						<TableBody>
							{(rowsPerPage > 0
								? data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
								: data
							).map((row: any, index: any) => {

								const isContinuar = ['PENDIENTE', 'EN_SUBSANACION', 'BLOQUEADO', 'EN TRÁMITE', 'EN VERIFICACION', 'INCONSISTENTE'].includes(
									row.estadoSolicitud
								);
								const gruposPermitidos = [
									'ASESOR_PTO',
									'ASESOR_PUNTO_ATENCION',
									'JEFE OFICINA ASESORA',
									'ASERA_J',
									'ASERA_P',
									'ASERA_T',
									'ASERA_A'
								];

								const isEstadoPermitido = [
									'APROBADA',
									'INCONSISTENTE',
									'RECHAZADO',
									'BLOQUEADO',
									'PENDIENTE',
									'EN TRÁMITE',
									'EN VERIFICACION'
								].includes(row.estadoSolicitud);

								const isUsuarioAsignado = row?.idUsuarioAsignado === user.USR_ID;
								const isUsuarioGrupoPermitido = gruposPermitidos.includes(grupo);

								const isUsuarioPermitido = isUsuarioAsignado || isUsuarioGrupoPermitido;

								const isEnVerificacion = row.estadoSolicitud === 'EN VERIFICACION';

								const isTareaVerificacionEmbargos =
									row.tareaNombre === 'Verificación embargos retiro parcial';

								const esTareaListasRestrictivas =
									String(row?.tareaNombre || '')
										.toLowerCase()
										.normalize('NFD')
										.replace(/[\u0300-\u036f]/g, '')
										.includes('verificacion de listas restrictivas');

								const tareaSinAsignar =
									row?.idUsuarioAsignado === null ||
									row?.idUsuarioAsignado === undefined ||
									row?.idUsuarioAsignado === '';

								const normalizarTexto = (texto: any) =>
									String(texto || '')
										.toLowerCase()
										.normalize('NFD')
										.replace(/[\u0300-\u036f]/g, '')
										.trim();

								const esCartaInconsistenciaGenerada =
									normalizarTexto(row?.tareaNombre).includes('carta de inconsistencia') ||
									normalizarTexto(row?.estadoSolicitud).includes('inconsistente');

								const puedeContinuarProceso =
									isContinuar &&
									isEstadoPermitido &&
									!esCartaInconsistenciaGenerada &&
									!row?.tareaNombre.includes('Revisión area jurídica') &&
									(
										isUsuarioAsignado ||
										(
											tareaSinAsignar &&
											isUsuarioGrupoPermitido &&
											esTareaListasRestrictivas
										)
									);

								const buttonText = puedeContinuarProceso ? 'Continuar' : 'Detalle';


								return (
									<TableRow key={index} className={index % 2 === 0 ? classes.alternateRow : ''}>
										<TableCell>

											<Button
												className={classes.processButton}
												onClick={() => {
													if (puedeContinuarProceso) {
														onClickItem(row);
														return;
													}

													if (isUsuarioPermitido || isUsuarioGrupoPermitido) {
														onClickItem(row);
														return;
													}

													NotificationService.genericAlert(
														'Advertencia',
														'No tiene permisos para realizar esta acción.'
													);
												}}
												disabled={
													!isUsuarioPermitido &&
													!isUsuarioGrupoPermitido &&
													!(
														isTareaVerificacionEmbargos &&
														isUsuarioGrupoPermitido &&
														isEstadoPermitido
													)
												}
											>
												{buttonText}
											</Button>
										</TableCell>
										<TableCell>{row.numeroSolicitud}</TableCell>
										<TableCell>{row.radicadoGestionDocumental}</TableCell>
										<TableCell>
											{row.tpsSubDescripcion === 'PRIMER PAGO' && row.tareaNombre.includes('V 14')
												? 'SOLICITUD VIVIENDA 14'
												: row.procesoDescripcion}
										</TableCell>
										<TableCell>{row.modalidadDescripcion}</TableCell>
										<TableCell>{row.tpsSubDescripcion}</TableCell>
										<TableCell>{row.cedulaCliente}</TableCell>
										<TableCell>{row.nombreCliente}</TableCell>
										<TableCell >
											{new Date(row.fechaSolicitud).toLocaleDateString('es-ES')} {' '}
											{new Date(row.fechaSolicitud).toLocaleTimeString('en-US', {
												hour: 'numeric',
												minute: 'numeric',
												hour12: true
											})}
										</TableCell>
										<TableCell>{UtilService.formatCurrency(row.valorRetirar)}</TableCell>
										<TableCell>{row.tareaNombre}</TableCell>
										<TableCell>{row.usuarioAsignado !== null ? row.usuarioAsignado : 'Sin Asignación'}</TableCell>
										<TableCell>{row.estadoSolicitud}</TableCell>
									</TableRow>
								)


							})}
						</TableBody>
					</Table>
				</TableContainer>
				<TablePagination
					rowsPerPageOptions={[10, 25, 50]}
					component="div"
					count={data.length}
					rowsPerPage={rowsPerPage}
					page={page}
					onPageChange={handleChangePage}
					onRowsPerPageChange={handleChangeRowsPerPage}
					labelRowsPerPage="Filas por página:"
				/>

			</Paper>
		</>
	);
};

const SolicitudesInicio = (props: Props) => {
	const catalogoApi = CatalogApi();
	const user = AuthService.getUserInfo();
	const ApiUsers = GrupoUsers();
	const ApiClientes = ApiClients();

	const userIngreso = `${user.USR_NOMBRE} ${user.USR_SEGUNDONOMBRE} ${user.USR_APELLIDO} ${user.USR_SEGUNDOAPELLIDO}`;

	const {
		estadosTareas,
		tipoSolicitudes,
		solicitudState,
		startGetAllTypeRequest,
		startGetAllStatesRequest,
		startPostConsultarSolicitudes,
		getFormulariosSolicitud,
		documentosAgregados
	} = props;
	const classes = useStyles();
	const api = apiSolicitud();
	const auditoriaApi = AuditoriaApi();
	const [selectedData, setSelectedData] = useState<any | null>(null);
	const [iniciarPantalla, setIniciarPantalla] = useState<string>('INICIO');
	const [tipoAccion, setTipoAccion] = useState<number>(BOTONERA_SOLICITUD.GUARDAR);
	const [responseSolicitud, setResponseSolicitud] = useState<any>([]);
	const [listaModalidades, setListaModalidades] = useState<any[]>([]);
	const [listaSubModalidades, setListaSubModalidades] = useState<any[]>([]);
	const [obtnerSolicitud, setObtnerSolicitud] = useState('');
	const [mostrarPantallaSolicitud, setMostrarPantallaSolicitud] = useState(false);
	const [mostrarPantallaSolicitudInicio, setMostrarPantallaSolicitudInicio] = useState(true);
	const [cedulaUsuario, setCedulaUsuario] = useState('');
	const [siEsAprobadaSolcitud, setSiEsAprobadaSolcitud] = useState(false);
	const [tipoAccionDetalleProcesar, setTipoProcesar] = useState<boolean>(false);
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [loading, setLoading] = useState(false);
	const [mostrarTabla, setMostrarTabla] = useState<boolean>(false);
	const [mensajesEjecutados, setMensajesEjecutados] = useState(false);
	const [deshalitarBotonesInconsistencia, setDeshalitarBotonesInconsistencia] = useState<boolean>(false);
	const [identificacionSolicitante, setIdentificacionSolicitante] = React.useState('');
	const [open, setOpen] = React.useState(false);
	const [openUsuario, setOpenUsuario] = React.useState(false);
	const [trazabilidad, setTrazabilidad] = useState<any>(null);
	const { radicado }: any = useParams();

	const [grupo, setGrupo] = useState(null);

	useEffect(() => {
		startGetAllTypeRequest(2);
		startGetAllStatesRequest();
	}, []);


	useEffect(() => {
		const obtenerGrupo = async () => {
			try {
				const grupos = await ApiUsers.ObtenerGruposUsuarioPorId(user.USR_ID);
				if (grupos?.data?.length > 0) {
					setGrupo(grupos.data[0]?.grupo);
				} else {
					setGrupo(null);
				}
			} catch (error) {
				console.error("Error al obtener los grupos:", error);
				setGrupo(null);
			}
		};

		obtenerGrupo();
	}, [user.USR_ID]);


	useEffect(() => {
		if (radicado !== '' && radicado !== undefined) {
			setValue('radicado', radicado)

			const request = {
				estadoTarea: 0,
				fechaFin: null,
				fechaIni: null,
				identificacion: "",
				modalidad: null,
				submodalidad: null,
				idUsuario: user.USR_ID,
				numRadicado: radicado,
				tipoSolicitud: null,
				numeroSolicitud: "",
				usuarioIngreso: "",
			};
			startPostConsultarSolicitudes(request, 'SI');
			setMostrarTabla(true)
		}
		else {

			const identidad = cedulaUsuario;
			const request = {
				estadoTarea: 0,
				fechaFin: null,
				fechaIni: null,
				identificacion: identidad ? cedulaUsuario : "1",
				modalidad: null,
				submodalidad: null,
				idUsuario: user.USR_ID,
				numRadicado: radicado !== undefined ? radicado : '',
				tipoSolicitud: null,
				numeroSolicitud: "",
				usuarioIngreso: "",
			};
			startPostConsultarSolicitudes(request, 'NO');
			setMostrarTabla(true)
		}
	}, []);


	useEffect(() => {
		if (obtnerSolicitud !== '0') {
			obtenerModalidadPorSolicitud();
			setValue('modalidad', '0')
		} else {
			setValue('modalidad', '0')
			setListaModalidades([])
		}

	}, [obtnerSolicitud]);


	const { control, handleSubmit, setValue, getValues, reset } = useForm({
		defaultValues: {
			radicado: '',
			identificacion: '',
			estadoTarea: 0,
			tipoSolicitud: '0',
			modalidad: '0',
			submodalidad: '0',
			fechaModalidadInicio: null,
			fechaModalidadFin: null,
			numeroSolicitud: "",
			solicitante: "",
			usuarioIngreso: ""
		},
	});


	const tieneAlgunFiltro = (data: any): boolean =>
		Boolean(
			data.radicado?.trim() ||
			data.identificacion?.trim() ||
			(data.estadoTarea !== 0) ||
			data.tipoSolicitud !== "0" ||
			data.modalidad !== "0" ||
			data.submodalidad !== "0" ||
			data.fechaModalidadInicio !== null ||
			data.fechaModalidadFin !== null ||
			data.numeroSolicitud?.trim() ||
			data.solicitante?.trim() ||
			data.usuarioIngreso?.trim()
		);


	const limpiarCampos = () => {
		setIdentificacionSolicitante('');
		setListaSubModalidades([]);
		reset();
	}

	const consultarTrazabilidad = (solicitudId: string) => {
		api.obtenerTrazabilidadSolicitud(solicitudId)
			.then((response) => {
				setTrazabilidad(response?.data);
			}).catch((e) => {
				toast(`Error al consultar trazabilidad ${e.message}`, { type: toast.TYPE.ERROR });
			});
	}

	const buscarSolicitante = (datos: any) => {
		setLoading(true)
		if (datos.nombreS_SOLICITANTE !== undefined && datos.apellidoS_SOLICITANTE !== undefined && datos?.nombreS_SOLICITANTE !== " ") {
			setValue('solicitante', `${datos?.nombreS_SOLICITANTE}  ${datos?.apellidoS_SOLICITANTE}`);
			setIdentificacionSolicitante(datos?.identificacioN_SOLICITANTE);
			setLoading(false);
			setOpen(false);
		} else if (datos?.razoN_SOCIAL_TERCERO !== '') {
			setValue('solicitante', `${datos?.razoN_SOCIAL_TERCERO}`);
			setIdentificacionSolicitante(datos?.identificacioN_SOLICITANTE);
			setLoading(false);
			setOpen(false);
		}
		else {
			setValue('solicitante', "");
			setIdentificacionSolicitante('');
			setLoading(false);
			setOpen(false);
		}
	}

	const buscarUsuarioIngreso = (datos: any) => {
		setLoading(true)
		if (datos.nombreS_USUARIO !== undefined && datos.apellidoS_USUARIO !== undefined) {
			setValue('usuarioIngreso', `${datos?.nombreS_USUARIO} ${datos?.apellidoS_USUARIO}`)
			setLoading(false)
			setOpenUsuario(false);
		} else {
			setValue('usuarioIngreso', "")
			setLoading(false)
			setOpenUsuario(false);
		}
	}

	const obtnerBoleano = (boleano: any) => {

		if (boleano) {
			obtnerSolicitudPorIdentificacion();
			setMostrarPantallaSolicitudInicio(true)
			setMostrarPantallaSolicitud(false)
			setMostrarPantallaSolicitudInicio(boleano)
		}


	}

	const obtenerCedulaDeSolicitud = (cedula: any) => {
		setCedulaUsuario(cedula)
	}

	const obtnerSolicitudPorIdentificacion = async () => {
		const identidad = cedulaUsuario;
		const request = {
			estadoTarea: 0,
			fechaFin: null,
			fechaIni: null,
			identificacion: identidad ? cedulaUsuario : "",
			modalidad: null,
			submodalidad: null,
			idUsuario: user.USR_ID,
			numRadicado: radicado !== undefined ? radicado : '',
			tipoSolicitud: null,
			numeroSolicitud: "",
			usuarioIngreso: "",
			solicitante: ""
		};
		startPostConsultarSolicitudes(request, 'NO');
	};

	const { publicIp } = useIp();

	const handleSearch = (data: any) => {

		if (!tieneAlgunFiltro(data)) {
			toast.warn('Debe seleccionar al menos un filtro para realizar la consulta');
			return;
		}

		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands.find(
			(x: { brand: string }) => x.brand === "Google Chrome" || x.brand === "Microsoft Edge"
		)?.brand || "Navegador desconocido";

		try {
			setCedulaUsuario(data.identificacion)
			setMostrarTabla(true)

			const numRadicado = getValues('radicado')
			const identificacion = getValues('identificacion').trim();
			const estadoTarea = getValues('estadoTarea');
			const tipoSolicitud = getValues('tipoSolicitud') === '0' ? null : getValues('tipoSolicitud');
			const modalidad = getValues('modalidad') === '0' ? null : getValues('modalidad');
			const submodalidad = getValues('submodalidad') === '0' ? null : getValues('submodalidad');
			const fechaIni = getValues('fechaModalidadInicio');
			const fechaFin = getValues('fechaModalidadFin');
			const numeroSolicitud = getValues('numeroSolicitud').trim();
			const usuarioIngreso = getValues("usuarioIngreso").trim();
			const solicitante = identificacionSolicitante;

			if (numRadicado !== '' || identificacion !== '' || estadoTarea !== 0 || tipoSolicitud !== null || modalidad !== '' || submodalidad !== '' || fechaIni !== null || fechaFin !== null || numeroSolicitud !== '' || usuarioIngreso !== '' || solicitante !== '') {
				const datos: ParametrosBusqueda | any = { identificacion, estadoTarea, tipoSolicitud, numRadicado, modalidad, submodalidad, fechaIni, fechaFin, idUsuario: user.USR_ID, numeroSolicitud, usuarioIngreso, solicitante };
				startPostConsultarSolicitudes(datos, 'SI');

				const requestAuditoria: Auditoria = {
					navegador: usrAg,
					sistemaOperativo: nav.userAgentData?.platform || 'Desconocido',
					nombreUsuario: user.USR_USERNAME,
					// usurioId: user.USR_ID,
					ip: publicIp,
					modulo: 'Gestión de Solicitudes',
					hostname: document.location.host,
					tipoAccion: 'CONSULTAS',
					idTipoAccion: 3,
					descripcion: `Consultó solicitudes con los siguientes filtros:
					- Identificación: ${identificacion}
					- Estado Tarea: ${estadoTarea}
					- Tipo Solicitud: ${tipoSolicitud}
					- Número de Radicado: ${numRadicado}
					- Modalidad: ${modalidad}
					- SubModalidad: ${submodalidad}
					- Fecha Inicio: ${fechaIni}
					- Fecha Fin: ${fechaFin}
					- Número Solicitud: ${numeroSolicitud}
					- Usuario Ingreso: ${usuarioIngreso}
					- ApoderadoAutorizado: ${solicitante}`,
					usurioId: ''
				};
				auditoriaApi.InsertAuditoria(requestAuditoria);



			} else {
				toast('Debe seleccionar al menos un filtro', { type: toast.TYPE.WARNING });
			}
		} catch (error: any) {
			const descripcionError = `Error al consultar solicitudes.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

			const requestAuditoriaError: Auditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData?.platform,
				nombreUsuario: user?.USR_USERNAME,
				usurioId: user?.USR_ID,
				ip: publicIp,
				modulo: 'Gestión de Solicitudes',
				hostname: document.location.host,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError
			};

			auditoriaApi.InsertAuditoria(requestAuditoriaError);
		}

	};

	const consultaSolicitud = async (consulta: ConsultarSolicitudModel) => {
		setLoading(true);
		try {
			const response = await api.PostConsultarSolicitud(consulta);
			setIsLoading(false);


			if (response && response.data) {
				if (response.data.solicitudPropietarioDto && response.data.solicitudPropietarioDto.length > 0) {

					setResponseSolicitud(response.data);
					consultarTrazabilidad(response?.data?.solicitudId);
					setLoading(false);
				} else {
					setResponseSolicitud(response.data);
					setLoading(false);
				}
			}
		} catch (error) {
			setLoading(false);
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		}
	};

	const agregarNuevaSolicitudOnClick = () => {
		setMostrarPantallaSolicitud(true)
		setMostrarPantallaSolicitudInicio(false)
	}


	const [userAsig, setUserAsig] = useState<string | undefined>('');
	const [tipodDoc, setTipodDoc] = useState<number>(0);
	const [numDoc, setNumDoc] = useState('');
	const [estadoDeCliente, setEstadoDeCliente] = useState('');


	const onClickItem = async (data: SolicitudDto) => {

		const userAsignado = data.usuarioAsignado;
		setUserAsig(userAsignado);
		setTipodDoc(data.idTipoIdentificacion);
		setNumDoc(data.cedulaCliente);

		setTipoAccion(BOTONERA_SOLICITUD.ACTUALIZAR);
		if (data.estadoSolicitud === ESTADO_SOLICITUD.APROBADA ||
			data.estadoSolicitud === ESTADO_SOLICITUD.RECHAZADA
			|| data.estadoSolicitud === ESTADO_SOLICITUD.ANULADA
		) {
			setTipoProcesar(true);
			setSiEsAprobadaSolcitud(true);

		} else {
			setTipoProcesar(false);
			setSiEsAprobadaSolcitud(false);
		}

		const modelConsultaSolicitud: ConsultarSolicitudModel = {
			solicitudTareaId: data.solicitudTareaId,
			tipoSubModalidadId: GUID_DEFAULT,
			identificacion: data.cedulaCliente,
			tipoIdentificacion: data.idTipoIdentificacion,
			estadoTarea: 0,
			modalidad: data.modalidad,
			tipoSolicitud: data.solicitudTareaId,
			numRadicado: '',
			fechaIni: data.fechaModalidadInicio,
			fechaFin: data.fechaModalidadFin,
			tareaNombre: data.tareaNombre,
			numeroSolicitud: "",
			usuarioIngreso: "",
			solicitante: ""
		};

		await consultaSolicitud(modelConsultaSolicitud);
		setSelectedData(data);
		getFormulariosSolicitud(data?.timCodigo);
		setIniciarPantalla('INICIO');
	};


	useEffect(() => {
		const fetchData = async () => {
			if (tipodDoc !== 0 && numDoc !== '') {
				try {
					const response = await ApiClientes.getClientByIdentificationTypeIdentificationNumberRequest(
						tipodDoc as number,
						numDoc
					);

					const { estadoCliente } = response.data;
					setEstadoDeCliente(estadoCliente)

				} catch (error) {
					console.log('Error al obtener los datos del cliente');

				}
			}
		};
		fetchData();
	}, [tipodDoc, numDoc]);

	const onItemGoBack = async () => {
		setMensajesEjecutados(mensajesEjecutados)

		const identificacion = getValues('identificacion').trim() || responseSolicitud?.identificacionCliente || '';
		const numRadicado = getValues('radicado');
		const estadoTarea = getValues('estadoTarea');
		const tipoSolicitud = getValues('tipoSolicitud') === '0' ? null : getValues('tipoSolicitud');
		const modalidad = getValues('modalidad') === '0' ? null : getValues('modalidad');
		const submodalidad = getValues('submodalidad') === '0' ? null : getValues('submodalidad');
		const numeroSolicitud = getValues('numeroSolicitud');
		const usuarioIngreso = getValues("usuarioIngreso");
		const solicitante = getValues("solicitante");

		if (numRadicado !== '' || identificacion !== '' || estadoTarea !== 0 || tipoSolicitud !== null || modalidad !== null || submodalidad !== null || numeroSolicitud !== '' || usuarioIngreso !== '' || solicitante !== '') {
			const datos: ParametrosBusqueda | any = { identificacion, estadoTarea, tipoSolicitud, modalidad, submodalidad, numRadicado, numeroSolicitud, usuarioIngreso, solicitante };
			startPostConsultarSolicitudes(datos, 'NO');
		}

		setIniciarPantalla('');
		setResponseSolicitud({} as ResponseSeveranceModel);
	};

	const onItemBack = async () => {
		setIniciarPantalla('');
	};

	const obtenerModalidadPorSolicitud = () => {
		const idTipoSolicitud = obtnerSolicitud === '0' ? null : obtnerSolicitud
		catalogoApi
			.getAllTypeModality(idTipoSolicitud)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setListaModalidades(response.data)

					} else {
						NotificationService.genericAlert(
							'Error en actualización',
							'Ocurrio un error al intentar actualizar, intente de nuevo.',
							'error',
						);

					}
				}
			})
	}

	const obtenerSubmodalidad = async (idTipoModalidad: string) => {

		if (idTipoModalidad === '0') {
			setListaSubModalidades([]);
			setValue('submodalidad', '0');
			return;
		}

		catalogoApi
			.getAllTypeSubModality(idTipoModalidad)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						if (response?.data && response?.data.length > 0) {
							setListaSubModalidades(response.data);
							if (response?.data.length === 1) {
								const objSubmodalidad = response.data[0];
								setValue('submodalidad', objSubmodalidad?.idTipoSubModalidad);
							}
						} else {
							setListaSubModalidades([]);
							setValue('submodalidad', '0');
						}
					}
				}
			}).catch((e: any) => {
				toast(`Error al consultar Submodalidad ${e.message}`, { type: toast.TYPE.ERROR });
			})
	}

	const openModalSolicitante = () => {
		setOpen(true);
		setValue('solicitante', '')
	};
	const openModalUsuarioIngreso = () => {
		setOpenUsuario(true);
		setValue('usuarioIngreso', '')
	};

	const handleClose = () => {
		setOpen(false);
		if (getValues("solicitante").trim() === '') {
			setIdentificacionSolicitante('');
		}
	};
	const handleCloseUsuario = () => {
		setOpenUsuario(false);
	};


	if (loading) {
		return <CHLoadingProcess />;
	}


	if (selectedData && responseSolicitud && iniciarPantalla === 'INICIO' && solicitudState && solicitudState.formulariosSolicitud) {

		const formulario = () => {

			const nombreTareaActualNormalizado = String(responseSolicitud?.nombreTarea || '')
				.toLowerCase()
				.normalize('NFD')
				.replace(/[\u0300-\u036f]/g, '');

			const tareaSeleccionadaNormalizada = String(selectedData?.tareaNombre || '')
				.toLowerCase()
				.normalize('NFD')
				.replace(/[\u0300-\u036f]/g, '');

			const esVerificacionListasRestrictivas =
				nombreTareaActualNormalizado.includes('verificacion de listas restrictivas') ||
				tareaSeleccionadaNormalizada.includes('verificacion de listas restrictivas');


			const esRadicacionDevueltaPorRechazo =
				responseSolicitud?.soL_RECHAZADA === '1' &&
				nombreTareaActualNormalizado.includes('radicacion') &&
				selectedData?.estadoSolicitud === 'PENDIENTE';


			if (esRadicacionDevueltaPorRechazo) {
				return (
					<InformacionProcesoSolicitud
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={BOTONERA_SOLICITUD.ACTUALIZAR}
						tipoAccionDetalleProcesar={false}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						deshalitarBotonesInconsistencia={deshalitarBotonesInconsistencia}
						setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo={grupo}
						estadoDeCliente={estadoDeCliente}
						userIngreso={userIngreso}
						userAsig={userAsig}
						esRadicacionDevueltaPorRechazo
					/>
				);
			}

			if (esVerificacionListasRestrictivas) {
				return (
					<InformacionProcesoSolicitud
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						deshalitarBotonesInconsistencia={deshalitarBotonesInconsistencia}
						setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo={grupo}
						estadoDeCliente={estadoDeCliente}
						userIngreso={userIngreso}
						userAsig={userAsig}
						esVerificacionListasRestrictivas
					/>
				);
			}


			if (selectedData?.tareaNombre?.includes('Cargue de documentos') ||
				selectedData?.tareaNombre?.includes('Cargue de documentos V 14') ||
				selectedData?.tareaNombre?.includes('Cargue de documentos V 8') ||
				selectedData?.tareaNombre?.includes('Cargue de Documentos Desafiliaciones') ||
				selectedData?.tareaNombre?.includes('Verificación novedades en los datos') ||
				selectedData?.tareaNombre?.includes('Revisión area jurídica') ||
				selectedData?.tareaNombre?.includes('Cargue de Documentos Subsidio Secuestrados') ||
				selectedData?.tareaNombre?.includes('Cargue de documentos Giro de recursos'
				)) {
				return (
					<InformacionProcesoSolicitud
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						deshalitarBotonesInconsistencia={deshalitarBotonesInconsistencia}
						setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo={grupo}
						estadoDeCliente={estadoDeCliente}
						userIngreso={userIngreso}
						userAsig={userAsig}
					/>
				);
			}
			if (selectedData?.tareaNombre?.includes('Seguridad documental grupo 1')) {
				return (
					<ProcesoSeguridadDocumental
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo1={responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 1')}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						datosCuentasBloqueos={undefined}
						userAsig={userAsig}
						grupo={grupo}
						userIngreso={userIngreso}
					/>
				);
			}

			if (selectedData?.tareaNombre?.includes('Seguridad documental grupo 2')) {
				return (
					<ProcesoSeguridadDocumental
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo1={responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						datosCuentasBloqueos={undefined}
						userAsig={userAsig}
						grupo={grupo}
						userIngreso={userIngreso}
					/>
				);
			}
			if (selectedData?.tareaNombre?.includes('Seguridad documental de grupo 1 Desafiliaciones')) {
				return (
					<ProcesoSeguridadDocumental
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo1={responseSolicitud?.nombreTarea.includes('Seguridad documental de grupo 1 Desafiliaciones')}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						datosCuentasBloqueos={undefined}
						userAsig={userAsig}
						grupo={grupo}
						userIngreso={userIngreso}
					/>
				);
			}
			if (selectedData?.tareaNombre?.includes('Seguridad documental de grupo 2 Desafiliaciones')) {
				return (
					<ProcesoSeguridadDocumental
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo1={responseSolicitud?.nombreTarea.includes('Seguridad documental de grupo 1 Desafiliaciones')}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						datosCuentasBloqueos={undefined}
						userAsig={userAsig}
						grupo={grupo}
						userIngreso={userIngreso}
					/>
				);
			}

			if (responseSolicitud?.nombreTarea?.includes('Verificación embargos') ||
				responseSolicitud?.nombreTarea?.includes('Verificación de Embargos Desafiliaciones') ||
				responseSolicitud?.nombreTarea?.includes('Verificación cuenta individual') ||
				responseSolicitud?.nombreTarea?.includes('Verificación de Cuenta Individual Desafiliaciones') ||
				responseSolicitud?.nombreTarea?.includes('Trámite de pago') ||
				responseSolicitud?.nombreTarea?.includes('Verificación de Novedades Desafiliaciones')) {
				return (
					<ProcesoVerificacionEmbargo
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						userAsig={userAsig}
						grupo={grupo}
						userIngreso={userIngreso}
						estadoDeCliente={estadoDeCliente}
					/>
				);
			}
			if (responseSolicitud?.nombreTarea?.includes('Pago')) {
				return (
					<ProcesoVerificacionEmbargo
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						userAsig={userAsig}
						grupo={grupo}
						userIngreso={userIngreso}
						estadoDeCliente={estadoDeCliente}
					/>
				);
			}
			if (
				!esRadicacionDevueltaPorRechazo &&
				trazabilidad &&
				trazabilidad.length >= 1 &&
				String(trazabilidad[0]?.descripcionTarea).includes('Radicación') &&
				String(trazabilidad[0]?.descripcionEstadoSolicitud) === 'PENDIENTE' &&
				String(trazabilidad[1]?.descripcionEstadoSolicitud) === 'RECHAZADA'
			) {
				return (
					<ProcesoRadicacion
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						tipoSolicitud={selectedData?.estadoSolicitud}
					/>
				)
			}

			if (selectedData?.timCodigo !== '' && solicitudState && solicitudState.formulariosSolicitud) {


				const { titulo, subtitulo, icono, botonera, formularios } = solicitudState.formulariosSolicitud
				return (
					<DynamicPage
						dynamicTitle={{ topText: titulo, title: subtitulo, icon: icono }}
						formsList={formularios}
						withButtons={botonera}
						responseSolicitud={responseSolicitud}
						// tipoAccion={tipoAccion}
						tipoAccion={
							esRadicacionDevueltaPorRechazo
								? BOTONERA_SOLICITUD.ACTUALIZAR
								: tipoAccion
						}
						// tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						tipoAccionDetalleProcesar={
							esRadicacionDevueltaPorRechazo
								? false
								: tipoAccionDetalleProcesar
						}
						modalidadSolicitud={selectedData?.timCodigo}
						onItemGoBack={onItemGoBack}
						onItemBack={onItemBack}
						obtnerBoleano={obtnerBoleano}
						inicioSolicitud={1 === 1}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						tipoSolicitud={selectedData?.estadoSolicitud}
						documentosAgregados={documentosAgregados}
					/>
				)
			}
			return (
				<InformacionProcesoSolicitud
					responseSolicitud={responseSolicitud}
					onItemGoBack={onItemGoBack}
					tipoAccion={tipoAccion}
					tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
					obtnerBoleano={false}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}
					deshalitarBotonesInconsistencia={deshalitarBotonesInconsistencia}
					setDeshalitarBotonesInconsistencia={setDeshalitarBotonesInconsistencia}
					formulariosSolicitud={solicitudState.formulariosSolicitud}
					grupo={grupo}
					estadoDeCliente=''
					userIngreso={undefined}
					userAsig={undefined}
				/>
			);


		}

		const { titulo, subtitulo, icono } = solicitudState.formulariosSolicitud

		return (
			<>
				<LoadingV2Component key="loader" loading={isLoading} tittle='' />

				{
					(selectedData?.estadoSolicitud === "APROBADA" &&
						(selectedData?.tareaNombre === 'Radicación solicitud retiro parcial de cesantías' ||
							selectedData?.tareaNombre === 'Radicación V 8' ||
							selectedData?.tareaNombre === 'Radicación V 14' ||
							selectedData?.tareaNombre === 'Radicacion Solicitud de Desafiliaciones' ||
							selectedData?.tareaNombre === 'Radicación subsidio secuestrados' ||
							selectedData?.tareaNombre === 'Radicación solicitud Giro de recursos')) ?
						<div className={classes.margin}>
							<CHPageTitleV2 topText={titulo} title1={subtitulo} icono={icono} />
						</div>

						: ''
				}
				<div>
					{formulario()}
				</div>

				{

					((selectedData?.estadoSolicitud === 'APROBADA' || selectedData?.estadoSolicitud === 'ANULADA') &&
						(selectedData?.timCodigo === 'VIUSADA' ||
							selectedData?.timCodigo === 'CVURG' ||
							selectedData?.timCodigo === "VINUEVA" ||
							selectedData?.timCodigo === "CNVGR" ||
							selectedData?.timCodigo === "VINUEVASP" ||
							selectedData?.timCodigo === "LIBERGRAV" ||
							selectedData?.timCodigo === "APTGHGR" ||
							selectedData?.timCodigo === "VIUSADAPP" ||
							selectedData?.timCodigo === "VINUEVAPP" ||
							selectedData?.timCodigo === "CESOCLEAS" ||
							selectedData?.timCodigo === 'READACPROES' ||
							selectedData?.timCodigo === "CONSNUEVASP" ||
							selectedData?.timCodigo === "ADQUITER" ||
							selectedData?.timCodigo === "AHOPROEDU" ||
							selectedData?.timCodigo === "REPARVIV" ||
							selectedData?.timCodigo === "ARMVGR" ||
							selectedData?.timCodigo === "CONSNUEVA" ||
							selectedData?.timCodigo === "CSLGR" ||
							selectedData?.timCodigo === "PAGOPREDIAL" ||
							selectedData?.timCodigo === "REPAES" ||
							selectedData?.timCodigo === "CAEXLEHAPP" ||
							selectedData?.timCodigo === "CAEXLEHA" ||
							selectedData?.timCodigo === "DESCONCESANT" ||
							selectedData?.timCodigo === "DESSOLAPORT" ||
							selectedData?.timCodigo === "EJEOPCOMP" ||
							selectedData?.timCodigo === "PAGOCRHIP" ||
							selectedData?.timCodigo === 'VIUSADASP' ||
							selectedData?.timCodigo === 'CCSLSS' ||

							selectedData?.timCodigo === "RETINST" ||
							selectedData?.timCodigo === "ACREDV8") &&
						(selectedData?.tareaNombre === 'Radicación solicitud retiro parcial de cesantías' ||
							selectedData?.tareaNombre === 'Solicitud de retiro V 14' ||
							selectedData?.tareaNombre === 'Radicación solicitud Giro de recursos' ||
							selectedData?.tareaNombre === 'Radicación subsidio secuestrados' ||
							selectedData?.tareaNombre === "Radicación V 14" ||
							selectedData?.tareaNombre === 'Radicación V 8' ||
							selectedData?.tareaNombre === 'Radicacion Solicitud de Desafiliaciones' ||
							selectedData?.tareaNombre === 'Verificación en tiempo')
					) ? (

						<div className={classes.marginBotonRegresar}>
							<Button
								className={classes.buttonRegresar}
								variant="outlined"
								onClick={onItemGoBack}
							>
								REGRESAR
							</Button>
						</div>
					) : ''
				}

			</>
		);
	}

	return (
		<>
			{mostrarPantallaSolicitudInicio && (
				<CHPrivateContainer>
					<LoadingComponent key="loader" loading={solicitudState.loading} />
					<CHPageTitle
						topText="Solicitudes"
						subText=""
						title1="Gestión de"
						title2="Solicitudes "
					/>
					<CHPaper title="Información Solicitud">
						<Grid container>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="numeroSolicitud"
									control={control}
									rules={{
										minLength: {
											value: 4,
											message:
												'La longitud mínima del campo es de 4 caracteres',
										},
										maxLength: {
											value: 30,
											message:
												'La longitud máxima del campo es de 30 caracteres',
										},
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											InputLabelProps={{ shrink: true }}
											label="Número de solicitud"
											value={value}
											onChange={e => {


												onChange(e)
											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="radicado"
									control={control}
									rules={{
										minLength: {
											value: 4,
											message:
												'La longitud mínima del campo es de 4 caracteres',
										},
										maxLength: {
											value: 30,
											message:
												'La longitud máxima del campo es de 30 caracteres',
										},
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											InputLabelProps={{ shrink: true }}
											label="No. Radicado"
											value={value}
											onChange={e => {
												onChange(e)
											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="identificacion"
									control={control}
									rules={{
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
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField

											numberMask
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											InputLabelProps={{ shrink: true }}
											label="No. Identificación"
											value={value}
											onChange={e => {
												onChange(e)
											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="estadoTarea"
									control={control}
									render={({ field: { onChange, value } }) => (
										<CHSelect
											defaultValue=""
											label="Estado del Proceso"
											value={value}
											onChange={onChange}

										>
											<MenuItem key={0} value={0}>
												Todos
											</MenuItem>
											{estadosTareas?.map(
												(option: ESTADOTAREADTO) => {
													const EN_SUBSANACION = 330;
													if (option.id !== EN_SUBSANACION) {
														return (
															<MenuItem
																key={option.id}
																value={option.id}
															>
																{option.descripcion}
															</MenuItem>
														)
													}
													return null;
												}
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="tipoSolicitud"
									control={control}
									render={({ field: { onChange, value } }) => (
										<CHSelect
											defaultValue="0"
											label="Tipo de Solicitud"
											value={value}
											onChange={(e: any) => {
												setObtnerSolicitud(e.target.value);
												setListaSubModalidades([]);
												setValue('submodalidad', '0');
												onChange(e);
											}}
										>
											<MenuItem key="0" value="0">
												Todos
											</MenuItem>
											{tipoSolicitudes?.map(
												(option: TYPEREQUEST) => (
													<MenuItem
														key={option.idTipoSolicitud}
														value={option.idTipoSolicitud}
													>
														{
															option.descripcionTipoSolicitud
														}
													</MenuItem>
												),
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="modalidad"
									control={control}
									render={({ field: { onChange, value } }) => (
										<CHSelect
											defaultValue="0"
											label="Tipo Modalidad"
											value={value}
											onChange={e => {
												onChange(e);
												obtenerSubmodalidad(String(e.target.value));
											}}
										>
											<MenuItem key="0" value="0">
												Todos
											</MenuItem>
											{listaModalidades?.map(
												(option: any) => (
													<MenuItem
														key={option.idTipoModalidad}
														value={option.idTipoModalidad}
													>
														{
															option.descripcionTipoModalidad
														}
													</MenuItem>
												),
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="submodalidad"
									control={control}
									render={({ field: { onChange, value } }) => (
										<CHSelect
											defaultValue="0"
											label="Tipo Sub - modalidad"
											value={value}
											onChange={onChange}
										>
											<MenuItem key="0" value="0">
												Todos
											</MenuItem>
											{listaSubModalidades?.map(
												(option: any) => (
													<MenuItem
														key={option.idTipoSubModalidad}
														value={option.idTipoSubModalidad}
													>
														{
															option.descripcionTipoSubModalidad
														}
													</MenuItem>
												),
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="fechaModalidadInicio"
									control={control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHDatePicker

											error={!!error}
											InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
											helperText={error ? error.message : null}
											label="Fecha Inicial"
											value={value}
											onChange={onChange}

										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="fechaModalidadFin"
									control={control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHDatePicker
											InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
											error={!!error}
											helperText={error ? error.message : null}
											label="Fecha Final"
											value={value}
											onChange={onChange}

										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="solicitante"
									control={control}
									rules={{
										minLength: {
											value: 3,
											message:
												'La longitud mínima del campo es de 4 caracteres',
										},

									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField


											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly
											InputLabelProps={{ shrink: true }}
											label="Apoderdado y/o autorizado"
											value={value}
											onClick={openModalSolicitante}
											onChange={e => {

												onChange(e)

											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={4}>
								<Controller
									name="usuarioIngreso"
									control={control}
									rules={{
										minLength: {
											value: 3,
											message:
												'La longitud mínima del campo es de 4 caracteres',
										},

									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly
											InputLabelProps={{ shrink: true }}
											label="Usuario Ingreso"
											value={value}
											onChange={e => {

												onChange(e)

											}}
											onClick={openModalUsuarioIngreso}
										/>
									)}
								/>
							</Grid>
						</Grid>
					</CHPaper>
					<div className="buttons">
						<Button
							className={classes.button}
							variant="outlined"
							onClick={handleSubmit(handleSearch)}
						>
							CONSULTAR
						</Button>
						<Button
							className={classes.button}
							variant="outlined"
							onClick={agregarNuevaSolicitudOnClick}
						>
							AGREGAR SOLICITUD
						</Button>
						<Button
							className={classes.button}
							variant="outlined"
							onClick={limpiarCampos}
						>
							LIMPIAR
						</Button>
					</div>
					{
						mostrarTabla &&
						<SolicitudesTable
							data={solicitudState?.solicitudes}
							onClickItem={onClickItem}
							grupo={grupo}

						/>
					}
				</CHPrivateContainer>
			)

			}
			<ModalSolicitanteBuscar
				open={open}
				onClose={handleClose}
				buscarSolicitante={buscarSolicitante}
			/>
			<ModalUsuarioIngresoBuscar
				open={openUsuario}
				onClose={handleCloseUsuario}
				buscarUsuarioIngreso={buscarUsuarioIngreso} />

			{
				mostrarPantallaSolicitud &&
				<Solicitudes
					obtenerCedulaDeSolicitud={obtenerCedulaDeSolicitud}
					obtnerBoleano={obtnerBoleano}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}

				/>
			}
		</>
	);
};

interface LinkMapStateToProps {
	estadosTareas: ESTADOTAREADTO[],
	tipoSolicitudes: TYPEREQUEST[],
	solicitudState: any;
};

const mapStateToProps = (state: AppState) => ({
	estadosTareas: state.catalog.estadosTareas,
	tipoSolicitudes: state.catalog.typeRequest,
	solicitudState: state.solicitud,
});

interface LinkMapDispatchToProps {
	startGetAllTypeRequest: (idTipoActor: number) => void;
	startGetAllStatesRequest: () => void;
	startPostConsultarSolicitudes: (busqueda: ParametrosBusqueda, mostrarMensaje: string) => void;
	getFormulariosSolicitud: (timCodigo: string) => void;
	documentosAgregados: (documentos: any) => void;
};

const mapDispatchToProps = {
	startGetAllTypeRequest,
	startGetAllStatesRequest,
	startPostConsultarSolicitudes,
	getFormulariosSolicitud,
	documentosAgregados
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(SolicitudesInicio);