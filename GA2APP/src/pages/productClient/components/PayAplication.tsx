import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Radio,
    RadioGroup,
    FormControl,
    makeStyles,
    createStyles,
    Grid,
    Button,
    Typography,
    MenuItem,
    FormControlLabel,
    Checkbox,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import 'react-datepicker/dist/react-datepicker.css';
import { addDays, addMonths, endOfMonth, startOfMonth, subDays, subMonths } from 'date-fns';
import { AppState } from '../../../store/rootReducer';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import ApiproductoClients from '../../../services/api-producto.service'
import {
    AplicacionPago,
    ProductoDto,
} from '../../../types/producto.model.type';
import { startAplicacionDePago, startGetProductoByIdentification } from '../../../actions/producto.action';
import NotificationService from '../../../services/notification.service';
import UtilService from '../../../utils/util.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import TablaLeasingPlanPago from './TablaLeasingPlanPago';
import TablaCreditoPlanPago from './TablaCreditoPlanPago';
import { ClienteDto } from '../../../types/client.model.type';
import { LoginState } from '../../../reducers/login.reducer';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import ProcedureRequests from '../../ProcedureRequests/ProcedureRequests';
import { CatalogState } from '../../../reducers/catalog.reducer';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import CarteraApi from '../../../services/api-cartera-service';
import CatalogApi from '../../../services/api-catalog.service';
import CreditApi from '../../../services/api-credit.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        btn: {
            margin: '8px ',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        button_left: {
            textAlign: 'left',
        },
        button_right: {
            textAlign: 'right',
        },

        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
    }),
);

const CUOTAS_PENDIENTES_CANCELACION_TOTAL = 0;

const PayAplication = (props: any) => {

    const api = ApiproductoClients()
    const carteraApi = CarteraApi();
    const productClient = ApiproductoClients();
    const creditApi = CreditApi();

    const classes = useStyles();
    const {
        productosSelec,
        idType,
        idNumber,
        clienteRequest,
        loginState,
        catalogState,
        productoState,
        startGetProductoByIdentification
    } = props;

    const { control, setValue } = useForm<AplicacionPago>({});


    const formularioPay = useForm({
        'mode': 'onChange',
        defaultValues: {
            valorPago: '',
            fechaAplicacion: new Date(),
            fechaEjecucion: new Date(),
            alternativas: '',
            valorPagoCuota: '',
            ncuotas: 1,
            aplicaPago: false,
            validaMovContable: false,
            productoCliente: [],
            aplicacionPago: [],
            aplazarDisminucionCuota: false,
            incluyeMora: false,
            tipoRecurso: ''
        }
    })

    const producto = productoState.productos[0];

    const alternativoSeleccionado = formularioPay.watch('alternativas');
    const valorPagoWatch = formularioPay.watch('valorPago');

    const validarAplicacionPagoWatch: any = formularioPay.getValues('aplicacionPago');

    const tipoRecursoSeleccionado = formularioPay.watch('tipoRecurso');

    const tieneTipoRecurso =
        tipoRecursoSeleccionado === 'RECURSOS_PROPIOS' ||
        tipoRecursoSeleccionado === 'CUENTA_INDIVIDUAL';

    const valorCuotaBaseWatch = UtilService.cleanCurrencyValue(
        validarAplicacionPagoWatch?.valorPagoCuota ?? 0
    );


    const valorPagoActualWatch =
        typeof valorPagoWatch === 'number'
            ? valorPagoWatch
            : UtilService.cleanCurrencyValue(valorPagoWatch);

    const puedeIncluirMora =
        producto?.estadoCredito === 'EN MORA' &&
        alternativoSeleccionado === 'PAGO_CUOTA_CREDITO' &&
        Math.abs(valorPagoActualWatch - valorCuotaBaseWatch) <= 1;

    // const productos = productoState.productos.find((objeto:any) => objeto.idCredito === productosSelec.idCredito);
    const startOfCurrentMonth = startOfMonth(subMonths(new Date(), 7));
    const today = new Date();

    const [ocultarNCuota, setOcultarNCuota] = useState<boolean>(false)
    const [mostrarInfoAplicacionPago, setMostrarInfoAplicacionPago] = useState(false);
    const [listadoALternativos, setListadoALternativos] = useState<any[]>([]);
    const [cambioValor, setCambioValor] = useState<any>('')
    const [cantidadCuota, setCantidadCuota] = useState<any>(1)
    const [editable, setEdtitable] = useState<boolean>(true)
    const [selectedDate, setSelectedDate] = useState(new Date() || null);
    const [fechaSelec, setFechaSelec] = useState(new Date() || null);
    const [bloquearFecha, setBloquearFecha] = useState(true);
    const [isLoading, setIsLoading] = useState(false);
    const [visiblePlanPagos, setVisiblePlanPagos] = useState(false);
    const [mostarReversarPago, setMostarReversarPago] = useState(false);
    const [tituloPagoSimulacion, setTituloPagoSimulacion] = useState("Información de pago");
    const [idAplicacionPago, setIdAplicacionPago] = useState("");
    const [cuotasAdelantadas, setCuotasAdelantadas] = useState<any[]>([]);
    const [responseAplicacionPago, setResponseAplicacionPago] = useState<any[]>([]);
    const [requestAplicacionPago, setRequestAplicacionPago] = useState<any[]>([]);
    const [simulacionPago, setSimulacionPago] = useState(false);
    const [datosComprobante, setDatosComprobante] = useState<any>();
    const [fechaUltimoCierreMensual, setFechaUltimoCierreMensual] = useState<any>();

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const [tiposIdentificacion, setTiposIdentificacion] = useState<any[]>([]);
    const [notaListaRestrictiva, setNotaListaRestrictiva] = useState('');

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

    const consultarListaRestrictiva = async (codigoTipoDocumento: string) => {
        try {
            const respuesta = await creditApi.getValidacionActualizacionVigia(
                idNumber,
                codigoTipoDocumento
            );

            const risk = respuesta?.data?.riskLAFT;

            if (risk === 'ALTO') {
                swal({
                    title: 'Alerta',
                    text: `Afiliado con riesgo ${risk}, por favor validar pertinencia.`,
                    icon: 'warning',
                    buttons: {
                        confirm: {
                            text: 'Aceptar',
                        },
                    },
                });
            } else if (risk === 'BAJO' || risk === 'MEDIO') {
                swal({
                    title: 'Alerta',
                    text: `Afiliado con riesgo ${risk}, por favor validar pertinencia.`,
                    icon: 'warning',
                    buttons: {
                        confirm: {
                            text: 'Aceptar',
                        },
                    },
                });
            } else {
                setNotaListaRestrictiva('');
            }
        } catch (error) {
            console.error(error);
            setNotaListaRestrictiva('');
        }
    };

    useEffect(() => {
        const cargarTipos = async () => {
            const response = await catalogApi.getAllTypeDocuments();

            if (response.isSuccess) {
                setTiposIdentificacion(response.data);
            }
        };

        cargarTipos();
    }, []);

    useEffect(() => {

        if (!idNumber || !idType) return;
        if (!tiposIdentificacion.length) return;

        const tipoDocumento = tiposIdentificacion.find(
            x => x.id === Number(idType)
        );

        if (!tipoDocumento) return;

        consultarListaRestrictiva(tipoDocumento.codigo);

    }, [idNumber, idType, tiposIdentificacion]);

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
                        setLocalIpAddress((prevIp) => {
                            if (prevIp !== ip) {
                                enviarIp(ip);
                                return ip;
                            }
                            return prevIp;
                        });
                    }
                }
            };
            return () => peerConnection.close();
        };

        fetchLocalIPAddress();
    }, []);


    useEffect(() => {

        formularioPay.setValue('productoCliente', productoState.productos[0])
        obtenerCuotasAdelantadas();
        validarpagoAplicacionDos();
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Consulta de cartera',
            hostname: document.location.host
        } as Auditoria
        requestAuditoria.tipoAccion = 'CONSULTAS';
        requestAuditoria.idTipoAccion = 3;
        requestAuditoria.descripcion = `Consultó pestaña Aplicación de pago en botón Detalle  parámetros:            
        \n Identificación cliente:  ${clienteRequest.identificacion}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
    }, []);

    useEffect(() => {
        obtenerAlternativos()
    }, [cambioValor]);

    useEffect(() => {
        carteraApi
            .getUltimosCierres()
            .then(async (response: any) => {
                if (response) {
                    if (response.data != null && response.isSuccess) {
                        const fechaUltimoCierre: any = response.data.filter((value: any) => value.idTipoCierre === 2);

                        if (fechaUltimoCierre.length > 0) {
                            // Suponiendo que la fecha sea un string en formato ISO y necesitas convertirla a un objeto Date
                            const fechaOriginal = new Date(fechaUltimoCierre[0].fechaCierre); // Asegúrate de que 'fecha' sea el campo correcto
                            const nuevaFecha = endOfMonth(addMonths(fechaOriginal, 1)); // Suma un mes y toma el último día del mes
                            setFechaUltimoCierreMensual(nuevaFecha);
                            formularioPay.setValue('fechaAplicacion', nuevaFecha);
                            formularioPay.setValue('fechaEjecucion', nuevaFecha);

                        } else {
                            toast(`No se encontraron cierres de cartera`, { type: toast.TYPE.WARNING });
                        }
                    }
                }
            })
    }, []);

    useEffect(() => {
        formularioPay.setValue('productoCliente', productoState.productos[0])
        obtenerAlternativos()

    }, [productoState]);

    useEffect(() => {
        const validarAplicacionPago: any = formularioPay.getValues('aplicacionPago')
        formularioPay.setValue('valorPago', UtilService.formatCurrencyDos(validarAplicacionPago.valorPagoCuota * cantidadCuota))
    }, [cantidadCuota]);

    const obtenerMesAnio = (fecha: string | Date): string => {
        const date = new Date(fecha);
        const mes = (date.getMonth() + 1).toString().padStart(2, '0'); // Asegura dos dígitos en el mes
        return `${date.getFullYear()}-${mes}`; // Formato YYYY-MM
    };

    const obtenerMesSiguiente = (fecha: string | Date): string => {
        const date = new Date(fecha);
        date.setMonth(date.getMonth() + 1); // Sumar un mes
        const mes = (date.getMonth() + 1).toString().padStart(2, '0');
        return `${date.getFullYear()}-${mes}`;
    };

    const obtenerCuotasAdelantadas = () => {
        const validaProductoCliente: any = productoState.productos[0]

        api.getCuotasAdelantadas(validaProductoCliente.idCredito)
            .then(response => {
                if (response.isSuccess) {
                    setCuotasAdelantadas(response.data);
                }
            })
            .catch((_) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const CrearMovContableSeven = async (data: any, dataPago: any) => {

        const validaProductoCliente: any = productoState.productos[0]


        let fechaDePago;
        if (dataPago.fechaPago.includes('0001-01-01T00:00:00')) {
            fechaDePago = moment(new Date()).format();
        } else {
            fechaDePago = dataPago.fechaPago;
        }

        const request = {
            aplicaSimulacion: data.AplicaSimulacion,
            numeroCredito: validaProductoCliente.numeroCredito,
            tipoIdentificacion: Number(validaProductoCliente.tipoIdentificacion),
            numeroDocumentoCliente: validaProductoCliente.identificacion,
            cpgid: dataPago.idAplicacionPago,
            codigo: data.codigo,
            nCuota: data.nCuota,
            tipoCredito: validaProductoCliente.idTipoCredito,
            fechaPago: fechaDePago,
            tipoRecurso: data.tipoRecurso,
        }

        await api
            .postCreaSCnMcontCrearCuotaCartera(request)
            .then(async response => {
                if (response.isSuccess) {
                    setDatosComprobante(response.data)
                    toast(response.returnMessage, { type: toast.TYPE.SUCCESS, autoClose: 10000 });

                } else {
                    toast('Error al crear comprobante de pago', { type: toast.TYPE.ERROR, autoClose: 10000 });
                }

            })
            .catch((error) => {
                if (error.response && error.response.data) {
                    toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000 });
                } else {
                    toast(`Error al crear comprobante de pago: ${error}`, { type: toast.TYPE.ERROR });
                }
            }).finally(() => setIsLoading(false));
    };



    const validarpagoAplicacionDos = () => {
        const request = {
            nro_Credito: productoState.productos[0].numeroCredito,
            tcr_Id: productoState.productos[0].idTipoCredito
        }
        api
            .getValidarAplicacionPago(request)
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {

                        formularioPay.setValue('aplicacionPago', response.data)
                        obtenerAlternativos()

                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });

    }

    const AplicarPago = (request: any) => {
        setIsLoading(true);
        // validarpagoAplicacion();
        // formularioPay.setValue('incluyeMora', false);
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Aplicación de pagos',
            hostname: document.location.host
        } as Auditoria
        api.aplicacionDePago(request)
            .then(async (response) => {

                if (response) {
                    if (response.isSuccess) {

                        await setAplicacion(response.data);
                        await CrearMovContableSeven(request, response.data)
                        setIdAplicacionPago(response.data.idAplicacionPago)
                        setMostarReversarPago(true)
                        setMostrarInfoAplicacionPago(true);
                        setTituloPagoSimulacion("Información de pago ")
                        setIdAplicacionPago(response.data.idAplicacionPago)
                        setResponseAplicacionPago(response.data)
                        setRequestAplicacionPago(request)

                        if (formularioPay.getValues('validaMovContable')) {

                            await NotificationService.genericAlert(
                                'Creado con éxito ',
                                'Se creo aplicación de pagos',
                            );
                            requestAuditoria.tipoAccion = 'TRANSACCIONES';
                            requestAuditoria.idTipoAccion = 2;
                            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
                        \n Aplica Simulación: ${request.AplicaSimulacion ? 'Sí' : 'No'},
                        \n Número Crédito: ${request.numeroCredito},
                        \n Valor Pago: ${UtilService.cleanCurrencyValue(request.valorPago)},
                        \n Identificación: ${request.identificacion},
                        \n Fecha Pago Cuota: ${moment(request.fecha_Pago_Cuota).format('DD-MM-YYYY HH:mm')},  
                        \n Fecha Pago: ${moment(request.fecha_Pago).format('DD-MM-YYYY HH:mm')},
                        \n Código: ${request.codigo},
                        \n Cuota: ${request.nCuota}`
                            auditoriaApi.InsertAuditoria(requestAuditoria);
                        }

                    } else {

                        NotificationService.genericAlert(
                            'Error en aplicación de pagos',
                            `${response.returnMessage}`,
                            'warning',
                        );
                        setMostrarInfoAplicacionPago(false);
                        const error = 'Error en Simulación de pagos'
                        requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
                        \n Aplica Simulación: ${request.AplicaSimulacion ? 'Sí' : 'No'},
                        \n Número Crédito: ${request.numeroCredito},
                        \n Valor Pago: ${UtilService.cleanCurrencyValue(request.valorPago)},
                        \n Identificación: ${request.identificacion},
                        \n Fecha Pago Cuota: ${moment(request.fecha_Pago_Cuota).format('DD-MM-YYYY HH:mm')},  
                        \n Fecha Pago: ${moment(request.fecha_Pago).format('DD-MM-YYYY HH:mm')},
                        \n Código: ${request.codigo},
                        \n Cuota: ${request.nCuota}
                    \n Error:  ${error}
                    \n returnApiMessage: ${response.returnMessage}`
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                    startGetProductoByIdentification(productoState.productos[0].tipoIdentificacion
                        , productoState.productos[0].identificacion)
                    validarpagoAplicacionDos();
                }
            }).catch((error: any) => {
                toast(`Error AplicarPago: ${error.response.data.returnMessage}`, { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 5;
                requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
                        \n Aplica Simulación: ${request.AplicaSimulacion ? 'Sí' : 'No'},
                        \n Número Crédito: ${request.numeroCredito},
                        \n Valor Pago: ${UtilService.cleanCurrencyValue(request.valorPago)},
                        \n Identificación: ${request.identificacion},
                        \n Fecha Pago Cuota: ${moment(request.fecha_Pago_Cuota).format('DD-MM-YYYY HH:mm')},  
                        \n Fecha Pago: ${moment(request.fecha_Pago).format('DD-MM-YYYY HH:mm')},
                        \n Código: ${request.codigo},
                        \n Cuota: ${request.nCuota}
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            })
            .finally(() => setIsLoading(false));

    }



    const validarpagoAplicacion = () => {

        const validaProductoCliente: any = productoState.productos[0]
        const validaaplicacionPago: any = formularioPay.getValues('aplicacionPago')

        if (cambioValor === 'ABONO') {
            setEdtitable(true)
            setBloquearFecha(false)
            formularioPay.setValue('valorPago', '0')
            setOcultarNCuota(false)
            formularioPay.setValue('fechaAplicacion', validaProductoCliente?.fechaProximoPago)
            formularioPay.setValue('fechaEjecucion', validaProductoCliente?.fechaProximoPago)

        } else if (validaProductoCliente.deudaActual <= 1 && cambioValor !== 'LIQUIDACION_LEASING' && cambioValor !== 'OPCION_COMPRA'
            && cambioValor !== 'CANCELACION_TOTAL') {
            setEdtitable(false)
            setBloquearFecha(false)
            formularioPay.setValue('valorPago', '0')
            setOcultarNCuota(false)
            formularioPay.setValue('fechaAplicacion', new Date())

        } else if (cambioValor === 'DISMINUYE_OBLIGACION' || cambioValor === 'DISMINUYE_CUOTA') {
            setEdtitable(true)
            setBloquearFecha(false)
            formularioPay.setValue('valorPago', validaProductoCliente.saldoCapital.toFixed(0));
            setOcultarNCuota(false)

        } else if (cambioValor === '') {

            setSelectedDate(validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('fechaAplicacion', validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('valorPago', '0')
            setOcultarNCuota(false)
            setEdtitable(false)
            setBloquearFecha(false)

        } else if (cambioValor === 'OTROS_PAGOS') {

            setSelectedDate(validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('fechaAplicacion', validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('fechaEjecucion', new Date());
            formularioPay.setValue('valorPago', '0');
            formularioPay.setValue('incluyeMora', false);

            setOcultarNCuota(false)
            setEdtitable(true)
            setBloquearFecha(true);




        } else if (cambioValor === 'PAGO_CUOTA_CREDITO') {
            setSelectedDate(validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('fechaAplicacion', validaProductoCliente.fechaProximoPago);
            formularioPay.setValue(
                'valorPago',
                validaProductoCliente.deudaActual <= 1 ? 0 : validaaplicacionPago?.valorPagoCuota.toFixed(0)
            );
            formularioPay.setValue('incluyeMora', false);

            setOcultarNCuota(false);
            setEdtitable(true);
            setBloquearFecha(false);
        }
        else if (cambioValor === 'ABONO_CUOTA') {

            setSelectedDate(validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('fechaAplicacion', validaProductoCliente.fechaProximoPago);
            formularioPay.setValue('valorPago', validaProductoCliente.deudaActual <= 1 ? 0 : (validaaplicacionPago?.valorPagoCuota.toFixed(0)))
            setOcultarNCuota(false)
            setEdtitable(true)
            setBloquearFecha(false)

        } else if (cambioValor === 'DISMINUYE_REMANENTE') {

            setEdtitable(true)
            setOcultarNCuota(false)
            setBloquearFecha(false)
            formularioPay.setValue('fechaAplicacion', new Date())
            formularioPay.setValue('valorPago', validaProductoCliente.deudaActual <= 1 ? 0 : (validaProductoCliente.canonExtraordinarioCorte.toFixed(0)))

        } else if (cambioValor === 'ADELANTO_CUOTA') {
            const ultimaCuotaAdelantada = cuotasAdelantadas.at(0);
            const fechaAplicacionCuota = (ultimaCuotaAdelantada?.fechaAplicacion
                ? moment(ultimaCuotaAdelantada.fechaAplicacion).add(1, "months").endOf("month").startOf("day")
                : validaProductoCliente.fechaProximoPago);

            formularioPay.setValue('ncuotas', 1)
            formularioPay.setValue('fechaAplicacion', fechaAplicacionCuota);
            formularioPay.setValue('valorPago', validaProductoCliente.deudaActual <= 1 ? 0 : (validaaplicacionPago?.valorPagoCuota.toFixed(0)))
            setOcultarNCuota(true)
            setEdtitable(false)
            setBloquearFecha(false)

        } else if (cambioValor === 'CANCELACION_TOTAL') {
            const saldoCapital = limpiarNumero(
                validaProductoCliente?.saldoCapital
            );

            const interesRemanente = limpiarNumero(
                validaProductoCliente?.interesesRemanenteCorte
            );

            const valorCancelacionTotal =
                saldoCapital + interesRemanente;

            formularioPay.setValue(
                'valorPago',
                valorCancelacionTotal.toFixed(0)
            );

            setOcultarNCuota(false);
            setEdtitable(true);
            setBloquearFecha(true);

            formularioPay.setValue(
                'fechaAplicacion',
                fechaUltimoCierreMensual
            );

            formularioPay.setValue(
                'fechaEjecucion',
                fechaUltimoCierreMensual
            );

        } else if (cambioValor === 'LIQUIDACION_LEASING') {
            formularioPay.setValue(
                'valorPago',
                validaProductoCliente.valorDeudaRemanente.toFixed(0)
            );

            setOcultarNCuota(false);
            setEdtitable(false);
            setBloquearFecha(true);

            formularioPay.setValue(
                'fechaAplicacion',
                fechaUltimoCierreMensual
            );

            formularioPay.setValue(
                'fechaEjecucion',
                fechaUltimoCierreMensual
            );

        } else if (cambioValor === 'OPCION_COMPRA') {
            formularioPay.setValue(
                'valorPago',
                validaProductoCliente.saldoCapital.toFixed(0)
            );

            setOcultarNCuota(false);
            setEdtitable(false);
            setBloquearFecha(true);

            formularioPay.setValue(
                'fechaAplicacion',
                fechaUltimoCierreMensual
            );

            formularioPay.setValue(
                'fechaEjecucion',
                fechaUltimoCierreMensual
            );
        }

        if (validaaplicacionPago.mora && cambioValor !== 'PAGO_CUOTA_CREDITO') {
            formularioPay.setValue('valorPago', validaProductoCliente.valorPago <= 1 ? 0 : (validaaplicacionPago.valorPago.toFixed(0)))
            NotificationService.genericAlert(

                'El crédito en mora.',
                ` El valor total para colocarse al día es: ${(validaaplicacionPago.valorPago.toFixed(0))}`,
                'warning',
            );

            setEdtitable(true)
            setOcultarNCuota(false)
            setBloquearFecha(false)
        }
    }

    const obtenerPlanPago = async () => {
        const requestPlanDePago = {
            nro_Credito: productoState.productos[0].numeroCredito,
            tcr_Id: productoState.productos[0].idTipoCredito,
        };

        try {
            const response = await productClient.getPlanDePagoLeasing(requestPlanDePago);

            if (response && response.isSuccess) {
                return response.data?.planPago || [];
            }

            NotificationService.genericAlert(
                'Error en consulta plan de pago',
                `${response?.returnMessage || 'Error desconocido'}`,
                'error',
            );

            return [];
        } catch (error) {
            toast('Error del sistema consultando plan de pago', { type: toast.TYPE.ERROR });
            return [];
        }
    };

    const tienePagoParcialPrevioCuota = async () => {
        const planPago = await obtenerPlanPago();

        return planPago.some((cuota: any) =>
            String(cuota?.estadoCuota || '').trim().toUpperCase() === 'PAGADA PARCIALMENTE'
        );
    };

    const obtenerAlternativos = async () => {
        const opcionesPago = catalogState.Pay;
        const validaProductoCliente: any = productoState.productos[0];
        const validarAplicacionPago: any = formularioPay.getValues('aplicacionPago');
        const request = productoState?.productos[0].idCredito;

        let novedadesCartera = false;

        await productClient
            .getObtenerNovedadesCartera(request)
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {
                        if (
                            response.data &&
                            response.data.length > 0 &&
                            response.data.find((item: any) => item.tipoNovedad === 'ACELERACIÓN')
                        ) {
                            novedadesCartera = true;
                        }
                    }
                }
            })
            .catch(() => {
                toast('Error al consultar novedades', { type: toast.TYPE.ERROR });
            });

        const existePagoParcialPrevio = await tienePagoParcialPrevioCuota();

        const agregarOtrosPagosSiAplica = (codigos: string[]) =>
            existePagoParcialPrevio ? [...codigos, 'OTROS_PAGOS'] : codigos;

        const permitirOtrosPagos = (option: any) =>
            option.codigo !== 'OTROS_PAGOS' || existePagoParcialPrevio;

        const requiereLiquidacionLeasing =
            validaProductoCliente.tipoCredito === 'LEASING HABITACIONAL' &&
            validaProductoCliente.valorDeudaRemanente > 0 &&
            (
                validaProductoCliente.estadoCredito === 'CANCELADO' ||
                validaProductoCliente.estadoCredito === 'FINALIZADO'
            );

        // REGLA PRIORITARIA CANCELACIÓN TOTAL
        // Si las cuotas pendientes son iguales al valor configurado,
        // únicamente se debe mostrar CANCELACION_TOTAL.
        if (
            !requiereLiquidacionLeasing &&
            Number(validaProductoCliente?.plazoActual) ===
            CUOTAS_PENDIENTES_CANCELACION_TOTAL
        ) {
            const opcionCancelacionTotal = opcionesPago.find(
                (option: any) =>
                    option.codigo === 'CANCELACION_TOTAL'
            );

            setListadoALternativos(
                opcionCancelacionTotal
                    ? [opcionCancelacionTotal]
                    : []
            );

            validarpagoAplicacion();

            return;
        }


        if (novedadesCartera) {
            setListadoALternativos([]);
        } else if (
            opcionesPago[0] !== undefined &&
            validaProductoCliente !== undefined &&
            validarAplicacionPago !== undefined
        ) {
            if (validarAplicacionPago?.mora) {
                setListadoALternativos(
                    opcionesPago.filter((option: any) =>
                        agregarOtrosPagosSiAplica([
                            'PAGO_CUOTA_CREDITO',
                            'ABONO',
                        ]).includes(option.codigo)
                    )
                );

            } else if (
                validaProductoCliente.tipoCredito === 'LEASING HABITACIONAL' &&
                validaProductoCliente.valorDeudaRemanente > 0 &&
                (
                    validaProductoCliente.estadoCredito === 'CANCELADO' ||
                    validaProductoCliente.estadoCredito === 'FINALIZADO'
                )
            ) {
                setListadoALternativos(
                    opcionesPago.filter((option: any) =>
                        option?.codigo === 'LIQUIDACION_LEASING'
                    )
                );

            } else if (validaProductoCliente.deudaActual <= 50) {
                if (
                    validaProductoCliente.plazoTotal === validaProductoCliente.numeroCuotasCanceladas &&
                    validaProductoCliente.idEstadoCredito !== 4
                ) {
                    setListadoALternativos(
                        opcionesPago.filter((option: any) =>
                            agregarOtrosPagosSiAplica([
                                'CANCELA_PARCIAL',
                                'CANCELACION_TOTAL',
                            ]).includes(option.codigo)
                        )
                    );
                } else {
                    setListadoALternativos(
                        opcionesPago.filter((option: any) =>
                            agregarOtrosPagosSiAplica([]).includes(option.codigo)
                        )
                    );
                }

            } else if (
                (
                    validaProductoCliente.tipoCredito !== 'LEASING HABITACIONAL' ||
                    validaProductoCliente.idLineaCredito === 3
                ) &&
                validaProductoCliente.deudaActual > 0 &&
                validarAplicacionPago?.mora === false
            ) {
                if (
                    validaProductoCliente.idLineaCredito === 3 &&
                    validaProductoCliente.idEstadoCredito === 1
                ) {
                    if (validaProductoCliente.plazoActual === validaProductoCliente.plazoTotal) {
                        setListadoALternativos(
                            opcionesPago.filter((option: any) =>
                                agregarOtrosPagosSiAplica([
                                    'DISMINUYE_OBLIGACION',
                                    'DISMINUYE_CUOTA',
                                    'ADELANTO_CUOTA',
                                    'PAGO_CUOTA_CREDITO',
                                    'CANCELACION_TOTAL',
                                    'ABONO',
                                ]).includes(option.codigo)
                            )
                        );
                    } else {
                        setListadoALternativos(
                            opcionesPago.filter((option: any) =>
                                agregarOtrosPagosSiAplica([
                                    'DISMINUYE_OBLIGACION',
                                    'DISMINUYE_CUOTA',
                                    'ADELANTO_CUOTA',
                                    'PAGO_CUOTA_CREDITO',
                                    'OPCION_COMPRA',
                                    'ABONO',
                                ]).includes(option.codigo)
                            )
                        );
                    }
                } else {
                    setListadoALternativos(
                        opcionesPago.filter((option: any) =>
                            ![
                                'DISMINUYE_REMANENTE',
                                'OPCION_COMPRA',
                                'LIQUIDACION_LEASING',
                                'ABONO_CUOTA',
                            ].includes(option.codigo) &&
                            permitirOtrosPagos(option)
                        )
                    );
                }

            } else if (validaProductoCliente.idLineaCredito === 1) {
                const codigosPermitidos = agregarOtrosPagosSiAplica([
                    'ADELANTO_CUOTA',
                    'PAGO_CUOTA_CREDITO',
                    'DISMINUYE_REMANENTE',
                    'OPCION_COMPRA',
                    'ABONO_CUOTA',
                    'ABONO',
                ]);

                setListadoALternativos(
                    opcionesPago.filter((option: any) =>
                        codigosPermitidos.includes(option.codigo)
                    )
                );

            } else if (
                validaProductoCliente.idLineaCredito === 2 &&
                clienteRequest.numeroCuotas < 168
            ) {
                const codigosPermitidos = agregarOtrosPagosSiAplica([
                    'ADELANTO_CUOTA',
                    'PAGO_CUOTA_CREDITO',
                    'DISMINUYE_OBLIGACION',
                    'DISMINUYE_CUOTA',
                    'OPCION_COMPRA',
                    'ABONO',
                ]);

                setListadoALternativos(
                    opcionesPago.filter((option: any) =>
                        codigosPermitidos.includes(option.codigo)
                    )
                );

            } else if (
                (
                    validaProductoCliente.idLineaCredito === 2 ||
                    validaProductoCliente.idLineaCredito === 4
                ) &&
                validaProductoCliente.canonExtraordinarioCorte >= 1 &&
                validarAplicacionPago.mora === false
            ) {
                const codigosExcluidos = ['DISMINUYE_REMANENTE'];

                setListadoALternativos(
                    opcionesPago.filter((option: any) =>
                        !codigosExcluidos.includes(option.codigo) &&
                        permitirOtrosPagos(option)
                    )
                );

            } else if (
                validaProductoCliente.canonExtraordinarioCorte < 1 &&
                validarAplicacionPago?.mora === false
            ) {
                const codigosExcluidos = [
                    'DISMINUYE_REMANENTE',
                    'OPCION_COMPRA',
                    'ABONO_CUOTA',
                ];

                setListadoALternativos(
                    opcionesPago.filter((option: any) =>
                        !codigosExcluidos.includes(option.codigo) &&
                        permitirOtrosPagos(option)
                    )
                );
            }

            validarpagoAplicacion();
        }
    };

    const limpiarNumero = (valor: any) => {
        if (valor === null || valor === undefined || valor === '') return 0;

        if (typeof valor === 'number') return valor;

        return Number(
            String(valor)
                .replace(/\$/g, '')
                .replace(/\s/g, '')
                .replace(/\./g, '')
                .replace(',', '.')
        );
    };

    const calcularValorConMora = (codigoAlternativo: string, incluyeMora: boolean) => {
        const producto: any = productoState?.productos?.[0];
        const validarAplicacionPago: any = formularioPay.getValues('aplicacionPago');

        const valorCuota = limpiarNumero(validarAplicacionPago?.valorPagoCuota);
        const valorMora = limpiarNumero(producto?.acumuladoInteresMora);

        const debeSumarMora =
            incluyeMora === true &&
            producto?.estadoCredito === 'EN MORA' &&
            codigoAlternativo === 'PAGO_CUOTA_CREDITO';

        const valorFinal: any = debeSumarMora
            ? valorCuota + valorMora
            : valorCuota;

        formularioPay.setValue('valorPago', valorFinal, {
            shouldValidate: true,
            shouldDirty: true,
            shouldTouch: true,
        });

        formularioPay.clearErrors('valorPago');
    };

    const aplicacionPago = async (data: any) => {

        const validaProductoCliente: any = productoState.productos[0];
        const validarAplicacionPago: any = formularioPay.getValues('aplicacionPago');

        if (cambioValor === '') {
            NotificationService.genericAlert(
                'Credito',
                'Seleccione Tipo de Pago',
                'warning',
            );
            return;
        }

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter(
            (x: { brand: string }) =>
                x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge',
        )[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Aplicación de pagos',
            hostname: document.location.host,
        } as Auditoria;

        if (cambioValor === 'ABONO') {
            const valor = UtilService.cleanCurrencyValue(formularioPay.watch('valorPago'));

            if (valor === 0) {
                formularioPay.setError('valorPago', {
                    type: 'focus',
                    message: 'El valor debe ser mayor a cero',
                });
                return;
            }

            formularioPay.clearErrors('valorPago');
        }

        if (data.aplicaPago === true) {
            setVisiblePlanPagos(data.aplicaPago);

            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: UtilService.cleanCurrencyValue(data.valorPago),
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: moment(data.fechaAplicacion).format(),
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                incluyeMora: data.incluyeMora === true,
                tipoRecurso: data.tipoRecurso,
            };

            swal({
                title: `${cambioValor.replace('_', ' ').replace('_', ' ')}`,
                text: '¿Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Si'],
            }).then(async (respuesta) => {
                if (respuesta) {
                    api.aplicacionDePago(request)
                        .then(async (response) => {
                            if (response) {
                                if (response.isSuccess) {
                                    setSimulacionPago(true);

                                    await setAplicacion(response.data);
                                    setValue('fechaAplicacion', response.data.fechaPago);
                                    setValue('fechaPago', response.data.fechaCreacion);
                                    setMostrarInfoAplicacionPago(true);
                                    setMostarReversarPago(false);
                                    setSelectedDate(response.data.fechaPago);
                                    setTituloPagoSimulacion('Información de pago SIMULADA');

                                    await NotificationService.genericAlert(
                                        'Creado con éxito ',
                                        'Se creo Simulación de pagos',
                                    );

                                    requestAuditoria.tipoAccion = 'TRANSACCIONES';
                                    requestAuditoria.idTipoAccion = 2;
                                    requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
                                    \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
                                    \n número Crédito: ${validaProductoCliente.numeroCredito},
                                    \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
                                    \n identificación: ${validaProductoCliente.identificacion},
                                    \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
                                    \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
                                    \n código: ${data.alternativas},
                                    \n Cuota: ${data.ncuotas}`;

                                    auditoriaApi.InsertAuditoria(requestAuditoria);
                                } else {
                                    NotificationService.genericAlert(
                                        'Error en Simulación de pagos',
                                        `${response.returnMessage}`,
                                        'warning',
                                    );

                                    setMostrarInfoAplicacionPago(false);

                                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                                    requestAuditoria.idTipoAccion = 5;
                                    requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
                                    \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
                                    \n número Crédito: ${validaProductoCliente.numeroCredito},
                                    \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
                                    \n identificación: ${validaProductoCliente.identificacion},
                                    \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
                                    \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
                                    \n código: ${data.alternativas},
                                    \n Cuota: ${data.ncuotas}
                                    \n Error: Error en Simulación de pagos
                                    \n returnApiMessage: ${response.returnMessage}`;

                                    auditoriaApi.InsertAuditoria(requestAuditoria);
                                }

                                validarpagoAplicacionDos();
                            }
                        })
                        .catch((error: any) => {
                            toast(`Error: ${error.response?.data?.returnMessage} ${error.message}`, {
                                type: toast.TYPE.ERROR,
                            });

                            requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                            requestAuditoria.idTipoAccion = 5;
                            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
                            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
                            \n número Crédito: ${validaProductoCliente.numeroCredito},
                            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
                            \n identificación: ${validaProductoCliente.identificacion},
                            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
                            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
                            \n código: ${data.alternativas},
                            \n Cuota: ${data.ncuotas}
                            \n Error: ${error}
                            \n returnApiMessage: ${error?.response?.data?.returnMessage}`;

                            auditoriaApi.InsertAuditoria(requestAuditoria);
                        });
                }
            });

            return;
        }

        setVisiblePlanPagos(data.aplicaPago);

        const fechaSeleccionada = new Date(selectedDate);
        setFechaSelec(fechaSeleccionada);

        const valorpagoData =
            typeof data.valorPago === 'number'
                ? data.valorPago
                : UtilService.cleanCurrencyValue(data.valorPago);

        const incluyeMora = data.incluyeMora === true;

        const valorCuotaBase = limpiarNumero(validarAplicacionPago?.valorPagoCuota);
        const valorMora = limpiarNumero(validaProductoCliente?.acumuladoInteresMora);

        const valorPagoCuotaPermitido =
            cambioValor === 'PAGO_CUOTA_CREDITO' &&
                validaProductoCliente?.estadoCredito === 'EN MORA' &&
                incluyeMora
                ? valorCuotaBase + valorMora
                : valorCuotaBase;

        const pagoCuotaValido = Math.abs(valorpagoData - valorPagoCuotaPermitido) <= 1;

        if (cambioValor === 'OTROS_PAGOS') {
            const existePagoParcialPrevio = await tienePagoParcialPrevioCuota();
            if (!existePagoParcialPrevio) {
                NotificationService.genericAlertError(
                    'OTROS PAGOS',
                    'Este alternativo solo se permite cuando ya existe un pago parcial por PAGAR LA CUOTA DEL CRÉDITO.',
                    'warning',
                );
                return;
            }

            if (valorpagoData <= 0) {
                NotificationService.genericAlertError(
                    'VALOR PAGO',
                    'El valor debe ser mayor a cero',
                    'warning',
                );
                return;
            }

            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: valorpagoData,
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: fechaSeleccionada,
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                incluyeMora: false,
                tipoRecurso: data.tipoRecurso
            };

            swal({
                title: 'OTROS PAGOS',
                text: 'Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then((respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });

            return;
        }

        if (
            validarAplicacionPago?.mora &&
            cambioValor !== 'PAGO_CUOTA_CREDITO' &&
            cambioValor !== 'OTROS_PAGOS' &&
            cambioValor !== ''
        ) {
            const msg = 'Debe estar al día para realizar este pago';

            NotificationService.genericAlertError('Credito', msg, 'error');

            requestAuditoria.tipoAccion = 'VALIDACION';
            requestAuditoria.idTipoAccion = 9;
            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
            \n número Crédito: ${validaProductoCliente.numeroCredito},
            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
            \n identificación: ${validaProductoCliente.identificacion},
            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
            \n código: ${data.alternativas},
            \n Cuota: ${data.ncuotas}                        
            \n Validación: ${msg}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
            return;
        }

        if (
            validaProductoCliente.deudaActual <= 1 &&
            cambioValor !== 'LIQUIDACION_LEASING' &&
            cambioValor !== 'OPCION_COMPRA' &&
            cambioValor !== 'CANCELACION_TOTAL'
        ) {
            const msg = 'Crédito Sin Deuda Pendiente.';

            NotificationService.genericAlert('Credito', msg);

            requestAuditoria.tipoAccion = 'VALIDACION';
            requestAuditoria.idTipoAccion = 9;
            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
            \n número Crédito: ${validaProductoCliente.numeroCredito},
            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
            \n identificación: ${validaProductoCliente.identificacion},
            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
            \n código: ${data.alternativas},
            \n Cuota: ${data.ncuotas}                        
            \n Validación: ${msg}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
            return;
        }

        if (
            validaProductoCliente.canonExtraordinarioCorte <= 0 &&
            cambioValor === 'DISMINUYE_REMANENTE'
        ) {
            NotificationService.genericAlert(
                'Credito',
                'Seleccione Alternativo',
                'warning',
            );
            return;
        }

        if (
            (cambioValor === 'PAGO_CUOTA_CREDITO' || cambioValor === '') &&
            validarAplicacionPago?.acumuladosAlDia === 0 &&
            validarAplicacionPago.mora === false &&
            new Date(validaProductoCliente.fechaProximoPago) > new Date(fechaUltimoCierreMensual)
        ) {
            const msg = 'El crédito se encuentra al día.';

            NotificationService.genericAlert('Credito', msg);

            requestAuditoria.tipoAccion = 'VALIDACION';
            requestAuditoria.idTipoAccion = 9;
            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
            \n número Crédito: ${validaProductoCliente.numeroCredito},
            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
            \n identificación: ${validaProductoCliente.identificacion},
            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
            \n código: ${data.alternativas},
            \n Cuota: ${data.ncuotas}                        
            \n Validación: ${msg}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
            return;
        }

        if (
            valorpagoData > validaProductoCliente.canonExtraordinarioCorte &&
            cambioValor === 'DISMINUYE_REMANENTE'
        ) {
            const msg = 'Excede Monto Saldo Remanente';

            NotificationService.genericAlertError('VALOR PAGO', msg, 'warning');

            requestAuditoria.tipoAccion = 'VALIDACION';
            requestAuditoria.idTipoAccion = 9;
            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
            \n número Crédito: ${validaProductoCliente.numeroCredito},
            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
            \n identificación: ${validaProductoCliente.identificacion},
            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
            \n código: ${data.alternativas},
            \n Cuota: ${data.ncuotas}                        
            \n Validación: ${msg}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
            return;
        }

        if (
            valorpagoData > validaProductoCliente.deudaActual &&
            (cambioValor === 'DISMINUYE_OBLIGACION' || cambioValor === 'DISMINUYE_CUOTA')
        ) {
            const msg = 'Excede Monto Saldo Total';

            NotificationService.genericAlertError('VALOR PAGO', msg, 'warning');

            requestAuditoria.tipoAccion = 'VALIDACION';
            requestAuditoria.idTipoAccion = 9;
            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
            \n número Crédito: ${validaProductoCliente.numeroCredito},
            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
            \n identificación: ${validaProductoCliente.identificacion},
            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
            \n código: ${data.alternativas},
            \n Cuota: ${data.ncuotas}                        
            \n Validación: ${msg}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
            return;
        }

        if (valorpagoData > validaProductoCliente.deudaActual && cambioValor === 'ADELANTO_CUOTA') {
            const msg = 'Excede Monto Saldo Total';

            NotificationService.genericAlertError('VALOR PAGO', msg, 'warning');

            requestAuditoria.tipoAccion = 'VALIDACION';
            requestAuditoria.idTipoAccion = 9;
            requestAuditoria.descripcion = `Simulación de aplicación de pagos parámetros:
            \n Aplica Simulación: ${data.aplicaPago ? 'Sí' : 'No'},
            \n número Crédito: ${validaProductoCliente.numeroCredito},
            \n valor Pago: ${UtilService.cleanCurrencyValue(data.valorPago)},
            \n identificación: ${validaProductoCliente.identificacion},
            \n fecha Pago Cuota: ${moment(data.fechaAplicacion).format('DD-MM-YYYY HH:mm')},  
            \n fecha Pago: ${moment(data.fechaEjecucion).format('DD-MM-YYYY HH:mm')},
            \n código: ${data.alternativas},
            \n Cuota: ${data.ncuotas}                        
            \n Validación: ${msg}`;

            await auditoriaApi.InsertAuditoria(requestAuditoria);
            return;
        }

        if (
            (valorpagoData >= validarAplicacionPago?.valorPago ||
                valorpagoData <= validaProductoCliente.deudaActual ||
                valorpagoData <= validaProductoCliente.canonExtraordinarioCorte) &&
            cambioValor !== 'PAGO_CUOTA_CREDITO' &&
            cambioValor !== 'LIQUIDACION_LEASING' &&
            cambioValor !== 'OPCION_COMPRA' &&
            cambioValor !== ''
        ) {
            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: UtilService.cleanCurrencyValue(data.valorPago),
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: moment(data.fechaAplicacion).format(),
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                aplazarDisminucionCuota: data.aplazarDisminucionCuota,
                tipoRecurso: data.tipoRecurso
            };

            swal({
                title: `${cambioValor.replace('_', ' ')}`,
                text: '¿Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then(async (respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });

            return;
        }

        if (
            pagoCuotaValido &&
            cambioValor === 'PAGO_CUOTA_CREDITO' &&
            (validarAplicacionPago?.acumuladosAlDia !== 0 ||
                validarAplicacionPago?.mora === true ||
                new Date(validaProductoCliente.fechaProximoPago) <= new Date(fechaUltimoCierreMensual))
        ) {
            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: valorpagoData,
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: fechaSeleccionada,
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                incluyeMora: data.incluyeMora === true,
                tipoRecurso: data.tipoRecurso
            };

            swal({
                title: 'PAGO DE CUOTA',
                text: 'Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then((respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });

            return;
        }

        if (
            validarAplicacionPago?.acumuladosAlDia > 0 &&
            validarAplicacionPago?.mora === true &&
            validarAplicacionPago?.valorPagoCuota === 0
        ) {
            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: UtilService.cleanCurrencyValue(data.valorPago),
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: fechaSeleccionada,
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                incluyeMora: data.incluyeMora === true,
            };

            swal({
                title: 'PAGO DE CUOTA',
                text: 'Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then((respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });

            return;
        }

        if (cambioValor === 'LIQUIDACION_LEASING') {
            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: UtilService.cleanCurrencyValue(data.valorPago),
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: fechaSeleccionada,
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
            };

            swal({
                title: 'LIQUIDACIÓN DE LEASING',
                text: 'Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then((respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });

            return;
        }

        if (cambioValor === 'OPCION_COMPRA') {
            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: UtilService.cleanCurrencyValue(data.valorPago),
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: fechaSeleccionada,
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                tipoRecurso: data.tipoRecurso,
            };

            swal({
                title: 'OPCIÓN DE COMPRA',
                text: 'Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then((respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });

            return;
        }

        if (cambioValor === 'PAGO_CUOTA_CREDITO') {
            const valorPermitido = incluyeMora
                ? valorCuotaBase + valorMora
                : valorCuotaBase;

            if (valorpagoData <= 0) {
                NotificationService.genericAlertError(
                    'VALOR PAGO',
                    'El valor debe ser mayor a cero',
                    'warning',
                );
                return;
            }

            if (valorpagoData > valorPermitido) {
                NotificationService.genericAlertError(
                    incluyeMora
                        ? 'El valor abonado no debe superar la cuota mensual más la mora'
                        : 'El valor abonado no debe superar la cuota mensual',
                    '',
                );
                return;
            }

            const request = {
                AplicaSimulacion: data.aplicaPago,
                numeroCredito: validaProductoCliente.numeroCredito,
                valorPago: valorpagoData,
                identificacion: validaProductoCliente.identificacion,
                fecha_Pago_Cuota: fechaSeleccionada,
                fecha_Pago: moment(data.fechaEjecucion).format(),
                codigo: data.alternativas,
                nCuota: data.ncuotas,
                tcr_Id: productoState.productos[0].idTipoCredito,
                incluyeMora: data.incluyeMora === true,
            };

            swal({
                title: 'PAGO DE CUOTA',
                text: 'Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            }).then((respuesta) => {
                if (respuesta) {
                    AplicarPago(request);
                }
            });


        }
    };

    const setAplicacion = (pagos: AplicacionPago) => {

        setValue('abonoSeguroHogar', pagos.abonoSeguroHogar);
        setValue('abonoSeguroVida', pagos.abonoSeguroVida);
        setValue('abonoInteresCorriente', pagos.abonoInteresCorriente);
        setValue('abonoInteresMora', pagos.abonoInteresMora);
        setValue('abonoCapital', pagos.abonoCapital);
        setValue('valorPago', pagos.valorPago);
        setValue('valorPagoCuota', pagos.valorPagoCuota);
        setValue('capitalRemanente', pagos.capitalRemanente);
        setValue('interesRemanente', pagos.interesRemanente);
        setValue('totalDeudaRemanente', pagos.totalDeudaRemanente);
        setValue('valorCanonExtraordinario', pagos.valorCanonExtraordinario)
    };

    const handleReversarPago = async () => {

        const requestReversionPago = {
            IdPago: idAplicacionPago,
            Estado_Id: datosComprobante?.estado_Id,
            Comprobante_Id: datosComprobante?.comprobante_Id
        }
        swal({
            title: 'Reversar',
            text: '¿Está seguro de reversar el pago?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
        }).then(respuesta => {
            if (respuesta) {
                api.reversarPago(requestReversionPago)
                    .then(async (response) => {
                        if (response) {
                            if (response.isSuccess) {
                                setMostarReversarPago(false)
                                startGetProductoByIdentification(productoState.productos[0].tipoIdentificacion
                                    , productoState.productos[0].identificacion)
                                validarpagoAplicacionDos()
                                setMostrarInfoAplicacionPago(false);

                                await NotificationService.genericAlert(
                                    'Reversión',
                                    'La reversión se realizó correctamente',
                                );

                            } else {
                                setMostarReversarPago(true)
                                NotificationService.genericAlert(
                                    'Error en el proceso de reversión',
                                    `${response.returnMessage}`,
                                    'warning',
                                )
                            }
                        }
                    }).catch(() => {
                        toast("Error del sistema", { type: toast.TYPE.ERROR });
                    });
            }
        });
    };

    return (
        <CHPrivateContainer>
            <LoadingV2Component key="loader" loading={isLoading} tittle='Aplicando Pago...' />
            <CHPaper title="Aplicación de pagos">
                <Grid container>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        Número de Crédito: {productoState.productos[0]?.numeroCredito}
                    </Typography>
                </Grid>
                <Grid container>
                    {
                        ocultarNCuota &&
                        <Grid className={classes.column} item xs={3} md={3}>
                            <Controller
                                name="ncuotas"
                                control={formularioPay.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error } }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Numeros de Cuotas"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={e => {

                                            const cantidad = e.target.value
                                            setCantidadCuota(cantidad)

                                            onChange(e);
                                        }}
                                    >
                                        <MenuItem key={1} value={1}>
                                            1-Cuota
                                        </MenuItem>
                                        <MenuItem key={2} value={2}>
                                            2-Cuotas
                                        </MenuItem>
                                        <MenuItem key={3} value={3}>
                                            3-Cuotas
                                        </MenuItem>
                                        <MenuItem key={4} value={4}>
                                            4-Cuotas
                                        </MenuItem>
                                        <MenuItem key={5} value={5}>
                                            5-Cuotas
                                        </MenuItem>
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    }
                    <Grid className={classes.column} item xs={3} md={3}>
                        <Controller
                            name="valorPago"
                            control={formularioPay.control}
                            rules={{
                                required: 'Este campo requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right'
                                        }
                                    }}
                                    isReadOnly={!editable || !tieneTipoRecurso}
                                    label="Valor Pago *"
                                    value={value}
                                    onChange={(e: any) => {
                                        onChange(e);

                                        if (cambioValor === 'PAGO_CUOTA_CREDITO') {
                                            const nuevoValor = limpiarNumero(e.target.value);
                                            const validarAplicacionPago: any =
                                                formularioPay.getValues('aplicacionPago');

                                            const valorCuotaBase =
                                                limpiarNumero(validarAplicacionPago?.valorPagoCuota);

                                            if (nuevoValor !== valorCuotaBase) {
                                                formularioPay.setValue('incluyeMora', false);
                                            }
                                        }
                                    }}
                                    currencyMask
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    {
                        cambioValor === 'PAGO_CUOTA_CREDITO' ||
                            cambioValor === 'ADELANTO_CUOTA' ||
                            cambioValor === 'OTROS_PAGOS' ||
                            cambioValor === '' ?
                            <Grid className={classes.column} item xs={3} md={3}>
                                <Controller
                                    name="fechaAplicacion"
                                    control={formularioPay.control}
                                    rules={{
                                        required: 'Este campo es requerido'
                                    }}
                                    render={({
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePicker
                                            error={!!error}
                                            label="Fecha cuota pagada*"
                                            value={selectedDate}
                                            invalidDateMessage="Fecha no valida"
                                            minDateMessage="La fecha no debe ser antes de la fecha minima"
                                            maxDateMessage="La fecha no debe ser posterior a la fecha maxima"
                                            onChange={(date: any) => {
                                                setSelectedDate(date);
                                                formularioPay.setValue('fechaAplicacion', date);
                                            }}
                                            disabled={!bloquearFecha}
                                        />
                                    )}
                                />
                            </Grid>
                            :
                            <Grid className={classes.column} item xs={3} md={3}>
                                <Controller
                                    name="fechaAplicacion"
                                    control={formularioPay.control}
                                    rules={{
                                        required: 'Este campo es requerido'
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePicker
                                            error={!!error}
                                            label="Fecha extraordinario pago*"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                    }
                    <Grid className={classes.column} item xs={3} md={3}>
                        <Controller
                            name="fechaEjecucion"
                            control={formularioPay.control}
                            rules={{
                                required: 'Este campo es requerido'
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    label="Fecha Pago*"
                                    value={value}
                                    invalidDateMessage='Fecha no valida'
                                    // minDateMessage='La fecha no debe ser anterior a el mes actual' SOLICITUD DE EDGAR PARA PRUEBAS CARTERA 03-08-2026
                                    // maxDateMessage='La fecha no debe ser posterior a la actual' SOLICITUD DE EDGAR PARA PRUEBAS CARTERA 03-08-2026
                                    onChange={onChange}
                                // minDate={startOfCurrentMonth} SOLICITUD DE EDGAR PARA PRUEBAS CARTERA 03-08-2026
                                // maxDate={today} SOLICITUD DE EDGAR PARA PRUEBAS CARTERA 03-08-2026
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={3} md={3}>
                        <Controller
                            name="alternativas"
                            control={formularioPay.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error } }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Alternativas"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={e => {
                                        const codigoAlternativo: any = e.target.value;

                                        setCambioValor(codigoAlternativo);
                                        onChange(e);

                                        const incluyeMora = formularioPay.getValues('incluyeMora');

                                        const permiteMora =
                                            producto?.estadoCredito === 'EN MORA' &&
                                            codigoAlternativo === 'PAGO_CUOTA_CREDITO';

                                        if (!permiteMora) {
                                            formularioPay.setValue('incluyeMora', false);
                                        }

                                        if (codigoAlternativo === 'PAGO_CUOTA_CREDITO') {
                                            calcularValorConMora(
                                                codigoAlternativo,
                                                permiteMora ? incluyeMora : false
                                            );
                                        }
                                    }}
                                >
                                    <MenuItem value="">
                                        Seleccione un alternativo
                                    </MenuItem>
                                    {
                                        listadoALternativos
                                            ?.filter(
                                                (option: any) =>
                                                    !['ABONO', 'DISMINUYE_REMANENTE', 'ADELANTO_CUOTA'].includes(option.codigo)
                                            )
                                            .map((option: any) => (
                                                <MenuItem
                                                    key={option.codigo}
                                                    value={option.codigo}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ))
                                    }
                                </CHSelect>
                            )}
                        />
                    </Grid>


                    {producto.estadoCredito === "EN MORA" ?
                        <Grid className={classes.column} item xs={6} md={3}>
                            <Controller
                                name="incluyeMora"
                                control={formularioPay.control}
                                defaultValue={false}
                                render={({ field: { onChange, value } }) => (
                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={!!value}
                                                disabled={!puedeIncluirMora}
                                                onChange={(e) => {
                                                    const { checked } = e.target;

                                                    onChange(checked);

                                                    calcularValorConMora(
                                                        alternativoSeleccionado,
                                                        checked
                                                    );
                                                }}
                                                color="primary"
                                            />
                                        }
                                        label="usted esta en Mora, ¿Incluir valor de mora?"
                                    />
                                )}
                            />
                        </Grid>
                        : null}
                </Grid>



                {
                    cambioValor === 'DISMINUYE_CUOTA' && (
                        <Grid container item xs={2} md={4}>
                            <Controller
                                name="aplazarDisminucionCuota"
                                control={formularioPay.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        defaultValue='false'
                                        label="¿Aplicar la disminución después de la próxima cuota?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />

                        </Grid>
                    )
                }

                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <Controller
                            name="tipoRecurso"
                            control={formularioPay.control}
                            render={({ field }) => (
                                <FormControl component="fieldset">
                                    <RadioGroup row {...field}>
                                        <FormControlLabel
                                            value="RECURSOS_PROPIOS"
                                            control={<Radio color="primary" />}
                                            label="Recursos propios"
                                        />

                                        <FormControlLabel
                                            value="CUENTA_INDIVIDUAL"
                                            control={<Radio color="primary" />}
                                            label="Cuenta individual"
                                        />
                                    </RadioGroup>
                                </FormControl>
                            )}
                        />
                    </Grid>
                </Grid>

                <Grid container>
                    <Grid className={classes.column} item xs={4} md={4}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            onClick={formularioPay.handleSubmit(
                                aplicacionPago
                            )}
                        >
                            Aplicar
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            {mostrarInfoAplicacionPago ? (
                <CHPaper title={`${tituloPagoSimulacion}`}>
                    <div>
                        <Grid container>
                            <Typography
                                className={classes.titleform}
                                variant="h5"
                                component="h6"
                            >
                                Información
                            </Typography>
                        </Grid>
                        <Grid container>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="fechaPago"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePicker
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            name="fechaPago"
                                            label="Fecha Pago"
                                            onChange={onChange}
                                            value={value}
                                            readOnly
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                {!simulacionPago && (
                                    <Controller
                                        name="fechaAplicacion"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                label="Fecha Aplicación de Pago"
                                                onChange={onChange}
                                                value={fechaSelec}
                                                InputLabelProps={{ shrink: true }}
                                                readOnly
                                            />
                                        )}
                                    />
                                )}
                                {simulacionPago && (
                                    <Controller
                                        name="fechaAplicacion"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                label="Fecha Aplicación de Pago"
                                                onChange={onChange}
                                                value={value}
                                                InputLabelProps={{ shrink: true }}
                                                readOnly
                                            />
                                        )}
                                    />
                                )}
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="abonoSeguroHogar"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly
                                            name="abonoSeguroHogar"
                                            label="Abono Seguro Hogar"
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }
                                            }}
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="abonoSeguroVida"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }
                                            }}
                                            isReadOnly
                                            name="abonoSeguroVida"
                                            label="Abono Seguro Vida"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="abonoInteresCorriente"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }
                                            }}
                                            isReadOnly
                                            label="Interés Corriente"
                                            name="abonoInteresCorriente"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="abonoInteresMora"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            isReadOnly
                                            label="Interés Mora"
                                            name="abonoInteresMora"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="abonoCapital"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            isReadOnly
                                            label="Abono a Capital"
                                            name="abonoCapital"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="valorPago"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            label="Valor Pago"
                                            name="valorPago"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="capitalRemanente"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            label="Capital Remanente "
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="interesRemanente"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            label="Interés Remanente "
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="totalDeudaRemanente"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            label="Total Deuda Remanente"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Controller
                                    name="valorCanonExtraordinario"
                                    control={control}
                                    rules={{
                                        required: 'Este campo requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right'
                                                }

                                            }}
                                            label="Valor Canon Extraordinario"
                                            value={value}
                                            onChange={onChange}
                                            currencyMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            {mostarReversarPago &&
                                <Grid item xs={12} md={3}>
                                    <Button
                                        variant="contained"
                                        type="submit"
                                        onClick={(handleReversarPago)}
                                        style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                                    >
                                        Reversar
                                    </Button>
                                </Grid>
                            }
                        </Grid>
                    </div>
                </CHPaper>
            ) : (
                ''
            )}

        </CHPrivateContainer>
    );
};

const mapStateToProps = (state: AppState) => ({
    productoState: state.producto,
    loginState: state.login,
    catalogState: state.catalog,

});

interface LinkMapDispatchToProps {
    loginState: LoginState;
    producto: ProductoDto;
    idType: any;
    idNumber: any;
    clienteRequest: ClienteDto;
    catalogState: CatalogState;
    productoState: any;
    startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
}

const mapDispatchToProps = {
    startAplicacionDePago,
    startGetProductoByIdentification,
};

type props = LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(PayAplication);