import { createStyles, 
         Theme,
         Tab,
         Tabs, 
         makeStyles,
         withStyles, 
         Button,
         Grid} from '@material-ui/core';
import React, { useState, useEffect } from 'react';
import swal from 'sweetalert';
import { useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import ApiCatalogo from '../../../services/api-catalog.service';
import NotificationService from '../../../services/notification.service';
import { CITYDTO } from '../../../types/catalog.type';
import UtilService from '../../../utils/util.service';
import { ESTADO_PROYECTO, TABS_INDEX_PROYECTOS, TIPO_CONTRATO_PROYECTO } from '../constants/fondoSolidaridad.constants';
import { BeneficiarioPago, Supervisor, Poliza, Inmueble, ItemAvance, ClaseInmueble, Supervision } from '../constants/fondosolidaridadform.model.type';
import ConstructoraSupervisorBeneficiarioPago from './ConstructoraSupervisorBeneficiarioPago';
import ContratoProyecto from './ContratoProyecto';
import Inmuebles from './Inmuebles';
import Polizas from './Poliza';
import Proyectos from './Proyectos';
import SupervisorTecnico from './SupervisorTecnico';

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
    mostrarProyectoContrato,
    mostrarPoliza,
    mostrarInmueble,
    mostrarSupervisionTecnica,
}: {
    currentTab: number;
    mostrarProyectoContrato: boolean;
    mostrarPoliza: boolean;
    mostrarInmueble: boolean;
    mostrarSupervisionTecnica: boolean;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
}) {
    return (
        <AntTabs
            value={currentTab}
            onChange={onTabChange}
            aria-label="ant example"
        >
            <AntTab
                label="Supervisor y Beneficiario de Pago"  value={TABS_INDEX_PROYECTOS.CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO}
            />
            {
                mostrarProyectoContrato && (
                <AntTab
                  label="Proyecto y Contrato" value={TABS_INDEX_PROYECTOS.PROYECTO_Y_CONTRATO}
                />
              )
            } 
           {
                mostrarPoliza && (
                <AntTab label="Póliza" value={TABS_INDEX_PROYECTOS.POLIZA} />
              
            )}
            {
                mostrarInmueble && (
                <AntTab label="Inmuebles" value={TABS_INDEX_PROYECTOS.INMUEBLES} />
              )
            } 
            {
                mostrarSupervisionTecnica && (
                    <AntTab label="Supervisión Técnica" value={TABS_INDEX_PROYECTOS.SUPERVISOR_TECNICO} />
                )
            } 
        </AntTabs>
    );
}

function PanelProyectos ({
    currentTabValue,
    formularioProyecto,
    informacionProyecto,
    informacionSupervisores,
    informacionBeneficiarios,
    informacionPolizas,
    informacionTiposInmuebles,
    informacionClasesInmuebles,
    informacionItemsAvance, 
    ciudades,
    ciudadesLicenciaConstruccion,
    ciudadesLicenciaUrbanismo,
    estadoProyecto,
    setSupervisores,
    setBeneficiarios,
    obtenerCiudades,
    obtenerCiudadesLicenciaConstruccion,
    obtenerCiudadesLicenciaUrbanismo,
    setPoliza,
    setInmuebles,
    setClasesInmueble,
    setItemAvances,
    setSupervisiones,
}:{
    currentTabValue: number;
    formularioProyecto: UseFormReturn<any>;
    informacionProyecto: any;
    informacionSupervisores: any[];
    informacionBeneficiarios: any[];
    informacionPolizas: any[];
    informacionTiposInmuebles: any[];
    informacionClasesInmuebles: any[];
    informacionItemsAvance: any[];
    ciudades: CITYDTO[];
    ciudadesLicenciaConstruccion: CITYDTO[];
    ciudadesLicenciaUrbanismo: CITYDTO[];
    estadoProyecto: number;
    setSupervisores: (value: Supervisor[]) => void;
    setBeneficiarios: (value: BeneficiarioPago[]) => void;
    obtenerCiudades: (idDepartamento: number) => void;
    obtenerCiudadesLicenciaConstruccion: (idDepartamento: number) => void;
    obtenerCiudadesLicenciaUrbanismo: (idDepartamento: number) => void;
    setPoliza: (value: Poliza[]) => void;
    setInmuebles: (value: Inmueble[]) => void;
    setClasesInmueble: (value: ClaseInmueble[]) => void;
    setItemAvances: (value: ItemAvance[]) => void;
    setSupervisiones: (value: Supervision[]) => void;
}) {

    switch (currentTabValue) {
        case TABS_INDEX_PROYECTOS.CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO:
            return (
                <ConstructoraSupervisorBeneficiarioPago 
                    informacionProyecto={informacionProyecto}
                    informacionSupervisores={informacionSupervisores}
                    informacionBeneficiarios={informacionBeneficiarios}
                    estadoProyecto={estadoProyecto}
                    setSupervisores={setSupervisores}
                    setBeneficiarios={setBeneficiarios}
                />
            );
        case TABS_INDEX_PROYECTOS.PROYECTO_Y_CONTRATO:
            return (
               <ContratoProyecto
                  informacionProyecto={informacionProyecto}
                  formularioProyecto={formularioProyecto}
                  ciudades={ciudades}
                  ciudadesLicenciaConstruccion={ciudadesLicenciaConstruccion}
                  ciudadesLicenciaUrbanismo={ciudadesLicenciaUrbanismo}
                  obtenerCiudades={obtenerCiudades}
                  obtenerCiudadesLicenciaConstruccion={obtenerCiudadesLicenciaConstruccion}
                  obtenerCiudadesLicenciaUrbanismo={obtenerCiudadesLicenciaUrbanismo}
                  estadoProyecto={estadoProyecto}
               />
            );
        case TABS_INDEX_PROYECTOS.POLIZA:
            return (
              <Polizas   
                  informacionProyecto={informacionProyecto}
                  informacionPolizas={informacionPolizas}
                  setPoliza={setPoliza} 
                  estadoProyecto={estadoProyecto}
              />
            );
        case TABS_INDEX_PROYECTOS.INMUEBLES:
        return (
              <Inmuebles 
                  informacionTiposInmuebles={informacionTiposInmuebles}
                  informacionClasesInmuebles={informacionClasesInmuebles}
                  informacionItemsAvance={informacionItemsAvance}
                  setInmuebles={setInmuebles}
                  setClasesInmueble={setClasesInmueble}
                  setItemAvances={setItemAvances}
                  estadoProyecto={estadoProyecto}
               />
            );
        case TABS_INDEX_PROYECTOS.SUPERVISOR_TECNICO: 
        return (
            <SupervisorTecnico
               informacionProyecto={informacionProyecto}
               informacionSupervisores={informacionSupervisores}
               setSupervisiones={setSupervisiones}
            />
        );
        default:
            return null;
    }
};

const AdministracionProyectos = (props: Props) => {

    const classes = useStyles();
    const apiCatalog = ApiCatalogo();
    const {informacionProyecto,onItemIsSelected} = props;
    const apiFondoSolidaridad= ApiFondoSolidaridad();
    const [currentTabValue, setCurrentTabValue] = useState(
        TABS_INDEX_PROYECTOS.CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO,
    );
    const [regresarInicio, setRegresarInicio] = useState(false);
    const [supervisores, setSupervisores] = useState<Supervisor[]>([]);
    const [beneficiarios, setBeneficiarios] = useState<BeneficiarioPago[]>([]);
    const [poliza, setPoliza] = useState<Poliza[]>([]);
    const [inmuebles, setInmuebles] = useState<Inmueble[]>([]);
    const [itemAvances, setItemAvances] = useState<ItemAvance[]>([]);
    const [clasesInmueble, setClasesInmueble] = useState<ClaseInmueble[]>([]);
    const [supervisiones,setSupervisiones] = useState<Supervision[]>([]);
    const [ciudades,setCiudades] = useState<CITYDTO[]>([]);
    const [ciudadesLicenciaConstruccion,setCiudadesLicenciaConstruccion] = useState<CITYDTO[]>([]);
    const [ciudadesLicenciaUrbanismo,setCiudadesLicenciaUrbanismo] = useState<CITYDTO[]>([]);
    const [mostrarProyectoContrato, setMostrarProyectoContrato] = useState(false);
    const [mostrarPoliza, setMostrarPoliza] = useState(false);
    const [mostrarInmueble, setMostrarInmueble] = useState(false);
    const [mostrarSupervisionTecnica, setMostrarSupervisionTecnica] = useState(false);
    const [camposLecturaSupervicionTecnica, setCamposLecturaSupervicionTecnica] = useState(false);
    const [idProyecto, setIdProyecto] = useState('');

    const formularioProyecto = useForm({
        defaultValues: {
            proyectoId: '',
            constructora: '',
            nombreProyecto: '',
            direccion: '',
            departamentoId: '',
            ciudadId: '',
            telefono: '',
            valorProyecto: '',
            cupos: '',
            latitud: '',
            longitud: '',
            tipoProyecto: '', 
            estado: ESTADO_PROYECTO.EN_CREACION,
            nombreEstado: '',
            fechaContrato: new Date(),
            fechaVigencia: new Date(),
            fechaActaInicio: new Date(),
            fechaEntrega: new Date(),
            objetoContrato: '',
            tipoContrato: '',
            numeroRegistro: '',
            resolucion: '',
            curaduria: '',
            departamentoLicenciaConstruccionId: '',
            ciudadLicenciaConstruccionId: '',
            fechaVigenciaLicenciaConstruccion: new Date(),
            fechaResolucion: new Date(),
            titularLicenciaConstruccion: '',
            resolucionUrbanismo: '',
            curaduriaUrbanismo: '',
            departamentoLicenciaUrbanismoId: '',
            ciudadLicenciaUrbanismoId: '',
            fechaVigenciaUrbanismo: new Date(),
            titularLicenciaUrbanismo: '',
            fechaResolucionUrbanismo: new Date(),
            mismaLicencia: false
        },
    });

    useEffect(() => {
        if(informacionProyecto && informacionProyecto?.nombreProyecto){
            setIdProyecto(informacionProyecto?.codigo);
            obtenerCiudades(informacionProyecto?.departamentoId);
            formularioProyecto.setValue('proyectoId',informacionProyecto?.codigo);
            formularioProyecto.setValue('constructora',informacionProyecto?.constructoraId);
            formularioProyecto.setValue('nombreProyecto',informacionProyecto?.nombreProyecto);
            formularioProyecto.setValue('direccion',informacionProyecto?.direccion);
            formularioProyecto.setValue('departamentoId',informacionProyecto?.departamentoId);
            formularioProyecto.setValue('ciudadId',informacionProyecto?.municipioId);
            formularioProyecto.setValue('telefono',informacionProyecto?.telefono);
            formularioProyecto.setValue('valorProyecto',informacionProyecto?.valor);
            formularioProyecto.setValue('cupos',informacionProyecto?.cupos);
            formularioProyecto.setValue('latitud',informacionProyecto?.latitud);
            formularioProyecto.setValue('longitud',informacionProyecto?.longitud);
            formularioProyecto.setValue('tipoProyecto',informacionProyecto?.tipoProyecto);
            formularioProyecto.setValue('estado',informacionProyecto?.estadoId);
            formularioProyecto.setValue('fechaContrato',informacionProyecto?.fechaCreacion);
            formularioProyecto.setValue('fechaVigencia',informacionProyecto?.fechaVigencia);
            formularioProyecto.setValue('fechaActaInicio',informacionProyecto?.fechaInicio);
            formularioProyecto.setValue('fechaEntrega',informacionProyecto?.fechaEntrega);
            formularioProyecto.setValue('objetoContrato',informacionProyecto?.objeto);
            formularioProyecto.setValue('tipoContrato',informacionProyecto?.tipoContrato === 'FIDUCIA'? String(TIPO_CONTRATO_PROYECTO.FIDUCIA) : String(TIPO_CONTRATO_PROYECTO.CAJA_HONOR));
            formularioProyecto.setValue('numeroRegistro',informacionProyecto?.numeroRegistro);

            obtenerCiudadesLicenciaConstruccion(informacionProyecto?.licenciaConstruccion?.deparamentoId);
            formularioProyecto.setValue('resolucion',informacionProyecto?.licenciaConstruccion?.resolucion);
            formularioProyecto.setValue('curaduria',informacionProyecto?.licenciaConstruccion?.curaduria);
            formularioProyecto.setValue('departamentoLicenciaConstruccionId',informacionProyecto?.licenciaConstruccion?.deparamentoId);
            formularioProyecto.setValue('ciudadLicenciaConstruccionId',informacionProyecto?.licenciaConstruccion?.municipioId);
            formularioProyecto.setValue('fechaVigenciaLicenciaConstruccion',informacionProyecto?.licenciaConstruccion?.fechaVigencia);
            formularioProyecto.setValue('titularLicenciaConstruccion',informacionProyecto?.licenciaConstruccion?.titular);
            formularioProyecto.setValue('fechaResolucion',informacionProyecto?.licenciaConstruccion?.fechaResolucion);
            formularioProyecto.setValue('mismaLicencia', informacionProyecto?.licenciaConstruccion?.mismaLicencia === 1);

            obtenerCiudadesLicenciaUrbanismo(informacionProyecto?.licenciaUrbanismo?.deparamentoId);
            formularioProyecto.setValue('resolucionUrbanismo',informacionProyecto?.licenciaUrbanismo?.resolucion);
            formularioProyecto.setValue('curaduriaUrbanismo',informacionProyecto?.licenciaUrbanismo?.curaduria);
            formularioProyecto.setValue('departamentoLicenciaUrbanismoId',informacionProyecto?.licenciaUrbanismo?.deparamentoId);
            formularioProyecto.setValue('ciudadLicenciaUrbanismoId',informacionProyecto?.licenciaUrbanismo?.municipioId);
            formularioProyecto.setValue('fechaVigenciaUrbanismo',informacionProyecto?.licenciaUrbanismo?.fechaVigencia);
            formularioProyecto.setValue('titularLicenciaUrbanismo',informacionProyecto?.licenciaUrbanismo?.titular);
            formularioProyecto.setValue('fechaResolucionUrbanismo',informacionProyecto?.licenciaUrbanismo?.fechaResolucion);
            setMostrarProyectoContrato(true);

            if(informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION ||
               informacionProyecto?.estadoId === ESTADO_PROYECTO.VIGENTE ||
               informacionProyecto?.estadoId === ESTADO_PROYECTO.ANULADO && informacionProyecto?.polizas !== null){
                setMostrarPoliza(true);
                window.scroll({top:100,})
            }
          
            if((informacionProyecto?.polizas && 
               informacionProyecto?.polizas.length > 0 &&
               informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION) ||
               informacionProyecto?.estadoId === ESTADO_PROYECTO.VIGENTE ||
               informacionProyecto?.estadoId === ESTADO_PROYECTO.ANULADO &&
               informacionProyecto?.inmublesProyectos !== null &&
               informacionProyecto?.clasesInmuebles !== null &&
               informacionProyecto?.listaAvance !== null){
                setMostrarInmueble(true);
                window.scroll({top:100,})
            }

            if(informacionProyecto?.inmublesProyectos && 
               informacionProyecto?.inmublesProyectos.length > 0){
               setMostrarSupervisionTecnica(true);
               consultarSupervisionTecnica(informacionProyecto?.codigo);
               window.scroll({top:100,})
            }
        }
    },[informacionProyecto])

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        setCurrentTabValue(newValue);
    };

    const consultarSupervisionTecnica = async(idProyecto: string) => {
        await apiFondoSolidaridad.getConsultarSupervisionTecnica(idProyecto)
        .then(response => {
            if(response.data && response.data.length > 0){
                setCamposLecturaSupervicionTecnica(true);
                const listaSupervisiones = response.data.map((supervision: any) => {
                    if(supervision){
                        const supervisionNueva: Supervision = {
                            id: supervision.idSupervicionTecnica,
                            personaAtendioVisita: supervision.personaAtendioVisita,
                            cargo: supervision.cargo,
                            supervisorTecnico: supervision.supervisoTecnicoTipoDocumento,
                            nombreSupervisorTecnico: supervision.nombreSupervisorTecnico,
                            fechaVisita: supervision.fechaVisita,
                            desarrolloProyecto: supervision.desarrolloProyecto, 
                            observacionesGenerales: supervision.observacionGeneral,
                        }
                        return supervisionNueva;
                    }
                    return null;
              })
              setSupervisiones(listaSupervisiones);
            }else{
                setCamposLecturaSupervicionTecnica(false); 
            }
        }).catch(() => {
            toast(`Error al consultar supervisiones en administración proyectos`, {type: toast.TYPE.ERROR});
        })
    }

    const obtenerCiudades = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudades(datos);
        }
    };

    const obtenerCiudadesLicenciaConstruccion = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudadesLicenciaConstruccion(datos);
        }
    };

    const obtenerCiudadesLicenciaUrbanismo = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudadesLicenciaUrbanismo(datos);
        }
    };

    const opciones = async() => {

        if(informacionProyecto === null){
            if(supervisores.length === 0){
                toast('Debe registrar al menos un supervisor',{type: toast.TYPE.ERROR});
                return;
            }
    
            if(beneficiarios.length === 0){
                toast('Debe registrar al menos un beneficiario de pago',{type: toast.TYPE.ERROR});
                return;
            }
            
            // validacion temporal && currentTabValue === TABS_INDEX_PROYECTOS.CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO && mostrarPoliza === false
            if(supervisores.length > 0 && 
               beneficiarios.length > 0 && 
               currentTabValue === TABS_INDEX_PROYECTOS.CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO && 
               mostrarPoliza === false){
                setMostrarProyectoContrato(true);
                window.scroll({top:100,})
                setCurrentTabValue(TABS_INDEX_PROYECTOS.PROYECTO_Y_CONTRATO);
            }
        }

        // crear Poliza
        if(informacionProyecto && 
           informacionProyecto?.nombreProyecto && 
           informacionProyecto?.polizas === null &&
           informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION){

            if(poliza.length === 0){
                toast('Debe registrar al menos una poliza',{type: toast.TYPE.ERROR});
                setCurrentTabValue(TABS_INDEX_PROYECTOS.POLIZA);
                return;
            }

            crearPolizas(poliza);
        }

        // crear inmuebles
        if(informacionProyecto?.polizas && 
           informacionProyecto?.polizas.length > 0 &&
           informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION){

            if(inmuebles.length === 0){
                toast('Debe registrar al menos un inmueble',{type: toast.TYPE.ERROR});
            }

            if(clasesInmueble.length === 0){
                toast('Debe registrar al menos una clase inmueble',{type: toast.TYPE.ERROR});
            }

            if(itemAvances.length === 0){
                toast('Debe registrar al menos un item de avance',{type: toast.TYPE.ERROR});
            }

            if(inmuebles.length > 0 && itemAvances.length > 0 && clasesInmueble.length > 0){
                crearInmuebles(inmuebles,clasesInmueble,itemAvances);
            }
        }

        // // Crear supervisiones
        if(informacionProyecto?.inmublesProyectos && 
            informacionProyecto?.inmublesProyectos.length > 0 &&
            informacionProyecto?.estadoId === ESTADO_PROYECTO.VIGENTE){
            if(supervisiones && supervisiones.length === 0 && currentTabValue === TABS_INDEX_PROYECTOS.SUPERVISOR_TECNICO){
                toast('Debe registrar al menos una supervisión',{type: toast.TYPE.ERROR});
            }

            if(supervisiones && supervisiones.length > 0 && currentTabValue === TABS_INDEX_PROYECTOS.SUPERVISOR_TECNICO){
                crearSupervision(supervisiones);
            }
        }
    }

    function formatoHoraDosDigitos(paramHora: number) {
        return paramHora < 10 ? `0${paramHora}` : String(paramHora);
    }

    const guardarProyectoContrato = async(data: any) => {

        const fecha = new Date(data.fechaContrato);
        const hora = formatoHoraDosDigitos(fecha.getHours());
        const minutos = fecha.getMinutes().toString().padStart(2, '0');
        const segundos = fecha.getSeconds().toString().padStart(2, '0');
        const anio = fecha.getFullYear();
        const mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
        const dia = fecha.getDate().toString().padStart(2, '0');

        const fechaFinal = `${anio}-${mes}-${dia}T${hora}:${minutos}:${segundos}Z`;

        if(currentTabValue === TABS_INDEX_PROYECTOS.PROYECTO_Y_CONTRATO){
            const listaSupervisores: any[] = supervisores.map(supervisor => {
                if(supervisor){
                    return({
                        sP_TIPO_DOCUMENTO: supervisor.tipoDocumento,
                        sP_NUMERO_DOCUMENTO: supervisor.numeroDocumento,
                        sP_NOMBRE_APELLIDO: supervisor.nombresApellidos,
                        sP_CARGO: supervisor.cargo,
                        sP_EMAIL: supervisor.email,
                        sP_TELEFONO: supervisor.telefono,
                    })
                }
                return null;
            })
            const listaBeneficiarios: any[] = beneficiarios.map(beneficiario => {
                if(beneficiario){
                    return({
                        bP_TIPO_DOCUMENTO: beneficiario.tipoDocumento,
                        bP_NUMERO_DOCUMENTO: beneficiario.numeroDocumento,
                        bP_DIGITO_VEFICACION: beneficiario.digitoVerificacion,
                        bP_RAZON_SOCIAL: beneficiario.nombreRazonSocial,
                        bP_TIPO_CUENTA: beneficiario.tipoCuenta,
                        bP_TIPO_ENTIDAD: String(beneficiario.entidad),
                        bP_ACTIVIDAD_CUENTA: beneficiario.actividadEconomica,
                        bP_NUMERO_CUENTA: beneficiario.numeroCuenta,
                        bP_DIRECCION: beneficiario.direccion,
                    })
                }
                return null;
            })
            const proyectoNuevo = {
                pC_CONTRUCTORA_ID: data.constructora,
                pC_NOMBRE: String(data.nombreProyecto).toUpperCase(),
                pC_DIRECCION: data.direccion,
                pC_DEPARTAMENTO_ID: data.departamentoId,
                pC_MUNICIPIO_ID: data.ciudadId,
                pC_TELEFONO: data.telefono,
                pC_VALOR: UtilService.cleanCurrencyValue(data.valorProyecto),
                pC_CUPOS: data.cupos,
                pC_LATITUD: data.latitud,
                pC_LONGITUD: data.longitud,
                pC_ESTADO_ID: data.estado,
                pC_FECHA_CREACION: fechaFinal,
                pC_FECHA_VIGENCIA: data.fechaVigencia,
                pC_OBJETO: data.objetoContrato,
                pC_TIPO_CONTRATO_ID: data.tipoContrato,
                pC_NUMERO_REGISTRO: String(data.numeroRegistro),
                pC_FECHA_ACTA_INICIO: data.fechaActaInicio,
                pC_FECHA_ENTREGA: data.fechaEntrega,
                pC_TIPO_PROYECTO: data.tipoProyecto,
                licenciaConstrucionDtos: {
                    luP_RESOLUCION: data.resolucion,
                    luP_CURADURIA: data.curaduria,
                    luP_DEPARTAMENTO_ID: data.departamentoLicenciaConstruccionId,
                    luP_MUNICIPIO_ID: data.ciudadLicenciaConstruccionId,
                    luP_DATE_VIGENCIA: data.fechaVigenciaLicenciaConstruccion,
                    luP_TITULAR: data.titularLicenciaConstruccion,
                    luP_DATE_RESOLUCION: data.fechaResolucion,
                    luP_MISMA_LICENCI: data.mismaLicencia === true? 1:0,
                },
                licenciaUrbanismoDtos: {
                    luP_RESOLUCION: data.resolucionUrbanismo,
                    luP_CURADURIA: data.curaduriaUrbanismo,
                    luP_DEPARTAMENTO_ID: data.departamentoLicenciaUrbanismoId,
                    luP_MUNICIPIO_ID: data.ciudadLicenciaUrbanismoId,
                    luP_DATE_VIGENCIA: data.fechaVigenciaUrbanismo,
                    luP_TITULAR: data.titularLicenciaUrbanismo,
                    luP_DATE_RESOLUCION: data.fechaResolucionUrbanismo,
                    luP_MISMA_LICENCI: data.mismaLicencia === true? 1:0,
                },
                supervisor:listaSupervisores,
                beneficiarios: listaBeneficiarios
            }
           crearProyecto(proyectoNuevo);
        }
    }

    const crearProyecto = async(proyectoNuevo: any) => {
        await apiFondoSolidaridad.postCrearProyecto(proyectoNuevo)
             .then(response => {
                 if(response?.returnMessage === 'OK'){
                    NotificationService.genericAlert(
                        'Guardado!',
                        'Información almacenada',
                    );
                    onItemIsSelected(response?.data);
                    setIdProyecto(response?.data);
                    window.scrollTo(1000,0);
                    setMostrarPoliza(true);
                    setCurrentTabValue(TABS_INDEX_PROYECTOS.POLIZA);
                 }
             }).catch((e) => {
                 toast(`Error al crear Proyecto ${e.message}`, {type: toast.TYPE.ERROR});
             })
    }

    const crearPolizas = async(listaPolizas: any) => {
        const arrayPolizas: any[] = listaPolizas.map((poliza: Poliza) => {
            if(poliza){
                const polizaNueva = {
                    pP_CONCEPTOPOLIZA_ID: poliza?.concepto,
                    pP_RADICADOPOLIZA: poliza?.radicado,
                    pP_FECHAINICIO: new Date(poliza?.fechaInicioPoliza),
                    pP_FECHAVIGENCIA: new Date(poliza?.fechaVigencia),
                    pP_NUMEROPOLIZA: poliza?.numeroPoliza,
                    pP_ASEGURADORA: poliza?.aseguradora,
                    pP_VALORPOLIZA: UtilService.cleanCurrencyValue(poliza?.valorPoliza),
                    pP_PORCENTAJE: Number(String(poliza?.porcentajePoliza).replace('%','')),
                    pP_AJUSTADOR: poliza?.ajustador,
                    pP_DESCRIPCION: poliza?.descripcionPoliza,
                    pC_PROYECTO_ID: idProyecto
                }
               return polizaNueva;
            }
            return null;
        })
        await apiFondoSolidaridad.postCrearPolizasProyecto(arrayPolizas)
             .then(response => {
                 if(response?.returnMessage === 'OK'){
                    NotificationService.genericAlert(
                        'Guardado!',
                        'Información almacenada',
                    );
                   onItemIsSelected(idProyecto);
                   setMostrarInmueble(true);
                   window.scroll({top:100,})
                   setCurrentTabValue(TABS_INDEX_PROYECTOS.INMUEBLES);
                }
             }).catch(() => {
                 toast(`Error al crear Poliza`, {type: toast.TYPE.ERROR});
             })
    }

    const crearInmuebles = async(inmuebles: any,clasesInmuebles: any,itemAvances: any) => {
        const listaInmuebles = inmuebles.map((inmueble: Inmueble) => {
            if(inmueble){
                const claseInmuebleNueva = {
                    codigoTipoInmueble: inmueble.tipoInmueble,
                    valor: UtilService.cleanCurrencyValue(inmueble.valor),
                    areaPrivada: inmueble.areaPrivada,
                    areaConstruida: inmueble.areaConstruida,
                    codigoTipoVivienda: inmueble.tipoVivienda,
                    codigoClaseVivienda: inmueble.claseVivienda,
                    cantidad: inmueble.cantidad,
                    descripcion: inmueble.descripcion,
                    proyectoId: idProyecto,
                }
                return claseInmuebleNueva;
            }
            return null;
       })
       const listaClasesInmuebles = clasesInmuebles.map((claseInmueble: ClaseInmueble) => {
            if(claseInmueble){
                const claseInmuebleNueva = {
                    proyectoId: idProyecto,
                    claseVivienda: claseInmueble.claseInmueble,
                    numeroVivienda: claseInmueble.numeroInmueble,
                    direccionInmueble: claseInmueble.direccionInmueble
                }
                return claseInmuebleNueva;
            }
            return null;
       })
       const listaItemsAvance = itemAvances.map((item: ItemAvance) => {
        if(item){
            const itemAvanceNuevo = {
                    proyectoId: idProyecto,
                    porcentaje: Number(String(item.porcentaje).replace('%','')),
                    itemAvance: item.itemAvance
            }
            return itemAvanceNuevo;
        }
        return null;
   })

   const arrayInmuebles = {
         inmuebles: listaInmuebles,
         claseInmuebles: listaClasesInmuebles,
         listaAvances: listaItemsAvance
   }

   await apiFondoSolidaridad.postCrearInmueblesProyecto(arrayInmuebles)
   .then(response => {
       if(response?.returnMessage === 'OK'){
          const request = {
            codigoProyecto: ESTADO_PROYECTO.VIGENTE,
            proyectoId: idProyecto
          }
          apiFondoSolidaridad.postActualizarEstadoProyecto(request)
          .then(responseEstado => {
                if(responseEstado?.data === 'OK'){
                    NotificationService.genericAlert(
                        'Guardado!',
                        'Información almacenada',
                    );
                   onItemIsSelected(idProyecto);
                   setMostrarSupervisionTecnica(true);
                   window.scroll({top:100,})
                   setCurrentTabValue(TABS_INDEX_PROYECTOS.SUPERVISOR_TECNICO);
                }
          })
      }
   }).catch(() => {
       toast(`Error al crear Inmuebles`, {type: toast.TYPE.ERROR});
   })
}

const crearSupervision = async(supervisiones: any) => {

    const listaSupervisiones = supervisiones.map((supervision: Supervision) => {
        if(supervision){
            const supervisionNueva = {
                idSupervicionTecnica: supervision.id,
                idProyecto: idProyecto,
                personaAtendioVisita: supervision.personaAtendioVisita,
                cargo: supervision.cargo,
                supervisoTecnicoTipoDocumento: String(supervision.supervisorTecnico),
                nombreSupervisorTecnico: supervision.nombreSupervisorTecnico,
                fechaVisita: supervision.fechaVisita,
                desarrolloProyecto: supervision.desarrolloProyecto,
                observacionGeneral: supervision.observacionesGenerales,
            }
            return supervisionNueva;
        }
        return null;
   })

    await apiFondoSolidaridad.postGuardarInformacionSupervisionTecnica(listaSupervisiones)
    .then(response => {
        if(response?.returnMessage === 'OK'){
            NotificationService.genericAlert(
                'Guardado!',
                'Información almacenada',
            );
           window.scroll({top:100,})
           onClickRegresar();
       }
    }).catch(() => {
        toast(`Error al crear Supervisiones`, {type: toast.TYPE.ERROR});
    })
}

    const anular = () => {
        swal({
            text: '¿Está seguro que desea anular el proyecto?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                const request = {
                    codigoProyecto: ESTADO_PROYECTO.ANULADO,
                    proyectoId: idProyecto
                }
                apiFondoSolidaridad.postActualizarEstadoProyecto(request)
                .then(responseEstado => {
                      if(responseEstado?.data === 'OK'){
                         NotificationService.genericAlert(
                            'Exitoso!',
                            'Se realizó la petición con éxito.',
                         );
                         window.scroll({top:100,})
                         onClickRegresar();
                      }
                })
            }
        });
    }

    const onClickRegresar = () => {
        setRegresarInicio(true);
    }

    if(regresarInicio){
        return(
          <Proyectos />
        )
    }

    return (
        <>
            <CHPageTitle
                    topText="Gestión de Proyectos"
                    subText=""
                    title1="Administración de"
                    title2="Proyectos de Vivienda"
            />
            <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
                mostrarProyectoContrato={mostrarProyectoContrato}
                mostrarPoliza={mostrarPoliza}
                mostrarInmueble={mostrarInmueble}
                mostrarSupervisionTecnica={mostrarSupervisionTecnica}
            />
            <PanelProyectos
                currentTabValue={currentTabValue}
                formularioProyecto={formularioProyecto}
                informacionProyecto={informacionProyecto}
                estadoProyecto={informacionProyecto? informacionProyecto.estadoId : 0}
                informacionSupervisores={informacionProyecto? informacionProyecto?.supervisors : supervisores}
                informacionBeneficiarios={informacionProyecto? informacionProyecto?.beneficiario: beneficiarios} 
                informacionPolizas={informacionProyecto? informacionProyecto?.polizas : poliza}  
                informacionTiposInmuebles={informacionProyecto? informacionProyecto?.inmublesProyectos : inmuebles}
                informacionClasesInmuebles={informacionProyecto? informacionProyecto?.clasesInmuebles : clasesInmueble}   
                informacionItemsAvance={informacionProyecto? informacionProyecto?.listaAvance : itemAvances}             
                ciudades={ciudades}
                ciudadesLicenciaConstruccion={ciudadesLicenciaConstruccion}   
                ciudadesLicenciaUrbanismo={ciudadesLicenciaUrbanismo}  
                setSupervisores={setSupervisores}
                setBeneficiarios={setBeneficiarios}
                obtenerCiudades={obtenerCiudades}
                obtenerCiudadesLicenciaConstruccion={obtenerCiudadesLicenciaConstruccion}
                obtenerCiudadesLicenciaUrbanismo={obtenerCiudadesLicenciaUrbanismo}
                setPoliza={setPoliza}
                setInmuebles={setInmuebles}
                setClasesInmueble={setClasesInmueble}
                setItemAvances={setItemAvances}
                setSupervisiones={setSupervisiones}
            />
            <Grid container>
                <Grid className={classes.columnButtonAdd} item xs={12} md={12} >
                {
                    ((mostrarProyectoContrato === false &&
                     mostrarPoliza === false &&
                     mostrarInmueble === false &&
                     mostrarSupervisionTecnica === false) ||
                     (currentTabValue === TABS_INDEX_PROYECTOS.CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO &&
                      mostrarProyectoContrato === true &&
                      mostrarPoliza === false &&
                      mostrarInmueble === false &&
                      mostrarSupervisionTecnica === false &&
                      (informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION ||
                       informacionProyecto?.estadoId === undefined ))) && (
                        <Button
                            className={classes.button}
                            variant="outlined"
                            color="default"
                            onClick={opciones}
                        >
                            SIGUIENTE
                        </Button>
                       )
                    }
                    {mostrarProyectoContrato === true && 
                     mostrarPoliza === false && 
                     mostrarInmueble === false &&
                     mostrarSupervisionTecnica === false &&
                     currentTabValue === TABS_INDEX_PROYECTOS.PROYECTO_Y_CONTRATO &&
                     (informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION || 
                      informacionProyecto?.estadoId === undefined )
                     &&(
                      <Button
                          className={classes.button}
                          variant="outlined"
                          color="default"
                          onClick={formularioProyecto.handleSubmit(guardarProyectoContrato)}
                      >
                          GUARDAR
                      </Button>
                    )}
                    {mostrarProyectoContrato === true &&
                    (informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION || informacionProyecto?.estadoId === ESTADO_PROYECTO.VIGENTE) &&
                    (mostrarPoliza === true || mostrarInmueble === true ||  mostrarSupervisionTecnica === true) && !camposLecturaSupervicionTecnica
                      &&  (
                            <Button
                                className={classes.button}
                                variant="outlined"
                                color="default"
                                onClick={opciones}
                            >
                            GUARDAR
                        </Button>
                        )
                    }
                    {mostrarProyectoContrato === true &&
                     (informacionProyecto?.estadoId === ESTADO_PROYECTO.EN_CREACION ||
                      informacionProyecto?.estadoId === ESTADO_PROYECTO.VIGENTE) && (
                        <Button
                            className={classes.button}
                            variant="outlined"
                            color="default"
                            onClick={anular}
                        >
                           ANULAR
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
        </>
    )
}

type Props = {
    informacionProyecto: any;
    onItemIsSelected: (idProyecto: string) => void;
}

export default AdministracionProyectos;