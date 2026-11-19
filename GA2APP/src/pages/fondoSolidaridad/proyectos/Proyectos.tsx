import { createStyles, makeStyles, Grid, MenuItem, Button } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { AppState } from '../../../store/rootReducer';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { startGetAllDepartments } from '../../../actions/catalog.action';
import { CITYDTO, DEPARTAMENTODTO } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';
import AdministracionProyectos from './AministracionProyectos';
import ConstructoraProyecto from './constructoras/ConstructoraProyecto';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { LoadingComponent } from '../../../components/loading/loading.component';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';

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

function ProyectosTable({
    proyectos,
    loading,
    onClickItem,
}: {
    proyectos: any[];
    loading: boolean;
    onClickItem: (idProyecto: string) => void;
}) {
    const classes = useStyles();

    return (
        <>
            <LoadingComponent loading={loading} />
            <table className="credit-table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>
                            Procesar
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Proyecto
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Dirección
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Departamento
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Municipio
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Teléfono
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Vigencia
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Cupos
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Estado
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Fecha de Creación
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {proyectos.map((row, index) => (
                        <tr key={index}>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => onClickItem(row.codigo)}
                                >
                                    {row?.estado === 'EN CREACIÓN' ? 'PROCESAR' : 'DETALLES'}
                                </Button>
                            </td>
                            <td>{row?.nombreProyecto}</td>
                            <td>{row?.direccion}</td>
                            <td>{row?.deparmento}</td>
                            <td>{row?.ciudad}</td>
                            <td>{row?.telefono}</td>
                            <td>{moment(row?.fechaVigencia).format('YYYY-MM-DD')}</td>
                            <td>{row?.cupos}</td>
                            <td>{row?.estado}</td>
                            <td>{UtilService.getFullDate(new Date(row?.fechaCreacion))}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </>
    )
}

const Proyectos = (props: Props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const { loginState, catalogState, startGetAllDepartments } = props;
    const [estadosProyecto, setEstadosProyecto] = useState<any[]>([]);
    const [proyectos, setProyectos] = useState<any[]>([]);
    const [ciudades, setCiudades] = useState<CITYDTO[]>([]);
    const [selectedData, setSelectedData] = useState<any>(null);
    const [openProyecto, setOpenProyecto] = useState(false);
    const [openConstructoras, setOpenConstructoras] = useState(false);
    const [loading, setLoading] = useState(false);

    const formularioBusquedaProyectos = useForm({
        defaultValues: {
            proyecto: 'SELECCIONE',
            departamento: 0,
            ciudad: 0,
            estado: 'SELECCIONE',
        },
    });

    useEffect(() => {
        const busqueda = {
            proyectoId: '',
            idDepartamento: '',
            idCiudad: '',
            idEstado: ''
        }
        consultarProyectos(busqueda);
    }, [proyectos.length > 0])

    useEffect(() => {
        startGetAllDepartments(loginState.claims);
        consultarEstadoProyecto();
    }, [])

    const consultarEstadoProyecto = async () => {
        const ESTADO_PROYECTO = 97;
        await apiCatalog.getObtenerValoresCatalogosPorIdCatalogo(ESTADO_PROYECTO)
            .then(response => {
                if (response.data) {
                    setEstadosProyecto(response.data);
                }
            }).catch(() => {
                toast(`Error al consultar estados de proyectos`, { type: toast.TYPE.ERROR });
            })
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

    // const consultarProyectosFiltros = async (data: any) => {
    //     setLoading(true);
    //     const busqueda = {
    //         proyectoId: data.proyecto === 'SELECCIONE' ? '' : data.proyecto,
    //         idDepartamento: data.departamento === 0 ? '' : data.departamento,
    //         idCiudad: data.ciudad === 0 ? '' : data.ciudad,
    //         idEstado: data.estado === 'SELECCIONE' ? '' : data.estado
    //     }

    //     await apiFondoSolidaridad.getConsultarProyectos(busqueda)
    //         .then(response => {
    //             if (response.data) {
    //                 setProyectos(response.data);
    //                 setLoading(false);

    //                 const auditoriaApi = AuditoriaApi();
    //                 const user = AuthService.getUserInfo();
    //                 const nav = window.navigator as any;
    //                 const usrAg = nav.userAgentData.brands.find(
    //                     (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    //                 )?.brand;

    //                 const proyectoNombre = proyectos?.find(
    //                     p => p.codigo === busqueda.proyectoId
    //                 )?.nombreProyecto || 'No especificado';

    //                 const departamentoNombre = catalogState?.departments.find(
    //                     d => d.id === busqueda.idDepartamento
    //                   )?.nombre || 'No especificado';

    //                   const ciudadNombre = ciudades?.find(
    //                     c => c.id === busqueda.idCiudad
    //                   )?.nombre || 'No especificada';

    //                   const estadoNombre = estadosProyecto?.find(
    //                     e => e.idValor === busqueda.idEstado
    //                   )?.descripcion || 'No especificado';

    //                 const requestAuditoria = {
    //                     navegador: usrAg,
    //                     sistemaOperativo: nav.userAgentData.platform,
    //                     nombreUsuario: user.USR_USERNAME,
    //                     usurioId: user.USR_ID,
    //                     ip: publicIpAddress,
    //                     modulo: 'Fondo Solidaridad / Proyectos',
    //                     hostname: document.location.host,
    //                     tipoAccion: 'CONSULTAS',
    //                     idTipoAccion: 3,
    //                     descripcion: `Consulta información proyectos con los siguientes filtros:
    //               - Proyecto: ${proyectoNombre || 'No especificada'}
    //               - Departamento: ${departamentoNombre || 'No especificado'}
    //               - Municipio: ${ciudadNombre || 'No especificada'}
    //               - Estado: ${estadoNombre || 'No especificado'}`,
    //                 } as Auditoria;

    //                 auditoriaApi.InsertAuditoria(requestAuditoria);
    //             }
    //             else {
    //                 setLoading(false);
    //                 toast("No se encontraron resultados");
    //             }
    //         }).catch(() => {
    //             setLoading(false);
    //             toast(`Error al consultar proyectos`, { type: toast.TYPE.ERROR });
    //         })
    // }

    const consultarProyectosFiltros = async (data: any) => {
        setLoading(true);
        const busqueda = {
            proyectoId: data.proyecto === 'SELECCIONE' ? '' : data.proyecto,
            idDepartamento: data.departamento === 0 ? '' : data.departamento,
            idCiudad: data.ciudad === 0 ? '' : data.ciudad,
            idEstado: data.estado === 'SELECCIONE' ? '' : data.estado
        };

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const response = await apiFondoSolidaridad.getConsultarProyectos(busqueda);

            if (response?.data) {
                setProyectos(response.data);
                toast('Consulta exitosa');
            } else {
                toast("No se encontraron resultados");
            }

          
            const proyectoNombre = proyectos?.find(
                p => p.codigo === busqueda.proyectoId
            )?.nombreProyecto || 'No especificado';

            const departamentoNombre = catalogState?.departments.find(
                d => d.id === busqueda.idDepartamento
            )?.nombre || 'No especificado';

            const ciudadNombre = ciudades?.find(
                c => c.id === busqueda.idCiudad
            )?.nombre || 'No especificada';

            const estadoNombre = estadosProyecto?.find(
                e => e.idValor === busqueda.idEstado
            )?.descripcion || 'No especificado';

            const requestAuditoria: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Fondo Solidaridad / Proyectos',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de proyectos con los siguientes filtros:
                - Proyecto: ${proyectoNombre}
                - Departamento: ${departamentoNombre}
                - Municipio: ${ciudadNombre}
                - Estado: ${estadoNombre}`
            };

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {

            const descripcionError = `Error al consultar proyectos.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Fondo Solidaridad / Proyectos',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar proyectos', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }
    };


    const consultarProyectos = async (busqueda: any) => {
        setLoading(true);
        await apiFondoSolidaridad.getConsultarProyectos(busqueda)
            .then(response => {
                if (response.data) {
                    setProyectos(response.data);
                    setLoading(false);
                }
                else {
                    setLoading(false);
                    toast("No se encontraron resultados");
                }
            }).catch(() => {
                setLoading(false);
                toast(`Error al consultar proyectos`, { type: toast.TYPE.ERROR });
            })
    }

    const onItemIsSelected = async (idProyecto: string) => {
        await apiFondoSolidaridad.getConsultarProyectoPorId(idProyecto)
            .then(response => {
                if (response.data) {
                    setSelectedData(response.data);
                }
            }).catch(() => {
                toast(`Error al consultar proyectos`, { type: toast.TYPE.ERROR });
            })
    }

    const obtenerCiudades = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudades(datos);
        }
    };

    const handleOpenInformacionProyecto = () => { setOpenProyecto(true) };

    const handleOpenConstructoras = () => { setOpenConstructoras(true) };

    if (openProyecto || selectedData) {
        return (
            <AdministracionProyectos
                informacionProyecto={selectedData}
                onItemIsSelected={onItemIsSelected}
            />
        )
    }

    if (openConstructoras) {
        return (
            <ConstructoraProyecto
                setOpenConstructoras={setOpenConstructoras}
            />
        )
    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Gestión de Proyectos"
                subText=""
                title1="Administración de"
                title2="Proyectos de Vivienda"
            />
            <CHPaper title="Información Proyectos">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="proyecto"
                            control={formularioBusquedaProyectos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Proyecto"
                                >
                                    <MenuItem key='SELECCIONE' value='SELECCIONE' >
                                        SELECCIONE...
                                    </MenuItem>
                                    {proyectos.map(
                                        (option: any) => (
                                            <MenuItem
                                                key={option.codigo}
                                                value={option.codigo}
                                            >
                                                {option.nombreProyecto}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento"
                            control={formularioBusquedaProyectos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={e => {
                                        obtenerCiudades(Number(e.target.value));
                                        onChange(e);
                                    }}
                                    label="Departamento"
                                >
                                    <MenuItem key={0} value={0} >
                                        SELECCIONE...
                                    </MenuItem>
                                    {catalogState?.departments.map(
                                        (option: DEPARTAMENTODTO) => {
                                            if (option.id !== 0) {
                                                return (
                                                    <MenuItem key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.nombre}
                                                    </MenuItem>
                                                )
                                            }
                                            return null;
                                        }
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad"
                            control={formularioBusquedaProyectos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Municipio"
                                >
                                    <MenuItem key={0} value={0} >
                                        SELECCIONE...
                                    </MenuItem>
                                    {ciudades?.map(
                                        (option: CITYDTO) => {
                                            if (option.id !== 0) {
                                                return (
                                                    <MenuItem key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.nombre}
                                                    </MenuItem>
                                                )
                                            }
                                            return null;
                                        }
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="estado"
                            control={formularioBusquedaProyectos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Estado"
                                >
                                    <MenuItem key='SELECCIONE' value='SELECCIONE'>
                                        SELECCIONE...
                                    </MenuItem>
                                    {estadosProyecto?.map(
                                        (option: any) => {
                                            if (option.id !== 0) {
                                                return (
                                                    <MenuItem key={option.idValor}
                                                        value={option.idValor}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                )
                                            }
                                            return null;
                                        }
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
                    onClick={handleOpenConstructoras}
                >
                    AGREGAR CONSTRUCTORA
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={handleOpenInformacionProyecto}
                >
                    AGREGAR PROYECTO
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={formularioBusquedaProyectos.handleSubmit(consultarProyectosFiltros)}
                >
                    CONSULTAR
                </Button>
            </div>
            <ProyectosTable
                proyectos={proyectos}
                loading={loading}
                onClickItem={onItemIsSelected}
            />
        </CHPrivateContainer>
    )
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllDepartments: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllDepartments
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Proyectos);