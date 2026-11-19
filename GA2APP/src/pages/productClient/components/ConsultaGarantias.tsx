import { Button, Card, CardContent, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import ContainerTitle from '../../../components/containe-title/container-title';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CarteraApi from '../../../services/api-cartera-service';
import AuthService from '../../../services/auth.service';
import TituloIcono from '../../AdministracionCuentas/components/titulo-icono/tituloIcono.component';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import {
    startGetAllIdentificationTypes,
    startGetAllTipoVivienda,
    startGetAllTipoCredito,
} from '../../../actions/catalog.action';
import {startGetProductoByIdentification} from '../../../actions/producto.action';
import { AppState } from '../../../store/rootReducer';
import { ProductState } from '../../../reducers/producto.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CatalogApi from '../../../services/api-catalog.service';
import ReportApi from '../../../services/api-reportes.service';
import TablaReporteGeneralGarantia from './reportes/reporteGeneralGarantia/TablaReporteGeneralGarantia';

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
        root: {
            flexGrow: 1,
        },
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917'
        },
    }),
);
const ConsultaGarantias = (props: Props) => {

    const {
        productoState,
        catalogState,
        loginState,
        startGetAllIdentificationTypes,
        startGetAllTipoVivienda,
        startGetAllTipoCredito,
        startGetProductoByIdentification,
    } = props;
    
    const classes = useStyles();
    const carteraApi =CarteraApi();
    const reportesApi = ReportApi();
    const formularioConsulta = useForm({
        defaultValues: {
            tipoIdentificacion: '',
            identificacionCliente: '',
            idTipoCredito: '',
            numeroCredito: '',
            periodo:'',
        }
    })
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState<any[]|null>(null);
    const [periodoSeleccionado, setPeriodoSeleccionado] = useState<Date|null>(null);

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllTipoVivienda(loginState.claims);
        startGetAllTipoCredito(loginState.claims);
    }, []);

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]

        const hexadecimal = convertIp
        await catalogApi
            .getObtenerIpLocal(hexadecimal)
            .then(async (response: any) => {

                if (response) {
                    if (response.isSuccess) {
                        setPublicIpAddress(response.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

    useEffect(() => {
        const fetchLocalIPAddress = () => {
            const peerConnection = new RTCPeerConnection({ iceServers: [] });
            peerConnection.createDataChannel('');
            peerConnection.createOffer()
                .then((offer) => peerConnection.setLocalDescription(offer))
                .catch((error) => console.error('Error creating offer:', error));

            peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    const ip: any = event.candidate.address;
                    if (isValidIPv4(ip) || isValidIPv6(ip)) {
                        enviarIp(ip);
                    } else {
                        enviarIp(ip);
                    }
                }
            };

            return () => {
                peerConnection.close();
            };
        };
        fetchLocalIPAddress();

    }, []);
  
    const handleSearchButton = async(data: any) => {
        const fechaMoment = moment(data.periodo);
        setIsLoading(true);
        setPeriodoSeleccionado(data.periodo);
        const auditoriaApi=AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;   
        
        const params = {
            anio: Number(fechaMoment.format("YYYY")),
            mes: Number(fechaMoment.format("MM")),
            numeroCredito: String(data.numeroCredito),
            tipoCredito: data.idTipoCredito === 0 ||  data.idTipoCredito === ''? null : data.idTipoCredito,   
            identificacion: data.identificacionCliente
        }

        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Consulta Garantias',
            hostname:document.location.host
        } as Auditoria

        let tipoCredito = 'No especificado';

        if(data.idTipoCredito === 0 || data.idTipoCredito === ''){
            tipoCredito = 'NINGUNO';
        } else if (data.idTipoCredito === 1) {
            tipoCredito = 'LEASING HABITACIONAL';
        } else if (data.idTipoCredito === 2) {
            tipoCredito = 'CRÉDITO HIPOTECARIO';
        } else if (data.idTipoCredito === 3) {
            tipoCredito = 'CRÉDITO DE CONSUMO';
        } else if (data.idTipoCredito === 4) {
            tipoCredito = 'CRÉDITO EDUCATIVO';
        } 

        await reportesApi.reporteGeneralGarantia(params)
        .then((response: any) => {
            if (response.isSuccess) {
                setData(response.data);

                requestAuditoria.tipoAccion='CONSULTAS';
                requestAuditoria.idTipoAccion=3;
                requestAuditoria.descripcion= `Consulta de Garantías con los siguientes filtros:
                - No. Identificación: ${data.identificacionCliente || 'No especificada'}
                - Tipo Crédito: ${tipoCredito || 'No especificado'}
                - Mes: ${data.mes|| 'No especificada'}
                - Año: ${data.año || 'No especificado'}`;
                auditoriaApi.InsertAuditoria(requestAuditoria);
                setIsLoading(false);
    
            } else {
                    toast("No se encontraron resultados", { type: toast.TYPE.ERROR, autoClose: 10000 });
                    setIsLoading(false);
                    requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion= `Consulta de Garantías con los siguientes filtros:
                    \n No. Identificación: ${data.identificacionCliente || 'No especificada'}
                    \n Tipo Crédito: ${tipoCredito || 'No especificado'}
                    \n Mes: ${data.mes|| 'No especificada'}
                    \n Año: ${data.año || 'No especificado'}
                    \n Validación: No se encontraron resultados`;
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            }
        )
        .catch((error: any) => {
            setIsLoading(false);
            if (error.response && error.response.data) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=8;
            requestAuditoria.descripcion= `Consulta de Garantías con los siguientes filtros:
            \n No. Identificación: ${data.identificacionCliente || 'No especificada'}
            \n Tipo Crédito: ${tipoCredito || 'No especificado'}
            \n Mes: ${data.mes|| 'No especificada'}
            \n Año: ${data.año || 'No especificado'}
            \n returnApiMessage: ${error?.response?.data?.returnMessage }`;
            auditoriaApi.InsertAuditoria(requestAuditoria);
        })
        .finally(() => {
            setIsLoading(false);
        });
    }

    return(
        <div className={classes.root}>
            
                <PageIndicators
                    topTextNivel1='Garantias'
                    topTextNivel2='Gestion de Garantias' 
                />
                <IconTitle
                    title='Consulta'
                    subTitle='Garantias'
                    icon='fch-Buscar-0-1'
                />
                <Grid >
                    <Grid item xs={12}>
                        <Card>
                            <ContainerTitle title="Garantias"/>
                            <CardContent>
                                <br />    
                                <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIcono
                                        title1="Número"
                                        subtitle="Identificación"
                                        icono="event_available"
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="identificacionCliente"
                                        control={formularioConsulta.control}
                                        rules={{
                                            pattern: {
                                                value: /^[0-9]+$/g,
                                                message: 'Este campo es requerido y debe contener solo números'
                                            },
                                            minLength: {
                                                value: 4,
                                                message:
                                                    'La longitud mínima del campo es de 4 caracteres',
                                            },
                                            maxLength: {
                                                value: 12,
                                                message:
                                                    'La longitud máxima del campo es de 12 caracteres',
                                            },
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly={false}
                                                InputLabelProps={{ shrink: true }}
                                                label="Número de Identificación"
                                                value={value}
                                                onChange={onChange}
                                                inputProps={{
                                                    inputMode: 'numeric',
                                                    pattern: '[0-9]*'
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                                </Grid>
                                <br />
                                <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIcono
                                        title1="Tipo"
                                        subtitle="Crédito"
                                        icono="fact_check"
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="idTipoCredito"
                                        control={formularioConsulta.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label="Tipo De Crédito"
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                <MenuItem key={0} value={0}>
                                                           NINGUNO
                                                </MenuItem>
                                                {catalogState.tipoCredito?.map(
                                                    (option: any) => (
                                                        <MenuItem key={option.tcR_ID} value={option.tcR_ID}>
                                                            {option.tcR_DESCRIPCION}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                                </Grid>
                                <br />
                               <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIcono
                                        title1="Número"
                                        subtitle="Crédito"
                                        icono="event_available"
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="numeroCredito"
                                        control={formularioConsulta.control}
                                        rules={{
                                            pattern: /^[a-zA-Z\-0-9]+$/g
                                                
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message ||  "El número de crédito solo acepta alfanumérico, solo letras, numéros y guión" : null
                                                }
                                                
                                                label="Número de Crédito"
                                                value={value}
                                                onChange={(onChange)}
                                                numberOnlyMask
                                            
                                            />
                                        )}
                                    />
                                </Grid>
                                </Grid>
                                <br />
                                <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIcono
                                        title1="Fecha"
                                        subtitle=""
                                        icono="event_available"
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="periodo"
                                        control={formularioConsulta.control}
                                        rules={{
                                            required:
                                                'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                   views={['year', 'month']}
                                                    label="Fecha *"
                                                    InputLabelProps={{ shrink: true}}
                                                    onChange={onChange}
                                                    value={value}
                                                    error={!!error}
                                                    format="yyyy/MM"
                                                    inputProps={{placeholder: "YYYY/MM"}}
                                                    helperText={
                                                        error ? error.message : null
                                                    }
                                                    autoOk
                                            />
                                        )}
                                    />
                                </Grid>
                                </Grid>
                                <br />
                                <div className="buttonssolicitud">
                                    <Button
                                    className={classes.buttonprincipal}
                                    variant="outlined"
                                    onClick={formularioConsulta.handleSubmit(
                                            handleSearchButton,
                                        )}
                                    >
                                    CONSULTAR GARANTIA
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    </Grid>
                </Grid>
                <LoadingV2Component key="loader" loading={isLoading} tittle='Generando reporte...' />
                {data && <TablaReporteGeneralGarantia data={data} periodo={periodoSeleccionado} />}
        </div>
    );
}
interface LinkMapStateToProps {
    productoState: ProductState;
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    productoState: state.producto,
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetAllTipoVivienda: (claims: string) => void;
    startGetAllTipoCredito: (claims: string) => void;
    startGetProductoByIdentification: (identificationType: number, idNumber: string, idProducto: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetAllTipoVivienda,
    startGetAllTipoCredito,
    startGetProductoByIdentification,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;
export default connect(mapStateToProps, mapDispatchToProps)(ConsultaGarantias);