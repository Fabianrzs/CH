import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { sortBy } from 'lodash';
import { toast } from 'react-toastify';
import moment from 'moment';
import { useHistory } from 'react-router-dom';
import { AppState } from '../../store/rootReducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import {
    busquedaDesembargoForm,
    conceptoEmbargoForm,
} from '../embargos/constants/embargosform.model.type';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../components/ch-select/CHSelect';
import {
    CITYDTO,
    DEPARTAMENTODTO,
    IDENTIFICATIONTYPEDTO,
    VALORCATALOGO,
} from '../../types/catalog.type';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../reducers/catalog.reducer';
import UtilService from '../../utils/util.service';
import { ObtenerDesembargoDto, DesembargoDto, ConceptoDesembargosDto, CuentaModelDto } from '../../types/desembargo.model.type';
import { startGetAllTypeDocuments } from '../../actions/catalog.action';
import ApiCatalog from '../../services/api-catalog.service';
import ApiCourt from '../../services/api-courts.service';
import { JuzgadoDto } from '../../types/court.model.type';
import { desembargoForm } from '../desembargosConceptos/constants/desembargosform.model.type';
import { DesembargosState } from '../../reducers/desembargos.reducer';
import { BusquedaEmbargos, ConceptoEmbargosDto, EmbargoDto } from '../../types/embargo.model.type';
import AuthService from '../../services/auth.service';
import { ClienteDto } from '../../types/client.model.type';
import ApiEmbargos from '../../services/api-embargos.service';
import ApiClients from '../../services/api-clients.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
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

function DesembargoTable({
    data,
    clientModel,
    onClickItem,
    datoEstado
}: {
    data: EmbargoDto[] | null;
    clientModel: ClienteDto;
    onClickItem: (data: EmbargoDto) => void;
    datoEstado: any;
}) {
    const classes = useStyles();
    const history = useHistory();
    const apiClientes = ApiClients();


    const [conceptos, setConceptos] = useState<ConceptoEmbargosDto[]>([]);

    const obtenerConceptos = async (tipoDocumentoId: any, numeroDocumento: any) => {
        try {
            const result = await apiClientes.getCuentasConceptosPorTipoNumeroIdentificacion(
                tipoDocumentoId,
                numeroDocumento
            );


            setConceptos(result.data);
        } catch (error) {
            console.error('Error al obtener los conceptos', error);
        }
    };

    const handleOnClickDetalle = (row: EmbargoDto) => {
        onClickItem(row);
        obtenerConceptos(row.tipoIdentificacionId, row.identificacionCliente);

        // history.push(`/InformacionDesembargo?id=${row.id}`);
        history.push({
            pathname: "/InformacionDesembargo",
            search: `?id=${row.id}`,
            state: { datoEstado, row }
          });

    }

    if (data === null || !Array.isArray(data)) {
        return <div>No se encontraron resultados</div>;
    }


    const [nombreUsuario, setNombreUsuario] = useState('');

    useEffect(() => {
        const User = AuthService.getUserInfo();
        if (User && User.USR_USERNAME) {
            setNombreUsuario(User.USR_NOMBRE);

        }
    }, []);

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
                        <th style={{ textAlign: 'center' }}>Juzgado</th>
                        <th style={{ textAlign: 'center' }}>Tipo Proceso</th>
                        <th style={{ textAlign: 'center' }}>Fecha Inicio</th>
                        <th style={{ textAlign: 'center' }}>Fecha Fin</th>
                        <th style={{ textAlign: 'center' }}>Usuario</th>
                        <th style={{ textAlign: 'center' }}>Estado</th>

                    </tr>
                </thead>
                <tbody>
                    {Array.from(new Map(data?.map(item => [item.numeroSolicitud, item])).values()).map((row, index) => {
                        const solicitud = clientModel?.solicitudes[index];
                        return (
                            <tr key={`table-${index}`}>
                                <td>
                                    <Button
                                        className={classes.processButton}
                                        onClick={() => handleOnClickDetalle(row)}
                                        disabled={row.estadoEmbargo === "PROCESADA" && row.creadopornombre !== nombreUsuario}
                                    >
                                       {row.estadoEmbargo === 'EN TRÁMITE' || row.estadoEmbargo === 'ANULADA' ? 'Detalle' : 'Procesar'}
                                    </Button>
                                </td>
                                <td>{row.numeroSolicitud}</td>
                                <td>{row.radicadoWorkManager}</td>
                                <td>{row.tipoIdentificacion}</td>
                                <td>{row.identificacionCliente}</td>
                                <td>{row.nombreDemandado}</td>
                                <td>{row.juzgado}</td>
                                <td>{row.tipoProceso}</td>
                                <td>{UtilService.getFullDate(row.fechaRegistro)}</td>
                                <td>{UtilService.getDate(row.fechaActualizacion)}</td>
                                <td>{`${row.creadopornombre} ${row.creadoporapellido}`}</td>
                                <td>{row.estadoEmbargo}</td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </>
    );
}

const DesembargoInicio = (props: props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiCourt = ApiCourt();
    const apiClient = ApiClients();
    const api = ApiEmbargos();

    const { catalogState, startGetAllTypeDocuments } = props;
    const [documento, setDocumento] = useState('');
    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [desembargos, setDesembargos] = useState<ObtenerDesembargoDto[]>([]);
    const datoEstado = desembargos[0]?.estadoEmbargo;
    
    const [estad, setEstad] = useState<any[]>();
    const formularioBusquedaDesembargos = useForm({
        defaultValues: busquedaDesembargoForm,
    });

    useEffect(() => {
        startGetAllTypeDocuments();
        getAllCatalogoPorId(72);

    }, []);

    const onItemIsSelected = (data: any) => {
        setDesembargos(data);

    };

    const getAllCatalogoPorId = async (Id: number) => {
        await apiCatalog
            .getAllCatalogoPorId(Id)
            .then(async (response: any) => {
                if (response && response.isSuccess) {
                    const filteredData = response.data.filter(
                        (item: any) => item.descripcion !== "RADICADA"
                    );
                    setEstad(filteredData);
                }
            })
            .catch(() => {
                toast('Error al obtener entidades bancarias');
            });
    }

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
        const clienteIdentificacion = formularioBusquedaDesembargos.watch('clienteIdentificacion') || '';
        const radicadoWorkManager = formularioBusquedaDesembargos.watch('radicadoWorkManager') || '';
        const fechaInicial = formularioBusquedaDesembargos.watch('fechaInicial');
        const fechaFinal = formularioBusquedaDesembargos.watch('fechaFinal');
        const clienteTipoIndentificacion = formularioBusquedaDesembargos.watch('clienteTipoIndentificacion');
        const Estado = 1;
        const Estados = formularioBusquedaDesembargos.watch('Estados');

        const Datas: BusquedaEmbargos = {
            clienteIdentificacion,
            clienteTipoIndentificacion,
            radicadoWorkManager,
            fechaInicial,
            fechaFinal,
            Estado,
            Estados,
        }
        consultarDesembargosFiltros(Datas);

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
                modulo: 'Gestión de Desembargos',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de Gestión de desembargos con los siguientes filtros:
                - Fecha Inicial: ${Datas.fechaInicial || 'No especificada'}
                - Fecha Final: ${Datas.fechaFinal || 'No especificado'}
                - Tipo de identificación: ${descripcionTipoIdentificacion || 'No especificado'}
                - Número de identificación: ${Datas.clienteIdentificacion || 'No especificado'}
                - Número Radicado Asociado: ${Datas.radicadoWorkManager || 'No especificado'}
                - Estado: ${descripcionEstado || 'No especificado'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al consultar Desembargos:
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
                modulo: 'Gestión de Desembargos',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };

            await auditoriaApi.InsertAuditoria(requestAuditoriaError);

            NotificationService.genericAlertError("ERROR", "Error al obtener datos de la API.");
        }

    };

    const consultarDesembargosFiltros = async (busqueda: BusquedaEmbargos) => {
        await api
            .postObtenerRangoFiltros(busqueda)
            .then(async response => {
                if (response && response.data !== null) {
                    const array = response.data;
                    if (response.isSuccess && array) {
                        setDesembargos(array);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const validateDate = (data: Date | null) => {
        if (data === null) {
            return true;
        }

        let message = '';

        if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    };

    const today = new Date();

    useEffect(() => {
        const url = window.location.href;
        const radicado = url.split('/')[4];
        formularioBusquedaDesembargos.setValue('radicadoWorkManager', radicado);
        handleSearch();
    }, []);

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Desembargos"
                subText=""
                title1="Gestión de"
                title2="Desembargos"
            />
            <CHPaper title="Información Solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaInicial"
                            rules={{
                                validate: validateDate
                            }}
                            control={formularioBusquedaDesembargos.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Inicial"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
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

                            name="fechaFinal"
                            control={formularioBusquedaDesembargos.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Final"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="clienteTipoIndentificacion"
                            control={formularioBusquedaDesembargos.control}
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

                                    {catalogState.IdentificationType?.map(
                                        (option: IDENTIFICATIONTYPEDTO) => (
                                            <MenuItem key={option.id} value={option.id}>
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
                            control={formularioBusquedaDesembargos.control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Este campo es requerido y debe contener solo números'
                                },
                                required: {
                                    value: false,
                                    message: 'Este campo es requerido y debe contener solo números',
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
                                                        /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else {
                                            onChange(e);
                                        }
                                        setDocumento(e.target.value);
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
                            control={formularioBusquedaDesembargos.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número Radicado Asociado"
                                    value={value}
                                    onChange={(e) => {
                                        const newValue = e.target.value.replace(/[^0-9-]/g, '');
                                        onChange(newValue);
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
                            name="Estados"
                            control={formularioBusquedaDesembargos.control}
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

                                    {estad?.map(
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
                    onClick={formularioBusquedaDesembargos.handleSubmit(
                        handleSearch,
                    )}
                >
                    CONSULTAR
                </Button>
            </div>
            <DesembargoTable data={desembargos} onClickItem={onItemIsSelected} datoEstado={datoEstado}/>
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});
interface LinkMapDispatchToProps {
    startGetAllTypeDocuments: () => void;
}

const mapDispatchToProps = {
    startGetAllTypeDocuments,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(DesembargoInicio);
