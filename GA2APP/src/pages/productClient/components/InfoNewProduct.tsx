import React, { useEffect, useState } from "react";
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import {
    Grid,
    makeStyles,
    createStyles,
    Typography,
    Button,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { CITYDTO, FORCEDTO, TIPOINMUEBLE } from '../../../types/catalog.type';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import ApiproductoClients from '../../../services/api-producto.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import NotificationService from '../../../services/notification.service';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import AddressComponentV2 from './address.component.v2';
import { Address } from '../../../types/address.type';
import { setFormDataAddres } from './address.component.constants';
import { LoadingV2Component } from '../../../components/loading/loadingV2.component';
import CatalogApi from '../../../services/api-catalog.service';
import { LoginState } from "../../../reducers/login.reducer";
import { CatalogState } from "../../../reducers/catalog.reducer";
import { SimuladorGeneralState } from "../../../reducers/simuladorGeneral.reducer";
import AuthService from "../../../services/auth.service";
import { Auditoria } from "../../../types/Auditoria.model.type";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import CHSelect from "../../../components/ch-select/CHSelect";
import TablaPlanDePagos from "./TablaPlanDePagos";
import FormularioInformacionDelInmueble from "./FormularioInformacionDelInmueble";
import FormularioDatosProducto from "./FormularioDatosProducto";
import FormularioDatosSeguros from "./FormularioDatosSeguros";
import FormularioBeneficiosYAlivios from "./FormularioBeneficiosYAlivios";
import CreditApi from "../../../services/api-credit.service";
import CarteraApi from "../../../services/api-cartera-service";
import TablaPlanPagosCredito from "./TablaPlanPagosCredito";

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginRight: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        }
    }),
);

const calcularEdad = (fechaNacimiento: any, fechaReferencia: Date): number => {
    if (!fechaNacimiento || !fechaReferencia || Number.isNaN(fechaReferencia.getTime())) {
        return 0;
    }

    const birthDate = new Date(fechaNacimiento);
    const referenciaYear = fechaReferencia.getFullYear();
    const birthYear = birthDate.getFullYear();

    if (Number.isNaN(birthDate.getTime()) || birthYear < 1900 || birthYear > referenciaYear) {
        return 0;
    }

    let calculatedAge = referenciaYear - birthYear;
    const cumpleEseAno = new Date(referenciaYear, birthDate.getMonth(), birthDate.getDate());

    if (cumpleEseAno > fechaReferencia) {
        calculatedAge -= 1;
    }

    return calculatedAge;
};

const construirPlanPagoParaEnviar = (planPagoRaw: any[], esLeasingActual: boolean) => {
    if (!planPagoRaw || planPagoRaw.length === 0) return [];

    return planPagoRaw.map((item: any) => {
        const fecha = moment(item.fechaPago).isValid()
            ? moment(item.fechaPago).format()
            : item.fechaPago;

        if (esLeasingActual) {
            return {
                Fecha: fecha,
                Cuota: item.cuota,
                SeguroVida: item.seguroVidaLeasing,
                SeguroHogar: item.seguroHogar,
                CuotaTradicional: item.cuotaUnidadPagadora,
                CuotaUnidadPagadora: item.cuotaUnidadPagadora,
                CapitalPorPagar: item.capitalpagar,
                SaldoCapital: item.saldoLeasing,
                InteresPorPagar: item.interesPorPagar,
                Saldo: item.saldoLeasing,
                Interes: item.interesLeasing,
                Capital: item.capitalLeasing,
                SaldoInicial: item.saldoLeasing,
                DeudaRemanente: item.valorDeudaRemanente,
            };
        }

        return {
            Fecha: fecha,
            Cuota: item.cuota,
            SeguroVida: item.seguroVida,
            SeguroHogar: item.seguroHogar,
            CuotaTradicional: item.cuotaUnidadPagadora,
            CuotaUnidadPagadora: item.cuotaUnidadPagadora,
            CapitalPorPagar: item.capitalPorPagar,
            SaldoCapital: item.saldo,
            InteresPorPagar: item.interesesPorPagar,
            Saldo: item.saldo,
            Interes: item.intereses,
            Capital: item.capital,
            SaldoInicial: item.saldo,
            DeudaRemanente: item.valorDeudaRemanente,
        };
    });
};

function InfoNewProduct({
    dataUsuario,
    isDataLoaded,
    categoriaSalario,
    salarioMinimoAnual,
    setSalarioMinimoAnual,
    loginState,
    catalogState,
    simuladorState,
    starGetCreditLine,
    setReadonlyFinal,
    readonlyFinal,
    nuevosDatos,
    setTituloBoton,
    tituloBoton,
    setMostrarTablaSegunFormulario,
    mostrarTablaSegunFormulario,


}: {
    dataUsuario: any;
    isDataLoaded: boolean;
    categoriaSalario?: number | null;
    salarioMinimoAnual?: any;
    setSalarioMinimoAnual?: React.Dispatch<React.SetStateAction<any>>;
    loginState: LoginState;
    catalogState: CatalogState;
    simuladorState: SimuladorGeneralState;
    starGetCreditLine: (data: number) => void;
    setReadonlyFinal: any;
    readonlyFinal: boolean;
    nuevosDatos: any;
    setTituloBoton: any;
    tituloBoton: any;
    setMostrarTablaSegunFormulario: any;
    mostrarTablaSegunFormulario: any;

}) {

    const auditoriaApi = AuditoriaApi();
    const apiProducto = ApiproductoClients();
    const apiCatalog = CatalogApi();
    const apiCredit = CreditApi();
    const carteraApi = CarteraApi();
    const classes = useStyles();

    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [isLoadingPlanPagos, setIsLoadingPlanPagos] = useState(false);
    const [esLeasing, setEsLeasing] = useState(false);
    const [direccion, setDireccion] = useState('');
    const [ciudades3, setCiudades3] = useState<CITYDTO[]>([]);
    const [tipoInmuebles, setTipoInmuebles] = useState<TIPOINMUEBLE[]>([]);
    const [openDireccionDetalleProducto, setOpenDireccionDetalleProducto] = useState(false);
    const [direccionValue, setDireccionValue] = useState(true);
    const [file, setFile] = useState<File>();
    const [buttonDisable, setButtonDisable] = useState(true);
    const [age, setAge] = useState<number | null>(null);
    const [planPago, setPlanPago] = useState<any[]>([]);
    const [detallesSimulacionTabla, setDetallesSimulacionTabla] = useState<any[]>([]);
    const [viviendasVis, setViviendasVis] = useState<number | null>(null);
    const [valorInmueblePP, setValorInmueblePP] = useState<any>(null);
    const [cambioCiudad, setCambioCiudad] = useState<any>(null);
    const [productoDatos, setProductoDatos] = useState<any>(null);
    const [aseguradoraVida, setAseguradoraVida] = useState('');
    const [idAseguradoraVida, setIdAseguradoraVida] = useState<number | null>(null);
    const [aseguradoras, setAseguradoras] = useState<any[]>([]);
    const [obtenrsocId, setObtenrsocId] = useState('');
    const [obtenerPlanPagosVer, setObtenerPlanPagosVer] = useState<any[]>([]);
    const [bandera, setBandera] = useState(false)
    const [reportRequest, setReportRequest] = useState<any>({ planPago: [] });
    const [datosFinancierosPersonales, setDatosFinancierosPersonales] = useState<any>(null);


    const formualioCartera = useForm({
        defaultValues: {
            identificacion: '',
            nombre: '',
            edad: '',
            fuerza: 0,
            valorInmueblePlanPagos: '',
            valorInmueble: '',
            idTipoVivienda: '',
            idEstadoVivienda: '',
            claseInmueble: '',
            noMatricula: '',
            escritura: '',
            fechaEscritura: new Date(),
            notaria: '',
            linderos: '',
            direccionInmueble: '',
            departamento: '',
            ciudadInmueble: '',
            descripcionInmueble: '',
            numeroCredito: '',
            idTipoCredito: '',
            lineaCredito: '',
            fechaSolicitud: new Date(),
            fechaDesembolso: new Date(),
            valorDesembolso: 0,
            valorCanonInicial: 0,
            tipoAbonoExtra: '',
            valorAbonoExtra: '',
            tasaCreditoEA: '',
            plazoTotal: '',
            tasaSeguroVida: '',
            porcentajeSeguroVida: '',
            porcentajeExtraPrimaSeguroVida: '',
            aseguradoraVida: '',
            tasaSeguroHogar: '',
            saldoCapital: '',
            saldoTotal: '',
            tasaFrech: 0,
            AlivioFrech: '',
            porcentajeCanonInicial: '',
            porcentajeOpcionCompra: 0,
            canonExtraordinario: '',
            saldoCapitalTradicional: '',
            saldoInicialRedicido: '',
            SaldoCuotaTradicional: '',
            valorCuotaTradicional: '',
            Calificacion: '',
            score: '',
            recursosFuturos: 0,
            totalMes: '',
            deducciones: ''
        }
    })

    const FormualrioDireccion = useForm({
        defaultValues: {
            idTipoCalle: '',
            idLetra1: '',
            numeroCalle1: '',
            direccionBis1: '',
            idCardinal1: '',
            numeroCalle2: '',
            idLetra2: '',
            idCardinal2: '',
            numeroCalle3: '',
            idLetra3: '',
            idCardinal3: '',
            direccionComplemento: '',
        }
    })

    const fechaDesembolsoWatch = formualioCartera.watch('fechaDesembolso');

    useEffect(() => {
        const nombreCompleto = [
            dataUsuario?.primerNombre,
            dataUsuario?.segundoNombre,
            dataUsuario?.primerApellido,
            dataUsuario?.segundoApellido,
        ]
            .filter(Boolean)
            .join(' ')
            .trim();

        formualioCartera.setValue('nombre', nombreCompleto);
    }, [dataUsuario]);

    useEffect(() => {
        if (dataUsuario) {
            const fechaReferencia = fechaDesembolsoWatch
                ? new Date(fechaDesembolsoWatch)
                : new Date();

            const edadCalculada = calcularEdad(dataUsuario.fechaNacimiento, fechaReferencia);
            setAge(edadCalculada);
        }
    }, [dataUsuario, fechaDesembolsoWatch]);

    useEffect(() => {
        cargarTipoInmueble();
    }, []);

    useEffect(() => {
        const valorInmueble = Number(
            UtilService.cleanCurrencyValue(
                formualioCartera.getValues('valorInmueblePlanPagos')
            )
        );

        const salarioMinimo = Number(
            salarioMinimoAnual?.smlv_SALARIO ??
            salarioMinimoAnual?.salarioMinimo ??
            salarioMinimoAnual?.valor ??
            salarioMinimoAnual
        );



    }, [valorInmueblePP, salarioMinimoAnual]);

    useEffect(() => {

        ObtenerProductoCreditoCartera()
        obtenerDatosUnos()
        cargarDatosFinancierosPersonales()
    }, [dataUsuario])

    useEffect(() => {
        const cargarAseguradoras = async () => {
            const response = await CreditApi().getObtenerAseguradoras();

            if (response.isSuccess) {
                setAseguradoras(response.data);
            }
        };

        cargarAseguradoras();
    }, []);

    const resetFormularioCartera = () => {
        const currentValues = formualioCartera.getValues();
        formualioCartera.reset({
            identificacion: '',
            nombre: currentValues.nombre,
            edad: '',
            fuerza: 0,
            valorInmueblePlanPagos: '',
            valorInmueble: '',
            idTipoVivienda: '',
            idEstadoVivienda: '',
            claseInmueble: '',
            noMatricula: '',
            escritura: '',
            fechaEscritura: new Date(),
            notaria: '',
            linderos: '',
            direccionInmueble: '',
            departamento: '',
            ciudadInmueble: '',
            descripcionInmueble: '',
            numeroCredito: '',
            idTipoCredito: '',
            lineaCredito: '',
            fechaSolicitud: new Date(),
            fechaDesembolso: new Date(),
            valorDesembolso: 0,
            valorCanonInicial: 0,
            tipoAbonoExtra: '',
            valorAbonoExtra: '',
            tasaCreditoEA: '',
            plazoTotal: '',
            tasaSeguroVida: '',
            porcentajeSeguroVida: '',
            porcentajeExtraPrimaSeguroVida: '',
            aseguradoraVida: '',
            tasaSeguroHogar: '',
            saldoCapital: '',
            saldoTotal: '',
            tasaFrech: 0,
            AlivioFrech: '',
            porcentajeCanonInicial: '',
            porcentajeOpcionCompra: 0,
            canonExtraordinario: '',
            saldoCapitalTradicional: '',
            saldoInicialRedicido: '',
            SaldoCuotaTradicional: '',
            valorCuotaTradicional: '',
            Calificacion: '',
            score: '',
            recursosFuturos: 0,
            totalMes: currentValues.totalMes,
            deducciones: currentValues.deducciones,

        });
    }






    const cargarDatosFinancierosPersonales = async () => {
        try {
            if (!dataUsuario?.identificacion) {
                return null;
            }

            const request = String(dataUsuario.identificacion);

            const response = await apiCredit.getObtenerDatosFinancierosPersonales(request);

            if (response?.isSuccess && response?.data) {
                setDatosFinancierosPersonales(response.data);

                const valorTotalMes = response.data.valorSalario ?? response.data.totalIngresos;

                if (valorTotalMes !== undefined && valorTotalMes !== null) {
                    formualioCartera.setValue('totalMes', valorTotalMes);
                }

                if (response.data.totalEgresos !== undefined && response.data.totalEgresos !== null) {
                    formualioCartera.setValue('deducciones', response.data.totalEgresos);
                }

                return response.data;
            }

            setDatosFinancierosPersonales(null);
            return null;
        } catch (error) {
            console.error('ERROR EN cargarDatosFinancierosPersonales - InfoNewProduct:', error);
            return null;
        }
    };

    const obtenerDatosUnos = async () => {
        try {
            if (!dataUsuario?.identificacion) {
                return;
            }

            const request = String(dataUsuario.identificacion);

            const response = await apiCredit.getObtenerSimulacionPersonales(request);

            if (response?.isSuccess && response?.data) {
                const datos = response.data;

                formualioCartera.setValue('valorInmueblePlanPagos', datos.valorInmueble);
                formualioCartera.setValue('valorInmueble', datos.avaluoInmueble);
                formualioCartera.setValue('direccionInmueble', datos.direccion);
            }
        } catch (error) {
            console.error('ERROR EN obtenerDatosUnos - InfoNewProduct:', error);
            toast('Error consultando datos iniciales de simulación', {
                type: toast.TYPE.ERROR
            });
        }
    };

    const ObtenerProductoCreditoCartera = async () => {

        const request = String(dataUsuario?.identificacion)
        await
            carteraApi
                .postObtenerProductoCreditoCartera(request)
                .then(async (response: any) => {
                    if (response) {
                        setBandera(response?.data?.aprobadoPasoCartera)
                        setProductoDatos(response.data.datosProductoCarteraProducto)
                        setObtenrsocId(response?.data?.smC_ID)
                        if (response.isSuccess) {                        
                            const datosInfoMuebles = response.data.informacionInmueble;
                            setTituloBoton('VER PLAN DE PAGOS')
                            setMostrarTablaSegunFormulario(true)

                            setReadonlyFinal(response?.data?.aprobadoPasoCartera);

                            formualioCartera.setValue(
                                'idTipoVivienda',
                                String(datosInfoMuebles?.idTipoVivienda)
                            );
                            const departamentoCredito = Number(datosInfoMuebles?.dpd_codigo);
                            const ciudadCredito = Number(datosInfoMuebles?.dpc_codigo);

                            filtroCiudadesPorDepartamento(departamentoCredito);
                            filtroTipoViviendaPorCiudad(ciudadCredito);

                            formualioCartera.setValue('departamento', String(departamentoCredito));
                            formualioCartera.setValue('ciudadInmueble', String(ciudadCredito));


                            const datosProductos = response.data.datosProductoCarteraProducto
                            const datosAfiliados = response.data.datosAfiliadoCarteraProducto
                            const { alivioCuotaFrech, alivioTasaFrechPorcentaje } = response.data.beneficiosAliviosCarteraProducto
                            const { seguroVida, porcentajeSeguroVida, porcentajeExtraPrimaSeguroVida, seguroHogar, aseguradoraVida, idAseguradoraVida } = response.data.datosSeguros
                            formualioCartera.setValue('valorInmueble', datosInfoMuebles?.valorAvaluo)
                            formualioCartera.setValue('idTipoVivienda', datosInfoMuebles?.idTipoVivienda)
                            formualioCartera.setValue('claseInmueble', datosInfoMuebles?.claseInmuebleID)
                            formualioCartera.setValue('noMatricula', datosInfoMuebles?.matriculasInmueble[0]?.numeroMatricula)
                            formualioCartera.setValue('notaria', datosInfoMuebles?.notaria)
                            formualioCartera.setValue('escritura', datosInfoMuebles?.escritura)
                            formualioCartera.setValue('linderos', datosInfoMuebles?.linderos)
                            formualioCartera.setValue('descripcionInmueble', datosInfoMuebles?.descripcionInmueble)
                            formualioCartera.setValue('idEstadoVivienda', datosInfoMuebles?.tipoViviendaID)

                            formualioCartera.setValue('fechaEscritura', datosInfoMuebles.fechaEscritura)
                            formualioCartera.setValue('numeroCredito', datosProductos?.numeroCredito)
                            formualioCartera.setValue('idTipoCredito', datosProductos?.tipoCredito)
                            formualioCartera.setValue('valorDesembolso', datosProductos?.valorDesembolso)
                            formualioCartera.setValue('valorCanonInicial', datosProductos?.canonInial)
                            formualioCartera.setValue('porcentajeOpcionCompra', datosProductos?.opcionDeCompra)
                            formualioCartera.setValue('tipoAbonoExtra', datosProductos?.tipoAbonoExtra)
                            formualioCartera.setValue('valorAbonoExtra', datosProductos?.valorAbonoExtra)
                            formualioCartera.setValue('tasaCreditoEA', datosProductos?.tasaCreditoEA)
                            formualioCartera.setValue('score', datosProductos?.score)

                            formualioCartera.setValue('plazoTotal', datosProductos?.plazoTotal)
                            formualioCartera.setValue('lineaCredito', datosProductos?.lineaCredito)
                            formualioCartera.setValue('recursosFuturos', datosAfiliados?.recursosFuturos)
                            // formualioCartera.setValue('fechaDesembolso',)

                            formualioCartera.setValue('fechaSolicitud', datosProductos?.fechaSolicitud);
                            formualioCartera.setValue('fechaDesembolso', datosProductos?.fechaDesembolso);
                            formualioCartera.setValue('Calificacion', 'A')
                            formualioCartera.setValue('tasaSeguroVida', seguroVida)
                            formualioCartera.setValue('porcentajeSeguroVida', porcentajeSeguroVida)
                            formualioCartera.setValue('porcentajeExtraPrimaSeguroVida', porcentajeExtraPrimaSeguroVida)
                            if (response.data.aprobadoPasoCartera) {
                                formualioCartera.setValue(
                                    'aseguradoraVida',
                                    String(idAseguradoraVida ?? '')
                                );
                            } else {
                                formualioCartera.setValue(
                                    'aseguradoraVida',
                                    ''
                                );
                            }
                            formualioCartera.setValue('tasaSeguroHogar', seguroHogar)
                            formualioCartera.setValue('tasaFrech', alivioTasaFrechPorcentaje)

                            formualioCartera.setValue('AlivioFrech', alivioCuotaFrech ? "true" : "false");
                        } else {

                            setTituloBoton('CREAR PLAN DE PAGOS')
                            setMostrarTablaSegunFormulario(false)
                            resetFormularioCartera()
                            FormualrioDireccion.reset()
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    resetFormularioCartera()
                    FormualrioDireccion.reset()
                });
    }

    const filtroTipoViviendaPorCiudad = async (idCiudad: any) => {

        const filtroVivienda = catalogState.City.find((item: any) => item.id === Number(idCiudad));
        if (filtroVivienda?.vis_NoVis) {
            setViviendasVis(1)
        } else {
            setViviendasVis(2)
        }
    };

    const filtroCiudadesPorDepartamento = async (idDepartment: any) => {

        const filtroCity = catalogState.City.filter((item: any) => item.idDepartamento === Number(idDepartment));

        setCiudades3(filtroCity);
    };

    const resetAddress = () => {
        FormualrioDireccion.setValue('idTipoCalle', '');
        FormualrioDireccion.setValue('idLetra1', '');
        FormualrioDireccion.setValue('numeroCalle1', '');
        FormualrioDireccion.setValue('direccionBis1', '');
        FormualrioDireccion.setValue('idCardinal1', '');
        FormualrioDireccion.setValue('numeroCalle2', '');
        FormualrioDireccion.setValue('idLetra2', '');
        FormualrioDireccion.setValue('idCardinal2', '');
        FormualrioDireccion.setValue('numeroCalle3', '');
        FormualrioDireccion.setValue('idLetra3', '');
        FormualrioDireccion.setValue('idCardinal3', '');
        FormualrioDireccion.setValue('direccionComplemento', '');
    };

    const handleOpenAddressDetalleProducto = () => {
        resetAddress();
        setOpenDireccionDetalleProducto(true);
    };

    const handleKeyPressDetalleProducto = (e: any) => {
        if (e.key === 'Enter') {
            handleOpenAddressDetalleProducto();
        }
    };

    const onCloseAddressDetalleProducto = (value: Address) => {
        if (value !== null) {
            const result = setFormDataAddres(value);
            formualioCartera.setValue('direccionInmueble', result);
            setOpenDireccionDetalleProducto(false);
            setDireccionValue(false);
            formualioCartera.clearErrors('direccionInmueble');
        } else {
            formualioCartera.setValue('direccionInmueble', direccion);
            setOpenDireccionDetalleProducto(false);
            setDireccionValue(true);
        }
    };

    const handleFileChosen = (file: any) => {
        if (file !== undefined) {
            setFile(file);
            handleButtonChange(false);
        } else {
            handleButtonChange(true);
        }
    };

    const handleButtonChange = (disabled: boolean) =>

        setButtonDisable(disabled);
        
    const generarPlanPagos = async (data: any) => {
        setIsLoadingPlanPagos(true)

        if (tituloBoton === 'VER PLAN DE PAGOS') {

            const request = {
                smc_id: obtenrsocId,
            };
            await apiCredit
                .getObtenerDatosSimulacion(request)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                            setObtenerPlanPagosVer(response?.data)
                            setIsLoadingPlanPagos(false)
                            setButtonDisable(false)
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingPlanPagos(false)
                });



            return
        }
        
        const datosFinancierosPersonalesActuales = await cargarDatosFinancierosPersonales();

        let idSimulacionActual = '';
        let datosSimulacionPersonalesActual: any = null;

        try {
            const responseSimulacionPersonales = await apiCredit.getObtenerSimulacionPersonales(
                String(dataUsuario?.identificacion || '')
            );

            if (responseSimulacionPersonales?.isSuccess && responseSimulacionPersonales?.data) {
                datosSimulacionPersonalesActual = responseSimulacionPersonales.data;

                if (responseSimulacionPersonales.data.idSimulacion) {
                    idSimulacionActual = responseSimulacionPersonales.data.idSimulacion;
                }
            }
        } catch (error) {
            console.error('Error al disparar getObtenerSimulacionPersonales antes de simular:', error);
        }

        try {
            const codigoTipoViviendaNU = Number(data.idTipoVivienda) === 1 ? 'U' : 'N';
            const tipoViviendaActualizarVivienda = codigoTipoViviendaNU === 'N' ? '3' : '4';

            const requestActualizarVivienda = {
                numeroDocumento: String(dataUsuario?.identificacion || ''),
                valorInmueble: Number(UtilService.cleanCurrencyValue(data.valorInmueblePlanPagos) || 0),
                avaluoInmueble: Number(UtilService.cleanCurrencyValue(data.valorInmueble) || 0),
                tipoVivienda: tipoViviendaActualizarVivienda,
                DpdIdInmueble: String(
                    datosSimulacionPersonalesActual?.dpdIdInmueble ?? data.departamento ?? ''
                ),
                DpcIdInmueble: String(
                    datosSimulacionPersonalesActual?.dpcIdInmueble ?? data.ciudadInmueble ?? ''
                ),
                usr_id: null,
                numeroObligacionCompraCartera: '',
                compraCartera: false,
                simId: esLeasing ? '2' : '1',
            };

            const responseActualizarVivienda = await apiCredit.patchActualizarPersonalesVivienda(requestActualizarVivienda);

            if (!idSimulacionActual && responseActualizarVivienda?.isSuccess && responseActualizarVivienda?.data?.idSimulacion) {
                idSimulacionActual = responseActualizarVivienda.data.idSimulacion;
            }
        } catch (error) {
            console.error('Error al disparar actualizarPersonalesVivienda:', error);
        }

        if (idSimulacionActual) {
            try {
                const requestDatosFinancieros = {
                    idSimulacion: idSimulacionActual,
                    descripcionSalario: 'SUELDO BÁSICO *',
                    valorSalario: datosFinancierosPersonalesActuales?.valorSalario ?? Number(UtilService.cleanCurrencyValue(data.totalMes) || 0),
                    descripcionOtroIng1: 'Seleccione',
                    valorOtrosIngresos1: 0,
                    descripcionOtroIng2: 'Seleccione',
                    valorOtrosIngresos2: 0,
                    descripcionOtroIng3: 'Seleccione',
                    valorOtrosIngresos3: 0,
                    descripcionOtroIng4: 'Seleccione',
                    valorOtrosIngresos4: 0,
                    descripcionOtroIng5: 'Seleccione',
                    valorOtrosIngresos5: 0,
                    valorTotalIngresos: datosFinancierosPersonalesActuales?.totalIngresos ?? Number(UtilService.cleanCurrencyValue(data.totalMes) || 0),
                    valorTotalEgresos: datosFinancierosPersonalesActuales?.totalEgresos ?? Number(UtilService.cleanCurrencyValue(data.deducciones) || 0)
                };

                await apiCredit.postCrearSimulacionDatosFinancieros(requestDatosFinancieros);
            } catch (error) {
                console.error('Error al disparar CrearSimulacionDatosFinancieros:', error);
            }
        } else {
            console.warn('No se disparó CrearSimulacionDatosFinancieros: no se obtuvo un idSimulacion válido.');
        }

        const tipoViviendaSimulacion = Number(data.idTipoVivienda) === 1 ? 'U' : 'N';

        const request = esLeasing
            ? {
                valor: Number(UtilService.cleanCurrencyValue(data.valorInmueblePlanPagos) || 0),
                documento: String(dataUsuario?.identificacion || ''),
                tipoIdentificacion: String(dataUsuario?.idTipoIdentificacion || ''),
                plazo: 0,
                tipoVivienda: tipoViviendaSimulacion,
                canonInicial: String(data.valorCanonInicial ?? '0').replace('%', ''),
                porOpcionCompra: Number(data.porcentajeOpcionCompra || 0),
                simId: '2',
                valorPrestamo: 0,
                valorInicialSimuladoInmueble: 0,
                valorViviendaSeguro: Number(UtilService.cleanCurrencyValue(data.valorInmueble) || 0),
                TasaVoluntariaAportes: false,
                avaluoInmueble: 0,
                tipoSimularOProspectar: 1,
                usr_id: null,
                afl_id: 0,
            }
            : {
                valor: Number(UtilService.cleanCurrencyValue(data.valorInmueblePlanPagos) || 0),
                documento: String(dataUsuario?.identificacion || ''),
                tipoIdentificacion: String(dataUsuario?.idTipoIdentificacion || ''),
                tipoVivienda: tipoViviendaSimulacion,
                tipoAbono: Number(data.tipoAbonoExtra || 0),
                aplicaBeneficio: false,
                abonoUnico: false,
                valorCuota: 0,
                valorPrestamo: Number(UtilService.cleanCurrencyValue(data.valorDesembolso) || 0),
                valorViviendaSeguro: Number(UtilService.cleanCurrencyValue(data.valorInmueble) || 0),
                TasaVoluntariaAportes: false,
                plazo: Number(data.plazoTotal || 0),
                simId: '1',
                avaluoInmueble: 0,
                tipoSimularOProspectar: 0,
                usr_id: null,
                afl_id: 0,
            };

        await
            apiCredit
                .postCreditCreateClientSimulation(request)
                .then(async (response: any) => {
                    setIsLoadingPlanPagos(false)
                    if (response) {
                        if (response.isSuccess) {

                            if (response.data && response.data.length !== 0) {
                                setPlanPago(response.data);
                                handleButtonChange(false);

                                setDetallesSimulacionTabla(response.data)
                                
                                const datosFormulario = formualioCartera.getValues();

                                const requestReporte = {
                                    // Datos afiliado
                                    identificacion: String(dataUsuario.identificacion),
                                    tipoIdentificacion: String(dataUsuario.idTipoIdentificacion),
                                    nombreCliente: datosFormulario.nombre,
                                    edad: Number(age ?? 0),
                                    fuerza: String(dataUsuario.idFuerza),

                                    // Información inmueble
                                    valorVivienda: UtilService.cleanCurrencyValue(datosFormulario.valorInmueblePlanPagos),
                                    valorInmueble: UtilService.cleanCurrencyValue(datosFormulario.valorInmueble),

                                    idTipoVivienda: Number(datosFormulario.idTipoVivienda),
                                    idEstadoVivienda: Number(datosFormulario.idEstadoVivienda),

                                    claseInmueble: String(datosFormulario.claseInmueble),

                                    noMatricula: datosFormulario.noMatricula,
                                    escritura: datosFormulario.escritura,
                                    fechaEscritura: datosFormulario.fechaEscritura,
                                    notaria: datosFormulario.notaria,
                                    linderos: datosFormulario.linderos,
                                    direccionInmueble: datosFormulario.direccionInmueble,

                                    departamento: String(datosFormulario.departamento),
                                    ciudadInmueble: String(datosFormulario.ciudadInmueble),

                                    descripcionInmueble: datosFormulario.descripcionInmueble,

                                    // Datos producto
                                    numeroCredito: datosFormulario.numeroCredito,
                                    idTipoCredito: Number(datosFormulario.idTipoCredito),
                                    lineaCredito: String(datosFormulario.lineaCredito),

                                    fechaSolicitud: datosFormulario.fechaSolicitud,
                                    fechaDesembolso: datosFormulario.fechaDesembolso,

                                    valorDesembolso: UtilService.cleanCurrencyValue(datosFormulario.valorDesembolso),

                                    valorCanonInicial: UtilService.cleanCurrencyValue(datosFormulario.valorCanonInicial),

                                    tipoAbonoExtra: datosFormulario.tipoAbonoExtra,

                                    valorAbonoExtra: UtilService.cleanCurrencyValue(datosFormulario.valorAbonoExtra),

                                    tasaCreditoEA: String(datosFormulario.tasaCreditoEA),

                                    plazoTotal: Number(datosFormulario.plazoTotal),

                                    // Seguros
                                    tasaSeguroVida: String(datosFormulario.tasaSeguroVida),
                                    porcentajeSeguroVida: String(datosFormulario.porcentajeSeguroVida),
                                    porcentajeExtraPrimaSeguroVida: String(datosFormulario.porcentajeExtraPrimaSeguroVida),
                                    tasaSeguroHogar: String(datosFormulario.tasaSeguroHogar),

                                    // Beneficios
                                    saldoCapital: UtilService.cleanCurrencyValue(datosFormulario.saldoCapital),

                                    saldoTotal: UtilService.cleanCurrencyValue(datosFormulario.saldoTotal),

                                    tasaFrech: Number(datosFormulario.tasaFrech),

                                    AlivioFrech: 0.0,

                                    porcentajeCanonInicial: UtilService.cleanCurrencyValue(
                                        datosFormulario.porcentajeCanonInicial
                                    ),

                                    porcentajeOpcionCompra: Number(
                                        datosFormulario.porcentajeOpcionCompra
                                    ),

                                    canonExtraordinario: UtilService.cleanCurrencyValue(
                                        datosFormulario.canonExtraordinario
                                    ),

                                    saldoCapitalTradicional: UtilService.cleanCurrencyValue(
                                        datosFormulario.saldoCapitalTradicional
                                    ),

                                    saldoInicialRedicido: UtilService.cleanCurrencyValue(
                                        datosFormulario.saldoInicialRedicido
                                    ),

                                    SaldoCuotaTradicional: UtilService.cleanCurrencyValue(
                                        datosFormulario.SaldoCuotaTradicional
                                    ),

                                    valorCuotaTradicional: UtilService.cleanCurrencyValue(
                                        datosFormulario.valorCuotaTradicional
                                    ),

                                    Calificacion: datosFormulario.Calificacion,
                                    score: datosFormulario.score,

                                    recursosFuturos: UtilService.cleanCurrencyValue(
                                        datosFormulario.recursosFuturos
                                    ),

                                    // Tabla
                                    planPago: response.data,
                                };

                                setReportRequest(requestReporte);
                            } else {
                                handleButtonChange(true);
                                setDetallesSimulacionTabla([])
                            }
                        } else {
                            toast(`Error: ${response.returnMessage || 'No fue posible crear el plan de pagos'}`, {
                                type: toast.TYPE.ERROR,
                            });
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingPlanPagos(false)
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


    const guardarFormulario = async (data: any) => {

        const respuesta = await swal({
            text: "¿Está seguro que desea agregar el producto?",
            icon: "warning",
            buttons: {
                cancelar: {
                    text: "Cancelar",
                    value: false,
                    visible: true,
                    closeModal: true,
                },
                confirmar: {
                    text: "Confirmar",
                    value: true,
                    visible: true,
                    closeModal: true,
                },
            },
            closeOnClickOutside: false,
        });

        if (!respuesta) {
            return;
        }

        if (!detallesSimulacionTabla || detallesSimulacionTabla.length === 0) {
            toast('Debe crear el plan de pagos antes de agregar el producto.', {
                type: toast.TYPE.ERROR,
            });
            return;
        }

        setIsLoading(true);

        const user = AuthService.getUserInfo();

        const canonExtraordinarios = data.canonExtraordinario
            ? String(data.canonExtraordinario).replace('%', '')
            : 0;

        const canonInicial = data.porcentajeCanonInicial
            ? String(data.porcentajeCanonInicial).replace('%', '')
            : 0;

        const opcionCompra = data.porcentajeOpcionCompra
            ? String(data.porcentajeOpcionCompra).replace('%', '')
            : 0;

        const tasaCreditoEA = String(data.tasaCreditoEA).replace('%', '');

        const request = {
            tipoIdentificacion: String(dataUsuario?.idTipoIdentificacion),
            identificacion: dataUsuario?.identificacion,
            numeroCredito: data.numeroCredito,
            idTipoCredito: data.idTipoCredito,
            fechaSolicitud: data.fechaSolicitud,
            fechaDesembolso: data.fechaDesembolso,
            valorDesembolso: UtilService.cleanCurrencyValue(data.valorDesembolso),
            plazoTotal: data.plazoTotal,
            tasaSeguroVida: data.tasaSeguroVida,
            porcentajeSeguroVida: data.porcentajeSeguroVida,
            porcentajeExtraPrimaSeguroVida: data.porcentajeExtraPrimaSeguroVida,
            tasaSeguroHogar: data.tasaSeguroHogar,
            tasaCreditoEA: tasaCreditoEA,
            score: data.score,
            valorInmueblePlanPagos: UtilService.cleanCurrencyValue(data.valorInmueblePlanPagos),
            ValorViviendaSeguroHogar: UtilService.cleanCurrencyValue(data.valorInmueble),
            idTipoVivienda: data.idTipoVivienda,
            idEstadoVivienda: data.idEstadoVivienda,
            direccionInmueble: data.direccionInmueble,
            noMatricula: data.noMatricula,
            notaria: data.notaria,
            escritura: data.escritura,
            linderos: data.linderos,
            descripcionInmueble: data.descripcionInmueble,
            fechaEscritura: data.fechaEscritura,
            porcentajeCanonInicial: canonInicial,
            porcentajeOpcionCompra: Number(opcionCompra),
            canonExtraordinario: canonExtraordinarios,
            saldoCapitalTradicional: UtilService.cleanCurrencyValue(data.saldoCapitalTradicional),
            saldoInicialRedicido: UtilService.cleanCurrencyValue(data.saldoInicialRedicido),
            valorCuotaTradicional: UtilService.cleanCurrencyValue(data.valorCuotaTradicional),
        };

        const formData = new FormData();

        formData.append('recursosFuturos', String(UtilService.cleanCurrencyValue(data.recursosFuturos) || '0'));
        formData.append('fechaDesembolso', moment(data.fechaDesembolso).format() || '');
        formData.append('valorDesembolso', String(UtilService.cleanCurrencyValue(data.valorDesembolso) || '0'));
        formData.append('plazoTotal', data.plazoTotal || '');
        formData.append('TipoCreditoModalidad', data.idTipoCredito || '');
        formData.append('valorInmueblePlanPagos', String(UtilService.cleanCurrencyValue(data.valorInmueblePlanPagos) || '0'));
        formData.append('ValorViviendaSeguroHogar', String(UtilService.cleanCurrencyValue(data.valorInmueble) || '0'));
        formData.append('ViviendaVis', data.idTipoVivienda || '');
        formData.append('EstadoVivienda', data.idEstadoVivienda || '');
        formData.append('tasaSeguroVida', data.tasaSeguroVida || '');
        formData.append('porcentajeSeguroVida', data.porcentajeSeguroVida || '');
        formData.append('porcentajeExtraPrimaSeguroVida', data.porcentajeExtraPrimaSeguroVida || '');
        formData.append('idAseguradoraVida', data.aseguradoraVida);
        formData.append('TasaSeguroHogar', data.tasaSeguroHogar || '');
        formData.append('tasaEA', String(UtilService.cleanCurrencyValue(data.tasaCreditoEA) || '0'));
        formData.append('tasaFrech', String(UtilService.cleanCurrencyValue(data.tasaFrech) || '0'));
        formData.append('ValorAlivioCuota', String(UtilService.cleanCurrencyValue(data.valorCuotaTradicional) || '0'));
        formData.append('tipoAbonoExtra', data.tipoAbonoExtra || '');
        formData.append('ValorAbonoExtra', String(UtilService.cleanCurrencyValue(data.valorAbonoExtra) || '0'));
        formData.append('PorCanonInicial', String(UtilService.cleanCurrencyValue(data.valorCanonInicial) || '0'));
        formData.append('PorOpcionCompra', String(UtilService.cleanCurrencyValue(data.porcentajeOpcionCompra) || '0'));
        formData.append('AlivioFrech', (data.AlivioFrech === 'S').toString());
        formData.append('CreadoPor', user.USR_ID || '');
        formData.append('numeroCredito', data.numeroCredito || '');
        formData.append('TipoIdentificacion', String(dataUsuario?.idTipoIdentificacion) || '');
        formData.append('NumeroIdentificacion', String(dataUsuario?.identificacion));
        formData.append('direccionInmueble', data.direccionInmueble || '');
        formData.append('CiudadInmueble', data.ciudadInmueble || '');
        formData.append('NumeroMatricula', data.noMatricula || '');
        formData.append('EscrituraInmueble', data.escritura || '');
        formData.append('FechaEscritura', moment(data.fechaEscritura).format() || '');
        formData.append('fechaSolicitud', moment(data.fechaSolicitud).format() || '');
        formData.append('notaria', data.notaria || '');
        formData.append('linderos', data.linderos || '');
        formData.append('descripcionInmueble', data.descripcionInmueble || '');
        formData.append('score', data.score || '');
        formData.append('Calificacion', data.Calificacion || '');
        formData.append('ClaseInmueble', data.claseInmueble || '');
        formData.append('LineaCredito', data.lineaCredito || '');
        formData.append('saldoCapital', '0');
        formData.append('saldoCapitalTradicional', String(UtilService.cleanCurrencyValue(data.saldoCapitalTradicional) || '0'));
        formData.append('SaldoCuotaTradicional', String(UtilService.cleanCurrencyValue(data.SaldoCuotaTradicional) || '0'));

        const planPagoParaEnviar = construirPlanPagoParaEnviar(detallesSimulacionTabla, esLeasing);

        planPagoParaEnviar.forEach((cuota: any, index: number) => {
            Object.entries(cuota).forEach(([key, value]) => {
                formData.append(
                    `PlanPago[${index}].${key}`,
                    value !== null && value !== undefined ? String(value) : ''
                );
            });
        });

        const nav = window.navigator as any;

        const usrAg =
            nav?.userAgentData?.brands?.find(
                (x: { brand: string }) =>
                    x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand || navigator.userAgent;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav?.userAgentData?.platform || navigator.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Agregar producto',
            hostname: document.location.host
        } as Auditoria;

        try {
            const response = await apiProducto.postCrearProductoCliente(formData);

            if (response?.isSuccess) {
                await NotificationService.genericAlert(
                    'Guardado con éxito',
                    'Se guardó el producto correctamente.'
                );

                requestAuditoria.tipoAccion = 'TRANSACCIONES';
                requestAuditoria.idTipoAccion = 2;
                requestAuditoria.descripcion = `Agregar producto parámetros:
                tipoIdentificacion: ${request.tipoIdentificacion},
                identificacion: ${request.identificacion},
                numeroCredito: ${request.numeroCredito},
                idTipoCredito: ${request.idTipoCredito},
                fechaSolicitud: ${request.fechaSolicitud},
                fechaDesembolso: ${request.fechaDesembolso},
                valorDesembolso: ${request.valorDesembolso},
                plazoTotal: ${request.plazoTotal}`;

                try {
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                } catch (errorAuditoria) {
                    console.error('Error guardando auditoría:', errorAuditoria);
                }

                return;
            }

            toast(`Error: ${response?.returnMessage || 'No fue posible agregar el producto'}`, {
                type: toast.TYPE.ERROR,
            });

        } catch (error: any) {
            console.error('Error creando producto:', error);

            toast(
                error?.response?.data?.returnMessage || 'Error al crear el producto',
                { type: toast.TYPE.ERROR }
            );

            requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion = 5;
            requestAuditoria.descripcion = `Error al agregar producto:
            tipoIdentificacion: ${request.tipoIdentificacion},
            identificacion: ${request.identificacion},
            numeroCredito: ${request.numeroCredito}
            ReturnApiMessage: ${error?.response?.data?.returnMessage}`;

            try {
                await auditoriaApi.InsertAuditoria(requestAuditoria);
            } catch (errorAuditoria) {
                console.error('Error guardando auditoría de error:', errorAuditoria);
            }

        } finally {
            setIsLoading(false);
            setIsLocalLoading(false);
        }
    };

    const cargarTipoInmueble = async () => {
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(43)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTipoInmuebles(response.data);

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };



    if (dataUsuario == null && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }
    if (isLocalLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <>
            <LoadingV2Component loading={isLoading} tittle='Creando producto cliente...' />
            <LoadingV2Component loading={isLoadingPlanPagos} tittle='Creando plan de pagos...' />

            <Grid container>

                <Grid className={classes.column} item xs={12} md={12}>
                    <Typography className={classes.titleform} variant="h5" component="h6">Datos Afiliado</Typography>
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="identificacion"
                        control={formualioCartera.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={readonlyFinal}
                                label="Identificación"
                                value={dataUsuario?.identificacion}
                                onChange={onChange}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="nombre"
                        control={formualioCartera.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={readonlyFinal}
                                label="Nombre y apellidos"
                                value={value}
                                onChange={onChange}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="edad"
                        control={formualioCartera.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={readonlyFinal}
                                name="edad"
                                label="Edad"
                                value={age !== null ? age : value}
                                onChange={onChange}
                                InputLabelProps={{ shrink: true }}
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="fuerza"
                        control={formualioCartera.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect

                                label="Fuerza"
                                value={dataUsuario?.idFuerza}
                                // value={objetoFiltrado?.fuerza}
                                readOnly={readonlyFinal}
                                open={false}
                            >
                                {catalogState.Force?.map(
                                    (option: FORCEDTO) => (
                                        <MenuItem
                                            key={option.id}
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
                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="recursosFuturos"
                        control={formualioCartera.control}
                        rules={{
                            required:
                                'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                InputLabelProps={{ shrink: true }}
                                helperText={
                                    error ? error.message : null
                                }
                                inputProps={{
                                    style: {
                                        textAlign: 'right'
                                    }

                                }}
                                label="Recursos Futuros*"
                                value={value}
                                isReadOnly={readonlyFinal}
                                onChange={onChange}
                                currencyMask

                            />
                        )}
                    />
                </Grid>

            </Grid>

            <Grid container>

                <FormularioInformacionDelInmueble
                    formualioCartera={formualioCartera}
                    classes={classes}
                    setValorInmueblePP={setValorInmueblePP}
                    catalogState={catalogState}
                    tipoInmuebles={tipoInmuebles}
                    handleOpenAddressDetalleProducto={handleOpenAddressDetalleProducto}
                    handleKeyPressDetalleProducto={handleKeyPressDetalleProducto}
                    filtroCiudadesPorDepartamento={filtroCiudadesPorDepartamento}
                    filtroTipoViviendaPorCiudad={filtroTipoViviendaPorCiudad}
                    setCambioCiudad={setCambioCiudad}
                    ciudades3={ciudades3}
                    readonlyFinal={readonlyFinal}
                    bandera={bandera}
                    salarioMinimoAnual={salarioMinimoAnual}
                />
            </Grid>

            <Grid container>
                <FormularioDatosProducto
                    formualioCartera={formualioCartera}
                    classes={classes}
                    catalogState={catalogState}
                    setEsLeasing={setEsLeasing}
                    starGetCreditLine={starGetCreditLine}
                    readonlyFinal={readonlyFinal}
                    productoDatos={productoDatos}
                    categoriaSalario={categoriaSalario}
                    setSalarioMinimoAnual={setSalarioMinimoAnual}
                />
            </Grid>

            <Grid container>
                <FormularioDatosSeguros
                    formualioCartera={formualioCartera}
                    classes={classes}
                    readonlyFinal={readonlyFinal}
                    aprobadoPasoCartera={bandera}
                    aseguradoras={aseguradoras}
                />

            </Grid>

            <Grid container>
                <FormularioBeneficiosYAlivios
                    formualioCartera={formualioCartera}
                    classes={classes}
                    readonlyFinal={readonlyFinal} />

            </Grid>

            <Grid container>

                <Grid className={classes.column} item xs={12} md={12}>
                    <Typography className={classes.titleform} variant="h5" component="h6">Información Financiera</Typography>
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="totalMes"
                        control={formualioCartera.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={readonlyFinal || (datosFinancierosPersonales?.valorSalario !== undefined && datosFinancierosPersonales?.valorSalario !== null) || (datosFinancierosPersonales?.totalIngresos !== undefined && datosFinancierosPersonales?.totalIngresos !== null)}
                                label="Total Ingresos Mes"
                                value={value}
                                onChange={onChange}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                                currencyMask
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="deducciones"
                        control={formualioCartera.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={readonlyFinal || (datosFinancierosPersonales?.totalEgresos !== undefined && datosFinancierosPersonales?.totalEgresos !== null)}
                                label="Deducciones"
                                value={value}
                                onChange={onChange}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                                currencyMask
                            />
                        )}
                    />
                </Grid>

            </Grid>
            <Grid container >
                <Grid className={classes.column} item xs={12} md={3} />

                <Grid item xs={12} md={6}>
                    {
                        bandera ? null :
                            <Button
                                variant="contained"
                                component="label"
                                color="primary"
                                className={classes.button}

                            >
                                <input
                                    type="file"
                                    onChange={e =>
                                        handleFileChosen(e.target.files != null ? e.target.files[0] : '',)
                                    }
                                    hidden
                                />
                                Cargar Plan Pagos
                            </Button>
                    }


                    <Button
                        variant="contained"
                        color="primary"
                        className={classes.button}
                        onClick={formualioCartera.handleSubmit(generarPlanPagos)}
                    >
                        {tituloBoton}
                    </Button>
                    <Button
                        disabled={buttonDisable}
                        variant="contained"
                        color="primary"
                        className={classes.button}
                        onClick={formualioCartera.handleSubmit(guardarFormulario)}
                    >
                        AGREGAR NUEVO PRODUCTO
                    </Button>

                </Grid>
            </Grid>
            <AddressComponentV2
                onOpen={openDireccionDetalleProducto}
                onClose={onCloseAddressDetalleProducto}
                control={FormualrioDireccion.control}
                getValues={FormualrioDireccion.watch}
                tipoDireccion={false}
                setValue={FormualrioDireccion.setValue}
            />
            <br />
            <br />
            {
                detallesSimulacionTabla.length !== 0 && !mostrarTablaSegunFormulario ?
                    <TablaPlanDePagos
                        reportRequest={reportRequest}
                    /> : null
            }
            {
                obtenerPlanPagosVer.length !== 0 && mostrarTablaSegunFormulario ?
                    <TablaPlanPagosCredito
                        obtenerPlanPagosVer={obtenerPlanPagosVer}
                    /> : null
            }
        </>)
}
export default InfoNewProduct