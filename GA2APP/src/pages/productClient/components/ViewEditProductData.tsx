import React, { useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    Button,
    withStyles,
    Tab,
    Tabs,
    Theme,
} from '@material-ui/core';
import { TABS_INDEX } from '../constants/producto.constants';
import ProductPanel from './ProductPanel';
import { ProductoDto } from '../../../types/producto.model.type';
import { ClienteDto } from '../../../types/client.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);
const AntTabs = withStyles({
    flexContainer: {
        display: 'flex',
        flexWrap: 'wrap',
    },
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
                label="Consulta de Cartera"  value={TABS_INDEX.CONSULTA_PRODUCTO}
            />
            <AntTab
                label="Consulta de Producto" value={TABS_INDEX.CONSULTA_OBLIGACIONES}
            />
            <AntTab label="Aplicación de Pago" value={TABS_INDEX.APLICACION} />
            <AntTab label="Novedades de Cartera" value={TABS_INDEX.NOVEDADES} />
            {/* <AntTab label="Visualizar Archivos" value={TABS_INDEX.UPLOAD_FILES} /> */}
            {/* <AntTab
                label="Aplicación masiva de pagos"
                value={TABS_INDEX.APLICACION_MASIVA_DE_PAGOS}
            /> */}

               <AntTab  label="Plan de Pagos" value={TABS_INDEX.PLAN_DE_PAGOS}/>
               <AntTab  label="Saldos Diarios" value={TABS_INDEX.SALDOS_DIARIOS}/>
               <AntTab  label="Cuotas Adelantadas" value={TABS_INDEX.CUOTAS_ADELANTADAS}/>
        </AntTabs>
    );
}

const Productos = (props: propsType) => {

    const classes = useStyles();

    const { onItemGoBack,  idType, idNumber, productoRequest, clienteRequest } =
        props;

    const [currentTabValue, setCurrentTabValue] = useState(
        TABS_INDEX.CONSULTA_PRODUCTO,
    );

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        setCurrentTabValue(newValue);
    };

    return (
        <>
            <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
            />
            <ProductPanel
                currentTabValue={currentTabValue}
                idType={idType}
                idNumber={idNumber}
                productoRequest={productoRequest}
                clienteRequest={clienteRequest}
            />
            {
      currentTabValue !== 8 ?
   <Grid container>
   <Grid className={classes.column} item xs={12} md={9} />
   <Grid className={classes.column} item xs={12} md={3}>
       <Button
           className={classes.button}
           variant="contained"
           color="primary"
           onClick={onItemGoBack}
       >
           REGRESAR
       </Button>
   </Grid>
</Grid>: '' 
            }
         
        </>
    );
};
type propsType = {
    onItemGoBack: () => void;
    productoRequest: ProductoDto;
    idType: number;
    idNumber: string;
    clienteRequest: ClienteDto;
};

export default Productos;
