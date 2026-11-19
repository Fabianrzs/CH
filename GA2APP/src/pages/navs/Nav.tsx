/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useEffect, useRef, useState } from 'react';
import { toast } from 'react-toastify';
import clsx from 'clsx';
import { RuleTester } from 'eslint';
import {
	createStyles,
	makeStyles,
	useTheme,
	Theme,
} from '@material-ui/core/styles';
import Drawer from '@material-ui/core/Drawer';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import List from '@material-ui/core/List';
import CssBaseline from '@material-ui/core/CssBaseline';
import Typography from '@material-ui/core/Typography';
import Divider from '@material-ui/core/Divider';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import ChevronRightIcon from '@material-ui/icons/ChevronRight';
import {
	AssignmentInd,
	BrightnessHigh,
	CardMembership,
	Home,
	Note,
	CloseOutlined,
	Today,
	TrendingUp,
	HomeWork,
	House,
	School,
	Gavel,
	FormatListBulleted,
	CreditCard,
	AccountCircle,
	FileCopy,
	AnnouncementOutlined,
	PersonTwoTone,
	AssessmentOutlined,
	AccountBalanceWalletOutlined,
	Receipt,
	CardGiftcard,
	PieChart,
	Lock,
	Search,
	AccountBalanceOutlined,
	Report,
	AccountBalance,
} from '@material-ui/icons';
import AttachmentIcon from '@material-ui/icons/Attachment';
import { Button } from '@material-ui/core';
import { useHistory } from 'react-router';
import { connect } from 'react-redux';
import { startGetNotification } from '../../actions/notification.action';
import LogoCH from '../../assets/images/LogoHorizontalCajaHonor.svg';
import { SideMenuItem } from '../../models/side-menu-item.model';
import CHSideMenuItem from '../../components/ch-side-menu-item/ch-side-menu-item';
import AuthService from '../../services/auth.service';
import { AppState } from '../../store/rootReducer';
import StorageService from '../../services/storage.service';
import { LoginState } from '../../reducers/login.reducer';
import { GrupoOpcionesModel } from '../../types/grupo-opciones.model.type';
import Api from '../../services/api.core.service';

const drawerWidth = 320;


const useStyles = makeStyles((theme: Theme) =>
	createStyles({
		root: {
			display: 'flex',
		},
		appBar: {
			zIndex: theme.zIndex.drawer + 1,
			transition: theme.transitions.create(['width', 'margin'], {
				easing: theme.transitions.easing.sharp,
				duration: theme.transitions.duration.leavingScreen,
			}),
		},
		appBarShift: {
			marginLeft: drawerWidth,
			width: `calc(100% - ${drawerWidth}px)`,
			transition: theme.transitions.create(['width', 'margin'], {
				easing: theme.transitions.easing.sharp,
				duration: theme.transitions.duration.enteringScreen,
			}),
		},
		menuButton: {
			marginRight: 36,
		},
		hide: {
			display: 'none',
		},
		logo: {
			flexGrow: 1,
		},
		drawer: {
			width: drawerWidth,
			flexShrink: 0,
			whiteSpace: 'nowrap',
		},
		drawerOpen: {
			width: drawerWidth,
			transition: theme.transitions.create('width', {
				easing: theme.transitions.easing.sharp,
				duration: theme.transitions.duration.enteringScreen,
			}),
		},
		drawerClose: {
			transition: theme.transitions.create('width', {
				easing: theme.transitions.easing.sharp,
				duration: theme.transitions.duration.leavingScreen,
			}),
			overflowX: 'hidden',
			width: theme.spacing(7) + 1,
			[theme.breakpoints.up('sm')]: {
				width: theme.spacing(9) + 1,
			},
		},
		toolbar: {
			display: 'flex',
			alignItems: 'left',
			justifyContent: 'left',
			padding: theme.spacing(0, 1),
			// necessary for content to be below app bar
			...theme.mixins.toolbar,
			background: '#1C3F8B',
			width: '55vh',
		},
		toolbarContent: {
			padding: theme.spacing(3),
		},
		content: {
			flexGrow: 1,
			padding: theme.spacing(3),
		},
		colorMenu: {
			color: 'white',
		},
		tamanoPanel: {
			height: '1000vh',
			background: '#1C3F8B',
			width: '55vh',
		},
		paddinBadge: {
			padding: theme.spacing(0, 1),
			cursor: 'pointer',
		},
		botonCerrarSesion: {
			color: 'white',
			fontSize: theme.spacing(2),
			fontWeight: theme.spacing(3),
		},
	}),
);

const menus: SideMenuItem[] = [
	{
		name: 'Inicio',
		icon: <Home />,
		link: '/Inicio',
		permission: '',
	},
	{
		name: 'Gestión De Afiliados',
		icon: <AssignmentInd />,
		link: '',
		permission: 'MENU_GESTION_AFILIADOS',
		subMenus: [
			{
				name: 'Hoja de Vida',
				icon: <Note />,
				link: '/HojaDeVida',
				permission: 'SUB_MENU_HOJA_DE_VIDA',
			},

			{
				name: 'Afiliaciones Extraordinarias',
				icon: <Note />,
				link: '/afiliaciones',
				permission: 'SUB_MENU_AFILIACIONES_EXTRAORDINARIAS',
			},
		],
	},
	{
		name: 'Cierre Mensual',
		icon: <Today />,
		link: '',
		permission: 'MENU_CIERRE_MENSUAL',
		subMenus: [
			{
				name: 'Ejecutar cierre mensual',
				icon: <Today />,
				link: '/CierreMensual',
				permission: 'SUB_MENU_EJECUTAR_CIERRE_MENSUAL',
			},

			{
				name: 'Reporte cierre mensual',
				icon: <Today />,
				link: '/ReporteCierreMensual',
				permission: 'SUB_MENU_REPORTE_CIERRE_MENSUAL',
			},
		],
	},
	{
		name: 'Entidades Educativas',
		icon: <School />,
		link: '/EntidadesEducativas',
		permission: 'MENU_ENTIDADES_EDUCATIVAS',
	},

	{
		name: 'Consulta Reporte TU',
		icon: <Report />,
		link: '/ConsultaReporteTU',
		permission: '',
	},
	{
		name: 'Administración de Novedades', 
		icon: <AnnouncementOutlined />,
		link: '',
		permission: 'MENU_ADMINISTRACION_NOVEDADES',
		subMenus: [
			{
				name: 'Cronograma de Novedades',
				icon: <AnnouncementOutlined />,
				link: '/Cronograma',
				permission: 'SUB_MENU_CRONOGRAMA_DE_NOVEDADES',
			},
			{
				name: 'Gestión de Novedades',
				icon: <AnnouncementOutlined />,
				link: '/Gestion',
				permission: 'SUB_MENU_GESTION_DE_NOVEDADES',
			},
			{
				name: 'Consulta de Novedades Activas',
				icon: <AnnouncementOutlined />,
				link: '/Consulta',
				permission: 'SUB_MENU_CONSULTA_DE_NOVEDADES_ACTIVAS',
			},
			{
				name: 'Reporte de Solución de Inconsistencias',
				icon: <AnnouncementOutlined />,
				link: '/ReporteInconsistencias',
				permission: 'SUB_MENU_CONSULTA_DE_NOVEDADES_ACTIVAS',
			},
			{
				name: 'Log de Auditoria Administración de Novedades',
				icon: <AnnouncementOutlined />,
				link: '/LogAuditoriaAdministraciónNovedades',
				permission: 'SUB_MENU_ACTUALIZACION_DE_NOVEDADES',
			},
		],
	},

	{
		name: 'Gestión de matrículas inmobiliarias',
		icon: <HomeWork />,
		link: '/Matriculas',
		permission: 'MENU_MATRICULAS_INMOBILIARIAS',
	},
	{
		name: 'Gestión Embargos',
		icon: <Gavel />,
		link: 'EmbargoConcepto',
		permission: 'MENU_GESTION_EMBARGOS',
		subMenus: [
			{
				name: 'Despachos Judiciales',
				icon: <AttachmentIcon />,
				link: '/DespachosJudiciales',
				permission: 'SUB_MENU_DESPACHOS_JUDICIALES',
			},
			{
				name: 'Embargos',
				icon: <AttachmentIcon />,
				link: '/EmbargosInicio',
				permission: 'SUB_MENU_EMBARGOS',
			},
			{
				name: 'Desembargos',
				icon: <AttachmentIcon />,
				link: '/DesembargoInicio',
				permission: 'SUB_MENU_DESEMBARGOS',
			},
			{
				name: 'Pago de Embargos',
				icon: <AttachmentIcon />,
				link: '/PagoEmbargosInicio',
				permission: 'SUB_MENU_PAGO_DE_EMBARGOS',
			},
		],
	},
	{
		name: 'Parametrización de crédito',
		icon: <CardMembership />,
		link: '',
		permission: 'MENU_PARAMERIZACION_CREDITO',
		subMenus: [
			{
				name: 'Parametrización General',
				icon: <TrendingUp />,
				link: '/ParametrizacionGeneral',
				permission: 'SUB_MENU_PARAMETRIZACION_GENERAL',
			},
			{
				name: 'Parámetros crédito',
				icon: <TrendingUp />,
				link: '/Simulador',
				permission: 'SUB_MENU_PARAMETROS_CREDITO',
			},
			{
				name: 'Excepciones tipo afiliado',
				icon: <TrendingUp />,
				link: '/ExcepcionSimuladorTipoActor',
				permission: 'SUB_MENU_EXCEPCIONES_TIPO_AFILIADO',
			},
			{
				name: 'Excepciones Unidad Ejecutora',
				icon: <TrendingUp />,
				link: '/ExcepcionSimuladorUnidadEjecutora',
				permission: 'SUB_MENU_EXCEPCIONES_UNIDAD_EJECUTORA',
			},
			{
				name: 'Parametrización de plazos',
				icon: <TrendingUp />,
				link: '/ParametrosPlazos',
				permission: 'SUB_MENU_PARAMETRIZACION_DE_PLAZOS',
			},
			{
				name: 'Parametrización de seguros',
				icon: <TrendingUp />,
				link: '/ParametrosSeguros',
				permission: 'SUB_MENU_PARAMETRIZACION_DE_SEGUROS',
			},
			{
				name: 'Parametrización Legal',
				icon: <TrendingUp />,
				link: '/ParametrizacionLegal',
				permission: 'SUB_MENU_PARAMETRIZACION_LEGAL',
			},
			{
				name: 'Parametrización de Liquidación',
				icon: <TrendingUp />,
				link: '/ParametrosLiquidacion',
				permission: 'SUB_MENU_PARAMETRIZACION_DE_LIQUIDACION',
			},
			{
				name: 'Parámetrización Tipos Documento',
				icon: <TrendingUp />,
				link: '/ParametrizacionTipoDocumento',
				permission: 'SUB_MENU_PARAMETRIZACION_TIPOS_DOCUMENTOS',
			},
			{
				name: 'Parametrización Géneros',
				icon: <TrendingUp />,
				link: '/ParametrizacionGenero',
				permission: 'SUB_MENU_PARAMETRIZACION_GENEROS',
			},
			{
				name: 'Parametrización Fuerzas',
				icon: <TrendingUp />,
				link: '/ParametrizacionFuerzas',
				permission: 'SUB_MENU_PARAMETRIZACION_FUERZAS',
			},
			{
				name: 'Parametrización Categorías',
				icon: <TrendingUp />,
				link: '/ParametrizacionCategorias',
				permission: 'SUB_MENU_PARAMETRIZACION_CATEGORIAS',
			},
			{
				name: 'Parametrización Seguro Hogar',
				icon: <TrendingUp />,
				link: '/ParametrosSeguroHogar',
				permission: 'SUB_MENU_PARAMETRIZACION_SEGURO_HOGAR',
			},
			{
				name: 'Tablas parámetros',
				icon: <TrendingUp />,
				link: '/TablasParametros',
				permission: 'SUB_MENU_TABLAS_PARAMETROS',
			},
			{
				name: 'Parametrización Salarial',
				icon: <TrendingUp />,
				link: '/ParametrizacionLeasing',
				permission: 'SUB_MENU_PARAMETRIZACION_SALARIAL',
			},
			{
				name: 'Parametrización Financiera',
				icon: <TrendingUp />,
				link: '/ParametrizacionFinanciera',
				permission: '',
			},
			{
				name: 'Parametrización Documentos',
				icon: <TrendingUp />,
				link: '/ParametrizacionDocumento',
				permission: '',
			},
		],
	},
	{
		name: 'Módulo Estado',
		icon: <MenuIcon />,
		link: '',
		permission: 'MENU_MODULO_ESTADO',
		subMenus: [
			{
				name: 'Módulo Estado',
				icon: <AttachmentIcon />,
				link: '/ModuloEstado',
				permission: 'SUB_MENU_MODULO_ESTADO',
			},
		],
	},
	{
		name: 'Módulo Bloqueos',
		icon: <MenuIcon />,
		link: '',
		permission: 'MENU_MODULO_BLOQUEOS',
		subMenus: [
			{
				name: 'Módulo Bloqueos',
				icon: <AttachmentIcon />,
				link: '/ModuloBloqueos',
				permission: 'SUB_MENU_MODULO_BLOQUEOS',
			},
		],
	},
	{
		name: 'Consulta Preaprobados',
		icon: <CardMembership />,
		link: '/ConsultaPreaprobados',
		permission: 'MENU_CONSULTA_PREAPROBADOS',
	},
	{
		name: 'Consulta Solicitudes',
		icon: <CreditCard />,
		link: '',
		permission: 'MENU_CONSULTA_SOLICITUDES',
		subMenus: [
			{
				name: 'Consultar Hipotecario/Leasing',
				icon: <AccountBalance />,
				link: '/procedimientos-solicitudes',
				permission:'SUB_MENU_SOLICITUDES_HIPOTECARIO_LEASING'
				
			},
			
		]
	},

	{

		name: 'Consulta Reportes Crédito',
		icon: <CreditCard />,
		link: '',
		permission: '',
		subMenus: [
			{
				name: 'Reportes de Crédito',
				icon: <CardMembership />,
				link: '/reportes-credito',
				permission: 'REPORTES_CREDITO',
			},
			{
				name: 'Créditos aprobados/rechazados',
				icon: <CardMembership />,
				link: '/reportes-credito-aprobados-rechazados',
				permission: '',
			},
		]
	},
	{
		name: 'Reclasificación de Clientes',
		icon: <CardMembership />,
		link: '/ConsultaProspeccion',
		permission: 'MENU_CONSULTA_PROSPECCION',
	},

	{
		name: 'Fondo Solidaridad',
		icon: <PersonTwoTone />,
		link: '',
		permission: 'FONDO_DE_SOLIDARIDAD',
		subMenus: [
			{
				name: 'Convocatorias',
				icon: <AttachmentIcon />,
				link: '/Convocatorias',
				permission: 'SUB_MENU_CONVOCATORIAS',
			},
			{
				name: 'Postulación',
				icon: <AttachmentIcon />,
				link: '/Postulacion',
				permission: 'SUB_MENU_POSTULACION',
			},
			{
				name: 'Proyectos',
				icon: <AttachmentIcon />,
				link: '/Proyectos',
				permission: 'SUB_MENU_PROYECTOS',
			},
			{
				name: 'Reportes FS',
				icon: <AttachmentIcon />,
				link: '/ReportesFondoSolidaridad',
				permission: 'SUB_MENU_POSTULACION',
			},
		],
	},

	{
		name: 'Cargue de Nómina',
		icon: <AssignmentInd />,
		link: '/CargueNomina',
		permission: 'MENU_CARGUE_NOMINA',
	},
	{
		name: 'Consulta Comité Crédito',
		icon: <AssignmentInd />,
		link: '/credit-committee',
		permission: 'MENU_CONSULTA_COMITE_CREDITO',
	},

	{
		name: 'Desembolsos',
		icon: <CardMembership />,
		link: 'desembolsos',
		permission: 'MENU_DESEMBOLSO',
	},
	{
		name: 'Productos Clientes',
		icon: <MenuIcon />,
		link: '',
		permission: 'MENU_PRODUCTOS_CLIENTES',
		subMenus: [
			{
				name: 'Agregar Producto',
				icon: <Note />,
				link: '/AddProductData',
				permission: 'SUB_MENU_AGREGAR_PRODUCTO',
			},
			{
				name: 'Consulta Cartera',
				icon: <CardMembership />,
				link: '/ProductData',
				permission: 'SUB_MENU_CONSULTA_CARTERA',
			},
			{
				name: 'Generar listas',
				icon: <CreditCard />,
				link: '/AplicacionMasivaDePagos',
				permission: 'SUB_MENU_APLICACION_DE_PAGOS',
			},
			{
				name: 'Actualización Seguros',
				icon: <CreditCard />,
				link: '/SeguroListas',
				permission: 'SUB_MENU_APLICACION_DE_PAGOS',
			},
			{
				name: 'Contabilidad Seven',
				icon: <CreditCard />,
				link: '/ContabilidadSeven',
				permission: 'SUB_MENU_CONTAVILIDAD_SEVEN'
			},

			
			{
				name: 'Garantias',
				icon: <CreditCard />,
				link: '/Garantias',
				permission: 'SUB_MENU_GARANTIAS'
			},
			{
				name: 'Reportes',
				icon: <FormatListBulleted />,
				link: '',
				permission: 'SUB_MENU_REPORTES',
				subMenus: [
					{
						name: 'Certificado de ingresos y retenciones',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/CertificadoIngresosRetenciones'
					},
					{
						name: 'Reporte de pagos',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/Pagos'
					},
					{
						name: 'Reporte general de mora',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/Mora'
					},
					{
						name: 'Reporte general de cartera',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/GeneralCartera'
					},
					{
						name: 'Reporte general de reestructuración',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/GeneralReestructuracion'
					},
					{
						name: 'Reporte general de garantías',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/GeneralGarantias'
					},
					{
						name: 'Reporte paz y salvo',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/PazSalvo'
					},
					{
						name: 'Reporte mensual de seguros',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/ReporteMensualSeguros'
					},
					{
						name: 'Movimiento cartera',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/MovimientoCartera'
					},
					{
						name: 'Reporte capital corriente',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/ReporteCapitalCorriente'
					},
					{
						name: 'Reporte APNRCAR',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/APNRCAR'
					},
					{
						name: 'Estado de cuenta',
						icon: <Note />,
						link: '/ProductosClientes/Reportes/EstadoCuenta'
					},
				]
			},
			{
				name: 'Generación Plan de Pagos',
				icon: <FormatListBulleted />,
				link: '',
				permission: 'SUB_MENU_EXTRACTOS_PLAN_PAGOS',
				subMenus: [
					{
						name: 'Extracto mensual',
						icon: <Note />,
						link: '/ProductosClientes/ExtractosPlanPagos/Mensual'
					},
					{
						name: 'Extracto anual',
						icon: <Note />,
						link: '/ProductosClientes/ExtractosPlanPagos/Anual'
					}
				]
			},
			{
				name: 'Extractos Financieros',
				icon: <FormatListBulleted />,
				link: '',
				permission: 'SUB_MENU_EXTRACTOS_FINANCIEROS',
				subMenus: [
					{
						name: 'Extracto mensual',
						icon: <Note />,
						link: '/ProductosClientes/ExtractosFinancieros/Mensual'
					},
					{
						name: 'Extracto anual',
						icon: <Note />,
						link: '/ProductosClientes/ExtractosFinancieros/Anual'
					}
				]
			},
			{
				name: 'Novedades de Crédito',
				icon: <FormatListBulleted />,
				link: '/NovedadesCredito',
				permission: 'SUB_MENU_NOVEDADES_DE_CREDITO',
			},
		],
	},
	{
		name: 'Control Peritos',
		icon: <PersonTwoTone />,
		link: '/ControlPeritos',
		permission: 'REPORTE_PERITO'
	},
	{
		name: 'Control Abogados',
		icon: <PersonTwoTone />,
		link: '/ControlAbogados',
		permission: 'REPORTE_ABOGADO'
	},
	{
		name: 'Cierres Cartera',
		icon: <FileCopy />,
		link: 'CierresCartera',
		permission: 'MENU_CIERRES_CARTERA',
		subMenus: [
			{
				name: 'Diario int. moratorio',
				icon: <Lock  />,
				link: '/CierreDiarioCartera',
				permission: 'SUB_MENU_DIARIO_INT_MORATORIO',
			},
			{
				name: 'Diario int. corrientes',
				icon: <Lock  />,
				link: '/CausacionDiariaInteresesCorrientes',
				permission: 'SUB_MENU_DIARIO_INT_CORRIENTES',
			},
			{
				name: 'Cierres realizados',
				icon: <Lock />,
				link: '/CierresCausacionesRealizadas',
				permission: 'SUB_MENU_CIERRES_REALIZADOS',
			},
			{
				name: 'Cierre Mensual',
				icon: <Lock />,
				link: '/CierreMensualCartera',
				permission: 'SUB_MENU_CIERRE_MENSUAL',
			},
			{
				name: 'Cierre Anual',
				icon: <Lock />,
				link: '/CierreAnualCartera',
				permission: 'SUB_MENU_CIERRE_ANUAL',
			},
			{
				name: 'Saldo de Cartera',
				icon: <PieChart  />,
				link: '/ConsultaSaldosYCartera',
				permission: 'SUB_MENU_INFORMACION_CONTABLE',
			},
			{
				name: 'Reporte contables',
				icon: <AssessmentOutlined  />,
				link: '',
				permission: 'SUB_MENU_REPORTE_CONTABLES',
				subMenus: [
					{
						name: 'Reporte causación contable',
						icon: <Note  />,
						link: '/CierresCartera/Reportes/CausacionContable'
					},
					{
						name: 'Reporte contable de pagos',
						icon: <Note />,
						link: '/CierresCartera/Reportes/Pagos'
					},
					{
						name: 'Reporte de cierre mensual',
						icon: <Note />,
						link: '/CierresCartera/Reportes/CierreMensual'
					}
				]
			},
		],
	},

	{
		name: 'Contabilidad Cartera',
		icon: <AccountBalanceWalletOutlined />,
		link: '',
		permission: 'SUB_MENU_REPORTE_CONTABLES',
		subMenus: [
			{
				name: 'Comprobantes',
				icon: <Receipt />,
				link: '',
				permission: 'SUB_MENU_COMPROBANTES_CONTABLES',
				subMenus: [
					{
						name: 'Comprobantes Cartera',
						icon: <Search   />,
						link: '/Comprobante/ConsultarComprobanteCartera'
					},
					{
						name: 'Comprobantes Credito',
						icon: <Search   />,
						link: '/Comprobante/ConsultarComprobanteCredito'
					},
					{
						name: 'Crear de comprobantes',
						icon: <Note  />,
						link: '/Comprobante/GenerarComprobante'
					},
					
					{
						name: 'Crear de comprobante integeción',
						icon: <Note  />,
						link: '/Comprobante/GenerarComprobanteIntegracion'
					},
				]
			},
			{
				name: 'Reportes',
				icon: <Receipt />,
				link: '',
				permission: 'SUB_MENU_BALANCE',
				subMenus: [
					{
						name: 'Balance',
						icon: <AccountBalanceOutlined/>,
						link: '/Reportes/GenerarBalanceCartera'
					},

				]
			}
		]
	},
	{
		name: 'Solicitudes',
		icon: <BrightnessHigh />,
		link: '',
		permission: 'MENU_SOLICITUDES',
		subMenus: [
			{
				name: 'Consultar/Crear Solicitud',
				icon: <CardMembership />,
				link: '/GestionSolicitudes',
				permission:'SUB_MENU_SOLICITUDES_RADICACION'
				
			},
			{
				name: 'Consulta de intervinientes',
				icon: <CardMembership />,
				link: '/GestionIntervinientes',
				permission: 'SUB_MENU_SOLICITUDES_INTERVINIENTES'
				
			},
		]
	},
	{
		name: 'Reportes Solicitudes',
		icon:  <FileCopy />,
		link: '/ReportesSolicitudes',
		permission: 'MENU_SOLICITUDES',
	},
	{
		name: 'Administración Cuentas',
		icon: <AccountCircle />,
		link: '',
		permission: 'MENU_ADMINISTRACION_CUENTAS',
		subMenus: [
			{
				name: 'Búsqueda de Cuentas',
				icon: <Note />,
				link: '/AdministracionCuentas',
				permission: 'SUB_MENU_BUSQUEDA_DE_CUENTAS',
			},
			{
				name: 'Reportes Cuentas Individuales',
				icon: <Note />,
				link: '/ReporteCuentaIndividual',
				permission: 'SUB_MENU_REPORTES_CUENTAS_INDIVIDUAL',
			},
			{
				name: 'Reintegros Unidades Ejecutoras',
				icon: <Note />,
				link: '/ReintegrosUE',
				permission: 'SUB_MENU_REINTEGRO_UNIDADES_EJECUTORAS',
			},
			{
				name: 'Registro I.P.C',
				icon: <Note />,
				link: '/IPC',
				permission: 'SUB_MENU_REINTEGRO_I.P.C',
			},
			{
				name: 'Verificación de Derechos',
				icon: <Note />,
				link: '/verificacionDerechos',
				permission: 'SUB_MENU_VERIFICACION_DE_DERECHOS',
			},
			{
				name: 'Abono de Intereses',
				icon: <Note />,
				link: '/AbonoIntereses',
				permission: 'SUB_MENU_ABONO_DE_INTERESES',
			},
		],
	},
	{
		name: 'Subsidios',
		icon: <CardGiftcard />,
		link: '',
		permission: 'FONDO_DE_SOLIDARIDAD',
		subMenus: [
			{
				name: 'Adjudicación de Subsidios',
				icon: <House />,
				link: '/AdjudicacionSubsidios',
				permission: 'SUB_MENU_CONVOCATORIAS',
			},
		],
	},
	{
		name: 'Roles y Perfiles',
		icon: <AssignmentInd />,
		link: '',
		permission: 'MENU_ROLES_PERFILES',
		subMenus: [
			{
				name: 'Administración de Usuarios',
				icon: <Note />,
				link: '/Usuarios',
				permission: 'SUB_MENU_ADMINISTRACION_DE_USUARIOS',
			},
			{
				name: 'Grupo de Usuarios',
				icon: <Note />,
				link: '/GrupoUsuarios',
				permission: 'SUB_MENU_GRUPO_DE_USUARIOS',
			},
			{
				name: 'Grupo de Opciones',
				icon: <Note />,
				link: '/GrupoOpciones',
				permission: 'SUB_MENU_GRUPO_DE_OPCIONES',
			},
			{
				name: 'Opciones',
				icon: <Note />,
				link: '/Opciones',
				permission: 'SUB_MENU_OPCIONES',
			},
		],
	},
	{
		name: 'Auditoría',
		icon: <Today />,
		link: '/Auditoria',
		permission: 'MENU_AUDITORIA',
	},
	{
		name: 'Reportes SFC',
		icon: <FileCopy />,
		link: '/ReportesGenerales',
		permission: 'MENU_REPORTES_GENERALES',
	},
	{
		name: 'Reportes GA2',
		icon: <FileCopy />,
		link: '/ReportesGA2',
		permission: 'MENU_REPORTES_GENERALES',
	},
	{
		name: 'Cerrar sesión',
		icon: <CloseOutlined />,
		link: '/Account',
		permission: '',
	},
];

const api = Api();

const MiniDrawer = (props: any) => {
	const { pageshow, notificationState, startGetNotification, loginState } =
		props;

	const classes = useStyles();
	const theme = useTheme();
	const [open, setOpen] = useState(false);
	const [itemOpen, setItemOpen] = useState<number | null>(null);
	const history = useHistory();
	const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

	const [opciones] = useState<GrupoOpcionesModel[]>(loginState.opciones);

	const [printedMenu, setPrintedMenu] = useState<SideMenuItem[]>([])

	useEffect(() => {
		
		menus.forEach((element) => {
			renderAuthButton(element)
		})
	}, [])

	useEffect(() => {
		const updateSession = async () => {
			try {
				const user = AuthService.getUserInfo();
				api.UpdateSession(user.USR_ID)
				.then(response => {
					if(!response.data){
						const user = AuthService.getUserInfo();
						api.Logout(user.USR_ID)
						.then(() => {
							window.localStorage.clear()
							StorageService.removeStorageFromLogout();
							history.push('/');
						}) 
					}
				});
			}
			catch (error) {
				console.error('Error making API call:', error);
			}
		}

		setInterval(updateSession, 60000000)
	}, [])

	useEffect(() => {
        const handleBeforeUnload = () => {
            const user = AuthService.getUserInfo();
            if (!user || !user.USR_ID) return; 
            const data = JSON.stringify({ userId: user.USR_ID });
            navigator.sendBeacon(data);
        };
        window.addEventListener("beforeunload", handleBeforeUnload);
        return () => {
            window.removeEventListener("beforeunload", handleBeforeUnload);
        };
    }, []);
	 
	  

	  useEffect(() => {
		const storedToast = localStorage.getItem('toastMessage'); 
		if(storedToast !== null){
			toast.dismiss(storedToast || undefined);
		}
	}, [window.location.href]);

	const handleClickMenu = (event: React.MouseEvent<HTMLButtonElement>) => {
		setAnchorEl(event.currentTarget);
	};

	const nombreUserLogin = (): string => {
		
		const user = AuthService.getUserInfo();
		return `${user.USR_NOMBRE} ${user.USR_APELLIDO}`;
	};

	const handleDrawerOpen = () => {
		
		setOpen(true);
	};

	const handleDrawerClose = () => {
		setOpen(false);
		setItemOpen(null);
	};

	const renderAuthButton = (menu: SideMenuItem) => {
		const isAdmin = opciones.findIndex(x => x.codigo === 'SUPER_ADMIN') >= 0;
		const selectedMenu =  opciones.find(x => x.codigo === menu.permission)
		if(!isAdmin && selectedMenu !=  null) {
			const newMenu = {
				name: menu.name,
				icon: menu.icon,
				link: menu.link,
				permission: menu.permission, 
				subMenus: menu.subMenus?.filter(x => selectedMenu?.subOpciones?.findIndex(y => x.permission === y.codigo) >= 0)
			};
			setPrintedMenu(x => [...x, newMenu])
		} else if((!isAdmin && selectedMenu != null) || menu.name === 'Inicio' || menu.name === 'Cerrar sesión'){
			setPrintedMenu(x => [...x, menu])
		} else if (isAdmin){
			setPrintedMenu(x => [...x, menu])
		}

	};

	return (
		<div className={classes.root}>
			<CssBaseline />
			<AppBar
				position="fixed"
				className={clsx(classes.appBar, {
					[classes.appBarShift]: open,
				})}
			>
				<Toolbar>
					<IconButton
						color="inherit"
						aria-label="open drawer"
						onClick={handleDrawerOpen}
						edge="start"
						className={clsx(classes.menuButton, {
							[classes.hide]: open,
						})}
					>
						<MenuIcon />
					</IconButton>
					<div>
						<Button
							aria-controls="nombre-login"
							aria-haspopup="true"
							onClick={handleClickMenu}
							className={classes.botonCerrarSesion}
						>
							{nombreUserLogin()}
						</Button>
					</div>
					<Typography variant="h6" noWrap className={classes.logo} />
					<a href="/Inicio">
						<img src={LogoCH} alt="" />
					</a>
				</Toolbar>
			</AppBar>
			<Drawer
				variant="permanent"
				className={clsx(classes.drawer, {
					[classes.drawerOpen]: open,
					[classes.drawerClose]: !open,
				})}
				classes={{
					paper: clsx({
						[classes.drawerOpen]: open,
						[classes.drawerClose]: !open,
					}),
				}}
			>
				<div className={classes.toolbar}>
					<IconButton onClick={handleDrawerClose}>
						{theme.direction === 'rtl' ? (
							<ChevronRightIcon />
						) : (
							<ChevronLeftIcon />
						)}
					</IconButton>
				</div>
				<Divider />
				<div className={classes.tamanoPanel}>
					<List>
						{printedMenu.map(
							(menu, key) =>
								(
									<CHSideMenuItem
										isOpen={itemOpen === key}
										setOpen={setItemOpen}
										id={key}
										onRedirect={handleDrawerClose}
										menu={menu}
										key={`menu-${key}`}
									/>
								),
						)}
					</List>
				</div>
			</Drawer>
			<main className={classes.content}>
				<div className={classes.toolbarContent} />
				{pageshow}
			</main>
		</div>
	);
};

const mapStateToProps = (state: AppState) => ({
	loginState: state.login,
	notificationState: state.notification,
});

interface LinkMapDispatchToProps {
	startGetNotification: () => void;
}

const mapDispatchToProps = {
	startGetNotification,
};

interface LinkMapStateToProps {
	loginState: LoginState;
}

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(MiniDrawer);
