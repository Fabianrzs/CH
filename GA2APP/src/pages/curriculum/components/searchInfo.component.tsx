import { connect } from 'react-redux';
import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,

} from '@material-ui/core';

import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AppState } from '../../../store/rootReducer';
import '../cv.scss';
import { startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../../actions/client.action';
import { ClientForm } from '../constants/form.mode.type';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { ClientState } from '../../../reducers/client.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { ClienteDto, RestaurarIntegracionDto } from '../../../types/client.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import history from '../../../utils/history.router';
import CHSelect from '../../../components/ch-select/CHSelect';
import { PopupEstadoCliente } from './PopupEstadoCliente';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import AuthService from '../../../services/auth.service';
import CatalogApi from '../../../services/api-catalog.service';
import NotificationService from '../../../services/notification.service';
import ApiClients from '../../../services/api-clients.service';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 8px',
        },
    }),
);

function ClientTable({
    data,
    isDataLoaded,
    onClickItem,
    onClickItemAccount,
}: {
    data: ClienteDto | null;
    isDataLoaded: boolean;
    onClickItem: (data: ClienteDto | null) => void;
    onClickItemAccount: () => void;
}) {
    const classes = useStyles();

    if (!isDataLoaded) {
        return null;
    }

    if (data == null && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }


    return (
        <table className="client-table">
            <thead>
                <tr>
                    <th> </th>
                    <th>Número de Documento</th>
                    <th>Nombre</th>
                    <th>Apellido</th>
                </tr>
            </thead>
            <tbody>
                <tr key="ClientInfo">
                    <td>
                        <Button
                            className={classes.processButton}
                            onClick={() => onClickItem(data)}
                        >
                            Detalle
                        </Button>
                        <Button
                            className={classes.accountButton}
                            onClick={() => onClickItemAccount()}
                        >
                            Cuenta
                        </Button>
                    </td>
                    <td>{isDataLoaded ? data?.identificacion : ''}</td>
                    <td>{isDataLoaded ? `${data?.primerNombre} ${data?.segundoNombre != null ? data?.segundoNombre : ''
                        }` : ''}</td>
                    <td>{`${data?.primerApellido} ${data?.segundoApellido != null
                        ? data?.segundoApellido
                        : ''
                        }`}</td>
                </tr>
            </tbody>
        </table>
    );
}

const SearchInfo = (props: Props) => {
    const classes = useStyles();
    const {
        startGetAllIdentificationTypes,
        startGetClientByIdentificationTypeIdentificationNumber,
        clientState,
        catalogState,
        loginState
    } = props;
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [selectedData, setSelectedData] = useState<ClienteDto | null>(null);
    const [selectedDataAccount, setSelectedDataAccount] = useState(false);

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);

    }, []);

    const { control, handleSubmit, watch } = useForm<ClienteDto>({
        defaultValues: ClientForm,
    });

    const identificationType = watch('idTipoIdentificacion');
    const identification = watch('identificacion');
    const identificacionDesc = catalogState.IdentificationType


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
    // };
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const clientApi = ApiClients();
    const handleRestoreInformation = async () => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro?',
        );
        try {
            if (confirmAlert) {
                setIsLocalLoading(true);
                const data: RestaurarIntegracionDto = {
                    idCliente: 0,
                    idInformacionFinanciera: 0,
                    idDireccion: 0,
                    idTipoIdentificacion: identificationType,
                    numeroIdentificacion: identification,
                };
                const response = await clientApi.restoreIntegration(data);


                await NotificationService.genericAlert(
                    'Éxito',
                    'Usuario migrado correctamente.',
                    'success',
                );
            }
        } catch (e) {
            await NotificationService.genericAlert(
                'Error en la actualización',
                'Ocurrió un error al intentar restaurar, intente de nuevo.',
                'error',
            );
            console.error(e);
        } finally {
            setIsLocalLoading(false);
        }
    };

    if (isLocalLoading) {
        return <CHLoadingProcess />;
    }


    const handleSearchButton = async () => {
        if (identification === '') {
            toast("Los campos de identificación y tipo de identificación son obligatorios");
            return;
        }

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.filter(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )[0]?.brand || 'Navegador desconocido';

        const identificationTypeDesc = catalogState.IdentificationType?.find(
            (option: any) => option.id === identificationType
        )?.descripcion || 'Desconocido';

        try {
            const requestAuditoria: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Consulta de hoja de vida',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta en hoja de vida por tipo y número de identificación: ${identificationTypeDesc}, ${identification}`,
            };

            await auditoriaApi.InsertAuditoria(requestAuditoria);

            await startGetClientByIdentificationTypeIdentificationNumber(
                '',
                identificationType as number,
                identification
            );

            setIsDataLoaded(true);
        } catch (error: any) {
            const descripcionError = `Error en consulta de hoja de vida:
                - Tipo de ID: ${identificationTypeDesc}
                - Número de ID: ${identification}
                - Error: ${error?.message || 'Error desconocido'}
                - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Consulta de hoja de vida',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            await auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
        }
    };



    const onItemIsSelected = (data: ClienteDto | null) => {
        setSelectedData(data);
    };

    const onClickItemAccount = () => {
        setSelectedDataAccount(true);
    };

    if (selectedData != null) {
        history.push('/InformacionCliente');
    }

    if (selectedDataAccount) {
        history.push('/Account');
    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Gestión de afiliados"
                subText=""
                title1="Hoja de"
                title2="Vida"
            />
            <LoadingComponent key="loader" loading={clientState.loading} />
            <CHPaper title="Consulta afiliados">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}

                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de identificación"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.IdentificationType?.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idTipoIdentificacion"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="identificacion"
                            control={control}
                            rules={{

                                minLength: {
                                    value: 4,
                                    message:
                                        'La longitud mínima del campo es de 4 caracteres',
                                },
                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 caracteres',
                                },
                                pattern: /^[0-9]+$/g
                            }}
                            defaultValue=""
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? "El número de identificación no debe contener letras" : null}
                                    label="Número de Identificación"
                                    value={value}
                                    onChange={onChange}
                                    numberMask
                                />
                            )}
                        />
                        {/* {
                            identificationType === 1 || identificationType === 2 ? (
                                <Controller
                                    name="identificacion"
                                    control={control}
                                    rules={{

                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 12,
                                            message:
                                                'La longitud máxima del campo es de 12 caracteres',
                                        },
                                        pattern: /^[0-9]+$/g
                                    }}
                                    defaultValue=""
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? "El número de identificación no debe contener letras" : null}
                                            label="Número de Identificación"
                                            value={value}
                                            onChange={onChange}
                                            type="number"
                                        />
                                    )}
                                />
                            ) : (
                                <Controller
                                    name="identificacion"
                                    control={control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 12,
                                            message:
                                                'La longitud máxima del campo es de 12 caracteres',
                                        },
                                    }}
                                    defaultValue=""
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMask
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            label="Número de Identificación"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            )
                        } */}
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSubmit(handleSearchButton)}
                        >
                            CONSULTAR HOJA DE VIDA
                        </Button>
                        {/* <Button
                            ariant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSubmit(handleRestoreInformation)}
                        >
                            ACTUALIZAR INFORMACIÓN
                        </Button> */}
                    </Grid>
                </Grid>
            </CHPaper>
            <ClientTable
                data={clientState.client}
                onClickItem={onItemIsSelected}
                isDataLoaded={isDataLoaded}
                onClickItemAccount={onClickItemAccount}
            />
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    clientState: ClientState;
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetClientByIdentificationTypeIdentificationNumber,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(SearchInfo);
