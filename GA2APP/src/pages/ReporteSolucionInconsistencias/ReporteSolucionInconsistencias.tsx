import React, { useEffect, useState } from 'react';
import { Button, Grid, IconButton, InputAdornment, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Typography } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import moment from 'moment';
import { toast } from 'react-toastify';
import { Close, CloudDownload, Search } from '@material-ui/icons';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHSelect from '../../components/ch-select/CHSelect';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHDatePickerV6 from '../../components/ch-datepicker/CHDatepickerV6';
import CHDatePickerV7 from '../../components/ch-datepicker/CHDatepickerV7';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { UnidadEjecutoraCronogramaDTO, UnidadEjecutoraCronogramaEditarDTO } from '../../types/cronograma.type';
import { ScheduleState } from '../../reducers/schedule.reducer';
import { CatalogState } from '../../reducers/catalog.reducer';
import CatalogApi from '../../services/api-catalog.service';
import ReportApi from '../../services/api-reportes.service';
import CHPaper from '../../components/ch-paper/CHPaper';
import { LoadingComponent } from '../../components/loading/loading.component';
import UtilService from '../../utils/util.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise';
import AuthService from '../../services/auth.service';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            display: 'flex'
        },
        root: {
            flexGrow: 1,
        },
        color: {
            color: '#061311'
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
            marginTop: '20px'
        },
        button: {

            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '200px',
            marginLeft: '0',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0px',
        },
        styleTable: {
            align: 'center',
            backgroundColor: '#FEC917'
        }

    }),
);

function ReporteSolucionInconsistencias({
    data,
    StartEditReportDateExecutingUnitsSchedule,
    scheduleState,
    startDeleteExecutingUnitsSchedule,
    startGetExecutingUnitsSchedule,
    catalogState,
    applyFilter,
}: {
    data: UnidadEjecutoraCronogramaDTO[] | undefined | null;
    StartEditReportDateExecutingUnitsSchedule: (unidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditarDTO) => void;
    scheduleState: ScheduleState;
    startDeleteExecutingUnitsSchedule: (unidadEjecutoraCronogramaDelete: UnidadEjecutoraCronogramaDTO | undefined) => void;
    startGetExecutingUnitsSchedule: () => void;
    applyFilter: (data: any) => void;
    catalogState: CatalogState;

}) {
    const classes = useStyles();
    const apiCatalogo = CatalogApi();
    const apiReportes = ReportApi();
    const user = AuthService.getUserInfo();

    const formularioSolucionInconsistencias = useForm({
        defaultValues: {
            unidadEjecutoraEnvio: '',
            unidadEjecutoraRecibe: '',
            tipoInconsistencia: '',
            concepto: '',
            periodoGeneracion: new Date(),
            periodoEnvio: new Date(),
            nomina: '',
            monto: 0,
            descripcion: '',
            identificacion: '',
            periodo: new Date(),
            solucionada: ''
        },
    });

    interface DataItem {

        concepto: string;
        cedula: number;
        nombre: string;
        unidadEjecutoraOrigen: string;
        unidadEjecutoraDestino: string;
        mesDescuento: Date;
        tipoInconsistencia: string;
        nomina: string;
        monto: number;
        solucionada: string;
        fechaCargue: Date;
        cedulaCargueInconsistencia: number;
        usuarioEjecutoSolucion: string;
        fechaSolucion: Date;

    }

    const dataPrueba: DataItem[] = [
        {
            concepto: 'AHORROS OBLIGATORIOS',
            cedula: 10001125,
            nombre: 'LUIS ALBERTO TREJOS ARBOLEDA',
            unidadEjecutoraOrigen: 'PENSIONADOS FFMM',
            unidadEjecutoraDestino: 'PENSIONADOS FFMM',
            mesDescuento: new Date("2024-08"),
            tipoInconsistencia: 'CUENTA CANCELADA',
            nomina: 'C:rutaDelArchivo.txt',
            monto: 263572,
            solucionada: 'NO',
            fechaCargue: new Date("2024-08"),
            cedulaCargueInconsistencia: 10001125,
            usuarioEjecutoSolucion: 'YENNY RINCON',
            fechaSolucion: new Date("2024-08")
        },
        {
            concepto: 'CRÉDITO',
            cedula: 225588,
            nombre: 'JUAN ALBERTO',
            unidadEjecutoraOrigen: 'ARMADA NACIONAL',
            unidadEjecutoraDestino: 'ARMADA NACIONAL',
            mesDescuento: new Date("2024-07"),
            tipoInconsistencia: 'CUENTA INCONSISTENTE',
            nomina: 'C:Archivo.txt',
            monto: 777859,
            solucionada: 'SI',
            fechaCargue: new Date("2024-07"),
            cedulaCargueInconsistencia: 225588,
            usuarioEjecutoSolucion: 'EDWIN',
            fechaSolucion: new Date("2024-07")
        },
        {
            concepto: 'CRÉDITO',
            cedula: 225588,
            nombre: 'JUAN DAVID',
            unidadEjecutoraOrigen: 'ARMADA NACIONAL',
            unidadEjecutoraDestino: 'ARMADA NACIONAL',
            mesDescuento: new Date("2024-07"),
            tipoInconsistencia: 'CUENTA INCONSISTENTE',
            nomina: 'C:Archivo.txt',
            monto: 777859,
            solucionada: 'SI',
            fechaCargue: new Date("2024-07"),
            cedulaCargueInconsistencia: 225588,
            usuarioEjecutoSolucion: 'EDWIN',
            fechaSolucion: new Date("2024-07")
        },
        {
            concepto: 'AHORRO VOLUNTARIO',
            cedula: 225588,
            nombre: 'DAVID SALAZAR',
            unidadEjecutoraOrigen: 'POLICÍA NACIONAL',
            unidadEjecutoraDestino: 'POLICÍA NACIONAL',
            mesDescuento: new Date("2024-06"),
            tipoInconsistencia: 'CUENTA RECHAZADA',
            nomina: 'C:Archivo.txt',
            monto: 777859,
            solucionada: 'SI',
            fechaCargue: new Date("2024-06"),
            cedulaCargueInconsistencia: 225588,
            usuarioEjecutoSolucion: 'EDWIN',
            fechaSolucion: new Date("2024-06")
        },
    ];

    const [listadoEjecutora, setListadoEjecutora] = useState<any[]>([]);
    const [selectedConcepto, setSelectedConcepto] = useState(null);
    const [products, setProducts] = useState<any[]>([]);
    const [open, setOpen] = useState(false);
    const [datos, setDatos] = useState<DataItem[]>([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    const [searchTerm, setSearchTerm] = useState('');
    const [searchOpen, setSearchOpen] = useState(false);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        obtenerListadoUnidadEjecutora();
    }, [])

    const obtenerListadoUnidadEjecutora = async () => {
        await apiCatalogo
            .getAllEXECUTINGUNITSRequest()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setListadoEjecutora(response.data)
                    }
                }
            })
            .catch(() => {
                toast.error('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerConceptos = async () => {
        await apiCatalogo.ObtenerConceptosNovedades().then(async response => {
            if (response) {
                if (response.isSuccess && response.data) {
                    setProducts(response.data);
                }
            }
        });
    }

    useEffect(() => {
        obtenerConceptos()
    }, []);

    const handleConceptoChange = (event: { target: { value: any; }; }) => {
        const selectedValue = event.target.value;
        setSelectedConcepto(selectedValue);
    };

    const inconsistencia = [
        { nombre: "Fuerza Diferente", id: 1 },
        { nombre: "Error en el código", id: 2 },
        { nombre: "Cuenta no ligada", id: 3 },
    ];

    const solucionada = [
        { nombre: "item 1", id: 1 },
        { nombre: "item 2", id: 2 },
        { nombre: "item 3", id: 3 },
    ];

    const handleChangePage = (event: any, newPage: any) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: any) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleConsultar = async () => {
        setDatos(dataPrueba);
        formularioSolucionInconsistencias.reset();
        setOpen(true);
    };

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);

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
        await apiCatalogo
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

    const handleDownloadExcel = async () => {
        setLoading(true);
        const informacionReporte = {
            columnName: [
                "Concepto",
                "Cédula",
                "Nombre",
                "Unidad Ejecutora Origen",
                "Unidad Ejecutora Destino",
                "Mes Descuento",
                "Tipo Inconsistencia",
                "Nómina",
                "Monto",
                "Solucionada",
                "Fecha Cargue",
                "Cédula Cargue Inconsistencia",
                "Usuario Ejecutó Solución",
                "Fecha Solución"
            ],
            columnDataList: datos?.map((item: any) => ({
                data: [
                    item.concepto,
                    String(item.cedula),
                    item.nombre,
                    item.unidadEjecutoraOrigen,
                    item.unidadEjecutoraDestino,
                    moment(item.mesDescuento).format("YYYY/MM"),
                    item.tipoInconsistencia,
                    item.nomina,
                    UtilService.formatCurrency(item.monto),
                    item.solucionada,
                    moment(item.fechaCargue).format("YYYY/MM"),
                    String(item.cedulaCargueInconsistencia),
                    item.usuarioEjecutoSolucion,
                    moment(item.fechaSolucion).format("YYYY/MM")
                ]
            })),
        };
    
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;
    
        const auditoriaApi = AuditoriaApi();
    
        try {
            const responseReporte = await apiReportes.GenerarExcels(informacionReporte);
            if (responseReporte) {
                const blob = new Blob([responseReporte], {
                    type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                });
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = 'archivo.xlsx';
                document.body.appendChild(link);
                link.click();
                window.URL.revokeObjectURL(url);
    

                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Solución Inconsistencias',
                    hostname: document.location.host,
                    tipoAccion: 'DESCARGAS',
                    idTipoAccion: 5,
                    descripcion: `Descarga de archivo Excel - Total registros: ${datos?.length}`
                } as Auditoria;
    
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        } catch (error: any) {

            const descripcionError = `Error en descarga Excel.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;
    
            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Solución Inconsistencias',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };
    
            auditoriaApi.InsertAuditoria(requestAuditoriaError);
    
            toast(`Error al generar reporte excel Solución Inconsistencias: ${error?.message}`, {
                type: toast.TYPE.ERROR
            });
        } finally {
            setLoading(false);
        }
    };    

    const filteredData = datos.filter(row =>
        Object.values(row).some(val =>
            String(val).toLowerCase().includes(searchTerm.toLowerCase())
        )
    );

    const handleSearchToggle = () => {
        setSearchOpen(!searchOpen);
    };

    const handleSearchChange = (event: any) => {
        setSearchTerm(event.target.value);
    };

    const handleClearSearch = () => {
        setSearchTerm('');
    };

    const handleClear = async () => {
        formularioSolucionInconsistencias.reset();
    }

    return (
        <>
            <div className={classes.root}>
                <IconTitle
                    title='Reporte de Solución'
                    subTitle='de Inconsistencias'
                    icon='fch-Administracion-Grupos-Usuarios-0-1'
                />
            </div>
            <LoadingComponent loading={loading}/>
            <CHPaper title="Reporte de Solución de Inconsistencias">
                <Grid container spacing={2} style={{ padding: '20px', background: 'white' }}>
                    <Grid className="item-containerModal" item xs={6} style={{ marginBottom: '20px', background: 'white' }}>
                        <Controller
                            name="unidadEjecutoraEnvio"
                            control={formularioSolucionInconsistencias.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Unidad Ejecutora Envio"
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    value={value}
                                >
                                    <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE...</MenuItem>
                                    {listadoEjecutora?.sort((a, b) => (a.nombre > b.nombre ? 1 : -1)).map(
                                        (option: any) => (
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
                    <Grid className="item-containerModal" item xs={6} style={{ marginBottom: '20px', background: 'white' }}>
                        <Controller
                            name="unidadEjecutoraRecibe"
                            control={formularioSolucionInconsistencias.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Unidad Ejecutora Recibe"
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    value={value}
                                >
                                    <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE...</MenuItem>
                                    {listadoEjecutora?.sort((a, b) => (a.nombre > b.nombre ? 1 : -1)).map(
                                        (option: any) => (
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
                    <Grid className="item-containerModal" item xs={6} style={{ marginBottom: '20px', background: 'white' }}>
                        <Controller
                            name="tipoInconsistencia"
                            control={formularioSolucionInconsistencias.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo Inconsistencia"
                                    defaultValue=""
                                    onChange={onChange}
                                    value={value}
                                >
                                    <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE...</MenuItem>
                                    {inconsistencia?.map((unidad, index) => (
                                        <MenuItem
                                            className={classes.color}
                                            key={`unidad-${index}`}
                                            value={unidad.id}
                                        >
                                            {unidad.nombre}

                                        </MenuItem>

                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className="item-containerModal" item xs={6} style={{ marginBottom: '20px', background: 'white' }}>
                        <Controller
                            name="concepto"
                            control={formularioSolucionInconsistencias.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Concepto"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={(event) => {
                                        onChange(event);
                                        handleConceptoChange(event);
                                    }}
                                >
                                    <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE...</MenuItem>
                                    {products?.map((product) => (
                                        <MenuItem key={product.idConceptoNovedad} value={product.idConceptoNovedad}>
                                            {product.descripcionConceptoNovedad}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid container spacing={2} alignItems="center">
                        <Grid item xs={12}>
                            <Typography className={classes.titleform} variant="h5" component="h6" gutterBottom>
                                Periodo Generación
                            </Typography>
                        </Grid>
                        <Grid item xs={5} container justifyContent="flex-end" alignItems="center">
                            <Controller
                                name="periodoGeneracion"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        disableFuture
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={2} container justifyContent="center" alignItems="center">
                            <Typography variant="subtitle1" gutterBottom>
                                Y
                            </Typography>
                        </Grid>
                        <Grid item xs={5}>
                            <Controller
                                name="periodoGeneracion"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        disableFuture
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} alignItems="center">
                        <Grid item xs={12}>
                            <Typography className={classes.titleform} variant="h5" component="h6" gutterBottom>
                                Periodo Envío
                            </Typography>
                        </Grid>
                        <Grid item xs={5} container justifyContent="flex-end" alignItems="center">
                            <Controller
                                name="periodoEnvio"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        disableFuture
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={2} container justifyContent="center" alignItems="center">
                            <Typography variant="subtitle1" gutterBottom>
                                Y
                            </Typography>
                        </Grid>
                        <Grid item xs={5}>
                            <Controller
                                name="periodoEnvio"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        disableFuture
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} alignItems="center">
                        <Grid className={classes.column} item xs={6}>
                            <Controller
                                name="nomina"
                                control={formularioSolucionInconsistencias.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        inputProps={{
                                            style: {
                                                padding: '10px',
                                                textAlign: 'right',
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        label="Nómina"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={6}>
                            <Controller
                                name="monto"
                                control={formularioSolucionInconsistencias.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        inputProps={{
                                            style: {
                                                padding: '10px',
                                                textAlign: 'right',
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        currencyMask
                                        label="Monto"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={6}>
                            <Controller
                                name="descripcion"
                                control={formularioSolucionInconsistencias.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        inputProps={{
                                            style: {
                                                padding: '10px',
                                                textAlign: 'right',
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        label="Descripción"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={6}>
                            <Controller
                                name="identificacion"
                                rules={{
                                    pattern: {
                                        value: /^[0-9]+$/g,
                                        message: 'Este campo es requerido y debe contener solo números'
                                    },
                                    required: {
                                        value: true,
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
                                        message: 'Número de identificación inválido'
                                    },
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        inputProps={{
                                            style: {
                                                padding: '10px',
                                                textAlign: 'right',
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        label="Identificación"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} alignItems="center" style={{ marginBottom: '20px' }}>
                        <Grid item xs={12}>
                            <Typography className={classes.titleform} variant="h5" component="h6" gutterBottom>
                                Periodo (Año - Mes)
                            </Typography>
                        </Grid>
                        <Grid item xs={6} container justify="flex-end" alignItems="center">
                            <Controller
                                name="periodo"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePickerV7
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        disableFuture
                                        autoOk
                                        views={["year"]}
                                        openTo="year"
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6} container justify="center" alignItems="center">
                            <Controller
                                name="periodo"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioSolucionInconsistencias.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePickerV6
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        disableFuture
                                        autoOk
                                        views={["month"]}
                                        openTo="month"
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid item xs={12} container justify="flex-start" alignItems="center">
                        <Controller
                            name="solucionada"
                            control={formularioSolucionInconsistencias.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Solucionada"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE</MenuItem>
                                    {solucionada?.map((unidad, index) => (
                                        <MenuItem
                                            className={classes.color}
                                            key={`unidad-${index}`}
                                            value={unidad.id}
                                        >
                                            {unidad.nombre}

                                        </MenuItem>

                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid container justifyContent="flex-end" spacing={2} style={{ marginTop: '20px' }}>
                        <Grid item>
                            <Button className={classes.button} variant="outlined" onClick={handleConsultar}>
                                CONSULTAR
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button className={classes.button} variant="outlined" onClick={handleClear}>
                                LIMPIAR CAMPOS
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </CHPaper>
                {open && (
                    <>
                        <Grid
                            container
                            justifyContent='flex-start'
                            alignItems="center"
                            style={{ padding: '20px', backgroundColor: 'white', borderTopLeftRadius: '14px', borderTopRightRadius: '14px',}}>
                            <Grid item>
                                {searchOpen ? (
                                    <CHTextField
                                        variant="outlined"
                                        placeholder="Buscar..."
                                        value={searchTerm}
                                        onChange={handleSearchChange}
                                        style={{ marginBottom: '20px', width: '500px', marginRight: '1400px' }}
                                        InputProps={{
                                            startAdornment: (
                                                <InputAdornment position="start">
                                                    <Search />
                                                </InputAdornment>
                                            ),
                                            endAdornment: searchTerm ? (
                                                <InputAdornment position="end">
                                                    <IconButton onClick={handleClearSearch}>
                                                        <Close />
                                                    </IconButton>
                                                </InputAdornment>
                                            ) : null,
                                        }}
                                    />
                                ) : (
                                    <IconButton onClick={handleSearchToggle}>
                                        <Search />
                                    </IconButton>
                                )}

                            </Grid>
                            <Grid item>
                                <IconButton
                                    color="default"
                                    onClick={handleDownloadExcel}
                                    title="Descargar Excel"
                                >
                                    <CloudDownload />
                                </IconButton>
                            </Grid>
                        </Grid>
                            <TableContainer>
                                <Table stickyHeader>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell className={classes.styleTable}>CONCEPTO</TableCell>
                                            <TableCell className={classes.styleTable}>CÉDULA</TableCell>
                                            <TableCell className={classes.styleTable}>NOMBRE</TableCell>
                                            <TableCell className={classes.styleTable}>UNIDAD EJECUTORA ORIGEN</TableCell>
                                            <TableCell className={classes.styleTable}>UNIDAD EJECUTORA DESTINO</TableCell>
                                            <TableCell className={classes.styleTable}>MES DESCUENTO</TableCell>
                                            <TableCell className={classes.styleTable}>TIPO INCONSISTENCIA</TableCell>
                                            <TableCell className={classes.styleTable}>NÓMINA</TableCell>
                                            <TableCell className={classes.styleTable}>MONTO</TableCell>
                                            <TableCell className={classes.styleTable}>SOLUCIONADA</TableCell>
                                            <TableCell className={classes.styleTable}>FECHA CARGUE</TableCell>
                                            <TableCell className={classes.styleTable}>CÉDULA CARGUE INCONSISTENCIA</TableCell>
                                            <TableCell className={classes.styleTable}>USUARIO EJECUTÓ SOLUCIÓN</TableCell>
                                            <TableCell className={classes.styleTable}>FECHA SOLUCIÓN</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {filteredData.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row, index) => (
                                            <TableRow key={index}
                                            style={{
                                                backgroundColor: index % 2 === 0 ? 'inherit' : '#cccaca',
                                            }}
                                            >
                                                <TableCell align="center">{row.concepto}</TableCell>
                                                <TableCell align="center">{row.cedula}</TableCell>
                                                <TableCell align="center">{row.nombre}</TableCell>
                                                <TableCell align="center">{row.unidadEjecutoraOrigen}</TableCell>
                                                <TableCell align="center">{row.unidadEjecutoraDestino}</TableCell>
                                                <TableCell align="center">{row.mesDescuento.toLocaleDateString()}</TableCell>
                                                <TableCell align="center" style={{ maxWidth: '200px', wordWrap: 'break-word', fontSize: '12px' }}>
                                                    {row.tipoInconsistencia}
                                                </TableCell>
                                                <TableCell align="center">{row.nomina}</TableCell>
                                                <TableCell align="center">{row.monto}</TableCell>
                                                <TableCell align="center">{row.solucionada}</TableCell>
                                                <TableCell align="center">{row.fechaCargue.toLocaleDateString()}</TableCell>
                                                <TableCell align="center">{row.cedulaCargueInconsistencia}</TableCell>
                                                <TableCell align="center">{row.usuarioEjecutoSolucion}</TableCell>
                                                <TableCell align="center">{row.fechaSolucion.toLocaleDateString()}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                            <TablePagination
                                rowsPerPageOptions={[10, 25, 50]}
                                component="div"
                                count={datos.length}
                                rowsPerPage={rowsPerPage}
                                page={page}
                                onPageChange={handleChangePage}
                                onRowsPerPageChange={handleChangeRowsPerPage}
                                labelRowsPerPage="Filas por página:"
                            />
                    </>
                )}

        </>
    );
}

export default (ReporteSolucionInconsistencias);