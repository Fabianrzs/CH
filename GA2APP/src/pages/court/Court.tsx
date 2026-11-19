import React, { useEffect, useState, useCallback } from 'react';
import {
    Button,
    createStyles,
    Grid,
    IconButton,
    InputAdornment,
    makeStyles,
    Menu,
    MenuItem,
} from '@material-ui/core';
import ClearIcon from '@material-ui/icons/Clear';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { connect, useDispatch } from 'react-redux';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import { AppState } from '../../store/rootReducer';
import TableCourt from './TableCourt';
import { startPostInsertCourt, startPostUpdateCourt } from '../../actions/courts.action';
import { CITYDTO, DEPARTAMENTODTO, TIPOCUENTA } from '../../types/catalog.type';
import CHSelect from '../../components/ch-select/CHSelect';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../components/ch-paper/CHPaper';
import ApiCatalog from '../../services/api-catalog.service';
import ApiCourt from '../../services/api-courts.service';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import { despachosJudicialesForm } from '../embargos/constants/embargosform.model.type';
import { DespachosJudiciales, JuzgadoDto } from '../../types/court.model.type';
import { juzgadoForm } from './constants/courtform.model.type';
import { GUID_DEFAULT } from '../../constants/constants';
import CHAutoComplete from '../../components/ch-auto-complete/CHAutoComplete';
import NotificationService from '../../services/notification.service';
import { LoadingComponent } from '../../components/loading/loading.component';
import { ModalCourtAdd } from './components/ModalCourtAdd';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise'


const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const Court = ({ startPostInsertCourt, startPostUpdateCourt }: props) => {
    const classes = useStyles();
    const api = ApiCatalog();
    const apiCourt = ApiCourt();
    const dispatch = useDispatch();

    const [departaments, setDepartaments] = useState<DEPARTAMENTODTO[]>([]);
    const [cities, setCities] = useState<CITYDTO[]>([]);
    const [openModalAdd, setOpenModalAdd] = useState(false);
    const [informacion, setInformacion] = useState<JuzgadoDto[]>([]);
    const [departamentValue, setDepartamentValue] = useState<DEPARTAMENTODTO | null>(null);
    const [cityValue, setCityValue] = useState<CITYDTO | null>(null);
    const [loading, setLoading] = useState(false);
    const [allTipoActor, setAllTipoActor] = useState<any[]>([]);
    const [estadoJuzgado, setEstadoJuzgado] = useState<number>();
    const [estados] = useState<any[]>([
        { idTipoCuenta: 0, descripcionTipoCuenta: "INACTIVO" },
        { idTipoCuenta: 1, descripcionTipoCuenta: "ACTIVO" },
    ]);
    const [departamentoNombre, setDepartamentoNombre] = useState<string | null>(null);
    const [ciudadNombre, setCiudadNombre] = useState<string | null>(null);


    const formularioDespachosJudiciales = useForm<DespachosJudiciales>({
        defaultValues: despachosJudicialesForm,
    });


    const formulario = useForm<JuzgadoDto>({
        defaultValues: juzgadoForm,
    });

    const formularioActor = useForm<JuzgadoDto>({
        defaultValues: juzgadoForm,
    });

    useEffect(() => {
        const fetchInitialData = async () => {
            await obtenerDepartamentos();
            await obtenerTipoActorEmbargo();
        };
        fetchInitialData();
    }, []);

    const obtenerDepartamentos = useCallback(async () => {
        try {
            const response = await api.getAllStates();
            const datos = response.data as DEPARTAMENTODTO[];
            setDepartaments(datos.filter(item => item.id !== 0));
        } catch {
            toast("Error al obtener los departamentos", { type: toast.TYPE.ERROR, autoClose: 5000 });
        }
    }, [api]);

    const obtenerCiudades = useCallback(async (idDepto: number | null) => {
        if (idDepto != null) {
            try {
                const response = await api.getCityOfState(idDepto.toString());
                setCities(response.data as CITYDTO[]);
            } catch {
                toast("Error al obtener las ciudades", { type: toast.TYPE.ERROR, autoClose: 5000 });
            }
        }
    }, [api]);

    const obtenerTipoActorEmbargo = useCallback(async () => {
        try {
            const response = await api.obtenerTipoActorEmbargo();
            if (response.isSuccess) {
                setAllTipoActor(response.data);
            } else {
                setAllTipoActor([]);
            }
        } catch {
            toast("Error al obtener los tipos de actor", { type: toast.TYPE.ERROR, autoClose: 5000 });

        }
    }, [api]);



    const handleSearch = useCallback(async () => {
        setLoading(true);
        try {
            const data = formularioDespachosJudiciales.getValues();
            setEstadoJuzgado(data.Estado);

            if (data.idDepartamento === 0) {
                data.idDepartamento = null
                data.idCiudad = null
            }
            if (data.idCiudad === 0) {
                data.idCiudad = null
            }
            await Search(data);
            console.log("Búsqueda realizada correctamente.");
        } catch (error) {
            console.error("Error en handleSearch:", error);
            NotificationService.genericAlertError("ERROR", "Error al realizar la búsqueda.");
        } finally {
            setLoading(false);
        }
    }, [formularioDespachosJudiciales, apiCourt]);

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
    const catalogApi = ApiCatalog();

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
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    } else {
                        setLocalIpAddress(ip);
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


    // const Search = async (data: any) => {
    //     try {
    //         const response = await apiCourt.postAllCourtsByCityCode(data);
    //         console.log("Datos recibidos de la API:", response.data);
    //         const resultados = response.data as JuzgadoDto[];
    //         setInformacion(response.data as JuzgadoDto[]);

    //         const coincidencia = resultados.find(
    //             (item) =>
    //                 item.idDepartamento === data.idDepartamento &&
    //                 item.idCiudad === data.idCiudad
    //         );

    //         if (coincidencia) {
    //             setDepartamentoNombre(coincidencia.departamento);
    //             setCiudadNombre(coincidencia.ciudad);
    //         } else {
    //             setDepartamentoNombre(null);
    //             setCiudadNombre(null);
    //         }

    //         const auditoriaApi = AuditoriaApi();
    //         const user = AuthService.getUserInfo();
    //         const nav = window.navigator as any;
    //         const usrAg = nav.userAgentData.brands.find(
    //             (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    //         )?.brand;

    //         const { Estado } = formularioDespachosJudiciales.getValues();
    //         const { CuentaDeposito } = formularioDespachosJudiciales.getValues();
    //         const { codigo } = formularioDespachosJudiciales.getValues();

    //         let descripcionEstado = 'No especificado';

    //         if (Estado === 1) {
    //             descripcionEstado = 'ACTIVO';
    //         } else if (Estado === 0) {
    //             descripcionEstado = 'INACTIVO';
    //         }

    //         const requestAuditoria = {
    //             navegador: usrAg,
    //             sistemaOperativo: nav.userAgentData.platform,
    //             nombreUsuario: user.USR_USERNAME,
    //             usurioId: user.USR_ID,
    //             ip: publicIpAddress,
    //             modulo: 'Despachos Judiciales',
    //             hostname: document.location.host,
    //             tipoAccion: 'CONSULTAS',
    //             idTipoAccion: 3,
    //             descripcion: `Consulta de despachos judiciales con los siguientes filtros:
    //             - Departamento: ${departamentoNombre || 'No especificada'}
    //             - Ciudad: ${ciudadNombre || 'No especificado'}
    //             - Código: ${codigo || 'No especificado'}
    //             - Cuenta de depósito: ${CuentaDeposito || 'No especificado'}
    //             - Estado: ${descripcionEstado || 'No especificado'}`,
    //         } as Auditoria;

    //         auditoriaApi.InsertAuditoria(requestAuditoria);

    //         if (response.data.length === 0) {
    //             swal({
    //                 title: 'Búsqueda sin resultados',
    //                 text: 'No se encontraron despachos que cumplan con los criterios de búsqueda.',
    //                 icon: 'info',
    //                 timer: 2000,
    //             });
    //         }


    //     } catch (error) {
    //         console.error("Error al obtener datos de la API:", error);
    //         NotificationService.genericAlertError("ERROR", "Error al obtener datos de la API.");
    //     } finally {
    //         setLoading(false);
    //     }
    // };

    const Search = async (data: any) => {
        setLoading(true); // Asegura que el loading comience
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand || 'Navegador desconocido';

        const { Estado, CuentaDeposito, codigo } = formularioDespachosJudiciales.getValues();

        let descripcionEstado = 'No especificado';
        if (Estado === 1) descripcionEstado = 'ACTIVO';
        else if (Estado === 0) descripcionEstado = 'INACTIVO';

        try {
            const response = await apiCourt.postAllCourtsByCityCode(data);
            console.log("Datos recibidos de la API:", response.data);
            const resultados = response.data as JuzgadoDto[];
            setInformacion(resultados);

            const coincidencia = resultados.find(
                (item) =>
                    item.idDepartamento === data.idDepartamento &&
                    item.idCiudad === data.idCiudad
            );

            if (coincidencia) {
                setDepartamentoNombre(coincidencia.departamento);
                setCiudadNombre(coincidencia.ciudad);
            } else {
                setDepartamentoNombre(null);
                setCiudadNombre(null);
            }

            const requestAuditoria: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Despachos Judiciales',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de despachos judiciales con los siguientes filtros:
                    - Departamento: ${coincidencia?.departamento || 'No especificado'}
                    - Ciudad: ${coincidencia?.ciudad || 'No especificado'}
                    - Código: ${codigo || 'No especificado'}
                    - Cuenta de depósito: ${CuentaDeposito || 'No especificado'}
                    - Estado: ${descripcionEstado}`,
            };

            await auditoriaApi.InsertAuditoria(requestAuditoria);

            if (resultados.length === 0) {
                swal({
                    title: 'Búsqueda sin resultados',
                    text: 'No se encontraron despachos que cumplan con los criterios de búsqueda.',
                    icon: 'info',
                    timer: 2000,
                });
            }

        } catch (error: any) {

            const descripcionError = `Error al consultar despachos judiciales:
                - Departamento: ${departamentoNombre || 'No especificado'}
                - Ciudad: ${ciudadNombre || 'No especificado'}
                - Código: ${codigo || 'No especificado'}
                - Cuenta de depósito: ${CuentaDeposito || 'No especificado'}
                - Estado: ${descripcionEstado}
                - Error: ${error?.message || 'Error desconocido'}
                - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Despachos Judiciales',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };

            await auditoriaApi.InsertAuditoria(requestAuditoriaError);

            NotificationService.genericAlertError("ERROR", "Error al obtener datos de la API.");
        } finally {
            setLoading(false);
        }
    };



    const handleOpenModalAdd = useCallback(() => {
        formulario.reset();
        formularioDespachosJudiciales.reset();
        obtenerDepartamentos();
        setOpenModalAdd(true);
        setCityValue(null);
        setDepartamentValue(null);
    }, [formulario, formularioDespachosJudiciales, obtenerDepartamentos]);

    const handleCloseModalAdd = async () => {
        try {
            formulario.reset();
            setCityValue(null);
            setDepartamentValue(null);
            setOpenModalAdd(false);
            console.log("Modal cerrado sin errores.");
        } catch (error) {
            console.error("Error en handleCloseModalAdd:", error);
        }
    };


    const handleAdd = async (data: JuzgadoDto) => {
        data.idJuzgado = GUID_DEFAULT;

        if (data.idDepartamento !== 0 && data.idCiudad !== 0) {
            try {
                const insertResponse = await startPostInsertCourt(data);

                if (insertResponse?.isSuccess) {
                    await handleCloseModalAdd();
                    handleSearch();
                    // NotificationService.genericAlert('Guardado', 'Se ha guardado correctamente la información'); 
                } else {
                    NotificationService.genericAlertError("ERROR", insertResponse?.returnMessage || "Error al guardar el juzgado.");
                }

            } catch (error) {
                console.error("Error al crear el juzgado:", error);
                NotificationService.genericAlertError("ERROR", "Error inesperado al intentar crear el juzgado.");
            }
        } else {
            NotificationService.genericAlertError("ERROR", "Ingrese el departamento o la ciudad");
        }
    };


    const handleEdit = async (data: JuzgadoDto) => {
        if (data.idDepartamento !== 0 && data.idCiudad !== 0) {
            try {
                const updateResponse = await startPostUpdateCourt(data);

                if (updateResponse?.isSuccess) {
                    await handleCloseModalAdd();
                    handleSearch();
                    NotificationService.genericAlert('Actualizado', 'Se ha actualizado correctamente la información');
                } else {
                    NotificationService.genericAlertError("ERROR", updateResponse.returnMessage || "Error al actualizar el juzgado.");
                }
            } catch (error) {
                console.error("Error al actualizar el juzgado:", error);
                NotificationService.genericAlertError("ERROR", "Error inesperado al intentar actualizar el juzgado.");
            }
        } else {
            NotificationService.genericAlertError("ERROR", "Ingrese el departamento o la ciudad");
        }
    };


    const onChangeDepartamento = (event: any, departament: DEPARTAMENTODTO | null) => {
        setDepartamentValue(departament);
        setCities([]);
        onChangeCitys(null, null)
        formularioDespachosJudiciales.setValue('idDepartamento', departament?.id ?? null);
        obtenerCiudades(departament?.id ?? null);
    };

    const onChangeCitys = (event: any, city: CITYDTO | null) => {
        setCityValue(city);
        formularioDespachosJudiciales.setValue('idCiudad', city?.id ?? null);
    };

    const handleClear = async () => {
        formularioDespachosJudiciales.reset(despachosJudicialesForm);
        onChangeDepartamento(null, null);
    };


    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Despachos Judiciales"
                subText=""
                title1="Gestión de"
                title2="Despachos Judiciales"
            />
            <CHPaper title="Información">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="idDepartamento"
                            control={formularioDespachosJudiciales.control}
                            rules={{ validate: () => formularioDespachosJudiciales.getValues("idDepartamento") !== 0 }}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={departamentValue}
                                    options={departaments}
                                    getOptionLabel={(option) => option.nombre}
                                    // onChange={onChangeDepartamento}
                                    onChange={(event, newValue) => {
                                        onChangeDepartamento(event, newValue);
                                        setDepartamentValue(newValue);
                                        formularioDespachosJudiciales.setValue('idDepartamento', newValue ? newValue.id : 0);
                                    }}



                                    renderInput={(params) => (
                                        <CHTextField
                                            label="Departamento *"
                                            {...params}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{
                                                ...params.inputProps,
                                                onInput: (e) => {
                                                    e.target.value = e.target.value.replace(/[^A-Za-zÁÉÍÓÚáéíóúÑñ\s]/g, '');
                                                },
                                                style: {
                                                    textTransform: "uppercase",
                                                },
                                            }}
                                        />
                                    )}
                                />
                            )}
                        />
                    </Grid>


                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="idCiudad"
                            control={formularioDespachosJudiciales.control}
                            rules={{ validate: () => formularioDespachosJudiciales.getValues("idCiudad") !== 0 }}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={cityValue}
                                    options={cities}
                                    getOptionLabel={(option) => option.nombre}
                                    // onChange={onChangeCitys}
                                    onChange={(event, newValue) => {
                                        onChangeCitys(event, newValue);
                                        setCityValue(newValue);
                                        formularioDespachosJudiciales.setValue('idCiudad', newValue ? newValue.id : 0);
                                    }}
                                    renderInput={(params) => (
                                        <CHTextField
                                            label="Ciudad *"
                                            {...params}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{
                                                ...params.inputProps,
                                                onInput: (e) => {
                                                    e.target.value = e.target.value.replace(/[^A-Za-zÁÉÍÓÚáéíóúÑñ\s]/g, '');
                                                },
                                                style: {
                                                    textTransform: "uppercase",
                                                },
                                            }}
                                        />
                                    )}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="codigo"
                            control={formularioDespachosJudiciales.control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Solo se permiten números.'
                                },
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (

                                <CHTextField
                                    label="Código"
                                    type="text"
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={(e) => {
                                        const newValue = e.target.value;
                                        if (newValue.length <= 12 && /^[0-9]*$/.test(newValue)) {
                                            onChange(newValue);
                                        }
                                    }}
                                    onKeyPress={(event) => {
                                        if (event.key === 'Enter') {
                                            handleSearch();
                                        }
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputProps={{
                                        inputMode: 'numeric',
                                        maxLength: 12,
                                        pattern: "^[0-9]*$",
                                        endAdornment: value ? (
                                            <InputAdornment position="end" style={{ paddingRight: '4px' }}>
                                                <IconButton
                                                    aria-label="clear selection"
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        formularioDespachosJudiciales.setValue('codigo', '');
                                                        onChange('');
                                                    }}
                                                    style={{ padding: '4px' }}
                                                >
                                                    <ClearIcon />
                                                </IconButton>
                                            </InputAdornment>
                                        ) : null,

                                    }}
                                />

                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="CuentaDeposito"
                            control={formularioDespachosJudiciales.control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Solo se permiten números.'
                                },
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    label="Cuenta de Depósito"
                                    type="text"
                                    InputLabelProps={{ shrink: true }}
                                    value={value}

                                    onChange={(e) => {
                                        const newValue = e.target.value;
                                        if (newValue.length <= 12 && /^[0-9]*$/.test(newValue)) {
                                            onChange(newValue);
                                        }
                                    }}
                                    onKeyPress={(event) => {
                                        if (event.key === 'Enter') {
                                            handleSearch();
                                        }
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputProps={{
                                        inputMode: 'numeric',
                                        maxLength: 12,
                                        pattern: "^[0-9]*$",
                                        endAdornment: value ? (
                                            <InputAdornment position="end" style={{ paddingRight: '4px' }}>
                                                <IconButton
                                                    aria-label="clear selection"
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        formularioDespachosJudiciales.setValue('CuentaDeposito', '');
                                                        onChange('');
                                                    }}
                                                    style={{ padding: '4px' }}
                                                >
                                                    <ClearIcon />
                                                </IconButton>
                                            </InputAdornment>
                                        ) : null,

                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="Estado"
                            control={formularioDespachosJudiciales.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    label="Estado"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {estados.map((option) => (
                                        <MenuItem key={option.idTipoCuenta} value={option.idTipoCuenta}>
                                            {option.descripcionTipoCuenta}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <div className="buttons">
                    <Button className={classes.button} variant="outlined" onClick={handleClear}>QUITAR FILTROS</Button>
                    <Button className={classes.button} variant="outlined" onClick={handleSearch}>
                        CONSULTAR
                    </Button>
                    <Button className={classes.button} variant="outlined" onClick={handleOpenModalAdd}>
                        AGREGAR DESPACHO JUDICIAL
                    </Button>
                </div>
                <br />
                <LoadingComponent key="loader" loading={loading} />
                {informacion.length > 0 && (
                    <Grid container>
                        <TableCourt
                            informacion={informacion}
                            obtenerCiudades={obtenerCiudades}
                            obtenerDespachosJudiciales={handleSearch}
                            formularioDespachosJudiciales={formularioDespachosJudiciales}
                            allTipoActor={allTipoActor}
                            estadoJuzgado={estadoJuzgado}
                        />
                    </Grid>
                )}
                <ModalCourtAdd
                    open={openModalAdd}
                    formulario={formulario}
                    formularioActor={formularioActor}
                    allDepartaments={departaments}
                    onClose={handleCloseModalAdd}
                    handleAdd={handleAdd}
                    handleEdit={handleEdit}
                    allTipoActor={allTipoActor}
                    estado={null}
                    informacion={null}

                />
            </CHPaper>
        </CHPrivateContainer>
    );
};

const mapStateToProps = (state: AppState) => ({ catalogState: state.catalog });

const mapDispatchToProps = {
    startPostInsertCourt,
    startPostUpdateCourt,
};

type props = LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Court);