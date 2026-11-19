import React, { useEffect, useState } from 'react';
import { createStyles, Tab, Tabs, Theme, withStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';

import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import BotoneraInformacionSolicitudComponent from '../components/botonera/botonera-informacion-solicitud-component';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import QuitarCaracteres from '../components/funciones/quitar-caracteres.function';
import NotificationService from '../../../services/notification.service';
import SolicitudApi from '../../../services/api-solicitud.service';
import CargueDocumentosSolicitud from '../components/cargue-documentos-solicitud/cargue-documentos-solicitud.component';
import { DocumentosPorSubModalidadModel } from '../../../models/documentos-por-submodalidad.model';
import VerDocumentosSolicitud from '../components/cargue-documentos-solicitud/ver-documentos-solicitud.component';

import GenerarComunicaciones from '../components/generar-comunicaciones/generar-comunicaciones.component';
import { BOTONERA_SOLICITUD } from '../../../constants/constants';
import { ComunicacionSolicitud, FormularioInconsistenciasDto } from '../../../models/requests/solicitud-tarea.model';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ResumenAcreditacion from './resumenAcreditacion.component';

const api = SolicitudApi();

const TABS_INDEX = {
    RESUMENSOLICITUD: 0,
    TRAMITE: 1,
    CARGUEDOCUMENTOS: 2,
    CARGAARCHIVOS: 3,
    VERARCHIVOS: 4,
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
    permissions,
}: {
    currentTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
    permissions: string;
}) {
    const quitarCaracter = QuitarCaracteres();

    return (
        <AntTabs
            value={currentTab}
            onChange={onTabChange}
            aria-label="ant example"
        >
            <AntTab label="Resumen" value={TABS_INDEX.RESUMENSOLICITUD} />
            <AntTab label="Trámite" value={TABS_INDEX.TRAMITE} />
            <AntTab label="Cargue de documentos" value={TABS_INDEX.CARGUEDOCUMENTOS} />
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
            <AntTab label="Ver Archivos" value={TABS_INDEX.VERARCHIVOS} />

        </AntTabs>
    );
}

function Panel({
    currentTabValue,
    responseSolicitud,
    control,
    grupo1,
    novedad,
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
    controlComunicaciones,
    setValueComunicaciones,
    
}: {
    documentosAgregados: (documentos: DocumentosPorSubModalidadModel[]) => void;
    currentTabValue: number;
    control: any;
    getValues: any;
    responseSolicitud: ResponseSeveranceModel | undefined;
    grupo1: boolean;
    novedad: number;
    setValue: any;
    setEstadoSubsanar: (value: boolean) => void;
    handleSubmit: any;
    reset: any;
    watch: any;
    listadoInconsistencias: FormularioInconsistenciasDto[];
    addInconsistencia: any;
    updateInconsistencias: any;
    deleteInconsistencias: any;
    controlComunicaciones: any;
    setValueComunicaciones: any;
 
}) {
    const [indexTabValue, setTndexTabValue] = useState(
        TABS_INDEX.CARGAARCHIVOS,
    );

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        setTndexTabValue(newValue);
    };

    switch (currentTabValue) {
        case TABS_INDEX.RESUMENSOLICITUD:
            return (
                <>
                    <ResumenAcreditacion responseSolicitud={responseSolicitud} />
                </>
            );        
        case TABS_INDEX.TRAMITE:
            return (
                <>
                    <GenerarComunicaciones
                        idSolicitud={responseSolicitud?.solicitudId || ''}
                        responseSolicitud={responseSolicitud}
                        control={controlComunicaciones}
                        setValue={setValueComunicaciones}
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
                        siEsAprobadaSolcitud={undefined} 
                      
                     />
                </>
            );
        case TABS_INDEX.VERARCHIVOS:
            return (
                <>
                    <VerDocumentosSolicitud
                        responseSolicitud={responseSolicitud}
                        siEsAprobadaSolcitud = {false}
                    />
                </>
            );
        default:
            return null;
    }
}

function Acreditacion({
    responseSolicitud,
    onItemGoBack,
    tipoAccion,
    tipoAccionDetalleProcesar,
    grupo1,
}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    onItemGoBack: () => void;
    tipoAccion: number;
    tipoAccionDetalleProcesar: boolean;
    grupo1: boolean;
}) {
    const [currentTabValue, setCurrentTabValue] = useState(
        TABS_INDEX.RESUMENSOLICITUD,
    );

    const [tipoAccionNumber, setAccionNumber] = useState(BOTONERA_SOLICITUD.ACTUALIZAR);
    const [aprobar, setAprobar] = useState(true);
    const [subsanado, setSubsanado] = useState(true);
    const [rechazar, setRechazar] = useState(true);
    const [actualizar, setActualizar] = useState(true);
    const [estadoSubsanar, setEstadoSubsanar] = useState(false);
    const [listadoInconsistencias, setListadoInconsistencias] = useState<FormularioInconsistenciasDto[]>([]);
    const [documentosAguardar, setDocumentosAguardar] = useState<
        DocumentosPorSubModalidadModel[]
    >([]);
    const [loading, setLoading] = useState(false);
   

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

    const { control, watch, getValues, setValue, handleSubmit, reset } = useForm({
        defaultValues: {
            inconsistenciaId: null,
            novedad: 0,
            listaInconsistenciaId: null,
            causalInterrupcionId: 0,
            herramientaId: null,
            puntoAtencionId: null,
            grupoInconsistencia: 0,
            solicitudId: responseSolicitud?.solicitudId,
            tecnicoId: null,
            objetoEstudio: '',
            analisis: '',
            observacion: '',
            subsanado: false,
            fechaSubsanacion: new Date(),
            estado: false,
            tecnicoReporta: '',
            puntoAtencion: 0,
            procedeDesbloqueo: false,
            index: 0
        },
    });

    const {
        control: controlComunicaciones,
        setValue: setValueComunicaciones,
        getValues: getValuesComunicaciones
    } = useForm<ComunicacionSolicitud>({
        defaultValues: {
            idSolicitud: responseSolicitud?.solicitudId,
            idCausalComunicacion: 0,
            idComunicacion: null,
            textoIncluir: '',
            firmaDigital: false,
            radicado: '',
        }
    })

    const novedad = watch('novedad')

    useEffect(() => {
        api.GetConsultarInconsistencia(responseSolicitud?.solicitudId as string)
            .then((result) => {
                setListadoInconsistencias(result.data)
            })
        if (getValues('subsanado')) {
            setAprobar(false);
            setSubsanado(true);
            setRechazar(true);
            setActualizar(true);
        } else {
            setAprobar(true);
            setSubsanado(false);
            setRechazar(false);
            setActualizar(false);
        }
    }, []);

   

    const onSubmitAprobar = async () => {
     
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro que desea continuar?',
        );
        if (confirmAlert) {
            setLoading(true)
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
                            // setSelectedData(response.data);
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
        console.log('onSubmitSubsanar');
    };

    const onSubmitRechazar = async () => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro que desea continuar?',
        );

        if (confirmAlert) {
            setLoading(true)
            const rechazarSolicitudTareaDto = {
                solicitudTareaId: responseSolicitud?.solicitudTareaId as string
            };
            await api
                .PostRechazarSolicitudTarea(rechazarSolicitudTareaDto)
                .then(async (response) => {
                    if (response) {
                        if (response.isSuccess) {
                            toast('Se rechazó la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
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
    const [error, setError] = useState(false);

    const onSubmit2 = async () => {
        
        const api = SolicitudApi();
        setLoading(true)
        if (currentTabValue === TABS_INDEX.CARGUEDOCUMENTOS) {
            for (let x = 0; x < documentosAguardar?.length; x += 1) {
                const elemento = documentosAguardar[x];
                const archivosCargados = elemento?.archivosCargados?.length;
                if (elemento.esRequerido && (archivosCargados === undefined || archivosCargados <= 0) && !elemento.esArchivoCargado) {
                    toast('Debe completar los documentos obligatorios o los números de folios', { type: toast.TYPE.ERROR });
                    return;
                }
            }
            const formDataImg = new FormData();
            documentosAguardar.forEach((file, index) => {
                const archivos = file.archivosCargados;
                if (archivos !== undefined) {
                    Array.from(archivos).forEach(files => {
                        const newName = `${file.idArchivo}${'--'}${files.name}${'--'}${file.numeroFolio}`;
                        formDataImg.append(`archivosCargados[${index}]`, files, newName);
                    });
                }
            });
            await api
                .postCargueDocumentosSolicitud(formDataImg, responseSolicitud?.solicitudId, responseSolicitud?.numeroRadicadoSG)
                .then((result) => {
                    if (result.returnMessage.toString().includes('Error')) {
                        toast(result.returnMessage.toString(), { type: toast.TYPE.ERROR });
                        setCurrentTabValue(TABS_INDEX.CARGUEDOCUMENTOS);
                        setAccionNumber(BOTONERA_SOLICITUD.ACTUALIZAR);
                        setActualizar(false);
                    } else {
                        toast('Archivos cargados correctamente', { type: toast.TYPE.SUCCESS });
                        setDocumentosAguardar([]);
                        setAprobar(false);
                    }
                })
                .catch(() => {
                    setCurrentTabValue(TABS_INDEX.CARGUEDOCUMENTOS);
                    setActualizar(false);
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        }

        setLoading(false)
    };

    const addInconsistencia = (data: any) => {
        setListadoInconsistencias(listadoInconsistencias => [...listadoInconsistencias, data])
        reset()
    }

    const updateInconsistencias = async (data: any) => {
        const items = listadoInconsistencias;
        const item = data;
        const index = watch('index')
        items[index - 1] = item;

        setListadoInconsistencias(items)
        reset()
    };

    const deleteInconsistencias = async (data: any) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Desea eliminar la inconsistencia?',
        );
        if (confirmAlert) {
            setLoading(true)
            setListadoInconsistencias(listadoInconsistencias?.filter(x => x !== data))
            await api.EliminarInconsistenciaSolicitud(data.inconsistenciaId)
            setLoading(false)
        }
    }

    return (
        <>
            <CHPageTitleV2
                topText="Solicitud"
                title1="Solicitud de Cesantías"
                icono="home"
            />
            <LoadingComponent key="loader" loading={loading} />
            <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
                permissions={responseSolicitud?.pestanas as string}
            />
            <Panel
                responseSolicitud={responseSolicitud}
                currentTabValue={currentTabValue}
                documentosAgregados={documentosAgregados}
                setEstadoSubsanar={setEstadoSubsanar}
                control={control}
                novedad={novedad}
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
                controlComunicaciones={controlComunicaciones}
                setValueComunicaciones={setValueComunicaciones}
            />
            <BotoneraInformacionSolicitudComponent
                onItemGoBack={onItemGoBack}
                tipoAccion={tipoAccionNumber}
                onSubmitAprobar={onSubmitAprobar}
                onSubmitSubsanar={onSubmitSubsanar}
                onSubmitGenerarPago={undefined}
                onSubmitRechazar={onSubmitRechazar}
                onSubmit={onSubmit2}
                disabledActualizar={false}
                disabledAprobar={false}
                disabledSubsanar={false}
                disabledRechazar={false}
                disabledRegresar={false}
                tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
                responseSolicitud={undefined}
                currentTabValue={currentTabValue}
                aprobarBloquear={undefined}
                hablitarActualiza={undefined}
                deshabilitarBTNRechazarGp2={undefined}
                 deshalitarBotonesInconsistencia={false}            />
        </>
    );
}

export default Acreditacion;
