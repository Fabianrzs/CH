import { React, useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    List,
    ListItem,
    ListItemText
} from '@material-ui/core';
import moment from 'moment';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { startGetAllGrades, startGetAllForces, startGetCategory, startGetAllExecutingUnits, startGetAllIdentificationTypes, startGetAllDepartments, startGetAllCities } from '../../../actions/catalog.action';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { CatalogState } from '../../../reducers/catalog.reducer';
import ApiClients from '../../../services/api-clients.service';
import ApiReportes from '../../../services/api-reportes.service';
import { AppState } from '../../../store/rootReducer';
import { CITYDTO, DEPARTAMENTODTO } from '../../../types/catalog.type';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import CatalogApi from '../../../services/api-catalog.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';

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
        button2: {
            width: '100%',
            marginTop: '1.5rem',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 8px',
        },

        buttonNoObl: {
            width: '17rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalSaldos: {
            width: '17rem',
            marginLeft: '31rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalProvision: {
            width: '17rem',
            marginLeft: '1.5rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
    }),
);

const api = ApiClients();
const apiReportes = ApiReportes();

function ConsultaDocumentosAfiliadosTable({
    data,
    watch,
    catalogState
}: {
    data: any;
    watch: any;
    catalogState: CatalogState;
}) {
    return (
        <div>
            {data.length > 0 != null ? (
                <table className="client-table">
                    <thead>
                        <tr>
                            <th>Tipo Identificación</th>
                            <th>Número identificación</th>
                            <th>Nombre</th>
                            <th>Fuerza</th>
                            <th>Unidad Ejecutora</th>
                            <th>Grado</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data?.map((element: any, index: any) => (
                            <tr key={`table-${index}`}>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.IdentificationType.find(x => x.id === element.idTipoIdentificacion)?.descripcion}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element.identificacion}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{`${element.primerNombre} ${element.primerApellido}`}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.Force.find(x => x.id === element.idFuerza)?.descripcion}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.ExecutingUnit.find(x => x.id === element.idUnidadEjecutora)?.nombre}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.Grade.find(x => x.id === element.idGrado)?.descripcion}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : null}
        </div>

    );
}

const ConsultaDatosBasicosAfiliado = ({
    catalogState,
    startGetAllGrades,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetAllIdentificationTypes,
    startGetAllDepartments,
    startGetAllCities
}: {
    catalogState: CatalogState;
    startGetAllGrades: () => void;
    startGetAllForces: () => void;
    startGetCategory: () => void;
    startGetAllExecutingUnits: () => void;
    startGetAllIdentificationTypes: () => void;
    startGetAllDepartments: () => void;
    startGetAllCities: () => void;
}) => {
    const classes = useStyles();

    useEffect(() => {
        startGetAllGrades()
        startGetAllForces()
        startGetCategory()
        startGetAllExecutingUnits()
        startGetAllIdentificationTypes()
        startGetAllDepartments()
        startGetAllCities()
    }, [])

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);

    const { control, handleSubmit, watch, setValue } = useForm<any>({
        defaultValues: {
            idDepartamento: 0,
            idCiudad: 0
        }
    });

    const catalogCity = [...catalogState.City];
    const [departamentoValue, setDepartamentoValue] = useState<DEPARTAMENTODTO | null>(null);
    const [ciudadValues, setCiudadValues] = useState<CITYDTO[]>(catalogCity);


    const [ciudadValue, setCiudadValue] = useState<CITYDTO | null>(null);
    const [nullCity, setNullCity] = useState(false);

    const onChangeDepartment = (
        event: any,
        newValue: DEPARTAMENTODTO | null,
    ) => {
        setCiudadValues(
            catalogCity.filter(x => x.idDepartamento === newValue?.codigo),
        );
        setDepartamentoValue(newValue);
        setNullCity(true);
        setValue('idDepartamento', newValue?.id);
        setValue('idCiudad', 0);
    };

    const onChangeCity = (event: any, newValue: CITYDTO | null) => {
        setNullCity(false);
        setCiudadValue(newValue);
        setValue('idCiudad', newValue?.codigo);
    };

    const [items, setItems] = useState([
        { id: 1, text: 'Información personal', checked: false },
        { id: 2, text: 'Información económica', checked: false },
        { id: 3, text: 'Información de contacto', checked: false },
    ]);

    const handleToggle = (id: any) => {
        setItems(items.map(item =>
            item.id === id ? { ...item, checked: !item.checked } : item
        ));
    };

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

    const getReportInformation = (data: any) => {
        setLoading(true);
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generación de Reportes GA2',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error: any) {
            const descripcionError = `Error al consultar afiliados por datos basicos.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generación de Reportes GA2',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar afiliados', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }

    }

    return (
        <div>
            <CHPaper title="Consulta afiliados">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="idDepartamento"
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            control={control}
                            defaultValue=""
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={departamentoValue}
                                    options={catalogState?.departments}
                                    getOptionLabel={option => option.nombre}
                                    onChange={onChangeDepartment}
                                    renderInput={params => (
                                        <CHTextField
                                            {...params}
                                            label="Departamento"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                    )}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="idCiudad"
                            control={control}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={
                                        nullCity ? null : ciudadValue
                                    }
                                    options={ciudadValues}
                                    getOptionLabel={option => option.nombre}
                                    onChange={onChangeCity}
                                    renderInput={params => (
                                        <CHTextField
                                            {...params}
                                            label="Ciudad"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                    )}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <List>
                            {items.map(item => (
                                <ListItem key={item.id} button onClick={() => handleToggle(item.id)}>
                                    <CHCheckbox
                                        checked={item.checked}
                                        onChange={() => handleToggle(item.id)}
                                        label=""
                                    />
                                    <ListItemText primary={item.text} />
                                </ListItem>
                            ))}
                        </List>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={() => getReportInformation}
                        >
                            CONSULTAR
                        </Button>
                    </Grid>
                    {/* <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={GenerarExcel}
                        >
                            GENERAR EXCEL 
                        </Button>
                    </Grid> */}
                </Grid>
            </CHPaper>
            <ConsultaDocumentosAfiliadosTable
                data={data}
                watch={watch}
                catalogState={catalogState}
            />
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
});

const mapDispatchToProps = {
    startGetAllGrades,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetAllIdentificationTypes,
    startGetAllDepartments,
    startGetAllCities,
};

export default connect(mapStateToProps, mapDispatchToProps)(ConsultaDatosBasicosAfiliado);
