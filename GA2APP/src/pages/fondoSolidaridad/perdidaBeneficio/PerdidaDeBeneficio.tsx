import {Button, 
    createStyles, 
    Grid, 
    makeStyles, 
    Tab, 
    Tabs, 
    Theme, 
    withStyles } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { TABS_INDEX_PERDIDA_BENEFICIO } from '../constants/fondoSolidaridad.constants';
import ResumenPostulacion from '../postulacion/ResumenPostulacion';
import InformacionPerdidaBeneficio from './InformacionPerdidaBeneficio';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';

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
             value={TABS_INDEX_PERDIDA_BENEFICIO.RESUMEN}
         />
       
         <AntTab
             label="Pérdida Beneficio"
             value={TABS_INDEX_PERDIDA_BENEFICIO.PERDIDA_BENEFICIO}
         />
         </AntTabs>
 );
}

function Panel ({
 currentTabValue,
 cliente,
 catalogState,
 datosSolicitudPostulacion,
 tipoSolicitud,
 formularioPostulacion,
 formularioResumenInformacionSolicitud,
 formularioObservacionCargueArchivos,
 formularioInformacionPerdidaBeneficio
}:{
 currentTabValue: number;
 cliente: any;
 catalogState: CatalogState;
 datosSolicitudPostulacion: any;
 tipoSolicitud: number;
 formularioPostulacion: any;
 formularioResumenInformacionSolicitud: any;
 formularioObservacionCargueArchivos: any;
 formularioInformacionPerdidaBeneficio: any;
}){
 switch (currentTabValue) {
     case TABS_INDEX_PERDIDA_BENEFICIO.RESUMEN:
         return (
             <>
                 <ResumenPostulacion 
                     cliente={cliente}
                     catalogState={catalogState}
                     tipoSolicitud={tipoSolicitud}
                     formularioPostulacion={formularioPostulacion}
                     datosSolicitudPostulacion={datosSolicitudPostulacion} 
                     formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud} 
                     formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
                 />
             </>
         );
     case TABS_INDEX_PERDIDA_BENEFICIO.PERDIDA_BENEFICIO:
         return (
             <>
                 <InformacionPerdidaBeneficio
                     formularioInformacionPerdidaBeneficio={formularioInformacionPerdidaBeneficio}
                 />
             </>
         );
     default:
         return null;
 }
}

const PerdidaDeBeneficio = (props: Props) => {

 const classes = useStyles();
 const apiFondoSolidaridad = ApiFondoSolidaridad();
 const {cliente,
        catalogState,
        datosSolicitudPostulacion,
        tipoSolicitud,
        formularioPostulacion,
        formularioResumenInformacionSolicitud,
        formularioObservacionCargueArchivos,
        regresarPantallaInicial} = props;
 const [currentTabValue, setCurrentTabValue] = useState(TABS_INDEX_PERDIDA_BENEFICIO.RESUMEN);

 const formularioInformacionPerdidaBeneficio = useForm({
     defaultValues: {
         valorAjustar: '',
         radicado: '',
         numeroResolucion: '',
         fechaEjecutoria: '',
         conceptoResolucion: ''
     }
 })

 useEffect(() => {
     consultarObservacionCargueArchivo(datosSolicitudPostulacion?.idSolicitud);
     formularioInformacionPerdidaBeneficio.setValue('radicado',datosSolicitudPostulacion?.noRadicado);
 }, [datosSolicitudPostulacion]);

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

 const handleTabChange = (
     event: React.ChangeEvent<any>,
     newValue: number,
 ) => {
     setCurrentTabValue(newValue);
 };

 return(
     <>
         <CHPageTitleV2
             topText="Gestión Pérdida De Beneficio"
             title1="Pérdida De Beneficio"
             title2='Fondo Solidaridad'
             icono="fact_check"
         />
         <ViewTabs
             currentTab={currentTabValue}
             onTabChange={handleTabChange}
         />
         <Panel
             currentTabValue={currentTabValue}
             cliente={cliente}
             catalogState={catalogState}
             datosSolicitudPostulacion={datosSolicitudPostulacion}
             tipoSolicitud={tipoSolicitud}
             formularioPostulacion={formularioPostulacion}
             formularioResumenInformacionSolicitud={formularioResumenInformacionSolicitud}
             formularioObservacionCargueArchivos={formularioObservacionCargueArchivos}
             formularioInformacionPerdidaBeneficio={formularioInformacionPerdidaBeneficio}
         />
         <Grid container>
             <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                 <Button
                     className={classes.button}
                     variant="outlined"
                 >
                     ACTUALIZAR
                 </Button>
                 <Button
                     className={classes.button}
                     variant="outlined"
                     color="default"
                     >
                     APROBAR 
                 </Button>
                 <Button
                     className={classes.button}
                     variant="outlined"
                     color="default"
                 //    onClick={rechazar}
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
 cliente: any;
 catalogState: CatalogState;
 datosSolicitudPostulacion: any;
 tipoSolicitud: number;
 formularioPostulacion: any;
 formularioResumenInformacionSolicitud: any;
 formularioObservacionCargueArchivos: any;
 regresarPantallaInicial: () => void;
}

export default PerdidaDeBeneficio;