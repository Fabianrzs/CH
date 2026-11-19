import {
    Box,
    Button,
    Card,
    CardContent,
    Container,
    createStyles,
    Grid,
    IconButton,
    makeStyles,
    MenuItem,
    Theme,
    Tooltip,
    Typography,
} from '@material-ui/core';
import { CancelOutlined, CheckCircle, CloudDownload, CloudDownloadOutlined, Done } from '@material-ui/icons';
import { format } from 'date-fns';
import MaterialTable from 'material-table';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import { useEffect, useMemo, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import React, { connect, useDispatch } from 'react-redux';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { useLocation } from 'react-router-dom';
import dayjs from 'dayjs';
import {
    startProcesarArchivosCargados,
    startConsultarArchivosCargados,
    startRechazarCargueNomina,
    startCrearSolicitudNomina,
    startObtenerSolicitudNomina
} from '../../../actions/cargue-nomina.action';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import { CargueNominaState } from '../../../reducers/cargue-nomina.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import ApiReportes from '../../../services/api-reportes.service';
import ApiCargueNomina from '../../../services/api-carguenomina.service';
import AuthService from '../../../services/auth.service';
import NotificationService from '../../../services/notification.service';
import { AppState } from '../../../store/rootReducer';
import { DocumentoCarga } from '../../../types/cargue-nomina.model.type';
import { EXECUTINGUNITDTO, GRADEDTO, IDENTIFICATIONTYPEDTO, STATEFILEDTO, TYPESFILEDTO } from '../../../types/catalog.type';
import UtilService from '../../../utils/util.service';
import ModalAprobarNomina from './modalAprobarNomina';
import ModalRechazarNomina from './modalRechazarNomina';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { startGetAllGrades, startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import { LoginState } from '../../../reducers/login.reducer';
import CatalogApi from '../../../services/api-catalog.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise'
import { AppDispatch } from '../../../store/store';
import CHTextField from '../../../components/ch-text-field/CHTextField';


const useStyles: any = makeStyles((theme: Theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnButton: {
            marginTop: 'auto',
            display: 'flex',
            justifyContent: 'center',
        },
        form: {
            marginTop: '50px',
            padding: '50px',
        },

        button: {
            width: '120px',
            margin: theme.spacing(1),
            marginBottom: '1rem',
        },
        smallTable: {
            "& .MuiTableCell-root": {
                fontSize: "12px",
                padding: "6px 8px",
                whiteSpace: "normal",
                wordBreak: "break-word",
                lineHeight: 1.2,
            },

            "& thead .MuiTableCell-root": {
                paddingTop: "10px",
                paddingBottom: "10px",
                lineHeight: 1.3,
                verticalAlign: "middle",
            },
        },
    }),
);

const DataTableNomina = (props: Props) => {

    const classes = useStyles();
    const {
        catalogState,
        cargueNominaState,
        loginState,
        startProcesarArchivosCargados,
        // startConsultarArchivosCargados,
        startRechazarCargueNomina,
        startCrearSolicitudNomina,
        startObtenerSolicitudNomina,
        startGetAllIdentificationTypes,
        startGetAllGrades,
    } = props;
    const apiReportes = ApiReportes();
    const apiCargueNomina = ApiCargueNomina()
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalRechazar, setOpenModalRechazar] = useState(false);
    const [itemRechazo, setItemRechazo] = useState([]);
    const [loading, setLoading] = useState(false);
    const location: any = useLocation();
    const tarea = location?.state?.tarea;
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllGrades(loginState.claims);
    }, []);

    const columns = [
        {
            // cuando se adapte el apiCargueNomina, cambiar todos los idDocumento por noSolicitud
            name: 'idDocumento',
            label: 'No. Solicitud',
            options: {
                setCellProps: () => ({ style: { width: 100, textAlign: 'center', whiteSpace: 'nowrap' } }),
                setCellHeaderProps: () => ({
                    style: { width: 100, textAlign: 'center', whiteSpace: 'normal', lineHeight: 1.1 },
                }),
            },
        },
        {
            name: 'idUnidadEjecutora',
            label: 'Unidad ejecutora',
            options: {
                setCellProps: () => ({ style: { textAlign: 'center', whiteSpace: 'normal', wordBreak: 'break-word' } }),
                customBodyRender: (value: string) => {
                    const id = Number(value);
                    const unidad = catalogState.ExecutingUnit?.find(item => item.id === id);
                    return unidad ? unidad.nombre : '';
                },
            },
        },
        {
            name: 'monto',
            label: 'Monto',
            options: {
                setCellProps: () => ({ style: { width: 110, textAlign: 'center', whiteSpace: 'nowrap' } }),
                setCellHeaderProps: () => ({ style: { width: 110, whiteSpace: 'nowrap' } }),
                customBodyRender: (value: any) => UtilService.formatCurrency(value),
            },
        },
        {
            name: 'idTipoArchivo',
            label: 'Tipo de archivo',
            options: {
                setCellProps: () => ({ style: { textAlign: 'center', whiteSpace: 'normal', wordBreak: 'break-word' } }),
                customBodyRender: (value: string) => {
                    const desc = catalogState.TypesFile?.find((item: { id: number }) => item.id === Number(value));
                    return desc ? desc.descripcion : '';
                },
            },
        },

        {
            name: 'documentoNombre',
            label: 'Nombre del archivo',
            options: {
                setCellProps: () => ({ style: { textAlign: 'center', whiteSpace: 'normal', wordBreak: 'break-word' } }),
                customBodyRender: (value: any) => <div style={{ textTransform: 'uppercase' }}>{value}</div>,
            },
        },
        {
            name: 'periodo',
            label: 'Período',
            options: {
                setCellProps: () => ({ style: { width: 90, textAlign: 'center', whiteSpace: 'nowrap' } }),
                setCellHeaderProps: () => ({ style: { width: 90, whiteSpace: 'nowrap' } }),
                customBodyRender: (value: any) => {
                    const s = value.toString();
                    return <span>{`${s.substring(0, 4)}/${s.substring(4, 6)}`}</span>;
                },
            },
        },
        {
            name: 'fechaCarga',
            label: 'Fecha de Cargue',
            options: {
                setCellProps: () => ({ style: { width: 110, textAlign: 'center', whiteSpace: 'nowrap' } }),
                setCellHeaderProps: () => ({ style: { width: 110, whiteSpace: 'nowrap' } }),
                customBodyRender: (value: any) => {
                    const s = value.toString();
                    return <span>{`${s.substring(0, 2)}/${s.substring(2, 4)}/${s.substring(4, 8)}`}</span>;
                },
            },
        },
        {
            name: 'estado',
            label: 'Estado',
            options: {
                setCellProps: () => ({ style: { textAlign: 'center', whiteSpace: 'normal', wordBreak: 'break-word' } }),
            },
        },
        {
            name: 'registros',
            label: 'Registros',
            options: {
                setCellProps: () => ({ style: { width: 80, textAlign: 'center', whiteSpace: 'nowrap' } }),
                setCellHeaderProps: () => ({ style: { width: 80, whiteSpace: 'nowrap' } }),
            },
        },
        {
            name: 'usuario',
            label: 'usuario',
            options: {
                setCellProps: () => ({ style: { textAlign: 'center', whiteSpace: 'normal', wordBreak: 'break-word' } }),
            },
        },
        {
            name: '',
            label: 'Acciones',
            options: {
                setCellProps: () => ({
                    style: { width: 70, textAlign: 'center', verticalAlign: 'middle' },
                }),
                setCellHeaderProps: () => ({ style: { width: 120, whiteSpace: 'nowrap' } }),
                customBodyRender: (value: any, tableMeta: any, rowData: any) => {
                    const idDocumento = tableMeta.rowData[0];
                    const datosRegistro = dataFiltrada.find((item: any) => item.idDocumento === idDocumento);

                    if (!datosRegistro) return null;

                    const { estado } = datosRegistro;

                    const ActionsCol = ({ children }: any) => (
                        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 4 }}>
                            {children}
                        </div>
                    );


                    if (estado === 'PENDIENTE POR AUTORIZAR') {
                        return (
                            <div>
                                <Tooltip title="Autorizar Nómina">
                                    <IconButton
                                        onClick={(e) => {
                                            e?.stopPropagation();
                                            procesarArchivosCargados(datosRegistro);
                                        }}
                                    >
                                        <CheckCircle />
                                    </IconButton>
                                </Tooltip>

                                <Tooltip title="Rechazar Nómina">
                                    <IconButton
                                        onClick={(e) => {
                                            e?.stopPropagation();
                                            AbrirModalRechazar(tableMeta);
                                        }}
                                    >
                                        <CancelOutlined />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title="Descargar txt">
                                    <IconButton
                                        onClick={(e) => {
                                            e?.stopPropagation();
                                            descargarTxt(tableMeta);
                                        }}
                                    >
                                        <CloudDownloadOutlined />
                                    </IconButton>
                                </Tooltip>
                            </div>
                        );
                    }

                    if (estado === 'PENDIENTE POR PROCESAR') {
                        return (
                            <Tooltip title="Procesar Nómina">
                                <IconButton
                                    onClick={(e) => {
                                        e?.stopPropagation();
                                        AbrirModalAprobar(tableMeta);
                                    }}
                                >
                                    <Done />
                                </IconButton>
                            </Tooltip>
                        );
                    }


                    if (estado === 'PENDIENTE POR APROBAR') {
                        return (
                            <Tooltip title="Aprobar Nómina">
                                <IconButton
                                    onClick={(e) => {
                                        e?.stopPropagation();
                                        AbrirModalAprobar(tableMeta);
                                    }}
                                >
                                    <Done />
                                </IconButton>
                            </Tooltip>
                        );
                    }
                    if (estado === 'APROBADO') {
                        return (

                            <Tooltip title="Descargar Excel">
                                <IconButton
                                    onClick={(e) => {
                                        e?.stopPropagation();
                                        decargaExcel(tableMeta);
                                    }}
                                >
                                    <CloudDownload style={{ color: 'green' }} />
                                </IconButton>
                            </Tooltip>

                        );
                    }

                    return null;
                },
            },
        }

    ];

    const formularioNomina = useForm<any>({

        defaultValues: {
            frz_Id: null,
            uej_Id: null,
            tdc_Id: null,
            dct_FechaCreacion: null,
            fechaInicioCarga: null,
            fechaFinalCarga: null,
            nombreArchivo: null,
            esd_Id: null,
            estado: null,

        },
    })

    const formularioRechazarNomina = useForm({
        defaultValues: {
            causalRechazo: "",
        }
    })



    const handleClearSelect = () => {
        formularioNomina.setValue('nombreArchivo', '');
        formularioNomina.setValue('frz_Id', null);
        formularioNomina.setValue('uej_Id', null);
        formularioNomina.setValue('tdc_Id', null);
        formularioNomina.setValue('dct_FechaCreacion', null);
        formularioNomina.setValue('fechaInicioCarga', null);
        formularioNomina.setValue('fechaFinalCarga', null);
        formularioNomina.setValue('esd_Id', null);
        formularioNomina.setValue('estado', null);
    }

    const handleExportToExcel = async () => {
        const columnNames = [
            "cuota",
            "fechaCorte",
            "fechaPago",
            "saldoLeasing",
            "capitalLeasing",
            "interesLeasing",
            "seguroVidaLeasing",
            "seguroHogar",
            "totalCuota",
        ];
        const columnDataList = cargueNominaState.consultaArchivosCargadosN.map((row: any) => ({
            data: columnNames.map((columnName) => String(row[columnName] || ''))
        }));

        if (columnDataList) {
            const request = {
                columnDataList: columnDataList.map(item => ({
                    data: item.data
                })),
                columnName: columnNames,
            };
            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Nómina-Fecha:${moment().format()}.csv`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
        }
    }

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

    useEffect(() => {
        if (tarea) {
            formularioNomina.setValue('uej_Id', Number(tarea.ueJ_USUARIO))
            formularioNomina.setValue('tdc_Id', tarea.nrN_TIPO_ARCHIVO)
            const periodo = dayjs(tarea.nrN_PERIODO, 'YYYYMM').toDate();
            formularioNomina.setValue('dct_FechaCreacion', periodo);
            const currentRequest: any = {
                frz_Id: null,
                uej_Id: Number(tarea.ueJ_USUARIO),
                tdc_Id: tarea.nrN_TIPO_ARCHIVO,
                dct_FechaCreacion: Number(tarea?.nrN_PERIODO),
                fechaInicioCarga: null,
                fechaFinalCarga: null,
                nombreArchivo: null,
                esd_Id: null,
                estado: null
            };
            startConsultarArchivosCargados(currentRequest)
        }
    }, [tarea]);

    const [appliedFilters, setAppliedFilters] = useState<any>(null);


    const ConsultaArchivosFiltro = async (data: any) => {
        const clean = (v: any) => {
            if (v === undefined || v === null) return null;
            if (typeof v === 'string') {
                const t = v.trim();
                return t.length ? t : null;
            }
            return v;
        };

        const { startNorm, endNorm } = normalizeDateRangeInclusive(
            data.fechaInicioCarga,
            data.fechaFinalCarga
        );

        const currentRequest: any = {
            idUnidadEjecutora: data.uej_Id != null ? Number(data.uej_Id) : null,
            idTipoArchivo: data.tdc_Id != null ? Number(data.tdc_Id) : null,
            periodo: data.dct_FechaCreacion
                ? parseInt(format(new Date(data.dct_FechaCreacion), 'yyyyMM'), 10)
                : null,
            fechaInicioCarga: clean(startNorm),
            fechaFinalCarga: clean(endNorm),
            documentoNombre: clean(data.nombreArchivo),
            estado: clean(data.estado),
            esdId: data.esd_Id != null ? Number(data.esd_Id) : null,
        };

        const initialRequest: any = {
            idUnidadEjecutora: null,
            idTipoArchivo: null,
            periodo: null,
            fechaInicioCarga: null,
            fechaFinalCarga: null,
            documentoNombre: null,
            estado: null,
            esdId: null,
        };

        const hasChanges = Object.keys(initialRequest).some((key) => {
            const a = clean(currentRequest[key]);
            const b = clean(initialRequest[key]);
            return a !== b;
        });

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            if (!hasChanges) {
                await NotificationService.genericAlert(
                    'Consulta Nómina',
                    'Debe seleccionar un filtro',
                    'warning',
                );
                return;
            }

            await dispatch(startConsultarArchivosCargados(currentRequest, true));


            setAppliedFilters(currentRequest);


            const unidadEjecutoraNombre =
                catalogState.ExecutingUnit?.find((item: EXECUTINGUNITDTO) => item.id === data.uej_Id)
                    ?.nombre || 'No especificada';

            const tipoArchivoNombre =
                catalogState.TypesFile?.find((item: TYPESFILEDTO) => item.id === data.tdc_Id)
                    ?.descripcion || 'No especificado';

            const periodoFormateado = data.dct_FechaCreacion
                ? format(new Date(data.dct_FechaCreacion), 'yyyy/MM')
                : 'No especificada';

            const fechaIniTxt = data.fechaInicioCarga
                ? format(new Date(data.fechaInicioCarga), 'yyyy-MM-dd')
                : 'No especificada';

            const fechaFinTxt = data.fechaFinalCarga
                ? format(new Date(data.fechaFinalCarga), 'yyyy-MM-dd')
                : 'No especificada';

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Cargue de Nómina',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta cargue de nómina con los siguientes filtros:
              - Unidad ejecutora: ${unidadEjecutoraNombre}
              - Tipo archivo: ${tipoArchivoNombre}
              - Periodo: ${periodoFormateado}
              - Fecha carga (desde): ${fechaIniTxt}
              - Fecha carga (hasta): ${fechaFinTxt}
              - Nombre documento: ${clean(data.nombreArchivo) ?? 'No especificado'}
              - Estado: ${clean(data.estado) ?? 'No especificado'}
            `,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al consultar cargue de nómina.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Cargue de Nómina',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar cargue de nómina', { type: toast.TYPE.ERROR });
        }
    };

    const dateString = (rowData: Date) => (
        <span>{UtilService.getFullDate(rowData)}</span>
    );

    const descargarTxt = async (item: any) => {
        console.log('ver', item)

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const id = item.rowData[0];
            const response = await apiCargueNomina.descarcarNominaTxt(id);

            const blob = new Blob([response.data], { type: 'text/plain;charset=utf-8' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `${item.rowData[4]}`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);

            const requestAuditoria: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Cargue de Nómina',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: 'Descarga de archivo de nómina'
            };

            auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error: any) {
            const descripcionError = `Error al generar archivo de nómina.
			- Error: ${error?.message || 'Error desconocido'}
			- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
			- Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Cargue de Nómina',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast('Error al descargar archivo de nómina', { type: 'error' });
        }
    };


    const procesarArchivosCargados = (item: any) => {
        swal({
            text: '¿Está seguro que desea Autorizar la Nómina?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {

                const req: DocumentoCarga = {
                    documentoId: item.idDocumento,
                };

                const user = AuthService.getUserInfo();
                const fechaActual = moment();
                const fechaActualFormateada = fechaActual.format();

                const req1: any = {
                    documentoId: item.idDocumento,
                    Monto: parseFloat(item.monto),
                    Periodo: item.periodo,
                    Registros: item.registros,
                    Nombre: item.documentoNombre,
                    TipoArchivo: item.tipoArchivo,
                    FechaInicialDocumento: fechaActualFormateada,
                    AutorizadoPor: user.USR_ID

                };

                startCrearSolicitudNomina(req1);
                startProcesarArchivosCargados([req]);
            }
        });
    }

    const rechazarArchivosCargados = async (data: any) => {

        const datosRegistro = { ...cargueNominaState.consultaArchivosCargadosN[data.rowIndex] };

        const user = AuthService.getUserInfo();
        const req: DocumentoCarga = {

            documentoId: String(datosRegistro.idDocumento),
            nombre: String(datosRegistro.documentoNombre),
            usuarioID: user.USR_ID,
            causalRechazoID: formularioRechazarNomina.getValues('causalRechazo')
        };

        const isConfirmed = await swal({
            title: 'Confirmación',
            text: '¿Está seguro de que desea rechazar la información?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
            dangerMode: true,
        });

        if (!isConfirmed) {
            return;
        }

        startRechazarCargueNomina([req]);

        handleCloseModalRechazar();
    }

    const AbrirModalAprobar = (item: any) => {
        startObtenerSolicitudNomina(item.rowData[0])
        setOpenModalAgregar(true);
    }

    const AbrirModalRechazar = (item: any) => {
        setOpenModalRechazar(true);
        setItemRechazo(item);
    }

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);

        const valoresFormulario = formularioNomina.getValues();

        const initialRequest: any = {
            frz_Id: null,
            uej_Id: null,
            tdc_Id: null,
            dct_FechaCreacion: null,
            fechaInicioCarga: null,
            fechaFinalCarga: null,
            nombreArchivo: null,
            esd_Id: null
        };
        const currentRequest: any = {
            frz_Id: valoresFormulario.uej_Id,
            uej_Id: valoresFormulario.uej_Id,
            tdc_Id: valoresFormulario.tdc_Id,
            dct_FechaCreacion: valoresFormulario.dct_FechaCreacion
                ? parseInt(format(new Date(valoresFormulario.dct_FechaCreacion), 'yyyyMM'), 10)
                : null,
            fechaInicioCarga: valoresFormulario.fechaInicioCarga || null,
            fechaFinalCarga: valoresFormulario.fechaFinalCarga || null,
            nombreArchivo: valoresFormulario.nombreArchivo || null,
            esd_Id: valoresFormulario.esd_Id || null
        };

        const hasChanges = Object.keys(currentRequest).some(
            (key) => currentRequest[key] !== initialRequest[key]
        );
        if (hasChanges) {
            startConsultarArchivosCargados(currentRequest)
        }


    };
    const handleCloseModalRechazar = async () => {
        setLoading(true)
        setOpenModalRechazar(false);
        const valoresFormulario = formularioNomina.getValues();

        const initialRequest: any = {
            frz_Id: null,
            uej_Id: null,
            tdc_Id: null,
            dct_FechaCreacion: null,
            fechaInicioCarga: null,
            fechaFinalCarga: null,
            nombreArchivo: null,
            esd_Id: null
        };
        const currentRequest: any = {
            frz_Id: valoresFormulario.uej_Id,
            uej_Id: valoresFormulario.uej_Id,
            tdc_Id: valoresFormulario.tdc_Id,
            dct_FechaCreacion: valoresFormulario.dct_FechaCreacion
                ? parseInt(format(new Date(valoresFormulario.dct_FechaCreacion), 'yyyyMM'), 10)
                : null,
            fechaInicioCarga: valoresFormulario.fechaInicioCarga || null,
            fechaFinalCarga: valoresFormulario.fechaFinalCarga || null,
            nombreArchivo: valoresFormulario.nombreArchivo || null,
            esd_Id: valoresFormulario.esd_Id || null
        };

        const hasChanges = Object.keys(currentRequest).some(
            (key) => currentRequest[key] !== initialRequest[key]
        );

        if (hasChanges) {
            try {
                const response = await apiCargueNomina.postConsultarArchivosRequest(currentRequest)
                if (response.isSuccess) {
                    setLoading(false)
                }
            } catch (error) {
                setLoading(false)
            }
            startConsultarArchivosCargados(currentRequest);
            setLoading(false)
        } else {
            setLoading(false)
            startConsultarArchivosCargados(currentRequest);
        }
    };
    const decargaExcel = async (item: any) => {
        setLoading(true);
        await apiCargueNomina
            .postConsultarArchivoCorregidoSolicitudNomina(item?.rowData[0])
            .then(async response => {
                if (response?.data) {
                    const nombreUnidadEjecutora = catalogState.ExecutingUnit.find((unidadEjecutora: EXECUTINGUNITDTO) => unidadEjecutora.id === response?.data?.encabezadoArchivosNomina?.ueJ_ID)
                    const informacionReporteExcel = {
                        columnNameEncabezado: [
                            "Nombre Unidad Ejecutora",
                            "Periodo Aporte"
                        ],
                        columnDataEncabezado: [
                            {
                                data: [
                                    nombreUnidadEjecutora?.nombre || '',
                                    `${String(response?.data?.encabezadoArchivosNomina?.eaN_PERIODO_APORTE).slice(0, 4)}/${String(response?.data?.encabezadoArchivosNomina?.eaN_PERIODO_APORTE).slice(-2)}`
                                ]
                            }

                        ],

                        columnNameCuerpo: [
                            "No.",
                            "ID",
                            "Unidad Ejecutora",
                            "Tipo Identificación",
                            "Código Militar",
                            "Grado",
                            "Dígito Fuerza",
                            "Nombre",
                            "Periodo",
                            "Base Cálculo",
                            "Embargo",
                            "Valor"
                        ],

                        columnDataCuerpo: response?.data?.cuerpoArchivosNomina?.map((item: any) => {
                            const tipoIdentificacion = catalogState.IdentificationType.find((identificacion: IDENTIFICATIONTYPEDTO) => identificacion.id === item.tiD_ID);
                            const grado = catalogState.Grade.find((gradoSeleccionado: GRADEDTO) => gradoSeleccionado.id === item.grD_ID);

                            return {
                                data: [
                                    String(item.caN_CONSECUTIVO),
                                    String(item.caN_ID),
                                    nombreUnidadEjecutora?.nombre,
                                    tipoIdentificacion?.descripcion,
                                    String(item.caN_COD_MILITAR),
                                    grado?.descripcion,
                                    String(item.caN_DIGITO_FUERZA),
                                    `${item.caN_PRIMER_NOMBRE} ${item.caN_SEGUNDO_NOMBRE || ''} ${item.caN_PRIMER_APELLIDO} ${item.caN_SEGUNDO_APELLIDO || ''}`,
                                    `${String(response?.data?.encabezadoArchivosNomina?.eaN_PERIODO_APORTE).slice(0, 4)}/${String(response?.data?.encabezadoArchivosNomina?.eaN_PERIODO_APORTE).slice(-2)}`,
                                    UtilService.formatCurrency(item.caN_BASE_CALCULO),
                                    item.caN_EMBARGO === 0 ? 'NO' : 'SI',
                                    UtilService.formatCurrency(item.caN_VALOR)
                                ]
                            };
                        }),

                        columnNameFin: [
                            "Total Registros",
                            "Total Neto",
                            "Indicador Control"
                        ],

                        columnDataFin: [
                            {
                                data: [
                                    String(response?.data?.finArchivosNomina?.faN_CONSECUTIVO),
                                    UtilService.formatCurrency(response?.data?.finArchivosNomina?.faN_SUMATORIA_APORTE),
                                    String(response?.data?.finArchivosNomina?.faN_INDICADOR_CONTROL),
                                ]
                            }

                        ]
                    };

                    await apiReportes.GenerarReporteExcelSolicitudNomina(informacionReporteExcel)
                        .then(responseReporte => {
                            if (responseReporte) {
                                const blob = new Blob([responseReporte], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                                const url = window.URL.createObjectURL(blob);
                                const link = document.createElement('a');
                                link.href = url;
                                link.download = `${cargueNominaState.consultaArchivosCargadosN[0].documentoNombre.substring(0, cargueNominaState.consultaArchivosCargadosN[0].documentoNombre.lastIndexOf('.')).replace(',', '')}.xlsx`;
                                document.body.appendChild(link);
                                link.click();
                                window.URL.revokeObjectURL(url);
                                setLoading(false);

                                const auditoriaApi = AuditoriaApi();
                                const user = AuthService.getUserInfo();
                                const nav = window.navigator as any;
                                const usrAg = nav.userAgentData.brands.find(
                                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                                )?.brand;

                                const requestAuditoria = {
                                    navegador: usrAg,
                                    sistemaOperativo: nav.userAgentData.platform,
                                    nombreUsuario: user.USR_USERNAME,
                                    usurioId: user.USR_ID,
                                    ip: publicIpAddress,
                                    modulo: 'Cargue Nómina',
                                    hostname: document.location.host,
                                    tipoAccion: 'DESCARGAS',
                                    idTipoAccion: 5,
                                    descripcion: 'Descarga de reporte solicitud nomina'

                                } as Auditoria;

                                auditoriaApi.InsertAuditoria(requestAuditoria);
                            }
                        })
                        .catch((e) => {
                            setLoading(false);
                            toast(`Error al generar reporte excel ${e.message}`, { type: toast.TYPE.ERROR });
                        })
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error al consultar informacion archivo nomina ${e.message}`, { type: toast.TYPE.ERROR });
            });

    }

    const normalizeDateRangeInclusive = (start: any, end: any) => {
        const startNorm = start ? dayjs(start).startOf('day').toISOString() : null;
        const endNorm = end ? dayjs(end).endOf('day').toISOString() : null;
        return { startNorm, endNorm };
    };

    const ESTADOS = [
        'PENDIENTE POR AUTORIZAR',
        'RECHAZADO',
        'PENDIENTE POR PROCESAR',
        'PENDIENTE POR APROBAR',
        'PROCESADO',
    ];

    const norm = (v: any) => (v ?? '').toString().trim().toUpperCase();

    const inInclusiveRange = (
        dateISO: any,
        startISO: any,
        endISO: any
    ) => {
        if (!dateISO) return false;

        const d = dayjs(dateISO);
        if (!d.isValid()) return false;

        const time = d.valueOf();

        const hasStart = !!startISO;
        const hasEnd = !!endISO;

        const startTime = hasStart ? dayjs(startISO).valueOf() : null;
        const endTime = hasEnd ? dayjs(endISO).valueOf() : null;

        if (!hasStart && !hasEnd) return true;

        if (hasStart && !hasEnd) {
            return time >= (startTime as number);
        }

        if (!hasStart && hasEnd) {
            return time <= (endTime as number);
        }

        return (
            time >= (startTime as number) &&
            time <= (endTime as number)
        );
    };

    const dataFiltrada = useMemo(() => {
        const rows = cargueNominaState.consultaArchivosCargadosN ?? [];
        if (!appliedFilters) return rows;

        return rows.filter((r: any) => {

            if (appliedFilters.idUnidadEjecutora != null) {
                if (Number(r?.idUnidadEjecutora) !== Number(appliedFilters.idUnidadEjecutora)) return false;
            }

            if (appliedFilters.idTipoArchivo != null) {
                if (Number(r?.idTipoArchivo) !== Number(appliedFilters.idTipoArchivo)) return false;
            }

            if (appliedFilters.periodo != null) {
                if (Number(r?.periodo) !== Number(appliedFilters.periodo)) return false;
            }

            if (!inInclusiveRange(r?.fechaCarga, appliedFilters.fechaInicioCarga, appliedFilters.fechaFinalCarga)) {
                return false;
            }

            if (appliedFilters.documentoNombre) {
                const needle = norm(appliedFilters.documentoNombre);
                const hay = norm(r?.documentoNombre);
                if (!hay.includes(needle)) return false;
            }

            if (appliedFilters.estado) {
                if (norm(r?.estado) !== norm(appliedFilters.estado)) return false;
            }

            if (appliedFilters.esdId != null) {
                if (Number(r?.esdId) !== Number(appliedFilters.esdId)) return false;
            }

            return true;
        });
    }, [cargueNominaState.consultaArchivosCargadosN, appliedFilters]);

    return (
        <>
            <LoadingComponent loading={cargueNominaState?.loading} />
            <LoadingComponent loading={loading} />
            <Container maxWidth="lg">

                <Card>
                    <CardContent>
                        {/* <Grid container> */}
                        <Typography variant="h6" align="center" className="titulo1">
                            Aprobar Cargue
                        </Typography>
                        <Typography variant="h6" align="center" className="titulo2">
                            Nómina
                        </Typography>
                        <Grid container >
                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="uej_Id"
                                    control={formularioNomina.control}
                                    // rules={{
                                    //     required: 'Este campo es requerido.',
                                    // }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Unidad Ejecutora"
                                            value={value || ''}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            {catalogState.ExecutingUnit?.map(
                                                (option: EXECUTINGUNITDTO) => (
                                                    <MenuItem
                                                        key={`menu${option.id}`}
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

                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="tdc_Id"
                                    control={formularioNomina.control}
                                    // rules={{
                                    //     required: 'Este campo es requerido.',
                                    // }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo Archivo"
                                            value={value || ''}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            {catalogState.TypesFile?.map(
                                                (option: TYPESFILEDTO) => (
                                                    <MenuItem
                                                        key={`menu${option.id}`}
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
                            {/* 
                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="esd_Id"
                                    control={formularioNomina.control}
                                    // rules={{
                                    //     required: 'Este campo es requerido.',
                                    // }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Estado Archivo"
                                            value={value || ''}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            {catalogState.StateFile?.map(
                                                (option: STATEFILEDTO) => (
                                                    <MenuItem
                                                        key={`menu${option.id}`}
                                                        value={option.id}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        
                                        </CHSelect>
                                    )}
                                />
                            </Grid>  */}

                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="dct_FechaCreacion"
                                    control={formularioNomina.control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHDatePicker
                                            InputLabelProps={{ shrink: true }}
                                            disableFuture
                                            format='yyyy/MM'
                                            label="Periodo"
                                            onChange={onChange}
                                            value={value}
                                            views={['year', 'month']}
                                            maxDate={new Date()}
                                        />
                                    )}
                                />
                            </Grid>


                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="fechaInicioCarga"
                                    control={formularioNomina.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePicker
                                            InputLabelProps={{ shrink: true }}

                                            disableFuture
                                            label="Fecha Inicio Carga"
                                            onChange={onChange}
                                            value={value !== null ? value : null}
                                            maxDate={new Date()}
                                        // maxDateMessage="La fecha de alta no debe ser mayor a la actual"
                                        // invalidDateMessage="Fecha inválida"
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="fechaFinalCarga"
                                    control={formularioNomina.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePicker
                                            InputLabelProps={{ shrink: true }}

                                            disableFuture
                                            label="Fecha Final Carga"
                                            onChange={onChange}
                                            value={value !== null ? value : null}
                                            maxDate={new Date()}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item xs={12} md={4} className={classes.column}>
                                <Controller
                                    name="nombreArchivo"
                                    control={formularioNomina.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField

                                            inputProps={{
                                                style: { textTransform: 'uppercase', },

                                            }}
                                            InputLabelProps={{ shrink: true }}
                                            label="Nombre Archivo"
                                            error={!!error}
                                            value={value !== null ? value : null}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                            isReadOnly={false}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="estado"
                                    control={formularioNomina.control}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Estado"
                                            value={value || ''}
                                            helperText={error ? error.message : null}
                                            onChange={onChange}
                                        >
                                            {ESTADOS.map((e) => (
                                                <MenuItem key={e} value={e}>
                                                    {e}
                                                </MenuItem>
                                            ))}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>

                            <Grid item xs={12}>
                                <Box textAlign="center" mt={2}>
                                    <Grid container justifyContent="center" spacing={2}>
                                        <Grid item>
                                            <Button
                                                className={classes.button}
                                                style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                                                variant="outlined"
                                                color="primary"
                                                onClick={handleClearSelect}
                                            >
                                                Limpiar
                                            </Button>
                                        </Grid>
                                        <Grid item>
                                            <Button
                                                className={classes.button}
                                                variant="outlined"
                                                color="primary"
                                                onClick={formularioNomina.handleSubmit(ConsultaArchivosFiltro)}
                                                style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                                            >
                                                Consultar
                                            </Button>
                                        </Grid>
                                    </Grid>
                                </Box>
                            </Grid>



                        </Grid>
                        <Grid item xs={12}>
                            <div
                                className={classes.smallTable}
                                style={{
                                    display: 'table',
                                    tableLayout: 'fixed',
                                    width: '100%',
                                }}
                            >
                                <MUIDataTable
                                    columns={columns}
                                    data={dataFiltrada}
                                    title=""
                                    options={{
                                        selectableRows: 'none',
                                        filter: false,
                                        print: false,
                                        elevation: 2,
                                        filterType: 'dropdown',
                                        responsive: 'standard',
                                        fixedHeader: true,
                                        fixedSelectColumn: true,
                                        tableBodyHeight: '450px',
                                        tableBodyMaxHeight: '500px',
                                        download: false,
                                        rowsPerPageOptions: [5, 10, 20, 50],
                                        textLabels: {
                                            body: {
                                                noMatch: 'Ningún Resultado',
                                            },
                                            pagination: {
                                                displayRows: 'de',
                                                next: 'Siguiente',
                                                previous: 'Anterior',
                                                rowsPerPage: 'Filas por página',
                                                jumpToPage: '',
                                            },
                                            viewColumns: {
                                                title: 'Mostrar Columnas',
                                            },
                                            toolbar: {
                                                search: 'Buscar',
                                                viewColumns: 'Ver columnas',
                                            },

                                        },
                                        viewColumns: true,
                                        setTableProps: () => ({
                                            style: { tableLayout: 'fixed', width: '100%' },
                                            size: 'small'
                                        }),

                                    }}
                                />
                            </div>

                        </Grid>

                    </CardContent>
                </Card>

                <ModalAprobarNomina
                    open={openModalAgregar}
                    handleClose={handleCloseModalAgregar}
                />
                <ModalRechazarNomina
                    open={openModalRechazar}
                    handleClose={handleCloseModalRechazar}
                    rechazarArchivosCargados={rechazarArchivosCargados}
                    itemRechazo={itemRechazo}
                    formularioRechazarNomina={formularioRechazarNomina}
                />

            </Container>
            {/* </Grid> */}
        </>
    );
};

interface LinkMapStateToProps {
    catalogState: CatalogState;
    cargueNominaState: CargueNominaState;
    loginState: LoginState;
}


const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    cargueNominaState: state.cargueNomina,
    loginState: state.login,
});

interface LinkMapDispatchProps {
    startProcesarArchivosCargados: (documentosPorProcesar: DocumentoCarga[],) => void;
    // startConsultarArchivosCargados: (request: any[],) => void;
    startRechazarCargueNomina: (request: any[],) => void;
    startCrearSolicitudNomina: (request: any[],) => void;
    startObtenerSolicitudNomina: (request: any[],) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetAllGrades: (claims: string) => void;
}

const mapDispatchToProps = {
    startProcesarArchivosCargados,
    // startConsultarArchivosCargados,
    startRechazarCargueNomina,
    startCrearSolicitudNomina,
    startObtenerSolicitudNomina,
    startGetAllIdentificationTypes,
    startGetAllGrades
};

type Props = LinkMapStateToProps & LinkMapDispatchProps;

export default connect(mapStateToProps, mapDispatchToProps)(DataTableNomina);



