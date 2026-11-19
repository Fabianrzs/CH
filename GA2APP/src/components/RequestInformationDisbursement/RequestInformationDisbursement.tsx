


// RequestInformationDisbursement

import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import {
    Button,
    Grid,
    IconButton,
    MenuItem,
    Table,
    TableCell,
    TableRow,
    TableBody,
    TableHead,
} from '@material-ui/core';
import { useForm, Controller } from 'react-hook-form';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import DeleteIcon from '@material-ui/icons/Delete';
import moment from 'moment';
import { connect } from 'react-redux';

import CHPaper from '../ch-paper/CHPaper';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHTextField from '../ch-text-field/CHTextField';
import CreditApi from '../../services/api-credit.service';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import CHSelect from '../ch-select/CHSelect';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import UtilService from '../../utils/util.service';
import AuthService from '../../services/auth.service';
import NotificationService from '../../services/notification.service';
import BotoneraPagosDesembolso from '../RequestInformationDistributionData/BotoneraPagosDesembolso';
import { CreditProductRequestModel } from '../../models/requests/create-simulation-client';
import { startGetMovementsAccountsClient, 
    startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import { startObtenerVendedorCredito} from "../../actions/credit.actions";
import { AppState } from '../../store/rootReducer';
import { ClientState } from '../../reducers/client.reducer';
import afiliadoApi from '../../services/api-afiliado.service';
import { CreditProductModel } from '../../models/credit-product.model';
import { LoadingV2Component } from '../loading/loadingV2.component';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginTop: '1rem',
            marginLeft: '.5rem',
        },
    }),
);

const api = CreditApi();
const apiBeneficiario = afiliadoApi()


function RequestInformationDisbursement({
    startGetMovementsAccountsClient,
    startGetClientByIdentificationTypeIdentificationNumber,

    clientState,
    startPostCreditProductData,
    creditProductData,
    procedureRequest,
    startObtenerVendedorCredito,
    CreditState,
    isLoading,
    validacionDeTablasDatosDesembolso
}: {
    startGetMovementsAccountsClient: (identification: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;

    clientState: ClientState;
    CreditState: any;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;    
    startObtenerVendedorCredito: (data: any) => void;    
    creditProductData: CreditProductModel;
    procedureRequest: ProcedureRequestModel;
    isLoading:boolean;
    validacionDeTablasDatosDesembolso:any;
}): any {
    
    const classes = useStyles();
    const [mostarPrimerPago, setMostrarPrimerPago] = useState(procedureRequest.stateActivityID)
    const [mostarSegundoPago, setMostarSegundoPago] = useState(procedureRequest.stateActivityID)
    const [ocultarPrimerPago, setOcultarPrimerPago] = useState(false)

  
    const isFinalizedActivity = procedureRequest.stateActivityID === 3 || procedureRequest.stateActivityID === 4;
    
    useEffect(()=> {
        
        if (procedureRequest.creditType === "CRÉDITO HIPOTECARIO") {
            if (procedureRequest.flowActivityID === 18 || procedureRequest.flowActivityID === 28 || procedureRequest.flowActivityID === 16) {
                setMostarSegundoPago(procedureRequest.flowActivityID)
                setOcultarPrimerPago(false)
            } 
        }else if (procedureRequest.flowActivityID === 50 || procedureRequest.flowActivityID === 16) {
            setMostrarPrimerPago(procedureRequest.flowActivityID)
            setMostarSegundoPago(procedureRequest.flowActivityID)
            setOcultarPrimerPago(true)
        }
    },[procedureRequest.creditType])
    
   
    // const mostarPrimerPago = procedureRequest.flowActivityID === 50  || procedureRequest.flowActivityID === 16  ;
    // const mostarSegundoPago = procedureRequest.flowActivityID ===  18 || procedureRequest.flowActivityID === 28 || procedureRequest.flowActivityID === 16;


   console.log({mostarSegundoPago})
   console.log({mostarPrimerPago})
   console.log({ocultarPrimerPago})
    
    const [resourceList, setResourceList] = useState<any[]>([]);
    const [isLoadingDos, setIsLoadingDos] = useState<boolean>(false);
    const [datosDesembolso, setDatosDesembolso] = useState<any>([]);
    const [datosCuotaInicial, setDatosCuotaInicial] = useState<any>([]);
    const [valorNuevo1, setValorNuevo1] = useState(0);
    const [valorNuevo2, setValorNuevo2] = useState(0);
    const [valorSaldoDesembloso, setValorSaldoDesembloso] = useState(0);
    const [valorSaldoCuentas, setValorSaldoCuentas] = useState(0);
    const [beneficiarioDesembolso, setBeneficiarioDesembolso] = useState<any>([]);
    const [dataPrimerPago, setDataPrimerPago] = useState<any>([]);
    const [segundoTitulo, setSegundoTitulo] = useState<any>("Listado Recursos - Segundo Pago");

    
    const { control, handleSubmit, setValue, getValues  } = useForm({
        defaultValues: {
            frechRate: 0,
            valueFeeRelief: 0,
            typeSource: '',
            valueDisbusement: '',
            date: new Date(),
            beneficiario: '',
            valueCanonInicial: 0,
        },
    });
 
    useEffect(()=>{
            console.log('ver', procedureRequest )
            if(procedureRequest.creditType.includes('LEASING')){
                setValue('valueCanonInicial', Number(procedureRequest.propertyValue-procedureRequest.creditValue))

            }else{
                setValue('valueCanonInicial',0)  
            }

    },[])
   
    
    // setValue('valueCanonInicial', Number(creditProductData?.canonInicial === undefined ? 0 : creditProductData?.canonInicial ))

    useEffect(() => {
        
        const dataRequest: CreditProductRequestModel = {
            ind: 2,
            soC_ID: procedureRequest.requestID,
        };
        startPostCreditProductData(dataRequest);
        const request =  {
            Soc_Id: procedureRequest.requestID,
            Sls_Id: '00000000-0000-0000-0000-000000000000'
        }
        startObtenerVendedorCredito(request)

    }, []);

    useEffect(() => {
        startGetClientByIdentificationTypeIdentificationNumber(
            '',
            1,
            String(procedureRequest.clientIdentificationNumber)
        )
    }, []);

    useEffect(() => {
        mostrarFuentes();
    }, [procedureRequest]);

    useEffect(() => {    
        setIsLoadingDos(true);
        api.getResourcesSource()
            .then(res => {
                
                if (res.isSuccess) {  
                     
                    if(mostarSegundoPago){
                        
                        const nuevoListadoALternativos = res.data.filter(
                            (option: any) =>
                                   option.idFuenteRecursos !== 1
                                && option.idFuenteRecursos !== 2
                        )
                        setResourceList(nuevoListadoALternativos)
                    }
                    
                        
                        if(procedureRequest.creditType ===  "CRÉDITO HIPOTECARIO" && procedureRequest?.sdP_OPCION_COMPRA_CARTERA ){

                             const nuevoListadoALternativos = res.data.filter(
                            (option: any) =>
                                   option.idFuenteRecursos === 1
                        )
                        setResourceList(nuevoListadoALternativos)
                        }else if(procedureRequest?.activity === 'DESEMBOLSO') {

                            const nuevoListadoALternativos = res.data.filter(
                                (option: any) =>
                                       option.caja === "S"
                            )
                            setResourceList(nuevoListadoALternativos)
                        }else if (procedureRequest?.activity === 'LEGALIZACIÓN'){
                           
                            if(procedureRequest.creditType === "LEASING HABITACIONAL"){
                                setResourceList(res.data)
                            }else {
                                const newListFuentes = res.data.filter((x:any) => x.idFuenteRecursos !== 1 && x.idFuenteRecursos !== 2 )
                                setResourceList(newListFuentes)
                            }
                            
                        }else {

                            const nuevoListadoALternativos = res.data.filter(
                                (option: any) =>
                                       option.idFuenteRecursos !== 3
                            )
                            setResourceList(nuevoListadoALternativos)
                        }
                    }                    
                
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            }).finally(() => setIsLoadingDos(false));

    }, []);

    useEffect(() => {
        
        consultarSimulacionCliente(
            String(procedureRequest?.clientIdentificationNumber),
        )    
        
      
       
    }, [procedureRequest]);

    useEffect(()=> {
        
        if( datosDesembolso.length !== 0){
            
            validacionDeTablasDatosDesembolso('Ok')
        }else{
            validacionDeTablasDatosDesembolso('Error')  
        }
    },[datosCuotaInicial, datosDesembolso ])

    useEffect(() => {
        
        if(procedureRequest.flowActivityID === 16){
            const cuentasVigentesExisten = clientState?.client?.cuentas.some(cuenta => cuenta.estadoCuenta === 'VIGENTE');

            if (cuentasVigentesExisten) {

            setValorSaldoDesembloso(procedureRequest.creditValue - valorNuevo1);
    
            const cuentasVigentes = clientState?.client?.cuentas.filter(cuenta => cuenta.estadoCuenta === 'VIGENTE');
            const sumaSaldosVigentes = cuentasVigentes.reduce((total, cuenta) => total + cuenta.saldoDisponible, 0);
            setValorSaldoCuentas(sumaSaldosVigentes - valorNuevo2);
            } else {

            setValorSaldoDesembloso(procedureRequest.creditValue - valorNuevo1);
            setValorSaldoCuentas(0);
            } 
        }else{
            setValue('valueDisbusement', String(valorSaldoDesembloso))
            setValorSaldoDesembloso(procedureRequest.creditValue - valorNuevo1);

        }




    }, [valorNuevo1,valorNuevo2,clientState, ]);

    const mostrarFuentes = async () => {
        
        setIsLoadingDos(true);
        const dataRequest = procedureRequest.requestID;
        await api
            .getFuentes(dataRequest)            
            .then(async response => {

                if (response) {
                    
                    if (response.isSuccess) {

                        
                        const ordenarRecursosCaja = [...response.data.filter((x:any) => 
                            x.idFuenteRecursos === 3)]
                        const index = ordenarRecursosCaja.length;
                        let sumaDesembolso = 0

                        const ordenarRecursosAfiliadoCuentas = [...response.data.filter((x:any) => 
                            x.idFuenteRecursos === 2)]
                        const index1 = ordenarRecursosAfiliadoCuentas.length;
                        let sumaordenarRecursosAfiliadoCuentas = 0

                        const ordenarRecursosAfiliado = [...response.data.filter((x:any) => 
                            x.idFuenteRecursos === 1),...response.data.filter((x:any) => 
                            x.idFuenteRecursos === 2)]
                            const index2 = ordenarRecursosAfiliado.length;
                        let sumaRecursosCuotaInicial = 0
          


                      
                        
                        
                        if( index1>0 || index2>0){
                               for (let i = 0; i < index1; i+=1 ) {
                                 const element = ordenarRecursosAfiliadoCuentas[i]                    
                                 sumaordenarRecursosAfiliadoCuentas += element.valorDesembolso                                                
                              }
                              setValorNuevo2(sumaordenarRecursosAfiliadoCuentas)

                               for (let i = 0; i < index2; i+=1 ) {
                                 const element = ordenarRecursosAfiliado[i]                    
                                 sumaRecursosCuotaInicial += element.valorDesembolso                                                
                              }

                              const newData = [...ordenarRecursosAfiliado];   
                              const newRow = {
                                
                                 descripcionFuenteRecursos:"TOTAL",
                                 valorDesembolso: sumaRecursosCuotaInicial,
                                 fechaDesembolso: "",
                              };
                             
                             newData.push(newRow);    
                             setDatosCuotaInicial(newData)

                            }else {
                                setDatosCuotaInicial([])


                            }

                            if(index>0){
                                for (let i = 0; i < index; i+=1 ) {
                                    const element = ordenarRecursosCaja[i]                    
                                    sumaDesembolso += element.valorDesembolso                                              
                                    }
    
                                    setValorNuevo1(sumaDesembolso)
                                    const newData = [...ordenarRecursosCaja];   
                                    const newRow = {
                                       descripcionFuenteRecursos:"TOTAL",
                                       valorDesembolso: sumaDesembolso,
                                       fechaDesembolso: "",
                                    };
                                   newData.push(newRow);
                                   setDatosDesembolso(newData)
                            } else  {
                                setDatosDesembolso([])
    
                            }
                           

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            }).finally(() => setIsLoadingDos(false));
    };

    const eliminar = (data: any) => async () => { 
        if(data.idSolicitudDesembolso === undefined){
            NotificationService.genericAlert(
                   'Desembolso',
                   'No se puede eliminar TOTAL.',
                   'warning'
               );
            return
        }

        console.log('resourceList', resourceList)
        
 
        const request = {
            idSolicitudCredito: data.idSolicitudCredito,
            idSolicitudDesembolso: data.idSolicitudDesembolso,
        };
        setIsLoadingDos(true);
       
        await api
            .deleteFuente(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {

                        if(data.idFuenteRecursos === 3){
                            
                            const valorEliminado = (valorSaldoDesembloso) + data.valorDesembolso
                            setValorSaldoDesembloso(valorEliminado)
                            setValue('valueDisbusement', String(valorEliminado) )
                            setValue('typeSource', "3")

                            mostrarFuentes();


                        }else if(data.idFuenteRecursos === 2){

                                const valorEliminado = (valorSaldoCuentas) + data.valorDesembolso
                                setValorSaldoCuentas(valorEliminado)
                                setValue('valueDisbusement', String(valorEliminado))
                                setValue('typeSource', "2")
                                mostrarFuentes();

                        }else {
                            
                            setValue('typeSource', "1")
                            const recursosProios = getValues('valueCanonInicial')
                            setValue("valueDisbusement", String(recursosProios))
                            // setValue('valueDisbusement', '0' )
                            mostrarFuentes();
                        }
                        toast('Se ha eliminado correctamente la fuente.', {
                            type: toast.TYPE.SUCCESS,
                        });
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });                      
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            }).finally(() => setIsLoadingDos(false));
    };

    const validacionSeven = (data: any) => async () => { 
        setDataPrimerPago(data)
    }

    const ActualizarDesembolsos = (data: any) => async () => {

        const user = AuthService.getUserInfo();
        const request = {
            solicitudCreditosDesembolsos: [
                {
                    idSolicitudCredito: procedureRequest.requestID,
                    idFuenteRecursos: data.typeSource,
                    fechaDesembolso: moment(data.date).format(),
                    valorDesembolso: UtilService.cleanCurrencyValue(data.valueDisbusement),
                    fechaCreacion: moment().format(),
                    creadoPor: user.USR_ID,
                    Vendedor_Id: data.beneficiario,

                },
            ],
            tasaSolicitudCreditoDto: {
                idSolicitudCredito: procedureRequest.requestID,
                tasaFrech: +data.frechRate,
                valorAlivio: UtilService.cleanCurrencyValue(
                    data.valueFeeRelief,
                ),
                fechaActualizacion: moment().format(),
                actualizadoPor: user.USR_ID,
            },
        };
        setIsLoadingDos(true);

        await api
            .postUploadDisbursementRates(request)
            .then(async response => {                
                if (response) {
                    if (response.isSuccess) {
                        toast('Se ha guardado correctamente la fuente.', {
                            type: toast.TYPE.SUCCESS,
                        });

                        mostrarFuentes();

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            }).finally(() => setIsLoadingDos(false));
 
    }

    const processForm = (data: any) => {  
         
        const valueData = UtilService.cleanCurrencyValue(data.valueDisbusement)
        const user = AuthService.getUserInfo();
        let valor = 0
        if( valueData <= valorSaldoDesembloso  && Number(data.typeSource) === 3 && valueData > 0) {
            valor = valorSaldoDesembloso - valueData;
            ActualizarDesembolsos(data)()
            setValue('valueDisbusement', String(valor))
            setValorSaldoDesembloso(valor)   

        } else if(valueData <= valorSaldoCuentas && Number(data.typeSource) === 2 && valueData > 0 ) {

            valor = valorSaldoCuentas - valueData;
            ActualizarDesembolsos(data)()
            setValue("valueDisbusement", String(valor))
            setValorSaldoCuentas(valor)   

        }else if (Number(data.typeSource) === 3){
            setValue("valueDisbusement", String(valorSaldoDesembloso))
            NotificationService.genericAlert(
                'EL valor supera monto del credito',
                'Intente de nuevo.',
                'warning',
            );
            
        }else if (Number(data.typeSource) === 2){
            setValue("valueDisbusement", String(valorSaldoCuentas ))
            NotificationService.genericAlert(
                'EL valor supera monto de cuentas',
                'Intente de nuevo.',
                'warning',
            );
            
        } else if( Number(data.typeSource) === 1 && valueData > 0 ) {
            ActualizarDesembolsos(data)()
            setValue("valueDisbusement", String(0))
            // const recursosProios = getValues('valueCanonInicial')
            // setValue("valueDisbusement", String(recursosProios))
        }

        mostrarFuentes();
       
    };
    
    const consultarSimulacionCliente = async (identificacion: string) => {
        setIsLoadingDos(true);
        await api
            .postCreditClientSimulation(identificacion)
            .then(async response => {

                if (response) {
                    if (response.isSuccess) {
                        setValue(
                            'frechRate',
                            response.data?.porcentajeTasaFres || 0,
                        );
                        setValue(
                            'valueFeeRelief',
                            response.data?.valorTasaMVFresh.toFixed(5)
                            || 0
                        );
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            }).finally(() => setIsLoadingDos(false));
    };

    if(isLoading){
        return <CHLoadingProcess/>
    }

    return (
        <>
        <LoadingV2Component key="loader" loading={isLoadingDos} tittle='Procesando...' />

        {!isFinalizedActivity && procedureRequest.activity !== "DESEMBOLSO"  && (
            <CHPaper title="Datos Desembolso">
                <Grid container>
                {mostarPrimerPago && (
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="valueCanonInicial"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    currencyMask
                                    label="Canon Inicial"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                )}
                {mostarSegundoPago && (
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="valueDisbusement"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    currencyMask
                                    label="Valor Credito"
                                    value={procedureRequest.creditValue}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                )}
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="frechRate"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={isFinalizedActivity}
                                    label="% Tasa Frech"
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    InputLabelProps={{ shrink: true }}
                                    helperText={error ? error.message : null}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="valueFeeRelief"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    
                                    isReadOnly={isFinalizedActivity}
                                    label="Valor Alivio Cuota"
                                    onChange={onChange}
                                    value={value }
                                    error={!!error}
                                    currencyMask
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="typeSource"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                
                                    error={!!error}
                                    label="Tipo Fuente"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        
                                        
                                        if(value.target.value === 3) {
                                            setValue('valueDisbusement', String(valorSaldoDesembloso))
                                        }else if(value.target.value === 2){
                                            // cuenta individual
                                            setValue('valueDisbusement', String(valorSaldoCuentas))
                                        }else{
                                           
                                            const recursosProios = getValues('valueCanonInicial')
                                            setValue("valueDisbusement", String(recursosProios))
                                            // setValue('valueDisbusement', '0')
                                        }
                                        onChange(value)
                                    }}
                                >
                                    <MenuItem value="">Seleccionar</MenuItem>
                                    {resourceList.map((resource, index) => (
                                        <MenuItem
                                            key={`resource-${index}`}
                                            value={resource.idFuenteRecursos}
                                        >
                                            {resource.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    {/* <Grid item md={4} className={classes.column}>
                        <Controller
                            name="beneficiario"
                            control={control}
                            rules={{
                                required: 'Campo requerido Agrege Beneficiario ',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                
                                    error={!!error}
                                    label="Beneficiario"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    <MenuItem value="">Seleccionar</MenuItem>
                                    {CreditState.formularioVendedor?.map((resource: any) => (
                                        <MenuItem
                                            key={resource.id}
                                            value={resource.id}
                                        >
                                            {resource.nombre_Vendedor}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid> */}
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="beneficiario"
                            control={control}
                            rules={{
                                required: 'Campo requerido. Agrega un Beneficiario.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Beneficiario"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={(e) => {
                                        const selectedVendedorId = e.target.value;

                                        const selectedVendedor = CreditState.formularioVendedor?.find(
                                            (vendedor: any) => vendedor.id === selectedVendedorId
                                        );

                                        if (
                                            !selectedVendedor.nombre_Vendedor ||
                                            !selectedVendedor.ciudad ||
                                            !selectedVendedor.cuenta ||
                                            !selectedVendedor.correo_Electronico ||
                                            !selectedVendedor.departamento ||
                                            !selectedVendedor.direccion ||
                                            !selectedVendedor.fecha_Entrega ||
                                            !selectedVendedor.numero_Doc ||
                                            !selectedVendedor.soc_id ||
                                            !selectedVendedor.telefono ||
                                            !selectedVendedor.tipo_Cuenta ||
                                            !selectedVendedor.tipo_Doc
                                        ) {
                                           
                                            NotificationService.genericAlert(
                                                'Vendedor',
                                                'Datos Incompletos.',
                                                'warning'
                                            );
                                         return  
                                        }

                                        onChange(e);
                                    }}
                                >
                                    <MenuItem value="">Seleccionar</MenuItem>
                                    {CreditState.formularioVendedor?.map((resource: any) => (
                                        <MenuItem key={resource.id} value={resource.id}>
                                            {resource.nombre_Vendedor}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="valueDisbusement"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    label="Valor"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="date"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha"
                                    onChange={onChange}
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    {/* <Grid item md={2} className={classes.column}>
                    <Button onClick={handleSubmit2(processRows)}>
                        Agregar
                    </Button>
                </Grid> */}
                </Grid>
                <Button
                        variant="contained"
                        color="primary"
                        onClick={handleSubmit(processForm)}
                        className={classes.button}
                    >
                        GUARDAR
                    </Button>
               
              
              
            </CHPaper>
        )}
            { mostarPrimerPago && ocultarPrimerPago?
                <CHPaper title="Listado Recursos - Primer Pago">
                <Table>
                    <TableHead>
                        <TableCell style = {{width:"25%"}}>Tipo Fuente</TableCell>
                        <TableCell style = {{width:"25%"}}>Beneficiario Pago</TableCell>
                        <TableCell style = {{width:"25%"}}>Valor</TableCell>
                        <TableCell style = {{width:"25%"}}>Fecha</TableCell>
                        {
                             procedureRequest.flowActivityID === 28 ? null:
                             <TableCell style = {{width:"10%"}}>Acción</TableCell>
                        }
                        
                    </TableHead>
                    <TableBody>
                       
                        {datosCuotaInicial.map((dato: any) => (
                            
                            <TableRow key={`row-${dato.idSolicitudDesembolso}`}>
                                <TableCell>
                                    {dato.descripcionFuenteRecursos}
                                </TableCell>
                                <TableCell>
                                 { (CreditState.formularioVendedor.find((item: any) => item.id === dato.vendedor_Id))?.nombre_Vendedor}
                                </TableCell>
                                <TableCell>
                                    {UtilService.formatCurrency(
                                        dato.valorDesembolso,
                                    )}
                                </TableCell>

                                <TableCell>
                                    {UtilService.getDate(dato.fechaDesembolso)}
                                </TableCell>
                                <TableCell>
                                    {!isFinalizedActivity && mostarPrimerPago  && (
                                        <IconButton
                                            aria-label="remove"
                                            size="small"
                                            onClick={eliminar(dato)}
                                        >
                                            <DeleteIcon fontSize="inherit" />
                                        </IconButton>
                                    )}
                                    {/* {!isFinalizedActivity && mostarPrimerPago  && (
                                       <button
                                       onClick={ validacionSeven(dato)}
                                       style={{
                                           backgroundColor: 'red',   
                                           padding: '10px',           
                                           color: 'white',            
                                           border: 'none',            
                                           marginRight: '10px',       
                                           cursor: 'pointer',         // Cambia el cursor al pasar el ratón
                                           borderRadius: '5px',  
                                       }}
                                       >
                                       
                                       </button>
                                    )} */}
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CHPaper>: null
            }
            

            {mostarSegundoPago || ocultarPrimerPago ?

            <CHPaper title={segundoTitulo}>
                <Table>
                    <TableHead>
                       <TableCell style = {{width:"20%"}}>Tipo Fuente</TableCell>
                       <TableCell style = {{width:"20%"}}>Beneficiario Pago</TableCell>
                        <TableCell style = {{width:"20%"}}>Valor</TableCell>
                        <TableCell style = {{width:"20%"}}>Fecha</TableCell>
                        {
                             procedureRequest.flowActivityID === 28 ? null:
                             <TableCell style = {{width:"10%"}}>Acción</TableCell>
                        }
                       
                    </TableHead>
                    <TableBody>
                        {datosDesembolso.map((dato: any) => (
                            
                            <TableRow key={`row-${dato.idSolicitudDesembolso}`}>
                                <TableCell>
                                    {dato.descripcionFuenteRecursos}
                                </TableCell>
                                <TableCell>
                                { (CreditState.formularioVendedor.find((item: any) => item.id === dato.vendedor_Id))?.nombre_Vendedor}
                                </TableCell>
                                <TableCell>
                                    {UtilService.formatCurrency(
                                        dato.valorDesembolso,
                                    )}
                                </TableCell>

                                <TableCell>
                                    {UtilService.getDate(dato.fechaDesembolso)}
                                </TableCell>
                                {
                             procedureRequest.flowActivityID === 28 ? null:
                             <TableCell>
                             {!isFinalizedActivity && (
                                 <IconButton
                                     aria-label="remove"
                                     size="small"
                                     onClick={eliminar(dato)}
                                 >
                                     <DeleteIcon fontSize="inherit" />
                                 </IconButton>
                             )}
                         </TableCell>
                                }
                              
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CHPaper>: null
            }
           

        
                <BotoneraPagosDesembolso
                procedureRequest={procedureRequest}
                creditProductData={creditProductData}
                dataPrimerPago={dataPrimerPago}
                datosCuotaInicial = {datosCuotaInicial}
                datosDesembolso = {datosDesembolso}
                />
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    CreditState: state.credit,

});
const mapDispatchToProps = {
    startGetMovementsAccountsClient,
    startGetClientByIdentificationTypeIdentificationNumber,
    startObtenerVendedorCredito,
};
export default connect( mapStateToProps , mapDispatchToProps)(RequestInformationDisbursement);
