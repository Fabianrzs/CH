import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    MenuItem,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import { useForm, Controller } from 'react-hook-form';
import moment from 'moment';
import PictureAsPdfIcon from '@material-ui/icons/PictureAsPdf';
import { toast } from 'react-toastify';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import { AppState } from '../../store/rootReducer';
import { startGetCreditFinancialAnalystData } from '../../actions/credit.actions';
import { CreditProductModel } from '../../models/credit-product.model';
import { CreditLegalDataModel } from '../../models/credit-legal-data.model';
import { CreditFinancialAnalystRequestModel } from '../../models/requests/credit-financial-analyst-request.model';
import { CreditTechnicalDataRequestModel } from '../../models/requests/credit-technical-data-request.model';
import { CreditProductRequestModel } from '../../models/requests/credit-product-request.model';
import { CreditLegalDataRequestModel } from '../../models/requests/credit-legal-data-request.model';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import { CreditSimulationDataModel } from '../../models/credit-simulation-data.model';
import { CreditConsultCreditRequestModel } from '../../models/requests/credit-consult-credit-request.model';
import { CreditConsultCreditModel } from '../../models/credit-consult-credit.model';
import UtilService from '../../utils/util.service';
import { CreditUpdateRecommendationRequestModel } from '../../models/requests/credit-update-recommendation-request.model';
import AuthService from '../../services/auth.service';
import CreditApi from '../../services/api-credit.service';
import NotificationService from '../../services/notification.service';
import CHSelect from '../ch-select/CHSelect';
import CatalogApi from '../../services/api-catalog.service';
import ApiFichaAnalista from '../../services/api-fichaAnalista.sercive';
import CHPrivateContainer from '../ch-private-container/CHPrivateContainer';
import Prospeccion from '../../services/api-prospeccion.service';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
       
    }),
);

type Inputs = {
    creditRecommendedValue: string;
    recommendedObservation: string;
    reliefType: string;
};

const creditApi = CreditApi();
const fichaAnalistaApi = ApiFichaAnalista()

function RequestInformationTechnicalFinancial({
    procedureRequest,
    informacionAfiliado,
    creditTechnicalData,
    creditLegalData,
    creditProductData,
    creditSimulationData,
    startGetCreditFinancialAnalystData,
    creditConsultCreditData,
    isLoading,
}: {
    procedureRequest: ProcedureRequestModel;
    informacionAfiliado: any;
    creditProductData: CreditProductModel;
    creditLegalData: CreditLegalDataModel;
    creditTechnicalData: any;
    creditSimulationData: CreditSimulationDataModel;
    creditConsultCreditData: CreditConsultCreditModel;
    isLoading: boolean;
    startGetCreditFinancialAnalystData: (
        request: CreditFinancialAnalystRequestModel,
    ) => void;
}) {
    const classes = useStyles();
    const catalogApi = CatalogApi();
    const prospeccion = Prospeccion()
    

    const isFinalizedActivity = procedureRequest.stateActivityID === 4;
    const { control, handleSubmit, setValue } = useForm<Inputs>({
        defaultValues: {
            recommendedObservation: '',
            creditRecommendedValue: '',
            reliefType: '',
        },
    });

    const formularioFichaAlanista = useForm({
        defaultValues: {
            nombre: '',
            cargo: ''
        }
    })

    const dataRequestTechnical: CreditTechnicalDataRequestModel = {
        indicador: 2,
        solicitudCredito: procedureRequest.requestID,
    };
    const dataRequestProduct: CreditProductRequestModel = {
        ind: 2,
        soC_ID: procedureRequest.requestID,
    };
    const dataRequestLegal: CreditLegalDataRequestModel = {
        idSolicitudCredito: procedureRequest.requestID,
        indicador: 2,
    };
    const dataConsultCreditRequest: CreditConsultCreditRequestModel = {
        estado: '0',
        idSolicitudCredito: procedureRequest.requestID,
        identificacion: `${procedureRequest.clientIdentificationNumber}`,
    };

    const dataRequest: CreditFinancialAnalystRequestModel = {
        requestClientSimulation: `${procedureRequest.clientIdentificationNumber}`,
        requestLegalData: dataRequestLegal,
        requestProductData: dataRequestProduct,
        requestTechnicalData: dataRequestTechnical,
        requestConsultCredit: dataConsultCreditRequest,
    };

    const [noTipoAlivio, setNoTipoAlivio] = useState(true);
    const [porcentajeInmueble , setPorcentajeInmueble] = useState<number>(0);
    const [pdfNoviseble , setPdfNoviseble] = useState<boolean>(false);
    const [datosProductosDos, setDatosProductosDos]=useState<any>({})
    const [actulizarFA, setActulizarFA] = useState<boolean>(false)
    const [obtenerIdActualizar, setObtnerIdActilizar] = useState<number>(0)
    const [isLoaded, setIsLoaded] = useState(false);
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [descripcionTipoDocumento, setDescripcionTipoDocumento] = useState<any>('');

    useEffect(() => {
        if(procedureRequest.creditTypeID === 1){
            setNoTipoAlivio(false)
        }
       
    }, [procedureRequest]);
    
    useEffect(() => {
        consultarTipoDocumentos();
    }, [creditProductData]);

    useEffect(()=> {
            obtenerFichaAnalisticaPorId() 
    }, [ actulizarFA, setActulizarFA])

    useEffect (() => {
        if(creditProductData?.sopPropertyValue !== undefined && creditTechnicalData.commercialValue !== null ){

            if (creditProductData?.sopPropertyValue >= Number(creditTechnicalData.commercialValue)){
            setPorcentajeInmueble(creditProductData?.sopPropertyValue / creditProductData?.sopPropertyValue ) 
            }else{
            setPorcentajeInmueble(Number(creditTechnicalData.commercialValue) / creditProductData?.sopCreditValue)  
            }
        }
    },[])

    useEffect(()=>{
        obtenerDatosProspeccionAfiliado()
    },[])

    useEffect(() => {
        startGetCreditFinancialAnalystData(dataRequest);
    }, [startGetCreditFinancialAnalystData]);



    const onSubmit = async (data: Inputs) => {
        setIsLocalLoading(true);
        const user = AuthService.getUserInfo();
        const request: CreditUpdateRecommendationRequestModel = {
            idSolicitudCredito: procedureRequest.requestID,
            valorRecomendadoCredito: UtilService.cleanCurrencyValue(
                data.creditRecommendedValue,
            ),
            tipoAlivio: data.reliefType,
            actualizadoPor: user.USR_ID,
            fechaActualiza: moment().format(),
            observacionRecomendacion: data.recommendedObservation,
        };
        try {
            await creditApi.postUpdateRequestCreditRecommendation(request);
            setIsLocalLoading(false);
            await NotificationService.genericAlert(
                'Actualizado con éxito',
                'Se actualizó la solicitud.',
            );
            startGetCreditFinancialAnalystData(dataRequest);
        } catch (e) {
            console.error(e);
        }
    };

    const obtenerDatosProspeccionAfiliado = async () => {
         const request = String(procedureRequest?.clientIdentificationNumber)
        // const request = String(7181365)
        await prospeccion
            .getConsultarDatosProspeccionByNumeroDocumento(request)
            .then((response: any) => {
                
                if (response?.data) {
                   
                    setDatosProductosDos(response?.data)
                }
            })

            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            })
    }

    const consultarTipoDocumentos = async () => {
        await catalogApi
            .getAllTypeDocuments()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        const documento = response.data.find(
                            (item: any) =>
                                item.id === creditProductData.tidSellerID,
                        );
                        if (documento !== undefined) {
                            setDescripcionTipoDocumento(documento.descripcion);
                        }
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const getResumePDF = async () => {
        try {
            setIsLocalLoading(true);
            const data = await creditApi.postObtainPDFResume(
                procedureRequest.requestID,
            );
            const mimeType = UtilService.getMimeTypeFromExtension('pdf');
            if (mimeType) {
                const file = new Blob([data], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                setIsLocalLoading(false);
                window.open(fileURL);
            }
        } catch (e) {
            console.error(e);
            setIsLocalLoading(false);
            await NotificationService.genericAlert(
                'No se pudo descargar el PDF',
                'Se encontro un error y no se pudo descargar el PDF. Por favor intenta de nuevo',
                'error',
            );
        }
    };

    if (creditConsultCreditData.requestCreditID != null && !isLoaded) {
        setIsLoaded(true);
        const fields = Object.keys(creditConsultCreditData);
        fields.forEach(field => {
            const value =
                creditConsultCreditData[
                    field as keyof CreditConsultCreditModel
                ];
            if (value != null) {
                setValue(field as any, value);
            }
        });
        setValue('reliefType', creditConsultCreditData.reliefType);
    }

    const obtenerFichaAnalisticaPorId = async() => {
        
        const response = {
            soc_id: procedureRequest.requestID,
            fa_Id: 0
        }
        await fichaAnalistaApi
            .postObtenerFichaAnalistaPorId(response)
            .then(async (response: any) => {
                if (response) {
                   if(response.data.length !==0){
                    setActulizarFA(true)
                    setObtnerIdActilizar(response.data[0].fa_Id)
                       formularioFichaAlanista.setValue('nombre', response.data[0].fa_Nombre)
                       formularioFichaAlanista.setValue('cargo', response.data[0].fa_Cargo)
                   }
                    
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }
   
    const enviar = async (data: any) => {
     
        const response = {
            fa_Id: 0,
            soc_id: procedureRequest.requestID,
            fa_Nombre: data.nombre,
            fa_Cargo: data.cargo
        }
        await fichaAnalistaApi
            .posGuardarFichaAnalista(response)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setPdfNoviseble(true)
                        setActulizarFA(true)
                        toast('Se guardo con exíto', { type: toast.TYPE.SUCCESS });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const actualizar = async (data: any) => {
        const response = {
            fa_Id: obtenerIdActualizar,
            soc_id: procedureRequest.requestID,
            fa_Nombre: data.nombre,
            fa_Cargo: data.cargo
        }
        await fichaAnalistaApi
            .posGuardarFichaAnalista(response)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setPdfNoviseble(true)
                        toast('Se actualizó con exíto', { type: toast.TYPE.SUCCESS });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }
  
    if (isLoading ) {
        return (
            <CHPrivateContainer>
                <CHLoadingProcess />
            </CHPrivateContainer>
        );
    }

    return (
        <>
        {
           pdfNoviseble && procedureRequest.generateResumePDF ?  (
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={getResumePDF}
                        style={{
                            width: '300px',
                            marginBottom: '1rem',
                        }}
                        startIcon={<PictureAsPdfIcon />}
                    >
                        Descargar Informe
                    </Button>
                
           ):''
        }

            <CHPaper title="Ficha Analista">
                <Grid container>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="nombre"
                            control={formularioFichaAlanista.control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    label="Nombre *"
                                    value={value}
                                    multiline
                                    maxRows={4}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="cargo"
                            control={formularioFichaAlanista.control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    label="Cargo *"
                                    value={value}
                                    multiline
                                    maxRows={4}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    {isFinalizedActivity  && !actulizarFA ? 
                        <Grid item md={6}>
                            <Button
                                style={{ background: '#1C3F8B', color: 'white' }}

                                onClick={formularioFichaAlanista.handleSubmit(enviar)}>
                                enviar
                            </Button>
                        </Grid>: 
                          <Grid item md={6}>
                          <Button
                              style={{ background: '#1C3F8B', color: 'white' }}

                              onClick={formularioFichaAlanista.handleSubmit(actualizar)}>
                              Actualizar
                          </Button>
                      </Grid>
                    }
                </Grid>
            </CHPaper>

            <CHPaper title="Datos Adicionales">
                <Grid container>
                    {
                       !noTipoAlivio ? '':
                        <Grid item md={4} className={classes.column}>
                        <Controller
                            name="reliefType"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de Alivio *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    <MenuItem value="No Aplica">
                                        NO APLICA
                                    </MenuItem>
                                    <MenuItem value="Cuota">CUOTA</MenuItem>
                                    <MenuItem value="Tasa Frech">
                                        TASA FRECH
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    }
                    
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="creditRecommendedValue"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={isFinalizedActivity}
                                    currencyMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Valor Crédito Recomendado *"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="recommendedObservation"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    label="Observación Recomendación *"
                                    value={value}
                                    multiline
                                    maxRows={4}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    {!isFinalizedActivity && (
                        <Grid   item md={6}>
                            <Button 
                                style={{background:'#1C3F8B', color:'white'}}
                          
                             onClick={handleSubmit(onSubmit)}>
                                Guardar
                            </Button>
                        </Grid>
                    )}
                </Grid>
            </CHPaper>
            <CHPaper title="Datos Estudio Financiero">
                <Grid container>
                    <Grid item md={12}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>N. Matricula</TableCell>
                                    <TableCell>Cédula Catastral</TableCell>
                                    <TableCell>
                                        Area Privada Construida
                                    </TableCell>
                                    <TableCell>
                                        Valor Avalúo Comercial
                                    </TableCell>
                                    <TableCell>
                                        Valor Avalúo Catastral
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                              
                                {creditTechnicalData?.creditTechnicalPropertyInformationData.map(
                                    (credit:any, key:any) => (
                                        <TableRow key={`realState-${key}`}>
                                            <TableCell>
                                                {credit.realStateRegistrationNumber.toUpperCase()}
                                            </TableCell>
                                            <TableCell>
                                                {credit.cadastralID.toUpperCase()}
                                            </TableCell>
                                            <TableCell>
                                                {creditTechnicalData?.areaPrivada}
                                            </TableCell>
                                            <TableCell
                                                style={{ textAlign: 'center' }}
                                            >
                                                {UtilService.formatCurrency(
                                                    creditTechnicalData?.commercialValue,
                                                )}
                                            </TableCell>
                                            <TableCell
                                                style={{ textAlign: 'center' }}
                                            >
                                                {UtilService.formatCurrency(
                                                    creditTechnicalData?.catastralValue,
                                                )}
                                            </TableCell>
                                        </TableRow>
                                    ),
                                )}
                            </TableBody>
                        </Table>
                    </Grid>
                </Grid>
            </CHPaper>
            <CHPaper title="Información vendedor del inmueble">
                <Grid container>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo de documento"
                            value={descripcionTipoDocumento}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Número documento"
                            value={creditProductData.sopSellerIDNumber || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Nombre o razón social"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            value={creditProductData.sopSellerName || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Dirección"
                            value={creditProductData.sopSellerAddress || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Departamento"
                            value={creditProductData.propertyState || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Ciudad"
                            value={creditProductData.propertyCity || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Correo electrónico"
                            value={creditProductData.sopSellerEmail || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Teléfono"
                            value={creditProductData.sopSellerPhone || ''}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <CHPaper title="Información Tecnica">
                <Grid container>
                    <Grid item className={classes.column} xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            currencyMask
                            label="Valor Avaluo Comercial"
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            value={creditTechnicalData.commercialValue || 0}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Nombre Aliado Técnico"
                            value={creditTechnicalData.createdName || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={12}>
                        <CHTextField
                            isReadOnly
                            label="Concepto Avalúo"
                            value={creditTechnicalData.conceptValue || ''}
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <CHPaper title="Información Juridica">
                <Grid container>
                    <Grid item className={classes.column} xs={12} md={4}>
                        <CHTextField
                            isReadOnly
                            label="Dirección del Inmueble"
                            value={creditProductData.sopSellerAddress || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={4}>
                        <CHTextField
                            isReadOnly
                            label="Departamento del Inmueble"
                            value={creditProductData.propertyState || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={4}>
                        <CHTextField
                            isReadOnly
                            label="Ciudad del Inmueble"
                            value={creditProductData.propertyCity || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Edad Jurídica Aproximada"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            value={creditLegalData.legalAge || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Nombre Aliado Juridico"
                            value={creditLegalData.createdName || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Concepto juridico analista"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            value={creditLegalData.legalConceptEnd || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Concepto del estudio jurídico"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            value={creditLegalData.legalConceptEst || ''}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <CHPaper title="Información pre-aprobado">
                <Grid container>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMask
                            label="Valor inmueble"
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            value={creditProductData?.sopPropertyValue || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMask
                            label="Valor a Financiar"
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            value={creditProductData?.sopCreditValue || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo Vivienda"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            value={creditProductData?.propertyType || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo Alivio"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            value={creditSimulationData.reliefType || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo Abono"
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            value={creditSimulationData.tipoAbono || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="% Tasa EA"
                            value={creditProductData?.tasaAnual || ''}
                            percentMask
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="% Tasa mes vencido"
                            value={creditProductData?.tasaMensual || ''}
                            percentMask
                        />
                    </Grid>
                  
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMask
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="Valor cuota total"
                            value={creditProductData?.valorCuotaTotal || ''}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMask
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="Valor cuota sin seguros"
                            value={
                                creditProductData?.cuotaTotalSinSeguros || ''
                            }
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Número PreAprobado"
                            value={creditProductData?.numeroPreaprobado || ''}
                            InputLabelProps={{ shrink: true }}
                        />
                    </Grid>

                    <Grid item className={classes.column} xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                percentMask
                                 inputProps={{
                                     style: {
                                         textAlign: 'right',
                                     },
                                 }}
                                label="% Porcentaje de financiación"
                                InputLabelProps={{ shrink: true }}
                                value={creditProductData?.porcentajeFinanciacion || 0 }  
                            />
                     </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                percentMask
                                 inputProps={{
                                     style: {
                                         textAlign: 'right',
                                     },
                                 }}
                                 InputLabelProps={{ shrink: true }}
                                label="Endeudamiento global"
                                value={datosProductosDos.edP_ENDUEDAMIENTO_ACTUAL_CUOTA_PORC || 0 }  
                            />
                     </Grid>
                     <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            currencyMask
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="Subsidio"
                            value={informacionAfiliado?.numeroCuotas !== 168? informacionAfiliado?.valorSubsidio : 0}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            InputLabelProps={{ shrink: true }}
                            label="Modalidad de Pago"
                            value='MES VENCIDO'
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            currencyMask
                            isReadOnly
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="Total aportado afiliado"
                            value={0}
                        />
                    </Grid>
                    <Grid item className={classes.column} xs={12} md={3}>
                        <CHTextField
                            currencyMask
                            isReadOnly
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                },
                            }}
                            InputLabelProps={{ shrink: true }}
                            label="Recursos cuenta individual"
                            value={informacionAfiliado?.saldoDisponible}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    // isLoading: state.credit.loading,
    creditTechnicalData: state.credit.creditTechnicalData,
    creditLegalData: state.credit.creditLegalData,
    creditProductData: state.credit.creditProductData,
    creditSimulationData: state.credit.creditSimulationData,
    creditConsultCreditData: state.credit.creditConsultCreditData,
});

const mapDispatchToProps = {
    startGetCreditFinancialAnalystData,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationTechnicalFinancial);