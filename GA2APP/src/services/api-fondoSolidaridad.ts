import axios, { AxiosRequestConfig } from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { Constructora } from '../pages/fondoSolidaridad/constants/fondosolidaridadform.model.type';

const ApiFondoSolidaridad = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}`,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const postCrearConvocatoria = (convocatoria: any) =>
        apiAxios
            .post(`convocatoria/CrearConvocatoria`, convocatoria,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarConvocatoria = (convocatoria: any) =>
        apiAxios
            .post(`convocatoria/UpdateDataConvocatoria`, convocatoria,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getObtenerConvocatorias = (busqueda: any) =>
        apiAxios
            .get(`convocatoria/obtenerConvocatoria?NoConvocatoria=${busqueda.numeroConvocatoria}&EstadoConvocatoriaId=${busqueda.estado}&FechaApertura=${busqueda.fechaDesde}&FechaCierre=${busqueda.fechaHasta}&FechaPublicacion=${busqueda.fechaPublicacion}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerEstadosConvocatoria = () =>
        apiAxios
            .get(`estadoConvocatoria/obtenerEstadoConvocatoria`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerListaNumerosConvocatorias = () =>
        apiAxios
            .get(`Convocatoria/ConsultarConvocatoriasId`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerIdConvocatoria = () =>
        apiAxios
            .get(`Convocatoria/ConsultarConvocatoriasId`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postCierreConvocatoria = (request: any) =>
        apiAxios
            .post(`convocatoria/CrearCierreConvocatoria`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarCierreConvocatoria = (request: any) =>
        apiAxios
            .post(`convocatoria/UpdateFechaCierre`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getObtenerNumeroConvocatoriaProximaParaCrear = () =>
        apiAxios
            .get(`Convocatoria/obtenetIdConvocatoria`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postEliminarConvocatoria = (request: any) =>
        apiAxios
            .post(`convocatoria/EliminarConvocatoria`, request, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarTipoSolicitudPostulante = () =>
        apiAxios
            .get(`postulante/ConsultarTipoSolicitudPostulante`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarTipoModalidadPostulante = (id: number) =>
        apiAxios
            .get(`postulante/ConsultarTipoModaliadPostulante?id=${id}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarTipoSubModalidadPostulante = (id: number) =>
        apiAxios
            .get(`postulante/ConsultarTipoSubModaliadPostulante?id=${id}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postCrearSolicitudPostulante = (informacionPostulante: any) =>
        apiAxios
            .post(`postulante/crearSolicitdPostulante`, informacionPostulante,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postValidarExistePostulacion = (identificacion: number) =>
        apiAxios
            .post(`postulante/validarSolicitud?id=${identificacion}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postValidarExistePostulacionPorId = (idSolicitud: string) =>
        apiAxios
            .post(`postulante/ObtenerInformcacionPostulacion?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarSolicitudPostulante = (informacionPostulante: any) =>
        apiAxios
            .post(`postulante/updateSolicitudPostulacion`, informacionPostulante,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAprobarSolicitudPostulante = (idSolicitud: string) =>
        apiAxios
            .post(`postulante/aprovedSolicitud?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarSolicitudPostulante = (idSolicitud: string) =>
        apiAxios
            .post(`postulante/declineSolicitud?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postObtenerListaArchivosPorModalidad = (request: any) =>
        apiAxios
            .post(`postulante/listaArchivoPostulante`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCargarDocumentosPostulacion = (data: FormData, idSolicitud: string | undefined) =>
        apiAxios
            .post(`postulante/cargarDocumentosPostulacion/${idSolicitud}`, data)
            .then((res: any) => res.data);

    const getDescargarArchivoSolicitudPostulante = async (rutaStorage: string) => {
        const datos = await apiAxios
            .get(`postulante/DescargarArchivoPorSolicitud?rutaStorage=${rutaStorage}`, {
                responseType: 'blob'
            })
            .then((res: any) => res.data);
        return datos;
    }

    const postEliminarArchivoPostulante = (request: any) =>
        apiAxios
            .post(`postulante/EliminarArchivoPorSolicitud`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarArchivoPorSolicitudPostulante = (idSolicitud: string) =>
        apiAxios
            .get(`postulante/ConsultarArchivoFsPorSolicitud?IdSolicitud=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAprobarCargueDocumentos = (request: any) =>
        apiAxios
            .post(`postulante/aprovarCargeDocumento`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarCargueDocumentos = (idSolicitud: string) =>
        apiAxios
            .post(`postulante/RechazarCargeDocumento?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAnularCargueDocumentos = (idSolicitud: string) =>
        apiAxios
            .post(`postulante/anularCagueDocumento?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAnularVerificacionEmbargos = (request: any) =>
        apiAxios
            .post(`postulante/AnularVerificateEmbargosPostulacion`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCrearEntrevista = (request: any) =>
        apiAxios
            .post(`/postulante/crearEntrevistas`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarEntrevista = (idSolicitud: string) =>
        apiAxios
            .get(`postulante/consultarEntrevistaById?IdSolicitud=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postActualizarEntrevista = (request: any) =>
        apiAxios
            .post(`/postulante/updateEntrevistas`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAprobarEntrevista = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/aprovarEntrevista?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAnularEntrevista = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/anularEntrevista?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarEntrevista = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/rechazarEntrevista?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarCalificacion = (request: any) =>
        apiAxios
            .post(`/postulante/actulizarCalificacion`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarCalificacion = (idSolicitud: string) =>
        apiAxios
            .get(`/postulante/ConsultarCalificacionFsPorSolicitud?IdSolicitud=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAprobarCalificacion = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/aprobarCalificacion?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarCalificacion = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/rechazarCalificacion?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAnularCalificacion = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/anularCalificacion?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarObservacionCargueDocumentos = (request: any) =>
        apiAxios
            .post(`/postulante/updateObservacionArchivo`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarObservacionCargueArchivo = (idSolicitud: string) =>
        apiAxios
            .get(`/postulante/consultarObservacionById?request=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarSolicitudes = (request: any | null) =>
        apiAxios
            .get(`/postulante/consultarSolicutdPostulacion?EtapasId=${request.idEtapa}&Identificacion=${request.identificacion}&TipoSolicitudId=${request.tipoModalidad}&NoConvocatoria=${request.numeroConvocatoria}&NoRadicado=${request.radicado}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarActividad = () =>
        apiAxios
            .get(`/postulante/consultarEtapaFs`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarListaPostulantesConvocatoria = (idConvocatoria: string) =>
        apiAxios
            .get(`/Convocatoria/ConsultarPostulanteConvocatoria?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarListaPostulantesConvocatoriaPorPuntaje = (idConvocatoria: string) =>
        apiAxios
            .get(`/Convocatoria/ConsultarPostulanteConvocatoriaOrdenByPuntaje?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarListaPostulantesRechazados = (idConvocatoria: string) =>
        apiAxios
            .get(`/Convocatoria/ConvocatoriaRechazados?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarListaPostulantesSinCupo = (idConvocatoria: string) =>
        apiAxios
            .get(`/Convocatoria/ConvocatoriaSinCupo?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarCierreConvocatoria = (idConvocatoria: string) =>
        apiAxios
            .get(`Convocatoria/GetDataCierreConvocatoria?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const reporteAfiliadosCalificados = (idConvocatoria: string) =>
        apiAxios
            .get(`convocatoria/ReportePostulanteCalificado?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getListaConvocatorias = () =>
        apiAxios
            .get(`Convocatoria/ConsultarListConvocatoriasId`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerTiposIntegrantes = () =>
        apiAxios
            .get(`postulante/ObtenerTipoIntegrante`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerTiposDiscapacidad = () =>
        apiAxios
            .get(`postulante/ObtenerTipoDiscapacidaPostulacion`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerTiposAyudaTecnica = () =>
        apiAxios
            .get(`postulante/ObtenerTipoAyudaTecnica`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerTipoParentesco = () =>
        apiAxios
            .get(`postulante/ObtenerTipoParentescoPostulacion`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postActualizarVerificacion = (request: any) =>
        apiAxios
            .post(`/postulante/UpdateVerificatePostulacion`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAprobarVerificacionPostulacion = (request: any) =>
        apiAxios
            .post(`/postulante/AprovedVerificatePostulacion`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarVerificacion = (request: any) =>
        apiAxios
            .post(`/postulante/DeclineVerificatedPostulation?request=${request}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarVerificacionEmbargos = (request: any) =>
        apiAxios
            .post(`/postulante/RechazarVerificateEmbargosPostulacion`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAprobarVerificacion = (request: any) =>
        apiAxios
            .post(`/postulante/AprovedVerificatePostulacion?request=${request}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postConsultarVerificacion = (idSolicitud: any) =>
        apiAxios
            .post(`/postulante/GetDataVerificatePostulacion?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getObtenerInformacionTrazabilidad = (idSolicitud: string) =>
        apiAxios
            .get(`/postulante/GetStagePostulation?id=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAprobarVerificacionNovedades = (request: any) =>
        apiAxios
            .post(`/postulante/AprovedVerificateNovedadesFs`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAprobarVerificacionEmbargo = (request: any) =>
        apiAxios
            .post(`/postulante/AprovedVerificateEmbargo`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAprobarVerificacionCuentasIndividuales = (request: any) =>
        apiAxios
            .post(`/postulante/AprovedVerificateCuentaIndivuales`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAnularVerificacionNovedades = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/AnularVericatedPostulacion?request=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAnularVerificacionCuentasIndividuales = (request: any) =>
        apiAxios
            .post(`/postulante/AnularVerificationCuentaIndividuales`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarVerificacionCuentasIndividuales = (request: any) =>
        apiAxios
            .post(`/postulante/RechazarVerificateCuentaIndividuales`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarNumeroFoliosCargueArchivos = (request: any) =>
        apiAxios
            .post(`/postulante/ActualizarFolio`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getGenerarInformacionRanking = (idConvocatoria: number) =>
        apiAxios
            .get(`/postulante/GetReportRanking?idConvocatoria=${idConvocatoria}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postConsultarObservacionVerificacion = (idSolicitud: string) =>
        apiAxios
            .post(`/postulante/GetDataVerificatePostulacion?id=${idSolicitud}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAgregarConstructora = (constructora: Constructora) =>
        apiAxios
            .post(`/Constructora/AgregarConstructora`, constructora,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarConstructoras = () =>
        apiAxios
            .get(`/Constructora/ConsultarConstructora`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postActualizarConstructora = (constructora: Constructora) =>
        apiAxios
            .post(`/Constructora/ActualizarConstructora`, constructora,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postEliminarConstructora = (idConstructora: string) =>
        apiAxios
            .post(`/Constructora/EliminarConstructoraById?IdConstructora=${idConstructora}`, {},
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarFoliosDodo = (request: any) =>
        apiAxios
            .post(`/postulante/ActualizarRadicadoPostulacion`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postRechazarEtapa = (request: any) =>
        apiAxios
            .post(`/postulante/UpdateRequestStageDto`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);


    const postAnularEtapa = (request: any) =>
        apiAxios
            .post(`/postulante/ActualizarAnuladoEtapaFs`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCrearProyecto = (proyecto: any) =>
        apiAxios
            .post(`/ProyectoConstructora/CrearProyecto`, proyecto,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCrearPolizasProyecto = (ListaPolizas: any[]) =>
        apiAxios
            .post(`/ProyectoConstructora/CrearPoliza`, ListaPolizas,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCrearInmueblesProyecto = (inmuebles: any) =>
        apiAxios
            .post(`/ProyectoConstructora/CrearInmueble`, inmuebles,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarEstadoProyecto = (request: any) =>
        apiAxios
            .post(`/ProyectoConstructora/ActualizarEstadoProyecto`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarProyectos = (busqueda: any) =>
        apiAxios
            .get(`/ProyectoConstructora/obtenerProyecto?ProyectoId=${busqueda.proyectoId}&IdDepartamento=${busqueda.idDepartamento}&IdCiudad=${busqueda.idCiudad}&IdEstado=${busqueda.idEstado}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarProyectoPorId = (idProyecto: string) =>
        apiAxios
            .get(`/ProyectoConstructora/obtenerProyectoById?id=${idProyecto}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarTiposInmuebles = () =>
        apiAxios
            .get(`/ProyectoConstructora/obtenerTipoInmuebles`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getConsultarTiposVivienda = (idTipoInmueble: number) =>
        apiAxios
            .get(`/ProyectoConstructora/ObtenerTipoVivienda?codigo=${idTipoInmueble}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postGuardarInformacionSupervisionTecnica = (request: any) =>
        apiAxios
            .post(`/ProyectoConstructora/GuardarInformacionSupervicionTecnica`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarSupervisionTecnica = (idProyecto: string) =>
        apiAxios
            .get(`ProyectoConstructora/ObtenerGuardaInformacionSupervisorTecnico?Id=${idProyecto}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    // const postGenerarExcelRanking = async (idConvocatoria: number, config?: AxiosRequestConfig) =>
    // apiAxios
    //     .post(`Reportes/ReportesGeneralRanking?idConvocatoria=${idConvocatoria}`, {}, {
    //         responseType: 'blob', 
    //     })
    //     .then(res => res.data)
    //     .catch((res) => {
    // })
    const postGenerarExcelRanking = async (
        idConvocatoria: number,
        config: AxiosRequestConfig = {}
    ): Promise<Blob> =>
        apiAxios
            .post(
                `Reportes/ReportesGeneralRanking?idConvocatoria=${idConvocatoria}`,
                {},
                { ...config, responseType: 'blob' }
            )
            .then(res => res.data)
            .catch((err) => {
                throw err;
            });


    const getConsultarBeneficairiosPostulantes = (idSolicitud: string) =>
        apiAxios
            .get(`postulante/ObtenerBeneficiarioPostulacion?IdSolicitud=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postGuardarInconsistenciaSolicitud = (request: any) =>
        apiAxios
            .post(`postulante/GuardarIncosistenciaSolicitudFondo`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postReportesFondoSolidaridad = (request: any) =>
        apiAxios
            .post(`Reportes/ReporteFondoSolidaridad`, request,
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getConsultarInconsistenciaSolicitud = (idSolicitud: string) =>
        apiAxios
            .get(`postulante/ObtenerIncosistenciaSolicitudFondo?IdSolicitud=${idSolicitud}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    return {
        postCrearConvocatoria,
        getObtenerConvocatorias,
        getObtenerEstadosConvocatoria,
        getObtenerListaNumerosConvocatorias,
        getObtenerNumeroConvocatoriaProximaParaCrear,
        postEliminarConvocatoria,
        postCierreConvocatoria,
        getConsultarTipoSolicitudPostulante,
        getConsultarTipoModalidadPostulante,
        getConsultarTipoSubModalidadPostulante,
        postCrearSolicitudPostulante,
        postValidarExistePostulacion,
        postActualizarSolicitudPostulante,
        postAprobarSolicitudPostulante,
        postRechazarSolicitudPostulante,
        postObtenerListaArchivosPorModalidad,
        postCargarDocumentosPostulacion,
        getDescargarArchivoSolicitudPostulante,
        postEliminarArchivoPostulante,
        getConsultarArchivoPorSolicitudPostulante,
        postAprobarCargueDocumentos,
        postRechazarCargueDocumentos,
        postAnularCargueDocumentos,
        postCrearEntrevista,
        getConsultarEntrevista,
        postActualizarEntrevista,
        postAprobarEntrevista,
        postAnularEntrevista,
        postRechazarEntrevista,
        postActualizarCalificacion,
        getConsultarCalificacion,
        postAprobarCalificacion,
        postRechazarCalificacion,
        postAnularCalificacion,
        postActualizarObservacionCargueDocumentos,
        getConsultarObservacionCargueArchivo,
        getConsultarSolicitudes,
        getConsultarActividad,
        getConsultarListaPostulantesConvocatoria,
        getConsultarListaPostulantesConvocatoriaPorPuntaje,
        getConsultarListaPostulantesRechazados,
        getConsultarListaPostulantesSinCupo,
        getConsultarCierreConvocatoria,
        reporteAfiliadosCalificados,
        getObtenerTiposIntegrantes,
        getObtenerTiposDiscapacidad,
        getObtenerTiposAyudaTecnica,
        getObtenerTipoParentesco,
        postActualizarVerificacion,
        postAprobarVerificacionPostulacion,
        postRechazarVerificacion,
        postAprobarVerificacion,
        postConsultarVerificacion,
        postActualizarConvocatoria,
        postActualizarCierreConvocatoria,
        getObtenerInformacionTrazabilidad,
        postAprobarVerificacionNovedades,
        postAprobarVerificacionEmbargo,
        postAprobarVerificacionCuentasIndividuales,
        postAnularVerificacionNovedades,
        postAnularVerificacionEmbargos,
        postAnularVerificacionCuentasIndividuales,
        postRechazarVerificacionEmbargos,
        postRechazarVerificacionCuentasIndividuales,
        postActualizarNumeroFoliosCargueArchivos,
        getGenerarInformacionRanking,
        postConsultarObservacionVerificacion,
        getObtenerIdConvocatoria,
        getListaConvocatorias,
        postAgregarConstructora,
        getConsultarConstructoras,
        postActualizarConstructora,
        postEliminarConstructora,
        postActualizarFoliosDodo,
        postRechazarEtapa,
        postCrearProyecto,
        postCrearPolizasProyecto,
        postCrearInmueblesProyecto,
        postGuardarInformacionSupervisionTecnica,
        getConsultarSupervisionTecnica,
        postActualizarEstadoProyecto,
        postAnularEtapa,
        getConsultarProyectos,
        getConsultarProyectoPorId,
        postGenerarExcelRanking,
        getConsultarTiposInmuebles,
        getConsultarTiposVivienda,
        postValidarExistePostulacionPorId,
        getConsultarBeneficairiosPostulantes,
        postGuardarInconsistenciaSolicitud,
        getConsultarInconsistenciaSolicitud,
        postReportesFondoSolidaridad
    };
};

export default ApiFondoSolidaridad;
