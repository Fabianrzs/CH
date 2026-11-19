import React, { forwardRef, useEffect, useImperativeHandle, useRef, useState } from 'react';
import {
    Button,
    createStyles,
    Grid,
    IconButton,
    LinearProgress,
    makeStyles,
    MenuItem,
    Tooltip,
} from '@material-ui/core';
import swal from 'sweetalert';
import { CloudDownloadOutlined, CancelOutlined, DeleteOutlined } from '@material-ui/icons';
import { toast } from 'react-toastify';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import moment from 'moment';
import saveAs from 'file-saver';
import { MaterialUiPickersDate } from '@material-ui/pickers/typings/date';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import ApiReportes from '../../../services/api-reportes.service';
import { Reportes } from '../../../models/reportes-data.model';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import TablasReportes, {
    ReporteCargaAnalista,
    ReporteConsultasRestrictivas,
    ReporteGeneralDeCartera,
    ReporteGeneralDeGarantia,
    ReporteGeneralDeGarantias,
    ReporteGeneralDeMora,
    ReportePreliminarDeCartera,
    ReportesAnalistasDeCredito,
    ReportesAvaluos,
    ReporteValorDeVenta,
} from './TablasReportes';
import CHDatePickerV3 from '../../../components/ch-datepicker/CHDatepickerV3';
import TablaReporteEstadoIntegraciones from './TablaReporteEstadoIntegraciones';
import CreditApi from '../../../services/api-credit.service';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import CatalogApi from '../../../services/api-catalog.service';
import ApiproductoClients from '../../../services/api-producto.service';
import NotificationService from '../../../services/notification.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

type Nullable<T> = T | undefined | null;

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
        },
        procesoReportesContenedor: {
            marginTop: '2rem',
            marginRight: '1rem',
            maxHeight: '500px',
            overflowY: 'auto'
        }
    }),
);
const Reports = () => {
    const apiReportes = ApiReportes();
    const apiCatalog = CatalogApi();
    const apiProductos = ApiproductoClients();
    const apiCredito = CreditApi();
    const formulario = useForm({
        mode: 'onChange',
        defaultValues: {
            nombreReporte: '',
            identificacion: '',
            fechaReportes: moment(new Date())
                .subtract(1, 'months')
                .format('YYYY-MM'),
            fecha: new Date(),
            fechaRePlanos: new Date(),
            fecha416: new Date(Number(new Date().getFullYear()), Number(new Date().getMonth()) + 1, 0),
            fechaInicio: new Date(),
            fechaFin: new Date(),

            tipoCredito: 0 as Nullable<number>,
            periodoActual: true as Nullable<boolean>,
            periodoReporte: null as Nullable<Date>,
            numeroCredito: null as Nullable<number>,
            fuerza: null as Nullable<number>,
            categoria: null as Nullable<number>,
            unidadEjecutora: null as Nullable<number>,
        },
    });

    const reporteSeleccionado = formulario.watch('nombreReporte');

    const [listaReportes, setListaReportes] = useState<Reportes[]>([]);
    const [fuerzas, setFuerzas] = useState<any[]>([]);
    const [categorias, setCategorias] = useState<any[]>([]);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const [tablaDocExcel, setTablaDocExcel] = useState(false);
    const [avaluos, setAvaluos] = useState(false);
    const [cargaAnalista, setCargaAnalista] = useState(false);
    const [isLoadingFile, setIsLoadingFile] = useState(false);
    const [reporteGeneralGarantias, setReporteGeneralGarantias] =
        useState(false);
    const [reportePreliminarCartera, setReportePreliminarCartera] =
        useState(false);
    const [reporteValorVenta, setReporteValorVenta] = useState(false);
    const [reportesAnalistasCredito, setReportesAnalistasCredito] =
        useState(false);
    const [reporteConsultasRestrictivas, setReporteConsultasRestrictivas] =
        useState(false);
    const [datosExcel, setDatosExcel] = useState<any[]>([]);
    const [conIdentificacion, setConIdentificacion] = useState(false);
    const [conFechaMesAnio, setConFechaMesAnio] = useState(false);
    const [fechas, setFechas] = useState(false);
    const [fechas416, setFechas416] = useState(false);
    const [fechasPlanos, setFechasPlanos] = useState(false);
    const [reporteEstadoIntegracion, setReporteEstadoIntegracion] =
        useState(false);
    const [datosReporteEstadoIntegracion, setDatosReporteEstadoIntegracion] =
        useState<any[]>([]);
    const [datosReporteGeneralCartera, setDatosReporteGeneralCartera] = useState<any[]|null>(null);
    const [datosReporteGeneralMora, setDatosReporteGeneralMora] = useState<any[]|null>(null);
    const [datosReporteGeneralGarantia, setDatosReporteGeneralGarantia] = useState<any[]|null>(null);

    const ProcesoGeneracionReportesRef = useRef<any>(); 

    
    useEffect(() => {
        listarRepostes();
        obtenerFuerzas();
        obtenerCategorias();
        obtenerUnidadesEjecutoras();
    }, []);

    const dias = (date: MaterialUiPickersDate ) => {
        const DateDia = date?.getDay()
        const day = DateDia 
            return   Number(day)!==5 
    }

    const listarRepostes = async () => {
        
        await apiReportes
            .generarListaReportes()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        const otrosReportes = [
                            'Certificado de haberes',
                            'Certificado ingresos y retenciones (anual)',
                            'Descuentos de Nómina',
                            'Extracto (anual)',
                            'Extracto (mensual)',
                            'Reporte 414',
                            'Reporte 416',
                            'Reporte Aseguradora',
                            'Reporte Carga analista',
                            'Reporte Cesiones',
                            'Reporte Comercial TU',
                            'Reporte Consultas Restrictivas',
                            'Reporte estado integracion',
                            'Reporte Estudios de Titulos',
                            'Reporte preliminar de cartera',
                            'Reporte preliminar de garantías',
                            'Reporte valor de venta vs. Valor de venta',
                            'Certificacion consolidada de haberes',
                            'Reporte Resumen Financiero',
                            'Reporte Cartera',
                            'Reporte Avalúos',
                            'Reporte Paz y Salvo'
                        ];

                        const array = response.data?.filter((x: any) => !otrosReportes.includes(x.nombreReporte));
                        setListaReportes(array);
                        
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerFuerzas = () => {
        apiCatalog.getAllForces()
            .then((response: any) => {
                if (response?.isSuccess) {
                    setFuerzas(response.data);
                }
            });
    }

    const obtenerCategorias = () => {
        apiCatalog.getCategories()
            .then((response: any) => {
                if (response?.isSuccess) {
                    setCategorias(response.data);
                }
            });
    }

    const obtenerUnidadesEjecutoras = () => {
        apiCatalog.getAllEXECUTINGUNITSRequest()
            .then((response: any) => {
                if (response?.isSuccess) {
                    setUnidadesEjecutoras(response.data);
                }
            });
    }

    const onSubmitCertificadoGeneralCartera = (data: any,requestAuditoria:Auditoria) => {
        setIsLoadingFile(true);
        const request = {
            periodoActual: data.periodoActual ?? true,
            tipoCredito: data.tipoCredito ? data.tipoCredito : null,
            anio: data.periodoReporte?.getFullYear() ?? null,
            mes: data.periodoReporte ? data.periodoReporte.getMonth() + 1 : null,
            numeroCredito: data.numeroCredito ? data.numeroCredito : null,
            fuerza: (!data.fuerza || data.fuerza === 'all') ? null : data.fuerza,
            categoria: (!data.categoria || data.categoria === 'all') ? null : data.categoria
        }
        let tipoCredito=request.tipoCredito===1?'LEASING HABITACIONAL':'';       
        tipoCredito=request.tipoCredito===2?'HIPOTECARIO':'CONSULTA TOTAL CARTERA';
        const auditoriaApi=AuditoriaApi();
        apiReportes
            .reporteGeneralCartera(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        setDatosReporteGeneralCartera(response.data ?? []);
                        requestAuditoria.tipoAccion='TRANSACCIONES';
                        requestAuditoria.idTipoAccion=2;
                        requestAuditoria.descripcion=`Generación reporte general de cartera parámetros:     
                         \n Periodo actual:  ${request.periodoActual?'Sí':'No'}                    
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                         \n Número crédito:  ${request.numeroCredito===null?'':request.numeroCredito} 
                         \n Fuerza:  ${request.fuerza===null?'TODAS':fuerzas.find(x=>x.id===request.fuerza)?.descripcion}
                         \n Categoría:  ${request.categoria===null?'TODAS':categorias.find(x=>x.id===request.categoria)?.descripcion}
                         `
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        setDatosReporteGeneralCartera([]);
                        requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion=8;
                        requestAuditoria.descripcion=`Error Generación reporte general de cartera parámetros:     
                         \n Periodo actual:  ${request.periodoActual?'Sí':'No'}                    
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                         \n Número crédito:  ${request.numeroCredito===null?'':request.numeroCredito} 
                         \n Fuerza:  ${request.fuerza===null?'TODAS':fuerzas.find(x=>x.id===request.fuerza)?.descripcion}
                         \n Categoría:  ${request.categoria===null?'TODAS':categorias.find(x=>x.id===request.categoria)?.descripcion}
                        \n returnApiMessage: ${response.returnMessage}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                }
            })
            .catch((error:any) => {
                toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Error Generación reporte general de cartera parámetros:     
                         \n Periodo actual:  ${request.periodoActual?'Sí':'No'}                    
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte===null||data.periodoReporte===undefined?'': moment(data.periodoReporte).format("YYYY/MM")}
                         \n Número crédito:  ${request.numeroCredito===null?'':request.numeroCredito} 
                         \n Fuerza:  ${request.fuerza===null?'TODAS':fuerzas.find(x=>x.id===request.fuerza)?.descripcion}
                         \n Categoría:  ${request.categoria===null?'TODAS':categorias.find(x=>x.id===request.categoria)?.descripcion}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }).finally(() => setIsLoadingFile(false));
    };

    const onSubmitReporteGeneralMora = (data: any,requestAuditoria:Auditoria) => {
        setIsLoadingFile(true);

        const date = new Date();
        const request = {
            anio: data.periodoReporte?.getFullYear() ?? date.getFullYear(),
            mes: (data.periodoReporte?.getMonth() ?? date.getMonth()) + 1
        }       
        const auditoriaApi=AuditoriaApi();
        apiReportes
            .reporteGeneralMora(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        setDatosReporteGeneralMora(response.data ?? []);
                        requestAuditoria.tipoAccion='TRANSACCIONES';
                        requestAuditoria.idTipoAccion=2;
                        requestAuditoria.descripcion=`Generación reporte general de mora parámetros:       
                         \n Periodo:  ${data.periodoReporte}                   
                        `
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        setDatosReporteGeneralMora([]);
                    }
                }
            })
            .catch((error:any) => {
                toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Error Generación reporte general de mora parámetros:       
                         \n Periodo:  ${data.periodoReporte}   
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }).finally(() => setIsLoadingFile(false));
    };

    const onSubmitReportePazSalvo = (data: any,requestAuditoria:Auditoria) => {
        setIsLoadingFile(true);
        const auditoriaApi=AuditoriaApi();
        apiReportes
            .reportePazSalvo(data.numeroCredito)
            .then(async response => {
                if (response?.type === "application/pdf") {
                    const url = URL.createObjectURL(response);
                    window.open(url);
                    requestAuditoria.tipoAccion='DESCARGAS';
                    requestAuditoria.idTipoAccion=5;
                    requestAuditoria.descripcion=`Descarga reporte de paz y salvo en formato pdf parámetros:
                         \n Número de crédito:  ${data.numeroCredito}                                              
                         \n archivo: ${data.nombreReporte}`;
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            })
            .catch(async (e) => {
                if(e.response) {
                    const result = JSON.parse(await e.response.data.text());
                    toast(result.returnMessage, { type: toast.TYPE.ERROR });
                } else {
                    toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                }
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Error Descarga reporte de paz y salvo formato pdf parámetros:
                \n Número de crédito:  ${data.numeroCredito}                                              
                \n archivo: ${data.nombreReporte}   
                \n Error:  ${e}
                \n returnApiMessage: ${e?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }).finally(() => setIsLoadingFile(false));
    };

    const onSubmitReporteCertificacionReteFuente = async (data: any,requestAuditoria: Auditoria) => {

        setIsLoadingFile(true);

        if(data.periodoReporte) {

            const anio = data.periodoReporte?.getFullYear();
            let tipoCredito=data.tipoCredito===1?'LEASING HABITACIONAL':'';       
                tipoCredito=data.tipoCredito===2?'HIPOTECARIO':'CONSULTA TOTAL CARTERA';
            const auditoriaApi=AuditoriaApi();
            try {
                const result = await apiReportes.RetefuenteAnioGravable({
                    anio: anio,
                    tipoCredito: data.tipoCredito ? data.tipoCredito : null,
                });
                
                
                if(result?.isSuccess) {
                    NotificationService.genericAlert(`Generación de certificación retefuente año gravable ${anio}`, result.returnMessage);
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Generación de certificación retefuente año gravable ${anio} parámetros:                            
                     \n Tipo crédito:  ${tipoCredito}
                     \n Periodo:  ${data.periodoReporte}                     
                     `
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }

                ProcesoGeneracionReportesRef.current?.consultarProcesos();
            }
            catch(e: any) {
                if(e.response) {
                    const result = JSON.parse(await e.response.data.text());
                    NotificationService.genericAlertError(result.returnMessage, 'No se pudo generar el reporte');
                } else {
                    NotificationService.genericAlertError(`Generación de certificación retefuente año gravable ${anio}`, 'No se pudo generar el reporte');
                }
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Error Generación de certificación retefuente año gravable ${anio} parámetros:                            
                     \n Tipo crédito:  ${tipoCredito}
                     \n Periodo:  ${data.periodoReporte}    
                \n Error:  ${e}
                \n returnApiMessage: ${e?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }
            finally {
                setIsLoadingFile(false)
            }
        }
    };

    const onSubmitReporteExtracto = async (data: any, tipoExtracto: 'mensual' | 'anual',requestAuditoria:Auditoria) => {

        setIsLoadingFile(true);
        if(data.periodoReporte) {
            const mes = data.periodoReporte.getMonth() + 1;
            const anio = data.periodoReporte.getFullYear();
            let tipoCredito=data.tipoCredito===1?'LEASING HABITACIONAL':'';       
            tipoCredito=data.tipoCredito===2?'HIPOTECARIO':'CONSULTA TOTAL CARTERA';
            const auditoriaApi=AuditoriaApi();
            try {
                const result = await apiProductos.generarReporteExtractoMasivo({
                    anio,
                    mes: tipoExtracto === 'mensual' ? mes : null,
                    tipoCredito: data.tipoCredito ? data.tipoCredito : null,
                    numeroCredito: data.numeroCredito ? data.numeroCredito : null,
                    unidadEjecutora: data.unidadEjecutora ? data.unidadEjecutora : null
                }).catch((error:any)=>{
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Generación de extracto ${tipoExtracto} parámetros:                         
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte}  
                         \n Número crédito:  ${data.numeroCredito ? data.numeroCredito : ''} 
                         \n Unidad ejecutora:  ${data.unidadEjecutora ? unidadesEjecutoras.find(x=>x.id===data.unidadEjecutora).sigla : 'TODAS'} 
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });

                if(result?.isSuccess) {
                    NotificationService.genericAlert(`Generación de extracto ${tipoExtracto}`, result.returnMessage);
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                        requestAuditoria.idTipoAccion=2;
                        requestAuditoria.descripcion=`Generación de extracto ${tipoExtracto} parámetros:                         
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte}  
                         \n Número crédito:  ${data.numeroCredito ? data.numeroCredito : ''} 
                         \n Unidad ejecutora:  ${data.unidadEjecutora ? unidadesEjecutoras.find(x=>x.id===data.unidadEjecutora).sigla : 'TODAS'} 
                        `
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }

                ProcesoGeneracionReportesRef.current?.consultarProcesos();
            }
            catch {
                NotificationService.genericAlertError(`Generación de extracto ${tipoExtracto}`, "Error al generar el reporte");
            }
            finally {
                setIsLoadingFile(false)
            }
        }
      
    };

    const onSubmitCertificadoGeneralGarantia = (data: any,requestAuditoria:Auditoria) => {
        setIsLoadingFile(true);
        const request = {
            tipoCredito: data.tipoCredito ? data.tipoCredito : null,
            anio: data.periodoReporte?.getFullYear() ?? null,
            mes: data.periodoReporte ? data.periodoReporte.getMonth() + 1 : null,
        }
        let tipoCredito=data.tipoCredito===1?'LEASING HABITACIONAL':'';       
        tipoCredito=data.tipoCredito===2?'HIPOTECARIO':'CONSULTA TOTAL CARTERA';
        const auditoriaApi=AuditoriaApi();
        apiReportes
            .reporteGeneralGarantia(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        setDatosReporteGeneralGarantia(response.data ?? []);
                        requestAuditoria.tipoAccion='TRANSACCIONES';
                        requestAuditoria.idTipoAccion=2;
                        requestAuditoria.descripcion=`Generación de certificado general de garantía parámetros:                         
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte}`                         
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        setDatosReporteGeneralGarantia([]);
                    }
                }
            })
            .catch((error:any) => {
                toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Generación de certificado general de garantía parámetros:                         
                         \n Tipo crédito:  ${tipoCredito}
                         \n Periodo:  ${data.periodoReporte} 
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
            }).finally(() => setIsLoadingFile(false));
    };
    
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
    }, []);

    const onSubmit = async (item: any) => {
        const user = AuthService.getUserInfo();
            const auditoriaApi= AuditoriaApi();                 
            const nav =(window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
            const requestAuditoria={
                    navegador:usrAg,
                    sistemaOperativo:nav.userAgentData.platform,
                    nombreUsuario:user.USR_USERNAME, 
                    usurioId:user.USR_ID,
                    ip: publicIpAddress,
                    modulo:'Otros Reportes',
                    hostname:document.location.host
            } as Auditoria;
        if (item.nombreReporte === 'Certificado de haberes') {
            setIsLoadingFile(true);
            const identificacion = {
                identificacion: item.identificacion,
            };
            await apiReportes
                .generarReportes(identificacion)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Identificación: ${identificacion.identificacion}
                         \n archivo: Certificado de haberes`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                         \n Identificación: ${identificacion.identificacion}
                         \n archivo: Certificado de haberes
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });

        } else if (item.nombreReporte === 'Reporte Resumen Financiero') {            
            setIsLoadingFile(true);
            const request = {
                identification: item.identificacion,
            };
            await apiReportes
                .ReporteResumenSolicitud(request)
                .then(async response => {
                    setIsLoadingFile(false);
            
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Identificación: ${request.identification}
                         \n archivo: Reporte Resumen Financiero`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                    }
                    
                )
                .catch(async(error)  => {
                    setIsLoadingFile(false);
                    const completeResponse = error.response;
                    const data = JSON.parse(await completeResponse.data.text()) 
                    toast(`Error: ${data.returnMessage}`, {
                        type: toast.TYPE.ERROR,
                    });
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                    \n Identificación: ${request.identification}
                    \n archivo: Reporte Resumen Financiero
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                })
            
        } else if (item.nombreReporte === 'Certificacion consolidada de haberes') {
           
            setIsLoadingFile(true);
            const identificacion = {
                customerIdentification: item.identificacion,
            };
            await apiCredito
                .ReporteGeneralHaberes(identificacion)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Identificación: ${identificacion.customerIdentification}
                         \n archivo: Certificación consolidada de haberes`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                })
                .catch(async(error)  => {
                    setIsLoadingFile(false);
                    const completeResponse = error.response;
                    const data = JSON.parse(await completeResponse.data.text()) 
                    toast(`Error: ${data.returnMessage}`, {
                        type: toast.TYPE.ERROR,
                    });
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                    \n Identificación: ${identificacion.customerIdentification}
                    \n archivo:  Certificación consolidada de haberes
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                })
            


        } else if (item.nombreReporte === 'Conciliación Mensual y Anual') {
            console.log('pendiente');
        } else if (item.nombreReporte === 'Descuentos de nómina al corte') {
            console.log('pendiente');
        } else if (item.nombreReporte === 'Estado de cuenta (mensual)') {
            setIsLoadingFile(true);
            const identificacion = {
                identificacion: item.identificacion,
            };
            await apiReportes
                .generarEstadoDeCuenta(identificacion)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Identificación: ${identificacion.identificacion}
                         \n archivo: Estado de cuenta (mensual)`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                })
                .catch((error:any) => {
                    toast('Error el número de identificación no existe', {
                        type: toast.TYPE.ERROR,
                    });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                    \n Identificación: ${identificacion.identificacion}
                    \n archivo: Estado de cuenta (mensual)
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Extracto (anual)') {
            onSubmitReporteExtracto(item, 'anual',requestAuditoria);
        } else if (item.nombreReporte === 'Extracto (mensual)') {
            onSubmitReporteExtracto(item, 'mensual',requestAuditoria);
        } else if (item.nombreReporte === 'Movimiento cartera') {
            setIsLoadingFile(true);

            const identificacion = {
                identificacion: item.identificacion,
            };
            await apiReportes
                .generarMovimientoCartera(identificacion)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Identificación: ${identificacion.identificacion}
                         \n archivo: Movimiento cartera`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                })
                .catch((error:any) => {
                    toast('Error el número de identificación no existe', {
                        type: toast.TYPE.ERROR,
                    });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                    \n Identificación: ${identificacion.identificacion}
                    \n archivo:  Movimiento cartera
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte Comercial TU') {
            setIsLoadingFile(true);
            const identificacion = {
                NumeroIdentificacion: item.identificacion,
            };
            await apiReportes
                .postReporteTransUnion(identificacion)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Identificación: ${identificacion.NumeroIdentificacion}
                         \n archivo: Reporte Comercial TU`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                })
                .catch(async(error)  => {
                    setIsLoadingFile(false);
                    const completeResponse = error.response;
                    const data = JSON.parse(await completeResponse.data.text()) 
                    toast(`Error: ${data.returnMessage}`, {
                        type: toast.TYPE.ERROR,
                    });
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                    \n Identificación: ${identificacion.NumeroIdentificacion}
                    \n archivo:   Reporte Comercial TU
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte 414') {
           
            const DateDia = item.fecha.getDay()
            if( DateDia!== 5){
                toast('Fecha no valida',  { type: toast.TYPE.ERROR },)
              
            }else{
                setIsLoadingFile(true);
                const request = {
                  fechaAplicacion:  moment(item.fecha).format('YYYY-MM-DDT05:00:00.000Z')
                 
                };
    
                await apiReportes
                    .generarMReporte414(request)
                    .then(async response => {
                        if (response) {
                            setIsLoadingFile(false);
                            const mimeType = response.type;
                            if (mimeType) {
                                const file = new Blob([response], {
                                    type: mimeType,
                                });
                                const fileURL = URL.createObjectURL(file);
                                window.open(fileURL, '_blank');
                                requestAuditoria.tipoAccion='DESCARGAS';
                                requestAuditoria.idTipoAccion=5;
                                requestAuditoria.descripcion=`Descarga de reporte
                                 \n Fecha aplicación: ${moment(item.fecha).format('DD-MM-YYYY')}
                                 \n archivo: Reporte 414`;
                                await auditoriaApi.InsertAuditoria(requestAuditoria);
                            }
                        }
                    })
                    .catch((error:any) => {
                        toast(
                            'No existe información en la base de datos con el filtro de fechas ingresado.',
                            { type: toast.TYPE.ERROR },
                        );
                        setIsLoadingFile(false);
                        requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion=8;
                        requestAuditoria.descripcion=`Error Descarga de reporte
                                 \n Fecha aplicación: ${moment(item.fecha).format('DD-MM-YYYY')}
                                 \n archivo: Reporte 414
                            \n Error:  ${error}
                            \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    });
            }

        } else if (item.nombreReporte === 'Reporte 416') {
     
      
            const dateNow = new Date();
           
          
            const fecha = new Date(item.fecha416);
            if (!moment(fecha, true).isValid()) {
                return;
            }
            if (fecha > dateNow) {
                return;
            }

            setIsLoadingFile(true);
                const request = {
                    fechaAplicacion: item.fecha416,
                };
                await apiReportes
                .generarMReporte416(request)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                        requestAuditoria.tipoAccion='DESCARGAS';
                        requestAuditoria.idTipoAccion=5;
                        requestAuditoria.descripcion=`Descarga de reporte
                         \n Fecha aplicación: ${moment(item.fecha416).format('DD-MM-YYYY')}
                         \n archivo: Reporte 416`;
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                })
                .catch((error:any) => {
                    toast(
                        'No existe información en la base de datos con el filtro de fechas ingresado.',
                        { type: toast.TYPE.ERROR },
                    );
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                             \n Fecha aplicación: ${moment(item.fecha).format('DD-MM-YYYY')}
                             \n archivo: Reporte 416
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
              
              
              
        } else if (item.nombreReporte === 'Reporte Plano 414') {
            setIsLoadingFile(true);
            const request = {
                fechaAplicacion: item.fechaRePlanos,
            };
            console.log("414");
            
            await apiReportes
                .generarMReportePlano414(request)
                .then(async response => {
                    setIsLoadingFile(false);
                
                    saveAs(response, 'Reporte-plano-414.txt');
                    requestAuditoria.tipoAccion='DESCARGAS';
                    requestAuditoria.idTipoAccion=5;
                    requestAuditoria.descripcion=`Descarga de reporte
                     \n Fecha aplicación: ${moment(item.fechaRePlanos).format('DD-MM-YYYY')}
                     \n archivo: Reporte-plano-414.txt`;
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                })
                .catch((error:any) => {
                    toast(
                        'No existe información en la base de datos con el filtro de fechas ingresado.',
                        { type: toast.TYPE.ERROR },
                    );
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                             \n Fecha aplicación: ${moment(item.fechaRePlanos).format('DD-MM-YYYY')}
                             \n archivo: Reporte-plano-414.txt
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if(item.nombreReporte === 'Reporte Plano 341'){
            setIsLoadingFile(true);
            const request = {
                fechaAplicacion: item.fechaRePlanos,
            };
            await apiReportes
                .generarReportePlano341(request)
                .then(async response => {
                    setIsLoadingFile(false);
                    saveAs(response, 'Reporte-plano-341.txt');
                    requestAuditoria.tipoAccion='DESCARGAS';
                    requestAuditoria.idTipoAccion=5;
                    requestAuditoria.descripcion=`Descarga de reporte
                     \n Fecha aplicación: ${moment(item.fechaRePlanos).format('DD-MM-YYYY')}
                     \n archivo: Reporte-plano-341.txt`;
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                })
                .catch((error:any) => {
                    toast(
                        'No existe información en la base de datos con el filtro de fechas ingresado.',
                        { type: toast.TYPE.ERROR },
                    );
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                             \n Fecha aplicación: ${moment(item.fechaRePlanos).format('DD-MM-YYYY')}
                             \n archivo:  Reporte-plano-341.txt
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte Plano 416') {
            setIsLoadingFile(true);
            const request = {
                fechaAplicacion: item.fechaRePlanos,
            };
            await apiReportes
                .generarReportePlano416(request)
                .then(async response => {
                   setIsLoadingFile(false);
                    saveAs(response, 'Reporte-plano-416.txt');
                    requestAuditoria.tipoAccion='DESCARGAS';
                    requestAuditoria.idTipoAccion=5;
                    requestAuditoria.descripcion=`Descarga de reporte
                     \n Fecha aplicación: ${moment(item.fechaRePlanos).format('DD-MM-YYYY')}
                     \n archivo: Reporte-plano-416.txt`;
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                })
                .catch((error:any) => {
                    toast(
                        'No existe información en la base de datos con el filtro de fechas ingresado.',
                        { type: toast.TYPE.ERROR },
                    );
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte
                             \n Fecha aplicación: ${moment(item.fechaRePlanos).format('DD-MM-YYYY')}
                             \n archivo: Reporte-plano-416.txt
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte Aseguradora') {
            setIsLoadingFile(true);
            setTablaDocExcel(true);
            apiReportes
                .reporteAseguradora()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte Aseguradora`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte Aseguradora
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte Avalúos') {
            setIsLoadingFile(true);
            setAvaluos(true);
            apiReportes
                .reporteAvaluos()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte Avalúos`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte Avalúos
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte Carga analista') {
            setIsLoadingFile(true);
            setCargaAnalista(true);
            apiReportes
                .reporteCargaAnalista()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte Carga analista`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                     requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte Carga analista
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte CDP Y RP') {
            console.log('pendiente');
        } else if (
            item.nombreReporte ===
            'Reporte Cesiones – Promesa – Escrituración Aliados Estrategicos'
        ) {
            console.log('pendiente');
        } else if (
            item.nombreReporte === 'Reporte experto información comercial'
        ) {
            console.log('pendiente');
        } else if (item.nombreReporte === 'Reporte General de cartera') {
            onSubmitCertificadoGeneralCartera(item,requestAuditoria);
        } else if (item.nombreReporte === 'Reporte de Mora') {
            onSubmitReporteGeneralMora(item,requestAuditoria);
        } else if (item.nombreReporte === 'Reporte general de garantías') {
            onSubmitCertificadoGeneralGarantia(item,requestAuditoria);
        } else if (item.nombreReporte === 'Reporte Paz y Salvo') {
            onSubmitReportePazSalvo(item,requestAuditoria);
        } else if (item.nombreReporte === 'Certificado ingresos y retenciones (anual)') {
            onSubmitReporteCertificacionReteFuente(item,requestAuditoria);
        } else if (item.nombreReporte === 'Reporte general de garantías') {
            setIsLoadingFile(true);
            setReporteGeneralGarantias(true);
            apiReportes
                .reporteGeneralGarantias()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte general de garantías`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte general de garantías
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte jefe') {
            setIsLoadingFile(true);

            apiReportes
                .reporteJefe()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte jefe`;
                            await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte jefe
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (
            item.nombreReporte === 'Reporte operaciones aprobadas y rechazadas'
        ) {
            console.log('pendiente');
        } else if (item.nombreReporte === 'Reporte preliminar de cartera') {
            setIsLoadingFile(true);
            setReportePreliminarCartera(true);
            apiReportes
                .reportePreliminarCartera()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte preliminar de cartera`;
                             await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte preliminar de cartera
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (
            item.nombreReporte === 'Reporte valor de venta vs. Valor de venta'
        ) {
            setIsLoadingFile(true);
            setReporteValorVenta(true);
            apiReportes
                .reporteValorVenta()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte valor de venta`;
                             await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte valor de venta
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reportes Analistas de Crédito') {
            setIsLoadingFile(true);
            setReportesAnalistasCredito(true);
            apiReportes
                .reportesAnalistasCredito()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte Analistas de Crédito`;
                             await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                    \n archivo: Reporte Analistas de Crédito
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte Consultas Restrictivas') {
            setIsLoadingFile(true);
            setReporteConsultasRestrictivas(true);
            apiReportes
                .reporteConsultaRestrictivas()
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosExcel(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='DESCARGAS';
                            requestAuditoria.idTipoAccion=5;
                            requestAuditoria.descripcion=`Descarga de reporte                             
                             \n archivo: Reporte Consultas Restrictivas`;
                             await auditoriaApi.InsertAuditoria(requestAuditoria);
                        } else {
                            toast('Error');
                            setIsLoadingFile(false);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Descarga de reporte                             
                        \n archivo: Reporte Consultas Restrictivas
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        } else if (item.nombreReporte === 'Reporte estado integracion') {
            setIsLoadingFile(true);

            if (new Date(item.fechaInicio) > new Date(item.fechaFin)) {
                toast('La fecha inicio no puede ser mayor a la fecha fin', {
                    type: toast.TYPE.ERROR,
                });
                setIsLoadingFile(false);
                return;
            }

            const request = {
                numeroDocumento: item.identificacion,
                fechaInicial: item.fechaInicio,
                fechaFinal: item.fechaFin,
            };
            apiReportes
                .reporteEstadoIntegracion(request)
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {
                            setDatosReporteEstadoIntegracion(response.data);
                            setIsLoadingFile(false);
                            requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Generación de reporte estado integración parámetros:
                         \n Identificación:  ${request.numeroDocumento}
                         \n Fecha inicial:  ${moment(request.fechaInicial).format("DD/MM/YYYY")}
                         \n Fecha final:  ${moment(request.fechaFinal).format("DD/MM/YYYY")}
                        `
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                        }
                    }
                })
                .catch((error:any) => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Error Generación de reporte estado integración parámetros:
                    \n Identificación:  ${request.numeroDocumento}
                    \n Fecha inicial:  ${moment(request.fechaInicial).format("DD/MM/YYYY")}
                    \n Fecha final:  ${moment(request.fechaFinal).format("DD/MM/YYYY")}
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                });
        }
    };
    const classes = useStyles();

    // if (isLoadingFile) {
    //     return <CHLoadingProcess />;
    // }

    return (
        <CHPrivateContainer>
            <LoadingV2Component key="loader" loading={isLoadingFile} tittle='Generando Reporte...' />

            <CHPaper title="Generación de reportes">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
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
                                    label="Documento*"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        const valor = value;
                                        if (
                                            valor.target.value ===
                                                'Certificacion consolidada de haberes' ||
                                            valor.target.value ===
                                                'Reporte Resumen Financiero' ||
                                            valor.target.value ===
                                                'Certificado de haberes' ||
                                            valor.target.value ===
                                                'Estado de cuenta (mensual)' ||
                                            valor.target.value ===
                                                'Movimiento cartera' ||
                                            valor.target.value ===
                                                'Reporte Comercial TU'
                                        ) {
                                            if (
                                                valor.target.value ===
                                                    'Estado de cuenta (mensual)'
                                            ) {
                                                setConIdentificacion(true);
                                                setConFechaMesAnio(true);
                                                setFechas(false);
                                                setFechasPlanos(false)
                                                setFechas416(false);
                                                setReporteEstadoIntegracion(
                                                    false,
                                                );
                                            } else {
                                                setConIdentificacion(true);
                                                setConFechaMesAnio(false);
                                                setReporteEstadoIntegracion(
                                                    false,
                                                );
                                            }

                                            setFechas(false);
                                            setFechas416(false);
                                        } else if (
                                            valor.target.value ===
                                                'Reporte Aseguradora' ||
                                            valor.target.value ===
                                                'Reporte Avalúos' ||
                                            valor.target.value ===
                                                'Reporte Carga analista' ||
                                            valor.target.value ===
                                                'Reporte general de cartera' ||
                                            valor.target.value ===
                                                'Reporte general de garantías' ||
                                            valor.target.value ===
                                                'Reporte jefe' ||
                                            valor.target.value ===
                                                'Reporte preliminar de cartera' ||
                                            valor.target.value ===
                                                'Reporte valor de venta vs. Valor de venta' ||
                                            valor.target.value ===
                                                'Reportes Analistas de Crédito' ||
                                            valor.target.value ===
                                                'Reporte General de cartera' ||
                                            valor.target.value ===
                                                'Reporte de Mora' ||
                                            valor.target.value ===
                                                'Reporte Paz y Salvo' ||
                                            valor.target.value ===
                                                'Extracto (anual)' ||
                                            valor.target.value ===
                                                'Extracto (mensual)' ||
                                            valor.target.value ===
                                                'Certificado ingresos y retenciones (anual)' ||
                                            valor.target.value ===
                                                'Reporte general de garantías'
                                        ) {
                                            setConIdentificacion(false);
                                            setConFechaMesAnio(false);
                                            setFechas(false);
                                            setFechas416(false);
                                            setReporteEstadoIntegracion(false);
                                            setFechasPlanos(false)
                                        } else if (
                                            valor.target.value ==='Reporte 414'
                                          
                                        ) {
                                            setConIdentificacion(false);
                                            setConFechaMesAnio(false);
                                            setFechas(true);
                                            setFechas416(false);
                                            setReporteEstadoIntegracion(false);
                                            setFechasPlanos(false)
                                        } else if(
                                            valor.target.value ===
                                            'Reporte estado integracion'
                                        ) {
                                            setReporteEstadoIntegracion(true);
                                            setConIdentificacion(true);
                                            setConFechaMesAnio(false);
                                            setFechas(false);
                                            setFechas416(false);
                                            setFechasPlanos(false)
                                        }
                                        else if (valor.target.value ===
                                            'Reporte 416'){
                                                setConIdentificacion(false);
                                                setConFechaMesAnio(false);
                                                setFechas416(true);
                                                setFechas(false);
                                                setFechasPlanos(false)
                                            }

                                            else if(  valor.target.value ===
                                                'Reporte Plano 414' ||
                                            valor.target.value ===
                                                'Reporte Plano 416' ||
                                            valor.target.value ===
                                                'Reporte Plano 341'){
                                                    setConIdentificacion(false);
                                                    setConFechaMesAnio(false);
                                                    setFechas(false);
                                                    setFechas416(false);
                                                    setReporteEstadoIntegracion(false);
                                                    setFechasPlanos(true)
                                                }
                                        onChange(value);
                                        setTablaDocExcel(false);
                                        setAvaluos(false);
                                        setCargaAnalista(false);
                                        setReporteGeneralGarantias(false);
                                        setReportePreliminarCartera(false);
                                        setReporteValorVenta(false);
                                        setReportesAnalistasCredito(false);
                                        setReporteConsultasRestrictivas(false);
                                        setDatosReporteGeneralCartera(null);
                                        setDatosReporteGeneralGarantia(null);
                                        setDatosReporteGeneralMora(null);

                                        const formOptions = { shouldValidate: false, shouldTouch: true};
                                        formulario.setValue('numeroCredito', null, formOptions);
                                        formulario.setValue('periodoReporte', null, formOptions);
                                        formulario.setValue('unidadEjecutora', null, formOptions);
                                        formulario.setValue('tipoCredito', null, formOptions);
                                    }}
                                >
                                    <MenuItem key={0} value={0}>
                                        -- Seleccionar --
                                    </MenuItem>
                                    {listaReportes?.map((option: any) => (
                                        <MenuItem
                                            key={option.idReporte}
                                            value={option.nombreReporte}
                                        >
                                            {option.nombreReporte}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
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
                                        InputLabelProps={{ shrink: true }} 
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}{' '}
                    {conFechaMesAnio && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaReportes"
                                control={formulario.control}
                                rules={{
                                    required: 'Es requirido',
                                    pattern: {
                                        value: /^\d{4}([/.]|[-.])(0?[1-9]|1[1-2])$/,
                                        message: 'Fecha no válida',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePickerV3
                                        views={['year', 'month']}
                                        error={!!error}
                                        label="Fecha *"
                                        invalidDateMessage="Fecha no válida"
                                        minDateMessage="La fecha no debe ser anterior a la fecha mínima"
                                        maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                        minDate={moment('1910-12').format(
                                            'YYYY-MM',
                                        )}
                                        disableFuture
                                        maxDate={new Date()}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {fechas && (
                        <Grid item md={3} className={classes.column}>
                            <Controller
                                name="fecha"
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
                                        shouldDisableDate={dias}
                                        onChange={onChange}
                                        disableFuture
                                        
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {fechas416 && (
                        <Grid item md={3} className={classes.column}>
                            <Controller
                                name="fecha416"
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
                                        invalidDateMessage='Fecha no valida'
                                        onChange={(value) => {
                                            const nevaFecha = (new Date(Number(value?.getFullYear()), Number(value?.getMonth()) + 1, 0))
                                            onChange(nevaFecha)
                                        }}
                                        disableFuture
                                        
                                    />
                                )}
                            />
                        </Grid>
                    )}
                      {fechasPlanos && (
                        <Grid item md={3} className={classes.column}>
                            <Controller
                                name="fechaRePlanos"
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
                                        invalidDateMessage='Fecha no valida'
                                        onChange={(value) => {
                                            onChange(value)
                                        }}
                                        disableFuture
                                        
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {reporteEstadoIntegracion && (
                        <Grid item md={3} className={classes.column}>
                            <Controller
                                name="fechaInicio"
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
                                        label="Fecha Inicio *"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {reporteEstadoIntegracion && (
                        <Grid item md={3} className={classes.column}>
                            <Controller
                                name="fechaFin"
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
                                        label="Fecha Fin *"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {reporteSeleccionado === 'Reporte General de cartera' &&
                        (
                            <>
                                <ControllerTipoCredito {...{classes, formulario}} />
                                <ControllerNumeroCredito {...{classes, formulario, required: false}} />
                                <ControllerPeriodoReporte {...{classes, formulario, admitePeriodoActual: true}} />
                                <ControllerPeriodoActual {...{classes, formulario}} />
                                <ControllerFuerza {...{classes, fuerzas, formulario}} />
                                <ControllerCategoria {...{classes, categorias, formulario}} />
                            </>
                        )
                    }
                    {reporteSeleccionado === 'Reporte de Mora' &&
                        (
                            <>
                                <ControllerPeriodoReporte {...{classes, formulario}} />
                            </>
                        )
                    }
                    {reporteSeleccionado === 'Reporte Paz y Salvo' &&
                        (
                            <>
                                <ControllerNumeroCredito {...{classes, formulario, required: true}} />
                            </>
                        )
                    }
                    {reporteSeleccionado === 'Extracto (mensual)' &&
                        (
                            <>
                                <ControllerTipoCredito {...{classes, formulario}} />
                                <ControllerNumeroCredito {...{classes, formulario, required: false}} />
                                <ControllerPeriodoReporte {...{classes, formulario}} />
                                <ControllerUnidadEjectutora {...{classes, unidadesEjecutoras, formulario}} />
                            </>
                        )
                    }
                    {reporteSeleccionado === 'Extracto (anual)' &&
                        (
                            <>
                                <ControllerTipoCredito {...{classes, formulario}} />
                                <ControllerNumeroCredito {...{classes, formulario, required: false}} />
                                <ControllerPeriodoReporte {...{classes, formulario, soloAnio: true}} />
                                <ControllerUnidadEjectutora {...{classes, unidadesEjecutoras, formulario}} />
                            </>
                        )
                    }
                    {reporteSeleccionado === 'Certificado ingresos y retenciones (anual)' &&
                        (
                            <>
                                <ControllerPeriodoReporte {...{classes, formulario, soloAnio: true}} />
                                <ControllerTipoCredito {...{classes, formulario}} />
                            </>
                        )
                    }
                    {reporteSeleccionado === 'Reporte general de garantías' &&
                        (
                            <>
                                <ControllerPeriodoReporte {...{classes, formulario}} />
                                <ControllerTipoCredito {...{classes, formulario}} />
                            </>
                        )
                    }
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            onClick={formulario.handleSubmit(onSubmit)}
                        >
                            GENERAR
                        </Button>
                    </Grid>
                </Grid>
                
                <div style={{width: '100%', 'display': 'table', 'tableLayout': 'fixed'}}>
                        {tablaDocExcel && <TablasReportes data={datosExcel} />}
                        {avaluos && <ReportesAvaluos data={datosExcel} />}
                        {cargaAnalista && (
                            <ReporteCargaAnalista data={datosExcel} />
                        )}
                        {reporteSeleccionado === 'Reporte General de cartera' && datosReporteGeneralCartera && (
                            <ReporteGeneralDeCartera data={datosReporteGeneralCartera} periodo={formulario.getValues('periodoReporte')} />
                        )}
                        {reporteSeleccionado === 'Reporte general de garantías' && datosReporteGeneralGarantia && (
                            <ReporteGeneralDeGarantia data={datosReporteGeneralGarantia} periodo={formulario.getValues('periodoReporte')} />
                        )}
                        {reporteSeleccionado === 'Reporte de Mora' && datosReporteGeneralMora && (
                            <ReporteGeneralDeMora data={datosReporteGeneralMora} periodo={formulario.getValues('periodoReporte')} />
                        )}
                        {reporteGeneralGarantias && (
                            <ReporteGeneralDeGarantias data={datosExcel} />
                        )}
                        {reportePreliminarCartera && (
                            <ReportePreliminarDeCartera data={datosExcel} />
                        )}
                        {reporteValorVenta && (
                            <ReporteValorDeVenta data={datosExcel} />
                        )}
                        {reportesAnalistasCredito && (
                            <ReportesAnalistasDeCredito data={datosExcel} />
                        )}
                        {reporteConsultasRestrictivas && (
                            <ReporteConsultasRestrictivas data={datosExcel} />
                        )}
                        {datosReporteEstadoIntegracion.length !== 0 && (
                            <TablaReporteEstadoIntegraciones
                                data={datosReporteEstadoIntegracion}
                            />
                        )}

                        {
                            (  
                                reporteSeleccionado === 'Extracto (anual)'   ||
                                reporteSeleccionado === 'Extracto (mensual)' ||
                                reporteSeleccionado === 'Certificado ingresos y retenciones (anual)'
                            ) && <ProcesoGeneracionReportes ref={ProcesoGeneracionReportesRef} tipoReporte={reporteSeleccionado}/>
                        }
                </div>

            </CHPaper>
        </CHPrivateContainer>
    );
};



const ControllerTipoCredito = (props: {classes: any, formulario: UseFormReturn<any>}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            name="tipoCredito"
            control={props.formulario.control}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHSelect
                    label="Tipo Crédito"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }>
                    <MenuItem value={0}>CONSULTA TOTAL CARTERA</MenuItem>
                    <MenuItem value={1}>LEASING HABITACIONAL</MenuItem>
                    <MenuItem value={2}>HIPOTECARIO</MenuItem>
                </CHSelect>
            )}
        />
    </Grid>
);

const ControllerNumeroCredito = (props: {classes: any, formulario: UseFormReturn<any>, required?: boolean}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            control={props.formulario.control}
            name="numeroCredito"
            rules={{
                required: {value: props.required ?? false, message: 'Este campo es requerido.'},
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHTextField
                    label="Número obligacion"
                    value={value}
                    onChange={onChange}
                    numberOnlyMask
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                />
            )}
        />
    </Grid>
);

const ControllerPeriodoReporte = (props: {classes: any, formulario: UseFormReturn<any>, admitePeriodoActual?: boolean, soloAnio?: boolean}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            name="periodoReporte"
            control={props.formulario.control}
            rules={{
                validate: (val: any) => {
                    const periodoActual = props.formulario.getValues('periodoActual');
                    return  (props.admitePeriodoActual ? (!periodoActual && !val) : !val)
                        ? 'El periodo es obligatorio'
                        : true;
                }
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHDatePicker
                    label="Periodo"
                    inputProps={{ readOnly: true }}
                    onChange={(date, _) => {
                        const lastDay = moment(date).endOf('month').startOf('day').toDate();
                        onChange(lastDay);
                    }}
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                    value={value}
                    autoOk
                    views={props.soloAnio ? ['year'] : ['year', 'month']}
                    format={props.soloAnio ? 'yyyy': 'yyyy-MMMM'}
                    placeholder={props.soloAnio ? 'YYYY': 'YYYY-MM'}
                    disabled={props.admitePeriodoActual && (props.formulario.getValues('periodoActual') ?? false)}
                />
            )}
        />
    </Grid>
);

const ControllerPeriodoActual = (props: {classes: any, formulario: UseFormReturn<any>}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            name="periodoActual"
            control={props.formulario.control}
            render={({
                field: { onChange, value },
            }) => (
                <CHCheckbox
                    label="Periodo Actual"
                    onChange={(e, value) => {
                        onChange(e);
                        props.formulario.setValue("periodoReporte", null, {shouldValidate: true});
                    }}
                    checked={value ?? false}
                />
            )}
        />
    </Grid>
)

const ControllerFuerza = (props: {classes: any, fuerzas: any[], formulario: UseFormReturn<any>}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            name="fuerza"
            control={props.formulario.control}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHSelect
                    defaultValue=""
                    label="Fuerza"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                >
                    <MenuItem value='all'>
                        TODAS
                    </MenuItem>
                    {props.fuerzas?.map((item: any) => (
                        <MenuItem key={item.id} value={item.id}>
                            {item.descripcion}
                        </MenuItem>
                    ))}
                </CHSelect>
            )}
        />
    </Grid>
);

const ControllerCategoria = (props: {classes: any, categorias: any[], formulario: UseFormReturn<any>}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            name="categoria"
            control={props.formulario.control}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHSelect
                    defaultValue=""
                    label="Categoría"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                >
                    <MenuItem value='all'>
                        TODAS
                    </MenuItem>
                    {props.categorias?.map((item: any) => (
                        <MenuItem key={item.id} value={item.id}>
                            {item.descripcion}
                        </MenuItem>
                    ))}
                </CHSelect>
            )}
        />
    </Grid>
);

const ControllerUnidadEjectutora = (props: {classes: any, unidadesEjecutoras: any[], formulario: UseFormReturn<any>}) => (
    <Grid className={props.classes.column} item xs={12} md={3}>
        <Controller
            name="unidadEjecutora"
            control={props.formulario.control}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHSelect
                    defaultValue=""
                    label="Unidad Ejecutora"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                >
                    <MenuItem value={0}>
                        TODAS
                    </MenuItem>
                    {props.unidadesEjecutoras?.map((item: any) => (
                        <MenuItem key={item.id} value={item.id}>
                            {item.nombre}
                        </MenuItem>
                    ))}
                </CHSelect>
            )}
        />
    </Grid>
);

const ProcesoGeneracionReportes = forwardRef((props: any, ref: any) => {
    
    const classes = useStyles();
    const apiProductos = ApiproductoClients();
    const [procesos, setProcesos] = useState<any[]>([]);
    const consultando = useRef(false);
    const finalizar = useRef(false);
    const tipoReporte = useRef<string|undefined>();

    useImperativeHandle(ref, () => ({
        consultarProcesos
    }));

    useEffect(() => {
        finalizar.current = false;
        consultarProcesos();

        return () => {
            finalizar.current = true;
        }
    }, []);

    useEffect(() => {
        tipoReporte.current = {
            'Extracto (anual)': 'EXTRACTO_ANUAL_MASIVO',
            'Extracto (mensual)': 'EXTRACTO_MENSUAL_MASIVO',
            'Certificado ingresos y retenciones (anual)': 'RETEFUENTE_AÑO_GRAVABLE_MASIVO'
        }[props.tipoReporte as string];

        consultando.current = false;
        setProcesos([]);
        console.log(tipoReporte.current);
        consultarProcesos();

    }, [props.tipoReporte]);

    const consultarProcesos = () => {

        if(consultando.current || finalizar.current || !tipoReporte.current) {
            return;
        }

        consultando.current = true;

        apiProductos.consultaProcesoGeneracionReporte(tipoReporte.current)
        .then((response: any) => {
            if (response?.isSuccess) {
                setProcesos(response.data);
                if(response?.data?.some((x: any) => x.estado === "PENDIENTE") && !finalizar.current) {
                    setTimeout(() => {
                        consultando.current = false;
                        consultarProcesos();
                    }, 5000);
                }
                else {
                    consultando.current = false;
                }
            }
            return response;
        }).catch(e => {
            setTimeout(() => {
                consultando.current = false;
                consultarProcesos();
            }, 5000);
        });
    }
        
    
    const cancelarProceso = async (idProceso: string) => {

        const result = await swal({ text: '¿Está seguro que desea cancelar?', buttons: { 
            confirm: { text: "Sí", value: true, visible: true },
            cancel: { text: "No", value: false, visible: true }
        }, icon: 'warning' });

        const proceso = procesos?.find(x => x.idProceso === idProceso);

        if(result && proceso)
        {
            proceso.estado = "CANCELANDO";
            setProcesos([...procesos]);
            apiProductos.cancelarProcesoGeneracionReporte(idProceso)
            .then((response: any) => {
                if (!response?.isSuccess) {
                    toast(response.returnMessage || 'fallo al cancelar', {type: 'error', hideProgressBar: true});
                }
            }).catch(e => {
                toast('fallo al cancelar', {type: 'error', hideProgressBar: true});
            }).finally(() => consultarProcesos());
        }
    }
    const eliminarProceso = async (idProceso: string) => {

        const result = await swal({ text: '¿Está seguro que desea eliminar?', buttons: { 
            confirm: { text: "Sí", value: true, visible: true },
            cancel: { text: "No", value: false, visible: true }
        }, icon: 'warning' });

        const proceso = procesos?.find(x => x.idProceso === idProceso);

        if(result && proceso)
        {
            proceso.estado = "ELIMINANDO";
            setProcesos([...procesos]);

            apiProductos.eliminarProcesoGeneracionReporte(idProceso)
            .then((response: any) => {
                if (!response?.isSuccess) {
                    toast(response.returnMessage || 'fallo al eliminar', {type: 'error', hideProgressBar: true});
                }
            }).catch(e => {
                toast('fallo al eliminar', {type: 'error', hideProgressBar: true});
            }).finally(() => consultarProcesos());
        }
    }

    const descargarReporte = (proceso: any) => {
        toast('Se ha iniciado la descarga, espere mientras finaliza ...', {type: 'success', hideProgressBar: true});

        apiProductos.generarTokenReporteMasivo(proceso.idProceso)
            .then(response => {
                
                if (response?.isSuccess && response?.data) {
                    const url = apiProductos.obtenerUriReporteMasivo(proceso.idProceso, response?.data);
                    window.open(url);
                }
                
            }).catch(e => {
                toast('fallo al descargar', {type: 'error', hideProgressBar: true});
            });
    }

    const mensajeEstado = (estado: string, progreso = 0) => {
        switch(estado) {
            case 'PENDIENTE':   return progreso ? 'Se esta generando el reporte ...' : 'Espere mientras se genera el reporte ...';
            case 'COMPLETADO':  return 'Reporte generado';
            case 'FALLIDO':     return 'Ocurrio un error al generar el reporte ...';
            case 'CANCELANDO':  return 'Cancelando ...';
            case 'ELIMINANDO':  return 'Eliminando ...';
            default: return '';
        }
    }

    return (
        <div className={classes.procesoReportesContenedor}>
            {
                procesos.map((proceso: any) => 
                    <div style={{display: 'flex', columnGap: '12px', marginTop: '1rem'}} key={proceso.idProceso}>
                        <div>
                            { proceso.estado === "COMPLETADO" &&
                                <Tooltip title="Descargar">
                                     <IconButton onClick={() => descargarReporte(proceso)}>
                                        <CloudDownloadOutlined/>
                                    </IconButton>
                                </Tooltip>
                            }
                            { (proceso.estado !== "PENDIENTE") &&
                                <Tooltip title="Eliminar">
                                     <IconButton onClick={() => eliminarProceso(proceso.idProceso)} 
                                            disabled={["ELIMINANDO", "CANCELANDO"].includes(proceso.estado) }>
                                        <DeleteOutlined/>
                                    </IconButton>
                                </Tooltip>
                            }
                            { proceso.estado === "PENDIENTE" &&
                                <Tooltip title="Cancelar">
                                    <IconButton onClick={() => cancelarProceso(proceso.idProceso)} >
                                        <CancelOutlined/>
                                    </IconButton>
                                </Tooltip>

                            }
                        </div>
                        <div style={{flexGrow: '1'}}>
                            <div>
                                <span style={{display: 'block'}}>{proceso.nombreReporte}</span>
                                <span style={{display: 'block'}}>{mensajeEstado(proceso.estado, proceso.progreso)}</span>
                            </div>
                            <div>
                                {(proceso.estado === "PENDIENTE") && <LinearProgress value={proceso.progreso} variant="determinate"/>}
                            </div>
                        </div>
                    </div>
                )
            }
        </div>
    );
});


export default Reports;
