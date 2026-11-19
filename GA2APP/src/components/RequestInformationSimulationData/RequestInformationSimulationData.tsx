import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Button, Grid, MenuItem } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHPaper from '../ch-paper/CHPaper';
import { startPostCreateSimulation } from '../../actions/credit.actions';
import { AppState } from '../../store/rootReducer';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import UtilService from '../../utils/util.service';
import {
    CreditCreateSimulationDataModel,
    DatosSimulacionParaEstudioForm,
    DatosSimulacionParaEstudioModel,
} from '../../models/credit-create-simulation-data.model';
import CHTextField from '../ch-text-field/CHTextField';
import CHSelect from '../ch-select/CHSelect';
import {
    CreateSimulationClient,
    CreditProductRequestModel,
} from '../../models/requests/create-simulation-client';
import {
    startGetAllTypeHome,
    startGetAllAbonoUnico,
    startGetAllTipoAbono,
    startGetAllTipoSubsidio,
    startGetAllEstadoVivienda,
} from '../../actions/catalog.action';
import CHCheckbox from '../ch-checkbox/CHCheckbox';
import ApiCatalog from '../../services/api-catalog.service';
import { LoginState } from '../../reducers/login.reducer';
import { CreditProductModel } from '../../models/credit-product.model';
import { CatalogState } from '../../reducers/catalog.reducer';
import { TYPEHOME } from '../../types/catalog.type';
import { GridDataCreditoHipotecario } from './RequestInformationHipotecario';
import { GridDataCreditoLeasing } from './RequestInformationLeasing';
import ApiCredit from '../../services/api-credit.service';
import {
    CREDITO,
    TIPO_AFILIADO,
} from './constants/RequestInformationSimulationData.constants';
import ApiSimulador from '../../services/api-simulador.service';
import NotificationService from '../../services/notification.service';
import ApiSimulacion from '../../services/api-simulacion.service';
import Prospeccion from '../../services/api-prospeccion.service';
import AuthService from '../../services/auth.service';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import { CreditConsultCreditRequestModel } from '../../models/requests/credit-consult-credit-request.model';

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '110px',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);

function RequestInformationSimulationData({
    startPostCreditProductData,
    procedureRequest,
    creditProductData,
    loginState,
    catalogState,
    isLoading,
    createSimulationData,
    startProcess,
    getAllTiVivienda,
    getAllEstados,
    getAllAbono,
    getAllTipoAbo,
    getAllTipoSubs,
    setHacerSimulacion,
}: {
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    creditProductData: CreditProductModel;
    loginState: LoginState;
    catalogState: CatalogState;
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean;
    createSimulationData: CreditCreateSimulationDataModel[];
    startProcess: (request: any) => void;
    getAllTiVivienda: () => void;
    getAllAbono: () => void;
    getAllEstados: (props: string) => void;
    getAllTipoAbo: () => void;
    getAllTipoSubs: () => void;
    setHacerSimulacion:any;
}) {
    const classes = useStyles();
    const apiCredit = ApiCredit();
    const apiCatalog = ApiCatalog();
    const apiSimulador = ApiSimulador();
    const simulacionApi = ApiSimulacion();
    const apiProspeccion = Prospeccion();

    const user = AuthService.getUserInfo();



    const formularioSimulador = useForm<CreateSimulationClient>({
        defaultValues: {
            documento: '',
            tipoVivienda: '',
            valorInmueble: '',
            tipoAbono: 1,
            subsidio: 'N',
            abonoUnico: false,
            valorCuota: 0,
            plazo: 0,
            valorPrestamo: 0,
            canonInicial: 0,
            opcionCompra: 0,
            ValorViviendaSeguro: 0,
            TasaVoluntariaAportes: false,
            fechaDesembolso:''

        },
    });

    const formularioSeguros = useForm({
        defaultValues: {
            maneja_poliza_vida: '',
            aseguradora_poliza_vida: '',
            maneja_poliza_hogar: '',
            aseguradora_poliza_hogar: '',
           
        },
    });

    const [tipoAbono, setTipoAbono] = useState<any>([]);
    const [datosInformacionSimulacion, setDatosInformacionSimulacion] =
        useState<DatosSimulacionParaEstudioModel>(
            DatosSimulacionParaEstudioForm,
        );
    const [
        aplicaBeneficioCreditoHipotecario,
        setAplicaBeneficioCreditoHipotecario,
    ] = useState(false);
    const [plazoMaxHipotecario, setPlazoMaxHipotercario] = useState(0);
    const [plazoMinHipotecario, setPlazoMinHipotercario] = useState(0);
    const [plazoMaxLeasing, setPlazoMaxLeasing] = useState(0);
    const [plazoMinLeasing, setPlazoMinLeasing] = useState(0);
    const [datosUsuario, setDatosUsuario] = useState<any>([])
    const [loading  , setLoading] = useState(false)
    const [estadoRestaurar  , setEstadoRestaurar] = useState('')
    const [manejaPoliza, setManejaPoliza] = useState([]);
    const [porcentajeExtraprima, setporcentajeExtraprima] = useState('')
  

  
    const [deshabilitarOpcionCompra, setDeshabilitarOpcionCompra] =
        useState(false);
    const deshabilitarValorCuota = formularioSimulador.watch('valorCuota') < 0;
    const deshabilitarvalorPrestamo = formularioSimulador.watch('valorPrestamo') < 0;

 

    useEffect(() => {
        obtnerextraprima()
        ObtenerDatosSimulacion()
        consultarManejaPoliza();
        obtenerDatosSimulacionParaEstudio(String(procedureRequest.clientIdentificationNumber),);
        plazoHipotecario();
        getAllTiVivienda();
        getAllEstados(loginState.claims);
        getAllAbono();
        getAllTipoAbo();
        getAllTipoSubs();
        consultarTipoAbonos();
        obtenerSeguroPorId();
    }, []);

    useEffect(() => {
  
        const dataRequest: CreditProductRequestModel = {
            ind: 2,
            soC_ID: procedureRequest.requestID,
        };
        startPostCreditProductData(dataRequest);
        obtenerSimulacionCliente();
    }, []);

    useEffect(() => {
        formularioSimulador.setValue(
            'valorInmueble',
            String(procedureRequest.propertyValue),
        );
        formularioSimulador.setValue(
            'tipoVivienda',
            creditProductData.propertyType,
        );
          formularioSimulador.setValue('fechaDesembolso', UtilService.getDate(creditProductData.fechDesembolso) )
    }, [procedureRequest, creditProductData.propertyType]);

    useEffect(() => {
        if (
            datosInformacionSimulacion?.tipoActorId ===
            TIPO_AFILIADO.SOLUCION_VIVIENDA
        ) {
            formularioSimulador.setValue('canonInicial', datosUsuario?.porcentajeCanonInicial); 
            formularioSimulador.setValue('opcionCompra',  datosUsuario?.porcentajeOpciondeCompra);
            console.log('ver qie llega aqui 3 🐱‍🏍🐱‍👤',datosUsuario)
       
            setDeshabilitarOpcionCompra(true);
        }

        if (
            datosInformacionSimulacion?.tipoActorId ===
            TIPO_AFILIADO.ADMINISTRACION_CESANTIAS
        ) {
            formularioSimulador.setValue('canonInicial', 20);
            formularioSimulador.setValue('opcionCompra', 10);
            setDeshabilitarOpcionCompra(true);
        }

        if (
            datosInformacionSimulacion?.tipoActorId === TIPO_AFILIADO.VETERANOS
        ) {
            formularioSimulador.setValue('canonInicial', 5);
            formularioSimulador.setValue('opcionCompra', 0);
            setDeshabilitarOpcionCompra(true);
        }
    }, [datosInformacionSimulacion, datosUsuario, setDatosUsuario]);

  
    const consultarManejaPoliza = async () => {
        
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(80)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setManejaPoliza(response.data);
                        // if(response.data === "S"){
                        //     formularioSeguros.setValue('maneja_poliza_vida', valor);
                        // }
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
        const guardarReporteSimulacion = async () => {

                const dataRequest : any = {
                smc_id:procedureRequest?.simulationID,
                afl_id:procedureRequest?.flowActivityID,
                simulacionId:  String(procedureRequest?.requestID),

            };

           

        await simulacionApi
            .postGuardarReporteSimulacion(dataRequest)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                    const respuesta = response;

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

        const obtenerSeguroPorId = async () => {
            const request = {
                numeroDocumento:  procedureRequest.clientIdentificationNumber.toString(),   
            }
            await apiProspeccion
                .postConsultarSegurosCredito(request)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                            if(response?.data?.length !== 0){
                                formularioSeguros.setValue('maneja_poliza_vida', response.data[0].ssC_CONVENIO_VIDA)
                                formularioSeguros.setValue('maneja_poliza_hogar', response.data[0].ssC_CONVENIO_HOGAR)      
                            } 
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        }

    const obtenerSimulacionCliente = () => {
        
        const request = { request: procedureRequest.clientIdentificationNumber.toString() };
        apiCredit
            .posDatosUsuario(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        formularioSimulador.setValue(
                            'valorInmueble',
                            UtilService.formatCurrency(
                                response.data.valorInmueble,
                            ),
                        );
                        // formularioSimulador.setValue('canonInicial', datosUsuario?.porcentajeCanonInicial); 
                        formularioSimulador.setValue('canonInicial', response.data.porcentajeCanonInicial); 
                        formularioSimulador.setValue('opcionCompra',  response.data.porcentajeOpciondeCompra);
                        formularioSimulador.setValue('valorPrestamo',  response.data.valorPrestamoSolicitado);
                        formularioSimulador.setValue('ValorViviendaSeguro',  response.data.valorInmuebleSeguro);
                        const valorViviensaSeg = response.data.aporte_Voluntario !== 0 
                        formularioSimulador.setValue('TasaVoluntariaAportes', valorViviensaSeg );
                        formularioSimulador.setValue('tipoAbono', 1);
                        formularioSimulador.setValue('subsidio', 'N');
                      
                        console.log('ver qie llega aqui 2 🐱‍🏍🐱‍👤', response.data)
                        setDatosUsuario(response.data);
                    } else {
                        NotificationService.genericAlert(
                            'Error en actualización',
                            'Ocurrio un error al intentar actualizar, intente de nuevo.',
                            'error',
                        );
                        
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

    const onSubmitRestaurar = async() => {
        setLoading(true)
        const informacion = {
            afl_id: procedureRequest?.flowActivityID,
            numero_Documento:procedureRequest.clientIdentificationNumber.toString(),
            restauracion: 1
        }
        await apiProspeccion
            .postRestaurarSimulacionPlanCliente(informacion)
            .then(async response => {
                setLoading(false)
                setEstadoRestaurar('S')
                obtenerSimulacionCliente()
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setLoading(false)
                setEstadoRestaurar('N')
            });
        // startProcess(informacion);
        guardarReporteSimulacion();
    }

    const onSubmit = async (request: any) => {
console.log('request', request)
        
if(user.USR_ID === '83e91a3c-02a3-4d49-995e-87154620f98d' ){
    setHacerSimulacion(true)

}
        let idSimulador;
        let plazoTotal = request.plazo;
        setAplicaBeneficioCreditoHipotecario(
            request?.tipoVivienda === 'NUEVA'
                ? request?.subsidio === 'S'
                : request?.subsidio === 'N',
        );

        if (formularioSeguros.getValues('maneja_poliza_vida') === '' || formularioSeguros.getValues('maneja_poliza_hogar') === '' ) {
            formularioSeguros.register('maneja_poliza_vida', {
                required: 'Este campo es requerido.',
            });
            formularioSeguros.register('maneja_poliza_hogar', {
                required: 'Este campo es requerido.',
            });


            return;
        }
        if (procedureRequest.creditType === 'CRÉDITO HIPOTECARIO') {
            idSimulador = 1;
        } else if (procedureRequest.creditType === 'LEASING HABITACIONAL') {
            idSimulador = 2;
        } else {
            idSimulador = 10;
        }

        if (datosInformacionSimulacion?.tipoCreditoId === CREDITO.HIPOTECARIO && plazoTotal === 0) {
            plazoTotal = 180;
        }

        if (datosInformacionSimulacion?.tipoCreditoId === CREDITO.HIPOTECARIO) {
            if (
                plazoTotal >= plazoMinHipotecario &&
                plazoTotal <= plazoMaxHipotecario
            ) {
                const informacion = {
                    valor: UtilService.cleanCurrencyValue( request.valorInmueble as number,),
                    documento:procedureRequest.clientIdentificationNumber.toString(),
                    tipoIdentificacion: String(creditProductData.tidID),
                    tipoVivienda: request.tipoVivienda === 'NUEVA' ? 'N' : 'U',
                    tipoAbono: Number(request.tipoAbono),
                    aplicaBeneficio: request.subsidio === 'S',
                    abonoUnico: request.abonoUnico,
                    valorCuota: UtilService.cleanCurrencyValue(request.valorCuota,),
                    valorPrestamo: UtilService.cleanCurrencyValue(request.valorPrestamo,),
                    valorViviendaSeguro: UtilService.cleanCurrencyValue(request.ValorViviendaSeguro,) === 0 ? 
                    UtilService.cleanCurrencyValue(request.valorInmueble as number,) : 
                    UtilService.cleanCurrencyValue(request.ValorViviendaSeguro,),
                    plazo: plazoTotal,
                    simId: String(idSimulador),
                    tipoSimularOProspectar: 3,
                    afl_id: procedureRequest.flowActivityID,
                    usr_id: user.USR_ID,
                    fechaDesembolso:request?.fechaDesembolso,
                    porcentajeExtraprima
                };
               await startProcess(informacion);
                guardarReporteSimulacion();

            } else {
                await NotificationService.genericAlert(
                    'Atención',
                    `No cumple con la politica de plazo único, ${request.plazo} meses`,
                    'warning',
                );
            }
        }

        if (datosInformacionSimulacion?.tipoCreditoId === CREDITO.LEASING) {

            const informacion = {
                valor: UtilService.cleanCurrencyValue(
                    request.valorInmueble as number,
                ),
                documento:
                    procedureRequest.clientIdentificationNumber.toString(),
                tipoIdentificacion: String(creditProductData.tidID),
                tipoVivienda:
                    request.tipoVivienda === 'NUEVA' ? 'N' : 'U',
                porOpcionCompra: request.opcionCompra,
                canonInicial: String(request.canonInicial),
                plazo: plazoTotal,
                simId: String(idSimulador),
                valorPrestamo: UtilService.cleanCurrencyValue(
                    request.valorPrestamo,
                ),
                valorViviendaSeguro: UtilService.cleanCurrencyValue(request.ValorViviendaSeguro,) === 0 ? 
                UtilService.cleanCurrencyValue(request.valorInmueble as number,) : 
                UtilService.cleanCurrencyValue(request.ValorViviendaSeguro,),
                TasaVoluntariaAportes: request.TasaVoluntariaAportes,
                tipoSimularOProspectar: 3,
                usr_id: user.USR_ID,
                fechaDesembolso:request?.fechaDesembolso,
                porcentajeExtraprima
            };
            startProcess(informacion);
        }
    };

    const consultarTipoAbonos = async () => {
        await apiCatalog
            .getTipoAbono()
            .then(async response => {
                setTipoAbono(response.data);
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerDatosSimulacionParaEstudio = async (request: any) => {
        await apiCredit
            .getObtenerDatosSimulacionParaEstudio(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response?.data !== null) {
                        setDatosInformacionSimulacion(response?.data);
                        
                        if(procedureRequest.flowActivityID !== 8)
                        {
                            guardarReporteSimulacion();
                        }
                           
                       
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    

    const plazoHipotecario = async () => {
        apiSimulador
            .getAllSimuladorTasas()
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        const [PlazoMeses] = response.data;
                        const [, , plazoMesesLeasing] = response.data;
                        setPlazoMaxHipotercario(PlazoMeses.maximoPlazoMeses);
                        setPlazoMinHipotercario(PlazoMeses.minimoPlazoMeses);
                        setPlazoMaxLeasing(plazoMesesLeasing.maximoPlazoMeses);
                        setPlazoMinLeasing(plazoMesesLeasing.minimoPlazoMeses);
                    } else {
                        NotificationService.genericAlert(
                            'Error en actualización',
                            'Ocurrio un error al intentar actualizar, intente de nuevo.',
                            'error',
                        );
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };


    const ObtenerDatosSimulacion = async () => {
        const request = {
            smc_id: procedureRequest.simulationID,
        };
        await apiCredit
            .getObtenerDatosSimulacion(request)
            .then(async (response: any) => {
                if (response) { 
                    if (response.isSuccess) {

                        
                       formularioSimulador.setValue('plazo', Number(response?.data?.length - 1));
                       formularioSimulador.setValue('fechaDesembolso', (response?.data[0].fechaDesembolso));
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

    const obtnerextraprima = async () =>{
        const request: CreditConsultCreditRequestModel = {
            estado: "0",
    idSolicitudCredito:procedureRequest.requestID,
    identificacion: String(procedureRequest?.clientIdentificationNumber),
        }

		
        try {
            
            const response = await apiCredit.postConsultCreditData(request);
            console.log(response.data.porcentajeExtraprima)
            setporcentajeExtraprima(response.data.porcentajeExtraprima)
        
        } catch (e) {
            console.log('errpr')
            
        }
    }

  



    const guardarSeguros = async (dataSeguro: any) => {
        
        const request = {
            numeroDocumento: procedureRequest.clientIdentificationNumber.toString(),
            soC_CONVENIO_ASEGURADORA: dataSeguro.maneja_poliza_vida,
            asE_ID: '',
            soC_CONVENIO_ASEGURADORA_HOGAR: dataSeguro.maneja_poliza_hogar,
            asE_ID_HOGAR: '',
        }
        await apiProspeccion
            .postRegistrarSeguroDatosCredito(request)
            .then()
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            })


    }

    if (isLoading || loading) {
        return <CHLoadingProcess />;
    }

    return (
        <>
            <CHPaper title="Simulación Información">
                <Grid container>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="valorInmueble"
                            control={formularioSimulador.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    label="Valor Inmueble"
                                    value={value}
                                    currencyMask
                                    disabled={procedureRequest.flowActivityID !==2}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="tipoVivienda"
                            control={formularioSimulador.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de Vivienda"
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    readOnly={procedureRequest.flowActivityID === 18}
                                    value={value}
                                >
                                    {catalogState?.typeHome.map(
                                        (option: TYPEHOME) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.descripcion}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                  
                    {datosInformacionSimulacion?.tipoCreditoId ===
                        CREDITO.LEASING && ( datosUsuario.tipoAfiliadoId !== 1 ? 
                        (
                                    <Grid item md={4} className={classes.column}>
                                        <Controller
                                            name="canonInicial"
                                            control={
                                                formularioSimulador.control
                                            }
                                            rules={{
                                                required: 'Este campo es requerido.',
                                                min: {
                                                    value: 0,
                                                    message: 'No puede ser menor a 0%',
                                                },
                                                max: {
                                                    value: 100,
                                                    message: 'No puede ser mayor a 100%',
                                                },
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    error={!!error}
                                                    helperText={
                                                        error ? error.message : null
                                                    }

                                                    label="Canon inicial (%)"
                                                    value={value}
                                                    onChange={value => {
                                                        const valor =
                                                            value.target.value;
                                                        formularioSimulador.register(
                                                            'canonInicial',
                                                            {
                                                                required:
                                                                    'Este campo es requerido.',

                                                                min: {
                                                                    value: 0,
                                                                    message:
                                                                        'No puede ser menor a 0%',
                                                                },
                                                                max: {
                                                                    value: 100,
                                                                    message:
                                                                        'No puede ser mayor a 100%',
                                                                },
                                                            },
                                                        );
                                                        onChange(valor);
                                                    }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                ) : (
                  
                        <Grid item md={4} className={classes.column}>
                            <Controller
                                name="canonInicial"
                                control={formularioSimulador.control}
                                
                                rules={{
                                    required: 'Este campo es requerido.',
                                    min: {
                                        value: 5,
                                        message: 'No puede ser menor a 5%',
                                    },
                                    max: {
                                        value: 15,
                                        message: 'No puede ser mayor a 15%',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        label="Canon inicial (%)"
                                        value={value}
                             
                                     onChange={value => {
                                                        const valor =
                                                            value.target.value;
                                                        formularioSimulador.register(
                                                            'canonInicial',
                                                            {
                                                                required:
                                                                    'Este campo es requerido.',

                                                                min: {
                                                                    value: 5,
                                                                    message:
                                                                        'No puede ser menor a 5%',
                                                                },
                                                                max: {
                                                                    value: 15,
                                                                    message:
                                                                        'No puede ser mayor a 15%',
                                                                },
                                                            },
                                                        );
                                                        onChange(valor);
                                                    }}
                                    />
                                )}
                            />
                        </Grid>
                   ) )}
                    {datosInformacionSimulacion?.tipoCreditoId ===
                        CREDITO.LEASING && (
                        <Grid item md={4} className={classes.column}>
                            <Controller
                                name="opcionCompra"
                                control={formularioSimulador.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Opción compra (%)"
                                        value={value}
                                        // isReadOnly={deshabilitarOpcionCompra}
                                        onChange={value => {
                                            onChange(value);
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {datosInformacionSimulacion?.tipoCreditoId ===
                        CREDITO.HIPOTECARIO && (
                        <Grid item md={4} className={classes.column}>
                            <Controller
                                name="tipoAbono"
                                control={formularioSimulador.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Desea realizar abonos extra?"
                                        value={value}
                                        readOnly={procedureRequest.flowActivityID === 18}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={onChange}
                                    >
                                        {tipoAbono?.map((option: any) => (
                                            <MenuItem
                                                key={option.idTipoAbono}
                                                value={option.idTipoAbono}
                                            >
                                                {option.descripcionTipoAbono}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    )}
                    {datosInformacionSimulacion?.tipoCreditoId ===
                        CREDITO.HIPOTECARIO &&
                        formularioSimulador.watch('tipoVivienda') ===
                            'NUEVA' && (
                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="subsidio"
                                    control={formularioSimulador.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Aplica Beneficio"
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            <MenuItem value="">
                                                Seleccionar
                                            </MenuItem>
                                            <MenuItem value="S">SI</MenuItem>
                                            <MenuItem value="N">NO</MenuItem>
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        )}
                    {datosInformacionSimulacion?.tipoCreditoId ===
                        CREDITO.HIPOTECARIO &&
                        datosInformacionSimulacion?.tipoActorId ===
                            TIPO_AFILIADO.SOLUCION_VIVIENDA &&
                        datosInformacionSimulacion?.cuotas < 168 && (
                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="abonoUnico"
                                    control={formularioSimulador.control}
                                    render={({ field: { onChange } }) => (
                                        <CHCheckbox
                                            label="Abono Único?"
                                            onChange={onChange}
                                            disabled={procedureRequest.flowActivityID === 18}
                                        />
                                    )}
                                />
                            </Grid>
                        )}
                      
                             <Grid item md={4} className={classes.column}>
                            <Controller
                                name="valorPrestamo"
                                control={formularioSimulador.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Valor Préstamo"
                                        value={value}
                                        disabled={deshabilitarvalorPrestamo}
                                        isReadOnly={procedureRequest.flowActivityID === 18}
                                        currencyMask
                                        helperText={error ? error.message : null}
                                        onChange={value => {
                                            onChange(value);
                                        }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                        
                  
                    {
                       datosInformacionSimulacion?.tipoCreditoId === CREDITO.LEASING ?
                       '':
                       <Grid item md={4} className={classes.column}>
                       <Controller
                           name="valorCuota"
                           control={formularioSimulador.control}
                           render={({
                               field: { onChange, value },
                               fieldState: { error },
                           }) => (
                               <CHTextField
                                   disabled={deshabilitarValorCuota}
                                   currencyMask
                                   error={!!error}
                                   label="Valor Cuota"
                                   value={value}
                                   isReadOnly={procedureRequest.flowActivityID === 18}
                                   helperText={error ? error.message : null}
                                   onChange={value => {
                                       formularioSimulador.setValue(
                                           'valorPrestamo',
                                           0,
                                       );
                                       onChange(value);
                                   }}
                                   inputProps={{
                                       style: {
                                           textAlign: 'right',
                                       },
                                   }}
                               />
                           )}
                       />
                   </Grid>
                    }

                    {/* {
                         datosInformacionSimulacion?.tipoCreditoId === CREDITO.LEASING && datosUsuario?.tipoAfiliadoId === 1 && datosInformacionSimulacion?.cuotas < 108 ?
                         '': */}
                         <Grid item md={4} className={classes.column}>
                         <Controller
                             name="plazo"
                             control={formularioSimulador.control}
                             render={({
                                 field: { onChange, value },
                                 fieldState: { error },
                             }) => (
                                 <CHTextField
                                     error={!!error}
                                     helperText={error ? error.message : null}
                                     label="Plazo"
                                     isReadOnly={procedureRequest.flowActivityID === 18}
                                     value={value}
                                     onChange={value => {
                                         formularioSimulador.setValue(
                                             'plazo',
                                             0,
                                         );
                                         onChange(value);
                                     }}
                                 />
                             )}
                         />
                     </Grid>
                     <Grid item md={4} className={classes.column}>
                        <Controller
                            name="ValorViviendaSeguro"
                            control={formularioSimulador.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    label="Valor Vivienda Seguro"
                                    value={value}
                                    isReadOnly={procedureRequest.flowActivityID === 18}
                                    currencyMask
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid container item xs={2} md={4} >
                            <Controller
                                name="TasaVoluntariaAportes"
                                control={formularioSimulador.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                    //    defaultValue = 'true'
                                        label="¿Aporte Voluntario?*"
                                        onChange={onChange}
                                        checked={value}
                                        disabled={procedureRequest.flowActivityID === 18}

                                    />
                                )}
                            />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="maneja_poliza_vida"
                                    control={formularioSeguros.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Maneja convenio póliza vida"
                                            value={value}
                                            readOnly={procedureRequest.flowActivityID === 18}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={value => {
                                                
                                                let valor = ''                                                   
                                                if (
                                                    value.target.value ===
                                                    'S'
                                                ) {
                                                    valor = 'S'
                                                    formularioSeguros.setValue('maneja_poliza_vida', valor);
                                                   
                                               
                                                }else{
                                                    valor = 'N'
                                                    formularioSeguros.setValue('maneja_poliza_vida', valor);
                                                    
                                                }

                                                onChange(value);
                                            }}
                                        >
                                           
                                            {manejaPoliza?.map(
                                                (option: any) => (
                                                    <MenuItem
                                                        key={option.codigo}
                                                        value={option.codigo}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                                       
                            <Grid item md={4} className={classes.column}>
                                <Controller
                                    name="maneja_poliza_hogar"
                                    control={formularioSeguros.control}
                                  
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            label="Maneja convenio póliza hogar"
                                            value={value}
                                            readOnly={procedureRequest.flowActivityID === 18}
                                            onChange={value => {
                                                let valor = ''                                                   
                                                if ( value.target.value ==='S') {
                                                    valor = 'S'
                                                    formularioSeguros.setValue("maneja_poliza_hogar", valor)
                                                
                                               
                                                }else{
                                                    valor = 'N'
                                                    formularioSeguros.setValue("maneja_poliza_hogar", valor)
                                                 
                                                }

                                                onChange(value);
                                            }}
                                        >
                                            {manejaPoliza?.map(
                                                (option: any) => (
                                                    <MenuItem
                                                        key={option.codigo}
                                                        value={option.codigo}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                          { procedureRequest.flowActivityID === 18 ?  <Grid item md={4} className={classes.column}>
                        <Controller
                            name="fechaDesembolso"
                            control={formularioSimulador.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha desembolso"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>: null}
                </Grid>
            </CHPaper>
           { procedureRequest?.flowActivityID === 18 &&
            <Grid item xs={12} md={12}>
                    <Button
                        onClick={() => {formularioSimulador.handleSubmit(onSubmit)();
                            formularioSeguros.handleSubmit(guardarSeguros)();}}
                        variant="contained"
                        size="large"
                        style={{ backgroundColor: '#a9a9a9', color: 'white', width: "100%" }}
                    >
                        Simular
                    </Button>
                </Grid>}
           { procedureRequest?.flowActivityID === 18 ? null : <Grid container spacing={2} style={{ marginBottom: "30px", marginTop: "20px" }}>
           <Grid item xs={12} md={6}>
                    <Button
                        onClick={() => {formularioSimulador.handleSubmit(onSubmit)();
                            formularioSeguros.handleSubmit(guardarSeguros)();}}
                        variant="contained"
                        size="large"
                        style={{ backgroundColor: '#a9a9a9', color: 'white', width: "100%" }}
                    >
                        Simular
                    </Button>
                </Grid>
                <Grid item xs={12} md={6}>
                    <Button
                        onClick={formularioSimulador.handleSubmit(onSubmitRestaurar)}
                        variant="contained"
                        size="large"
                        style={{ backgroundColor: '#a9a9a9', color: 'white', width: "100%" }}
                    >
                        Restaurar Simulación
                    </Button>
                </Grid>
            </Grid>}

            <br />

            {datosInformacionSimulacion?.tipoCreditoId === 1 && (
                <GridDataCreditoLeasing
                    createSimulationData={createSimulationData}
                    procedureRequest={procedureRequest}
                    estadoRestaurar={estadoRestaurar}
             
                />
                )}

            {datosInformacionSimulacion?.tipoCreditoId === 2 && (
                <GridDataCreditoHipotecario
                    createSimulationData={createSimulationData}
                    aplicaBeneficio={aplicaBeneficioCreditoHipotecario}
                    procedureRequest={procedureRequest} 
                    estadoRestaurar={estadoRestaurar}   
                />
            )}
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    // isLoading: state.credit.loading,
    catalogState: state.catalog,
    createSimulationData: state.credit.creditSimulationResultData,
    creditProductData: state.credit.creditProductData,
});

const mapDispatchToProps = (dispatch: any) => ({
    startProcess: (request: any) => dispatch(startPostCreateSimulation(request)),
    getAllTiVivienda: () => dispatch(startGetAllTypeHome()),
    getAllAbono: () => dispatch(startGetAllAbonoUnico()),
    getAllTipoAbo: () => dispatch(startGetAllTipoAbono()),
    getAllEstados: (props: string) => dispatch(startGetAllEstadoVivienda(props)),
    getAllTipoSubs: () => dispatch(startGetAllTipoSubsidio()),
});

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationSimulationData);
