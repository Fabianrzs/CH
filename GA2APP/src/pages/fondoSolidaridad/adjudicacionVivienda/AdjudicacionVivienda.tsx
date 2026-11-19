import { Button, createStyles, 
         Grid, 
         makeStyles,
         Tab, 
         Tabs, 
         Theme, 
         withStyles } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import { sortBy } from 'lodash';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { catalogPostulacionDto, imputabilidad_servicio_disminucion_capacidad, imputabilidad_servicio_muerte, TABS_INDEX_ADJUDICACION_VIVIENDA, TIPO_IMPUTABILIDAD, TIPO_MODALIDAD } from '../constants/fondoSolidaridad.constants';
import EntrevistaPostulacion from '../postulacion/EntrevistaPostulacion';
import ResumenPostulacion from '../postulacion/ResumenPostulacion';
import AsignacionViviendaLegislacion from './AsignacionViviendaLegislacion';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import Calificacion from '../postulacion/Calificacion';
import { LoadingComponent } from '../../../components/loading/loading.component';
import NotificationService from '../../../services/notification.service';

const useStyles = makeStyles(() =>
    createStyles({
        columnButtonAdd: {
            textAlign: 'right',
        },
        button: {
            width: '120px',
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
            minWidth: 72,
            marginRight: theme.spacing(4),
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
}: {
    currentTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
}) {
    return (
            <AntTabs
                value={currentTab}
                onChange={onTabChange}
                aria-label="ant example"
            >
            <AntTab
                label="Resumen"
                value={TABS_INDEX_ADJUDICACION_VIVIENDA.RESUMEN}
            />
          
            <AntTab
                label="Entrevista"
                value={TABS_INDEX_ADJUDICACION_VIVIENDA.ENTREVISTA}
            />

            <AntTab
                label="Calificación"
                value={TABS_INDEX_ADJUDICACION_VIVIENDA.CALIFICACION}
            />
            <AntTab
                label="Asignación de Vivienda y Legalización"
                value={TABS_INDEX_ADJUDICACION_VIVIENDA.ASIGNACION_VIVIENDA}
            />
            </AntTabs>
    );
}

function Panel({
    currentTabValue,
    formularioPostulacion,
    formularioEntrevista,
    formularioCalificacion,
    formularioAsignacionViviendaLegislacion,
    formularioResumenInformacionSolicitud,
    formularioObservacionCargueArchivos,
    postulantesControl,
    catalogState,
    datosSolicitudPostulacion,
    tipoSolicitud,
    listaTiposIntegrantes,
    listaTiposDiscapacidad,
    listaTiposAyudaTecnica,
    imputabilidadValue,
    nullImputabilidadValue,
    postulantesTable,
    totalPorcentajePostulacion,
    totalPorcentajeAportes,
    totalPorcentajeCesantias,
    mostrarCampoPorcentajeDisminucion,
    cliente,
    datosEntrevista,
    listaPostulantes,
    setListaPostulantes,
    setPostulantesTable,
    consultarTiposIntegrantes,
    consultarTiposDiscapcidad,
    consultarTiposAyudaTecnica,
    consultarEntrevista,
    onChangeImputabilidad,
    setTotalPorcentajePostulacion,
    setTotalPorcentajeAportes,
    setTotalPorcentajeCesantias,
    consultarCalificacion,
}: {
    currentTabValue: number;
    formularioPostulacion: UseFormReturn<any>;
    formularioEntrevista: UseFormReturn<any>;
    formularioCalificacion: UseFormReturn<any>;
    formularioAsignacionViviendaLegislacion: UseFormReturn<any>;
    formularioResumenInformacionSolicitud: any;
    formularioObservacionCargueArchivos: any;
    postulantesControl: UseFormReturn<any>;
    catalogState: CatalogState;
    datosSolicitudPostulacion: any;
    tipoSolicitud: number;
    imputabilidadValue: catalogPostulacionDto | null;
    postulantesTable: Postulante[];
    listaTiposIntegrantes: any[];
    listaTiposDiscapacidad: any[];
    listaTiposAyudaTecnica: any[];
    nullImputabilidadValue: boolean;
    totalPorcentajePostulacion: number;
    totalPorcentajeAportes: number;
    totalPorcentajeCesantias: number;
    mostrarCampoPorcentajeDisminucion: boolean;
    cliente: any;
    datosEntrevista: any;
    listaPostulantes: Postulante[];
    setListaPostulantes: (value: Postulante[])  => void;
    setPostulantesTable: (value: Postulante[]) => void;
    consultarTiposIntegrantes: () => void;
    consultarTiposDiscapcidad: () => void;
    consultarTiposAyudaTecnica: () => void;
    consultarEntrevista: (idSolicitud: string) => void;
    onChangeImputabilidad: (event: any, imputabilidad: catalogPostulacionDto) => void;
    setTotalPorcentajePostulacion: (value: number) => void;
    setTotalPorcentajeAportes: (value: number) => void;
    setTotalPorcentajeCesantias: (value: number) => void;
    consultarCalificacion: (idSolicitud: string) => void;
}) {

    // datos temporales
    const datosEntrevistaPrueba = {
        postulantes: null,
    }

    const datosPostulacionPrueba = {
        idSolicitud: '00000000-0000-0000-0000-000000000000',
        actividad: 'ADJUDICACION VIVIENDA',
        idModalidad: 1,
    }

    switch (currentTabValue) {
        case TABS_INDEX_ADJUDICACION_VIVIENDA.RESUMEN:
            return (
                <>
                    <ResumenPostulacion 
                        cliente={cliente}
                        catalogState={catalogState}
                        tipoSolicitud={tipoSolicitud}
                        datosSolicitudPostulacion={datosSolicitudPostulacion} 
                        formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud} 
                        formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
                    />
                </>
            );
        case TABS_INDEX_ADJUDICACION_VIVIENDA.ENTREVISTA:
            return (
                <>
                     <EntrevistaPostulacion
                        datosSolicitudPostulacion={datosSolicitudPostulacion || datosPostulacionPrueba}
                        catalogState={catalogState}
                        formularioEntrevista={formularioEntrevista}
                        datosEntrevista={datosEntrevista || datosEntrevistaPrueba}
                        cliente={cliente}
                        listaTiposIntegrantes={listaTiposIntegrantes}
                        listaTiposDiscapacidad={listaTiposDiscapacidad}
                        listaTiposAyudaTecnica={listaTiposAyudaTecnica}
                        camposEntrevistaSoloLectura
                        setListaPostulantes={setListaPostulantes}
                        consultarTiposIntegrantes={consultarTiposIntegrantes}
                        consultarTiposDiscapcidad={consultarTiposDiscapcidad}
                        consultarTiposAyudaTecnica={consultarTiposAyudaTecnica} 
                        postulantesControl={postulantesControl}            
                    />
                </>
            );
    
        case TABS_INDEX_ADJUDICACION_VIVIENDA.CALIFICACION:
            return (
                <>
               
                   <Calificacion
                        datosSolicitudPostulacion={datosSolicitudPostulacion || datosPostulacionPrueba}
                        formularioCalificacion={formularioCalificacion}
                        onChangeImputabilidad={onChangeImputabilidad}
                        imputabilidadValue={imputabilidadValue}
                        cliente={cliente}
                        nullImputabilidadValue={nullImputabilidadValue}
                        camposCalificacionSoloLectura
                        postulantesTable={postulantesTable}
                        totalPorcentajePostulacion={totalPorcentajePostulacion}
                        totalPorcentajeAportes={totalPorcentajeAportes}
                        totalPorcentajeCesantias={totalPorcentajeCesantias}
                        mostrarCampoPorcentajeDisminucion={mostrarCampoPorcentajeDisminucion}
                        setPostulantesTable={setPostulantesTable}
                        setTotalPorcentajePostulacion={setTotalPorcentajePostulacion}
                        setTotalPorcentajeAportes={setTotalPorcentajeAportes}
                        setTotalPorcentajeCesantias={setTotalPorcentajeCesantias} 
                        formularioPostulacion={formularioPostulacion} 
                        formularioEntrevista={formularioEntrevista} 
                        postulantesControl={postulantesControl} 

                        listaPostulantes={listaPostulantes} 
                        consultarCalificacion={consultarCalificacion}                        
                    /> 
                </>
            );
        case TABS_INDEX_ADJUDICACION_VIVIENDA.ASIGNACION_VIVIENDA:
            return (
                <>
                    <AsignacionViviendaLegislacion  
                        formulario={formularioAsignacionViviendaLegislacion}
                    />
                </>
        );
        default:
            return null;
    }
}

const AdjudicacionVivienda = (props: Props) => {
    const classes = useStyles();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {catalogState, 
           cliente,
           datosSolicitudPostulacion,
           tipoSolicitud,
           formularioPostulacion,
           formularioEntrevista,
           formularioCalificacion,
           formularioObservacionCargueArchivos,
           formularioResumenInformacionSolicitud,
           mostrarCampoPorcentajeDisminucion,
           listaPostulantes,
           setListaPostulantes,
           regresarPantallaInicial,
           setMostrarCampoPorcentajeDisminucion,
           consultarCalificacion} = props;
    const [currentTabValue, setCurrentTabValue] = useState(TABS_INDEX_ADJUDICACION_VIVIENDA.RESUMEN);
    
    const [listaTiposIntegrantes, setListaTiposIntegrantes] = useState<any[]>([]);
    const [listaTiposDiscapacidad, setListaTiposDiscapacidad] = useState<any[]>([]);
    const [listaTiposAyudaTecnica, setListaTiposAyudaTecnica] = useState<any[]>([]);

    const [imputabilidadValue, setImputabilidadValue] = useState<catalogPostulacionDto | null>(null);
    const [nullImputabilidadValue, setNullImputabilidadValue] = useState(false);
    const [postulantesTable,setPostulantesTable] = useState<Postulante[]>([]);
    const [totalPorcentajePostulacion, setTotalPorcentajePostulacion] = useState<number>(0);
    const [totalPorcentajeAportes, setTotalPorcentajeAportes] = useState<number>(0);
    const [totalPorcentajeCesantias, setTotalPorcentajeCesantias] = useState<number>(0);
    const [datosEntrevista, setDatosEntrevista] = useState<any>(null);
    const [loading, setLoading] = useState(false);

    const formularioAsignacionViviendaLegislacion = useForm({
        defaultValues: {
            estadoAdjudicacion: '',
            radicadoAceptacion: '',
            proyectoAsignar: '',
            tipoInmueble: '',
            tipoVivienda: '',
            claseVivienda: '',
            vivienda: '',
            valorVivienda: '',
            tiempoDesplazamiento: '',
            distancia: '',
            acompaniante: '',
            tipoIdentificacionAcompaniante: '',
            numeroIdentificacionAcompaniante: '',
            nombresApellidosAcompaniante: '',
            fechaEntregaVivienda: '',
            entregada: '',
            observacionesEntrega: '',
            departamentoId: '',
            ciudadId: '',
            notaria: '',
            fechaEscritura: '',
            estadoEscritura: '',
            numeroEscritura: '',
            radicadoEntrega: '',
            numeroMatriculaInmobiliaria: '',
            fechaEntregaEscrituracion: '',
            fechaEntregaEscritura: '',
            valorCuentaIndividual: '',
            valorCargoCajaHonor: '',
            valorTotalBeneficio:'',
            valorTotalLegislacion: '',
            tipoIdentificacionBeneficiario: '',
            numeroIdentificacionBeneficiario: '',
            nombreRazonSocial: '',
            tipoCuenta: '',
            entidad: '',
            numeroCuenta: '',
            numeroComprobante: '',
            observacion: ''
        }
    })

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
        // validacion temporal if(datosSolicitudPostulacion
        if(datosSolicitudPostulacion){
            consultarObservacionCargueArchivo(datosSolicitudPostulacion?.idSolicitud);
            consultarEntrevista(datosSolicitudPostulacion?.idSolicitud);
            consultarCalificacion(datosSolicitudPostulacion?.idSolicitud);
        }
        window.scrollTo(1000,0);
    },[datosSolicitudPostulacion])

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        setCurrentTabValue(newValue);
    };

    const onChangeImputabilidad = (event: any, imputabilidad: catalogPostulacionDto) => {
        setNullImputabilidadValue(imputabilidad == null);
        setImputabilidadValue(imputabilidad);
        if (imputabilidad) {
            formularioCalificacion.setValue('imputabilidadAlServicio',String(imputabilidad.id));
        } 
    };

    const consultarTiposIntegrantes = async () => {
        await apiFondoSolidaridad
            .getObtenerTiposIntegrantes()
            .then(async (response: any) => {
                if (response.data) {
                    setListaTiposIntegrantes(response?.data);
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
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
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
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
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarObservacionCargueArchivo = async (idSolicitud: string) => {
        await apiFondoSolidaridad
            .getConsultarObservacionCargueArchivo(idSolicitud)
            .then(async response => {
                if(response.data){
                    formularioResumenInformacionSolicitud.setValue('totalFolios',response?.data?.numeroFolio === 0? '' : String(response?.data?.numeroFolio));
                    formularioObservacionCargueArchivos.setValue('numeroFolios',response?.data?.numeroFolio === 0? '' : String(response?.data?.numeroFolio));
                }
            })
            .catch((e) => {
                toast(`Error consultar Observación cargue de archivo en adjudicación vivienda ${e.message}`, { type: toast.TYPE.ERROR });
            });
     };

    const consultarEntrevista = async (idSolicitud: string) => {
        setLoading(true);
        await apiFondoSolidaridad
            .getConsultarEntrevista(idSolicitud)
            .then(async response => {
                if(response.data){
                    setListaPostulantes(response.data.postulantes);
                    setDatosEntrevista(response.data);
                    formularioEntrevista.setValue('efS_ID', response?.data?.efS_ID);
                    formularioEntrevista.setValue('efS_ID_SOLICITUD', response?.data?.efS_ID_SOLICITUD);
                    formularioEntrevista.setValue('efS_TIPO_VIVIENDA', response?.data?.efS_TIPO_VIVIENDA);
                    formularioEntrevista.setValue('efS_VIVIENDA_PROPIA', response?.data?.efS_VIVIENDA_PROPIA);
                    formularioEntrevista.setValue('efS_HABITANTE', response?.data?.efS_HABITANTE === 0? '' : response?.data?.efS_HABITANTE);
                    formularioEntrevista.setValue('efS_INFORMATIVO_ADMIN', response?.data?.efS_INFORMATIVO_ADMIN);
                    formularioEntrevista.setValue('efS_CONCEPTO_PSICOSOCIAL', response?.data?.efS_CONCEPTO_PSICOSOCIAL);
                    formularioEntrevista.setValue('efS_OBSERVACION', response?.data?.efS_OBSERVACION);
                    setLoading(false);
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error del sistema Consultar entrevista en adjudicación vivienda${e.message}` , { type: toast.TYPE.ERROR });
            });
     };

    //  const consultarCalificacion = async (idSolicitud: string) => {
    //     const HECHO_MUERTE_DEL_AFILIADO = 'MUERTE DEL AFILIADO';
    //     let hechoDescripcion;
    //     await apiFondoSolidaridad
    //         .getConsultarCalificacion(idSolicitud)
    //         .then(async response => {
    //             if(response.data){
    //                 if(response?.data?.hechoDescripcion === null){
    //                     hechoDescripcion = '';
    //                 }else if(response?.data?.hechoDescripcion === HECHO_MUERTE_DEL_AFILIADO){
    //                     hechoDescripcion = '100';
    //                 }else{
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
    //                 if(response?.data?.porcentajePostulacion === 0){
    //                     formularioCalificacion.setValue('porcentajePostulacion', '');
    //                 }else{
    //                     formularioCalificacion.setValue('porcentajePostulacion', response?.data?.porcentajePostulacion === 100? response?.data?.porcentajePostulacion : 50);
    //                 }
    //                 formularioCalificacion.setValue('observacion', response?.data?.observacion);
    //                 if(response?.data?.hecho !== 100 && response?.data?.hecho !== 0){
    //                     formularioCalificacion.setValue('porcentajeDisminucion', response?.data?.hecho);
    //                     setMostrarCampoPorcentajeDisminucion(true);
    //                 }else{
    //                     formularioCalificacion.setValue('porcentajeDisminucion','');
    //                     setMostrarCampoPorcentajeDisminucion(false);
    //                 }
    //                 if(response?.data?.disfrutePension === null){
    //                     formularioCalificacion.setValue('disfrutePension', datosSolicitudPostulacion?.disfrutePension);
    //                 }else if (response?.data?.disfrutePension !== null){
    //                     formularioCalificacion.setValue('disfrutePension', response?.data?.disfrutePension);
    //                 }
     
    //                 let imputabilidadObject;
    //                 if (datosSolicitudPostulacion.idModalidad === TIPO_MODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA || 
    //                     response?.data?.tipoImputabilidad === TIPO_IMPUTABILIDAD.DISMINUCION){
    //                         imputabilidadObject = imputabilidad_servicio_disminucion_capacidad.find(
    //                             item =>
    //                                 item.id ===
    //                                 response?.data?.imputabilidadAlServicio
    //                         );
    //                 }else if ((datosSolicitudPostulacion.idModalidad === TIPO_MODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA 
    //                     ||  response?.data?.tipoImputabilidad === TIPO_IMPUTABILIDAD.MUERTE)){
    //                         imputabilidadObject = imputabilidad_servicio_muerte.find(
    //                             item =>
    //                                 item.id ===
    //                                 response?.data?.imputabilidadAlServicio
    //                         );
    //                 }
    //                 setImputabilidadValue(imputabilidadObject || null);
    //                 setNullImputabilidadValue(false);
    //                 formularioCalificacion.setValue(
    //                     'imputabilidadAlServicio',
    //                     response.data?.imputabilidadAlServicio,
    //                 );
     
    //                 // tipo imputabilidad
    //              if(datosSolicitudPostulacion.idModalidad === TIPO_MODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA){
    //                  formularioCalificacion.setValue('tipoImputabilidad',TIPO_IMPUTABILIDAD.DISMINUCION);
    //                  formularioCalificacion.setValue('hecho','0');
    //                  formularioCalificacion.setValue('tipoImputabilidad','DISMINUCIÓN DE LA CAPACIDAD LABORAL O MINUSVALÍA');
    //                  setMostrarCampoPorcentajeDisminucion(true);
    //              }else if(datosSolicitudPostulacion.idModalidad === TIPO_MODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA){
    //                  formularioCalificacion.setValue('tipoImputabilidad',TIPO_IMPUTABILIDAD.MUERTE);
    //                  formularioCalificacion.setValue('hecho','100');
    //                  formularioCalificacion.setValue('tipoImputabilidad','MUERTE');
    //                  setMostrarCampoPorcentajeDisminucion(false);
    //              }else{
    //                  formularioCalificacion.setValue('tipoImputabilidad','');
    //                  formularioCalificacion.setValue('hecho','0');
    //                  formularioCalificacion.setValue('tipoImputabilidad','DISMINUCIÓN DE LA CAPACIDAD LABORAL O MINUSVALÍA');
    //                  setMostrarCampoPorcentajeDisminucion(true);
    //              }
    //             }
    //         })
    //         .catch(() => {
    //             toast('Error al consultar calificación en adjudicación vivienda', { type: toast.TYPE.ERROR });
    //         });
    //  };

     const actualizar = (data: any) => {
        const informacionAdjudicacion = {
            adjudicacion: {
                estadoAdjudicacion: data.estadoAdjudicacion,
                radicacionAceptacion: data.radicadoAceptacion,
                idProyecto: data.proyectoAsignar,
                tipoInmueble: 0,
                tipoVivienda: 0,
                claseVivienda: 0,
                valorVivienda: data.valorVivienda
              },
              logistica: {
                timpoDesplazamiento: data.tiempoDesplazamiento,
                distancia: data.distancia,
                acompañante: data.acompaniante,
                tipoIdentificacion: data.tipoIdentificacionAcompaniante,
                numeroDocumento: data.numeroIdentificacionAcompaniante,
                nombreCompleto: data.nombresApellidosAcompaniante
              },
              entregaVivienda: {
                fechaCreacuib: data.fechaEntregaVivienda,
                entregada: data.entregada,
                observacion: data.observacionesEntrega
              }
        }

     }

    const aprobar = async() => {
        const confirmAlert = await NotificationService.confirmAlert(
            `¿Está seguro que desea realizar esta acción?`,
        );
        
        if(confirmAlert){
            console.log('aprobada')
        } 
    
    }

     const rechazar = () => {
        swal({
            title:'Seleccione una opción',
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
                 cancelar:  {
                     text: 'Cancelar',
                     value: null,
                     className: classes.buttonCancelar
                 },
             },
             closeOnClickOutside: false,
         })
         .then(respuesta => {
            if (respuesta === 'rechazar') {

                if(formularioAsignacionViviendaLegislacion.getValues('observacion') === ''){
                    formularioAsignacionViviendaLegislacion.setError('observacion', { type: "focus", message: "Este campo es requerido." })
                    NotificationService.genericAlert(
                        'Información',
                        `Se debe ingresar la observación en Asignación de Vivienda`,
                        'warning'
                    );
                    return;
                }
                formularioAsignacionViviendaLegislacion.clearErrors('observacion');
            }else if(respuesta === 'anular') {
                NotificationService.genericAlert(
                    'Información',
                    `Se debe ingresar la observación en Verificación`,
                    'warning'
                   );
            }

         })
     }

    return (
        <>
            {/* <LoadingComponent loading={loading} /> */}
            <CHPageTitleV2
				topText="Gestión Adjudicación Vivienda"
				title1="Adjudicación Vivienda"
                title2='Fondo Solidaridad'
				icono="fact_check"
			/>
             <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
            />
            <Panel 
                currentTabValue={currentTabValue}
                formularioPostulacion={formularioPostulacion}
                formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
                formularioEntrevista={formularioEntrevista}
                formularioCalificacion={formularioCalificacion}
                formularioAsignacionViviendaLegislacion={formularioAsignacionViviendaLegislacion}
                formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
                catalogState={catalogState}
                datosSolicitudPostulacion={datosSolicitudPostulacion}
                tipoSolicitud={tipoSolicitud}
                listaTiposIntegrantes={listaTiposIntegrantes}
                listaTiposDiscapacidad={listaTiposDiscapacidad}
                listaTiposAyudaTecnica={listaTiposAyudaTecnica}
                imputabilidadValue={imputabilidadValue}
                nullImputabilidadValue={nullImputabilidadValue}
                postulantesTable={postulantesTable}
                totalPorcentajePostulacion={totalPorcentajePostulacion}
                totalPorcentajeAportes={totalPorcentajeAportes}
                totalPorcentajeCesantias={totalPorcentajeCesantias}
                mostrarCampoPorcentajeDisminucion={mostrarCampoPorcentajeDisminucion}
                cliente={cliente}
                datosEntrevista={datosEntrevista}
                postulantesControl={postulantesControl}
                listaPostulantes={listaPostulantes}
                setPostulantesTable={setPostulantesTable}
                setListaPostulantes={setListaPostulantes}
                consultarTiposIntegrantes={consultarTiposIntegrantes}
                consultarTiposDiscapcidad={consultarTiposDiscapcidad}
                consultarTiposAyudaTecnica={consultarTiposAyudaTecnica}
                onChangeImputabilidad={onChangeImputabilidad}
                setTotalPorcentajePostulacion={setTotalPorcentajePostulacion}
                setTotalPorcentajeAportes={setTotalPorcentajeAportes}
                setTotalPorcentajeCesantias={setTotalPorcentajeCesantias}
                consultarEntrevista={consultarEntrevista}
                consultarCalificacion={consultarCalificacion}
            />
            <Grid container>
                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={formularioAsignacionViviendaLegislacion.handleSubmit(actualizar)}
                    >
                        ACTUALIZAR
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        onClick={aprobar}
                        >
                        APROBAR 
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        onClick={rechazar}
                    >
                        RECHAZAR
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="default"
                        onClick={regresarPantallaInicial}
                    >
                        REGRESAR
                    </Button>
                </Grid>
            </Grid>
        </>
    )
}

type Props = {
    catalogState: CatalogState;
    cliente: any;
    datosSolicitudPostulacion: any;
    tipoSolicitud: number;
    mostrarCampoPorcentajeDisminucion: boolean;
    formularioEntrevista: any;
    formularioPostulacion: any;
    formularioCalificacion: any;
    formularioResumenInformacionSolicitud: any;
    formularioObservacionCargueArchivos: any;
    listaPostulantes: Postulante[];
    setListaPostulantes: (postulantes: Postulante[]) => void;
    regresarPantallaInicial: () => void;
    setMostrarCampoPorcentajeDisminucion: (value: boolean) => void;
    consultarCalificacion: (idSolicitud: string) => void;
}

export default AdjudicacionVivienda;