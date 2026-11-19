import React from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import SearchInfo from '../pages/curriculum/components/searchInfo.component';
import Curriculum1 from '../pages/curriculum/curriculum';
import { Layout } from '../pages/Layout/Layout';
import ParametrizationLegal from '../pages/parametrizationLegal/parametrizationLegal';
import State from '../pages/state/state';
import LoginPage from '../pages/login/Login';
import AuthService from '../services/auth.service';
import PreaprobacionesInicio from '../pages/preaprobaciones/PreaprobacionesInicio';
import ProcedureRequests from '../pages/ProcedureRequests/ProcedureRequests';
import CargueNomina from '../pages/cargue_nomina/cargue_nomina';
import ProductData from '../pages/productClient/components/ProductData';
import DisbursementCreation from '../pages/productClient/components/DisbursementCreation';
import AddProductData from '../pages/productClient/components/AddProductData';
import Bloqueos from '../pages/parametrizationLock/Bloqueo';
import Notify from '../pages/notify/Notify';
import Notifications from '../pages/notifications/Notifications';
import ConsultCreditCommittee from '../pages/ConsultCreditCommittee/ConsultCreditCommittee';
import Disbursement from '../pages/Disbursement/Disbursement';
import Account from '../pages/account/account.component';
import ScheduleNovelties from '../pages/ScheduleNovelties/ScheduleNovelties';
import RequestInformationLegalizationData from '../components/RequestInformationLegalizationData/RequestInformationLegalizationData';
import Court from '../pages/court/Court';
import Embargos from '../pages/embargos/Embargos';
import EmbargosInicio from '../pages/embargos/EmbargosInicio';
import Desembargo from '../pages/desembargo/Desembargo';
import Simulador from '../pages/parametrizationMortgageCredit/Simulador';
import TipoActor from '../pages/parametrizationExceptionActorType/TipoActor';
import UnidadEjecutora from '../pages/parametrizationExceptionExecutorUnit/UnidadEjecutora';
import GeneralParameterization from '../pages/generalParameterization/components/GeneralParameterization';
import TipoDocumento from '../pages/parametrizationTypeId/TipoDocumento';
import Genero from '../pages/parametrizationGender/Genero';
import Fuerzas from '../pages/parametrizationForces/Fuerzas';
import Categorias from '../pages/ParametrizationCategories/Categoria';
import Plazos from '../pages/parametrization/Plazos';
import Seguros from '../pages/parametrization/Seguros';
import Liquidacion from '../pages/parametrization/Liquidacion';
import MasivePayAplication from '../pages/productClient/components/MassivePayAplication';
import SeguroListas from '../pages/productClient/components/SeguroListas';
import GenerarExtracto from '../pages/productClient/components/GenerarExtracto';
import AdminUsuarios from '../pages/RolesPerfiles/Usuarios/usuarios';
import EditarUsuarios from '../pages/RolesPerfiles/Usuarios/editar-usuario/editar-usuario';
import CrearUsuario from '../pages/RolesPerfiles/Usuarios/crear-usuario/crear-usuario';
import AdminGrupoUsuarios from '../pages/RolesPerfiles/grupo-usuarios/grupo-usuarios';
import CrearGrupoUsuarios from '../pages/RolesPerfiles/grupo-usuarios/crear-grupo-usuario/crear-grupo-usuario';
import ActualizarGrupoUsuarios from '../pages/RolesPerfiles/grupo-usuarios/actualizar-grupo-usuarios/actualizar-grupo-usuarios';
import RecuperacionContrasena from '../pages/RolesPerfiles/RecuperacionContrasena/recuperacion-contrasena';
import AdminGrupoOpciones from '../pages/RolesPerfiles/grupo-opciones/grupo-opciones';
import RestablecerContrasena from '../pages/RolesPerfiles/RestablecerContrasena/restablecer-contrasena';
import CrearGrupoOpciones from '../pages/RolesPerfiles/grupo-opciones/crear-grupo-opciones/crear-grupo-opciones';
import ActualizaGrupoOpciones from '../pages/RolesPerfiles/grupo-opciones/actualiza-grupo-opciones/actualiza-grupo-opciones';
import realStateRegistrations from '../pages/realStateRegistrations/realStateRegistrations';
import educationalEntities from '../pages/educationalEntities/educationalEntities';
import accordionEducationalEntities from '../pages/educationalEntities/components/accordionEducationalEntities';
import BasicInformationEntitiesComponent from '../pages/educationalEntities/components/BasicInformationEntities.component';
import campusComponent from '../pages/educationalEntities/components/campus.component';
import BlocksEntities from '../pages/educationalEntities/components/blocksEntities.component';
import AccountsComponent from '../pages/educationalEntities/components/accounts.component';
import programsComponent from '../pages/educationalEntities/components/programs.component';
import accordionRealStateRegistrations from '../pages/realStateRegistrations/components/accordionRealStateRegistrations';
import addNoveltyOperations from '../pages/realStateRegistrations/components/addNoveltyOperations';
import affiliatesExtraordinary from '../pages/affiliatesExtraordinary/affiliatesExtraordinary';
import formularioReintegro from '../pages/affiliatesExtraordinary/component/formularioReintegro';
import InsertarAfiliacion from '../pages/affiliatesExtraordinary/component/crearAfiliacion';
import Opciones from '../pages/RolesPerfiles/Opciones/Opciones';
import CrearOpciones from '../pages/RolesPerfiles/Opciones/Crear-opcion';
import AddEducationalEntities from '../pages/educationalEntities/components/addEducationalEntities';
import LogAuditoria from '../pages/Auditorias/auditorias';
import DetalleAuditoria from '../pages/Auditorias/detalleauditoria';
import formMatricula from '../pages/realStateRegistrations/components/formMatricula';
import editHistory from '../pages/realStateRegistrations/components/editHistory';
import AdministracionCuentas from '../pages/AdministracionCuentas/AdministracionCuentas';
import controlAdministracionCuentas from '../pages/AdministracionCuentas/tableroControlAdministracionCuentas/controlAdministracionCuentas';
import CierreMensual from '../pages/cierreMensual/cierremensual';
import ListarCierreMensual from '../pages/cierreMensual/ListaCierremensual';
import TablasParametros from '../pages/TablasParametros/components/searchTablasParametros';
import SolicitudesInicio from '../pages/solicitudes/SolicitudesInicio';
import Solicitudes from '../pages/solicitudes/solicitudes';
import RetiroDesvinculacion from '../pages/solicitudes/retiroDesvinculacionCapacidadPsicofisica/RetiroDesvinculacionCapacidadPsicofisica';
import BeneficiarioPorFallecimiento from '../pages/solicitudes/beneficiarioFallecimientoDelAfiliado/BeneficiarioPorFallecimientoDelAfiliado';
import MecanismosEspeciales from '../pages/solicitudes/mecanismosEspeciales/MecanismosEspeciales';
import CierreDiarioCartera from '../pages/cierresCartera/CierreDiarioCartera';
import CierreMensualCartera from '../pages/cierresCartera/CierreMensualCartera';
import CierreAnualCartera from '../pages/cierresCartera/CierreAnualCartera';
import DetalleEmbargosConcepto from '../pages/embargoConceptos/DetalleEmbargosConcepto';
import detalleAportes from '../pages/affiliatesExtraordinary/component/detalleAportes';
import Convocatoria from '../pages/fondoSolidaridad/convocatorias/Convocatoria';
import ReporteRanking from '../pages/fondoSolidaridad/reporteRanking/ReporteRanking';
import Reintegros from '../pages/AdministracionCuentas/components/reintegros/reintegros';
import PostulacionInicio from '../pages/fondoSolidaridad/postulacion/PostulacionInicio';
import IPC from '../pages/AdministracionCuentas/components/IPC/IPC';
import verificacionDerechos from '../pages/AdministracionCuentas/components/verificacion/verificacionDerechos';
import verificacionDerechosDetalles from '../pages/AdministracionCuentas/components/verificacion/verificacionDerechosDetalles';
import AbonoIntereses from '../pages/AdministracionCuentas/components/abono-intereses/abonoIntereses';
import AbonoInteresesDetalles from '../pages/AdministracionCuentas/components/abono-intereses/abonoInteresesDetalles';
import ReportsGenerales from '../pages/Reports/ReportsGenerales';
import SolicitudPostulacion from '../pages/fondoSolidaridad/postulacion/SolicitudPostulacion';
import SeguroHogar from '../components/ParametrizacionSegurosHV/SeguroHogar';
import newsManagement from '../pages/newsManagement/NewsManagement';
import AddNew from '../pages/newsManagement/AddNew';
import EditNew from '../pages/newsManagement/EditNew';
import DesembargoInicio from '../pages/desembargo/DesembargoInicio';
import consultaNovedadesActivas from '../pages/consultaNovedadesActivas/consultaNovedadesActivas';
import InformacionDesembargos from '../pages/desembargo/InformacionDesembargo';
import AnularEmbargo from '../pages/desembargo/AnularEmbargo';
import EditNewInData from '../pages/newsInData/EditNewInData';
import PagoEmbargosInicio from '../pages/pagoEmbargos/PagoEmbargosInicio';
import InformacionPagoEmbargos from '../pages/pagoEmbargos/InformacionPagoEmbargos';
import HomePage from '../pages/home/Home';
import RegistroNovedades from '../pages/registroNovedades/RegistroNovedades';
import Proyectos from '../pages/fondoSolidaridad/proyectos/Proyectos';
import RegistroEmbargos from '../pages/registroEmbargos/RegistroEmbargos';
import CausacionDiariaInteresesCorrientes from '../pages/cierresCartera/CausacionDiariaInteresesCorrientes';
import CierresCausacionesRealizadas from '../pages/cierresCartera/CierresCausacionesRealizadas';
import NovedadesCredito from '../pages/productClient/components/NovedadesCredito';
import ParametrizacionLeasing from '../pages/parametrizationLeasing/ParametrizacionLeasing';
import ReporteCuentasIndividual from '../pages/AdministracionCuentas/components/reportes-cuenta-individual/ReporteCuentasIndividual';
import SeeDetailNew from '../pages/newsManagement/SeeDetailNew';
import InformacionContable from '../pages/contable/InformacionContable';
import consultaNovedades from '../pages/consultaNovedadesActivas/consultaNovedades';
import ConsultaSaldos from '../pages/ConsultaSaldosYCartera/ConsultaSaldos';
import ConsultaReportesGA2 from '../pages/ReportesGA2/ReportesGA2';
import ReporteCausaciones from '../pages/contable/ReporteCausaciones';
import consultaNovedadesHistorial from '../pages/consultaNovedadesActivas/consultaNovedadesHistorial';
import ReporteContablePagos from '../pages/contable/ReporteContablePagos';
import ReportePagos from '../pages/cierresCartera/ReportePagos';
import ReporteSolucionInconsistencias from '../pages/ReporteSolucionInconsistencias/ReporteSolucionInconsistencias';
import AplicacionPagoMasivo from '../pages/productClient/components/AplicacionPagoMasivo';
import ExtractoMensual from '../pages/productClient/components/reportes/extractosFinancieros/ExtractoMensual';
import ExtractoAnual from '../pages/productClient/components/reportes/extractosFinancieros/ExtractoAnual';
import ExtractoPlanPagosMensual from '../pages/productClient/components/reportes/extractosPlanPagos/ExtractoMensual';
import ExtractoPlanPagosAnual from '../pages/productClient/components/reportes/extractosPlanPagos/ExtractoAnual';
import CertificadoIngresosRetenciones from '../pages/productClient/components/reportes/CertificadoIngresosRetenciones';
import ReporteGeneralCartera from '../pages/productClient/components/reportes/reporteGeneralCartera/ReporteGeneralCartera';
import ReporteGeneralReestructuracion from '../pages/productClient/components/reportes/reporteGeneralReestructuracion/ReporteGeneralReestructuracion';
import GenerarComprobanteContable from '../pages/contable/CrearComprobanteContable';
import GenerarComprobanteContableIntegracion from '../pages/contable/CrearComprobanteContableIntegracion';
import ReporteAPNRCAR from '../pages/productClient/components/reportes/reporteAPNRCAR/ReporteAPNRCAR';
import ReporteGeneralGarantia from '../pages/productClient/components/reportes/reporteGeneralGarantia/ReporteGeneralGarantia';
import ConsultarComprobanteCartera from '../pages/contable/ConsultarComprobanteCartera';
import IntervinienteInico from '../pages/solicitudes/intervinientes/IntervininteInico';
import AgregarNuevoActor from '../pages/solicitudes/intervinientes/AgregarNuevoActor';
import DetalleActulizacionesInterviniente from '../pages/solicitudes/intervinientes/DetalleActulizacionesInterviniente';
import ReporteGeneralMora from '../pages/productClient/components/reportes/reporteGeneralMora/ReporteGeneralMora';
import ReportePazSalvo from '../pages/productClient/components/reportes/reportePazSalvo/ReportePazSalvo';
import ReporteCierreMensualContable from '../pages/contable/ReporteCierreMensualContable/ReporteCierreMensualContable';
import ReporteEstadoCuenta from '../pages/productClient/components/reportes/reporteEstadoCuenta/ReporteEstadoCuenta';
import ReporteMensualSeguros from '../pages/productClient/components/reportes/ReporteMensualSeguros/ReporteMensualSeguros';
import MovimientoCartera from '../pages/productClient/components/reportes/reporteMovimientoCartera/ReporteMovimientoCartera';
import ReporteCapitalCorriente from '../pages/productClient/components/reportes/ReporteCapitalCorriente/ReporteCapitalCorriente';
import ConsultaGarantias from '../pages/productClient/components/ConsultaGarantias';
import ReporteRCredito from '../pages/productClient/components/reportes/reporteRCredito/ReporteRCredito';
import RequestPropeccion from '../components/RequestInformationSimulationData/RequestProspeccion';
import AdjudicacionSubsidios from '../pages/subsidios/adjudicacionSubsidios/AjudicacionSubsidiosInicio';
import ConsultarComprobanteCredito from '../pages/contable/ConsultarComprobanteCredito';
import InformacionUnidadEjecutora from '../pages/TablasParametros/components/componentsUnidadEjecutoras/InformacionUnidadEjecutora';
import ProcedureEducationRequests from '../pages/ProcedureRequests/ProcedureEducationRequests';
import ReportesFondoSolidaridad from '../pages/fondoSolidaridad/reportesFondoSolidaridad/ReportesFondoSolidaridad';
import ReportesSolicitudes from '../pages/reportesSolicitudes/ReportesSolicitudes';
import ReporteCredito from '../pages/reportesCredito/Reportes';
import GenerarBalanceCartera from '../pages/contable/Reportes Balance/GenerarBalanceCartera';
import ConsultaReporteTU from '../pages/ConsultaReporteTU';
import ParametrizacionFinanciera from '../pages/parametrizacionesCreditoNuevos/ParametrizacionFinanciera';
import ParametrizacionDocumento from '../pages/parametrizacionesCreditoNuevos/ParametrizacionDocumento';
import ControlPeritos from '../components/controlPeritos/ControlPeritos';
import ControlAbogados from '../components/controlAbogados/ControlAbogados';
import ResportesCreditoAprobadoRechazado from '../pages/reportesCredito/ResportesCredito-aprobado-rechazado';


const PrivateRoute = ({ component: Component, ...rest }: any) => (
	<Route
		{...rest}
		render={props =>
			AuthService.isAuthenticated() ? (
				<Component {...props} />
			) : (
				<Redirect
					to={{ pathname: '/', state: { from: props.location } }}
				/>
			)
		}
	/>
);

function RouterAppRoot() {
	return (
		<Switch>
			<Route exact path="/login" component={LoginPage} />
			<Route exact path="/" component={LoginPage} />
			<Route
				exact
				path="/RecuperacionContrasena"
				component={RecuperacionContrasena}
			/>
			<Route
				exact
				path="/RestablecerContrasena/:token"
				component={RestablecerContrasena}
			/>
			<PrivateRoute
				component={() => (
					<Layout>
						<Route path="/Inicio" component={HomePage} />
						<Route path="/HojaDeVida" component={SearchInfo} />
						{/* <Route path="/AsignarTareas" component={AsignarTareas} /> */}
						<Route
							path="/InformacionCliente"
							component={Curriculum1}
						/>
						<Route
							path="/AdministracionCuentas"
							component={AdministracionCuentas}
						/>
						<Route
							path="/ConsultaReporteTU"
							component={ConsultaReporteTU}
						/>
						<Route
							path="/ConsultaPreaprobados"
							component={PreaprobacionesInicio}
						/>
						<Route
							path="/procedimientos-solicitudes"
							component={ProcedureRequests}
						/>
						<Route
							path="/procedimientos-solicitudes-educativa"
							component={ProcedureEducationRequests}
						/>
						<Route
							path="/reportes-credito-aprobados-rechazados"
							component={ReporteCredito}
						/>
						<Route
							path="/reportes-credito"
							component={ResportesCreditoAprobadoRechazado}
						/>
						<Route
							path="/Convocatorias"
							component={Convocatoria}
						/>
						<Route
							path="/Postulacion/:radicado?/:numero?"
							component={PostulacionInicio}
						/>
						<Route
							path="/Proyectos"
							component={Proyectos}
						/>
						<Route
							path="/SolicitudPostulacion"
							component={SolicitudPostulacion}
						/>
						<Route
							path="/ReporteRanking"
							component={ReporteRanking}
						/>
						<Route
							path="/ReportesFondoSolidaridad"
							component={ReportesFondoSolidaridad}
						/>
						<Route
							path="/legalizacion"
							component={RequestInformationLegalizationData}
						/>
						<Route path="/DespachosJudiciales" component={Court} />
						<Route path="/Embargos" component={Embargos} />
						<Route
							path="/EmbargosInicio"
							component={EmbargosInicio}
						/>
						<Route path="/Desembargo" component={Desembargo} />
						<Route path="/DesembargoInicio" component={DesembargoInicio} />
						<Route path="/InformacionDesembargo" component={InformacionDesembargos} />
						<Route path="/AnularEmbargo" component={AnularEmbargo} />
						<Route
							path="/DetalleEmbargosConcepto"
							component={DetalleEmbargosConcepto}
						/>
						<Route
							path="/DesembargosDeCuenta"
							component={Desembargo}
						/>
						<Route
							path="/PagoEmbargosInicio"
							component={PagoEmbargosInicio}
						/>
						<Route
							path="/InformacionPagoEmbargos"
							component={InformacionPagoEmbargos}
						/>
						<Route
							path="/ParametrizacionGeneral"
							component={GeneralParameterization}
						/>
						<Route path="/Simulador" component={Simulador} />
						<Route
							path="/ExcepcionSimuladorTipoActor"
							component={TipoActor}
						/>
						<Route
							path="/ExcepcionSimuladorUnidadEjecutora"
							component={UnidadEjecutora}
						/>
						<Route path="/ParametrosPlazos" component={Plazos} />
						<Route path="/ParametrosSeguros" component={Seguros} />
						<Route
							path="/ParametrizacionLegal"
							component={ParametrizationLegal}
						/>
						<Route
							path="/ParametrosLiquidacion"
							component={Liquidacion}
						/>
						<Route
							path="/ParametrizacionTipoDocumento"
							component={TipoDocumento}
						/>
						<Route
							path="/ParametrizacionGenero"
							component={Genero}
						/>
						<Route
							path="/ParametrizacionFuerzas"
							component={Fuerzas}
						/>
						<Route
							path="/ParametrizacionCategorias"
							component={Categorias}
						/>
						<Route
							path="/ParametrizacionLeasing"
							component={ParametrizacionLeasing}
						/>
						<Route
							path="/ParametrizacionFinanciera"
							component={ParametrizacionFinanciera}
						/>
						<Route
							path="/ParametrizacionDocumento"
							component={ParametrizacionDocumento}
						/>
						
						<Route path="/cargueNomina/:tarea?" 
						component={CargueNomina} />
						<Route path="/notify" component={Notify} />
						<Route path="/ProductData" component={ProductData} />
						<Route
							path="/AplicacionMasivaDePagos"
							component={MasivePayAplication}
						/>
						<Route
							path="/SeguroListas"
							component={SeguroListas}
						/>
						<Route
							path="/ControlPeritos"
							component={ControlPeritos}
						/>
						<Route
							path="/ControlAbogados"
							component={ControlAbogados}
						/>
						<Route
							path="/ContabilidadSeven"
							component={AplicacionPagoMasivo}
						/>
						<Route
							path="/Garantias"
							component={ConsultaGarantias}
						/>
						<Route path="/GenerarExtracto" component={GenerarExtracto} />
						<Route path="/ModuloEstado" component={State} />
						<Route path="/ModuloBloqueos" component={Bloqueos} />
						<Route
							path="/Notificaciones"
							component={Notifications}
						/>
						<Route
							path="/credit-committee"
							component={ConsultCreditCommittee}
						/>
						<Route path="/desembolsos" component={Disbursement} />
						<Route
							path="/DisbursementCreation"
							component={DisbursementCreation}
						/>
						<Route
							path="/AddProductData/:item?"
							component={AddProductData}
						/>
						<Route
							path="/NovedadesCredito"
							component={NovedadesCredito}
						/>
						<Route
							path="/CierreDiarioCartera"
							component={CierreDiarioCartera}
						/>
						<Route
							path="/CausacionDiariaInteresesCorrientes"
							component={CausacionDiariaInteresesCorrientes}
						/>
						<Route
							path="/CierreMensualCartera"
							component={CierreMensualCartera}
						/>
						<Route
							path="/CierreAnualCartera"
							component={CierreAnualCartera}
						/>
						<Route
							path="/CierresCausacionesRealizadas"
							component={CierresCausacionesRealizadas}
						/>
						<Route
							path="/InformacionContable"
							component={InformacionContable}
						/>

						{/* Reportes Productos Clientes */}

						<Route
							path="/ProductosClientes/Reportes/Pagos"
							component={ReportePagos}
						/>

						<Route
							path="/ProductosClientes/Reportes/GeneralCartera"
							component={ReporteGeneralCartera}
						/>

						<Route
							path="/ProductosClientes/Reportes/GeneralReestructuracion"
							component={ReporteGeneralReestructuracion}
						/>

						<Route
							path="/ProductosClientes/Reportes/GeneralGarantias"
							component={ReporteGeneralGarantia}
						/>

						<Route
							path="/ProductosClientes/Reportes/Mora"
							component={ReporteGeneralMora}
						/>

						<Route
							path="/ProductosClientes/Reportes/PazSalvo"
							component={ReportePazSalvo}
						/>

						<Route
							path="/ProductosClientes/Reportes/CertificadoIngresosRetenciones"
							component={CertificadoIngresosRetenciones}
						/>

						<Route
							path="/ProductosClientes/Reportes/APNRCAR"
							component={ReporteAPNRCAR}
						/>

						<Route
							path="/ProductosClientes/Reportes/ReporteMensualSeguros"
							component={ReporteMensualSeguros}
						/>

						<Route
							path="/ProductosClientes/Reportes/MovimientoCartera"
							component={MovimientoCartera}
						/>

						<Route
							path="/ProductosClientes/Reportes/ReporteCapitalCorriente"
							component={ReporteCapitalCorriente}
						/>

						<Route
							path="/ProductosClientes/Reportes/EstadoCuenta"
							component={ReporteEstadoCuenta}
						/>

						<Route
							path="/ProductosClientes/Reportes/RCredito"
							component={ReporteRCredito}
						/>
						
						{/* Extractos Plan Pagos */}
						<Route
							path="/ProductosClientes/ExtractosPlanPagos/Mensual"
							component={ExtractoPlanPagosMensual}
						/>
						<Route
							path="/ProductosClientes/ExtractosPlanPagos/Anual"
							component={ExtractoPlanPagosAnual}
						/>

						{/* Extractos Finacieros */}
						<Route
							path="/ProductosClientes/ExtractosFinancieros/Mensual"
							component={ExtractoMensual}
						/>
						<Route
							path="/ProductosClientes/ExtractosFinancieros/Anual"
							component={ExtractoAnual}
						/>

						{/* Reportes Contables */}
						<Route
							path="/CierresCartera/Reportes/CausacionContable"
							component={ReporteCausaciones}
						/>
						<Route
							path="/CierresCartera/Reportes/Pagos"
							component={ReporteContablePagos}
						/>
						<Route
							path="/CierresCartera/Reportes/CierreMensual"
							component={ReporteCierreMensualContable}
						/>
						<Route
							path="/Comprobante/GenerarComprobante"
							component={GenerarComprobanteContable}
						/>
						
						<Route
							path="/Comprobante/ConsultarComprobanteCartera"
							component={ConsultarComprobanteCartera}
						/>
						<Route
							path="/Comprobante/ConsultarComprobanteCredito"
							component={ConsultarComprobanteCredito}
						/>

						<Route
							path="/Comprobante/GenerarComprobanteIntegracion"
							component={GenerarComprobanteContableIntegracion}
						/>

						<Route
							path="/Reportes/GenerarBalanceCartera"
							component={GenerarBalanceCartera}
						/>
						<Route
							path="/GestionSolicitudes/:radicado?"
							component={SolicitudesInicio}
						/>
						<Route
							path="/GestionIntervinientes"
							component={IntervinienteInico}
						/>
						<Route 
							path="/Solicitudes" 
							component={Solicitudes} 
						/>
						<Route 
							path="/ReportesSolicitudes" 
							component={ReportesSolicitudes} 
						/>
						<Route
							path="/RetiroDesvinculacion"
							component={RetiroDesvinculacion}
						/>
						<Route
							path="/BeneficiarioPorFallecimientoDelAfiliado"
							component={BeneficiarioPorFallecimiento}
						/>
						<Route
							path="/MecanismosEspeciales"
							component={MecanismosEspeciales}
						/>
						<Route path="/Account" component={Account} />
						<Route
							path="/Cronograma"
							component={ScheduleNovelties}
						/>
						<Route
							path="/Matriculas"
							component={realStateRegistrations}
						/>
						<Route
							path="/MatriculasInmobiliarias"
							component={accordionRealStateRegistrations}
						/>
						<Route
							path="/DetalleActualizacionesInterviniente"
							component={DetalleActulizacionesInterviniente}
						/>
						<Route
							path="/AgregarNuevaMatricula"
							component={formMatricula}
						/>
						<Route
							path="/AgregarNuevoActor"
							component={AgregarNuevoActor}
						/>
						<Route
							path="/EditarHistorialPropietario"
							component={editHistory}
						/>
						<Route
							path="/InsertarNovedad"
							component={addNoveltyOperations}
						/>
						<Route path="/Usuarios" component={AdminUsuarios} />
						<Route
							path="/ActualizarUsuario"
							component={EditarUsuarios}
						/>
						<Route path="/CrearUsuario" component={CrearUsuario} />
						<Route
							path="/GrupoUsuarios"
							component={AdminGrupoUsuarios}
						/>
						<Route
							path="/CrearGrupoUsuarios"
							component={CrearGrupoUsuarios}
						/>
						<Route
							path="/ActualizarGrupoUsuario"
							component={ActualizarGrupoUsuarios}
						/>
						<Route
							path="/GrupoOpciones"
							component={AdminGrupoOpciones}
						/>
						<Route
							path="/CrearGrupoOpciones"
							component={CrearGrupoOpciones}
						/>
						<Route
							path="/ActualizarGrupoOpciones"
							component={ActualizaGrupoOpciones}
						/>
						<Route
							path="/EntidadesEducativas"
							component={educationalEntities}
						/>
						<Route
							path="/EntidadesEducativasDetalle/:idEntidad"
							component={accordionEducationalEntities}
						/>
						<Route
							path="/AgregarEntidadEducativa"
							component={AddEducationalEntities}
						/>
						<Route
							path="/InformacionBasicaEntidades"
							component={BasicInformationEntitiesComponent}
						/>
						<Route
							path="/programas"
							component={programsComponent}
						/>
						<Route path="/sedes" component={campusComponent} />
						<Route path="/bloqueos" component={BlocksEntities} />
						<Route path="/cuentas" component={AccountsComponent} />
						<Route
							path="/afiliaciones"
							component={affiliatesExtraordinary}
						/>
						<Route
							path="/reintegro"
							component={formularioReintegro}
						/>
						<Route
							path="/InsertarAfiliacion"
							component={InsertarAfiliacion}
						/>
						<Route
							path="/DetalleAportes"
							component={detalleAportes}
						/>
						<Route path="/Opciones" component={Opciones} />
						<Route
							path="/CrearOpciones"
							component={CrearOpciones}
						/>
						<Route path="/Auditoria" component={LogAuditoria} />
						<Route
							path="/DetalleAuditoria"
							component={DetalleAuditoria}
						/>
						<Route
							path="/ControlAdministracionCuentas"
							component={controlAdministracionCuentas}
						/>
						<Route
							path="/ReporteCuentaIndividual"
							component={ReporteCuentasIndividual}
						/>
						<Route
							path="/CierreMensual"
							component={CierreMensual}
						/>
						<Route
							path="/ReporteCierreMensual"
							component={ListarCierreMensual}
						/>
						<Route
							path="/ReintegrosUE"
							component={Reintegros}
						/>
						<Route
							path="/AdjudicacionSubsidios"
							component={AdjudicacionSubsidios}
						/>
						<Route
							path="/IPC"
							component={IPC}
						/>
						<Route
							path="/VerificacionDerechos"
							component={verificacionDerechos}
						/>
						<Route
							path="/verificacionDerechosDetalles"
							component={verificacionDerechosDetalles}
						/>
						<Route
							path="/abonoIntereses"
							component={AbonoIntereses}
						/>
						<Route
							path="/abonoInteresesDetalles"
							component={AbonoInteresesDetalles}
						/>
						<Route
							path="/ReportesGenerales"
							component={ReportsGenerales}
						/>
					
						<Route path="/ParametrosSeguroHogar" component={SeguroHogar} />
						<Route
							path="/TablasParametros"
							component={TablasParametros}
						/>
						<Route
							path="/InformacionUnidadEjecutora"
							component={InformacionUnidadEjecutora}
						/>
						<Route 
							path="/Gestion" 
							component={newsManagement} 
						/>
						<Route 
							path="/AgregarNovedad" 
							component={AddNew} 
						/>
						<Route 
							path="/EditarNovedad" 
							component={EditNew} 
						/>
						<Route 
							path="/DetalleNovedad" 
							component={SeeDetailNew} 
						/>
						<Route 
							path="/Consulta" 
							component={consultaNovedadesActivas}
						/>
						<Route 
							path="/ConsultaProspeccion" 
							component={RequestPropeccion}
						/>
						<Route
							path="/ConsultaNovedades" 
							component={consultaNovedades}
						/>
						<Route
							path="/ConsultaNovedadesHistorial" 
							component={consultaNovedadesHistorial}
						/>
						<Route
							path="/ReporteInconsistencias"
							component={ReporteSolucionInconsistencias}
						/>
						<Route
							path="/EditarNovedadDatos"
							component={EditNewInData}
						/>
						<Route
							path="/LogAuditoriaAdministraciónNovedades"
							component={RegistroNovedades}

						/>
						<Route
							path="/LogDespachosJudiciales"
							component={RegistroEmbargos}

						/>
						<Route
							path="/ReporteCuentasIndividuales"
							component={ReporteCuentasIndividual}

						/>
						<Route
							path="/ConsultaSaldosYCartera"
							component={ConsultaSaldos}
						/>
						<Route
							path="/ReportesGA2"
							component={ConsultaReportesGA2}

						/>
					</Layout>
				)}
			/>
		</Switch>
	);
}

export default RouterAppRoot;
