import {
    createStyles,
    Tabs,
    Theme,
    withStyles,
    makeStyles,
    Tab,
    Grid,
    Button
} from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { useForm, UseFormReturn } from 'react-hook-form';
import { sortBy } from 'lodash';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import { CatalogState } from '../../../reducers/catalog.reducer';
import NotificationService from '../../../services/notification.service';
import {
    ACTIVIDAD_POSTULACION,
    catalogPostulacionDto,
    ESTADO_POSTULACION,
    hecho,
    ID_ACTIVIDAD_POSTULACION,
    imputabilidad_servicio_disminucion_capacidad,
    imputabilidad_servicio_muerte,
    TABS_INDEX,
    TIPO_IMPUTABILIDAD,
    TIPO_SUBMODALIDAD
} from '../constants/fondoSolidaridad.constants';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import Calificacion from './Calificacion';
import CargueDocumentosPostulacion from './CargueDocumentosPostulacion';
import EntrevistaPostulacion from './EntrevistaPostulacion';
import ResumenPostulacion from './ResumenPostulacion';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import ApiCliente from '../../../services/api-clients.service';
import { SolicitudDto } from '../../../models/requests/solicitud-tarea.model';
import { LoadingComponent } from '../../../components/loading/loading.component';
import UtilService from '../../../utils/util.service';
import TrazabilidadPostulacion from './TrazabilidadPostulacion';
import { ModalDeclararInconsisntencia } from './components/ModalDeclararInconsistencia';
import InconsistenciasPostulacion from './InconsistenciasPostulacion';


const useStyles = makeStyles(() =>
    createStyles({
        columnButtonAdd: {
            textAlign: 'right',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        buttonInconsistencia: {
            width: '220px',
            marginLeft: '.5rem',
        },
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
        },
        buttonActividad: {
            background: '#7cd1f9',
            fontSize: '11.5px',
            color: '#595959',
        },
        buttonCancelarDos: {
            backgroundColor: '#efefef',
            fontSize: '11.5px',
            color: '#595959'
        },
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
            minWidth: 72,
            marginRight: theme.spacing(4),
            '&:hover': {
                color: '#FEC917',
                opacity: 1,
                borderRadius: '6px 6px 0 0'
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
    datosSolicitudPostulacion,
    inconsistenciaSolicitud,
    mostrarPestaniaEntrevista,
    setMostrarPestaniaEntrevista,
    mostrarPestaniaCalificacion,
    setMostrarPestaniaCalificacion,
}: {
    currentTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
    datosSolicitudPostulacion: any;
    inconsistenciaSolicitud: any[];
    mostrarPestaniaEntrevista: boolean;
    setMostrarPestaniaEntrevista: (value: any) => void;
    mostrarPestaniaCalificacion: boolean;
    setMostrarPestaniaCalificacion: (value: any) => void;
}) {

    if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
        datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO)) {
        setMostrarPestaniaEntrevista(true);
    }

    if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
        datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO)) {
        setMostrarPestaniaCalificacion(true);
    }

    return (
        <AntTabs
            value={currentTab}
            onChange={onTabChange}
            aria-label="ant example"
        >

            <AntTab
                label="Resumen"
                value={TABS_INDEX.RESUMEN}
            />

            <AntTab
                label="Cargue Documentos"
                value={TABS_INDEX.CARGUE_ARCHIVO}
            />
            {
                mostrarPestaniaEntrevista && (
                    <AntTab
                        label="Entrevista"
                        value={TABS_INDEX.ENTREVISTA}
                    />
                )
            }

            {
                mostrarPestaniaCalificacion && (
                    <AntTab
                        label="Calificación"
                        value={TABS_INDEX.CALIFICACION}
                    />
                )

            }
            <AntTab
                label="Trazabilidad"
                value={TABS_INDEX.TRAZABILIDAD}
            />
            {
                inconsistenciaSolicitud && inconsistenciaSolicitud.length > 0 && (
                    <AntTab
                        label="Inconsistencias"
                        value={TABS_INDEX.INCONSITENCIAS}
                    />
                )
            }
        </AntTabs>
    );
}

function Panel({
    currentTabValue,
    cliente,
    catalogState,
    tipoSolicitud,
    formularioResumenInformacionSolicitud,
    formularioObservacionCargueArchivos,
    formularioEntrevista,
    formularioCalificacion,
    formularioPostulacion,
    postulantesControl,
    datosSolicitudPostulacion,
    datosEntrevista,
    camposCargueDocumentosSoloLectura,
    camposVerificacionNovedadesSoloLectura,
    camposVerificacionEmbargosSoloLectura,
    camposVerificacionCuentaIndividualSoloLectura,
    camposEntrevistaSoloLectura,
    camposCalificacionSoloLectura,
    imputabilidadValue,
    nullImputabilidadValue,
    listaTiposIntegrantes,
    listaTiposDiscapacidad,
    listaTiposAyudaTecnica,
    listaPostulantes,
    inconsistenciaSolicitud,
    postulantesTable,
    totalPorcentajePostulacion,
    totalPorcentajeAportes,
    totalPorcentajeCesantias,
    mostrarCampoPorcentajeDisminucion,
    mostrarVerificarNovedades,
    mostrarVerificarEmbargos,
    mostrarVerificarCuentaIndividual,
    beneficiariosPostulantesPostulacion,
    consultarTiposIntegrantes,
    consultarTiposDiscapcidad,
    consultarTiposAyudaTecnica,
    onChangeImputabilidad,
    documentosAgregados,
    setListaPostulantes,
    setPostulantesTable,
    setTotalPorcentajePostulacion,
    setTotalPorcentajeAportes,
    setTotalPorcentajeCesantias,
    setBeneficiariosPostulantesPostulacion,
    consultarBenficiariosPostulacion,
    consultarCalificacion
}: {
    currentTabValue: number;
    cliente: any;
    catalogState: CatalogState;
    tipoSolicitud: number;
    formularioResumenInformacionSolicitud: any;
    formularioObservacionCargueArchivos: UseFormReturn<any>;
    formularioEntrevista: UseFormReturn<any>;
    formularioCalificacion: UseFormReturn<any>;
    formularioPostulacion: UseFormReturn<any>;
    postulantesControl: UseFormReturn<any>;
    datosSolicitudPostulacion: any;
    datosEntrevista: any;
    camposCargueDocumentosSoloLectura: boolean;
    camposVerificacionNovedadesSoloLectura: boolean;
    camposVerificacionEmbargosSoloLectura: boolean;
    camposVerificacionCuentaIndividualSoloLectura: boolean;
    camposEntrevistaSoloLectura: boolean;
    camposCalificacionSoloLectura: boolean;
    imputabilidadValue: catalogPostulacionDto | null;
    nullImputabilidadValue: boolean;
    listaTiposIntegrantes: any;
    listaTiposAyudaTecnica: any;
    listaTiposDiscapacidad: any;
    inconsistenciaSolicitud: any[];
    postulantesTable: Postulante[];
    listaPostulantes: Postulante[];
    totalPorcentajePostulacion: number;
    totalPorcentajeAportes: number;
    totalPorcentajeCesantias: number;
    mostrarCampoPorcentajeDisminucion: boolean;
    mostrarVerificarNovedades: boolean;
    mostrarVerificarEmbargos: boolean;
    mostrarVerificarCuentaIndividual: boolean;
    beneficiariosPostulantesPostulacion: Postulante[];
    consultarTiposIntegrantes: () => void;
    consultarTiposDiscapcidad: () => void;
    consultarTiposAyudaTecnica: () => void;
    onChangeImputabilidad: (event: any, imputabilidad: catalogPostulacionDto) => void;
    documentosAgregados: (documentos: any[]) => void;
    setListaPostulantes: (postulantes: Postulante[]) => void;
    setPostulantesTable: (value: Postulante[]) => void;
    setTotalPorcentajePostulacion: (value: number) => void;
    setTotalPorcentajeAportes: (value: number) => void;
    setTotalPorcentajeCesantias: (value: number) => void;
    setBeneficiariosPostulantesPostulacion: (value: Postulante[]) => void;
    consultarBenficiariosPostulacion: (idSolicitud: string) => void;
    consultarCalificacion: (idSolicitud: string) => void;
}) {

    switch (currentTabValue) {
        case TABS_INDEX.RESUMEN:
            return (
                <>
                    <ResumenPostulacion
                        cliente={cliente}
                        catalogState={catalogState}
                        tipoSolicitud={tipoSolicitud}
                        formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
                        formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
                        beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion}
                        datosSolicitudPostulacion={datosSolicitudPostulacion}
                        consultarBenficiariosPostulacion={consultarBenficiariosPostulacion}
                        setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion}
                    />
                </>
            );
        case TABS_INDEX.CARGUE_ARCHIVO:
            return (
                <>
                    <CargueDocumentosPostulacion
                        formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
                        datosSolicitudPostulacion={datosSolicitudPostulacion}
                        documentosAgregados={documentosAgregados}
                        camposCargueDocumentosSoloLectura={camposCargueDocumentosSoloLectura}
                        camposVerificacionNovedadesSoloLectura={camposVerificacionNovedadesSoloLectura}
                        camposVerificacionEmbargosSoloLectura={camposVerificacionEmbargosSoloLectura}
                        camposVerificacionCuentaIndividualSoloLectura={camposVerificacionCuentaIndividualSoloLectura}
                        mostrarVerificarNovedades={mostrarVerificarNovedades}
                        mostrarVerificarEmbargos={mostrarVerificarEmbargos}
                        mostrarVerificarCuentaIndividual={mostrarVerificarCuentaIndividual}
                    />
                </>
            );
        case TABS_INDEX.ENTREVISTA:
            return (
                <>
                    <EntrevistaPostulacion
                        datosSolicitudPostulacion={datosSolicitudPostulacion}
                        catalogState={catalogState}
                        formularioEntrevista={formularioEntrevista}
                        postulantesControl={postulantesControl}
                        datosEntrevista={datosEntrevista}
                        cliente={cliente}
                        listaTiposIntegrantes={listaTiposIntegrantes}
                        listaTiposDiscapacidad={listaTiposDiscapacidad}
                        listaTiposAyudaTecnica={listaTiposAyudaTecnica}
                        camposEntrevistaSoloLectura={camposEntrevistaSoloLectura}
                        setListaPostulantes={setListaPostulantes}
                        consultarTiposIntegrantes={consultarTiposIntegrantes}
                        consultarTiposDiscapcidad={consultarTiposDiscapcidad}
                        consultarTiposAyudaTecnica={consultarTiposAyudaTecnica}
                    />

                </>
            );
        case TABS_INDEX.CALIFICACION:
            return (
                <>
                    <Calificacion
                        datosSolicitudPostulacion={datosSolicitudPostulacion}
                        formularioCalificacion={formularioCalificacion}
                        formularioPostulacion={formularioPostulacion}
                        postulantesControl={postulantesControl}
                        formularioEntrevista={formularioEntrevista}
                        listaPostulantes={listaPostulantes}
                        onChangeImputabilidad={onChangeImputabilidad}
                        imputabilidadValue={imputabilidadValue}
                        cliente={cliente}
                        nullImputabilidadValue={nullImputabilidadValue}
                        camposCalificacionSoloLectura={camposCalificacionSoloLectura}
                        postulantesTable={postulantesTable}
                        totalPorcentajePostulacion={totalPorcentajePostulacion}
                        totalPorcentajeAportes={totalPorcentajeAportes}
                        totalPorcentajeCesantias={totalPorcentajeCesantias}
                        mostrarCampoPorcentajeDisminucion={mostrarCampoPorcentajeDisminucion}
                        consultarCalificacion={consultarCalificacion}
                        setPostulantesTable={setPostulantesTable}
                        setTotalPorcentajePostulacion={setTotalPorcentajePostulacion}
                        setTotalPorcentajeAportes={setTotalPorcentajeAportes}
                        setTotalPorcentajeCesantias={setTotalPorcentajeCesantias}
                    />
                </>
            );
        case TABS_INDEX.TRAZABILIDAD:
            return (
                <>
                    <TrazabilidadPostulacion
                        datosSolicitudPostulacion={datosSolicitudPostulacion}
                    />
                </>
            );
        case TABS_INDEX.INCONSITENCIAS:
            return (
                <>
                    <InconsistenciasPostulacion
                        inconsistenciaSolicitud={inconsistenciaSolicitud}
                    />
                </>
            );
        default:
            return null;
    }
}

const EtapasPostulacion = (props: propsType) => {

    const classes = useStyles();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const apiClientes = ApiCliente();
    const {
        cliente,
        catalogState,
        tipoSolicitud,
        formularioResumenInformacionSolicitud,
        formularioSolicitud,
        formularioRetiro,
        formularioPostulacion,
        formularioObservacionCargueArchivos,
        datosSolicitudPostulacion,
        formularioEntrevista,
        formularioCalificacion,
        mostrarCampoPorcentajeDisminucion,
        listaPostulantes,
        openDeclararInconsistencia,
        formularioInconsistencia,
        beneficiariosPostulantesPostulacion,
        imputabilidadValue,
        nullImputabilidadValue,
        datosCalificacion,
        setListaPostulantes,
        setActividadSolicitud,
        setEstadoSolicitud,
        setAgregarNuevaPostulacion,
        setMostrarCampoPorcentajeDisminucion,
        consultarConvocatorias,
        setAgregarSolicitudPostulacion,
        setSelectedData,
        consultarSolicitudes,
        actualizarPostulacionValorAfectaciones,
        validarExisteSolicitudPorId,
        generarInconsistencia,
        handleOpenModalInconsistencia,
        handleCloseModalInconsistencia,
        setBeneficiariosPostulantesPostulacion,
        consultarBenficiariosPostulacion,
        onChangeImputabilidad,
        consultarCalificacion
    } = props;

    const [currentTabValue, setCurrentTabValue] = useState(
        TABS_INDEX.RESUMEN,
    );
    const [mostrarPestaniaEntrevista, setMostrarPestaniaEntrevista] = useState<boolean>(false);
    const [mostrarPestaniaCalificacion, setMostrarPestaniaCalificacion] = useState<boolean>(false);
    const [listaTiposIntegrantes, setListaTiposIntegrantes] = useState<any[]>([]);
    const [listaTiposDiscapacidad, setListaTiposDiscapacidad] = useState<any[]>([]);
    const [listaTiposAyudaTecnica, setListaTiposAyudaTecnica] = useState<any[]>([]);
    const [inconsistenciaSolicitud, setInconsistenciaSolicitud] = useState<any[]>([]);

    // Calificación
    const [postulantesTable, setPostulantesTable] = useState<Postulante[]>([]);
    const [totalPorcentajePostulacion, setTotalPorcentajePostulacion] = useState<number>(0);
    const [totalPorcentajeAportes, setTotalPorcentajeAportes] = useState<number>(0);
    const [totalPorcentajeCesantias, setTotalPorcentajeCesantias] = useState<number>(0);
    const [datosEntrevista, setDatosEntrevista] = useState<any>(null);

    const camposCargueDocumentosSoloLectura =
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO);

    const camposVerificacionNovedadesSoloLectura =
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE);

    const camposVerificacionEmbargosSoloLectura =
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE);

    const camposVerificacionCuentaIndividualSoloLectura =
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE);

    const camposEntrevistaSoloLectura =
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO);

    const camposCalificacionSoloLectura =
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.ANULADO) ||
        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.RECHAZADO);

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        setCurrentTabValue(newValue);
    };

    const [loading, setLoading] = useState(false);
    const [documentosAguardar, setDocumentosAguardar] = useState<any>([]);

    const documentosAgregados = (
        documentos: any,
    ): void => {
        setDocumentosAguardar(documentos);
    };

    // const [datosCalificacion, setDatosCalificacion] = useState<any>(null);
    // const [imputabilidadValue, setImputabilidadValue] = useState<catalogPostulacionDto | null>(null);
    // const [nullImputabilidadValue, setNullImputabilidadValue] = React.useState(false);

    const [mostrarVerificarNovedades, setMostrarVerificarNovedades] = useState(false);
    const [mostrarVerificarEmbargos, setMostrarVerificarEmbargos] = useState(false);
    const [mostrarVerificarCuentaIndividual, setMostrarVerificarCuentaIndividual] = useState(false);

    const postulantesControl = useForm({
        defaultValues: {
            id: '',
            idSolicitud: '',
            tipoIntegrante: '',
            nombre: '',
            apellido: '',
            tipoDocumentoId: '',
            numeroDocumento: '',
            departamentoExpedicion: '',
            ciudadExpedicion: '',
            parentesco: '',
            sexo: '',
            fechaNacimiento: new Date(),
            estadoCivil: '',
            ocupacion: '',
            nivelEducativo: '',
            afectionFisica: '',
            afectacionSensorial: '',
            enfermedadEspecifica: '',
            tratamientoMedico: '',
            dificultadEscaleras: '',
            medicoPermanente: '',
            discapacidad: '',
            ayudaTecnica: '',
            tipoAyudaTecnica: '',
            condicionSalud: '',
            porcentajePostulacion: '',
            porcentajeAportes: '',
            porcentajeCesantias: '',
        },
    });

    useEffect(() => {
        consultarEntrevista(datosSolicitudPostulacion.idSolicitud);
        consultarCalificacion(datosSolicitudPostulacion.idSolicitud);
        consultarObservacionCargueArchivo(datosSolicitudPostulacion.idSolicitud);
        consultarObservacionesVerificacion();
        consultarInconsistenciaSolicitud(datosSolicitudPostulacion.idSolicitud);
        // Cargue documentos inconsistente
        if (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) {
            formularioObservacionCargueArchivos.setValue('observacion', '');
        }
    }, [datosSolicitudPostulacion, cliente]);

    // const onChangeImputabilidad = (event: any, imputabilidad: any) => {
    //     setNullImputabilidadValue(imputabilidad == null);
    //     setImputabilidadValue(imputabilidad);
    //     if (imputabilidad) {
    //         formularioCalificacion.setValue('imputabilidadAlServicio', String(imputabilidad.id));
    //         formularioCalificacion.setValue('imputabilidadServicioDescripcion', imputabilidad.descripcion);
    //         formularioCalificacion.clearErrors('imputabilidadAlServicio');
    //     }
    // };

    const consultarInconsistenciaSolicitud = async (idSolicitud: string) => {
        await apiFondoSolidaridad
            .getConsultarInconsistenciaSolicitud(idSolicitud)
            .then(async response => {
                if (response?.data && response?.data?.length > 0) {
                    setInconsistenciaSolicitud(response?.data);
                }
            })
            .catch((e) => {
                toast(`Error al consultar convocatorias ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const consultarTiposIntegrantes = async () => {
        await apiFondoSolidaridad
            .getObtenerTiposIntegrantes()
            .then(async (response: any) => {
                if (response.data) {
                    setListaTiposIntegrantes(response?.data);
                }
            })
            .catch(() => {
                toast(`Error al cargar lista de tipos de integrantes`, { type: toast.TYPE.ERROR });
            });
    };

    const consultarTiposDiscapcidad = async () => {
        await apiFondoSolidaridad
            .getObtenerTiposDiscapacidad()
            .then(async (response: any) => {
                if (response.data) {
                    const datosOrdenados = sortBy(
                        response.data,
                        'descripcion',
                    );
                    setListaTiposDiscapacidad(datosOrdenados);
                }
            })
            .catch(() => {
                toast(`Error al cargar lista de tipos de discapacidad`, { type: toast.TYPE.ERROR });
            });
    };

    const consultarTiposAyudaTecnica = async () => {
        await apiFondoSolidaridad
            .getObtenerTiposAyudaTecnica()
            .then(async (response: any) => {
                if (response.data) {
                    const datosOrdenados = sortBy(
                        response.data,
                        'descripcion',
                    );
                    setListaTiposAyudaTecnica(datosOrdenados);
                }
            })
            .catch(e => {
                toast(`Error al cargar lista de tipos de ayuda técnica`, { type: toast.TYPE.ERROR });
            });
    };

    const actualizarValorAfectaciones = () => {
        const informacionPostulacion = {
            idSolicitud: datosSolicitudPostulacion.idSolicitud,
            tipoDocumentoId: datosSolicitudPostulacion.tipoDocumentoId,
            numeroDocumeto: Number(datosSolicitudPostulacion.numeroDocumeto),
            idTipoSolicitud: Number(datosSolicitudPostulacion.idTipoSolicitud),
            idModalidad: Number(datosSolicitudPostulacion.idModalidad),
            idSubModalidad: Number(datosSolicitudPostulacion.idSubModalidad),
            manifestacionAutorizacion: datosSolicitudPostulacion.manifestacionAutorizacion === true ? 1 : 0,
            departamento1: Number(datosSolicitudPostulacion.departamento1),
            ciuadad1: Number(datosSolicitudPostulacion.ciuadad1),
            departameno2: Number(datosSolicitudPostulacion.departameno2),
            ciudad2: Number(datosSolicitudPostulacion.ciudad2),
            deparmento3: Number(datosSolicitudPostulacion.deparmento3),
            ciudad3: Number(datosSolicitudPostulacion.ciudad3),
            firmaSolicitud: Number(datosSolicitudPostulacion.firmaSolicitud),
            numeroDocumentoFirma: Number(datosSolicitudPostulacion.numeroDocumentoFirma),
            numeroCelular: Number(datosSolicitudPostulacion.numeroCelular),
            numeroConvocatoria: Number(datosSolicitudPostulacion.numeroConvocatoria),
            observacion: datosSolicitudPostulacion.observacion,
            nombreFirma: datosSolicitudPostulacion.nombreFirma,
            apeliidoFirma: datosSolicitudPostulacion.apellidoFirma,
            representanteLegal: datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA ? datosSolicitudPostulacion.representanteLegal : false,
            spoUnidadEjecutora: datosSolicitudPostulacion.unidadEjecutora,
            spoCategoria: datosSolicitudPostulacion.categoria,
            nombreCompletoAfiliado: datosSolicitudPostulacion.nombreAfiliado,
            afectacionCuenta: datosSolicitudPostulacion.afectacionCuenta,
            valorAfectacion: UtilService.cleanCurrencyValue(formularioPostulacion.watch('valorAfectaciones')),
        }
        actualizarPostulacionValorAfectaciones(informacionPostulacion);
        validarExisteSolicitudPorId(datosSolicitudPostulacion?.noRadicado);
    }

    const actualizarCargueDocumentos = async (data: any) => {

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {

            if (currentTabValue === TABS_INDEX.CARGUE_ARCHIVO) {

                const requestFolios = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    numeroFolios: Number(data.numeroFolios)
                }
                await apiFondoSolidaridad
                    .postActualizarNumeroFoliosCargueArchivos(requestFolios)
                    .catch(() => {
                        setLoading(false);
                        toast('Error al actualizar numero de folios cargue documentos', { type: toast.TYPE.ERROR });
                    });

                const requestFoliosDodo = {
                    radicado: datosSolicitudPostulacion?.noRadicado,
                    numero_Folio: Number(data.numeroFolios)
                }

                await apiFondoSolidaridad
                    .postActualizarFoliosDodo(requestFoliosDodo)
                    .catch(() => {
                        setLoading(false);
                        toast('Error al actualizar numero de folios en Dodo', { type: toast.TYPE.ERROR });
                    });

                const request = {
                    solicitudId: datosSolicitudPostulacion.idSolicitud,
                    observacion: data.observacion
                }
                await apiFondoSolidaridad
                    .postActualizarObservacionCargueDocumentos(request)
                    .then(async response => {
                        if (response) {
                            if (data.observacionVerArchivos) {
                                const requestVerificacion = {
                                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                    observacion: data.observacionVerArchivos
                                }
                                await apiFondoSolidaridad
                                    .postActualizarVerificacion(requestVerificacion)
                                    .then(async response => {
                                        if (response) {
                                            setLoading(false);
                                            await NotificationService.genericAlert(
                                                'Actualizado con éxito',
                                                'Se realizó con éxito la actualización.',
                                            );
                                        }
                                    })
                                    .catch(() => {
                                        setLoading(false);
                                        toast('Error al actualizar observacion verificacion', { type: toast.TYPE.ERROR });
                                    });
                            } else {
                                setLoading(false);
                                await NotificationService.genericAlert(
                                    'Actualizado con éxito',
                                    'Se realizó con éxito la actualización.',
                                )
                            }
                        }
                    })
                    .catch(() => {
                        setLoading(false);
                        toast('Error al actualizar observacion documentos', { type: toast.TYPE.ERROR });
                    });
            }
        }
    }

    const consultarObservacionesVerificacion = async () => {
        await apiFondoSolidaridad
            .getObtenerInformacionTrazabilidad(datosSolicitudPostulacion.idSolicitud)
            .then(async (response: any) => {
                if (response.data) {
                    response.data.forEach((element: any) => {
                        if (element.activity === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES) {
                            setMostrarVerificarNovedades(true);
                            formularioObservacionCargueArchivos.setValue('observacionVerificacionNovedades', element.observation);
                        }
                        if (element.activity === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS) {
                            setMostrarVerificarEmbargos(true);
                            formularioObservacionCargueArchivos.setValue('observacionVerificacionEmbargos', element.observation);
                        }
                        if (element.activity === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL) {
                            setMostrarVerificarCuentaIndividual(true);
                            formularioObservacionCargueArchivos.setValue('observacionVerificacionCuentaIndividual', element.observation);
                        }
                    });
                }
            })
            .catch(e => {
                setLoading(false);
                toast(`Error al consultar información trazabilidad en cargue documentos ${e.message}`, { type: toast.TYPE.ERROR });
            });
    }

    const actualizarVerificacion = async (data: any) => {

        let requestVerificacion;
        let observacionIngresada = false;
        setLoading(true);

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
            if (data.observacionVerificacionNovedades) {
                requestVerificacion = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    observacion: data.observacionVerificacionNovedades,
                    idEtapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES,
                }
                observacionIngresada = true;
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
            if (data.observacionVerificacionEmbargos) {
                requestVerificacion = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    observacion: data.observacionVerificacionEmbargos,
                    idEtapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS,
                }
                observacionIngresada = true;
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
            if (data.observacionVerificacionCuentaIndividual) {
                requestVerificacion = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    observacion: data.observacionVerificacionCuentaIndividual,
                    idEtapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL,
                }
                observacionIngresada = true;
            }
        }

        if (observacionIngresada) {
            await apiFondoSolidaridad
                .postActualizarVerificacion(requestVerificacion)
                .then(async response => {
                    if (response) {
                        setLoading(false);
                        consultarObservacionesVerificacion();
                        await NotificationService.genericAlert(
                            'Actualizado con éxito',
                            'Se realizó con éxito la actualización.',
                        );
                    }
                })
                .catch((e) => {
                    setLoading(false);
                    toast(`Error en actualizar verificación ${e.message} `, { type: toast.TYPE.ERROR });
                });
        } else {
            setLoading(false);
        }
    }

    const actualizarEntrevista = async (data: any) => {
        setLoading(true);

        if (listaPostulantes) {
            if (listaPostulantes.length === 0) {
                toast('Debe registrar al menos un postulante', { type: toast.TYPE.ERROR });
                setLoading(false);
                return;
            }

            let faltaIngresarDatos = false;
            if (listaPostulantes.length >= 0) {
                listaPostulantes.forEach(item => {
                    if (item && !item.condicionSalud) {
                        faltaIngresarDatos = true;
                    }
                })
            }

            if (faltaIngresarDatos) {
                toast('Falta ingresar información de los postulantes', { type: toast.TYPE.ERROR });
            }
        }

        const request = {
            efS_ID: data.efS_ID,
            efS_ID_SOLICITUD: datosSolicitudPostulacion.idSolicitud,
            efS_VIVIENDA_PROPIA: data.efS_VIVIENDA_PROPIA,
            efS_TIPO_VIVIENDA: data.efS_TIPO_VIVIENDA,
            efS_HABITANTE: Number(data.efS_HABITANTE),
            efS_INFORMATIVO_ADMIN: data.efS_INFORMATIVO_ADMIN,
            efS_CARACTERIZACION: data.efS_CARACTERIZACION,
            efS_CONCEPTO_PSICOSOCIAL: data.efS_CONCEPTO_PSICOSOCIAL,
            efS_OBSERVACION: data.efS_OBSERVACION,
            postulantes: listaPostulantes,
        }
        await apiFondoSolidaridad
            .postActualizarEntrevista(request)
            .then(async response => {
                if (response.returnMessage === "Exitoso") {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Actualizado con éxito',
                        'Se realizó con éxito la actualización.',
                    );
                }
            }).catch((e) => {
                setLoading(false);
                toast(`Error en actualizar entrevista ${e.message}`, { type: toast.TYPE.ERROR });
            });
        consultarEntrevista(datosSolicitudPostulacion.idSolicitud);
    };

    const funcionPorcentajeDisminucion = (hecho: any, porcentajeDisminucion: any) => {
        const MUERTE_DEL_AFILIADO = '100';
        let porcentaje = 0;
        if (hecho === MUERTE_DEL_AFILIADO) {
            porcentaje = 100;
        } else if (hecho === '0' && typeof porcentajeDisminucion === 'string' && porcentajeDisminucion.includes('%')) {
            porcentaje = Number(porcentajeDisminucion.replace('%', ''));
        } else if (hecho === '0' && typeof porcentajeDisminucion === 'number') {
            porcentaje = porcentajeDisminucion;
        }
        return porcentaje;
    }

    const actualizarCalificacion = async (data: any) => {
        setLoading(true);

        const hechoSeleccionado = hecho.find(item => item.id === data.hecho);
        const request = {
            calificacionId: data.calificacionId,
            solicitudId: datosSolicitudPostulacion.idSolicitud,
            tipoImputabilidad: data.tipoImputabilidad,
            imputabilidadServicioDescripcion: data.imputabilidadServicioDescripcion,
            imputabilidadAlServicio: Number(data.imputabilidadAlServicio),
            porcentajePostulacion: Number(data.porcentajePostulacion),
            hecho: Number(data.hecho),
            disfrutePension: data.disfrutePension,
            datosResolucion: data.datosResolucion,
            observacion: data.observacion,
            hechoDescripcion: hechoSeleccionado?.descripcion,
            porcentajeDisminucion: funcionPorcentajeDisminucion(data.hecho, data.porcentajeDisminucion),
            descripcion: data.solicitudesPrevias,
        }
        await apiFondoSolidaridad
            .postActualizarCalificacion(request)
            .then(async response => {
                if (response.data) {
                    await NotificationService.genericAlert(
                        'Actualizado con éxito',
                        'Se realizó con éxito la actualización.',
                    );
                    await consultarCalificacion(datosSolicitudPostulacion.idSolicitud);
                    setLoading(false);
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error actualizar calificación ${e.message}`, { type: toast.TYPE.ERROR });
            });
        actualizarValorAfectaciones();
    }

    const consultarObservacionCargueArchivo = async (idSolicitud: string) => {
        await apiFondoSolidaridad
            .getConsultarObservacionCargueArchivo(idSolicitud)
            .then(async response => {
                if (response.data) {
                    formularioObservacionCargueArchivos.setValue('numeroFolios', response?.data?.numeroFolio === 0 ? '' : String(response?.data?.numeroFolio));
                    formularioObservacionCargueArchivos.setValue('observacion', response?.data?.observacion);
                }
            })
            .catch((e) => {
                toast(`Error consultar Observación cargue de archivo ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const consultarEntrevista = async (idSolicitud: string) => {
        setLoading(true);
        await apiFondoSolidaridad
            .getConsultarEntrevista(idSolicitud)
            .then(async response => {
                if (response.data) {
                    setListaPostulantes(response.data.postulantes);
                    setDatosEntrevista(response.data);
                    formularioEntrevista.setValue('efS_ID', response?.data?.efS_ID);
                    formularioEntrevista.setValue('efS_ID_SOLICITUD', response?.data?.efS_ID_SOLICITUD);
                    formularioEntrevista.setValue('efS_TIPO_VIVIENDA', response?.data?.efS_TIPO_VIVIENDA);
                    formularioEntrevista.setValue('efS_VIVIENDA_PROPIA', response?.data?.efS_VIVIENDA_PROPIA);
                    formularioEntrevista.setValue('efS_HABITANTE', response?.data?.efS_HABITANTE === 0 ? '' : response?.data?.efS_HABITANTE);
                    formularioEntrevista.setValue('efS_INFORMATIVO_ADMIN', response?.data?.efS_INFORMATIVO_ADMIN);
                    formularioEntrevista.setValue('efS_CONCEPTO_PSICOSOCIAL', response?.data?.efS_CONCEPTO_PSICOSOCIAL);
                    formularioEntrevista.setValue('efS_OBSERVACION', response?.data?.efS_OBSERVACION);
                    setLoading(false);
                } else {
                    setLoading(false);
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error del sistema Consultar entrevista ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    // const consultarCalificacion = async (idSolicitud: string) => {

    //     setLoading(true);
    //     const HECHO_MUERTE_DEL_AFILIADO = 'MUERTE DEL AFILIADO';
    //     let hechoDescripcion;
    //     await apiFondoSolidaridad
    //         .getConsultarCalificacion(idSolicitud)
    //         .then(async response => {
    //             if (response.data) {
    //                 if (response?.data?.hechoDescripcion === null) {
    //                     hechoDescripcion = '';
    //                 } else if (response?.data?.hechoDescripcion === HECHO_MUERTE_DEL_AFILIADO) {
    //                     hechoDescripcion = '100';
    //                 } else {
    //                     hechoDescripcion = '0';
    //                 }
    //                 formularioCalificacion.setValue('calificacionId', response?.data?.calificacionId);
    //                 formularioCalificacion.setValue('solicitudId', response?.data?.solicitudId);
    //                 formularioCalificacion.setValue('tipoImputabilidad', response?.data?.tipoImputabilidad);
    //                 formularioCalificacion.setValue('imputabilidadAlServicio', response?.data?.imputabilidadAlServicio);
    //                 formularioCalificacion.setValue('hecho', hechoDescripcion);
    //                 formularioCalificacion.setValue('datosResolucion', response?.data?.datosResolucion);
    //                 formularioCalificacion.setValue('estadoId', response?.data?.estadoId);
    //                 formularioCalificacion.setValue('actividad', response?.data?.actividad);
    //                 if (response?.data?.porcentajePostulacion === 0) {
    //                     formularioCalificacion.setValue('porcentajePostulacion', '');
    //                 } else {
    //                     formularioCalificacion.setValue('porcentajePostulacion', response?.data?.porcentajePostulacion === 100 ? response?.data?.porcentajePostulacion : 50);
    //                 }
    //                 formularioCalificacion.setValue('observacion', response?.data?.observacion);
    //                 // porcentaje disminucion
    //                 formularioCalificacion.setValue('porcentajeDisminucion', response?.data?.porcentajeDisminucion);
    //                 if (response?.data?.porcentajeDisminucion === 100) {
    //                     setMostrarCampoPorcentajeDisminucion(false);
    //                 } else {
    //                     setMostrarCampoPorcentajeDisminucion(true);
    //                 }

    //                 if (response?.data?.disfrutePension === null) {
    //                     formularioCalificacion.setValue('disfrutePension', datosSolicitudPostulacion?.disfrutePension);
    //                 } else if (response?.data?.disfrutePension !== null) {
    //                     formularioCalificacion.setValue('disfrutePension', response?.data?.disfrutePension);
    //                 }
    //                 setDatosCalificacion(response?.data);

    //                 // Solicitudes Previas
    //                 if (cliente && cliente?.solicitudes && cliente?.solicitudes.length > 0) {

    //                     const solicitudesPrevias = cliente?.solicitudes.filter((solicitud: any) =>
    //                         (solicitud.tipoSolicitud === 'FONDO SOLIDARIDAD 1305' ||
    //                             solicitud.tipoSolicitud === 'FONDO SOLIDARIDAD ACUERDO 01/2016' ||
    //                             solicitud.tipoSolicitud === 'GIRO DE RECURSOS FONDO SOLIDARIDAD' ||
    //                             solicitud.tipoSolicitud === 'GIRO DE RECURSOS FONDO SOLIDARIDAD ACUERDO 01/2016' ||
    //                             solicitud.tipoSolicitud === 'GIRO RECURSOS FONDO SOLIDARIDAD 1305' ||
    //                             solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD' ||
    //                             solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD ACUERDO 01/2016' ||
    //                             solicitud.tipoSolicitud === 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD' ||
    //                             solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD' ||
    //                             solicitud.tipoSolicitud === 'POSTULACIÓN FONDO SOLIDARIDAD')
    //                         && new Date(solicitud.fechaInicioSolicitud) < new Date(datosSolicitudPostulacion?.fechaCreacion));

    //                     if (solicitudesPrevias && solicitudesPrevias.length === 1) {
    //                         formularioCalificacion.setValue('solicitudesPrevias', solicitudesPrevias?.tipoSolicitud);
    //                     } else if (solicitudesPrevias && solicitudesPrevias.length > 1) {
    //                         const tipoSolicitudesUnicas: string[] = Array.from(
    //                             new Set(solicitudesPrevias.map((item: any) => item.tipoSolicitud))
    //                         );
    //                         const descripcionSolicitudesPrevias: string = tipoSolicitudesUnicas.join(', ');
    //                         formularioCalificacion.setValue('solicitudesPrevias', descripcionSolicitudesPrevias);
    //                     } else {
    //                         formularioCalificacion.setValue('solicitudesPrevias', 'N/A');
    //                     }
    //                 } else {
    //                     formularioCalificacion.setValue('solicitudesPrevias', 'N/A');
    //                 }

    //                 if (datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
    //                     datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.MECANISMOS_ESPECIALES) {
    //                     // Tipo imputabilidad
    //                     formularioCalificacion.setValue('tipoImputabilidad', TIPO_IMPUTABILIDAD.DISMINUCION);
    //                     formularioCalificacion.setValue('hecho', '0');
    //                     setMostrarCampoPorcentajeDisminucion(true);

    //                     // Imputabilidad al servicio 
    //                     const imputabilidadObject = imputabilidad_servicio_disminucion_capacidad.find(item => item?.id === response?.data?.imputabilidadAlServicio);
    //                     if (imputabilidadObject) {
    //                         setImputabilidadValue(imputabilidadObject || null);
    //                         setNullImputabilidadValue(false);
    //                         formularioCalificacion.setValue(
    //                             'imputabilidadAlServicio',
    //                             imputabilidadObject?.id,
    //                         );
    //                     }
    //                 } else if (datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA) {
    //                     // Tipo imputabilidad
    //                     formularioCalificacion.setValue('tipoImputabilidad', TIPO_IMPUTABILIDAD.MUERTE);
    //                     formularioCalificacion.setValue('hecho', '100');
    //                     setMostrarCampoPorcentajeDisminucion(false);

    //                     // Imputabilidad al servicio 
    //                     const imputabilidadObject = imputabilidad_servicio_muerte.find(item => item?.id === response?.data?.imputabilidadAlServicio);
    //                     if (imputabilidadObject) {
    //                         setImputabilidadValue(imputabilidadObject || null);
    //                         setNullImputabilidadValue(false);
    //                         formularioCalificacion.setValue(
    //                             'imputabilidadAlServicio',
    //                             imputabilidadObject?.id,
    //                         );
    //                     }
    //                 }
    //                 setLoading(false);
    //             } else {
    //                 setLoading(false);
    //             }

    //         })
    //         .catch(() => {
    //             setLoading(false);
    //             toast('Error al consultar calificación', { type: toast.TYPE.ERROR });
    //         });
    // };

    const aprobar = async () => {

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)
        ) {
            let archivosRequeridos = 0;
            documentosAguardar.forEach((item: any) => {
                if (item.requerido === 1) {
                    archivosRequeridos += 1;
                }
            })

            let archivosRequeridosCargados = 0;

            documentosAguardar.forEach((item: any) => {
                if (item.requerido === 1 && item.verificado === true) {
                    archivosRequeridosCargados += 1;
                }
            })

            if (documentosAguardar.length === 0 ||
                (archivosRequeridos > 0 &&
                    archivosRequeridosCargados > 0 &&
                    archivosRequeridos !== archivosRequeridosCargados)) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se deben cargar los archivos requeridos`,
                    'warning'
                );
                return;
            }

            let pendienteGuardarCampos = true;

            await apiFondoSolidaridad
                .getConsultarObservacionCargueArchivo(datosSolicitudPostulacion.idSolicitud)
                .then(async response => {
                    if (response?.data?.numeroFolio === 0) {
                        pendienteGuardarCampos = true;
                    } else if (response?.data?.observacion === '') {
                        pendienteGuardarCampos = true;
                    } else if (response?.data?.numeroFolio > 0 && response?.data?.observacion === '') {
                        pendienteGuardarCampos = true;
                    } else if (response?.data?.numeroFolio === 0 && response?.data?.observacion !== '') {
                        pendienteGuardarCampos = false;
                    } else {
                        pendienteGuardarCampos = false;
                    }
                })
                .catch((e) => {
                    toast(`Error consultar Observación cargue de archivo en aprobar etapas ${e.message}`, { type: toast.TYPE.ERROR });
                });

            if (pendienteGuardarCampos) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se deben diligenciar y actualizar los campos requeridos `,
                    'warning'
                );
                return;
            }

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {
                let etapa = ID_ACTIVIDAD_POSTULACION.ENTREVISTA;
                const SOLUCION_DE_VIVIENDA = 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA';

                // VERIFICACIÓN DE NOVEDADES

                // Si las cuentas se encuentran inactivas
                let numeroDeCuentas = 0;
                cliente?.cuentas?.forEach((item: any) => {
                    if (item.estadoCuenta === "INACTIVA") {
                        numeroDeCuentas += 1;
                    }
                })

                if (cliente?.cuentas.length === numeroDeCuentas) {
                    etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                }

                // Si las cuentas son Solución de Vivienda y el Estado es PRECANCELADA
                let numeroDeCuentasPrecanceladas = 0;
                cliente?.cuentas?.forEach((item: any) => {
                    if (item.tipoCuenta === SOLUCION_DE_VIVIENDA && item.estadoCuenta === "PRECANCELADA") {
                        numeroDeCuentasPrecanceladas += 1;
                    }
                })

                if (cliente?.cuentas.length === numeroDeCuentasPrecanceladas) {
                    etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                }

                if (cliente?.solicitudes && cliente?.solicitudes.length > 0) {

                    const solicitudDesafiliacion = cliente?.solicitudes?.some((item: any) => item.tipoSolicitud === 'DESAFILIACION' && item.estadoSolicitud === 'PROCESADA');
                    const solicitudDesafiliacionAportes = cliente?.solicitudes?.some((item: any) => item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' && item.estadoSolicitud === 'PROCESADA');
                    const solicitudDesafiliacionCesantias = cliente?.solicitudes?.some((item: any) => item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA CON CESANTÍAS' && item.estadoSolicitud === 'PROCESADA');
                    const reintegros = cliente?.solicitudes?.some((item: any) => item.tipoSolicitud === 'REINTEGROS' && item.modalidad === 'REINTEGRO POR DESAFILIACIÓN');

                    if (solicitudDesafiliacion) {
                        if (new Date(cliente?.fechaPrimeraCuota) > new Date(reintegros?.fechaInicioSolicitud)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        } else if (reintegros && new Date(solicitudDesafiliacion?.fechaInicioSolicitud) < new Date(reintegros?.fechaInicioSolicitud)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                        else if (new Date(cliente?.fechaPrimeraCuota) < new Date(datosSolicitudPostulacion?.fechaRetiroFuerza)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                        else if (cliente?.solicitudes?.some((item: any) => (item.tipoSolicitud === 'CREACION DE CUENTA' || item.tipoSolicitud === 'AFILIACION') && item.estadoSolicitud === 'PROCESADA')) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                    }

                    if (solicitudDesafiliacionAportes) {
                        if (new Date(cliente?.fechaPrimeraCuota) > new Date(reintegros?.fechaInicioSolicitud)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        } else if (reintegros && new Date(solicitudDesafiliacionAportes?.fechaInicioSolicitud) < new Date(reintegros?.fechaInicioSolicitud)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                        else if (new Date(cliente?.fechaPrimeraCuota) < new Date(datosSolicitudPostulacion?.fechaRetiroFuerza)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                        else if (cliente?.solicitudes?.some((item: any) => (item.tipoSolicitud === 'CREACION DE CUENTA' || item.tipoSolicitud === 'AFILIACION') && item.estadoSolicitud === 'PROCESADA')) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                    }

                    if (solicitudDesafiliacionCesantias) {
                        if (new Date(cliente?.fechaPrimeraCuota) > new Date(reintegros?.fechaInicioSolicitud)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        } else if (reintegros && new Date(solicitudDesafiliacionCesantias?.fechaInicioSolicitud) < new Date(reintegros?.fechaInicioSolicitud)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                        else if (new Date(cliente?.fechaPrimeraCuota) < new Date(datosSolicitudPostulacion?.fechaRetiroFuerza)) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                        else if (cliente?.solicitudes?.some((item: any) => (item.tipoSolicitud === 'CREACION DE CUENTA' || item.tipoSolicitud === 'AFILIACION') && item.estadoSolicitud === 'PROCESADA')) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                    }

                    // BLOQUEO POR NOVEDADES EN HEROES
                    if (datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
                        datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA) {
                        if (new Date(datosSolicitudPostulacion?.fechaRetiroFuerza) >= new Date('Jul 21 2005') &&
                            new Date(datosSolicitudPostulacion?.fechaRetiroFuerza) <= new Date('Jun 02 2009') &&
                            datosSolicitudPostulacion?.disfrutePension === 'SI' &&
                            new Date(datosSolicitudPostulacion?.fechaAsignacionInvalidez) <= new Date('Jun 03 2009') &&
                            new Date(cliente?.fechaPrimeraCuota) < new Date('May 03 2009') &&
                            cliente?.numeroCuotas > 0 &&
                            cliente?.solicitudes?.some((solicitud: any) => solicitud?.tipoSolicitud === 'AFILIACION' && (solicitud?.modalidad === 'EXTEMPORANEA' || solicitud?.modalidad === 'VOLUNTARIA')) &&
                            cliente?.cuentas && cliente?.cuentas.length > 0 && cliente?.cuentas?.some((cuenta: any) => cuenta?.estadoCuenta === 'VIGENTE' || cuenta?.estadoCuenta === 'MOROSA' || cuenta?.estadoCuenta === 'INACTIVA' || cuenta?.estadoCuenta === 'PRECANCELADA')) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                    }

                    // BLOQUEO POR NOVEDADES EN PROYECTOS Y MECANISMOS ESPECIALES
                    if (datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.MECANISMOS_ESPECIALES) {
                        if (new Date(datosSolicitudPostulacion?.fechaRetiroFuerza) < new Date('Jul 21 2005') &&
                            datosSolicitudPostulacion?.disfrutePension === 'SI' &&
                            new Date(datosSolicitudPostulacion?.fechaAsignacionInvalidez) < new Date('Jul 21 2005') &&
                            datosSolicitudPostulacion?.mismoAfiliado &&
                            new Date(cliente?.fechaPrimeraCuota) >= new Date('Jun 03 2009') &&
                            cliente?.numeroCuotas >= 24 &&
                            cliente?.solicitudes?.some((solicitud: any) => solicitud?.tipoSolicitud === 'AFILIACION' && (solicitud?.modalidad === 'EXTEMPORANEA' || solicitud?.modalidad === 'VOLUNTARIA')) &&
                            cliente?.cuentas && cliente?.cuentas.length > 0 && cliente?.cuentas?.some((cuenta: any) => cuenta?.estadoCuenta === 'MOROSA' || cuenta?.estadoCuenta === 'INACTIVA' || cuenta?.estadoCuenta === 'PRECANCELADA')) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                        }
                    }

                    // Estado del cliente
                    if (cliente?.estadoCliente === 'BLOQUEADO' ||
                        cliente?.estadoCliente === 'DESVINCULADO') {
                        etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                    }

                    // Vivienda 8 (MASVI)
                    const vivienda8 = cliente?.solicitudes?.some((solicitud: any) => solicitud?.tipoSolicitud === 'MASVI');

                    // Vivienda 14
                    const vivienda14 = cliente?.solicitudes?.some((solicitud: any) =>
                        solicitud?.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)' ||
                        solicitud?.tipoSolicitud === 'SOLUCION DE VIVIENDA (SEGUNDO PAGO)' ||
                        solicitud?.tipoSolicitud === 'SUBSIDIO (PRIMER PAGO)' ||
                        solicitud?.tipoSolicitud === 'SUBSIDIO (SEGUNDO PAGO)' ||
                        (solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'PRIMER PAGO') ||
                        (solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'SEGUNDO PAGO'));

                    // Si tiene dos solciitudes de Pérdida de beneficio 
                    const perdidaBeneficio = cliente?.solicitudes?.filter((solicitud: any) =>
                        solicitud?.tipoSolicitud === 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD');

                    // Pagos extraordinarios muerte del afiliado
                    const PagosExtraordinarios = cliente?.solicitudes?.some((solicitud: any) => solicitud?.tipoSolicitud === 'PAGOS EXTRAORDINARIOS' && solicitud?.modalidad === 'MUERTE DEL AFILIADO');

                    if (vivienda8 || vivienda14 || PagosExtraordinarios || (perdidaBeneficio && perdidaBeneficio.length >= 2)) {
                        etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES;
                    }
                }

                // VERIFICACIÓN DE EMBARGOS
                if (etapa === ID_ACTIVIDAD_POSTULACION.ENTREVISTA) {
                    if (cliente?.solicitudes && cliente?.solicitudes.length > 0) {
                        const embargo = cliente?.solicitudes.some((solicitud: any) => solicitud?.tipoSolicitud === 'EMBARGOS');
                        if (embargo && cliente?.embargos && cliente?.embargos.length > 0) {
                            const tieneEmbargo = cliente?.embargos.some((embargo: any) => embargo?.estadoEmbargo === 'INCONSISTENTE' || embargo?.estadoEmbargo === 'PROCESADA' || embargo?.estadoEmbargo === 'REMANENTE');
                            if (tieneEmbargo) {
                                etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS;
                            }
                        }
                    }
                }

                // VERIFICACIÓN DE CUENTA INDIVIDUAL
                if (etapa === ID_ACTIVIDAD_POSTULACION.ENTREVISTA) {
                    await apiClientes.getMovementsAccountsClient(cliente?.numeroDocumento)
                        .then(response => {
                            if (response.data) {
                                response.data.forEach((item: any) => {
                                    if (item.valorSolicitado > 0) {
                                        etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL;
                                    }
                                })
                            }
                        })

                    if (cliente && cliente?.solicitudes && cliente?.solicitudes.length > 0) {
                        // Reintegros
                        const bloqueoDineros = cliente?.solicitudes.some((solicitud: any) =>
                            (solicitud.estadoSolicitud === 'EN EVALUACION' ||
                                solicitud.estadoSolicitud === 'PROCESADO') &&
                            (solicitud.tipoSolicitud === 'REINTEGROS UNIDADES EJECUTORAS' &&
                                solicitud.modalidad === 'BLOQUEOS'));

                        if (bloqueoDineros) {
                            etapa = ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL;
                        }
                    }
                }

                const request = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    etapa: etapa
                }
                aprobarCargueDocumento(request);
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
            let existeObservacion = false;
            await apiFondoSolidaridad
                .postConsultarObservacionVerificacion(datosSolicitudPostulacion.idSolicitud)
                .then(async response => {
                    if (response.data) {
                        response.data.forEach((element: any) => {
                            if (element.observacion && element.nombreEtapa === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES) {
                                existeObservacion = true;
                            }
                        });
                    }
                })
                .catch((e) => {
                    setLoading(false);
                    toast(`Error al consultar observación verificación novedades ${e.message}`, { type: toast.TYPE.ERROR });
                });

            if (existeObservacion === false) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se deben diligenciar y actualizar la observación`,
                    'warning'
                );
                return;
            }

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {

                let etapaVerificacionNovedades = ID_ACTIVIDAD_POSTULACION.ENTREVISTA;

                // VERIFICACIÓN DE EMBARGOS
                if (cliente?.cuentas.length > 0) {
                    cliente?.cuentas?.forEach((cuenta: any) => {
                        // Validacion Embargos VIGENTES
                        if (cuenta?.embargos.length > 0) {
                            etapaVerificacionNovedades = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS;
                        }
                        // Validacion Embargos tabla de Bloqueos
                        if (cuenta?.conceptosSaldosBloqueos.length > 0) {
                            cuenta?.conceptosSaldosBloqueos.forEach((bloqueo: any) => {
                                if (bloqueo.estadoBloqueo === "VIGENTE") {
                                    etapaVerificacionNovedades = ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS;
                                }
                            })
                        }
                    })
                }

                // VERIFICACIÓN DE CUENTA INDIVIDUAL
                if (etapaVerificacionNovedades === ID_ACTIVIDAD_POSTULACION.ENTREVISTA) {
                    await apiClientes.getMovementsAccountsClient(cliente?.numeroDocumento)
                        .then(response => {
                            if (response.data) {
                                response.data.forEach((item: any) => {
                                    if (item.valorSolicitado > 0) {
                                        etapaVerificacionNovedades = ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL;
                                    }
                                })
                            }
                        })

                    if (cliente?.solicitudes && cliente?.solicitudes.length > 0) {
                        // Vivienda 8 (MASVI)
                        const vivienda8 = cliente?.solicitudes?.some((solicitud: any) => solicitud?.tipoSolicitud === 'MASVI');

                        // Vivienda 14
                        const vivienda14 = cliente?.solicitudes?.some((solicitud: any) =>
                            solicitud?.tipoSolicitud === 'SOLUCION DE VIVIENDA (PRIMER PAGO)' ||
                            solicitud?.tipoSolicitud === 'SOLUCION DE VIVIENDA (SEGUNDO PAGO)' ||
                            solicitud?.tipoSolicitud === 'SUBSIDIO (PRIMER PAGO)' ||
                            solicitud?.tipoSolicitud === 'SUBSIDIO (SEGUNDO PAGO)' ||
                            (solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'PRIMER PAGO') ||
                            (solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' && solicitud.modalidad === 'SEGUNDO PAGO'));

                        // Si tiene dos solciitudes de Pérdida de beneficio 
                        const perdidaBeneficio = cliente?.solicitudes?.filter((solicitud: any) =>
                            solicitud?.tipoSolicitud === 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD');

                        // Pagos extraordinarios muerte del afiliado
                        const PagosExtraordinarios = cliente?.solicitudes?.some((solicitud: any) => solicitud?.tipoSolicitud === 'PAGOS EXTRAORDINARIOS' && solicitud?.modalidad === 'MUERTE DEL AFILIADO');

                        if (vivienda8 || vivienda14 || PagosExtraordinarios || (perdidaBeneficio && perdidaBeneficio.length >= 2)) {
                            etapaVerificacionNovedades = ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL;
                        }

                    }
                }

                const request = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    idEtapa: etapaVerificacionNovedades
                }
                aprobarVerificacionNovedades(request);
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {

            let existeObservacion = false;
            await apiFondoSolidaridad
                .postConsultarObservacionVerificacion(datosSolicitudPostulacion.idSolicitud)
                .then(async response => {
                    if (response.data) {
                        response.data.forEach((element: any) => {
                            if (element.observacion && element.nombreEtapa === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS) {
                                existeObservacion = true;
                            }
                        });
                    }
                })
                .catch((e) => {
                    setLoading(false);
                    toast(`Error al consultar observación verificación embargos ${e.message}`, { type: toast.TYPE.ERROR });
                });

            if (existeObservacion === false) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se deben diligenciar y actualizar la observación`,
                    'warning'
                );
                return;
            }

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {
                let etapaVerificacionEmbargos = ID_ACTIVIDAD_POSTULACION.ENTREVISTA;

                // VERIFICACIÓN DE CUENTA INDIVIDUAL
                await apiClientes.getMovementsAccountsClient(cliente?.numeroDocumento)
                    .then(response => {
                        if (response.data) {
                            response.data.forEach((item: any) => {
                                if (item.valorSolicitado > 0) {
                                    etapaVerificacionEmbargos = ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL;
                                }
                            })
                        }
                    })

                const request = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    idEtapa: etapaVerificacionEmbargos
                }

                aprobarVerificacionEmbargos(request);
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {

            let existeObservacion = false;
            await apiFondoSolidaridad
                .postConsultarObservacionVerificacion(datosSolicitudPostulacion.idSolicitud)
                .then(async response => {
                    if (response.data) {
                        response.data.forEach((element: any) => {
                            if (element.observacion && element.nombreEtapa === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL) {
                                existeObservacion = true;
                            }
                        });
                    }
                })
                .catch((e) => {
                    setLoading(false);
                    toast(`Error al consultar observación verificación cuenta individual ${e.message}`, { type: toast.TYPE.ERROR });
                });

            if (existeObservacion === false) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se deben diligenciar y actualizar la observación`,
                    'warning'
                );
                return;
            }

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {
                const request = {
                    idSolicitud: datosSolicitudPostulacion.idSolicitud,
                    idEtapa: ID_ACTIVIDAD_POSTULACION.ENTREVISTA,
                }

                aprobarVerificacionCuentasIndividuales(request);
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {
            if (datosEntrevista && (
                datosEntrevista?.efS_CONCEPTO_PSICOSOCIAL === null ||
                datosEntrevista?.efS_HABITANTE === 0 ||
                datosEntrevista?.efS_TIPO_VIVIENDA === null ||
                datosEntrevista?.efS_VIVIENDA_PROPIA === null ||
                listaPostulantes.length === 0)
            ) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se debe ingresar la información requerida`,
                    'warning'
                );
                return;
            }

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {
                aprobarEntrevista(datosSolicitudPostulacion.idSolicitud);
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {

            if (datosCalificacion?.tipoImputabilidad === null ||
                datosCalificacion?.imputabilidadAlServicio === 0 ||
                datosCalificacion?.porcentajePostulacion === 0 ||
                datosCalificacion?.disfrutePension === null ||
                datosCalificacion?.datosResolucion === null) {
                await NotificationService.genericAlert(
                    'Información',
                    `Se debe ingresar la información requerida`,
                    'warning'
                );
                return;
            }

            if (postulantesTable) {
                if (postulantesTable.length > 0) {
                    let porcentajesDiligenciados = 0;
                    postulantesTable?.forEach((item: Postulante) => {
                        if (item?.porcentajePostulacion !== undefined &&
                            item?.porcentajeAportes !== undefined &&
                            item?.porcentajeCesantias !== undefined &&
                            item?.porcentajePostulacion > 0 &&
                            item?.porcentajeAportes > 0 &&
                            item?.porcentajeCesantias > 0) {
                            porcentajesDiligenciados += 1;
                        }
                    })

                    if (porcentajesDiligenciados !== postulantesTable.length) {
                        await NotificationService.genericAlert(
                            'Información',
                            `Debe registrarse los porcentajes`,
                            'warning'
                        );
                        return;
                    }
                }
            }

            if (formularioPostulacion.watch('afectacionesCuenta') === 'SI') {
                if (typeof formularioPostulacion.watch('valorAfectaciones') === 'string'
                    && formularioPostulacion.watch('valorAfectaciones').includes('$')
                    && (Number(formularioPostulacion.watch('valorAfectaciones').replace('$', ''))) <= 0) {
                    await NotificationService.genericAlert(
                        'Información',
                        `Debe registrarse el Valor de Afectaciones en la pestaña de Resumen`,
                        'warning'
                    );
                    return;
                }

                if (typeof formularioPostulacion.watch('valorAfectaciones') === 'number'
                    && formularioPostulacion.watch('valorAfectaciones') <= 0) {
                    await NotificationService.genericAlert(
                        'Información',
                        `Debe registrarse el Valor de Afectaciones en la pestaña de Resumen`,
                        'warning'
                    );
                    return;
                }
            }

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {
                aprobarCalificacion(datosSolicitudPostulacion.idSolicitud);
            }
        }

    }

    const aprobarCargueDocumento = async (request: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAprobarCargueDocumentos(request)
            .then(async response => {
                if (response.data && response.returnMessage === 'OK') {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                } else if (response.data && String(response.returnMessage).includes('No hay ningun archivo con el id')) {
                    setLoading(false);
                    toast(`Error al aprobar cargue de documentos ${response.returnMessage}`, { type: toast.TYPE.ERROR });
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error al aprobar cargue de documentos ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const aprobarVerificacionNovedades = async (request: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAprobarVerificacionNovedades(request)
            .then(async response => {
                if (response) {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error al aprobar verificación de novedades ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const aprobarVerificacionEmbargos = async (request: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAprobarVerificacionEmbargo(request)
            .then(async response => {
                if (response) {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error al aprobar verificación de embargos ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const aprobarVerificacionCuentasIndividuales = async (request: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAprobarVerificacionCuentasIndividuales(request)
            .then(async response => {
                if (response) {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error al aprobar verificación de cuentas individuales ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const aprobarEntrevista = async (idSolicitud: string) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAprobarEntrevista(idSolicitud)
            .then(async response => {
                if (response.data) {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error en aprobar entrevista ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const aprobarCalificacion = async (idSolicitud: string) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAprobarCalificacion(idSolicitud)
            .then(async response => {
                if (response.data) {
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error en aprobar calificación ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const rechazar = async () => {

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
            datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS) {
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
                .then(respuesta => {
                    if (respuesta === 'rechazar') {

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {
                            rechazarCargueDocumento(datosSolicitudPostulacion.idSolicitud);
                        }

                    } else if (respuesta === 'anular') {

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {
                            if (formularioObservacionCargueArchivos.getValues('observacion') === '' ||
                                formularioObservacionCargueArchivos.getValues('observacion') === null) {
                                NotificationService.genericAlert(
                                    'Información',
                                    `Se debe ingresar la observación en Cargue de Documentos`,
                                    'warning'
                                );
                                return;
                            }

                            const request = {
                                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                etapa: ID_ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS,
                                observacion: formularioObservacionCargueArchivos.getValues('observacion'),
                            }
                            anularEtapa(request);
                        }

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
                            if (formularioObservacionCargueArchivos.getValues('observacionVerificacionNovedades') === '' ||
                                formularioObservacionCargueArchivos.getValues('observacionVerificacionNovedades') === null) {
                                NotificationService.genericAlert(
                                    'Información',
                                    `Se debe ingresar la observación en Verificación`,
                                    'warning'
                                );
                                return;
                            }

                            const request = {
                                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                etapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES,
                                observacion: formularioObservacionCargueArchivos.getValues('observacionVerificacionNovedades'),
                            }
                            anularEtapa(request);
                        }

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
                            if (formularioObservacionCargueArchivos.getValues('observacionVerificacionEmbargos') === '' ||
                                formularioObservacionCargueArchivos.getValues('observacionVerificacionEmbargos') === null) {
                                NotificationService.genericAlert(
                                    'Información',
                                    `Se debe ingresar la observación en Verificación`,
                                    'warning'
                                );
                                return;
                            }

                            const request = {
                                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                etapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS,
                                observacion: formularioObservacionCargueArchivos.getValues('observacionVerificacionEmbargos'),
                            }
                            anularEtapa(request);
                        }

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) {
                            if (formularioObservacionCargueArchivos.getValues('observacionVerificacionCuentaIndividual') === '' ||
                                formularioObservacionCargueArchivos.getValues('observacionVerificacionCuentaIndividual') === null) {
                                NotificationService.genericAlert(
                                    'Información',
                                    `Se debe ingresar la observación en Verificación`,
                                    'warning'
                                );
                                return;
                            }

                            const request = {
                                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                etapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL,
                                observacion: formularioObservacionCargueArchivos.getValues('observacionVerificacionCuentaIndividual'),
                            }
                            anularEtapa(request);
                        }

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {
                            if (formularioEntrevista.getValues('efS_OBSERVACION') === '' ||
                                formularioEntrevista.getValues('efS_OBSERVACION') === null) {
                                NotificationService.genericAlert(
                                    'Información',
                                    `Se debe ingresar la observación en Entrevista`,
                                    'warning'
                                );
                                return;
                            }

                            const request = {
                                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                etapa: ID_ACTIVIDAD_POSTULACION.ENTREVISTA,
                                observacion: formularioEntrevista.getValues('efS_OBSERVACION'),
                            }
                            anularEtapa(request);
                        }

                        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {
                            if (formularioCalificacion.getValues('observacion') === '' ||
                                formularioCalificacion.getValues('observacion') === null) {
                                NotificationService.genericAlert(
                                    'Información',
                                    `Se debe ingresar la observación en Calificación`,
                                    'warning'
                                );
                                return;
                            }

                            const request = {
                                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                                etapa: ID_ACTIVIDAD_POSTULACION.CALIFICACION,
                                observacion: formularioCalificacion.getValues('observacion'),
                            }
                            anularEtapa(request);
                        }
                    }
                });
        }

        if (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL ||
            datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES ||
            datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if (confirmAlert) {

                if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                    datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO)) {
                    if (formularioObservacionCargueArchivos.getValues('observacionVerificacionNovedades') === '' ||
                        formularioObservacionCargueArchivos.getValues('observacionVerificacionNovedades') === null) {
                        NotificationService.genericAlert(
                            'Información',
                            `Se debe ingresar la observación en Verificación`,
                            'warning'
                        );
                        return;
                    }

                    const request = {
                        idSolicitud: datosSolicitudPostulacion.idSolicitud,
                        etapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES,
                        observacion: formularioObservacionCargueArchivos.getValues('observacionVerificacionNovedades'),
                    }
                    rechazarEtapa(request);
                }

                if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                    datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO)) {
                    if (formularioObservacionCargueArchivos.getValues('observacionVerificacionEmbargos') === '' ||
                        formularioObservacionCargueArchivos.getValues('observacionVerificacionEmbargos') === null) {
                        NotificationService.genericAlert(
                            'Información',
                            `Se debe ingresar la observación en Verificación`,
                            'warning'
                        );
                        return;
                    }

                    const request = {
                        idSolicitud: datosSolicitudPostulacion.idSolicitud,
                        etapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS,
                        observacion: formularioObservacionCargueArchivos.getValues('observacionVerificacionEmbargos'),
                    }
                    rechazarEtapa(request);
                }

                if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                    datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO)) {
                    if (formularioObservacionCargueArchivos.getValues('observacionVerificacionCuentaIndividual') === '' ||
                        formularioObservacionCargueArchivos.getValues('observacionVerificacionCuentaIndividual') === null) {
                        NotificationService.genericAlert(
                            'Información',
                            `Se debe ingresar la observación en Verificación`,
                            'warning'
                        );
                        return;
                    }

                    const request = {
                        idSolicitud: datosSolicitudPostulacion.idSolicitud,
                        etapa: ID_ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL,
                        observacion: formularioObservacionCargueArchivos.getValues('observacionVerificacionCuentaIndividual'),
                    }
                    rechazarEtapa(request);
                }

                if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                    datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                    datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA) {
                    if (formularioEntrevista.getValues('efS_OBSERVACION') === '' ||
                        formularioEntrevista.getValues('efS_OBSERVACION') === null) {
                        NotificationService.genericAlert(
                            'Información',
                            `Se debe ingresar la observación en Entrevista`,
                            'warning'
                        );
                        return;
                    }

                    const request = {
                        idSolicitud: datosSolicitudPostulacion.idSolicitud,
                        etapa: ID_ACTIVIDAD_POSTULACION.ENTREVISTA,
                        observacion: formularioEntrevista.getValues('efS_OBSERVACION'),
                    }
                    rechazarEtapa(request);
                }
            }
        }

        if ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) {
            swal({
                title: 'Seleccione una opción',
                text: 'Seleccione la activdad que desea asignar a la postulación',
                icon: 'warning',
                buttons: {
                    rechazar: {
                        text: 'Cargue Documentos',
                        value: 'cargueDocumentos',
                        className: classes.buttonActividad
                    },
                    anular: {
                        text: 'Entrevista',
                        value: 'entrevista',
                        className: classes.buttonActividad
                    },
                    cancelar: {
                        text: 'Cancelar   ',
                        value: null,
                        className: classes.buttonCancelarDos
                    },
                },
                closeOnClickOutside: false,
            })
                .then(async respuesta => {
                    if (formularioCalificacion.getValues('observacion') === '' ||
                        formularioCalificacion.getValues('observacion') === null) {
                        NotificationService.genericAlert(
                            'Información',
                            `Se debe ingresar la observación en Calificación`,
                            'warning'
                        );
                        return;
                    }

                    const confirmAlert = NotificationService.confirmAlert(
                        `¿Está seguro que desea realizar esta acción?`,
                    );

                    if (await confirmAlert && respuesta === 'cargueDocumentos') {

                        const request = {
                            idSolicitud: datosSolicitudPostulacion.idSolicitud,
                            etapa: ID_ACTIVIDAD_POSTULACION.CALIFICACION,
                            observacion: formularioCalificacion.getValues('observacion'),
                        }
                        rechazarEtapa(request);

                    } else if (await confirmAlert && respuesta === 'entrevista') {
                        rechazarCalificacion(datosSolicitudPostulacion.idSolicitud);
                    }
                })
        }

    }

    const rechazarCargueDocumento = async (idSolicitud: string) => {
        await apiFondoSolidaridad
            .postRechazarCargueDocumentos(idSolicitud)
            .then(async response => {
                if (response.data) {
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }

            })
            .catch((e) => {
                toast(`Error al rechazar cargue de documento ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const rechazarEtapa = async (request: any) => {
        await apiFondoSolidaridad
            .postRechazarEtapa(request)
            .then(async response => {
                if (response.returnMessage === 'Todo OK') {
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }

            })
            .catch((e) => {
                toast(`Error al rechazar etapa ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const rechazarCalificacion = async (idSolicitud: string) => {
        await apiFondoSolidaridad
            .postRechazarCalificacion(idSolicitud)
            .then(async response => {
                if (response.data) {
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }

            })
            .catch((e) => {
                toast(`Error al rechazar a entrevista en calificacion ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const anularEtapa = async (request: any) => {
        await apiFondoSolidaridad
            .postAnularEtapa(request)
            .then(async response => {
                if (response.returnMessage === 'OK') {
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    regresarPantallaInicial();
                }
            })
            .catch((e) => {
                toast(`Error al anular etapa ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const onClickRegresar = () => {
        regresarPantallaInicial();
    }

    const regresarPantallaInicial = () => {
        setLoading(true);
        formularioSolicitud.reset();
        formularioRetiro.reset();
        const busqueda = {
            numeroConvocatoria: '',
            estado: 1,
            fechaDesde: '',
            fechaHasta: '',
            fechaPublicacion: '',
        }
        consultarConvocatorias(busqueda);
        setAgregarNuevaPostulacion(false);
        setActividadSolicitud('');
        setEstadoSolicitud('');
        const request = {
            radicado: '',
            idEtapa: '',
            identificacion: '',
            tipoModalidad: '',
            numeroConvocatoria: '',
        }
        consultarSolicitudes(request);
        setAgregarSolicitudPostulacion(false);
        setSelectedData(null);
        setLoading(false);
    }

    return (
        <>
            <LoadingComponent loading={loading && cliente} />
            <CHPageTitleV2
                topText="Gestión de postulaciones"
                title1="Solicitud Postulación"
                title2='Fondo Solidaridad'
                icono="fact_check"
            />

            <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
                datosSolicitudPostulacion={datosSolicitudPostulacion}
                mostrarPestaniaEntrevista={mostrarPestaniaEntrevista}
                setMostrarPestaniaEntrevista={setMostrarPestaniaEntrevista}
                mostrarPestaniaCalificacion={mostrarPestaniaCalificacion}
                inconsistenciaSolicitud={inconsistenciaSolicitud}
                setMostrarPestaniaCalificacion={setMostrarPestaniaCalificacion}
            />
            <Panel
                currentTabValue={currentTabValue}
                cliente={cliente}
                catalogState={catalogState}
                tipoSolicitud={tipoSolicitud}
                datosSolicitudPostulacion={datosSolicitudPostulacion}
                documentosAgregados={documentosAgregados}
                camposCargueDocumentosSoloLectura={camposCargueDocumentosSoloLectura}
                camposVerificacionNovedadesSoloLectura={camposVerificacionNovedadesSoloLectura}
                camposVerificacionEmbargosSoloLectura={camposVerificacionEmbargosSoloLectura}
                camposVerificacionCuentaIndividualSoloLectura={camposVerificacionCuentaIndividualSoloLectura}
                camposEntrevistaSoloLectura={camposEntrevistaSoloLectura}
                camposCalificacionSoloLectura={camposCalificacionSoloLectura}
                formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
                formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
                formularioEntrevista={formularioEntrevista}
                postulantesControl={postulantesControl}
                datosEntrevista={datosEntrevista}
                listaTiposIntegrantes={listaTiposIntegrantes}
                listaTiposDiscapacidad={listaTiposDiscapacidad}
                listaTiposAyudaTecnica={listaTiposAyudaTecnica}
                listaPostulantes={listaPostulantes}
                inconsistenciaSolicitud={inconsistenciaSolicitud}
                beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion}
                consultarTiposIntegrantes={consultarTiposIntegrantes}
                consultarTiposDiscapcidad={consultarTiposDiscapcidad}
                consultarTiposAyudaTecnica={consultarTiposAyudaTecnica}
                setListaPostulantes={setListaPostulantes}
                formularioCalificacion={formularioCalificacion}
                formularioPostulacion={formularioPostulacion}
                onChangeImputabilidad={onChangeImputabilidad}
                imputabilidadValue={imputabilidadValue}
                nullImputabilidadValue={nullImputabilidadValue}
                postulantesTable={postulantesTable}
                setPostulantesTable={setPostulantesTable}
                totalPorcentajePostulacion={totalPorcentajePostulacion}
                setTotalPorcentajePostulacion={setTotalPorcentajePostulacion}
                totalPorcentajeAportes={totalPorcentajeAportes}
                setTotalPorcentajeAportes={setTotalPorcentajeAportes}
                totalPorcentajeCesantias={totalPorcentajeCesantias}
                setTotalPorcentajeCesantias={setTotalPorcentajeCesantias}
                mostrarCampoPorcentajeDisminucion={mostrarCampoPorcentajeDisminucion}
                mostrarVerificarNovedades={mostrarVerificarNovedades}
                mostrarVerificarEmbargos={mostrarVerificarEmbargos}
                mostrarVerificarCuentaIndividual={mostrarVerificarCuentaIndividual}
                consultarBenficiariosPostulacion={consultarBenficiariosPostulacion}
                consultarCalificacion={consultarCalificacion}
                setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion}
            />
            <Grid container>
                <Grid
                    className={classes.columnButtonAdd}
                    item
                    xs={12}
                    md={12}
                >

                    {datosSolicitudPostulacion.estadoProceso !== ESTADO_POSTULACION.ANULADO && (
                        ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD
                            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)
                        ) && (
                            <Button
                                className={classes.button}
                                variant="outlined"
                                onClick={formularioObservacionCargueArchivos.handleSubmit(actualizarCargueDocumentos)}
                            >
                                ACTUALIZAR
                            </Button>
                        )

                    )
                    }

                    {datosSolicitudPostulacion.estadoProceso !== ESTADO_POSTULACION.ANULADO && (
                        ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES
                            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)
                        ) && (
                            <Button
                                className={classes.button}
                                variant="outlined"
                                onClick={formularioObservacionCargueArchivos.handleSubmit(actualizarVerificacion)}
                            >
                                ACTUALIZAR
                            </Button>
                        )

                    )
                    }

                    {datosSolicitudPostulacion.estadoProceso !== ESTADO_POSTULACION.ANULADO && (
                        ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
                            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
                                && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) && (
                            <Button
                                className={classes.button}
                                variant="outlined"
                                onClick={formularioEntrevista.handleSubmit(actualizarEntrevista)}
                            >
                                ACTUALIZAR
                            </Button>
                        )
                    )
                    }

                    {datosSolicitudPostulacion.estadoProceso !== ESTADO_POSTULACION.ANULADO && (
                        ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION
                            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE))
                        && (
                            <Button
                                className={classes.button}
                                variant="outlined"
                                onClick={formularioCalificacion.handleSubmit(actualizarCalificacion)}
                            >
                                ACTUALIZAR
                            </Button>
                        )
                    )
                    }

                    {datosSolicitudPostulacion.estadoProceso !== ESTADO_POSTULACION.ANULADO && (
                        ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)) && (

                            <Button
                                className={classes.button}
                                variant="outlined"
                                color="default"
                                onClick={aprobar}
                            >
                                APROBAR
                            </Button>
                        ))}

                    {datosSolicitudPostulacion.estadoProceso !== ESTADO_POSTULACION.ANULADO && (
                        ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.ENTREVISTA &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
                            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS &&
                                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO)) && (
                            <Button
                                className={classes.button}
                                variant="outlined"
                                color="default"
                                onClick={rechazar}
                            >
                                RECHAZAR
                            </Button>
                        ))}
                    {(datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                        datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
                        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) ||
                        (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
                            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.APROBADO)
                        && (
                            <Button
                                className={classes.buttonInconsistencia}
                                variant="outlined"
                                color="default"
                                onClick={handleOpenModalInconsistencia}
                            >
                                DECLARAR INCONSISTENCIA
                            </Button>
                        )}
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        onClick={onClickRegresar}
                    >
                        REGRESAR
                    </Button>
                </Grid>
            </Grid>
            <ModalDeclararInconsisntencia
                open={openDeclararInconsistencia}
                handleClose={handleCloseModalInconsistencia}
                generarInconsistencia={generarInconsistencia}
                formularioInconsistencia={formularioInconsistencia}
            />
        </>
    )
}

type propsType = {
    formularioResumenInformacionSolicitud: any;
    formularioSolicitud: any;
    formularioRetiro: any;
    formularioPostulacion: any;
    formularioObservacionCargueArchivos: any;
    catalogState: CatalogState;
    tipoSolicitud: number;
    datosSolicitudPostulacion: any;
    listaPostulantes: Postulante[];
    formularioEntrevista: UseFormReturn<any>;
    formularioCalificacion: any;
    mostrarCampoPorcentajeDisminucion: boolean;
    openDeclararInconsistencia: boolean;
    formularioInconsistencia: any;
    beneficiariosPostulantesPostulacion: Postulante[];
    cliente: any;
    imputabilidadValue: catalogPostulacionDto | null;
    nullImputabilidadValue: boolean;
    datosCalificacion: any;
    setListaPostulantes: (lista: Postulante[]) => void;
    setActividadSolicitud: (value: string) => void;
    setEstadoSolicitud: (value: any) => void;
    setAgregarNuevaPostulacion: (value: boolean) => void;
    setMostrarCampoPorcentajeDisminucion: (value: boolean) => void;
    consultarConvocatorias: (busqueda: any) => void;
    setAgregarSolicitudPostulacion: (value: boolean) => void;
    setSelectedData: (value: SolicitudDto | null) => void;
    consultarSolicitudes: (request: any) => void;
    actualizarPostulacionValorAfectaciones: (datosPostulacion: any) => void;
    validarExisteSolicitudPorId: (idSolicitud: string) => void;
    generarInconsistencia: () => void;
    handleOpenModalInconsistencia: () => void;
    handleCloseModalInconsistencia: () => void;
    setBeneficiariosPostulantesPostulacion: (value: Postulante[]) => void;
    consultarBenficiariosPostulacion: (idSolicitud: string) => void;
    onChangeImputabilidad: (event: any, imputabilidad: catalogPostulacionDto) => void;
    consultarCalificacion: (idSolicitud: string) => void;
};

export default EtapasPostulacion;