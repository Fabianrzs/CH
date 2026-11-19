import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Card,
    CardContent,
    Typography,
} from '@material-ui/core';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import '../cv.scss';
import { ProductoDto } from '../../../types/producto.model.type';
import { ProductForm } from '../constants/productoform.model.type';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { ProductState } from '../../../reducers/producto.reducer';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import ViewEditProductData from './ViewEditProductData';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import {
    startGetAllIdentificationTypes,
    startGetAllTipoVivienda,
    startGetAllTipoCredito,
} from '../../../actions/catalog.action';
import {startGetProductoByIdentification,startGetProductosByIdentification} from '../../../actions/producto.action';
import { LoginState } from '../../../reducers/login.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import apiClient from '../../../services/api-clients.service';
import ApiproductoClients from '../../../services/api-producto.service';
import { ModalGenerarExtracto } from './modalConsultaCartera/ModalGenerarExtractos';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import ContainerTitle from '../../../components/containe-title/container-title';
import TituloIcono from '../../AdministracionCuentas/components/titulo-icono/tituloIcono.component';
import { ModalGenerarExtractoCredito } from './modalConsultaCartera/ModalGenerarExtractoCredito';
import { ModalGenerarCertificadoRetefuente } from './modalConsultaCartera/ModalGenerarCertificadoRetefuente';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalogApi from '../../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            marginLeft: '0%'
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
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

function ProductoClientTable({
    data,
    isDataLoaded,
    onClickItem,
    catalogState,
    formularioConsulta
}: {
    data: ProductoDto[];
    isDataLoaded: boolean;
    onClickItem: (data: ProductoDto) => void;
    catalogState: CatalogState;
    formularioConsulta: any;
}) {
    const classes = useStyles();
    const [loading, setLoading] = useState(false);
    const [openModal, setOpenModal] = useState(false);
    const [openModalRetefuente, setOpenModalRetefuente] = useState(false);
    const [selectedProduct, setSelectedProduct] = useState(false);
    const [tipoCredito, settipoCredito] = useState<string|undefined>('');

    const handleOpenModal = (product: any) => {
        settipoCredito(catalogState.tipoCredito.find(v=>v.tcR_ID===product.idTipoCredito)?.tcR_DESCRIPCION)
        setSelectedProduct(product);
        setOpenModal(true);
    };
    const handleCloseModal = () => setOpenModal(false);

    const handleOpenModalRetefuente = (product: any) => {
        settipoCredito(catalogState.tipoCredito.find(v=>v.tcR_ID===product.idTipoCredito)?.tcR_DESCRIPCION)
        setSelectedProduct(product);
        setOpenModalRetefuente(true);
    };
    const handleCloseModalRetefuente = () => setOpenModalRetefuente(false);
    
    if (!isDataLoaded) {
        return null;
    }

    console.log({data})

    if (data.length === 0) {

        return <Typography style={{padding:'20px'}}> <br /> No se encontraron resultados <br /></Typography>;
    }

    return (
        <> 
            <ModalGenerarExtractoCredito
                open={openModal}
                handleClose={handleCloseModal}
                producto={selectedProduct}
                tipoCredito={tipoCredito}
            />

            <ModalGenerarCertificadoRetefuente
                open={openModalRetefuente}
                handleClose={handleCloseModalRetefuente}
                producto={selectedProduct}
                tipoCredito={tipoCredito}
            />
            <LoadingComponent loading={loading} />
            <table
                className="client-table"
                style={{
                    margin: '20px',
                    width: '95%',
                }}
            >
                <thead>
                    <tr>
                        <th> </th>
                        <th> </th>
                        <th> </th>
                        <th style={{ textAlign: 'center' }}>Estado Vivienda</th>
                        <th style={{ textAlign: 'center' }}>Tipo Crédito</th>
                        <th style={{ textAlign: 'center' }}>Estado Crédito</th>
                    </tr>
                </thead>
                <tbody>
                    {data?.map((row, index) => (
                        <tr key={`tr${index}`}>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => {
                                        setLoading(true);
                                        onClickItem(row);
                                    }}
                                >
                                   Detalle 
                                </Button>
                            </td>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => handleOpenModal(row)}
                                >
                                   Extracto 
                                </Button>
                            </td>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => handleOpenModalRetefuente(row)}
                                >
                                   Retefuente 
                                </Button>
                            </td>
                            {catalogState.tipoVivienda?.map(item => {
                                if (item.id === row.idTipoVivienda) {
                                    return (
                                        <td key={item.id}>
                                            {item.descripcion}
                                        </td>
                                    );
                                }
                                return null;
                            })}

                            {catalogState.tipoCredito?.map(item => {
                                if (item.tcR_ID === row.idTipoCredito) {
                                    
                                    return (
                                        <td key={item.tcR_ID}>
                                            {item.tcR_DESCRIPCION}
                                        </td>
                                    );
                                }
                                return null;
                            })}
                            
                                        <td >
                                            {row.estadoCredito}
                                        </td>
                            
                        </tr>
                    ))}
                </tbody>
            </table>
        </>
    );
}

const SearchInfo = (props: Props) => {
    const classes = useStyles();
    const api = apiClient();
    const apiProducto = ApiproductoClients();
    const {
        productoState,
        catalogState,
        loginState,
        startGetAllIdentificationTypes,
        startGetAllTipoVivienda,
        startGetAllTipoCredito,
        startGetProductoByIdentification,
        startGetProductosByIdentification
    } = props;

    const [tipoDocumento, setTipoDocumento] = useState(1);
    const [documento, setDocumento] = useState('');
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [selectedData, setSelectedData] = useState<ProductoDto | null>(null);
    const [cliente, setCliente] = useState<ClienteDto | null>(null);
    const [isLoading, setIsLoading] = useState (false);

    const formularioConsulta = useForm({
        defaultValues: {
            tipoIdentificacion: '1',
            identificacionCliente: '',
        },
    });

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllTipoVivienda(loginState.claims);
        startGetAllTipoCredito(loginState.claims);
    }, []);

    const [typeSelected, setTypeSelected] = useState<string>('CÉDULA CIUDADANÍA');

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
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

    const handleSearchButton = async (data: any) => {
        const auditoriaApi=AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
                
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Consulta de cartera',
            hostname:document.location.host
            } as Auditoria
            
        startGetProductosByIdentification(
            data.tipoIdentificacion,
            data.identificacionCliente,
        )
        requestAuditoria.tipoAccion='CONSULTAS';
        requestAuditoria.idTipoAccion=3;
        requestAuditoria.descripcion=`Consultar productos de cliente por tipo y número de identificación parámetros:
        \n Tipo de identificación:  ${ typeSelected}
        \n Identificación:  ${data.identificacionCliente}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
        obtenerDatosCliente(
            data.tipoIdentificacion,
            data.identificacionCliente,
        );
        setIsDataLoaded(true);
    };


    const obtenerDatosCliente = async (
        identificationType: number,
        identificacionCliente: string,
    ) => {
        const auditoriaApi=AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
        
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Consulta de cartera',
            hostname:document.location.host
        } as Auditoria
        setIsLoading(true)
        await api
            .getClientByIdentificationTypeIdentificationNumberRequest(
                identificationType,
                identificacionCliente,
            )
            .then(async response => {
                const datos = response.data as ClienteDto;
                setCliente(datos);
                setIsLoading(false)
                requestAuditoria.tipoAccion='CONSULTAS';
            requestAuditoria.idTipoAccion=3;
            requestAuditoria.descripcion=`Obtener afiliado por tipo y número de identificación en agregar producto parámetros:
            \n Tipo de identificación:  ${typeSelected}
            \n Identificación:  ${identificacionCliente}`
            auditoriaApi.InsertAuditoria(requestAuditoria);
            })
            .catch((error:any) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setIsLoading(false)
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Obtener afiliado por tipo y número de identificación en agregar producto parámetros:
                \n Tipo de identificación:  ${typeSelected}
                \n Identificación:  ${identificacionCliente}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            });
    };

    const onItemIsSelected = (data: ProductoDto | null) => {
        setSelectedData(data);

        startGetProductoByIdentification(
            data?.tipoIdentificacion ?? 0,
            data?.identificacion ?? '',
            data?.idCredito,
        )

    };

    const onItemGoBack = () => {
        setSelectedData(null);
        setIsDataLoaded(false);
        setCliente(null);
        setTipoDocumento(1);
        setDocumento('');
        formularioConsulta.setValue('tipoIdentificacion', '1');
        formularioConsulta.setValue('identificacionCliente', '');
    };

    
    if (selectedData != null && cliente != null) {
        
        return (
            <ViewEditProductData
                onItemGoBack={onItemGoBack}
                productoRequest={selectedData}
                idType={tipoDocumento}
                idNumber={documento}
                clienteRequest={cliente}
            />
        );
    }

    return (

        <div className={classes.root}>
                <LoadingV2Component 
                key={`loader_${isLoading}_${productoState.loading}`}
                loading={isLoading || productoState.loading}
                tittle={isLoading ? 'Consultando cliente...' :  'Cargando producto...'}
                />
                <PageIndicators
                    topTextNivel1='Producto Clientes'
                    topTextNivel2='Gestion de Cartera' 
                />

                <IconTitle
                    title='Consulta'
                    subTitle='Cartera'
                    icon='fch-Buscar-0-1'
                />
                {/* <LoadingComponent key="loader" loading={productoState.loading} /> */}
                
                <Grid >
                    <Grid item xs={12}>
                        <Card>
                            <ContainerTitle title="Consulta de producto por cliente"/>
                            

                                <CardContent>


                                <Grid container justifyContent="space-around">
                                    <Grid item xs={3}>
                                        <TituloIcono
                                            title1="Tipo"
                                            subtitle="Identificación"
                                            icono="fact_check"
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Controller
                                            name="tipoIdentificacion"
                                            control={formularioConsulta.control}
                                            rules={{
                                                required: 'Este campo es requerido',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHSelect
                                                    label="Tipo de Identificación *"
                                                    value={value}
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    onChange={(e:any) => {
                                                        setTipoDocumento(
                                                            e.target.value as number,
                                                        );
                                                        onChange(e);
                                                        setTypeSelected(e.currentTarget.innerText as string)
                                                    }}
                                                >
                                                    {catalogState.IdentificationType?.map(
                                                        (option: IDENTIFICATIONTYPEDTO) => (
                                                            <MenuItem
                                                                key={option.id}
                                                                value={option.id}
                                                            >
                                                                {option.descripcion}
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
                                            title1="Numero"
                                            subtitle="Identificación"
                                            icono="event_available"
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Controller
                                            name="identificacionCliente"
                                            control={formularioConsulta.control}
                                            rules={{
                                                pattern: {
                                                    value: /^[0-9]+$/g,
                                                    message: 'Este campo es requerido y debe contener solo números'
                                                },
                                                required: {
                                                    value: true,
                                                    message: 'Este campo es requerido',
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
                                                    label="Número de Identificación *"
                                                    value={value}
                                                    onChange={e => {
                                                        if (
                                                            tipoDocumento === 1 ||
                                                            tipoDocumento === 2 ||
                                                            tipoDocumento === 5
                                                        ) {
                                                            if (e.target.value.length <= 12) {
                                                                onChange(
                                                                    e.target.value.replace(/[^\d]/g, '') // Reemplaza cualquier cosa que no sea un dígito
                                                                );
                                                            }
                                                        } else {
                                                            onChange(e);
                                                        }
                                                        setDocumento(e.target.value.replace(/[^\d]/g, '')); // Reemplaza cualquier cosa que no sea un dígito
                                                    }}
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

                                    <div className="buttonssolicitud">

                                        <Button
                                        className={classes.buttonprincipal}
                                        variant="outlined"
                                        onClick={formularioConsulta.handleSubmit(
                                                handleSearchButton,
                                            )}
                                        >
                                        CONSULTAR PRODUCTO
                                        </Button>
                                    </div>

                                </CardContent>

                                <ProductoClientTable
                                    data={productoState.productos}
                                    formularioConsulta={formularioConsulta}
                                    onClickItem={onItemIsSelected}
                                    isDataLoaded={isDataLoaded}
                                    catalogState={catalogState}
                                />
                                
                        </Card>
                    </Grid>
                </Grid>
        </div>
    );
};
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
    startGetProductoByIdentification: (identificationType: number, idNumber: string, idProducto:any) => void;
    startGetProductosByIdentification: (identificationType: number, idNumber: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetAllTipoVivienda,
    startGetAllTipoCredito,
    startGetProductoByIdentification,
    startGetProductosByIdentification,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(SearchInfo);
