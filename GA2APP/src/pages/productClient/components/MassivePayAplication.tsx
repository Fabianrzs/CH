import React, { useEffect, useRef, useState } from 'react';
import ReactDOM from 'react-dom';
import { Button, Chip, CircularProgress, createStyles, Grid, makeStyles, Tab, Tabs, Theme, withStyles } from '@material-ui/core';
import swal from 'sweetalert';
import moment from 'moment';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { Alert, AlertTitle } from '@material-ui/lab';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import TableMassivePayAplicationList from './TableMassivePayAplicationList';
import { ModalAplicacionMasivaPagosList } from './modalMassivePayAplication/ModalAplicacionMasivaPagosList';
import { procesoListarAplicacionMasivaPago } from '../../../types/producto.model.type';
import apiProducto from '../../../services/api-producto.service';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import CatalogApi from '../../../services/api-catalog.service';
import CreditApi from '../../../services/api-credit.service';
import AprobacionListas from './AprobacionLista';
import ListasPendientes from './ListasPendientes';
import ListasAprobadas from './ListasAprobadas';

const TABS_INDEX = {
    APROBACION_LISTA: 0,
    LISTAS_PENDIENTES: 1,
    LISTAS_APROBADAS: 2,
};

interface StyledTabProps {
    label: string;
    value: any;
}

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
            minWidth: 170,  // 72
            marginRight: theme.spacing(1),
            '&:hover': {
                color: '#FEC917',
                opacity: 1,
            },
            '&$selected': {
                color: '#1C3F8B',
                fontWeight: 'bold',
                borderBottonColor: '#1C3F8B'
            },
            '&:focus': {
                color: '#1C3F8B',
            },
        },
        selected: {},
    }),
)((props: StyledTabProps) => <Tab disableRipple {...props} />);

const ViewTabs = ({currentTab, onTabChange}: {
    currentTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
}) => (
    <AntTabs value={currentTab} onChange={onTabChange}>
        <AntTab label="Aprobación de Lista"  value={TABS_INDEX.APROBACION_LISTA}/>
        <AntTab label="Listas Pendientes" value={TABS_INDEX.LISTAS_PENDIENTES}/>
        <AntTab label="Listas Aprobadas" value={TABS_INDEX.LISTAS_APROBADAS}/>
    </AntTabs>
);


const CurrentView = (props: any) => {

    if(props.currentTabValue === 0)
        return <AprobacionListas />;
    
    if (props.currentTabValue === 1)
        return <ListasPendientes />;

    return <ListasAprobadas />;
};

const MassivePayAplication = () => {
    const [currentTabValue, setCurrentTabValue] = useState<number>(TABS_INDEX.APROBACION_LISTA);
    const handleTabChange = (event: React.ChangeEvent<any>, newValue: number) => setCurrentTabValue(newValue);

    return (
        <CHPrivateContainer>
            <CHPaper title="Aplicación Masiva de Pagos">
                <ViewTabs currentTab={currentTabValue} onTabChange={handleTabChange}/>
                <CurrentView currentTabValue={currentTabValue}/>
            </CHPaper>
        </CHPrivateContainer>
    );
}

export default MassivePayAplication;