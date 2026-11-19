import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    IconButton,
    DialogTitle,
    styled,
    Tabs,
    Tab,
} from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import CloseIcon from '@material-ui/icons/Close';
import { useForm } from 'react-hook-form';
import moment from 'moment';
import ApiReportes from '../../../../../../services/api-reportes.service';
import { ReporteEstadoCuentaPlanPago } from '../../types/ReporteEstadoCuentaPlanPago.type';
import TablaReportePlanPago from '../TablaReportePlanPago';
import ReporteEstadoCuentaAfiliado from '../../types/ReporteEstadoCuentaAfiliado.type';
import UtilService from '../../../../../../utils/util.service';
import TablaReporteMovimientos from '../TablaReporteMovimientos';
import ReporteEstadoMovimientos from '../../types/ReporteEstadoMovimientos.type';
import ControllerFecha from '../../controllers/ControllerFecha';

interface ModalsProps {
    open: boolean,
    data: ReporteEstadoCuentaAfiliado,
    handleClose: () => void
}

const TAB_INDEX = {
    PLAN_PAGOS: 0,
    MOVIMIENTOS: 1,
};


const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        rightAlign: {
            textAlign: 'right'
        },
        resumenInfo: {
            marginBottom: '1rem'
        },
        infoCliente: {
            display: 'flex',
            flexWrap: 'wrap',
            gap: '6rem'
        },
        infoObligacion: {
            display: 'grid',
            gap: '1rem 3rem',
            gridTemplateColumns: 'repeat(auto-fill, minmax(260px, 1fr))',
        },
        alert: {
            "& .MuiAlert-message": {
                "flex-grow": "1"
            }
        },
        buttonsecundario: {
            background: '#43b5f1',
        },
        button: {
            width: '250px',
            marginTop: '1.5rem',
            marginLeft:'90%'
        },
    }),
);

const CustomTabs = styled(Tabs)({
    borderTopLeftRadius: "5px", 
    borderTopRightRadius: "5px",
    overflow: "hidden", 
  });
  
const CustomTabItems = styled(Tab)({
    borderTopLeftRadius: "5px", 
    borderTopRightRadius: "5px",
    textTransform: "none", 
  });


const ViewTabs = ({currentTab, onTabChange}: {
    currentTab: number;
    onTabChange: (newValue: number) => void;
}) => (
    <CustomTabs
        value={currentTab}
        onChange={(_, value) => onTabChange(value)}
        aria-label="ant example"
        TabIndicatorProps={{
            style: { backgroundColor: "#1c3f8b" },
        }}
    >
        <CustomTabItems label="Plan pagos"  value={TAB_INDEX.PLAN_PAGOS} />
        <CustomTabItems label="Movimientos" value={TAB_INDEX.MOVIMIENTOS}/>
    </CustomTabs>
);

const ResumenInfo = (props: { data: ReporteEstadoCuentaAfiliado}) => {

    const classes = useStyles();
    const { data: resumen } = props;

    return (
        <Grid container>
            <Grid className={classes.resumenInfo} item xs={12}>
                <Alert severity="info" elevation={2} className={classes.alert}>
                    <AlertTitle><strong>INFORMACIÓN DEL CLIENTE</strong></AlertTitle>
                    <div className={classes.infoCliente}>
                        <div className={classes.infoPanel}>
                            <strong>Tipo Identificación :</strong>
                            <span>{resumen?.tipoDocumento}</span>
                            <strong>Identificación :</strong>
                            <span>{resumen?.numeroIdentificacion}</span>
                            <strong>Nombres : </strong>
                            <span>{resumen?.nombreAfiliado}</span>
                        </div>
                    </div>
                    <br />
                    <br />
                    <AlertTitle><strong>INFORMACIÓN DE OBLIGACIÓN</strong></AlertTitle>
                    <div className={classes.infoObligacion}>
                        <div className={classes.infoPanel}>
                            <strong>Número Crédito :</strong>
                            <span className={classes.rightAlign}>{resumen?.numeroCredito}</span>
                            <strong>Línea :</strong>
                            <span className={classes.rightAlign}>{resumen?.lineaCredito}</span>
                            <strong>Monto :</strong>
                            <span className={classes.rightAlign}>{UtilService.formatCurrency(resumen?.valorDesembolso)}</span>
                            <strong>Saldo :</strong>
                            <span className={classes.rightAlign}>{UtilService.formatCurrency(resumen?.saldo)}</span>
                        </div>
                        <div className={classes.infoPanel}>
                            <strong>Cuota :</strong>
                            <span className={classes.rightAlign}>{UtilService.formatCurrency(resumen?.valorCuota)}</span>
                            <strong>Plazo :</strong>
                            <span className={classes.rightAlign}>{resumen?.plazoTotal}</span>
                            <strong>Opcion Compra :</strong>
                            <span className={classes.rightAlign}>{UtilService.formatCurrency(resumen?.valorOpcionCompra)}</span>
                            <strong>Valor Inmueble :</strong>
                            <span className={classes.rightAlign}>{UtilService.formatCurrency(resumen?.valorVivienda)}</span>
                        </div>
                        <div className={classes.infoPanel}>
                            <strong>Apertura :</strong>
                            <span className={classes.rightAlign}>{UtilService.getDate(resumen?.fechaDesembolso)}</span>
                            <strong>Ultimo Pago :</strong>
                            <span className={classes.rightAlign}>{UtilService.getDate(resumen?.fechaUltimoPago)}</span>
                            <strong>Próximo Pago :</strong>
                            <span className={classes.rightAlign}>{UtilService.getDate(resumen?.fechaProximoPago)}</span>
                            <strong>Canón Extra :</strong>
                            <span className={classes.rightAlign}>{UtilService.formatCurrency(resumen?.valorCanonInicial)}</span>
                        </div>
                    </div>
                </Alert>
                
            </Grid>          
        </Grid>
    );
}

export const ModalReporteDetalles = (props: ModalsProps) => {
    const api = ApiReportes();
    const classes = useStyles();
    const currentDate = new Date();

    const { open, data, handleClose } = props;
    const [isLoading, setIsLoading] = useState(false);
    const [planPago, setPlanPago] = useState<ReporteEstadoCuentaPlanPago[]>([]);
    const [movimientos, setMovimientos] = useState<ReporteEstadoMovimientos[]>([]);
    const [currentTabIndex, setCurrentTabIndex] = useState(0);
    const [request, setRequest] = useState({
        fechaInicial: moment(currentDate).startOf('year').toDate(),
        fechaFinal: moment(currentDate).endOf('year').startOf('day').toDate()
    });
    
    const formControls = useForm({
        'mode': 'onChange',
        defaultValues: {
            fechaInicial: request.fechaInicial,
            fechaFinal: request.fechaFinal,
        },
    });


    useEffect(() => {
        
        if(currentTabIndex === TAB_INDEX.PLAN_PAGOS)
            cargarPlanPago();

        if(currentTabIndex === TAB_INDEX.MOVIMIENTOS)
            cargarMovimientos(request);

    }, [data, currentTabIndex]);

    const cargarPlanPago = async () => {
        if(data) {
            setIsLoading(true);
            setPlanPago([]);
            api.reporteEstadoCuentaPlanPago(data.idCredito)
                .then(result => result?.isSuccess && result.data && setPlanPago(result.data))
                .catch(() => setPlanPago([]))
                .finally(() => setIsLoading(false));
        } else {
            setPlanPago([]);
        }
    }

    const cargarMovimientos = async (request: {fechaInicial?: Date, fechaFinal?: Date}) => {
        if(data) {
            setIsLoading(true);
            setMovimientos([]);
                api.reporteEstadoMovimientos({
                    idCredito: data.idCredito,
                    fechaInicial: request.fechaInicial ? moment(request.fechaInicial).format("YYYY-MM-DD") : null,
                    fechaFinal: request.fechaFinal ? moment(request.fechaFinal).format("YYYY-MM-DD") : null,
                })
                .then(result => result?.isSuccess && result.data && setMovimientos(result.data))
                .catch(() => setMovimientos([]))
                .finally(() => setIsLoading(false));
        } else {
            setMovimientos([]);
        }
    }

    const onSubmit = (formValues: any) => {

        const currentRequest = {fechaInicial: formValues.fechaInicial, fechaFinal: formValues.fechaFinal};
        setRequest(currentRequest);
        cargarMovimientos(currentRequest);
    } 

    const exit = () => {
        handleClose();
    };

    return (
        <Dialog
                open={open}
                onClose={exit}
                aria-labelledby="form-dialog-title"
                maxWidth="xl"
                fullScreen
            >
                <DialogTitle>
                    <Grid container justifyContent='space-between' alignItems='center'>
                        <Grid item>
                            <Typography className={classes.titleform} variant="h5" component="h6">
                                DETALLES
                            </Typography>
                        </Grid>
                        <Grid item>
                            <IconButton onClick={exit}>
                                <CloseIcon />
                            </IconButton>
                        </Grid>
                    </Grid>
                </DialogTitle>
                <DialogContent  >
                    {data && <ResumenInfo data={data}/>}
                    <br />
                    <ViewTabs currentTab={currentTabIndex} onTabChange={setCurrentTabIndex} />
                    <br />
                    {
                        currentTabIndex === TAB_INDEX.PLAN_PAGOS &&
                        <TablaReportePlanPago data={planPago} estadoCuenta={data} isLoading={isLoading} />
                    }
                    {
                        currentTabIndex === TAB_INDEX.MOVIMIENTOS &&
                        <>
                            <Grid container justifyContent='space-around' style={{'margin': '2rem 0'}}>
                                <Grid item xs={12} md={3}>
                                    <ControllerFecha form={formControls} name="fechaInicial" label="Fecha Inicial" />
                                </Grid>
                                <Grid item xs={12} md={3}>
                                    <ControllerFecha form={formControls} name="fechaFinal" label="Fecha Final" />
                                </Grid>
                                <Grid item container xs={12} md={3} alignItems='center'>
                                    <Button
                                        type='submit'
                                        variant="contained"
                                        color="primary"
                                        style={{width: '200px'}}
                                        disabled={!formControls.formState.isValid}
                                        onClick={formControls.handleSubmit(onSubmit)}>
                                        Consultar
                                    </Button>
                                </Grid>
                            </Grid>
                            <TablaReporteMovimientos  
                                data={movimientos} estadoCuenta={data} isLoading={isLoading} 
                                formData={request}
                            />
                            
                        </>
                    }
                    <Grid container>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            onClick={handleClose}
                        >
                            REGRESAR
                        </Button>
                    </Grid>
                </DialogContent>
            </Dialog>
    );
};