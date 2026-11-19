import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import React, { useEffect, useMemo, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { Delete, Edit, Search, Share, ShareOutlined } from '@material-ui/icons';
import moment from 'moment';
import { toast } from 'react-toastify';
import { CatalogState } from '../../reducers/catalog.reducer';
import { ProductState } from '../../reducers/producto.reducer';
import {startGetAllIdentificationTypes,
    startGetAllCities,
    startGetAllDepartments,
    startGetBeneficiarioCartera,
    startGetConceptosCartera,
    startGetTipoComprobante
} from '../../actions/catalog.action';
import {startGetProductoByIdentification,
        startPostCrearComprobanteCartera,
        startPostObtenerComprobanteCartera,
        startPostActualizarMovimientoComprobante
        
    } from '../../actions/producto.action';
import {startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import { LoginState } from '../../reducers/login.reducer';
import { AppState } from '../../store/rootReducer';
import { CITYDTO, DEPARTAMENTODTO, IDENTIFICATIONTYPEDTO, MOVIMIENTOS } from '../../types/catalog.type';
import ContainerTitle from '../../components/containe-title/container-title';
import ModalEditarMovimientoCartera from './components/ModalEditarMovimiento';
import { ClientState } from '../../reducers/client.reducer';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';
import { LoadingComponent } from '../../components/loading/loading.component';
import ApiproductoClients from '../../services/api-producto.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';


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
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
		},
        totales: {
            textAlign: 'right'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
            marginBottom: '2rem',

        },
        buttonContainer: {
            display: 'flex',
            justifyContent: 'center', // Centra los botones horizontalmente
            marginTop: '1rem'
        }
      
    }),
);

const GenerarComprobanteContableIntegracion = (props: Props) => {
    const classes = useStyles();
    const {
        catalogState,
        loginState,
        productState,
        clientState,
        startGetAllIdentificationTypes,
        startGetProductoByIdentification,
        startGetAllCities,
        startGetAllDepartments,
        startPostCrearComprobanteCartera,
        startPostActualizarMovimientoComprobante,
        startPostObtenerComprobanteCartera,
        startGetBeneficiarioCartera,
        startGetConceptosCartera,
        startGetTipoComprobante,
        startGetClientByIdentificationTypeIdentificationNumber,
    } = props;

    const api = ApiproductoClients()

    const [isLoading, setIsLoading] = useState (false);

    const comprobanteControl = useForm({
		defaultValues: {
            fechaComprobante: new Date(),
            tipoComprobante: '',
		},
	});

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllDepartments()
        startGetAllCities()
        startGetBeneficiarioCartera()
        startGetConceptosCartera()
        startGetTipoComprobante()
    }, []);



    const handleActualizarMovimiento = async (data: any) => {

        debugger
        const request = {
            fechaComprobante: data.fechaComprobante,
            tipo: data.tipoComprobante,
        };
        
                api.postGenerarComprobanteCausaciones(request)
                .then(async (response) => {
                    setIsLoading(true)
                    if (response) {
                        setIsLoading(false)

                        if (response.isSuccess) {

                            await NotificationService.genericAlert(
                                'Reporte',
                                'El reporte se genero correctamente',
                            );

                        } else {
                            setIsLoading(false)
                            NotificationService.genericAlert(
                                'Error en el proceso de report',
                                `${response.returnMessage}`,
                                'warning',
                            )
                        }
                    }
                }).catch(() => {
                    setIsLoading(true)
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                });
            
        
    };


    return(
    <CHPrivateContainer>
        <PageIndicators
            topTextNivel1='Cierres Cartera'
            topTextNivel2='Contabilidad Cartera' 
        />

      <LoadingV2Component key="loader" loading={isLoading} tittle='Generando Reporte...' />

        <IconTitle
            title='Generar '
            subTitle='Reporte Cartera'
            icon='fch-Administracion-Usuarios-0-1'     
        />

        <CHPaper title="Datos de Comprobante">

            <ContainerTitle title="CREAR COMPROBANTE" />

            <Grid container>

  

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="fechaComprobante"
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        control={comprobanteControl.control}
                        render={({
                            field: { onChange, value },
                            
                        }) => (
                            <CHDatePicker
                                label="Fecha Comprobante*"
                                inputProps={{ readOnly: true }}
                                onChange={onChange}
                                value={value}
                                autoOk
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="tipoComprobante"
                        control={comprobanteControl.control}

                        defaultValue=""
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Tipo*"
                                value={value}
                                helperText={error ? error.message : null}
                                onChange={onChange}
                            >
                                <MenuItem key="D" value="1">
                                    Reporte Causaciones
                                </MenuItem>
                                <MenuItem key="C" value="2">
                                    Reporte Contable Pagos
                                </MenuItem>
                            </CHSelect>
                        )}
                    />
                </Grid>

            <Grid className={classes.column} item xs={12} md={4}>

                <Button
                    size="small"
                    variant="contained"
                    color="primary"
                    onClick={comprobanteControl.handleSubmit(handleActualizarMovimiento)}
                >
                    Crear Comprobante
                </Button>

            </Grid>

            </Grid>

        </CHPaper>

    </CHPrivateContainer>
    )

}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
    productState: ProductState;
    clientState: ClientState;

}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    productState: state.producto,
    clientState: state.client,


});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;    startGetAllDepartments: () => void;
    startPostCrearComprobanteCartera: (data: any) => void;
    startPostActualizarMovimientoComprobante: (data: any) => void;
    startPostObtenerComprobanteCartera: (data: any) => void;
    startGetAllCities: (stateID?: string) => void;
    startGetBeneficiarioCartera: () => void;
    startGetConceptosCartera: () => void;
    startGetTipoComprobante: () => void;
    
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetProductoByIdentification,
    startGetAllDepartments,
    startPostCrearComprobanteCartera,
    startPostObtenerComprobanteCartera,
    startGetAllCities,
    startGetBeneficiarioCartera,
    startGetConceptosCartera,
    startGetTipoComprobante,
    startPostActualizarMovimientoComprobante,
    startGetClientByIdentificationTypeIdentificationNumber
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(GenerarComprobanteContableIntegracion);




