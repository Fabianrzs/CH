import React, {
    useEffect,
    useState,
} from 'react';
import { toast } from 'react-toastify';
import moment from 'moment';
import {
    Button,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TablePagination,
    TextField,
    MenuItem,
    makeStyles,
    IconButton,
    Menu,
    Checkbox,
    FormControlLabel,
} from '@material-ui/core';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import {
    SaveAlt as SaveAltIcon,
    FilterList as FilterListIcon,
} from '@material-ui/icons';

import { LoadingV2Component } from '../loading/loadingV2.component';
import CreditApi from '../../services/api-credit.service';
import ApiReportes from '../../services/api-reportes.service';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import NotificationService from '../../services/notification.service';
import PageIndicators from '../page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../page-title-icon/page-title-icon';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import UtilService from '../../utils/util.service';
import ApiAuditorias from '../../services/api-auditorias.servise';

interface ReporteCredito {
    fechA_SOLICITUD: string;
    numerO_DOCUMENTO: string;
    nombrE_COMPLETO: string;
    estadoCredito: string;
    departamentO_ID: string;
    ciudaD_ID: string;
    puntO_ATENCION: string;
    tipO_CREDITO: string;
    valoR_INMUBLE: number;
    valoR_AVALUO: number;
    valoR_PRESTAMO: number;
    consecutivo: number;
    descripcionTarea: string;
    correoAsesor: string;
    nombreCompletoAsesor: string;
}

interface Abogado {
    usR_ID: string;
    usR_NOMBRE: string;
    usR_SEGUNDONOMBRE?: string | null;
    usR_APELLIDO: string;
    usR_SEGUNDOAPELLIDO?: string | null;
    usR_USERNAME: string;
    usR_IDENTIFICACION: string;
    usR_EMAIL: string;
    usR_ESTADO: boolean;
}

const useStyles = makeStyles({
    tableHeader: {
        backgroundColor: '#FEC917',
        fontWeight: 'bold',
    },
});

const columnasDisponibles = [
    {
        id: 'fechA_SOLICITUD',
        label: 'Fecha Solicitud',
    },
    {
        id: 'numerO_DOCUMENTO',
        label: 'Documento',
    },
    {
        id: 'nombrE_COMPLETO',
        label: 'Nombre',
    },
    {
        id: 'estadoCredito',
        label: 'Estado Crédito',
    },
    {
        id: 'departamentO_ID',
        label: 'Departamento',
    },
    {
        id: 'ciudaD_ID',
        label: 'Ciudad',
    },
    {
        id: 'puntO_ATENCION',
        label: 'Punto Atención',
    },
    {
        id: 'tipO_CREDITO',
        label: 'Tipo Crédito',
    },
    {
        id: 'valoR_INMUBLE',
        label: 'Valor Inmueble',
    },
    {
        id: 'valoR_AVALUO',
        label: 'Valor Avalúo',
    },
    {
        id: 'valoR_PRESTAMO',
        label: 'Valor Préstamo',
    },
    {
        id: 'descripcionTarea',
        label: 'Descripción Tarea',
    },
    {
        id: 'correoAsesor',
        label: 'Correo Asesor',
    },
    {
        id: 'nombreCompletoAsesor',
        label: 'Nombre Completo Asesor',
    },
];

const ControlAbogados = () => {
    const apiCredit = CreditApi();
    const apiReportes = ApiReportes();
    const catalogApi = CatalogApi();

    const classes = useStyles();

    const [
        fechaInicio,
        setFechaInicio,
    ] = useState<Date | null>(null);

    const [
        fechaFinal,
        setFechaFinal,
    ] = useState<Date | null>(null);

    const [
        numeroDocumento,
        setNumeroDocumento,
    ] = useState<string>('');

    const [
        consecutivoSolicitud,
        setConsecutivoSolicitud,
    ] = useState<string>('');

    const [
        estadoTarea,
        setEstadoTarea,
    ] = useState<any>(0);

    const [
        data,
        setData,
    ] = useState<ReporteCredito[]>([]);

    const [
        page,
        setPage,
    ] = useState(0);

    const [
        rowsPerPage,
        setRowsPerPage,
    ] = useState(10);

    const [
        isLoading,
        setIsLoading,
    ] = useState(false);

    const [
        publicIpAddress,
        setPublicIpAddress,
    ] = useState<string | null>(null);

    const [
        localIpAddress,
        setLocalIpAddress,
    ] = useState<string | null>(null);

    const [
        abogadoSeleccionado,
        setAbogadoSeleccionado,
    ] = useState<string>('');

    const [
        listaAbogados,
        setListaAbogados,
    ] = useState<Abogado[]>([]);

    const [
        listaEstadoTareas,
        setListaEstadoTareas,
    ] = useState<any[]>([]);

    const [
        columnasVisibles,
        setColumnasVisibles,
    ] = useState<string[]>(
        columnasDisponibles.map(
            (columna) => columna.id,
        ),
    );

    const [
        anchorEl,
        setAnchorEl,
    ] = useState<null | HTMLElement>(null);

    const handleOpenMenu = (
        event: React.MouseEvent<HTMLElement>,
    ) => {
        setAnchorEl(event.currentTarget);
    };

    const handleCloseMenu = () => {
        setAnchorEl(null);
    };

    const limpiarFiltros = () => {
        setFechaInicio(null);
        setFechaFinal(null);
        setNumeroDocumento('');
        setConsecutivoSolicitud('');
        setEstadoTarea(0);
        setAbogadoSeleccionado('');
        setData([]);
        setPage(0);
    };

    const obtenerAbogados = async () => {
        try {
            /*
             * 1 = Peritos
             * 2 = Abogados
             */
            const response =
                await apiCredit.getconsultarusuariocredito(
                    '2',
                );

            if (response?.isSuccess) {
                setListaAbogados(
                    response.data || [],
                );
            } else {
                setListaAbogados([]);
            }
        } catch (error) {
            setListaAbogados([]);

            toast(
                'No fue posible consultar los abogados',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        }
    };

    const obtenerEstadosTareas =
        async () => {
            try {
                const response =
                    await catalogApi.getActivityStatus();

                if (response?.isSuccess) {
                    setListaEstadoTareas(
                        response.data || [],
                    );
                } else {
                    setListaEstadoTareas([]);
                }
            } catch (error) {
                setListaEstadoTareas([]);
            }
        };

    useEffect(() => {
        obtenerAbogados();
        obtenerEstadosTareas();
    }, []);

    const isValidIPv4 = (
        ip: string,
    ) => {
        const ipv4Regex =
            /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;

        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (
        ip: string,
    ) => {
        const ipv6Regex =
            /([a-f0-9:]+:+)+[a-f0-9]+/i;

        return ipv6Regex.test(ip);
    };

    const enviarIp = async (
        ip: string,
    ) => {
        try {
            const convertIp =
                ip.split('.')[0];

            const hexadecimal =
                convertIp;

            const response =
                await catalogApi.getObtenerIpLocal(
                    hexadecimal,
                );

            if (
                response &&
                response.isSuccess
            ) {
                setPublicIpAddress(
                    response.data,
                );
            }
        } catch (error) {
            toast(
                'Error del sistema',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        }
    };

    useEffect(() => {
        const peerConnection =
            new RTCPeerConnection({
                iceServers: [],
            });

        peerConnection.createDataChannel('');

        peerConnection
            .createOffer()
            .then((offer) =>
                peerConnection.setLocalDescription(
                    offer,
                ),
            )
            .catch(() => {
                peerConnection.close();
            });

        peerConnection.onicecandidate = (
            event,
        ) => {
            if (!event.candidate) {
                return;
            }

            const ip: any =
                event.candidate.address;

            setLocalIpAddress(ip);

            if (
                isValidIPv4(ip) ||
                isValidIPv6(ip)
            ) {
                enviarIp(ip);
            } else {
                enviarIp(ip);
            }
        };

        return () => {
            peerConnection.close();
        };
    }, []);

    const handleBuscar = async () => {
        setIsLoading(true);

        const request = {
            fechaInicio: fechaInicio
                ? fechaInicio
                      .toISOString()
                      .split('T')[0]
                : null,

            fechaFinal: fechaFinal
                ? fechaFinal
                      .toISOString()
                      .split('T')[0]
                : null,

            consecutivoSolicitud:
                consecutivoSolicitud.trim() !==
                ''
                    ? Number(
                          consecutivoSolicitud,
                      )
                    : null,

            numeroDocumento:
                numeroDocumento.trim() !== ''
                    ? numeroDocumento
                    : null,

            estadoCredito:
                estadoTarea &&
                Number(estadoTarea) !== 0
                    ? Number(estadoTarea)
                    : null,

            /*
             * Igual que Control Perito:
             * se envía el usuario seleccionado.
             */
            usuarioId:
                abogadoSeleccionado !== ''
                    ? abogadoSeleccionado
                    : null,
        };

        const auditoriaApi =
            ApiAuditorias();

        const user =
            AuthService.getUserInfo();

        const nav =
            window.navigator as any;

        const usrAg =
            nav.userAgentData?.brands?.find(
                (x: {
                    brand: string;
                }) =>
                    x.brand ===
                        'Google Chrome' ||
                    x.brand ===
                        'Microsoft Edge',
            )?.brand;

        try {
            const response =
                await apiCredit.postReportesSolicitudCreditoGeneralUsuarioExterno(
                    request,
                );

            if (response?.isSuccess) {
                setData(
                    response.data || [],
                );

                setPage(0);

                if (
                    !response.data ||
                    response.data.length === 0
                ) {
                    toast(
                        'No hay créditos para estos filtros',
                        {
                            type: toast.TYPE.WARNING,
                        },
                    );
                }

                const abogado =
                    listaAbogados.find(
                        (item) =>
                            item.usR_ID ===
                            abogadoSeleccionado,
                    );

                const nombreAbogado =
                    abogado
                        ? [
                              abogado.usR_NOMBRE,
                              abogado.usR_SEGUNDONOMBRE,
                              abogado.usR_APELLIDO,
                              abogado.usR_SEGUNDOAPELLIDO,
                          ]
                              .filter(Boolean)
                              .join(' ')
                              .replace(
                                  /\s+/g,
                                  ' ',
                              )
                              .trim()
                        : 'Todos';

                const requestAuditoria = {
                    navegador: usrAg,

                    sistemaOperativo:
                        nav
                            .userAgentData
                            ?.platform,

                    nombreUsuario:
                        user.USR_USERNAME,

                    usurioId:
                        user.USR_ID,

                    ip: publicIpAddress,

                    modulo:
                        'Control Abogados',

                    hostname:
                        document.location
                            .host,

                    tipoAccion:
                        'CONSULTAS',

                    idTipoAccion: 3,

                    descripcion:
                        `Consulta Control Abogados:
Número documento: ${request.numeroDocumento}
Consecutivo: ${request.consecutivoSolicitud}
Estado: ${request.estadoCredito}
Abogado: ${nombreAbogado}
Usuario abogado ID: ${request.usuarioId}
Fecha inicio: ${request.fechaInicio}
Fecha final: ${request.fechaFinal}`,
                } as Auditoria;

                auditoriaApi.InsertAuditoria(
                    requestAuditoria,
                );
            } else {
                setData([]);

                toast(
                    response?.returnMessage ||
                        'No hay créditos para estos filtros',
                    {
                        type: toast.TYPE.WARNING,
                    },
                );
            }
        } catch (error: any) {
            setData([]);

            const descripcionError =
                `Error en consulta Control Abogados
Número documento: ${request.numeroDocumento}
Consecutivo: ${request.consecutivoSolicitud}
Estado: ${request.estadoCredito}
Abogado ID: ${request.usuarioId}
Fecha inicio: ${request.fechaInicio}
Fecha final: ${request.fechaFinal}
Error: ${
                    error?.message ||
                    'Error desconocido'
                }`;

            const requestAuditoriaError:
                Auditoria = {
                navegador: usrAg,

                sistemaOperativo:
                    nav.userAgentData
                        ?.platform,

                nombreUsuario:
                    user?.USR_USERNAME,

                usurioId:
                    user?.USR_ID,

                ip: publicIpAddress,

                modulo:
                    'Control Abogados',

                hostname:
                    document.location
                        .host,

                tipoAccion:
                    'ERRORES Y EXCEPCIONES',

                idTipoAccion: 8,

                descripcion:
                    descripcionError,
            };

            auditoriaApi.InsertAuditoria(
                requestAuditoriaError,
            );

            toast(
                'Ocurrió un error al realizar la consulta.',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        } finally {
            setIsLoading(false);
        }
    };

    const exportToExcel = async () => {
        const columnDataList =
            data.map((row: any) => ({
                data: [
                    String(
                        row.fechA_SOLICITUD ??
                            '',
                    ),
                    String(
                        row.numerO_DOCUMENTO ??
                            '',
                    ),
                    String(
                        row.nombrE_COMPLETO ??
                            '',
                    ),
                    String(
                        row.estadoCredito ??
                            '',
                    ),
                    String(
                        row.departamentO_ID ??
                            '',
                    ),
                    String(
                        row.ciudaD_ID ??
                            '',
                    ),
                    String(
                        row.puntO_ATENCION ??
                            '',
                    ),
                    String(
                        row.tipO_CREDITO ??
                            '',
                    ),
                    String(
                        row.valoR_INMUBLE ??
                            '',
                    ),
                    String(
                        row.valoR_AVALUO ??
                            '',
                    ),
                    String(
                        row.valoR_PRESTAMO ??
                            '',
                    ),
                    String(
                        row.descripcionTarea ??
                            '',
                    ),
                    String(
                        row.correoAsesor ??
                            '',
                    ),
                    String(
                        row.nombreCompletoAsesor ??
                            '',
                    ),
                ],
            }));

        const request = {
            columnDataList:
                columnDataList.map(
                    (item) => ({
                        data: item.data,
                    }),
                ),

            columnName: [
                'Fecha Solicitud',
                'Documento',
                'Nombre',
                'Estado Crédito',
                'Departamento',
                'Ciudad',
                'Punto Atención',
                'Tipo Crédito',
                'Valor Inmueble',
                'Valor Avalúo',
                'Valor Préstamo',
                'Descripción Tarea',
                'Correo Asesor',
                'Nombre Completo Asesor',
            ],
        };

        const auditoriaApi =
            ApiAuditorias();

        const user =
            AuthService.getUserInfo();

        const nav =
            window.navigator as any;

        const usrAg =
            nav.userAgentData?.brands?.find(
                (x: {
                    brand: string;
                }) =>
                    x.brand ===
                        'Google Chrome' ||
                    x.brand ===
                        'Microsoft Edge',
            )?.brand;

        try {
            const response =
                await apiReportes.GenerarExcels(
                    request,
                );

            const blob =
                new Blob(
                    [response],
                    {
                        type:
                            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                    },
                );

            const url =
                window.URL.createObjectURL(
                    blob,
                );

            const link =
                document.createElement(
                    'a',
                );

            link.href = url;

            link.download =
                `Control_Abogados_${moment().format(
                    'YYYYMMDD_HHmmss',
                )}.xlsx`;

            document.body.appendChild(
                link,
            );

            link.click();

            document.body.removeChild(
                link,
            );

            window.URL.revokeObjectURL(
                url,
            );

            const requestAuditoria = {
                navegador: usrAg,

                sistemaOperativo:
                    nav.userAgentData
                        ?.platform,

                nombreUsuario:
                    user.USR_USERNAME,

                usurioId:
                    user.USR_ID,

                ip: publicIpAddress,

                modulo:
                    'Control Abogados',

                hostname:
                    document.location
                        .host,

                tipoAccion:
                    'DESCARGAS',

                idTipoAccion: 5,

                descripcion:
                    'Descarga de archivo Excel Control Abogados',
            } as Auditoria;

            auditoriaApi.InsertAuditoria(
                requestAuditoria,
            );
        } catch (error: any) {
            const descripcionError =
                `Error en descarga de archivo Control Abogados.
Error: ${
                    error?.message ||
                    'Error desconocido'
                }
Mensaje API: ${
                    error?.response?.data
                        ?.returnMessage ||
                    'No disponible'
                }
Código HTTP: ${
                    error?.response?.status ||
                    'No disponible'
                }`;

            const requestAuditoriaError:
                Auditoria = {
                navegador: usrAg,

                sistemaOperativo:
                    nav.userAgentData
                        ?.platform,

                nombreUsuario:
                    user?.USR_USERNAME,

                usurioId:
                    user?.USR_ID,

                ip: publicIpAddress,

                modulo:
                    'Control Abogados',

                hostname:
                    document.location
                        .host,

                tipoAccion:
                    'ERRORES Y EXCEPCIONES',

                idTipoAccion: 8,

                descripcion:
                    descripcionError,
            };

            auditoriaApi.InsertAuditoria(
                requestAuditoriaError,
            );

            NotificationService.genericAlert(
                'Error',
                'No se pudo generar el archivo.',
                'error',
            );
        }
    };

    const handleChangePage = (
        event: unknown,
        newPage: number,
    ) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement>,
    ) => {
        setRowsPerPage(
            parseInt(
                event.target.value,
                10,
            ),
        );

        setPage(0);
    };

    return (
        <div>
            <LoadingV2Component
                key="loader"
                loading={isLoading}
                tittle="Consultando..."
            />

            <PageIndicators
                topTextNivel1="Control Abogados"
                topTextNivel2="Consulta"
            />

            <IconTitle
                title="Control Abogados"
                subTitle=""
                icon="fch-Vigencia-GMF-1-2"
            />

            <MuiPickersUtilsProvider
                utils={DateFnsUtils}
            >
                <Paper
                    style={{
                        padding: 24,
                        marginTop: 20,
                    }}
                >
                    <Grid
                        container
                        spacing={3}
                        alignItems="center"
                    >
                        <Grid
                            item
                            xs={12}
                            sm={4}
                        >
                            <TextField
                                label="Número Documento"
                                value={
                                    numeroDocumento
                                }
                                onChange={(e) =>
                                    setNumeroDocumento(
                                        e.target.value.replace(
                                            /[^0-9]/g,
                                            '',
                                        ),
                                    )
                                }
                                fullWidth
                                variant="standard"
                                type="number"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        </Grid>

                        <Grid
                            item
                            xs={12}
                            sm={4}
                        >
                            <TextField
                                label="Consecutivo Solicitud"
                                value={
                                    consecutivoSolicitud
                                }
                                onChange={(e) =>
                                    setConsecutivoSolicitud(
                                        e.target.value.replace(
                                            /[^0-9]/g,
                                            '',
                                        ),
                                    )
                                }
                                fullWidth
                                variant="standard"
                                type="number"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        </Grid>

                        <Grid
                            item
                            xs={12}
                            sm={4}
                        >
                            <TextField
                                label="Estado Tarea"
                                select
                                value={
                                    estadoTarea
                                }
                                onChange={(e) =>
                                    setEstadoTarea(
                                        e.target.value,
                                    )
                                }
                                fullWidth
                                variant="standard"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            >
                                <MenuItem
                                    value={0}
                                >
                                    <em>
                                        Todos
                                    </em>
                                </MenuItem>

                                {listaEstadoTareas.map(
                                    (
                                        option,
                                    ) => (
                                        <MenuItem
                                            key={
                                                option.id
                                            }
                                            value={
                                                option.id
                                            }
                                        >
                                            {
                                                option.descripcion
                                            }
                                        </MenuItem>
                                    ),
                                )}
                            </TextField>
                        </Grid>

                        <Grid
                            item
                            xs={12}
                            sm={4}
                        >
                            <TextField
                                label="Abogado"
                                select
                                value={
                                    abogadoSeleccionado
                                }
                                onChange={(
                                    e,
                                ) =>
                                    setAbogadoSeleccionado(
                                        e.target
                                            .value as string,
                                    )
                                }
                                fullWidth
                                variant="standard"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            >
                                <MenuItem value="">
                                    <em>
                                        Todos los abogados
                                    </em>
                                </MenuItem>

                                {listaAbogados.map(
                                    (
                                        option,
                                    ) => {
                                        const nombreCompleto =
                                            [
                                                option.usR_NOMBRE,
                                                option.usR_SEGUNDONOMBRE,
                                                option.usR_APELLIDO,
                                                option.usR_SEGUNDOAPELLIDO,
                                            ]
                                                .filter(
                                                    Boolean,
                                                )
                                                .join(
                                                    ' ',
                                                )
                                                .replace(
                                                    /\s+/g,
                                                    ' ',
                                                )
                                                .trim();

                                        return (
                                            <MenuItem
                                                key={
                                                    option.usR_ID
                                                }
                                                value={
                                                    option.usR_ID
                                                }
                                            >
                                                {
                                                    nombreCompleto
                                                }
                                            </MenuItem>
                                        );
                                    },
                                )}
                            </TextField>
                        </Grid>

                        <Grid
                            item
                            xs={12}
                            sm={4}
                        >
                            <CHDatePicker
                                label="Fecha Inicio"
                                format="yyyy-MM-dd"
                                value={
                                    fechaInicio
                                }
                                onChange={
                                    setFechaInicio
                                }
                                fullWidth
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        </Grid>

                        <Grid
                            item
                            xs={12}
                            sm={4}
                        >
                            <CHDatePicker
                                label="Fecha Final"
                                format="yyyy-MM-dd"
                                value={
                                    fechaFinal
                                }
                                onChange={
                                    setFechaFinal
                                }
                                fullWidth
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        </Grid>

                        <Grid
                            item
                            xs={6}
                            sm={2}
                        >
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={
                                    handleBuscar
                                }
                                fullWidth
                            >
                                Buscar
                            </Button>
                        </Grid>

                        <Grid
                            item
                            xs={6}
                            sm={2}
                        >
                            <Button
                                variant="outlined"
                                color="secondary"
                                onClick={
                                    limpiarFiltros
                                }
                                fullWidth
                            >
                                Limpiar
                            </Button>
                        </Grid>
                    </Grid>

                    {data.length > 0 && (
                        <TableContainer
                            component={Paper}
                            style={{
                                marginTop:
                                    '50px',
                            }}
                        >
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell
                                            colSpan={
                                                columnasVisibles.length
                                            }
                                            align="right"
                                        >
                                            <Button
                                                startIcon={
                                                    <SaveAltIcon />
                                                }
                                                variant="outlined"
                                                color="secondary"
                                                onClick={
                                                    exportToExcel
                                                }
                                                style={{
                                                    marginRight: 8,
                                                }}
                                            >
                                                Descargar Excel
                                            </Button>

                                            <IconButton
                                                onClick={
                                                    handleOpenMenu
                                                }
                                                aria-label="Filtrar columnas"
                                            >
                                                <FilterListIcon />
                                            </IconButton>

                                            <Menu
                                                anchorEl={
                                                    anchorEl
                                                }
                                                keepMounted
                                                open={Boolean(
                                                    anchorEl,
                                                )}
                                                onClose={
                                                    handleCloseMenu
                                                }
                                            >
                                                {columnasDisponibles.map(
                                                    (
                                                        col,
                                                    ) => (
                                                        <MenuItem
                                                            key={
                                                                col.id
                                                            }
                                                        >
                                                            <FormControlLabel
                                                                control={
                                                                    <Checkbox
                                                                        id={`chk-${col.id}`}
                                                                        checked={columnasVisibles.includes(
                                                                            col.id,
                                                                        )}
                                                                        onChange={() => {
                                                                            setColumnasVisibles(
                                                                                (
                                                                                    prev,
                                                                                ) =>
                                                                                    prev.includes(
                                                                                        col.id,
                                                                                    )
                                                                                        ? prev.filter(
                                                                                              (
                                                                                                  id,
                                                                                              ) =>
                                                                                                  id !==
                                                                                                  col.id,
                                                                                          )
                                                                                        : [
                                                                                              ...prev,
                                                                                              col.id,
                                                                                          ],
                                                                            );
                                                                        }}
                                                                    />
                                                                }
                                                                label={
                                                                    col.label
                                                                }
                                                            />
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </Menu>
                                        </TableCell>
                                    </TableRow>

                                    <TableRow
                                        className={
                                            classes.tableHeader
                                        }
                                    >
                                        {columnasDisponibles
                                            .filter(
                                                (
                                                    col,
                                                ) =>
                                                    columnasVisibles.includes(
                                                        col.id,
                                                    ),
                                            )
                                            .map(
                                                (
                                                    col,
                                                ) => (
                                                    <TableCell
                                                        key={
                                                            col.id
                                                        }
                                                    >
                                                        {
                                                            col.label
                                                        }
                                                    </TableCell>
                                                ),
                                            )}
                                    </TableRow>
                                </TableHead>

                                <TableBody>
                                    {data
                                        .slice(
                                            page *
                                                rowsPerPage,
                                            page *
                                                rowsPerPage +
                                                rowsPerPage,
                                        )
                                        .map(
                                            (
                                                row,
                                                idx,
                                            ) => (
                                                <TableRow
                                                    key={`${row.consecutivo}-${idx}`}
                                                >
                                                    {columnasVisibles.includes(
                                                        'fechA_SOLICITUD',
                                                    ) && (
                                                        <TableCell>
                                                            {row.fechA_SOLICITUD
                                                                ? new Date(
                                                                      row.fechA_SOLICITUD,
                                                                  ).toLocaleDateString()
                                                                : ''}
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'numerO_DOCUMENTO',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.numerO_DOCUMENTO
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'nombrE_COMPLETO',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.nombrE_COMPLETO
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'estadoCredito',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.estadoCredito
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'departamentO_ID',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.departamentO_ID
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'ciudaD_ID',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.ciudaD_ID
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'puntO_ATENCION',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.puntO_ATENCION
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'tipO_CREDITO',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.tipO_CREDITO
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'valoR_INMUBLE',
                                                    ) && (
                                                        <TableCell>
                                                            {UtilService.formatCurrency(
                                                                row.valoR_INMUBLE,
                                                            )}
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'valoR_AVALUO',
                                                    ) && (
                                                        <TableCell>
                                                            {UtilService.formatCurrency(
                                                                row.valoR_AVALUO,
                                                            )}
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'valoR_PRESTAMO',
                                                    ) && (
                                                        <TableCell>
                                                            {UtilService.formatCurrency(
                                                                row.valoR_PRESTAMO,
                                                            )}
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'descripcionTarea',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.descripcionTarea
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'correoAsesor',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.correoAsesor
                                                            }
                                                        </TableCell>
                                                    )}

                                                    {columnasVisibles.includes(
                                                        'nombreCompletoAsesor',
                                                    ) && (
                                                        <TableCell>
                                                            {
                                                                row.nombreCompletoAsesor
                                                            }
                                                        </TableCell>
                                                    )}
                                                </TableRow>
                                            ),
                                        )}
                                </TableBody>
                            </Table>

                            <TablePagination
                                component="div"
                                count={
                                    data.length
                                }
                                page={page}
                                onPageChange={
                                    handleChangePage
                                }
                                rowsPerPage={
                                    rowsPerPage
                                }
                                onRowsPerPageChange={
                                    handleChangeRowsPerPage
                                }
                                labelRowsPerPage="Filas por página"
                            />
                        </TableContainer>
                    )}
                </Paper>
            </MuiPickersUtilsProvider>
        </div>
    );
};

export default ControlAbogados;