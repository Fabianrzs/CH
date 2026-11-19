import React, { useEffect, useState } from 'react';
import './ScheduleNovelties.styles.scss';
import { Button, Grid, MenuItem } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { GetApp } from '@material-ui/icons';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { CatalogState } from '../../reducers/catalog.reducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import { AppState } from '../../store/rootReducer';
import { EXECUTINGUNITDTO } from '../../types/catalog.type';
import { startGetAllExecutingUnits, startGetAllForces, startGetAverageCost, startGetFormat } from '../../actions/catalog.action';
import { LoginState } from '../../reducers/login.reducer';
import { startGetExecutingUnitsSchedule, StartEditReportDateExecutingUnitsSchedule, startDeleteExecutingUnitsSchedule } from '../../actions/schedule.action';
import { UnidadEjecutoraCronogramaDTO, UnidadEjecutoraCronogramaEditarDTO } from '../../types/cronograma.type';
import { ScheduleState } from '../../reducers/schedule.reducer';
import UtilService from '../../utils/util.service';
import EditSchedule from './components/editSchedule.component';
import NotificationService from '../../services/notification.service';
import { LoadingComponent } from '../../components/loading/loading.component';
import AddSchedule from './components/Addschedule.component';
import CHPaperNew from '../../components/ch-paper/CHPaperV2';
import CHPageTitle2 from '../../components/ch-page-title/CHPageTitleV2';
import ContainerTitle from '../../components/containe-title/container-title';
import ApiSchedule from '../../services/api-cronograma.service';
import CHSelect from '../../components/ch-select/CHSelect';
import CHPaper from '../../components/ch-paper/CHPaper';
import AuditoriaApi from '../../services/api-auditorias.servise'
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import CatalogApi from '../../services/api-catalog.service';

const api = ApiSchedule();
const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            color: '#42b7ef',
            borderColor: "fec917",
            display: 'flex'
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
            color: '#42b7ef'
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '12%',
            marginLeft: '87%',
        },
        processButton2: {
            backgroundColor: '#fec917',
            color: '#1c3f8b',
            width: '20%',
            marginLeft: '1%',
        },
        processButton3: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '15%',
            marginLeft: '85%',
        },
        icon: {
            color: '#42b7ef',
        },

        iconRed: {
            color: '#FF0000',

        },

        title: {
            padding: '-4px',
            textAlign: 'left',
            color: '#1c3f8b',
            borderColor: "fec917",
            fontSize: "1rem"
        },

        icon2: {
            fontSize: '2rem',
            color: '#1c3f8b',
            marginRight: '2px',
            marginTop: '10px',
            textAlign: 'center',
            marginBottom: '-10px',
        },

        marginInput: {

            marginLeft: '5%',
        },
        downloadIcon: {
            display: 'flex',
            justifyContent: 'flex-end',
            alignItems: 'center',
            marginBottom: '10px'
        }

    }),
);

function ScheduleTable({
    data,
    StartEditReportDateExecutingUnitsSchedule,
    scheduleState,
    startDeleteExecutingUnitsSchedule,
    catalogState,
    applyFilter,
}: {
    data: UnidadEjecutoraCronogramaDTO[] | undefined | null;
    StartEditReportDateExecutingUnitsSchedule: (unidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditarDTO) => void;
    scheduleState: ScheduleState;
    startDeleteExecutingUnitsSchedule: (unidadEjecutoraCronogramaDelete: UnidadEjecutoraCronogramaDTO | undefined) => void;
    applyFilter: (data: any) => void;
    catalogState: CatalogState;
}) {

    const [selectedData, setSelectedData] = useState<UnidadEjecutoraCronogramaDTO | undefined>();
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const [isOpenAdd, setIsOpenAdd] = useState<boolean>(false);


    const [cronogramaUnidadesEjecutorasEliminar, setCronogramaUnidadesEjecutorasEliminar] = React.useState(null);
    useEffect(() => {
        async function confirmAlertDelete(cronogramaUnidadesEjecutorasEliminar: any) {
            if (cronogramaUnidadesEjecutorasEliminar != null) {
                const confirmAlert = await NotificationService.confirmAlert(
                    '¿Está seguro que desea eliminar esta novedad?',
                );

                try {
                    if (confirmAlert) {
                        startDeleteExecutingUnitsSchedule(cronogramaUnidadesEjecutorasEliminar);
                    }
                } catch (e) {
                    await NotificationService.genericAlert(
                        'Error en la eliminación',
                        'Ocurrio un error al intentar eliminar, intente de nuevo.',
                        'error',
                    );
                }
            }
        }

        confirmAlertDelete(cronogramaUnidadesEjecutorasEliminar);
    }, [cronogramaUnidadesEjecutorasEliminar]);


    const onClickItem = (unidadEjecutora: UnidadEjecutoraCronogramaDTO | undefined) => (event: React.MouseEvent) => {
        setSelectedData(unidadEjecutora);
        setIsOpen(true);
        event.preventDefault();
    };

    const onClose = (value: UnidadEjecutoraCronogramaDTO) => {
        setIsOpen(false);
        applyFilter(value)
    };

    const onCloseAdd = () => {
        setIsOpenAdd(false);
    };

    const classes = useStyles();

    return (
        <Grid container>
            <CHPaperNew>
                <Grid>
                    <ContainerTitle
                        title="Cronograma de Novedades"
                    />
                </Grid>

                <table className="table">
                    <thead>
                        <tr>
                            <th className='unidadEje'>Unidad Ejecutora</th>
                            <th className='fechaReporte'>Fecha Reporte</th>
                            <th className='fechaInicio'>Fecha Inicio</th>
                            <th className='fechaFin'>Fecha Fin</th>
                            <th className='periodo'>Período</th>
                            <th className='formato'>Formato</th>
                            <th className='medioEnv'>Medio Envío</th>
                            <th className='acciones'>Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data?.filter(x => x.idUnidadEjecutora !== 4).map((row, index) => (
                            <tr key={`table${index}`}>
                                <td className='unidadEje2'>{row.nombreUnidadEjecutora}</td>
                                <td className='fechaReporte2'>{UtilService.getDate(new Date())}</td>
                                <td className='fechaInicio2'>{row.fechaReporte.toString() === "0001-01-01T00:00:00" ? "SIN ASIGNAR" : UtilService.getDate(row.fechaInicio)}</td>
                                <td className='fechaFin2'>{row.fechaReporte.toString() === "0001-01-01T00:00:00" ? "SIN ASIGNAR" : UtilService.getDate(row.fechaFin)}</td>
                                <td className='periodo2'>{row.fechaReporte.toString() === "0001-01-01T00:00:00" ? "SIN ASIGNAR" : `${UtilService.getDate(row.periodo).split('-')[1]}-${UtilService.getDate(row.periodo).split('-')[2]}`}</td>
                                <td className='formato2'>{row.descripcionFormato}</td>
                                <td className='medioEnv2'>{row.descripcionMedioEnvio}</td>
                                <Grid>
                                    <td className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} onClick={onClickItem(row)}  > </td>

                                </Grid>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <EditSchedule
                    isOpen={isOpen}
                    data={selectedData}
                    StartEditReportDateExecutingUnitsSchedule={StartEditReportDateExecutingUnitsSchedule}
                    onClose={onClose}
                    scheduleState={scheduleState}
                    catalogState={catalogState}
                />
                <AddSchedule
                    isOpen={isOpenAdd}
                    onClose={onCloseAdd}
                />
            </CHPaperNew>

        </Grid>
    );
}

function ScheduleNovelties({
    catalogState,
    loginState,
    scheduleState,
    startGetAllExecutingUnits,
    startGetAllForces,
    startGetExecutingUnitsSchedule,
    StartEditReportDateExecutingUnitsSchedule,
    startDeleteExecutingUnitsSchedule,
    startGetAverageCost,
    startGetFormat,
}:
    {
        catalogState: CatalogState;
        loginState: LoginState
        scheduleState: ScheduleState;
        startGetExecutingUnitsSchedule: () => void;
        startGetAllExecutingUnits: (props: string) => void;
        startGetAllForces: (props: string) => void;
        StartEditReportDateExecutingUnitsSchedule: (unidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditarDTO) => void;
        startDeleteExecutingUnitsSchedule: (unidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO | undefined) => void;
        startGetAverageCost: () => void;
        startGetFormat: () => void;
    }): any {
    const classes = useStyles();

    const [unidadesEjecutorasLista, setUnidadesEjecutorasLista] = React.useState<EXECUTINGUNITDTO[] | undefined>(catalogState.ExecutingUnit.sort((a, b) => (a.nombre > b.nombre ? 1 : -1)));
    const [unidadEjecutora, setUnidadEjecutora] = React.useState(0);
    const [cronogramaLista, setCronogramaLista] = React.useState<UnidadEjecutoraCronogramaDTO[] | undefined | null>();
    const [loading, setLoading] = React.useState<boolean>(false);

    useEffect(() => {
        startGetAllExecutingUnits(loginState.claims);
        startGetAllForces(loginState.claims);
        startGetAverageCost();
        startGetFormat();
        getExecutingUnitsSchedule();
    }, [])


    const getExecutingUnitsSchedule = async () => {
        setLoading(true)
        await api.getExecutingUnitsSchedule()
            .then((response => {
                setCronogramaLista(response?.data)
            }))
        setLoading(false)
    }

    const onChangeUnidadEjecutora = (unidadEjecutora: any) => {
        setUnidadEjecutora(unidadEjecutora.target.value)
    }

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
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

    // const aplicarFiltro = async () => {

    //     setLoading(true)
    //     await api.getExecutingUnitsSchedule()
    //         .then((response => {
    //             const list = response?.data?.filter((x: any) => x.idUnidadEjecutora === unidadEjecutora)
    //             setCronogramaLista(list)

    //             const auditoriaApi = AuditoriaApi();

    //             const user = AuthService.getUserInfo();
    //             const nav = window.navigator as any;
    //             const usrAg = nav.userAgentData.brands.find(
    //                 (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    //             )?.brand;

    //             const request = {
    //                 nombreUnidadEjecutora: list?.[0]?.nombreUnidadEjecutora ?? null
    //             };

    //             const requestAuditoria = {
    //                 navegador: usrAg,
    //                 sistemaOperativo: nav.userAgentData.platform,
    //                 nombreUsuario: user.USR_USERNAME,
    //                 usurioId: user.USR_ID,
    //                 ip: publicIpAddress,
    //                 modulo: 'Cronograma de novedades',
    //                 hostname: document.location.host,
    //                 tipoAccion: 'CONSULTAS',
    //                 idTipoAccion: 3,
    //                 descripcion: `Consulta de Unidad Ejecutora con el filtro de:
	// 				- Unidad Ejecutora: ${request.nombreUnidadEjecutora || 'No especificada'}`,
    //             } as Auditoria;

    //             auditoriaApi.InsertAuditoria(requestAuditoria);


    //         }))
    //     setLoading(false)
    // }

    const aplicarFiltro = async () => {
        setLoading(true);
    
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;
    
        const requestAuditoriaBase = {
            navegador: usrAg || 'Desconocido',
            sistemaOperativo: nav.userAgentData?.platform || 'Desconocido',
            nombreUsuario: user?.USR_USERNAME || 'Usuario no identificado',
            usurioId: user?.USR_ID || 0,
            ip: publicIpAddress,
            modulo: 'Cronograma de novedades',
            hostname: document.location.host,
        };
    
        try {
            const response = await api.getExecutingUnitsSchedule();
            const list = response?.data?.filter((x: any) => x.idUnidadEjecutora === unidadEjecutora);
            setCronogramaLista(list);
    
            const request = {
                nombreUnidadEjecutora: list?.[0]?.nombreUnidadEjecutora ?? null
            };
    
            const requestAuditoria = {
                ...requestAuditoriaBase,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de Unidad Ejecutora con el filtro de:
                    - Unidad Ejecutora: ${request.nombreUnidadEjecutora || 'No especificada'}`,
            } as Auditoria;
    
            await auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            console.error("Error en aplicarFiltro:", error);
    
            const descripcionError = `Error al aplicar el filtro de Unidad Ejecutora.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;
    
            const requestAuditoriaError: Auditoria = {
                ...requestAuditoriaBase,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError,
            };
    
            await auditoriaApi.InsertAuditoria(requestAuditoriaError);
    
            alert("Ocurrió un error al aplicar el filtro. Intenta nuevamente o contacta al administrador.");
        } finally {
            setLoading(false);
        }
    };
    

    const quitarFiltro = async () => {
        setLoading(true)
        await api.getExecutingUnitsSchedule()
            .then((response => {
                setCronogramaLista(response?.data)
            }))
        setLoading(false)
        setUnidadEjecutora(0)
    }

    const applyFilter = async (data: any) => {
        setCronogramaLista(cronogramaLista?.map(x => x.id === data.id ? data : x))
    }

    return (
        <CHPrivateContainer>
            <LoadingComponent key="loader" loading={scheduleState.loading || loading} />
            <LoadingComponent key="loader" loading={scheduleState.firstLoading} />
            <CHPageTitle2
                topText="Administración de Novedades"
                topText2="Novedades Altas y Bajas"
                title1="Cronograma"
                title2="de Novedades"
                icono="calendar_month"
            />
            <CHPaper title="Cronograma de novedades">
                <Grid container>
                    <Grid className={classes.marginInput} md={5}>
                        <Grid className='subcontainerModal'>
                            <Grid container>
                                <CHSelect
                                    defaultValue=""
                                    label="Unidad Ejecutora"
                                    onChange={onChangeUnidadEjecutora}
                                    value={unidadEjecutora}

                                >
                                    {unidadesEjecutorasLista?.map(
                                        (option: EXECUTINGUNITDTO) => (
                                            <MenuItem

                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            </Grid>
                        </Grid>
                    </Grid>
                    <Button
                        variant="contained"
                        className={classes.processButton}
                        onClick={() => aplicarFiltro()}
                    >
                        APLICAR FILTRO
                    </Button>


                </Grid>
            </CHPaper>
            <ScheduleTable
                data={cronogramaLista}
                StartEditReportDateExecutingUnitsSchedule={StartEditReportDateExecutingUnitsSchedule}
                scheduleState={scheduleState}
                startDeleteExecutingUnitsSchedule={startDeleteExecutingUnitsSchedule}
                catalogState={catalogState}
                startGetExecutingUnitsSchedule={startGetExecutingUnitsSchedule}
                applyFilter={applyFilter}
            />
            <Button
                className={classes.processButton3}
                onClick={() => quitarFiltro()}
            >
                QUITAR FILTRO
            </Button>
        </CHPrivateContainer>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    scheduleState: state.schedule,
});


const mapDispatchToProps = {
    startGetAllExecutingUnits,
    startGetAllForces,
    startGetExecutingUnitsSchedule,
    StartEditReportDateExecutingUnitsSchedule,
    startDeleteExecutingUnitsSchedule,
    startGetAverageCost,
    startGetFormat,
};

export default connect(mapStateToProps, mapDispatchToProps)(ScheduleNovelties);
