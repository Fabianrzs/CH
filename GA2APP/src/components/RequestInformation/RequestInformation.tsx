import React, { useEffect, useState } from 'react';
import { Box, Button, Grid, IconButton, Tab, Tabs, withStyles } from '@material-ui/core';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import moment from 'moment';
import { Control, Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AttachFile } from '@material-ui/icons';
import { useHistory } from 'react-router-dom';
import CHPageTitle from '../ch-page-title/CHPageTitle';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import UtilService from '../../utils/util.service';
import { RequestActivityRequestModel } from '../../models/requests/request-activity-request.model';
import {
    REQUEST_ACTIVITY_PROCESS,
    REQUEST_ACTIVITY_TABS,
    REQUEST_TYPE_REQUEST,
} from '../../constants/constants';
import HelperService from '../../utils/helper.service';
import NotificationService from '../../services/notification.service';
import {
    resetCreditState,
    startPostCreditBasicData,
    startPostCreditObservationsData,
    startPostCreditProductData,
    startRequestActivityProcess,
    startObtenerVendedorCredito,
    startPostCreditFilesData,
} from '../../actions/credit.actions';
import { startGetAllPostInformationBasicSeverance } from '../../actions/severance.action';
import { CreditBasicDataRequestModel } from '../../models/requests/credit-basic-data-request.model';
import RequestInformationBasicData from '../RequestInformationBasicData/RequestInformationBasicData';
import { CreditBasicDataModel } from '../../models/credit-basic-data.model';
import { AppState } from '../../store/rootReducer';
import RequestInformationProductData from '../RequestInformationProductData/RequestInformationProductData';
import { CreditProductRequestModel } from '../../models/requests/credit-product-request.model';
import { CreditProductModel } from '../../models/credit-product.model';
import RequestInformationObservations from '../RequestInformationObservations/RequestInformationObservations';
import { CreditObservationModel } from '../../models/credit-observation.model';
import { CreditObservationsRequestModel } from '../../models/requests/credit-observations.request.model';
import RequestInformationViewFiles from '../RequestInformationViewFiles/RequestInformationViewFiles';
import RequestInformationTechnicalAnalyst from '../RequestInformationTechnicalAnalyst/RequestInformationTechnicalAnalyst';
import RequestInformationTechnicalLegal from '../RequestInformationTechnicalLegal/RequestInformationTechnicalLegal';
import RequestInformationFinancialData from '../RequestInformationFinancialData/RequestInformationFinancialData';
import RequestInformationTechnicalFinancial from '../RequestInformationTechnicalFinancial/RequestInformationTechnicalFinancial';
import RequestInformationUploadFiles from '../RequestInformationUploadFiles/RequestInformationUploadFiles';
import RequestInformationInsuranceData from '../RequestInformationInsuranceData/RequestInformationInsuranceData';
import RequestInformationSimulationData from '../RequestInformationSimulationData/RequestInformationSimulationData';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import RequestInformationDisbursement from '../RequestInformationDisbursement/RequestInformationDisbursement';
import RequestInformationLegalizationData from '../RequestInformationLegalizationData/RequestInformationLegalizationData';
import RequestInformationDistributionData from '../RequestInformationDistributionData/RequestInformationDistributionData';
import InformacionBeneficiario from '../InformacionBeneficiario/InformacionBeneficiario';
import CreditApi from '../../services/api-credit.service';
import DatosAliadosEstrategicos from '../DatosAliadosEstrategicos/DatosAliadosEstrategicos';
import CentralDeRiesgo from '../ConsultaCentralRiesgo/CentralDeRiesgo';
import ValidacionTelefonica from '../ValidacionTelefonicaAfiliado/ValidacionTelefonica';
import CesantiasApi from '../../services/api-cesantias.service';
import { CreditTechnicalDataRequestModel } from '../../models/requests/credit-technical-data-request.model';
import { CreditFlowDocumentsRequestModel } from '../../models/requests/credit-flow-documents-request.model';
import afiliadoApi from '../../services/api-afiliado.service';
import ApiFichaAnalista from '../../services/api-fichaAnalista.sercive';
import ApiSimulacion from '../../services/api-simulacion.service';
import Prospeccion from '../../services/api-prospeccion.service';
import ApiReportes from '../../services/api-reportes.service';
import ApiClientes from '../../services/api-clients.service';
import { LoadingV2Component } from '../loading/loadingV2.component';
import { CreditFilesRequestModel } from '../../models/requests/credit-files-request.model';
import { CreditFileModel } from '../../models/credit-file.model';
import RecursosACongelar from '../RequestRecursosCongelar/RecursosACongelar';
import SolicitudesNotificaciones from '../RequestSolicitudesNotificaciones/SolicitudesNotificaciones';
import FacturaSeven from './FacturaSeven';
import AuthService from '../../services/auth.service';
import ModalDevolverFlujos from './ModalDevolverFlujo/ModalDevolverFlujos';
import { validarAfiliadoYVendedor } from '../../services/verificacionTercero';
import { CatalogState } from '../../reducers/catalog.reducer';
import AvaluoComercial from './avaluoComercial/AvaluoComercialFormulario';
import Ubicacion from '../ubicacion/Ubicacion';

type DocumentoTemporal = {
    idTipoDocumento: number;
    archivo: File;
    rutaImagen: string;
    creadoPor: string;
    fechaCreacion: string;
    numero_documento: string;
};
const TABS_INDEX = {
    SUMMARY: 0,
    BASIC_DATA: 1,
    ADDITIONAL_DATA: 2,
    UPLOAD_FILES: 3,
    VIEW_FILES: 4,
    VIEW_OBSERVATIONS: 5,
    FINANCIAL_DATA: 6,
    FINANCIAL_ANALYST: 7,
    LEGAL_ANALYST: 8,
    TECHNICAL_ANALYST: 9,
    INSURANCE_DATA: 10,
    SIMULATION: 11,
    LEGALIZATION: 12,
    DISBURSEMENT: 13,
    DISTRIBUTION: 14,
    INFORMATION: 15,
    DATOSALIADOSESTATEGICOS: 16,
    CENTRAL_RIESGO: 17,
    VALIDACION_TELEFONICA: 18,
    RECURSOSACONGELAR: 19,
    SOLICITUDYNOTIFICACIONES:20,
    FACTURASEVEN:21,
    AVALUO_COMERCIAL:22,
    UBICACION:23
};

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '125px',
            marginLeft: '.3rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
    }),
);

const AntTabs = withStyles({
    flexContainer: {
        display: 'flex',
        flexWrap: 'wrap',
      },
    root: {
        borderBottom: '1px solid #e8e8e8',
        marginBottom: '1rem',
    },
    indicator: {
        backgroundColor: '#FEC917',
    },
})(Tabs);


const AntTab = withStyles((theme: Theme) =>
    createStyles({
        root: {
            textTransform: 'none',
            minWidth: 170,
            color: 'gray',
            marginRight: theme.spacing(1),

            '&:hover': {
                color: '#FEC917',
                opacity: 1,
            },
            '&$selected': {
                color: '#FEC917',
                fontWeight: theme.typography.fontWeightMedium,
            },
            '&:focus': {
                color: '#FEC917',
            },
        },
        selected: {},
    }),
)((props: StyledTabProps) => <Tab disableRipple {...props} />);

interface StyledTabProps {
    label: string;
    value: any;
}

function ViewTabs({
    currentTab,
    onTabChange,
    permissions,
}: {
    currentTab: number;
    onTabChange: (event: React.ChangeEvent<any>, newValue: number) => void;
    permissions: string[];
}) {
 
    return (
        <AntTabs
            value={currentTab}
            onChange={onTabChange}
            aria-label="ant example"
        >
            <AntTab label="Resumen" value={TABS_INDEX.SUMMARY} />
            {permissions?.map(permission => {
                switch (permission) {

                    
                    case REQUEST_ACTIVITY_TABS.INSURANCE_ANALYST:
                        return (
                            <AntTab
                                value={TABS_INDEX.INSURANCE_DATA}
                                key={`tab-${permission}`}
                                label="Datos Seguros"
                               
                            />
                        );

                    case REQUEST_ACTIVITY_TABS.CREATE_SIMULATION:
                        return (
                            <AntTab
                                value={TABS_INDEX.SIMULATION}
                                key={`tab-${permission}`}
                                label="Datos Simulación"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.ADDITIONAL_BASIC:
                        return (
                            <AntTab
                                label="Información Básica"
                                key={`tab-${permission}`}
                                value={TABS_INDEX.BASIC_DATA}
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.FACTURA_SEVEN:
                        return (
                            <AntTab
                                label="Factura Seven"
                                key={`tab-${permission}`}
                                value={TABS_INDEX.FACTURASEVEN}
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.UPLOAD_FILES:
                        return (
                            <AntTab
                                value={TABS_INDEX.UPLOAD_FILES}
                                key={`tab-${permission}`}
                                label="Subir Archivos"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.ADDITIONAL_DATA:
                        return (
                            <AntTab
                                value={TABS_INDEX.ADDITIONAL_DATA}
                                key={`tab-${permission}`}
                                label="Datos Garantía"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.TECHNICAL_ANALYST:
                        return (
                            <AntTab
                                value={TABS_INDEX.TECHNICAL_ANALYST}
                                key={`tab-${permission}`}
                                label="Datos Estudio Técnico"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.AVALUO_COMERCIAL:
                        return (
                            <AntTab
                                value={TABS_INDEX.AVALUO_COMERCIAL}
                                key={`tab-${permission}`}
                                label="Avalúo Comercial"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.UBICACION:
                        return (
                            <AntTab
                                value={TABS_INDEX.UBICACION}
                                key={`tab-${permission}`}
                                label="Ubicación"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.VIEW_OBSERVATIONS:
                        return (
                            <AntTab
                                value={TABS_INDEX.VIEW_OBSERVATIONS}
                                key={`tab-${permission}`}
                                label="Observaciones"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.FINANCIAL_DATA:
                        return (
                            <AntTab
                                value={TABS_INDEX.FINANCIAL_DATA}
                                key={`tab-${permission}`}
                                label="Información Financiera"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.LEGAL_ANALYST:
                        return (
                            <AntTab
                                value={TABS_INDEX.LEGAL_ANALYST}
                                key={`tab-${permission}`}
                                label="Datos Estudio Legal"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.FINANCIAL_ANALYST:
                        return (
                            <AntTab
                                value={TABS_INDEX.FINANCIAL_ANALYST}
                                key={`tab-${permission}`}
                                label="Datos Estudio Financiero"
                            />
                        );

                    case REQUEST_ACTIVITY_TABS.LEGALIZATION:
                        return (
                            <AntTab
                                value={TABS_INDEX.LEGALIZATION}
                                key={`tab-${permission}`}
                                label="Datos Legalización"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.DISBURSEMENT:
                        return (
                            <AntTab
                                value={TABS_INDEX.DISBURSEMENT}
                                key={`tab-${permission}`}
                                label="Datos Desembolso"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS.DISTRIBUTION:
                        return (
                            <AntTab
                                value={TABS_INDEX.DISTRIBUTION}
                                key={`tab-${permission}`}
                                label="Datos Reparto"
                            />
                        );

                    case REQUEST_ACTIVITY_TABS?.INFORMATION:
                        return (
                            <AntTab
                                value={TABS_INDEX.INFORMATION}
                                key={`tab-${permission}`}
                                label="Información Vendedor"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS?.DATOSALIADOSESTATEGICOS:
                        return (
                            <AntTab
                                value={TABS_INDEX.DATOSALIADOSESTATEGICOS}
                                key={`tab-${permission}`}
                                label="Datos Aliados Estrategicos"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS?.CENTRAL_RIESGO:
                        return (
                            <AntTab
                                value={TABS_INDEX.CENTRAL_RIESGO}
                                key={`tab-${permission}`}
                                label="Datos Central Riesgo"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS?.VALIDACION_TELEFONICA:
                        return (
                            <AntTab
                                value={TABS_INDEX.VALIDACION_TELEFONICA}
                                key={`tab-${permission}`}
                                label="Validación Telefónica"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS?.RECURSOSACONGELAR:
                        return (
                            <AntTab
                                value={TABS_INDEX.RECURSOSACONGELAR}
                                key={`tab-${permission}`}
                                label="Bloqueo de Recursos"
                            />
                        );
                    case REQUEST_ACTIVITY_TABS?.SOLICITUDYNOTIFICACIONES:
                        return (
                            <AntTab
                                value={TABS_INDEX.SOLICITUDYNOTIFICACIONES}
                                key={`tab-${permission}`}
                                label="Solicitudes y Notificaciones"
                            />
                        );
   

                    default:
                        return null;
                }
            })}
        </AntTabs>
    );
}

function Summary({
    typeRequest,
    procedureRequest,
    isLoading,
    control,
    documentosObligatorios,
    obterReporteHijo
}: {
    typeRequest?: string;
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean;
    control: Control<{observation: string;}>;
    documentosObligatorios: any
    obterReporteHijo: any
}) {

    const classes = useStyles();
    const credito = CreditApi();
    const apiReportes = ApiReportes();

    const [segunObervacion, setSegunObservacio] = useState<any>(false)
    const [numeroRadicado, setNumeroRadicado] =useState("")
    const [reports, setReports] =useState("")
    const [isLocalLoading, setIsLocalLoading] = useState<boolean>(false);

    useEffect(() => {
        setSegunObservacio(false)
        datosInfoSolicitud();
        if (procedureRequest.flowActivityID !== 2){
            setSegunObservacio(true)
        }
          
    }, [])

    useEffect(() => {
        if(documentosObligatorios.length !== 0  &&
            (procedureRequest.flowActivityID === 8 || 
                procedureRequest.flowActivityID === 12))
        {
            reporteFinanciero()
        }

        if(documentosObligatorios.length !== 0  &&
            procedureRequest.flowActivityID === 3)
        {
            reporteHaberes()
        }
    }, [documentosObligatorios])


     const reporteHaberes  = async () => {
        
            const identificacion = {
                customerIdentification: String(procedureRequest.clientIdentificationNumber),
            };
            await credito
                .ReporteGeneralHaberes(identificacion)
                .then(async response => {
                    setReports(response)
                    
                }
                    
                )
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                }).finally(() => setIsLocalLoading(false));
      
    }

    const reporteFinanciero  = async () => {
        const filtroDeRutasLLenas = documentosObligatorios.filter((x: { tipO_DOCUMENTO: string; }) => x.tipO_DOCUMENTO === "Reporte Resumen Financiero")

       
            // setIsLocalLoading(true);
            const request = {
                identification: String(procedureRequest.clientIdentificationNumber),
            };
            await apiReportes
                .ReporteResumenSolicitud(request)
                .then(async response => {
                    setReports(response)
                    }
                    
                )
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                }).finally(() => setIsLocalLoading(false));
    }


    const datosInfoSolicitud = async () => {
       
        await credito
            .getObtenerSolicitudCreditoTipoNumeroIdentificacion(
               
                1,
                String(procedureRequest.clientIdentificationNumber)
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        setNumeroRadicado(response.data.slS_NUMERO_RADICADO)   
                     }
                      
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
             
            });
    };


 
    if (isLoading) {
        return <CHLoadingProcess />;
    }
  
    return (
        <CHPaper title="Información">
            <LoadingV2Component key="loader" loading={isLocalLoading} tittle='' />
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Actividad"
                        value={procedureRequest.activity}
                        inputProps={{
                            style: {
                                textTransform: 'uppercase',
                                fontSize: '13.7px',
                            },
                        }}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="No. Solicitud"
                        value={procedureRequest.requestNumber}
                    />
                </Grid>

{console.log(procedureRequest)}
                <Grid className={classes.column2} item xs={12} md={3}>
                   { procedureRequest?.flowActivityID !== 1 && procedureRequest?.flowActivityID !== 7 &&  procedureRequest?.flowActivityID !== 4 ? 
                   <a
                        href={`http://cpvmp-112p:91/LinkFormulario/FormularioWM/Index?columna=Cod_Barras&valorColumna=${numeroRadicado}&idFormulario=20474`}
                        target="_blank"
                        rel="noopener noreferrer"
                        style={{ textDecoration: 'none', color: 'inherit' }}
                    >
                        <Box display="flex" alignItems="center"> {/* Usa flexbox para alinear los elementos */}
                            <CHTextField
                                isReadOnly
                                label="Número Radicado"
                                value={numeroRadicado}
                            />
                            <IconButton>
                                <AttachFile />
                            </IconButton>
                        </Box>
                    </a>:
                      <Box display="flex" alignItems="center"> {/* Usa flexbox para alinear los elementos */}
                      <CHTextField
                          isReadOnly
                          label="Número Radicado"
                          value={numeroRadicado}
                      />
                   
                  </Box>
                    }
                </Grid>

                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Tipo Crédito"
                        value={procedureRequest.creditType}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="No. Documento Cliente"
                        value={procedureRequest.clientIdentificationNumber}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Nombre / Razón Social"
                        value={procedureRequest.clientName}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Fecha Solicitud"
                        value={UtilService.getDate(
                            procedureRequest.requestDate,
                        )}
                    />
                </Grid>

                {procedureRequest.flowActivityID !==7 ?(
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        currencyMask
                        label="Valor Crédito"
                        value={procedureRequest.creditValue}
                        inputProps={{
                            style: {
                                textAlign: 'right',
                            },
                        }}
                    />
                </Grid>
                
                ):[]}
                {procedureRequest.flowActivityID ===7 ?( 
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        currencyMask
                        label="Valor Pre-Aprobado"
                        value={procedureRequest.creditValue}
                        inputProps={{
                            style: {
                                textAlign: 'right',
                            },
                        }}
                    />
                </Grid>
                
                ):[]}

                {REQUEST_TYPE_REQUEST.SEVERANCE !== typeRequest ? (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Actividad Actual"
                            value={procedureRequest.activity}
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                    fontSize: '13.7px',
                                },
                            }}
                        />
                    </Grid>
                ) : null}
                {REQUEST_TYPE_REQUEST.SEVERANCE !== typeRequest ? (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Plazo"
                            value={procedureRequest.financingTerm}
                        />
                    </Grid>
                ) : null}
                <Grid className={classes.column2} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Estado"
                        value={procedureRequest.stateActivity}
                    />
                </Grid>
                <Grid className={classes.column2} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Correo"
                        value={procedureRequest.personalEmail}
                    />
                </Grid>
                <Grid className={classes.column2} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Dirección Residencia"
                        value={procedureRequest.homeAddress}
                    />
                </Grid>
                {REQUEST_TYPE_REQUEST.SEVERANCE !== typeRequest ? (
                    <Grid className={classes.column2} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Estado Construcción"
                            value={procedureRequest.livingType}
                        />
                    </Grid>
                ) : null}
                <Grid className={classes.column2} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Teléfono Residencia"
                        value={procedureRequest.homePhone}
                    />
                </Grid>
                <Grid className={classes.column2} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Celular"
                        value={procedureRequest.cellphoneNumber}
                    />
                </Grid>

                
                {
                    segunObervacion || procedureRequest.stateActivity !== "FINALIZADA" &&
                    <Grid className={classes.column2} item xs={12} md={12}>
                        <Controller
                            name="observation"
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
                                    label="Concepto Asesor *"
                                    value={value}
                                    helperText={error ? error.message : null}

                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                }
                {!segunObervacion || procedureRequest.stateActivity !== "FINALIZADA" &&
                    <Grid className={classes.column2} item xs={12} md={12}>
                        <Controller
                            name="observation"
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
                                    label="Concepto Analista *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                }
            

    
            {/* {procedureRequest.flowActivityID === 8 || procedureRequest.flowActivityID === 12 ? (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Button
                            style={{ backgroundColor: '#a9a9a9', color: 'white', width: '100%' }}
                            variant="contained"
                            onClick={reporteFinanciero()}
                        >
                            Reporte Financiero
                        </Button>
                    </Grid>
                ) : null}  */}
            

                      

            </Grid>
        </CHPaper>
        
    );
}

function Panel({
    typeRequest,
    currentTabValue,
    informacionAfiliado,
    procedureRequest,
    isLoadingCredit,
    creditBasicData,
    creditProductData,
    creditObservationsData,
    startPostCreditBasicData,
    startPostCreditProductData,
    startPostCreditObservationsData,
    startGetAllPostInformationBasicSeverance,
    control,
    validarDocObligatorios,
    documentosObligatorios,
    obterReporteHijo,
    validacionDeTablasDatosDesembolso,
    setHacerSimulacion,
    catalogState,
    documentosTemporales,
setDocumentosTemporales,
}: {
    typeRequest?: string;
    currentTabValue: number;
    informacionAfiliado: any;
    procedureRequest: ProcedureRequestModel;
    isLoadingCredit: boolean;
    creditBasicData: CreditBasicDataModel;
    creditProductData: CreditProductModel;
    creditObservationsData: CreditObservationModel[];
    startPostCreditBasicData: (data: CreditBasicDataRequestModel) => void;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    startPostCreditObservationsData: (data: CreditObservationsRequestModel,) => void;
    startGetAllPostInformationBasicSeverance: (numeroIdentificacion: number,) => void;
    control: Control<{observation: string;}>;
    validarDocObligatorios: any;
    documentosObligatorios: any;
    obterReporteHijo: any;
    validacionDeTablasDatosDesembolso:any;
    setHacerSimulacion:any
    catalogState: CatalogState;
    documentosTemporales: DocumentoTemporal[];

    setDocumentosTemporales: React.Dispatch<
        React.SetStateAction<DocumentoTemporal[]>

>;
}) {

    const [valor, setValor] = useState('')

    const docObligatorio = (data: any) => {
        setValor(data)

    }
   

    useEffect(() => {
        validarDocObligatorios(valor)
    }, [valor, setValor])

    useEffect(() => {
        validacionDeTablasDatosDesembolso(valor)
    }, [])



    switch (currentTabValue) { 
        case TABS_INDEX.SUMMARY:
            return (
                <Summary
                   documentosObligatorios={documentosObligatorios}
                   obterReporteHijo={obterReporteHijo}
                    typeRequest={typeRequest}
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                    control={control}
                    
                />
            );
        case TABS_INDEX.BASIC_DATA:
            return (
                <RequestInformationBasicData
                    typeRequest={typeRequest}
                    isLoading={isLoadingCredit}
                    informacionAfiliado={informacionAfiliado}
                    creditBasicData={creditBasicData}
                    startPostCreditBasicData={startPostCreditBasicData}
                    procedureRequest={procedureRequest}
                    startGetAllPostInformationBasicSeverance={
                        startGetAllPostInformationBasicSeverance
                    }
                />
            );
        case TABS_INDEX.FACTURASEVEN:
            return (
                <FacturaSeven
                   
                    procedureRequest={procedureRequest}
                    
                />
            );
        case TABS_INDEX.ADDITIONAL_DATA:
            return (
                <RequestInformationProductData
                    startPostCreditProductData={startPostCreditProductData}
                    creditProductData={creditProductData}
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                    catalogState={catalogState}
                />
            );
        case TABS_INDEX.VIEW_OBSERVATIONS:
            return (
                <RequestInformationObservations
                    isLoading={isLoadingCredit}
                    procedureRequest={procedureRequest}
                    observationsData={creditObservationsData}
                    startPostCreditObservationsData={
                        startPostCreditObservationsData
                    }
                />
            );
            case TABS_INDEX.UPLOAD_FILES:
                return (
                    <RequestInformationUploadFiles
                        procedureRequest={procedureRequest}
                        docObligatorio={docObligatorio}
                        isLoading={isLoadingCredit}
                        documentosTemporales={
                            documentosTemporales
                        }
                        setDocumentosTemporales={
                            setDocumentosTemporales
                        }
                    />
                );
        case TABS_INDEX.VIEW_FILES:
            return (
                <RequestInformationViewFiles
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.TECHNICAL_ANALYST:
            return (
                <RequestInformationTechnicalAnalyst
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
            case TABS_INDEX.AVALUO_COMERCIAL:
                return (
                    <AvaluoComercial
                        procedureRequest={procedureRequest}
                        isLoading={isLoadingCredit}
                    />
                );
            case TABS_INDEX.UBICACION:
                return (
                    <Ubicacion
                        procedureRequest={procedureRequest}
                        isLoading={isLoadingCredit}
                    />
                );
        case TABS_INDEX.LEGAL_ANALYST:
            return (
                <RequestInformationTechnicalLegal
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.FINANCIAL_DATA:
            return (
                <RequestInformationFinancialData
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.FINANCIAL_ANALYST:
            return (
                <RequestInformationTechnicalFinancial
                    procedureRequest={procedureRequest}
                    informacionAfiliado={informacionAfiliado}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.INSURANCE_DATA:
            return (
                <RequestInformationInsuranceData
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.SIMULATION:
            return (
                <RequestInformationSimulationData
                    startPostCreditProductData={startPostCreditProductData}
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                    setHacerSimulacion={setHacerSimulacion}
                />
            );
        case TABS_INDEX.LEGALIZATION:
            return (
                <RequestInformationLegalizationData
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.DISBURSEMENT:
            return (
                <RequestInformationDisbursement
                    procedureRequest={procedureRequest}
                    creditProductData={creditProductData}
                    startPostCreditProductData={startPostCreditProductData}
                    isLoading={isLoadingCredit}
                    validacionDeTablasDatosDesembolso = {validacionDeTablasDatosDesembolso}

                />
            );
        case TABS_INDEX.DISTRIBUTION:
            return (
                <RequestInformationDistributionData
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );

        case TABS_INDEX.INFORMATION:
            return (
                <InformacionBeneficiario
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );

        case TABS_INDEX.DATOSALIADOSESTATEGICOS:
            return (
                <DatosAliadosEstrategicos
                    procedureRequest={procedureRequest}
                   
                />
            );

        case TABS_INDEX.CENTRAL_RIESGO:
            return (
                <CentralDeRiesgo      
                procedureRequest={procedureRequest}
                isLoading={isLoadingCredit}

                />
            );

        case TABS_INDEX.VALIDACION_TELEFONICA:
            return (
                <ValidacionTelefonica
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.RECURSOSACONGELAR:
            return (
                <RecursosACongelar
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );
        case TABS_INDEX.SOLICITUDYNOTIFICACIONES:
            return (
                <SolicitudesNotificaciones
                    procedureRequest={procedureRequest}
                    isLoading={isLoadingCredit}
                />
            );



        default:
            return null;

    }

}
function RequestInformation({
    typeRequest,
    procedureRequest,
    onItemGoBack,
    startRequestActivityProcess,
    startPostCreditBasicData,
    startPostCreditProductData,
    creditBasicData,
    isLoadingCredit,
    creditProductData,
    creditObservationsData,
    formularioVendedor,
    creditFilesData,
    catalogState,
    startPostCreditObservationsData,
    error,
    responseRequestActivity,
    startGetAllPostInformationBasicSeverance,
    informationBasicSeverance,
    startObtenerVendedorCredito,
    startPostCreditFilesData,
}: {
    typeRequest?: string;
    error: any;
    responseRequestActivity: any;
    procedureRequest: ProcedureRequestModel;
    isLoadingCredit: boolean;
    creditBasicData: CreditBasicDataModel;
    creditProductData: CreditProductModel;
    creditObservationsData: CreditObservationModel[];
    informationBasicSeverance: CreditBasicDataModel;
    formularioVendedor: any;
    creditFilesData: CreditFileModel[];
    catalogState: CatalogState;
    onItemGoBack: () => void;
    startRequestActivityProcess: (data: RequestActivityRequestModel, requestType: number,) => void;
    startPostCreditBasicData: (data: CreditBasicDataRequestModel) => void;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    startPostCreditObservationsData: (data: CreditObservationsRequestModel,) => void;
    startGetAllPostInformationBasicSeverance: (numeroIdentificacion: number,) => void;
    startObtenerVendedorCredito: (simulationID: any) => void;
    startPostCreditFilesData: (data: CreditFilesRequestModel) => void;
}) {
    const credito = CreditApi();
    const prospeccion = Prospeccion();
    const simulacionApi = ApiSimulacion();
    const apiSeven = CesantiasApi();
    const apiAfiliado = afiliadoApi();
    const fichaAnalistaApi = ApiFichaAnalista();
    const apiBeneficiario = afiliadoApi()
    const apiClientes = ApiClientes();
    const user = AuthService.getUserInfo();
    const classes = useStyles();

    const [isLoading, setIsLoading] = useState (false);
    const [currentTabValue, setCurrentTabValue] = useState(TABS_INDEX.SUMMARY);
    const [estadoActividad, setEstadoActividad] = useState(procedureRequest.stateActivity,);
    const [openModal, setOpenModal] = useState(false);
    const [dataType, setDataType] = useState({});
    const [docObligatorio, setDocObligatorio] = useState<any[]>([])
    const [documentosObligatorios, setDocumentosObligatorios] = useState<any[]>([])
    const [mensajeErrorSeven, setmensajeErrorSeven] = useState<any[]>([])
    const [numeroCDP, setNumeroCDP] = useState("false");
    const [verNuemeroCDP, setVerNuemeroCDP] = useState(false);
    const [errorCdp, SetErrorCdp] = useState(false);
    const [peritosPreTramite, setPeritosPreTramite] = useState<any[]>([]);
    const [informacionAfiliado, setInformacionAfiliado] = useState<any>(null);
    const [datosObligatoriosDesemboloso, setDatosObligatoriosDesembolso] = useState('')
    const [hacerSimulacion, setHacerSimulacion] = useState(false)
    const [documentoVendedor, setDocumentoVendedor] = useState<string>('')
    const [
        numeroRadicadoDodo,
        setNumeroRadicadoDodo,
    ] = useState('');
    const [
        documentosTemporales,
        setDocumentosTemporales,
    ] = useState<DocumentoTemporal[]>([]);

    const [
        ultimaAccionEjecutada,
        setUltimaAccionEjecutada,
    ] = useState<number | null>(null);
    
    const [
        procesandoAccion,
        setProcesandoAccion,
    ] = useState(false);
    const history = useHistory();

    useEffect(() => {
        if (
            procedureRequest
                ?.flowActivityID !== 12
        ) {
            listarArchivos();
        }
    
        if (
            procedureRequest
                ?.flowActivityID === 2
        ) {
            obtenerPeritoPreTramite();
        }
    
        obtenerNumeroRadicadoDodo();
    
        obtenerInformacionAfiliado();
    
        obtnerBenficiarios();
    
        obtenerobtenerDatosDesembolso();
    }, [procedureRequest]);
     
    const { control, handleSubmit, getValues, setValue } = useForm({
        defaultValues: {
            observation: '',
        },

      

    });

    const {
        handleSubmit: handleSubmit2,
        control: control2,
        getValues : getValues2,
        setValue: setValue2
    } = useForm({
        defaultValues: {
            validacionFirmas: "",
            validacionDesembolso: "",
            validacionEstudioTecnico: {},
            validacionEstudioLegal: {},
            validacionAliadoEstrategico: {},
            validacionFichaAnalista: {},
            validacionTelefonicaAfiliado: {},
            validacionDocuemntos: false,
            validacionDocuemntos2: false,
            validacionAprobacionCredito: false,
            validacionBeneficiario: 0,
            validacionDesembolsos: false,
        },
    });


    const enviarDocumentosDODO =
    async (): Promise<boolean> => {
        if (
            documentosTemporales.length ===
            0
        ) {
            return true;
        }

        if (!numeroRadicadoDodo) {
            await NotificationService.genericAlert(
                'Error',
                'No fue posible obtener el número de radicado para enviar los documentos.',
                'error',
            );

            return false;
        }

        try {
            setIsLoading(
                true,
            );

            const documentos =
                documentosTemporales.map(
                    documento => ({
                        idSolicitudCredito:
                            procedureRequest.requestID,

                        idTipoDocumento:
                            documento.idTipoDocumento,

                        codigoBarras:
                            '',

                        rutaImagen:
                            documento.rutaImagen,

                        creadoPor:
                            documento.creadoPor,

                        fechaCreacion:
                            documento.fechaCreacion,

                        numero_documento:
                            documento.numero_documento,
                    }),
                );

            const payloadDodo = {
                documentos,

                radicado:
                    numeroRadicadoDodo,
            };

            const respuestaDodo =
                await credito.PostCargarDocumentosActividadCreditoDODO(
                    payloadDodo,
                );

            if (
                !respuestaDodo?.isSuccess
            ) {
                await NotificationService.genericAlert(
                    'Error',
                    respuestaDodo?.returnMessage ||
                        'No fue posible enviar los documentos a DODO.',
                    'error',
                );

                return false;
            }

            setDocumentosTemporales(
                [],
            );

            return true;
        } catch {
            await NotificationService.genericAlert(
                'Error',
                'No fue posible enviar los documentos a DODO.',
                'error',
            );

            return false;
        } finally {
            setIsLoading(
                false,
            );
        }
    };
    const obtenerNumeroRadicadoDodo =
    async () => {
        try {
            const response =
                await credito.getObtenerSolicitudCreditoTipoNumeroIdentificacion(
                    1,
                    String(
                        procedureRequest
                            .clientIdentificationNumber,
                    ),
                );

            if (
                !response?.isSuccess ||
                !response?.data
            ) {
                await NotificationService.genericAlert(
                    'Error',
                    'No fue posible obtener el número de radicado.',
                    'error',
                );

                return;
            }

            const radicado =
                response.data
                    .soC_NUMERO_RADICADO ||
                response.data
                    .slS_NUMERO_RADICADO ||
                '';

            setNumeroRadicadoDodo(
                radicado,
            );
        } catch {
            await NotificationService.genericAlert(
                'Error',
                'No fue posible consultar el número de radicado.',
                'error',
            );
        }
    };

    const [cambiarNombreBotonAprobar, setCambiarNombreBotonAprobar] = useState('APROBAR')

  

    useEffect(() => {
        if (
            ultimaAccionEjecutada === null ||
            (responseRequestActivity == null && error == null)
        ) {
            return;
        }

        const mostrarResultadoActividad = async () => {
            if (error != null) {
                await NotificationService.genericAlert(
                    'Error',
                    error || 'No fue posible procesar la solicitud.',
                    'error',
                );

                setProcesandoAccion(false);
                setUltimaAccionEjecutada(null);
                return;
            }

            if (
                ultimaAccionEjecutada ===
                REQUEST_ACTIVITY_PROCESS.APPROVE
            ) {
                await NotificationService.genericAlert(
                    'Exitoso',
                    'La solicitud fue aprobada correctamente.',
                    'success',
                );
            } else if (
                ultimaAccionEjecutada ===
                REQUEST_ACTIVITY_PROCESS.REJECTED
            ) {
                await NotificationService.genericAlert(
                    'Exitoso',
                    'La solicitud fue rechazada correctamente.',
                    'success',
                );
            } else if (
                ultimaAccionEjecutada ===
                REQUEST_ACTIVITY_PROCESS.BACK
            ) {
                await NotificationService.genericAlert(
                    'Exitoso',
                    'La solicitud fue devuelta correctamente.',
                    'success',
                );
            } else if (
                ultimaAccionEjecutada ===
                REQUEST_ACTIVITY_PROCESS.DESISTIR
            ) {
                await NotificationService.genericAlert(
                    'Exitoso',
                    'La solicitud fue desistida correctamente.',
                    'success',
                );
            } else {
                await NotificationService.genericAlert(
                    'Exitoso',
                    'La solicitud fue procesada correctamente.',
                    'success',
                );
            }

            setProcesandoAccion(false);
            setUltimaAccionEjecutada(null);
        };

        mostrarResultadoActividad();
    }, [
        error,
        responseRequestActivity,
        ultimaAccionEjecutada,
    ]);

    const obtenerPeritoPreTramite = async () => {
        await prospeccion
            .getConsultarPeritoDetalle(procedureRequest.requestID)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        
                        setPeritosPreTramite([response.data])
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const actializarPeritoPreTramite = async () => {

        const informacionPerito: any = {
            trS_ID: procedureRequest.trsID,
            acT_ID: procedureRequest.idActivity,
            // acT_ID: String(procedureRequest?.iD_ACTIVIDAD),
            trS_OBSERVACION: peritosPreTramite[0]?.peR_OBSERVACION,
            iD_USUARIO_PERITO: peritosPreTramite[0]?.peR_PERITO_ID,
        };
        await credito
            .postActualizaTramitePerito(informacionPerito)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        console.log(response.data) 
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };


    const obtenerInformacionAfiliado = async () => {

        setIsLoading(true);
        const numeroDocumento = String(procedureRequest?.clientIdentificationNumber);
        await apiClientes
        .getClientByIdentificationTypeIdentificationNumberRequest(
            1,
            numeroDocumento
        )
        .then(async response => {
            if (response?.data) {
                setInformacionAfiliado(response?.data);
                setIsLoading(false);
            }
        })
        .catch((e) => {
            toast(`Error en consultar cliente ${e.message}` , { type: toast.TYPE.ERROR });
            setIsLoading(false);
        });

    };

    const listarArchivos = async () => {
        const request: CreditFlowDocumentsRequestModel = {
            tcR_ID: procedureRequest.creditTypeID,
            orden: procedureRequest.order,
            principal: procedureRequest.principalActivity,
            soC_ID: procedureRequest.requestID,
            afL_ID: procedureRequest.flowActivityID,
        };
    
        await credito
            .postCreditFiles(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        setDocumentosObligatorios(
                            response.data,
                        );
    
                        const filtroDeRutasLLenas =
                            response.data.filter(
                                (x: {
                                    obligatorio: string;
                                }) =>
                                    x.obligatorio ===
                                    'S',
                            );
    
                        setValue2(
                            'validacionDocuemntos',
                            false,
                        );
    
                        setValue2(
                            'validacionDocuemntos2',
                            false,
                        );
    
                        for (
                            let i = 0;
                            i <
                            filtroDeRutasLLenas.length;
                            i += 1
                        ) {
                            const documento =
                                filtroDeRutasLLenas[
                                    i
                                ];
    
                            const tieneDocumentoServidor =
                                documento.rutA_IMAGEN !==
                                    null &&
                                documento.rutA_IMAGEN !==
                                    '';
    
                            const tieneDocumentoTemporal =
                                documentosTemporales.some(
                                    documentoTemporal =>
                                        documentoTemporal.idTipoDocumento ===
                                        Number(
                                            documento.tdC_ID,
                                        ),
                                );
    
                            if (
                                documento.obligatorio ===
                                    'S' &&
                                !tieneDocumentoServidor &&
                                !tieneDocumentoTemporal
                            ) {
                                setValue2(
                                    'validacionDocuemntos',
                                    true,
                                );
                            }
                        }
    
                        for (
                            let i = 0;
                            i <
                            filtroDeRutasLLenas.length;
                            i += 1
                        ) {
                            if (
                                filtroDeRutasLLenas[
                                    i
                                ]
                                    .dcS_APROBACION !==
                                    1 &&
                                filtroDeRutasLLenas[
                                    i
                                ].obligatorio ===
                                    'S'
                            ) {
                                setValue2(
                                    'validacionDocuemntos2',
                                    true,
                                );
                            }
                        }
                    } else {
                        await NotificationService.genericAlert(
                            'Error en actualización',
                            'Ocurrio un error al intentar actualizar, intente de nuevo.',
                            'error',
                        );
                    }
                }
            })
            .catch(() => {
                toast(
                    'Error del sistema',
                    {
                        type: toast.TYPE.ERROR,
                    },
                );
            });
    };

        const obtenerCreditLegalData = async (type:number[]) => {
                    const dataRequest : any = {
                    indicador: 2,
                    idSolicitudCredito: procedureRequest.requestID,
                };
            await credito
                .postCreditLegalData(dataRequest)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                           
                            setValue2('validacionEstudioLegal',response.data)
                           
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

        const obtenerSolicCreditoInfTecnica = async (type:number[]) => {
          
            
                    const dataRequest: CreditTechnicalDataRequestModel = {
                    indicador: 2,
                    solicitudCredito: procedureRequest.requestID,
                };
            await credito
                .postCreditTechnicalData(dataRequest)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                          
                            setValue2('validacionEstudioTecnico',response.data)
                           
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

        const ObtenerAliadosEstrategicos = async (type:number[]) => {
            const dataRequest : any = {
                soc_Id: procedureRequest.requestID,
            };
            await apiAfiliado
                .postObtenerAliadosEstrategicos(dataRequest)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                           
                            setValue2('validacionAliadoEstrategico',response.data)
                           
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

        const obtenerFichaAnalisticaPorId = async(type:number[]) => {
        
            const response = {
                soc_id: procedureRequest.requestID,
                fa_Id: 0
            }
            await fichaAnalistaApi
                .postObtenerFichaAnalistaPorId(response)
                .then(async (response: any) => {
                    if (response) {
                       if(response.data.length !==0  ){
                        
                        setValue2('validacionFichaAnalista',response.data)
                       
                       }   else {
                        setValue2('validacionFichaAnalista',JSON.stringify(response.data))
                       }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
    
        }

        const obtenerValidacionTelefonica = async (type:number[]) => {
            const response = {
                soc_Id: procedureRequest.requestID,
            }
            await apiAfiliado
                .postObtenerValidacionTelefonica(response)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess ) {
                          
                            setValue2('validacionTelefonicaAfiliado',response.data)
                           
                           
                        } else {
                            toast(`Error: ${response.returnMessage}`, {
                                type: toast.TYPE.ERROR,
                            });
                        }
                    }
                })
                .catch(e => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
    
        }

        const obtnerBenficiarios = async() => {
            const request =  {
                Soc_Id: procedureRequest.requestID,
                Sls_Id: '00000000-0000-0000-0000-000000000000'
            }
            await credito
            .postObtenerVendedor(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        const docVendedor = response.data[0].numero_Doc
                        setDocumentoVendedor(String(docVendedor))
                        
                        setValue2('validacionBeneficiario',response.data.length)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        }

        const obtenerobtenerDatosDesembolso = async () => {
            await credito
                .GetLegalizacionData(procedureRequest.requestID)
                .then(async response => {
     
                   setValue2('validacionFirmas',response.data.validaFirmas)
                   setValue2('validacionDesembolso',response.data.validaDesembolso)

                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        };
        
        const consultarVigenciaSimulacion = async () => {
            await prospeccion
                .postConsultarVigenciaSimulacion(procedureRequest.clientIdentificationNumber)
                .then(async response => {
                    console.log('consultarVigenciaSimulacion',response.data)
                    
                    // if(response.data === false){
                    //     await NotificationService.genericAlert(
                    //         'Vigencia de simulación',
                    //         'Plazo de vigencia caduco',
                    //         'error',
                    // )}
                 
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        };

        const creaSCnMcontAprobacionCredito = async () => {
            debugger
            setIsLoading(true)
            const request:any = {
                afl_Id: procedureRequest?.flowActivityID,
                idSolicitudCredito:  procedureRequest.requestID,
                tipoIdentificacion:  1,
                numeroDocumentoCliente: String(procedureRequest?.clientIdentificationNumber),
                tipoCredito:procedureRequest.creditTypeID,
                tipoPago:2,
                user: AuthService.getUserInfo().USR_ID,
            }
   
            await credito
                .postCreaSCnMcontAprobacionCredito(request)
                .then(async response => {
                    setIsLoading(false)
                    if(response){
                        if(response.isSuccess){
                            const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                            const mensajeError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                            const codigoNoValidoMatch = response.returnMessage[1].match(/MCnMcontMts.ValidarCnCuent - (\d+) \./);
                            setmensajeErrorSeven(mensajeError)
                            if(retornoExitoso[1] === "1"){
                                setValue2('validacionAprobacionCredito',false)

                                NotificationService.genericAlertError(
                                    
                                    'Aprobacion Comite',
                                    ` ${(mensajeError[1])}`,  
                                    'warning',
                                ); 
                            }
                            else if(retornoExitoso[1] === "0"){
                                setValue2('validacionAprobacionCredito',true)
                            }
                        }
                    }
                })
                .catch(() => {
                    setIsLoading(false)
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        };

    const handleTabChange = (
        event: React.ChangeEvent<any>,
        newValue: number,
    ) => {
        setCurrentTabValue(newValue);
    };

    const handleCloseModal = () => {
        setOpenModal(false);
    };

    const obterFlujoModalHijo = (item: any) => {
   
        setEstadoActividad("")
    }

    const obterReporteHijo = () => {
        listarArchivos()
    }

    const validarDocObligatorios = (item: any[]) => {      
        setDocObligatorio(item)
    }
    const  validacionDeTablasDatosDesembolso = (dato:any) => {
 
        setDatosObligatoriosDesembolso(dato)
    
      }

        const CrearTerceroSevenCredito = async(type:number[]) => {
        setIsLoading(true)
           
         if(procedureRequest.flowActivityID === 12){
            creaSCnMcontAprobacionCredito()
            return
        }
     
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await credito
            .postCrearTerceroSevenCredito(response)
            .then(async response => {
                setIsLoading(false)
                if(response){
                    if(response.isSuccess){  
                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        const retornoMensaje = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                    
                        if(retornoExitoso[1] !== "0"){
                        
                            NotificationService.genericAlertError(
                                
                                'AFILIADO VERIFICADO',
                                ` ${retornoMensaje[1]}`,  
                                'warning',
                            ); 
                            
                            if ( procedureRequest?.flowActivityID === 18) {
                                
                                CrearCDPSegundoPago(type)
                            }else{
                                CrearCDPPrimerPago(type)
                            }
                           
                        }else{
                            
                            NotificationService.genericAlertError(
                                
                                'AFILIADO VERIFICADO',
                                `Exitoso`,  
                                'success',
                                
                            );
                            
                            if (procedureRequest?.flowActivityID === 12 || procedureRequest?.flowActivityID === 18) {
                                
                                CrearCDPSegundoPago(type)
                            }else{
                                CrearCDPPrimerPago(type)
                            }
                            
                           
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        


    }

    const CrearCDPSegundoPagoFuncional = async(type:number[]) => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await credito
            .postCrearCDPSegundoPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){  
                setIsLoading(false)
                const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
                const retornoFallido = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                setNumeroCDP(numCDP[1])

                if(retornoExitoso[1] !== "0"){
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'CONSULTA CDP',
                        'No hay disponibilidad de recursos',
                 
                         'warning',
                    ); 

                    SetErrorCdp(true)
                   
                }else{
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'CONSULTA CDP',
                        ` El numero de CDP asignado es: ${(numCDP[1])}`,  
                        'success',
                    );

                  

                    if (getValues('observation') !== '') {

                        CrearProveedorSeven(type)
                                
                }else {
                    NotificationService.genericAlert(
                        'Resumen',
                        'El campo observación es obligatorio .',
                        'warning',)
                       
                }
             
                }
            }else {
                setIsLoading(false)
                toast(`Error: ${response.returnMessage}`, {
                    type: toast.TYPE.ERROR,
                });

            }}
              })
             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const CrearProveedorSeven = async(type:number[]) => {
        
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: 1,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await credito
            .postCrearProveedorSeven(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){
                    
                        setIsLoading(false)
                        
                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        const retornoMensaje = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                    
                        if(retornoExitoso[1] !== "0"){
                        
                            // guardarDatosInformacion()
                            console.log('este es si')
                           
                        }
                        else{
                          
                            NotificationService.genericAlertError(
                                
                                'CREAR BENEFICIARIO',
                                ` ${retornoMensaje[1]}`,  
                                'success',
                            );
                            // guardarDatosInformacion()
                            // setVerNuemeroCDP(true)
                            
                        }
                       
                  
                        const confirmAlert = await NotificationService.confirmAlert(
                            '¿Está seguro ?',
                        );
                    
        
                        if (confirmAlert  ) {
                            if(getValues2('validacionAprobacionCredito') === false &&  
                             (procedureRequest.flowActivityID === 12 || procedureRequest.flowActivityID === 18)){
                               
                                NotificationService.genericAlertError(        
                                procedureRequest.flowActivityID === 12 ?'Aprobacion Comite':'Aprobacion Desembolso',
                                    `${(mensajeErrorSeven[1])}`,  
                                    'warning',
                                ); 
                              
                            }else{
                                debugger
                                const data: any = {
                                    acT_OBSERVACION: getValues('observation'),
                                    afL_ID: procedureRequest.flowActivityID,
                                    acT_FECHA_MODIFICACION: moment().format(),
                                    trS_ID: procedureRequest.trsID,
                                    actividaD_PRINCIPAL: procedureRequest.principalActivity,
                                    idSolicitudCredito: procedureRequest.requestID,
                                    tipoIdentificacion: creditProductData.tidID,
                                    numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
                                    tipoPago:1
                                    
                                };
                                setIsLoading(false)
            
                                setUltimaAccionEjecutada(type[0]);
                                setProcesandoAccion(true);
                                startRequestActivityProcess(data, type[0]);
                                if (type[0] === REQUEST_ACTIVITY_PROCESS.APPROVE) {
                                    setEstadoActividad('FINALIZADA');
                                    
                                } else {
                                    setEstadoActividad('');
                                }
                                setValue('observation', '');  
                            }
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
            }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    }

    const CrearCDPPrimerPago = async(type:number[]) => {
        
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await credito
            .postCrearCDPPrimerPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){  
                setIsLoading(false)
                const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
                const retornoFallido = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                setNumeroCDP(numCDP[1])

                if(retornoExitoso[1] !== "0"){
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'CONSULTA CDP',
                        'No hay disponibilidad de recursos',
                 
                         'warning',
                    ); 

                    SetErrorCdp(true)
                   
                }else{
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'CONSULTA CDP',
                        ` El numero de CDP asignado es: ${(numCDP[1])}`,  
                        'success',
                    );
                    guardarDatosInformacion(numCDP[1])
                    if (getValues('observation') !== '') {

     
                        const confirmAlert = await NotificationService.confirmAlert(
                            '¿Está seguro ?',
                        );
        
                        if (confirmAlert  ) {
                     
                            if(getValues2('validacionAprobacionCredito') === false &&  
                             (procedureRequest.flowActivityID === 12 || procedureRequest.flowActivityID === 18)){
                             
                                NotificationService.genericAlertError(        
                                procedureRequest.flowActivityID === 12 ?'Aprobacion Comite':'Aprobacion Desembolso',
                                    `${(mensajeErrorSeven[1])}`,  
                                    'warning',
                                ); 
                            
                            }else{
                                
                                const data: any = {
                                    acT_OBSERVACION: getValues('observation'),
                                    afL_ID: procedureRequest.flowActivityID,
                                    acT_FECHA_MODIFICACION: moment().format(),
                                    trS_ID: procedureRequest.trsID,
                                    actividaD_PRINCIPAL: procedureRequest.principalActivity,
                                    idSolicitudCredito: procedureRequest.requestID,
                                    tipoIdentificacion: creditProductData.tidID,
                                    numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
                                   
                                    tipoPago:1
                                };
            
                                setUltimaAccionEjecutada(type[0]);
                                setProcesandoAccion(true);
                                startRequestActivityProcess(data, type[0]);
                                if (type[0] === REQUEST_ACTIVITY_PROCESS.APPROVE) {
                                    setEstadoActividad('FINALIZADA');
                                } else {
                                    setEstadoActividad('');
                                }
                                setValue('observation', '');
        
                            }
                        }          
                }else {
                    NotificationService.genericAlert(
                        'Resumen',
                        'El campo observación es obligatorio .',
                        'warning',)
                }
             
                }
            }else {
                setIsLoading(false)
                toast(`Error: ${response.returnMessage}`, {
                    type: toast.TYPE.ERROR,
                });

            }}
              })
              

             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        


    }

    const CrearCDPSegundoPago = async(data: any) => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await credito
            .postCrearCDPSegundoPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){  
                        setIsLoading(false)
                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
                        setNumeroCDP(numCDP[1])

                        if(retornoExitoso[1] !== "0"){
                            setVerNuemeroCDP(true)
                            NotificationService.genericAlertError(
                                
                                'CONSULTA CDP',
                                'No hay disponibilidad de recursos',
                                'warning',
                            ); 
                        }else{
                            setVerNuemeroCDP(true)
                            NotificationService.genericAlertError(
                                
                                'CONSULTA CDP',
                                ` El numero de CDP asignado es: ${(numCDP[1])}`,  
                                'success',
                            );
                            // CrearFacturaSevenSegundoPago();
                            
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                    });
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        


    }

    const CrearFacturaSevenSegundoPago = async(type:any) => {
            
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await credito
            .postCrearFacturaSevenSegundoPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){ 
                        setIsLoading(false)

                        const retornoExitoso:any = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        if(retornoExitoso[1] !== "0"){
                            NotificationService.genericAlertError(
                                
                                'FACTURA SEGUNDO PAGO',
                                `Rechazado intente nuevamente`,
                                'warning',
                            ); 
                        }else{
                            NotificationService.genericAlertError(
                                

                                'FACTURA SEGUNDO PAGO',
                                'Creada con éxito', 
                                'success',
                            );

                            if (getValues('observation') !== '') {
                            
                                const confirmAlert = await NotificationService.confirmAlert(
                                    '¿Está seguro ?',
                                );
                
                                if (confirmAlert  ) {
                                    if(getValues2('validacionAprobacionCredito') === false &&  
                                     (procedureRequest.flowActivityID === 12 || procedureRequest.flowActivityID === 18)){
                              
                                        NotificationService.genericAlertError(        
                                        procedureRequest.flowActivityID === 12 ?'Aprobacion Comite':'Aprobacion Desembolso',
                                            `${(mensajeErrorSeven[1])}`,  
                                            'warning',
                                        ); 
                                        
                                    }else{
                                        
                                        const data: any = {
                                            acT_OBSERVACION: getValues('observation'),
                                            afL_ID: procedureRequest.flowActivityID,
                                            acT_FECHA_MODIFICACION: moment().format(),
                                            trS_ID: procedureRequest.trsID,
                                            actividaD_PRINCIPAL: procedureRequest.principalActivity,
                                            idSolicitudCredito: procedureRequest.requestID,
                                            tipoIdentificacion: creditProductData.tidID,
                                            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
                                           
                                            tipoPago:1
                                            
                                        };
                                        setIsLoading(false)
                                        setUltimaAccionEjecutada(type[0]);
                                setProcesandoAccion(true);
                                startRequestActivityProcess(data, type[0]);
                                        if (type[0] === REQUEST_ACTIVITY_PROCESS.APPROVE) {
                                            setEstadoActividad('FINALIZADA');
                                            
                                        } else {
                                            setEstadoActividad('');
                                        }
                                        setValue('observation', '');
                
                                    }
                                }          
                        }else {
                
                            NotificationService.genericAlert(
                                'Resumen',
                                'El campo observación es obligatorio .',
                                'warning',)
                               
                        }
                        if(procedureRequest.activity === 'DESEMBOLSO' && procedureRequest.flowActivityID === 28 ){
                            creaSCnMcontAprobacionCredito();
                        }
                            
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }
    
    const guardarDatosInformacion = (cdpOrigin:any) => {

        
        const request = {
            
            simulationID: procedureRequest.simulationID,
            cdp: String(cdpOrigin)
        };
        credito
        .postGuardarCdp(request)
        .then(response => {
            if (response) {
                if (response.isSuccess) {
                    NotificationService.genericAlertError(  
                        'Se guardo Solicitud y notificaciones',
                        `Exitoso`,  
                        'success',
                    );
                }
            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    
       
    };

    const  irCartera = () => {
       if(procedureRequest.flowActivityID === 18){
        history.push({
            pathname: '/AddProductData',
            state: { procedureRequest }   
        })
       }              
    }

 
    const verificaionDocumentosAAA = async (
        
        type: number[],
    ): Promise<void> => {
        debugger
        const accion = type[0];

        if (
            accion === REQUEST_ACTIVITY_PROCESS.APPROVE &&
            !hacerSimulacion &&
            user.USR_ID ===
                '83e91a3c-02a3-4d49-995e-87154620f98d'
        ) {
            await NotificationService.genericAlertError(
                'Simular',
                'Se debe simular para aprobar',
                'warning',
            );

            return;
        }

        const cedulaAfiliado = String(
            procedureRequest?.clientIdentificationNumber || '',
        );
        const cedulaVendedorActual = String(
            documentoVendedor || '',
        );

        if (
            !cedulaAfiliado ||
            !cedulaVendedorActual
        ) {
            await NotificationService.genericAlertError(
                'Datos faltantes',
                'No se encontraron los documentos del afiliado o del vendedor.',
                'warning',
            );

            return;
        }

        const tercerosValidos =
            await validarAfiliadoYVendedor(
                cedulaAfiliado,
                cedulaVendedorActual,
                credito.PostVerificacionTerc,
                NotificationService,
            );

        if (!tercerosValidos) {
            return;
        }

        const confirmar =
            await NotificationService.confirmAlert(
                '¿Está seguro?',
            );

        if (!confirmar) {
            return;
        }

        if (
            accion === REQUEST_ACTIVITY_PROCESS.APPROVE &&
            procedureRequest?.activity ===
                'BLOQUEO DE RECURSOS'
        ) {
            await registrarBloqueoCuentaCredito();
        }

        if (
            accion === REQUEST_ACTIVITY_PROCESS.APPROVE &&
            getValues2(
                'validacionAprobacionCredito',
            ) === false &&
            procedureRequest.flowActivityID === 12
        ) {
            await NotificationService.genericAlertError(
                'Aprobación Comité',
                mensajeErrorSeven?.[1] ||
                    'No fue posible validar la aprobación del comité.',
                'warning',
            );

            return;
        }

        if (
            accion ===
            REQUEST_ACTIVITY_PROCESS.APPROVE
        ) {
            const documentosEnviados =
                await enviarDocumentosDODO();

            if (!documentosEnviados) {
                return;
            }
        }

        const data: any = {
            acT_OBSERVACION:
                getValues('observation'),
            afL_ID:
                procedureRequest.flowActivityID,
            acT_FECHA_MODIFICACION:
                moment().format(),
            trS_ID:
                procedureRequest.trsID,
            actividaD_PRINCIPAL:
                procedureRequest.principalActivity,
            NUMERO_DOCUMENTO: String(procedureRequest?.clientIdentificationNumber),
            RADICADO: procedureRequest?.numerO_RADICADO,
        };

        setUltimaAccionEjecutada(
            accion,
        );
        setProcesandoAccion(
            true,
        );

        startRequestActivityProcess(
            data,
            accion,
        );

        if (
            accion ===
            REQUEST_ACTIVITY_PROCESS.APPROVE
        ) {
            setEstadoActividad(
                'FINALIZADA',
            );
        } else {
            setEstadoActividad('');
        }

        setValue(
            'observation',
            '',
        );
    };

    const validarAvaluoComercialDiligenciado =
    async (): Promise<boolean> => {
        try {
            const socId =
                procedureRequest?.requestID;

            if (!socId) {
                await NotificationService.genericAlert(
                    'Avalúo comercial',
                    'No se encontró el identificador de la solicitud.',
                    'warning',
                );

                return false;
            }

            const response =
                await credito.getConsultarAvaluoComercialCredito(
                    socId,
                );

            const responseNormalizada =
                response?.data &&
                response?.isSuccess === undefined
                    ? response.data
                    : response;

            if (
                responseNormalizada?.isSuccess !==
                    true ||
                !responseNormalizada?.data
            ) {
                await NotificationService.genericAlert(
                    'Avalúo comercial pendiente',
                    'Debe diligenciar y guardar la información del avalúo comercial antes de aprobar la solicitud.',
                    'warning',
                );

                setCurrentTabValue(
                    TABS_INDEX.AVALUO_COMERCIAL,
                );

                return false;
            }

            const avaluo =
                responseNormalizada.data;

            const resumen =
                avaluo?.resumenEjecutivo || {};

            const antecedentes =
                avaluo?.antecedentes || {};

            const aspectosJuridicos =
                avaluo?.aspectosJuridicos || {};

            const observaciones =
                avaluo?.observacionesGenerales ||
                {};

            const dependencias =
                avaluo?.dependencias || {};

            const construccion =
                avaluo?.construccion || {};

            const cuadroValoracion =
                avaluo?.cuadroValoracion || {};

            const tieneValor = (
                valor: any,
            ): boolean =>
                valor !== null &&
                valor !== undefined &&
                String(valor).trim() !== '';

            const camposFaltantes: string[] =
                [];

            /*
             * Resumen ejecutivo
             */
            if (!tieneValor(resumen.cliente)) {
                camposFaltantes.push('Cliente');
            }

            if (
                !tieneValor(
                    resumen.numeroDocumento,
                )
            ) {
                camposFaltantes.push(
                    'Número de documento',
                );
            }

            if (!tieneValor(resumen.direccion)) {
                camposFaltantes.push(
                    'Dirección',
                );
            }

            if (!tieneValor(resumen.sector)) {
                camposFaltantes.push('Sector');
            }

            if (!tieneValor(resumen.barrio)) {
                camposFaltantes.push('Barrio');
            }

            if (
                !tieneValor(
                    resumen.departamentoId,
                )
            ) {
                camposFaltantes.push(
                    'Departamento',
                );
            }

            if (!tieneValor(resumen.ciudadId)) {
                camposFaltantes.push('Ciudad');
            }

            /*
             * Antecedentes
             */
            if (
                !tieneValor(
                    antecedentes.fechaInforme,
                )
            ) {
                camposFaltantes.push(
                    'Fecha informe',
                );
            }

            if (
                !tieneValor(
                    antecedentes.vetustez,
                )
            ) {
                camposFaltantes.push(
                    'Vetustez',
                );
            }

            if (
                !tieneValor(
                    antecedentes.fechaPagoAvaluo,
                )
            ) {
                camposFaltantes.push(
                    'Fecha pago avalúo',
                );
            }

            if (
                !tieneValor(
                    antecedentes.fechaVisita,
                )
            ) {
                camposFaltantes.push(
                    'Fecha visita',
                );
            }

            /*
             * Aspectos jurídicos
             */
            if (
                !tieneValor(
                    aspectosJuridicos.propietario,
                )
            ) {
                camposFaltantes.push(
                    'Propietario',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos.escritura,
                )
            ) {
                camposFaltantes.push(
                    'Escritura',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos.notaria,
                )
            ) {
                camposFaltantes.push('Notaría');
            }

            if (
                !tieneValor(
                    aspectosJuridicos.fechaEscritura,
                )
            ) {
                camposFaltantes.push(
                    'Fecha escritura',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos
                        .departamentoEscrituraId,
                )
            ) {
                camposFaltantes.push(
                    'Departamento escritura',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos
                        .ciudadEscrituraId,
                )
            ) {
                camposFaltantes.push(
                    'Ciudad escritura',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos
                        .cedulaCatastral,
                )
            ) {
                camposFaltantes.push(
                    'Cédula catastral',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos
                        .licenciaConstruccion,
                )
            ) {
                camposFaltantes.push(
                    'Licencia de construcción',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos
                        .nombreConjunto,
                )
            ) {
                camposFaltantes.push(
                    'Nombre conjunto',
                );
            }

            if (
                !tieneValor(
                    aspectosJuridicos
                        .coeficienteCopropiedad,
                )
            ) {
                camposFaltantes.push(
                    'Coeficiente de copropiedad',
                );
            }

            /*
             * Observaciones generales
             */
            if (
                !tieneValor(
                    observaciones
                        .distribucionInmueble,
                )
            ) {
                camposFaltantes.push(
                    'Distribución del inmueble',
                );
            }

            if (
                !tieneValor(
                    observaciones
                        .caracteristicasAdicionales,
                )
            ) {
                camposFaltantes.push(
                    'Características adicionales',
                );
            }

            /*
             * Dependencias
             */
            if (
                !tieneValor(
                    dependencias.descripcion,
                )
            ) {
                camposFaltantes.push(
                    'Descripción',
                );
            }

            if (
                !tieneValor(
                    dependencias.garaje,
                )
            ) {
                camposFaltantes.push('Garaje');
            }

            const tieneGaraje =
                String(
                    dependencias.garaje || '',
                )
                    .trim()
                    .toUpperCase() === 'SI';

            if (
                tieneGaraje &&
                !tieneValor(
                    dependencias.descripcionGaraje,
                )
            ) {
                camposFaltantes.push(
                    'Descripción del garaje',
                );
            }

            /*
             * Construcción
             */
            if (
                !tieneValor(
                    construccion
                        .salaAreaConstruida,
                )
            ) {
                camposFaltantes.push(
                    'Área construida',
                );
            }

            if (
                !tieneValor(
                    construccion.areaPrivada,
                )
            ) {
                camposFaltantes.push(
                    'Área privada',
                );
            }

            /*
             * Cuadro de valoración
             */
            if (
                !tieneValor(
                    cuadroValoracion.inmueble,
                )
            ) {
                camposFaltantes.push(
                    'Valor comercial del inmueble',
                );
            }

            if (camposFaltantes.length > 0) {
                await NotificationService.genericAlert(
                    'Avalúo comercial incompleto',
                    `Debe completar los datos del avalúo comercial antes de aprobar. Campos pendientes: ${camposFaltantes.join(
                        ', ',
                    )}.`,
                    'warning',
                );

                setCurrentTabValue(
                    TABS_INDEX.AVALUO_COMERCIAL,
                );

                return false;
            }

            return true;
        } catch (error) {
            await NotificationService.genericAlert(
                'Avalúo comercial',
                'No fue posible validar la información del avalúo comercial. Intente nuevamente.',
                'error',
            );

            return false;
        }
    };

    const handleActionButton = async (
        type: number[],
    ): Promise<void> => {
        const accion = type[0];

        if (procesandoAccion) {
            return;
        }

        const tienePestanaAvaluoComercial =
    procedureRequest?.panels?.includes(
        REQUEST_ACTIVITY_TABS.AVALUO_COMERCIAL,
    );

if (
    accion ===
        REQUEST_ACTIVITY_PROCESS.APPROVE &&
    tienePestanaAvaluoComercial
) {
    const avaluoCompleto =
        await validarAvaluoComercialDiligenciado();

    if (!avaluoCompleto) {
        return;
    }
}

        if (
            accion ===
            REQUEST_ACTIVITY_PROCESS.REJECTED
        ) {
            const confirmar =
                await NotificationService.confirmAlert(
                    'Confirmar rechazo',
                    '¿Está seguro de que desea rechazar esta solicitud?',
                );

            if (!confirmar) {
                return;
            }

            const data: any = {
                acT_OBSERVACION:
                    getValues('observation'),
                afL_ID:
                    procedureRequest.flowActivityID,
                acT_FECHA_MODIFICACION:
                    moment().format(),
                trS_ID:
                    procedureRequest.trsID,
                actividaD_PRINCIPAL:
                    procedureRequest.principalActivity,
                numerO_DOCUMENTO:
                    String(
                        procedureRequest
                            ?.clientIdentificationNumber ||
                            '',
                    ),
            };

            setUltimaAccionEjecutada(
                accion,
            );
            setProcesandoAccion(
                true,
            );

            startRequestActivityProcess(
                data,
                accion,
            );

            setEstadoActividad(
                'RECHAZADA',
            );
            setValue(
                'observation',
                '',
            );

            return;
        }
       

        if (
            procedureRequest?.flowActivityID === 16 &&
            (
                datosObligatoriosDesemboloso ===
                    'Error' ||
                datosObligatoriosDesemboloso ===
                    ''
            ) &&
            accion !==
                REQUEST_ACTIVITY_PROCESS.DESISTIR
        ) {
            await NotificationService.genericAlert(
                'Datos Desembolso',
                'Tienes que diligenciar los datos de desembolso.',
                'warning',
            );

            return;
        }

        if (
            !String(
                getValues('observation') ||
                    '',
            ).trim()
        ) {
            await NotificationService.genericAlert(
                'Resumen',
                'El campo observación es obligatorio.',
                'warning',
            );

            return;
        }

        if (
            procedureRequest.activity ===
                'DESEMBOLSO' &&
            accion ===
                REQUEST_ACTIVITY_PROCESS.APPROVE
        ) {
            await CrearFacturaSevenSegundoPago(
                type,
            );

            return;
        }

        if (
            procedureRequest.activity ===
                'FINANCIERA' &&
            accion ===
                REQUEST_ACTIVITY_PROCESS.APPROVE
        ) {
            await CrearCDPSegundoPagoFuncional(
                type,
            );

            return;
        }

        if (
            accion ===
            REQUEST_ACTIVITY_PROCESS.DESISTIR
        ) {
            try {
                const confirmado =
                    await NotificationService.confirmAlert(
                        '¿Está seguro?',
                    );

                if (!confirmado) {
                    return;
                }

                const data: any = {
                    acT_OBSERVACION:
                        getValues('observation'),
                    afL_ID:
                        procedureRequest.flowActivityID,
                    acT_FECHA_MODIFICACION:
                        moment().format(),
                    trS_ID:
                        procedureRequest.trsID,
                    actividaD_PRINCIPAL:
                        procedureRequest.principalActivity,
                };

                setUltimaAccionEjecutada(
                    accion,
                );
                setProcesandoAccion(
                    true,
                );

                startRequestActivityProcess(
                    data,
                    accion,
                );

                setEstadoActividad(
                    'DESISTIDA',
                );
                setValue(
                    'observation',
                    '',
                );
            } catch {
                setProcesandoAccion(
                    false,
                );

                await NotificationService.genericAlert(
                    'Error',
                    'No fue posible desistir la solicitud. Intenta nuevamente.',
                    'error',
                );
            }

            return;
        }

        /*
         * DEVOLVER se procesa desde el modal.
         * No debe continuar a las validaciones de aprobación.
         */
        if (
            accion ===
            REQUEST_ACTIVITY_PROCESS.BACK
        ) {
            setOpenModal(
                true,
            );

            return;
        }

        if (
            procedureRequest
                ?.flowActivityID !== 12
        ) {
            await listarArchivos();
        }

        if (
            procedureRequest
                ?.flowActivityID === 2
        ) {
            await actializarPeritoPreTramite();
        }

        /*
         * Estas funciones únicamente consultan y actualizan
         * las validaciones. Ya no aprueban internamente.
         */
        if (
            procedureRequest.flowActivityID ===
            4
        ) {
            await obtenerSolicCreditoInfTecnica(
                type,
            );
        }

        if (
            procedureRequest.flowActivityID ===
            7
        ) {
            await obtenerCreditLegalData(
                type,
            );
        }

        if (
            procedureRequest.flowActivityID ===
            6
        ) {
            await ObtenerAliadosEstrategicos(
                type,
            );
        }

        if (
            procedureRequest.flowActivityID ===
            8
        ) {
            await obtenerFichaAnalisticaPorId(
                type,
            );
            await obtenerValidacionTelefonica(
                type,
            );
        }

        if (
            procedureRequest.flowActivityID ===
            16
        ) {
            await obtnerBenficiarios();
        }

        if (
            getValues2(
                'validacionEstudioTecnico',
            ) === null &&
            procedureRequest.flowActivityID ===
                4
        ) {
            await NotificationService.genericAlert(
                'Datos estudio técnico',
                'Completar Campos Obligatorios *.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionEstudioLegal',
            ) === null &&
            procedureRequest.flowActivityID ===
                7
        ) {
            await NotificationService.genericAlert(
                'Datos estudio legal',
                'Completar Campos Obligatorios *.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionAliadoEstrategico',
            ) === null &&
            procedureRequest.flowActivityID ===
                6
        ) {
            await NotificationService.genericAlert(
                'Datos Aliados Estratégicos',
                'Completar Campos Obligatorios *.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionFichaAnalista',
            ) === '[]' &&
            procedureRequest.flowActivityID ===
                8
        ) {
            await NotificationService.genericAlert(
                'Datos Estudio Financiero',
                'Completar Campos Ficha Analista Obligatorios *.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionTelefonicaAfiliado',
            ) === null &&
            procedureRequest.flowActivityID ===
                8
        ) {
            await NotificationService.genericAlert(
                'Validación Telefónica',
                'Completar Campos Obligatorios *.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionBeneficiario',
            ) === 0 &&
            procedureRequest.flowActivityID ===
                16
        ) {
            await NotificationService.genericAlert(
                'Información Beneficiario',
                'Completar Campos Obligatorios *.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionDocuemntos',
            ) &&
            procedureRequest.flowActivityID !==
                12
        ) {
            await NotificationService.genericAlert(
                'Subir Archivos',
                'Los Documentos con "SI" son obligatorios.',
                'warning',
            );

            return;
        }

        if (
            getValues2(
                'validacionDocuemntos2',
            ) &&
            procedureRequest.flowActivityID ===
                16
        ) {
            await NotificationService.genericAlert(
                'Subir Archivos',
                'Los Documentos con "SI" deben ser aprobados.',
                'warning',
            );

            return;
        }

        /*
         * Único punto para aprobar el flujo normal.
         */
        await verificaionDocumentosAAA(
            type,
        );
    };

      
    useEffect(() => {
        
        const buttonLabels:any = {
            "BLOQUEO DE RECURSOS": "BLOQUEAR",
            "PIGNORACION DE RECURSOS": "PIGNORAR",
            "LEGALIZACIÓN":"LEGALIZAR"
        };
        setCambiarNombreBotonAprobar(buttonLabels[procedureRequest?.activity] || 'APROBAR');
        
    }, [procedureRequest]);
    
    const registrarBloqueoCuentaCredito = async() => {
        try {
            const request = {
                tipoIdentificacion: 1,
                numeroIdentificacion: String(procedureRequest.clientIdentificationNumber)
            }
            const response = await credito.postRegistrarBloqueoCuentaCredito(request);
            if (response?.isSuccess) {
                toast('Datos de afiliado con cuenta bloqueada', { type: toast.TYPE.SUCCESS });
            }
        } catch (error) {
            console.log(error);
        }
    }



    if (isLoading) {
        return <CHLoadingProcess />;
    }
    
    return (
        <>
            <CHPageTitle
                topText={
                    typeRequest === REQUEST_TYPE_REQUEST.SEVERANCE
                        ? 'Solicitud'
                        : 'Información Simulación'
                }
                subText=""
                title1={
                    typeRequest === REQUEST_TYPE_REQUEST.SEVERANCE
                        ? 'Solicitud de'
                        : 'Análisis de'
                }
                title2={
                    typeRequest === REQUEST_TYPE_REQUEST.SEVERANCE
                        ? 'Cesantías'
                        : 'Crédito'
                }
            />
              
            <ViewTabs
                currentTab={currentTabValue}
                onTabChange={handleTabChange}
                permissions={procedureRequest.panels}
            />
           <Panel
    documentosObligatorios={documentosObligatorios}
    typeRequest={typeRequest}
    isLoadingCredit={isLoadingCredit}
    informacionAfiliado={informacionAfiliado}
    currentTabValue={currentTabValue}
    procedureRequest={procedureRequest}
    creditBasicData={
        creditBasicData.accID === undefined
            ? informationBasicSeverance
            : creditBasicData
    }
    creditProductData={creditProductData}
    creditObservationsData={creditObservationsData}
    catalogState={catalogState}
    startPostCreditBasicData={startPostCreditBasicData}
    startPostCreditProductData={startPostCreditProductData}
    startPostCreditObservationsData={
        startPostCreditObservationsData
    }
    startGetAllPostInformationBasicSeverance={
        startGetAllPostInformationBasicSeverance
    }
    control={control}
    validarDocObligatorios={
        validarDocObligatorios
    }
    obterReporteHijo={obterReporteHijo}
    validacionDeTablasDatosDesembolso={
        validacionDeTablasDatosDesembolso
    }
    setHacerSimulacion={
        setHacerSimulacion
    }
    documentosTemporales={
        documentosTemporales
    }
    setDocumentosTemporales={
        setDocumentosTemporales
    }
/>
            <div className="buttons">
                {estadoActividad === 'PENDIENTE' && (
                    <>
                    { console.log('desistir', procedureRequest.flowActivityID)}
                    { console.log('desistir', procedureRequest)}
                       { procedureRequest?.activity === 'LEGALIZACIÓN' || procedureRequest.flowActivityID === 16 ||   procedureRequest.flowActivityID === 8 ||  procedureRequest.flowActivityID === 12 || 
                       procedureRequest.flowActivityID === 21 || procedureRequest.flowActivityID ===  31 || procedureRequest.flowActivityID ===  32 || procedureRequest.flowActivityID ===  23  ||
                       procedureRequest.flowActivityID ===  24 || procedureRequest.flowActivityID ===  25 || procedureRequest.flowActivityID === 26 ||  procedureRequest.flowActivityID ===  28 ||
                       procedureRequest.flowActivityID ===  29 || procedureRequest.flowActivityID ===  30  || procedureRequest.flowActivityID ===  33 || procedureRequest.flowActivityID ===  18 && 
                       <Button
                            className={classes.button}
                            variant="outlined"
                            disabled={procesandoAccion || isLoading}
                            onClick={handleSubmit(
                                HelperService.partial(
                                    handleActionButton,
                                    REQUEST_ACTIVITY_PROCESS.DESISTIR,
                                ),
                            )}
                        >
                            DESISTIR
                        </Button>}
                       
                        <Button
                            className={classes.button}
                            variant="outlined"
                            disabled={procesandoAccion || isLoading}
                            onClick={handleSubmit(
                                HelperService.partial(
                                    handleActionButton,
                                    REQUEST_ACTIVITY_PROCESS.APPROVE,
                                ),
                                
                            )}
                        >
                            {cambiarNombreBotonAprobar} 
                        </Button>



                        {
                            
                            procedureRequest?.devuelveFlujo === 'S'
                                || procedureRequest.flowActivityID === 25
                                || procedureRequest.flowActivityID === 24 
                                || procedureRequest.flowActivityID ===  23
                                ||  procedureRequest.flowActivityID === 26
                                || procedureRequest.flowActivityID === 28
                                || procedureRequest.flowActivityID === 7
                                || procedureRequest.flowActivityID === 4
                                ?
                                <Button
                                    className={classes.button}
                                    variant="outlined"
                                    disabled={procesandoAccion || isLoading}
                                    onClick={handleSubmit(
                                        HelperService.partial(
                                            handleActionButton,
                                            REQUEST_ACTIVITY_PROCESS.BACK ,
                                        ),
                                    )}

                                >
                                    DEVOLVER
                                </Button> : null
                        }
                        {
                            procedureRequest?.rechazaFlujo === 'S' || procedureRequest.flowActivityID === 2 &&
                            <Button
                                className={classes.button}
                                variant="outlined"
                                onClick={handleSubmit(
                                    HelperService.partial(
                                        handleActionButton,
                                        REQUEST_ACTIVITY_PROCESS.REJECTED,
                                    ),
                                )}
                            >
                                RECHAZAR
                            </Button>
                        }

                    </>
                )}
                { console.log(' console.log', procedureRequest)}

                {procedureRequest.flowActivityID === 18 && estadoActividad === 'FINALIZADA' ?
                        <Button
                            className={classes.button}
                            variant="outlined"
                            onClick={() => { irCartera() }}
                        >Ir a Cartera
                        </Button> : null
                }
                {estadoActividad === 'PENDIENTE' && (
<>
{procedureRequest.flowActivityID === 16 &&
                            <Button
                                className={classes.button}
                                variant="outlined"
                                disabled={procesandoAccion || isLoading}
                                onClick={handleSubmit(
                                    HelperService.partial(
                                        handleActionButton,
                                        REQUEST_ACTIVITY_PROCESS.DESISTIR,
                                    ),
                                )}
                            >
                                DESISTIR
                            </Button>
                    }</>
                 
                )}

                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onItemGoBack}
                >
                    REGRESAR
                </Button>
            </div>
            <ModalDevolverFlujos             
                open={openModal}
                handleClose={handleCloseModal}
                obterFlujoModalHijo={obterFlujoModalHijo}
                procedureRequest={procedureRequest}
                dataType= {dataType}
                obsrvacionDevolver = {getValues('observation')}
                
            />
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    isLoadingCredit: state.credit.loading,
    creditBasicData: state.credit.creditBasicData,
    creditProductData: state.credit.creditProductData,
    creditObservationsData: state.credit.creditObservationsData,
    error: state.credit.error,
    responseRequestActivity: state.credit.responseRequestActivity,
    informationBasicSeverance: state.severance.informationBasicSeverance,
    formularioVendedor: state.credit.formularioVendedor,
    creditFilesData: state.credit.creditFilesData,
});

const mapDispatchToProps: any = {
    startRequestActivityProcess,
    startPostCreditBasicData,
    startPostCreditProductData,
    startPostCreditObservationsData,
    resetCreditState,
    startGetAllPostInformationBasicSeverance,
    startObtenerVendedorCredito,
    startPostCreditFilesData
};

export default connect(mapStateToProps, mapDispatchToProps)(RequestInformation);