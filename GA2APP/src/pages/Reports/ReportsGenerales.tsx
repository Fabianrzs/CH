import { Button, createStyles, Fab, makeStyles, MenuItem } from '@material-ui/core';
import Grid from '@material-ui/core/Grid';
import { CloudDownload } from '@material-ui/icons';
import convert from 'convert-string-bytes';
import saveAs from 'file-saver';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { dataExcel } from './helpers/HelperExcel';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import ApiReportesGenerales from '../../services/api-reportesGenerales-service';
import NotificationService from '../../services/notification.service';
import TablaListaCartera from './Components/TablaListaCartera';
import TablaPreliminarCartera from './Components/TablaPreliminarCartera';
import { Reportes } from '../../models/reportes-data.model';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import AuditoriaApi from '../../services/api-auditorias.servise';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import ApiReportes from '../../services/api-reportes.service';
import DownloadBlob from '../../utils/blob';
import CatalogApi from '../../services/api-catalog.service';
import GetFormato341Excel from './Components/Formato341Excel';
import GetFormato453y454Excel from './Components/Formato453y454Excel';
import GetFormato477Excel from './Components/Formato477Excel';
import GetFormato416Excel from './Components/Formato416Excel';
import GetFormato414Excel from './Components/Formato414Excel';
import GetFormato536Excel from './Components/Formato536Excel';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'left',
        },
        button: {
            width: '49%',
        },
        button2: {
            width: '49%',
            marginLeft: '.8rem',
        },
    }),
);

function ReportsGenerales(Prop: any) {
    useEffect(() => {
        listarRepostes()
    }, [])
    const ApiGA2ReporteCartera = ApiReportesGenerales();
    const apiReportes = ApiReportes();

    const [listaReportes, setListaReportes] = useState<Reportes[]>([]);

    const listarRepostes = async () => {
        setIsLoadingFile(true);
        await ApiGA2ReporteCartera
            .ConsultarReportesGenerales()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setListaReportes(response.data);
                        setIsLoadingFile(false);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setIsLoadingFile(false);
            });

    };

    interface FormularioValues {
        nombreReporte: string;
        identificacion: string;
        fechaAplicacion: string | null;
        celular: string;
        apellido: string;
        tipoReporte: string;
    }

    const formulario = useForm<FormularioValues>({
        mode: 'onChange',
        defaultValues: {
            nombreReporte: '',
            identificacion: '',
            fechaAplicacion: '',
            celular: '',
            apellido: '',
            tipoReporte: '',
        },
    });

    const [isLoadingFile, setIsLoadingFile] = useState(false);
    const [isDescarga, SetIsDescarga] = useState(false);
    const [dataGeneralCartera, setdataGeneralCartera] = useState(false);
    const [dataPreliminarCartera, setdataPreliminarCartera] = useState(false);
    const [datosExcel, setDatosExcel] = useState<any[]>([]);
    const [bytesExcel, SetbytesExcel] = useState<any[]>([]);
    const [fileName, SetFilename] = useState<any>([]);
    const [conIdentificacion, setConIdentificacion] = useState(false);
    const [CamposTransUnion, setCamposTransUnion] = useState(false);
    const [conFecha, SerFecha] = useState(false);
    const [tipoReporte, setTipoReporte] = useState('');

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

    const GetDataReporteCartera = async () => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera
            .generarListaReportes()
            .then(async (response: any) => {
                if (response) {

                    if (response.isSuccess) {
                        setIsLoadingFile(false);

                        if (response.data !== null) {
                            setdataGeneralCartera(true)
                            setDatosExcel(response.data)
                            SetFilename(response.data[0].fileExcel.fileDownloadName)
                            const data = convert.stringToBytes(response.data[0].fileExcel.fileContents)
                            SetbytesExcel(data)
                            SetIsDescarga(true)
                            requestAuditoria.tipoAccion = 'DESCARGAS';
                            requestAuditoria.idTipoAccion = 5;
                            requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                         \n archivo: ${response.data[0].fileExcel.fileDownloadName}`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        }
                        else {
                            setdataGeneralCartera(false)
                            await NotificationService.genericAlert(
                                'Se realizo la consulta',
                                'No se encontraron datos para generar el excel',
                            );
                            SetIsDescarga(false)
                        }
                    } else {
                        setIsLoadingFile(false);
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                        requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion = 8;
                        requestAuditoria.descripcion = `Descarga de reporte en formato excel:      
                        \n returnApiMessage: ${response.returnMessage}`
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                }
            })
    }


    const GetDataReportePreliminarCartera = async () => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera
            .generarPreliminarCartera()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setIsLoadingFile(false);

                        if (response.data !== null) {
                            setdataPreliminarCartera(true)
                            setDatosExcel(response.data)
                            SetFilename(response.data[0].fileExcel.fileDownloadName)
                            const data = convert.stringToBytes(response.data[0].fileExcel.fileContents)
                            SetbytesExcel(data)
                            SetIsDescarga(true)
                            requestAuditoria.tipoAccion = 'DESCARGAS';
                            requestAuditoria.idTipoAccion = 5;
                            requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                             \n archivo: ${response.data[0].fileExcel.fileDownloadName}`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        }
                        else {
                            setdataPreliminarCartera(false)
                            await NotificationService.genericAlert(
                                'Se realizo la consulta',
                                'No se encontraron datos para generar el excel',
                            );
                            SetIsDescarga(false)
                        }
                    } else {
                        setIsLoadingFile(false);
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                        // setIsLocalLoading(false); 
                        requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion = 8;
                        requestAuditoria.descripcion = `Descarga de reporte en formato excel:      
                            \n returnApiMessage: ${response.returnMessage}`
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                }
            })
    }



    const GetDataReporteMoraCartera = async () => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera
            .generarMoraCartera()
            .then(async response => {
                if (response.isSuccess) {

                    const data = convert.stringToBytes(response.data[0].file);
                    const byteArrays = dataExcel(data)
                    const blob = new Blob(byteArrays, { type: 'application/vnd.ms-excel' })
                    saveAs(new Blob([blob], {}), 'ReporteGeneralMora.xlsx');
                    requestAuditoria.tipoAccion = 'DESCARGAS';
                    requestAuditoria.idTipoAccion = 5;
                    requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                     \n archivo: ReporteGeneralMora.xlsx`;
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
                else {
                    setdataPreliminarCartera(false)
                    await NotificationService.genericAlert(
                        'Informacion',
                        'Error en la consulta porfavor contacte al administrador',
                    );
                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion = 8;
                    requestAuditoria.descripcion = `Descarga de reporte en formato excel:      
                                \n returnApiMessage: ${response.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
                setIsLoadingFile(false)
            })
            .catch((error: any) => {
                toast(
                    'No existe información en la base de datos ',
                    { type: toast.TYPE.ERROR },
                );
                setIsLoadingFile(false);
                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 8;
                requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            });
    }

    const GetDataReporteGarantias = async () => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera
            .generarReporteGarantias()
            .then(async response => {

                if (response.isSuccess) {

                    const data = convert.stringToBytes(response.data[0].file);
                    const byteArrays = dataExcel(data)
                    const blob = new Blob(byteArrays, { type: 'application/vnd.ms-excel' })
                    saveAs(new Blob([blob], {}), 'ReporteGarantias.xlsx');
                    requestAuditoria.tipoAccion = 'DESCARGAS';
                    requestAuditoria.idTipoAccion = 5;
                    requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                     \n archivo: ReporteGarantias.xlsx`;
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
                else {
                    setdataPreliminarCartera(false)
                    await NotificationService.genericAlert(
                        'Se realizo la consulta',
                        'No se encontraron datos para generar el excel',
                    );
                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion = 8;
                    requestAuditoria.descripcion = `Descarga de reporte en formato excel:      
                                \n returnApiMessage: ${response.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
                setIsLoadingFile(false)
            })
            .catch((error: any) => {
                toast(
                    'No existe información en la base de datos ',
                    { type: toast.TYPE.ERROR },
                );
                setIsLoadingFile(false);
                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 5;
                requestAuditoria.descripcion = `Descarga de reporte en formato excel:
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            });
    }

    const GetMovimientoDeCartera = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera
            .MovimientoDeCartera(item)
            .then(async (response: any) => {

                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 7;
                        requestAuditoria.descripcion = `Descarga de reporte en formato pdf:
                            \n archivo: MovimientoCartera${item.identificacion}.pdf`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            })
    }


    const GetFormato341 = async (item: any) => {
        const request = {
            fechaAplicacion: item.fechaAplicacion,
        };
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera.ReportePlanoSFCF341(request)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const currentDate = new Date();
                        const formattedDate = item.fechaAplicacion;

                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `F341_${formattedDate}.txt`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 7;
                        requestAuditoria.descripcion = `Descarga de reporte en formato txt:
                            \n archivo: ${filename}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };

    const informacionDatos341Excel = (item: any) => {
        const datosInfo: any[] = [];
        GetFormato341Excel(item, datosInfo, setIsLoadingFile);
    }

    const GetFormato414 = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera.ReportePlanoSFCF414(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const currentDate = new Date();
                        const formattedDate = item?.fechaAplicacion;

                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `F414_${formattedDate}.txt`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Descarga de reporte en formato txt:
                            \n archivo: ${filename}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: No se encuentran datos para esta fecha`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };

    const informacionDatos414Excel = async (item: any) => {

        const requestFecha = {
            fechaAplicacion: item?.fechaAplicacion
        }
        await ApiGA2ReporteCartera.generarReporteInforme414(requestFecha)
            .then(async (response: any) => {
                if (response.data && response?.data?.length > 0) {
                    GetFormato414Excel(item, response.data, publicIpAddress, setIsLoadingFile);
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            }).catch((error: any) => {
                if (error?.response?.data?.returnMessage) {
                    toast(
                        `${error?.response?.data?.returnMessage}`,
                        { type: toast.TYPE.ERROR },
                    );
                }
                setIsLoadingFile(false);
            });

    }

    const GetFormato416 = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera.ReportePlanoSFCF416(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const formattedDate = moment(item.fechaAplicacion).format("YYYYMMDD");
                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `F416_${formattedDate}.txt`;
                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Descarga de reporte en formato txt:
                                    \n archivo: ${filename}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };

    const informacionDatos416Excel = async (item: any) => {
        setIsLoadingFile(true);
        await ApiGA2ReporteCartera.ReportePlanoSFCF416(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    const mimeType = response.type;
                    if (mimeType) {
                        const texto = await response.text();
                        GetFormato416Excel(item, texto, publicIpAddress, setIsLoadingFile);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: No se encuentran datos para esta fecha`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    }

    const GetFormato477 = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;

        await ApiGA2ReporteCartera.ReportePlanoSFCF477(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);

                    const mimeType = response.type;

                    if (mimeType) {

                        const formattedDate = moment(item.fechaAplicacion).format("YYYYMMDD");
                        const filename = `F477_${formattedDate}.txt`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename);

                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion =
                            `Descarga de reporte en formato txt:\n archivo: ${filename}`;

                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast('Error: no se encontraron datos en el sistema', {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };

    const informacionDatos477Excel = async (item: any) => {

        const requestFecha = {
            fechaAplicacion: item?.fechaAplicacion
        }
        await ApiGA2ReporteCartera.generarReporteInforme477(requestFecha)
            .then(async (response: any) => {
                if (response.data && response?.data?.length > 0) {
                    GetFormato477Excel(item, response.data, publicIpAddress, setIsLoadingFile);
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            }).catch((error: any) => {
                if (error?.response?.data?.returnMessage) {
                    toast(
                        `${error?.response?.data?.returnMessage}`,
                        { type: toast.TYPE.ERROR },
                    );
                }
                setIsLoadingFile(false);
            });

    }

    const GetFormato453 = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera.ReportePlanoSFCF453(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const currentDate = new Date();
                        const formattedDate = item.fechaAplicacion.toISOString().replace(/[:T]/g, '');
                        const notimeDate = item.fechaAplicacion.toISOString().split('T');
                        const newDate = notimeDate[0].replace(/[:T-]/g, '');

                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `${newDate}453.txt`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Descarga de reporte en formato txt:
                                    \n archivo: ${filename}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };

    const informacionDatos453y454 = async (item: any) => {

        const requestFecha = {
            fechaAplicacion: item.fechaAplicacion
        }
        await ApiGA2ReporteCartera.generarReporteInforme453y454(requestFecha)
            .then(async (response: any) => {
                if (response.data && response?.data?.length > 0) {
                    setIsLoadingFile(false);
                    GetFormato453y454Excel(item, response.data, publicIpAddress, setIsLoadingFile);
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            }).catch((error: any) => {
                if (error?.response?.data?.returnMessage === 'No existe información en la base de datos con el filtro de fechas ingresado.') {
                    toast(
                        'No se encontraron datos en el sistema',
                        { type: toast.TYPE.ERROR },
                    );
                }

                setIsLoadingFile(false);
            });

    }

    const GetFormato536 = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera.ReportePlanoSFCF536(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const currentDate = new Date();
                        const formattedDate = item?.fechaAplicacion;

                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `F536_${formattedDate}.txt`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Descarga de reporte en formato txt:
                            \n archivo: ${filename}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: No se encuentran datos para esta fecha`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };


    const informacionDatos536Excel = async (item: any) => {
        setIsLoadingFile(true);
        await ApiGA2ReporteCartera.ReportePlanoSFCF536(item)
            .then(async (response: any) => {
                if (response !== undefined) {

                    const mimeType = response.type;
                    if (mimeType) {
                        const texto = await response.text();
                        GetFormato536Excel(item, texto, publicIpAddress, setIsLoadingFile);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: No se encuentran datos para esta fecha`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });

    }

    const GetFormato343 = async (item: any) => {
        const user = AuthService.getUserInfo();
        const auditoriaApi = AuditoriaApi();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Otros reportes',
            hostname: document.location.host
        } as Auditoria;
        await ApiGA2ReporteCartera.ReportePlanoSFCF343(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const currentDate = new Date();
                        const formattedDate = item.fechaAplicacion.toISOString().replace(/[:T]/g, '');

                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `Reporte_343_${formattedDate}.txt`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Descarga de reporte en formato txt:
                                    \n archivo: ${filename}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };

    const GetRCredito = async (data: any) => {

        setIsLoadingFile(true);
        const request = {
            anio: data.fechaAplicacion?.getFullYear() ?? null,
            mes: data.fechaAplicacion ? data.fechaAplicacion.getMonth() + 1 : null,
        };

        apiReportes
            .reporteRCredito(request)
            .then(async response => {
                if (response?.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    const blob = new Blob([response], { type: response.type });
                    const periodoFormat = moment(data.periodoReporte).format("YYYY-MM");
                    const filename = `Reporte RCredito ${periodoFormat}`;
                    DownloadBlob(blob, filename);
                }
            })
            .catch(async (e) => {
                let message;

                if (e.response) {
                    const result = JSON.parse(await e.response.data.text());
                    message = result.returnMessage;
                } else {
                    message = 'No se pudo generar el reporte';
                }
                toast(message, { type: toast.TYPE.ERROR });

            }).finally(() => setIsLoadingFile(false));

    };




    const GetReporteTransUnion = async (item: any) => {

        await ApiGA2ReporteCartera.ReporteSolicitudTransUnion(item)
            .then(async (response: any) => {
                if (response !== undefined) {
                    setIsLoadingFile(false);
                    const mimeType = response.type;

                    if (mimeType) {
                        // Obtener la fecha y hora actual
                        const currentDate = new Date();
                        const formattedDate = currentDate.toISOString().replace(/[:T]/g, '');

                        // Nombre de archivo deseado (cambia 'miarchivo' a lo que desees)
                        const filename = `Reporte_TransUnion_${formattedDate}.pdf`;

                        const file = new Blob([response], { type: mimeType });
                        saveAs(file, filename); // Aquí se asigna el nombre al archivo
                        setIsLoadingFile(false);
                        const user = AuthService.getUserInfo();
                        const auditoriaApi = AuditoriaApi();
                        const nav = (window.navigator as any);
                        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
                        const requestAuditoria = {
                            navegador: usrAg,
                            sistemaOperativo: nav.userAgentData.platform,
                            nombreUsuario: user.USR_USERNAME,
                            usurioId: user.USR_ID,
                            ip: publicIpAddress,
                            modulo: 'Otros reportes',
                            hostname: document.location.host
                        } as Auditoria;
                        requestAuditoria.tipoAccion = 'DESCARGAS';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Descarga de reporte en formato pdf:
                                 \n archivo: ${filename}`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                } else {
                    setIsLoadingFile(false);
                    toast(`Error: no se encontraron datos en el sistema`, {
                        type: toast.TYPE.ERROR,
                    });
                }
            });
    };


    const onDownload = async (item: any) => {
        const byteArrays = dataExcel(bytesExcel)
        const blob = new Blob(byteArrays, { type: 'application/vnd.ms-excel' })
        saveAs(new Blob([blob], {}), fileName);
    }

    const onSubmit = async (item: any) => {

        if (item.nombreReporte === 'Reporte TransUnion') {
            setIsLoadingFile(true)
            const requestObject = {
                TipoIdentificacion: 1, // Reemplaza con el valor correcto
                Identificacion: item.identificacion, // Reemplaza con el valor correcto
                Telefono: item.celular, // Reemplaza con el valor correcto
                FechaExpedicionIdentificacion: item.fechaAplicacion, // Reemplaza con el valor correcto
                Apellidos: item.apellido, // Reemplaza con el valor correcto
                SalarioMensual: 2000000, // Reemplaza con el valor correcto
                TraerHistorialCrediticio: "Y", // Reemplaza con el valor correcto
                SLS_ID: "123e4567-e89b-12d3-a456-426614174001" // Reemplaza con el valor correcto
            };
            GetReporteTransUnion(requestObject)
        }
        else {
            const fecha = item.fechaAplicacion;
            const dia = fecha.getDate() - 1;
            const mes = fecha.getMonth() + 1;
            const año = fecha.getFullYear();
            const fechaFormateada = new Date(`${año}-${mes}-${dia}`);
            fechaFormateada.setHours(19, 0, 0, 0);
            item.fechaAplicacion = fechaFormateada

            if (item.nombreReporte === 'Reporte general de cartera') {
                setIsLoadingFile(true)
                GetDataReporteCartera()
                setdataPreliminarCartera(false)
            }
            else if (item.nombreReporte === 'Reporte preliminar de cartera') {
                setIsLoadingFile(true)
                setdataGeneralCartera(false)
                GetDataReportePreliminarCartera()
            }
            else if (item.nombreReporte === 'Movimiento de cartera') {
                setIsLoadingFile(true)
                setdataPreliminarCartera(false)
                setdataGeneralCartera(false)
                GetMovimientoDeCartera(item)
            }
            else if (item.nombreReporte === 'Reporte general de mora') {
                setIsLoadingFile(true)
                setdataPreliminarCartera(false)
                setdataGeneralCartera(false)
                GetDataReporteMoraCartera()
            }
            else if (item.nombreReporte === 'Reporte general de garantías') {
                setIsLoadingFile(true)
                GetDataReporteGarantias()
            }
            else if (item.nombreReporte === 'Formato 341' && item.tipoReporte === 'txt') {
                setIsLoadingFile(true)
                GetFormato341(item)
            }
            else if (item.nombreReporte === 'Formato 341' && item.tipoReporte === 'excel') {
                setIsLoadingFile(true)
                informacionDatos341Excel(item);
            }
            else if (item.nombreReporte === 'Formato 414' && item.tipoReporte === 'txt') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true)
                GetFormato414(item)
            }
            else if (item.nombreReporte === 'Formato 414' && item.tipoReporte === 'excel') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true);
                informacionDatos414Excel(item);
            }
            else if (item.nombreReporte === 'Formato 416' && item.tipoReporte === 'txt') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true)
                GetFormato416(item)
            }
            else if (item.nombreReporte === 'Formato 416' && item.tipoReporte === 'excel') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true)
                informacionDatos416Excel(item)
            }
            else if (item.nombreReporte === 'Formato 477' && item.tipoReporte === 'txt') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true);
                GetFormato477(item);
            }
            else if (item.nombreReporte === 'Formato 477' && item.tipoReporte === 'excel') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true);
                informacionDatos477Excel(item);
            }
            else if (item.nombreReporte === 'Formato 453/454' && item.tipoReporte === 'txt') {
                setIsLoadingFile(true)
                GetFormato453(item)
            }
            else if (item.nombreReporte === 'Formato 453/454' && item.tipoReporte === 'excel') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true)
                informacionDatos453y454(item)
            }
            else if (item.nombreReporte === 'Formato 343' && item.tipoReporte === 'txt') {
                setIsLoadingFile(true)
                GetFormato343(item)
            }
            else if (item.nombreReporte === 'RCredito') {
                setIsLoadingFile(true)
                GetRCredito(item)
            }
            else if (item.nombreReporte === 'Formato 536' && item.tipoReporte === 'txt') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true);
                GetFormato536(item);
            }
            else if (item.nombreReporte === 'Formato 536' && item.tipoReporte === 'excel') {
                item.fechaAplicacion = moment(fecha).format('YYYY-MM-DD');
                setIsLoadingFile(true);
                informacionDatos536Excel(item);
            }
        }
    }

    const classes = useStyles();

    if (isLoadingFile) {
        return <CHLoadingProcess />;
    }

    const onlyMondays = (date: any) => date.getDay() !== 5

    const lastDayOfMonth = (date: any) => {
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        const lastDay = new Date(year, month, 0).getDate();
        return date.getDate() !== lastDay;
    };

    const lastDayOfQuarter = (date: any) => {
        const month = date.getMonth() + 1;
        const isLastDayOfQuarter = (month % 3 === 0) && (date.getDate() === new Date(date.getFullYear, month, 0).getDate());
        return !isLastDayOfQuarter;
    }

    const lastDayOfQuarter343 = (date: any) => {
        const month = date.getMonth() + 1;
        const day = date.getDate();
        const year = date.getFullYear();

        let lastDay;
        switch (month) {
            case 3:
                lastDay = 31;
                break;
            case 6:
                lastDay = 30;
                break;
            case 9:
                lastDay = 30;
                break;
            case 12:
                lastDay = 31;
                break;
            default:
                return true; // No es un mes de fin de trimestre.
        }

        return day !== lastDay;
    };

    const anyDay = (date: any) => true;

    return (
        <CHPrivateContainer>
            <CHPaper title="Generación de reportes">
                <Grid container>
                    <Grid className={classes.column} item xs={3} md={3}>
                        <Controller
                            name="nombreReporte"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Documentos"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        const valor = value;
                                        formulario.setValue('fechaAplicacion', null);
                                        formulario.setValue('tipoReporte', '');
                                        if (
                                            valor.target.value === 'Movimiento de cartera'
                                        ) {
                                            setConIdentificacion(true);
                                            SetIsDescarga(false)
                                        } else if (valor.target.value === 'Reporte general de cartera'
                                            || valor.target.value === 'Reporte preliminar de cartera' || valor.target.value === 'Reporte general de mora') {
                                            setConIdentificacion(false);
                                            SerFecha(false)
                                        }
                                        else if (valor.target.value === 'Formato 341' || valor.target.value === 'Formato 414'
                                            || valor.target.value === 'Formato 416' || valor.target.value === 'Formato 477'
                                            || valor.target.value === 'Formato 453/454' || valor.target.value === 'Formato 343'
                                            || valor.target.value === 'Formato 343' || valor.target.value === 'Formato 536' || valor.target.value === 'RCredito') {
                                            SetIsDescarga(false)
                                            setConIdentificacion(false);
                                            SerFecha(true)
                                        }
                                        else if (valor.target.value === 'Reporte TransUnion') {
                                            SerFecha(false)
                                            setConIdentificacion(false);
                                            SetIsDescarga(false)
                                            setCamposTransUnion(true);

                                        }
                                        onChange(value);
                                        const valorTipoReporte = String(valor.target.value);
                                        setTipoReporte(valorTipoReporte);
                                    }}
                                >
                                    <MenuItem key={0} value={0}>
                                        -- Seleccionar --
                                    </MenuItem>
                                    {listaReportes?.map((option: any) => {
                                        if (option.nombreReporte !== 'Formato 417' &&
                                            option.nombreReporte !== 'Formato 341' &&
                                            option.nombreReporte !== 'Formato 343') {

                                            return (
                                                <MenuItem
                                                    key={option.idReporte}
                                                    value={option.nombreReporte}
                                                >
                                                    {option.nombreReporte}
                                                </MenuItem>
                                            )
                                        }
                                        return null;
                                    })}
                                </CHSelect>
                            )}
                        />
                        {conIdentificacion && (
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="identificacion"
                                    control={formulario.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            type="number"
                                            error={!!error}
                                            label="Número Identificación *"
                                            value={value}
                                            InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            </Grid>
                        )}
                        {CamposTransUnion && (
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="identificacion"
                                    control={formulario.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            type="number"
                                            error={!!error}
                                            label="Número Identificación *"
                                            value={value}
                                            InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            </Grid>
                        )}

                    </Grid>
                    {(conFecha && tipoReporte !== "0") && (
                        <Grid item md={3} className={classes.column}>
                            <Controller
                                name="fechaAplicacion"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        label="Fecha*"
                                        value={value}
                                        shouldDisableDate={(e) => {
                                            if (formulario.watch('nombreReporte').includes('414')) {
                                                return onlyMondays(e)
                                            }
                                            if (formulario.watch('nombreReporte').includes('416')) {
                                                return lastDayOfMonth(e)
                                            }
                                            if (formulario.watch('nombreReporte').includes('477')) {
                                                return lastDayOfMonth(e);
                                            }
                                            if (formulario.watch('nombreReporte').includes('341')) {
                                                return lastDayOfMonth(e)
                                            }
                                            if (formulario.watch('nombreReporte').includes('453')) {
                                                return lastDayOfMonth(e)
                                            }
                                            if (formulario.watch('nombreReporte').includes('343')) {
                                                return lastDayOfQuarter343(e)
                                            }
                                            if (formulario.watch('nombreReporte').includes('536')) {
                                                return lastDayOfMonth(e);
                                            }
                                            return false;
                                        }}
                                        invalidDateMessage='Fecha no valida'
                                        onChange={(e) => {
                                            console.log(e)
                                            onChange(e)
                                        }}
                                        InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
                                        disableFuture

                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {(tipoReporte !== "" && tipoReporte !== "0" && tipoReporte !== 'RCredito') && (
                        <Grid className={classes.column} item xs={3} md={3}>
                            <Controller
                                name="tipoReporte"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Tipo Reporte"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                    >
                                        <MenuItem key='txt' value='txt'>
                                            TXT
                                        </MenuItem>
                                        <MenuItem key='excel' value='excel'>
                                            EXCEL
                                        </MenuItem>
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    )}

                </Grid>
                <Grid container>
                    <Grid className={classes.column} md={6}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            onClick={formulario.handleSubmit(onSubmit)}
                        >
                            GENERAR
                        </Button>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        {isDescarga && (
                            <Fab color="primary" aria-label="add" onClick={formulario.handleSubmit(onDownload)}>
                                <CloudDownload />
                            </Fab>
                        )}
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={12}>
                        {dataGeneralCartera && (
                            <TablaListaCartera listaCarteraGeneral={datosExcel} />
                        )}
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={12}>
                        {dataPreliminarCartera && (
                            <TablaPreliminarCartera listaCarteraPreliminar={datosExcel} />
                        )}
                    </Grid>
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    );
}
export default ReportsGenerales;