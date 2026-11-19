import { Grid, makeStyles, createStyles, Button, MenuItem, IconButton } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { Delete } from '@material-ui/icons';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import NotificationService from '../../../services/notification.service';
import AgregarConvocatoria from './AgregarConvocatoria';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import UtilService from '../../../utils/util.service';
import InformacionConvocatoria from './InformacionConvocatoria';
import { ESTADO_CONVOCATORIA } from '../constants/fondoSolidaridad.constants';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CatalogApi from '../../../services/api-catalog.service';
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

function ConvocatorriasTable({
    data,
    onClickItem,
    eliminarConvocatoria,
}: {
    data: any[] | null;
    onClickItem: (data: any) => void;
    eliminarConvocatoria: (idConvocatoria: string) => void;
}) {
    const classes = useStyles();

    if (data === null) {
        return <div>No se encontraron resultados</div>;
    }

    return (
        <>
            <table className="credit-table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>
                            Procesar
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Id Convocatoria
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Convocatoria
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Resolución
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Diario Oficial
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Fecha de Apertura
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Fecha de Cierre
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Fecha de Publicación
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Beneficiarios
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Estado
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Acción
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {data?.map((row, index) => (
                        <tr key={`table-${index}`}>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => onClickItem(row)}
                                >
                                    {
                                        row.nombreEstado === ESTADO_CONVOCATORIA.APERTURA ||
                                            row.nombreEstado === ESTADO_CONVOCATORIA.VERIFICACION ?
                                            'CONTINUAR' : 'DETALLES'
                                    }
                                </Button>
                            </td>
                            <td>{row.idConvocatoria}</td>
                            <td>{row.numeroConvocatoria}</td>
                            <td>{row.numeroResolucion}</td>
                            <td>{row.numeroDiariOficial}</td>
                            <td>{UtilService.getDate(row.fechaApertura)}</td>
                            <td>{UtilService.getDate(row.fechaCierre)}</td>
                            <td>{UtilService.getDate(row.fechaPublicacion)}</td>
                            <td>{row.numeroBeneficiario}</td>
                            <td>{String(row.nombreEstado).toLocaleUpperCase()}</td>
                            <td>
                                {row.nombreEstado === ESTADO_CONVOCATORIA.APERTURA && (
                                    <IconButton
                                        aria-label="Eliminar"
                                        color="primary"
                                        size="small"
                                        onClick={() => {
                                            eliminarConvocatoria(row.idConvocatoria);
                                        }}
                                    >
                                        <Delete />
                                    </IconButton>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </>
    );
}

const Convocatoria = () => {

    const classes = useStyles();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const [agregarConvocatoria, setAgregarConvocatoria] = useState(false);
    const [convocatorias, setConvocatorias] = useState<any[]>([]);
    const [numerosConvocatorias, setNumerosConvocatorias] = useState<any[]>([]);
    const [estadosConvocatorias, setEstadoConvocatorias] = useState<any[]>([]);
    const [selectedData, setSelectedData] = useState<any>(null);
    const [loading, setLoading] = useState(false);

    const formularioBusquedaConvocatorias = useForm({
        defaultValues: {
            numeroConvocatoria: 0,
            estado: 0,
            fechaDesde: null,
            fechaHasta: null,
            fechaPublicacion: null,
        },
    });

    useEffect(() => {
        consultarNumerosConvocatorias();
        consultarEstadosConvocatoria();
    }, []);

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
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

    const handleSearch = (data: any) => {
        if (data.fechaDesde && data.fechaHasta) {
            if (new Date(data.fechaHasta) < new Date(data.fechaDesde)) {
                NotificationService.genericAlert(
                    '',
                    'La fecha desde no puede ser mayor a la fecha hasta',
                    'warning',
                );
                return;
            }
        }

        const busqueda = {
            numeroConvocatoria: data.numeroConvocatoria === 0 ? '' : data.numeroConvocatoria,
            estado: data.estado === 0 ? '' : data.estado,
            fechaDesde: data.fechaDesde === null ? '' : moment(data.fechaDesde).format('YYYY-MM-DD'),
            fechaHasta: data.fechaHasta === null ? '' : moment(data.fechaHasta).format('YYYY-MM-DD'),
            fechaPublicacion: data.fechaPublicacion === null ? '' : moment(data.fechaPublicacion).format('YYYY-MM-DD'),
        }
        consultarConvocatorias(busqueda);

        let descripcionEtapa = 'No especificado';

        if (data.estado === 1) {
            descripcionEtapa = 'APERTURA';
        } else if (data.estado === 2) {
            descripcionEtapa = 'VERIFICACIÓN';
        } else if (data.estado === 5) {
            descripcionEtapa = 'CERRADA';
        } else if (data.estado === 6) {
            descripcionEtapa = 'ANULADO';
        }

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Fondo Solidaridad / Convocatorias',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta convocatorias fondo solidaridad con los siguientes filtros:
                      - No. Convocatoria: ${busqueda.numeroConvocatoria || 'No especificada'}
                      - Etapa del Proceso: ${descripcionEtapa || 'No especificado'}
                      - Fecha Desde: ${busqueda.fechaDesde || 'No especificada'}
                      - Fecha Hasta: ${busqueda.fechaHasta || 'No especificado'}
                      - Fecha Publicación: ${busqueda.fechaPublicacion || 'No especificado'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error en consulta de información crédito
            - No. Convocatoria: ${busqueda.numeroConvocatoria || 'No especificada'}
            - Etapa del Proceso: ${descripcionEtapa || 'No especificado'}
            - Fecha Desde: ${busqueda.fechaDesde || 'No especificada'}
            - Fecha Hasta: ${busqueda.fechaHasta || 'No especificado'}
            - Fecha Publicación: ${busqueda.fechaPublicacion || 'No especificado'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Fondo Solidaridad / Convocatorias',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
        }

    }

    const onItemIsSelected = (data: any) => {
        setSelectedData(data);
    };

    const consultarConvocatorias = async (busqueda: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .getObtenerConvocatorias(busqueda)
            .then(async response => {
                if (response?.data && response?.data.length > 0) {
                    setConvocatorias(response.data);
                    setLoading(false);
                } else {
                    setConvocatorias([]);
                    setLoading(false);
                    toast("No se encontraron resultados");
                }
            })
            .catch(() => {
                setLoading(false);
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarNumerosConvocatorias = async () => {
        await apiFondoSolidaridad
            .getListaConvocatorias()
            .then(async response => {
                if (response.data) {
                    const array: Set<number> = new Set(response.data);
                    const arraySinDuplicados: number[] = Array.from(array);
                    setNumerosConvocatorias(arraySinDuplicados);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarEstadosConvocatoria = async () => {
        await apiFondoSolidaridad
            .getObtenerEstadosConvocatoria()
            .then(async response => {
                if (response.data) {
                    setEstadoConvocatorias(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const eliminarConvocatoria = async (idConvocatoriaParametro: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            `¿Está seguro que desea eliminar la convocatoria?`,
        );

        if (confirmAlert) {
            const request = {
                idConvocatoria: idConvocatoriaParametro,
                delete: 1
            }
            await apiFondoSolidaridad
                .postEliminarConvocatoria(request)
                .then(async response => {
                    if (response.data) {
                        await NotificationService.genericAlert(
                            'Eliminado con éxito',
                            'Se realizó con éxito la eliminación de la convocatoria.',
                        );
                        const busqueda = {
                            numeroConvocatoria: '',
                            estado: '',
                            fechaDesde: '',
                            fechaHasta: '',
                            fechaPublicacion: '',
                        }
                        consultarConvocatorias(busqueda);
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });

        }
    };

    const agregarNuevaConvocatoria = () =>
        setAgregarConvocatoria(true);

    if (loading) {
        return <LoadingComponent loading={loading} />
    }

    if (agregarConvocatoria) {
        return (
            <AgregarConvocatoria
                setAgregarConvocatoria={setAgregarConvocatoria}
                consultarConvocatorias={consultarConvocatorias}
            />
        )
    }

    if (selectedData) {
        return (
            <InformacionConvocatoria
                datosConvocatoria={selectedData}
                setSelectedData={setSelectedData}
                consultarConvocatorias={consultarConvocatorias}
            />
        )
    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Gestión Convocatorias"
                subText=""
                title1="Convocatorias"
                title2="Fondo Solidaridad"
            />
            <CHPaper title="Información Convocatoria">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroConvocatoria"
                            control={formularioBusquedaConvocatorias.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="No. Convocatoria"
                                >
                                    <MenuItem key={0} value={0} >
                                        SELECCIONE...
                                    </MenuItem>
                                    {numerosConvocatorias?.map(
                                        (option: any) => (
                                            <MenuItem key={option}
                                                value={option}
                                            >
                                                {option}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="estado"
                            control={formularioBusquedaConvocatorias.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Etapa del Proceso"
                                >
                                    <MenuItem key={0} value={0} >
                                        SELECCIONE...
                                    </MenuItem>
                                    {estadosConvocatorias?.map(
                                        (option: any) => (
                                            <MenuItem key={option.idEstado}
                                                value={option.idEstado}
                                            >
                                                {String(option.nombreEstado).toLocaleUpperCase()}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Controller
                            name="fechaDesde"
                            control={formularioBusquedaConvocatorias.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Desde"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: false }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Controller
                            name="fechaHasta"
                            control={formularioBusquedaConvocatorias.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Hasta"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: false }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Controller
                            name="fechaPublicacion"
                            control={formularioBusquedaConvocatorias.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Publicación"
                                    onChange={onChange}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={formularioBusquedaConvocatorias.handleSubmit(
                        handleSearch,
                    )}
                >
                    CONSULTAR
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={agregarNuevaConvocatoria}
                >
                    AGREGAR CONVOCATORIA
                </Button>
            </div>
            {convocatorias.length > 0 ? (
                <ConvocatorriasTable
                    data={convocatorias}
                    onClickItem={onItemIsSelected}
                    eliminarConvocatoria={eliminarConvocatoria}
                />
            ) : null}
        </CHPrivateContainer>
    )
}

export default Convocatoria;