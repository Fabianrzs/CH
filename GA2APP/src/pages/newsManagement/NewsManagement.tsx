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
import { useLocation } from 'react-router-dom';
import swal from 'sweetalert';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye } from '@fortawesome/free-solid-svg-icons';
import { AppState } from '../../store/rootReducer';
import './newsManagement.scss';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import { startGetNovedadesAfiliado } from '../../actions/novedades.action';
import { LoadingComponent } from '../../components/loading/loading.component';
import { ClientState } from '../../reducers/client.reducer';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import { ClienteDto } from '../../types/client.model.type';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import history from '../../utils/history.router';
import CHSelect from '../../components/ch-select/CHSelect';
import { ClientForm } from '../curriculum/constants/form.mode.type';
import ApiNovedades from '../../services/api-novedades.service';
import UtilService from '../../utils/util.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import AuthService from '../../services/auth.service';
import ApiSchedule from '../../services/api-cronograma.service';
import CatalogApi from '../../services/api-catalog.service';
import ContainerTitle from '../../components/containe-title/container-title';
import ApiClients from '../../services/api-clients.service';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import AuditoriaApi from '../../services/api-auditorias.servise'
import { Auditoria } from '../../types/Auditoria.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            display: 'flex'
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
            margin: '8px 1px',
            marginLeft: '.5rem',
            width: '100px'
        },
        icon: {
            color: '#42b7ef',
            paddingRight: '10px',
        },
        alineationRight: {
            margin: '0 5rem 0 60rem',
            position: 'relative',
            display: 'flex'
        },
        actions: {
            margin: '8px 1px',
            width: '120px'
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
        },
        transparentButton: {
            background: 'none',
            border: 'none',
            padding: 0,
            cursor: 'pointer',
            '&:hover, &:active': {
                background: 'transparent !important',
            },
        },
        textAlign: {
            textAlign: 'center'
        }


    }),
);




function ClientTable({
    data,
    nombreIdentificaciones,
    datosCliente,
    setData,
}: {
    data: any[];
    nombreIdentificaciones: string;
    datosCliente: any;
    setData: (data: any) => void;

}) {

    const classes = useStyles();
    const api = ApiSchedule();

    const token = '';
    const [sortedData, setSortedData] = useState<Array<any>>([]);


    useEffect(() => {
        const updateSortedData = () => {

            const sortedList: Array<any> = [...data].sort((a, b) => {
                const dateA = new Date(a.fechaInicioNovedad);
                const dateB: Date = new Date(b.fechaInicioNovedad);
                if (dateA.getTime() === dateB.getTime()) {
                    return data.indexOf(a) - data.indexOf(b);
                }
                return dateA.getTime() - dateB.getTime();
            });
            setSortedData(sortedList);
        }
        updateSortedData();

    }, [data]);

    if (data?.length === 0) {
        return <div>No se encontraron datos</div>;
    }

    const handleDetail = async (idNovedad: number) => {
        history.push(`/DetalleNovedad?idNovedad=${idNovedad}`);
    }

    const handleEdit = async (idNovedad: number) => {
        history.push(`/EditarNovedad?idNovedad=${idNovedad}`);
    }


    const guardarLog = async (tipoDocumento: any, nDocumento: any, fechaInicio: any, idUnidadEjecutora: number) => {
        const user = AuthService.getUserInfo();
        await api.InsertarAudoriaNovedades({
            idModulo: 1,
            tipoDocumento: tipoDocumento,
            nDocumento: nDocumento,
            detalleModificacion: `Se ha eliminado una novedad para el afiliado con número de cédula ${nDocumento}`,
            fechaModificacion: new Date(),
            usuarioModificacion: user.USR_ID,
            fechaInicio: fechaInicio,
            fechaFin: new Date(),
            token: token,
            idUnidadEjecutora: idUnidadEjecutora
        });

    };


    const handleDelete = async (idNovedad: number, fechaInicio: string, idUnidadEjecutora: number) => {
        const selectedData = data.find((item) => item.idNovedad === idNovedad);

        if (!selectedData) {
            swal({
                text: 'No se encontró la novedad seleccionada.',
                icon: 'error',
            });
            return;
        }

        const confirmDelete = await swal({
            text: '¿Está seguro que desea eliminar la novedad?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
            dangerMode: true,
        });

        if (!confirmDelete) {
            return;
        }


        try {
            const response = await api.EliminarNovedades(idNovedad);

            if (response.isSuccess) {
                swal({
                    text: 'Novedad eliminada',
                    icon: 'success',
                });

                const {
                    idTipoIdentificacion,
                    identificacion: numeroDocumento,
                } = selectedData;

                const user = AuthService.getUserInfo();
                await guardarLog(idTipoIdentificacion, numeroDocumento, fechaInicio, idUnidadEjecutora);

                setData(data.filter((x) => x.idNovedad !== idNovedad));
            } else {
                swal({
                    text: `Error al eliminar la novedad: ${response.errorMessage}`,
                    icon: 'error',
                });
            }
        } catch (error) {
            console.error('Error al eliminar la novedad:', error);
            swal({
                text: `Error al eliminar la novedad: ${error}`,
                icon: 'error',
            });
        }
    };


    const latestItem = data.reduce((latest, current) => {
        const latestStartDate = new Date(latest?.fechaInicioNovedad || 0);
        const currentStartDate = new Date(current.fechaInicioNovedad);
        return currentStartDate > latestStartDate ? current : latest;
    }, null);



    return (
        <Grid container>
            <CHPaperNew>
                <Grid>
                    <ContainerTitle title="Tabla de Novedades" />
                </Grid>
                <table className="table">
                    <thead>
                        <tr>
                            <th className={classes.textAlign}>Tipo de Identificación</th>
                            <th className={classes.textAlign}>Número de Identificación</th>
                            <th className={classes.textAlign}>Nombres y Apellidos</th>
                            <th className={classes.textAlign}>Concepto</th>
                            <th className={classes.textAlign}>Fecha Inicio</th>
                            <th className={classes.textAlign}>Fecha Fin</th>
                            <th className={classes.textAlign}>Porcentaje %</th>
                            <th className={classes.textAlign}>Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        {sortedData?.map((item, index) => (
                            <tr key={item.idNovedad}>
                                <td className={classes.textAlign}>{nombreIdentificaciones}</td>
                                <td className={classes.textAlign}>{item.identificacion}</td>
                                <td className={classes.textAlign}>
                                    {`${datosCliente.primerNombre ?? ''} ${datosCliente.segundoNombre ?? ''} ${datosCliente.primerApellido ?? ''} ${datosCliente.segundoApellido ?? ''}`}
                                </td>
                                <td className={classes.textAlign}>{item.nombreConceptoNovedad}</td>
                                <td className={classes.textAlign}>{UtilService.getDate(item.fechaInicioNovedad)}</td>
                                <td className={classes.textAlign}>{UtilService.getDate(item.fechaFinNovedad)}</td>
                                <td className={classes.textAlign}>{`${item.porcentaje}%`}</td>
                                <td style={{ display: 'flex', alignItems: 'center' }}>
                                    <div style={{ marginRight: '10px' }}>
                                        {item.idNovedad === latestItem.idNovedad ? (
                                            <>
                                                <p>Editar</p>
                                                <span aria-label="column2" onClick={() => handleEdit(item.idNovedad)}>
                                                    <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                                </span>
                                            </>
                                        ) : (
                                            <>
                                                <p>Ver</p>
                                                <span aria-label="column2" onClick={() => handleDetail(item.idNovedad)}>
                                                    <span className={`ico faEye icoMid-3 ${classes.icon}`}>
                                                        <FontAwesomeIcon icon={faEye} />
                                                    </span>
                                                </span>
                                            </>
                                        )}
                                    </div>
                                    {item.idNovedad === latestItem.idNovedad && (
                                        <div>
                                            <p>Eliminar</p>
                                            <button onClick={() => handleDelete(item.idNovedad, item.identificacion, item.fechaInicio, item.idUnidadEjecutora)} className={classes.transparentButton}>
                                                <span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} />
                                            </button>
                                        </div>
                                    )}
                                </td>
                            </tr>

                        ))}
                    </tbody>

                </table>
                <Grid className={classes.column} item xs={12} md={3}>

                    <Button
                        className={classes.button}
                        onClick={() => setData([])}
                    >
                        QUITAR FILTRO
                    </Button>
                </Grid>
            </CHPaperNew>


        </Grid>
    );
}


const NewsManagement = (props: Props) => {

    const useQuery = () =>
        new URLSearchParams(useLocation().search);


    const query = useQuery();
    const identificationTypo = query.get('idTipoIdentificacion');
    const identificationB = query.get('identificacion');


    const classes = useStyles();
    const { startGetAllIdentificationTypes, startGetClientByIdentificationTypeIdentificationNumber, startGetNovedadesAfiliado, clientState, catalogState, loginState } = props;
    const [isLoading, setIsLoading] = useState(false);
    const [selectedData, setSelectedData] = useState<ClienteDto | null>(null);
    const [selectedDataAccount, setSelectedDataAccount] = useState(false);
    const [nombreIdentificaciones, setNombreIdentificaciones] = useState<any>('');

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
    }, []);

    const { control, handleSubmit, watch, setValue, reset } = useForm<ClienteDto>({
        defaultValues: ClientForm,
    });

    const identificationTypoNumber = identificationTypo ? Number(identificationTypo) : 0;
    const identificationBString = identificationB || "";


    useEffect(() => {
        setValue('idTipoIdentificacion', identificationTypoNumber);
        setValue('identificacion', identificationBString);
    }, [identificationTypoNumber, identificationBString, setValue]);


    const [novedades, setNovedades] = useState<any[]>([])
    const [datoPrimario, setDatoPrimario] = useState<any>();
    const [tipoIdentific, setTipoIdentific] = useState<number>();


    const identificationType = watch('idTipoIdentificacion') || tipoIdentific;
    const identification = watch('identificacion') || datoPrimario;
    const api = ApiNovedades();
    const apiClientes = ApiClients();
    const [datosClient, setDatosClient] = useState<any>({});
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

    const nuevaConsultaNovedades = async () => {
        setIsLoading(true);
    
        try {
            if (identificationType !== 0 && identification !== '') {
                const responseA = await apiClientes.getClientByIdentificationTypeIdentificationNumberRequest(
                    identificationType as number,
                    identification
                );
    
                const auditoriaApi = AuditoriaApi();
                const user = AuthService.getUserInfo();
                const nav = window.navigator as any;
                const usrAg = nav.userAgentData.brands.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
    
                const request = {
                    nombreTipoIdentificacion: responseA.data.nombreTipoIdentificacion,
                    identificacion: responseA.data.identificacion
                };
    
                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData.platform,
                    nombreUsuario: user.USR_USERNAME,
                    usurioId: user.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Consulta de novedades',
                    hostname: document.location.host,
                    tipoAccion: 'CONSULTAS',
                    idTipoAccion: 3,
                    descripcion: `Consulta de novedades con los filtros:
                        - Tipo de identificación: ${request.nombreTipoIdentificacion || 'No especificada'}
                        - Identificación: ${request.identificacion || 'No especificada'}`
                } as Auditoria;
    
                auditoriaApi.InsertAuditoria(requestAuditoria);
    
                const { primerNombre, segundoNombre, primerApellido, segundoApellido } = responseA.data;
                const datosCliente = {
                    primerNombre,
                    segundoNombre,
                    primerApellido,
                    segundoApellido
                };
                setDatosClient(datosCliente);
    
                if (responseA.data == null) {
                    toast('Afiliado no encontrado');
                    setNovedades([]);
                    return;
                }
    
                const response = await api.getObtenerNovedadesAfiliado(
                    identificationType as number,
                    identification
                );
    
                if (response && response.isSuccess && response.data.length > 0) {
                    const sortedNovedades = response.data.sort((a, b) => {
                        const dateComparison = new Date(b.fechaInicioNovedad).getTime() - new Date(a.fechaInicioNovedad).getTime();
                        return dateComparison !== 0 ? dateComparison : b.idNovedad - a.idNovedad;
                    });
    
                    setNovedades(sortedNovedades);
                    setTipoIdentific(identificationType);
                    setDatoPrimario(identification);
    
                    const nombreIdentificacion = catalogState.IdentificationType.find(
                        x => x.id === response.data[0].idTipoIdentificacion
                    );
                    setNombreIdentificaciones(nombreIdentificacion?.descripcion);
    
                    toast.success("Afiliado presenta novedades", { autoClose: 5000 });
                } else {
                    toast("Afiliado no presenta novedades", { type: toast.TYPE.ERROR, autoClose: 5000 });
                    setNovedades([]);
                }
            } else {
                toast("Tipo de identificación e identificación son requeridos", { type: toast.TYPE.ERROR, autoClose: 5000 });
                setNovedades([]);
            }
        } catch (error: any) {
            toast("Error al procesar la consulta", { type: toast.TYPE.ERROR, autoClose: 5000 });
            setNovedades([]);
    
            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData?.brands?.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;
    
            const descripcionError = `Error en consulta de novedades.
                - Error: ${error?.message || 'Error desconocido'}
                - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                - Código HTTP: ${error?.response?.status || 'No disponible'}`;
    
            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de novedades',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };
    
            auditoriaApi.InsertAuditoria(requestAuditoriaError);
        } finally {
            setIsLoading(false);
        }
    };
    

    const handleSearchButton = async () => {
        await nuevaConsultaNovedades();
    };

    useEffect(() => {
        const fetchData = async () => {
            setIsLoading(true);

            if (clientState && clientState.client) {
                try {
                    const response = await api.getObtenerNovedadesAfiliado(
                        clientState.client.idTipoIdentificacion as number,
                        clientState.client.identificacion
                    );

                    if (response) {
                        setIsLoading(false);
                        if (response.isSuccess) {

                            const sortedList = response.data.sort((a: any, b: any) => {
                                const dateComparison = new Date(b.fechaInicioNovedad).getTime() - new Date(a.fechaInicioNovedad).getTime();

                                return dateComparison !== 0 ? dateComparison : b.idNovedad - a.idNovedad;
                            });

                            setNovedades(sortedList);

                            const nombreIdentificacion = catalogState.IdentificationType.find(
                                x => x.id === sortedList[0].idTipoIdentificacion
                            );
                            setNombreIdentificaciones(nombreIdentificacion?.descripcion);
                        }
                    }
                } catch (error) {
                    setIsLoading(false);
                }
            }
        };

        const edit = window.location.href.includes('Gestion?edit=true');
        if (edit) {
            fetchData();
        }
    }, [clientState]);


    const onClickAddnew = () => {
        history.push({
            pathname: '/AgregarNovedad',
            state: {
                datoPrimario: datoPrimario,
                tipoIdentific: tipoIdentific,
                tipoIdentificacion: identificationType,
                numeroIdentificacion: identification,
            }
        });
    };


    if (selectedData != null) {
        history.push('/InformacionCliente');
    }

    if (selectedDataAccount) {
        history.push('/Account');
    }

    if (isLoading) {
        return <CHLoadingProcess />;
    }

    return (

        <CHPrivateContainer>
            <CHPageTitle
                title1="Gestión de"
                title2="Novedades"
                topText="Gestión de Novedades"
                subText=""
            />
            <LoadingComponent key="loader" loading={clientState.loading} />
            <CHPaper title="Gestión de novedades">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            name="idTipoIdentificacion"
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de identificación*"
                                    value={value || identificationTypo}
                                    helperText={error ? error.message : null}
                                    onChange={(e: any) => {
                                        onChange(e);
                                    }}
                                >
                                    {catalogState.IdentificationType?.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}

                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        {
                            identificationType === 1 || identificationType === 2 ? (
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
                                        pattern: /^[0-9]+$/g
                                    }}

                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? "El número de identificación no debe contener letras y es requerido" : null}
                                            label="Número de Identificación*"
                                            value={value !== undefined && value !== null ? value : identificationB}
                                            onChange={(e) => {
                                                onChange(e);
                                            }}
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
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            label="Número de Identificación*"
                                            value={value !== undefined && value !== null ? value : identificationB}
                                            onChange={(e) => {
                                                onChange(e);
                                            }}
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}
                                        />
                                    )}
                                />
                            )
                        }
                    </Grid>
                    <Grid className={classes.column} item xs={8} md={3}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSubmit(handleSearchButton)}
                        >
                            CONSULTAR

                        </Button>
                    </Grid>
                    <Grid className={classes.column} item xs={8} md={3}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color='primary'
                            onClick={onClickAddnew}
                        >
                            Agregar Novedad
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            <ClientTable
                data={novedades}
                nombreIdentificaciones={nombreIdentificaciones}
                clientState={clientState}
                datosCliente={datosClient}
                setData={setNovedades}
                identificationType={identificationType}
                identification={identification}
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
    startGetNovedadesAfiliado: any
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetClientByIdentificationTypeIdentificationNumber,
    startGetNovedadesAfiliado,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(NewsManagement);