import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import {
	Button,
	Card,
	CardContent,
	CircularProgress,
	Container,
	createStyles,
	Grid,
	makeStyles,
	MenuItem,
	Theme,
} from '@material-ui/core';
import CHSelect from '../../components/ch-select/CHSelect';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import TituloIcono from './components/titulo-icono/tituloIcono.component';
import { AppState } from '../../store/rootReducer';
import {
	startGetAllTypeRequest,
	startGetAllTypeModality,
	startGetAllTypeSubModality,
	startGetAllTypeDocuments,
} from '../../actions/catalog.action';
import { clearDetailRequestInformation } from '../../actions/credit.actions';
import { CatalogState } from '../../reducers/catalog.reducer';
import {
	TYPEREQUEST,
	TYPEMODALITY,
	TYPESUBMODALITY,
	IDENTIFICATIONTYPEDTO,
} from '../../types/catalog.type';
import './solicitudes.styles.scss';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import NotificationService from '../../services/notification.service';
import { CreateSeveranceModel } from '../../models/requests/create-severance.model';
import CHTextField from '../../components/ch-text-field/CHTextField';
import SolicitudApi from '../../services/api-solicitud.service';
import { ConsultarSolicitudModel } from '../../models/requests/consultar-solicitud.model';
import { RequestRequestSeveranceModel } from '../../models/requests/request-request-severance.model';
import { startGetAllPostRequestSeverance } from '../../actions/severance.action';
import { BOTONERA_SOLICITUD, GUID_DEFAULT, PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION } from '../../constants/constants';
import { ResponseSeveranceModel } from '../../models/requests/response-severance.model';
import AuthService from '../../services/auth.service';
import InformacionProcesoSolicitud from './informacionProcesoSolicitud/informacionProcesoSolicitud';
import CHPageTitleV2 from '../../components/ch-page-title/CHPageTitleV2';
import ApiClients from '../../services/api-clients.service';
import { getFormulariosSolicitud } from '../../actions/solicitud.action';
import ProcesoSeguridadDocumental from './procesoSeguridadDocumental/procesoSeguridadDocumental';
import ProcesoVerificacionEmbargo from './procesoVerificacionEmbargos/procesoVerificacionEmbargos';
import DynamicPage from './dynamicPage/DynamicPage.component';
import { SolicitudState } from '../../reducers/solicitud.reducer';
import apiCredito from '../../services/api-credit.service';
import Acreditacion from './acreditacion/acreditacion.component';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import UtilService from '../../utils/util.service';
import ApiEmbargo from '../../services/api-embargos.service';
import InformacionAcreditacion from './components/InformacionAcreditacion/InformacionAcreditacion';
import ProcesoAcreditacion from './components/procesoAcreditacion/procesoAcreditacion';
import history from '../../utils/history.router';

const useStyles = makeStyles((theme: Theme) =>
	createStyles({
		buttonprincipal: {
			width: '250px',
			height: '50px',
			marginLeft: '1rem',
			background: '#FEC917',
		},
		buttonsecundario: {
			width: '250px',
			height: '50px',
			marginLeft: '1rem',
			background: '#43b5f1',
		},
		buttonProgress: {
			position: 'absolute',
			top: '50%',
			left: '50%',
			marginTop: -12,
			marginLeft: -12,
		},
		wrapper: {
			margin: theme.spacing(1),
			position: 'relative',
		},
	}),
);

function Solicitudes({
	catalogState,
	isLoadingSeverance,
	solicitudState,
	startypeRequest,
	startTypeModality,
	startSubModality,
	clearDetailInformation,
	obtenerTipoDocumento,
	startPostRequestSeverance,
	getFormulariosSolicitud,
	obtenerCedulaDeSolicitud,
	obtnerBoleano,
	siEsAprobadaSolcitud,
}: {
	catalogState: CatalogState,
	isLoadingSeverance: boolean,
	solicitudState: SolicitudState,
	startypeRequest: (IdTipoActor: number) => void,
	startTypeModality: (idTipoSolicitud: string) => void,
	startSubModality: (idTipoModalidad: string) => void,
	clearDetailInformation: () => void,
	obtenerTipoDocumento: () => void,
	startPostRequestSeverance: (data: RequestRequestSeveranceModel) => void,
	getFormulariosSolicitud: (nombreActividad: string) => void,
	obtenerCedulaDeSolicitud: any,
	obtnerBoleano: any
	siEsAprobadaSolcitud: boolean;

}): any {


	const api = SolicitudApi();


	const apiDos = ApiClients();
	const apiCre = apiCredito();
	const apiEmbargo = ApiEmbargo();
	const [tipoAccion, setTipoAccion] = useState<number>(BOTONERA_SOLICITUD.GUARDAR);
	const [responseSolicitud, setResponseSolicitud] = useState<ResponseSeveranceModel>();
	const [iniciarPantalla, setIniciarPantalla] = useState<string>('');
	const [selectedTipoSubModalidad, setSelectedTipoSubModalidad] = useState<string>('');
	const [selectedModalidad, setModalidadData] = useState<string>('');
	const [tipoAccionDetalleProcesar, setTipoProcesar] = useState<boolean>(false);
	const [loading, setLoading] = React.useState(false);
	const [isloading, setIsLoading] = React.useState(false);
	const [tipoMidalidadValidar, setTipoModalidaValidar] = useState<any>(null);
	const [numeroCedulaVerificar, setNumeroCedulaVerificar] = useState<any>(null);
	const [datoCuota, setDatoCuotas] = useState<any>(0);
	const [tipoEmbargo, setTipoEmbargo] = useState<any>('');
	const [mensajesEjecutados, setMensajesEjecutados] = useState(false);
	const [deshablitarBotonAceptar, setDesHabilitarBotonAceptar] = useState(true);
	const [flujos37_102, setFlujos37_102] = useState<any>({})
	const [validarRetiro, setValidarRetiro] = useState<number>(0);
	const [mostrarMensaje, setMostrarMensaje] = useState(false);
	const [onSubmitDynamic, setOnSubmitDynamic] = useState<(() => void) | null>(null);
	const HABILITAR_VIGIA_Y_TUS_DATOS = false;
	const HABILITAR_TUS_DATOS = true;
	const HABILITAR_VIGIA = true;

	const [mostrarBotonera, setMostrarBotonera] = useState(false);
	const onSubmitSafe = onSubmitDynamic || (() => console.warn("."));



	useEffect(() => {
		if (responseSolicitud?.nombreTarea?.includes('Verificación en tiempo')) {
			setMostrarBotonera(true);
		} else {
			setMostrarBotonera(false);
		}
	}, [responseSolicitud]);

	const consultarSolicitudAcreditacion = async (consulta: ConsultarSolicitudModel) => {
		setLoading(true);
		try {
			const response = await api.PostConsultarSolicitud(consulta);

			if (response && response.data) {
				return response.data;
			}
			console.warn("La API no devolvió datos válidos.");
			return {};
		} catch (error) {
			console.error("Error en consultarSolicitudAcreditacion:", error);
			toast("Error del sistema", { type: toast.TYPE.ERROR });
			return {};
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		console.log('.', onSubmitSafe);
	}, [onSubmitSafe]);

	const {
		control,
		setValue,
		handleSubmit,
		register,
		getValues,
		watch,
		reset,
	} = useForm({
		defaultValues: {
			tipoDocumento: '',
			numeroDocumento: '',
			typerequest: '',
			typemodality: '',
			typesubmodality: '',
		},
	});

	const tipoDSolicitud = watch('typerequest');
	const obtenerCodigoDocumentoTusDatos = (
		tipoDocumento: string | number | null | undefined,
	): string | undefined => {
		const value = String(tipoDocumento ?? '').trim();

		const tiposDocumento: Record<string, string> = {
			'1': 'CC',
			'2': 'NIT',
			'3': 'CE',
			'4': 'TI',
			'5': 'NUI',
			'6': 'PAS',
			'7': 'RC',
			'8': 'CJ',
			'9': 'CM',
		};

		return tiposDocumento[value];
	};
	const documentType = obtenerCodigoDocumentoTusDatos(watch('tipoDocumento'));

	const [descripSubModalidad, setDescripSubModalidad] = useState<string | undefined>(undefined);
	const acreditacionV8Solicitud = '2b2fb78e-031d-4ef7-a64b-f4f0acfc75c8';
	const mostrarBotoneraEnProcesoAcreditacion = responseSolicitud
		? !responseSolicitud?.nombreTarea?.includes('Verificación en tiempo')
		: false;
	const [solicitudes, setSolicitudes] = useState<any[]>([]);
	const [nivelAfiliado, setNivelAfiliado] = useState<any>(null);

	const formularioAcreditacion = useForm({
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
			observacion: '',
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
			fecha_Firma_Escritura: new Date(),
			acuerdo: 'Acuerdo 02 de 2020',
			noRadicado: '',
			fechaGiro: new Date(),
			valorGirado: 0,
			documentoBeneficiarioPago: 0,
			beneficiarioPago: '',
			fechaSeguimiento: new Date(),
			fechaProximoSeguimiento: new Date(),
			acredito: '',
			cumpleRequisito: '',
			causal: '',
			accionRealizar: '',
			observaciones: ''
		},
	});

	useEffect(() => {
		obtenerTipoDocumento();
		clearDetailInformation();
		catalogState.typeModality = [];
		catalogState.typeSubModality = [];
		startypeRequest(2);
		setTipoProcesar(false);
	}, []);

	useEffect(() => {
		if (responseSolicitud) {
			setOnSubmitDynamic(() => async () => {
				setIsLoading(true);
				const formData = formularioAcreditacion.getValues();
				const consulta = {
					tipoSubModalidadId: selectedTipoSubModalidad?.toString().trim() || "00000000-0000-0000-0000-000000000000",
					solicitudTareaId: responseSolicitud?.solicitudTareaId || "00000000-0000-0000-0000-000000000000",
					identificacion: responseSolicitud?.identificacionCliente || "",
					tipoIdentificacion: responseSolicitud?.tipoIdentificacion || 1,
					tipoCuenta: "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA",
					tipoVinculacion: "AFILIADO ADMINISTRACION CESANTIAS"
				};

				const solicitudData = await consultarSolicitudAcreditacion(consulta);
				if (!solicitudData) {
					toast("No se pudo obtener la información de la solicitud", { type: toast.TYPE.ERROR });
					setIsLoading(false);
					return;
				}

				if (
					!formData?.cumpleRequisito?.toString().trim() ||
					!formData?.numero_Folio?.toString().trim() ||
					formData?.numero_Folio === "0" ||
					!formData?.observaciones?.toString().trim()
				) {

					NotificationService.genericAlert(
						'Advertencia',
						'Por favor completar todos los campos.',
						'warning',
					);
					setIsLoading(false);
					return;
				}

				const dataToSend = {
					...solicitudData,
					cumpleRequisito: formData?.cumpleRequisito || '',
					causal: formData?.causal || "",
					accionRealizar: formData?.accionRealizar || "",
					acuerdo: formData?.acuerdo || "",
					observacion: formData?.observaciones,
					numero_Folio: formData?.numero_Folio,
				};


				try {
					if (tipoAccion === BOTONERA_SOLICITUD.GUARDAR) {
						const response = await api.PostSolicitudTarea(dataToSend);
						if (response?.isSuccess) {
							NotificationService.genericAlert("Guardado", "Se ha guardado correctamente la solicitud.");
							setTipoAccion(BOTONERA_SOLICITUD.ACTUALIZAR);
						} else {
							toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
						}
					} else if (tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR) {
						const response = await api.PostActualizarSolicitudTarea(dataToSend);
						if (response?.isSuccess) {
							setCambiarEstadoTramite(true);
							const refreshed = await consultarSolicitudAcreditacion(consulta);
							setTipoSolicitud(refreshed.estado);
							NotificationService.genericAlert("Actualizado", "Se ha actualizado correctamente la solicitud.");
						} else {
							toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
						}
					}
				} catch (e) {
					toast("Error del sistema", { type: toast.TYPE.ERROR });
				} finally {
					setIsLoading(false);
				}
			});
		}

	}, [responseSolicitud, formularioAcreditacion, selectedTipoSubModalidad, tipoAccion]);

	 useEffect(() => {
		if (catalogState?.typeSubModality?.length === 1) {
			const typeSubmodality = catalogState?.typeSubModality[0];
			const dato = `${typeSubmodality.idTipoSubModalidad}_${typeSubmodality.timCodigo}`
						
			setValue('typesubmodality', dato);
			setDescripSubModalidad(typeSubmodality?.descripcionTipoSubModalidad);
			setSelectedTipoSubModalidad(typeSubmodality.idTipoSubModalidad);
			getFormulariosSolicitud(typeSubmodality.timCodigo);
		}
	}, [catalogState.typeSubModality]);

	useEffect(() => {
		const acreditacion = '27b2c612-bde0-42a1-8447-024cd0edfb2c';
		if (catalogState?.typeSubModality?.length === 1) {
			const typeSubmodality = catalogState?.typeSubModality[0];
			const dato = `${typeSubmodality.idTipoSubModalidad}_${typeSubmodality.timCodigo}`
			setValue('typesubmodality', dato);
			setSelectedTipoSubModalidad(typeSubmodality.idTipoSubModalidad);
			getFormulariosSolicitud(typeSubmodality.timCodigo);
			if (acreditacion === tipoMidalidadValidar) {
				setDescripSubModalidad(typeSubmodality?.descripcionTipoSubModalidad);
			}
		}
	}, [catalogState.typeSubModality])

	useEffect(() => {
		if (numeroCedulaVerificar !== null && tipoDSolicitud !== '') {
			obtenerNumerosCuota();
		}
	}, [tipoDSolicitud, numeroCedulaVerificar]);


	const obtenerJuzgadoEmbargo = async () => {

		await apiEmbargo
			.getObtenerEmbargosByIdentificacion(getValues('numeroDocumento'))
			.then(async response => {
				if (response) {
					setTipoEmbargo(response.data[0].embargo.juzgado)
				}
			})
			.catch((e) => {
				toast(`Error del sistema  ${e}`, {
					type: toast.TYPE.ERROR,
				});
			})
	}

	const [matriculaAcre, setMatriculaAcre] = useState();
	const [vrRetiroAcre, setVrRetiroAcre] = useState();

	const solicitud = async (clientState: any, cuentaTipo: any) => {

		const modelConsultaSolicitud: ConsultarSolicitudModel = {
			tipoSubModalidadId: selectedTipoSubModalidad,
			solicitudTareaId: GUID_DEFAULT,
			identificacion: getValues('numeroDocumento'),
			tipoIdentificacion: Number(getValues('tipoDocumento')),
			tipoCuenta: cuentaTipo,
			tipoVinculacion: clientState?.tipoVinculacion
		};

		await api.GetConsultarSolucitud(modelConsultaSolicitud)
			.then(async response => {

				const esAcreditacionV8 =
					response.data.descripcionSubModalidad === "ACREDITACION V8" ||
					response.data.descripcionModalidad === "ACREDITACION V8" ||
					response.data.descripcionProceso === "ACREDITACION V8";


				if (esAcreditacionV8) {

					setTipoAccion(BOTONERA_SOLICITUD.GUARDAR);

					setResponseSolicitud({ ...response.data });
				}

				if (response !== null &&
					response.returnMessage !== '' &&
					response.isSuccess === false &&
					response.data === null
				) {
					setLoading(false);
					setDesHabilitarBotonAceptar(true);
					NotificationService.genericAlwarning(
						'Advertencia',
						`${response.returnMessage}`,
						'warning'
					);
					return;
				}
				const confirmAlert = await NotificationService.confirmAlert('¿Está seguro que desea continuar?');

				if (!confirmAlert) {
					setLoading(false);
					setDesHabilitarBotonAceptar(false);
					return;
				}

				if (response !== null &&
					response.returnMessage !== '' &&
					response.isSuccess === false &&
					response.data !== null) {

					const estadosNoPermitidos = ["PROCESADA", "EN TRÁMITE", "PENDIENTE"];

					if (estadosNoPermitidos.includes(response.data.estadoSolicitud)) {
						NotificationService.genericAlwarning(
							"Alerta",
							`No puedes continuar porque ya existe una solicitud en estado: ${response.data.estadoSolicitud}`,
							"warning"
						);
						return;
					}

					const user = AuthService.getUserInfo();

					if (tipoDSolicitud !== acreditacionV8Solicitud) {

						if (confirmAlert && response.data.soL_ASIGNADO === user.USR_ID) {
							if (confirmAlert && (response.data.nombreTarea === "Verificación en tiempo" || response.data.soL_ASIGNADO === user.USR_ID)) {

								const modelo: RequestRequestSeveranceModel =
								{
									solicitudId:
										response.data.solicitudId,
									subModalidadId: response.data.tipoSubModalidadId,
									IdCliente: response.data.clienteId,
								};

								if (
									response.data.solicitudEncontrada ===
									true
								) {
									setTipoAccion(
										BOTONERA_SOLICITUD.ACTUALIZAR,
									);
								} else {
									setTipoAccion(
										BOTONERA_SOLICITUD.GUARDAR,
									);
								}

								if (clientState.cuentas[1] !== undefined) {
									response.valorTotal = clientState.cuentas[1].saldoDisponibleEmbargos;
								} else if (clientState.cuentas[0] !== undefined) {
									response.valorTotal = clientState.cuentas[0].saldoDisponibleEmbargos;
								}
								setResponseSolicitud(response.data);


								await startPostRequestSeverance(modelo);
								setIniciarPantalla('INICIA');
								setModalidadData(
									response.data.volverPantalla,
								);
							} else if (confirmAlert && response.data?.nombreTarea !== "Verificación en tiempo") {
								toast(`Error, esta solicitud ya se encuentra asignada`, { type: toast.TYPE.ERROR });
							}

						}
					}
				}

				if (confirmAlert) {
					const user = AuthService.getUserInfo();
					const modelSolicitud: CreateSeveranceModel = {
						cuentaId: null,
						tipoSubModalidad: selectedTipoSubModalidad,
						clienteId: Number(response.data?.clienteId || 0),
						creadoPor: user.USR_ID,
						procesoId: getValues('typerequest'),
						timCodigo: selectedModalidad
					};

					if (response.data.timCodigo !== "") {
						getFormulariosSolicitud(modelSolicitud.timCodigo);

					}


					await api.posCreateRequestSeverance(modelSolicitud)
						.then(async creacion => {
							if (creacion?.data && creacion.isSuccess) {
								setTipoAccion(BOTONERA_SOLICITUD.GUARDAR);
								response.data.valorTotal = clientState.saldoDisponible;
								setResponseSolicitud(creacion.data);
								setIniciarPantalla('INICIA');
								toast('Solicitud creada correctamente', { type: toast.TYPE.SUCCESS });
							} else {
								toast(creacion.returnMessage, { type: toast.TYPE.ERROR });
							}
						})
						.catch((e) => {
							toast(`Error del sistema  ${e}`, { type: toast.TYPE.ERROR });
						});
				}

				setLoading(false);
				setMatriculaAcre(response.data.solicitudMatriculaDto?.[0]?.numeroMatricula ?? '');
				setVrRetiroAcre(response.data.valorRetirar)

			})
			.catch(() => {
				setLoading(false);
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});



	};

	const obtenerFlujoDodo = async (tipo: any, numero: any) => {
		const request = {
			tipoDocumentoId: tipo,
			numeroDocumento: numero
		}
		try {
			const response = await api.PostObtenerFlujoDodo(request);
			if (response.isSuccess) {
				setFlujos37_102(response.data)
			}
		} catch (error) {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		}
	}


	const obtenerNumerosCuota = async () => {
		const acreditacion8 = '5b0a16af-16ef-40f2-b384-a46fe00b0f63';
		const vivienda8 = 'b61024bb-f484-4aa5-94f5-73d0750d95a2';
		const vivienda14 = '4503b975-fa78-45d7-9860-dbcaa9957eb6';
		const retiroCesantias = '2fe2f564-2778-442b-a278-ef6ebfc5e6e9';
		const devolucionAvacAvav = '6f659d82-1711-46a9-b21f-9904313ef9ad';
		const pagosExtraordinarios = '3335af25-736d-4437-a3c4-ebc53661606a';

		const tipoDocumentoSeleccionado = watch('tipoDocumento');
		const identificationType = Number(tipoDocumentoSeleccionado);
		const documentType = obtenerCodigoDocumentoTusDatos(
			tipoDocumentoSeleccionado,
		);
		const identification = watch('numeroDocumento');

		const mensajeServicioNoDisponible =
			'El servicio de consulta no está disponible en este momento. Por favor contactar con el área correspondiente de Caja Honor';

		const mostrarServicioNoDisponible = async () => {
			await NotificationService.genericAlwarning(
				'Advertencia',
				mensajeServicioNoDisponible,
				'warning',
			);
		};

		const obtenerFechaValida = (fecha: unknown): Date | null => {
			if (fecha === null || fecha === undefined || String(fecha).trim() === '') {
				return null;
			}

			const fechaTexto = String(fecha).trim();
			const fechaFormatoIso = /^(\d{4})-(\d{2})-(\d{2})/.exec(fechaTexto);

			if (fechaFormatoIso) {
				const [, anio, mes, dia] = fechaFormatoIso;

				const fechaLocal = new Date(
					Number(anio),
					Number(mes) - 1,
					Number(dia),
				);

				return Number.isNaN(fechaLocal.getTime())
					? null
					: fechaLocal;
			}

			const fechaConvertida = new Date(fechaTexto);

			return Number.isNaN(fechaConvertida.getTime())
				? null
				: fechaConvertida;
		};

		const obtenerAniosPermitidosPorHallazgo = (
			riskLAFT: unknown,
		): number | null => {
			const nivelNormalizado = String(riskLAFT ?? '')
				.normalize('NFD')
				.replace(/[\u0300-\u036f]/g, '')
				.trim()
				.toUpperCase();

			const vigenciaPorNivel: Record<string, number> = {
				BAJO: 3,
				MEDIO: 2,
				ALTO: 1,
			};

			return vigenciaPorNivel[nivelNormalizado] ?? null;
		};

		const bloquearYRedirigirActualizacionDatos = async () => {
			setDesHabilitarBotonAceptar(true);
			setIsLoading(false);

			await NotificationService.genericAlwarning(
				'Advertencia',
				'Afiliado debe actualizar información de datos',
				'warning',
			);

			history.push('/HojaDeVida');
		};

		/*
			Retorna true únicamente cuando se requiere actualización y el usuario
			fue redirigido a Hoja de Vida. Si VIGIA/Tus Datos fallan, muestra la
			alerta del servicio, pero retorna false para permitir continuar.
		*/
		const validarActualizacionDatosVigia = async (
			fechaActualizaDatosContactoRaw: unknown,
		): Promise<boolean> => {
			if (!documentType) {
				await mostrarServicioNoDisponible();
				setDesHabilitarBotonAceptar(false);
				return false;
			}

			try {
				// Solo se consulta VIGIA para obtener el nivel de riesgo.
				const respuestaVigia =
					await apiCre.getValidacionActualizacionVigia(
						identification,
						documentType,
					);

				const vigiaData = respuestaVigia?.data;

				const vigiaDisponible =
					respuestaVigia?.isSuccess === true &&
					vigiaData !== null &&
					vigiaData !== undefined;

				if (!vigiaDisponible) {
					await mostrarServicioNoDisponible();
					setDesHabilitarBotonAceptar(false);
					return false;
				}

				// VIGIA: BAJO, MEDIO o ALTO.
				const nivelHallazgo = vigiaData?.riskLAFT;

				const aniosPermitidos = obtenerAniosPermitidosPorHallazgo(
					nivelHallazgo,
				);

				if (aniosPermitidos === null) {
					await mostrarServicioNoDisponible();
					setDesHabilitarBotonAceptar(false);
					return false;
				}

				// DOS: fechaActualizaDatosContacto.
				const fechaActualizacionContacto = obtenerFechaValida(
					fechaActualizaDatosContactoRaw,
				);

				/*
					Si DOS no trae fecha, se exige actualización.
					No se muestra alerta de servicio no disponible porque
					VIGIA sí respondió correctamente.
				*/
				if (!fechaActualizacionContacto) {
					await bloquearYRedirigirActualizacionDatos();
					return true;
				}

				const hoy = new Date();
				hoy.setHours(0, 0, 0, 0);

				const fechaLimite = new Date(fechaActualizacionContacto);
				fechaLimite.setHours(0, 0, 0, 0);
				fechaLimite.setFullYear(
					fechaLimite.getFullYear() + aniosPermitidos,
				);

				/*
					BAJO: fecha menor a 3 años.
					MEDIO: fecha menor a 2 años.
					ALTO: fecha menor a 1 año.
				*/
				const requiereActualizarDatos =
					hoy.getTime() >= fechaLimite.getTime();

				// console.log('[Validación actualización de datos]', {
				// 	fechaActualizaDatosContactoRaw,
				// 	riskLAFT: nivelHallazgo,
				// 	aniosPermitidos,
				// 	fechaLimite,
				// 	requiereActualizarDatos,
				// });

				if (requiereActualizarDatos) {
					await bloquearYRedirigirActualizacionDatos();
					return true;
				}

				return false;
			} catch (error) {
				console.error(
					'Error al consultar VIGIA para actualización de datos:',
					error,
				);

				await mostrarServicioNoDisponible();
				setDesHabilitarBotonAceptar(false);
				return false;
			}
		};

		setIsLoading(true);

		try {
			const response =
				await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(
					identificationType,
					identification,
				);

			const mensajeRespuestaDos = String(
				response?.message ??
				response?.errorMessage ??
				response?.detail ??
				'',
			)
				.normalize('NFD')
				.replace(/[\u0300-\u036f]/g, '')
				.toUpperCase();

			const estadoRespuestaDos = Number(
				response?.statusCode ?? response?.status,
			);

			/*
				Se considera afiliado inexistente si DOS responde correctamente,
				pero data llega null; también se cubre un eventual 404 o mensaje
				explícito de no encontrado.
			*/
			const afiliadoNoExiste =
				(response?.isSuccess === true && response?.data === null) ||
				estadoRespuestaDos === 404 ||
				/AFILIADO\s+NO\s+EXISTE|AFILIADO\s+NO\s+ENCONTRADO|NO\s+SE\s+ENCONTRO|NOT\s+FOUND/.test(
					mensajeRespuestaDos,
				);

			if (afiliadoNoExiste) {
				await mostrarServicioNoDisponible();

				// Este es el único error de consulta que bloquea el trámite.
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				setValue('numeroDocumento', '');
				setValue('typerequest', '');
				return;
			}

			const respuestaDosValida =
				response?.isSuccess === true &&
				response?.data !== null &&
				response?.data !== undefined &&
				Array.isArray(response.data?.solicitudes);

			if (!respuestaDosValida) {
				await mostrarServicioNoDisponible();

				// Demás fallas de DOS: informa, pero deja continuar.
				setDesHabilitarBotonAceptar(false);
				setIsLoading(false);
				return;
			}

			const datosAfiliado = response.data;

			setSolicitudes(datosAfiliado.solicitudes);
			obtenerFlujoDodo(identificationType, identification);

			const bloqueadoPorDesafiliacion =
				await validarDesafiliacionParaSubsidioSecuestrados(
					datosAfiliado.solicitudes,
				);

			if (bloqueadoPorDesafiliacion) {
				return;
			}

			if (
				datosAfiliado.idTipoActor === 1 &&
				tipoDSolicitud === retiroCesantias
			) {
				toast(
					'Tipo de Vinculación no aplica para este tipo de trámite',
					{ type: toast.TYPE.ERROR },
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const requiredFields = [
				'primerNombre',
			];

			const missingFields = requiredFields.filter((field) => {
				const value = datosAfiliado[field];
				return value === undefined || value === null || value === 0;
			});

			if (missingFields.length > 0) {
				await bloquearYRedirigirActualizacionDatos();
				return;
			}

			setDatoCuotas(datosAfiliado);

			// Validación general para todos los trámites.
			const fueRedirigidoPorActualizacion =
				await validarActualizacionDatosVigia(
					datosAfiliado?.fechaActualizaDatosContacto,
				);

			if (fueRedirigidoPorActualizacion) {
				return;
			}

			if (tipoDSolicitud === retiroCesantias) {
				const cuentas = Array.isArray(datosAfiliado.cuentas)
					? datosAfiliado.cuentas
					: [];

				const validacionCesantias =
					validarCuentaCesantiasParaRetiro(cuentas);

				if (!validacionCesantias.tieneCuenta) {
					NotificationService.genericAlwarning(
						'Alerta',
						'Tipo de cuenta del afiliado no aplica para retiro parcial de cesantías',
						'warning',
					);
					setDesHabilitarBotonAceptar(true);
					setIsLoading(false);
					return;
				}

				if (validacionCesantias.bloqueaRadicacionPorSaldo) {
					setDesHabilitarBotonAceptar(true);
					setIsLoading(false);

					await NotificationService.genericAlwarning(
						'Alerta',
						'El afiliado cuenta con el 100% del saldo disponible bloqueado para esta solicitud',
						'warning',
					);

					return;
				}

				if (
					validacionCesantias.totalSaldoConceptos <= 0 ||
					validacionCesantias.saldoDisponible <= 0
				) {
					NotificationService.genericAlwarning(
						'Alerta',
						'El afiliado cuenta con saldo 0',
						'warning',
					);
					setDesHabilitarBotonAceptar(true);
					setIsLoading(false);
					return;
				}
			}

			if (tipoDSolicitud === pagosExtraordinarios) {
				if (response.data?.solicitudes.length > 0) {
					const tieneDesafiliacion = response.data?.solicitudes.find((solicitud: any) => solicitud?.tipoSolicitud === 'DESAFILIACIONES' && (solicitud?.estadoSolicitud === 'PENDIENTE' || solicitud?.estadoSolicitud === 'PROCESADA'));

					if(tieneDesafiliacion){
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado presenta solicitud ${tieneDesafiliacion?.tipoSolicitud} en modalidad ${tieneDesafiliacion?.modalidad} en estado ${tieneDesafiliacion?.estadoSolicitud}`,
							'warning',
						);
						setDesHabilitarBotonAceptar(true);
						setIsLoading(false);
						return;
					}
				}
			}

			if (
				tipoDSolicitud === vivienda14 &&
				datosAfiliado.numeroCuotas < 168
			) {
				setDesHabilitarBotonAceptar(true);
				toast(
					'El número de cuotas aportadas no corresponde al tipo de trámite.',
					{ type: toast.TYPE.ERROR },
				);
				setIsLoading(false);
				return;
			}

			if (
				datosAfiliado.idTipoActor !== 1 &&
				(tipoDSolicitud === vivienda8 || tipoDSolicitud === vivienda14)
			) {
				toast(
					'Tipo de Vinculación no aplica para este tipo de trámite',
					{ type: toast.TYPE.ERROR },
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			let fechaReintegro: any;
			let fechaSolicitudDesafiliacion: any;

			const solicitudDesafiliacion = datosAfiliado.solicitudes.find(
				(item: any) =>
					item.tipoSolicitud === 'DESAFILIACION' &&
					item.estadoSolicitud === 'PROCESADA',
			);

			const solicitudDesafiliacionAportes = datosAfiliado.solicitudes.find(
				(item: any) =>
					item.tipoSolicitud ===
					'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' &&
					item.estadoSolicitud === 'PROCESADA',
			);

			const solicitudDesafiliacionCesantias = datosAfiliado.solicitudes.find(
				(item: any) =>
					item.tipoSolicitud ===
					'DESAFILIACION CI SOLUCION VIVIENDA CON CESANTÍAS' &&
					item.estadoSolicitud === 'PROCESADA',
			);

			const solicitudReintegro = datosAfiliado.solicitudes.find(
				(item: any) =>
					item.tipoSolicitud === 'REINTEGROS' &&
					item.estadoSolicitud === 'PROCESADA',
			);

			if (
				solicitudDesafiliacion &&
				(
					tipoDSolicitud === retiroCesantias ||
					tipoDSolicitud === vivienda8 ||
					tipoDSolicitud === vivienda14
				)
			) {
				fechaSolicitudDesafiliacion =
					solicitudDesafiliacion.fechaFinSolicitud;

				if (solicitudReintegro) {
					fechaReintegro = solicitudReintegro.fechaFinSolicitud;

					if (
						fechaReintegro &&
						fechaSolicitudDesafiliacion &&
						new Date(fechaReintegro) <
						new Date(fechaSolicitudDesafiliacion)
					) {
						NotificationService.genericAlwarning(
							'Advertencia',
							`Trámite desafiliación por ${solicitudDesafiliacion.modalidad?.toLowerCase()}.`,
							'warning',
						);
						setDesHabilitarBotonAceptar(true);
						setIsLoading(false);
						return;
					}
				} else {
					NotificationService.genericAlwarning(
						'Advertencia',
						`Trámite desafiliación por ${solicitudDesafiliacion.modalidad?.toLowerCase()}.`,
						'warning',
					);
					setDesHabilitarBotonAceptar(true);
					setIsLoading(false);
					return;
				}
			}

			if (
				solicitudDesafiliacionAportes &&
				(tipoDSolicitud === vivienda8 || tipoDSolicitud === vivienda14)
			) {
				fechaSolicitudDesafiliacion =
					solicitudDesafiliacionAportes.fechaFinSolicitud;

				if (solicitudReintegro) {
					fechaReintegro = solicitudReintegro.fechaFinSolicitud;

					if (
						fechaReintegro &&
						fechaSolicitudDesafiliacion &&
						new Date(fechaReintegro) <
						new Date(fechaSolicitudDesafiliacion)
					) {
						NotificationService.genericAlwarning(
							'Advertencia',
							'Trámite de desafiliación, modalidad solo aportes; procesado',
							'warning',
						);
						setDesHabilitarBotonAceptar(true);
						setIsLoading(false);
						return;
					}
				} else {
					NotificationService.genericAlwarning(
						'Advertencia',
						'Trámite de desafiliación, modalidad solo aportes; procesado',
						'warning',
					);
					setDesHabilitarBotonAceptar(true);
					setIsLoading(false);
					return;
				}
			}

			if (
				solicitudDesafiliacionCesantias &&
				(tipoDSolicitud === vivienda8 || tipoDSolicitud === vivienda14)
			) {
				fechaSolicitudDesafiliacion =
					solicitudDesafiliacionCesantias.fechaFinSolicitud;

				if (solicitudReintegro) {
					fechaReintegro = solicitudReintegro.fechaFinSolicitud;

					if (
						fechaReintegro &&
						fechaSolicitudDesafiliacion &&
						new Date(fechaReintegro) <
						new Date(fechaSolicitudDesafiliacion)
					) {
						NotificationService.genericAlwarning(
							'Advertencia',
							'Trámite de desafiliación, con cesantías; procesado',
							'warning',
						);
						setDesHabilitarBotonAceptar(true);
						setIsLoading(false);
						return;
					}
				} else {
					NotificationService.genericAlwarning(
						'Advertencia',
						'Trámite de desafiliación, con cesantías; procesado',
						'warning',
					);
					setDesHabilitarBotonAceptar(true);
					setIsLoading(false);
					return;
				}
			}

			const solicitudPendiente = datosAfiliado.solicitudes.find(
				(item: any) =>
					(item.tipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS' || 
					 item.tipoSolicitud === 'DEVOLUCIÓN DE SALDOS') &&
					![
						'ANULADA',
						'INCONSISTENTE',
						'PROCESADA',
						'APROBADA',
					].includes(item.estadoSolicitud),
			);

			if (solicitudPendiente) {
				NotificationService.genericAlwarning(
					'Advertencia',
					`El afiliado tiene un proceso de ${solicitudPendiente.tipoSolicitud}, en estado ${solicitudPendiente.estadoSolicitud}.`,
					'warning',
				);
				setIsLoading(false);
				setDesHabilitarBotonAceptar(true);
				return;
			}

			if (datosAfiliado.situacionActual === 'FALLECIDO') {
				NotificationService.genericAlwarning(
					'Advertencia',
					'Trámite de desafiliación, modalidad muerte del afiliado; procesado',
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const solicitudes: any[] = Array.isArray(datosAfiliado.solicitudes)
				? datosAfiliado.solicitudes
				: [];

			const cuentas: any[] = Array.isArray(datosAfiliado.cuentas)
				? datosAfiliado.cuentas
				: [];

			const contieneMuerteAfiliado = solicitudes.some(
				(solicitud: any) =>
					solicitud.modalidad === 'MUERTE DEL AFILIADO' &&
					solicitud.estadoSolicitud === 'PROCESADA' &&
					tipoDSolicitud === acreditacion8,
			);

			if (contieneMuerteAfiliado) {
				await NotificationService.genericAlert(
					'Advertencia',
					'Trámite desafiliación por muerte del afiliado.',
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const retiroInstitucion = solicitudes.some(
				(solicitud: any) =>
					solicitud.modalidad === 'RETIRO DE LA INSTITUCIÓN' &&
					solicitud.estadoSolicitud === 'PROCESADA' &&
					tipoDSolicitud === acreditacion8,
			);

			if (retiroInstitucion) {
				await NotificationService.genericAlert(
					'Advertencia',
					'Trámite desafiliación por retiro de la institución.',
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const presentaSolicitudV8oMasvi = solicitudes.find(
				(solicitud: any) =>
					(
						String(solicitud.actividad ?? '')
							.toUpperCase()
							.includes('V 8') ||
						solicitud.tipoSolicitud === 'MASVI'
					) &&
					!['ANULADA', 'INCONSISTENTE'].includes(
						solicitud.estadoSolicitud,
					),
			);

			if (
				presentaSolicitudV8oMasvi &&
				tipoDSolicitud === vivienda8
			) {

				const estadoSolicitud =
					String(
						presentaSolicitudV8oMasvi
							?.estadoSolicitud ?? '',
					)
						.trim()
						.toUpperCase();

				const estadoSolicitudMostrar =
					estadoSolicitud === 'RADICADA'
						? 'PENDIENTE'
						: estadoSolicitud;

				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta solicitud de MASVI en estado: ${estadoSolicitudMostrar}`,
					'warning',
				);

				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const tieneSolicitudV8oMasvi = solicitudes.some(
				(solicitud: any) =>
					(
						String(solicitud.actividad ?? '')
							.toUpperCase()
							.includes('V 8') ||
						solicitud.tipoSolicitud === 'MASVI'
					) &&
					solicitud.estadoSolicitud === 'PROCESADA',
			);

			if (!tieneSolicitudV8oMasvi && tipoDSolicitud === acreditacion8) {
				await NotificationService.genericAlert(
					'Advertencia',
					'Afiliado no presenta solicitud de Vivienda 8.',
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const procesoV8Pendiente = solicitudes.some(
				(solicitud: any) =>
					solicitud.tipoSolicitud ===
					'MODELO ANTICIPADO DE SOLUCIÓN DE VIVIENDA' &&
					solicitud.estadoSolicitud === 'PENDIENTE' &&
					tipoDSolicitud === acreditacion8,
			);

			if (procesoV8Pendiente) {
				await NotificationService.genericAlert(
					'Advertencia',
					'Afiliado presenta solicitud de Vivienda 8 en estado PENDIENTE.',
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const solicitudV8Bloqueante = solicitudes.find(
				(solicitud: any) =>
					String(solicitud.actividad ?? '')
						.toUpperCase()
						.includes('V 8') &&
					!['INCONSISTENTE', 'ANULADO'].includes(
						String(solicitud.estadoSolicitud ?? '').toUpperCase(),
					),
			);

			if (
				solicitudV8Bloqueante &&
				(tipoDSolicitud === vivienda8 || tipoDSolicitud === vivienda14)
			) {
				await NotificationService.genericAlert(
					'Advertencia',
					`El afiliado tiene una solicitud con actividad "${solicitudV8Bloqueante.actividad}" en estado "${solicitudV8Bloqueante.estadoSolicitud}".`,
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			// Validaciones AVAC/AVAV
			if (tipoDSolicitud === devolucionAvacAvav) {

				if (solicitudes?.length > 0) {
					const solicitudAvacAvavPendiente = solicitudes?.find((solicitud: any) => solicitud?.tipoSolicitud === 'DEVOLUCIÓN DE AHORROS AVAC - AVAV' && solicitud?.estadoSolicitud === 'PENDIENTE');
					if (solicitudAvacAvavPendiente) {
						await NotificationService.genericAlert(
							'Advertencia',
							`Afiliado presenta solicitud de ${solicitudAvacAvavPendiente?.modalidad} en estado ${solicitudAvacAvavPendiente?.estadoSolicitud}`,
							'warning',
						);
						setDesHabilitarBotonAceptar(true);
						setIsLoading(false);
						return;
					}
				}

				if (cuentas?.length > 0) {

					// Validación saldos AVAC
					const cuentasAvac = cuentas?.filter((cuenta: any) => cuenta?.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS');
					if (cuentasAvac.length > 0) {
						const tieneSaldo = cuentasAvac.some((cuenta: any) => cuenta.saldoDisponible > 0);
						if (!tieneSaldo) {
							await NotificationService.genericAlert(
								'Advertencia',
								'El afiliado no cuenta con saldo disponible para esta solicitud.',
								'warning',
							);
							setDesHabilitarBotonAceptar(true);
							setIsLoading(false);
							return;
						}
					}

					// Validación saldos AVAV
					const cuentasAvav = cuentas?.filter((cuenta: any) => cuenta?.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO');
					if (cuentasAvav.length > 0) {
						const tieneSaldo = cuentasAvav.some((cuenta: any) => cuenta.saldoDisponible > 0);
						if (!tieneSaldo) {
							await NotificationService.genericAlert(
								'Advertencia',
								'El afiliado no cuenta con saldo disponible para esta solicitud.',
								'warning',
							);
							setDesHabilitarBotonAceptar(true);
							setIsLoading(false);
							return;
						}
					}
				}
			}

			const cuentasCesantias = cuentas.filter((cuenta: any) =>
				String(cuenta.tipoCuenta ?? '')
					.toUpperCase()
					.includes('CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS'),
			);

			const tieneSaldo = cuentasCesantias.some(
				(cuenta: any) => cuenta.saldoDisponible > 0,
			);

			if (
				tipoDSolicitud !== retiroCesantias &&
				tipoDSolicitud !== devolucionAvacAvav &&
				cuentasCesantias.length > 0 &&
				!tieneSaldo
			) {
				await NotificationService.genericAlert(
					'Advertencia',
					'El afiliado no cuenta con saldo disponible para esta solicitud.',
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			const solicitudV8Masvi = solicitudes.find(
				(solicitud: any) =>
					solicitud.tipoSolicitud === 'MASVI' &&
					!['INCONSISTENTE', 'ANULADO'].includes(
						String(solicitud.estadoSolicitud ?? '').toUpperCase(),
					),
			);

			if (solicitudV8Masvi && tipoDSolicitud === vivienda8) {
				await NotificationService.genericAlert(
					'Advertencia',
					`El afiliado cuenta con un proceso masvi en estado, "${solicitudV8Masvi.estadoSolicitud}".`,
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			let tiposVinculacion: any;

			if (datosAfiliado.tiposVinculacion) {
				tiposVinculacion = datosAfiliado.tiposVinculacion.find(
					(element: any) =>
						element.idTipoActor === datosAfiliado.idTipoActor,
				);
			}

			if (
				tiposVinculacion &&
				(
					tiposVinculacion.descripcionTipoActor ===
					'AFILIADO ADMINISTRACION CESANTIAS' ||
					tiposVinculacion.descripcionTipoActor ===
					'AFILIADO ADMINISTRACIÓN CESANTÍAS'
				) &&
				(
					tipoMidalidadValidar === vivienda8 ||
					tipoMidalidadValidar === vivienda14
				)
			) {
				NotificationService.genericAlwarning(
					'Advertencia',
					'Tipo de vinculación no corresponde al tipo de trámite',
					'warning',
				);
				setIsLoading(false);
				setDesHabilitarBotonAceptar(true);
				return;
			}

			const solicitudBloqueo = solicitudes.find(
				(solicitud: any) =>
					!['ANULADA', 'INCONSISTENTE'].includes(
						solicitud.estadoSolicitud,
					) &&
					solicitud.modalidad === 'ACREDITACION V8' &&
					solicitud.tipoSolicitud === 'ACREDITACION V8',
			);

			if (solicitudBloqueo) {
				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta solicitud de Acreditación en estado, "${solicitudBloqueo.estadoSolicitud}".`,
					'warning',
				);
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);
				return;
			}

			setDesHabilitarBotonAceptar(false);
			setIsLoading(false);
		} catch (error: any) {
			console.error(
				'Error al consultar afiliado en obtenerNumerosCuota:',
				error,
			);

			await mostrarServicioNoDisponible();

			// Error técnico general: muestra alerta, pero permite continuar.
			setDesHabilitarBotonAceptar(false);
			setIsLoading(false);
		}
	};

	useEffect(() => {
		const validarBloqueo = async () => {
			if (tipoDSolicitud === '5b0a16af-16ef-40f2-b384-a46fe00b0f63') {
				const solicitudV8NoAcreditacion = solicitudes.find(
					(solicitud: any) =>
						solicitud.modalidad?.toUpperCase() === 'NO ACREDITACION V8' &&
						!["INCONSISTENTE", "ANULADO"].includes(
							solicitud.estadoSolicitud?.toUpperCase()
						)
				);

				if (solicitudV8NoAcreditacion) {
					setDesHabilitarBotonAceptar(true);
					await NotificationService.genericAlert(
						"Advertencia",
						`El afiliado tiene una solicitud con modalidad "${solicitudV8NoAcreditacion.modalidad}" en estado "${solicitudV8NoAcreditacion.estadoSolicitud}".`,
						"warning"
					);

					return;
				}
			}

			// si no aplica, desbloquear
			if (tipoDSolicitud === '5b0a16af-16ef-40f2-b384-a46fe00b0f63') {
				setDesHabilitarBotonAceptar(false);
			}
		};

		validarBloqueo();
	}, [tipoDSolicitud, solicitudes]);

	const [tipoSolicitud, setTipoSolicitud] = useState<any>();

	const validarBloqueo = (idTipoSolicitud: string, solicitudes: any[]) => {

		const solicitudBloqueo = solicitudes.find((solicitud: any) =>
			!['ANULADA', 'INCONSISTENTE'].includes(solicitud.estadoSolicitud) &&
			solicitud.modalidad === 'ACREDITACION V8' &&
			solicitud.tipoSolicitud === 'ACREDITACION V8' &&
			idTipoSolicitud === '5b0a16af-16ef-40f2-b384-a46fe00b0f63'
		);


		if (solicitudBloqueo) {
			setDesHabilitarBotonAceptar(true);
			NotificationService.genericAlwarning(
				'Alerta',
				`El afiliado presenta solicitud de Acreditación en estado "${solicitudBloqueo.estadoSolicitud}".`,
				'warning'
			);
		} else {
			setDesHabilitarBotonAceptar(false);
		}
	};


	const classes = useStyles();

	const [cambiarEstadoTramite, setCambiarEstadoTramite] = useState(false);

	const obtenerAcuerdo = (fechaInicio: string) => {
		if (fechaInicio >= '2020-09-24') return 'Acuerdo 02 de 2020';
		if (fechaInicio >= '2017-12-20' && fechaInicio <= '2020-09-23') return 'Acuerdo 05 de 2017';
		if (fechaInicio >= '2016-04-29' && fechaInicio <= '2017-12-19') return 'Acuerdo 01 de 2016';
		if (fechaInicio >= '2015-06-05' && fechaInicio <= '2016-04-28') return 'Acuerdo 01 de 2015';
		if (fechaInicio >= '2013-06-07' && fechaInicio <= '2015-06-04') return 'Acuerdo 02 de 2013';
		if (fechaInicio >= '2011-02-12' && fechaInicio <= '2013-06-06') return 'Acuerdo 01 de 2011';
		if (fechaInicio >= '2009-10-21' && fechaInicio <= '2011-02-11') return 'Acuerdo 04 de 2009';

		return '';
	};



	const normalizarTexto = (valor: any): string =>
		valor
			?.toString()
			.trim()
			.toUpperCase()
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '') || '';

	const CUENTA_CESANTIAS = 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS';

	const TIPO_SOLICITUD_SUBSIDIO_SECUESTRADOS =
		'd766cd98-00bc-40fe-971a-a0cb1c2ce788';

	const ESTADOS_DESAFILIACION_BLOQUEANTES = [
		'PENDIENTE',
		'PROCESADA',
	];

	type ResultadoValidacionResciliacion = {
		permitirRadicacion: boolean;
		mensaje?: string;
	};

	const esSolicitudDeResciliacion = (): boolean => {
		const tipoSolicitudSeleccionado =
			catalogState.typeRequest?.find(
				(item: TYPEREQUEST) =>
					String(item.idTipoSolicitud) ===
					String(tipoDSolicitud),
			);

		return (
			normalizarTexto(
				tipoSolicitudSeleccionado
					?.descripcionTipoSolicitud,
			) === 'RESCILIACION'
		);
	};

	const obtenerModalidadSeleccionada =
		(): TYPEMODALITY | undefined =>
			catalogState.typeModality?.find(
				(item: TYPEMODALITY) =>
					String(item.idTipoModalidad) ===
					String(tipoMidalidadValidar),
			);

	const esTramiteVivienda8 = (
		solicitud: any,
	): boolean => {
		const tipoSolicitud = normalizarTexto(
			solicitud?.tipoSolicitud,
		);
		const modalidad = normalizarTexto(
			solicitud?.modalidad,
		);
		const actividad = normalizarTexto(
			solicitud?.actividad,
		);
		const descripcionProceso = normalizarTexto(
			solicitud?.descripcionProceso,
		);

		return (
			tipoSolicitud === 'VIVIENDA 8' ||
			modalidad === 'VIVIENDA 8' ||
			tipoSolicitud === 'MASVI' ||
			tipoSolicitud ===
				'MODELO ANTICIPADO DE SOLUCION DE VIVIENDA' ||
			tipoSolicitud ===
				'MODELO ANTICIPADO DE SOLUCION DE VIVIENDA 8' ||
			modalidad.includes('VIVIENDA 8') ||
			actividad.includes('VIVIENDA 8') ||
			actividad.includes('V 8') ||
			descripcionProceso.includes('VIVIENDA 8')
		);
	};

	const esSolicitudNoAcreditacion = (
		solicitud: any,
	): boolean =>
		[
			solicitud?.tipoSolicitud,
			solicitud?.modalidad,
			solicitud?.subModalidad,
			solicitud?.submodalidad,
			solicitud?.actividad,
			solicitud?.descripcionProceso,
			solicitud?.nombreTarea,
		].some(valor =>
			normalizarTexto(valor).includes(
				'NO ACREDITACION',
			),
		);

	const validarResciliacionVivienda8 = (
		datosAfiliado: any,
	): ResultadoValidacionResciliacion => {
		if (!esSolicitudDeResciliacion()) {
			return { permitirRadicacion: true };
		}

		const modalidadSeleccionada =
			obtenerModalidadSeleccionada();
		const descripcionModalidad = normalizarTexto(
			modalidadSeleccionada
				?.descripcionTipoModalidad,
		);
		const esModalidadVivienda8 =
			descripcionModalidad.includes('VIVIENDA 8');

		if (!esModalidadVivienda8) {
			return { permitirRadicacion: true };
		}

		const solicitudesAfiliado = Array.isArray(
			datosAfiliado?.solicitudes,
		)
			? datosAfiliado.solicitudes
			: [];

		const solicitudesVivienda8Procesadas =
			solicitudesAfiliado.filter(
				(solicitud: any) =>
					esTramiteVivienda8(solicitud) &&
					normalizarTexto(
						solicitud?.estadoSolicitud,
					) === 'PROCESADA',
			);

		console.log('[VALIDACIÓN RESCILIACIÓN]', {
			tipoDSolicitud,
			tipoMidalidadValidar,
			descripcionModalidad,
			esModalidadVivienda8,
			solicitudesVivienda8Procesadas,
		});

		if (solicitudesVivienda8Procesadas.length === 0) {
			return {
				permitirRadicacion: false,
				mensaje:
					'Afiliado no cuenta con tramite de Vivienda 8 procesado',
			};
		}

		const presentaNoAcreditacion =
			solicitudesAfiliado.some(
				(solicitud: any) =>
					esSolicitudNoAcreditacion(solicitud) &&
					['PENDIENTE', 'APROBADA'].includes(
						normalizarTexto(
							solicitud?.estadoSolicitud,
						),
					),
			);

		if (presentaNoAcreditacion) {
			return {
				permitirRadicacion: false,
				mensaje:
					'Afiliado presenta solicitud de no acreditación',
			};
		}

		return { permitirRadicacion: true };
	};

	const esSolicitudDesafiliacion = (
		solicitud: any,
	): boolean => {
		const tipoSolicitudNormalizado = normalizarTexto(
			solicitud?.tipoSolicitud,
		);

		return tipoSolicitudNormalizado.includes(
			'DESAFILIACION',
		);
	};

	const obtenerDesafiliacionBloqueante = (
		solicitudes: any[],
	): any | undefined => {
		if (!Array.isArray(solicitudes)) {
			return undefined;
		}

		return solicitudes.find((solicitud: any) => {
			const estadoNormalizado = normalizarTexto(
				solicitud?.estadoSolicitud,
			);

			return (
				esSolicitudDesafiliacion(solicitud) &&
				ESTADOS_DESAFILIACION_BLOQUEANTES.includes(
					estadoNormalizado,
				)
			);
		});
	};

	const validarDesafiliacionParaSubsidioSecuestrados = async (
		solicitudesAfiliado: any[],
	): Promise<boolean> => {
		if (
			tipoDSolicitud !==
			TIPO_SOLICITUD_SUBSIDIO_SECUESTRADOS
		) {
			return false;
		}

		const solicitudDesafiliacion =
			obtenerDesafiliacionBloqueante(
				solicitudesAfiliado,
			);

		if (!solicitudDesafiliacion) {
			return false;
		}

		const tipoSolicitudEncontrada = String(
			solicitudDesafiliacion?.tipoSolicitud ??
			'DESAFILIACIONES',
		).trim();

		const estadoEncontrado = String(
			solicitudDesafiliacion?.estadoSolicitud ?? '',
		).trim();

		await NotificationService.genericAlwarning(
			'Alerta',
			`Afiliado presenta solicitud ${tipoSolicitudEncontrada} en estado ${estadoEncontrado}`,
			'warning',
		);

		setDesHabilitarBotonAceptar(true);
		setIsLoading(false);

		return true;
	};

	const convertirNumero = (valor: any): number => {
		if (typeof valor === 'number') return valor;

		if (valor === null || valor === undefined) return 0;

		let limpio = valor
			.toString()
			.replace(/[^\d,.-]/g, '');

		if (!limpio) return 0;

		const ultimaComa = limpio.lastIndexOf(',');
		const ultimoPunto = limpio.lastIndexOf('.');

		if (ultimaComa > ultimoPunto) {
			limpio = limpio.replace(/\./g, '').replace(',', '.');
		} else {
			limpio = limpio.replace(/,/g, '');
		}

		const numero = Number(limpio);
		return Number.isFinite(numero) ? numero : 0;
	};

	const obtenerPrimerValorNumerico = (objeto: any, campos: string[]): number => {
		const campo = campos.find(
			key => objeto?.[key] !== undefined && objeto?.[key] !== null
		);

		return campo ? convertirNumero(objeto[campo]) : 0;
	};

	const obtenerTotalSaldoConceptosCuenta = (cuenta: any): number => {
		const valorDirecto = obtenerPrimerValorNumerico(cuenta, [
			'totalSaldoConceptos',
			'totalSaldosConceptos',
			'saldoTotalConceptos',
			'totalSaldo',
			'saldo',
		]);

		if (valorDirecto > 0) return valorDirecto;

		const conceptos = Array.isArray(cuenta?.conceptosSaldosBloqueos)
			? cuenta.conceptosSaldosBloqueos
			: [];

		return conceptos.reduce(
			(total: number, concepto: any) =>
				total +
				obtenerPrimerValorNumerico(concepto, [
					'saldo',
					'valorSaldo',
					'saldoConcepto',
					'valorConcepto',
				]),
			0
		);
	};

	const obtenerTotalBloqueosEmbargosCuenta = (cuenta: any): number => {
		const valorDirecto = obtenerPrimerValorNumerico(cuenta, [
			'totalBloqueosEmbargos',
			'totalBloqueosYOEmbargos',
			'totalBloqueosYEmbargos',
			'totalBloqueos',
			'bloqueo',
			'saldoBloqueado',
		]);

		if (valorDirecto > 0) return valorDirecto;

		const conceptos = Array.isArray(cuenta?.conceptosSaldosBloqueos)
			? cuenta.conceptosSaldosBloqueos
			: [];

		return conceptos.reduce(
			(total: number, concepto: any) =>
				total +
				obtenerPrimerValorNumerico(concepto, [
					'bloqueo',
					'valorBloqueo',
					'valorAcumulado',
					'valorEmbargo',
				]),
			0
		);
	};

	type ValidacionCuentaCesantiasRetiro = {
		tieneCuenta: boolean;
		totalSaldoConceptos: number;
		totalBloqueosEmbargos: number;
		totalBloqueos: number;
		totalEmbargos: number;
		saldoDisponible: number;
		tieneSaldo100Bloqueado: boolean;
		tieneBloqueo: boolean;
		tieneEmbargo: boolean;
		razonEmbargo: string;
		bloqueaRadicacionPorBloqueo: boolean;
		tieneBloqueoParcial: boolean;
		tieneSaldoDisponible: boolean;
		tieneSaldoNegativo: boolean;
		bloqueaRadicacionPorSaldo: boolean;
	};

	const obtenerSaldoPositivoCuenta = (cuenta: any): number => {
		const camposSaldo = [
			'saldo',
			'valorSaldo',
			'saldoConcepto',
			'valorConcepto',
		];

		const conceptos = Array.isArray(cuenta?.conceptosSaldosBloqueos)
			? cuenta.conceptosSaldosBloqueos
			: [];

		const existenSaldosEnConceptos = conceptos.some((concepto: any) =>
			camposSaldo.some(
				campo =>
					concepto?.[campo] !== undefined &&
					concepto?.[campo] !== null,
			),
		);

		// Cuando existen detalles, solo suma los saldos positivos.
		// Los saldos negativos no deben disminuir el saldo que está bloqueado.
		if (existenSaldosEnConceptos) {
			return conceptos.reduce((total: number, concepto: any) => {
				const saldoConcepto = obtenerPrimerValorNumerico(
					concepto,
					camposSaldo,
				);

				return total + Math.max(saldoConcepto, 0);
			}, 0);
		}

		// Respaldo cuando la API no trae el detalle por conceptos.
		const saldoCuenta = obtenerPrimerValorNumerico(cuenta, [
			'totalSaldoConceptos',
			'totalSaldosConceptos',
			'saldoTotalConceptos',
			'totalSaldo',
			'saldo',
		]);

		return Math.max(saldoCuenta, 0);
	};


	const tieneSaldoNegativoCuenta = (cuenta: any, margen = 0.01): boolean => {
		const camposCuenta = [
			'saldoDisponible',
			'totalSaldoConceptos',
			'totalSaldosConceptos',
			'saldoTotalConceptos',
			'totalSaldo',
			'saldo',
		];

		const saldoNegativoEnCuenta = camposCuenta.some((campo) => {
			const valor = cuenta?.[campo];

			return (
				valor !== undefined &&
				valor !== null &&
				convertirNumero(valor) < -margen
			);
		});

		const conceptos = Array.isArray(cuenta?.conceptosSaldosBloqueos)
			? cuenta.conceptosSaldosBloqueos
			: [];

		const saldoNegativoEnConceptos = conceptos.some((concepto: any) => {
			const saldoConcepto = obtenerPrimerValorNumerico(concepto, [
				'saldo',
				'valorSaldo',
				'saldoConcepto',
				'valorConcepto',
			]);

			return saldoConcepto < -margen;
		});

		return saldoNegativoEnCuenta || saldoNegativoEnConceptos;
	};

	const validarCuentaCesantiasParaRetiro = (
		cuentas: any[],
		tipoCuentaObjetivo = CUENTA_CESANTIAS,
	): ValidacionCuentaCesantiasRetiro => {
		const cuentasCesantias = cuentas.filter(
			(cuenta: any) =>
				normalizarTexto(cuenta?.tipoCuenta) ===
				normalizarTexto(tipoCuentaObjetivo),
		);


		const totalBloqueos = 0;
		const totalEmbargos = 0;
		const razonesEmbargo: string[] = [];

		const margen = 0.01;

		const tieneSaldoNegativo = cuentasCesantias.some((cuenta: any) =>
			tieneSaldoNegativoCuenta(cuenta, margen),
		);

		const resumenCuentas = cuentasCesantias.map((cuenta: any) => ({
			saldoNeto: obtenerTotalSaldoConceptosCuenta(cuenta),
			saldoPositivo: obtenerSaldoPositivoCuenta(cuenta),
			bloqueosEmbargos: obtenerTotalBloqueosEmbargosCuenta(cuenta),
		}));

		const totalBloqueosEmbargos = resumenCuentas.reduce(
			(total: number, cuenta: any) => total + cuenta.bloqueosEmbargos,
			0,
		);

		const tieneBloqueoOEmbargo =
			totalBloqueosEmbargos > margen ||
			totalBloqueos > margen ||
			totalEmbargos > margen;

		// const tieneSaldo100Bloqueado =
		// 	totalSaldoConceptos > margen &&
		// 	totalBloqueosEmbargos >= totalSaldoConceptos - margen;



		const totalSaldoConceptos = resumenCuentas.reduce(
			(total: number, cuenta: any) => total + cuenta.saldoNeto,
			0,
		);


		const tieneSaldo100Bloqueado = resumenCuentas.some(
			(cuenta: any) =>
				cuenta.saldoPositivo > margen &&
				cuenta.bloqueosEmbargos >= cuenta.saldoPositivo - margen,
		);

		const bloqueaRadicacionPorSaldo =
			tieneSaldo100Bloqueado || tieneSaldoNegativo;

		const saldoDisponible = totalSaldoConceptos - totalBloqueosEmbargos;
		const tieneSaldoDisponible = saldoDisponible > margen;

		const bloqueaRadicacionPorBloqueo =
			tieneBloqueoOEmbargo && !tieneSaldoDisponible;

		const tieneBloqueoParcial =
			tieneBloqueoOEmbargo && tieneSaldoDisponible;

		const razonEmbargo =
			razonesEmbargo.length > 0
				? razonesEmbargo
					.filter((razon, index, array) => array.indexOf(razon) === index)
					.join(', ')
				: 'embargo';

		return {
			tieneCuenta: cuentasCesantias.length > 0,
			totalSaldoConceptos,
			totalBloqueosEmbargos,
			totalBloqueos,
			totalEmbargos,
			saldoDisponible,
			tieneSaldoDisponible,
			tieneSaldo100Bloqueado,
			tieneBloqueo: totalBloqueos > margen,
			tieneEmbargo: totalEmbargos > margen,
			tieneBloqueoParcial,
			bloqueaRadicacionPorBloqueo,
			razonEmbargo,
			tieneSaldoNegativo,
			bloqueaRadicacionPorSaldo
		};
	};

	const onSubmit = async () => {

		if (!tipoSolicitud) {
			NotificationService.genericAlwarning(
				'Alerta',
				'Tipo de solicitud no definido.',
				'warning'
			);
			return;
		}

		const subsidioSecuestrados = 'd766cd98-00bc-40fe-971a-a0cb1c2ce788';
		const vivienda8 = 'b61024bb-f484-4aa5-94f5-73d0750d95a2';
		const vivienda14 = '4503b975-fa78-45d7-9860-dbcaa9957eb6';
		const retiroCesantias = '27b2c612-bde0-42a1-8447-024cd0edfb2c';
		const devolucionAvacAvav = '6f659d82-1711-46a9-b21f-9904313ef9ad';
		const tipoSolicitudDevolucionSaldoCuenta = 'ac59d836-656a-4f84-82ed-6f4a9914ef72';
		const acreditacion8 = '5b0a16af-16ef-40f2-b384-a46fe00b0f63';

		const tipoSolicitudRetiroParcialCesantias = '2fe2f564-2778-442b-a278-ef6ebfc5e6e9';

		const solicitudesAfiliado = Array.isArray(
			datoCuota?.solicitudes,
		)
			? datoCuota.solicitudes
			: [];

		const resultadoValidacionResciliacion =
			validarResciliacionVivienda8(datoCuota);

		if (!resultadoValidacionResciliacion.permitirRadicacion) {
			await NotificationService.genericAlwarning(
				'Advertencia',
				resultadoValidacionResciliacion.mensaje ?? '',
				'warning',
			);

			setLoading(false);
			setIsLoading(false);
			return;
		}

		const bloqueadoPorDesafiliacion =
			await validarDesafiliacionParaSubsidioSecuestrados(
				solicitudesAfiliado,
			);

		if (bloqueadoPorDesafiliacion) {
			return;
		}

		const esRetiroParcialCesantias =
			tipoDSolicitud === tipoSolicitudRetiroParcialCesantias ||
			tipoSolicitud === tipoSolicitudRetiroParcialCesantias;

		if (esRetiroParcialCesantias) {
			const cuentas = Array.isArray(datoCuota?.cuentas)
				? datoCuota.cuentas
				: [];

			const validacionCesantias =
				validarCuentaCesantiasParaRetiro(cuentas);

			if (!validacionCesantias.tieneCuenta) {
				NotificationService.genericAlwarning(
					'Alerta',
					'Tipo de cuenta del afiliado no aplica para retiro parcial de cesantías',
					'warning'
				);
				return;
			}

			let totalBloqueos = 0;
			let totalEmbargos = 0;

			const razonesEmbargo = new Set<string>();

			cuentas.forEach((cuenta: any) => {
				const conceptos = Array.isArray(
					cuenta?.conceptosSaldosBloqueos
				)
					? cuenta.conceptosSaldosBloqueos
					: [];

				conceptos.forEach((concepto: any) => {
					const razonOriginal = String(
						concepto?.razon ?? ''
					).trim();

					const razonNormalizada =
						normalizarTexto(razonOriginal);

					const valorConcepto = convertirNumero(
						concepto?.valorAcumulado ??
						concepto?.valorEmbargo ??
						concepto?.valorBloqueo ??
						concepto?.bloqueo ??
						0
					);

					const esEmbargo =
						razonNormalizada.includes('EMBARGO');

					if (esEmbargo) {
						totalEmbargos += valorConcepto;

						if (razonOriginal) {
							razonesEmbargo.add(razonOriginal);
						}

						return;
					}

					if (valorConcepto > 0) {
						totalBloqueos += valorConcepto;
					}
				});
			});

			if (totalBloqueos > 0) {
				await NotificationService.messageAlert(
					'Alerta',
					'Afiliado presenta bloqueos en el saldo disponible de la cuenta',
					'warning'
				);
			}

			if (totalEmbargos > 0) {
				const razonEmbargo =
					Array.from(razonesEmbargo).join(', ') ||
					'embargo';

				await NotificationService.messageAlert(
					'Alerta',
					`Presenta un embargo por ${razonEmbargo} de ${UtilService.formatCurrency(
						totalEmbargos
					)}`,
					'warning'
				);
			}

			if (
				validacionCesantias.totalSaldoConceptos <= 0 ||
				validacionCesantias.saldoDisponible <= 0
			) {
				NotificationService.genericAlwarning(
					'Alerta',
					'El afiliado cuenta con saldo 0',
					'warning'
				);
				return;
			}
		}

		// Submodalidades
		const submodalidadV8ViviendaUsada = 'd42f1ca2-6d25-448b-9652-20c144489639';

		const tpoDeSolicitud = datoCuota.solicitudes.some(
			(item: any) => vivienda8 === tipoSolicitud
		);

		const filtroTipoSolicitud = datoCuota.solicitudes.filter(
			(item: any) => vivienda8 === tipoSolicitud
		);

		const solicitudesTipoActual = datoCuota.solicitudes.filter(
			(item: any) => item.tipoSolicitud === tipoSolicitud
		);

		const estadoNoPermitido = [
			'PROCESADA', 'EN TRÁMITE', 'PENDIENTE',
			'EN SUBSANACIÓN', 'RECHAZADA', 'ANULADA',
			'BLOQUEADO', 'EN VERIFICACION'
		];

		const solicitudConEstadoNoPermitido = solicitudesTipoActual.some(
			(item: any) => estadoNoPermitido.includes(item.estadoSolicitud)
		);

		if (solicitudConEstadoNoPermitido) {
			const solicitud = solicitudesTipoActual.find(
				(item: any) => estadoNoPermitido.includes(item.estadoSolicitud)
			);

			if (solicitud) {
				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta solicitud de MASVI en estado: ${solicitud.estadoSolicitud}`,
					'warning'
				);
				return;
			}
		}

		// Validaciones Devolucion Saldo 
		if (tipoDSolicitud === tipoSolicitudDevolucionSaldoCuenta) {
			
			if(descripSubModalidad && descripSubModalidad.includes('SALDO APORTES')){
				if (datoCuota?.cuentas.length > 0) {
					let saldo;
					let tieneCuentaSolucionVivienda = false;
					datoCuota?.cuentas.forEach((cuenta: any) => {
						if (cuenta?.tipoCuenta === "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA") {
							saldo = cuenta?.saldoDisponible;
							tieneCuentaSolucionVivienda =  true;
						}
					})
	
					if(!tieneCuentaSolucionVivienda){
						NotificationService.genericAlwarning(
							'Alerta',
							`Tipo de cuenta no aplica para este trámite`,
							'warning'
						);
						return;
					}
	
					if (saldo && (saldo === 0 || saldo < 0)) {
						toast(
							'Saldo en ceros',
							{ type: toast.TYPE.ERROR },
						)
						return;
					}
				}
	
				const solicitudSaldoAportes = datoCuota?.solicitudes?.find((solicitud: any) => solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' && solicitud.modalidad === 'SALDO DE APORTES' && solicitud.estadoSolicitud === 'PENDIENTE')
				if(solicitudSaldoAportes){
					await NotificationService.genericAlert(
						"Advertencia",
						`El afiliado tiene una solicitud con actividad "${solicitudSaldoAportes.actividad}" en estado "${solicitudSaldoAportes.estadoSolicitud}".`,
						"warning"
					);
					return;
				}
	
				// Para radicar solo aportes debe cumplir con alguna solicitud procesada
	
				const tienePreRequisito = datoCuota?.solicitudes?.some(
					(solicitud: any) =>
						(
							solicitud.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' &&
							solicitud.modalidad === 'ACTO ADMINISTRATIVO NO ACREDITACIÓN' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA CON CESANTÍAS' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)' &&
							solicitud.modalidad === 'CON SEGUNDO PAGO SIMULTANEO' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'SUBSIDIO (SEGUNDO PAGO)' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'PAGOS EXTRAORDINARIOS' &&
							solicitud.modalidad === 'LIQUIDACION SOCIEDAD CONYUGAL' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'PRIMER PAGO SECUESTRADO' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) ||
						(
							solicitud.tipoSolicitud === 'SEGUNDO PAGO SECUESTRADO' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						)
				);
				
				if (!tienePreRequisito) {
					await NotificationService.genericAlert(
						"Advertencia",
						'Afiliado no cuenta con el pre requisito para este tipo de solicitud',
						"warning"
					);
					return;
				}
	
	
			
			}

			if(descripSubModalidad && descripSubModalidad.includes('SALDO AVAV')){
				if (datoCuota?.cuentas.length > 0) {
					let saldo;
					let tieneCuentaAvav = false;
					datoCuota?.cuentas.forEach((cuenta: any) => {
						if (cuenta?.tipoCuenta === "AHORRO VOLUNTARIO AFILIADO VETERANO") {
							saldo = cuenta?.saldoDisponible;
							tieneCuentaAvav =  true;
						}
					})
	
					if(!tieneCuentaAvav){
						NotificationService.genericAlwarning(
							'Alerta',
							`Tipo de cuenta no aplica para este trámite`,
							'warning'
						);
						return;
					}
	
					if (saldo && (saldo === 0 || saldo < 0)) {
						toast(
							'Saldo en ceros',
							{ type: toast.TYPE.ERROR },
						)
						return;
					}
				}

				const solicitudAhorroAvav = datoCuota?.solicitudes?.find((solicitud: any) => solicitud.tipoSolicitud === 'DEVOLUCIÓN AHORROS AVAV' && solicitud.modalidad === 'DESAFILIACIÓN AVAV' && solicitud.estadoSolicitud === 'PROCESADA')
				if(!solicitudAhorroAvav){
					await NotificationService.genericAlert(
						"Advertencia",
						`Afiliado no cuenta con el pre requisito para este tipo de solicitud`,
						"warning"
					);
					return;
				}
			}

			if(descripSubModalidad && descripSubModalidad.includes('SALDOS CUENTA AVAC')){
				if (datoCuota?.cuentas.length > 0) {
					let saldo;
					let tieneCuentaAvav = false;
					datoCuota?.cuentas.forEach((cuenta: any) => {
						if (cuenta?.tipoCuenta === "AHORRO VOLUNTARIO AFILIADO CESANTÍAS") {
							saldo = cuenta?.saldoDisponible;
							tieneCuentaAvav =  true;
						}
					})
	
					if(!tieneCuentaAvav){
						NotificationService.genericAlwarning(
							'Alerta',
							`Tipo de cuenta no aplica para este trámite`,
							'warning'
						);
						return;
					}
	
					if (saldo && (saldo === 0 || saldo < 0)) {
						toast(
							'Saldo en ceros',
							{ type: toast.TYPE.ERROR },
						)
						return;
					}
				}

				const solicitudAhorroDefinitivoAvac = datoCuota?.solicitudes?.find((solicitud: any) => solicitud.modalidad === 'DEVOLUCIÓN DEFINITIVA DE AHORROS AVAC' && solicitud.estadoSolicitud === 'PROCESADA')
				if(!solicitudAhorroDefinitivoAvac){
					await NotificationService.genericAlert(
						"Advertencia",
						`Afiliado no cuenta con el pre requisito para este tipo de solicitud`,
						"warning"
					);
					return;
				}
			}

			if(descripSubModalidad && descripSubModalidad.includes('RETIRO DE LA INSTITUCIÓN / MUERTE DEL AFILIADO')){
				if (datoCuota?.cuentas.length > 0) {
					let saldo;
					let tieneCuentaAvav = false;
					datoCuota?.cuentas.forEach((cuenta: any) => {
						if (cuenta?.tipoCuenta === "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS") {
							saldo = cuenta?.saldoDisponible;
							tieneCuentaAvav =  true;
						}
					})
	
					if(!tieneCuentaAvav){
						NotificationService.genericAlwarning(
							'Alerta',
							`Tipo de cuenta no aplica para este trámite`,
							'warning'
						);
						return;
					}
	
					if (saldo && (saldo === 0 || saldo < 0)) {
						toast(
							'Saldo en ceros',
							{ type: toast.TYPE.ERROR },
						)
						return;
					}
				}

				const cumpleRequisito = datoCuota?.solicitudes?.some(
					(solicitud: any) =>
						(
							solicitud.tipoSolicitud === 'DESAFILIACION' &&
							solicitud.modalidad === 'MUERTE DEL AFILIADO' &&
							solicitud.estadoSolicitud === 'PROCESADA'
						) 					
				);

				if (!cumpleRequisito) {
					await NotificationService.genericAlert(
						"Advertencia",
						'Afiliado no cuenta con el pre requisito para este tipo de solicitud',
						"warning"
					);
					return;
				}
			}

				// Alertas
				const solicitudPagoExtraOrdinario = datoCuota?.solicitudes?.find((solicitud: any) => solicitud.tipoSolicitud === 'PAGOS EXTRAORDINARIOS'  && solicitud.modalidad === 'MUERTE DEL AFILIADO' && solicitud.estadoSolicitud === 'PROCESADA')
				if(solicitudPagoExtraOrdinario){
					await NotificationService.genericAlert(
						"Advertencia",
						`El afiliado presenta un trámite "${solicitudPagoExtraOrdinario.tipoSolicitud}" con modalidad "${solicitudPagoExtraOrdinario.modalidad}  "en estado "${solicitudPagoExtraOrdinario.estadoSolicitud}".`,
						"warning"
					);
				}
	
				const solicitudGiroRecursos = datoCuota?.solicitudes?.find((solicitud: any) => solicitud.modalidad === 'GIRO DE RECURSOS COMPRA DE VIVIENDA SEGUNDO PAGO' && solicitud.estadoSolicitud === 'PROCESADA')
				if(solicitudGiroRecursos){
					await NotificationService.genericAlert(
						"Advertencia",
						`El afiliado presenta un trámite "${solicitudGiroRecursos.tipoSolicitud}" con modalidad "${solicitudGiroRecursos.modalidad}  "en estado "${solicitudGiroRecursos.estadoSolicitud}".`,
						"warning"
					);
				}
	
				const solicitudFondoSolidaridadHeroes = datoCuota?.solicitudes?.find((solicitud: any) => solicitud.tipoSolicitud === 'POSTULACIÓN FONDO SOLIDARIDAD'  && solicitud.modalidad === 'HÉROES' && solicitud.estadoSolicitud === 'PROCESADA')
				if(solicitudFondoSolidaridadHeroes){
					await NotificationService.genericAlert(
						"Advertencia",
						`El afiliado presenta un trámite "${solicitudFondoSolidaridadHeroes.tipoSolicitud}" con modalidad "${solicitudFondoSolidaridadHeroes.modalidad}  "en estado "${solicitudFondoSolidaridadHeroes.estadoSolicitud}".`,
						"warning"
					);
				}
		}

		// Validaciones secuestrados
		if (tipoDSolicitud === subsidioSecuestrados
			&& datoCuota?.solicitudes && datoCuota?.solicitudes.length > 0) {

			if (descripSubModalidad === 'PRIMER PAGO' &&
				datoCuota?.solicitudes.some((solicitud: any) =>
					(solicitud?.tipoSolicitud.includes('SUBSIDIO SECUESTRADOS') && solicitud?.modalidad.includes('PRIMER PAGO'))
					|| solicitud?.tipoSolicitud.includes('(PRIMER PAGO)'))) {
				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta solicitud con primer pago de subsidio`,
					'warning'
				);
				return;
			}

			if (descripSubModalidad === 'SEGUNDO PAGO' &&
				datoCuota?.solicitudes.some((solicitud: any) =>
					(solicitud?.tipoSolicitud.includes('SUBSIDIO') && solicitud?.modalidad === 'PRIMER PAGO' && solicitud?.estadoSolicitud !== 'PROCESADA')
					|| (solicitud?.tipoSolicitud.includes('(PRIMER PAGO)') && solicitud?.estadoSolicitud !== 'PROCESADA'))) {
				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado no presenta una solicitud con primer pago de subsidio procesado`,
					'warning'
				);
				return;
			}

			if (descripSubModalidad === 'SEGUNDO PAGO' &&
				datoCuota?.solicitudes.some((solicitud: any) =>
					(solicitud?.tipoSolicitud.includes('SUBSIDIO') && solicitud?.modalidad === 'SEGUNDO PAGO')
					|| (solicitud?.tipoSolicitud.includes('SUBSIDIO (SEGUNDO PAGO)') && solicitud?.estadoSolicitud !== 'RADICADA')
					|| (solicitud?.tipoSolicitud.includes('(SEGUNDO PAGO)')))) {
				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta solicitud con segundo pago de subsidio`,
					'warning'
				);
				return;
			}
		}

		// let acuerdoSolicitud = '';
		// if (filtroTipoSolicitud.length > 0) {
		// 	acuerdoSolicitud = obtenerAcuerdo(filtroTipoSolicitud[0].fechaInicioSolicitud);
		// }


		const numeroCuotas = Number(datoCuota.numeroCuotas)

		// VIVIENDA 8
		if (tipoMidalidadValidar === vivienda8) {

			const cuentas = Array.isArray(datoCuota?.cuentas)
				? datoCuota.cuentas
				: [];

			const tieneSaldoDisponibleEmbargosNegativo = cuentas.some(
				(cuenta: any) => {
					const saldoDisponibleEmbargos = Number(
						cuenta?.saldoDisponibleEmbargos ?? 0,
					);

					const totalBloqueos = Number(cuenta?.totalBloqueos ?? 0);
					const totalEmbargos = Number(cuenta?.totalEmbargos ?? 0);

					const tieneBloqueoOEmbargo =
						totalBloqueos > 0 || totalEmbargos > 0;

					return (
						tieneBloqueoOEmbargo &&
						saldoDisponibleEmbargos < 0
					);
				},
			);

			if (tieneSaldoDisponibleEmbargosNegativo) {
				await NotificationService.genericAlwarning(
					'Alerta',
					'El afiliado cuenta con el 100% del saldo disponible bloqueado para esta solicitud',
					'warning',
				);

				return;
			}

			const SUBMODALIDAD_MODELO_ANTICIPADO_APORTES_CONYUGE = [
				'cdc1c06c-c71e-4e97-a3a9-85ec972f28ca',
				'55a045aa-313b-40a3-afbe-85e88f34f70a',
				'3655730a-f48c-4938-b693-0c45680bd8e3',
			];

			if (
				numeroCuotas < 96 &&
				!SUBMODALIDAD_MODELO_ANTICIPADO_APORTES_CONYUGE.some(
					(item: any) => item === selectedTipoSubModalidad,
				)
			) {
				toast(
					'Error: El número de cuotas aportadas no corresponde para este trámite',
					{ type: toast.TYPE.ERROR },
				);
				return;
			}


			if (numeroCuotas < 96 && !SUBMODALIDAD_MODELO_ANTICIPADO_APORTES_CONYUGE.some((item: any) => item === selectedTipoSubModalidad)) {
				toast("Error: El número de cuotas aportadas no corresponde para este trámite",
					{ type: toast.TYPE.ERROR })
				return;
			}


			// Vivienda Usada V8
			if (numeroCuotas > 162 && selectedTipoSubModalidad === submodalidadV8ViviendaUsada) {
				toast("Error: El número de cuotas aportadas no corresponde para este trámite",
					{ type: toast.TYPE.ERROR })
				return;
			}

			if (SUBMODALIDAD_MODELO_ANTICIPADO_APORTES_CONYUGE.some((item: any) => item === selectedTipoSubModalidad)) {

				if (numeroCuotas <= 96) {

					if (!datoCuota?.identificacionConyuge) {
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado no aplica para radicar Vivienda 8 en modelo anticipado de aportes cónyuge`,
							'warning'
						);
						return
					}

					if (datoCuota?.identificacionConyuge) {
						try {
							const response = await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(
								datoCuota?.idTipoIdentificacionConyuge,
								datoCuota?.identificacionConyuge
							);

							if (response?.data === null) {
								NotificationService.genericAlwarning(
									'Alerta',
									`Afiliado no aplica para radicar Vivienda 8 en modelo anticipado de aportes cónyuge`,
									'warning'
								);
								return
							}

							if (response?.data !== null && response?.data?.numeroCuotas <= 96) {
								NotificationService.genericAlwarning(
									'Alerta',
									`Afiliado no aplica para radicar Vivienda 8 en modelo aticipado de aportes cónyuge`,
									'warning'
								);
								return
							}
						} catch (e) {
							toast('Error al consultar cónyuge',
								{ type: toast.TYPE.ERROR })
						}
					}
				}
				else if (numeroCuotas >= 96) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado no aplica para radicar Vivienda 8 en modelo anticipado de aportes cónyuge`,
						'warning'
					);
					return
				}
			}
		}


		// VIVIENDA 14
		if (
			tipoDSolicitud === vivienda14 &&
			normalizarTexto(descripSubModalidad) === 'PRIMER PAGO'
		) {
			const cuentas = Array.isArray(datoCuota?.cuentas)
				? datoCuota.cuentas
				: [];

			let totalBloqueosEmbargos = 0;
			const razonesEmbargo = new Set<string>();

			cuentas.forEach((cuenta: any) => {
				const conceptos = Array.isArray(cuenta?.conceptosSaldosBloqueos)
					? cuenta.conceptosSaldosBloqueos
					: [];

				conceptos.forEach((concepto: any) => {
					const razonOriginal = String(concepto?.razon ?? '').trim();
					const esEmbargo = normalizarTexto(razonOriginal).includes(
						'EMBARGO',
					);

					if (!esEmbargo) {
						return;
					}

					totalBloqueosEmbargos += convertirNumero(
						concepto?.valorAcumulado ??
						concepto?.valorEmbargo ??
						concepto?.bloqueo ??
						0,
					);

					if (razonOriginal) {
						razonesEmbargo.add(razonOriginal);
					}
				});
			});

			const razonEmbargo =
				Array.from(razonesEmbargo).join(', ') || 'embargo';

			if (totalBloqueosEmbargos > 0) {
				await NotificationService.messageAlert(
					'Alerta',
					`Presenta un embargo por ${razonEmbargo} de ${UtilService.formatCurrency(
						totalBloqueosEmbargos,
					)}`,
					'warning',
				);
			}
		}

		// if (tipoDSolicitud === vivienda14 && descripSubModalidad === 'PRIMER PAGO') {
		// 	const solicitudNoPermitidaPrimerPago = datoCuota?.solicitudes.find((solicitud: any) =>
		// 		(solicitud.estadoSolicitud !== 'INCONSISTENTE' &&
		// 			solicitud.estadoSolicitud !== 'ANULADA') &&
		// 		(solicitud.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)' ||
		// 			(solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'PRIMER PAGO')));

		// 	if (solicitudNoPermitidaPrimerPago) {
		// 		setDesHabilitarBotonAceptar(true);
		// 		NotificationService.genericAlwarning(
		// 			'Alerta',
		// 			`El afiliado presenta solicitud de Solución de vivienda 14 en estado: ${solicitudNoPermitidaPrimerPago.estadoSolicitud}`,
		// 			'warning'
		// 		);
		// 		return;
		// 	}
		// }

		if (
			tipoDSolicitud === vivienda14 &&
			normalizarTexto(descripSubModalidad) === 'PRIMER PAGO'
		) {
			const estadosBloqueantesPrimerPago = [
				'PROCESADA',
				'PENDIENTE',
				'EN VERIFICACION',
			];

			const solicitudNoPermitidaPrimerPago = (
				Array.isArray(datoCuota?.solicitudes)
					? datoCuota.solicitudes
					: []
			).find((solicitud: any) => {
				const tipoSolicitudHistorica = normalizarTexto(
					solicitud?.tipoSolicitud,
				);

				const modalidadHistorica = normalizarTexto(
					solicitud?.modalidad,
				);

				const estadoHistorico = normalizarTexto(
					solicitud?.estadoSolicitud,
				);

				const esPrimerPagoHistorico =
					tipoSolicitudHistorica ===
					'SOLUCION DE VIVIENDA (PRIMER PAGO)' ||
					(
						tipoSolicitudHistorica === 'SOLICITUD DE SUBSIDIO' &&
						modalidadHistorica === 'PRIMER PAGO'
					);

				return (
					esPrimerPagoHistorico &&
					estadosBloqueantesPrimerPago.includes(estadoHistorico)
				);
			});

			if (solicitudNoPermitidaPrimerPago) {
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);

				await NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta una solicitud de primer pago en estado: ${solicitudNoPermitidaPrimerPago.estadoSolicitud}`,
					'warning',
				);

				return;
			}
		}

		if (
			tipoDSolicitud === vivienda14 &&
			normalizarTexto(descripSubModalidad) === 'SEGUNDO PAGO'
		) {
			const solicitudesPrimerPago = datoCuota?.solicitudes.filter((solicitud: any) =>
				solicitud.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)' ||
				(solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'PRIMER PAGO'));

			if (!solicitudesPrimerPago) {
				setDesHabilitarBotonAceptar(true);
				NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado no cuenta con una solicitud de primer pago procesado`,
					'warning'
				);
				return;
			}

			if (solicitudesPrimerPago && solicitudesPrimerPago.length > 0) {

				const encontradaProcesada = solicitudesPrimerPago.some((item: any) => item.estadoSolicitud === 'PROCESADA');

				if (!encontradaProcesada) {
					setDesHabilitarBotonAceptar(true);
					NotificationService.genericAlwarning(
						'Alerta',
						`El afiliado no cuenta con una solicitud de primer pago procesado`,
						'warning'
					);
					return;
				}
			}



			const estadosBloqueantesSegundoPago = [
				'PROCESADA',
				'PENDIENTE',
				'EN VERIFICACION',
			];

			const solicitudSegundoPagoBloqueante = (
				Array.isArray(datoCuota?.solicitudes)
					? datoCuota.solicitudes
					: []
			).find((solicitud: any) => {
				const tipoSolicitudHistorica = normalizarTexto(
					solicitud?.tipoSolicitud,
				);

				const modalidadHistorica = normalizarTexto(
					solicitud?.modalidad,
				);

				const estadoHistorico = normalizarTexto(
					solicitud?.estadoSolicitud,
				);

				// Reconoce el segundo pago sin depender de una única modalidad o nombre.
				const esSegundoPagoHistorico =
					modalidadHistorica === 'SEGUNDO PAGO' ||
					tipoSolicitudHistorica.includes('SEGUNDO PAGO');

				return (
					esSegundoPagoHistorico &&
					estadosBloqueantesSegundoPago.includes(estadoHistorico)
				);
			});

			if (solicitudSegundoPagoBloqueante) {
				setDesHabilitarBotonAceptar(true);
				setIsLoading(false);

				await NotificationService.genericAlwarning(
					'Alerta',
					`El afiliado presenta una solicitud de segundo pago en estado: ${solicitudSegundoPagoBloqueante.estadoSolicitud}`,
					'warning',
				);

				return;
			}
		}

		if (tipoDSolicitud === vivienda14 && descripSubModalidad === 'SIMULTÁNEO') {

			const solicitudesPrimerYSegundoPago = datoCuota?.solicitudes.filter((solicitud: any) =>
				solicitud.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)' ||
				(solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'PRIMER PAGO') ||
				solicitud.tipoSolicitud === 'SUBSIDIO (SEGUNDO PAGO)');

			if (solicitudesPrimerYSegundoPago && solicitudesPrimerYSegundoPago.length > 0) {
				const noPermitirSimultaneo = solicitudesPrimerYSegundoPago.some((item: any) =>
					item.estadoSolicitud !== 'INCONSISTENTE' && item.estadoSolicitud !== 'ANULADA');

				if (noPermitirSimultaneo) {
					setDesHabilitarBotonAceptar(true);
					NotificationService.genericAlwarning(
						'Alerta',
						`El afiliado presenta solicitud de Solución de vivienda 14 en trámite`,
						'warning'
					);
					return;
				}
			}
		}


		if (tpoDeSolicitud && tipoMidalidadValidar === vivienda14) {

			if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2020-09-24' && filtroTipoSolicitud[0].modalidad === "MASVI COMPRA DE VIVIENDA USADA") {
				const { fechaFinSolicitud }: { fechaFinSolicitud?: any } = filtroTipoSolicitud[0];
				const fechaFin = new Date(fechaFinSolicitud)
				if (fechaFin) {
					const fechaActual: Date = new Date();
					const diferenciaEnMilisegundos: number = fechaActual.getTime() - fechaFin.getTime();
					const diferenciaEnMeses: number = diferenciaEnMilisegundos / (1000 * 60 * 60 * 24 * 30.44);
					if (diferenciaEnMeses <= 6) {
						if (!flujos37_102.tieneFlujo37) {
							NotificationService.genericAlwarning(
								'Alerta',
								`Afiliado pendiente por acreditación según acuerdo 02 de 2020`,
								'warning')

						}
					} else {
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado incumplió las obligaciones del modelo anticipado para solución de vivienda, según acuerdo 02 de 2020`,
							'warning')
					}
					return
				}


			}

			if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2017-12-20' && filtroTipoSolicitud[0].fechaInicioSolicitud <= '2020-09-23') {
				if (!flujos37_102.tieneFlujo37) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado incumplió las obligaciones del modelo anticipado para solución de vivienda, según acuerdo 05 de 2017`,
						'warning')
					return
				}
			} else if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2016-04-29' && filtroTipoSolicitud[0].fechaInicioSolicitud <= '2017-12-19') {
				if (!flujos37_102.tieneFlujo37) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado incumplió las obligaciones del modelo anticipado para solución de vivienda, según acuerdo 01 de 2016`,
						'warning')
					return
				}
			} else if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2015-06-05' && filtroTipoSolicitud[0].fechaInicioSolicitud <= '2016-04-28') {
				if (!flujos37_102.tieneFlujo37) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado incumplió las obligaciones del modelo anticipado para solución de vivienda, según acuerdo 01 de 2015`,
						'warning')
					return
				}
			} else if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2013-06-07' && filtroTipoSolicitud[0].fechaInicioSolicitud <= '2015-06-04') {
				if (!flujos37_102.tieneFlujo37) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado pendiente por acreditación según acuerdo 02 de 2013`,
						'warning')
					return
				}
			} else if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2011-02-12' && filtroTipoSolicitud[0].fechaInicioSolicitud <= '2013-06-06') {
				if (!flujos37_102.tieneFlujo37) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado pendiente por acreditación según acuerdo 01 de 2011`,
						'warning')
					return
				}
			}
			else if (filtroTipoSolicitud.length > 0 && filtroTipoSolicitud[0].fechaInicioSolicitud >= '2009-10-21' && filtroTipoSolicitud[0].fechaInicioSolicitud <= '2011-02-11') {
				if (!flujos37_102.tieneFlujo37) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Afiliado incumplió las obligaciones del modelo anticipado para solución de vivienda, según acuerdo 04 de 2009`,
						'warning')
					return
				}
			}
		}

		if (tipoMidalidadValidar === vivienda14) {

			const fechaAlta: any = new Date(datoCuota?.fechaAlta);
			const fechaLimite: any = new Date("2004-11-02");


			const differenceInMilliseconds = fechaLimite - fechaAlta;


			const millisecondsInYear = 1000 * 60 * 60 * 24 * 365.25;
			const differenceInYears = differenceInMilliseconds / millisecondsInYear;


			if (datoCuota.numeroCuotas < 168) {
				if (datoCuota?.idFuerza === 213 && datoCuota?.idUnidadEjecutora === 1 && differenceInYears < 15) {
					toast("Afiliado no cumple con tiempo de servicio", { type: toast.TYPE.ERROR });
					return;
				}
			}

			if (datoCuota?.identificacionConyuge) {
				try {
					const response: any = await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(
						datoCuota?.idTipoIdentificacionConyuge,
						datoCuota?.identificacionConyuge
					);

					if (response?.data !== null && response?.data?.solicitudes &&
						(response?.data?.solicitudes.some((solicitud: any) => solicitud.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)') ||
							response?.data?.solicitudes.some((solicitud: any) => solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && (solicitud.modalidad === 'PRIMER PAGO' || solicitud.modalidad === 'SIMULTÁNEO')) ||
							response?.data?.solicitudes.some((solicitud: any) => solicitud.tipoSolicitud === 'SUBSIDIO (SEGUNDO PAGO)'))) {
						NotificationService.genericAlwarning(
							'Alerta',
							`Trámite no procede, presenta bloqueo por cruce cónyuge`,
							'warning'
						);
						return
					}
				} catch (e) {
					toast('Error al consultar cónyuge en validacionse cruce cónyuge',
						{ type: toast.TYPE.ERROR })
				}
			}
		}

		if (tipoMidalidadValidar === vivienda14 && tpoDeSolicitud === true && datoCuota.estadoCliente === "BLOQUEADO") {
			if (numeroCuotas === 168) {
				toast("Error: Afiliado no acreditó. Trámite no procede",
					{ type: toast.TYPE.ERROR })
				return
			}

		}

		// VALIDACIONES DEVOLUCIÓN DE AHORROS AVAC
		if (descripSubModalidad && descripSubModalidad?.includes('AVAC')) {

			if (datoCuota?.cuentas.length > 0) {
				const cuentaAVAC = datoCuota?.cuentas?.find((cuenta: any) => cuenta?.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS');
				if (!cuentaAVAC) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Tipo de cuenta no aplica para la solicitud`,
						'warning'
					);
					return;
				}
			}

			if (descripSubModalidad === 'DEVOLUCIÓN PARCIAL DE AHORROS AVAC') {
				if (datoCuota?.solicitudes.length > 0) {
					const solicitudDefinitivaAVAC = datoCuota?.solicitudes?.find((solicitud: any) => solicitud?.tipoSolicitud === 'DEVOLUCIÓN DE AHORROS AVAC - AVAV' && solicitud?.modalidad === 'DEVOLUCIÓN DEFINITIVA DE AHORROS AVAC' && solicitud?.estadoSolicitud === 'PENDIENTE');

					if (solicitudDefinitivaAVAC) {
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado ya cuenta con una solicitud de Devolución definitiva de ahorros AVAC`,
							'warning'
						);
						return;
					}
				}

				if (datoCuota?.solicitudes.length > 0) {

					const autorizacionDescuento = datoCuota?.solicitudes.find((solicitud: any) => solicitud?.tipoSolicitud === 'AUTORIZACION DESCUENTO AVAC' && solicitud?.estadoSolicitud === 'PROCESADA');
					if (!autorizacionDescuento) {
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado no cuenta con la solicitud Autorización Descuento AVAC`,
							'warning'
						);
						return;
					}

				}
			}

			if (datoCuota?.tipoVinculacion !== "AFILIADO ADMINISTRACION CESANTIAS") {
				NotificationService.genericAlwarning(
					'Alerta',
					`Tipo de vinculación no aplica para la solicitud`,
					'warning'
				);
				return;
			}

			if (descripSubModalidad === 'DEVOLUCIÓN DEFINITIVA DE AHORROS AVAC') {
				NotificationService.genericAlwarning(
					'Alerta',
					`Recuerde que, al radicar esta solicitud, no seguirá operando su descuento`,
					'warning'
				);
			}

		}

		// VALIDACIONES DEVOLUCIÓN DE AHORROS AVAV

		if (descripSubModalidad && descripSubModalidad?.includes('AVAV')) {
			if (datoCuota?.cuentas.length > 0) {
				const cuentaAVAV = datoCuota?.cuentas?.find((cuenta: any) => cuenta?.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO');
				if (!cuentaAVAV) {
					NotificationService.genericAlwarning(
						'Alerta',
						`Tipo de cuenta no aplica para la solicitud`,
						'warning'
					);
					return;
				}
			}

			if (descripSubModalidad === 'DEVOLUCIÓN PARCIAL DE AHORROS AVAV') {

				if (datoCuota?.solicitudes.length > 0) {
					const solicitudDefinitivaAVAV = datoCuota?.solicitudes?.find((solicitud: any) => solicitud?.tipoSolicitud === 'DEVOLUCIÓN DE AHORROS AVAC - AVAV' && solicitud?.modalidad === 'DEVOLUCIÓN DEFINITIVA DE AHORROS AVAV' && solicitud?.estadoSolicitud === 'PENDIENTE');

					if (solicitudDefinitivaAVAV) {
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado ya cuenta con una solicitud de Devolución definitiva de ahorros AVAV`,
							'warning'
						);
						return;
					}
				}

				if (datoCuota?.solicitudes.length > 0) {

					const afiliacionVeteranos = datoCuota?.solicitudes.find((solicitud: any) => solicitud?.modalidad === 'AFILIADO VETERANO' && solicitud?.estadoSolicitud === 'PROCESADA');
					if (!afiliacionVeteranos) {
						NotificationService.genericAlwarning(
							'Alerta',
							`Afiliado no cuenta con la solicitud Afiliación Veteranos`,
							'warning'
						);
						return;
					}

				}
			}

			if (descripSubModalidad === 'DEVOLUCIÓN DEFINITIVA DE AHORROS AVAV') {
				NotificationService.genericAlwarning(
					'Alerta',
					`Recuerde que, al radicar esta solicitud, no seguirá operando su descuento`,
					'warning'
				);
			}
		}


		// datoCuota?.solicitudes


		setLoading(true);
		const identificationType = Number(watch('tipoDocumento'));
		const identification = watch('numeroDocumento');
		const tipoSolicitudId = getValues('typerequest');
		const modalidadId = getValues('typemodality');

		obtenerCedulaDeSolicitud(identification)

		const tipoSolicitudObject = catalogState.typeRequest.find(x =>
			x.idTipoSolicitud === tipoSolicitudId
		);

		const modalidadObject = catalogState.typeModality.find(x =>
			x.idTipoModalidad === modalidadId
		);
		const documentType = obtenerCodigoDocumentoTusDatos(watch('tipoDocumento'));

		if (!identification || !documentType) {
			setLoading(false);
			await NotificationService.genericAlwarning(
				'Alerta',
				'No fue posible validar TusDatos porque falta identificación o tipo de documento.',
				'warning'
			);
			return;
		}

		const mensajesAlertaRiesgoTusDatos: string[] = [];
		const mensajesAlertaActualizacionDatos: string[] = [];
		const mensajesAlertaConyuge: string[] = [];

		const agregarMensajeUnico = (lista: string[], mensaje: string) => {
			if (!lista.includes(mensaje)) {
				lista.push(mensaje);
			}
		};

		const esperar = (ms: number): Promise<void> =>
			new Promise(resolve => setTimeout(resolve, ms));

		const mensajeAfiliadoRiesgoMedio =
			'Afiliado con riesgo medio, por favor validar pertinencia.';

		const mensajeAfiliadoRiesgoAlto =
			'Afiliado riesgo alto. Se requiere revisar la operación. Por favor comunicarse con la Unidad de Cumplimiento.';

		const mensajeListasRestrictivas = mensajeAfiliadoRiesgoAlto;

		const mostrarAlertaSecuencial = (mensaje: string): Promise<void> =>
			Promise.resolve(
				NotificationService.genericAlwarning(
					'Alerta',
					mensaje,
					'warning'
				)
			).then(() => undefined);

		const mostrarAlertasSecuenciales = (mensajes: string[]): Promise<void> =>
			mensajes.reduce<Promise<void>>(
				(promesaAnterior, mensaje) =>
					promesaAnterior
						.then(() => mostrarAlertaSecuencial(mensaje))
						.then(() => esperar(300))
						.then(() => undefined),
				Promise.resolve()
			);

		const normalizarNivelRiesgoTusDatos = (nivel: any): string =>
			nivel
				?.toString()
				.trim()
				.toLowerCase()
				.normalize('NFD')
				.replace(/[\u0300-\u036f]/g, '') || '';


		const [
			responseFenix,
			responseClienteDatos,
			responseTusDatos,
		] = await Promise.all([
			// BIOMETRÍA SIGUE ACTIVA
			apiCre.postValidarBiometriaFenix(identification),

			// DOS SIGUE ACTIVO
			apiDos.getClientByIdentificationTypeIdentificationNumberRequest(
				Number(watch('tipoDocumento')),
				identification,
			),

			// TUS DATOS QUEDA APAGADO
			HABILITAR_TUS_DATOS
				? apiCre.validarTusDatosConPolling(
					identification,
					documentType,
				)
				: Promise.resolve(null),
		]);

		/**
 * VALIDACIÓN RIESGO TUSDATOS / LISTAS RESTRICTIVAS
 */

		if (HABILITAR_TUS_DATOS) {

			if (!responseTusDatos?.isSuccess) {
				setLoading(false);
				await NotificationService.genericAlwarning(
					'Alerta',
					responseTusDatos?.returnMessage ||
					'No fue posible validar la información en TusDatos.',
					'warning'
				);
				return;
			}


			const tusDatos =
				responseTusDatos?.data?.historyDataCustomer ??
				responseTusDatos?.data;

			if (!tusDatos) {
				setLoading(false);
				await NotificationService.genericAlwarning(
					'Alerta riesgo TusDatos',
					'No fue posible obtener la información de riesgo desde TusDatos.',
					'warning'
				);
				return;
			}

			const detectionRiskAfiliadoRaw = tusDatos?.detectionRisk;

			const detectionRiskAfiliadoNormalizado =
				normalizarNivelRiesgoTusDatos(detectionRiskAfiliadoRaw);

			if (detectionRiskAfiliadoNormalizado === 'alto') {
				setNivelAfiliado(1);
			}

			if (detectionRiskAfiliadoNormalizado === 'medio' || detectionRiskAfiliadoNormalizado === 'bajo') {
				setNivelAfiliado(0);
			}


			if (tusDatos?.isPEPS === true) {
				agregarMensajeUnico(
					mensajesAlertaRiesgoTusDatos,
					mensajeListasRestrictivas
				);
			}

			if (detectionRiskAfiliadoNormalizado === 'medio') {
				agregarMensajeUnico(
					mensajesAlertaRiesgoTusDatos,
					mensajeAfiliadoRiesgoMedio
				);
			}

			if (detectionRiskAfiliadoNormalizado === 'alto') {
				agregarMensajeUnico(
					mensajesAlertaRiesgoTusDatos,
					mensajeAfiliadoRiesgoAlto
				);
			}

		}


		/**
		 * VALIDACIÓN INFORMACIÓN DEL CÓNYUGE
		 */
		if (
			solicitudState.formulariosSolicitud?.formularios?.some(
				(formulario: string) => formulario === 'InformacionInmueble'
			)
		) {
			agregarMensajeUnico(
				mensajesAlertaConyuge,
				'Recuerde validar la información del cónyuge.'
			);
		}


		/**
		 * ALERTAS FINALES
		 * Se muestran, pero no bloquean la radicación.
		 */

		const alertasFinales = [
			...mensajesAlertaRiesgoTusDatos,
			...mensajesAlertaActualizacionDatos,
			...mensajesAlertaConyuge,
		].filter((mensaje: string) => mensaje);

		if (alertasFinales.length > 0) {
			await mostrarAlertasSecuenciales(alertasFinales);
		}

		const isValidFenix = responseFenix?.isSuccess &&
			responseFenix?.data?.newDataSet?.table1?.a008HUELLA === 1 &&
			responseFenix?.data?.newDataSet?.table1?.a008CEDULA === 1;

		if (isValidFenix) {
			if (HABILITAR_TUS_DATOS) {
				const responseVigia = await apiCre.PostVerificacionTerc(
					{
						pe_Origen: PE_ORIGEN,
						pe_Dato: identification.toString(),
						pe_TipoVerificacion: PE_TIPO_VERIFICACION,
						pe_Porcentaje: PE_PORCENTAJE
					}
				);

				const isValidVigia = responseVigia.data?.encontrado === 'N';

				if (!isValidVigia) {
					agregarMensajeUnico(
						mensajesAlertaRiesgoTusDatos,
						mensajeListasRestrictivas
					);
				}


				if (responseVigia?.returnMessage && responseVigia?.returnMessage === 'Error en el servicio de vigía por favor intentar más tarde.') {
					setLoading(false);
					toast(responseVigia?.returnMessage, {
						type: toast.TYPE.ERROR,
					});
					return;
				}

			}
			await apiDos.getClientByIdentificationTypeIdentificationNumberRequest(
				identificationType,
				identification,
			).then(async clientState => {

				if (clientState.data?.cuentas[0].embargos && clientState.data?.cuentas[0].embargos.length !== 0) {
					obtenerJuzgadoEmbargo()
				}

				if (clientState?.data === null) {
					toast('Afiliado no existe', { type: toast.TYPE.ERROR });
					setLoading(false);
				} else {

					const { saldoDisponible } = clientState.data;

					const cuentaTipo = clientState.data.cuentas[0].tipoCuenta;

					interface Cuenta {
						estadoCuenta: string;
						tipoCuenta: string;
					}

					const estadoCuenta: Cuenta[] | undefined = clientState.data.cuentas;
					const { estadoCliente, estadosCausaleslCliente } = clientState.data;

					let mensajeModal = '';

					if (estadoCuenta && estadoCuenta.length > 0) {
						let vigente = 'pd';

						for (let index = 0; index < estadoCuenta.length; index += 1) {

							const cuenta = estadoCuenta[index];

							if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'rpcVigente';
							} else if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 14' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'v14Vigente'
							} else if ((cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS' || cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO') && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE AHORROS AVAC - AVAV' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'vDevAvacAvacVigente'
							}
							else if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'DevSaldosVigente'
							}
							else if (cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'DevSaldosVigente'
							}
							else if (cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'DevSaldosVigente'
							}
							else if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'DevSaldosVigente'
							}
							else if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 8' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'v8Vigente'
							} else if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'ACREDITACION V8' && cuenta.estadoCuenta === 'VIGENTE') {
								if (estadoCliente !== 'VIGENTE') {
									mensajeModal = `Afiliado presenta Estado de Cliente ${estadoCliente}`;
								}
								vigente = 'acreV8Vigente'
							}
						}

						if (vigente !== 'rpcVigente' && vigente !== 'v14Vigente' && vigente !== 'v8Vigente' && vigente !== 'acreV8Vigente' && vigente !== 'vDevAvacAvacVigente' && vigente !== 'DevSaldosVigente') {
							for (let index = 0; index < estadoCuenta.length; index += 1) {
								const cuenta = estadoCuenta[index];

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente !== 'VIGENTE') {
									const causalesCliente = estadosCausaleslCliente
										.filter((causal: { nombreEstado: any; }) => causal.nombreEstado === estadoCliente)
										.map((causal: { descripcionCausal: any; }) => causal.descripcionCausal);

									const descripcionCausales = causalesCliente.length > 0 ? causalesCliente.join(', ') : 'sin causal(es)';

									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta} , Estado de Cliente ${estadoCliente} y Causal(es): ${descripcionCausales}`;
									}
								}

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}`;
									}
								}

								// if ((cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS' || cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO') && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE AHORROS AVAC - AVAV' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente !== 'VIGENTE') {
								// 	const causalesCliente = estadosCausaleslCliente
								// 		.filter((causal: { nombreEstado: any; }) => causal.nombreEstado === estadoCliente)
								// 		.map((causal: { descripcionCausal: any; }) => causal.descripcionCausal);

								// 	const descripcionCausales = causalesCliente.length > 0 ? causalesCliente.join(', ') : 'sin causal(es)';

								// 	if (cuenta.estadoCuenta === "CANCELADA") {
								// 		mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta} , Estado de Cliente ${estadoCliente} y Causal(es): ${descripcionCausales}`;
								// 	}
								// }

								if ((cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS' || cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO') && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE AHORROS AVAC - AVAV' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta cuenta ${cuenta.tipoCuenta} en Estado ${cuenta.estadoCuenta}`;
									}
								}
								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && modalidadObject?.descripcionTipoModalidad.includes('SOLO APORTES') && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta cuenta ${cuenta.tipoCuenta} en Estado ${cuenta.estadoCuenta}`;
									}
								}
								if (cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO VETERANO' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && modalidadObject?.descripcionTipoModalidad.includes('AVAV') && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta cuenta ${cuenta.tipoCuenta} en Estado ${cuenta.estadoCuenta}`;
									}
								}
								if (cuenta.tipoCuenta === 'AHORRO VOLUNTARIO AFILIADO CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && modalidadObject?.descripcionTipoModalidad.includes('AVAC') && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta cuenta ${cuenta.tipoCuenta} en Estado ${cuenta.estadoCuenta}`;
									}
								}
								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS' && tipoSolicitudObject?.descripcionTipoSolicitud === 'DEVOLUCIÓN DE SALDOS' && modalidadObject?.descripcionTipoModalidad.includes('RETIRO DE LA INSTITUCIÓN / MUERTE DEL AFILIADO') && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta cuenta ${cuenta.tipoCuenta} en Estado ${cuenta.estadoCuenta}`;
									}
								}
								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 14' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente !== 'VIGENTE') {
									const causalesCliente = estadosCausaleslCliente
										.filter((causal: { nombreEstado: any; }) => causal.nombreEstado === estadoCliente)
										.map((causal: { descripcionCausal: any; }) => causal.descripcionCausal);

									const descripcionCausales = causalesCliente.length > 0 ? causalesCliente.join(', ') : 'sin causal(es)';

									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}, Estado de Cliente ${estadoCliente} y Causal(es): ${descripcionCausales}`;
									}
								}

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 14' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}`;
									}
								}

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 8' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente !== 'VIGENTE') {
									const causalesCliente = estadosCausaleslCliente
										.filter((causal: { nombreEstado: any; }) => causal.nombreEstado === estadoCliente)
										.map((causal: { descripcionCausal: any; }) => causal.descripcionCausal);

									const descripcionCausales = causalesCliente.length > 0 ? causalesCliente.join(', ') : 'sin causal(es)';

									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}, Estado de Cliente ${estadoCliente} y Causal(es): ${descripcionCausales}`;
									}
								}

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 8' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}`;
									}
								}

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'ACREDITACION V8' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente !== 'VIGENTE') {
									const causalesCliente = estadosCausaleslCliente
										.filter((causal: { nombreEstado: any; }) => causal.nombreEstado === estadoCliente)
										.map((causal: { descripcionCausal: any; }) => causal.descripcionCausal);

									const descripcionCausales = causalesCliente.length > 0 ? causalesCliente.join(', ') : 'sin causal(es)';

									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}, Estado de Cliente ${estadoCliente} y Causal(es): ${descripcionCausales}`;
									}
								}

								if (cuenta.tipoCuenta === 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA' && tipoSolicitudObject?.descripcionTipoSolicitud === 'ACREDITACION V8' && cuenta.estadoCuenta !== 'VIGENTE' && estadoCliente === 'VIGENTE') {
									if (cuenta.estadoCuenta === "CANCELADA") {
										mensajeModal = `Afiliado presenta Estado de Cuenta ${cuenta.estadoCuenta}`;
									}
								}
							}
						}

					} else {
						mensajeModal = `No hay cuentas disponibles`;
					}

					if (mensajeModal !== '' && mensajeModal !== null) {
						await NotificationService.messageAlert(mensajeModal);
					}

					let SaldoV14V8 = 0;

					clientState.data.cuentas.forEach((item: any, index: number) => {
						if (item.tipoCuenta === "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA") {
							SaldoV14V8 += item.saldoDisponible;
						}
					});

					if (retiroCesantias === tipoMidalidadValidar) {
						let sumaSaldoPRyVi = 0;

						for (let i = 0; i < datoCuota.cuentas.length; i += 1) {
							const cuenta = datoCuota.cuentas[i];

							if (cuenta.estadoCuenta === "PRECANCELADA" || cuenta.estadoCuenta === "VIGENTE") {
								sumaSaldoPRyVi += Number(cuenta.saldoDisponible);
							}
						}

						if (sumaSaldoPRyVi <= 0) {
							toast('No tiene saldos para este tipo de solicitud', { type: toast.TYPE.ERROR });
							setLoading(false);
							return;
						}
					}

					if (SaldoV14V8 <= 0 && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 8') {
						setLoading(false);
						toast(
							'Error: No tiene saldos suficientes para esta solicitud',
							{ type: toast.TYPE.ERROR },
						);
						return;
					}

					if (SaldoV14V8 <= 0 && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 14') {
						setLoading(false);
						toast(
							'Error: No tiene saldos suficientes para esta solicitud',
							{ type: toast.TYPE.ERROR },
						);
						return;
					}

					if (clientState.data.idTipoActor === 2 && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 14' && SaldoV14V8 <= 0) {
						setLoading(false);
						toast(
							'No aplica para Afiliados de Administración de Cesantías.',
							{ type: toast.TYPE.ERROR },
						)
						return
					}

					if (clientState.data.idTipoActor === 2 && tipoSolicitudObject?.descripcionTipoSolicitud === 'VIVIENDA 8' && SaldoV14V8 <= 0) {
						setLoading(false);
						toast(
							'No aplica para Afiliados de Administración de Cesantías.',
							{ type: toast.TYPE.ERROR },
						)
						return
					}

					// Verificar bloqueos y embargos en cuentas
					if ((tipoSolicitudId === vivienda8 || 
						 tipoSolicitudId === devolucionAvacAvav ||
						 tipoSolicitudId === tipoSolicitudDevolucionSaldoCuenta) && clientState.data?.cuentas?.length > 0) {

						let totalBloqueos = 0;
						let totalBloqueosEmbargos = 0;
						let razonEmbargo = '';

						clientState.data.cuentas.forEach((cuenta: any) => {
							if (cuenta?.conceptosSaldosBloqueos?.length > 0) {
								cuenta.conceptosSaldosBloqueos.forEach((concepto: any) => {
									const razon = String(concepto?.razon || '').toUpperCase();
									const valorAcumulado = Number(concepto?.valorAcumulado || 0);

									const esEmbargo = razon.includes('EMBARGO');

									if (esEmbargo) {
										razonEmbargo = concepto?.razon;
										totalBloqueosEmbargos += valorAcumulado;
									} else {
										totalBloqueos += valorAcumulado;
									}
								});
							}
						});

						if (totalBloqueos > 0) {
							await NotificationService.messageAlert(
								"Alerta",
								`Afiliado presenta bloqueos en el saldo disponible de la cuenta`,
								"warning"
							);
						}

						if (totalBloqueosEmbargos > 0) {
							await NotificationService.messageAlert(
								"Alerta",
								`Presenta un embargo por ${razonEmbargo} de ${UtilService.formatCurrency(totalBloqueosEmbargos)}`,
								"warning"
							);
						}
					}

					solicitud(clientState.data, cuentaTipo);
				}
			})
				.catch(() => {
					setLoading(false);
					toast('Error del sistema', {
						type: toast.TYPE.ERROR,
					});
				});
		} else if (responseFenix?.data == null) {
			setLoading(false);
			await NotificationService.genericAlert(
				'Error!',
				'Error interno del servidor',
			);
		}
		else {
			setLoading(false);
			await NotificationService.genericAlwarning(
				'Advertencia!',
				`No hay información en biometría, dirigirse al punto de registro Biométrico.`,
			);
		}
	};
	
	// const onItemGoBack = async () => {

	// 	const confirmAlert = await NotificationService.confirmAlert(
	// 		'¿Está seguro?',
	// 	);
	// 	if (confirmAlert) {
	// 		obtnerBoleano(true)
	// 		setIniciarPantalla('');
	// 		setResponseSolicitud({} as ResponseSeveranceModel);
	// 	}
	// };

	const onItemGoBack = async (pedirConfirmacion = true) => {
		if (pedirConfirmacion) {
			const confirmAlert = await NotificationService.confirmAlert(
				'¿Está seguro?',
			);

			if (!confirmAlert) {
				return;
			}
		}

		obtnerBoleano(true);
		setIniciarPantalla('');
		setResponseSolicitud({} as ResponseSeveranceModel);
	};

	const onItemBack = async () => {
		setIniciarPantalla('');
	};

	const aprobarSolicitudTarea = async () => {

		setLoading(true)
		const aprobarSolicitudTareaDto = {
			solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
			clienteId: responseSolicitud?.clienteId as number,
			soL_RECHAZADA: responseSolicitud?.soL_RECHAZADA,
		};

		try {
			const response = await api.PostAprobarSolicitudTarea(aprobarSolicitudTareaDto)
			if (response && response?.isSuccess) {

				toast('Se aprobó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
				obtnerBoleano(true)
				setIsLoading(false)
				onItemBack()
			} else {
				toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
				setIsLoading(false)
			}
		} catch (e) {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
			setIsLoading(false)
		}

	}

	if (iniciarPantalla !== '') {
		const formulario = () => {

			if (responseSolicitud?.nombreTarea?.includes('Cargue de documentos')) {

				return (
					<InformacionProcesoSolicitud
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						deshalitarBotonesInconsistencia={false}
						setDeshalitarBotonesInconsistencia={undefined}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						grupo={undefined}
					/>
				);
			}
			if (responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 1')) {

				return (
					<ProcesoSeguridadDocumental
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						grupo1={responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						datosCuentasBloqueos={undefined} userAsig={undefined} grupo={undefined} userIngreso='' />
				);
			}
			if (responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo 2')) {
				return (
					<ProcesoSeguridadDocumental
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						grupo1={responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados}
						datosCuentasBloqueos={undefined} grupo={undefined} userAsig={undefined} userIngreso='' />
				);
			}
			if (responseSolicitud?.nombreTarea?.includes('Acreditacion')) {
				return (
					<Acreditacion
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccion={tipoAccion}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						grupo1={responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1')}
					/>
				);
			}
			if (responseSolicitud?.nombreTarea?.includes('Verificación embargos') ||
				responseSolicitud?.nombreTarea?.includes('Verificación cuenta individual') ||
				responseSolicitud?.nombreTarea?.includes('Trámite de pago') ||
				responseSolicitud?.nombreTarea?.includes('Verificación novedades en los datos')) {
				return (
					<ProcesoVerificacionEmbargo
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						formulariosSolicitud={solicitudState.formulariosSolicitud}
						obtnerBoleano={obtnerBoleano}
						siEsAprobadaSolcitud={siEsAprobadaSolcitud}
						setMensajesEjecutados={setMensajesEjecutados}
						mensajesEjecutados={mensajesEjecutados} grupo={undefined} userAsig={undefined} userIngreso='' />
				);
			}
			if (responseSolicitud?.nombreTarea?.includes('Verificación en tiempo')) {
				const { titulo, subtitulo, icono, botonera, formularios } = solicitudState.formulariosSolicitud
				return (
					<ProcesoAcreditacion
						dynamicTitle={{ topText: titulo, title: subtitulo, icon: icono }}
						responseSolicitud={responseSolicitud}
						onItemGoBack={onItemGoBack}
						tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
						cambiarEstadoTramite={cambiarEstadoTramite}
						withButtons={mostrarBotonera}
						onSubmit={onSubmitSafe}
						formularioAcreditacion={formularioAcreditacion}
						setValidarRetiro={setValidarRetiro}
						setMostrarMensaje={setMostrarMensaje}
						tipoAccion={tipoAccion}
						mostrarBotonera={mostrarBotoneraEnProcesoAcreditacion}
						matriculaAcre={matriculaAcre ?? ''}
						vrRetiroAcre={vrRetiroAcre ?? 0}
						isloading={isloading}
						onSubmitAprobar={aprobarSolicitudTarea}
					/>

				)
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
						mensajesEjecutados={mensajesEjecutados} grupo={undefined}
					/>
				);
			}
			if (responseSolicitud?.timCodigo !== '' && solicitudState && solicitudState.formulariosSolicitud) {
				const { titulo, subtitulo, icono, botonera, formularios } = solicitudState.formulariosSolicitud
				return (

					<>

						<DynamicPage
							dynamicTitle={{ topText: titulo, title: subtitulo, icon: icono }}
							formsList={formularios}
							withButtons={solicitudState.formulariosSolicitud.botonera}
							responseSolicitud={responseSolicitud}
							tipoAccion={tipoAccion}
							tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
							modalidadSolicitud={responseSolicitud?.timCodigo || ''}
							onItemGoBack={onItemGoBack}
							onItemBack={onItemBack}
							obtnerBoleano={obtnerBoleano}
							inicioSolicitud={1 === 1}
							siEsAprobadaSolcitud={siEsAprobadaSolcitud}
							tipoSolicitud={tipoSolicitud}
							nivelAfiliado={nivelAfiliado}
						/>
					</>
				)
			}

			return (
				<InformacionProcesoSolicitud
					responseSolicitud={responseSolicitud}
					onItemGoBack={onItemGoBack}
					tipoAccion={tipoAccion}
					tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
					obtnerBoleano={obtnerBoleano}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}
					deshalitarBotonesInconsistencia={false}
					setDeshalitarBotonesInconsistencia={undefined}
					formulariosSolicitud={solicitudState.formulariosSolicitud} grupo={undefined} />
			);


		};



		return formulario();
	}

	if (isLoadingSeverance) {
		return (
			<CHPrivateContainer>
				<CHLoadingProcess />
			</CHPrivateContainer>
		);
	}

	const OnClickCancelar = () => {
		reset();
		setResponseSolicitud(undefined);
		setIniciarPantalla('');
		setSelectedTipoSubModalidad('');
		setModalidadData('');
		setTipoModalidaValidar(null);
		setNumeroCedulaVerificar(null);
		setDatoCuotas(0);
		setSolicitudes([]);
		setDesHabilitarBotonAceptar(true);
		obtnerBoleano(true);
	};

	return (
		<CHPrivateContainer>
			<LoadingV2Component key="loader" loading={loading} tittle='Consultando Biometria y Vigía Sarlaft' />
			<LoadingV2Component key="loader" loading={isloading} tittle='Validando información' />
			<CHPageTitleV2
				topText="Solicitud"
				title1="Solicitud"
				icono="fact_check"
			/>
			<Container>

				<Grid item xs={12}>
					<Card>
						<CardContent>
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIcono
										title1="Tipo"
										subtitle="de documento"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={6}>
									<Controller
										name="tipoDocumento"
										control={control}
										rules={{
											required:
												'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHSelect
												error={!!error}
												label="TIPO DE DOCUMENTO"
												value={value}
												helperText={
													error ? error.message : null
												}
												onChange={onChange}
											>
												{catalogState.IdentificationType?.map(
													(
														option: IDENTIFICATIONTYPEDTO,
													) => (
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
							<br />
							<br />
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIcono
										title1="Número"
										subtitle="de documento"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={6}>
									<Controller
										name="numeroDocumento"
										control={control}
										rules={{
											required:
												'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												numberMask
												error={!!error}
												helperText={
													error ? error.message : null
												}
												isReadOnly={false}
												InputLabelProps={{ shrink: true }}
												label="NÚMERO DE DOCUMENTO"
												value={value}
												onChange={value => {
													setValue('typerequest', '')
													setValue('typemodality', '');
													setValue('typesubmodality', '');
													setNumeroCedulaVerificar(value.target.value)
													onChange(value)
												}}
											/>
										)}
									/>
								</Grid>
							</Grid>
							<br />
							<br />
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIcono
										title1="Tipo"
										subtitle="de solicitud"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={6}>
									<Controller
										name="typerequest"
										control={control}
										rules={{
											required:
												'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHSelect
												error={!!error}
												label="TIPO SOLICITUD*"
												value={value}
												helperText={
													error ? error.message : null
												}
												onChange={value => {
													const idTipoSolicitud = value.target.value as string;
													setValue('typemodality', '');
													startTypeModality(idTipoSolicitud);
													setValue('typesubmodality', '');
													startSubModality('');
													setTipoModalidaValidar(null);
													setModalidadData('');
													setSelectedTipoSubModalidad('');
													register('typemodality', {
														required:
															'Este campo es requerido.',
													});
													setTipoSolicitud(idTipoSolicitud)
													validarBloqueo(idTipoSolicitud, solicitudes);
													onChange(value);
												}}
											>
												{catalogState.typeRequest?.map(
													(option: TYPEREQUEST) => (
														<MenuItem
															key={
																option.idTipoSolicitud
															}
															value={
																option.idTipoSolicitud
															}
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
							</Grid>
							<br />
							<br />
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIcono
										title1="Modalidad"
										subtitle=""
										icono="today"
									/>
								</Grid>
								<Grid item xs={6}>
									<Controller
										name="typemodality"
										control={control}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHSelect
												error={!!error}
												label="TIPO MODALIDAD"
												value={value}
												helperText={
													error ? error.message : null
												}
												onChange={value => {
													const dato = value.target.value as string;
													const codigoModalidad = dato.split('_')[0];
													const idTipoModalidad = dato.split('_')[1];
													setModalidadData(codigoModalidad);
													setTipoModalidaValidar(idTipoModalidad);
													startSubModality(idTipoModalidad.toString());
													setValue('typesubmodality', '');
													setSelectedTipoSubModalidad('');
													register('typesubmodality', { required: 'Este campo es requerido.' });
													onChange(value);

													const solicitudes = datoCuota?.solicitudes || [];

													if (idTipoModalidad === '18cba3da-5fb5-41b5-a6dc-a12b8a51a582') {
														const tieneNoAcreditacionV8 = solicitudes.some((solicitud: any) =>
															solicitud.modalidad === 'NO ACREDITACION V8' &&
															solicitud.estadoSolicitud !== 'INCONSISTENTE' &&
															solicitud.estadoSolicitud !== 'ANULADA'
														);

														if (tieneNoAcreditacionV8) {
															NotificationService.genericAlwarning(
																'Advertencia',
																'Afiliado incumple con los términos de acreditación',
																'warning'
															);
															setDesHabilitarBotonAceptar(true);
														}
													}
												}}

												disabled={
													catalogState.typeModality?.length === undefined ||
													catalogState.typeModality?.length === 0
												}
											>
												{catalogState.typeModality?.map(
													(option: TYPEMODALITY) => {
														const dato = `${option.codigoModalidad}_${option.idTipoModalidad}`;
														return (
															<MenuItem
																key={option.idTipoModalidad}
																value={dato}
															>
																{option.descripcionTipoModalidad}
															</MenuItem>
														);
													},
												)}
											</CHSelect>
										)}
									/>
								</Grid>
							</Grid>
							<br />
							<br />
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIcono
										title1="Sub-modalidad"
										subtitle=""
										icono="date_range"
									/>
								</Grid>
								<Grid item xs={6}>
									<Controller
										name="typesubmodality"
										control={control}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHSelect
												error={!!error}
												label="TIPO SUB-MODALIDAD"
												value={value}
												helperText={
													error ? error.message : null
												}
												onChange={async value => {
													const dato = (value.target.value as string);
													const idTipoSubmodalidad = dato.split('_')[0];
													const timCodigo = dato.split('_')[1];

													setSelectedTipoSubModalidad(idTipoSubmodalidad);

													getFormulariosSolicitud(timCodigo);
													onChange(value);
													const selectedOption = catalogState.typeSubModality?.find(
														(option) => `${option.idTipoSubModalidad}_${option.timCodigo}` === dato
													);
													const descripcionTipoSubModalidad = selectedOption?.descripcionTipoSubModalidad;
													setDescripSubModalidad(descripcionTipoSubModalidad);
												}}
												disabled={
													catalogState.typeSubModality?.length === undefined ||
													catalogState.typeSubModality?.length === 0
												}
											>
												{catalogState.typeSubModality?.map(
													(option: TYPESUBMODALITY) => {
														const dato = `${option.idTipoSubModalidad}_${option.timCodigo}`;
														return (
															<MenuItem
																key={option.idTipoSubModalidad}
																value={dato}
															>
																{option.descripcionTipoSubModalidad}
															</MenuItem>
														);
													}
												)}
											</CHSelect>
										)}
									/>
								</Grid>
							</Grid>
						</CardContent>
					</Card>
				</Grid>
				<div className="buttonssolicitud">
					<Button
						className={classes.buttonsecundario}
						variant="outlined"
						onClick={OnClickCancelar}
					>
						CANCELAR
					</Button>
					<div className={classes.wrapper}>
						<Button
							className={classes.buttonprincipal}
							variant="outlined"
							disabled={deshablitarBotonAceptar || loading}
							onClick={handleSubmit(onSubmit)}

						>
							ACEPTAR
						</Button>
						{loading && <CircularProgress size={24} className={classes.buttonProgress} />}
					</div>

				</div>
			</Container>
		</CHPrivateContainer>
	);
}

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	isLoadingSeverance: state.severance.loading,
	clientState: state.client.client,
	solicitudState: state.solicitud
});

const mapDispatchToProps = (dispatch: any) => ({
	startypeRequest: (IdTipoActor: number) =>
		dispatch(startGetAllTypeRequest(IdTipoActor)),
	startTypeModality: (idTipoSolicitud: string) =>
		dispatch(startGetAllTypeModality(idTipoSolicitud)),
	startSubModality: (idTipoModalidad: string) =>
		dispatch(startGetAllTypeSubModality(idTipoModalidad)),
	clearDetailInformation: () => dispatch(clearDetailRequestInformation()),
	obtenerTipoDocumento: () => dispatch(startGetAllTypeDocuments()),
	startPostRequestSeverance: (data: RequestRequestSeveranceModel) =>
		dispatch(startGetAllPostRequestSeverance(data)),
	getFormulariosSolicitud: (timCodigo: string) =>
		dispatch(getFormulariosSolicitud(timCodigo))
});

export default connect(mapStateToProps, mapDispatchToProps)(Solicitudes);