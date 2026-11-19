import axios, { AxiosResponse } from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { RequestActivityRequestModel } from '../models/requests/request-activity-request.model';
import { CreditBasicDataRequestModel } from '../models/requests/credit-basic-data-request.model';
import { CreditProductRequestModel, Matricula } from '../models/requests/credit-product-request.model';
import { CreditObservationsRequestModel } from '../models/requests/credit-observations.request.model';
import { CreditFilesRequestModel } from '../models/requests/credit-files-request.model';
import { CreditFinancialDataRequestModel } from '../models/requests/credit-financial-data-request.model';
import { CreditTechnicalCreateRequestModel } from '../models/requests/credit-technical-create-request.model';
import { CreditTechnicalDataRequestModel } from '../models/requests/credit-technical-data-request.model';
import { CreditLegalDataRequestModel } from '../models/requests/credit-legal-data-request.model';
import { CreditLegalCreateRequestModel } from '../models/requests/credit-legal-create-request.model';
import { CreditFlowDocumentsRequestModel } from '../models/requests/credit-flow-documents-request.model';
import { CreditConsultCreditRequestModel } from '../models/requests/credit-consult-credit-request.model';
import { CreditUpdateRecommendationRequestModel } from '../models/requests/credit-update-recommendation-request.model';
import { CreditUpdateInsuranceRequestModel } from '../models/requests/credit-update-insurance-request.model';
import { ViewPdfFiRiesgoModel, ViewPdfModel } from '../models/requests/credit-view-pdf.model';
import { CreateDesembolsoModel } from '../models/requests/credit-desembolso-model';
import { ParametrosDesembolsoSevenModel } from '../models/credito-desembolso-seven';
import { ConsultaPreaprobado } from '../types/preaprobados.model.type';
import { IS_DEBUG } from '../constants/constants';

const CreditApi = () => {
	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'accept': 'application/json',
			'X-Frame-Options': 'sameorigin'
		},
		baseURL: `${routega2api}credito`,
		timeout: 600000,
	});

	const apiAxios2 = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/xml',
			'accept': 'application/json'
		},
		baseURL: `${routega2api}`,
		timeout: 120000,
	});

	const apiAxiosTusDatos = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			Accept: 'application/json',
			'X-Frame-Options': 'sameorigin'
		},
		baseURL: routega2api,
		timeout: 600000,
	});

	apiAxiosTusDatos.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();

		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}

		delete config.headers['Content-Type'];
		delete config.headers['content-type'];

		delete config.headers.common?.['Content-Type'];
		delete config.headers.common?.['content-type'];

		delete config.headers.get?.['Content-Type'];
		delete config.headers.get?.['content-type'];

		return config;
	});


	apiAxios.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();

		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}

		return config;
	});
	apiAxios2.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	const getProcessRequest = async (data: any) =>
		apiAxios
			.post(`obtenerTramiteSolicitud`, data)
			.then((res: any) => res.data);

	const putRequestActivityApprove = async (data: RequestActivityRequestModel) =>
		apiAxios
			.put(`apruebaActividadTramite`, data)
			.then((res: any) => res.data);

	const putRequestActivityRejected = async (data: RequestActivityRequestModel) =>
		apiAxios
			.put(`rechazarActividadTramite`, data)
			.then((res: any) => res.data);

	const putRequestActivityRemedy = async (data: RequestActivityRequestModel) =>
		apiAxios
			.put(`enviaSubsancionActividadTramite`, data)
			.then((res: any) => res.data);

	const putRequestActivityUpdate = async (data: RequestActivityRequestModel) =>
		apiAxios
			.put(`actualizaActividadTramite`, data)
			.then((res: any) => res.data);

	const putRequestActivityBack = async (data: RequestActivityRequestModel) =>
		apiAxios
			.put(`devolverActividadTramite`, data)
			.then((res: any) => res.data);
	const putRequestActivityDisitir = async (data: RequestActivityRequestModel) =>
		apiAxios
			.put(`desistirActividadTramite`, data)
			.then((res: any) => res.data);

	const postCreditBasicData = async (data: CreditBasicDataRequestModel) =>
		apiAxios
			.post('obtenerSolicCreditoBasica', data)
			.then((res: any) => res.data);

	const postCreditProductData = async (data: CreditProductRequestModel) =>
		apiAxios
			.post('obtenerSolicCreditoProducto', data)
			.then((res: any) => res.data);

	const postCreditObservations = async (data: CreditObservationsRequestModel) =>
		apiAxios
			.post('obtenerObservacionTramite', data)
			.then((res: any) => res.data);

	const postCreditFiles = async (data: CreditFilesRequestModel) =>
		apiAxios
			.post('obtenerDocumentosActSolicitud', data)
			.then((res: any) => res.data)

	const getPrueba = async (data: any) =>
		apiAxios
			.post('insertForm', data)
			.then((res: any) => res.data);

	const postCreditFinancialData = async (data: CreditFinancialDataRequestModel) =>
		apiAxios
			.post('obtenerSolicCreditoFinancieros', data)
			.then((res: any) => res.data);

	const postCreditCreateTechnicalData = async (data: CreditTechnicalCreateRequestModel) =>
		apiAxios
			.post('crearSolicCreditoInfTecnica', data)
			.then((res: any) => res.data);

	const postCreditTechnicalData = async (data: CreditTechnicalDataRequestModel) =>
		apiAxios
			.post('obtenerSolicCreditoInfTecnica', data)
			.then((res: any) => res.data);

	const postCreditLegalData = async (data: CreditLegalDataRequestModel) =>
		apiAxios
			.post('obtenerSolicCreditoInfJuridica', data)
			.then((res: any) => res.data);

	const postCreditCreateLegalData = async (data: CreditLegalCreateRequestModel) =>
		apiAxios
			.post('crearSolicCreditoInfJuridica', data)
			.then((res: any) => res.data);

	const postCreditClientSimulation = (id: string) =>
		apiAxios
			.get(`credito/consultarSimulacionCliente/?request=${id}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const postValidarBiometriaFenix = (id: string) => {
		if (IS_DEBUG) {
			const response = {
				isSuccess: true,
				data: {
					newDataSet: {
						table1: { a008HUELLA: 1, a008CEDULA: 1 }
					}
				}
			}
			return Promise.resolve(response).then(res => res);
		}
		return apiAxios.get(`credito/validarBiometria?request=${id}`, {
			baseURL: routega2api,
		})
			.then(res => res.data)
			.catch((err: any) => err);
	}

	const TUSDATOS_INTERVALO_MS = 6000;

	const esperar = (ms: number) =>
		new Promise<void>(resolve => setTimeout(resolve, ms));

	let colaTusDatos: Promise<unknown> = Promise.resolve();
	let ultimaConsultaTusDatos = 0;

	const ejecutarConsultaTusDatosEnCola = <T>(
		consulta: () => Promise<T>
	): Promise<T> => {
		const ejecucion = colaTusDatos
			.catch(() => undefined)
			.then(async () => {
				const tiempoTranscurrido = Date.now() - ultimaConsultaTusDatos;
				const tiempoEspera = TUSDATOS_INTERVALO_MS - tiempoTranscurrido;

				if (tiempoEspera > 0) {
					await esperar(tiempoEspera);
				}

				ultimaConsultaTusDatos = Date.now();

				return consulta();
			});

		colaTusDatos = ejecucion.then(
			() => undefined,
			() => undefined
		);

		return ejecucion;
	};

	const obtenerErrorTusDatos = (err: any) => ({
		...(err?.response?.data || {}),
		status: err?.response?.status,
		httpStatus: err?.response?.status,
		isSuccess: false,
		returnMessage:
			err?.response?.data?.returnMessage ||
			err?.message ||
			'No fue posible realizar la consulta en TusDatos.',
	});

	const getValidacionTusDatos = (
		identificacion: string,
		documentType: string,
	) => {
		if (IS_DEBUG) {
			const response = {
				isSuccess: true,
				returnMessage: 'Consulta exitosa',
				data: {
					jobId: null,
					isFromHistory: true,
					historyDataCustomer: {
						isPEPS: false,
						detectionRisk: 'bajo',
					},
				},
			};

			return Promise.resolve(response);
		}

		return ejecutarConsultaTusDatosEnCola(() =>
			apiAxiosTusDatos
				.get(
					'terceros/validacion/riesgos/tusdatos',
					{
						params: {
							Identificacion: identificacion,
							DocumentType: documentType,
						},
					},
				)
				.then(res => res.data)
				.catch(obtenerErrorTusDatos),
		);
	};

	const getPollingTusDatos = (
		identificacion: string,
		jobId: string,
	) => {
		if (IS_DEBUG) {
			const response = {
				isSuccess: true,
				returnMessage: 'Consulta exitosa',
				data: {
					jobId: null,
					isFromHistory: false,
					historyDataCustomer: {
						isPEPS: false,
						detectionRisk: 'bajo',
					},
				},
			};

			return Promise.resolve(response);
		}

		return ejecutarConsultaTusDatosEnCola(() =>
			apiAxiosTusDatos
				.get(
					'terceros/validacion/riesgos/tusdatos/polling',
					{
						params: {
							Identificacion: identificacion,
							jobId,
						},
					},
				)
				.then(res => res.data)
				.catch(obtenerErrorTusDatos),
		);
	};

	const validarTusDatosConPolling = async (
		identificacion: string,
		documentType: string,
		maxIntentos = 10,
		intervaloMs = 6000,
		maxReintentosError415 = 2,
	) => {
		let cantidadReintentos415 = 0;

		/* eslint-disable no-await-in-loop */
		while (cantidadReintentos415 <= maxReintentosError415) {
			const primeraRespuesta = await getValidacionTusDatos(
				identificacion,
				documentType,
			);

			if (!primeraRespuesta?.isSuccess) {
				return primeraRespuesta;
			}

			const jobId = primeraRespuesta?.data?.jobId;

			if (!jobId) {
				return primeraRespuesta;
			}

			let reiniciarConsultaTusDatos = false;

			for (
				let intento = 1;
				intento <= maxIntentos;
				intento += 1
			) {
				await esperar(intervaloMs);

				const respuestaPolling =
					await getPollingTusDatos(
						identificacion,
						jobId,
					);

				const statusPolling =
					respuestaPolling?.status ||
					respuestaPolling?.httpStatus;

				if (Number(statusPolling) === 415) {
					cantidadReintentos415 += 1;
					reiniciarConsultaTusDatos = true;
					break;
				}

				if (!respuestaPolling?.isSuccess) {
					return respuestaPolling;
				}

				const pollingJobId =
					respuestaPolling?.data?.jobId;

				const historyDataCustomer =
					respuestaPolling?.data
						?.historyDataCustomer;

				if (
					!pollingJobId &&
					historyDataCustomer
				) {
					return respuestaPolling;
				}
			}

			if (!reiniciarConsultaTusDatos) {
				return {
					isSuccess: false,
					returnMessage:
						'La validación de TusDatos no respondió dentro del tiempo esperado.',
					data: null,
				};
			}
		}
		/* eslint-enable no-await-in-loop */

		return {
			isSuccess: false,
			returnMessage:
				'No fue posible completar la validación de TusDatos después de reiniciar la consulta.',
			data: null,
		};
	};

	const getValidacionActualizacionVigia = (
		identificacion: string,
		documentType: string
	) => {
		if (IS_DEBUG) {
			const response = {
				isSuccess: true,
				returnMessage: 'Consulta exitosa',
				data: {
					jobId: null,
					isFromHistory: true,
					historyDataCustomer: {
						isPEPS: false,
						detectionRisk: 'bajo'
					}
				}
			};

			return Promise.resolve(response);
		}

		return apiAxiosTusDatos.get(
			'terceros/validacion/actualizacion/vigia',
			{
				params: {
					Identificacion: identificacion,
					DocumentType: documentType
				}
			}
		)
			.then(res => res.data)
			.catch((err: any) => err?.response?.data || err);
	};

	const patchActualizarPersonalesVivienda = async (request: any) =>
	apiAxios
		.patch('actualizarPersonalesVivienda', request)
		.then((res: any) => res.data);

	const postCrearSimulacionDatosFinancieros = async (request: any) =>
		apiAxios
			.post('CrearSimulacionDatosFinancieros', request)
			.then((res: any) => res.data);

	const postCreditCreateClientSimulation = async (request: any) =>
		apiAxios
			.post('CrearSimulacionCliente', request, {
				baseURL: `${routega2api}prospeccion`
			})
			.then((res: any) => res.data);

	const postCreditFlowDocuments = async (data: CreditFlowDocumentsRequestModel) =>
		apiAxios
			.post('obtenerDocumentosFlujo', data)
			.then((res: any) => res.data);

	const getCreditFlowDocuments = () =>
		apiAxios
			.get('obtenerTipoDocumento')
			.then((res: any) => res.data);

	const postCreditUploadDocument = async (data: FormData) =>
		apiAxios
			.post('cargaDocumentoSolicCredito', data)
			.then((res: any) => res.data);

	const postCreditCreateDocument = async (data: any) =>
		apiAxios
			.post('crearDocumentoSolicCredito', data)
			.then((res: any) => res.data);

	const postCreditDownloadFileS = async (request: string) =>
		apiAxios
			.post('descargaDocumentoSolicCredito', request, {
				params: {
					request,
				},
				responseType: 'blob'
			})
			.then((res: any) => res.data);

	const postCreditDownloadFileLeasing = async (request: string) =>
		apiAxios
			.post('generarCesionLeasing', request, {
				params: {
					request,
				},
				responseType: 'blob'
			})
			.then((res: any) => res.data);

	const postCreditDownloadFile = async (request: ViewPdfModel) =>
		apiAxios
			.post('generarReporteHaberes', request, {
				responseType: 'blob'
			})
			.then((res: any) => res.data);

	const postCreditDownloadFileFiRiesgo = async (request: ViewPdfFiRiesgoModel) =>
		apiAxios
			.post('obtenerFichaRiesgo', request, {
				responseType: 'blob'
			})
			.then((res: any) => res.data);

	const postConsultCreditData = async (data: CreditConsultCreditRequestModel) =>
		apiAxios
			.post('consultarSolicitudCredito', data)
			.then((res: any) => res.data);

	const postUpdateRequestCreditRecommendation = async (
		request: CreditUpdateRecommendationRequestModel,
	) =>
		apiAxios
			.post('actualizaSolicCreditoRecomendacion', request)
			.then((res: any) => res.data);

	const postUpdateInsurance = async (request: CreditUpdateInsuranceRequestModel) =>
		apiAxios
			.post('actualizaSolicCreditoSeguro', request)
			.then((res: any) => res.data);

	const postObtainPDFResume = async (requestID: string) =>
		apiAxios
			.post('obtenerDatosSolicComite', null, {
				params: {
					request: requestID,
				},
				responseType: 'blob'
			})
			.then((res: any) => res.data);

	const postCreditCommittee = async (request: any) =>
		apiAxios
			.post('obtenerComiteCredito', request)
			.then((res: any) => res.data);

	const postCreditCommitteeSoli = async (request: any) =>
		apiAxios
			.post('obtenerComiteCreditoSolicitud', request)
			.then((res: any) => res.data);

	const getMembersCommittee = () =>
		apiAxios.get('obtenerIntegranteComite').then((res: any) => res.data);

	const getTopicsCommittee = () =>
		apiAxios.get('obtenerTemasComite').then((res: any) => res.data);

	const postGetMembersCommit = async (request: any) =>
		apiAxios
			.post('obtenerComiteCreditoIntegrante', request)
			.then((res: any) => res.data);

	const postGetTopicsCommit = async (request: any) =>
		apiAxios
			.post('obtenerComiteCreditoTemas', request)
			.then((res: any) => res.data);

	const postCreateCommitteeTopics = async (request: any) =>
		apiAxios
			.post('crearComiteCreditoTemas', request)
			.then((res: any) => res.data);

	const postCreateCommitteeMembers = async (request: any) =>
		apiAxios
			.post('crearComiteCreditoIntegrante', request)
			.then((res: any) => res.data);

	const postCreateCommittee = async (request: any) =>
		apiAxios
			.post('crearComiteCredito', request)
			.then((res: any) => res.data);

	const postCreditCommitteeRequest = async (request: any) =>
		apiAxios
			.post('obtenerComiteCredito', request)
			.then((res: any) => res.data);

	const postCreditCommitteeApprove = async (request: any) =>
		apiAxios
			.post('aprobarComiteCredito', request)
			.then((res: any) => res.data);

	const postCreateCreditCommitteeRequest = async (request: any) =>
		apiAxios
			.post('crearComiteCreditoSolicitud', request)
			.then((res: any) => res.data);

	const postFinancialData = async (request: any) =>
		apiAxios
			.post('cargarDatosFinancieros', request)
			.then((res: any) => res.data);

	const getResourcesSource = () =>
		apiAxios
			.get('obtenerFuentesRecursos')
			.then((res: AxiosResponse) => res.data);

	const postUploadDisbursementRates = async (request: any) =>
		apiAxios
			.post('cargarDatosSolicCreditoTasasDesembolso', request)
			.then((res: AxiosResponse) => res.data);

	const getFuentes = (request: any) =>
		apiAxios
			.get(`credito/obtenerFuentesRecursosSolicitud?request=${request}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const deleteFuente = (index: any) =>
		apiAxios
			.delete(`credito/EliminarFuentesRecursosSolicitud?IdSolicitudCredito=${index.idSolicitudCredito}&IdSolicitudDesembolso=${index.idSolicitudDesembolso}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getInsurances = () =>
		apiAxios
			.get('obteneraseguradoras')
			.then((res: AxiosResponse) => res.data);

	const postDesembolso = async (data: CreateDesembolsoModel) =>
		apiAxios
			.post('obtenerDesembolsosSolicitud', data)
			.then((res: any) => res.data);

	const patchDesembolso = async (data: CreateDesembolsoModel) =>
		apiAxios
			.post('AplicarDesembolsoSolicitud', data)
			.then((res: any) => res.data);

	const postLegalizacion = async (data: any) =>
		apiAxios
			.post('actualizaSolicCreditoDesembolso', data)
			.then((res: any) => res.data);

	const postComitePdf = (request: any) =>
		apiAxios
			.post(`generarActaComite?request=${request}`, {}, {
				responseType: 'blob',
			})
			.then((res: any) => res.data);

	const postLegalizacionFirmas = async (request: any) =>
		apiAxios
			.post('actualizaSolicCreditoFirmas', request)
			.then((res: any) => res.data);

	const PostVerificacionTerc = async (request: any) => {
		if (IS_DEBUG) {
			const response = {
				data: {
					encontrado: 'N'
				}
			}
			return Promise.resolve(response).then(res => res);
		}

		return apiAxios.post('verificacionTercero', request)
			.then((res: any) => res.data)
			.catch((err: any) => err)
	}

	const GetLegalizacionData = (request: any) =>
		apiAxios
			.get(`obtenerSolicitudCredito?IdSolicitud=${request}&Indicador=${1}`, {
			})
			.then((res: any) => res.data);

	const getCausalDesistimiento = () =>
		apiAxios.get(`obtenerCausalDesistimiento`)
			.then((res: any) => res.data);

	const PostDesistminetoDesembolso = async (request: any) =>
		apiAxios
			.post('desistirDesembolsoSolicitud', request)
			.then((res: any) => res.data);

	const postObtenerHistorialComercialBuro = async (request: any) =>
		apiAxios
			.post('obtenerHistorialComercialBuro', request)
			.then((res: any) => res.data);

	const postObtenerPreaprobaciones = (request: ConsultaPreaprobado) =>
		apiAxios
			.post('ObtenerPreaprobaciones', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getObtenerSimulacionPersonales = (request: string) =>
		apiAxios
			.get(`obtenerSimulacionPersonales?request=${request}`)
			.then((res: any) => res.data);

	const getObtenerDatosFinancierosPersonales = (request: string) =>
		apiAxios
			.get(`obtenerDatosFinancierosPersonales?request=${request}`)
			.then((res: any) => res.data);

	const getConsultarSimulacionCliente = (request: any) =>
		apiAxios
			.get(`consultarSimulacionCliente?request=${request}`)
			.then((res: any) => res.data);

	const postCierreDiarioCartera = (request: any) =>
		apiAxios
			.post('credito/CierreDiarioCartera', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postCierreMensualCartera = (request: any) =>
		apiAxios
			.post('credito/CierreMensualCartera', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postCierreAnualCartera = (request: any) =>
		apiAxios
			.post('credito/CierreAnualCartera', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postCausacionDiariaInteresesCorrientes = (request: any) =>
		apiAxios
			.post('credito/CausacionDiariaInteresesCorrientes', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getObtenerTiposDeCierreCartera = () =>
		apiAxios
			.get('credito/ObtenerTiposDeCierreCartera', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const ObtenerCierresCausacionesRealizadas = (request: any) =>
		apiAxios
			.post('credito/ObtenerCierresCausacionesRealizadas', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const insertarProcesoAplicacionPago = (request: any) =>
		apiAxios
			.post('credito/InsertarProcesoAplicacionPagos', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const actualizarProcesoAplicacionPago = (request: any) =>
		apiAxios
			.post('credito/ActualizarProcesoAplicacionPagos', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const obtenerProcesoAplicacionPagos = (request: {
		unidadEjecutora?: number,
		estado?: string,
		fechaCreacion?: Date,
		tipoCredito?: number
	}) =>
		apiAxios
			.get(`credito/obtenerProcesoAplicacionPagos`, {
				baseURL: routega2api,
				params: {
					idUnidadEjecutora: request.unidadEjecutora,
					periodo: formatPeriodo(request.fechaCreacion),
					estado: request.estado,
					IdTipoCredito: request?.tipoCredito
				}
			}).then(res => res.data);


	const formatPeriodo = (fecha?: Date) => {
		if (fecha) {
			const year = fecha.getFullYear();
			const month = (fecha.getMonth() + 1).toString().padStart(2, '0');
			return `${year}${month}`;
		}
		return null;
	};


	const obtenerProcesoAplicacionPagosPorUnidadEjecutora = (unidadEjecutora?: number, tipoCredito?: number, periodo?: string, estado?: string) =>
		apiAxios
			.get(`credito/ObtenerProcesoAplicacionPagosPorUnidadEjecutora`, {
				baseURL: routega2api,
				params: {
					unidadEjecutora,
					tipoCredito,
					periodo,
					estado
				}
			}).then(res => res.data);

	const aplicarPagoMasivoDesdeLista = (idProceso: number) =>
		apiAxios
			.post(`cartera/AplicarPagoMasivoDesdeLista`, {}, {
				baseURL: routega2api,
				params: { idProceso },
				timeout: 240000
			}).then(res => res.data);

	const eliminarProcesoAplicacionPagos = (idProceso: number) =>
		apiAxios
			.delete(`credito/EliminarProcesoAplicacionPagos`, {
				baseURL: routega2api,
				params: { idProceso }
			}).then(res => res.data);

	const getDescargarReporteAltasBajas = (request: any) =>
		apiAxios
			.get('cartera/descargarReporteAltasBajas', {
				baseURL: routega2api,
				params: {
					idUnidadEjecutora: request.idUnidadEjecutora,
					tipoReporte: request.tipoReporte,
					periodoGeneracion: request.periodoGeneracion
				},
				responseType: 'blob',
				timeout: 180000
			})
			.then((res: any) => res.data);

	const getObtenerDatosSimulacionParaEstudio = (request: any) =>
		apiAxios
			.get(`ObtenerDatosSimulacionParaEstudio?request=${request}`)
			.then((res: any) => res.data);

	const postCargarDocumentosSolicitudCredito = (data: FormData, idSolicitud: string | undefined) =>
		apiAxios
			.post(`${'CargarDocumentosSolicitudCredito/'} ${idSolicitud}`, data)
			.then((res: any) => res.data);

	const getObtenerCausalesRechazo = () =>
		apiAxios
			.get('ObtenerCausalesRechazo')
			.then((res: any) => res.data);

	const getObtenerEstadosTramite = () =>
		apiAxios
			.get('ObtenerEstadosTramite')
			.then((res: any) => res.data);

	const getObtenerTramitesPendientes = () =>
		apiAxios
			.get('ObtenerTramitesPendientes')
			.then((res: any) => res.data);

	const postObtenerMatriculasCreditoPorSolicitud = async (socId: any) =>
		apiAxios
			.post(`ObtenerMatriculasCreditoporSolicitud?request=${socId}`, {}, {
				baseURL: `${routega2api}prospeccion`
			})
			.then((res: any) => res.data);

	const postCrearMatriculaCredito = (matricula: Matricula) =>
		apiAxios
			.post(`Crearmatriculacredito`, matricula,)
			.then((res: any) => res.data);

	const updateMatriculaCredito = (matricula: Matricula) =>
		apiAxios
			.patch(`ActualizarMatriculasCredito`, matricula)
			.then(res => res.data);



	const deleteMatriculaCredito = (idMatricula: any) =>
		apiAxios
			.delete(`credito/EliminarMatriculasCreditoSolicitud?request=${idMatricula}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const posDatosUsuario = (request: any) =>
		apiAxios
			.get(`obtenerSimulacionPersonales?request=${request.request}`)
			.then((res: any) => res.data);

	const postObtenerTramiteSolicitudCliente = (identificacion: string) =>
		apiAxios
			.post(`obtenerTramiteSolicitudCli?request=${identificacion}`, {},)
			.then((res: any) => res.data);


	const postCrearEscritura = (request: any) =>
		apiAxios
			.post('CrearEscritura', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postObtenerEscritura = (request: any) =>
		apiAxios
			.post('ObtenerEscritura', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postActualizarEscritura = (request: any) =>
		apiAxios
			.post('ActualizarEscritura', request, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const posCrearTerceroSeven = (parametrosDesembolsoSevenModel: ParametrosDesembolsoSevenModel) =>
		apiAxios
			.post(`cesantias/crearTerceroSeven`, parametrosDesembolsoSevenModel,)
			.then(res => res.data);

	const posCrearCDPSeven = (parametrosDesembolsoSevenModel: ParametrosDesembolsoSevenModel) =>
		apiAxios
			.post(`cesantias/crearCDPSeven`, parametrosDesembolsoSevenModel,)
			.then(res => res.data);

	const posCrearProveedorSeven = (parametrosDesembolsoSevenModel: ParametrosDesembolsoSevenModel) =>
		apiAxios
			.post(`cesantias/crearProveedorSeven`, parametrosDesembolsoSevenModel,)
			.then(res => res.data);

	const posCrearFacturaProveSeven = (parametrosDesembolsoSevenModel: ParametrosDesembolsoSevenModel) =>
		apiAxios
			.post(`cesantias/crearFacturaProveSeven`, parametrosDesembolsoSevenModel,)
			.then(res => res.data);

	const getObtenerValorDesembolsoCredito = (numeroDocumento: string) =>
		apiAxios
			.get(`credito/ObtenerValorDesembolsoCredito?numeroDocumento=${numeroDocumento}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const posPagoCuotaInicalSeven = (parametrosDesembolsoSevenModel: ParametrosDesembolsoSevenModel) =>
		apiAxios
			.post(`credito/pagoCuotaInicalSeven`, parametrosDesembolsoSevenModel, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const postObtenerSolicitudCreditoJuridica = (parametrosSolicitud: any) =>
		apiAxios
			.post(`credito/obtenerSolicCreditoInfJurInm`, parametrosSolicitud, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const postCrearInvitadoCmite = (invitado: any) =>
		apiAxios
			.post(`credito/CrearIntegranteComiteCredito`, invitado, {
				baseURL: routega2api,
			})
			.then(res => res.data);
	const getObtnerInvitadosComite = () =>
		apiAxios
			.get('obtenerIntegranteComite')
			.then((res: any) => res.data);

	const postCrearSolicitudJuridicaInmueble = (parametrosSolicitud: any) =>
		apiAxios
			.post(`credito/crearSolicCreditoInfJurInm`, parametrosSolicitud, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const postCrearAseguradoras = async (data: any) =>
		apiAxios
			.post('CrearAseguradoras', data)
			.then((res: any) => res.data);

	const getObtenerAseguradoras = () =>
		apiAxios
			.get(
				`credito/obteneraseguradoras`,
				{
					baseURL: routega2api,
				},
			)
			.then((res: any) => res.data);

	const putAseguradorasActulizar = (response: any) =>
		apiAxios
			.patch(`actualizaraseguradoras`, response)
			.then(res => res.data);

	const getObtenerTipoActividad = async (request: any) =>
		apiAxios
			.get(`obtenertipoactividad?request=${request}`)
			.then((res: any) => res.data);

	const getObtenerSolicitudCreditoTipoNumeroIdentificacion = (tipoDocumento: number, numeroDocumento: string) =>
		apiAxios
			.get(
				`ObtenerSolicitudCreditoTipoNumeroIdentificacion?tipoDocumentoId=${tipoDocumento}&numeroDocumento=${numeroDocumento}`,
				{
					baseURL: routega2api,
				},
			)
			.then((res: any) => res.data);


	const postProspeccionTransUnion = async (data: any) =>
		apiAxios
			.post(`ProspeccionCredito`, data)
			.then(res => res.data);

	const postReporteInformacionComercial = (request: any) =>
		apiAxios2
			.post(
				'Reportes/ReporteTransUnionPropiedades', request, {

				responseType: 'blob',

			})
			.then(res => res.data);

	const postReporteInformacionComercialPropiedades = async (xml: any) => {
		try {
			const response = await apiAxios2.post(`Reportes/ReporteTransUnionPropiedades?xml=${xml}`, {}, {
				responseType: 'json', // Indica que esperas una respuesta en formato Blob
			});
			return response.data;
		} catch (error) {
			console.error('Error al enviar el objeto JSON al backend:', error);
			return null;
		}
	};

	const ObtenerProductoByIdentificacionRequest = async (data: any) => {

		apiAxios
			.post(`obtenerProductoCliente`, data)
			.then(res => res.data);
	}

	const postCrearCDPSeven = async (data: any) =>
		apiAxios
			.post('CrearCDPSevenCredito', data)
			.then((res: any) => res.data);

	const DesmbolsoCreditoMomento1 = async (data: any) =>
		apiAxios
			.post('DesEmbolsoCreditoMomento1', data)
			.then((res: any) => res.data);

	const DesmbolsoCreditoMomento2 = async (data: any) =>
		apiAxios
			.post('DesEmbolsoCreditoMomento2', data)
			.then((res: any) => res.data);



	const postCrearTerceroSevenCredito = async (data: any) =>
		apiAxios
			.post('CrearTerceroSevenCredito', data)
			.then((res: any) => res.data);

	const postCrearProveedorSeven = async (data: any) =>
		apiAxios
			.post('CrearProveedorSevenCredito', data)
			.then((res: any) => res.data);

	const postCrearCDPPrimerPago = async (data: any) =>
		apiAxios
			.post('CrearCDPSevenCreditoPrimerPago', data)
			.then((res: any) => res.data);

	const postCrearCDPSegundoPago = async (data: any) =>
		apiAxios
			.post('CrearCDPSevenCreditoSegundoPago', data)
			.then((res: any) => res.data);

	const postCrearFacturaSevenPrimerPago = async (data: any) =>
		apiAxios
			.post('CrearFacturaSevenPrimerPago', data)
			.then((res: any) => res.data);

	const postCrearFacturaSevenSegundoPago = async (data: any) =>
		apiAxios
			.post('CrearFacturaSevenSegundoPago', data)
			.then((res: any) => res.data);

	const postCrearSolicitudCredito = async (request: any) =>
		apiAxios
			.post('prospeccion/CrearSolicitudCredito', request)
			.then((res: any) => res.data);

	const getObtenerDatosSimulacion = (request: any) =>
		apiAxios
			.get(`obtenerDetalleSimulacionCliente?request=${request.smc_id}`)
			.then((res: any) => res.data);

	const postCreaSCnMcontAprobacionCredito = async (request: any) =>
		apiAxios
			.post('CreaSCnMcontAprobacionCredito', request)
			.then((res: any) => res.data);

	const postObtenerValoresProyectados = (request: any) =>
		apiAxios
			.post(`ObtenerValoresProyectados?request=${request}`, {},)
			.then((res: any) => res.data);

	const postObtenerVendedor = (data: any) =>
		apiAxios
			.post(`ObtenerInformacionVendedor`, data)
			.then((res: any) => res.data);

	const postCrearVendedor = (data: any) =>
		apiAxios
			.post('CrearInformacionVendedor', data)
			.then((res: any) => res.data);



	const deleteEliminarVendedor = (index: any) =>
		apiAxios
			.delete(`EliminarInformacionVendedor?Id=${index}`)
			.then(res => res.data);


	const ReporteGeneralHaberes = (request: any) =>
		apiAxios
			.post(
				'ReporteHaberes', request, {

				responseType: 'blob',
			})
			.then(res => res.data);

	const postActualizaTramitePerito = (request: any) =>
		apiAxios
			.post(`ActualizaTramitePerito`, request,)
			.then((res: any) => res.data);

	const postDocumentosActSimulacion = (request: any) =>
		apiAxios
			.post(`obtenerDocumentosActSimulacion`, request)
			.then((res: any) => res.data);

	const postVerArchivos = async (request: string) =>
		apiAxios
			.post('descargaDocumentoSolicCredito', request, {
				params: {
					request,
				},
				responseType: 'blob'
			})
			.then((res: any) => res.data);

	const postActualizaDocumentoLegalizacion = (request: any) =>
		apiAxios
			.post(`ActualizarDocLegalizacion`, request,)
			.then((res: any) => res.data);

	const postObtenerValorCdp = (request: any) =>
		apiAxios
			.post(`ObtenerValorCdp`, request,)
			.then((res: any) => res.data);

	const postObtenerValorFactura = (request: any) =>
		apiAxios
			.post(`obtenervalorfactura`, request,)
			.then((res: any) => res.data);

	const postObtenerComprobantesCredito = (request: any) =>
		apiAxios
			.post(`ObtenerComprobantesCredito`, request,)
			.then((res: any) => res.data);

	const postGuardarCdp = (request: any) =>
		apiAxios
			.post(`InsertarNotificacionesSolicitudesCredito`, request,)
			.then((res: any) => res.data);

	const postConsultarNotificacionesSolicitudesCredito = (request: any) =>
		apiAxios
			.post(`ConsultarNotificacionesSolicitudesCredito?request=${request}`,)
			.then((res: any) => res.data);

	const PostConsultarAprobacionAsesorCredito = (request: any) =>
		apiAxios
			.post(`ConsultarAprobacionAsesorCredito?request=${request}`,)
			.then((res: any) => res.data);
	const postObtenerPesoYTalla = (request: any) =>
		apiAxios
			.post(`ConsultarAsegurabilidadCredito?request=${request}`,)
			.then((res: any) => res.data);

	const postInsertarCdp = (request: any) =>
		apiAxios
			.post(`InsertarNotificacionesSolicitudesCredito`, request)
			.then((res: any) => res.data);
	const postObteberExcelCredito = (request: any) =>
		apiAxios
			.post(`ObtenerReportesSolicitudCreditoGeneral`, request)
			.then((res: any) => res.data);
	const postReportesSolicitudCreditoGeneralUsuarioExterno = (request: any) =>
		apiAxios
			.post(`ReportesSolicitudCreditoGeneralUsuarioExterno`, request)
			.then((res: any) => res.data);

	const getConsultarFacturaSeven = async (request: any) =>
		apiAxios
			.get(`ConsultarDatosFacturaSeven?request=${request}`)
			.then((res: any) => res.data);

	const getObtenertipoactividadporid = (request: any) =>
		apiAxios
			.get(`obtenertipoactividadporid?request=${request}`)
			.then((res: any) => res.data);

	const postRegistrarBloqueoCuentaCredito = (request: any) =>
		apiAxios
			.post(`RegistrarBloqueoCuentaCredito`, request)
			.then((res: any) => res.data);


	const getObtenerSalarioMinimoAnual = async (
		anio: number,
		categoria: number
	) =>
		apiAxios2
			.get(
				`ObtenerSalarioMinimoAnual?CTG_ID=${categoria}&SMLV_ANIO=${anio}`
			)
			.then((res: any) => res.data);


	const getConsultarAliadosEstrategicos = async () =>
		apiAxios
			.get(`ConsultarAliadosEstrategicos`)
			.then((res: any) => res.data);


	const insertarActualizarAvaluoComercialCredito = (request: any) =>
		apiAxios
			.post(`InsertarActualizarAvaluoComercialCredito`, request)
			.then((res: any) => res.data);


	const getConsultarAvaluoComercialCredito = async (
		socId: string
	) =>
		apiAxios
			.get(
				`ConsultarAvaluoComercialCredito/${socId}`
			)
			.then((res: any) => res.data);


	const getConsultaValoresSimulacionCreditoReclasificiacion = (numeroDocumento: string) =>
		apiAxios
			.get(`/ConsultaValoresSimulacionCreditoReclasificiacion/${numeroDocumento}`)
			.then((res: any) => res.data);


	const getConsultarValidacionCreditoProspeccion = (numeroDocumento: string) =>
		apiAxios
			.get(`/ConsultarValidacionCreditoProspeccion/${numeroDocumento}`)
			.then((res: any) => res.data);

	const postInsertarActualizarValidacionCreditoProspeccion = (request: any) =>
		apiAxios
			.post(`InsertarActualizarValidacionCreditoProspeccion`, request)
			.then((res: any) => res.data);

	const PostCargarDocumentosActividadCreditoDODO = (
		data: any,
	) =>
		apiAxios
			.post(
				'CargarDocumentosActividadCreditoDODO',
				data,
				{
					baseURL:
						routega2api,

					headers: {
						'Content-Type':
							'application/json',

						Accept:
							'application/json',
					},
				},
			)
			.then(
				(res: any) =>
					res.data,
			);


	const getconsultarusuariocredito = (request: string) =>
		apiAxios
			.get(`/consultarusuariocredito/${request}`)
			.then((res: any) => res.data);

	const PostObtenerDocumentoPorTipoActividad = (data: any) =>
		apiAxios
			.post(`ObtenerDocumentoPorTipoActividad`, data)
			.then((res: any) => res.data);

	const GetObtenerTiposDocumento = () =>
		apiAxios
			.get(`/ObtenerTiposDocumento`)
			.then((res: any) => res.data);

	const PostEliminarDocumentoActividadCredito = (data: any) =>
		apiAxios
			.post(`EliminarDocumentoActividadCredito`, data)
			.then((res: any) => res.data);

	const PostInsertarDocumentoCreditoPorActividad = (
		data: {
			taC_ID: number;
			tdC_ID: number;
			dcA_OBLIGATORIO: string;
			dcA_CARGA: string;
			dcA_VISUALIZA: string;
		},
	) =>
		apiAxios
			.post(
				'InsertarDocumentoCreditoPorActividad',
				data,
			)
			.then((res: any) => res.data);

	const postObtenerReporteTransUnion = (data: any) =>
		apiAxios
			.post(`ObtenerReporteTransUnion`, data)
			.then((res: any) => res.data);

	return {
		getProcessRequest,
		putRequestActivityApprove,
		putRequestActivityRejected,
		putRequestActivityUpdate,
		putRequestActivityRemedy,
		postCreditBasicData,
		postCreditProductData,
		postCreditObservations,
		postCreditFiles,
		postCreditFinancialData,
		postCreditCreateTechnicalData,
		postCreditTechnicalData,
		postCreditLegalData,
		postCreditCreateLegalData,
		postCreditClientSimulation,
		postCreditFlowDocuments,
		postCreditUploadDocument,
		postCreditCreateDocument,
		postCreditDownloadFile,
		postConsultCreditData,
		postCreditCreateClientSimulation,
		postUpdateRequestCreditRecommendation,
		postUpdateInsurance,
		postObtainPDFResume,
		postCreditCommittee,
		postCreditCommitteeSoli,
		getMembersCommittee,
		getTopicsCommittee,
		postGetMembersCommit,
		postGetTopicsCommit,
		postCreateCommitteeTopics,
		postCreateCommittee,
		postCreateCommitteeMembers,
		postCreditCommitteeRequest,
		postCreditCommitteeApprove,
		postCreateCreditCommitteeRequest,
		postFinancialData,
		getResourcesSource,
		postUploadDisbursementRates,
		getInsurances,
		postDesembolso,
		getCreditFlowDocuments,
		postCreditDownloadFileFiRiesgo,
		postCreditDownloadFileS,
		getPrueba,
		postLegalizacion,
		postComitePdf,
		postLegalizacionFirmas,
		PostVerificacionTerc,
		GetLegalizacionData,
		postCreditDownloadFileLeasing,
		patchDesembolso,
		getCausalDesistimiento,
		PostDesistminetoDesembolso,
		getFuentes,
		deleteFuente,
		postObtenerHistorialComercialBuro,
		postValidarBiometriaFenix,
		getValidacionTusDatos,
		getPollingTusDatos,
		validarTusDatosConPolling,
		getValidacionActualizacionVigia,
		postObtenerPreaprobaciones,
		getObtenerSimulacionPersonales,
		getObtenerDatosFinancierosPersonales,
		getConsultarSimulacionCliente,
		postCierreMensualCartera,
		postCierreDiarioCartera,
		postCausacionDiariaInteresesCorrientes,
		getObtenerTiposDeCierreCartera,
		ObtenerCierresCausacionesRealizadas,
		insertarProcesoAplicacionPago,
		actualizarProcesoAplicacionPago,
		obtenerProcesoAplicacionPagos,
		obtenerProcesoAplicacionPagosPorUnidadEjecutora,
		aplicarPagoMasivoDesdeLista,
		eliminarProcesoAplicacionPagos,
		getDescargarReporteAltasBajas,
		getObtenerDatosSimulacionParaEstudio,
		postCargarDocumentosSolicitudCredito,
		postCierreAnualCartera,
		getObtenerCausalesRechazo,
		getObtenerEstadosTramite,
		getObtenerTramitesPendientes,
		postObtenerMatriculasCreditoPorSolicitud,
		postCrearMatriculaCredito,
		updateMatriculaCredito,
		deleteMatriculaCredito,
		posDatosUsuario,
		postObtenerTramiteSolicitudCliente,
		postCrearEscritura,
		postObtenerEscritura,
		postActualizarEscritura,
		posCrearTerceroSeven,
		posCrearCDPSeven,
		posCrearProveedorSeven,
		posCrearFacturaProveSeven,
		getObtenerValorDesembolsoCredito,
		posPagoCuotaInicalSeven,
		postObtenerSolicitudCreditoJuridica,
		postCrearInvitadoCmite,
		getObtnerInvitadosComite,
		postCrearSolicitudJuridicaInmueble,
		postCrearAseguradoras,
		getObtenerAseguradoras,
		putAseguradorasActulizar,
		getObtenerTipoActividad,
		putRequestActivityBack,
		putRequestActivityDisitir,
		getObtenerSolicitudCreditoTipoNumeroIdentificacion,
		postProspeccionTransUnion,
		postReporteInformacionComercial,
		postReporteInformacionComercialPropiedades,
		ObtenerProductoByIdentificacionRequest,
		postCrearCDPSeven,
		postCrearSolicitudCredito,
		DesmbolsoCreditoMomento1,
		DesmbolsoCreditoMomento2,
		postCrearTerceroSevenCredito,
		postCrearProveedorSeven,
		postCrearCDPPrimerPago,
		postCrearCDPSegundoPago,
		postCrearFacturaSevenPrimerPago,
		postCrearFacturaSevenSegundoPago,
		getObtenerDatosSimulacion,
		postCreaSCnMcontAprobacionCredito,
		postObtenerValoresProyectados,
		postObtenerVendedor,
		postCrearVendedor,
		deleteEliminarVendedor,
		ReporteGeneralHaberes,
		postActualizaTramitePerito,
		postVerArchivos,
		postDocumentosActSimulacion,
		postObtenerValorCdp,
		postObtenerValorFactura,
		postActualizaDocumentoLegalizacion,
		postObtenerComprobantesCredito,
		postGuardarCdp,
		postConsultarNotificacionesSolicitudesCredito,
		PostConsultarAprobacionAsesorCredito,
		postObtenerPesoYTalla,
		postInsertarCdp,
		postObteberExcelCredito,
		getConsultarFacturaSeven,
		getObtenertipoactividadporid,
		postRegistrarBloqueoCuentaCredito,
		getObtenerSalarioMinimoAnual,
		getConsultarAliadosEstrategicos,
		insertarActualizarAvaluoComercialCredito,
		getConsultarAvaluoComercialCredito,
		getConsultaValoresSimulacionCreditoReclasificiacion,
		getConsultarValidacionCreditoProspeccion,
		postInsertarActualizarValidacionCreditoProspeccion,
		PostCargarDocumentosActividadCreditoDODO,
		getconsultarusuariocredito,
		postReportesSolicitudCreditoGeneralUsuarioExterno,
		PostObtenerDocumentoPorTipoActividad,
		GetObtenerTiposDocumento,
		PostEliminarDocumentoActividadCredito,
		PostInsertarDocumentoCreditoPorActividad,
		postObtenerReporteTransUnion,
		patchActualizarPersonalesVivienda,
		postCrearSimulacionDatosFinancieros,
	};
};

export default CreditApi;