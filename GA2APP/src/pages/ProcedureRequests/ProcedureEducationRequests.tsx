import React, { useEffect, useState } from 'react';
import './ProcedureRequests.styles.scss';
import { Button, Grid, MenuItem } from '@material-ui/core';
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
import RequestInformationEducation from '../../components/RequestInformation/RequestInformationEducation';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise'

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



function ProcedureEducationRequests({
    startGetCreditType,
    startGetActivitiesStatus,
    creditTypes,
    activitiesStatus,
    isLoadingCatalog,
    isLoadingCredit,
    startPostProcedureRequest,
    procedureRequests,
    resetCreditState,
    clearDetailRequestInformation,
    typeRequest,
    procedureRequest,
    startRequestActivityProcess,
    startPostCreditBasicData,
    startPostCreditProductData,
    creditBasicData,
    creditProductData,
    creditObservationsData,
    startPostCreditObservationsData,
    error,
    responseRequestActivity,
    startGetAllPostInformationBasicSeverance,
    informationBasicSeverance,
    startObtenerVendedorCredito,
    startPostCreditFilesData,
    formularioVendedor,
    creditFilesData,

}: {
    startGetCreditType: () => void;
    startGetActivitiesStatus: () => void;
    resetCreditState: () => void;
    startPostProcedureRequest: (data: ProcedureRequestRequestModel) => void;
    creditTypes: CreditTypeModel[];
    activitiesStatus: ActivityStatusModel[];
    isLoadingCatalog: boolean;
    isLoadingCredit: boolean;
    procedureRequests: ProcedureRequestModel[];
    clearDetailRequestInformation: () => void;

    typeRequest?: string;
    error: any;
    responseRequestActivity: any;
    procedureRequest: ProcedureRequestModel;
    // onItemGoBack: () => void;    
    startRequestActivityProcess: (data: RequestActivityRequestModel, requestType: number,) => void;
    startPostCreditBasicData: (data: CreditBasicDataRequestModel) => void;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    startPostCreditObservationsData: (data: CreditObservationsRequestModel,) => void;
    creditBasicData: CreditBasicDataModel;
    creditProductData: CreditProductModel;
    creditObservationsData: CreditObservationModel[];
    startGetAllPostInformationBasicSeverance: (numeroIdentificacion: number,) => void;
    informationBasicSeverance: CreditBasicDataModel;
    startObtenerVendedorCredito: (simulationID: any) => void;
    startPostCreditFilesData: (data: CreditFilesRequestModel) => void;
    formularioVendedor: any;
    creditFilesData: CreditFileModel[];

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

    console.log({ creditBasicData })

    useEffect(() => {
        document.title = 'Procedimientos Solicitudes | Caja Honor';
        startGetCreditType();
        startGetActivitiesStatus();
    }, []);


    useEffect(() => {
        const listaCredito = creditTypes.filter(x => x.id !== 3)
        setNuevaLista(listaCredito);
    }, [creditTypes])
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
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    } else {
                        setLocalIpAddress(ip);
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
                modulo: 'Consultar Credito educativo',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de crédito educativo con los siguientes filtros:
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
                modulo: 'Consultar Credito educativo',
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

    if (selectedData != null) {
        return (
            <CHPrivateContainer>
                <RequestInformationEducation
                    startPostCreditFilesData={startPostCreditFilesData}
                    startObtenerVendedorCredito={startObtenerVendedorCredito}
                    typeRequest={typeRequest}
                    startRequestActivityProcess={startRequestActivityProcess}
                    startPostCreditBasicData={startPostCreditBasicData}
                    startPostCreditProductData={startPostCreditProductData}
                    creditBasicData={creditBasicData}
                    creditProductData={creditProductData}
                    creditObservationsData={creditObservationsData}
                    startPostCreditObservationsData={startPostCreditObservationsData}
                    error={error}
                    responseRequestActivity={responseRequestActivity}
                    startGetAllPostInformationBasicSeverance={startGetAllPostInformationBasicSeverance}
                    informationBasicSeverance={informationBasicSeverance}
                    isLoadingCredit={isLoadingCredit}
                    procedureRequest={selectedData}
                    onItemGoBack={onItemGoBack}
                    formularioVendedor={formularioVendedor}
                    creditFilesData={creditFilesData}

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
                title2="de Crédito Educativo"
            />
            <CHPaper title="Información Crédito Educativo">
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

            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmit}
                >
                    CONSULTAR
                </Button>
            </div>
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
    isLoadingCredit: state.catalog.loading,
    procedureRequests: state.credit.procedureRequests,
    formularioVendedor: state.credit.formularioVendedor,
    creditFilesData: state.credit.creditFilesData,
});

const mapDispatchToProps = {
    startGetCreditType,
    startGetActivitiesStatus,
    startPostProcedureRequest,
    resetCreditState,
    clearDetailRequestInformation,
    startObtenerVendedorCredito,
    startPostCreditFilesData
};

export default connect(mapStateToProps, mapDispatchToProps)(ProcedureEducationRequests);
