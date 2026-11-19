import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import {
	Button,
	Card,
	CardContent,
	Container,
	createStyles,
	Grid,
	makeStyles,
	MenuItem,
	Theme,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import '../../solicitudes/solicitudes.styles.scss';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import CHSelect from '../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { startGetAllIdentificationTypes, 
         startGetAllForces, 
         startGetAllGrades, 
         startGetCategory,
		 startGetAllExecutingUnits,
         startGetPhoneTypes,
		 startGetAllCivilState,
		 startGetAllProfessions,
		 startGetAllDepartments} from '../../../actions/catalog.action';
import Postulacion from './Postulacion';
import ApiClient from '../../../services/api-clients.service';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import ApiCredito from '../../../services/api-credit.service';
import { ModalRetiroDesvinculacion } from './components/ModalRetiroDesvinculacion';
import { ACTIVIDAD_POSTULACION, catalogPostulacionDto, ESTADO_CONVOCATORIA, ESTADO_POSTULACION, imputabilidad_servicio_disminucion_capacidad, imputabilidad_servicio_muerte, TIPO_IMPUTABILIDAD, TIPO_MODALIDAD, TIPO_SOLICITUD, TIPO_SUBMODALIDAD } from '../constants/fondoSolidaridad.constants';
import NotificationService from '../../../services/notification.service';
import EtapasPostulacion from './EtapasPostulacion';
import { SolicitudDto } from '../../../models/requests/solicitud-tarea.model';
import { PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION } from '../../../constants/constants';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import AdjudicacionVivienda from '../adjudicacionVivienda/AdjudicacionVivienda';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import { ModalDeclararInconsisntencia } from './components/ModalDeclararInconsistencia';
import PerdidaDeBeneficio from '../perdidaBeneficio/PerdidaDeBeneficio';
import TituloIconoV3 from '../../AdministracionCuentas/components/titulo-icono/tituloIcono.componentV3';

const useStyles = makeStyles((theme: Theme)  =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
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
		processButton: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
			margin: '8px 0',
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
		swalStyle: {
			width: '800px',
			fontSize: '15px'
		},
		buttonConfirmacion: {
			background: '#7cd1f9',
            color: '#595959'
		},
		buttonCancelar: {
			backgroundColor: '#efefef',
			color: '#595959'
		 },
	}),
);

const SolicitudPostulacion = (props: Props) => {

const classes = useStyles();
const apiCliente = ApiClient();
const apiCredito = ApiCredito();
const apiFondoSolidaridad  = ApiFondoSolidaridad();
const {catalogState,
       loginState, 
	   actividadSolicitud,
	   estadoSolicitud,
	   datosSolicitudPostulacion,
	   selectedData,
	   setActividadSolicitud,
	   setEstadoSolicitud,
	   setDatosSolicitudPostulacion,
       setAgregarSolicitudPostulacion, 
	   setSelectedData,
	   consultarSolicitudes,
       startGetAllIdentificationTypes, 
       startGetAllForces, 
       startGetAllGrades,
       startGetCategory,
	   startGetAllExecutingUnits,
       startGetPhoneTypes,
	   startGetAllCivilState,
	   startGetAllProfessions,
	   startGetAllDepartments
    } = props;

const [agregarNuevaPostulacion,setAgregarNuevaPostulacion] = useState(false);
const [tipoSolicitud,setTipoSolicitud] = useState<any []>([]);
const [tipoModalidad,setTipoModalidad] = useState<any []>([]);
const [tipoSubModalidad,setSubTipoModalidad] = useState<any []>([]);
const [cliente,setCliente] = useState<any>(null);
const [loadingBiometriaVigia,setLoadingBiometriaVigia] = useState(false);
const [loading,setLoading] = useState(false);
const [openModalRetiroDesvinculacion,setModalRetiroDesvinculacion] = useState(false);
const [openModalDeclararInconsistencia,setModalDeclararInconsistencia] = useState(false);
const [existePostulacion,setExistePostulacion] = useState(false);
const [tipoSolicitudSeleccionado,setTipoSolicitudSeleccionado] = useState(0);
const [mostrarCampoDisfrutePension,setMostrarCampoDisfrutePension] = useState(false);
const [mostrarCampoPorcentajeDisminucion, setMostrarCampoPorcentajeDisminucion] = useState(false);
const [beneficiariosPostulantesPostulacion, setBeneficiariosPostulantesPostulacion] = useState<Postulante[]>([]);
const [mostrarCampoInvalidezoMuerte,setMostrarCampoInvalidezoMuerte] = useState(true);
const [cuentasDiferentesAnuladasCanceladasBeneficiarios,setCuentasDiferentesAnuladasCanceladasBeneficiarios] = useState(false);
const [cuentasCanceladasBeneficiarios,setCuentasCanceladasBeneficiarios] = useState(false);
const [listaPostulantes, setListaPostulantes] = useState<Postulante[]>([]);
const [datosCalificacion, setDatosCalificacion] = useState<any>(null);
const [imputabilidadValue, setImputabilidadValue] = useState<catalogPostulacionDto | null>(null);
const [nullImputabilidadValue, setNullImputabilidadValue] = React.useState(false);
const [numeroRadicado,setNumeroRadicado] = useState('');
const SOLUCION_VIVIENDA = 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA';
const ADMINISTRACION_CESANTIAS = 'CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS';
const TIPO_VICULACION_AFILIADO_ADMIN_CESANTIAS = 2;
const TIPO_VICULACION_AFILIADO_BENEFICIARIO = 3;

const formularioResumenInformacionSolicitud = useForm ({
    defaultValues: {
        nombres: '',
        apellidos: '',
		numeroDocumento: '',
        tipoSolicitud: '',
        modalidad: '',
        submodalidad: '',
		telefono:'',
		celular: '',
		email: '',
		fechaInicio: '',
		fechaFinal: '',
		numeroRadicado: '',
		totalFolios: '',
		modeloSolucionVivienda: '',
		numeroConvocatoria: '',
		numeroResolucionApertura: '',
		fechaResolucionApertura: '',
		numeroResolucionCierre: '',
		observacion: '',
    }
})

const formularioSolicitud = useForm ({
    defaultValues: {
        tipoDocumento: '',
        numeroDocumento: '',
		convocatoria: '',
        tipoSolicitud: '',
        tipoModalidad: '',
        tipoSubModalidad: '',
    }
})

const formularioRetiro = useForm ({
    defaultValues: {
		fechaRetiroFuerza: '',
		fechaInvalidezMuerte: '',
        fechaRetiro: '',
        disfrutePension: '',
    }
})

const formularioDeclaracionInconsistencia = useForm ({
    defaultValues: {
        radicadoRespuesta: '',
        descripcion: '',
    }
})

const formularioPostulacion = useForm({
	defaultValues: {
		idSolicitud: '',
		idConvocatoria: '',
		tipoDocumentoId: '',
		numeroDocumento: '',
		idTipoSolicitud: '',
		idModalidad:'',
		idSubModalidad: '',
		manifestacionAutorizacion: false,
		departamento1: '',
		ciudad1: '',
		departamento2: '',
		ciudad2: '',
		departamento3: '',
		ciudad3: '',
		nombreFirma: '',
		apellidoFirma: '',
		firmaSolicitud: '',
		numeroDocumentoFirma: '',
		numeroCelular: '',
		numeroConvocatoria: '',
		representanteLegal: false,
		disfrutePension: '',
		observacion: '',
		unidadEjecutora: '',
		categoria: '',
		nombreAfiliado: '',
		modalidadSolucionVivienda: '',
		afectacionesCuenta: '',
		valorAfectaciones: '',
		mismoAfiliado: false,
		numeroRadicado: '',
	},
});

const formularioObservacionCargueArchivos = useForm ({
	   defaultValues: {
	      numeroFolios:'',
	      observacion: '',
	      observacionVerificacionNovedades: '',
	      observacionVerificacionEmbargos: '',
	      observacionVerificacionCuentaIndividual: '',
	   }
})

const formularioEntrevista = useForm ({
	defaultValues: {
		efS_ID: '',
		efS_ID_SOLICITUD: '',
		efS_VIVIENDA_PROPIA: '',
		efS_TIPO_VIVIENDA:'',
		efS_HABITANTE: '',
		efS_INFORMATIVO_ADMIN: '',
		efS_CONCEPTO_PSICOSOCIAL: '',
		efS_OBSERVACION: '',
	}
 })

 const formularioCalificacion = useForm({
	   defaultValues: {
	      calificacionId: '',
	      solicitudId: '',
	      tipoImputabilidad:'',
		  imputabilidadServicioDescripcion:'',
	      imputabilidadAlServicio: '',
	      porcentajePostulacion: '',
	      hecho: '',
	      disfrutePension: '',
	      datosResolucion: '',
	      estadoId: '',
	      actividad: '',
	      puntaje: '',
	      observacion:'',
	      hechoDescripcion: '',
	      porcentajeDisminucion: '',
		  solicitudesPrevias: '',
	   },
});
 
const camposSolicitudPostulacionSoloLectura = 
(actividadSolicitud === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD 
&& estadoSolicitud ===  ESTADO_POSTULACION.APROBADO) ||
(actividadSolicitud === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD 
&& estadoSolicitud ===  ESTADO_POSTULACION.ANULADO);

useEffect(() => {
    startGetAllIdentificationTypes(loginState.claims);
    startGetAllForces(loginState.claims);
    startGetAllGrades(loginState.claims);
    startGetCategory(loginState.claims);
	startGetAllExecutingUnits(loginState.claims);
    startGetPhoneTypes(loginState.claims);
	startGetAllCivilState(loginState.claims);
	startGetAllProfessions(loginState.claims);
	startGetAllDepartments();
	formularioSolicitud.reset();
	formularioRetiro.reset();
	const busqueda = {
		numeroConvocatoria: '',
		estado: '',
		fechaDesde: '',
		fechaHasta: '',
		fechaPublicacion: '',
	}
	consultarConvocatorias(busqueda);
	consultarTipoSolicitud();
}, []);

useEffect(() => {
if(selectedData){
	consultarCliente();
	setExistePostulacion(true);
	setActividadSolicitud(selectedData.actividad || '');
	setEstadoSolicitud(selectedData.estadoProceso || '');
}
}, []);

const consultarCliente = async() => {
	setLoading(true);
	await apiCliente
	  .getClientByIdentificationTypeIdentificationNumberRequest(
		  Number(selectedData?.tipoDocumentoId),
		  String(selectedData?.numeroDocumeto)
	  )
	  .then(async response => {
		  if (response?.data) {
		     setLoading(false);
			 setCliente(response.data);
			 validarExisteSolicitudPorId(selectedData?.noRadicado || '');
		  }
	  })
	  .catch((e) => {
		  setLoading(false);
		  toast(`Error en consultar cliente solicitud postulación ${e.message}` , { type: toast.TYPE.ERROR });
	  });
}

const validarSolucionViviendayAfectacionesCuenta = () => {
	let modalidadSolucionVivienda = "ADJUDICACIÓN DE VIVIENDA";
	let afectacionesEnCuenta = 'NO';
	if(cliente){
	  if(cliente?.solicitudes?.length > 0){
		cliente?.solicitudes?.forEach((item: any) => {
			if((item.estadoSolicitud === 'PROCESADA') && 
				// MASVI(Vivienda 8)
			   ((item.tipoSolicitud === 'MASVI') ||
				// Pago Extraordinario
				(item.tipoSolicitud === 'PAGOS EXTRAORDINARIOS' 
				 && item.modalidad === 'PAGO EXTEMPORANEO') ||
				// Pago Bloqueo y/o Embargo
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'ALIVIO FINANCIERO') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'CANON ARRENDAMIENTO LEASING') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'CUOTA CREDITO HIPOTECARIO') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'EMBARGO JUDICIAL') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'FINALIZACION CONTRATO LEASING') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'PAGO COOPERATIVAS') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'PAGO CUOTA INCIAL CREDITO HIPOTECARIO') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'PAGO LEASING - CUOTA INICIAL') ||
				(item.tipoSolicitud === 'PAGO BLOQUEO Y/O EMBARGO'
				 && item.modalidad === 'SOLICITUD INTERNA') ||
				// Hoja de Servicio
				(item.tipoSolicitud === 'HOJA DE SERVICIO'
				 && item.modalidad === 'PAGO HOJA DE SERVICIO PONAL') ||
				// Pago Bloqueos
				(item.tipoSolicitud === 'PAGO BLOQUEOS'
				 && item.modalidad === 'PAGO COOPERATIVAS') ||
				(item.tipoSolicitud === 'PAGO BLOQUEOS'
				 && item.modalidad === 'PAGO PARCIAL DE SUBSIDIO') ||
				(item.tipoSolicitud === 'PAGO BLOQUEOS'
				 && item.modalidad === 'SOLICITUD INTERNA') ||
				// Pago Embargos
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'ALIMENTOS') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'EJECUTIVO ALIMENTOS') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'EJECUTIVO SINGULAR') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'INVESTIGACION DE PATERNIDAD') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'LIQUIDACION DE SOCIEDAD CONYUGAL') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'PAGO EMBARGO SUBSIDIO') ||
				// Embargos
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'ALIMENTOS') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'EJECUTIVO ALIMENTOS') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'EJECUTIVO SINGULAR') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'INVESTIGACION DE PATERNIDAD') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'LIQUIDACION DE SOCIEDAD CONYUGAL') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'OTROS PROCESOS') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'SISTEMA DE INFORMACION UNIDAD EJECUTORA') ||
				(item.tipoSolicitud === 'EMBARGOS'
				 && item.modalidad === 'VARIACION PORCENTAJE EMBARGO') ||
				// Bloqueo y/o Embargo de la cuenta
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'BLOQUEO PAGO COOPERATIVAS') ||
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'BLOQUEO PAGO PARCIAL SUBSIDIO') ||
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'BLOQUEO PARA DEVOLUCION DE RECURSOS UNIDAD EJECUTORA') ||
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'BLOQUEO PREVENTIVO LEASING') ||
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'BLOQUEOS POR ACREEDORES') ||
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'CANON ARRENDAMIENTO LEASING') ||
				(item.tipoSolicitud === 'BLOQUEO Y/O EMBARGO DE CUENTA'
				 && item.modalidad === 'SOLICITUDES Y PETICIONES AFILIADOS') ||
				// Bloqueos
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'BLOQUEO PAGO COOPERATIVAS') ||
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'BLOQUEO PREVENTIVO') ||
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'CREDITO HIPOTECARIO') ||
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'DEVOLUCION DE RECURSOS UNIDADES EJECUTORAS') ||
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'FONDO DE SOLIDARIDAD') ||
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'SOLICITUD INTERNA') ||
				(item.tipoSolicitud === 'BLOQUEOS'
				 && item.modalidad === 'SOLICITUDES Y PETICIONES AFILIADOS'))){
				 modalidadSolucionVivienda = "GIRO DE RECURSOS";
				 afectacionesEnCuenta="SI";
			}
		})
	  }
	}
	return `${modalidadSolucionVivienda}-${afectacionesEnCuenta}`;
}

const validacionesBeneficiairios = async(beneficiarios: any[], beneficiariosNoEncontradosEnSistema: any[], tipoSubModalidad: number) => {

  let beneficiariosNoCumplenCondiciones = false;
  let cantidadDocumentos = 0; 
  let tieneCuentasSolucionViviendaCanceladas = 0;
  let cuentasSolucionViviendaDiferenteCancelada=0;	
  let tieneCuentasAdminCesantiasCancelada = 0;
  let cuentasAdminCesantiasDiferenteCancelada=0;
  let desafiliacion = false;
  const vinculacionAdministracionCesantias: any[] = [];
  const solucionDeViviendaCancelada: any[] = [];
  const solucionDeViviendaDiferenteCanceladaAfiliadoFallecido: any[] = [];
  const cuentaAdministracionCesantias: any[] = [];
  const solicitudesDePagoEnTramite: any[] = [];
  const desafiliacionesRetiroInstitucion: any[] = [];
  const desafiliaciones: any[] = [];
  const masviAfiliadosBloqueados: any [] = [];
  const perdidaDeBeneficio: any [] = [];
  const pagoExtraordinario: any [] = [];

  beneficiarios.forEach(beneficiario => {
	if(beneficiario.cuentas){
		if((beneficiario.cuentas)){
			beneficiario.cuentas?.forEach((item: any) => {
			if(item.tipoCuenta === SOLUCION_VIVIENDA &&
			   item.estadoCuenta !== 'CANCELADA'){
				cuentasSolucionViviendaDiferenteCancelada += 1;
			}else if(item.tipoCuenta === SOLUCION_VIVIENDA &&
					 item.estadoCuenta === 'CANCELADA'){
				tieneCuentasSolucionViviendaCanceladas += 1;
			}

			if(item.tipoCuenta === ADMINISTRACION_CESANTIAS &&
			   item.estadoCuenta !== 'CANCELADA'){
				cuentasAdminCesantiasDiferenteCancelada += 1;
			}
			else if(item.tipoCuenta === ADMINISTRACION_CESANTIAS &&
					item.estadoCuenta === 'CANCELADA'){
				tieneCuentasAdminCesantiasCancelada += 1;
			}
		})
	  }
	}

	if(beneficiario.idTipoActor === TIPO_VICULACION_AFILIADO_ADMIN_CESANTIAS){
		vinculacionAdministracionCesantias.push(beneficiario.identificacion);
	}

	if(tieneCuentasSolucionViviendaCanceladas > 0 
		&& cuentasSolucionViviendaDiferenteCancelada === 0){
		solucionDeViviendaCancelada.push(beneficiario.identificacion);
	}

	// submodalidad
	if(tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA &&
	   cuentasSolucionViviendaDiferenteCancelada >= 1 ){
	   solucionDeViviendaDiferenteCanceladaAfiliadoFallecido.push(beneficiario.identificacion);
	}

	if(cuentasAdminCesantiasDiferenteCancelada > 0
		&& tieneCuentasAdminCesantiasCancelada === 0){
		cuentaAdministracionCesantias.push(beneficiario.identificacion);
	}
	
	if(beneficiario?.solicitudes && beneficiario?.solicitudes.length > 0){
		const tieneSolicitudesPendientes = beneficiario.solicitudes.some((solicitud: any) => solicitud.estadoSolicitud === 'PENDIENTE' &&
		(solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' ||
		 solicitud.tipoSolicitud === 'DESAFILIACIÓN' ||
		 solicitud.tipoSolicitud === 'VERIFICACION DE DERECHOS Y ABONOS DE INTERESES' ||
		 solicitud.tipoSolicitud === 'DESAFILIACIÓN SÓLO APORTES' ||
		 solicitud.tipoSolicitud === 'DESAFILIACIÓN CON CESANTÍAS' ||
		 solicitud.tipoSolicitud === 'DESAFILIACIONES' ||
		 solicitud.tipoSolicitud === 'CRÉDITO HIPOTECARIO' ||
		 solicitud.tipoSolicitud === 'SOLUCIÓN DE VIVIENDA' ||
		 solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' ||
		 solicitud.tipoSolicitud === 'RETIRO DEFINITIVO DE LA INSTITUCIÓN' ||
		 solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' ||
		 solicitud.tipoSolicitud === 'ABONOS DE INTERESES' ||
		 solicitud.tipoSolicitud === 'POSTULACIÓN FONDO DE SOLIDARIDAD' ||
		 solicitud.tipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS'));

		if(tieneSolicitudesPendientes){
			solicitudesDePagoEnTramite.push(beneficiario.identificacion);
		}

		let fechaMaxima:any;
		const solicitudesDesafiliacion: any[] = [];
		beneficiario?.solicitudes.forEach((item: any) => {
				if((item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA CON CESANTÍAS' ||
					item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' ||
					item.tipoSolicitud === 'DESAFILIACION') && (item.estadoSolicitud === 'PROCESADA')){
						solicitudesDesafiliacion.push(item);
				}
		});

		if(solicitudesDesafiliacion){

			if(solicitudesDesafiliacion.length === 1){
				fechaMaxima = solicitudesDesafiliacion[0].fechaInicioSolicitud;
			}else if(solicitudesDesafiliacion.length > 1){
				fechaMaxima = solicitudesDesafiliacion[0].fechaInicioSolicitud;
				solicitudesDesafiliacion.forEach((item: any)=> {
					if(Date.parse(item.fechaInicioSolicitud) > Date.parse(fechaMaxima)){
						fechaMaxima = item.fechaInicioSolicitud;
					}
				})
			}

			const solicitudDesafiliacionReciente: any[] = [];

			solicitudesDesafiliacion.forEach((item:any) => {
				
			if((Date.parse(item.fechaInicioSolicitud) === Date.parse(fechaMaxima)) &&
					(item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA CON CESANTÍAS' ||
					item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' ||
					item.tipoSolicitud === 'DESAFILIACION') && (item.estadoSolicitud === 'PROCESADA')){
						solicitudDesafiliacionReciente.push(item);
					}
			})

			const solicitudReintegroDesafiliacion = beneficiario?.solicitudes.find((item: any) => 
			   item.tipoSolicitud === 'REINTEGROS' &&
			   item.modalidad === 'REINTEGRO POR DESAFILIACIÓN');

			// validaciones desafiliaciones
			if(solicitudDesafiliacionReciente){
				const desafiliacionRetiroInstitucion = solicitudDesafiliacionReciente.find((item: any) => item.tipoSolicitud === 'DESAFILIACION' && item.modalidad === 'RETIRO DE LA INSTITUCIÓN');
				if(desafiliacionRetiroInstitucion && 
				   Date.parse(beneficiario?.fechaPrimeraCuota) < Date.parse(desafiliacionRetiroInstitucion?.fechaInicioSolicitud)){
					desafiliacionesRetiroInstitucion.push(beneficiario?.identificacion);
				}
			}

			let excepcionReintegro = false;
			// Si la fecha de la desafiliacion de Reintegro es menor a todas las fechas de desafiliaciones recientes
			if(solicitudReintegroDesafiliacion && solicitudDesafiliacionReciente){
				solicitudDesafiliacionReciente.forEach(desafiliacionReciente => {
					if(desafiliacionReciente){
						if(Date.parse(solicitudReintegroDesafiliacion.fechaInicioSolicitud) < 
				           Date.parse(desafiliacionReciente?.fechaInicioSolicitud)) {
					       desafiliacion = true;
				        }
					}
				})

				if(desafiliacion === true){
					excepcionReintegro = true;
				}
			}

			if(excepcionReintegro === false && solicitudDesafiliacionReciente){
				solicitudDesafiliacionReciente.forEach(desafiliacionReciente => {
					if(desafiliacionReciente){
						if(Date.parse(beneficiario?.fechaPrimeraCuota) < 
				           Date.parse(desafiliacionReciente?.fechaInicioSolicitud)) {
					       desafiliacion = true;
				        }
					}
				})
			}
		}

		if(desafiliacion){
			desafiliaciones.push(beneficiario?.identificacion);
		}

		beneficiario.solicitudes.forEach((item: any) => {
			if(item.tipoSolicitud === 'MASVI' 
					&& item.estadoSolicitud === 'PROCESADA'
					&& beneficiario?.solicitudes.estadoCliente === 'BLOQUEADO'){
					masviAfiliadosBloqueados.push(beneficiario?.identificacion);
			} else if(item.tipoSolicitud === 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD'
					&& item.estadoSolicitud === 'PROCESADA'){
						perdidaDeBeneficio.push(beneficiario?.identificacion);
			} else if(item.tipoSolicitud === 'PAGOS EXTRAORDINARIOS' 
				   && item.modalidad === 'MUERTE DEL AFILIADO'
				   && item.estadoSolicitud === 'PROCESADA'){
					pagoExtraordinario.push(beneficiario?.identificacion);
		}})
	}

	cantidadDocumentos += 1;

	if(cantidadDocumentos === beneficiarios.length){
		let mensaje = '';

		if(beneficiariosNoEncontradosEnSistema && beneficiariosNoEncontradosEnSistema.length === 1){
			mensaje = `El beneficiario con cédula ${beneficiariosNoEncontradosEnSistema.reverse().toString()} no se encuentra en el sistema. \n`;
		}

		if(vinculacionAdministracionCesantias.length === 1){
			 mensaje = `El beneficiario con cédula ${vinculacionAdministracionCesantias.reverse().toString()} tiene vinculación Administración de Cesantías. \n`;
		}

		if(tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA &&
		   solucionDeViviendaDiferenteCanceladaAfiliadoFallecido.length === 0 && solucionDeViviendaCancelada.length === 1){
		   mensaje = `El beneficiario con cédula ${solucionDeViviendaCancelada.reverse().toString()} tiene cuenta Solución vivienda cancelada. \n`;
		}

		if(tipoSubModalidad !== TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA &&
		   solucionDeViviendaCancelada.length === 1){
			 mensaje = `El beneficiario con cédula ${solucionDeViviendaCancelada.reverse().toString()} tiene cuenta Solución vivienda cancelada. \n`;
		}

		if(cuentaAdministracionCesantias.length === 1){
			 mensaje = `El beneficiario con cédula ${cuentaAdministracionCesantias.reverse().toString()} tiene cuenta Administración de Cesantías. \n`;
		   }

		if(solicitudesDePagoEnTramite.length === 1){
			 mensaje = `El beneficiario con cédula ${solicitudesDePagoEnTramite.reverse().toString()} tiene solicitudes de pago en trámite. \n`;
		}

		if(desafiliacionesRetiroInstitucion.length === 1){
			mensaje = `El beneficiario con cédula ${desafiliacionesRetiroInstitucion.reverse().toString()} registra desafiliación Retiro de la Institución con fecha de solicitud mayor a fecha de primer aporte. \n`;
		  }

		if(desafiliaciones.length === 1){
			 mensaje = `El beneficiario con cédula ${desafiliaciones.reverse().toString()} registra desafiliación. \n`;
		   }

		if(masviAfiliadosBloqueados.length === 1){
			 mensaje = `El beneficiario con cédula ${masviAfiliadosBloqueados.reverse().toString()} registra Acreditación Vivivenda 8 en estado Bloqueado. \n`;
		   }
		   
		if(perdidaDeBeneficio.length === 1){
			 mensaje = `El beneficiario con cédula ${perdidaDeBeneficio.reverse().toString()} registra una pérdida de beneficio, ¿Está seguro qué el postulante a Fondo de Solidaridad no es el mismo que registra la perdida de beneficio?. \n`;
		}

		if(pagoExtraordinario.length === 1){
			 mensaje = `El beneficiario con cédula ${pagoExtraordinario.reverse().toString()} registra un pago extraordinario, ¿Está seguro qué el postulante a fondo de solidaridad no es el mismo beneficiario de pago?. \n`;
		}

		if(beneficiariosNoEncontradosEnSistema && beneficiariosNoEncontradosEnSistema.length > 1){
			 let cedulas = '';
			 beneficiariosNoEncontradosEnSistema.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} no se encuentran en el sistema. \n \n`;
		}

		if(vinculacionAdministracionCesantias.length > 1){
			 let cedulas = '';
			 vinculacionAdministracionCesantias.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} tiene vinculación Administración de Cesantías. \n \n`;
			}

		if(tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA &&
			solucionDeViviendaDiferenteCanceladaAfiliadoFallecido.length === 0 && solucionDeViviendaCancelada.length > 1){
			let cedulas = '';
			solucionDeViviendaCancelada.reverse().forEach((cedula: any) => {
			cedulas += `${cedula} \n`;
			});
			mensaje += `Los beneficiarios con cédulas: \n ${cedulas} tienen cuenta Solución vivienda cancelada. \n \n`;
		}

		 if(tipoSubModalidad !== TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA &&
			 solucionDeViviendaCancelada.length > 1){
			 let cedulas = '';
			 solucionDeViviendaCancelada.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} tienen cuenta Solución vivienda cancelada. \n \n`;
		}

		 if(cuentaAdministracionCesantias.length > 1){
			 let cedulas = '';
			 cuentaAdministracionCesantias.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} tienen cuenta Administración de Cesantías. \n \n`;
			}

		 if(solicitudesDePagoEnTramite.length > 1){
			 let cedulas = '';
			 solicitudesDePagoEnTramite.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} tienen solicitudes de pago en trámite. \n \n`;
			}

		if(desafiliacionesRetiroInstitucion.length > 1){
			let cedulas = '';
			desafiliacionesRetiroInstitucion.reverse().forEach((cedula: any) => {
			cedulas += `${cedula} \n`;
			});
			mensaje += `Los beneficiarios con cédulas: \n ${cedulas} registran desafiliación Retiro de la Institución con fecha de solicitud mayor a fecha de primer aporte. \n \n`;
		   }

		 if(desafiliaciones.length > 1){
			 let cedulas = '';
			 desafiliaciones.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} registran desafiliación. \n \n`;
			}

		 if(masviAfiliadosBloqueados.length > 1){
			 let cedulas = '';
			 masviAfiliadosBloqueados.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} registra Acreditación Vivivenda 8 en estado Bloqueado. \n \n`;
			}

		 if(perdidaDeBeneficio.length > 1){
			 let cedulas = '';
			 perdidaDeBeneficio.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} registran una pérdida de beneficio, ¿Está seguro qué el postulante a Fondo de Solidaridad no es el mismo que registra la perdida de beneficio?. \n`;
			}

		 if(pagoExtraordinario.length > 1){
			 let cedulas = '';
			 pagoExtraordinario.reverse().forEach((cedula: any) => {
			 cedulas += `${cedula} \n`;
			 });
			 mensaje += `Los beneficiarios con cédulas: \n ${cedulas} registran un pago extraordinario, ¿Está seguro qué el postulante a fondo de solidaridad no es el mismo beneficiario de pago?. \n \n`;
			}

		 if(mensaje){
			 beneficiariosNoCumplenCondiciones =  true;
			 toast(<div style={{ whiteSpace: 'pre-line' }}>{mensaje}</div>,  { type: toast.TYPE.ERROR });
		 }
	}
  })
	return beneficiariosNoCumplenCondiciones;
}

const onSubmit = async (data: any) => {
	setLoadingBiometriaVigia(true);

	const responseVigia = await apiCredito.PostVerificacionTerc(
	{
		pe_Origen: PE_ORIGEN,
		pe_Dato: String(data.numeroDocumento),
		pe_TipoVerificacion: PE_TIPO_VERIFICACION,
		pe_Porcentaje: PE_PORCENTAJE
	});

	const isValidVigia = responseVigia.data?.encontrado === 'N';
	if (isValidVigia) {
	  
		setLoadingBiometriaVigia(false);
		setLoading(true);
		let clienteAfiliado: any;
		let tieneCuentasSolucionViviendaCanceladas = 0;
		let cuentasSolucionViviendaDiferenteCancelada = 0;	
		let tieneCuentasAdminCesantiasCancelada = 0;
		let cuentasAdminCesantiasDiferenteCancelada = 0;
		let cuentasCanceladasYAnuladas = false;
		let dependientesCuentasCanceladasYAnuladas = 0;
		let dependientesCuentasCanceladas = 0;
		const beneficiarios: any[] = [];
		const beneficiariosNoEncontrados: any[] = [];
		try {
			const response = await apiCliente.getClientByIdentificationTypeIdentificationNumberRequest(
				data.tipoDocumento,
				data.numeroDocumento,
			);
			if(response.data){
				clienteAfiliado = response?.data;
				if(data.tipoModalidad === TIPO_MODALIDAD.HEROES && data.tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA){

					if(clienteAfiliado && clienteAfiliado?.cuentas && clienteAfiliado?.cuentas.length > 0 && clienteAfiliado?.dependientes && clienteAfiliado?.dependientes.length === 0){
						const cuentasDiferentesAnuladaCancelada =  clienteAfiliado?.cuentas.some((item:any) => item?.tipoCuenta === SOLUCION_VIVIENDA && item?.estadoCuenta !== 'CANCELADA' && item?.estadoCuenta !== 'ANULADA');
						if(cuentasDiferentesAnuladaCancelada){
							cuentasCanceladasYAnuladas = false;
						}else{
							cuentasCanceladasYAnuladas = true;
						}
					}

					if (response.data.dependientes && response.data.dependientes.length > 0) {
							await Promise.all(response.data.dependientes.map(async (dependiente:any) => {
							if (dependiente) {
								const codigoDocumento = dependiente?.identificacion.split('-')[0];
								const tipoDocumento = catalogState?.IdentificationType.find(item => item.codigo === codigoDocumento);
								const numeroDocumento = dependiente?.identificacion.split('-')[1];
								const responseDependiente = await apiCliente.getClientByIdentificationTypeIdentificationNumberRequest(
									tipoDocumento !== undefined? tipoDocumento?.id : 0,
									numeroDocumento,
								);
								if (responseDependiente.data) {
									beneficiarios.push(responseDependiente.data);
									if(responseDependiente?.data?.cuentas && responseDependiente?.data?.cuentas.length > 0){
										const cuentasDependiente = responseDependiente?.data?.cuentas.some((item:any) => item?.tipoCuenta === SOLUCION_VIVIENDA && item.estadoCuenta !== 'CANCELADA' && item.estadoCuenta !== 'ANULADA');
										const cuentasDependienteCancelada = responseDependiente?.data?.cuentas.some((item:any) => item?.tipoCuenta === SOLUCION_VIVIENDA && item.estadoCuenta === 'CANCELADA');
										if(cuentasDependiente){
											dependientesCuentasCanceladasYAnuladas += 1;
										}
										if(cuentasDependienteCancelada){
											dependientesCuentasCanceladas += 1;
										}
									}
								}
								else{
									beneficiariosNoEncontrados.push(numeroDocumento);
								}
							}
						}));
					}

					// Validación afiliado principal y beneficiarios con cuentas canceladas
					if(clienteAfiliado && clienteAfiliado?.cuentas && clienteAfiliado?.cuentas.length > 0 && clienteAfiliado?.dependientes && clienteAfiliado?.dependientes.length > 0){
						const cuentaClientePrincipalCancelada =  clienteAfiliado?.cuentas.some((item:any) => item?.tipoCuenta === SOLUCION_VIVIENDA && item?.estadoCuenta === 'CANCELADA');
						if(cuentaClientePrincipalCancelada && response.data?.dependientes.length === dependientesCuentasCanceladas){
							setCuentasCanceladasBeneficiarios(true);
						}
					}

					// Validación cuentas Canceladas y Anuladas con beneficiarios
					if(clienteAfiliado && clienteAfiliado?.cuentas && clienteAfiliado?.cuentas.length > 0 && clienteAfiliado?.dependientes && clienteAfiliado?.dependientes.length === 0){
					
						const cuentasDiferentesAnuladaCancelada =  clienteAfiliado?.cuentas.some((item:any) => item?.tipoCuenta === SOLUCION_VIVIENDA && item?.estadoCuenta !== 'CANCELADA' && item?.estadoCuenta !== 'ANULADA');
						if(cuentasDiferentesAnuladaCancelada){
							cuentasCanceladasYAnuladas = false;
						}else if(cuentasDiferentesAnuladaCancelada === false && dependientesCuentasCanceladasYAnuladas > 0){
							cuentasCanceladasYAnuladas = false;
							setCuentasDiferentesAnuladasCanceladasBeneficiarios(true);
						}else if(cuentasDiferentesAnuladaCancelada === false && dependientesCuentasCanceladasYAnuladas === 0){
							cuentasCanceladasYAnuladas = true;
						}
					}
				}
		    }
		} catch (error) {
			toast(`Error al consultar información afiliado: ${error}` , { type: toast.TYPE.ERROR });
		}

		if(!clienteAfiliado){
		    setLoading(false);
			toast('Afiliado no existe', { type: toast.TYPE.ERROR });
			return;
		}
		setCliente(clienteAfiliado);

		    if(data.tipoSolicitud === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD){

				if(clienteAfiliado.cuentas){
					clienteAfiliado.cuentas?.forEach((item: any) => {
						if(item.tipoCuenta === SOLUCION_VIVIENDA &&
						   item.estadoCuenta !== 'CANCELADA'){
							cuentasSolucionViviendaDiferenteCancelada += 1;
						}else if(item.tipoCuenta === SOLUCION_VIVIENDA &&
								 item.estadoCuenta === 'CANCELADA'){
							tieneCuentasSolucionViviendaCanceladas += 1;
						}
		
						if(item.tipoCuenta === ADMINISTRACION_CESANTIAS &&
						   item.estadoCuenta !== 'CANCELADA'){
							cuentasAdminCesantiasDiferenteCancelada += 1;
						}
						else if(item.tipoCuenta === ADMINISTRACION_CESANTIAS &&
								item.estadoCuenta === 'CANCELADA'){
							tieneCuentasAdminCesantiasCancelada += 1;
						}
				})

			 }

			 	if(cuentasCanceladasYAnuladas){
					setLoading(false);
					toast('No puede continuar. El afiliado solo tiene cuentas en estado Cancelada o Anulada', { type: toast.TYPE.ERROR });
					return;
				}
		
				if(clienteAfiliado.idTipoActor === TIPO_VICULACION_AFILIADO_BENEFICIARIO){
					setLoading(false);
					toast('Sr(a), Beneficiario debe realizar la postulación con la cuenta del afiliado fallecido', { type: toast.TYPE.ERROR });
					return;
				}

				if(data.tipoModalidad === TIPO_MODALIDAD.HEROES && data.tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA 
					&& tieneCuentasSolucionViviendaCanceladas > 0 
					&& cuentasSolucionViviendaDiferenteCancelada === 0 
					&& beneficiarios.length === 0 
					&& beneficiariosNoEncontrados.length === 0){
					 setLoading(false);
					 toast('No puede continuar. El afiliado tiene cuenta Solución vivienda cancelada', { type: toast.TYPE.ERROR });
					 return;
				 }

				if(data.tipoModalidad !== TIPO_MODALIDAD.HEROES && data.tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA
				   && tieneCuentasSolucionViviendaCanceladas > 0 
				   && cuentasSolucionViviendaDiferenteCancelada === 0){
				    setLoading(false);
					toast('No puede continuar. El afiliado tiene cuenta Solución vivienda cancelada', { type: toast.TYPE.ERROR });
					return;
				}

				if(	cuentasAdminCesantiasDiferenteCancelada > 0
					&& tieneCuentasAdminCesantiasCancelada === 0){
					 setLoading(false);
					 toast('No puede continuar. El afiliado tiene cuenta Administración de Cesantías', { type: toast.TYPE.ERROR });
					 return;
				}

			if(clienteAfiliado?.solicitudes){
				const tieneSolicitudesPendientes = clienteAfiliado?.solicitudes.some((solicitud:any) => solicitud.estadoSolicitud === 'PENDIENTE' &&
				(solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' ||
				 solicitud.tipoSolicitud === 'DESAFILIACIÓN' ||
				 solicitud.tipoSolicitud === 'VERIFICACION DE DERECHOS Y ABONOS DE INTERESES' ||
				 solicitud.tipoSolicitud === 'DESAFILIACIÓN SÓLO APORTES' ||
				 solicitud.tipoSolicitud === 'DESAFILIACIÓN CON CESANTÍAS' ||
				 solicitud.tipoSolicitud === 'DESAFILIACIONES' ||
				 solicitud.tipoSolicitud === 'CRÉDITO HIPOTECARIO' ||
				 solicitud.tipoSolicitud === 'SOLUCIÓN DE VIVIENDA' ||
				 solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' ||
				 solicitud.tipoSolicitud === 'RETIRO DEFINITIVO DE LA INSTITUCIÓN' ||
				 solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' ||
				 solicitud.tipoSolicitud === 'ABONOS DE INTERESES' ||
				 solicitud.tipoSolicitud === 'POSTULACIÓN FONDO DE SOLIDARIDAD' ||
				 solicitud.tipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS'));

				if(tieneSolicitudesPendientes){
					setLoading(false);
					toast('El afiliado tiene solicitudes de pago en trámite', { type: toast.TYPE.ERROR });
					return;
				}
			}

				// validacionesBeneficiairios(beneficiarios,beneficiariosNoEncontrados,data.tipoSubModalidad);
				if(data.tipoModalidad === TIPO_MODALIDAD.HEROES && data.tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA &&
				   beneficiarios && beneficiarios.length > 0){
				   const beneficiariosNoCumplenCondiciones = await validacionesBeneficiairios(beneficiarios,beneficiariosNoEncontrados,data.tipoSubModalidad);
				   if(beneficiariosNoCumplenCondiciones === true){
					  setLoading(false);
					  return;
				   }
				}
				
				// Validar si existe solicitud de postulacion
				if(data.tipoModalidad === TIPO_MODALIDAD.HEROES && data.tipoSubModalidad !== TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA ||
				   data.tipoModalidad === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
				
					await apiFondoSolidaridad
					.postValidarExistePostulacion(Number(data.numeroDocumento))
					.then(async (responseExisteSolicitud: any) => {
						if (responseExisteSolicitud) {
							if (responseExisteSolicitud.isSuccess && responseExisteSolicitud.data
								&& responseExisteSolicitud.data.departamento1 !== 0
								&& responseExisteSolicitud.data.departameno2 !== 0
								&& responseExisteSolicitud.data.deparmento3 !== 0) {

									// if(idUsuario !== responseExisteSolicitud?.data?.userId){
									// 	setLoading(false);
									// 	await NotificationService.genericAlert(
									// 		'',
									// 		'El usuario no se encuentra asignado a la postulación',
									// 		'warning',
									// 	);
									// 	return;
									// }
								    setDatosSolicitudPostulacion(responseExisteSolicitud.data);
									setExistePostulacion(true);
									setActividadSolicitud(responseExisteSolicitud.data?.actividad);
									setEstadoSolicitud(responseExisteSolicitud.data?.estadoProceso);

									const confirmAlert = await NotificationService.confirmAlert(
										`Ya existe una solicitud de ${clienteAfiliado.primerNombre} ${clienteAfiliado.segundoNombre} ${clienteAfiliado.primerApellido} ${clienteAfiliado.segundoApellido}
												en trámite para ${responseExisteSolicitud.data?.tipoModalidadNombre}, \n ¿Está seguro que desea continuar?`,
									);
									if (confirmAlert) {
									    setLoading(false);
										setAgregarNuevaPostulacion(true);
									}else{
									    setLoading(false);
										setAgregarNuevaPostulacion(false);
										setActividadSolicitud('');
									}
							}
							// Si no existe
							else{
								setExistePostulacion(false);
								setLoading(false);
								let fuerzaAfiliado;
								if(clienteAfiliado?.idFuerza){
								   fuerzaAfiliado = catalogState.Force.find(item => item.id === clienteAfiliado?.idFuerza);
								}
								const unidadEjecutora = catalogState.ExecutingUnit.find(item => item.id === clienteAfiliado?.idUnidadEjecutora);
								if(data.tipoModalidad === TIPO_MODALIDAD.HEROES &&
								  (data.tipoSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
								   data.tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA)){
										handleOpenModalRetiro();
								}
								else if(data.tipoModalidad === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
									const grado = catalogState.Grade.find(item => item.id === clienteAfiliado?.idGrado);
									if(unidadEjecutora?.nombre === 'PENSIONADOS FFMM' 
									   && (grado?.descripcion === 'SOLDADO PROFESIONAL' 
									   || grado?.descripcion === 'SOLDADO VOLUNTARIO'
									   || grado?.descripcion === 'SOLDADO PENSIONADO')
									   && clienteAfiliado?.numeroCuotas > 24){
										handleOpenModalRetiro();
									}else{
										toast('Afiliado no aplica para proyectos y mecanismos especiales', { type: toast.TYPE.ERROR });
									}
								}
							}
						}
					})
					.catch((e) => {
						setLoading(false);
						toast(`Error validar si existe al crear postulación ${e.message}`, { type: toast.TYPE.ERROR });
					});
				}else{
					setExistePostulacion(false);
					setLoading(false);
					handleOpenModalRetiro();
				}
		    }

			if(data.tipoSolicitud === TIPO_SOLICITUD.AJUDICACION_VIVIENDA ||
			   data.tipoSolicitud === TIPO_SOLICITUD.PERDIDA_BENEFICIO){
				await apiFondoSolidaridad
					.postValidarExistePostulacion(Number(data.numeroDocumento))
					.then(async (responseExisteSolicitud: any) => {
						if (responseExisteSolicitud) {
							if (responseExisteSolicitud.isSuccess && responseExisteSolicitud.data
								&& responseExisteSolicitud.data.departamento1 !== 0
								&& responseExisteSolicitud.data.departameno2 !== 0
								&& responseExisteSolicitud.data.deparmento3 !== 0) {
								setDatosSolicitudPostulacion(responseExisteSolicitud.data);
								setExistePostulacion(true);
								setTipoSolicitudSeleccionado(data.tipoSolicitud);
								setLoading(false);

							}else{
								await NotificationService.genericAlert(
									'',
									'El usuario no se encuentra asignado a la postulación',
									'warning',
								);
								setTipoSolicitudSeleccionado(data.tipoSolicitud);
								setLoading(false);
							}
			    }}).catch((e) => {
					setLoading(false);
					toast(`Error validar si existe postulación en adjudicación de vivienda ${e.message}`, { type: toast.TYPE.ERROR });
				});
			}

	    } else {
			setLoading(false);
			setLoadingBiometriaVigia(false);
			await NotificationService.genericAlert(
				'Advertencia!',
				`Favor acercarse al punto de atención!.`,
			);
		}
} 

const validacionRetiro = async(data: any) => {

	let desafiliaciones = false;
	if(cuentasCanceladasBeneficiarios){
		swal({
			title:'Información',
			text: 'El afiliado principal y beneficiario(s) presentan cuentas Solución de Vivienda en estado CANCELADA. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
			icon: 'warning',
			buttons: {
				rechazar: {
				text: 'ACEPTAR',
				value: 'ACEPTAR',
				className: classes.buttonConfirmacion
				},
				anular: {
					text: 'CANCELAR',
					value: 'CANCELAR',
					className: classes.buttonCancelar
				},
			},
			closeOnClickOutside: false,
		})
		.then(respuesta => {
			if (respuesta === 'ACEPTAR') {
				desafiliaciones = true;
				handleCloseModalRetiro();
				setLoading(true);
				crearPostulacion(data.disfrutePension, data, 'EL AFILIADO PRINCIPAL Y BENEFICIARIO(S) PRESENTAN CUENTAS CANCELADAS');
		}})
	}
	else if(cuentasDiferentesAnuladasCanceladasBeneficiarios){
		swal({
			title:'Información',
			text: 'El afiliado presenta cuentas Solución de Vivienda en estado CANCELADA o ANULADA. Beneficiario(s) presentan cuentas diferentes a estado Cancelada y Anulada. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
			icon: 'warning',
			buttons: {
				rechazar: {
				text: 'ACEPTAR',
				value: 'ACEPTAR',
				className: classes.buttonConfirmacion
				},
				anular: {
					text: 'CANCELAR',
					value: 'CANCELAR',
					className: classes.buttonCancelar
				},
			},
			closeOnClickOutside: false,
		})
		.then(respuesta => {
			if (respuesta === 'ACEPTAR') {
				desafiliaciones = true;
				handleCloseModalRetiro();
				setLoading(true);
				crearPostulacion(data.disfrutePension, data, 'AFILIADO PRESENTA CUENTAS DE SOLUCIÓN DE VIVIENDA EN ESTADO CANCELADA O ANULADA. BENEFICIARIO (S) PRESENTAN CUENTAS DIFERENTES A CANCELADA Y ANULADA');
		}})
	}
	else if(cuentasDiferentesAnuladasCanceladasBeneficiarios === false &&  cliente && cliente?.solicitudes && cliente?.solicitudes.length > 0) {
		if(cliente && cliente?.solicitudes && cliente?.solicitudes.length > 0){

			let desafiliacion = false;
			let desafiliacionAportes = false;
			let desafiliacionCesantias = false;
			const solicitudDesafiliacion = cliente?.solicitudes?.some((item : any) => item.tipoSolicitud === 'DESAFILIACION' && item.estadoSolicitud === 'PROCESADA');
			const solicitudDesafiliacionAportes = cliente?.solicitudes?.some((item : any) => item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C' && item.estadoSolicitud === 'PROCESADA');
			const solicitudDesafiliacionCesantias = cliente?.solicitudes?.some((item : any) => item.tipoSolicitud === 'DESAFILIACION CI SOLUCION VIVIENDA CON CESANTÍAS' && item.estadoSolicitud === 'PROCESADA');
			const reintegros = cliente?.solicitudes?.some((item : any) => item.tipoSolicitud === 'REINTEGROS' && item.modalidad === 'REINTEGRO POR DESAFILIACIÓN');
	
			if(solicitudDesafiliacion){
				if(new Date(cliente?.fechaPrimeraCuota) > new Date(reintegros?.fechaInicioSolicitud)){
					desafiliacion = false;
				}else if(reintegros && new Date(solicitudDesafiliacion?.fechaInicioSolicitud) < new Date(reintegros?.fechaInicioSolicitud)){
					desafiliacion = false;
				}else if(new Date(cliente?.fechaPrimeraCuota) < new Date(data.fechaRetiro)){
					desafiliacion = false;
				}else if(cliente?.solicitudes?.some((item : any) => (item.tipoSolicitud === 'CREACION DE CUENTA' || item.tipoSolicitud === 'AFILIACION') && item.estadoSolicitud === 'PROCESADA')){
					desafiliacion = false;
				}else{
					desafiliacion = true;
				}
			}
	
			if(solicitudDesafiliacionAportes){
				if(new Date(cliente?.fechaPrimeraCuota) > new Date(reintegros?.fechaInicioSolicitud)){
					desafiliacionAportes = false;
				}else if(reintegros && new Date(solicitudDesafiliacionAportes?.fechaInicioSolicitud) < new Date(reintegros?.fechaInicioSolicitud)){
					desafiliacionAportes = false;
				}else if(new Date(cliente?.fechaPrimeraCuota) < new Date(data.fechaRetiro)){
					desafiliacionAportes = false;
				}else if(cliente?.solicitudes?.some((item : any) => (item.tipoSolicitud === 'CREACION DE CUENTA' || item.tipoSolicitud === 'AFILIACION') && item.estadoSolicitud === 'PROCESADA')){
					desafiliacionAportes = false;
				}else{
					desafiliacionAportes = true;
				}
			}
	
			if(solicitudDesafiliacionCesantias){
				if(new Date(cliente?.fechaPrimeraCuota) > new Date(reintegros?.fechaInicioSolicitud)){
					desafiliacionCesantias = false;
				}else if(reintegros && new Date(solicitudDesafiliacionAportes?.fechaInicioSolicitud) < new Date(reintegros?.fechaInicioSolicitud)){
					desafiliacionCesantias = false;
				}else if(new Date(cliente?.fechaPrimeraCuota) < new Date(data.fechaRetiro)){
					desafiliacionCesantias = false;
				}else if(cliente?.solicitudes?.some((item : any) => (item.tipoSolicitud === 'CREACION DE CUENTA' || item.tipoSolicitud === 'AFILIACION') && item.estadoSolicitud === 'PROCESADA')){
					desafiliacionCesantias = false;
				}else{
					desafiliacionCesantias = true;
				}
			}
	
			if(desafiliacion){
				swal({
					title:'Información',
					text: 'El afiliado presenta desafiliación. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
					icon: 'warning',
					buttons: {
						rechazar: {
						text: 'ACEPTAR',
						value: 'ACEPTAR',
						className: classes.buttonConfirmacion
						},
						anular: {
							text: 'CANCELAR',
							value: 'CANCELAR',
							className: classes.buttonCancelar
						},
					},
					closeOnClickOutside: false,
				})
				.then(respuesta => {
					if (respuesta === 'ACEPTAR') {
						desafiliaciones = true;
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, 'AFILIADO PRESENTA DESAFILIACIÓN EN ESTADO PROCESADA');
				}})
			}
			
			else if(desafiliacionAportes){
				swal({
					title:'Información',
					text: 'El afiliado presenta desafiliación solución vivienda solo aportes. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
					icon: 'warning',
					buttons: {
						rechazar: {
						text: 'ACEPTAR',
						value: 'ACEPTAR',
						className: classes.buttonConfirmacion
						},
						anular: {
							text: 'CANCELAR',
							value: 'CANCELAR',
							className: classes.buttonCancelar
						},
					},
					closeOnClickOutside: false,
				})
				.then(respuesta => {
					if (respuesta === 'ACEPTAR') {
						desafiliaciones = true;
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, 'AFILIADO PRESENTA DESAFILIACION SOLUCION VIVIENDA SOLO APORTES EN ESTADO PROCESADA');
				}})
			}
	
			else if(desafiliacionCesantias){
				swal({
					title:'Información',
					text: 'El afiliado presenta desafiliación solución vivienda con cesantías. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
					icon: 'warning',
					buttons: {
						rechazar: {
						text: 'ACEPTAR',
						value: 'ACEPTAR',
						className: classes.buttonConfirmacion
						},
						anular: {
							text: 'CANCELAR',
							value: 'CANCELAR',
							className: classes.buttonCancelar
						},
					},
					closeOnClickOutside: false,
				})
				.then(respuesta => {
					if (respuesta === 'ACEPTAR') {
						desafiliaciones = true;
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, 'AFILIADO PRESENTA DESAFILIACIÓN SOLUCIÓN VIVIENDA CON CESANTÍAS EN ESTADO PROCESADA');
				}})
			}
			else if( Number(formularioSolicitud.watch('tipoSolicitud')) === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD
		&&  Number(formularioSolicitud.watch('tipoModalidad')) === TIPO_MODALIDAD.HEROES
		&& (Number(formularioSolicitud.watch('tipoSubModalidad')) === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
		   (Number(formularioSolicitud.watch('tipoSubModalidad')) === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA))){
				if(data.disfrutePension){
					if(data.disfrutePension === 'SI'){
	
						if(new Date(data.fechaRetiroFuerza) >= new Date('Jul 21 2005') &&
						   new Date(data.fechaRetiroFuerza) <= new Date('Jun 02 2009') &&
						   new Date(data.fechaInvalidezMuerte) < new Date('Jun 03 2009')){
								swal({
								title:'Información',
								text: 'El afiliado cuenta con pensión y fecha de la asignación de invalidez o muerte menor a 3 de Junio de 2009. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
								icon: 'warning',
								buttons: {
									rechazar: {
									text: 'ACEPTAR',
									value: 'ACEPTAR',
									className: classes.buttonConfirmacion
									},
									anular: {
										text: 'CANCELAR',
										value: 'CANCELAR',
										className: classes.buttonCancelar
									},
								},
								closeOnClickOutside: false,
							})
							.then(respuesta => {
								if (respuesta === 'ACEPTAR') {
									handleCloseModalRetiro();
									setLoading(true);
									crearPostulacion(data.disfrutePension, data, 'AFILIADO CUENTA CON PENSIÓN Y LA FECHA DE ASIGNACIÓN DE INVALIDEZ O MUERTE ES MENOR AL 3 DE JUNIO DE 2009');
							}})
						}else{
							handleCloseModalRetiro();
							setLoading(true);
							crearPostulacion(data.disfrutePension, data, '');
						}
	
					} else if (data.disfrutePension === 'NO'){
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, '');
					}
				}else{
					handleCloseModalRetiro();
					setLoading(true);
					formularioRetiro.setValue('disfrutePension','NO');
					crearPostulacion('NO', data, '');
				}
			}
	
		   else if( Number(formularioSolicitud.watch('tipoSolicitud')) === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD
			 && Number(formularioSolicitud.watch('tipoModalidad')) === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
				if(data.disfrutePension){
					if(data.disfrutePension === 'SI'){
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, '');
					}else if (data.disfrutePension === 'NO'){
	
						swal({
							title:'Información',
							text: 'El afiliado No cuenta con pensión. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
							icon: 'warning',
							 buttons: {
								 rechazar: {
								   text: 'ACEPTAR',
								   value: 'ACEPTAR',
								   className: classes.buttonConfirmacion
								 },
								 anular: {
									 text: 'CANCELAR',
									 value: 'CANCELAR',
									 className: classes.buttonCancelar
								 },
							 },
							 closeOnClickOutside: false,
						 })
						 .then(respuesta => {
							 if (respuesta === 'ACEPTAR') {
								handleCloseModalRetiro();
								setLoading(true);
								crearPostulacion(data.disfrutePension, data, 'AFILIADO NO CUENTA CON PENSIÓN');
						}})
					}
				}
			}
		}
	}

	// Validación general

	else if( Number(formularioSolicitud.watch('tipoSolicitud')) === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD
		&&  Number(formularioSolicitud.watch('tipoModalidad')) === TIPO_MODALIDAD.HEROES
		&& (Number(formularioSolicitud.watch('tipoSubModalidad')) === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
		   (Number(formularioSolicitud.watch('tipoSubModalidad')) === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA))){
				if(data.disfrutePension){
					if(data.disfrutePension === 'SI'){
	
						if(new Date(data.fechaRetiroFuerza) >= new Date('Jul 21 2005') &&
						   new Date(data.fechaRetiroFuerza) <= new Date('Jun 02 2009') &&
						   new Date(data.fechaInvalidezMuerte) < new Date('Jun 03 2009')){
								swal({
								title:'Información',
								text: 'El afiliado cuenta con pensión y fecha de la asignación de invalidez o muerte menor a 3 de Junio de 2009. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
								icon: 'warning',
								buttons: {
									rechazar: {
									text: 'ACEPTAR',
									value: 'ACEPTAR',
									className: classes.buttonConfirmacion
									},
									anular: {
										text: 'CANCELAR',
										value: 'CANCELAR',
										className: classes.buttonCancelar
									},
								},
								closeOnClickOutside: false,
							})
							.then(respuesta => {
								if (respuesta === 'ACEPTAR') {
									handleCloseModalRetiro();
									setLoading(true);
									crearPostulacion(data.disfrutePension, data, 'AFILIADO CUENTA CON PENSIÓN Y LA FECHA DE ASIGNACIÓN DE INVALIDEZ O MUERTE ES MENOR AL 3 DE JUNIO DE 2009');
							}})
						}else{
							handleCloseModalRetiro();
							setLoading(true);
							crearPostulacion(data.disfrutePension, data, '');
						}
	
					} else if (data.disfrutePension === 'NO'){
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, '');
					}
				}else{
					handleCloseModalRetiro();
					setLoading(true);
					formularioRetiro.setValue('disfrutePension','NO');
					crearPostulacion('NO', data, '');
				}
			}
	
		   else if( Number(formularioSolicitud.watch('tipoSolicitud')) === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD
			 && Number(formularioSolicitud.watch('tipoModalidad')) === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
				if(data.disfrutePension){
					if(data.disfrutePension === 'SI'){
						handleCloseModalRetiro();
						setLoading(true);
						crearPostulacion(data.disfrutePension, data, '');
					}else if (data.disfrutePension === 'NO'){
	
						swal({
							title:'Información',
							text: 'El afiliado No cuenta con pensión. La postulación se creará en estado INCONSISTENTE. ¿Desea Continuar?',
							icon: 'warning',
							 buttons: {
								 rechazar: {
								   text: 'ACEPTAR',
								   value: 'ACEPTAR',
								   className: classes.buttonConfirmacion
								 },
								 anular: {
									 text: 'CANCELAR',
									 value: 'CANCELAR',
									 className: classes.buttonCancelar
								 },
							 },
							 closeOnClickOutside: false,
						 })
						 .then(respuesta => {
							 if (respuesta === 'ACEPTAR') {
								handleCloseModalRetiro();
								setLoading(true);
								crearPostulacion(data.disfrutePension, data, 'AFILIADO NO CUENTA CON PENSIÓN');
						}})
					}
				}
			}
	
}

const crearPostulacion = async(disfrutePensionParam: string,data: any, mensajeInconsistencia: string) => {

	await apiCliente
	.getClientByIdentificationTypeIdentificationNumberRequest(
		Number(formularioSolicitud.watch('tipoDocumento')),
		String(formularioSolicitud.watch('numeroDocumento'))
	).then(async responseAfiliado => {
		if(responseAfiliado.data){
			setExistePostulacion(false);
			const solucionViviendayAfectaciones = String(validarSolucionViviendayAfectacionesCuenta());
			let idConvocatoria = 0;
			await apiFondoSolidaridad
			.getObtenerIdConvocatoria()
			.then(async response => {
				if(response.data){
					idConvocatoria = response.data.idConvocatoria;
				}
			})
			.catch((e) => {
				toast(`Error en consultar id convocatoria en postulación ${e.message}`, { type: toast.TYPE.ERROR });
			});
			const unidadEjecutora = catalogState.ExecutingUnit.find(item => item.id === responseAfiliado?.data?.idUnidadEjecutora);
			const categoria = catalogState.categories.find(item => item.id === responseAfiliado?.data?.idCategoria);
			let fuerzaAfiliado;
			if(responseAfiliado?.data.idFuerza){
			   fuerzaAfiliado = catalogState.Force.find(item => item.id === responseAfiliado?.data.idFuerza);
			}
			const informacionPostulacion = {
				idConvocatoria: idConvocatoria,
				tipoDocumentoId: Number(formularioSolicitud.watch('tipoDocumento')),
				numeroDocumeto: Number(formularioSolicitud.watch('numeroDocumento')),
				idTipoSolicitud: Number(formularioSolicitud.watch('tipoSolicitud')),
				idModalidad: Number(formularioSolicitud.watch('tipoModalidad')),
				idSubModalidad: Number(formularioSolicitud.watch('tipoSubModalidad')),
				numeroConvocatoria: Number(formularioSolicitud.watch('convocatoria')),
				spoDistruePension: disfrutePensionParam, 
				nombreCompletoAfiliado: `${responseAfiliado?.data.primerNombre} ${responseAfiliado?.data.segundoNombre} ${responseAfiliado?.data.primerApellido} ${responseAfiliado?.data.segundoApellido}`,
				fuerza: fuerzaAfiliado?.descripcion || '',
				spoUnidadEjecutora: unidadEjecutora?.nombre || '',
				spoCategoria: categoria?.descripcion || '',
				modalidadSolucionVivienda: solucionViviendayAfectaciones.split('-')[0],
				afectacionCuenta: solucionViviendayAfectaciones.split('-')[1],
				valorAfectacion: 0,
				fechaRetiro: data.fechaRetiro,
				fechaRetiroFuerza: data.fechaRetiroFuerza,
				fechaAsignacionInvalidez: data.fechaInvalidezMuerte,
				mismoAfiliado: false,
			}
			setDatosSolicitudPostulacion(informacionPostulacion);
				await apiFondoSolidaridad
					.postCrearSolicitudPostulante(informacionPostulacion)
					.then(async response => {
						if(response.data){
							formularioPostulacion.setValue('idSolicitud',response?.data?.idSolicitud);
							setCliente(responseAfiliado.data);
							setNumeroRadicado(response?.data?.noRadicado);
							if(mensajeInconsistencia && mensajeInconsistencia !== ''){
								const request = {
									idSolicitud: response?.data?.idSolicitud,
									description: mensajeInconsistencia
								}
								await apiFondoSolidaridad
									.postGuardarInconsistenciaSolicitud(request)
									.then(async response => {
										if(response.data){
											await NotificationService.genericAlert(
												'Guardado',
												'Se ha guardado correctamente la información',
											);
											setLoading(false);
											setAgregarNuevaPostulacion(true);
										}
									})
									.catch((e) => {
										toast(`Error al guardar inconsistencia solicitud ${e.message}`, { type: toast.TYPE.ERROR });
									});
							}else{
								await NotificationService.genericAlert(
									'Guardado',
									'Se ha guardado correctamente la información',
								);
								setLoading(false);
								setAgregarNuevaPostulacion(true);
							}
						}else if(response.data === null && response.isSuccess === false && String(response.returnMessage).includes('No se creo la solictud exitosamente')){
							setLoading(false);
							NotificationService.genericAlert(
								'Información',
								`No se puede continuar con la solicitud. El Radicado no se creó correctamente. Por favor actualizar la información de contacto del Afiliado Principal`,
								'warning'
							);
						}else if(response.data === null && response.isSuccess === false){
							setLoading(false);
							NotificationService.genericAlert(
								'Información',
								`${response.returnMessage}`,
								'warning'
							);
						}
					})
					.catch((e) => {
						setLoading(false);
						toast(`Error al crear postulación ${e.message}`, { type: toast.TYPE.ERROR });
					});
		}
	})
	.catch(() => {
		setLoading(false);
		toast('Error del sistema al consultar informacion afiliado', { type: toast.TYPE.ERROR });
	});
}

const onClickCancelar = async() => {
	setAgregarSolicitudPostulacion(false);
	setSelectedData(null);
};

const consultarConvocatorias = async (busqueda: any) => {
	await apiFondoSolidaridad
		.getObtenerConvocatorias(busqueda)
		.then(async response => {
			if(response?.data.length > 0){
				if(response.data[response?.data.length - 1].nombreEstado === ESTADO_CONVOCATORIA.APERTURA
				  || response.data[response?.data.length - 1].nombreEstado === ESTADO_CONVOCATORIA.VERIFICACION){
					formularioSolicitud.setValue('convocatoria',response.data[response?.data.length - 1].numeroConvocatoria);
				}else{
					await NotificationService.genericAlert(
						'Información',
						`Para crear una postulación debe existir una convocatoria en estado de Apertura o Verificación`,
						'warning'
					);
				}
			}
		})
		.catch((e) => {
			toast(`Error al consultar convocatorias ${e.message}`, { type: toast.TYPE.ERROR });
		});
};

const consultarTipoSolicitud = async () => {
	await apiFondoSolidaridad
		.getConsultarTipoSolicitudPostulante()
		.then(async response => {
			if(response.data){
				setTipoSolicitud(response.data);
			}
		})
		.catch((e) => {
			toast(`Error al consultar tipo solicitud ${e.message}`, { type: toast.TYPE.ERROR });
		});
};

const consultarTipoModalidad = async (idTipoSolicitud: number) => {
	await apiFondoSolidaridad
	.getConsultarTipoModalidadPostulante(idTipoSolicitud)
	.then(async response => {
		if(response.data){
			setTipoModalidad(response.data);
		}
	})
	.catch((e) => {
		toast(`Error al consultar tipo modalidad ${e.message}`, { type: toast.TYPE.ERROR });
	});
};

const consultarTipoSubModalidad = async (idTipoModalidad: number) => {
	await apiFondoSolidaridad
		.getConsultarTipoSubModalidadPostulante(idTipoModalidad)
		.then(async response => {
			if(response.data){
				if(idTipoModalidad === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
				   formularioSolicitud.setValue('tipoSubModalidad',response.data[0].id);
				}else{
					formularioSolicitud.setValue('tipoSubModalidad','');
				}
				setSubTipoModalidad(response?.data);
			}
		})
		.catch((e) => {
			toast(`Error al consultar tipo submodalidad ${e.message}`, { type: toast.TYPE.ERROR });
		});
};

const consultarBenficiariosPostulacion = async (idSolicitud: string) => {
	setLoading(true);
	await apiFondoSolidaridad
		.getConsultarEntrevista(idSolicitud)
		.then(async response => {
			if(response.data !== null){
				setBeneficiariosPostulantesPostulacion(response?.data?.postulantes as Postulante[]);
				setListaPostulantes(response.data.postulantes);
				setLoading(false);
			}else{
				await apiFondoSolidaridad
				.getConsultarBeneficairiosPostulantes(idSolicitud)
				.then(async response => {
					if(response.data){
						setBeneficiariosPostulantesPostulacion(response?.data as Postulante[]);
					}else{
						setLoading(false);
					}
				})
				.catch((e) => {
					setLoading(false);
					toast(`Error del sistema Consultar entrevista ${e.message}` , { type: toast.TYPE.ERROR });
				});
            }
		})
		.catch((e) => {
			setLoading(false);
			toast(`Error del sistema Consultar entrevista en Resumen ${e.message}` , { type: toast.TYPE.ERROR });
		});

 };

const actualizarPostulacion = async (datosPostulacion: any) => {
	setLoading(true);
	await apiFondoSolidaridad
		.postActualizarSolicitudPostulante(datosPostulacion)
		.then(async response => {
			if(response.data){
				setLoading(false);
				await NotificationService.genericAlert(
					'Actualizado con éxito',
					'Se realizó con éxito la actualización.',
				);
			}
		})
		.catch((e) => {
		    setLoading(false);
			toast(`Error del sistema actualizar postulacion ${e.message}`, { type: toast.TYPE.ERROR });
		});
};

const actualizarPostulacionAfiliadoFallecido = async (datosPostulacion: any) => {

	if(beneficiariosPostulantesPostulacion && beneficiariosPostulantesPostulacion.length === 0){
		toast('Debe registrar al menos un Beneficiario Postulante',{type: toast.TYPE.ERROR});
		return;
	}

	setLoading(true);
	const arrayBeneficiarios = beneficiariosPostulantesPostulacion.map(({ tieneCuenta, ...beneficiario }) => beneficiario);
	const request = {
		efS_ID: null,
		efS_ID_SOLICITUD: datosSolicitudPostulacion?.idSolicitud? datosSolicitudPostulacion?.idSolicitud:formularioPostulacion.getValues('idSolicitud'),
		efS_VIVIENDA_PROPIA: null,
		efS_TIPO_VIVIENDA: null,
		efS_HABITANTE: null,
		efS_INFORMATIVO_ADMIN: null,
		efS_CARACTERIZACION: null,
		efS_CONCEPTO_PSICOSOCIAL: null,
		efS_OBSERVACION: null,
		postulantes: arrayBeneficiarios,
	}
	await apiFondoSolidaridad
		.postActualizarSolicitudPostulante(datosPostulacion)
		.then(async response => {
			if(response.data){
				await apiFondoSolidaridad
				.postActualizarEntrevista(request)
				.then(async responseEntrevista => {
					if(responseEntrevista.returnMessage ===  "Exitoso"){
						setLoading(false);
						await NotificationService.genericAlert(
							'Actualizado con éxito',
							'Se realizó con éxito la actualización.',
						);
		    } 
			}).catch((e) => {
					setLoading(false);
					toast(`Error en actualizar entrevista ene postulacion ${e.message}`, { type: toast.TYPE.ERROR });
			});
			}
		})
		.catch((e) => {
		    setLoading(false);
			toast(`Error del sistema actualizar postulacion en afiliado fallecido ${e.message}`, { type: toast.TYPE.ERROR });
		});
};

const actualizarPostulacionValorAfectaciones = async (datosPostulacion: any) => {
	setLoading(true);
	await apiFondoSolidaridad
		.postActualizarSolicitudPostulante(datosPostulacion)
		.then(async response => {
			if(response.data){
				setLoading(false);
			}
		})
		.catch((e) => {
		    setLoading(false);
			toast(`Error del sistema Actualizar Valor Afectaciones ${e.message}`, { type: toast.TYPE.ERROR });
		});
};

const consultarCalificacion = async (idSolicitud: string) => {

	setLoading(true);
	const HECHO_MUERTE_DEL_AFILIADO = 'MUERTE DEL AFILIADO';
	let hechoDescripcion;
	await apiFondoSolidaridad
		.getConsultarCalificacion(idSolicitud)
		.then(async response => {
			if (response.data) {
				if (response?.data?.hechoDescripcion === null) {
					hechoDescripcion = '';
				} else if (response?.data?.hechoDescripcion === HECHO_MUERTE_DEL_AFILIADO) {
					hechoDescripcion = '100';
				} else {
					hechoDescripcion = '0';
				}
				formularioCalificacion.setValue('calificacionId', response?.data?.calificacionId);
				formularioCalificacion.setValue('solicitudId', response?.data?.solicitudId);
				formularioCalificacion.setValue('tipoImputabilidad', response?.data?.tipoImputabilidad);
				formularioCalificacion.setValue('imputabilidadAlServicio', response?.data?.imputabilidadAlServicio);
				formularioCalificacion.setValue('hecho', hechoDescripcion);
				formularioCalificacion.setValue('datosResolucion', response?.data?.datosResolucion);
				formularioCalificacion.setValue('estadoId', response?.data?.estadoId);
				formularioCalificacion.setValue('actividad', response?.data?.actividad);
				if (response?.data?.porcentajePostulacion === 0) {
					formularioCalificacion.setValue('porcentajePostulacion', '');
				} else {
					formularioCalificacion.setValue('porcentajePostulacion', response?.data?.porcentajePostulacion === 100 ? response?.data?.porcentajePostulacion : 50);
				}
				formularioCalificacion.setValue('observacion', response?.data?.observacion);
				// porcentaje disminucion
				formularioCalificacion.setValue('porcentajeDisminucion', response?.data?.porcentajeDisminucion);
				if (response?.data?.porcentajeDisminucion === 100) {
					setMostrarCampoPorcentajeDisminucion(false);
				} else {
					setMostrarCampoPorcentajeDisminucion(true);
				}

				if (response?.data?.disfrutePension === null) {
					formularioCalificacion.setValue('disfrutePension', datosSolicitudPostulacion?.disfrutePension);
				} else if (response?.data?.disfrutePension !== null) {
					formularioCalificacion.setValue('disfrutePension', response?.data?.disfrutePension);
				}
				setDatosCalificacion(response?.data);

				// Solicitudes Previas
				if (cliente && cliente?.solicitudes && cliente?.solicitudes.length > 0) {

					const solicitudesPrevias = cliente?.solicitudes.filter((solicitud: any) =>
						(solicitud.tipoSolicitud === 'FONDO SOLIDARIDAD 1305' ||
							solicitud.tipoSolicitud === 'FONDO SOLIDARIDAD ACUERDO 01/2016' ||
							solicitud.tipoSolicitud === 'GIRO DE RECURSOS FONDO SOLIDARIDAD' ||
							solicitud.tipoSolicitud === 'GIRO DE RECURSOS FONDO SOLIDARIDAD ACUERDO 01/2016' ||
							solicitud.tipoSolicitud === 'GIRO RECURSOS FONDO SOLIDARIDAD 1305' ||
							solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD' ||
							solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD ACUERDO 01/2016' ||
							solicitud.tipoSolicitud === 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD' ||
							solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD' ||
							solicitud.tipoSolicitud === 'POSTULACIÓN FONDO SOLIDARIDAD')
						&& new Date(solicitud.fechaInicioSolicitud) < new Date(datosSolicitudPostulacion?.fechaCreacion));

					if (solicitudesPrevias && solicitudesPrevias.length === 1) {
						formularioCalificacion.setValue('solicitudesPrevias', solicitudesPrevias?.tipoSolicitud);
					} else if (solicitudesPrevias && solicitudesPrevias.length > 1) {
						const tipoSolicitudesUnicas: string[] = Array.from(
							new Set(solicitudesPrevias.map((item: any) => item.tipoSolicitud))
						);
						const descripcionSolicitudesPrevias: string = tipoSolicitudesUnicas.join(', ');
						formularioCalificacion.setValue('solicitudesPrevias', descripcionSolicitudesPrevias);
					} else {
						formularioCalificacion.setValue('solicitudesPrevias', 'N/A');
					}
				} else {
					formularioCalificacion.setValue('solicitudesPrevias', 'N/A');
				}

				if (datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
					datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.MECANISMOS_ESPECIALES) {
					// Tipo imputabilidad
					formularioCalificacion.setValue('tipoImputabilidad', TIPO_IMPUTABILIDAD.DISMINUCION);
					formularioCalificacion.setValue('hecho', '0');
					setMostrarCampoPorcentajeDisminucion(true);

					
					// Imputabilidad al servicio 
					const imputabilidadObject = imputabilidad_servicio_disminucion_capacidad.find(item => item?.id === response?.data?.imputabilidadAlServicio);
					if (imputabilidadObject) {
						setImputabilidadValue(imputabilidadObject || null);
						setNullImputabilidadValue(false);
						formularioCalificacion.setValue(
							'imputabilidadAlServicio',
							imputabilidadObject?.id,
						);
					}
				} else if (datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA) {
					// Tipo imputabilidad
					formularioCalificacion.setValue('tipoImputabilidad', TIPO_IMPUTABILIDAD.MUERTE);
					formularioCalificacion.setValue('hecho', '100');
					setMostrarCampoPorcentajeDisminucion(false);

					// Imputabilidad al servicio 
					const imputabilidadObject = imputabilidad_servicio_muerte.find(item => item?.id === response?.data?.imputabilidadAlServicio);
					if (imputabilidadObject) {
						setImputabilidadValue(imputabilidadObject || null);
						setNullImputabilidadValue(false);
						formularioCalificacion.setValue(
							'imputabilidadAlServicio',
							imputabilidadObject?.id,
						);
					}
				}
				setLoading(false);
			} else {
				setLoading(false);
			}

		})
		.catch(() => {
			setLoading(false);
			toast('Error al consultar calificación', { type: toast.TYPE.ERROR });
		});
};

const onChangeImputabilidad = (event: any, imputabilidad: any) => {
	setNullImputabilidadValue(imputabilidad == null);
	setImputabilidadValue(imputabilidad);
	if (imputabilidad) {
		formularioCalificacion.setValue('imputabilidadAlServicio', String(imputabilidad.id));
		formularioCalificacion.setValue('imputabilidadServicioDescripcion', imputabilidad.descripcion);
		formularioCalificacion.clearErrors('imputabilidadAlServicio');
	}
};


const generarInconsistencia = () => {
	console.log('generar oficio')
}

const handleOpenModalRetiro = () => setModalRetiroDesvinculacion(true);

const handleOpenModalInconsistencia = () => setModalDeclararInconsistencia(true);

const handleCloseModalRetiro = () => {
	setModalRetiroDesvinculacion(false);
	setMostrarCampoDisfrutePension(false);
	setMostrarCampoInvalidezoMuerte(true);
	setLoading(false);
	formularioRetiro.reset();
}

const handleCloseModalDeclararInconsistencia = () => {
	setModalDeclararInconsistencia(false);
	setLoading(false);
	formularioDeclaracionInconsistencia.reset();
}

const validarExisteSolicitudPorId = async (idSolicitud: string) => {
        await apiFondoSolidaridad
            .postValidarExistePostulacionPorId(idSolicitud)
            .then(async response => {
                if(response.data){
					if(selectedData){
						const datos = {
							actividad: selectedData.actividad,
							apellidoFirma: response.data?.apellidoFirma,
							ciuadad1: response.data?.ciuadad1,
							ciudad2: response.data?.ciudad2,
							ciudad3: response.data?.ciudad3,
							deparmento3: response.data?.deparmento3,
							departameno2: response.data?.departameno2,
							departamento1: response.data?.departamento1,
							estadoProceso: selectedData.estadoProceso,
							fechaCreacion: response.data?.fechaCreacion,
							fechaExpedicion: response.data?.fechaExpedicion,
							firmaSolicitud: response.data?.firmaSolicitud,
							idModalidad: response.data?.idModalidad,
							idSolicitud: response.data?.idSolicitud,
							idSubModalidad: response.data?.idSubModalidad,
							idTipoSolicitud: response.data?.idTipoSolicitud,
							manifestacionAutorizacion: response.data?.manifestacionAutorizacion,
							nombreCompleto: response.data?.nombreCompleto,
							nombreFirma: response.data?.nombreFirma,
							numeroCelular: response.data?.numeroCelular,
							numeroConvocatoria: response.data?.numeroConvocatoria,
							numeroDocumentoFirma: response.data?.numeroDocumentoFirma,
							numeroDocumeto: response.data?.numeroDocumeto,
							observacion: response.data?.observacion,
							observacionDocumento: response.data?.observacionDocumento,
							postulantes: response.data?.postulantes,
							tipoModalidadNombre: response.data?.tipoModalidadNombre,
							tipoSolicitudNombre: response.data?.tipoSolicitudNombre,
							tipoSubModalidadNombre: response.data?.tipoSubModalidadNombre,
							representanteLegal: response.data?.representanteLegal,
							disfrutePension: response.data?.disfrutePension,
							noRadicado: response.data?.noRadicado,
							numeroResolucion: response.data?.numeroResolucion,
							numeroResolucionCierre: response.data?.numeroResolucionCierre,
							fechaApertura: response.data?.fechaApertura,
							fechaCierre: response.data?.fechaCierre,
							modalidadSolucionVivienda: response?.data?.modalidadSolucionVivienda,
							afectacionCuenta: response?.data?.afectacionCuenta,
							valorAfectacion: response?.data?.valorAfectacion,
						}
						setDatosSolicitudPostulacion(datos);
						formularioPostulacion.setValue('idSolicitud',response?.data?.idSolicitud);
						formularioPostulacion.setValue('afectacionesCuenta',response?.data?.afectacionCuenta);
						formularioPostulacion.setValue('valorAfectaciones',response?.data?.valorAfectacion);
						formularioPostulacion.setValue('numeroRadicado',response.data?.noRadicado);
						formularioPostulacion.setValue('modalidadSolucionVivienda',response.data?.modalidadSolucionVivienda);
					}else{
						setDatosSolicitudPostulacion(response.data);
					}
                }
            })
            .catch((e) => {
                toast(`Error validar existe solicitud ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

	const regresarPantallaInicial = () => {
        formularioSolicitud.reset();
        formularioRetiro.reset();
        setAgregarNuevaPostulacion(false);
        setActividadSolicitud('');
        setEstadoSolicitud('');
        setAgregarSolicitudPostulacion(false);
        setSelectedData(null);
    }

if(actividadSolicitud && estadoSolicitud){

  if((actividadSolicitud === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD 
	  && estadoSolicitud ===  ESTADO_POSTULACION.APROBADO) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS 
	  && estadoSolicitud ===  ESTADO_POSTULACION.PENDIENTE) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS 
	  && estadoSolicitud ===  ESTADO_POSTULACION.APROBADO) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS 
	  && estadoSolicitud ===  ESTADO_POSTULACION.ANULADO) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES 
	  && estadoSolicitud ===  ESTADO_POSTULACION.BLOQUEADO) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES 
	  && estadoSolicitud ===  ESTADO_POSTULACION.PENDIENTE) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS 
	  && estadoSolicitud ===  ESTADO_POSTULACION.BLOQUEADO) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS 
	  && estadoSolicitud ===  ESTADO_POSTULACION.PENDIENTE) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL 
	  && estadoSolicitud ===  ESTADO_POSTULACION.BLOQUEADO) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL 
	  && estadoSolicitud ===  ESTADO_POSTULACION.PENDIENTE) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.ENTREVISTA 
	  && estadoSolicitud === ESTADO_POSTULACION.PENDIENTE) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.ENTREVISTA 
	  && estadoSolicitud === ESTADO_POSTULACION.APROBADO) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.ENTREVISTA 
	  && estadoSolicitud === ESTADO_POSTULACION.ANULADO) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CARGUE_DOCUMENTOS 
	  && estadoSolicitud === ESTADO_POSTULACION.INCONSISTENTE) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.ENTREVISTA 
	  && estadoSolicitud === ESTADO_POSTULACION.INCONSISTENTE) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CALIFICACION 
	  && estadoSolicitud === ESTADO_POSTULACION.PENDIENTE) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CALIFICACION 
	  && estadoSolicitud === ESTADO_POSTULACION.APROBADO) || 
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CALIFICACION 
	  && estadoSolicitud === ESTADO_POSTULACION.ANULADO) ||
	  (actividadSolicitud === ACTIVIDAD_POSTULACION.CALIFICACION 
	  && estadoSolicitud === ESTADO_POSTULACION.RECHAZADO)){

		return(
			<EtapasPostulacion 
				formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
				formularioSolicitud={formularioSolicitud}
				formularioRetiro={formularioRetiro}
				formularioPostulacion={formularioPostulacion}
				formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
				formularioEntrevista={formularioEntrevista}
				formularioCalificacion={formularioCalificacion}
				formularioInconsistencia={formularioDeclaracionInconsistencia}
				imputabilidadValue={imputabilidadValue}
				nullImputabilidadValue={nullImputabilidadValue}
				mostrarCampoPorcentajeDisminucion={mostrarCampoPorcentajeDisminucion}
				catalogState={catalogState}
				cliente={cliente}
				tipoSolicitud={Number(formularioSolicitud.getValues('tipoSolicitud'))}
				listaPostulantes={listaPostulantes}
				datosSolicitudPostulacion={datosSolicitudPostulacion} 
				openDeclararInconsistencia={openModalDeclararInconsistencia}
				beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion}
				datosCalificacion={datosCalificacion}
				setMostrarCampoPorcentajeDisminucion={setMostrarCampoPorcentajeDisminucion}
				setListaPostulantes={setListaPostulantes}
				setActividadSolicitud={setActividadSolicitud}
				setEstadoSolicitud={setEstadoSolicitud}
				setAgregarNuevaPostulacion={setAgregarNuevaPostulacion}
				consultarConvocatorias={consultarConvocatorias}   
				setAgregarSolicitudPostulacion={setAgregarSolicitudPostulacion}
				setSelectedData={setSelectedData}
				consultarSolicitudes={consultarSolicitudes}
				actualizarPostulacionValorAfectaciones={actualizarPostulacionValorAfectaciones}
				validarExisteSolicitudPorId={validarExisteSolicitudPorId}
				generarInconsistencia={generarInconsistencia}
				handleOpenModalInconsistencia={handleOpenModalInconsistencia}
				handleCloseModalInconsistencia={handleCloseModalDeclararInconsistencia}
				consultarBenficiariosPostulacion={consultarBenficiariosPostulacion}
				setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion}
				onChangeImputabilidad={onChangeImputabilidad}
				consultarCalificacion={consultarCalificacion}
			/>
		)
	}

	 if((actividadSolicitud === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD 
		&& estadoSolicitud ===  ESTADO_POSTULACION.INCONSISTENTE) || 
		(actividadSolicitud === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD 
		&& estadoSolicitud ===  ESTADO_POSTULACION.PENDIENTE) ||
		(actividadSolicitud === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD 
		&& estadoSolicitud ===  ESTADO_POSTULACION.ANULADO)){
  
		  return(
			<Postulacion
				setAgregarNuevaPostulacion={setAgregarNuevaPostulacion} 
				formularioPostulacion={formularioPostulacion}
				loginState={loginState} 
				tipoSolicitud={Number(formularioSolicitud.watch('tipoSolicitud'))} 
				tipoModalidad={Number(formularioSolicitud.watch('tipoModalidad'))}     
				consultarConvocatorias={consultarConvocatorias} 
				numeroRadicado={numeroRadicado}  
				formularioSolicitud={formularioSolicitud}   
				formularioRetiro={formularioRetiro} 
				datosSolicitudPostulacion={datosSolicitudPostulacion}
				beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion}
				setDatosSolicitudPostulacion={setDatosSolicitudPostulacion}
				setActividadSolicitud={setActividadSolicitud}
				setEstadoSolicitud={setEstadoSolicitud}
				setAgregarSolicitudPostulacion={setAgregarSolicitudPostulacion}
				selectedData={selectedData}
				consultarSolicitudes={consultarSolicitudes}
				setSelectedData={setSelectedData}
				actualizarPostulacion={actualizarPostulacion}
				setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion}
				actualizarPostulacionAfiliadoFallecido={actualizarPostulacionAfiliadoFallecido}
				consultarBenficiariosPostulacion={consultarBenficiariosPostulacion}
				cliente={cliente}
				existePostulacion={existePostulacion}
				camposSolicitudPostulacionSoloLectura={camposSolicitudPostulacionSoloLectura}
			/>
		  )
	  }
}

if(agregarNuevaPostulacion){
    return (
		<Postulacion
			setAgregarNuevaPostulacion={setAgregarNuevaPostulacion} 
			formularioPostulacion={formularioPostulacion}
			loginState={loginState} 
			tipoSolicitud={Number(formularioSolicitud.watch('tipoSolicitud'))} 
			tipoModalidad={Number(formularioSolicitud.watch('tipoModalidad'))}     
			consultarConvocatorias={consultarConvocatorias} 
			numeroRadicado={numeroRadicado}    
			formularioSolicitud={formularioSolicitud}   
			formularioRetiro={formularioRetiro} 
			datosSolicitudPostulacion={datosSolicitudPostulacion}
			beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion}
			setDatosSolicitudPostulacion={setDatosSolicitudPostulacion}
			setActividadSolicitud={setActividadSolicitud}
			setEstadoSolicitud={setEstadoSolicitud}
			setAgregarSolicitudPostulacion={setAgregarSolicitudPostulacion}
			selectedData={selectedData}
			consultarSolicitudes={consultarSolicitudes}
			setSelectedData={setSelectedData}
			actualizarPostulacion={actualizarPostulacion}
			actualizarPostulacionAfiliadoFallecido={actualizarPostulacionAfiliadoFallecido}
			consultarBenficiariosPostulacion={consultarBenficiariosPostulacion}
			setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion}
			cliente={cliente}
			existePostulacion={existePostulacion}
			camposSolicitudPostulacionSoloLectura={camposSolicitudPostulacionSoloLectura}
		/>
    )
}

if(tipoSolicitudSeleccionado === TIPO_SOLICITUD.AJUDICACION_VIVIENDA){
	return (
		<AdjudicacionVivienda
			catalogState={catalogState}
			cliente={cliente}
			datosSolicitudPostulacion={datosSolicitudPostulacion}
			tipoSolicitud={Number(formularioSolicitud.watch('tipoSolicitud'))}
			mostrarCampoPorcentajeDisminucion={mostrarCampoPorcentajeDisminucion}
			formularioPostulacion={formularioPostulacion}
			formularioCalificacion={formularioCalificacion}
			formularioEntrevista={formularioEntrevista}
			formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
			formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
			listaPostulantes={listaPostulantes}
			setListaPostulantes={setListaPostulantes}
			regresarPantallaInicial={regresarPantallaInicial}
			setMostrarCampoPorcentajeDisminucion={setMostrarCampoPorcentajeDisminucion}
			consultarCalificacion={consultarCalificacion}
		/>
	)
}

if(tipoSolicitudSeleccionado === TIPO_SOLICITUD.PERDIDA_BENEFICIO){
	return (
		<PerdidaDeBeneficio
			cliente={cliente}
			catalogState={catalogState}
			tipoSolicitud={Number(formularioSolicitud.watch('tipoSolicitud'))}
			formularioPostulacion={formularioPostulacion}
			datosSolicitudPostulacion={datosSolicitudPostulacion}
			formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
			formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
			regresarPantallaInicial={regresarPantallaInicial}
		/>
	)
}

return (
		<>
		<CHPrivateContainer>
			<LoadingV2Component key="loader" loading={loadingBiometriaVigia} tittle='Consultando Vigía Sarlaft' />
			<LoadingComponent loading={loading} />
			<CHPageTitleV2
				topText="Gestión de postulaciones"
				title1="Solicitud Postulación"
                title2='Fondo Solidaridad'
				icono="fact_check"
			/>
			<Container>
				<Grid item xs={12}>
					<Card>
						<CardContent>
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIconoV3
										title1="Tipo"
										subtitle="de documento"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={7}>
									<Controller
										name="tipoDocumento"
										control={formularioSolicitud.control}
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
									<TituloIconoV3
										title1="Número"
										subtitle="de documento"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={7}>
									<Controller
										name="numeroDocumento"
										control={formularioSolicitud.control}
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
												InputLabelProps={{ shrink: true }}
												isReadOnly={false}
												label="NÚMERO DE DOCUMENTO"
												value={value}
												onChange={onChange}
											/>
										)}
									/>
								</Grid>
							</Grid>
							<br />
							<br />
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIconoV3
										title1="Convocatoria"
										subtitle=""
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={7}>
									<Controller
										name="convocatoria"
										control={formularioSolicitud.control}
										rules={{
											required: {
												value: Number(formularioSolicitud.watch('tipoSolicitud')) === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD,
												message:
													'Este campo es requerido.',
											},
										 }}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												error={!!error}
												helperText={
													error ? error.message : null
												}
												InputLabelProps={{ shrink: true }}
												isReadOnly
												label="CONVOCATORIA"
												value={value}
												onChange={onChange}
											/>
										)}
									/>
								</Grid>
							</Grid>
							<br />
							<br />
							<Grid container justifyContent="space-around">
								<Grid item xs={3}>
									<TituloIconoV3
										title1="Tipo"
										subtitle="de solicitud"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={7}>
									<Controller
										name="tipoSolicitud"
										control={formularioSolicitud.control}
										rules={{
											required: 'Este campo es requerido.',
										 }}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHSelect
												error={!!error}
												label="TIPO SOLICITUD"
												value={value}
												helperText={
													error ? error.message : null
												}
												onChange={e => {
													consultarTipoModalidad(Number(e.target.value));
													onChange(e);
												}}
											>
											{tipoSolicitud?.map(
												(option: any) => ( 
													<MenuItem key={option.id} 
															  value={option.id}
													>
														{option.nombre}
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
									<TituloIconoV3
										title1="Modalidad"
										subtitle="(Modelo)"
										icono="event_available"
									/>
								</Grid>
								<Grid item xs={7}>
									<Controller
										name="tipoModalidad"
										control={formularioSolicitud.control}
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
												label="MODALIDAD (MODELO)"
												value={ value }
												helperText={
													error ? error.message : null
												}
												onChange={e  => {
													consultarTipoSubModalidad(Number(e.target.value));
													onChange(e);
												}}
											>
												
												{tipoModalidad?.map(
												(option: any) => ( 
													<MenuItem key={option.id} 
															  value={option.id}
													>
														{option.nombre}
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
                                    <TituloIconoV3
                                        title1="Sub-Modalidad"
                                        subtitle="(Calidad Postulación)"
                                        icono="event_available"
                                    />
                                </Grid>
                                <Grid item xs={7}>
                                    <Controller
                                        name="tipoSubModalidad"
                                        control={formularioSolicitud.control}
                                        // rules={{
                                        //     required:
                                        //         'Este campo es requerido.',
                                        // }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="SUB-MODALIDAD (CALIDAD POSTULACIÓN)"
                                                value={ value }
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                            {tipoSubModalidad?.map(
                                                (option: any) => (
                                                    <MenuItem key={option.id}
                                                              value={option.id}
                                                    >
                                                        {option.nombre}
                                                    </MenuItem>
                                                 ),
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
						disabled={loading}
						onClick={onClickCancelar}
					>
						CANCELAR
					</Button>
					<div className={classes.wrapper}>
						<Button
							className={classes.buttonprincipal}
							variant="outlined"
							disabled={loading}
							onClick={formularioSolicitud.handleSubmit(onSubmit)}
						>
							ACEPTAR
						</Button>
					</div>
				</div>
			</Container>
		</CHPrivateContainer>
		<ModalRetiroDesvinculacion
			cliente={cliente}
			open={openModalRetiroDesvinculacion}
			handleClose={handleCloseModalRetiro}
			formularioRetiro={formularioRetiro}
			mostrarCampoDisfrutePension={mostrarCampoDisfrutePension}
			setMostrarCampoDisfrutePension={setMostrarCampoDisfrutePension}
			mostrarCampoInvalidezoMuerte={mostrarCampoInvalidezoMuerte}
			setMostrarCampoInvalidezoMuerte={setMostrarCampoInvalidezoMuerte}
			validacionRetiro={validacionRetiro}
			tipoSolicitud={Number(formularioSolicitud.watch('tipoSolicitud'))} 
			tipoModalidad={Number(formularioSolicitud.watch('tipoModalidad'))}  
			tipoSubModalidad={Number(formularioSolicitud.watch('tipoSubModalidad'))}    
		/>
		<ModalDeclararInconsisntencia
			open={openModalDeclararInconsistencia}
			handleClose={handleCloseModalDeclararInconsistencia}
			generarInconsistencia={generarInconsistencia}
			formularioInconsistencia={formularioDeclaracionInconsistencia}
		/>
		</>
	);
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    loginState: LoginState;
	actividadSolicitud: string;
	estadoSolicitud: string;
	datosSolicitudPostulacion: any;
	selectedData: SolicitudDto | null;
	setActividadSolicitud: (value: string) => void;
	setEstadoSolicitud: (value: string) => void;
	setDatosSolicitudPostulacion: (value: any | null) => void;
    setAgregarSolicitudPostulacion: (value: boolean) => void;
	setSelectedData: (value : SolicitudDto | null ) => void;
	consultarSolicitudes: (request: any) => void;
	startGetAllIdentificationTypes: (claims: string) => void;
    startGetAllForces: (claims: string) => void;
    startGetAllGrades: (claims: string) => void;
    startGetCategory: (claims: string) => void;
	startGetAllExecutingUnits: (claims: string) => void;
    startGetPhoneTypes: (claims: string) => void;
	startGetAllCivilState: (claims: string) => void;
	startGetAllProfessions: (claims: string) => void;
	startGetAllDepartments: () => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetAllForces,
    startGetAllGrades,
    startGetCategory,
	startGetAllExecutingUnits,
    startGetPhoneTypes,
	startGetAllCivilState,
	startGetAllProfessions,
	startGetAllDepartments,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(SolicitudPostulacion);
