import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Button,
    TablePagination,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { format } from 'date-fns';
import { AppState } from '../../store/rootReducer';
import history from '../../utils/history.router';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../components/ch-select/CHSelect';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { IDENTIFICATIONTYPEDTO, TIPOCUENTA, VALORCATALOGO } from '../../types/catalog.type';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import {
    BusquedaEmbargos,
    ConceptoEmbargosDto,
    EmbargoDto,
    EmbargoModelDto,
} from '../../types/embargo.model.type';
import UtilService from '../../utils/util.service';
import ApiEmbargos from '../../services/api-embargos.service';
import { busquedaEmbargosForm } from './constants/embargosform.model.type';
import Embargos from './Embargos';
import AuthService from '../../services/auth.service';
import { ClienteDto } from '../../types/client.model.type';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise'
import { Auditoria } from '../../types/Auditoria.model.type';
import NotificationService from '../../services/notification.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
    }),
);

function EmbargosTable({
    data,
    clientModel,
    onClickItem,
}: {
    data: EmbargoDto[] | null;
    clientModel?: ClienteDto;
    onClickItem: (data: EmbargoDto) => void;
}) {
    const classes = useStyles();
    const [nombreUsuario, setNombreUsuario] = useState('');
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);

    useEffect(() => {
        const user = AuthService.getUserInfo();
        if (user && user.USR_USERNAME) {
            setNombreUsuario(user.USR_NOMBRE);
        }
    }, []);

    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    if (data === null) {
        return <div>No se encontraron resultados</div>;
    }

    const paginatedData = data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

    return (
        <>
            <table className="credit-table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>Detalle</th>
                        <th style={{ textAlign: 'center' }}>Número Solicitud</th>
                        <th style={{ textAlign: 'center' }}>No. Radicado Asociado</th>
                        <th style={{ textAlign: 'center' }}>Tipo de Identificación</th>
                        <th style={{ textAlign: 'center' }}>No. Identificación</th>
                        <th style={{ textAlign: 'center' }}>Nombres y Apellidos</th>
                        <th style={{ textAlign: 'center' }}>Juzgado</th>
                        {/* <th style={{ textAlign: 'center' }}>Fecha Radicado Asociado</th> */}
                        <th style={{ textAlign: 'center' }}>Tipo Proceso</th>
                        <th style={{ textAlign: 'center' }}>Fecha Inicio</th>
                        <th style={{ textAlign: 'center' }}>Fecha Fin</th>
                        <th style={{ textAlign: 'center' }}>Usuario</th>
                        <th style={{ textAlign: 'center' }}>Estado</th>
                    </tr>
                </thead>
                <tbody>
                    {[...new Map(paginatedData.map(item => [item.numeroSolicitud, item])).values()].map((row, index) => (
                        <tr key={`table-${index}`}>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => onClickItem(row)}
                                    disabled={row.estadoEmbargo === 'EN TRÁMITE' && row.creadopornombre !== nombreUsuario}
                                >
                                    {row.estadoEmbargo === 'EN TRÁMITE' ? 'Procesar' : 'Detalle'}
                                </Button>
                            </td>
                            <td>{row.numeroSolicitud}</td>
                            <td>{row.radicadoWorkManager}</td>
                            <td>{row.tipoIdentificacion}</td>
                            <td>{row.identificacionCliente}</td>
                            <td>{row.nombreDemandado}</td>
                            <td>{row.juzgado}</td>
                            {/* <td>{UtilService.getDate(row.fechaRadicadoWorkManager)}</td> */}
                            <td>{row.tipoProceso}</td>
                            <td>
                                {format(new Date(row.fechaRegistro), 'dd MMM yyyy')} {format(new Date(row.fechaRegistro), 'hh:mm:ss a')}
                            </td>
                            <td>
                                {format(new Date(row.fechaActualizacion), 'dd MMM yyyy')} {format(new Date(row.fechaActualizacion), 'hh:mm:ss a')}
                            </td>
                            <td>{`${row.creadopornombre} ${row.creadoporapellido}`}</td>
                            <td>{row.estadoEmbargo}</td>
                        </tr>
                    ))}
                </tbody>

            </table>
            <TablePagination
                rowsPerPageOptions={[5, 10, 25]}
                component="div"
                count={data.length}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
            />
        </>
    );
}


const EmbargosInicio = (props: Props) => {
    const classes = useStyles();
    const api = ApiEmbargos();
    const catalogApi = CatalogApi();
    const { catalogState, loginState, startGetAllIdentificationTypes } = props;
    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [tipoRadicado, setTipoRadicado] = useState(0);
    const [embargos, setEmbargos] = useState<EmbargoDto[]>([]);
    const [embargo, setEmbargo] = useState<EmbargoDto | null>(null);
    const [embargoModel, setEmbargoModel] = useState<EmbargoModelDto | null>(
        null,
    );
    const [modoActualizar, setModoActualizar] = useState(false);
    const [estado, setEstado] = useState<any[]>();
    const [modalidad, setModalidad] = useState<any[]>([
        { idTipoCuenta: 0, descripcionTipoCuenta: "INACTIVO" },
        { idTipoCuenta: 1, descripcionTipoCuenta: "ACTIVO" },

    ]);

    const formularioBusquedaEmbargos = useForm({
        defaultValues: busquedaEmbargosForm,
    });

    useEffect(() => {
        getAllCatalogoPorId(72);
        startGetAllIdentificationTypes(loginState.claims);
    }, []);
    useEffect(() => {
        const url = window.location.href;
        const radicado = url.split('/')[4];
        formularioBusquedaEmbargos.setValue('radicadoWorkManager', radicado);
        handleSearch();
    }, []);


    const onItemIsSelected = (data: EmbargoDto) => {
        setEmbargo(data);
        setModoActualizar(true);
        consultarEmbargo(data.id || '');
    };
    const getAllCatalogoPorId = async (Id: number) => {
        await catalogApi
            .getAllCatalogoPorId(Id)
            .then(async (response: any) => {
                if (response && response.isSuccess) {
                    const filteredData = response.data.filter(
                        (item: any) => item.descripcion !== "RADICADA"
                    );
                    setEstado(filteredData);
                }
            })
            .catch(() => {
                toast('Error al obtener entidades bancarias');
            });
    }

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);

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


    const handleSearch = async () => {
        const clienteIdentificacion = formularioBusquedaEmbargos.watch('clienteIdentificacion') || '';
        const radicadoWorkManager = formularioBusquedaEmbargos.watch('radicadoWorkManager') || '';
        const fechaInicial = formularioBusquedaEmbargos.watch('fechaInicial');
        const fechaFinal = formularioBusquedaEmbargos.watch('fechaFinal');
        const clienteTipoIndentificacion = formularioBusquedaEmbargos.watch('clienteTipoIndentificacion');
        const Estado = 1;
        const Estados = formularioBusquedaEmbargos.watch('Estados');

        const ajustarHoraFinalDelDia = (fecha: Date): Date => {
            const fechaAjustada = new Date(fecha);
            fechaAjustada.setHours(23, 59, 59, 999);
            return fechaAjustada;
        };

        const fechaFinalAjustada = fechaFinal ? ajustarHoraFinalDelDia(new Date(fechaFinal)) : null;


        const Datas: BusquedaEmbargos = {
            clienteIdentificacion,
            clienteTipoIndentificacion,
            radicadoWorkManager,
            fechaInicial,
            fechaFinal: fechaFinalAjustada,
            Estado,
            Estados,
        }
        consultarEmbargosFiltros(Datas);

        const auditoriaApi = AuditoriaApi();

        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        let descripcionEstado = 'No especificado';
        let descripcionTipoIdentificacion = 'No especificado';

        if (Estados === 395) {
            descripcionEstado = 'EN TRÁMITE';
        } else if (Estados === 396) {
            descripcionEstado = 'PROCESADA';
        } else if (Estados === 397) {
            descripcionEstado = 'INCONSISTENTE';
        } else if (Estados === 553) {
            descripcionEstado = 'REMANENTE';
        }

        if (clienteTipoIndentificacion === 1) {
            descripcionTipoIdentificacion = 'CÉDULA DE CIUDADANÍA';
        } else if (clienteTipoIndentificacion === 3) {
            descripcionTipoIdentificacion = 'CÉDULA DE EXTRANJERÍA';
        } else if (clienteTipoIndentificacion === 8) {
            descripcionTipoIdentificacion = 'CÓDIGO JUZGADO';
        } else if (clienteTipoIndentificacion === 9) {
            descripcionTipoIdentificacion = 'CÓDIGO MILITAR';
        } else if (clienteTipoIndentificacion === 2) {
            descripcionTipoIdentificacion = 'NÚMERO DE IDENTIFICACIÓN TRIBUTARIA';
        } else if (clienteTipoIndentificacion === 5) {
            descripcionTipoIdentificacion = 'NÚMERO ÚNICO DE IDENTIFICACIÓN';
        } else if (clienteTipoIndentificacion === 6) {
            descripcionTipoIdentificacion = 'PASAPORTE';
        } else if (clienteTipoIndentificacion === 7) {
            descripcionTipoIdentificacion = 'REGISTRO CIVIL';
        } else if (clienteTipoIndentificacion === 4) {
            descripcionTipoIdentificacion = 'TARJETA DE IDENTIDAD';
        }


        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Gestión de embargos',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de Gestión de embargos con los siguientes filtros:
                - Fecha Inicial: ${Datas.fechaInicial || 'No especificada'}
                - Fecha Final: ${Datas.fechaFinal || 'No especificado'}
                - Tipo de identificación: ${descripcionTipoIdentificacion || 'No especificado'}
                - Número de identificación: ${Datas.clienteIdentificacion || 'No especificado'}
                - Número Radicado Asociado: ${Datas.radicadoWorkManager || 'No especificado'}
                - Estado: ${descripcionEstado || 'No especificado'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al consultar Embargos:
            - Fecha Inicial: ${Datas.fechaInicial || 'No especificada'}
            - Fecha Final: ${Datas.fechaFinal || 'No especificado'}
            - Tipo de identificación: ${descripcionTipoIdentificacion || 'No especificado'}
            - Número de identificación: ${Datas.clienteIdentificacion || 'No especificado'}
            - Número Radicado Asociado: ${Datas.radicadoWorkManager || 'No especificado'}
            - Estado: ${descripcionEstado || 'No especificado'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Gestión de Embargos',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };

            await auditoriaApi.InsertAuditoria(requestAuditoriaError);

            NotificationService.genericAlertError("ERROR", "Error al obtener datos de la API.");
        }


    };
    const consultarEmbargosFiltros = async (Datas: BusquedaEmbargos) => {

        await api
            .postObtenerRangoFiltros(Datas)
            .then(async response => {
                if (response && response.data !== null) {
                    const array = response.data;
                    if (response.isSuccess && array) {
                        setEmbargos(array);
                    }
                }
            })
            .catch(() => {
                toast('Por favor, llenar todos los campos obligatorios');
            });
    };

    const consultarEmbargo = async (idEmbargo: string) => {
        await api
            .getObtenerEmbargosById(idEmbargo)
            .then(async response => {
                if (response) {
                    const array = response.data;
                    if (response.isSuccess && array) {
                        setEmbargoModel(array);
                    }
                }
            })
            .catch(() => {
                toast('Por favor, llenar todos los campos obligatorios');
            });
    };

    const handleOnClickCancelarEditar = () => {
        setEmbargo(null);
        setEmbargoModel(null);
    };

    if (embargo != null && embargoModel !== null) {
        return (
            <Embargos
                embargoModel={embargoModel}
                modoActualizar={modoActualizar}
                setModoActualizar={setModoActualizar}
                handleOnClickCancelarEditar={handleOnClickCancelarEditar}
                handleSearch={handleSearch}
            />
        );
    }

    const validateDate = (data: Date | null) => {
        if (data === null) {
            return true;
        }

        const date = moment(data);
        let message = '';

        if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    };

    const validateEndDate = (data: Date | null) => {
        if (data === null) {
            return true;
        }
        const date = moment(data);
        let message = '';

        const fechaInicial = formularioBusquedaEmbargos.watch('fechaInicial')
        if (fechaInicial !== null) {

            if (data > new Date()) {
                message = "La fecha debe ser menor a la fecha actual"
            } else if (data < new Date(fechaInicial)) {
                message = "La fecha debe ser menor a la fecha inicial"
            }
        }
        return message !== '' ? message : true;
    };


    const today = new Date();

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Embargos"
                subText=""
                title1="Gestión de"
                title2="Embargos"
            />
            <CHPaper title="Información Solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaInicial"
                            rules={{
                                validate: validateDate
                            }}
                            control={formularioBusquedaEmbargos.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Inicial"
                                    onChange={onChange}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                    minDate={new Date("1-01-1950")}
                                    maxDate={today}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            rules={{
                                validate: validateEndDate
                            }}
                            name="fechaFinal"
                            control={formularioBusquedaEmbargos.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Final"
                                    onChange={onChange}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                    minDate={new Date("1-01-1950")}
                                    maxDate={today}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="clienteTipoIndentificacion"
                            control={formularioBusquedaEmbargos.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Tipo de Identificación"
                                    value={value}
                                    onChange={e => {
                                        setTipoDocumento(
                                            e.target.value as number,
                                        );
                                        onChange(e);
                                    }}
                                >
                                    <MenuItem key={0} value={0}>
                                        SELECCIONE...
                                    </MenuItem>
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
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="clienteIdentificacion"
                            control={formularioBusquedaEmbargos.control}
                            rules={{
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
                                min: {
                                    value: 1,
                                    message: 'Número de cédula inválido'
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
                                    required={false}
                                    numberMask
                                    onChange={e => {
                                        if (
                                            tipoDocumento === 1 ||
                                            tipoDocumento === 2 ||
                                            tipoDocumento === 5
                                        ) {
                                            if (e.target.value.length <= 12) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else if (
                                            tipoDocumento === 0 ||
                                            tipoDocumento === 3 ||
                                            tipoDocumento === 4 ||
                                            tipoDocumento === 6 ||
                                            tipoDocumento === 7 ||
                                            tipoDocumento === 8
                                        ) {
                                            if (e.target.value.length <= 20) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[ñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        }
                                    }}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        if (keyCode === 13) {
                                            handleSearch();
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="radicadoWorkManager"
                            rules={{
                                minLength: {
                                    value: 11,
                                    message:
                                        'La longitud mínima del campo es de 11 caracteres',
                                },
                                maxLength: {
                                    value: 23,
                                    message:
                                        'La longitud máxima del campo es de 23 caracteres',
                                },
                                min: {
                                    value: 1,
                                    message: 'Número de radicado inválido'
                                },
                                pattern: {
                                    value: /^[\d-\s]*$/g,
                                    message: 'Este campo debe contener solo números'
                                },

                            }}
                            control={formularioBusquedaEmbargos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número Radicado Asociado"
                                    value={value}
                                    required={false}

                                    onChange={e => {
                                        if (
                                            tipoRadicado === 1 ||
                                            tipoRadicado === 2 ||
                                            tipoRadicado === 5
                                        ) {
                                            if (e.target.value.length <= 23) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[^-\d\s]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else if (
                                            tipoRadicado === 0 ||
                                            tipoRadicado === 3 ||
                                            tipoRadicado === 4 ||
                                            tipoRadicado === 6 ||
                                            tipoRadicado === 7 ||
                                            tipoRadicado === 8
                                        ) {
                                            if (e.target.value.length <= 23) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[^-\d\s]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        }
                                    }}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /[^-\d\s]/g;
                                        if (keyCode === 13) {
                                            handleSearch();
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="Estados"
                            control={formularioBusquedaEmbargos.control}
                            rules={{
                                // required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Estados"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >

                                    {estado?.map(
                                        (option: VALORCATALOGO) => (
                                            <MenuItem
                                                key={option.idValor}
                                                value={option.idValor}
                                            >
                                                {option.descripcion.toUpperCase()}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={handleSearch}
                >
                    CONSULTAR
                </Button>
            </div>
            <EmbargosTable data={embargos} onClickItem={onItemIsSelected} />
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(EmbargosInicio);
