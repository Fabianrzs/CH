import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Grid,
    makeStyles,
    createStyles,
    Typography,
    Button,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';

import { toast } from 'react-toastify';
import { useLocation } from 'react-router-dom';

import {
    startGetAllTipoCredito,
    startGetAllTiVivienda,
    startGetAllTipoVivienda,
    startGetAllEstadoVivienda,
    startGetAllIdentificationTypes,
    startGetCategory,
    startGetAllForces,
    startGetAllDepartments,
    startGetAllCities,
    starGetCreditLine
} from '../../../actions/catalog.action';
import {
    startGetClientByIdentificationTypeIdentificationNumber
} from '../../../actions/client.action';
import {
    startGetSimuladorGeneral
} from '../../../actions/simuladorGeneral.action';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { ClientState } from '../../../reducers/client.reducer';
import { AppState } from '../../../store/rootReducer';
import '../cv.scss';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { ProductState } from '../../../reducers/producto.reducer';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import { SimuladorGeneralState } from '../../../reducers/simuladorGeneral.reducer';
import InfoNewProduct from './InfoNewProduct';
import ApiClients from '../../../services/api-clients.service';
import CatalogApi from '../../../services/api-catalog.service';
import { useIp } from '../../../shared/context/IpContext';
import CreditApi from '../../../services/api-credit.service';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginRight: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        }
    }),
);

const Productos = (props: Props) => {
    const classes = useStyles();
    const { catalogState,
        loginState,
        clienteState,
        productoState,
        simuladorState,
        startGetClientByIdentificationTypeIdentificationNumber,
        startGetAllTipoCredito,
        startGetAllTiVivienda,
        startGetAllTipoVivienda,
        startGetAllEstadoVivienda,
        startGetAllIdentificationTypes,
        startGetCategory,
        startGetAllForces,
        startGetAllDepartments,
        startGetAllCities,
        starGetCreditLine,
        startGetSimuladorGeneral
    } = props;

    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [mostrarFormularios, setMostrarFormularios] = useState(false);
    const [cliente, setCliente] = useState<any[]>([]);
    const [readonlyFinal, setReadonlyFinal] = useState(false);
    const [tituloBoton, setTituloBoton] = useState('CREAR PLAN DE PAGOS');
    const [mostrarTablaSegunFormulario, setMostrarTablaSegunFormulario] = useState(false);
    const [categoriaSalario, setCategoriaSalario] = useState<number | null>(null);
    const [salarioMinimoAnual, setSalarioMinimoAnual] = useState<any>(null);
 

    const location: any = useLocation()

    const nuevosDatos = location?.state?.procedureRequest




    const formularioConsulta = useForm({
        defaultValues: {
            identificationType: 1,
            identification: '',
        }
    })


    console.log('Agregar Producto',nuevosDatos )


    useEffect(() => {

        const _data = {
            identificationType: 1,
            identification: nuevosDatos?.clientIdentificationNumber
        }
        formularioConsulta.setValue('identification', nuevosDatos?.clientIdentificationNumber)
        if (nuevosDatos?.clientIdentificationNumber !== undefined) {
            setReadonlyFinal(true)
            handleSearchButton(_data)

        }

    }, [nuevosDatos?.clientIdentificationNumber !== undefined])

    useEffect(() => {
        startGetAllTipoCredito(loginState.claims);
        startGetAllTiVivienda(loginState.claims);
        startGetAllTipoVivienda(loginState.claims);
        startGetAllEstadoVivienda(loginState.claims);
        startGetAllIdentificationTypes(loginState.claims);
        startGetCategory(loginState.claims);
        startGetAllForces(loginState.claims);
        startGetAllDepartments();
        startGetAllCities();
        startGetSimuladorGeneral();
    }, []);
    const apiCliente = ApiClients()
    const identificationType = formularioConsulta.watch('identificationType');

   

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
        try {
            if (!ip || typeof ip !== 'string') {
                return;
            }
    
            const convertIp = ip.split('.')[0];
            const hexadecimal = convertIp;
    
            const response = await catalogApi.getObtenerIpLocal(hexadecimal);
    
            if (response?.isSuccess) {
                setPublicIpAddress(response.data);
            }
        } catch (e) {
            console.error('ERROR EN enviarIp - AddProductData:', e);
            // Mejor no mostrar toast aquí porque no afecta la consulta del afiliado
            // toast('Error consultando IP local', { type: toast.TYPE.ERROR });
        }
    };

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



    const handleSearchButton = async (data: any) => {
        setMostrarTablaSegunFormulario(false);
        setIsDataLoaded(true);

        const request = {
            identificationType: data.identificationType,
            identification: data.identification
        };

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;

        const usrAg =
            nav?.userAgentData?.brands?.find(
                (x: { brand: string }) =>
                    x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand || navigator.userAgent;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav?.userAgentData?.platform || navigator.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Agregar Producto',
            hostname: document.location.host
        } as Auditoria;

        try {
            await consultarClientes(request);

            requestAuditoria.tipoAccion = 'CONSULTAS';
            requestAuditoria.idTipoAccion = 3;
            requestAuditoria.descripcion = `Consultar afiliado por tipo y número de identificación en agregar producto parámetros:
            \n Tipo de identificación: ${typeSelected}
            \n Identificación: ${request.identification}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error: any) {
            setIsDataLoaded(false);

            requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion = 8;
            requestAuditoria.descripcion = `Obtener afiliado por tipo y número de identificación en agregar producto parámetros:
            \n Tipo de identificación: ${typeSelected}
            \n Identificación: ${request.identification}
            \n Error: ${error}
            \n returnApiMessage: ${error?.response?.data?.returnMessage}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    };


    const consultarClientes = async (request: any) => {
        setIsDataLoaded(true);

        await apiCliente
            .getClientByIdentificationTypeIdentificationNumberRequest(
                Number(request.identificationType),
                String(request.identification)
            )
            .then(async response => {
                if (response?.data) {
                    const clienteData = response.data;

                    setCliente(clienteData);

                    const categoria = clienteData?.categoria ?? clienteData?.idCategoria;
                    setCategoriaSalario(categoria);

                    setMostrarFormularios(true);
                    setIsDataLoaded(false);
                    setReadonlyFinal(false);
                    setTituloBoton('CREAR PLAN DE PAGOS');
                } else {
                    toast(`Cliente no existe`, { type: toast.TYPE.ERROR });
                    setMostrarFormularios(false);
                    setIsDataLoaded(false);
                    setReadonlyFinal(false);
                }
            })
            .catch((e) => {
                console.log('ERROR CLIENTE:', e);
                toast(`Error al consultar`, { type: toast.TYPE.ERROR });
                setIsDataLoaded(false);
                setReadonlyFinal(false);
            });
    };
    const [typeSelected, setTypeSelected] = useState<string>('CÉDULA CIUDADANÍA');
    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Información Productos"
                subText=""
                title1="Agregar Producto"
                title2=""
            />
            <CHPaper title="Gestión Producto">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Typography className={classes.titleform} variant="h5" component="h6">Consultar Afiliado</Typography>
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="identificationType"
                            control={formularioConsulta.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tipo documento *"
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    value={value}
                                    onChange={(e: any) => {
                                        const selected = e;
                                        onChange(selected);
                                        setTypeSelected(e.currentTarget.innerText)
                                    }}
                                >
                                    {catalogState.IdentificationType?.map(
                                        (option: any) => (
                                            <MenuItem key={option.id} value={option.id}>
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        {
                            identificationType as number === 1 || identificationType as number === 2 || identificationType as number === 4 ? (
                                <Controller
                                    name="identification"
                                    control={formularioConsulta.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        pattern: /^[0-9]+$/g
                                    }}

                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMask
                                            error={!!error}
                                            helperText={error ? "El número de identificación no debe contener letras" : null}
                                            label="Número de Identificación"
                                            value={value}
                                            onChange={onChange}
                                            type="number"
                                        />
                                    )}
                                />
                            ) : (
                                <Controller
                                    name="identification"
                                    control={formularioConsulta.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        pattern: /^[aA-zZ0-9]+$/g
                                    }}
                                    render={({
                                        field: { onChange, value },

                                    }) => (
                                        <CHTextField
                                            label="Número de Identificación"
                                            value={value}
                                            type='text'
                                            InputLabelProps={{ shrink: true }}
                                            onChange={value => {
                                                const newValor = value.target.value
                                                if (newValor.length <= 20) {
                                                    onChange(
                                                        value.target.value.replace(
                                                            /[ñÑáéíóúÁÉÍÓÚ\W]/g,
                                                            '',
                                                        ),
                                                    );
                                                }
                                            }}
                                        />
                                    )}
                                />
                            )
                        }
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={formularioConsulta.handleSubmit(handleSearchButton)}
                        >
                            CONSULTAR AFILIADO
                        </Button>
                    </Grid>
                </Grid>
                <LoadingComponent loading={isDataLoaded} />

                {
                    mostrarFormularios ?
                        <InfoNewProduct
                            dataUsuario={cliente}
                            categoriaSalario={categoriaSalario}
                            salarioMinimoAnual={salarioMinimoAnual}
                            setSalarioMinimoAnual={setSalarioMinimoAnual}
                            isDataLoaded={isDataLoaded}
                            loginState={loginState}
                            catalogState={catalogState}
                            simuladorState={simuladorState}
                            starGetCreditLine={starGetCreditLine}
                            setReadonlyFinal={setReadonlyFinal}
                            readonlyFinal={readonlyFinal}
                            nuevosDatos={nuevosDatos?.clientIdentificationNumber}
                            setTituloBoton={setTituloBoton}
                            tituloBoton={tituloBoton}
                            setMostrarTablaSegunFormulario={setMostrarTablaSegunFormulario}
                            mostrarTablaSegunFormulario={mostrarTablaSegunFormulario}
                        /> : null
                }



            </CHPaper>
        </CHPrivateContainer>
    );
};
interface LinkMapStateToProps {
    loginState: LoginState;
    catalogState: CatalogState;
    clienteState: ClientState;
    productoState: ProductState;
    simuladorState: SimuladorGeneralState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    clienteState: state.client,
    productoState: state.producto,
    simuladorState: state.simuladorGeneral,
});

interface LinkMapDispatchToProps {
    startGetClientByIdentificationTypeIdentificationNumber: (
        props: string,
        idType: number,
        idNumber: string
    ) => void;
    startGetAllTipoCredito: (data: string) => void;
    startGetAllTiVivienda: (data: string) => void;
    startGetAllTipoVivienda: (data: string) => void;
    startGetAllEstadoVivienda: (data: string) => void;
    startGetAllIdentificationTypes: (data: string) => void;
    startGetCategory: (data: string) => void;
    startGetAllForces: (data: string) => void;
    startGetAllDepartments: () => void;
    startGetAllCities: () => void;
    starGetCreditLine: (data: number) => void;
    startGetSimuladorGeneral: () => void;
}
const mapDispatchToProps = {
    startGetClientByIdentificationTypeIdentificationNumber,
    startGetAllTipoCredito,
    startGetAllTiVivienda,
    startGetAllTipoVivienda,
    startGetAllEstadoVivienda,
    startGetAllIdentificationTypes,
    startGetCategory,
    startGetAllForces,
    startGetAllDepartments,
    startGetAllCities,
    starGetCreditLine,
    startGetSimuladorGeneral,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;
export default connect(mapStateToProps, mapDispatchToProps)(Productos);
function async(arg0: string, identificationType: number, identification: any) {
    throw new Error('Function not implemented.');
}

