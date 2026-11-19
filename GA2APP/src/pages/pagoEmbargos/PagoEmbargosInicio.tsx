import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Button,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AppState } from '../../store/rootReducer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { IDENTIFICATIONTYPEDTO, TIPOCUENTA } from '../../types/catalog.type';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import {
    BusquedaEmbargos,
    EmbargoDto,
    EmbargoModelDto,
} from '../../types/embargo.model.type';
import UtilService from '../../utils/util.service';
import ApiEmbargos from '../../services/api-embargos.service';
import AuthService from '../../services/auth.service';
import { ClienteDto } from '../../types/client.model.type';
import { busquedaEmbargosForm } from '../embargos/constants/embargosform.model.type';
import PagoEmbargos from './PagoEmbargos';
import GenerarArchivo from './components/GenerarArchivo';
import AuditoriaApi from '../../services/api-auditorias.servise';
import ApiCatalog from '../../services/api-catalog.service';
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

function PagoEmbargosTable({
    data,
    onClickItem,
    isPagoEmbargosModule
}: {
    data: EmbargoDto[] | null;
    onClickItem: (data: EmbargoDto) => void;
    isPagoEmbargosModule?: boolean;
}) {
    const classes = useStyles();


    if (data === null) {
        return <div>No se encontraron resultados</div>;
    }

    const user = AuthService.getUserInfo();

    function procesarEstado(estados: any) {
        if (estados === "Procesada") {
            return "En curso";
        }
        return estados;
    }
    let estadoInicial = "En curso";

    estadoInicial = procesarEstado(estadoInicial);
    const [nombreUsuario, setNombreUsuario] = useState('');

    useEffect(() => {
        if (user && user.USR_USERNAME) {
            setNombreUsuario(user.USR_NOMBRE);
        }
    }, []);

    function mapEstado(estado: string): string {
        if (isPagoEmbargosModule && estado === "PROCESADA") {
            return "EN TRÁMITE";
        }
        return estado;
    }    


    return (
        <>
            <table className="credit-table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>Detalle</th>
                        <th style={{ textAlign: 'center' }}>
                            Número Solicitud
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Radicado Asociado
                        </th>
                        <th style={{ textAlign: 'center' }}>Tipo de Identificación</th>
                        <th style={{ textAlign: 'center' }}>
                            No. Identificación
                        </th>
                        <th style={{ textAlign: 'center' }}>Nombres y Apellidos</th>
                        <th style={{ textAlign: 'center' }}>Tipo Proceso</th>
                        <th style={{ textAlign: 'center' }}>Valor / Porcentaje</th>
                        <th style={{ textAlign: 'center' }}>Fecha Inicio</th>
                        <th style={{ textAlign: 'center' }}>Fecha Fin</th>
                        <th style={{ textAlign: 'center' }}>Usuario</th>
                        <th style={{ textAlign: 'center' }}>Estado</th>
                    </tr>
                </thead>
                <tbody>
                    {data?.map((row, index) => {
                        const valorFormateado = (() => {
                            if (row.valor >= 1 && row.valor <= 100) {
                                return `${row.valor}%`;
                            }

                            if (row.valor >= 1000) {
                                return UtilService.formatCurrency(row.valor);
                            }

                            return row.valor ?? 'Sin valor';
                        })();

                        return (
                            <tr key={`table-${index}`}>
                                <td>
                                    <Button
                                        className={classes.processButton}
                                        onClick={() => onClickItem(row)}
                                        disabled={
                                            row.estadoEmbargo !== "PROCESADA" ||
                                            row.creadopornombre !== nombreUsuario
                                        }
                                    >
                                        PROCESAR
                                    </Button>
                                </td>
                                <td>{row.numeroSolicitud}</td>
                                <td>{row.radicadoWorkManager}</td>
                                <td>{row.tipoIdentificacion}</td>
                                <td>{row.identificacionCliente}</td>
                                <td>{row.nombreDemandado}</td>
                                <td>{row.tipoProceso}</td>
                                <td>{valorFormateado}</td>
                                <td>{UtilService.getDate(row.fechaRegistro)}</td>
                                <td>{UtilService.getDate(row.fechaActualizacion)}</td>
                                <td>{`${row.creadopornombre} ${row.creadoporapellido}`}</td>
                                {/* <td>{row.estadoEmbargo}</td> */}
                                <td>{mapEstado(row.estadoEmbargo)}</td>
                            </tr>
                        );
                    })}
                </tbody>

            </table>
        </>
    );
}

const PagoEmbargosInicio = (props: Props) => {
    const classes = useStyles();
    const api = ApiEmbargos();
    const { catalogState } = props;

    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [tipoRadicado, setTipoRadicado] = useState(0);
    const [embargos, setEmbargos] = useState<EmbargoDto[]>([]);
    const [embargo, setEmbargo] = useState<EmbargoDto | null>(null);
    const [embargoModel, setEmbargoModel] = useState<EmbargoModelDto | null>(null);
    const [modoActualizar, setModoActualizar] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [estado, setEstado] = useState<any[]>([
        { idTipoCuenta: 395, descripcionTipoCuenta: "EN TRÁMITE" },
        { idTipoCuenta: 532, descripcionTipoCuenta: "RADICADA" },
        { idTipoCuenta: 479, descripcionTipoCuenta: "INCONSISTENTE" },
        { idTipoCuenta: 396, descripcionTipoCuenta: "PROCESADA" },
    ]);
    const [vista, setVista] = useState<'INICIO' | 'RESULTADO'>('INICIO');

    const formularioBusquedaEmbargos = useForm({
        defaultValues: busquedaEmbargosForm,
    });

    const openModalGenerarArchivo = () => setIsModalOpen(true);
    const handleClose = () => setIsModalOpen(false);

    const handleOnClickCancelarEditar = () => {
        setEmbargo(null);
        setEmbargoModel(null);
        setModoActualizar(false);
    };

    const onItemIsSelected = (data: EmbargoDto) => {
        setEmbargo(data);
        setModoActualizar(true);
        consultarEmbargo(data.id || '');
    };

    const consultarEmbargo = async (idEmbargo: string) => {
        try {
            const response = await api.getObtenerEmbargosById(idEmbargo);
            if (response && response.isSuccess) {
                setEmbargoModel(response.data);
            }
        } catch {
            toast('Error al consultar un embargo');
        }
    };

    const consultarEmbargosFiltros = async (busqueda: BusquedaEmbargos) => {
        try {
            const response = await api.postObtenerRangoFiltros(busqueda);
            if (response && response.isSuccess) {
                setEmbargos(response.data);
            }
        } catch {
            toast('Error al obtener los embargos');
        }
    };

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
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

    const handleSearch = async () => {
        const busqueda: BusquedaEmbargos = {
            clienteIdentificacion: formularioBusquedaEmbargos.watch('clienteIdentificacion') || '',
            clienteTipoIndentificacion: formularioBusquedaEmbargos.watch('clienteTipoIndentificacion'),
            radicadoWorkManager: formularioBusquedaEmbargos.watch('radicadoWorkManager') || '',
            fechaInicial: formularioBusquedaEmbargos.watch('fechaInicial'),
            fechaFinal: formularioBusquedaEmbargos.watch('fechaFinal'),
            Estado: 1,
            Estados: formularioBusquedaEmbargos.watch('Estados'),
        };
        consultarEmbargosFiltros(busqueda);

        const auditoriaApi = AuditoriaApi();
        const Estados = formularioBusquedaEmbargos.watch('Estados');
        const clienteTipoIndentificacion = formularioBusquedaEmbargos.watch('clienteTipoIndentificacion');

        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        let descripcionEstado = 'No especificado';
        let descripcionTipoIdentificacion = 'No especificado';

        if (Estados === 395) {
            descripcionEstado = 'EN TRÁMITE';
        } else if (Estados === 532) {
            descripcionEstado = 'RADICADA';
        } else if (Estados === 479) {
            descripcionEstado = 'INCONSISTENTE';
        } else if (Estados === 396) {
            descripcionEstado = 'PROCESADA';
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
                modulo: 'Pago de Embargos',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de pago de embargos con los siguientes filtros:
                - Tipo de identificación: ${descripcionTipoIdentificacion || 'No especificado'}
                - Número de identificación: ${busqueda.clienteIdentificacion || 'No especificado'}
                - Número Radicado Asociado: ${busqueda.radicadoWorkManager || 'No especificado'}
                - Estado: ${descripcionEstado || 'No especificado'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error: any) {
            const descripcionError = `Error al consultar pago de embargos:
                - Tipo de identificación: ${descripcionTipoIdentificacion || 'No especificado'}
                - Número de identificación: ${busqueda.clienteIdentificacion || 'No especificado'}
                - Número Radicado Asociado: ${busqueda.radicadoWorkManager || 'No especificado'}
                - Estado: ${descripcionEstado || 'No especificado'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Pago de Embargos',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };

            await auditoriaApi.InsertAuditoria(requestAuditoriaError);

            NotificationService.genericAlertError("ERROR", "Error al obtener datos de la API.");
        }

    };

    useEffect(() => {
        const url = window.location.href;
        const radicado = url.split('/')[4];
        formularioBusquedaEmbargos.setValue('radicadoWorkManager', radicado);
        handleSearch();
    }, []);

    return (
        <CHPrivateContainer>
            {vista === 'INICIO' && embargo != null && embargoModel !== null ? (
                <PagoEmbargos
                    embargoModel={embargoModel}
                    modoActualizar={modoActualizar}
                    setModoActualizar={setModoActualizar}
                    handleOnClickCancelarEditar={handleOnClickCancelarEditar}
                    setVista={setVista}
                />
            ) : (
                <>
                    <CHPageTitle
                        topText="Pago de Embargos"
                        subText=""
                        title1="Pago de"
                        title2="Embargos"
                    />
                    <CHPaper title="Información Solicitud">
                        <Grid container>
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
                                                const regex = /^[\d-\s]*$/;
                                                if (!regex.test(keyValue)) {
                                                    event.preventDefault();
                                                }
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
                                                (
                                                    option: TIPOCUENTA,
                                                ) => (
                                                    <MenuItem
                                                        key={
                                                            option.idTipoCuenta
                                                        }
                                                        value={
                                                            option.idTipoCuenta

                                                        }
                                                    >
                                                        {
                                                            option.descripcionTipoCuenta
                                                        }
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
                        <Button
                            className={classes.button}
                            variant="outlined"
                            onClick={openModalGenerarArchivo}
                        >
                            GENERAR ARCHIVO
                        </Button>
                    </div>
                    <PagoEmbargosTable data={embargos} onClickItem={onItemIsSelected} isPagoEmbargosModule/>

                    {isModalOpen && (
                        <GenerarArchivo
                            open={isModalOpen} handleClose={handleClose} embargo={embargo}
                        />
                    )}
                </>
            )}
            {isModalOpen && (
                <GenerarArchivo
                    open={isModalOpen}
                    handleClose={handleClose}
                    embargo={embargo}
                />
            )}
        </CHPrivateContainer>
    );
}


interface LinkMapStateToProps {
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});



const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps;

export default connect(mapStateToProps, mapDispatchToProps)(PagoEmbargosInicio);