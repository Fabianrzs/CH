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
import { Controller, useForm } from 'react-hook-form';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import moment from 'moment';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import { AppState } from '../../store/rootReducer';
import {
    clearDetailRequestInformation,
    resetCreditState,
    startPostCreditCommittee,
    startPostCreditCommitteeApprove,
    startPostCreditCommitteeSoli,
    startPostCreaSCnMcontAprobacionCredito,
} from '../../actions/credit.actions';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import { CreditCommitteeModel } from '../../models/credit-committee.model';
import HelperService from '../../utils/helper.service';
import CreditCommitteeView from '../../components/CreditCommittee/CreditCommitteeView/CreditCommitteeView';
import CreditCommitteeRequest from '../../components/CreditCommittee/CreditCommitteeRequest/CreditCommitteeRequest';
import NotificationService from '../../services/notification.service';
import UtilService from '../../utils/util.service';
import CreditApi from '../../services/api-credit.service';
import { LoadingComponent } from '../../components/loading/loading.component';
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
        buttonAdd: {
            width: '200px',
            marginLeft: '1.5rem',
            marginTop: '.5rem',

        },
    }),
);


const api = CreditApi();

function ConsultCreditCommittee({
    isLoading,
    creditCommittee,
    creditCommitteeSol,
    startPostCreditCommittee,
    startPostCreditCommitteeApprove,
    startPostCreaSCnMcontAprobacionCredito,
    startPostCreditCommitteeSoli,
    clearDetailRequestInformation,
    resetCreditState,
    redirectTo,
    approveSuccessfully,
    createdCommittee,
}: {
    isLoading: boolean;
    creditCommittee: CreditCommitteeModel[];
    startPostCreditCommittee: (request: any) => void;
    startPostCreditCommitteeApprove: (request: any) => void;
    startPostCreaSCnMcontAprobacionCredito: (request: any) => void;
    startPostCreditCommitteeSoli: (request: any) => void;
    clearDetailRequestInformation: () => void;
    resetCreditState: () => void;
    redirectTo: string | null;
    approveSuccessfully?: boolean;
    createdCommittee: any;
    creditCommitteeSol: any;
}) {
    const classes = useStyles();
    const [selectedCommittee, setSelectedCommittee] = useState<any>(null);
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [newCommittee, setNewCommittee] = useState(false);
    const [isLoadingFile, setIsLoadingFile] = useState(false);
    const [listaSolicitudes, setListaSolicitudes] = useState<any[]>([]);
    const [mostrarFormularioComite, setMostrarFormularioComite] = useState(false);
    const [mostrarListaSolicitudes, setMostrarListaSolicitudes] = useState(false);
    const [mostrarBotonProcesar, setMostrarBotonProcesar] = useState(false);
    const [approveItem, SetApproveItem] = useState(0);



    if (creditCommittee.length > 0 && !isDataLoaded) {
        resetCreditState();
    }

    const { control, handleSubmit } = useForm({
        defaultValues: {
            committeeDate: null,
            numberCommittee: '',
            state: '',
        },
    });

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

    const handleConsult = (data: any) => {
        setIsDataLoaded(true);
        let request = {
            codigoEstado: data.state,
            fecha: moment(data.committeeDate).format('YYYY/MM/DD'),
            numeroComite: data.numberCommittee,
        };
        let descripcionEstado = 'No especifica';


        if (data.committeeDate == null) {
            request = HelperService.omit(request, ['fecha']);
        }
        if (data.numberCommittee === '') {
            request = HelperService.omit(request, ['numeroComite']);
        }
        if (data.state === '') {
            request = HelperService.omit(request, ['codigoEstado']);
        }


        if (request.codigoEstado === 'A') {
            descripcionEstado = 'Aprobado'
        } else if (request.codigoEstado === 'C') {
            descripcionEstado = 'Creado'
        }

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            startPostCreditCommittee(request);

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta comité crédito',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta comité crédito con los siguientes filtros:
                            - Fecha: ${request.fecha || 'No especificada'}
                            - Número Comité: ${request.numeroComite || 'No especificado'}
                            - Estado: ${descripcionEstado || 'No especificada'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al consultar proyectos.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta comité crédito',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar comité crédito', { type: toast.TYPE.ERROR });
        }


    };



    useEffect(() => {
        obtenerTramitesPendientes();
        if (approveSuccessfully) {
            (async () => {

                const req: any = { idComiteCredito: approveItem, tipoOperacion: 1 }
                await startPostCreaSCnMcontAprobacionCredito(req)
                await swal({
                    title: 'Exitoso!',
                    content: {
                        element: 'div',
                        attributes: {
                            innerHTML: `Se aprobó el comité con éxito.`
                        },
                    },
                    icon: 'success',
                });
            })();
        }
    }, [approveSuccessfully]);

    // useEffect(() => {
    //     
    //     if (creditCommitteeSol[0]) {
    //         // const req: any = {idComiteCredito: creditCommittee[0]?.codigoComite}
    //         const request:any = {
    //             // afl_Id: procedureRequest?.flowActivityID,
    //             idSolicitudCredito:  creditCommitteeSol[0]?.idSolicitud,
    //             tipoIdentificacion:  creditCommitteeSol[0]?.tipo_Identificacion,
    //             numeroDocumentoCliente: creditCommitteeSol[0]?.identificacion,
    //             tipoCredito:creditCommitteeSol[0]?.idTipoCredito,
    //             tipoPago:2
    //         }
    //       startPostCreaSCnMcontAprobacionCredito(request)

    //     }
    // }, [creditCommitteeSol]);







    const onItemGoBack = () => {

        clearDetailRequestInformation();
        setSelectedCommittee(null);
        setNewCommittee(false);
        setMostrarFormularioComite(false);
        setMostrarListaSolicitudes(false);
    };

    const obtenerTramitesPendientes = async () => {
        await api
            .getObtenerTramitesPendientes()
            .then(async response => {
                if (response) {
                    if (response.isSuccess && response.data) {
                        setListaSolicitudes(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    if ((selectedCommittee != null || newCommittee) && redirectTo == null) {
        return (
            <CreditCommitteeView
                onItemGoBack={onItemGoBack}
                creditCommittee={selectedCommittee}
                listaSolicitudes={listaSolicitudes}
                obtenerTramitesPendientes={obtenerTramitesPendientes}
                mostrarFormularioComite={mostrarFormularioComite}
                mostrarListaSolicitudes={mostrarListaSolicitudes}
                setMostrarFormularioComite={setMostrarFormularioComite}
                setMostrarListaSolicitudes={setMostrarListaSolicitudes}
                mostrarBotonProcesar={mostrarBotonProcesar}
                setMostrarBotonProcesar={setMostrarBotonProcesar}
            />
        );
    }

    const handleNewCommittee = () => {
        setNewCommittee(true);
        setMostrarFormularioComite(true);
        setMostrarListaSolicitudes(false);
        setMostrarBotonProcesar(false);
    }
    // if (isLoading || isLoadingFile) {
    //     return <CHLoadingProcess />;
    // }


    const handleApprove = async (item: CreditCommitteeModel) => {

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro que desea aprobar?',
        );
        if (confirmAlert) {

            const request: any = {
                actualizadoPor: item.actualizadoPor,
                codigoComite: item.codigoComite,
                fechaActuliza: item.fechaActualiza,
                user: AuthService.getUserInfo().USR_ID,
            }
            SetApproveItem(item.codigoComite)
            await startPostCreditCommitteeApprove(request);
        }
    };

    const handlePdf = (item: CreditCommitteeModel) => async () => {

        const request = item.codigoComite

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            setIsLoadingFile(true);
            const response = await api.postComitePdf(request);
            const mimeType = response.type;
            const file = new Blob([response], { type: mimeType });
            const fileURL = URL.createObjectURL(file);
            window.open(fileURL, '_blank');
            setIsLoadingFile(false);


            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Comité Crédito',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: 'Descarga de Acta comité'

            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error: any) {
            setIsLoadingFile(false);

            const descripcionError = `Error al generar archivo.
			- Error: ${error?.message || 'Error desconocido'}
			- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
			- Código HTTP: ${error?.response?.status || 'No disponible'}`;

			const requestAuditoriaError: Auditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData?.platform,
				nombreUsuario: user?.USR_USERNAME,
				usurioId: user?.USR_ID,
				ip: publicIpAddress,
				modulo: 'Comité Crédito',
				hostname: document.location.host,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError
			};

			auditoriaApi.InsertAuditoria(requestAuditoriaError);

            await NotificationService.genericAlert(
                'Error',
                'Error al obtener el archivo. Intente nuevamente.',
                'error',
            );

        }
    }

    return (
        <CHPrivateContainer>

            <LoadingComponent key="loader" loading={isLoading || isLoadingFile} />
            <CHPageTitle
                topText="Informacion Credito"
                subText=""
                title1="Consulta Comité"
                title2="Crédito"
            />
            <CHPaper title="Consulta">
                <Grid container>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="committeeDate"
                            control={control}
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
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="numberCommittee"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    label="Número Comité"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="state"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    error={!!error}
                                    label="Estado"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    <MenuItem value="C">Creado</MenuItem>
                                    <MenuItem value="A">Aprobado</MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    < Grid container >
                        <Grid item md={12} xs={12}  >
                            <Button
                                className={classes.buttonAdd}
                                color="primary"

                                variant="outlined"
                                onClick={handleSubmit(handleConsult)}>

                                Consultar
                            </Button>
                            <Button
                                className={classes.buttonAdd}
                                color="primary"
                                variant="outlined"
                                onClick={() => handleNewCommittee()}>

                                Nuevo
                            </Button>



                        </Grid>
                    </Grid>


                </Grid>
            </CHPaper>
            {creditCommittee.length > 0 && (
                <CHPaper title="Resultado">
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Acciones</TableCell>
                                <TableCell>Fecha</TableCell>
                                <TableCell>Numero Comite</TableCell>
                                <TableCell>Lugar</TableCell>
                                <TableCell>Hora Inicio</TableCell>
                                <TableCell>Hora Fin</TableCell>
                                <TableCell>Estado</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {creditCommittee.map(item => (
                                <TableRow key={item.numeroComite}>
                                    <TableCell>
                                        {item.codigoEstado === 'C' &&
                                            <Button
                                                style={{
                                                    marginRight: '1rem',
                                                }}
                                                className={classes.buttonAdd}
                                                variant="contained"
                                                color="primary"
                                                onClick={() => {
                                                    setSelectedCommittee(item);
                                                    setMostrarFormularioComite(true);
                                                    setMostrarListaSolicitudes(true);
                                                }}
                                            >
                                                Modificar
                                            </Button>
                                        }

                                        {item.codigoEstado === 'C' && (
                                            <Button
                                                style={{
                                                    marginRight: '1rem',
                                                }}
                                                className={classes.buttonAdd}
                                                onClick={() => { handleApprove(item) }}
                                                variant="contained"
                                                color="primary"
                                            >
                                                Aprobar
                                            </Button>
                                        )}
                                        {item.codigoEstado !== 'C' && (
                                            <Button
                                                className={classes.buttonAdd}
                                                onClick={handlePdf(item)}
                                                variant="contained"
                                                color="primary"
                                            >
                                                PDF
                                            </Button>
                                        )}

                                    </TableCell>
                                    <TableCell>
    {new Date(item.fecha).toLocaleDateString('es-CO', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
    })}
</TableCell>
                                    <TableCell>{item.numeroComite}</TableCell>
                                    <TableCell>{item.lugar}</TableCell>
                                    <TableCell>{item.horaInicio}</TableCell>
                                    <TableCell>
                                        {item.horaFinalizacion}
                                    </TableCell>
                                    <TableCell>{item.estado}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </CHPaper>
            )}
        </CHPrivateContainer>
    );
}

const mapStateToProps = (state: AppState) => ({
    isLoading: state.credit.loading,
    creditCommittee: state.credit.creditCommitteeList,
    creditCommitteeSol: state.credit.creditCommitteeSoli,
    error: state.credit.error,
    redirectTo: state.credit.redirectTo,
    approveSuccessfully: state.credit.approveCreditCommitteeSuccessfully,
    createdCommittee: state.credit.createdCommittee,


});

const mapDispatchToProps = {
    startPostCreditCommittee,
    startPostCreditCommitteeApprove,
    startPostCreaSCnMcontAprobacionCredito,
    clearDetailRequestInformation,
    resetCreditState,
    startPostCreditCommitteeSoli,


};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ConsultCreditCommittee);
