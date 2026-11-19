import { createStyles, Tab, Tabs, withStyles, Theme, Paper, Grid, makeStyles, Button } from '@material-ui/core';
import React, {useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';
import { useForm, UseFormReturn } from 'react-hook-form';
import history from '../../../../utils/history.router';
import CHPageTitleV2 from '../../../../components/ch-page-title/CHPageTitleV2';
import CHPrivateContainer from '../../../../components/ch-private-container/CHPrivateContainer';
import { StyledTabProps } from '../../../productClient/components/reportes/componentes/CustomTabs';
import { TABS_INDEX_UE } from '../../constants/tablaParametroform.model.type';
import Informacion from './Informacion';

const useStyles = makeStyles(() =>
createStyles({
    margin: {
            marginLeft: '3%',
    },
    columnButtonAdd: {
        textAlign: 'right',
    },
    button: {
        width: '120px',
        marginLeft: '.5rem',
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
       color: '#808080',
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
                label="Información"
                value={TABS_INDEX_UE.INFORMACION}
            />
            <AntTab
                label="Contactos"
                value={TABS_INDEX_UE.CONTACTOS}
            />
            <AntTab
                label="Direcciones"
                value={TABS_INDEX_UE.DIRECCIONES}
            />
            <AntTab
                label="Tipos de Archivos"
                value={TABS_INDEX_UE.TIPOS_ARCHIVOS}
            />
            <AntTab
                label="Anotaciones"
                value={TABS_INDEX_UE.ANOTACIONES}
            />
            <AntTab
                label="Presupuesto"
                value={TABS_INDEX_UE.PRESUPUESTO}
            />
            <AntTab
                label="Usuarios"
                value={TABS_INDEX_UE.USUARIOS}
            />
            <AntTab
                label="Generación de Archivos"
                value={TABS_INDEX_UE.GENERACION_ARCHIVOS}
            />
            <AntTab
                label="Movimientos"
                value={TABS_INDEX_UE.MOVIMIENTOS}
            />
        </AntTabs>
    );
}

function Panel({
    currentTabValue,
    formularioInformacion
 }: {
    currentTabValue: number;
    formularioInformacion:  UseFormReturn<any>;
 }) {
 
    switch (currentTabValue) {
        case TABS_INDEX_UE.INFORMACION:
            return (
                <>
                    <Informacion 
                        formulario={formularioInformacion}
                    />
                </>
            );
        default:
            return null;
    }
 }

const InformacionUnidadEjecutora = () => {
    const classes = useStyles();
    const location = useLocation();
    const data: any = location.state;
    const [currentTabValue, setCurrentTabValue] = useState(TABS_INDEX_UE.INFORMACION);

    const formularioInformacion = useForm ({
        defaultValues: {
            identificacion: '',
            nombre: '',
            sigla: '',
            fuerza: '',
            codigoUE: '',
            tasaAporte: '',
            areaNegocio:'',
            frmID: '',
            aval: '',
            principalUE: ''
        }
    })

    useEffect(() => {
        formularioInformacion.setValue('identificacion', data?.UEJ_IDENTIFICACION);
        formularioInformacion.setValue('nombre', data?.UEJ_NOMBRE);
        formularioInformacion.setValue('sigla', data?.UEJ_SIGLA);
        formularioInformacion.setValue('fuerza', data?.FRZ_ID);
        formularioInformacion.setValue('codigoUE', data?.UEJ_CODIGO);
        formularioInformacion.setValue('tasaAporte', data?.UEJ_TASA_APORTE);
        formularioInformacion.setValue('areaNegocio', data?.UEJ_AREA_NEGOCIO);
        formularioInformacion.setValue('frmID', data?.FRM_ID);
        formularioInformacion.setValue('aval', data?.UEJ_AVAL);
        formularioInformacion.setValue('aval', data?.UEJ_ID_PRINCIPAL);
    },[])

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
     ) => {
        setCurrentTabValue(newValue);
    };

    console.log('data', data)

    const onClickRegresar = () => {
        history.push('/TablasParametros');
    } 

    return(
        <>
            <CHPrivateContainer>
                <CHPageTitleV2
                    topText="Gestión de Unidad Ejecutora"
                    title1="Información"
                    title2='Unidad Ejecutora'
                    icono="fact_check"
                />
               <Grid container>
                    <Grid className={classes.margin} item xs={12} md={12}>
                        <ViewTabs
                            currentTab={currentTabValue}
                            onTabChange={handleTabChange}
                        />
                        <Panel
                            currentTabValue={currentTabValue}
                            formularioInformacion={formularioInformacion}
                        />
                    </Grid>
               </Grid>
               <Grid container>
                    <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                        <Button
                            className={classes.button}
                            variant="outlined"
                            color="default"
                           // onClick={onClickRegresar}
                        >
                            ACTUALIZAR
                        </Button>
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
            </CHPrivateContainer>
        </>
    )
}

export default InformacionUnidadEjecutora;