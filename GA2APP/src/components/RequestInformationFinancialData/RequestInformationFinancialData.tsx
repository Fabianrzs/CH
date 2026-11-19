import React, { ReactElement, useEffect, useState } from 'react';
import { Button, FormLabel, Grid, MenuItem } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import { useForm, Controller } from 'react-hook-form';
import moment from 'moment';
import { toast } from 'react-toastify';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import { CreditFinancialDataModel } from '../../models/credit-financial-data.model';
import { CreditFinancialDataRequestModel } from '../../models/requests/credit-financial-data-request.model';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import { CreditBasicDataRequestModel } from '../../models/requests/credit-basic-data-request.model';
import { CreditBasicDataModel } from '../../models/credit-basic-data.model';
import { AppState } from '../../store/rootReducer';
import { startGetPayrollCategoryConcept } from '../../actions/catalog.action';
import {
    startPostCreditBasicData,
    startPostCreditFinancialData,
} from '../../actions/credit.actions';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import CHSelect from '../ch-select/CHSelect';
import UtilService from '../../utils/util.service';
import CreditApi from '../../services/api-credit.service';
import AuthService from '../../services/auth.service';
import NotificationService from '../../services/notification.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

function RequestInformationFinancialData({
    financialData,
    startPostCreditFinancialData,
    procedureRequest,
    startPostCreditBasicData,
    creditBasicData,
    startGetPayrollCategoryConcept,
    payrollCategoriesConcept,
    isLoadingCatalog,
    isLoading
}: {
    financialData: CreditFinancialDataModel;
    procedureRequest: ProcedureRequestModel;
    startPostCreditFinancialData: (
        data: CreditFinancialDataRequestModel,
    ) => void;
    startPostCreditBasicData: (data: CreditBasicDataRequestModel) => void;
    creditBasicData: CreditBasicDataModel;
    startGetPayrollCategoryConcept: (data: any) => void;
    payrollCategoriesConcept: any[];
    isLoadingCatalog: boolean;
    isLoading: boolean;
}): ReactElement {
    const classes = useStyles();
    const api = CreditApi();
    const [concepts, setConcepts] = useState<any[]>([]);
    const [isLoadingDos, setIsLoadingDos] = useState<boolean>(false);
    const [sumarValores, setSumarValores] = useState(0);
    const [totalDescuentos, setTotalDescuentos] = useState(0);
    const [valorDescuento, setValorDescuento] = useState(0);
    const isFinalizedActivity = procedureRequest.stateActivityID === 4;
    
    const [estadoIngreso1, setEstadoIngreso1] = useState(0);
    const [estadoIngreso2, setEstadoIngreso2] = useState(0);
    const [estadoIngreso3, setEstadoIngreso3] = useState(0);
    const [estadoIngreso4, setEstadoIngreso4] = useState(0);
    const [estadoIngreso5, setEstadoIngreso5] = useState(0);
    const [estadoIngreso6, setEstadoIngreso6] = useState(0);
    const [CapacidadPago, setCapacidadPago] = useState(0);
    const [totalNeto, setTotalNeto] = useState(0);
    const [estadoSuma, setEstadoSuma] = useState(0);
    const [egresosTotales, setEgresosTotales] = useState(0);
    const [salarioMinimo, setSalarioMinimo] = useState(0);


  
    const formuarioFinanciera = useForm({
        defaultValues: {
            desSalario: 1,
            ddlDevengado2: '',
            ddlDevengado3: '',
            ddlDevengado4: '',
            ddlDevengado5: '',
            ddlDevengado6: '',
            totalEgresos: "0",
            total: '',
            capacidadPago:'0.00',
            ingreso1: 0,
            ingreso2: 0,
            ingreso3: 0,
            ingreso4: 0,
            ingreso5: 0,
            ingreso6: 0,
        },
    });


    const [selectDataFinancieraDos, setSelectDataFinancieraDos] = useState<
        any[]
    >([]);
    const [selectDataFinancieraTres, setSelectDataFinancieraTres] = useState<
        any[]
    >([]);
    const [selectDataFinancieraCuatro, setSelectDataFinancieraCuatro] =
        useState<any[]>([]);
    const [selectDataFinancieraCinco, setSelectDataFinancieraCinco] = useState<
        any[]
    >([]);

    useEffect(() => {
        setConcepts(payrollCategoriesConcept);
    }, [setConcepts, payrollCategoriesConcept]);

    useEffect(() => {
        const data: CreditFinancialDataRequestModel = {
            ind: 2,
            soC_ID: procedureRequest.requestID,
        };
        const dataRequest: CreditBasicDataRequestModel = {
            ind: 2,
            soC_ID: procedureRequest.requestID,
        };
        

        startPostCreditFinancialData(data);
        startPostCreditBasicData(dataRequest);

        api.postCreditFinancialData(data)
            .then(async response => {
                if(response.data === null){
                    toast('No tiene datos financieros', { type: toast.TYPE.ERROR })
                }else if (response) {
                        if (response?.isSuccess) {

                            formuarioFinanciera.setValue('ingreso1', response?.data.soF_VALOR_CONCEPTO_SALARIO      ,);
                            formuarioFinanciera.setValue('ingreso2', response?.data.soF_VALOR_CONCEPTO_1 ,);
                            formuarioFinanciera.setValue('ingreso3', response?.data.soF_VALOR_CONCEPTO_2 ,);
                            formuarioFinanciera.setValue('ingreso4', response?.data.soF_VALOR_CONCEPTO_3 ,);
                            formuarioFinanciera.setValue('ingreso5', response?.data.soF_VALOR_CONCEPTO_4 ,);
                            formuarioFinanciera.setValue('ingreso6', response?.data.soF_VALOR_CONCEPTO_5 ,);
                            
                            formuarioFinanciera.setValue('ddlDevengado2', response?.data.soF_CONCEPTO_1,);
                            formuarioFinanciera.setValue('ddlDevengado3', response?.data.soF_CONCEPTO_2,);
                            formuarioFinanciera.setValue('ddlDevengado4', response?.data.soF_CONCEPTO_3,);
                            formuarioFinanciera.setValue('ddlDevengado5', response?.data.soF_CONCEPTO_4,);
                            formuarioFinanciera.setValue('ddlDevengado6', response?.data.soF_CONCEPTO_5,);    
                            formuarioFinanciera.setValue('totalEgresos', response?.data.soF_VALOR_TOTAL_EGRESOS,);    

                            
                            setEstadoSuma (response?.data.soF_VALOR_TOTAL_INGRESOS ,);
                            setEstadoIngreso1(response?.data.soF_VALOR_CONCEPTO_SALARIO)
                            setEstadoIngreso2(response?.data.soF_VALOR_CONCEPTO_1)
                            setEstadoIngreso3(response?.data.soF_VALOR_CONCEPTO_2)
                            setEstadoIngreso4(response?.data.soF_VALOR_CONCEPTO_3)
                            setEstadoIngreso5(response?.data.soF_VALOR_CONCEPTO_4)
                            setEstadoIngreso6(response?.data.soF_VALOR_CONCEPTO_5)
    
                        }
                    }
                    
                
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [startPostCreditFinancialData, startPostCreditBasicData]);

    useEffect (() => {
        setEstadoSuma(
            estadoIngreso1 + estadoIngreso2 +
            estadoIngreso3 + estadoIngreso4 +
            estadoIngreso5 + estadoIngreso6
        )
        
    },[estadoIngreso1, estadoIngreso2, estadoIngreso3, estadoIngreso4, estadoIngreso5, estadoIngreso6]);

    useEffect(() => {
        const totalConDescuentos = sumarValores - valorDescuento;
        setTotalDescuentos(totalConDescuentos);
    }, [valorDescuento]);

    


    const deshabilitar = formuarioFinanciera.watch('ddlDevengado2') === 'Seleccione';
    const deshabilitar3 = formuarioFinanciera.watch('ddlDevengado3') === 'Seleccione';
    const deshabilitar4 = formuarioFinanciera.watch('ddlDevengado4') === 'Seleccione';
    const deshabilitar5 = formuarioFinanciera.watch('ddlDevengado5') === 'Seleccione';
    const deshabilitar6 = formuarioFinanciera.watch('ddlDevengado6') === 'Seleccione';
    const habilitar2 =  formuarioFinanciera.watch('ddlDevengado2') !== 'Seleccione';
    const habilitar3 =  formuarioFinanciera.watch('ddlDevengado3') !== 'Seleccione';
    const habilitar4 =  formuarioFinanciera.watch('ddlDevengado4') !== 'Seleccione';
    const habilitar5 =  formuarioFinanciera.watch('ddlDevengado5') !== 'Seleccione';


    useEffect(() => {
        if (creditBasicData.accID != null) {
            const data = {
                idCategoria: creditBasicData.ctgID,
            };
            startGetPayrollCategoryConcept(data);
        }
    }, []);

    const getConceptDescription = (concept: any): string => {
        let conceptString = '';
        if (concept != null) {
            conceptString = `${concept.id} - ${concept.descripcion}`;
        }
        return conceptString;
    };


    const uploadFile = async (data: any) => {
        setIsLoadingDos(true);
     
        try {
            const user = AuthService.getUserInfo();       
       
            const dataAdapted = {
                idSolicitudCredito: procedureRequest.requestID,
                idSof: financialData.sofID,
                idSimulacion: procedureRequest.simulationID,
              
                descripcionSalario: getConceptDescription(concepts.find(concept => concept.id === 1),),
                valorSalario: estadoIngreso1,
                descripcionOtroIng1: data.ddlDevengado2,
                valorOtrosIngresos1: estadoIngreso2,
                descripcionOtroIng2: data.ddlDevengado3,
                valorOtrosIngresos2: estadoIngreso3,
                descripcionOtroIng3: data.ddlDevengado4,
                valorOtrosIngresos3: estadoIngreso4,
                descripcionOtroIng4: data.ddlDevengado5,
                valorOtrosIngresos4: estadoIngreso5,
                descripcionOtroIng5: data.ddlDevengado6,
                valorOtrosIngresos5: estadoIngreso6,
                valorTotalIngresos:  estadoSuma,
                valorTotalEgresos: UtilService.cleanCurrencyValue(data.totalEgresos,),



    
                fechaActualiza: moment().format(),
                fechaCreacion: moment().format(),
                creadoPor: user.USR_ID,
                usuarioActualiza: user.USR_ID,
            };
            const api = CreditApi();
            const response = await api.postFinancialData(dataAdapted);
            setIsLoadingDos(false);
            if (response.isSuccess == null) {
                await NotificationService.genericAlert(
                    'Error',
                    'Ocurrio un error al actualizar intentar nuevamente.',
                );
            } else {
                await NotificationService.genericAlert(
                    'Información actualizada',
                    'Se actualizó la información de manera correcta.',
                );
            }
        } catch (e) {
            setIsLoadingDos(false);
            console.error(e);
        }
    };

    if (isLoadingCatalog || isLoadingDos || isLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <>
            <CHPaper title="Información Financiera Solicitud">
                <Grid container>
                    <Grid item md={6} xs={12} className={classes.column}>
                        <Controller
                            name="desSalario"
                            control={formuarioFinanciera.control}
                     
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                error={!!error}
                                label=""
                                value={value}
                                helperText={
                                    error ? error.message : null
                                }
                                onChange={value => {
                                    formuarioFinanciera.setValue(
                                        'desSalario',
                                        1,
                                    );
                                    onChange(value);
                                }}
                                readOnly
                                >
                                  
                                    {concepts.map(
                                        (item: any, index: number) => (
                                            <MenuItem
                                                key={index}
                                                value={item.id}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                        <Controller
                            name="ddlDevengado2"
                            control={formuarioFinanciera.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                error={!!error}
                                label=""
                           
                                value={value}
                                helperText={
                                    error ? error.message : null
                                }
                                onChange={value => {
                                    
                                    const array =
                                    concepts.filter(
                                            item =>
                                                item.descripcion !==
                                                value
                                                    .target
                                                    .value,
                                        );
                                    setSelectDataFinancieraDos(
                                        array,
                                    );
                                
                               
                                    formuarioFinanciera.setValue(
                                        'ddlDevengado3',
                                        'Seleccione',
                                    );
                                    formuarioFinanciera.setValue(
                                        'ddlDevengado4',
                                        'Seleccione',
                                    );
                                    formuarioFinanciera.setValue(
                                        'ddlDevengado5',
                                        'Seleccione',
                                    );
                                    formuarioFinanciera.setValue(
                                        'ddlDevengado6',
                                        'Seleccione',
                                    );

                                    formuarioFinanciera.setValue('ingreso2', 0);
                                    formuarioFinanciera.setValue('ingreso3', 0);
                                    formuarioFinanciera.setValue('ingreso4', 0);
                                    formuarioFinanciera.setValue('ingreso5', 0);
                                    formuarioFinanciera.setValue('ingreso6', 0);
                                    setEstadoIngreso2(0)
                                    setEstadoIngreso3(0)
                                    setEstadoIngreso4(0)
                                    setEstadoIngreso5(0)
                                    setEstadoIngreso6(0)

                                    onChange(value);
                                }}
                                
                            >
                                     <MenuItem
                                                key="Seleccione"
                                                value="Seleccione"
                                            >
                                                --SELECCIONE--
                                            </MenuItem>
                                    {concepts.map(
                                        (item: any, index: number) => (
                                            <MenuItem
                                                key={index}
                                                value={item.descripcion}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                        <Controller
                            name="ddlDevengado3"
                            control={formuarioFinanciera.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                            error={!!error}
                                            label=""
                                            disabled={!habilitar2}
                                            value={value}
                                            helperText={
                                                error ? error.message 
                                                : null
                                            }
                                            onChange={value => {
                                                const array =
                                                    selectDataFinancieraDos.filter(
                                                        item =>
                                                            item.descripcion !==
                                                            value
                                                                .target
                                                                .value,
                                                    );
                                                setSelectDataFinancieraTres(
                                                    array,
                                                );
                                             
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado3',
                                                    'Seleccione',
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado4',
                                                    'Seleccione',
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado5',
                                                    'Seleccione',
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado6',
                                                    'Seleccione',
                                                );

                                        
                                                formuarioFinanciera.setValue('ingreso3', 0);
                                                formuarioFinanciera.setValue('ingreso4', 0);
                                                formuarioFinanciera.setValue('ingreso5', 0);
                                                formuarioFinanciera.setValue('ingreso6', 0);
                                                setEstadoIngreso3(0)
                                                setEstadoIngreso4(0)
                                                setEstadoIngreso5(0)
                                                setEstadoIngreso6(0)

                                                onChange(value);
                                            }}
                                            
                                        >
                                      <MenuItem
                                                key="Seleccione"
                                                value="Seleccione"
                                            >
                                                --SELECCIONE--
                                            </MenuItem>
                                    {concepts.map(
                                        (item: any, index: number) => (
                                            <MenuItem
                                                key={index}
                                                value={item.descripcion}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                        <Controller
                            name="ddlDevengado4"
                            control={formuarioFinanciera.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                            error={!!error}
                                            label=""
                                            disabled={
                                                !habilitar3
                                            }
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={value => {
                                                const array =
                                                    selectDataFinancieraTres.filter(
                                                        item =>
                                                            item.descripcion !==
                                                            value
                                                                .target
                                                                .value,
                                                    );
                                                setSelectDataFinancieraCuatro(
                                                    array,
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado4',
                                                    'Seleccione',
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado5',
                                                    'Seleccione',
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado6',
                                                    'Seleccione',
                                                );
                                                
                                                formuarioFinanciera.setValue('ingreso4', 0);
                                                formuarioFinanciera.setValue('ingreso5', 0);
                                                formuarioFinanciera.setValue('ingreso6', 0);
                                       
                                                setEstadoIngreso4(0)
                                                setEstadoIngreso5(0)
                                                setEstadoIngreso6(0)

                                                onChange(value);
                                            }}
                                            
                                        >
                                    <MenuItem
                                                key="Seleccione"
                                                value="Seleccione"
                                            >
                                                --SELECCIONE--
                                            </MenuItem>
                                    {concepts.map(
                                        (item: any, index: number) => (
                                            <MenuItem
                                                key={index}
                                                value={item.descripcion}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                            
                        <Controller
                            name="ddlDevengado5"
                            control={formuarioFinanciera.control}
                      

                            render={({
                                field: { onChange, value },
                                
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                            error={!!error}
                                            label=""
                                            value={value}
                                            disabled={!habilitar4}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={value => {
                                                if (value.target.value ==='Seleccione') {

                                                    formuarioFinanciera.setValue('ingreso5', 0);
                                                    setEstadoIngreso5(0)
                                                
                                         
                                                }
                                                const array =
                                                    selectDataFinancieraCuatro.filter(
                                                        item =>
                                                            item.descripcion !==
                                                            value
                                                                .target
                                                                .value,
                                                    );
                                                setSelectDataFinancieraCinco(
                                                    array,
                                                );
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado6',
                                                    'Seleccione',
                                                );

                                                formuarioFinanciera.setValue('ingreso5', 0);
                                                formuarioFinanciera.setValue('ingreso6', 0);
                                                setEstadoIngreso5(0)
                                                setEstadoIngreso6(0)
                                            
                                                onChange(value);
                                            }}
                                            
                                                                                  
                                        >
                                    <MenuItem
                                                key="Seleccione"
                                                value="Seleccione"
                                            >
                                                --SELECCIONE--
                                            </MenuItem>
                                    {concepts.map(
                                        (item: any, index: number) => (
                                            <MenuItem
                                                key={index}
                                                value={item.descripcion}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                        <Controller
                            name="ddlDevengado6"
                            control={formuarioFinanciera.control}
                   

                            render={({
                                field: { onChange, value },
                                
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                            error={!!error}
                                            label=""
                                            value={value}
                                            disabled={!habilitar5}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={value => {
                                                if (value.target.value ==='Seleccione') {

                                                    formuarioFinanciera.setValue('ingreso6', 0);    
                                                  
                                                    setEstadoIngreso6(0)                                                            }
                                                formuarioFinanciera.setValue(
                                                    'ddlDevengado6',
                                                    'Seleccione',
                                                );
                                                onChange(value);
                                            }}
                                            
                                        >
                                  <MenuItem
                                                key="Seleccione"
                                                value="Seleccione"
                                            >
                                                --SELECCIONE--
                                            </MenuItem>
                                    {concepts.map(
                                        (item: any, index: number) => (
                                            <MenuItem
                                                key={index}
                                                value={item.descripcion}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                      
                    </Grid>
                    <Grid item md={6} xs={12} className={classes.column}>
                      
                
                        <Controller
                           name="ingreso1"
                           control={formuarioFinanciera.control}
                           render={({
                               field: {onChange,  value, },
                        
                        }) => (
                         <CHTextField
                                    label=""
                                    value={value}
                                    currencyMask
                                    inputProps={{
                                        style: {textAlign: 'right' },
                                    }}
                                    InputLabelProps={{shrink: true}} 
                                    onChange={value => {
                                        const valor1 = UtilService.cleanCurrencyValue(value.target.value)
                                        setEstadoIngreso1(valor1)
                                        onChange(valor1)
                                    }}
                                />
                            )}
                        />
                        <Controller
                           name="ingreso2"
                           control={formuarioFinanciera.control}
                           render={({
                               field: {onChange,  value, },
                        
                        }) => (
                         <CHTextField
                                    label=""
                                    value={value}
                                    disabled={deshabilitar}
                                    currencyMask
                                    inputProps={{
                                        style: {textAlign: 'right' },
                                    }}
                                    InputLabelProps={{shrink: true}} 
                                    onChange={value => {
                                        const valor1 = UtilService.cleanCurrencyValue(value.target.value)
                                        setEstadoIngreso2(valor1)
                                        onChange(valor1)
                                    }}
                                />
                            )}
                        />
                        <Controller
                           name="ingreso3"
                           control={formuarioFinanciera.control}
                           render={({
                               field: {onChange,  value, },
                        
                        }) => (
                         <CHTextField
                                    label=""
                                    value={value}
                                    disabled={deshabilitar3}
                                    currencyMask
                                    inputProps={{
                                        style: {textAlign: 'right' },
                                    }}
                                    InputLabelProps={{shrink: true}} 
                                    onChange={value => {
                                        const valor1 = UtilService.cleanCurrencyValue(value.target.value)
                                        setEstadoIngreso3(valor1)
                                        onChange(valor1)
                                    }}
                                />
                            )}
                        />
                        <Controller
                           name="ingreso4"
                           control={formuarioFinanciera.control}
                           render={({
                               field: {onChange,  value, },
                        
                        }) => (
                         <CHTextField
                                    label=""
                                    value={value}
                                    disabled={deshabilitar4}
                                    currencyMask
                                    inputProps={{
                                        style: {textAlign: 'right' },
                                    }}
                                    InputLabelProps={{shrink: true}} 
                                    onChange={value => {
                                        const valor1 = UtilService.cleanCurrencyValue(value.target.value)
                                        setEstadoIngreso4(valor1)
                                        onChange(valor1)
                                    }}
                                />
                            )}
                        />
                        <Controller
                           name="ingreso5"
                           control={formuarioFinanciera.control}
                           render={({
                               field: {onChange,  value, },
                        
                        }) => (
                         <CHTextField
                                    label=""
                                    value={value}
                                    disabled={deshabilitar5}
                                    currencyMask
                                    inputProps={{
                                        style: {textAlign: 'right' },
                                    }}
                                    InputLabelProps={{shrink: true}} 
                                    onChange={value => {
                                        const valor1 = UtilService.cleanCurrencyValue(value.target.value)
                                        setEstadoIngreso5(valor1)
                                        onChange(valor1)
                                    }}
                                />
                            )}
                        />
                        <Controller
                           name="ingreso6"
                           control={formuarioFinanciera.control}
                           render={({
                               field: {onChange,  value, },
                        
                        }) => (
                         <CHTextField
                                    label=""
                                    value={value}
                                    disabled={deshabilitar6}
                                    currencyMask
                                    inputProps={{
                                        style: {textAlign: 'right' },
                                    }}
                                    InputLabelProps={{shrink: true}} 
                                    onChange={value => {
                                        const valor1 = UtilService.cleanCurrencyValue(value.target.value)
                                        setEstadoIngreso6(valor1)
                                        onChange(valor1)
                                    }}
                                />
                            )}
                        />


                    </Grid>

                    <Grid item md={6} xs={12} className={classes.column}>
                        <b>Total</b>
                    </Grid>
                    <Grid item md={6} xs={12} className={classes.column}>
                       
                                <CHTextField
                                    label=""
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            fontWeight: 'bold',
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={estadoSuma}
                                    currencyMask
                             
                                 />
                    </Grid>
                    <Grid item md={6} xs={12} className={classes.column}>
                        <b>Descuentos</b>
                    </Grid>
                    <Grid item md={6} xs={12} className={classes.column}>
                        <Controller
                            name="totalEgresos"
                            control={formuarioFinanciera.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                            label=""
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            value={value}
                                            onChange={(e: {
                                                target: { value: any };
                                            }) => {
                                                setValorDescuento(
                                                    Number(
                                                        UtilService.cleanCurrencyValue(
                                                            e.target.value,
                                                        ),
                                                    ),
                                                );
                                                onChange(e);
                                            }}
                                            currencyMask
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                            }}
                                        />
                            )}
                        />
                    </Grid>
 
                </Grid>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={formuarioFinanciera.handleSubmit(uploadFile)}
                >
                    Actualizar
                </Button>
            </CHPaper>
 
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    payrollCategoriesConcept: state.catalog.payrollCategoriesConcept,
    isLoadingCatalog: state.catalog.loading,
    creditBasicData: state.credit.creditBasicData,
    financialData: state.credit.creditFinancialData,
});

const mapDispatchToProps = {
    startGetPayrollCategoryConcept,
    startPostCreditBasicData,
    startPostCreditFinancialData,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationFinancialData);
