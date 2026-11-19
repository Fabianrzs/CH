import React, { useEffect, useState } from 'react';
import { createStyles, makeStyles, Tab, Tabs, Theme, withStyles } from '@material-ui/core';
import ResumenSolicitud from '../components/resumen-solicitud/resumen-solicitud.component';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import TrazabilidadSolicitud from '../components/trazabilidad/trazabilidad.component';
import DynamicPage from '../dynamicPage/DynamicPage.component';
import { FormulariosSolicitudDto } from '../../../models/requests/solicitud-tarea.model';

const TABS_INDEX = {
	RESUMENSOLICITUD: 'resumen',
	CARGUEDOCUMENTOS: 'carguededocumentosretiroparcial',
	CARGUEDOCUMENTOSV14: 'carguededocumentosv14',
	CARGUEDOCUMENTOSV8: 'carguededocumentosv8',
	CARGUEDOCUMENTOSDESAFILIACION: 'carguededocumentosdesafiliaciones',
	VALIDACIONDOCUMENTOS: 'validaciondedocumentos',
	CARGAARCHIVOS: 'cargaarchivos_2',
	VERARCHIVOS: 'verarchivos_2',
	FORMULARIO_INCONSISTENCIAS: 'formulario',
	VERDETALLESOLICITUD: 'Detalle Solicitud'
};

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
	permissions,
	responseSolicitud,
	siEsAprobadaSolcitud,
}: {
	currentTab: string;
	onTabChange: (event: React.ChangeEvent<any>, newValue: string) => void;
	permissions: string;
	responseSolicitud: ResponseSeveranceModel | undefined;
	siEsAprobadaSolcitud: boolean;
}) {

	return (
		<>
            <AntTabs
                value={currentTab}
                onChange={onTabChange}
                aria-label="ant example"
            >
                <AntTab label="Resumen" value={TABS_INDEX.RESUMENSOLICITUD} />
                <AntTab label="Detalle Solicitud" value={TABS_INDEX.VERDETALLESOLICITUD} />
            </AntTabs>
		</>
	);
}

function Panel({
	currentTabValue,
	responseSolicitud,
	observaciones,
    tipoAccion,
	siEsAprobadaSolcitud,
	formulariosSolicitud,
    tipoSolicitud,
    onItemGoBack,
}: {
	currentTabValue: string;
	responseSolicitud: ResponseSeveranceModel | undefined;
	observaciones: string;
    tipoAccion: number;
	siEsAprobadaSolcitud: boolean;
	formulariosSolicitud: FormulariosSolicitudDto | any;
    tipoSolicitud: any;
    onItemGoBack: () => void;
}) {

	let formularioPanel = null;

	if (currentTabValue === TABS_INDEX.RESUMENSOLICITUD) {
		formularioPanel = (
			<>
				<ResumenSolicitud 
					responseSolicitud={responseSolicitud} 
					observaciones={observaciones} 
				/>
				<TrazabilidadSolicitud
					responseSolicitud={responseSolicitud}
					observaciones={observaciones}
				/>
			</>
		);
	}
	else if (currentTabValue === TABS_INDEX.VERDETALLESOLICITUD) {
        const { formularios } = formulariosSolicitud;
		formularioPanel = (
			<DynamicPage
				dynamicTitle={{ topText: "", title: "", icon: "" }}
				formsList={formularios}
				withButtons
				responseSolicitud={responseSolicitud}
				tipoAccion={tipoAccion}
				tipoAccionDetalleProcesar={false}
				modalidadSolicitud={responseSolicitud?.timCodigo as string}
				onItemGoBack={onItemGoBack}
				onItemBack={() => { } }
				obtnerBoleano={undefined}
				inicioSolicitud={false}
				siEsAprobadaSolcitud={siEsAprobadaSolcitud}
				tipoSolicitud={tipoSolicitud} 
		    />
		);
	}
	return formularioPanel;
}

function ProcesoRadicacion({
	responseSolicitud,
	onItemGoBack,
	tipoAccion,
	tipoAccionDetalleProcesar,
	obtnerBoleano,
	siEsAprobadaSolcitud,
	setDeshalitarBotonesInconsistencia,
	formulariosSolicitud,
    tipoSolicitud,
}: {
	responseSolicitud: any | undefined;
	onItemGoBack: () => void;
	tipoAccion: number;
	tipoAccionDetalleProcesar: boolean;
	obtnerBoleano: any | undefined;
	siEsAprobadaSolcitud: boolean;
	setDeshalitarBotonesInconsistencia:any;
	formulariosSolicitud: FormulariosSolicitudDto | any;
    tipoSolicitud: any;
}) {

	const [currentTabValue, setCurrentTabValue] = useState(
		TABS_INDEX.RESUMENSOLICITUD,
	);
	const [observaciones, setObservaciones] = useState('');
    const [titulosIcon, setTitulosIcon] = useState({});

	const handleTabChange = (
		event: React.ChangeEvent<any>,
		newValue: string,
	) => {
		setCurrentTabValue(newValue);
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
		else if (responseSolicitud?.descripcionProceso === 'GIRO DE RECURSOS'){
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Solicitud de Giro de Recursos",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
		else if (responseSolicitud?.descripcionProceso === 'SUBSIDIO SECUESTRADOS'){
			pageTitleProps = {
				topText: "Solicitud",
				title1: "Subsidio Secuestrados ",
				icono: "home"
			};
			setTitulosIcon(pageTitleProps)
		}
	}, [responseSolicitud])

	return (
		<>
			<CHPageTitleV2   {...titulosIcon} />
			<ViewTabs
				currentTab={currentTabValue}
				onTabChange={handleTabChange}
				permissions={responseSolicitud?.pestanas as string}
				responseSolicitud={responseSolicitud}
				siEsAprobadaSolcitud={siEsAprobadaSolcitud}
			/>
			<Panel
				responseSolicitud={responseSolicitud}
				currentTabValue={currentTabValue}
                tipoAccion={tipoAccion}
				observaciones={observaciones as string}
				siEsAprobadaSolcitud={siEsAprobadaSolcitud}
				formulariosSolicitud={formulariosSolicitud}
                onItemGoBack={onItemGoBack}
                tipoSolicitud={tipoSolicitud}
			/>
		</>
	);
}
export default ProcesoRadicacion;
