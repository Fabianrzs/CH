import { connect } from 'react-redux';
import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { AppState } from '../../store/rootReducer';
import './consultaNovedadesActivas.scss';
import { startGetAllExecutingUnits, startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import history from '../../utils/history.router';
import CHSelect from '../../components/ch-select/CHSelect';
import ApiSchedule from '../../services/api-cronograma.service';
import AddSchedule from '../ScheduleNovelties/components/Addschedule.component';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import NotificationService from '../../services/notification.service';
import ContainerTitle from '../../components/containe-title/container-title';
import { startDeleteExecutingUnitsSchedule, StartEditReportDateExecutingUnitsSchedule, startGetExecutingUnitsSchedule } from '../../actions/schedule.action';
import CatalogApi from '../../services/api-catalog.service';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import CHDatePickerV5 from '../../components/ch-datepicker/CHDatePickerV5';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import AuthService from '../../services/auth.service';
import { User } from '../../types/user.type';
import asigTareas from '../../services/api-asignacionTareas.service';
import { ScheduleState } from '../../reducers/schedule.reducer';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise'


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
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            height: '3rem',
            width: '20rem',
            margin: '1rem 1rem'
        },
        icon: {
            color: '#42b7ef',
            paddingRight: '10px',
        },
        columnNewButton: {
            padding: '10px',
            textAlign: 'left',
        },
        buttonNewPage: {
            width: 'fit-content',
            marginLeft: '0',
            marginTop: '1.5rem',
            backgroundColor: 'transparent',
            color: '#1C3F8B',
            border: 'none',
            cursor: 'pointer',
        },
        textAlign: {
            textAlign: 'center'
        }
    }),
);


function ConsultaNovedadesAc({
    data,
    loginState,
    startGetAllExecutingUnits,
    fechaInicio,
    fechaFin,
    nameUnidadEjecutora,
    periodoNovedad,
    codigoNovedadAlta

}: {
    data: any[];
    loginState: LoginState;
    startGetAllExecutingUnits: (props: string) => void;
    fechaInicio: any;
    fechaFin: any;
    nameUnidadEjecutora: any;
    periodoNovedad: any;
    codigoNovedadAlta: any[];
}) {



    const classes = useStyles();
    const [isOpenAdd, setIsOpenAdd] = useState<boolean>(false);
    const [cronogramaUnidadesEjecutorasEliminar, setCronogramaUnidadesEjecutorasEliminar] = React.useState(null);
    const api = ApiSchedule()
    const apiIdUser = asigTareas()
    const user = AuthService.getUserInfo();
    const [idUser, setIdUser] = useState();
    const [descargando, setDescargando] = useState(false);
    const [recursos, setRecursos] = useState(false);
    const [nombreArchivo, setNombreArchivo] = useState();
    const [descargado, setDescagado] = useState();
    const otroFormato = true;
    let codigoEncontrado;
    const catalogApi = CatalogApi();

    const onCloseAdd = () => {
        setIsOpenAdd(false);
    };
    const getIds = async () => {
        try {
            const request = {
                idUsuario: user.USR_ID,
            };
            const response = await apiIdUser.getObtenerAsignacionTareas(request);

            if (response && response.isSuccess) {

                const ids = response.data.map((asignacion: any) => asignacion.idUsuario)[0];
                setIdUser(ids)
            }
        } catch (error) {
            toast.error(`Error al obtener datos de la API: ${error}`, { type: toast.TYPE.ERROR });

        }
    };


    useEffect(() => {
        async function confirmAlertDelete(cronogramaUnidadesEjecutorasEliminar: any) {
            if (cronogramaUnidadesEjecutorasEliminar != null) {
                const confirmAlert = await NotificationService.confirmAlert(
                    '¿Está seguro que desea eliminar esta novedad?',
                );

                try {
                    if (confirmAlert) {
                        startDeleteExecutingUnitsSchedule(cronogramaUnidadesEjecutorasEliminar);
                    }
                } catch (e) {
                    await NotificationService.genericAlert(
                        'Error en la eliminación',
                        'Ocurrio un error al intentar eliminar, intente de nuevo.',
                        'error',
                    );
                }
            }
        }

        confirmAlertDelete(cronogramaUnidadesEjecutorasEliminar);
    }, [cronogramaUnidadesEjecutorasEliminar]);


    useEffect(() => {
        getIds();

    }, []);

    useEffect(() => {
        startGetAllExecutingUnits(loginState.claims)
    }, [])


    const [seleccion, setSeleccion] = useState<{ alta: number | null; baja: number | null }>({
        alta: null,
        baja: null,
    });

    const handleCheckboxChange = (index: number, tipo: 'alta' | 'baja') => {
        setSeleccion((prevSeleccion) => {
            const nuevasSeleccion = { ...prevSeleccion };

            if (tipo === 'alta') {

                if (nuevasSeleccion.baja !== null && nuevasSeleccion.baja !== index) {
                    nuevasSeleccion.baja = null;
                }


                nuevasSeleccion.alta = nuevasSeleccion.alta === index ? null : index;
            } else if (tipo === 'baja') {

                if (nuevasSeleccion.alta !== null && nuevasSeleccion.alta !== index) {
                    nuevasSeleccion.alta = null;
                }


                nuevasSeleccion.baja = nuevasSeleccion.baja === index ? null : index;
            }


            if (tipo === 'alta' || tipo === 'baja') {
                console.log(`Checkbox de ${tipo} en la fila ${index} cambiado`);
            }

            return nuevasSeleccion;
        });
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


    const onClickGenerar = async (row: any, index: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                        otroFormato: otroFormato
                    });

                    const a = document.createElement('a');
                    a.href = `data:image/png;base64,${response.data.archivoB64}`;


                    const serverFileExtension = response.data.nameFile.split('.').pop();

                    a.download = response.data.nameFile.replace(
                        `.${serverFileExtension}`,
                        `.${serverFileExtension}`
                    );

                    a.click();


                    const requestAuditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'DESCARGAS',
                        idTipoAccion: 5,
                        descripcion: `Descarga de archivo para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                    } as Auditoria;

                    auditoriaApi.InsertAuditoria(requestAuditoria);
                } catch (error: any) {
                    const descripcionError = `Error en descarga.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);

                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo.',
                        'error',
                    );
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarExcelCasur = async (index: any, row: any) => {

        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,

                    });
                    setDescargando(false);

                    if (response.data.nameFile) {

                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 12) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo EXCEL para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato xlsx.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );
                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga EXCEL.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        }
        else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarTxtPensionados = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }
                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                    });

                    const a = document.createElement('a');
                    a.href = `data:image/png;base64,${response.data.archivoB64}`;

                    const serverFileExtension = response.data.nameFile.split('.').pop();

                    a.download = response.data.nameFile.replace(
                        `.${serverFileExtension}`,
                        `.${serverFileExtension}`
                    );

                    a.click();

                    const requestAuditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'DESCARGAS',
                        idTipoAccion: 5,
                        descripcion: `Descarga de archivo TXT para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                    } as Auditoria;

                    auditoriaApi.InsertAuditoria(requestAuditoria);

                } catch (error: any) {
                    const descripcionError = `Error en descarga TXT.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo.',
                        'error',
                    );
                }
            }
        } else {

            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarExcelPensionados = async (index: any, row: any) => {

        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                        otroFormato: otroFormato

                    });
                    setDescargando(false);

                    if (response.data.nameFile) {

                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 8) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo EXCEL para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato xlsx.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );
                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga EXCEL.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarExcelCajaPromotora = async (index: any, row: any) => {

        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,


                    });
                    setDescargando(false);

                    if (response.data.nameFile) {

                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 2) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo EXCEL para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato xlsx.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );
                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga EXCEL.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarTxtFac = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                    });

                    const a = document.createElement('a');
                    a.href = `data:image/png;base64,${response.data.archivoB64}`;

                    const serverFileExtension = response.data.nameFile.split('.').pop();

                    a.download = response.data.nameFile.replace(
                        `.${serverFileExtension}`,
                        `.${serverFileExtension}`
                    );

                    a.click();

                    const requestAuditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'DESCARGAS',
                        idTipoAccion: 5,
                        descripcion: `Descarga de archivo TXT para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                    } as Auditoria;

                    auditoriaApi.InsertAuditoria(requestAuditoria);

                } catch (error: any) {
                    const descripcionError = `Error en descarga TXT.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo.',
                        'error',
                    );
                }

            }
        } else {

            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarPdfFac = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }
                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                        otroFormato: otroFormato
                    });

                    setDescargando(false);

                    if (response.data.nameFile) {
                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 11) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo PDF para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato PDF.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );

                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga PDF.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo PDF.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarPdfComGenFFMM = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                    });

                    setDescargando(false);

                    if (response.data.nameFile) {
                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 9) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo PDF para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato PDF.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );

                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga PDF.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo PDF.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarPdfAEJPMP = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }
                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                    });

                    setDescargando(false);

                    if (response.data.nameFile) {
                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 21) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo PDF para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato PDF.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );

                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga PDF.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo PDF.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarPdfSanidadMil = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                    });

                    setDescargando(false);

                    if (response.data.nameFile) {
                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 13) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo PDF para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;
        
                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato PDF.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );

                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga PDF.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;
        
                            const requestAuditoriaError: Auditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'ERRORES Y EXCEPCIONES',
                                idTipoAccion: 8,
                                descripcion: descripcionError
                            };
        
                            auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo PDF.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const onClickGenerarPdfDivri = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }

                const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

                try {
                    setDescargando(true);
                    const esAlta = seleccion.alta !== null;

                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                    });

                    setDescargando(false);

                    if (response.data.nameFile) {
                        const { nameFile, archivoB64 } = response.data;
                        if (row.idUnidadEjecutora === 24) {
                            const a = document.createElement('a');
                            a.href = `data:image/png;base64,${archivoB64}`;
                            a.download = nameFile;
                            a.click();

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'DESCARGAS',
                                idTipoAccion: 5,
                                descripcion: `Descarga de archivo PDF para unidad ejecutora ${data[0]?.nombreUnidadEjecutora}`
                            } as Auditoria;
        
                            auditoriaApi.InsertAuditoria(requestAuditoria);

                        } else {
                            NotificationService.genericAlert(
                                'Error',
                                'El archivo no tiene formato PDF.',
                                'error',
                            );
                        }
                    } else {
                        NotificationService.genericAlert(
                            'Error',
                            'El archivo no tiene formato xlsx.',
                            'error',
                        );

                    }
                } catch (error: any) {
                    const descripcionError = `Error en descarga PDF.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;
        
                            const requestAuditoriaError: Auditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData?.platform,
                                nombreUsuario: user?.USR_USERNAME,
                                usurioId: user?.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Consulta de novedades Activas',
                                hostname: document.location.host,
                                tipoAccion: 'ERRORES Y EXCEPCIONES',
                                idTipoAccion: 8,
                                descripcion: descripcionError
                            };
        
                            auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo generar el archivo PDF.',
                        'error',
                    );
                    setDescargando(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const [idConceptoNovedadHisto, setIdConceptoNovedadHisto] = useState();
    const [nombreConceptoNovedadHisto, setNombreConceptoNovedadHisto] = useState();
    const [idUnidadEjecutoraHisto, setIdUnidadEjecutoraHisto] = useState();

    const onClickGenerarDefinitiva = async (index: any, row: any) => {
        let dHorizontal = false;

        if (row.novedadesBaja === 0 && row.novedadesAlta === 0) {
            NotificationService.genericAlert(
                `${row.nombreConceptoNovedad}`,
                'No tiene novedades activas!',
                'warning',
            );
            return;
        }

        if (seleccion.alta === index || seleccion.baja === index) {
            if (seleccion.alta !== null || seleccion.baja !== null) {
                if (seleccion.alta !== null && seleccion.baja !== null) {
                    dHorizontal = true;
                }
                try {
                    setRecursos(true);
                    const esAlta = seleccion.alta !== null;
                    const response = await api.GenerarArchivo({
                        idUnidadEjecutora: row.idUnidadEjecutora,
                        idConceptoNovedad: row.idConceptoNovedad,
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFin,
                        esAlta: esAlta,
                        codigo: esAlta ? row.codigoAlta : row.codigoBaja,
                        idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                        dHorizontal: dHorizontal,
                        otroFormato: otroFormato
                    });

                    const codigoNovedad = esAlta ? row.codigoAlta : row.codigoBaja;
                    if (codigoNovedadAlta.includes(codigoNovedad)) {
                        codigoEncontrado = codigoNovedad;
                    } else {
                        console.log(`El código ${codigoNovedad} no está en el array de códigos de alta.`);
                    }

                    const nombreConceptoNov = row.idConceptoNovedad === seleccion.alta ? row.nombreConceptoNovedad : row.nombreConceptoNovedad;
                    const idConceptoNovedad = row?.idConceptoNovedad;
                    const idUnidadEjecutora = row?.idUnidadEjecutora;

                    history.push({
                        pathname: '/ConsultaNovedades',
                        state: {
                            idUnidadEjecutora: idUnidadEjecutora,
                            idConceptoNovedad: idConceptoNovedad,
                            idCodigoUsuario: idUser,
                            nameUnidadEjecutora: nameUnidadEjecutora,
                            periodoNovedad: periodoNovedad,
                            nombreConceptoNovedad: nombreConceptoNov,
                            otroFormato: otroFormato,
                            fechaInicio: fechaInicio,
                            fechaFin: fechaFin,
                            codigoNovedad: codigoNovedad,
                            archivoName: nombreArchivo,
                            descargado: descargado
                        }
                    });

                    setRecursos(false);
                    setNombreConceptoNovedadHisto(nombreConceptoNov);
                    setIdUnidadEjecutoraHisto(idUnidadEjecutora);
                    setIdConceptoNovedadHisto(idConceptoNovedad);


                } catch (error) {
                    NotificationService.genericAlert(
                        'Error',
                        'No se pudo acceder a la descarga definitiva.',
                        'error',
                    );
                    setRecursos(false);
                }
            }
        } else {
            NotificationService.genericAlert(
                'Selección requerida',
                `Debe seleccionar una opción de alta o baja del concepto ${row.nombreConceptoNovedad} para generar el archivo.`,
                'warning',
            );
        }
    };

    const descargaTxtGeneral = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora !== 8 && row.idUnidadEjecutora !== 11 && row.idUnidadEjecutora !== 9) {
            return (
                <button
                    onClick={() => onClickGenerar(row, filaIndex)}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    Descargar TXT
                </button>
            );
        }
        return null;
    };

    const descargaExcelCasur = (filaIndex: number, row: any) => {

        if (row.idUnidadEjecutora === 12) {
            return (
                <button
                    onClick={(e) => onClickGenerarExcelCasur(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar EXCEL'}
                </button>
            );
        }
        return null;

    };

    const descargaTxtPensionados = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 8) {
            return (
                <button
                    onClick={(e) => onClickGenerarTxtPensionados(filaIndex, row)}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    Descargar TXT
                </button>
            );
        }
        return null;

    };

    const descargaExcelPensionados = (filaIndex: number, row: any) => {

        if (row.idUnidadEjecutora === 8) {
            return (
                <button
                    onClick={(e) => onClickGenerarExcelPensionados(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar EXCEL'}
                </button>
            );
        }
        return null;

    };

    const descargaExcelCajaPromotora = (filaIndex: number, row: any) => {

        if (row.idUnidadEjecutora === 2) {
            return (
                <button
                    onClick={(e) => onClickGenerarExcelCajaPromotora(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar EXCEL'}
                </button>
            );
        }
        return null;

    };

    const descargaPdfFac = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 11) {
            return (
                <button
                    onClick={(e) => onClickGenerarPdfFac(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar PDF'}
                </button>
            );
        }
        return null;

    };

    const descargaTxtFac = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 11) {
            return (
                <button
                    onClick={(e) => onClickGenerarTxtFac(filaIndex, row)}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    Descargar TXT
                </button>
            );
        }
        return null;

    };

    const descargaPdfComGenFFMM = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 9) {
            return (
                <button
                    onClick={(e) => onClickGenerarPdfComGenFFMM(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar PDF'}
                </button>
            );
        }
        return null;

    };

    const descargaPdfAEJPMP = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 21) {
            return (
                <button
                    onClick={(e) => onClickGenerarPdfAEJPMP(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar PDF'}
                </button>
            );
        }
        return null;

    };

    const descargaPdfSanidadMil = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 13) {
            return (
                <button
                    onClick={(e) => onClickGenerarPdfSanidadMil(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar PDF'}
                </button>
            );
        }
        return null;

    };

    const descargaPdfDivri = (filaIndex: number, row: any) => {
        if (row.idUnidadEjecutora === 24) {
            return (
                <button
                    onClick={(e) => onClickGenerarPdfDivri(filaIndex, row)}
                    disabled={descargando}
                    style={{
                        backgroundColor: 'green',
                        padding: '5px',
                        color: 'primary',
                        border: 'none',
                        marginRight: '10px',
                        cursor: descargando ? 'not-allowed' : 'pointer',
                        borderRadius: '5px',
                    }}
                >
                    {descargando ? 'Descargando...' : 'Descargar PDF'}
                </button>
            );
        }
        return null;

    };


    return (
        <><Grid container>
            <CHPaperNew>
                <Grid>
                    <ContainerTitle
                        title="Consulta de Novedades Activas" />
                </Grid>
                <table className="table">
                    <thead>
                        <tr>
                            <th className={classes.textAlign}>Unidad Ejecutora</th>
                            <th className={classes.textAlign}>Concepto</th>
                            <th className={classes.textAlign}>Código De Alta</th>
                            <th className={classes.textAlign}>Código De Baja</th>
                            <th className={classes.textAlign}>Novedades De Alta</th>
                            <th className={classes.textAlign}>Novedades De Baja</th>
                            <th className='descarga' colSpan={6} style={{ textAlign: 'center' }}>Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data?.filter(x => x.idUnidadEjecutora !== 4).map((row, index) => (
                            <tr key={`table${index}`}>
                                <td className={classes.textAlign}>{row.nombreUnidadEjecutora}</td>
                                <td className={classes.textAlign}>{row.nombreConceptoNovedad}</td>
                                <td className={classes.textAlign}>{row.codigoAlta}</td>
                                <td className={classes.textAlign}>{row.codigoBaja}</td>
                                <td>
                                    <CHCheckbox
                                        color='primary'
                                        label={row.novedadesAlta}
                                        disabled={!row.novedadesAlta}
                                        checked={seleccion.alta === index}
                                        onChange={() => handleCheckboxChange(index, 'alta')}
                                    />
                                </td>
                                <td>
                                    {row.idConceptoNovedad === 3 ? null : (
                                        <CHCheckbox
                                            color='primary'
                                            label={row.novedadesBaja}
                                            disabled={!row.novedadesBaja}
                                            checked={seleccion.baja === index}
                                            onChange={() => handleCheckboxChange(index, 'baja')}
                                        />
                                    )}
                                </td>
                                <td>
                                    {row.idUnidadEjecutora !== 2 && row.idUnidadEjecutora !== 8 && row.idUnidadEjecutora !== 9 && row.idUnidadEjecutora !== 11 && row.idUnidadEjecutora !== 13 && row.idUnidadEjecutora !== 21 && row.idUnidadEjecutora !== 24 && (
                                        <>
                                            {descargaTxtGeneral(index, row)}
                                        </>
                                    )}
                                    {descargaTxtFac(index, row)}
                                    {descargaTxtPensionados(index, row)}
                                </td>
                                <td>
                                    {descargaExcelCasur(index, row)}
                                    {descargaExcelPensionados(index, row)}
                                    {descargaExcelCajaPromotora(index, row)}
                                    {descargaPdfFac(index, row)}
                                    {descargaPdfComGenFFMM(index, row)}
                                    {descargaPdfAEJPMP(index, row)}
                                    {descargaPdfSanidadMil(index, row)}
                                    {descargaPdfDivri(index, row)}
                                </td>
                                <td>
                                    <button
                                        onClick={(e) => onClickGenerarDefinitiva(index, row)}
                                        disabled={recursos}
                                        style={{
                                            backgroundColor: '#1C3F8B',
                                            padding: '5px',
                                            color: 'primary',
                                            border: 'none',
                                            marginRight: '10px',
                                            cursor: recursos ? 'not-allowed' : 'pointer',
                                            borderRadius: '5px',
                                        }}
                                    >
                                        {recursos ? 'Cargando Archivos...' : 'DESCARGA DEFINITIVA'}

                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <AddSchedule
                    isOpen={isOpenAdd}
                    onClose={onCloseAdd} />
            </CHPaperNew>

        </Grid>
        </>
    );
}


const consultaNovedadesActivas = ({
    startGetAllExecutingUnits,
    loginState,
    catalogState,
    startGetExecutingUnitsSchedule,
}: {
    startGetAllExecutingUnits: (data: any) => void;
    loginState: LoginState;
    catalogState: CatalogState;
    startGetExecutingUnitsSchedule: (data: any) => void;
}
) => {

    const apiCatalogo = CatalogApi();
    const api = ApiSchedule();
    const classes = useStyles();
    const today = new Date();

    const formularioUnidadEjecutora = useForm({
        defaultValues: {
            unidadEjecutora: '',
            fechaInicio: '',
            fechaFin: ''
        }
    })

    interface ArchivoId {
        isSuccess: boolean;
        data: { estado: string }[];
    }

    const [listadoEjecutora, setListadoEjecutora] = useState<any[]>([])
    const [datosTablaUnidadEjecutora, setDatosTablaUnidadEjecutora] = useState<any[]>([])
    const [isLoading, setIsLoading] = useState(false);

    const [nombreConceptoNov, setnombreConceptoNov] = useState();
    const [nombreUnidadEjecutora, setNombreUnidadEjecutora] = useState<string | null>(null);
    const [periodoNovedad, setPeriodoNovedad] = useState<any>();
    const [estadoArchivoNov, setEstadoArchivoNov] = useState();
    const [archivoId, setArchivoId] = useState<ArchivoId | any>();
    const [codigoNovedadAlta, setCodigoNovedadAlta] = useState<any>();


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

    // const filtro = async (data: any) => {
    //     setIsLoading(true);
    //     const request = {
    //         idUnidadEjecutora: data.unidadEjecutora,
    //         fechaInicio: data.fechaInicio,
    //         fechaFin: data.fechaFin,
    //         nombreUnidadEjecutora: data.nombreUnidadEjecutora,
    //         codigoAlta: data.codigoAlta
    //     };

    //     const nombreUnidad = listadoEjecutora.find((x: any) => x.id === data.unidadEjecutora)?.nombre || 'No especificada';
    //     const fechaInicio = data.fechaInicio ? new Date(data.fechaInicio).toLocaleDateString('es-CO') : 'No especificada';
    //     const fechaFin = data.fechaFin ? new Date(data.fechaFin).toLocaleDateString('es-CO') : 'No especificada';

    //     try {
    //         const response = await api.ObtenerNovedadesPorUnidadEjecutora(request);

    //         const auditoriaApi = AuditoriaApi();
    //         const user = AuthService.getUserInfo();
    //         const nav = window.navigator as any;
    //         const usrAg = nav.userAgentData.brands.find(
    //             (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    //         )?.brand;

    //         const requestAuditoria = {
    //             navegador: usrAg,
    //             sistemaOperativo: nav.userAgentData.platform,
    //             nombreUsuario: user.USR_USERNAME,
    //             usurioId: user.USR_ID,
    //             ip: publicIpAddress,
    //             modulo: 'Consulta de novedades activas',
    //             hostname: document.location.host,
    //             tipoAccion: 'CONSULTAS',
    //             idTipoAccion: 3,
    //             descripcion: `Consulta de novedades realizada con los siguientes filtros:
    //                 - Unidad ejecutora: ${nombreUnidad}
    //                 - Fecha inicial: ${fechaInicio}
    //                 - Fecha final: ${fechaFin}`

    //         } as Auditoria;

    //         auditoriaApi.InsertAuditoria(requestAuditoria);

    //         setIsLoading(false);
    //         const codigos = response.data.map((codigo: any) => codigo.codigoAlta);
    //         setCodigoNovedadAlta(codigos);

    //         if (response && response.isSuccess) {
    //             const [primerNovedad] = response.data;
    //             const { idUnidadEjecutora } = primerNovedad;

    //             setDatosTablaUnidadEjecutora(response.data)

    //             const periodoUnitResponse = await api.getExecutingUnitsSchedule();

    //             if (periodoUnitResponse && periodoUnitResponse.isSuccess) {

    //                 const unidadEjecutoraId = idUnidadEjecutora;
    //                 const periodoUnit = periodoUnitResponse.data.find((unit: { idUnidadEjecutora: any; }) => unit.idUnidadEjecutora === unidadEjecutoraId);

    //                 if (periodoUnit) {
    //                     const { nombreUnidadEjecutora, periodo } = periodoUnit;
    //                     setNombreUnidadEjecutora(nombreUnidadEjecutora);
    //                     setPeriodoNovedad(periodo);

    //                 } else {
    //                     toast(`No se encontró la unidad ejecutora con ID ${unidadEjecutoraId}`);
    //                 }
    //             } else {
    //                 toast.error('Error al obtener datos de periodo', { type: toast.TYPE.ERROR });
    //             }
    //         }

    //     } catch (error) {
    //         setIsLoading(false);
    //         toast.error('Error del sistema', { type: toast.TYPE.ERROR });
    //     }
    // };

    const filtro = async (data: any) => {
        setIsLoading(true);

        const request = {
            idUnidadEjecutora: data.unidadEjecutora,
            fechaInicio: data.fechaInicio,
            fechaFin: data.fechaFin,
            nombreUnidadEjecutora: data.nombreUnidadEjecutora,
            codigoAlta: data.codigoAlta
        };

        const nombreUnidad = listadoEjecutora.find((x: any) => x.id === data.unidadEjecutora)?.nombre || 'No especificada';
        const fechaInicio = data.fechaInicio ? new Date(data.fechaInicio).toLocaleDateString('es-CO') : 'No especificada';
        const fechaFin = data.fechaFin ? new Date(data.fechaFin).toLocaleDateString('es-CO') : 'No especificada';

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const response = await api.ObtenerNovedadesPorUnidadEjecutora(request);

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de novedades activas',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de novedades realizada con los siguientes filtros:
                    - Unidad ejecutora: ${nombreUnidad}
                    - Fecha inicial: ${fechaInicio}
                    - Fecha final: ${fechaFin}`
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

            setIsLoading(false);

            const codigos = response.data.map((codigo: any) => codigo.codigoAlta);
            setCodigoNovedadAlta(codigos);

            if (response && response.isSuccess) {
                const [primerNovedad] = response.data;
                const { idUnidadEjecutora } = primerNovedad;

                setDatosTablaUnidadEjecutora(response.data)

                const periodoUnitResponse = await api.getExecutingUnitsSchedule();

                if (periodoUnitResponse && periodoUnitResponse.isSuccess) {
                    const periodoUnit = periodoUnitResponse.data.find(
                        (unit: { idUnidadEjecutora: any }) => unit.idUnidadEjecutora === idUnidadEjecutora
                    );

                    if (periodoUnit) {
                        const { nombreUnidadEjecutora, periodo } = periodoUnit;
                        setNombreUnidadEjecutora(nombreUnidadEjecutora);
                        setPeriodoNovedad(periodo);
                    } else {
                        toast(`No se encontró la unidad ejecutora con ID ${idUnidadEjecutora}`);
                    }
                } else {
                    toast.error('Error al obtener datos de periodo', { type: toast.TYPE.ERROR });
                }
            }
        } catch (error: any) {
            setIsLoading(false);

            const descripcionError = `Error en consulta de novedades.
                - Error: ${error?.message || 'Error desconocido'}
                - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de novedades',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast.error('Error del sistema', { type: toast.TYPE.ERROR });
        }
    };


    const validateDate = (value: string) => {
        const date = moment(value);
        let message = '';
        if (date > moment()) {
            message = "La fecha debe ser menor a la fecha actual";
        }

        return message !== '' ? message : true;
    };

    const validateEndDate = (value: string) => {
        const date = moment(value);
        let message = '';
        if (date > moment()) {
            message = "La fecha debe ser menor a la fecha actual";
        } else if (date < moment(formularioUnidadEjecutora.watch('fechaInicio'))) {
            message = "La fecha debe ser menor a la fecha inicial";
        }

        return message !== '' ? message : true;
    };


    if (isLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <>

            <CHPrivateContainer>
                <CHPageTitle
                    topText="Consulta de novedades activas"
                    subText=""
                    title1="Consulta de"
                    title2="Novedades Activas"
                />
                <CHPaper title="Consulta de novedades">
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="unidadEjecutora"
                                control={formularioUnidadEjecutora.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Unidad Ejecutora*"
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                        value={value}
                                    >
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
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaInicio"
                                rules={{
                                    validate: validateDate,
                                    required: 'La fecha inicial es requerida.',
                                }}
                                control={formularioUnidadEjecutora.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePickerV5
                                        error={!!error}
                                        helperText={error ? error.message : ""}
                                        label="Fecha Inicial*"
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
                                name="fechaFin"
                                control={formularioUnidadEjecutora.control}
                                rules={{
                                    validate: validateEndDate,
                                    required: 'La fecha final es requerida.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePickerV5
                                        error={!!error}
                                        helperText={error ? error.message : ""}
                                        label="Fecha Final*"
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
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.accountButton}
                            onClick={formularioUnidadEjecutora.handleSubmit(filtro)}
                        >
                            CONSULTAR

                        </Button>

                    </Grid>
                </CHPaper>
                <ConsultaNovedadesAc
                    data={datosTablaUnidadEjecutora}
                    startGetAllExecutingUnits={startGetAllExecutingUnits}
                    startGetExecutingUnitsSchedule={startGetExecutingUnitsSchedule}
                    loginState={loginState}
                    catalogState={catalogState}
                    fechaInicio={formularioUnidadEjecutora.watch('fechaInicio')}
                    fechaFin={formularioUnidadEjecutora.watch('fechaFin')}
                    nameUnidadEjecutora={nombreUnidadEjecutora}
                    periodoNovedad={periodoNovedad}
                    estadoArchivoNov={estadoArchivoNov}
                    archivoId={archivoId}
                    codigoNovedadAlta={codigoNovedadAlta}

                />
            </CHPrivateContainer>
        </>
    )
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
});

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetClientByIdentificationTypeIdentificationNumber,
    StartEditReportDateExecutingUnitsSchedule,
    startDeleteExecutingUnitsSchedule,
    startGetExecutingUnitsSchedule,
    startGetAllExecutingUnits
};


export default connect(mapStateToProps, mapDispatchToProps)(consultaNovedadesActivas);