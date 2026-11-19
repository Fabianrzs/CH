import React, { useEffect, useState } from 'react';
import {
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow,
    Typography,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { AppState } from '../../../store/rootReducer';
import { startGetAllUnidadEjecutora } from '../../../actions/simulador.action';
import { startGetAllTipoCredito } from '../../../actions/catalog.action';
import { startGetAllAplicacionPago } from '../../../actions/producto.action';
import { LoginState } from '../../../reducers/login.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import {
    IDENTIFICATIONTYPEDTO,
    TIPOCREDITO,
} from '../../../types/catalog.type';
import { SimuladorState } from '../../../reducers/simulador.reducer';
import {
    AplicacionPago,
    ProductoDto,
} from '../../../types/producto.model.type';
import { UnidadEjecutoraDto } from '../../../types/simulador.model.type';
import { ProductState } from '../../../reducers/producto.reducer';
import UtilService from '../../../utils/util.service';
import { ClienteDto } from '../../../types/client.model.type';
import ApiproductoClients from '../../../services/api-producto.service';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import CatalogApi from '../../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const ObligationsQuery = (props: props) => {
    const apiProducto = ApiproductoClients();
    const classes = useStyles();
    const {
        idType,
        idNumber,
        // producto,
        productosSelec,
        clienteRequest,
        loginState,
        catalogState,
        simuladorState,
        productoState,
        startGetAllUnidadEjecutora,
        startGetAllTipoCredito,
        startGetAllAplicacionPago,
    } = props;

    const producto = productoState.productos[0];


    const [isLoading, setIsLoading] = useState(false);

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
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

    const formatDecimal = (value?: number | null) =>
        value != null ? value.toFixed(2).replace('.', ',') : '';

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
                        setLocalIpAddress((prevIp) => {
                            if (prevIp !== ip) {
                                enviarIp(ip);
                                return ip;
                            }
                            return prevIp;
                        });
                    }
                }
            };

            return () => peerConnection.close();
        };

        fetchLocalIPAddress();
    }, []);


    useEffect(() => {
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Consulta de cartera',
            hostname: document.location.host
        } as Auditoria
        requestAuditoria.tipoAccion = 'CONSULTAS';
        requestAuditoria.idTipoAccion = 3;
        requestAuditoria.descripcion = `Consultó pestaña Consulta de producto en botón Detalle  parámetros:            
            \n Identificación cliente:  ${clienteRequest.identificacion}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
    })
    return (
        <CHPrivateContainer>
            <LoadingV2Component key="loader" loading={isLoading} tittle='' />
            <CHPaper title="Consulta obligaciones">
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    />
                </Grid>


                <Grid container>

                    <Grid container>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Datos Crédito

                        </Typography>
                    </Grid>

                    <Grid container>





                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                name="numeroCredito"
                                label="Número de Crédito"
                                value={productoState.productos[0].numeroCredito}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHSelect
                                name="idTipoCredito"
                                label="Línea Crédito"
                                value={productoState.productos[0].idTipoCredito}
                                open={false}
                                readOnly
                            >
                                {catalogState.tipoCredito?.map(
                                    (option: TIPOCREDITO) => (
                                        <MenuItem
                                            key={option.tcR_ID}
                                            value={option.tcR_ID}
                                        >
                                            {option.tcR_DESCRIPCION}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        </Grid>




                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Línea Crédito"
                                name="lineaCredito"
                                value={productoState.productos[0].lineaCredito}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Estado"
                                name="estadoCredito"
                                value={productoState.productos[0].estadoCredito}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Fecha Desembolso"
                                name="fechaDesembolso"
                                value={UtilService.getDate(productoState.productos[0].fechaDesembolso)}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Valor Desembolso"
                                name="valorDesembolso"
                                value={formatDecimal(productoState.productos[0].valorDesembolso)}
                                currencyMaskDecimal
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Fecha Ultimo Pago"
                                name="fechaUltimoPago"
                                value={UtilService.getDate(productoState.productos[0].fechaUltimoPago)}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Calificación"
                                name="Calificacion"
                                InputLabelProps={{ shrink: true }}
                                value={productoState.productos[0].calificacion}
                            />
                        </Grid>

                    </Grid>

                    <Grid container style={{ marginTop: "40px" }}>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Información Final del Contrato

                        </Typography>
                    </Grid>

                </Grid>

                <Grid container>


                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMaskDecimal
                            inputProps={{
                                style: {
                                    textAlign: 'right'
                                }
                            }}
                            label="Saldo Capital remanente"
                            value={formatDecimal(productoState.productos[0].saldoCapitalRemanenteFinal)}
                            InputLabelProps={{ shrink: true }}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMaskDecimal
                            inputProps={{
                                style: {
                                    textAlign: 'right'
                                }

                            }}
                            label="Saldo interés remanente"
                            name="saldoInteresRemanente"
                            value={formatDecimal(productoState.productos[0].interesesRemanenteFinal)}
                            InputLabelProps={{ shrink: true }}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMaskDecimal
                            inputProps={{
                                style: {
                                    textAlign: 'right'
                                }
                            }}
                            label="Canon Extraordinario"
                            name="canonExtraordinario"
                            value={formatDecimal(productoState.productos[0].canonExtraordinarioFinal)}
                            InputLabelProps={{ shrink: true }}
                        />
                    </Grid>

                    <Grid container>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                name="valorOpcionCompra"
                                label="Valor Opcion Compra"
                                inputProps={{
                                    style: {
                                        textAlign: 'right'
                                    }

                                }}
                                value={formatDecimal(productoState.productos[0].valorOpcionCompra)}
                                currencyMaskDecimal
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                name="totalObligacion"
                                label="Valor Total Adeudado"
                                inputProps={{
                                    style: {
                                        textAlign: 'right'
                                    }

                                }}

                                value={formatDecimal(productoState.productos[0].valorTotalAdeudadoFinal)}
                                currencyMaskDecimal
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                name="valorVivienda"
                                label="valor de la Garantia"
                                inputProps={{
                                    style: {
                                        textAlign: 'right'
                                    }

                                }}
                                value={formatDecimal(productoState.productos[0].valorGarantia)}
                                currencyMaskDecimal
                            />
                        </Grid>
                    </Grid>
                </Grid>

            </CHPaper>
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    catalogState: CatalogState;
    simuladorState: SimuladorState;
    productoState: ProductState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    catalogState: state.catalog,
    simuladorState: state.simulador,
    productoState: state.producto,
});
interface LinkMapDispatchToProps {
    idType: number;
    idNumber: string;
    productosSelec: any;
    clienteRequest: ClienteDto;
    startGetAllUnidadEjecutora: () => void;
    startGetAllTipoCredito: (props: string) => void;
    startGetAllAplicacionPago: (idCredito: string) => void;
}

const mapDispatchToProps = {
    startGetAllUnidadEjecutora,
    startGetAllTipoCredito,
    startGetAllAplicacionPago,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ObligationsQuery);
