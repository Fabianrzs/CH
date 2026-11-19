import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Column } from 'material-table';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import {
    ConsultaTablaParametroForm,
} from '../constants/tablaParametroform.model.type';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CHSelect from '../../../components/ch-select/CHSelect';
import ApiCatalog from '../../../services/api-catalog.service';
import ApiCredit from '../../../services/api-credit.service';
import { VALORCATEGORIA, VALORTABLAPARAMETRO } from '../../../types/catalog.type';
import { TablaParametroDto, TablaTitleParametroDto } from '../../../types/tablaParametro.model.type';
import DatosTable from './tablaDataParametros';
import { AppState } from '../../../store/rootReducer';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise'

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
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        processButton: {
            backgroundColor: '#42b7ef',
            color: '#fff',
            width: '14%',
            marginLeft: '5%',
        },
    }),
);



const TablasParametrosInicio = (loginState: AppState) => {

    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const [selectedData, setSelectedData] = useState<TablaParametroDto | null>(
        null,
    );
    const [datosTablaParametro, setDatosTablasParametros] = useState<any[]>([]);
    const [dataTitle, setDataTitle] = useState<TablaTitleParametroDto[]>([]);
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [categorias, setCategorias] = useState<VALORCATEGORIA[]>(
        [],
    );
    const [tablasParametros, setTablasParametros] = useState<VALORTABLAPARAMETRO[]>(
        [],
    );

    const [descripcionSeleccionada, setDescripcionSeleccionada] = useState<string>("");
    const [loading, setLoading] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [valueCategoria, setValueCategoria] = useState(0);
    const [valueTablaParametro, setValueTableParametro] = useState('0');
    const [valueTablaParametroSeleccionado, setValueTableParametroSeleccionado] = useState('');

    const formularioConsulta = useForm({
        defaultValues: ConsultaTablaParametroForm,
    });

    useEffect(() => {
        consultarCatalogoListaCategorias();
    }, []);

    const onItemIsSelected = (data: TablaParametroDto | null) => {
        setSelectedData(data);
    };

    const onItemGoBack = () => {
        setSelectedData(null);
        setDataTitle([]);
        setIsDataLoaded(false);
        formularioConsulta.reset();
    };

    const consultarCatalogoListaCategorias = async () => {
        await apiCatalog
            .getAllCatalogoTablasParametros()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setCategorias(response?.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const tablasParametrosPorCategoria = async (idCategoria: number) => {
        setValueCategoria(idCategoria)
        const ID_INSTITUCIONES_ACREDITADAS = 4;
        const ID_JUZGADOS = 5;
        await apiCatalog
            .getTablasParametrosPorCategoria(idCategoria)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTablasParametros(response?.data.filter((item: any) => item.idTablaParametro !== ID_INSTITUCIONES_ACREDITADAS &&
                            item.idTablaParametro !== ID_JUZGADOS));
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const valueTablaParametroSelected = (value: string) => {
        setValueTableParametro(value)
    };

    const formatColumnName = (nombreColumna: string) => {
        const sinPrefijo = nombreColumna.startsWith('ENT_') ? nombreColumna.replace(/^ENT_/, '') : nombreColumna;


        switch (sinPrefijo) {
            case 'NOMBRE_RAZON_SOCIAL':
                return 'NOMBRE Y/O RAZON SOCIAL';
            case 'OTRO_NOMBRE_ESPECIAL':
                return 'TEXTO FORMATEADO ESPECIAL';
            // Añadir más casos específicos según sea necesario
            default:
                return sinPrefijo.replace(/_/g, ' ').toUpperCase();
        }
    };

    const datosParametro = tablasParametros.map(item => ({
        descripcion: item.descripcion,
    }));

    const load = () => {

    }

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

    const buscarDatosTablasParametros = async () => {
        try {
            const response = await apiCatalog.getDatosTablasParametros(valueTablaParametro);
            setValueTableParametroSeleccionado(valueTablaParametro);
            if (response?.isSuccess && response.data) {
                setDatosTablasParametros(response.data);

                let descripcionCategoria = 'No especificado';
                let descripcionParametro = 'No especificado';

                if (valueCategoria === 1) {
                    descripcionCategoria = 'ENTIDADES';
                } else if (valueCategoria === 3) {
                    descripcionCategoria = 'LISTA DE VALORES';
                } else if (valueCategoria === 2) {
                    descripcionCategoria = 'PROCESOS DE TRABAJO';
                }

                
                if (valueTablaParametro === 'ENT_ENTIDAD_BANCARIA') {
                    descripcionParametro = 'BANCOS';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'COOPERATIVAS';
                } else if (valueTablaParametro === 'FRZ_FUERZA') {
                    descripcionParametro = 'FUERZAS';
                } else if (valueTablaParametro === 'UEJ_UNIDADES_EJECUTORAS') {
                    descripcionParametro = 'UNIDADES EJECUTORAS';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'UNIDADES OPERATIVAS';
                }
                else if (valueTablaParametro === 'CTL_CATALOGO') {
                    descripcionParametro = 'CATÁLOGO';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'DIVISIÓN POLÍTICA';
                } else if (valueTablaParametro === 'EST_ESTADO') {
                    descripcionParametro = 'ESTADOS';
                } else if (valueTablaParametro === 'MDL_MODALIDAD_AFILIACION') {
                    descripcionParametro = 'MODALIDADES';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'RAZONES DE PAGO';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'REQUISITOS TIPO DE SOLICITUD';
                } else if (valueTablaParametro === 'TDC_TIPO_DOCUMENTO') {
                    descripcionParametro = 'TIPOS DE DOCUMENTO';
                } else if (valueTablaParametro === 'TIP_TIPO_NOVEDAD') {
                    descripcionParametro = 'TIPO DE NOVEDAD';
                } else if (valueTablaParametro === 'TSP_TIPO_SOLICITUD_POSTULANTE') {
                    descripcionParametro = 'TIPO DE SOLICITUD';
                } else if (valueTablaParametro === 'TVV_TIPO_VIVIENDA') {
                    descripcionParametro = 'TIPOS DE VIVIENDA';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'TIPOS INCONSISTENCIA DE CARGA';
                }

                else if (valueTablaParametro === 'CTG_CATEGORIA') {
                    descripcionParametro = 'CATEGORÍAS';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'DÍAS NO HÁBILES';
                } else if (valueTablaParametro === 'FRT_FORMATO') {
                    descripcionParametro = 'FORMATO CARGA Y GENERACIÓN DE ARCHIVOS';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'GRUPO DE FORMATOS';
                } else if (valueTablaParametro === 'PARAM_VALORES_IPC') {
                    descripcionParametro = 'I.P.C';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'PRESUPUESTO MASVI';
                } else if (valueTablaParametro === 'RGL_REGLAS') {
                    descripcionParametro = 'REGLAS DE NEGOCIO';
                } else if (valueTablaParametro === 'RGL_REGLAS') {
                    descripcionParametro = 'SALARIO MINIMO';
                } else if (valueTablaParametro === 'TAR_TIPO_ARCHIVO') {
                    descripcionParametro = 'TIPO DE ARCHIVOS';
                } else if (valueTablaParametro === 'PORDEFINIR') {
                    descripcionParametro = 'TIPO DE INCONSISTENCIAS';
                }  

                const user = AuthService.getUserInfo();
                const nav = window.navigator as any;
                const usrAg = nav.userAgentData.brands.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData.platform,
                    nombreUsuario: user.USR_USERNAME,
                    usurioId: user.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Consulta tablas parámetros',
                    hostname: document.location.host,
                    tipoAccion: 'CONSULTAS',
                    idTipoAccion: 3,
                    descripcion: `Consulta de tablas parámetros con los siguientes filtros:
                    - Categoría: ${descripcionCategoria || 'No especificado'}
                    - Tabla parámetro: ${descripcionParametro || 'No especificado'}`,
                } as Auditoria;
        
                auditoriaApi.InsertAuditoria(requestAuditoria);


            }
        } catch (e) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }
    };

    const buscarEsquemaTablasParametros = async () => {
        setLoading(true);
        try {
            const response = await apiCatalog.getEsquemasTablasParametros(valueTablaParametro);
            if (response?.isSuccess && response.data?.length > 0) {
                setDataTitle(response.data);
                await buscarDatosTablasParametros();
            } else {
                toast('No se encontraron resultados para la tabla seleccionada.', { type: toast.TYPE.WARNING, autoClose: 5000 });
            }
        } catch (error) {
            toast('Error del sistema. Por favor, intenta más tarde.', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }
    };

    const handleSearchButton = async () => {
        if (!isLoading) {
            setIsDataLoaded(false);
            await buscarEsquemaTablasParametros();
            setIsDataLoaded(true);
        }
    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Gestión de tablas parámetros"
                subText=""
                title1="Consulta"
                title2="Tablas parámetros"
            />
            <CHPaper title="Información tabla parámetro">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="categoria"
                            control={formularioConsulta.control}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Categoría"
                                    value={valueCategoria}
                                    onChange={(event) => {
                                        tablasParametrosPorCategoria(Number(event.target.value))
                                    }}
                                >
                                    <MenuItem key={0} value={0}>
                                        SELECCIONE...
                                    </MenuItem>
                                    {categorias?.map(
                                        (option: VALORCATEGORIA) => (
                                            <MenuItem
                                                key={option.idCategoria}
                                                value={option.idCategoria}
                                            >
                                                {option.categoria}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tabla"
                            control={formularioConsulta.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tabla parámetro"
                                    value={valueTablaParametro}
                                    onChange={
                                        (event) => {
                                            valueTablaParametroSelected(String(event.target.value))
                                        }
                                    }
                                >
                                    <MenuItem key={0} value={0}>
                                        SELECCIONE...
                                    </MenuItem>
                                    {tablasParametros?.map(
                                        (option: VALORTABLAPARAMETRO) => (
                                            <MenuItem
                                                key={option.idTablaParametro}
                                                value={option.tablaParametro}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={10} md={3}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={formularioConsulta.handleSubmit(handleSearchButton)}
                        >
                            BUSCAR DATOS DE TABLA

                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            <LoadingComponent loading={loading} />
            {isDataLoaded && (
                <DatosTable
                    data={datosTablaParametro}
                    dataTitle={dataTitle}
                    isDataLoaded={isDataLoaded}
                    loginState={loginState.login}
                    load={load}
                    descripcionSeleccionada={descripcionSeleccionada}
                    valueTablaParametroSeleccionado={valueTablaParametroSeleccionado}
                />
            )}
        </CHPrivateContainer>
    );
};

export default TablasParametrosInicio;
