import { Grid, makeStyles, createStyles, Button } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import Convocatoria from './Convocatoria';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import PostuladosCalificados from './PostuladosCalificados';
import PostuladosRechazados from './PostuladosRechazados';
import { ModalCierreConvocatoria } from './components/ModalCierreConvocatoria';
import PostuladosSinCupo from './PostuladosSinCupo';
import { ESTADO_CONVOCATORIA, listaEstadosConvocatoria } from '../constants/fondoSolidaridad.constants';
import NotificationService from '../../../services/notification.service';
import { LoadingComponent } from '../../../components/loading/loading.component';
import UtilService from '../../../utils/util.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import CatalogApi from '../../../services/api-catalog.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnRight: {
            padding: '10px',
            textAlign: 'right',
        },
        button: {
            marginTop: '1.5rem',
            marginRight: '.5rem',
        },
    }),
);

const InformacionConvocatoria = (props: propsType) => {

    const classes = useStyles();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const [convocatoriaInicio, setConvocatoriaInicio] = useState(false);
    const [datosPostulantesCalificados, setDatosPostulantesCalificados] = useState<any[]>([]);
    const [totalPostuladosCalificados, setTotalPostuladosCalificados] = useState(0);
    const [totalPostuladosRechazados, setTotalPostuladosRechazados] = useState(0);
    const [totalPostuladosSinCupo, setTotalPostuladosSinCupo] = useState(0);
    const [camposDeshabilitados, setCamposDeshabilitados] = useState(false);

    const [openModal, setOpenModal] = useState(false);
    const [loading, setLoading] = useState(false);
    const { datosConvocatoria, setSelectedData, consultarConvocatorias } = props;

    const formularioConvocatoria = useForm({
        defaultValues: {
            idConvocatoria: '',
            fechaApertura: new Date(),
            fechaCierre: new Date(),
            fechaPublicacion: new Date(),
            fechaResolucion: new Date(),
            numeroBeneficiario: 0,
            numeroResolucion: null,
            numeroResolucionCierre: null,
            numeroConvocatoria: null,
            valorOficial: 0,
            valorSubOficial: 0,
            valorSoldadoProfesionalAgente: 0,
            numeroDiarioOficial: null,
            estadoConvocatoriaId: '',
            montoTotal: null,
            acuerdo: '',
            fechaNotificacion: new Date(),
            totalPostulados: 0,
        }
    })

    useEffect(() => {
        if (datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.CERRADA) {
            consultarInformacionCierreConvocatoria(datosConvocatoria?.idConvocatoria);
        } else {
            formularioConvocatoria.setValue('idConvocatoria', datosConvocatoria.idConvocatoria);
            formularioConvocatoria.setValue('fechaApertura', datosConvocatoria.fechaApertura);
            formularioConvocatoria.setValue('fechaPublicacion', datosConvocatoria.fechaPublicacion);
            formularioConvocatoria.setValue('numeroBeneficiario', datosConvocatoria.numeroBeneficiario);
            formularioConvocatoria.setValue('numeroResolucion', datosConvocatoria.numeroResolucion);
            formularioConvocatoria.setValue('numeroResolucionCierre', datosConvocatoria.numeroResolucionCierre);
            formularioConvocatoria.setValue('numeroConvocatoria', datosConvocatoria.numeroConvocatoria);
            formularioConvocatoria.setValue('numeroDiarioOficial', datosConvocatoria.numeroDiariOficial);
            formularioConvocatoria.setValue('estadoConvocatoriaId', datosConvocatoria.estadoConvocatoriaId);
            formularioConvocatoria.setValue('fechaResolucion', datosConvocatoria.fechaResolucion);
        }
    }, [datosConvocatoria]);

    useEffect(() => {
        formularioConvocatoria.setValue('totalPostulados', totalPostuladosCalificados + totalPostuladosRechazados + totalPostuladosSinCupo);
    }, [totalPostuladosCalificados, totalPostuladosRechazados, totalPostuladosSinCupo]);

    const consultarInformacionCierreConvocatoria = async (idConvocatoria: string) => {
        setLoading(true);
        await apiFondoSolidaridad
            .getConsultarCierreConvocatoria(idConvocatoria)
            .then(async response => {
                if (response.data) {
                    setLoading(false);
                    setCamposDeshabilitados(true);
                    formularioConvocatoria.setValue('idConvocatoria', datosConvocatoria.idConvocatoria);
                    formularioConvocatoria.setValue('numeroResolucion', datosConvocatoria.numeroResolucion);
                    formularioConvocatoria.setValue('numeroResolucionCierre', response?.data.numeroResolucionCierre);
                    formularioConvocatoria.setValue('fechaApertura', response?.data?.fechaApertura);
                    formularioConvocatoria.setValue('fechaCierre', response?.data?.fechaCierre);
                    formularioConvocatoria.setValue('numeroBeneficiario', response?.data?.numeroBeneficiarios);
                    formularioConvocatoria.setValue('numeroConvocatoria', response?.data?.numeroConvocatoria);
                    formularioConvocatoria.setValue('valorOficial', response?.data?.valorOficial);
                    formularioConvocatoria.setValue('valorSubOficial', response?.data?.valorSubOficial);
                    formularioConvocatoria.setValue('valorSoldadoProfesionalAgente', response?.data?.valorSoldado);
                    formularioConvocatoria.setValue('acuerdo', response?.data?.acuerdo);
                    formularioConvocatoria.setValue('fechaPublicacion', response?.data?.fechaPublicacionDiario);
                    formularioConvocatoria.setValue('fechaNotificacion', response?.data?.fechaNotificacion);
                }
            })
            .catch(() => {
                setLoading(false);
                toast('Error al consultar cierre convocatoria', { type: toast.TYPE.ERROR });
            });
    }

    const regresarInicio = () => setConvocatoriaInicio(true);

    const handleOpenModal = () => setOpenModal(true);

    const handleCloseModal = () => setOpenModal(false);

    const handleOnClickCierreConvocatoria = (data: any) => {
        swal({
            text: "¿Está seguro que desea continuar?",
            icon: "warning",
            buttons: ["Sí", "No"]
        }).then(respuesta => {
            if (!respuesta) {
                const request = {
                    idConvocatoria: datosConvocatoria?.idConvocatoria,
                    numeroResolucionCierre: data.numeroResolucionCierre,
                    fechaApertura: data.fechaApertura,
                    fechaCierre: data.fechaCierre,
                    numeroBeneficiarios: Number(data.numeroBeneficiario),
                    numeroConvocatoria: data.numeroConvocatoria,
                    valorOficial: UtilService.cleanCurrencyValue(data.valorOficial),
                    valorSubOficial: UtilService.cleanCurrencyValue(data.valorSubOficial),
                    valorSoldado: UtilService.cleanCurrencyValue(data.valorSoldadoProfesionalAgente),
                    acuerdo: data.acuerdo,
                    fechaPublicacionDiario: data.fechaPublicacion,
                    fechaNotificacion: data.fechaNotificacion
                }
                cierreConvocatoria(request);
            }
        })
    };

    const deshabilitarGenerarReporte = () => {
        let deshabilitar = false;
        if (datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.VERIFICACION ||
            datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.CERRADA) {
            deshabilitar = false;
        }
        else if (datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.ANULADO ||
            datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.APERTURA) {
            deshabilitar = true;
        }
        return deshabilitar;
    }

    const cierreConvocatoria = async (request: any) => {
        setLoading(true);
        const numeroBeneficiarios = formularioConvocatoria.getValues('numeroBeneficiario');

        if (datosPostulantesCalificados.length >= numeroBeneficiarios) {
            await apiFondoSolidaridad
                .getConsultarCierreConvocatoria(datosConvocatoria?.idConvocatoria)
                .then(async response => {
                    if (response.data) {
                        const informacionCierre =
                        {
                            fechaPublicacionDiario: formularioConvocatoria.getValues('fechaPublicacion'),
                            fechaNotificacion: formularioConvocatoria.getValues('fechaNotificacion'),
                            idConvocatoria: datosConvocatoria?.idConvocatoria
                        }
                        await apiFondoSolidaridad
                            .postActualizarCierreConvocatoria(informacionCierre)
                            .then(async response => {
                                if (response.data) {
                                    setLoading(false);
                                    await NotificationService.genericAlert(
                                        'Actualizado con éxito',
                                        'Se realizó con éxito la actualización.',
                                    );
                                }
                            })
                            .catch(() => {
                                setLoading(false);
                                toast('Error del sistema', { type: toast.TYPE.ERROR });
                            });
                    } else {
                        await apiFondoSolidaridad
                            .postCierreConvocatoria(request)
                            .then(async response => {
                                if (response.data) {
                                    setLoading(false);
                                    await NotificationService.genericAlert(
                                        'Exitoso!',
                                        'Se realizó la petición con éxito. Convocatoria Cerrada',
                                    );
                                    handleCloseModal();
                                    setConvocatoriaInicio(true);
                                    setSelectedData(null);
                                } else if (!response.data &&
                                    response.isSuccess === false &&
                                    response.returnMessage) {
                                    setLoading(false);
                                    await NotificationService.genericAlert(
                                        '',
                                        response.returnMessage,
                                        'warning',
                                    );
                                }
                            })
                            .catch(() => {
                                setLoading(false);
                                toast('Error del sistema', { type: toast.TYPE.ERROR });
                            });
                        const busqueda = {
                            numeroConvocatoria: '',
                            estado: '',
                            fechaDesde: '',
                            fechaHasta: '',
                            fechaPublicacion: '',
                        }
                        consultarConvocatorias(busqueda);
                        setConvocatoriaInicio(true);
                    }
                })
                .catch(() => {
                    setLoading(false);
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        } else {
            setLoading(false);
            await NotificationService.genericAlert(
                '',
                'La cantidad de calificados aprobados es inferior en comparación con el número de beneficiarios',
                'warning',
            );
        }

    }

    const solicitudTransferencia = () => {
        swal({
            title: "Solicitud Transferencia de Recursos",
            text: "¿Está seguro que desea continuar?",
            icon: "warning",
            buttons: ["Sí", "No"]
        }).then(respuesta => {
            if (!respuesta) {
                const res = false

            }
        })
    }

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

    // Generar Reporte Ranking
    // const generarListado = async () => {
    //     setLoading(true);
    //     await apiFondoSolidaridad.postGenerarExcelRanking(datosConvocatoria?.idConvocatoria)
    //         .then(response => {
    //             if (response) {
    //                 const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    //                 const url = window.URL.createObjectURL(blob);
    //                 const link = document.createElement('a');
    //                 link.href = url;
    //                 link.download = `Ranking-Fecha:${moment().format()}.csv`;
    //                 document.body.appendChild(link);
    //                 link.click();
    //                 window.URL.revokeObjectURL(url);
    //                 setLoading(false);

    //                 const auditoriaApi = AuditoriaApi();
    //                 const user = AuthService.getUserInfo();
    //                 const nav = window.navigator as any;
    //                 const usrAg = nav.userAgentData.brands.find(
    //                     (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    //                 )?.brand;

    //                 try {
    //                     const requestAuditoria = {
    //                         navegador: usrAg,
    //                         sistemaOperativo: nav.userAgentData.platform,
    //                         nombreUsuario: user.USR_USERNAME,
    //                         usurioId: user.USR_ID,
    //                         ip: publicIpAddress,
    //                         modulo: 'Fondo de Solidaridad / Convocatorias',
    //                         hostname: document.location.host,
    //                         tipoAccion: 'DESCARGAS',
    //                         idTipoAccion: 5,
    //                         descripcion: 'Descarga de Reporte General Ranking'

    //                     } as Auditoria;

    //                     auditoriaApi.InsertAuditoria(requestAuditoria);
    //                 } catch (error: any) {
    //                     const descripcionError = `Error en descarga de archivo}.
    //                     - Error: ${error?.message || 'Error desconocido'}
    //                     - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
    //                     - Código HTTP: ${error?.response?.status || 'No disponible'}`;

    //                     const requestAuditoriaError: Auditoria = {
    //                         navegador: usrAg,
    //                         sistemaOperativo: nav.userAgentData?.platform,
    //                         nombreUsuario: user?.USR_USERNAME,
    //                         usurioId: user?.USR_ID,
    //                         ip: publicIpAddress,
    //                         modulo: 'Fondo Solidaridad / Convocatorias',
    //                         hostname: document.location.host,
    //                         tipoAccion: 'ERRORES Y EXCEPCIONES',
    //                         idTipoAccion: 8,
    //                         descripcion: descripcionError
    //                     };

    //                     auditoriaApi.InsertAuditoria(requestAuditoriaError);

    //                     toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
    //                 }



    //             }
    //         })
    //         .catch(() => {
    //             setLoading(false);
    //             toast('Error al generar reporte', { type: toast.TYPE.ERROR });
    //         })

    // }

    const generarListado = async () => {
        setLoading(true);
        try {
            const response = await apiFondoSolidaridad.postGenerarExcelRanking(
                datosConvocatoria?.idConvocatoria,
                { responseType: 'blob' }
            );

            if (response?.data) {
                const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = `Ranking-Fecha:${moment().format('YYYY-MM-DD_HH-mm')}.xlsx`;
                document.body.appendChild(link);
                link.click();
                window.URL.revokeObjectURL(url);

                // Auditoría...
                const auditoriaApi = AuditoriaApi();
                const user = AuthService.getUserInfo();
                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find((x: { brand: string }) =>
                    x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;

                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Fondo de Solidaridad / Convocatorias',
                    hostname: document.location.host,
                    tipoAccion: 'DESCARGAS',
                    idTipoAccion: 5,
                    descripcion: 'Descarga de Reporte General Ranking'
                };

                auditoriaApi.InsertAuditoria(requestAuditoria);
            } else {
                toast('No se recibió archivo para descarga.', { type: toast.TYPE.ERROR });
            }
        } catch (error: any) {
            toast('Error al generar reporte', { type: toast.TYPE.ERROR });

            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData?.brands?.find((x: { brand: string }) =>
                x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;

            const descripcionError = `Error en descarga de archivo.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

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

            AuditoriaApi().InsertAuditoria(requestAuditoriaError);
        } finally {
            setLoading(false);
        }
    };


    const actualizarConvocatoria = async () => {

        if (String(formularioConvocatoria.watch('numeroBeneficiario')) === '') {
            toast('El número beneficiarios deben registrarse');
            return;
        }
        const idEstadoConvocatoria = listaEstadosConvocatoria.find(item => item.nombreEstado === datosConvocatoria?.nombreEstado);
        const informacionConvocatoria = {
            idConvocatoria: datosConvocatoria?.idConvocatoria,
            fechaApertura: datosConvocatoria?.fechaApertura,
            fechaCierre: formularioConvocatoria.getValues('fechaCierre'),
            fechaPublicacion: datosConvocatoria?.fechaPublicacion,
            numeroBeneficiario: Number(formularioConvocatoria.getValues('numeroBeneficiario')),
            numeroResolucion: datosConvocatoria?.numeroResolucion,
            numeroConvocatoria: datosConvocatoria?.numeroConvocatoria,
            numeroDiariOficial: datosConvocatoria?.numeroDiariOficial,
            estadoConvocatoriaId: idEstadoConvocatoria?.idEstado,
        }

        await apiFondoSolidaridad
            .postActualizarConvocatoria(informacionConvocatoria)
            .then(async response => {
                if (response.data) {
                    await NotificationService.genericAlert(
                        'Actualizado con éxito',
                        'Se realizó con éxito la actualización.',
                    );
                    setConvocatoriaInicio(true);
                    setSelectedData(null);
                    formularioConvocatoria.reset();
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

    if (convocatoriaInicio) {
        return (
            <Convocatoria />
        )
    }

    if (loading) {
        return <LoadingComponent loading={loading} />
    }

    return (
        <>
            <CHPaper title="Información convocatoria">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaApertura"
                            control={formularioConvocatoria.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    readOnly
                                    label="Fecha de Apertura"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaCierre"
                            control={formularioConvocatoria.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha de Cierre *"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: camposDeshabilitados }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    minDate={new Date()}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroConvocatoria"
                            control={formularioConvocatoria.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de Convocatoria "
                                    value={value}
                                    onChange={onChange}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroBeneficiario"
                            control={formularioConvocatoria.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
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
                                    label="Número de Beneficiarios *"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <=
                                            4
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[\WA-Za-z]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroResolucion"
                            control={formularioConvocatoria.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de Resolución de Apertura *"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <=
                                            4
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[\WA-Za-z]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaResolucion"
                            control={formularioConvocatoria.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    readOnly
                                    label="Fecha de Resolución de Apertura"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="totalPostulados"
                            control={formularioConvocatoria.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    label="Total Postulados"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <=
                                            4
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[\WA-Za-z]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="montoTotal"
                            control={formularioConvocatoria.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Monto Total"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <PostuladosCalificados
                datosConvocatoria={datosConvocatoria}
                datosPostulantesCalificados={datosPostulantesCalificados}
                setDatosPostulantesCalificados={setDatosPostulantesCalificados}
                setTotalPostuladosCalificados={setTotalPostuladosCalificados}
            />
            <PostuladosRechazados
                datosConvocatoria={datosConvocatoria}
                setTotalPostuladosRechazados={setTotalPostuladosRechazados}
            />
            <PostuladosSinCupo
                datosConvocatoria={datosConvocatoria}
                setTotalPostuladosSinCupo={setTotalPostuladosSinCupo}
            />
            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    color="primary"
                    disabled={datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.CERRADA
                        || datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.ANULADO}
                    onClick={actualizarConvocatoria}
                >
                    ACTUALIZAR
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    color="primary"
                    disabled={datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.APERTURA
                        || datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.VERIFICACION
                        || datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.ANULADO}
                    onClick={solicitudTransferencia}
                >
                    SOLICITUD TRANSFERENCIA DE RECURSOS
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={handleOpenModal}
                    disabled={datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.APERTURA
                        || datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.ANULADO}
                >
                    CERRAR CONVOCATORIA
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    disabled={deshabilitarGenerarReporte()}
                    onClick={generarListado}
                >
                    GENERAR LISTADO
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={regresarInicio}
                >
                    REGRESAR
                </Button>
            </div>
            <ModalCierreConvocatoria
                open={openModal}
                handleClose={handleCloseModal}
                formulario={formularioConvocatoria}
                datosConvocatoria={datosConvocatoria}
                cierreConvocatoria={handleOnClickCierreConvocatoria}
                camposDeshabilitados={camposDeshabilitados}
            />
        </>
    )
}

type propsType = {
    datosConvocatoria: any;
    setSelectedData: any;
    consultarConvocatorias: (busqueda: any) => void;
};

export default InformacionConvocatoria;