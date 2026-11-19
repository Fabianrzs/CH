import React, { useEffect, useState } from 'react';
import { Button, Grid, MenuItem, Table, TableBody, TableCell, TableHead, TableRow } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaperNew from '../../components/ch-paper/CHPaperV2';
import CHSelect from '../../components/ch-select/CHSelect';

import { CatalogState } from '../../reducers/catalog.reducer';
import { AppState } from '../../store/rootReducer';
import { startGetAllIdentificationTypes, startGetTypeAffiliate } from '../../actions/catalog.action';
import { LoginState } from '../../reducers/login.reducer';
import { IDENTIFICATIONTYPEDTO, TIPOAFILIACIONDTO } from '../../types/catalog.type';
import { AfiliacionesExtraordinariasDTO, ConsultaAfiliacionesRequestDTO, ConsultaAfiliacionRequestDTOInitial } from '../../types/typeAffiliate.type';
import { AffiliateExtraordinaryState } from '../../reducers/affiliateExtraordinary.reducer';
import { startDeleteAffiliate, startGetAffiliateExtraordinaryDetail, startGetExtraordinaryAffiliate } from '../../actions/affiliatesExtraordinary.action';
import history from '../../utils/history.router';
import { LoadingComponent } from '../../components/loading/loading.component';
import ContainerTitle from '../../components/containe-title/container-title';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import NotificationService from '../../services/notification.service';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../components/ch-text-field/CHTextField';
import AuditoriaApi from '../../services/api-auditorias.servise'
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import CatalogApi from '../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '50px',
            textAlign: 'left',
            color: '#42b7ef',
            borderColor: "fec917"
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        icon: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#42b7ef',
        },
        title: {
            textAlign: 'left',
            color: '#42b7ef',
            borderColor: "fec917",
            fontSize: "1rem",
            margin: '10px',
            marginTop: '-10px',
        },
        processButton: {
            backgroundColor: '#42b7ef',
            color: '#fff',
            width: '12%',
            marginLeft: '66%',
        },
        icon2: {
            fontSize: '2rem',
            color: '#42b7ef',
            marginRight: '2px',
            marginTop: '10px',
            textAlign: 'center',
            marginBottom: '-10px',
        },
        marginInput: {
            margin: '2%',
            marginLeft: '5%',
        },
        button3: {
            backgroundColor: '#fec917',
            color: '#1c3f8b',
            width: '15%',
            marginLeft: '1%',
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
        },
    }),
);

function affiliatesExtraordinary({
    catalogState,
    startGetAllIdentificationTypes,
    startGetTypeAffiliate,
    loginState,
    affiliateExtraordinaryState,
    startGetExtraordinaryAffiliate,
    startGetAffiliateExtraordinaryDetail,
    startDeleteAffiliate,
}: {
    catalogState: CatalogState;
    startGetAllIdentificationTypes: (props: string) => void;
    startGetTypeAffiliate: () => void;
    loginState: LoginState;
    affiliateExtraordinaryState: AffiliateExtraordinaryState;
    startGetExtraordinaryAffiliate: (data: ConsultaAfiliacionesRequestDTO) => void;
    startGetAffiliateExtraordinaryDetail: (data: ConsultaAfiliacionesRequestDTO) => void;
    startDeleteAffiliate: (data: AfiliacionesExtraordinariasDTO) => void;
}): any {
    useEffect(() => {
        startGetTypeAffiliate()
        startGetAllIdentificationTypes(loginState.claims)
    }, [])
    const classes = useStyles();
    const today = new Date();
    const [isDataLoaded, setIsDataLoaded] = useState(false);

    const { control, handleSubmit } = useForm<ConsultaAfiliacionesRequestDTO>({
        defaultValues: ConsultaAfiliacionRequestDTOInitial
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

    const submitConsultaAfiliaciones = (data: ConsultaAfiliacionesRequestDTO) => {
        const auditoriaApi = AuditoriaApi();
        startGetExtraordinaryAffiliate(data)
        setIsDataLoaded(true);

        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        const request ={

            // tipoDocumento: data.tipoDocumento,
            numeroDocumento: data.numeroDocumento,
            tipoafiliacion: data.tipoafiliacion,
            numeroRadicado: data.numeroRadicado,
            fechaRadicado: data.fechaRadicado,
            fechaElaboracion: data.fechaElaboracion,
            nombresYApellidos: data.nombresYApellidos
        }
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Afiliaciones Extraordinarias',
            hostname: document.location.host,
            tipoAccion: 'CONSULTAS',
            idTipoAccion: 3,
            descripcion: `Consulta de afiliaciones extraordinarias con los siguientes filtros:
            - Número de documento: ${request.numeroDocumento || 'No especificada'}
            - Tipo de afiliación: ${request.tipoafiliacion || 'No especificado'}
            - Número radicado: ${request.numeroRadicado || 'No especificado'}
            - Fecha de radicado: ${request.fechaRadicado || 'No especificado'}
            - Fecha de elaboración: ${request.fechaElaboracion || 'No especificado'}
            - Nombres y apellidos: ${request.nombresYApellidos || 'No especificado'}`,
        } as Auditoria;

        auditoriaApi.InsertAuditoria(requestAuditoria);
    }

    return (
        <CHPrivateContainer>
            <PageIndicators
                topTextNivel1='Gestión Afiliados'
                topTextNivel2='Afiliaciones Extraordinarias' />

            <IconTitle
                title='Afiliaciones'
                subTitle='Extraordinarias'
                icon='fch-Gestion-Afiliados-0-1'
            />
            <LoadingComponent key="loader" loading={affiliateExtraordinaryState.loading} />
            <CHPaperNew>
                <Grid container>
                    <Grid className={`item-containerModal ${classes.marginInput}`} md={3}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroRadicado"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        label="Número Radicado"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className={`item-containerModal ${classes.marginInput}`} md={3}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="fechaRadicado"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        label="Fecha Radicado"
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={moment().format('DD/MM/YYYY')}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className={`item-containerModal ${classes.marginInput}`} md={3}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="fechaElaboracion"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        label="Fecha de Elaboración"
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={moment().format('DD/MM/YYYY')}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className={`item-containerModal ${classes.marginInput}`} md={3} >
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="nombresYApellidos"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        label="Nombres y apellidos"
                                        value={value}
                                        onChange={e => {
                                            const newValue = e.target.value.replace(/[^a-zA-Z ]/g, '');
                                            onChange(newValue);
                                        }}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className={`item-containerModal ${classes.marginInput}`} md={3} >
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroDocumento"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        label="Número de Identificación"
                                        value={value}
                                        onChange={e => {
                                            const newValue = e.target.value.replace(/\D/g, '');
                                            onChange(newValue);
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    {/* <Grid className={`item-containerModal ${classes.marginInput}`} md={3}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="tipoafiliacion"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo de Afiliación"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                    >
                                        {catalogState.tipoAfiliacion?.map(
                                            (option: TIPOAFILIACIONDTO) => (
                                                <MenuItem key={option.idTipoAfiliacion} value={option.idTipoAfiliacion}>
                                                    {option.descripcionTipoAfiliacion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid> */}
                    <div className="buttons">
                        <Button
                            className={classes.button}
                            variant="outlined"
                            onClick={handleSubmit(submitConsultaAfiliaciones)}
                        >
                            CONSULTAR
                        </Button>
                        <Button
                            className={classes.button}
                            variant="outlined"
                            onClick={() => history.push('/insertarAfiliacion')}
                        >
                            AGREGAR SOLICITUD
                        </Button>
                    </div>
                </Grid>
            </CHPaperNew>
            <AffiliatesExtraordinaryTable
                data={affiliateExtraordinaryState.afiliacionesExtrardinarias}
                isDataLoaded={isDataLoaded}
                catalogState={catalogState}
                startGetAffiliateExtraordinaryDetail={startGetAffiliateExtraordinaryDetail}
                startDeleteAffiliate={startDeleteAffiliate}
            />
        </CHPrivateContainer >
    );
}

function AffiliatesExtraordinaryTable({
    data,
    isDataLoaded,
    catalogState,
    startGetAffiliateExtraordinaryDetail,
    startDeleteAffiliate,
}: {
    data: AfiliacionesExtraordinariasDTO[] | null;
    isDataLoaded: boolean;
    catalogState: CatalogState;
    startGetAffiliateExtraordinaryDetail: (data: ConsultaAfiliacionesRequestDTO) => void;
    startDeleteAffiliate: (data: AfiliacionesExtraordinariasDTO) => void;
}): any {
    const classes = useStyles();
    const [eliminarAfiliacion, setEliminarAfiliacion] = React.useState({} as AfiliacionesExtraordinariasDTO);


    useEffect(() => {
        async function confirmAlertDelete(eliminarAfiliacion: any) {
            if (eliminarAfiliacion.idAfiliacion != null) {
                const confirmAlert = await NotificationService.confirmAlert(
                    '¿Desea eliminar esta afiliación?',
                );

                try {
                    if (confirmAlert) {
                        startDeleteAffiliate(eliminarAfiliacion);
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

        confirmAlertDelete(eliminarAfiliacion);
    }, [eliminarAfiliacion]);


    if (!isDataLoaded) {
        return null;
    }

    if ((data == null || data.length === 0) && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }
    

    const getAffiliateDetail = (identification: string) => {
        const request: ConsultaAfiliacionesRequestDTO = {
            numeroDocumento: identification,
            tipoafiliacion: 0,
            tipoDocumento: 0,
            numeroRadicado: '',
            fechaRadicado: null,
            fechaElaboracion: null,
            nombresYApellidos: ''
        }
        startGetAffiliateExtraordinaryDetail(request)
    }

    const onClickEliminar = (eliminarAfiliacion: AfiliacionesExtraordinariasDTO) => {
        setEliminarAfiliacion(eliminarAfiliacion)
    }

    return (
        <Grid container>
            <CHPaperNew>
                <Grid>
                    <ContainerTitle title="Afiliaciones Extraordinarias" />
                </Grid>
                <Table
                    className="client-table"
                    style={{
                        margin: '20px',
                        width: '95%',
                    }}
                >
                    <TableHead>
                        <TableRow>
                            <TableCell style={{ textAlign: 'center' }}>Detalle</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>N° de Radicado</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>N° Identificación</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>Nombres y Apellidos</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>Tipo Afiliación</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>Fecha Inicio</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>Fecha Fin</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>Estado</TableCell>
                            <TableCell style={{ textAlign: 'center' }}>Usuario</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data?.map((row, index) => (
                            <TableRow key={`table-schedule${index}`}>
                                <TableCell style={{ textAlign: 'center' }}>
                                    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                        <div style={{ marginRight: '10px' }}>
                                            <span aria-label="column2" onClick={() => getAffiliateDetail(row.cedula)}>
                                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                            </span>
                                        </div>
                                        <div>
                                            <span onClick={() => onClickEliminar(row)}>
                                                <span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} />
                                            </span>
                                        </div>
                                    </div>
                                </TableCell>
                                <TableCell style={{ textAlign: 'center' }}>{row.numeroRadicado}</TableCell>
                                <TableCell style={{ textAlign: 'center' }}>{row.cedula}</TableCell>
                                <TableCell style={{ textAlign: 'center' }}>
                                    {[row.primerNombre, row.segundoNombre, row.primerApellido, row.segundoApellido]
                                        .filter(Boolean)
                                        .join(' ')}
                                </TableCell>
                                <TableCell style={{ textAlign: 'center' }}>
                                    {catalogState.tipoAfiliacion.find(x => x.idTipoAfiliacion === row.idTipoAfiliacionExtraordinaria)?.descripcionTipoAfiliacion}
                                </TableCell>
                                <TableCell style={{ textAlign: 'center' }}>.</TableCell>
                                <TableCell style={{ textAlign: 'center' }}>.</TableCell>
                                <TableCell style={{ textAlign: 'center' }}>{row.estado}</TableCell>
                                <TableCell style={{ textAlign: 'center' }}>.</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CHPaperNew>
        </Grid>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    affiliateExtraordinaryState: state.affiliateExtraordinary,
});

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetTypeAffiliate,
    startGetExtraordinaryAffiliate,
    startGetAffiliateExtraordinaryDetail,
    startDeleteAffiliate
}
export default connect(mapStateToProps, mapDispatchToProps)(affiliatesExtraordinary);
