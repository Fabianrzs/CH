import React, { useEffect, useState, useRef } from 'react';
import { createStyles, Tab, Tabs, Theme, withStyles } from '@material-ui/core';
import { useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import moment from 'moment';
import ResumenSolicitud from '../components/resumen-solicitud/resumen-solicitud.component';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import BotoneraInformacionSolicitudComponent from '../components/botonera/botonera-informacion-solicitud-component';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import NotificationService from '../../../services/notification.service';
import SolicitudApi from '../../../services/api-solicitud.service';
import CargueDocumentosSolicitud from '../components/cargue-documentos-solicitud/cargue-documentos-solicitud.component';
import { DocumentosPorSubModalidadModel } from '../../../models/documentos-por-submodalidad.model';
import VerDocumentosSolicitud from '../components/cargue-documentos-solicitud/ver-documentos-solicitud.component';
import FormularioInconsistenciasGrupo1 from '../components/formulario-inconsistencias/formulario-inconsistencias-grupo1.component';
import GenerarComunicaciones from '../components/generar-comunicaciones/generar-comunicaciones.component';
import { BOTONERA_SOLICITUD, MODALIDAD_SOLICITUD } from '../../../constants/constants';
import { ComunicacionSolicitud, FormularioInconsistenciasDto, FormulariosSolicitudDto } from '../../../models/requests/solicitud-tarea.model';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ObservacionesSolicitud from '../components/observaciones/observaciones';
import { UserState } from '../../../reducers/user.reducer';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import TrazabilidadSolicitud from '../components/trazabilidad/trazabilidad.component';
import DynamicPage from '../dynamicPage/DynamicPage.component';
import HistorialInconsistenciaSolicitud from '../components/historialIconsistenciaSolicitud/HistorialInconsistenciaSolicitud';
import AuthService from '../../../services/auth.service';
import ApiInconsistencias from "../../../services/api-inconsistencia.service";
import EntidadesEducativasApi from '../../../services/api-entidadesEducativas.service';

const api = SolicitudApi();

const TABS_INDEX = {
    RESUMENSOLICITUD: 0,
    CARGUEDOCUMENTOS: 1,
    CARGAARCHIVOS: 2,
    VERARCHIVOS: 3,
    INCONSISTENCIAS: 4,
    COMUNICACIONES: 5,
    VERDETALLESOLICITUD: 6,
    HISTORIALINCONSISTENCIA: 7,
    VERIFICACIONCUENTAINDIVIDUAL: 10,
    VERIFICACIONESDECUENTA: 11
};

const apiEntidadesEducativas = EntidadesEducativasApi();


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
    grupo1,
    userAsig,
    userIngreso,
}: {
    currentTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
    grupo1: boolean;
    userAsig: string | undefined;
    userIngreso: string;
}) {

    const tabsToShow = [
        { label: "Resumen", value: TABS_INDEX.RESUMENSOLICITUD },
        { label: "Cargue de documentos", value: TABS_INDEX.CARGUEDOCUMENTOS },
        { label: `Seguridad documental ${grupo1 ? 'Grupo 1' : 'Grupo 2'}`, value: TABS_INDEX.INCONSISTENCIAS },
        { label: "Generar comunicaciones", value: TABS_INDEX.COMUNICACIONES },
        { label: "Detalle Solicitud", value: TABS_INDEX.VERDETALLESOLICITUD },
        { label: "Historial Inconsistencia", value: TABS_INDEX.HISTORIALINCONSISTENCIA }
    ];

    const filteredTabs = tabsToShow.filter(tab => {
        if (userAsig !== userIngreso) {
            return tab.value === TABS_INDEX.RESUMENSOLICITUD ||
                tab.value === TABS_INDEX.CARGUEDOCUMENTOS ||
                tab.value === TABS_INDEX.VERDETALLESOLICITUD;
        }

        if (tab.value === TABS_INDEX.INCONSISTENCIAS || tab.value === TABS_INDEX.COMUNICACIONES) {
            return userAsig === userIngreso;
        }

        return true;
    });

    return (

        <AntTabs
            value={currentTab}
            onChange={onTabChange}
            aria-label="ant example"
        >
            {filteredTabs.map(tab => (
                <AntTab key={tab.value} label={tab.label} value={tab.value} />
            ))}
        </AntTabs>

    );
}

function ViewTabsCargaArchivos({

    indexTab,
    onTabChange,
}: {
    indexTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
}) {
    return (
        <AntTabs
            value={indexTab}
            onChange={onTabChange}
            aria-label="ant example"
        >
            <AntTab label="Carga Archivos" value={TABS_INDEX.CARGAARCHIVOS} />


        </AntTabs>
    );
}

function Panel({
    currentTabValue,
    responseSolicitud,
    control,
    grupo1,
    setValue,
    getValues,
    documentosAgregados,
    setEstadoSubsanar,
    handleSubmit,
    reset,
    watch,
    listadoInconsistencias,
    addInconsistencia,
    updateInconsistencias,
    deleteInconsistencias,
    formularioObservaciones,
    observaciones,
    formulariosSolicitud,
    siEsAprobadaSolcitud,
    inconsistenciasSeleccionadas,
    deshablitarNinguna,
    desHabilitarNingunaGrupo1,
    setDeshabilitarNinguna,
    setCausalYNovedades,
    causalYNovedades,
    setMensajesEjecutados,
    mensajesEjecutados,
    quitarTiposNovedades,
    setQuitarTiposNovedades,
    setTraerIdAntrior,
    caulsaSeleccionadas,
    setResetNovedad,
    resetNovedad,
    setTraerTodaData,
    estadoSubsanar,
    mostrarCheck,
    llamarNovedades,
    setLlamarNovedades,
    setMostrarCheck,
    deshativarBtnEliminarGp2,
    trazabilidad,
    refresh,
    formularioInformacionComunicacion,
    mostrarObservacionesSoloLecturaVerificacion,
    entidadBloqueadaControlPreventivo,
    validandoBloqueoEntidad,
    setOmitirAlertaCausalTemporalmente,
    matriculaBloqueadaActiva
}: {
    documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
    currentTabValue: number;
    control: any;
    getValues: any;
    responseSolicitud: ResponseSeveranceModel | undefined;
    grupo1: boolean;
    setValue: any;
    setEstadoSubsanar: (value: boolean) => void;
    handleSubmit: any;
    reset: any;
    watch: any;
    listadoInconsistencias: FormularioInconsistenciasDto[];
    addInconsistencia: any;
    updateInconsistencias: any;
    deleteInconsistencias: any;
    formularioObservaciones: any
    observaciones: string;
    formulariosSolicitud: FormulariosSolicitudDto | any;
    siEsAprobadaSolcitud: boolean;
    inconsistenciasSeleccionadas: any;
    deshablitarNinguna: any;
    desHabilitarNingunaGrupo1: boolean;
    setDeshabilitarNinguna: (novedad: boolean) => void;
    setCausalYNovedades: (causalNovedades: boolean) => void;
    causalYNovedades: any;
    setMensajesEjecutados: (valor: any) => void;
    mensajesEjecutados: boolean;
    quitarTiposNovedades: boolean;
    setQuitarTiposNovedades: (valu: any) => void;
    setTraerIdAntrior: (idAnterior: any) => void;
    caulsaSeleccionadas: any;
    setResetNovedad: (reset: any) => void;
    resetNovedad: any
    setTraerTodaData: (data: any) => void;
    estadoSubsanar: any;
    mostrarCheck: any;
    llamarNovedades: any;
    setLlamarNovedades: any;
    setMostrarCheck: any;
    deshativarBtnEliminarGp2: any;
    trazabilidad: any[];
    refresh: boolean;
    formularioInformacionComunicacion: UseFormReturn<any>;
    mostrarObservacionesSoloLecturaVerificacion: boolean;
    entidadBloqueadaControlPreventivo: boolean;
    validandoBloqueoEntidad: boolean;
    setOmitirAlertaCausalTemporalmente: (
        value: boolean,
    ) => void;
    matriculaBloqueadaActiva: boolean;
}) {

    const apiInconsistencias = ApiInconsistencias();



    useEffect(() => {

        if (!mensajesEjecutados && currentTabValue === 4) {
            mensajesBloqueos(responseSolicitud?.solicitudId || '');
            setMensajesEjecutados(true)

        } else {
            setMensajesEjecutados(false)
        }
    }, [currentTabValue, responseSolicitud]);



    const mensajesBloqueos = async (
        solicitudId: string,
    ): Promise<void> => {
        try {
            const response =
                await apiInconsistencias.GetInconsistencias(
                    solicitudId,
                );

            if (!response) {
                toast(
                    'Error consultando los bloqueos.',
                    { type: toast.TYPE.ERROR },
                );
                return;
            }

            const inconsistencias = Array.isArray(
                response?.data,
            )
                ? response.data
                : [];

            const bloqueoManualOAutomatico =
                inconsistencias.find(
                    (item: any) => {
                        const nombreReglaNormalizado =
                            String(
                                item?.nombrE_REGLA || '',
                            )
                                .trim()
                                .toUpperCase();

                        const esControlPreventivo =
                            nombreReglaNormalizado.includes(
                                'CONTROL PREVENTIVO SOLICITUDES',
                            );

                        if (esControlPreventivo) {
                            return false;
                        }

                        return (
                            nombreReglaNormalizado.includes(
                                'BLOQUEO AUTOMATICO',
                            ) ||
                            nombreReglaNormalizado.includes(
                                'BLOQUEO MANUAL',
                            )
                        );
                    },
                );

            if (!bloqueoManualOAutomatico) {
                return;
            }

            const nombreRegla = String(
                bloqueoManualOAutomatico
                    ?.nombrE_REGLA || '',
            ).trim();

            const nombreReglaNormalizado =
                nombreRegla.toUpperCase();

            const esBloqueoAutomatico =
                nombreReglaNormalizado.includes(
                    'BLOQUEO AUTOMATICO',
                );

            const esBloqueoManual =
                nombreReglaNormalizado.includes(
                    'BLOQUEO MANUAL',
                );

            if (
                !esBloqueoAutomatico &&
                !esBloqueoManual
            ) {
                return;
            }

            if (esBloqueoAutomatico) {
                const matriculas = Array.isArray(
                    responseSolicitud
                        ?.solicitudMatriculaDto,
                )
                    ? responseSolicitud
                        ?.solicitudMatriculaDto
                    : [];

                const siMatriculaBloqueada =
                    matriculas?.some(
                        (item: any) => {
                            const valorBloqueo =
                                String(
                                    item?.bloqueo ?? '',
                                )
                                    .trim()
                                    .toLowerCase();

                            const bloqueoActivo =
                                item?.bloqueo === true ||
                                valorBloqueo === 'true' ||
                                valorBloqueo === '1' ||
                                valorBloqueo === 'si' ||
                                valorBloqueo === 'sí';

                            const tipoBloqueo =
                                String(
                                    item?.tipo_bloqueo ??
                                    item?.tipoBloqueo ??
                                    '',
                                ).trim();

                            return (
                                bloqueoActivo ||
                                tipoBloqueo === '1'
                            );
                        },
                    );

                /*
                 * La inconsistencia puede permanecer en el
                 * historial, pero la alerta no debe mostrarse
                 * cuando la matrícula ya está desbloqueada.
                 */
                if (!siMatriculaBloqueada) {
                    return;
                }
            }

            const titulo = esBloqueoAutomatico
                ? 'Bloqueo Automático'
                : 'Bloqueo Manual';

            const partesRegla =
                nombreRegla.split(':');

            const mensaje =
                partesRegla.length > 1
                    ? partesRegla
                        .slice(1)
                        .join(':')
                        .trim()
                    : nombreRegla;

            await NotificationService.genericAlwarning(
                titulo,
                mensaje,
                'warning',
            );
        } catch (error) {
            console.error(
                'Error consultando mensajes de bloqueo:',
                error,
            );

            toast(
                'Error consultando los mensajes del proceso de seguridad documental.',
                { type: toast.TYPE.ERROR },
            );
        }
    };

    const [indexTabValue, setTndexTabValue] = useState(
        TABS_INDEX.CARGAARCHIVOS,
    );

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {

        setTndexTabValue(newValue);
    };

    const normalizarTextoPanel = (texto: any) =>
        String(texto || '')
            .trim()
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '');

    const tareaPendienteTrazabilidad = trazabilidad?.find((item: any) =>
        normalizarTextoPanel(item?.descripcionEstadoSolicitud) === 'pendiente'
    );

    const estaEnVerificacionPorTrazabilidad =
        normalizarTextoPanel(tareaPendienteTrazabilidad?.descripcionTarea)
            .startsWith('verificacion');

    const mostrarSoloObservacionesVerificacion =
        mostrarObservacionesSoloLecturaVerificacion || estaEnVerificacionPorTrazabilidad;


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
                    <ViewTabsCargaArchivos
                        indexTab={indexTabValue}
                        onTabChange={handleTabChange}
                    />
                    <PanelCargarArchivo
                        currentTabValue={indexTabValue}
                        responseSolicitud={responseSolicitud}
                        documentosAgregados={documentosAgregados}
                        siEsAprobadaSolcitud={siEsAprobadaSolcitud}
                    />
                    <ObservacionesSolicitud
                        control={formularioObservaciones}
                        seccion={String(currentTabValue)}
                        trazabilidad={trazabilidad}
                        soloLecturaVerificacion={mostrarSoloObservacionesVerificacion}
                    />
                </>
            );

        case TABS_INDEX.INCONSISTENCIAS:

            const nombreTareaActual = String(responseSolicitud?.nombreTarea || '');

            const mostrarObservacionGrupo1EnGrupo2 =
                nombreTareaActual.includes('Seguridad documental grupo 2') ||
                nombreTareaActual.includes('Seguridad documental de grupo 2');

            if (mostrarSoloObservacionesVerificacion) {
                return (
                    <>
                        <ObservacionesSolicitud
                            control={formularioObservaciones}
                            seccion={String(currentTabValue)}
                            trazabilidad={trazabilidad}
                            mostrarObservacionGrupo1EnGrupo2={mostrarObservacionGrupo1EnGrupo2}
                            soloLecturaVerificacion


                        />
                    </>
                );
            }



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
                        entidadBloqueadaControlPreventivo={
                            entidadBloqueadaControlPreventivo
                        }
                        validandoBloqueoEntidad={
                            validandoBloqueoEntidad
                        }
                        inconsistenciasSeleccionadas={inconsistenciasSeleccionadas}
                        deshablitarNinguna={deshablitarNinguna}
                        setDeshabilitarNinguna={setDeshabilitarNinguna}
                        desHabilitarNingunaGrupo1={desHabilitarNingunaGrupo1}
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
                        matriculaBloqueadaActiva={
                            matriculaBloqueadaActiva
                        }
                    />
                    <ObservacionesSolicitud
                        control={formularioObservaciones}
                        seccion={String(currentTabValue)}
                        trazabilidad={trazabilidad}
                        // soloLecturaVerificacion={mostrarSoloObservacionesVerificacion}
                        mostrarObservacionGrupo1EnGrupo2={mostrarObservacionGrupo1EnGrupo2}
                        soloLecturaVerificacion={false}

                    />
                </>
            );
        case TABS_INDEX.COMUNICACIONES:
            return (
                <>
                    <GenerarComunicaciones
                        idSolicitud={responseSolicitud?.solicitudId || ''}
                        responseSolicitud={responseSolicitud}
                        formularioInformacionComunicacion={formularioInformacionComunicacion}
                        trazabilidad={trazabilidad}
                    />
                </>
            );
        case TABS_INDEX.VERDETALLESOLICITUD:
            const tipoAccion = (responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.VINUEVA || responseSolicitud?.timCodigo === MODALIDAD_SOLICITUD.CNVGR) ? 0 : 5;
            const { formularios } = formulariosSolicitud;
            return (
                <DynamicPage
                    dynamicTitle={{ topText: "", title: "", icon: "" }}
                    formsList={formularios}
                    withButtons={false}
                    responseSolicitud={responseSolicitud}
                    tipoAccion={tipoAccion}
                    tipoAccionDetalleProcesar={false}
                    modalidadSolicitud={responseSolicitud?.timCodigo as string}
                    onItemGoBack={() => { } }
                    onItemBack={() => { } }
                    obtnerBoleano={undefined}
                    inicioSolicitud={false}
                    siEsAprobadaSolcitud={siEsAprobadaSolcitud}
                                  
                    />
            );
        case TABS_INDEX.HISTORIALINCONSISTENCIA:
            return (
                <HistorialInconsistenciaSolicitud responseSolicitud={responseSolicitud} />
            );
        default:
            return null;
    }
}

function PanelCargarArchivo({
    documentosAgregados,
    currentTabValue,
    responseSolicitud,
    siEsAprobadaSolcitud,
}: {
    documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
    currentTabValue: number;
    responseSolicitud: ResponseSeveranceModel | undefined;
    siEsAprobadaSolcitud: boolean
}) {

    switch (currentTabValue) {
        case TABS_INDEX.CARGAARCHIVOS:
            return (
                <>
                    <CargueDocumentosSolicitud
                        responseSolicitud={responseSolicitud}
                        documentosAgregados={documentosAgregados}
                        siEsAprobadaSolcitud={siEsAprobadaSolcitud}
                    />
                </>
            );
        case TABS_INDEX.VERARCHIVOS:
            return (
                <>
                    <VerDocumentosSolicitud
                        responseSolicitud={responseSolicitud}
                        siEsAprobadaSolcitud={siEsAprobadaSolcitud}
                    />
                </>
            );
        default:
            return null;
    }
}

function ProcesoSeguridadDocumental({
    responseSolicitud,
    onItemGoBack,
    tipoAccion,
    tipoAccionDetalleProcesar,
    grupo1,
    userState,
    catalogState,
    formulariosSolicitud,
    obtnerBoleano,
    siEsAprobadaSolcitud,
    setMensajesEjecutados,
    mensajesEjecutados,
    datosCuentasBloqueos,
    userAsig,
    grupo,
    userIngreso

}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    onItemGoBack: () => void;
    tipoAccion: number;
    tipoAccionDetalleProcesar: boolean;
    grupo1: boolean;
    userState: UserState;
    catalogState: CatalogState;
    formulariosSolicitud: FormulariosSolicitudDto | any;
    obtnerBoleano: any | undefined;
    siEsAprobadaSolcitud: boolean;
    setMensajesEjecutados: (valor: any) => void;
    mensajesEjecutados: boolean;
    datosCuentasBloqueos: any;
    userAsig: string | undefined;
    grupo: any;
    userIngreso: string;
}) {

    const [currentTabValue, setCurrentTabValue] = useState(
        TABS_INDEX.RESUMENSOLICITUD,
    );
    const [primerCargue, setPrimerCargue] = useState(false);
    const [trazabilidad, setTrazabilidad] = useState<any[]>([]);
    const [observaciones, setObservaciones] = useState('');
    const rechazandoRef = useRef(false);
    const [
        matriculaBloqueadaActiva,
        setMatriculaBloqueadaActiva,
    ] = useState(false);


    const normalizarTexto = (texto: any) =>
        String(texto || '')
            .trim()
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '');

    const nombreTareaActual = normalizarTexto(responseSolicitud?.nombreTarea);

    const estaEnFlujoVerificacion =
        nombreTareaActual.startsWith('verificacion') ||
        nombreTareaActual.startsWith('verificaciones');



    const textoValido = (valor: any) =>
        valor !== null &&
        valor !== undefined &&
        String(valor).trim() !== '';

    const buscarUltimaObservacionPorGrupo = (data: any[], grupo: number) =>
        [...data]
            .reverse()
            .find((x: any) =>
                String(x?.descripcionTarea || '')
                    .toLowerCase()
                    .includes(`seguridad documental grupo ${grupo}`) &&
                textoValido(x?.observacion)
            );


    const obtenerTrazabilidad = async () => {
        await api.obtenerTrazabilidadSolicitud(responseSolicitud?.solicitudId)
            .then((response) => {
                const data = Array.isArray(response?.data) ? response.data : [];

                setTrazabilidad(data);

                const nombreTareaActual = normalizarTexto(responseSolicitud?.nombreTarea);

                const estaEnVerificacion =
                    nombreTareaActual.startsWith('verificacion') ||
                    nombreTareaActual.startsWith('verificaciones');

                const actividadGrupo1 = buscarUltimaObservacionPorGrupo(data, 1);
                const actividadGrupo2 = buscarUltimaObservacionPorGrupo(data, 2);

                formularioObservaciones.setValue(
                    'observacionesGrupo1',
                    actividadGrupo1?.observacion || '',
                    {
                        shouldDirty: false,
                    },
                );

                formularioObservaciones.setValue(
                    'observacionesGrupo2',
                    actividadGrupo2?.observacion || '',
                    {
                        shouldDirty: false,
                    },
                );

                if (estaEnVerificacion) {
                    formularioObservaciones.setValue(
                        'observaciones',
                        '',
                        {
                            shouldDirty: false,
                        },
                    );

                    return;
                }

                const observacionActualCampo =
                    formularioObservaciones.getValues('observaciones');

                if (nombreTareaActual.includes('grupo 1')) {
                    formularioObservaciones.setValue(
                        'observaciones',
                        actividadGrupo1?.observacion ||
                        observacionActualCampo ||
                        '',
                        {
                            shouldValidate: true,
                            shouldDirty: false,
                        },
                    );
                }

                if (nombreTareaActual.includes('grupo 2')) {
                    formularioObservaciones.setValue(
                        'observaciones',
                        actividadGrupo2?.observacion ||
                        observacionActualCampo ||
                        '',
                        {
                            shouldValidate: true,
                            shouldDirty: false,
                        },
                    );
                }
            });
    };


    const normalizarBoolean = (value: any): boolean => {
        if (value === true) return true;

        const valor = String(value ?? '')
            .trim()
            .toLowerCase();

        return valor === 'true' || valor === '1' || valor === 'si' || valor === 'sí';
    };

    const esNovedadSubsanableNoSubsanadaGrupo1O2 = (item: any): boolean => {
        const grupoInconsistencia = Number(item?.grupoInconsistencia);
        const novedad = normalizarTexto(item?.novedad);

        return (
            (grupoInconsistencia === 1 || grupoInconsistencia === 2) &&
            novedad === 'novedad subsanable' &&
            !normalizarBoolean(item?.subsanado)
        );
    };

    useEffect(() => {
        if (!responseSolicitud?.solicitudId) {
            return;
        }

        obtenerTrazabilidad();
    }, [
        responseSolicitud?.solicitudId,
        responseSolicitud?.solicitudTareaId,
        responseSolicitud?.nombreTarea,
    ]);

    useEffect(() => {
        setObservaciones(trazabilidad?.find(x => x.idTareaSolicitud === responseSolicitud?.solicitudTareaId)?.observacion);
    }, [trazabilidad])

    const [tipoAccionNumber, setAccionNumber] = useState(BOTONERA_SOLICITUD.ACTUALIZAR);
    const [aprobar, setAprobar] = useState(true);
    const [estadoSubsanar, setEstadoSubsanar] = useState(false);
    const [resetNovedad, setResetNovedad] = useState(false);
    const [hablitarActualiza, setHablitarActualiza] = useState(false);
    const [listadoInconsistencias, setListadoInconsistencias] = useState<FormularioInconsistenciasDto[]>([]);
    const [documentosAguardar, setDocumentosAguardar] = useState<
        DocumentosPorSubModalidadModel[]
    >([]);
    const [loading, setLoading] = useState(false);
    const [titulosIcon, setTitulosIcon] = useState({});
    const [inconsistenciasSeleccionadas, setInconsistenciasSeleccionadas] = useState<string[]>([]);
    const [caulsaSeleccionadas, setCausalSeleccionadas] = useState<string[]>([]);
    const [deshablitarNinguna, setDeshabilitarNinguna] = useState(false);
    const [causalYNovedades, setCausalYNovedades] = useState(false);
    const [quitarTiposNovedades, setQuitarTiposNovedades] = useState(false);
    const [traerIdAnterior, setTraerIdAntrior] = useState('');
    const [traerTodaData, setTraerTodaData] = useState<any>({});
    const [mostrarCheck, setMostrarCheck] = useState(false);
    const [llamarNovedades, setLlamarNovedades] = useState(false);
    const [deshabilitarBTNRechazarGp2, setDeshabilitarBTNRechazarGp2] = useState(true);
    const [deshativarBtnEliminarGp2, setDeshativarBtnEliminarGp2] = useState(false);
    const [desHabilitarNingunaGrupo1, setdesHabilitarNingunaGrupo1] = useState(false);

    const documentosAgregados = (
        documentos: DocumentosPorSubModalidadModel[],
    ): void => {
        setDocumentosAguardar(documentos);
    };

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        if (responseSolicitud?.nombreTarea.includes('Seguridad documental') &&
            currentTabValue === TABS_INDEX.INCONSISTENCIAS &&
            listadoInconsistencias &&
            listadoInconsistencias.length === 0 &&
            newValue === TABS_INDEX.COMUNICACIONES) {
            NotificationService.genericAlwarning(
                'Alerta',
                `Por favor diligenciar la información de la pestaña Seguridad Documental`,
                'warning'
            )
            return
        }
        setCurrentTabValue(newValue);
    };
    const user = AuthService.getUserInfo();
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
            // fechaSubsanacion: new Date(),
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

    // useEffect(() => {
    //     verificarEstadosNovedades()
    // }, [respoder]);

    const omitirAlertaCausalRef = useRef<boolean>(false);

    const setOmitirAlertaCausalTemporalmente = (
        value: boolean,
    ): void => {
        omitirAlertaCausalRef.current = value;
    };

    const verificarEstadosNovedades = async () => {
        setLoading(true);

        const request =
            responseSolicitud?.solicitudId as string;

        const esGrupo2Actual =
            nombreTareaActual.includes('grupo 2');

        if (!esGrupo2Actual) {
            setDeshabilitarBTNRechazarGp2(true);
            setMatriculaBloqueadaActiva(false);
        }

        await api
            .GetConsultarInconsistencia(request)
            .then(async (result) => {
                const dataInconsistencias = Array.isArray(
                    result?.data,
                )
                    ? result.data
                    : [];

                const siMatriculaBloqueada = Boolean(
                    responseSolicitud
                        ?.solicitudMatriculaDto
                        ?.some(
                            (item: any) =>
                                normalizarBoolean(
                                    item?.bloqueo,
                                ) ||
                                String(
                                    item?.tipo_bloqueo ??
                                    item?.tipoBloqueo ??
                                    '',
                                ).trim() === '1',
                        ),
                );



                const debeBloquearListaNovedades =
                    esGrupo2Actual &&
                    siMatriculaBloqueada;

                setMatriculaBloqueadaActiva(
                    debeBloquearListaNovedades,
                );



                const obtenerFechaNovedad = (item: any): number => {
                    const fecha =
                        item?.fecha ??
                        item?.fechaRegistro ??
                        item?.fechaCreacion;

                    const fechaConvertida = moment(
                        fecha,
                        [
                            moment.ISO_8601,
                            'DD/MM/YYYY hh:mm:ss A',
                            'DD/MM/YYYY HH:mm:ss',
                        ],
                        true,
                    );

                    if (!fechaConvertida.isValid()) {
                        return 0;
                    }

                    return fechaConvertida.valueOf();
                };

                const obtenerUltimasCausalesPorGrupo = (
                    inconsistencias: any[],
                ): any[] => {
                    const causales = inconsistencias.filter(
                        (item: any) =>
                            item.novedad ===
                            'CAUSAL DE INTERRUPCIÓN',
                    );

                    const causalesPorGrupo = causales.reduce(
                        (
                            acumulador: Record<number, any>,
                            causal: any,
                        ) => {
                            const grupo = Number(
                                causal.grupoInconsistencia,
                            );

                            const causalActual =
                                acumulador[grupo];

                            if (!causalActual) {
                                acumulador[grupo] = causal;
                                return acumulador;
                            }

                            const fechaCausal =
                                obtenerFechaNovedad(causal);

                            const fechaCausalActual =
                                obtenerFechaNovedad(causalActual);

                            if (
                                fechaCausal >=
                                fechaCausalActual
                            ) {
                                acumulador[grupo] = causal;
                            }

                            return acumulador;
                        },
                        {},
                    );

                    return Object.values(causalesPorGrupo);
                };

                const ultimasCausalesPorGrupo =
                    obtenerUltimasCausalesPorGrupo(
                        dataInconsistencias,
                    );

                const validarCausalPendiente =
                    ultimasCausalesPorGrupo.some(
                        (item: any) =>
                            item.subsanado === false,
                    );

                const validarCausalInterrupcionSubsanada =
                    ultimasCausalesPorGrupo.some(
                        (item: any) =>
                            item.subsanado === true,
                    );

                const grupo2 = dataInconsistencias.some(
                    (item: any) =>
                        item.grupoInconsistencia === 2,
                );

                const esCausalInterrupcionPendiente = (
                    item: any,
                ): boolean =>
                    item.novedad ===
                    'CAUSAL DE INTERRUPCIÓN' &&
                    item.subsanado === false;

                const esCausalInterrupcionSubsanada = (
                    item: any,
                ): boolean =>
                    item.novedad ===
                    'CAUSAL DE INTERRUPCIÓN' &&
                    item.subsanado === true;

                /*
                 * VALIDACIONES GRUPO 1
                 */
                if (
                    responseSolicitud?.nombreTarea.includes(
                        'grupo 1',
                    )
                ) {
                    const validarInforme114 =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.novedad ===
                                'INFORME 114 - INCONSISTENCIA',
                        );


                    if (validarInforme114) {
                        setHablitarActualiza(false);
                        setLoading(false);
                        setListadoInconsistencias(
                            dataInconsistencias,
                        );
                        setDeshabilitarNinguna(true);

                        await api
                            .ObtenerComunicacionesSolicitud(
                                request,
                            )
                            .then((response) => {
                                if (
                                    response.data === null
                                ) {
                                    NotificationService.genericAlwarning(
                                        'Alerta',
                                        `Recuerde que para continuar es necesario que genere una comunicación al afiliado`,
                                        'warning',
                                    );
                                }
                            });

                        return;
                    }

                    /*
                     * Solo genera la alerta cuando existe
                     * una causal de interrupción con subsanado false.
                     */
                    if (validarCausalPendiente && !omitirAlertaCausalRef.current) {
                        setHablitarActualiza(true);
                        setLoading(false);
                        setListadoInconsistencias(
                            dataInconsistencias,
                        );
                        setDeshabilitarNinguna(false);

                        NotificationService.genericAlwarning(
                            'CAUSAL DE INTERRUPCIÓN',
                            `La solicitud queda en revisión. \n ¡Por favor regresa a la pantalla de inicio!`,
                            'warning',
                        );

                        return;
                    }

                    setDeshabilitarNinguna(false);

                    if (
                        validarCausalInterrupcionSubsanada &&
                        dataInconsistencias.length > 0
                    ) {
                        setdesHabilitarNingunaGrupo1(
                            true,
                        );
                    } else {
                        setdesHabilitarNingunaGrupo1(
                            false,
                        );
                    }

                    setHablitarActualiza(false);
                    setAprobar(true);
                    setLoading(false);
                }

                /*
                 * VALIDACIONES GRUPO 2
                 */
                if (
                    dataInconsistencias.length > 0 &&
                    nombreTareaActual.includes('grupo 2')
                ) {
                    const validarNoExistesNovedadesGrupo2 =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.grupoInconsistencia ===
                                2,
                        );

                    const novedadAgregadaNinguna =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.grupoInconsistencia ===
                                2 &&
                                item.novedad === 'NINGUNA',
                        );

                    const validarInforme114Grupo1 =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.novedad ===
                                'INFORME 114 - INCONSISTENCIA' &&
                                item.grupoInconsistencia ===
                                1,
                        );

                    const validarInforme114Grupo2 =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.novedad ===
                                'INFORME 114 - INCONSISTENCIA' &&
                                item.grupoInconsistencia ===
                                2,
                        );

                    if (
                        validarInforme114Grupo1 ||
                        validarInforme114Grupo2
                    ) {
                        await api
                            .ObtenerComunicacionesSolicitud(
                                request,
                            )
                            .then((response) => {
                                if (
                                    response.data === null
                                ) {
                                    NotificationService.genericAlwarning(
                                        'Alerta',
                                        `Recuerde que para continuar es necesario que genere una comunicación al afiliado`,
                                        'warning',
                                    );
                                }
                            });
                    }

                    if (
                        !validarNoExistesNovedadesGrupo2
                    ) {
                        setDeshabilitarNinguna(false);
                    }

                    if (novedadAgregadaNinguna) {
                        setDeshabilitarNinguna(true);
                    }

                    const siMatriculaBloqueada = Boolean(
                        responseSolicitud
                            ?.solicitudMatriculaDto
                            ?.some(
                                (item: any) => {
                                    const bloqueo =
                                        normalizarBoolean(
                                            item?.bloqueo,
                                        );

                                    const tipoBloqueo =
                                        String(
                                            item?.tipo_bloqueo ??
                                            item?.tipoBloqueo ??
                                            '',
                                        ).trim();

                                    return (
                                        bloqueo ||
                                        tipoBloqueo === '1'
                                    );
                                },
                            ),
                    );


                    /*
                     * Verifica las causales de ambos grupos.
                     * Si existe al menos una con subsanado false,
                     * conserva la alerta de Grupo 2.
                     */
                    if (
                        validarCausalPendiente &&
                        !omitirAlertaCausalRef.current
                    ) {
                        setHablitarActualiza(true);
                        setLoading(false);
                        setListadoInconsistencias(
                            dataInconsistencias,
                        );

                        NotificationService.genericAlwarning(
                            'CAUSAL DE INTERRUPCIÓN',
                            'La solicitud queda en revisión. \n ¡Por favor regresa a la pantalla de inicio!',
                            'warning',
                        );

                        return;
                    }

                    /*
                     * Solamente se bloquean las acciones cuando
                     * la matrícula continúa bloqueada y todavía
                     * no existe un análisis que autorice el
                     * desbloqueo.
                     */
                    if (debeBloquearListaNovedades) {
                        setListadoInconsistencias(
                            dataInconsistencias,
                        );
                        setLoading(false);

                        NotificationService.genericAlwarning(
                            'MATRÍCULA BLOQUEADA',
                            'La solicitud requiere análisis de trazabilidad y uso de inmueble en el módulo Matrículas inmobiliarias.',
                            'warning',
                        );

                        return;
                    }

                    /*
                     * Si no existe bloqueo o ya fue registrado
                     * el desbloqueo, se habilitan nuevamente las
                     * acciones de la tabla.
                     */
                    setMatriculaBloqueadaActiva(false);
                    setLoading(false);

                    const existeNovedadSubsanableNoSubsanadaGrupo1O2 =
                        dataInconsistencias.some(
                            esNovedadSubsanableNoSubsanadaGrupo1O2,
                        );

                    setDeshabilitarBTNRechazarGp2(
                        !existeNovedadSubsanableNoSubsanadaGrupo1O2,
                    );

                    const algunSubsanadoFalseGrupo1 =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.grupoInconsistencia ===
                                1 &&
                                item.subsanado === false &&
                                item.novedad !==
                                'INFORME 114 - INCONSISTENCIA',
                        );

                    const algunSubsanadoFalseGrupo2 =
                        dataInconsistencias.some(
                            (item: any) =>
                                item.grupoInconsistencia ===
                                2 &&
                                item.subsanado === false,
                        );

                    if (
                        !algunSubsanadoFalseGrupo1 &&
                        !algunSubsanadoFalseGrupo2 &&
                        grupo2
                    ) {
                        setHablitarActualiza(false);
                        setLoading(false);
                    } else {
                        setHablitarActualiza(true);
                        setAprobar(true);
                        setLoading(false);
                    }

                    if (validarInforme114Grupo2) {
                        setHablitarActualiza(false);
                        setLoading(false);
                    }
                } else if (
                    !grupo2 &&
                    !responseSolicitud?.nombreTarea.includes(
                        'grupo 1',
                    )
                ) {
                    setHablitarActualiza(true);
                    setLoading(false);
                }

                setListadoInconsistencias(
                    dataInconsistencias,
                );
            });
    };

    useEffect(() => {
        if (!responseSolicitud?.solicitudId) {
            return;
        }

        const nombreTarea = normalizarTexto(responseSolicitud?.nombreTarea);

        const esSeguridadDocumental =
            nombreTarea.includes('seguridad documental grupo 1') ||
            nombreTarea.includes('seguridad documental grupo 2');

        if (!esSeguridadDocumental) {
            return;
        }

        verificarEstadosNovedades();
    }, [
        responseSolicitud?.solicitudId,
        responseSolicitud?.solicitudTareaId,
        responseSolicitud?.nombreTarea,
    ]);


    const onSubmitAprobar = async () => {

        const informacionObservaciones = formularioObservaciones.getValues();

        if (informacionObservaciones?.observaciones == null || informacionObservaciones?.observaciones === '') {
            toast('Por favor ingrese alguna observación', { type: toast.TYPE.ERROR });
            return;
        }

        await api.actualizarObservaciones({
            idSolicitudTarea: responseSolicitud?.solicitudTareaId,
            observaciones: informacionObservaciones?.observaciones
        })

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro que desea continuar ?',
        );
        if (confirmAlert) {

            setLoading(true)
            const aprobarSolicitudTareaDto = {
                solicitudTareaId: responseSolicitud?.solicitudTareaId as string,
                clienteId: responseSolicitud?.clienteId as number
            };

            await api
                .PostAprobarSolicitudTarea(aprobarSolicitudTareaDto)
                .then(async (response) => {
                    if (response) {
                        if (response.isSuccess) {
                            toast('Se aprobó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
                            // setSelectedData(response.data);

                            obtnerBoleano(true);
                            onItemGoBack();
                        } else {
                            toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
                        }
                    }
                }).catch(() => {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                });
            setLoading(false)
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

    const validarNovedadParaRechazar = async (): Promise<boolean> => {
        const nombreTarea = normalizarTexto(responseSolicitud?.nombreTarea);

        const esSeguridadDocumental =
            nombreTarea.includes('seguridad documental grupo 1') ||
            nombreTarea.includes('seguridad documental grupo 2');

        if (!esSeguridadDocumental) {
            return true;
        }

        const grupoActual = nombreTarea.includes('grupo 2') ? 2 : 1;

        const novedadSeleccionada = normalizarTexto(getValues('novedad'));

        const hayNovedadSeleccionadaSinAgregar =
            novedadSeleccionada &&
            novedadSeleccionada !== 'seleccione';

        try {
            const response = await api.GetConsultarInconsistencia(
                responseSolicitud?.solicitudId as string
            );

            const novedades = Array.isArray(response?.data) ? response.data : [];

            const novedadesGrupoActual = novedades.filter((item: any) =>
                Number(item?.grupoInconsistencia) === grupoActual
            );

            if (novedadesGrupoActual.length === 0) {
                if (hayNovedadSeleccionadaSinAgregar) {
                    toast(
                        `Debe agregar la novedad seleccionada a la lista antes de rechazar en Seguridad documental grupo ${grupoActual}.`,
                        { type: toast.TYPE.ERROR }
                    );
                    return false;
                }

                toast(
                    `Debe seleccionar y agregar al menos una novedad antes de rechazar en Seguridad documental grupo ${grupoActual}.`,
                    { type: toast.TYPE.ERROR }
                );
                return false;
            }

            const soloTieneNinguna = novedadesGrupoActual.every((item: any) =>
                normalizarTexto(item?.novedad) === 'ninguna'
            );

            if (soloTieneNinguna) {
                toast(
                    `Para rechazar debe agregar una novedad diferente a NINGUNA en Seguridad documental grupo ${grupoActual}.`,
                    { type: toast.TYPE.ERROR }
                );
                return false;
            }

            return true;
        } catch (error) {
            toast('No fue posible validar las novedades antes de rechazar.', {
                type: toast.TYPE.ERROR
            });
            return false;
        }
    };

    const esErrorLlaveDuplicada = (mensaje: any): boolean => {
        const texto = String(mensaje || '').toLowerCase();

        return (
            texto.includes('primary key') ||
            texto.includes('duplicate key') ||
            texto.includes('cannot insert duplicate key') ||
            texto.includes('pk__slt_soli')
        );
    };

    const verificarSiRechazoQuedoAplicado = async (): Promise<boolean> => {
        try {
            const responseTrazabilidad = await api.obtenerTrazabilidadSolicitud(
                responseSolicitud?.solicitudId
            );

            const data = Array.isArray(responseTrazabilidad?.data)
                ? responseTrazabilidad.data
                : [];

            setTrazabilidad(data);

            const tareaActual = data.find((item: any) =>
                item?.idTareaSolicitud === responseSolicitud?.solicitudTareaId
            );

            const tareaActualRechazada =
                normalizarTexto(tareaActual?.descripcionEstadoSolicitud) === 'rechazada';

            return tareaActualRechazada;
        } catch (error) {
            console.error('Error verificando rechazo en trazabilidad:', error);
            return false;
        }
    };

    const verificarResultadoRechazoEnTrazabilidad = async (): Promise<boolean> => {
        try {
            const responseTrazabilidad = await api.obtenerTrazabilidadSolicitud(
                responseSolicitud?.solicitudId
            );

            const data = Array.isArray(responseTrazabilidad?.data)
                ? responseTrazabilidad.data
                : [];

            setTrazabilidad(data);

            const tareaActual = data.find((item: any) =>
                item?.idTareaSolicitud === responseSolicitud?.solicitudTareaId
            );

            const tareaActualRechazada =
                normalizarTexto(tareaActual?.descripcionEstadoSolicitud) === 'rechazada';

            const existeRadicacionPendiente = data.some((item: any) =>
                normalizarTexto(item?.descripcionTarea).includes('radicacion') &&
                normalizarTexto(item?.descripcionEstadoSolicitud) === 'pendiente'
            );

            return tareaActualRechazada || existeRadicacionPendiente;
        } catch (error) {
            console.error('Error verificando trazabilidad después del rechazo:', error);
            return false;
        }
    };

    const onSubmitRechazar = async () => {
        if (rechazandoRef.current) {
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
            return;
        }

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro que desea continuar?',
        );

        if (!confirmAlert) {
            return;
        }

        rechazandoRef.current = true;
        setLoading(true);

        try {
            const responseActualizarObservacion = await api.actualizarObservaciones({
                idSolicitudTarea: responseSolicitud?.solicitudTareaId,
                observaciones: observacionActual
            });

            if (responseActualizarObservacion?.isSuccess === false) {
                toast(
                    responseActualizarObservacion?.returnMessage ||
                    'No fue posible guardar la observación antes de rechazar.',
                    { type: toast.TYPE.ERROR }
                );
                return;
            }

            const rechazarSolicitudTareaDto = {
                solicitudTareaId: responseSolicitud?.solicitudTareaId as string
            };


            const response = await api.PostRechazarSolicitudTarea(
                rechazarSolicitudTareaDto
            );


            if (response) {
                if (response.isSuccess) {
                    toast('Se rechazó la solicitud correctamente.', {
                        type: toast.TYPE.SUCCESS
                    });

                    onItemGoBack();
                    obtnerBoleano(true);
                    return;
                }

                if (esErrorLlaveDuplicada(response.returnMessage)) {
                    const rechazoQuedoAplicado = await verificarSiRechazoQuedoAplicado();

                    if (rechazoQuedoAplicado) {
                        toast('Se rechazó la solicitud correctamente.', {
                            type: toast.TYPE.SUCCESS
                        });

                        onItemGoBack();
                        obtnerBoleano(true);
                        return;
                    }
                }

                toast(
                    response.returnMessage || 'No fue posible rechazar la solicitud.',
                    { type: toast.TYPE.ERROR }
                );
            }

            toast(
                response?.returnMessage || 'No fue posible rechazar la solicitud.',
                { type: toast.TYPE.ERROR }
            );
        } catch (error) {
            console.error('Error rechazando solicitud:', error);

            toast('No fue posible rechazar la solicitud.', {
                type: toast.TYPE.ERROR
            });
        } finally {
            rechazandoRef.current = false;
            setLoading(false);
        }
    };

    const [error, setError] = useState(false);


    const onSubmit2 = async () => {
        const informacionObservaciones =
            formularioObservaciones.getValues();

        const observacionGuardada = String(
            informacionObservaciones?.observaciones || '',
        ).trim();

        if (!observacionGuardada) {
            toast(
                'Por favor ingrese alguna observación',
                { type: toast.TYPE.ERROR },
            );

            setActualizacionInconsistenciasCompletada(false);

            return false;
        }

        setLoading(true);

        try {
            const responseActualizar =
                await api.actualizarObservaciones({
                    idSolicitudTarea:
                        responseSolicitud?.solicitudTareaId,
                    observaciones:
                        observacionGuardada,
                });

            if (responseActualizar?.isSuccess === false) {
                toast(
                    responseActualizar?.returnMessage ||
                    'No fue posible actualizar la información.',
                    { type: toast.TYPE.ERROR },
                );

                setActualizacionInconsistenciasCompletada(false);

                return false;
            }

            formularioObservaciones.setValue(
                'observaciones',
                observacionGuardada,
                {
                    shouldValidate: true,
                    shouldDirty: false,
                },
            );

            if (
                currentTabValue ===
                TABS_INDEX.COMUNICACIONES
            ) {
                setAprobar(false);

                formularioObservaciones.reset(
                    formularioObservaciones.getValues(),
                );

                toast(
                    'Información actualizada correctamente',
                    { type: toast.TYPE.SUCCESS },
                );

                return true;
            }

            if (
                currentTabValue ===
                TABS_INDEX.INCONSISTENCIAS
            ) {
                setActualizacionInconsistenciasCompletada(
                    true,
                );

                setAprobar(false);

                formularioObservaciones.reset(
                    formularioObservaciones.getValues(),
                );

                toast(
                    'Información actualizada correctamente',
                    { type: toast.TYPE.SUCCESS },
                );

                return true;
            }
        } catch (error) {
            console.error(
                'Error actualizando observaciones:',
                error,
            );

            setActualizacionInconsistenciasCompletada(
                false,
            );

            toast(
                'No fue posible actualizar la información.',
                { type: toast.TYPE.ERROR },
            );

            return false;
        } finally {
            setLoading(false);
        }
        return false;
    };

    const addInconsistencia = async (data: any) => {
        setMostrarCheck(false);
        setResetNovedad(true);

        const nuevaInconsistencia: string =
            data?.listaInconsistenciaId;

        const nuevaCausalInterrupcion: string =
            data?.causalInterrupcionId;

        if (
            !caulsaSeleccionadas.includes(
                nuevaCausalInterrupcion,
            )
        ) {
            setCausalSeleccionadas(
                (prevCausalInterrupcion) => [
                    ...prevCausalInterrupcion,
                    nuevaCausalInterrupcion,
                ],
            );
        }

        if (
            !inconsistenciasSeleccionadas.includes(
                nuevaInconsistencia,
            )
        ) {
            setInconsistenciasSeleccionadas(
                (prevInconsistencias) => [
                    ...prevInconsistencias,
                    nuevaInconsistencia,
                ],
            );
        }

        if (data.novedad === 'NINGUNA') {
            setDeshabilitarNinguna(true);
            setValue('subsanado', true);
        }

        if (data.novedad === 'PAGOS SUCESIVOS') {
            setDeshabilitarNinguna(true);
        }

        if (
            data.novedad === 'NOVEDAD SUBSANABLE' ||
            data.novedad ===
            'ANÁLISIS DESBLOQUEO MATRÍCULAS' ||
            data.novedad ===
            'CAUSAL DE INTERRUPCIÓN'
        ) {
            setCausalYNovedades(true);
            setDeshabilitarNinguna(false);
        }

        if (
            data?.novedad ===
            'INFORME 114 - INCONSISTENCIA'
        ) {
            setDeshabilitarNinguna(true);
        }

        if (
            data.novedad === 'Seleccione' ||
            data.novedad === 0
        ) {
            toast(
                'Debe seleccionar un campo válido',
                { type: toast.TYPE.ERROR },
            );
            return;
        }

        const isNovedadNingunaGrupo2 =
            responseSolicitud?.nombreTarea?.includes(
                'grupo 2',
            ) &&
            data.novedad === 'NINGUNA';

        if (isNovedadNingunaGrupo2) {
            data.subsanado = true;
        }

        setQuitarTiposNovedades(true);

        data.grupoInconsistencia =
            grupo1 ? 1 : 2;

        const solicitudTareaId =
            responseSolicitud?.solicitudTareaId;

        const fechaRegistro =
            data?.fechaRegistro ||
            data?.fecha_Novedad ||
            moment().format(
                'YYYY-MM-DDTHH:mm:ss',
            );

        const nuevaData = {
            ...data,
            solicitudId:
                data?.solicitudId ||
                responseSolicitud?.solicitudId,
            solicitudTareaId,
            fechaRegistro,
        };

        console.log('nuevaData', nuevaData);



        if (
            currentTabValue !==
            TABS_INDEX.INCONSISTENCIAS
        ) {
            reset();
            return;
        }

        let result: any;

        if (data.inconsistenciaId === null) {
            result =
                await api.PostInsertarInconsistenciaSolicitud(
                    nuevaData,
                );
        } else {
            result =
                await api.PostActualizarInconsistenciaSolicitud(
                    nuevaData,
                );
        }


        if (!result?.isSuccess) {
            setError(true);
            setCurrentTabValue(
                TABS_INDEX.INCONSISTENCIAS,
            );
            setAccionNumber(
                BOTONERA_SOLICITUD.ACTUALIZAR,
            );

            toast(
                result?.returnMessage ||
                'Error interno del servidor',
                { type: toast.TYPE.ERROR },
            );

            return;
        }

        setError(false);

        marcarInconsistenciasPendientesDeActualizar();

        setListadoInconsistencias(
            (prevListado) => [
                ...prevListado,
                nuevaData,
            ],
        );

        await verificarEstadosNovedades();

        toast(
            'Información guardada correctamente',
            { type: toast.TYPE.SUCCESS },
        );

        reset();
    };

    // FUNCIÓN NUEVA

    // const addInconsistencia = async (data: any) => {
    //     setMostrarCheck(false);
    //     setResetNovedad(true);

    //     const nuevaInconsistencia: string =
    //         data?.listaInconsistenciaId;

    //     const nuevaCausalInterrupcion: string =
    //         data?.causalInterrupcionId;

    //     if (
    //         nuevaCausalInterrupcion &&
    //         !caulsaSeleccionadas.includes(
    //             nuevaCausalInterrupcion,
    //         )
    //     ) {
    //         setCausalSeleccionadas(
    //             (prevCausalInterrupcion) => [
    //                 ...prevCausalInterrupcion,
    //                 nuevaCausalInterrupcion,
    //             ],
    //         );
    //     }

    //     if (
    //         nuevaInconsistencia &&
    //         !inconsistenciasSeleccionadas.includes(
    //             nuevaInconsistencia,
    //         )
    //     ) {
    //         setInconsistenciasSeleccionadas(
    //             (prevInconsistencias) => [
    //                 ...prevInconsistencias,
    //                 nuevaInconsistencia,
    //             ],
    //         );
    //     }

    //     if (data?.novedad === 'NINGUNA') {
    //         setDeshabilitarNinguna(true);
    //         setValue('subsanado', true);
    //         data.subsanado = true;
    //     }

    //     if (data?.novedad === 'PAGOS SUCESIVOS') {
    //         setDeshabilitarNinguna(true);
    //     }

    //     if (
    //         data?.novedad === 'NOVEDAD SUBSANABLE' ||
    //         data?.novedad ===
    //         'ANÁLISIS DESBLOQUEO MATRÍCULAS' ||
    //         data?.novedad ===
    //         'CAUSAL DE INTERRUPCIÓN'
    //     ) {
    //         setCausalYNovedades(true);
    //         setDeshabilitarNinguna(false);
    //     }

    //     if (
    //         data?.novedad ===
    //         'INFORME 114 - INCONSISTENCIA'
    //     ) {
    //         setDeshabilitarNinguna(true);
    //     }

    //     if (
    //         data?.novedad === 'Seleccione' ||
    //         data?.novedad === 0
    //     ) {
    //         toast(
    //             'Debe seleccionar un campo válido',
    //             {
    //                 type: toast.TYPE.ERROR,
    //             },
    //         );

    //         return;
    //     }

    //     const isNovedadNingunaGrupo2 =
    //         responseSolicitud?.nombreTarea
    //             ?.toLowerCase()
    //             .includes('grupo 2') &&
    //         data?.novedad === 'NINGUNA';

    //     if (isNovedadNingunaGrupo2) {
    //         data.subsanado = true;
    //     }

    //     if (
    //         currentTabValue !==
    //         TABS_INDEX.INCONSISTENCIAS
    //     ) {
    //         reset();
    //         return;
    //     }

    //     setQuitarTiposNovedades(true);

    //     const solicitudTareaId =
    //         responseSolicitud?.solicitudTareaId;

    //     const fechaRegistro =
    //         data?.fechaRegistro ||
    //         data?.fecha_Novedad ||
    //         moment().format(
    //             'YYYY-MM-DDTHH:mm:ss',
    //         );

    //     const esSubsanacion =
    //         data?.subsanado === true ||
    //         data?.subsanado === 'true';

    //     const grupoInconsistencia =
    //         grupo1 ? 1 : 2;

    //     const solicitudId =
    //         data?.solicitudId ||
    //         responseSolicitud?.solicitudId;

    //     const inconsistenciaRegistrada =
    //         esSubsanacion
    //             ? listadoInconsistencias.find(
    //                 (
    //                     item:
    //                         FormularioInconsistenciasDto,
    //                 ) => {
    //                     const estaPendiente =
    //                         item?.subsanado === false;

    //                     const mismaSolicitud =
    //                         item?.solicitudId ===
    //                         solicitudId;

    //                     const mismoGrupo =
    //                         Number(
    //                             item?.grupoInconsistencia,
    //                         ) === grupoInconsistencia;

    //                     const mismaCausal =
    //                         Boolean(
    //                             data?.causalInterrupcionId,
    //                         ) &&
    //                         item?.causalInterrupcionId ===
    //                         data?.causalInterrupcionId;

    //                     const mismaInconsistencia =
    //                         Boolean(
    //                             data?.listaInconsistenciaId,
    //                         ) &&
    //                         item?.listaInconsistenciaId ===
    //                         data?.listaInconsistenciaId;

    //                     return (
    //                         estaPendiente &&
    //                         mismaSolicitud &&
    //                         mismoGrupo &&
    //                         (
    //                             mismaCausal ||
    //                             mismaInconsistencia
    //                         )
    //                     );
    //                 },
    //             )
    //             : undefined;

    //     const inconsistenciaId =
    //         data?.inconsistenciaId ??
    //         inconsistenciaRegistrada?.inconsistenciaId ??
    //         null;

    //     const nuevaData = {
    //         ...data,
    //         inconsistenciaId,
    //         solicitudId,
    //         solicitudTareaId,
    //         fechaRegistro,
    //         grupoInconsistencia,
    //         subsanado: esSubsanacion,
    //     };

    //     if (
    //         esSubsanacion &&
    //         !inconsistenciaId
    //     ) {
    //         console.log(
    //             '[INCONSISTENCIA NO ENCONTRADA PARA SUBSANAR]',
    //             {
    //                 data,
    //                 grupoVinculoNovedadId:
    //                     data?.grupoVinculoNovedadId,
    //                 listadoInconsistencias,
    //             },
    //         );

    //         toast(
    //             'No se encontró el identificador de la inconsistencia que se desea subsanar',
    //             {
    //                 type: toast.TYPE.ERROR,
    //             },
    //         );

    //         return;
    //     }



    //     let result: any;

    //     try {
    //         if (esSubsanacion) {
    //             result =
    //                 await api.PostActualizarInconsistenciaSolicitud(
    //                     nuevaData,
    //                 );
    //         } else {
    //             result =
    //                 await api.PostInsertarInconsistenciaSolicitud(
    //                     nuevaData,
    //                 );
    //         }


    //         if (!result?.isSuccess) {
    //             setError(true);

    //             setCurrentTabValue(
    //                 TABS_INDEX.INCONSISTENCIAS,
    //             );

    //             setAccionNumber(
    //                 BOTONERA_SOLICITUD.ACTUALIZAR,
    //             );

    //             toast(
    //                 result?.returnMessage ||
    //                 'Error interno del servidor',
    //                 {
    //                     type: toast.TYPE.ERROR,
    //                 },
    //             );

    //             return;
    //         }

    //         setError(false);

    //         marcarInconsistenciasPendientesDeActualizar();

    //         if (esSubsanacion) {
    //             setListadoInconsistencias(
    //                 (prevListado) =>
    //                     prevListado.map(
    //                         (item: any) => {
    //                             const itemId =
    //                                 item?.inconsistenciaId ??
    //                                 item?.idInconsistenciaSolicitud ??
    //                                 item?.id;

    //                             const mismoId =
    //                                 String(itemId) ===
    //                                 String(inconsistenciaId);

    //                             const mismoGrupoVinculo =
    //                                 item
    //                                     ?.grupoVinculoNovedadId ===
    //                                 nuevaData
    //                                     ?.grupoVinculoNovedadId;

    //                             if (
    //                                 mismoId ||
    //                                 mismoGrupoVinculo
    //                             ) {
    //                                 return {
    //                                     ...item,
    //                                     ...nuevaData,
    //                                     ...(result?.data ?? {}),
    //                                     inconsistenciaId,
    //                                     subsanado: true,
    //                                 };
    //                             }

    //                             return item;
    //                         },
    //                     ),
    //             );
    //         } else {
    //             const inconsistenciaInsertada =
    //                 result?.data
    //                     ? {
    //                         ...nuevaData,
    //                         ...result.data,
    //                     }
    //                     : nuevaData;

    //             setListadoInconsistencias(
    //                 (prevListado) => [
    //                     ...prevListado,
    //                     inconsistenciaInsertada,
    //                 ],
    //             );
    //         }

    //         await verificarEstadosNovedades();

    //         toast(
    //             esSubsanacion
    //                 ? 'Información actualizada correctamente'
    //                 : 'Información insertada correctamente',
    //             {
    //                 type: toast.TYPE.SUCCESS,
    //             },
    //         );

    //         reset();
    //     } catch (error: any) {
    //         const respuestaError =
    //             error?.response?.data ??
    //             error?.data ??
    //             error;


    //         setError(true);

    //         toast(
    //             respuestaError?.returnMessage ||
    //             respuestaError?.message ||
    //             (
    //                 esSubsanacion
    //                     ? 'Se presentó un error al subsanar la inconsistencia'
    //                     : 'Se presentó un error al registrar la inconsistencia'
    //             ),
    //             {
    //                 type: toast.TYPE.ERROR,
    //             },
    //         );
    //     }
    // };

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

        if (data.novedad === "NINGUNA" && !responseSolicitud?.nombreTarea.includes("grupo 1")) {
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

        const index = Number(
            watch('index'),
        );

        const items = [
            ...listadoInconsistencias,
        ];

        if (index > 0) {
            items[index - 1] = data;
        }

        setListadoInconsistencias(items);

        const actualizarHabilitado = listadoInconsistencias?.every((novedad: any) => novedad.subsanado);

        if (responseSolicitud?.nombreTarea.includes("grupo 2")) {
            setAprobar(true);
            setHablitarActualiza(actualizarHabilitado);
        }
        else {
            setHablitarActualiza(actualizarHabilitado);
        }

        if (responseSolicitud?.nombreTarea.includes("grupo 1")) {
            setHablitarActualiza(true);
        }

        if (responseSolicitud?.nombreTarea.includes("grupo 1") || responseSolicitud?.nombreTarea.includes("grupo 2")) {

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

                                marcarInconsistenciasPendientesDeActualizar();

                                await verificarEstadosNovedades()

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


    // const deleteInconsistencias = async (data: any) => {

    //     const confirmAlert = await NotificationService.confirmAlert(
    //         '¿Desea eliminar la inconsistencia?'
    //     );

    //     if (confirmAlert) {
    //         setLoading(true);

    //         try {
    //             const request = {
    //                 sol_ID_FK: responseSolicitud?.solicitudId,
    //                 inC_INCONSISTENCIA: data.novedad,
    //                 tpO_NOV_ID: data.listaInconsistenciaId,
    //                 caU_ID: data.causalInterrupcionId,
    //                 inF_ID: data.inconsistenciaId,
    //                 inc_GRUPO: data.grupoInconsistencia,
    //                 slT_ID: responseSolicitud?.solicitudTareaId,
    //             };

    //             const response = await api.EliminarNovedad(request);

    //             if (response?.isSuccess) {
    //                 setListadoInconsistencias((prev) =>
    //                     prev.filter((x) => x.listaInconsistenciaId !== data.listaInconsistenciaId)
    //                 );
    //                 toast('Se eliminó correctamente.', { type: toast.TYPE.SUCCESS });
    //                 verificarEstadosNovedades()

    //             } else {
    //                 toast(`Error: ${response?.returnMessage}`, { type: toast.TYPE.ERROR });
    //             }

    //         } catch {
    //             toast('Error del sistema', { type: toast.TYPE.ERROR });
    //         } finally {
    //             setLoading(false);
    //         }
    //     }
    // };

    const deleteInconsistencias = async (
        data: any,
        opciones: {
            mostrarAlerta?: boolean;
            mostrarToast?: boolean;
        } = {},
    ) => {
        const {
            mostrarAlerta = true,
            mostrarToast = true,
        } = opciones;

        const confirmAlert = mostrarAlerta
            ? await NotificationService.confirmAlert(
                '¿Desea eliminar la inconsistencia?',
            )
            : true;

        if (!confirmAlert) {
            return {
                isSuccess: false,
                isCancelled: true,
            };
        }

        setLoading(true);

        try {
            const request = {
                sol_ID_FK: responseSolicitud?.solicitudId,
                inC_INCONSISTENCIA: data.novedad,
                tpO_NOV_ID: data.listaInconsistenciaId,
                caU_ID: data.causalInterrupcionId,
                inF_ID: data.inconsistenciaId,
                inc_GRUPO: data.grupoInconsistencia,
                slT_ID: responseSolicitud?.solicitudTareaId,
            };

            const response = await api.EliminarNovedad(request);

            if (response?.isSuccess) {
                marcarInconsistenciasPendientesDeActualizar();

                setListadoInconsistencias((prev) =>
                    prev.filter((x) => {
                        if (data?.inconsistenciaId) {
                            return x.inconsistenciaId !== data.inconsistenciaId;
                        }

                        return x.listaInconsistenciaId !== data.listaInconsistenciaId;
                    }),
                );

                if (mostrarToast) {
                    toast('Se eliminó correctamente.', {
                        type: toast.TYPE.SUCCESS,
                    });
                }

                await verificarEstadosNovedades();

                return response;
            }

            if (mostrarToast) {
                toast(`Error: ${response?.returnMessage}`, {
                    type: toast.TYPE.ERROR,
                });
            }

            return response;
        } catch (error) {
            if (mostrarToast) {
                toast('Error del sistema', {
                    type: toast.TYPE.ERROR,
                });
            }

            return {
                isSuccess: false,
                error,
            };
        } finally {
            setLoading(false);
        }
    };

    const formularioObservaciones = useForm({
        defaultValues: {
            idSolicitud: responseSolicitud?.solicitudTareaId,
            observaciones: '',
            observacionesGrupo1: '',
            observacionesGrupo2: ''
        }
    })

    const marcarInconsistenciasPendientesDeActualizar =
        () => {
            setActualizacionInconsistenciasCompletada(
                false,
            );

        };

    const observacionActual =
        formularioObservaciones.watch(
            'observaciones',
        );

    const observacionesModificadas =
        formularioObservaciones.formState.isDirty;


    useEffect(() => {
        if (!observacionesModificadas) {
            return;
        }

        marcarInconsistenciasPendientesDeActualizar();
    }, [
        observacionActual,
        observacionesModificadas,
    ]);

    const [
        actualizacionInconsistenciasCompletada,
        setActualizacionInconsistenciasCompletada,
    ] = useState(false);



    let grupoInconsistenciaActual: number | null = null;

    if (nombreTareaActual.includes('grupo 1')) {
        grupoInconsistenciaActual = 1;
    } else if (nombreTareaActual.includes('grupo 2')) {
        grupoInconsistenciaActual = 2;
    }

    const inconsistenciasGrupoActual = Array.isArray(
        listadoInconsistencias,
    )
        ? listadoInconsistencias.filter(
            (item: any) =>
                Number(item?.grupoInconsistencia) ===
                grupoInconsistenciaActual,
        )
        : [];


    const inconsistenciasGrupos1Y2 = Array.isArray(
        listadoInconsistencias,
    )
        ? listadoInconsistencias.filter(
            (item: any) => {
                const grupoItem = Number(
                    item?.grupoInconsistencia,
                );

                return (
                    grupoItem === 1 ||
                    grupoItem === 2
                );
            },
        )
        : [];


    let inconsistenciasParaRechazar: FormularioInconsistenciasDto[] =
        [];

    if (grupoInconsistenciaActual === 2) {
        inconsistenciasParaRechazar =
            inconsistenciasGrupos1Y2;
    }


    const normalizarValorInconsistencia = (valor: any) =>
        String(valor ?? '').trim().toUpperCase();

    const esRegistroSubsanado = (item: any) =>
        item?.subsanado === true ||
        String(item?.subsanado).toLowerCase() === 'true';

    const novedadesValidasParaRechazar =
        inconsistenciasParaRechazar.filter(
            (item: any) => {
                const novedad =
                    normalizarValorInconsistencia(
                        item?.novedad,
                    );

                return (
                    Boolean(novedad) &&
                    novedad !== 'SELECCIONE'
                );
            },
        );

    const obtenerLlaveLogicaInconsistencia = (
        item: any,
    ) => {
        const idVinculo =
            item?.grupoVinculoNovedadId ||
            item?.idGrupoNovedad ||
            item?.novedadVinculoId;

        if (idVinculo) {
            return normalizarValorInconsistencia(
                idVinculo,
            );
        }

        return [
            item?.grupoInconsistencia,
            item?.novedad,
            item?.listaInconsistenciaId,
            item?.causalInterrupcionId,
            item?.informe,
            item?.pagoSucesivo,
        ]
            .map(normalizarValorInconsistencia)
            .join('|');
    };


    const novedadesSubsanablesParaRechazar =
        novedadesValidasParaRechazar.filter(
            (item: any) =>
                normalizarValorInconsistencia(
                    item?.novedad,
                ) === 'NOVEDAD SUBSANABLE',
        );

    const llavesSubsanablesParaRechazar =
        Array.from(
            new Set(
                novedadesSubsanablesParaRechazar.map(
                    obtenerLlaveLogicaInconsistencia,
                ),
            ),
        );

    const todasLasSubsanablesParaRechazarEstanSubsanadas =
        llavesSubsanablesParaRechazar.length > 0 &&
        llavesSubsanablesParaRechazar.every(
            (llave: string) =>
                novedadesSubsanablesParaRechazar.some(
                    (item: any) =>
                        obtenerLlaveLogicaInconsistencia(
                            item,
                        ) === llave &&
                        esRegistroSubsanado(item),
                ),
        );

    const haySubsanablePendienteParaRechazar =
        llavesSubsanablesParaRechazar.length > 0 &&
        !todasLasSubsanablesParaRechazarEstanSubsanadas;


    const novedadesValidasGrupoActual =
        inconsistenciasGrupoActual.filter(
            (item: any) => {
                const novedad = normalizarValorInconsistencia(
                    item?.novedad,
                );

                return Boolean(novedad) && novedad !== 'SELECCIONE';
            },
        );

    const hayInforme114 =
        novedadesValidasGrupoActual.some(
            (item: any) =>
                normalizarValorInconsistencia(
                    item?.novedad,
                ) ===
                'INFORME 114 - INCONSISTENCIA',
        );

    const causalesInterrupcionGrupoActual =
        novedadesValidasGrupoActual.filter(
            (item: any) =>
                normalizarValorInconsistencia(
                    item?.novedad,
                ) ===
                'CAUSAL DE INTERRUPCIÓN',
        );

    const llavesCausalesInterrupcion =
        Array.from(
            new Set(
                causalesInterrupcionGrupoActual.map(
                    obtenerLlaveLogicaInconsistencia,
                ),
            ),
        );

    const todasLasCausalesEstanSubsanadas =
        llavesCausalesInterrupcion.length > 0 &&
        llavesCausalesInterrupcion.every(
            (llave: string) =>
                causalesInterrupcionGrupoActual.some(
                    (item: any) =>
                        obtenerLlaveLogicaInconsistencia(
                            item,
                        ) === llave &&
                        esRegistroSubsanado(item),
                ),
        );

    const hayCausalInterrupcionPendiente =
        llavesCausalesInterrupcion.length > 0 &&
        !todasLasCausalesEstanSubsanadas;

    const hayNovedades =
        novedadesValidasGrupoActual.length > 0;

    const novedadesSubsanables =
        novedadesValidasGrupoActual.filter(
            (item: any) =>
                normalizarValorInconsistencia(
                    item?.novedad,
                ) === 'NOVEDAD SUBSANABLE',
        );

    const llavesSubsanables = Array.from(
        new Set(
            novedadesSubsanables.map(
                obtenerLlaveLogicaInconsistencia,
            ),
        ),
    );

    const todasLasSubsanablesEstanSubsanadas =
        llavesSubsanables.length > 0 &&
        llavesSubsanables.every((llave: string) =>
            novedadesSubsanables.some(
                (item: any) =>
                    obtenerLlaveLogicaInconsistencia(item) ===
                    llave &&
                    esRegistroSubsanado(item),
            ),
        );

    const haySubsanablePendiente =
        llavesSubsanables.length > 0 &&
        !todasLasSubsanablesEstanSubsanadas;

    let haySubsanablePendienteParaFlujo =
        haySubsanablePendiente;

    if (grupoInconsistenciaActual === 2) {
        haySubsanablePendienteParaFlujo =
            haySubsanablePendienteParaRechazar;
    }

    const hayNovedadesNoSubsanables =
        novedadesValidasGrupoActual.some(
            (item: any) =>
                normalizarValorInconsistencia(
                    item?.novedad,
                ) !== 'NOVEDAD SUBSANABLE',
        );

    let puedeActualizarInconsistencias = false;
    let puedeAprobarInconsistencias = false;
    let puedeRechazarInconsistencias = false;

    // const puedeRegresarInconsistencias = true;

    if (!hayNovedades) {
        puedeActualizarInconsistencias = false;
        puedeAprobarInconsistencias = false;
        puedeRechazarInconsistencias = false;
    } else if (hayInforme114) {
        /*
         * INFORME 114:
         * Aprobar directamente y permitir regresar.
         */
        puedeActualizarInconsistencias = false;
        puedeAprobarInconsistencias = true;
        puedeRechazarInconsistencias = false;
    } else if (hayCausalInterrupcionPendiente) {
        /*
         * Causal sin subsanar:
         * solamente Regresar.
         */
        puedeActualizarInconsistencias = false;
        puedeAprobarInconsistencias = false;
        puedeRechazarInconsistencias = false;
    } else if (
        grupoInconsistenciaActual === 2 &&
        haySubsanablePendienteParaFlujo
    ) {
        /*
         * Novedad subsanable pendiente en Grupo 2:
         * Rechazar o Regresar.
         */
        puedeActualizarInconsistencias = false;
        puedeAprobarInconsistencias = false;
        puedeRechazarInconsistencias = true;
    } else if (
        actualizacionInconsistenciasCompletada
    ) {
        /*
         * NINGUNA, subsanable resuelta o causal
         * subsanada después de actualizar.
         */
        puedeActualizarInconsistencias = false;
        puedeAprobarInconsistencias = true;
        puedeRechazarInconsistencias = false;
    } else {
        /*
         * Primero se debe actualizar y luego aprobar.
         */
        puedeActualizarInconsistencias = true;
        puedeAprobarInconsistencias = false;
        puedeRechazarInconsistencias = false;
    }

    puedeRechazarInconsistencias =
        grupoInconsistenciaActual === 2 &&
        haySubsanablePendienteParaRechazar;

    // BLOQUEOS ENTIDADES

    const [
        entidadBloqueadaControlPreventivo,
        setEntidadBloqueadaControlPreventivo,
    ] = useState<boolean>(false);

    const [
        validandoBloqueoEntidad,
        setValidandoBloqueoEntidad,
    ] = useState<boolean>(false);

    const bloqueoEntidadActivo =
        grupoInconsistenciaActual === 2 &&
        (
            entidadBloqueadaControlPreventivo ||
            validandoBloqueoEntidad
        );


    const obtenerIdEntidadEducativaReal = (
        entidad: any,
    ): string => {
        const razonSocial = String(
            entidad?.razonSocial || '',
        ).trim();

        const partesRazonSocial =
            razonSocial.split('_');

        const posibleIdEntidad =
            partesRazonSocial[0] || '';

        const formatoUuid =
            /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

        if (
            posibleIdEntidad &&
            formatoUuid.test(posibleIdEntidad)
        ) {
            return posibleIdEntidad;
        }

        return String(
            entidad?.idEntidad || '',
        ).trim();
    };

    const obtenerOperacionMasReciente = (
        operaciones: any[],
    ) => {
        if (!Array.isArray(operaciones)) {
            return null;
        }

        if (operaciones.length === 0) {
            return null;
        }

        const operacionesOrdenadas = [
            ...operaciones,
        ].sort((a: any, b: any) => {
            const fechaA = new Date(
                a?.fechaBloqueo || 0,
            ).getTime();

            const fechaB = new Date(
                b?.fechaBloqueo || 0,
            ).getTime();

            return fechaB - fechaA;
        });

        return operacionesOrdenadas[0];
    };

    const entidadTieneBloqueoControlPreventivo = (
        operaciones: any[],
    ) => {
        const ultimaOperacion =
            obtenerOperacionMasReciente(operaciones);

        if (!ultimaOperacion) {
            return false;
        }

        const nombreOperacion = normalizarTexto(
            ultimaOperacion?.nombreOperacion,
        );

        const nombreCausal = normalizarTexto(
            ultimaOperacion?.nombreCausal,
        );

        const observaciones = normalizarTexto(
            ultimaOperacion?.observaciones,
        );

        const esBloqueo =
            nombreOperacion === 'bloqueo';

        const esControlPreventivo =
            nombreCausal.includes(
                'control preventivo',
            ) ||
            observaciones.includes(
                'control preventivo solicitudes',
            );

        return esBloqueo && esControlPreventivo;
    };

    const validarBloqueoEntidadesEducativas =
        async () => {
            const esGrupo2 =
                nombreTareaActual.includes(
                    'seguridad documental',
                ) &&
                nombreTareaActual.includes(
                    'grupo 2',
                );

            if (!esGrupo2) {
                setEntidadBloqueadaControlPreventivo(
                    false,
                );
                return;
            }

            const entidades =
                responseSolicitud
                    ?.solicitudTerceroEntidadEducativaDto;

            if (!Array.isArray(entidades)) {
                setEntidadBloqueadaControlPreventivo(
                    false,
                );

                return;
            }

            const idsEntidades = Array.from(
                new Set(
                    entidades
                        .map(
                            obtenerIdEntidadEducativaReal,
                        )
                        .filter(Boolean),
                ),
            );

            if (idsEntidades.length === 0) {
                setEntidadBloqueadaControlPreventivo(
                    false,
                );

                return;
            }

            setValidandoBloqueoEntidad(true);

            try {
                const resultados = await Promise.all(
                    idsEntidades.map(
                        async (idEntidad: string) => {
                            const bloqueos =
                                await apiEntidadesEducativas
                                    .GetBloqueos(idEntidad);



                            let operaciones: any[] = [];

                            if (Array.isArray(bloqueos)) {
                                operaciones = bloqueos;
                            } else if (
                                Array.isArray(bloqueos?.data)
                            ) {
                                operaciones = bloqueos.data;
                            } else if (
                                bloqueos?.data?.idBloqueo
                            ) {
                                operaciones = [
                                    bloqueos.data,
                                ];
                            } else if (
                                bloqueos?.idBloqueo
                            ) {
                                operaciones = [
                                    bloqueos,
                                ];
                            }

                            const tieneBloqueo =
                                entidadTieneBloqueoControlPreventivo(
                                    operaciones,
                                );



                            return tieneBloqueo;
                        },
                    ),
                );

                const existeEntidadBloqueada =
                    resultados.some(Boolean);



                setEntidadBloqueadaControlPreventivo(
                    existeEntidadBloqueada,
                );


            } catch (error) {
                console.error(
                    'Error validando bloqueo de entidades educativas:',
                    error,
                );

                setEntidadBloqueadaControlPreventivo(
                    false,
                );
            } finally {
                setValidandoBloqueoEntidad(false);
            }
        };

    const entidadesEducativasSolicitud =
        responseSolicitud
            ?.solicitudTerceroEntidadEducativaDto;

    let llaveEntidadesEducativas = '';

    if (
        Array.isArray(
            entidadesEducativasSolicitud,
        )
    ) {
        llaveEntidadesEducativas =
            entidadesEducativasSolicitud
                .map(
                    obtenerIdEntidadEducativaReal,
                )
                .filter(Boolean)
                .join('|');
    }

    useEffect(() => {
        validarBloqueoEntidadesEducativas();
    }, [
        responseSolicitud?.solicitudId,
        responseSolicitud?.nombreTarea,
        llaveEntidadesEducativas,
    ]);


    return (
        <>
            <CHPageTitleV2 {...titulosIcon} />
            <LoadingComponent key="loader" loading={loading} />
            <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
                grupo1={grupo1}
                userAsig={userAsig}
                userIngreso={userIngreso}
            />
            <Panel
                responseSolicitud={responseSolicitud}
                currentTabValue={currentTabValue}
                documentosAgregados={documentosAgregados}
                setEstadoSubsanar={setEstadoSubsanar}
                control={control}
                setValue={setValue}
                grupo1={grupo1}
                handleSubmit={handleSubmit}
                reset={reset}
                getValues={getValues}
                watch={watch}
                listadoInconsistencias={listadoInconsistencias}
                addInconsistencia={addInconsistencia}
                updateInconsistencias={updateInconsistencias}
                deleteInconsistencias={deleteInconsistencias}
                formularioObservaciones={formularioObservaciones}
                formulariosSolicitud={formulariosSolicitud}
                observaciones={observaciones as string}
                siEsAprobadaSolcitud={siEsAprobadaSolcitud}
                inconsistenciasSeleccionadas={inconsistenciasSeleccionadas}
                deshablitarNinguna={deshablitarNinguna}
                setDeshabilitarNinguna={setDeshabilitarNinguna}
                desHabilitarNingunaGrupo1={desHabilitarNingunaGrupo1}
                setCausalYNovedades={setCausalYNovedades}
                causalYNovedades={causalYNovedades}
                setMensajesEjecutados={setMensajesEjecutados}
                mensajesEjecutados={mensajesEjecutados}
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
                formularioInformacionComunicacion={formularioInformacionComunicacion}
                mostrarObservacionesSoloLecturaVerificacion={estaEnFlujoVerificacion}
                entidadBloqueadaControlPreventivo={
                    entidadBloqueadaControlPreventivo
                }
                validandoBloqueoEntidad={
                    validandoBloqueoEntidad
                }
                setOmitirAlertaCausalTemporalmente={
                    setOmitirAlertaCausalTemporalmente
                }

                matriculaBloqueadaActiva={
                    matriculaBloqueadaActiva
                }
            />
            {/* <BotoneraInformacionSolicitudComponent
                onItemGoBack={onItemGoBack}
                tipoAccion={tipoAccionNumber}
                onSubmitAprobar={onSubmitAprobar}
                onSubmitSubsanar={onSubmitSubsanar}
                onSubmitRechazar={onSubmitRechazar}
                onSubmitGenerarPago={undefined}
                onSubmit={onSubmit2}
                disabledActualizar={false}
                disabledAprobar={aprobar}
                disabledSubsanar={false}
                disabledRechazar={false}
                disabledRegresar={false}
                tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
                responseSolicitud={responseSolicitud}
                currentTabValue={currentTabValue}
                aprobarBloquear={undefined}
                hablitarActualiza={hablitarActualiza}
                deshabilitarBTNRechazarGp2={deshabilitarBTNRechazarGp2} deshalitarBotonesInconsistencia={false}
                userAsig={userAsig}
                grupo={grupo}
                userIngreso={userIngreso}
            /> */}
            <BotoneraInformacionSolicitudComponent
                onItemGoBack={onItemGoBack}
                tipoAccion={tipoAccionNumber}
                onSubmitAprobar={onSubmitAprobar}
                onSubmitSubsanar={onSubmitSubsanar}
                onSubmitRechazar={onSubmitRechazar}
                onSubmitGenerarPago={undefined}
                onSubmit={onSubmit2}
                disabledActualizar={
                    bloqueoEntidadActivo ||
                    !puedeActualizarInconsistencias
                }
                disabledAprobar={
                    bloqueoEntidadActivo ||
                    !puedeAprobarInconsistencias
                }
                disabledSubsanar
                disabledRechazar={
                    bloqueoEntidadActivo ||
                    !puedeRechazarInconsistencias
                }
                disabledRegresar={false}
                tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
                responseSolicitud={responseSolicitud}
                currentTabValue={currentTabValue}
                aprobarBloquear={undefined}
                hablitarActualiza={!puedeActualizarInconsistencias}
                deshabilitarBTNRechazarGp2={
                    !puedeRechazarInconsistencias
                }
                deshalitarBotonesInconsistencia={!hayNovedades}
                userAsig={userAsig}
                grupo={grupo}
                userIngreso={userIngreso}
                requiereActualizarParaAprobar
                handleSubmit={handleSubmit}
                onActualizacionCompletada={() => {
                    setActualizacionInconsistenciasCompletada(true);
                }}
            />
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    userState: state.users,
    catalogState: state.catalog
});

export default connect(mapStateToProps)(ProcesoSeguridadDocumental);