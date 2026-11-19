import React, { useEffect, useState } from 'react';
import './ProcedureRequests.styles.scss';
import { Box, Button, Grid, MenuItem } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import { AppState } from '../../store/rootReducer';
import {
    startGetActivitiesStatus,
    startGetAllIdentificationTypes,
    startGetCreditType,
} from '../../actions/catalog.action';
import { CreditTypeModel } from '../../models/credit-type.model';
import { ActivityStatusModel } from '../../models/activity-status.model';
import {
    clearDetailRequestInformation,
    resetCreditState,
    startPostProcedureRequest,
    startObtenerVendedorCredito,
    startPostCreditFilesData
} from '../../actions/credit.actions';
import { ProcedureRequestRequestModel } from '../../models/requests/procedure-request-request.model';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import UtilService from '../../utils/util.service';
import RequestInformation from '../../components/RequestInformation/RequestInformation';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import { CreditBasicDataModel } from '../../models/credit-basic-data.model';
import { CreditObservationModel } from '../../models/credit-observation.model';
import { CreditProductModel } from '../../models/credit-product.model';
import { CreditObservationsRequestModel } from '../../models/requests/credit-observations.request.model';
import { CreditProductRequestModel } from '../../models/requests/credit-product-request.model';
import { CreditBasicDataRequestModel } from '../../models/requests/credit-basic-data-request.model';
import { RequestActivityRequestModel } from '../../models/requests/request-activity-request.model';
import { CreditFilesRequestModel } from '../../models/requests/credit-files-request.model';
import { CreditFileModel } from '../../models/credit-file.model';
import CreditTable from './CreditTable';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import AuditoriaApi from '../../services/api-auditorias.servise'
import { Auditoria } from '../../types/Auditoria.model.type';
import ApiReportes from '../../services/api-reportes.service';
import { LoginState, } from '../../reducers/login.reducer';
import { CatalogState } from '../../reducers/catalog.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },

    }),
);


function ProcedureRequests({
    creditTypes,
    activitiesStatus,
    isLoadingCatalog,
    isLoadingCredit,
    loginState,
    procedureRequests,
    catalogState,
    typeRequest,
    creditBasicData,
    creditProductData,
    creditObservationsData,
    error,
    responseRequestActivity,
    informationBasicSeverance,
    formularioVendedor,
    creditFilesData,
    startGetCreditType,
    startGetActivitiesStatus,
    startPostProcedureRequest,
    resetCreditState,
    clearDetailRequestInformation,
    startRequestActivityProcess,
    startPostCreditBasicData,
    startPostCreditProductData,
    startPostCreditObservationsData,
    startGetAllPostInformationBasicSeverance,
    startObtenerVendedorCredito,
    startPostCreditFilesData,
    startGetAllIdentificationTypes
}: {
    creditTypes: CreditTypeModel[];
    activitiesStatus: ActivityStatusModel[];
    loginState: LoginState;
    catalogState: CatalogState;
    isLoadingCatalog: boolean;
    isLoadingCredit: boolean;
    procedureRequests: ProcedureRequestModel[];
    typeRequest?: string;
    error: any;
    responseRequestActivity: any;
    creditBasicData: CreditBasicDataModel;
    creditProductData: CreditProductModel;
    creditObservationsData: CreditObservationModel[];
    informationBasicSeverance: CreditBasicDataModel;
    formularioVendedor: any;
    creditFilesData: CreditFileModel[];
    startGetCreditType: () => void;
    startGetActivitiesStatus: () => void;
    resetCreditState: () => void;
    startPostProcedureRequest: (data: ProcedureRequestRequestModel) => void;
    clearDetailRequestInformation: () => void;
    startRequestActivityProcess: (data: RequestActivityRequestModel, requestType: number,) => void;
    startPostCreditBasicData: (data: CreditBasicDataRequestModel) => void;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    startPostCreditObservationsData: (data: CreditObservationsRequestModel,) => void;
    startGetAllPostInformationBasicSeverance: (numeroIdentificacion: number,) => void;
    startObtenerVendedorCredito: (simulationID: any) => void;
    startPostCreditFilesData: (data: CreditFilesRequestModel) => void;
    startGetAllIdentificationTypes: (claims: string) => void;

}): any {

    const classes = useStyles();

    const [userID, setUserID] = useState('');
    const [creditType, setCreditType] = useState(0);
    const [activityStatus, setActivityStatus] = useState(1);
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [nuevaLista, setNuevaLista] = useState<CreditTypeModel[]>([]);
    const [selectedData, setSelectedData] =
        useState<ProcedureRequestModel | null>(null);

    if (procedureRequests.length > 0 && !isDataLoaded) {
        resetCreditState();
    }

    useEffect(() => {
        document.title = 'Procedimientos Solicitudes | Caja Honor';
        startGetCreditType();
        startGetActivitiesStatus();
        startGetAllIdentificationTypes(loginState.claims);
    }, []);


    useEffect(() => {
        const listaCredito = creditTypes.filter(x => x.id !== 3)
        setNuevaLista(listaCredito);
    }, [creditTypes])

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const apiReportes = ApiReportes();

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

    const onSubmit = () => {

        setIsDataLoaded(true);
        const data: ProcedureRequestRequestModel = {};
        if (userID !== '') {
            data.identificacion = userID;
        }
        if (creditType !== 0) {
            data.tipoCredito = creditType;
        }

        if (activityStatus !== 0) {
            data.estado = activityStatus;
        }

        let descripcionEtapa = 'No especificado';
        let descripcionTipoCredito = 'No especificado';

        if (data.estado === 6) {
            descripcionEtapa = 'EN DEVOLUCIÓN';
        } else if (data.estado === 5) {
            descripcionEtapa = 'RECHAZADA';
        } else if (data.estado === 4) {
            descripcionEtapa = 'FINALIZADA';
        } else if (data.estado === 2) {
            descripcionEtapa = 'EN TRÁMITE';
        } else if (data.estado === 1) {
            descripcionEtapa = 'PENDIENTE';
        }



        if (data.tipoCredito === 4) {
            descripcionTipoCredito = 'CREDITO EDUCATIVO';
        } else if (data.tipoCredito === 2) {
            descripcionTipoCredito = 'CRÉDITO HIPOTECARIO';
        } else if (data.tipoCredito === 1) {
            descripcionTipoCredito = 'LEASING HABITACIONAL';
        }

        const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consultar Hipotecario/Leasing',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de solicitudes de crédito con los siguientes filtros:
                - No. documento cliente: ${data.identificacion || 'No especificada'}
                - Etapa proceso: ${descripcionEtapa || 'No especificado'}
                - Tipo crédito: ${descripcionTipoCredito || 'No especificado'}`,
            } as Auditoria;
    
            auditoriaApi.InsertAuditoria(requestAuditoria);
    
            startPostProcedureRequest(data);
        } catch (error: any) {
            const descripcionError = `Error en consulta de información crédito
            - No. documento cliente: ${data.identificacion || 'No especificada'}
            - Etapa proceso: ${descripcionEtapa || 'No especificado'}
            - Tipo crédito: ${descripcionTipoCredito || 'No especificado'}`;

                const requestAuditoriaError: Auditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Consultar Hipotecario/Leasing',
                    hostname: document.location.host,
                    tipoAccion: 'ERRORES Y EXCEPCIONES',
                    idTipoAccion: 8,
                    descripcion: descripcionError
                };

                auditoriaApi.InsertAuditoria(requestAuditoriaError);

                toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
        }

       
    };

    const onItemIsSelected = (data: ProcedureRequestModel) => {
        setSelectedData(data);
    };

    const onItemGoBack = () => {
        clearDetailRequestInformation();
        setSelectedData(null);
        const data: ProcedureRequestRequestModel = {};
        if (userID !== '') {
            data.identificacion = userID;
        }
        if (creditType !== 0) {
            data.tipoCredito = creditType;
        }
        if (activityStatus !== 0) {
            data.estado = activityStatus;
        }
        startPostProcedureRequest(data);
    };

    const reporteExcel = async () => {
        console.log('procedureRequests',procedureRequests)
        if (!procedureRequests || procedureRequests.length === 0) {
          toast('No hay datos para exportar', { type: toast.TYPE.INFO });
          return;
        }
        const columnName = [
          'Fecha Solicitud',
          'No. Radicado',
          'Tipo de Crédito',
          'No. Documento Cliente',
          'Actividad Actual',
          'Estado',
          'Observación'
        ];
        const columnDataList = (procedureRequests ?? []).map((r: any) => ({
          data: [
            UtilService.getDate(r?.requestDate),
            String(r?.numerO_RADICADO ?? ''),       
            String(r?.creditType ?? ''),               
            String(r?.clientIdentificationNumber ?? ''), 
            String(r?.activity ?? ''),                 
            String(r?.stateActivity ?? ''),            
            String(r?.observacioN_ACTIVIDAD?? 'SIN OBSERVACIÓN'),            
          ],
        }));
      
    
        const request = { columnDataList, columnName };
        try {
         
          const resp = await apiReportes.GenerarExcels(request);
          const buffer = (resp?.data ?? resp) as ArrayBuffer;
          const blob = new Blob(
            [buffer],
            { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
          );
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `Solicitudes_${new Date().toISOString().slice(0,10)}.xlsx`;
          document.body.appendChild(a);
          a.click();
          a.remove();
          window.URL.revokeObjectURL(url);
        } catch (e) {
          console.error(e);
          toast('No se pudo generar el Excel', { type: toast.TYPE.ERROR });
        }
      };
      
   
      const canShowExport = (procedureRequests ?? []).some(
        (a: any) => a?.activity === 'EVALUACIÓN CRÉDITO'
    );
    
    if (selectedData != null) {
        return (
            <CHPrivateContainer>
    
              
    
                <RequestInformation
                    typeRequest={typeRequest}
                    catalogState={catalogState}
                    creditBasicData={creditBasicData}
                    creditProductData={creditProductData}
                    creditObservationsData={creditObservationsData}
                    error={error}
                    responseRequestActivity={responseRequestActivity}
                    informationBasicSeverance={informationBasicSeverance}
                    isLoadingCredit={isLoadingCredit}
                    procedureRequest={selectedData}
                    formularioVendedor={formularioVendedor}
                    creditFilesData={creditFilesData}
                    startPostCreditFilesData={startPostCreditFilesData}
                    startObtenerVendedorCredito={startObtenerVendedorCredito}
                    startRequestActivityProcess={startRequestActivityProcess}
                    startPostCreditBasicData={startPostCreditBasicData}
                    startPostCreditProductData={startPostCreditProductData}
                    startPostCreditObservationsData={startPostCreditObservationsData}
                    startGetAllPostInformationBasicSeverance={startGetAllPostInformationBasicSeverance}
                    onItemGoBack={onItemGoBack}
                />
            </CHPrivateContainer>
        );
    }

    if (isLoadingCredit || isLoadingCatalog) {
        return (
            <CHPrivateContainer>
                <CHLoadingProcess />
            </CHPrivateContainer>
        );
    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Solicitudes de Crédito"
                subText=""
                title1="Gestión solicitudes"
                title2="de Crédito"
            />
            <CHPaper title="Información Crédito">
                <form>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <CHTextField
                                isReadOnly={false}
                                id="client-id"
                                value={userID}
                                onChange={e => {
                                    setUserID(e.target.value);
                                }}
                                label="No. Documento Cliente"
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <CHSelect
                                defaultValue=""
                                label="Etapa Proceso"
                                value={activityStatus}
                                onChange={e => {
                                    const value = e.target.value as number;
                                    setActivityStatus(value);
                                }}
                            >
                                <MenuItem value={0} key="activity-status-0">
                                    TODOS
                                </MenuItem>
                                {activitiesStatus?.map((val, key) => (
                                    <MenuItem
                                        value={val.id}
                                        key={`activity-status-${key}`}
                                    >
                                        {val.description}
                                    </MenuItem>
                                ))}
                            </CHSelect>
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <CHSelect
                                defaultValue="0"
                                label="Tipo Crédito"
                                value={creditType}
                                onChange={e => {
                                    const value = e.target.value as number;
                                    setCreditType(value);
                                }}
                            >
                                <MenuItem value={0} key="credit-type-0">
                                    TODOS
                                </MenuItem>
                                {nuevaLista?.map((val, key) => (
                                    <MenuItem
                                        value={val.id}
                                        key={`credit-type-${key}`}
                                    >
                                        {val.description}
                                    </MenuItem>
                                ))}
                            </CHSelect>
                        </Grid>
                    </Grid>
                </form>
            </CHPaper>
            <Box
                display="flex"
                justifyContent="flex-end"
                alignItems="center"
              
                mt={2}
            >
                {canShowExport && (
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={reporteExcel}
                    >
                        Generar Reporte Excel
                    </Button>
                )}
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmit}
                >
                    CONSULTAR
                </Button>
            </Box>

            <CreditTable
                data={procedureRequests}
                onClickItem={onItemIsSelected}
                isDataLoaded={isDataLoaded}
            />
        </CHPrivateContainer>
    );
}

const mapStateToProps = (state: AppState) => ({
    creditTypes: state.catalog.creditTypes,
    activitiesStatus: state.catalog.activitiesStatus,
    isLoadingCatalog: state.catalog.loading,
    isLoadingCredit: state.credit.loading,
    procedureRequests: state.credit.procedureRequests,
    formularioVendedor: state.credit.formularioVendedor,
    creditFilesData: state.credit.creditFilesData,
    loginState: state.login,
    catalogState: state.catalog,
});

const mapDispatchToProps = {
    startGetCreditType,
    startGetActivitiesStatus,
    startPostProcedureRequest,
    resetCreditState,
    clearDetailRequestInformation,
    startObtenerVendedorCredito,
    startPostCreditFilesData,
    startGetAllIdentificationTypes
};

export default connect(mapStateToProps, mapDispatchToProps)(ProcedureRequests);