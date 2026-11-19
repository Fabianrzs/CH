import React, { useEffect, useState } from 'react';
import {
    createStyles,
    Grid,
    InputAdornment,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import {
    startGetAllTipoCredito,
    startGetAllTiVivienda,
    startGetAllTipoVivienda,
    startGetAllEstadoVivienda,
    startGetAllEstadoCartera,
    startGetCategory,
    startGetAllGrades,
    startGetAllForces,
    startGetAllExecutingUnits,
    startGetPayApplication,
    startGetAllCities,
} from '../../../actions/catalog.action';

import { startGetClientByIdentificationTypeIdentificationNumber } from '../../../actions/client.action';
import { startGetAllAplicacionPago, startGetProductoByIdentification } from '../../../actions/producto.action';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import {
    CATEGORIADTO,
    EXECUTINGUNITDTO,
    FORCEDTO,
    GRADEDTO,
    PAYDTO,
} from '../../../types/catalog.type';
import { AplicacionPago, ProductoDto } from '../../../types/producto.model.type';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import PayAplicationTable from './tablaApliacionPagos';
import { ProductState } from '../../../reducers/producto.reducer';
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
    }),
);

const ProductQuery = (props: any) => {
    const classes = useStyles();
    const {
        idType,
        idNumber,
        producto,
        clienteRequest,
        loginState,
        catalogState,
        productoState,
        productosSelec,

        startGetAllTipoCredito,
        startGetAllTiVivienda,
        startGetAllTipoVivienda,
        startGetAllEstadoVivienda,
        startGetAllEstadoCartera,
        startGetClientByIdentificationTypeIdentificationNumber,
        startGetAllForces,
        startGetAllExecutingUnits,
        startGetPayApplication,
        startGetAllAplicacionPago,
        startGetProductoByIdentification,
        startGetCategory,
        startGetAllGrades,
        startGetAllCities,
    } = props;

    const { control } = useForm({
        defaultValues: productoState.productos[0],
    });

    const [age, setAge] = useState<number | null>(null);
    const [isLoading, setIsLoading] = useState(false);


    // const objetoFiltrado = productoState.productos.find((objeto:any) => objeto.idCredito === producto.idCredito);

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
        if (catalogState.Force.length === 0) {
            startGetAllForces(loginState.claims);
        }
        if (catalogState.ExecutingUnit.length === 0) {
            startGetAllExecutingUnits(loginState.claims)
        }

        startGetPayApplication();

        if (catalogState.categories.length === 0) {
            startGetCategory();
        }
        if (catalogState.Grade.length === 0) {
            startGetAllGrades(loginState.claims)
        }
        if (catalogState.City.length === 0) {
            startGetAllCities()
        }

        startGetAllAplicacionPago(productoState.productos[0].idCredito);
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
        requestAuditoria.descripcion = `Consultó pestaña Consulta de Cartera en botón Detalle  parámetros:            
            \n Identificación cliente:  ${clienteRequest.identificacion}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
    }, []);


    useEffect(() => {
        const birthDate = new Date(clienteRequest.fechaNacimiento);
        const currentYear = new Date().getFullYear();
        const birthYear = birthDate.getFullYear();

        if (Number.isNaN(birthDate.getTime()) || birthYear < 1900 || birthYear > currentYear) {
            setAge(0);
            return;
        }

        let calculatedAge = currentYear - birthYear;
        const thisYearBirthday = new Date(currentYear, birthDate.getMonth(), birthDate.getDate());
        if (thisYearBirthday > new Date()) {
            calculatedAge -= 1;
        }

        setAge(calculatedAge);
    }, []);



    const obtenerDescripcionExtraordinario = (id: any) => {

        if (id === "1" || id === "2" || id === "5") {

            const data = catalogState.Pay.find((x: PAYDTO) => x.id.toString() === id)?.descripcion;
            return data
        }

        return "Sin Extraordinarios";
    }

    return (
        <CHPrivateContainer>
            <LoadingV2Component key="loader" loading={isLoading} tittle='' />
            <LoadingV2Component key="loader" loading={productoState.loading} tittle='' />
            <CHPaper title="Consulta Cartera">
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Datos Afiliado
                    </Typography>
                </Grid>

                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="identificacion"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Identificación"
                                    value={productoState.productos[0].identificacion}
                                    onChange={onChange}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <CHTextField
                            label="Nombre y apellidos"
                            name="nombre"
                            value={productoState.productos[0].nombreCliente}
                            InputLabelProps={{ shrink: true }}
                            isReadOnly
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="edad"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    name="edad"
                                    label="Edad"
                                    value={age !== null ? age : productoState.productos[0].edad}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="categoria"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Categoría"
                                    value={clienteRequest?.idCategoria}
                                    open={false}
                                    readOnly
                                >
                                    {catalogState.categories?.map(
                                        (option: CATEGORIADTO) => (
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

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="grado"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Cargo / Grado"
                                    value={clienteRequest?.idGrado}
                                    open={false}
                                    readOnly
                                >
                                    {catalogState.Grade?.map(
                                        (option: GRADEDTO) => (
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

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="fuerza"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    name="fuerza"
                                    label="Fuerza"
                                    value={clienteRequest?.idFuerza}
                                    open={false}
                                    readOnly
                                >
                                    {catalogState.Force?.map(
                                        (option: FORCEDTO) => (
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
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="unidadEjecutor"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    name="unidadEjecutor"
                                    label="Unidad Ejecutora"
                                    value={clienteRequest?.idUnidadEjecutora}
                                    open={false}
                                    readOnly
                                >
                                    {catalogState.ExecutingUnit?.map(
                                        (option: EXECUTINGUNITDTO) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container style={{ marginTop: "40px" }}>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Datos Vivienda
                    </Typography>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorVivienda"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Valor Vivienda"
                                    value={formatDecimal(productoState.productos[0].valorVivienda)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorViviendaSeguroHogar"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Valor Avaluo"
                                    value={formatDecimal(productoState.productos[0].valorViviendaSeguroHogar)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>



                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            label="Tipo"
                            value={productoState.productos[0].visOrNVis}
                            InputLabelProps={{ shrink: true }}
                            isReadOnly
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            label="Estado"
                            value={productoState.productos[0].tipoVivienda}
                            InputLabelProps={{ shrink: true }}
                            isReadOnly
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="claseInmueble"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    name="claseInmueble"
                                    label="Clase inmueble"
                                    value={productoState.productos[0].claseInmueble}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="noMatricula"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    name="noMatricula"
                                    label="Matricula Inmueble"
                                    value={productoState.productos[0].noMatricula}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="escritura"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Escritura Inmueble"
                                    value={productoState.productos[0].escritura}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaEscritura"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Escritura"
                                    value={UtilService.getDate(productoState.productos[0].fechaEscritura)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="notaria"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Notaria"
                                    value={productoState.productos[0].notaria}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="linderos"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Linderos Inmueble"
                                    value={productoState.productos[0].linderos}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={9}>
                        <Controller
                            name="direccionInmueble"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Dirección Inmueble"
                                    value={productoState.productos[0].direccionInmueble}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudadInmueble"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            height: '38px',
                                        },
                                    }}
                                    label="Ciudad Inmueble"
                                    value={catalogState.City.find((x: any) => x.id === Number(productoState.productos[0].ciudadInmueble))?.nombre}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={9}>
                        <Controller
                            name="descripcionInmueble"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    multiline
                                    rows={2}
                                    label="Descripción Inmueble"
                                    value={productoState.productos[0].descripcionInmueble}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid container style={{ marginTop: "40px" }}>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Datos Crédito

                        </Typography>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroCredito"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Número de Crédito"
                                    value={productoState.productos[0].numeroCredito}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            label="Tipo Crédito"
                            value={productoState.productos[0].tipoCredito}
                            InputLabelProps={{ shrink: true }}
                            isReadOnly
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="lineaCredito"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Linea Crédito"
                                    value={productoState.productos[0].lineaCredito}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="score"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Puntaje Buro de Crédito"
                                    value={productoState.productos[0].score}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}

                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaSolicitud"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Solicitud Inicial"
                                    value={UtilService.getDate(productoState.productos[0].fechaSolicitud)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaDesembolso"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Desembolso Final"
                                    value={UtilService.getDate(productoState.productos[0].fechaDesembolso)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaEstadoCredito"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Estado Crédito"
                                    value={UtilService.getDate(productoState.productos[0].fechaEstadoCredito)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}




                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ultimoPagoInteresCorriente"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Ultimo Pago Interes Corriente"
                                    value={UtilService.getDate(productoState.productos[0].ultimoPagoInteresCorriente)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="diaPago"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Día Pago"
                                    value={productoState.productos[0].diaPago}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaUltimoPago"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Ultimo Pago"
                                    value={UtilService.getDate(productoState.productos[0].fechaUltimoPago)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>


                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaProximoPago"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Próximo Pago"
                                    value={UtilService.getDate(productoState.productos[0].fechaProximoPago)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaFinPagos"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Fecha Fin de Pagos"
                                    value={UtilService.getDate(productoState.productos[0].fechaFinPagos)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorDesembolso"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}

                                    label="Valor Desembolso"
                                    value={formatDecimal(productoState.productos[0].valorDesembolso)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorCanonInicial"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Canon Inicial"
                                    value={formatDecimal(productoState.productos[0].valorCanonInicial)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoAbonoExtra"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Tipo Abono Extra"
                                    value={obtenerDescripcionExtraordinario(productoState.productos[0].tipoAbonoExtra)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorAbonoExtra"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Valor Abono Extra"
                                    value={formatDecimal(productoState.productos[0].valorAbonoExtra) ?? 0}
                                    // value={obtnerProductoTablaFilter[0]?.valorPago.toFixed(0) ?? 0}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tasaCreditoEA"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly
                                    label="Tasa Crédito EA (%)"
                                    InputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                {' '}
                                                % EA   {' '}
                                            </InputAdornment>
                                        ),
                                    }}
                                    value={((productoState.productos[0].tasaCreditoEA * 100)).toFixed(2)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}


                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tasaCreditoPer"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly
                                    label="Tasa Periódica (%)"

                                    InputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                {'    '}
                                                NMMV  {' '}
                                            </InputAdornment>
                                        ),
                                    }}
                                    value={(productoState.productos[0].tasaCreditoPer * 100).toFixed(5)}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}

                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorCuota"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}

                                    label="Valor Cuota"
                                    value={formatDecimal(productoState.productos[0].valorCuota)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <div className="campos MuiFormControl-marginNormal">
                            <div className="control">
                                <div className="label MuiFormLabel-root">

                                    Plazo Inicial / Cuotas Pagadas / Cuotas Pendientes

                                </div>
                                <div className="input">
                                    <Controller
                                        name="plazoTotal"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                isReadOnly
                                                value={productoState.productos[0].plazoTotal}
                                                onChange={onChange}
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                inputProps={{
                                                    style: {
                                                        borderTop:
                                                            'none !important',
                                                        borderRight:
                                                            'none !important',
                                                        borderLeft:
                                                            'none !important',
                                                        outline: 'none',
                                                        textAlign: 'center',
                                                    },
                                                }}
                                            />
                                        )}
                                    />
                                </div>
                            </div>
                            <div className="control">
                                <div className="value">/</div>
                            </div>
                            <div className="control">
                                <div className="input">
                                    <Controller
                                        name="numeroCuotasCanceladas"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                isReadOnly
                                                value={productoState.productos[0].numeroCuotasCanceladas}
                                                onChange={onChange}
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                inputProps={{
                                                    style: {
                                                        borderTop:
                                                            'none !important',
                                                        borderRight:
                                                            'none !important',
                                                        borderLeft:
                                                            'none !important',
                                                        outline: 'none',
                                                        textAlign: 'center',
                                                    },
                                                }}
                                            />
                                        )}
                                    />
                                </div>
                            </div>
                            <div className="control">
                                <div className="value">/</div>
                            </div>
                            <div className="control">
                                <div className="input">
                                    <Controller
                                        name="plazoActual"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                isReadOnly
                                                value={productoState.productos[0].plazoActual}
                                                onChange={onChange}
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                inputProps={{
                                                    style: {
                                                        borderTop:
                                                            'none !important',
                                                        borderRight:
                                                            'none !important',
                                                        borderLeft:
                                                            'none !important',
                                                        outline: 'none',
                                                        textAlign: 'center',
                                                    },
                                                }}
                                            />
                                        )}
                                    />
                                </div>
                            </div>
                        </div>
                    </Grid>

                    <Grid container style={{ marginTop: "40px" }}>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Seguros

                        </Typography>
                    </Grid>


                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="tasaSeguroVida"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Seguros Vida con extraprima (%)"
                                        value={productoState.productos[0].tasaSeguroVida}
                                        type="number"
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>

                            <CHTextField
                                isReadOnly
                                inputProps={{ style: { textAlign: 'right', }, }}
                                label="Seguro Vida"
                                value={formatDecimal(productoState.productos[0].valSeguroVida)}
                                currencyMaskDecimal
                                InputLabelProps={{ shrink: true }}

                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>

                            <CHTextField
                                isReadOnly
                                inputProps={{ style: { textAlign: 'right', }, }}
                                label="Acumulado Seguro Vida"
                                value={formatDecimal(productoState.productos[0].acumuladoSeguroVida)}
                                currencyMaskDecimal
                                InputLabelProps={{ shrink: true }}

                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="ultimoPagoSeguroVida"
                                control={control}
                                rules={{
                                    required: 'Este campo requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        label="Fecha Ultimo Pago Seguro Vida"
                                        value={UtilService.getDate(productoState.productos[0].ultimoPagoSeguroVida)}
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>

                    <Grid container>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="porcentajeSeguroVida"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Seguro Vida (%)"
                                        value={productoState.productos[0].porcentajeSeguroVida}
                                        type="number"
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="porcentajeExtraPrimaSeguroVida"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        label="Extraprima (%)"
                                        InputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                            endAdornment: (
                                                <InputAdornment position="end">
                                                    %
                                                </InputAdornment>
                                            ),
                                        }}
                                        value={productoState.productos[0].porcentajeExtraPrimaSeguroVida}
                                        type="number"
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="aseguradoraVida"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        label="Aseguradora"
                                        value={productoState.productos[0].aseguradoraVida}
                                        type="number"
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>

                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="tasaSeguroHogar"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Seguros Hogar (%)"
                                        value={productoState.productos[0].tasaSeguroHogar.toFixed(5)}
                                        type="number"
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>

                            <CHTextField
                                isReadOnly
                                inputProps={{ style: { textAlign: 'right', }, }}
                                label="Seguro Hogar"
                                value={formatDecimal(productoState.productos[0].valSeguroHogar)}
                                currencyMaskDecimal
                                InputLabelProps={{ shrink: true }}

                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                inputProps={{ style: { textAlign: 'right', }, }}
                                label="Acumulado Seguro Hogar"
                                value={formatDecimal(productoState.productos[0].acumuladoSeguroHogar)}
                                currencyMaskDecimal
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="ultimoPagoSeguroHogar"
                                control={control}
                                rules={{
                                    required: 'Este campo requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        label="Fecha Ultimo Pago Seguro Hogar"
                                        value={UtilService.getDate(productoState.productos[0].ultimoPagoSeguroHogar)}
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>



                    </Grid>


                    <Grid container style={{ marginTop: "40px" }}>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Información al Corte

                        </Typography>
                    </Grid>

                    <Grid container>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="saldoCapital"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Saldo Capital"
                                        value={formatDecimal(productoState.productos[0].saldoCapital)}
                                        onChange={onChange}
                                        currencyMaskDecimal
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>


                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="interesRemanente"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Interes Remanente"
                                        value={formatDecimal(productoState.productos[0].interesesRemanenteCorte)}
                                        onChange={onChange}
                                        currencyMaskDecimal
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>

                        {/* <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="interesMora"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Interes Corriente"
                                        value={productoState.productos[0].interesMora || 0}
                                        onChange={onChange}
                                        currencyMask
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid> */}

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="saldoTotal"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Saldo a fin de mes"
                                        value={formatDecimal(productoState.productos[0].saldoTotalCorte)}
                                        onChange={onChange}
                                        currencyMaskDecimal
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>

                    </Grid>

                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="CapitalRemanenteCorte"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Capital Remanente"
                                        value={formatDecimal(productoState.productos[0].capitalRemanenteCorte)}
                                        onChange={onChange}
                                        currencyMaskDecimal
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>


                    <Grid container style={{ marginTop: "40px" }}>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Informacion Mora

                        </Typography>
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="diasMora"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Días Mora"
                                    value={productoState.productos[0].diasMora}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="saldoCapitalMora"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Capital en Mora"
                                    value={formatDecimal(productoState.productos[0].saldoCapitalMora)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="acumuladoInteresMora"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Interés Mora"
                                    value={formatDecimal(productoState.productos[0].acumuladoInteresMora)}
                                    onChange={onChange}
                                    currencyMaskDecimal
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ultimoPagoInteresMora"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Ultima fecha Mora"
                                    value={
                                        UtilService.getDate(value) ===
                                            '01-01-1900'
                                            ? ''
                                            : UtilService.getDate(productoState.productos[0].ultimoPagoInteresMora)
                                    }
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>




                    <Grid container style={{ marginTop: "40px" }}>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            Beneficios y Alivios

                        </Typography>
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tasaFrech"
                            control={control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Alivio en Tasa (Frech) %"
                                    value={productoState.productos[0].tasaFrech}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField

                            label="Alivio en cuota (Frech)"
                            value={formatDecimal(productoState.productos[0].valorAlivioCuota)}
                            InputLabelProps={{ shrink: true }}
                            inputProps={{ style: { textAlign: 'right', }, }}
                            isReadOnly
                            currencyMaskDecimal
                        />
                    </Grid>


                </Grid >

                <Grid container style={{ marginTop: "40px" }}>

                    <PayAplicationTable
                        producto={productoState.productos[0]}
                        pagos={productoState.aplicacionPagos}
                        productoState={productoState}
                        productosSelec={productoState.productos[0]}
                        catalogState={catalogState}

                    />
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    );
};
interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
    productoState: ProductState;
    ProductoDto: any;
}



const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    catalogState: state.catalog,
    productoState: state.producto,

});

interface LinkMapDispatchToProps {
    idType: number;
    idNumber: string;
    producto: ProductoDto;
    clienteRequest: ClienteDto;
    productoState: any;
    productosSelec: any;
    productos: ProductoDto;

    startGetAllTipoCredito: (data: string) => void;
    startGetAllTiVivienda: (data: string) => void;
    startGetAllTipoVivienda: (data: string) => void;
    startGetAllEstadoVivienda: (data: string) => void;
    startGetAllEstadoCartera: (data: string) => void;
    startGetCategory: () => void;
    startGetAllGrades: (props: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        props: string,
        idType: number,
        idNumber: string,
    ) => void;
    startGetAllForces: (props: string) => void;
    startGetAllExecutingUnits: (props: string) => void;
    startGetPayApplication: () => void;
    startGetAllAplicacionPago: (idCredito: string) => void;
    startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
    startGetAllCities: () => void;

}

const mapDispatchToProps = {
    startGetAllTipoCredito,
    startGetAllTiVivienda,
    startGetAllTipoVivienda,
    startGetAllEstadoVivienda,
    startGetAllEstadoCartera,
    startGetCategory,
    startGetAllGrades,
    startGetClientByIdentificationTypeIdentificationNumber,
    startGetAllForces,
    startGetAllExecutingUnits,
    startGetPayApplication,
    startGetAllAplicacionPago,
    startGetProductoByIdentification,
    startGetAllCities


};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ProductQuery);

